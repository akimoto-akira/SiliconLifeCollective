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
/// System information tool.
/// Queries processes and environment variables through CommandLineExecutor.
/// Verifies the executor pipeline.
/// </summary>
public class SystemTool : ITool
{
    public string Name => "system";

    public string Description =>
        "Query system information. Actions: 'list_processes' (running processes), " +
        "'get_env' (specific environment variable), 'get_env_all' (all environment variables), " +
        "'system_info' (OS and runtime information).";

    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

    public Dictionary<string, object> GetParameterSchema()
    {
        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The action to perform: list_processes, get_env, get_env_all, system_info",
                    ["enum"] = new[] { "list_processes", "get_env", "get_env_all", "system_info" }
                },
                ["variable"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Environment variable name (for get_env action)"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj))
        {
            return ToolResult.Failed("Missing 'action' parameter");
        }

        string action = actionObj?.ToString() ?? "";

        return action.ToLowerInvariant() switch
        {
            "list_processes" => ExecuteListProcesses(callerId),
            "get_env" => ExecuteGetEnv(parameters),
            "get_env_all" => ExecuteGetEnvAll(),
            "system_info" => ExecuteSystemInfo(),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    private ToolResult ExecuteListProcesses(Guid callerId)
    {
        string command = OperatingSystem.IsWindows()
            ? "tasklist /FO CSV /NH"
            : "ps aux --no-headers";

        ExecutorRequest request = new(callerId, command, "list_processes");
        ExecutorResult result = CommandLineExecutor.Execute(request, TimeSpan.FromSeconds(10));

        if (result.Success)
        {
            // Limit output to avoid overwhelming context
            string[] lines = (result.Output ?? "").Split('\n', StringSplitOptions.RemoveEmptyEntries);
            string limitedOutput = lines.Length > 30
                ? string.Join("\n", lines.Take(30)) + $"\n... ({lines.Length - 30} more processes)"
                : result.Output ?? "";

            return ToolResult.Successful(limitedOutput);
        }

        return ToolResult.Failed(result.Error ?? "Failed to list processes");
    }

    private ToolResult ExecuteGetEnv(Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("variable", out object? varObj) || string.IsNullOrWhiteSpace(varObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'variable' parameter for get_env");
        }

        string varName = varObj.ToString()!;
        string? value = Environment.GetEnvironmentVariable(varName);

        if (value != null)
        {
            return ToolResult.Successful($"{varName}={value}");
        }

        return ToolResult.Failed($"Environment variable '{varName}' not found");
    }

    private ToolResult ExecuteGetEnvAll()
    {
        var vars = Environment.GetEnvironmentVariables();
        var sorted = new List<string>();
        foreach (System.Collections.DictionaryEntry entry in vars)
        {
            sorted.Add($"{entry.Key}={entry.Value}");
        }
        sorted.Sort(StringComparer.OrdinalIgnoreCase);

        // Limit output
        string output = sorted.Count > 50
            ? string.Join("\n", sorted.Take(50)) + $"\n... ({sorted.Count - 50} more variables)"
            : string.Join("\n", sorted);

        return ToolResult.Successful(output);
    }

    private ToolResult ExecuteSystemInfo()
    {
        string info = $"OS: {Environment.OSVersion.Platform} {Environment.OSVersion.VersionString}\n" +
            $"Machine: {Environment.MachineName}\n" +
            $".NET: {Environment.Version}\n" +
            $"Processor Count: {Environment.ProcessorCount}\n" +
            $"Working Directory: {Environment.CurrentDirectory}\n" +
            $"64-bit OS: {Environment.Is64BitOperatingSystem}\n" +
            $"64-bit Process: {Environment.Is64BitProcess}";

        return ToolResult.Successful(info);
    }
}
