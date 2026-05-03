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

using System.Collections.Concurrent;

namespace SiliconLife.Speedy.Internal;

/// <summary>
/// 异步写入队列。生产者（公共 API）调用 <see cref="Enqueue"/> 立即返回，
/// 单线程消费者按批次将操作：
///   1) 先以"WAL 批次 + CRC + Commit"落盘；
///   2) 再把数据和新 Directory 写到主 .spk 文件；
///   3) 通过双 Header 槽位切换原子提交；
///   4) 最后截断 WAL。
/// 任意一步崩溃后重启时都可以通过 WAL 重放或 Header 回退恢复到一致状态。
/// </summary>
internal sealed class WriteQueue : IDisposable
{
    private readonly PackFileWriter _writer;
    private readonly DirectoryMap _directoryMap;
    private readonly EntryCache _entryCache;
    private readonly WriteAheadLog _wal;

    private readonly ConcurrentQueue<WriteOperation> _queue = new();
    private readonly ManualResetEventSlim _hasItems = new(initialState: false);
    private readonly CancellationTokenSource _cts = new();
    private readonly Task _consumerTask;

    private readonly object _flushLock = new();
    private TaskCompletionSource? _flushTcs;

    private volatile bool _disposed;

    public WriteQueue(PackFileWriter writer, DirectoryMap directoryMap, EntryCache entryCache, WriteAheadLog wal)
    {
        _writer = writer;
        _directoryMap = directoryMap;
        _entryCache = entryCache;
        _wal = wal;
        _consumerTask = Task.Factory.StartNew(
            ConsumeLoop,
            _cts.Token,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);
    }

    /// <summary>入队单个操作。</summary>
    public void Enqueue(WriteOperation op)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(WriteQueue));
        _queue.Enqueue(op);
        _hasItems.Set();
    }

    /// <summary>入队一批操作（事务提交）。</summary>
    public void EnqueueBatch(IReadOnlyList<WriteOperation> ops)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(WriteQueue));
        foreach (var op in ops)
            _queue.Enqueue(op);
        _hasItems.Set();
    }

    /// <summary>
    /// 返回一个在"当前所有已入队操作都已持久化"后完成的 Task。
    /// </summary>
    public Task FlushAsync()
    {
        lock (_flushLock)
        {
            if (_queue.IsEmpty && _flushTcs == null)
                return Task.CompletedTask;

            _flushTcs ??= new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            _hasItems.Set();
            return _flushTcs.Task;
        }
    }

    private void ConsumeLoop()
    {
        var ct = _cts.Token;
        while (!ct.IsCancellationRequested)
        {
            try
            {
                _hasItems.Wait(ct);
                _hasItems.Reset();

                ProcessBatch();

                if (_queue.IsEmpty)
                {
                    TaskCompletionSource? toSignal = null;
                    lock (_flushLock)
                    {
                        if (_flushTcs != null)
                        {
                            toSignal = _flushTcs;
                            _flushTcs = null;
                        }
                    }
                    toSignal?.TrySetResult();
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                TaskCompletionSource? toFail = null;
                lock (_flushLock)
                {
                    if (_flushTcs != null)
                    {
                        toFail = _flushTcs;
                        _flushTcs = null;
                    }
                }
                toFail?.TrySetException(ex);
            }
        }

        // 关机时排空剩余工作，保证 Dispose + FlushAsync 的一致性。
        try { ProcessBatch(); } catch { /* ignore */ }

        TaskCompletionSource? shutdownTcs;
        lock (_flushLock)
        {
            shutdownTcs = _flushTcs;
            _flushTcs = null;
        }
        shutdownTcs?.TrySetResult();
    }

    private void ProcessBatch()
    {
        var operations = new List<WriteOperation>();
        while (_queue.TryDequeue(out var op))
            operations.Add(op);

        if (operations.Count == 0)
            return;

        // 合并对同一路径的重复写入：只保留最后一个，减少磁盘 I/O 并压缩 Directory。
        var latestByPath = new Dictionary<string, WriteOperation>(StringComparer.Ordinal);
        foreach (var op in operations)
            latestByPath[op.NormalizedPath] = op;
        var finalOps = new List<WriteOperation>(latestByPath.Values);

        // ─── 第 1 步：WAL 预写 ───
        // 将整批操作（包含数据体）追加到 WAL 并 fsync。
        // 之后任何崩溃都可以通过重放 WAL 恢复这一批。
        _wal.AppendBatch(finalOps);

        // ─── 第 2 步：写主文件数据块 ───
        var dirtyPaths = new List<string>(finalOps.Count);

        foreach (var op in finalOps)
        {
            switch (op)
            {
                case WriteEntry write:
                    DateTime? createdAt = null;
                    if (_directoryMap.TryGet(write.NormalizedPath, out var oldEntry))
                    {
                        createdAt = oldEntry.CreatedAt;
                        _writer.ReleaseEntry(oldEntry);
                    }
                    var entry = _writer.AppendEntry(
                        write.NormalizedPath, write.Data, write.ContentType, createdAt);
                    _directoryMap.Set(write.NormalizedPath, entry);
                    dirtyPaths.Add(write.NormalizedPath);
                    break;

                case DeleteEntry delete:
                    if (delete.OldEntry != null)
                        _writer.ReleaseEntry(delete.OldEntry);
                    _directoryMap.Remove(delete.NormalizedPath);
                    _entryCache.Invalidate(delete.NormalizedPath);
                    break;
            }
        }

        // ─── 第 3 步：写新 Directory + 原子切换双 Header 槽位 ───
        var snapshot = _directoryMap.Snapshot();
        _writer.WriteDirectoryAndCommit(snapshot);

        // ─── 第 4 步：至此新状态已在磁盘生效，WAL 可以安全清空 ───
        _wal.Truncate();

        // 清除 pin，允许 TTL/LRU 正常回收。
        foreach (var path in dirtyPaths)
            _entryCache.Unpin(path);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _hasItems.Set();
        _cts.Cancel();

        try { _consumerTask.Wait(TimeSpan.FromSeconds(10)); }
        catch { /* ignore */ }

        _hasItems.Dispose();
        _cts.Dispose();
    }
}
