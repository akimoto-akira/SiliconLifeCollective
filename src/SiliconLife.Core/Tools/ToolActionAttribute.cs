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
/// Declares the enumerable actions that a tool supports.
/// Used by the ToolAction permission framework to enable fine-grained
/// Action-level access control per silicon being.
/// 
/// When this attribute is present on a tool class, the ToolManager can:
/// 1) Filter the "action" parameter's enum list in GetToolDefinitions()
///    based on per-being permission configuration.
/// 2) Validate at runtime in ExecuteTool() that the being is allowed
///    to invoke the requested action.
/// 
/// If no ToolActionAttribute is present on a tool, all actions are
/// considered allowed (backward compatible).
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ToolActionAttribute : Attribute
{
    /// <summary>
    /// Gets the action names that this tool declares.
    /// These names correspond to the valid values of the "action" parameter
    /// in the tool's parameter schema.
    /// </summary>
    public string[] Actions { get; }

    /// <summary>
    /// Creates a new ToolActionAttribute with the specified action names.
    /// </summary>
    /// <param name="actions">The action names declared by this tool</param>
    public ToolActionAttribute(params string[] actions)
    {
        Actions = actions ?? Array.Empty<string>();
    }
}
