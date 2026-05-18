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
/// Controlled type registry: replaces <c>AppDomain.CurrentDomain.GetAssemblies()</c> reflection scanning.
/// Plugins explicitly register their exposed types in <see cref="IPlugin.OnLoad"/>,
/// and the runtime only looks up types from the registry.
/// <para>This follows the same security pattern as <see cref="PermissionedStreamFactory"/> and
/// <see cref="SafePath"/>: plugins do not directly access <c>AppDomain</c>,
/// but instead go through a core-provided safe entry point.</para>
/// </summary>
public interface ITypeRegistry
{
    /// <summary>
    /// Registers a single type.
    /// </summary>
    /// <param name="type">The type to register.</param>
    void RegisterType(Type type);

    /// <summary>
    /// Registers multiple types at once.
    /// </summary>
    /// <param name="types">The types to register.</param>
    void RegisterTypes(IEnumerable<Type> types);

    /// <summary>
    /// Registers all non-abstract types from the specified assembly
    /// that are subclasses of the given base type.
    /// </summary>
    /// <param name="assembly">The assembly to scan for types.</param>
    /// <param name="baseType">Only types that are subclasses of this type will be registered.</param>
    void RegisterFromAssembly(System.Reflection.Assembly assembly, Type baseType);

    /// <summary>
    /// Finds a type by its full name (replaces <c>AppDomain.CurrentDomain.GetAssemblies()</c> scanning in <c>FindType()</c>).
    /// Supports generic type name resolution (e.g., <c>MyType`1[SomeArg]</c>).
    /// </summary>
    /// <param name="fullName">The full name of the type to find.</param>
    /// <returns>The found type, or <c>null</c> if not registered.</returns>
    Type? FindType(string fullName);

    /// <summary>
    /// Finds all non-abstract subtypes of the specified base type
    /// (replaces <c>AppDomain.CurrentDomain.GetAssemblies()</c> scanning in <c>GetAllWordTypes()</c>).
    /// </summary>
    /// <param name="baseType">The base type to search subtypes for.</param>
    /// <returns>Enumerable of non-abstract subtypes.</returns>
    IEnumerable<Type> FindSubtypesOf(Type baseType);

    /// <summary>
    /// Finds all non-abstract types that implement the specified interface.
    /// </summary>
    /// <param name="interfaceType">The interface type to search implementations for.</param>
    /// <returns>Enumerable of implementing types.</returns>
    IEnumerable<Type> FindImplementationsOf(Type interfaceType);
}
