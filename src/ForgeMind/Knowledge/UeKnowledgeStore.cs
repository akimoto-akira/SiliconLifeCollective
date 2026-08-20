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
/// Versioned Unreal Engine knowledge base ("textbook") facade.
/// <para>Backend selection depends on the build configuration, resolved next to
/// the plugin assembly: DEBUG builds always use the <c>Knowledge/</c> folder;
/// release builds prefer the read-only <c>ue-knowledge.spk</c> pack and fall
/// back to the <c>Knowledge/</c> folder when the pack is absent or empty.</para>
/// <para>Query scope for a target version is always
/// <c>universal ∪ target</c>; a same-id entry in the target bucket
/// shadows the universal one.</para>
/// </summary>
public sealed class UeKnowledgeStore : IDisposable
{
    /// <summary>File name of the release-stage read-only knowledge pack.</summary>
    public const string PackFileName = "ue-knowledge.spk";

    /// <summary>Directory name of the development-stage knowledge folder.</summary>
    public const string FolderName = "Knowledge";

    private readonly IKnowledgeSource _source;

    private UeKnowledgeStore(IKnowledgeSource source) => _source = source;

    /// <summary>Human-readable backend description (for diagnostics).</summary>
    public string SourceDescription => _source.Description;

    /// <summary>
    /// Temporary absolute-path override of the plugin base directory (the folder that
    /// contains <see cref="FolderName"/>). AppContext.BaseDirectory resolves to the HOST
    /// executable directory (not the plugin directory), so while the deployment layout
    /// is unsettled the store is forced to this exact location. Remove this override
    /// once the base directory resolves next to the plugin assembly.
    /// </summary>
    private const string ForcedBaseDirectory = @"D:\SiliconLifeCollective\PluginDemo\ForgeMind";

    /// <summary>
    /// Opens the knowledge store with the build-configuration dispatch
    /// (<see cref="Open(string)"/>) rooted at the temporary absolute path
    /// <see cref="ForcedBaseDirectory"/>.
    /// </summary>
    public static UeKnowledgeStore? Open() => Open(ForcedBaseDirectory);

    /// <summary>
    /// Opens the knowledge store found under <paramref name="baseDirectory"/>.
    /// DEBUG builds always use the development folder; release builds prefer the
    /// read-only .spk pack (<see cref="OpenPack"/>) and fall back to the same
    /// folder when the pack is absent or empty.
    /// Returns null when no backend is available.
    /// </summary>
    public static UeKnowledgeStore? Open(string baseDirectory)
    {
#if DEBUG
        // Development stage — always edit-friendly folder tree, even if a pack exists
        return OpenFolder(baseDirectory);
#else
        // Release stage — read-only pack wins; folder fallback when the pack is missing
        return OpenPack(baseDirectory) ?? OpenFolder(baseDirectory);
#endif
    }

    /// <summary>
    /// Opens the release-stage read-only asset pack (<see cref="PackFileName"/>) under
    /// <paramref name="baseDirectory"/>. Not implemented yet — the pack is an asset
    /// deliverable, so this stays empty and always returns null until the pack
    /// format/pipeline exists; callers then fall back to the folder backend.
    /// </summary>
    public static UeKnowledgeStore? OpenPack(string baseDirectory)
    {
        // TODO: release-stage asset pack (PackFileName) — reserved, intentionally empty.
        return null;
    }

    /// <summary>
    /// Opens the development-stage folder tree (<see cref="FolderName"/>) under
    /// <paramref name="baseDirectory"/>. Returns null when the folder does not exist.
    /// </summary>
    public static UeKnowledgeStore? OpenFolder(string baseDirectory)
    {
        try
        {
            string folderPath = Path.Combine(baseDirectory, FolderName);
            if (Directory.Exists(folderPath))
                return new UeKnowledgeStore(new FolderKnowledgeSource(folderPath));
        }
        catch
        {
            // Fall through — no knowledge base available
        }

        return null;
    }

    /// <summary>Lists all version buckets present in the backend.</summary>
    public IReadOnlyList<UeVersion> ListVersions() => _source.ListVersions();

    /// <summary>Lists the entry metadata of one single bucket.</summary>
    public IReadOnlyList<UeKnowledgeEntry> ListEntries(UeVersion version) => _source.ListEntries(version);

