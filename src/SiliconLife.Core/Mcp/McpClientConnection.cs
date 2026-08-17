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
/// Abstract JSON-RPC 2.0 connection to an external MCP server.
/// Implements the minimal protocol subset needed for tools:
/// initialize → notifications/initialized → tools/list → tools/call.
/// Transport subclasses implement <see cref="SendRequest"/> (request/response)
/// and <see cref="SendNotification"/>.
/// </summary>
public abstract class McpClientConnection : IDisposable
{
    /// <summary>The MCP protocol version requested during initialize.</summary>
    public const string ProtocolVersion = "2025-06-18";

    private static readonly ILogger _logger = LogManager.Instance.GetLogger<McpClientConnection>();

    private int _nextRequestId;

    /// <summary>
    /// Initializes a new instance of the <see cref="McpClientConnection"/> class.
    /// </summary>
    /// <param name="serverConfig">The server configuration (endpoint, headers, etc.).</param>
    protected McpClientConnection(McpServerConfig serverConfig)
    {
        ServerConfig = serverConfig;
    }

    /// <summary>Gets the server configuration this connection was created from.</summary>
    protected McpServerConfig ServerConfig { get; }

    /// <summary>Gets a value indicating whether the initialize handshake completed.</summary>
    public bool IsConnected { get; protected set; }

    /// <summary>Gets the last error message, if any.</summary>
    public string? LastError { get; protected set; }

    /// <summary>
    /// Event raised when the server sends the tools/list_changed notification.
    /// The manager reacts by re-fetching tools/list.
    /// </summary>
    public event Action? ToolsChanged;

    /// <summary>
    /// Fires the <see cref="ToolsChanged"/> event. Called by subclasses when
    /// a tools/list_changed notification arrives.
    /// </summary>
    protected void OnToolsChanged() => ToolsChanged?.Invoke();

    /// <summary>
    /// Sends a JSON-RPC request and returns the "result" object of the response.
    /// Throws <see cref="InvalidOperationException"/> on transport failure,
    /// JSON-RPC error response, or timeout.
    /// </summary>
    /// <param name="method">The JSON-RPC method name.</param>
    /// <param name="payload">The params object, or null.</param>
    /// <param name="timeoutSeconds">The response timeout in seconds.</param>
    /// <returns>The result JsonElement of the response.</returns>
    protected abstract JsonElement SendRequest(string method, object? payload, int timeoutSeconds);

    /// <summary>Sends a JSON-RPC notification (no response expected).</summary>
    /// <param name="method">The notification method name.</param>
    protected abstract void SendNotification(string method);

