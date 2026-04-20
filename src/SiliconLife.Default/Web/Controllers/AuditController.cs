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
using SiliconLife.Default.Web.Models;

namespace SiliconLife.Default.Web;

[WebCode]
public class AuditController : Controller
{
    private readonly SkinManager _skinManager;
    private readonly ITokenUsageAudit? _audit;
    private readonly SiliconBeingManager? _beingManager;

    public AuditController()
    {
        var locator = ServiceLocator.Instance;
        _skinManager = locator.GetService<SkinManager>()!;
        _audit = locator.TokenUsageAudit;
        _beingManager = locator.BeingManager;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/audit";

        if (path == "/audit" || path == "/audit/index")
            Index();
        else if (path == "/api/audit/summary")
            GetSummary();
        else if (path == "/api/audit/trend")
            GetTrend();
        else if (path == "/api/audit/export")
            Export();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.AuditView();
        var vm = new Models.AuditViewModel { Skin = skin, ActiveMenu = "audit" };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetSummary()
    {
        if (_audit == null)
        {
            RenderJson(new
            {
                requestCount = 0,
                totalPromptTokens = 0L,
                totalCompletionTokens = 0L,
                totalTokens = 0L,
                byClient = new List<AuditSummaryItem>(),
                byBeing = new List<AuditSummaryItem>()
            });
            return;
        }

        var clientTypeFilter = GetQueryValue("clientType", "");
        var beingIdFilter = GetQueryValue("beingId", "");
        var timeRange = GetQueryValue("timeRange", "month");
        var now = DateTime.UtcNow;

        // For week, need special range handling
        IncompleteDate range;
        if (timeRange == "week")
        {
            // Query entire month's data, then filter by frontend
            range = new IncompleteDate(now.Year, now.Month);
        }
        else
        {
            range = BuildRangeFromParams();
        }

        var query = new TokenUsageQuery
        {
            Range = range,
            AIClientType = string.IsNullOrEmpty(clientTypeFilter) ? null : clientTypeFilter,
            BeingId = string.IsNullOrEmpty(beingIdFilter) ? null : Guid.TryParse(beingIdFilter, out var bid) ? bid : (Guid?)null,
            GroupByAIClientType = true,
            GroupByBeingId = true
        };

        TokenUsageSummary summary = _audit.QuerySummary(query);

        // For week, need to filter and only count this week's data
        if (timeRange == "week")
        {
            var weekStart = now.Date.AddDays(-(int)now.DayOfWeek);
            var weekEnd = weekStart.AddDays(7);
            
            // Re-query records and filter
            var recordsQuery = new TokenUsageQuery
            {
                Range = range,
                AIClientType = string.IsNullOrEmpty(clientTypeFilter) ? null : clientTypeFilter,
                BeingId = string.IsNullOrEmpty(beingIdFilter) ? null : Guid.TryParse(beingIdFilter, out var beingId) ? beingId : (Guid?)null,
                GroupByAIClientType = false,
                GroupByBeingId = false
            };
            
            var records = _audit.QueryRecords(recordsQuery);
            var filteredRecords = records.Where(r => r.Timestamp >= weekStart && r.Timestamp < weekEnd).ToList();
            
            summary = new TokenUsageSummary
            {
                RequestCount = filteredRecords.Count,
                SuccessCount = filteredRecords.Count(r => r.Success),
                FailureCount = filteredRecords.Count(r => !r.Success),
                TotalPromptTokens = filteredRecords.Sum(r => r.PromptTokens),
                TotalCompletionTokens = filteredRecords.Sum(r => r.CompletionTokens),
                TotalTokens = filteredRecords.Sum(r => r.TotalTokens),
                ByAIClientType = filteredRecords.GroupBy(r => r.AIClientType ?? "Unknown")
                    .ToDictionary(g => g.Key, g => new TokenUsageSummary
                    {
                        RequestCount = g.Count(),
                        TotalPromptTokens = g.Sum(r => r.PromptTokens),
                        TotalCompletionTokens = g.Sum(r => r.CompletionTokens),
                        TotalTokens = g.Sum(r => r.TotalTokens)
                    }),
                ByBeingId = filteredRecords.GroupBy(r => r.BeingId)
                    .ToDictionary(g => g.Key, g => new TokenUsageSummary
                    {
                        RequestCount = g.Count(),
                        TotalPromptTokens = g.Sum(r => r.PromptTokens),
                        TotalCompletionTokens = g.Sum(r => r.CompletionTokens),
                        TotalTokens = g.Sum(r => r.TotalTokens)
                    })
            };
        }

        var byClient = summary.ByAIClientType.Select(kvp => new AuditSummaryItem
        {
            Key = kvp.Key,
            Name = kvp.Key,
            RequestCount = kvp.Value.RequestCount,
            TotalPromptTokens = kvp.Value.TotalPromptTokens,
            TotalCompletionTokens = kvp.Value.TotalCompletionTokens,
            TotalTokens = kvp.Value.TotalTokens
        }).ToList();

        Dictionary<string, string> beingNameMap = _beingManager != null
            ? summary.ByBeingId.Keys
                .Select(id => new { Guid = id, Being = _beingManager.GetBeing(id) })
                .ToDictionary(x => x.Guid.ToString(), x => x.Being?.Name ?? x.Guid.ToString())
            : summary.ByBeingId.Keys.ToDictionary(id => id.ToString(), id => id.ToString());

        var byBeing = summary.ByBeingId.Select(kvp => new AuditSummaryItem
        {
            Key = kvp.Key.ToString(),
            Name = beingNameMap.TryGetValue(kvp.Key.ToString(), out string? name) ? name : kvp.Key.ToString(),
            RequestCount = kvp.Value.RequestCount,
            TotalPromptTokens = kvp.Value.TotalPromptTokens,
            TotalCompletionTokens = kvp.Value.TotalCompletionTokens,
            TotalTokens = kvp.Value.TotalTokens
        }).ToList();

        RenderJson(new
        {
            requestCount = summary.RequestCount,
            totalPromptTokens = summary.TotalPromptTokens,
            totalCompletionTokens = summary.TotalCompletionTokens,
            totalTokens = summary.TotalTokens,
            byClient,
            byBeing,
            beings = beingNameMap
        });
    }

    private void GetTrend()
    {
        if (_audit == null)
        {
            RenderJson(new { points = new List<object>(), beings = new Dictionary<string, string>() });
            return;
        }

        var clientTypeFilter = GetQueryValue("clientType", "");
        var beingIdFilter = GetQueryValue("beingId", "");
        var timeRange = GetQueryValue("timeRange", "month");
        var now = DateTime.UtcNow;

        // For week, query the whole month and filter in GroupBy methods
        IncompleteDate range;
        if (timeRange == "week")
        {
            range = new IncompleteDate(now.Year, now.Month);
        }
        else
        {
            range = BuildRangeFromParams();
        }

        var query = new TokenUsageQuery
        {
            Range = range,
            AIClientType = string.IsNullOrEmpty(clientTypeFilter) ? null : clientTypeFilter,
            BeingId = string.IsNullOrEmpty(beingIdFilter) ? null : Guid.TryParse(beingIdFilter, out var bid) ? bid : (Guid?)null,
            GroupByAIClientType = false,
            GroupByBeingId = false
        };

        List<TokenUsageRecord> records = _audit.QueryRecords(query);

        var grouped = timeRange switch
        {
            "today" => GroupByHour(records, now),
            "week" => GroupByHourForWeek(records, now),
            "month" => GroupByDay(records, now),
            "year" => GroupByMonth(records, now),
            _ => GroupByDay(records, now)
        };

        Dictionary<string, string> beings = BuildBeingNameMap(records);

        RenderJson(new { points = grouped, beings });
    }

    private List<object> GroupByHourForWeek(List<TokenUsageRecord> records, DateTime now)
    {
        // Calculate week start date (Sunday)
        var weekStart = now.Date.AddDays(-(int)now.DayOfWeek);
        var weekEnd = weekStart.AddDays(7); // Exclude day 8

        // Create dictionary for 7 days × 24 hours
        var hourDict = new Dictionary<string, (long promptTokens, long completionTokens, long totalTokens)>();
        for (var day = 0; day < 7; day++)
        {
            var currentDate = weekStart.AddDays(day);
            for (int hour = 0; hour < 24; hour++)
            {
                var hourStart = currentDate.AddHours(hour);
                var hourKey = hourStart.ToString("yyyy-MM-dd HH:00");
                hourDict[hourKey] = (0L, 0L, 0L);
            }
        }

        // Fill actual data (only count this week's data)
        foreach (var record in records)
        {
            var recordDate = record.Timestamp.Date;
            if (recordDate >= weekStart && recordDate < weekEnd)
            {
                var hourKey = record.Timestamp.ToString("yyyy-MM-dd HH:00");
                if (hourDict.ContainsKey(hourKey))
                {
                    var existing = hourDict[hourKey];
                    hourDict[hourKey] = (
                        existing.promptTokens + record.PromptTokens,
                        existing.completionTokens + record.CompletionTokens,
                        existing.totalTokens + record.TotalTokens
                    );
                }
            }
        }

        // Convert to result list
        var result = hourDict.Select(kv => new
        {
            date = kv.Key,
            promptTokens = kv.Value.promptTokens,
            completionTokens = kv.Value.completionTokens,
            totalTokens = kv.Value.totalTokens
        }).OrderBy(x => x.date).Cast<object>().ToList();

        return result;
    }

    private List<object> GroupByHour(List<TokenUsageRecord> records, DateTime now)
    {
        var today = now.Date;
        var hourlyData = new List<object>();

        for (int hour = 0; hour < 24; hour++)
        {
            var hourStart = today.AddHours(hour);
            var hourEnd = hourStart.AddHours(1);
            
            var hourRecords = records.Where(r => r.Timestamp >= hourStart && r.Timestamp < hourEnd).ToList();
            
            hourlyData.Add(new
            {
                date = hourStart.ToString("yyyy-MM-dd HH:00"),
                promptTokens = hourRecords.Sum(r => r.PromptTokens),
                completionTokens = hourRecords.Sum(r => r.CompletionTokens),
                totalTokens = hourRecords.Sum(r => r.TotalTokens)
            });
        }

        return hourlyData;
    }

    private List<object> GroupByDay(List<TokenUsageRecord> records, DateTime now)
    {
        // Get time range start and end dates
        var range = BuildRangeFromParams();
        var (startDate, endDate) = range.GetRange();
        endDate = endDate.Date; // Only take date part

        // Create dictionary for all dates
        var dateDict = new Dictionary<string, (long promptTokens, long completionTokens, long totalTokens)>();
        for (var date = startDate.Date; date <= endDate; date = date.AddDays(1))
        {
            var dateKey = date.ToString("yyyy-MM-dd");
            dateDict[dateKey] = (0L, 0L, 0L);
        }

        // Fill actual data
        foreach (var record in records)
        {
            var dateKey = record.Timestamp.ToString("yyyy-MM-dd");
            if (dateDict.ContainsKey(dateKey))
            {
                var existing = dateDict[dateKey];
                dateDict[dateKey] = (
                    existing.promptTokens + record.PromptTokens,
                    existing.completionTokens + record.CompletionTokens,
                    existing.totalTokens + record.TotalTokens
                );
            }
        }

        // Convert to result list
        var result = dateDict.Select(kv => new
        {
            date = kv.Key,
            promptTokens = kv.Value.promptTokens,
            completionTokens = kv.Value.completionTokens,
            totalTokens = kv.Value.totalTokens
        }).OrderBy(x => x.date).Cast<object>().ToList();

        return result;
    }

    private List<object> GroupByMonth(List<TokenUsageRecord> records, DateTime now)
    {
        // Create dictionary for 12 months of current year
        var monthDict = new Dictionary<string, (long promptTokens, long completionTokens, long totalTokens)>();
        for (int month = 1; month <= 12; month++)
        {
            var monthKey = $"{now.Year}-{month:00}";
            monthDict[monthKey] = (0L, 0L, 0L);
        }

        // Fill actual data
        foreach (var record in records)
        {
            var monthKey = record.Timestamp.ToString("yyyy-MM");
            if (monthDict.ContainsKey(monthKey))
            {
                var existing = monthDict[monthKey];
                monthDict[monthKey] = (
                    existing.promptTokens + record.PromptTokens,
                    existing.completionTokens + record.CompletionTokens,
                    existing.totalTokens + record.TotalTokens
                );
            }
        }

        // Convert to result list
        var result = monthDict.Select(kv => new
        {
            date = kv.Key,
            promptTokens = kv.Value.promptTokens,
            completionTokens = kv.Value.completionTokens,
            totalTokens = kv.Value.totalTokens
        }).OrderBy(x => x.date).Cast<object>().ToList();

        return result;
    }

    private void Export()
    {
        if (_audit == null)
        {
            Response.StatusCode = 204;
            Response.Close();
            return;
        }

        var clientTypeFilter = GetQueryValue("clientType", "");
        var beingIdFilter = GetQueryValue("beingId", "");
        var range = BuildRangeFromParams();

        var query = new TokenUsageQuery
        {
            Range = range,
            AIClientType = string.IsNullOrEmpty(clientTypeFilter) ? null : clientTypeFilter,
            BeingId = string.IsNullOrEmpty(beingIdFilter) ? null : Guid.TryParse(beingIdFilter, out var bid) ? bid : (Guid?)null,
            GroupByAIClientType = false,
            GroupByBeingId = false
        };

        List<TokenUsageRecord> records = _audit.QueryRecords(query);

        var csv = new System.Text.StringBuilder();
        csv.AppendLine("Timestamp,BeingId,AIClientType,PromptTokens,CompletionTokens,TotalTokens,Success");
        foreach (var r in records)
        {
            csv.AppendLine($"{r.Timestamp:yyyy-MM-dd HH:mm:ss},{r.BeingId},{r.AIClientType},{r.PromptTokens},{r.CompletionTokens},{r.TotalTokens},{r.Success}");
        }

        Response.ContentType = "text/csv";
        Response.Headers["Content-Disposition"] = "attachment; filename=audit_export.csv";
        var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
        Response.ContentLength64 = bytes.Length;
        Response.OutputStream.Write(bytes, 0, bytes.Length);
        Response.Close();
    }

    private IncompleteDate BuildRangeFromParams()
    {
        var timeRange = GetQueryValue("timeRange", "month");
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

    private Dictionary<string, string> BuildBeingNameMap(List<TokenUsageRecord> records)
    {
        Dictionary<string, string> nameMap = new Dictionary<string, string>();

        if (_beingManager == null)
        {
            foreach (TokenUsageRecord record in records)
            {
                string guidStr = record.BeingId.ToString();
                if (!nameMap.ContainsKey(guidStr))
                    nameMap[guidStr] = guidStr;
            }
            return nameMap;
        }

        foreach (TokenUsageRecord record in records)
        {
            string guidStr = record.BeingId.ToString();
            if (nameMap.ContainsKey(guidStr))
                continue;

            SiliconBeingBase? being = _beingManager.GetBeing(record.BeingId);
            nameMap[guidStr] = being?.Name ?? guidStr;
        }

        return nameMap;
    }
}
