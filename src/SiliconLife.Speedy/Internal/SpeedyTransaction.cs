using System.Text.Json;

namespace SiliconLife.Speedy.Internal;

/// <summary>
/// Buffers write and delete operations locally until <see cref="Commit"/> is called.
/// Implements <see cref="SiliconLife.Speedy.IPackTransaction"/> (AC-6.1 – AC-6.7).
/// </summary>
internal sealed class SpeedyTransaction : SiliconLife.Speedy.IPackTransaction
{
    private readonly SpeedyPack _pack;
    private readonly List<WriteOperation> _pending = new();
    private bool _committed;
    private bool _disposed;

    internal SpeedyTransaction(SpeedyPack pack)
    {
        _pack = pack ?? throw new ArgumentNullException(nameof(pack));
    }

    // ─── IPackTransaction ─────────────────────────────────────────────────────

    /// <inheritdoc/>
    public bool IsCommitted => _committed;

    /// <summary>
    /// Buffers a raw-bytes write. Does not affect the main cache or file (AC-6.2).
    /// </summary>
    public void Write(string path, ReadOnlySpan<byte> data)
    {
        ThrowIfDone();
        var normalizedPath = PathNormalizer.Normalize(path);
        _pending.Add(new WriteEntry(normalizedPath, data.ToArray(), "raw"));
    }

    /// <summary>
    /// Buffers a raw-bytes write (byte[] overload to avoid generic ambiguity).
    /// </summary>
    public void Write(string path, byte[] data) => Write(path, data.AsSpan());

    /// <summary>
    /// Serializes <paramref name="value"/> as JSON and buffers the write (AC-6.2).
    /// </summary>
    public void Write<T>(string path, T value)
    {
        ThrowIfDone();
        var normalizedPath = PathNormalizer.Normalize(path);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
        _pending.Add(new WriteEntry(normalizedPath, bytes, "json"));
    }

    /// <summary>
    /// Buffers a delete. Does not affect the main cache or file (AC-6.2).
    /// </summary>
    public void Delete(string path)
    {
        ThrowIfDone();
        var normalizedPath = PathNormalizer.Normalize(path);
        _pending.Add(new DeleteEntry(normalizedPath));
    }

    /// <summary>
    /// Atomically applies all buffered operations to the main cache and write queue (AC-6.3).
    /// </summary>
    public void Commit()
    {
        ThrowIfDone();
        _pack.ApplyTransactionBatch(_pending);
        _committed = true;
    }

    /// <summary>
    /// Discards all buffered operations. The main cache and file are unaffected (AC-6.4).
    /// </summary>
    public void Rollback()
    {
        if (_committed) return;
        _pending.Clear();
    }

    /// <summary>
    /// Automatically rolls back if <see cref="Commit"/> was not called (AC-6.5).
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        if (!_committed)
            Rollback();
    }

    // ─── Private helpers ──────────────────────────────────────────────────────

    private void ThrowIfDone()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        if (_committed)
            throw new InvalidOperationException("Transaction has already been committed.");
    }
}
