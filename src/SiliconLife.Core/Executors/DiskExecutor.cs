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

/// <summary>
/// Static executor for file system operations.
/// Provides a safe wrapper for disk IO initiated by AI tools.
/// Permission checking is NOT implemented in Phase 5 (deferred to Phase 6).
/// </summary>
public static class DiskExecutor
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Executes a disk operation request synchronously with timeout
    /// </summary>
    public static ExecutorResult Execute(ExecutorRequest request, TimeSpan? timeout = null)
    {
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
            "read_file" => ExecuteReadFile(path),
            "write_file" => ExecuteWriteFile(path, request.Parameters),
            "append_file" => ExecuteAppendFile(path, request.Parameters),
            "delete_file" => ExecuteDeleteFile(path),
            "list_directory" => ExecuteListDirectory(path),
            "create_directory" => ExecuteCreateDirectory(path),
            "exists" => ExecuteExists(path),
            "get_file_info" => ExecuteGetFileInfo(path),
            _ => ExecutorResult.Failed($"Unknown disk operation type: {request.Type}")
        };
    }

    private static ExecutorResult ExecuteReadFile(string path)
    {
        try
        {
            if (!File.Exists(path))
            {
                return ExecutorResult.Failed($"File not found: {path}");
            }

            string content = File.ReadAllText(path);
            return ExecutorResult.Successful(content);
        }
        catch (Exception ex)
        {
            return ExecutorResult.Failed($"Failed to read file: {ex.Message}");
        }
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

            File.WriteAllText(path, contentObj.ToString());
            return ExecutorResult.Successful($"File written successfully: {path}");
        }
        catch (Exception ex)
        {
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

            File.AppendAllText(path, contentObj.ToString());
            return ExecutorResult.Successful($"Content appended to file: {path}");
        }
        catch (Exception ex)
        {
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
                return ExecutorResult.Successful($"File deleted: {path}");
            }

            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
                return ExecutorResult.Successful($"Directory deleted: {path}");
            }

            return ExecutorResult.Failed($"Path not found: {path}");
        }
        catch (Exception ex)
        {
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

            return ExecutorResult.Successful(string.Join("\n", entries));
        }
        catch (Exception ex)
        {
            return ExecutorResult.Failed($"Failed to list directory: {ex.Message}");
        }
    }

    private static ExecutorResult ExecuteCreateDirectory(string path)
    {
        try
        {
            Directory.CreateDirectory(path);
            return ExecutorResult.Successful($"Directory created: {path}");
        }
        catch (Exception ex)
        {
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
                return ExecutorResult.Successful(info);
            }

            return ExecutorResult.Failed($"Path not found: {path}");
        }
        catch (Exception ex)
        {
            return ExecutorResult.Failed($"Failed to get file info: {ex.Message}");
        }
    }
}
