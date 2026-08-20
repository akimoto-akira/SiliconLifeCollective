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

using System.Text.Json;

namespace ForgeMind.Knowledge;

/// <summary>
/// Shared JSON handling for knowledge index files.
/// </summary>
internal static class KnowledgeJson
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>Parses an index.json payload; returns an empty index on failure.</summary>
    public static KnowledgeIndex ParseIndex(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new KnowledgeIndex();

        try
        {
            return JsonSerializer.Deserialize<KnowledgeIndex>(json, Options) ?? new KnowledgeIndex();
        }
        catch
        {
            return new KnowledgeIndex();
        }
    }
}

/// <summary>
/// Development-stage knowledge backend: reads the textbook straight from a
/// folder tree — <c>{version}/index.json</c> plus <c>{version}/entries/{id}.md</c>.
/// </summary>
internal sealed class FolderKnowledgeSource : IKnowledgeSource
{
    private readonly string _root;

    public FolderKnowledgeSource(string root) => _root = root;

    public string Description => "folder:" + _root;

    public IReadOnlyList<UeVersion> ListVersions()
    {
        try
        {
            return Directory.GetDirectories(_root)
                .Select(dir => new UeVersion(Path.GetFileName(dir)))
                .Where(version => version.IsValidBucketName)
                .ToArray();
        }
        catch
        {
            return [];
        }
    }

    public IReadOnlyList<UeKnowledgeEntry> ListEntries(UeVersion version)
    {
        if (!version.IsValidBucketName)
            return [];

        string indexFile = Path.Combine(_root, version.Bucket, "index.json");
        if (!File.Exists(indexFile))
            return [];

        try
        {
            string? json = UnrealTextReader.LoadStringAuto(new FileInfo(indexFile));
            return KnowledgeJson.ParseIndex(json).Entries;
        }
        catch
        {
            return [];
        }
    }

    public string? ReadEntryBody(UeVersion version, string id)
    {
        if (!version.IsValidBucketName || !UeVersion.IsValidId(id))
            return null;

        string bodyFile = Path.Combine(_root, version.Bucket, "entries", id + ".md");
        if (!File.Exists(bodyFile))
            return null;

        try
        {
            return UnrealTextReader.LoadStringAuto(new FileInfo(bodyFile));
        }
        catch
        {
            return null;
        }
    }

    public void Dispose()
    {
        // No resources to release
    }
}
