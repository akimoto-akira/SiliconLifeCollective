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

namespace SiliconLife.Speedy.Internal;

/// <summary>
/// In-memory cache with TTL and LRU eviction strategies.
/// Supports "pinned" entries which are not evicted — used to keep
/// not-yet-persisted writes alive until the WriteQueue flushes them.
/// </summary>
internal sealed class EntryCache
{
    private readonly int _maxEntries;
    private readonly TimeSpan _ttl;
    private readonly Dictionary<string, CacheEntry> _cache = new(StringComparer.Ordinal);
    private readonly object _lock = new();

    private sealed class CacheEntry
    {
        public byte[] Data { get; }
        public DateTime ExpiresAt { get; set; }
        public DateTime LastAccessed { get; set; }
        public bool Pinned { get; set; }

        public CacheEntry(byte[] data, DateTime expiresAt, bool pinned)
        {
            Data = data;
            ExpiresAt = expiresAt;
            LastAccessed = DateTime.UtcNow;
            Pinned = pinned;
        }
    }

    public EntryCache(int maxEntries, TimeSpan ttl)
    {
        _maxEntries = maxEntries;
        _ttl = ttl;
    }

    /// <summary>
    /// Sets a cache entry, (re)starting the TTL timer.
    /// When <paramref name="pinned"/> is true, the entry is excluded from
    /// TTL expiration and LRU eviction until <see cref="Unpin"/> is called.
    /// </summary>
    public void Set(string normalizedPath, byte[] data, bool pinned = false)
    {
        lock (_lock)
        {
            var expiresAt = DateTime.UtcNow.Add(_ttl);
            _cache[normalizedPath] = new CacheEntry(data, expiresAt, pinned);
            EvictIfNecessary();
        }
    }

    /// <summary>
    /// Tries to get a cached entry. Resets TTL timer on successful access.
    /// Expired (non-pinned) entries are lazily evicted here.
    /// </summary>
    public bool TryGet(string normalizedPath, out byte[] data)
    {
        lock (_lock)
        {
            data = Array.Empty<byte>();

            if (!_cache.TryGetValue(normalizedPath, out var entry))
                return false;

            // Non-pinned expired entries are lazily evicted.
            if (!entry.Pinned && DateTime.UtcNow > entry.ExpiresAt)
            {
                _cache.Remove(normalizedPath);
                return false;
            }

            // Reset TTL timer and LRU timestamp on read access (per spec).
            var now = DateTime.UtcNow;
            entry.LastAccessed = now;
            entry.ExpiresAt = now.Add(_ttl);
            data = entry.Data;
            return true;
        }
    }

    /// <summary>
    /// Removes a specific cache entry regardless of pin state.
    /// </summary>
    public void Invalidate(string normalizedPath)
    {
        lock (_lock)
            _cache.Remove(normalizedPath);
    }

    /// <summary>
    /// Releases the pin on an entry, allowing it to be evicted normally.
    /// Called by the WriteQueue after successful persistence.
    /// </summary>
    public void Unpin(string normalizedPath)
    {
        lock (_lock)
        {
            if (_cache.TryGetValue(normalizedPath, out var entry))
            {
                entry.Pinned = false;
                entry.LastAccessed = DateTime.UtcNow;
                entry.ExpiresAt = DateTime.UtcNow.Add(_ttl);
                // After unpinning, size limit might need enforcement.
                EvictIfNecessary();
            }
        }
    }

    /// <summary>
    /// Evicts non-pinned entries if cache exceeds maximum capacity (LRU).
    /// Pinned entries are always retained so pending writes never get lost.
    /// </summary>
    private void EvictIfNecessary()
    {
        while (_cache.Count > _maxEntries)
        {
            string? lruKey = null;
            DateTime oldestAccess = DateTime.MaxValue;

            foreach (var (key, entry) in _cache)
            {
                if (entry.Pinned)
                    continue;

                if (entry.LastAccessed < oldestAccess)
                {
                    oldestAccess = entry.LastAccessed;
                    lruKey = key;
                }
            }

            if (lruKey == null)
                break; // All remaining entries are pinned — can't evict.

            _cache.Remove(lruKey);
        }
    }
}
