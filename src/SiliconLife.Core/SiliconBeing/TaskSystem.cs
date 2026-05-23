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
    Pending,
    Running,
    SubmittedForReview,
    UnderReview,
    Rework,
    Completed,
    Failed,
    Cancelled
}

public sealed class ChatHistoryCycle
{
    public List<ChatMessage> Messages { get; set; } = new();
    public string StartStatus { get; set; } = string.Empty;
    public string? EndStatus { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public DateTime? TriggerTime { get; set; }

    public ChatHistoryCycle() { }

    public ChatHistoryCycle(TaskStatus startStatus)
    {
        StartStatus = startStatus.ToString();
        StartedAt = DateTime.Now;
    }

    public ChatHistoryCycle(TimerExecutionState startStatus, DateTime? triggerTime = null)
    {
        StartStatus = startStatus.ToString();
        StartedAt = DateTime.Now;
        TriggerTime = triggerTime;
    }

    public TaskStatus GetTaskStartStatus() => Enum.TryParse<TaskStatus>(StartStatus, out var s) ? s : TaskStatus.Pending;
    public TaskStatus? GetTaskEndStatus() => EndStatus != null && Enum.TryParse<TaskStatus>(EndStatus, out var s) ? s : null;
    public TimerExecutionState GetTimerStartStatus() => Enum.TryParse<TimerExecutionState>(StartStatus, out var s) ? s : TimerExecutionState.Idle;
    public TimerExecutionState? GetTimerEndStatus() => EndStatus != null && Enum.TryParse<TimerExecutionState>(EndStatus, out var s) ? s : null;

    public void SetEndStatus(TaskStatus status) { EndStatus = status.ToString(); EndedAt = DateTime.Now; }
    public void SetEndStatus(TimerExecutionState status) { EndStatus = status.ToString(); EndedAt = DateTime.Now; }
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
    /// Gets or sets the project ID this task belongs to. Null for private being tasks.
    /// </summary>
    public Guid? ProjectId { get; set; }

    public Guid AssigneeGuid { get; set; }

    public Guid ExecutorGuid { get; set; }

    public Guid? ReviewerGuid { get; set; }

    public List<string> RequiredTools { get; set; } = new();

    public Guid CreatedByGuid { get; set; }

    public List<ChatHistoryCycle> ChatHistory { get; set; } = new();

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

    public void Cancel()
    {
        Status = TaskStatus.Cancelled;
        CompletedAt = DateTime.Now;
    }

    public void SubmitForReview()
    {
        Status = TaskStatus.SubmittedForReview;
    }

    public void StartReview()
    {
        Status = TaskStatus.UnderReview;
    }

    public void ReworkTask()
    {
        Status = TaskStatus.Rework;
    }

    public ChatHistoryCycle GetCurrentCycle()
    {
        if (ChatHistory.Count == 0)
        {
            ChatHistory.Add(new ChatHistoryCycle(Status));
        }
        return ChatHistory[^1];
    }

    public void SealCurrentCycle(TaskStatus endStatus)
    {
        if (ChatHistory.Count == 0) return;
        ChatHistory[^1].SetEndStatus(endStatus);
    }

    public void AppendNewCycle()
    {
        ChatHistory.Add(new ChatHistoryCycle(Status));
    }
}

public sealed class TaskStatistics
{
    public int Total { get; set; }
    public int Pending { get; set; }
    public int Running { get; set; }
    public int SubmittedForReview { get; set; }
    public int UnderReview { get; set; }
    public int Rework { get; set; }
    public int Completed { get; set; }
    public int Failed { get; set; }
    public int Cancelled { get; set; }
}

/// <summary>
/// System for managing tasks with dependencies, priorities, and state tracking.
/// </summary>
public sealed class TaskSystem
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<TaskSystem>();
    private readonly SiliconBeingBase _owner;

    public Guid OwnerId => _owner.Id;
    public string OwnerName => _owner.Name;
    public bool IsCurator => _owner.IsCurator;
    public int Count => TaskCenter.Instance.GetTasksForBeing(OwnerId).Count();
    public int PendingCount => TaskCenter.Instance.GetRunnableTasks(OwnerId).Count;
    public int RunningCount => TaskCenter.Instance.GetTasksForBeing(OwnerId).Count(t => t.Status == TaskStatus.Running);

    public TaskSystem(SiliconBeingBase owner)
    {
        _owner = owner ?? throw new ArgumentNullException(nameof(owner));

        _logger.Info(_owner.Id, "TaskSystem created for being {0} ({1})", owner.Name, owner.Id);
    }

