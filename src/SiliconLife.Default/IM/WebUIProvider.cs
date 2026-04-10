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
using System.Text;
using System.Text.Json;
using SiliconLife.Collective;
using SiliconLife.Default.Web;

namespace SiliconLife.Default.IM;

/// <summary>
/// Streaming buffer state for a single session.
/// Accumulates incremental content from AI streaming responses.
/// Cleared immediately when the stream ends to prevent duplication with persisted chat history.
/// </summary>
internal class StreamingBuffer
{
    public Guid StreamId { get; set; }
    public StringBuilder Content { get; } = new();
    public StringBuilder Thinking { get; } = new();
    public string? SenderName { get; set; }
    public Guid SenderId { get; set; }
    public bool IsActive { get; set; }

    public void Clear()
    {
        StreamId = Guid.Empty;
        Content.Clear();
        Thinking.Clear();
        SenderName = null;
        SenderId = Guid.Empty;
        IsActive = false;
    }
}

public class WebUIProvider : IIMProvider
{
    private readonly Router _router;
    private WebSocketHandler _webSocketHandler;
    private readonly Dictionary<Guid, WebSocket> _userConnections = new();
    private readonly object _lock = new();
    private readonly Queue<(Guid senderId, Guid receiverId, string content)> _messageQueue = new();
    private readonly Dictionary<Guid, TaskCompletionSource<AskPermissionResult>> _pendingPermissionRequests = new();
    private readonly Dictionary<Guid, StreamingBuffer> _streamingBuffers = new();
    private readonly object _streamingLock = new();

