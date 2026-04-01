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

using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Default implementation of a silicon being
/// </summary>
public class DefaultSiliconBeing : SiliconBeingBase
{
    private readonly IAIClient _aiClient;
    private readonly string? _soulContent;
    private readonly ChatSystem? _chatSystem;
    private readonly IMManager? _imManager;
    private readonly Guid _userId;
    private volatile bool _isProcessing;

    /// <summary>
    /// Gets whether this silicon being is idle (no pending tasks)
    /// </summary>
    public override bool IsIdle
    {
        get
        {
            if (_chatSystem != null)
            {
                return _chatSystem.GetPendingMessages(Id).Count == 0 && !_isProcessing;
            }
            return ChatService?.GetPendingMessages(Id).Count == 0 && !_isProcessing;
        }
    }

    /// <summary>
    /// Initializes a new instance of the DefaultSiliconBeing class
    /// </summary>
    /// <param name="id">The unique identifier</param>
    /// <param name="name">The name of the silicon being</param>
    /// <param name="aiClient">The AI client</param>
    /// <param name="beingDirectory">The directory path for this being</param>
    /// <param name="soulContent">The soul content for this being</param>
    public DefaultSiliconBeing(
        Guid id,
        string name,
        IAIClient aiClient,
        string beingDirectory,
        string? soulContent)
        : this(id, name, aiClient, beingDirectory, soulContent, null, null, Guid.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the DefaultSiliconBeing class with ChatSystem and IMManager
    /// </summary>
    public DefaultSiliconBeing(
        Guid id,
        string name,
        IAIClient aiClient,
        string beingDirectory,
        string? soulContent,
        ChatSystem chatSystem,
        IMManager imManager,
        Guid userId)
        : base(id, name)
    {
        _aiClient = aiClient;
        _soulContent = soulContent;
        _chatSystem = chatSystem;
        _imManager = imManager;
        _userId = userId;
        _isProcessing = false;
    }

    /// <summary>
    /// Called by SiliconBeingManager on each tick
    /// Checks for messages and processes them
    /// </summary>
    /// <param name="deltaTime">Time elapsed since the last tick</param>
    public override void Tick(TimeSpan deltaTime)
    {
        if (!_isProcessing)
        {
            ContextManager contextManager;

            if (_chatSystem != null)
            {
                List<ChatMessage> pendingMessages = _chatSystem.GetPendingMessages(Id);
                if (pendingMessages.Count == 0)
                {
                    return;
                }

                contextManager = new ContextManager(_aiClient, _soulContent, _chatSystem, Id, _userId);
            }
            else if (ChatService != null)
            {
                contextManager = new ContextManager(_aiClient, _soulContent, ChatService, Id);

                if (!contextManager.HasPendingMessages())
                {
                    return;
                }
            }
            else
            {
                return;
            }

            _isProcessing = true;
            try
            {
                Language language = Config.Instance.Data.Language;
                DefaultLocalizationBase localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(language);
                
                Console.WriteLine($"{Name}: {localization.ThinkingMessage}");

                if (_chatSystem == null)
                {
                    contextManager.FetchPendingMessages();
                }

                AIResponse response = contextManager.GetResponse();

                if (response.Success && !string.IsNullOrEmpty(response.Content))
                {
                    _chatSystem?.AddMessage(Id, _userId, response.Content);
                    contextManager.MarkMessageProcessed();

                    if (_imManager != null)
                    {
                        _ = _imManager.SendMessageAsync(Id, _userId, $"{Name}: {response.Content}");
                    }
                    else
                    {
                        Console.WriteLine($"{Name}: {response.Content}");
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"{Name}: {localization.ErrorMessage} {response.ErrorMessage}");
                    Console.WriteLine();
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
    }
}
