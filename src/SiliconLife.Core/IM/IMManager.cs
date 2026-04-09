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
    }

    /// <summary>
    /// Handles an incoming IM message. If the receiver ID is empty, the message
    /// is routed to the first available <see cref="SiliconBeingBase"/>.
    /// </summary>
    private void OnMessageReceived(object? sender, IMMessageEventArgs e)
    {
        ChatMessage msg = e.Message;
        if (msg.ChannelId == Guid.Empty)
        {
            List<SiliconBeingBase> beings = _beingManager.GetAllBeings();
            if (beings.Count > 0)
            {
                msg.ChannelId = beings[0].Id;
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
    public async Task SendMessageAsync(Guid senderId, Guid channelId, string content)
    {
        await _imProvider.SendMessageAsync(senderId, channelId, content);
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
        await _imProvider.SendStreamChunkAsync(senderId, channelId, chunk);
    }

    /// <summary>
    /// Starts the IM manager and activates the underlying IM provider.
    /// </summary>
    public async Task StartAsync()
    {
        _isRunning = true;
        await _imProvider.StartAsync();
    }

    /// <summary>
    /// Stops the IM manager and deactivates the underlying IM provider.
    /// </summary>
    public async Task StopAsync()
    {
        _isRunning = false;
        await _imProvider.StopAsync();
    }
}
