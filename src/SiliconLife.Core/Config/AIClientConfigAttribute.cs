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
/// Attribute for marking configuration properties that are specific to a particular AI client type.
/// Properties with this attribute will only be displayed when the corresponding AI client is selected.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class AIClientConfigAttribute : Attribute
{
    /// <summary>
    /// Gets the AI client type name that this configuration property belongs to.
    /// This should match the class name of the IAIClient implementation (e.g., "OllamaClient").
    /// </summary>
    public string ClientType { get; }

    /// <summary>
    /// Gets or sets the display order within the AI client configuration section (default: 0)
    /// </summary>
    public int Order { get; set; } = 0;

    /// <summary>
    /// Initializes a new instance of the AIClientConfigAttribute class
    /// </summary>
    /// <param name="clientType">The AI client type name (e.g., "OllamaClient")</param>
    public AIClientConfigAttribute(string clientType)
    {
        ClientType = clientType ?? throw new ArgumentNullException(nameof(clientType));
    }
}
