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
/// Result of a tool execution
/// </summary>
public class ToolResult
{
    /// <summary>
    /// Gets whether the tool execution was successful
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Gets the result message (may be displayed to the user)
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets optional structured data returned by the tool
    /// </summary>
    public object? Data { get; }

    private ToolResult(bool success, string message, object? data)
    {
        Success = success;
        Message = message;
        Data = data;
    }

    /// <summary>
    /// Creates a successful tool result
    /// </summary>
    public static ToolResult Successful(string message, object? data = null)
    {
        return new ToolResult(true, message, data);
    }

    /// <summary>
    /// Creates a failed tool result
    /// </summary>
    public static ToolResult Failed(string message)
    {
        return new ToolResult(false, message, null);
    }
}
