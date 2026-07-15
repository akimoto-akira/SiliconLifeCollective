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
/// Factory for creating DeepSeek client instances.
/// DeepSeek uses OpenAI-compatible API format with Bearer token authentication.
/// </summary>
public class DeepSeekClientFactory : IAIClientFactory, IAIClientFactoryHelp
{
    private const string DefaultEndpoint = "https://api.deepseek.com";

    private static readonly Dictionary<string, string> Models = new()
    {
        ["deepseek-v4-flash"] = "DeepSeek-V4-Flash (1M context, high speed)",
        ["deepseek-v4-pro"] = "DeepSeek-V4-Pro (1M context, flagship reasoning)",
    };

    public IAIClient CreateClient(Dictionary<string, object> config)
    {
        string apiKey = config.TryGetValue("apiKey", out var ak)
            ? ak.ToString() ?? ""
            : "";

        string model = config.TryGetValue("model", out var m)
            ? m.ToString() ?? "deepseek-v4-flash"
            : "deepseek-v4-flash";

        string endpoint = config.TryGetValue("endpoint", out var ep)
            ? ep.ToString() ?? DefaultEndpoint
            : DefaultEndpoint;

        bool thinkingEnabled = config.TryGetValue("thinkingEnabled", out var te)
            ? Convert.ToBoolean(te.ToString())
            : true;

        string reasoningEffort = config.TryGetValue("reasoningEffort", out var re)
            ? re.ToString() ?? "high"
            : "high";

        int? contextWindowTokens = null;
        if (config.TryGetValue("contextWindowTokens", out var cwt))
        {
            if (cwt is int intValue)
                contextWindowTokens = Math.Min(intValue, DeepSeekClient.MaxContextWindowTokens);
            else if (int.TryParse(cwt.ToString(), out int parsedValue))
                contextWindowTokens = Math.Min(parsedValue, DeepSeekClient.MaxContextWindowTokens);
        }

        return new DeepSeekClient(apiKey, endpoint, model, contextWindowTokens, thinkingEnabled, reasoningEffort);
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
            ["apiKey"] = localization.GetConfigDisplayName("DeepSeekApiKey", out _),
            ["model"] = localization.GetConfigDisplayName("DeepSeekModel", out _),
            ["endpoint"] = localization.GetConfigDisplayName("DeepSeekEndpoint", out _),
            ["contextWindowTokens"] = localization.GetConfigDisplayName("DeepSeekContextWindowTokens", out _)
        };
    }

    public Dictionary<string, string>? GetConfigKeyOptions(
        string configKey, Dictionary<string, object> currentConfig, Language language)
    {
        if (configKey == "model")
            return Models;
        return null;
    }

    public string? GetHelpTopicId() => "deepseek-setup";
}
