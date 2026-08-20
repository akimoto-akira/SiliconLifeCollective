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
using System.Text.Json;
using SiliconLife.Collective;

namespace ForgeMind;

/// <summary>
/// Unreal Engine project launch tool ("启动项目").
/// Opens the project in the Unreal Editor after verifying that it is not
/// already open.
/// <para>Pipeline: resolve the .uproject (accepts the file itself or its
/// folder, error when missing) → detect an editor process whose command line
/// already references this .uproject (the opener may be any parent — Epic
/// Launcher, a desktop shortcut, etc. — so detection scans all editor
/// processes via a WMI query) → resolve the engine from the EngineAssociation
/// → launch <c>UnrealEditor.exe "&lt;uproject&gt;"</c> detached.</para>
/// </summary>
public class UnrealLaunchProjectTool : ITool
{
    /// <summary>
    /// Editor process image names whose command lines are inspected
    /// (5.x uses UnrealEditor, older branches UE4Editor / UE5Editor).
    /// </summary>
    private static readonly string[] EditorProcessNames = ["UnrealEditor.exe", "UE4Editor.exe", "UE5Editor.exe"];

    /// <summary>Wait limit for the WMI detection query.</summary>
    private static readonly TimeSpan DetectionTimeout = TimeSpan.FromSeconds(20);

    public string Name => "unreal_launch";

    public string Description =>
        "Launch an Unreal Engine project in the Unreal Editor. " +
        "Requires the 'path' parameter - either the .uproject file or the folder containing it. " +
        "The tool first checks whether the project is already open in an editor " +
        "(regardless of who launched it) and refuses to open it twice; " +
        "the engine is resolved from the project's EngineAssociation.";

    public string GetDisplayName(Language language) => language switch
    {
        Language.ZhCN => "启动项目",
        Language.ZhHK => "啟動項目",
        Language.JaJP => "プロジェクト起動",
        Language.KoKR => "프로젝트 실행",
        _ => "Launch Project"
    };

    public Dictionary<string, object> GetParameterSchema()
    {
        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["path"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Path to the .uproject file, or the folder containing it"
                }
            },
            ["required"] = new[] { "path" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!OperatingSystem.IsWindows())
            return ToolResult.Failed("Launching UE projects is only supported on Windows");

        if (!parameters.TryGetValue("path", out object? pathObj) ||
            string.IsNullOrWhiteSpace(pathObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'path' parameter");
        }

        string path = pathObj.ToString()!;

        // 1. Resolve the .uproject — accepts the file itself or its folder
        FileInfo? projectFile = UnrealProjectTool.ResolveProjectFile(path);
        if (projectFile == null)
        {
            return ToolResult.Failed(
                $"No valid .uproject found at '{path}' - expected a .uproject file or the folder containing it");
        }

        // 2. Refuse to launch when any editor already has this project open
        if (FindEditorProcess(projectFile.FullName, out int existingPid))
        {
            return ToolResult.Failed(
                $"'{projectFile.Name}' is already open in an Unreal Editor process (PID {existingPid})");
        }

        // 3. Resolve the engine installation from the EngineAssociation
        string engineAssociation = UnrealProjectTool.GetEngineAssociation(projectFile.Directory!) ?? "";
        UnrealEngineInstallInfo? engine = UnrealEngineTool.FindUnrealEngine(engineAssociation);
        if (engine == null)
        {
            return ToolResult.Failed(
                $"Could not resolve the engine for association '{engineAssociation}' - cannot launch the project");
        }

        string editorPath = Path.Combine(engine.Value.InstallLocation, "Engine", "Binaries", "Win64", "UnrealEditor.exe");
        if (!File.Exists(editorPath))
        {
            return ToolResult.Failed($"UnrealEditor.exe not found: {editorPath}");
        }

        // 4. Launch detached — the editor keeps running after this tool returns
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = editorPath,
                Arguments = $"\"{projectFile.FullName}\"",
                UseShellExecute = false
            };

            using var process = Process.Start(startInfo);
            if (process == null)
                return ToolResult.Failed("Failed to start UnrealEditor.exe");

            return ToolResult.Successful(
                $"Launched '{projectFile.Name}' with engine '{engineAssociation}' (editor PID {process.Id})",
                new
                {
                    projectFile = projectFile.FullName,
                    engineAssociation,
                    editorPath,
                    editorPid = process.Id
                });
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Launch failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Detects a running Unreal Editor whose command line references the given
    /// .uproject. Uses a PowerShell + WMI query because .NET cannot read the
    /// command line of processes it did not start — the opener can be any
    /// parent (Epic Launcher, Explorer, another tool). Shared with
    /// UnrealBuildTool, which must not compile an editor target while the
    /// editor holds its binaries locked.
    /// </summary>
    internal static bool FindEditorProcess(string projectFilePath, out int processId)
    {
        processId = 0;

        string names = string.Join("','", EditorProcessNames);
        string script =
            $"Get-CimInstance Win32_Process -Filter \"Name='{names}'\" " +
            "| Select-Object ProcessId, CommandLine | ConvertTo-Json -Compress";

        var startInfo = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = $"-NoProfile -NonInteractive -Command \"{script.Replace("\"", "\\\"")}\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        try
        {
            using var process = Process.Start(startInfo);
            if (process == null)
                return false;

            string output = process.StandardOutput.ReadToEnd();
            process.StandardError.ReadToEnd();

            if (!process.WaitForExit(DetectionTimeout))
                return false;

            if (process.ExitCode != 0)
                return false;

            if (string.IsNullOrWhiteSpace(output))
                return false;

            // ConvertTo-Json returns a single object for one match, an array otherwise
            using var document = JsonDocument.Parse(output);
            IEnumerable<JsonElement> rows =
                document.RootElement.ValueKind == JsonValueKind.Array
                    ? document.RootElement.EnumerateArray()
                    : [document.RootElement];

            foreach (JsonElement row in rows)
            {
                string? commandLine = row.TryGetProperty("CommandLine", out JsonElement cmdProp)
                    ? cmdProp.GetString()
                    : null;

                if (string.IsNullOrEmpty(commandLine))
                    continue;

                // Case-insensitive full-path match against the editor command line
                if (commandLine.Contains(projectFilePath, StringComparison.OrdinalIgnoreCase))
                {
                    if (row.TryGetProperty("ProcessId", out JsonElement pidProp) &&
                        pidProp.TryGetInt32(out int pid))
                    {
                        processId = pid;
                    }

                    return true;
                }
            }

            return false;
        }
        catch
        {
            // Detection unavailable — never block the launch on a failed check
            return false;
        }
    }
}
