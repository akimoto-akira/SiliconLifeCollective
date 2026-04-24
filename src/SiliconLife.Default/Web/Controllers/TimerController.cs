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
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<TimerController>();
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
        else if (path.StartsWith("/timer-executions/"))
            ShowExecutionHistory();
        else if (path == "/api/timer-executions/list")
            GetExecutionList();
        else if (path.StartsWith("/timer-execution/"))
            ShowExecutionDetail();
        else if (path == "/api/timer-execution/messages")
            GetExecutionMessages();
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

    private void ShowExecutionHistory()
    {
        // Get timerId from route parameters
        if (!Parameters.TryGetValue("timerId", out var timerIdStr) || !Guid.TryParse(timerIdStr, out var timerId))
        {
            Response.StatusCode = 400;
            Response.Close();
            return;
        }

        TimerItem? timer = FindTimer(timerId);

        if (timer == null)
        {
            Response.StatusCode = 404;
            Response.Close();
            return;
        }

        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.TimerExecutionHistoryView();
        var vm = new Models.TimerExecutionHistoryViewModel
        {
            Skin = skin,
            ActiveMenu = "timers",
            TimerId = timerId,
            TimerName = timer.Name
        };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetExecutionList()
    {
        var timerIdStr = Request.QueryString["timerId"];
        if (string.IsNullOrEmpty(timerIdStr) || !Guid.TryParse(timerIdStr, out var timerId))
        {
            _logger.Warn(null, "GetExecutionList: Invalid timerId parameter");
            RenderJson(new List<object>());
            return;
        }

        // Find timer
        TimerItem? timer = FindTimer(timerId);
        if (timer == null)
        {
            _logger.Warn(null, "GetExecutionList: Timer not found: {0}", timerId);
            RenderJson(new List<object>());
            return;
        }

        // Get execution history from TimerItem
        var executions = timer.GetExecutionHistory();
        var result = executions.Select(e => new
        {
            executionId = e.ExecutionId.ToString(),
            triggeredAt = e.TriggeredAt.ToString("yyyy-MM-dd HH:mm:ss"),
            completedAt = e.CompletedAt?.ToString("yyyy-MM-dd HH:mm:ss"),
            state = e.State.ToString(),
            stepCount = e.CurrentStep,
            messageCount = e.Messages.Count
        }).ToList();

        _logger.Info(null, "GetExecutionList: Returning {0} executions for timer {1}", result.Count, timerId);
        RenderJson(result);
    }

    private void ShowExecutionDetail()
    {
        // Get executionId from route parameters
        if (!Parameters.TryGetValue("executionId", out var executionIdStr) || !Guid.TryParse(executionIdStr, out var executionId))
        {
            Response.StatusCode = 400;
            Response.Close();
            return;
        }

        // Get timerId from query string
        var timerIdStr = Request.QueryString["timerId"];
        if (string.IsNullOrEmpty(timerIdStr) || !Guid.TryParse(timerIdStr, out var timerId))
        {
            Response.StatusCode = 400;
            Response.Close();
            return;
        }

        // Find timer and load execution
        TimerItem? timer = FindTimer(timerId);

        if (timer == null || string.IsNullOrEmpty(timer.CurrentExecutionFile))
        {
            Response.StatusCode = 404;
            Response.Close();
            return;
        }

        var execution = timer.GetExecution(executionId);

        if (execution == null)
        {
            Response.StatusCode = 404;
            Response.Close();
            return;
        }

        var being = FindTimerOwner(timerId);
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.TimerExecutionDetailView();
        var vm = new Models.TimerExecutionDetailViewModel
        {
            Skin = skin,
            ActiveMenu = "timers",
            TimerId = timerId,
            ExecutionId = executionId.ToString(),
            TimerName = execution.TimerName,
            TriggeredAt = execution.TriggeredAt.ToString("yyyy-MM-dd HH:mm:ss"),
            CompletedAt = execution.CompletedAt?.ToString("yyyy-MM-dd HH:mm:ss"),
            State = execution.State.ToString(),
            ToolDisplayNames = GetToolDisplayNames(being)
        };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetExecutionMessages()
    {
        var executionIdStr = Request.QueryString["executionId"];
        if (string.IsNullOrEmpty(executionIdStr) || !Guid.TryParse(executionIdStr, out var executionId))
        {
            _logger.Warn(null, "GetExecutionMessages: Invalid executionId parameter");
            RenderJson(new { messages = new List<object>() });
            return;
        }

        var timerIdStr = Request.QueryString["timerId"];
        if (string.IsNullOrEmpty(timerIdStr) || !Guid.TryParse(timerIdStr, out var timerId))
        {
            _logger.Warn(null, "GetExecutionMessages: Invalid timerId parameter");
            RenderJson(new { messages = new List<object>() });
            return;
        }

        // Find timer
        TimerItem? timer = FindTimer(timerId);
        if (timer == null)
        {
            _logger.Warn(null, "GetExecutionMessages: Timer not found: {0}", timerId);
            RenderJson(new { messages = new List<object>() });
            return;
        }

        // Get execution from TimerItem
        var execution = timer.GetExecution(executionId);
        if (execution == null)
        {
            _logger.Warn(null, "GetExecutionMessages: Execution not found: {0}", executionId);
            RenderJson(new { messages = new List<object>() });
            return;
        }

        _logger.Info(null, "GetExecutionMessages: Loaded execution with {0} messages", execution.Messages.Count);

        // Get being for sender name resolution
        var being = FindTimerOwner(timerId);
        var userNickname = Config.Instance?.Data?.UserNickname ?? "User";
        var userId = Config.Instance?.Data?.UserGuid ?? Guid.Empty;

        // Pair tool calls with their results
        // One Assistant message may contain multiple tool calls; each tool result
        // is a separate Tool message matched by ToolCallId.
        var toolCallMap = new Dictionary<string, int>(); // toolCallId -> index in result list
        var result = new List<dynamic>();

        foreach (var m in execution.Messages)
        {
            var senderBeing = _beingManager.GetBeing(m.SenderId);
            var senderName = senderBeing?.Name ?? (m.SenderId == userId ? userNickname : m.SenderId.ToString());

            if (m.Role == MessageRole.Tool && !string.IsNullOrEmpty(m.ToolCallId))
            {
                // This is a tool result, find matching tool call
                if (toolCallMap.TryGetValue(m.ToolCallId, out var toolCallIndex))
                {
                    // Append result to the tool call's results list
                    var original = result[toolCallIndex];
                    var existingResults = (List<dynamic>)original.toolResults;
                    existingResults.Add(new
                    {
                        toolCallId = m.ToolCallId,
                        content = m.Content,
                        timestamp = m.Timestamp.ToString("yyyy-MM-dd HH:mm:ss")
                    });
                }
                else
                {
                    // Tool result without matching call, show separately
                    result.Add(new
                    {
                        id = m.Id.ToString(),
                        senderId = m.SenderId.ToString(),
                        content = m.Content,
                        thinking = (string?)null,
                        role = "Tool",
                        senderName = senderName,
                        timestamp = m.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                        toolCallsJson = (string?)null,
                        toolCallId = m.ToolCallId,
                        toolResults = new List<dynamic>()
                    });
                }
            }
            else if (!string.IsNullOrEmpty(m.ToolCallsJson))
            {
                // This is a tool call, store it for later merging
                var toolCallData = new
                {
                    id = m.Id.ToString(),
                    senderId = m.SenderId.ToString(),
                    content = m.Content,
                    thinking = m.Thinking,
                    role = m.Role.ToString(),
                    senderName = senderName,
                    timestamp = m.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    toolCallsJson = m.ToolCallsJson,
                    toolCallId = (string?)null,
                    toolResults = new List<dynamic>()
                } as dynamic;

                // Extract ALL tool call IDs from toolCallsJson for matching
                try
                {
                    var toolCalls = System.Text.Json.JsonSerializer.Deserialize<List<Dictionary<string, object>>>(m.ToolCallsJson);
                    if (toolCalls != null)
                    {
                        foreach (var tc in toolCalls)
                        {
                            if (tc.ContainsKey("Id") && tc["Id"] != null)
                            {
                                toolCallMap[tc["Id"].ToString()!] = result.Count;
                            }
                        }
                    }
                }
                catch
                {
                    // If parsing fails, use message ID as fallback
                    toolCallMap[m.Id.ToString()] = result.Count;
                }

                result.Add(toolCallData);
            }
            else
            {
                // Regular message (User or Assistant without tool calls)
                result.Add(new
                {
                    id = m.Id.ToString(),
                    senderId = m.SenderId.ToString(),
                    content = m.Content,
                    thinking = m.Thinking,
                    role = m.Role.ToString(),
                    senderName = senderName,
                    timestamp = m.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    toolCallsJson = (string?)null,
                    toolCallId = (string?)null,
                    toolResults = new List<dynamic>()
                });
            }
        }

        RenderJson(new { messages = result });
    }

    /// <summary>
    /// Find a timer by ID across all beings.
    /// </summary>
    private TimerItem? FindTimer(Guid timerId)
    {
        var beings = _beingManager.GetAllBeings();
        foreach (var being in beings)
        {
            var timer = being.TimerSystem?.Get(timerId);
            if (timer != null)
                return timer;
        }
        return null;
    }

    /// <summary>
    /// Find the being that owns a timer by timer ID.
    /// </summary>
    private SiliconBeingBase? FindTimerOwner(Guid timerId)
    {
        var beings = _beingManager.GetAllBeings();
        foreach (var being in beings)
        {
            var timer = being.TimerSystem?.Get(timerId);
            if (timer != null)
                return being;
        }
        return null;
    }

    /// <summary>
    /// Build tool display names dictionary with localization from a being's ToolManager.
    /// </summary>
    private static Dictionary<string, string> GetToolDisplayNames(SiliconBeingBase? being)
    {
        var result = new Dictionary<string, string>();
        if (being?.ToolManager == null)
            return result;

        var language = Config.Instance?.Data?.Language ?? Language.ZhCN;
        foreach (var toolName in being.ToolManager.GetToolNames())
        {
            if (result.ContainsKey(toolName)) continue;
            var tool = being.ToolManager.GetTool(toolName);
            if (tool != null)
                result[toolName] = tool.GetDisplayName(language);
        }
        return result;
    }
}
