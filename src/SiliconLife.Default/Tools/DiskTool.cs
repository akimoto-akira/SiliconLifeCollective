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

namespace SiliconLife.Default;

/// <summary>
/// Disk file operations tool.
/// Performs file read/write and directory operations through DiskExecutor.
/// Verifies the disk executor pipeline.
/// </summary>
public class DiskTool : ITool
{
    public string Name => "disk";

    public string Description =>
        "Perform file and directory operations. Actions: read_file, write_file, " +
        "list_directory, delete_file, create_directory, exists, get_file_info.";

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
                    ["enum"] = new[] { "read_file", "write_file", "list_directory", "delete_file", "create_directory", "exists", "get_file_info" }
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
                }
            },
            ["required"] = new[] { "action", "path" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj))
        {
            return ToolResult.Failed("Missing 'action' parameter");
        }

        if (!parameters.TryGetValue("path", out object? pathObj) || string.IsNullOrWhiteSpace(pathObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'path' parameter");
        }

        string action = actionObj?.ToString() ?? "";
        string path = pathObj.ToString()!;

        var requestParams = new Dictionary<string, object>();
        if (parameters.TryGetValue("content", out object? contentObj) && contentObj != null)
        {
            requestParams["content"] = contentObj.ToString()!;
        }

        ExecutorRequest request = new(Guid.Empty, path, action, requestParams);
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
}
