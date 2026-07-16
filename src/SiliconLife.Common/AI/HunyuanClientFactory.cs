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
/// Factory for creating Tencent Hunyuan (腾讯混元) client instances.
/// Supports TokenHub (recommended) and legacy platform endpoints.
/// </summary>
public class HunyuanClientFactory : IAIClientFactory, IAIClientFactoryHelp
{
    private static readonly string[] ModelIds =
    [
        "hy3", "hy3-preview", "hunyuan-lite", "hunyuan-turbos-latest",
        "hunyuan-t1-latest", "hunyuan-a13b", "hunyuan-functioncall",
    ];

    public IAIClient CreateClient(Dictionary<string, object> config)
    {
        string apiKey = config.TryGetValue("apiKey", out var ak)
            ? ak.ToString() ?? "" : "";

        string model = config.TryGetValue("model", out var m)
            ? m.ToString() ?? "hy3" : "hy3";

        // Auto-select endpoint based on model name if not explicitly configured
        string endpoint = config.TryGetValue("endpoint", out var ep) && !string.IsNullOrEmpty(ep.ToString())
            ? ep.ToString()!
            : model.ToLowerInvariant() switch
            {
                "hy3" or "hy3-preview" => HunyuanClient.TokenHubEndpoint,
                _ => HunyuanClient.LegacyEndpoint
            };

        bool thinkingEnabled = config.TryGetValue("thinkingEnabled", out var te)
            ? Convert.ToBoolean(te.ToString())
            : false;

        int? contextWindowTokens = null;
        if (config.TryGetValue("contextWindowTokens", out var cwt))
        {
            if (cwt is int intValue)
                contextWindowTokens = Math.Min(intValue, HunyuanClient.MaxContextWindowTokens);
            else if (int.TryParse(cwt.ToString(), out int parsedValue))
                contextWindowTokens = Math.Min(parsedValue, HunyuanClient.MaxContextWindowTokens);
        }

        return new HunyuanClient(apiKey, endpoint, model, contextWindowTokens, thinkingEnabled);
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
            ["apiKey"] = localization.GetConfigDisplayName("HunyuanApiKey", out _),
            ["model"] = localization.GetConfigDisplayName("HunyuanModel", out _),
            ["endpoint"] = localization.GetConfigDisplayName("HunyuanEndpoint", out _),
            ["contextWindowTokens"] = localization.GetConfigDisplayName("HunyuanContextWindowTokens", out _)
        };
    }

    public Dictionary<string, string>? GetConfigKeyOptions(
        string configKey, Dictionary<string, object> currentConfig, Language language)
    {
        if (configKey == "model")
        {
            var localization = LocalizationManager.Instance.GetLocalization(language) as DefaultLocalizationBase;
            var models = new Dictionary<string, string>();
            foreach (var modelId in ModelIds)
            {
                string displayName = localization?.GetConfigDisplayName($"HunyuanModel_{modelId}", out _) ?? modelId;
                models[modelId] = displayName;
            }
            return models;
        }
        return null;
    }

    public string? GetHelpTopicId() => "hunyuan-setup";
}
