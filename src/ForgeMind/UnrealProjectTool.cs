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

using System.Text.Json;
using SiliconLife.Collective;

namespace ForgeMind;

/// <summary>
/// Unreal Engine project detection and validation tool.
/// Ported from UnrealEngineCacheCtrl.Services.ProjectService:
/// locates <c>.uproject</c> files, parses project descriptions, analyzes
/// project structure (C++ / Blueprint) and scores integrity.
/// </summary>
public class UnrealProjectTool : ITool
{
    /// <summary>
    /// Name of the ForgeMind companion plugin on the UE side. Its presence and
    /// enablement gate engine-side features exposed by this plugin.
    /// </summary>
    private const string CompanionPluginName = "ForgeMindForUE";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public string Name => "unreal_project";

    public string Description =>
        "Detect and validate Unreal Engine projects. " +
        "Actions: 'check' (whether a directory is a valid UE project, locates the .uproject file), " +
        "'analyze' (analyze project structure: project type, modules, plugins, companion plugin ForgeMindForUE, C++/Blueprint detection), " +
        "'validate' (validate project integrity and produce a 0-100 score with missing items). " +
        "Requires the 'path' parameter pointing to the project directory.";

    public string GetDisplayName(Language language) => language switch
    {
        Language.ZhCN => "UE 项目检测",
        Language.ZhHK => "UE 專案檢測",
        Language.JaJP => "UE プロジェクト検出",
        Language.KoKR => "UE 프로젝트 감지",
        _ => "UE Project Detection"
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
                    ["description"] = "The action to perform: check, analyze, validate",
                    ["enum"] = new[] { "check", "analyze", "validate" }
                },
                ["path"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Absolute path to the Unreal Engine project directory"
                },
                ["allPlugins"] = new Dictionary<string, object>
                {
                    ["type"] = "boolean",
                    ["description"] = "Analyze only: false lists only enabled plugins; true lists every project and engine plugin with its enabled state and storage location"
                }
            },
            ["required"] = new[] { "action", "path" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj))
            return ToolResult.Failed("Missing 'action' parameter");

        if (!parameters.TryGetValue("path", out object? pathObj) ||
            string.IsNullOrWhiteSpace(pathObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'path' parameter");
        }

        string action = actionObj?.ToString()?.ToLowerInvariant() ?? "";
        string path = pathObj.ToString()!;

        bool allPlugins = parameters.TryGetValue("allPlugins", out object? allPluginsObj) &&
                          bool.TryParse(allPluginsObj?.ToString(), out bool parsedAllPlugins) && parsedAllPlugins;

        DirectoryInfo directory;
        try
        {
            directory = new DirectoryInfo(path);
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Invalid path '{path}': {ex.Message}");
        }

        if (!directory.Exists)
            return ToolResult.Failed($"Directory does not exist: {directory.FullName}");

        return action switch
        {
            "check" => ExecuteCheck(directory),
            "analyze" => ExecuteAnalyze(directory, allPlugins),
            "validate" => ExecuteValidate(directory),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    // ===== action: check =====

    private static ToolResult ExecuteCheck(DirectoryInfo directory)
    {
        bool isValid = CheckProjectInDirectory(directory, out FileInfo? projectFile);

        if (!isValid)
        {
            return ToolResult.Failed(
                $"No valid Unreal Engine project found in '{directory.FullName}' (no parseable .uproject file)");
        }

        return ToolResult.Successful(
            $"'{directory.FullName}' is a valid Unreal Engine project (project file: {projectFile!.Name})",
            new
            {
                isValid = true,
                projectFile = projectFile.FullName,
                directory = directory.FullName
            });
    }

    // ===== action: analyze =====

    private static ToolResult ExecuteAnalyze(DirectoryInfo directory, bool allPlugins)
    {
        if (!CheckProjectInDirectory(directory, out FileInfo? projectFile))
        {
            return ToolResult.Failed(
                $"No valid Unreal Engine project found in '{directory.FullName}'");
        }

        UnrealProjectDescription desc = default;
        string[] modules = [];
        string engineAssociation = "";

        try
        {
            desc = ParseProjectDescription(projectFile!);
            engineAssociation = desc.EngineAssociation ?? "";
            modules = GetModuleNames(desc);
        }
        catch
        {
            // Structure detection continues even if description parsing fails
        }

        UnrealProjectType projectType = IdentifyProjectType(directory, desc);

        string[] specialDirs = ScanSpecialDirectories(directory);

        var warnings = new List<string>();
        List<DetectedPlugin> plugins = DetectPlugins(directory, desc, allPlugins, warnings);

        object companionPlugin = DetectCompanionPlugin(directory, desc);

        return ToolResult.Successful(
            $"Analyzed UE project '{projectFile!.Name}' - type: {projectType}",
            new
            {
                projectFile = projectFile.FullName,
                engineAssociation,
                projectType = projectType.ToString(),
                modules,
                plugins,
                companionPlugin,
                specialDirectories = specialDirs,
                hasCppCode = DetectCppCode(directory),
                hasBlueprints = DetectBlueprints(directory),
                warnings
            });
    }

    // ===== action: validate =====

    private static ToolResult ExecuteValidate(DirectoryInfo directory)
    {
        var errors = new List<string>();
        var warnings = new List<string>();
        var missingFiles = new List<string>();
        var missingDirs = new List<string>();
        var corruptedFiles = new List<string>();

        int totalChecks = 0;
        int passedChecks = 0;

        // Project file presence and validity
        totalChecks++;
        if (CheckProjectInDirectory(directory, out FileInfo? projectFile))
        {
            passedChecks++;

            totalChecks++;
            if (ValidateProjectFile(projectFile!))
            {
                passedChecks++;
            }
            else
            {
                corruptedFiles.Add(projectFile!.Name);
            }
        }
        else
        {
            missingFiles.Add("*.uproject");
        }

        // Required directories
        string[] requiredDirs = ["Content"];
        foreach (string dirName in requiredDirs)
        {
            totalChecks++;
            DirectoryInfo dir = new(Path.Combine(directory.FullName, dirName));
            if (dir.Exists)
            {
                passedChecks++;
            }
            else
            {
                missingDirs.Add(dirName);
                warnings.Add($"Missing recommended directory: {dirName}");
            }
        }

        // Module integrity (Source/<Module>/<Module>.Build.cs)
        UnrealProjectDescription desc = default;
        if (projectFile != null)
        {
            try
            {
                desc = ParseProjectDescription(projectFile);
            }
            catch
            {
                // Skip module checks when the description cannot be parsed
            }
        }

        if (desc.Modules != null)
        {
            foreach (UnrealEngineModuleDescription module in desc.Modules)
            {
                if (string.IsNullOrEmpty(module.Name))
                    continue;

                totalChecks++;
                string modulePath = Path.Combine(directory.FullName, "Source", module.Name);
                DirectoryInfo moduleDir = new(modulePath);

                if (moduleDir.Exists)
                {
                    passedChecks++;

                    totalChecks++;
                    string buildFile = Path.Combine(modulePath, module.Name + ".Build.cs");
                    if (File.Exists(buildFile))
                    {
                        passedChecks++;
                    }
                    else
                    {
                        missingFiles.Add($"{module.Name}.Build.cs");
                    }
                }
                else
                {
                    missingDirs.Add($"Source/{module.Name}");
                }
            }
        }

        int integrityScore = totalChecks > 0 ? passedChecks * 100 / totalChecks : 0;
        bool isValid = errors.Count == 0 && integrityScore >= 70;

        string message = isValid
            ? $"Project integrity OK (score {integrityScore}/100)"
            : $"Project integrity check failed (score {integrityScore}/100)";

        var data = new
        {
            isValid,
            integrityScore,
            errors,
            warnings,
            missingFiles,
            missingDirs,
            corruptedFiles
        };

        // Always return Successful so the AI receives the full details;
        // isValid inside data carries the verdict.
        return ToolResult.Successful(message, data);
    }

    // ===== Ported ProjectService helpers =====

    /// <summary>
    /// Checks whether the directory contains a valid UE project.
    /// </summary>
    private static bool CheckProjectInDirectory(DirectoryInfo directory, out FileInfo? projectFile)
    {
        projectFile = null;

        try
        {
            FileInfo[] projectFiles = GetProjectFiles(directory);
            if (projectFiles.Length == 0)
                return false;

            projectFile = SelectBestProjectFile(directory, projectFiles);
            return ValidateProjectFile(projectFile);
        }
        catch
        {
            projectFile = null;
            return false;
        }
    }

    /// <summary>
    /// Gets all .uproject files in the directory (top level only).
    /// </summary>
    private static FileInfo[] GetProjectFiles(DirectoryInfo directory)
    {
        try
        {
            return directory.GetFiles("*.uproject", SearchOption.TopDirectoryOnly);
        }
        catch
        {
            return [];
        }
    }

    /// <summary>
    /// Picks the best project file — prefers the one whose stem matches the directory name.
    /// </summary>
    private static FileInfo SelectBestProjectFile(DirectoryInfo directory, FileInfo[] projectFiles)
    {
        if (projectFiles.Length == 1)
            return projectFiles[0];

        string dirName = directory.Name;
        FileInfo? matchingFile = projectFiles.FirstOrDefault(f =>
            string.Equals(Path.GetFileNameWithoutExtension(f.Name), dirName, StringComparison.OrdinalIgnoreCase));

        return matchingFile ?? projectFiles[0];
    }

    /// <summary>
    /// Validates a project file: must parse as JSON with FileVersion > 0.
    /// </summary>
    private static bool ValidateProjectFile(FileInfo projectFile)
    {
        if (!projectFile.Exists)
            return false;

        try
        {
            string? content = UnrealTextReader.LoadStringAuto(projectFile);
            if (string.IsNullOrWhiteSpace(content))
                return false;

            UnrealProjectDescription? projectDesc = JsonSerializer.Deserialize<UnrealProjectDescription>(content, JsonOptions);
            return projectDesc.HasValue && projectDesc.Value.FileVersion > 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Parses the project description from a .uproject file.
    /// Throws on failure.
    /// </summary>
    private static UnrealProjectDescription ParseProjectDescription(FileInfo projectFile)
    {
        string? content = UnrealTextReader.LoadStringAuto(projectFile);
        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("Project file is empty");

        UnrealProjectDescription? desc = JsonSerializer.Deserialize<UnrealProjectDescription>(content, JsonOptions);
        return desc ?? throw new InvalidOperationException("Failed to parse project description");
    }

    /// <summary>
    /// Reads the EngineAssociation of the project in the given directory.
    /// Returns null when the directory is not a valid UE project or parsing fails.
    /// </summary>
    internal static string? GetEngineAssociation(DirectoryInfo directory)
    {
        if (!CheckProjectInDirectory(directory, out FileInfo? projectFile))
            return null;

        try
        {
            return ParseProjectDescription(projectFile!).EngineAssociation;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Resolves a valid .uproject from a path that may point to the project
    /// file itself or to the folder containing it. Returns null when none found.
    /// </summary>
    internal static FileInfo? ResolveProjectFile(string path)
    {
        try
        {
            if (File.Exists(path) && path.EndsWith(".uproject", StringComparison.OrdinalIgnoreCase))
            {
                var file = new FileInfo(path);
                return ValidateProjectFile(file) ? file : null;
            }

            var directory = new DirectoryInfo(path);
            if (!directory.Exists)
                return null;

            return CheckProjectInDirectory(directory, out FileInfo? projectFile) ? projectFile : null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Identifies the project type: CppProject or BlueprintOnly.
    /// A UE project has exactly two forms — a C++ project (which always
    /// contains Blueprints as well) or a pure Blueprint project. Modules
    /// declared in the .uproject also imply a C++ project even if sources
    /// are missing; anything without C++ code is a Blueprint project.
    /// </summary>
    private static UnrealProjectType IdentifyProjectType(DirectoryInfo directory, UnrealProjectDescription description)
    {
        if (DetectCppCode(directory))
            return UnrealProjectType.CppProject;

        // Modules declared in .uproject imply a C++ project even if sources are missing
        if (description.Modules != null && description.Modules.Length > 0)
            return UnrealProjectType.CppProject;

        // No C++ code detected — it is a pure Blueprint project
        return UnrealProjectType.BlueprintOnly;
    }

    /// <summary>
    /// Scans the well-known UE project directories at the project root.
    /// </summary>
    private static string[] ScanSpecialDirectories(DirectoryInfo projectDirectory)
    {
        string[] specialDirNames = ["Source", "Content", "Config", "Plugins", "Binaries", "Intermediate", "Saved", "Build"];
        var found = new List<string>();

        foreach (string dirName in specialDirNames)
        {
            try
            {
                if (Directory.Exists(Path.Combine(projectDirectory.FullName, dirName)))
                    found.Add(dirName);
            }
            catch
            {
                // Ignore access errors
            }
        }

        return found.ToArray();
    }

    /// <summary>
    /// Detects C++ sources under the Source directory.
    /// </summary>
    private static bool DetectCppCode(DirectoryInfo projectDirectory)
    {
        DirectoryInfo sourceDir = new(Path.Combine(projectDirectory.FullName, "Source"));
        if (!sourceDir.Exists)
            return false;

        try
        {
            return sourceDir.GetFiles("*.cpp", SearchOption.AllDirectories).Length > 0 ||
                   sourceDir.GetFiles("*.h", SearchOption.AllDirectories).Length > 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Detects blueprint assets (*.uasset) under the Content directory.
    /// </summary>
    private static bool DetectBlueprints(DirectoryInfo projectDirectory)
    {
        DirectoryInfo contentDir = new(Path.Combine(projectDirectory.FullName, "Content"));
        if (!contentDir.Exists)
            return false;

        try
        {
            return contentDir.GetFiles("*.uasset", SearchOption.AllDirectories).Length > 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Collects module names declared in the project description.
    /// </summary>
    private static string[] GetModuleNames(UnrealProjectDescription description)
    {
        if (description.Modules == null)
            return [];

        return description.Modules
            .Where(m => !string.IsNullOrEmpty(m.Name))
            .Select(m => m.Name)
            .ToArray()!;
    }

    /// <summary>
    /// Detects plugins with their effective enabled state.
    /// Project plugins live in the project Plugins folder; engine plugins live
    /// in the engine installation. A same-named entry in the .uproject Plugins
    /// array overrides the .uplugin EnabledByDefault value. Missing
    /// EnabledByDefault defaults to true for project plugins and false for
    /// engine plugins. When <paramref name="allPlugins"/> is false only
    /// enabled plugins are returned.
    /// </summary>
    private static List<DetectedPlugin> DetectPlugins(
        DirectoryInfo projectDirectory, UnrealProjectDescription description,
        bool allPlugins, List<string> warnings)
    {
        // .uproject declarations keyed by plugin name — they override EnabledByDefault
        var overrides = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        if (description.Plugins != null)
        {
            foreach (UnrealEnginePluginDescription entry in description.Plugins)
            {
                if (!string.IsNullOrEmpty(entry.Name))
                    overrides.TryAdd(entry.Name, entry.Enabled);
            }
        }

        var result = new List<DetectedPlugin>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // 1. Project plugins — stored under the project Plugins folder
        result.AddRange(ScanPluginDirectory(
            Path.Combine(projectDirectory.FullName, "Plugins"),
            "project", defaultEnabledByDefault: true, overrides, seen));

        // 2. Engine plugins
        if (allPlugins)
        {
            UnrealEngineInstallInfo? engine = UnrealEngineTool.FindUnrealEngine(description.EngineAssociation ?? "");
            if (engine == null)
            {
                warnings.Add("Custom engine not found");
            }
            else
            {
                result.AddRange(ScanPluginDirectory(
                    Path.Combine(engine.Value.InstallLocation ?? "", "Engine", "Plugins"),
                    "engine", defaultEnabledByDefault: false, overrides, seen));
            }
        }

        // .uproject entries that no scanned plugin shadowed — engine-side declarations
        foreach (KeyValuePair<string, bool> entry in overrides)
        {
            if (seen.Contains(entry.Key))
                continue;

            result.Add(new DetectedPlugin { Name = entry.Key, Enabled = entry.Value, Location = "engine" });
        }

        return allPlugins ? result : result.FindAll(p => p.Enabled);
    }

    /// <summary>
    /// Recursively scans a Plugins directory for .uplugin files and resolves
    /// each plugin's effective enabled state. The plugin name is the .uplugin
    /// file name; the first occurrence of a name wins (project copies shadow
    /// engine copies because they are scanned first).
    /// </summary>
    private static IEnumerable<DetectedPlugin> ScanPluginDirectory(
        string pluginsDirectory, string location, bool defaultEnabledByDefault,
        IReadOnlyDictionary<string, bool> overrides, HashSet<string> seen)
    {
        if (!Directory.Exists(pluginsDirectory))
            yield break;

        string[] pluginFiles;
        try
        {
            pluginFiles = Directory.GetFiles(pluginsDirectory, "*.uplugin", SearchOption.AllDirectories);
        }
        catch
        {
            yield break;
        }

        foreach (string file in pluginFiles)
        {
            string name = Path.GetFileNameWithoutExtension(file);
            if (string.IsNullOrEmpty(name) || !seen.Add(name))
                continue;

            bool enabled = defaultEnabledByDefault;
            if (TryReadEnabledByDefault(new FileInfo(file), out bool enabledByDefault))
                enabled = enabledByDefault;

            // A same-named .uproject declaration overrides EnabledByDefault
            if (overrides.TryGetValue(name, out bool projectEnabled))
                enabled = projectEnabled;

            yield return new DetectedPlugin { Name = name, Enabled = enabled, Location = location };
        }
    }

    /// <summary>
    /// Locates the ForgeMind companion plugin (ForgeMindForUE) in the project
    /// Plugins folder or the engine installation — its presence and enablement
    /// gate engine-side features. Uses targeted lookups instead of full scans.
    /// A project copy shadows an engine copy, so the project side is checked first.
    /// </summary>
    private static object DetectCompanionPlugin(DirectoryInfo projectDirectory, UnrealProjectDescription description)
    {
        FileInfo? upluginFile = FindPluginFile(
            Path.Combine(projectDirectory.FullName, "Plugins"), CompanionPluginName);
        string location = "project";
        bool defaultEnabledByDefault = true;

        if (upluginFile == null)
        {
            // Engine side — resolve the engine installation only when the project side misses
            UnrealEngineInstallInfo? engine = UnrealEngineTool.FindUnrealEngine(description.EngineAssociation ?? "");
            if (engine != null)
            {
                upluginFile = FindPluginFile(
                    Path.Combine(engine.Value.InstallLocation ?? "", "Engine", "Plugins"), CompanionPluginName);
                location = "engine";
                defaultEnabledByDefault = false;
            }
        }

        if (upluginFile == null)
            return new { installed = false, enabled = false, location = (string?)null };

        bool enabled = defaultEnabledByDefault;
        if (TryReadEnabledByDefault(upluginFile, out bool enabledByDefault))
            enabled = enabledByDefault;

        // A same-named .uproject declaration overrides EnabledByDefault
        if (description.Plugins != null)
        {
            foreach (UnrealEnginePluginDescription entry in description.Plugins)
            {
                if (string.Equals(entry.Name, CompanionPluginName, StringComparison.OrdinalIgnoreCase))
                {
                    enabled = entry.Enabled;
                    break;
                }
            }
        }

        return new { installed = true, enabled, location };
    }

    /// <summary>
    /// Finds a specific .uplugin file by plugin name anywhere under a Plugins
    /// directory. Returns null when absent or unreadable.
    /// </summary>
    private static FileInfo? FindPluginFile(string pluginsDirectory, string pluginName)
    {
        if (!Directory.Exists(pluginsDirectory))
            return null;

        try
        {
            string[] files = Directory.GetFiles(pluginsDirectory, pluginName + ".uplugin", SearchOption.AllDirectories);
            return files.Length > 0 ? new FileInfo(files[0]) : null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Reads the EnabledByDefault field from a .uplugin descriptor file.
    /// Returns false when the field is absent or the file cannot be parsed.
    /// </summary>
    private static bool TryReadEnabledByDefault(FileInfo upluginFile, out bool enabledByDefault)
    {
        enabledByDefault = false;
        try
        {
            string? content = UnrealTextReader.LoadStringAuto(upluginFile);
            if (string.IsNullOrWhiteSpace(content))
                return false;

            UnrealPluginDescriptor? descriptor = JsonSerializer.Deserialize<UnrealPluginDescriptor>(content, JsonOptions);
            if (descriptor?.EnabledByDefault == null)
                return false;

            enabledByDefault = descriptor.Value.EnabledByDefault.Value;
            return true;
        }
        catch
        {
            return false;
        }
    }
}
