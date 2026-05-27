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

namespace SiliconLife.Demo.ForbiddenStringBypass;

/// <summary>
/// ⚠️ ANTI-PATTERN: Demonstrates string-based reflection bypass attempts that are CAUGHT
/// by PluginLoader's #US (User String) heap scanning — the LAST LINE OF DEFENSE.
/// 
/// 【How #US Heap Scanning Works】
/// When C# code contains a string literal, the compiler emits an `ldstr` IL instruction
/// that references a token in the #US (User String) heap of the PE metadata.
/// PluginLoader scans ALL entries in the #US heap and checks each against a list of
/// forbidden prefixes (ForbiddenStringPrefixes). This means:
/// 
/// 1. String concatenation at compile time → the FULL result is in #US heap → CAUGHT
/// 2. String interpolation → the template parts are in #US heap → CAUGHT
/// 3. Const string fields → their values are inlined into #US heap → CAUGHT
/// 4. Runtime string building (char arrays) → bypasses #US scan BUT
///    still needs Type.GetType/Assembly.Load which are blocked by MemberRef scanning
/// 
/// 【Why This is the Last Line of Defense】
/// Even if a plugin somehow avoids TypeRef and MemberRef detection, any string constant
/// matching a forbidden prefix will trigger [ILString] violation. Combined with MemberRef
/// blocking of Type.GetType/Assembly.Load, there is NO way to dynamically resolve
/// forbidden types through strings.
/// 
/// 【Forbidden String Prefixes (ForbiddenStringPrefixes)】
/// - "System.IO."                    → File system types
/// - "System.Net.Http"               → HTTP
/// - "System.Net.WebSockets"         → WebSocket
/// - "System.Net.Sockets"            → Raw sockets
/// - "System.Net.Mail"               → SMTP
/// - "System.Net.NetworkInformation" → Network probing
/// - "System.Net.Security"           → SslStream
/// - "System.Diagnostics.Process"    → Process / command line
/// - "Microsoft.CodeAnalysis"        → Roslyn
/// - "System.Reflection.Emit"        → IL emission
/// - "System.Runtime.Loader"         → AssemblyLoadContext
/// - "System.CodeDom.Compiler"       → Legacy CodeDom
/// - "Microsoft.Win32"               → Registry
/// 
/// This plugin demonstrates what NOT to do. Each violation is marked with
/// ⚠️ VIOLATION: [ILString] comment indicating the scanned string.
/// </summary>
public class ForbiddenStringBypassPlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.forbiddenstringbypass";
    public string Version => "1.0.0";
    public string GetName(Language language) => "Forbidden String Bypass Anti-Pattern";
    public string GetDescription(Language language) =>
        "Demonstrates FORBIDDEN string-based reflection bypass attempts. " +
        "Shows why string concatenation, interpolation, and obfuscation cannot " +
        "bypass PluginLoader's #US heap scanning mechanism.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    public void OnLoad()
    {
    }

    public void OnStart()
    {
        Console.WriteLine("\n========== FORBIDDEN STRING BYPASS ANTI-PATTERNS ==========");
        Console.WriteLine("⚠️  This plugin demonstrates string tricks that are ALL caught by #US heap scanning.\n");

        // NOTE: These methods demonstrate code patterns that would cause the plugin
        // to be REJECTED by PluginLoader during #US heap string scanning (Step 5).
        // In a real scenario, PluginLoader would reject this plugin at load time.

        DemonstrateDirectString();
        DemonstrateStringConcatenation();
        DemonstrateStringInterpolation();
        DemonstrateConstFields();
        DemonstratePartialStrings();
        DemonstrateMultipleTargets();

        Console.WriteLine("\n========== WHY OBFUSCATION FAILS ==========");
        DemonstrateWhyObfuscationFails();

        Console.WriteLine("\n========== THE COMPLETE DEFENSE CHAIN ==========");
        DemonstrateDefenseChain();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Direct forbidden type name string used with Type.GetType.
    /// The full string "System.IO.File, System.Runtime" is stored in #US heap.
    /// </summary>
    private void DemonstrateDirectString()
    {
        Console.WriteLine("[Violation 1] Direct Type Name String");
        Console.WriteLine("  ⚠️ VIOLATION: [ILString] \"System.IO.File, System.Runtime\" matches forbidden prefix \"System.IO.\"");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     // Direct string literal containing forbidden type name");
        Console.WriteLine("     Type? fileType = Type.GetType(\"System.IO.File, System.Runtime\");");
        Console.WriteLine("     var method = fileType?.GetMethod(\"ReadAllText\");");
        Console.WriteLine("     method?.Invoke(null, new object[] { \"secret.txt\" });");
        Console.WriteLine();
        Console.WriteLine("  🔍 DETECTION:");
        Console.WriteLine("     The string \"System.IO.File, System.Runtime\" is stored in the #US heap.");
        Console.WriteLine("     ScanUserStrings() iterates ALL #US entries and checks StartsWith(\"System.IO.\").");
        Console.WriteLine("     → Match found → Violation reported");
        Console.WriteLine();
        Console.WriteLine("  💡 DOUBLE BLOCK:");
        Console.WriteLine("     Even without #US scanning, Type.GetType is blocked by MemberRef scan.");
        Console.WriteLine("     This is defense-in-depth: two independent layers catch this attack.");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: String concatenation — compiler resolves const concatenation at compile time,
    /// placing the FULL result in #US heap. Non-const concatenation still produces partial strings.
    /// </summary>
    private void DemonstrateStringConcatenation()
    {
        Console.WriteLine("[Violation 2] String Concatenation (Compile-Time)");
        Console.WriteLine("  ⚠️ VIOLATION: [ILString] \"System.Net.Http.HttpClient, System.Net.Http\" matches forbidden prefix \"System.Net.Http\"");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     // Attacker tries to split the string to avoid detection");
        Console.WriteLine("     const string ns = \"System.Net.Http\";");
        Console.WriteLine("     const string typeName = \".HttpClient\";");
        Console.WriteLine("     const string assembly = \", System.Net.Http\";");
        Console.WriteLine("     // But const + const = compiler folds into one string in #US heap!");
        Console.WriteLine("     Type? type = Type.GetType(ns + typeName + assembly);");
        Console.WriteLine();
        Console.WriteLine("  🔍 DETECTION:");
        Console.WriteLine("     C# compiler performs constant folding on const string concatenation.");
        Console.WriteLine("     The final result \"System.Net.Http.HttpClient, System.Net.Http\" is stored");
        Console.WriteLine("     as a single entry in the #US heap.");
        Console.WriteLine("     Additionally, each const string part (\"System.Net.Http\", etc.) is ALSO");
        Console.WriteLine("     stored separately in the #US heap as individual ldstr operands.");
        Console.WriteLine("     → Multiple violations from a single concatenation!");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: String interpolation — template parts are stored in #US heap.
    /// </summary>
    private void DemonstrateStringInterpolation()
    {
        Console.WriteLine("[Violation 3] String Interpolation");
        Console.WriteLine("  ⚠️ VIOLATION: [ILString] \"System.IO.FileStream\" matches forbidden prefix \"System.IO.\"");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     string className = \"FileStream\";");
        Console.WriteLine("     // Interpolation: the format string contains the forbidden prefix");
        Console.WriteLine("     string fullName = $\"System.IO.{className}, System.Runtime\";");
        Console.WriteLine("     Type? type = Type.GetType(fullName);");
        Console.WriteLine();
        Console.WriteLine("  🔍 DETECTION:");
        Console.WriteLine("     String interpolation compiles to string.Format or string.Concat.");
        Console.WriteLine("     The literal parts (\"System.IO.\" and \", System.Runtime\") are stored");
        Console.WriteLine("     as separate entries in the #US heap.");
        Console.WriteLine("     The prefix \"System.IO.\" alone is enough to trigger the violation!");
        Console.WriteLine("     → Partial strings still match forbidden prefixes");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Const fields — values are inlined into #US heap at every usage site.
    /// </summary>
    private void DemonstrateConstFields()
    {
        Console.WriteLine("[Violation 4] Const Fields");
        Console.WriteLine("  ⚠️ VIOLATION: [ILString] \"System.Diagnostics.Process\" matches forbidden prefix \"System.Diagnostics.Process\"");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     // Hiding the string in a const field does NOT help");
        Console.WriteLine("     private const string ProcessType = \"System.Diagnostics.Process\";");
        Console.WriteLine("     private const string AssemblyName = \", System.Runtime\";");
        Console.WriteLine();
        Console.WriteLine("     // The const value is inlined by the compiler at every usage site");
        Console.WriteLine("     Type? type = Type.GetType(ProcessType + AssemblyName);");
        Console.WriteLine();
        Console.WriteLine("  🔍 DETECTION:");
        Console.WriteLine("     Const fields are NOT stored as field values in metadata.");
        Console.WriteLine("     Instead, the compiler inlines their values directly into the IL");
        Console.WriteLine("     as ldstr instructions → values appear in #US heap.");
        Console.WriteLine("     → Const fields provide ZERO obfuscation");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Even splitting into partial strings that individually match a prefix.
    /// </summary>
    private void DemonstratePartialStrings()
    {
        Console.WriteLine("[Violation 5] Partial String Fragments");
        Console.WriteLine("  ⚠️ VIOLATION: [ILString] \"System.Reflection.Emit.AssemblyBuilder\" matches forbidden prefix \"System.Reflection.Emit\"");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     // Even partial fragments that match a prefix are caught");
        Console.WriteLine("     string part1 = \"System.Reflection.Emit\";");
        Console.WriteLine("     string part2 = \".AssemblyBuilder\";");
        Console.WriteLine("     string fullType = part1 + part2;");
        Console.WriteLine("     // part1 alone already matches \"System.Reflection.Emit\" prefix!");
        Console.WriteLine();
        Console.WriteLine("  🔍 DETECTION:");
        Console.WriteLine("     Each string literal is stored separately in #US heap.");
        Console.WriteLine("     \"System.Reflection.Emit\" by itself matches the forbidden prefix.");
        Console.WriteLine("     You cannot split a forbidden string into parts where NONE match a prefix");
        Console.WriteLine("     unless you split within the prefix itself (e.g., \"System.\" + \"Reflection.Emit\").");
        Console.WriteLine("     But then you still need Type.GetType which is MemberRef-blocked!");
        Console.WriteLine("     → Splitting provides NO escape route");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Multiple forbidden strings targeting different namespaces.
    /// </summary>
    private void DemonstrateMultipleTargets()
    {
        Console.WriteLine("[Violation 6] Multiple Forbidden Targets");
        Console.WriteLine("  ⚠️ VIOLATION: [ILString] \"System.Net.Sockets.TcpClient\" matches forbidden prefix \"System.Net.Sockets\"");
        Console.WriteLine("  ⚠️ VIOLATION: [ILString] \"Microsoft.Win32.Registry\" matches forbidden prefix \"Microsoft.Win32\"");
        Console.WriteLine("  ⚠️ VIOLATION: [ILString] \"System.Runtime.Loader.AssemblyLoadContext\" matches forbidden prefix \"System.Runtime.Loader\"");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     // Every forbidden string in the entire assembly is caught");
        Console.WriteLine("     string tcp = \"System.Net.Sockets.TcpClient\";");
        Console.WriteLine("     string registry = \"Microsoft.Win32.Registry\";");
        Console.WriteLine("     string loader = \"System.Runtime.Loader.AssemblyLoadContext\";");
        Console.WriteLine();
        Console.WriteLine("  🔍 DETECTION:");
        Console.WriteLine("     #US heap scanning checks ALL strings in the entire assembly.");
        Console.WriteLine("     It does NOT stop at the first violation — it collects ALL of them.");
        Console.WriteLine("     The final report lists every single forbidden string found.");
        Console.WriteLine("     → No amount of scattering strings across methods can help");
        Console.WriteLine();
    }

    /// <summary>
    /// Explains why string obfuscation techniques all fail against #US heap scanning.
    /// </summary>
    private void DemonstrateWhyObfuscationFails()
    {
        Console.WriteLine("[Why Obfuscation Fails]");
        Console.WriteLine();
        Console.WriteLine("  Common obfuscation attempts and why they ALL fail:");
        Console.WriteLine();
        Console.WriteLine("  ┌───────────────────────────────────────────────────────────────────────┐");
        Console.WriteLine("  │ Technique               │ Why It Fails                                │");
        Console.WriteLine("  ├───────────────────────────────────────────────────────────────────────┤");
        Console.WriteLine("  │ Const concatenation     │ Compiler folds into single #US entry        │");
        Console.WriteLine("  │ String interpolation    │ Literal parts stored in #US heap            │");
        Console.WriteLine("  │ Const fields            │ Values inlined at usage → appear in #US     │");
        Console.WriteLine("  │ Split into variables    │ Each ldstr operand scanned independently    │");
        Console.WriteLine("  │ Base64 encoding         │ Decode needs Convert.FromBase64String       │");
        Console.WriteLine("  │                         │ + Encoding.GetString → result is runtime    │");
        Console.WriteLine("  │                         │ string, BUT Type.GetType is MemberRef-blocked│");
        Console.WriteLine("  │ Char array building     │ new char[]{'S','y','s',...} has no ldstr     │");
        Console.WriteLine("  │                         │ BUT Type.GetType is still MemberRef-blocked │");
        Console.WriteLine("  │ XOR encryption          │ Encrypted string is not readable in #US     │");
        Console.WriteLine("  │                         │ BUT decryption + Type.GetType = MemberRef   │");
        Console.WriteLine("  │ Reverse string          │ Reversed literal still in #US if it matches │");
        Console.WriteLine("  │                         │ AND string.Reverse + GetType = MemberRef    │");
        Console.WriteLine("  └───────────────────────────────────────────────────────────────────────┘");
        Console.WriteLine();
        Console.WriteLine("  KEY INSIGHT:");
        Console.WriteLine("  The #US heap scan catches the STRING. The MemberRef scan catches the METHOD.");
        Console.WriteLine("  To dynamically load a type, you need BOTH a string AND a resolution method.");
        Console.WriteLine("  PluginLoader blocks BOTH independently:");
        Console.WriteLine("  - #US scan → blocks the forbidden string from existing in the assembly");
        Console.WriteLine("  - MemberRef scan → blocks Type.GetType/Assembly.Load/Activator.CreateInstance");
        Console.WriteLine("  → Even if you bypass ONE layer, the OTHER still catches you!");
        Console.WriteLine();
    }

    /// <summary>
    /// Shows the complete defense chain and how #US scanning fits as the last line.
    /// </summary>
    private void DemonstrateDefenseChain()
    {
        Console.WriteLine("[The Complete Defense Chain]");
        Console.WriteLine();
        Console.WriteLine("  PluginLoader's 5-step scanning creates an unbreakable defense:");
        Console.WriteLine();
        Console.WriteLine("  ┌─────────────────────────────────────────────────────────────────────────┐");
        Console.WriteLine("  │ Step │ Mechanism              │ What It Catches                          │");
        Console.WriteLine("  ├─────────────────────────────────────────────────────────────────────────┤");
        Console.WriteLine("  │  1   │ TypeRef Table           │ Direct references to forbidden types    │");
        Console.WriteLine("  │      │                         │ (System.IO.File, HttpClient, etc.)      │");
        Console.WriteLine("  ├─────────────────────────────────────────────────────────────────────────┤");
        Console.WriteLine("  │  2   │ ExportedType Table      │ Forwarded types from forbidden          │");
        Console.WriteLine("  │      │                         │ namespaces                               │");
        Console.WriteLine("  ├─────────────────────────────────────────────────────────────────────────┤");
        Console.WriteLine("  │  3   │ MemberRef Table         │ Calls to Type.GetType, Assembly.Load,  │");
        Console.WriteLine("  │      │                         │ Activator.CreateInstance, etc.          │");
        Console.WriteLine("  ├─────────────────────────────────────────────────────────────────────────┤");
        Console.WriteLine("  │  4   │ Unsafe Markers          │ [DllImport], unsafe blocks,             │");
        Console.WriteLine("  │      │                         │ PinvokeImpl flag                         │");
        Console.WriteLine("  ├─────────────────────────────────────────────────────────────────────────┤");
        Console.WriteLine("  │  5   │ #US Heap Scan           │ String constants matching forbidden     │");
        Console.WriteLine("  │      │ (THIS DEMO)             │ prefixes — THE LAST LINE OF DEFENSE    │");
        Console.WriteLine("  └─────────────────────────────────────────────────────────────────────────┘");
        Console.WriteLine();
        Console.WriteLine("  Why Step 5 is the \"Last Line\":");
        Console.WriteLine("  - Steps 1-4 catch ACTIVE code (types, methods, attributes)");
        Console.WriteLine("  - Step 5 catches PASSIVE data (strings that COULD be used for bypass)");
        Console.WriteLine("  - Even if an attacker finds a way to call a type-resolution method");
        Console.WriteLine("    that we didn't block in MemberRef, the forbidden type name string");
        Console.WriteLine("    MUST exist somewhere in the assembly → #US scan catches it");
        Console.WriteLine();
        Console.WriteLine("  ⚠️  TRADE-OFF: Minor false positives may occur.");
        Console.WriteLine("  If your plugin contains a log message like:");
        Console.WriteLine("    logger.Info(\"Failed to connect to System.Net.Http endpoint\");");
        Console.WriteLine("  This string matches the \"System.Net.Http\" prefix and will be flagged.");
        Console.WriteLine("  This is an ACCEPTED trade-off for closing the security loophole.");
        Console.WriteLine("  Solution: Rephrase log messages to avoid forbidden prefixes.");
        Console.WriteLine();
    }

    public void OnStop()
    {
        Console.WriteLine("\n[ForbiddenStringBypass] Plugin stopped. No actual reflection operations were performed.");
    }

    public void OnUnload()
    {
    }
}
