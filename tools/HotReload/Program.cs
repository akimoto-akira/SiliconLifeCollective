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
// bin 输出目录中，否则在"覆盖自身"时会因文件锁失败。
// ------------------------------------------------------------------

using System.Diagnostics;

namespace SiliconLife.Tools.HotReload;

internal static class Program
{
    private const string FastProcessName = "SiliconLife.Fast";
    private const string FastExeName = "SiliconLife.Fast.exe";
    private const int DefaultPort = 8080;
    private const int ShutdownTimeoutMs = 15_000;
    private const int PostShutdownDelayMs = 500;

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

        // target 默认为 exe 自身所在目录
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
        Console.WriteLine("🛑 [1/4] Requesting graceful shutdown...");
        await RequestGracefulShutdown(port);

        // Step 2: wait for the process to exit (fallback to force-kill on timeout)
        Console.WriteLine($"\n⏳ [2/4] Waiting for {FastProcessName} to exit...");
        await WaitForProcessExit(FastProcessName, ShutdownTimeoutMs);

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

    private static async Task WaitForProcessExit(string processName, int timeoutMs)
    {
        var sw = Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds < timeoutMs)
        {
            var procs = Process.GetProcessesByName(processName);
            if (procs.Length == 0)
            {
                Console.WriteLine($"   ✅ {processName} has exited.");
                return;
            }
            foreach (var p in procs) p.Dispose();
            await Task.Delay(300);
        }

        Console.WriteLine($"   ⚠️  Timed out after {timeoutMs / 1000}s; force-killing {processName}...");
        ForceKill(processName);
        await Task.Delay(500);
    }

    private static void ForceKill(string processName)
    {
        foreach (var p in Process.GetProcessesByName(processName))
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
                p.Dispose();
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
            string a = args[i];
            switch (a)
            {
                case "--source":
                case "-s":
                    if (++i >= args.Length) return null;
                    opts.Source = args[i];
                    break;
                case "--target":
                case "-t":
                    if (++i >= args.Length) return null;
                    opts.Target = args[i];
                    break;
                case "--port":
                case "-p":
                    if (++i >= args.Length) return null;
                    if (!int.TryParse(args[i], out int port)) return null;
                    opts.Port = port;
                    break;
                case "-h":
                case "--help":
                case "/?":
                    return null;
                default:
                    Console.Error.WriteLine($"Unknown argument: {a}");
                    return null;
            }
        }

        if (string.IsNullOrWhiteSpace(opts.Source))
        {
            Console.Error.WriteLine("Missing required argument: --source <path>");
            return null;
        }
        return opts;
    }

    private static void PrintUsage()
    {
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  HotReload.exe --source <build-output-dir> [--target <deploy-dir>] [--port <port>]");
        Console.WriteLine();
        Console.WriteLine("Arguments:");
        Console.WriteLine("  --source, -s   Required. Directory containing freshly built files to copy.");
        Console.WriteLine("  --target, -t   Optional. Destination directory. Defaults to the folder of HotReload.exe itself.");
        Console.WriteLine("  --port,   -p   Optional. Fast web server port. Default: 8080.");
    }
}
