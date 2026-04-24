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

namespace SiliconLife.Default.Web;

[WebCode]
public class MemoryController : Controller
{
    private readonly SkinManager _skinManager;
    private readonly SiliconBeingManager _beingManager;

    public MemoryController()
    {
        _skinManager = ServiceLocator.Instance.GetService<SkinManager>()!;
        _beingManager = ServiceLocator.Instance.BeingManager!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/memory";

        if (path == "/memory" || path == "/memory/index")
            Index();
        else if (path == "/api/memory/list")
            GetList();
        else if (path.StartsWith("/api/memory/detail/"))
            GetDetail();
        else if (path == "/api/memory/stats")
            GetStats();
        else if (path == "/api/memory/search")
            Search();
        else if (path == "/api/memory/beings")
            GetBeings();
        else if (path.StartsWith("/api/memory/trace/"))
            TraceOriginal();
        else if (path == "/api/memory/timeline-html")
            GetTimelineHtml();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var beingId = GetQueryParam("beingId");
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.MemoryView();
        var vm = new Models.MemoryViewModel 
        { 
            Skin = skin, 
            ActiveMenu = "memory",
            CurrentBeingId = string.IsNullOrWhiteSpace(beingId) ? null : Guid.Parse(beingId)
        };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetList()
    {
        try
        {
            var beingId = GetQueryParam("beingId");
            var page = int.Parse(GetQueryParam("page", "1"));
            var pageSize = int.Parse(GetQueryParam("pageSize", "20"));
            var type = GetQueryParam("type");
            var keyword = GetQueryParam("keyword");
            var startDate = GetQueryParam("startDate");
            var endDate = GetQueryParam("endDate");
            var showSummariesOnly = GetQueryParam("showSummaries", "all");

            if (string.IsNullOrWhiteSpace(beingId))
            {
                RenderJson(new { error = "Missing beingId parameter", data = new List<object>() });
                return;
            }

            var being = _beingManager.GetBeing(Guid.Parse(beingId));
            if (being?.Memory == null)
            {
                RenderJson(new { error = "Memory system not available", data = new List<object>() });
                return;
            }

            // Get all entries first, then apply filters
            var entries = being.Memory.QueryAll(0);

            // Apply keyword filter
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                entries = being.Memory.Search(keyword, 0);
            }

            // Apply type filter (supports comma-separated multiple types)
            if (!string.IsNullOrWhiteSpace(type))
            {
                var types = type.Split(',', StringSplitOptions.RemoveEmptyEntries);
                entries = entries.Where(e => !string.IsNullOrWhiteSpace(e.Type) && types.Contains(e.Type)).ToList();
            }

            // Apply date range filter
            if (!string.IsNullOrWhiteSpace(startDate) || !string.IsNullOrWhiteSpace(endDate))
            {
                DateTime? startDt = null;
                DateTime? endDt = null;
                
                if (DateTime.TryParse(startDate, out var parsedStart))
                    startDt = parsedStart;
                if (DateTime.TryParse(endDate, out var parsedEnd))
                    endDt = parsedEnd;

                entries = entries.Where(e =>
                {
                    var entryDate = ResolveTimestamp(e.Timestamp);
                    if (startDt.HasValue && entryDate < startDt.Value) return false;
                    if (endDt.HasValue && entryDate > endDt.Value) return false;
                    return true;
                }).ToList();
            }

            // Apply summary filter
            if (showSummariesOnly == "summary")
            {
                entries = entries.Where(e => e.IsSummary).ToList();
            }
            else if (showSummariesOnly == "original")
            {
                entries = entries.Where(e => !e.IsSummary).ToList();
            }

            var totalCount = entries.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var pagedEntries = entries.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var items = pagedEntries.Select(e => ConvertToMemoryItem(e, being.Id)).ToList();

            RenderJson(new
            {
                success = true,
                data = items,
                pagination = new
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalCount = totalCount,
                    totalPages = totalPages
                }
            });
        }
        catch (Exception ex)
        {
            RenderJson(new { error = ex.Message, data = new List<object>() });
        }
    }

    private void GetTimelineHtml()
    {
        try
        {
            var beingId = GetQueryParam("beingId");
            var type = GetQueryParam("type");
            var keyword = GetQueryParam("keyword");
            var startDate = GetQueryParam("startDate");
            var endDate = GetQueryParam("endDate");
            var showSummaries = GetQueryParam("showSummaries", "all");

            if (string.IsNullOrWhiteSpace(beingId))
            {
                RenderJson(new { error = "Missing beingId parameter", html = "" });
                return;
            }

            var being = _beingManager.GetBeing(Guid.Parse(beingId));
            if (being?.Memory == null)
            {
                RenderJson(new { error = "Memory system not available", html = "" });
                return;
            }

            var entries = being.Memory.QueryAll(0);

            // Apply keyword filter
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                entries = being.Memory.Search(keyword, 0);
            }

            // Apply type filter
            if (!string.IsNullOrWhiteSpace(type))
            {
                var types = type.Split(',', StringSplitOptions.RemoveEmptyEntries);
                entries = entries.Where(e => !string.IsNullOrWhiteSpace(e.Type) && types.Contains(e.Type)).ToList();
            }

            // Apply date range filter
            if (!string.IsNullOrWhiteSpace(startDate) || !string.IsNullOrWhiteSpace(endDate))
            {
                DateTime? startDt = null;
                DateTime? endDt = null;
                if (DateTime.TryParse(startDate, out var parsedStart))
                    startDt = parsedStart;
                if (DateTime.TryParse(endDate, out var parsedEnd))
                    endDt = parsedEnd;

                entries = entries.Where(e =>
                {
                    var entryDate = ResolveTimestamp(e.Timestamp);
                    if (startDt.HasValue && entryDate < startDt.Value) return false;
                    if (endDt.HasValue && entryDate > endDt.Value) return false;
                    return true;
                }).ToList();
            }

            // Apply summary filter
            if (showSummaries == "summary")
            {
                entries = entries.Where(e => e.IsSummary).ToList();
            }
            else if (showSummaries == "original")
            {
                entries = entries.Where(e => !e.IsSummary).ToList();
            }

            var html = BuildTimelineHtml(entries, being.Id);
            RenderJson(new { success = true, html = html });
        }
        catch (Exception ex)
        {
            RenderJson(new { error = ex.Message, html = "" });
        }
    }

    private string BuildTimelineHtml(List<MemoryEntry> entries, Guid beingId)
    {
        if (entries.Count == 0)
            return "<p style='text-align: center; padding: 40px; color: var(--text-secondary);'>暂无记忆数据</p>";

        var sb = new System.Text.StringBuilder();
        sb.Append("<div class='memory-tree'>");

        var grouped = entries
            .Select(e => new { Entry = e, Ts = ResolveTimestamp(e.Timestamp) })
            .GroupBy(x => x.Ts.Year)
            .OrderByDescending(g => g.Key)
            .Select(yg => new
            {
                Year = yg.Key,
                Count = yg.Count(),
                Months = yg
                    .GroupBy(x => x.Ts.Month)
                    .OrderByDescending(g => g.Key)
                    .Select(mg => new
                    {
                        Month = mg.Key,
                        Count = mg.Count(),
                        Days = mg
                            .GroupBy(x => x.Ts.Day)
                            .OrderByDescending(g => g.Key)
                            .Select(dg => new
                            {
                                Day = dg.Key,
                                Count = dg.Count(),
                                Hours = dg
                                    .GroupBy(x => x.Ts.Hour)
                                    .OrderByDescending(g => g.Key)
                                    .Select(hg => new
                                    {
                                        Hour = hg.Key,
                                        Count = hg.Count(),
                                        Items = hg.Select(x => ConvertToMemoryItem(x.Entry, beingId)).ToList()
                                    }).ToList()
                            }).ToList()
                    }).ToList()
            }).ToList();

        foreach (var year in grouped)
        {
            sb.Append($"<details open><summary style='font-size: 16px; font-weight: 600; padding: 8px; cursor: pointer;'>📅 {year.Year}年 ({year.Count}条)</summary><div style='padding-left: 20px;'>");
            foreach (var month in year.Months)
            {
                sb.Append($"<details><summary style='font-size: 14px; padding: 6px; cursor: pointer;'>📅 {year.Year}年{month.Month}月 ({month.Count}条)</summary><div style='padding-left: 20px;'>");
                foreach (var day in month.Days)
                {
                    var moStr = month.Month.ToString().PadLeft(2, '0');
                    var dStr = day.Day.ToString().PadLeft(2, '0');
                    sb.Append($"<details><summary style='font-size: 13px; padding: 4px; cursor: pointer;'>📅 {year.Year}-{moStr}-{dStr} ({day.Count}条)</summary><div style='padding-left: 20px;'>");
                    foreach (var hour in day.Hours)
                    {
                        var hStr = hour.Hour.ToString().PadLeft(2, '0');
                        sb.Append($"<details><summary style='font-size: 12px; padding: 4px; cursor: pointer;'>🕐 {hStr}:00 ({hour.Count}条)</summary><div style='padding-left: 20px;'>");
                        foreach (var memory in hour.Items)
                        {
                            sb.Append(RenderMemoryCardHtml(memory));
                        }
                        sb.Append("</div></details>");
                    }
                    sb.Append("</div></details>");
                }
                sb.Append("</div></details>");
            }
            sb.Append("</div></details>");
        }

        sb.Append("</div>");
        return sb.ToString();
    }

    private string RenderMemoryCardHtml(Models.MemoryItem memory)
    {
        var typeColor = "var(--border-color)";
        var typeIcon = "📝";
        switch (memory.Type)
        {
            case "chat": typeColor = "#4CAF50"; typeIcon = "💬"; break;
            case "tool_call": typeColor = "#2196F3"; typeIcon = "🔧"; break;
            case "task": typeColor = "#FF9800"; typeIcon = "📋"; break;
            case "timer": typeColor = "#9C27B0"; typeIcon = "⏰"; break;
        }

        var sb = new System.Text.StringBuilder();
        sb.Append($"<div class='memory-card' data-id='{memory.Id}' style='padding: 15px; margin-bottom: 15px; border-left: 4px solid {typeColor}; border-radius: 8px; position: relative; background: var(--bg-secondary); cursor: pointer;'>");
        sb.Append("<div style='display: flex; align-items: center; gap: 10px; margin-bottom: 8px;'>");
        sb.Append($"<span style='font-size: 18px;'>{typeIcon}</span>");
        sb.Append($"<span style='font-size: 12px; color: var(--text-secondary);'>{memory.TimestampDisplay}</span>");
        if (memory.IsSummary)
        {
            sb.Append("<span style='background: var(--accent-color); color: white; padding: 2px 8px; border-radius: 4px; font-size: 11px; margin-left: auto;'>压缩总结</span>");
        }
        sb.Append("</div>");
        sb.Append($"<div style='margin-bottom: 8px; line-height: 1.5;'>{System.Web.HttpUtility.HtmlEncode(memory.Content)}</div>");

        if (memory.Keywords.Count > 0)
        {
            sb.Append("<div style='display: flex; gap: 5px; flex-wrap: wrap; margin-bottom: 5px;'>");
            foreach (var kw in memory.Keywords)
            {
                sb.Append($"<span style='background: var(--bg-tertiary); padding: 2px 8px; border-radius: 3px; font-size: 11px; color: var(--text-secondary);'>#{System.Web.HttpUtility.HtmlEncode(kw)}</span>");
            }
            sb.Append("</div>");
        }

        if (memory.RelatedBeings.Count > 0)
        {
            sb.Append($"<div style='font-size: 11px; color: var(--text-secondary);'>👥 关联: {memory.RelatedBeings.Count} 个智能体</div>");
        }

        sb.Append("</div>");
        return sb.ToString();
    }

    private void GetDetail()
    {
        try
        {
            var id = GetRouteParameter("id");
            var beingId = GetQueryParam("beingId");

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(beingId))
            {
                RenderJson(new { error = "Missing required parameters" });
                return;
            }

            var being = _beingManager.GetBeing(Guid.Parse(beingId));
            if (being?.Memory == null)
            {
                RenderJson(new { error = "Memory system not available" });
                return;
            }

            var allEntries = being.Memory.QueryAll(0);
            var entry = allEntries.FirstOrDefault(e => e.Id == Guid.Parse(id));

            if (entry == null)
            {
                RenderJson(new { error = "Memory entry not found" });
                return;
            }

            RenderJson(new
            {
                success = true,
                data = ConvertToMemoryItem(entry, being.Id)
            });
        }
        catch (Exception ex)
        {
            RenderJson(new { error = ex.Message });
        }
    }

    private void GetStats()
    {
        try
        {
            var beingId = GetQueryParam("beingId");

            if (string.IsNullOrWhiteSpace(beingId))
            {
                RenderJson(new { error = "Missing beingId parameter" });
                return;
            }

            var being = _beingManager.GetBeing(Guid.Parse(beingId));
            if (being?.Memory == null)
            {
                RenderJson(new { error = "Memory system not available" });
                return;
            }

            var stats = being.Memory.GetStatistics();
            var allEntries = being.Memory.QueryAll(0);
            var summaryCount = allEntries.Count(e => e.IsSummary);
            var originalCount = allEntries.Count(e => !e.IsSummary);
            
            // Calculate type distribution
            var typeDistribution = allEntries
                .Where(e => !string.IsNullOrWhiteSpace(e.Type))
                .GroupBy(e => e.Type!)
                .ToDictionary(g => g.Key, g => g.Count());
            
            // Calculate keyword frequency (top 20)
            var keywordFrequency = allEntries
                .SelectMany(e => e.Keywords)
                .Where(k => !string.IsNullOrWhiteSpace(k))
                .GroupBy(k => k.ToLowerInvariant())
                .OrderByDescending(g => g.Count())
                .Take(20)
                .ToDictionary(g => g.Key, g => g.Count());

            RenderJson(new
            {
                success = true,
                data = new
                {
                    totalEntries = stats.TotalEntries,
                    oldestEntry = stats.OldestEntry?.ToString(),
                    newestEntry = stats.NewestEntry?.ToString(),
                    summaryCount = summaryCount,
                    originalCount = originalCount,
                    typeDistribution = typeDistribution,
                    keywordFrequency = keywordFrequency
                }
            });
        }
        catch (Exception ex)
        {
            RenderJson(new { error = ex.Message });
        }
    }

    private void Search()
    {
        try
        {
            var beingId = GetQueryParam("beingId");
            var keyword = GetQueryParam("keyword");
            var maxResults = int.Parse(GetQueryParam("maxResults", "50"));

            if (string.IsNullOrWhiteSpace(beingId) || string.IsNullOrWhiteSpace(keyword))
            {
                RenderJson(new { error = "Missing required parameters" });
                return;
            }

            var being = _beingManager.GetBeing(Guid.Parse(beingId));
            if (being?.Memory == null)
            {
                RenderJson(new { error = "Memory system not available" });
                return;
            }

            var results = being.Memory.Search(keyword, maxResults);
            var items = results.Select(e => ConvertToMemoryItem(e, being.Id)).ToList();

            RenderJson(new
            {
                success = true,
                data = items,
                count = items.Count
            });
        }
        catch (Exception ex)
        {
            RenderJson(new { error = ex.Message });
        }
    }

    private void GetBeings()
    {
        try
        {
            var beings = _beingManager.GetAllBeings();
            var beingInfos = beings.Select(b => new Models.BeingInfo
            {
                Id = b.Id,
                Name = b.Name ?? b.Id.ToString()
            }).ToList();

            RenderJson(new
            {
                success = true,
                data = beingInfos
            });
        }
        catch (Exception ex)
        {
            RenderJson(new { error = ex.Message });
        }
    }

    private void TraceOriginal()
    {
        try
        {
            var summaryId = GetRouteParameter("id");
            var beingId = GetQueryParam("beingId");

            if (string.IsNullOrWhiteSpace(summaryId) || string.IsNullOrWhiteSpace(beingId))
            {
                RenderJson(new { error = "Missing required parameters" });
                return;
            }

            var being = _beingManager.GetBeing(Guid.Parse(beingId));
            if (being?.Memory == null)
            {
                RenderJson(new { error = "Memory system not available" });
                return;
            }

            var allEntries = being.Memory.QueryAll(0);
            var summaryEntry = allEntries.FirstOrDefault(e => e.Id == Guid.Parse(summaryId));

            if (summaryEntry == null || !summaryEntry.IsSummary)
            {
                RenderJson(new { error = "Summary entry not found" });
                return;
            }

            // Return all original entries (non-summary) from the same time period
            var originalEntries = allEntries
                .Where(e => !e.IsSummary && e.Timestamp <= summaryEntry.Timestamp)
                .OrderByDescending(e => e.Timestamp)
                .Take(20)
                .Select(e => ConvertToMemoryItem(e, being.Id))
                .ToList();

            RenderJson(new
            {
                success = true,
                data = new
                {
                    summary = ConvertToMemoryItem(summaryEntry, being.Id),
                    originalEntries = originalEntries
                }
            });
        }
        catch (Exception ex)
        {
            RenderJson(new { error = ex.Message });
        }
    }

    private Models.MemoryItem ConvertToMemoryItem(MemoryEntry entry, Guid beingId)
    {
        var timestamp = ResolveTimestamp(entry.Timestamp);
        return new Models.MemoryItem
        {
            Id = entry.Id,
            Content = entry.Content,
            IsSummary = entry.IsSummary,
            RelatedBeings = entry.RelatedBeings,
            CreatedAt = timestamp,
            TimestampDisplay = timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
            Type = entry.Type,
            Keywords = entry.Keywords
        };
    }

    private static DateTime ResolveTimestamp(IncompleteDate d)
    {
        try
        {
            return new DateTime(
                Math.Max(1, d.Year),
                Math.Max(1, d.Month ?? 1),
                Math.Max(1, d.Day ?? 1),
                d.Hour ?? 0,
                d.Minute ?? 0,
                d.Second ?? 0);
        }
        catch
        {
            return DateTime.MinValue;
        }
    }

    private string GetQueryParam(string name, string defaultValue = "")
    {
        var query = Request.Url?.Query ?? "";
        var pairs = query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries);
        foreach (var pair in pairs)
        {
            var parts = pair.Split('=', 2);
            if (parts[0] == name && parts.Length > 1)
                return Uri.UnescapeDataString(parts[1]);
        }
        return defaultValue;
    }

    private string GetRouteParameter(string name)
    {
        // Extract from path manually since RouteParameters is not available
        var path = Request.Url?.AbsolutePath ?? "";
        var parts = path.Split('/');
        
        // For /api/memory/detail/{id} or /api/memory/trace/{id}
        if (parts.Length >= 5)
            return parts[4];
        
        return "";
    }
}
