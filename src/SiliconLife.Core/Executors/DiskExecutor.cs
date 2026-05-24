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

namespace SiliconLife.Collective;

using System.Text;

/// <summary>
/// Static executor for file system operations.
/// Provides a safe wrapper for disk IO initiated by AI tools.
/// Permission checking via <see cref="PermissionManager"/> through <see cref="ServiceLocator"/>.
/// </summary>
public static class DiskExecutor
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger(typeof(DiskExecutor));
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Executes a disk operation request synchronously with timeout.
    /// Checks permission via the caller's PermissionManager before executing.
    /// </summary>
    public static ExecutorResult Execute(ExecutorRequest request, TimeSpan? timeout = null)
    {
        if (!CheckPermission(request))
        {
            return ExecutorResult.Failed($"Permission denied: file access to '{request.ResourcePath}'");
        }

        TimeSpan actualTimeout = timeout ?? DefaultTimeout;

        try
        {
            Task<ExecutorResult> task = Task.Run(() => ExecuteCore(request));
            if (task.Wait(actualTimeout))
            {
                return task.Result;
            }
            return ExecutorResult.Failed("Operation timed out");
        }
        catch (AggregateException ex)
        {
            Exception? inner = ex.InnerException;
            return ExecutorResult.Failed(inner?.Message ?? ex.Message);
        }
    }

    private static ExecutorResult ExecuteCore(ExecutorRequest request)
    {
        string path = request.ResourcePath;

        if (string.IsNullOrWhiteSpace(path))
        {
            return ExecutorResult.Failed("Path is empty");
        }

        // Normalize path
        path = Path.GetFullPath(path);

        return request.Type switch
        {
            "read_file" => ExecuteReadFile(path, request.Parameters),
            "write_file" => ExecuteWriteFile(path, request.Parameters),
            "append_file" => ExecuteAppendFile(path, request.Parameters),
            "delete_file" => ExecuteDeleteFile(path),
            "list_directory" => ExecuteListDirectory(path),
            "create_directory" => ExecuteCreateDirectory(path),
            "exists" => ExecuteExists(path),
            "get_file_info" => ExecuteGetFileInfo(path),
            "search_content" => ExecuteSearchContent(path, request.Parameters),
            "search_files" => ExecuteSearchFiles(path, request.Parameters),
            _ => ExecutorResult.Failed($"Unknown disk operation type: {request.Type}")
        };
    }

    private static ExecutorResult ExecuteReadFile(string path, Dictionary<string, object> parameters = null)
    {
        try
        {
            if (!File.Exists(path))
            {
                return ExecutorResult.Failed($"File not found: {path}");
            }

            Encoding encoding = DetectEncoding(path, parameters);
            string content = File.ReadAllText(path, encoding);
            _logger.Info(null, "Disk read: {0}, size={1}, encoding={2}", path, content.Length, encoding.WebName);
            return ExecutorResult.Successful(content);
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Disk read failed: {0}, {1}", ex, path);
            return ExecutorResult.Failed($"Failed to read file: {ex.Message}");
        }
    }

    /// <summary>
    /// Detects file encoding from parameters or file content.
    /// Priority: explicit encoding parameter > BOM detection > UTF-8 default.
    /// </summary>
    private static Encoding DetectEncoding(string path, Dictionary<string, object> parameters)
    {
        // 1. Check explicit encoding parameter
        if (parameters != null && parameters.TryGetValue("encoding", out object? encodingObj))
        {
            string encodingName = encodingObj?.ToString()?.Trim() ?? "";
            if (!string.IsNullOrEmpty(encodingName))
            {
                try
                {
                    return Encoding.GetEncoding(encodingName);
                }
                catch
                {
                    // Fall through to auto-detection
                }
            }
        }

        // 2. Auto-detect from file content (BOM and byte patterns)
        return DetectEncodingFromContent(path);
    }

    /// <summary>
    /// Detects encoding by reading file bytes and checking for BOM or byte patterns.
    /// Falls back to UTF-8 (no BOM) if detection fails.
    /// </summary>
    private static Encoding DetectEncodingFromContent(string path)
    {
        try
        {
            byte[] bytes = File.ReadAllBytes(path);
            if (bytes.Length == 0)
            {
                return Encoding.UTF8; // Empty file, default to UTF-8
            }

            // Check BOM (Byte Order Mark)
            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
            {
                return new UTF8Encoding(true); // UTF-8 with BOM
            }

            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
            {
                return Encoding.Unicode; // UTF-16 LE (UTF-8 little-endian)
            }

            if (bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF)
            {
                return Encoding.BigEndianUnicode; // UTF-16 BE
            }

            if (bytes.Length >= 4 && bytes[0] == 0x00 && bytes[1] == 0x00 && bytes[2] == 0xFE && bytes[3] == 0xFF)
            {
                return Encoding.UTF32; // UTF-32 BE
            }

            // Try to detect GB2312/GBK by byte pattern (common in Chinese Windows)
            // Heuristic: if high byte ratio is significant and valid GBK ranges
            if (MightBeGBK(bytes))
            {
                try
                {
                    Encoding gb2312 = Encoding.GetEncoding("GB2312");
                    // Verify by attempting to decode
                    gb2312.GetString(bytes);
                    return gb2312;
                }
                catch
                {
                    // GB2312 not available, fall through
                }
            }

            // Default: UTF-8 without BOM
            return new UTF8Encoding(false);
        }
        catch
        {
            return Encoding.UTF8; // Fallback
        }
    }

    /// <summary>
    /// Heuristic check if bytes might be GBK/GB2312 encoded.
    /// Looks for byte pairs in valid GBK character ranges.
    /// </summary>
    private static bool MightBeGBK(byte[] bytes)
    {
        int highByteCount = 0;
        int validGBKPairs = 0;
        int sampleSize = Math.Min(bytes.Length, 1024); // Check first 1KB

        for (int i = 0; i < sampleSize; i++)
        {
            if (bytes[i] > 0x7F)
            {
                highByteCount++;
                // Check if this could be a valid GBK lead byte (0x81-0xFE)
                if (bytes[i] >= 0x81 && bytes[i] <= 0xFE && i + 1 < sampleSize)
                {
                    byte trailByte = bytes[i + 1];
                    // GBK trail byte range: 0x40-0x7E or 0x80-0xFE
                    if ((trailByte >= 0x40 && trailByte <= 0x7E) || (trailByte >= 0x80 && trailByte <= 0xFE))
                    {
                        validGBKPairs++;
                        i++; // Skip trail byte
                    }
                }
            }
        }

        // If >20% high bytes and >50% of them form valid GBK pairs, likely GBK
        return highByteCount > sampleSize * 0.2 && validGBKPairs > highByteCount * 0.5;
    }

    private static ExecutorResult ExecuteWriteFile(string path, Dictionary<string, object> parameters)
    {
        try
        {
            if (!parameters.TryGetValue("content", out object? contentObj) || contentObj == null)
            {
                return ExecutorResult.Failed("Missing 'content' parameter");
            }

            string? directory = Path.GetDirectoryName(path);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string content = contentObj.ToString() ?? string.Empty;
            File.WriteAllText(path, content);
            _logger.Info(null, "Disk write: {0}, size={1}", path, content.Length);
            return ExecutorResult.Successful($"File written successfully: {path}");
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Disk write failed: {0}, {1}", ex, path);
            return ExecutorResult.Failed($"Failed to write file: {ex.Message}");
        }
    }

    private static ExecutorResult ExecuteAppendFile(string path, Dictionary<string, object> parameters)
    {
        try
        {
            if (!parameters.TryGetValue("content", out object? contentObj) || contentObj == null)
            {
                return ExecutorResult.Failed("Missing 'content' parameter");
            }

            string? directory = Path.GetDirectoryName(path);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string content = contentObj.ToString() ?? string.Empty;
            File.AppendAllText(path, content);
            _logger.Info(null, "Disk append: {0}, size={1}", path, content.Length);
            return ExecutorResult.Successful($"Content appended to file: {path}");
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Disk append failed: {0}, {1}", ex, path);
            return ExecutorResult.Failed($"Failed to append to file: {ex.Message}");
        }
    }

    private static ExecutorResult ExecuteDeleteFile(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                _logger.Info(null, "Disk delete: {0}", path);
                return ExecutorResult.Successful($"File deleted: {path}");
            }

            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
                _logger.Info(null, "Disk delete directory: {0}", path);
                return ExecutorResult.Successful($"Directory deleted: {path}");
            }

            return ExecutorResult.Failed($"Path not found: {path}");
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Disk delete failed: {0}, {1}", ex, path);
            return ExecutorResult.Failed($"Failed to delete: {ex.Message}");
        }
    }

    private static ExecutorResult ExecuteListDirectory(string path)
    {
        try
        {
            if (!Directory.Exists(path))
            {
                return ExecutorResult.Failed($"Directory not found: {path}");
            }

            var entries = new List<string>();
            foreach (string dir in Directory.GetDirectories(path))
            {
                entries.Add($"[DIR] {Path.GetFileName(dir)}");
            }
            foreach (string file in Directory.GetFiles(path))
            {
                FileInfo fi = new(file);
                entries.Add($"[FILE] {fi.Name} ({fi.Length} bytes)");
            }

            if (entries.Count == 0)
            {
                return ExecutorResult.Successful("(empty directory)");
            }

            string result = string.Join("\n", entries);
            _logger.Info(null, "Disk list directory: {0}, entries={1}", path, entries.Count);
            return ExecutorResult.Successful(result);
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Disk list directory failed: {0}, {1}", ex, path);
            return ExecutorResult.Failed($"Failed to list directory: {ex.Message}");
        }
    }

    private static ExecutorResult ExecuteCreateDirectory(string path)
    {
        try
        {
            Directory.CreateDirectory(path);
            _logger.Info(null, "Disk create directory: {0}", path);
            return ExecutorResult.Successful($"Directory created: {path}");
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Disk create directory failed: {0}, {1}", ex, path);
            return ExecutorResult.Failed($"Failed to create directory: {ex.Message}");
        }
    }

    private static ExecutorResult ExecuteExists(string path)
    {
        bool exists = File.Exists(path) || Directory.Exists(path);
        return ExecutorResult.Successful(exists ? "true" : "false");
    }

    private static ExecutorResult ExecuteGetFileInfo(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                FileInfo fi = new(path);
                string info = $"File: {fi.FullName}\n" +
                    $"Size: {fi.Length} bytes\n" +
                    $"Created: {fi.CreationTime:yyyy-MM-dd HH:mm:ss}\n" +
                    $"Modified: {fi.LastWriteTime:yyyy-MM-dd HH:mm:ss}\n" +
                    $"Attributes: {fi.Attributes}";
                _logger.Info(null, "Disk get file info: {0}, size={1}", path, fi.Length);
                return ExecutorResult.Successful(info);
            }

            if (Directory.Exists(path))
            {
                DirectoryInfo di = new(path);
                int fileCount = di.GetFiles().Length;
                int dirCount = di.GetDirectories().Length;
                string info = $"Directory: {di.FullName}\n" +
                    $"Files: {fileCount}\n" +
                    $"Subdirectories: {dirCount}\n" +
                    $"Created: {di.CreationTime:yyyy-MM-dd HH:mm:ss}\n" +
                    $"Modified: {di.LastWriteTime:yyyy-MM-dd HH:mm:ss}";
                _logger.Info(null, "Disk get directory info: {0}, files={1}, dirs={2}", path, fileCount, dirCount);
                return ExecutorResult.Successful(info);
            }

            return ExecutorResult.Failed($"Path not found: {path}");
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Disk get file info failed: {0}, {1}", ex, path);
            return ExecutorResult.Failed($"Failed to get file info: {ex.Message}");
        }
    }

    /// <summary>
    /// Searches file contents recursively for a keyword.
    /// Parameters: keyword, pattern, max_results, case_sensitive, max_file_size
    /// </summary>
    private static ExecutorResult ExecuteSearchContent(string directory, Dictionary<string, object> parameters)
    {
        try
        {
            if (!parameters.TryGetValue("keyword", out object? keywordObj) || string.IsNullOrWhiteSpace(keywordObj?.ToString()))
            {
                return ExecutorResult.Failed("Missing 'keyword' parameter for search_content");
            }

            if (!Directory.Exists(directory))
            {
                return ExecutorResult.Failed($"Directory not found: {directory}");
            }

            string keyword = keywordObj.ToString()!;
            string pattern = parameters.TryGetValue("pattern", out object? patternObj) && patternObj != null
                ? patternObj.ToString()!
                : "*.*";
            int maxResults = GetIntParameter(parameters, "max_results", 50);
            bool caseSensitive = GetBoolParameter(parameters, "case_sensitive", false);
            long maxFileSize = GetIntParameter(parameters, "max_file_size", 10 * 1024 * 1024); // 10MB default

            var results = new List<string>();
            var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            SearchContentRecursive(directory, pattern, keyword, comparison, maxResults, maxFileSize, results);

            _logger.Info(null, "Disk search_content: dir={0}, keyword={1}, matches={2}", directory, keyword, results.Count);

            if (results.Count == 0)
            {
                return ExecutorResult.Successful($"No file contents found matching keyword '{keyword}' in {directory}");
            }

            return ExecutorResult.Successful($"Found {results.Count} matches for keyword '{keyword}':\n" +
                string.Join("\n", results.Take(maxResults)));
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Disk search_content failed: {0}, {1}", ex, directory);
            return ExecutorResult.Failed($"Failed to search file contents: {ex.Message}");
        }
    }

    /// <summary>
    /// Recursively search file contents, handling UnauthorizedAccessException per directory.
    /// Only searches text and code files, skipping binary files and oversized files.
    /// </summary>
    private static void SearchContentRecursive(
        string directory, string pattern, string keyword,
        StringComparison comparison, int maxResults, long maxFileSize, List<string> results)
    {
        if (results.Count >= maxResults) return;

        // 1. Search file contents in current directory
        try
        {
            foreach (var file in Directory.EnumerateFiles(directory, pattern, SearchOption.TopDirectoryOnly))
            {
                if (results.Count >= maxResults) return;

                // Skip binary files - only search text and code files
                if (!IsTextFile(file)) continue;

                // Skip oversized files
                try
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.Length > maxFileSize) continue;
                }
                catch { continue; }

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
                    SearchContentRecursive(subDir, pattern, keyword, comparison, maxResults, maxFileSize, results);
                }
                catch (UnauthorizedAccessException) { }
                catch (IOException) { }
            }
        }
        catch (UnauthorizedAccessException) { }
        catch (IOException) { }
    }

    /// <summary>
    /// Searches files by name recursively for a keyword.
    /// Parameters: keyword, pattern, max_results, case_sensitive
    /// </summary>
    private static ExecutorResult ExecuteSearchFiles(string directory, Dictionary<string, object> parameters)
    {
        try
        {
            if (!parameters.TryGetValue("keyword", out object? keywordObj) || string.IsNullOrWhiteSpace(keywordObj?.ToString()))
            {
                return ExecutorResult.Failed("Missing 'keyword' parameter for search_files");
            }

            if (!Directory.Exists(directory))
            {
                return ExecutorResult.Failed($"Directory not found: {directory}");
            }

            string keyword = keywordObj.ToString()!;
            string pattern = parameters.TryGetValue("pattern", out object? patternObj) && patternObj != null
                ? patternObj.ToString()!
                : "*.*";
            int maxResults = GetIntParameter(parameters, "max_results", 50);
            bool caseSensitive = GetBoolParameter(parameters, "case_sensitive", false);

            var results = new List<string>();
            var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            SearchFilesRecursive(directory, pattern, keyword, comparison, maxResults, results);

            _logger.Info(null, "Disk search_files: dir={0}, keyword={1}, matches={2}", directory, keyword, results.Count);

            if (results.Count == 0)
            {
                return ExecutorResult.Successful($"No files found matching keyword '{keyword}' in {directory}");
            }

            return ExecutorResult.Successful($"Found {results.Count} files matching keyword '{keyword}':\n" +
                string.Join("\n", results));
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Disk search_files failed: {0}, {1}", ex, directory);
            return ExecutorResult.Failed($"Failed to search files: {ex.Message}");
        }
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

    /// <summary>
    /// Check if a file is likely a text/code file based on extension.
    /// </summary>
    public static bool IsTextFile(string filePath)
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
            ".svg", ".xaml",
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

    /// <summary>
    /// Gets an integer parameter from the parameters dictionary with a default value.
    /// </summary>
    private static int GetIntParameter(Dictionary<string, object> parameters, string key, int defaultValue)
    {
        if (parameters.TryGetValue(key, out object? valueObj) && valueObj != null)
        {
            if (int.TryParse(valueObj.ToString(), out int parsedValue))
            {
                return parsedValue;
            }
        }
        return defaultValue;
    }

    /// <summary>
    /// Gets a boolean parameter from the parameters dictionary with a default value.
    /// </summary>
    private static bool GetBoolParameter(Dictionary<string, object> parameters, string key, bool defaultValue)
    {
        if (parameters.TryGetValue(key, out object? valueObj) && valueObj != null)
        {
            if (bool.TryParse(valueObj.ToString(), out bool parsedValue))
            {
                return parsedValue;
            }
        }
        return defaultValue;
    }

    /// <summary>
    /// Checks permission for a disk operation via the caller's PermissionManager.
    /// </summary>
    private static bool CheckPermission(ExecutorRequest request)
    {
        PermissionManager? pm = ServiceLocator.Instance.GetPermissionManager(request.CallerId);
        if (pm == null) return false; // No manager available, deny by default for security
        return pm.CheckPermission(request.CallerId, PermissionType.FileAccess, request.ResourcePath);
    }
}
