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

using System.Collections.Concurrent;

namespace SiliconLife.Collective;

/// <summary>
/// Centralized task management center - the authoritative data source for all tasks.
/// Implements singleton pattern for global access and data consistency.
/// </summary>
public class TaskCenter
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<TaskCenter>();
    private static TaskCenter? _instance;
    private static readonly object _lock = new();
    
    // Thread-safe storage for all tasks
    private readonly ConcurrentDictionary<Guid, TaskItem> _tasks = new();
    
    // Private constructor for singleton
    private TaskCenter() {}
    
    /// <summary>
    /// Gets the singleton instance of TaskCenter
    /// </summary>
    public static TaskCenter Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new TaskCenter();
                        _logger.Info(null, "TaskCenter singleton instance created");
                    }
                }
            }
            return _instance;
        }
    }
    
    /// <summary>
    /// Adds a new task to the task center
    /// </summary>
    /// <param name="task">The task to add</param>
    /// <returns>True if added successfully, false if task with same ID already exists</returns>
    public bool AddTask(TaskItem task)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task));
            
        bool added = _tasks.TryAdd(task.Id, task);
        if (added)
        {
            _logger.Info(null, "Task added to TaskCenter: {0} (ID: {1})", task.Title, task.Id);
        }
        else
        {
            _logger.Warn(null, "Task already exists in TaskCenter: {0} (ID: {1})", task.Title, task.Id);
        }
        return added;
    }
    
    /// <summary>
    /// Updates an existing task in the task center
    /// </summary>
    /// <param name="task">The task with updated information</param>
    /// <returns>True if updated successfully, false if task doesn't exist</returns>
    public bool UpdateTask(TaskItem task)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task));
            
        if (!_tasks.ContainsKey(task.Id))
        {
            _logger.Warn(null, "Task not found for update: {0} (ID: {1})", task.Title, task.Id);
            return false;
        }
        
        _tasks[task.Id] = task;
        _logger.Debug(null, "Task updated in TaskCenter: {0} (ID: {1})", task.Title, task.Id);
        return true;
    }
    
    /// <summary>
    /// Removes a task from the task center
    /// </summary>
    /// <param name="taskId">The ID of the task to remove</param>
    /// <returns>True if removed successfully, false if task doesn't exist</returns>
    public bool RemoveTask(Guid taskId)
    {
        bool removed = _tasks.TryRemove(taskId, out TaskItem? removedTask);
        if (removed)
        {
            _logger.Info(null, "Task removed from TaskCenter: {0} (ID: {1})", removedTask?.Title ?? "Unknown", taskId);
        }
        else
        {
            _logger.Warn(null, "Task not found for removal (ID: {0})", taskId);
        }
        return removed;
    }
    
    /// <summary>
    /// Gets a task by its ID
    /// </summary>
    /// <param name="taskId">The ID of the task to retrieve</param>
    /// <returns>The task if found, null otherwise</returns>
    public TaskItem? GetTask(Guid taskId)
    {
        _tasks.TryGetValue(taskId, out TaskItem? task);
        return task;
    }
    
    /// <summary>
    /// Gets all tasks in the task center
    /// </summary>
    /// <returns>Collection of all tasks</returns>
    public IEnumerable<TaskItem> GetAllTasks()
    {
        return _tasks.Values;
    }
    
    /// <summary>
    /// Gets tasks assigned to a specific being
    /// </summary>
    /// <param name="beingId">The ID of the being</param>
    /// <returns>Collection of tasks assigned to the being</returns>
    public IEnumerable<TaskItem> GetTasksForBeing(Guid beingId)
    {
        return _tasks.Values.Where(t => t.ExecutorGuids.Contains(beingId));
    }
    
    /// <summary>
    /// Gets tasks belonging to a specific project
    /// </summary>
    /// <param name="projectId">The ID of the project</param>
    /// <returns>Collection of tasks for the project</returns>
    public IEnumerable<TaskItem> GetTasksForProject(Guid projectId)
    {
        return _tasks.Values.Where(t => t.ProjectId == projectId);
    }
    
    /// <summary>
    /// Gets tasks that are runnable (pending and dependencies met) for a specific being
    /// </summary>
    /// <param name="beingId">The ID of the being</param>
    /// <returns>Collection of runnable tasks</returns>
    public List<TaskItem> GetRunnableTasks(Guid beingId)
    {
        var assignedTasks = GetTasksForBeing(beingId).ToList();
        var runnableTasks = new List<TaskItem>();
        
        foreach (var task in assignedTasks)
        {
            if (task.Status == TaskStatus.Pending && AreDependenciesMet(task))
            {
                runnableTasks.Add(task);
            }
        }
        
        // Sort by priority (lower values = higher priority)
        return runnableTasks.OrderBy(t => t.Priority).ToList();
    }
    
    /// <summary>
    /// Checks if all dependencies for a task are completed
    /// </summary>
    /// <param name="task">The task to check</param>
    /// <returns>True if all dependencies are met, false otherwise</returns>
    private bool AreDependenciesMet(TaskItem task)
    {
        if (task.Dependencies.Count == 0)
            return true;
            
        foreach (var depId in task.Dependencies)
        {
            var depTask = GetTask(depId);
            if (depTask == null || depTask.Status != TaskStatus.Completed)
            {
                return false;
            }
        }
        return true;
    }
    
    /// <summary>
    /// Gets tasks filtered by status
    /// </summary>
    /// <param name="status">The status to filter by</param>
    /// <returns>Collection of tasks with the specified status</returns>
    public IEnumerable<TaskItem> GetTasksByStatus(TaskStatus status)
    {
        return _tasks.Values.Where(t => t.Status == status);
    }
    
    /// <summary>
    /// Gets pending tasks for a being (shorter version for compatibility with existing code)
    /// </summary>
    /// <param name="beingId">The ID of the being</param>
    /// <returns>True if there are pending tasks, false otherwise</returns>
    public bool HasPendingTasks(Guid beingId)
    {
        return GetRunnableTasks(beingId).Count > 0;
    }
    
    /// <summary>
    /// Gets the total count of tasks in the task center
    /// </summary>
    public int TaskCount => _tasks.Count;
    
    /// <summary>
    /// Gets the count of pending tasks for a specific being
    /// </summary>
    /// <param name="beingId">The ID of the being</param>
    /// <returns>The number of pending tasks</returns>
    public int GetPendingTaskCount(Guid beingId)
    {
        return GetRunnableTasks(beingId).Count;
    }
}