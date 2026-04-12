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
/// Curator management tool. Only available to the Silicon Curator (main administrator).
/// Provides operations for managing silicon beings: list, inspect, compile, reset.
/// The curator is a normal silicon being with IsCurator=true and access to all tools
/// (including this one). All management logic is handled directly by this tool.
/// </summary>
[SiliconManagerOnly]
public class CuratorTool : ITool
{
    public string Name => "silicon_manager";

    public string Description =>
        "Manage silicon beings. Actions: 'list_beings' (list all beings with status), " +
        "'get_code' (view a being's custom source code), " +
        "'reset' (revert to default implementation).";

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
                    ["description"] = "The management action to perform",
                    ["enum"] = new[] { "list_beings", "get_code", "reset" }
                },
                ["being_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The GUID of the target silicon being (required for most actions)"
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

        // list_beings does not require being_id
        if (action == "list_beings")
        {
            return ExecuteListBeings(callerId);
        }

        // All other actions require being_id
        if (!parameters.TryGetValue("being_id", out object? beingIdObj) ||
            string.IsNullOrWhiteSpace(beingIdObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'being_id' parameter");
        }

        if (!Guid.TryParse(beingIdObj.ToString(), out Guid beingId))
        {
            return ToolResult.Failed($"Invalid being_id: '{beingIdObj}' — must be a valid GUID");
        }

        return action switch
        {
            "get_code" => ExecuteGetCode(beingId),
            "reset" => ExecuteReset(beingId),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    private ToolResult ExecuteListBeings(Guid callerId)
    {
        SiliconBeingManager? beingManager = MainLoop.BeingManager;
        if (beingManager == null)
        {
            return ToolResult.Failed("SiliconBeingManager is not available.");
        }

        List<SiliconBeingBase> beings = beingManager.GetAllBeings();
        if (beings.Count == 0)
        {
            return ToolResult.Successful("No silicon beings registered.");
        }

        var lines = new List<string> { $"Found {beings.Count} silicon being(s):", "" };

        foreach (SiliconBeingBase being in beings)
        {
            string status = being.IsCurator ? "[CURATOR]" : "[BEING]";
            string compiled = being.IsCustomCompiled ? $" (custom: {being.CustomTypeName})" : " (default)";
            string idle = being.IsIdle ? "idle" : "busy";

            lines.Add($"  {status} {being.Id:N} — {being.Name}{compiled} — {idle}");
        }

        return ToolResult.Successful(string.Join("\n", lines));
    }

    private ToolResult ExecuteGetCode(Guid beingId)
    {
        string beingDirectory = GetBeingDirectory(beingId);
        string codePath = Path.Combine(beingDirectory, "code.enc");

        if (!File.Exists(codePath))
        {
            return ToolResult.Successful($"Being {beingId:N} is using the default implementation (no custom code).");
        }

        byte[] encryptedCode = File.ReadAllBytes(codePath);

        if (!CodeEncryption.TryDecryptToString(encryptedCode, beingId, out string? sourceCode))
        {
            return ToolResult.Failed($"Failed to decrypt code for being {beingId:N}.");
        }

        if (sourceCode != null && sourceCode.Length > 8000)
        {
            sourceCode = sourceCode.Substring(0, 8000) + "\n... (truncated, total length: " + sourceCode.Length + " characters)";
        }

        return ToolResult.Successful($"Custom code for being {beingId:N}:\n\n{sourceCode}");
    }

    private ToolResult ExecuteReset(Guid beingId)
    {
        string beingDirectory = GetBeingDirectory(beingId);
        DynamicBeingLoader.DeleteCustomCode(beingDirectory);

        SiliconBeingManager? beingManager = MainLoop.BeingManager;
        if (beingManager != null)
        {
            beingManager.ReplaceWithDefault(beingId);
        }

        return ToolResult.Successful($"Being {beingId:N} has been reset to the default implementation.");
    }

    /// <summary>
    /// Gets the data directory for a silicon being.
    /// </summary>
    private static string GetBeingDirectory(Guid beingId)
    {
        string dataDirectory = Config.Instance?.Data?.DataDirectory?.FullName
            ?? Path.Combine(Environment.CurrentDirectory, "data");
        return Path.Combine(dataDirectory, "SiliconManager", beingId.ToString());
    }
}
