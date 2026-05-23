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
    private readonly Dictionary<Guid, List<ChatMessage>> _inbox;
    private readonly object _lock = new object();

    public SimpleChatService()
    {
        _inbox = new Dictionary<Guid, List<ChatMessage>>();
    }

    /// <summary>
    /// Gets pending messages for a specific silicon being (where receiver = beingId)
    /// </summary>
    public List<ChatMessage> GetPendingMessages(Guid beingId)
    {
        lock (_lock)
        {
            if (!_inbox.TryGetValue(beingId, out List<ChatMessage>? messages))
            {
                return [];
            }

            return messages.Where(m => !m.ReadBy.Contains(beingId)).ToList();
        }
    }

    /// <summary>
    /// Marks a message as read by a receiver
    /// </summary>
    public void MarkMessageProcessed(Guid receiverId, Guid messageId)
    {
        lock (_lock)
        {
            if (!_inbox.TryGetValue(receiverId, out List<ChatMessage>? messages))
            {
                return;
            }

            ChatMessage? msg = messages.FirstOrDefault(m => m.Id == messageId);
            if (msg != null && !msg.ReadBy.Contains(receiverId))
            {
                msg.ReadBy.Add(receiverId);
            }
        }
    }

    /// <summary>
    /// Adds a message to a channel (sender -> receiver)
    /// </summary>
    public void AddMessage(Guid senderId, Guid receiverId, ChatMessage message)
    {
        lock (_lock)
        {
            if (!_inbox.ContainsKey(receiverId))
            {
                _inbox[receiverId] = new List<ChatMessage>();
            }

            _inbox[receiverId].Add(message);
        }
    }
}
