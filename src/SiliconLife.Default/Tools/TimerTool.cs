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

using System.Text.Json;
using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Tool for managing silicon being timers and alarms with calendar-based scheduling.
/// </summary>
public class TimerTool : ITool
{
    /// <inheritdoc/>
    public string Name => "timer";

    /// <inheritdoc/>
    public string Description =>
        "Manage timers and alarms for the silicon being using calendar-based scheduling. " +
        "Actions: 'create_once' (one-time alarm), 'create_recurring' (recurring timer), " +
        "'list' (list timers), 'get' (get timer details), 'pause' (pause timer), " +
        "'resume' (resume timer), 'cancel' (cancel timer), 'delete' (delete timer), " +
        "'stats' (get timer statistics), 'tick' (check for triggered timers).";

    /// <inheritdoc/>
    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

    public Dictionary<string, object> GetParameterSchema()
    {
        string[] calendarIds = GetAvailableCalendarIds();

        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The action to perform: create_once, create_recurring, list, get, pause, resume, cancel, delete, stats, tick",
                    ["enum"] = new[] { "create_once", "create_recurring", "list", "get", "pause", "resume", "cancel", "delete", "stats", "tick" }
                },
                ["name"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Timer name (used with create_once, create_recurring)"
                },
                ["calendar_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Calendar system identifier. Use 'gregorian' for standard dates, 'chinese_lunar' for Chinese lunar calendar, 'interval' for fixed time intervals, etc.",
                    ["enum"] = calendarIds
                },
                ["calendar_conditions"] = new Dictionary<string, object>
                {
                    ["type"] = "object",
                    ["description"] = "Calendar component conditions (key=componentId, value=integer). " +
                        "For gregorian: {\"hour\":8}=daily at 8am, {\"day\":1}=1st of month, {\"month\":1,\"day\":1}=Jan 1st, {\"weekday\":1,\"hour\":9}=every Mon 9am. " +
                        "For chinese_lunar: {\"month\":1,\"day\":1}=Spring Festival, {\"day\":1,\"hour\":8}=every 1st of lunar month at 8am. " +
                        "For interval: {\"days\":2}=every 2 days, {\"hours\":1}=hourly, {\"minutes\":30}=every 30min.",
                    ["additionalProperties"] = new Dictionary<string, object>
                    {
                        ["type"] = "integer"
                    }
                },
                ["description"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Timer description (optional, used with create_once, create_recurring)"
                },
                ["timer_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Timer ID (GUID, used with get, pause, resume, cancel, delete)"
                },
                ["status"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Filter by status: active, paused, triggered, cancelled, all (used with action=list)",
                    ["enum"] = new[] { "active", "paused", "triggered", "cancelled", "all" }
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        SiliconBeingBase? being = GetSiliconBeing(callerId);
        if (being?.TimerSystem == null)
        {
            return ToolResult.Failed("Timer system not available");
        }

        if (!parameters.TryGetValue("action", out object? actionObj))
        {
            return ToolResult.Failed("Missing 'action' parameter");
        }

        string action = actionObj?.ToString() ?? "";

        try
        {
            return action.ToLowerInvariant() switch
            {
                "create_once" => ExecuteCreateOnce(being, parameters),
                "create_recurring" => ExecuteCreateRecurring(being, parameters),
                "list" => ExecuteList(being, parameters),
                "get" => ExecuteGet(being, parameters),
                "pause" => ExecutePause(being, parameters),
                "resume" => ExecuteResume(being, parameters),
                "cancel" => ExecuteCancel(being, parameters),
                "delete" => ExecuteDelete(being, parameters),
                "stats" => ExecuteStats(being, parameters),
                "tick" => ExecuteTick(being),
                _ => ToolResult.Failed($"Unknown action: {action}")
            };
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Timer operation failed: {ex.Message}");
        }
    }

    private SiliconBeingBase? GetSiliconBeing(Guid callerId)
    {
        SiliconBeingManager? manager = ServiceLocator.Instance.BeingManager;
        if (manager == null)
            return null;

        return manager.GetBeing(callerId);
    }

    private ToolResult ExecuteCreateOnce(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("name", out object? nameObj) || string.IsNullOrWhiteSpace(nameObj?.ToString()))
            return ToolResult.Failed("Missing 'name' parameter");

        if (!parameters.TryGetValue("calendar_id", out object? calObj) || string.IsNullOrWhiteSpace(calObj?.ToString()))
            return ToolResult.Failed("Missing 'calendar_id' parameter");

        Dictionary<string, int>? conditions = ParseCalendarConditions(parameters);
        if (conditions == null || conditions.Count == 0)
            return ToolResult.Failed("Missing or invalid 'calendar_conditions' parameter");

        string name = nameObj!.ToString()!;
        string calendarId = calObj!.ToString()!;

        TimerItem? timer = being.TimerSystem!.CreateOneShot(name, calendarId, conditions);
        if (timer == null)
            return ToolResult.Failed($"Cannot create timer: resolver returned null for calendar={calendarId}");

