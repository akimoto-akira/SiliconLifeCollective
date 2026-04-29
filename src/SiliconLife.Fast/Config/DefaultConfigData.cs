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
using LiteDB;

namespace SiliconLife.Fast;

/// <summary>
/// Default implementation of configuration data
/// </summary>
public class DefaultConfigData : ConfigDataBase
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<DefaultConfigData>();
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
    public override string AIClientType { get; set; } = "OllamaClient";

    /// <summary>
    /// Gets or sets the global AI client configuration dictionary.
    /// Used when silicon beings don't have their own AI config.
    /// </summary>
    [ConfigGroup("AI", Order = 1, DisplayNameKey = "AIConfig", DescriptionKey = "AIConfigDescription")]
    public override Dictionary<string, object> AIConfig { get; set; } = new Dictionary<string, object>
    {
        ["endpoint"] = "http://localhost:11434",
        ["model"] = "qwen3.5:cloud",
        ["temperature"] = 0.7,
        ["maxTokens"] = 4096
    };

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

    /// <summary>
    /// Returns identifier indicating LiteDB storage
    /// </summary>
    public override string GetConfigPath()
    {
        return "LiteDB:app_config";
    }

    /// <summary>
    /// Loads configuration from LiteDB (replaces config.json)
    /// </summary>
    public override void LoadConfig()
    {
        try
        {
            var appConfig = LiteDBManager.GetConfig();
            
            // Map AppConfig -> DefaultConfigData
            ConfigType = appConfig.ConfigType;
            DataDirectory = new DirectoryInfo(appConfig.DataDirectory);
            CuratorGuid = appConfig.CuratorGuid;
            Language = appConfig.Language;
            TickTimeout = TimeSpan.FromMinutes(appConfig.TickTimeoutMinutes);
            MaxTimeoutCount = appConfig.MaxTimeoutCount;
            WatchdogTimeout = TimeSpan.FromMinutes(appConfig.WatchdogTimeoutMinutes);
            MinimumLogLevel = appConfig.MinimumLogLevel;
            AIClientType = appConfig.AIClientType;
            
            // Deserialize AIConfig from BsonDocument
            AIConfig = BsonMapper.Global.Deserialize<Dictionary<string, object>>(appConfig.AIConfig) ?? new Dictionary<string, object>();
            
            WebPort = appConfig.WebPort;
            AllowIntranet = appConfig.AllowIntranet;
            WebSkin = appConfig.WebSkin ?? string.Empty;
            UserNickname = appConfig.UserNickname;
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Config Load Error from LiteDB: {0}", ex.Message);
        }
    }

    /// <summary>
    /// Saves configuration to LiteDB (replaces config.json)
    /// </summary>
    public override void SaveConfig()
    {
        try
        {
            var appConfig = new AppConfig
            {
                ConfigType = ConfigType,
                DataDirectory = DataDirectory.FullName,
                CuratorGuid = CuratorGuid,
                Language = Language,
                TickTimeoutMinutes = (int)TickTimeout.TotalMinutes,
                MaxTimeoutCount = MaxTimeoutCount,
                WatchdogTimeoutMinutes = (int)WatchdogTimeout.TotalMinutes,
                MinimumLogLevel = MinimumLogLevel,
                AIClientType = AIClientType,
                AIConfig = BsonMapper.Global.Serialize(AIConfig).AsDocument,
                WebPort = WebPort,
                AllowIntranet = AllowIntranet,
                WebSkin = string.IsNullOrEmpty(WebSkin) ? null : WebSkin,
                UserNickname = UserNickname
            };
            
            LiteDBManager.SaveConfig(appConfig);
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Config Save Error to LiteDB: {0}", ex.Message);
        }
    }
}
