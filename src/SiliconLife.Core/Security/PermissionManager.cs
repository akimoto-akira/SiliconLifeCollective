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
/// Permission manager for a silicon being.
/// Each being holds its own PermissionManager instance.
///
/// Query priority:
/// 1. UserFrequencyCache â€?high-frequency user decisions (memory-only)
/// 2. IPermissionCallback â€?domain-specific callback rules
/// 3. If callback returns AskUser (or no callback):
///    - Curator â†?IPermissionAskHandler (IM inquiry)
///    - Non-curator â†?GlobalACL â†?default deny
/// </summary>
public class PermissionManager
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<PermissionManager>();
    private readonly SiliconBeingBase _owner;
    private readonly UserFrequencyCache _frequencyCache;
    private readonly GlobalACL _globalAcl;
    private IPermissionCallback? _callback;
    private readonly IPermissionAskHandler? _askHandler;
    private readonly IPermissionCallback? _defaultCallback;

    /// <summary>
    /// Gets the owner being's GUID (computed in real-time from the owner)
    /// </summary>
    public Guid OwnerId => _owner.Id;

    /// <summary>
    /// Gets whether the owner is a curator (computed in real-time from the owner)
    /// </summary>
    public bool IsCurator => _owner.IsCurator;

    /// <summary>
    /// Gets the frequency cache for external access
    /// </summary>
    public UserFrequencyCache FrequencyCache => _frequencyCache;

    /// <summary>
    /// Gets the global ACL for external access
    /// </summary>
    public GlobalACL GlobalAcl => _globalAcl;

    /// <summary>
    /// Gets the current permission callback (may be custom or default).
    /// Used for state migration.
    /// </summary>
    public IPermissionCallback? CustomCallback => _callback;

    /// <summary>
    /// Gets the current ask handler.
    /// Used for state migration.
    /// </summary>
    public IPermissionAskHandler? AskHandler => _askHandler;

    /// <summary>
    /// Creates a new PermissionManager for a silicon being.
    /// OwnerId and IsCurator are computed in real-time from the owner being.
    /// </summary>
    /// <param name="owner">The silicon being that owns this PermissionManager</param>
    /// <param name="globalAcl">The global ACL (shared across beings)</param>
    /// <param name="callback">Optional domain-specific permission callback</param>
    /// <param name="askHandler">Optional user-ask handler for AskUser responses</param>
    /// <param name="cacheExpiration">Expiration time for frequency cache entries (default: 1 hour)</param>
    public PermissionManager(
        SiliconBeingBase owner,
        GlobalACL globalAcl,
        IPermissionCallback? callback = null,
        IPermissionAskHandler? askHandler = null,
        TimeSpan? cacheExpiration = null)
    {
        _owner = owner;
        _globalAcl = globalAcl;
        _callback = callback;
        _defaultCallback = callback;
        _askHandler = askHandler;
        _frequencyCache = new UserFrequencyCache(cacheExpiration ?? TimeSpan.FromHours(1));
        _logger.Info(_owner.Id, "PermissionManager created for being {0} ({1})", owner.Name, owner.Id);
    }

    /// <summary>
    /// Replaces the permission callback with a custom compiled one.
    /// Used by the stealth channel (DynamicCompilationExecutor â†?Curator â†?this).
    /// </summary>
    /// <param name="customCallback">The custom permission callback</param>
    public void SetCustomCallback(IPermissionCallback customCallback)
    {
        _callback = customCallback ?? throw new ArgumentNullException(nameof(customCallback));
        _logger.Info(_owner.Id, "Custom permission callback set for being {0}", OwnerId);
    }

    /// <summary>
    /// Resets the permission callback to the original default.
    /// </summary>
    public void ResetCallback()
    {
        _callback = _defaultCallback;
        _logger.Info(_owner.Id, "Permission callback reset to default for being {0}", OwnerId);
    }

    /// <summary>
    /// Checks whether a permission request is allowed.
    /// Follows the priority chain:
    /// 1. UserFrequencyCache â€?high-frequency user decisions (memory-only)
    /// 2. IPermissionCallback â€?domain-specific callback rules
    /// 3. If callback returns AskUser (or no callback):
    ///    - Curator â†?IPermissionAskHandler (IM inquiry)
    ///    - Non-curator â†?GlobalACL â†?default deny
    /// </summary>
    /// <param name="callerId">The GUID of the silicon being making the request</param>
    /// <param name="permissionType">The type of permission being checked</param>
    /// <param name="resource">The resource path (URL, file path, command, etc.)</param>
    /// <returns>True if the operation is allowed, false otherwise</returns>
    public bool CheckPermission(Guid callerId, PermissionType permissionType, string resource)
    {
        // Priority 1: Check frequency cache (high-frequency user decisions)
        PermissionResult? cachedResult = _frequencyCache.Query(permissionType, resource);
        if (cachedResult.HasValue)
        {
            AuditLog(callerId, permissionType, resource, cachedResult.Value, "Frequency cache");
            return cachedResult.Value == PermissionResult.Allowed;
        }

        // Priority 2: Callback
        if (_callback != null)
        {
            PermissionResult callbackResult = _callback.Evaluate(callerId, permissionType, resource);
            if (callbackResult != PermissionResult.AskUser)
            {
                AuditLog(callerId, permissionType, resource, callbackResult, "Callback");
                return callbackResult == PermissionResult.Allowed;
            }
        }

        // Priority 3: Branch based on curator status when callback returns AskUser or no callback
        if (IsCurator)
        {
            // Curator: ask user via IM
            if (_askHandler != null)
            {
                AskPermissionResult userDecision = _askHandler.AskUser(callerId, permissionType, resource);

                PermissionResult userResult = userDecision.Allowed ? PermissionResult.Allowed : PermissionResult.Denied;
                _frequencyCache.Record(permissionType, resource, userResult, userDecision.AddToCache, userDecision.CacheDuration);

                AuditLog(callerId, permissionType, resource, userResult, "User decision (curator)");
                return userDecision.Allowed;
            }

            AuditLog(callerId, permissionType, resource, PermissionResult.Denied, "No ask handler for curator, default deny");
            return false;
        }

        // Non-curator: check Global ACL
        PermissionResult? aclResult = _globalAcl.Query(permissionType, resource);
        if (aclResult.HasValue)
        {
            AuditLog(callerId, permissionType, resource, aclResult.Value, "Global ACL");
            return aclResult.Value == PermissionResult.Allowed;
        }

        // No matching rule â€?deny by default
        AuditLog(callerId, permissionType, resource, PermissionResult.Denied, "No matching rule, default deny");
        return false;
    }

    /// <summary>
    /// Evaluates a permission request and returns the three-state result without triggering user prompts.
    /// This is a read-only pre-evaluation: it checks caches, callbacks, and ACL rules but does NOT
    /// invoke IPermissionAskHandler. Use this when you need to know the permission state upfront
    /// (e.g., to inform the AI whether user confirmation will be needed).
    /// </summary>
    /// <param name="callerId">The GUID of the silicon being making the request</param>
    /// <param name="permissionType">The type of permission being checked</param>
    /// <param name="resource">The resource path (URL, file path, command, etc.)</param>
    /// <returns>PermissionResult: Allowed, Denied, or AskUser (AskUser means user confirmation will be needed at execution time)</returns>
    public PermissionResult EvaluatePermission(Guid callerId, PermissionType permissionType, string resource)
    {
        // Priority 1: Check frequency cache (high-frequency user decisions)
        PermissionResult? cachedResult = _frequencyCache.Query(permissionType, resource);
        if (cachedResult.HasValue)
        {
            return cachedResult.Value;
        }

        // Priority 2: Callback
        if (_callback != null)
        {
            PermissionResult callbackResult = _callback.Evaluate(callerId, permissionType, resource);
            if (callbackResult != PermissionResult.AskUser)
            {
                return callbackResult;
            }
        }

        // Priority 3: Branch based on curator status when callback returns AskUser or no callback
        if (IsCurator)
        {
            // Curator: will need user confirmation at execution time
            return PermissionResult.AskUser;
        }

        // Non-curator: check Global ACL
        PermissionResult? aclResult = _globalAcl.Query(permissionType, resource);
        if (aclResult.HasValue)
        {
            return aclResult.Value;
        }

        // No matching rule â€?deny by default
        return PermissionResult.Denied;
    }

    /// <summary>
    /// Records an audit log entry for a permission decision.
    /// Override in subclass or extend via event to persist.
    /// </summary>
    protected virtual void AuditLog(Guid callerId, PermissionType permissionType, string resource, PermissionResult result, string reason)
    {
        // Audit log is handled by AuditLogger injected via ServiceLocator
        ServiceLocator.Instance.AuditLogger?.Log(callerId, permissionType, resource, result, reason);
    }
}
