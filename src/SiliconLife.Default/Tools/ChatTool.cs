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

using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Chat tool for inter-being communication.
/// Allows silicon beings to send messages to other beings through ChatSystem.
/// Verifies the tool-to-chat-system pipeline.
/// </summary>
public class ChatTool : ITool
{
    public string Name => "chat";

    public string Description =>
        "Send a message to another silicon being or to the user. " +
        "Use this to communicate with other silicon beings in the collective.";

    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

    public Dictionary<string, object> GetParameterSchema()
    {
        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["target_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The GUID of the target being or user to send the message to"
                },
                ["message"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The message content to send"
                }
            },
            ["required"] = new[] { "target_id", "message" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        ChatSystem? chatSystem = ServiceLocator.Instance.ChatSystem;
        if (chatSystem == null)
        {
            return ToolResult.Failed("ChatSystem is not configured");
        }

        if (!parameters.TryGetValue("target_id", out object? targetObj) || string.IsNullOrWhiteSpace(targetObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'target_id' parameter");
        }

        if (!parameters.TryGetValue("message", out object? messageObj) || string.IsNullOrWhiteSpace(messageObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'message' parameter");
        }

        if (!Guid.TryParse(targetObj.ToString(), out Guid targetId))
        {
            return ToolResult.Failed($"Invalid target_id: '{targetObj}' — must be a valid GUID");
        }

        try
        {
            // Get or create the session between caller and target to obtain the correct session ID
            SessionBase session = chatSystem.GetOrCreateSession(callerId, targetId);

            string content = messageObj.ToString()!;
            ChatMessage chatMsg = new(callerId, session.Id, content)
            {
                Role = MessageRole.Assistant,
            };

            // Persist message to ChatSystem
            chatSystem.AddMessage(chatMsg);

            // Push via IMManager for real-time SSE delivery to frontend
            IMManager? imManager = ServiceLocator.Instance.IMManager;
            SiliconBeingManager? beingManager = ServiceLocator.Instance.BeingManager;
            SiliconBeingBase? callerBeing = beingManager?.GetBeing(callerId);
            string senderName = callerBeing?.Name ?? callerId.ToString();
            if (imManager != null)
            {
                _ = imManager.SendMessageAsync(callerId, session.Id, content, senderName: senderName);
            }

            return ToolResult.Successful($"Message sent to {targetId}");
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to send message: {ex.Message}");
        }
    }
}
