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

/// <summary>MCP server transport type.</summary>
public enum McpTransportType
{
    /// <summary>Subprocess stdio transport (command + args + env).</summary>
    Stdio,

    /// <summary>Streamable HTTP transport (url + optional headers).</summary>
    Http,
}

/// <summary>Connection state of an MCP server entry.</summary>
public enum McpConnectionState
{
    /// <summary>Not enabled in configuration.</summary>
    Disabled,

    /// <summary>Handshake completed and tools loaded.</summary>
    Connected,

    /// <summary>Enabled but the last connect attempt failed.</summary>
    Failed,

    /// <summary>Enabled but not yet attempted (lazy connect pending).</summary>
    Pending,
}

/// <summary>Connection status snapshot of an MCP server (for the Web UI / monitoring).</summary>
public class McpServerStatus
{
    /// <summary>Gets the server id.</summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>Gets the human-readable server name.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Gets the transport type.</summary>
    public McpTransportType Transport { get; init; }

    /// <summary>Gets the connection state.</summary>
    public McpConnectionState State { get; init; }

    /// <summary>Gets whether the server is enabled in configuration.</summary>
    public bool Enabled { get; init; }

    /// <summary>Gets the number of discovered tools.</summary>
    public int ToolCount { get; init; }

    /// <summary>Gets the endpoint description (command line or URL).</summary>
    public string Endpoint { get; init; } = string.Empty;

    /// <summary>Gets the last error message, if any.</summary>
    public string? LastError { get; init; }

    /// <summary>Gets the ids of the discovered tools.</summary>
    public List<string> ToolNames { get; init; } = new();
}

/// <summary>Connection configuration for one external MCP server.</summary>
public class McpServerConfig
{
    /// <summary>
    /// Gets or sets the server identifier (lowercase letters/digits/underscores).
    /// Used as the tool-name prefix and permission key: mcp_{id}_{toolName}.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>Gets or sets the human-readable name (Web UI display).</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the transport type.</summary>
    public McpTransportType Transport { get; set; } = McpTransportType.Stdio;

    /// <summary>Gets or sets the executable path for stdio transport, e.g. "npx".</summary>
    public string? Command { get; set; }

    /// <summary>
    /// Gets or sets the argument list for stdio transport,
    /// e.g. ["-y", "@modelcontextprotocol/server-filesystem", "/data"].
    /// </summary>
    public List<string> Args { get; set; } = new();

    /// <summary>Gets or sets environment variables for stdio transport.</summary>
    public Dictionary<string, string> Environment { get; set; } = new();

    /// <summary>Gets or sets the server URL for HTTP transport, e.g. "http://localhost:3000/mcp".</summary>
    public string? Url { get; set; }

    /// <summary>Gets or sets additional request headers for HTTP transport (e.g. Authorization).</summary>
    public Dictionary<string, string> Headers { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether this server is enabled.
    /// Defaults to false — servers must be enabled explicitly.
    /// </summary>
    public bool Enabled { get; set; } = false;

    /// <summary>
    /// Gets or sets the original tool-name whitelist: empty = all tools of the
    /// server; non-empty = only these original tool names.
    /// </summary>
    public List<string> AllowedTools { get; set; } = new();

    /// <summary>
    /// Gets or sets the being ids allowed to use this server;
    /// empty = all beings (including the curator).
    /// </summary>
    public List<Guid> AllowedBeingIds { get; set; } = new();
}
