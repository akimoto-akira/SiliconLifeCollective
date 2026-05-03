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

using System.Text.Json;
using MessagePack;
using SiliconLife.Speedy.Internal;

namespace SiliconLife.Speedy;

/// <summary>
/// .spk 数据包的读写入口。内部整合了：
///   · 内存 <see cref="DirectoryMap"/>（目录索引）
///   · <see cref="EntryCache"/>（TTL/LRU 缓存，支持 pin）
///   · <see cref="WriteAheadLog"/>（预写日志，保证批次原子性）
///   · <see cref="PackFileWriter"/> 的双缓冲 Header 提交协议
/// 以实现低延迟读、异步写，并在进程正常/异常退出时都能保持文件一致性。
/// </summary>
/// <remarks>
/// 崩溃一致性由三道防线保障：
///   1. 数据写入遵循 Copy-on-Write：新数据和新 Directory 都写到新位置，旧版本
///      在 Header 切换前一直可读；
///   2. 双 Header 槽位交替写入并 fsync，保证任意时刻至少有一个槽位处于一致状态；
///   3. WAL 记录每一批待提交操作（含数据体），若主文件的 Header 切换尚未完成，
///      重启时可通过重放 WAL 把这一批重新应用到主文件。
/// 线程安全：公共成员可跨线程并发调用。
/// </remarks>
public sealed class SpeedyPack : IDisposable
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    static SpeedyPack()
    {
        var resolver = MessagePack.Resolvers.CompositeResolver.Create(
            MessagePack.Resolvers.BuiltinResolver.Instance,
            MessagePack.Resolvers.ContractlessStandardResolver.Instance);

        var options = MessagePackSerializerOptions.Standard
            .WithResolver(resolver)
            .WithCompression(MessagePackCompression.Lz4Block);

        MessagePackSerializer.DefaultOptions = options;
    }

    private readonly string _filePath;
    private readonly SpeedyPackOptions _options;
    private readonly DirectoryMap _directoryMap;
    private readonly EntryCache _entryCache;

    private WriteQueue? _writeQueue;
    private PackFileWriter? _writer;
    private PackFileReader _reader;
    private WriteAheadLog? _wal;

    private bool _disposed;
    private readonly SemaphoreSlim _compactLock = new(1, 1);

    private SpeedyPack(
        string filePath,
        SpeedyPackOptions options,
        DirectoryMap directoryMap,
        EntryCache entryCache,
        PackFileReader reader,
        PackFileWriter? writer,
        WriteQueue? writeQueue,
        WriteAheadLog? wal)
    {
        _filePath = filePath;
        _options = options;
        _directoryMap = directoryMap;
        _entryCache = entryCache;
        _reader = reader;
        _writer = writer;
        _writeQueue = writeQueue;
        _wal = wal;
    }

    /// <summary>
    /// 打开现有的 .spk 文件并加载其 Directory；若文件不存在则自动创建。
    /// 打开时会执行以下崩溃恢复动作：
    ///   1. 若检测到旧版 v1 格式，先就地迁移到 v2；
    ///   2. 读取双 Header 槽位并选择有效且 Sequence 最大的作为活动 Header；
    ///   3. 扫描 .wal，重放所有"已提交但未应用"的批次并清空 WAL。
    /// </summary>
    public static SpeedyPack Open(string filePath, SpeedyPackOptions? options = null)
    {
        options ??= new SpeedyPackOptions();

        var directoryMap = new DirectoryMap();
        var entryCache = new EntryCache(options.MaxCacheEntries, options.EntryCacheTtl);

        if (!File.Exists(filePath))
        {
            EnsureDirectoryExists(filePath);
            var newWriter = PackFileWriter.Create(filePath);
            var newReader = PackFileReader.Open(filePath);
            WriteAheadLog? newWal = null;
            WriteQueue? newQueue = null;
            if (!options.ReadOnly)
            {
                newWal = new WriteAheadLog(filePath + ".wal");
                newWal.Open();
                newWal.Truncate(); // 清理任何残留
                newQueue = new WriteQueue(newWriter, directoryMap, entryCache, newWal);
            }
            return new SpeedyPack(filePath, options, directoryMap, entryCache,
                newReader, options.ReadOnly ? null : newWriter, newQueue, newWal);
        }

        // 1. 检测 v1 旧文件并就地迁移。
        UpgradeLegacyFileIfNeeded(filePath);

        // 2. 读活动 Header + 加载 Directory。
        var reader = PackFileReader.Open(filePath);
        var active = reader.TryReadActiveHeader()
            ?? throw new InvalidDataException(
                $"Both header slots of '{filePath}' are corrupted. " +
                $"Recovery from WAL alone is not supported when both headers are unreadable.");
        var directory = reader.LoadDirectory(active.Header);
        directoryMap.LoadFrom(directory);

        if (options.ReadOnly)
        {
            return new SpeedyPack(filePath, options, directoryMap, entryCache,
                reader, writer: null, writeQueue: null, wal: null);
        }

        // 3. 打开 writer（重建 FreeList）。
        var writer = PackFileWriter.Open(filePath, directory, active.Header, active.Slot);

        // 4. 打开 WAL 并恢复任何未应用的已提交批次。
        var wal = new WriteAheadLog(filePath + ".wal");
        wal.Open();
        var pendingBatches = wal.RecoverCommittedBatches();

        var writeQueue = new WriteQueue(writer, directoryMap, entryCache, wal);

        if (pendingBatches.Count > 0)
        {
            // 将待恢复批次交给 WriteQueue 正常处理：会重新 AppendBatch、
            // 写主文件、切换 Header、Truncate WAL。对已应用过的批次重放是
            // 幂等的（会写到新位置并覆盖 Directory 条目）。
            foreach (var batch in pendingBatches)
            {
                if (batch.Count > 0)
                    writeQueue.EnqueueBatch(batch);
            }
            try { writeQueue.FlushAsync().Wait(TimeSpan.FromSeconds(30)); }
            catch { /* 忽略超时 —— 下次启动会继续恢复 */ }
        }
        else
        {
            // 没有待恢复批次但 WAL 文件可能存在残片（Commit 未写完的半批）。
            wal.Truncate();
        }

        return new SpeedyPack(filePath, options, directoryMap, entryCache,
            reader, writer, writeQueue, wal);
    }

    /// <summary>
    /// 强制创建一个新的 .spk 文件（若已存在则覆盖），并清理对应的 .wal。
    /// </summary>
    public static SpeedyPack Create(string filePath, SpeedyPackOptions? options = null)
    {
        options ??= new SpeedyPackOptions();

        EnsureDirectoryExists(filePath);

        // 覆盖创建前清理可能存在的 WAL 残留。
        var walPath = filePath + ".wal";
        if (File.Exists(walPath))
        {
            try { File.Delete(walPath); } catch { /* ignore */ }
        }

        var directoryMap = new DirectoryMap();
        var entryCache = new EntryCache(options.MaxCacheEntries, options.EntryCacheTtl);

        var writer = PackFileWriter.Create(filePath);
        var reader = PackFileReader.Open(filePath);
        var wal = new WriteAheadLog(walPath);
        wal.Open();
        var writeQueue = new WriteQueue(writer, directoryMap, entryCache, wal);

        return new SpeedyPack(filePath, options, directoryMap, entryCache,
            reader, writer, writeQueue, wal);
    }

    /// <summary>写入原始字节数据。缓存同步更新，磁盘持久化异步。</summary>
    public void Write(string path, ReadOnlySpan<byte> data)
    {
        ThrowIfReadOnly();
        var normalizedPath = PathNormalizer.Normalize(path);
        var bytes = data.ToArray();

        _entryCache.Set(normalizedPath, bytes, pinned: true);
        _writeQueue!.Enqueue(new WriteEntry(normalizedPath, bytes, "raw"));
    }

    /// <summary>写入原始字节数据（byte[] 重载）。</summary>
    public void Write(string path, byte[] data) => Write(path, data.AsSpan());

    /// <summary>
    /// 以显式 contentType（"raw" / "json" / "text"）写入字节。
    /// </summary>
    public void Write(string path, byte[] data, string contentType)
    {
        ThrowIfReadOnly();
        if (data is null) throw new ArgumentNullException(nameof(data));
        if (string.IsNullOrEmpty(contentType)) contentType = "raw";
        var normalizedPath = PathNormalizer.Normalize(path);

        _entryCache.Set(normalizedPath, data, pinned: true);
        _writeQueue!.Enqueue(new WriteEntry(normalizedPath, data, contentType));
    }

    /// <summary>序列化为 JSON 后写入。</summary>
    public void Write<T>(string path, T value)
    {
        ThrowIfReadOnly();
        var normalizedPath = PathNormalizer.Normalize(path);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, _jsonOptions);

        _entryCache.Set(normalizedPath, bytes, pinned: true);
        _writeQueue!.Enqueue(new WriteEntry(normalizedPath, bytes, "json"));
    }

    /// <summary>读取原始字节；不存在返回 null。</summary>
    public byte[]? Read(string path)
    {
        var normalizedPath = PathNormalizer.Normalize(path);

        if (_entryCache.TryGet(normalizedPath, out var cached))
            return cached;

        if (!_directoryMap.TryGet(normalizedPath, out var entry))
            return null;

        var bytes = _reader.ReadAt(entry.Offset, entry.Length);
        _entryCache.Set(normalizedPath, bytes, pinned: false);
        return bytes;
    }

    /// <summary>读取并反序列化 JSON 值；不存在返回 default。</summary>
    public T? Read<T>(string path)
    {
        var bytes = Read(path);
        if (bytes is null) return default;
        return JsonSerializer.Deserialize<T>(bytes, _jsonOptions);
    }

    /// <summary>判断路径是否存在。</summary>
    public bool Exists(string path)
    {
        var normalizedPath = PathNormalizer.Normalize(path);
        return _directoryMap.TryGet(normalizedPath, out _);
    }

    /// <summary>删除指定路径；缓存同步失效，磁盘持久化异步。</summary>
    public void Delete(string path)
    {
        ThrowIfReadOnly();
        var normalizedPath = PathNormalizer.Normalize(path);

        _directoryMap.TryGet(normalizedPath, out var oldEntry);
        _entryCache.Invalidate(normalizedPath);
        _directoryMap.Remove(normalizedPath);
        _writeQueue!.Enqueue(new DeleteEntry(normalizedPath) { OldEntry = oldEntry });
    }

    /// <summary>返回条目元数据（ContentType、长度、时间戳）。</summary>
    public (string ContentType, int Length, DateTime CreatedAt, DateTime UpdatedAt)? GetEntryMetadata(string path)
    {
        var normalizedPath = PathNormalizer.Normalize(path);
        if (!_directoryMap.TryGet(normalizedPath, out var entry))
            return null;

        return (entry.ContentType, entry.Length, entry.CreatedAt, entry.UpdatedAt);
    }

    /// <summary>返回文件头部和整体统计信息。</summary>
    public SpkFileInfo GetFileInfo()
    {
        var header = _reader.ReadHeader();
        var snapshot = _directoryMap.Snapshot();

        int jsonEntries = 0, rawEntries = 0, textEntries = 0;
        foreach (var entry in snapshot.Values)
        {
            switch (entry.ContentType)
            {
                case "json": jsonEntries++; break;
                case "raw": rawEntries++; break;
                case "text": textEntries++; break;
            }
        }

        var fileSize = new FileInfo(_filePath).Length;

        return new SpkFileInfo(
            FilePath: _filePath,
            FileSize: fileSize,
            Magic: System.Text.Encoding.ASCII.GetString(header.Magic),
            Version: header.Version,
            Flags: header.Flags,
            DirectoryOffset: header.DirectoryOffset,
            DirectoryLength: header.DirectoryLength,
            TotalEntries: snapshot.Count,
            JsonEntries: jsonEntries,
            RawEntries: rawEntries,
            TextEntries: textEntries);
    }

    /// <summary>列举指定目录的直接子条目（路径）。</summary>
    public IReadOnlyList<string> ListEntries(string directoryPath = "")
    {
        var normalizedPath = PathNormalizer.Normalize(directoryPath);
        return _directoryMap.ListChildren(normalizedPath);
    }

    /// <summary>列举指定目录的直接子目录。</summary>
    public IReadOnlyList<string> ListDirectories(string directoryPath = "")
    {
        var normalizedPath = PathNormalizer.Normalize(directoryPath);
        return _directoryMap.ListDirectories(normalizedPath);
    }

    /// <summary>开启一个事务。操作会缓冲直至 Commit 时一并落盘。</summary>
    public IPackTransaction BeginTransaction()
    {
        ThrowIfReadOnly();
        return new SpeedyTransaction(this);
    }

    /// <summary>等待所有已入队写入持久化完成。</summary>
    public Task FlushAsync()
    {
        if (_writeQueue is null) return Task.CompletedTask;
        return _writeQueue.FlushAsync();
    }

    /// <summary>
    /// 将文件压缩重写，剔除 FreeList 空洞。过程中使用临时文件，成功后原子替换。
    /// </summary>
    public async Task CompactAsync()
    {
        ThrowIfReadOnly();

        await _compactLock.WaitAsync().ConfigureAwait(false);
        try
        {
            await FlushAsync().ConfigureAwait(false);

            var snapshot = _directoryMap.Snapshot();

            var tempPath = _filePath + ".compact.tmp";
            try
            {
                var newEntries = new Dictionary<string, DirectoryEntry>(StringComparer.Ordinal);

                using (var tempWriter = PackFileWriter.Create(tempPath))
                {
                    foreach (var (normalizedPath, oldEntry) in snapshot)
                    {
                        byte[] bytes;
                        if (_entryCache.TryGet(normalizedPath, out var cached))
                            bytes = cached;
                        else
                            bytes = _reader.ReadAt(oldEntry.Offset, oldEntry.Length);

                        var newEntry = tempWriter.AppendEntryUpdate(
                            normalizedPath, bytes, oldEntry.ContentType, oldEntry);
                        newEntries[normalizedPath] = newEntry;
                    }

                    tempWriter.WriteDirectoryAndCommit(newEntries);
                    tempWriter.Flush();
                }

                // 关闭旧的 writer / WAL / reader，准备替换。
                _writeQueue!.Dispose();
                _reader.Dispose();
                _writer!.Dispose();
                _wal?.Dispose();

                File.Move(tempPath, _filePath, overwrite: true);

                // 原 WAL 也要清空（压缩后其内容已失去意义）。
                var walPath = _filePath + ".wal";
                if (File.Exists(walPath))
                {
                    try { File.Delete(walPath); } catch { /* ignore */ }
                }

                _directoryMap.LoadFrom(newEntries);

                _reader = PackFileReader.Open(_filePath);
                var active = _reader.TryReadActiveHeader()
                    ?? throw new InvalidDataException("Compacted file has no valid header slot.");
                _writer = PackFileWriter.Open(_filePath, newEntries, active.Header, active.Slot);

                _wal = new WriteAheadLog(walPath);
                _wal.Open();
                _writeQueue = new WriteQueue(_writer, _directoryMap, _entryCache, _wal);
            }
            catch
            {
                if (File.Exists(tempPath))
                {
                    try { File.Delete(tempPath); } catch { /* ignore */ }
                }
                throw;
            }
        }
        finally
        {
            _compactLock.Release();
        }
    }

    /// <summary>
    /// 清空所有挂起写入后关闭所有文件句柄。即便 Flush 超时，WAL 里的已提交批次
    /// 会在下次 Open 时被重放，因此不会丢失已 Commit 的数据。
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        if (_writeQueue != null)
        {
            try
            {
                var flushTask = _writeQueue.FlushAsync();
                flushTask.Wait(TimeSpan.FromSeconds(10));
            }
            catch { /* ignore */ }

            try { _writeQueue.Dispose(); } catch { /* ignore */ }
        }

        try { _reader.Dispose(); } catch { /* ignore */ }
        try { _writer?.Dispose(); } catch { /* ignore */ }
        try { _wal?.Dispose(); } catch { /* ignore */ }

        _compactLock.Dispose();
    }

    private static void EnsureDirectoryExists(string filePath)
    {
        var dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);
    }

    private void ThrowIfReadOnly()
    {
        if (_options.ReadOnly)
            throw new InvalidOperationException("This SpeedyPack instance is read-only.");
    }

    /// <summary>
    /// 供 <see cref="SpeedyTransaction.Commit"/> 使用：以原子方式把一批操作
    /// 同时应用到主缓存和写入队列。
    /// </summary>
    internal void ApplyTransactionBatch(IEnumerable<WriteOperation> ops)
    {
        var opList = ops as IReadOnlyList<WriteOperation> ?? ops.ToList();

        foreach (var op in opList)
        {
            switch (op)
            {
                case WriteEntry write:
                    _entryCache.Set(write.NormalizedPath, write.Data, pinned: true);
                    break;
                case DeleteEntry delete:
                    if (_directoryMap.TryGet(delete.NormalizedPath, out var existing))
                        delete.OldEntry = existing;
                    _entryCache.Invalidate(delete.NormalizedPath);
                    _directoryMap.Remove(delete.NormalizedPath);
                    break;
            }
        }

        _writeQueue!.EnqueueBatch(opList);
    }

    // ─── Legacy v1 迁移 ─────────────────────────────────────────────────────

    /// <summary>
    /// 检测文件是否为 v1 legacy 格式；若是则将其迁移为 v2 双 Header 格式。
    /// 迁移通过"临时文件 + 原子 File.Move"完成，过程中原文件不会被破坏。
    /// </summary>
    private static void UpgradeLegacyFileIfNeeded(string filePath)
    {
        // 先以只读方式检查头部，避免与后续 PackFileReader 抢独占。
        SpkHeader? v1Header;
        Dictionary<string, DirectoryEntry>? legacyDirectory = null;
        using (var probe = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            // 若其中任一 v2 槽位有效，则认为文件已是 v2，直接返回。
            var slotA = SpkHeader.TryReadSlot(probe, 0);
            var slotB = SpkHeader.TryReadSlot(probe, 1);
            if (slotA != null || slotB != null) return;

            // 否则尝试读 v1 legacy 头部。
            v1Header = SpkHeader.TryReadLegacyV1(probe);
            if (v1Header == null) return; // 不是 v1 也不是有效 v2，交由上层处理

            if (v1Header.DirectoryLength > 0)
            {
                probe.Position = v1Header.DirectoryOffset;
                var buf = new byte[v1Header.DirectoryLength];
                var read = 0;
                while (read < buf.Length)
                {
                    var n = probe.Read(buf, read, buf.Length - read);
                    if (n <= 0) break;
                    read += n;
                }
                legacyDirectory = MessagePackSerializer.Deserialize<Dictionary<string, DirectoryEntry>>(buf)
                                  ?? new Dictionary<string, DirectoryEntry>(StringComparer.Ordinal);
            }
            else
            {
                legacyDirectory = new Dictionary<string, DirectoryEntry>(StringComparer.Ordinal);
            }
        }

        // 用临时 v2 文件重写所有 entry。
        var tmpPath = filePath + ".v2upgrade.tmp";
        if (File.Exists(tmpPath))
        {
            try { File.Delete(tmpPath); } catch { /* ignore */ }
        }

        using (var legacyReader = PackFileReader.Open(filePath))
        using (var newWriter = PackFileWriter.Create(tmpPath))
        {
            var newEntries = new Dictionary<string, DirectoryEntry>(StringComparer.Ordinal);
            foreach (var (path, oldEntry) in legacyDirectory)
            {
                var data = legacyReader.ReadAt(oldEntry.Offset, oldEntry.Length);
                var newEntry = newWriter.AppendEntryUpdate(path, data, oldEntry.ContentType, oldEntry);
                newEntries[path] = newEntry;
            }
            newWriter.WriteDirectoryAndCommit(newEntries);
            newWriter.Flush();
        }

        // 原子替换原文件；失败时保留原文件不动。
        File.Move(tmpPath, filePath, overwrite: true);

        // 迁移后清理可能存在的旧 .wal（v1 没有 WAL，但以防残留）。
        var walPath = filePath + ".wal";
        if (File.Exists(walPath))
        {
            try { File.Delete(walPath); } catch { /* ignore */ }
        }
    }
}
