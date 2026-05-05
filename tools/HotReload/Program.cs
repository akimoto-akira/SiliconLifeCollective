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
//
// SiliconLife 热更新器 (HotReload.exe)
// ------------------------------------------------------------------
// 目标：配合 HotReloadTool 完成一次"曲线救国"式热更新流程：
//
//   1. POST /api/system/shutdown 优雅关闭正在运行的 Fast 实例
//   2. 等待 SiliconLife.Fast 进程退出
//   3. 将 --source 目录中的所有文件复制到 --target 目录（默认是本 exe 所在目录）
//      —— 跳过 HotReload.* 自身文件，避免覆盖正在运行的自己
//   4. 重新启动 --target 目录下的 SiliconLife.Fast.exe
//
// 该程序必须独立编译部署（见 HotReload.csproj），不能放在 SiliconLife.Fast 的
// bin 输出目录中，否则在"覆盖自己"时会因文件锁失败。
// ------------------------------------------------------------------

using System.Diagnostics;

namespace SiliconLife.Tools.HotReload;

internal static class Program
{
    private const string FastProcessName = "SiliconLife.Fast";
    private const string FastExeName = "SiliconLife.Fast.exe";
    private const int DefaultPort = 8080;
    private const int ShutdownTimeoutMs = 30_000;  // 增加超时时间到30秒
    private const int PostShutdownDelayMs = 2000;  // 增加延迟到2秒，确保端口释放

    // 文件名以这些前缀开头的文件属于更新器自身，复制时必须跳过。
    private static readonly string[] SelfFilePrefixes = new[]
    {
        "HotReload.",      // HotReload.exe / HotReload.dll / HotReload.pdb / HotReload.deps.json / HotReload.runtimeconfig.json
    };

