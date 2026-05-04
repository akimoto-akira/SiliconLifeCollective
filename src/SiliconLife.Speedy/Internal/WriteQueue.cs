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
/// Asynchronous write queue. Producers (public API) call <see cref="Enqueue"/> and return immediately,
/// single-thread consumer processes operations in batches:
///   1) First persist with "WAL batch + CRC + Commit";
///   2) Then write data and new Directory to the main .spk file;
///   3) Atomically commit via dual Header slot switching;
///   4) Finally truncate WAL.
/// After a crash at any step, a consistent state can be recovered via WAL replay or Header rollback upon restart.
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

    /// <summary>Enqueues a single operation.</summary>
    public void Enqueue(WriteOperation op)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(WriteQueue));
        _queue.Enqueue(op);
        _hasItems.Set();
    }

    /// <summary>Enqueues a batch of operations (transaction commit).</summary>
    public void EnqueueBatch(IReadOnlyList<WriteOperation> ops)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(WriteQueue));
        foreach (var op in ops)
            _queue.Enqueue(op);
        _hasItems.Set();
    }

    /// <summary>
    /// Returns a Task that completes after "all currently enqueued operations have been persisted".
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

        // Drain remaining work during shutdown to ensure Dispose + FlushAsync consistency.
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

        // Merge duplicate writes to the same path: keep only the last one, reducing disk I/O and compressing Directory.
        var latestByPath = new Dictionary<string, WriteOperation>(StringComparer.Ordinal);
        foreach (var op in operations)
            latestByPath[op.NormalizedPath] = op;
        var finalOps = new List<WriteOperation>(latestByPath.Values);

        // ─── Step 1: WAL Pre-write ───
        // Append the entire batch of operations (including data bodies) to WAL and fsync.
        // After this, any crash can be recovered by replaying WAL.
        _wal.AppendBatch(finalOps);

        // ─── Step 2: Write main file data blocks ───
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

        // ─── Step 3: Write new Directory + atomically switch dual Header slots ───
        var snapshot = _directoryMap.Snapshot();
        _writer.WriteDirectoryAndCommit(snapshot);

        // ─── Step 4: At this point the new state has taken effect on disk, WAL can be safely cleared ───
        _wal.Truncate();

        // Clear pin, allowing TTL/LRU to reclaim normally.
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
