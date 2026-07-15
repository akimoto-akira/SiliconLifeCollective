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
/// Factory for creating Moonshot AI (Kimi) client instances.
/// </summary>
public class MoonshotClientFactory : IAIClientFactory, IAIClientFactoryHelp
{
    private const string DefaultEndpoint = "https://api.moonshot.cn/v1";

    private static readonly Dictionary<string, string> Models = new()
    {
        ["kimi-k2.6"] = "Kimi K2.6 (Flagship, 256K, Multimodal) - Recommended",
        ["kimi-k2.5"] = "Kimi K2.5 (Cost-effective Flagship, 256K)",
        ["kimi-k2.7-code"] = "Kimi K2.7 Code (Coding, 256K, Forced Thinking)",
        ["moonshot-v1-8k"] = "Moonshot V1 8K",
        ["moonshot-v1-32k"] = "Moonshot V1 32K",
        ["moonshot-v1-128k"] = "Moonshot V1 128K",
    };

    public IAIClient CreateClient(Dictionary<string, object> config)
    {
        string apiKey = config.TryGetValue("apiKey", out var ak)
            ? ak.ToString() ?? "" : "";

        string model = config.TryGetValue("model", out var m)
            ? m.ToString() ?? "kimi-k2.6" : "kimi-k2.6";

        string endpoint = config.TryGetValue("endpoint", out var ep)
            ? ep.ToString() ?? DefaultEndpoint : DefaultEndpoint;

        int? contextWindowTokens = null;
        if (config.TryGetValue("contextWindowTokens", out var cwt))
        {
            if (cwt is int intValue)
                contextWindowTokens = Math.Min(intValue, MoonshotClient.MaxContextWindowTokens);
            else if (int.TryParse(cwt.ToString(), out int parsedValue))
                contextWindowTokens = Math.Min(parsedValue, MoonshotClient.MaxContextWindowTokens);
        }

        return new MoonshotClient(apiKey, endpoint, model, contextWindowTokens);
    }

    public Dictionary<string, string> GetConfigKeysMetadata(Language language)
    {
        var localization = LocalizationManager.Instance.GetLocalization(language) as DefaultLocalizationBase;

        if (localization == null)
        {
            return new Dictionary<string, string>
            {
                ["apiKey"] = "API Key",
                ["model"] = "Model",
                ["endpoint"] = "Endpoint",
                ["contextWindowTokens"] = "Context Window Tokens"
            };
        }

        return new Dictionary<string, string>
        {
            ["apiKey"] = localization.GetConfigDisplayName("MoonshotApiKey", out _),
            ["model"] = localization.GetConfigDisplayName("MoonshotModel", out _),
            ["endpoint"] = localization.GetConfigDisplayName("MoonshotEndpoint", out _),
            ["contextWindowTokens"] = localization.GetConfigDisplayName("MoonshotContextWindowTokens", out _)
        };
    }

    public Dictionary<string, string>? GetConfigKeyOptions(
        string configKey, Dictionary<string, object> currentConfig, Language language)
    {
        if (configKey == "model")
            return Models;
        return null;
    }

    public string? GetHelpTopicId() => "moonshot-setup";
}
