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

using SiliconLife.Collective;
using SiliconLife.Default;
using SiliconLife.Default.IM;
using SiliconLife.Default.Web;
using System.Text;

namespace SiliconLife.Default;

public class Program
{
    private static bool _shouldExit = false;
    private static CoreHost? _host;
    private static WebHost? _webHost;

    public static async Task Main(string[] args)
    {
        RegisterLocalizations();
        ConfigDataBaseConverter.RegisterConfigType("Default", typeof(DefaultConfigData));

        Config config = Config.Instance;
        config.Initialize(new DefaultConfigData());
        config.LoadConfig();

        DefaultConfigData configData = (DefaultConfigData)config.Data;
        DefaultLocalizationBase localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(configData.Language);

        Console.WriteLine(localization.WelcomeMessage);
        Console.WriteLine();

        IAIClientFactory aiClientFactory = new OllamaClientFactory();
        IAIClient aiClient = aiClientFactory.CreateClient(configData.OllamaEndpoint, configData.DefaultModel);

        IStorage storage = new FileSystemStorage(configData.DataDirectory);
        ITimeStorage timeStorage = new FileSystemTimeStorage(
            Path.Combine(configData.DataDirectory, "chat"));

        ChatSystem chatSystem = new ChatSystem(timeStorage);

        ITimeStorage auditStorage = new FileSystemTimeStorage(
            Path.Combine(configData.DataDirectory, "audit"));
        AuditLogger auditLogger = new AuditLogger(auditStorage);

        GlobalACL globalAcl = new GlobalACL(storage);

        var router = new Router();
        router.SetInitialized(File.Exists(configData.GetConfigPath()));
        IIMProvider imProvider = new WebUIProvider(router);
        imProvider.ExitRequested += (s, e) => RequestExit();

        DefaultPermissionCallback permissionCallback = new DefaultPermissionCallback(configData.DataDirectory);
        IMPermissionAskHandler askHandler = new IMPermissionAskHandler(imProvider);

        IMManager imManager = new IMManager(imProvider, chatSystem, MainLoop.BeingManager);

        DefaultSiliconBeingFactory beingFactory = new DefaultSiliconBeingFactory(
            aiClient,
            storage,
            timeStorage,
            configData.DataDirectory,
            configData.UserGuid,
            permissionCallback,
            askHandler);

        DynamicBeingLoader dynamicBeingLoader = new DynamicBeingLoader();

        CoreHostBuilder builder = new CoreHostBuilder()
            .SetConfig(configData)
            .SetAIClient(aiClient)
            .SetStorage(storage)
            .SetTimeStorage(timeStorage)
            .SetChatSystem(chatSystem)
            .SetAuditLogger(auditLogger)
            .SetGlobalACL(globalAcl)
            .SetIMProvider(imProvider)
            .SetIMManager(imManager)
            .SetBeingFactory(beingFactory)
            .SetDynamicBeingLoader(dynamicBeingLoader);

        _host = builder.Build();

        await _host.StartAsync();

        SiliconBeingBase defaultBeing = beingFactory.CreateBeing(configData.CuratorGuid, "Default");

        string beingDirectory = Path.Combine(configData.DataDirectory, "SiliconManager", configData.CuratorGuid.ToString());

        if (DynamicBeingLoader.HasCustomPermissionCallback(beingDirectory))
        {
            try
            {
                CompilationResult permResult = dynamicBeingLoader.LoadPermissionCallback(configData.CuratorGuid, beingDirectory);
                if (permResult.Success && permResult.CompiledType != null)
                {
                    MainLoop.BeingManager.ReplacePermissionCallback(configData.CuratorGuid, permResult.CompiledType);
                }
            }
            catch
            {
            }
        }

        MainLoop.BeingManager.RegisterBeing(defaultBeing);

        if (DynamicBeingLoader.HasCustomCode(beingDirectory))
        {
            try
            {
                Type? customType = dynamicBeingLoader.LoadBeingType(configData.CuratorGuid, beingDirectory);
                if (customType != null)
                {
                    MainLoop.BeingManager.ReplaceBeing(configData.CuratorGuid, customType);
                }
            }
            catch
            {
            }
        }

        await StartWebServerAsync(configData, router, chatSystem, configData.UserGuid, (WebUIProvider)imProvider);

        Console.CancelKeyPress += async (s, e) =>
        {
            e.Cancel = true;
            await ShutdownAsync();
        };

        while (!_shouldExit)
        {
            await Task.Delay(100);
        }

        await ShutdownAsync();
    }

    private static async Task ShutdownAsync()
    {
        if (_webHost != null)
        {
            await _webHost.StopAsync();
            _webHost.Dispose();
        }

        if (_host != null)
        {
            await _host.StopAsync();
        }

        _shouldExit = true;
    }

    private static async Task StartWebServerAsync(DefaultConfigData configData, Router router, ChatSystem chatSystem, Guid userId, WebUIProvider webUiProvider)
    {
        var beingManager = MainLoop.BeingManager;
        var codeBrowser = new WebCodeBrowser();
        var localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(configData.Language);
        var skinManager = new SkinManager();
        skinManager.DiscoverSkins(typeof(SkinManager).Assembly);
        router.RegisterControllers(beingManager, chatSystem, userId, webUiProvider.GetPermissionTcs, codeBrowser, configData, localization, skinManager);

        if (!string.IsNullOrEmpty(configData.StaticFilesPath) && Directory.Exists(configData.StaticFilesPath))
        {
            router.SetStaticFilesPath(configData.StaticFilesPath);
        }

        _webHost = new WebHost(configData.WebPort, router, configData.AllowIntranet);

        try
        {
            await _webHost.StartAsync();
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = $"http://localhost:{configData.WebPort}/",
                    UseShellExecute = true
                });
            }
            catch
            {
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to start web server: {ex.Message}");
        }
    }

    private static void RegisterLocalizations()
    {
        LocalizationManager.Instance.Register<ZhCN>(Language.ZhCN);
        LocalizationManager.Instance.Register<EnUS>(Language.EnUS);
    }

    public static void RequestExit()
    {
        _shouldExit = true;
    }
}
