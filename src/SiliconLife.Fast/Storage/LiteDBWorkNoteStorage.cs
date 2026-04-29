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

using LiteDB;
using SiliconLife.Collective;

namespace SiliconLife.Fast;

/// <summary>
/// LiteDB implementation of IWorkNoteStorage interface
/// </summary>
public class LiteDBWorkNoteStorage : IWorkNoteStorage
{
    private readonly ILiteCollection<WorkNoteRecord> _collection;

    public LiteDBWorkNoteStorage()
    {
        _collection = LiteDBManager.WorkNoteCollection;
    }

    public WorkNoteEntry CreateNote(WorkNoteEntry note)
    {
        // Auto-assign page number if not set
        if (note.PageNumber == 0)
        {
            note.PageNumber = GetPageCount(note.OwnerType, note.OwnerId) + 1;
        }

        var bsonData = BsonMapper.Global.Serialize(note);
        var record = new WorkNoteRecord
        {
            NoteId = note.Id,
            OwnerType = note.OwnerType,
            OwnerId = note.OwnerId,
            PageNumber = note.PageNumber,
            Data = bsonData.AsDocument,
            CreatedAt = note.CreatedAt
        };

        _collection.Insert(record);
        return note;
    }

    public WorkNoteEntry? ReadNote(Guid noteId)
    {
        var record = _collection.FindOne(x => x.NoteId == noteId);
        if (record == null)
        {
            return null;
        }

        return BsonMapper.Global.Deserialize<WorkNoteEntry>(record.Data);
    }

    public WorkNoteEntry? ReadNoteByPage(WorkNoteOwnerType ownerType, string ownerId, int pageNumber)
    {
        var record = _collection.FindOne(x => 
            x.OwnerType == ownerType && 
            x.OwnerId == ownerId && 
            x.PageNumber == pageNumber);
        
        if (record == null)
        {
            return null;
        }

        return BsonMapper.Global.Deserialize<WorkNoteEntry>(record.Data);
    }

    public WorkNoteEntry UpdateNote(WorkNoteEntry note)
    {
        note.Version++;
        note.UpdatedAt = DateTime.UtcNow;

        var bsonData = BsonMapper.Global.Serialize(note);
        var record = _collection.FindOne(x => x.NoteId == note.Id);
        
        if (record != null)
        {
            record.Data = bsonData.AsDocument;
            record.PageNumber = note.PageNumber;
            _collection.Update(record);
        }
        else
        {
            // If not found, create it
            record = new WorkNoteRecord
            {
                NoteId = note.Id,
                OwnerType = note.OwnerType,
                OwnerId = note.OwnerId,
                PageNumber = note.PageNumber,
                Data = bsonData.AsDocument,
                CreatedAt = note.CreatedAt
            };
            _collection.Insert(record);
        }

        return note;
    }

    public bool DeleteNote(Guid noteId)
    {
        return _collection.DeleteMany(x => x.NoteId == noteId) > 0;
    }

    public List<WorkNoteEntry> ListNotes(WorkNoteOwnerType ownerType, string ownerId)
    {
        var records = _collection.Find(x => 
                x.OwnerType == ownerType && 
                x.OwnerId == ownerId)
            .OrderBy(r => r.PageNumber)
            .ToList();

        return records
            .Select(r => BsonMapper.Global.Deserialize<WorkNoteEntry>(r.Data)!)
            .Where(e => e != null)
            .ToList();
    }

    public string GenerateDirectory(WorkNoteOwnerType ownerType, string ownerId)
    {
        var notes = ListNotes(ownerType, ownerId);
        if (notes.Count == 0)
        {
            return "No work notes available.";
        }

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("Work Notes Directory:");
        sb.AppendLine();

        foreach (var note in notes)
        {
            sb.AppendLine($"Page {note.PageNumber}: {note.Summary}");
            if (!string.IsNullOrEmpty(note.Keywords))
            {
                sb.AppendLine($"  Keywords: {note.Keywords}");
            }
        }

        return sb.ToString();
    }

    public List<WorkNoteEntry> SearchNotes(WorkNoteOwnerType ownerType, string ownerId, string keyword, int maxCount = 0)
    {
        var records = _collection.Find(x => 
                x.OwnerType == ownerType && 
                x.OwnerId == ownerId)
            .ToList();

        var keywordLower = keyword.ToLowerInvariant();
        var results = records.Where(r =>
        {
            var data = r.Data;
            // Search in Summary, Content, and Keywords fields
            foreach (var element in data)
            {
                if (element.Value.IsString)
                {
                    var value = element.Value.AsString.ToLowerInvariant();
                    if ((element.Key == "Summary" || element.Key == "Content" || element.Key == "Keywords") 
                        && value.Contains(keywordLower))
                    {
                        return true;
                    }
                }
            }
            return false;
        });

        if (maxCount > 0)
        {
            results = results.Take(maxCount);
        }

        return results
            .Select(r => BsonMapper.Global.Deserialize<WorkNoteEntry>(r.Data)!)
            .Where(e => e != null)
            .ToList();
    }

    public int GetPageCount(WorkNoteOwnerType ownerType, string ownerId)
    {
        return _collection.Count(x => 
            x.OwnerType == ownerType && 
            x.OwnerId == ownerId);
    }
}
