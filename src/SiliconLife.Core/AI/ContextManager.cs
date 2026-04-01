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
/// Manages AI context including system prompt and conversation history
/// </summary>
public class ContextManager
{
    private readonly IAIClient _aiClient;
    private readonly string? _systemPrompt;
    private readonly List<Message> _messages;
    private readonly IChatService? _chatService;
    private readonly ChatSystem? _chatSystem;
    private readonly Guid _beingId;
    private readonly Guid _userId;
    private readonly List<Guid> _processedMessageIds;

    /// <summary>
    /// Initializes a new instance of the ContextManager class
    /// </summary>
    /// <param name="aiClient">The AI client to use</param>
    /// <param name="systemPrompt">Optional system prompt (soul content)</param>
    public ContextManager(IAIClient aiClient, string? systemPrompt = null)
        : this(aiClient, systemPrompt, null, Guid.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the ContextManager class
    /// </summary>
    /// <param name="aiClient">The AI client to use</param>
    /// <param name="systemPrompt">Optional system prompt (soul content)</param>
    /// <param name="chatService">The chat service for fetching messages</param>
    /// <param name="beingId">The silicon being ID</param>
    public ContextManager(IAIClient aiClient, string? systemPrompt, IChatService? chatService, Guid beingId)
        : this(aiClient, systemPrompt, chatService, null, beingId, Guid.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the ContextManager class with ChatSystem support
    /// </summary>
    /// <param name="aiClient">The AI client to use</param>
    /// <param name="systemPrompt">Optional system prompt (soul content)</param>
    /// <param name="chatSystem">The chat system for persistent message storage</param>
    /// <param name="beingId">The silicon being ID</param>
    /// <param name="userId">The user ID</param>
    public ContextManager(IAIClient aiClient, string? systemPrompt, ChatSystem chatSystem, Guid beingId, Guid userId)
        : this(aiClient, systemPrompt, null, chatSystem, beingId, userId)
    {
    }

    /// <summary>
    /// Initializes a new instance of the ContextManager class
    /// </summary>
    /// <param name="aiClient">The AI client to use</param>
    /// <param name="systemPrompt">Optional system prompt (soul content)</param>
    /// <param name="chatService">The chat service for fetching messages</param>
    /// <param name="chatSystem">The chat system for persistent message storage</param>
    /// <param name="beingId">The silicon being ID</param>
    /// <param name="userId">The user ID</param>
    private ContextManager(
        IAIClient aiClient,
        string? systemPrompt,
        IChatService? chatService,
        ChatSystem? chatSystem,
        Guid beingId,
        Guid userId)
    {
        _aiClient = aiClient;
        _systemPrompt = systemPrompt;
        _chatService = chatService;
        _chatSystem = chatSystem;
        _beingId = beingId;
        _userId = userId;
        _messages = new List<Message>();
        _processedMessageIds = new List<Guid>();

        if (_chatSystem != null && _beingId != Guid.Empty && _userId != Guid.Empty)
        {
            LoadHistoryMessages();
            FetchPendingFromChatSystem();
        }
    }

    /// <summary>
    /// Loads historical messages from ChatSystem
    /// </summary>
    private void LoadHistoryMessages()
    {
        if (_chatSystem == null || _beingId == Guid.Empty || _userId == Guid.Empty)
        {
            return;
        }

        List<ChatMessage> history = _chatSystem.GetMessages(_userId, _beingId);
        foreach (ChatMessage msg in history)
        {
            AddMessageToContext(msg);
            _processedMessageIds.Add(msg.Id);
        }
    }

    /// <summary>
    /// Fetches pending messages from ChatSystem and adds them to context
    /// </summary>
    private void FetchPendingFromChatSystem()
    {
        if (_chatSystem == null || _beingId == Guid.Empty || _userId == Guid.Empty)
        {
            return;
        }

        List<ChatMessage> pending = _chatSystem.GetPendingMessages(_beingId);
        foreach (ChatMessage msg in pending)
        {
            if (!_processedMessageIds.Contains(msg.Id))
            {
                AddMessageToContext(msg);
                _processedMessageIds.Add(msg.Id);
            }
        }
    }

    private void AddMessageToContext(ChatMessage msg)
    {
        if (msg.SenderId == _userId)
        {
            _messages.Add(new Message(MessageRole.User, msg.Content));
        }
        else if (msg.SenderId == _beingId)
        {
            _messages.Add(new Message(MessageRole.Assistant, msg.Content));
        }
    }

    /// <summary>
    /// Checks if there are pending messages to process
    /// </summary>
    /// <returns>True if there are pending messages</returns>
    public bool HasPendingMessages()
    {
        if (_chatService == null || _beingId == Guid.Empty)
        {
            return false;
        }
        return _chatService!.GetPendingMessages(_beingId).Count > 0;
    }

    /// <summary>
    /// Fetches pending messages from chat service and adds them to context
    /// </summary>
    /// <returns>True if messages were fetched</returns>
    public bool FetchPendingMessages()
    {
        if (_chatService == null || _beingId == Guid.Empty)
        {
            return false;
        }

        List<ChatMessage> messages = _chatService.GetPendingMessages(_beingId);
        if (messages.Count == 0)
        {
            return false;
        }

        foreach (ChatMessage msg in messages)
        {
            _messages.Add(new Message(MessageRole.User, msg.Content));
        }

        return true;
    }

    /// <summary>
    /// Marks the first pending message as processed
    /// </summary>
    public void MarkMessageProcessed()
    {
        if (_chatService != null && _beingId != Guid.Empty)
        {
            List<ChatMessage> messages = _chatService.GetPendingMessages(_beingId);
            if (messages.Count > 0)
            {
                _chatService.MarkMessageProcessed(_beingId, messages[0].Id);
            }
        }

        if (_chatSystem != null && _processedMessageIds.Count > 0)
        {
            _chatSystem.MarkMessagesAsProcessed(_processedMessageIds);
            _processedMessageIds.Clear();
        }
    }

    /// <summary>
    /// Adds an assistant message to the context and persists it
    /// </summary>
    private void AddAssistantMessage(string content)
    {
        _messages.Add(new Message(MessageRole.Assistant, content));

        if (_chatSystem != null && _beingId != Guid.Empty && _userId != Guid.Empty)
        {
            _chatSystem.AddMessage(_beingId, _userId, content);
        }
    }

    /// <summary>
    /// Sends the current context to AI and gets the response
    /// </summary>
    /// <returns>The AI response</returns>
    public AIResponse GetResponse()
    {
        AIRequest request = new AIRequest(_aiClient.DefaultModel);

        if (!string.IsNullOrEmpty(_systemPrompt))
        {
            request.Messages.Add(new Message(MessageRole.System, _systemPrompt));
        }

        request.Messages.AddRange(_messages);

        AIResponse response = _aiClient.Chat(request);

        if (response.Success && !string.IsNullOrEmpty(response.Content))
        {
            AddAssistantMessage(response.Content);
        }

        return response;
    }

    /// <summary>
    /// Sends the current context to AI and gets the response asynchronously
    /// </summary>
    /// <returns>The AI response</returns>
    public async Task<AIResponse> GetResponseAsync()
    {
        AIRequest request = new AIRequest(_aiClient.DefaultModel);

        if (!string.IsNullOrEmpty(_systemPrompt))
        {
            request.Messages.Add(new Message(MessageRole.System, _systemPrompt));
        }

        request.Messages.AddRange(_messages);

        AIResponse response = await _aiClient.ChatAsync(request);

        if (response.Success && !string.IsNullOrEmpty(response.Content))
        {
            AddAssistantMessage(response.Content);
        }

        return response;
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
