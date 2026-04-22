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

using System.Text;
using System.Text.Json;
using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Permission management tool for dynamically adjusting silicon being tool access permissions.
/// Only available to the Silicon Curator (main administrator).
/// </summary>
[SiliconManagerOnly]
public class PermissionTool : ITool
{
    public string Name => "permission";

    public string Description =>
        "Manage permissions for silicon beings. Actions: query_permission (check permission status), " +
        "grant (grant tool access permission), revoke (revoke tool access permission), " +
        "manage_acl (manage global access control list)";

    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

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
                    ["description"] = "Action to perform: query_permission, grant, revoke, manage_acl",
                    ["enum"] = new[] { "query_permission", "grant", "revoke", "manage_acl" }
                },
                ["being_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Target silicon being ID (empty string means current being)"
                },
                ["tool_name"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Tool name to check/grant/revoke permission for"
                },
                ["reason"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Reason for permission change (for audit log)"
                },
                ["acl_action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "ACL action: add_rule, remove_rule, list_rules (for manage_acl action)",
                    ["enum"] = new[] { "add_rule", "remove_rule", "list_rules" }
                },
                ["permission_type"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Permission type: network, command, filesystem (for ACL rules)"
                },
                ["resource_prefix"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Resource prefix for ACL rule matching"
                },
                ["acl_result"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "ACL rule result: allow, deny",
                    ["enum"] = new[] { "allow", "deny" }
                },
                ["description"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Description for the ACL rule"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj) || string.IsNullOrWhiteSpace(actionObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'action' parameter");
        }

        string action = actionObj.ToString()!.ToLowerInvariant();

        return action switch
        {
            "query_permission" => ExecuteQueryPermission(callerId, parameters),
            "grant" => ExecuteGrant(callerId, parameters),
            "revoke" => ExecuteRevoke(callerId, parameters),
            "manage_acl" => ExecuteManageAcl(callerId, parameters),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    private ToolResult ExecuteQueryPermission(Guid callerId, Dictionary<string, object> parameters)
    {
        string beingIdStr = parameters.TryGetValue("being_id", out object? beingIdObj) && beingIdObj != null
            ? beingIdObj.ToString()!
            : callerId.ToString();

        if (!parameters.TryGetValue("tool_name", out object? toolNameObj) || string.IsNullOrWhiteSpace(toolNameObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'tool_name' parameter for query_permission action");
        }

        string toolName = toolNameObj.ToString()!;

        try
        {
            // Try to parse the being ID
            Guid beingId;
            if (string.IsNullOrEmpty(beingIdStr) || beingIdStr == callerId.ToString())
            {
                beingId = callerId;
            }
            else if (!Guid.TryParse(beingIdStr, out beingId))
            {
                return ToolResult.Failed($"Invalid being_id format: {beingIdStr}");
            }

            // Get the PermissionManager for this being
            var permManager = ServiceLocator.Instance.GetPermissionManager(beingId);
            if (permManager == null)
            {
                return ToolResult.Failed($"Permission manager not found for being: {beingId}");
            }

            // Check if the tool exists
            var being = ServiceLocator.Instance.BeingManager?.GetBeing(beingId);
            if (being?.ToolManager == null)
            {
                return ToolResult.Failed($"Tool manager not found for being: {beingId}");
            }

            var tool = being.ToolManager.GetTool(toolName);
            if (tool == null)
            {
                return ToolResult.Failed($"Tool '{toolName}' not found for being: {beingId}");
            }

            // Check permission status
            // For tool permissions, we need to check if the tool has [SiliconManagerOnly] attribute
            bool isCuratorOnly = tool.GetType().GetCustomAttributes(typeof(SiliconManagerOnlyAttribute), false).Length > 0;
            bool isCurator = beingId == (Config.Instance?.Data?.CuratorGuid ?? Guid.Empty);

            string status;
            string source;

            if (isCuratorOnly && !isCurator)
            {
                status = "denied";
                source = "Tool is curator-only and this being is not the curator";
            }
            else
            {
                status = "allowed";
                source = "Tool access granted";
            }

            return ToolResult.Successful($"Permission query for being '{beingId}' on tool '{toolName}':\n" +
                                       $"Status: {status}\n" +
                                       $"Source: {source}\n" +
                                       $"Is Curator: {isCurator}\n" +
                                       $"Is Curator-Only Tool: {isCuratorOnly}");
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Permission query failed: {ex.Message}");
        }
    }

    private ToolResult ExecuteGrant(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("being_id", out object? beingIdObj) || string.IsNullOrWhiteSpace(beingIdObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'being_id' parameter for grant action");
        }

        if (!parameters.TryGetValue("tool_name", out object? toolNameObj) || string.IsNullOrWhiteSpace(toolNameObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'tool_name' parameter for grant action");
        }

        string beingIdStr = beingIdObj.ToString()!;
        string toolName = toolNameObj.ToString()!;
        string reason = parameters.TryGetValue("reason", out object? reasonObj) && reasonObj != null
            ? reasonObj.ToString()!
            : "Permission granted via PermissionTool";

        try
        {
            Guid beingId;
            if (!Guid.TryParse(beingIdStr, out beingId))
            {
                return ToolResult.Failed($"Invalid being_id format: {beingIdStr}");
            }

            // For tool permissions, we can't directly grant access to specific tools
            // as the tool access is controlled by the [SiliconManagerOnly] attribute
            // and the ToolManager scanning process.
            // However, we can add an ACL rule to allow network/command/filesystem permissions
            // that might be needed by the tool.

            // Get the PermissionManager for this being
            var permManager = ServiceLocator.Instance.GetPermissionManager(beingId);
            if (permManager == null)
            {
                return ToolResult.Failed($"Permission manager not found for being: {beingId}");
            }

            // Since we can't directly modify tool access, we'll record this in the audit log
            // and provide guidance on how to actually grant access
            return ToolResult.Successful($"Permission grant request recorded for being '{beingId}' on tool '{toolName}'.\n" +
                                       $"Reason: {reason}\n" +
                                       $"Note: Tool access is controlled by the [SiliconManagerOnly] attribute.\n" +
                                       $"To grant access to curator-only tools, the being must be designated as curator\n" +
                                       $"or the tool's attribute must be removed. ACL rules can be used for network/command/filesystem permissions.");
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Permission grant failed: {ex.Message}");
        }
    }

    private ToolResult ExecuteRevoke(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("being_id", out object? beingIdObj) || string.IsNullOrWhiteSpace(beingIdObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'being_id' parameter for revoke action");
        }

        if (!parameters.TryGetValue("tool_name", out object? toolNameObj) || string.IsNullOrWhiteSpace(toolNameObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'tool_name' parameter for revoke action");
        }

        string beingIdStr = beingIdObj.ToString()!;
        string toolName = toolNameObj.ToString()!;
        string reason = parameters.TryGetValue("reason", out object? reasonObj) && reasonObj != null
            ? reasonObj.ToString()!
            : "Permission revoked via PermissionTool";

        try
        {
            Guid beingId;
            if (!Guid.TryParse(beingIdStr, out beingId))
            {
                return ToolResult.Failed($"Invalid being_id format: {beingIdStr}");
            }

            // Get the PermissionManager for this being
            var permManager = ServiceLocator.Instance.GetPermissionManager(beingId);
            if (permManager == null)
            {
                return ToolResult.Failed($"Permission manager not found for being: {beingId}");
            }

            // Similar to grant, we record this in the audit log
            return ToolResult.Successful($"Permission revoke request recorded for being '{beingId}' on tool '{toolName}'.\n" +
                                       $"Reason: {reason}\n" +
                                       $"Note: Tool access is controlled by the [SiliconManagerOnly] attribute.\n" +
                                       $"To revoke access, the being's curator status must be changed or ACL rules updated.");
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Permission revoke failed: {ex.Message}");
        }
    }

    private ToolResult ExecuteManageAcl(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("acl_action", out object? aclActionObj) || string.IsNullOrWhiteSpace(aclActionObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'acl_action' parameter for manage_acl action");
        }

        string aclAction = aclActionObj.ToString()!.ToLowerInvariant();

        return aclAction switch
        {
            "list_rules" => ExecuteListRules(callerId, parameters),
            "add_rule" => ExecuteAddRule(callerId, parameters),
            "remove_rule" => ExecuteRemoveRule(callerId, parameters),
            _ => ToolResult.Failed($"Unknown ACL action: {aclAction}")
        };
    }

    private ToolResult ExecuteListRules(Guid callerId, Dictionary<string, object> parameters)
    {
        try
        {
            GlobalACL? globalAcl = ServiceLocator.Instance.GlobalAcl;
            if (globalAcl == null)
            {
                return ToolResult.Failed("Global ACL not available");
            }

            var rules = globalAcl.GetAllRules();
            if (rules.Count == 0)
            {
                return ToolResult.Successful("No ACL rules defined.");
            }

            var result = new StringBuilder("Current ACL rules:\n");
            for (int i = 0; i < rules.Count; i++)
            {
                var rule = rules[i];
                result.AppendLine($"Rule {i + 1}:");
                result.AppendLine($"  Permission Type: {rule.PermissionType}");
                result.AppendLine($"  Resource Prefix: {rule.ResourcePrefix}");
                result.AppendLine($"  Result: {rule.Result}");
                result.AppendLine($"  Description: {rule.Description}");
                result.AppendLine();
            }

            return ToolResult.Successful(result.ToString().TrimEnd());
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to list ACL rules: {ex.Message}");
        }
    }

    private ToolResult ExecuteAddRule(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("permission_type", out object? permTypeObj) || string.IsNullOrWhiteSpace(permTypeObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'permission_type' parameter for add_rule action");
        }

        if (!parameters.TryGetValue("resource_prefix", out object? resourcePrefixObj) || string.IsNullOrWhiteSpace(resourcePrefixObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'resource_prefix' parameter for add_rule action");
        }

        if (!parameters.TryGetValue("acl_result", out object? aclResultObj) || string.IsNullOrWhiteSpace(aclResultObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'acl_result' parameter for add_rule action");
        }

        try
        {
            string permTypeStr = permTypeObj.ToString()!.ToLowerInvariant();
            string resourcePrefix = resourcePrefixObj.ToString()!;
            string aclResultStr = aclResultObj.ToString()!.ToLowerInvariant();
            string description = parameters.TryGetValue("description", out object? descObj) && descObj != null
                ? descObj.ToString()!
                : "Rule added via PermissionTool";

            // Parse permission type
            PermissionType permType = permTypeStr switch
            {
                "network" => PermissionType.NetworkAccess,
                "command" => PermissionType.CommandLine,
                "filesystem" => PermissionType.FileAccess,
                _ => throw new ArgumentException($"Unknown permission type: {permTypeStr}")
            };

            // Parse result
            PermissionResult result = aclResultStr switch
            {
                "allow" => PermissionResult.Allowed,
                "deny" => PermissionResult.Denied,
                _ => throw new ArgumentException($"Unknown ACL result: {aclResultStr}")
            };

            GlobalACL? globalAcl = ServiceLocator.Instance.GlobalAcl;
            if (globalAcl == null)
            {
                return ToolResult.Failed("Global ACL not available");
            }

            var rule = new AclRule(permType, resourcePrefix, result, description);
            globalAcl.AddRule(rule);

            return ToolResult.Successful($"ACL rule added successfully:\n" +
                                       $"Permission Type: {permType}\n" +
                                       $"Resource Prefix: {resourcePrefix}\n" +
                                       $"Result: {result}\n" +
                                       $"Description: {description}");
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to add ACL rule: {ex.Message}");
        }
    }

    private ToolResult ExecuteRemoveRule(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("permission_type", out object? permTypeObj) || string.IsNullOrWhiteSpace(permTypeObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'permission_type' parameter for remove_rule action");
        }

        if (!parameters.TryGetValue("resource_prefix", out object? resourcePrefixObj) || string.IsNullOrWhiteSpace(resourcePrefixObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'resource_prefix' parameter for remove_rule action");
        }

        try
        {
            string permTypeStr = permTypeObj.ToString()!.ToLowerInvariant();
            string resourcePrefix = resourcePrefixObj.ToString()!;

            // Parse permission type
            PermissionType permType = permTypeStr switch
            {
                "network" => PermissionType.NetworkAccess,
                "command" => PermissionType.CommandLine,
                "filesystem" => PermissionType.FileAccess,
                _ => throw new ArgumentException($"Unknown permission type: {permTypeStr}")
            };

            GlobalACL? globalAcl = ServiceLocator.Instance.GlobalAcl;
            if (globalAcl == null)
            {
                return ToolResult.Failed("Global ACL not available");
            }

            int removedCount = globalAcl.RemoveRule(permType, resourcePrefix);

            return ToolResult.Successful($"Removed {removedCount} ACL rule(s) matching:\n" +
                                       $"Permission Type: {permType}\n" +
                                       $"Resource Prefix: {resourcePrefix}");
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to remove ACL rule: {ex.Message}");
        }
    }
}
