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

namespace SiliconLife.Demo.MinimalPlugin;

/// <summary>
/// Minimal IPlugin implementation — all properties return hardcoded values,
/// lifecycle methods are empty but documented to illustrate call timing.
/// </summary>
public class MinimalPlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.minimal";

    public string Version => "1.0.0";

    public string GetName(Language language) => "Minimal Plugin Demo";

    public string GetDescription(Language language) =>
        "A minimal IPlugin implementation that demonstrates the plugin lifecycle with hardcoded values.";

    public string GetAuthor(Language language) => "SiliconLife Collective";

    /// <summary>
    /// Called once when the plugin DLL is loaded into the host process.
    /// Use this to validate configuration, register types, or prepare resources.
    /// This is the first lifecycle method called.
    /// </summary>
    public void OnLoad()
    {
    }

    /// <summary>
    /// Called when the host has fully started and all plugins have been loaded.
    /// Safe to interact with other plugins and shared services here.
    /// This is the second lifecycle method called.
    /// </summary>
    public void OnStart()
    {
    }

    /// <summary>
    /// Called when the host is shutting down gracefully.
    /// Release resources, flush buffers, and save state here.
    /// This is the third lifecycle method called (before OnUnload).
    /// </summary>
    public void OnStop()
    {
    }

    /// <summary>
    /// Called when the plugin is being unloaded from the host process.
    /// Perform final cleanup here — this is the last lifecycle method called.
    /// </summary>
    public void OnUnload()
    {
    }
}
