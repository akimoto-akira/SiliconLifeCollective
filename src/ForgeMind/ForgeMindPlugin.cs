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

using ForgeMind.Bridge;
using SiliconLife.Collective;

// Certified-author mode: the GUID is derived from the assembly name via the host's
// HMACSHA256 scheme (see PluginLoader.ComputeAuthorCertGuid). With a valid declaration
// the host treats this plugin as first-party and bypasses the metadata security scan,
// so compiler-emitted TypeRefs (Unsafe / MemoryMarshal etc.) no longer block loading.
[assembly: PluginAuthorCert("4a4cdaaf-4f22-0c54-ff37-d049fcd47534")]

namespace ForgeMind;

/// <summary>
/// ForgeMind plugin — extends the SiliconLife host with ForgeMind capabilities.
/// Implements the full <see cref="IPlugin"/> lifecycle and logs each transition
/// through the host <see cref="ILogger"/>.
/// <para>Provides UE tools (project / engine / sln / switchversion / launch /
/// build / knowledge / editor) and hosts the TCP bridge server for the ForgeMindForUE
/// companion plugin.</para>
/// </summary>
[PluginCapability(Capability.FileIO,
    Reason = "Reads .uproject files, LauncherInstalled.dat and project directories for UE detection")]
[PluginCapability(Capability.Registry,
    Reason = "Reads HKCU Epic Games builds registry to discover source-built Unreal Engines")]
[PluginCapability(Capability.Process,
    Reason = "Runs UnrealVersionSelector.exe /projectfiles to regenerate Visual Studio project files")]
[PluginCapability(Capability.Network,
    Reason = "Local loopback TCP bridge to the ForgeMindForUE companion plugin")]
public class ForgeMindPlugin : IPlugin
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<ForgeMindPlugin>();

    /// <summary>
    /// TCP bridge server for ForgeMindForUE companions. Created on load so
    /// tools can reference it; actually listens only after <see cref="OnStart"/>.
    /// </summary>
    internal static ForgeMindBridgeServer BridgeServer { get; } = new();

    /// <summary>
    /// System-message contributor reporting live UE bridge state into AI requests.
    /// </summary>
    private static readonly ForgeMindSystemContext SystemContext = new();

    public string Id => "com.siliconlife.forgemind";

    public string Version => "1.0.0";

    public string GetName(Language language) => language switch
    {
        Language.ZhCN => "ForgeMind",
        Language.ZhHK => "ForgeMind",
        Language.JaJP => "ForgeMind",
        Language.KoKR => "ForgeMind",
        _ => "ForgeMind"
    };

    public string GetDescription(Language language) => language switch
    {
        Language.ZhCN => "ForgeMind 插件：为宿主提供 ForgeMind 扩展能力。",
        Language.ZhHK => "ForgeMind 插件：為宿主提供 ForgeMind 擴展能力。",
        Language.JaJP => "ForgeMind プラグイン：ホストに ForgeMind 拡張機能を提供します。",
        Language.KoKR => "ForgeMind 플러그인: 호스트에 ForgeMind 확장 기능을 제공합니다.",
        _ => "ForgeMind plugin: extends the host with ForgeMind capabilities."
    };

    public string GetAuthor(Language language) => "Hoshino Kennji";

    /// <summary>
    /// Called once when the plugin DLL is loaded into the host process.
    /// Validate configuration and register types/tools here.
    /// </summary>
    public void OnLoad()
    {
        ContextManager.RegisterSystemContextContributor(SystemContext);
        _logger.Info(null, "[ForgeMind] OnLoad — plugin v{0} loaded", Version);
    }

    /// <summary>
    /// Called when the host has fully started and all plugins have been loaded.
    /// Safe to interact with other plugins and shared services here.
    /// </summary>
    public void OnStart()
    {
        _logger.Info(null, "[ForgeMind] OnStart — host ready, plugin started");

        try
        {
            BridgeServer.Start();
        }
        catch (Exception ex)
        {
            _logger.Error(null, "[ForgeMind] Bridge server failed to start: {0}", ex.Message);
        }
    }

    /// <summary>
    /// Called when the host is shutting down gracefully.
    /// Release resources, flush buffers, and save state here.
    /// </summary>
    public void OnStop()
    {
        try
        {
            BridgeServer.Stop();
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "[ForgeMind] Bridge server stop failed: {0}", ex.Message);
        }

        _logger.Info(null, "[ForgeMind] OnStop — plugin stopping");
    }

    /// <summary>
    /// Called when the plugin is being unloaded from the host process.
    /// Perform final cleanup here — this is the last lifecycle method called.
    /// </summary>
    public void OnUnload()
    {
        ContextManager.UnregisterSystemContextContributor(SystemContext);
        _logger.Info(null, "[ForgeMind] OnUnload — plugin unloaded");
    }
}
