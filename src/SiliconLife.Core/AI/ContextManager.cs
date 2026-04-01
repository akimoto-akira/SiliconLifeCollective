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
    private readonly Guid _beingId;

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
    {
        _aiClient = aiClient;
        _systemPrompt = systemPrompt;
        _chatService = chatService;
        _beingId = beingId;
        _messages = new List<Message>();
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
        return _chatService.GetPendingMessages(_beingId).Count > 0;
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

        List<string> messages = _chatService.GetPendingMessages(_beingId);
        if (messages.Count == 0)
        {
            return false;
        }

        foreach (string message in messages)
        {
            AddUserMessage(message);
        }

        return true;
    }

    /// <summary>
    /// Marks the first pending message as processed
    /// </summary>
    public void MarkMessageProcessed()
    {
        if (_chatService == null || _beingId == Guid.Empty)
        {
            return;
        }

        List<string> messages = _chatService.GetPendingMessages(_beingId);
        if (messages.Count > 0)
        {
            _chatService.MarkMessageProcessed(_beingId, messages[0]);
        }
    }

    /// <summary>
    /// Adds a user message to the context
    /// </summary>
    /// <param name="content">The message content</param>
    public void AddUserMessage(string content)
    {
        _messages.Add(new Message(MessageRole.User, content));
    }

    /// <summary>
    /// Adds an assistant message to the context
    /// </summary>
    /// <param name="content">The message content</param>
    public void AddAssistantMessage(string content)
    {
        _messages.Add(new Message(MessageRole.Assistant, content));
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
