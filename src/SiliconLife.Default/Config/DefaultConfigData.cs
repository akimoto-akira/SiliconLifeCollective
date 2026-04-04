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

    /// <summary>
    /// Gets or sets the web skin name
    /// </summary>
    public string WebSkin { get; set; } = null!;

    /// <summary>
    /// Gets or sets the nickname of the human user
    /// </summary>
    public override string UserNickname { get; set; } = "User";

    private string GetConfigFilePath()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string configPath = Path.Combine(baseDir, "config.json");

        if (File.Exists(configPath))
        {
            return configPath;
        }

        return Path.Combine(Directory.GetCurrentDirectory(), "config.json");
    }

    public override string GetConfigPath()
    {
        return GetConfigFilePath();
    }

    public override void LoadConfig()
    {
        string configPath = GetConfigFilePath();

        if (File.Exists(configPath))
        {
            try
            {
                string json = File.ReadAllText(configPath);
                DefaultConfigData? loadedData = JsonSerializer.Deserialize<DefaultConfigData>(json, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters = 
                    { 
                        new System.Text.Json.Serialization.JsonStringEnumConverter(),
                        new GuidConverter(),
                        new ConfigDataBaseConverter()
                    }
                });
                if (loadedData != null)
                {
                    ConfigType = loadedData.ConfigType;
                    DataDirectory = loadedData.DataDirectory;
                    CuratorGuid = loadedData.CuratorGuid;
                    Language = loadedData.Language;
                    TickTimeout = loadedData.TickTimeout;
                    MaxTimeoutCount = loadedData.MaxTimeoutCount;
                    WatchdogTimeout = loadedData.WatchdogTimeout;
                    OllamaEndpoint = loadedData.OllamaEndpoint;
                    DefaultModel = loadedData.DefaultModel;
                    StaticFilesPath = loadedData.StaticFilesPath;
                    WebPort = loadedData.WebPort;
                    AllowIntranet = loadedData.AllowIntranet;
                    WebSkin = loadedData.WebSkin;
                    UserNickname = loadedData.UserNickname;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Config Load Error: {ex.Message}");
            }
        }
    }

    public override void SaveConfig()
    {
        string json = JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = 
            { 
                new System.Text.Json.Serialization.JsonStringEnumConverter(),
                new GuidConverter(),
                new ConfigDataBaseConverter()
            }
        });
        string configPath = GetConfigFilePath();
        File.WriteAllText(configPath, json);
    }
}
