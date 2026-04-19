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
using SiliconLife.Default.Logging;
using SiliconLife.Default.Web;
using System.Text;

namespace SiliconLife.Default;

public class Program
{
    private static readonly ILogger _logger;
    private static bool _shouldExit = false;
    private static CoreHost? _host;
    private static WebHost? _webHost;

    static Program()
    {
        LogManager.Instance.AddProvider(new ConsoleLoggerProvider());
        _logger = LogManager.Instance.GetLogger<Program>();
    }

    public static async Task Main(string[] args)
    {
        _logger.Info("Application starting...");

        RegisterLocalizations();
        ConfigDataBaseConverter.RegisterConfigType("Default", typeof(DefaultConfigData));

        Config config = Config.Instance;
        config.Initialize(new DefaultConfigData());
        config.LoadConfig();

        DefaultConfigData configData = (DefaultConfigData)config.Data;
        LogManager.Instance.AddProvider(new FileSystemLoggerProvider(configData));
        _logger.Info("Configuration loaded: endpoint={0}, model={1}", 
            configData.AIConfig["endpoint"], configData.AIConfig["model"]);

        DefaultLocalizationBase localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(configData.Language);

        Console.WriteLine(localization.WelcomeMessage);
        Console.WriteLine();

        IStorage storage = new FileSystemStorage(configData.DataDirectory.FullName);
        ITimeStorage timeStorage = new FileSystemTimeStorage(
            Path.Combine(configData.DataDirectory.FullName, "chat"));

        ChatSystem chatSystem = new ChatSystem(timeStorage);
        _logger.Info("Initialized: ChatSystem");

        ITimeStorage auditStorage = new FileSystemTimeStorage(
            Path.Combine(configData.DataDirectory.FullName, "audit"));
        AuditLogger auditLogger = new AuditLogger(auditStorage);
        _logger.Info("Initialized: AuditLogger");

        ITimeStorage tokenUsageStorage = new FileSystemTimeStorage(
            Path.Combine(configData.DataDirectory.FullName, "token-usage"));
        TokenUsageAuditManager tokenUsageAuditManager = new TokenUsageAuditManager(tokenUsageStorage);
        _logger.Info("Initialized: TokenUsageAuditManager");

        GlobalACL globalAcl = new GlobalACL(storage);
        _logger.Info("Initialized: GlobalACL");

        Router router = new Router();
        router.SetInitialized(File.Exists(configData.GetConfigPath()));
        IIMProvider imProvider = new WebUIProvider(router);
        imProvider.ExitRequested += (s, e) => RequestExit();

        DefaultPermissionCallback permissionCallback = new DefaultPermissionCallback(configData.DataDirectory.FullName);
        IMPermissionAskHandler askHandler = new IMPermissionAskHandler(imProvider);

        IMManager imManager = new IMManager(imProvider, chatSystem, MainLoop.BeingManager);
        _logger.Info("Initialized: IMManager");

        DefaultSiliconBeingFactory beingFactory = new DefaultSiliconBeingFactory(
            configData.AIConfig,
            storage,
            timeStorage,
            configData.DataDirectory.FullName,
            permissionCallback,
            askHandler);

        DynamicBeingLoader dynamicBeingLoader = new DynamicBeingLoader();

        CoreHostBuilder builder = new CoreHostBuilder()
            .SetConfig(configData)
            .SetStorage(storage)
            .SetTimeStorage(timeStorage)
            .SetChatSystem(chatSystem)
            .SetAuditLogger(auditLogger)
            .SetGlobalACL(globalAcl)
            .SetIMProvider(imProvider)
            .SetIMManager(imManager)
            .SetBeingFactory(beingFactory)
            .SetDynamicBeingLoader(dynamicBeingLoader)
            .SetTokenUsageAuditManager(tokenUsageAuditManager);

        _host = builder.Build();

        await _host.StartAsync();
        _logger.Info("CoreHost started");

        // Only create curator if it was previously initialized (CuratorGuid is set)
        if (configData.CuratorGuid != Guid.Empty)
        {
            SiliconBeingBase defaultBeing = beingFactory.CreateBeing(configData.CuratorGuid, "");
            _logger.Info("Curator created: {0} ({1})", defaultBeing.Name, defaultBeing.Id);
            RegisterAndConfigureCurator(defaultBeing, configData, dynamicBeingLoader);
        }

        await StartWebServerAsync(configData, router, (WebUIProvider)imProvider, beingFactory, dynamicBeingLoader, localization);

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
        _logger.Info("Application shutting down...");

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
        _logger.Info("Application shutdown complete");
    }

