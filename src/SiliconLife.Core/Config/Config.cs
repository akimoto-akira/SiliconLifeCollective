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
/// Configuration manager (Singleton pattern).
/// Uses ReaderWriterLockSlim for optimized lock granularity:
/// multiple concurrent readers, exclusive writer.
/// </summary>
public class Config : IDisposable
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<Config>();
    private static readonly Lazy<Config> _instance = new Lazy<Config>(() => new Config());
    private readonly ReaderWriterLockSlim _lock = new();
    private ConfigDataBase _data;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    public static Config Instance => _instance.Value;

    /// <summary>
    /// Gets the configuration data
    /// </summary>
    public ConfigDataBase Data
    {
        get
        {
            _lock.EnterReadLock();
            try
            {
                return _data;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }

    /// <summary>
    /// Initializes the config with a specific data type
    /// </summary>
    /// <param name="data">The configuration data instance</param>
    public void Initialize(ConfigDataBase data)
    {
        _lock.EnterWriteLock();
        try
        {
            _data = data;
            _logger.Info(null, $"Config initialized with data type: {data.GetType().Name}");
        }
        finally
        {
            _lock.ExitWriteLock();
        }
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

    /// <summary>
    /// Gets the configuration file path from the configuration data
    /// </summary>
    /// <returns>The full path to the configuration file</returns>
    public string GetConfigPath()
    {
        _lock.EnterReadLock();
        try
        {
            return _data.GetConfigPath();
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Loads configuration from file using the configuration data's LoadConfig method.
    /// Uses write lock since loading modifies internal state of _data.
    /// </summary>
    public void LoadConfig()
    {
        _lock.EnterWriteLock();
        try
        {
            try
            {
                _data.LoadConfig();
                _logger.Info(null, $"Config loaded from {_data.GetConfigPath()}");
            }
            catch (Exception)
            {
                _logger.Error(null, $"Failed to load config from {_data.GetConfigPath()}");
                throw;
            }
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Saves current configuration to file using the configuration data's SaveConfig method.
    /// Uses read lock since saving only reads _data without modification.
    /// </summary>
    public void SaveConfig()
    {
        _lock.EnterReadLock();
        try
        {
            try
            {
                _data.SaveConfig();
                _logger.Info(null, $"Config saved to {_data.GetConfigPath()}");
            }
            catch (Exception)
            {
                _logger.Error(null, $"Failed to save config to {_data.GetConfigPath()}");
                throw;
            }
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Reloads configuration from file
    /// </summary>
    public void Reload()
    {
        LoadConfig();
        _logger.Info(null, $"Config reloaded from {_data.GetConfigPath()}");
    }

    public void Dispose()
    {
        _lock.Dispose();
    }
}
