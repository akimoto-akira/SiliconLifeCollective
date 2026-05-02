// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using MessagePack;

namespace SiliconLife.Speedy.Internal;

/// <summary>
/// Handles writing data to .spk files with 4K alignment and a FreeList-based
/// block allocator. Overwrites and deletes release their old space back to the
/// FreeList so future writes can reuse it in-place, behaving like a real disk
/// allocator rather than a pure append-only log.
/// </summary>
/// <remarks>
/// Layout: [Header(32B, padded to 4K)] [Data blocks, 4K aligned] [Directory region (4K aligned)].
/// Each entry and each directory region occupies <c>AlignUp(length, 4K)</c> bytes of the
/// file. When released, that aligned space goes back to the <see cref="FreeList"/>.
/// The FreeList is always in memory; on open it is reconstructed from the on-disk
/// directory snapshot plus the current file length.
/// </remarks>
internal sealed class PackFileWriter : IDisposable
{
    private const int AlignmentSize = 4096; // 4K alignment

    private readonly string _filePath;
    private readonly FileStream _stream;
    private readonly BinaryWriter _writer;
    private readonly object _lock = new();
    private readonly FreeList _freeList = new();

    private SpkHeader _header;

    private PackFileWriter(string filePath, FileStream stream, SpkHeader header)
    {
        _filePath = filePath;
        _stream = stream;
        _writer = new BinaryWriter(_stream);
        _header = header;
    }

    /// <summary>
    /// Creates a new .spk file with initial header. File is truncated if exists.
    /// </summary>
    public static PackFileWriter Create(string filePath)
    {
        var header = SpkHeader.CreateNew();
        var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);

        // Reserve first 4K for header (aligned). Directory initially at 4K (empty).
        var writer = new BinaryWriter(stream);
        header.DirectoryOffset = AlignmentSize;
        header.DirectoryLength = 0;
        header.WriteTo(writer);
        writer.Flush();
        stream.SetLength(AlignmentSize);
        stream.Flush(true);

