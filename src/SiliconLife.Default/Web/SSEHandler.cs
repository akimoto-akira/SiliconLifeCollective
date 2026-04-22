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
using System.Text;
using System.Text.Json;
using SiliconLife.Collective;

namespace SiliconLife.Default.Web;

/// <summary>
/// Manages Server-Sent Events (SSE) connections.
/// Provides real-time push from server to client without polling.
/// </summary>
public class SSEHandler : IDisposable
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<SSEHandler>();
    private readonly List<SSEClient> _clients = new();
    private readonly object _lock = new();
    private Timer? _heartbeatTimer;
    private bool _disposed;

    public event Action<SSEClient>? OnConnected;
    public event Action<SSEClient>? OnDisconnected;

    public SSEHandler()
    {
        _heartbeatTimer = new Timer(SendHeartbeat, null, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(15));
    }

    private void SendHeartbeat(object? state)
    {
        List<SSEClient> clientsCopy;
        lock (_lock)
        {
            clientsCopy = new List<SSEClient>(_clients);
        }

        foreach (SSEClient client in clientsCopy)
        {
            try
            {
                if (client.IsClosed)
                {
                    RemoveClient(client);
                    continue;
                }

                client.SendEvent("heartbeat", new { timestamp = DateTime.UtcNow });
            }
            catch
            {
                RemoveClient(client);
            }
        }
    }

    public async Task HandleSSERequest(HttpListenerContext context, Guid userId, Guid? channelId)
    {
        HttpListenerResponse response = context.Response;
        response.ContentType = "text/event-stream";
        response.Headers["Cache-Control"] = "no-cache";
        response.Headers["Connection"] = "keep-alive";
        response.Headers["Access-Control-Allow-Origin"] = "*";

        SSEClient client = new SSEClient(userId, channelId, response);
        
        lock (_lock)
        {
            _clients.Add(client);
        }

        _logger.Info(null, $"SSE connected: userId={userId}, channelId={channelId}");
        OnConnected?.Invoke(client);

        try
        {
            byte[] buffer = Encoding.UTF8.GetBytes(": connected\n\n");
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            await response.OutputStream.FlushAsync();

            while (!client.IsClosed && !_disposed)
            {
                await Task.Delay(100);
            }
        }
        catch (HttpListenerException)
        {
        }
        catch (Exception ex)
        {
            _logger.Error(null, $"SSE error: {ex.Message}");
        }
        finally
        {
            RemoveClient(client);
        }
    }

    private void RemoveClient(SSEClient client)
    {
        lock (_lock)
        {
            _clients.Remove(client);
        }
        _logger.Info(null, $"SSE disconnected: userId={client.UserId}");
        OnDisconnected?.Invoke(client);
    }

    public async Task SendToUserAsync(Guid userId, string eventType, object data)
    {
        List<SSEClient> targets;
        lock (_lock)
        {
            targets = _clients.Where(c => c.UserId == userId).ToList();
        }

        foreach (SSEClient client in targets)
        {
            try
            {
                await client.SendEventAsync(eventType, data);
            }
            catch
            {
                RemoveClient(client);
            }
        }
    }

    public async Task SendToChannelAsync(Guid channelId, string eventType, object data)
    {
        List<SSEClient> targets;
        lock (_lock)
        {
            targets = _clients.Where(c => c.ChannelId == channelId).ToList();
        }

        foreach (SSEClient client in targets)
        {
            try
            {
                await client.SendEventAsync(eventType, data);
            }
            catch
            {
                RemoveClient(client);
            }
        }
    }

    public async Task SendToAllAsync(string eventType, object data)
    {
        List<SSEClient> clientsCopy;
        lock (_lock)
        {
            clientsCopy = new List<SSEClient>(_clients);
        }

        foreach (SSEClient client in clientsCopy)
        {
            try
            {
                await client.SendEventAsync(eventType, data);
            }
            catch
            {
                RemoveClient(client);
            }
        }
    }

    public async Task SendHistoryAsync(SSEClient client, List<ChatMessage> messages, Guid requestUserId)
    {
        var beingManager = ServiceLocator.Instance.BeingManager;
        var beingDict = beingManager?.GetAllBeings()?.ToDictionary(b => b.Id);

        var historyData = messages.Select(m =>
        {
            string roleStr = m.Role.ToString();
            string senderName = "";
            if (beingDict != null && m.SenderId != requestUserId && beingDict.TryGetValue(m.SenderId, out var b))
            {
                senderName = b.Name ?? "";
            }
            return new
            {
                id = m.Id.ToString(),
                senderId = m.SenderId.ToString(),
                content = m.Content,
                thinking = m.Thinking,
                role = roleStr,
                senderName,
                timestamp = m.Timestamp,
                toolCallsJson = m.ToolCallsJson,
                toolCallId = m.ToolCallId,
                promptTokens = m.PromptTokens,
                completionTokens = m.CompletionTokens,
                totalTokens = m.TotalTokens
            };
        }).ToList();

        await client.SendEventAsync("history", new { messages = historyData });
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

    public SSEClient? GetClient(Guid userId)
    {
        lock (_lock)
        {
            return _clients.FirstOrDefault(c => c.UserId == userId);
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _heartbeatTimer?.Dispose();

        List<SSEClient> remaining;
        lock (_lock)
        {
            remaining = new List<SSEClient>(_clients);
            _clients.Clear();
        }

        foreach (SSEClient client in remaining)
        {
            try
            {
                client.Close();
            }
            catch
            {
            }
        }
    }
}

/// <summary>
/// Represents a single SSE client connection.
/// </summary>
public class SSEClient
{
    public Guid UserId { get; }
    public Guid? ChannelId { get; set; }
    public HttpListenerResponse Response { get; }
    private readonly object _lock = new();
    private bool _isClosed;

    public SSEClient(Guid userId, Guid? channelId, HttpListenerResponse response)
    {
        UserId = userId;
        ChannelId = channelId;
        Response = response;
        _isClosed = false;
    }

    public bool IsClosed => _isClosed;

    public void SendEvent(string eventType, object data)
    {
        lock (_lock)
        {
            if (_isClosed) return;

            try
            {
                string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                string message = $"event: {eventType}\ndata: {json}\n\n";
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                Response.OutputStream.Write(buffer, 0, buffer.Length);
                Response.OutputStream.Flush();
            }
            catch
            {
                _isClosed = true;
            }
        }
    }

    public async Task SendEventAsync(string eventType, object data)
    {
        await Task.Run(() => SendEvent(eventType, data));
    }

    public void Close()
    {
        lock (_lock)
        {
            if (_isClosed) return;
            _isClosed = true;
        }

        try
        {
            Response.Close();
        }
        catch
        {
        }
    }
}
