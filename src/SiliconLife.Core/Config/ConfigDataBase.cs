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
    [ConfigIgnore("Internal system use, for polymorphic deserialization")]
    public abstract string ConfigType { get; set; }

    /// <summary>
    /// Gets or sets the GUID of the curator (main administrator)
    /// </summary>
    [ConfigIgnore("Internal system identifier, not recommended to modify manually")]
    public abstract Guid CuratorGuid { get; set; }

    /// <summary>
    /// Gets or sets the language setting for the application
    /// </summary>
    [ConfigGroup("Basic", Order = 4, DisplayNameKey = "Language", DescriptionKey = "Language")]
    public abstract Language Language { get; set; }

    /// <summary>
    /// Gets or sets the timeout duration for each tick execution
    /// </summary>
    [ConfigGroup("Runtime", Order = 1, DisplayNameKey = "TickTimeout", DescriptionKey = "TickTimeout")]
    public abstract TimeSpan TickTimeout { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of consecutive timeouts allowed before circuit breaker triggers
    /// </summary>
    [ConfigGroup("Runtime", Order = 2, DisplayNameKey = "MaxTimeoutCount", DescriptionKey = "MaxTimeoutCount")]
    public abstract int MaxTimeoutCount { get; set; }

    /// <summary>
    /// Gets or sets the watchdog timeout duration.
    /// If the main loop thread does not update heartbeat within this duration,
    /// the watchdog will consider it hung and attempt to restart it.
    /// </summary>
    [ConfigGroup("Runtime", Order = 3, DisplayNameKey = "WatchdogTimeout", DescriptionKey = "WatchdogTimeout")]
    public abstract TimeSpan WatchdogTimeout { get; set; }

    /// <summary>
    /// Gets or sets the global minimum log level.
    /// Log entries below this level will not be recorded.
    /// </summary>
    [ConfigGroup("Runtime", Order = 4, DisplayNameKey = "MinLogLevel", DescriptionKey = "MinLogLevel")]
    public abstract LogLevel MinimumLogLevel { get; set; }

    /// <summary>
    /// Gets the reserved GUID representing the human user
    /// </summary>
    [ConfigIgnore("System reserved GUID, fixed value cannot be modified")]
    public Guid UserGuid { get; } = new Guid("00000000-0000-0000-0000-000000000001");

    /// <summary>
    /// Gets the reserved GUID representing the global broadcast channel
    /// </summary>
    [ConfigIgnore("System reserved GUID, fixed value cannot be modified")]
    public Guid BroadcastChannelGuid { get; } = new Guid("00000000-0000-0000-0000-000000000002");

    /// <summary>
    /// Gets or sets the nickname of the human user
    /// </summary>
    [ConfigGroup("User", Order = 2, DisplayNameKey = "UserNickname", DescriptionKey = "UserNickname")]
    public abstract string UserNickname { get; set; }

    /// <summary>
    /// Gets or sets the AI client type to use (e.g., "OllamaClient", "OpenAIClient")
    /// </summary>
    [ConfigGroup("AI", Order = 0, DisplayNameKey = "AIClientType", DescriptionKey = "AIClientType")]
    public abstract string AIClientType { get; set; }

    /// <summary>
    /// Gets or sets the global AI client configuration dictionary.
    /// Used when silicon beings don't have their own AI config.
    /// </summary>
    [ConfigGroup("AI", Order = 1, DisplayNameKey = "AIConfig", DescriptionKey = "AIConfigDescription")]
    public abstract Dictionary<string, object> AIConfig { get; set; }

    /// <summary>
    /// Gets or sets the list of plugin directories for auto-discovery.
    /// Each entry can be an absolute path or a relative path (relative to the application base directory).
    /// If empty upon load, defaults to ["plugins"] (relative to the application base directory).
    /// </summary>
    [ConfigGroup("Basic", Order = 5, DisplayNameKey = "PluginDirectories", DescriptionKey = "PluginDirectories")]
    public abstract List<string> PluginDirectories { get; set; }

    /// <summary>
    /// Gets or sets the list of IM platform configurations.
    /// Supports multiple IM platforms simultaneously, each with individual enable/disable control.
    /// </summary>
    [ConfigGroup("IM", Order = 6, DisplayNameKey = "IMPlatforms", DescriptionKey = "IMPlatforms")]
    public abstract List<IMPlatformConfig> IMPlatforms { get; set; }

    /// <summary>
    /// Gets or sets whether the skill system is enabled (default true).
    /// </summary>
    [ConfigGroup("Skill", Order = 0, DisplayNameKey = "SkillEnabled", DescriptionKey = "SkillEnabled")]
    public virtual bool SkillEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the global maximum tool round limit for a single skill execution
    /// (prevents runaway loops, default 10).
    /// </summary>
    [ConfigGroup("Skill", Order = 1, DisplayNameKey = "GlobalMaxToolRound", DescriptionKey = "GlobalMaxToolRound")]
    public virtual int GlobalMaxToolRound { get; set; } = 10;

    /// <summary>
    /// Gets or sets the global skill execution timeout limit in seconds (default 300).
    /// </summary>
    [ConfigGroup("Skill", Order = 2, DisplayNameKey = "GlobalSkillTimeoutSeconds", DescriptionKey = "GlobalSkillTimeoutSeconds")]
    public virtual int GlobalSkillTimeoutSeconds { get; set; } = 300;

    /// <summary>
    /// Gets or sets the maximum number of custom (being/user-created) skills per being (default 50).
    /// </summary>
    [ConfigGroup("Skill", Order = 3, DisplayNameKey = "MaxCustomSkillsPerBeing", DescriptionKey = "MaxCustomSkillsPerBeing")]
    public virtual int MaxCustomSkillsPerBeing { get; set; } = 50;

    /// <summary>
    /// Gets or sets whether the MCP (Model Context Protocol) integration is
    /// enabled (default false — external MCP servers must be enabled explicitly).
    /// </summary>
    [ConfigGroup("Mcp", Order = 0, DisplayNameKey = "McpEnabled", DescriptionKey = "McpEnabled")]
    public virtual bool McpEnabled { get; set; } = false;

    /// <summary>
    /// Gets or sets the timeout in seconds for a single MCP tool call (default 60).
    /// </summary>
    [ConfigGroup("Mcp", Order = 1, DisplayNameKey = "McpToolTimeoutSeconds", DescriptionKey = "McpToolTimeoutSeconds")]
    public virtual int McpToolTimeoutSeconds { get; set; } = 60;

    /// <summary>
    /// Gets or sets the maximum number of MCP tools visible to one being
    /// (prevents context bloat, default 40).
    /// </summary>
    [ConfigGroup("Mcp", Order = 2, DisplayNameKey = "MaxMcpToolsPerBeing", DescriptionKey = "MaxMcpToolsPerBeing")]
    public virtual int MaxMcpToolsPerBeing { get; set; } = 40;

    /// <summary>
    /// Gets or sets the maximum MCP tool result length in characters;
    /// longer results are truncated (default 32768).
    /// </summary>
    [ConfigGroup("Mcp", Order = 3, DisplayNameKey = "McpMaxResponseLength", DescriptionKey = "McpMaxResponseLength")]
    public virtual int McpMaxResponseLength { get; set; } = 32768;

    /// <summary>
    /// Gets or sets the list of external MCP server configurations.
    /// Only servers listed here can be connected; each entry defaults to
    /// disabled and must be enabled explicitly.
    /// </summary>
    public virtual List<McpServerConfig> McpServers { get; set; } = new();

    /// <summary>
    /// Gets the configuration file path
    /// </summary>
    /// <returns>The full path to the configuration file</returns>
    public abstract string GetConfigPath();

    /// <summary>
    /// Checks whether the configuration data exists
    /// </summary>
    /// <returns>True if configuration exists, false otherwise</returns>
    public abstract bool ConfigExists();

    /// <summary>
    /// Loads configuration from the configuration file
    /// </summary>
    public abstract void LoadConfig();

    /// <summary>
    /// Saves current configuration to the configuration file
    /// </summary>
    public abstract void SaveConfig();
}
