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
public class TimerController : Controller
{
    private readonly SiliconBeingManager _beingManager;
    private readonly SkinManager _skinManager;

    public TimerController()
    {
        var locator = ServiceLocator.Instance;
        _beingManager = locator.BeingManager!;
        _skinManager = locator.GetService<SkinManager>()!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/timers";

        if (path == "/timers" || path == "/timers/index")
            Index();
        else if (path == "/api/timers/list")
            GetList();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.TimerView();
        var vm = new Models.TimerViewModel { Skin = skin, ActiveMenu = "timers" };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetList()
    {
        var beingIdStr = Request.QueryString["beingId"];
        List<object> allTimers = new();

        if (!string.IsNullOrEmpty(beingIdStr) && Guid.TryParse(beingIdStr, out var beingId))
        {
            var being = _beingManager.GetBeing(beingId);
            if (being?.TimerSystem != null)
            {
                var timers = GetTimersFromSystem(being.TimerSystem);
                allTimers.AddRange(timers);
            }
        }
        else
        {
            var beings = _beingManager.GetAllBeings();
            foreach (var being in beings)
            {
                if (being.TimerSystem != null)
                {
                    var timers = GetTimersFromSystem(being.TimerSystem);
                    allTimers.AddRange(timers);
                }
            }
        }

        RenderJson(allTimers);
    }

    private static List<object> GetTimersFromSystem(TimerSystem timerSystem)
    {
        var timers = timerSystem.GetAll();
        var result = new List<object>();
        var registry = CalendarTool.BuildCalendarRegistry();

        foreach (var timer in timers)
        {
            string calendarDesc = GetCalendarDescription(timer.CalendarId, timer.CalendarConditions, registry);
            string calendarName = GetCalendarName(timer.CalendarId, registry);

            result.Add(new
            {
                id = timer.Id.ToString(),
                name = timer.Name,
                description = timer.Description ?? "",
                type = timer.Type.ToString().ToLowerInvariant(),
                status = timer.Status.ToString().ToLowerInvariant(),
                triggerTime = timer.TriggerTime,
                triggerTimeFormatted = timer.TriggerTime.ToString("yyyy-MM-dd HH:mm:ss"),
                calendarId = timer.CalendarId,
                calendarName = calendarName,
                calendarDescription = calendarDesc,
                calendarConditions = timer.CalendarConditions,
                timesTriggered = timer.TimesTriggered,
                createdAt = timer.CreatedAt,
                lastTriggeredAt = timer.LastTriggeredAt
            });
        }

        return result;
    }

    private static string GetCalendarDescription(string calendarId, Dictionary<string, int> conditions, Dictionary<string, CalendarBase> registry)
    {
        if (calendarId == "interval")
        {
            int days = conditions.TryGetValue("days", out int d) ? d : 0;
            int hours = conditions.TryGetValue("hours", out int h) ? h : 0;
            int minutes = conditions.TryGetValue("minutes", out int m) ? m : 0;
            int seconds = conditions.TryGetValue("seconds", out int s) ? s : 0;

            var loc = CalendarTool.GetLocalization();
            return loc.LocalizeIntervalDescription(days, hours, minutes, seconds);
        }

        if (registry.TryGetValue(calendarId, out CalendarBase? calendar))
        {
            try
            {
                return calendar.Localize(conditions);
            }
            catch
            {
            }
        }

        return string.Join(", ", conditions.Select(kv => $"{kv.Key}={kv.Value}"));
    }

    private static string GetCalendarName(string calendarId, Dictionary<string, CalendarBase> registry)
    {
        if (calendarId == "interval")
        {
            var loc = CalendarTool.GetLocalization();
            return loc.CalendarIntervalName;
        }

        if (registry.TryGetValue(calendarId, out CalendarBase? calendar))
        {
            return calendar.CalendarName;
        }

        return calendarId;
    }
}
