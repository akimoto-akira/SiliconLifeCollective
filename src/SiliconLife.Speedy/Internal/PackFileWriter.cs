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
/// .spk 文件的写入器，对外提供 4K 对齐的 FreeList 分配器以及"数据写入 +
/// Directory 切换 + Header 双缓冲原子提交"的安全提交协议。
/// </summary>
/// <remarks>
/// 文件布局（v2）：
///   [0..4K)   HeaderSlotA
///   [4K..8K)  HeaderSlotB
///   [8K..)    数据块 + Directory（通过 FreeList 分配，4K 对齐）
///
/// 提交协议（原子性关键）：
///   1. 新数据块始终分配到新的位置（COW），绝不覆盖旧数据；
///   2. 新 Directory 也分配到新位置；
///   3. 新 Header 写到"对侧"空闲槽位，并在 fsync 之后切换活动槽位；
///   4. 任意步骤崩溃，未完成的写入对旧活动槽位完全不可见。
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
    /// 创建一个新的 v2 .spk 文件：初始化双 Header 槽位（槽位 A 有效、槽位 B 为空），
    /// 文件长度恰好等于 TotalHeaderSize=8K。
    /// </summary>
    public static PackFileWriter Create(string filePath)
    {
        var header = SpkHeader.CreateNew();
        var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);

        // 先把文件扩展到 8K，整块清零，再写有效槽位 A。
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
    /// 以给定的"活动 Header + 活动槽位 + 已加载的 Directory"打开文件用于写入，
    /// 并据此重建 FreeList。
    /// </summary>
    public static PackFileWriter Open(
        string filePath,
        IReadOnlyDictionary<string, DirectoryEntry> directory,
        SpkHeader activeHeader,
        int activeSlot)
    {
        var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
        // 确保文件至少有双 Header 区域长度。
        if (stream.Length < SpkHeader.TotalHeaderSize)
            stream.SetLength(SpkHeader.TotalHeaderSize);

        var writer = new PackFileWriter(filePath, stream, activeHeader, activeSlot);
        writer.BuildFreeList(directory, stream.Length);
        return writer;
    }

    /// <summary>
    /// 根据"活动 Directory + 活动 Header Directory 区 + 文件长度"重建 FreeList。
    /// 双 Header 区（前 8K）视为占用；任何落在 live 区域之间的间隙都视为可复用空间。
    /// </summary>
    private void BuildFreeList(IReadOnlyDictionary<string, DirectoryEntry> directory, long fileLength)
    {
        _freeList.Clear();

        var occupied = new List<(long Offset, long Length)>
        {
            // 双 Header 槽位区，永久占用。
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
    /// 追加（或复用空闲块）一个数据条目，返回新的 DirectoryEntry。
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

    /// <summary>在保留原 CreatedAt 的前提下，重写条目到新的位置。</summary>
    public DirectoryEntry AppendEntryUpdate(string normalizedPath, byte[] data, string contentType, DirectoryEntry oldEntry)
    {
        return AppendEntry(normalizedPath, data, contentType, oldEntry.CreatedAt);
    }

    /// <summary>将旧条目占用的对齐空间归还给 FreeList。</summary>
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
    /// 安全提交协议：先把新的 Directory 写到新位置并 fsync，再把新 Header
    /// 写到对侧槽位并 fsync，然后切换活动槽位，最后把旧 Directory 区释放
    /// 给 FreeList。中途任意崩溃都不会让活动槽位指向不完整状态。
    /// </summary>
    public void WriteDirectoryAndCommit(IReadOnlyDictionary<string, DirectoryEntry> entries)
    {
        lock (_lock)
        {
            var oldDirOffset = _header.DirectoryOffset;
            var oldDirLength = _header.DirectoryLength;

            // 1. 序列化并分配新 Directory 区（新位置，不覆盖旧 Directory）。
            var dirBytes = MessagePackSerializer.Serialize(entries);
            var alignedSize = AlignUp(dirBytes.Length);
            var newDirOffset = AllocateSpaceInternal(alignedSize);

            // 2. 写 Directory 数据并 fsync：必须在切换 Header 之前落盘。
            _stream.Position = newDirOffset;
            _writer.Write(dirBytes);
            _writer.Flush();
            _stream.Flush(flushToDisk: true);

            // 3. 构造新 Header 写入对侧槽位，并 fsync。Sequence 严格自增，
            //    确保崩溃恢复时能明确区分新旧槽位。
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

            // 4. Header fsync 成功后，对侧槽位正式成为活动槽位。
            _header = newHeader;
            _activeSlot = inactiveSlot;

            // 5. 旧 Directory 区此时才能归还给 FreeList（在此之前若崩溃，
            //    旧槽位仍然指向它，必须保持可读）。
            if (oldDirLength > 0 && oldDirOffset >= SpkHeader.TotalHeaderSize)
                _freeList.Release(oldDirOffset, AlignUp(oldDirLength));
        }
    }

    /// <summary>兼容旧签名；等价于 WriteDirectoryAndCommit。</summary>
    public void WriteDirectory(IReadOnlyDictionary<string, DirectoryEntry> entries)
        => WriteDirectoryAndCommit(entries);

    /// <summary>强制所有已缓冲的写入落到磁盘。</summary>
    public void Flush()
    {
        lock (_lock)
        {
            _writer.Flush();
            _stream.Flush(flushToDisk: true);
        }
    }

    /// <summary>当前活动槽位索引（供诊断与测试使用）。</summary>
    public int ActiveSlot
    {
        get { lock (_lock) return _activeSlot; }
    }

    /// <summary>当前活动 Header 的 Sequence（供诊断使用）。</summary>
    public long Sequence
    {
        get { lock (_lock) return _header.Sequence; }
    }

    // ─── 内部分配器 ────────────────────────────────────────────────────────

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
