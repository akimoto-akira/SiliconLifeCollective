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
/// Tool for managing silicon being timers and alarms.
/// </summary>
public class TimerTool : ITool
{
    /// <inheritdoc/>
    public string Name => "timer";

    /// <inheritdoc/>
    public string Description =>
        "Manage timers and alarms for the silicon being. " +
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
                ["trigger_time"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Trigger time (format: yyyy-MM-dd HH:mm:ss or HH:mm:ss for today, used with create_once)"
                },
                ["interval_seconds"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Interval in seconds (used with create_recurring)"
                },
                ["max_triggers"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum number of triggers (optional, used with create_recurring)"
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
                },
                ["description"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Timer description (optional, used with create_once, create_recurring)"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        var being = GetSiliconBeing(callerId);
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
        var manager = ServiceLocator.Instance.BeingManager;
        if (manager == null)
            return null;

        return manager.GetBeing(callerId);
    }

    private DateTime ParseTime(string timeStr)
    {
        if (DateTime.TryParse(timeStr, out DateTime result))
        {
            return result;
        }

        if (TimeSpan.TryParse(timeStr, out TimeSpan ts))
        {
            return DateTime.Today.Add(ts);
        }

        throw new ArgumentException($"Invalid time format: {timeStr}");
    }

    private ToolResult ExecuteCreateOnce(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("name", out object? nameObj) || string.IsNullOrWhiteSpace(nameObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'name' parameter");
        }

        if (!parameters.TryGetValue("trigger_time", out object? timeObj) || string.IsNullOrWhiteSpace(timeObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'trigger_time' parameter");
        }

        string name = nameObj!.ToString()!;
        DateTime triggerTime = ParseTime(timeObj!.ToString()!);

        var timer = being.TimerSystem!.CreateOneShot(name, triggerTime);

        return ToolResult.Successful($"One-time timer created (ID: {timer.Id}, Trigger: {timer.TriggerTime:yyyy-MM-dd HH:mm:ss})");
    }

    private ToolResult ExecuteCreateRecurring(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("name", out object? nameObj) || string.IsNullOrWhiteSpace(nameObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'name' parameter");
        }

        if (!parameters.TryGetValue("interval_seconds", out object? intervalObj) || !int.TryParse(intervalObj?.ToString(), out int intervalSeconds))
        {
            return ToolResult.Failed("Missing or invalid 'interval_seconds' parameter");
        }

        string name = nameObj!.ToString()!;
        TimeSpan interval = TimeSpan.FromSeconds(intervalSeconds);

        int? maxTriggers = null;
        if (parameters.TryGetValue("max_triggers", out object? maxObj) && int.TryParse(maxObj?.ToString(), out int max))
        {
            maxTriggers = max;
        }

        var timer = being.TimerSystem!.CreateRecurring(name, interval, null, maxTriggers);

        string info = $"Recurring timer created (ID: {timer.Id}, Interval: {interval}, ";
        info += timer.MaxTriggers.HasValue ? $"Max: {timer.MaxTriggers})" : "Infinite)";

        return ToolResult.Successful(info);
    }

    private ToolResult ExecuteList(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        TimerStatus? status = null;
        if (parameters.TryGetValue("status", out object? statusObj) && !string.IsNullOrWhiteSpace(statusObj?.ToString()))
        {
            status = statusObj!.ToString()!.ToLowerInvariant() switch
            {
                "active" => TimerStatus.Active,
                "paused" => TimerStatus.Paused,
                "triggered" => TimerStatus.Triggered,
                "cancelled" => TimerStatus.Cancelled,
                _ => null
            };
        }

        var timers = being.TimerSystem!.GetAll(status);

        if (timers.Count == 0)
        {
            return ToolResult.Successful("No timers found.");
        }

        var lines = new List<string> { $"Found {timers.Count} timers:" };
        foreach (var timer in timers.Take(20))
        {
            var timeUntil = timer.GetTimeUntilNextTrigger();
            string triggerInfo = timeUntil.HasValue ? $"Next: {timeUntil.Value:hh\\:mm\\:ss}" : "N/A";
            lines.Add($"- [{timer.Status}] {timer.Name} ({timer.Type}, {triggerInfo}, ID: {timer.Id})");
        }

        return ToolResult.Successful(string.Join("\n", lines));
    }

    private ToolResult ExecuteGet(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("timer_id", out object? idObj) || !Guid.TryParse(idObj?.ToString(), out Guid timerId))
        {
            return ToolResult.Failed("Missing or invalid 'timer_id' parameter");
        }

        var timer = being.TimerSystem!.Get(timerId);
        if (timer == null)
        {
            return ToolResult.Failed($"Timer not found: {timerId}");
        }

        var lines = new List<string>
        {
            $"Timer: {timer.Name}",
            $"ID: {timer.Id}",
            $"Type: {timer.Type}",
            $"Status: {timer.Status}",
            $"Trigger Time: {timer.TriggerTime:yyyy-MM-dd HH:mm:ss}",
            $"Times Triggered: {timer.TimesTriggered}",
            $"Created: {timer.CreatedAt:yyyy-MM-dd HH:mm:ss}"
        };

        if (timer.Interval.HasValue)
            lines.Add($"Interval: {timer.Interval.Value.TotalSeconds} seconds");
        if (timer.MaxTriggers.HasValue)
            lines.Add($"Max Triggers: {timer.MaxTriggers}");
        if (timer.LastTriggeredAt.HasValue)
            lines.Add($"Last Triggered: {timer.LastTriggeredAt:yyyy-MM-dd HH:mm:ss}");
        if (timer.Description != null)
            lines.Add($"Description: {timer.Description}");

        var timeUntil = timer.GetTimeUntilNextTrigger();
        if (timeUntil.HasValue && timer.Status == TimerStatus.Active)
            lines.Add($"Time Until Next: {timeUntil.Value:hh\\:mm\\:ss}");

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
        var stats = being.TimerSystem!.GetStatistics();

        var lines = new List<string>
        {
            "Timer Statistics:",
            $"- Total: {stats.Total}",
            $"- Active: {stats.Active}",
            $"- Paused: {stats.Paused}",
            $"- Triggered: {stats.Triggered}",
            $"- Cancelled: {stats.Cancelled}",
            $"- Next Trigger: {(stats.NextTrigger?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A")}"
        };

        return ToolResult.Successful(string.Join("\n", lines));
    }

    private ToolResult ExecuteTick(SiliconBeingBase being)
    {
        var triggered = being.TimerSystem!.Tick();

        if (triggered.Count == 0)
        {
            return ToolResult.Successful("No timers triggered.");
        }

        var lines = new List<string> { $"{triggered.Count} timer(s) triggered:" };
        foreach (var timer in triggered)
        {
            lines.Add($"- {timer.Name} (ID: {timer.Id}, Times: {timer.TimesTriggered})");
        }

        return ToolResult.Successful(string.Join("\n", lines));
    }
}
