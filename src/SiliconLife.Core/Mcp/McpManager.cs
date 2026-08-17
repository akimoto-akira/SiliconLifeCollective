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
/// Manages connections to external MCP servers and the tools they provide.
/// Design mirrors SkillManager: configuration-driven, rate-limited refresh
/// (30s throttle, same as skill hot-reload), permission-aware tool
/// enumeration per being. One shared singleton instance; each being's
/// ToolManager holds McpTool wrapper instances filtered by
/// AllowedBeingIds/AllowedTools and the being's permission config.
/// </summary>
public sealed class McpManager : IDisposable
{
    private static readonly Lazy<McpManager> _instance = new(() => new McpManager());
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<McpManager>();

    private readonly Dictionary<string, ServerEntry> _servers = new();
    private readonly object _lock = new();
    private bool _loaded;
    private DateTime _lastRefreshUtc = DateTime.MinValue;
    private string _configFingerprint = string.Empty;

    private const int RefreshIntervalSeconds = 30;
    private const int RetryFailedIntervalSeconds = 60;

    private sealed class ServerEntry
    {
        public McpServerConfig Config = null!;
        public McpClientConnection? Connection;
        public List<McpToolDefinition> Tools = new();
        public bool Connected;
        public string? LastError;
        public DateTime LastAttemptUtc = DateTime.MinValue;
        public bool ToolsDirty;
    }

    private McpManager()
    {
    }

    /// <summary>Gets the shared singleton instance.</summary>
    public static McpManager Instance => _instance.Value;

    /// <summary>Gets the global MCP switch (config "Mcp" group, default off).</summary>
    public static bool McpEnabled => Config.Instance?.Data?.McpEnabled ?? false;

    /// <summary>Truncates a tool result text to the configured maximum length.</summary>
    /// <param name="text">The raw result text.</param>
    /// <returns>The truncated text with a marker suffix when truncated.</returns>
    public static string TruncateResult(string text)
    {
        int maxLength = Config.Instance?.Data?.McpMaxResponseLength ?? 32768;
        if (maxLength <= 0 || text.Length <= maxLength)
        {
            return text;
        }
        return text[..maxLength] + "\n...[truncated]";
    }

    /// <summary>
    /// Loads the server list from configuration (once). Connecting happens
    /// lazily on the first refresh/sync.
    /// </summary>
    public void EnsureLoaded()
    {
        lock (_lock)
        {
            if (_loaded)
            {
                return;
            }

            foreach (McpServerConfig config in Config.Instance?.Data?.McpServers ?? new List<McpServerConfig>())
            {
                if (string.IsNullOrEmpty(config.Id) || _servers.ContainsKey(config.Id))
                {
                    continue;
                }
                _servers[config.Id] = new ServerEntry { Config = config };
            }

            _loaded = true;
            _logger.Info(null, "[Mcp] loaded {0} server(s) from config", _servers.Count);
        }
    }

    /// <summary>Gets the live connection for a connected server, or null.</summary>
    /// <param name="serverId">The server id.</param>
    /// <returns>The active connection when the server is connected.</returns>
    public McpClientConnection? GetConnection(string serverId)
    {
        lock (_lock)
        {
            return _servers.TryGetValue(serverId, out ServerEntry? entry) && entry.Connected
                ? entry.Connection
                : null;
        }
    }

