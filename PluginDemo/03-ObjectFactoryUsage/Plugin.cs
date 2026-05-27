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

namespace SiliconLife.Demo.ObjectFactoryUsage;

/// <summary>
/// A demo service with a parameterless constructor.
/// RegisterAutoFactory can create this without any arguments.
/// </summary>
public class SimpleService
{
    public string GetInfo() => "SimpleService created via parameterless constructor";
}

/// <summary>
/// A demo service with a parameterized constructor.
/// RegisterAutoFactory analyzes constructors and matches arguments by type.
/// </summary>
public class ConfiguredService
{
    private readonly string _name;

    public ConfiguredService(string name)
    {
        _name = name;
    }

    public string GetInfo() => $"ConfiguredService created with name='{_name}'";
}

/// <summary>
/// Demonstrates <see cref="IObjectFactory"/> registration and instance creation.
/// OnLoad registers factories via <see cref="IObjectFactory.RegisterAutoFactory"/>;
/// OnStart creates instances via <see cref="IObjectFactory.CreateInstance"/> and
/// the generic <see cref="IObjectFactory.CreateInstance{T}"/>.
/// </summary>
public class ObjectFactoryUsagePlugin : IPlugin
{
    private IObjectFactory? _factory;

    public string Id => "com.siliconlife.demo.objectfactory";
    public string Version => "1.0.0";
    public string GetName(Language language) => "ObjectFactory Usage Demo";
    public string GetDescription(Language language) =>
        "Demonstrates IObjectFactory: register types with RegisterAutoFactory in OnLoad, create instances with CreateInstance in OnStart.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    /// <summary>
    /// Called once when the plugin DLL is loaded into the host process.
    /// Retrieves <see cref="IObjectFactory"/> from <see cref="ServiceLocator"/>
    /// and registers factories for demo service types.
    /// <para>
    /// <see cref="IObjectFactory.RegisterAutoFactory"/> analyzes the type's constructors
    /// and generates a factory delegate that matches constructor parameters by type.
    /// For parameterless constructors, no arguments are needed at creation time.
    /// For parameterized constructors, pass matching arguments to <see cref="IObjectFactory.CreateInstance"/>.
    /// </para>
    /// </summary>
    public void OnLoad()
    {
        _factory = ServiceLocator.Instance.GetService<IObjectFactory>();
        if (_factory == null) return;

        _factory.RegisterAutoFactory(typeof(SimpleService));
        _factory.RegisterAutoFactory(typeof(ConfiguredService));
    }

    /// <summary>
    /// Called when the host has fully started and all plugins have been loaded.
    /// Creates instances using both the non-generic and generic <see cref="IObjectFactory.CreateInstance"/>
    /// overloads, then invokes methods on the created objects.
    /// </summary>
    public void OnStart()
    {
        if (_factory == null) return;

        object? simple = _factory.CreateInstance(typeof(SimpleService));
        if (simple is SimpleService s)
        {
            Console.WriteLine($"[ObjectFactoryUsage] {s.GetInfo()}");
        }

        ConfiguredService? configured = _factory.CreateInstance<ConfiguredService>("DemoPlugin");
        if (configured != null)
        {
            Console.WriteLine($"[ObjectFactoryUsage] {configured.GetInfo()}");
        }
    }

    public void OnStop()
    {
    }

    public void OnUnload()
    {
        _factory = null;
    }
}
