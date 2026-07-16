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
/// Factory for creating MiniMax client instances.
/// </summary>
public class MiniMaxClientFactory : IAIClientFactory, IAIClientFactoryHelp
{
    private const string DomesticEndpoint = "https://api.minimaxi.com/v1";
    private const string InternationalEndpoint = "https://api.minimax.io/v1";

    private static readonly string[] ModelIds =
    [
        "MiniMax-M3", "MiniMax-M2.7", "MiniMax-M2.7-highspeed", "MiniMax-M2.5", "MiniMax-M2",
    ];

    public IAIClient CreateClient(Dictionary<string, object> config)
    {
        string apiKey = config.TryGetValue("apiKey", out var ak)
            ? ak.ToString() ?? "" : "";

        string model = config.TryGetValue("model", out var m)
            ? m.ToString() ?? "MiniMax-M3" : "MiniMax-M3";

        // Support domestic/international endpoint selection
        string endpoint = config.TryGetValue("endpoint", out var ep) && ep.ToString() == "international"
            ? InternationalEndpoint
            : DomesticEndpoint;

        int? contextWindowTokens = null;
        if (config.TryGetValue("contextWindowTokens", out var cwt))
        {
            if (cwt is int intValue)
                contextWindowTokens = Math.Min(intValue, MiniMaxClient.MaxContextWindowTokens);
            else if (int.TryParse(cwt.ToString(), out int parsedValue))
                contextWindowTokens = Math.Min(parsedValue, MiniMaxClient.MaxContextWindowTokens);
        }

        return new MiniMaxClient(apiKey, endpoint, model, contextWindowTokens);
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
                ["endpoint"] = "Platform (domestic/international)",
                ["contextWindowTokens"] = "Context Window Tokens"
            };
        }

        return new Dictionary<string, string>
        {
            ["apiKey"] = localization.GetConfigDisplayName("MiniMaxApiKey", out _),
            ["model"] = localization.GetConfigDisplayName("MiniMaxModel", out _),
            ["endpoint"] = localization.GetConfigDisplayName("MiniMaxEndpoint", out _),
            ["contextWindowTokens"] = localization.GetConfigDisplayName("MiniMaxContextWindowTokens", out _)
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
                string displayName = localization?.GetConfigDisplayName($"MiniMaxModel_{modelId}", out _) ?? modelId;
                models[modelId] = displayName;
            }
            return models;
        }
        if (configKey == "endpoint")
            return new Dictionary<string, string>
            {
                ["domestic"] = "Domestic (api.minimaxi.com)",
                ["international"] = "International (api.minimax.io)"
            };
        return null;
    }

    public string? GetHelpTopicId() => "minimax-setup";
}
