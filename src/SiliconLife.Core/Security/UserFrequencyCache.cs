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

namespace SiliconLife.Collective;

/// <summary>
/// A cached user permission decision with expiration.
/// </summary>
public sealed class CachedPermission
{
    /// <summary>The permission type</summary>
    public PermissionType PermissionType { get; }

    /// <summary>The resource prefix that was decided on</summary>
    public string ResourcePrefix { get; }

    /// <summary>The user's decision</summary>
    public PermissionResult Result { get; }

    /// <summary>When this cache entry was created</summary>
    public DateTime CreatedAt { get; }

    /// <summary>When this cache entry expires</summary>
    public DateTime ExpiresAt { get; }

    public CachedPermission(PermissionType permissionType, string resourcePrefix, PermissionResult result, DateTime expiresAt)
    {
        PermissionType = permissionType;
        ResourcePrefix = resourcePrefix;
        Result = result;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
    }

    /// <summary>Checks if this cache entry has expired</summary>
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;
}

/// <summary>
/// High-frequency permission decision cache.
/// Users explicitly opt-in to caching (not auto-detected).
/// Prefix matching. Memory-only, configurable expiration.
///
/// Two lists:
/// - HighAllow: frequently allowed resources
/// - HighDeny: frequently denied resources
/// </summary>
public class UserFrequencyCache
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<UserFrequencyCache>();
    private readonly List<CachedPermission> _cache = new();
    private readonly TimeSpan _expiration;
    private readonly object _lock = new();

    /// <summary>
    /// Gets the number of cached entries
    /// </summary>
    public int Count
    {
        get { lock (_lock) { return _cache.Count; } }
    }

    /// <summary>
    /// Creates a new UserFrequencyCache
    /// </summary>
    /// <param name="expiration">Cache entry expiration time (default: 1 hour)</param>
    public UserFrequencyCache(TimeSpan? expiration = null)
    {
        _expiration = expiration ?? TimeSpan.FromHours(1);
    }

    /// <summary>
    /// Records a user's permission decision into the cache.
    /// If an existing entry for the same (type, prefix) exists, it is updated.
    /// </summary>
    /// <param name="permissionType">The permission type</param>
    /// <param name="resource">The resource path (prefix-stored)</param>
    /// <param name="result">The user's decision</param>
    /// <param name="addToCache">If false, the decision is NOT cached (user did not opt in)</param>
    public void Record(PermissionType permissionType, string resource, PermissionResult result, bool addToCache = true)
    {
        if (!addToCache) return;

        lock (_lock)
        {
            for (int i = 0; i < _cache.Count; i++)
            {
                CachedPermission entry = _cache[i];
                if (entry.PermissionType == permissionType &&
                    resource.StartsWith(entry.ResourcePrefix, StringComparison.OrdinalIgnoreCase))
                {
                    _cache[i] = new CachedPermission(permissionType, resource, result, DateTime.UtcNow + _expiration);
                    _logger.Debug("Frequency cache recorded: type={Type}, resource={Resource}, result={Result}", permissionType, resource, result);
                    return;
                }
            }

            _cache.Add(new CachedPermission(permissionType, resource, result, DateTime.UtcNow + _expiration));
            _logger.Debug("Frequency cache recorded: type={Type}, resource={Resource}, result={Result}", permissionType, resource, result);
            CleanExpired();
        }
    }

    /// <summary>
    /// Queries the cache for a matching entry.
    /// Uses prefix matching: checks if the resource starts with any cached prefix.
    /// </summary>
    /// <param name="permissionType">The permission type</param>
    /// <param name="resource">The resource path to check</param>
    /// <returns>The cached result, or null if no match (or expired)</returns>
    public PermissionResult? Query(PermissionType permissionType, string resource)
    {
        lock (_lock)
        {
            foreach (CachedPermission entry in _cache)
            {
                if (entry.IsExpired) continue;

                if (entry.PermissionType == permissionType &&
                    resource.StartsWith(entry.ResourcePrefix, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.Trace("Frequency cache hit: type={Type}, resource={Resource}, result={Result}", permissionType, resource, entry.Result);
                    return entry.Result;
                }
            }
        }
        _logger.Trace("Frequency cache miss: type={Type}, resource={Resource}", permissionType, resource);
        return null;
    }

    /// <summary>
    /// Removes a specific cached entry by prefix
    /// </summary>
    /// <param name="permissionType">The permission type</param>
    /// <param name="resourcePrefix">The resource prefix to remove</param>
    /// <returns>True if an entry was removed</returns>
    public bool Remove(PermissionType permissionType, string resourcePrefix)
    {
        lock (_lock)
        {
            int removed = _cache.RemoveAll(e =>
                e.PermissionType == permissionType &&
                e.ResourcePrefix.Equals(resourcePrefix, StringComparison.OrdinalIgnoreCase));
            return removed > 0;
        }
    }

    /// <summary>
    /// Clears all cached entries
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _cache.Clear();
        }
    }

    /// <summary>
    /// Gets all cached entries (for display / management)
    /// </summary>
    public List<CachedPermission> GetAll()
    {
        lock (_lock)
        {
            CleanExpired();
            return _cache.ToList();
        }
    }

    /// <summary>
    /// Cleans up expired entries
    /// </summary>
    private void CleanExpired()
    {
        int removed = _cache.RemoveAll(e => e.IsExpired);
        if (removed > 0)
        {
            _logger.Debug("Frequency cache cleanup: removed {Count} expired entries", removed);
        }
    }
}