    /// <summary>
    /// Creates a new task with the specified parameters.
    /// </summary>
    /// <param name="title">The title of the task.</param>
    /// <param name="description">The description of the task.</param>
    /// <param name="priority">The priority of the task (lower values = higher priority).</param>
    /// <param name="dependencies">Optional list of task IDs that this task depends on.</param>
    /// <returns>The created task item.</returns>
    public TaskItem Create(string title, string description = "", int priority = 100, List<Guid>? dependencies = null, Guid? assigneeGuid = null, Guid? executorGuid = null)
    {
        Guid effectiveAssignee = assigneeGuid ?? OwnerId;
        Guid effectiveExecutor = executorGuid ?? OwnerId;

        var task = new TaskItem(title, description)
        {
            Priority = priority,
            AssigneeGuid = effectiveAssignee,
            ExecutorGuid = effectiveExecutor,
            CreatedByGuid = OwnerId
        };

        if (dependencies != null)
        {
            task.Dependencies = dependencies;
        }

        task.ChatHistory.Add(new ChatHistoryCycle(TaskStatus.Pending));

        TaskCenter.Instance.AddTask(task);

        _logger.Info(_owner.Id, "Task created and added to TaskCenter: {0} ({1}), priority={2}", title, task.Id, priority);

        return task;
    }

    /// <summary>
    /// Gets a task by its ID from TaskCenter.
    /// </summary>
    /// <param name="taskId">The ID of the task to retrieve.</param>
    /// <returns>The task item if found; otherwise, null.</returns>
    public TaskItem? Get(Guid taskId)
    {
        return TaskCenter.Instance.GetTask(taskId);
    }

    /// <summary>
    /// Gets all tasks, optionally filtered by status.
    /// </summary>
    /// <param name="status">The status to filter by (null for all tasks).</param>
    /// <returns>A list of task items.</returns>
    public List<TaskItem> GetAll(TaskStatus? status = null)
    {
        var tasks = TaskCenter.Instance.GetTasksForBeing(OwnerId);
        
        if (status == null)
            return tasks.ToList();

        return tasks.Where(t => t.Status == status).ToList();
    }

    /// <summary>
    /// Gets all pending tasks ordered by priority and creation time.
    /// </summary>
    /// <returns>A list of pending task items.</returns>
    public List<TaskItem> GetPending()
    {
        return TaskCenter.Instance.GetRunnableTasks(OwnerId);
    }

    /// <summary>
    /// Gets all tasks that can run by the specified being (pending, dependencies completed, and being is an executor).
    /// </summary>
    /// <param name="beingGuid">The GUID of the being to filter tasks for.</param>
    /// <returns>A list of runnable task items for the specified being.</returns>
    public List<TaskItem> GetRunnableTasks(Guid beingGuid)
    {
        return TaskCenter.Instance.GetRunnableTasks(beingGuid);
    }

