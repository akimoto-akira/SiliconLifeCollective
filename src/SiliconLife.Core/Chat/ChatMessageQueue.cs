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

using System.Collections.Concurrent;

namespace SiliconLife.Collective;

/// <summary>
/// Per-channel message queue that serialises AI processing so that only one
/// message per channel is processed at a time.  When the AI finishes a message,
/// the next queued message is automatically dequeued and dispatched.
/// Thread-safe via <see cref="ConcurrentDictionary{TKey,TValue}"/> and locks.
/// </summary>
public class ChatMessageQueue
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<ChatMessageQueue>();
    private readonly ConcurrentDictionary<Guid, Queue<QueuedMessage>> _queues = new();
    private readonly ConcurrentDictionary<Guid, bool> _processingFlags = new();
    private readonly object _lock = new();

    /// <summary>Maximum number of messages allowed per channel queue.</summary>
    private const int MaxQueueSize = 100;

    /// <summary>
    /// Represents a message waiting in the queue.
    /// </summary>
    public class QueuedMessage
    {
        /// <summary>The queued chat message.</summary>
        public ChatMessage Message { get; set; } = new ChatMessage();

        /// <summary>When the message was enqueued.</summary>
        public DateTime EnqueuedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Adds a message to the channel's queue.
    /// If the channel is not currently processing, the message can be immediately dequeued.
    /// </summary>
    /// <param name="channelId">The channel ID.</param>
    /// <param name="message">The chat message to enqueue.</param>
    /// <returns>The position in the queue (0 means head of queue / being processed).</returns>
    public int Enqueue(Guid channelId, ChatMessage message)
    {
        lock (_lock)
        {
            if (!_queues.TryGetValue(channelId, out var queue))
            {
                queue = new Queue<QueuedMessage>();
                _queues[channelId] = queue;
            }

            // Enforce queue size limit
            if (queue.Count >= MaxQueueSize)
            {
                _logger.Warn(null, "Queue for channel {0} is full ({1}), dropping oldest message", channelId, MaxQueueSize);
                queue.Dequeue();
            }

            queue.Enqueue(new QueuedMessage
            {
                Message = message,
                EnqueuedAt = DateTime.Now
            });

            int position = _processingFlags.TryGetValue(channelId, out bool isProcessing) && isProcessing
                ? queue.Count
                : 0;

            _logger.Debug(null, "Message enqueued for channel {0}, position={1}, queueSize={2}", channelId, position, queue.Count);
            return position;
        }
    }

    /// <summary>
    /// Attempts to dequeue the next message for a channel.
    /// Only succeeds if the channel is not currently processing.
    /// </summary>
    /// <param name="channelId">The channel ID.</param>
    /// <param name="message">The dequeued message, or <c>null</c>.</param>
    /// <returns><c>true</c> if a message was dequeued; <c>false</c> otherwise.</returns>
    public bool TryDequeue(Guid channelId, out QueuedMessage? message)
    {
        message = null;

        if (_processingFlags.TryGetValue(channelId, out bool isProcessing) && isProcessing)
        {
            return false; // Already processing, cannot dequeue
        }

        lock (_lock)
        {
            if (!_queues.TryGetValue(channelId, out var queue) || queue.Count == 0)
            {
                return false;
            }

            message = queue.Dequeue();
            _processingFlags[channelId] = true;
            _logger.Debug(null, "Message dequeued for channel {0}, remaining={1}", channelId, queue.Count);
            return true;
        }
    }

    /// <summary>
    /// Marks the current processing as complete for the channel.
    /// Must be called after AI finishes processing a message, regardless of success or failure.
    /// </summary>
    /// <param name="channelId">The channel ID.</param>
    public void MarkComplete(Guid channelId)
    {
        _processingFlags[channelId] = false;
        _logger.Debug(null, "Processing marked complete for channel {0}", channelId);
    }

    /// <summary>
    /// Checks whether a channel is currently processing a message.
    /// </summary>
    /// <param name="channelId">The channel ID.</param>
    /// <returns><c>true</c> if the channel is currently processing.</returns>
    public bool IsProcessing(Guid channelId)
    {
        return _processingFlags.TryGetValue(channelId, out bool isProcessing) && isProcessing;
    }

    /// <summary>
    /// Gets the number of messages waiting in the queue for a channel.
    /// </summary>
    /// <param name="channelId">The channel ID.</param>
    /// <returns>The queue length (0 if no queue exists).</returns>
    public int GetQueueLength(Guid channelId)
    {
        lock (_lock)
        {
            if (_queues.TryGetValue(channelId, out var queue))
            {
                return queue.Count;
            }
            return 0;
        }
    }

    /// <summary>
    /// Gets the queue position for a channel.
    /// Returns 0 if currently processing, or the number of messages waiting.
    /// </summary>
    /// <param name="channelId">The channel ID.</param>
    /// <returns>0 if processing or no messages; otherwise the number of queued messages.</returns>
    public int GetQueuePosition(Guid channelId)
    {
        if (IsProcessing(channelId))
        {
            return 0;
        }
        return GetQueueLength(channelId);
    }

    /// <summary>
    /// Clears the queue for a specific channel.
    /// </summary>
    /// <param name="channelId">The channel ID.</param>
    public void ClearQueue(Guid channelId)
    {
        lock (_lock)
        {
            _queues.TryRemove(channelId, out _);
            _processingFlags.TryRemove(channelId, out _);
            _logger.Debug(null, "Queue cleared for channel {0}", channelId);
        }
    }
}
