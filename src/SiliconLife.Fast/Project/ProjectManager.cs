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
using SiliconLife.Fast;

namespace SiliconLife.Fast;

/// <summary>
/// Default implementation of IProjectManager.
/// Manages project spaces using file system storage.
/// </summary>
public class ProjectManager : IProjectManager
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<ProjectManager>();
    private readonly IStorage _storage;
    private readonly string _baseDirectory;
    private readonly object _lock = new();
    private const string ProjectsKey = "projects/index";
    private readonly Dictionary<Guid, WorkNoteSystem> _workNoteSystems = new();
    private readonly Dictionary<Guid, ProjectTaskSystem> _projectTaskSystems = new();

    /// <inheritdoc/>
    public int ActiveProjectCount => ListProjects(includeArchived: false).Count;

    /// <inheritdoc/>
    public int ArchivedProjectCount => ListProjects(includeArchived: true).Count(p => p.Status == ProjectStatus.Archived);

    /// <summary>
    /// Initializes a new instance of the ProjectManager class
    /// </summary>
    /// <param name="storage">The storage implementation to use</param>
    /// <param name="baseDirectory">The base data directory for project storage</param>
    public ProjectManager(IStorage storage, string baseDirectory)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _baseDirectory = baseDirectory ?? throw new ArgumentNullException(nameof(baseDirectory));
    }

    /// <inheritdoc/>
    public ProjectSpace CreateProject(string name, string description, Guid createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Project name cannot be empty", nameof(name));
        }

        var project = new ProjectSpace
        {
            Name = name.Trim(),
            Description = description?.Trim() ?? string.Empty,
            CreatedBy = createdBy,
            Status = ProjectStatus.Active
        };

        // Store project metadata in SpeedyPack
        string projectKey = $"projects/{project.Id}";
        SpeedyPackRegistry.Pack.Write(projectKey, project);

        // Initialize work note system for the project
        var workNoteStorage = new SpeedyWorkNoteStorage();
        var workNoteSystem = new WorkNoteSystem(workNoteStorage, project.Id, WorkNoteOwnerType.Project);
        lock (_lock)
        {
            _workNoteSystems[project.Id] = workNoteSystem;
            var projects = LoadProjectsInternal();
            projects.Add(project);
            SaveProjectsInternal(projects);
        }

        _logger.Info(createdBy, "Created project space '{0}' (ID: {1})", project.Name, project.Id);
        return project;
    }

    /// <inheritdoc/>
    public bool ArchiveProject(Guid projectId)
    {
        lock (_lock)
        {
            var projects = LoadProjectsInternal();
            var project = projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null || project.Status != ProjectStatus.Active)
            {
                return false;
            }

            project.Status = ProjectStatus.Archived;
            project.ArchivedAt = DateTime.UtcNow;
            project.UpdatedAt = DateTime.UtcNow;

            SaveProjectsInternal(projects);
        }

        _logger.Info(null, "Archived project space {0}", projectId);
        return true;
    }

    /// <inheritdoc/>
    public bool RestoreProject(Guid projectId)
    {
        lock (_lock)
        {
            var projects = LoadProjectsInternal();
            var project = projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null || project.Status != ProjectStatus.Archived)
            {
                return false;
            }

            project.Status = ProjectStatus.Active;
            project.ArchivedAt = null;
            project.UpdatedAt = DateTime.UtcNow;

            SaveProjectsInternal(projects);
        }

        _logger.Info(null, "Restored project space {0}", projectId);
        return true;
    }

    /// <inheritdoc/>
    public bool DestroyProject(Guid projectId)
    {
        lock (_lock)
        {
            var projects = LoadProjectsInternal();
            var project = projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return false;
            }

            project.Status = ProjectStatus.Destroyed;
            project.UpdatedAt = DateTime.UtcNow;

            // Clean up project metadata from SpeedyPack
            string projectKey = $"projects/{project.Id}";
            SpeedyPackRegistry.Pack.Delete(projectKey);

            _workNoteSystems.Remove(projectId);
            _projectTaskSystems.Remove(projectId);
            SaveProjectsInternal(projects);
        }

        _logger.Info(null, "Destroyed project space {0}", projectId);
        return true;
    }

    /// <inheritdoc/>
    public ProjectSpace? GetProject(Guid projectId)
    {
        lock (_lock)
        {
            var projects = LoadProjectsInternal();
            return projects.FirstOrDefault(p => p.Id == projectId);
        }
    }

    /// <inheritdoc/>
    public List<ProjectSpace> ListProjects(bool includeArchived = false)
    {
        lock (_lock)
        {
            var projects = LoadProjectsInternal();
            if (includeArchived)
            {
                return projects.Where(p => p.Status != ProjectStatus.Destroyed).ToList();
            }
            return projects.Where(p => p.Status == ProjectStatus.Active).ToList();
        }
    }

    /// <inheritdoc/>
    public bool AssignBeing(Guid projectId, Guid beingId)
    {
        lock (_lock)
        {
            var projects = LoadProjectsInternal();
            var project = projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return false;
            }

            if (!project.AssignedBeings.Contains(beingId))
            {
                project.AssignedBeings.Add(beingId);
                project.UpdatedAt = DateTime.UtcNow;
                SaveProjectsInternal(projects);
            }

            return true;
        }
    }

    /// <inheritdoc/>
    public bool RemoveBeing(Guid projectId, Guid beingId)
    {
        lock (_lock)
        {
            var projects = LoadProjectsInternal();
            var project = projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return false;
            }

            if (project.AssignedBeings.Remove(beingId))
            {
                project.UpdatedAt = DateTime.UtcNow;
                SaveProjectsInternal(projects);
            }

            return true;
        }
    }

    /// <inheritdoc/>
    public bool IsBeingAssigned(Guid projectId, Guid beingId)
    {
        lock (_lock)
        {
            var projects = LoadProjectsInternal();
            var project = projects.FirstOrDefault(p => p.Id == projectId);
            return project != null && project.AssignedBeings.Contains(beingId);
        }
    }

    /// <inheritdoc/>
    public ProjectSpace? UpdateProject(Guid projectId, string? name = null, string? description = null)
    {
        lock (_lock)
        {
            var projects = LoadProjectsInternal();
            var project = projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null || project.Status != ProjectStatus.Active)
            {
                return null;
            }

            if (name != null)
            {
                project.Name = name.Trim();
            }

            if (description != null)
            {
                project.Description = description.Trim();
            }

            project.UpdatedAt = DateTime.UtcNow;
            SaveProjectsInternal(projects);

            return project;
        }
    }

    private List<ProjectSpace> LoadProjectsInternal()
    {
        var projects = _storage.Read<List<ProjectSpace>>(ProjectsKey);
        return projects ?? new List<ProjectSpace>();
    }

    private void SaveProjectsInternal(List<ProjectSpace> projects)
    {
        _storage.Write(ProjectsKey, projects);
    }

    /// <inheritdoc/>
    public WorkNoteSystem? GetWorkNoteSystem(Guid projectId)
    {
        lock (_lock)
        {
            // Return cached instance if available
            if (_workNoteSystems.TryGetValue(projectId, out var cached))
            {
                return cached;
            }

            // Try to load from existing project
            var projects = LoadProjectsInternal();
            var project = projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null || project.Status == ProjectStatus.Destroyed)
            {
                return null;
            }

            if (string.IsNullOrEmpty(project.StoragePath))
            {
                // Legacy project without storage path, store metadata in SpeedyPack
                string projectKey = $"projects/{project.Id}";
                SpeedyPackRegistry.Pack.Write(projectKey, project);
                SaveProjectsInternal(projects);
            }

            var storage = new SpeedyWorkNoteStorage();
            var system = new WorkNoteSystem(storage, project.Id, WorkNoteOwnerType.Project);
            _workNoteSystems[projectId] = system;
            return system;
        }
    }

    /// <inheritdoc/>
    public ProjectTaskSystem? GetTaskSystem(Guid projectId)
    {
        lock (_lock)
        {
            // Return cached instance if available
            if (_projectTaskSystems.TryGetValue(projectId, out var cached))
            {
                return cached;
            }

            // Try to load from existing project
            var projects = LoadProjectsInternal();
            var project = projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null || project.Status == ProjectStatus.Destroyed)
            {
                return null;
            }

            if (string.IsNullOrEmpty(project.StoragePath))
            {
                // Legacy project without storage path, store metadata in SpeedyPack
                string projectKey = $"projects/{project.Id}";
                SpeedyPackRegistry.Pack.Write(projectKey, project);
                SaveProjectsInternal(projects);
            }

            var storage = new SpeedyStorage();
            var system = new ProjectTaskSystem(project.Id, storage);
            _projectTaskSystems[projectId] = system;
            return system;
        }
    }
}
