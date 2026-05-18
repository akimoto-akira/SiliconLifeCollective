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
/// Thread-safe implementation of <see cref="ITypeRegistry"/>.
/// Plugins register their types during <see cref="IPlugin.OnLoad"/>,
/// and the registry provides controlled lookup without exposing <c>AppDomain</c>.
/// </summary>
public sealed class TypeRegistry : ITypeRegistry
{
    private readonly Dictionary<string, Type> _typesByName = new(StringComparer.Ordinal);
    private readonly List<Type> _allTypes = new();
    private readonly object _lock = new();
    private readonly ILogger _logger = LogManager.Instance.GetLogger<TypeRegistry>();

    public void RegisterType(Type type)
    {
        if (type == null) return;

        lock (_lock)
        {
            if (type.FullName != null)
            {
                _typesByName[type.FullName] = type;
            }
            _allTypes.Add(type);
        }

        _logger.Debug(null, "TypeRegistry: registered {0}", type.FullName);
    }

    public void RegisterTypes(IEnumerable<Type> types)
    {
        foreach (var type in types)
        {
            RegisterType(type);
        }
    }

    public void RegisterFromAssembly(System.Reflection.Assembly assembly, Type baseType)
    {
        int count = 0;
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAbstract || type.IsInterface) continue;
            if (!baseType.IsAssignableFrom(type) && !type.IsSubclassOf(baseType)) continue;
            RegisterType(type);
            count++;
        }
        _logger.Debug(null, "TypeRegistry: registered {0} types from {1} (base={2})",
            count, assembly.GetName().Name, baseType.Name);
    }

    public Type? FindType(string fullName)
    {
        if (string.IsNullOrEmpty(fullName)) return null;

        lock (_lock)
        {
            if (_typesByName.TryGetValue(fullName, out var exact))
                return exact;

            int backtick = fullName.IndexOf('`');
            if (backtick == -1) return null;

            int bracket = fullName.IndexOf('[');
            if (bracket == -1) return null;

            string genericDefName = fullName.Substring(0, bracket);
            if (!_typesByName.TryGetValue(genericDefName, out var genericDef))
                return null;

            string countStr = fullName.Substring(backtick + 1, bracket - backtick - 1);
            if (!int.TryParse(countStr, out int typeParamCount))
                return null;

            var typeArgs = new List<Type>(typeParamCount);
            int pos = bracket;
            for (int i = 0; i < typeParamCount; i++)
            {
                int open = fullName.IndexOf('[', pos + 1);
                if (open == -1) return null;
                int close = fullName.IndexOf(']', open + 1);
                if (close == -1) return null;
                string argName = fullName.Substring(open + 1, close - open - 1).Split(',')[0];
                Type? argType = FindType(argName);
                if (argType == null) return null;
                typeArgs.Add(argType);
                pos = close;
            }

            try
            {
                return genericDef.MakeGenericType(typeArgs.ToArray());
            }
            catch (Exception ex)
            {
                _logger.Warn(null, "TypeRegistry: failed to make generic type {0}: {1}", fullName, ex.Message);
                return null;
            }
        }
    }

    public IEnumerable<Type> FindSubtypesOf(Type baseType)
        => FindTypes(t => !t.IsAbstract && baseType.IsAssignableFrom(t) && t != baseType);

    public IEnumerable<Type> FindImplementationsOf(Type interfaceType)
    {
        if (!interfaceType.IsInterface) return Enumerable.Empty<Type>();
        return FindTypes(t => !t.IsAbstract && interfaceType.IsAssignableFrom(t));
    }

    private IEnumerable<Type> FindTypes(Func<Type, bool> predicate)
    {
        lock (_lock)
        {
            return _allTypes.Where(predicate).ToList();
        }
    }
}
