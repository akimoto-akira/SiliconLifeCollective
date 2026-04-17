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

using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Tool for managing silicon being memory/notes.
/// </summary>
public class MemoryTool : ITool
{
    /// <inheritdoc/>
    public string Name => "memory";

    /// <inheritdoc/>
    public string Description =>
        "Manage memory/notes for the silicon being. " +
        "Actions: " +
        "'add' (add a new memory entry); " +
        "'recent' (get recent memories, optional max_results); " +
        "'query' (query memories by time range: requires 'year', optional 'month', 'day', 'hour', 'minute', 'second', optional 'max_results'); " +
        "'stats' (get memory statistics).";

    /// <inheritdoc/>
    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

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
                    ["description"] = "The action to perform: add, recent, query, stats",
                    ["enum"] = new[] { "add", "recent", "query", "stats" }
                },
                ["content"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Memory content to add (required for action=add)"
                },
                ["max_results"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum number of results to return (used with recent and query)"
                },
                ["year"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Year to query (optional for action=query; omit to query all entries)"
                },
                ["month"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Month to query, 1–12 (optional, used with action=query)"
                },
                ["day"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Day to query, 1–31 (optional, used with action=query)"
                },
                ["hour"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Hour to query, 0–23 (optional, used with action=query)"
                },
                ["minute"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Minute to query, 0–59 (optional, used with action=query)"
                },
                ["second"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Second to query, 0–59 (optional, used with action=query)"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        SiliconBeingBase? being = GetSiliconBeing(callerId);
        DefaultLocalizationBase loc = GetLocalization();

        if (being?.Memory == null)
            return ToolResult.Failed(loc.MemoryToolNotAvailable);

        if (!parameters.TryGetValue("action", out object? actionObj))
            return ToolResult.Failed(loc.MemoryToolMissingAction);

        string action = actionObj?.ToString()?.ToLowerInvariant() ?? "";

        try
        {
            return action switch
            {
                "add"    => ExecuteAdd(being, parameters, loc),
                "recent" => ExecuteRecent(being, parameters, loc),
                "query"  => ExecuteQuery(being, parameters, loc),
                "stats"  => ExecuteStats(being, loc),
                _        => ToolResult.Failed(loc.MemoryToolUnknownAction(action))
            };
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"{loc.ErrorMessage}: {ex.Message}");
        }
    }

    private static ToolResult ExecuteAdd(SiliconBeingBase being, Dictionary<string, object> parameters, DefaultLocalizationBase loc)
    {
        if (!parameters.TryGetValue("content", out object? contentObj) || string.IsNullOrWhiteSpace(contentObj?.ToString()))
            return ToolResult.Failed(loc.MemoryToolMissingContent);

        var entry = being.Memory!.Add(contentObj!.ToString()!);
        return ToolResult.Successful($"Memory added (ID: {entry.Id})");
    }

    private static ToolResult ExecuteRecent(SiliconBeingBase being, Dictionary<string, object> parameters, DefaultLocalizationBase loc)
    {
        int count = 10;
        if (parameters.TryGetValue("max_results", out object? countObj) && int.TryParse(countObj?.ToString(), out int c))
            count = c;

        List<MemoryEntry> memories = being.Memory!.GetRecent(count);

        if (memories.Count == 0)
            return ToolResult.Successful(loc.MemoryToolNoMemories);

        var lines = new List<string> { loc.MemoryToolRecentHeader(memories.Count) };
        foreach (MemoryEntry memory in memories)
            lines.Add($"- [{memory.Timestamp}] {memory.Content}");

        return ToolResult.Successful(string.Join("\n", lines));
    }

    private static ToolResult ExecuteQuery(SiliconBeingBase being, Dictionary<string, object> parameters, DefaultLocalizationBase loc)
    {
        int? year   = TryGetInt(parameters, "year");
        int? month  = TryGetInt(parameters, "month");
        int? day    = TryGetInt(parameters, "day");
        int? hour   = TryGetInt(parameters, "hour");
        int? minute = TryGetInt(parameters, "minute");
        int? second = TryGetInt(parameters, "second");

        int maxResults = 0;
        if (parameters.TryGetValue("max_results", out object? mrObj) && int.TryParse(mrObj?.ToString(), out int mr))
            maxResults = mr;

        List<MemoryEntry> memories;
        string rangeDesc;

        if (year == null)
        {
            memories = being.Memory!.QueryAll(maxResults);
            rangeDesc = "*";
        }
        else
        {
            IncompleteDate range;
            try
            {
                range = new IncompleteDate(year.Value, month, day, hour, minute, second);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return ToolResult.Failed($"{loc.MemoryToolInvalidYear}: {ex.ParamName}");
            }

            memories = being.Memory!.Query(range, maxResults);
            rangeDesc = range.ToString();
        }

        if (memories.Count == 0)
            return ToolResult.Successful(loc.MemoryToolQueryNoResults);

        var lines = new List<string> { loc.MemoryToolQueryHeader(memories.Count, rangeDesc) };
        foreach (MemoryEntry memory in memories)
            lines.Add($"- [{memory.Timestamp}] {memory.Content}");

        return ToolResult.Successful(string.Join("\n", lines));
    }

    private static ToolResult ExecuteStats(SiliconBeingBase being, DefaultLocalizationBase loc)
    {
        MemoryStatistics stats = being.Memory!.GetStatistics();

        var lines = new List<string>
        {
            loc.MemoryToolStatsHeader,
            $"{loc.MemoryToolStatsTotal}: {stats.TotalEntries}",
            $"{loc.MemoryToolStatsOldest}: {stats.OldestEntry?.ToString() ?? loc.MemoryToolStatsNA}",
            $"{loc.MemoryToolStatsNewest}: {stats.NewestEntry?.ToString() ?? loc.MemoryToolStatsNA}"
        };

        return ToolResult.Successful(string.Join("\n", lines));
    }

    private static SiliconBeingBase? GetSiliconBeing(Guid callerId)
        => ServiceLocator.Instance.BeingManager?.GetBeing(callerId);

    private static DefaultLocalizationBase GetLocalization()
    {
        Language language = Config.Instance?.Data?.Language ?? Language.ZhCN;
        if (LocalizationManager.Instance.TryGetLocalization(language, out LocalizationBase? loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc;
        // fallback — should never happen in practice
        return new ZhCN();
    }

    private static int? TryGetInt(Dictionary<string, object> parameters, string key)
        => parameters.TryGetValue(key, out object? v) && int.TryParse(v?.ToString(), out int n) ? n : null;
}
