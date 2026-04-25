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

using System.Text.Json;
using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// File system implementation of IWorkNoteStorage.
/// Stores all work notes in a single JSON file.
/// Note: This implementation prioritizes functionality over performance.
/// </summary>
public class FileSystemWorkNoteStorage : IWorkNoteStorage
{
    private readonly string _filePath;
    private readonly object _lock = new();
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// Initializes a new instance of FileSystemWorkNoteStorage
    /// </summary>
    /// <param name="baseDirectory">The base directory for storing work notes</param>
    public FileSystemWorkNoteStorage(string baseDirectory)
    {
        // All notes stored in a single JSON file directly under base directory
        _filePath = Path.Combine(baseDirectory, "work_notes.json");
    }

    private class NotesData
    {
        public List<WorkNoteEntry> Notes { get; set; } = new();
    }

    private NotesData LoadData()
    {
        lock (_lock)
        {
            if (!File.Exists(_filePath))
            {
                return new NotesData();
            }
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<NotesData>(json, _jsonOptions) ?? new NotesData();
        }
    }

    private void SaveData(NotesData data)
    {
        lock (_lock)
        {
            string json = JsonSerializer.Serialize(data, _jsonOptions);
            File.WriteAllText(_filePath, json);
        }
    }

    /// <inheritdoc/>
    public WorkNoteEntry CreateNote(WorkNoteEntry note)
    {
        var data = LoadData();

        // Assign page number if not set
        if (note.PageNumber <= 0)
        {
            note.PageNumber = data.Notes.Count > 0 ? data.Notes.Max(n => n.PageNumber) + 1 : 1;
        }

        // Set default visibility
        if (note.OwnerType == WorkNoteOwnerType.SiliconBeing && note.Visibility == WorkNoteVisibility.Public)
        {
            note.Visibility = WorkNoteVisibility.Private;
        }
        else if (note.OwnerType == WorkNoteOwnerType.Project && note.Visibility == WorkNoteVisibility.Private)
        {
            note.Visibility = WorkNoteVisibility.Public;
        }

        data.Notes.Add(note);
        SaveData(data);

        return note;
    }

    /// <inheritdoc/>
    public WorkNoteEntry? ReadNote(Guid noteId)
    {
        var data = LoadData();
        return data.Notes.FirstOrDefault(n => n.Id == noteId);
    }

    /// <inheritdoc/>
    public WorkNoteEntry? ReadNoteByPage(WorkNoteOwnerType ownerType, string ownerId, int pageNumber)
    {
        var data = LoadData();
        return data.Notes.FirstOrDefault(n =>
            n.OwnerType == ownerType &&
            n.OwnerId == ownerId &&
            n.PageNumber == pageNumber);
    }

    /// <inheritdoc/>
    public WorkNoteEntry UpdateNote(WorkNoteEntry note)
    {
        var data = LoadData();
        var existing = data.Notes.FirstOrDefault(n => n.Id == note.Id);
        if (existing == null)
        {
            throw new FileNotFoundException($"Note {note.Id} not found");
        }

        existing.Content = note.Content;
        existing.Summary = note.Summary;
        existing.Keywords = note.Keywords;
        existing.ModifiedByGuid = note.ModifiedByGuid;
        existing.Version++;
        existing.UpdatedAt = DateTime.UtcNow;

        SaveData(data);
        return existing;
    }

    /// <inheritdoc/>
    public bool DeleteNote(Guid noteId)
    {
        var data = LoadData();
        var note = data.Notes.FirstOrDefault(n => n.Id == noteId);
        if (note == null) return false;

        data.Notes.Remove(note);
        SaveData(data);
        return true;
    }

    /// <inheritdoc/>
    public List<WorkNoteEntry> ListNotes(WorkNoteOwnerType ownerType, string ownerId)
    {
        var data = LoadData();
        return data.Notes
            .Where(n => n.OwnerType == ownerType && n.OwnerId == ownerId)
            .OrderBy(n => n.PageNumber)
            .ToList();
    }

    /// <inheritdoc/>
    public string GenerateDirectory(WorkNoteOwnerType ownerType, string ownerId)
    {
        var notes = ListNotes(ownerType, ownerId);
        if (notes.Count == 0)
        {
            return "No work notes available.";
        }

        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"# Work Notes Directory ({notes.Count} pages)");
        sb.AppendLine();

        foreach (var note in notes)
        {
            string summary = string.IsNullOrEmpty(note.Summary) ? "(No summary)" : note.Summary;
            sb.AppendLine($"- **Page {note.PageNumber}**: {summary}");
            if (!string.IsNullOrEmpty(note.Keywords))
            {
                sb.AppendLine($"  - *Keywords*: {note.Keywords}");
            }
        }

        return sb.ToString();
    }

    /// <inheritdoc/>
    public List<WorkNoteEntry> SearchNotes(WorkNoteOwnerType ownerType, string ownerId, string keyword, int maxCount = 0)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return new List<WorkNoteEntry>();
        }

        var keywordLower = keyword.ToLowerInvariant();
        var notes = ListNotes(ownerType, ownerId);

        var results = notes.Where(note =>
            (!string.IsNullOrEmpty(note.Summary) && note.Summary.ToLowerInvariant().Contains(keywordLower)) ||
            (!string.IsNullOrEmpty(note.Content) && note.Content.ToLowerInvariant().Contains(keywordLower)) ||
            (!string.IsNullOrEmpty(note.Keywords) && note.Keywords.ToLowerInvariant().Contains(keywordLower))
        ).ToList();

        if (maxCount > 0)
        {
            results = results.Take(maxCount).ToList();
        }

        return results;
    }

    /// <inheritdoc/>
    public int GetPageCount(WorkNoteOwnerType ownerType, string ownerId)
    {
        return ListNotes(ownerType, ownerId).Count;
    }
}
