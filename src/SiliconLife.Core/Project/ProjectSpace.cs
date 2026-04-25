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
