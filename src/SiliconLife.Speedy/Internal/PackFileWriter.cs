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
/// .spk file writer, providing a 4K-aligned FreeList allocator and a safe commit protocol of
/// "data writing + Directory switching + Header dual-buffer atomic commit".
/// </summary>
/// <remarks>
/// File layout (v2):
///   [0..4K)   HeaderSlotA
///   [4K..8K)  HeaderSlotB
///   [8K..)    Data blocks + Directory (allocated via FreeList, 4K-aligned)
///
/// Commit protocol (atomicity key):
///   1. New data blocks are always allocated to new locations (COW), never overwriting old data;
///   2. New Directory is also allocated to a new location;
///   3. New Header is written to the "opposite" idle slot, and the active slot is switched after fsync;
///   4. At any step crash, incomplete writes are completely invisible to the old active slot.
/// </remarks>
internal sealed class PackFileWriter : IDisposable
{
    public const int AlignmentSize = 4096;

    private readonly string _filePath;
    private readonly FileStream _stream;
    private readonly BinaryWriter _writer;
    private readonly object _lock = new();
    private readonly FreeList _freeList = new();

    private SpkHeader _header;
    private int _activeSlot; // 当前生效的 Header 槽位（0 或 1）

    private PackFileWriter(string filePath, FileStream stream, SpkHeader header, int activeSlot)
    {
        _filePath = filePath;
        _stream = stream;
        _writer = new BinaryWriter(_stream);
        _header = header;
        _activeSlot = activeSlot;
    }

    /// <summary>
    /// Creates a new v2 .spk file: initializes dual Header slots (slot A valid, slot B empty),
    /// file length exactly equals TotalHeaderSize=8K.
    /// </summary>
    public static PackFileWriter Create(string filePath)
    {
        var header = SpkHeader.CreateNew();
        var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);

        // First expand the file to 8K, clear the entire block, then write valid slot A.
        stream.SetLength(SpkHeader.TotalHeaderSize);
        var zeroSlot = new byte[SpkHeader.SlotSize];
        stream.Position = 0;
        stream.Write(zeroSlot, 0, SpkHeader.SlotSize);
        stream.Write(zeroSlot, 0, SpkHeader.SlotSize);

        header.DirectoryOffset = SpkHeader.TotalHeaderSize;
        header.DirectoryLength = 0;
        header.Sequence = 1;
        header.WriteToSlot(stream, 0);

        stream.Flush();
        stream.Flush(flushToDisk: true);

