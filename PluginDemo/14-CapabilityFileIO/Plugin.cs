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
using System.IO;
using System.Text;
using SiliconLife.Collective;

namespace SiliconLife.Demo.CapabilityFileIO;

/// <summary>
/// Demonstrates declaring Capability.FileIO to gain direct access to System.IO file-system types.
///
/// Without [PluginCapability(Capability.FileIO)], references to File, FileStream, Directory,
/// StreamReader(string), StreamWriter(string) etc. would cause the plugin to be REJECTED
/// during PluginLoader's security scan (see 07-ForbiddenFileIO).
///
/// By declaring the capability, PluginLoader relaxes the System.IO namespace ban beyond
/// the built-in SystemIOAllowedTypes whitelist (MemoryStream, BinaryReader, GZipStream, etc.).
///
/// Key points:
///   1. Exempts ALL of System.IO namespace — not just specific types
///   2. ILString scanning: strings starting with "System.IO." are not flagged
///   3. Plugins should STILL prefer PermissionedStreamFactory where possible
///   4. SpeedyPack does NOT require Capability.FileIO — it is the recommended alternative
/// </summary>
[PluginCapability(Capability.FileIO, Reason = "Direct log file access for audit trail")]
public class CapabilityFileIOPlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.capabilityfileio";
    public string Version => "1.0.0";
    public string GetName(Language language) => "Capability.FileIO Demo";
    public string GetDescription(Language language) =>
        "Demonstrates declaring Capability.FileIO for direct System.IO file access. " +
        "Contrasts with 07-ForbiddenFileIO and 04-SafeSystemIO.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    public void OnLoad()
    {
        Console.WriteLine("[CapabilityFileIO] Plugin loaded with Capability.FileIO declaration");
    }

    public void OnStart()
    {
        Console.WriteLine("\n========== Capability.FileIO Demo ==========");
        Console.WriteLine("Declared: [PluginCapability(Capability.FileIO, Reason = \"Direct log file access for audit trail\")]");
        Console.WriteLine();

        DemonstrateFileAccess();
        DemonstrateExemptedTypes();
        DemonstrateRecommendedAlternatives();
        DemonstrateStillForbidden();

        Console.WriteLine("\n========== Comparison ==========");
        Console.WriteLine("  04-SafeSystemIO:     No declaration — uses only MemoryStream/BinaryReader/GZipStream");
        Console.WriteLine("  07-ForbiddenFileIO:  No declaration — File/Directory REJECTED by PluginLoader");
        Console.WriteLine("  14-CapabilityFileIO: [PluginCapability(Capability.FileIO)] — File/Directory ALLOWED");
    }

    /// <summary>
    /// Demonstrates using File and Directory types directly.
    /// Without Capability.FileIO, these would trigger: [TypeRef] System.IO.File, [TypeRef] System.IO.Directory
    /// </summary>
    private void DemonstrateFileAccess()
    {
        Console.WriteLine("[Demo 1] Direct File and Directory access");
        Console.WriteLine("  ✅ ALLOWED by Capability.FileIO:");
        Console.WriteLine("    string content = File.ReadAllText(\"audit.log\");");
        Console.WriteLine("    File.WriteAllText(\"output.log\", \"entry\");");
        Console.WriteLine("    string[] files = Directory.GetFiles(\"./logs\");");
        Console.WriteLine("    using var fs = new FileStream(\"data.bin\", FileMode.Open);");
        Console.WriteLine("    using var reader = new StreamReader(\"config.json\"); // string ctor");
        Console.WriteLine();
    }

    /// <summary>
    /// Lists all types exempted by Capability.FileIO.
    /// </summary>
    private void DemonstrateExemptedTypes()
    {
        Console.WriteLine("[Demo 2] Types exempted by Capability.FileIO");
        Console.WriteLine("  ALL of System.IO namespace is exempted (beyond SystemIOAllowedTypes whitelist):");
        Console.WriteLine("  ✅ File, FileInfo, File.Load*, File.Write*");
        Console.WriteLine("  ✅ Directory, DirectoryInfo, Directory.Get*");
        Console.WriteLine("  ✅ FileStream (direct constructor with path)");
        Console.WriteLine("  ✅ StreamReader/StreamWriter (string path constructors)");
        Console.WriteLine("  ✅ FileSystemWatcher, Path, DriveInfo, etc.");
        Console.WriteLine();
        Console.WriteLine("  ILString exemption:");
        Console.WriteLine("  ✅ Strings starting with \"System.IO.\" are not flagged in #US heap scan");
        Console.WriteLine();
    }

    /// <summary>
    /// Even with Capability.FileIO, PermissionedStreamFactory and SpeedyPack are still recommended.
    /// </summary>
    private void DemonstrateRecommendedAlternatives()
    {
        Console.WriteLine("[Demo 3] Recommended alternatives (still prefer these when possible)");
        Console.WriteLine();
        Console.WriteLine("  Priority order for file access:");
        Console.WriteLine("  1️⃣  SpeedyPack — no Capability.FileIO needed, built-in caching/WAL/transactions");
        Console.WriteLine("     using var pack = SpeedyPack.Open(\"data.spk\");");
        Console.WriteLine("     pack.Write(\"key\", myObject);");
        Console.WriteLine("     var data = pack.Read<MyType>(\"key\");");
        Console.WriteLine();
        Console.WriteLine("  2️⃣  PermissionedStreamFactory — no Capability.FileIO needed, audited access");
        Console.WriteLine("     using var stream = PermissionedStreamFactory.OpenRead(\"file.txt\");");
        Console.WriteLine("     using var reader = new StreamReader(stream);");
        Console.WriteLine();
        Console.WriteLine("  3️⃣  Capability.FileIO + direct System.IO — only when above options don't suffice");
        Console.WriteLine("     File.WriteAllText(\"special.log\", \"data\");");
        Console.WriteLine();
        Console.WriteLine("  Why prefer SpeedyPack/PermissionedStreamFactory?");
        Console.WriteLine("  - Audit trail: all access is logged and traceable");
        Console.WriteLine("  - Path validation: prevents directory traversal attacks");
        Console.WriteLine("  - Access control: workspace boundary enforcement");
        Console.WriteLine("  - Resource tracking: prevents stream leaks");
        Console.WriteLine();
    }

    /// <summary>
    /// Undeclarable capabilities remain blocked even with Capability.FileIO.
    /// </summary>
    private void DemonstrateStillForbidden()
    {
        Console.WriteLine("[Demo 4] Still FORBIDDEN despite Capability.FileIO");
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
        Console.WriteLine("[CapabilityFileIO] Plugin stopped.");
    }

    public void OnUnload()
    {
    }
}
