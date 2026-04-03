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

public class ServiceLocator
{
    private static readonly Lazy<ServiceLocator> _instance = new(() => new ServiceLocator());
    private readonly Dictionary<Type, object> _services = new();
    private readonly Dictionary<Guid, PermissionManager> _permissionManagers = new();
    private readonly object _lock = new();

    public static ServiceLocator Instance => _instance.Value;

    private ServiceLocator() { }

    public ChatSystem? ChatSystem => Get<ChatSystem>();
    public IMManager? IMManager => Get<IMManager>();
    public AuditLogger? AuditLogger => Get<AuditLogger>();
    public GlobalACL? GlobalAcl => Get<GlobalACL>();
    public ISiliconBeingFactory? BeingFactory => Get<ISiliconBeingFactory>();
    public SiliconBeingManager? BeingManager => Get<SiliconBeingManager>();
    public DynamicBeingLoader? DynamicBeingLoader => Get<DynamicBeingLoader>();

    public void Register<T>(T service) where T : class
    {
        lock (_lock)
        {
            _services[typeof(T)] = service;
        }
    }

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

    public void RegisterPermissionManager(Guid beingId, PermissionManager manager)
    {
        lock (_lock)
        {
            _permissionManagers[beingId] = manager;
        }
    }

    public PermissionManager? GetPermissionManager(Guid beingId)
    {
        lock (_lock)
        {
            _permissionManagers.TryGetValue(beingId, out PermissionManager? manager);
            return manager;
        }
    }

    public void UnregisterPermissionManager(Guid beingId)
    {
        lock (_lock)
        {
            _permissionManagers.Remove(beingId);
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _services.Clear();
            _permissionManagers.Clear();
        }
    }
}
