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
    private bool _isProcessing;

    /// <summary>
    /// Gets whether this silicon being is idle (no pending tasks)
    /// </summary>
    public override bool IsIdle => ChatService?.GetPendingMessages(Id).Count == 0 && !_isProcessing;

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
        : base(id, name)
    {
        _aiClient = aiClient;
        _soulContent = soulContent;
        _isProcessing = false;
    }

    /// <summary>
    /// Called by SiliconBeingManager on each tick
    /// Checks for messages and processes them
    /// </summary>
    /// <param name="deltaTime">Time elapsed since the last tick</param>
    public override void Tick(TimeSpan deltaTime)
    {
        if (!_isProcessing && ChatService != null)
        {
            ContextManager contextManager = new ContextManager(_aiClient, _soulContent, ChatService, Id);

            if (!contextManager.HasPendingMessages())
            {
                return;
            }

            _isProcessing = true;
            try
            {
                // Get current localization
                Language language = Config.Instance.Data.Language;
                DefaultLocalizationBase localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(language);
                
                Console.WriteLine($"{Name}: {localization.ThinkingMessage}");

                contextManager.FetchPendingMessages();

                AIResponse response = contextManager.GetResponse();

                if (response.Success && !string.IsNullOrEmpty(response.Content))
                {
                    Console.WriteLine($"{Name}: {response.Content}");
                    Console.WriteLine();
                    contextManager.MarkMessageProcessed();
                }
                else
                {
                    Console.WriteLine($"{Name}: {localization.ErrorMessage} {response.ErrorMessage}");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                // Get current localization
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