    /// <summary>
    /// Produces the McpTool wrappers visible to one being: enabled + connected
    /// servers only, AllowedBeingIds / AllowedTools filters applied, the
    /// server-level permission key checked (whole-server disable), capped by
    /// MaxMcpToolsPerBeing.
    /// </summary>
    /// <param name="beingId">The target being id.</param>
    /// <param name="permissions">The being's tool action permissions.</param>
    /// <returns>The wrapper tool instances to register.</returns>
    public List<McpTool> GetToolsForBeing(Guid beingId, ToolActionPermissionConfig? permissions)
    {
        var result = new List<McpTool>();
        if (!McpEnabled)
        {
            return result;
        }

        EnsureLoaded();

        int maxTools = Config.Instance?.Data?.MaxMcpToolsPerBeing ?? 40;
        List<ServerEntry> snapshot;
        lock (_lock)
        {
            snapshot = _servers.Values.ToList();
        }

        foreach (ServerEntry entry in snapshot)
        {
            if (result.Count >= maxTools)
            {
                break;
            }

            if (!entry.Config.Enabled || !entry.Connected || entry.Tools.Count == 0)
            {
                continue;
            }

            if (entry.Config.AllowedBeingIds.Count > 0 && !entry.Config.AllowedBeingIds.Contains(beingId))
            {
                continue;
            }

            if (permissions != null && permissions.IsActionDisabled($"mcp_{entry.Config.Id}", "execute"))
            {
                continue;
            }

            foreach (McpToolDefinition definition in entry.Tools)
            {
                if (result.Count >= maxTools)
                {
                    break;
                }

                if (entry.Config.AllowedTools.Count > 0 && !entry.Config.AllowedTools.Contains(definition.Name))
                {
                    continue;
                }

                result.Add(new McpTool(entry.Config.Id, definition));
            }
        }

        return result;
    }

    /// <summary>
    /// Reconciles the MCP tools registered in one being's ToolManager with
    /// the current desired set (mirrors the SkillManager hot-reload diff).
    /// </summary>
    /// <param name="being">The target being.</param>
    public void SyncToolsForBeing(SiliconBeingBase being)
    {
        ToolManager? toolManager = being.ToolManager;
        if (toolManager == null)
        {
            return;
        }

        List<McpTool> desired = GetToolsForBeing(being.Id, being.ToolActionPermissions);
        var desiredNames = new HashSet<string>(desired.Select(t => t.Name));

        // Remove MCP tools that are no longer desired
        foreach (string name in toolManager.GetToolNames().ToList())
        {
            if (name.StartsWith("mcp_", StringComparison.Ordinal) && !desiredNames.Contains(name))
            {
                toolManager.UnregisterTool(name);
            }
        }

        // Register new ones
        foreach (McpTool tool in desired)
        {
            if (!toolManager.HasTool(tool.Name))
            {
                toolManager.RegisterTool(tool);
            }
        }
    }

    /// <summary>Re-syncs the MCP tools of all registered beings.</summary>
    public void SyncAllBeings()
    {
        var beingManager = ServiceLocator.Instance.BeingManager;
        if (beingManager == null)
        {
            return;
        }

        foreach (SiliconBeingBase being in beingManager.GetAllBeings())
        {
            SyncToolsForBeing(being);
        }
    }

    /// <summary>
    /// Rate-limited (30s) refresh: detects config changes and server-side
    /// tools/list_changed notifications, retries failed servers after a
    /// cool-down, and re-syncs beings when something changed.
    /// </summary>
    public void RefreshIfNeeded()
    {
        if (!McpEnabled)
        {
            return;
        }

        EnsureLoaded();

        DateTime now = DateTime.UtcNow;
        lock (_lock)
        {
            if ((now - _lastRefreshUtc).TotalSeconds < RefreshIntervalSeconds)
            {
                return;
            }
            _lastRefreshUtc = now;
        }

        bool changed = false;

        string fingerprint = ComputeConfigFingerprint();
        if (fingerprint != _configFingerprint)
        {
            _configFingerprint = fingerprint;
            ApplyConfigDiff();
            changed = true;
        }
        else
        {
            changed |= RetryFailedServers();
        }

        changed |= ReloadDirtyToolLists();

        if (changed)
        {
            SyncAllBeings();
        }
    }

    private string ComputeConfigFingerprint()
    {
        try
        {
            return JsonSerializer.Serialize(Config.Instance?.Data?.McpServers ?? new List<McpServerConfig>());
        }
        catch
        {
            return string.Empty;
        }
    }

