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

using System.Diagnostics;
using System.Text;
using SiliconLife.Collective;

using SiliconLife.Common.Localization;

namespace SiliconLife.Common.Tools;

/// <summary>
/// Hot reload tool for the Silicon Curator.
/// Compiles the SiliconLife.Fast project via <c>dotnet build</c>, and on success
/// launches an external HotReload.exe program that gracefully stops the currently
/// running Fast instance, overwrites deployment files from the build output, and
/// restarts Fast — giving the system a basic "self-development" capability.
/// </summary>
[ToolAction("execute", "build_only")]
[SiliconManagerOnly]
[ToolScenario(ToolScenarioFlag.Chat)]
public class HotReloadTool : ITool
{
    // Default paths — may be overridden via tool parameters at invocation time.
    private const string DefaultProjectPath =
        @"d:\SiliconLifeCollective\src\SiliconLife.Fast\SiliconLife.Fast.csproj";
    private const string DefaultSourcePath =
        @"d:\SiliconLifeCollective\src\SiliconLife.Fast\bin\Debug\net9.0-windows";
    private const string DefaultHotReloadExe =
        @"d:\SiliconLifeCollective\tools\HotReload\bin\Release\net9.0\HotReload.exe";
    private const int DefaultPort = 8080;
    private const int BuildTimeoutMs = 180_000; // 3 minutes

    public string Name => "hot_reload";

    public string Description =>
        "Compile SiliconLife.Fast and launch the external HotReload.exe to gracefully " +
        "restart the running Fast instance with the new build. " +
        "Actions: 'execute' (run the full build + hot-reload flow), " +
        "'build_only' (only compile, do not trigger restart).";

    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

