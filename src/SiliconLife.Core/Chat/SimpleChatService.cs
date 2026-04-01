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
/// Simple in-memory chat service implementation for testing
/// </summary>
public class SimpleChatService : IChatService
{
    private readonly Dictionary<Guid, Dictionary<Guid, List<string>>> _channels;
    private readonly object _lock = new object();

    public SimpleChatService()
    {
        _channels = new Dictionary<Guid, Dictionary<Guid, List<string>>>();
    }

    private Guid GetChannelKey(Guid senderId, Guid receiverId)
    {
        return senderId < receiverId ? senderId : receiverId;
    }

    private (Guid sender, Guid receiver) GetChannelPair(Guid channelKey, Guid originalSender, Guid originalReceiver)
    {
        if (channelKey == originalSender)
        {
            return (originalSender, originalReceiver);
        }
        return (originalReceiver, originalSender);
    }

    /// <summary>
    /// Gets pending messages for a specific silicon being (where receiver = beingId)
    /// </summary>
    /// <param name="beingId">The silicon being ID (receiver)</param>
    /// <returns>List of pending messages</returns>
    public List<string> GetPendingMessages(Guid beingId)
    {
        lock (_lock)
        {
            List<string> result = new List<string>();

            foreach (KeyValuePair<Guid, Dictionary<Guid, List<string>>> channel in _channels)
            {
                foreach (KeyValuePair<Guid, List<string>> senderMessages in channel.Value)
                {
                    Guid sender = senderMessages.Key;
                    List<string> messages = senderMessages.Value;

                    if (channel.Key == beingId)
                    {
                        result.AddRange(messages);
                    }
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Marks a message as processed
    /// </summary>
    /// <param name="receiverId">The silicon being ID (receiver)</param>
    /// <param name="message">The message to mark as processed</param>
    public void MarkMessageProcessed(Guid receiverId, string message)
    {
        lock (_lock)
        {
            foreach (Dictionary<Guid, List<string>> channel in _channels.Values)
            {
                foreach (List<string> messages in channel.Values)
                {
                    if (messages.Contains(message))
                    {
                        messages.Remove(message);
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Adds a message to a channel (sender -> receiver)
    /// </summary>
    /// <param name="senderId">The sender ID (0 for user)</param>
    /// <param name="receiverId">The receiver ID (silicon being)</param>
    /// <param name="message">The message content</param>
    public void AddMessage(Guid senderId, Guid receiverId, string message)
    {
        lock (_lock)
        {
            Guid channelKey = receiverId;

            if (!_channels.ContainsKey(channelKey))
            {
                _channels[channelKey] = new Dictionary<Guid, List<string>>();
            }

            if (!_channels[channelKey].ContainsKey(senderId))
            {
                _channels[channelKey][senderId] = new List<string>();
            }

            _channels[channelKey][senderId].Add(message);
        }
    }
}
