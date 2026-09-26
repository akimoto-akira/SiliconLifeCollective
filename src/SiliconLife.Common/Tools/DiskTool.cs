// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using SiliconLife.Collective;

using SiliconLife.Common.Localization;

namespace SiliconLife.Common.Tools;

/// <summary>
/// Disk file operations tool.
/// Performs file read/write and directory operations through DiskExecutor.
/// Verifies the disk executor pipeline.
/// </summary>
[ToolAction("read_file", "write_file", "list_directory", "delete_file", "create_directory", "exists", "get_file_info", "count_lines", "read_lines", "clear_file", "replace_lines", "replace_text", "replace_text_all", "list_drives", "search_files", "search_content")]
[ToolScenario(ToolScenarioFlag.Chat | ToolScenarioFlag.Task | ToolScenarioFlag.Timer)]
public class DiskTool : ITool
{
    public string Name => "disk";

    public string Description =>
        "Perform file and directory operations. Actions: read_file, write_file, " +
        "list_directory, delete_file, create_directory, exists, get_file_info, count_lines, read_lines, clear_file, " +
        "replace_lines (removes line_count old lines at start_line and inserts content in place; " +
        "line_count defaults to the number of content lines), " +
        "replace_text, replace_text_all (not recommended for code editing), " +
        "list_drives (all drives with type and capacity information), " +
        "search_files (search files by name), search_content (search file contents).";

    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

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
                    ["description"] = "The action to perform",
                    ["enum"] = new[] { "read_file", "write_file", "list_directory", "delete_file", "create_directory", "exists", "get_file_info", "count_lines", "read_lines", "clear_file", "replace_lines", "replace_text", "replace_text_all", "list_drives", "search_files", "search_content" }
                },
                ["path"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The file or directory path"
                },
                ["content"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Content to write (for write_file action)"
                },
                ["start_line"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "1-based line number (for read_lines: first line to read; for replace_lines: first line to replace)"
                },
                ["line_count"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Number of lines (for read_lines: lines to read; for replace_lines: number of old lines to remove before inserting content, defaults to the number of content lines)"
                },
                ["old_text"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The exact text to find and replace (for replace_text action)"
                },
                ["new_text"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The text to replace with (for replace_text action)"
                },
                ["keyword"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Search keyword (for search_files and search_content actions)"
                },
                ["directory"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Directory to search in (for search actions, default: current directory)"
                },
                ["pattern"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "File pattern (for search actions, default: *.*)"
                },
                ["max_results"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum number of results (for search actions, default: 50)"
                },
                ["case_sensitive"] = new Dictionary<string, object>
                {
                    ["type"] = "boolean",
                    ["description"] = "Case sensitive search (for search actions, default: false)"
                },
                ["max_depth"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum search depth (for search actions, default: 10, 0 = current directory only)"
                },
                ["max_file_size"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum file size in bytes for content search (for search_content, default: 10485760 = 10MB)"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj))
        {
            return ToolResult.Failed("Missing 'action' parameter");
        }

        string action = actionObj?.ToString() ?? "";

        // list_drives does not require a path
        if (action == "list_drives")
            return ExecuteListDrives();

        // search_files and search_content use keyword instead of path
        if (action == "search_files")
            return ExecuteSearchFiles(callerId, parameters);

        if (action == "search_content")
            return ExecuteSearchContent(callerId, parameters);

        // All other actions require path
        if (!parameters.TryGetValue("path", out object? pathObj) || string.IsNullOrWhiteSpace(pathObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'path' parameter");
        }

        string path = pathObj.ToString()!;

        var requestParams = new Dictionary<string, object>();
        if (parameters.TryGetValue("content", out object? contentObj) && contentObj != null)
        {
            requestParams["content"] = contentObj.ToString()!;
        }

        // Pass encoding parameter to read operations
        if (parameters.TryGetValue("encoding", out object? encodingObj) && encodingObj != null)
        {
            requestParams["encoding"] = encodingObj.ToString()!;
        }

        if (action == "count_lines")
        {
            // Verify it's a text file before reading
            if (!IsTextFile(path))
                return ToolResult.Failed($"Cannot count lines: '{path}' is not a text file. Only text and code files are supported.");

            var readParams = new Dictionary<string, object>(requestParams);
            ExecutorRequest readRequest = new(callerId, path, "read_file", readParams);
            ExecutorResult readResult = DiskExecutor.Execute(readRequest);
            if (!readResult.Success)
                return ToolResult.Failed(readResult.Error ?? "Failed to read file for line count");

            int lineCount = SplitLines(readResult.Output ?? "").Count;
            return ToolResult.Successful(lineCount.ToString());
        }

        if (action == "read_lines")
        {
            // Verify it's a text file before reading
            if (!IsTextFile(path))
                return ToolResult.Failed($"Cannot read lines: '{path}' is not a text file. Only text and code files are supported.");

            if (!parameters.TryGetValue("start_line", out object? startLineObj) ||
                !int.TryParse(startLineObj?.ToString(), out int startLine) || startLine < 1)
                return ToolResult.Failed("Missing or invalid 'start_line' parameter (must be a positive integer)");

            if (!parameters.TryGetValue("line_count", out object? lineCountObj) ||
                !int.TryParse(lineCountObj?.ToString(), out int lineCount) || lineCount < 1)
                return ToolResult.Failed("Missing or invalid 'line_count' parameter (must be a positive integer)");

            var readParams = new Dictionary<string, object>(requestParams);
            ExecutorRequest readRequest = new(callerId, path, "read_file", readParams);
            ExecutorResult readResult = DiskExecutor.Execute(readRequest);
            if (!readResult.Success)
                return ToolResult.Failed(readResult.Error ?? "Failed to read file");

            List<string> lines = SplitLines(readResult.Output ?? "");
            int zeroBasedStart = startLine - 1;
            if (zeroBasedStart >= lines.Count)
                return ToolResult.Failed($"start_line {startLine} exceeds file length ({lines.Count} lines)");

            string output = string.Join("\n", lines.Skip(zeroBasedStart).Take(lineCount));
            return ToolResult.Successful(output);
        }

        if (action == "replace_text_all")
        {
            // NOTE: Not recommended for code editing — use replace_text for precise single-match replacement.
            if (!parameters.TryGetValue("old_text", out object? oldTextAllObj) || oldTextAllObj == null)
                return ToolResult.Failed("Missing 'old_text' parameter");

            if (!parameters.TryGetValue("new_text", out object? newTextAllObj) || newTextAllObj == null)
                return ToolResult.Failed("Missing 'new_text' parameter");

            // Verify it's a text file before reading
            if (!IsTextFile(path))
                return ToolResult.Failed($"Cannot replace text: '{path}' is not a text file. Only text and code files are supported.");

            string oldTextAll = oldTextAllObj.ToString()!;
            string newTextAll = newTextAllObj.ToString()!;

            var readParamsAll = new Dictionary<string, object>(requestParams);
            ExecutorRequest readRequest = new(callerId, path, "read_file", readParamsAll);
            ExecutorResult readResult = DiskExecutor.Execute(readRequest);
            if (!readResult.Success)
                return ToolResult.Failed(readResult.Error ?? "Failed to read file");

            string fileContent = readResult.Output ?? "";
            int matchCount = 0;
            int searchStart = 0;
            while ((searchStart = fileContent.IndexOf(oldTextAll, searchStart, StringComparison.Ordinal)) != -1)
            {
                matchCount++;
                searchStart += oldTextAll.Length;
            }

            if (matchCount == 0)
                return ToolResult.Failed("'old_text' not found in file");

            string updatedContent = fileContent.Replace(oldTextAll, newTextAll, StringComparison.Ordinal);
            var writeParams = new Dictionary<string, object> { ["content"] = updatedContent };
            ExecutorRequest writeRequest = new(callerId, path, "write_file", writeParams);
            ExecutorResult writeResult = DiskExecutor.Execute(writeRequest);
            return writeResult.Success
                ? ToolResult.Successful($"Replaced {matchCount} occurrence(s) successfully")
                : ToolResult.Failed(writeResult.Error ?? "Failed to write file");
        }

        if (action == "replace_text")
        {
            if (!parameters.TryGetValue("old_text", out object? oldTextObj) || oldTextObj == null)
                return ToolResult.Failed("Missing 'old_text' parameter");

            if (!parameters.TryGetValue("new_text", out object? newTextObj) || newTextObj == null)
                return ToolResult.Failed("Missing 'new_text' parameter");

            // Verify it's a text file before reading
            if (!IsTextFile(path))
                return ToolResult.Failed($"Cannot replace text: '{path}' is not a text file. Only text and code files are supported.");

            string oldText = oldTextObj.ToString()!;
            string newText = newTextObj.ToString()!;

            var readParams = new Dictionary<string, object>(requestParams);
            ExecutorRequest readRequest = new(callerId, path, "read_file", readParams);
            ExecutorResult readResult = DiskExecutor.Execute(readRequest);
            if (!readResult.Success)
                return ToolResult.Failed(readResult.Error ?? "Failed to read file");

            string fileContent = readResult.Output ?? "";
            int matchCount = 0;
            int searchStart = 0;
            while ((searchStart = fileContent.IndexOf(oldText, searchStart, StringComparison.Ordinal)) != -1)
            {
                matchCount++;
                searchStart += oldText.Length;
            }

            if (matchCount == 0)
                return ToolResult.Failed("'old_text' not found in file");
            if (matchCount > 1)
                return ToolResult.Failed($"'old_text' found {matchCount} times in file, must be unique");

            string updatedContent = fileContent.Replace(oldText, newText, StringComparison.Ordinal);
            var writeParams = new Dictionary<string, object> { ["content"] = updatedContent };
            ExecutorRequest writeRequest = new(callerId, path, "write_file", writeParams);
            ExecutorResult writeResult = DiskExecutor.Execute(writeRequest);
            return writeResult.Success
                ? ToolResult.Successful("Text replaced successfully")
                : ToolResult.Failed(writeResult.Error ?? "Failed to write file");
        }

        if (action == "replace_lines")
        {
            if (!parameters.TryGetValue("start_line", out object? startLineObj) ||
                !int.TryParse(startLineObj?.ToString(), out int startLine) || startLine < 1)
                return ToolResult.Failed("Missing or invalid 'start_line' parameter (must be a positive integer)");

            if (!parameters.TryGetValue("content", out object? replaceContentObj) || replaceContentObj == null)
                return ToolResult.Failed("Missing 'content' parameter");

            // Verify it's a text file before reading
            if (!IsTextFile(path))
                return ToolResult.Failed($"Cannot replace lines: '{path}' is not a text file. Only text and code files are supported.");

            // Number of old lines to remove: explicit line_count, or default to the number of content lines
            int removeCount;
            if (parameters.TryGetValue("line_count", out object? removeCountObj))
            {
                if (!int.TryParse(removeCountObj?.ToString(), out int parsedRemove) || parsedRemove < 0)
                    return ToolResult.Failed("Invalid 'line_count' parameter (must be a non-negative integer)");
                removeCount = parsedRemove;
            }
            else
            {
                removeCount = -1; // resolved after content is split
            }

            var readParams = new Dictionary<string, object>(requestParams);
            ExecutorRequest readRequest = new(callerId, path, "read_file", readParams);
            ExecutorResult readResult = DiskExecutor.Execute(readRequest);
            if (!readResult.Success)
                return ToolResult.Failed(readResult.Error ?? "Failed to read file");

            List<string> lines = SplitLines(readResult.Output ?? "");
            int totalLines = lines.Count;

            if (startLine > totalLines)
                return ToolResult.Failed($"'start_line' {startLine} is out of range (file has {totalLines} lines)");

            string[] newLines = SplitLines(replaceContentObj.ToString()!).ToArray();
            if (removeCount < 0)
                removeCount = newLines.Length;

            if (startLine - 1 + removeCount > totalLines)
                return ToolResult.Failed($"Replacement range exceeds file: lines {startLine}-{startLine - 1 + removeCount} requested but file has only {totalLines} lines");

            // Build result: remove exactly removeCount lines at start_line, then insert content at the same position
            var resultLines = new List<string>(totalLines - removeCount + newLines.Length);
            resultLines.AddRange(lines.Take(startLine - 1));
            resultLines.AddRange(newLines);
            resultLines.AddRange(lines.Skip(startLine - 1 + removeCount));

            var writeParams = new Dictionary<string, object> { ["content"] = string.Join("\n", resultLines) };
            ExecutorRequest writeRequest = new(callerId, path, "write_file", writeParams);
            ExecutorResult writeResult = DiskExecutor.Execute(writeRequest);
            return writeResult.Success
                ? ToolResult.Successful($"Lines replaced successfully: start_line={startLine}, removed {removeCount} line(s), inserted {newLines.Length} line(s), total lines {totalLines} -> {resultLines.Count}")
                : ToolResult.Failed(writeResult.Error ?? "Failed to write file");
        }

        if (action == "clear_file")
        {
            // Verify it's a text file before clearing
            if (!IsTextFile(path))
                return ToolResult.Failed($"Cannot clear file: '{path}' is not a text file. Only text and code files are supported.");

            var clearParams = new Dictionary<string, object> { ["content"] = "" };
            ExecutorRequest clearRequest = new(callerId, path, "write_file", clearParams);
            ExecutorResult clearResult = DiskExecutor.Execute(clearRequest);
            return clearResult.Success
                ? ToolResult.Successful("File cleared successfully")
                : ToolResult.Failed(clearResult.Error ?? "Failed to clear file");
        }

        ExecutorRequest request = new(callerId, path, action, requestParams);
        
        // For read operations, verify it's a text file
        if (action == "read_file" && !IsTextFile(path))
        {
            return ToolResult.Failed($"Cannot read file: '{path}' is not a text file. Only text and code files are supported. Use get_file_info for binary file metadata.");
        }
        
        ExecutorResult result = DiskExecutor.Execute(request);

        if (result.Success)
        {
            // Truncate very long file contents, keeping whole lines and telling the caller how to continue
            string output = result.Output ?? "";
            if (output.Length > 10000)
            {
                string head = output.Substring(0, 10000);
                int lastBreak = head.LastIndexOf('\n');
                if (lastBreak >= 0)
                {
                    head = head.Substring(0, lastBreak);
                    int linesShown = SplitLines(head).Count;
                    int totalLines = SplitLines(output).Count;
                    output = head +
                        $"\n... (truncated: showing {linesShown} of {totalLines} lines; use read_lines with start_line={linesShown + 1} to continue)";
                }
                else
                {
                    output = head +
                        $"\n... (truncated, total length: {output.Length} characters; file may contain extremely long lines)";
                }
            }
            return ToolResult.Successful(output);
        }

        return ToolResult.Failed(result.Error ?? $"Disk operation '{action}' failed");
    }

    private static ToolResult ExecuteListDrives()
    {
        DriveInfo[] drives = DriveInfo.GetDrives();
        var lines = new List<string>();

        foreach (DriveInfo drive in drives)
        {
            string driveType = drive.DriveType switch
            {
                DriveType.Fixed       => "Fixed",
                DriveType.Removable   => "Removable (USB/SD)",
                DriveType.Network     => "Network",
                DriveType.CDRom       => "CD/DVD",
                DriveType.Ram         => "RAM Disk",
                DriveType.NoRootDirectory => "No Root",
                _                     => "Unknown"
            };

            if (!drive.IsReady)
            {
                lines.Add($"{drive.Name}  [{driveType}]  (not ready)");
                continue;
            }

            double totalGb = drive.TotalSize / 1024.0 / 1024.0 / 1024.0;
            double freeGb  = drive.TotalFreeSpace / 1024.0 / 1024.0 / 1024.0;
            double usedGb  = totalGb - freeGb;
            double usedPct = totalGb > 0 ? usedGb / totalGb * 100.0 : 0;

            string label = string.IsNullOrWhiteSpace(drive.VolumeLabel) ? "(no label)" : drive.VolumeLabel;
            lines.Add(
                $"{drive.Name}  [{driveType}]  {label}  " +
                $"Used: {usedGb:F1} GB / {totalGb:F1} GB ({usedPct:F1}%)  " +
                $"Free: {freeGb:F1} GB  " +
                $"Format: {drive.DriveFormat}");
        }

        return ToolResult.Successful($"Drives ({drives.Length}):\n" + string.Join("\n", lines));
    }

    private ToolResult ExecuteSearchFiles(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("keyword", out object? keywordObj) || string.IsNullOrWhiteSpace(keywordObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'keyword' parameter for search_files action");
        }

        string directory = parameters.TryGetValue("directory", out object? dirObj) && dirObj != null
            ? dirObj.ToString()!
            : Directory.GetCurrentDirectory();

        var executorParams = new Dictionary<string, object>();
        executorParams["keyword"] = keywordObj.ToString()!;
        if (parameters.TryGetValue("pattern", out object? patObj) && patObj != null) executorParams["pattern"] = patObj.ToString()!;
        if (parameters.TryGetValue("max_results", out object? maxObj) && maxObj != null) executorParams["max_results"] = maxObj.ToString()!;
        if (parameters.TryGetValue("case_sensitive", out object? csObj) && csObj != null) executorParams["case_sensitive"] = csObj.ToString()!;

        ExecutorRequest request = new(callerId, directory, "search_files", executorParams);
        ExecutorResult result = DiskExecutor.Execute(request);
        return result.Success
            ? ToolResult.Successful(result.Output ?? "")
            : ToolResult.Failed(result.Error ?? "Search files failed");
    }

    private ToolResult ExecuteSearchContent(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("keyword", out object? keywordObj) || string.IsNullOrWhiteSpace(keywordObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'keyword' parameter for search_content action");
        }

        string directory = parameters.TryGetValue("directory", out object? dirObj) && dirObj != null
            ? dirObj.ToString()!
            : Directory.GetCurrentDirectory();

        var executorParams = new Dictionary<string, object>();
        executorParams["keyword"] = keywordObj.ToString()!;
        if (parameters.TryGetValue("pattern", out object? patObj) && patObj != null) executorParams["pattern"] = patObj.ToString()!;
        if (parameters.TryGetValue("max_results", out object? maxObj) && maxObj != null) executorParams["max_results"] = maxObj.ToString()!;
        if (parameters.TryGetValue("case_sensitive", out object? csObj) && csObj != null) executorParams["case_sensitive"] = csObj.ToString()!;
        if (parameters.TryGetValue("max_file_size", out object? mfsObj) && mfsObj != null) executorParams["max_file_size"] = mfsObj.ToString()!;

        ExecutorRequest request = new(callerId, directory, "search_content", executorParams);
        ExecutorResult result = DiskExecutor.Execute(request);
        return result.Success
            ? ToolResult.Successful(result.Output ?? "")
            : ToolResult.Failed(result.Error ?? "Search content failed");
    }

    /// <summary>
    /// Check if a file is likely a text/code file based on extension.
    /// Delegates to DiskExecutor.IsTextFile for consistent logic.
    /// </summary>
    private static bool IsTextFile(string filePath) => DiskExecutor.IsTextFile(filePath);

    /// <summary>
    /// Splits content into lines, treating \r\n, \n and lone \r as line breaks.
    /// A trailing line break does not produce an extra empty line, so line numbers
    /// stay stable even when the file contains mixed line endings.
    /// </summary>
    private static List<string> SplitLines(string content)
    {
        var lines = new List<string>();
        if (content.Length == 0)
        {
            lines.Add(string.Empty);
            return lines;
        }

        int start = 0;
        for (int i = 0; i < content.Length; i++)
        {
            char c = content[i];
            if (c == '\n')
            {
                int end = i > start && content[i - 1] == '\r' ? i - 1 : i;
                lines.Add(content.Substring(start, end - start));
                start = i + 1;
            }
            else if (c == '\r' && (i + 1 >= content.Length || content[i + 1] != '\n'))
            {
                lines.Add(content.Substring(start, i - start));
                start = i + 1;
            }
        }

        if (start < content.Length)
            lines.Add(content.Substring(start));
        return lines;
    }
}
