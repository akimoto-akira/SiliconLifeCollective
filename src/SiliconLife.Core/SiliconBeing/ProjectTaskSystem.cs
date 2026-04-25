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

namespace SiliconLife.Collective;

/// <summary>
/// System for managing tasks within a project space.
/// Project tasks are independent from being tasks and support assignees.
/// </summary>
public sealed class ProjectTaskSystem
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<ProjectTaskSystem>();
    private readonly Guid _projectId;
    private readonly IStorage _storage;
    private readonly string _storageKey;
    private readonly object _lock = new();

    private List<TaskItem> _tasks = new();

    /// <summary>
    /// Gets the project ID that owns this task system.
    /// </summary>
    public Guid ProjectId => _projectId;

    /// <summary>
    /// Gets the total number of tasks.
    /// </summary>
    public int Count => _tasks.Count;

    /// <summary>
    /// Gets the number of pending tasks.
    /// </summary>
    public int PendingCount => _tasks.Count(t => t.Status == TaskStatus.Pending);

    /// <summary>
    /// Gets the number of running tasks.
    /// </summary>
    public int RunningCount => _tasks.Count(t => t.Status == TaskStatus.Running);

    /// <summary>
    /// Initializes a new instance of the ProjectTaskSystem class.
    /// </summary>
    /// <param name="projectId">The project ID that owns this task system.</param>
    /// <param name="storage">The storage to use for persisting tasks.</param>
    public ProjectTaskSystem(Guid projectId, IStorage storage)
    {
        _projectId = projectId;
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _storageKey = "project-tasks";

        _logger.Info(null, "ProjectTaskSystem created for project {0}", projectId);

        Load();
    }

    private void Load()
    {
        try
        {
            List<TaskItem>? tasks = _storage.Read<List<TaskItem>>(_storageKey);
            _tasks = tasks ?? new List<TaskItem>();
        }
        catch (Exception)
        {
            _logger.Warn(null, "Failed to load project tasks from storage for project {0}", _projectId);
            _tasks = new List<TaskItem>();
        }
    }

    /// <summary>
    /// Saves all tasks to storage.
    /// </summary>
    public void Save()
    {
        try
        {
            _storage.Write(_storageKey, _tasks);
        }
        catch (Exception)
        {
            _logger.Error(null, "Failed to save project tasks to storage for project {0}", _projectId);
        }
    }

    /// <summary>
    /// Creates a new project task with the specified parameters.
    /// </summary>
    /// <param name="title">The title of the task.</param>
    /// <param name="description">The description of the task.</param>
    /// <param name="createdBy">The GUID of the being creating the task.</param>
    /// <param name="priority">The priority of the task (lower values = higher priority).</param>
    /// <param name="assigneeGuids">Optional list of assignee GUIDs.</param>
    /// <param name="dependencies">Optional list of task IDs that this task depends on.</param>
    /// <returns>The created task item.</returns>
    public TaskItem Create(string title, string description, Guid createdBy, int priority = 100, List<Guid>? assigneeGuids = null, List<Guid>? dependencies = null)
    {
        lock (_lock)
        {
            var task = new TaskItem(title, description)
            {
                ProjectId = _projectId,
                CreatedByGuid = createdBy,
                Priority = priority
            };

            if (assigneeGuids != null)
            {
                task.AssigneeGuids = assigneeGuids.ToList();
            }

            if (dependencies != null)
            {
                task.Dependencies = dependencies.ToList();
            }

            _tasks.Add(task);
            Save();

            _logger.Info(null, "Project task added: {0} ({1}), project={2}, priority={3}", title, task.Id, _projectId, priority);

            return task;
        }
    }

    /// <summary>
    /// Gets a task by its ID.
    /// </summary>
    /// <param name="taskId">The ID of the task to retrieve.</param>
    /// <returns>The task item if found; otherwise, null.</returns>
    public TaskItem? Get(Guid taskId)
    {
        lock (_lock)
        {
            return _tasks.FirstOrDefault(t => t.Id == taskId);
        }
    }

    /// <summary>
    /// Gets all tasks, optionally filtered by status.
    /// </summary>
    /// <param name="status">The status to filter by (null for all tasks).</param>
    /// <returns>A list of task items.</returns>
    public List<TaskItem> GetAll(TaskStatus? status = null)
    {
        lock (_lock)
        {
            if (status == null)
                return _tasks.ToList();

            return _tasks.Where(t => t.Status == status).ToList();
        }
    }

    /// <summary>
    /// Gets all pending tasks ordered by priority and creation time.
    /// </summary>
    /// <returns>A list of pending task items.</returns>
    public List<TaskItem> GetPending()
    {
        lock (_lock)
        {
            return _tasks
                .Where(t => t.Status == TaskStatus.Pending)
                .OrderBy(t => t.Priority)
                .ThenBy(t => t.CreatedAt)
                .ToList();
        }
    }

    /// <summary>
    /// Updates a task's properties.
    /// </summary>
    /// <param name="taskId">The ID of the task to update.</param>
    /// <param name="title">New title (null to keep existing).</param>
    /// <param name="description">New description (null to keep existing).</param>
    /// <param name="priority">New priority (null to keep existing).</param>
    /// <returns>True if updated successfully; otherwise, false.</returns>
    public bool Update(Guid taskId, string? title = null, string? description = null, int? priority = null)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return false;

            if (title != null)
                task.Title = title;

            if (description != null)
                task.Description = description;

            if (priority.HasValue)
                task.Priority = priority.Value;

            Save();
            _logger.Info(null, "Project task updated: {0} ({1})", task.Title, taskId);
            return true;
        }
    }

    /// <summary>
    /// Assigns a being to a task.
    /// </summary>
    /// <param name="taskId">The task ID.</param>
    /// <param name="beingGuid">The being GUID to assign.</param>
    /// <returns>True if assigned successfully; otherwise, false.</returns>
    public bool Assign(Guid taskId, Guid beingGuid)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return false;

            if (!task.AssigneeGuids.Contains(beingGuid))
            {
                task.AssigneeGuids.Add(beingGuid);
                Save();
                _logger.Info(null, "Assigned being {0} to project task {1}", beingGuid, taskId);
            }

            return true;
        }
    }

    /// <summary>
    /// Removes an assignee from a task.
    /// </summary>
    /// <param name="taskId">The task ID.</param>
    /// <param name="beingGuid">The being GUID to remove.</param>
    /// <returns>True if removed successfully; otherwise, false.</returns>
    public bool RemoveAssignee(Guid taskId, Guid beingGuid)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return false;

            if (task.AssigneeGuids.Remove(beingGuid))
            {
                Save();
                _logger.Info(null, "Removed assignee {0} from project task {1}", beingGuid, taskId);
            }

            return true;
        }
    }

    /// <summary>
    /// Marks a task as started.
    /// </summary>
    /// <param name="taskId">The task ID.</param>
    /// <returns>True if started successfully; otherwise, false.</returns>
    public bool Start(Guid taskId)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null || task.Status != TaskStatus.Pending)
                return false;

            task.Start();
            Save();
            _logger.Info(null, "Project task started: {0} ({1})", task.Title, taskId);
            return true;
        }
    }

    /// <summary>
    /// Marks a task as completed successfully.
    /// </summary>
    /// <param name="taskId">The task ID.</param>
    /// <returns>True if completed successfully; otherwise, false.</returns>
    public bool Complete(Guid taskId)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null || task.Status != TaskStatus.Running)
                return false;

            task.Complete();
            Save();
            _logger.Info(null, "Project task completed: {0} ({1})", task.Title, taskId);
            return true;
        }
    }

    /// <summary>
    /// Marks a task as failed with an error message.
    /// </summary>
    /// <param name="taskId">The task ID.</param>
    /// <param name="error">The error message.</param>
    /// <returns>True if failed successfully; otherwise, false.</returns>
    public bool Fail(Guid taskId, string error)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null || task.Status != TaskStatus.Running)
                return false;

            task.Fail(error);
            Save();
            _logger.Info(null, "Project task failed: {0} ({1}), error={2}", task.Title, taskId, error);
            return true;
        }
    }

    /// <summary>
    /// Cancels a pending task.
    /// </summary>
    /// <param name="taskId">The task ID.</param>
    /// <returns>True if cancelled successfully; otherwise, false.</returns>
    public bool Cancel(Guid taskId)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null || task.Status != TaskStatus.Pending)
                return false;

            task.Cancel();
            Save();
            _logger.Info(null, "Project task cancelled: {0} ({1})", task.Title, taskId);
            return true;
        }
    }

    /// <summary>
    /// Deletes a task by its ID.
    /// </summary>
    /// <param name="taskId">The task ID.</param>
    /// <returns>True if deleted successfully; otherwise, false.</returns>
    public bool Delete(Guid taskId)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return false;

            _tasks.Remove(task);
            Save();
            _logger.Info(null, "Project task deleted: {0} ({1})", task.Title, taskId);
            return true;
        }
    }

    /// <summary>
    /// Gets statistics about the task system.
    /// </summary>
    /// <returns>A TaskStatistics object containing counts.</returns>
    public TaskStatistics GetStatistics()
    {
        lock (_lock)
        {
            return new TaskStatistics
            {
                Total = _tasks.Count,
                Pending = _tasks.Count(t => t.Status == TaskStatus.Pending),
                Running = _tasks.Count(t => t.Status == TaskStatus.Running),
                Completed = _tasks.Count(t => t.Status == TaskStatus.Completed),
                Failed = _tasks.Count(t => t.Status == TaskStatus.Failed),
                Cancelled = _tasks.Count(t => t.Status == TaskStatus.Cancelled)
            };
        }
    }
}
