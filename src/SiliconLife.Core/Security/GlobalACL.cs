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
/// A single ACL rule with prefix matching.
/// Rules are evaluated in order; first match wins.
/// </summary>
public sealed class AclRule
{
    /// <summary>The permission type this rule applies to</summary>
    public PermissionType PermissionType { get; }

    /// <summary>The resource prefix to match (case-insensitive)</summary>
    public string ResourcePrefix { get; }

    /// <summary>Whether this rule allows or denies access</summary>
    public PermissionResult Result { get; }

    /// <summary>Optional description for this rule</summary>
    public string? Description { get; set; }

    public AclRule(PermissionType permissionType, string resourcePrefix, PermissionResult result, string? description = null)
    {
        PermissionType = permissionType;
        ResourcePrefix = resourcePrefix;
        Result = result;
        Description = description;
    }
}

/// <summary>
/// Global Access Control List — prefix-matching rule table shared across all silicon beings.
/// Persists to IStorage. Supports user management (add/remove rules).
/// First match wins; no match returns null.
/// </summary>
public class GlobalACL
{
    private readonly List<AclRule> _rules = new();
    private readonly IStorage? _storage;
    private readonly object _lock = new();
    private const string StorageKeyPrefix = "acl/";
    private const string RulesKey = "acl/rules";

    /// <summary>
    /// Gets the number of rules in the ACL
    /// </summary>
    public int RuleCount
    {
        get { lock (_lock) { return _rules.Count; } }
    }

    /// <summary>
    /// Creates a GlobalACL without persistence (in-memory only)
    /// </summary>
    public GlobalACL()
    {
    }

    /// <summary>
    /// Creates a GlobalACL with IStorage persistence
    /// </summary>
    /// <param name="storage">The storage instance for persistence</param>
    public GlobalACL(IStorage storage)
    {
        _storage = storage;
        LoadFromStorage();
    }

    /// <summary>
    /// Queries the ACL for a matching rule.
    /// First prefix match wins. Returns null if no rule matches.
    /// </summary>
    /// <param name="permissionType">The permission type to check</param>
    /// <param name="resource">The resource path to match</param>
    /// <returns>The matched rule's result, or null if no match</returns>
    public PermissionResult? Query(PermissionType permissionType, string resource)
    {
        lock (_lock)
        {
            foreach (AclRule rule in _rules)
            {
                if (rule.PermissionType == permissionType &&
                    resource.StartsWith(rule.ResourcePrefix, StringComparison.OrdinalIgnoreCase))
                {
                    return rule.Result;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Adds a rule to the ACL
    /// </summary>
    /// <param name="rule">The rule to add</param>
    public void AddRule(AclRule rule)
    {
        lock (_lock)
        {
            _rules.Add(rule);
            SaveToStorage();
        }
    }

    /// <summary>
    /// Removes all rules matching the given prefix
    /// </summary>
    /// <param name="permissionType">The permission type</param>
    /// <param name="resourcePrefix">The resource prefix to match</param>
    /// <returns>The number of rules removed</returns>
    public int RemoveRule(PermissionType permissionType, string resourcePrefix)
    {
        int removed;
        lock (_lock)
        {
            removed = _rules.RemoveAll(r =>
                r.PermissionType == permissionType &&
                r.ResourcePrefix.Equals(resourcePrefix, StringComparison.OrdinalIgnoreCase));
            SaveToStorage();
        }
        return removed;
    }

    /// <summary>
    /// Gets all rules (for display / management)
    /// </summary>
    public List<AclRule> GetAllRules()
    {
        lock (_lock)
        {
            return _rules.ToList();
        }
    }

    private void SaveToStorage()
    {
        if (_storage == null) return;

        try
        {
            var ruleData = _rules.Select(r => new Dictionary<string, string>
            {
                ["type"] = r.PermissionType.ToString(),
                ["prefix"] = r.ResourcePrefix,
                ["result"] = r.Result.ToString(),
                ["desc"] = r.Description ?? ""
            }).ToList();

            string json = System.Text.Json.JsonSerializer.Serialize(ruleData);
            _storage.Write(RulesKey, System.Text.Encoding.UTF8.GetBytes(json));
        }
        catch
        {
            // Persistence failure should not crash the permission system
        }
    }

    private void LoadFromStorage()
    {
        if (_storage == null) return;

        try
        {
            byte[]? data = _storage.Read(RulesKey);
            if (data == null) return;

            string json = System.Text.Encoding.UTF8.GetString(data);
            var ruleData = System.Text.Json.JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);
            if (ruleData == null) return;

            foreach (var item in ruleData)
            {
                if (!item.TryGetValue("type", out string? typeStr) ||
                    !item.TryGetValue("prefix", out string? prefix) ||
                    !item.TryGetValue("result", out string? resultStr))
                {
                    continue;
                }

                if (!Enum.TryParse<PermissionType>(typeStr, true, out PermissionType permType))
                    continue;

                if (!Enum.TryParse<PermissionResult>(resultStr, true, out PermissionResult result))
                    continue;

                string? desc = item.TryGetValue("desc", out string? d) ? d : null;
                _rules.Add(new AclRule(permType, prefix, result, desc));
            }
        }
        catch
        {
            // Load failure — start fresh
        }
    }
}
