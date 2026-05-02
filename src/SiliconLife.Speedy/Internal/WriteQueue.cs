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
/// Asynchronous write queue. Producers (public API) call <see cref="Enqueue"/>
/// and return immediately; a single consumer task drains operations in batches
/// and writes them to disk via <see cref="PackFileWriter"/>.
/// </summary>
internal sealed class WriteQueue : IDisposable
{
    private readonly PackFileWriter _writer;
    private readonly DirectoryMap _directoryMap;
    private readonly EntryCache _entryCache;

    private readonly ConcurrentQueue<WriteOperation> _queue = new();
    private readonly ManualResetEventSlim _hasItems = new(initialState: false);
    private readonly CancellationTokenSource _cts = new();
    private readonly Task _consumerTask;

    private readonly object _flushLock = new();
    private TaskCompletionSource? _flushTcs;

    private volatile bool _disposed;

    public WriteQueue(PackFileWriter writer, DirectoryMap directoryMap, EntryCache entryCache)
    {
        _writer = writer;
        _directoryMap = directoryMap;
        _entryCache = entryCache;
        _consumerTask = Task.Factory.StartNew(
            ConsumeLoop,
            _cts.Token,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);
    }

    /// <summary>Enqueues a single operation for later persistence.</summary>
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
    /// Returns a task that completes when every operation enqueued prior to
    /// this call has been persisted to disk. If the queue is empty and no
    /// batch is in flight, returns a completed task immediately.
    /// </summary>
    public Task FlushAsync()
    {
        // Wait for current batch to complete, even if queue is empty.
        lock (_flushLock)
        {
            if (_queue.IsEmpty && _flushTcs == null)
                return Task.CompletedTask;

            _flushTcs ??= new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            _hasItems.Set(); // Make sure the consumer wakes up.
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
                // Block until work arrives or a short poll interval elapses.
                _hasItems.Wait(ct);
                _hasItems.Reset();

                ProcessBatch();

                // Signal any awaiting FlushAsync callers now that the queue is drained.
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
                // Fail the current flush tcs so callers don't hang forever,
                // then swallow — the queue keeps running for future work.
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

        // Drain remaining work on shutdown so Dispose + FlushAsync stays consistent.
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

        // Collapse duplicate writes to the same path — only the last wins,
        // reducing disk I/O and keeping the directory region compact.
        var latestByPath = new Dictionary<string, WriteOperation>(StringComparer.Ordinal);
        foreach (var op in operations)
            latestByPath[op.NormalizedPath] = op;

        var dirtyPaths = new List<string>(latestByPath.Count);

        foreach (var op in latestByPath.Values)
        {
            switch (op)
            {
                case WriteEntry write:
                    // On overwrite: release the space occupied by the old entry to
                    // FreeList, and preserve the original CreatedAt so that
                    // AppendEntry may reuse the same block in-place in the FreeList,
                    // avoiding infinite file growth.
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
                    // On deletion: use the OldEntry pre-captured by
                    // SpeedyPack.Delete / ApplyTransactionBatch to return the old
                    // space to FreeList. The synchronous DirectoryMap removal has
                    // already been performed; we perform Remove/Invalidate again
                    // here to keep it idempotent.
                    if (delete.OldEntry != null)
                        _writer.ReleaseEntry(delete.OldEntry);
                    _directoryMap.Remove(delete.NormalizedPath);
                    _entryCache.Invalidate(delete.NormalizedPath);
                    break;
            }
        }

        // Persist directory region + header and fsync.
        var snapshot = _directoryMap.Snapshot();
        _writer.WriteDirectory(snapshot);
        _writer.Flush();

        // Unpin successfully-persisted writes so TTL/LRU eviction may reclaim them.
        foreach (var path in dirtyPaths)
            _entryCache.Unpin(path);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        // Wake the consumer so it can notice cancellation and drain.
        _hasItems.Set();
        _cts.Cancel();

        try { _consumerTask.Wait(TimeSpan.FromSeconds(10)); }
        catch { /* ignore */ }

        _hasItems.Dispose();
        _cts.Dispose();
    }
}
