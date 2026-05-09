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
[ToolScenario(ToolScenarioFlag.Chat | ToolScenarioFlag.Task | ToolScenarioFlag.Timer)]
public class DiskTool : ITool
{
    public string Name => "disk";

    public string Description =>
        "Perform file and directory operations. Actions: read_file, write_file, " +
        "list_directory, delete_file, create_directory, exists, get_file_info, count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all (not recommended for code editing), " +
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
                    ["description"] = "1-based line number to start reading from (for read_lines action)"
                },
                ["line_count"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Number of lines to read (for read_lines action)"
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

            int lineCount = (readResult.Output ?? "").Split('\n').Length;
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

            string[] lines = (readResult.Output ?? "").Split('\n');
            int zeroBasedStart = startLine - 1;
            if (zeroBasedStart >= lines.Length)
                return ToolResult.Failed($"start_line {startLine} exceeds file length ({lines.Length} lines)");

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

            var readParams = new Dictionary<string, object>(requestParams);
            ExecutorRequest readRequest = new(callerId, path, "read_file", readParams);
            ExecutorResult readResult = DiskExecutor.Execute(readRequest);
            if (!readResult.Success)
                return ToolResult.Failed(readResult.Error ?? "Failed to read file");

            string[] lines = (readResult.Output ?? "").Split('\n');
            int totalLines = lines.Length;

            if (startLine > totalLines)
                return ToolResult.Failed($"'start_line' {startLine} is out of range (file has {totalLines} lines)");

            string[] newLines = replaceContentObj.ToString()!.Split('\n');

            // Build result: lines before start_line, then overlay/append new lines
            var resultLines = new List<string>(lines);
            int zeroBasedStart = startLine - 1;
            for (int i = 0; i < newLines.Length; i++)
            {
                int targetIndex = zeroBasedStart + i;
                if (targetIndex < resultLines.Count)
                    resultLines[targetIndex] = newLines[i];
                else
                    resultLines.Add(newLines[i]);
            }

            var writeParams = new Dictionary<string, object> { ["content"] = string.Join("\n", resultLines) };
            ExecutorRequest writeRequest = new(callerId, path, "write_file", writeParams);
            ExecutorResult writeResult = DiskExecutor.Execute(writeRequest);
            return writeResult.Success
                ? ToolResult.Successful("Lines replaced successfully")
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
            // Truncate very long file contents
            string output = result.Output ?? "";
            if (output.Length > 10000)
            {
                output = output.Substring(0, 10000) + "\n... (truncated, total length: " + output.Length + " characters)";
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

        string keyword = keywordObj.ToString()!;
        string directory = parameters.TryGetValue("directory", out object? dirObj) && dirObj != null
            ? dirObj.ToString()!
            : Directory.GetCurrentDirectory();
        string pattern = parameters.TryGetValue("pattern", out object? patternObj) && patternObj != null
            ? patternObj.ToString()!
            : "*.*";
        int maxResults = 50;
        bool caseSensitive = false;

        if (parameters.TryGetValue("max_results", out object? maxResultsObj) && maxResultsObj != null)
        {
            if (int.TryParse(maxResultsObj.ToString(), out int parsedMaxResults))
            {
                maxResults = parsedMaxResults;
            }
        }

        if (parameters.TryGetValue("case_sensitive", out object? caseSensitiveObj) && caseSensitiveObj != null)
        {
            if (bool.TryParse(caseSensitiveObj.ToString(), out bool parsedCaseSensitive))
            {
                caseSensitive = parsedCaseSensitive;
            }
        }

        var results = new List<string>();
        var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

        try
        {
            SearchFilesRecursive(directory, pattern, keyword, comparison, maxResults, results);
        }
        catch (Exception ex)
        {
            // If top-level directory itself is inaccessible, report error
            if (results.Count == 0)
                return ToolResult.Failed($"Error searching files: {ex.Message}");
        }

        if (results.Count == 0)
        {
            return ToolResult.Successful($"No files found matching keyword '{keyword}' in {directory}");
        }

        return ToolResult.Successful($"Found {results.Count} files matching keyword '{keyword}':\n" +
                                   string.Join("\n", results));
    }

    /// <summary>
    /// Recursively search files by name, handling UnauthorizedAccessException per directory.
    /// </summary>
    private static void SearchFilesRecursive(
        string directory, string pattern, string keyword,
        StringComparison comparison, int maxResults, List<string> results)
    {
        if (results.Count >= maxResults) return;

        // 1. Search files in current directory first
        try
        {
            foreach (var file in Directory.EnumerateFiles(directory, pattern, SearchOption.TopDirectoryOnly))
            {
                var fileName = Path.GetFileName(file);
                if (fileName.Contains(keyword, comparison))
                {
                    results.Add(file);
                    if (results.Count >= maxResults) return;
                }
            }
        }
        catch (UnauthorizedAccessException) { }
        catch (IOException) { }

        // 2. Recurse into subdirectories
        try
        {
            foreach (var subDir in Directory.EnumerateDirectories(directory))
            {
                if (results.Count >= maxResults) return;
                try
                {
                    SearchFilesRecursive(subDir, pattern, keyword, comparison, maxResults, results);
                }
                catch (UnauthorizedAccessException) { }
                catch (IOException) { }
            }
        }
        catch (UnauthorizedAccessException) { }
        catch (IOException) { }
    }

    private ToolResult ExecuteSearchContent(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("keyword", out object? keywordObj) || string.IsNullOrWhiteSpace(keywordObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'keyword' parameter for search_content action");
        }

        string keyword = keywordObj.ToString()!;
        string directory = parameters.TryGetValue("directory", out object? dirObj) && dirObj != null
            ? dirObj.ToString()!
            : Directory.GetCurrentDirectory();
        string pattern = parameters.TryGetValue("pattern", out object? patternObj) && patternObj != null
            ? patternObj.ToString()!
            : "*.*";
        int maxResults = 50;
        bool caseSensitive = false;

        if (parameters.TryGetValue("max_results", out object? maxResultsObj) && maxResultsObj != null)
        {
            if (int.TryParse(maxResultsObj.ToString(), out int parsedMaxResults))
            {
                maxResults = parsedMaxResults;
            }
        }

        if (parameters.TryGetValue("case_sensitive", out object? caseSensitiveObj) && caseSensitiveObj != null)
        {
            if (bool.TryParse(caseSensitiveObj.ToString(), out bool parsedCaseSensitive))
            {
                caseSensitive = parsedCaseSensitive;
            }
        }

        var results = new List<string>();
        var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

        try
        {
            SearchContentRecursive(directory, pattern, keyword, comparison, maxResults, results);
        }
        catch (Exception ex)
        {
            if (results.Count == 0)
                return ToolResult.Failed($"Error searching file contents: {ex.Message}");
        }

        if (results.Count == 0)
        {
            return ToolResult.Successful($"No file contents found matching keyword '{keyword}' in {directory}");
        }

        return ToolResult.Successful($"Found {results.Count} matches for keyword '{keyword}':\n" +
                                   string.Join("\n", results.Take(maxResults)));
    }

    /// <summary>
    /// Recursively search file contents, handling UnauthorizedAccessException per directory.
    /// Only searches text and code files, skipping binary files.
    /// </summary>
    private static void SearchContentRecursive(
        string directory, string pattern, string keyword,
        StringComparison comparison, int maxResults, List<string> results)
    {
        if (results.Count >= maxResults) return;

        // 1. Search file contents in current directory
        try
        {
            foreach (var file in Directory.EnumerateFiles(directory, pattern, SearchOption.TopDirectoryOnly))
            {
                if (results.Count >= maxResults) return;
                
                // Skip binary files - only search text and code files
                if (!IsTextFile(file))
                    continue;
                
                try
                {
                    var content = File.ReadAllText(file);
                    var lines = content.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains(keyword, comparison))
                        {
                            results.Add($"{file}:{i + 1}: {lines[i].Trim()}");
                            if (results.Count >= maxResults) return;
                        }
                    }
                }
                catch { /* Skip files we can't read */ }
            }
        }
        catch (UnauthorizedAccessException) { }
        catch (IOException) { }

        // 2. Recurse into subdirectories
        try
        {
            foreach (var subDir in Directory.EnumerateDirectories(directory))
            {
                if (results.Count >= maxResults) return;
                try
                {
                    SearchContentRecursive(subDir, pattern, keyword, comparison, maxResults, results);
                }
                catch (UnauthorizedAccessException) { }
                catch (IOException) { }
            }
        }
        catch (UnauthorizedAccessException) { }
        catch (IOException) { }
    }

    /// <summary>
    /// Check if a file is likely a text/code file based on extension.
    /// </summary>
    private static bool IsTextFile(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLowerInvariant();
        
        // Text and code file extensions
        var textExtensions = new HashSet<string>
        {
            // Code files
            ".cs", ".vb", ".fs", ".cpp", ".c", ".h", ".hpp", ".cxx", ".cc",
            ".java", ".kt", ".scala", ".groovy",
            ".py", ".pyw", ".pyi", ".pyx",
            ".js", ".jsx", ".ts", ".tsx", ".mjs",
            ".html", ".htm", ".xhtml",
            ".css", ".scss", ".sass", ".less",
            ".json", ".xml", ".yaml", ".yml", ".toml", ".ini", ".cfg", ".conf",
            ".md", ".markdown", ".txt", ".log",
            ".sql", ".sh", ".bash", ".zsh", ".ps1", ".bat", ".cmd",
            ".rb", ".php", ".go", ".rs", ".swift", ".m", ".mm",
            ".r", ".R", ".pl", ".pm", ".t", ".lua", ".dart",
            ".vue", ".svelte", ".astro",
            ".dockerfile", ".gitignore", ".editorconfig",
            ".csproj", ".vbproj", ".fsproj", ".sln",
            ".props", ".targets", ".nuspec",
            
            // Config and data files
            ".env", ".properties", ".gradle",
            ".makefile", ".cmake",
            ".graphql", ".proto",
            ".svg", ".xml", ".xaml",
            ".resx", ".licx",
            ".mdx",
            
            // Markup and template files
            ".ejs", ".pug", ".jade", ".hbs", ".mustache",
            ".liquid", ".jinja", ".jinja2",
            ".blade.php", ".twig",
        };

        // Check if file has no extension (could be Dockerfile, Makefile, etc.)
        if (string.IsNullOrEmpty(extension))
        {
            string fileName = Path.GetFileName(filePath).ToLowerInvariant();
            var specialFiles = new HashSet<string>
            {
                "dockerfile", "makefile", "rakefile", "gemfile",
                "vagrantfile", "jenkinsfile", "brewfile",
                ".gitignore", ".dockerignore", ".editorconfig",
                ".env", ".env.example", ".env.local",
                "license", "readme", "changelog",
                "authors", "contributors"
            };
            return specialFiles.Contains(fileName);
        }

        return textExtensions.Contains(extension);
    }
}
