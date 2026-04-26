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
using System.Text.Json;

namespace SiliconLife.Collective;

/// <summary>
/// Manages tools available to a silicon being.
/// Each silicon being holds its own ToolManager instance.
/// Supports reflection-based assembly scanning for tool discovery.
/// </summary>
public class ToolManager
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<ToolManager>();
    private readonly Dictionary<string, ITool> _tools = new();
    private readonly object _lock = new();
    private readonly bool _curatorOnly;

    /// <summary>
    /// Gets the number of registered tools
    /// </summary>
    public int ToolCount
    {
        get
        {
            lock (_lock)
            {
                return _tools.Count;
            }
        }
    }

    /// <summary>
    /// Initializes a new ToolManager
    /// </summary>
    /// <param name="curatorOnly">If true, only tools with [SiliconManagerOnly] are registered during scanning</param>
    public ToolManager(bool curatorOnly = false)
    {
        _curatorOnly = curatorOnly;
    }

    /// <summary>
    /// Registers a tool instance
    /// </summary>
    /// <param name="tool">The tool to register</param>
    public void RegisterTool(ITool tool)
    {
        lock (_lock)
        {
            _tools[tool.Name] = tool;
        }
        _logger.Debug(null, $"Tool registered: {tool.Name}");
    }

    /// <summary>
    /// Scans the specified assembly for ITool implementations and registers them.
    /// Only non-abstract types with parameterless constructors are discovered.
    /// When curatorOnly=true, only tools with [SiliconManagerOnly] are registered.
    /// When curatorOnly=false, only tools without [SiliconManagerOnly] are registered.
    /// </summary>
    /// <param name="assembly">The assembly to scan</param>
    /// <returns>The number of tools discovered and registered</returns>
    public int ScanAssembly(Assembly assembly)
    {
        int count = 0;

        foreach (Type type in assembly.GetTypes())
        {
            if (!typeof(ITool).IsAssignableFrom(type) || type.IsAbstract || type.IsInterface)
            {
                continue;
            }

            bool hasManagerOnlyAttr = type.GetCustomAttribute<SiliconManagerOnlyAttribute>() != null;

            if (_curatorOnly && !hasManagerOnlyAttr)
            {
                continue;
            }

            if (!_curatorOnly && hasManagerOnlyAttr)
            {
                continue;
            }

            try
            {
                ITool? tool = Activator.CreateInstance(type) as ITool;
                if (tool != null)
                {
                    RegisterTool(tool);
                    count++;
                }
            }
            catch (Exception ex)
            {
                _logger.Warn(null, $"Failed to instantiate tool '{type.Name}': {ex.Message}");
            }
        }

        _logger.Info(null, $"Assembly scan: found {count} tools from {assembly.GetName().Name}");
        return count;
    }

    /// <summary>
    /// Scans the specified assembly for ALL ITool implementations regardless of
    /// [SiliconManagerOnly] attribute. Used by curator beings that have access
    /// to every tool (both normal and curator-only).
    /// </summary>
    /// <param name="assembly">The assembly to scan</param>
    /// <returns>The number of tools discovered and registered</returns>
    public int ScanAssemblyAll(Assembly assembly)
    {
        int count = 0;

        foreach (Type type in assembly.GetTypes())
        {
            if (!typeof(ITool).IsAssignableFrom(type) || type.IsAbstract || type.IsInterface)
            {
                continue;
            }

            try
            {
                ITool? tool = Activator.CreateInstance(type) as ITool;
                if (tool != null)
                {
                    RegisterTool(tool);
                    count++;
                }
            }
            catch (Exception ex)
            {
                _logger.Warn(null, $"Failed to instantiate tool '{type.Name}': {ex.Message}");
            }
        }

        _logger.Info(null, $"Assembly scan (all): found {count} tools from {assembly.GetName().Name}");
        return count;
    }

    /// <summary>
    /// Gets tool definitions for all registered tools (for AI request)
    /// </summary>
    /// <returns>List of tool definitions</returns>
    public List<ToolDefinition> GetToolDefinitions()
    {
        lock (_lock)
        {
            return _tools.Values.Select(t => new ToolDefinition(
                t.Name,
                t.Description,
                t.GetParameterSchema()
            )).ToList();
        }
    }

    /// <summary>
    /// Executes a tool by name with the given parameters
    /// </summary>
    /// <param name="name">The tool name</param>
    /// <param name="parameters">The parameters for the tool</param>
    /// <param name="being">The silicon being instance (callerId will be obtained from being.Id)</param>
    /// <returns>The tool execution result</returns>
    public ToolResult ExecuteTool(string name, Dictionary<string, object>? parameters = null, SiliconBeingBase? being = null)
    {
        Guid callerId = being?.Id ?? Guid.Empty;
        
        ITool? tool;
        lock (_lock)
        {
            _tools.TryGetValue(name, out tool);
        }

        if (tool == null)
        {
            _logger.Warn(null, $"Tool not found: {name}");
            return ToolResult.Failed($"Tool '{name}' not found");
        }

        _logger.Info(null, $"Tool execution: {name}, caller={callerId}");
        try
        {
            var convertedParams = ConvertParameters(parameters ?? new Dictionary<string, object>());
            ToolResult result = tool.Execute(callerId, convertedParams);
            
            // Record tool execution to memory
            RecordToolExecutionToMemory(being, name, result);
            
            _logger.Debug(null, $"Tool execution succeeded: {name}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error(null, $"Tool execution failed: {name}, error={ex.Message}", ex);
            
            // Record tool execution failure to memory
            RecordToolExecutionErrorToMemory(being, name, ex.Message);
            
            return ToolResult.Failed($"Tool '{name}' execution failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets a registered tool by name
    /// </summary>
    /// <param name="name">The tool name</param>
    /// <returns>The tool, or null if not found</returns>
    public ITool? GetTool(string name)
    {
        lock (_lock)
        {
            _tools.TryGetValue(name, out ITool? tool);
            return tool;
        }
    }

    /// <summary>
    /// Checks if a tool with the given name is registered
    /// </summary>
    /// <param name="name">The tool name</param>
    /// <returns>True if the tool is registered</returns>
    public bool HasTool(string name)
    {
        lock (_lock)
        {
            return _tools.ContainsKey(name);
        }
    }

    /// <summary>
    /// Gets the names of all registered tools
    /// </summary>
    /// <returns>List of tool names</returns>
    public List<string> GetToolNames()
    {
        lock (_lock)
        {
            return _tools.Keys.ToList();
        }
    }

    /// <summary>
    /// Converts all JsonElement values in the parameters dictionary to native .NET types.
    /// System.Text.Json deserializes Dictionary&lt;string, object&gt; with JsonElement values,
    /// but tools expect native types (string, int, bool, etc.).
    /// </summary>
    private static Dictionary<string, object> ConvertParameters(Dictionary<string, object> parameters)
    {
        var result = new Dictionary<string, object>();
        foreach (var kvp in parameters)
        {
            var converted = ConvertJsonValue(kvp.Value);
            if (converted != null)
                result[kvp.Key] = converted;
        }
        return result;
    }

    /// <summary>
    /// Recursively converts a JsonElement to its native .NET type.
    /// </summary>
    private static object? ConvertJsonValue(object? value)
    {
        if (value is JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => TryGetNumber(element),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                JsonValueKind.Array => element.EnumerateArray().Select(e => ConvertJsonValue(e)).ToList(),
                JsonValueKind.Object => element.EnumerateObject()
                    .Where(p => ConvertJsonValue(p.Value) != null)
                    .ToDictionary(p => p.Name, p => ConvertJsonValue(p.Value)!),
                _ => value
            };
        }
        return value;
    }

    /// <summary>
    /// Tries to convert a numeric JsonElement to the most appropriate .NET numeric type.
    /// Priority: int → long → double
    /// </summary>
    private static object TryGetNumber(JsonElement element)
    {
        if (element.TryGetInt32(out int intVal))
            return intVal;
        if (element.TryGetInt64(out long longVal))
            return longVal;
        return element.GetDouble();
    }

    /// <summary>
    /// Records tool execution to the being's memory.
    /// </summary>
    private void RecordToolExecutionToMemory(SiliconBeingBase? being, string toolName, ToolResult result)
    {
        if (being?.Memory == null)
        {
            return;
        }

        try
        {
            string status = result.Success ? "成功" : "失败";
            string content = $"[工具执行] {toolName} - {status}";
            
            // Add brief result info if successful
            if (result.Success && !string.IsNullOrEmpty(result.Message))
            {
                string preview = result.Message.Length > 100 
                    ? result.Message.Substring(0, 100) 
                    : result.Message;
                content += $": {preview}";
            }
            
            being.Memory.Add(content, null);
        }
        catch (Exception ex)
        {
            _logger.Warn(being.Id, $"Failed to record tool execution to memory: {ex.Message}");
        }
    }

    /// <summary>
    /// Records tool execution error to the being's memory.
    /// </summary>
    private void RecordToolExecutionErrorToMemory(SiliconBeingBase? being, string toolName, string errorMessage)
    {
        if (being?.Memory == null)
        {
            return;
        }

        try
        {
            string content = $"[工具错误] {toolName} 执行失败: {errorMessage}";
            being.Memory.Add(content, null);
        }
        catch (Exception ex)
        {
            _logger.Warn(being.Id, $"Failed to record tool error to memory: {ex.Message}");
        }
    }
}
