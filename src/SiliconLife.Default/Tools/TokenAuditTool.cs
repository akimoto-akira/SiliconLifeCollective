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

using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Token usage audit tool. Only available to the Silicon Curator (main administrator).
/// Provides token usage statistics and trends for AI requests.
/// Returns aggregated data similar to the Web audit dashboard.
/// </summary>
[SiliconManagerOnly]
public class TokenAuditTool : ITool
{
    public string Name => "token_audit";

    public string Description =>
        "Query AI token usage statistics and trends. Actions: " +
        "'summary' (get token usage summary with statistics), " +
        "'trend' (get token usage trend data points over time). " +
        "Supports time ranges: today (24 hours), week (7*24 hours), month (daily), year (monthly).";

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
                    ["description"] = "The audit action to perform: 'summary' for statistics, 'trend' for time series data",
                    ["enum"] = new[] { "summary", "trend" }
                },
                ["time_range"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Time range for the query: 'today' (24 data points), 'week' (168 data points), 'month' (28-31 data points), 'year' (12 data points)",
                    ["enum"] = new[] { "today", "week", "month", "year" },
                    ["default"] = "month"
                },
                ["being_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Filter by specific silicon being ID (GUID format). Omit for all beings."
                },
                ["client_type"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Filter by AI client type (e.g., 'OllamaClient'). Omit for all clients."
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj))
        {
            return ToolResult.Failed("Missing 'action' parameter");
        }

        string action = actionObj?.ToString()?.ToLowerInvariant() ?? "";

        return action switch
        {
            "summary" => ExecuteSummary(parameters),
            "trend" => ExecuteTrend(parameters),
            _ => ToolResult.Failed($"Unknown action: {action}. Supported actions: 'summary', 'trend'")
        };
    }

    private ToolResult ExecuteSummary(Dictionary<string, object> parameters)
    {
        var audit = ServiceLocator.Instance.TokenUsageAudit;
        if (audit == null)
        {
            return ToolResult.Failed("Token usage audit service is not available");
        }

        var timeRange = GetParameter(parameters, "time_range", "month");
        var clientTypeFilter = GetParameter(parameters, "client_type", "");
        var beingIdFilter = GetParameter(parameters, "being_id", "");

        var range = BuildRangeFromParams(timeRange);
        var query = new TokenUsageQuery
        {
            Range = range,
            AIClientType = string.IsNullOrEmpty(clientTypeFilter) ? null : clientTypeFilter,
            BeingId = string.IsNullOrEmpty(beingIdFilter) ? null : Guid.TryParse(beingIdFilter, out var bid) ? bid : (Guid?)null,
            GroupByAIClientType = true,
            GroupByBeingId = true
        };

        var summary = audit.QuerySummary(query);

        var result = new System.Text.StringBuilder();
        result.AppendLine("=== Token Usage Summary ===");
        result.AppendLine($"Time Range: {timeRange}");
        result.AppendLine($"Total Requests: {summary.RequestCount}");
        result.AppendLine($"Successful: {summary.SuccessCount}");
        result.AppendLine($"Failed: {summary.FailureCount}");
        result.AppendLine($"Prompt Tokens: {summary.TotalPromptTokens:N0}");
        result.AppendLine($"Completion Tokens: {summary.TotalCompletionTokens:N0}");
        result.AppendLine($"Total Tokens: {summary.TotalTokens:N0}");

        if (summary.ByAIClientType.Count > 0)
        {
            result.AppendLine("\n--- By AI Client ---");
            foreach (var kvp in summary.ByAIClientType.OrderByDescending(x => x.Value.TotalTokens))
            {
                result.AppendLine($"  {kvp.Key}: {kvp.Value.TotalTokens:N0} tokens ({kvp.Value.RequestCount} requests)");
            }
        }

        if (summary.ByBeingId.Count > 0)
        {
            result.AppendLine("\n--- By Silicon Being ---");
            var beingManager = ServiceLocator.Instance.GetService<SiliconBeingManager>();
            foreach (var kvp in summary.ByBeingId.OrderByDescending(x => x.Value.TotalTokens))
            {
                string beingName = kvp.Key.ToString();
                if (beingManager != null)
                {
                    var being = beingManager.GetBeing(kvp.Key);
                    if (being != null)
                    {
                        beingName = $"{being.Name} ({kvp.Key})";
                    }
                }
                result.AppendLine($"  {beingName}: {kvp.Value.TotalTokens:N0} tokens ({kvp.Value.RequestCount} requests)");
            }
        }

        return ToolResult.Successful(result.ToString());
    }

    private ToolResult ExecuteTrend(Dictionary<string, object> parameters)
    {
        var audit = ServiceLocator.Instance.TokenUsageAudit;
        if (audit == null)
        {
            return ToolResult.Failed("Token usage audit service is not available");
        }

        var timeRange = GetParameter(parameters, "time_range", "month");
        var clientTypeFilter = GetParameter(parameters, "client_type", "");
        var beingIdFilter = GetParameter(parameters, "being_id", "");

        var now = DateTime.UtcNow;
        IncompleteDate range;

        // For week, query the whole month and filter in memory
        if (timeRange == "week")
        {
            range = new IncompleteDate(now.Year, now.Month);
        }
        else
        {
            range = BuildRangeFromParams(timeRange);
        }

        var query = new TokenUsageQuery
        {
            Range = range,
            AIClientType = string.IsNullOrEmpty(clientTypeFilter) ? null : clientTypeFilter,
            BeingId = string.IsNullOrEmpty(beingIdFilter) ? null : Guid.TryParse(beingIdFilter, out var bid) ? bid : (Guid?)null,
            GroupByAIClientType = false,
            GroupByBeingId = false
        };

        var records = audit.QueryRecords(query);
        var dataPoints = timeRange switch
        {
            "today" => GroupByHour(records, now),
            "week" => GroupByHourForWeek(records, now),
            "month" => GroupByDay(records, now),
            "year" => GroupByMonth(records, now),
            _ => GroupByDay(records, now)
        };

        var result = new System.Text.StringBuilder();
        result.AppendLine($"=== Token Usage Trend ({timeRange}) ===");
        result.AppendLine($"Data Points: {dataPoints.Count}");
        result.AppendLine();

        // Show summary statistics
        var totalPrompt = dataPoints.Sum(p => p.PromptTokens);
        var totalCompletion = dataPoints.Sum(p => p.CompletionTokens);
        var totalTokens = dataPoints.Sum(p => p.TotalTokens);
        result.AppendLine($"Period Total:");
        result.AppendLine($"  Prompt Tokens: {totalPrompt:N0}");
        result.AppendLine($"  Completion Tokens: {totalCompletion:N0}");
        result.AppendLine($"  Total Tokens: {totalTokens:N0}");
        result.AppendLine();

        // Show data points (limit display for large datasets)
        result.AppendLine("Data Points:");
        int displayLimit = timeRange switch
        {
            "today" => 24,
            "week" => 168,
            "month" => dataPoints.Count,
            "year" => 12,
            _ => dataPoints.Count
        };

        foreach (var point in dataPoints.Take(displayLimit))
        {
            if (point.TotalTokens > 0)
            {
                result.AppendLine($"  {point.Date}: Prompt={point.PromptTokens:N0}, Completion={point.CompletionTokens:N0}, Total={point.TotalTokens:N0}");
            }
        }

        var hiddenCount = dataPoints.Count - displayLimit;
        if (hiddenCount > 0)
        {
            result.AppendLine($"  ... and {hiddenCount} more data points with 0 tokens");
        }

        return ToolResult.Successful(result.ToString());
    }

    private List<DataPoint> GroupByHour(List<TokenUsageRecord> records, DateTime now)
    {
        var today = now.Date;
        var dataPoints = new List<DataPoint>();

        for (int hour = 0; hour < 24; hour++)
        {
            var hourStart = today.AddHours(hour);
            var hourEnd = hourStart.AddHours(1);

            var hourRecords = records.Where(r => r.Timestamp >= hourStart && r.Timestamp < hourEnd).ToList();

            dataPoints.Add(new DataPoint
            {
                Date = hourStart.ToString("yyyy-MM-dd HH:00"),
                PromptTokens = hourRecords.Sum(r => r.PromptTokens),
                CompletionTokens = hourRecords.Sum(r => r.CompletionTokens),
                TotalTokens = hourRecords.Sum(r => r.TotalTokens)
            });
        }

        return dataPoints;
    }

    private List<DataPoint> GroupByHourForWeek(List<TokenUsageRecord> records, DateTime now)
    {
        var weekStart = now.Date.AddDays(-(int)now.DayOfWeek);
        var weekEnd = weekStart.AddDays(7);

        var dataPoints = new List<DataPoint>();
        for (var day = 0; day < 7; day++)
        {
            var currentDate = weekStart.AddDays(day);
            for (int hour = 0; hour < 24; hour++)
            {
                var hourStart = currentDate.AddHours(hour);
                var hourRecords = records.Where(r => r.Timestamp >= hourStart && r.Timestamp < hourStart.AddHours(1)).ToList();

                dataPoints.Add(new DataPoint
                {
                    Date = hourStart.ToString("yyyy-MM-dd HH:00"),
                    PromptTokens = hourRecords.Sum(r => r.PromptTokens),
                    CompletionTokens = hourRecords.Sum(r => r.CompletionTokens),
                    TotalTokens = hourRecords.Sum(r => r.TotalTokens)
                });
            }
        }

        return dataPoints;
    }

    private List<DataPoint> GroupByDay(List<TokenUsageRecord> records, DateTime now)
    {
        var range = BuildRangeFromParams(GetParameter(new Dictionary<string, object>(), "time_range", "month"));
        var (startDate, endDate) = range.GetRange();
        endDate = endDate.Date;

        var dateDict = new Dictionary<string, (long PromptTokens, long CompletionTokens, long TotalTokens)>();
        for (var date = startDate.Date; date <= endDate; date = date.AddDays(1))
        {
            var dateKey = date.ToString("yyyy-MM-dd");
            dateDict[dateKey] = (0, 0, 0);
        }

        foreach (var record in records)
        {
            var dateKey = $"{record.Timestamp.Year}-{record.Timestamp.Month:D2}-{record.Timestamp.Day:D2}";
            if (dateDict.ContainsKey(dateKey))
            {
                var existing = dateDict[dateKey];
                dateDict[dateKey] = (
                    existing.PromptTokens + record.PromptTokens,
                    existing.CompletionTokens + record.CompletionTokens,
                    existing.TotalTokens + record.TotalTokens
                );
            }
        }

        return dateDict.Select(kv => new DataPoint
        {
            Date = kv.Key,
            PromptTokens = kv.Value.PromptTokens,
            CompletionTokens = kv.Value.CompletionTokens,
            TotalTokens = kv.Value.TotalTokens
        }).OrderBy(x => x.Date).ToList();
    }

    private List<DataPoint> GroupByMonth(List<TokenUsageRecord> records, DateTime now)
    {
        var monthDict = new Dictionary<string, (long PromptTokens, long CompletionTokens, long TotalTokens)>();
        for (int month = 1; month <= 12; month++)
        {
            var monthKey = $"{now.Year}-{month:00}";
            monthDict[monthKey] = (0, 0, 0);
        }

        foreach (var record in records)
        {
            var monthKey = $"{record.Timestamp.Year}-{record.Timestamp.Month:D2}";
            if (monthDict.ContainsKey(monthKey))
            {
                var existing = monthDict[monthKey];
                monthDict[monthKey] = (
                    existing.PromptTokens + record.PromptTokens,
                    existing.CompletionTokens + record.CompletionTokens,
                    existing.TotalTokens + record.TotalTokens
                );
            }
        }

        return monthDict.Select(kv => new DataPoint
        {
            Date = kv.Key,
            PromptTokens = kv.Value.PromptTokens,
            CompletionTokens = kv.Value.CompletionTokens,
            TotalTokens = kv.Value.TotalTokens
        }).OrderBy(x => x.Date).ToList();
    }

    private IncompleteDate BuildRangeFromParams(string timeRange)
    {
        var now = DateTime.UtcNow;

        return timeRange switch
        {
            "today" => new IncompleteDate(now.Year, now.Month, now.Day),
            "week" => new IncompleteDate(now.Year, now.Month, now.AddDays(-(int)now.DayOfWeek).Day),
            "month" => new IncompleteDate(now.Year, now.Month),
            "year" => new IncompleteDate(now.Year),
            _ => new IncompleteDate(now.Year, now.Month)
        };
    }

    private static string GetParameter(Dictionary<string, object> parameters, string key, string defaultValue)
    {
        if (parameters.TryGetValue(key, out object? value))
        {
            return value?.ToString() ?? defaultValue;
        }
        return defaultValue;
    }

    private class DataPoint
    {
        public string Date { get; init; } = "";
        public long PromptTokens { get; init; }
        public long CompletionTokens { get; init; }
        public long TotalTokens { get; init; }
    }
}
