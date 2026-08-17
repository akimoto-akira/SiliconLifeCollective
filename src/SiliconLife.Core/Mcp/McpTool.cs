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
/// Wraps a single tool exposed by an external MCP server as an ITool.
/// The tool name is prefixed to guarantee global uniqueness and to form a
/// permission namespace: mcp_{serverId}_{originalToolName}. The wrapper does
/// not hold a connection: it resolves the live connection through
/// <see cref="McpManager"/> on every call, so server reconnects take effect
/// without re-registering tools.
/// </summary>
public class McpTool : ITool
{
    private readonly string _serverId;
    private readonly McpToolDefinition _definition;

    /// <summary>
    /// Initializes a new instance of the <see cref="McpTool"/> class.
    /// </summary>
    /// <param name="serverId">The MCP server id (used as name prefix).</param>
    /// <param name="definition">The tool definition reported by the server.</param>
    public McpTool(string serverId, McpToolDefinition definition)
    {
        _serverId = serverId;
        _definition = definition;
    }

    /// <summary>Gets the original (unprefixed) tool name on the server.</summary>
    public string OriginalName => _definition.Name;

    /// <summary>Gets the server-level permission key (mcp_{serverId}).</summary>
    public string ServerPermissionKey => $"mcp_{_serverId}";

    /// <summary>Gets the prefixed tool name: mcp_{serverId}_{name}.</summary>
    public string Name => $"mcp_{_serverId}_{_definition.Name}";

    /// <summary>Gets the server-reported tool description.</summary>
    public string Description =>
        string.IsNullOrWhiteSpace(_definition.Description)
            ? $"MCP tool '{_definition.Name}' provided by server '{_serverId}'"
            : _definition.Description;

    /// <summary>MCP servers do not provide localized names; returns the prefixed name.</summary>
    /// <param name="language">Ignored.</param>
    /// <returns>The prefixed tool name.</returns>
    public string GetDisplayName(Language language) => Name;

    /// <summary>
    /// Passes through the server-reported inputSchema (already a JSON Schema
    /// with "type": "object" at the top level).
    /// </summary>
    /// <returns>The parameter schema dictionary.</returns>
    public Dictionary<string, object> GetParameterSchema() => _definition.InputSchema;

    /// <summary>
    /// Calls the tool on the live server connection. Runtime permission check
    /// covers both the server-level key (mcp_{serverId}) and the tool-level
    /// key (the full prefixed name) with action "execute" — the same model
    /// the skill system uses (skillId + "execute").
    /// </summary>
    /// <param name="callerId">The id of the calling being.</param>
    /// <param name="parameters">The tool arguments.</param>
    /// <returns>The tool result with truncated text content.</returns>
    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        var being = ServiceLocator.Instance.BeingManager?.GetBeing(callerId);
        var permissions = being?.ToolActionPermissions;
        if (permissions != null)
        {
            if (permissions.IsActionDisabled(ServerPermissionKey, "execute"))
            {
                return ToolResult.Failed($"MCP server '{_serverId}' is disabled for this being");
            }
            if (permissions.IsActionDisabled(Name, "execute"))
            {
                return ToolResult.Failed($"MCP tool '{Name}' is disabled for this being");
            }
        }

        McpClientConnection? connection = McpManager.Instance.GetConnection(_serverId);
        if (connection == null)
        {
            return ToolResult.Failed($"MCP server '{_serverId}' is not connected");
        }

        int timeoutSeconds = Config.Instance?.Data?.McpToolTimeoutSeconds ?? 60;
        McpCallResult result = connection.CallTool(_definition.Name, parameters, timeoutSeconds);
        if (!result.IsSuccess)
        {
            return ToolResult.Failed(result.ErrorMessage ?? "MCP call failed");
        }

        return ToolResult.Successful(McpManager.TruncateResult(result.TextContent));
    }
}
