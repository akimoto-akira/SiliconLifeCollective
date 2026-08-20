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

using ForgeMind.Knowledge;
using SiliconLife.Collective;

namespace ForgeMind;

/// <summary>
/// Unreal Engine knowledge base ("textbook") tool.
/// Provides curated Q&amp;A answers and implementation solutions
/// (C++ / Blueprint / Material / other) organized by dynamic engine version
/// buckets (universal plus any "major.minor"). Backed by <see cref="UeKnowledgeStore"/>:
/// a read-only .spk pack in release stage, a folder tree in development stage.
/// </summary>
public class UnrealKnowledgeTool : ITool
{
    /// <summary>Maximum number of search results returned to the caller.</summary>
    private const int MaxSearchResults = 10;

    public string Name => "unreal_knowledge";

    public string Description =>
        "Unreal Engine knowledge base: curated answers and implementation solutions " +
        "(C++ / Blueprint / Material / other) organized by engine version buckets " +
        "(universal plus any version like 5.6). " +
        "Actions: 'list' (browse entries, optional 'category' filter), " +
        "'search' (keyword search, requires 'query'), " +
        "'get' (fetch the full lesson, requires 'id'). " +
        "Version scope comes from 'engineVersion', or from the project's .uproject when 'path' is given; " +
        "otherwise only universal knowledge is used.";

    public string GetDisplayName(Language language) => language switch
    {
        Language.ZhCN => "UE 知识库",
        Language.ZhHK => "UE 知識庫",
        Language.JaJP => "UE ナレッジベース",
        Language.KoKR => "UE 지식 베이스",
        _ => "UE Knowledge Base"
    };

