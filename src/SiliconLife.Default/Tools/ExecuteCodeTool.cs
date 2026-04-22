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
using System.Text;
using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Code execution tool for running scripts in multiple languages.
/// Only available to the Silicon Curator (main administrator).
/// Supports executing code snippets in various programming languages.
/// </summary>
[SiliconManagerOnly]
public class ExecuteCodeTool : ITool
{
    public string Name => "execute_code";

    public string Description =>
        "Execute code scripts in various programming languages. Actions: run_script (execute code), " +
        "list_languages (list supported languages), install_package (install dependency packages). " +
        "WARNING: This tool can execute arbitrary code and should only be used by trusted administrators.";

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
                    ["description"] = "Action to perform: run_script, list_languages, install_package",
                    ["enum"] = new[] { "run_script", "list_languages", "install_package" }
                },
                ["language"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Programming language like 'python', 'javascript', 'bash', 'powershell'"
                },
                ["code"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Code to execute (for run_script action)"
                },
                ["timeout"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Execution timeout in seconds (for run_script action), default 30"
                },
                ["stdin"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Standard input content (for run_script action)"
                },
                ["package"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Package name to install (for install_package action)"
                },
                ["version"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Package version to install (for install_package action)"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj) || string.IsNullOrWhiteSpace(actionObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'action' parameter");
        }

        string action = actionObj.ToString()!.ToLowerInvariant();

        return action switch
        {
            "run_script" => ExecuteRunScript(callerId, parameters),
            "list_languages" => ExecuteListLanguages(callerId, parameters),
            "install_package" => ExecuteInstallPackage(callerId, parameters),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    private ToolResult ExecuteRunScript(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("language", out object? langObj) || string.IsNullOrWhiteSpace(langObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'language' parameter for run_script action");
        }

        if (!parameters.TryGetValue("code", out object? codeObj) || string.IsNullOrWhiteSpace(codeObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'code' parameter for run_script action");
        }

        string language = langObj.ToString()!.ToLowerInvariant();
        string code = codeObj.ToString()!;
        int timeout = 30;
        string? stdin = null;

        if (parameters.TryGetValue("timeout", out object? timeoutObj) && timeoutObj != null)
        {
            if (int.TryParse(timeoutObj.ToString(), out int parsedTimeout))
            {
                timeout = parsedTimeout;
            }
        }

        if (parameters.TryGetValue("stdin", out object? stdinObj) && stdinObj != null)
        {
            stdin = stdinObj.ToString();
        }

        // Get the interpreter path for the specified language
        string? interpreterPath = GetInterpreterPath(language);
        if (string.IsNullOrEmpty(interpreterPath))
        {
            return ToolResult.Failed($"Language '{language}' is not supported or interpreter not found");
        }

        try
        {
            return RunCodeWithInterpreter(interpreterPath, language, code, stdin, timeout);
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Code execution failed: {ex.Message}");
        }
    }

    private ToolResult ExecuteListLanguages(Guid callerId, Dictionary<string, object> parameters)
    {
        var supportedLanguages = new Dictionary<string, string>();

        // Check which interpreters are available on the system
        var interpreters = new Dictionary<string, string[]>
        {
            ["python"] = new[] { "python", "python3" },
            ["javascript"] = new[] { "node", "nodejs" },
            ["bash"] = new[] { "bash", "sh" },
            ["powershell"] = new[] { "powershell", "pwsh" },
            ["ruby"] = new[] { "ruby" },
            ["perl"] = new[] { "perl" },
            ["php"] = new[] { "php" },
            ["java"] = new[] { "java" }
        };

        foreach (var kvp in interpreters)
        {
            foreach (var cmd in kvp.Value)
            {
                if (IsCommandAvailable(cmd))
                {
                    string version = GetCommandVersion(cmd);
                    supportedLanguages[kvp.Key] = $"{cmd} ({version})";
                    break;
                }
            }
        }

        if (supportedLanguages.Count == 0)
        {
            return ToolResult.Successful("No supported programming language interpreters found on this system.");
        }

        var result = new StringBuilder("Supported programming languages:\n");
        foreach (var kvp in supportedLanguages)
        {
            result.AppendLine($"- {kvp.Key}: {kvp.Value}");
        }

        return ToolResult.Successful(result.ToString().TrimEnd());
    }

    private ToolResult ExecuteInstallPackage(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("language", out object? langObj) || string.IsNullOrWhiteSpace(langObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'language' parameter for install_package action");
        }

        if (!parameters.TryGetValue("package", out object? packageObj) || string.IsNullOrWhiteSpace(packageObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'package' parameter for install_package action");
        }

        string language = langObj.ToString()!.ToLowerInvariant();
        string package = packageObj.ToString()!;
        string? version = null;

        if (parameters.TryGetValue("version", out object? versionObj) && versionObj != null)
        {
            version = versionObj.ToString();
        }

        // Get package manager command based on language
        string? packageManagerCmd = GetPackageManagerCommand(language);
        if (string.IsNullOrEmpty(packageManagerCmd))
        {
            return ToolResult.Failed($"Package installation is not supported for language '{language}'");
        }

        string packageSpec = string.IsNullOrEmpty(version) ? package : $"{package}=={version}";
        string fullCommand = $"{packageManagerCmd} {packageSpec}";

        try
        {
            return RunCommandWithTimeout(fullCommand, 60); // 60 seconds timeout for package installation
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Package installation failed: {ex.Message}");
        }
    }

    private ToolResult RunCodeWithInterpreter(string interpreterPath, string language, string code, string? stdin, int timeoutSeconds)
    {
        // Create a temporary file with the code
        string tempFile = Path.Combine(Path.GetTempPath(), $"siliconlife_{Guid.NewGuid()}.{GetFileExtension(language)}");
        File.WriteAllText(tempFile, code);

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = interpreterPath,
                Arguments = $"\"{tempFile}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = !string.IsNullOrEmpty(stdin),
                CreateNoWindow = true,
                WorkingDirectory = Directory.GetCurrentDirectory()
            };

            // Set UTF-8 encoding for output
            startInfo.StandardOutputEncoding = Encoding.UTF8;
            startInfo.StandardErrorEncoding = Encoding.UTF8;

            using var process = Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start process");

            // Write stdin if provided
            if (!string.IsNullOrEmpty(stdin))
            {
                process.StandardInput.Write(stdin);
                process.StandardInput.Close();
            }

            // Wait for process to complete with timeout
            bool exited = process.WaitForExit(timeoutSeconds * 1000);
            if (!exited)
            {
                process.Kill();
                return ToolResult.Failed($"Code execution timed out after {timeoutSeconds} seconds");
            }

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            int exitCode = process.ExitCode;

            var result = new StringBuilder();
            if (!string.IsNullOrEmpty(output))
            {
                result.AppendLine("STDOUT:");
                result.AppendLine(output.TrimEnd());
            }

            if (!string.IsNullOrEmpty(error))
            {
                result.AppendLine("STDERR:");
                result.AppendLine(error.TrimEnd());
            }

            result.AppendLine($"Exit code: {exitCode}");

            if (exitCode != 0 && string.IsNullOrEmpty(error))
            {
                result.AppendLine("Execution failed with non-zero exit code");
            }

            return ToolResult.Successful(result.ToString().TrimEnd());
        }
        finally
        {
            // Clean up temporary file
            if (File.Exists(tempFile))
            {
                try { File.Delete(tempFile); } catch { }
            }
        }
    }

    private ToolResult RunCommandWithTimeout(string command, int timeoutSeconds)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "cmd.exe" : "/bin/bash",
            Arguments = Environment.OSVersion.Platform == PlatformID.Win32NT ? $"/c \"{command}\"" : $"-c \"{command}\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WorkingDirectory = Directory.GetCurrentDirectory()
        };

        startInfo.StandardOutputEncoding = Encoding.UTF8;
        startInfo.StandardErrorEncoding = Encoding.UTF8;

        using var process = Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start process");

        bool exited = process.WaitForExit(timeoutSeconds * 1000);
        if (!exited)
        {
            process.Kill();
            return ToolResult.Failed($"Command execution timed out after {timeoutSeconds} seconds");
        }

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        int exitCode = process.ExitCode;

        var result = new StringBuilder();
        if (!string.IsNullOrEmpty(output))
        {
            result.AppendLine("STDOUT:");
            result.AppendLine(output.TrimEnd());
        }

        if (!string.IsNullOrEmpty(error))
        {
            result.AppendLine("STDERR:");
            result.AppendLine(error.TrimEnd());
        }

        result.AppendLine($"Exit code: {exitCode}");

        return ToolResult.Successful(result.ToString().TrimEnd());
    }

    private string? GetInterpreterPath(string language)
    {
        return language.ToLowerInvariant() switch
        {
            "python" => FindCommandPath(new[] { "python", "python3" }),
            "javascript" or "js" or "node" => FindCommandPath(new[] { "node", "nodejs" }),
            "bash" => FindCommandPath(new[] { "bash", "sh" }),
            "powershell" or "ps" or "ps1" => FindCommandPath(new[] { "powershell", "pwsh" }),
            "ruby" => FindCommandPath(new[] { "ruby" }),
            "perl" => FindCommandPath(new[] { "perl" }),
            "php" => FindCommandPath(new[] { "php" }),
            "java" => FindCommandPath(new[] { "java" }),
            _ => null
        };
    }

    private string? GetPackageManagerCommand(string language)
    {
        return language.ToLowerInvariant() switch
        {
            "python" => IsCommandAvailable("pip") ? "pip install" : (IsCommandAvailable("pip3") ? "pip3 install" : null),
            "javascript" or "js" or "node" => IsCommandAvailable("npm") ? "npm install" : (IsCommandAvailable("yarn") ? "yarn add" : null),
            "ruby" => IsCommandAvailable("gem") ? "gem install" : null,
            "php" => IsCommandAvailable("composer") ? "composer require" : null,
            _ => null
        };
    }

    private string GetFileExtension(string language)
    {
        return language.ToLowerInvariant() switch
        {
            "python" => "py",
            "javascript" or "js" or "node" => "js",
            "bash" => "sh",
            "powershell" or "ps" or "ps1" => "ps1",
            "ruby" => "rb",
            "perl" => "pl",
            "php" => "php",
            "java" => "java",
            _ => "txt"
        };
    }

    private string? FindCommandPath(string[] commands)
    {
        foreach (var cmd in commands)
        {
            if (IsCommandAvailable(cmd))
            {
                return cmd;
            }
        }
        return null;
    }

    private bool IsCommandAvailable(string command)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "where" : "which",
                Arguments = command,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo);
            process?.WaitForExit(5000);
            return process?.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    private string GetCommandVersion(string command)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = "--version",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo);
            if (process != null)
            {
                process.WaitForExit(5000);
                string output = process.StandardOutput.ReadToEnd().Trim();
                if (string.IsNullOrEmpty(output))
                {
                    output = process.StandardError.ReadToEnd().Trim();
                }
                return string.IsNullOrEmpty(output) ? "unknown version" : output;
            }
        }
        catch
        {
            // Ignore errors
        }
        return "unknown version";
    }
}
