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
    public override string ConfigType { get; set; } = "Default";

    /// <summary>
    /// Gets or sets the data directory path for storing all application data
    /// </summary>
    public override string DataDirectory { get; set; } = "./data";

    /// <summary>
    /// Gets or sets the GUID of the curator (main administrator)
    /// </summary>
    public override Guid CuratorGuid { get; set; } = new Guid("73696c69-636f-6e00-0000-000000000000");

    /// <summary>
    /// Gets or sets the language setting for the application
    /// </summary>
    public override Language Language { get; set; } = Language.ZhCN;

    /// <summary>
    /// Gets or sets the timeout duration for each tick execution
    /// </summary>
    public override TimeSpan TickTimeout { get; set; } = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Gets or sets the maximum number of consecutive timeouts allowed before circuit breaker triggers
    /// </summary>
    public override int MaxTimeoutCount { get; set; } = 3;

    /// <summary>
    /// Gets or sets the watchdog timeout duration.
    /// </summary>
    public override TimeSpan WatchdogTimeout { get; set; } = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Gets or sets the Ollama API endpoint URL
    /// </summary>
    public string OllamaEndpoint { get; set; } = "http://localhost:11434";

    /// <summary>
    /// Gets or sets the default AI model to use
    /// </summary>
    public string DefaultModel { get; set; } = "qwen3.5:cloud";

    /// <summary>
    /// Gets or sets the static files directory path
    /// </summary>
    public string StaticFilesPath { get; set; } = "./wwwroot";

    /// <summary>
    /// Gets or sets the web server port
    /// </summary>
    public int WebPort { get; set; } = 8080;

    /// <summary>
    /// Gets or sets whether to allow intranet access (requires admin)
    /// </summary>
    public bool AllowIntranet { get; set; } = false;
}
