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

public class DynamicCompileTool : ITool
{
    private readonly DynamicBeingLoader _loader = new();

    public string Name => "dynamic_compile";

    public string Description =>
        "Dynamic code compilation tool for silicon beings. " +
        "Actions: 'compile' (validate code without saving), " +
        "'save' (compile and save encrypted code), " +
        "'self_replace' (compile and immediately replace yourself), " +
        "'activate' (activate saved code), " +
        "'preview_saved' (view your saved code), " +
        "'clear_saved' (delete saved code).";

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
                    ["description"] = "The compilation action to perform",
                    ["enum"] = new[] { "compile", "save", "self_replace", "activate", "preview_saved", "clear_saved" }
                },
                ["code"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "C# source code to compile (for compile and save actions)"
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

        string action = actionObj?.ToString()?.ToLowerInvariant() ?? "";

        return action switch
        {
            "compile" => ExecuteCompile(callerId, parameters),
            "save" => ExecuteSave(callerId, parameters),
            "self_replace" => ExecuteSelfReplace(callerId, parameters),
            "activate" => ExecuteActivate(callerId),
            "preview_saved" => ExecutePreviewSaved(callerId),
            "clear_saved" => ExecuteClearSaved(callerId),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    private ToolResult ExecuteCompile(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("code", out object? codeObj) ||
            string.IsNullOrWhiteSpace(codeObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'code' parameter — provide the C# source code to compile.");
        }

        string sourceCode = codeObj.ToString()!;

        CompilationResult result = _loader.CompileBeing(sourceCode, callerId);
        if (!result.Success)
        {
            string errors = result.Errors.Count > 0
                ? "\nErrors:\n  " + string.Join("\n  ", result.Errors)
                : "";
            return ToolResult.Failed($"Compilation failed.{errors}");
        }

        return ToolResult.Successful(
            $"Compilation successful.\n" +
            $"Security scan: {(result.SecurityResult?.Passed == true ? "PASSED" : "FAILED")}\n" +
            $"Compiled type: {result.CompiledType?.FullName}");
    }

    private ToolResult ExecuteSave(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("code", out object? codeObj) ||
            string.IsNullOrWhiteSpace(codeObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'code' parameter — provide the C# source code to compile and save.");
        }

        string sourceCode = codeObj.ToString()!;

        CompilationResult compileResult = _loader.CompileBeing(sourceCode, callerId);
        if (!compileResult.Success)
        {
            string errors = compileResult.Errors.Count > 0
                ? "\nErrors:\n  " + string.Join("\n  ", compileResult.Errors)
                : "";
            return ToolResult.Failed($"Compilation failed.{errors}");
        }

        string beingDirectory = GetBeingDirectory(callerId);
        bool saved = _loader.SaveBeingCode(callerId, beingDirectory, sourceCode);
        if (!saved)
        {
            return ToolResult.Failed(
                "Failed to save code: Security scan rejected the code or file write failed.");
        }

        return ToolResult.Successful(
            $"Code saved successfully for being {callerId:N}.\n" +
            "Use action='self_replace' to compile and activate your new code.");
    }

    private ToolResult ExecuteSelfReplace(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("code", out object? codeObj) ||
            string.IsNullOrWhiteSpace(codeObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'code' parameter — provide the C# source code to compile and replace yourself.");
        }

        string sourceCode = codeObj.ToString()!;

        CompilationResult compileResult = _loader.CompileBeing(sourceCode, callerId);
        if (!compileResult.Success)
        {
            string errors = compileResult.Errors.Count > 0
                ? "\nErrors:\n  " + string.Join("\n  ", compileResult.Errors)
                : "";
            return ToolResult.Failed($"Compilation failed.{errors}");
        }

        string beingDirectory = GetBeingDirectory(callerId);
        bool saved = _loader.SaveBeingCode(callerId, beingDirectory, sourceCode);
        if (!saved)
        {
            return ToolResult.Failed(
                "Failed to save code: Security scan rejected the code or file write failed.");
        }

        SiliconBeingManager? beingManager = MainLoop.BeingManager;
        if (beingManager == null)
        {
            return ToolResult.Failed("SiliconBeingManager is not available.");
        }

        bool replaced = beingManager.ReplaceBeing(callerId, compileResult.CompiledType!);
        if (!replaced)
        {
            return ToolResult.Failed("Failed to replace yourself. You may not be registered.");
        }

        return ToolResult.Successful($"You have been recompiled and replaced successfully.");
    }

    private ToolResult ExecuteActivate(Guid callerId)
    {
        string beingDirectory = GetBeingDirectory(callerId);
        string codePath = Path.Combine(beingDirectory, "code.enc");

        if (!File.Exists(codePath))
        {
            return ToolResult.Failed(
                "No saved code found. Use action='save' to compile and save your code first.");
        }

        Type? compiledType;
        try
        {
            compiledType = _loader.LoadBeingType(callerId, beingDirectory);
            if (compiledType == null)
            {
                return ToolResult.Failed(
                    "Saved code exists but failed to load. It may be corrupted.");
            }
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to load saved code: {ex.Message}");
        }

        SiliconBeingManager? beingManager = MainLoop.BeingManager;
        if (beingManager == null)
        {
            return ToolResult.Failed("SiliconBeingManager is not available.");
        }

        bool replaced = beingManager.ReplaceBeing(callerId, compiledType);
        if (!replaced)
        {
            return ToolResult.Failed("Failed to activate saved code. You may not be registered.");
        }

        return ToolResult.Successful($"Saved code has been activated successfully.");
    }

    private ToolResult ExecutePreviewSaved(Guid callerId)
    {
        string beingDirectory = GetBeingDirectory(callerId);
        string codePath = Path.Combine(beingDirectory, "code.enc");

        if (!File.Exists(codePath))
        {
            return ToolResult.Successful("No saved code found. Use action='save' to compile and save your code.");
        }

        byte[] encryptedCode = File.ReadAllBytes(codePath);

        if (!CodeEncryption.TryDecryptToString(encryptedCode, callerId, out string? sourceCode))
        {
            return ToolResult.Failed("Failed to decrypt saved code.");
        }

        if (sourceCode != null && sourceCode.Length > 8000)
        {
            sourceCode = sourceCode.Substring(0, 8000) + "\n... (truncated, total length: " + sourceCode.Length + " characters)";
        }

        return ToolResult.Successful($"Saved code for being {callerId:N}:\n\n{sourceCode}");
    }

    private ToolResult ExecuteClearSaved(Guid callerId)
    {
        string beingDirectory = GetBeingDirectory(callerId);
        DynamicBeingLoader.DeleteCustomCode(beingDirectory);

        return ToolResult.Successful($"Saved code for being {callerId:N} has been cleared.");
    }

    private static string GetBeingDirectory(Guid beingId)
    {
        string dataDirectory = Config.Instance?.Data?.DataDirectory?.FullName
            ?? Path.Combine(Environment.CurrentDirectory, "data");
        return Path.Combine(dataDirectory, "SiliconManager", beingId.ToString());
    }
}
