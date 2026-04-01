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
    private static SiliconBeingManager? _siliconBeingManager;
    private static DefaultSiliconBeing? _defaultBeing;
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

        IChatService chatService = new SimpleChatService();

        DefaultSiliconBeingFactory beingFactory = new DefaultSiliconBeingFactory(
            aiClient,
            storage,
            configData.DataDirectory);

        _siliconBeingManager = new SiliconBeingManager();

        _defaultBeing = (DefaultSiliconBeing)beingFactory.CreateBeing(configData.CuratorGuid, "Default");
        _defaultBeing.ChatService = chatService;

        _siliconBeingManager.RegisterBeing(_defaultBeing);

        ConsoleTickObject consoleTickObject = new ConsoleTickObject(_defaultBeing, localization);

        // Set config for MainLoop
        MainLoop.SetConfig(configData);

        // Register tick objects
        MainLoop.Register(_siliconBeingManager);
        MainLoop.Register(consoleTickObject);

        MainLoop.Start();

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
