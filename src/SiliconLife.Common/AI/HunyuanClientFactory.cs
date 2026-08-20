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
    /// <summary>
    /// Models currently offered on the TokenHub console (aligned with the
    /// official 模型列表, doc 1823/130051): the Hunyuan language models
    /// plus every third-party language model served through TokenHub.
    /// Legacy Hunyuan ids such as hunyuan-lite / hunyuan-turbos-latest /
    /// hunyuan-t1-latest were taken offline on 2026-06-10 and are no
    /// longer listed here.
    /// </summary>
    private static readonly string[] ModelIds =
    [
        "hy3", "hy3-preview", "hy-mt2-pro", "hy-mt2-plus", "hy-mt2-lite",
        "hunyuan-role-latest", "hy-role",
        "deepseek-v4-flash-202605", "deepseek-v4-pro-202606",
        "deepseek-v4-flash", "deepseek-v4-pro",
        "glm-5.3", "glm-5.2", "glm-5.1", "glm-5v-turbo", "glm-5-turbo", "glm-5",
        "kimi-k2.7-code-highspeed", "kimi-k3", "kimi-k2.7-code", "kimi-k2.6", "kimi-k2.5",
        "minimax-m3", "minimax-m2.7",
        "qwen3.5-flash", "qwen3.5-plus",
        "mimo-v2.5-pro",
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
                // Every model currently listed on TokenHub goes to the
                // TokenHub endpoint; anything else (user-configured legacy
                // ids) falls back to the legacy platform endpoint.
                "hy3" or "hy3-preview" or "hy-mt2-pro" or "hy-mt2-plus"
                    or "hy-mt2-lite" or "hunyuan-role-latest" or "hy-role"
                    or "deepseek-v4-flash-202605" or "deepseek-v4-pro-202606"
                    or "deepseek-v4-flash" or "deepseek-v4-pro"
                    or "glm-5.3" or "glm-5.2" or "glm-5.1" or "glm-5v-turbo"
                    or "glm-5-turbo" or "glm-5"
                    or "kimi-k2.7-code-highspeed" or "kimi-k3" or "kimi-k2.7-code"
                    or "kimi-k2.6" or "kimi-k2.5"
                    or "minimax-m3" or "minimax-m2.7"
                    or "qwen3.5-flash" or "qwen3.5-plus"
                    or "mimo-v2.5-pro"
                    => HunyuanClient.TokenHubEndpoint,
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
