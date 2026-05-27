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

using SiliconLife.Collective;

namespace SiliconLife.Demo.TypeRegistryUsage;

/// <summary>
/// Abstract base class for demo tools.
/// All tool types that this plugin registers inherit from this class,
/// enabling <see cref="ITypeRegistry.FindSubtypesOf"/> to discover them.
/// </summary>
public abstract class DemoTool
{
    public abstract string Name { get; }
    public abstract string Execute();
}

public class GreetingTool : DemoTool
{
    public override string Name => "Greeting";
    public override string Execute() => "Hello from GreetingTool!";
}

public class FarewellTool : DemoTool
{
    public override string Name => "Farewell";
    public override string Execute() => "Goodbye from FarewellTool!";
}

public class StatusTool : DemoTool
{
    public override string Name => "Status";
    public override string Execute() => "All systems operational.";
}

/// <summary>
/// Demonstrates <see cref="ITypeRegistry"/> registration and lookup.
/// OnLoad registers three <see cref="DemoTool"/> subtypes;
/// OnStart queries them back via <see cref="ITypeRegistry.FindSubtypesOf"/>.
/// </summary>
public class TypeRegistryUsagePlugin : IPlugin
{
    private ITypeRegistry? _registry;

    public string Id => "com.siliconlife.demo.typeregistry";
    public string Version => "1.0.0";
    public string GetName(Language language) => "TypeRegistry Usage Demo";
    public string GetDescription(Language language) =>
        "Demonstrates ITypeRegistry: register custom types in OnLoad, discover them with FindSubtypesOf in OnStart.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    /// <summary>
    /// Called once when the plugin DLL is loaded into the host process.
    /// Retrieves <see cref="ITypeRegistry"/> from <see cref="ServiceLocator"/>
    /// and registers three <see cref="DemoTool"/> subtypes.
    /// Also demonstrates <see cref="ITypeRegistry.RegisterFromAssembly"/> as an alternative.
    /// </summary>
    public void OnLoad()
    {
        _registry = ServiceLocator.Instance.GetService<ITypeRegistry>();
        if (_registry == null) return;

        _registry.RegisterType(typeof(GreetingTool));
        _registry.RegisterType(typeof(FarewellTool));
        _registry.RegisterType(typeof(StatusTool));

        // Alternative: register all DemoTool subtypes from this assembly at once
        // _registry.RegisterFromAssembly(typeof(TypeRegistryUsagePlugin).Assembly, typeof(DemoTool));
    }

    /// <summary>
    /// Called when the host has fully started and all plugins have been loaded.
    /// Queries all registered <see cref="DemoTool"/> subtypes and prints their names.
    /// </summary>
    public void OnStart()
    {
        if (_registry == null) return;

        IEnumerable<Type> toolTypes = _registry.FindSubtypesOf(typeof(DemoTool));
        foreach (Type t in toolTypes)
        {
            // In a real plugin you would log this; Console.WriteLine is for demo clarity only
            Console.WriteLine($"[TypeRegistryUsage] Found tool: {t.FullName}");
        }
    }

    public void OnStop()
    {
    }

    public void OnUnload()
    {
        _registry = null;
    }
}
