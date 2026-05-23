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
/// Thread-safe implementation of <see cref="IObjectFactory"/>.
/// Plugins register factory delegates during <see cref="IPlugin.OnLoad"/>,
/// and the factory creates instances only through registered delegates,
/// preventing arbitrary type instantiation via <c>Activator.CreateInstance</c>.
/// </summary>
public sealed class ObjectFactory : IObjectFactory
{
    private readonly Dictionary<Type, Func<object?[], object>> _factories = new();
    private readonly object _lock = new();
    private readonly ILogger _logger = LogManager.Instance.GetLogger<ObjectFactory>();

    public void RegisterFactory(Type type, Func<object?[], object> factory)
    {
        lock (_lock)
        {
            _factories[type] = factory;
        }
        _logger.Debug(null, "ObjectFactory: registered factory for {0}", type.FullName);
    }

    public void RegisterFactory<T>(Func<object?[], T> factory) where T : class
    {
        RegisterFactory(typeof(T), args => factory(args)!);
    }

    public void RegisterAutoFactory(Type type)
    {
        if (type.IsAbstract || type.IsInterface)
        {
            _logger.Warn(null, "ObjectFactory: cannot auto-register abstract/interface type {0}", type.FullName);
            return;
        }

        var ctors = type.GetConstructors();
        Func<object?[], object> factory = args =>
        {
            if (args == null || args.Length == 0)
            {
                var parameterless = ctors.FirstOrDefault(c => c.GetParameters().Length == 0);
                if (parameterless != null) return parameterless.Invoke(null);
                var firstCtor = ctors.FirstOrDefault();
                if (firstCtor != null && firstCtor.GetParameters().Length == 0) return firstCtor.Invoke(null);
                return System.Activator.CreateInstance(type)!;
            }

            foreach (var ctor in ctors)
            {
                var parameters = ctor.GetParameters();
                if (parameters.Length != args.Length) continue;

                bool match = true;
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (args[i] == null)
                    {
                        if (parameters[i].ParameterType.IsValueType &&
                            parameters[i].ParameterType != typeof(System.Nullable<>))
                        {
                            match = false;
                            break;
                        }
                        continue;
                    }

                    var argType = args[i]!.GetType();
                    if (!parameters[i].ParameterType.IsAssignableFrom(argType))
                    {
                        match = false;
                        break;
                    }
                }

                if (match) return ctor.Invoke(args);
            }

            var fallback = ctors.FirstOrDefault(c => c.GetParameters().Length == 0);
            if (fallback != null) return fallback.Invoke(null);

            return System.Activator.CreateInstance(type)!;
        };

        RegisterFactory(type, factory);
    }

    public void RegisterAutoFactoryFromAssembly(System.Reflection.Assembly assembly, Type baseType)
    {
        int count = 0;
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAbstract || type.IsInterface) continue;
            if (!baseType.IsAssignableFrom(type) && !type.IsSubclassOf(baseType)) continue;
            RegisterAutoFactory(type);
            count++;
        }
        _logger.Debug(null, "ObjectFactory: auto-registered {0} types from {1} (base={2})",
            count, assembly.GetName().Name, baseType.Name);
    }

    public object? CreateInstance(Type type, params object?[] args)
    {
        Func<object?[], object>? factory;
        lock (_lock)
        {
            if (!_factories.TryGetValue(type, out factory))
            {
                _logger.Warn(null, "ObjectFactory: no factory registered for {0}", type.FullName);
                return null;
            }
        }

        try
        {
            return factory(args);
        }
        catch (Exception ex)
        {
            _logger.Error(null, "ObjectFactory: failed to create {0}: {1}", type.FullName, ex.Message);
            return null;
        }
    }

    public T? CreateInstance<T>(params object?[] args) where T : class
    {
        return CreateInstance(typeof(T), args) as T;
    }

    public bool IsRegistered(Type type)
    {
        lock (_lock)
        {
            return _factories.ContainsKey(type);
        }
    }
}
