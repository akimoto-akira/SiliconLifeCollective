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
/// Workflow template definition, describing a complete state machine flow.
/// </summary>
public class WorkflowTemplate
{
    /// <summary>
    /// Gets or sets the unique template name (e.g., "CodeReview", "PluginOnboarding").
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Gets or sets the template description (for UI display).
    /// </summary>
    public string Description { get; set; } = "";

    /// <summary>
    /// Gets or sets the list of all possible states.
    /// </summary>
    public List<string> States { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of state transition rules.
    /// </summary>
    public List<Transition> Transitions { get; set; } = new();

    /// <summary>
    /// Gets the initial state (defaults to States[0]).
    /// </summary>
    public string InitialState => States.FirstOrDefault() ?? "";

    /// <summary>
    /// Gets or sets the list of terminal states (workflow ends when reaching these states).
    /// </summary>
    public List<string> TerminalStates { get; set; } = new();

    /// <summary>
    /// Gets or sets the role definitions required by this workflow template.
    /// Key: role name, Value: role definition with description and staffing requirements.
    /// Workflows that require specific functional roles (e.g., POIClassifier, Translator)
    /// declare them here. The role pool in ProjectSpace must satisfy these requirements
    /// for the workflow to proceed effectively.
    /// </summary>
    public Dictionary<string, RoleDefinition> RoleDefinitions { get; set; } = new();

    /// <summary>
    /// Gets or sets the template metadata (extension fields for plugin use).
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Validates whether the given role assignments satisfy all role definitions in this template.
    /// </summary>
    /// <param name="roleAssignments">
    /// A dictionary mapping role names to lists of assigned being GUIDs
    /// (typically from ProjectSpace.RoleAssignments).
    /// </param>
    /// <param name="unsatisfiedRoles">
    /// When this method returns, contains the list of role names that are not satisfied,
    /// with descriptions of why they fail (understaffed or overstaffed).
    /// </param>
    /// <returns>True if all role definitions are satisfied; false otherwise.</returns>
    public bool ValidateRoleAssignments(
        Dictionary<string, List<Guid>> roleAssignments,
        out List<string> unsatisfiedRoles)
    {
        unsatisfiedRoles = new List<string>();

        foreach (var kvp in RoleDefinitions)
        {
            string roleName = kvp.Key;
            var roleDef = kvp.Value;

            int assignedCount = 0;
            if (roleAssignments.TryGetValue(roleName, out var beings))
            {
                assignedCount = beings.Count;
            }

            if (!roleDef.IsSatisfied(assignedCount))
            {
                unsatisfiedRoles.Add($"{roleName}: {roleDef.GetStatusText(assignedCount)}");
            }
        }

        return unsatisfiedRoles.Count == 0;
    }
}
