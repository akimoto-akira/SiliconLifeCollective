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

    /// <summary>
    /// Modes for the input loop to know how to route the next line of input.
    /// </summary>
    private enum InputMode
    {
        Normal,
        Permission,
        Cache
    }

    private readonly object _modeLock = new();
    private InputMode _mode = InputMode.Normal;
    private TaskCompletionSource<string>? _inputTcs;

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

    public Task<AskPermissionResult> AskPermissionAsync(PermissionType permissionType, string resource, string allowCode, string denyCode)
    {
        DefaultLocalizationBase localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(
            Config.Instance.Data.Language);

        // Switch to permission mode and wait for user input
        TaskCompletionSource<string> tcs;
        lock (_modeLock)
        {
            _mode = InputMode.Permission;
            _inputTcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            tcs = _inputTcs;
        }

        Console.WriteLine();
        Console.WriteLine($"{localization.PermissionRequestHeader} {localization.GetPermissionTypeName(permissionType)}: {resource}");
        Console.WriteLine($"  {localization.AllowCodeLabel}: {allowCode}");
        Console.WriteLine($"  {localization.DenyCodeLabel}:  {denyCode}");
        Console.WriteLine($"  {localization.PermissionReplyInstruction}");
        Console.Write("> ");

        string input = tcs.Task.GetAwaiter().GetResult();

        bool allowed = string.Equals(input, allowCode, StringComparison.OrdinalIgnoreCase);
        bool addToCache = false;

        if (allowed)
        {
            // Switch to cache mode and wait for the next input
            TaskCompletionSource<string> cacheTcs;
            lock (_modeLock)
            {
                _mode = InputMode.Cache;
                _inputTcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
                cacheTcs = _inputTcs;
            }

            Console.Write($"{localization.AddToCachePrompt} ");
            string? cacheInput = cacheTcs.Task.GetAwaiter().GetResult();
            addToCache = string.Equals(cacheInput, "y", StringComparison.OrdinalIgnoreCase);
        }

        // Return to normal mode
        lock (_modeLock)
        {
            _mode = InputMode.Normal;
            _inputTcs = null;
        }

        return Task.FromResult(new AskPermissionResult { Allowed = allowed, AddToCache = addToCache });
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

            string trimmed = line.Trim();

            if (trimmed.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                trimmed.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                ExitRequested?.Invoke(this, EventArgs.Empty);
                return;
            }

            TaskCompletionSource<string>? tcs;
            InputMode currentMode;
            lock (_modeLock)
            {
                currentMode = _mode;
                tcs = _inputTcs;
            }

            if (tcs != null && currentMode != InputMode.Normal)
            {
                tcs.TrySetResult(trimmed);
                continue;
            }

            // Dispatch message processing to a background thread
            // so RunInputLoop is never blocked by event handlers
            ChatMessage msg = new(_userId, _curatorGuid, trimmed);
            _ = Task.Run(() => MessageReceived?.Invoke(this, new IMMessageEventArgs(msg)));
        }
    }
}
