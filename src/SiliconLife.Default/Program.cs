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
using SiliconLife.Default.Knowledge;
using SiliconLife.Default.Logging;
using SiliconLife.Default.Config;
using SiliconLife.App.Web;
using System.Text;
using SiliconLife.Common;
using SiliconLife.Common.Security;
using SiliconLife.Common.SiliconBeing;
using SiliconLife.Common.WebView;

using SiliconLife.Common.Localization;

namespace SiliconLife.Default;

public class Program
{
    private static readonly ILogger _logger;
    private static bool _shouldExit = false;
    private static CoreHost? _host;
    private static WebHost? _webHost;
    private static PluginLoader? _pluginLoader;

    static Program()
    {
        LogManager.Instance.AddProvider(new ConsoleLoggerProvider());
        _logger = LogManager.Instance.GetLogger<Program>();
    }

    public static async Task Main(string[] args)
    {
        Debug.RegisterCallback(Console.WriteLine);

        _logger.Info(null, "Application starting...");

        RegisterLocalizations();
        ConfigDataBaseConverter.RegisterConfigType("Default", typeof(DefaultConfigData));

        SiliconLife.Collective.Config config = SiliconLife.Collective.Config.Instance;
        config.Initialize(new DefaultConfigData());
        config.LoadConfig();

        DefaultConfigData configData = (DefaultConfigData)config.Data;
        LogManager.Instance.AddProvider(new FileSystemLoggerProvider(configData));

        // Load plugins after logging is initialized
        string pluginDir = Path.Combine(AppContext.BaseDirectory, "plugins");
        _pluginLoader = new PluginLoader(pluginDir);
        _pluginLoader.LoadAll();
        _logger.Info(null, "Plugins loaded from {0}", pluginDir);

        configData.AIConfig.TryGetValue("endpoint", out var endpointValue);
        configData.AIConfig.TryGetValue("model", out var modelValue);
        _logger.Info(null, "Configuration loaded: endpoint={0}, model={1}",
            endpointValue?.ToString() ?? "N/A", modelValue?.ToString() ?? "N/A");

        DefaultLocalizationBase localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(configData.Language);

        Console.WriteLine(localization.WelcomeMessage);
        Console.WriteLine();

        IStorage storage = new FileSystemStorage(configData.DataDirectory.FullName);
        ITimeStorage timeStorage = new FileSystemTimeStorage(
            Path.Combine(configData.DataDirectory.FullName, "chat"));

        // Register storage factories for SiliconBeing creation
        ServiceLocator.Instance.Register<Func<string, ITimeStorage>>(dir => new FileSystemTimeStorage(dir));
        ServiceLocator.Instance.Register<Func<string, IStorage>>(dir => new FileSystemStorage(dir));
        ServiceLocator.Instance.Register<Func<string, IWorkNoteStorage>>(dir => new FileSystemWorkNoteStorage(dir));
        ServiceLocator.Instance.Register<Func<SiliconBeingBase, object>>(being => new PlaywrightWebView((DefaultSiliconBeing)being));
        _logger.Info(null, "Registered: Storage Factories");

        // Initialize project manager
        IProjectManager projectManager = new ProjectManager(storage, configData.DataDirectory.FullName);
        ServiceLocator.Instance.Register<IProjectManager>(projectManager);
        _logger.Info(null, "Initialized: ProjectManager");

        // Initialize workflow engine
        var serviceProvider = new ServiceProvider();
        var workflowEngine = new WorkflowEngine(timeStorage, serviceProvider);
        workflowEngine.RegisterTemplate(CodeReviewWorkflow.CreateTemplate());
        ((ProjectManager)projectManager).SetWorkflowEngine(workflowEngine);
        ServiceLocator.Instance.Register<WorkflowEngine>(workflowEngine);
        new WorkflowTickObject(workflowEngine);
        _logger.Info(null, "Initialized: WorkflowEngine");

        ChatSystem chatSystem = new ChatSystem(timeStorage);
        _logger.Info(null, "Initialized: ChatSystem");

        ITimeStorage auditStorage = new FileSystemTimeStorage(
            Path.Combine(configData.DataDirectory.FullName, "audit"));
        AuditLogger auditLogger = new AuditLogger(auditStorage);
        _logger.Info(null, "Initialized: AuditLogger");

        ITimeStorage tokenUsageStorage = new FileSystemTimeStorage(
            Path.Combine(configData.DataDirectory.FullName, "token-usage"));
        TokenUsageAuditManager tokenUsageAuditManager = new TokenUsageAuditManager(tokenUsageStorage);
        _logger.Info(null, "Initialized: TokenUsageAuditManager");

        // Initialize knowledge network system
        string knowledgeStoragePath = Path.Combine(configData.DataDirectory.FullName, "knowledge");
        KnowledgeNetwork knowledgeNetwork = new KnowledgeNetwork();
        knowledgeNetwork.Initialize(knowledgeStoragePath);
        ServiceLocator.Instance.Register<IKnowledgeNetwork>(knowledgeNetwork);
        _logger.Info(null, "Initialized: KnowledgeNetwork at {0}", knowledgeStoragePath);

        GlobalACL globalAcl = new GlobalACL(storage);
        _logger.Info(null, "Initialized: GlobalACL");

        StreamCancellationManager streamCancellationManager = new StreamCancellationManager();
        _logger.Info(null, "Initialized: StreamCancellationManager");

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

        // Notify all plugins that the host is fully started
        _pluginLoader?.NotifyAllStarted();

        // Only create curator if it was previously initialized (CuratorGuid is set)
        if (configData.CuratorGuid != Guid.Empty)
        {
            SiliconBeingBase defaultBeing = beingFactory.CreateBeing(configData.CuratorGuid, "");
            _logger.Info(null, "Curator created: {0} ({1})", defaultBeing.Name, defaultBeing.Id);
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
        _logger.Info(null, "Application shutting down...");

        if (_webHost != null)
        {
            await _webHost.StopAsync();
            _webHost.Dispose();
        }

        if (_host != null)
        {
            await _host.StopAsync();
        }

        // Unload all plugins before exit
        _pluginLoader?.NotifyAllStopping();
        _pluginLoader?.UnloadAll();
        _logger.Info(null, "Plugins unloaded");

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
        
        // French
        LocalizationManager.Instance.Register<FrFR>(Language.FrFR);
        LocalizationManager.Instance.Register<FrCA>(Language.FrCA);
        LocalizationManager.Instance.Register<FrCH>(Language.FrCH);
        
        // Italian
        LocalizationManager.Instance.Register<ItIT>(Language.ItIT);
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
}
