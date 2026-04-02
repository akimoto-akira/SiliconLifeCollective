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

using System.Text.Json;

namespace SiliconLife.Collective;

/// <summary>
/// Manages AI context including system prompt and conversation history.
/// Acts as the "brain" of a silicon being: perceives input (loads history/pending),
/// thinks (calls AI), acts (executes tools), speaks (delivers output),
/// and remembers (persists to ChatSystem).
/// Supports Tool Call loop: AI returns tool_calls → execute tools →
/// feed results back → AI continues → until plain text response.
/// </summary>
public class ContextManager
{
    private readonly IAIClient _aiClient;
    private readonly SiliconBeingBase _being;

    private readonly List<Message> _messages;
    private readonly HashSet<Guid> _contextMessageIds;
    private readonly List<Guid> _unreadMessageIds;
    private bool _needsContinuation;
    private bool _hasNewPendingMessages;

    /// <summary>
    /// Gets whether this brain session has work to do.
    /// True if there are pending user messages or an unfinished tool call loop
    /// (detected from chat history — last message is a Tool result).
    /// </summary>
    public bool HasWork => _needsContinuation || _hasNewPendingMessages;

    /// <summary>
    /// Initializes a new instance of the ContextManager class.
    /// Loads history from ChatSystem and detects if a previous brain session
    /// left off mid-tool-call (continuation needed).
    /// </summary>
    /// <param name="being">The silicon being that owns this context</param>
    /// <exception cref="ArgumentNullException">Thrown when being or its AIClient is null</exception>
    public ContextManager(SiliconBeingBase being)
    {
        _being = being ?? throw new ArgumentNullException(nameof(being));
        _aiClient = being.AIClient ?? throw new ArgumentNullException(nameof(being.AIClient));
        _messages = new List<Message>();
        _contextMessageIds = new HashSet<Guid>();
        _unreadMessageIds = new List<Guid>();

        ChatSystem? chatSystem = ServiceRegistry.Instance.ChatSystem;
        if (chatSystem != null && _being.Id != Guid.Empty && _being.UserId != Guid.Empty)
        {
            LoadHistoryMessages();
            FetchUnreadMessages();
            DetectContinuationFromHistory();
        }
    }

    /// <summary>
    /// Loads historical messages from the single chat session into context.
    /// Only tracks already-read messages in _contextMessageIds, so that
    /// FetchUnreadMessages can still pick up unread messages from the same session.
    /// </summary>
    private void LoadHistoryMessages()
    {
        ChatSystem? chatSystem = ServiceRegistry.Instance.ChatSystem;
        if (chatSystem == null || _being.Id == Guid.Empty || _being.UserId == Guid.Empty)
        {
            return;
        }

        List<ChatMessage> history = chatSystem.GetMessages(_being.UserId, _being.Id);
        foreach (ChatMessage msg in history)
        {
            AddMessageToContext(msg);
            // Only track read messages — unread ones will be picked up by FetchUnreadMessages
            if (msg.ReadBy.Contains(_being.Id))
            {
                _contextMessageIds.Add(msg.Id);
            }
        }
    }

    /// <summary>
    /// Detects whether a previous brain session left off mid-tool-call.
    /// If the last message in history is a Tool result, the previous session
    /// executed tool calls but never got the AI's follow-up response.
    /// </summary>
    private void DetectContinuationFromHistory()
    {
        if (_messages.Count > 0 && _messages[^1].Role == MessageRole.Tool)
        {
            _needsContinuation = true;
        }
    }

    /// <summary>
    /// Checks if a being needs tool call continuation (for the body to detect before creating brain).
    /// Looks at the last message in ChatSystem history.
    /// </summary>
    /// <param name="being">The silicon being to check</param>
    /// <returns>True if the last history message is a Tool result</returns>
    public static bool NeedsContinuation(SiliconBeingBase being)
    {
        ChatSystem? chatSystem = ServiceRegistry.Instance.ChatSystem;
        if (chatSystem == null || being.Id == Guid.Empty || being.UserId == Guid.Empty)
        {
            return false;
        }

        List<ChatMessage> history = chatSystem.GetMessages(being.UserId, being.Id);
        if (history.Count == 0)
        {
            return false;
        }

        ChatMessage last = history[^1];
        return last.Role == MessageRole.Tool;
    }

