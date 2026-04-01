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
/// Default implementation of configuration data
/// </summary>
public class DefaultConfigData : ConfigDataBase
{
    /// <summary>
    /// Gets or sets the data directory path for storing all application data
    /// </summary>
    public override string DataDirectory { get; set; } = "./data";

    /// <summary>
    /// Gets or sets the GUID of the curator (main administrator)
    /// </summary>
    public override Guid CuratorGuid { get; set; } = Guid.Empty;

    /// <summary>
    /// Gets or sets the language setting for the application
    /// </summary>
    public override Language Language { get; set; } = Language.ZhCN;

    /// <summary>
    /// Gets or sets the Ollama API endpoint URL
    /// </summary>
    public string OllamaEndpoint { get; set; } = "http://localhost:11434";

    /// <summary>
    /// Gets or sets the default AI model to use
    /// </summary>
    public string DefaultModel { get; set; } = "qwen3:4b";
}
