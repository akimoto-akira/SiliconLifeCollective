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
/// Tick object for handling console input and forwarding to silicon beings
/// </summary>
public class ConsoleTickObject : TickObject
{
    private readonly DefaultSiliconBeing _siliconBeing;
    private readonly DefaultLocalizationBase _localization;

    /// <summary>
    /// Initializes a new instance of the ConsoleTickObject class
    /// </summary>
    /// <param name="siliconBeing">The silicon being to forward messages to</param>
    /// <param name="localization">The localization instance</param>
    public ConsoleTickObject(DefaultSiliconBeing siliconBeing, DefaultLocalizationBase localization)
        : base(TimeSpan.Zero, true)
    {
        _siliconBeing = siliconBeing;
        _localization = localization;
    }

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
                Program.RequestExit();
                return;
            }

            _siliconBeing.ChatService?.AddMessage(Guid.Empty, _siliconBeing.Id, input);
        }
    }
}