    /// <summary>
    /// Fetches unread messages (not yet read by this being) from all sessions.
    /// GetPendingMessages already filters by ReadBy, so only truly new messages
    /// are returned. Deduplicates within this batch only (in case of repeated calls).
    /// </summary>
    private void FetchUnreadMessages()
    {
        ChatSystem? chatSystem = ServiceRegistry.Instance.ChatSystem;
        if (chatSystem == null || _being.Id == Guid.Empty)
        {
            return;
        }

        List<ChatMessage> unread = chatSystem.GetPendingMessages(_being.Id);
        foreach (ChatMessage msg in unread)
        {
            // Skip messages already processed in a previous FetchUnreadMessages call
            if (_contextMessageIds.Contains(msg.Id))
            {
                continue;
            }

            // Context already loaded by LoadHistoryMessages — just track for read marking
            _contextMessageIds.Add(msg.Id);
            _unreadMessageIds.Add(msg.Id);
            _hasNewPendingMessages = true;
        }
    }

    private void AddMessageToContext(ChatMessage msg)
    {
        if (msg.Role.HasValue)
        {
            AddMessageToContextByRole(msg);
        }
        else
        {
            // Legacy behavior: infer role from SenderId
            if (msg.SenderId == _being.UserId)
            {
                _messages.Add(new Message(MessageRole.User, msg.Content));
            }
            else if (msg.SenderId == _being.Id)
            {
                _messages.Add(new Message(MessageRole.Assistant, msg.Content));
            }
        }
    }

    private void AddMessageToContextByRole(ChatMessage msg)
    {
        MessageRole role = msg.Role!.Value;

        if (role == MessageRole.Assistant && !string.IsNullOrEmpty(msg.ToolCallsJson))
        {
            List<ToolCall>? toolCalls = null;
            try
            {
                toolCalls = JsonSerializer.Deserialize<List<ToolCall>>(msg.ToolCallsJson);
            }
            catch { /* ignore deserialization errors */ }

            _messages.Add(Message.AssistantWithToolCalls(
                msg.Content, toolCalls ?? [], msg.Thinking));
        }
        else if (role == MessageRole.Tool)
        {
            _messages.Add(Message.ToolResultMessage(msg.ToolCallId ?? "", msg.Content));
        }
        else
        {
            _messages.Add(new Message(role, msg.Content) { Thinking = msg.Thinking });
        }
    }

    /// <summary>
    /// Marks truly unread messages as read by this being in ChatSystem.
    /// Only messages fetched by FetchUnreadMessages are marked, not history.
    /// </summary>
    public void MarkMessageProcessed()
    {
        ChatSystem? chatSystem = ServiceRegistry.Instance.ChatSystem;
        if (chatSystem != null && _unreadMessageIds.Count > 0 && _being.Id != Guid.Empty)
        {
            chatSystem.MarkMessagesAsRead(_unreadMessageIds, _being.Id);
            _unreadMessageIds.Clear();
        }
    }

    /// <summary>
    /// Adds an assistant message to the context and persists it
    /// </summary>
    private void AddAssistantMessage(string content)
    {
        _messages.Add(new Message(MessageRole.Assistant, content));

        ChatSystem? chatSystem = ServiceRegistry.Instance.ChatSystem;
        if (chatSystem != null && _being.Id != Guid.Empty && _being.UserId != Guid.Empty)
        {
            chatSystem.AddMessage(_being.Id, _being.UserId, content);
        }
    }

    /// <summary>
    /// Builds an AIRequest from the current context
    /// </summary>
    private AIRequest BuildRequest()
    {
        AIRequest request = new AIRequest(_aiClient.DefaultModel);

        if (!string.IsNullOrEmpty(_being.SoulContent))
        {
            request.Messages.Add(new Message(MessageRole.System, _being.SoulContent));
        }

        request.Messages.AddRange(_messages);

        // Add tool definitions if ToolManager is available and has tools
        ToolManager? toolManager = _being.ToolManager;
        if (toolManager != null && toolManager.ToolCount > 0)
        {
            request.Tools = toolManager.GetToolDefinitions();
        }

        return request;
    }

