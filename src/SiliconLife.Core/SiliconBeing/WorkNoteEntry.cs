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
/// Represents a single page of a work note.
/// Each page is an independent storage unit with its own content and metadata.
/// </summary>
public sealed class WorkNoteEntry
{
    /// <summary>
    /// Gets or sets the unique identifier for this note page
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the page number (1-based index)
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Gets or sets the brief summary/description of this note page.
    /// Used for generating context directory to help AI quickly understand note content.
    /// Recommended length: ≤100 characters.
    /// </summary>
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the content of this note page.
    /// Supports text, lists, tables, code blocks, and other formats.
    /// Original data format is preserved without compression.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner type (silicon being or project)
    /// </summary>
    public WorkNoteOwnerType OwnerType { get; set; }

    /// <summary>
    /// Gets or sets the owner ID (silicon being GUID or project ID)
    /// </summary>
    public string OwnerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the visibility/permission level
    /// </summary>
    public WorkNoteVisibility Visibility { get; set; }

    /// <summary>
    /// Gets or sets the creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last modification timestamp
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the version number (incremented on each edit)
    /// </summary>
    public int Version { get; set; } = 1;

    /// <summary>
    /// Gets or sets comma-separated keywords for search and indexing
    /// </summary>
    public string Keywords { get; set; } = string.Empty;

    /// <summary>
    /// Creates a new work note entry with default values
    /// </summary>
    public WorkNoteEntry()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}

/// <summary>
/// Defines the type of owner for a work note
/// </summary>
public enum WorkNoteOwnerType
{
    /// <summary>
    /// Owned by a silicon being (private by default, like a diary)
    /// </summary>
    SiliconBeing,

    /// <summary>
    /// Owned by a project (public by default, like a work notebook)
    /// </summary>
    Project
}

/// <summary>
/// Defines the visibility/permission level for work notes
/// </summary>
public enum WorkNoteVisibility
{
    /// <summary>
    /// Private - only visible to the owner
    /// </summary>
    Private,

    /// <summary>
    /// Public - visible to all with project access
    /// </summary>
    Public,

    /// <summary>
    /// Shared - visible to specific users (future enhancement)
    /// </summary>
    Shared
}
