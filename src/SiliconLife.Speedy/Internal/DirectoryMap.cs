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
/// In-memory directory index mapping normalized paths to their metadata.
/// Uses a sorted dictionary for O(log n) lookups.
/// </summary>
internal sealed class DirectoryMap
{
    private readonly Dictionary<string, DirectoryEntry> _entries = new(StringComparer.Ordinal);
    private readonly object _lock = new();

    /// <summary>
    /// Gets the number of entries in the directory.
    /// </summary>
    public int Count
    {
        get
        {
            lock (_lock)
                return _entries.Count;
        }
    }

    /// <summary>
    /// Tries to get an entry by normalized path.
    /// </summary>
    public bool TryGet(string normalizedPath, out DirectoryEntry entry)
    {
        lock (_lock)
            return _entries.TryGetValue(normalizedPath, out entry!);
    }

    /// <summary>
    /// Adds or updates an entry in the directory.
    /// </summary>
    public void Set(string normalizedPath, DirectoryEntry entry)
    {
        lock (_lock)
            _entries[normalizedPath] = entry;
    }

    /// <summary>
    /// Removes an entry from the directory.
    /// </summary>
    public void Remove(string normalizedPath)
    {
        lock (_lock)
            _entries.Remove(normalizedPath);
    }

    /// <summary>
    /// Lists all direct child entries under a directory path.
    /// </summary>
    public IReadOnlyList<string> ListChildren(string directoryPath)
    {
        lock (_lock)
        {
            var children = new List<string>();
            var prefix = string.IsNullOrEmpty(directoryPath) 
                ? "" 
                : directoryPath.EndsWith('/') 
                    ? directoryPath 
                    : directoryPath + "/";

            foreach (var path in _entries.Keys)
            {
                if (!path.StartsWith(prefix, StringComparison.Ordinal))
                    continue;

                var relativePath = path.Substring(prefix.Length);
                if (!relativePath.Contains('/'))
                    children.Add(path);
            }

            return children;
        }
    }

    /// <summary>
    /// Lists all direct sub-directories under a directory path.
    /// </summary>
    public IReadOnlyList<string> ListDirectories(string directoryPath)
    {
        lock (_lock)
        {
            var directories = new HashSet<string>();
            var prefix = string.IsNullOrEmpty(directoryPath) 
                ? "" 
                : directoryPath.EndsWith('/') 
                    ? directoryPath 
                    : directoryPath + "/";

            foreach (var path in _entries.Keys)
            {
                if (!path.StartsWith(prefix, StringComparison.Ordinal))
                    continue;

                var relativePath = path.Substring(prefix.Length);
                var slashIndex = relativePath.IndexOf('/');
                if (slashIndex >= 0)
                {
                    var dirName = relativePath.Substring(0, slashIndex);
                    var fullPath = string.IsNullOrEmpty(prefix.TrimEnd('/')) 
                        ? dirName 
                        : prefix.TrimEnd('/') + "/" + dirName;
                    directories.Add(fullPath);
                }
            }

            return directories.OrderBy(d => d).ToList();
        }
    }

    /// <summary>
    /// Returns a snapshot of all entries (thread-safe).
    /// </summary>
    public IReadOnlyDictionary<string, DirectoryEntry> Snapshot()
    {
        lock (_lock)
            return new Dictionary<string, DirectoryEntry>(_entries, StringComparer.Ordinal);
    }

    /// <summary>
    /// Loads entries from a dictionary (used during initialization).
    /// </summary>
    public void LoadFrom(IReadOnlyDictionary<string, DirectoryEntry> entries)
    {
        lock (_lock)
        {
            _entries.Clear();
            foreach (var (path, entry) in entries)
                _entries[path] = entry;
        }
    }
}