    public Dictionary<string, object> GetParameterSchema()
    {
        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The action to perform: list, search, get",
                    ["enum"] = new[] { "list", "search", "get" }
                },
                ["category"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "List only: filter by category",
                    ["enum"] = new[] { KnowledgeCategory.Cpp, KnowledgeCategory.Blueprint, KnowledgeCategory.Material, KnowledgeCategory.Other }
                },
                ["query"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Search only: keywords to match against titles, keywords, questions and answers"
                },
                ["id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Get only: the entry id to fetch"
                },
                ["engineVersion"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Engine version bucket: 'universal' or any version like '5.6' (defaults to universal)"
                },
                ["path"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Optional project directory; its .uproject EngineAssociation selects the version bucket"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj))
            return ToolResult.Failed("Missing 'action' parameter");

        string action = actionObj?.ToString()?.ToLowerInvariant() ?? "";

        using UeKnowledgeStore? store = UeKnowledgeStore.Open();
        if (store == null)
        {
            return ToolResult.Failed(
                $"Knowledge base not available - no {UeKnowledgeStore.PackFileName} or {UeKnowledgeStore.FolderName}/ found next to the plugin");
        }

        (UeVersion target, string versionSource, List<string> notes) = ResolveTargetVersion(parameters);

        return action switch
        {
            "list" => ExecuteList(store, target, versionSource, notes, parameters),
            "search" => ExecuteSearch(store, target, versionSource, notes, parameters),
            "get" => ExecuteGet(store, target, versionSource, notes, parameters),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    // ===== action: list =====

    private static ToolResult ExecuteList(
        UeKnowledgeStore store, UeVersion target, string versionSource,
        List<string> notes, Dictionary<string, object> parameters)
    {
        string? category = parameters.TryGetValue("category", out object? categoryObj)
            ? categoryObj?.ToString()?.ToLowerInvariant()
            : null;

        IEnumerable<UeKnowledgeStore.ScopedEntry> entries = store.ListEntriesInScope(target);
        if (!string.IsNullOrEmpty(category))
            entries = entries.Where(e =>
                string.Equals(e.Entry.Category, category, StringComparison.OrdinalIgnoreCase));

        var list = entries
            .Select(scoped => new
            {
                id = scoped.Entry.Id,
                category = scoped.Entry.Category,
                title = scoped.Entry.Title,
                question = scoped.Entry.Question,
                version = scoped.Version.Bucket,
                requiresCompanion = scoped.Entry.RequiresCompanion
            })
            .ToArray();

        return ToolResult.Successful(
            $"Listed {list.Length} knowledge entries (scope: {ScopeName(target)})",
            new
            {
                knowledgeSource = store.SourceDescription,
                availableVersions = store.ListVersions().Select(v => v.Bucket).ToArray(),
                engineVersion = target.Bucket,
                versionSource,
                notes,
                entries = list
            });
    }

    // ===== action: search =====

    private static ToolResult ExecuteSearch(
        UeKnowledgeStore store, UeVersion target, string versionSource,
        List<string> notes, Dictionary<string, object> parameters)
    {
        string? query = parameters.TryGetValue("query", out object? queryObj)
            ? queryObj?.ToString()
            : null;

        if (string.IsNullOrWhiteSpace(query))
            return ToolResult.Failed("Missing 'query' parameter");

        var results = store.Search(target, query)
            .Take(MaxSearchResults)
            .Select(hit => new
            {
                id = hit.Scoped.Entry.Id,
                category = hit.Scoped.Entry.Category,
                title = hit.Scoped.Entry.Title,
                answer = hit.Scoped.Entry.Answer,
                version = hit.Scoped.Version.Bucket,
                requiresCompanion = hit.Scoped.Entry.RequiresCompanion,
                score = hit.Score
            })
            .ToArray();

        return ToolResult.Successful(
            $"Found {results.Length} matching entries for '{query}' (scope: {ScopeName(target)})",
            new
            {
                knowledgeSource = store.SourceDescription,
                engineVersion = target.Bucket,
                versionSource,
                notes,
                query,
                results
            });
    }

    // ===== action: get =====

    private static ToolResult ExecuteGet(
        UeKnowledgeStore store, UeVersion target, string versionSource,
        List<string> notes, Dictionary<string, object> parameters)
    {
        string? id = parameters.TryGetValue("id", out object? idObj)
            ? idObj?.ToString()
            : null;

        if (string.IsNullOrWhiteSpace(id))
            return ToolResult.Failed("Missing 'id' parameter");

        UeKnowledgeStore.ScopedEntry? scoped = store.FindInScope(target, id);
        if (scoped == null)
        {
            // Report where the entry lives when it exists outside the current scope
            IReadOnlyList<UeKnowledgeStore.ScopedEntry> elsewhere = store.FindEverywhere(id);
            if (elsewhere.Count > 0)
            {
                return ToolResult.Successful(
                    $"Entry '{id}' is not available for scope {ScopeName(target)} - it belongs to: " +
                    string.Join(", ", elsewhere.Select(e => e.Version.Bucket)),
                    new
                    {
                        foundInScope = false,
                        id,
                        engineVersion = target.Bucket,
                        versionSource,
                        notes,
                        availableIn = elsewhere.Select(e => e.Version.Bucket).ToArray()
                    });
            }

            return ToolResult.Failed($"Unknown knowledge entry id: {id}");
        }

        UeKnowledgeEntry entry = scoped.Value.Entry;
        string? body = store.ReadEntryBody(scoped.Value.Version, entry.Id);
        if (body == null)
            notes.Add($"Entry body file missing for '{entry.Id}'");

        if (entry.RequiresCompanion)
            notes.Add("This entry requires the ForgeMindForUE companion plugin — verify via unreal_project analyze (companionPlugin)");

        return ToolResult.Successful(
            $"Knowledge entry '{entry.Id}' ({entry.Category}, version {scoped.Value.Version.Bucket})",
            new
            {
                foundInScope = true,
                id = entry.Id,
                category = entry.Category,
                title = entry.Title,
                question = entry.Question,
                keywords = entry.Keywords,
                answer = entry.Answer,
                version = scoped.Value.Version.Bucket,
                requiresCompanion = entry.RequiresCompanion,
                related = entry.Related,
                body,
                engineVersion = target.Bucket,
                versionSource,
                notes
            });
    }

    // ===== Version resolution =====

    /// <summary>
    /// Resolves the target version bucket: explicit 'engineVersion' parameter wins;
    /// otherwise the project's .uproject EngineAssociation; otherwise universal.
    /// </summary>
    private static (UeVersion Target, string Source, List<string> Notes) ResolveTargetVersion(
        Dictionary<string, object> parameters)
    {
        var notes = new List<string>();

        if (parameters.TryGetValue("engineVersion", out object? versionObj) &&
            !string.IsNullOrWhiteSpace(versionObj?.ToString()))
        {
            return (new UeVersion(versionObj!.ToString()!), "parameter", notes);
        }

        if (parameters.TryGetValue("path", out object? pathObj) &&
            !string.IsNullOrWhiteSpace(pathObj?.ToString()))
        {
            try
            {
                var directory = new DirectoryInfo(pathObj.ToString()!);
                if (directory.Exists)
                {
                    string? association = UnrealProjectTool.GetEngineAssociation(directory);
                    if (association == null)
                    {
                        notes.Add("Not a valid UE project — universal knowledge only");
                    }
                    else if (Guid.TryParse(association, out _))
                    {
                        notes.Add("Source build detected (GUID association) — universal knowledge only");
                    }
                    else
                    {
                        return (UeVersion.FromEngineAssociation(association), "project file", notes);
                    }
                }
                else
                {
                    notes.Add("Project directory does not exist — universal knowledge only");
                }
            }
            catch (Exception ex)
            {
                notes.Add($"Invalid path - universal knowledge only ({ex.Message})");
            }
        }

        return (UeVersion.Universal, "default", notes);
    }

    private static string ScopeName(UeVersion target) =>
        target.IsUniversal ? "universal" : "universal + " + target.Bucket;
}
