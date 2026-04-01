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
/// Tick object for handling console input and AI chat
/// </summary>
public class ConsoleTickObject : TickObject
{
    private readonly IAIClient _aiClient;
    private readonly DefaultLocalizationBase _localization;
    private bool _shouldExit = false;

    /// <summary>
    /// Initializes a new instance of the ConsoleTickObject class
    /// </summary>
    /// <param name="aiClient">The AI client</param>
    /// <param name="localization">The localization instance</param>
    public ConsoleTickObject(IAIClient aiClient, DefaultLocalizationBase localization)
        : base(TimeSpan.Zero, true)
    {
        _aiClient = aiClient;
        _localization = localization;
    }

    /// <summary>
    /// Gets whether the console should exit
    /// </summary>
    public bool ShouldExit => _shouldExit;

    /// <summary>
    /// Called when the tick interval has elapsed
    /// </summary>
    /// <param name="deltaTime">Time elapsed since the last tick</param>
    protected override void OnTick(TimeSpan deltaTime)
    {
        if (Console.KeyAvailable)
        {
            Console.Write(_localization.InputPrompt);
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine(_localization.ShutdownMessage);
                _shouldExit = true;
                return;
            }

            ProcessInputAsync(input).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Processes user input and gets AI response
    /// </summary>
    /// <param name="input">The user input</param>
    private async Task ProcessInputAsync(string input)
    {
        try
        {
            Console.WriteLine("Thinking...");
            AIResponse response = await _aiClient.ChatAsync(input);

            if (response.Success)
            {
                Console.WriteLine(response.Content);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"Error: {response.ErrorMessage}");
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            Console.WriteLine();
        }
    }
}
