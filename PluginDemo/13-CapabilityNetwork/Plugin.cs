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

using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using SiliconLife.Collective;

namespace SiliconLife.Demo.CapabilityNetwork;

/// <summary>
/// Demonstrates declaring Capability.Network to gain access to System.Net.* types.
///
/// Without the [PluginCapability(Capability.Network)] declaration, any reference to
/// HttpClient / TcpClient / SmtpClient etc. would cause the plugin to be REJECTED
/// during PluginLoader's security scan (see 08-ForbiddenNetwork).
///
/// By declaring the capability with a clear Reason, PluginLoader relaxes the
/// corresponding scan rules and allows the plugin to use network types.
///
/// Key points:
///   1. [PluginCapability(Capability.Network, Reason = "...")] on the plugin class
///   2. Reason is written to the security audit log at load time
///   3. Exempts: System.Net.Http, System.Net.WebSockets, System.Net.Sockets,
///      System.Net.Mail, System.Net.NetworkInformation, System.Net.Security,
///      and per-type bans inside System.Net (HttpWebRequest, WebClient, Dns, etc.)
///   4. Undeclarable capabilities (P/Invoke, Unsafe, Reflection.Emit) are STILL blocked
/// </summary>
[PluginCapability(Capability.Network, Reason = "Calls weather REST API to retrieve forecast data")]
public class CapabilityNetworkPlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.capabilitynetwork";
    public string Version => "1.0.0";
    public string GetName(Language language) => "Capability.Network Demo";
    public string GetDescription(Language language) =>
        "Demonstrates declaring Capability.Network to access System.Net.* network types. " +
        "Contrasts with 08-ForbiddenNetwork where no declaration causes rejection.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    private HttpClient? _httpClient;

    public void OnLoad()
    {
        // HttpClient references System.Net.Http.HttpClient — normally forbidden,
        // but Capability.Network exempts the System.Net.Http namespace from TypeRef scanning.
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
        Console.WriteLine("[CapabilityNetwork] HttpClient created — allowed by Capability.Network declaration");
    }

    public void OnStart()
    {
        Console.WriteLine("\n========== Capability.Network Demo ==========");
        Console.WriteLine("This plugin declared [PluginCapability(Capability.Network, Reason = \"Calls weather REST API\")]");
        Console.WriteLine("PluginLoader reads this declaration from PE metadata BEFORE the security scan,");
        Console.WriteLine("and relaxes the corresponding scan rules accordingly.\n");

        DemonstrateHttpClient();
        DemonstrateExemptedNamespaces();
        DemonstrateStillForbidden();

        Console.WriteLine("\n========== Comparison with 08-ForbiddenNetwork ==========");
        Console.WriteLine("  08-ForbiddenNetwork: No declaration → plugin REJECTED by PluginLoader");
        Console.WriteLine("  13-CapabilityNetwork: [PluginCapability(Capability.Network)] → plugin LOADED successfully");
    }

    /// <summary>
    /// Demonstrates using HttpClient to make a network request.
    /// Without Capability.Network, this type reference would trigger: [TypeRef] System.Net.Http.HttpClient
    /// </summary>
    private void DemonstrateHttpClient()
    {
        Console.WriteLine("[Demo 1] Using HttpClient (System.Net.Http.HttpClient)");
        Console.WriteLine("  ✅ ALLOWED: [PluginCapability(Capability.Network)] exempts System.Net.Http namespace");

        // In a real scenario, this would make an actual HTTP request.
        // For demo purposes, we just show the code pattern.
        Console.WriteLine("  Code pattern:");
        Console.WriteLine("    var response = await _httpClient.GetAsync(\"https://api.weather.com/forecast\");");
        Console.WriteLine("    string json = await response.Content.ReadAsStringAsync();");
        Console.WriteLine();
    }

    /// <summary>
    /// Lists all namespace exemptions granted by Capability.Network.
    /// </summary>
    private void DemonstrateExemptedNamespaces()
    {
        Console.WriteLine("[Demo 2] Namespaces exempted by Capability.Network");
        Console.WriteLine("  ✅ System.Net.Http            — HttpClient, HttpRequestMessage, HttpResponseMessage");
        Console.WriteLine("  ✅ System.Net.WebSockets      — ClientWebSocket, WebSocket");
        Console.WriteLine("  ✅ System.Net.Sockets         — TcpClient, UdpClient, Socket");
        Console.WriteLine("  ✅ System.Net.Mail            — SmtpClient, MailMessage");
        Console.WriteLine("  ✅ System.Net.NetworkInformation — Ping, NetworkInterface");
        Console.WriteLine("  ✅ System.Net.Security        — SslStream");
        Console.WriteLine("  ✅ System.Net (per-type bans) — HttpWebRequest, WebClient, Dns, etc.");
        Console.WriteLine();

        Console.WriteLine("  ILString exemptions:");
        Console.WriteLine("  ✅ Strings starting with \"System.Net.Http\" are not flagged");
        Console.WriteLine("  ✅ Strings starting with \"System.Net.WebSockets\" are not flagged");
        Console.WriteLine("  ✅ Strings starting with \"System.Net.Sockets\" are not flagged");
        Console.WriteLine("  ✅ Strings starting with \"System.Net.Mail\" are not flagged");
        Console.WriteLine("  ✅ Strings starting with \"System.Net.NetworkInformation\" are not flagged");
        Console.WriteLine("  ✅ Strings starting with \"System.Net.Security\" are not flagged");
        Console.WriteLine();
    }

    /// <summary>
    /// Even with Capability.Network, some capabilities remain undeclarable and are always blocked.
    /// </summary>
    private void DemonstrateStillForbidden()
    {
        Console.WriteLine("[Demo 3] Still FORBIDDEN despite Capability.Network");
        Console.WriteLine("  ❌ [DllImport] — P/Invoke is always blocked (undeclarable capability)");
        Console.WriteLine("  ❌ Marshal / NativeMemory — Native interop is always blocked");
        Console.WriteLine("  ❌ System.Runtime.CompilerServices.Unsafe — Unsafe code is always blocked");
        Console.WriteLine("  ❌ System.Reflection.Emit — Dynamic IL emission is always blocked");
        Console.WriteLine("  ❌ System.Runtime.Loader — AssemblyLoadContext is always blocked");
        Console.WriteLine("  ❌ Microsoft.Win32 — Registry access is always blocked");
        Console.WriteLine();
        Console.WriteLine("  These are \"undeclarable capabilities\" — no Capability enum value exists for them.");
        Console.WriteLine("  PluginLoader enforces these checks regardless of any declared capabilities.");
        Console.WriteLine();
    }

    public void OnStop()
    {
        _httpClient?.Dispose();
        _httpClient = null;
        Console.WriteLine("[CapabilityNetwork] Plugin stopped. HttpClient disposed.");
    }

    public void OnUnload()
    {
    }
}
