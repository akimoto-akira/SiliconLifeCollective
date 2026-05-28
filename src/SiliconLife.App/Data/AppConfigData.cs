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

namespace SiliconLife.App.Data;

/// <summary>
/// Abstract base configuration data for all applications.
/// Concrete implementations should provide storage-specific logic.
/// </summary>
public abstract class AppConfigData : ConfigDataBase
{
    [ConfigIgnore("系统内部使用，用于多态反序列化")]
    public override string ConfigType { get; set; } = "Default";

    [ConfigIgnore("系统内部标识，不建议手动修改")]
    public override Guid CuratorGuid { get; set; }


    [ConfigGroup("Basic", Order = 4, DisplayNameKey = "Language", DescriptionKey = "Language")]
    public override Language Language { get; set; } = Language.ZhCN;

    [ConfigGroup("Runtime", Order = 1, DisplayNameKey = "TickTimeout", DescriptionKey = "TickTimeout")]
    public override TimeSpan TickTimeout { get; set; } = TimeSpan.FromMinutes(10);

    [ConfigGroup("Runtime", Order = 2, DisplayNameKey = "MaxTimeoutCount", DescriptionKey = "MaxTimeoutCount")]
    public override int MaxTimeoutCount { get; set; } = 3;

    [ConfigGroup("Runtime", Order = 3, DisplayNameKey = "WatchdogTimeout", DescriptionKey = "WatchdogTimeout")]
    public override TimeSpan WatchdogTimeout { get; set; } = TimeSpan.FromMinutes(10);

    [ConfigGroup("Runtime", Order = 4, DisplayNameKey = "MinLogLevel", DescriptionKey = "MinLogLevel")]
    public override LogLevel MinimumLogLevel { get; set; } = LogLevel.Trace;

    [ConfigGroup("AI", Order = 0, DisplayNameKey = "AIClientType", DescriptionKey = "AIClientType")]
    public override string AIClientType { get; set; } = "OllamaClient";

    [ConfigGroup("AI", Order = 1, DisplayNameKey = "AIConfig", DescriptionKey = "AIConfigDescription")]
    public override Dictionary<string, object> AIConfig { get; set; } = new Dictionary<string, object>
    {
        ["endpoint"] = "http://localhost:11434",
        ["model"] = "qwen3.5:cloud",
        ["temperature"] = 0.7,
        ["maxTokens"] = 4096
    };

    [ConfigGroup("Web", Order = 2, DisplayNameKey = "WebPort", DescriptionKey = "WebPort")]
    public int WebPort { get; set; } = 8080;

    [ConfigGroup("Web", Order = 4, DisplayNameKey = "WebSkin", DescriptionKey = "WebSkin")]
    public string WebSkin { get; set; } = null!;

    [ConfigGroup("Basic", Order = 5, DisplayNameKey = "PluginDirectories", DescriptionKey = "PluginDirectories")]
    public override List<string> PluginDirectories { get; set; } = new List<string>();

    [ConfigGroup("User", Order = 2, DisplayNameKey = "UserNickname", DescriptionKey = "UserNickname")]
    public override string UserNickname { get; set; } = "User";

    private string GetConfigKey() => "config";

    public override string GetConfigPath() => GetConfigKey();

    public abstract override bool ConfigExists();

    public abstract override void LoadConfig();

    public abstract override void SaveConfig();
}