    public event EventHandler<IMMessageEventArgs>? MessageReceived;
    public event EventHandler<StreamChunkEventArgs>? StreamChunkReceived;
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
        _webSocketHandler = new WebSocketHandler();
        _webSocketHandler.OnMessageReceived += OnWebSocketMessageReceived;
        _router.SetSharedWebSocketHandler(_webSocketHandler);
        _webSocketHandler.StartHealthCheck();
    }

    private async void OnWebSocketMessageReceived(WebSocket ws, string message)
    {
        try
        {
            WebSocketMessage? wsMessage = JsonSerializer.Deserialize<WebSocketMessage>(message, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (wsMessage == null) return;

            switch (wsMessage.Type)
            {
                case "connect":
                    string? userId = wsMessage.SenderId;
                    if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid guid))
                    {
                        WebSocketMessage connectMsg = new WebSocketMessage
                        {
                            Type = "user_connected",
                            Content = userId,
                            Timestamp = DateTime.UtcNow
                        };
                        await _webSocketHandler.SendToAllAsync(connectMsg.ToJson());
                    }
                    break;

                case "chat":
                    if (!string.IsNullOrEmpty(wsMessage.SenderId) && !string.IsNullOrEmpty(wsMessage.ChannelId))
                    {
                        if (Guid.TryParse(wsMessage.SenderId, out Guid sender) && Guid.TryParse(wsMessage.ChannelId, out Guid channel))
                        {
                            ChatMessage chatMessage = new ChatMessage
                            {
                                Id = Guid.NewGuid(),
                                SenderId = sender,
                                ChannelId = channel,
                                Content = wsMessage.Content,
                                Timestamp = DateTime.Now,
                                Type = MessageType.Text
                            };
                            MessageReceived?.Invoke(this, new IMMessageEventArgs(chatMessage));
                        }
                    }
                    break;

                case "permission_response":
                    if (!string.IsNullOrEmpty(wsMessage.SenderId) && Guid.TryParse(wsMessage.SenderId, out Guid userGuid))
                    {
                        if (_pendingPermissionRequests.TryGetValue(userGuid, out TaskCompletionSource<AskPermissionResult>? tcs))
                        {
                            AskPermissionResult result = new AskPermissionResult
                            {
                                Allowed = wsMessage.Content?.ToLower() == "allow"
                            };
                            tcs.SetResult(result);
                            _pendingPermissionRequests.Remove(userGuid);
                        }
                    }
                    break;

                case "stream_chunk":
                    if (!string.IsNullOrEmpty(wsMessage.SenderId) && !string.IsNullOrEmpty(wsMessage.ChannelId))
                    {
                        if (Guid.TryParse(wsMessage.SenderId, out Guid streamSender) && Guid.TryParse(wsMessage.ChannelId, out Guid streamChannel))
                        {
                            StreamChunk? chunk = JsonSerializer.Deserialize<StreamChunk>(wsMessage.Content ?? "{}", new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            if (chunk != null)
                            {
                                StreamChunkReceived?.Invoke(this, new StreamChunkEventArgs(streamSender, streamChannel, chunk));
                            }
                        }
                    }
                    break;

                case "pong":
                    _webSocketHandler.UpdateActivity(ws);
                    break;

                case "poll_streaming":
                    if (!string.IsNullOrEmpty(wsMessage.ChannelId) && Guid.TryParse(wsMessage.ChannelId, out Guid pollSessionId))
                    {
                        StreamingBuffer? buffer = null;
                        lock (_streamingLock)
                        {
                            _streamingBuffers.TryGetValue(pollSessionId, out buffer);
                        }

                        WebSocketMessage streamingResponse = new WebSocketMessage
                        {
                            Type = "streaming_data",
                            ChannelId = pollSessionId.ToString(),
                            Timestamp = DateTime.UtcNow
                        };

                        if (buffer != null && buffer.IsActive)
                        {
                            streamingResponse.Content = buffer.Content.ToString();
                            streamingResponse.Thinking = buffer.Thinking.Length > 0 ? buffer.Thinking.ToString() : null;
                            streamingResponse.SenderId = buffer.SenderId.ToString();
                            streamingResponse.SenderName = buffer.SenderName;
                            streamingResponse.StreamId = buffer.StreamId;
                        }
                        else
                        {
                            streamingResponse.Content = string.Empty;
                        }

                        await _webSocketHandler.SendToAsync(ws, streamingResponse.ToJson());
                    }
                    break;

                case "poll_history":
                    if (!string.IsNullOrEmpty(wsMessage.ChannelId) && Guid.TryParse(wsMessage.ChannelId, out Guid historySessionId))
                    {
                        Guid? requestUserId = null;
                        if (!string.IsNullOrEmpty(wsMessage.SenderId) && Guid.TryParse(wsMessage.SenderId, out Guid parsedUserId))
                        {
                            requestUserId = parsedUserId;
                        }

                        ChatSystem? chatSystem = ServiceLocator.Instance.ChatSystem;
                        if (chatSystem != null)
                        {
                            ISession? session = chatSystem.GetSessionByChannelId(historySessionId);
                            if (session == null)
                            {
                                session = chatSystem.GetSession(historySessionId);
                            }

                            List<ChatMessage> messages = session?.GetMessages(0, 500) ?? new List<ChatMessage>();
                            string? afterId = wsMessage.Content;
                            List<ChatMessage> filtered = messages;
                            if (!string.IsNullOrEmpty(afterId) && Guid.TryParse(afterId, out Guid afterGuid))
                            {
                                int idx = messages.FindIndex(m => m.Id == afterGuid);
                                if (idx >= 0)
                                {
                                    filtered = messages.Skip(idx + 1).ToList();
                                }
                                else
                                {
                                    filtered = new List<ChatMessage>();
                                }
                            }

                            var historyData = filtered.Select(m =>
                            {
                                string roleStr;
                                if (m.Role != null)
                                {
                                    roleStr = m.Role.Value.ToString();
                                }
                                else if (requestUserId.HasValue && m.SenderId == requestUserId.Value)
                                {
                                    roleStr = "User";
                                }
                                else
                                {
                                    roleStr = "Assistant";
                                }
                                return new
                                {
                                    id = m.Id.ToString(),
                                    senderId = m.SenderId.ToString(),
                                    content = m.Content,
                                    thinking = m.Thinking,
                                    role = roleStr,
                                    timestamp = m.Timestamp
                                };
                            }).ToList();

                            WebSocketMessage historyResponse = new WebSocketMessage
                            {
                                Type = "history_data",
                                ChannelId = historySessionId.ToString(),
                                Content = JsonSerializer.Serialize(historyData),
                                Timestamp = DateTime.UtcNow
                            };

                            await _webSocketHandler.SendToAsync(ws, historyResponse.ToJson());
                        }
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WebSocket message error: {ex.Message}");
        }
    }

    public Task StartAsync()
    {
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        _webSocketHandler.Dispose();
        return Task.CompletedTask;
    }

    public async Task SendMessageAsync(Guid senderId, Guid channelId, string content, string? thinking = null, string? senderName = null)
    {
        WebSocketMessage message = new WebSocketMessage
        {
            Type = "chat",
            SenderId = senderId.ToString(),
            SenderName = senderName,
            ChannelId = channelId.ToString(),
            Content = content,
            Thinking = thinking,
            Timestamp = DateTime.Now
        };

        await _webSocketHandler.SendToAllAsync(message.ToJson());
    }

    public async Task SendStreamChunkAsync(Guid senderId, Guid channelId, StreamChunk chunk)
    {
        string accumulatedContent = string.Empty;
        string? accumulatedThinking = null;

        lock (_streamingLock)
        {
            if (!_streamingBuffers.TryGetValue(channelId, out StreamingBuffer? buffer))
            {
                buffer = new StreamingBuffer();
                _streamingBuffers[channelId] = buffer;
            }

            if (!buffer.IsActive || buffer.StreamId != chunk.StreamId)
            {
                buffer.Clear();
                buffer.StreamId = chunk.StreamId;
                buffer.SenderId = senderId;
                buffer.SenderName = null;
                buffer.IsActive = true;
            }

            if (!string.IsNullOrEmpty(chunk.Content))
            {
                buffer.Content.Append(chunk.Content);
            }

            if (!string.IsNullOrEmpty(chunk.Thinking))
            {
                buffer.Thinking.Append(chunk.Thinking);
            }

            accumulatedContent = buffer.Content.ToString();
            accumulatedThinking = buffer.Thinking.Length > 0 ? buffer.Thinking.ToString() : null;

            if (chunk.IsFinal)
            {
                buffer.Clear();
            }
        }

        WebSocketMessage message = new WebSocketMessage
        {
            Type = "streaming_data",
            SenderId = senderId.ToString(),
            ChannelId = channelId.ToString(),
            Content = accumulatedContent,
            Thinking = accumulatedThinking,
            StreamId = chunk.StreamId,
            IsFinal = chunk.IsFinal,
            Timestamp = DateTime.UtcNow
        };

        await _webSocketHandler.SendToAllAsync(message.ToJson());
    }

    public async Task<AskPermissionResult> AskPermissionAsync(PermissionType permissionType, string resource, string allowCode, string denyCode)
    {
        WebSocketMessage message = new WebSocketMessage
        {
            Type = "permission_ask",
            Content = $"Permission required: {permissionType}\nResource: {resource}\nAllow code: {allowCode}\nDeny code: {denyCode}",
            Timestamp = DateTime.UtcNow
        };

        await _webSocketHandler.SendToAllAsync(message.ToJson());

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