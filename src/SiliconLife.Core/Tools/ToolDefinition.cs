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
/// Definition of a tool, describing its name, description, and parameter schema
/// for inclusion in AI requests
/// </summary>
public class ToolDefinition
{
    /// <summary>
    /// Gets the tool name (used as function name)
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the tool description
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the JSON Schema describing the tool parameters
    /// </summary>
    public Dictionary<string, object> Parameters { get; }

    /// <summary>
    /// Creates a new tool definition
    /// </summary>
    public ToolDefinition(string name, string description, Dictionary<string, object> parameters)
    {
        Name = name;
        Description = description;
        Parameters = parameters;
    }
}
