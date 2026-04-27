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
using System.Text.Json.Serialization;

namespace SiliconLife.Default;

/// <summary>
/// Factory for creating Ollama client instances
/// </summary>
public class OllamaClientFactory : IAIClientFactory, IAIClientFactoryHelp
{
    /// <summary>
    /// Creates an Ollama client instance based on the provided configuration dictionary
    /// </summary>
    /// <param name="config">Configuration dictionary with keys like "endpoint", "model", etc.</param>
    /// <returns>An Ollama client instance</returns>
    public IAIClient CreateClient(Dictionary<string, object> config)
    {
        // Extract required fields
        string endpoint = config.TryGetValue("endpoint", out var ep) 
            ? ep.ToString() ?? "http://localhost:11434"
            : "http://localhost:11434";
        
        string model = config.TryGetValue("model", out var m) 
            ? m.ToString() ?? "llama3.2"
            : "llama3.2";
        
        return new OllamaClient(endpoint, model);
    }
    
    /// <summary>
    /// Gets the configuration keys metadata for Ollama client
    /// </summary>
    /// <param name="language">The language to use for localized display labels</param>
    public Dictionary<string, string> GetConfigKeysMetadata(Language language)
    {
        // Get localization instance for the specified language
        var localization = LocalizationManager.Instance.GetLocalization(language) as DefaultLocalizationBase;
        
        if (localization == null)
        {
            // Return English default values if localization is unavailable
            return new Dictionary<string, string>
            {
                ["endpoint"] = "Ollama Endpoint",
                ["model"] = "Default Model",
                ["temperature"] = "Temperature",
                ["maxTokens"] = "Max Tokens"
            };
        }
        
        // Return localized text
        return new Dictionary<string, string>
        {
            ["endpoint"] = localization.GetConfigDisplayName("OllamaEndpoint", out _),
            ["model"] = localization.GetConfigDisplayName("DefaultModel", out _),
            ["temperature"] = localization.GetConfigDisplayName("Temperature", out _),
            ["maxTokens"] = localization.GetConfigDisplayName("MaxTokens", out _)
        };
    }
    
    /// <summary>
    /// Gets the optional values for a specific configuration key.
    /// </summary>
    /// <param name="configKey">The configuration key to get options for</param>
    /// <param name="currentConfig">Current configuration dictionary for context-dependent options</param>
    /// <param name="language">The language to use for localized display text</param>
    /// <returns>
    /// A dictionary mapping programming values to localized display text, or null if the config key should use a text input.
    /// </returns>
    public Dictionary<string, string>? GetConfigKeyOptions(string configKey, Dictionary<string, object> currentConfig, Language language)
    {
        // Only "model" has predefined options, others use text input
        if (configKey == "model")
        {
            // Try to fetch available models from Ollama API
            string endpoint = currentConfig.TryGetValue("endpoint", out var ep) 
                ? ep.ToString() ?? "http://localhost:11434"
                : "http://localhost:11434";
            
            try
            {
                var models = FetchAvailableModels(endpoint);
                if (models != null && models.Count > 0)
                {
                    return models;
                }
            }
            catch
            {
                // If fetching fails, return null to use text input
                // Ollama returns 404 for models not loaded locally, so fallback list is meaningless
            }
        }
        
        // All other config keys use text input
        return null;
    }
    
    /// <summary>
    /// Fetches available models from Ollama API
    /// </summary>
    private static Dictionary<string, string>? FetchAvailableModels(string endpoint)
    {
        try
        {
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(3);
            
            var response = httpClient.GetAsync($"{endpoint.TrimEnd('/')}/api/tags").Result;
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            
            var json = response.Content.ReadAsStringAsync().Result;
            var doc = JsonDocument.Parse(json);
            
            if (!doc.RootElement.TryGetProperty("models", out var modelsElement))
            {
                return null;
            }
            
            var models = new Dictionary<string, string>();
            foreach (var model in modelsElement.EnumerateArray())
            {
                if (model.TryGetProperty("name", out var nameElement))
                {
                    var modelName = nameElement.GetString();
                    if (!string.IsNullOrEmpty(modelName))
                    {
                        // Use model name as both value and display text
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
    
    /// <summary>
    /// Gets the help documentation topic ID for Ollama client factory
    /// </summary>
    public string? GetHelpTopicId() => "ollama-setup";
    
}
