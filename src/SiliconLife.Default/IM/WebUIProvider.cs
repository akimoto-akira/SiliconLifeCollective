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

using System.Text;
using System.Text.Json;
using SiliconLife.Collective;
using SiliconLife.Default.Web;

namespace SiliconLife.Default.IM;

/// <summary>
/// Streaming buffer state for a single session.
/// Accumulates incremental content from AI streaming responses.
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
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<WebUIProvider>();
    private readonly Router _router;
    private readonly SSEHandler _sseHandler;
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

    public SSEHandler SSEHandler => _sseHandler;

    public WebUIProvider(Router router)
    {
        _router = router;
        _sseHandler = new SSEHandler();
        _sseHandler.OnConnected += OnSSEConnected;
        _sseHandler.OnDisconnected += OnSSEDisconnected;
        _router.SetSharedSSEHandler(_sseHandler);
        _logger.Info("WebUIProvider initialized with SSE");
    }

    private void OnSSEConnected(SSEClient client)
    {
        _logger.Info($"SSE client connected: userId={client.UserId}, channelId={client.ChannelId}");

        if (client.ChannelId.HasValue)
        {
            _ = SendHistoryToClientAsync(client);
        }
    }

    private void OnSSEDisconnected(SSEClient client)
    {
        _logger.Info($"SSE client disconnected: userId={client.UserId}");
    }

    private async Task SendHistoryToClientAsync(SSEClient client)
    {
        if (!client.ChannelId.HasValue)
        {
            _logger.Debug("SendHistoryToClientAsync: channelId is null");
            return;
        }

        ChatSystem? chatSystem = ServiceLocator.Instance.ChatSystem;
        if (chatSystem == null)
        {
            _logger.Debug("SendHistoryToClientAsync: chatSystem is null");
            return;
        }

        _logger.Info($"SendHistoryToClientAsync: looking for session with channelId={client.ChannelId.Value}");
        SessionBase? session = chatSystem.GetSessionByChannelId(client.ChannelId.Value);
        if (session == null)
        {
            _logger.Debug("SendHistoryToClientAsync: GetSessionByChannelId returned null, trying GetSession");
            session = chatSystem.GetSession(client.ChannelId.Value);
        }

        if (session == null)
        {
            _logger.Warn($"SendHistoryToClientAsync: session not found for channelId={client.ChannelId.Value}");
            return;
        }

        List<ChatMessage> messages = session.GetMessages(0, 500);
        _logger.Info($"SendHistoryToClientAsync: found {messages.Count} messages for session {session.Id}");
        await _sseHandler.SendHistoryAsync(client, messages, client.UserId);
    }

    public void HandleChatMessage(Guid senderId, Guid channelId, string content)
    {
        ChatMessage chatMessage = new ChatMessage
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            ChannelId = channelId,
            Content = content,
            Timestamp = DateTime.Now,
            Type = MessageType.Text
        };
        MessageReceived?.Invoke(this, new IMMessageEventArgs(chatMessage));
    }

    public void HandlePermissionResponse(Guid userId, bool allowed)
    {
        if (_pendingPermissionRequests.TryGetValue(userId, out TaskCompletionSource<AskPermissionResult>? tcs))
        {
            AskPermissionResult result = new AskPermissionResult
            {
                Allowed = allowed
            };
            tcs.SetResult(result);
            _pendingPermissionRequests.Remove(userId);
        }
    }

    public Task StartAsync()
    {
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        _sseHandler.Dispose();
        return Task.CompletedTask;
    }

    public async Task SendMessageAsync(Guid senderId, Guid channelId, string content, string? thinking = null, string? senderName = null)
    {
        var message = new
        {
            type = "chat",
            senderId = senderId.ToString(),
            senderName,
            channelId = channelId.ToString(),
            content,
            thinking,
            timestamp = DateTime.Now
        };

        await _sseHandler.SendToChannelAsync(channelId, "message", message);
        _logger.Debug($"Message sent to channel {channelId}");
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

        var message = new
        {
            type = "streaming",
            senderId = senderId.ToString(),
            channelId = channelId.ToString(),
            content = accumulatedContent,
            thinking = accumulatedThinking,
            streamId = chunk.StreamId.ToString(),
            isFinal = chunk.IsFinal,
            timestamp = DateTime.UtcNow
        };

        await _sseHandler.SendToChannelAsync(channelId, "streaming", message);
        _logger.Trace($"Stream chunk sent to channel {channelId}, isFinal={chunk.IsFinal}");
    }

    public async Task<AskPermissionResult> AskPermissionAsync(PermissionType permissionType, string resource, string allowCode, string denyCode)
    {
        var message = new
        {
            type = "permission_ask",
            content = $"Permission required: {permissionType}\nResource: {resource}\nAllow code: {allowCode}\nDeny code: {denyCode}",
            timestamp = DateTime.UtcNow
        };

        await _sseHandler.SendToAllAsync("permission", message);

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

    public async Task NotifyChannelChanged(Guid userId, Guid channelId)
    {
        SSEClient? client = _sseHandler.GetClient(userId);
        if (client != null)
        {
            client.ChannelId = channelId;
            await SendHistoryToClientAsync(client);
        }
    }
}
