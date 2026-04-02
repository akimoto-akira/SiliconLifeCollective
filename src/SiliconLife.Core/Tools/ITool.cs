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
/// Interface for tools that silicon beings can invoke via AI.
/// Tool parameters are described using JSON Schema format.
/// </summary>
public interface ITool
{
    /// <summary>
    /// Gets the unique name of this tool (used as function name in AI requests)
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets a human-readable description of this tool
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the JSON Schema describing the parameters for this tool.
    /// The schema must have "type": "object" at the top level.
    /// </summary>
    Dictionary<string, object> GetParameterSchema();

    /// <summary>
    /// Executes the tool with the given parameters
    /// </summary>
    /// <param name="callerId">The GUID of the silicon being invoking this tool</param>
    /// <param name="parameters">The parameters provided by the AI</param>
    /// <returns>The result of the tool execution</returns>
    ToolResult Execute(Guid callerId, Dictionary<string, object> parameters);
}
