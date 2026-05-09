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
using SiliconLife.Speedy;

namespace SiliconLife.Fast;

/// <summary>
/// <see cref="IWorkNoteStorage"/> adapter backed by a <see cref="SpeedyStorage"/> (.spk) file.
/// Notes are stored as JSON at paths: <c>work_notes/{ownerType}/{ownerId}/{noteId}.json</c>.
/// An index per owner is maintained at <c>work_notes/{ownerType}/{ownerId}/_index.json</c>.
/// A global reverse-lookup at <c>work_notes/_global/{noteId}.json</c> enables
/// <see cref="ReadNote"/> by ID without scanning all owners.
/// </summary>
/// <remarks>
/// The underlying <see cref="SpeedyPack"/> is the single instance owned by
/// <see cref="SpeedyPackRegistry"/>. Disposing this wrapper does <em>not</em> close
/// the pack — call <see cref="SpeedyPackRegistry.Dispose"/> during application shutdown.
/// </remarks>
public sealed class SpeedyWorkNoteStorage : IWorkNoteStorage, IDisposable
{
    private readonly SpeedyStorage _storage;

    /// <summary>
    /// Wraps the single shared <see cref="SpeedyPack"/> from
    /// <see cref="SpeedyPackRegistry"/> as an <see cref="IWorkNoteStorage"/> implementation.
    /// </summary>
    /// <param name="dir">Optional directory path used to extract a key prefix for isolation.</param>
    public SpeedyWorkNoteStorage(string dir = "")
    {
        _storage = new SpeedyStorage(dir);
    }

    // ─── Path helpers ─────────────────────────────────────────────────────────

    private static string NoteKey(WorkNoteOwnerType ownerType, string ownerId, Guid noteId) =>
        $"work_notes/{ownerType}/{ownerId}/{noteId}";

    private static string IndexKey(WorkNoteOwnerType ownerType, string ownerId) =>
        $"work_notes/{ownerType}/{ownerId}/_index";

    private static string GlobalRefKey(Guid noteId) =>
        $"work_notes/_global/{noteId}";

    // ─── Index helpers ────────────────────────────────────────────────────────

    private List<Guid> LoadIndex(WorkNoteOwnerType ownerType, string ownerId)
    {
        var lists = _storage.Read<List<Guid>>(IndexKey(ownerType, ownerId));
        return lists.FirstOrDefault() ?? new List<Guid>();
    }

    private void SaveIndex(WorkNoteOwnerType ownerType, string ownerId, List<Guid> index) =>
        _storage.Write(IndexKey(ownerType, ownerId), index);

    // ─── IWorkNoteStorage ─────────────────────────────────────────────────────

    public WorkNoteEntry CreateNote(WorkNoteEntry note)
    {
        if (note.PageNumber == 0)
            note.PageNumber = GetPageCount(note.OwnerType, note.OwnerId) + 1;

        _storage.Write(NoteKey(note.OwnerType, note.OwnerId, note.Id), note);
        _storage.Write(GlobalRefKey(note.Id), new NoteOwnerRef(note.OwnerType, note.OwnerId));

        var index = LoadIndex(note.OwnerType, note.OwnerId);
        if (!index.Contains(note.Id))
        {
            index.Add(note.Id);
            SaveIndex(note.OwnerType, note.OwnerId, index);
        }

        return note;
    }

    public WorkNoteEntry? ReadNote(Guid noteId)
    {
        var ownerRefs = _storage.Read<NoteOwnerRef>(GlobalRefKey(noteId));
        var ownerRef = ownerRefs.FirstOrDefault();
        if (ownerRef == null) return null;
        var notes = _storage.Read<WorkNoteEntry>(NoteKey(ownerRef.OwnerType, ownerRef.OwnerId, noteId));
        return notes.FirstOrDefault();
    }

