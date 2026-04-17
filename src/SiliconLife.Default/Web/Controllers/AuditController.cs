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

    public AuditController()
    {
        var locator = ServiceLocator.Instance;
        _skinManager = locator.GetService<SkinManager>()!;
        _audit = locator.TokenUsageAudit;
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
        var range = BuildRangeFromParams();

        var query = new TokenUsageQuery
        {
            Range = range,
            AIClientType = string.IsNullOrEmpty(clientTypeFilter) ? null : clientTypeFilter,
            BeingId = string.IsNullOrEmpty(beingIdFilter) ? null : Guid.TryParse(beingIdFilter, out var bid) ? bid : (Guid?)null,
            GroupByAIClientType = true,
            GroupByBeingId = true
        };

        TokenUsageSummary summary = _audit.QuerySummary(query);

        var byClient = summary.ByAIClientType.Select(kvp => new AuditSummaryItem
        {
            Key = kvp.Key,
            RequestCount = kvp.Value.RequestCount,
            TotalPromptTokens = kvp.Value.TotalPromptTokens,
            TotalCompletionTokens = kvp.Value.TotalCompletionTokens,
            TotalTokens = kvp.Value.TotalTokens
        }).ToList();

        var byBeing = summary.ByBeingId.Select(kvp => new AuditSummaryItem
        {
            Key = kvp.Key.ToString(),
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
            byBeing
        });
    }

    private void GetTrend()
    {
        if (_audit == null)
        {
            RenderJson(new { points = new List<object>() });
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

        var grouped = records
            .GroupBy(r => r.Timestamp.ToString("yyyy-MM-dd"))
            .OrderBy(g => g.Key)
            .Select(g => new
            {
                date = g.Key,
                promptTokens = g.Sum(r => r.PromptTokens),
                completionTokens = g.Sum(r => r.CompletionTokens),
                totalTokens = g.Sum(r => r.TotalTokens)
            })
            .ToList();

        RenderJson(new { points = grouped });
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
            "week" => BuildWeekRange(now),
            "month" => new IncompleteDate(now.Year, now.Month),
            "year" => new IncompleteDate(now.Year),
            _ => new IncompleteDate(now.Year, now.Month)
        };
    }

    private static IncompleteDate BuildWeekRange(DateTime now)
    {
        var weekStart = now.Date.AddDays(-(int)now.DayOfWeek);
        return new IncompleteDate(weekStart.Year, weekStart.Month, weekStart.Day);
    }
}
