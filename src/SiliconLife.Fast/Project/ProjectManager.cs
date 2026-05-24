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
using SiliconLife.Common.Localization;

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
    private const string ProjectsPrefix = "projects/";
    private readonly Dictionary<Guid, WorkNoteSystem> _workNoteSystems = new();
    private readonly Dictionary<Guid, ProjectTaskSystem> _projectTaskSystems = new();
    private WorkflowEngine? _workflowEngine;

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
    public ProjectSpace CreateProject(string name, string description, Guid createdBy, string? workflowTemplateName = null)
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
            Status = ProjectStatus.Active,
            WorkflowTemplateName = workflowTemplateName?.Trim() ?? string.Empty
        };

        // Auto-create group chat session for the project
        var chatSystem = ServiceLocator.Instance.ChatSystem;
        if (chatSystem != null)
        {
            // Get localized prefix for group chat name
            string groupChatPrefix = "Project Group";
            var language = SiliconLife.Collective.Config.Instance?.Data?.Language ?? Language.EnUS;
            if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) && loc is DefaultLocalizationBase defaultLoc)
            {
                groupChatPrefix = defaultLoc.ProjectGroupChatPrefix;
            }
            
            var groupChatSession = chatSystem.CreateGroupSession(
                new List<Guid> { createdBy }, // Initially only the creator
                $"{groupChatPrefix}：{project.Name}"
            );
            project.GroupChatSessionId = groupChatSession.Id;
        }

        // Auto-create broadcast channel for the project
        var broadcastId = Guid.NewGuid();
        if (chatSystem != null)
        {
            // Get localized prefix for broadcast channel name
            string broadcastPrefix = "Project Broadcast";
            var language = SiliconLife.Collective.Config.Instance?.Data?.Language ?? Language.EnUS;
            if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) && loc is DefaultLocalizationBase defaultLoc)
            {
                broadcastPrefix = defaultLoc.ProjectBroadcastPrefix;
            }
            
            var broadcastChannel = chatSystem.GetOrCreateBroadcastChannel(
                broadcastId,
                $"{broadcastPrefix}：{project.Name}"
            );
            project.BroadcastChannelId = broadcastChannel.Id;
        }

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

        _logger.Info(createdBy, "Created project space '{0}' (ID: {1}), workflow template: {2}, group chat: {3}, broadcast: {4}", 
            project.Name, project.Id, 
            string.IsNullOrEmpty(project.WorkflowTemplateName) ? "None" : project.WorkflowTemplateName,
            project.GroupChatSessionId,
            project.BroadcastChannelId);

        if (!string.IsNullOrEmpty(project.WorkflowTemplateName) && _workflowEngine != null)
        {
            try
            {
                _workflowEngine.CreateInstanceAsync(
                    project.Id,
                    project.WorkflowTemplateName,
                    project.Id.ToString(),
                    createdBy).GetAwaiter().GetResult();
                _logger.Info(createdBy, "Auto-created workflow instance for project '{0}' with template '{1}'",
                    project.Name, project.WorkflowTemplateName);
            }
            catch (Exception ex)
            {
                _logger.Warn(createdBy, "Failed to auto-create workflow instance for project '{0}': {1}",
                    project.Name, ex.Message);
            }
        }

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
                
                // Sync group chat session members
                SyncGroupChatMembers(project);
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
                
                // Sync group chat session members
                SyncGroupChatMembers(project);
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
        var projects = new List<ProjectSpace>();
        
        // Scan all project keys under "projects/" prefix
        var projectKeys = _storage.ListKeys(ProjectsPrefix);
        
        foreach (var key in projectKeys)
        {
            string metaKey = key + "meta.json";
            if (!_storage.Exists(metaKey))
            {
                continue;
            }
            
            // Read project data (each meta key stores one ProjectSpace)
            var projectData = _storage.Read<ProjectSpace>(metaKey);
            if (projectData != null && projectData.Length > 0)
            {
                projects.AddRange(projectData);
            }
        }
        
        return projects;
    }

    private void SaveProjectsInternal(List<ProjectSpace> projects)
    {
        // Save each project to its own meta key
        foreach (var project in projects)
        {
            string projectKey = $"{ProjectsPrefix}{project.Id}/meta";
            _storage.Write(projectKey, project);
        }
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

            // Ensure group chat session is loaded in ChatSystem
            if (project.GroupChatSessionId.HasValue)
            {
                var chatSystem = ServiceLocator.Instance.ChatSystem;
                if (chatSystem != null)
                {
                    // Try to get existing session, if not found, restore it
                    var existingSession = chatSystem.GetSession(project.GroupChatSessionId.Value);
                    if (existingSession == null)
                    {
                        // Session not in memory, need to restore from storage
                        // Read metadata from storage to get members and name
                        string metaKey = $"sessions/group/{project.GroupChatSessionId.Value}/meta";
                        var metaDicts = _storage.Read<Dictionary<string, object>>(metaKey);
                        var metaDict = metaDicts.FirstOrDefault();
                        
                        if (metaDict != null)
                        {
                            string name = metaDict.TryGetValue("Name", out var nameObj) ? nameObj?.ToString() ?? "" : "";
                            var members = new List<Guid>();
                            
                            if (metaDict.TryGetValue("Members", out var membersObj) && membersObj is List<object> memberList)
                            {
                                foreach (var m in memberList)
                                {
                                    if (Guid.TryParse(m?.ToString(), out Guid memberId))
                                        members.Add(memberId);
                                }
                            }
                            
                            // Restore session with fixed ID
                            chatSystem.CreateGroupSession(members, name, project.GroupChatSessionId.Value);
                            _logger.Info(null, "Restored group chat session for project {0}: {1}", projectId, project.GroupChatSessionId.Value);
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(project.StoragePath))
            {
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

            // Ensure group chat session is loaded in ChatSystem
            if (project.GroupChatSessionId.HasValue)
            {
                var chatSystem = ServiceLocator.Instance.ChatSystem;
                if (chatSystem != null)
                {
                    // Try to get existing session, if not found, restore it
                    var existingSession = chatSystem.GetSession(project.GroupChatSessionId.Value);
                    if (existingSession == null)
                    {
                        // Session not in memory, need to restore from storage
                        // Read metadata from storage to get members and name
                        string metaKey = $"sessions/group/{project.GroupChatSessionId.Value}/meta";
                        var metaDicts = _storage.Read<Dictionary<string, object>>(metaKey);
                        var metaDict = metaDicts.FirstOrDefault();
                        
                        if (metaDict != null)
                        {
                            string name = metaDict.TryGetValue("Name", out var nameObj) ? nameObj?.ToString() ?? "" : "";
                            var members = new List<Guid>();
                            
                            if (metaDict.TryGetValue("Members", out var membersObj) && membersObj is List<object> memberList)
                            {
                                foreach (var m in memberList)
                                {
                                    if (Guid.TryParse(m?.ToString(), out Guid memberId))
                                        members.Add(memberId);
                                }
                            }
                            
                            // Restore session with fixed ID
                            chatSystem.CreateGroupSession(members, name, project.GroupChatSessionId.Value);
                            _logger.Info(null, "Restored group chat session for project {0}: {1}", projectId, project.GroupChatSessionId.Value);
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(project.StoragePath))
            {
                SaveProjectsInternal(projects);
            }

            var system = new ProjectTaskSystem(project.Id);
            _projectTaskSystems[projectId] = system;
            return system;
        }
    }

    /// <inheritdoc/>
    public bool AssignRole(Guid projectId, string roleName, Guid beingId)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return false;

        roleName = roleName.Trim();

        lock (_lock)
        {
            var projects = LoadProjectsInternal();
            var project = projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
                return false;

            // Being must be assigned to the project first
            if (!project.AssignedBeings.Contains(beingId))
                return false;

            if (!project.RoleAssignments.TryGetValue(roleName, out var beings))
            {
                beings = new List<Guid>();
                project.RoleAssignments[roleName] = beings;
            }

            if (!beings.Contains(beingId))
            {
                beings.Add(beingId);
                project.UpdatedAt = DateTime.UtcNow;
                SaveProjectsInternal(projects);
                _logger.Info(beingId, "Assigned being {0} to role '{1}' in project {2}", beingId, roleName, projectId);
            }

            return true;
        }
    }

    /// <inheritdoc/>
    public bool RemoveRole(Guid projectId, string roleName, Guid beingId)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return false;

        roleName = roleName.Trim();

        lock (_lock)
        {
            var projects = LoadProjectsInternal();
            var project = projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
                return false;

            if (project.RoleAssignments.TryGetValue(roleName, out var beings) && beings.Remove(beingId))
            {
                // Clean up empty role lists
                if (beings.Count == 0)
                    project.RoleAssignments.Remove(roleName);

                project.UpdatedAt = DateTime.UtcNow;
                SaveProjectsInternal(projects);
                _logger.Info(beingId, "Removed being {0} from role '{1}' in project {2}", beingId, roleName, projectId);
                return true;
            }

            return false;
        }
    }

    /// <inheritdoc/>
    public WorkflowEngine? GetWorkflowEngine()
    {
        return _workflowEngine;
    }

    /// <summary>
    /// Sets the workflow engine for this project manager.
    /// Called during initialization to register the engine.
    /// </summary>
    /// <param name="engine">The workflow engine to set</param>
    public void SetWorkflowEngine(WorkflowEngine engine)
    {
        _workflowEngine = engine ?? throw new ArgumentNullException(nameof(engine));
        _logger.Info(null, "Workflow engine registered with ProjectManager");
    }

    /// <summary>
    /// Synchronize group chat session members with project's assigned beings.
    /// Uses full sync strategy: removes members not in project, adds missing members.
    /// This ensures data consistency even if some operations were missed.
    /// </summary>
    private void SyncGroupChatMembers(ProjectSpace project)
    {
        if (!project.GroupChatSessionId.HasValue)
        {
            return;
        }

        var chatSystem = ServiceLocator.Instance.ChatSystem;
        if (chatSystem == null)
        {
            return;
        }

        var session = chatSystem.GetSession(project.GroupChatSessionId.Value) as GroupChatSession;
        if (session == null)
        {
            _logger.Warn(null, "Group chat session {0} not found for project {1}, cannot sync members", 
                project.GroupChatSessionId.Value, project.Id);
            return;
        }

        // Get current members from both sources
        var currentMembers = session.Members.ToList();
        var expectedMembers = project.AssignedBeings.ToList();

        // Find members to remove (in session but not in project)
        var membersToRemove = currentMembers.Where(m => !expectedMembers.Contains(m)).ToList();
        
        // Find members to add (in project but not in session)
        var membersToAdd = expectedMembers.Where(m => !currentMembers.Contains(m)).ToList();

        // Remove extra members
        foreach (var memberId in membersToRemove)
        {
            session.RemoveMember(memberId);
            _logger.Info(null, "Removed being {0} from group chat for project {1} (not in project)", 
                memberId, project.Id);
        }

        // Add missing members
        foreach (var memberId in membersToAdd)
        {
            session.AddMember(memberId);
            _logger.Info(null, "Added being {0} to group chat for project {1} (was missing)", 
                memberId, project.Id);
        }

        if (membersToRemove.Count > 0 || membersToAdd.Count > 0)
        {
            _logger.Info(null, "Synced group chat for project {0}: removed {1}, added {2}, total members: {3}",
                project.Id, membersToRemove.Count, membersToAdd.Count, session.Members.Count);
        }
    }
}
