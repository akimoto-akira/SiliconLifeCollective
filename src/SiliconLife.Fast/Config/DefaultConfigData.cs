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
using SiliconLife.App.Data;
using SiliconLife.Collective;
using SiliconLife.Fast;

namespace SiliconLife.Fast.Config;

/// <summary>
/// Fast implementation of configuration data.
/// Configuration is persisted using the SpeedyPackRegistry for high-performance storage.
/// </summary>
public class DefaultConfigData : AppConfigData
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<DefaultConfigData>();
    
    [ConfigIgnore("系统内部使用，用于多态反序列化")]
    public override string ConfigType { get; set; } = "Fast";

    private string GetConfigKey() => "config";

    public override string GetConfigPath() => GetConfigKey();

    public override bool ConfigExists() => SpeedyPackRegistry.Pack.Exists(GetConfigKey());

    public override void LoadConfig()
    {
        try
        {
            var loaded = SpeedyPackRegistry.Pack.Read<DefaultConfigData>(GetConfigKey());
            if (loaded == null) return;

            ConfigType = loaded.ConfigType;
            CuratorGuid = loaded.CuratorGuid;
            Language = loaded.Language;
            TickTimeout = loaded.TickTimeout;
            MaxTimeoutCount = loaded.MaxTimeoutCount;
            WatchdogTimeout = loaded.WatchdogTimeout;
            MinimumLogLevel = loaded.MinimumLogLevel;
            AIClientType = loaded.AIClientType;
            AIConfig = loaded.AIConfig ?? new Dictionary<string, object>();
            WebPort = loaded.WebPort;
            WebSkin = loaded.WebSkin;
            UserNickname = loaded.UserNickname;
        }
        catch (Exception)
        {
        }
    }

    public override void SaveConfig()
    {
        try
        {
            SpeedyPackRegistry.Pack.Write(GetConfigKey(), this);
            _logger.Info(null, "Configuration saved successfully to SpeedyPack");
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to save configuration: {0}", ex.Message);
            throw; // Re-throw to ensure caller knows save failed
        }
    }
}
