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

using System.Text;
using System.Text.Json;
using Microsoft.Win32;
using SiliconLife.Collective;

namespace ForgeMind;

/// <summary>
/// Unreal Engine installation detection and configuration tool.
/// Ported from UnrealEngineCacheCtrl.Services.UnrealEngineService:
/// enumerates engines registered by the Epic Launcher (LauncherInstalled.dat)
/// and source builds registered in the Windows registry, and resolves a
/// project's EngineAssociation to a concrete engine installation.
/// </summary>
public class UnrealEngineTool : ITool
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>Epic Launcher installation record file.</summary>
    private const string LauncherInstallDatPath =
        @"C:\ProgramData\Epic\UnrealEngineLauncher\LauncherInstalled.dat";

    /// <summary>Registry path where source builds register their GUID → install path mapping.</summary>
    private const string BuildsRegistrySubKey =
        @"SOFTWARE\Epic Games\Unreal Engine\Builds";

    public string Name => "unreal_engine";

    public string Description =>
        "Detect Unreal Engine installations and resolve project-engine associations. " +
        "Actions: 'list' (all installed engines: Epic Launcher installs and registry source builds, " +
        "with install location, version and identifiers), " +
        "'is_installed' (whether any Unreal Engine is present on this machine), " +
        "'match' (resolve a project's EngineAssociation — version like '5.4' or a source-build GUID — " +
        "to the matching installed engine).";

    public string GetDisplayName(Language language) => language switch
    {
        Language.ZhCN => "UE 引擎检测",
        Language.ZhHK => "UE 引擎檢測",
        Language.JaJP => "UE エンジン検出",
        Language.KoKR => "UE 엔진 감지",
        _ => "UE Engine Detection"
    };

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
                    ["description"] = "The action to perform: list, is_installed, match",
                    ["enum"] = new[] { "list", "is_installed", "match" }
                },
                ["association"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Engine association from a .uproject file (for 'match' action): " +
                                     "a launcher version like '5.4' or a source-build GUID"
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

        return action switch
        {
            "list" => ExecuteList(),
            "is_installed" => ExecuteIsInstalled(),
            "match" => ExecuteMatch(parameters),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    // ===== action: list =====

    private static ToolResult ExecuteList()
    {
        UnrealEngineInstallInfo[] engines = GetAllUnrealApplications();

        if (engines.Length == 0)
        {
            return ToolResult.Successful("No Unreal Engine installation found on this machine",
                new { engines = Array.Empty<object>() });
        }

        return ToolResult.Successful($"Found {engines.Length} Unreal Engine installation(s)",
            new
            {
                engines = engines.Select(e => new
                {
                    appName = e.AppName,
                    appVersion = e.AppVersion,
                    installLocation = e.InstallLocation,
                    namespaceId = e.NamespaceId,
                    itemId = e.ItemId
                }).ToArray()
            });
    }

    // ===== action: is_installed =====

    private static ToolResult ExecuteIsInstalled()
    {
        bool installed = IsUnrealEngineInstalled();
        return ToolResult.Successful(
            installed
                ? "Unreal Engine is installed on this machine"
                : "Unreal Engine is NOT installed on this machine",
            new { isInstalled = installed });
    }

    // ===== action: match =====

    private static ToolResult ExecuteMatch(Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("association", out object? assocObj) ||
            string.IsNullOrWhiteSpace(assocObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'association' parameter (e.g. '5.4' or a source-build GUID)");
        }

        string association = assocObj.ToString()!;
        UnrealEngineInstallInfo? engine = FindUnrealEngine(association);

        if (engine == null)
        {
            return ToolResult.Failed(
                $"No installed Unreal Engine matches association '{association}'");
        }

        UnrealEngineInstallInfo found = engine.Value;
        return ToolResult.Successful(
            $"Association '{association}' resolved to engine '{found.AppName}' at '{found.InstallLocation}'",
            new
            {
                appName = found.AppName,
                appVersion = found.AppVersion,
                installLocation = found.InstallLocation,
                namespaceId = found.NamespaceId,
                itemId = found.ItemId
            });
    }

    // ===== Ported UnrealEngineService helpers =====

    /// <summary>
    /// Collects all Unreal Engine installations:
    /// ① Epic Launcher record file (LauncherInstalled.dat);
    /// ② source builds registered under HKCU\SOFTWARE\Epic Games\Unreal Engine\Builds
    ///    (Windows-only, requires Capability.Registry).
    /// </summary>
    private static UnrealEngineInstallInfo[] GetAllUnrealApplications()
    {
        var result = new List<UnrealEngineInstallInfo>();

        // ① Epic Launcher installed engines
        FileInfo installInfoFile = new(LauncherInstallDatPath);
        if (installInfoFile.Exists)
        {
            try
            {
                string? json = UnrealTextReader.LoadStringAuto(installInfoFile);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    LauncherInstallList? list = JsonSerializer.Deserialize<LauncherInstallList>(json, JsonOptions);
                    if (list?.InstallationList != null)
                        result.AddRange(list.InstallationList);
                }
            }
            catch
            {
                // Ignore malformed LauncherInstalled.dat
            }
        }

        // ② Source builds from the registry (Windows only)
        if (OperatingSystem.IsWindows())
        {
            try
            {
                using RegistryKey? unrealBuildKey = Registry.CurrentUser.OpenSubKey(BuildsRegistrySubKey);
                if (unrealBuildKey != null)
                {
                    foreach (string buildName in unrealBuildKey.GetValueNames())
                    {
                        try
                        {
                            if (!Guid.TryParse(buildName, out Guid guid))
                                continue;

                            if (unrealBuildKey.GetValue(buildName) is not string buildPath ||
                                string.IsNullOrWhiteSpace(buildPath))
                            {
                                continue;
                            }

                            DirectoryInfo buildLocation = new(buildPath);

                            // Load the per-build version descriptor
                            FileInfo buildVersionFile =
                                new(Path.Combine(buildLocation.FullName, "Engine", "Build", "Build.version"));
                            string? buildVersionString = UnrealTextReader.LoadString(buildVersionFile, Encoding.UTF8);
                            if (buildVersionString == null)
                                continue;

                            UnrealBuildInfo? buildInfo =
                                JsonSerializer.Deserialize<UnrealBuildInfo>(buildVersionString, JsonOptions);
                            if (buildInfo == null)
                                continue;

                            result.Add(new UnrealEngineInstallInfo
                            {
                                AppName = "Binary build at " + buildLocation.FullName,
                                InstallLocation = buildLocation.FullName,
                                NamespaceId = "regUe",
                                ArtifactId = "Build_" + buildLocation.FullName,
                                ItemId = guid.ToString("N"),
                                AppVersion = buildInfo.Value.ToString()
                            });
                        }
                        catch
                        {
                            // Skip malformed registry entries
                        }
                    }
                }
            }
            catch
            {
                // Registry unavailable — launcher results still apply
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Whether any Unreal Engine (launcher or source build) is installed.
    /// </summary>
    private static bool IsUnrealEngineInstalled()
    {
        foreach (UnrealEngineInstallInfo info in GetAllUnrealApplications())
        {
            string ns = info.NamespaceId?.ToLowerInvariant() ?? "";
            if (ns is "ue" or "regue")
            {
                if ((info.AppName ?? "").ToLowerInvariant().StartsWith("ue_"))
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Resolves a .uproject EngineAssociation to an installed engine:
    /// first by GUID (source builds), then by "UE_&lt;version&gt;" app name (launcher builds).
    /// Returns <see langword="null"/> when nothing matches.
    /// </summary>
    internal static UnrealEngineInstallInfo? FindUnrealEngine(string association)
    {
        UnrealEngineInstallInfo[] allApplications = GetAllUnrealApplications();

        // GUID association → source build registered in the registry
        if (Guid.TryParse(association, out Guid guid))
        {
            string guidString = guid.ToString("N");

            foreach (UnrealEngineInstallInfo app in allApplications)
            {
                if (app.ItemId == guidString)
                {
                    string ns = app.NamespaceId?.ToLowerInvariant() ?? "";
                    if (ns == "regue")
                        return app;

                    if (ns == "ue" && (app.AppName ?? "").ToLowerInvariant().StartsWith("ue_"))
                        return app;
                }
            }
        }

        // Version association → launcher build named "UE_<version>"
        foreach (UnrealEngineInstallInfo app in allApplications)
        {
            if (app.AppName == "UE_" + association)
                return app;
        }

        return null;
    }
}