    private void ApplyConfigDiff()
    {
        List<McpServerConfig> configs = Config.Instance?.Data?.McpServers ?? new List<McpServerConfig>();

        lock (_lock)
        {
            // Servers removed from config → disconnect and drop
            foreach (string id in _servers.Keys.ToList())
            {
                if (configs.All(c => c.Id != id))
                {
                    ServerEntry entry = _servers[id];
                    entry.Connection?.Dispose();
                    _servers.Remove(id);
                    _logger.Info(null, "[Mcp] server removed: {0}", id);
                }
            }

            foreach (McpServerConfig config in configs)
            {
                if (string.IsNullOrEmpty(config.Id))
                {
                    continue;
                }

                if (!_servers.TryGetValue(config.Id, out ServerEntry? entry))
                {
                    _servers[config.Id] = new ServerEntry { Config = config };
                    _logger.Info(null, "[Mcp] server added: {0}", config.Id);
                }
                else if (!ConfigEquals(entry.Config, config))
                {
                    entry.Connection?.Dispose();
                    entry.Connection = null;
                    entry.Connected = false;
                    entry.Tools = new List<McpToolDefinition>();
                    entry.Config = CloneConfig(config);
                    _logger.Info(null, "[Mcp] server config changed: {0}", config.Id);
                }
            }
        }

        ConnectPendingServers();
    }

    private static bool ConfigEquals(McpServerConfig a, McpServerConfig b)
    {
        try
        {
            return JsonSerializer.Serialize(a) == JsonSerializer.Serialize(b);
        }
        catch
        {
            return false;
        }
    }

    private static McpServerConfig CloneConfig(McpServerConfig config)
    {
        // Re-deserialize through JSON for a deep copy
        return JsonSerializer.Deserialize<McpServerConfig>(JsonSerializer.Serialize(config)) ?? config;
    }

    private bool RetryFailedServers()
    {
        bool changed = false;
        List<ServerEntry> retryable;

        lock (_lock)
        {
            retryable = _servers.Values
                .Where(e => e.Config.Enabled && !e.Connected &&
                    (DateTime.UtcNow - e.LastAttemptUtc).TotalSeconds > RetryFailedIntervalSeconds)
                .ToList();
        }

        foreach (ServerEntry entry in retryable)
        {
            _logger.Info(null, "[Mcp] retrying failed server: {0}", entry.Config.Id);
            if (TryConnect(entry))
            {
                changed = true;
            }
        }

        return changed;
    }

