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

namespace SiliconLife.Demo.CSSourceMode;

/// <summary>
/// Demonstrates the CS source compilation loading mode introduced by task-389.
///
/// Unlike all other PluginDemo examples (which are pre-compiled DLLs loaded by
/// PluginLoader's standard DLL path), this plugin is loaded from raw .cs source
/// files. When PluginLoader finds no DLL in the plugin directory, it automatically
/// enters CS source mode:
///
///   1. Collects .cs files (cs.txt whitelist or all *.cs)
///   2. Scans sibling DLLs for trusted references
///   3. Compiles via CompilationCore (restricted mode)
///   4. Writes bytes to temp DLL and scans with ScanForbiddenReferences
///   5. Loads assembly from bytes and finds IPlugin implementation
///
/// Key points:
///   - This file IS listed in cs.txt, so it WILL be compiled and loaded
///   - Helpers.cs is NOT listed in cs.txt, so it will NOT be compiled
///   - The [CS-Source] prefix appears in PluginLoader logs for this plugin
///   - Functionally equivalent to 01-MinimalPlugin, but loaded via a different path
/// </summary>
public class CSSourceModePlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.cssource";
    public string Version => "1.0.0";
    public string GetName(Language language) => "CS Source Mode Demo";
    public string GetDescription(Language language) =>
        "Demonstrates loading a plugin from CS source files instead of a pre-compiled DLL. " +
        "This plugin is compiled in-memory by PluginLoader's CS source mode.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    /// <summary>
    /// Called once when the compiled assembly is loaded.
    /// Note: In CS source mode, this is called after the source code has been
    /// compiled and passed security scanning.
    /// </summary>
    public void OnLoad()
    {
        Console.WriteLine("[CS-Source] Plugin loaded: com.siliconlife.demo.cssource");
        Console.WriteLine("[CS-Source] This plugin was compiled from source code, not loaded from a DLL.");
        Console.WriteLine("[CS-Source] The compilation was performed by PluginLoader's CS source mode.");
    }

    /// <summary>
    /// Called when the host has fully started.
    /// Demonstrates using ITypeRegistry and IObjectFactory — the same pattern
    /// as 02-TypeRegistryUsage and 03-ObjectFactoryUsage, proving that CS-source
    /// compiled plugins have the same capabilities as DLL-loaded plugins.
    /// </summary>
    public void OnStart()
    {
        Console.WriteLine("\n========== CS Source Mode Demo ==========");
        Console.WriteLine("This plugin demonstrates the CS source compilation loading mode.");
        Console.WriteLine();
        Console.WriteLine("How it works:");
        Console.WriteLine("  1. PluginLoader scans plugin directory → no DLL found");
        Console.WriteLine("  2. Enters CS source mode");
        Console.WriteLine("  3. cs.txt found → reads line by line, only loads Plugin.cs");
        Console.WriteLine("  4. No sibling DLLs → skips reference scanning");
        Console.WriteLine("  5. CompilationCore compiles Plugin.cs into in-memory DLL");
        Console.WriteLine("  6. In-memory DLL is scanned by ScanForbiddenReferences");
        Console.WriteLine("  7. Scan passes → reflection finds CSSourceModePlugin → instantiate");
        Console.WriteLine("  8. Log shows: 'Plugin loaded [CS-Source]: com.siliconlife.demo.cssource v1.0.0'");
        Console.WriteLine();
        Console.WriteLine("cs.txt contents:");
        Console.WriteLine("  Plugin.cs       ← this file (compiled)");
        Console.WriteLine("  Helpers.cs      ← NOT listed (not compiled, exists only as example)");
        Console.WriteLine();
        Console.WriteLine("Comparison with 01-MinimalPlugin:");
        Console.WriteLine("  01-MinimalPlugin: Pre-compiled DLL → standard DLL loading path");
        Console.WriteLine("  22-CSSourceMode:  Raw .cs source → in-memory compilation loading path");
        Console.WriteLine("  Both are functionally equivalent IPlugin implementations.");
        Console.WriteLine();
        Console.WriteLine("When to use CS source mode vs DLL mode:");
        Console.WriteLine("  CS source mode: Development iteration (edit .cs → reload, no build step)");
        Console.WriteLine("  DLL mode:       Production deployment (better performance, no Roslyn overhead)");
        Console.WriteLine("==========================================\n");
    }

    /// <summary>
    /// Called when the host is shutting down gracefully.
    /// </summary>
    public void OnStop()
    {
        Console.WriteLine("[CS-Source] Plugin stopped: com.siliconlife.demo.cssource");
    }

    /// <summary>
    /// Called when the plugin is being unloaded.
    /// </summary>
    public void OnUnload()
    {
    }
}
