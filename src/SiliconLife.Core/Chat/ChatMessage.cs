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
    /// <summary>
    /// Gets or sets the unique identifier of the message.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the sender's unique identifier.
    /// </summary>
    public Guid SenderId { get; set; }

    /// <summary>
    /// Gets or sets the channel identifier (conversation or group ID).
    /// </summary>
    public Guid ChannelId { get; set; }

    /// <summary>
    /// Gets or sets the content of the message.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the message was sent.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the type of the message.
    /// </summary>
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

    /// <summary>
    /// Initializes a new instance of the ChatMessage class.
    /// </summary>
    public ChatMessage()
    {
        Id = Guid.NewGuid();
        Timestamp = DateTime.Now;
        Type = MessageType.Text;
    }

    /// <summary>
    /// Initializes a new instance of the ChatMessage class with sender, channel, and content.
    /// </summary>
    /// <param name="senderId">The sender's unique identifier.</param>
    /// <param name="channelId">The channel identifier.</param>
    /// <param name="content">The message content.</param>
    public ChatMessage(Guid senderId, Guid channelId, string content)
        : this()
    {
        SenderId = senderId;
        ChannelId = channelId;
        Content = content;
    }

    /// <summary>
    /// Initializes a new instance of the ChatMessage class with all parameters.
    /// </summary>
    /// <param name="senderId">The sender's unique identifier.</param>
    /// <param name="channelId">The channel identifier.</param>
    /// <param name="content">The message content.</param>
    /// <param name="type">The message type.</param>
    public ChatMessage(Guid senderId, Guid channelId, string content, MessageType type)
        : this(senderId, channelId, content)
    {
        Type = type;
    }
}
