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
    [ConfigGroup("基础配置", Order = 2, DisplayName = "数据目录", Description = "Data directory path for storing all application data")]
    public override DirectoryInfo DataDirectory { get; set; } = new DirectoryInfo("./data");

    /// <summary>
    /// Gets or sets the GUID of the curator (main administrator)
    /// </summary>
    [ConfigIgnore("系统内部标识，不建议手动修改")]
    public override Guid CuratorGuid { get; set; }

    /// <summary>
    /// Gets or sets the language setting for the application
    /// </summary>
    [ConfigGroup("基础配置", Order = 4, DisplayName = "语言设置", Description = "Language setting for the application")]
    public override Language Language { get; set; } = Language.ZhCN;

    /// <summary>
    /// Gets or sets the timeout duration for each tick execution
    /// </summary>
    [ConfigGroup("运行时配置", Order = 1, DisplayName = "Tick 超时", Description = "Timeout duration for each tick execution")]
    public override TimeSpan TickTimeout { get; set; } = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Gets or sets the maximum number of consecutive timeouts allowed before circuit breaker triggers
    /// </summary>
    [ConfigGroup("运行时配置", Order = 2, DisplayName = "最大超时次数", Description = "Maximum consecutive timeouts before circuit breaker triggers")]
    public override int MaxTimeoutCount { get; set; } = 3;

    /// <summary>
    /// Gets or sets the watchdog timeout duration.
    /// </summary>
    [ConfigGroup("运行时配置", Order = 3, DisplayName = "看门狗超时", Description = "Watchdog timeout duration for detecting hung main loop")]
    public override TimeSpan WatchdogTimeout { get; set; } = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Gets or sets the global minimum log level.
    /// </summary>
    [ConfigGroup("运行时配置", Order = 4, DisplayName = "最小日志级别", Description = "Global minimum log level")]
    public override LogLevel MinimumLogLevel { get; set; } = LogLevel.Trace;

    /// <summary>
    /// Gets or sets the AI client type to use
    /// </summary>
    [ConfigGroup("AI 配置", Order = 0, DisplayName = "AI 客户端类型", Description = "AI client type to use")]
    public string AIClientType { get; set; } = "OllamaClient";

    /// <summary>
    /// Gets or sets the Ollama API endpoint URL
    /// </summary>
    [AIClientConfig("OllamaClient", Order = 1)]
    [ConfigGroup("AI 配置", Order = 1, DisplayName = "Ollama 端点", Description = "Ollama API endpoint URL")]
    public string OllamaEndpoint { get; set; } = "http://localhost:11434";

    /// <summary>
    /// Gets or sets the default AI model to use
    /// </summary>
    [AIClientConfig("OllamaClient", Order = 2)]
    [ConfigGroup("AI 配置", Order = 2, DisplayName = "默认模型", Description = "Default AI model to use")]
    public string DefaultModel { get; set; } = "qwen3.5:cloud";

    /// <summary>
    /// Gets or sets the web server port
    /// </summary>
    [ConfigGroup("Web 配置", Order = 2, DisplayName = "Web 端口", Description = "Web server port")]
    public int WebPort { get; set; } = 8080;

    /// <summary>
    /// Gets or sets whether to allow intranet access (requires admin)
    /// </summary>
    [ConfigGroup("Web 配置", Order = 3, DisplayName = "允许内网访问", Description = "Allow intranet access (requires admin)")]
    public bool AllowIntranet { get; set; } = false;

    /// <summary>
    /// Gets or sets the web skin name
    /// </summary>
    [ConfigGroup("Web 配置", Order = 4, DisplayName = "Web 皮肤", Description = "Web skin name")]
    public string WebSkin { get; set; } = null!;

    /// <summary>
    /// Gets or sets the nickname of the human user
    /// </summary>
    [ConfigGroup("用户配置", Order = 2, DisplayName = "用户昵称", Description = "Nickname of the human user")]
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
