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
/// Default implementation of a silicon being.
/// The being is the "body" — it stays alive, detects trigger scenes,
/// and calls the corresponding method on the brain (ContextManager).
/// </summary>
public class DefaultSiliconBeing : SiliconBeingBase
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<DefaultSiliconBeing>();
    private volatile bool _isProcessing;

    /// <summary>
    /// Gets the data directory path for this silicon being
    /// </summary>
    public string? BeingDirectory { get; set; }

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
    /// Saves this being's state (name) to state.json in its directory.
    /// Called by the factory on first creation.
    /// </summary>
    public void SaveState()
    {
        if (BeingDirectory == null) return;
        try
        {
            var state = new Dictionary<string, string> { ["name"] = Name };
            string json = System.Text.Json.JsonSerializer.Serialize(state, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path.Combine(BeingDirectory, "state.json"), json);
            _logger.Debug("Being {0}: state saved to {1}", Name, Path.Combine(BeingDirectory, "state.json"));
        }
        catch (Exception ex)
        {
            _logger.Warn("Being {0}: failed to save state", Name, ex);
        }
    }

    /// <summary>
    /// Loads this being's name from state.json in its directory.
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
            var state = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            if (state != null && state.TryGetValue("name", out string? name))
            {
                Name = name;
                _logger.Debug("Being {0}: state loaded from {1}", Name, stateFilePath);

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
            _logger.Warn("Being {0}: failed to load state", Name, ex);
        }
        return false;
    }

    /// <summary>
    /// Called by SiliconBeingManager on each tick.
    /// Detects the trigger scene and calls the corresponding brain method.
    /// Priority: Continuation > Chat > Timer > Task > MemoryCompression
    /// </summary>
    /// <param name="deltaTime">Time elapsed since the last tick</param>
    public override void Tick(TimeSpan deltaTime)
    {
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
                    _logger.Info("Being {0}: detected continuation in session {1}", Name, session.Id);
                    ExecuteBrain("ThinkContinuation", session, brain => brain.ThinkOnChat());
                    return;
                }
            }

            foreach (var session in sessions)
            {
                ContextManager brain = new ContextManager(this, session);
                if (brain.HasWork)
                {
                    _logger.Info("Being {0}: detected pending messages in session {1}", Name, session.Id);
                    ExecuteBrain("ThinkOnChat", session, _ => brain.ThinkOnChat());
                    return;
                }
            }

            if (TimerSystem != null && TimerSystem.HasPendingTimers())
            {
                List<TimerItem> triggeredTimers = TimerSystem.Tick();
                foreach (TimerItem timer in triggeredTimers)
                {
                    _logger.Info("Being {0}: timer triggered - {1} ({2})", Name, timer.Name, timer.Id);
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
                    _logger.Info("Being {0}: pending task detected - {1} ({2})", Name, task.Title, task.Id);
                    ExecuteBrain("ThinkOnTask", null, _ => new ContextManager(this, null).ThinkOnTask(task));
                    return;
                }
            }

            if (Memory != null && Memory.ShouldCompress())
            {
                return;
                _logger.Debug("Being {0}: memory compression needed", Name);
                ExecuteBrain("ThinkOnMemoryCompress", null, _ => new ContextManager(this, null).ThinkOnMemoryCompress());
                return;
            }
        }
        catch (Exception ex)
        {
            _logger.Error("Being {0}: unexpected error during tick", Name, ex);

            Language language = Config.Instance.Data.Language;
            DefaultLocalizationBase localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(language);

            Memory?.Add(localization.FormatMemoryEventRuntimeError(ex.Message));
            _logger.Info("{0}: {1} {2}", Name, localization.UnexpectedErrorMessage, ex.Message);
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
        _logger.Info("Being {0}: executing brain scene {1}", Name, sceneName);

        Language language = Config.Instance.Data.Language;
        DefaultLocalizationBase localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(language);

        _logger.Info("{0}: {1}", Name, localization.ThinkingMessage);

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
            _logger.Info("{0}: {1}", Name, localization.ToolCallMessage);
        }
        else if (!response.Success)
        {
            _logger.Error("Being {0}: brain scene {1} failed: {2}", Name, sceneName, response.ErrorMessage ?? "unknown");
            _logger.Info("{0}: {1} {2}", Name, localization.ErrorMessage, response.ErrorMessage);
        }
        else
        {
            _logger.Debug("Being {0}: brain scene {1} completed", Name, sceneName);
        }
    }
}