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
/// Tool for managing work notes for silicon beings.
/// Supports creating, reading, updating, deleting, listing, searching, and generating directories.
/// </summary>
public class WorkNoteTool : ITool
{
    /// <inheritdoc/>
    public string Name => "work_note";

    /// <inheritdoc/>
    public string Description =>
        "Manage work notes for the silicon being. " +
        "Work notes are page-based, like a personal diary (private by default). " +
        "Actions: " +
        "'create' (create a new note page: requires 'summary' and 'content', optional 'keywords'); " +
        "'read' (read a specific note page: requires 'page_number' or 'note_id'); " +
        "'update' (update an existing note page: requires 'page_number' and 'content', optional 'summary' and 'keywords'); " +
        "'delete' (delete a note page: requires 'page_number' or 'note_id'); " +
        "'list' (list all note pages with summaries); " +
        "'directory' (generate a directory/overview of all notes for context); " +
        "'search' (search notes by keyword: requires 'keyword', optional 'max_results').";

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
                    ["description"] = "The action to perform: create, read, update, delete, list, directory, search",
                    ["enum"] = new[] { "create", "read", "update", "delete", "list", "directory", "search" }
                },
                ["page_number"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "The page number (1-based) for read, update, delete actions"
                },
                ["note_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The unique note ID (GUID format) for read or delete actions"
                },
                ["summary"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Brief summary/description of the note page (≤100 characters, used for create/update)"
                },
                ["content"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The content of the note page (supports text, lists, tables, code blocks)"
                },
                ["keywords"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Comma-separated keywords for search and indexing (optional)"
                },
                ["keyword"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Search keyword for search action"
                },
                ["max_results"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum number of results to return (used with search)"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    /// <inheritdoc/>
    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        try
        {
            if (!parameters.TryGetValue("action", out var actionObj))
            {
                return ToolResult.Failed("Missing required parameter: action");
            }

            string action = actionObj?.ToString()?.ToLowerInvariant() ?? "";
            if (string.IsNullOrEmpty(action))
            {
                return ToolResult.Failed("Missing required parameter: action");
            }

            // Get the silicon being
            SiliconBeingBase? being = ServiceLocator.Instance.BeingManager?.GetBeing(callerId);
            if (being == null)
            {
                return ToolResult.Failed($"Silicon being not found: {callerId}");
            }

            if (being.WorkNoteSystem == null)
            {
                return ToolResult.Failed("Work note system is not initialized for this silicon being");
            }

            return action.ToLowerInvariant() switch
            {
                "create" => ExecuteCreate(being, parameters),
                "read" => ExecuteRead(being, parameters),
                "update" => ExecuteUpdate(being, parameters),
                "delete" => ExecuteDelete(being, parameters),
                "list" => ExecuteList(being),
                "directory" => ExecuteDirectory(being),
                "search" => ExecuteSearch(being, parameters),
                _ => ToolResult.Failed($"Unknown action: {action}")
            };
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Work note tool execution failed: {ex.Message}");
        }
    }

    private ToolResult ExecuteCreate(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("summary", out var summaryObj))
        {
            return ToolResult.Failed("Missing required parameter: summary (for create action)");
        }

        string summary = summaryObj?.ToString() ?? "";
        if (string.IsNullOrEmpty(summary))
        {
            return ToolResult.Failed("Missing required parameter: summary (for create action)");
        }

        if (!parameters.TryGetValue("content", out var contentObj))
        {
            return ToolResult.Failed("Missing required parameter: content (for create action)");
        }

        string content = contentObj?.ToString() ?? "";
        if (string.IsNullOrEmpty(content))
        {
            return ToolResult.Failed("Missing required parameter: content (for create action)");
        }

        string keywords = parameters.TryGetValue("keywords", out var kwObj)
            ? (kwObj?.ToString() ?? "")
            : string.Empty;

        WorkNoteEntry created = being.WorkNoteSystem.CreateNote(summary, content, keywords);

        return ToolResult.Successful(
            $"Work note page created successfully.\n" +
            $"Page Number: {created.PageNumber}\n" +
            $"Note ID: {created.Id}\n" +
            $"Summary: {created.Summary}",
            created);
    }

    private ToolResult ExecuteRead(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        WorkNoteEntry? note = null;

        // Try to read by note_id first
        if (parameters.TryGetValue("note_id", out var noteIdObj))
        {
            string noteIdStr = noteIdObj?.ToString() ?? "";
            if (Guid.TryParse(noteIdStr, out Guid noteId))
            {
                note = being.WorkNoteSystem.ReadNote(noteId);
            }
            else
            {
                return ToolResult.Failed("Invalid note_id format (must be a GUID)");
            }
        }
        // Try to read by page_number
        else if (parameters.TryGetValue("page_number", out var pageNumObj) && int.TryParse(pageNumObj?.ToString(), out int pageNum))
        {
            note = being.WorkNoteSystem.ReadNote(pageNum);
        }
        else
        {
            return ToolResult.Failed("Missing required parameter: page_number or note_id (for read action)");
        }

        if (note == null)
        {
            return ToolResult.Failed("Note page not found");
        }

        return ToolResult.Successful(
            $"Page {note.PageNumber}:\n" +
            $"Summary: {note.Summary}\n" +
            $"Keywords: {note.Keywords}\n" +
            $"Version: {note.Version}\n" +
            $"Created: {note.CreatedAt:yyyy-MM-dd HH:mm:ss}\n" +
            $"Updated: {note.UpdatedAt:yyyy-MM-dd HH:mm:ss}\n" +
            $"\nContent:\n{note.Content}",
            note);
    }

    private ToolResult ExecuteUpdate(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("page_number", out var pageNumObj) || !int.TryParse(pageNumObj?.ToString(), out int pageNum))
        {
            return ToolResult.Failed("Missing required parameter: page_number (for update action)");
        }

        // Update fields if provided
        string? content = parameters.TryGetValue("content", out var contentObj) ? contentObj?.ToString() : null;
        string? summary = parameters.TryGetValue("summary", out var summaryObj) ? summaryObj?.ToString() : null;
        string? keywords = parameters.TryGetValue("keywords", out var kwObj) ? kwObj?.ToString() : null;

        WorkNoteEntry updated = being.WorkNoteSystem.UpdateNote(pageNum, content, summary, keywords);

        return ToolResult.Successful(
            $"Work note page updated successfully.\n" +
            $"Page Number: {updated.PageNumber}\n" +
            $"Version: {updated.Version}\n" +
            $"Updated: {updated.UpdatedAt:yyyy-MM-dd HH:mm:ss}",
            updated);
    }

    private ToolResult ExecuteDelete(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        int? pageNum = null;

        // Try to get page_number directly
        if (parameters.TryGetValue("page_number", out var pageNumObj) && int.TryParse(pageNumObj?.ToString(), out int p))
        {
            pageNum = p;
        }
        // Try to get note_id and find corresponding page
        else if (parameters.TryGetValue("note_id", out var noteIdObj))
        {
            string noteIdStr = noteIdObj?.ToString() ?? "";
            if (Guid.TryParse(noteIdStr, out Guid noteId))
            {
                WorkNoteEntry? note = being.WorkNoteSystem.ReadNote(noteId);
                if (note != null)
                {
                    pageNum = note.PageNumber;
                }
                else
                {
                    return ToolResult.Failed("Note not found");
                }
            }
            else
            {
                return ToolResult.Failed("Invalid note_id format (must be a GUID)");
            }
        }
        else
        {
            return ToolResult.Failed("Missing required parameter: page_number or note_id (for delete action)");
        }

        bool deleted = being.WorkNoteSystem.DeleteNote(pageNum.Value);
        if (!deleted)
        {
            return ToolResult.Failed("Failed to delete note page");
        }

        return ToolResult.Successful($"Work note page deleted successfully.");
    }

    private ToolResult ExecuteList(SiliconBeingBase being)
    {
        var notes = being.WorkNoteSystem.ListNotes();
        if (notes.Count == 0)
        {
            return ToolResult.Successful("No work notes found.");
        }

        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"# Work Notes ({notes.Count} pages)");
        sb.AppendLine();

        foreach (var note in notes)
        {
            string summary = string.IsNullOrEmpty(note.Summary) ? "(No summary)" : note.Summary;
            sb.AppendLine($"## Page {note.PageNumber}");
            sb.AppendLine($"- **Summary**: {summary}");
            if (!string.IsNullOrEmpty(note.Keywords))
            {
                sb.AppendLine($"- **Keywords**: {note.Keywords}");
            }
            sb.AppendLine($"- **Version**: {note.Version}");
            sb.AppendLine($"- **Updated**: {note.UpdatedAt:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine();
        }

        return ToolResult.Successful(sb.ToString(), notes);
    }

    private ToolResult ExecuteDirectory(SiliconBeingBase being)
    {
        string directory = being.WorkNoteSystem.GenerateDirectory();
        return ToolResult.Successful(directory);
    }

    private ToolResult ExecuteSearch(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("keyword", out var keywordObj))
        {
            return ToolResult.Failed("Missing required parameter: keyword (for search action)");
        }

        string keyword = keywordObj?.ToString() ?? "";
        if (string.IsNullOrEmpty(keyword))
        {
            return ToolResult.Failed("Missing required parameter: keyword (for search action)");
        }

        int maxResults = 0;
        if (parameters.TryGetValue("max_results", out var maxObj) && int.TryParse(maxObj?.ToString(), out int max))
        {
            maxResults = max;
        }

        var results = being.WorkNoteSystem.SearchNotes(keyword, maxResults);
        if (results.Count == 0)
        {
            return ToolResult.Successful($"No work notes found matching keyword: '{keyword}'");
        }

        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"# Search Results for '{keyword}' ({results.Count} matches)");
        sb.AppendLine();

        foreach (var note in results)
        {
            string summary = string.IsNullOrEmpty(note.Summary) ? "(No summary)" : note.Summary;
            sb.AppendLine($"## Page {note.PageNumber}");
            sb.AppendLine($"- **Summary**: {summary}");
            sb.AppendLine($"- **Keywords**: {note.Keywords}");
            sb.AppendLine($"- **Updated**: {note.UpdatedAt:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine();
        }

        return ToolResult.Successful(sb.ToString(), results);
    }
}
