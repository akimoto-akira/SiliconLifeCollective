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
/// Factory for creating Zhipu AI (智谱 GLM) client instances.
/// Zhipu uses OpenAI-compatible API format with Bearer token authentication.
/// </summary>
public class ZhipuClientFactory : IAIClientFactory, IAIClientFactoryHelp
{
    private const string DefaultEndpoint = "https://open.bigmodel.cn/api/paas/v4";

    private static readonly Dictionary<string, string> Models = new()
    {
        ["glm-4-flash"] = "GLM-4-Flash (Free, 128K) - Recommended for debugging",
        ["glm-4.7-flash"] = "GLM-4.7-Flash (Free, 200K)",
        ["glm-4-air"] = "GLM-4-Air (0.5 CNY/M, 128K)",
        ["glm-4-flashx"] = "GLM-4-FlashX (0.1 CNY/M, 128K)",
        ["glm-4-plus"] = "GLM-4-Plus (5 CNY/M, 128K)",
        ["glm-4-long"] = "GLM-4-Long (1 CNY/M, 1M context)",
        ["glm-4.6"] = "GLM-4.6 (Flagship, 200K)",
        ["glm-4.7"] = "GLM-4.7 (Flagship, 200K)",
        ["glm-5"] = "GLM-5 (Coding Agent, 128K)",
        ["glm-5.1"] = "GLM-5.1 (Long-range Agent, 128K)",
    };

    public IAIClient CreateClient(Dictionary<string, object> config)
    {
        string apiKey = config.TryGetValue("apiKey", out var ak)
            ? ak.ToString() ?? ""
            : "";

        string model = config.TryGetValue("model", out var m)
            ? m.ToString() ?? "glm-4-flash"
            : "glm-4-flash";

        string endpoint = config.TryGetValue("endpoint", out var ep)
            ? ep.ToString() ?? DefaultEndpoint
            : DefaultEndpoint;

        bool thinkingEnabled = config.TryGetValue("thinkingEnabled", out var te)
            ? Convert.ToBoolean(te.ToString())
            : false;

        int? contextWindowTokens = null;
        if (config.TryGetValue("contextWindowTokens", out var cwt))
        {
            if (cwt is int intValue)
                contextWindowTokens = Math.Min(intValue, ZhipuClient.MaxContextWindowTokens);
            else if (int.TryParse(cwt.ToString(), out int parsedValue))
                contextWindowTokens = Math.Min(parsedValue, ZhipuClient.MaxContextWindowTokens);
        }

        return new ZhipuClient(apiKey, endpoint, model, contextWindowTokens, thinkingEnabled);
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
            ["apiKey"] = localization.GetConfigDisplayName("ZhipuApiKey", out _),
            ["model"] = localization.GetConfigDisplayName("ZhipuModel", out _),
            ["endpoint"] = localization.GetConfigDisplayName("ZhipuEndpoint", out _),
            ["contextWindowTokens"] = localization.GetConfigDisplayName("ZhipuContextWindowTokens", out _)
        };
    }

    public Dictionary<string, string>? GetConfigKeyOptions(
        string configKey, Dictionary<string, object> currentConfig, Language language)
    {
        if (configKey == "model")
            return Models;
        return null;
    }

    public string? GetHelpTopicId() => "zhipu-setup";
}