    public WorkNoteEntry? ReadNoteByPage(WorkNoteOwnerType ownerType, string ownerId, int pageNumber)
    {
        foreach (var id in LoadIndex(ownerType, ownerId))
        {
            var notes = _storage.Read<WorkNoteEntry>(NoteKey(ownerType, ownerId, id));
            var note = notes.FirstOrDefault();
            if (note?.PageNumber == pageNumber)
                return note;
        }
        return null;
    }

    public WorkNoteEntry UpdateNote(WorkNoteEntry note)
    {
        note.Version++;
        note.UpdatedAt = DateTime.UtcNow;

        _storage.Write(NoteKey(note.OwnerType, note.OwnerId, note.Id), note);
        _storage.Write(GlobalRefKey(note.Id), new NoteOwnerRef(note.OwnerType, note.OwnerId));

        var index = LoadIndex(note.OwnerType, note.OwnerId);
        if (!index.Contains(note.Id))
        {
            index.Add(note.Id);
            SaveIndex(note.OwnerType, note.OwnerId, index);
        }

        return note;
    }

    public bool DeleteNote(Guid noteId)
    {
        var ownerRefs = _storage.Read<NoteOwnerRef>(GlobalRefKey(noteId));
        var ownerRef = ownerRefs.FirstOrDefault();
        if (ownerRef == null) return false;

        _storage.Delete(NoteKey(ownerRef.OwnerType, ownerRef.OwnerId, noteId));
        _storage.Delete(GlobalRefKey(noteId));

        var index = LoadIndex(ownerRef.OwnerType, ownerRef.OwnerId);
        if (index.Remove(noteId))
            SaveIndex(ownerRef.OwnerType, ownerRef.OwnerId, index);

        return true;
    }

    public List<WorkNoteEntry> ListNotes(WorkNoteOwnerType ownerType, string ownerId)
    {
        var noteEntries = new List<WorkNoteEntry>();
        foreach (var id in LoadIndex(ownerType, ownerId))
        {
            var notes = _storage.Read<WorkNoteEntry>(NoteKey(ownerType, ownerId, id));
            var note = notes.FirstOrDefault();
            if (note != null) noteEntries.Add(note);
        }
        noteEntries.Sort((a, b) => a.PageNumber.CompareTo(b.PageNumber));
        return noteEntries;
    }

    public string GenerateDirectory(WorkNoteOwnerType ownerType, string ownerId)
    {
        var notes = ListNotes(ownerType, ownerId);
        if (notes.Count == 0) return "No work notes available.";

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("Work Notes Directory:");
        sb.AppendLine();
        foreach (var note in notes)
        {
            sb.AppendLine($"Page {note.PageNumber}: {note.Summary}");
            if (!string.IsNullOrEmpty(note.Keywords))
                sb.AppendLine($"  Keywords: {note.Keywords}");
        }
        return sb.ToString();
    }

    public List<WorkNoteEntry> SearchNotes(WorkNoteOwnerType ownerType, string ownerId, string keyword, int maxCount = 0)
    {
        var kw = keyword.ToLowerInvariant();
        var results = ListNotes(ownerType, ownerId).Where(n =>
            (n.Summary?.ToLowerInvariant().Contains(kw) ?? false) ||
            (n.Content?.ToLowerInvariant().Contains(kw) ?? false) ||
            (n.Keywords?.ToLowerInvariant().Contains(kw) ?? false));
        return (maxCount > 0 ? results.Take(maxCount) : results).ToList();
    }

    public int GetPageCount(WorkNoteOwnerType ownerType, string ownerId) =>
        LoadIndex(ownerType, ownerId).Count;

    // ─── IDisposable ──────────────────────────────────────────────────────────

    /// <summary>
    /// No-op. The underlying <see cref="SpeedyPack"/> lifetime is controlled by
    /// <see cref="SpeedyPackRegistry.Dispose"/>.
    /// </summary>
    public void Dispose() { }

    private sealed record NoteOwnerRef(WorkNoteOwnerType OwnerType, string OwnerId);
}
