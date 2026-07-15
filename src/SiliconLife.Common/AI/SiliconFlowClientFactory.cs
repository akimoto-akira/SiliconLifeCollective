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
using System.Text.Json;

namespace SiliconLife.Common.AI;

/// <summary>
/// Factory for creating SiliconFlow (硅基流动 SiliconCloud) client instances.
/// SiliconFlow aggregates 100+ open-source models from multiple vendors.
/// </summary>
public class SiliconFlowClientFactory : IAIClientFactory, IAIClientFactoryHelp
{
    private const string DefaultEndpoint = "https://api.siliconflow.cn/v1";

    private static readonly string[] FallbackModelIds =
    [
        "deepseek-ai/DeepSeek-V3.2",
        "Qwen/Qwen3.5-9B",
        "Qwen/Qwen3.6-27B",
        "Qwen/Qwen3.6-35B-A3B",
        "zai-org/GLM-5.2",
        "Pro/zai-org/GLM-5.1",
        "deepseek-ai/DeepSeek-V4-Flash",
        "deepseek-ai/DeepSeek-V4-Pro",
        "Pro/moonshotai/Kimi-K2.6",
        "stepfun-ai/Step-3.5-Flash",
        "MiniMaxAI/MiniMax-M2.5",
    ];

    public IAIClient CreateClient(Dictionary<string, object> config)
    {
        string apiKey = config.TryGetValue("apiKey", out var ak)
            ? ak.ToString() ?? "" : "";

        string model = config.TryGetValue("model", out var m)
            ? m.ToString() ?? "deepseek-ai/DeepSeek-V3.2" : "deepseek-ai/DeepSeek-V3.2";

        string endpoint = config.TryGetValue("endpoint", out var ep)
            ? ep.ToString() ?? DefaultEndpoint : DefaultEndpoint;

        int? contextWindowTokens = null;
        if (config.TryGetValue("contextWindowTokens", out var cwt))
        {
            if (cwt is int intValue)
                contextWindowTokens = Math.Min(intValue, SiliconFlowClient.MaxContextWindowTokens);
            else if (int.TryParse(cwt.ToString(), out int parsedValue))
                contextWindowTokens = Math.Min(parsedValue, SiliconFlowClient.MaxContextWindowTokens);
        }

        return new SiliconFlowClient(apiKey, endpoint, model, contextWindowTokens);
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
            ["apiKey"] = localization.GetConfigDisplayName("SiliconFlowApiKey", out _),
            ["model"] = localization.GetConfigDisplayName("SiliconFlowModel", out _),
            ["endpoint"] = localization.GetConfigDisplayName("SiliconFlowEndpoint", out _),
            ["contextWindowTokens"] = localization.GetConfigDisplayName("SiliconFlowContextWindowTokens", out _)
        };
    }

    public Dictionary<string, string>? GetConfigKeyOptions(
        string configKey, Dictionary<string, object> currentConfig, Language language)
    {
        if (configKey == "model")
        {
            string? apiKey = currentConfig.TryGetValue("apiKey", out var ak) ? ak.ToString() : null;
            if (!string.IsNullOrEmpty(apiKey))
            {
                Dictionary<string, string>? models = FetchAvailableModels(apiKey);
                if (models != null && models.Count > 0)
                    return models;
            }
            return BuildFallbackModels(language);
        }
        return null;
    }

    private static Dictionary<string, string>? FetchAvailableModels(string apiKey)
    {
        try
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            var response = client.GetAsync($"{DefaultEndpoint}/models").Result;
            if (!response.IsSuccessStatusCode) return null;

            string json = response.Content.ReadAsStringAsync().Result;
            using var doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("data", out var dataElement)) return null;

            var models = new Dictionary<string, string>();
            foreach (JsonElement item in dataElement.EnumerateArray())
            {
                if (item.TryGetProperty("id", out var idElement))
                {
                    string modelName = idElement.GetString() ?? "";
                    if (!string.IsNullOrEmpty(modelName))
                        models[modelName] = modelName;
                }
            }
            return models.Count > 0 ? models : null;
        }
        catch { return null; }
    }

    private static Dictionary<string, string> BuildFallbackModels(Language language)
    {
        var localization = LocalizationManager.Instance.GetLocalization(language) as DefaultLocalizationBase;
        var models = new Dictionary<string, string>();
        foreach (string modelId in FallbackModelIds)
        {
            string localizedName = localization?.GetConfigDisplayName($"SiliconFlowModel_{modelId}", out _) ?? modelId;
            models[modelId] = $"{localizedName} ({modelId})";
        }
        return models;
    }

    public string? GetHelpTopicId() => "siliconflow-setup";
}
