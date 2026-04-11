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

namespace SiliconLife.Collective;

/// <summary>
/// Represents the status of a task.
/// </summary>
public enum TaskStatus
{
    /// <summary>
    /// The task is pending and not yet started.
    /// </summary>
    Pending,

    /// <summary>
    /// The task is currently running.
    /// </summary>
    Running,

    /// <summary>
    /// The task has been completed successfully.
    /// </summary>
    Completed,

    /// <summary>
    /// The task has failed.
    /// </summary>
    Failed,

    /// <summary>
    /// The task has been cancelled.
    /// </summary>
    Cancelled
}

/// <summary>
/// Represents a single task item in the task system.
/// </summary>
public sealed class TaskItem
{
    /// <summary>
    /// Gets or sets the unique identifier of the task.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the task.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of the task.
    /// </summary>
    public TaskStatus Status { get; set; } = TaskStatus.Pending;

    /// <summary>
    /// Gets or sets the priority of the task (lower values = higher priority).
    /// </summary>
    public int Priority { get; set; } = 100;

    /// <summary>
    /// Gets or sets the timestamp when the task was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the task started running.
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the task completed (successfully or not).
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Gets or sets the list of task IDs that this task depends on.
    /// </summary>
    public List<Guid> Dependencies { get; set; } = new();

    /// <summary>
    /// Gets or sets the error message if the task failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets additional metadata for the task.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the TaskItem class.
    /// </summary>
    public TaskItem()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    /// <summary>
    /// Initializes a new instance of the TaskItem class with the specified title and description.
    /// </summary>
    /// <param name="title">The title of the task.</param>
    /// <param name="description">The description of the task.</param>
    public TaskItem(string title, string description = "") : this()
    {
        Title = title;
        Description = description;
    }

    /// <summary>
    /// Determines whether the task can run based on its status and dependencies.
    /// </summary>
    /// <param name="allTasks">The list of all tasks to check dependencies against.</param>
    /// <returns>True if the task can run; otherwise, false.</returns>
    public bool CanRun(List<TaskItem> allTasks)
    {
        if (Status != TaskStatus.Pending)
            return false;

        foreach (var depId in Dependencies)
        {
            var dep = allTasks.FirstOrDefault(t => t.Id == depId);
            if (dep == null || dep.Status != TaskStatus.Completed)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Marks the task as started.
    /// </summary>
    public void Start()
    {
        Status = TaskStatus.Running;
        StartedAt = DateTime.Now;
    }

    /// <summary>
    /// Marks the task as completed successfully.
    /// </summary>
    public void Complete()
    {
        Status = TaskStatus.Completed;
        CompletedAt = DateTime.Now;
    }

    /// <summary>
    /// Marks the task as failed with an error message.
    /// </summary>
    /// <param name="error">The error message describing why the task failed.</param>
    public void Fail(string error)
    {
        Status = TaskStatus.Failed;
        CompletedAt = DateTime.Now;
        ErrorMessage = error;
    }

    /// <summary>
    /// Cancels the task.
    /// </summary>
    public void Cancel()
    {
        Status = TaskStatus.Cancelled;
        CompletedAt = DateTime.Now;
    }
}

/// <summary>
/// Provides statistics about the task system.
/// </summary>
public sealed class TaskStatistics
{
    /// <summary>
    /// Gets or sets the total number of tasks.
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// Gets or sets the number of pending tasks.
    /// </summary>
    public int Pending { get; set; }

    /// <summary>
    /// Gets or sets the number of running tasks.
    /// </summary>
    public int Running { get; set; }

    /// <summary>
    /// Gets or sets the number of completed tasks.
    /// </summary>
    public int Completed { get; set; }

    /// <summary>
    /// Gets or sets the number of failed tasks.
    /// </summary>
    public int Failed { get; set; }

    /// <summary>
    /// Gets or sets the number of cancelled tasks.
    /// </summary>
    public int Cancelled { get; set; }
}

/// <summary>
/// System for managing tasks with dependencies, priorities, and state tracking.
/// </summary>
public sealed class TaskSystem
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<TaskSystem>();
    private readonly IStorage _storage;
    private readonly string _storageKey;
    private readonly object _lock = new();

    private List<TaskItem> _tasks = new();

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
    /// Initializes a new instance of the TaskSystem class with the specified storage.
    /// </summary>
    /// <param name="storage">The storage to use for persisting tasks.</param>
    /// <exception cref="ArgumentNullException">Thrown when storage is null.</exception>
    public TaskSystem(IStorage storage)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _storageKey = "tasks";

        Load();
    }

