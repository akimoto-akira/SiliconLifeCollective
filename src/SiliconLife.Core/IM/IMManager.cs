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

namespace SiliconLife.Collective;

/// <summary>
/// Manages instant-messaging (IM) integration, bridging incoming IM messages
/// into the internal <see cref="ChatSystem"/> and enabling outbound messages
/// through the configured <see cref="IIMProvider"/>.
/// </summary>
public class IMManager
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<IMManager>();
    private readonly IIMProvider _imProvider;
    private readonly ChatSystem _chatSystem;
    private readonly SiliconBeingManager _beingManager;
    private bool _isRunning;
    private readonly ChatMessageQueue _messageQueue = new();

    /// <summary>
    /// Initializes a new <see cref="IMManager"/> and subscribes to the
    /// provider's <see cref="IIMProvider.MessageReceived"/> event.
    /// </summary>
    /// <param name="imProvider">The IM transport provider.</param>
    /// <param name="chatSystem">The internal chat system to route messages into.</param>
    /// <param name="beingManager">The being manager used to resolve default receivers.</param>
    public IMManager(IIMProvider imProvider, ChatSystem chatSystem, SiliconBeingManager beingManager)
    {
        _imProvider = imProvider;
        _chatSystem = chatSystem;
        _beingManager = beingManager;
        _isRunning = false;

        _imProvider.MessageReceived += OnMessageReceived;
        _logger.Info(null, "IMManager initialized with provider {0}", imProvider.GetType().Name);
    }

    /// <summary>
    /// Handles an incoming IM message. If the receiver ID is empty, the message
    /// is routed to the first available <see cref="SiliconBeingBase"/>.
    /// Messages are queued per channel to prevent concurrent AI processing.
    /// </summary>
    private void OnMessageReceived(object? sender, IMMessageEventArgs e)
    {
        ChatMessage msg = e.Message;
        _logger.Info(null, "IM message received: sender={0}, channel={1}", msg.SenderId, msg.ChannelId);
        if (msg.ChannelId == Guid.Empty)
        {
            // Reject messages with empty ChannelId instead of routing to a default being
            _logger.Warn(null, "Received message with empty ChannelId from sender={0}, rejecting", msg.SenderId);
            return;
        }

        // Enqueue the message and try to process it
        int position = _messageQueue.Enqueue(msg.ChannelId, msg);

        // Notify frontend about queue status
        NotifyQueueStatus(msg.ChannelId, position);

        // Try to dequeue and process the next message
        ProcessNextMessage(msg.ChannelId);
    }

    /// <summary>
    /// Attempts to dequeue and process the next message in the channel's queue.
    /// Only proceeds if the channel is not currently processing another message.
    /// </summary>
    private void ProcessNextMessage(Guid channelId)
    {
        if (_messageQueue.TryDequeue(channelId, out var queuedMsg))
        {
            // Dequeued successfully — add to ChatSystem which triggers AI thinking
            _chatSystem.AddMessage(queuedMsg.Message.SenderId, queuedMsg.Message.ChannelId, queuedMsg.Message.Content);
        }
    }

    /// <summary>
    /// Notifies the frontend about the current queue status for a channel.
    /// Pushes a <c>queue_status</c> SSE event via the IM provider.
    /// </summary>
    private void NotifyQueueStatus(Guid channelId, int position)
    {
        try
        {
            int total = _messageQueue.GetQueueLength(channelId);
            _ = _imProvider.SendQueueStatusAsync(channelId, position, total);
        }
        catch (Exception ex)
        {
            _logger.Debug(null, "Failed to notify queue status for channel {0}: {1}", channelId, ex.Message);
        }
    }

    /// <summary>
    /// Called by ContextManager when AI finishes processing a message.
    /// Marks the channel as available and processes the next queued message.
    /// </summary>
    /// <param name="channelId">The channel ID that has finished processing.</param>
    public void NotifyMessageProcessed(Guid channelId)
    {
        _logger.Debug(null, "Message processed for channel {0}", channelId);
        _messageQueue.MarkComplete(channelId);

        // Process the next message in queue, if any
        ProcessNextMessage(channelId);

        // Notify frontend that queue position changed
        int position = _messageQueue.GetQueuePosition(channelId);
        NotifyQueueStatus(channelId, position);
    }

    /// <summary>
    /// Sends a message through the IM provider.
    /// </summary>
    /// <param name="senderId">The ID of the message sender.</param>
    /// <param name="channelId">The channel ID.</param>
    /// <param name="content">The message content.</param>
    public async Task SendMessageAsync(Guid senderId, Guid channelId, string content, string? thinking = null, string? senderName = null, int? promptTokens = null, int? completionTokens = null, int? totalTokens = null)
    {
        _logger.Debug(null, "Sending IM message: sender={0}, channel={1}", senderId, channelId);
        await _imProvider.SendMessageAsync(senderId, channelId, content, thinking, senderName, promptTokens, completionTokens, totalTokens);
    }

    /// <summary>
    /// Sends a streaming chunk through the IM provider.
    /// Used for real-time streaming responses like AI text generation.
    /// </summary>
    /// <param name="senderId">The ID of the message sender.</param>
    /// <param name="channelId">The channel ID.</param>
    /// <param name="chunk">The stream chunk to send.</param>
    public async Task SendStreamChunkAsync(Guid senderId, Guid channelId, StreamChunk chunk)
    {
        _logger.Trace(null, "Sending stream chunk: sender={0}, channel={1}", senderId, channelId);
        await _imProvider.SendStreamChunkAsync(senderId, channelId, chunk);
    }

    /// <summary>
    /// Sends a tool update event through the IM provider.
    /// Used for real-time notification when AI calls tools or tools return results.
    /// </summary>
    /// <param name="senderId">The ID of the message sender.</param>
    /// <param name="channelId">The channel ID.</param>
    /// <param name="role">Message role (Assistant for tool call, Tool for tool result).</param>
    /// <param name="content">The message content.</param>
    /// <param name="toolCallsJson">Serialized tool calls JSON (for Assistant role with tool calls).</param>
    /// <param name="toolCallId">Tool call ID (for Tool role result messages).</param>
    /// <param name="thinking">Optional thinking content.</param>
    /// <param name="senderName">Optional sender name.</param>
    /// <param name="promptTokens">Number of tokens in the prompt.</param>
    /// <param name="completionTokens">Number of tokens in the completion.</param>
    /// <param name="totalTokens">Total number of tokens.</param>
    public async Task SendToolUpdateAsync(Guid senderId, Guid channelId, string role, string content, string? toolCallsJson = null, string? toolCallId = null, string? thinking = null, string? senderName = null, int? promptTokens = null, int? completionTokens = null, int? totalTokens = null)
    {
        _logger.Debug(null, "Sending tool update: sender={0}, channel={1}, role={2}", senderId, channelId, role);
        await _imProvider.SendToolUpdateAsync(senderId, channelId, role, content, toolCallsJson, toolCallId, thinking, senderName, promptTokens, completionTokens, totalTokens);
    }

    /// <summary>
    /// Starts the IM manager and activates the underlying IM provider.
    /// </summary>
    public async Task StartAsync()
    {
        _isRunning = true;
        await _imProvider.StartAsync();
        _logger.Info(null, "IM manager started");
    }

    /// <summary>
    /// Stops the IM manager and deactivates the underlying IM provider.
    /// </summary>
    public async Task StopAsync()
    {
        _isRunning = false;
        await _imProvider.StopAsync();
        _logger.Info(null, "IM manager stopped");
    }
}
