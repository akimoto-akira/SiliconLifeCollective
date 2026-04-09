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

using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace SiliconLife.Default.Web;

/// <summary>
/// Manages WebSocket connections with health-check (ping/pong) support.
/// Tracks last activity per client and periodically sends application-level
/// ping messages. Clients that remain inactive beyond the timeout are
/// automatically closed and removed.
/// </summary>
public class WebSocketHandler : IDisposable
{
    private readonly List<WebSocket> _clients = new();
    private readonly Dictionary<WebSocket, DateTime> _clientLastActivity = new();
    private readonly object _lock = new();
    private Timer? _healthCheckTimer;
    private TimeSpan _pingInterval = TimeSpan.FromSeconds(30);
    private TimeSpan _clientTimeout = TimeSpan.FromSeconds(60);
    private bool _disposed;

    public event Action<WebSocket>? OnConnected;
    public event Action<WebSocket, string>? OnMessageReceived;
    public event Action<WebSocket>? OnDisconnected;

    /// <summary>
    /// Starts the periodic health-check loop.
    /// On each tick the handler sends a ping to every client and closes
    /// any client whose last activity is older than <paramref name="clientTimeout"/>.
    /// </summary>
    /// <param name="pingInterval">How often to send pings (default 30 s)</param>
    /// <param name="clientTimeout">Inactivity duration before a client is evicted (default 60 s)</param>
    public void StartHealthCheck(TimeSpan? pingInterval = null, TimeSpan? clientTimeout = null)
    {
        _pingInterval = pingInterval ?? TimeSpan.FromSeconds(30);
        _clientTimeout = clientTimeout ?? TimeSpan.FromSeconds(60);
        _healthCheckTimer = new Timer(HealthCheckCallback, null, _pingInterval, _pingInterval);
    }

    /// <summary>
    /// Stops the health-check loop.
    /// </summary>
    public void StopHealthCheck()
    {
        _healthCheckTimer?.Dispose();
        _healthCheckTimer = null;
    }

    /// <summary>
    /// Updates the last-activity timestamp for a client.
    /// Called automatically on every received message; can also be called
    /// externally (e.g. when a pong is processed at the application layer).
    /// </summary>
    public void UpdateActivity(WebSocket client)
    {
        lock (_lock)
        {
            if (_clientLastActivity.ContainsKey(client))
            {
                _clientLastActivity[client] = DateTime.UtcNow;
            }
        }
    }

    private void HealthCheckCallback(object? state)
    {
        List<WebSocket> staleClients = new List<WebSocket>();
        DateTime now = DateTime.UtcNow;

        lock (_lock)
        {
            foreach (KeyValuePair<WebSocket, DateTime> kvp in _clientLastActivity)
            {
                if (now - kvp.Value > _clientTimeout)
                {
                    staleClients.Add(kvp.Key);
                }
            }
        }

        foreach (WebSocket client in staleClients)
        {
            try
            {
                if (client.State == WebSocketState.Open)
                {
                    client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Timeout", CancellationToken.None).Wait(TimeSpan.FromSeconds(5));
                }
            }
            catch
            {
            }
        }

        _ = SendPingToAllAsync();
    }

    private async Task SendPingToAllAsync()
    {
        WebSocketMessage pingMsg = new WebSocketMessage { Type = "ping" };
        await SendToAllAsync(pingMsg.ToJson());
    }

    public async Task HandleWebSocketRequest(HttpListenerContext context)
    {
        if (!context.Request.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            context.Response.Close();
            return;
        }

        WebSocket? webSocket = null;
        try
        {
            System.Net.WebSockets.WebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
            webSocket = wsContext.WebSocket;

            lock (_lock)
            {
                _clients.Add(webSocket);
                _clientLastActivity[webSocket] = DateTime.UtcNow;
            }

            OnConnected?.Invoke(webSocket);

            byte[] buffer = new byte[4096];
            ArraySegment<byte> segment = new ArraySegment<byte>(buffer);
            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(segment, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    break;
                }
                else if (result.MessageType == WebSocketMessageType.Text)
                {
                    lock (_lock)
                    {
                        _clientLastActivity[webSocket] = DateTime.UtcNow;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    OnMessageReceived?.Invoke(webSocket, message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WebSocket error: {ex.Message}");
        }
        finally
        {
            if (webSocket != null)
            {
                lock (_lock)
                {
                    _clients.Remove(webSocket);
                    _clientLastActivity.Remove(webSocket);
                }
                OnDisconnected?.Invoke(webSocket);
                webSocket.Dispose();
            }
        }
    }

    public async Task SendToAllAsync(string message)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(message);
        ArraySegment<byte> segment = new ArraySegment<byte>(bytes);

        List<WebSocket> clientsCopy;
        lock (_lock)
        {
            clientsCopy = new List<WebSocket>(_clients);
        }

        foreach (WebSocket client in clientsCopy)
        {
            if (client.State == WebSocketState.Open)
            {
                try
                {
                    await client.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch
                {
                }
            }
        }
    }

    public async Task SendToAsync(WebSocket client, string message)
    {
        if (client.State == WebSocketState.Open)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    public int ClientCount
    {
        get
        {
            lock (_lock)
            {
                return _clients.Count;
            }
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        StopHealthCheck();

        List<WebSocket> remaining;
        lock (_lock)
        {
            remaining = new List<WebSocket>(_clients);
            _clients.Clear();
            _clientLastActivity.Clear();
        }

        foreach (WebSocket client in remaining)
        {
            try
            {
                if (client.State == WebSocketState.Open)
                {
                    client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server shutting down", CancellationToken.None).Wait(TimeSpan.FromSeconds(3));
                }
            }
            catch
            {
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}

public class WebSocketMessage
{
    public string Type { get; set; } = "";
    public string Content { get; set; } = "";
    public string? SenderId { get; set; }
    public string? SenderName { get; set; }
    public string? ChannelId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Thinking content (chain-of-thought reasoning from the AI).
    /// </summary>
    public string? Thinking { get; set; }

    /// <summary>
    /// Stream ID for streaming messages.
    /// All chunks in the same stream share this ID.
    /// </summary>
    public Guid? StreamId { get; set; }

    /// <summary>
    /// Whether this is the final chunk in a stream.
    /// </summary>
    public bool? IsFinal { get; set; }

    public static WebSocketMessage FromJson(string json)
    {
        return System.Text.Json.JsonSerializer.Deserialize<WebSocketMessage>(json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new WebSocketMessage();
    }

    public string ToJson()
    {
        return System.Text.Json.JsonSerializer.Serialize(this, new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase });
    }
}