    /// <summary>
    /// Executes tool calls and returns tool result messages.
    /// Each tool result is added to the context messages.
    /// </summary>
    private List<Message> ExecuteToolCalls(List<ToolCall> toolCalls)
    {
        List<Message> toolResultMessages = new List<Message>();
        ToolManager? toolManager = _being.ToolManager;

        foreach (ToolCall toolCall in toolCalls)
        {
            ToolResult result;
            if (toolManager != null)
            {
                result = toolManager.ExecuteTool(toolCall.Name, _being.Id, toolCall.Arguments);
            }
            else
            {
                result = ToolResult.Failed($"No tool manager available for tool '{toolCall.Name}'");
            }

            string resultContent = SerializeToolResult(result);
            string toolCallId = string.IsNullOrEmpty(toolCall.Id)
                ? Guid.NewGuid().ToString()
                : toolCall.Id;

            Message toolMsg = Message.ToolResultMessage(toolCallId, resultContent);
            toolResultMessages.Add(toolMsg);
            _messages.Add(toolMsg);
        }

        return toolResultMessages;
    }

    /// <summary>
    /// Serializes a ToolResult to a JSON string for sending back to the AI
    /// </summary>
    private static string SerializeToolResult(ToolResult result)
    {
        var obj = new Dictionary<string, object>
        {
            ["success"] = result.Success,
            ["message"] = result.Message ?? ""
        };

        if (result.Data != null)
        {
            obj["data"] = result.Data;
        }

        try
        {
            return JsonSerializer.Serialize(obj);
        }
        catch
        {
            return JsonSerializer.Serialize(new { success = result.Success, message = result.Message });
        }
    }

    /// <summary>
    /// Sends the current context to AI and gets the response.
    /// If AI returns tool_calls, executes them and persists intermediate messages to chat history.
    /// The next tick will load history and the AI will see the full context to continue.
    /// </summary>
    /// <returns>The AI response (may contain tool_calls or plain text)</returns>
    public AIResponse GetResponse()
    {
        AIRequest request = BuildRequest();
        AIResponse response = _aiClient.Chat(request);

        if (response.Success && response.HasToolCalls)
        {
            PersistToolCallRound(response);
        }
        else if (response.Success && !string.IsNullOrEmpty(response.Content))
        {
            AddAssistantMessage(response.Content);
        }

        return response;
    }

    /// <summary>
    /// Sends the current context to AI and gets the response asynchronously.
    /// </summary>
    /// <returns>The AI response (may contain tool_calls or plain text)</returns>
    public async Task<AIResponse> GetResponseAsync()
    {
        AIRequest request = BuildRequest();
        AIResponse response = await _aiClient.ChatAsync(request);

        if (response.Success && response.HasToolCalls)
        {
            PersistToolCallRound(response);
        }
        else if (response.Success && !string.IsNullOrEmpty(response.Content))
        {
            AddAssistantMessage(response.Content);
        }

        return response;
    }

    /// <summary>
    /// Scene: 1-on-1 chat.
    /// Perceives chat messages, calls AI, delivers response via IM.
    /// </summary>
    /// <returns>The AI response</returns>
    public AIResponse ThinkOnChat()
    {
        AIResponse response = GetResponse();

        if (response.Success && response.HasToolCalls)
        {
            // Tool calls executed, results persisted — yield time slice
        }
        else if (response.Success && !string.IsNullOrEmpty(response.Content))
        {
            DeliverOutput(response.Content);
        }

        MarkMessageProcessed();
        return response;
    }

    /// <summary>
    /// Scene: group chat.
    /// Perceives group messages, calls AI, delivers response in the group.
    /// </summary>
    /// <returns>The AI response</returns>
    public AIResponse ThinkOnGroupChat()
    {
        // TODO: Load group context, deliver to group
        AIResponse response = GetResponse();

        if (response.Success && response.HasToolCalls)
        {
            // Tool calls executed, results persisted — yield time slice
        }
        else if (response.Success && !string.IsNullOrEmpty(response.Content))
        {
            DeliverOutput(response.Content);
        }

        MarkMessageProcessed();
        return response;
    }

