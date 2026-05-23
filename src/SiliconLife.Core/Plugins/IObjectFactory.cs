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
/// Controlled object factory: replaces <c>Activator.CreateInstance()</c>.
/// Plugins register factory delegates for each type they need to dynamically create
/// during <see cref="IPlugin.OnLoad"/>. The runtime only creates instances through
/// registered delegates, preventing arbitrary type instantiation.
/// <para>This follows the same security pattern as <see cref="PermissionedStreamFactory"/>:
/// plugins do not directly call <c>Activator.CreateInstance</c>,
/// but instead go through a core-provided safe entry point.</para>
/// </summary>
public interface IObjectFactory
{
    /// <summary>
    /// Registers a factory delegate for the specified type.
    /// </summary>
    /// <param name="type">The type to register a factory for.</param>
    /// <param name="factory">
    /// A delegate that takes an array of constructor arguments and returns a new instance.
    /// The factory is responsible for argument validation and type conversion.
    /// </param>
    void RegisterFactory(Type type, Func<object?[], object> factory);

    /// <summary>
    /// Registers a factory delegate for the specified type (generic version).
    /// </summary>
    /// <typeparam name="T">The type to register a factory for.</typeparam>
    /// <param name="factory">A delegate that takes an array of constructor arguments and returns a new instance.</param>
    void RegisterFactory<T>(Func<object?[], T> factory) where T : class;

    /// <summary>
    /// Automatically registers a factory for the specified type by analyzing its constructors.
    /// The generated factory delegate matches constructor parameters by type against the provided arguments,
    /// falling back to parameterless construction when no arguments match.
    /// <para>This is a convenience method for types with simple constructor patterns
    /// (parameterless, or single-parameter accepting a known parent type).</para>
    /// </summary>
    /// <param name="type">The type to auto-register a factory for.</param>
    void RegisterAutoFactory(Type type);

    /// <summary>
    /// Automatically registers factories for all non-abstract types in the specified assembly
    /// that are subclasses of the given base type.
    /// </summary>
    /// <param name="assembly">The assembly to scan for types.</param>
    /// <param name="baseType">Only types that are subclasses of this type will be registered.</param>
    void RegisterAutoFactoryFromAssembly(System.Reflection.Assembly assembly, Type baseType);

    /// <summary>
    /// Creates an instance of the specified type using a previously registered factory delegate
    /// (replaces <c>Activator.CreateInstance</c>).
    /// </summary>
    /// <param name="type">The type to create an instance of.</param>
    /// <param name="args">Constructor arguments to pass to the factory delegate.</param>
    /// <returns>The created instance, or <c>null</c> if no factory is registered or creation fails.</returns>
    object? CreateInstance(Type type, params object?[] args);

    /// <summary>
    /// Creates an instance of the specified type using a previously registered factory delegate (generic version).
    /// </summary>
    /// <typeparam name="T">The type to create an instance of.</typeparam>
    /// <param name="args">Constructor arguments to pass to the factory delegate.</param>
    /// <returns>The created instance, or <c>null</c> if no factory is registered or creation fails.</returns>
    T? CreateInstance<T>(params object?[] args) where T : class;

    /// <summary>
    /// Checks whether a factory has been registered for the specified type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if a factory is registered; otherwise, <c>false</c>.</returns>
    bool IsRegistered(Type type);
}
