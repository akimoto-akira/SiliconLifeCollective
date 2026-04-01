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
    /// <summary>
    /// Main method
    /// </summary>
    public static async Task Main(string[] args)
    {
        RegisterLocalizations();

        Config config = Config.Instance;
        DefaultConfigData configData = (DefaultConfigData)config.Data;
        DefaultLocalizationBase localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(configData.Language);

        Console.WriteLine(localization.WelcomeMessage);
        Console.WriteLine();

        IAIClientFactory aiClientFactory = new OllamaClientFactory();
        IAIClient aiClient = aiClientFactory.CreateClient(configData.OllamaEndpoint, configData.DefaultModel);

        ConsoleTickObject consoleTickObject = new ConsoleTickObject(aiClient, localization);
        TestTickObject testTickObject = new TestTickObject();
        
        MainLoop.Start();

        while (!consoleTickObject.ShouldExit)
        {
            await Task.Delay(100);
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
}
