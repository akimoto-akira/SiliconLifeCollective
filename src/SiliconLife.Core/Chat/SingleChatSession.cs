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

using System.Security.Cryptography;
using System.Text.Json;

namespace SiliconLife.Collective;

/// <summary>
/// Single chat session — persists messages via <see cref="ITimeStorage"/>.
/// </summary>
public class SingleChatSession : ISession
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<SingleChatSession>();
    private readonly ITimeStorage _storage;
    private readonly string _storageKey;
    private readonly object _lock = new();

    /// <inheritdoc/>
    public Guid Id { get; }

    /// <inheritdoc/>
    public SessionType Type => SessionType.SingleChat;

    /// <inheritdoc/>
    public string Name { get; } = "";

    /// <summary>Participant 1 GUID</summary>
    public Guid Participant1 { get; }

    /// <summary>Participant 2 GUID</summary>
    public Guid Participant2 { get; }

    /// <inheritdoc/>
    public List<Guid> Members { get; }

    /// <summary>
    /// Initialize single chat session.
    /// </summary>
    public SingleChatSession(Guid participant1, Guid participant2, ITimeStorage storage)
    {
        Participant1 = participant1;
        Participant2 = participant2;
        Members = [participant1, participant2];
        _storage = storage;

        // Deterministic Id from sorted participant GUIDs via MD5
        string sorted = string.Compare(participant1.ToString(), participant2.ToString(), StringComparison.Ordinal) <= 0
            ? $"{participant1}{participant2}"
            : $"{participant2}{participant1}";
        byte[] hash = MD5.HashData(System.Text.Encoding.UTF8.GetBytes(sorted));
        Id = new Guid(hash);

        _storageKey = $"sessions/single/{Id}";
    }

    /// <inheritdoc/>
    public void AddMessage(ChatMessage message)
    {
        lock (_lock)
        {
            byte[] data = JsonSerializer.SerializeToUtf8Bytes(message);
            _storage.Write(_storageKey, message.Timestamp, data);
            _logger.Debug("Session {Id}: message added from {SenderId}", Id, message.SenderId);
        }
    }

    /// <inheritdoc/>
    public List<ChatMessage> GetMessages(int offset = 0, int limit = 50)
    {
        lock (_lock)
        {
            // Early exit: if no data exists for this session key, skip the year loop
            if (!_storage.Exists(_storageKey))
                return [];

            List<ChatMessage> allMessages = new();
            int year = DateTime.UtcNow.Year;

            while (allMessages.Count < offset + limit && year >= 1)
            {
                IncompleteDate range = new(year);
                List<TimeEntry> entries = _storage.Query(_storageKey, range);
                foreach (TimeEntry entry in entries)
                {
                    ChatMessage? msg = JsonSerializer.Deserialize<ChatMessage>(entry.Data);
                    if (msg != null)
                        allMessages.Add(msg);
                }
                year--;
            }

            allMessages.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
            _logger.Trace("Session {Id}: retrieving {Count} messages", Id, allMessages.Count);
            return allMessages.Skip(offset).Take(limit).ToList();
        }
    }

    /// <inheritdoc/>
    public List<ChatMessage> GetPendingMessages(Guid participantId)
    {
        lock (_lock)
        {
            List<ChatMessage> pending = new();
            int year = DateTime.UtcNow.Year;

            while (year >= 1)
            {
                IncompleteDate range = new(year);
                List<TimeEntry> entries = _storage.Query(_storageKey, range);
                bool foundAny = false;

                foreach (TimeEntry entry in entries)
                {
                    ChatMessage? msg = JsonSerializer.Deserialize<ChatMessage>(entry.Data);
                    if (msg != null && msg.SenderId != participantId && !msg.ReadBy.Contains(participantId))
                    {
                        pending.Add(msg);
                        foundAny = true;
                    }
                }

                if (foundAny)
                    break;
                year--;
            }

            pending.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
            _logger.Debug("Session {Id}: {Count} pending messages for {ParticipantId}", Id, pending.Count, participantId);
            return pending;
        }
    }

    /// <inheritdoc/>
    public void MarkMessageAsRead(Guid messageId, Guid readerId)
    {
        lock (_lock)
        {
            int year = DateTime.UtcNow.Year;

            while (year >= 1)
            {
                IncompleteDate range = new(year);
                List<TimeEntry> entries = _storage.Query(_storageKey, range);

                foreach (TimeEntry entry in entries)
                {
                    ChatMessage? msg = JsonSerializer.Deserialize<ChatMessage>(entry.Data);
                    if (msg != null && msg.Id == messageId && !msg.ReadBy.Contains(readerId))
                    {
                        msg.ReadBy.Add(readerId);
                        byte[] data = JsonSerializer.SerializeToUtf8Bytes(msg);
                        _storage.Write(_storageKey, entry.Timestamp, data);
                        _logger.Trace("Session {Id}: message {MessageId} read by {ReaderId}", Id, messageId, readerId);
                        return;
                    }
                }

                year--;
            }
        }
    }

    /// <inheritdoc/>
    public void MarkMessagesAsRead(IEnumerable<Guid> messageIds, Guid readerId)
    {
        lock (_lock)
        {
            HashSet<Guid> ids = new(messageIds);
            int year = DateTime.UtcNow.Year;

            while (year >= 1 && ids.Count > 0)
            {
                IncompleteDate range = new(year);
                List<TimeEntry> entries = _storage.Query(_storageKey, range);

                foreach (TimeEntry entry in entries)
                {
                    ChatMessage? msg = JsonSerializer.Deserialize<ChatMessage>(entry.Data);
                    if (msg != null && ids.Contains(msg.Id) && !msg.ReadBy.Contains(readerId))
                    {
                        msg.ReadBy.Add(readerId);
                        byte[] data = JsonSerializer.SerializeToUtf8Bytes(msg);
                        _storage.Write(_storageKey, entry.Timestamp, data);
                        ids.Remove(msg.Id);
                    }
                }

                year--;
            }
        }
    }
}