    /// <summary>
    /// Attempts to start the next runnable task for the specified being.
    /// </summary>
    /// <param name="beingGuid">The GUID of the being to get tasks for.</param>
    /// <param name="task">The started task if successful; otherwise, null.</param>
    /// <returns>True if a task was started; otherwise, false.</returns>
    public bool TryStartNext(Guid beingGuid, out TaskItem? task)
    {
        var runnable = GetRunnableTasks(beingGuid);
        task = runnable.FirstOrDefault();

        if (task != null)
        {
            task.Start();
            TaskCenter.Instance.UpdateTask(task);
            _logger.Info(_owner.Id, "Task started: {0} ({1})", task.Title, task.Id);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks whether there are any tasks that can run by the specified being.
    /// </summary>
    /// <param name="beingGuid">The GUID of the being to check tasks for.</param>
    /// <returns>True if there are runnable tasks for the being; otherwise, false.</returns>
    public bool HasPendingTasks(Guid beingGuid)
    {
        return TaskCenter.Instance.HasPendingTasks(beingGuid);
    }

    /// <summary>
    /// Starts a pending task, transitioning it to Running status.
    /// </summary>
    /// <param name="taskId">The ID of the task to start.</param>
    /// <returns>True if the task was started; otherwise, false.</returns>
    public bool Start(Guid taskId)
    {
        var task = TaskCenter.Instance.GetTask(taskId);
        if (task != null && task.Status == TaskStatus.Pending)
        {
            task.Start();
            TaskCenter.Instance.UpdateTask(task);
            _logger.Info(_owner.Id, "Task started: {0} ({1})", task.Title, task.Id);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Marks a running task as completed.
    /// </summary>
    /// <param name="taskId">The ID of the task to complete.</param>
    public void Complete(Guid taskId)
    {
        var task = TaskCenter.Instance.GetTask(taskId);
        if (task != null && task.Status == TaskStatus.Running)
        {
            task.Complete();
            TaskCenter.Instance.UpdateTask(task);
            _logger.Info(_owner.Id, "Task completed: {0} ({1})", task.Title, task.Id);
        }
    }

    /// <summary>
    /// Marks a running task as failed with an error message.
    /// </summary>
    /// <param name="taskId">The ID of the task to fail.</param>
    /// <param name="error">The error message.</param>
    public void Fail(Guid taskId, string error)
    {
        var task = TaskCenter.Instance.GetTask(taskId);
        if (task != null && task.Status == TaskStatus.Running)
        {
            task.Fail(error);
            TaskCenter.Instance.UpdateTask(task);
            _logger.Warn(_owner.Id, "Task failed: {0} ({1}), error={2}", task.Title, task.Id, error);
        }
    }

    /// <summary>
    /// Cancels a pending task.
    /// </summary>
    /// <param name="taskId">The ID of the task to cancel.</param>
    public void Cancel(Guid taskId)
    {
        var task = TaskCenter.Instance.GetTask(taskId);
        if (task != null && task.Status == TaskStatus.Pending)
        {
            task.Cancel();
            TaskCenter.Instance.UpdateTask(task);
            _logger.Info(_owner.Id, "Task cancelled: {0} ({1})", task.Title, task.Id);
        }
    }

    public bool SubmitForReview(Guid taskId)
    {
        var task = TaskCenter.Instance.GetTask(taskId);
        if (task == null || task.Status != TaskStatus.Running)
            return false;

        task.SealCurrentCycle(TaskStatus.SubmittedForReview);
        task.SubmitForReview();
        task.AppendNewCycle();
        TaskCenter.Instance.UpdateTask(task);
        _logger.Info(_owner.Id, "Task submitted for review: {0} ({1})", task.Title, task.Id);
        return true;
    }

    /// <summary>
    /// Updates the priority of a pending task.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="newPriority">The new priority value (lower = higher priority).</param>
    /// <returns>True if the priority was updated; otherwise, false.</returns>
    public bool UpdatePriority(Guid taskId, int newPriority)
    {
        var task = TaskCenter.Instance.GetTask(taskId);
        if (task != null && task.Status == TaskStatus.Pending)
        {
            task.Priority = newPriority;
            TaskCenter.Instance.UpdateTask(task);
            return true;
        }

        return false;
    }

    public bool AddDependency(Guid taskId, Guid dependencyId)
    {
        var task = TaskCenter.Instance.GetTask(taskId);
        if (task != null && task.Status == TaskStatus.Pending && !task.Dependencies.Contains(dependencyId))
        {
            var dep = TaskCenter.Instance.GetTask(dependencyId);
            if (dep != null)
            {
                task.Dependencies.Add(dependencyId);
                TaskCenter.Instance.UpdateTask(task);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Removes a dependency from a task.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="dependencyId">The ID of the dependency to remove.</param>
    public void RemoveDependency(Guid taskId, Guid dependencyId)
    {
        var task = TaskCenter.Instance.GetTask(taskId);
        if (task != null)
        {
            task.Dependencies.Remove(dependencyId);
            TaskCenter.Instance.UpdateTask(task);
        }
    }

    public bool Delete(Guid taskId)
    {
        var task = TaskCenter.Instance.GetTask(taskId);
        if (task != null && task.Status != TaskStatus.Running)
        {
            TaskCenter.Instance.RemoveTask(taskId);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Clears all tasks or tasks with a specific status.
    /// </summary>
    /// <param name="status">The status to clear (null to clear all tasks).</param>
    public void Clear(TaskStatus? status = null)
    {
        var tasks = TaskCenter.Instance.GetTasksForBeing(OwnerId);
        if (status == null)
        {
            foreach (var task in tasks.ToList())
            {
                TaskCenter.Instance.RemoveTask(task.Id);
            }
        }
        else
        {
            foreach (var task in tasks.Where(t => t.Status == status).ToList())
            {
                TaskCenter.Instance.RemoveTask(task.Id);
            }
        }
    }

    public TaskStatistics GetStatistics()
    {
        var tasks = TaskCenter.Instance.GetTasksForBeing(OwnerId).ToList();
        return new TaskStatistics
        {
            Total = tasks.Count,
            Pending = tasks.Count(t => t.Status == TaskStatus.Pending),
            Running = tasks.Count(t => t.Status == TaskStatus.Running),
            SubmittedForReview = tasks.Count(t => t.Status == TaskStatus.SubmittedForReview),
            UnderReview = tasks.Count(t => t.Status == TaskStatus.UnderReview),
            Rework = tasks.Count(t => t.Status == TaskStatus.Rework),
            Completed = tasks.Count(t => t.Status == TaskStatus.Completed),
            Failed = tasks.Count(t => t.Status == TaskStatus.Failed),
            Cancelled = tasks.Count(t => t.Status == TaskStatus.Cancelled)
        };
    }

    /// <summary>
    /// Gets all tasks that depend on the specified task.
    /// </summary>
    /// <param name="taskId">The ID of the task to find dependents for.</param>
    /// <returns>A list of tasks that depend on the specified task.</returns>
    public List<TaskItem> GetDependents(Guid taskId)
    {
        return TaskCenter.Instance.GetTasksForBeing(OwnerId)
            .Where(t => t.Dependencies.Contains(taskId))
            .ToList();
    }

    /// <summary>
    /// Checks whether adding a dependency would create a circular dependency.
    /// </summary>
    /// <param name="taskId">The ID of the task to check.</param>
    /// <param name="newDependencyId">The ID of the proposed new dependency.</param>
    /// <returns>True if adding the dependency would create a cycle; otherwise, false.</returns>
    public bool HasCircularDependency(Guid taskId, Guid newDependencyId)
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

            var task = TaskCenter.Instance.GetTask(current);
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
