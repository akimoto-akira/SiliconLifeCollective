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

using SiliconLife.Common.Localization;

namespace SiliconLife.Common.Tools;

/// <summary>
/// Chat tool for inter-being communication.
/// Allows silicon beings to send messages to other beings through ChatSystem.
/// Verifies the tool-to-chat-system pipeline.
/// </summary>
[ToolAction("send", "mark_read")]
[ToolScenario(ToolScenarioFlag.Chat)]
[ChatOnly]
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
                ["chat_type"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["enum"] = new[] { "single", "group" },
                    ["description"] = "Chat type: 'single' for direct message with one being, 'group' for group chat session"
                },
                ["target_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The GUID of the target being (for 'single' chat) or group session (for 'group' chat)"
                },
                ["message"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The message content to send (required for 'send' action, optional for 'mark_read')"
                }
            },
            ["required"] = new[] { "action", "chat_type", "target_id" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        ChatSystem? chatSystem = ServiceLocator.Instance.ChatSystem;
        if (chatSystem == null)
        {
            return ToolResult.Failed("ChatSystem is not configured");
        }

        // Validate chat_type (required)
        if (!parameters.TryGetValue("chat_type", out object? chatTypeObj) || string.IsNullOrWhiteSpace(chatTypeObj?.ToString()))
        {
            return ToolResult.Failed("Missing required 'chat_type' parameter. Must be 'single' or 'group'");
        }

        string chatType = chatTypeObj.ToString()!.ToLowerInvariant();
        if (chatType != "single" && chatType != "group")
        {
            return ToolResult.Failed($"Invalid chat_type: '{chatType}'. Must be 'single' or 'group'");
        }

        // Validate target_id (required)
        if (!parameters.TryGetValue("target_id", out object? targetObj) || string.IsNullOrWhiteSpace(targetObj?.ToString()))
        {
            return ToolResult.Failed("Missing required 'target_id' parameter");
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
                    return ExecuteSend(callerId, targetId, chatType, parameters, chatSystem);
                case "mark_read":
                    return ExecuteMarkRead(callerId, targetId, chatType, chatSystem);
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
    private ToolResult ExecuteSend(Guid callerId, Guid targetId, string chatType, Dictionary<string, object> parameters, ChatSystem chatSystem)
    {
        if (!parameters.TryGetValue("message", out object? messageObj) || string.IsNullOrWhiteSpace(messageObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'message' parameter for 'send' action");
        }

        SessionBase? session;
        if (chatType == "single")
        {
            // For single chat: target_id is the other being's ID, get or create session between caller and target
            session = chatSystem.GetOrCreateSession(callerId, targetId);
        }
        else
        {
            // For group chat: target_id is the group session ID, must exist
            session = chatSystem.GetSession(targetId);
            if (session == null)
            {
                return ToolResult.Failed($"Group session {targetId} not found. Cannot send message to non-existent group.");
            }
        }

        string content = messageObj.ToString()!;
        ChatMessage chatMsg = new(callerId, session.Id, content)
        {
            Role = MessageRole.Assistant,
        };

        if (chatType == "group" && session.Type == SessionType.GroupChat)
        {
            chatMsg.MentionedIds = MentionParser.ParseMentionedIds(content, session.Members);
        }

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

        string targetType = chatType == "single" ? "being" : "group";
        return ToolResult.Successful($"Message sent to {targetType} {targetId}");
    }

    /// <summary>
    /// Execute the 'mark_read' action: mark all pending messages from the target as read.
    /// This allows the caller to acknowledge messages without sending a reply (read but no response).
    /// </summary>
    private ToolResult ExecuteMarkRead(Guid callerId, Guid targetId, string chatType, ChatSystem chatSystem)
    {
        SessionBase? session;
        if (chatType == "single")
        {
            // For single chat: target_id is the other being's ID, get or create session
            session = chatSystem.GetOrCreateSession(callerId, targetId);
        }
        else
        {
            // For group chat: target_id is the group session ID, must exist
            session = chatSystem.GetSession(targetId);
            if (session == null)
            {
                return ToolResult.Failed($"Group session {targetId} not found. Cannot mark messages as read for non-existent group.");
            }
        }

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

        string targetType = chatType == "single" ? "being" : "group";
        _logger.Info(callerId, "Marked {0} messages from {1} ({2}) as read (mark_read action)", messageIdsToMark.Count, targetId, targetType);

        return ToolResult.Successful($"Marked {messageIdsToMark.Count} message(s) from {targetType} {targetId} as read");
    }
}
