// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Text;
using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Log query tool for silicon beings to access their historical operation records and system logs.
/// Enables self-auditing and reviewing of past activities.
/// </summary>
public class LogTool : ITool
{
    public string Name => "log";

    public string Description =>
        "Query historical operation records and system logs. Actions: query_operations (query operation logs), " +
        "query_tool_calls (query tool call history), query_conversations (query conversation history), " +
        "export (export logs in JSON or text format)";

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
                    ["description"] = "Action to perform: query_operations, query_tool_calls, query_conversations, export",
                    ["enum"] = new[] { "query_operations", "query_tool_calls", "query_conversations", "export" }
                },
                ["since"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Start time in ISO 8601 format (for query_operations)"
                },
                ["until"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "End time in ISO 8601 format (for query_operations)"
                },
                ["action_filter"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Filter by specific action type like 'tool_call', 'ai_request' (for query_operations)"
                },
                ["max_count"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum number of records to return, default 50"
                },
                ["tool_name"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Filter by specific tool name (for query_tool_calls)"
                },
                ["success_only"] = new Dictionary<string, object>
                {
                    ["type"] = "boolean",
                    ["description"] = "Only return successful/failed calls (for query_tool_calls)"
                },
                ["session_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Specific session ID (for query_conversations)"
                },
                ["keyword"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Search keyword (for query_conversations)"
                },
                ["format"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Export format: 'json' or 'text' (for export action)",
                    ["enum"] = new[] { "json", "text" }
                },
                ["time_range"] = new Dictionary<string, object>
                {
                    ["type"] = "object",
                    ["description"] = "Time range object with 'since' and 'until' properties (for export action)"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj) || string.IsNullOrWhiteSpace(actionObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'action' parameter");
        }

        string action = actionObj.ToString()!.ToLowerInvariant();

        return action switch
        {
            "query_operations" => ExecuteQueryOperations(callerId, parameters),
            "query_tool_calls" => ExecuteQueryToolCalls(callerId, parameters),
            "query_conversations" => ExecuteQueryConversations(callerId, parameters),
            "export" => ExecuteExport(callerId, parameters),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    private ToolResult ExecuteQueryOperations(Guid callerId, Dictionary<string, object> parameters)
    {
        int maxCount = 50;
        if (parameters.TryGetValue("max_count", out object? maxCountObj) && maxCountObj != null)
        {
            if (int.TryParse(maxCountObj.ToString(), out int parsedMaxCount))
            {
                maxCount = parsedMaxCount;
            }
        }

        string? since = parameters.TryGetValue("since", out object? sinceObj) && sinceObj != null
            ? sinceObj.ToString()
            : null;
        string? until = parameters.TryGetValue("until", out object? untilObj) && untilObj != null
            ? untilObj.ToString()
            : null;
        string? actionFilter = parameters.TryGetValue("action_filter", out object? actionFilterObj) && actionFilterObj != null
            ? actionFilterObj.ToString()
            : null;

        // Get the being to access its logs
        var being = ServiceLocator.Instance.BeingManager?.GetBeing(callerId);
        if (being == null)
        {
            return ToolResult.Failed("Could not find the calling being");
        }

        // For now, return placeholder data since we don't have direct access to operation logs
        // In a real implementation, this would query the actual operation log storage
        var result = new StringBuilder($"Operation logs for being '{callerId}' (limited to {maxCount} records):\n");

        if (!string.IsNullOrEmpty(since))
        {
            result.AppendLine($"  Since: {since}");
        }
        if (!string.IsNullOrEmpty(until))
        {
            result.AppendLine($"  Until: {until}");
        }
        if (!string.IsNullOrEmpty(actionFilter))
        {
            result.AppendLine($"  Action filter: {actionFilter}");
        }

        result.AppendLine("\nNote: Operation log query is not fully implemented yet.");
        result.AppendLine("This is a placeholder response showing the query parameters.");

        return ToolResult.Successful(result.ToString().TrimEnd());
    }

    private ToolResult ExecuteQueryToolCalls(Guid callerId, Dictionary<string, object> parameters)
    {
        int maxCount = 50;
        if (parameters.TryGetValue("max_count", out object? maxCountObj) && maxCountObj != null)
        {
            if (int.TryParse(maxCountObj.ToString(), out int parsedMaxCount))
            {
                maxCount = parsedMaxCount;
            }
        }

        string? toolName = parameters.TryGetValue("tool_name", out object? toolNameObj) && toolNameObj != null
            ? toolNameObj.ToString()
            : null;
        bool? successOnly = parameters.TryGetValue("success_only", out object? successOnlyObj) && successOnlyObj != null
            && bool.TryParse(successOnlyObj.ToString(), out bool parsedSuccessOnly)
            ? parsedSuccessOnly
            : (bool?)null;

        // Get the being to access its logs
        var being = ServiceLocator.Instance.BeingManager?.GetBeing(callerId);
        if (being == null)
        {
            return ToolResult.Failed("Could not find the calling being");
        }

        // For now, return placeholder data
        var result = new StringBuilder($"Tool call history for being '{callerId}' (limited to {maxCount} records):\n");

        if (!string.IsNullOrEmpty(toolName))
        {
            result.AppendLine($"  Tool filter: {toolName}");
        }
        if (successOnly.HasValue)
        {
            result.AppendLine($"  Success only: {successOnly.Value}");
        }

        result.AppendLine("\nNote: Tool call history query is not fully implemented yet.");
        result.AppendLine("This is a placeholder response showing the query parameters.");

        return ToolResult.Successful(result.ToString().TrimEnd());
    }

    private ToolResult ExecuteQueryConversations(Guid callerId, Dictionary<string, object> parameters)
    {
        int maxCount = 50;
        if (parameters.TryGetValue("max_count", out object? maxCountObj) && maxCountObj != null)
        {
            if (int.TryParse(maxCountObj.ToString(), out int parsedMaxCount))
            {
                maxCount = parsedMaxCount;
            }
        }

        string? sessionId = parameters.TryGetValue("session_id", out object? sessionIdObj) && sessionIdObj != null
            ? sessionIdObj.ToString()
            : null;
        string? keyword = parameters.TryGetValue("keyword", out object? keywordObj) && keywordObj != null
            ? keywordObj.ToString()
            : null;

        // Get the being to access its logs
        var being = ServiceLocator.Instance.BeingManager?.GetBeing(callerId);
        if (being == null)
        {
            return ToolResult.Failed("Could not find the calling being");
        }

        // For now, return placeholder data
        var result = new StringBuilder($"Conversation history for being '{callerId}' (limited to {maxCount} records):\n");

        if (!string.IsNullOrEmpty(sessionId))
        {
            result.AppendLine($"  Session ID: {sessionId}");
        }
        if (!string.IsNullOrEmpty(keyword))
        {
            result.AppendLine($"  Keyword: {keyword}");
        }

        result.AppendLine("\nNote: Conversation history query is not fully implemented yet.");
        result.AppendLine("This is a placeholder response showing the query parameters.");

        return ToolResult.Successful(result.ToString().TrimEnd());
    }

    private ToolResult ExecuteExport(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("format", out object? formatObj) || string.IsNullOrWhiteSpace(formatObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'format' parameter for export action");
        }

        string format = formatObj.ToString()!.ToLowerInvariant();
        if (format != "json" && format != "text")
        {
            return ToolResult.Failed("Format must be 'json' or 'text'");
        }

        // Get the being to access its logs
        var being = ServiceLocator.Instance.BeingManager?.GetBeing(callerId);
        if (being == null)
        {
            return ToolResult.Failed("Could not find the calling being");
        }

        // Get time range if provided
        string timeRangeInfo = "";
        if (parameters.TryGetValue("time_range", out object? timeRangeObj) && timeRangeObj != null)
        {
            timeRangeInfo = $" with time range: {timeRangeObj}";
        }

        // For now, return placeholder data
        return ToolResult.Successful($"Log export requested in '{format}' format{timeRangeInfo}.\n" +
                                   "Note: Log export functionality is not fully implemented yet.\n" +
                                   "This is a placeholder response.");
    }
}
