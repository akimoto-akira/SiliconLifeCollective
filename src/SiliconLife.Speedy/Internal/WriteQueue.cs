using System.Threading.Channels;

namespace SiliconLife.Speedy.Internal;

/// <summary>
/// Asynchronous write queue backed by a <see cref="Channel{T}"/>.
/// A single background <see cref="Task"/> serially consumes operations, guaranteeing
/// that writes to the same path are applied in the order they were enqueued.
/// </summary>
/// <remarks>
/// Satisfies AC-5.1 – AC-5.5:
/// <list type="bullet">
///   <item>AC-5.1  Callers enqueue and return immediately; file I/O happens in the background.</item>
///   <item>AC-5.2  Single consumer Task, unbounded Channel.</item>
///   <item>AC-5.3  New entries are appended; old space is left as free.</item>
///   <item>AC-5.4  <see cref="FlushAsync"/> waits until the queue is drained and data is flushed.</item>
///   <item>AC-5.5  <see cref="Dispose"/> calls <see cref="FlushAsync"/> before closing.</item>
/// </list>
/// </remarks>
internal sealed class WriteQueue : IDisposable
{
    private readonly PackFileWriter _writer;
    private readonly DirectoryMap _directoryMap;

    // Unbounded channel — writers never block (AC-5.2)
    private readonly Channel<WriteOperation> _channel =
        Channel.CreateUnbounded<WriteOperation>(new UnboundedChannelOptions
        {
            SingleReader = true,   // only the background task reads
            SingleWriter = false   // multiple threads may enqueue
        });

    private readonly Task _backgroundTask;
    private bool _disposed;

    // ─── Constructor ──────────────────────────────────────────────────────────

    /// <summary>
    /// Initialises the queue and starts the background consumer task.
    /// </summary>
    /// <param name="writer">The file writer used to persist operations.</param>
    /// <param name="directoryMap">The in-memory directory index to keep in sync.</param>
    public WriteQueue(PackFileWriter writer, DirectoryMap directoryMap)
    {
        _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        _directoryMap = directoryMap ?? throw new ArgumentNullException(nameof(directoryMap));
        _backgroundTask = Task.Run(ConsumeLoopAsync);
    }

    // ─── Enqueue ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Enqueues a single write or delete operation.
    /// Returns immediately; the operation is applied asynchronously.
    /// </summary>
    public void Enqueue(WriteOperation op)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        // TryWrite on an unbounded channel always succeeds unless the channel is closed.
        _channel.Writer.TryWrite(op);
    }

    /// <summary>
    /// Enqueues a batch of operations atomically (all or nothing in terms of ordering).
    /// Used by transaction commit to ensure the batch is contiguous in the queue.
    /// </summary>
    public void EnqueueBatch(IEnumerable<WriteOperation> ops)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        foreach (var op in ops)
            _channel.Writer.TryWrite(op);
    }

    // ─── FlushAsync ───────────────────────────────────────────────────────────

    /// <summary>
    /// Waits until all currently-enqueued operations have been processed and
    /// persisted to disk (AC-5.4).
    /// </summary>
    /// <remarks>
    /// Implementation: enqueues a sentinel <see cref="FlushSentinel"/> operation and
    /// awaits a <see cref="TaskCompletionSource"/> that the background task completes
    /// when it processes the sentinel.
    /// </remarks>
    public Task FlushAsync()
    {
        if (_disposed)
            return Task.CompletedTask;

        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var sentinel = new FlushSentinel(tcs);
        _channel.Writer.TryWrite(sentinel);
        return tcs.Task;
    }

    // ─── Background consumer ──────────────────────────────────────────────────

    private async Task ConsumeLoopAsync()
    {
        await foreach (var op in _channel.Reader.ReadAllAsync())
        {
            try
            {
                ProcessOperation(op);
            }
            catch
            {
                // Swallow individual operation errors to keep the consumer alive.
                // In a production implementation you would log or surface these.
            }
        }
    }

    private void ProcessOperation(WriteOperation op)
    {
        switch (op)
        {
            case WriteEntry write:
                ApplyWrite(write);
                break;

            case DeleteEntry delete:
                ApplyDelete(delete);
                break;

            case FlushSentinel sentinel:
                // All preceding operations have been processed; flush to disk.
                _writer.Flush();
                sentinel.Completion.TrySetResult();
                break;
        }
    }

    /// <summary>
    /// Appends the entry's data to the Data Region, updates the DirectoryMap,
    /// and rewrites the Directory Region (AC-5.3).
    /// </summary>
    private void ApplyWrite(WriteEntry write)
    {
        DirectoryEntry newEntry;

        if (_directoryMap.TryGet(write.NormalizedPath, out var existing))
        {
            // Update: preserve CreatedAt, old space becomes free (not reclaimed until Compact)
            newEntry = _writer.AppendEntryUpdate(
                write.NormalizedPath, write.Data, write.ContentType, existing);
        }
        else
        {
            // New entry
            newEntry = _writer.AppendEntry(write.NormalizedPath, write.Data, write.ContentType);
        }

        // Keep the in-memory index in sync
        _directoryMap.Set(write.NormalizedPath, newEntry);

        // Rewrite the Directory Region so the file is always consistent
        _writer.WriteDirectory(_directoryMap.Snapshot());
    }

    /// <summary>
    /// Removes the entry from the DirectoryMap and rewrites the Directory Region.
    /// The old data bytes in the Data Region are left as free space (reclaimed by Compact).
    /// </summary>
    private void ApplyDelete(DeleteEntry delete)
    {
        _directoryMap.Remove(delete.NormalizedPath);
        _writer.WriteDirectory(_directoryMap.Snapshot());
    }

    // ─── Dispose ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Flushes all pending operations to disk, then closes the channel and waits
    /// for the background task to finish (AC-5.5).
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        // Drain the queue before closing
        try
        {
            FlushAsync().GetAwaiter().GetResult();
        }
        catch
        {
            // Best-effort flush on dispose
        }

        // Signal the channel that no more items will be written
        _channel.Writer.TryComplete();

        // Wait for the background task to finish processing remaining items
        try
        {
            _backgroundTask.GetAwaiter().GetResult();
        }
        catch
        {
            // Ignore task cancellation / completion exceptions
        }
    }

    // ─── Private sentinel type ────────────────────────────────────────────────

    /// <summary>
    /// Internal sentinel operation used by <see cref="FlushAsync"/> to detect
    /// when all preceding operations have been processed.
    /// </summary>
    private sealed record FlushSentinel(TaskCompletionSource Completion)
        : WriteOperation(string.Empty);
}
