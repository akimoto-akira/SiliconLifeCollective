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
/// Work note system for managing page-based work notes for silicon beings.
/// Acts as a high-level manager that uses IWorkNoteStorage for persistence.
/// Provides business logic for creating, reading, updating, deleting, and searching notes.
/// </summary>
public sealed class WorkNoteSystem
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<WorkNoteSystem>();
    private readonly IWorkNoteStorage _storage;
    private readonly Guid _beingId;
    private readonly WorkNoteOwnerType _ownerType;

    /// <summary>
    /// Gets the total number of note pages for this owner.
    /// </summary>
    public int PageCount
    {
        get
        {
            try
            {
                return _storage.GetPageCount(_ownerType, _beingId.ToString());
            }
            catch
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the WorkNoteSystem class.
    /// </summary>
    /// <param name="storage">The work note storage implementation to use</param>
    /// <param name="beingId">The silicon being ID that owns these notes</param>
    /// <param name="ownerType">The owner type (defaults to SiliconBeing)</param>
    /// <exception cref="ArgumentNullException">Thrown when storage is null</exception>
    public WorkNoteSystem(IWorkNoteStorage storage, Guid beingId, WorkNoteOwnerType ownerType = WorkNoteOwnerType.SiliconBeing)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _beingId = beingId;
        _ownerType = ownerType;
        _logger.Info(beingId, "Work note system initialized for {0} owner {1}", ownerType, beingId);
    }

    /// <summary>
    /// Creates a new work note page
    /// </summary>
    /// <param name="summary">Brief summary of the note (≤100 characters)</param>
    /// <param name="content">The content of the note page</param>
    /// <param name="keywords">Optional comma-separated keywords for search</param>
    /// <param name="visibility">Optional visibility setting (defaults based on owner type)</param>
    /// <param name="authorGuid">The GUID of the creator (mandatory for project notes)</param>
    /// <returns>The created note entry</returns>
    public WorkNoteEntry CreateNote(string summary, string content, string keywords = "", WorkNoteVisibility? visibility = null, Guid authorGuid = default)
    {
        if (string.IsNullOrWhiteSpace(summary))
        {
            throw new ArgumentException("Summary cannot be empty", nameof(summary));
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content cannot be empty", nameof(content));
        }

        var note = new WorkNoteEntry
        {
            Summary = summary,
            Content = content,
            Keywords = keywords,
            OwnerType = _ownerType,
            OwnerId = _beingId.ToString(),
            AuthorGuid = authorGuid,
            ModifiedByGuid = authorGuid
        };

        if (visibility.HasValue)
        {
            note.Visibility = visibility.Value;
        }

        WorkNoteEntry created = _storage.CreateNote(note);
        _logger.Info(_beingId, "Created work note page {0} (ID: {1})", created.PageNumber, created.Id);

        return created;
    }

    /// <summary>
    /// Reads a specific work note page by page number
    /// </summary>
    /// <param name="pageNumber">The page number (1-based)</param>
    /// <returns>The note entry, or null if not found</returns>
    public WorkNoteEntry? ReadNote(int pageNumber)
    {
        if (pageNumber <= 0)
        {
            throw new ArgumentException("Page number must be greater than 0", nameof(pageNumber));
        }

        return _storage.ReadNoteByPage(_ownerType, _beingId.ToString(), pageNumber);
    }

    /// <summary>
    /// Reads a specific work note page by note ID
    /// </summary>
    /// <param name="noteId">The unique note ID</param>
    /// <returns>The note entry, or null if not found</returns>
    public WorkNoteEntry? ReadNote(Guid noteId)
    {
        return _storage.ReadNote(noteId);
    }

    /// <summary>
    /// Updates an existing work note page
    /// </summary>
    /// <param name="pageNumber">The page number to update</param>
    /// <param name="content">New content (or null to keep existing)</param>
    /// <param name="summary">New summary (or null to keep existing)</param>
    /// <param name="keywords">New keywords (or null to keep existing)</param>
    /// <param name="modifiedByGuid">The GUID of the modifier (mandatory for project notes)</param>
    /// <returns>The updated note entry</returns>
    public WorkNoteEntry UpdateNote(int pageNumber, string? content = null, string? summary = null, string? keywords = null, Guid modifiedByGuid = default)
    {
        WorkNoteEntry? note = ReadNote(pageNumber);
        if (note == null)
        {
            throw new KeyNotFoundException($"Note page {pageNumber} not found");
        }

        if (content != null)
        {
            note.Content = content;
        }

        if (summary != null)
        {
            note.Summary = summary;
        }

        if (keywords != null)
        {
            note.Keywords = keywords;
        }

        note.ModifiedByGuid = modifiedByGuid;

        WorkNoteEntry updated = _storage.UpdateNote(note);
        _logger.Info(_beingId, "Updated work note page {0} (version {1})", updated.PageNumber, updated.Version);

        return updated;
    }

    /// <summary>
    /// Deletes a work note page
    /// </summary>
    /// <param name="pageNumber">The page number to delete</param>
    /// <returns>True if successfully deleted, false if not found</returns>
    public bool DeleteNote(int pageNumber)
    {
        WorkNoteEntry? note = ReadNote(pageNumber);
        if (note == null)
        {
            return false;
        }

        bool deleted = _storage.DeleteNote(note.Id);
        if (deleted)
        {
            _logger.Info(_beingId, "Deleted work note page {0}", pageNumber);
        }

        return deleted;
    }

    /// <summary>
    /// Lists all note pages for this owner
    /// </summary>
    /// <returns>List of note entries sorted by page number</returns>
    public List<WorkNoteEntry> ListNotes()
    {
        return _storage.ListNotes(_ownerType, _beingId.ToString());
    }

    /// <summary>
    /// Generates a directory/summary of all notes.
    /// Used by ContextManager to provide AI with an overview of available notes.
    /// </summary>
    /// <returns>Formatted directory string with page numbers and summaries</returns>
    public string GenerateDirectory()
    {
        return _storage.GenerateDirectory(_ownerType, _beingId.ToString());
    }

    /// <summary>
    /// Searches notes by keyword across content, summary, and keywords fields
    /// </summary>
    /// <param name="keyword">The search keyword (case-insensitive)</param>
    /// <param name="maxCount">Maximum number of results (0 = no limit)</param>
    /// <returns>List of matching note entries</returns>
    public List<WorkNoteEntry> SearchNotes(string keyword, int maxCount = 0)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return new List<WorkNoteEntry>();
        }

        return _storage.SearchNotes(_ownerType, _beingId.ToString(), keyword, maxCount);
    }
}
