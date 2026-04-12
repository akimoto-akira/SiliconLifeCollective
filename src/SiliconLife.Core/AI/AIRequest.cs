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
/// Request to AI service
/// </summary>
public class AIRequest
{
    /// <summary>
    /// Gets or sets the model name to use
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of chat messages in the conversation.
    /// IAIClient implementations are responsible for converting ChatMessage to their specific API format.
    /// </summary>
    public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

    /// <summary>
    /// Gets or sets the list of tools available for the AI to call.
    /// Null or empty means no tools are available.
    /// </summary>
    public List<ToolDefinition>? Tools { get; set; }

    /// <summary>
    /// Creates a new AI request with the specified model
    /// </summary>
    public AIRequest(string model)
    {
        Model = model;
    }

    /// <summary>
    /// Adds a message to the conversation
    /// </summary>
    public void AddMessage(MessageRole role, string content)
    {
        Messages.Add(new ChatMessage
        {
            Role = role,
            Content = content,
        });
    }
}