    public Dictionary<string, object> GetParameterSchema()
    {
        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The hot-reload action: 'execute' runs the full flow; 'build_only' compiles without restarting.",
                    ["enum"] = new[] { "execute", "build_only" }
                },
                ["project_path"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Optional. Absolute path to the .csproj to compile. Defaults to SiliconLife.Fast.csproj."
                },
                ["source_path"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Optional. Absolute path to the build output directory (source for file copy). Defaults to Fast bin/Debug."
                },
                ["hot_reload_exe"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Optional. Absolute path to HotReload.exe. Defaults to tools/HotReload/bin/Release/net9.0/HotReload.exe."
                },
                ["port"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Optional. Port of the running Fast web server. Defaults to 8080."
                },
                ["configuration"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Optional. Build configuration (Debug or Release). Defaults to Debug.",
                    ["enum"] = new[] { "Debug", "Release" }
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj))
            return ToolResult.Failed("Missing 'action' parameter");

        string action = actionObj?.ToString()?.ToLowerInvariant() ?? "";

        string projectPath = GetStringParam(parameters, "project_path", DefaultProjectPath);
        string sourcePath = GetStringParam(parameters, "source_path", DefaultSourcePath);
        string hotReloadExe = GetStringParam(parameters, "hot_reload_exe", DefaultHotReloadExe);
        string configuration = GetStringParam(parameters, "configuration", "Debug");
        int port = GetIntParam(parameters, "port", DefaultPort);

        return action switch
        {
            "execute" => ExecuteFullFlow(projectPath, sourcePath, hotReloadExe, configuration, port),
            "build_only" => ExecuteBuildOnly(projectPath, configuration),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    private ToolResult ExecuteFullFlow(
        string projectPath,
        string sourcePath,
        string hotReloadExe,
        string configuration,
        int port)
    {
        // Step 1: compile the project
        var build = BuildProject(projectPath, configuration);
        if (!build.Success)
        {
            return ToolResult.Failed(build.Output);
        }

        // Step 2: sanity-check paths
        if (!Directory.Exists(sourcePath))
        {
            return ToolResult.Failed(
                $"Build succeeded but source path does not exist: {sourcePath}");
        }
        if (!File.Exists(hotReloadExe))
        {
            return ToolResult.Failed(
                $"HotReload.exe not found: {hotReloadExe}. " +
                "Please build the tools/HotReload project first.");
        }

        // Step 3: launch HotReload.exe (detached)
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = hotReloadExe,
                Arguments = $"--source \"{sourcePath}\" --port {port}",
                WorkingDirectory = Path.GetDirectoryName(hotReloadExe)!,
                UseShellExecute = true, // detach from parent so it survives our shutdown
                CreateNoWindow = false
            };

            var proc = Process.Start(psi);
            if (proc == null)
            {
                return ToolResult.Failed("Failed to start HotReload.exe");
            }

            var sb = new StringBuilder();
            sb.AppendLine("✅ Build succeeded. HotReload.exe launched.");
            sb.AppendLine($"  Source : {sourcePath}");
            sb.AppendLine($"  Updater: {hotReloadExe}");
            sb.AppendLine($"  Port   : {port}");
            sb.AppendLine();
            sb.AppendLine("The external updater will now:");
            sb.AppendLine("  1. POST /api/system/shutdown to stop this instance");
            sb.AppendLine("  2. Wait for the process to exit");
            sb.AppendLine("  3. Copy build output into its own directory");
            sb.AppendLine("  4. Start the new Fast instance");
            sb.AppendLine();
            sb.AppendLine("This process will be terminated shortly. New code takes effect after restart.");
            return ToolResult.Successful(sb.ToString());
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to launch HotReload.exe: {ex.Message}");
        }
    }

    private ToolResult ExecuteBuildOnly(string projectPath, string configuration)
    {
        var build = BuildProject(projectPath, configuration);
        return build.Success
            ? ToolResult.Successful(build.Output)
            : ToolResult.Failed(build.Output);
    }

    private static (bool Success, string Output) BuildProject(string projectPath, string configuration)
    {
        if (!File.Exists(projectPath))
        {
            return (false, $"Project file not found: {projectPath}");
        }

        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"build \"{projectPath}\" -c {configuration} --nologo -v m",
                WorkingDirectory = Path.GetDirectoryName(projectPath)!,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            using var proc = Process.Start(psi);
            if (proc == null)
            {
                return (false, "Failed to start dotnet build");
            }

            var stdout = new StringBuilder();
            var stderr = new StringBuilder();
            proc.OutputDataReceived += (_, e) => { if (e.Data != null) stdout.AppendLine(e.Data); };
            proc.ErrorDataReceived += (_, e) => { if (e.Data != null) stderr.AppendLine(e.Data); };
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            if (!proc.WaitForExit(BuildTimeoutMs))
            {
                try { proc.Kill(entireProcessTree: true); } catch { }
                return (false, $"Build timed out after {BuildTimeoutMs / 1000}s");
            }

            // Ensure async reads flush
            proc.WaitForExit();

            string combined = stdout.ToString() + stderr.ToString();
            if (proc.ExitCode == 0)
            {
                return (true, $"✅ Build succeeded ({configuration}).\n\n" + Tail(combined, 60));
            }
            return (false, $"❌ Build failed (exit {proc.ExitCode}).\n\n" + Tail(combined, 80));
        }
        catch (Exception ex)
        {
            return (false, $"Build invocation failed: {ex.Message}");
        }
    }

    private static string GetStringParam(Dictionary<string, object> parameters, string key, string defaultValue)
    {
        if (parameters.TryGetValue(key, out var v) && v != null)
        {
            string s = v.ToString() ?? "";
            if (!string.IsNullOrWhiteSpace(s)) return s;
        }
        return defaultValue;
    }

    private static int GetIntParam(Dictionary<string, object> parameters, string key, int defaultValue)
    {
        if (parameters.TryGetValue(key, out var v) && v != null)
        {
            if (v is int i) return i;
            if (v is long l) return (int)l;
            if (int.TryParse(v.ToString(), out int parsed)) return parsed;
        }
        return defaultValue;
    }

    /// <summary>Returns the last <paramref name="maxLines"/> lines of <paramref name="text"/>.</summary>
    private static string Tail(string text, int maxLines)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        var lines = text.Split('\n');
        if (lines.Length <= maxLines) return text;
        return string.Join('\n', lines[(lines.Length - maxLines)..]);
    }
}
