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
using SiliconLife.Collective;

namespace ForgeMind;

/// <summary>
/// Unreal Engine Visual Studio solution regeneration tool ("重新生成sln").
/// Regenerates the .sln and project files of a UE project via the Epic
/// Launcher's UnrealVersionSelector.
/// <para>Pipeline: resolve the .uproject (accepts the file itself or its
/// folder, error when missing) → verify UnrealVersionSelector.exe exists
/// (error when missing) → run
/// <c>UnrealVersionSelector.exe /projectfiles "&lt;uproject&gt;"</c> and wait
/// for completion. The regeneration typically takes tens of seconds to
/// several minutes.</para>
/// </summary>
public class UnrealSlnTool : ITool
{
    /// <summary>
    /// Fixed location of the Epic Launcher's UnrealVersionSelector, which
    /// performs the project file regeneration for the associated engine.
    /// </summary>
    private const string UnrealVersionSelectorPath =
        @"C:\Program Files (x86)\Epic Games\Launcher\Engine\Binaries\Win64\UnrealVersionSelector.exe";

    /// <summary>
    /// Generous wait limit for the regeneration process, which normally runs
    /// tens of seconds to several minutes.
    /// </summary>
    private static readonly TimeSpan RegenerationTimeout = TimeSpan.FromMinutes(10);

    public string Name => "unreal_sln";

    public string Description =>
        "Regenerate the Visual Studio solution (.sln) and project files of an Unreal Engine project. " +
        "Requires the 'path' parameter — either the .uproject file or the folder containing it. " +
        "Note: the regeneration runs tens of seconds to several minutes.";

    public string GetDisplayName(Language language) => language switch
    {
        Language.ZhCN => "重新生成sln",
        Language.ZhHK => "重新生成sln",
        Language.JaJP => "sln 再生成",
        Language.KoKR => "sln 재생성",
        _ => "Regenerate VS Solution"
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

        // 2. The Epic Launcher's UnrealVersionSelector performs the regeneration
        if (!File.Exists(UnrealVersionSelectorPath))
        {
            return ToolResult.Failed($"UnrealVersionSelector.exe not found: {UnrealVersionSelectorPath}");
        }

        // 3. Run the regeneration — takes tens of seconds to several minutes
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = UnrealVersionSelectorPath,
                Arguments = $"/projectfiles \"{projectFile.FullName}\"",
                UseShellExecute = false
            };

            using var process = Process.Start(startInfo);
            if (process == null)
                return ToolResult.Failed("Failed to start UnrealVersionSelector.exe");

            if (!process.WaitForExit(RegenerationTimeout))
                return ToolResult.Failed(
                    $"UnrealVersionSelector.exe did not finish within {RegenerationTimeout.TotalMinutes:0} minutes");

            if (process.ExitCode != 0)
                return ToolResult.Failed(
                    $"UnrealVersionSelector.exe exited with code {process.ExitCode} for '{projectFile.Name}'");

            return ToolResult.Successful(
                $"Regenerated Visual Studio project files for '{projectFile.Name}'",
                new
                {
                    projectFile = projectFile.FullName,
                    command = $"\"{UnrealVersionSelectorPath}\" /projectfiles \"{projectFile.FullName}\"",
                    exitCode = process.ExitCode
                });
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Regeneration failed: {ex.Message}");
        }
    }
}
