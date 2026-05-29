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
/// Factory for creating Qiniu AI (七牛云AI大模型推理服务) client instances.
/// Qiniu AI uses OpenAI-compatible API format with API Key authentication.
/// </summary>
public class QiniuAIClientFactory : IAIClientFactory, IAIClientFactoryHelp
{
    public IAIClient CreateClient(Dictionary<string, object> config)
    {
        string endpoint = config.TryGetValue("endpoint", out var ep)
            ? ep.ToString() ?? "https://api.qnaigc.com/v1"
            : "https://api.qnaigc.com/v1";

        string apiKey = config.TryGetValue("apiKey", out var ak)
            ? ak.ToString() ?? throw new InvalidOperationException("Qiniu AI API key is not configured")
            : throw new InvalidOperationException("Qiniu AI API key is not configured");

        string model = config.TryGetValue("model", out var m)
            ? m.ToString() ?? "deepseek-v3"
            : "deepseek-v3";

        int? contextWindowTokens = null;
        if (config.TryGetValue("contextWindowTokens", out var cwt))
        {
            if (cwt is int intValue)
                contextWindowTokens = Math.Min(intValue, QiniuAIClient.MaxContextWindowTokens);
            else if (int.TryParse(cwt.ToString(), out int parsedValue))
                contextWindowTokens = Math.Min(parsedValue, QiniuAIClient.MaxContextWindowTokens);
        }

        return new QiniuAIClient(endpoint, apiKey, model, contextWindowTokens);
    }

    public Dictionary<string, string> GetConfigKeysMetadata(Language language)
    {
        var localization = LocalizationManager.Instance.GetLocalization(language) as DefaultLocalizationBase;

        if (localization == null)
        {
            return new Dictionary<string, string>
            {
                ["apiKey"] = "API Key",
                ["endpoint"] = "Qiniu AI Endpoint",
                ["model"] = "Default Model",
                ["contextWindowTokens"] = "Context Window Tokens"
            };
        }

        return new Dictionary<string, string>
        {
            ["apiKey"] = localization.GetConfigDisplayName("QiniuAIApiKey", out _),
            ["endpoint"] = localization.GetConfigDisplayName("QiniuAIEndpoint", out _),
            ["model"] = localization.GetConfigDisplayName("QiniuAIModel", out _),
            ["contextWindowTokens"] = localization.GetConfigDisplayName("QiniuAIContextWindowTokens", out _)
        };
    }

    public Dictionary<string, string>? GetConfigKeyOptions(
        string configKey, Dictionary<string, object> currentConfig, Language language)
    {
        if (configKey == "model")
        {
            if (currentConfig.TryGetValue("endpoint", out var ep) && ep.ToString() is string endpoint)
            {
                try
                {
                    Dictionary<string, string>? models = FetchAvailableModels(endpoint, currentConfig);
                    if (models != null && models.Count > 0)
                        return models;
                }
                catch { }
            }
        }

        return null;
    }

    private static Dictionary<string, string>? FetchAvailableModels(
        string chatEndpoint, Dictionary<string, object> currentConfig)
    {
        try
        {
            string baseUrl = chatEndpoint;

            using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(3) };

            // Add API Key authentication if available
            if (currentConfig.TryGetValue("apiKey", out var ak) && ak.ToString() is string apiKey && !string.IsNullOrEmpty(apiKey))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            }

            var response = httpClient.GetAsync($"{baseUrl}/models").Result;
            if (!response.IsSuccessStatusCode)
                return null;

            string json = response.Content.ReadAsStringAsync().Result;
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("data", out var dataElement))
                return null;

            var models = new Dictionary<string, string>();
            foreach (JsonElement item in dataElement.EnumerateArray())
            {
                if (item.TryGetProperty("id", out var idElement))
                {
                    string modelName = idElement.GetString() ?? "";
                    if (!string.IsNullOrEmpty(modelName))
                    {
                        models[modelName] = modelName;
                    }
                }
            }

            return models.Count > 0 ? models : null;
        }
        catch
        {
            return null;
        }
    }

    public string? GetHelpTopicId() => "qiniu-ai-setup";
}
