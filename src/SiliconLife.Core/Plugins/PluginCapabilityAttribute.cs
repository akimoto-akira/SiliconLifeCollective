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
/// Declares that the attributed plugin class requires a specific runtime capability.
/// Apply this attribute to the class that directly implements <see cref="IPlugin"/>.
/// Multiple capabilities may be declared by stacking multiple <see cref="PluginCapabilityAttribute"/> instances
/// (because <c>AllowMultiple = true</c>).
/// </summary>
/// <remarks>
/// <para>The <see cref="PluginLoader"/> reads capability declarations directly from PE metadata
/// (the CustomAttribute table) before running the security scan, so declarations take effect
/// even during the initial load-time scan.</para>
/// <para>All declarations — including the <see cref="Reason"/> field — are written to the host's
/// security audit log at plugin load time.</para>
/// <para><b>Example — single capability:</b></para>
/// <code>
/// [PluginCapability(Capability.Network, Reason = "Calls weather REST API")]
/// public sealed class WeatherPlugin : IPlugin { ... }
/// </code>
/// <para><b>Example — stacked capabilities:</b></para>
/// <code>
/// [PluginCapability(Capability.Network, Reason = "Calls AI model API endpoint")]
/// [PluginCapability(Capability.AI,      Reason = "Provides IAIService to downstream plugins")]
/// public sealed class AiConnectorPlugin : IPlugin { ... }
/// </code>
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class PluginCapabilityAttribute : Attribute
{
    /// <summary>
    /// Initializes a new capability declaration with the specified capability.
    /// </summary>
    /// <param name="capability">The capability this plugin requires.</param>
    public PluginCapabilityAttribute(Capability capability)
    {
        Capability = capability;
    }

    /// <summary>
    /// Gets the capability this plugin requires.
    /// </summary>
    public Capability Capability { get; }

    /// <summary>
    /// Gets or sets a human-readable explanation of why this capability is required.
    /// This value is written to the security audit log at load time.
    /// Providing a clear reason is strongly recommended for all production plugins.
    /// </summary>
    /// <example>"Calls the weather REST API to retrieve forecast data."</example>
    public string? Reason { get; set; }
}
