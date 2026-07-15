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
using SiliconLife.Common.Localization;

namespace SiliconLife.Common.AI;

/// <summary>
/// Factory for creating Baidu ERNIE (文心一言) client instances.
/// Uses Qianfan v2 OpenAI-compatible API with Bearer token authentication.
/// </summary>
public class ErnieClientFactory : IAIClientFactory, IAIClientFactoryHelp
{
    private const string DefaultEndpoint = "https://qianfan.baidubce.com/v2";

    private static readonly Dictionary<string, string> Models = new()
    {
        ["ernie-5.1"] = "ERNIE 5.1 (Flagship, 128K, Multimodal)",
        ["ernie-4.0-turbo-8k"] = "ERNIE 4.0 Turbo (8K, Cost-effective)",
        ["ernie-4.0-8k"] = "ERNIE 4.0 (8K)",
        ["ernie-3.5-8k"] = "ERNIE 3.5 (8K, Economy)",
        ["ernie-3.5-128k"] = "ERNIE 3.5 (128K, Long context)",
        ["ernie-speed-128k"] = "ERNIE Speed (128K, Free)",
        ["ernie-speed-8k"] = "ERNIE Speed (8K, Free) - Recommended for debugging",
        ["ernie-tiny-8k"] = "ERNIE Tiny (8K, Free)",
    };

    public IAIClient CreateClient(Dictionary<string, object> config)
    {
        string apiKey = config.TryGetValue("apiKey", out var ak)
            ? ak.ToString() ?? "" : "";

        string model = config.TryGetValue("model", out var m)
            ? m.ToString() ?? "ernie-5.1" : "ernie-5.1";

        string endpoint = config.TryGetValue("endpoint", out var ep)
            ? ep.ToString() ?? DefaultEndpoint : DefaultEndpoint;

        int? contextWindowTokens = null;
        if (config.TryGetValue("contextWindowTokens", out var cwt))
        {
            if (cwt is int intValue)
                contextWindowTokens = Math.Min(intValue, ErnieClient.MaxContextWindowTokens);
            else if (int.TryParse(cwt.ToString(), out int parsedValue))
                contextWindowTokens = Math.Min(parsedValue, ErnieClient.MaxContextWindowTokens);
        }

        return new ErnieClient(apiKey, endpoint, model, contextWindowTokens);
    }

    public Dictionary<string, string> GetConfigKeysMetadata(Language language)
    {
        var localization = LocalizationManager.Instance.GetLocalization(language) as DefaultLocalizationBase;

        if (localization == null)
        {
            return new Dictionary<string, string>
            {
                ["apiKey"] = "API Key (bce-v3/...)",
                ["model"] = "Model",
                ["endpoint"] = "Endpoint",
                ["contextWindowTokens"] = "Context Window Tokens"
            };
        }

        return new Dictionary<string, string>
        {
            ["apiKey"] = localization.GetConfigDisplayName("ErnieApiKey", out _),
            ["model"] = localization.GetConfigDisplayName("ErnieModel", out _),
            ["endpoint"] = localization.GetConfigDisplayName("ErnieEndpoint", out _),
            ["contextWindowTokens"] = localization.GetConfigDisplayName("ErnieContextWindowTokens", out _)
        };
    }

    public Dictionary<string, string>? GetConfigKeyOptions(
        string configKey, Dictionary<string, object> currentConfig, Language language)
    {
        if (configKey == "model")
            return Models;
        return null;
    }

    public string? GetHelpTopicId() => "ernie-setup";
}
