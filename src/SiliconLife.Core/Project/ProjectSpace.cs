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
/// Represents a project space in the Silicon Life Collective platform.
/// A project space is an isolated workspace initiated by a curator,
/// used for storing process data and result data for silicon beings.
/// </summary>
public sealed class ProjectSpace
{
    /// <summary>
    /// Gets or sets the unique identifier for this project space
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the project space
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the project space
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the project space
    /// </summary>
    public ProjectStatus Status { get; set; } = ProjectStatus.Active;

    /// <summary>
    /// Gets or sets the GUID of the curator who created this project space
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last update timestamp
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the archival timestamp (null if not archived)
    /// </summary>
    public DateTime? ArchivedAt { get; set; }

    /// <summary>
    /// Gets or sets the list of silicon being GUIDs assigned to this project
    /// </summary>
    public List<Guid> AssignedBeings { get; set; } = new();

    /// <summary>
    /// Gets or sets the storage directory path for this project space
    /// </summary>
    public string StoragePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the workflow template name associated with this project (set at creation, immutable).
    /// Empty string means no workflow template is assigned.
    /// </summary>
    public string WorkflowTemplateName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the group chat session ID for this project (auto-created on project creation).
    /// Used for project-specific group communication between silicon beings.
    /// </summary>
    public Guid? GroupChatSessionId { get; set; }

    /// <summary>
    /// Gets or sets the broadcast channel ID for this project (auto-created on project creation).
    /// Used for project-wide announcements and notifications.
    /// </summary>
    public Guid? BroadcastChannelId { get; set; }

    /// <summary>
    /// Gets or sets the role assignments for this project.
    /// Key: role name (must match a RoleDefinition.RoleName in the project's WorkflowTemplate),
    /// Value: list of silicon being GUIDs assigned to that role.
    /// Roles are managed by the curator via ProjectTool (assign_role / remove_role / list_roles).
    /// </summary>
    public Dictionary<string, List<Guid>> RoleAssignments { get; set; } = new();

    /// <summary>
    /// Gets or sets the project-level tool action permission config for this project.
    /// This is a single unified config that applies to all beings in the project.
    /// At runtime, the effective permissions are computed as:
    /// EffectiveDisabled = BeingGlobalDisabled ∪ ProjectDisabled
    /// (i.e., intersection of allowed actions — both must allow for an action to be permitted).
    /// null means no project-level restrictions (all beings inherit their global permissions).
    /// </summary>
    public ToolActionPermissionConfig? ToolActionPermissions { get; set; }

    /// <summary>
    /// Creates a new project space with default values
    /// </summary>
    public ProjectSpace()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}

/// <summary>
/// Defines the lifecycle status of a project space
/// </summary>
public enum ProjectStatus
{
    /// <summary>
    /// Project is active and available for use
    /// </summary>
    Active,

    /// <summary>
    /// Project is archived (read-only, can be restored)
    /// </summary>
    Archived,

    /// <summary>
    /// Project is destroyed (data will be cleaned up)
    /// </summary>
    Destroyed
}
