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
/// A broadcast channel open to all silicon beings in the collective.
/// Unlike <see cref="GroupChatSession"/>, the channel ID is fixed and does not
/// depend on membership. Subscribers join dynamically and only receive messages
/// posted after their subscription time.
/// </summary>
public class BroadcastChannel : SessionBase
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<BroadcastChannel>();
    private readonly ITimeStorage _storage;
    private readonly string _storageKey;
    private readonly object _lock = new();

    // subscriberId -> subscription time (only messages after this time are "pending")
    private readonly Dictionary<Guid, DateTime> _subscriptions = new();

    /// <inheritdoc/>
    public override SessionType Type => SessionType.Broadcast;

    /// <inheritdoc/>
    public override string Name { get; set; }

    /// <summary>
    /// Current subscriber IDs. For broadcast, Members = active subscribers.
    /// </summary>
    public override List<Guid> Members { get; protected set; } = [];

    /// <summary>
    /// Initialize a broadcast channel with a fixed, well-known ID.
    /// </summary>
    /// <param name="channelId">Fixed channel ID (e.g. a well-known Guid constant)</param>
    /// <param name="storage">Time-indexed storage for message persistence</param>
    /// <param name="name">Channel display name</param>
    public BroadcastChannel(Guid channelId, ITimeStorage storage, string name = "")
        : base(Array.Empty<Guid>())
    {
        // Override the deterministic ID with the fixed one
        // We use a backing field trick via the protected constructor route:
        // Since SessionBase computes Id in constructor, we shadow it here.
        _fixedId = channelId;
        Name = name;
        _storage = storage;
        _storageKey = $"sessions/broadcast/{channelId}";
    }

    // Shadow Id so we can use a fixed channel GUID instead of member-derived hash
    private readonly Guid _fixedId;
    /// <inheritdoc/>
    public new Guid Id => _fixedId;

    /// <summary>
    /// Subscribe a silicon being to this channel.
    /// The being will only receive messages posted on or after the subscription time.
    /// </summary>
    public void Subscribe(Guid beingId)
    {
        lock (_lock)
        {
            if (!_subscriptions.ContainsKey(beingId))
            {
                _subscriptions[beingId] = DateTime.UtcNow;
                Members.Add(beingId);
                _logger.Info(null, "BroadcastChannel {0}: {1} subscribed at {2:u}", _fixedId, beingId, _subscriptions[beingId]);
            }
        }
    }

    /// <summary>
    /// Unsubscribe a silicon being from this channel.
    /// </summary>
    public void Unsubscribe(Guid beingId)
    {
        lock (_lock)
        {
            _subscriptions.Remove(beingId);
            Members.Remove(beingId);
            _logger.Info(null, "BroadcastChannel {0}: {1} unsubscribed", _fixedId, beingId);
        }
    }

    /// <summary>
    /// Returns the UTC time at which the given being subscribed, or null if not subscribed.
    /// </summary>
    public DateTime? GetSubscriptionTime(Guid beingId)
    {
        lock (_lock)
        {
            return _subscriptions.TryGetValue(beingId, out DateTime t) ? t : null;
        }
    }

    /// <inheritdoc/>
    public override void AddMessage(ChatMessage message)
    {
        lock (_lock)
        {
            _storage.Write(_storageKey, message.Timestamp, message);
            _logger.Debug(null, "BroadcastChannel {0}: message from {1}", _fixedId, message.SenderId);
        }
    }

    /// <inheritdoc/>
    public override List<ChatMessage> GetMessages(int offset = 0, int limit = 50)
    {
        lock (_lock)
        {
            List<ChatMessage> all = LoadAllMessages();
            return all.Skip(offset).Take(limit).ToList();
        }
    }

    /// <summary>
    /// Get pending messages for a subscriber �?only messages posted after subscription time
    /// that the subscriber has not yet read.
    /// </summary>
    public override List<ChatMessage> GetPendingMessages(Guid participantId)
    {
        lock (_lock)
        {
            if (!_subscriptions.TryGetValue(participantId, out DateTime subscribedAt))
                return []; // not a subscriber

            List<ChatMessage> all = LoadAllMessages();
            return all
                .Where(m => m.Timestamp >= subscribedAt
                         && m.SenderId != participantId
                         && !m.ReadBy.Contains(participantId))
                .ToList();
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
                    if (entry.Data?.Id == messageId && !entry.Data.ReadBy.Contains(readerId))
                    {
                        entry.Data.ReadBy.Add(readerId);
                        _storage.Write(_storageKey, entry.Timestamp, entry.Data);
                        _logger.Trace(null, "BroadcastChannel {0}: message {1} read by {2}", _fixedId, messageId, readerId);
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

    // ── helpers ──────────────────────────────────────────────────────────────

    private List<ChatMessage> LoadAllMessages()
    {
        List<ChatMessage> all = [];
        int year = DateTime.UtcNow.Year;
        while (year >= 2026)          // broadcast channels didn't exist before the project started
        {
            IncompleteDate range = new(year);
            List<TimeEntry<ChatMessage>> entries = _storage.Query<ChatMessage>(_storageKey, range);
            foreach (TimeEntry<ChatMessage> e in entries)
            {
                if (e.Data != null) all.Add(e.Data);
            }
            year--;
        }
        all.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
        return all;
    }
}
