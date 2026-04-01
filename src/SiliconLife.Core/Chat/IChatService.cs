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
/// Interface for chat service
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Gets pending messages for a specific silicon being (where receiver = beingId)
    /// </summary>
    /// <param name="beingId">The silicon being ID (receiver)</param>
    /// <returns>List of pending messages</returns>
    List<ChatMessage> GetPendingMessages(Guid beingId);

    /// <summary>
    /// Marks a message as processed
    /// </summary>
    /// <param name="receiverId">The silicon being ID (receiver)</param>
    /// <param name="messageId">The message ID to mark as processed</param>
    void MarkMessageProcessed(Guid receiverId, Guid messageId);

    /// <summary>
    /// Adds a message to a channel (sender -> receiver)
    /// </summary>
    /// <param name="senderId">The sender ID</param>
    /// <param name="receiverId">The receiver ID (silicon being)</param>
    /// <param name="message">The message to add</param>
    void AddMessage(Guid senderId, Guid receiverId, ChatMessage message);
}
