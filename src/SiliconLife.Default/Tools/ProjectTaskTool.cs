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
using TaskStatus = SiliconLife.Collective.TaskStatus;

namespace SiliconLife.Default;

/// <summary>
/// Tool for managing tasks within project spaces.
/// Supports creating, listing, updating, assigning, and tracking project tasks.
/// </summary>
public class ProjectTaskTool : ITool
{
    /// <inheritdoc/>
    public string Name => "project_task";

    /// <inheritdoc/>
    public string Description =>
        "Manage tasks within project spaces. " +
        "Actions: 'create' (create a project task), 'list' (list project tasks), 'get' (get task details), " +
        "'update' (update task title/description/priority), 'assign' (assign a being to a task), " +
        "'remove_assignee' (remove an assignee from a task), 'start' (start a task), " +
        "'complete' (mark task completed), 'fail' (mark task failed), 'cancel' (cancel a task), " +
        "'delete' (delete a task), 'stats' (get task statistics).";

    /// <inheritdoc/>
    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

    /// <inheritdoc/>
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
                    ["description"] = "The action to perform: create, list, get, update, assign, remove_assignee, start, complete, fail, cancel, delete, stats",
                    ["enum"] = new[] { "create", "list", "get", "update", "assign", "remove_assignee", "start", "complete", "fail", "cancel", "delete", "stats" }
                },
                ["project_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Project ID (GUID, required for all actions)"
                },
                ["task_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Task ID (GUID, used with get, update, assign, remove_assignee, start, complete, fail, cancel, delete)"
                },
                ["title"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Task title (used with action=create, update)"
                },
                ["description"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Task description (used with action=create, update)"
                },
                ["priority"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Task priority (lower number = higher priority, used with action=create, update)"
                },
                ["assignee_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Assignee being GUID (used with action=assign, remove_assignee)"
                },
                ["status"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Filter by status: pending, running, completed, failed, cancelled, all (used with action=list)",
                    ["enum"] = new[] { "pending", "running", "completed", "failed", "cancelled", "all" }
                },
                ["error"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Error message (used with action=fail)"
                }
            },
            ["required"] = new[] { "action", "project_id" }
        };
    }

    /// <inheritdoc/>
    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        var projectManager = ServiceLocator.Instance.GetService<IProjectManager>();
        if (projectManager == null)
        {
            return ToolResult.Failed("Project manager is not initialized");
        }

        if (!parameters.TryGetValue("action", out var actionObj))
        {
            return ToolResult.Failed("Missing required parameter: action");
        }

        if (!parameters.TryGetValue("project_id", out var projectIdObj) || !Guid.TryParse(projectIdObj?.ToString(), out Guid projectId))
        {
            return ToolResult.Failed("Missing or invalid required parameter: project_id");
        }

        var taskSystem = projectManager.GetTaskSystem(projectId);
        if (taskSystem == null)
        {
            return ToolResult.Failed($"Project task system not found for project: {projectId}");
        }

        string action = actionObj?.ToString()?.ToLowerInvariant() ?? "";
        if (string.IsNullOrEmpty(action))
        {
            return ToolResult.Failed("Missing required parameter: action");
        }

        try
        {
            return action switch
            {
                "create" => ExecuteCreate(taskSystem, callerId, parameters),
                "list" => ExecuteList(taskSystem, parameters),
                "get" => ExecuteGet(taskSystem, parameters),
                "update" => ExecuteUpdate(taskSystem, parameters),
                "assign" => ExecuteAssign(taskSystem, parameters),
                "remove_assignee" => ExecuteRemoveAssignee(taskSystem, parameters),
                "start" => ExecuteStart(taskSystem, parameters),
                "complete" => ExecuteComplete(taskSystem, parameters),
                "fail" => ExecuteFail(taskSystem, parameters),
                "cancel" => ExecuteCancel(taskSystem, parameters),
                "delete" => ExecuteDelete(taskSystem, parameters),
                "stats" => ExecuteStats(taskSystem),
                _ => ToolResult.Failed($"Unknown action: {action}")
            };
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Project task tool execution failed: {ex.Message}");
        }
    }

    private static ToolResult ExecuteCreate(ProjectTaskSystem taskSystem, Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("title", out var titleObj) || string.IsNullOrWhiteSpace(titleObj?.ToString()))
        {
            return ToolResult.Failed("Missing required parameter: title (for create action)");
        }

        string title = titleObj!.ToString()!;
        string description = parameters.TryGetValue("description", out var descObj) ? descObj?.ToString() ?? "" : "";
        int priority = 100;
        if (parameters.TryGetValue("priority", out var priorityObj) && int.TryParse(priorityObj?.ToString(), out int p))
        {
            priority = p;
        }

        var task = taskSystem.Create(title, description, callerId, priority);
        return ToolResult.Successful($"Project task created: {task.Title} (ID: {task.Id}, Priority: {task.Priority})");
    }

    private static ToolResult ExecuteList(ProjectTaskSystem taskSystem, Dictionary<string, object> parameters)
    {
        TaskStatus? status = null;
        if (parameters.TryGetValue("status", out var statusObj) && !string.IsNullOrWhiteSpace(statusObj?.ToString()))
        {
            string statusStr = statusObj!.ToString()!.ToLowerInvariant();
            if (statusStr != "all")
            {
                status = statusStr switch
                {
                    "pending" => TaskStatus.Pending,
                    "running" => TaskStatus.Running,
                    "completed" => TaskStatus.Completed,
                    "failed" => TaskStatus.Failed,
                    "cancelled" => TaskStatus.Cancelled,
                    _ => null
                };
            }
        }

        var tasks = taskSystem.GetAll(status);
        if (tasks.Count == 0)
        {
            return ToolResult.Successful("No project tasks found.");
        }

        var lines = new List<string> { $"Found {tasks.Count} project tasks:" };
        foreach (var task in tasks.OrderBy(t => t.Priority).Take(20))
        {
            lines.Add($"- [{task.Status}] #{task.Priority} {task.Title} (ID: {task.Id})");
        }

        return ToolResult.Successful(string.Join("\n", lines));
    }

    private static ToolResult ExecuteGet(ProjectTaskSystem taskSystem, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("task_id", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
        {
            return ToolResult.Failed("Missing or invalid 'task_id' parameter");
        }

        var task = taskSystem.Get(taskId);
        if (task == null)
        {
            return ToolResult.Failed($"Project task not found: {taskId}");
        }

        var lines = new List<string>
        {
            $"Task: {task.Title}",
            $"ID: {task.Id}",
            $"Status: {task.Status}",
            $"Priority: {task.Priority}",
            $"Created: {task.CreatedAt:yyyy-MM-dd HH:mm:ss}",
            $"Created By: {task.CreatedByGuid}",
            $"Description: {task.Description}"
        };

        if (task.AssigneeGuids.Count > 0)
            lines.Add($"Assignees: {string.Join(", ", task.AssigneeGuids)}");
        if (task.StartedAt.HasValue)
            lines.Add($"Started: {task.StartedAt:yyyy-MM-dd HH:mm:ss}");
        if (task.CompletedAt.HasValue)
            lines.Add($"Completed: {task.CompletedAt:yyyy-MM-dd HH:mm:ss}");
        if (task.ErrorMessage != null)
            lines.Add($"Error: {task.ErrorMessage}");
        if (task.Dependencies.Count > 0)
            lines.Add($"Dependencies: {string.Join(", ", task.Dependencies)}");

        return ToolResult.Successful(string.Join("\n", lines));
    }

    private static ToolResult ExecuteUpdate(ProjectTaskSystem taskSystem, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("task_id", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
        {
            return ToolResult.Failed("Missing or invalid 'task_id' parameter");
        }

        string? title = parameters.TryGetValue("title", out var titleObj) ? titleObj?.ToString() : null;
        string? description = parameters.TryGetValue("description", out var descObj) ? descObj?.ToString() : null;
        int? priority = null;
        if (parameters.TryGetValue("priority", out var priorityObj) && int.TryParse(priorityObj?.ToString(), out int p))
        {
            priority = p;
        }

        if (title == null && description == null && !priority.HasValue)
        {
            return ToolResult.Failed("At least one of 'title', 'description', or 'priority' must be provided for update");
        }

        if (!taskSystem.Update(taskId, title, description, priority))
        {
            return ToolResult.Failed($"Project task not found or update failed: {taskId}");
        }

        return ToolResult.Successful($"Project task {taskId} updated successfully.");
    }

    private static ToolResult ExecuteAssign(ProjectTaskSystem taskSystem, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("task_id", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
        {
            return ToolResult.Failed("Missing or invalid 'task_id' parameter");
        }

        if (!parameters.TryGetValue("assignee_id", out var assigneeObj) || !Guid.TryParse(assigneeObj?.ToString(), out Guid assigneeId))
        {
            return ToolResult.Failed("Missing or invalid 'assignee_id' parameter");
        }

        if (!taskSystem.Assign(taskId, assigneeId))
        {
            return ToolResult.Failed($"Failed to assign being {assigneeId} to task {taskId}");
        }

        return ToolResult.Successful($"Being {assigneeId} assigned to project task {taskId}.");
    }

    private static ToolResult ExecuteRemoveAssignee(ProjectTaskSystem taskSystem, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("task_id", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
        {
            return ToolResult.Failed("Missing or invalid 'task_id' parameter");
        }

        if (!parameters.TryGetValue("assignee_id", out var assigneeObj) || !Guid.TryParse(assigneeObj?.ToString(), out Guid assigneeId))
        {
            return ToolResult.Failed("Missing or invalid 'assignee_id' parameter");
        }

        if (!taskSystem.RemoveAssignee(taskId, assigneeId))
        {
            return ToolResult.Failed($"Failed to remove assignee {assigneeId} from task {taskId}");
        }

        return ToolResult.Successful($"Assignee {assigneeId} removed from project task {taskId}.");
    }

    private static ToolResult ExecuteStart(ProjectTaskSystem taskSystem, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("task_id", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
        {
            return ToolResult.Failed("Missing or invalid 'task_id' parameter");
        }

        if (!taskSystem.Start(taskId))
        {
            return ToolResult.Failed($"Failed to start project task {taskId}. Task may not exist or is not in pending status.");
        }

        return ToolResult.Successful($"Project task {taskId} started.");
    }

    private static ToolResult ExecuteComplete(ProjectTaskSystem taskSystem, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("task_id", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
        {
            return ToolResult.Failed("Missing or invalid 'task_id' parameter");
        }

        if (!taskSystem.Complete(taskId))
        {
            return ToolResult.Failed($"Failed to complete project task {taskId}. Task may not exist or is not in running status.");
        }

        return ToolResult.Successful($"Project task {taskId} marked as completed.");
    }

    private static ToolResult ExecuteFail(ProjectTaskSystem taskSystem, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("task_id", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
        {
            return ToolResult.Failed("Missing or invalid 'task_id' parameter");
        }

        string error = parameters.TryGetValue("error", out var errorObj) ? errorObj?.ToString() ?? "Unknown error" : "Unknown error";

        if (!taskSystem.Fail(taskId, error))
        {
            return ToolResult.Failed($"Failed to mark project task {taskId} as failed. Task may not exist or is not in running status.");
        }

        return ToolResult.Successful($"Project task {taskId} marked as failed: {error}");
    }

    private static ToolResult ExecuteCancel(ProjectTaskSystem taskSystem, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("task_id", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
        {
            return ToolResult.Failed("Missing or invalid 'task_id' parameter");
        }

        if (!taskSystem.Cancel(taskId))
        {
            return ToolResult.Failed($"Failed to cancel project task {taskId}. Task may not exist or is not in pending status.");
        }

        return ToolResult.Successful($"Project task {taskId} cancelled.");
    }

    private static ToolResult ExecuteDelete(ProjectTaskSystem taskSystem, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("task_id", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
        {
            return ToolResult.Failed("Missing or invalid 'task_id' parameter");
        }

        if (!taskSystem.Delete(taskId))
        {
            return ToolResult.Failed($"Project task not found: {taskId}");
        }

        return ToolResult.Successful($"Project task {taskId} deleted.");
    }

    private static ToolResult ExecuteStats(ProjectTaskSystem taskSystem)
    {
        var stats = taskSystem.GetStatistics();
        var lines = new List<string>
        {
            "Project Task Statistics:",
            $"  Total: {stats.Total}",
            $"  Pending: {stats.Pending}",
            $"  Running: {stats.Running}",
            $"  Completed: {stats.Completed}",
            $"  Failed: {stats.Failed}",
            $"  Cancelled: {stats.Cancelled}"
        };

        return ToolResult.Successful(string.Join("\n", lines));
    }
}
