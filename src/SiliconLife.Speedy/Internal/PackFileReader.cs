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
/// Responsible for reading data blocks and Directory areas from .spk files. v2 files use dual Header slots,
/// and the slot with valid CRC and larger Sequence is preferred as the active Header when reading.
/// </summary>
internal sealed class PackFileReader : IDisposable
{
    private readonly string _filePath;
    private readonly FileStream _stream;
    private readonly BinaryReader _reader;
    private readonly object _lock = new();

    private PackFileReader(string filePath, FileStream stream)
    {
        _filePath = filePath;
        _stream = stream;
        _reader = new BinaryReader(_stream);
    }

    /// <summary>
    /// Opens the .spk file for reading.
    /// </summary>
    public static PackFileReader Open(string filePath)
    {
        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        return new PackFileReader(filePath, stream);
    }

    /// <summary>
    /// Selects the current active Header. Prefers the slot with valid CRC and larger Sequence between the two.
    /// If both slots are invalid, returns null (caller uses this to determine if legacy / error handling is needed).
    /// </summary>
    public (SpkHeader Header, int Slot)? TryReadActiveHeader()
    {
        lock (_lock)
        {
            var a = SpkHeader.TryReadSlot(_stream, 0);
            var b = SpkHeader.TryReadSlot(_stream, 1);

            if (a != null && b != null)
                return a.Sequence >= b.Sequence ? (a, 0) : (b, 1);
            if (a != null) return (a, 0);
            if (b != null) return (b, 1);
            return null;
        }
    }

    /// <summary>
    /// Reads the Directory area pointed to by a Header and deserializes it.
    /// </summary>
    public Dictionary<string, DirectoryEntry> LoadDirectory(SpkHeader header)
    {
        lock (_lock)
        {
            if (header.DirectoryLength == 0)
                return new Dictionary<string, DirectoryEntry>(StringComparer.Ordinal);

            _stream.Position = header.DirectoryOffset;
            var dirBytes = _reader.ReadBytes(header.DirectoryLength);

            var map = MessagePackSerializer.Deserialize<Dictionary<string, DirectoryEntry>>(dirBytes)
                      ?? new Dictionary<string, DirectoryEntry>(StringComparer.Ordinal);

            // Ensure the returned dictionary uses Ordinal comparer to avoid case sensitivity differences.
            if (!ReferenceEquals(map.Comparer, StringComparer.Ordinal))
            {
                var reCompared = new Dictionary<string, DirectoryEntry>(StringComparer.Ordinal);
                foreach (var (k, v) in map) reCompared[k] = v;
                return reCompared;
            }
            return map;
        }
    }

    /// <summary>
    /// Simplified interface for external queries of the current active Header. Throws when no valid slot is found.
    /// </summary>
    public SpkHeader ReadHeader()
    {
        var active = TryReadActiveHeader()
            ?? throw new InvalidDataException("Neither header slot is valid.");
        return active.Header;
    }

    /// <summary>
    /// Reads raw data at the specified offset and length.
    /// </summary>
    public byte[] ReadAt(long offset, int length)
    {
        lock (_lock)
        {
            _stream.Position = offset;
            return _reader.ReadBytes(length);
        }
    }

    public void Dispose()
    {
        _reader?.Dispose();
        _stream?.Dispose();
    }
}
