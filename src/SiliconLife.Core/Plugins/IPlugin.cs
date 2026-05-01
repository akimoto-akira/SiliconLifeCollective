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
/// Core interface that all SiliconLife plugins must implement.
/// A plugin is a self-contained extension unit that can register tools,
/// hook into being lifecycle events, and contribute services to the host.
/// </summary>
public interface IPlugin
{
    /// <summary>
    /// Gets the unique identifier of this plugin.
    /// Must be stable across versions (e.g., "com.example.my-plugin").
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the human-readable display name of this plugin.
    /// </summary>
    /// <param name="language">The language to use for the display name.</param>
    string GetName(Language language);

    /// <summary>
    /// Gets the semantic version string of this plugin (e.g., "1.0.0").
    /// </summary>
    string Version { get; }

    /// <summary>
    /// Gets a short description of what this plugin does.
    /// </summary>
    /// <param name="language">The language to use for the description.</param>
    string GetDescription(Language language);

    /// <summary>
    /// Gets the author or organization that created this plugin.
    /// </summary>
    /// <param name="language">The language to use for the author name.</param>
    string GetAuthor(Language language);

    /// <summary>
    /// Called once when the plugin is loaded into the host.
    /// Use this to validate configuration and register tools.
    /// </summary>
    void OnLoad();

    /// <summary>
    /// Called when the host is fully started and all plugins have been loaded.
    /// </summary>
    void OnStart();

    /// <summary>
    /// Called when the host is shutting down gracefully.
    /// Release resources and flush buffers here.
    /// </summary>
    void OnStop();

    /// <summary>
    /// Called when the plugin is being unloaded.
    /// Perform final cleanup here.
    /// </summary>
    void OnUnload();
}
