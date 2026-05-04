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
/// Factory for creating Volcengine Ark client instances.
/// Volcengine Ark uses OpenAI-compatible API format with Bearer token authentication.
/// Model parameter accepts inference endpoint IDs (e.g., "ep-20241212123456-abcde").
/// </summary>
public class VolcengineArkClientFactory : IAIClientFactory, IAIClientFactoryHelp
{
    /// <summary>
    /// Default Volcengine Ark API endpoint
    /// </summary>
    private const string DefaultEndpoint = "https://ark.cn-beijing.volces.com/api/v3/chat/completions";

    /// <summary>
    /// Creates a Volcengine Ark client instance based on the provided configuration dictionary
    /// </summary>
    /// <param name="config">Configuration dictionary with keys: "apiKey", "endpointId"</param>
    /// <returns>A Volcengine Ark client instance</returns>
    public IAIClient CreateClient(Dictionary<string, object> config)
    {
        string apiKey = config.TryGetValue("apiKey", out var ak)
            ? ak.ToString() ?? ""
            : "";

        string endpointId = config.TryGetValue("endpointId", out var m)
            ? m.ToString() ?? ""
            : "";

        return new VolcengineArkClient(apiKey, DefaultEndpoint, endpointId);
    }

    /// <summary>
    /// Gets the configuration keys metadata for Volcengine Ark client
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
                ["endpointId"] = "Inference Endpoint ID",
            };
        }

        return new Dictionary<string, string>
        {
            ["apiKey"] = localization.GetConfigDisplayName("VolcengineArkApiKey", out _),
            ["endpointId"] = localization.GetConfigDisplayName("VolcengineArkEndpointId", out _),
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
        if (configKey == "endpointId")
        {
            // Try to fetch available endpoints from Volcengine Ark API
            string? apiKey = currentConfig.TryGetValue("apiKey", out var ak) ? ak.ToString() : null;

            if (!string.IsNullOrEmpty(apiKey))
            {
                Dictionary<string, string>? endpoints = FetchAvailableEndpointsAsync(apiKey, language).GetAwaiter().GetResult();
                if (endpoints != null)
                    return endpoints;
            }
        }

        // apiKey uses free text input
        return null;
    }

    /// <summary>
    /// Fetches available inference endpoints from Volcengine Ark API dynamically.
    /// Uses the /api/v3/endpoints endpoint.
    /// </summary>
    private async Task<Dictionary<string, string>?> FetchAvailableEndpointsAsync(string apiKey, Language language)
    {
        try
        {
            string baseUrl = DefaultEndpoint.Replace("/chat/completions", "");

            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            HttpResponseMessage response = await client.GetAsync($"{baseUrl}/endpoints");
            if (!response.IsSuccessStatusCode)
                return null;

            string json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var endpoints = new Dictionary<string, string>();
            if (doc.RootElement.TryGetProperty("data", out var data))
            {
                foreach (JsonElement item in data.EnumerateArray())
                {
                    string id = item.TryGetProperty("id", out var idProp) ? idProp.GetString() ?? "" : "";
                    string name = item.TryGetProperty("name", out var nameProp) ? nameProp.GetString() ?? "" : "";
                    string modelName = item.TryGetProperty("model", out var modelProp) ? modelProp.GetString() ?? "" : "";

                    if (!string.IsNullOrEmpty(id))
                    {
                        // Format: "ModelName (endpointId)" or just "endpointId"
                        string displayName = !string.IsNullOrEmpty(name)
                            ? $"{name} ({id})"
                            : id;
                        endpoints[id] = displayName;
                    }
                }
            }

            return endpoints.Count > 0 ? endpoints : null;
        }
        catch
        {
            return null; // Network error or timeout, fallback to text input
        }
    }

    /// <summary>
    /// Gets the help documentation topic ID for Volcengine Ark client factory
    /// </summary>
    public string? GetHelpTopicId() => "volcengine-ark";
}
