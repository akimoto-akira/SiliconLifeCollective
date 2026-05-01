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
/// Handles reading data from .spk files.
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
    /// Opens a .spk file for reading.
    /// </summary>
    public static PackFileReader Open(string filePath)
    {
        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        return new PackFileReader(filePath, stream);
    }

    /// <summary>
    /// Reads the file header.
    /// </summary>
    public SpkHeader ReadHeader()
    {
        lock (_lock)
        {
            _stream.Position = 0;
            return SpkHeader.ReadFrom(_reader);
        }
    }

    /// <summary>
    /// Reads a data block at the specified offset and length.
    /// </summary>
    public byte[] ReadAt(long offset, int length)
    {
        lock (_lock)
        {
            _stream.Position = offset;
            return _reader.ReadBytes(length);
        }
    }

    /// <summary>
    /// Loads the directory index from the file.
    /// </summary>
    public Dictionary<string, DirectoryEntry> LoadDirectory()
    {
        lock (_lock)
        {
            var header = ReadHeader();
            
            if (header.DirectoryLength == 0)
                return new Dictionary<string, DirectoryEntry>(StringComparer.Ordinal);

            _stream.Position = header.DirectoryOffset;
            var dirBytes = _reader.ReadBytes(header.DirectoryLength);

            return MessagePackSerializer.Deserialize<Dictionary<string, DirectoryEntry>>(dirBytes)
                   ?? new Dictionary<string, DirectoryEntry>(StringComparer.Ordinal);
        }
    }

    public void Dispose()
    {
        _reader?.Dispose();
        _stream?.Dispose();
    }
}
