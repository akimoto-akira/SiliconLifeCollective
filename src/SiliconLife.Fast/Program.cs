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
using SiliconLife.Fast.Knowledge;
using SiliconLife.Fast.Logging;
using SiliconLife.Fast.Tray;
using SiliconLife.App.Web;
using SiliconLife.App.IM;
using SiliconLife.App;
using SiliconLife.Fast.Config;
using System.Text;
using SiliconLife.Common;
using SiliconLife.Common.Security;
using SiliconLife.Common.SiliconBeing;
using SiliconLife.Common.WebView;

using SiliconLife.Common.Localization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;

namespace SiliconLife.Fast;

public class Program
{
    private static readonly ILogger _logger;
    private static bool _shouldExit = false;
    private static CoreHost? _host;
    private static WebHost? _webHost;
    private static TrayStatusWindow? _trayWindow;
    private static PluginLoader? _pluginLoader;

    static Program()
    {
        _logger = LogManager.Instance.GetLogger<Program>();
    }

    [STAThread]
    public static void Main(string[] args)
    {
        // Build and run Avalonia app
        // Initialization happens in App.OnFrameworkInitializationCompleted
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, shutdownMode: ShutdownMode.OnExplicitShutdown);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

    public static async Task StartAsync(string[] args)
    {
        Debug.RegisterCallback(msg => _logger.Warn(null, "Debug: {0}", msg));

        SpeedyPackRegistry.Initialize();
        _logger.Info(null, "Application starting...");

        RegisterLocalizations();
        ConfigDataBaseConverter.RegisterConfigType("Fast", typeof(DefaultConfigData));

        SiliconLife.Collective.Config config = SiliconLife.Collective.Config.Instance;
        config.Initialize(new DefaultConfigData());
        config.LoadConfig();

        DefaultConfigData configData = (DefaultConfigData)config.Data;
        LogManager.Instance.AddProvider(new SpeedyLoggerProvider());

        // Register TypeRegistry and ObjectFactory before loading plugins,
        // so plugins can register types/factories during OnLoad()
        ServiceLocator.Instance.Register<ITypeRegistry>(new TypeRegistry());
        ServiceLocator.Instance.Register<IObjectFactory>(new ObjectFactory());
        _logger.Info(null, "Registered: TypeRegistry, ObjectFactory");

        // Resolve plugin directories: if empty, default to ["plugins"] relative to base directory
        if (configData.PluginDirectories.Count == 0)
        {
            configData.PluginDirectories = new List<string> { "plugins" };
        }
        // Resolve relative paths to absolute paths based on application base directory
        for (int i = 0; i < configData.PluginDirectories.Count; i++)
        {
            string dir = configData.PluginDirectories[i];
            if (!Path.IsPathRooted(dir))
            {
                configData.PluginDirectories[i] = Path.Combine(AppContext.BaseDirectory, dir);
            }
        }

        // Load plugins from all configured directories using a single PluginLoader
        _pluginLoader = new PluginLoader(configData.PluginDirectories);
        _pluginLoader.LoadAll();
        ServiceLocator.Instance.Register(_pluginLoader);
        ServiceLocator.Instance.RegisterToolAssembly(typeof(SiliconLife.App.Web.Router).Assembly);

        configData.AIConfig.TryGetValue("endpoint", out var endpointValue);
        configData.AIConfig.TryGetValue("model", out var modelValue);
        _logger.Info(null, "Configuration loaded: endpoint={0}, model={1}",
            endpointValue?.ToString() ?? "N/A", modelValue?.ToString() ?? "N/A");

        DefaultLocalizationBase localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(configData.Language);

        IStorage storage = new SpeedyStorage();
        ITimeStorage timeStorage = new SpeedyTimeStorage();
        ServiceLocator.Instance.Register<Func<string, ITimeStorage>>(dir => new SpeedyTimeStorage(dir));
        ServiceLocator.Instance.Register<Func<string, IStorage>>(dir =>
        {
            // Extract relative path from full directory path for key prefix
            // e.g., "d:\data\SiliconManager\{GUID}" → "SiliconManager/{GUID}"
            string relativePath = dir;
            string currentDir = Environment.CurrentDirectory;
            if (relativePath.StartsWith(currentDir, StringComparison.OrdinalIgnoreCase))
            {
                relativePath = relativePath.Substring(currentDir.Length).TrimStart('\\', '/');
            }
            // Normalize path separators to forward slashes for consistent key mapping
            relativePath = relativePath.Replace('\\', '/').TrimEnd('/');
            return new SpeedyStorage(relativePath);
        });
        ServiceLocator.Instance.Register<Func<string, IWorkNoteStorage>>(dir => new SpeedyWorkNoteStorage());
        ServiceLocator.Instance.Register<Func<Guid, string>>(id => $"SiliconManager/{id}");
        ServiceLocator.Instance.Register<Func<SiliconBeingBase, object>>(being => new PlaywrightWebView(being));
        _logger.Info(null, "Registered: Storage Factories");

        TaskCenter.Instance.Initialize(storage);
        _logger.Info(null, "Initialized: TaskCenter");

        // Initialize project manager
        IProjectManager projectManager = new ProjectManager(storage, Environment.CurrentDirectory);
        ServiceLocator.Instance.Register<IProjectManager>(projectManager);
        _logger.Info(null, "Initialized: ProjectManager");

        // Initialize workflow engine
        var serviceProvider = new ServiceProvider();
        var workflowEngine = new WorkflowEngine(timeStorage, serviceProvider);

        // Register example workflows
        workflowEngine.RegisterTemplate(CodeReviewWorkflow.CreateTemplate());

        ((ProjectManager)projectManager).SetWorkflowEngine(workflowEngine);
        ServiceLocator.Instance.Register<WorkflowEngine>(workflowEngine);

        // Register workflow tick object (ticks every 60 seconds)
        new WorkflowTickObject(workflowEngine);

        _logger.Info(null, "Initialized: WorkflowEngine");

        ChatSystem chatSystem = new ChatSystem(timeStorage);
        _logger.Info(null, "Initialized: ChatSystem");

        ITimeStorage auditStorage = new SpeedyTimeStorage();
        AuditLogger auditLogger = new AuditLogger(auditStorage);
        _logger.Info(null, "Initialized: AuditLogger");

        ITimeStorage tokenUsageStorage = new SpeedyTimeStorage();
        TokenUsageAuditManager tokenUsageAuditManager = new TokenUsageAuditManager(tokenUsageStorage);
        _logger.Info(null, "Initialized: TokenUsageAuditManager");

        // Initialize knowledge network system
        KnowledgeNetwork knowledgeNetwork = new KnowledgeNetwork();
        knowledgeNetwork.Initialize(Environment.CurrentDirectory);
        ServiceLocator.Instance.Register<IKnowledgeNetwork>(knowledgeNetwork);
        _logger.Info(null, "Initialized: KnowledgeNetwork at {0}", Environment.CurrentDirectory);

        GlobalACL globalAcl = new GlobalACL(storage);
        _logger.Info(null, "Initialized: GlobalACL");

        StreamCancellationManager streamCancellationManager = new StreamCancellationManager();
        _logger.Info(null, "Initialized: StreamCancellationManager");

        Router router = new Router();
        router.SetInitialized(configData.ConfigExists());
        // 在深拷贝副本上解析 ${ENV_VAR} 占位符，configData.IMPlatforms 保持占位符原样，
        // 避免后续 SaveConfig 将解析后的明文密钥写入持久化存储
        List<IMPlatformConfig> resolvedPlatforms =
            SiliconLife.Common.IM.ConfigSecretResolver.CreateResolvedCopy(configData.IMPlatforms);
        IIMProvider imProvider = CreateIMProvider(resolvedPlatforms, router, out WebUIProvider webUiProvider);
        imProvider.ExitRequested += (s, e) => RequestExit();

        DefaultPermissionCallback permissionCallback = new DefaultPermissionCallback(string.Empty);
        IMPermissionAskHandler askHandler = new IMPermissionAskHandler(imProvider);

        IMManager imManager = new IMManager(imProvider, chatSystem, MainLoop.BeingManager);
        _logger.Info(null, "Initialized: IMManager");

        DefaultSiliconBeingFactory beingFactory = new DefaultSiliconBeingFactory(
            configData.AIConfig,
            storage,
            timeStorage,
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

        // Notify all plugins that the host is fully started
        _pluginLoader?.NotifyAllStarted();

        // Only create curator if it was previously initialized (CuratorGuid is set)
        if (configData.CuratorGuid != Guid.Empty)
        {
            SiliconBeingBase defaultBeing = beingFactory.CreateBeing(configData.CuratorGuid, "");
            _logger.Info(null, "Curator created: {0} ({1})", defaultBeing.Name, defaultBeing.Id);
            RegisterAndConfigureCurator(defaultBeing, configData, dynamicBeingLoader);
        }

        // Load all persisted non-curator beings from SiliconManager directory
        MainLoop.BeingManager.LoadPersistedBeings(beingFactory);

        await StartWebServerAsync(configData, router, webUiProvider, beingFactory, dynamicBeingLoader, localization);

        if (_shouldExit)
        {
            return;
        }

        TrayLocalizationBase trayLocalization = GetTrayLocalization(configData.Language);
        _trayWindow = new TrayStatusWindow(trayLocalization, configData.WebPort);
        _trayWindow.ExitRequested += (s, e) => RequestExit();
        
        // Platform-adaptive tray initialization
        bool isLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
            System.Runtime.InteropServices.OSPlatform.Linux);
        bool noTray = args.Contains("--no-tray");
        
        App.SetStatusWindow(_trayWindow);

        if (!isLinux && !noTray)
        {
            // Windows/macOS: Initialize tray icon
            string iconPath = Path.Combine(AppContext.BaseDirectory, "slc.ico");
            if (File.Exists(iconPath))
            {
                App.InitializeTray(_trayWindow, iconPath, configData.WebPort, trayLocalization);
                _logger.Info(null, "TrayIcon initialized: {0}", iconPath);
            }
            else
            {
                _logger.Warn(null, "Tray icon not found at {0}, tray icon will not be displayed", iconPath);
            }
        }
        else
        {
            // Linux: Skip tray icon (inconsistent support across desktop environments)
            // Show status window directly and auto-open browser
            _logger.Info(null, "Running on Linux - tray icon disabled. Web UI: http://localhost:{0}/", configData.WebPort);
            
            // Show status window on Linux (primary UI)
            _trayWindow.Show();
            
            if (!noTray)
            {
                // Auto-open browser on Linux for better UX
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = $"http://localhost:{configData.WebPort}/",
                        UseShellExecute = true
                    });
                    _logger.Info(null, "Auto-opened browser for Web UI");
                }
                catch (Exception ex)
                {
                    _logger.Warn(null, "Failed to auto-open browser: {0}", ex.Message);
                }
            }
        }
        
        Console.WriteLine($"[INFO] Status window created. Access web UI at: http://localhost:{configData.WebPort}/");
        if (isLinux)
        {
            Console.WriteLine($"[INFO] Linux detected: Status window shown. Close window to exit application.");
        }
        _logger.Info(null, "Initialized: TrayStatusWindow (Avalonia). Web UI: http://localhost:{0}/", configData.WebPort);
    }

    private static async Task ShutdownAsync()
    {
        _logger.Info(null, "Application shutting down...");

        // Update tray status
        TrayLocalizationBase shutdownTrayLocalization = GetTrayLocalization(SiliconLife.Collective.Config.Instance.Data.Language);
        _trayWindow?.UpdateStatus(shutdownTrayLocalization.ShuttingDown);

        // Stop web server first to release port
        if (_webHost != null)
        {
            _logger.Info(null, "Stopping web server...");
            await _webHost.StopAsync();
            _webHost.Dispose();
            _logger.Info(null, "Web server stopped");
        }

        // Stop core host
        if (_host != null)
        {
            _logger.Info(null, "Stopping core host...");
            await _host.StopAsync();
            _logger.Info(null, "Core host stopped");
        }

        if (_trayWindow != null)
        {
            await _trayWindow.CloseAndWaitAsync();
        }

        // Unload all plugins before disposing core resources
        if (_pluginLoader != null)
        {
            _pluginLoader.NotifyAllStopping();
            _pluginLoader.UnloadAll();
            _logger.Info(null, "Plugins unloaded");
        }

        // Flush and close the single SpeedyPack file handle
        SpeedyPackRegistry.Dispose();

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

        _webHost = new WebHost(configData.WebPort, router);

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

            var language = SiliconLife.Collective.Config.Instance?.Data?.Language ?? Language.ZhCN;
            var trayLoc = GetTrayLocalization(language);

            _logger.Error(null, "Web server startup failed: {0}", ex, ex.Message);
            RequestExit();
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

        // Polish
        LocalizationManager.Instance.Register<PlPL>(Language.PlPL);

        // German
        LocalizationManager.Instance.Register<DeDE>(Language.DeDE);
        LocalizationManager.Instance.Register<DeAT>(Language.DeAT);
        LocalizationManager.Instance.Register<DeCH>(Language.DeCH);
        LocalizationManager.Instance.Register<DeLU>(Language.DeLU);
        LocalizationManager.Instance.Register<DeLI>(Language.DeLI);

        // French
        LocalizationManager.Instance.Register<FrFR>(Language.FrFR);
        LocalizationManager.Instance.Register<FrCA>(Language.FrCA);
        LocalizationManager.Instance.Register<FrCH>(Language.FrCH);

        // Italian
        LocalizationManager.Instance.Register<ItIT>(Language.ItIT);

        // Portuguese variants
        LocalizationManager.Instance.Register<PtPT>(Language.PtPT);
        LocalizationManager.Instance.Register<PtBR>(Language.PtBR);

        // Russian
        LocalizationManager.Instance.Register<RuRU>(Language.RuRU);
    }

    public static void RequestExit()
    {
        _logger.Info(null, "Exit requested - initiating graceful shutdown");
        _shouldExit = true;

        // Shutdown Avalonia application
        Dispatcher.UIThread.Post(() =>
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            {
                lifetime.Shutdown();
            }
        });

        _logger.Info(null, "Exit request completed");
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

            // Polish
            Language.PlPL => new TrayPlPL(),

            // German variants
            Language.DeDE => new TrayDeDE(),
            Language.DeAT => new TrayDeAT(),
            Language.DeCH => new TrayDeCH(),
            Language.DeLU => new TrayDeLU(),
            Language.DeLI => new TrayDeLI(),

            // French variants
            Language.FrFR => new TrayFrFR(),
            Language.FrCA => new TrayFrCA(),
            Language.FrCH => new TrayFrCH(),

            // Italian
            Language.ItIT => new TrayItIT(),

            // Portuguese variants
            Language.PtPT => new TrayPtPT(),
            Language.PtBR => new TrayPtBR(),

            // Russian
            Language.RuRU => new TrayRuRU(),

            // Default to English
            _ => new TrayEnUS()
        };
    }

    /// <summary>
    /// Registers a curator being and configures its custom permission callback and code
    /// </summary>
    private static void RegisterAndConfigureCurator(SiliconBeingBase curator, DefaultConfigData configData, DynamicBeingLoader dynamicBeingLoader)
    {
        // Register being FIRST before applying custom callbacks
        MainLoop.BeingManager.RegisterBeing(curator);
        _logger.Info(null, "Registered curator: {0} ({1})", curator.Name, curator.Id);

        if (curator.Storage != null && DynamicBeingLoader.HasCustomPermissionCallback(curator.Storage))
        {
            try
            {
                CompilationResult permResult = dynamicBeingLoader.LoadPermissionCallback(curator.Id, curator.Storage);
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

        if (curator.Storage != null && DynamicBeingLoader.HasCustomCode(curator.Storage))
        {
            try
            {
                Type? customType = dynamicBeingLoader.LoadBeingType(curator.Id, curator.Storage);
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

    /// <summary>
    /// 根据 IM 平台配置创建 IM Provider 实例。
    /// 支持单平台（直接创建）和多平台聚合模式。
    /// 入参为占位符已解析的配置副本，与持久化的原始配置对象无共享引用。
    /// Web 前端（聊天页 SSE 等）依赖 WebUIProvider，通过 out 参数带出该实例引用；
    /// 若配置中不含 webui 平台，会自动补充一个 WebUIProvider 纳入装配。
    /// </summary>
    private static IIMProvider CreateIMProvider(List<IMPlatformConfig> platformConfigs, Router router, out WebUIProvider webUiProvider)
    {
        var enabledConfigs = platformConfigs.Where(c => c.Enabled).ToList();

        // 单平台且为 webui 的情况，直接返回 WebUIProvider（性能优化）
        if (enabledConfigs.Count == 1 && enabledConfigs[0].Platform == "webui")
        {
            _logger.Info(null, "Using single WebUIProvider");
            webUiProvider = new WebUIProvider(router);
            return webUiProvider;
        }

        // 多平台或非 webui 的情况，创建各平台 Provider 并聚合
        var providers = new List<IIMProvider>();
        foreach (var cfg in enabledConfigs)
        {
            IIMProvider? provider = CreatePlatformProvider(cfg, router);
            if (provider != null)
            {
                providers.Add(provider);
                _logger.Info(null, "Created provider for platform: {0}", cfg.Platform);
            }
            else
            {
                _logger.Warn(null, "Failed to create provider for platform: {0} (not implemented yet)", cfg.Platform);
            }
        }

        if (providers.Count == 0)
        {
            _logger.Warn(null, "No valid IM providers created, falling back to WebUIProvider");
        }

        // 配置中不含 webui 时自动补充一个 WebUIProvider，保证 Web 前端始终可用
        WebUIProvider? existingWebUi = providers.OfType<WebUIProvider>().FirstOrDefault();
        if (existingWebUi == null)
        {
            _logger.Info(null, "No webui platform configured, auto-adding WebUIProvider for web frontend");
            existingWebUi = new WebUIProvider(router);
            providers.Add(existingWebUi);
        }
        webUiProvider = existingWebUi;

        if (providers.Count == 1)
        {
            _logger.Info(null, "Using single provider: {0}", providers[0].GetType().Name);
            return providers[0];
        }

        _logger.Info(null, "Using AggregateIMProvider with {0} platform(s)", providers.Count);
        return new SiliconLife.Common.IM.AggregateIMProvider(providers);
    }

    /// <summary>
    /// 创建单个平台的 IM Provider 实例（平台工厂统一查询 IMProviderRegistry）。
    /// </summary>
    private static IIMProvider? CreatePlatformProvider(IMPlatformConfig cfg, Router router)
    {
        // webui 依赖 App 层 Router，无法在 Common 层注册工厂，保留特判
        if (cfg.Platform == "webui")
        {
            return new WebUIProvider(router);
        }

        var metadata = SiliconLife.Common.IM.IMProviderRegistry.Get(cfg.Platform);
        return metadata?.CreateProvider?.Invoke(cfg);
    }
}
