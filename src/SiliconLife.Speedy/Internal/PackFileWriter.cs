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
/// Handles writing data to .spk files with 4K alignment and pre-allocation.
/// </summary>
/// <remarks>
/// Layout: [Header(32B, padded to 4K)] [Data blocks, 4K aligned] [Directory region (4K aligned), grows by rewriting past the old one].
/// Every <see cref="AppendEntry"/> places new data past both the existing
/// data frontier and the current directory region, so directories are never
/// overwritten. Old directory regions become fragmentation removed by Compact.
/// </remarks>
internal sealed class PackFileWriter : IDisposable
{
    private const int AlignmentSize = 4096; // 4K alignment

    private readonly string _filePath;
    private readonly FileStream _stream;
    private readonly BinaryWriter _writer;
    private readonly object _lock = new();

    /// <summary>Smallest aligned position where new data may be written.</summary>
    private long _dataFrontier;
    private SpkHeader _header;

    private PackFileWriter(string filePath, FileStream stream, SpkHeader header, long initialFrontier)
    {
        _filePath = filePath;
        _stream = stream;
        _writer = new BinaryWriter(_stream);
        _header = header;
        _dataFrontier = initialFrontier;
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

        // Frontier starts at first 4K boundary after the header region.
        return new PackFileWriter(filePath, stream, header, initialFrontier: AlignmentSize);
    }

    /// <summary>
    /// Opens an existing .spk file for writing. Computes the safe write frontier
    /// from the on-disk directory so appends never overwrite existing data.
    /// </summary>
    public static PackFileWriter Open(string filePath)
    {
        var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
        var reader = new BinaryReader(stream);
        stream.Position = 0;
        var header = SpkHeader.ReadFrom(reader);

        var frontier = ComputeFrontier(stream, header);
        return new PackFileWriter(filePath, stream, header, frontier);
    }

    /// <summary>
    /// Computes the smallest 4K-aligned offset that lies past all existing
    /// data blocks AND past the existing directory region.
    /// </summary>
    private static long ComputeFrontier(FileStream stream, SpkHeader header)
    {
        long maxDataEnd = AlignmentSize; // past the 4K header region

        if (header.DirectoryLength > 0 && header.DirectoryOffset >= AlignmentSize)
        {
            try
            {
                stream.Position = header.DirectoryOffset;
                using var reader = new BinaryReader(stream, System.Text.Encoding.UTF8, leaveOpen: true);
                var dirBytes = reader.ReadBytes(header.DirectoryLength);
                var entries = MessagePackSerializer.Deserialize<Dictionary<string, DirectoryEntry>>(dirBytes);
                if (entries != null)
                {
                    foreach (var entry in entries.Values)
                    {
                        var end = entry.Offset + entry.Length;
                        if (end > maxDataEnd) maxDataEnd = end;
                    }
                }
            }
            catch
            {
                // Corrupted directory — fall back to past-header frontier.
            }
        }

        // Also skip past the current directory region itself.
        var dirEnd = header.DirectoryOffset + header.DirectoryLength;
        if (dirEnd > maxDataEnd) maxDataEnd = dirEnd;

        return AlignToBoundary(maxDataEnd);
    }

    /// <summary>
    /// Appends a data entry to the file at the current 4K-aligned frontier.
    /// Never overwrites existing data or the current directory region.
    /// </summary>
    public DirectoryEntry AppendEntry(string normalizedPath, byte[] data, string contentType, DateTime? createdAt = null)
    {
        lock (_lock)
        {
            // Ensure frontier is past the current directory region as well.
            var dirEnd = _header.DirectoryOffset + _header.DirectoryLength;
            if (dirEnd > _dataFrontier) _dataFrontier = dirEnd;

            var alignedOffset = AlignToBoundary(_dataFrontier);

            // Pre-allocate in 4K increments (with one extra chunk as headroom).
            var neededSize = alignedOffset + data.Length + AlignmentSize;
            if (neededSize > _stream.Length)
                PreAllocateFile(neededSize);

            _stream.Position = alignedOffset;
            _writer.Write(data);
            _writer.Flush();

            _dataFrontier = alignedOffset + data.Length;

            var now = DateTime.UtcNow;
            return new DirectoryEntry
            {
                Offset = alignedOffset,
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
    /// Writes the directory region past the data frontier (4K aligned) and
    /// updates the header accordingly. Old directory region becomes fragmentation.
    /// </summary>
    public void WriteDirectory(IReadOnlyDictionary<string, DirectoryEntry> entries)
    {
        lock (_lock)
        {
            var dirBytes = MessagePackSerializer.Serialize(entries);
            var alignedDirOffset = AlignToBoundary(_dataFrontier);
            var neededSize = alignedDirOffset + dirBytes.Length + AlignmentSize;
            if (neededSize > _stream.Length)
                PreAllocateFile(neededSize);

            _stream.Position = alignedDirOffset;
            _writer.Write(dirBytes);
            _writer.Flush();

            _header.DirectoryOffset = alignedDirOffset;
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

    /// <summary>
    /// Pre-allocates file space rounded up to a 4K boundary.
    /// Avoids frequent small expansions on modern file systems.
    /// </summary>
    private void PreAllocateFile(long targetSize)
    {
        var alignedSize = AlignToBoundary(targetSize);
        _stream.SetLength(alignedSize);
    }

    /// <summary>
    /// Rounds up to the nearest 4K boundary (returns 0 when value is 0).
    /// </summary>
    private static long AlignToBoundary(long value)
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
