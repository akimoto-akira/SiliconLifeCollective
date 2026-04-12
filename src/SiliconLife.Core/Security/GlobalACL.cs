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

internal sealed class AclRuleData
{
    public PermissionType PermissionType { get; set; }
    public string ResourcePrefix { get; set; } = string.Empty;
    public PermissionResult Result { get; set; }
    public string? Description { get; set; }
}

/// <summary>
/// Global Access Control List — prefix-matching rule table shared across all silicon beings.
/// Persists to IStorage. Supports user management (add/remove rules).
/// First match wins; no match returns null.
/// </summary>
public class GlobalACL
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<GlobalACL>();
    private readonly List<AclRule> _rules = new();
    private readonly IStorage? _storage;
    private readonly object _lock = new();
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
        _logger.Info("GlobalACL initialized with storage, loaded {0} rules", _rules.Count);
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
                    _logger.Debug("ACL match: type={0}, prefix={1}, result={2}", permissionType, rule.ResourcePrefix, rule.Result);
                    return rule.Result;
                }
            }
        }
        _logger.Trace("ACL no match: type={0}, resource={1}", permissionType, resource);
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
            _logger.Info("ACL rule added: type={0}, prefix={1}, result={2}", rule.PermissionType, rule.ResourcePrefix, rule.Result);
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
            _logger.Info("ACL rule removed: type={0}, prefix={1}, count={2}", permissionType, resourcePrefix, removed);
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
            var ruleData = _rules.Select(r => new AclRuleData
            {
                PermissionType = r.PermissionType,
                ResourcePrefix = r.ResourcePrefix,
                Result = r.Result,
                Description = r.Description
            }).ToList();

            _storage.Write(RulesKey, ruleData);
        }
        catch (Exception ex)
        {
            _logger.Warn("Failed to persist ACL rules: {0}", ex.Message);
        }
    }

    private void LoadFromStorage()
    {
        if (_storage == null) return;

        try
        {
            List<AclRuleData>? ruleData = _storage.Read<List<AclRuleData>>(RulesKey);
            if (ruleData == null) return;

            foreach (var item in ruleData)
            {
                _rules.Add(new AclRule(item.PermissionType, item.ResourcePrefix, item.Result, item.Description));
            }
        }
        catch (Exception ex)
        {
            _logger.Warn("Failed to load ACL rules from storage: {0}", ex.Message);
        }
    }
}
