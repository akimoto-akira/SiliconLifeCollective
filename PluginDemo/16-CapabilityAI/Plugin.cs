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
using SiliconLife.Collective;

namespace SiliconLife.Demo.CapabilityAI;

/// <summary>
/// Demonstrates declaring Capability.AI to access the IAIService controlled entry point.
///
/// Capability.AI is fundamentally different from the other capabilities:
///   - It does NOT exempt any forbidden namespace or type in the security scan
///   - Instead, it allows the plugin to obtain an IAIService reference from the host
///   - IAIService lives in the SiliconLife.Collective namespace (never in forbidden lists)
///   - If the AI client needs network access, you must ALSO declare Capability.Network
///
/// This demonstrates the "controlled entry point" pattern:
///   - PermissionedStreamFactory → controlled file access (no Capability needed)
///   - NetworkExecutor → controlled network access (no Capability needed)
///   - CommandLineExecutor → controlled process access (no Capability needed)
///   - IAIService → controlled AI service access (Capability.AI needed)
///
/// Key points:
///   1. Capability.AI alone does NOT grant network access
///   2. To use HttpClient + IAIService together, stack both capabilities
///   3. The host injects IAIService based on the Capability.AI declaration
/// </summary>
[PluginCapability(Capability.AI, Reason = "Chat completion service for user interactions")]
public class CapabilityAIPlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.capabilityai";
    public string Version => "1.0.0";
    public string GetName(Language language) => "Capability.AI Demo";
    public string GetDescription(Language language) =>
        "Demonstrates declaring Capability.AI for IAIService access. " +
        "Shows that AI capability alone does not grant network access.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    // IAIService would be injected by the host at load time based on Capability.AI declaration.
    // In this demo, we simulate its usage.
    private object? _aiService;

    public void OnLoad()
    {
        Console.WriteLine("[CapabilityAI] Plugin loaded with Capability.AI declaration");
        // In a real scenario, the host would inject IAIService here:
        // _aiService = host.GetAIService(this);
    }

    public void OnStart()
    {
        Console.WriteLine("\n========== Capability.AI Demo ==========");
        Console.WriteLine("Declared: [PluginCapability(Capability.AI, Reason = \"Chat completion service for user interactions\")]");
        Console.WriteLine();

        DemonstrateAIServiceUsage();
        DemonstrateNoNetworkAccess();
        DemonstrateCapabilityStacking();
        DemonstrateControlledEntryPointPattern();

        Console.WriteLine("\n========== Comparison ==========");
        Console.WriteLine("  Capability.Network → Exempts System.Net.* from security scan");
        Console.WriteLine("  Capability.FileIO  → Exempts System.IO from security scan");
        Console.WriteLine("  Capability.Process → Exempts Process* types from security scan");
        Console.WriteLine("  Capability.AI      → Does NOT exempt any namespace; enables IAIService injection");
    }

    private void DemonstrateAIServiceUsage()
    {
        Console.WriteLine("[Demo 1] IAIService usage");
        Console.WriteLine("  ✅ With Capability.AI, the host injects an IAIService reference:");
        Console.WriteLine("    var response = await _aiService.ChatCompletionAsync(");
        Console.WriteLine("        prompt: \"Summarize this document\",");
        Console.WriteLine("        maxTokens: 500);");
        Console.WriteLine("    Console.WriteLine(response.Text);");
        Console.WriteLine();
    }

    private void DemonstrateNoNetworkAccess()
    {
        Console.WriteLine("[Demo 2] Capability.AI does NOT grant network access");
        Console.WriteLine("  ❌ Cannot use HttpClient with only Capability.AI:");
        Console.WriteLine("    // This would be REJECTED — System.Net.Http is still forbidden");
        Console.WriteLine("    var client = new HttpClient(); // ❌ BLOCKED");
        Console.WriteLine();
        Console.WriteLine("  ✅ To use both AI service and network, stack capabilities:");
        Console.WriteLine("    [PluginCapability(Capability.Network, Reason = \"API endpoint access\")]");
        Console.WriteLine("    [PluginCapability(Capability.AI, Reason = \"Chat completion service\")]");
        Console.WriteLine("    public class MyPlugin : IPlugin { ... }");
        Console.WriteLine();
    }

    private void DemonstrateCapabilityStacking()
    {
        Console.WriteLine("[Demo 3] Capability stacking for AI + Network");
        Console.WriteLine("  When you need both IAIService and direct network access:");
        Console.WriteLine();
        Console.WriteLine("  [PluginCapability(Capability.Network, Reason = \"Calls remote AI endpoint\")]");
        Console.WriteLine("  [PluginCapability(Capability.AI, Reason = \"Provides IAIService to downstream plugins\")]");
        Console.WriteLine("  public class AiConnectorPlugin : IPlugin { ... }");
        Console.WriteLine();
        Console.WriteLine("  See 17-CapabilityStacked for full stacking examples.");
        Console.WriteLine();
    }

    private void DemonstrateControlledEntryPointPattern()
    {
        Console.WriteLine("[Demo 4] Controlled entry point pattern across all capabilities");
        Console.WriteLine();
        Console.WriteLine("  | Resource    | Controlled Entry Point     | Capability Needed |");
        Console.WriteLine("  |-------------|---------------------------|-------------------|");
        Console.WriteLine("  | Files       | PermissionedStreamFactory | None              |");
        Console.WriteLine("  | Network     | NetworkExecutor           | None              |");
        Console.WriteLine("  | Process     | CommandLineExecutor       | None              |");
        Console.WriteLine("  | Data store  | SpeedyPack               | None              |");
        Console.WriteLine("  | AI Service  | IAIService               | Capability.AI     |");
        Console.WriteLine();
        Console.WriteLine("  IAIService is unique: it REQUIRES Capability.AI declaration.");
        Console.WriteLine("  This is because AI service access is an opt-in feature,");
        Console.WriteLine("  not a default capability available to all plugins.");
        Console.WriteLine();
    }

    public void OnStop()
    {
        _aiService = null;
        Console.WriteLine("[CapabilityAI] Plugin stopped.");
    }

    public void OnUnload()
    {
    }
}
