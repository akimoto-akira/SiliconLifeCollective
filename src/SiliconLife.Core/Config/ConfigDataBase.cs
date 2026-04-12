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

namespace SiliconLife.Collective;

/// <summary>
/// Abstract base class for configuration data
/// </summary>
public abstract class ConfigDataBase
{
    /// <summary>
    /// Gets or sets the configuration type identifier for polymorphic deserialization
    /// </summary>
    [ConfigIgnore("系统内部使用，用于多态反序列化")]
    public abstract string ConfigType { get; set; }

    /// <summary>
    /// Gets or sets the data directory for storing all application data
    /// </summary>
    [ConfigGroup("基础配置", Order = 2, DisplayName = "数据目录", Description = "Data directory path for storing all application data")]
    public abstract DirectoryInfo DataDirectory { get; set; }

    /// <summary>
    /// Gets or sets the GUID of the curator (main administrator)
    /// </summary>
    [ConfigIgnore("系统内部标识，不建议手动修改")]
    public abstract Guid CuratorGuid { get; set; }

    /// <summary>
    /// Gets or sets the language setting for the application
    /// </summary>
    [ConfigGroup("基础配置", Order = 4, DisplayName = "语言设置", Description = "Language setting for the application")]
    public abstract Language Language { get; set; }

    /// <summary>
    /// Gets or sets the timeout duration for each tick execution
    /// </summary>
    [ConfigGroup("运行时配置", Order = 1, DisplayName = "Tick 超时", Description = "Timeout duration for each tick execution")]
    public abstract TimeSpan TickTimeout { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of consecutive timeouts allowed before circuit breaker triggers
    /// </summary>
    [ConfigGroup("运行时配置", Order = 2, DisplayName = "最大超时次数", Description = "Maximum consecutive timeouts before circuit breaker triggers")]
    public abstract int MaxTimeoutCount { get; set; }

    /// <summary>
    /// Gets or sets the watchdog timeout duration.
    /// If the main loop thread does not update heartbeat within this duration,
    /// the watchdog will consider it hung and attempt to restart it.
    /// </summary>
    [ConfigGroup("运行时配置", Order = 3, DisplayName = "看门狗超时", Description = "Watchdog timeout duration for detecting hung main loop")]
    public abstract TimeSpan WatchdogTimeout { get; set; }

    /// <summary>
    /// Gets or sets the global minimum log level.
    /// Log entries below this level will not be recorded.
    /// </summary>
    [ConfigGroup("运行时配置", Order = 4, DisplayName = "最小日志级别", Description = "Global minimum log level")]
    public abstract LogLevel MinimumLogLevel { get; set; }

    /// <summary>
    /// Gets the reserved GUID representing the human user
    /// </summary>
    [ConfigIgnore("系统保留 GUID，固定值不可修改")]
    public Guid UserGuid { get; } = new Guid("00000000-0000-0000-0000-000000000001");

    /// <summary>
    /// Gets or sets the nickname of the human user
    /// </summary>
    [ConfigGroup("用户配置", Order = 2, DisplayName = "用户昵称", Description = "Nickname of the human user")]
    public abstract string UserNickname { get; set; }

    /// <summary>
    /// Gets the configuration file path
    /// </summary>
    /// <returns>The full path to the configuration file</returns>
    public abstract string GetConfigPath();

    /// <summary>
    /// Loads configuration from the configuration file
    /// </summary>
    public abstract void LoadConfig();

    /// <summary>
    /// Saves current configuration to the configuration file
    /// </summary>
    public abstract void SaveConfig();
}
