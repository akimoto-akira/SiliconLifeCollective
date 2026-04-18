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
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.MemoryView();
        var vm = new Models.MemoryViewModel { Skin = skin, ActiveMenu = "memory" };
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

            List<MemoryEntry> entries;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                // Use search
                entries = being.Memory.Search(keyword, 0);
            }
            else if (!string.IsNullOrWhiteSpace(type))
            {
                // Filter by type
                entries = being.Memory.GetByType(type, 0);
            }
            else
            {
                // Get all
                entries = being.Memory.QueryAll(0);
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

            RenderJson(new
            {
                success = true,
                data = new
                {
                    totalEntries = stats.TotalEntries,
                    oldestEntry = stats.OldestEntry?.ToString(),
                    newestEntry = stats.NewestEntry?.ToString(),
                    summaryCount = summaryCount
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
        => new(d.Year, d.Month ?? 1, d.Day ?? 1,
               d.Hour ?? 0, d.Minute ?? 0, d.Second ?? 0);

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
