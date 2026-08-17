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

namespace SiliconLife.Collective;

/// <summary>
/// A tool as reported by an external MCP server via tools/list.
/// The input schema is a JSON Schema with "type": "object" at the top
/// level — the same contract as <see cref="ITool.GetParameterSchema"/>.
/// </summary>
public class McpToolDefinition
{
    /// <summary>Gets or sets the original tool name on the server (unprefixed).</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the tool description reported by the server.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Gets or sets the JSON Schema of the tool parameters (native .NET values).</summary>
    public Dictionary<string, object> InputSchema { get; set; } = new();
}

/// <summary>Outcome of one tools/call invocation on an MCP server.</summary>
public class McpCallResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="McpCallResult"/> class.
    /// </summary>
    /// <param name="isSuccess">Whether the call succeeded.</param>
    /// <param name="textContent">The flattened text content (success case).</param>
    /// <param name="errorMessage">The error message (failure case).</param>
    private McpCallResult(bool isSuccess, string textContent, string? errorMessage)
    {
        IsSuccess = isSuccess;
        TextContent = textContent;
        ErrorMessage = errorMessage;
    }

    /// <summary>Gets a value indicating whether the call succeeded.</summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets the flattened text content: all text items joined by newlines;
    /// image/resource items are represented by placeholder descriptions.
    /// </summary>
    public string TextContent { get; }

    /// <summary>Gets the error message when the call failed.</summary>
    public string? ErrorMessage { get; }

    /// <summary>Creates a successful result.</summary>
    public static McpCallResult Ok(string text) => new(true, text, null);

    /// <summary>Creates a failed result.</summary>
    public static McpCallResult Fail(string error) => new(false, string.Empty, error);
}
