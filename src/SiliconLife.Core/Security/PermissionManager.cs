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
/// 1. IsCurator check — curators have highest privileges (always allowed)
/// 2. UserFrequencyCache — high-frequency user decisions (memory-only)
/// 3. GlobalACL — prefix-matching rule table (persistent)
/// 4. IPermissionCallback — domain-specific callback rules
/// 5. IPermissionAskHandler — ask user (fallback)
/// </summary>
public class PermissionManager
{
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
    }

    /// <summary>
    /// Replaces the permission callback with a custom compiled one.
    /// Used by the stealth channel (DynamicCompilationExecutor → Curator → this).
    /// </summary>
    /// <param name="customCallback">The custom permission callback</param>
    public void SetCustomCallback(IPermissionCallback customCallback)
    {
        _callback = customCallback ?? throw new ArgumentNullException(nameof(customCallback));
    }

    /// <summary>
    /// Resets the permission callback to the original default.
    /// </summary>
    public void ResetCallback()
    {
        _callback = _defaultCallback;
    }

    /// <summary>
    /// Checks whether a permission request is allowed.
    /// Follows the priority chain: Curator → FrequencyCache → GlobalACL → Callback → AskUser.
    /// </summary>
    /// <param name="callerId">The GUID of the silicon being making the request</param>
    /// <param name="permissionType">The type of permission being checked</param>
    /// <param name="resource">The resource path (URL, file path, command, etc.)</param>
    /// <returns>True if the operation is allowed, false otherwise</returns>
    public bool CheckPermission(Guid callerId, PermissionType permissionType, string resource)
    {
        // Priority 1: Curators always have full access
        if (IsCurator)
        {
            AuditLog(callerId, permissionType, resource, PermissionResult.Allowed, "Curator privilege");
            return true;
        }

        // Priority 2: Check frequency cache (high-frequency user decisions)
        PermissionResult? cachedResult = _frequencyCache.Query(permissionType, resource);
        if (cachedResult.HasValue)
        {
            AuditLog(callerId, permissionType, resource, cachedResult.Value, "Frequency cache");
            return cachedResult.Value == PermissionResult.Allowed;
        }

        // Priority 3: Check Global ACL
        PermissionResult? aclResult = _globalAcl.Query(permissionType, resource);
        if (aclResult.HasValue)
        {
            AuditLog(callerId, permissionType, resource, aclResult.Value, "Global ACL");
            return aclResult.Value == PermissionResult.Allowed;
        }

        // Priority 4: Callback
        if (_callback != null)
        {
            PermissionResult callbackResult = _callback.Evaluate(callerId, permissionType, resource);
            if (callbackResult != PermissionResult.AskUser)
            {
                AuditLog(callerId, permissionType, resource, callbackResult, "Callback");
                return callbackResult == PermissionResult.Allowed;
            }
        }

        // Priority 5: Ask user
        if (_askHandler != null)
        {
            AskPermissionResult userDecision = _askHandler.AskUser(callerId, permissionType, resource);

            // Record the user's decision in frequency cache (if user opted in)
            PermissionResult userResult = userDecision.Allowed ? PermissionResult.Allowed : PermissionResult.Denied;
            _frequencyCache.Record(permissionType, resource, userResult, userDecision.AddToCache);

            AuditLog(callerId, permissionType, resource, userResult, "User decision");
            return userDecision.Allowed;
        }

        // No handler available — deny by default
        AuditLog(callerId, permissionType, resource, PermissionResult.Denied, "No handler, default deny");
        return false;
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
