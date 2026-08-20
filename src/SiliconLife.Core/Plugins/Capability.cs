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
/// Declares a runtime capability that a plugin may require.
/// Plugins declare capabilities via <see cref="PluginCapabilityAttribute"/> on their plugin class.
/// The <see cref="PluginLoader"/> reads these declarations from PE metadata before the security scan,
/// and relaxes the corresponding scan rules accordingly.
/// </summary>
/// <remarks>
/// <para><b>Undeclarable capabilities</b> — the following are always blocked regardless of any declaration:</para>
/// <list type="bullet">
///   <item>P/Invoke and native interop (<c>DllImport</c>, <c>Marshal</c>, <c>NativeMemory</c>, etc.)</item>
///   <item>Unsafe code (<c>[module: System.Security.UnverifiableCode]</c>)</item>
///   <item>Dynamic IL emission (<c>System.Reflection.Emit</c>)</item>
///   <item>Assembly dynamic loading (<c>System.Runtime.Loader</c>, <c>Assembly.Load*</c>)</item>
///   <item>Dynamic Roslyn compilation (<c>Microsoft.CodeAnalysis</c>)</item>
///   <item>Dangerous reflection members (<c>Type.GetType</c>, <c>Activator.CreateInstance</c>, etc.)</item>
/// </list>
/// </remarks>
public enum Capability
{
    /// <summary>
    /// Grants access to <c>System.Net.*</c> network types:
    /// <c>HttpClient</c>, <c>WebSocket</c>, <c>TcpClient</c>, <c>Dns</c>, <c>SmtpClient</c>, etc.
    /// <para>Relaxes the blanket ban on <c>System.Net.Http</c>, <c>System.Net.WebSockets</c>,
    /// <c>System.Net.Sockets</c>, <c>System.Net.Mail</c>, <c>System.Net.NetworkInformation</c>,
    /// <c>System.Net.Security</c> namespaces, and the type-level bans inside <c>System.Net</c>.</para>
    /// </summary>
    Network = 0,

    /// <summary>
    /// Grants full access to <c>System.IO</c> file-system types:
    /// <c>File</c>, <c>FileStream</c>, <c>Directory</c>, <c>FileInfo</c>, <c>StreamReader</c>, etc.
    /// <para>Relaxes the <c>System.IO</c> namespace ban beyond the built-in memory-stream exemption list
    /// (<see cref="PluginLoader"/>.<c>SystemIOAllowedTypes</c>).</para>
    /// <para>Plugins should still prefer <c>PermissionedStreamFactory</c> where possible,
    /// as it provides additional audit trails and access control.</para>
    /// </summary>
    FileIO = 1,

    /// <summary>
    /// Grants access to <c>System.Diagnostics.Process</c> and related types for launching child processes.
    /// <para>Relaxes the per-type ban on <c>Process</c>, <c>ProcessStartInfo</c>,
    /// <c>ProcessThread</c>, <c>ProcessModule</c>, and related process types.</para>
    /// <para>Plugins should still prefer <c>CommandLineExecutor</c> where possible,
    /// as it provides sandboxing, timeouts, and output capture.</para>
    /// </summary>
    Process = 2,

    /// <summary>
    /// Declares that the plugin acts as or depends on an AI service provider.
    /// Allows the plugin to obtain and use an <c>IAIService</c> reference injected by the host.
    /// <para>This capability alone does not grant network access.
    /// Declare <see cref="Network"/> separately if the plugin also needs direct HTTP or socket access.</para>
    /// <para>Example of stacked capabilities:</para>
    /// <code>
    /// [PluginCapability(Capability.Network, Reason = "Calls remote AI endpoint")]
    /// [PluginCapability(Capability.AI, Reason = "Provides IAIService to downstream plugins")]
    /// public sealed class AiConnectorPlugin : IPlugin { ... }
    /// </code>
    /// </summary>
    AI = 3,

    /// <summary>
    /// Grants access to Windows Registry types under <c>Microsoft.Win32</c>:
    /// <c>Registry</c>, <c>RegistryKey</c>, etc.
    /// <para>Relaxes the blanket ban on the <c>Microsoft.Win32</c> namespace (Rule 6)</para>
    /// <para>Registry access is inherently Windows-only. Plugins should guard with
    /// <c>OperatingSystem.IsWindows()</c> before touching the registry and degrade
    /// gracefully on other platforms.</para>
    /// </summary>
    Registry = 4,
}
