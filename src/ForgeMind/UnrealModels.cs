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

namespace ForgeMind;

/// <summary>
/// Wrapper for the Epic Launcher installation record file
/// (<c>C:\ProgramData\Epic\UnrealEngineLauncher\LauncherInstalled.dat</c>).
/// </summary>
internal sealed class LauncherInstallList
{
    /// <summary>
    /// List of installations registered by the Epic Launcher.
    /// </summary>
    public List<UnrealEngineInstallInfo>? InstallationList { get; set; }
}

/// <summary>
/// Unreal Engine application installation info.
/// Ported from UnrealEngineCacheCtrl.Models.UnrealEngineInstallInfo.
/// </summary>
internal struct UnrealEngineInstallInfo
{
    /// <summary>Install path.</summary>
    public string InstallLocation { get; set; }

    /// <summary>Application namespace id ("ue" = Epic Launcher, "regUe" = registry / source build).</summary>
    public string NamespaceId { get; set; }

    /// <summary>Item id (GUID without dashes for registry builds).</summary>
    public string ItemId { get; set; }

    /// <summary>Artifact id.</summary>
    public string ArtifactId { get; set; }

    /// <summary>Application version string.</summary>
    public string AppVersion { get; set; }

    /// <summary>Application name (e.g. "UE_5.4").</summary>
    public string AppName { get; set; }

    /// <summary>Returns the application name.</summary>
    public override readonly string ToString() => AppName;
}

/// <summary>
/// Unreal build info parsed from <c>Engine/Build/Build.version</c>.
/// Ported from UnrealEngineCacheCtrl.Models.UnrealBuildInfo.
/// </summary>
internal struct UnrealBuildInfo
{
    /// <summary>Major version.</summary>
    public int MajorVersion { get; set; }

    /// <summary>Minor version.</summary>
    public int MinorVersion { get; set; }

    /// <summary>Patch version.</summary>
    public int PatchVersion { get; set; }

    /// <summary>Changelist.</summary>
    public ulong Changelist { get; set; }

    /// <summary>Compatible changelist.</summary>
    public ulong CompatibleChangelist { get; set; }

    /// <summary>Whether this is a licensee version.</summary>
    public int IsLicenseeVersion { get; set; }

    /// <summary>Whether this is a promoted build.</summary>
    public int IsPromotedBuild { get; set; }

    /// <summary>Branch name.</summary>
    public string BranchName { get; set; }

    /// <summary>Classic version representation.</summary>
    public readonly Version Version => new(MajorVersion, MinorVersion, PatchVersion);

    /// <summary>Formats as "Major.Minor.Patch-Changelist+Branch".</summary>
    public override readonly string ToString() =>
        MajorVersion + "." + MinorVersion + "." + PatchVersion + "-" + Changelist + "+" + BranchName;
}

/// <summary>
/// Unreal project description parsed from a <c>.uproject</c> file.
/// Ported from UnrealEngineCacheCtrl.Models.UnrealProjectDescription.
/// </summary>
internal struct UnrealProjectDescription
{
    /// <summary>Project file version.</summary>
    public int FileVersion { get; set; }

    /// <summary>Engine association (launcher version like "5.4" or a source-build GUID).</summary>
    public string EngineAssociation { get; set; }

    /// <summary>Project category.</summary>
    public string Category { get; set; }

    /// <summary>Project description.</summary>
    public string Description { get; set; }

    /// <summary>Whether engine default plugins are disabled by default.</summary>
    public bool DisableEnginePluginsByDefault { get; set; }

    /// <summary>Whether this is an enterprise project.</summary>
    public bool Enterprise { get; set; }

    /// <summary>Additional plugin directories.</summary>
    public string[]? AdditionalPluginDirectories { get; set; }

    /// <summary>Supported target platforms.</summary>
    public string[]? TargetPlatforms { get; set; }

    /// <summary>Epic sample name hash.</summary>
    public string EpicSampleNameHash { get; set; }

    /// <summary>Module list.</summary>
    public UnrealEngineModuleDescription[]? Modules { get; set; }

    /// <summary>Plugin list.</summary>
    public UnrealEnginePluginDescription[]? Plugins { get; set; }

    /// <summary>Returns the engine association.</summary>
    public override readonly string ToString() => EngineAssociation;
}

/// <summary>
/// Unreal module info inside a <c>.uproject</c> file.
/// </summary>
internal struct UnrealEngineModuleDescription
{
    /// <summary>Module name.</summary>
    public string Name { get; set; }

    /// <summary>Module type.</summary>
    public string Type { get; set; }

    /// <summary>Loading phase.</summary>
    public string LoadingPhase { get; set; }

    /// <summary>Whitelisted build platforms.</summary>
    public string[]? WhitelistPlatforms { get; set; }

    /// <summary>Blacklisted build platforms.</summary>
    public string[]? BlacklistPlatforms { get; set; }

    /// <summary>Returns the module name.</summary>
    public override readonly string ToString() => Name;
}

/// <summary>
/// Unreal plugin info inside a <c>.uproject</c> file.
/// </summary>
internal struct UnrealEnginePluginDescription
{
    /// <summary>Plugin name.</summary>
    public string Name { get; set; }

    /// <summary>Whether the plugin is enabled.</summary>
    public bool Enabled { get; set; }

    /// <summary>Whether the plugin is optional.</summary>
    public bool Optional { get; set; }

    /// <summary>Plugin description.</summary>
    public string Description { get; set; }

    /// <summary>Marketplace URL.</summary>
    public string MarketplaceURL { get; set; }

    /// <summary>Whitelisted build platforms.</summary>
    public string[]? WhitelistPlatforms { get; set; }

    /// <summary>Blacklisted build platforms.</summary>
    public string[]? BlacklistPlatforms { get; set; }

    /// <summary>Returns the plugin name.</summary>
    public override readonly string ToString() => Name;
}

/// <summary>
/// Subset of a <c>.uplugin</c> descriptor file needed for enablement detection.
/// </summary>
internal struct UnrealPluginDescriptor
{
    /// <summary>Whether the plugin is enabled by default. Null when the field is absent.</summary>
    public bool? EnabledByDefault { get; set; }
}

/// <summary>
/// A plugin detected by structure analysis, with its effective enablement
/// after applying the <c>.uproject</c> overrides.
/// </summary>
internal struct DetectedPlugin
{
    /// <summary>Plugin name (the .uplugin file name without extension).</summary>
    public string Name { get; set; }

    /// <summary>Effective enabled state.</summary>
    public bool Enabled { get; set; }

    /// <summary>Storage location: "project" (project Plugins folder) or "engine" (engine Plugins folder).</summary>
    public string Location { get; set; }
}

/// <summary>
/// Project type classification used by structure analysis.
/// A UE project has exactly two forms: a C++ project (which always contains
/// Blueprints as well) or a pure Blueprint project. Anything without C++
/// code is a Blueprint project.
/// </summary>
internal enum UnrealProjectType
{
    CppProject,
    BlueprintOnly
}
