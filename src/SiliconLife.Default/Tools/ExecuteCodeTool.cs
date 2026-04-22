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

using System.Reflection;
using System.Text;
using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// C# code execution tool with security scanning and compilation.
/// Only available to the Silicon Curator (main administrator).
/// All code is compiled through DynamicCompilationExecutor with security checks.
/// </summary>
[SiliconManagerOnly]
public class ExecuteCodeTool : ITool
{
    private readonly DynamicCompilationExecutor _compilationExecutor;

    public ExecuteCodeTool()
    {
        _compilationExecutor = new DynamicCompilationExecutor();
    }

    public string Name => "execute_code";

    public string Description =>
        "Execute C# code scripts with security scanning and compilation. " +
        "Actions: run_script (compile and execute C# code). " +
        "WARNING: This tool compiles and executes C# code with security scanning. " +
        "Only available to trusted administrators.";

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
                    ["description"] = "Action to perform: run_script",
                    ["enum"] = new[] { "run_script" }
                },
                ["code"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "C# code to compile and execute"
                },
                ["timeout"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Execution timeout in seconds, default 30"
                }
            },
            ["required"] = new[] { "action", "code" }
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
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    private ToolResult ExecuteRunScript(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("code", out object? codeObj) || string.IsNullOrWhiteSpace(codeObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'code' parameter for run_script action");
        }

        string code = codeObj.ToString()!;
        int timeout = 30;

        if (parameters.TryGetValue("timeout", out object? timeoutObj) && timeoutObj != null)
        {
            if (int.TryParse(timeoutObj.ToString(), out int parsedTimeout))
            {
                timeout = parsedTimeout;
            }
        }

        try
        {
            return CompileAndExecuteCode(code, timeout);
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Code execution failed: {ex.Message}");
        }
    }

    private ToolResult CompileAndExecuteCode(string code, int timeoutSeconds)
    {
        // Wrap the code in a class that can be compiled
        string wrappedCode = WrapCodeInClass(code);

        // Compile the code with security scanning
        CompilationResult compileResult = _compilationExecutor.Compile<object>(wrappedCode, "ScriptExecutor");

        if (!compileResult.Success)
        {
            var errorMessages = new StringBuilder("Compilation failed:\n");
            
            // Add security scan violations if any
            if (compileResult.SecurityResult != null && !compileResult.SecurityResult.Passed)
            {
                errorMessages.AppendLine("Security violations:");
                foreach (var violation in compileResult.SecurityResult.Violations)
                {
                    errorMessages.AppendLine($"  - {violation}");
                }
            }

            // Add compilation errors
            if (compileResult.Errors.Count > 0)
            {
                errorMessages.AppendLine("\nCompilation errors:");
                foreach (var error in compileResult.Errors)
                {
                    errorMessages.AppendLine($"  - {error}");
                }
            }

            return ToolResult.Failed(errorMessages.ToString().TrimEnd());
        }

        // Execute the compiled code
        try
        {
            object? instance = CompilationCore.CreateInstance(compileResult);
            if (instance == null)
            {
                return ToolResult.Failed("Failed to create instance of compiled code");
            }

            // Find and execute the Execute method
            MethodInfo? executeMethod = instance.GetType().GetMethod("Execute");
            if (executeMethod == null)
            {
                return ToolResult.Failed("Compiled code does not have an Execute method");
            }

            // Execute with timeout
            using var cancellationTokenSource = new System.Threading.CancellationTokenSource();
            cancellationTokenSource.CancelAfter(timeoutSeconds * 1000);

            var task = System.Threading.Tasks.Task.Run(() => executeMethod.Invoke(instance, null));
            
            if (task.Wait(timeoutSeconds * 1000))
            {
                var result = task.Result;
                string output = result?.ToString() ?? "(no output)";
                return ToolResult.Successful($"Execution completed successfully:\n{output}");
            }
            else
            {
                return ToolResult.Failed($"Code execution timed out after {timeoutSeconds} seconds");
            }
        }
        catch (System.Reflection.TargetInvocationException ex)
        {
            return ToolResult.Failed($"Runtime error: {ex.InnerException?.Message ?? ex.Message}");
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Execution error: {ex.Message}");
        }
    }

    private string WrapCodeInClass(string code)
    {
        return @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ScriptExecutor
{
    public string Execute()
    {
" + code + @"
    }
}";
    }
}
