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
using SiliconLife.App.Web.Models;
using SiliconLife.App.Web;

namespace SiliconLife.App.Web.Controllers;

[WebCode]
public class AuditController : Controller
{
    private readonly SkinManager _skinManager;
    private readonly AuditLogger? _auditLogger;
    private readonly SiliconBeingManager? _beingManager;

    public AuditController()
    {
        var locator = ServiceLocator.Instance;
        _skinManager = locator.GetService<SkinManager>()!;
        _auditLogger = locator.AuditLogger;
        _beingManager = locator.BeingManager;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/audit";

        if (path == "/audit" || path == "/audit/index")
            Index();
        else if (path == "/api/audit/list")
            GetList();
        else if (path == "/api/audit/summary")
            GetSummary();
        else if (path == "/api/audit/beings")
            GetBeings();
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

    private void GetList()
    {
        if (_auditLogger == null)
        {
            RenderJson(new { entries = new List<object>(), total = 0, page = 1, totalPages = 0 });
            return;
        }

        var page = int.TryParse(GetQueryValue("page", "1"), out var p) ? Math.Max(1, p) : 1;
        var pageSize = 50;
        var permissionTypeFilter = GetQueryValue("permissionType", "");
        var resultFilter = GetQueryValue("result", "");
        var beingIdFilter = GetQueryValue("beingId", "");
        var startDateStr = GetQueryValue("startDate", "");
        var endDateStr = GetQueryValue("endDate", "");

        IncompleteDate? range = null;
        if (!string.IsNullOrEmpty(startDateStr) && DateTime.TryParse(startDateStr, out var startDate))
        {
            if (!string.IsNullOrEmpty(endDateStr) && DateTime.TryParse(endDateStr, out var endDate))
            {
                range = new IncompleteDate(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute);
            }
            else
            {
                range = new IncompleteDate(startDate.Year, startDate.Month, startDate.Day);
            }
        }

        List<AuditEntry> entries = _auditLogger.Query(range);

        if (!string.IsNullOrEmpty(beingIdFilter) && Guid.TryParse(beingIdFilter, out var beingId))
        {
            entries = entries.Where(e => e.CallerId == beingId).ToList();
        }

        if (!string.IsNullOrEmpty(permissionTypeFilter) && Enum.TryParse<PermissionType>(permissionTypeFilter, out var permType))
        {
            entries = entries.Where(e => e.PermissionType == permType).ToList();
        }

        if (!string.IsNullOrEmpty(resultFilter) && Enum.TryParse<PermissionResult>(resultFilter, out var permResult))
        {
            entries = entries.Where(e => e.Result == permResult).ToList();
        }

        if (!string.IsNullOrEmpty(startDateStr) && DateTime.TryParse(startDateStr, out var startDt))
        {
            entries = entries.Where(e =>
            {
                var ts = BuildDateTime(e.Timestamp);
                return ts >= startDt;
            }).ToList();
        }

        if (!string.IsNullOrEmpty(endDateStr) && DateTime.TryParse(endDateStr, out var endDt))
        {
            entries = entries.Where(e =>
            {
                var ts = BuildDateTime(e.Timestamp);
                return ts <= endDt;
            }).ToList();
        }

        entries = entries.OrderByDescending(e => BuildDateTime(e.Timestamp)).ToList();

        var total = entries.Count;
        var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));
        var pagedEntries = entries.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var beingNameMap = BuildBeingNameMap(pagedEntries);

        var result = pagedEntries.Select(e => new
        {
            timestamp = FormatTimestamp(e.Timestamp),
            callerId = e.CallerId.ToString(),
            callerName = beingNameMap.TryGetValue(e.CallerId.ToString(), out var name) ? name : e.CallerId.ToString(),
            permissionType = e.PermissionType.ToString(),
            resource = e.Resource,
            result = e.Result.ToString(),
            reason = e.Reason
        }).ToList();

        RenderJson(new { entries = result, total, page, totalPages });
    }

    private void GetSummary()
    {
        if (_auditLogger == null)
        {
            RenderJson(new { total = 0, allowed = 0, denied = 0, askUser = 0 });
            return;
        }

        var beingIdFilter = GetQueryValue("beingId", "");
        var startDateStr = GetQueryValue("startDate", "");
        var endDateStr = GetQueryValue("endDate", "");

        IncompleteDate? range = null;
        if (!string.IsNullOrEmpty(startDateStr) && DateTime.TryParse(startDateStr, out var startDate))
        {
            range = new IncompleteDate(startDate.Year, startDate.Month, startDate.Day);
        }

        List<AuditEntry> entries = _auditLogger.Query(range);

        if (!string.IsNullOrEmpty(beingIdFilter) && Guid.TryParse(beingIdFilter, out var beingId))
        {
            entries = entries.Where(e => e.CallerId == beingId).ToList();
        }

        if (!string.IsNullOrEmpty(startDateStr) && DateTime.TryParse(startDateStr, out var startDt))
        {
            entries = entries.Where(e => BuildDateTime(e.Timestamp) >= startDt).ToList();
        }

        if (!string.IsNullOrEmpty(endDateStr) && DateTime.TryParse(endDateStr, out var endDt))
        {
            entries = entries.Where(e => BuildDateTime(e.Timestamp) <= endDt).ToList();
        }

        RenderJson(new
        {
            total = entries.Count,
            allowed = entries.Count(e => e.Result == PermissionResult.Allowed),
            denied = entries.Count(e => e.Result == PermissionResult.Denied),
            askUser = entries.Count(e => e.Result == PermissionResult.AskUser)
        });
    }

    private void GetBeings()
    {
        if (_auditLogger == null)
        {
            RenderJson(new List<object>());
            return;
        }

        List<AuditEntry> entries = _auditLogger.Query(null);
        var beingIds = entries.Select(e => e.CallerId).Distinct().ToList();

        var result = beingIds.Select(id =>
        {
            var being = _beingManager?.GetBeing(id);
            return new
            {
                id = id.ToString(),
                displayName = being?.Name ?? id.ToString()
            };
        }).ToList();

        RenderJson(result);
    }

    private Dictionary<string, string> BuildBeingNameMap(List<AuditEntry> entries)
    {
        var nameMap = new Dictionary<string, string>();

        if (_beingManager == null)
        {
            foreach (var entry in entries)
            {
                var guidStr = entry.CallerId.ToString();
                if (!nameMap.ContainsKey(guidStr))
                    nameMap[guidStr] = guidStr;
            }
            return nameMap;
        }

        foreach (var entry in entries)
        {
            var guidStr = entry.CallerId.ToString();
            if (nameMap.ContainsKey(guidStr))
                continue;

            var being = _beingManager.GetBeing(entry.CallerId);
            nameMap[guidStr] = being?.Name ?? guidStr;
        }

        return nameMap;
    }

    private static DateTime BuildDateTime(IncompleteDate date)
    {
        return new DateTime(
            date.Year,
            date.Month ?? 1,
            date.Day ?? 1,
            date.Hour ?? 0,
            date.Minute ?? 0,
            date.Second ?? 0);
    }

    private static string FormatTimestamp(IncompleteDate date)
    {
        var dt = BuildDateTime(date);
        return dt.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
