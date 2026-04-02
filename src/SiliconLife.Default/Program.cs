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

namespace SiliconLife.Default;

/// <summary>
/// Main program entry point
/// </summary>
public class Program
{
    private static bool _shouldExit = false;

    /// <summary>
    /// Main method
    /// </summary>
    public static void Main(string[] args)
    {
        RegisterLocalizations();
        ConfigDataBaseConverter.RegisterConfigType("Default", typeof(DefaultConfigData));

        Config config = Config.Instance;
        config.Initialize(new DefaultConfigData());
        config.LoadOrCreateConfig();

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
        ServiceRegistry.Instance.ChatSystem = chatSystem;

        // Phase 6: Permission system setup
        ITimeStorage auditStorage = new FileSystemTimeStorage(
            Path.Combine(configData.DataDirectory, "audit"));
        AuditLogger auditLogger = new AuditLogger(auditStorage);
        ServiceRegistry.Instance.AuditLogger = auditLogger;

        GlobalACL globalAcl = new GlobalACL(storage);
        ServiceRegistry.Instance.GlobalAcl = globalAcl;

        IIMProvider imProvider = new ConsoleIMProvider(configData.UserGuid, configData.CuratorGuid);
        imProvider.ExitRequested += (s, e) => RequestExit();

        DefaultPermissionCallback permissionCallback = new DefaultPermissionCallback(configData.DataDirectory);
        IMPermissionAskHandler askHandler = new IMPermissionAskHandler(imProvider);

        SiliconBeingManager beingManager = MainLoop.BeingManager;

        IMManager imManager = new IMManager(imProvider, chatSystem, beingManager);
        ServiceRegistry.Instance.IMManager = imManager;

        DefaultSiliconBeingFactory beingFactory = new DefaultSiliconBeingFactory(
            aiClient,
            storage,
            configData.DataDirectory,
            configData.UserGuid,
            permissionCallback,
            askHandler);

        SiliconBeingBase defaultBeing = beingFactory.CreateBeing(configData.CuratorGuid, "Default");

        beingManager.RegisterBeing(defaultBeing);

        MainLoop.SetConfig(configData);
        MainLoop.Register(beingManager);
        MainLoop.Start();

        _ = imManager.StartAsync();

        while (!_shouldExit)
        {
            Thread.Sleep(100);
        }

        MainLoop.Stop();
    }

    /// <summary>
    /// Registers all available localizations
    /// </summary>
    private static void RegisterLocalizations()
    {
        LocalizationManager.Instance.Register<ZhCN>(Language.ZhCN);
        LocalizationManager.Instance.Register<EnUS>(Language.EnUS);
    }

    /// <summary>
    /// Signals the application to exit
    /// </summary>
    public static void RequestExit()
    {
        _shouldExit = true;
    }
}
