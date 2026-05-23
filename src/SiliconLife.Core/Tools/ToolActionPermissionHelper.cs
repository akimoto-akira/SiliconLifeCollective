// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy at
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
/// Helper for computing effective tool action permissions by merging
/// global (being-level) and project-level configurations.
/// 
/// Merge rule: EffectiveDisabled = GlobalDisabled ∪ ProjectDisabled
/// (The intersection of restrictions — both sources can only ADD restrictions,
/// never remove them. This ensures project-level cannot override global to allow
/// something that was globally denied.)
/// </summary>
public static class ToolActionPermissionHelper
{
    /// <summary>
    /// Computes the effective tool action permission config for a being
    /// within a specific project context.
    /// </summary>
    /// <param name="globalPermissions">The being's global tool action permissions (null = all allowed)</param>
    /// <param name="projectPermissions">The project-level tool action permissions (null = no project restrictions)</param>
    /// <returns>Merged ToolActionPermissionConfig representing the effective permissions</returns>
    public static ToolActionPermissionConfig MergePermissions(
        ToolActionPermissionConfig? globalPermissions,
        ToolActionPermissionConfig? projectPermissions)
    {
        var result = new ToolActionPermissionConfig();

        // Start with global disabled actions
        if (globalPermissions != null)
        {
            foreach (var toolName in globalPermissions.GetRestrictedToolNames())
            {
                var disabledSet = globalPermissions.GetDisabledActions(toolName);
                foreach (var action in disabledSet)
                {
                    result.DisableAction(toolName, action);
                }
            }
        }

        // Add project-level disabled actions (union of restrictions)
        if (projectPermissions != null)
        {
            foreach (var toolName in projectPermissions.GetRestrictedToolNames())
            {
                var disabledSet = projectPermissions.GetDisabledActions(toolName);
                foreach (var action in disabledSet)
                {
                    result.DisableAction(toolName, action);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Gets the effective permissions for a being, optionally merging with
    /// project-level permissions if a project context is available.
    /// </summary>
    /// <param name="being">The silicon being</param>
    /// <param name="projectId">Optional project ID for project-level filtering</param>
    /// <returns>The effective ToolActionPermissionConfig</returns>
    public static ToolActionPermissionConfig GetEffectivePermissions(SiliconBeingBase being, Guid? projectId)
    {
        if (projectId == null || projectId == Guid.Empty)
        {
            // No project context, use global permissions directly
            return being.ToolActionPermissions ?? new ToolActionPermissionConfig();
        }

        // Get project-level permissions
        var projectManager = ServiceLocator.Instance.ProjectManager;
        ProjectSpace? project = projectManager?.GetProject(projectId.Value);
        ToolActionPermissionConfig? projectPermissions = project?.ToolActionPermissions;

        return MergePermissions(being.ToolActionPermissions, projectPermissions);
    }
}
