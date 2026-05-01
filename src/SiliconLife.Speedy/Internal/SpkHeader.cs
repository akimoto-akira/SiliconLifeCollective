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
/// 32-byte header at the beginning of every .spk file.
/// </summary>
internal sealed class SpkHeader
{
    public const int Size = 32;
    public static readonly byte[] MagicBytes = { 0x53, 0x50, 0x4B, 0x59 }; // "SPKY"

    public byte[] Magic { get; set; } = MagicBytes;
    public ushort Version { get; set; } = 1;
    public ushort Flags { get; set; } = 0;
    public long DirectoryOffset { get; set; }
    public int DirectoryLength { get; set; }

    public static SpkHeader CreateNew()
    {
        return new SpkHeader
        {
            Magic = MagicBytes,
            Version = 1,
            Flags = 0,
            DirectoryOffset = Size, // Directory starts right after header
            DirectoryLength = 0
        };
    }

    public void WriteTo(BinaryWriter writer)
    {
        writer.Write(Magic);
        writer.Write(Version);
        writer.Write(Flags);
        writer.Write(DirectoryOffset);
        writer.Write(DirectoryLength);
        // Pad to 32 bytes
        var padding = Size - (4 + 2 + 2 + 8 + 4); // 12 bytes padding
        for (int i = 0; i < padding; i++)
            writer.Write((byte)0);
    }

    public static SpkHeader ReadFrom(BinaryReader reader)
    {
        var magic = reader.ReadBytes(4);
        var version = reader.ReadUInt16();
        var flags = reader.ReadUInt16();
        var dirOffset = reader.ReadInt64();
        var dirLength = reader.ReadInt32();
        // Skip padding
        reader.ReadBytes(12);

        return new SpkHeader
        {
            Magic = magic,
            Version = version,
            Flags = flags,
            DirectoryOffset = dirOffset,
            DirectoryLength = dirLength
        };
    }
}
