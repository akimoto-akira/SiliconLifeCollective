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

using System.Net.WebSockets;
using System.Text.Json;
using SiliconLife.Collective;
using SiliconLife.Default.Web;

namespace SiliconLife.Default.IM;

public class WebUIProvider : IIMProvider
{
    private readonly Router _router;
    private WebSocketHandler? _webSocketHandler;
    private readonly Dictionary<Guid, WebSocket> _userConnections = new();
    private readonly object _lock = new();
    private readonly Queue<(Guid senderId, Guid receiverId, string content)> _messageQueue = new();
    private readonly Dictionary<Guid, TaskCompletionSource<AskPermissionResult>> _pendingPermissionRequests = new();

    public event EventHandler<IMMessageEventArgs>? MessageReceived;
    public event EventHandler? ExitRequested;

    public Func<Guid, TaskCompletionSource<AskPermissionResult>> GetPermissionTcs => (Guid userId) =>
    {
        if (!_pendingPermissionRequests.TryGetValue(userId, out var tcs))
        {
            tcs = new TaskCompletionSource<AskPermissionResult>();
            _pendingPermissionRequests[userId] = tcs;
        }
        return tcs;
    };

    public WebUIProvider(Router router)
    {
        _router = router;
        InitializeWebSocket();
    }

    private void InitializeWebSocket()
    {
        _router.RegisterWebSocket("/ws", async (handler, message) =>
        {
            try
            {
                var wsMessage = JsonSerializer.Deserialize<WebSocketMessage>(message);
                if (wsMessage == null) return;

                switch (wsMessage.Type)
                {
                    case "connect":
                        var userId = wsMessage.SenderId;
                        if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out var guid))
                        {
                            foreach (var ws in _userConnections.Values)
                            {
                                if (ws.State == System.Net.WebSockets.WebSocketState.Open)
                                {
                                    var connectMsg = new WebSocketMessage
                                    {
                                        Type = "user_connected",
                                        Content = userId,
                                        Timestamp = DateTime.UtcNow
                                    };
                                    await handler.SendToAsync(ws, connectMsg.ToJson());
                                }
                            }
                        }
                        break;

                    case "chat":
                        if (!string.IsNullOrEmpty(wsMessage.SenderId) && !string.IsNullOrEmpty(wsMessage.ReceiverId))
                        {
                            if (Guid.TryParse(wsMessage.SenderId, out var sender) && Guid.TryParse(wsMessage.ReceiverId, out var receiver))
                            {
                                var chatMessage = new ChatMessage
                                {
                                    Id = Guid.NewGuid(),
                                    SenderId = sender,
                                    ReceiverId = receiver,
                                    Content = wsMessage.Content,
                                    Timestamp = DateTime.UtcNow,
                                    Type = MessageType.Text
                                };
                                MessageReceived?.Invoke(this, new IMMessageEventArgs(chatMessage));
                            }
                        }
                        break;

                    case "permission_response":
                        if (!string.IsNullOrEmpty(wsMessage.SenderId) && Guid.TryParse(wsMessage.SenderId, out var userGuid))
                        {
                            if (_pendingPermissionRequests.TryGetValue(userGuid, out var tcs))
                            {
                                var result = new AskPermissionResult
                                {
                                    Allowed = wsMessage.Content?.ToLower() == "allow"
                                };
                                tcs.SetResult(result);
                                _pendingPermissionRequests.Remove(userGuid);
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket message error: {ex.Message}");
            }
        });
    }

    public Task StartAsync()
    {
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        return Task.CompletedTask;
    }

    public async Task SendMessageAsync(Guid senderId, Guid receiverId, string content)
    {
        var message = new WebSocketMessage
        {
            Type = "chat",
            SenderId = senderId.ToString(),
            ReceiverId = receiverId.ToString(),
            Content = content,
            Timestamp = DateTime.UtcNow
        };

        if (_webSocketHandler != null)
        {
            await _webSocketHandler.SendToAllAsync(message.ToJson());
        }
    }

    public async Task<AskPermissionResult> AskPermissionAsync(PermissionType permissionType, string resource, string allowCode, string denyCode)
    {
        var message = new WebSocketMessage
        {
            Type = "permission_ask",
            Content = $"Permission required: {permissionType}\nResource: {resource}\nAllow code: {allowCode}\nDeny code: {denyCode}",
            Timestamp = DateTime.UtcNow
        };

        if (_webSocketHandler != null)
        {
            await _webSocketHandler.SendToAllAsync(message.ToJson());
        }

        var tcs = new TaskCompletionSource<AskPermissionResult>();
        var userId = Guid.Empty;
        _pendingPermissionRequests[userId] = tcs;

        try
        {
            var result = await tcs.Task.WaitAsync(TimeSpan.FromMinutes(1));
            return result;
        }
        catch (TimeoutException)
        {
            return new AskPermissionResult { Allowed = false };
        }
        finally
        {
            _pendingPermissionRequests.Remove(userId);
        }
    }

    public void AddConnection(Guid userId, System.Net.WebSockets.WebSocket webSocket)
    {
        lock (_lock)
        {
            _userConnections[userId] = webSocket;
        }
    }

    public void RemoveConnection(Guid userId)
    {
        lock (_lock)
        {
            _userConnections.Remove(userId);
        }
    }
}
