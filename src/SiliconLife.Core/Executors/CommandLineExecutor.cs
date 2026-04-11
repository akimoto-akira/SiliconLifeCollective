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

using System.Diagnostics;

namespace SiliconLife.Collective;

/// <summary>
/// Static executor for command-line operations.
/// Provides timeout control, output capture, and cross-platform separator detection.
/// </summary>
public static class CommandLineExecutor
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger(typeof(CommandLineExecutor));
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Command separators that could be used for command injection.
    /// Detected across platforms: ||, &&, |, &, ;
    /// </summary>
    private static readonly string[] DangerousSeparators = ["||", "&&", "|", "&", ";"];

    /// <summary>
    /// Executes a command-line request synchronously with timeout
    /// </summary>
    public static ExecutorResult Execute(ExecutorRequest request, TimeSpan? timeout = null)
    {
        TimeSpan actualTimeout = timeout ?? DefaultTimeout;

        try
        {
            Task<ExecutorResult> task = Task.Run(() => ExecuteCore(request));
            if (task.Wait(actualTimeout))
            {
                return task.Result;
            }
            return ExecutorResult.Failed("Operation timed out");
        }
        catch (AggregateException ex)
        {
            Exception? inner = ex.InnerException;
            return ExecutorResult.Failed(inner?.Message ?? ex.Message);
        }
    }

    private static ExecutorResult ExecuteCore(ExecutorRequest request)
    {
        string command = request.ResourcePath;

        if (string.IsNullOrWhiteSpace(command))
        {
            return ExecutorResult.Failed("Command is empty");
        }

        string? detectedSeparator = DetectDangerousSeparator(command);
        if (detectedSeparator != null)
        {
            _logger.Warn("Command contains dangerous separator '{Separator}': {Command}", detectedSeparator, command);
            return ExecutorResult.Failed(
                $"Command contains dangerous separator '{detectedSeparator}'. Multi-command execution is not allowed.");
        }

        _logger.Info("Command: {Command}", command);

        try
        {
            ProcessStartInfo psi;
            if (OperatingSystem.IsWindows())
            {
                psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
            }
            else
            {
                psi = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{command.Replace("\"", "\\\"")}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
            }

            using Process process = new Process { StartInfo = psi };
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (!process.WaitForExit((int)DefaultTimeout.TotalMilliseconds))
            {
                try { process.Kill(); } catch { }
                _logger.Warn("Command timed out: {Command}", command);
                return ExecutorResult.Failed("Command timed out");
            }

            string fullOutput = string.IsNullOrEmpty(output) ? error :
                string.IsNullOrEmpty(error) ? output : $"{output}\n{error}";

            if (process.ExitCode == 0)
            {
                _logger.Debug("Command output: {Output}", fullOutput.Length > 200 ? fullOutput.Substring(0, 200) + "..." : fullOutput);
                return ExecutorResult.Successful(fullOutput, process.ExitCode);
            }

            _logger.Error("Command failed: exit code={ExitCode}", process.ExitCode);
            return ExecutorResult.Failed($"Exit code {process.ExitCode}: {error}", process.ExitCode);
        }
        catch (Exception ex)
        {
            _logger.Error("Command execution failed: {Command}, {Exception}", ex, command);
            return ExecutorResult.Failed($"Command execution failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Detects dangerous command separators that could allow command injection.
    /// </summary>
    private static string? DetectDangerousSeparator(string command)
    {
        foreach (string separator in DangerousSeparators)
        {
            if (command.Contains($" {separator} ") ||
                command.Contains($" {separator}") ||
                command.Contains($"{separator} "))
            {
                return separator;
            }
        }
        return null;
    }

    /// <summary>
    /// Checks permission for a command-line operation via the caller's PermissionManager.
    /// </summary>
    private static bool CheckPermission(ExecutorRequest request)
    {
        PermissionManager? pm = ServiceLocator.Instance.GetPermissionManager(request.CallerId);
        if (pm == null) return true;
        return pm.CheckPermission(request.CallerId, PermissionType.CommandLine, request.ResourcePath);
    }
}
