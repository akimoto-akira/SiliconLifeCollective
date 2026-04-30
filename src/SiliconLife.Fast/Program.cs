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
using SiliconLife.Fast;
using SiliconLife.Fast.IM;
using SiliconLife.Fast.Knowledge;
using SiliconLife.Fast.Logging;
using SiliconLife.Fast.Tray;
using SiliconLife.Fast.Web;
using System.Text;
using SiliconLife.Common.Security;
using SiliconLife.Common.SiliconBeing;
using SiliconLife.Common.WebView;

using SiliconLife.Common.Localization;

namespace SiliconLife.Fast;

public class Program
{
    private static readonly ILogger _logger;
    private static bool _shouldExit = false;
    private static CoreHost? _host;
    private static WebHost? _webHost;
    private static TrayStatusWindow? _trayWindow;

    static Program()
    {
        _logger = LogManager.Instance.GetLogger<Program>();
    }

    public static async Task Main(string[] args)
    {
        _logger.Info(null, "Application starting...");

        RegisterLocalizations();
        ConfigDataBaseConverter.RegisterConfigType("Default", typeof(DefaultConfigData));

        // Initialize LiteDB before loading configuration
        string dbPath = GetDatabasePath();
        LiteDBManager.Initialize(dbPath);
        _logger.Info(null, "LiteDB initialized at {0}", dbPath);

        Config config = Config.Instance;
        config.Initialize(new DefaultConfigData());
        config.LoadConfig();

        DefaultConfigData configData = (DefaultConfigData)config.Data;
        LogManager.Instance.AddProvider(new LiteDBLoggerProvider());
        configData.AIConfig.TryGetValue("endpoint", out var endpointValue);
        configData.AIConfig.TryGetValue("model", out var modelValue);
        _logger.Info(null, "Configuration loaded: endpoint={0}, model={1}",
            endpointValue?.ToString() ?? "N/A", modelValue?.ToString() ?? "N/A");

        DefaultLocalizationBase localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(configData.Language);

        Console.WriteLine(localization.WelcomeMessage);
        Console.WriteLine();

        IStorage storage = new LiteDBStorage();
        ITimeStorage timeStorage = new LiteDBTimeStorage();

        // Register storage factories for SiliconBeing creation
        ServiceLocator.Instance.Register<Func<string, ITimeStorage>>(dir => new LiteDBTimeStorage());
        ServiceLocator.Instance.Register<Func<string, IStorage>>(dir => new LiteDBStorage());
        ServiceLocator.Instance.Register<Func<string, IWorkNoteStorage>>(dir => new LiteDBWorkNoteStorage());
        ServiceLocator.Instance.Register<Func<SiliconBeingBase, object>>(being => new PlaywrightWebView((DefaultSiliconBeing)being));
        _logger.Info(null, "Registered: Storage Factories");

        // Initialize project manager
        IProjectManager projectManager = new ProjectManager(storage, configData.DataDirectory.FullName);
        ServiceLocator.Instance.Register<IProjectManager>(projectManager);
        _logger.Info(null, "Initialized: ProjectManager");

        ChatSystem chatSystem = new ChatSystem(timeStorage);
        _logger.Info(null, "Initialized: ChatSystem");

        ITimeStorage auditStorage = new LiteDBTimeStorage();
        AuditLogger auditLogger = new AuditLogger(auditStorage);
        _logger.Info(null, "Initialized: AuditLogger");

        ITimeStorage tokenUsageStorage = new LiteDBTimeStorage();
        TokenUsageAuditManager tokenUsageAuditManager = new TokenUsageAuditManager(tokenUsageStorage);
        _logger.Info(null, "Initialized: TokenUsageAuditManager");

        // Initialize knowledge network system
        KnowledgeNetwork knowledgeNetwork = new KnowledgeNetwork();
        knowledgeNetwork.Initialize(configData.DataDirectory.FullName);
        ServiceLocator.Instance.Register<IKnowledgeNetwork>(knowledgeNetwork);
        _logger.Info(null, "Initialized: KnowledgeNetwork at {0}", configData.DataDirectory.FullName);

        GlobalACL globalAcl = new GlobalACL(storage);
        _logger.Info(null, "Initialized: GlobalACL");

        StreamCancellationManager streamCancellationManager = new StreamCancellationManager();
        _logger.Info(null, "Initialized: StreamCancellationManager");

        // Clean up duplicate config records (if any)
        try
        {
            LiteDBManager.DeduplicateConfig();
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to deduplicate config: {0}", ex.Message);
        }

        Router router = new Router();
        router.SetInitialized(configData.ConfigExists());
        IIMProvider imProvider = new WebUIProvider(router);
        imProvider.ExitRequested += (s, e) => RequestExit();

        DefaultPermissionCallback permissionCallback = new DefaultPermissionCallback(configData.DataDirectory.FullName);
        IMPermissionAskHandler askHandler = new IMPermissionAskHandler(imProvider);

        IMManager imManager = new IMManager(imProvider, chatSystem, MainLoop.BeingManager);
        _logger.Info(null, "Initialized: IMManager");

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
            .SetTokenUsageAuditManager(tokenUsageAuditManager)
            .SetStreamCancellationManager(streamCancellationManager);

        _host = builder.Build();

        await _host.StartAsync();
        _logger.Info(null, "CoreHost started");

        // Only create curator if it was previously initialized (CuratorGuid is set)
        if (configData.CuratorGuid != Guid.Empty)
        {
            SiliconBeingBase defaultBeing = beingFactory.CreateBeing(configData.CuratorGuid, "");
            _logger.Info(null, "Curator created: {0} ({1})", defaultBeing.Name, defaultBeing.Id);
            RegisterAndConfigureCurator(defaultBeing, configData, dynamicBeingLoader);
        }

        await StartWebServerAsync(configData, router, (WebUIProvider)imProvider, beingFactory, dynamicBeingLoader, localization);

        // Initialize tray status window after web server started
        TrayLocalizationBase trayLocalization = GetTrayLocalization(configData.Language);
        _trayWindow = new TrayStatusWindow(trayLocalization, configData.WebPort);
        _trayWindow.ExitRequested += (s, e) => RequestExit();
        _logger.Info(null, "Initialized: TrayStatusWindow");
        
        // Start Windows Forms message loop with ApplicationContext (form stays hidden)
        var trayContext = new TrayApplicationContext(_trayWindow);
        Application.Run(trayContext);

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
        _logger.Info(null, "Application shutting down...");

        // Update tray status
        TrayLocalizationBase shutdownTrayLocalization = GetTrayLocalization(Config.Instance.Data.Language);
        _trayWindow?.UpdateStatus(shutdownTrayLocalization.ShuttingDown);

        if (_webHost != null)
        {
            await _webHost.StopAsync();
            _webHost.Dispose();
        }

        if (_host != null)
        {
            await _host.StopAsync();
        }

        // Dispose tray window
        _trayWindow?.Dispose();

        // Shutdown LiteDB
        LiteDBManager.Shutdown();
        _logger.Info(null, "LiteDB shutdown complete");

        _shouldExit = true;
        _logger.Info(null, "Application shutdown complete");
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
            _logger.Info(null, "Curator created: {0} ({1})", curator.Name, curator.Id);

            // Write default soul content to storage
            string soulContent = localization.DefaultCuratorSoul;
            curator.SoulContent = soulContent;

            RegisterAndConfigureCurator(curator, configData, dynamicBeingLoader);
        });

        router.RegisterControllers();

        _webHost = new WebHost(configData.WebPort, router, configData.AllowIntranet);

        try
        {
            await _webHost.StartAsync();
            _logger.Info(null, "Web server started on port {0}", configData.WebPort);
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
            _logger.Error(null, "Failed to start web server: {0}", ex, ex.Message);
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
        
        // Japanese
        LocalizationManager.Instance.Register<JaJP>(Language.JaJP);
        
        // Korean
        LocalizationManager.Instance.Register<KoKR>(Language.KoKR);
        
        // Spanish
        LocalizationManager.Instance.Register<EsES>(Language.EsES);
        LocalizationManager.Instance.Register<EsMX>(Language.EsMX);
        
        // Czech
        LocalizationManager.Instance.Register<CsCZ>(Language.CsCZ);
        
        // German
        LocalizationManager.Instance.Register<DeDE>(Language.DeDE);
        LocalizationManager.Instance.Register<DeAT>(Language.DeAT);
        LocalizationManager.Instance.Register<DeCH>(Language.DeCH);
        LocalizationManager.Instance.Register<DeLU>(Language.DeLU);
        LocalizationManager.Instance.Register<DeLI>(Language.DeLI);
    }

    /// <summary>
    /// Determines the database file path
    /// </summary>
    private static string GetDatabasePath()
    {
        // Use siliconlife.db in the application base directory
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string dbPath = Path.Combine(baseDir, "siliconlife.db");
        
        if (!File.Exists(dbPath))
        {
            // Fallback to current directory
            dbPath = Path.Combine(Directory.GetCurrentDirectory(), "siliconlife.db");
        }
        
        return dbPath;
    }

    public static void RequestExit()
    {
        _shouldExit = true;
    }

    /// <summary>
    /// Gets the appropriate tray localization based on the configured language
    /// </summary>
    private static TrayLocalizationBase GetTrayLocalization(Language language)
    {
        return language switch
        {
            // Chinese variants
            Language.ZhCN => new TrayZhCN(),
            Language.ZhHK => new TrayZhHK(),
            Language.ZhTW => new TrayZhHK(), // Traditional Chinese
            Language.ZhSG => new TrayZhSG(),
            Language.ZhMO => new TrayZhHK(),
            Language.ZhMY => new TrayZhMY(),
            
            // English variants
            Language.EnUS => new TrayEnUS(),
            Language.EnGB => new TrayEnGB(),
            Language.EnCA => new TrayEnCA(),
            Language.EnAU => new TrayEnAU(),
            Language.EnIN => new TrayEnIN(),
            Language.EnSG => new TrayEnSG(),
            Language.EnZA => new TrayEnZA(),
            Language.EnIE => new TrayEnIE(),
            Language.EnNZ => new TrayEnNZ(),
            Language.EnMY => new TrayEnMY(),
            
            // Japanese
            Language.JaJP => new TrayJaJP(),
            
            // Korean
            Language.KoKR => new TrayKoKR(),
            
            // Spanish variants
            Language.EsES => new TrayEsES(),
            Language.EsMX => new TrayEsMX(),
            
            // Czech
            Language.CsCZ => new TrayCsCZ(),
            
            // German variants
            Language.DeDE => new TrayDeDE(),
            Language.DeAT => new TrayDeAT(),
            Language.DeCH => new TrayDeCH(),
            Language.DeLU => new TrayDeLU(),
            Language.DeLI => new TrayDeLI(),
            
            // Default to English
            _ => new TrayEnUS()
        };
    }

    /// <summary>
    /// Registers a curator being and configures its custom permission callback and code
    /// </summary>
    private static void RegisterAndConfigureCurator(SiliconBeingBase curator, DefaultConfigData configData, DynamicBeingLoader dynamicBeingLoader)
    {
        string beingDirectory = Path.Combine(configData.DataDirectory.FullName, "SiliconManager", curator.Id.ToString());

        // Register being FIRST before applying custom callbacks
        MainLoop.BeingManager.RegisterBeing(curator);
        _logger.Info(null, "Registered curator: {0} ({1})", curator.Name, curator.Id);

        if (DynamicBeingLoader.HasCustomPermissionCallback(beingDirectory))
        {
            try
            {
                CompilationResult permResult = dynamicBeingLoader.LoadPermissionCallback(curator.Id, beingDirectory);
                if (permResult.Success && permResult.CompiledType != null)
                {
                    MainLoop.BeingManager.ReplacePermissionCallback(curator.Id, permResult.CompiledType);
                    _logger.Info(null, "Loaded custom permission callback for curator {0}", curator.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.Warn(null, "Failed to load custom permission callback for curator {0}", ex, curator.Id);
            }
        }

        if (DynamicBeingLoader.HasCustomCode(beingDirectory))
        {
            try
            {
                Type? customType = dynamicBeingLoader.LoadBeingType(curator.Id, beingDirectory);
                if (customType != null)
                {
                    MainLoop.BeingManager.ReplaceBeing(curator.Id, customType);
                    _logger.Info(null, "Loaded custom code for curator {0}: {1}", curator.Id, customType.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.Warn(null, "Failed to load custom code for curator {0}", ex, curator.Id);
            }
        }
    }
}
