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

using System.Text;
using System.Text.Json;

using SiliconLife.Collective;

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
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<ContextManager>();
    private readonly IAIClient _aiClient;
    private readonly SiliconBeingBase _being;
    private readonly SessionBase? _session;

    private readonly List<ChatMessage> _messages;
    private readonly HashSet<Guid> _contextMessageIds;
    private bool _needsContinuation;
    private bool _hasNewPendingMessages;

    /// <summary>
    /// Gets whether this brain session has work to do.
    /// True if there are pending user messages or an unfinished tool call loop
    /// (detected from chat history — last message is a Tool result).
    /// </summary>
    public bool HasWork => _needsContinuation || _hasNewPendingMessages;

    /// <summary>
    /// Gets the session this context manager is associated with.
    /// May be null for non-chat scenarios (timer, task, memory compression).
    /// </summary>
    public SessionBase? Session => _session;

    /// <summary>
    /// Initializes a new instance of the ContextManager class.
    /// Loads history from ChatSystem and detects if a previous brain session
    /// left off mid-tool-call (continuation needed).
    /// </summary>
    /// <param name="being">The silicon being that owns this context</param>
    /// <param name="session">The chat session (single or group), or null for non-chat scenarios</param>
    /// <exception cref="ArgumentNullException">Thrown when being or its AIClient is null</exception>
    public ContextManager(SiliconBeingBase being, SessionBase? session)
    {
        _being = being ?? throw new ArgumentNullException(nameof(being));
        _aiClient = being.AIClient ?? throw new ArgumentNullException(nameof(being.AIClient));
        _session = session;
        _messages = new List<ChatMessage>();
        _contextMessageIds = new HashSet<Guid>();

        if (_being.Id != Guid.Empty && _session != null)
        {
            LoadHistoryMessages();
            FetchUnreadMessages();
            DetectContinuationFromHistory();
        }

        _logger.Info("ContextManager created for being {0}, session={1}", _being.Name, _session?.Id.ToString() ?? "null");
    }

    /// <summary>
    /// Loads historical messages from the session into context.
    /// Only tracks already-read messages in _contextMessageIds, so that
    /// FetchUnreadMessages can still pick up unread messages from the same session.
    /// </summary>
    private void LoadHistoryMessages()
    {
        if (_session == null) return;

        List<ChatMessage> history = _session.GetMessages();
        foreach (ChatMessage msg in history)
        {
            AddMessageToContext(msg);
            if (msg.ReadBy.Contains(_being.Id))
            {
                _contextMessageIds.Add(msg.Id);
            }
        }

        _logger.Debug("Loaded {0} history messages from session {1}", history.Count, _session.Id);
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
            _logger.Info("Detected continuation from history for being {0}", _being.Name);
        }
    }

    /// <summary>
    /// Checks if a being needs tool call continuation in a specific session.
    /// Looks at the last message in the session history.
    /// </summary>
    /// <param name="being">The silicon being to check</param>
    /// <param name="session">The chat session</param>
    /// <returns>True if the last history message is a Tool result</returns>
    public static bool NeedsContinuation(SiliconBeingBase being, SessionBase session)
    {
        List<ChatMessage> history = session.GetMessages();
        if (history.Count == 0)
        {
            return false;
        }

        ChatMessage last = history[^1];
        return (last.Role == MessageRole.Tool) || (string.IsNullOrEmpty(last.Content) && !string.IsNullOrEmpty(last.Thinking));
    }

    /// <summary>
    /// Fetches unread messages (not yet read by this being) from all sessions.
    /// GetPendingMessages already filters by ReadBy, so only truly new messages
    /// are returned. Deduplicates within this batch only (in case of repeated calls).
    /// </summary>
    private void FetchUnreadMessages()
    {
        ChatSystem? chatSystem = ServiceLocator.Instance.ChatSystem;
        if (chatSystem == null || _being.Id == Guid.Empty)
        {
            return;
        }

        List<ChatMessage> unread = chatSystem.GetPendingMessages(_being.Id);
        List<Guid> messageIdsToMark = new List<Guid>();
        
        foreach (ChatMessage msg in unread)
        {
            // Skip messages already processed in a previous FetchUnreadMessages call
            if (_contextMessageIds.Contains(msg.Id))
            {
                continue;
            }

            // Context already loaded by LoadHistoryMessages — just track
            _contextMessageIds.Add(msg.Id);
            _hasNewPendingMessages = true;
            messageIdsToMark.Add(msg.Id);
        }
        
        // Mark messages as read in the session
        if (messageIdsToMark.Count > 0 && _session != null)
        {
            _session.MarkMessagesAsRead(messageIdsToMark, _being.Id);
            _logger.Debug("Fetched {0} unread messages for being {1}", messageIdsToMark.Count, _being.Id);
        }
    }

    private void AddMessageToContext(ChatMessage msg)
    {
        _messages.Add(msg);
    }

    /// <summary>
    /// Adds an assistant message to the context and persists it
    /// </summary>
    private void AddAssistantMessage(string? content, string? thinking = null, int? promptTokens = null, int? completionTokens = null, int? totalTokens = null)
    {
        ChatMessage chatMsg = new(_being.Id, _session?.Id ?? Guid.Empty, content ?? string.Empty)
        {
            Role = MessageRole.Assistant,
            Thinking = thinking,
            PromptTokens = promptTokens,
            CompletionTokens = completionTokens,
            TotalTokens = totalTokens,
        };
        _messages.Add(chatMsg);

        if (_session != null)
        {
            ChatSystem? chatSystem = ServiceLocator.Instance.ChatSystem;
            if (chatSystem != null && _being.Id != Guid.Empty)
            {
                chatSystem.AddMessage(chatMsg);
            }
        }
    }

    private void RecordTokenUsage(AIResponse response)
    {
        if (response.PromptTokens == null && response.CompletionTokens == null && response.TotalTokens == null)
        {
            return;
        }

        ITokenUsageAudit? audit = ServiceLocator.Instance.TokenUsageAudit;
        if (audit == null)
        {
            return;
        }

        try
        {
            var record = new TokenUsageRecord(
                DateTime.UtcNow,
                _being.Id,
                _aiClient.GetType().Name,
                response.PromptTokens ?? 0,
                response.CompletionTokens ?? 0,
                response.TotalTokens ?? 0,
                _session?.Id ?? Guid.Empty,
                response.Success
            );

            audit.Record(record);
        }
        catch (Exception ex)
        {
            _logger.Warn("Failed to record token usage: {0}", ex.Message);
        }
    }

    /// <summary>
    /// Builds an AIRequest from the current context
    /// </summary>
    /// <param name="scenarioContext">Optional scenario-specific context from caller</param>
    private AIRequest BuildRequest(string? scenarioContext = null)
    {
        AIRequest request = new AIRequest(_aiClient.DefaultModel);

        if (!string.IsNullOrEmpty(_being.SoulContent))
        {
            request.Messages.Add(new ChatMessage
            {
                Role = MessageRole.System,
                Content = _being.SoulContent,
            });
        }

        Language language = Config.Instance?.Data?.Language ?? Language.ZhCN;
        request.Messages.Add(new ChatMessage
        {
            Role = MessageRole.System,
            Content = $"Your name: {_being.Name}\nYour GUID: {_being.Id}\nCurrent time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\nSystem language: {language}",
        });

        if (!string.IsNullOrEmpty(scenarioContext))
        {
            request.Messages.Add(new ChatMessage
            {
                Role = MessageRole.System,
                Content = scenarioContext,
            });
        }

        request.Messages.AddRange(_messages);

        ToolManager? toolManager = _being.ToolManager;
        if (toolManager != null && toolManager.ToolCount > 0)
        {
            request.Tools = toolManager.GetToolDefinitions();
        }

        _logger.Debug("Building AI request: {0} messages, {1} tools", request.Messages.Count, request.Tools?.Count ?? 0);

        return request;
    }

    /// <summary>
    /// Executes tool calls and returns tool result messages.
    /// Each tool result is added to the context messages.
    /// </summary>
    private List<ChatMessage> ExecuteToolCalls(List<ToolCall> toolCalls)
    {
        _logger.Info("Executing {0} tool calls for being {1}", toolCalls.Count, _being.Name);

        List<ChatMessage> toolResultMessages = new List<ChatMessage>();
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

            _logger.Info("Tool call: {0}, success={1}", toolCall.Name, result.Success);

            string resultContent = SerializeToolResult(result);
            string toolCallId = string.IsNullOrEmpty(toolCall.Id)
                ? Guid.NewGuid().ToString()
                : toolCall.Id;

            ChatMessage toolMsg = new(_being.Id, _session?.Id ?? Guid.Empty, resultContent)
            {
                Role = MessageRole.Tool,
                ToolCallId = toolCallId,
            };
            toolResultMessages.Add(toolMsg);
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
    /// <param name="scenarioContext">Optional scenario-specific context from caller</param>
    /// <returns>The AI response (may contain tool_calls or plain text)</returns>
    public AIResponse GetResponse(string? scenarioContext = null)
    {
        _logger.Info("Getting AI response for being {0}", _being.Name);

        AIRequest request = BuildRequest(scenarioContext);
        AIResponse response = _aiClient.Chat(request);

        if (response.Success && response.HasToolCalls)
        {
            _logger.Debug("AI returned tool calls, persisting intermediate round");
            PersistAndDeliverToolCallRound(response);
        }
        else if (response.Success)
        {
            _logger.Debug("AI returned text response, length={0}", response.Content?.Length ?? 0);
            AddAssistantMessage(response.Content, response.Thinking, response.PromptTokens, response.CompletionTokens, response.TotalTokens);
        }

        RecordTokenUsage(response);
        return response;
    }

    /// <summary>
    /// Sends the current context to AI and gets the response asynchronously.
    /// </summary>
    /// <param name="scenarioContext">Optional scenario-specific context from caller</param>
    /// <returns>The AI response (may contain tool_calls or plain text)</returns>
    public async Task<AIResponse> GetResponseAsync(string? scenarioContext = null)
    {
        AIRequest request = BuildRequest(scenarioContext);
        AIResponse response = await _aiClient.ChatAsync(request);

        if (response.Success && response.HasToolCalls)
        {
            _logger.Debug("AI returned tool calls (async), persisting intermediate round");
            PersistAndDeliverToolCallRound(response);
        }
        else if (response.Success)
        {
            _logger.Debug("AI returned text response (async), length={0}", response.Content?.Length ?? 0);
            AddAssistantMessage(response.Content, response.Thinking, response.PromptTokens, response.CompletionTokens, response.TotalTokens);
        }

        RecordTokenUsage(response);
        return response;
    }

    /// <summary>
    /// Sends the current context to AI using streaming and gets the response asynchronously.
    /// Streams incremental tokens to the IM layer for real-time frontend display.
    /// Falls back to non-streaming if the AI client does not support streaming.
    /// </summary>
    /// <param name="scenarioContext">Optional scenario-specific context from caller</param>
    /// <param name="cancellationToken">Cancellation token to abort the stream</param>
    /// <returns>The final AI response (assembled from all stream chunks)</returns>
    public async Task<AIResponse> GetResponseStreamAsync(string? scenarioContext = null, CancellationToken cancellationToken = default)
    {
        bool? streamingMode = _aiClient.StreamingMode;
        if (streamingMode == false)
        {
            _logger.Warn("Falling back to non-streaming mode");
            return await GetResponseAsync(scenarioContext);
        }

        AIRequest request = BuildRequest(scenarioContext);
        StringBuilder contentBuilder = new();
        StringBuilder thinkingBuilder = new();
        List<ToolCall>? toolCalls = null;
        string model = string.Empty;
        int? promptTokens = null;
        int? completionTokens = null;
        int? totalTokens = null;
        bool streamSucceeded = false;

        Guid streamId = Guid.NewGuid();

        try
        {
            await foreach (AIResponse chunk in _aiClient.ChatStreamAsync(request, cancellationToken))
            {
                if (!chunk.Success)
                {
                    return chunk;
                }

                if (!string.IsNullOrEmpty(chunk.Content))
                {
                    contentBuilder.Append(chunk.Content);
                    StreamChunk streamChunk = StreamChunk.Continue(streamId, chunk.Content, contentBuilder.Length, chunk.Thinking);
                    DeliverStreamChunk(streamChunk);
                }
                else if (!string.IsNullOrEmpty(chunk.Thinking))
                {
                    thinkingBuilder.Append(chunk.Thinking);
                    StreamChunk streamChunk = StreamChunk.Continue(streamId, "", contentBuilder.Length, chunk.Thinking);
                    DeliverStreamChunk(streamChunk);
                }

                if (chunk.HasToolCalls)
                {
                    toolCalls = chunk.ToolCalls;
                }

                if (chunk.IsStreamFinal)
                {
                    streamSucceeded = true;
                    model = chunk.Model;
                    promptTokens = chunk.PromptTokens;
                    completionTokens = chunk.CompletionTokens;
                    totalTokens = chunk.TotalTokens;
                }
            }
        }
        catch (NotImplementedException)
        {
            if (streamingMode == true)
            {
                return AIResponse.Failed("AI client requires streaming but ChatStreamAsync is not implemented");
            }
            _logger.Warn("Streaming not implemented, falling back to non-streaming mode");
            return await GetResponseAsync(scenarioContext);
        }

        if (!streamSucceeded)
        {
            if (streamingMode == true)
            {
                return AIResponse.Failed("AI client requires streaming but stream ended without final chunk");
            }
            _logger.Warn("Stream ended without final chunk, falling back to non-streaming mode");
            return await GetResponseAsync(scenarioContext);
        }

        StreamChunk endChunk = StreamChunk.End(streamId, promptTokens: promptTokens, completionTokens: completionTokens, totalTokens: totalTokens);
        DeliverStreamChunk(endChunk);

        string fullContent = contentBuilder.ToString();
        string? fullThinking = thinkingBuilder.Length > 0 ? thinkingBuilder.ToString() : null;

        AIResponse response = new AIResponse
        {
            Model = model,
            Content = fullContent,
            Thinking = fullThinking,
            ToolCalls = toolCalls,
            PromptTokens = promptTokens,
            CompletionTokens = completionTokens,
            TotalTokens = totalTokens,
            Success = true
        };

        if (response.Success && response.HasToolCalls)
        {
            PersistAndDeliverToolCallRound(response);
        }
        else if (response.Success)
        {
            AddAssistantMessage(response.Content, response.Thinking, response.PromptTokens, response.CompletionTokens, response.TotalTokens);
        }

        RecordTokenUsage(response);
        return response;
    }

    /// <summary>
    /// Scene: 1-on-1 chat.
    /// Perceives chat messages, calls AI, delivers response via IM.
    /// </summary>
    /// <returns>The AI response</returns>
    public AIResponse ThinkOnChat()
    {
        _logger.Info("ThinkOnChat: being={0}, session={1}", _being.Name, _session?.Id.ToString() ?? "null");

        string? scenarioContext = BuildSingleChatScenarioContext();
        AIResponse response = GetResponse(scenarioContext);

        if (response.Success && response.HasToolCalls)
        {
            // Tool calls executed, results persisted — yield time slice
            Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
            LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);
            string toolNames = string.Join(", ", response.ToolCalls!.Select(t => t.Name));
            RecordToMemory(loc.FormatMemoryEventToolCall(toolNames));
        }
        else if (response.Success && (!string.IsNullOrEmpty(response.Content) || !string.IsNullOrEmpty(response.Thinking)))
        {
            DeliverOutput(response.Content, response.Thinking, response.PromptTokens, response.CompletionTokens, response.TotalTokens);

            if (!string.IsNullOrEmpty(response.Content))
            {
                Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
                LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);
                Guid otherId = _session?.Members.FirstOrDefault(id => id != _being.Id) ?? Guid.Empty;
                SiliconBeingManager? beingManager = ServiceLocator.Instance.BeingManager;
                SiliconBeingBase? otherBeing = otherId != Guid.Empty ? beingManager?.GetBeing(otherId) : null;
                string partnerName = otherBeing?.Name ?? Config.Instance?.Data?.UserNickname ?? otherId.ToString();
                RecordToMemory(loc.FormatMemoryEventSingleChat(partnerName, response.Content));
            }
        }

        return response;
    }

    /// <summary>
    /// Builds scenario context for single chat.
    /// Includes information about the conversation partner (human or silicon being).
    /// </summary>
    private string? BuildSingleChatScenarioContext()
    {
        if (_session == null)
        {
            return null;
        }

        Guid otherId = _session.Members.FirstOrDefault(id => id != _being.Id);
        if (otherId == Guid.Empty)
        {
            return null;
        }

        SiliconBeingManager? beingManager = ServiceLocator.Instance.BeingManager;
        SiliconBeingBase? otherBeing = beingManager?.GetBeing(otherId);

        StringBuilder sb = new();
        sb.AppendLine("Scene: Single chat");

        if (otherBeing != null)
        {
            sb.AppendLine($"Partner type: Silicon being");
            sb.AppendLine($"Partner name: {otherBeing.Name}");
            sb.AppendLine($"Partner ID: {otherBeing.Id}");
        }
        else
        {
            sb.AppendLine($"Partner type: Human user");
            sb.AppendLine($"Partner name: {Config.Instance.Data.UserNickname}");
            sb.AppendLine($"Partner ID: {otherId}");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Scene: 1-on-1 chat with streaming support.
    /// Uses streaming when the AI client supports it, falling back to non-streaming otherwise.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to abort the stream</param>
    /// <returns>The AI response</returns>
    public async Task<AIResponse> ThinkOnChatStreamAsync(CancellationToken cancellationToken = default)
    {
        _logger.Info("ThinkOnChatStream: being={0}, session={1}", _being.Name, _session?.Id.ToString() ?? "null");

        string? scenarioContext = BuildSingleChatScenarioContext();
        AIResponse response = await GetResponseStreamAsync(scenarioContext, cancellationToken);

        if (response.Success && response.HasToolCalls)
        {
            // Tool calls executed, results persisted — yield time slice
            Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
            LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);
            string toolNames = string.Join(", ", response.ToolCalls!.Select(t => t.Name));
            RecordToMemory(loc.FormatMemoryEventToolCall(toolNames));
        }
        else if (response.Success && (!string.IsNullOrEmpty(response.Content) || !string.IsNullOrEmpty(response.Thinking)))
        {
            DeliverOutput(response.Content, response.Thinking, response.PromptTokens, response.CompletionTokens, response.TotalTokens);

            if (!string.IsNullOrEmpty(response.Content))
            {
                Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
                LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);
                Guid otherId = _session?.Members.FirstOrDefault(id => id != _being.Id) ?? Guid.Empty;
                SiliconBeingManager? beingManager = ServiceLocator.Instance.BeingManager;
                SiliconBeingBase? otherBeing = otherId != Guid.Empty ? beingManager?.GetBeing(otherId) : null;
                string partnerName = otherBeing?.Name ?? Config.Instance?.Data?.UserNickname ?? otherId.ToString();
                RecordToMemory(loc.FormatMemoryEventSingleChat(partnerName, response.Content));
            }
        }

        return response;
    }

    /// <summary>
    /// Scene: group chat.
    /// Perceives group messages, calls AI, delivers response in the group.
    /// </summary>
    /// <returns>The AI response</returns>
    public AIResponse ThinkOnGroupChat()
    {
        _logger.Info("ThinkOnGroupChat: being={0}", _being.Name);

        string? scenarioContext = null;
        AIResponse response = GetResponse(scenarioContext);

        if (response.Success && response.HasToolCalls)
        {
            // Tool calls executed, results persisted — yield time slice
            Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
            LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);
            string toolNames = string.Join(", ", response.ToolCalls!.Select(t => t.Name));
            RecordToMemory(loc.FormatMemoryEventToolCall(toolNames));
        }
        else if (response.Success && (!string.IsNullOrEmpty(response.Content) || !string.IsNullOrEmpty(response.Thinking)))
        {
            DeliverOutput(response.Content, response.Thinking, response.PromptTokens, response.CompletionTokens, response.TotalTokens);

            if (!string.IsNullOrEmpty(response.Content))
            {
                Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
                LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);
                string sessionId = _session?.Id.ToString() ?? string.Empty;
                RecordToMemory(loc.FormatMemoryEventGroupChat(sessionId, response.Content));
            }
        }

        return response;
    }

    /// <summary>
    /// Scene: group chat with streaming support.
    /// Uses streaming when the AI client supports it, falling back to non-streaming otherwise.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to abort the stream</param>
    /// <returns>The AI response</returns>
    public async Task<AIResponse> ThinkOnGroupChatStreamAsync(CancellationToken cancellationToken = default)
    {
        _logger.Info("ThinkOnGroupChatStream: being={0}", _being.Name);

        string? scenarioContext = null;
        AIResponse response = await GetResponseStreamAsync(scenarioContext, cancellationToken);

        if (response.Success && response.HasToolCalls)
        {
            // Tool calls executed, results persisted — yield time slice
            Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
            LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);
            string toolNames = string.Join(", ", response.ToolCalls!.Select(t => t.Name));
            RecordToMemory(loc.FormatMemoryEventToolCall(toolNames));
        }
        else if (response.Success && (!string.IsNullOrEmpty(response.Content) || !string.IsNullOrEmpty(response.Thinking)))
        {
            DeliverOutput(response.Content, response.Thinking, response.PromptTokens, response.CompletionTokens, response.TotalTokens);

            if (!string.IsNullOrEmpty(response.Content))
            {
                Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
                LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);
                string sessionId = _session?.Id.ToString() ?? string.Empty;
                RecordToMemory(loc.FormatMemoryEventGroupChat(sessionId, response.Content));
            }
        }

        return response;
    }

    /// <summary>
    /// Scene: task execution.
    /// Loads task context, calls AI, writes back task result.
    /// </summary>
    /// <param name="task">The task item to execute</param>
    /// <returns>The AI response</returns>
    public AIResponse ThinkOnTask(TaskItem task)
    {
        _logger.Info("ThinkOnTask: being={0}, task={1} ({2})", _being.Name, task.Title, task.Id);

        string? scenarioContext = BuildTaskScenarioContext(task);
        AIResponse response = GetResponse(scenarioContext);

        if (response.Success && !string.IsNullOrEmpty(response.Content))
        {
            Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
            LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);
            RecordToMemory(loc.FormatMemoryEventTask(response.Content));
        }

        return response;
    }

    /// <summary>
    /// Scene: scheduled timer.
    /// Loads timer context, calls AI, may not deliver output.
    /// </summary>
    /// <param name="timer">The timer item that triggered</param>
    /// <returns>The AI response</returns>
    public AIResponse ThinkOnTimer(TimerItem timer)
    {
        _logger.Info("ThinkOnTimer: being={0}, timer={1} ({2})", _being.Name, timer.Name, timer.Id);

        string? scenarioContext = BuildTimerScenarioContext(timer);
        AIResponse response = GetResponse(scenarioContext);

        if (response.Success && !string.IsNullOrEmpty(response.Content))
        {
            Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
            LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);
            RecordToMemory(loc.FormatMemoryEventTimer(response.Content));
        }

        return response;
    }

    private static string BuildTaskScenarioContext(TaskItem task)
    {
        StringBuilder sb = new();
        sb.AppendLine("Scene: Task execution");
        sb.AppendLine($"Task ID: {task.Id}");
        sb.AppendLine($"Task title: {task.Title}");
        if (!string.IsNullOrEmpty(task.Description))
            sb.AppendLine($"Task description: {task.Description}");
        sb.AppendLine($"Task priority: {task.Priority}");
        sb.AppendLine($"Task status: {task.Status}");
        sb.AppendLine($"Task created at: {task.CreatedAt:yyyy-MM-dd HH:mm:ss}");
        if (task.StartedAt.HasValue)
            sb.AppendLine($"Task started at: {task.StartedAt.Value:yyyy-MM-dd HH:mm:ss}");
        if (task.Dependencies.Count > 0)
            sb.AppendLine($"Task dependencies: {string.Join(", ", task.Dependencies)}");
        if (task.Metadata.Count > 0)
        {
            sb.AppendLine("Task metadata:");
            foreach (KeyValuePair<string, string> kv in task.Metadata)
                sb.AppendLine($"  {kv.Key}: {kv.Value}");
        }
        return sb.ToString();
    }

    private static string BuildTimerScenarioContext(TimerItem timer)
    {
        StringBuilder sb = new();
        sb.AppendLine("Scene: Scheduled timer triggered");
        sb.AppendLine($"Timer ID: {timer.Id}");
        sb.AppendLine($"Timer name: {timer.Name}");
        if (!string.IsNullOrEmpty(timer.Description))
            sb.AppendLine($"Timer description: {timer.Description}");
        sb.AppendLine($"Timer type: {timer.Type}");
        sb.AppendLine($"Timer calendar: {timer.CalendarId}");
        sb.AppendLine($"Timer conditions: {string.Join(", ", timer.CalendarConditions.Select(kv => $"{kv.Key}={kv.Value}"))}");
        sb.AppendLine($"Timer trigger time: {timer.TriggerTime:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine($"Times triggered: {timer.TimesTriggered}");
        if (timer.LastTriggeredAt.HasValue)
            sb.AppendLine($"Last triggered at: {timer.LastTriggeredAt.Value:yyyy-MM-dd HH:mm:ss}");
        if (timer.Metadata.Count > 0)
        {
            sb.AppendLine("Timer metadata:");
            foreach (KeyValuePair<string, string> kv in timer.Metadata)
                sb.AppendLine($"  {kv.Key}: {kv.Value}");
        }
        return sb.ToString();
    }

    /// <summary>
    /// Scene: memory compression.
    /// Compresses old memories level by level: second -> minute -> hour -> day -> month -> year.
    /// Uses Generate API (non-chat) for pure text compression.
    /// </summary>
    /// <returns>The AI response</returns>
    public AIResponse ThinkOnMemoryCompress()
    {
        _logger.Info("ThinkOnMemoryCompress: being={0}", _being.Name);

        if (_being.Memory == null)
        {
            return new AIResponse { Success = false, ErrorMessage = "Memory not initialized" };
        }

        var compressData = _being.Memory.FindLevelToCompress();
        if (!compressData.HasValue)
        {
            return new AIResponse { Success = true, Content = "No level to compress" };
        }

        var (level, entries) = compressData.Value;

        if (entries.Count == 0)
        {
            return new AIResponse { Success = true, Content = "No entries to compress" };
        }

        var memoryEntries = entries
            .Select(e => e.Data)
            .Where(e => e != null)
            .ToList();

        string levelDesc = GetLevelDescription(level);
        string rangeDesc = GetLevelRangeDescription(level);
        string contentText = string.Join("\n", memoryEntries.Select(e => $"[{e.Timestamp}] {e.Content}"));

        var language = Config.Instance?.Data?.Language ?? Language.ZhCN;
        var localization = LocalizationManager.Instance.GetLocalization(language);
        string systemPrompt = localization.MemoryCompressionSystemPrompt;
        string userPrompt = localization.GetMemoryCompressionUserPrompt(levelDesc, rangeDesc, contentText);

        AIResponse response = _aiClient.Generate(systemPrompt, userPrompt);

        if (response.Success && !string.IsNullOrEmpty(response.Content))
        {
            _being.Memory.AddSummary(level, $"[{level} Compression] {response.Content}");
        }

        return response;
    }

    private string GetLevelDescription(IncompleteDate level)
    {
        if (level.Second.HasValue)
            return $"second {level.Year}-{level.Month:D2}-{level.Day:D2} {level.Hour:D2}:{level.Minute:D2}:{level.Second:D2}";
        if (level.Minute.HasValue)
            return $"minute {level.Year}-{level.Month:D2}-{level.Day:D2} {level.Hour:D2}:{level.Minute:D2}";
        if (level.Hour.HasValue)
            return $"hour {level.Year}-{level.Month:D2}-{level.Day:D2} {level.Hour:D2}";
        if (level.Day.HasValue)
            return $"day {level.Year}-{level.Month:D2}-{level.Day:D2}";
        if (level.Month.HasValue)
            return $"month {level.Year}-{level.Month:D2}";
        return $"year {level.Year}";
    }

    private string GetLevelRangeDescription(IncompleteDate level)
    {
        var (start, end) = level.GetRange();
        
        if (level.Second.HasValue)
            return $"{start:yyyy-MM-dd HH:mm:ss} to {end:yyyy-MM-dd HH:mm:ss}";
        if (level.Minute.HasValue)
            return $"{start:yyyy-MM-dd HH:mm} to {end:yyyy-MM-dd HH:mm}";
        if (level.Hour.HasValue)
            return $"{start:yyyy-MM-dd HH} to {end:yyyy-MM-dd HH}";
        if (level.Day.HasValue)
            return $"{start:yyyy-MM-dd} to {end:yyyy-MM-dd}";
        if (level.Month.HasValue)
            return $"{start:yyyy-MM} to {end:yyyy-MM}";
        return $"{start:yyyy} to {end:yyyy}";
    }

    /// <summary>
    /// Delivers the AI's response to the user via IM or console.
    /// </summary>
    private void DeliverOutput(string content, string? thinking = null, int? promptTokens = null, int? completionTokens = null, int? totalTokens = null)
    {
        _logger.Debug("Delivering output for being {0}, length={1}", _being.Name, content.Length);

        IMManager? imManager = ServiceLocator.Instance.IMManager;
        if (imManager != null && _session != null)
        {
            _ = imManager.SendMessageAsync(_being.Id, _session.Id, content, thinking, _being.Name, promptTokens, completionTokens, totalTokens);
        }
        else
        {
            Console.WriteLine($"{_being.Name}: {content}");
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Delivers a streaming chunk to the IM layer for real-time frontend display.
    /// The chunk is accumulated in the WebUIProvider's streaming buffer,
    /// which the frontend polls via WebSocket.
    /// </summary>
    private void DeliverStreamChunk(StreamChunk chunk)
    {
        IMManager? imManager = ServiceLocator.Instance.IMManager;
        if (imManager != null && _session != null)
        {
            _ = imManager.SendStreamChunkAsync(_being.Id, _session.Id, chunk);
        }
    }

    /// <summary>
    /// Executes tool calls from an AI response and persists all intermediate messages
    /// (assistant with tool_calls + tool results) to chat history, then yields the time slice.
    /// </summary>
    private List<ChatMessage>? PersistToolCallRound(AIResponse response)
    {
        ChatMessage assistantMsg = new(_being.Id, _session?.Id ?? Guid.Empty, response.Content)
        {
            Role = MessageRole.Assistant,
            Thinking = response.Thinking,
            ToolCallsJson = JsonSerializer.Serialize(response.ToolCalls!),
            PromptTokens = response.PromptTokens,
            CompletionTokens = response.CompletionTokens,
            TotalTokens = response.TotalTokens,
        };
        _messages.Add(assistantMsg);

        return ExecuteToolCalls(response.ToolCalls!);
    }

    /// <summary>
    /// Persists the tool call round (assistant + tool results) to storage
    /// and delivers SSE events for real-time frontend display.
    /// </summary>
    private void PersistAndDeliverToolCallRound(AIResponse response)
    {
        ChatMessage assistantMsg = new(_being.Id, _session?.Id ?? Guid.Empty, response.Content)
        {
            Role = MessageRole.Assistant,
            Thinking = response.Thinking,
            ToolCallsJson = JsonSerializer.Serialize(response.ToolCalls!),
            PromptTokens = response.PromptTokens,
            CompletionTokens = response.CompletionTokens,
            TotalTokens = response.TotalTokens,
        };
        _messages.Add(assistantMsg);

        List<ChatMessage> toolResultMessages = ExecuteToolCalls(response.ToolCalls!);

        if (_session != null)
        {
            ChatSystem? chatSystem = ServiceLocator.Instance.ChatSystem;
            if (chatSystem != null && _being.Id != Guid.Empty)
            {
                chatSystem.AddMessage(assistantMsg);

                foreach (ChatMessage msg in toolResultMessages)
                {
                    chatSystem.AddMessage(msg);
                }
            }
        }

        DeliverToolUpdate(assistantMsg);
        foreach (ChatMessage msg in toolResultMessages)
        {
            DeliverToolUpdate(msg);
        }
    }

    /// <summary>
    /// Delivers a tool-related message to the IM layer for real-time frontend display.
    /// Handles both assistant messages with tool_calls and tool result messages.
    /// </summary>
    private void DeliverToolUpdate(ChatMessage msg)
    {
        IMManager? imManager = ServiceLocator.Instance.IMManager;
        if (imManager != null && _session != null)
        {
            string roleStr = msg.Role.ToString();
            _ = imManager.SendToolUpdateAsync(
                msg.SenderId,
                _session.Id,
                roleStr,
                msg.Content,
                msg.ToolCallsJson,
                msg.ToolCallId,
                msg.Thinking,
                _being.Name,
                msg.PromptTokens,
                msg.CompletionTokens,
                msg.TotalTokens);
        }
    }

    /// <summary>
    /// Records an event to the being's memory using localized text.
    /// No-op if the being has no memory system configured.
    /// </summary>
    private void RecordToMemory(string content)
    {
        if (_being.Memory == null || string.IsNullOrEmpty(content))
        {
            return;
        }

        try
        {
            Language language = Config.Instance?.Data?.Language ?? Language.ZhCN;
            LocalizationBase localization = LocalizationManager.Instance.GetLocalization(language);
            _being.Memory.Add(content, null);
        }
        catch (Exception ex)
        {
            _logger.Warn("Failed to record to memory: {0}", ex.Message);
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