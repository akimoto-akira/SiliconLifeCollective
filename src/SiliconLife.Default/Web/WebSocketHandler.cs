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

public class WebSocketHandler
{
    private readonly List<WebSocket> _clients = new();
    private readonly object _lock = new();

    public event Action<WebSocket>? OnConnected;
    public event Action<WebSocket, string>? OnMessageReceived;
    public event Action<WebSocket>? OnDisconnected;

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
            var wsContext = await context.AcceptWebSocketAsync(null);
            webSocket = wsContext.WebSocket;

            lock (_lock)
            {
                _clients.Add(webSocket);
            }

            OnConnected?.Invoke(webSocket);

            var buffer = new byte[4096];
            var segment = new ArraySegment<byte>(buffer);
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(segment, CancellationToken.None);
                
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    break;
                }
                else if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
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
                }
                OnDisconnected?.Invoke(webSocket);
                webSocket.Dispose();
            }
        }
    }

    public async Task SendToAllAsync(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        var segment = new ArraySegment<byte>(bytes);

        List<WebSocket> clientsCopy;
        lock (_lock)
        {
            clientsCopy = new List<WebSocket>(_clients);
        }

        foreach (var client in clientsCopy)
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
            var bytes = Encoding.UTF8.GetBytes(message);
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
}

public class WebSocketMessage
{
    public string Type { get; set; } = "";
    public string Content { get; set; } = "";
    public string? SenderId { get; set; }
    public string? ReceiverId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static WebSocketMessage FromJson(string json)
    {
        return System.Text.Json.JsonSerializer.Deserialize<WebSocketMessage>(json) ?? new WebSocketMessage();
    }

    public string ToJson()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}
