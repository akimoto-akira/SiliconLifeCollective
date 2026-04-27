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
/// Single chat session �?persists messages via <see cref="ITimeStorage"/>.
/// </summary>
public class SingleChatSession : SessionBase
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<SingleChatSession>();
    private readonly ITimeStorage _storage;
    private readonly string _storageKey;
    private readonly object _lock = new();

    /// <inheritdoc/>
    public override SessionType Type => SessionType.SingleChat;

    /// <inheritdoc/>
    public override string Name { get; set; } = "";

    /// <summary>Participant 1 GUID</summary>
    public Guid Participant1 { get; }

    /// <summary>Participant 2 GUID</summary>
    public Guid Participant2 { get; }

    /// <inheritdoc/>
    public override List<Guid> Members { get; protected set; }

    /// <summary>
    /// Initialize single chat session.
    /// </summary>
    public SingleChatSession(Guid participant1, Guid participant2, ITimeStorage storage)
        : base(new[] { participant1, participant2 })
    {
        Participant1 = participant1;
        Participant2 = participant2;
        Members = [participant1, participant2];
        _storage = storage;
        _storageKey = $"sessions/single/{Id}";
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
            if (!_storage.Exists(_storageKey))
                return [];

            List<ChatMessage> allMessages = new();
            int year = DateTime.UtcNow.Year;

            // Load messages from recent years until we have enough
            while (allMessages.Count < limit && year >= 1)
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
            _logger.Trace(null, "Session {0}: retrieving {1} most recent messages", Id, Math.Min(allMessages.Count, limit));
            // Return the most recent messages
            return allMessages.Skip(Math.Max(0, allMessages.Count - limit)).Take(limit).ToList();
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