        string desc = GetHumanReadableDescription(calendarId, conditions);
        return ToolResult.Successful(
            $"One-shot timer created (ID: {timer.Id}, Calendar: {calendarId}, Condition: {desc}, " +
            $"Trigger: {timer.TriggerTime:yyyy-MM-dd HH:mm:ss})");
    }

    private ToolResult ExecuteCreateRecurring(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("name", out object? nameObj) || string.IsNullOrWhiteSpace(nameObj?.ToString()))
            return ToolResult.Failed("Missing 'name' parameter");

        if (!parameters.TryGetValue("calendar_id", out object? calObj) || string.IsNullOrWhiteSpace(calObj?.ToString()))
            return ToolResult.Failed("Missing 'calendar_id' parameter");

        Dictionary<string, int>? conditions = ParseCalendarConditions(parameters);
        if (conditions == null || conditions.Count == 0)
            return ToolResult.Failed("Missing or invalid 'calendar_conditions' parameter");

        string name = nameObj!.ToString()!;
        string calendarId = calObj!.ToString()!;

        TimerItem? timer = being.TimerSystem!.CreateRecurring(name, calendarId, conditions);
        if (timer == null)
            return ToolResult.Failed($"Cannot create timer: resolver returned null for calendar={calendarId}");

        string desc = GetHumanReadableDescription(calendarId, conditions);
        return ToolResult.Successful(
            $"Recurring timer created (ID: {timer.Id}, Calendar: {calendarId}, Condition: {desc}, " +
            $"Next trigger: {timer.TriggerTime:yyyy-MM-dd HH:mm:ss})");
    }

    private ToolResult ExecuteList(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        TimerStatus? status = ParseStatusFilter(parameters);

        List<TimerItem> timers = being.TimerSystem!.GetAll(status);

        if (timers.Count == 0)
        {
            return ToolResult.Successful("No timers found.");
        }

        List<string> lines = new List<string> { $"Found {timers.Count} timers:" };
        foreach (TimerItem timer in timers.Take(20))
        {
            TimeSpan? timeUntil = timer.GetTimeUntilNextTrigger();
            string triggerInfo = timeUntil.HasValue ? $"Next: {timeUntil.Value:hh\\:mm\\:ss}" : "N/A";
            string desc = GetHumanReadableDescription(timer.CalendarId, timer.CalendarConditions);
            lines.Add($"- [{timer.Status}] {timer.Name} ({timer.CalendarId}, {desc}, {triggerInfo}, ID: {timer.Id})");
        }

        return ToolResult.Successful(string.Join("\n", lines));
    }

    private ToolResult ExecuteGet(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("timer_id", out object? idObj) || !Guid.TryParse(idObj?.ToString(), out Guid timerId))
        {
            return ToolResult.Failed("Missing or invalid 'timer_id' parameter");
        }

        TimerItem? timer = being.TimerSystem!.Get(timerId);
        if (timer == null)
        {
            return ToolResult.Failed($"Timer not found: {timerId}");
        }

        string desc = GetHumanReadableDescription(timer.CalendarId, timer.CalendarConditions);
        List<string> lines = new List<string>
        {
            $"Timer: {timer.Name}",
            $"ID: {timer.Id}",
            $"Type: {timer.Type}",
            $"Status: {timer.Status}",
            $"Calendar: {timer.CalendarId}",
            $"Condition: {desc}",
            $"Trigger Time: {timer.TriggerTime:yyyy-MM-dd HH:mm:ss}",
            $"Times Triggered: {timer.TimesTriggered}",
            $"Created: {timer.CreatedAt:yyyy-MM-dd HH:mm:ss}"
        };

        if (timer.LastTriggeredAt.HasValue)
            lines.Add($"Last Triggered: {timer.LastTriggeredAt:yyyy-MM-dd HH:mm:ss}");
        if (timer.Description != null)
            lines.Add($"Description: {timer.Description}");

        TimeSpan? timeUntil = timer.GetTimeUntilNextTrigger();
        if (timeUntil.HasValue && timer.Status == TimerStatus.Active)
            lines.Add($"Time Until Next: {timeUntil.Value:dd\\:hh\\:mm\\:ss}");

        return ToolResult.Successful(string.Join("\n", lines));
    }

    private ToolResult ExecutePause(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("timer_id", out object? idObj) || !Guid.TryParse(idObj?.ToString(), out Guid timerId))
        {
            return ToolResult.Failed("Missing or invalid 'timer_id' parameter");
        }

        being.TimerSystem!.Pause(timerId);
        return ToolResult.Successful($"Timer {timerId} paused.");
    }

    private ToolResult ExecuteResume(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("timer_id", out object? idObj) || !Guid.TryParse(idObj?.ToString(), out Guid timerId))
        {
            return ToolResult.Failed("Missing or invalid 'timer_id' parameter");
        }

        being.TimerSystem!.Resume(timerId);
        return ToolResult.Successful($"Timer {timerId} resumed.");
    }

    private ToolResult ExecuteCancel(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("timer_id", out object? idObj) || !Guid.TryParse(idObj?.ToString(), out Guid timerId))
        {
            return ToolResult.Failed("Missing or invalid 'timer_id' parameter");
        }

        being.TimerSystem!.Cancel(timerId);
        return ToolResult.Successful($"Timer {timerId} cancelled.");
    }

    private ToolResult ExecuteDelete(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("timer_id", out object? idObj) || !Guid.TryParse(idObj?.ToString(), out Guid timerId))
        {
            return ToolResult.Failed("Missing or invalid 'timer_id' parameter");
        }

        bool deleted = being.TimerSystem!.Delete(timerId);
        if (deleted)
        {
            return ToolResult.Successful($"Timer {timerId} deleted.");
        }
        else
        {
            return ToolResult.Failed($"Cannot delete timer {timerId}");
        }
    }

    private ToolResult ExecuteStats(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        TimerStatistics stats = being.TimerSystem!.GetStatistics();

        List<string> lines = new List<string>
        {
            "Timer Statistics:",
            $"- Total: {stats.Total}",
            $"- Active: {stats.Active}",
            $"- Paused: {stats.Paused}",
            $"- Triggered: {stats.Triggered}",
            $"- Cancelled: {stats.Cancelled}",
            $"- Next Trigger: {(stats.NextTrigger?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A")}"
        };

        if (stats.ByCalendar.Count > 0)
        {
            lines.Add("- By Calendar:");
            foreach (KeyValuePair<string, int> kv in stats.ByCalendar)
            {
                lines.Add($"  - {kv.Key}: {kv.Value}");
            }
        }

        return ToolResult.Successful(string.Join("\n", lines));
    }

    private ToolResult ExecuteTick(SiliconBeingBase being)
    {
        List<TimerItem> triggered = being.TimerSystem!.Tick();

        if (triggered.Count == 0)
        {
            return ToolResult.Successful("No timers triggered.");
        }

        List<string> lines = new List<string> { $"{triggered.Count} timer(s) triggered:" };
        foreach (TimerItem timer in triggered)
        {
            lines.Add($"- {timer.Name} (ID: {timer.Id}, Times: {timer.TimesTriggered})");
        }

        return ToolResult.Successful(string.Join("\n", lines));
    }

    private static Dictionary<string, int>? ParseCalendarConditions(Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("calendar_conditions", out object? condObj) || condObj == null)
            return null;

        Dictionary<string, int> result = new Dictionary<string, int>();

        if (condObj is Dictionary<string, object> dict)
        {
            foreach (KeyValuePair<string, object> kv in dict)
            {
                if (int.TryParse(kv.Value?.ToString(), out int value))
                    result[kv.Key] = value;
            }
        }
        else if (condObj is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Object)
        {
            foreach (JsonProperty property in jsonElement.EnumerateObject())
            {
                if (property.Value.ValueKind == JsonValueKind.Number && property.Value.TryGetInt32(out int value))
                    result[property.Name] = value;
                else if (int.TryParse(property.Value.ToString(), out int parsedValue))
                    result[property.Name] = parsedValue;
            }
        }

        return result.Count > 0 ? result : null;
    }

    private static TimerStatus? ParseStatusFilter(Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("status", out object? statusObj) || string.IsNullOrWhiteSpace(statusObj?.ToString()))
            return null;

        return statusObj!.ToString()!.ToLowerInvariant() switch
        {
            "active" => TimerStatus.Active,
            "paused" => TimerStatus.Paused,
            "triggered" => TimerStatus.Triggered,
            "cancelled" => TimerStatus.Cancelled,
            "all" => null,
            _ => null
        };
    }

    private static string GetHumanReadableDescription(string calendarId, Dictionary<string, int> conditions)
    {
        if (calendarId == "interval")
        {
            List<string> parts = new List<string>();
            if (conditions.TryGetValue("days", out int d) && d > 0) parts.Add($"every {d} day(s)");
            if (conditions.TryGetValue("hours", out int h) && h > 0) parts.Add($"every {h} hour(s)");
            if (conditions.TryGetValue("minutes", out int m) && m > 0) parts.Add($"every {m} min(s)");
            if (conditions.TryGetValue("seconds", out int s) && s > 0) parts.Add($"every {s} sec(s)");
            return parts.Count > 0 ? string.Join(", ", parts) : "interval";
        }

        Dictionary<string, CalendarBase> registry = CalendarTool.BuildCalendarRegistry();
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

    private static string[] GetAvailableCalendarIds()
    {
        Dictionary<string, CalendarBase> registry = CalendarTool.BuildCalendarRegistry();
        List<string> ids = registry.Keys.ToList();
        ids.Add("interval");
        ids.Sort(StringComparer.OrdinalIgnoreCase);
        return ids.ToArray();
    }
}
