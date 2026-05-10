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

public class TaskCenter
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<TaskCenter>();
    private static TaskCenter? _instance;
    private static readonly object _lock = new();

    private readonly ConcurrentDictionary<Guid, TaskItem> _tasks = new();
    private IStorage? _storage;
    private const string StorageKey = "tasks";

    private TaskCenter() {}

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

    public void Initialize(IStorage storage)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        LoadAll();
    }

    private void LoadAll()
    {
        if (_storage == null) return;

        try
        {
            TaskItem[] tasks = _storage.Read<TaskItem>(StorageKey);
            if (tasks != null)
            {
                foreach (var task in tasks)
                {
                    _tasks.TryAdd(task.Id, task);
                }
                _logger.Info(null, "TaskCenter loaded {0} task(s) from storage", tasks.Length);
            }
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to load tasks from storage", ex);
        }
    }

    public void Save()
    {
        if (_storage == null) return;

        try
        {
            var tasks = _tasks.Values.ToList();
            _storage.Write(StorageKey, tasks);
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to save tasks to storage", ex);
        }
    }

    public bool AddTask(TaskItem task)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task));

        bool added = _tasks.TryAdd(task.Id, task);
        if (added)
        {
            _logger.Info(null, "Task added to TaskCenter: {0} (ID: {1})", task.Title, task.Id);
            Save();
        }
        else
        {
            _logger.Warn(null, "Task already exists in TaskCenter: {0} (ID: {1})", task.Title, task.Id);
        }
        return added;
    }

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
        Save();
        return true;
    }

    public bool RemoveTask(Guid taskId)
    {
        bool removed = _tasks.TryRemove(taskId, out TaskItem? removedTask);
        if (removed)
        {
            _logger.Info(null, "Task removed from TaskCenter: {0} (ID: {1})", removedTask?.Title ?? "Unknown", taskId);
            Save();
        }
        else
        {
            _logger.Warn(null, "Task not found for removal (ID: {0})", taskId);
        }
        return removed;
    }

    public TaskItem? GetTask(Guid taskId)
    {
        _tasks.TryGetValue(taskId, out TaskItem? task);
        return task;
    }

    public IEnumerable<TaskItem> GetAllTasks()
    {
        return _tasks.Values;
    }

    public IEnumerable<TaskItem> GetTasksForBeing(Guid beingId)
    {
        return _tasks.Values.Where(t => t.ExecutorGuid == beingId);
    }

    public IEnumerable<TaskItem> GetTasksForProject(Guid projectId)
    {
        return _tasks.Values.Where(t => t.ProjectId == projectId);
    }

    public IEnumerable<TaskItem> GetPersonalTasks(Guid beingId)
    {
        return _tasks.Values.Where(t => t.ProjectId == null && t.ExecutorGuid == beingId);
    }

    public IEnumerable<TaskItem> GetProjectTasks(Guid projectId)
    {
        return _tasks.Values.Where(t => t.ProjectId == projectId);
    }

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

        return runnableTasks.OrderBy(t => t.Priority).ToList();
    }

    public List<TaskItem> GetContinuationTasks(Guid beingId)
    {
        return _tasks.Values.Where(t =>
            t.ExecutorGuid == beingId &&
            t.Status == TaskStatus.Running &&
            t.ChatHistory.Count > 0 &&
            t.ChatHistory[^1].Messages.Count > 0 &&
            t.ChatHistory[^1].EndStatus == null).ToList();
    }

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

    public IEnumerable<TaskItem> GetTasksByStatus(TaskStatus status)
    {
        return _tasks.Values.Where(t => t.Status == status);
    }

    public bool HasPendingTasks(Guid beingId)
    {
        return GetRunnableTasks(beingId).Count > 0;
    }

    public int TaskCount => _tasks.Count;

    public int GetPendingTaskCount(Guid beingId)
    {
        return GetRunnableTasks(beingId).Count;
    }
}
