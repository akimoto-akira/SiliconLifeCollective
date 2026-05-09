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
/// Task query component - provides filtered access to tasks from TaskCenter.
/// Each Being/Project has its own queryer for customized task enumeration.
/// </summary>
public class TaskEnumerator
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<TaskEnumerator>();
    private readonly Guid _ownerId;
    private readonly TaskEnumeratorType _type;
    private readonly TaskCenter _taskCenter;
    
    /// <summary>
    /// Creates a new TaskEnumerator for a silicon being
    /// </summary>
    /// <param name="beingId">The ID of the silicon being that owns this enumerator</param>
    public TaskEnumerator(Guid beingId)
    {
        _ownerId = beingId;
        _type = TaskEnumeratorType.Being;
        _taskCenter = TaskCenter.Instance;
        _logger.Debug(_ownerId, "TaskEnumerator created for being");
    }
    
    /// <summary>
    /// Creates a new TaskEnumerator for a project
    /// </summary>
    /// <param name="projectId">The ID of the project that owns this enumerator</param>
    public TaskEnumerator(Guid projectId, bool isProject)
    {
        _ownerId = projectId;
        _type = TaskEnumeratorType.Project;
        _taskCenter = TaskCenter.Instance;
        _logger.Debug(_ownerId, "TaskEnumerator created for project");
    }
    
    /// <summary>
    /// Enumerates all tasks accessible to the owner
    /// </summary>
    /// <returns>Collection of accessible tasks</returns>
    public IEnumerable<TaskItem> EnumerateAll()
    {
        switch (_type)
        {
            case TaskEnumeratorType.Being:
                return _taskCenter.GetTasksForBeing(_ownerId);
            case TaskEnumeratorType.Project:
                return _taskCenter.GetTasksForProject(_ownerId);
            default:
                return Enumerable.Empty<TaskItem>();
        }
    }
    
    /// <summary>
    /// Enumerates runnable tasks (pending + dependencies met) for the owner
    /// </summary>
    /// <returns>Collection of runnable tasks</returns>
    public IEnumerable<TaskItem> EnumerateRunnable()
    {
        if (_type == TaskEnumeratorType.Being)
        {
            return _taskCenter.GetRunnableTasks(_ownerId);
        }
        else if (_type == TaskEnumeratorType.Project)
        {
            var projectTasks = _taskCenter.GetTasksForProject(_ownerId);
            return projectTasks.Where(t => t.Status == TaskStatus.Pending && AreDependenciesMet(t));
        }
        return Enumerable.Empty<TaskItem>();
    }
    
    /// <summary>
    /// Enumerates tasks by priority level
    /// </summary>
    /// <param name="maxPriority">Maximum priority level (lower values = higher priority)</param>
    /// <returns>Collection of tasks at or above the specified priority</returns>
    public IEnumerable<TaskItem> EnumerateByPriority(int maxPriority = 100)
    {
        return EnumerateAll().Where(t => t.Priority <= maxPriority)
                             .OrderBy(t => t.Priority);
    }
    
    /// <summary>
    /// Enumerates tasks by status
    /// </summary>
    /// <param name="status">The status to filter by</param>
    /// <returns>Collection of tasks with the specified status</returns>
    public IEnumerable<TaskItem> EnumerateByStatus(TaskStatus status)
    {
        return EnumerateAll().Where(t => t.Status == status);
    }
    
    /// <summary>
    /// Enumerates pending tasks for the owner
    /// </summary>
    /// <returns>Collection of pending tasks</returns>
    public IEnumerable<TaskItem> EnumeratePending()
    {
        return EnumerateByStatus(TaskStatus.Pending);
    }
    
    /// <summary>
    /// Enumerates completed tasks for the owner
    /// </summary>
    /// <returns>Collection of completed tasks</returns>
    public IEnumerable<TaskItem> EnumerateCompleted()
    {
        return EnumerateByStatus(TaskStatus.Completed);
    }
    
    /// <summary>
    /// Enumerates failed tasks for the owner
    /// </summary>
    /// <returns>Collection of failed tasks</returns>
    public IEnumerable<TaskItem> EnumerateFailed()
    {
        return EnumerateByStatus(TaskStatus.Failed);
    }
    
    /// <summary>
    /// Enumerates tasks that require specific tools
    /// </summary>
    /// <param name="toolName">The name of the required tool</param>
    /// <returns>Collection of tasks requiring the specified tool</returns>
    public IEnumerable<TaskItem> EnumerateTasksRequiringTool(string toolName)
    {
        return EnumerateAll().Where(t => t.RequiredTools.Contains(toolName, StringComparer.OrdinalIgnoreCase));
    }
    
    /// <summary>
    /// Enumerates tasks by assignee
    /// </summary>
    /// <param name="assigneeId">The ID of the assignee</param>
    /// <returns>Collection of tasks assigned by the specified assignee</returns>
    public IEnumerable<TaskItem> EnumerateByAssignee(Guid assigneeId)
    {
        return EnumerateAll().Where(t => t.AssigneeGuid == assigneeId);
    }
    
    /// <summary>
    /// Enumerates tasks by creation date range
    /// </summary>
    /// <param name="startDate">Start date (inclusive)</param>
    /// <param name="endDate">End date (inclusive)</param>
    /// <returns>Collection of tasks created within the date range</returns>
    public IEnumerable<TaskItem> EnumerateByDateRange(DateTime startDate, DateTime endDate)
    {
        return EnumerateAll().Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
                             .OrderBy(t => t.CreatedAt);
    }
    
    /// <summary>
    /// Searches tasks by title or description containing the specified text
    /// </summary>
    /// <param name="searchText">The text to search for</param>
    /// <returns>Collection of tasks matching the search criteria</returns>
    public IEnumerable<TaskItem> Search(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return Enumerable.Empty<TaskItem>();
            
        var searchLower = searchText.ToLowerInvariant();
        return EnumerateAll().Where(t => 
            (t.Title?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (t.Description?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false)
        );
    }
    
    /// <summary>
    /// Gets the count of tasks accessible to the owner
    /// </summary>
    /// <returns>The total number of accessible tasks</returns>
    public int GetTaskCount()
    {
        return EnumerateAll().Count();
    }
    
    /// <summary>
    /// Gets the count of pending tasks for the owner
    /// </summary>
    /// <returns>The number of pending tasks</returns>
    public int GetPendingTaskCount()
    {
        return EnumeratePending().Count();
    }
    
    /// <summary>
    /// Gets the count of runnable tasks for the owner
    /// </summary>
    /// <returns>The number of runnable tasks</returns>
    public int GetRunnableTaskCount()
    {
        return EnumerateRunnable().Count();
    }
    
    /// <summary>
    /// Checks if the owner has any pending tasks
    /// </summary>
    /// <returns>True if there are pending tasks, false otherwise</returns>
    public bool HasPendingTasks()
    {
        return GetPendingTaskCount() > 0;
    }
    
    /// <summary>
    /// Checks if the owner has any runnable tasks
    /// </summary>
    /// <returns>True if there are runnable tasks, false otherwise</returns>
    public bool HasRunnableTasks()
    {
        return GetRunnableTaskCount() > 0;
    }
    
    /// <summary>
    /// Gets high priority tasks (priority <= 50) for the owner
    /// </summary>
    /// <returns>Collection of high priority tasks</returns>
    public IEnumerable<TaskItem> GetHighPriorityTasks()
    {
        return EnumerateByPriority(50);
    }
    
    /// <summary>
    /// Gets overdue tasks (created more than 7 days ago and still pending)
    /// </summary>
    /// <returns>Collection of overdue tasks</returns>
    public IEnumerable<TaskItem> GetOverdueTasks()
    {
        var sevenDaysAgo = DateTime.Now.AddDays(-7);
        return EnumeratePending().Where(t => t.CreatedAt <= sevenDaysAgo);
    }
    
    /// <summary>
    /// Gets a summary of task statistics for the owner
    /// </summary>
    /// <returns>Task statistics summary</returns>
    public global::SiliconLife.Collective.TaskStatistics GetStatistics()
    {
        var allTasks = EnumerateAll().ToList();
        return new global::SiliconLife.Collective.TaskStatistics
        {
            Total = allTasks.Count,
            Pending = allTasks.Count(t => t.Status == TaskStatus.Pending),
            Running = allTasks.Count(t => t.Status == TaskStatus.Running),
            Completed = allTasks.Count(t => t.Status == TaskStatus.Completed),
            Failed = allTasks.Count(t => t.Status == TaskStatus.Failed),
            Cancelled = allTasks.Count(t => t.Status == TaskStatus.Cancelled)
        };
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
            var depTask = _taskCenter.GetTask(depId);
            if (depTask == null || depTask.Status != TaskStatus.Completed)
            {
                return false;
            }
        }
        return true;
    }
}

/// <summary>
/// Represents the type of entity that owns a TaskEnumerator
/// </summary>
public enum TaskEnumeratorType
{
    /// <summary>Enumerator for a silicon being</summary>
    Being,
    
    /// <summary>Enumerator for a project</summary>
    Project
}
