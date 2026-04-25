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
/// Interface for managing project spaces in the Silicon Life Collective platform.
/// Provides lifecycle management (create, archive, restore, destroy) and
/// assignment of silicon beings to projects.
/// </summary>
public interface IProjectManager
{
    /// <summary>
    /// Creates a new project space. Only curators can create projects.
    /// </summary>
    /// <param name="name">The name of the project</param>
    /// <param name="description">The description of the project</param>
    /// <param name="createdBy">The GUID of the curator creating the project</param>
    /// <returns>The created project space</returns>
    ProjectSpace CreateProject(string name, string description, Guid createdBy);

    /// <summary>
    /// Archives a project space. Archived projects become read-only.
    /// </summary>
    /// <param name="projectId">The project ID to archive</param>
    /// <returns>True if archived successfully, false if not found</returns>
    bool ArchiveProject(Guid projectId);

    /// <summary>
    /// Restores an archived project space to active status.
    /// </summary>
    /// <param name="projectId">The project ID to restore</param>
    /// <returns>True if restored successfully, false if not found or not archived</returns>
    bool RestoreProject(Guid projectId);

    /// <summary>
    /// Destroys a project space. Associated silicon beings are unaffected.
    /// Only cleans up project space data.
    /// </summary>
    /// <param name="projectId">The project ID to destroy</param>
    /// <returns>True if destroyed successfully, false if not found</returns>
    bool DestroyProject(Guid projectId);

    /// <summary>
    /// Gets a project space by its ID
    /// </summary>
    /// <param name="projectId">The project ID</param>
    /// <returns>The project space, or null if not found</returns>
    ProjectSpace? GetProject(Guid projectId);

    /// <summary>
    /// Lists all project spaces
    /// </summary>
    /// <param name="includeArchived">Whether to include archived projects</param>
    /// <returns>List of project spaces</returns>
    List<ProjectSpace> ListProjects(bool includeArchived = false);

    /// <summary>
    /// Assigns a silicon being to a project
    /// </summary>
    /// <param name="projectId">The project ID</param>
    /// <param name="beingId">The silicon being GUID to assign</param>
    /// <returns>True if assigned successfully, false if project not found</returns>
    bool AssignBeing(Guid projectId, Guid beingId);

    /// <summary>
    /// Removes a silicon being from a project
    /// </summary>
    /// <param name="projectId">The project ID</param>
    /// <param name="beingId">The silicon being GUID to remove</param>
    /// <returns>True if removed successfully, false if not found</returns>
    bool RemoveBeing(Guid projectId, Guid beingId);

    /// <summary>
    /// Checks if a silicon being is assigned to a project
    /// </summary>
    /// <param name="projectId">The project ID</param>
    /// <param name="beingId">The silicon being GUID</param>
    /// <returns>True if the being is assigned to the project</returns>
    bool IsBeingAssigned(Guid projectId, Guid beingId);

    /// <summary>
    /// Updates a project space's name and/or description
    /// </summary>
    /// <param name="projectId">The project ID</param>
    /// <param name="name">New name (null to keep existing)</param>
    /// <param name="description">New description (null to keep existing)</param>
    /// <returns>The updated project space, or null if not found</returns>
    ProjectSpace? UpdateProject(Guid projectId, string? name = null, string? description = null);

    /// <summary>
    /// Gets the count of active projects
    /// </summary>
    int ActiveProjectCount { get; }

    /// <summary>
    /// Gets the count of archived projects
    /// </summary>
    int ArchivedProjectCount { get; }

    /// <summary>
    /// Gets the work note system for a specific project.
    /// Returns null if project not found or destroyed.
    /// </summary>
    /// <param name="projectId">The project ID</param>
    /// <returns>The work note system for the project, or null</returns>
    WorkNoteSystem? GetWorkNoteSystem(Guid projectId);

    /// <summary>
    /// Gets the task system for a specific project.
    /// Returns null if project not found or destroyed.
    /// </summary>
    /// <param name="projectId">The project ID</param>
    /// <returns>The task system for the project, or null</returns>
    ProjectTaskSystem? GetTaskSystem(Guid projectId);
}