    /// <summary>
    /// Lists entries of the merged scope (universal ∪ target).
    /// A same-id entry in the target bucket shadows the universal one.
    /// </summary>
    public IReadOnlyList<ScopedEntry> ListEntriesInScope(UeVersion target)
    {
        var result = new List<ScopedEntry>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Target bucket first so it shadows universal entries with the same id
        foreach (UeKnowledgeEntry entry in _source.ListEntries(target))
        {
            if (UeVersion.IsValidId(entry.Id) && seen.Add(entry.Id))
                result.Add(new ScopedEntry(entry, target));
        }

        if (!target.IsUniversal)
        {
            foreach (UeKnowledgeEntry entry in _source.ListEntries(UeVersion.Universal))
            {
                if (UeVersion.IsValidId(entry.Id) && seen.Add(entry.Id))
                    result.Add(new ScopedEntry(entry, UeVersion.Universal));
            }
        }

        return result;
    }

    /// <summary>
    /// Finds an entry by id within the merged scope (universal ∪ target).
    /// Returns null when the id is absent from the scope.
    /// </summary>
    public ScopedEntry? FindInScope(UeVersion target, string id)
    {
        foreach (ScopedEntry scoped in ListEntriesInScope(target))
        {
            if (string.Equals(scoped.Entry.Id, id, StringComparison.OrdinalIgnoreCase))
                return scoped;
        }

        return null;
    }

    /// <summary>
    /// Finds an entry by id in any bucket — used to report where an
    /// out-of-scope entry lives.
    /// </summary>
    public IReadOnlyList<ScopedEntry> FindEverywhere(string id)
    {
        var result = new List<ScopedEntry>();

        foreach (UeVersion version in _source.ListVersions())
        {
            foreach (UeKnowledgeEntry entry in _source.ListEntries(version))
            {
                if (string.Equals(entry.Id, id, StringComparison.OrdinalIgnoreCase))
                    result.Add(new ScopedEntry(entry, version));
            }
        }

        return result;
    }

    /// <summary>Reads the Markdown body of an entry; null when missing.</summary>
    public string? ReadEntryBody(UeVersion version, string id) => _source.ReadEntryBody(version, id);

    /// <summary>
    /// Keyword search over the merged scope. Tokens are matched
    /// case-insensitively against id, keywords, title, question and answer;
    /// results are sorted by descending score.
    /// </summary>
    public IReadOnlyList<SearchHit> Search(UeVersion target, string query)
    {
        // ToCharArray() from a string constant instead of a char array initializer:
        // both [..] and new char[] {..} make Roslyn lower constant value-type arrays
        // through MemoryMarshal.CreateReadOnlySpan / Unsafe, and the host's plugin
        // security scanner rejects those TypeRefs
        string[] tokens = (query ?? "")
            .Split(" \t,，".ToCharArray(), StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(t => t.ToLowerInvariant())
            .ToArray();

        if (tokens.Length == 0)
            return [];

        var hits = new List<SearchHit>();

        foreach (ScopedEntry scoped in ListEntriesInScope(target))
        {
            int score = 0;
            UeKnowledgeEntry entry = scoped.Entry;

            foreach (string token in tokens)
            {
                if (entry.Id.Contains(token, StringComparison.OrdinalIgnoreCase))
                    score += 3;

                if (entry.Keywords.Any(k => k.Contains(token, StringComparison.OrdinalIgnoreCase)))
                    score += 3;

                if (entry.Title.Contains(token, StringComparison.OrdinalIgnoreCase))
                    score += 2;

                if (entry.Question.Contains(token, StringComparison.OrdinalIgnoreCase))
                    score += 1;

                if (entry.Answer.Contains(token, StringComparison.OrdinalIgnoreCase))
                    score += 1;
            }

            if (score > 0)
                hits.Add(new SearchHit(scoped, score));
        }

        return hits.OrderByDescending(h => h.Score).ToArray();
    }

    public void Dispose() => _source.Dispose();

    /// <summary>An entry together with the version bucket it was found in.</summary>
    public readonly record struct ScopedEntry(UeKnowledgeEntry Entry, UeVersion Version);

    /// <summary>A scored search result.</summary>
    public readonly record struct SearchHit(ScopedEntry Scoped, int Score);
}
