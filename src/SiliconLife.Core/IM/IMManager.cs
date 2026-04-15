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
        _logger.Info("IMManager initialized with provider {0}", imProvider.GetType().Name);
    }

    /// <summary>
    /// Handles an incoming IM message. If the receiver ID is empty, the message
    /// is routed to the first available <see cref="SiliconBeingBase"/>.
    /// </summary>
    private void OnMessageReceived(object? sender, IMMessageEventArgs e)
    {
        ChatMessage msg = e.Message;
        _logger.Info("IM message received: sender={0}, channel={1}", msg.SenderId, msg.ChannelId);
        if (msg.ChannelId == Guid.Empty)
        {
            List<SiliconBeingBase> beings = _beingManager.GetAllBeings();
            if (beings.Count > 0)
            {
                msg.ChannelId = beings[0].Id;
                _logger.Debug("Routing message to default being: {0}", beings[0].Id);
                _chatSystem.AddMessage(msg.SenderId, msg.ChannelId, msg.Content);
            }
        }
        else
        {
            _chatSystem.AddMessage(msg.SenderId, msg.ChannelId, msg.Content);
        }
    }

    /// <summary>
    /// Sends a message through the IM provider.
    /// </summary>
    /// <param name="senderId">The ID of the message sender.</param>
    /// <param name="channelId">The channel ID.</param>
    /// <param name="content">The message content.</param>
    public async Task SendMessageAsync(Guid senderId, Guid channelId, string content, string? thinking = null, string? senderName = null, int? promptTokens = null, int? completionTokens = null, int? totalTokens = null)
    {
        _logger.Debug("Sending IM message: sender={0}, channel={1}", senderId, channelId);
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
        _logger.Trace("Sending stream chunk: sender={0}, channel={1}", senderId, channelId);
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
    public async Task SendToolUpdateAsync(Guid senderId, Guid channelId, string role, string content, string? toolCallsJson = null, string? toolCallId = null, string? thinking = null, string? senderName = null)
    {
        _logger.Debug("Sending tool update: sender={0}, channel={1}, role={2}", senderId, channelId, role);
        await _imProvider.SendToolUpdateAsync(senderId, channelId, role, content, toolCallsJson, toolCallId, thinking, senderName);
    }

    /// <summary>
    /// Starts the IM manager and activates the underlying IM provider.
    /// </summary>
    public async Task StartAsync()
    {
        _isRunning = true;
        await _imProvider.StartAsync();
        _logger.Info("IM manager started");
    }

    /// <summary>
    /// Stops the IM manager and deactivates the underlying IM provider.
    /// </summary>
    public async Task StopAsync()
    {
        _isRunning = false;
        await _imProvider.StopAsync();
        _logger.Info("IM manager stopped");
    }
}
