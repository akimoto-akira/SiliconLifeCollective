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
using System.Reflection;
using SiliconLife.Collective;

namespace SiliconLife.Demo.ForbiddenReflection;

/// <summary>
/// ⚠️ ANTI-PATTERN: Demonstrates reflection operations that are FORBIDDEN in plugins.
/// 
/// The following reflection methods are blocked from plugins (MemberRef scanning):
/// - Type.GetType(string) — dynamically resolve a type by name
/// - Activator.CreateInstance() — instantiate arbitrary types
/// - Assembly.Load / LoadFile / LoadFrom — dynamically load assemblies
/// - Assembly.GetType(string) — resolve type from a loaded assembly
/// 
/// These operations are dangerous because they could:
/// 1. Bypass PluginLoader's TypeRef scanning by resolving forbidden types at runtime
/// 2. Instantiate types that were never declared in metadata (invisible to static scan)
/// 3. Load untrusted assemblies that bypass all security checks
/// 4. Access internal/private framework types that should not be accessible
/// 
/// ✅ CORRECT APPROACH: Use ITypeRegistry.FindType + IObjectFactory.CreateInstance.
/// ITypeRegistry provides:
/// - Type lookup by full name (only from explicitly registered types)
/// - Subtype discovery (FindSubtypesOf / FindImplementationsOf)
/// IObjectFactory provides:
/// - Controlled instance creation via registered factory delegates
/// - Constructor analysis and automatic factory registration
/// 
/// NOTE: typeof(X).Assembly is SAFE because it references a compile-time known type.
/// Assembly.Load(string) is FORBIDDEN because it can load any arbitrary assembly at runtime.
/// 
/// This plugin demonstrates what NOT to do. Each violation is marked with
/// ⚠️ VIOLATION comment and shows the correct alternative.
/// </summary>
public class ForbiddenReflectionPlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.forbiddenreflection";
    public string Version => "1.0.0";
    public string GetName(Language language) => "Forbidden Reflection Anti-Pattern";
    public string GetDescription(Language language) =>
        "Demonstrates FORBIDDEN reflection operations and their correct alternatives. " +
        "Shows why Type.GetType/Activator.CreateInstance/Assembly.Load are banned " +
        "and how to use ITypeRegistry + IObjectFactory safely.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    public void OnLoad()
    {
    }

    public void OnStart()
    {
        Console.WriteLine("\n========== FORBIDDEN REFLECTION ANTI-PATTERNS ==========");
        Console.WriteLine("⚠️  This plugin demonstrates operations that will be BLOCKED by PluginLoader.\n");

        // NOTE: These methods demonstrate code patterns that would cause the plugin
        // to be REJECTED by PluginLoader during MemberRef scanning.
        // In a real scenario, PluginLoader would reject this plugin during loading.

        DemonstrateTypeGetType();
        DemonstrateActivatorCreateInstance();
        DemonstrateAssemblyLoad();
        DemonstrateAssemblyLoadFile();
        DemonstrateAssemblyGetType();

        Console.WriteLine("\n========== SAFE REFLECTION PATTERNS ==========");
        DemonstrateSafePatterns();

        Console.WriteLine("\n========== CORRECT ALTERNATIVES ==========");
        DemonstrateCorrectApproach();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Type.GetType(string) to dynamically resolve a type
    /// MemberRef blocked: System.Type::GetType(System.String)
    /// </summary>
    private void DemonstrateTypeGetType()
    {
        Console.WriteLine("[Violation 1] Type.GetType(string)");
        Console.WriteLine("  ⚠️ VIOLATION: [MemberRef] System.Type::GetType(System.String)");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     // Resolve a type by its assembly-qualified name at runtime");
        Console.WriteLine("     Type? fileType = Type.GetType(\"System.IO.File, System.Runtime\");");
        Console.WriteLine("     // Now you have access to a forbidden type!");
        Console.WriteLine("     var method = fileType?.GetMethod(\"ReadAllText\");");
        Console.WriteLine("     method?.Invoke(null, new object[] { \"secret.txt\" });");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     // Use ITypeRegistry to find only explicitly registered types");
        Console.WriteLine("     Type? myType = typeRegistry.FindType(\"MyPlugin.MyCustomType\");");
        Console.WriteLine("     // Only types registered during OnLoad are discoverable");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Activator.CreateInstance to instantiate arbitrary types
    /// MemberRef blocked: System.Activator::CreateInstance
    /// </summary>
    private void DemonstrateActivatorCreateInstance()
    {
        Console.WriteLine("[Violation 2] Activator.CreateInstance");
        Console.WriteLine("  ⚠️ VIOLATION: [MemberRef] System.Activator::CreateInstance");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     // Create an instance of any type, bypassing factory controls");
        Console.WriteLine("     Type? httpClientType = Type.GetType(\"System.Net.Http.HttpClient, System.Net.Http\");");
        Console.WriteLine("     object? client = Activator.CreateInstance(httpClientType!);");
        Console.WriteLine("     // Now you have an HttpClient without declaring Capability.Network!");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     // Use IObjectFactory to create instances from registered factories");
        Console.WriteLine("     var instance = objectFactory.CreateInstance<MyService>();");
        Console.WriteLine("     // Only types with registered factories can be instantiated");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Assembly.Load to dynamically load an assembly
    /// MemberRef blocked: System.Reflection.Assembly::Load(System.String)
    /// </summary>
    private void DemonstrateAssemblyLoad()
    {
        Console.WriteLine("[Violation 3] Assembly.Load");
        Console.WriteLine("  ⚠️ VIOLATION: [MemberRef] System.Reflection.Assembly::Load(System.String)");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     // Load any assembly by name, completely bypassing security scan");
        Console.WriteLine("     Assembly asm = Assembly.Load(\"System.Net.Http\");");
        Console.WriteLine("     Type? httpType = asm.GetType(\"System.Net.Http.HttpClient\");");
        Console.WriteLine("     object? client = Activator.CreateInstance(httpType!);");
        Console.WriteLine("     // Entire security model is defeated!");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     // Use typeof(X).Assembly for compile-time known assemblies (SAFE)");
        Console.WriteLine("     Assembly myAsm = typeof(MyPlugin).Assembly;  // ✅ This is safe");
        Console.WriteLine("     // Use ITypeRegistry.FindType for runtime type discovery");
        Console.WriteLine("     Type? type = typeRegistry.FindType(\"MyPlugin.SomeType\");");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Assembly.LoadFile / Assembly.LoadFrom to load DLL from disk
    /// MemberRef blocked: System.Reflection.Assembly::LoadFile(System.String)
    /// MemberRef blocked: System.Reflection.Assembly::LoadFrom(System.String)
    /// </summary>
    private void DemonstrateAssemblyLoadFile()
    {
        Console.WriteLine("[Violation 4] Assembly.LoadFile / LoadFrom");
        Console.WriteLine("  ⚠️ VIOLATION: [MemberRef] System.Reflection.Assembly::LoadFile(System.String)");
        Console.WriteLine("  ⚠️ VIOLATION: [MemberRef] System.Reflection.Assembly::LoadFrom(System.String)");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     // Load an arbitrary DLL from disk, no security scan applied");
        Console.WriteLine("     Assembly asm = Assembly.LoadFile(@\"C:\\malware\\evil.dll\");");
        Console.WriteLine("     // Or from a URL path");
        Console.WriteLine("     Assembly asm2 = Assembly.LoadFrom(@\"\\\\network\\share\\trojan.dll\");");
        Console.WriteLine("     // Loaded assembly has full trust, no PluginLoader scan!");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     // Plugins cannot load external DLLs. All dependencies must be");
        Console.WriteLine("     // placed in the plugin directory and scanned by PluginLoader.");
        Console.WriteLine("     // Use ITypeRegistry.RegisterFromAssembly in OnLoad for your own assembly.");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Assembly.GetType(string) to resolve type from loaded assembly
    /// MemberRef blocked: System.Reflection.Assembly::GetType(System.String)
    /// </summary>
    private void DemonstrateAssemblyGetType()
    {
        Console.WriteLine("[Violation 5] Assembly.GetType(string)");
        Console.WriteLine("  ⚠️ VIOLATION: [MemberRef] System.Reflection.Assembly::GetType(System.String)");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     // Even from a legitimately loaded assembly, string-based type resolution is blocked");
        Console.WriteLine("     Assembly runtime = typeof(object).Assembly;");
        Console.WriteLine("     Type? processType = runtime.GetType(\"System.Diagnostics.Process\");");
        Console.WriteLine("     // Bypasses TypeRef scanning since the type name is a runtime string");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     // Use ITypeRegistry.FindType which only searches registered types");
        Console.WriteLine("     Type? safeType = typeRegistry.FindType(\"MyPlugin.MySafeType\");");
        Console.WriteLine("     // Forbidden types are never registered, so they can never be found");
        Console.WriteLine();
    }

    /// <summary>
    /// Demonstrates reflection patterns that ARE safe and allowed.
    /// </summary>
    private void DemonstrateSafePatterns()
    {
        Console.WriteLine("[Safe Pattern 1] typeof(X).Assembly");
        Console.WriteLine("  ✅ SAFE: Accessing the assembly of a compile-time known type");
        Console.WriteLine("     Assembly myAsm = typeof(ForbiddenReflectionPlugin).Assembly;");
        Console.WriteLine("     // This is safe because the type is resolved at compile-time,");
        Console.WriteLine("     // visible in TypeRef table, and already scanned by PluginLoader.");
        Console.WriteLine();
        Console.WriteLine("[Safe Pattern 2] typeof(X).GetProperties() / GetMethods()");
        Console.WriteLine("  ✅ SAFE: Inspecting members of a compile-time known type");
        Console.WriteLine("     var props = typeof(MyData).GetProperties();");
        Console.WriteLine("     // Reflection on known types is safe — the type itself was already");
        Console.WriteLine("     // validated during loading. No new types are introduced.");
        Console.WriteLine();
        Console.WriteLine("[Safe Pattern 3] Generic type parameters");
        Console.WriteLine("  ✅ SAFE: Using generic constraints with known types");
        Console.WriteLine("     IEnumerable<Type> subtypes = typeRegistry.FindSubtypesOf(typeof(BaseTool));");
        Console.WriteLine("     // Generic parameters reference compile-time types, fully scannable.");
        Console.WriteLine();
    }

    /// <summary>
    /// Demonstrates the CORRECT way to do type discovery and object creation in plugins.
    /// </summary>
    private void DemonstrateCorrectApproach()
    {
        Console.WriteLine("[Correct Approach] Using ITypeRegistry + IObjectFactory");
        Console.WriteLine("  ✅ This is the SAFE way to discover types and create instances:");
        Console.WriteLine();
        Console.WriteLine("     // ── In OnLoad: Register your types ──");
        Console.WriteLine("     public void OnLoad()");
        Console.WriteLine("     {");
        Console.WriteLine("         // Register individual types");
        Console.WriteLine("         typeRegistry.RegisterType(typeof(MyCustomTool));");
        Console.WriteLine("         typeRegistry.RegisterType(typeof(MyService));");
        Console.WriteLine();
        Console.WriteLine("         // Or register all subtypes from your own assembly");
        Console.WriteLine("         typeRegistry.RegisterFromAssembly(typeof(MyPlugin).Assembly, typeof(BaseTool));");
        Console.WriteLine();
        Console.WriteLine("         // Register factories for dynamic creation");
        Console.WriteLine("         objectFactory.RegisterAutoFactory(typeof(MyCustomTool));");
        Console.WriteLine("         objectFactory.RegisterAutoFactory(typeof(MyService));");
        Console.WriteLine("     }");
        Console.WriteLine();
        Console.WriteLine("     // ── In OnStart: Discover and create ──");
        Console.WriteLine("     public void OnStart()");
        Console.WriteLine("     {");
        Console.WriteLine("         // Find a type by name (only registered types are discoverable)");
        Console.WriteLine("         Type? toolType = typeRegistry.FindType(\"MyPlugin.MyCustomTool\");");
        Console.WriteLine();
        Console.WriteLine("         // Find all subtypes of a base class");
        Console.WriteLine("         IEnumerable<Type> tools = typeRegistry.FindSubtypesOf(typeof(BaseTool));");
        Console.WriteLine();
        Console.WriteLine("         // Find all implementations of an interface");
        Console.WriteLine("         IEnumerable<Type> impls = typeRegistry.FindImplementationsOf(typeof(IMyService));");
        Console.WriteLine();
        Console.WriteLine("         // Create instances safely");
        Console.WriteLine("         var tool = objectFactory.CreateInstance<MyCustomTool>();");
        Console.WriteLine("         var service = objectFactory.CreateInstance(typeof(MyService));");
        Console.WriteLine("     }");
        Console.WriteLine();
        Console.WriteLine("  ⚠️  Security Notes:");
        Console.WriteLine("     - ITypeRegistry only contains types explicitly registered during OnLoad");
        Console.WriteLine("     - Forbidden types (System.IO.File, HttpClient, Process) are never registered");
        Console.WriteLine("     - IObjectFactory only creates instances for types with registered factories");
        Console.WriteLine("     - typeof(X).Assembly is safe; Assembly.Load(string) is not");
        Console.WriteLine("     - String-based reflection bypasses are caught by IL string scanning (#US heap)");
        Console.WriteLine();
    }

    public void OnStop()
    {
        Console.WriteLine("\n[ForbiddenReflection] Plugin stopped. No actual reflection operations were performed.");
    }

    public void OnUnload()
    {
    }
}
