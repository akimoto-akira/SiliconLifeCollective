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
/// Interface for managing work notes (create, read, update, delete, search).
/// Work notes are page-based, with each page being an independent storage unit.
/// Supports both silicon being-owned (private) and project-owned (public) notes.
/// </summary>
public interface IWorkNoteStorage
{
    /// <summary>
    /// Creates a new work note page
    /// </summary>
    /// <param name="note">The work note entry to create</param>
    /// <returns>The created note entry with assigned page number and ID</returns>
    WorkNoteEntry CreateNote(WorkNoteEntry note);

    /// <summary>
    /// Reads a specific work note page by ID
    /// </summary>
    /// <param name="noteId">The unique note page ID</param>
    /// <returns>The note entry, or null if not found</returns>
    WorkNoteEntry? ReadNote(Guid noteId);

    /// <summary>
    /// Reads a specific work note page by owner and page number
    /// </summary>
    /// <param name="ownerType">The owner type (silicon being or project)</param>
    /// <param name="ownerId">The owner ID</param>
    /// <param name="pageNumber">The page number (1-based)</param>
    /// <returns>The note entry, or null if not found</returns>
    WorkNoteEntry? ReadNoteByPage(WorkNoteOwnerType ownerType, string ownerId, int pageNumber);

    /// <summary>
    /// Updates an existing work note page
    /// </summary>
    /// <param name="note">The note entry with updated content</param>
    /// <returns>The updated note entry with incremented version</returns>
    WorkNoteEntry UpdateNote(WorkNoteEntry note);

    /// <summary>
    /// Deletes a work note page
    /// </summary>
    /// <param name="noteId">The unique note page ID</param>
    /// <returns>True if successfully deleted, false if not found</returns>
    bool DeleteNote(Guid noteId);

    /// <summary>
    /// Lists all note pages for a specific owner
    /// </summary>
    /// <param name="ownerType">The owner type</param>
    /// <param name="ownerId">The owner ID</param>
    /// <returns>List of note entries sorted by page number</returns>
    List<WorkNoteEntry> ListNotes(WorkNoteOwnerType ownerType, string ownerId);

    /// <summary>
    /// Generates a directory/summary of all notes for an owner.
    /// Used by ContextManager to provide AI with an overview of available notes.
    /// </summary>
    /// <param name="ownerType">The owner type</param>
    /// <param name="ownerId">The owner ID</param>
    /// <returns>Formatted directory string with page numbers and summaries</returns>
    string GenerateDirectory(WorkNoteOwnerType ownerType, string ownerId);

    /// <summary>
    /// Searches notes by keyword across content, summary, and keywords fields
    /// </summary>
    /// <param name="ownerType">The owner type</param>
    /// <param name="ownerId">The owner ID</param>
    /// <param name="keyword">The search keyword (case-insensitive)</param>
    /// <param name="maxCount">Maximum number of results (0 = no limit)</param>
    /// <returns>List of matching note entries</returns>
    List<WorkNoteEntry> SearchNotes(WorkNoteOwnerType ownerType, string ownerId, string keyword, int maxCount = 0);

    /// <summary>
    /// Gets the total page count for a specific owner
    /// </summary>
    /// <param name="ownerType">The owner type</param>
    /// <param name="ownerId">The owner ID</param>
    /// <returns>Total number of note pages</returns>
    int GetPageCount(WorkNoteOwnerType ownerType, string ownerId);
}