    private static async Task StartWebServerAsync(DefaultConfigData configData, Router router, WebUIProvider webUiProvider, DefaultSiliconBeingFactory beingFactory, DynamicBeingLoader dynamicBeingLoader, DefaultLocalizationBase localization)
    {
        WebCodeBrowser codeBrowser = new WebCodeBrowser();
        SkinManager skinManager = new SkinManager();
        skinManager.DiscoverSkins(typeof(SkinManager).Assembly);

        ServiceLocator locator = ServiceLocator.Instance;
        locator.Register(skinManager);
        locator.Register(codeBrowser);
        locator.Register(router);
        locator.Register<Func<Guid, TaskCompletionSource<AskPermissionResult>>>(webUiProvider.GetPermissionTcs);

        router.SetOnFirstInit((curatorName) =>
        {
            // First-run initialization: create curator with a pre-generated GUID
            // Fix: creating the being before saving CuratorGuid caused curator-specific tools to fail loading on first launch
            Guid curatorGuid = Guid.NewGuid();
            configData.CuratorGuid = curatorGuid;
            SiliconBeingBase curator = beingFactory.CreateBeing(curatorGuid, curatorName);
            configData.SaveConfig();
            _logger.Info("Curator created: {0} ({1})", curator.Name, curator.Id);

            // Write default soul file
            string beingDirectory = Path.Combine(configData.DataDirectory.FullName, "SiliconManager", curator.Id.ToString());
            string soulContent = localization.DefaultCuratorSoul;
            curator.SoulContent = soulContent;
            SoulFileManager.SaveSoul(beingDirectory, soulContent);

            RegisterAndConfigureCurator(curator, configData, dynamicBeingLoader);
        });

        router.RegisterControllers();

        _webHost = new WebHost(configData.WebPort, router, configData.AllowIntranet);

        try
        {
            await _webHost.StartAsync();
            _logger.Info("Web server started on port {0}", configData.WebPort);
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
            _logger.Error("Failed to start web server: {0}", ex, ex.Message);
        }
    }

    private static void RegisterLocalizations()
    {
        LocalizationManager.Instance.Register<ZhCN>(Language.ZhCN);
        LocalizationManager.Instance.Register<EnUS>(Language.EnUS);
        LocalizationManager.Instance.Register<ZhHK>(Language.ZhHK);
        LocalizationManager.Instance.Register<ZhSG>(Language.ZhSG);
        LocalizationManager.Instance.Register<ZhMO>(Language.ZhMO);
        LocalizationManager.Instance.Register<ZhTW>(Language.ZhTW);
        LocalizationManager.Instance.Register<ZhMY>(Language.ZhMY);
        
        // English variants
        LocalizationManager.Instance.Register<EnGB>(Language.EnGB);
        LocalizationManager.Instance.Register<EnCA>(Language.EnCA);
        LocalizationManager.Instance.Register<EnAU>(Language.EnAU);
        LocalizationManager.Instance.Register<EnIN>(Language.EnIN);
        LocalizationManager.Instance.Register<EnSG>(Language.EnSG);
        LocalizationManager.Instance.Register<EnZA>(Language.EnZA);
        LocalizationManager.Instance.Register<EnIE>(Language.EnIE);
        LocalizationManager.Instance.Register<EnNZ>(Language.EnNZ);
        LocalizationManager.Instance.Register<EnMY>(Language.EnMY);
    }

    public static void RequestExit()
    {
        _shouldExit = true;
    }

    /// <summary>
    /// Registers a curator being and configures its custom permission callback and code
    /// </summary>
    private static void RegisterAndConfigureCurator(SiliconBeingBase curator, DefaultConfigData configData, DynamicBeingLoader dynamicBeingLoader)
    {
        string beingDirectory = Path.Combine(configData.DataDirectory.FullName, "SiliconManager", curator.Id.ToString());

        if (DynamicBeingLoader.HasCustomPermissionCallback(beingDirectory))
        {
            try
            {
                CompilationResult permResult = dynamicBeingLoader.LoadPermissionCallback(curator.Id, beingDirectory);
                if (permResult.Success && permResult.CompiledType != null)
                {
                    MainLoop.BeingManager.ReplacePermissionCallback(curator.Id, permResult.CompiledType);
                    _logger.Info("Loaded custom permission callback for curator {0}", curator.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.Warn("Failed to load custom permission callback for curator {0}", ex, curator.Id);
            }
        }

        MainLoop.BeingManager.RegisterBeing(curator);
        _logger.Info("Registered curator: {0} ({1})", curator.Name, curator.Id);

        if (DynamicBeingLoader.HasCustomCode(beingDirectory))
        {
            try
            {
                Type? customType = dynamicBeingLoader.LoadBeingType(curator.Id, beingDirectory);
                if (customType != null)
                {
                    MainLoop.BeingManager.ReplaceBeing(curator.Id, customType);
                    _logger.Info("Loaded custom code for curator {0}: {1}", curator.Id, customType.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.Warn("Failed to load custom code for curator {0}", ex, curator.Id);
            }
        }
    }
}
