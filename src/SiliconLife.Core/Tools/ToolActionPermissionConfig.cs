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
/// Configuration model for per-being tool action permissions.
/// Stores which actions within each tool are disabled (denied) for a specific being.
/// 
/// Design: Uses a blacklist approach — actions NOT in the DisabledActions list are allowed.
/// This ensures backward compatibility: an empty config means all actions are allowed,
/// which matches the current behavior before this feature was introduced.
/// 
/// Structure: Dictionary keyed by tool name, value is a set of disabled action names.
/// Only tools with at least one disabled action need an entry.
/// </summary>
public class ToolActionPermissionConfig
{
    /// <summary>
    /// Gets or sets the per-tool disabled action map.
    /// Key: tool name (e.g., "chat", "disk", "memory")
    /// Value: set of disabled action names (e.g., {"delete", "write_file"})
    /// 
    /// If a tool name is not present in this dictionary, all its actions are allowed.
    /// If a tool name is present but the set is empty, all its actions are allowed.
    /// </summary>
    public Dictionary<string, HashSet<string>> DisabledActions { get; set; } = new();

    /// <summary>
    /// Checks whether a specific action on a tool is disabled for this being.
    /// </summary>
    /// <param name="toolName">The tool name</param>
    /// <param name="actionName">The action name within the tool</param>
    /// <returns>True if the action is disabled, false if allowed</returns>
    public bool IsActionDisabled(string toolName, string actionName)
    {
        if (DisabledActions == null) return false;
        if (!DisabledActions.TryGetValue(toolName, out var disabledSet)) return false;
        return disabledSet.Contains(actionName);
    }

    /// <summary>
    /// Checks whether a specific action on a tool is allowed for this being.
    /// </summary>
    /// <param name="toolName">The tool name</param>
    /// <param name="actionName">The action name within the tool</param>
    /// <returns>True if the action is allowed, false if disabled</returns>
    public bool IsActionAllowed(string toolName, string actionName) => !IsActionDisabled(toolName, actionName);

    /// <summary>
    /// Gets all disabled actions for a specific tool.
    /// Returns an empty set if the tool has no disabled actions.
    /// </summary>
    /// <param name="toolName">The tool name</param>
    /// <returns>Set of disabled action names</returns>
    public HashSet<string> GetDisabledActions(string toolName)
    {
        if (DisabledActions == null) return new HashSet<string>();
        return DisabledActions.TryGetValue(toolName, out var set) ? set : new HashSet<string>();
    }

    /// <summary>
    /// Sets an action as disabled for a specific tool.
    /// </summary>
    public void DisableAction(string toolName, string actionName)
    {
        DisabledActions ??= new Dictionary<string, HashSet<string>>();
        if (!DisabledActions.TryGetValue(toolName, out var set))
        {
            set = new HashSet<string>();
            DisabledActions[toolName] = set;
        }
        set.Add(actionName);
    }

    /// <summary>
    /// Sets an action as allowed for a specific tool (removes from disabled list).
    /// </summary>
    public void EnableAction(string toolName, string actionName)
    {
        if (DisabledActions == null) return;
        if (DisabledActions.TryGetValue(toolName, out var set))
        {
            set.Remove(actionName);
            if (set.Count == 0)
            {
                DisabledActions.Remove(toolName);
            }
        }
    }

    /// <summary>
    /// Gets the list of tool names that have at least one disabled action.
    /// </summary>
    public List<string> GetRestrictedToolNames()
    {
        if (DisabledActions == null) return new List<string>();
        return DisabledActions.Where(kvp => kvp.Value.Count > 0).Select(kvp => kvp.Key).ToList();
    }
}