        return new PackFileWriter(filePath, stream, header, activeSlot: 0);
    }

    /// <summary>
    /// Opens the file for writing with the given "active Header + active slot + loaded Directory",
    /// and rebuilds the FreeList accordingly.
    /// </summary>
    public static PackFileWriter Open(
        string filePath,
        IReadOnlyDictionary<string, DirectoryEntry> directory,
        SpkHeader activeHeader,
        int activeSlot)
    {
        var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
        // Ensure the file is at least as long as the dual Header area.
        if (stream.Length < SpkHeader.TotalHeaderSize)
            stream.SetLength(SpkHeader.TotalHeaderSize);

        var writer = new PackFileWriter(filePath, stream, activeHeader, activeSlot);
        writer.BuildFreeList(directory, stream.Length);
        return writer;
    }

    /// <summary>
    /// Rebuilds FreeList based on "active Directory + active Header Directory area + file length".
    /// The dual Header area (first 8K) is treated as occupied; any gaps between live areas are treated as reusable space.
    /// </summary>
    private void BuildFreeList(IReadOnlyDictionary<string, DirectoryEntry> directory, long fileLength)
    {
        _freeList.Clear();

        var occupied = new List<(long Offset, long Length)>
        {
            // Dual Header slot area, permanently occupied.
            (0L, (long)SpkHeader.TotalHeaderSize)
        };

        foreach (var entry in directory.Values)
        {
            if (entry.Length > 0 && entry.Offset >= SpkHeader.TotalHeaderSize)
                occupied.Add((entry.Offset, AlignUp(entry.Length)));
        }

        if (_header.DirectoryLength > 0 && _header.DirectoryOffset >= SpkHeader.TotalHeaderSize)
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

        if (fileLength > cursor)
            _freeList.Release(cursor, fileLength - cursor);
    }

    /// <summary>
    /// Appends (or reuses a free block) a data entry, returning the new DirectoryEntry.
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

    /// <summary>Rewrites the entry to a new location while preserving the original CreatedAt.</summary>
    public DirectoryEntry AppendEntryUpdate(string normalizedPath, byte[] data, string contentType, DirectoryEntry oldEntry)
    {
        return AppendEntry(normalizedPath, data, contentType, oldEntry.CreatedAt);
    }

    /// <summary>Returns the aligned space occupied by the old entry to the FreeList.</summary>
    public void ReleaseEntry(DirectoryEntry entry)
    {
        if (entry is null) return;
        if (entry.Length <= 0 || entry.Offset < SpkHeader.TotalHeaderSize) return;

        lock (_lock)
        {
            _freeList.Release(entry.Offset, AlignUp(entry.Length));
        }
    }

    /// <summary>
    /// Safe commit protocol: first writes the new Directory to a new location and fsyncs, then writes the new Header
    /// to the opposite slot and fsyncs, then switches the active slot, and finally releases the old Directory area
    /// to the FreeList. A crash at any point will not leave the active slot pointing to an incomplete state.
    /// </summary>
    public void WriteDirectoryAndCommit(IReadOnlyDictionary<string, DirectoryEntry> entries)
    {
        lock (_lock)
        {
            var oldDirOffset = _header.DirectoryOffset;
            var oldDirLength = _header.DirectoryLength;

            // 1. Serialize and allocate new Directory area (new location, does not overwrite old Directory).
            var dirBytes = MessagePackSerializer.Serialize(entries);
            var alignedSize = AlignUp(dirBytes.Length);
            var newDirOffset = AllocateSpaceInternal(alignedSize);

            // 2. Write Directory data and fsync: must be flushed to disk before switching Header.
            _stream.Position = newDirOffset;
            _writer.Write(dirBytes);
            _writer.Flush();
            _stream.Flush(flushToDisk: true);

            // 3. Construct new Header and write to opposite slot, then fsync. Sequence strictly increments,
            //    ensuring clear distinction between old and new slots during crash recovery.
            var inactiveSlot = 1 - _activeSlot;
            var newHeader = new SpkHeader
            {
                Magic = SpkHeader.MagicBytes,
                Version = SpkHeader.CurrentVersion,
                Flags = _header.Flags,
                DirectoryOffset = newDirOffset,
                DirectoryLength = dirBytes.Length,
                Sequence = _header.Sequence + 1
            };
            newHeader.WriteToSlot(_stream, inactiveSlot);
            _writer.Flush();
            _stream.Flush(flushToDisk: true);

            // 4. After Header fsync succeeds, the opposite slot officially becomes the active slot.
            _header = newHeader;
            _activeSlot = inactiveSlot;

            // 5. Old Directory area can only be returned to FreeList at this point (if a crash occurs before this,
            //    the old slot still points to it and must remain readable).
            if (oldDirLength > 0 && oldDirOffset >= SpkHeader.TotalHeaderSize)
                _freeList.Release(oldDirOffset, AlignUp(oldDirLength));
        }
    }

    /// <summary>Compatible with old signature; equivalent to WriteDirectoryAndCommit.</summary>
    public void WriteDirectory(IReadOnlyDictionary<string, DirectoryEntry> entries)
        => WriteDirectoryAndCommit(entries);

    /// <summary>Forces all buffered writes to disk.</summary>
    public void Flush()
    {
        lock (_lock)
        {
            _writer.Flush();
            _stream.Flush(flushToDisk: true);
        }
    }

    /// <summary>Current active slot index (for diagnostics and testing).</summary>
    public int ActiveSlot
    {
        get { lock (_lock) return _activeSlot; }
    }

    /// <summary>Sequence of the current active Header (for diagnostics).</summary>
    public long Sequence
    {
        get { lock (_lock) return _header.Sequence; }
    }

    // ─── Internal Allocator ────────────────────────────────────────────────────────

    private long AllocateSpaceInternal(long alignedLength)
    {
        if (alignedLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(alignedLength));

        if (_freeList.TryAllocate(alignedLength, out var offset))
            return offset;

        offset = _stream.Length;
        _stream.SetLength(offset + alignedLength);
        return offset;
    }

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
