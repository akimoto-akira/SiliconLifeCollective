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
public class LogController : Controller
{
    private const int DefaultPageSize = 100;

    private readonly SkinManager _skinManager;
    private readonly SiliconBeingManager _beingManager;

    public LogController()
    {
        _skinManager = ServiceLocator.Instance.GetService<SkinManager>()!;
        _beingManager = ServiceLocator.Instance.BeingManager!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/logs";

        if (path == "/logs" || path == "/logs/index")
            Index();
        else if (path == "/api/logs/list")
            GetList();
        else if (path == "/api/logs/beings")
            GetBeings();
        else if (path == "/api/logs/levels")
            GetLevels();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.LogView();
        var vm = new Models.LogViewModel { Skin = skin, ActiveMenu = "logs" };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetList()
    {
        var levelFilter = GetQueryValue("level", "");
        var beingFilter = GetQueryValue("beingId", "");
        var startDateStr = GetQueryValue("startDate", "");
        var endDateStr = GetQueryValue("endDate", "");
        var page = int.TryParse(GetQueryValue("page", "1"), out int p) ? p : 1;
        var pageSize = int.TryParse(GetQueryValue("pageSize", DefaultPageSize.ToString()), out int ps) ? ps : DefaultPageSize;

        DateTime? startTime = null;
        DateTime? endTime = null;
        
        if (!string.IsNullOrEmpty(startDateStr) && DateTime.TryParse(startDateStr, out var start))
        {
            startTime = start;
        }
        
        if (!string.IsNullOrEmpty(endDateStr) && DateTime.TryParse(endDateStr, out var end))
        {
            endTime = end;
        }

        Guid? beingIdFilter = null;
        bool systemOnly = false;
        
        if (!string.IsNullOrEmpty(beingFilter))
        {
            if (beingFilter == "system")
            {
                systemOnly = true;
            }
            else if (Guid.TryParse(beingFilter, out var beingId))
            {
                beingIdFilter = beingId;
            }
        }

        LogLevel? level = null;
        if (!string.IsNullOrEmpty(levelFilter))
        {
            if (Enum.TryParse<LogLevel>(levelFilter, true, out var parsedLevel))
            {
                level = parsedLevel;
            }
        }

        // Use LogManager to read logs instead of direct file access
        var logEntries = LogManager.Instance.ReadLogs(
            startTime: startTime,
            endTime: endTime,
            beingId: beingIdFilter,
            systemOnly: systemOnly,
            levelFilter: level,
            maxCount: 0); // No limit here, we'll paginate

        var logs = logEntries.Select(e => new LogItem
        {
            Timestamp = e.Timestamp,
            Level = e.Level.ToString(),
            Category = e.Category,
            Message = e.Message,
            Details = e.Exception?.ToString(),
            BeingId = e.BeingId
        }).ToList();

        var total = logs.Count;
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);
        var pagedLogs = logs.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        RenderJson(new
        {
            logs = pagedLogs,
            page,
            pageSize,
            total,
            totalPages
        });
    }

    private void GetBeings()
    {
        var beings = _beingManager.GetAllBeings();
        var list = beings.Select(b => new
        {
            id = b.Id.ToString(),
            displayName = $"{b.Name} ({b.Id})"
        }).ToList();

        RenderJson(list);
    }

    private void GetLevels()
    {
        var config = Config.Instance.Data as DefaultConfigData;
        var language = config?.Language ?? Language.EnUS;
        
        if (!LocalizationManager.Instance.TryGetLocalization(language, out var localization))
        {
            RenderJson(new { error = "Localization not found" });
            return;
        }

        var levels = new[] 
        {
            new { value = "Trace", displayName = localization!.GetLogLevelName(LogLevel.Trace) },
            new { value = "Debug", displayName = localization.GetLogLevelName(LogLevel.Debug) },
            new { value = "Info", displayName = localization.GetLogLevelName(LogLevel.Information) },
            new { value = "Warning", displayName = localization.GetLogLevelName(LogLevel.Warning) },
            new { value = "Error", displayName = localization.GetLogLevelName(LogLevel.Error) }
        };

        RenderJson(levels);
    }
}