    private void Load()
    {
        try
        {
            byte[]? data = _storage.Read(_storageKey);
            if (data == null || data.Length == 0)
                return;

            string json = System.Text.Encoding.UTF8.GetString(data);
            _tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }
        catch (Exception ex)
        {
            _logger.Warn("Failed to load tasks from storage", ex);
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
            string json = JsonSerializer.Serialize(_tasks);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(json);
            _storage.Write(_storageKey, data);
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to save tasks to storage", ex);
        }
    }

    /// <summary>
    /// Creates a new task with the specified parameters.
    /// </summary>
    /// <param name="title">The title of the task.</param>
    /// <param name="description">The description of the task.</param>
    /// <param name="priority">The priority of the task (lower values = higher priority).</param>
    /// <param name="dependencies">Optional list of task IDs that this task depends on.</param>
    /// <returns>The created task item.</returns>
    public TaskItem Create(string title, string description = "", int priority = 100, List<Guid>? dependencies = null)
    {
        lock (_lock)
        {
            var task = new TaskItem(title, description)
            {
                Priority = priority
            };

            if (dependencies != null)
            {
                task.Dependencies = dependencies;
            }

            _tasks.Add(task);
            Save();

            _logger.Info("Task added: {0} ({1}), priority={2}", title, task.Id, priority);

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
    /// Gets all tasks that can run (pending and with all dependencies completed).
    /// </summary>
    /// <returns>A list of runnable task items.</returns>
    public List<TaskItem> GetRunnableTasks()
    {
        lock (_lock)
        {
            return _tasks
                .Where(t => t.CanRun(_tasks))
                .OrderBy(t => t.Priority)
                .ThenBy(t => t.CreatedAt)
                .ToList();
        }
    }

    /// <summary>
    /// Attempts to start the next runnable task.
    /// </summary>
    /// <param name="task">The started task if successful; otherwise, null.</param>
    /// <returns>True if a task was started; otherwise, false.</returns>
    public bool TryStartNext(out TaskItem? task)
    {
        lock (_lock)
        {
            var runnable = GetRunnableTasks();
            task = runnable.FirstOrDefault();

            if (task != null)
            {
                task.Start();
                Save();
                _logger.Info("Task started: {0} ({1})", task.Title, task.Id);
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Checks whether there are any tasks that can run.
    /// </summary>
    /// <returns>True if there are runnable tasks; otherwise, false.</returns>
    public bool HasPendingTasks()
    {
        lock (_lock)
        {
            bool hasPending = _tasks.Any(t => t.CanRun(_tasks));
            _logger.Debug("Checking pending tasks: {0} pending", _tasks.Count(t => t.Status == TaskStatus.Pending));
            return hasPending;
        }
    }

    /// <summary>
    /// Marks a running task as completed.
    /// </summary>
    /// <param name="taskId">The ID of the task to complete.</param>
    public void Complete(Guid taskId)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null && task.Status == TaskStatus.Running)
            {
                task.Complete();
                Save();
                _logger.Info("Task completed: {0} ({1})", task.Title, task.Id);
            }
        }
    }

    /// <summary>
    /// Marks a running task as failed with an error message.
    /// </summary>
    /// <param name="taskId">The ID of the task to fail.</param>
    /// <param name="error">The error message.</param>
    public void Fail(Guid taskId, string error)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null && task.Status == TaskStatus.Running)
            {
                task.Fail(error);
                Save();
                _logger.Warn("Task failed: {0} ({1}), error={2}", task.Title, task.Id, error);
            }
        }
    }

    /// <summary>
    /// Cancels a pending task.
    /// </summary>
    /// <param name="taskId">The ID of the task to cancel.</param>
    public void Cancel(Guid taskId)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null && task.Status == TaskStatus.Pending)
            {
                task.Cancel();
                Save();
                _logger.Info("Task cancelled: {0} ({1})", task.Title, task.Id);
            }
        }
    }

    /// <summary>
    /// Updates the priority of a pending task.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="newPriority">The new priority value (lower = higher priority).</param>
    /// <returns>True if the priority was updated; otherwise, false.</returns>
    public bool UpdatePriority(Guid taskId, int newPriority)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null && task.Status == TaskStatus.Pending)
            {
                task.Priority = newPriority;
                Save();
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Adds a dependency to a pending task.
    /// </summary>
    /// <param name="taskId">The ID of the task to add the dependency to.</param>
    /// <param name="dependencyId">The ID of the task to depend on.</param>
    /// <returns>True if the dependency was added; otherwise, false.</returns>
    public bool AddDependency(Guid taskId, Guid dependencyId)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null && task.Status == TaskStatus.Pending && !task.Dependencies.Contains(dependencyId))
            {
                var dep = _tasks.FirstOrDefault(t => t.Id == dependencyId);
                if (dep != null)
                {
                    task.Dependencies.Add(dependencyId);
                    Save();
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Removes a dependency from a task.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="dependencyId">The ID of the dependency to remove.</param>
    public void RemoveDependency(Guid taskId, Guid dependencyId)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.Dependencies.Remove(dependencyId);
                Save();
            }
        }
    }

    /// <summary>
    /// Deletes a task (cannot delete running tasks).
    /// </summary>
    /// <param name="taskId">The ID of the task to delete.</param>
    /// <returns>True if the task was deleted; otherwise, false.</returns>
    public bool Delete(Guid taskId)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null && task.Status != TaskStatus.Running)
            {
                _tasks.Remove(task);
                Save();
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Clears all tasks or tasks with a specific status.
    /// </summary>
    /// <param name="status">The status to clear (null to clear all tasks).</param>
    public void Clear(TaskStatus? status = null)
    {
        lock (_lock)
        {
            if (status == null)
            {
                _tasks.Clear();
            }
            else
            {
                _tasks = _tasks.Where(t => t.Status != status).ToList();
            }

            Save();
        }
    }

    /// <summary>
    /// Gets statistics about the task system.
    /// </summary>
    /// <returns>A TaskStatistics object with counts for each status.</returns>
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

    /// <summary>
    /// Gets all tasks that depend on the specified task.
    /// </summary>
    /// <param name="taskId">The ID of the task to find dependents for.</param>
    /// <returns>A list of tasks that depend on the specified task.</returns>
    public List<TaskItem> GetDependents(Guid taskId)
    {
        lock (_lock)
        {
            return _tasks.Where(t => t.Dependencies.Contains(taskId)).ToList();
        }
    }

    /// <summary>
    /// Checks whether adding a dependency would create a circular dependency.
    /// </summary>
    /// <param name="taskId">The ID of the task to check.</param>
    /// <param name="newDependencyId">The ID of the proposed new dependency.</param>
    /// <returns>True if adding the dependency would create a cycle; otherwise, false.</returns>
    public bool HasCircularDependency(Guid taskId, Guid newDependencyId)
    {
        lock (_lock)
        {
            var visited = new HashSet<Guid>();
            var queue = new Queue<Guid>();
            queue.Enqueue(newDependencyId);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current == taskId)
                    return true;

                if (visited.Contains(current))
                    continue;

                visited.Add(current);

                var task = _tasks.FirstOrDefault(t => t.Id == current);
                if (task != null)
                {
                    foreach (var dep in task.Dependencies)
                    {
                        queue.Enqueue(dep);
                    }
                }
            }

            return false;
        }
    }
}
