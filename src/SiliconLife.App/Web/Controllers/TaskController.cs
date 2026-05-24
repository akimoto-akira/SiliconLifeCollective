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
using SiliconLife.App.Web;

namespace SiliconLife.App.Web.Controllers;

[WebCode]
public class TaskController : Controller
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<TaskController>();
    private readonly SiliconBeingManager _beingManager;
    private readonly SkinManager _skinManager;

    public TaskController()
    {
        var locator = ServiceLocator.Instance;
        _beingManager = locator.BeingManager!;
        _skinManager = locator.GetService<SkinManager>()!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/tasks";

        if (path == "/tasks" || path == "/tasks/index")
            Index();
        else if (path == "/api/tasks/list")
            GetList();
        else if (path.StartsWith("/task-cycles/"))
            ShowExecutionHistory();
        else if (path == "/api/task-cycles/list")
            GetExecutionList();
        else if (path.StartsWith("/task-cycle/"))
            ShowExecutionDetail();
        else if (path == "/api/task-cycle/messages")
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
        var view = new Views.TaskView();
        var beingIdStr = Request.QueryString["beingId"];
        Guid? beingId = null;
        if (!string.IsNullOrEmpty(beingIdStr) && Guid.TryParse(beingIdStr, out var parsedId))
            beingId = parsedId;
        var vm = new Models.TaskViewModel { Skin = skin, ActiveMenu = "tasks", CurrentBeingId = beingId };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetList()
    {
        var beingIdStr = Request.QueryString["beingId"];
        List<object> allTasks = new();

        if (!string.IsNullOrEmpty(beingIdStr) && Guid.TryParse(beingIdStr, out var beingId))
        {
            var being = _beingManager.GetBeing(beingId);
            if (being?.TaskSystem != null)
            {
                allTasks.AddRange(GetTasksFromSystem(being.TaskSystem, being.Name));
            }
        }
        else
        {
            var beings = _beingManager.GetAllBeings();
            foreach (var being in beings)
            {
                if (being.TaskSystem != null)
                {
                    allTasks.AddRange(GetTasksFromSystem(being.TaskSystem, being.Name));
                }
            }
        }

        RenderJson(allTasks);
    }

    private static List<object> GetTasksFromSystem(TaskSystem taskSystem, string beingName)
    {
        var tasks = taskSystem.GetAll();
        var result = new List<object>();

        foreach (var task in tasks)
        {
            result.Add(new
            {
                id = task.Id.ToString(),
                name = task.Title,
                description = task.Description ?? "",
                status = task.Status.ToString().ToLowerInvariant(),
                priority = task.Priority,
                createdAt = task.CreatedAt,
                createdAtFormatted = task.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                startedAt = task.StartedAt,
                completedAt = task.CompletedAt,
                assignedTo = beingName,
                errorMessage = task.ErrorMessage ?? ""
            });
        }

        return result;
    }

    private void ShowExecutionHistory()
    {
        if (!Parameters.TryGetValue("taskId", out var taskIdStr) || !Guid.TryParse(taskIdStr, out var taskId))
        {
            Response.StatusCode = 400;
            Response.Close();
            return;
        }

        var task = FindTask(taskId);
        if (task == null)
        {
            Response.StatusCode = 404;
            Response.Close();
            return;
        }

        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.TaskExecutionHistoryView();
        var vm = new Models.TaskExecutionHistoryViewModel
        {
            Skin = skin,
            ActiveMenu = "tasks",
            TaskId = taskId,
            TaskName = task.Title
        };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetExecutionList()
    {
        var taskIdStr = Request.QueryString["taskId"];
        if (string.IsNullOrEmpty(taskIdStr) || !Guid.TryParse(taskIdStr, out var taskId))
        {
            _logger.Warn(null, "GetExecutionList: Invalid taskId parameter");
            RenderJson(new List<object>());
            return;
        }

        var task = FindTask(taskId);
        if (task == null)
        {
            _logger.Warn(null, "GetExecutionList: Task not found: {0}", taskId);
            RenderJson(new List<object>());
            return;
        }

        var result = task.ChatHistory.Select((c, i) => new
        {
            cycleIndex = i,
            startedAt = c.StartedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            endedAt = c.EndedAt?.ToString("yyyy-MM-dd HH:mm:ss"),
            state = c.EndStatus ?? c.StartStatus,
            roundCount = c.Messages.Count(m => m.Role == MessageRole.Assistant),
            messageCount = c.Messages.Count
        }).ToList();

        _logger.Info(null, "GetExecutionList: Returning {0} cycles for task {1}", result.Count, taskId);
        RenderJson(result);
    }

    private void ShowExecutionDetail()
    {
        if (!Parameters.TryGetValue("cycleIndex", out var cycleIndexStr) || !int.TryParse(cycleIndexStr, out var cycleIndex))
        {
            Response.StatusCode = 400;
            Response.Close();
            return;
        }

        var taskIdStr = Request.QueryString["taskId"];
        if (string.IsNullOrEmpty(taskIdStr) || !Guid.TryParse(taskIdStr, out var taskId))
        {
            Response.StatusCode = 400;
            Response.Close();
            return;
        }

        var task = FindTask(taskId);
        if (task == null || cycleIndex < 0 || cycleIndex >= task.ChatHistory.Count)
        {
            Response.StatusCode = 404;
            Response.Close();
            return;
        }

        var cycle = task.ChatHistory[cycleIndex];
        var being = FindTaskOwner(taskId);
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.TaskExecutionDetailView();
        var vm = new Models.TaskExecutionDetailViewModel
        {
            Skin = skin,
            ActiveMenu = "tasks",
            TaskId = taskId,
            CycleIndex = cycleIndex,
            TaskName = task.Title,
            StartedAt = cycle.StartedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            EndedAt = cycle.EndedAt?.ToString("yyyy-MM-dd HH:mm:ss"),
            State = cycle.EndStatus ?? cycle.StartStatus,
            ToolDisplayNames = GetToolDisplayNames(being)
        };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetExecutionMessages()
    {
        var cycleIndexStr = Request.QueryString["cycleIndex"];
        if (string.IsNullOrEmpty(cycleIndexStr) || !int.TryParse(cycleIndexStr, out var cycleIndex))
        {
            _logger.Warn(null, "GetExecutionMessages: Invalid cycleIndex parameter");
            RenderJson(new { messages = new List<object>() });
            return;
        }

        var taskIdStr = Request.QueryString["taskId"];
        if (string.IsNullOrEmpty(taskIdStr) || !Guid.TryParse(taskIdStr, out var taskId))
        {
            _logger.Warn(null, "GetExecutionMessages: Invalid taskId parameter");
            RenderJson(new { messages = new List<object>() });
            return;
        }

        var task = FindTask(taskId);
        if (task == null)
        {
            _logger.Warn(null, "GetExecutionMessages: Task not found: {0}", taskId);
            RenderJson(new { messages = new List<object>() });
            return;
        }

        if (cycleIndex < 0 || cycleIndex >= task.ChatHistory.Count)
        {
            _logger.Warn(null, "GetExecutionMessages: Cycle index {0} out of range for task {1}", cycleIndex, taskId);
            RenderJson(new { messages = new List<object>() });
            return;
        }

        var messages = task.ChatHistory[cycleIndex].Messages;
        _logger.Info(null, "GetExecutionMessages: Loaded cycle {0} with {1} messages", cycleIndex, messages.Count);

        var being = FindTaskOwner(taskId);
        var userNickname = Config.Instance?.Data?.UserNickname ?? "User";
        var userId = Config.Instance?.Data?.UserGuid ?? Guid.Empty;

        var toolCallMap = new Dictionary<string, int>();
        var result = new List<dynamic>();

        foreach (var m in messages)
        {
            var senderBeing = _beingManager.GetBeing(m.SenderId);
            var senderName = senderBeing?.Name ?? (m.SenderId == userId ? userNickname : m.SenderId.ToString());

            if (m.Role == MessageRole.Tool && !string.IsNullOrEmpty(m.ToolCallId))
            {
                if (toolCallMap.TryGetValue(m.ToolCallId, out var toolCallIndex))
                {
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
                    toolCallMap[m.Id.ToString()] = result.Count;
                }

                result.Add(toolCallData);
            }
            else
            {
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

    private Collective.TaskItem? FindTask(Guid taskId)
    {
        return TaskCenter.Instance.GetTask(taskId);
    }

    private SiliconBeingBase? FindTaskOwner(Guid taskId)
    {
        var beings = _beingManager.GetAllBeings();
        foreach (var being in beings)
        {
            var task = being.TaskSystem?.Get(taskId);
            if (task != null)
                return being;
        }
        return null;
    }

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
