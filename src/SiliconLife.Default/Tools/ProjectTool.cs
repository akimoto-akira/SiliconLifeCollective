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

namespace SiliconLife.Default;

/// <summary>
/// Tool for managing project spaces. Curator-only.
/// Supports creating, archiving, restoring, destroying, listing, and updating projects,
/// as well as assigning/removing silicon beings from projects.
/// </summary>
[SiliconManagerOnly]
public class ProjectTool : ITool
{
    /// <inheritdoc/>
    public string Name => "project";

    /// <inheritdoc/>
    public string Description =>
        "Manage project spaces. Only curators can use this tool. " +
        "Actions: 'create' (create a new project space), 'archive' (archive a project), " +
        "'restore' (restore an archived project), 'destroy' (destroy a project and clean up data), " +
        "'list' (list all projects), 'get' (get project details), " +
        "'assign' (assign a silicon being to a project), 'remove' (remove a being from a project), " +
        "'update' (update project name/description).";

    /// <inheritdoc/>
    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

    /// <inheritdoc/>
    public Dictionary<string, object> GetParameterSchema()
    {
        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The action to perform: create, archive, restore, destroy, list, get, assign, remove, update",
                    ["enum"] = new[] { "create", "archive", "restore", "destroy", "list", "get", "assign", "remove", "update" }
                },
                ["name"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Project name (used with create, update)"
                },
                ["description"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Project description (used with create, update)"
                },
                ["project_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Project ID (GUID, used with archive, restore, destroy, get, assign, remove, update)"
                },
                ["being_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Silicon being GUID (used with assign, remove)"
                },
                ["include_archived"] = new Dictionary<string, object>
                {
                    ["type"] = "boolean",
                    ["description"] = "Whether to include archived projects in list (used with list)"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    /// <inheritdoc/>
    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        var projectManager = ServiceLocator.Instance.GetService<IProjectManager>();
        if (projectManager == null)
        {
            return ToolResult.Failed("Project manager is not initialized");
        }

        var being = ServiceLocator.Instance.BeingManager?.GetBeing(callerId);
        if (being == null)
        {
            return ToolResult.Failed($"Silicon being not found: {callerId}");
        }

        if (!being.IsCurator)
        {
            return ToolResult.Failed("Only curators can manage projects");
        }

        if (!parameters.TryGetValue("action", out var actionObj))
        {
            return ToolResult.Failed("Missing required parameter: action");
        }

        string action = actionObj?.ToString()?.ToLowerInvariant() ?? "";
        if (string.IsNullOrEmpty(action))
        {
            return ToolResult.Failed("Missing required parameter: action");
        }

        try
        {
            return action switch
            {
                "create" => ExecuteCreate(projectManager, being, parameters),
                "archive" => ExecuteArchive(projectManager, parameters),
                "restore" => ExecuteRestore(projectManager, parameters),
                "destroy" => ExecuteDestroy(projectManager, parameters),
                "list" => ExecuteList(projectManager, parameters),
                "get" => ExecuteGet(projectManager, parameters),
                "assign" => ExecuteAssign(projectManager, parameters),
                "remove" => ExecuteRemove(projectManager, parameters),
                "update" => ExecuteUpdate(projectManager, parameters),
                _ => ToolResult.Failed($"Unknown action: {action}")
            };
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Project tool execution failed: {ex.Message}");
        }
    }

    private static ToolResult ExecuteCreate(IProjectManager pm, SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("name", out var nameObj) || string.IsNullOrWhiteSpace(nameObj?.ToString()))
        {
            return ToolResult.Failed("Missing required parameter: name (for create action)");
        }

        string name = nameObj!.ToString()!.Trim();
        string description = parameters.TryGetValue("description", out var descObj) ? descObj?.ToString() ?? "" : "";

        var project = pm.CreateProject(name, description, being.Id);

