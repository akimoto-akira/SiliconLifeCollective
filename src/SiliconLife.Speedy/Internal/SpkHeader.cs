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

namespace SiliconLife.Speedy.Internal;

/// <summary>
/// .spk file header structure. Uses a "dual-buffer Header slot + CRC32 + monotonically increasing Sequence"
/// design to ensure at least one slot is in a consistent state at any time:
///
/// Layout:
///   [0x0000..0x1000) HeaderSlotA (4K)
///   [0x1000..0x2000) HeaderSlotB (4K)
///   [0x2000..)       Data block / Directory area (allocated by FreeList)
///
/// Structure of each slot:
///   [0..4)   Magic "SPKY"        (4)
///   [4..6)   Version             (2)
///   [6..8)   Flags               (2)
///   [8..16)  DirectoryOffset     (8)
///   [16..20) DirectoryLength     (4)
///   [20..28) Sequence            (8)     — incremented on each successful commit
///   [28..32) CRC32               (4)     — covers [0..28)
///   [32..4096) Reserved (filled with 0 when writing)
///
/// The commit flow always writes data first, then writes to the idle slot on the opposite side;
/// after successful fsync, the opposite slot becomes the "current active slot". This means during
/// crash recovery:
///   * Both slots have valid CRC → choose the one with larger Sequence as the active slot;
///   * Only one slot has valid CRC → choose it;
///   * Both invalid → try parsing in v1 legacy format, then trigger upgrade.
///
/// Compatibility: v1 old files have Header length of 32B at offset 0, without Sequence / CRC;
/// SpeedyPack triggers a full migration to v2 when v1 is detected during Open.
/// </summary>
internal sealed class SpkHeader
{
    public const int SlotSize = 4096;
    public const int SlotCount = 2;
    public const int TotalHeaderSize = SlotSize * SlotCount; // 8192

    public const int HeaderBodySize = 28; // 参与 CRC 的字节数
    public const int HeaderCrcSize = 4;
    public const int HeaderPayloadSize = HeaderBodySize + HeaderCrcSize; // 32

    public static readonly byte[] MagicBytes = { 0x53, 0x50, 0x4B, 0x59 }; // "SPKY"
    public const ushort CurrentVersion = 2;
    public const ushort LegacyVersion = 1;

    public byte[] Magic { get; set; } = MagicBytes;
    public ushort Version { get; set; } = CurrentVersion;
    public ushort Flags { get; set; } = 0;
    public long DirectoryOffset { get; set; }
    public int DirectoryLength { get; set; }
    public long Sequence { get; set; } = 0;

    /// <summary>Creates a brand new v2 Header with Sequence=1.</summary>
    public static SpkHeader CreateNew()
    {
        return new SpkHeader
        {
            Magic = MagicBytes,
            Version = CurrentVersion,
            Flags = 0,
            DirectoryOffset = TotalHeaderSize,
            DirectoryLength = 0,
            Sequence = 1
        };
    }

    /// <summary>
    /// Writes the Header to the specified slot (0 or 1). The entire 4K slot area is cleared and
    /// written with 32B of valid payload, the rest remains 0. Caller is responsible for fsync.
    /// </summary>
    public void WriteToSlot(Stream stream, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= SlotCount)
            throw new ArgumentOutOfRangeException(nameof(slotIndex));

        var body = SerializeBody();
        var crc = Crc32.Compute(body);

        var slotBuffer = new byte[SlotSize];
        Buffer.BlockCopy(body, 0, slotBuffer, 0, HeaderBodySize);
        var crcBytes = BitConverter.GetBytes(crc);
        Buffer.BlockCopy(crcBytes, 0, slotBuffer, HeaderBodySize, 4);

        stream.Position = (long)slotIndex * SlotSize;
        stream.Write(slotBuffer, 0, SlotSize);
    }

    /// <summary>
    /// Tries to read the Header from the specified slot and verify CRC. Invalid (wrong Magic / CRC error /
    /// Version error / beyond file length) returns null.
    /// </summary>
    public static SpkHeader? TryReadSlot(Stream stream, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= SlotCount) return null;
        var slotOffset = (long)slotIndex * SlotSize;
        if (slotOffset + HeaderPayloadSize > stream.Length) return null;

        stream.Position = slotOffset;
        var body = new byte[HeaderBodySize];
        if (!ReadFully(stream, body, HeaderBodySize)) return null;

        var crcBuf = new byte[4];
        if (!ReadFully(stream, crcBuf, 4)) return null;

        var storedCrc = BitConverter.ToUInt32(crcBuf, 0);
        var computedCrc = Crc32.Compute(body);
        if (storedCrc != computedCrc) return null;

        var magic = new byte[4];
        Buffer.BlockCopy(body, 0, magic, 0, 4);
        if (!magic.AsSpan().SequenceEqual(MagicBytes)) return null;

        var version = BitConverter.ToUInt16(body, 4);
        if (version != CurrentVersion) return null;

        return new SpkHeader
        {
            Magic = magic,
            Version = version,
            Flags = BitConverter.ToUInt16(body, 6),
            DirectoryOffset = BitConverter.ToInt64(body, 8),
            DirectoryLength = BitConverter.ToInt32(body, 16),
            Sequence = BitConverter.ToInt64(body, 20)
        };
    }

    /// <summary>
    /// Tries to read the header at the beginning of the file in v1 legacy format (32B, no CRC/Sequence).
    /// Only used to identify old files and trigger migration.
    /// </summary>
    public static SpkHeader? TryReadLegacyV1(Stream stream)
    {
        if (stream.Length < 32) return null;
        stream.Position = 0;
        var buf = new byte[32];
        if (!ReadFully(stream, buf, 32)) return null;

        var magic = new byte[4];
        Buffer.BlockCopy(buf, 0, magic, 0, 4);
        if (!magic.AsSpan().SequenceEqual(MagicBytes)) return null;

        var version = BitConverter.ToUInt16(buf, 4);
        if (version != LegacyVersion) return null;

        return new SpkHeader
        {
            Magic = magic,
            Version = version,
            Flags = BitConverter.ToUInt16(buf, 6),
            DirectoryOffset = BitConverter.ToInt64(buf, 8),
            DirectoryLength = BitConverter.ToInt32(buf, 16),
            Sequence = 0
        };
    }

    private byte[] SerializeBody()
    {
        var buf = new byte[HeaderBodySize];
        Buffer.BlockCopy(Magic, 0, buf, 0, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(Version), 0, buf, 4, 2);
        Buffer.BlockCopy(BitConverter.GetBytes(Flags), 0, buf, 6, 2);
        Buffer.BlockCopy(BitConverter.GetBytes(DirectoryOffset), 0, buf, 8, 8);
        Buffer.BlockCopy(BitConverter.GetBytes(DirectoryLength), 0, buf, 16, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(Sequence), 0, buf, 20, 8);
        return buf;
    }

    private static bool ReadFully(Stream stream, byte[] buffer, int count)
    {
        var read = 0;
        while (read < count)
        {
            var n = stream.Read(buffer, read, count - read);
            if (n <= 0) return false;
            read += n;
        }
        return true;
    }
}
