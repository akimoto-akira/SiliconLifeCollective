// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace SiliconLife.Collective;

/// <summary>
/// Central chat system that manages all chat sessions (single and group).
/// Provides message routing, persistence via <see cref="ITimeStorage"/>, and pending message tracking.
/// </summary>
public class ChatSystem
{
    private readonly ITimeStorage _storage;
    private readonly object _lock = new();
    private Dictionary<Guid, ISession> _sessions = new();

    public ChatSystem(ITimeStorage storage)
    {
        _storage = storage;
    }

    /// <summary>
    /// Add a message to the session between the given sender and receiver.
    /// Creates the session if it does not exist yet.
    /// </summary>
    public void AddMessage(Guid senderId, Guid receiverId, string content)
    {
        ISession session = GetOrCreateSession(senderId, receiverId);
        ChatMessage message = new(senderId, receiverId, content);
        session.AddMessage(message);
    }

    /// <summary>
    /// Add a pre-constructed ChatMessage to the appropriate session.
    /// Used for persisting messages with rich metadata (e.g. tool-related fields).
    /// Creates the session if it does not exist yet.
    /// </summary>
    public void AddMessage(ChatMessage message)
    {
        ISession session = GetOrCreateSession(message.SenderId, message.ReceiverId);
        session.AddMessage(message);
    }

    /// <summary>
    /// Retrieve recent messages from the session between the given user and being.
    /// Creates the session if it does not exist yet.
    /// </summary>
    public List<ChatMessage> GetMessages(Guid userId, Guid beingId, int limit = 50)
    {
        ISession session = GetOrCreateSession(userId, beingId);
        return session.GetMessages(0, limit);
    }

    /// <summary>
    /// Get or create a single chat session for the two participants.
    /// Searches existing sessions for a matching pair in either order.
    /// </summary>
    public ISession GetOrCreateSession(Guid participant1, Guid participant2)
    {
        lock (_lock)
        {
            // Search for an existing session matching both participant orderings
            foreach (ISession session in _sessions.Values)
            {
                if (session is SingleChatSession single &&
                    single.Participant1 == participant1 && single.Participant2 == participant2)
                {
                    return session;
                }
                if (session is SingleChatSession single2 &&
                    single2.Participant1 == participant2 && single2.Participant2 == participant1)
                {
                    return session;
                }
            }

            SingleChatSession newSession = new(participant1, participant2, _storage);
            _sessions[newSession.Id] = newSession;
            return newSession;
        }
    }

    /// <summary>
    /// Create a new group chat session with the specified members.
    /// </summary>
    public ISession CreateGroupSession(List<Guid> members)
    {
        Guid groupId = Guid.NewGuid();

        lock (_lock)
        {
            GroupChatSession newSession = new(groupId, members, _storage);
            _sessions[groupId] = newSession;
            return newSession;
        }
    }

    /// <summary>
    /// Retrieve a group chat session by its group ID.
    /// Returns null if not found or if the session is not a group session.
    /// </summary>
    public ISession? GetGroupSession(Guid groupId)
    {
        lock (_lock)
        {
            if (_sessions.TryGetValue(groupId, out ISession? session))
            {
                if (session.Type == SessionType.GroupChat)
                {
                    return session;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// Collect all pending (unprocessed) messages across all sessions for the given being.
    /// </summary>
    public List<ChatMessage> GetPendingMessages(Guid beingId)
    {
        List<ChatMessage> result = [];

        lock (_lock)
        {
            foreach (ISession session in _sessions.Values)
            {
                result.AddRange(session.GetPendingMessages(beingId));
            }
        }

        return result;
    }

    /// <summary>
    /// Mark a single message as read by a specific reader across all sessions.
    /// </summary>
    public void MarkMessageAsRead(Guid messageId, Guid readerId)
    {
        lock (_lock)
        {
            foreach (ISession session in _sessions.Values)
            {
                if (session.GetMessages(0, int.MaxValue).Any(m => m.Id == messageId))
                {
                    session.MarkMessageAsRead(messageId, readerId);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Mark multiple messages as read by a specific reader across all sessions.
    /// </summary>
    public void MarkMessagesAsRead(IEnumerable<Guid> messageIds, Guid readerId)
    {
        lock (_lock)
        {
            foreach (ISession session in _sessions.Values)
            {
                session.MarkMessagesAsRead(messageIds, readerId);
            }
        }
    }
}
