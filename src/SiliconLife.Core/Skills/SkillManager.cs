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
/// Manages skills available to a silicon being.
/// Each silicon being holds its own SkillManager instance.
/// Handles registration, lookup, permission filtering and execution.
/// Design mirrors <see cref="ToolManager"/>.
/// </summary>
public class SkillManager
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<SkillManager>();

    private readonly Dictionary<string, SkillDefinition> _skills = new();
    private readonly HashSet<string> _executingSkills = new();
    private readonly Dictionary<string, AutoSkillTickObject> _autoSkillTickObjects = new();
    private readonly object _lock = new();

    // Hot-reload state
    private string _skillsFingerprint = string.Empty;
    private DateTime _lastRefreshTime = DateTime.MinValue;
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(30);

    /// <summary>Gets the number of registered skills.</summary>
    public int SkillCount
    {
        get
        {
            lock (_lock)
            {
                return _skills.Count;
            }
        }
    }

    /// <summary>Gets the number of being/user-created skills (used for quota checks).</summary>
    public int CustomSkillCount
    {
        get
        {
            lock (_lock)
            {
                return _skills.Values.Count(s => s.Source is SkillSource.Being or SkillSource.User);
            }
        }
    }

    /// <summary>Gets the maximum number of custom skills per being (from config, default 50).</summary>
    public static int MaxCustomSkills => Config.Instance?.Data?.MaxCustomSkillsPerBeing ?? 50;

    /// <summary>Gets whether the skill system is globally enabled (default true).</summary>
    public static bool SkillEnabled => Config.Instance?.Data?.SkillEnabled ?? true;

    /// <summary>Registers a skill. Registering an existing id overwrites it.</summary>
    public void RegisterSkill(SkillDefinition skill)
    {
        if (skill == null || string.IsNullOrWhiteSpace(skill.Id))
        {
            _logger.Warn(null, "Cannot register skill without an id");
            return;
        }

        lock (_lock)
        {
            _skills[skill.Id] = skill;
        }
        _logger.Debug(null, "Skill registered: {0} (v{1}, source={2})", skill.Id, skill.Version, skill.Source);
    }

    /// <summary>Scans an assembly for <see cref="ISkillProvider"/> implementations and registers their skills.</summary>
    /// <returns>The number of skills discovered and registered.</returns>
    public int ScanAssembly(Assembly assembly)
    {
        int count = 0;

        foreach (Type type in assembly.GetTypes())
        {
            if (!typeof(ISkillProvider).IsAssignableFrom(type) || type.IsAbstract || type.IsInterface)
            {
                continue;
            }

            try
            {
                if (Activator.CreateInstance(type) is not ISkillProvider provider) continue;

                foreach (var skill in provider.GetSkills())
                {
                    if (skill == null || string.IsNullOrWhiteSpace(skill.Id)) continue;

                    // Plugin-provided skills are tagged with Plugin source
                    var registered = skill.Source == SkillSource.Plugin
                        ? skill
                        : new SkillDefinition
                        {
                            Id = skill.Id,
                            Description = skill.Description,
                            DisplayNameKey = skill.DisplayNameKey,
                            Version = skill.Version,
                            Tags = skill.Tags,
                            ParameterSchema = skill.ParameterSchema,
                            SystemPromptTemplate = skill.SystemPromptTemplate,
                            ToolWhitelist = skill.ToolWhitelist,
                            ToolActionRestrictions = skill.ToolActionRestrictions,
                            MaxToolRound = skill.MaxToolRound,
                            Timeout = skill.Timeout,
                            OnCompleteAction = skill.OnCompleteAction,
                            Source = SkillSource.Plugin,
                            TriggerMode = skill.TriggerMode,
                            AutoTriggerCondition = skill.AutoTriggerCondition,
                            Metadata = skill.Metadata,
                        };
                    RegisterSkill(registered);
                    count++;
                }
            }
            catch (Exception ex)
            {
                _logger.Warn(null, "Failed to instantiate ISkillProvider '{0}': {1}", type.Name, ex.Message);
            }
        }

        if (count > 0)
        {
            _logger.Info(null, "Skill assembly scan: found {0} skill(s) from {1}", count, assembly.GetName().Name);
        }
        return count;
    }

    /// <summary>
    /// Scans the assemblies of all currently-loaded plugins (resolved via
    /// <see cref="ServiceLocator"/> → <see cref="PluginLoader"/>) for
    /// <see cref="ISkillProvider"/> implementations and registers their skills.
    /// </summary>
    public int ScanAllPluginAssemblies()
    {
        PluginLoader? loader = ServiceLocator.Instance.GetService<PluginLoader>();
        if (loader == null)
        {
            return 0;
        }

        int total = 0;
        foreach (IPlugin plugin in loader.Plugins)
        {
            try
            {
                total += ScanAssembly(plugin.GetType().Assembly);
            }
            catch (Exception ex)
            {
                _logger.Warn(null, "Failed to scan plugin assembly for skills '{0}': {1}", plugin.Id, ex.Message);
            }
        }
        return total;
    }

    /// <summary>Gets a skill definition by id (null when not registered).</summary>
    public SkillDefinition? GetSkill(string id)
    {
        lock (_lock)
        {
            return _skills.TryGetValue(id, out var skill) ? skill : null;
        }
    }

    /// <summary>Gets all registered skill definitions.</summary>
    public List<SkillDefinition> GetAllSkills()
    {
        lock (_lock)
        {
            return _skills.Values.ToList();
        }
    }

    /// <summary>Gets all auto-trigger skills (schedule mode).</summary>
    public List<SkillDefinition> GetAutoSkills()
    {
        lock (_lock)
        {
            return _skills.Values
                .Where(s => s.TriggerMode == SkillTriggerMode.Auto)
                .ToList();
        }
    }

    /// <summary>
    /// Gets skill definitions converted to function-calling format
    /// (for injection into AIRequest.Tools). Skills that are disabled by the
    /// being's permission config, or whose metadata is incomplete, are filtered out.
    /// </summary>
    public List<ToolDefinition> GetSkillDefinitions(Guid beingId, ToolActionPermissionConfig? permissions)
    {
        var definitions = new List<SkillDefinition>();
        lock (_lock)
        {
            definitions = _skills.Values.ToList();
        }

        var result = new List<ToolDefinition>();
        foreach (var skill in definitions)
        {
            // Incomplete drafts are not exposed to the AI
            if (string.IsNullOrEmpty(skill.Id) || string.IsNullOrEmpty(skill.Description)) continue;
            if (SkillMarkdownParser.NeedsCompletion(skill)) continue;

            // Skill-level permission check: the skill id acts as a tool name with action "execute"
            if (permissions != null && permissions.IsActionDisabled(skill.Id, "execute"))
            {
                _logger.Debug(beingId, "Skill '{0}' skipped: disabled by tool action permissions", skill.Id);
                continue;
            }

            result.Add(new ToolDefinition(skill.Id, skill.Description, skill.ParameterSchema));
        }
        return result;
    }

    /// <summary>Checks whether a being is allowed to use a skill.</summary>
    public bool IsSkillAllowed(string skillId, SiliconBeingBase being)
    {
        var skill = GetSkill(skillId);
        if (skill == null) return false;

        // Skill-level permission: curator can always use; others follow ToolActionPermissions
        if (being.IsCurator) return true;
        return being.ToolActionPermissions?.IsActionAllowed(skillId, "execute") ?? true;
    }

    /// <summary>Unregisters a skill. Returns true when it existed.</summary>
    public bool UnregisterSkill(string id)
    {
        lock (_lock)
        {
            return _skills.Remove(id);
        }
    }

    /// <summary>
    /// Reloads skills from storage when the skills/ directory changed (hot reload).
    /// Keeps builtin/plugin skills; replaces being/user skills with the persisted versions.
    /// Rate-limited internally (checks at most once per 30 seconds).
    /// </summary>
    public void RefreshFromStorage(IStorage storage)
    {
        if (storage == null) return;

        if (DateTime.UtcNow - _lastRefreshTime < RefreshInterval) return;
        _lastRefreshTime = DateTime.UtcNow;

        try
        {
            string fingerprint = SkillFileManager.ComputeDirectoryFingerprint(storage);
            if (fingerprint == _skillsFingerprint) return;

            var skills = SkillFileManager.LoadAllSkills(storage);
            lock (_lock)
            {
                // Keep builtin and plugin skills; drop old being/user versions
                foreach (var old in _skills.Values.Where(s => s.Source is SkillSource.Being or SkillSource.User).ToList())
                {
                    _skills.Remove(old.Id);
                }
                foreach (var skill in skills)
                {
                    _skills[skill.Id] = skill;
                }
            }
            _skillsFingerprint = fingerprint;

            _logger.Info(null, "Skills refreshed from storage: {0} skill(s) loaded", skills.Count);
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to refresh skills from storage: {0}", ex.Message);
        }
    }

    /// <summary>
    /// Synchronizes <see cref="AutoSkillTickObject"/> registrations with the current
    /// set of auto-trigger skills: registers tick objects for new auto skills and
    /// unregisters tick objects whose skills no longer exist. Skills with an
    /// unsupported schedule format are skipped with a warning.
    /// </summary>
    public void SyncAutoSkillTickObjects(SiliconBeingBase being)
    {
        List<SkillDefinition> autoSkills = GetAutoSkills();
        var currentIds = new HashSet<string>(autoSkills.Select(s => s.Id), StringComparer.Ordinal);

        List<AutoSkillTickObject> toRemove;
        List<(string id, SkillDefinition skill)> toAdd;
        lock (_lock)
        {
            toRemove = _autoSkillTickObjects
                .Where(kvp => !currentIds.Contains(kvp.Key))
                .Select(kvp => kvp.Value)
                .ToList();
            foreach (var tickObject in toRemove)
            {
                _autoSkillTickObjects.Remove(tickObject.SkillId);
            }

            toAdd = autoSkills
                .Where(s => !_autoSkillTickObjects.ContainsKey(s.Id))
                .Select(s => (s.Id, s))
                .ToList();
        }

        foreach (var tickObject in toRemove)
        {
            MainLoop.Unregister(tickObject);
        }

        foreach (var (id, skill) in toAdd)
        {
            try
            {
                var tickObject = new AutoSkillTickObject(skill, being);
                lock (_lock)
                {
                    _autoSkillTickObjects[id] = tickObject;
                }
            }
            catch (ArgumentException ex)
            {
                _logger.Warn(being.Id, "Auto skill '{0}' not scheduled: {1}", id, ex.Message);
            }
        }
    }

    /// <summary>
    /// Executes a skill.
    /// 1. Permission check + recursion guard
    /// 2. Builds a sub AIRequest (system prompt from template + tool whitelist)
    /// 3. Sub-loop: AI + tool calls (at most MaxToolRound rounds, clamped by global config)
    /// 4. Result handling (write memory / notify curator / broadcast)
    /// 5. Returns a ToolResult (same return type as ToolManager.ExecuteTool)
    /// </summary>
    public ToolResult ExecuteSkill(
        string skillId,
        Dictionary<string, object>? parameters,
        SiliconBeingBase being)
    {
        var skill = GetSkill(skillId);
        if (skill == null)
        {
            return ToolResult.Failed($"Skill '{skillId}' not found");
        }

        if (!SkillEnabled)
        {
            return ToolResult.Failed("Skill system is disabled by configuration");
        }

        // Permission check
        if (!IsSkillAllowed(skillId, being))
        {
            _logger.Warn(being.Id, "Skill '{0}' denied for being {1}", skillId, being.Name);
            return ToolResult.Failed($"Skill '{skillId}' is not allowed for this being");
        }

        // Recursion guard
        lock (_lock)
        {
            if (_executingSkills.Contains(skillId))
            {
                return ToolResult.Failed($"Skill '{skillId}' is already executing — recursive calls are not allowed");
            }
            _executingSkills.Add(skillId);
        }

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            return ExecuteSkillCore(skill, parameters, being, stopwatch);
        }
        finally
        {
            lock (_lock)
            {
                _executingSkills.Remove(skillId);
            }
        }
    }

    private ToolResult ExecuteSkillCore(
        SkillDefinition skill,
        Dictionary<string, object>? parameters,
        SiliconBeingBase being,
        System.Diagnostics.Stopwatch stopwatch)
    {
        if (being.AIClient == null)
        {
            return ToolResult.Failed("AI client is not available for skill execution");
        }

        IAIClient client = being.AIClient;

        // Clamp execution strategy by global config
        int maxToolRound = Math.Min(
            skill.MaxToolRound > 0 ? skill.MaxToolRound : 5,
            Config.Instance?.Data?.GlobalMaxToolRound ?? 10);
        int globalTimeoutSeconds = Config.Instance?.Data?.GlobalSkillTimeoutSeconds ?? 300;
        TimeSpan timeout = skill.Timeout > TimeSpan.Zero && skill.Timeout <= TimeSpan.FromSeconds(globalTimeoutSeconds)
            ? skill.Timeout
            : TimeSpan.FromSeconds(globalTimeoutSeconds);

        // Effective permissions = being permissions ∪ skill restrictions (stricter side wins)
        var effectivePermissions = MergePermissions(being.ToolActionPermissions, skill.ToolActionRestrictions);

        // Tool whitelist: empty = inherit all of the being's tools
        ToolManager? toolManager = being.ToolManager;
        List<ToolDefinition> availableTools;
        HashSet<string> allowedToolNames;

        if (toolManager == null || toolManager.ToolCount == 0)
        {
            availableTools = new List<ToolDefinition>();
            allowedToolNames = new HashSet<string>();
        }
        else if (skill.ToolWhitelist.Count > 0)
        {
            allowedToolNames = new HashSet<string>(skill.ToolWhitelist);
            availableTools = toolManager.GetToolDefinitions(skill.ToolWhitelist, being.Id, effectivePermissions);
        }
        else
        {
            availableTools = toolManager.GetToolDefinitions(ToolScenarioFlag.All, being.Id, effectivePermissions);
            allowedToolNames = new HashSet<string>(availableTools.Select(t => t.Name));
        }

        if (skill.ToolWhitelist.Count > 0 && availableTools.Count == 0)
        {
            return ToolResult.Failed($"Skill '{skill.Id}' has no usable tools in its whitelist (intersection with the being's tools is empty)");
        }

        // Build the sub request
        var convertedParams = ConvertParameters(parameters ?? new Dictionary<string, object>());
        string systemPrompt = FillTemplate(skill.SystemPromptTemplate, convertedParams);

        AIRequest request = new(client.DefaultModel)
        {
            Messages = new List<ChatMessage>
            {
                new(being.Id, Guid.Empty, systemPrompt) { Role = MessageRole.System },
                new(being.Id, Guid.Empty, BuildTaskUserMessage(skill, convertedParams)) { Role = MessageRole.User },
            },
        };
        if (availableTools.Count > 0 && client.SupportsToolCalls != false)
        {
            request.Tools = availableTools;
        }

        _logger.Info(being.Id, "[Skill] execution start: skill={0}, rounds<={1}, timeout={2}s, tools={3}",
            skill.Id, maxToolRound, timeout.TotalSeconds, allowedToolNames.Count);

        // Sub-loop: AI + tool calls
        int round = 0;
        string finalContent = string.Empty;
        while (round < maxToolRound)
        {
            if (stopwatch.Elapsed >= timeout)
            {
                var timeoutResult = ToolResult.Failed(
                    $"Skill '{skill.Id}' timed out after {stopwatch.Elapsed.TotalSeconds:F1}s ({round} round(s) completed)");
                HandleCompletion(skill, being, timeoutResult, round, stopwatch);
                return timeoutResult;
            }

            AIResponse response = client.Chat(request);
            round++;

            if (!response.Success)
            {
                var failResult = ToolResult.Failed($"Skill '{skill.Id}' AI request failed: {response.ErrorMessage}");
                HandleCompletion(skill, being, failResult, round, stopwatch);
                return failResult;
            }

            if (response.HasToolCalls)
            {
                // Record the assistant tool-call round
                request.Messages.Add(new ChatMessage(being.Id, Guid.Empty, response.Content ?? "")
                {
                    Role = MessageRole.Assistant,
                    ToolCallsJson = JsonSerializer.Serialize(response.ToolCalls!),
                });

                foreach (ToolCall toolCall in response.ToolCalls!)
                {
                    ToolResult result;
                    if (toolManager != null && allowedToolNames.Contains(toolCall.Name))
                    {
                        result = toolManager.ExecuteTool(toolCall.Name, toolCall.Arguments, being: being);
                    }
                    else
                    {
                        result = ToolResult.Failed($"Tool '{toolCall.Name}' is not available in skill '{skill.Id}' (not in whitelist)");
                    }

                    string toolCallId = string.IsNullOrEmpty(toolCall.Id) ? Guid.NewGuid().ToString() : toolCall.Id;
                    request.Messages.Add(new ChatMessage(being.Id, Guid.Empty, SerializeToolResult(result))
                    {
                        Role = MessageRole.Tool,
                        ToolCallId = toolCallId,
                    });

                    _logger.Info(being.Id, "[Skill] tool call in {0}: {1}, success={2}", skill.Id, toolCall.Name, result.Success);
                }

                continue;
            }

            // Plain text response — skill finished
            finalContent = response.Content ?? string.Empty;
            break;
        }

        ToolResult finalResult;
        if (string.IsNullOrEmpty(finalContent))
        {
            finalResult = ToolResult.Failed(
                $"Skill '{skill.Id}' exceeded the maximum tool round limit ({maxToolRound}) without producing a final answer");
        }
        else
        {
            finalResult = ToolResult.Successful(finalContent);
        }

        HandleCompletion(skill, being, finalResult, round, stopwatch);
        return finalResult;
    }

    /// <summary>
    /// Post-completion handling: audit log + memory write + curator notification / broadcast.
    /// </summary>
    private void HandleCompletion(
        SkillDefinition skill,
        SiliconBeingBase being,
        ToolResult result,
        int rounds,
        System.Diagnostics.Stopwatch stopwatch)
    {
        double duration = stopwatch.Elapsed.TotalSeconds;

        // Audit log (§9.4)
        _logger.Info(being.Id, "[Skill] being={0} skill={1} result={2} rounds={3} duration={4:F1}s",
            being.Name, skill.Id, result.Success ? "success" : "failed", rounds, duration);

        string action = skill.OnCompleteAction?.Trim().ToLowerInvariant() ?? "write_memory";
        if (action == "none") return;

        if (action is "write_memory" or "notify_curator" or "broadcast")
        {
            try
            {
                string status = result.Success ? "成功" : "失败";
                string preview = result.Message?.Length > 100 ? result.Message[..100] : result.Message;
                being.Memory?.Add($"[技能执行] {skill.Id} - {status}: {preview}");
            }
            catch (Exception ex)
            {
                _logger.Warn(being.Id, "Failed to write skill execution to memory: {0}", ex.Message);
            }
        }

        if (action is "notify_curator" or "broadcast")
        {
            try
            {
                string content = $"[技能执行] {being.Name} 执行技能 {skill.Id} " +
                    $"({(result.Success ? "成功" : "失败")}, {rounds} 轮, {duration:F1}s):\n" +
                    $"{(result.Message?.Length > 500 ? result.Message[..500] + "..." : result.Message)}";
                DeliverNotification(being, content, action == "broadcast");
            }
            catch (Exception ex)
            {
                _logger.Warn(being.Id, "Failed to deliver skill completion notification: {0}", ex.Message);
            }
        }
    }

    private static void DeliverNotification(SiliconBeingBase being, string content, bool broadcast)
    {
        ChatSystem? chatSystem = ServiceLocator.Instance.ChatSystem;
        if (chatSystem == null) return;

        if (broadcast)
        {
            Guid channelId = Config.Instance?.Data?.BroadcastChannelGuid ?? Guid.Empty;
            if (channelId == Guid.Empty) return;

            var channel = chatSystem.GetOrCreateBroadcastChannel(channelId);
            chatSystem.AddMessage(new ChatMessage(being.Id, channel.Id, content)
            {
                Role = MessageRole.Assistant,
            });
        }
        else
        {
            var curator = SiliconBeingManager.GetCuratorBeing();
            if (curator == null || curator.Id == being.Id) return;

            var session = chatSystem.GetOrCreateSession(being.Id, curator.Id);
            chatSystem.AddMessage(new ChatMessage(being.Id, session.Id, content)
            {
                Role = MessageRole.Assistant,
            });

            // Push via IMManager for real-time SSE delivery
            IMManager? imManager = ServiceLocator.Instance.IMManager;
            if (imManager != null)
            {
                _ = imManager.SendMessageAsync(being.Id, session.Id, content, senderName: being.Name);
            }
        }
    }

    /// <summary>Merges the skill's tool action restrictions into the being's permissions (union of disabled actions).</summary>
    private static ToolActionPermissionConfig? MergePermissions(
        ToolActionPermissionConfig? beingPermissions,
        ToolActionPermissionConfig? skillRestrictions)
    {
        if (skillRestrictions == null || skillRestrictions.DisabledActions.Count == 0)
        {
            return beingPermissions;
        }

        var merged = new ToolActionPermissionConfig();
        if (beingPermissions != null)
        {
            foreach (var kvp in beingPermissions.DisabledActions)
            {
                foreach (var action in kvp.Value)
                {
                    merged.DisableAction(kvp.Key, action);
                }
            }
        }
        foreach (var kvp in skillRestrictions.DisabledActions)
        {
            foreach (var action in kvp.Value)
            {
                merged.DisableAction(kvp.Key, action);
            }
        }
        return merged;
    }

    /// <summary>Fills {param} placeholders in the system prompt template from the skill arguments.</summary>
    private static string FillTemplate(string template, Dictionary<string, object> parameters)
    {
        if (string.IsNullOrEmpty(template)) return string.Empty;

        string result = template;
        foreach (var kvp in parameters)
        {
            result = result.Replace($"{{{kvp.Key}}}", kvp.Value?.ToString() ?? "");
        }
        return result;
    }

    private static string BuildTaskUserMessage(SkillDefinition skill, Dictionary<string, object> parameters)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("请执行以下技能任务。");
        sb.AppendLine($"Skill: {skill.Id} (v{skill.Version})");
        sb.AppendLine($"Description: {skill.Description}");
        if (parameters.Count > 0)
        {
            sb.AppendLine("Parameters:");
            sb.AppendLine(JsonSerializer.Serialize(parameters));
        }
        sb.AppendLine();
        sb.AppendLine("按照系统提示词的指引完成任务。需要时调用提供的工具。完成后直接输出最终结果，不要输出其他解释。");
        return sb.ToString();
    }

    /// <summary>Serializes a ToolResult to JSON for sending back to the AI.</summary>
    private static string SerializeToolResult(ToolResult result)
    {
        var obj = new Dictionary<string, object>
        {
            ["success"] = result.Success,
            ["message"] = result.Message ?? ""
        };

        if (result.Data != null)
        {
            obj["data"] = result.Data;
        }

        try
        {
            return JsonSerializer.Serialize(obj);
        }
        catch
        {
            return JsonSerializer.Serialize(new { success = result.Success, message = result.Message });
        }
    }

    /// <summary>Converts JsonElement values in the parameters dictionary to native .NET types.</summary>
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

    private static object? ConvertJsonValue(object? value)
    {
        if (value is JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.TryGetInt32(out int i) ? i
                    : element.TryGetInt64(out long l) ? l : element.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                JsonValueKind.Array => element.EnumerateArray().Select(e => ConvertJsonValue(e)!).ToList(),
                JsonValueKind.Object => element.EnumerateObject()
                    .Where(p => ConvertJsonValue(p.Value) != null)
                    .ToDictionary(p => p.Name, p => ConvertJsonValue(p.Value)!),
                _ => value
            };
        }
        return value;
    }
}
