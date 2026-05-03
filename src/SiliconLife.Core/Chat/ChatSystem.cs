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
/// Central chat system that manages all chat sessions (single and group).
/// Provides message routing, persistence via <see cref="ITimeStorage"/>, and pending message tracking.
/// </summary>
public class ChatSystem
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<ChatSystem>();
    private readonly ITimeStorage _storage;
    private readonly object _lock = new();
    private Dictionary<Guid, SessionBase> _sessions = new();
    private readonly Dictionary<Guid, BroadcastChannel> _broadcastChannels = new();

    public ChatSystem(ITimeStorage storage)
    {
        _storage = storage;
        _logger.Info(null, "ChatSystem initialized");
        
        // Load existing group chat sessions from storage
        LoadGroupChatSessions();
    }

    /// <summary>
    /// Add a message to the session for the given channel.
    /// Creates the session if it does not exist yet.
    /// </summary>
    public void AddMessage(Guid senderId, Guid channelId, string content, string? thinking = null)
    {
        SessionBase? session = GetSessionByChannelId(channelId);
        ChatMessage message = new(senderId, channelId, content) { Thinking = thinking };
        session?.AddMessage(message);
        _logger.Debug(null, "Message added: sender={0}, channel={1}, length={2}", senderId, channelId, content.Length);
    }

    /// <summary>
    /// Add a pre-constructed ChatMessage to the appropriate session.
    /// Used for persisting messages with rich metadata (e.g. tool-related fields).
    /// Creates the session if it does not exist yet.
    /// </summary>
    public void AddMessage(ChatMessage message)
    {
        SessionBase? session = GetSessionByChannelId(message.ChannelId);
        session?.AddMessage(message);
        _logger.Debug(null, "Message added: sender={0}, channel={1}, length={2}", message.SenderId, message.ChannelId, message.Content.Length);
    }

    /// <summary>
    /// Retrieve recent messages from the session between the given user and being.
    /// Creates the session if it does not exist yet.
    /// </summary>
    public List<ChatMessage> GetMessages(Guid userId, Guid beingId, int limit = 50)
    {
        SessionBase session = GetOrCreateSession(userId, beingId);
        _logger.Debug(null, "Retrieving messages: user={0}, being={1}, limit={2}", userId, beingId, limit);
        return session.GetMessages(limit);
    }

    /// <summary>
    /// Get or create a single chat session for the two participants.
    /// Searches existing sessions for a matching pair in either order.
    /// </summary>
    public SessionBase GetOrCreateSession(Guid participant1, Guid participant2)
    {
        lock (_lock)
        {
            foreach (SessionBase session in _sessions.Values)
            {
                if (session is SingleChatSession single &&
                    single.Participant1 == participant1 && single.Participant2 == participant2)
                {
                    _logger.Trace(null, "Reusing existing session: {0}", session.Id);
                    return session;
                }
                if (session is SingleChatSession single2 &&
                    single2.Participant1 == participant2 && single2.Participant2 == participant1)
                {
                    _logger.Trace(null, "Reusing existing session: {0}", session.Id);
                    return session;
                }
            }

            SingleChatSession newSession = new(participant1, participant2, _storage);
            _sessions[newSession.Id] = newSession;
            _logger.Info(null, "Created new single chat session: {0}, participants={1},{2}", newSession.Id, participant1, participant2);
            return newSession;
        }
    }

    /// <summary>
    /// Get or create a session by channel ID.
    /// For single chat, the channel ID is derived from participant GUIDs.
    /// For group chat, the channel ID is the group ID.
    /// If the session does not exist, returns null.
    /// </summary>
    public SessionBase? GetSessionByChannelId(Guid channelId)
    {
        lock (_lock)
        {
            return _sessions.TryGetValue(channelId, out var session) ? session : null;
        }
    }

    /// <summary>
    /// Create a new group chat session with the specified members.
    /// </summary>
    /// <param name="members">Member list</param>
    /// <param name="name">Group display name</param>
    /// <param name="fixedSessionId">Optional fixed session ID. If provided, uses this ID instead of generating a new one.</param>
    public SessionBase CreateGroupSession(List<Guid> members, string name = "", Guid? fixedSessionId = null)
    {
        lock (_lock)
        {
            Guid sessionId = fixedSessionId ?? Guid.NewGuid();
            
            // Check if session with this ID already exists
            if (_sessions.ContainsKey(sessionId))
            {
                _logger.Info(null, "Reusing existing group chat session: {0}", sessionId);
                return _sessions[sessionId];
            }
            
            GroupChatSession newSession = new(sessionId, members, _storage, name);
            _sessions[newSession.Id] = newSession;
            _logger.Info(null, "Created group chat session: {0}, members={1}", newSession.Id, members.Count);
            return newSession;
        }
    }

    /// <summary>
    /// Load existing group chat sessions from storage on startup.
    /// Scans sessions/group/*/meta for persisted session metadata.
    /// </summary>
    private void LoadGroupChatSessions()
    {
        try
        {
            // List all session directories under sessions/group/
            // Returns keys like: "sessions/group/{sessionId}/"
            var sessionKeys = _storage.ListKeys("sessions/group");
            
            if (!sessionKeys.Any())
            {
                _logger.Info(null, "No existing group chat sessions found in storage");
                return;
            }

            int loadedCount = 0;
            lock (_lock)
            {
                foreach (var sessionKey in sessionKeys)
                {
                    // Extract sessionId from key path
                    // Format: "sessions/group/{sessionId}/" or "{sessionId}/"
                    string sessionIdStr = sessionKey.TrimEnd('/');
                    int lastSlash = sessionIdStr.LastIndexOf('/');
                    if (lastSlash >= 0)
                    {
                        sessionIdStr = sessionIdStr[(lastSlash + 1)..];
                    }

                    if (!Guid.TryParse(sessionIdStr, out Guid sessionId))
                    {
                        _logger.Warn(null, "Invalid session ID format in storage: {0}", sessionKey);
                        continue;
                    }

                    // Skip if already loaded
                    if (_sessions.ContainsKey(sessionId))
                        continue;

                    // Read session metadata
                    string metaKey = $"sessions/group/{sessionIdStr}/meta";
                    var metaDictArray = _storage.Read<Dictionary<string, object>>(metaKey);
                    var metaDict = metaDictArray.FirstOrDefault();
                    
                    if (metaDict == null)
                    {
                        _logger.Warn(null, "Failed to read metadata for session: {0}", sessionId);
                        continue;
                    }

                    string name = metaDict.TryGetValue("Name", out var nameObj) ? nameObj?.ToString() ?? "" : "";
                    var members = new List<Guid>();
                    
                    if (metaDict.TryGetValue("Members", out var membersObj) && membersObj is List<object> memberList)
                    {
                        foreach (var m in memberList)
                        {
                            if (Guid.TryParse(m?.ToString(), out Guid memberId))
                                members.Add(memberId);
                        }
                    }

                    // Recreate the session with fixed ID
                    GroupChatSession session = new(sessionId, members, _storage, name);
                    _sessions[sessionId] = session;
                    loadedCount++;
                    _logger.Info(null, "Loaded group chat session from storage: {0}, name='{1}', members={2}", sessionId, name, members.Count);
                }
            }

            _logger.Info(null, "Loaded {0} group chat session(s) from storage", loadedCount);
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to load group chat sessions from storage: {0}", ex.Message);
        }
    }

    /// <summary>
    /// Get any session (single or group) by its unique ID.
    /// Returns null if not found.
    /// </summary>
    public SessionBase? GetSession(Guid sessionId)
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
    public SessionBase? GetGroupSession(Guid groupId)
    {
        lock (_lock)
        {
            if (_sessions.TryGetValue(groupId, out SessionBase? session))
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
            foreach (SessionBase session in _sessions.Values)
            {
                result.AddRange(session.GetPendingMessages(beingId));
            }
        }

        _logger.Debug(null, "Found {0} pending messages for being {1}", result.Count, beingId);
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
            foreach (SessionBase session in _sessions.Values)
            {
                session.MarkMessageAsRead(messageId, readerId);
            }
        }
        _logger.Trace(null, "Message {0} marked as read by {1}", messageId, readerId);
    }

    /// <summary>
    /// Mark multiple messages as read by a specific reader across all sessions.
    /// </summary>
    public void MarkMessagesAsRead(IEnumerable<Guid> messageIds, Guid readerId)
    {
        lock (_lock)
        {
            foreach (SessionBase session in _sessions.Values)
            {
                session.MarkMessagesAsRead(messageIds, readerId);
            }
        }
    }

    /// <summary>
    /// Get all sessions that involve the specified user.
    /// Ensures sessions exist for all known user-being pairs by creating them if needed.
    /// </summary>
    public List<SessionBase> GetSessionsForUser(Guid userId, IEnumerable<Guid> beingIds)
    {
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

    /// <summary>
    /// Get message count metrics for the specified time range.
    /// Returns message counts grouped by minute for the last N minutes.
    /// </summary>
    /// <param name="minutes">Number of minutes to look back</param>
    /// <returns>Dictionary mapping timestamp (HH:mm) to message count</returns>
    public Dictionary<string, int> GetMessageMetrics(int minutes = 20)
    {
        DateTime cutoff = DateTime.Now.AddMinutes(-minutes);
        Dictionary<string, int> metrics = [];

        lock (_lock)
        {
            foreach (SessionBase session in _sessions.Values)
            {
                List<ChatMessage> messages = session.GetMessages(int.MaxValue);
                foreach (ChatMessage message in messages)
                {
                    if (message.Timestamp >= cutoff)
                    {
                        string key = message.Timestamp.ToString("HH:mm");
                        if (!metrics.ContainsKey(key))
                        {
                            metrics[key] = 0;
                        }
                        metrics[key]++;
                    }
                }
            }
        }

        _logger.Debug(null, "Message metrics collected: {0} time slots", metrics.Count);
        return metrics;
    }

    /// <summary>
    /// Get total message count across all sessions.
    /// </summary>
    public int GetTotalMessageCount()
    {
        int count = 0;
        lock (_lock)
        {
            foreach (SessionBase session in _sessions.Values)
            {
                count += session.GetMessages(int.MaxValue).Count;
            }
        }
        return count;
    }

    // ── Broadcast ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Get or create a broadcast channel with a fixed, well-known ID.
    /// Suitable for system-wide announcements across the silicon collective.
    /// </summary>
    /// <param name="channelId">Fixed channel GUID (use a project-level constant)</param>
    /// <param name="name">Human-readable channel name</param>
    public BroadcastChannel GetOrCreateBroadcastChannel(Guid channelId, string name = "")
    {
        lock (_lock)
        {
            if (_broadcastChannels.TryGetValue(channelId, out BroadcastChannel? existing))
                return existing;

            BroadcastChannel channel = new(channelId, _storage, name);
            _broadcastChannels[channelId] = channel;
            _logger.Info(null, "BroadcastChannel created: {0} ({1})", channelId, name);
            return channel;
        }
    }

    /// <summary>
    /// Broadcast a message to a channel. All subscribers will see it as pending
    /// until they read it.
    /// </summary>
    /// <param name="senderId">The sender (system or a silicon being)</param>
    /// <param name="channelId">Target broadcast channel ID</param>
    /// <param name="content">Message content</param>
    public void Broadcast(Guid senderId, Guid channelId, string content)
    {
        lock (_lock)
        {
            if (!_broadcastChannels.TryGetValue(channelId, out BroadcastChannel? channel))
            {
                _logger.Warn(null, "Broadcast failed: channel {0} not found", channelId);
                return;
            }

            ChatMessage message = new(senderId, channelId, content)
            {
                Type = MessageType.SystemNotification
            };
            channel.AddMessage(message);
            _logger.Info(null, "Broadcast sent to channel {0} by {1}", channelId, senderId);
        }
    }

    /// <summary>
    /// Collect pending broadcast messages for a given being across all channels
    /// they are subscribed to.
    /// </summary>
    public List<ChatMessage> GetPendingBroadcasts(Guid beingId)
    {
        List<ChatMessage> result = [];
        lock (_lock)
        {
            foreach (BroadcastChannel channel in _broadcastChannels.Values)
            {
                result.AddRange(channel.GetPendingMessages(beingId));
            }
        }
        _logger.Debug(null, "{0} pending broadcast(s) for {1}", result.Count, beingId);
        return result;
    }
}
