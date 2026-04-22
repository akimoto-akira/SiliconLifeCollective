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
using System.Text.Json;

namespace SiliconLife.Default;

/// <summary>
/// Default implementation of a silicon being.
/// The being is the "body" — it stays alive, detects trigger scenes,
/// and calls the corresponding method on the brain (ContextManager).
/// </summary>
public class DefaultSiliconBeing : SiliconBeingBase
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<DefaultSiliconBeing>();
    private volatile bool _isProcessing;

    /// <summary>
    /// Gets whether this silicon being is idle (no pending tasks and not thinking).
    /// </summary>
    public override bool IsIdle => !_isProcessing;

    /// <summary>
    /// Initializes a new instance of the DefaultSiliconBeing class
    /// </summary>
    /// <param name="id">The unique identifier</param>
    /// <param name="name">The name of the silicon being</param>
    public DefaultSiliconBeing(Guid id, string name)
        : base(id, name)
    {
        _isProcessing = false;
    }

    /// <summary>
    /// Saves this being's state (name, AI config) to state.json in its directory.
    /// Called by the factory on first creation and when config changes.
    /// </summary>
    public void SaveState()
    {
        if (BeingDirectory == null) return;
        try
        {
            var state = new Dictionary<string, object>
            {
                ["name"] = Name,
                ["aiClientType"] = AIClientType ?? "",
                ["aiConfig"] = AIClientConfig ?? new Dictionary<string, object>()
            };
            
            string json = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path.Combine(BeingDirectory, "state.json"), json);
            
            _logger.Debug(Id, "Being {0}: state saved to {1}", Name, Path.Combine(BeingDirectory, "state.json"));
        }
        catch (Exception ex)
        {
            _logger.Warn(Id, "Being {0}: failed to save state", Name, ex);
        }
    }

    /// <summary>
    /// Loads this being's name and AI config from state.json in its directory.
    /// Returns true if state was loaded successfully.
    /// </summary>
    public bool LoadState()
    {
        if (BeingDirectory == null) return false;
        try
        {
            string stateFilePath = Path.Combine(BeingDirectory, "state.json");
            if (!File.Exists(stateFilePath)) return false;

            string json = File.ReadAllText(stateFilePath);
            var state = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            
            if (state != null)
            {
                // Load name
                if (state.TryGetValue("name", out var nameObj))
                {
                    Name = nameObj?.ToString() ?? "";
                }
                
                // Load aiClientType
                if (state.TryGetValue("aiClientType", out var typeObj))
                {
                    AIClientType = typeObj?.ToString();
                }
                
                // Load aiConfig
                if (state.TryGetValue("aiConfig", out var configObj))
                {
                    if (configObj is JsonElement configElement)
                    {
                        AIClientConfig = DeserializeDictionary(configElement);
                    }
                    else if (configObj is Dictionary<string, object> dict)
                    {
                        AIClientConfig = dict;
                    }
                }
                
                // Initialize backup config
                BackupAIClientConfig = AIClientConfig?.ToDictionary(k => k.Key, v => v.Value);
                
                _logger.Debug(Id, "Being {0}: state loaded from {1}", Name, stateFilePath);

                Language language = Config.Instance?.Data?.Language ?? Language.ZhCN;
                if (LocalizationManager.Instance.TryGetLocalization(language, out LocalizationBase? loc) &&
                    loc is DefaultLocalizationBase defaultLoc)
                {
                    Memory?.Add(defaultLoc.FormatMemoryEventStartup());
                }

                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.Warn(Id, "Being {0}: failed to load state", Name, ex);
        }
        return false;
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
    /// Priority: AI Config Change > Continuation > Chat > Timer > Task > MemoryCompression
    /// </summary>
    /// <param name="deltaTime">Time elapsed since the last tick</param>
    public override void Tick(TimeSpan deltaTime)
    {
        // 1. Check if AI config has changed
        if (CheckAndRebuildAIClient())
        {
            // Config changed and client rebuilt, skip this tick
            return;
        }
        
        // 2. Original tick logic
        if (_isProcessing || AIClient == null)
        {
            return;
        }

        _isProcessing = true;
        try
        {
            var sessions = BuildSessionList();

            foreach (var session in sessions)
            {
                if (ContextManager.NeedsContinuation(this, session))
                {
                    _logger.Info(Id, "Being {0}: detected continuation in session {1}", Name, session.Id);
                    ExecuteBrain("ThinkContinuation", session, brain => brain.ThinkOnChat());
                    return;
                }
            }

            foreach (var session in sessions)
            {
                ContextManager brain = new ContextManager(this, session);
                if (brain.HasWork)
                {
                    _logger.Info(Id, "Being {0}: detected pending messages in session {1}", Name, session.Id);
                    ExecuteBrain("ThinkOnChat", session, _ => brain.ThinkOnChat());
                    return;
                }
            }

            if (TimerSystem != null && TimerSystem.HasPendingTimers())
            {
                List<TimerItem> triggeredTimers = TimerSystem.Tick();
                foreach (TimerItem timer in triggeredTimers)
                {
                    _logger.Info(Id, "Being {0}: timer triggered - {1} ({2})", Name, timer.Name, timer.Id);
                    ExecuteBrain("ThinkOnTimer", null, _ => new ContextManager(this, null).ThinkOnTimer(timer));
                }
                return;
            }

            if (TaskSystem != null && TaskSystem.HasPendingTasks())
            {
                List<TaskItem> runnable = TaskSystem.GetRunnableTasks();
                if (runnable.Count > 0)
                {
                    TaskItem task = runnable[0];
                    _logger.Info(Id, "Being {0}: pending task detected - {1} ({2})", Name, task.Title, task.Id);
                    ExecuteBrain("ThinkOnTask", null, _ => new ContextManager(this, null).ThinkOnTask(task));
                    return;
                }
            }

            if (Memory != null && Memory.ShouldCompress())
            {
                return;
                _logger.Debug(Id, "Being {0}: memory compression needed", Name);
                ExecuteBrain("ThinkOnMemoryCompress", null, _ => new ContextManager(this, null).ThinkOnMemoryCompress());
                return;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(Id, "Being {0}: unexpected error during tick", Name, ex);

            Language language = Config.Instance.Data.Language;
            DefaultLocalizationBase localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(language);

            Memory?.Add(localization.FormatMemoryEventRuntimeError(ex.Message));
            _logger.Info(Id, "{0}: {1} {2}", Name, localization.UnexpectedErrorMessage, ex.Message);
        }
        finally
        {
            _isProcessing = false;
        }
    }

    /// <summary>
    /// Builds the list of chat sessions:
    /// single chat sessions with the project user ID + all other silicon beings (excluding self).
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

        return sessions;
    }

    /// <summary>
    /// Executes a brain function with logging and continuation tracking.
    /// </summary>
    private void ExecuteBrain(string sceneName, SessionBase? session, Func<ContextManager, AIResponse> thinkFunc)
    {
        _logger.Info(Id, "Being {0}: executing brain scene {1}", Name, sceneName);

        Language language = Config.Instance.Data.Language;
        DefaultLocalizationBase localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(language);

        _logger.Info(Id, "{0}: {1}", Name, localization.ThinkingMessage);

        ContextManager brain = new ContextManager(this, session);

        AIResponse response;
        if (sceneName == "ThinkOnChat" || sceneName == "ThinkContinuation")
        {
            response = brain.ThinkOnChatStreamAsync().GetAwaiter().GetResult();
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
        }
        else
        {
            _logger.Debug(Id, "Being {0}: brain scene {1} completed", Name, sceneName);
        }
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
}