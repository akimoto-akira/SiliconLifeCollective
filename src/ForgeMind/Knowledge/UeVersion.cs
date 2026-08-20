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

namespace ForgeMind.Knowledge;

/// <summary>
/// Engine version bucket for knowledge entries.
/// Fully dynamic: any version string ("major.minor") forms a bucket at runtime,
/// so new engine releases never require a code change.
/// The only fixed concept is <see cref="Universal"/> — knowledge applicable
/// to every engine version.
/// </summary>
public readonly struct UeVersion : IEquatable<UeVersion>
{
    /// <summary>Bucket name for knowledge applicable to every engine version.</summary>
    public const string UniversalBucket = "universal";

    /// <summary>Knowledge applicable to all engine versions.</summary>
    public static readonly UeVersion Universal = new(UniversalBucket);

    /// <summary>Normalized bucket name ("universal" or "major.minor").</summary>
    public string Bucket { get; }

    /// <summary>Creates a version bucket from a raw name (trimmed, lowercased).</summary>
    public UeVersion(string bucket)
    {
        Bucket = (bucket ?? UniversalBucket).Trim().ToLowerInvariant();
        if (Bucket.Length == 0)
            Bucket = UniversalBucket;
    }

    /// <summary>Whether this is the universal bucket.</summary>
    public bool IsUniversal => Bucket == UniversalBucket;

    /// <summary>Whether the bucket name is a valid directory/segment name.</summary>
    public bool IsValidBucketName => IsValidId(Bucket);

    /// <summary>
    /// Maps a .uproject EngineAssociation to a version bucket.
    /// Version strings like "5.6" or "5.6.1" map to their "major.minor" bucket;
    /// GUID associations (source builds) and empty values map to <see cref="Universal"/>.
    /// </summary>
    public static UeVersion FromEngineAssociation(string? association)
    {
        if (string.IsNullOrWhiteSpace(association))
            return Universal;

        if (Guid.TryParse(association.Trim(), out _))
            return Universal;

        // Accept "major.minor(.patch)" and normalize to "major.minor"
        string[] parts = association.Trim().Split('.');
        if (parts.Length >= 2 && int.TryParse(parts[0], out _) && int.TryParse(parts[1], out _))
            return new UeVersion(parts[0] + "." + parts[1]);

        return Universal;
    }

    /// <summary>
    /// Whether a name is safe to use as an entry id or bucket name
    /// (letters, digits, underscore, hyphen, dot — no path traversal).
    /// </summary>
    internal static bool IsValidId(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        foreach (char c in value)
        {
            if (!(char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == '.'))
                return false;
        }

        return value != "." && value != "..";
    }

    public bool Equals(UeVersion other) => Bucket == other.Bucket;

    public override bool Equals(object? obj) => obj is UeVersion other && Equals(other);

    public override int GetHashCode() => Bucket.GetHashCode();

    public override string ToString() => Bucket;

    public static bool operator ==(UeVersion left, UeVersion right) => left.Equals(right);

    public static bool operator !=(UeVersion left, UeVersion right) => !left.Equals(right);
}
