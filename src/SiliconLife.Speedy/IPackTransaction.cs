namespace SiliconLife.Speedy;

/// <summary>
/// Represents an atomic transaction over a <see cref="SpeedyPack"/> instance.
/// Operations are buffered locally until <see cref="Commit"/> is called.
/// Disposing without committing automatically rolls back.
/// </summary>
public interface IPackTransaction : IDisposable
{
    /// <summary>Writes raw bytes to <paramref name="path"/> within the transaction.</summary>
    void Write(string path, ReadOnlySpan<byte> data);

    /// <summary>Writes raw bytes to <paramref name="path"/> within the transaction.</summary>
    void Write(string path, byte[] data);

    /// <summary>Serializes <paramref name="value"/> as JSON and writes it to <paramref name="path"/>.</summary>
    void Write<T>(string path, T value);

    /// <summary>Marks <paramref name="path"/> for deletion within the transaction.</summary>
    void Delete(string path);

    /// <summary>
    /// Atomically commits all buffered operations to the main cache and write queue.
    /// After commit, changes are immediately visible to readers.
    /// </summary>
    void Commit();

    /// <summary>
    /// Discards all buffered operations. The main cache and file are unaffected.
    /// </summary>
    void Rollback();

    /// <summary>Whether <see cref="Commit"/> has been called successfully.</summary>
    bool IsCommitted { get; }
}
