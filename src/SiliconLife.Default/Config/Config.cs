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
using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Configuration manager (Singleton pattern)
/// </summary>
public class Config
{
    private static readonly Lazy<Config> _instance = new Lazy<Config>(() => new Config());
    private const string ConfigFileName = "config.json";
    private ConfigDataBase _data = new DefaultConfigData();

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    public static Config Instance => _instance.Value;

    /// <summary>
    /// Gets the configuration data
    /// </summary>
    public ConfigDataBase Data => _data;

    private Config()
    {
        LoadOrCreateConfig();
    }

    /// <summary>
    /// Loads configuration from file or creates default configuration
    /// </summary>
    private void LoadOrCreateConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            try
            {
                string json = File.ReadAllText(ConfigFileName);
                _data = JsonSerializer.Deserialize<DefaultConfigData>(json) ?? new DefaultConfigData();
            }
            catch (Exception)
            {
                _data = new DefaultConfigData();
            }
        }
        else
        {
            _data = new DefaultConfigData();
            SaveConfig();
        }
    }

    /// <summary>
    /// Saves current configuration to file
    /// </summary>
    public void SaveConfig()
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        string json = JsonSerializer.Serialize(_data, options);
        File.WriteAllText(ConfigFileName, json);
    }

    /// <summary>
    /// Reloads configuration from file
    /// </summary>
    public void Reload()
    {
        LoadOrCreateConfig();
    }
}
