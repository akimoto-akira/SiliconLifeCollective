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

namespace SiliconLife.Demo.CapabilityDenied;

/// <summary>
/// ⚠️ ANTI-PATTERN: Demonstrates that declaring a capability does NOT bypass undeclarable capability bans.
///
/// This plugin declares [PluginCapability(Capability.Network)], which exempts System.Net.* types.
/// However, the code also attempts to use DllImport, Marshal, Unsafe, and Reflection.Emit,
/// which are UNDECLARABLE capabilities — they are ALWAYS blocked regardless of any declaration.
///
/// Key points:
///   1. Capability.Network exempts System.Net.* but NOT P/Invoke, Unsafe, Reflection.Emit
///   2. No Capability enum value exists for undeclarable capabilities
///   3. PluginLoader enforces these checks REGARDLESS of any declared capabilities
///   4. Declaring an undefined Capability enum value is silently ignored by PluginLoader
///
/// This plugin would be REJECTED by PluginLoader due to the undeclarable capability violations.
/// The Capability.Network declaration is valid, but the P/Invoke/Unsafe violations override it.
/// </summary>
[PluginCapability(Capability.Network, Reason = "Network access for API calls")]
public class CapabilityDeniedPlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.capabilitydenied";
    public string Version => "1.0.0";
    public string GetName(Language language) => "Capability Denied Anti-Pattern";
    public string GetDescription(Language language) =>
        "Anti-pattern: declaring Capability.Network does NOT bypass undeclarable capability bans. " +
        "P/Invoke, Unsafe, Reflection.Emit are ALWAYS blocked.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    public void OnLoad()
    {
    }

    public void OnStart()
    {
        Console.WriteLine("\n========== Capability Denied Anti-Pattern ==========");
        Console.WriteLine("This plugin declares [PluginCapability(Capability.Network)] but attempts");
        Console.WriteLine("to use undeclarable capabilities. It would be REJECTED by PluginLoader.\n");

        DemonstrateUndeclarableCapabilities();
        DemonstrateBoundary();
        DemonstrateInvalidCapabilityValue();

        Console.WriteLine("\n========== Why These Capabilities Are Undeclarable ==========");
        Console.WriteLine("  They CANNOT be safely audited at runtime:");
        Console.WriteLine("  - P/Invoke: Arbitrary native code execution, no managed safety guarantees");
        Console.WriteLine("  - Unsafe: Bypasses CLR type safety and bounds checking");
        Console.WriteLine("  - Reflection.Emit: Can generate arbitrary IL at runtime");
        Console.WriteLine("  - AssemblyLoadContext: Can load unchecked DLLs bypassing security scan");
        Console.WriteLine("  - Registry: OS-level system access outside plugin sandbox");
    }

    /// <summary>
    /// Shows what happens when you try to use undeclarable capabilities despite declaring Capability.Network.
    /// </summary>
    private void DemonstrateUndeclarableCapabilities()
    {
        Console.WriteLine("[Violation 1] ⚠️ P/Invoke — ALWAYS blocked");
        Console.WriteLine("  [PluginCapability(Capability.Network)] does NOT exempt this:");
        Console.WriteLine("  ❌ [DllImport(\"kernel32.dll\")] — [PInvoke] scan catches this");
        Console.WriteLine("  ❌ Marshal.PtrToStringAnsi(ptr) — [TypeRef] System.Runtime.InteropServices.Marshal");
        Console.WriteLine("  ❌ NativeMemory.Alloc(100) — [TypeRef] System.Runtime.InteropServices.NativeMemory");
        Console.WriteLine();

        Console.WriteLine("[Violation 2] ⚠️ Unsafe code — ALWAYS blocked");
        Console.WriteLine("  ❌ [assembly: System.Security.UnverifiableCode] — [UnsafeMarker] scan catches this");
        Console.WriteLine("  ❌ System.Runtime.CompilerServices.Unsafe — [TypeRef] scan catches this");
        Console.WriteLine();

        Console.WriteLine("[Violation 3] ⚠️ Reflection.Emit — ALWAYS blocked");
        Console.WriteLine("  ❌ System.Reflection.Emit.DynamicMethod — [TypeRef] namespace ban catches this");
        Console.WriteLine("  ❌ System.Reflection.Emit.AssemblyBuilder — [TypeRef] namespace ban catches this");
        Console.WriteLine();

        Console.WriteLine("[Violation 4] ⚠️ Assembly Loading — ALWAYS blocked");
        Console.WriteLine("  ❌ System.Runtime.Loader.AssemblyLoadContext — [TypeRef] namespace ban catches this");
        Console.WriteLine("  ❌ Assembly.Load(byte[]) — [MemberRef] member ban catches this");
        Console.WriteLine();

        Console.WriteLine("[Violation 5] ⚠️ Registry — ALWAYS blocked");
        Console.WriteLine("  ❌ Microsoft.Win32.Registry — [TypeRef] namespace ban catches this");
        Console.WriteLine();
    }

    /// <summary>
    /// Shows the boundary between declarable and undeclarable capabilities.
    /// </summary>
    private void DemonstrateBoundary()
    {
        Console.WriteLine("[Boundary] Declarable vs. Undeclarable capabilities");
        Console.WriteLine();
        Console.WriteLine("  ✅ Declarable (Capability enum values exist):");
        Console.WriteLine("    Capability.Network  → exempts System.Net.*");
        Console.WriteLine("    Capability.FileIO   → exempts System.IO");
        Console.WriteLine("    Capability.Process  → exempts Process* types");
        Console.WriteLine("    Capability.AI       → enables IAIService injection");
        Console.WriteLine();
        Console.WriteLine("  ❌ Undeclarable (NO Capability enum value exists):");
        Console.WriteLine("    P/Invoke / native interop  → always blocked");
        Console.WriteLine("    Unsafe code                 → always blocked");
        Console.WriteLine("    Reflection.Emit             → always blocked");
        Console.WriteLine("    AssemblyLoadContext         → always blocked");
        Console.WriteLine("    Registry access             → always blocked");
        Console.WriteLine("    Dynamic compilation         → always blocked");
        Console.WriteLine();
    }

    /// <summary>
    /// Shows that declaring an invalid/undefined Capability enum value is silently ignored.
    /// </summary>
    private void DemonstrateInvalidCapabilityValue()
    {
        Console.WriteLine("[Edge Case] Invalid Capability enum value");
        Console.WriteLine("  PluginLoader reads the CustomAttribute blob as an int32 enum value.");
        Console.WriteLine("  If the value does not match any defined Capability enum member,");
        Console.WriteLine("  it is silently ignored (Enum.IsDefined check in ReadDeclaredCapabilities).");
        Console.WriteLine();
        Console.WriteLine("  Example: [PluginCapability((Capability)99)] → ignored, no exemption granted");
        Console.WriteLine("  This prevents plugins from declaring 'future' capabilities that don't exist yet.");
        Console.WriteLine();
    }

    public void OnStop()
    {
        Console.WriteLine("[CapabilityDenied] Plugin would be REJECTED by PluginLoader in production.");
    }

    public void OnUnload()
    {
    }
}
