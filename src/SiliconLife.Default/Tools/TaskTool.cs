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
/// Tool for managing silicon being tasks.
/// </summary>
public class TaskTool : ITool
{
    /// <inheritdoc/>
    public string Name => "task";

    /// <inheritdoc/>
    public string Description =>
        "Manage tasks for the silicon being. " +
        "Actions: 'create' (create a new task), 'list' (list tasks), 'get' (get task details), " +
        "'complete' (mark task complete), 'fail' (mark task failed), 'cancel' (cancel task), " +
        "'delete' (delete task), 'update_priority' (change priority), " +
        "'add_dependency' (add task dependency), 'stats' (get task statistics).";

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
                    ["description"] = "The action to perform: create, list, get, complete, fail, cancel, delete, update_priority, add_dependency, stats",
                    ["enum"] = new[] { "create", "list", "get", "complete", "fail", "cancel", "delete", "update_priority", "add_dependency", "stats" }
                },
                ["title"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Task title (used with action=create)"
                },
                ["description"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Task description (used with action=create)"
                },
                ["priority"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Task priority (lower number = higher priority, used with action=create, update_priority)"
                },
                ["task_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Task ID (GUID, used with get, complete, fail, cancel, delete, update_priority, add_dependency)"
                },
                ["status"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Filter by status: pending, running, completed, failed, cancelled, all (used with action=list)",
                    ["enum"] = new[] { "pending", "running", "completed", "failed", "cancelled", "all" }
                },
                ["dependency_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Dependency task ID (GUID, used with action=add_dependency)"
                },
                ["error"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Error message (used with action=fail)"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        var being = GetSiliconBeing(callerId);
        if (being?.TaskSystem == null)
        {
            return ToolResult.Failed("Task system not available");
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
                "create" => ExecuteCreate(being, parameters),
                "list" => ExecuteList(being, parameters),
                "get" => ExecuteGet(being, parameters),
                "complete" => ExecuteComplete(being, parameters),
                "fail" => ExecuteFail(being, parameters),
                "cancel" => ExecuteCancel(being, parameters),
                "delete" => ExecuteDelete(being, parameters),
                "update_priority" => ExecuteUpdatePriority(being, parameters),
                "add_dependency" => ExecuteAddDependency(being, parameters),
                "stats" => ExecuteStats(being, parameters),
                _ => ToolResult.Failed($"Unknown action: {action}")
            };
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Task operation failed: {ex.Message}");
        }
    }

    private SiliconBeingBase? GetSiliconBeing(Guid callerId)
    {
        var manager = ServiceLocator.Instance.BeingManager;
        if (manager == null)
            return null;

        return manager.GetBeing(callerId);
    }

    private ToolResult ExecuteCreate(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("title", out object? titleObj) || string.IsNullOrWhiteSpace(titleObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'title' parameter");
        }

        string title = titleObj!.ToString()!;
        string description = parameters.TryGetValue("description", out object? descObj) ? descObj?.ToString() ?? "" : "";
        int priority = 100;
        if (parameters.TryGetValue("priority", out object? priorityObj) && int.TryParse(priorityObj?.ToString(), out int p))
        {
            priority = p;
        }

        var task = being.TaskSystem!.Create(title, description, priority);

        return ToolResult.Successful($"Task created (ID: {task.Id}, Priority: {task.Priority})");
    }

    private ToolResult ExecuteList(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        Collective.TaskStatus? status = null;
        if (parameters.TryGetValue("status", out object? statusObj) && !string.IsNullOrWhiteSpace(statusObj?.ToString()))
        {
            status = statusObj!.ToString()!.ToLowerInvariant() switch
            {
                "pending" => Collective.TaskStatus.Pending,
                "running" => Collective.TaskStatus.Running,
                "completed" => Collective.TaskStatus.Completed,
                "failed" => Collective.TaskStatus.Failed,
                "cancelled" => Collective.TaskStatus.Cancelled,
                _ => null
            };
        }

        var tasks = being.TaskSystem!.GetAll(status);

        if (tasks.Count == 0)
        {
            return ToolResult.Successful("No tasks found.");
        }

        var lines = new List<string> { $"Found {tasks.Count} tasks:" };
        foreach (var task in tasks.OrderBy(t => t.Priority).Take(20))
        {
            lines.Add($"- [{task.Status}] #{task.Priority} {task.Title} (ID: {task.Id})");
        }

        return ToolResult.Successful(string.Join("\n", lines));
    }

    private ToolResult ExecuteGet(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("task_id", out object? idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
        {
            return ToolResult.Failed("Missing or invalid 'task_id' parameter");
        }

        var task = being.TaskSystem!.Get(taskId);
        if (task == null)
        {
            return ToolResult.Failed($"Task not found: {taskId}");
        }

        var lines = new List<string>
        {
            $"Task: {task.Title}",
            $"ID: {task.Id}",
            $"Status: {task.Status}",
            $"Priority: {task.Priority}",
            $"Created: {task.CreatedAt:yyyy-MM-dd HH:mm:ss}",
            $"Description: {task.Description}"
        };

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

    private ToolResult ExecuteComplete(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("task_id", out object? idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
        {
            return ToolResult.Failed("Missing or invalid 'task_id' parameter");
        }

        being.TaskSystem!.Complete(taskId);
        return ToolResult.Successful($"Task {taskId} marked as completed.");
    }

    private ToolResult ExecuteFail(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("task_id", out object? idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
        {
            return ToolResult.Failed("Missing or invalid 'task_id' parameter");
        }

        string error = parameters.TryGetValue("error", out object? errorObj) ? errorObj?.ToString() ?? "Unknown error" : "Unknown error";
        being.TaskSystem!.Fail(taskId, error);
        return ToolResult.Successful($"Task {taskId} marked as failed: {error}");
    }

    private ToolResult ExecuteCancel(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("task_id", out object? idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
        {
            return ToolResult.Failed("Missing or invalid 'task_id' parameter");
        }

        being.TaskSystem!.Cancel(taskId);
        return ToolResult.Successful($"Task {taskId} cancelled.");
    }

    private ToolResult ExecuteDelete(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("task_id", out object? idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
        {
            return ToolResult.Failed("Missing or invalid 'task_id' parameter");
        }

        bool deleted = being.TaskSystem!.Delete(taskId);
        if (deleted)
        {
            return ToolResult.Successful($"Task {taskId} deleted.");
        }
        else
        {
            return ToolResult.Failed($"Cannot delete task {taskId} (may be running or not found)");
        }
    }

    private ToolResult ExecuteUpdatePriority(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("task_id", out object? idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
        {
            return ToolResult.Failed("Missing or invalid 'task_id' parameter");
        }

        if (!parameters.TryGetValue("priority", out object? priorityObj) || !int.TryParse(priorityObj?.ToString(), out int priority))
        {
            return ToolResult.Failed("Missing or invalid 'priority' parameter");
        }

        bool updated = being.TaskSystem!.UpdatePriority(taskId, priority);
        if (updated)
        {
            return ToolResult.Successful($"Task {taskId} priority updated to {priority}.");
        }
        else
        {
            return ToolResult.Failed($"Cannot update priority for task {taskId} (may not be pending or not found)");
        }
    }

    private ToolResult ExecuteAddDependency(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("task_id", out object? idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
        {
            return ToolResult.Failed("Missing or invalid 'task_id' parameter");
        }

        if (!parameters.TryGetValue("dependency_id", out object? depObj) || !Guid.TryParse(depObj?.ToString(), out Guid depId))
        {
            return ToolResult.Failed("Missing or invalid 'dependency_id' parameter");
        }

        if (being.TaskSystem!.HasCircularDependency(taskId, depId))
        {
            return ToolResult.Failed("Cannot add dependency: would create a circular dependency");
        }

        bool added = being.TaskSystem.AddDependency(taskId, depId);
        if (added)
        {
            return ToolResult.Successful($"Added dependency: task {taskId} depends on {depId}.");
        }
        else
        {
            return ToolResult.Failed($"Cannot add dependency (task may not exist or not be pending)");
        }
    }

    private ToolResult ExecuteStats(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        var stats = being.TaskSystem!.GetStatistics();

        var lines = new List<string>
        {
            "Task Statistics:",
            $"- Total: {stats.Total}",
            $"- Pending: {stats.Pending}",
            $"- Running: {stats.Running}",
            $"- Completed: {stats.Completed}",
            $"- Failed: {stats.Failed}",
            $"- Cancelled: {stats.Cancelled}"
        };

        return ToolResult.Successful(string.Join("\n", lines));
    }
}
