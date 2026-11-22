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
/// In-memory hierarchical directory index. Paths are stored as a tree, one
/// dictionary level per path segment, so lookups are O(path depth) and
/// <see cref="ListChildren"/> / <see cref="ListDirectories"/> only touch the
/// direct children of the requested node — independent of total entry count.
/// (The previous flat dictionary had to scan every key on every directory
/// enumeration, which degraded linearly as data grew.)
/// </summary>
/// <remarks>
/// A node may hold both an <see cref="DirectoryEntry"/> (when its path is
/// itself a stored key, e.g. "x/y" while "x/y/z" also exists) and child
/// nodes. Removing a node prunes it when it has neither an entry nor children,
/// so directory listings never expose ghost prefixes.
/// Thread-safe: all public methods are executed under internal lock protection.
/// </remarks>
internal sealed class DirectoryMap
{
    private sealed class Node
    {
        /// <summary>Entry stored at this exact path; null when only a directory prefix.</summary>
        public DirectoryEntry? Entry;

        /// <summary>Child segments (single path component each) → sub-tree.</summary>
        public readonly Dictionary<string, Node> Children = new(StringComparer.Ordinal);
    }

    private readonly Node _root = new();
    private readonly object _lock = new();
    private int _count;

    /// <summary>Gets the number of entries in the directory.</summary>
    public int Count
    {
        get
        {
            lock (_lock)
                return _count;
        }
    }

    /// <summary>
    /// Tries to get an entry by normalized path.
    /// </summary>
    public bool TryGet(string normalizedPath, out DirectoryEntry entry)
    {
        lock (_lock)
        {
            var node = FindNode(_root, normalizedPath);
            if (node?.Entry != null)
            {
                entry = node.Entry;
                return true;
            }
            entry = null!;
            return false;
        }
    }

    /// <summary>
    /// Adds or updates an entry in the directory, creating intermediate nodes as needed.
    /// </summary>
    public void Set(string normalizedPath, DirectoryEntry entry)
    {
        lock (_lock)
        {
            var node = EnsureNode(_root, normalizedPath);
            if (node.Entry == null)
                _count++;
            node.Entry = entry;
        }
    }

    /// <summary>
    /// Removes an entry from the directory, pruning now-empty ancestor nodes.
    /// </summary>
    public void Remove(string normalizedPath)
    {
        lock (_lock)
        {
            if (normalizedPath.Length == 0)
            {
                if (_root.Entry != null)
                {
                    _root.Entry = null;
                    _count--;
                }
                return;
            }

            var segments = normalizedPath.Split('/');
            RemoveInternal(_root, segments, 0);
        }
    }

    /// <summary>
    /// Recursively removes the entry and prunes empty nodes. Returns true when
    /// the caller's child node is now empty and should be detached.
    /// </summary>
    private bool RemoveInternal(Node node, string[] segments, int index)
    {
        if (index == segments.Length)
        {
            if (node.Entry != null)
            {
                node.Entry = null;
                _count--;
            }
            return node.Entry == null && node.Children.Count == 0;
        }

        if (!node.Children.TryGetValue(segments[index], out var child))
            return false;

        var prune = RemoveInternal(child, segments, index + 1);
        if (prune)
            node.Children.Remove(segments[index]);

        return node.Entry == null && node.Children.Count == 0;
    }

    /// <summary>
    /// Lists all direct child entries under a directory path.
    /// Only the requested node's children are examined — O(child count), not O(total entries).
    /// </summary>
    public IReadOnlyList<string> ListChildren(string directoryPath)
    {
        lock (_lock)
        {
            var result = new List<string>();
            var node = FindNode(_root, directoryPath);
            if (node == null)
                return result;

            foreach (var (segment, child) in node.Children)
            {
                if (child.Entry != null)
                    result.Add(JoinPath(directoryPath, segment));
            }
            return result;
        }
    }

    /// <summary>
    /// Lists all direct sub-directories under a directory path (sorted, matching
    /// the previous flat implementation's ordering). A node is a directory when
    /// it has children; nodes that are only leaves are not listed.
    /// </summary>
    public IReadOnlyList<string> ListDirectories(string directoryPath)
    {
        lock (_lock)
        {
            var result = new List<string>();
            var node = FindNode(_root, directoryPath);
            if (node == null)
                return result;

            foreach (var (segment, child) in node.Children)
            {
                if (child.Children.Count > 0)
                    result.Add(JoinPath(directoryPath, segment));
            }
            result.Sort(StringComparer.Ordinal);
            return result;
        }
    }

    /// <summary>
    /// Returns a flat snapshot of all entries (thread-safe). Used for compaction,
    /// FreeList rebuild and statistics.
    /// </summary>
    public IReadOnlyDictionary<string, DirectoryEntry> Snapshot()
    {
        lock (_lock)
        {
            var result = new Dictionary<string, DirectoryEntry>(StringComparer.Ordinal);
            CollectEntries(_root, "", result);
            return result;
        }
    }

    /// <summary>
    /// Returns the directory tree in its serializable v3 form (thread-safe).
    /// Used as the payload of the .spk Directory area on commit.
    /// </summary>
    public DirectoryNodeDto SnapshotDto()
    {
        lock (_lock)
        {
            return ToDto(_root);
        }
    }

