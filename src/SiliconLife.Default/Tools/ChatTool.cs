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
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<ChatTool>();
    
    public string Name => "chat";

    public string Description =>
        "Send a message to another silicon being or to the user, or mark all pending messages from a target as read. " +
        "Use 'send' action to communicate; use 'mark_read' action to acknowledge messages without replying (read but no response).";

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
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["enum"] = new[] { "send", "mark_read" },
                    ["description"] = "Action to perform: 'send' to send a message, 'mark_read' to mark all pending messages from target as read without replying"
                },
                ["target_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The GUID of the target being or user"
                },
                ["message"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The message content to send (required for 'send' action, optional for 'mark_read')"
                }
            },
            ["required"] = new[] { "action", "target_id" }
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

        if (!Guid.TryParse(targetObj.ToString(), out Guid targetId))
        {
            return ToolResult.Failed($"Invalid target_id: '{targetObj}' — must be a valid GUID");
        }

        // Determine action (default to 'send' for backward compatibility)
        string action = "send";
        if (parameters.TryGetValue("action", out object? actionObj) && !string.IsNullOrWhiteSpace(actionObj?.ToString()))
        {
            action = actionObj.ToString()!.ToLowerInvariant();
        }

        try
        {
            switch (action)
            {
                case "send":
                    return ExecuteSend(callerId, targetId, parameters, chatSystem);
                case "mark_read":
                    return ExecuteMarkRead(callerId, targetId, chatSystem);
                default:
                    return ToolResult.Failed($"Unknown action: '{action}'. Valid actions are: 'send', 'mark_read'");
            }
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to execute chat action: {ex.Message}");
        }
    }

    /// <summary>
    /// Execute the 'send' action: send a message to the target.
    /// </summary>
    private ToolResult ExecuteSend(Guid callerId, Guid targetId, Dictionary<string, object> parameters, ChatSystem chatSystem)
    {
        if (!parameters.TryGetValue("message", out object? messageObj) || string.IsNullOrWhiteSpace(messageObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'message' parameter for 'send' action");
        }

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

    /// <summary>
    /// Execute the 'mark_read' action: mark all pending messages from the target as read.
    /// This allows the caller to acknowledge messages without sending a reply (read but no response).
    /// </summary>
    private ToolResult ExecuteMarkRead(Guid callerId, Guid targetId, ChatSystem chatSystem)
    {
        // Get the session between caller and target
        SessionBase session = chatSystem.GetOrCreateSession(callerId, targetId);

        // Get pending messages from the target (messages sent by target that caller hasn't read)
        List<ChatMessage> pendingMessages = session.GetPendingMessages(callerId);

        // Filter to only messages from the target
        List<Guid> messageIdsToMark = pendingMessages
            .Where(msg => msg.SenderId == targetId)
            .Select(msg => msg.Id)
            .ToList();

        if (messageIdsToMark.Count == 0)
        {
            return ToolResult.Successful($"No pending messages from {targetId} to mark as read");
        }

        // Mark all as read
        session.MarkMessagesAsRead(messageIdsToMark, callerId);

        _logger.Info(callerId, "Marked {0} messages from {1} as read (mark_read action)", messageIdsToMark.Count, targetId);

        return ToolResult.Successful($"Marked {messageIdsToMark.Count} message(s) from {targetId} as read");
    }
}
