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

using System.Text;

namespace SiliconLife.Speedy.Internal;

/// <summary>
/// Normalizes virtual paths to ensure consistency across platforms.
/// Converts to lowercase, forward slashes, removes leading/trailing slashes.
/// </summary>
internal static class PathNormalizer
{
    private const char Separator = '/';

    /// <summary>
    /// Normalizes a virtual path for consistent internal representation.
    /// </summary>
    public static string Normalize(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return string.Empty;

        // Replace backslashes with forward slashes
        var normalized = path.Replace('\\', Separator);

        // Convert to lowercase for case-insensitive comparison
        normalized = normalized.ToLowerInvariant();

        // Remove leading and trailing slashes
        normalized = normalized.Trim(Separator);

        // Collapse multiple consecutive slashes
        while (normalized.Contains($"{Separator}{Separator}"))
        {
            normalized = normalized.Replace($"{Separator}{Separator}", $"{Separator}");
        }

        return normalized;
    }

    /// <summary>
    /// Extracts the parent directory path from a given path.
    /// </summary>
    public static string GetParent(string path)
    {
        var normalized = Normalize(path);
        if (string.IsNullOrEmpty(normalized))
            return string.Empty;

        var lastSlash = normalized.LastIndexOf(Separator);
        return lastSlash < 0 ? string.Empty : normalized.Substring(0, lastSlash);
    }

    /// <summary>
    /// Extracts the file/entry name from a path.
    /// </summary>
    public static string GetName(string path)
    {
        var normalized = Normalize(path);
        if (string.IsNullOrEmpty(normalized))
            return string.Empty;

        var lastSlash = normalized.LastIndexOf(Separator);
        return lastSlash < 0 ? normalized : normalized.Substring(lastSlash + 1);
    }

    /// <summary>
    /// Combines multiple path segments into a single normalized path.
    /// </summary>
    public static string Combine(params string[] paths)
    {
        if (paths == null || paths.Length == 0)
            return string.Empty;

        var builder = new StringBuilder();
        foreach (var path in paths)
        {
            if (string.IsNullOrWhiteSpace(path))
                continue;

            var trimmed = path.Trim(Separator);
            if (builder.Length > 0 && !string.IsNullOrEmpty(trimmed))
            {
                builder.Append(Separator);
            }
            builder.Append(trimmed);
        }

        return Normalize(builder.ToString());
    }
}
