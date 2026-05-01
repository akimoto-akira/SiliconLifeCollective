using System.Collections.Concurrent;

namespace SiliconLife.Speedy.Internal;

/// <summary>
/// In-memory directory index for a .spk file.
/// Maps normalized paths to their <see cref="DirectoryEntry"/> metadata.
/// Always resident in memory; listing operations never trigger disk I/O.
/// Thread-safe via <see cref="ConcurrentDictionary{TKey,TValue}"/>.
/// </summary>
internal sealed class DirectoryMap
{
    // Normalized path → entry metadata
    private readonly ConcurrentDictionary<string, DirectoryEntry> _entries = new(StringComparer.Ordinal);

    // ─── Basic CRUD ───────────────────────────────────────────────────────────

    /// <summary>
    /// Attempts to retrieve the <see cref="DirectoryEntry"/> for the given normalized path.
    /// </summary>
    public bool TryGet(string normalizedPath, out DirectoryEntry entry)
        => _entries.TryGetValue(normalizedPath, out entry!);

    /// <summary>
    /// Adds or updates the entry for the given normalized path.
    /// </summary>
    public void Set(string normalizedPath, DirectoryEntry entry)
        => _entries[normalizedPath] = entry;

    /// <summary>
    /// Removes the entry for the given normalized path.
    /// No-op if the path does not exist.
    /// </summary>
    public void Remove(string normalizedPath)
        => _entries.TryRemove(normalizedPath, out _);

    // ─── Listing ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns the normalized paths of all direct child entries under
    /// <paramref name="normalizedParent"/>.
    /// <para>
    /// A path is a direct child of <paramref name="normalizedParent"/> when:
    /// <list type="bullet">
    ///   <item>For non-root parent: the path starts with "<c>{parent}/</c>" and
    ///         contains no further '/' after the prefix.</item>
    ///   <item>For root (empty string): the path contains no '/' at all.</item>
    /// </list>
    /// </para>
    /// </summary>
    /// <param name="normalizedParent">
    /// The already-normalized parent path, or empty string / "/" for the root.
    /// </param>
    /// <returns>
    /// A read-only list of normalized entry paths that are direct children of the parent.
    /// </returns>
    public IReadOnlyList<string> ListChildren(string normalizedParent)
    {
        // Normalize the parent: treat "/" as root (empty string)
        var parent = normalizedParent == "/" ? string.Empty : (normalizedParent ?? string.Empty);

        var results = new List<string>();

        if (parent.Length == 0)
        {
            // Root: direct children have no '/' in their path
            foreach (var key in _entries.Keys)
            {
                if (!key.Contains('/'))
                    results.Add(key);
            }
        }
        else
        {
            var prefix = parent + "/";
            foreach (var key in _entries.Keys)
            {
                if (key.StartsWith(prefix, StringComparison.Ordinal))
                {
                    // Must be a direct child: no '/' after the prefix
                    var remainder = key.AsSpan(prefix.Length);
                    if (!remainder.Contains('/'))
                        results.Add(key);
                }
            }
        }

        return results;
    }

    /// <summary>
    /// Infers the names of direct sub-directories under <paramref name="normalizedParent"/>
    /// by examining the paths of all stored entries.
    /// <para>
    /// Directories are not stored explicitly; they are derived from entry paths.
    /// A sub-directory name is the first path segment after the parent prefix that
    /// is followed by at least one more segment.
    /// </para>
    /// </summary>
    /// <param name="normalizedParent">
    /// The already-normalized parent path, or empty string / "/" for the root.
    /// </param>
    /// <returns>
    /// A read-only list of distinct normalized sub-directory paths that are direct
    /// children of the parent (e.g. "config/profile" for parent "config").
    /// </returns>
    public IReadOnlyList<string> ListDirectories(string normalizedParent)
    {
        var parent = normalizedParent == "/" ? string.Empty : (normalizedParent ?? string.Empty);

        var dirs = new HashSet<string>(StringComparer.Ordinal);

        if (parent.Length == 0)
        {
            // Root: any path that contains '/' contributes a top-level directory
            foreach (var key in _entries.Keys)
            {
                var slashIndex = key.IndexOf('/');
                if (slashIndex > 0)
                    dirs.Add(key[..slashIndex]);
            }
        }
        else
        {
            var prefix = parent + "/";
            foreach (var key in _entries.Keys)
            {
                if (key.StartsWith(prefix, StringComparison.Ordinal))
                {
                    var remainder = key.AsSpan(prefix.Length);
                    var slashIndex = remainder.IndexOf('/');
                    if (slashIndex > 0)
                    {
                        // There is at least one more segment after the first — it's a sub-directory
                        var dirName = remainder[..slashIndex].ToString();
                        dirs.Add(parent + "/" + dirName);
                    }
                }
            }
        }

        return dirs.ToList();
    }

    // ─── Serialization ────────────────────────────────────────────────────────

    /// <summary>
    /// Returns a snapshot copy of the current directory as a plain
    /// <see cref="Dictionary{TKey,TValue}"/> suitable for MessagePack serialization.
    /// </summary>
    public Dictionary<string, DirectoryEntry> Snapshot()
        => new(_entries, StringComparer.Ordinal);

    /// <summary>
    /// Replaces the current in-memory state with the entries from
    /// <paramref name="serialized"/> (typically loaded from the Directory Region of a .spk file).
    /// All existing entries are discarded.
    /// </summary>
    public void LoadFrom(Dictionary<string, DirectoryEntry> serialized)
    {
        _entries.Clear();
        foreach (var (key, value) in serialized)
            _entries[key] = value;
    }
}
