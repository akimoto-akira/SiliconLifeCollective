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
using System.Text.Json;

namespace SiliconLife.Default;

/// <summary>
/// Factory for creating DashScope (Alibaba Cloud Bailian) client instances
/// </summary>
public class DashScopeClientFactory : IAIClientFactory
{
    /// <summary>
    /// Region-to-endpoint mapping for DashScope API
    /// </summary>
    private static readonly Dictionary<string, string> RegionEndpoints = new()
    {
        ["beijing"] = "https://dashscope.aliyuncs.com/compatible-mode/v1/chat/completions",
        ["virginia"] = "https://dashscope-us.aliyuncs.com/compatible-mode/v1/chat/completions",
        ["singapore"] = "https://dashscope-intl.aliyuncs.com/compatible-mode/v1/chat/completions",
        ["hongkong"] = "https://cn-hongkong.dashscope.aliyuncs.com/compatible-mode/v1/chat/completions",
        ["frankfurt"] = "https://{workspaceId}.eu-central-1.maas.aliyuncs.com/compatible-mode/v1/chat/completions",
    };

    /// <summary>
    /// Fallback model IDs when API fetch fails.
    /// Display names are resolved via localization at runtime.
    /// </summary>
    private static readonly string[] FallbackModelIds =
    [
        "qwen3-max",
        "qwen3.6-plus",
        "qwen3.6-flash",
        "qwen-max",
        "qwen-plus",
        "qwen-turbo",
        "qwen3-coder-plus",
        "qwq-plus",
        "deepseek-v3.2",
        "deepseek-r1",
        "glm-5.1",
        "kimi-k2.5",
        "llama-4-maverick",
    ];

    /// <summary>
    /// Creates a DashScope client instance based on the provided configuration dictionary
    /// </summary>
    /// <param name="config">Configuration dictionary with keys: "apiKey", "region", "model"</param>
    /// <returns>A DashScope client instance</returns>
    public IAIClient CreateClient(Dictionary<string, object> config)
    {
        string apiKey = config.TryGetValue("apiKey", out var ak)
            ? ak.ToString() ?? ""
            : "";

        string region = config.TryGetValue("region", out var r)
            ? r.ToString() ?? "beijing"
            : "beijing";

        string model = config.TryGetValue("model", out var m)
            ? m.ToString() ?? "qwen3.6-plus"
            : "qwen3.6-plus";

        string endpoint = RegionEndpoints.GetValueOrDefault(region, RegionEndpoints["beijing"]);

        return new DashScopeClient(apiKey, endpoint, model);
    }

    /// <summary>
    /// Gets the configuration keys metadata for DashScope client
    /// </summary>
    /// <param name="language">The language to use for localized display labels</param>
    public Dictionary<string, string> GetConfigKeysMetadata(Language language)
    {
        var localization = LocalizationManager.Instance.GetLocalization(language) as DefaultLocalizationBase;

        if (localization == null)
        {
            // Fallback to English defaults
            return new Dictionary<string, string>
            {
                ["apiKey"] = "API Key",
                ["region"] = "Region",
                ["model"] = "Model"
            };
        }

        return new Dictionary<string, string>
        {
            ["apiKey"] = localization.GetConfigDisplayName("DashScopeApiKey", out _),
            ["region"] = localization.GetConfigDisplayName("DashScopeRegion", out _),
            ["model"] = localization.GetConfigDisplayName("DashScopeModel", out _)
        };
    }

    /// <summary>
    /// Gets the optional values for a specific configuration key.
    /// Used by Web UI to determine whether to render a text input or dropdown select.
    /// </summary>
    /// <param name="configKey">The configuration key to get options for</param>
    /// <param name="currentConfig">Current configuration dictionary for context-dependent options</param>
    /// <param name="language">The language to use for localized display text</param>
    /// <returns>
    /// A dictionary mapping programming values to localized display text, or null if the config key should use a text input.
    /// </returns>
    public Dictionary<string, string>? GetConfigKeyOptions(
        string configKey, Dictionary<string, object> currentConfig, Language language)
    {
        var localization = LocalizationManager.Instance.GetLocalization(language) as DefaultLocalizationBase;

        if (configKey == "region")
        {
            return new Dictionary<string, string>
            {
                ["beijing"] = localization?.GetConfigDisplayName("DashScopeRegionBeijing", out _) ?? "China North 2 (Beijing)",
                ["virginia"] = localization?.GetConfigDisplayName("DashScopeRegionVirginia", out _) ?? "US (Virginia)",
                ["singapore"] = localization?.GetConfigDisplayName("DashScopeRegionSingapore", out _) ?? "Singapore",
                ["hongkong"] = localization?.GetConfigDisplayName("DashScopeRegionHongkong", out _) ?? "Hong Kong (China)",
                ["frankfurt"] = localization?.GetConfigDisplayName("DashScopeRegionFrankfurt", out _) ?? "Germany (Frankfurt)"
            };
        }

        if (configKey == "model")
        {
            // Prefer dynamic fetch via API (100+ models available)
            string? apiKey = currentConfig.TryGetValue("apiKey", out var ak) ? ak.ToString() : null;
            string region = currentConfig.TryGetValue("region", out var r) ? r.ToString() ?? "beijing" : "beijing";

            if (!string.IsNullOrEmpty(apiKey))
            {
                Dictionary<string, string>? models = FetchAvailableModelsAsync(apiKey, region, language).GetAwaiter().GetResult();
                if (models != null)
                    return models;
            }

            // Fallback to recommended models when API fetch fails
            return BuildFallbackModels(language);
        }

        // apiKey uses free text input
        return null;
    }

    /// <summary>
    /// Builds the fallback models dictionary with localized display names.
    /// Format: "Localized Name (modelId)" for better quota control visibility.
    /// </summary>
    private static Dictionary<string, string> BuildFallbackModels(Language language)
    {
        var localization = LocalizationManager.Instance.GetLocalization(language) as DefaultLocalizationBase;
        var models = new Dictionary<string, string>();

        foreach (string modelId in FallbackModelIds)
        {
            string localizedName = localization?.GetConfigDisplayName($"DashScopeModel_{modelId}", out _) ?? modelId;
            // Format: "Localized Name (modelId)" for quota control visibility
            models[modelId] = $"{localizedName} ({modelId})";
        }

        return models;
    }

    /// <summary>
    /// Fetches available models from DashScope API dynamically.
    /// Uses the OpenAI-compatible /models endpoint.
    /// </summary>
    private async Task<Dictionary<string, string>?> FetchAvailableModelsAsync(string apiKey, string region, Language language)
    {
        try
        {
            string baseUrl = RegionEndpoints.GetValueOrDefault(region, RegionEndpoints["beijing"])
                .Replace("/chat/completions", "");

            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            HttpResponseMessage response = await client.GetAsync($"{baseUrl}/models");
            if (!response.IsSuccessStatusCode)
                return null;

            string json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var localization = LocalizationManager.Instance.GetLocalization(language) as DefaultLocalizationBase;
            var models = new Dictionary<string, string>();
            foreach (JsonElement item in doc.RootElement.GetProperty("data").EnumerateArray())
            {
                string id = item.GetProperty("id").GetString() ?? "";
                
                // Try to get localized name, fallback to model id directly
                bool found = false;
                string localizedName = localization?.GetConfigDisplayName($"DashScopeModel_{id}", out found) ?? id;
                
                // Use localized name if found, otherwise use id directly
                models[id] = found ? $"{localizedName} ({id})" : id;
            }

            return models.Count > 0 ? models : null;
        }
        catch
        {
            return null; // Network error or timeout, fallback to recommended list
        }
    }
}
