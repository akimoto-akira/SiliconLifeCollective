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
        "Manage permissions for silicon beings. Actions: query_permission (check permission status for a specific resource), " +
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
                    ["description"] = "Action to perform: query_permission, manage_acl",
                    ["enum"] = new[] { "query_permission", "manage_acl" }
                },
                ["being_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Target silicon being ID (empty string means current being)"
                },
                ["permission_type"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Permission type: network, command, filesystem, function, data (for query_permission action)"
                },
                ["resource"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Resource to check permission for (URL, file path, command, etc.) (for query_permission action)"
                },
                ["acl_action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "ACL action: add_rule, remove_rule, list_rules (for manage_acl action)",
                    ["enum"] = new[] { "add_rule", "remove_rule", "list_rules" }
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
            "manage_acl" => ExecuteManageAcl(callerId, parameters),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    private ToolResult ExecuteQueryPermission(Guid callerId, Dictionary<string, object> parameters)
    {
        string beingIdStr = parameters.TryGetValue("being_id", out object? beingIdObj) && beingIdObj != null
            ? beingIdObj.ToString()!
            : callerId.ToString();

        if (!parameters.TryGetValue("permission_type", out object? permTypeObj) || string.IsNullOrWhiteSpace(permTypeObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'permission_type' parameter for query_permission action. Valid types: network, command, filesystem, function, data");
        }

        if (!parameters.TryGetValue("resource", out object? resourceObj) || string.IsNullOrWhiteSpace(resourceObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'resource' parameter for query_permission action");
        }

        string permTypeStr = permTypeObj.ToString()!.ToLowerInvariant();
        string resource = resourceObj.ToString()!;

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

            // Parse permission type
            PermissionType permType = permTypeStr switch
            {
                "network" => PermissionType.NetworkAccess,
                "command" => PermissionType.CommandLine,
                "filesystem" or "file" => PermissionType.FileAccess,
                "function" => PermissionType.Function,
                "data" => PermissionType.DataAccess,
                _ => throw new ArgumentException($"Unknown permission type: {permTypeStr}. Valid types: network, command, filesystem, function, data")
            };

            // Get the PermissionManager for this being
            var permManager = ServiceLocator.Instance.GetPermissionManager(beingId);
            if (permManager == null)
            {
                return ToolResult.Failed($"Permission manager not found for being: {beingId}");
            }

            // Resolve the actual tested path for filesystem/command resources
            string resolvedResource = resource;
            if (permType == PermissionType.FileAccess)
            {
                try { resolvedResource = Path.GetFullPath(resource); } catch { /* keep original */ }
            }

            // Evaluate permission using the actual permission system (returns three-state result)
            PermissionResult result = permManager.EvaluatePermission(beingId, permType, resource);
            
            // Get additional context about the decision
            bool isCurator = beingId == (Config.Instance?.Data?.CuratorGuid ?? Guid.Empty);
            
            // Check frequency cache for context
            PermissionResult? cachedResult = permManager.FrequencyCache.Query(permType, resource);
            string cacheInfo = cachedResult.HasValue ? $"Cached: {cachedResult.Value}" : "Not cached";

            // Build result message with clear guidance for AI
            string statusMessage = result switch
            {
                PermissionResult.Allowed => "ALLOWED - You can proceed with this operation",
                PermissionResult.Denied => "DENIED - This operation is blocked. Do not attempt.",
                PermissionResult.AskUser => "ASK_USER - This operation requires user confirmation. Please ask the user for permission before proceeding.",
                _ => "UNKNOWN"
            };

            // Include resolved path when it differs from input (helps AI understand path resolution)
            string resolvedInfo = resolvedResource != resource
                ? $"Resolved Resource: {resolvedResource}\n"
                : "";

            return ToolResult.Successful($"Permission evaluation for being '{beingId}':\n" +
                                       $"Permission Type: {permType}\n" +
                                       $"Resource: {resource}\n" +
                                       resolvedInfo +
                                       $"Result: {result}\n" +
                                       $"Status: {statusMessage}\n" +
                                       $"Is Curator: {isCurator}\n" +
                                       $"Frequency Cache: {cacheInfo}");
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Permission query failed: {ex.Message}");
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
