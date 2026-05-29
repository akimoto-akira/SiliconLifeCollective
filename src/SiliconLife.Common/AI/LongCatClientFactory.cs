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
/// Factory for creating LongCat (美团LongCat大模型) client instances.
/// LongCat uses OpenAI-compatible API format with API Key authentication.
/// </summary>
public class LongCatClientFactory : IAIClientFactory, IAIClientFactoryHelp
{
    public IAIClient CreateClient(Dictionary<string, object> config)
    {
        string endpoint = config.TryGetValue("endpoint", out var ep)
            ? ep.ToString() ?? "https://api.longcat.chat/openai"
            : "https://api.longcat.chat/openai";

        string apiKey = config.TryGetValue("apiKey", out var ak)
            ? ak.ToString() ?? throw new InvalidOperationException("LongCat API key is not configured")
            : throw new InvalidOperationException("LongCat API key is not configured");

        string model = config.TryGetValue("model", out var m)
            ? m.ToString() ?? "LongCat-Flash-Chat"
            : "LongCat-Flash-Chat";

        int? contextWindowTokens = null;
        if (config.TryGetValue("contextWindowTokens", out var cwt))
        {
            if (cwt is int intValue)
                contextWindowTokens = Math.Min(intValue, LongCatClient.MaxContextWindowTokens);
            else if (int.TryParse(cwt.ToString(), out int parsedValue))
                contextWindowTokens = Math.Min(parsedValue, LongCatClient.MaxContextWindowTokens);
        }

        return new LongCatClient(endpoint, apiKey, model, contextWindowTokens);
    }

    public Dictionary<string, string> GetConfigKeysMetadata(Language language)
    {
        var localization = LocalizationManager.Instance.GetLocalization(language) as DefaultLocalizationBase;

        if (localization == null)
        {
            return new Dictionary<string, string>
            {
                ["apiKey"] = "API Key",
                ["endpoint"] = "LongCat Endpoint",
                ["model"] = "Default Model",
                ["contextWindowTokens"] = "Context Window Tokens"
            };
        }

        return new Dictionary<string, string>
        {
            ["apiKey"] = localization.GetConfigDisplayName("LongCatApiKey", out _),
            ["endpoint"] = localization.GetConfigDisplayName("LongCatEndpoint", out _),
            ["model"] = localization.GetConfigDisplayName("LongCatModel", out _),
            ["contextWindowTokens"] = localization.GetConfigDisplayName("LongCatContextWindowTokens", out _)
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

            var response = httpClient.GetAsync($"{baseUrl}/v1/models").Result;
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

    public string? GetHelpTopicId() => "longcat-setup";
}
