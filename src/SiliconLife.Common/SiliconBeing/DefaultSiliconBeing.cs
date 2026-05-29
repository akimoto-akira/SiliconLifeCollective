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
using SiliconLife.Common.AI;
using SiliconLife.Common.Localization;
using System.Text.Json;

namespace SiliconLife.Common.SiliconBeing;

/// <summary>
/// Default implementation of a silicon being.
/// The being is the "body" — it stays alive, detects trigger scenes,
/// and calls the corresponding method on the brain (ContextManager).
/// </summary>
public class DefaultSiliconBeing : SiliconBeingBase
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<DefaultSiliconBeing>();
    private volatile bool _isProcessing;
    private volatile int _activityRaw = (int)BeingActivity.Idle;
    private int _consecutiveErrorCount;
    private const int ConsecutiveErrorStopCount = 10;
    
    /// <summary>
    /// Gets the current activity of this silicon being.
    /// Updated by <see cref="Tick"/> when entering a brain scene, and kept
    /// as-is across ticks. Reset to <see cref="BeingActivity.Idle"/> only when
    /// a full tick elapses without triggering any scene.
    /// </summary>
    public override BeingActivity CurrentActivity => (BeingActivity)_activityRaw;

    /// <summary>
    /// Resets the consecutive error count, allowing the being to resume processing.
    /// Call this when an external disturbance (e.g. user message, AI config change)
    /// should give the being another chance to recover from a stopped state.
    /// </summary>
    public void ResetConsecutiveErrorCount() => _consecutiveErrorCount = 0;

    /// <summary>
    /// Initializes a new instance of the DefaultSiliconBeing class
    /// </summary>
    /// <param name="id">The unique identifier</param>
    /// <param name="name">The name of the silicon being</param>
    public DefaultSiliconBeing(Guid id, string name)
        : base(id, name)
    {
        _isProcessing = false;
        _activityRaw = (int)BeingActivity.Idle;
    }

    public override void SaveState()
    {
        base.SaveState();
    }

    public override bool LoadState()
    {
        if (!base.LoadState()) return false;

        Language language = Config.Instance?.Data?.Language ?? Language.ZhCN;
        if (LocalizationManager.Instance.TryGetLocalization(language, out LocalizationBase? loc) &&
            loc is DefaultLocalizationBase defaultLoc)
        {
            Memory?.Add(defaultLoc.FormatMemoryEventStartup());
        }

        return true;
    }
    
    /// <summary>
    /// Deserializes a JsonElement to Dictionary<string, object>
    /// </summary>
    private Dictionary<string, object> DeserializeDictionary(JsonElement element)
    {
        var dict = new Dictionary<string, object>();
        if (element.ValueKind == System.Text.Json.JsonValueKind.Object)
        {
            foreach (var property in element.EnumerateObject())
            {
                dict[property.Name] = property.Value.ValueKind switch
                {
                    System.Text.Json.JsonValueKind.String => property.Value.GetString() ?? "",
                    System.Text.Json.JsonValueKind.Number => property.Value.GetDouble(),
                    System.Text.Json.JsonValueKind.True => true,
                    System.Text.Json.JsonValueKind.False => false,
                    _ => property.Value.GetRawText()
                };
            }
        }
        return dict;
    }

    /// <summary>
    /// Called by SiliconBeingManager on each tick.
    /// Detects the trigger scene and calls the corresponding brain method.
    /// Priority: AI config change > continuation > chat > timer > task > project > memory compression
    /// </summary>
    /// <param name="deltaTime">Time elapsed since the last tick</param>
    public override void Tick(TimeSpan deltaTime)
    {
        // 1. Check if AI config has changed (external disturbance - resets error count)
        if (CheckAndRebuildAIClient())
        {
            _consecutiveErrorCount = 0;
            return;
        }

        // 2. If consecutive errors reached threshold, check for external triggers
        if (_consecutiveErrorCount >= ConsecutiveErrorStopCount)
        {
            if (HasPendingChatWork())
            {
                _consecutiveErrorCount = 0;
                List<SessionBase> allSession = BuildSessionList();
                foreach(SessionBase sb in allSession)
                {
                    ContextManager cm = new ContextManager(this, sb);
                    cm.CommitMessagesAsRead();
                }
                _activityRaw = (int)BeingActivity.Idle;
                return;
            }
            else
            {
                _activityRaw = (int)BeingActivity.Stopped;
                return;
            }
        }

        // 3. Original tick logic
        if (_isProcessing || AIClient == null)
        {
            return;
        }

        _isProcessing = true;
        bool errorOccurred = false;
        try
        {
            var sessions = BuildSessionList();

            foreach (var session in sessions)
            {
                if (ContextManager.NeedsContinuation(this, session))
                {
                    if (session.Type == SessionType.GroupChat)
                    {
                        _activityRaw = (int)BeingActivity.GroupChat;
                        _logger.Info(Id, "Being {0}: detected group chat continuation in session {1}", Name, session.Id);
                        if (!ExecuteBrain("ThinkContinuation", session, brain => brain.ThinkOnGroupChat()))
                            errorOccurred = true;
                        return;
                    }
                    else
                    {
                        _activityRaw = (int)BeingActivity.SingleChat;
                        _logger.Info(Id, "Being {0}: detected continuation in session {1}", Name, session.Id);
                        if (!ExecuteBrain("ThinkContinuation", session, brain => brain.ThinkOnChat()))
                            errorOccurred = true;
                        return;
                    }
                }
            }

            foreach (var session in sessions)
            {
                ContextManager brain = new ContextManager(this, session);
                if (brain.HasWork)
                {
                    // Check if the last AI response was a mark_read action
                    // If so, skip thinking to save tokens (read but no reply).
                    // mark_read is a lightweight bookkeeping step: keep the
                    // previous activity value intact rather than counting it
                    // as a fresh scene trigger.
                    if (WasJustMarkRead(session))
                    {
                        brain.CommitMessagesAsRead();
                        _logger.Info(Id, "Being {0}: skipped reply after mark_read in session {1}", Name, session.Id);
                        return;
                    }

                    if (session.Type == SessionType.GroupChat)
                    {
                        var mentionStatus = ShouldReplyInGroupChat(session, brain);
                        if (mentionStatus == GroupChatMentionStatus.NotMentionedSkip)
                        {
                            brain.CommitMessagesAsRead();
                            _logger.Info(Id, "Being {0}: skipping group chat session {1} - not mentioned and no direct question", Name, session.Id);
                            continue;
                        }

                        _activityRaw = (int)BeingActivity.GroupChat;
                        _logger.Info(Id, "Being {0}: detected pending messages in group chat session {1} (mention={2})", Name, session.Id, mentionStatus);
                        if (!ExecuteBrain("ThinkOnGroupChat", session, _ => brain.ThinkOnGroupChat()))
                            errorOccurred = true;
                        return;
                    }
                    else
                    {
                        _activityRaw = (int)BeingActivity.SingleChat;
                        _logger.Info(Id, "Being {0}: detected pending messages in session {1}", Name, session.Id);
                        if (!ExecuteBrain("ThinkOnChat", session, _ => brain.ThinkOnChat()))
                            errorOccurred = true;
                        return;
                    }
                }
            }

            // Broadcast processing: check for pending broadcast messages
            ChatSystem? chatSystem = ServiceLocator.Instance.ChatSystem;
            if (chatSystem != null)
            {
                List<ChatMessage> pendingBroadcasts = chatSystem.GetPendingBroadcasts(Id);
                if (pendingBroadcasts.Count > 0)
                {
                    _activityRaw = (int)BeingActivity.Broadcast;
                    _logger.Info(Id, "Being {0}: {1} pending broadcast message(s) detected", Name, pendingBroadcasts.Count);
                    foreach (var broadcast in pendingBroadcasts)
                    {
                        Memory?.Add($"Broadcast received: {broadcast.Content}");
                        chatSystem.MarkBroadcastAsRead(broadcast.Id, Id);
                    }
                    return;
                }
            }

            if (TimerSystem != null && HasTimerWork())
            {
                List<TimerItem> timersToProcess = GetTimersToProcess();

                if (timersToProcess.Count > 0)
                {
                    TimerItem timer = timersToProcess[0];
                    _activityRaw = (int)BeingActivity.Timer;
                    _logger.Info(Id, "Being {0}: processing timer {1} (state={2}, round={3})",
                        Name, timer.Name, timer.ExecutionState, timer.CurrentRound);

                    if (!ExecuteBrain("ThinkOnTimer", null, _ => new ContextManager(this, timer).ThinkOnTimer(timer)))
                        errorOccurred = true;
                    return;
                }
            }

            if (IsCurator)
            {
                var executingSession = GetExecutingProjectThinkSession();
                if (executingSession != null)
                {
                    _activityRaw = (int)BeingActivity.Project;
                    _logger.Info(Id, "Being {0}: continuing project think session {1} (round={2})",
                        Name, executingSession.Id, executingSession.CurrentRound);
                    if (!ExecuteBrain("ThinkOnProjectContinue", null,
                        _ => new ContextManager(this, executingSession).ThinkOnProjectContinue(executingSession)))
                        errorOccurred = true;
                    return;
                }
            }

            List<TaskItem> continuationTasks = TaskCenter.Instance.GetContinuationTasks(Id);
            if (continuationTasks.Count > 0)
            {
                TaskItem task = continuationTasks[0];
                _activityRaw = (int)BeingActivity.Task;
                _logger.Info(Id, "Being {0}: continuing task - {1} ({2})", Name, task.Title, task.Id);
                if (!ExecuteBrain("ThinkOnTask", null, _ => new ContextManager(this, task).ThinkOnTask(task)))
                    errorOccurred = true;
                return;
            }

            if (TaskEnumerator != null && TaskEnumerator.HasRunnableTasks())
            {
                List<TaskItem> runnable = TaskEnumerator.EnumerateRunnable().ToList();
                if (runnable.Count > 0)
                {
                    TaskItem task = runnable[0];
                    _activityRaw = (int)BeingActivity.Task;
                    _logger.Info(Id, "Being {0}: pending task detected - {1} ({2})", Name, task.Title, task.Id);
                    if (!ExecuteBrain("ThinkOnTask", null, _ => new ContextManager(this, task).ThinkOnTask(task)))
                        errorOccurred = true;
                    return;
                }
            }

            if (IsCurator)
            {
                if (HasProjectsNeedingAttention())
                {
                    _activityRaw = (int)BeingActivity.Project;
                    _logger.Info(Id, "Being {0}: checking projects needing attention", Name);
                    if (!ExecuteBrain("ThinkOnProject", null, brain => brain.ThinkOnProject()))
                        errorOccurred = true;
                    return;
                }
            }

            if (Memory != null && Memory.ShouldCompress(out var compressData))
            {
                _activityRaw = (int)BeingActivity.MemoryCompression;
                _logger.Debug(Id, "Being {0}: memory compression needed at level {1}", Name, compressData.Value.Level);
                if (!ExecuteBrain("ThinkOnMemoryCompress", null, _ => new ContextManager(this, (SessionBase?)null).ThinkOnMemoryCompress(compressData)))
                    errorOccurred = true;
                return;
            }

            // Reached the end of Tick without triggering any scene branch
            // (all the early-return branches above were skipped). Only in
            // this "fully idle tick" case do we fall back to Idle, so that
            // the previously displayed activity stays visible until the
            // being actually has nothing to do.
            _activityRaw = (int)BeingActivity.Idle;
        }
        catch (Exception ex)
        {
            errorOccurred = true;
            _logger.Error(Id, "Being {0}: unexpected error during tick (consecutive={1})", Name, ex, _consecutiveErrorCount + 1);

            Language language = Config.Instance.Data.Language;
            DefaultLocalizationBase localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(language);

            Memory?.Add(localization.FormatMemoryEventRuntimeError(ex.Message));
            _logger.Info(Id, "{0}: {1} {2}", Name, localization.UnexpectedErrorMessage, ex.Message);
        }
        finally
        {
            if (errorOccurred)
                _consecutiveErrorCount++;
            else
                _consecutiveErrorCount = 0;
            _isProcessing = false;
        }
    }

    /// <summary>
    /// Checks if there is pending chat work (continuation or unread messages).
    /// Used to detect external disturbances (user messages) that should reset the error count.
    /// </summary>
    private bool HasPendingChatWork()
    {
        var sessions = BuildSessionList();
        foreach (var session in sessions)
        {
            if (ContextManager.NeedsContinuation(this, session))
                return true;
            if (new ContextManager(this, session).HasWork)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Builds the list of chat sessions:
    /// single chat sessions with the project user ID + all other silicon beings (excluding self) + group chat sessions.
    /// </summary>
    private List<SessionBase> BuildSessionList()
    {
        var sessions = new List<SessionBase>();
        ChatSystem? chatSystem = ServiceLocator.Instance.ChatSystem;
        if (chatSystem == null)
        {
            return sessions;
        }

        Guid userId = Config.Instance.Data.UserGuid;
        if (userId != Guid.Empty)
        {
            SessionBase session = chatSystem.GetOrCreateSession(Id, userId);
            sessions.Add(session);
        }

        SiliconBeingManager? beingManager = ServiceLocator.Instance.BeingManager;
        if (beingManager != null)
        {
            foreach (var other in beingManager.GetAllBeings())
            {
                if (other.Id != Id)
                {
                    SessionBase session = chatSystem.GetOrCreateSession(Id, other.Id);
                    sessions.Add(session);
                }
            }
        }

        // Add group chat sessions where this being is a member
        var groupSessions = chatSystem.GetGroupChatSessionsForMember(Id);
        foreach (var groupSession in groupSessions)
        {
            sessions.Add(groupSession);
        }

        return sessions;
    }

    /// <summary>
    /// Executes a brain function with logging and continuation tracking.
    /// Returns true if the brain scene executed successfully, false otherwise.
    /// </summary>
    private bool ExecuteBrain(string sceneName, SessionBase? session, Func<ContextManager, AIResponse> thinkFunc)
    {
        _logger.Info(Id, "Being {0}: executing brain scene {1}", Name, sceneName);

        Language language = Config.Instance.Data.Language;
        DefaultLocalizationBase localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(language);

        _logger.Info(Id, "{0}: {1}", Name, localization.ThinkingMessage);

        ContextManager brain = new ContextManager(this, session);

        AIResponse response;
        if (sceneName == "ThinkOnChat" || sceneName == "ThinkContinuation" || sceneName == "ThinkOnGroupChat")
        {
            if (sceneName == "ThinkOnGroupChat")
            {
                response = brain.ThinkOnGroupChatStreamAsync().GetAwaiter().GetResult();
            }
            else
            {
                response = brain.ThinkOnChatStreamAsync().GetAwaiter().GetResult();
            }
        }
        else
        {
            response = thinkFunc(brain);
        }

        if (response.Success && response.HasToolCalls)
        {
            _logger.Info(Id, "{0}: {1}", Name, localization.ToolCallMessage);
        }
        else if (!response.Success)
        {
            _logger.Error(Id, "Being {0}: brain scene {1} failed: {2}", Name, sceneName, response.ErrorMessage ?? "unknown");
            _logger.Info(Id, "{0}: {1} {2}", Name, localization.ErrorMessage, response.ErrorMessage);
            return false;
        }
        else
        {
            _logger.Debug(Id, "Being {0}: brain scene {1} completed", Name, sceneName);
        }

        return true;
    }
    
    /// <summary>
    /// Checks if AI config has changed and rebuilds the client if necessary.
    /// Also initializes the client if it's null.
    /// Returns true if client was initialized or rebuilt.
    /// </summary>
    private bool CheckAndRebuildAIClient()
    {
        // If client is null, always initialize it
        if (AIClient == null)
        {
            RebuildAIClientFromConfig();
            UpdateConfigBackups();
            return AIClient != null; // Return true if initialization succeeded
        }
        
        // If using fallback client, restore original config client
        if (this.IsUsingFallbackClient)
        {
            this.IsUsingFallbackClient = false;
            RebuildAIClientFromConfig();
            UpdateConfigBackups();
            return true;
        }
        
        // Deep compare current config with backup config
        if (!IsAIClientConfigChanged())
        {
            return false;
        }
        
        // Config changed, rebuild client
        RebuildAIClientFromConfig();
        UpdateConfigBackups();
        
        return true;
    }
    
    /// <summary>
    /// Updates all config backups to current values for future change detection
    /// </summary>
    private void UpdateConfigBackups()
    {
        BackupAIClientConfig = AIClientConfig?.ToDictionary(k => k.Key, v => v.Value);
        BackupEffectiveAIClientType = ResolveEffectiveAIClientType(AIClientType);
        var globalConfig = Config.Instance?.Data?.AIConfig;
        BackupGlobalAIConfig = globalConfig?.ToDictionary(k => k.Key, v => v.Value);
    }
    
    /// <summary>
    /// Deep compares AI config to detect changes
    /// </summary>
    private bool IsAIClientConfigChanged()
    {
        // Check if the effective AI client type has changed
        string currentEffectiveType = ResolveEffectiveAIClientType(AIClientType);
        if (currentEffectiveType != BackupEffectiveAIClientType)
            return true;
        
        // Check being-level config changes
        if (AIClientConfig != null && AIClientConfig.Count > 0)
        {
            // Being has independent config, compare with backup
            return !AreDictionariesEqual(AIClientConfig, BackupAIClientConfig);
        }
        
        // Being uses global config, check if global config has changed
        var globalConfig = Config.Instance?.Data?.AIConfig;
        return !AreDictionariesEqual(globalConfig, BackupGlobalAIConfig);
    }
    
    /// <summary>
    /// Deep compares two dictionaries for equality
    /// </summary>
    private static bool AreDictionariesEqual(Dictionary<string, object>? a, Dictionary<string, object>? b)
    {
        if (a == null && b == null) return true;
        if (a == null || b == null) return false;
        if (a.Count != b.Count) return false;
        
        foreach (var kvp in a)
        {
            if (!b.TryGetValue(kvp.Key, out var bValue))
                return false;
            if (!object.Equals(kvp.Value, bValue))
                return false;
        }
        
        return true;
    }
    
    /// <summary>
    /// Rebuilds AI client from current configuration
    /// </summary>
    private void RebuildAIClientFromConfig()
    {
        try
        {
            IAIClientFactory factory = GetAIClientFactory();
            
            IAIClient newClient;
            if (AIClientConfig != null && AIClientConfig.Count > 0)
            {
                // Has independent config, create dedicated client
                newClient = factory.CreateClient(AIClientConfig);
                _logger.Info(Id, "Being {0}: rebuilding AI client with independent config", Name);
            }
            else
            {
                // No independent config, use global config to create client
                var globalConfig = Config.Instance?.Data?.AIConfig;
                if (globalConfig != null && globalConfig.Count > 0)
                {
                    newClient = factory.CreateClient(globalConfig);
                    _logger.Info(Id, "Being {0}: rebuilding AI client with global config", Name);
                }
                else
                {
                    _logger.Error(Id, "Being {0}: no AI config available", Name);
                    return;
                }
            }
            
            // Dispose old client
            if (AIClient != null && AIClient is IDisposable disposable)
            {
                disposable.Dispose();
            }
            
            AIClient = newClient;
            _logger.Info(Id, "Being {0}: AI client rebuilt successfully", Name);
        }
        catch (Exception ex)
        {
            _logger.Error(Id, "Being {0}: failed to rebuild AI client", Name, ex);
        }
    }
    
    /// <summary>
    /// Gets the AI client factory based on AIClientType
    /// </summary>
    private IAIClientFactory GetAIClientFactory()
    {
        string clientType = NormalizeClientType(
            ResolveEffectiveAIClientType(AIClientType));
        
        return clientType switch
        {
            "OllamaClient" => new OllamaClientFactory(),
            "DashScopeClient" => new DashScopeClientFactory(),
            "VolcengineArkClient" => new VolcengineArkClientFactory(),
            _ => new OllamaClientFactory()
        };
    }
    
    /// <summary>
    /// Resolves the effective AI client type, falling through empty strings and nulls.
    /// Priority: being's own type → global config type → default "OllamaClient".
    /// </summary>
    private static string ResolveEffectiveAIClientType(string? beingType = null)
    {
        if (!string.IsNullOrEmpty(beingType))
            return beingType;
        var globalType = Config.Instance?.Data?.AIClientType;
        if (!string.IsNullOrEmpty(globalType))
            return globalType;
        return "OllamaClient";
    }
    
    /// <summary>
    /// Normalizes client type string by stripping "Factory" suffix if present.
    /// Config may store "DashScopeClientFactory" but factory switch expects "DashScopeClient".
    /// </summary>
    private static string NormalizeClientType(string clientType)
    {
        if (clientType.EndsWith("Factory"))
            return clientType.Substring(0, clientType.Length - 7);
        return clientType;
    }

    /// <summary>
    /// Determines whether a silicon being should reply in a group chat based on mention status.
    /// </summary>
    private enum GroupChatMentionStatus
    {
        DirectlyMentioned,
        AllMentioned,
        NotMentionedMayReply,
        NotMentionedSkip
    }

    /// <summary>
    /// Determines whether this being should reply in a group chat session.
    /// Checks pending messages for @mentions targeting this being.
    /// - Directly mentioned (@name) → must reply
    /// - @all/@everyone → must reply
    /// - Not mentioned but pending messages → may reply (AI decides)
    /// - Not mentioned and no relevant context → skip (mark as read)
    /// </summary>
    private GroupChatMentionStatus ShouldReplyInGroupChat(SessionBase session, ContextManager brain)
    {
        if (session.Type != SessionType.GroupChat)
        {
            return GroupChatMentionStatus.NotMentionedMayReply;
        }

        ChatSystem? chatSystem = ServiceLocator.Instance.ChatSystem;
        if (chatSystem == null)
        {
            return GroupChatMentionStatus.NotMentionedMayReply;
        }

        List<ChatMessage> pendingMessages = chatSystem.GetPendingMessages(Id)
            .Where(m => m.ChannelId == session.Id)
            .ToList();

        if (pendingMessages.Count == 0)
        {
            return GroupChatMentionStatus.NotMentionedSkip;
        }

        bool anyDirectMention = false;
        bool anyAllMention = false;
        bool anyUnmentioned = false;

        foreach (var msg in pendingMessages)
        {
            if (msg.SenderId == Id)
            {
                continue;
            }

            if (msg.MentionedIds != null && msg.MentionedIds.Count > 0)
            {
                if (msg.MentionedIds.Contains(Guid.Empty))
                {
                    anyAllMention = true;
                }
                else if (msg.MentionedIds.Contains(Id))
                {
                    anyDirectMention = true;
                }
                else
                {
                    anyUnmentioned = true;
                }
            }
            else
            {
                anyUnmentioned = true;
            }
        }

        if (anyDirectMention)
        {
            return GroupChatMentionStatus.DirectlyMentioned;
        }

        if (anyAllMention)
        {
            return GroupChatMentionStatus.AllMentioned;
        }

        if (anyUnmentioned)
        {
            return GroupChatMentionStatus.NotMentionedMayReply;
        }

        return GroupChatMentionStatus.NotMentionedSkip;
    }

    /// <summary>
    /// Checks if the last AI response in the session was a mark_read action.
    /// This is used to skip thinking after AI explicitly marked messages as read (read but no reply).
    /// For group chats, checks if the last message from this being was mark_read.
    /// For single chats, maintains original logic of checking if user is in session.
    /// </summary>
    /// <param name="session">The chat session to check</param>
    /// <returns>True if the last assistant message called mark_read tool</returns>
    private bool WasJustMarkRead(SessionBase session)
    {
        try
        {
            // For single chat sessions (with user): skip mark_read logic
            if (session.Type != SessionType.GroupChat && session.Members.Contains(Config.Instance.Data.UserGuid))
            {
                return false;
            }
            // Get last 5 messages from session
            var messages = session.GetMessages(5);
            if (messages.Count < 2)
                return false;

            // Find the last assistant message from this being
            for (int i = messages.Count - 1; i >= 0; i--)
            {
                var msg = messages[i];
                // For group chats, only check messages from this being
                // For single chats, check any assistant message
                if (msg.Role == MessageRole.Assistant && msg.SenderId == Id && !string.IsNullOrEmpty(msg.ToolCallsJson))
                {
                    // Parse tool calls JSON
                    try
                    {
                        var toolCalls = JsonSerializer.Deserialize<List<ToolCall>>(msg.ToolCallsJson);
                        if (toolCalls != null)
                        {
                            // Check if any tool call is chat tool with mark_read action
                            foreach (var toolCall in toolCalls)
                            {
                                if (toolCall.Name == "chat" && toolCall.Arguments != null)
                                {
                                    if (toolCall.Arguments.TryGetValue("action", out var actionObj))
                                    {
                                        string? action = actionObj?.ToString()?.ToLowerInvariant();
                                        if (action == "mark_read")
                                        {
                                            _logger.Debug(Id, "Detected mark_read action in last assistant message from this being for session {0}", session.Id);
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (JsonException ex)
                    {
                        _logger.Warn(Id, "Failed to parse ToolCallsJson for session {0}: {1}", session.Id, ex.Message);
                    }

                    // Found an assistant message with tool calls from this being, but not mark_read
                    break;
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.Warn(Id, "WasJustMarkRead check failed for session {0}: {1}", session.Id, ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Checks if there is any timer work to do (new triggers or ongoing executions)
    /// </summary>
    private bool HasTimerWork()
    {
        if (TimerSystem == null) return false;

        // Check if there are executing timers (not yet completed)
        List<TimerItem> allTimers = TimerSystem.GetAll();
        bool hasExecuting = allTimers.Any(t =>
            t.ExecutionState == TimerExecutionState.Started ||
            t.ExecutionState == TimerExecutionState.Executing);

        return hasExecuting || TimerSystem.HasPendingTimers();
    }

    /// <summary>
    /// Gets timers that need processing (new triggers + ongoing executions)
    /// </summary>
    private List<TimerItem> GetTimersToProcess()
    {
        List<TimerItem> result = new();

        if (TimerSystem == null) return result;

        List<TimerItem> allTimers = TimerSystem.GetAll();
        foreach (var timer in allTimers)
        {
            if (timer.ExecutionState == TimerExecutionState.Started ||
                timer.ExecutionState == TimerExecutionState.Executing)
            {
                result.Add(timer);
            }
        }

        List<TimerItem> triggered = TimerSystem.Tick();
        foreach (var timer in triggered)
        {
            if (result.Any(t => t.Id == timer.Id))
            {
                switch (timer.OverlapPolicy)
                {
                    case TimerOverlapPolicy.Wait:
                        _logger.Info(Id, "Timer {0}: previous execution still running, waiting (OverlapPolicy=Wait)", timer.Name);
                        continue;
                    case TimerOverlapPolicy.Skip:
                        _logger.Info(Id, "Timer {0}: previous execution still running, skipping this trigger (OverlapPolicy=Skip)", timer.Name);
                        continue;
                    case TimerOverlapPolicy.ForceNew:
                        _logger.Info(Id, "Timer {0}: previous execution still running, forcing new cycle (OverlapPolicy=ForceNew)", timer.Name);
                        timer.SealCurrentCycle(TimerExecutionState.Failed);
                        timer.ExecutionState = TimerExecutionState.Idle;
                        timer.AppendNewCycle(timer.TriggerTime);
                        result.RemoveAll(t => t.Id == timer.Id);
                        result.Add(timer);
                        break;
                }
            }
            else
            {
                timer.ExecutionState = TimerExecutionState.Idle;
                timer.AppendNewCycle(timer.TriggerTime);
                result.Add(timer);
            }
        }

        return result;
    }

    private ProjectThinkSession? GetExecutingProjectThinkSession()
    {
        var projectManager = ServiceLocator.Instance.ProjectManager;
        if (projectManager == null) return null;

        var projects = projectManager.ListProjects(includeArchived: false);
        foreach (var project in projects)
        {
            if (project.Status != ProjectStatus.Active)
                continue;

            foreach (var session in project.ThinkSessions)
            {
                if (session.BeingId != Id)
                    continue;

                if (session.State == ProjectThinkState.Started || session.State == ProjectThinkState.Executing)
                {
                    if (session.NeedsContinuation())
                        return session;
                }
            }
        }
        return null;
    }

    private bool HasProjectsNeedingAttention()
    {
        var projectManager = ServiceLocator.Instance.ProjectManager;
        if (projectManager == null) return false;

        var projects = projectManager.ListProjects(includeArchived: false);
        foreach (var project in projects)
        {
            if (project.CreatedBy == Id && project.Status == ProjectStatus.Active)
            {
                if (!ShouldThinkOnProject(project))
                    continue;

                var reasons = GetProjectAttentionReasons(project);
                if (reasons.Count > 0)
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Determines the specific reasons why a project needs curator attention.
    /// Returns a list of reasons (empty means no attention needed).
    /// </summary>
    private static List<ProjectAttentionReason> GetProjectAttentionReasons(ProjectSpace project)
    {
        var reasons = new List<ProjectAttentionReason>();

        // 1) Missing workflow template
        if (string.IsNullOrEmpty(project.WorkflowTemplateName))
        {
            reasons.Add(ProjectAttentionReason.MissingTemplate);
            return reasons; // No point checking roles without a template
        }

        // 2) Has template but role pool is completely empty
        if (project.RoleAssignments.Count == 0)
        {
            reasons.Add(ProjectAttentionReason.EmptyRolePool);
            return reasons; // If no roles assigned at all, no need to check individual roles
        }

        // 3) Has template and role pool, but some roles don't meet requirements
        var workflowEngine = ServiceLocator.Instance.ProjectManager?.GetWorkflowEngine();
        if (workflowEngine != null)
        {
            var template = workflowEngine.GetTemplate(project.WorkflowTemplateName);
            if (template != null && template.RoleDefinitions.Count > 0)
            {
                template.ValidateRoleAssignments(project.RoleAssignments, out var unsatisfiedRoles);
                if (unsatisfiedRoles.Count > 0)
                {
                    reasons.Add(ProjectAttentionReason.UnsatisfiedRoles);
                }
            }
        }

        return reasons;
    }

    private bool ShouldThinkOnProject(ProjectSpace project)
    {
        var taskSystem = ServiceLocator.Instance.ProjectManager?.GetTaskSystem(project.Id);
        if (taskSystem == null) return true;

        var tasks = taskSystem.GetAll();

        if (tasks.Count == 0) return true;

        if (tasks.All(t => t.Status == Collective.TaskStatus.Completed)) return true;

        if (DateTime.UtcNow - project.UpdatedAt > TimeSpan.FromMinutes(10))
        {
            var hasStuckTasks = tasks.Any(t =>
                t.Status == Collective.TaskStatus.Running &&
                t.StartedAt.HasValue &&
                DateTime.UtcNow - t.StartedAt.Value > TimeSpan.FromMinutes(15));
            if (hasStuckTasks) return true;
        }

        return false;
    }
}