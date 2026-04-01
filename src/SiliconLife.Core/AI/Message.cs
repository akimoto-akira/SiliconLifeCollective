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
    Assistant
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
    /// Creates a new message with the specified role and content
    /// </summary>
    public Message(MessageRole role, string content)
    {
        Role = role;
        Content = content;
    }
}
