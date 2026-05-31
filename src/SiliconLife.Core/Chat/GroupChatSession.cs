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
/// Metadata record for group chat session persistence.
/// Replaces anonymous type to support System.Text.Json ReferenceHandler.Preserve.
/// </summary>
public record GroupChatSessionMetadata
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public List<Guid> Members { get; init; } = [];
    public string Type { get; init; } = "";
    public DateTime UpdatedAt { get; init; }
}

/// <summary>
/// Group chat session — persists messages via <see cref="ITimeStorage"/>.
/// Uses a fixed session ID (not derived from members) to ensure stability
/// when members are added or removed.
/// </summary>
public class GroupChatSession : SessionBase
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<GroupChatSession>();
    private readonly ITimeStorage _storage;
    private readonly string _storageKey;
    private readonly string _metaKey;
    private readonly object _lock = new();

    /// <inheritdoc/>
    public override SessionType Type => SessionType.GroupChat;

    /// <inheritdoc/>
    public override string Name { get; set; }

    /// <inheritdoc/>
    public override List<Guid> Members { get; protected set; }

    /// <inheritdoc/>
    public override Guid Id { get; }

    /// <summary>
    /// Initialize group chat session with a fixed ID.
    /// </summary>
    /// <param name="sessionId">Fixed session ID (e.g. from ProjectSpace.GroupChatSessionId)</param>
    /// <param name="members">Member list</param>
    /// <param name="storage">Time-indexed storage for message persistence</param>
    /// <param name="name">Group display name (optional)</param>
    public GroupChatSession(Guid sessionId, List<Guid> members, ITimeStorage storage, string name = "")
    {
        Id = sessionId;
        Members = new(members);
        Name = name;
        _storage = storage;
        _storageKey = $"sessions/group/{sessionId}";
        _metaKey = $"sessions/group/{sessionId}/meta";
        
        // Persist session metadata on creation
        SaveMetadata();
        _logger.Info(null, "GroupChatSession created: {0}, name='{1}', members={2}", sessionId, name, members.Count);
    }

    /// <summary>
    /// Add a member.
    /// </summary>
    public void AddMember(Guid memberId)
    {
        lock (_lock)
        {
            if (!Members.Contains(memberId))
            {
                Members.Add(memberId);
                SaveMetadata();
                _logger.Info(null, "GroupChatSession {0}: member {1} added, total={2}", Id, memberId, Members.Count);
            }
        }
    }

    /// <summary>
    /// Remove a member.
    /// </summary>
    public void RemoveMember(Guid memberId)
    {
        lock (_lock)
        {
            if (Members.Remove(memberId))
            {
                SaveMetadata();
                _logger.Info(null, "GroupChatSession {0}: member {1} removed, total={2}", Id, memberId, Members.Count);
            }
        }
    }

    /// <inheritdoc/>
    public override void AddMessage(ChatMessage message)
    {
        lock (_lock)
        {
            var ts = message.Timestamp;
            _storage.Write(_storageKey, new IncompleteDate(ts.Year, ts.Month, ts.Day, ts.Hour, ts.Minute, ts.Second), message);
            _logger.Debug(null, "Session {0}: message added from {1}", Id, message.SenderId);
        }
    }

    /// <inheritdoc/>
    public override List<ChatMessage> GetMessages(int limit = 10)
    {
        lock (_lock)
        {
            // Pull the most recent N entries directly from storage — ignore time ranges.
            List<TimeEntry<ChatMessage>> entries = _storage.QueryLatest<ChatMessage>(_storageKey, limit);
            List<ChatMessage> messages = entries
                .Where(e => e.Data != null)
                .Select(e => e.Data!)
                .ToList();

            messages.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
            _logger.Trace(null, "Session {0}: retrieving {1} most recent messages", Id, messages.Count);
            return messages;
        }
    }

    /// <inheritdoc/>
    public override List<ChatMessage> GetPendingMessages(Guid participantId)
    {
        lock (_lock)
        {
            List<TimeEntry<ChatMessage>> entries = _storage.Query<ChatMessage>(_storageKey, null);
            List<ChatMessage> pending = entries
                .Where(e => e.Data != null
                         && e.Data.SenderId != participantId
                         && !e.Data.ReadBy.Contains(participantId))
                .Select(e => e.Data!)
                .ToList();

            pending.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
            _logger.Debug(null, "Session {0}: {1} pending messages for {2}", Id, pending.Count, participantId);
            return pending;
        }
    }

    /// <inheritdoc/>
    public override void MarkMessageAsRead(Guid messageId, Guid readerId)
    {
        lock (_lock)
        {
            List<TimeEntry<ChatMessage>> entries = _storage.Query<ChatMessage>(_storageKey, null);
            foreach (TimeEntry<ChatMessage> entry in entries)
            {
                if (entry.Data != null && entry.Data.Id == messageId && !entry.Data.ReadBy.Contains(readerId))
                {
                    entry.Data.ReadBy.Add(readerId);
                    _storage.Write(_storageKey, entry.Timestamp, entry.Data);
                    _logger.Trace(null, "Session {0}: message {1} read by {2}", Id, messageId, readerId);
                    return;
                }
            }
        }
    }

    /// <inheritdoc/>
    public override void MarkMessagesAsRead(IEnumerable<Guid> messageIds, Guid readerId)
    {
        lock (_lock)
        {
            HashSet<Guid> ids = new(messageIds);
            if (ids.Count == 0) return;

            List<TimeEntry<ChatMessage>> entries = _storage.Query<ChatMessage>(_storageKey, null);
            foreach (TimeEntry<ChatMessage> entry in entries)
            {
                if (entry.Data != null && ids.Contains(entry.Data.Id) && !entry.Data.ReadBy.Contains(readerId))
                {
                    entry.Data.ReadBy.Add(readerId);
                    _storage.Write(_storageKey, entry.Timestamp, entry.Data);
                    ids.Remove(entry.Data.Id);
                    if (ids.Count == 0) break;
                }
            }
        }
    }

    // ── metadata persistence ──────────────────────────────────────────────

    /// <summary>
    /// Persist session metadata (ID, name, members, type) to storage.
    /// </summary>
    private void SaveMetadata()
    {
        var meta = new GroupChatSessionMetadata
        {
            Id = Id,
            Name = Name,
            Members = Members,
            Type = Type.ToString(),
            UpdatedAt = DateTime.UtcNow
        };
        // Metadata is not time-indexed, use IStorage.Write (inherited by ITimeStorage)
        _storage.Write(_metaKey, meta);
        _logger.Trace(null, "GroupChatSession {0}: metadata saved", Id);
    }
}
