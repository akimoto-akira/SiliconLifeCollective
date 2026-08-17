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
using System.Text.Json.Serialization;

using SiliconLife.Collective;
using SiliconLife.App.Web;

namespace SiliconLife.App.Web.Controllers;

/// <summary>
/// Web controller for the MCP server management page and REST API.
/// Server configurations are global (shared by all beings); the page is
/// reached from the beings list, so the beingId parameter is kept for
/// navigation context. Adding/removing/toggling servers takes effect
/// immediately: the McpManager reconnects and re-registers the tools of
/// every being.
/// </summary>
[WebCode]
public class McpController : Controller
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<McpController>();
    private readonly SkinManager _skinManager;
    private readonly SiliconBeingManager _beingManager;

    private static readonly JsonSerializerOptions McpJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    public McpController()
    {
        _skinManager = ServiceLocator.Instance.GetService<SkinManager>()!;
        _beingManager = ServiceLocator.Instance.BeingManager!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/mcp";

        if (path == "/mcp" || path == "/mcp/index")
            Index();
        else if (path == "/api/mcp/list-servers")
            ListServers();
        else if (path == "/api/mcp/list-tools")
            ListTools();
        else if (path == "/api/mcp/add-server")
            AddServer();
        else if (path == "/api/mcp/toggle")
            Toggle();
        else if (path == "/api/mcp/remove-server")
            RemoveServer();
        else if (path == "/api/mcp/reconnect")
            Reconnect();
        else if (path == "/api/mcp/test-tool")
            TestTool();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        Guid beingId = Guid.TryParse(GetQueryValue("beingId"), out Guid parsed) ? parsed : Guid.Empty;
        string beingName = string.Empty;
        if (beingId != Guid.Empty)
        {
            beingName = _beingManager.GetBeing(beingId)?.Name ?? string.Empty;
        }

        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.McpView();
        var vm = new Models.McpViewModel
        {
            Skin = skin,
            ActiveMenu = "beings",
            BeingId = beingId,
            BeingName = beingName,
            McpEnabled = McpManager.McpEnabled,
        };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void ListServers()
    {
        try
        {
            var manager = McpManager.Instance;
            List<McpServerStatus> statuses = manager.GetServerStatuses();

            var servers = statuses
                .OrderBy(s => s.Id, StringComparer.Ordinal)
                .Select(s => new
                {
                    id = s.Id,
                    name = s.Name,
                    transport = s.Transport.ToString().ToLowerInvariant(),
                    state = s.State.ToString().ToLowerInvariant(),
                    enabled = s.Enabled,
                    toolCount = s.ToolCount,
                    endpoint = s.Endpoint,
                    lastError = s.LastError,
                })
                .ToList();

            RenderJson(new
            {
                success = true,
                data = servers,
                mcpEnabled = McpManager.McpEnabled,
                connected = statuses.Count(s => s.State == McpConnectionState.Connected),
                toolTotal = statuses.Sum(s => s.ToolCount),
            });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void ListTools()
    {
        string serverId = GetQueryValue("serverId");
        if (string.IsNullOrWhiteSpace(serverId))
        {
            RenderJson(new { success = false, error = "Missing serverId parameter" });
            return;
        }

        try
        {
            McpClientConnection? connection = McpManager.Instance.GetConnection(serverId);
            if (connection == null)
            {
                RenderJson(new { success = false, error = $"Server '{serverId}' is not connected" });
                return;
            }

            List<McpToolDefinition> tools = connection.ListTools();
            var data = tools
                .Select(t => new
                {
                    name = $"mcp_{serverId}_{t.Name}",
                    description = t.Description,
                    schema = t.InputSchema,
                })
                .ToList();

            RenderJson(new { success = true, data });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void AddServer()
    {
        try
        {
            McpServerConfig? config;
            try
            {
                string body = GetRequestBody();
                config = JsonSerializer.Deserialize<McpServerConfig>(body, McpJsonOptions);
            }
            catch (JsonException)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            if (config == null)
            {
                RenderJson(new { success = false, error = "Missing server configuration" });
                return;
            }

            string? error = McpManager.Instance.AddServer(config);
            if (error != null)
            {
                RenderJson(new { success = false, error });
                return;
            }

            _logger.Info(null, "[Mcp] server added via Web UI: {0}", config.Id);
            RenderJson(new { success = true, data = new { serverId = config.Id, message = $"Server '{config.Id}' added" } });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void Toggle()
    {
        try
        {
            var body = GetJsonBody<ToggleBody>();
            if (body == null || string.IsNullOrWhiteSpace(body.ServerId))
            {
                RenderJson(new { success = false, error = "Missing 'serverId' in request body" });
                return;
            }

            string? error = McpManager.Instance.ToggleServer(body.ServerId, body.Enabled);
            if (error != null)
            {
                RenderJson(new { success = false, error });
                return;
            }

            RenderJson(new { success = true, data = new { message = $"Server '{body.ServerId}' {(body.Enabled ? "enabled" : "disabled")}" } });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void RemoveServer()
    {
        try
        {
            var body = GetJsonBody<ServerIdBody>();
            if (body == null || string.IsNullOrWhiteSpace(body.ServerId))
            {
                RenderJson(new { success = false, error = "Missing 'serverId' in request body" });
                return;
            }

            string? error = McpManager.Instance.RemoveServer(body.ServerId);
            if (error != null)
            {
                RenderJson(new { success = false, error });
                return;
            }

            RenderJson(new { success = true, data = new { message = $"Server '{body.ServerId}' removed" } });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void Reconnect()
    {
        try
        {
            var body = GetJsonBody<ServerIdBody>();
            if (body == null || string.IsNullOrWhiteSpace(body.ServerId))
            {
                RenderJson(new { success = false, error = "Missing 'serverId' in request body" });
                return;
            }

            string? error = McpManager.Instance.ReconnectServer(body.ServerId);
            if (error != null)
            {
                RenderJson(new { success = false, error });
                return;
            }

            RenderJson(new { success = true, data = new { message = $"Server '{body.ServerId}' reconnected" } });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void TestTool()
    {
        try
        {
            var body = GetJsonBody<TestBody>();
            if (body == null || string.IsNullOrWhiteSpace(body.ServerId) || string.IsNullOrWhiteSpace(body.ToolName))
            {
                RenderJson(new { success = false, error = "Missing 'serverId' or 'toolName' in request body" });
                return;
            }

            Dictionary<string, object> arguments = new(StringComparer.Ordinal);
            if (!string.IsNullOrWhiteSpace(body.ArgumentsJson))
            {
                try
                {
                    var parsed = JsonSerializer.Deserialize<Dictionary<string, object>>(body.ArgumentsJson, McpJsonOptions);
                    if (parsed != null)
                    {
                        arguments = new Dictionary<string, object>(parsed, StringComparer.Ordinal);
                    }
                }
                catch (JsonException)
                {
                    RenderJson(new { success = false, error = "Invalid arguments JSON" });
                    return;
                }
            }

            McpCallResult result = McpManager.Instance.TestTool(body.ServerId, body.ToolName, arguments);
            if (result.IsSuccess)
            {
                RenderJson(new { success = true, data = new { message = result.TextContent } });
            }
            else
            {
                RenderJson(new { success = false, error = result.ErrorMessage ?? "MCP tool call failed" });
            }
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    // ===== Request bodies =====

    private class ToggleBody
    {
        public string ServerId { get; set; } = string.Empty;
        public bool Enabled { get; set; }
    }

    private class ServerIdBody
    {
        public string ServerId { get; set; } = string.Empty;
    }

    private class TestBody
    {
        public string ServerId { get; set; } = string.Empty;
        public string ToolName { get; set; } = string.Empty;
        public string? ArgumentsJson { get; set; }
    }
}
