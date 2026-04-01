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
    private readonly ITimeStorage _storage;
    private readonly string _storageKey;
    private readonly List<ChatMessage> _messages = [];
    private readonly object _lock = new();

    /// <inheritdoc/>
    public Guid Id { get; }

    /// <inheritdoc/>
    public SessionType Type => SessionType.SingleChat;

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
    public List<ChatMessage> GetPendingMessages(Guid receiverId)
    {
        lock (_lock)
        {
            return _messages
                .Where(m => m.ReceiverId == receiverId && !m.IsProcessed)
                .ToList();
        }
    }

    /// <inheritdoc/>
    public void MarkMessageAsProcessed(Guid messageId)
    {
        lock (_lock)
        {
            ChatMessage? message = _messages.FirstOrDefault(m => m.Id == messageId);
            if (message != null)
            {
                message.IsProcessed = true;
                SaveMessage(message);
            }
        }
    }

    /// <inheritdoc/>
    public void MarkMessagesAsProcessed(IEnumerable<Guid> messageIds)
    {
        lock (_lock)
        {
            HashSet<Guid> ids = new(messageIds);

            foreach (ChatMessage message in _messages)
            {
                if (ids.Contains(message.Id) && !message.IsProcessed)
                {
                    message.IsProcessed = true;
                    SaveMessage(message);
                }
            }
        }
    }
}
