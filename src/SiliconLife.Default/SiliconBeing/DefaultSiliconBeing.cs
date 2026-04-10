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
        }
        catch
        {
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
                return true;
            }
        }
        catch
        {
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

            // Check continuation across all sessions (highest priority)
            foreach (var session in sessions)
            {
                if (ContextManager.NeedsContinuation(this, session))
                {
                    ExecuteBrain("ThinkContinuation", session, brain => brain.ThinkOnChat());
                    return;
                }
            }

            // Check for pending messages from any session
            foreach (var session in sessions)
            {
                ContextManager brain = new ContextManager(this, session);
                if (brain.HasWork)
                {
                    ExecuteBrain("ThinkOnChat", session, _ => brain.ThinkOnChat());
                    return;
                }
            }

            if (TimerSystem != null && TimerSystem.HasPendingTimers())
            {
                ExecuteBrain("ThinkOnTimer", null, _ => new ContextManager(this, null).ThinkOnTimer());
                return;
            }

            if (TaskSystem != null && TaskSystem.HasPendingTasks())
            {
                ExecuteBrain("ThinkOnTask", null, _ => new ContextManager(this, null).ThinkOnTask());
                return;
            }

            if (Memory != null && Memory.ShouldCompress())
            {
                ExecuteBrain("ThinkOnMemoryCompress", null, _ => new ContextManager(this, null).ThinkOnMemoryCompress());
                return;
            }
        }
        catch (Exception ex)
        {
            Language language = Config.Instance.Data.Language;
            DefaultLocalizationBase localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(language);

            Console.WriteLine($"{Name}: {localization.UnexpectedErrorMessage} {ex.Message}");
            Console.WriteLine();
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
    private List<ISession> BuildSessionList()
    {
        var sessions = new List<ISession>();
        ChatSystem? chatSystem = ServiceLocator.Instance.ChatSystem;
        if (chatSystem == null)
        {
            return sessions;
        }

        Guid userId = Config.Instance.Data.UserGuid;
        if (userId != Guid.Empty)
        {
            ISession session = chatSystem.GetOrCreateSession(Id, userId);
            sessions.Add(session);
        }

        SiliconBeingManager? beingManager = ServiceLocator.Instance.BeingManager;
        if (beingManager != null)
        {
            foreach (var other in beingManager.GetAllBeings())
            {
                if (other.Id != Id)
                {
                    ISession session = chatSystem.GetOrCreateSession(Id, other.Id);
                    sessions.Add(session);
                }
            }
        }

        return sessions;
    }

    /// <summary>
    /// Executes a brain function with logging and continuation tracking.
    /// </summary>
    private void ExecuteBrain(string sceneName, ISession? session, Func<ContextManager, AIResponse> thinkFunc)
    {
        Language language = Config.Instance.Data.Language;
        DefaultLocalizationBase localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(language);

        Console.WriteLine($"{Name}: {localization.ThinkingMessage}");

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
            Console.WriteLine($"{Name}: {localization.ToolCallMessage}");
        }
        else if (!response.Success)
        {
            Console.WriteLine($"{Name}: {localization.ErrorMessage} {response.ErrorMessage}");
            Console.WriteLine();
        }
    }
}