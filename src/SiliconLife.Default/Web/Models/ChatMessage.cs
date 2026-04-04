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

namespace SiliconLife.Default.Web.Models;

/// <summary>
/// Represents a tool call made by a silicon being during message processing.
/// </summary>
public class ToolCallInfo
{
    public string Name { get; init; } = "";
    public string? Target { get; init; }
    public bool Success { get; init; }
}

/// <summary>
/// Represents a single chat message in the conversation.
/// User messages are always aligned to the right.
/// Silicon being messages may contain thinking, text, and tool calls.
/// </summary>
public class ChatMessage
{
    /// <summary>true = user message (right-aligned), false = silicon being message (left-aligned)</summary>
    public bool IsUser { get; init; }

    /// <summary>Sender name (for silicon being messages in group chat)</summary>
    public string? SenderName { get; init; }

    /// <summary>Main text content (always visible)</summary>
    public string? Text { get; init; }

    /// <summary>Thinking process (collapsed by default)</summary>
    public string? Thinking { get; init; }

    /// <summary>Tool calls during this message (collapsed by default)</summary>
    public IEnumerable<ToolCallInfo>? ToolCalls { get; init; }

    /// <summary>Timestamp string (e.g. "14:30")</summary>
    public string? Time { get; init; }
}