    /// <summary>
    /// Performs the MCP initialize handshake and loads the tool list.
    /// </summary>
    /// <returns>True when the handshake and tools/list both succeeded.</returns>
    public bool Initialize()
    {
        try
        {
            var initParams = new Dictionary<string, object>
            {
                ["protocolVersion"] = ProtocolVersion,
                ["capabilities"] = new Dictionary<string, object>(),
                ["clientInfo"] = new Dictionary<string, object>
                {
                    ["name"] = "SiliconLifeCollective",
                    ["version"] = "0.2.0",
                },
            };

            JsonElement result = SendRequest("initialize", initParams, 30);
            SendNotification("notifications/initialized");

            IsConnected = true;
            LastError = null;
            _logger.Info(null, "[Mcp] server initialized: {0} ({1})", ServerConfig.Id, ServerConfig.Transport);
            return true;
        }
        catch (Exception ex)
        {
            IsConnected = false;
            LastError = ex.Message;
            _logger.Warn(null, "[Mcp] initialize failed for '{0}': {1}", ServerConfig.Id, ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Fetches the full tool list (follows nextCursor pagination).
    /// </summary>
    /// <returns>The tool definitions reported by the server.</returns>
    public List<McpToolDefinition> ListTools()
    {
        var tools = new List<McpToolDefinition>();
        string? cursor = null;
        int pages = 0;

        do
        {
            var payload = new Dictionary<string, object>();
            if (cursor != null)
            {
                payload["cursor"] = cursor;
            }

            JsonElement result = SendRequest("tools/list", payload, 30);
            if (result.ValueKind == JsonValueKind.Object && result.TryGetProperty("tools", out var toolsElem)
                && toolsElem.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement tool in toolsElem.EnumerateArray())
                {
                    McpToolDefinition? definition = ParseTool(tool);
                    if (definition != null)
                    {
                        tools.Add(definition);
                    }
                }
            }

            cursor = result.ValueKind == JsonValueKind.Object
                && result.TryGetProperty("nextCursor", out var cursorElem)
                && cursorElem.ValueKind == JsonValueKind.String
                ? cursorElem.GetString()
                : null;
            pages++;
        }
        while (cursor != null && pages < 20);

        return tools;
    }

    /// <summary>
    /// Calls a tool on the server. Arguments must already contain native
    /// .NET values (they are serialized as the "arguments" object).
    /// </summary>
    /// <param name="name">The original tool name on the server.</param>
    /// <param name="arguments">The tool arguments.</param>
    /// <param name="timeoutSeconds">The call timeout in seconds.</param>
    /// <returns>The call result with flattened text content.</returns>
    public McpCallResult CallTool(string name, Dictionary<string, object> arguments, int timeoutSeconds)
    {
        try
        {
            var payload = new Dictionary<string, object>
            {
                ["name"] = name,
                ["arguments"] = arguments,
            };

            JsonElement result = SendRequest("tools/call", payload, timeoutSeconds);
            return ParseCallResult(result);
        }
        catch (Exception ex)
        {
            return McpCallResult.Fail($"MCP call '{name}' failed: {ex.Message}");
        }
    }

    /// <summary>Releases the connection (subprocess / HTTP session).</summary>
    public abstract void Dispose();

    /// <summary>Builds the JSON-RPC request document for the given method.</summary>
    /// <param name="id">The request id.</param>
    /// <param name="method">The method name.</param>
    /// <param name="payload">The params object, or null.</param>
    /// <returns>The serialized request JSON (one line, no trailing newline).</returns>
    protected static string BuildRequestJson(int id, string method, object? payload)
    {
        var request = new Dictionary<string, object>
        {
            ["jsonrpc"] = "2.0",
            ["id"] = id,
            ["method"] = method,
        };
        if (payload != null)
        {
            request["params"] = payload;
        }
        return JsonSerializer.Serialize(request);
    }

    /// <summary>Builds the JSON-RPC notification document for the given method.</summary>
    /// <param name="method">The notification method name.</param>
    /// <returns>The serialized notification JSON (one line, no trailing newline).</returns>
    protected static string BuildNotificationJson(string method)
    {
        return JsonSerializer.Serialize(new Dictionary<string, object>
        {
            ["jsonrpc"] = "2.0",
            ["method"] = method,
        });
    }

    /// <summary>Allocates the next request id.</summary>
    /// <returns>A unique request id for this connection.</returns>
    protected int NextRequestId()
    {
        return Interlocked.Increment(ref _nextRequestId);
    }

    /// <summary>
    /// Parses one incoming JSON-RPC message frame. Exactly one of the outputs
    /// is populated: response (id + result or error) or notification (method only).
    /// </summary>
    /// <param name="line">The raw JSON text of the frame.</param>
    /// <param name="id">The response id, or null for notifications.</param>
    /// <param name="method">The notification method, or null for responses.</param>
    /// <param name="result">The result element of a successful response.</param>
    /// <param name="errorMessage">The error message of a failed response.</param>
    /// <returns>True when the line was a valid JSON object frame.</returns>
    protected static bool TryParseFrame(
        string line,
        out int? id,
        out string? method,
        out JsonElement? result,
        out string? errorMessage)
    {
        id = null;
        method = null;
        result = null;
        errorMessage = null;

        JsonElement frame;
        try
        {
            frame = JsonSerializer.Deserialize<JsonElement>(line);
        }
        catch
        {
            return false;
        }

        if (frame.ValueKind != JsonValueKind.Object)
        {
            return false;
        }

        if (frame.TryGetProperty("id", out var idElem) &&
            (idElem.ValueKind == JsonValueKind.Number || idElem.ValueKind == JsonValueKind.String))
        {
            id = idElem.ValueKind == JsonValueKind.Number ? idElem.GetInt32() : int.TryParse(idElem.GetString(), out int parsed) ? parsed : null;
        }

        if (frame.TryGetProperty("method", out var methodElem) && methodElem.ValueKind == JsonValueKind.String)
        {
            method = methodElem.GetString();
            return true;
        }

        if (frame.TryGetProperty("error", out var errorElem) && errorElem.ValueKind == JsonValueKind.Object)
        {
            string message = errorElem.TryGetProperty("message", out var messageElem) && messageElem.ValueKind == JsonValueKind.String
                ? messageElem.GetString() ?? "unknown error"
                : "unknown error";
            int code = errorElem.TryGetProperty("code", out var codeElem) && codeElem.ValueKind == JsonValueKind.Number
                ? codeElem.GetInt32()
                : 0;
            errorMessage = $"JSON-RPC error {code}: {message}";
            return true;
        }

        if (frame.TryGetProperty("result", out var resultElem))
        {
            result = resultElem;
            return true;
        }

        return false;
    }

    /// <summary>Extracts the error message of a response frame, or null.</summary>
    /// <param name="id">The frame id.</param>
    /// <param name="result">The frame result.</param>
    /// <param name="errorMessage">The frame error message.</param>
    /// <returns>The error message when this frame is an error response, otherwise null.</returns>
    protected static string? ResponseError(int? id, JsonElement? result, string? errorMessage)
    {
        if (id != null && errorMessage != null)
        {
            return errorMessage;
        }
        return null;
    }

    private static McpToolDefinition? ParseTool(JsonElement tool)
    {
        if (tool.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        string? name = tool.TryGetProperty("name", out var nameElem) && nameElem.ValueKind == JsonValueKind.String
            ? nameElem.GetString()
            : null;
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        string description = tool.TryGetProperty("description", out var descElem) && descElem.ValueKind == JsonValueKind.String
            ? descElem.GetString() ?? string.Empty
            : string.Empty;

        var schema = new Dictionary<string, object>();
        if (tool.TryGetProperty("inputSchema", out var schemaElem) && schemaElem.ValueKind == JsonValueKind.Object)
        {
            schema = ConvertToObject(schemaElem);
        }
        else
        {
            // Servers must report a schema; fall back to an empty object schema
            schema["type"] = "object";
            schema["properties"] = new Dictionary<string, object>();
        }

        return new McpToolDefinition
        {
            Name = name,
            Description = description,
            InputSchema = schema,
        };
    }

    private static McpCallResult ParseCallResult(JsonElement result)
    {
        if (result.ValueKind != JsonValueKind.Object)
        {
            return McpCallResult.Fail("Invalid tools/call result (not an object)");
        }

        bool isError = result.TryGetProperty("isError", out var isErrorElem)
            && isErrorElem.ValueKind == JsonValueKind.True;

        var textParts = new List<string>();
        if (result.TryGetProperty("content", out var contentElem) && contentElem.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement item in contentElem.EnumerateArray())
            {
                if (item.ValueKind != JsonValueKind.Object)
                {
                    continue;
                }

                string type = item.TryGetProperty("type", out var typeElem) && typeElem.ValueKind == JsonValueKind.String
                    ? typeElem.GetString() ?? "text"
                    : "text";

                if (type == "text" && item.TryGetProperty("text", out var textElem) && textElem.ValueKind == JsonValueKind.String)
                {
                    textParts.Add(textElem.GetString() ?? string.Empty);
                }
                else if (type == "image")
                {
                    textParts.Add("[image content]");
                }
                else if (type == "resource")
                {
                    textParts.Add("[resource content]");
                }
            }
        }

        string text = string.Join("\n", textParts);
        return isError
            ? McpCallResult.Fail(string.IsNullOrEmpty(text) ? "MCP tool reported an error" : text)
            : McpCallResult.Ok(string.IsNullOrEmpty(text) ? "(empty result)" : text);
    }

    /// <summary>Recursively converts a JsonElement into native .NET dictionary/list values.</summary>
    /// <param name="element">The element to convert.</param>
    /// <returns>The native representation (Dictionary/List/string/number/bool).</returns>
    protected static Dictionary<string, object> ConvertToObject(JsonElement element)
    {
        var result = new Dictionary<string, object>();
        foreach (JsonProperty property in element.EnumerateObject())
        {
            object? value = ConvertValue(property.Value);
            if (value != null)
            {
                result[property.Name] = value;
            }
        }
        return result;
    }

    private static object? ConvertValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.TryGetInt32(out int i) ? i
                : element.TryGetInt64(out long l) ? l : element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            JsonValueKind.Array => element.EnumerateArray().Select(ConvertValue).Where(v => v != null).ToList(),
            JsonValueKind.Object => ConvertToObject(element),
            _ => element.ToString(),
        };
    }
}
