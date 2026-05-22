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

using System.Reflection;
using System.Text.Json;

namespace SiliconLife.Collective;

/// <summary>
/// Manages tools available to a silicon being.
/// Each silicon being holds its own ToolManager instance.
/// Supports reflection-based assembly scanning for tool discovery.
/// Supports ToolAction-level permission filtering based on per-being config.
/// </summary>
public class ToolManager
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<ToolManager>();
    private readonly Dictionary<string, ITool> _tools = new();
    private readonly Dictionary<string, ToolScenarioFlag> _toolScenarios = new();
    private readonly Dictionary<string, string[]> _toolActions = new();
    private readonly HashSet<string> _chatOnlyTools = new();
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

            Type toolType = tool.GetType();
            var scenarioAttr = toolType.GetCustomAttribute<ToolScenarioAttribute>();
            _toolScenarios[tool.Name] = scenarioAttr?.Scenarios ?? ToolScenarioFlag.All;

            var actionAttr = toolType.GetCustomAttribute<ToolActionAttribute>();
            if (actionAttr != null && actionAttr.Actions.Length > 0)
            {
                _toolActions[tool.Name] = actionAttr.Actions;
            }

            if (toolType.GetCustomAttribute<ChatOnlyAttribute>() != null)
            {
                _chatOnlyTools.Add(tool.Name);
            }
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
    /// Scans the assemblies of all currently-loaded plugins (resolved via
    /// <see cref="ServiceLocator"/> &#8594; <see cref="PluginLoader"/>) for ITool implementations
    /// and registers them. Honors the same <see cref="SiliconManagerOnlyAttribute"/> filtering
    /// rules as <see cref="ScanAssembly"/>.
    /// <para>Returns 0 if no <see cref="PluginLoader"/> is registered or no plugins are loaded.</para>
    /// </summary>
    /// <returns>The total number of tools discovered and registered across all plugin assemblies</returns>
    public int ScanAllPluginAssemblies()
    {
        PluginLoader? loader = ServiceLocator.Instance.GetService<PluginLoader>();
        if (loader == null)
        {
            _logger.Debug(null, "PluginLoader not registered; skipping plugin tool scan");
            return 0;
        }

        int total = 0;
        var plugins = loader.Plugins;
        foreach (IPlugin plugin in plugins)
        {
            try
            {
                total += ScanAssembly(plugin.GetType().Assembly);
            }
            catch (Exception ex)
            {
                _logger.Warn(null, $"Failed to scan plugin assembly for '{plugin.Id}': {ex.Message}");
            }
        }

        _logger.Info(null, $"Plugin assemblies scan: registered {total} tool(s) from {plugins.Count} plugin(s)");
        return total;
    }

    /// <summary>
    /// Scans the assemblies of all currently-loaded plugins for ALL ITool implementations
    /// regardless of <see cref="SiliconManagerOnlyAttribute"/>. Used by curator beings.
    /// <para>Returns 0 if no <see cref="PluginLoader"/> is registered or no plugins are loaded.</para>
    /// </summary>
    /// <returns>The total number of tools discovered and registered across all plugin assemblies</returns>
    public int ScanAllPluginAssembliesAll()
    {
        PluginLoader? loader = ServiceLocator.Instance.GetService<PluginLoader>();
        if (loader == null)
        {
            _logger.Debug(null, "PluginLoader not registered; skipping plugin tool scan");
            return 0;
        }

        int total = 0;
        var plugins = loader.Plugins;
        foreach (IPlugin plugin in plugins)
        {
            try
            {
                total += ScanAssemblyAll(plugin.GetType().Assembly);
            }
            catch (Exception ex)
            {
                _logger.Warn(null, $"Failed to scan plugin assembly for '{plugin.Id}': {ex.Message}");
            }
        }

        _logger.Info(null, $"Plugin assemblies scan (all): registered {total} tool(s) from {plugins.Count} plugin(s)");
        return total;
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
    /// Gets tool definitions for specific tools by name (for task-specific AI requests)
    /// </summary>
    /// <param name="requiredToolNames">List of tool names to get definitions for</param>
    /// <returns>List of tool definitions for the specified tools</returns>
    public List<ToolDefinition> GetToolDefinitions(List<string> requiredToolNames)
    {
        if (requiredToolNames == null || requiredToolNames.Count == 0)
        {
            return GetToolDefinitions();
        }

        lock (_lock)
        {
            var definitions = new List<ToolDefinition>();
            foreach (var toolName in requiredToolNames)
            {
                if (_tools.TryGetValue(toolName, out ITool? tool))
                {
                    definitions.Add(new ToolDefinition(
                        tool.Name,
                        tool.Description,
                        tool.GetParameterSchema()
                    ));
                }
                else
                {
                    _logger?.Warn(null, "Required tool '{0}' not found in tool manager", toolName);
                }
            }
            return definitions;
        }
    }

    public List<ToolDefinition> GetToolDefinitions(ToolScenarioFlag scenario)
    {
        lock (_lock)
        {
            var definitions = new List<ToolDefinition>();
            foreach (var kvp in _tools)
            {
                if ((_toolScenarios.TryGetValue(kvp.Key, out var flags) && (flags & scenario) != 0) ||
                    !_toolScenarios.ContainsKey(kvp.Key))
                {
                    definitions.Add(new ToolDefinition(
                        kvp.Value.Name,
                        kvp.Value.Description,
                        kvp.Value.GetParameterSchema()
                    ));
                }
            }
            return definitions;
        }
    }

    /// <summary>
    /// Gets tool definitions for a specific scenario, filtered by the being's
    /// ToolAction permission configuration. Disabled actions are removed from
    /// the "action" parameter's enum list in the tool schema.
    /// </summary>
    /// <param name="scenario">The tool scenario flag</param>
    /// <param name="beingId">The being ID to filter actions for</param>
    /// <param name="permissions">The being's tool action permission config (null = all allowed)</param>
    /// <returns>List of tool definitions with action enums filtered</returns>
    public List<ToolDefinition> GetToolDefinitions(ToolScenarioFlag scenario, Guid beingId, ToolActionPermissionConfig? permissions)
    {
        lock (_lock)
        {
            var definitions = new List<ToolDefinition>();
            foreach (var kvp in _tools)
            {
                if ((_toolScenarios.TryGetValue(kvp.Key, out var flags) && (flags & scenario) != 0) ||
                    !_toolScenarios.ContainsKey(kvp.Key))
                {
                    var schema = kvp.Value.GetParameterSchema();
                    
                    // Filter action enum based on permissions
                    if (permissions != null && _toolActions.TryGetValue(kvp.Key, out var declaredActions))
                    {
                        schema = FilterActionEnum(schema, declaredActions, permissions, kvp.Key);
                    }
                    
                    definitions.Add(new ToolDefinition(
                        kvp.Value.Name,
                        kvp.Value.Description,
                        schema
                    ));
                }
            }
            return definitions;
        }
    }

    /// <summary>
    /// Gets tool definitions for specific tool names, filtered by the being's
    /// ToolAction permission configuration. Disabled actions are removed from
    /// the "action" parameter's enum list in the tool schema.
    /// </summary>
    /// <param name="requiredToolNames">List of tool names to get definitions for</param>
    /// <param name="beingId">The being ID to filter actions for</param>
    /// <param name="permissions">The being's tool action permission config (null = all allowed)</param>
    /// <returns>List of tool definitions with action enums filtered</returns>
    public List<ToolDefinition> GetToolDefinitions(List<string> requiredToolNames, Guid beingId, ToolActionPermissionConfig? permissions)
    {
        if (requiredToolNames == null || requiredToolNames.Count == 0)
        {
            return GetToolDefinitions(ToolScenarioFlag.All, beingId, permissions);
        }

        lock (_lock)
        {
            var definitions = new List<ToolDefinition>();
            foreach (var toolName in requiredToolNames)
            {
                if (_tools.TryGetValue(toolName, out ITool? tool))
                {
                    var schema = tool.GetParameterSchema();
                    
                    // Filter action enum based on permissions
                    if (permissions != null && _toolActions.TryGetValue(toolName, out var declaredActions))
                    {
                        schema = FilterActionEnum(schema, declaredActions, permissions, toolName);
                    }
                    
                    definitions.Add(new ToolDefinition(
                        tool.Name,
                        tool.Description,
                        schema
                    ));
                }
                else
                {
                    _logger?.Warn(null, "Required tool '{0}' not found in tool manager", toolName);
                }
            }
            return definitions;
        }
    }

    /// <summary>
    /// Filters the "action" parameter's enum list in a tool schema based on
    /// the being's permission configuration. Removes disabled actions from the enum.
    /// If all actions are disabled, the tool definition is still returned but
    /// the action enum will be empty (the AI will not be able to call it).
    /// </summary>
    private static Dictionary<string, object> FilterActionEnum(
        Dictionary<string, object> schema,
        string[] declaredActions,
        ToolActionPermissionConfig permissions,
        string toolName)
    {
        // Get the set of disabled actions for this tool
        var disabledActions = permissions.GetDisabledActions(toolName);
        if (disabledActions.Count == 0)
        {
            return schema; // No filtering needed
        }

        // Deep clone the schema to avoid modifying the original
        var filteredSchema = new Dictionary<string, object>(schema);
        
        if (filteredSchema.TryGetValue("properties", out var propsObj) &&
            propsObj is Dictionary<string, object> properties &&
            properties.TryGetValue("action", out var actionObj) &&
            actionObj is Dictionary<string, object> actionDef)
        {
            // Clone the action definition
            var filteredActionDef = new Dictionary<string, object>(actionDef);
            
            if (filteredActionDef.TryGetValue("enum", out var enumObj) && enumObj is object[] enumValues)
            {
                // Filter out disabled actions from the enum
                var filteredEnum = enumValues
                    .Where(v => !disabledActions.Contains(v?.ToString() ?? ""))
                    .ToArray();
                
                filteredActionDef["enum"] = filteredEnum;
                var filteredProperties = new Dictionary<string, object>(properties);
                filteredProperties["action"] = filteredActionDef;
                filteredSchema["properties"] = filteredProperties;
            }
        }

        return filteredSchema;
    }

    /// <summary>
    /// Checks whether a specific action on a tool is allowed for a given being.
    /// Used for runtime validation in ExecuteTool.
    /// </summary>
    /// <param name="toolName">The tool name</param>
    /// <param name="actionName">The action being invoked</param>
    /// <param name="permissions">The being's tool action permission config (null = all allowed)</param>
    /// <returns>True if the action is allowed, false if denied</returns>
    public bool IsActionAllowed(string toolName, string actionName, ToolActionPermissionConfig? permissions)
    {
        if (permissions == null) return true;
        if (!_toolActions.ContainsKey(toolName)) return true; // No ToolActionAttribute = no restriction
        return permissions.IsActionAllowed(toolName, actionName);
    }

    /// <summary>
    /// Gets the declared actions for a tool (from ToolActionAttribute).
    /// Returns null if the tool has no ToolActionAttribute.
    /// </summary>
    /// <param name="toolName">The tool name</param>
    /// <returns>Array of action names, or null if not declared</returns>
    public string[]? GetDeclaredActions(string toolName)
    {
        lock (_lock)
        {
            return _toolActions.TryGetValue(toolName, out var actions) ? actions : null;
        }
    }

    /// <summary>
    /// Gets all tool names that have ToolActionAttribute declarations.
    /// </summary>
    /// <returns>Dictionary of tool name → declared action names</returns>
    public Dictionary<string, string[]> GetAllDeclaredActions()
    {
        lock (_lock)
        {
            return new Dictionary<string, string[]>(_toolActions);
        }
    }

    public bool IsChatOnlyTool(string toolName)
    {
        lock (_lock)
        {
            return _chatOnlyTools.Contains(toolName);
        }
    }

    /// <summary>
    /// Executes a tool by name with the given parameters.
    /// Performs Action-level permission check if the tool has ToolActionAttribute
    /// and the being has a ToolActionPermissionConfig.
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

        // Action-level permission check
        var convertedParams = ConvertParameters(parameters ?? new Dictionary<string, object>());
        if (_toolActions.ContainsKey(name) && being?.ToolActionPermissions != null)
        {
            if (convertedParams.TryGetValue("action", out var actionObj) && actionObj != null)
            {
                string actionName = actionObj.ToString() ?? "";
                if (!string.IsNullOrEmpty(actionName) && !IsActionAllowed(name, actionName, being.ToolActionPermissions))
                {
                    _logger.Warn(callerId, $"Action '{actionName}' on tool '{name}' denied for being {being.Name}");
                    return ToolResult.Failed($"Action '{actionName}' on tool '{name}' is not allowed for this being");
                }
            }
        }

        _logger.Info(null, $"Tool execution: {name}, caller={callerId}");
        try
        {
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
