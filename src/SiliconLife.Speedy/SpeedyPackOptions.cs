namespace SiliconLife.Speedy;

/// <summary>
/// Configuration options for a <see cref="SpeedyPack"/> instance.
/// </summary>
public sealed class SpeedyPackOptions
{
    /// <summary>
    /// Time-to-live for each entry in the in-memory cache.
    /// Expired entries are lazily evicted on next access.
    /// Default: 5 minutes.
    /// </summary>
    public TimeSpan EntryCacheTtl { get; init; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Maximum number of entries to keep in the in-memory cache.
    /// When exceeded, the least-recently-used entry is evicted.
    /// Default: 10,000.
    /// </summary>
    public int MaxCacheEntries { get; init; } = 10_000;

    /// <summary>
    /// Whether to enable per-entry compression.
    /// Default: false.
    /// </summary>
    public bool EnableCompression { get; init; } = false;

    /// <summary>
    /// Whether to open the pack in read-only mode.
    /// Default: false.
    /// </summary>
    public bool ReadOnly { get; init; } = false;
}
