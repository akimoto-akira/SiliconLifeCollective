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

using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SiliconLife.Collective;

/// <summary>
/// MCP connection over Streamable HTTP: each JSON-RPC request is one POST;
/// the response is either a plain application/json document or an SSE
/// stream (text/event-stream) whose data frames carry the JSON-RPC response.
/// The Mcp-Session-Id header returned by the server is replayed on
/// subsequent requests, per the Streamable HTTP transport spec.
/// </summary>
public sealed class HttpMcpClientConnection : McpClientConnection
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<HttpMcpClientConnection>();

    private readonly HttpClient _httpClient;
    private string? _sessionId;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpMcpClientConnection"/> class.
    /// </summary>
    /// <param name="serverConfig">The server configuration (url + headers).</param>
    public HttpMcpClientConnection(McpServerConfig serverConfig) : base(serverConfig)
    {
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(10), // per-request timeouts are enforced below
        };
    }

    /// <inheritdoc/>
    protected override JsonElement SendRequest(string method, object? payload, int timeoutSeconds)
    {
        if (_disposed)
        {
            throw new InvalidOperationException("Connection disposed");
        }

        if (string.IsNullOrWhiteSpace(ServerConfig.Url))
        {
            throw new InvalidOperationException("HTTP transport requires a url");
        }

        try
        {
            return SendRequestAsync(method, payload, timeoutSeconds).GetAwaiter().GetResult();
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            IsConnected = false;
            LastError = ex.Message;
            throw;
        }
    }

    private async Task<JsonElement> SendRequestAsync(string method, object? payload, int timeoutSeconds)
    {
        int id = NextRequestId();
        string json = BuildRequestJson(id, method, payload);

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
        using var request = new HttpRequestMessage(HttpMethod.Post, ServerConfig.Url);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        request.Headers.Accept.ParseAdd("application/json");
        request.Headers.Accept.ParseAdd("text/event-stream");

        foreach (var header in ServerConfig.Headers)
        {
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (_sessionId != null)
        {
            request.Headers.TryAddWithoutValidation("Mcp-Session-Id", _sessionId);
        }

        using HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cts.Token);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"HTTP {(int)response.StatusCode} {response.ReasonPhrase}");
        }

        // Capture the session id assigned by the server (first response wins)
        if (_sessionId == null && response.Headers.TryGetValues("Mcp-Session-Id", out var values))
        {
            string? session = values.FirstOrDefault();
            if (!string.IsNullOrEmpty(session))
            {
                _sessionId = session;
            }
        }

        string? mediaType = response.Content.Headers.ContentType?.MediaType;
        if (string.Equals(mediaType, "text/event-stream", StringComparison.OrdinalIgnoreCase))
        {
            return await ReadSseResponseAsync(response, id, timeoutSeconds, cts.Token);
        }

        string body = await response.Content.ReadAsStringAsync(cts.Token);
        if (string.IsNullOrWhiteSpace(body))
        {
            throw new InvalidOperationException("empty response body");
        }

        if (TryParseFrame(body, out int? frameId, out string? _, out JsonElement? result, out string? errorMessage))
        {
            if (errorMessage != null)
            {
                throw new InvalidOperationException(errorMessage);
            }
            if (frameId != id)
            {
                throw new InvalidOperationException($"response id mismatch (expected {id}, got {frameId})");
            }
            if (result != null)
            {
                return result.Value;
            }
        }

        throw new InvalidOperationException("invalid JSON-RPC response");
    }

    private async Task<JsonElement> ReadSseResponseAsync(
        HttpResponseMessage response,
        int requestId,
        int timeoutSeconds,
        CancellationToken ct)
    {
        var stopwatch = Stopwatch.StartNew();

        using Stream stream = await response.Content.ReadAsStreamAsync(ct);
        using var reader = new StreamReader(stream, Encoding.UTF8);

        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (stopwatch.Elapsed.TotalSeconds > timeoutSeconds)
            {
                throw new TimeoutException($"no matching SSE response within {timeoutSeconds}s");
            }

            if (!line.StartsWith("data:", StringComparison.Ordinal))
            {
                continue;
            }

            string payload = line["data:".Length..].Trim();
            if (payload.Length == 0 || payload == "[DONE]")
            {
                continue;
            }

            if (!TryParseFrame(payload, out int? id, out string? method, out JsonElement? result, out string? errorMessage))
            {
                continue;
            }

            if (id == requestId)
            {
                if (errorMessage != null)
                {
                    throw new InvalidOperationException(errorMessage);
                }
                if (result != null)
                {
                    return result.Value;
                }
            }
            else if (id == null && method == "notifications/tools/list_changed")
            {
                _logger.Info(null, "[Mcp] tools/list_changed notification from '{0}'", ServerConfig.Id);
                OnToolsChanged();
            }
            // other frames (server-initiated requests/notifications) are ignored
        }

        throw new InvalidOperationException("SSE stream ended without a matching response");
    }

    /// <inheritdoc/>
    protected override void SendNotification(string method)
    {
        // Fire-and-forget: notifications carry no id and expect no response.
        try
        {
            string json = BuildNotificationJson(method);
            using var request = new HttpRequestMessage(HttpMethod.Post, ServerConfig.Url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Headers.Accept.ParseAdd("application/json");

            foreach (var header in ServerConfig.Headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (_sessionId != null)
            {
                request.Headers.TryAddWithoutValidation("Mcp-Session-Id", _sessionId);
            }

            _httpClient.Send(request, HttpCompletionOption.ResponseContentRead);
        }
        catch (Exception ex)
        {
            _logger.Debug(null, "[Mcp] notification '{0}' to '{1}' failed: {2}", method, ServerConfig.Id, ex.Message);
        }
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;
        _httpClient.Dispose();
        IsConnected = false;
    }
}
