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
using System.Text;

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
/// Abstract base class for chat sessions.
/// Provides unified deterministic ID generation via MD5 hash.
/// </summary>
public abstract class SessionBase
{
    /// <summary>Unique session identifier (deterministic)</summary>
    public Guid Id { get; }

    /// <summary>Session type</summary>
    public abstract SessionType Type { get; }

    /// <summary>Session display name</summary>
    public abstract string Name { get; set; }

    /// <summary>Member list</summary>
    public abstract List<Guid> Members { get; protected set; }

    /// <summary>
    /// Protected constructor that generates a deterministic ID from member GUIDs.
    /// </summary>
    /// <param name="members">Member GUID collection for ID generation</param>
    protected SessionBase(IEnumerable<Guid> members)
    {
        Id = ComputeDeterministicId(members);
    }

    /// <summary>
    /// Computes a deterministic GUID from a collection of member GUIDs.
    /// Members are sorted and concatenated, then hashed via MD5.
    /// </summary>
    /// <param name="members">Member GUID collection</param>
    /// <returns>Deterministic GUID</returns>
    protected static Guid ComputeDeterministicId(IEnumerable<Guid> members)
    {
        List<Guid> sorted = members.OrderBy(g => g.ToString()).ToList();
        string concatenated = string.Join("", sorted);
        byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes(concatenated));
        return new Guid(hash);
    }

    /// <summary>
    /// Add a message
    /// </summary>
    /// <param name="message">Chat message</param>
    public abstract void AddMessage(ChatMessage message);

    /// <summary>
    /// Get message list
    /// </summary>
    /// <param name="offset">Offset</param>
    /// <param name="limit">Limit</param>
    /// <returns>Message list (ordered by timestamp ascending)</returns>
    public abstract List<ChatMessage> GetMessages(int offset = 0, int limit = 50);

    /// <summary>
    /// Get pending (unread) messages for a specific participant.
    /// A message is pending if the participant's ID is not in the message's ReadBy list.
    /// </summary>
    /// <param name="participantId">The participant (reader) to check unread messages for</param>
    /// <returns>Unread message list</returns>
    public abstract List<ChatMessage> GetPendingMessages(Guid participantId);

    /// <summary>
    /// Mark a single message as read by a specific participant.
    /// </summary>
    /// <param name="messageId">Message ID</param>
    /// <param name="readerId">The participant who read the message</param>
    public abstract void MarkMessageAsRead(Guid messageId, Guid readerId);

    /// <summary>
    /// Mark multiple messages as read by a specific participant.
    /// </summary>
    /// <param name="messageIds">Message ID collection</param>
    /// <param name="readerId">The participant who read the messages</param>
    public abstract void MarkMessagesAsRead(IEnumerable<Guid> messageIds, Guid readerId);
}
