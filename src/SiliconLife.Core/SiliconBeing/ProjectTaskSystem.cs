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

public sealed class ProjectTaskSystem
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<ProjectTaskSystem>();
    private readonly Guid _projectId;

    public Guid ProjectId => _projectId;
    public int Count => TaskCenter.Instance.GetProjectTasks(_projectId).Count();
    public int PendingCount => TaskCenter.Instance.GetProjectTasks(_projectId).Count(t => t.Status == TaskStatus.Pending);
    public int RunningCount => TaskCenter.Instance.GetProjectTasks(_projectId).Count(t => t.Status == TaskStatus.Running);

    public ProjectTaskSystem(Guid projectId, IStorage storage)
    {
        _projectId = projectId;

        if (storage != null)
        {
            TaskCenter.Instance.Initialize(storage);
        }

        _logger.Info(null, "ProjectTaskSystem created for project {0}", projectId);
    }

    public TaskItem Create(string title, string description, Guid assigneeGuid, Guid executorGuid, Guid? reviewerGuid = null, int priority = 100, List<Guid>? dependencies = null)
    {
        var task = new TaskItem(title, description)
        {
            ProjectId = _projectId,
            AssigneeGuid = assigneeGuid,
            ExecutorGuid = executorGuid,
            ReviewerGuid = reviewerGuid,
            CreatedByGuid = assigneeGuid,
            Priority = priority
        };

        if (dependencies != null)
        {
            task.Dependencies = dependencies.ToList();
        }

        task.ChatHistory.Add(new ChatHistoryCycle(TaskStatus.Pending));

        TaskCenter.Instance.AddTask(task);

        _logger.Info(null, "Project task added: {0} ({1}), project={2}, priority={3}, executor={4}",
            title, task.Id, _projectId, priority, executorGuid);

        return task;
    }

    public TaskItem? Get(Guid taskId)
    {
        return TaskCenter.Instance.GetTask(taskId);
    }

    public List<TaskItem> GetAll(TaskStatus? status = null)
    {
        var tasks = TaskCenter.Instance.GetProjectTasks(_projectId);

        if (status == null)
            return tasks.ToList();

        return tasks.Where(t => t.Status == status).ToList();
    }

    public List<TaskItem> GetPending()
    {
        return TaskCenter.Instance.GetProjectTasks(_projectId)
            .Where(t => t.Status == TaskStatus.Pending)
            .OrderBy(t => t.Priority)
            .ThenBy(t => t.CreatedAt)
            .ToList();
    }

    public bool Update(Guid taskId, string? title = null, string? description = null, int? priority = null)
    {
        var task = TaskCenter.Instance.GetTask(taskId);
        if (task == null)
            return false;

        if (title != null)
            task.Title = title;

        if (description != null)
            task.Description = description;

        if (priority.HasValue)
            task.Priority = priority.Value;

        TaskCenter.Instance.UpdateTask(task);
        _logger.Info(null, "Project task updated: {0} ({1})", task.Title, taskId);
        return true;
    }

    public bool Assign(Guid taskId, Guid beingGuid)
    {
        var task = TaskCenter.Instance.GetTask(taskId);
        if (task == null)
            return false;

        task.ExecutorGuid = beingGuid;
        TaskCenter.Instance.UpdateTask(task);
        _logger.Info(null, "Assigned executor {0} to project task {1}", beingGuid, taskId);
        return true;
    }

    public bool RemoveAssignee(Guid taskId, Guid beingGuid)
    {
        var task = TaskCenter.Instance.GetTask(taskId);
        if (task == null)
            return false;

        if (task.ExecutorGuid == beingGuid)
        {
            task.ExecutorGuid = Guid.Empty;
            TaskCenter.Instance.UpdateTask(task);
            _logger.Info(null, "Removed executor {0} from project task {1}", beingGuid, taskId);
        }

        return true;
    }

    public bool Start(Guid taskId)
    {
        var task = TaskCenter.Instance.GetTask(taskId);
        if (task == null || task.Status != TaskStatus.Pending)
            return false;

        task.Start();
        TaskCenter.Instance.UpdateTask(task);
        _logger.Info(null, "Project task started: {0} ({1})", task.Title, taskId);
        return true;
    }

    public bool Complete(Guid taskId)
    {
        var task = TaskCenter.Instance.GetTask(taskId);
        if (task == null || task.Status != TaskStatus.Running)
            return false;

        task.Complete();
        TaskCenter.Instance.UpdateTask(task);
        _logger.Info(null, "Project task completed: {0} ({1})", task.Title, taskId);
        return true;
    }

    public bool Fail(Guid taskId, string error)
    {
        var task = TaskCenter.Instance.GetTask(taskId);
        if (task == null || task.Status != TaskStatus.Running)
            return false;

        task.Fail(error);
        TaskCenter.Instance.UpdateTask(task);
        _logger.Info(null, "Project task failed: {0} ({1}), error={2}", task.Title, taskId, error);
        return true;
    }

    public bool Cancel(Guid taskId)
    {
        var task = TaskCenter.Instance.GetTask(taskId);
        if (task == null || task.Status != TaskStatus.Pending)
            return false;

        task.Cancel();
        TaskCenter.Instance.UpdateTask(task);
        _logger.Info(null, "Project task cancelled: {0} ({1})", task.Title, taskId);
        return true;
    }

    public bool Delete(Guid taskId)
    {
        var task = TaskCenter.Instance.GetTask(taskId);
        if (task == null)
            return false;

        TaskCenter.Instance.RemoveTask(taskId);
        _logger.Info(null, "Project task deleted: {0} ({1})", task.Title, taskId);
        return true;
    }

    public TaskStatistics GetStatistics()
    {
        var tasks = TaskCenter.Instance.GetProjectTasks(_projectId).ToList();
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
}