    /// <summary>
    /// Loads entries from a flat dictionary (used after compaction to reload the rewritten Directory).
    /// </summary>
    public void LoadFrom(IReadOnlyDictionary<string, DirectoryEntry> entries)
    {
        lock (_lock)
        {
            _root.Children.Clear();
            _root.Entry = null;
            _count = 0;
            foreach (var (path, entry) in entries)
            {
                var node = EnsureNode(_root, path);
                if (node.Entry == null)
                    _count++;
                node.Entry = entry;
            }
        }
    }

    /// <summary>
    /// Loads the directory tree directly from a deserialized v3 DTO, rebuilding
    /// internal nodes with Ordinal comparers (thread-safe).
    /// </summary>
    public void LoadFromDto(DirectoryNodeDto root)
    {
        lock (_lock)
        {
            _root.Children.Clear();
            _root.Entry = null;
            _count = 0;

            if (root == null)
                return;

            _root.Entry = root.Entry;
            if (root.Entry != null)
                _count++;

            if (root.Children != null)
            {
                foreach (var (segment, childDto) in root.Children)
                {
                    var child = new Node();
                    _root.Children[segment] = child;
                    _count += LoadNode(child, childDto);
                }
            }
        }
    }

    /// <summary>Recursively rebuilds an internal node from a DTO; returns entries counted.</summary>
    private static int LoadNode(Node node, DirectoryNodeDto dto)
    {
        int count = 0;
        node.Entry = dto.Entry;
        if (dto.Entry != null)
            count++;

        if (dto.Children != null)
        {
            foreach (var (segment, childDto) in dto.Children)
            {
                var child = new Node();
                node.Children[segment] = child;
                count += LoadNode(child, childDto);
            }
        }
        return count;
    }

    /// <summary>
    /// Counts entries by content type directly on the tree, without building a
    /// flat snapshot (used by GetFileInfo for statistics).
    /// </summary>
    public (int Total, int Json, int Raw, int Text) CountByContentType()
    {
        lock (_lock)
        {
            int json = 0, raw = 0, text = 0;
            CountEntriesByType(_root, ref json, ref raw, ref text);
            return (json + raw + text, json, raw, text);
        }
    }

    /// <summary>Recursively tallies entry content types.</summary>
    private static void CountEntriesByType(Node node, ref int json, ref int raw, ref int text)
    {
        if (node.Entry != null)
        {
            switch (node.Entry.ContentType)
            {
                case "json": json++; break;
                case "raw": raw++; break;
                case "text": text++; break;
            }
        }
        foreach (var (_, child) in node.Children)
            CountEntriesByType(child, ref json, ref raw, ref text);
    }

    /// <summary>Builds a serializable v3 tree from a flat dictionary (used by compaction).</summary>
    public static DirectoryNodeDto BuildDto(IReadOnlyDictionary<string, DirectoryEntry> entries)
    {
        var root = new DirectoryNodeDto();
        foreach (var (path, entry) in entries)
        {
            var node = root;
            if (path.Length > 0)
            {
                foreach (var segment in path.Split('/'))
                {
                    node.Children ??= new Dictionary<string, DirectoryNodeDto>(StringComparer.Ordinal);
                    if (!node.Children.TryGetValue(segment, out var child))
                    {
                        child = new DirectoryNodeDto();
                        node.Children[segment] = child;
                    }
                    node = child;
                }
            }
            node.Entry = entry;
        }
        return root;
    }

    /// <summary>Descends to the node for the given path, or null when absent.</summary>
    private static Node? FindNode(Node root, string normalizedPath)
    {
        if (normalizedPath.Length == 0)
            return root;

        var node = root;
        foreach (var segment in normalizedPath.Split('/'))
        {
            if (!node.Children.TryGetValue(segment, out node))
                return null;
        }
        return node;
    }

    /// <summary>Descends to the node for the given path, creating missing nodes.</summary>
    private static Node EnsureNode(Node root, string normalizedPath)
    {
        if (normalizedPath.Length == 0)
            return root;

        var node = root;
        foreach (var segment in normalizedPath.Split('/'))
        {
            if (!node.Children.TryGetValue(segment, out var child))
            {
                child = new Node();
                node.Children[segment] = child;
            }
            node = child;
        }
        return node;
    }

    private static void CollectEntries(Node node, string prefix, Dictionary<string, DirectoryEntry> result)
    {
        if (node.Entry != null)
            result[prefix] = node.Entry;
        foreach (var (segment, child) in node.Children)
            CollectEntries(child, JoinPath(prefix, segment), result);
    }

    private static DirectoryNodeDto ToDto(Node node)
    {
        var dto = new DirectoryNodeDto { Entry = node.Entry };
        if (node.Children.Count > 0)
        {
            dto.Children = new Dictionary<string, DirectoryNodeDto>(StringComparer.Ordinal);
            foreach (var (segment, child) in node.Children)
                dto.Children[segment] = ToDto(child);
        }
        return dto;
    }

    private static string JoinPath(string prefix, string segment)
        => prefix.Length == 0 ? segment : prefix + "/" + segment;
}
