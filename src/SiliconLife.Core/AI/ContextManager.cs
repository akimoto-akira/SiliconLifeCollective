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

using System.IO;
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
#if DEBUG
    public const string AIDebugLogDirectory = "AIDebugLogs";
#endif

    /// <summary>
    /// Maximum number of recent chat messages loaded into context per AI request.
    /// Limits the context window to avoid unbounded token consumption during
    /// long-running sessions. Older messages are still preserved in storage and
    /// summarized into long-term memory via the compression pipeline.
    /// </summary>
    private const int MaxContextMessages = 20;

    private static readonly ILogger _logger = LogManager.Instance.GetLogger<ContextManager>();
    private IAIClient _aiClient;
    private readonly SiliconBeingBase _being;
    private readonly SessionBase? _session;

    private readonly List<ChatMessage> _messages;
    private readonly HashSet<Guid> _contextMessageIds;
    private readonly List<Guid> _pendingMarkAsReadIds;
    private bool _needsContinuation;
    private bool _hasNewPendingMessages;

    /// <summary>
    /// Timer context (for timer scenarios, null for chat/task scenarios)
    /// </summary>
    private TimerItem? _timerForContext;

    /// <summary>
    /// Gets whether this brain session has work to do.
    /// True if there are pending user messages or an unfinished tool call loop
    /// (detected from chat history → last message is a Tool result).
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
        _pendingMarkAsReadIds = new List<Guid>();

        if (_being.Id != Guid.Empty && _session != null)
        {
            LoadHistoryMessages();
            FetchUnreadMessages();
            DetectContinuationFromHistory();
        }

        _logger.Info(_being.Id, "ContextManager created for being {0}, session={1}", _being.Name, _session?.Id.ToString() ?? "null");
    }

    /// <summary>
    /// Initializes a new instance of the ContextManager class for timer execution.
    /// Creates or loads timer execution context from JSON file.
    /// </summary>
    /// <param name="being">The silicon being that owns this context</param>
    /// <param name="timer">The timer item for execution</param>
    /// <exception cref="ArgumentNullException">Thrown when being or its AIClient is null</exception>
    public ContextManager(SiliconBeingBase being, TimerItem timer)
    {
        _being = being ?? throw new ArgumentNullException(nameof(being));
        _aiClient = being.AIClient ?? throw new ArgumentNullException(nameof(being.AIClient));
        _session = null;
        _messages = new List<ChatMessage>();
        _contextMessageIds = new HashSet<Guid>();
        _pendingMarkAsReadIds = new List<Guid>();
        _timerForContext = timer;

        ChatHistoryCycle currentCycle = timer.GetCurrentCycle();
        if (currentCycle.Messages.Count > 0)
        {
            foreach (var msg in currentCycle.Messages)
            {
                _messages.Add(msg);
            }
        }

        _logger.Info(_being.Id, "ContextManager created for being {0}, timer={1}", _being.Name, timer.Name);
    }

    private TaskItem? _taskForContext;

    public ContextManager(SiliconBeingBase being, TaskItem task)
    {
        _being = being ?? throw new ArgumentNullException(nameof(being));
        _aiClient = being.AIClient ?? throw new ArgumentNullException(nameof(being.AIClient));
        _session = null;
        _messages = new List<ChatMessage>();
        _contextMessageIds = new HashSet<Guid>();
        _pendingMarkAsReadIds = new List<Guid>();
        _taskForContext = task;

        ChatHistoryCycle currentCycle = task.GetCurrentCycle();
        if (currentCycle.Messages.Count > 0)
        {
            foreach (var msg in currentCycle.Messages)
            {
                _messages.Add(msg);
            }
        }

        _logger.Info(_being.Id, "ContextManager created for being {0}, task={1} ({2})", _being.Name, task.Title, task.Id);
    }

    /// <summary>
    /// Loads historical messages from the session into context.
    /// Applies a sliding context window (MaxContextMessages) to keep token
    /// usage bounded over long sessions. Older messages remain in storage and
    /// surface back through the memory-context injection pipeline.
    /// Only tracks already-read messages in _contextMessageIds, so that
    /// FetchUnreadMessages can still pick up unread messages from the same session.
    /// </summary>
    private void LoadHistoryMessages()
    {
        if (_session == null) return;

        List<ChatMessage> history = _session.GetMessages(MaxContextMessages);

        // Add messages in chronological order (from earliest to latest)
        foreach (ChatMessage msg in history)
        {
            AddMessageToContext(msg);
            if (msg.ReadBy.Contains(_being.Id))
            {
                _contextMessageIds.Add(msg.Id);
            }
        }

        _logger.Debug(_being.Id, "Loaded {0} history messages from session {1} (window={2})",
            history.Count, _session.Id, MaxContextMessages);
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
            _logger.Info(_being.Id, "Detected continuation from history for being {0}", _being.Name);
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
        
        foreach (ChatMessage msg in unread)
        {
            // Skip messages already processed in a previous FetchUnreadMessages call
            if (_contextMessageIds.Contains(msg.Id))
            {
                continue;
            }

            // Verify message belongs to the current session
            if (_session != null && msg.ChannelId != Guid.Empty && msg.ChannelId != _session.Id)
            {
                _logger.Debug(_being.Id, "Skipping message {0} from different session {1}", msg.Id, msg.ChannelId);
                continue;
            }

            // Context already loaded by LoadHistoryMessages → just track
            _contextMessageIds.Add(msg.Id);
            _hasNewPendingMessages = true;
            _pendingMarkAsReadIds.Add(msg.Id);
        }
        
        if (_pendingMarkAsReadIds.Count > 0)
        {
            _logger.Debug(_being.Id, "Fetched {0} unread messages for being {1}", _pendingMarkAsReadIds.Count, _being.Id);
        }
    }

    /// <summary>
    /// Commits pending messages as read after AI processing succeeds.
    /// Should only be called when AI response is successful, so that
    /// failed messages remain unread and can be retried on the next tick.
    /// </summary>
    public void CommitMessagesAsRead()
    {
        if (_pendingMarkAsReadIds.Count > 0 && _session != null)
        {
            _session.MarkMessagesAsRead(_pendingMarkAsReadIds, _being.Id);
            _logger.Debug(_being.Id, "Committed {0} messages as read for being {1}", _pendingMarkAsReadIds.Count, _being.Id);
            _pendingMarkAsReadIds.Clear();
            
            // Reset the flag since messages are now marked as read
            _hasNewPendingMessages = false;
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

        if (_timerForContext != null)
        {
            _timerForContext.GetCurrentCycle().Messages.Add(chatMsg);
            _being.TimerSystem?.Save();
        }
        else if (_taskForContext != null)
        {
            _taskForContext.GetCurrentCycle().Messages.Add(chatMsg);
            TaskCenter.Instance.UpdateTask(_taskForContext);
        }
        else if (_session != null)
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
            var now = DateTime.UtcNow;
            var record = new TokenUsageRecord(
                new IncompleteDate(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second),
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
            _logger.Warn(_being.Id, "Failed to record token usage: {0}", ex.Message);
        }
    }

    /// <summary>
    /// Builds an AIRequest from the current context
    /// </summary>
    /// <param name="scenarioContext">Optional scenario-specific context from caller</param>
    private AIRequest BuildRequest(string? scenarioContext = null, TaskItem? task = null, ToolScenarioFlag scenario = ToolScenarioFlag.All)
    {
        // Record unread user messages to memory before building the request
        RecordUnreadMessagesToMemory();

        AIRequest request = new AIRequest(_aiClient.DefaultModel);

        if (!string.IsNullOrEmpty(_being.SoulContent))
        {
            request.Messages.Add(new ChatMessage
            {
                Role = MessageRole.System,
                Content = _being.SoulContent,
            });
        }

        // Add common prompt (behavior guidelines shared by all silicon beings)
        Language language = Config.Instance?.Data?.Language ?? Language.ZhCN;
        LocalizationBase loc = LocalizationManager.Instance.GetLocalization(language);
        request.Messages.Add(new ChatMessage
        {
            Role = MessageRole.System,
            Content = loc.CommonSystemPrompt,
        });

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

        // Knowledge network context (placed after scenario, before memory).
        // Provides the AI with awareness of what knowledge is available in the graph,
        // enabling it to query and use knowledge for decision-making.
        string? knowledgeContext = BuildKnowledgeContext();
        if (!string.IsNullOrEmpty(knowledgeContext))
        {
            request.Messages.Add(new ChatMessage
            {
                Role = MessageRole.System,
                Content = knowledgeContext,
            });
        }

        // Memory context (placed at the end of system prompts, immediately before the
        // conversation). Rationale:
        // - Closest to the dialog, so the model's attention is strongest here.
        // - Semantic layering: earlier system messages = instructions, last one = context.
        // - Aligns with RAG best practice (retrieved info at end of system prompt).
        string? memoryContext = BuildMemoryContext();
        if (!string.IsNullOrEmpty(memoryContext))
        {
            request.Messages.Add(new ChatMessage
            {
                Role = MessageRole.System,
                Content = memoryContext,
            });
        }

        request.Messages.AddRange(_messages);

        ToolManager? toolManager = _being.ToolManager;
        if (toolManager != null && toolManager.ToolCount > 0 && _aiClient.SupportsToolCalls != false)
        {
            if (task?.RequiredTools != null && task.RequiredTools.Count > 0)
            {
                request.Tools = toolManager.GetToolDefinitions(task.RequiredTools);
            }
            else
            {
                request.Tools = toolManager.GetToolDefinitions(scenario);
            }
        }

        _logger.Debug(_being.Id, "Building AI request: {0} messages, {1} tools, memory={2}",
            request.Messages.Count, request.Tools?.Count ?? 0, !string.IsNullOrEmpty(memoryContext));

        return request;
    }

    /// <summary>
    /// Records all unread user messages to memory before AI processing.
    /// Only records User role messages from other beings (not self, not Tool messages).
    /// </summary>
    private void RecordUnreadMessagesToMemory()
    {
        if (_being.Memory == null || _pendingMarkAsReadIds.Count == 0)
        {
            return;
        }

        try
        {
            Language language = Config.Instance?.Data?.Language ?? Language.ZhCN;
            LocalizationBase loc = LocalizationManager.Instance.GetLocalization(language);

            int recordedCount = 0;

            foreach (var msgId in _pendingMarkAsReadIds)
            {
                // Find the message in context
                var msg = _messages.FirstOrDefault(m => m.Id == msgId);
                if (msg == null) continue;

                // Only record User role messages (skip Assistant and Tool messages)
                if (msg.Role != MessageRole.User) continue;

                // Skip messages from self
                if (msg.SenderId == _being.Id) continue;

                // Skip empty messages
                if (string.IsNullOrWhiteSpace(msg.Content)) continue;

                // Resolve sender name and determine if it's a silicon being
                string senderName = "Unknown";
                List<Guid>? relatedBeings = null;
                
                SiliconBeingManager? beingManager = ServiceLocator.Instance.BeingManager;
                SiliconBeingBase? senderBeing = beingManager?.GetBeing(msg.SenderId);
                
                if (senderBeing != null)
                {
                    // Sender is a silicon being
                    senderName = senderBeing.Name;
                    relatedBeings = new List<Guid> { msg.SenderId };
                }
                else
                {
                    // Sender is a human user
                    senderName = Config.Instance?.Data?.UserNickname ?? "User";
                }

                // Record to memory with localized format
                string memoryContent = loc.FormatMemoryEventSingleChat(senderName, _being.Name, msg.Content);
                _being.Memory.Add(memoryContent, relatedBeings);
                recordedCount++;

                _logger.Debug(_being.Id, "Recorded unread message {0} to memory (sender={1})", msgId, senderName);
            }

            if (recordedCount > 0)
            {
                _logger.Info(_being.Id, "Recorded {0} unread user messages to memory", recordedCount);
            }
        }
        catch (Exception ex)
        {
            _logger.Warn(_being.Id, "Failed to record unread messages to memory: {0}", ex.Message);
        }
    }

    /// <summary>
    /// Builds a memory-context block from the being's compressed memory summaries.
    /// Combines recent day-level (short-term) and month-level (long-term) summaries
    /// so the silicon being can recall past interactions like a human would.
    /// Returns null when memory is unavailable or empty.
    /// </summary>
    private string? BuildKnowledgeContext()
    {
        var knowledgeNetwork = ServiceLocator.Instance.Get<IKnowledgeNetwork>();
        if (knowledgeNetwork == null) return null;

        try
        {
            var stats = knowledgeNetwork.GetStatistics();
            if (stats.TotalTriples == 0) return null;

            StringBuilder sb = new();
            sb.AppendLine("Knowledge network available:");
            sb.AppendLine($"- {stats.TotalTriples} knowledge triples, {stats.TotalSubjects} subjects, {stats.TotalPredicates} relation types");

            // Provide top hub nodes as navigation hints
            var degreeDist = knowledgeNetwork.GetDegreeDistribution();
            if (degreeDist.TopHubNodes.Count > 0)
            {
                sb.AppendLine("- Key concepts:");
                foreach (var hub in degreeDist.TopHubNodes.Take(5))
                {
                    sb.AppendLine($"  * {hub.Entity} (out: {hub.OutDegree}, in: {hub.InDegree})");
                }
            }

            sb.AppendLine();
            sb.AppendLine("You can use the 'knowledge' tool to query, search, traverse, or find paths in the knowledge network.");

            return sb.ToString();
        }
        catch (Exception ex)
        {
            _logger.Warn(_being.Id, "Failed to build knowledge context: {0}", ex.Message);
            return null;
        }
    }

    private string? BuildMemoryContext()
    {
        if (_being.Memory == null) return null;

        List<MemoryEntry> memories;
        try
        {
            memories = _being.Memory.GetMemoryContextForChat();
        }
        catch (Exception ex)
        {
            _logger.Warn(_being.Id, "Failed to load memory context: {0}", ex.Message);
            return null;
        }

        if (memories.Count == 0) return null;

        StringBuilder sb = new();
        sb.AppendLine("You remember...");
        foreach (MemoryEntry memory in memories)
        {
            sb.AppendLine($"- [{memory.Timestamp}] {memory.Content}");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Executes tool calls and returns tool result messages.
    /// Each tool result is added to the context messages.
    /// </summary>
    private List<ChatMessage> ExecuteToolCalls(List<ToolCall> toolCalls)
    {
        _logger.Info(_being.Id, "Executing {0} tool calls for being {1}", toolCalls.Count, _being.Name);

        List<ChatMessage> toolResultMessages = new List<ChatMessage>();
        ToolManager? toolManager = _being.ToolManager;

        foreach (ToolCall toolCall in toolCalls)
        {
            ToolResult result;
            if (toolManager != null)
            {
                result = toolManager.ExecuteTool(toolCall.Name, toolCall.Arguments, being: _being);
            }
            else
            {
                result = ToolResult.Failed($"No tool manager available for tool '{toolCall.Name}'");
            }

            _logger.Info(_being.Id, "Tool call: {0}, success={1}", toolCall.Name, result.Success);

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
    public AIResponse GetResponse(string? scenarioContext = null, TaskItem? task = null, ToolScenarioFlag scenario = ToolScenarioFlag.All)
    {
        _logger.Info(_being.Id, "Getting AI response for being {0}", _being.Name);

        AIRequest request = BuildRequest(scenarioContext, task, scenario);
        AIResponse response = _aiClient.Chat(request);

#if DEBUG
        SaveAIDebugLog(request, response, scenarioContext);
#endif

        if (response.Success && response.HasToolCalls)
        {
            _logger.Debug(_being.Id, "AI returned tool calls, persisting intermediate round");
            PersistAndDeliverToolCallRound(response);
        }
        else if (response.Success)
        {
            _logger.Debug(_being.Id, "AI returned text response, length={0}", response.Content?.Length ?? 0);
            AddAssistantMessage(response.Content, response.Thinking, response.PromptTokens, response.CompletionTokens, response.TotalTokens);
        }

        RecordTokenUsage(response);
        return response;
    }

    /// <summary>
    /// Sends the current context to AI and gets the response asynchronously.
    /// Implements fallback strategy: if connection fails, temporarily uses global config.
    /// </summary>
    /// <param name="scenarioContext">Optional scenario-specific context from caller</param>
    /// <returns>The AI response (may contain tool_calls or plain text)</returns>
    public async Task<AIResponse> GetResponseAsync(string? scenarioContext = null, ToolScenarioFlag scenario = ToolScenarioFlag.All)
    {
        AIRequest request = BuildRequest(scenarioContext, scenario: scenario);
        
        try
        {
            AIResponse response = await _aiClient.ChatAsync(request);
            
#if DEBUG
            SaveAIDebugLog(request, response, scenarioContext);
#endif

            if (response.Success && response.HasToolCalls)
            {
                _logger.Debug(_being.Id, "AI returned tool calls (async), persisting intermediate round");
                PersistAndDeliverToolCallRound(response);
            }
            else if (response.Success)
            {
                _logger.Debug(_being.Id, "AI returned text response (async), length={0}", response.Content?.Length ?? 0);
                AddAssistantMessage(response.Content, response.Thinking, response.PromptTokens, response.CompletionTokens, response.TotalTokens);
            }

            RecordTokenUsage(response);
            return response;
        }
        catch (HttpRequestException ex) when (IsConnectionError(ex))
        {
            // Connection error: try fallback with global config
            _logger.Warn(_being.Id, "Being {0}: AI client connection error, attempting fallback to global config", _being.Name, ex);
            
            return await ExecuteWithFallbackClientAsync(request);
        }
    }

    /// <summary>
    /// Sends the current context to AI using streaming and gets the response asynchronously.
    /// Streams incremental tokens to the IM layer for real-time frontend display.
    /// Falls back to non-streaming if the AI client does not support streaming.
    /// </summary>
    /// <param name="scenarioContext">Optional scenario-specific context from caller</param>
    /// <param name="cancellationToken">Cancellation token to abort the stream</param>
    /// <returns>The final AI response (assembled from all stream chunks)</returns>
    public async Task<AIResponse> GetResponseStreamAsync(string? scenarioContext = null, CancellationToken cancellationToken = default, ToolScenarioFlag scenario = ToolScenarioFlag.All)
    {
        bool? streamingMode = _aiClient.StreamingMode;
        if (streamingMode == false)
        {
            _logger.Warn(_being.Id, "Falling back to non-streaming mode");
            return await GetResponseAsync(scenarioContext, scenario);
        }

        AIRequest request = BuildRequest(scenarioContext, scenario: scenario);
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
            // Send initial streaming event to show stop button on frontend
            StreamChunk initialChunk = StreamChunk.Start(streamId);
            DeliverStreamChunk(initialChunk);

            await foreach (AIResponse chunk in _aiClient.ChatStreamAsync(request, cancellationToken))
            {
                // Check if cancellation was requested during iteration
                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.Debug(_being.Id, "Stream cancelled during iteration, throwing OperationCanceledException");
                    cancellationToken.ThrowIfCancellationRequested();
                }
                
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
            _logger.Warn(_being.Id, "Streaming not implemented, falling back to non-streaming mode");
            return await GetResponseAsync(scenarioContext, scenario);
        }

        // If the token was cancelled (OllamaClient may have swallowed OperationCanceledException
        // via yield break), surface it here to prevent falling through to non-streaming fallback.
        cancellationToken.ThrowIfCancellationRequested();

        if (!streamSucceeded)
        {
            if (streamingMode == true)
            {
                return AIResponse.Failed("AI client requires streaming but stream ended without final chunk");
            }
            _logger.Warn(_being.Id, "Stream ended without final chunk, falling back to non-streaming mode");
            return await GetResponseAsync(scenarioContext, scenario);
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

#if DEBUG
        SaveAIDebugLog(request, response, scenarioContext);
#endif

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
        _logger.Info(_being.Id, "ThinkOnChat: being={0}, session={1}", _being.Name, _session?.Id.ToString() ?? "null");

        string? scenarioContext = BuildSingleChatScenarioContext();
        AIResponse response = GetResponse(scenarioContext, scenario: ToolScenarioFlag.Chat);

        if (response.Success && response.HasToolCalls)
        {
            // Tool calls executed, results persisted → yield time slice
            Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
            LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);
            string toolNames = string.Join(", ", response.ToolCalls!.Select(t => t.Name));
            RecordToMemory(loc.FormatMemoryEventToolCall(toolNames));
            CommitMessagesAsRead();
        }
        else if (response.Success && (!string.IsNullOrEmpty(response.Content) || !string.IsNullOrEmpty(response.Thinking)))
        {
            DeliverOutput(response.Content, response.Thinking, response.PromptTokens, response.CompletionTokens, response.TotalTokens);
            CommitMessagesAsRead();

            if (!string.IsNullOrEmpty(response.Content))
            {
                Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
                LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);
                Guid otherId = _session?.Members.FirstOrDefault(id => id != _being.Id) ?? Guid.Empty;
                SiliconBeingManager? beingManager = ServiceLocator.Instance.BeingManager;
                SiliconBeingBase? otherBeing = otherId != Guid.Empty ? beingManager?.GetBeing(otherId) : null;
                string partnerName = otherBeing?.Name ?? Config.Instance?.Data?.UserNickname ?? otherId.ToString();
                
                // If the other party is a silicon being, associate with memory; if human user, don't associate
                List<Guid>? relatedBeings = otherBeing != null ? new List<Guid> { otherId } : null;
                RecordToMemory(loc.FormatMemoryEventSingleChat(_being.Name, partnerName, response.Content), relatedBeings);
            }
        }
        else if (!response.Success)
        {
            // AI request failed → notify frontend and keep messages unread for retry
            string errorMsg = response.ErrorMessage ?? "Unknown AI error";
            DeliverOutput($"[Error] AI request failed: {errorMsg}");
            _logger.Error(_being.Id, "ThinkOnChat failed: being={0}, error={1}", _being.Name, errorMsg);
        }

        return response;
    }

    /// <summary>
    /// Builds scenario context for single chat.
    /// Includes information about the conversation partner (human or silicon being)
    /// plus conversation guidelines tailored for one-on-one dialog.
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

        sb.AppendLine();
        sb.AppendLine("CONVERSATION GUIDELINES:");
        sb.AppendLine("- This is a private conversation, be warm and personal.");
        sb.AppendLine("- Reference past interactions when relevant (see your memory context).");
        sb.AppendLine("- Ask follow-up questions to keep the conversation engaging.");
        sb.AppendLine("- Keep responses concise unless detailed explanation is requested.");

        return sb.ToString();
    }

    /// <summary>
    /// Builds scenario context for group chat, including session metadata
    /// and behavioral guidelines tuned for multi-participant conversations.
    /// </summary>
    private string? BuildGroupChatScenarioContext()
    {
        if (_session == null)
        {
            return null;
        }

        StringBuilder sb = new();
        sb.AppendLine("Scene: Group chat");
        sb.AppendLine($"Session ID: {_session.Id}");
        sb.AppendLine($"Members: {_session.Members.Count} participants");

        var pendingMessages = _messages.Where(m => m.SenderId != _being.Id && !m.ReadBy.Contains(_being.Id)).ToList();
        bool isDirectlyMentioned = pendingMessages.Any(m => m.MentionedIds != null && m.MentionedIds.Contains(_being.Id));
        bool isAllMentioned = pendingMessages.Any(m => m.MentionedIds != null && m.MentionedIds.Contains(Guid.Empty));
        bool isNotMentioned = !isDirectlyMentioned && !isAllMentioned && pendingMessages.Any(m => m.MentionedIds == null || m.MentionedIds.Count == 0 || !m.MentionedIds.Contains(_being.Id));

        sb.AppendLine();
        sb.AppendLine("MENTION STATUS:");
        if (isDirectlyMentioned)
        {
            sb.AppendLine("- You were DIRECTLY MENTIONED (@name) in this conversation. You MUST respond.");
        }
        else if (isAllMentioned)
        {
            sb.AppendLine("- You were mentioned via @all/@everyone. You MUST respond.");
        }
        else if (isNotMentioned)
        {
            sb.AppendLine("- You were NOT mentioned in this conversation. You may respond only if you have valuable and relevant input. Otherwise, use the 'mark_read' action to acknowledge without replying.");
        }

        sb.AppendLine();
        sb.AppendLine("GROUP CHAT GUIDELINES:");
        sb.AppendLine("- Only respond when addressed or when you have valuable input.");
        sb.AppendLine("- Be concise to avoid overwhelming the group.");
        sb.AppendLine("- Reference specific members by name when responding to them.");
        sb.AppendLine("- Avoid repeating information already shared in the conversation.");
        if (isNotMentioned)
        {
            sb.AppendLine("- Since you were not mentioned, prefer using 'mark_read' unless the message clearly requires your expertise or direct involvement.");
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
        _logger.Info(_being.Id, "ThinkOnChatStream: being={0}, session={1}", _being.Name, _session?.Id.ToString() ?? "null");

        // Resolve the effective cancellation token: if a channelId is available,
        // try to obtain a managed token from StreamCancellationManager so that
        // external stop requests can cancel this stream.
        CancellationToken effectiveToken = cancellationToken;
        Guid channelId = _session?.Id ?? Guid.Empty;
        StreamCancellationManager? cancellationManager = null;
        
        _logger.Info(_being.Id, "ThinkOnChatStreamAsync: channelId={0}, cancellationToken={1}", channelId, cancellationToken == CancellationToken.None ? "None" : "Provided");
        
        if (channelId != Guid.Empty && cancellationToken == CancellationToken.None)
        {
            cancellationManager = ServiceLocator.Instance.GetService<StreamCancellationManager>();
            if (cancellationManager != null)
            {
                effectiveToken = cancellationManager.CreateToken(channelId);
                _logger.Info(_being.Id, "ThinkOnChatStreamAsync: Created cancellation token for channel {0}", channelId);
            }
            else
            {
                _logger.Warn(_being.Id, "ThinkOnChatStreamAsync: StreamCancellationManager not available for channel {0}", channelId);
            }
        }

        string? scenarioContext = BuildSingleChatScenarioContext();
        AIResponse response;
        try
        {
            response = await GetResponseStreamAsync(scenarioContext, effectiveToken, ToolScenarioFlag.Chat);
        }
        catch (OperationCanceledException) when (effectiveToken.IsCancellationRequested)
        {
            _logger.Info(_being.Id, "ThinkOnChatStreamAsync cancelled for channel {0}, stream abandoned", channelId);
            cancellationManager?.CompleteStream(channelId);
            
            // Mark messages as read so next Tick won't retry this message
            // User explicitly cancelled, meaning they don't want a response
            CommitMessagesAsRead();
            
            return AIResponse.Failed("思考已被用户中止");
        }
        finally
        {
            cancellationManager?.CompleteStream(channelId);

            // Notify the IMManager message queue that processing is complete,
            // so it can dispatch the next queued message for this channel.
            IMManager? imManager = ServiceLocator.Instance.IMManager;
            if (imManager != null && channelId != Guid.Empty)
            {
                imManager.NotifyMessageProcessed(channelId);
            }
        }

        if (response.Success && response.HasToolCalls)
        {
            // Tool calls executed, results persisted → yield time slice
            Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
            LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);
            string toolNames = string.Join(", ", response.ToolCalls!.Select(t => t.Name));
            RecordToMemory(loc.FormatMemoryEventToolCall(toolNames));
            CommitMessagesAsRead();
        }
        else if (response.Success && (!string.IsNullOrEmpty(response.Content) || !string.IsNullOrEmpty(response.Thinking)))
        {
            DeliverOutput(response.Content, response.Thinking, response.PromptTokens, response.CompletionTokens, response.TotalTokens);
            CommitMessagesAsRead();

            if (!string.IsNullOrEmpty(response.Content))
            {
                Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
                LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);
                Guid otherId = _session?.Members.FirstOrDefault(id => id != _being.Id) ?? Guid.Empty;
                SiliconBeingManager? beingManager = ServiceLocator.Instance.BeingManager;
                SiliconBeingBase? otherBeing = otherId != Guid.Empty ? beingManager?.GetBeing(otherId) : null;
                string partnerName = otherBeing?.Name ?? Config.Instance?.Data?.UserNickname ?? otherId.ToString();
                
                // If the other party is a silicon being, associate with memory; if human user, don't associate
                List<Guid>? relatedBeings = otherBeing != null ? new List<Guid> { otherId } : null;
                RecordToMemory(loc.FormatMemoryEventSingleChat(_being.Name, partnerName, response.Content), relatedBeings);
            }
        }
        else if (!response.Success)
        {
            // AI request failed → notify frontend and keep messages unread for retry
            string errorMsg = response.ErrorMessage ?? "Unknown AI error";
            DeliverOutput($"[Error] AI request failed: {errorMsg}");
            _logger.Error(_being.Id, "ThinkOnChatStreamAsync failed: being={0}, error={1}", _being.Name, errorMsg);
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
        _logger.Info(_being.Id, "ThinkOnGroupChat: being={0}", _being.Name);

        string? scenarioContext = BuildGroupChatScenarioContext();
        AIResponse response = GetResponse(scenarioContext, scenario: ToolScenarioFlag.Chat);

        if (response.Success && response.HasToolCalls)
        {
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
        _logger.Info(_being.Id, "ThinkOnGroupChatStream: being={0}", _being.Name);

        string? scenarioContext = BuildGroupChatScenarioContext();
        AIResponse response = await GetResponseStreamAsync(scenarioContext, cancellationToken, ToolScenarioFlag.Chat);

        if (response.Success && response.HasToolCalls)
        {
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
        _logger.Info(_being.Id, "ThinkOnTask: being={0}, task={1} ({2}), status={3}", _being.Name, task.Title, task.Id, task.Status);

        if (task.Status == TaskStatus.Pending)
        {
            task.Start();
        }

        ChatHistoryCycle cycle = task.GetCurrentCycle();

        string? scenarioContext = BuildTaskScenarioContext(task);
        AIResponse response = GetResponse(scenarioContext, task, ToolScenarioFlag.Task);

        cycle.Messages.AddRange(_messages.Skip(cycle.Messages.Count).ToList());

        if (response.Success && !string.IsNullOrEmpty(response.Content))
        {
            Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
            LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);
            RecordToMemory(loc.FormatMemoryEventTask(response.Content));
        }

        TaskCenter.Instance.UpdateTask(task);

        return response;
    }

    public static bool NeedsContinuation(SiliconBeingBase being, TaskItem task)
    {
        if (task.Status != TaskStatus.Running)
            return false;

        if (task.ChatHistory.Count == 0)
            return false;

        ChatHistoryCycle lastCycle = task.ChatHistory[^1];
        if (lastCycle.EndStatus != null)
            return false;

        if (lastCycle.Messages.Count == 0)
            return false;

        ChatMessage lastMsg = lastCycle.Messages[^1];
        return lastMsg.Role == MessageRole.Tool || lastMsg.Role == MessageRole.Assistant;
    }

    public AIResponse ThinkOnProject()
    {
        var project = FindProjectNeedingAttention();
        if (project == null)
        {
            return AIResponse.Successful("No project needs attention at this time.");
        }

        _logger.Info(_being.Id, "ThinkOnProject: being={0}, project={1} ({2})", _being.Name, project.Name, project.Id);

        string scenarioContext = BuildProjectScenarioContext(project);
        AIResponse response = GetResponse(scenarioContext, scenario: ToolScenarioFlag.Project);

        if (response.Success && !string.IsNullOrEmpty(response.Content))
        {
            Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
            LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);
            RecordToMemory(loc.FormatMemoryEventTask($"ThinkOnProject: {response.Content}"));
        }

        return response;
    }

    private ProjectSpace? FindProjectNeedingAttention()
    {
        var projectManager = ServiceLocator.Instance.ProjectManager;
        if (projectManager == null) return null;

        var projects = projectManager.ListProjects(includeArchived: false);
        foreach (var project in projects)
        {
            if (project.CreatedBy == _being.Id
                && string.IsNullOrEmpty(project.WorkflowTemplateName)
                && project.Status == ProjectStatus.Active)
            {
                var taskSystem = projectManager.GetTaskSystem(project.Id);
                if (taskSystem == null) return project;

                var tasks = taskSystem.GetAll();
                if (tasks.Count == 0) return project;
                if (tasks.All(t => t.Status == TaskStatus.Completed)) return project;
                if (DateTime.UtcNow - project.UpdatedAt > TimeSpan.FromMinutes(10))
                {
                    var hasStuckTasks = tasks.Any(t =>
                        t.Status == TaskStatus.Running &&
                        t.StartedAt.HasValue &&
                        DateTime.UtcNow - t.StartedAt.Value > TimeSpan.FromMinutes(15));
                    if (hasStuckTasks) return project;
                }
            }
        }
        return null;
    }

    private string BuildProjectScenarioContext(ProjectSpace project)
    {
        StringBuilder sb = new();
        sb.AppendLine("Scene: Project management (ThinkOnProject)");
        sb.AppendLine($"You are the curator of project \"{project.Name}\".");
        sb.AppendLine($"Project ID: {project.Id}");
        sb.AppendLine($"Project goal: {project.Description}");

        var beingManager = ServiceLocator.Instance.BeingManager;
        sb.AppendLine("Team members:");
        foreach (var beingId in project.AssignedBeings)
        {
            var being = beingManager?.GetBeing(beingId);
            sb.AppendLine($"  - {being?.Name ?? beingId.ToString()}");
        }

        var taskSystem = ServiceLocator.Instance.ProjectManager?.GetTaskSystem(project.Id);
        sb.AppendLine("Current task status:");
        if (taskSystem != null)
        {
            var tasks = taskSystem.GetAll();
            if (tasks.Count == 0)
            {
                sb.AppendLine("  (No tasks yet - project needs initialization)");
            }
            else
            {
                foreach (var task in tasks)
                {
                    var executor = beingManager?.GetBeing(task.ExecutorGuid);
                    sb.AppendLine($"  [{task.Status}] #{task.Priority} {task.Title} (executor: {executor?.Name ?? "unassigned"})");
                }
            }
        }
        else
        {
            sb.AppendLine("  (No task system)");
        }

        if (project.GroupChatSessionId.HasValue)
        {
            var chatSystem = ServiceLocator.Instance.ChatSystem;
            var session = chatSystem?.GetSession(project.GroupChatSessionId.Value);
            if (session != null)
            {
                var recentMessages = session.GetMessages(10);
                if (recentMessages.Count > 0)
                {
                    sb.AppendLine("Recent group chat messages:");
                    foreach (var msg in recentMessages)
                    {
                        var sender = beingManager?.GetBeing(msg.SenderId);
                        sb.AppendLine($"  {sender?.Name ?? msg.SenderId.ToString()}: {msg.Content}");
                    }
                }
            }
        }

        sb.AppendLine();
        sb.AppendLine("PROJECT MANAGEMENT GUIDELINES:");
        sb.AppendLine("- As the curator, you decide the next step for this project.");
        sb.AppendLine("- Create tasks to break down the project into actionable items.");
        sb.AppendLine("- Assign tasks to team members using their being IDs.");
        sb.AppendLine("- Use 'chat' action to communicate with the team in the project group chat.");
        sb.AppendLine("- Use 'broadcast' action for project-wide announcements.");
        sb.AppendLine("- Use 'complete' action when all project work is done.");
        sb.AppendLine("- Use 'status' action to review the current project state.");
        sb.AppendLine("- Be decisive and action-oriented. Create specific, well-defined tasks.");

        return sb.ToString();
    }

    /// <summary>
    /// Scene: scheduled timer.
    /// Each Tick executes only one AI call round.
    /// Maintains context continuity via ChatHistoryCycle on TimerItem.
    /// </summary>
    /// <param name="timer">The timer item to process</param>
    /// <returns>The AI response</returns>
    public AIResponse ThinkOnTimer(TimerItem timer)
    {
        _logger.Info(_being.Id, "ThinkOnTimer: being={0}, timer={1} ({2}), state={3}",
            _being.Name, timer.Name, timer.Id, timer.ExecutionState);

        try
        {
            Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
            LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);

            if (timer.ExecutionState == TimerExecutionState.Idle)
            {
                return ExecuteTimerStart(timer, loc);
            }

            if (timer.ExecutionState == TimerExecutionState.Started ||
                timer.ExecutionState == TimerExecutionState.Executing)
            {
                return ExecuteTimerContinue(timer, loc);
            }

            _logger.Warn(_being.Id, "Timer {0} already in terminal state {1}",
                timer.Name, timer.ExecutionState);
            return AIResponse.Failed("Timer already completed");
        }
        catch (Exception ex)
        {
            _logger.Error(_being.Id, "ThinkOnTimer exception: being={0}, timer={1}, error={2}",
                _being.Name, timer.Name, ex.Message);

            Language lang = Config.Instance?.Data?.Language ?? Language.ZhCN;
            LocalizationBase loc = LocalizationManager.Instance.GetLocalization(lang);
            string errorMessage = loc.FormatTimerErrorNotification(timer.Name, ex.Message);
            DeliverTimerOutput(errorMessage);
            RecordToMemory(loc.FormatMemoryEventTimerError(timer.Name, ex.Message));

            timer.ExecutionState = TimerExecutionState.Failed;
            timer.SealCurrentCycle(TimerExecutionState.Failed);
            _being.TimerSystem?.Save();

            return AIResponse.Failed(ex.Message);
        }
    }

    private AIResponse ExecuteTimerStart(TimerItem timer, LocalizationBase loc)
    {
        _logger.Info(_being.Id, "Timer start: {0}", timer.Name);

        string startMessage = loc.FormatTimerStartNotification(timer.Name);
        DeliverTimerOutput(startMessage);

        timer.ExecutionState = TimerExecutionState.Started;
        timer.CurrentRound = 0;

        string scenarioContext = BuildTimerScenarioContext(timer);

        _messages.Add(new ChatMessage
        {
            Role = MessageRole.User,
            Content = $"Timer '{timer.Name}' has been triggered. Please execute the scheduled task.",
        });

        ChatHistoryCycle cycle = timer.GetCurrentCycle();
        cycle.Messages.AddRange(_messages);

        AIResponse response = GetResponse(scenarioContext, scenario: ToolScenarioFlag.Timer);
        timer.CurrentRound++;

        if (!response.Success)
        {
            timer.ExecutionState = TimerExecutionState.Failed;
            timer.SealCurrentCycle(TimerExecutionState.Failed);
            string errorMsg = loc.FormatTimerErrorNotification(timer.Name, response.ErrorMessage ?? "AI call failed");
            DeliverTimerOutput(errorMsg);
            RecordToMemory(loc.FormatMemoryEventTimerError(timer.Name, response.ErrorMessage ?? "AI call failed"));
        }
        else if (response.HasToolCalls)
        {
            timer.ExecutionState = TimerExecutionState.Executing;
            string toolNames = string.Join(", ", response.ToolCalls!.Select(t => t.Name));
            RecordToMemory(loc.FormatMemoryEventToolCall(toolNames));
        }
        else if (!string.IsNullOrEmpty(response.Content))
        {
            timer.ExecutionState = TimerExecutionState.Completed;
            timer.SealCurrentCycle(TimerExecutionState.Completed);
            string endMessage = loc.FormatTimerEndNotification(timer.Name, response.Content);
            DeliverTimerOutput(endMessage);
            RecordToMemory(loc.FormatMemoryEventTimer(response.Content));
        }

        _being.TimerSystem?.Save();
        return response;
    }

    private AIResponse ExecuteTimerContinue(TimerItem timer, LocalizationBase loc)
    {
        timer.CurrentRound++;

        if (timer.CurrentRound > timer.MaxRounds)
        {
            _logger.Warn(_being.Id, "Timer {0}: Max rounds ({1}) reached", timer.Name, timer.MaxRounds);
            timer.ExecutionState = TimerExecutionState.Completed;
            timer.SealCurrentCycle(TimerExecutionState.Failed);
            string endMessage = loc.FormatTimerEndNotification(timer.Name, "Timer executed (max rounds reached)");
            DeliverTimerOutput(endMessage);
            _being.TimerSystem?.Save();
            return AIResponse.Failed("Max rounds reached");
        }

        _logger.Info(_being.Id, "Timer continue round {0}: {1}", timer.CurrentRound, timer.Name);

        string scenarioContext = BuildTimerScenarioContext(timer);
        AIResponse response = GetResponse(scenarioContext, scenario: ToolScenarioFlag.Timer);

        if (!response.Success)
        {
            timer.ExecutionState = TimerExecutionState.Failed;
            timer.SealCurrentCycle(TimerExecutionState.Failed);
            string errorMsg = loc.FormatTimerErrorNotification(timer.Name, response.ErrorMessage ?? "AI call failed");
            DeliverTimerOutput(errorMsg);
            RecordToMemory(loc.FormatMemoryEventTimerError(timer.Name, response.ErrorMessage ?? "AI call failed"));
        }
        else if (!response.HasToolCalls && !string.IsNullOrEmpty(response.Content))
        {
            timer.ExecutionState = TimerExecutionState.Completed;
            timer.SealCurrentCycle(TimerExecutionState.Completed);
            string endMessage = loc.FormatTimerEndNotification(timer.Name, response.Content);
            DeliverTimerOutput(endMessage);
            RecordToMemory(loc.FormatMemoryEventTimer(response.Content));
        }
        else if (response.HasToolCalls)
        {
            _logger.Info(_being.Id, "Timer {0}: Tool calls returned, will continue on next tick", timer.Name);
            string toolNames = string.Join(", ", response.ToolCalls!.Select(t => t.Name));
            RecordToMemory(loc.FormatMemoryEventToolCall(toolNames));
        }

        _being.TimerSystem?.Save();
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

        sb.AppendLine();
        sb.AppendLine("TASK EXECUTION GUIDELINES:");
        sb.AppendLine("- Analyze the task requirements carefully before acting.");
        sb.AppendLine("- Use available tools to complete the task step by step.");
        sb.AppendLine("- Report progress and any obstacles you encounter.");
        sb.AppendLine("- When the task is complete, provide a clear summary of what was done.");
        sb.AppendLine("- If the task cannot be completed, explain why and suggest alternatives.");

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

        // Add execution guidance for efficient AI behavior
        sb.AppendLine();
        sb.AppendLine("IMPORTANT INSTRUCTIONS:");
        sb.AppendLine("- This is an automated timer execution. You must complete ALL required operations efficiently.");
        sb.AppendLine("- You can call multiple tools in sequence to accomplish the timer's purpose.");
        sb.AppendLine("- After completing all operations, return a FINAL SUMMARY as plain text (no tool calls) describing what was done.");
        sb.AppendLine("- The final summary will be delivered to the user as the timer execution result.");
        sb.AppendLine("- Be concise and focused. Avoid unnecessary operations or lengthy explanations.");
        sb.AppendLine("- If you encounter errors, report them clearly in your final response.");

        sb.AppendLine();
        sb.AppendLine("ERROR HANDLING:");
        sb.AppendLine("- If a tool call fails due to permission denied, skip it and continue with other operations.");
        sb.AppendLine("- If a tool call times out, report the error and move on to the next operation.");
        sb.AppendLine("- Do not retry failed operations more than once.");
        sb.AppendLine("- Always provide a final summary even if some operations failed.");

        sb.AppendLine();
        sb.AppendLine("TIME AWARENESS:");
        sb.AppendLine("- This is an automated background task; keep execution time minimal.");
        sb.AppendLine("- Prioritize critical operations over optional ones.");
        sb.AppendLine("- If the task seems too complex, focus on the most important aspects.");

        return sb.ToString();
    }

    /// <summary>
    /// Scene: memory compression.
    /// Compresses memories using hierarchical aggregation: minute summaries -> hour -> day -> month -> year.
    /// Uses Generate API (non-chat) for pure text compression.
    /// </summary>
    /// <param name="compressData">The compression level and entries to compress. If null, will be determined automatically.</param>
    /// <returns>The AI response</returns>
    public AIResponse ThinkOnMemoryCompress((IncompleteDate Level, List<TimeEntry<MemoryEntry>> Entries)? compressData = null)
    {
        _logger.Info(_being.Id, "ThinkOnMemoryCompress: being={0}", _being.Name);

        if (_being.Memory == null)
        {
            return new AIResponse { Success = false, ErrorMessage = "Memory not initialized" };
        }

        // Use provided compressData or find level to compress automatically
        var compressDataValue = compressData ?? _being.Memory.FindLevelToCompress();
        if (!compressDataValue.HasValue)
        {
            return new AIResponse { Success = true, Content = "No level to compress" };
        }

        var (level, entries) = compressDataValue.Value;

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
        _logger.Debug(_being.Id, "Delivering output for being {0}, length={1}", _being.Name, content.Length);

        IMManager? imManager = ServiceLocator.Instance.IMManager;
        if (imManager != null && _session != null)
        {
            _ = imManager.SendMessageAsync(_being.Id, _session.Id, content, thinking, _being.Name, promptTokens, completionTokens, totalTokens);
        }
        else
        {
            _logger.Info(_being.Id, "{0}: {1}", _being.Name, content);
        }
        _logger.Info(_being.Id, "");
    }

    /// <summary>
    /// Delivers output for timer execution (no session required).
    /// Resolves the correct session for routing and persists the message to ChatSystem.
    /// Curator sends directly to the user; non-curator sends to the curator for relay.
    /// </summary>
    private void DeliverTimerOutput(string content, string? thinking = null)
    {
        _logger.Debug(_being.Id, "Delivering timer output for being {0}, length={1}", _being.Name, content.Length);

        // Determine the target: curator → user, non-curator → curator
        Guid targetId = Guid.Empty;
        if (_being.IsCurator)
        {
            targetId = Config.Instance?.Data?.UserGuid ?? Guid.Empty;
        }
        else
        {
            Guid curatorGuid = Config.Instance?.Data?.CuratorGuid ?? Guid.Empty;
            SiliconBeingManager? beingManager = ServiceLocator.Instance.BeingManager;
            SiliconBeingBase? curator = curatorGuid != Guid.Empty ? beingManager?.GetBeing(curatorGuid) : null;
            targetId = curator?.Id ?? Guid.Empty;
        }

        if (targetId == Guid.Empty)
        {
            _logger.Warn(_being.Id, "DeliverTimerOutput: no valid target for being {0}", _being.Name);
            return;
        }

        // Get or create session to obtain the correct session ID for routing
        ChatSystem? chatSystem = ServiceLocator.Instance.ChatSystem;
        SessionBase? session = chatSystem?.GetOrCreateSession(_being.Id, targetId);
        if (session == null)
        {
            _logger.Warn(_being.Id, "DeliverTimerOutput: failed to get session for being {0} → target {1}", _being.Name, targetId);
            return;
        }

        // Persist message to ChatSystem
        ChatMessage chatMsg = new(_being.Id, session.Id, content)
        {
            Role = MessageRole.Assistant,
            Thinking = thinking,
        };
        chatSystem!.AddMessage(chatMsg);

        // Push via IM (SSE) using the session ID so SSE clients can receive it
        IMManager? imManager = ServiceLocator.Instance.IMManager;
        if (imManager != null)
        {
            _ = imManager.SendMessageAsync(_being.Id, session.Id, content, thinking, _being.Name);
        }

        _logger.Info(_being.Id, "[TIMER] {0}: {1}", _being.Name, content);
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

        if (_timerForContext != null)
        {
            _timerForContext.GetCurrentCycle().Messages.Add(assistantMsg);
            _timerForContext.GetCurrentCycle().Messages.AddRange(toolResultMessages);
            _being.TimerSystem?.Save();
        }
        else if (_taskForContext != null)
        {
            _taskForContext.GetCurrentCycle().Messages.Add(assistantMsg);
            _taskForContext.GetCurrentCycle().Messages.AddRange(toolResultMessages);
            TaskCenter.Instance.UpdateTask(_taskForContext);
        }
        else if (_session != null)
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
        _messages.AddRange(toolResultMessages);
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
    /// <param name="content">The memory content</param>
    /// <param name="relatedBeings">List of related being IDs (e.g., conversation partner)</param>
    private void RecordToMemory(string content, List<Guid>? relatedBeings = null)
    {
        if (_being.Memory == null || string.IsNullOrEmpty(content))
        {
            return;
        }

        try
        {
            Language language = Config.Instance?.Data?.Language ?? Language.ZhCN;
            LocalizationBase localization = LocalizationManager.Instance.GetLocalization(language);
            _being.Memory.Add(content, relatedBeings);
        }
        catch (Exception ex)
        {
            _logger.Warn(_being.Id, "Failed to record to memory: {0}", ex.Message);
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
    
    /// <summary>
    /// Executes AI request with fallback client when connection fails.
    /// Sets the IsUsingFallbackClient flag so that DefaultSiliconBeing can handle it on next tick.
    /// </summary>
    private async Task<AIResponse> ExecuteWithFallbackClientAsync(AIRequest request)
    {
        // Mark being as using fallback client
        // The actual fallback client creation will be handled by DefaultSiliconBeing on next tick
        _being.IsUsingFallbackClient = true;
        
        _logger.Error(_being.Id, "Being {0}: AI client connection failed. Will attempt recovery on next tick.", _being.Name);
        
        // Return error response - the being will handle recovery on next tick
        return AIResponse.Failed("AI client connection failed, will retry on next tick");
    }
    
    /// <summary>
    /// Determines if an exception is a connection error (vs timeout or other errors)
    /// </summary>
    private bool IsConnectionError(HttpRequestException ex)
    {
        // Connection refused, name resolution failure, etc.
        return ex.InnerException is System.Net.Sockets.SocketException 
               || ex.Message.Contains("Connection refused", StringComparison.OrdinalIgnoreCase)
               || ex.Message.Contains("Name or service not known", StringComparison.OrdinalIgnoreCase)
               || ex.Message.Contains("No connection could be made", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Saves AI request and response to a debug log file for analysis.
    /// Only active in DEBUG builds.
    /// </summary>
    /// <param name="request">The AI request that was sent</param>
    /// <param name="response">The AI response received</param>
    /// <param name="scenario">Optional scenario description</param>
#if DEBUG
    private void SaveAIDebugLog(AIRequest request, AIResponse response, string? scenario = null)
    {
        try
        {
            string debugDir = Path.Combine(AppContext.BaseDirectory, AIDebugLogDirectory);
            if (!Directory.Exists(debugDir))
            {
                Directory.CreateDirectory(debugDir);
            }

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            string fileName = $"AI_Debug_{_being.Name}_{timestamp}.json";
            string filePath = Path.Combine(debugDir, fileName);

            var debugData = new
            {
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                BeingId = _being.Id,
                BeingName = _being.Name,
                Scenario = scenario ?? "Unknown",
                Request = new
                {
                    request.Model,
                    Messages = request.Messages.Select(m => new
                    {
                        m.Role,
                        m.Content,
                        m.Thinking,
                        m.ToolCallId
                    }).ToList(),
                    Tools = request.Tools?.Select(t => new { t.Name, t.Description }).ToList()
                },
                Response = new
                {
                    response.Success,
                    response.Content,
                    response.Thinking,
                    response.Model,
                    response.PromptTokens,
                    response.CompletionTokens,
                    response.TotalTokens,
                    response.ErrorMessage,
                    HasToolCalls = response.HasToolCalls,
                    ToolCalls = response.ToolCalls?.Select(t => new { t.Name, t.Arguments }).ToList()
                }
            };

            string json = JsonSerializer.Serialize(debugData, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(filePath, json);
            _logger.Debug(_being.Id, "AI debug log saved to {0}", filePath);
        }
        catch (Exception ex)
        {
            _logger.Warn(_being.Id, "Failed to save AI debug log: {0}", ex.Message);
        }
    }
#endif
}
