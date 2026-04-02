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
/// Message type classification
/// </summary>
public enum MessageType
{
    /// <summary>Plain text</summary>
    Text,
    /// <summary>Image attachment</summary>
    Image,
    /// <summary>File attachment</summary>
    File,
    /// <summary>System notification (not displayed to user)</summary>
    SystemNotification
}

public class ChatMessage
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public MessageType Type { get; set; }

    /// <summary>
    /// List of participant IDs who have read this message.
    /// A message is "unread" for a participant if their ID is not in this list.
    /// </summary>
    public List<Guid> ReadBy { get; set; } = [];

    /// <summary>
    /// Optional explicit message role for AI conversation context.
    /// When null, role is inferred from SenderId (User or Assistant).
    /// Used to persist Tool-related messages that cannot be inferred from sender alone.
    /// </summary>
    public MessageRole? Role { get; set; }

    /// <summary>
    /// Tool call ID for tool result messages, matching the originating ToolCall.
    /// </summary>
    public string? ToolCallId { get; set; }

    /// <summary>
    /// Serialized tool calls JSON for assistant messages that requested tool execution.
    /// </summary>
    public string? ToolCallsJson { get; set; }

    /// <summary>
    /// Thinking content (chain-of-thought reasoning from the AI).
    /// </summary>
    public string? Thinking { get; set; }

    public ChatMessage()
    {
        Id = Guid.NewGuid();
        Timestamp = DateTime.UtcNow;
        Type = MessageType.Text;
    }

    public ChatMessage(Guid senderId, Guid receiverId, string content)
        : this()
    {
        SenderId = senderId;
        ReceiverId = receiverId;
        Content = content;
    }

    public ChatMessage(Guid senderId, Guid receiverId, string content, MessageType type)
        : this(senderId, receiverId, content)
    {
        Type = type;
    }
}
