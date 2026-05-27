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
using SiliconLife.Collective;

namespace SiliconLife.Demo.CapabilityStacked;

/// <summary>
/// Demonstrates stacking multiple PluginCapability attributes on a single plugin.
///
/// PluginCapabilityAttribute has AllowMultiple = true, so you can declare
/// multiple capabilities by stacking attributes. PluginLoader processes each
/// declared capability independently and merges their exemption rules.
///
/// This example simulates an AI connector plugin that needs:
///   - Capability.Network: to call remote AI API endpoints via HttpClient
///   - Capability.AI: to receive IAIService injection from the host
///
/// Key points:
///   1. Stacking syntax: multiple [PluginCapability] attributes on one class
///   2. PluginLoader merges exemption rules from all declared capabilities
///   3. Stacking does NOT grant unlimited power — undeclarable capabilities remain blocked
///   4. Each capability's Reason is logged independently in the audit trail
/// </summary>
[PluginCapability(Capability.Network, Reason = "API endpoint access for remote AI models")]
[PluginCapability(Capability.AI, Reason = "AI service provider for downstream plugins")]
public class CapabilityStackedPlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.capabilitystacked";
    public string Version => "1.0.0";
    public string GetName(Language language) => "Capability Stacking Demo";
    public string GetDescription(Language language) =>
        "Demonstrates stacking multiple PluginCapability attributes. " +
        "Network + AI stacked for an AI connector plugin.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    public void OnLoad()
    {
        Console.WriteLine("[CapabilityStacked] Plugin loaded with stacked capabilities: Network + AI");
    }

    public void OnStart()
    {
        Console.WriteLine("\n========== Capability Stacking Demo ==========");
        Console.WriteLine("Stacked declarations:");
        Console.WriteLine("  [PluginCapability(Capability.Network, Reason = \"API endpoint access for remote AI models\")]");
        Console.WriteLine("  [PluginCapability(Capability.AI, Reason = \"AI service provider for downstream plugins\")]");
        Console.WriteLine();

        DemonstrateStackedUsage();
        DemonstrateMergedExemptions();
        DemonstrateStillForbidden();

        Console.WriteLine("\n========== Stacking Rules ==========");
        Console.WriteLine("  ✅ AllowMultiple = true — stack as many capabilities as needed");
        Console.WriteLine("  ✅ PluginLoader merges exemption rules from ALL declared capabilities");
        Console.WriteLine("  ✅ Each capability's Reason is logged independently in the audit trail");
        Console.WriteLine("  ❌ Stacking does NOT bypass undeclarable capability bans");
        Console.WriteLine("  ❌ Reflection.Emit, P/Invoke, Unsafe, Registry are ALWAYS blocked");
    }

    private void DemonstrateStackedUsage()
    {
        Console.WriteLine("[Demo 1] Using both Network and AI capabilities");
        Console.WriteLine("  ✅ Network: HttpClient for API calls");
        Console.WriteLine("    var client = new HttpClient();");
        Console.WriteLine("    var response = await client.GetAsync(\"https://api.openai.com/v1/models\");");
        Console.WriteLine();
        Console.WriteLine("  ✅ AI: IAIService for providing AI services to downstream plugins");
        Console.WriteLine("    var chatResponse = await _aiService.ChatCompletionAsync(prompt);");
        Console.WriteLine();
    }

    private void DemonstrateMergedExemptions()
    {
        Console.WriteLine("[Demo 2] Merged exemption rules from all declared capabilities");
        Console.WriteLine();
        Console.WriteLine("  From Capability.Network:");
        Console.WriteLine("    ✅ System.Net.Http.* — HttpClient, HttpRequestMessage, etc.");
        Console.WriteLine("    ✅ System.Net.WebSockets.* — ClientWebSocket, etc.");
        Console.WriteLine("    ✅ System.Net.Sockets.* — TcpClient, etc.");
        Console.WriteLine("    ✅ System.Net (per-type bans) — HttpWebRequest, WebClient, Dns, etc.");
        Console.WriteLine("    ✅ ILString: \"System.Net.*\" prefixes not flagged");
        Console.WriteLine();
        Console.WriteLine("  From Capability.AI:");
        Console.WriteLine("    ✅ IAIService injection enabled (no TypeRef exemptions needed)");
        Console.WriteLine();
        Console.WriteLine("  Combined: plugin can use HttpClient AND IAIService");
        Console.WriteLine();
    }

    private void DemonstrateStillForbidden()
    {
        Console.WriteLine("[Demo 3] Still FORBIDDEN despite stacked capabilities");
        Console.WriteLine("  ❌ [DllImport] — P/Invoke always blocked (no Capability exists for this)");
        Console.WriteLine("  ❌ Marshal / NativeMemory — native interop always blocked");
        Console.WriteLine("  ❌ System.Runtime.CompilerServices.Unsafe — unsafe code always blocked");
        Console.WriteLine("  ❌ System.Reflection.Emit — IL emission always blocked");
        Console.WriteLine("  ❌ System.Runtime.Loader — AssemblyLoadContext always blocked");
        Console.WriteLine("  ❌ Microsoft.Win32 — Registry access always blocked");
        Console.WriteLine();
        Console.WriteLine("  Stacking more capabilities does NOT bypass these hard bans.");
        Console.WriteLine("  No Capability enum value exists for P/Invoke, Unsafe, Reflection.Emit, etc.");
        Console.WriteLine();
    }

    public void OnStop()
    {
        Console.WriteLine("[CapabilityStacked] Plugin stopped.");
    }

    public void OnUnload()
    {
    }
}