        return ToolResult.Successful(
            $"Project created successfully.\n" +
            $"Name: {project.Name}\n" +
            $"ID: {project.Id}\n" +
            $"Status: {project.Status}",
            project);
    }

    private static ToolResult ExecuteArchive(IProjectManager pm, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("project_id", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid projectId))
        {
            return ToolResult.Failed("Missing or invalid parameter: project_id (for archive action)");
        }

        bool result = pm.ArchiveProject(projectId);
        if (result)
        {
            return ToolResult.Successful($"Project {projectId} archived successfully.");
        }
        return ToolResult.Failed($"Failed to archive project {projectId} (not found or not active).");
    }

    private static ToolResult ExecuteRestore(IProjectManager pm, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("project_id", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid projectId))
        {
            return ToolResult.Failed("Missing or invalid parameter: project_id (for restore action)");
        }

        bool result = pm.RestoreProject(projectId);
        if (result)
        {
            return ToolResult.Successful($"Project {projectId} restored successfully.");
        }
        return ToolResult.Failed($"Failed to restore project {projectId} (not found or not archived).");
    }

    private static ToolResult ExecuteDestroy(IProjectManager pm, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("project_id", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid projectId))
        {
            return ToolResult.Failed("Missing or invalid parameter: project_id (for destroy action)");
        }

        bool result = pm.DestroyProject(projectId);
        if (result)
        {
            return ToolResult.Successful($"Project {projectId} destroyed successfully. Associated beings are unaffected.");
        }
        return ToolResult.Failed($"Failed to destroy project {projectId} (not found).");
    }

    private static ToolResult ExecuteList(IProjectManager pm, Dictionary<string, object> parameters)
    {
        bool includeArchived = false;
        if (parameters.TryGetValue("include_archived", out var archivedObj) && archivedObj is bool b)
        {
            includeArchived = b;
        }

        var projects = pm.ListProjects(includeArchived);
        if (projects.Count == 0)
        {
            return ToolResult.Successful("No projects found.");
        }

        var lines = new List<string> { $"Found {projects.Count} project(s):" };
        foreach (var p in projects)
        {
            string statusIcon = p.Status switch
            {
                ProjectStatus.Active => "[A]",
                ProjectStatus.Archived => "[X]",
                _ => "[?]"
            };
            lines.Add($"{statusIcon} {p.Name} (ID: {p.Id}, Beings: {p.AssignedBeings.Count})");
        }

        return ToolResult.Successful(string.Join("\n", lines), projects);
    }

    private static ToolResult ExecuteGet(IProjectManager pm, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("project_id", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid projectId))
        {
            return ToolResult.Failed("Missing or invalid parameter: project_id (for get action)");
        }

        var project = pm.GetProject(projectId);
        if (project == null)
        {
            return ToolResult.Failed($"Project not found: {projectId}");
        }

        var lines = new List<string>
        {
            $"Project: {project.Name}",
            $"ID: {project.Id}",
            $"Status: {project.Status}",
            $"Description: {project.Description}",
            $"Created: {project.CreatedAt:yyyy-MM-dd HH:mm:ss}",
            $"Updated: {project.UpdatedAt:yyyy-MM-dd HH:mm:ss}",
            $"Assigned Beings: {project.AssignedBeings.Count}"
        };

        if (project.ArchivedAt.HasValue)
        {
            lines.Add($"Archived: {project.ArchivedAt.Value:yyyy-MM-dd HH:mm:ss}");
        }

        if (project.AssignedBeings.Count > 0)
        {
            lines.Add("Being IDs:");
            foreach (var beingId in project.AssignedBeings)
            {
                lines.Add($"  - {beingId}");
            }
        }

        return ToolResult.Successful(string.Join("\n", lines), project);
    }

    private static ToolResult ExecuteAssign(IProjectManager pm, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("project_id", out var pidObj) || !Guid.TryParse(pidObj?.ToString(), out Guid projectId))
        {
            return ToolResult.Failed("Missing or invalid parameter: project_id (for assign action)");
        }

        if (!parameters.TryGetValue("being_id", out var bidObj) || !Guid.TryParse(bidObj?.ToString(), out Guid beingId))
        {
            return ToolResult.Failed("Missing or invalid parameter: being_id (for assign action)");
        }

        bool result = pm.AssignBeing(projectId, beingId);
        if (result)
        {
            return ToolResult.Successful($"Assigned being {beingId} to project {projectId}.");
        }
        return ToolResult.Failed($"Failed to assign being to project {projectId} (project not found).");
    }

    private static ToolResult ExecuteRemove(IProjectManager pm, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("project_id", out var pidObj) || !Guid.TryParse(pidObj?.ToString(), out Guid projectId))
        {
            return ToolResult.Failed("Missing or invalid parameter: project_id (for remove action)");
        }

        if (!parameters.TryGetValue("being_id", out var bidObj) || !Guid.TryParse(bidObj?.ToString(), out Guid beingId))
        {
            return ToolResult.Failed("Missing or invalid parameter: being_id (for remove action)");
        }

        bool result = pm.RemoveBeing(projectId, beingId);
        if (result)
        {
            return ToolResult.Successful($"Removed being {beingId} from project {projectId}.");
        }
        return ToolResult.Failed($"Failed to remove being from project {projectId}.");
    }

    private static ToolResult ExecuteUpdate(IProjectManager pm, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("project_id", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid projectId))
        {
            return ToolResult.Failed("Missing or invalid parameter: project_id (for update action)");
        }

        string? name = parameters.TryGetValue("name", out var nameObj) ? nameObj?.ToString() : null;
        string? description = parameters.TryGetValue("description", out var descObj) ? descObj?.ToString() : null;

        if (name == null && description == null)
        {
            return ToolResult.Failed("At least one of 'name' or 'description' must be provided for update action");
        }

        var project = pm.UpdateProject(projectId, name, description);
        if (project != null)
        {
            return ToolResult.Successful(
                $"Project updated successfully.\n" +
                $"Name: {project.Name}\n" +
                $"Description: {project.Description}",
                project);
        }
        return ToolResult.Failed($"Failed to update project {projectId} (not found or not active).");
    }
}