    /// <summary>
    /// Scene: task execution.
    /// Loads task context, calls AI, writes back task result.
    /// </summary>
    /// <returns>The AI response</returns>
    public AIResponse ThinkOnTask()
    {
        // TODO: Load task context, write back result
        AIResponse response = GetResponse();
        MarkMessageProcessed();
        return response;
    }

    /// <summary>
    /// Scene: scheduled timer.
    /// Loads timer context, calls AI, may not deliver output.
    /// </summary>
    /// <returns>The AI response</returns>
    public AIResponse ThinkOnTimer()
    {
        // TODO: Load timer context
        AIResponse response = GetResponse();
        MarkMessageProcessed();
        return response;
    }

    /// <summary>
    /// Delivers the AI's response to the user via IM or console.
    /// </summary>
    private void DeliverOutput(string content)
    {
        IMManager? imManager = ServiceRegistry.Instance.IMManager;
        if (imManager != null)
        {
            _ = imManager.SendMessageAsync(_being.Id, _being.UserId, $"{_being.Name}: {content}");
        }
        else
        {
            Console.WriteLine($"{_being.Name}: {content}");
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Executes tool calls from an AI response and persists all intermediate messages
    /// (assistant with tool_calls + tool results) to chat history, then yields the time slice.
    /// </summary>
    private void PersistToolCallRound(AIResponse response)
    {
        // Add assistant message with tool calls to in-memory context
        _messages.Add(Message.AssistantWithToolCalls(response.Content, response.ToolCalls!, response.Thinking));

        // Execute all tool calls (adds tool result messages to _messages)
        ExecuteToolCalls(response.ToolCalls!);

        // Persist the entire round (assistant + tool results) to ChatSystem
        PersistToolRoundToChatSystem();
    }

    /// <summary>
    /// Persists the last assistant+tool_calls message and all subsequent tool result messages to ChatSystem
    /// </summary>
    private void PersistToolRoundToChatSystem()
    {
        ChatSystem? chatSystem = ServiceRegistry.Instance.ChatSystem;
        if (chatSystem == null || _being.Id == Guid.Empty || _being.UserId == Guid.Empty)
        {
            return;
        }

        // Find the last assistant message with tool calls
        int lastAssistantIndex = -1;
        for (int i = _messages.Count - 1; i >= 0; i--)
        {
            if (_messages[i].Role == MessageRole.Assistant && _messages[i].ToolCalls != null)
            {
                lastAssistantIndex = i;
                break;
            }
        }

        if (lastAssistantIndex < 0)
        {
            return;
        }

        // Persist all messages from the assistant message onwards
        for (int i = lastAssistantIndex; i < _messages.Count; i++)
        {
            Message msg = _messages[i];
            if (msg.Role == MessageRole.Assistant)
            {
                ChatMessage chatMsg = new(_being.Id, _being.UserId, msg.Content)
                {
                    Role = MessageRole.Assistant,
                    Thinking = msg.Thinking,
                };
                if (msg.ToolCalls != null && msg.ToolCalls.Count > 0)
                {
                    chatMsg.ToolCallsJson = JsonSerializer.Serialize(msg.ToolCalls);
                }
                chatSystem.AddMessage(chatMsg);
            }
            else if (msg.Role == MessageRole.Tool)
            {
                ChatMessage chatMsg = new(_being.Id, _being.UserId, msg.Content)
                {
                    Role = MessageRole.Tool,
                    ToolCallId = msg.ToolCallId,
                };
                chatSystem.AddMessage(chatMsg);
            }
        }
    }

    /// <summary>
    /// Clears all messages from the context
    /// </summary>
    public void ClearMessages()
    {
        _messages.Clear();
    }

    /// <summary>
    /// Gets the number of messages in the context
    /// </summary>
    public int MessageCount => _messages.Count;
}
