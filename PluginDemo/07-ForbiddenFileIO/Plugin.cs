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

namespace SiliconLife.Demo.ForbiddenFileIO;

/// <summary>
/// ⚠️ ANTI-PATTERN: Demonstrates file I/O operations that are FORBIDDEN in plugins.
/// 
/// System.IO types that directly access the file system are globally banned from plugins:
/// - File, File.ReadAllText, File.WriteAllText, File.AppendAllText
/// - FileStream (when opened directly, not via PermissionedStreamFactory)
/// - Directory, Directory.GetFiles, Directory.CreateDirectory
/// - StreamReader/StreamWriter (when wrapping direct file paths)
/// 
/// These operations bypass the plugin security audit and could:
/// 1. Read sensitive files outside the workspace
/// 2. Overwrite critical system files
/// 3. Create files in unauthorized locations
/// 4. Traverse directory structures without permission checks
/// 
/// ✅ CORRECT APPROACH: Use PermissionedStreamFactory to get audited file access,
/// or use SpeedyPack for structured data storage.
/// 
/// This plugin demonstrates what NOT to do. Each violation is marked with
/// ⚠️ VIOLATION comment and shows the correct alternative.
/// </summary>
public class ForbiddenFileIOPlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.forbiddenfileio";
    public string Version => "1.0.0";
    public string GetName(Language language) => "Forbidden File I/O Anti-Pattern";
    public string GetDescription(Language language) =>
        "Demonstrates FORBIDDEN System.IO file operations and their correct alternatives. " +
        "Shows why direct file access is banned and how to use PermissionedStreamFactory.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    public void OnLoad()
    {
    }

    public void OnStart()
    {
        Console.WriteLine("\n========== FORBIDDEN FILE I/O ANTI-PATTERNS ==========");
        Console.WriteLine("⚠️  This plugin demonstrates operations that will be BLOCKED by PluginLoader.\n");

        // NOTE: These methods are commented out because they would cause compilation errors
        // when the plugin is loaded through PluginLoader (due to TypeRef scanning).
        // In a real scenario, PluginLoader would reject this plugin during loading.

        DemonstrateFileReadAllText();
        DemonstrateFileWriteAllText();
        DemonstrateDirectFileStream();
        DemonstrateDirectoryGetFiles();
        DemonstrateStreamReaderDirectPath();

        Console.WriteLine("\n========== CORRECT ALTERNATIVES ==========");
        DemonstrateCorrectApproach();
    }

    /// <summary>
    /// ⚠️ VIOLATION: File.ReadAllText
    /// TypeRef blocked: System.IO.File::ReadAllText(System.String)
    /// </summary>
    private void DemonstrateFileReadAllText()
    {
        Console.WriteLine("[Violation 1] File.ReadAllText");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.IO.File::ReadAllText");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     string content = File.ReadAllText(\"config.json\");");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     using var stream = PermissionedStreamFactory.OpenRead(\"config.json\");");
        Console.WriteLine("     using var reader = new StreamReader(stream);");
        Console.WriteLine("     string content = reader.ReadToEnd();");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: File.WriteAllText
    /// TypeRef blocked: System.IO.File::WriteAllText(System.String, System.String)
    /// </summary>
    private void DemonstrateFileWriteAllText()
    {
        Console.WriteLine("[Violation 2] File.WriteAllText");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.IO.File::WriteAllText");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     File.WriteAllText(\"output.log\", \"some data\");");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     using var stream = PermissionedStreamFactory.OpenWrite(\"output.log\");");
        Console.WriteLine("     using var writer = new StreamWriter(stream);");
        Console.WriteLine("     writer.Write(\"some data\");");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Direct FileStream construction
    /// TypeRef blocked: System.IO.FileStream::.ctor(System.String, System.IO.FileMode)
    /// </summary>
    private void DemonstrateDirectFileStream()
    {
        Console.WriteLine("[Violation 3] Direct FileStream");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.IO.FileStream::.ctor");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     using var fs = new FileStream(\"data.bin\", FileMode.Open);");
        Console.WriteLine("     fs.Read(buffer, 0, buffer.Length);");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     using var fs = PermissionedStreamFactory.OpenRead(\"data.bin\");");
        Console.WriteLine("     fs.Read(buffer, 0, buffer.Length);");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Directory.GetFiles
    /// TypeRef blocked: System.IO.Directory::GetFiles(System.String)
    /// </summary>
    private void DemonstrateDirectoryGetFiles()
    {
        Console.WriteLine("[Violation 4] Directory.GetFiles");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.IO.Directory::GetFiles");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     string[] files = Directory.GetFiles(\"./logs\", \"*.txt\");");
        Console.WriteLine("     foreach (var file in files) { ... }");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     // Use SpeedyPack for structured file enumeration:");
        Console.WriteLine("     using var pack = SpeedyPack.Open(\"logs.spk\");");
        Console.WriteLine("     var entries = pack.ListEntries(\"/\");");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: StreamReader with direct file path
    /// TypeRef blocked: System.IO.StreamReader::.ctor(System.String)
    /// </summary>
    private void DemonstrateStreamReaderDirectPath()
    {
        Console.WriteLine("[Violation 5] StreamReader with direct path");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.IO.StreamReader::.ctor(string)");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     using var reader = new StreamReader(\"config.json\");");
        Console.WriteLine("     string line = reader.ReadLine();");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     using var stream = PermissionedStreamFactory.OpenRead(\"config.json\");");
        Console.WriteLine("     using var reader = new StreamReader(stream);");
        Console.WriteLine("     string line = reader.ReadLine();");
        Console.WriteLine();
    }

    /// <summary>
    /// Demonstrates the CORRECT way to perform file operations in plugins.
    /// </summary>
    private void DemonstrateCorrectApproach()
    {
        Console.WriteLine("[Correct Approach] Using PermissionedStreamFactory");
        Console.WriteLine("  ✅ This is the SAFE way to access files:");
        Console.WriteLine();
        Console.WriteLine("     // PermissionedStreamFactory provides:");
        Console.WriteLine("     // 1. Path validation (prevents directory traversal)");
        Console.WriteLine("     // 2. Permission checking (workspace restrictions)");
        Console.WriteLine("     // 3. Audit logging (all access is recorded)");
        Console.WriteLine("     // 4. Resource cleanup tracking");
        Console.WriteLine();
        Console.WriteLine("     using var readStream = PermissionedStreamFactory.OpenRead(\"data.txt\");");
        Console.WriteLine("     using var reader = new StreamReader(readStream);");
        Console.WriteLine("     string content = reader.ReadToEnd();");
        Console.WriteLine();
        Console.WriteLine("     using var writeStream = PermissionedStreamFactory.OpenWrite(\"output.txt\");");
        Console.WriteLine("     using var writer = new StreamWriter(writeStream);");
        Console.WriteLine("     writer.Write(content.ToUpper());");
        Console.WriteLine();
        Console.WriteLine("  📚 For structured data storage, prefer SpeedyPack:");
        Console.WriteLine("     using var pack = SpeedyPack.Open(\"data.spk\");");
        Console.WriteLine("     pack.WriteText(\"config.json\", \"{\\\"key\\\": \\\"value\\\"}\");");
        Console.WriteLine("     string value = pack.ReadText(\"config.json\");");
        Console.WriteLine();
    }

    public void OnStop()
    {
        Console.WriteLine("\n[ForbiddenFileIO] Plugin stopped. No actual file operations were performed.");
    }

    public void OnUnload()
    {
    }
}
