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

using System.Text.Json;

namespace SiliconLife.Collective;

/// <summary>
/// Configuration manager (Singleton pattern)
/// </summary>
public class Config
{
    private static readonly Lazy<Config> _instance = new Lazy<Config>(() => new Config());
    private const string ConfigFileName = "config.json";
    private ConfigDataBase _data;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    public static Config Instance => _instance.Value;

    /// <summary>
    /// Gets the configuration data
    /// </summary>
    public ConfigDataBase Data => _data;

    /// <summary>
    /// Initializes the config with a specific data type
    /// </summary>
    /// <param name="data">The configuration data instance</param>
    public void Initialize(ConfigDataBase data)
    {
        _data = data;
    }

    private Config()
    {
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = 
            { 
                new System.Text.Json.Serialization.JsonStringEnumConverter(),
                new GuidConverter(),
                new ConfigDataBaseConverter()
            }
        };
    }

    private string GetConfigFilePath()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string configPath = Path.Combine(baseDir, ConfigFileName);

        if (File.Exists(configPath))
        {
            return configPath;
        }

        return Path.Combine(Directory.GetCurrentDirectory(), ConfigFileName);
    }

    /// <summary>
    /// Loads configuration from file or creates default configuration
    /// </summary>
    public void LoadOrCreateConfig()
    {
        string configPath = GetConfigFilePath();

        if (File.Exists(configPath))
        {
            try
            {
                string json = File.ReadAllText(configPath);
                ConfigDataBase? loadedData = JsonSerializer.Deserialize<ConfigDataBase>(json, _jsonOptions);
                if (loadedData != null)
                {
                    _data = loadedData;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Config Load Error: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Saves current configuration to file
    /// </summary>
    public void SaveConfig()
    {
        string json = JsonSerializer.Serialize(_data, _jsonOptions);
        string configPath = GetConfigFilePath();
        File.WriteAllText(configPath, json);
    }

    /// <summary>
    /// Reloads configuration from file
    /// </summary>
    public void Reload()
    {
        LoadOrCreateConfig();
    }
}
