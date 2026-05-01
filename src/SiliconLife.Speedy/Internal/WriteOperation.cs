namespace SiliconLife.Speedy.Internal;

/// <summary>
/// Base record for all operations that can be enqueued in the <see cref="WriteQueue"/>.
/// </summary>
internal abstract record WriteOperation(string NormalizedPath);

/// <summary>
/// Represents a write (create or update) operation: appends <see cref="Data"/> to the
/// Data Region and updates the directory index.
/// </summary>
internal record WriteEntry(string NormalizedPath, byte[] Data, string ContentType)
    : WriteOperation(NormalizedPath);

/// <summary>
/// Represents a delete operation: marks the existing entry's space as free and removes
/// it from the directory index.
/// </summary>
internal record DeleteEntry(string NormalizedPath)
    : WriteOperation(NormalizedPath);
