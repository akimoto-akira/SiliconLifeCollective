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
        "Actions: 'add' (add a new memory), 'recent' (get recent memories), 'stats' (get memory statistics).";

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
                    ["description"] = "The action to perform: add, recent, stats",
                    ["enum"] = new[] { "add", "recent", "stats" }
                },
                ["content"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The memory content to add (used with action=add)"
                },
                ["max_results"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum number of results (used with action=recent)"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        var being = GetSiliconBeing(callerId);
        if (being?.Memory == null)
        {
            return ToolResult.Failed("Memory system not available");
        }

        if (!parameters.TryGetValue("action", out object? actionObj))
        {
            return ToolResult.Failed("Missing 'action' parameter");
        }

        string action = actionObj?.ToString() ?? "";

        try
        {
            return action.ToLowerInvariant() switch
            {
                "add" => ExecuteAdd(being, parameters),
                "recent" => ExecuteRecent(being, parameters),
                "stats" => ExecuteStats(being, parameters),
                _ => ToolResult.Failed($"Unknown action: {action}")
            };
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Memory operation failed: {ex.Message}");
        }
    }

    private SiliconBeingBase? GetSiliconBeing(Guid callerId)
    {
        var manager = ServiceLocator.Instance.BeingManager;
        if (manager == null)
            return null;

        return manager.GetBeing(callerId);
    }

    private ToolResult ExecuteAdd(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("content", out object? contentObj) || string.IsNullOrWhiteSpace(contentObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'content' parameter");
        }

        string content = contentObj!.ToString()!;
        var entry = being.Memory!.Add(content);

        return ToolResult.Successful($"Memory added (ID: {entry.Id})");
    }

    private ToolResult ExecuteRecent(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        int count = 10;
        if (parameters.TryGetValue("max_results", out object? countObj) && int.TryParse(countObj?.ToString(), out int c))
        {
            count = c;
        }

        var memories = being.Memory!.GetRecent(count);

        if (memories.Count == 0)
        {
            return ToolResult.Successful("No memories yet.");
        }

        var lines = new List<string> { $"Recent {memories.Count} memories:" };
        foreach (var memory in memories)
        {
            lines.Add($"- [{memory.Timestamp:yyyy-MM-dd HH:mm}] {memory.Content.Substring(0, Math.Min(50, memory.Content.Length))}");
        }

        return ToolResult.Successful(string.Join("\n", lines));
    }

    private ToolResult ExecuteStats(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        var stats = being.Memory!.GetStatistics();

        var lines = new List<string>
        {
            "Memory Statistics:",
            $"- Total: {stats.TotalEntries}",
            $"- Oldest: {(stats.OldestEntry?.ToString() ?? "N/A")}",
            $"- Newest: {(stats.NewestEntry?.ToString() ?? "N/A")}"
        };

        return ToolResult.Successful(string.Join("\n", lines));
    }
}
