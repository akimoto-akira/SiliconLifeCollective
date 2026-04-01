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

public class ConsoleIMProvider : IIMProvider
{
    public event EventHandler<IMMessageEventArgs>? MessageReceived;
    public event EventHandler? ExitRequested;

    private bool _isRunning;
    private Task? _inputTask;
    private readonly Guid _userId;
    private readonly Guid _curatorGuid;

    public ConsoleIMProvider(Guid userId, Guid curatorGuid)
    {
        _userId = userId;
        _curatorGuid = curatorGuid;
    }

    public Task StartAsync()
    {
        _isRunning = true;
        _inputTask = Task.Run(RunInputLoop);
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        _isRunning = false;
        return Task.CompletedTask;
    }

    public Task SendMessageAsync(Guid senderId, Guid receiverId, string content)
    {
        Console.WriteLine(content);
        return Task.CompletedTask;
    }

    private async Task RunInputLoop()
    {
        while (_isRunning)
        {
            Console.Write("> ");
            string? line = await Task.Run(() => Console.ReadLine());

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                line.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                ExitRequested?.Invoke(this, EventArgs.Empty);
                return;
            }

            MessageReceived?.Invoke(this, new IMMessageEventArgs(new ChatMessage(_userId, _curatorGuid, line)));
        }
    }
}
