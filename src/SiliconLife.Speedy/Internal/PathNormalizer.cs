namespace SiliconLife.Speedy.Internal;

/// <summary>
/// Provides path normalization utilities for SpeedyPack entry paths.
/// All paths are normalized to lowercase, forward-slash separated, with no leading/trailing slashes,
/// and with all ".." traversal segments removed.
/// </summary>
internal static class PathNormalizer
{
    /// <summary>
    /// Normalizes a path string:
    /// - Returns empty string for null/empty input
    /// - Replaces backslashes with forward slashes
    /// - Splits by '/' and filters out empty segments, '.' segments, and '..' segments
    /// - Joins remaining segments with '/'
    /// - Converts to lowercase
    /// </summary>
    /// <param name="path">The raw path string to normalize.</param>
    /// <returns>A normalized path string, never starting or ending with '/'.</returns>
    public static string Normalize(string? path)
    {
        if (string.IsNullOrEmpty(path))
            return string.Empty;

        // Replace backslashes with forward slashes
        var normalized = path.Replace('\\', '/');

        // Split, filter out empty/dot/dotdot segments, then join
        var segments = normalized
            .Split('/')
            .Where(s => s.Length > 0 && s != "." && s != "..")
            .ToArray();

        return string.Join('/', segments).ToLowerInvariant();
    }

    /// <summary>
    /// Returns the parent directory portion of an already-normalized path.
    /// If the path has no '/' (root-level entry), returns empty string.
    /// </summary>
    /// <param name="normalizedPath">An already-normalized path (output of <see cref="Normalize"/>).</param>
    /// <returns>
    /// The parent directory path, or empty string if the entry is at the root level.
    /// </returns>
    /// <example>
    /// "config/profile/settings" → "config/profile"
    /// "config/profile"         → "config"
    /// "config"                 → ""
    /// ""                       → ""
    /// </example>
    public static string GetParentDirectory(string normalizedPath)
    {
        if (string.IsNullOrEmpty(normalizedPath))
            return string.Empty;

        var lastSlash = normalizedPath.LastIndexOf('/');
        return lastSlash < 0 ? string.Empty : normalizedPath[..lastSlash];
    }
}
