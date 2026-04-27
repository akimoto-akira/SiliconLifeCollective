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
        _logger.Info(null, "WebUIProvider initialized with SSE");
    }

    private void OnSSEConnected(SSEClient client)
    {
        _logger.Info(null, $"SSE client connected: userId={client.UserId}, channelId={client.ChannelId}");

        if (client.ChannelId.HasValue)
        {
            _ = SendHistoryToClientAsync(client);
        }
    }

    private void OnSSEDisconnected(SSEClient client)
    {
        _logger.Info(null, $"SSE client disconnected: userId={client.UserId}");
    }

    private async Task SendHistoryToClientAsync(SSEClient client)
    {
        if (!client.ChannelId.HasValue)
        {
            _logger.Debug(null, "SendHistoryToClientAsync: channelId is null");
            return;
        }

        ChatSystem? chatSystem = ServiceLocator.Instance.ChatSystem;
        if (chatSystem == null)
        {
            _logger.Debug(null, "SendHistoryToClientAsync: chatSystem is null");
            return;
        }

        _logger.Info(null, $"SendHistoryToClientAsync: looking for session with channelId={client.ChannelId.Value}");
        SessionBase? session = chatSystem.GetSessionByChannelId(client.ChannelId.Value);
        if (session == null)
        {
            _logger.Debug(null, "SendHistoryToClientAsync: GetSessionByChannelId returned null, trying GetSession");
            session = chatSystem.GetSession(client.ChannelId.Value);
        }

        if (session == null)
        {
            _logger.Warn(null, $"SendHistoryToClientAsync: session not found for channelId={client.ChannelId.Value}");
            return;
        }

        List<ChatMessage> messages = session.GetMessages(500);
        _logger.Info(null, $"SendHistoryToClientAsync: found {messages.Count} messages for session {session.Id}");
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

    public void HandlePermissionResponse(Guid userId, bool allowed, bool addToCache = false, TimeSpan? cacheDuration = null)
    {
        if (_pendingPermissionRequests.TryGetValue(userId, out TaskCompletionSource<AskPermissionResult>? tcs))
        {
            AskPermissionResult result = new AskPermissionResult
            {
                Allowed = allowed,
                AddToCache = addToCache,
                CacheDuration = cacheDuration
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

    public async Task SendMessageAsync(Guid senderId, Guid channelId, string content, string? thinking = null, string? senderName = null, int? promptTokens = null, int? completionTokens = null, int? totalTokens = null)
    {
        var message = new
        {
            type = "chat",
            senderId = senderId.ToString(),
            senderName,
            channelId = channelId.ToString(),
            content,
            thinking,
            promptTokens,
            completionTokens,
            totalTokens,
            timestamp = DateTime.Now
        };

        await _sseHandler.SendToChannelAsync(channelId, "message", message);
        _logger.Debug(null, $"Message sent to channel {channelId}");
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
            promptTokens = chunk.IsFinal ? chunk.PromptTokens : null,
            completionTokens = chunk.IsFinal ? chunk.CompletionTokens : null,
            totalTokens = chunk.IsFinal ? chunk.TotalTokens : null,
            timestamp = DateTime.UtcNow
        };

        await _sseHandler.SendToChannelAsync(channelId, "streaming", message);
        _logger.Trace(null, $"Stream chunk sent to channel {channelId}, isFinal={chunk.IsFinal}");
    }

    public async Task<AskPermissionResult> AskPermissionAsync(PermissionType permissionType, string resource, string allowCode, string denyCode)
    {
        Guid userId = Config.Instance.Data.UserGuid;

        var message = new
        {
            type = "permission_ask",
            permissionType = permissionType.ToString(),
            resource,
            allowCode,
            denyCode,
            content = $"Permission required: {permissionType}\nResource: {resource}\nAllow code: {allowCode}\nDeny code: {denyCode}",
            timestamp = DateTime.UtcNow
        };

        await _sseHandler.SendToAllAsync("permission", message);

        var tcs = new TaskCompletionSource<AskPermissionResult>();
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

    public async Task SendToolUpdateAsync(Guid senderId, Guid channelId, string role, string content, string? toolCallsJson = null, string? toolCallId = null, string? thinking = null, string? senderName = null, int? promptTokens = null, int? completionTokens = null, int? totalTokens = null)
    {
        var message = new
        {
            type = "tool",
            senderId = senderId.ToString(),
            senderName,
            channelId = channelId.ToString(),
            content,
            role,
            toolCallsJson,
            toolCallId,
            thinking,
            promptTokens,
            completionTokens,
            totalTokens,
            timestamp = DateTime.UtcNow
        };

        await _sseHandler.SendToChannelAsync(channelId, "tool", message);
        _logger.Debug(null, $"Tool update sent to channel {channelId}, role={role}");
    }

    /// <summary>
    /// Sends a stream-stopped event to the frontend when the user cancels AI thinking.
    /// </summary>
    /// <param name="channelId">The channel ID where the stream was stopped.</param>
    public async Task SendStreamStoppedAsync(Guid channelId)
    {
        var message = new
        {
            type = "stopped",
            channelId = channelId.ToString(),
            reason = "user_cancelled",
            timestamp = DateTime.UtcNow
        };

        await _sseHandler.SendToChannelAsync(channelId, "stopped", message);
        _logger.Info(null, $"Stream stopped event sent to channel {channelId}");
    }

    /// <summary>
    /// Handles a file message by pushing it to the frontend via SSE.
    /// </summary>
    /// <param name="message">The file message to deliver.</param>
    public async Task HandleFileMessage(ChatMessage message)
    {
        var fileMessage = new
        {
            type = "file",
            messageId = message.Id.ToString(),
            senderId = message.SenderId.ToString(),
            channelId = message.ChannelId.ToString(),
            content = message.Content,
            fileMetadata = message.FileMetadata != null ? new
            {
                filePath = message.FileMetadata.FilePath,
                fileName = message.FileMetadata.FileName,
                fileSize = message.FileMetadata.FileSize,
                mimeType = message.FileMetadata.MimeType,
                isLocalPath = message.FileMetadata.IsLocalPath
            } : null,
            timestamp = message.Timestamp
        };

        await _sseHandler.SendToChannelAsync(message.ChannelId, "file", fileMessage);
        _logger.Debug(null, $"File message sent to channel {message.ChannelId}");
    }

    /// <summary>
    /// Sends a queue status event to the frontend indicating message queue position.
    /// </summary>
    /// <param name="channelId">The channel ID.</param>
    /// <param name="position">The position in the queue (0 means currently being processed).</param>
    /// <param name="totalCount">The total number of messages in the queue.</param>
    public async Task SendQueueStatusAsync(Guid channelId, int position, int totalCount = 0)
    {
        var message = new
        {
            type = "queue_status",
            channelId = channelId.ToString(),
            position,
            total = totalCount,
            timestamp = DateTime.UtcNow
        };

        await _sseHandler.SendToChannelAsync(channelId, "queue_status", message);
        _logger.Debug(null, $"Queue status sent to channel {channelId}, position={position}");
    }
}
