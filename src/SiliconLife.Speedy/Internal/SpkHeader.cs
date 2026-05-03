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
/// .spk 文件头部结构。采用"双缓冲 Header 槽位 + CRC32 + 单调递增 Sequence"
/// 的设计来确保任何时刻至少有一个槽位处于一致状态：
///
/// 布局：
///   [0x0000..0x1000) HeaderSlotA (4K)
///   [0x1000..0x2000) HeaderSlotB (4K)
///   [0x2000..)       数据块 / Directory 区域（由 FreeList 分配）
///
/// 每个槽位的结构：
///   [0..4)   Magic "SPKY"        (4)
///   [4..6)   Version             (2)
///   [6..8)   Flags               (2)
///   [8..16)  DirectoryOffset     (8)
///   [16..20) DirectoryLength     (4)
///   [20..28) Sequence            (8)     — 每次成功提交自增
///   [28..32) CRC32               (4)     — 覆盖 [0..28)
///   [32..4096) 保留（写入时填 0）
///
/// 提交流程始终先写数据、再写对侧空闲的槽位；成功 fsync 后对侧槽位变为
/// "当前活动槽位"。这意味着崩溃恢复时：
///   * 两个槽位 CRC 都有效 → 选择 Sequence 更大的作为活动槽位；
///   * 仅一个槽位 CRC 有效 → 选择它；
///   * 都无效 → 尝试按 v1 legacy 格式解析，然后触发升级。
///
/// 兼容性：v1 的旧文件 Header 长度为 32B 写在 0 偏移，没有 Sequence / CRC；
/// SpeedyPack 在 Open 时若检测到 v1 会触发全量迁移到 v2。
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

    /// <summary>创建一个全新的 v2 Header，Sequence=1。</summary>
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
    /// 将 Header 写入指定槽位（0 或 1）。槽位整体 4K 区域会被清零并写入
    /// 32B 的有效 payload，其余保持 0。调用者负责 fsync。
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
    /// 尝试读取指定槽位的 Header 并校验 CRC。无效（Magic 错 / CRC 错 /
    /// Version 错 / 超出文件长度）返回 null。
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
    /// 尝试以 v1 legacy 格式（32B，无 CRC/Sequence）读取文件开头的头部。
    /// 仅用于识别老文件并触发迁移。
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
