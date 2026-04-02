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
/// Global service registry (Singleton pattern).
/// Holds shared infrastructure services accessible by all silicon beings.
/// </summary>
public class ServiceRegistry
{
    private static readonly Lazy<ServiceRegistry> _instance = new(() => new ServiceRegistry());
    private readonly Dictionary<Guid, PermissionManager> _permissionManagers = new();
    private readonly object _lock = new();

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    public static ServiceRegistry Instance => _instance.Value;

    /// <summary>
    /// Gets or sets the shared chat system for persistent message storage
    /// </summary>
    public ChatSystem? ChatSystem { get; set; }

    /// <summary>
    /// Gets or sets the shared IM manager for sending messages
    /// </summary>
    public IMManager? IMManager { get; set; }

    /// <summary>
    /// Gets or sets the shared audit logger for permission decisions
    /// </summary>
    public AuditLogger? AuditLogger { get; set; }

    /// <summary>
    /// Gets or sets the shared Global ACL for permission rules
    /// </summary>
    public GlobalACL? GlobalAcl { get; set; }

    private ServiceRegistry() { }

    /// <summary>
    /// Registers a PermissionManager for a silicon being
    /// </summary>
    /// <param name="beingId">The silicon being's GUID</param>
    /// <param name="manager">The permission manager instance</param>
    public void RegisterPermissionManager(Guid beingId, PermissionManager manager)
    {
        lock (_lock)
        {
            _permissionManagers[beingId] = manager;
        }
    }

    /// <summary>
    /// Gets the PermissionManager for a specific silicon being
    /// </summary>
    /// <param name="beingId">The silicon being's GUID</param>
    /// <returns>The PermissionManager, or null if not registered</returns>
    public PermissionManager? GetPermissionManager(Guid beingId)
    {
        lock (_lock)
        {
            _permissionManagers.TryGetValue(beingId, out PermissionManager? manager);
            return manager;
        }
    }

    /// <summary>
    /// Removes a PermissionManager registration
    /// </summary>
    /// <param name="beingId">The silicon being's GUID</param>
    public void UnregisterPermissionManager(Guid beingId)
    {
        lock (_lock)
        {
            _permissionManagers.Remove(beingId);
        }
    }
}
