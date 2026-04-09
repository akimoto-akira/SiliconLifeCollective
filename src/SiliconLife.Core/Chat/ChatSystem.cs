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

using System.Security.Cryptography;

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
    /// Computes a deterministic channel ID for a single chat session between two participants.
    /// The ID is derived from sorted participant GUIDs via MD5 hash.
    /// </summary>
    /// <param name="participant1">First participant's ID</param>
    /// <param name="participant2">Second participant's ID</param>
    /// <returns>The deterministic channel ID for this conversation</returns>
    public static Guid ComputeSingleChatChannelId(Guid participant1, Guid participant2)
    {
        string sorted = string.Compare(participant1.ToString(), participant2.ToString(), StringComparison.Ordinal) <= 0
            ? $"{participant1}{participant2}"
            : $"{participant2}{participant1}";
        byte[] hash = MD5.HashData(System.Text.Encoding.UTF8.GetBytes(sorted));
        return new Guid(hash);
    }

    /// <summary>
    /// Add a message to the session for the given channel.
    /// Creates the session if it does not exist yet.
    /// </summary>
    public void AddMessage(Guid senderId, Guid channelId, string content, string? thinking = null)
    {
        ISession? session = GetSessionByChannelId(channelId);
        ChatMessage message = new(senderId, channelId, content) { Thinking = thinking };
        session?.AddMessage(message);
    }

    /// <summary>
    /// Add a pre-constructed ChatMessage to the appropriate session.
    /// Used for persisting messages with rich metadata (e.g. tool-related fields).
    /// Creates the session if it does not exist yet.
    /// </summary>
    public void AddMessage(ChatMessage message)
    {
        ISession session = GetOrCreateSession(message.SenderId, message.ChannelId);
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
    /// Get or create a session by channel ID.
    /// For single chat, the channel ID is derived from participant GUIDs.
    /// For group chat, the channel ID is the group ID.
    /// If the session does not exist, returns null.
    /// </summary>
    public ISession? GetSessionByChannelId(Guid channelId)
    {
        lock (_lock)
        {
            return _sessions.TryGetValue(channelId, out var session) ? session : null;
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
    /// Get any session (single or group) by its unique ID.
    /// Returns null if not found.
    /// </summary>
    public ISession? GetSession(Guid sessionId)
    {
        lock (_lock)
        {
            return _sessions.TryGetValue(sessionId, out var session) ? session : null;
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
    /// Broadcasts to all sessions; each session scans storage and updates if found.
    /// </summary>
    public void MarkMessageAsRead(Guid messageId, Guid readerId)
    {
        lock (_lock)
        {
            foreach (ISession session in _sessions.Values)
            {
                session.MarkMessageAsRead(messageId, readerId);
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

    /// <summary>
    /// Get all sessions that involve the specified user.
    /// Ensures sessions exist for all known user-being pairs by creating them if needed.
    /// </summary>
    public List<ISession> GetSessionsForUser(Guid userId, IEnumerable<Guid> beingIds)
    {
        // Ensure sessions exist for all known user-being pairs
        foreach (Guid beingId in beingIds)
        {
            GetOrCreateSession(userId, beingId);
        }

        lock (_lock)
        {
            return _sessions.Values
                .Where(s => s.Members.Contains(userId))
                .ToList();
        }
    }
}
