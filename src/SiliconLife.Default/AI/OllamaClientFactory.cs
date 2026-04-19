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

namespace SiliconLife.Default;

/// <summary>
/// Factory for creating Ollama client instances
/// </summary>
public class OllamaClientFactory : IAIClientFactory
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
        // 获取指定语言的本地化实例
        var localization = LocalizationManager.Instance.GetLocalization(language) as DefaultLocalizationBase;
        
        if (localization == null)
        {
            // 如果无法获取本地化，返回英文默认值
            return new Dictionary<string, string>
            {
                ["endpoint"] = "Ollama Endpoint",
                ["model"] = "Default Model",
                ["temperature"] = "Temperature",
                ["maxTokens"] = "Max Tokens"
            };
        }
        
        // 返回已本地化的文本
        return new Dictionary<string, string>
        {
            ["endpoint"] = localization.GetConfigDisplayName("OllamaEndpoint"),
            ["model"] = localization.GetConfigDisplayName("DefaultModel"),
            ["temperature"] = localization.GetConfigDisplayName("Temperature"),
            ["maxTokens"] = localization.GetConfigDisplayName("MaxTokens")
        };
    }
}
