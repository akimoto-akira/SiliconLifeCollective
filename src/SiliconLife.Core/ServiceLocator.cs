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
/// Thread-safe singleton service locator that provides a central registry
/// for core services and per-being <see cref="PermissionManager"/> instances.
/// </summary>
public class ServiceLocator
{
    private static readonly Lazy<ServiceLocator> _instance = new(() => new ServiceLocator());
    private readonly Dictionary<Type, object> _services = new();
    private readonly Dictionary<Guid, PermissionManager> _permissionManagers = new();
    private readonly object _lock = new();

    /// <summary>Gets the singleton instance of <see cref="ServiceLocator"/>.</summary>
    public static ServiceLocator Instance => _instance.Value;

    private ServiceLocator() { }

    /// <summary>Gets the registered <see cref="ChatSystem"/>, or <c>null</c>.</summary>
    public ChatSystem? ChatSystem => Get<ChatSystem>();

    /// <summary>Gets the registered <see cref="IMManager"/>, or <c>null</c>.</summary>
    public IMManager? IMManager => Get<IMManager>();

    /// <summary>Gets the registered <see cref="AuditLogger"/>, or <c>null</c>.</summary>
    public AuditLogger? AuditLogger => Get<AuditLogger>();

    /// <summary>Gets the registered <see cref="GlobalACL"/>, or <c>null</c>.</summary>
    public GlobalACL? GlobalAcl => Get<GlobalACL>();

    /// <summary>Gets the registered <see cref="ISiliconBeingFactory"/>, or <c>null</c>.</summary>
    public ISiliconBeingFactory? BeingFactory => Get<ISiliconBeingFactory>();

    /// <summary>Gets the registered <see cref="SiliconBeingManager"/>, or <c>null</c>.</summary>
    public SiliconBeingManager? BeingManager => Get<SiliconBeingManager>();

    /// <summary>Gets the registered <see cref="DynamicBeingLoader"/>, or <c>null</c>.</summary>
    public DynamicBeingLoader? DynamicBeingLoader => Get<DynamicBeingLoader>();

    /// <summary>
    /// Registers a service instance under its concrete type.
    /// </summary>
    /// <typeparam name="T">The service type used as the lookup key.</typeparam>
    /// <param name="service">The service instance to register.</param>
    public void Register<T>(T service) where T : class
    {
        lock (_lock)
        {
            _services[typeof(T)] = service;
        }
    }

    /// <summary>
    /// Retrieves a previously registered service by type, or <c>null</c>
    /// if no service of the requested type has been registered.
    /// </summary>
    public T? Get<T>() where T : class
    {
        lock (_lock)
        {
            if (_services.TryGetValue(typeof(T), out object? service))
            {
                return service as T;
            }
            return null;
        }
    }

    /// <summary>
    /// Registers a <see cref="PermissionManager"/> for a specific being.
    /// </summary>
    /// <param name="beingId">The unique ID of the silicon being.</param>
    /// <param name="manager">The permission manager to associate.</param>
    public void RegisterPermissionManager(Guid beingId, PermissionManager manager)
    {
        lock (_lock)
        {
            _permissionManagers[beingId] = manager;
        }
    }

    /// <summary>
    /// Retrieves the <see cref="PermissionManager"/> registered for the
    /// specified being, or <c>null</c> if none exists.
    /// </summary>
    public PermissionManager? GetPermissionManager(Guid beingId)
    {
        lock (_lock)
        {
            _permissionManagers.TryGetValue(beingId, out PermissionManager? manager);
            return manager;
        }
    }

    /// <summary>
    /// Removes the <see cref="PermissionManager"/> registered for the
    /// specified being.
    /// </summary>
    public void UnregisterPermissionManager(Guid beingId)
    {
        lock (_lock)
        {
            _permissionManagers.Remove(beingId);
        }
    }

    /// <summary>
    /// Clears all registered services and permission managers.
    /// Typically called during host shutdown.
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _services.Clear();
            _permissionManagers.Clear();
        }
    }
}
