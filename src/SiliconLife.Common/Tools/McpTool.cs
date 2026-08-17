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

using SiliconLife.Common.Localization;

// Alias: this class is also named McpTool (the AI-facing query tool), while
// SiliconLife.Collective.McpTool is the per-server tool wrapper.
using McpWrappedTool = SiliconLife.Collective.McpTool;

namespace SiliconLife.Common.Tools;

/// <summary>
/// Read-only tool for querying the MCP (Model Context Protocol) integration
/// state: connected servers, the tools they provide, and how to call them.
/// Servers are added/removed only by the user through the Web UI — the AI
/// cannot modify the server list.
/// </summary>
[ToolAction("status", "list_servers", "list_tools")]
[ToolScenario(ToolScenarioFlag.All)]
public class McpTool : ITool
{
    /// <inheritdoc/>
    public string Name => "mcp";

    /// <inheritdoc/>
    public string Description =>
        "Query external MCP (Model Context Protocol) servers and the tools they provide for the silicon being. " +
        "Actions: " +
        "'status' (global overview: enabled state, server count, tool count); " +
        "'list_servers' (list configured servers with connection state and tool counts); " +
        "'list_tools' (list available tools with prefixed names 'mcp_{server}_{tool}', descriptions and parameter schemas; optional 'server_id' filters one server). " +
        "MCP tools are called directly by their prefixed name like any other tool.";

    /// <inheritdoc/>
    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

    /// <inheritdoc/>
    public Dictionary<string, object> GetParameterSchema()
    {
        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The action to perform: status, list_servers, list_tools",
                    ["enum"] = new[] { "status", "list_servers", "list_tools" }
                },
                ["server_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Server id (optional, filters list_tools to one server)"
                },
                ["include_schema"] = new Dictionary<string, object>
                {
                    ["type"] = "boolean",
                    ["description"] = "Include parameter schemas in list_tools output (optional, default false)"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    /// <inheritdoc/>
    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj) || actionObj == null)
        {
            return ToolResult.Failed("Missing required parameter 'action'");
        }

        string action = actionObj.ToString() ?? string.Empty;
        var manager = McpManager.Instance;

        if (!McpManager.McpEnabled)
        {
            return ToolResult.Successful("MCP integration is disabled (config: McpEnabled=false)");
        }

        switch (action)
        {
            case "status":
                return ExecuteStatus(manager);
            case "list_servers":
                return ExecuteListServers(manager);
            case "list_tools":
                string? serverId = parameters.TryGetValue("server_id", out object? serverObj) ? serverObj?.ToString() : null;
                bool includeSchema = parameters.TryGetValue("include_schema", out object? schemaObj)
                    && schemaObj.ToString() == "True";
                return ExecuteListTools(manager, callerId, serverId, includeSchema);
            default:
                return ToolResult.Failed($"Unknown action '{action}'");
        }
    }

    private static ToolResult ExecuteStatus(McpManager manager)
    {
        List<McpServerStatus> servers = manager.GetServerStatuses();
        int connected = servers.Count(s => s.State == McpConnectionState.Connected);
        string summary = $"MCP enabled. Servers: {servers.Count} (connected: {connected}). " +
            $"Total tools: {servers.Sum(s => s.ToolCount)}.";
        return ToolResult.Successful(summary);
    }

    private static ToolResult ExecuteListServers(McpManager manager)
    {
        List<McpServerStatus> servers = manager.GetServerStatuses();
        if (servers.Count == 0)
        {
            return ToolResult.Successful("No MCP servers configured. Servers are added by the user in the Web UI (/mcp).");
        }

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("MCP servers:");
        foreach (McpServerStatus server in servers)
        {
            sb.AppendLine($"- {server.Id} ({server.Name}): state={server.State.ToString().ToLowerInvariant()}, transport={server.Transport.ToString().ToLowerInvariant()}, tools={server.ToolCount}");
            if (!string.IsNullOrEmpty(server.LastError))
            {
                sb.AppendLine($"  last error: {server.LastError}");
            }
        }
        return ToolResult.Successful(sb.ToString().TrimEnd());
    }

    private static ToolResult ExecuteListTools(McpManager manager, Guid callerId, string? serverId, bool includeSchema)
    {
        var being = ServiceLocator.Instance.BeingManager?.GetBeing(callerId);
        List<McpWrappedTool> tools = manager.GetToolsForBeing(callerId, being?.ToolActionPermissions);
        if (tools.Count == 0)
        {
            return ToolResult.Successful("No MCP tools available for this being.");
        }

        if (!string.IsNullOrEmpty(serverId))
        {
            tools = tools.Where(t => t.Name.StartsWith($"mcp_{serverId}_", StringComparison.Ordinal)).ToList();
        }

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("MCP tools (call them by name like any other tool):");
        foreach (McpWrappedTool tool in tools)
        {
            sb.AppendLine($"- {tool.Name}: {tool.Description}");
            if (includeSchema)
            {
                string schemaJson = JsonSerializer.Serialize(tool.GetParameterSchema());
                sb.AppendLine($"  schema: {schemaJson}");
            }
        }
        return ToolResult.Successful(sb.ToString().TrimEnd());
    }
}
