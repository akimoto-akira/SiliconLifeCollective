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
/// Unreal Engine version switch tool ("切换引擎版本").
/// Launches the official UnrealVersionSelector with <c>/switchversion</c> so
/// the user can move the project to another engine version through the
/// official dialog, then reports how the project's EngineAssociation changed.
/// <para>Pipeline: resolve the .uproject (accepts the file itself or its
/// folder, error when missing) → verify UnrealVersionSelector.exe exists
/// (error when missing) → read the EngineAssociation before the switch →
/// run <c>UnrealVersionSelector.exe /switchversion "&lt;uproject&gt;"</c> and
/// wait for the dialog to close → read the EngineAssociation again and
/// report the change.</para>
/// </summary>
public class UnrealSwitchVersionTool : ITool
{
    /// <summary>
    /// Fixed location of the Epic Launcher's UnrealVersionSelector, which
    /// performs the engine version switch for the project.
    /// </summary>
    private const string UnrealVersionSelectorPath =
        @"C:\Program Files (x86)\Epic Games\Launcher\Engine\Binaries\Win64\UnrealVersionSelector.exe";

    /// <summary>
    /// Wait limit for the switch process. The official dialog is interactive
    /// and the following conversion may run several minutes.
    /// </summary>
    private static readonly TimeSpan SwitchVersionTimeout = TimeSpan.FromMinutes(10);

    public string Name => "unreal_switchversion";

    public string Description =>
        "Launch the official Unreal version switch dialog to change the engine version of an Unreal Engine project. " +
        "Requires the 'path' parameter — either the .uproject file or the folder containing it. " +
        "The tool monitors the project's EngineAssociation before and after the switch and reports the change.";

    public string GetDisplayName(Language language) => language switch
    {
        Language.ZhCN => "切换引擎版本",
        Language.ZhHK => "切換引擎版本",
        Language.JaJP => "エンジンバージョン切替",
        Language.KoKR => "엔진 버전 전환",
        _ => "Switch Engine Version"
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

        // 2. The Epic Launcher's UnrealVersionSelector performs the switch
        if (!File.Exists(UnrealVersionSelectorPath))
        {
            return ToolResult.Failed($"UnrealVersionSelector.exe not found: {UnrealVersionSelectorPath}");
        }

        // 3. EngineAssociation before the switch — the comparison baseline
        string associationBefore = UnrealProjectTool.GetEngineAssociation(projectFile.Directory!) ?? "";

        // 4. Run the switch dialog — interactive, then conversion may follow
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = UnrealVersionSelectorPath,
                Arguments = $"/switchversion \"{projectFile.FullName}\"",
                UseShellExecute = false
            };

            using var process = Process.Start(startInfo);
            if (process == null)
                return ToolResult.Failed("Failed to start UnrealVersionSelector.exe");

            if (!process.WaitForExit(SwitchVersionTimeout))
                return ToolResult.Failed(
                    $"UnrealVersionSelector.exe did not finish within {SwitchVersionTimeout.TotalMinutes:0} minutes");

            if (process.ExitCode != 0)
                return ToolResult.Failed(
                    $"UnrealVersionSelector.exe exited with code {process.ExitCode} for '{projectFile.Name}'");
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Version switch failed: {ex.Message}");
        }

        // 5. EngineAssociation after the switch — report the change
        string associationAfter = UnrealProjectTool.GetEngineAssociation(projectFile.Directory!) ?? "";
        bool changed = !string.Equals(associationBefore, associationAfter, StringComparison.Ordinal);

        return ToolResult.Successful(
            changed
                ? $"Switched '{projectFile.Name}' from '{associationBefore}' to '{associationAfter}'"
                : $"No version change detected for '{projectFile.Name}' (dialog cancelled or same version selected)",
            new
            {
                projectFile = projectFile.FullName,
                engineAssociationBefore = associationBefore,
                engineAssociationAfter = associationAfter,
                changed
            });
    }
}
