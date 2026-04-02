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

using System.Text.Json;

namespace SiliconLife.Collective;

/// <summary>
/// Group chat session — persists messages via <see cref="ITimeStorage"/>.
/// </summary>
public class GroupChatSession : ISession
{
    private readonly ITimeStorage _storage;
    private readonly string _storageKey;
    private readonly List<ChatMessage> _messages = [];
    private readonly object _lock = new();

    /// <inheritdoc/>
    public Guid Id { get; }

    /// <inheritdoc/>
    public SessionType Type => SessionType.GroupChat;

    /// <inheritdoc/>
    public List<Guid> Members { get; private set; }

    /// <summary>
    /// Initialize group chat session.
    /// </summary>
    /// <param name="groupId">Group GUID</param>
    /// <param name="members">Member list</param>
    /// <param name="storage">Time-indexed storage for message persistence</param>
    public GroupChatSession(Guid groupId, List<Guid> members, ITimeStorage storage)
    {
        Id = groupId;
        Members = new(members);
        _storage = storage;
        _storageKey = $"sessions/group/{Id}";

        LoadMessages();
    }

    /// <summary>
    /// Earliest year that has been fully loaded from disk.
    /// </summary>
    private int _loadedEarliestYear = DateTime.UtcNow.Year;

    private void LoadMessages()
    {
        LoadMessagesForYear(DateTime.UtcNow.Year);
    }

    /// <summary>
    /// Load messages for a specific year from storage into _messages.
    /// </summary>
    private void LoadMessagesForYear(int year)
    {
        lock (_lock)
        {
            HashSet<Guid> existingIds = [.. _messages.Select(m => m.Id)];

            IncompleteDate range = new(year);
            List<TimeEntry> entries = _storage.Query(_storageKey, range);
            foreach (TimeEntry entry in entries)
            {
                ChatMessage? msg = JsonSerializer.Deserialize<ChatMessage>(entry.Data);
                if (msg != null && !existingIds.Contains(msg.Id))
                {
                    _messages.Add(msg);
                    existingIds.Add(msg.Id);
                }
            }
            _messages.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));

            if (year < _loadedEarliestYear)
            {
                _loadedEarliestYear = year;
            }
        }
    }

    private void SaveMessage(ChatMessage message)
    {
        byte[] data = JsonSerializer.SerializeToUtf8Bytes(message);
        _storage.Write(_storageKey, message.Timestamp, data);
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
    public void AddMessage(ChatMessage message)
    {
        lock (_lock)
        {
            _messages.Add(message);
            SaveMessage(message);
        }
    }

    /// <inheritdoc/>
    public List<ChatMessage> GetMessages(int offset = 0, int limit = 50)
    {
        lock (_lock)
        {
            // If in-memory messages don't have enough, load earlier years from disk
            while (_messages.Count - offset < limit && _loadedEarliestYear > 1)
            {
                _loadedEarliestYear--;
                LoadMessagesForYear(_loadedEarliestYear);
            }

            return _messages
                .Skip(offset)
                .Take(limit)
                .ToList();
        }
    }

    /// <inheritdoc/>
    public List<ChatMessage> GetPendingMessages(Guid participantId)
    {
        lock (_lock)
        {
            return _messages
                .Where(m => m.SenderId != participantId && !m.ReadBy.Contains(participantId))
                .ToList();
        }
    }

    /// <inheritdoc/>
    public void MarkMessageAsRead(Guid messageId, Guid readerId)
    {
        lock (_lock)
        {
            ChatMessage? message = _messages.FirstOrDefault(m => m.Id == messageId);
            if (message != null && !message.ReadBy.Contains(readerId))
            {
                message.ReadBy.Add(readerId);
                SaveMessage(message);
            }
        }
    }

    /// <inheritdoc/>
    public void MarkMessagesAsRead(IEnumerable<Guid> messageIds, Guid readerId)
    {
        lock (_lock)
        {
            HashSet<Guid> ids = new(messageIds);

            foreach (ChatMessage message in _messages)
            {
                if (ids.Contains(message.Id) && !message.ReadBy.Contains(readerId))
                {
                    message.ReadBy.Add(readerId);
                    SaveMessage(message);
                }
            }
        }
    }
}
