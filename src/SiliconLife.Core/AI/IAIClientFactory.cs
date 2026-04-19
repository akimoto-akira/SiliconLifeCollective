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

namespace SiliconLife.Collective;

/// <summary>
/// Factory interface for creating AI client instances
/// </summary>
public interface IAIClientFactory
{
    /// <summary>
    /// Creates an AI client instance based on the provided configuration dictionary
    /// </summary>
    /// <param name="config">Configuration dictionary with keys like "endpoint", "model", etc.</param>
    /// <returns>An AI client instance</returns>
    IAIClient CreateClient(Dictionary<string, object> config);
    
    /// <summary>
    /// Gets the configuration keys metadata for this AI client type.
    /// Used by Web UI to dynamically generate configuration form fields.
    /// </summary>
    /// <param name="language">The language to use for localized display labels</param>
    /// <returns>Dictionary mapping config key to display label</returns>
    /// <example>
    /// Returns: { "endpoint" => "Ollama Endpoint", "model" => "Default Model" }
    /// Config keys must not contain "." character.
    /// Display labels are already localized based on the provided language.
    /// </example>
    Dictionary<string, string> GetConfigKeysMetadata(Language language);
    
    /// <summary>
    /// Gets the optional values for a specific configuration key.
    /// Used by Web UI to determine whether to render a text input or dropdown select.
    /// </summary>
    /// <param name="configKey">The configuration key to get options for</param>
    /// <param name="currentConfig">Current configuration dictionary for context-dependent options</param>
    /// <param name="language">The language to use for localized display text</param>
    /// <returns>
    /// A dictionary mapping programming values to localized display text, or null if the config key should use a text input.
    /// When not null, the UI should render a dropdown select with these options.
    /// When null, the UI should render a text input field.
    /// </returns>
    /// <example>
    /// For "model" key, might return: { "gpt-3.5-turbo" => "GPT-3.5 Turbo", "gpt-4" => "GPT-4" }
    /// For "endpoint" key, might return: null (free text input)
    /// </example>
    Dictionary<string, string>? GetConfigKeyOptions(string configKey, Dictionary<string, object> currentConfig, Language language);
}