    private static int Main(string[] args)
    {
        Console.WriteLine("=== SiliconLife Hot Reload Updater ===");

        var options = ParseArgs(args);
        if (options == null)
        {
            PrintUsage();
            return 2;
        }

        // target 默认为本 exe 所在目录
        string selfDir = AppContext.BaseDirectory.TrimEnd('\\', '/');
        string target = string.IsNullOrWhiteSpace(options.Target) ? selfDir : options.Target;

        Console.WriteLine($"Source : {options.Source}");
        Console.WriteLine($"Target : {target}");
        Console.WriteLine($"Port   : {options.Port}");
        Console.WriteLine();

        if (!Directory.Exists(options.Source))
        {
            Console.Error.WriteLine($"❌ Source directory not found: {options.Source}");
            return 3;
        }
        if (!Directory.Exists(target))
        {
            Console.Error.WriteLine($"❌ Target directory not found: {target}");
            return 4;
        }

        try
        {
            Run(options.Source, target, options.Port).GetAwaiter().GetResult();
            Console.WriteLine("\n✅ Hot reload completed.");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"\n❌ Hot reload failed: {ex.Message}");
            Console.Error.WriteLine(ex.StackTrace);
            return 1;
        }
    }

    private static async Task Run(string sourcePath, string targetPath, int port)
    {
        // Step 1: graceful shutdown request
        Console.WriteLine("🔌 [1/4] Requesting graceful shutdown...");
        await RequestGracefulShutdown(port);

        // Step 2: wait for the process to exit (fallback to force-kill on timeout)
        Console.WriteLine($"\n⏳ [2/4] Waiting for {FastProcessName} to exit...");
        await WaitForProcessExit(FastExeName, ShutdownTimeoutMs);

        // Small buffer so OS releases file handles
        await Task.Delay(PostShutdownDelayMs);

        // Step 3: copy build output over target (skipping self)
        Console.WriteLine("\n📦 [3/4] Copying files...");
        int copied = CopyFiles(sourcePath, targetPath);
        Console.WriteLine($"   Copied {copied} file(s).");

        // Step 4: restart Fast
        Console.WriteLine("\n🚀 [4/4] Restarting Fast...");
        RestartFast(targetPath);
    }

    // ------------------------------------------------------------------
    // Shutdown
    // ------------------------------------------------------------------

    private static async Task RequestGracefulShutdown(int port)
    {
        using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
        try
        {
            var response = await client.PostAsync(
                $"http://localhost:{port}/api/system/shutdown",
                content: null);

            Console.WriteLine($"   HTTP {(int)response.StatusCode} {response.StatusCode}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("   ⚠️  Shutdown endpoint returned non-success; will rely on process wait/kill.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ⚠️  Shutdown request failed: {ex.Message}");
            Console.WriteLine("   Proceeding to wait/kill directly.");
        }
    }

    private static async Task WaitForProcessExit(string exeName, int timeoutMs)
    {
        var sw = Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds < timeoutMs)
        {
            var procs = FindProcessesByExeName(exeName);
            if (procs.Count == 0)
            {
                Console.WriteLine($"   ✅ {FastProcessName} has exited.");
                return;
            }
            
            foreach (var p in procs) 
            {
                try 
                {
                    Console.WriteLine($"   Process still running: PID {p.Id}, {p.ProcessName}");
                }
                catch { }
            }
            
            foreach (var p in procs) 
            {
                try 
                {
                    p.Dispose();
                }
                catch { }
            }
            
            await Task.Delay(500);  // 增加检查间隔到500ms
        }

        Console.WriteLine($"   ⚠️  Timed out after {timeoutMs / 1000}s; force-killing {FastProcessName}...");
        ForceKill(FastExeName);
        await Task.Delay(1000);  // 强制杀死后等待更长时间
    }

    private static List<Process> FindProcessesByExeName(string exeName)
    {
        var result = new List<Process>();
        try
        {
            var allProcs = Process.GetProcesses();
            string exeNameWithoutExtension = Path.GetFileNameWithoutExtension(exeName);
            foreach (var p in allProcs)
            {
                try
                {
                    // 使用 ProcessName 属性来查找进程，更加可靠
                    if (p.ProcessName.Equals(exeNameWithoutExtension, StringComparison.OrdinalIgnoreCase) ||
                        (!string.IsNullOrEmpty(p.MainModule?.FileName) && 
                         p.MainModule.FileName.EndsWith(exeName, StringComparison.OrdinalIgnoreCase)))
                    {
                        result.Add(p);
                    }
                }
                catch (Exception)
                {
                    // Skip processes we can't access
                }
            }
        }
        catch (Exception)
        {
            // Skip any errors in getting processes
        }
        return result;
    }

    private static void ForceKill(string exeName)
    {
        var procs = FindProcessesByExeName(exeName);
        foreach (var p in procs)
        {
            try
            {
                p.Kill(entireProcessTree: true);
                p.WaitForExit(5000);
                Console.WriteLine($"   ✅ Killed pid {p.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ⚠️  Could not kill pid {p.Id}: {ex.Message}");
            }
            finally
            {
                try
                {
                    p.Dispose();
                }
                catch { }
            }
        }
    }

    // ------------------------------------------------------------------
    // File copy
    // ------------------------------------------------------------------

    private static int CopyFiles(string sourcePath, string targetPath)
    {
        var sourceRoot = new DirectoryInfo(sourcePath);
        string sourceFull = sourceRoot.FullName;
        string targetFull = new DirectoryInfo(targetPath).FullName;

        int count = 0;
        foreach (var file in sourceRoot.EnumerateFiles("*", SearchOption.AllDirectories))
        {
            // Skip self files — HotReload.exe / HotReload.dll etc. must never be overwritten
            // because they are locked while this process is running.
            if (IsSelfFile(file.Name))
            {
                continue;
            }

            string relative = Path.GetRelativePath(sourceFull, file.FullName);
            string dest = Path.Combine(targetFull, relative);
            string? destDir = Path.GetDirectoryName(dest);
            if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            try
            {
                file.CopyTo(dest, overwrite: true);
                count++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ⚠️  Failed to copy '{relative}': {ex.Message}");
            }
        }
        return count;
    }

    private static bool IsSelfFile(string fileName)
    {
        foreach (string prefix in SelfFilePrefixes)
        {
            if (fileName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    // ------------------------------------------------------------------
    // Restart
    // ------------------------------------------------------------------

    private static void RestartFast(string targetPath)
    {
        string exePath = Path.Combine(targetPath, FastExeName);
        if (!File.Exists(exePath))
        {
            throw new FileNotFoundException(
                $"Fast executable not found after copy: {exePath}");
        }

        // 额外等待确保端口完全释放
        Console.WriteLine("   ⏳ Waiting for port to be fully released...");
        Thread.Sleep(1000);

        var psi = new ProcessStartInfo
        {
            FileName = exePath,
            WorkingDirectory = targetPath,
            UseShellExecute = true
        };
        var proc = Process.Start(psi);
        if (proc == null)
        {
            throw new InvalidOperationException("Failed to start Fast process");
        }
        Console.WriteLine($"   ✅ Started: {exePath}  (pid {proc.Id})");
    }

    // ------------------------------------------------------------------
    // Args parsing
    // ------------------------------------------------------------------

    private sealed class Options
    {
        public string Source { get; set; } = "";
        public string Target { get; set; } = "";
        public int Port { get; set; } = DefaultPort;
    }

    private static Options? ParseArgs(string[] args)
    {
        var opts = new Options();
        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];
            if (arg == "--source" && i + 1 < args.Length)
            {
                opts.Source = args[++i];
            }
            else if (arg == "--target" && i + 1 < args.Length)
            {
                opts.Target = args[++i];
            }
            else if (arg == "--port" && i + 1 < args.Length)
            {
                if (int.TryParse(args[++i], out int p) && p > 0 && p <= 65535)
                {
                    opts.Port = p;
                }
                else
                {
                    Console.Error.WriteLine($"Invalid port number: {args[i]}");
                    return null;
                }
            }
            else if (arg == "--build-only")
            {
                // 仅编译模式：不再继续执行
                Console.WriteLine("=== Build-only mode ===");
                Console.WriteLine("Source: " + opts.Source);
                Console.WriteLine("Target: " + opts.Target);
                if (!Directory.Exists(opts.Source))
                {
                    Console.Error.WriteLine($"Source directory not found: {opts.Source}");
                    return null;
                }
                if (!Directory.Exists(opts.Target))
                {
                    Console.Error.WriteLine($"Target directory not found: {opts.Target}");
                    return null;
                }
                
                Console.WriteLine("Copying files...");
                int copied = CopyFiles(opts.Source, opts.Target);
                Console.WriteLine($"Copied {copied} file(s).");
                Console.WriteLine("✅ Build-only mode completed.");
                return null; // 提前返回
            }
            else if (arg == "-?" || arg == "--help")
            {
                return null; // Print usage
            }
        }

        // Validate required source argument
        if (string.IsNullOrWhiteSpace(opts.Source))
        {
            Console.Error.WriteLine("Missing required argument --source");
            return null;
        }

        return opts;
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Usage: HotReload.exe [options]");
        Console.WriteLine("\nOptions:");
        Console.WriteLine("  --source <path>       Source directory of the new build (required)");
        Console.WriteLine("  --target <path>       Target directory to copy to (default: current dir)");
        Console.WriteLine("  --port <number>       Port number of the running Fast instance (default: 8080)");
        Console.WriteLine("  --build-only          Only copy files, do not restart (useful for build scripts)");
        Console.WriteLine("  -?, --help            Show this help message");
        Console.WriteLine("\nExamples:");
        Console.WriteLine("  HotReload.exe --source \"..\\SiliconLife.Fast\\bin\\Debug\"");
        Console.WriteLine("  HotReload.exe --source \"C:\\build\\output\" --target \"C:\\prod\" --port 8081");
        Console.WriteLine("  HotReload.exe --source \"C:\\build\" --build-only");
    }
}