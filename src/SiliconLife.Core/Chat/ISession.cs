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
/// Session type
/// </summary>
public enum SessionType
{
    /// <summary>Single chat</summary>
    SingleChat,
    /// <summary>Group chat</summary>
    GroupChat
}

/// <summary>
/// Chat session interface
/// </summary>
public interface ISession
{
    /// <summary>Unique session identifier</summary>
    Guid Id { get; }

    /// <summary>Session type</summary>
    SessionType Type { get; }

    /// <summary>Member list</summary>
    List<Guid> Members { get; }

    /// <summary>
    /// Add a message
    /// </summary>
    /// <param name="message">Chat message</param>
    void AddMessage(ChatMessage message);

    /// <summary>
    /// Get message list
    /// </summary>
    /// <param name="offset">Offset</param>
    /// <param name="limit">Limit</param>
    /// <returns>Message list (ordered by timestamp ascending)</returns>
    List<ChatMessage> GetMessages(int offset = 0, int limit = 50);

    /// <summary>
    /// Get pending messages
    /// </summary>
    /// <param name="receiverId">Receiver GUID</param>
    /// <returns>Unread message list</returns>
    List<ChatMessage> GetPendingMessages(Guid receiverId);

    /// <summary>
    /// Mark single message as processed
    /// </summary>
    /// <param name="messageId">Message ID</param>
    void MarkMessageAsProcessed(Guid messageId);

    /// <summary>
    /// Mark multiple messages as processed
    /// </summary>
    /// <param name="messageIds">Message ID collection</param>
    void MarkMessagesAsProcessed(IEnumerable<Guid> messageIds);
}