    private bool ReloadDirtyToolLists()
    {
        List<ServerEntry> dirty;

        lock (_lock)
        {
            dirty = _servers.Values.Where(e => e.Connected && e.ToolsDirty).ToList();
            foreach (ServerEntry entry in dirty)
            {
                entry.ToolsDirty = false;
            }
        }

        bool changed = false;
        foreach (ServerEntry entry in dirty)
        {
            try
            {
                McpClientConnection? connection = entry.Connection;
                if (connection != null)
                {
                    entry.Tools = connection.ListTools();
                    changed = true;
                    _logger.Info(null, "[Mcp] tool list reloaded: {0} ({1} tools)", entry.Config.Id, entry.Tools.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.Warn(null, "[Mcp] reload tool list failed for '{0}': {1}", entry.Config.Id, ex.Message);
            }
        }

        return changed;
    }

    /// <summary>
    /// Attempts to connect all enabled servers that have no connection yet.
    /// Called at being creation and after configuration changes.
    /// </summary>
    public void ConnectPendingServers()
    {
        List<ServerEntry> pending;
        lock (_lock)
        {
            pending = _servers.Values.Where(e => e.Config.Enabled && e.Connection == null).ToList();
        }

        foreach (ServerEntry entry in pending)
        {
            TryConnect(entry);
        }
    }

    private bool TryConnect(ServerEntry entry)
    {
        entry.LastAttemptUtc = DateTime.UtcNow;

        McpClientConnection connection = entry.Config.Transport == McpTransportType.Stdio
            ? new StdioMcpClientConnection(entry.Config)
            : new HttpMcpClientConnection(entry.Config);

        try
        {
            if (!connection.Initialize())
            {
                entry.LastError = connection.LastError ?? "initialize failed";
                entry.Connected = false;
                connection.Dispose();
                _logger.Warn(null, "[Mcp] connect failed: {0} ({1})", entry.Config.Id, entry.LastError);
                return false;
            }

            entry.Tools = connection.ListTools();
            entry.Connection = connection;
            entry.Connected = true;
            entry.LastError = null;
            entry.ToolsDirty = false;
            connection.ToolsChanged += () =>
            {
                lock (_lock)
                {
                    if (_servers.TryGetValue(entry.Config.Id, out ServerEntry? current) && current == entry)
                    {
                        entry.ToolsDirty = true;
                    }
                }
            };

            _logger.Info(null, "[Mcp] server connected: {0} (transport={1}, tools={2})",
                entry.Config.Id, entry.Config.Transport, entry.Tools.Count);
            return true;
        }
        catch (Exception ex)
        {
            entry.Connected = false;
            entry.LastError = ex.Message;
            connection.Dispose();
            _logger.Warn(null, "[Mcp] connect failed: {0} ({1})", entry.Config.Id, ex.Message);
            return false;
        }
    }

    /// <summary>Gets the connection status snapshot of all servers (for the Web UI).</summary>
    /// <returns>The status list.</returns>
    public List<McpServerStatus> GetServerStatuses()
    {
        EnsureLoaded();

        List<ServerEntry> snapshot;
        lock (_lock)
        {
            snapshot = _servers.Values.ToList();
        }

        return snapshot.Select(entry => new McpServerStatus
        {
            Id = entry.Config.Id,
            Name = string.IsNullOrEmpty(entry.Config.Name) ? entry.Config.Id : entry.Config.Name,
            Transport = entry.Config.Transport,
            Enabled = entry.Config.Enabled,
            State = !entry.Config.Enabled
                ? McpConnectionState.Disabled
                : entry.Connected
                    ? McpConnectionState.Connected
                    : entry.LastError != null ? McpConnectionState.Failed : McpConnectionState.Pending,
            ToolCount = entry.Tools.Count,
            Endpoint = entry.Config.Transport == McpTransportType.Stdio
                ? $"{entry.Config.Command} {string.Join(" ", entry.Config.Args)}".Trim()
                : entry.Config.Url ?? string.Empty,
            LastError = entry.LastError,
            ToolNames = entry.Tools.Select(t => t.Name).ToList(),
        }).ToList();
    }

    /// <summary>
    /// Adds a server to the configuration, saves it, and (when enabled)
    /// connects immediately. Returns an error message, or null on success.
    /// </summary>
    /// <param name="config">The server configuration to add.</param>
    /// <returns>An error message, or null when the server was added.</returns>
    public string? AddServer(McpServerConfig config)
    {
        if (string.IsNullOrWhiteSpace(config.Id) || !System.Text.RegularExpressions.Regex.IsMatch(config.Id, "^[a-z0-9_]+$"))
        {
            return "Server id must contain only lowercase letters, digits and underscores";
        }

        List<McpServerConfig> servers = Config.Instance.Data.McpServers;
        if (servers.Any(s => s.Id == config.Id))
        {
            return $"Server '{config.Id}' already exists";
        }

        servers.Add(CloneConfig(config));
        Config.Instance.SaveConfig();

        EnsureLoaded();
        lock (_lock)
        {
            _servers[config.Id] = new ServerEntry { Config = CloneConfig(config) };
        }

        if (config.Enabled)
        {
            ConnectPendingServers();
        }

        SyncAllBeings();
        return null;
    }

    /// <summary>
    /// Removes a server from the configuration, disconnects it, and
    /// unregisters its tools from all beings.
    /// </summary>
    /// <param name="serverId">The server id.</param>
    /// <returns>An error message, or null on success.</returns>
    public string? RemoveServer(string serverId)
    {
        List<McpServerConfig> servers = Config.Instance.Data.McpServers;
        int removed = servers.RemoveAll(s => s.Id == serverId);
        if (removed == 0)
        {
            return $"Server '{serverId}' not found";
        }

        Config.Instance.SaveConfig();

        lock (_lock)
        {
            if (_servers.Remove(serverId, out ServerEntry? entry))
            {
                entry.Connection?.Dispose();
            }
        }

        SyncAllBeings();
        _logger.Info(null, "[Mcp] server removed: {0}", serverId);
        return null;
    }

    /// <summary>
    /// Enables or disables a server; enabling connects it immediately,
    /// disabling disconnects it.
    /// </summary>
    /// <param name="serverId">The server id.</param>
    /// <param name="enabled">The new enabled state.</param>
    /// <returns>An error message, or null on success.</returns>
    public string? ToggleServer(string serverId, bool enabled)
    {
        McpServerConfig? config = Config.Instance.Data.McpServers.FirstOrDefault(s => s.Id == serverId);
        if (config == null)
        {
            return $"Server '{serverId}' not found";
        }

        config.Enabled = enabled;
        Config.Instance.SaveConfig();

        EnsureLoaded();
        lock (_lock)
        {
            if (_servers.TryGetValue(serverId, out ServerEntry? entry))
            {
                entry.Config = CloneConfig(config);
                if (!enabled)
                {
                    entry.Connection?.Dispose();
                    entry.Connection = null;
                    entry.Connected = false;
                }
            }
        }

        if (enabled)
        {
            ConnectPendingServers();
        }

        SyncAllBeings();
        return null;
    }

    /// <summary>Forces a reconnect of one server.</summary>
    /// <param name="serverId">The server id.</param>
    /// <returns>An error message, or null on success.</returns>
    public string? ReconnectServer(string serverId)
    {
        EnsureLoaded();

        ServerEntry? entry;
        lock (_lock)
        {
            _servers.TryGetValue(serverId, out entry);
        }

        if (entry == null)
        {
            return $"Server '{serverId}' not found";
        }

        lock (_lock)
        {
            entry.Connection?.Dispose();
            entry.Connection = null;
            entry.Connected = false;
            entry.Tools = new List<McpToolDefinition>();
        }

        bool ok = TryConnect(entry);
        SyncAllBeings();
        return ok ? null : entry.LastError ?? "reconnect failed";
    }

    /// <summary>
    /// Tests one tool call directly against the server (Web UI test button).
    /// The tool name may be passed with or without the mcp_{serverId}_ prefix.
    /// </summary>
    /// <param name="serverId">The server id.</param>
    /// <param name="toolName">The original or prefixed tool name.</param>
    /// <param name="arguments">The tool arguments (native values).</param>
    /// <returns>The call result.</returns>
    public McpCallResult TestTool(string serverId, string toolName, Dictionary<string, object> arguments)
    {
        McpClientConnection? connection = GetConnection(serverId);
        if (connection == null)
        {
            return McpCallResult.Fail($"MCP server '{serverId}' is not connected");
        }

        string originalName = toolName;
        string prefix = $"mcp_{serverId}_";
        if (originalName.StartsWith(prefix, StringComparison.Ordinal))
        {
            originalName = originalName[prefix.Length..];
        }

        int timeoutSeconds = Config.Instance?.Data?.McpToolTimeoutSeconds ?? 60;
        McpCallResult result = connection.CallTool(originalName, arguments, timeoutSeconds);
        return result.IsSuccess
            ? McpCallResult.Ok(TruncateResult(result.TextContent))
            : result;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        lock (_lock)
        {
            foreach (ServerEntry entry in _servers.Values)
            {
                entry.Connection?.Dispose();
            }
            _servers.Clear();
            _loaded = false;
        }
    }
}
