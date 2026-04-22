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
using System.Text.Json;
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
        "export (export logs in JSON or text format), get_system_info (get basic system information)";

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
                    ["enum"] = new[] { "query_operations", "query_tool_calls", "query_conversations", "export", "get_system_info" }
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
                ["other_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The other participant's GUID (user ID or another being ID)"
                },
                ["my_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Optional: your own being GUID (for querying conversations on behalf of another being)"
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
                },
                ["include_details"] = new Dictionary<string, object>
                {
                    ["type"] = "boolean",
                    ["description"] = "Include detailed information like all being IDs (for get_system_info action)"
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
            "get_system_info" => ExecuteGetSystemInfo(callerId, parameters),
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

        // Query audit logs from AuditLogger
        var auditLogger = ServiceLocator.Instance.AuditLogger;
        if (auditLogger == null)
        {
            return ToolResult.Failed("Audit logger is not available");
        }

        // Parse time range
        DateTime? startTime = null;
        DateTime? endTime = null;
        
        if (!string.IsNullOrEmpty(since) && DateTime.TryParse(since, out var start))
        {
            startTime = DateTime.SpecifyKind(start, DateTimeKind.Utc);
        }
        
        if (!string.IsNullOrEmpty(until) && DateTime.TryParse(until, out var end))
        {
            endTime = DateTime.SpecifyKind(end, DateTimeKind.Utc);
        }

        // Query all audit entries (ITimeStorage doesn't support DateTime range, so we query all and filter)
        List<AuditEntry> allEntries = auditLogger.Query(null);
        
        // Filter by time range and caller ID
        var filteredEntries = allEntries
            .Where(e => e.CallerId == callerId)
            .Where(e =>
            {
                var tsUtc = DateTime.SpecifyKind(e.Timestamp, DateTimeKind.Utc);
                if (startTime.HasValue && tsUtc < startTime.Value) return false;
                if (endTime.HasValue && tsUtc > endTime.Value) return false;
                return true;
            })
            .Where(e => string.IsNullOrEmpty(actionFilter) || 
                       e.PermissionType.ToString().Contains(actionFilter, StringComparison.OrdinalIgnoreCase) ||
                       e.Resource.Contains(actionFilter, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(e => e.Timestamp)
            .Take(maxCount)
            .ToList();

        if (filteredEntries.Count == 0)
        {
            return ToolResult.Successful($"No operation logs found for being '{callerId}' in the specified time range.");
        }

        var result = new StringBuilder($"Operation logs for being '{callerId}' ({filteredEntries.Count} records):\n\n");
        
        foreach (var entry in filteredEntries)
        {
            string resultStr = entry.Result == PermissionResult.Allowed ? "ALLOWED" : 
                             entry.Result == PermissionResult.Denied ? "DENIED" : "ASK_USER";
            
            result.AppendLine($"[{entry.Timestamp:yyyy-MM-dd HH:mm:ss}] {resultStr}");
            result.AppendLine($"  Type: {entry.PermissionType}");
            result.AppendLine($"  Resource: {entry.Resource}");
            result.AppendLine($"  Reason: {entry.Reason}");
            result.AppendLine();
        }

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

        // Query logs from LogManager
        var logs = LogManager.Instance.ReadLogs(
            beingId: callerId,
            systemOnly: false,
            maxCount: maxCount * 2); // Get more to allow filtering

        // Filter for tool-related logs
        var toolCallLogs = logs
            .Where(e => e.Message.Contains("Tool", StringComparison.OrdinalIgnoreCase) || 
                       e.Message.Contains("tool", StringComparison.OrdinalIgnoreCase))
            .Where(e => string.IsNullOrEmpty(toolName) || 
                       e.Message.Contains(toolName, StringComparison.OrdinalIgnoreCase))
            .Where(e => !successOnly.HasValue || e.Level == LogLevel.Error == !successOnly.Value)
            .OrderByDescending(e => e.Timestamp)
            .Take(maxCount)
            .ToList();

        if (toolCallLogs.Count == 0)
        {
            return ToolResult.Successful($"No tool call history found for being '{callerId}'.");
        }

        var result = new StringBuilder($"Tool call history for being '{callerId}' ({toolCallLogs.Count} records):\n\n");
        
        foreach (var log in toolCallLogs)
        {
            result.AppendLine($"[{log.Timestamp:yyyy-MM-dd HH:mm:ss}] {log.Level.ToString().ToUpper()}");
            result.AppendLine($"  {log.Message}");
            if (!string.IsNullOrEmpty(log.Category))
            {
                result.AppendLine($"  Category: {log.Category}");
            }
            result.AppendLine();
        }

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

        string? otherIdStr = parameters.TryGetValue("other_id", out object? otherIdObj) && otherIdObj != null
            ? otherIdObj.ToString()
            : null;
        string? myIdStr = parameters.TryGetValue("my_id", out object? myIdObj) && myIdObj != null
            ? myIdObj.ToString()
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

        var chatSystem = ServiceLocator.Instance.ChatSystem;
        if (chatSystem == null)
        {
            return ToolResult.Failed("Chat system is not available");
        }

        // Determine the two participants
        Guid participant1;
        Guid participant2;

        if (!string.IsNullOrEmpty(myIdStr) && Guid.TryParse(myIdStr, out var myId))
        {
            // Intrusive query: being is querying on behalf of another being
            participant1 = myId;
            
            if (!string.IsNullOrEmpty(otherIdStr) && Guid.TryParse(otherIdStr, out var otherId))
            {
                participant2 = otherId;
            }
            else
            {
                // If other_id not provided, query with user
                var config = Config.Instance?.Data;
                if (config == null)
                {
                    return ToolResult.Failed("Configuration is not available");
                }
                participant2 = config.UserGuid;
            }
        }
        else
        {
            // Normal query: being queries its own conversation
            participant1 = callerId;
            
            if (!string.IsNullOrEmpty(otherIdStr) && Guid.TryParse(otherIdStr, out var otherId))
            {
                participant2 = otherId;
            }
            else
            {
                // If other_id not provided, query with user
                var config = Config.Instance?.Data;
                if (config == null)
                {
                    return ToolResult.Failed("Configuration is not available");
                }
                participant2 = config.UserGuid;
            }
        }

        // Use ChatSystem's GetMessages to get conversation between two participants
        List<ChatMessage> messages = chatSystem.GetMessages(participant1, participant2, maxCount);

        // Filter by keyword if provided
        if (!string.IsNullOrEmpty(keyword))
        {
            messages = messages
                .Where(m => m.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                           (m.Thinking?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false))
                .ToList();
        }

        if (messages.Count == 0)
        {
            string participant1Name = participant1 == callerId ? "Self" : participant1.ToString("N");
            string participant2Name = participant2 == Config.Instance?.Data?.UserGuid ? "User" : participant2.ToString("N");
            return ToolResult.Successful($"No conversation history found between {participant1Name} and {participant2Name}.");
        }

        var result = new StringBuilder($"Conversation history ({messages.Count} records):\n");
        result.AppendLine($"  Between: {participant1:N} and {participant2:N}\n");
        
        foreach (var msg in messages)
        {
            string senderName = msg.SenderId == participant1 ? "Participant1" : 
                               msg.SenderId == Config.Instance?.Data?.UserGuid ? "User" : 
                               msg.SenderId.ToString("N").Substring(0, 8);
            result.AppendLine($"[{msg.Timestamp:yyyy-MM-dd HH:mm:ss}] {senderName}:");
            
            if (!string.IsNullOrEmpty(msg.Content))
            {
                result.AppendLine($"  {msg.Content}");
            }
            
            if (!string.IsNullOrEmpty(msg.Thinking))
            {
                result.AppendLine($"  [Thinking: {msg.Thinking}]");
            }
            
            if (msg.Role == MessageRole.Tool)
            {
                result.AppendLine($"  [Tool Result]");
            }
            
            result.AppendLine();
        }

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
        DateTime? startTime = null;
        DateTime? endTime = null;
        
        if (parameters.TryGetValue("time_range", out object? timeRangeObj) && timeRangeObj != null)
        {
            // Parse time range object (expected: {"since": "...", "until": "..."})
            try
            {
                var timeRangeJson = timeRangeObj.ToString();
                if (!string.IsNullOrEmpty(timeRangeJson))
                {
                    using var doc = JsonDocument.Parse(timeRangeJson);
                    var root = doc.RootElement;
                    
                    if (root.TryGetProperty("since", out var sinceProp))
                    {
                        var sinceStr = sinceProp.GetString();
                        if (!string.IsNullOrEmpty(sinceStr) && DateTime.TryParse(sinceStr, out var since))
                        {
                            startTime = DateTime.SpecifyKind(since, DateTimeKind.Utc);
                        }
                    }
                    
                    if (root.TryGetProperty("until", out var untilProp))
                    {
                        var untilStr = untilProp.GetString();
                        if (!string.IsNullOrEmpty(untilStr) && DateTime.TryParse(untilStr, out var until))
                        {
                            endTime = DateTime.SpecifyKind(until, DateTimeKind.Utc);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ToolResult.Failed($"Invalid time_range format: {ex.Message}");
            }
        }

        // Collect all logs
        var exportData = new
        {
            BeingId = callerId,
            ExportTime = DateTime.UtcNow,
            TimeRange = new
            {
                Since = startTime?.ToString("o"),
                Until = endTime?.ToString("o")
            },
            OperationLogs = ExportOperationLogs(callerId, startTime, endTime),
            ToolCallLogs = ExportToolCallLogs(callerId, startTime, endTime),
            ConversationLogs = ExportConversationLogs(callerId, startTime, endTime)
        };

        if (format == "json")
        {
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            string json = JsonSerializer.Serialize(exportData, jsonOptions);
            return ToolResult.Successful(json);
        }
        else
        {
            // Text format
            var result = new StringBuilder();
            result.AppendLine($"=== Log Export for Being {callerId} ===");
            result.AppendLine($"Export Time: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");
            
            if (startTime.HasValue)
            {
                result.AppendLine($"Time Range: {startTime.Value:yyyy-MM-dd HH:mm:ss} to {(endTime.HasValue ? endTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "now")}");
            }
            
            result.AppendLine();
            result.AppendLine("--- Operation Logs ---");
            result.AppendLine(string.Join("\n", ExportOperationLogs(callerId, startTime, endTime)));
            result.AppendLine();
            result.AppendLine("--- Tool Call Logs ---");
            result.AppendLine(string.Join("\n", ExportToolCallLogs(callerId, startTime, endTime)));
            result.AppendLine();
            result.AppendLine("--- Conversation Logs ---");
            result.AppendLine(string.Join("\n", ExportConversationLogs(callerId, startTime, endTime)));
            
            return ToolResult.Successful(result.ToString());
        }
    }

    private ToolResult ExecuteGetSystemInfo(Guid callerId, Dictionary<string, object> parameters)
    {
        // Get the being to verify it exists
        var being = ServiceLocator.Instance.BeingManager?.GetBeing(callerId);
        if (being == null)
        {
            return ToolResult.Failed("Could not find the calling being");
        }

        var config = Config.Instance?.Data;
        if (config == null)
        {
            return ToolResult.Failed("Configuration is not available");
        }

        bool includeDetails = parameters.TryGetValue("include_details", out object? detailsObj) && 
                             detailsObj != null && 
                             bool.TryParse(detailsObj.ToString(), out bool parsedDetails) && 
                             parsedDetails;

        var sb = new StringBuilder();
        sb.AppendLine("=== System Information ===");
        sb.AppendLine();
        sb.AppendLine("[System IDs]");
        sb.AppendLine($"  User GUID: {config.UserGuid}");
        sb.AppendLine($"  Broadcast Channel GUID: {config.BroadcastChannelGuid}");
        sb.AppendLine($"  Curator GUID: {config.CuratorGuid}");
        sb.AppendLine($"  Current Being GUID: {callerId}");
        sb.AppendLine($"  Current Being Name: {being.Name}");
        sb.AppendLine($"  Is Curator: {being.IsCurator}");
        
        sb.AppendLine();
        sb.AppendLine("[System Settings]");
        sb.AppendLine($"  Language: {config.Language}");
        sb.AppendLine($"  User Nickname: {config.UserNickname}");
        sb.AppendLine($"  AI Client Type: {config.AIClientType}");
        sb.AppendLine($"  Minimum Log Level: {config.MinimumLogLevel}");
        sb.AppendLine($"  Data Directory: {config.DataDirectory.FullName}");
        
        if (includeDetails)
        {
            sb.AppendLine();
            sb.AppendLine("[All Beings]");
            var beingManager = ServiceLocator.Instance.BeingManager;
            if (beingManager != null)
            {
                var allBeings = beingManager.GetAllBeings();
                foreach (var b in allBeings)
                {
                    string curatorMark = b.IsCurator ? " [CURATOR]" : "";
                    sb.AppendLine($"  {b.Id} - {b.Name}{curatorMark}");
                }
            }
            else
            {
                sb.AppendLine("  Being manager not available");
            }
        }

        return ToolResult.Successful(sb.ToString().TrimEnd());
    }

    private List<string> ExportOperationLogs(Guid callerId, DateTime? startTime, DateTime? endTime)
    {
        var auditLogger = ServiceLocator.Instance.AuditLogger;
        if (auditLogger == null) return new List<string> { "Audit logger not available" };

        var allEntries = auditLogger.Query(null);
        var filtered = allEntries
            .Where(e => e.CallerId == callerId)
            .Where(e =>
            {
                var tsUtc = DateTime.SpecifyKind(e.Timestamp, DateTimeKind.Utc);
                if (startTime.HasValue && tsUtc < startTime.Value) return false;
                if (endTime.HasValue && tsUtc > endTime.Value) return false;
                return true;
            })
            .OrderByDescending(e => e.Timestamp)
            .Take(100)
            .ToList();

        return filtered.Select(e => 
            $"[{e.Timestamp:yyyy-MM-dd HH:mm:ss}] {e.Result} | {e.PermissionType} | {e.Resource} | {e.Reason}"
        ).ToList();
    }

    private List<string> ExportToolCallLogs(Guid callerId, DateTime? startTime, DateTime? endTime)
    {
        var logs = LogManager.Instance.ReadLogs(
            beingId: callerId,
            systemOnly: false,
            maxCount: 200);

        var filtered = logs
            .Where(e => e.Message.Contains("Tool", StringComparison.OrdinalIgnoreCase))
            .Where(e =>
            {
                var tsUtc = DateTime.SpecifyKind(e.Timestamp, DateTimeKind.Utc);
                if (startTime.HasValue && tsUtc < startTime.Value) return false;
                if (endTime.HasValue && tsUtc > endTime.Value) return false;
                return true;
            })
            .OrderByDescending(e => e.Timestamp)
            .Take(100)
            .ToList();

        return filtered.Select(e => 
            $"[{e.Timestamp:yyyy-MM-dd HH:mm:ss}] {e.Level} | {e.Message}"
        ).ToList();
    }

    private List<string> ExportConversationLogs(Guid callerId, DateTime? startTime, DateTime? endTime)
    {
        var chatSystem = ServiceLocator.Instance.ChatSystem;
        if (chatSystem == null) return new List<string> { "Chat system not available" };

        var messages = new List<ChatMessage>();
        var beingManager = ServiceLocator.Instance.BeingManager;
        
        if (beingManager != null)
        {
            var allBeings = beingManager.GetAllBeings();
            foreach (var b in allBeings)
            {
                if (b.Id == callerId) continue;
                
                try
                {
                    var sessionMessages = chatSystem.GetMessages(b.Id, callerId, 100);
                    messages.AddRange(sessionMessages);
                }
                catch
                {
                    // Skip sessions that don't exist
                }
            }
        }

        var filtered = messages
            .Where(m =>
            {
                var tsUtc = DateTime.SpecifyKind(m.Timestamp, DateTimeKind.Utc);
                if (startTime.HasValue && tsUtc < startTime.Value) return false;
                if (endTime.HasValue && tsUtc > endTime.Value) return false;
                return true;
            })
            .OrderByDescending(m => m.Timestamp)
            .Take(100)
            .ToList();

        return filtered.Select(m => 
            $"[{m.Timestamp:yyyy-MM-dd HH:mm:ss}] {m.SenderId:N8} | {m.Content.Substring(0, Math.Min(100, m.Content.Length))}"
        ).ToList();
    }
}
