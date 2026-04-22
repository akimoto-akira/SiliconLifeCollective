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
/// Group chat session â€?persists messages via <see cref="ITimeStorage"/>.
/// </summary>
public class GroupChatSession : SessionBase
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<GroupChatSession>();
    private readonly ITimeStorage _storage;
    private readonly string _storageKey;
    private readonly object _lock = new();

    /// <inheritdoc/>
    public override SessionType Type => SessionType.GroupChat;

    /// <inheritdoc/>
    public override string Name { get; set; }

    /// <inheritdoc/>
    public override List<Guid> Members { get; protected set; }

    /// <summary>
    /// Initialize group chat session.
    /// </summary>
    /// <param name="members">Member list</param>
    /// <param name="storage">Time-indexed storage for message persistence</param>
    /// <param name="name">Group display name (optional)</param>
    public GroupChatSession(List<Guid> members, ITimeStorage storage, string name = "")
        : base(members)
    {
        Members = new(members);
        Name = name;
        _storage = storage;
        _storageKey = $"sessions/group/{Id}";
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
            Members.Remove(memberId);
        }
    }

    /// <inheritdoc/>
    public override void AddMessage(ChatMessage message)
    {
        lock (_lock)
        {
            _storage.Write(_storageKey, message.Timestamp, message);
            _logger.Debug(null, "Session {0}: message added from {1}", Id, message.SenderId);
        }
    }

    /// <inheritdoc/>
    public override List<ChatMessage> GetMessages(int offset = 0, int limit = 50)
    {
        lock (_lock)
        {
            List<ChatMessage> allMessages = new();
            int year = DateTime.UtcNow.Year;

            while (allMessages.Count < offset + limit && year >= 1)
            {
                IncompleteDate range = new(year);
                List<TimeEntry<ChatMessage>> entries = _storage.Query<ChatMessage>(_storageKey, range);
                foreach (TimeEntry<ChatMessage> entry in entries)
                {
                    if (entry.Data != null)
                        allMessages.Add(entry.Data);
                }
                year--;
            }

            allMessages.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
            _logger.Trace(null, "Session {0}: retrieving {1} messages", Id, allMessages.Count);
            return allMessages.Skip(offset).Take(limit).ToList();
        }
    }

    /// <inheritdoc/>
    public override List<ChatMessage> GetPendingMessages(Guid participantId)
    {
        lock (_lock)
        {
            List<ChatMessage> pending = new();
            int year = DateTime.UtcNow.Year;

            while (year >= 1)
            {
                IncompleteDate range = new(year);
                List<TimeEntry<ChatMessage>> entries = _storage.Query<ChatMessage>(_storageKey, range);
                bool foundAny = false;

                foreach (TimeEntry<ChatMessage> entry in entries)
                {
                    if (entry.Data != null && entry.Data.SenderId != participantId && !entry.Data.ReadBy.Contains(participantId))
                    {
                        pending.Add(entry.Data);
                        foundAny = true;
                    }
                }

                if (foundAny)
                    break;
                year--;
            }

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
            int year = DateTime.UtcNow.Year;

            while (year >= 1)
            {
                IncompleteDate range = new(year);
                List<TimeEntry<ChatMessage>> entries = _storage.Query<ChatMessage>(_storageKey, range);

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

                year--;
            }
        }
    }

    /// <inheritdoc/>
    public override void MarkMessagesAsRead(IEnumerable<Guid> messageIds, Guid readerId)
    {
        lock (_lock)
        {
            HashSet<Guid> ids = new(messageIds);
            int year = DateTime.UtcNow.Year;

            while (year >= 1 && ids.Count > 0)
            {
                IncompleteDate range = new(year);
                List<TimeEntry<ChatMessage>> entries = _storage.Query<ChatMessage>(_storageKey, range);

                foreach (TimeEntry<ChatMessage> entry in entries)
                {
                    if (entry.Data != null && ids.Contains(entry.Data.Id) && !entry.Data.ReadBy.Contains(readerId))
                    {
                        entry.Data.ReadBy.Add(readerId);
                        _storage.Write(_storageKey, entry.Timestamp, entry.Data);
                        ids.Remove(entry.Data.Id);
                    }
                }

                year--;
            }
        }
    }
}
