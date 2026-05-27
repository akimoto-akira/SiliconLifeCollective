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
using System.Diagnostics;
using SiliconLife.Collective;

namespace SiliconLife.Demo.CapabilityProcess;

/// <summary>
/// Demonstrates declaring Capability.Process to gain access to System.Diagnostics.Process types.
///
/// Without [PluginCapability(Capability.Process)], references to Process, ProcessStartInfo,
/// ProcessThread, ProcessModule etc. would cause the plugin to be REJECTED during
/// PluginLoader's security scan (see 09-ForbiddenProcess).
///
/// Key points:
///   1. Exempts Process* type-level bans under System.Diagnostics
///   2. ILString scanning: strings starting with "System.Diagnostics.Process" are not flagged
///   3. Only Process-related types are exempted — Stopwatch, Debug, Trace remain allowed (always were)
///   4. CommandLineExecutor is still the recommended way when possible
/// </summary>
[PluginCapability(Capability.Process, Reason = "Launch build tools for CI pipeline")]
public class CapabilityProcessPlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.capabilityprocess";
    public string Version => "1.0.0";
    public string GetName(Language language) => "Capability.Process Demo";
    public string GetDescription(Language language) =>
        "Demonstrates declaring Capability.Process for direct System.Diagnostics.Process access. " +
        "Contrasts with 09-ForbiddenProcess.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    public void OnLoad()
    {
        Console.WriteLine("[CapabilityProcess] Plugin loaded with Capability.Process declaration");
    }

    public void OnStart()
    {
        Console.WriteLine("\n========== Capability.Process Demo ==========");
        Console.WriteLine("Declared: [PluginCapability(Capability.Process, Reason = \"Launch build tools for CI pipeline\")]");
        Console.WriteLine();

        DemonstrateProcessUsage();
        DemonstrateExemptedTypes();
        DemonstrateRecommendedAlternative();
        DemonstrateStillForbidden();

        Console.WriteLine("\n========== Comparison with 09-ForbiddenProcess ==========");
        Console.WriteLine("  09-ForbiddenProcess:  No declaration → Process/ProcessStartInfo REJECTED");
        Console.WriteLine("  15-CapabilityProcess: [PluginCapability(Capability.Process)] → Process ALLOWED");
    }

    private void DemonstrateProcessUsage()
    {
        Console.WriteLine("[Demo 1] Direct Process usage");
        Console.WriteLine("  ✅ ALLOWED by Capability.Process:");
        Console.WriteLine("    var startInfo = new ProcessStartInfo(\"dotnet\")");
        Console.WriteLine("    {");
        Console.WriteLine("        Arguments = \"build\",
        Console.WriteLine("        RedirectStandardOutput = true,");
        Console.WriteLine("        UseShellExecute = false");
        Console.WriteLine("    };");
        Console.WriteLine("    using var process = Process.Start(startInfo);");
        Console.WriteLine("    string output = process.StandardOutput.ReadToEnd();");
        Console.WriteLine("    process.WaitForExit();");
        Console.WriteLine();
    }

    private void DemonstrateExemptedTypes()
    {
        Console.WriteLine("[Demo 2] Types exempted by Capability.Process");
        Console.WriteLine("  Only Process-related types under System.Diagnostics are exempted:");
        Console.WriteLine("  ✅ Process");
        Console.WriteLine("  ✅ ProcessStartInfo");
        Console.WriteLine("  ✅ ProcessThread, ProcessThreadCollection");
        Console.WriteLine("  ✅ ProcessModule, ProcessModuleCollection");
        Console.WriteLine("  ✅ ProcessPriorityClass, ProcessWindowStyle");
        Console.WriteLine();
        Console.WriteLine("  Types always allowed (not in forbidden list):");
        Console.WriteLine("  ✅ Stopwatch, Debug, Trace, Activity (always safe)");
        Console.WriteLine();
        Console.WriteLine("  ILString exemption:");
        Console.WriteLine("  ✅ Strings starting with \"System.Diagnostics.Process\" are not flagged");
        Console.WriteLine();
    }

    private void DemonstrateRecommendedAlternative()
    {
        Console.WriteLine("[Demo 3] Recommended alternative: CommandLineExecutor");
        Console.WriteLine("  CommandLineExecutor provides:");
        Console.WriteLine("  - Sandboxing: restricted command allowlist");
        Console.WriteLine("  - Timeouts: automatic process termination after timeout");
        Console.WriteLine("  - Output capture: structured stdout/stderr capture");
        Console.WriteLine("  - Audit logging: all process launches are logged");
        Console.WriteLine();
        Console.WriteLine("  Code pattern (no Capability.Process needed):");
        Console.WriteLine("    var result = await CommandLineExecutor.ExecuteAsync(");
        Console.WriteLine("        \"dotnet\", \"build\", timeout: TimeSpan.FromSeconds(60));");
        Console.WriteLine("    if (result.ExitCode == 0) { ... }");
        Console.WriteLine();
        Console.WriteLine("  When to use Capability.Process instead:");
        Console.WriteLine("  - Need fine-grained control over process I/O streams");
        Console.WriteLine("  - Need process event handling (Exited, OutputDataReceived)");
        Console.WriteLine("  - CommandLineExecutor's command allowlist is too restrictive");
        Console.WriteLine();
    }

    private void DemonstrateStillForbidden()
    {
        Console.WriteLine("[Demo 4] Still FORBIDDEN despite Capability.Process");
        Console.WriteLine("  ❌ [DllImport] — P/Invoke always blocked");
        Console.WriteLine("  ❌ Marshal / NativeMemory — native interop always blocked");
        Console.WriteLine("  ❌ System.Runtime.CompilerServices.Unsafe — unsafe code always blocked");
        Console.WriteLine("  ❌ System.Reflection.Emit — IL emission always blocked");
        Console.WriteLine("  ❌ System.Runtime.Loader — AssemblyLoadContext always blocked");
        Console.WriteLine("  ❌ Microsoft.Win32 — Registry access always blocked");
        Console.WriteLine();
    }

    public void OnStop()
    {
        Console.WriteLine("[CapabilityProcess] Plugin stopped.");
    }

    public void OnUnload()
    {
    }
}
