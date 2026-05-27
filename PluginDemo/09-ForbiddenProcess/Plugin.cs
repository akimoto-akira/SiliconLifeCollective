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

namespace SiliconLife.Demo.ForbiddenProcess;

/// <summary>
/// ⚠️ ANTI-PATTERN: Demonstrates process operations that are FORBIDDEN in plugins.
/// 
/// System.Diagnostics.Process and ProcessStartInfo are blocked from plugins:
/// - Process.Start(), ProcessStartInfo
/// - Process.Kill(), Process.WaitForExit()
/// - Process.StandardInput/Output/Error redirection
/// 
/// These operations are dangerous because they could:
/// 1. Execute arbitrary commands without audit
/// 2. Launch malware or unwanted applications
/// 3. Access system resources without permission checks
/// 4. Bypass the plugin security sandbox
/// 
/// ✅ CORRECT APPROACH: Use CommandLineExecutor for safe, audited command execution.
/// CommandLineExecutor provides:
/// - Command injection protection (blocks dangerous separators like ||, &&, |, &, ;)
/// - Timeout enforcement (default 30 seconds)
/// - Audit logging (all commands are logged)
/// - Output capture and error handling
/// 
/// NOTE: Only Process-related types are forbidden, not the entire System.Diagnostics namespace.
/// Types like Stopwatch, Debug, Trace remain available.
/// 
/// This plugin demonstrates what NOT to do. Each violation is marked with
/// ⚠️ VIOLATION comment and shows the correct alternative.
/// </summary>
public class ForbiddenProcessPlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.forbiddenprocess";
    public string Version => "1.0.0";
    public string GetName(Language language) => "Forbidden Process Anti-Pattern";
    public string GetDescription(Language language) =>
        "Demonstrates FORBIDDEN Process/ProcessStartInfo operations and their correct alternatives. " +
        "Shows why direct process execution is banned and how to use CommandLineExecutor.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    public void OnLoad()
    {
    }

    public void OnStart()
    {
        Console.WriteLine("\n========== FORBIDDEN PROCESS ANTI-PATTERNS ==========");
        Console.WriteLine("⚠️  This plugin demonstrates operations that will be BLOCKED by PluginLoader.\n");

        // NOTE: These methods are commented out because they would cause compilation errors
        // when the plugin is loaded through PluginLoader (due to TypeRef scanning).
        // In a real scenario, PluginLoader would reject this plugin during loading.

        DemonstrateProcessStart();
        DemonstrateProcessStartInfo();
        DemonstrateProcessWithArguments();
        DemonstrateProcessOutputRedirection();
        DemonstrateProcessKill();

        Console.WriteLine("\n========== CORRECT ALTERNATIVES ==========");
        DemonstrateCorrectApproach();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Process.Start with simple command
    /// TypeRef blocked: System.Diagnostics.Process::Start(System.String)
    /// </summary>
    private void DemonstrateProcessStart()
    {
        Console.WriteLine("[Violation 1] Process.Start");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Diagnostics.Process::Start(string)");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     Process.Start(\"notepad.exe\");");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     var request = new ExecutorRequest { ResourcePath = \"notepad.exe\" };");
        Console.WriteLine("     var result = CommandLineExecutor.Execute(request);");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: ProcessStartInfo with detailed configuration
    /// TypeRef blocked: System.Diagnostics.ProcessStartInfo::.ctor()
    /// </summary>
    private void DemonstrateProcessStartInfo()
    {
        Console.WriteLine("[Violation 2] ProcessStartInfo");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Diagnostics.ProcessStartInfo::.ctor");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     var psi = new ProcessStartInfo {");
        Console.WriteLine("         FileName = \"cmd.exe\",");
        Console.WriteLine("         Arguments = \"/c dir\",");
        Console.WriteLine("         UseShellExecute = false,");
        Console.WriteLine("         RedirectStandardOutput = true");
        Console.WriteLine("     };");
        Console.WriteLine("     using var process = new Process { StartInfo = psi };");
        Console.WriteLine("     process.Start();");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     var request = new ExecutorRequest { ResourcePath = \"dir\" };");
        Console.WriteLine("     var result = CommandLineExecutor.Execute(request);");
        Console.WriteLine("     Console.WriteLine(result.Output);");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Process with command-line arguments
    /// TypeRef blocked: System.Diagnostics.Process::Start(System.Diagnostics.ProcessStartInfo)
    /// </summary>
    private void DemonstrateProcessWithArguments()
    {
        Console.WriteLine("[Violation 3] Process with arguments");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Diagnostics.Process::Start(ProcessStartInfo)");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     var psi = new ProcessStartInfo(\"ping\", \"127.0.0.1 -n 4\") {");
        Console.WriteLine("         UseShellExecute = false,");
        Console.WriteLine("         RedirectStandardOutput = true");
        Console.WriteLine("     };");
        Console.WriteLine("     using var process = Process.Start(psi);");
        Console.WriteLine("     process.WaitForExit();");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     var request = new ExecutorRequest { ResourcePath = \"ping 127.0.0.1 -n 4\" };");
        Console.WriteLine("     var result = CommandLineExecutor.Execute(request, TimeSpan.FromSeconds(10));");
        Console.WriteLine("     Console.WriteLine(result.Output);");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Process output redirection
    /// TypeRef blocked: System.Diagnostics.Process::get_StandardOutput()
    /// </summary>
    private void DemonstrateProcessOutputRedirection()
    {
        Console.WriteLine("[Violation 4] Process output redirection");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Diagnostics.Process::StandardOutput");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     var psi = new ProcessStartInfo(\"ipconfig\") {");
        Console.WriteLine("         UseShellExecute = false,");
        Console.WriteLine("         RedirectStandardOutput = true,");
        Console.WriteLine("         RedirectStandardError = true");
        Console.WriteLine("     };");
        Console.WriteLine("     using var process = Process.Start(psi);");
        Console.WriteLine("     string output = process.StandardOutput.ReadToEnd();");
        Console.WriteLine("     string error = process.StandardError.ReadToEnd();");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     var request = new ExecutorRequest { ResourcePath = \"ipconfig\" };");
        Console.WriteLine("     var result = CommandLineExecutor.Execute(request);");
        Console.WriteLine("     if (result.Success) Console.WriteLine(result.Output);");
        Console.WriteLine("     else Console.WriteLine(result.Error);");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Process.Kill
    /// TypeRef blocked: System.Diagnostics.Process::Kill()
    /// </summary>
    private void DemonstrateProcessKill()
    {
        Console.WriteLine("[Violation 5] Process.Kill");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Diagnostics.Process::Kill");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     Process[] processes = Process.GetProcessesByName(\"notepad\");");
        Console.WriteLine("     foreach (var p in processes) p.Kill();");
        Console.WriteLine("  ✅ CORRECT APPROACH:");
        Console.WriteLine("     // Process killing is not supported through CommandLineExecutor");
        Console.WriteLine("     // for security reasons. Contact system administrator if needed.");
        Console.WriteLine();
    }

    /// <summary>
    /// Demonstrates the CORRECT way to execute commands in plugins.
    /// </summary>
    private void DemonstrateCorrectApproach()
    {
        Console.WriteLine("[Correct Approach] Using CommandLineExecutor");
        Console.WriteLine("  ✅ This is the SAFE way to execute commands:");
        Console.WriteLine();
        Console.WriteLine("     // CommandLineExecutor provides:");
        Console.WriteLine("     // 1. Command injection protection (blocks ||, &&, |, &, ;)");
        Console.WriteLine("     // 2. Timeout enforcement (default 30s, configurable)");
        Console.WriteLine("     // 3. Audit logging (all commands are recorded)");
        Console.WriteLine("     // 4. Output capture and error handling");
        Console.WriteLine("     // 5. Cross-platform support (cmd.exe on Windows, /bin/bash on Unix)");
        Console.WriteLine();
        Console.WriteLine("     // Example 1: Simple command");
        Console.WriteLine("     var request1 = new ExecutorRequest { ResourcePath = \"dir\" };");
        Console.WriteLine("     var result1 = CommandLineExecutor.Execute(request1);");
        Console.WriteLine("     if (result1.Success) Console.WriteLine(result1.Output);");
        Console.WriteLine();
        Console.WriteLine("     // Example 2: Command with timeout");
        Console.WriteLine("     var request2 = new ExecutorRequest { ResourcePath = \"ping 127.0.0.1\" };");
        Console.WriteLine("     var result2 = CommandLineExecutor.Execute(request2, TimeSpan.FromSeconds(10));");
        Console.WriteLine();
        Console.WriteLine("     // Example 3: Check execution result");
        Console.WriteLine("     if (!result2.Success)");
        Console.WriteLine("     {");
        Console.WriteLine("         Console.WriteLine($\"Error: {result2.Error}\");");
        Console.WriteLine("         Console.WriteLine($\"Exit Code: {result2.ExitCode}\");");
        Console.WriteLine("     }");
        Console.WriteLine();
        Console.WriteLine("  ⚠️  Security Notes:");
        Console.WriteLine("     - Multi-command execution is blocked (e.g., \"cmd1 && cmd2\")");
        Console.WriteLine("     - Dangerous separators are detected and rejected");
        Console.WriteLine("     - All command execution is logged for audit");
        Console.WriteLine("     - Commands run with plugin's permission level");
        Console.WriteLine();
    }

    public void OnStop()
    {
        Console.WriteLine("\n[ForbiddenProcess] Plugin stopped. No actual process operations were performed.");
    }

    public void OnUnload()
    {
    }
}
