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
using SiliconLife.Common.Localization;
using SiliconLife.Fast.Web.Component;

namespace SiliconLife.Fast.Web;

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
        var loc = ((DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(((DefaultConfigData)Config.Instance.Data).Language));
        
        if (entries.Count == 0)
        {
            return new PComponent(loc.MemoryTimelineEmptyState)
                .Style("text-align: center; padding: 40px; color: var(--text-secondary);")
                .Render();
        }

        var grouped = entries
            .Select(e => new { Entry = e, Ts = ResolveTimestamp(e.Timestamp) })
            .ToList();

        // Group ALL entries (including summaries) by hierarchy
        var allGrouped = grouped
            .GroupBy(x => x.Ts.Year)
            .OrderByDescending(g => g.Key)
            .Select(yg => new
            {
                Year = yg.Key,
                Count = yg.Count(), // Count all entries including summaries
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
                                        Minutes = hg
                                            .GroupBy(x => x.Ts.Minute)
                                            .OrderByDescending(g => g.Key)
                                            .Select(mg => new
                                            {
                                                Minute = mg.Key,
                                                Count = mg.Count(),
                                                Items = mg.Select(x => ConvertToMemoryItem(x.Entry, beingId)).ToList()
                                            }).ToList()
                                    }).ToList()
                            }).ToList()
                    }).ToList()
            }).ToList();

        // Extract summaries for quick lookup
        var summariesByTime = grouped
            .Where(x => x.Entry.IsSummary)
            .GroupBy(x => new { x.Ts.Year, x.Ts.Month, x.Ts.Day, x.Ts.Hour, x.Ts.Minute, x.Ts.Second })
            .ToDictionary(g => g.Key, g => g.First());

        var tree = new DivComponent().Class("memory-tree");

        foreach (var year in allGrouped)
        {
            var yearSummaryKey = new { Year = year.Year, Month = 1, Day = 1, Hour = 0, Minute = 0, Second = 0 };
            var yearSummary = summariesByTime.ContainsKey(yearSummaryKey) ? summariesByTime[yearSummaryKey] : null;

            var yearDetails = new DetailsComponent().Open();
            yearDetails.AddSummary(new SpanComponent()
                .Style("font-size: 16px; font-weight: 600; padding: 8px; cursor: pointer;")
                .Text($"📅 {string.Format(loc.MemoryTimelineYearFormat, year.Year, year.Count)}"));
            if (yearSummary != null)
            {
                yearDetails.AddSummary(BuildSummaryBlock(loc.MemoryYearSummaryLabel, yearSummary.Entry.Content,
                    padding: 10, fontSizeLabel: 12, fontSizeContent: 13, marginTop: 6, summaryId: yearSummary.Entry.Id.ToString()));
            }

            var yearBody = new DivComponent().Style("padding-left: 20px;");
            foreach (var month in year.Months)
            {
                var monthSummaryKey = new { Year = year.Year, Month = month.Month, Day = 1, Hour = 0, Minute = 0, Second = 0 };
                var monthSummary = summariesByTime.ContainsKey(monthSummaryKey) ? summariesByTime[monthSummaryKey] : null;

                var monthDetails = new DetailsComponent();
                monthDetails.AddSummary(new SpanComponent()
                    .Style("font-size: 14px; padding: 6px; cursor: pointer;")
                    .Text($"📅 {string.Format(loc.MemoryTimelineMonthFormat, year.Year, month.Month, month.Count)}"));
                if (monthSummary != null)
                {
                    monthDetails.AddSummary(BuildSummaryBlock(loc.MemoryMonthSummaryLabel, monthSummary.Entry.Content,
                        padding: 8, fontSizeLabel: 11, fontSizeContent: 12, marginTop: 5, summaryId: monthSummary.Entry.Id.ToString()));
                }

                var monthBody = new DivComponent().Style("padding-left: 20px;");
                foreach (var day in month.Days)
                {
                    var daySummaryKey = new { Year = year.Year, Month = month.Month, Day = day.Day, Hour = 0, Minute = 0, Second = 0 };
                    var daySummary = summariesByTime.ContainsKey(daySummaryKey) ? summariesByTime[daySummaryKey] : null;

                    var moStr = month.Month.ToString().PadLeft(2, '0');
                    var dStr = day.Day.ToString().PadLeft(2, '0');

                    var dayDetails = new DetailsComponent();
                    dayDetails.AddSummary(new SpanComponent()
                        .Style("font-size: 13px; padding: 4px; cursor: pointer;")
                        .Text($"📅 {string.Format(loc.MemoryTimelineDayFormat, year.Year, moStr, dStr, day.Count)}"));
                    if (daySummary != null)
                    {
                        dayDetails.AddSummary(BuildSummaryBlock(loc.MemoryDaySummaryLabel, daySummary.Entry.Content,
                            padding: 6, fontSizeLabel: 11, fontSizeContent: 12, marginTop: 4, summaryId: daySummary.Entry.Id.ToString()));
                    }

                    var dayBody = new DivComponent().Style("padding-left: 20px;");
                    foreach (var hour in day.Hours)
                    {
                        var hourSummaryKey = new { Year = year.Year, Month = month.Month, Day = day.Day, Hour = hour.Hour, Minute = 0, Second = 0 };
                        var hourSummary = summariesByTime.ContainsKey(hourSummaryKey) ? summariesByTime[hourSummaryKey] : null;

                        var hStr = hour.Hour.ToString().PadLeft(2, '0');

                        var hourDetails = new DetailsComponent();
                        hourDetails.AddSummary(new SpanComponent()
                            .Style("font-size: 12px; padding: 4px; cursor: pointer;")
                            .Text($"🕐 {string.Format(loc.MemoryTimelineHourFormat, hStr, hour.Count)}"));
                        if (hourSummary != null)
                        {
                            hourDetails.AddSummary(BuildSummaryBlock(loc.MemoryHourSummaryLabel, hourSummary.Entry.Content,
                                padding: 6, fontSizeLabel: 11, fontSizeContent: 12, marginTop: 4, summaryId: hourSummary.Entry.Id.ToString()));
                        }

                        var hourBody = new DivComponent().Style("padding-left: 20px;");
                        foreach (var minute in hour.Minutes)
                        {
                            var minuteSummaryKey = new { Year = year.Year, Month = month.Month, Day = day.Day, Hour = hour.Hour, Minute = minute.Minute, Second = 0 };
                            var minuteSummary = summariesByTime.ContainsKey(minuteSummaryKey) ? summariesByTime[minuteSummaryKey] : null;

                            var mStr = minute.Minute.ToString().PadLeft(2, '0');

                            var minuteDetails = new DetailsComponent();
                            minuteDetails.AddSummary(new SpanComponent()
                                .Style("font-size: 11px; padding: 3px; cursor: pointer;")
                                .Text($"🕐 {string.Format(loc.MemoryTimelineMinuteFormat, hStr, mStr, minute.Count)}"));
                            if (minuteSummary != null)
                            {
                                minuteDetails.AddSummary(BuildSummaryBlock(loc.MemoryMinuteSummaryLabel, minuteSummary.Entry.Content,
                                    padding: 5, fontSizeLabel: 10, fontSizeContent: 11, marginTop: 3, summaryId: minuteSummary.Entry.Id.ToString()));
                            }

                            var minuteBody = new DivComponent().Style("padding-left: 20px;");
                            foreach (var memory in minute.Items)
                            {
                                minuteBody.Add(BuildMemoryCard(memory, loc));
                            }
                            minuteDetails.Add(minuteBody);

                            hourBody.Add(minuteDetails);
                        }
                        hourDetails.Add(hourBody);

                        dayBody.Add(hourDetails);
                    }
                    dayDetails.Add(dayBody);

                    monthBody.Add(dayDetails);
                }
                monthDetails.Add(monthBody);

                yearBody.Add(monthDetails);
            }
            yearDetails.Add(yearBody);

            tree.Add(yearDetails);
        }

        return tree.Render();
    }

    private static DivComponent BuildSummaryBlock(string label, string content,
        int padding, int fontSizeLabel, int fontSizeContent, int marginTop, string summaryId = "")
    {
        var summaryDiv = new DivComponent()
            .Style($"background: var(--bg-secondary); padding: {padding}px; border-radius: 6px; margin-top: {marginTop}px; border-left: 3px solid var(--accent-primary); font-weight: normal; cursor: pointer;")
            .Add(new DivComponent()
                .Style($"font-size: {fontSizeLabel}px; color: var(--text-secondary); margin-bottom: {Math.Max(2, marginTop - 2)}px;")
                .Add(new SpanComponent().Text($"📝 {label}")))
            .Add(new DivComponent()
                .Style($"font-size: {fontSizeContent}px; line-height: 1.6;")
                .Add(new SpanComponent().Text(content)));

        // Add click handler and data attributes if summaryId is provided
        if (!string.IsNullOrEmpty(summaryId))
        {
            summaryDiv.Attr("data-summary-id", summaryId)
                      .Attr("onclick", $"showSummaryDetail('{summaryId}')");
        }

        return summaryDiv;
    }

    private DivComponent BuildMemoryCard(Models.MemoryItem memory, DefaultLocalizationBase loc)
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

        var card = new DivComponent()
            .Class("memory-card")
            .Attr("data-id", memory.Id.ToString())
            .Style($"padding: 15px; margin-bottom: 15px; border-left: 4px solid {typeColor}; border-radius: 8px; position: relative; background: var(--bg-secondary); cursor: pointer;");

        // Header row: icon + timestamp + optional summary badge
        var headerRow = new DivComponent()
            .Style("display: flex; align-items: center; gap: 10px; margin-bottom: 8px;")
            .Add(new SpanComponent().Style("font-size: 18px;").Text(typeIcon))
            .Add(new SpanComponent()
                .Style("font-size: 12px; color: var(--text-secondary);")
                .Text(memory.TimestampDisplay));

        if (memory.IsSummary)
        {
            headerRow.Add(new SpanComponent()
                .Style("background: var(--accent-color); color: white; padding: 2px 8px; border-radius: 4px; font-size: 11px; margin-left: auto;")
                .Text(loc.MemorySummaryBadge));
        }
        card.Add(headerRow);

        // Content
        card.Add(new DivComponent()
            .Style("margin-bottom: 8px; line-height: 1.5;")
            .Add(new SpanComponent().Text(memory.Content)));

        // Keywords
        if (memory.Keywords.Count > 0)
        {
            var kwRow = new DivComponent()
                .Style("display: flex; gap: 5px; flex-wrap: wrap; margin-bottom: 5px;");
            foreach (var kw in memory.Keywords)
            {
                kwRow.Add(new SpanComponent()
                    .Style("background: var(--bg-tertiary); padding: 2px 8px; border-radius: 3px; font-size: 11px; color: var(--text-secondary);")
                    .Text($"#{kw}"));
            }
            card.Add(kwRow);
        }

        // Related beings
        if (memory.RelatedBeings.Count > 0)
        {
            card.Add(new DivComponent()
                .Style("font-size: 11px; color: var(--text-secondary);")
                .Add(new SpanComponent().Text(string.Format(loc.MemoryRelatedBeingsLabel, memory.RelatedBeings.Count))));
        }

        return card;
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
            // Year must be valid (1-9999)
            int year = d.Year >= 1 && d.Year <= 9999 ? d.Year : 1;
            
            // Month defaults to 1 if null or invalid
            int month = d.Month.HasValue && d.Month.Value >= 1 && d.Month.Value <= 12 
                ? d.Month.Value : 1;
            
            // Day defaults to 1 if null or invalid
            int day = d.Day.HasValue && d.Day.Value >= 1 && d.Day.Value <= 31 
                ? d.Day.Value : 1;
            
            // Hour defaults to 0 if null or invalid
            int hour = d.Hour.HasValue && d.Hour.Value >= 0 && d.Hour.Value <= 23 
                ? d.Hour.Value : 0;
            
            // Minute defaults to 0 if null or invalid
            int minute = d.Minute.HasValue && d.Minute.Value >= 0 && d.Minute.Value <= 59 
                ? d.Minute.Value : 0;
            
            // Second defaults to 0 if null or invalid
            int second = d.Second.HasValue && d.Second.Value >= 0 && d.Second.Value <= 59 
                ? d.Second.Value : 0;
            
            return new DateTime(year, month, day, hour, minute, second);
        }
        catch
        {
            // Fallback to current time if everything fails
            return DateTime.Now;
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
