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

    private readonly ConcurrentDictionary<Guid, TaskItem> _loadedTasks = new();
    private readonly ConcurrentDictionary<Guid, byte> _knownTaskIds = new();
    private IStorage? _storage;
    private bool _initialized;
    private const string TaskDirectory = "tasks";

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
        if (_initialized) return;

        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        ScanTaskDirectory();
        _initialized = true;
    }

    private void ScanTaskDirectory()
    {
        if (_storage == null) return;

        try
        {
            foreach (var key in _storage.ListKeys(TaskDirectory))
            {
                if (key.EndsWith("/"))
                    continue;

                string fileName = Path.GetFileNameWithoutExtension(key);
                if (Guid.TryParse(fileName, out Guid taskId))
                {
                    _knownTaskIds.TryAdd(taskId, 0);
                }
            }

            _logger.Info(null, "TaskCenter scanned {0} task(s) from storage", _knownTaskIds.Count);
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to scan task directory", ex);
        }
    }

    private static string GetTaskKey(Guid taskId) => $"{TaskDirectory}/{taskId}.json";

    private TaskItem? LoadTask(Guid taskId)
    {
        if (_storage == null) return null;

        try
        {
            var result = _storage.Read<TaskItem>(GetTaskKey(taskId)).FirstOrDefault();
            if (result != null)
            {
                _loadedTasks.TryAdd(taskId, result);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to load task {0} from storage", taskId, ex);
            return null;
        }
    }

    private void EnsureAllTasksLoaded()
    {
        foreach (var taskId in _knownTaskIds.Keys)
        {
            if (!_loadedTasks.ContainsKey(taskId))
            {
                LoadTask(taskId);
            }
        }
    }

    private void SaveTask(TaskItem task)
    {
        if (_storage == null) return;

        try
        {
            _storage.Write(GetTaskKey(task.Id), task);
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to save task {0} to storage", task.Id, ex);
        }
    }

    private void DeleteTaskFile(Guid taskId)
    {
        if (_storage == null) return;

        try
        {
            _storage.Delete(GetTaskKey(taskId));
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to delete task {0} from storage", taskId, ex);
        }
    }

    public void Save()
    {
    }

    public bool AddTask(TaskItem task)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task));

        bool added = _knownTaskIds.TryAdd(task.Id, 0);
        if (added)
        {
            _loadedTasks[task.Id] = task;
            SaveTask(task);
            _logger.Info(null, "Task added to TaskCenter: {0} (ID: {1})", task.Title, task.Id);
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

        if (!_knownTaskIds.ContainsKey(task.Id))
        {
            _logger.Warn(null, "Task not found for update: {0} (ID: {1})", task.Title, task.Id);
            return false;
        }

        _loadedTasks[task.Id] = task;
        SaveTask(task);
        _logger.Debug(null, "Task updated in TaskCenter: {0} (ID: {1})", task.Title, task.Id);
        return true;
    }

    public bool RemoveTask(Guid taskId)
    {
        bool removed = _knownTaskIds.TryRemove(taskId, out _);
        if (removed)
        {
            _loadedTasks.TryRemove(taskId, out TaskItem? removedTask);
            DeleteTaskFile(taskId);
            _logger.Info(null, "Task removed from TaskCenter: {0} (ID: {1})", removedTask?.Title ?? "Unknown", taskId);
        }
        else
        {
            _logger.Warn(null, "Task not found for removal (ID: {0})", taskId);
        }

        return removed;
    }

    public TaskItem? GetTask(Guid taskId)
    {
        if (_loadedTasks.TryGetValue(taskId, out TaskItem? task))
            return task;

        if (!_knownTaskIds.ContainsKey(taskId))
            return null;

        return LoadTask(taskId);
    }

    public IEnumerable<TaskItem> GetAllTasks()
    {
        EnsureAllTasksLoaded();
        return _loadedTasks.Values;
    }

    public IEnumerable<TaskItem> GetTasksForBeing(Guid beingId)
    {
        return GetAllTasks().Where(t => t.ExecutorGuid == beingId);
    }

    public IEnumerable<TaskItem> GetTasksForProject(Guid projectId)
    {
        return GetAllTasks().Where(t => t.ProjectId == projectId);
    }

    public IEnumerable<TaskItem> GetPersonalTasks(Guid beingId)
    {
        return GetAllTasks().Where(t => t.ProjectId == null && t.ExecutorGuid == beingId);
    }

    public IEnumerable<TaskItem> GetProjectTasks(Guid projectId)
    {
        return GetAllTasks().Where(t => t.ProjectId == projectId);
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
        return GetAllTasks().Where(t =>
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
        return GetAllTasks().Where(t => t.Status == status);
    }

    public bool HasPendingTasks(Guid beingId)
    {
        return GetRunnableTasks(beingId).Count > 0;
    }

    public int TaskCount => _knownTaskIds.Count;

    public int GetPendingTaskCount(Guid beingId)
    {
        return GetRunnableTasks(beingId).Count;
    }
}
