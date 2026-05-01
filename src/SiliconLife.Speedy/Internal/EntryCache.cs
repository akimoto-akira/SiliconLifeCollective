using System.Collections.Concurrent;

namespace SiliconLife.Speedy.Internal;

/// <summary>
/// In-memory entry cache with TTL-based lazy eviction and LRU capacity eviction.
/// Thread-safe via <see cref="ConcurrentDictionary{TKey,TValue}"/>.
/// </summary>
/// <remarks>
/// Satisfies AC-4.1 through AC-4.5:
/// <list type="bullet">
///   <item>AC-4.1: Written entries are immediately readable from cache.</item>
///   <item>AC-4.2: Callers populate the cache on first disk read (lazy loading).</item>
///   <item>AC-4.3: TTL-expired entries are lazily evicted on next access.</item>
///   <item>AC-4.4: When count exceeds <c>maxEntries</c>, the LRU entry is evicted.</item>
///   <item>AC-4.5: <see cref="Invalidate"/> synchronously removes the entry.</item>
/// </list>
/// </remarks>
internal sealed class EntryCache
{
    // ─── Inner type ───────────────────────────────────────────────────────────

    private sealed class CacheItem
    {
        public byte[] Data { get; }
        public DateTime CreatedAt { get; }
        public DateTime LastAccessed { get; set; }

        public CacheItem(byte[] data, DateTime now)
        {
            Data = data;
            CreatedAt = now;
            LastAccessed = now;
        }
    }

    // ─── Fields ───────────────────────────────────────────────────────────────

    private readonly ConcurrentDictionary<string, CacheItem> _entries =
        new(StringComparer.Ordinal);

    private readonly int _maxEntries;
    private readonly TimeSpan _ttl;

    /// <summary>
    /// Provides the current UTC time. Defaults to <see cref="DateTime.UtcNow"/>.
    /// Inject a custom provider in tests to control time without sleeping.
    /// </summary>
    private readonly Func<DateTime> _utcNow;

    // ─── Constructor ──────────────────────────────────────────────────────────

    /// <summary>
    /// Initialises the cache with the given capacity and TTL.
    /// </summary>
    /// <param name="maxEntries">Maximum number of entries before LRU eviction kicks in.</param>
    /// <param name="ttl">Time-to-live for each entry. Expired entries are lazily removed.</param>
    /// <param name="utcNow">
    /// Optional time provider. Defaults to <c>() =&gt; DateTime.UtcNow</c>.
    /// Pass a custom delegate in unit tests to control time deterministically.
    /// </param>
    public EntryCache(int maxEntries, TimeSpan ttl, Func<DateTime>? utcNow = null)
    {
        if (maxEntries <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxEntries), "Must be greater than zero.");

        _maxEntries = maxEntries;
        _ttl = ttl;
        _utcNow = utcNow ?? (() => DateTime.UtcNow);
    }

    // ─── Public API ───────────────────────────────────────────────────────────

    /// <summary>
    /// Attempts to retrieve the cached bytes for <paramref name="normalizedPath"/>.
    /// Returns <c>false</c> (and removes the entry) if the entry has expired.
    /// Updates <c>LastAccessed</c> on a cache hit.
    /// </summary>
    public bool TryGet(string normalizedPath, out byte[] data)
    {
        if (_entries.TryGetValue(normalizedPath, out var item))
        {
            var now = _utcNow();

            // AC-4.3: Lazy TTL eviction
            if (now - item.CreatedAt > _ttl)
            {
                _entries.TryRemove(normalizedPath, out _);
                data = Array.Empty<byte>();
                return false;
            }

            // AC-4.1 / AC-4.4: Update LRU timestamp on hit
            item.LastAccessed = now;
            data = item.Data;
            return true;
        }

        data = Array.Empty<byte>();
        return false;
    }

    /// <summary>
    /// Stores <paramref name="data"/> under <paramref name="normalizedPath"/>.
    /// If the cache is at capacity, the least-recently-used entry is evicted first.
    /// </summary>
    public void Set(string normalizedPath, byte[] data)
    {
        var now = _utcNow();

        // AC-4.4: LRU eviction when at capacity (only when adding a brand-new key)
        if (!_entries.ContainsKey(normalizedPath) && _entries.Count >= _maxEntries)
            EvictLru();

        _entries[normalizedPath] = new CacheItem(data, now);
    }

    /// <summary>
    /// Removes the entry for <paramref name="normalizedPath"/> from the cache.
    /// No-op if the path is not cached. Satisfies AC-4.5.
    /// </summary>
    public void Invalidate(string normalizedPath)
        => _entries.TryRemove(normalizedPath, out _);

    /// <summary>
    /// Removes all entries from the cache.
    /// </summary>
    public void Clear()
        => _entries.Clear();

    // ─── Private helpers ──────────────────────────────────────────────────────

    /// <summary>
    /// Scans all entries and removes the one with the oldest <c>LastAccessed</c> timestamp.
    /// This is a best-effort scan; in a highly concurrent scenario the evicted entry
    /// may not be the globally oldest, but correctness (no data corruption) is preserved.
    /// </summary>
    private void EvictLru()
    {
        string? lruKey = null;
        DateTime lruTime = DateTime.MaxValue;

        foreach (var (key, item) in _entries)
        {
            if (item.LastAccessed < lruTime)
            {
                lruTime = item.LastAccessed;
                lruKey = key;
            }
        }

        if (lruKey is not null)
            _entries.TryRemove(lruKey, out _);
    }
}
