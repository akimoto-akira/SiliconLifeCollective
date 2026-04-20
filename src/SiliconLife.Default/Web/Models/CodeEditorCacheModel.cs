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

namespace SiliconLife.Default.Web.Models;

/// <summary>
/// Code Editor Cache Model - Manages code cache for editor instances
/// </summary>
public static class CodeEditorCacheModel
{
    private record CodeEntry(string Code, DateTime LastAccessTime, string Language);

    private static readonly ConcurrentDictionary<string, CodeEntry> _cache = new();
    private static readonly object _cleanupLock = new();
    private const int MaxCacheSize = 100;
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(30);

    /// <summary>
    /// Registers a new editor instance
    /// </summary>
    /// <param name="language">Programming language</param>
    /// <returns>Editor GUID</returns>
    public static string Register(string language)
    {
        var editorGuid = Guid.NewGuid().ToString("N");
        _cache[editorGuid] = new CodeEntry(string.Empty, DateTime.Now, language);
        Cleanup();
        return editorGuid;
    }

    /// <summary>
    /// Updates editor code content
    /// </summary>
    /// <param name="editorGuid">Editor GUID</param>
    /// <param name="code">Code content</param>
    /// <returns>Whether the update was successful</returns>
    public static bool Update(string editorGuid, string code)
    {
        if (!_cache.TryGetValue(editorGuid, out var entry))
            return false;

        _cache[editorGuid] = entry with { Code = code, LastAccessTime = DateTime.Now };
        return true;
    }

    /// <summary>
    /// Gets editor code content
    /// </summary>
    /// <param name="editorGuid">Editor GUID</param>
    /// <returns>Code content, returns null if not exists</returns>
    public static string? GetCode(string editorGuid)
    {
        if (!_cache.TryGetValue(editorGuid, out var entry))
            return null;

        _cache[editorGuid] = entry with { LastAccessTime = DateTime.Now };
        return entry.Code;
    }

    /// <summary>
    /// Gets editor language
    /// </summary>
    /// <param name="editorGuid">Editor GUID</param>
    /// <returns>Language identifier, returns null if not exists</returns>
    public static string? GetLanguage(string editorGuid)
    {
        return _cache.TryGetValue(editorGuid, out var entry) ? entry.Language : null;
    }

    /// <summary>
    /// Unregisters an editor instance
    /// </summary>
    /// <param name="editorGuid">Editor GUID</param>
    /// <returns>Whether the unregistration was successful</returns>
    public static bool Unregister(string editorGuid)
    {
        return _cache.TryRemove(editorGuid, out _);
    }

    /// <summary>
    /// Cleans up expired cache (LRU strategy)
    /// </summary>
    public static void Cleanup()
    {
        if (!Monitor.TryEnter(_cleanupLock))
            return;

        try
        {
            var now = DateTime.Now;
            var expiredKeys = _cache
                .Where(kvp => now - kvp.Value.LastAccessTime > CacheExpiration)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in expiredKeys)
            {
                _cache.TryRemove(key, out _);
            }

            if (_cache.Count > MaxCacheSize)
            {
                var keysToRemove = _cache
                    .OrderBy(kvp => kvp.Value.LastAccessTime)
                    .Take(_cache.Count - MaxCacheSize + 50)
                    .Select(kvp => kvp.Key)
                    .ToList();

                foreach (var key in keysToRemove)
                {
                    _cache.TryRemove(key, out _);
                }
            }
        }
        finally
        {
            Monitor.Exit(_cleanupLock);
        }
    }
}
