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
    [ConfigIgnore("系统内部使用，用于多态反序列化")]
    public override string ConfigType { get; set; } = "Default";

    /// <summary>
    /// Gets or sets the data directory for storing all application data
    /// </summary>
    [ConfigGroup("Basic", Order = 2, DisplayNameKey = "DataDirectory", DescriptionKey = "DataDirectory")]
    public override DirectoryInfo DataDirectory { get; set; } = new DirectoryInfo("./data");

    /// <summary>
    /// Gets or sets the GUID of the curator (main administrator)
    /// </summary>
    [ConfigIgnore("系统内部标识，不建议手动修改")]
    public override Guid CuratorGuid { get; set; }

    /// <summary>
    /// Gets or sets the language setting for the application
    /// </summary>
    [ConfigGroup("Basic", Order = 4, DisplayNameKey = "Language", DescriptionKey = "Language")]
    public override Language Language { get; set; } = Language.ZhCN;

    /// <summary>
    /// Gets or sets the timeout duration for each tick execution
    /// </summary>
    [ConfigGroup("Runtime", Order = 1, DisplayNameKey = "TickTimeout", DescriptionKey = "TickTimeout")]
    public override TimeSpan TickTimeout { get; set; } = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Gets or sets the maximum number of consecutive timeouts allowed before circuit breaker triggers
    /// </summary>
    [ConfigGroup("Runtime", Order = 2, DisplayNameKey = "MaxTimeoutCount", DescriptionKey = "MaxTimeoutCount")]
    public override int MaxTimeoutCount { get; set; } = 3;

    /// <summary>
    /// Gets or sets the watchdog timeout duration.
    /// </summary>
    [ConfigGroup("Runtime", Order = 3, DisplayNameKey = "WatchdogTimeout", DescriptionKey = "WatchdogTimeout")]
    public override TimeSpan WatchdogTimeout { get; set; } = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Gets or sets the global minimum log level.
    /// </summary>
    [ConfigGroup("Runtime", Order = 4, DisplayNameKey = "MinLogLevel", DescriptionKey = "MinLogLevel")]
    public override LogLevel MinimumLogLevel { get; set; } = LogLevel.Trace;

    /// <summary>
    /// Gets or sets the AI client type to use
    /// </summary>
    [ConfigGroup("AI", Order = 0, DisplayNameKey = "AIClientType", DescriptionKey = "AIClientType")]
    public string AIClientType { get; set; } = "OllamaClient";

    /// <summary>
    /// Gets or sets the Ollama API endpoint URL
    /// </summary>
    [AIClientConfig("OllamaClient", Order = 1)]
    [ConfigGroup("AI", Order = 1, DisplayNameKey = "OllamaEndpoint", DescriptionKey = "OllamaEndpoint")]
    public string OllamaEndpoint { get; set; } = "http://localhost:11434";

    /// <summary>
    /// Gets or sets the default AI model to use
    /// </summary>
    [AIClientConfig("OllamaClient", Order = 2)]
    [ConfigGroup("AI", Order = 2, DisplayNameKey = "DefaultModel", DescriptionKey = "DefaultModel")]
    public string DefaultModel { get; set; } = "qwen3.5:cloud";

    /// <summary>
    /// Gets or sets the web server port
    /// </summary>
    [ConfigGroup("Web", Order = 2, DisplayNameKey = "WebPort", DescriptionKey = "WebPort")]
    public int WebPort { get; set; } = 8080;

    /// <summary>
    /// Gets or sets whether to allow intranet access (requires admin)
    /// </summary>
    [ConfigGroup("Web", Order = 3, DisplayNameKey = "AllowIntranetAccess", DescriptionKey = "AllowIntranetAccess")]
    public bool AllowIntranet { get; set; } = false;

    /// <summary>
    /// Gets or sets the web skin name
    /// </summary>
    [ConfigGroup("Web", Order = 4, DisplayNameKey = "WebSkin", DescriptionKey = "WebSkin")]
    public string WebSkin { get; set; } = null!;

    /// <summary>
    /// Gets or sets the nickname of the human user
    /// </summary>
    [ConfigGroup("User", Order = 2, DisplayNameKey = "UserNickname", DescriptionKey = "UserNickname")]
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
                        new DirectoryInfoConverter(),
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
                    MinimumLogLevel = loadedData.MinimumLogLevel;
                    AIClientType = loadedData.AIClientType;
                    OllamaEndpoint = loadedData.OllamaEndpoint;
                    DefaultModel = loadedData.DefaultModel;
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
                new DirectoryInfoConverter(),
                new ConfigDataBaseConverter()
            }
        });
        string configPath = GetConfigFilePath();
        File.WriteAllText(configPath, json);
    }
}
