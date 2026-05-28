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

using SiliconLife.App.Data;
using SiliconLife.Collective;
using System.IO;
using System.Text.Json;

namespace SiliconLife.Default.Config;

/// <summary>
/// Default implementation of configuration data
/// </summary>
public class DefaultConfigData : AppConfigData
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<DefaultConfigData>();

    [ConfigIgnore("系统内部使用，用于多态反序列化")]
    public override string ConfigType { get; set; } = "Default";

    /// <summary>
    /// Gets or sets the data directory for storing all application data
    /// </summary>
    [ConfigGroup("Basic", Order = 2, DisplayNameKey = "DataDirectory", DescriptionKey = "DataDirectory")]
    public DirectoryInfo DataDirectory { get; set; } = new DirectoryInfo("./data");

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

    public override bool ConfigExists()
    {
        return File.Exists(GetConfigFilePath());
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
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                    MaxDepth = 128,
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
                    AIConfig = loadedData.AIConfig ?? new Dictionary<string, object>();
                    WebPort = loadedData.WebPort;
                    WebSkin = loadedData.WebSkin;
                    PluginDirectories = loadedData.PluginDirectories ?? new List<string>();
                    UserNickname = loadedData.UserNickname;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(null, "Config Load Error: {0}", ex.Message);
            }
        }
    }

    public override void SaveConfig()
    {
        string json = JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
            MaxDepth = 128,
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
