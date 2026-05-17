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

using SiliconLife.Common.Localization;

namespace SiliconLife.Common.Tools;

[SiliconManagerOnly]
[ToolScenario(ToolScenarioFlag.Project)]
public class ProjectWorkTool : ITool
{
    public string Name => "project_work";

    public string Description =>
        "Project work actions for the curator in ThinkOnProject scenario. " +
        "Actions: 'create-task' (create a project task), 'assign-task' (assign a being to a task), " +
        "'chat' (send message to project group chat), 'broadcast' (broadcast to project channel), " +
        "'complete' (mark project as completed), 'status' (get project status).";

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
                    ["description"] = "The action to perform",
                    ["enum"] = new[] { "create-task", "assign-task", "chat", "broadcast", "complete", "status" }
                },
                ["project_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Project ID (GUID, required for all actions)"
                },
                ["title"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Task title (for create-task)"
                },
                ["description"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Task description (for create-task)"
                },
                ["assignee_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Silicon being GUID to assign (for create-task, assign-task)"
                },
                ["priority"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Task priority, lower = higher priority (for create-task, default 100)"
                },
                ["task_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Task GUID (for assign-task)"
                },
                ["message"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Message content (for chat)"
                },
                ["content"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Broadcast content (for broadcast)"
                },
                ["summary"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Completion summary (for complete)"
                }
            },
            ["required"] = new[] { "action", "project_id" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        var projectManager = ServiceLocator.Instance.ProjectManager;
        if (projectManager == null)
        {
            return ToolResult.Failed("Project manager is not initialized");
        }

        if (!parameters.TryGetValue("action", out var actionObj))
        {
            return ToolResult.Failed("Missing required parameter: action");
        }

        string action = actionObj?.ToString()?.ToLowerInvariant() ?? "";
        if (string.IsNullOrEmpty(action))
        {
            return ToolResult.Failed("Missing required parameter: action");
        }

        if (!parameters.TryGetValue("project_id", out var pidObj) || !Guid.TryParse(pidObj?.ToString(), out Guid projectId))
        {
            return ToolResult.Failed("Missing or invalid parameter: project_id");
        }

        var project = projectManager.GetProject(projectId);
        if (project == null)
        {
            return ToolResult.Failed($"Project not found: {projectId}");
        }

        if (project.CreatedBy != callerId)
        {
            return ToolResult.Failed("Only the project creator (curator) can use project work actions");
        }

        try
        {
            return action switch
            {
                "create-task" => ExecuteCreateTask(projectManager, project, callerId, parameters),
                "assign-task" => ExecuteAssignTask(projectManager, project, parameters),
                "chat" => ExecuteChat(project, callerId, parameters),
                "broadcast" => ExecuteBroadcast(project, callerId, parameters),
                "complete" => ExecuteComplete(projectManager, project, parameters),
                "status" => ExecuteStatus(projectManager, project),
                _ => ToolResult.Failed($"Unknown action: {action}")
            };
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Project work tool execution failed: {ex.Message}");
        }
    }

    private static ToolResult ExecuteCreateTask(IProjectManager pm, ProjectSpace project, Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("title", out var titleObj) || string.IsNullOrWhiteSpace(titleObj?.ToString()))
        {
            return ToolResult.Failed("Missing required parameter: title (for create-task action)");
        }

        string title = titleObj!.ToString()!;
        string description = parameters.TryGetValue("description", out var descObj) ? descObj?.ToString() ?? "" : "";
        int priority = 100;
        if (parameters.TryGetValue("priority", out var priorityObj) && int.TryParse(priorityObj?.ToString(), out int p))
        {
            priority = p;
        }

        Guid executorGuid = callerId;
        if (parameters.TryGetValue("assignee_id", out var assigneeObj) && Guid.TryParse(assigneeObj?.ToString(), out Guid assigneeId))
        {
            executorGuid = assigneeId;
        }

        var taskSystem = pm.GetTaskSystem(project.Id);
        if (taskSystem == null)
        {
            return ToolResult.Failed("Project task system is not available");
        }

        var task = taskSystem.Create(title, description, callerId, executorGuid, null, priority);
        return ToolResult.Successful($"Project task created: {task.Title} (ID: {task.Id}, Priority: {task.Priority}, Executor: {executorGuid})");
    }

    private static ToolResult ExecuteAssignTask(IProjectManager pm, ProjectSpace project, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("task_id", out var tidObj) || !Guid.TryParse(tidObj?.ToString(), out Guid taskId))
        {
            return ToolResult.Failed("Missing or invalid parameter: task_id (for assign-task action)");
        }

        if (!parameters.TryGetValue("assignee_id", out var aidObj) || !Guid.TryParse(aidObj?.ToString(), out Guid assigneeId))
        {
            return ToolResult.Failed("Missing or invalid parameter: assignee_id (for assign-task action)");
        }

        var taskSystem = pm.GetTaskSystem(project.Id);
        if (taskSystem == null)
        {
            return ToolResult.Failed("Project task system is not available");
        }

        bool result = taskSystem.Assign(taskId, assigneeId);
        if (result)
        {
            return ToolResult.Successful($"Assigned being {assigneeId} to task {taskId}");
        }
        return ToolResult.Failed($"Failed to assign being to task {taskId} (task not found)");
    }

    private static ToolResult ExecuteChat(ProjectSpace project, Guid callerId, Dictionary<string, object> parameters)
    {
        if (!project.GroupChatSessionId.HasValue)
        {
            return ToolResult.Failed("Project does not have a group chat session");
        }

        if (!parameters.TryGetValue("message", out var msgObj) || string.IsNullOrWhiteSpace(msgObj?.ToString()))
        {
            return ToolResult.Failed("Missing required parameter: message (for chat action)");
        }

        string message = msgObj!.ToString()!;
        var chatSystem = ServiceLocator.Instance.ChatSystem;
        if (chatSystem == null)
        {
            return ToolResult.Failed("Chat system is not initialized");
        }

        chatSystem.AddMessage(callerId, project.GroupChatSessionId.Value, message);
        return ToolResult.Successful($"Message sent to project group chat");
    }

    private static ToolResult ExecuteBroadcast(ProjectSpace project, Guid callerId, Dictionary<string, object> parameters)
    {
        if (!project.BroadcastChannelId.HasValue)
        {
            return ToolResult.Failed("Project does not have a broadcast channel");
        }

        if (!parameters.TryGetValue("content", out var contentObj) || string.IsNullOrWhiteSpace(contentObj?.ToString()))
        {
            return ToolResult.Failed("Missing required parameter: content (for broadcast action)");
        }

        string content = contentObj!.ToString()!;
        var chatSystem = ServiceLocator.Instance.ChatSystem;
        if (chatSystem == null)
        {
            return ToolResult.Failed("Chat system is not initialized");
        }

        chatSystem.Broadcast(callerId, project.BroadcastChannelId.Value, content);
        return ToolResult.Successful($"Broadcast sent to project channel");
    }

    private static ToolResult ExecuteComplete(IProjectManager pm, ProjectSpace project, Dictionary<string, object> parameters)
    {
        var taskSystem = pm.GetTaskSystem(project.Id);
        if (taskSystem != null)
        {
            var tasks = taskSystem.GetAll();
            var incompleteTasks = tasks.Where(t => t.Status != Collective.TaskStatus.Completed && t.Status != Collective.TaskStatus.Cancelled).ToList();
            if (incompleteTasks.Count > 0)
            {
                return ToolResult.Failed($"Cannot complete project: {incompleteTasks.Count} task(s) are not yet completed or cancelled. Complete or cancel all tasks first.");
            }
        }

        string summary = parameters.TryGetValue("summary", out var summaryObj) ? summaryObj?.ToString() ?? "" : "";
        bool result = pm.ArchiveProject(project.Id);
        if (result)
        {
            string message = string.IsNullOrEmpty(summary)
                ? $"Project '{project.Name}' marked as completed and archived."
                : $"Project '{project.Name}' marked as completed and archived. Summary: {summary}";
            return ToolResult.Successful(message);
        }
        return ToolResult.Failed($"Failed to complete project {project.Id}");
    }

    private static ToolResult ExecuteStatus(IProjectManager pm, ProjectSpace project)
    {
        var beingManager = ServiceLocator.Instance.BeingManager;
        var lines = new List<string>
        {
            $"Project: {project.Name}",
            $"ID: {project.Id}",
            $"Status: {project.Status}",
            $"Description: {project.Description}",
            $"Created: {project.CreatedAt:yyyy-MM-dd HH:mm:ss}",
            $"Updated: {project.UpdatedAt:yyyy-MM-dd HH:mm:ss}"
        };

        lines.Add($"Team members ({project.AssignedBeings.Count}):");
        foreach (var beingId in project.AssignedBeings)
        {
            var being = beingManager?.GetBeing(beingId);
            lines.Add($"  - {being?.Name ?? beingId.ToString()}");
        }

        var taskSystem = pm.GetTaskSystem(project.Id);
        if (taskSystem != null)
        {
            var tasks = taskSystem.GetAll();
            var stats = taskSystem.GetStatistics();
            lines.Add($"Tasks: {stats.Total} total, {stats.Pending} pending, {stats.Running} running, {stats.Completed} completed");
            if (tasks.Count > 0)
            {
                lines.Add("Task list:");
                foreach (var task in tasks.OrderBy(t => t.Priority).Take(20))
                {
                    var executor = beingManager?.GetBeing(task.ExecutorGuid);
                    lines.Add($"  [{task.Status}] #{task.Priority} {task.Title} (executor: {executor?.Name ?? "unassigned"})");
                }
            }
        }
        else
        {
            lines.Add("Tasks: No task system");
        }

        return ToolResult.Successful(string.Join("\n", lines));
    }
}