        // FreeList starts empty; future writes will extend the file when needed.
        return new PackFileWriter(filePath, stream, header);
    }

    /// <summary>
    /// Opens an existing .spk file for writing and rebuilds its FreeList from
    /// the supplied directory snapshot plus the current file length. Any gap
    /// between live regions (live entries + current directory + header) becomes
    /// a reusable free block.
    /// </summary>
    public static PackFileWriter Open(string filePath, IReadOnlyDictionary<string, DirectoryEntry> directory)
    {
        var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
        var reader = new BinaryReader(stream);
        stream.Position = 0;
        var header = SpkHeader.ReadFrom(reader);

        var writer = new PackFileWriter(filePath, stream, header);
        writer.BuildFreeList(directory, stream.Length);
        return writer;
    }

    /// <summary>
    /// Rebuilds the in-memory FreeList by walking live occupied regions
    /// (header, directory, every live entry) in offset order; every gap,
    /// plus any trailing pre-allocated space past the last live region, is
    /// added as a reusable free block.
    /// </summary>
    private void BuildFreeList(IReadOnlyDictionary<string, DirectoryEntry> directory, long fileLength)
    {
        _freeList.Clear();

        var occupied = new List<(long Offset, long Length)>
        {
            // The first 4K is reserved for the header region.
            (0L, (long)AlignmentSize)
        };

        foreach (var entry in directory.Values)
        {
            if (entry.Length > 0 && entry.Offset >= AlignmentSize)
                occupied.Add((entry.Offset, AlignUp(entry.Length)));
        }

        if (_header.DirectoryLength > 0 && _header.DirectoryOffset >= AlignmentSize)
            occupied.Add((_header.DirectoryOffset, AlignUp(_header.DirectoryLength)));

        occupied.Sort((a, b) => a.Offset.CompareTo(b.Offset));

        long cursor = 0;
        foreach (var (off, len) in occupied)
        {
            if (off > cursor)
                _freeList.Release(cursor, off - cursor);
            var end = off + len;
            if (end > cursor) cursor = end;
        }

        // Trailing pre-allocated space past the last live region is reusable too.
        if (fileLength > cursor)
            _freeList.Release(cursor, fileLength - cursor);
    }

    /// <summary>
    /// Appends (or in-place reuses) a data entry. The allocator tries the
    /// FreeList first and only extends the file when no suitable free block
    /// exists, so repeated overwrites of the same key no longer bloat the file.
    /// </summary>
    public DirectoryEntry AppendEntry(string normalizedPath, byte[] data, string contentType, DateTime? createdAt = null)
    {
        lock (_lock)
        {
            var alignedLength = AlignUp(data.Length);
            var offset = AllocateSpaceInternal(alignedLength);

            _stream.Position = offset;
            _writer.Write(data);
            _writer.Flush();

            var now = DateTime.UtcNow;
            return new DirectoryEntry
            {
                Offset = offset,
                Length = data.Length,
                ContentType = contentType,
                CreatedAt = createdAt ?? now,
                UpdatedAt = now
            };
        }
    }

    /// <summary>
    /// Appends a rewritten entry while preserving the original CreatedAt.
    /// </summary>
    public DirectoryEntry AppendEntryUpdate(string normalizedPath, byte[] data, string contentType, DirectoryEntry oldEntry)
    {
        return AppendEntry(normalizedPath, data, contentType, oldEntry.CreatedAt);
    }

    /// <summary>
    /// Releases the space previously occupied by <paramref name="entry"/> back
    /// to the FreeList so subsequent allocations can reuse it. Callers invoke
    /// this from <see cref="WriteQueue"/> before overwriting or deleting an
    /// entry. The physical length returned is <c>AlignUp(entry.Length)</c>.
    /// </summary>
    public void ReleaseEntry(DirectoryEntry entry)
    {
        if (entry is null) return;
        if (entry.Length <= 0 || entry.Offset < AlignmentSize) return;

        lock (_lock)
        {
            _freeList.Release(entry.Offset, AlignUp(entry.Length));
        }
    }

    /// <summary>
    /// Writes the directory region and updates the header. The new directory
    /// is allocated via the FreeList (falling back to file extension), and the
    /// previous directory region is returned to the FreeList.
    /// </summary>
    public void WriteDirectory(IReadOnlyDictionary<string, DirectoryEntry> entries)
    {
        lock (_lock)
        {
            var oldDirOffset = _header.DirectoryOffset;
            var oldDirLength = _header.DirectoryLength;

            // Release old dir region BEFORE allocating the new one so the new
            // directory can reuse the same spot when it fits.
            if (oldDirLength > 0 && oldDirOffset >= AlignmentSize)
                _freeList.Release(oldDirOffset, AlignUp(oldDirLength));

            var dirBytes = MessagePackSerializer.Serialize(entries);
            var alignedSize = AlignUp(dirBytes.Length);
            var dirOffset = AllocateSpaceInternal(alignedSize);

            _stream.Position = dirOffset;
            _writer.Write(dirBytes);
            _writer.Flush();

            _header.DirectoryOffset = dirOffset;
            _header.DirectoryLength = dirBytes.Length;

            // Rewrite header at position 0.
            _stream.Position = 0;
            _header.WriteTo(_writer);
            _writer.Flush();
        }
    }

    /// <summary>
    /// Forces all buffered writes down to the OS and to the physical device.
    /// </summary>
    public void Flush()
    {
        lock (_lock)
        {
            _writer.Flush();
            _stream.Flush(flushToDisk: true);
        }
    }

    // ─── Internal allocator ───────────────────────────────────────────────────

    /// <summary>
    /// Allocates <paramref name="alignedLength"/> bytes. Prefers a reusable
    /// FreeList block; when none fits, extends the underlying file by exactly
    /// the requested size. Must be called under <see cref="_lock"/>.
    /// </summary>
    private long AllocateSpaceInternal(long alignedLength)
    {
        if (alignedLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(alignedLength));

        if (_freeList.TryAllocate(alignedLength, out var offset))
            return offset;

        // No free block big enough — extend the file.
        offset = _stream.Length;
        _stream.SetLength(offset + alignedLength);
        return offset;
    }

    /// <summary>
    /// Rounds up to the nearest 4K boundary (returns 0 when value is 0).
    /// </summary>
    private static long AlignUp(long value)
    {
        if (value <= 0) return 0;
        return (value + AlignmentSize - 1) & ~((long)AlignmentSize - 1);
    }

    public void Dispose()
    {
        try { _writer?.Flush(); _stream?.Flush(true); } catch { /* ignore */ }
        _writer?.Dispose();
        _stream?.Dispose();
    }
}
