// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace SiliconLife.Collective;

/// <summary>
/// Message role in AI conversation
/// </summary>
public enum MessageRole
{
    /// <summary>
    /// System message (provides context/instructions)
    /// </summary>
    System,

    /// <summary>
    /// User message
    /// </summary>
    User,

    /// <summary>
    /// Assistant message (AI response)
    /// </summary>
    Assistant,

    /// <summary>
    /// Tool message (result of a tool execution, sent back to AI)
    /// </summary>
    Tool
}

/// <summary>
/// Single message in AI conversation
/// </summary>
public class Message
{
    /// <summary>
    /// Gets or sets the role of the message sender
    /// </summary>
    public MessageRole Role { get; set; }

    /// <summary>
    /// Gets or sets the content of the message
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the thinking content of the message (chain-of-thought reasoning)
    /// </summary>
    public string? Thinking { get; set; }

    /// <summary>
    /// Gets or sets the tool call ID (for Tool role messages, matches the originating ToolCall)
    /// </summary>
    public string? ToolCallId { get; set; }

    /// <summary>
    /// Gets or sets the tool calls (for Assistant role messages when AI requests tool execution)
    /// </summary>
    public List<ToolCall>? ToolCalls { get; set; }

    /// <summary>
    /// Creates a new message with the specified role and content
    /// </summary>
    public Message(MessageRole role, string content)
    {
        Role = role;
        Content = content;
    }

    /// <summary>
    /// Creates a tool result message
    /// </summary>
    /// <param name="toolCallId">The ID of the originating tool call</param>
    /// <param name="content">The tool result content (typically JSON)</param>
    public static Message ToolResultMessage(string toolCallId, string content)
    {
        return new Message(MessageRole.Tool, content)
        {
            ToolCallId = toolCallId
        };
    }

    /// <summary>
    /// Creates an assistant message with tool calls
    /// </summary>
    /// <param name="content">Optional content (may be empty when tools are called)</param>
    /// <param name="toolCalls">The tool calls requested by the AI</param>
    /// <param name="thinking">Optional thinking content (chain-of-thought)</param>
    public static Message AssistantWithToolCalls(string content, List<ToolCall> toolCalls, string? thinking = null)
    {
        return new Message(MessageRole.Assistant, content)
        {
            ToolCalls = toolCalls,
            Thinking = thinking
        };
    }
}
