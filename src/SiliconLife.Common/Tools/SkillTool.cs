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

using System.Text.Json;

using SiliconLife.Collective;

using SiliconLife.Common.Localization;

namespace SiliconLife.Common.Tools;

/// <summary>
/// Tool for managing skills for silicon beings.
/// Supports creating, listing, updating (parameters or Markdown), deleting,
/// exporting and importing (JSON or Markdown) skills.
/// </summary>
[ToolAction("create", "list", "update", "update_from_md", "delete", "export", "export_md", "import", "import_md")]
[ToolScenario(ToolScenarioFlag.All)]
public class SkillTool : ITool
{
    /// <inheritdoc/>
    public string Name => "skill";

    /// <inheritdoc/>
    public string Description =>
        "Manage skills (reusable tool orchestration + prompt templates) for the silicon being. " +
        "Actions: " +
        "'create' (create a new skill: requires 'id' and 'system_prompt', optional 'description', 'parameter_schema', 'tool_whitelist', 'tags', 'max_tool_round', 'timeout', 'on_complete', 'trigger_mode', 'auto_trigger_condition'; missing metadata is auto-completed by AI); " +
        "'list' (list all available skills with summaries); " +
        "'update' (update an existing skill from parameters: requires 'skill_id'); " +
        "'update_from_md' (update a skill from a Markdown string — YAML front matter + prompt body); " +
        "'delete' (delete a skill: requires 'skill_id'); " +
        "'export' (export a skill as JSON: requires 'skill_id'); " +
        "'export_md' (export a skill as Markdown: requires 'skill_id'); " +
        "'import' (import a skill from JSON: requires 'json'); " +
        "'import_md' (import a skill from Markdown: requires 'markdown').";

    /// <inheritdoc/>
    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

    /// <inheritdoc/>
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
                    ["description"] = "The action to perform: create, list, update, update_from_md, delete, export, export_md, import, import_md",
                    ["enum"] = new[] { "create", "list", "update", "update_from_md", "delete", "export", "export_md", "import", "import_md" }
                },
                ["skill_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Skill ID (required for update, update_from_md, delete, export, export_md)"
                },
                ["system_prompt"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "System prompt template for create/update actions. Supports {param} placeholders that are filled from skill arguments at execution time"
                },
                ["description"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "One-sentence description of the skill for AI function calling (create/update)"
                },
                ["version"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Semantic version of the skill, e.g. '1.0.0' (optional, default '1.0.0')"
                },
                ["tags"] = new Dictionary<string, object>
                {
                    ["type"] = "array",
                    ["description"] = "Classification tags for the skill (optional)",
                    ["items"] = new Dictionary<string, object> { ["type"] = "string" }
                },
                ["parameter_schema"] = new Dictionary<string, object>
                {
                    ["type"] = "object",
                    ["description"] = "JSON Schema of the skill parameters. Declare every {param} placeholder used in the system prompt"
                },
                ["tool_whitelist"] = new Dictionary<string, object>
                {
                    ["type"] = "array",
                    ["description"] = "Allowed tool names during skill execution. Empty list = inherit all tools of the being (optional)",
                    ["items"] = new Dictionary<string, object> { ["type"] = "string" }
                },
                ["max_tool_round"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum tool call rounds during execution (optional, default 5)"
                },
                ["timeout"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Execution timeout, e.g. '60s', '2m' (optional, default '60s')"
                },
                ["on_complete"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Action after completion: write_memory, notify_curator, broadcast, or none (optional, default 'write_memory')",
                    ["enum"] = new[] { "write_memory", "notify_curator", "broadcast", "none" }
                },
                ["trigger_mode"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Trigger mode: manual or auto (optional, default 'manual')",
                    ["enum"] = new[] { "manual", "auto" }
                },
                ["auto_trigger_condition"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Schedule condition for auto trigger mode, e.g. 'daily 09:00', 'interval 6h', or a cron expression (optional)"
                },
                ["markdown"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Markdown content for update_from_md / import_md actions. Two formats accepted: (1) full format with YAML front matter (---) + body, or (2) pure Markdown body without front matter — missing metadata (id, description, parameter_schema) will be auto-generated by AI during import"
                },
                ["json"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "JSON-serialized SkillDefinition for the import action"
                },
            },
            ["required"] = new[] { "action" }
        };
    }

    /// <inheritdoc/>
    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        try
        {
            if (!parameters.TryGetValue("action", out var actionObj))
            {
                return ToolResult.Failed("Missing required parameter: action");
            }

            string action = actionObj?.ToString()?.ToLowerInvariant() ?? "";
            if (string.IsNullOrEmpty(action))
            {
                return ToolResult.Failed("Missing required parameter: action");
            }

            SiliconBeingBase? being = ServiceLocator.Instance.BeingManager?.GetBeing(callerId);
            if (being == null)
            {
                return ToolResult.Failed($"Silicon being not found: {callerId}");
            }

            if (being.SkillManager == null)
            {
                return ToolResult.Failed("Skill system is not available");
            }

            return action switch
            {
                "create" => ExecuteCreate(being, parameters),
                "list" => ExecuteList(being),
                "update" => ExecuteUpdate(being, parameters),
                "update_from_md" => ExecuteUpdateFromMarkdown(being, parameters),
                "delete" => ExecuteDelete(being, parameters),
                "export" => ExecuteExport(being, parameters),
                "export_md" => ExecuteExportMarkdown(being, parameters),
                "import" => ExecuteImport(being, parameters),
                "import_md" => ExecuteImportMarkdown(being, parameters),
                _ => ToolResult.Failed($"Unknown action: {action}")
            };
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Skill tool execution failed: {ex.Message}");
        }
    }

    private static ToolResult ExecuteCreate(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!TryGetString(parameters, "id", out string? id) || string.IsNullOrWhiteSpace(id))
        {
            return ToolResult.Failed("Missing required parameter: id");
        }

        if (!TryGetString(parameters, "system_prompt", out string? systemPrompt) || string.IsNullOrWhiteSpace(systemPrompt))
        {
            return ToolResult.Failed("Missing required parameter: system_prompt");
        }

        var skillManager = being.SkillManager!;
        if (skillManager.GetSkill(id) != null)
        {
            return ToolResult.Failed($"Skill '{id}' already exists — use the 'update' action to modify it");
        }

        if (skillManager.CustomSkillCount >= SkillManager.MaxCustomSkills)
        {
            return ToolResult.Failed($"Maximum custom skill limit reached ({SkillManager.MaxCustomSkills})");
        }

        SkillDefinition skill = BuildSkillFromParameters(id, systemPrompt, parameters);

        // Complete missing metadata (description / parameter_schema) via AI
        if (SkillMarkdownParser.NeedsCompletion(skill))
        {
            skill = CompleteOrFallback(being, skill, systemPrompt);
            if (skill == null || string.IsNullOrEmpty(skill.Id))
            {
                return ToolResult.Failed("AI metadata completion failed — please provide at least 'description' and 'parameter_schema'");
            }
        }

        skillManager.RegisterSkill(skill);
        if (being.Storage != null && !SkillFileManager.SaveSkillAsMarkdown(being.Storage, skill))
        {
            return ToolResult.Failed($"Failed to persist skill '{skill.Id}' to storage");
        }

        return ToolResult.Successful($"Skill '{skill.Id}' created successfully (v{skill.Version})");
    }

    private static ToolResult ExecuteList(SiliconBeingBase being)
    {
        var skills = being.SkillManager!.GetAllSkills();
        if (skills.Count == 0)
        {
            return ToolResult.Successful("No skills registered.");
        }

        var lines = skills
            .OrderBy(s => s.Id, StringComparer.Ordinal)
            .Select(s => $"- {s.Id} (v{s.Version}, source={s.Source.ToString().ToLowerInvariant()}, trigger={s.TriggerMode.ToString().ToLowerInvariant()}): {s.Description}");
        return ToolResult.Successful($"{skills.Count} skill(s) registered:\n" + string.Join("\n", lines), skills);
    }

    private static ToolResult ExecuteUpdate(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!TryGetString(parameters, "skill_id", out string? skillId) || string.IsNullOrWhiteSpace(skillId))
        {
            return ToolResult.Failed("Missing required parameter: skill_id");
        }

        var skillManager = being.SkillManager!;
        SkillDefinition? existing = skillManager.GetSkill(skillId);
        if (existing == null)
        {
            return ToolResult.Failed($"Skill '{skillId}' not found");
        }

        if (!CanModifySkill(being, skillId))
        {
            return ToolResult.Failed($"Permission denied: cannot modify skill '{skillId}'");
        }

        // The new id may differ from the old one (rename); default to the existing id
        string newId = TryGetString(parameters, "id", out string? declaredId) && !string.IsNullOrWhiteSpace(declaredId)
            ? declaredId!
            : existing.Id;
        string systemPrompt = TryGetString(parameters, "system_prompt", out string? prompt) && !string.IsNullOrWhiteSpace(prompt)
            ? prompt!
            : existing.SystemPromptTemplate;

        SkillDefinition updated = BuildSkillFromParameters(newId, systemPrompt, parameters, fallback: existing);

        if (SkillMarkdownParser.NeedsCompletion(updated))
        {
            updated = CompleteOrFallback(being, updated, systemPrompt) ?? updated;
            if (string.IsNullOrEmpty(updated.Id))
            {
                return ToolResult.Failed("AI metadata completion failed — please provide 'description' and 'parameter_schema'");
            }
        }

        // Archive the previous version (skill evolution history)
        if (being.Storage != null)
        {
            SkillFileManager.ArchiveSkill(being.Storage, existing);
        }

        skillManager.UnregisterSkill(existing.Id);
        skillManager.RegisterSkill(updated);
        if (being.Storage != null)
        {
            if (!SkillFileManager.SaveSkillAsMarkdown(being.Storage, updated))
            {
                return ToolResult.Failed($"Failed to persist skill '{updated.Id}' to storage");
            }
            if (updated.Id != existing.Id)
            {
                SkillFileManager.DeleteSkill(being.Storage, existing.Id);
            }
        }

        return ToolResult.Successful($"Skill '{updated.Id}' updated successfully (v{updated.Version})");
    }

    private static ToolResult ExecuteUpdateFromMarkdown(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!TryGetString(parameters, "markdown", out string? markdown) || string.IsNullOrWhiteSpace(markdown))
        {
            return ToolResult.Failed("'markdown' parameter is required for update_from_md");
        }

        SkillDefinition? skill = SkillMarkdownParser.Parse(markdown);
        if (skill == null)
        {
            return ToolResult.Failed("Failed to parse Markdown");
        }

        // The target skill is identified by the skill_id parameter, or by the id in the front matter
        string targetId = TryGetString(parameters, "skill_id", out string? declaredId) && !string.IsNullOrWhiteSpace(declaredId)
            ? declaredId!
            : skill.Id;
        if (string.IsNullOrWhiteSpace(targetId))
        {
            return ToolResult.Failed("Cannot determine the skill to update — provide 'skill_id' or an 'id' in the YAML front matter");
        }

        var skillManager = being.SkillManager!;
        SkillDefinition? existing = skillManager.GetSkill(targetId);
        if (existing == null)
        {
            return ToolResult.Failed($"Skill '{targetId}' not found");
        }

        if (!CanModifySkill(being, targetId))
        {
            return ToolResult.Failed($"Permission denied: cannot modify skill '{targetId}'");
        }

        // Complete missing metadata (pure Markdown mode) via AI
        if (SkillMarkdownParser.NeedsCompletion(skill))
        {
            skill = CompleteOrFallback(being, skill, markdown);
            if (skill == null || string.IsNullOrEmpty(skill.Id))
            {
                return ToolResult.Failed("AI metadata completion failed — please provide at least an 'id' in YAML front matter");
            }
        }

        // Keep the target id and preserve ownership fields
        skill = CloneSkill(skill, id: targetId, newSource: existing.Source == SkillSource.Builtin ? SkillSource.User : existing.Source);

        if (being.Storage != null)
        {
            SkillFileManager.ArchiveSkill(being.Storage, existing);
        }

        skillManager.UnregisterSkill(existing.Id);
        skillManager.RegisterSkill(skill);
        if (being.Storage != null)
        {
            if (!SkillFileManager.SaveSkillAsMarkdown(being.Storage, skill))
            {
                return ToolResult.Failed($"Failed to persist skill '{skill.Id}' to storage");
            }
            if (skill.Id != existing.Id)
            {
                SkillFileManager.DeleteSkill(being.Storage, existing.Id);
            }
        }

        return ToolResult.Successful($"Skill '{skill.Id}' updated from Markdown (v{skill.Version})");
    }

    private static ToolResult ExecuteDelete(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!TryGetString(parameters, "skill_id", out string? skillId) || string.IsNullOrWhiteSpace(skillId))
        {
            return ToolResult.Failed("Missing required parameter: skill_id");
        }

        var skillManager = being.SkillManager!;
        SkillDefinition? existing = skillManager.GetSkill(skillId);
        if (existing == null)
        {
            return ToolResult.Failed($"Skill '{skillId}' not found");
        }

        if (!CanModifySkill(being, skillId))
        {
            return ToolResult.Failed($"Permission denied: cannot delete skill '{skillId}'");
        }

        skillManager.UnregisterSkill(skillId);
        if (being.Storage != null)
        {
            SkillFileManager.DeleteSkill(being.Storage, skillId);
        }

        return ToolResult.Successful($"Skill '{skillId}' deleted");
    }

    private static ToolResult ExecuteExport(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!TryGetString(parameters, "skill_id", out string? skillId) || string.IsNullOrWhiteSpace(skillId))
        {
            return ToolResult.Failed("Missing required parameter: skill_id");
        }

        SkillDefinition? skill = being.SkillManager!.GetSkill(skillId);
        if (skill == null)
        {
            return ToolResult.Failed($"Skill '{skillId}' not found");
        }

        string json = SkillFileManager.ExportSkill(skill);
        return ToolResult.Successful($"Skill '{skillId}' exported as JSON", json);
    }

    private static ToolResult ExecuteExportMarkdown(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!TryGetString(parameters, "skill_id", out string? skillId) || string.IsNullOrWhiteSpace(skillId))
        {
            return ToolResult.Failed("Missing required parameter: skill_id");
        }

        SkillDefinition? skill = being.SkillManager!.GetSkill(skillId);
        if (skill == null)
        {
            return ToolResult.Failed($"Skill '{skillId}' not found");
        }

        string markdown = SkillMarkdownParser.Serialize(skill);
        return ToolResult.Successful($"Skill '{skillId}' exported as Markdown", markdown);
    }

    private static ToolResult ExecuteImport(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!TryGetString(parameters, "json", out string? json) || string.IsNullOrWhiteSpace(json))
        {
            return ToolResult.Failed("'json' parameter is required for import");
        }

        SkillDefinition? skill = SkillFileManager.ImportSkill(json);
        if (skill == null || string.IsNullOrWhiteSpace(skill.Id))
        {
            return ToolResult.Failed("Failed to import skill from JSON — invalid SkillDefinition");
        }

        var skillManager = being.SkillManager!;
        if (skillManager.CustomSkillCount >= SkillManager.MaxCustomSkills && skillManager.GetSkill(skill.Id) == null)
        {
            return ToolResult.Failed($"Maximum custom skill limit reached ({SkillManager.MaxCustomSkills})");
        }

        // Imported skills are owned by the importing being
        skill = CloneSkill(skill, newSource: SkillSource.Being);
        skillManager.RegisterSkill(skill);
        if (being.Storage != null && !SkillFileManager.SaveSkillAsMarkdown(being.Storage, skill))
        {
            return ToolResult.Failed($"Failed to persist skill '{skill.Id}' to storage");
        }

        return ToolResult.Successful($"Skill '{skill.Id}' imported from JSON (v{skill.Version})");
    }

    private static ToolResult ExecuteImportMarkdown(SiliconBeingBase being, Dictionary<string, object> parameters)
    {
        if (!TryGetString(parameters, "markdown", out string? markdown) || string.IsNullOrWhiteSpace(markdown))
        {
            return ToolResult.Failed("'markdown' parameter is required for import_md");
        }

        SkillDefinition? skill = SkillMarkdownParser.Parse(markdown);
        if (skill == null)
        {
            return ToolResult.Failed("Failed to parse Markdown");
        }

        var skillManager = being.SkillManager!;
        if (skillManager.CustomSkillCount >= SkillManager.MaxCustomSkills && (string.IsNullOrEmpty(skill.Id) || skillManager.GetSkill(skill.Id) == null))
        {
            return ToolResult.Failed($"Maximum custom skill limit reached ({SkillManager.MaxCustomSkills})");
        }

        // Complete missing metadata (pure Markdown mode) via AI
        if (SkillMarkdownParser.NeedsCompletion(skill))
        {
            skill = CompleteOrFallback(being, skill, markdown);
            if (skill == null || string.IsNullOrEmpty(skill.Id))
            {
                return ToolResult.Failed("AI metadata completion failed — please provide at least an 'id' in YAML front matter");
            }
        }

        // Imported skills are owned by the importing being
        skill = CloneSkill(skill, newSource: SkillSource.Being);
        skillManager.RegisterSkill(skill);
        if (being.Storage != null && !SkillFileManager.SaveSkillAsMarkdown(being.Storage, skill))
        {
            return ToolResult.Failed($"Failed to persist skill '{skill.Id}' to storage");
        }

        return ToolResult.Successful($"Skill '{skill.Id}' imported from Markdown (v{skill.Version})");
    }

    /// <summary>
    /// Builds a SkillDefinition from tool parameters.
    /// Fields not present in the parameters fall back to the given existing
    /// definition (update flow) or to defaults (create flow).
    /// </summary>
    private static SkillDefinition BuildSkillFromParameters(
        string id,
        string systemPrompt,
        Dictionary<string, object> parameters,
        SkillDefinition? fallback = null)
    {
        // Skills created/updated by the being via this tool are owned by it
        SkillSource source = SkillSource.Being;

        return new SkillDefinition
        {
            Id = id,
            Description = GetString(parameters, "description")
                ?? fallback?.Description
                ?? "",
            DisplayNameKey = GetString(parameters, "display_name_key")
                ?? fallback?.DisplayNameKey
                ?? "",
            Version = GetString(parameters, "version")
                ?? fallback?.Version
                ?? "1.0.0",
            Tags = GetStringList(parameters, "tags", fallback?.Tags),
            ParameterSchema = GetParameterSchema(parameters, fallback?.ParameterSchema),
            SystemPromptTemplate = systemPrompt,
            ToolWhitelist = GetStringList(parameters, "tool_whitelist", fallback?.ToolWhitelist),
            ToolActionRestrictions = fallback?.ToolActionRestrictions,
            MaxToolRound = GetInt(parameters, "max_tool_round", fallback?.MaxToolRound ?? 5),
            Timeout = GetTimeout(parameters, fallback?.Timeout ?? TimeSpan.FromSeconds(60)),
            OnCompleteAction = GetString(parameters, "on_complete")
                ?? fallback?.OnCompleteAction
                ?? "write_memory",
            Source = source,
            TriggerMode = GetTriggerMode(parameters, fallback?.TriggerMode ?? SkillTriggerMode.Manual),
            AutoTriggerCondition = GetString(parameters, "auto_trigger_condition")
                ?? fallback?.AutoTriggerCondition,
            Metadata = fallback?.Metadata ?? new Dictionary<string, object>(),
        };
    }

    /// <summary>
    /// Runs AI metadata completion; on failure falls back to deterministic
    /// id extraction from the body. Returns null when no usable definition remains.
    /// </summary>
    private static SkillDefinition? CompleteOrFallback(SiliconBeingBase being, SkillDefinition partial, string rawContent)
    {
        try
        {
            SkillDefinition? completed = SkillMetadataCompleter.Complete(being, rawContent, partial);
            if (completed != null)
            {
                return completed;
            }
        }
        catch
        {
            // Fall through to the deterministic fallback below
        }

        // Deterministic fallback: only the id can be recovered without AI
        if (string.IsNullOrEmpty(partial.Id))
        {
            string? extracted = SkillMarkdownParser.ExtractIdFromBody(partial.SystemPromptTemplate ?? "");
            if (string.IsNullOrEmpty(extracted)) return null;
            return CloneSkill(partial, id: extracted);
        }
        return partial;
    }

    /// <summary>
    /// Permission check: the curator may modify every skill; other beings may
    /// only modify skills they created themselves (source=being) or user-imported ones.
    /// </summary>
    private static bool CanModifySkill(SiliconBeingBase being, string skillId)
    {
        if (being.IsCurator) return true;
        var existing = being.SkillManager?.GetSkill(skillId);
        return existing?.Source is SkillSource.Being or SkillSource.User;
    }

    private static SkillDefinition CloneSkill(
        SkillDefinition source,
        string? id = null,
        SkillSource? newSource = null)
    {
        return new SkillDefinition
        {
            Id = id ?? source.Id,
            Description = source.Description,
            DisplayNameKey = source.DisplayNameKey,
            Version = source.Version,
            Tags = source.Tags,
            ParameterSchema = source.ParameterSchema,
            SystemPromptTemplate = source.SystemPromptTemplate,
            ToolWhitelist = source.ToolWhitelist,
            ToolActionRestrictions = source.ToolActionRestrictions,
            MaxToolRound = source.MaxToolRound,
            Timeout = source.Timeout,
            OnCompleteAction = source.OnCompleteAction,
            Source = newSource ?? source.Source,
            TriggerMode = source.TriggerMode,
            AutoTriggerCondition = source.AutoTriggerCondition,
            Metadata = source.Metadata,
        };
    }

    // ===== Parameter helpers =====

    private static bool TryGetString(Dictionary<string, object> parameters, string key, out string? value)
    {
        if (parameters.TryGetValue(key, out var obj) && obj != null)
        {
            string? s = obj is JsonElement el && el.ValueKind == JsonValueKind.String
                ? el.GetString()
                : obj.ToString();
            if (!string.IsNullOrWhiteSpace(s))
            {
                value = s;
                return true;
            }
        }
        value = null;
        return false;
    }

    private static string? GetString(Dictionary<string, object> parameters, string key)
    {
        return TryGetString(parameters, key, out string? value) ? value : null;
    }

    private static List<string> GetStringList(Dictionary<string, object> parameters, string key, List<string>? fallback)
    {
        if (parameters.TryGetValue(key, out var obj) && obj is JsonElement el)
        {
            if (el.ValueKind == JsonValueKind.Array)
            {
                var list = el.EnumerateArray()
                    .Select(e => e.ValueKind == JsonValueKind.String ? e.GetString() : e.ToString())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s!.Trim())
                    .ToList();
                if (list.Count > 0) return list;
            }
            else if (el.ValueKind == JsonValueKind.String)
            {
                string? single = el.GetString();
                if (!string.IsNullOrWhiteSpace(single)) return new List<string> { single.Trim() };
            }
        }
        else if (obj != null && obj.ToString() is { Length: > 0 } text)
        {
            return text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        }
        return fallback ?? new List<string>();
    }

    private static Dictionary<string, object> GetParameterSchema(Dictionary<string, object> parameters, Dictionary<string, object>? fallback)
    {
        if (parameters.TryGetValue("parameter_schema", out var obj) && obj is JsonElement el && el.ValueKind == JsonValueKind.Object)
        {
            try
            {
                var schema = JsonSerializer.Deserialize<Dictionary<string, object>>(el.GetRawText());
                if (schema != null && schema.Count > 0) return schema;
            }
            catch
            {
                // Invalid schema — fall back
            }
        }
        return fallback ?? new Dictionary<string, object>();
    }

    private static int GetInt(Dictionary<string, object> parameters, string key, int fallback)
    {
        if (parameters.TryGetValue(key, out var obj) && obj != null)
        {
            if (obj is JsonElement el && el.ValueKind == JsonValueKind.Number && el.TryGetInt32(out int i))
            {
                return i;
            }
            if (int.TryParse(obj.ToString(), out int parsed))
            {
                return parsed;
            }
        }
        return fallback;
    }

    private static TimeSpan GetTimeout(Dictionary<string, object> parameters, TimeSpan fallback)
    {
        string? text = GetString(parameters, "timeout");
        if (string.IsNullOrWhiteSpace(text)) return fallback;
        try
        {
            return SkillMarkdownParser.ParseTimeSpan(text);
        }
        catch
        {
            return fallback;
        }
    }

    private static SkillTriggerMode GetTriggerMode(Dictionary<string, object> parameters, SkillTriggerMode fallback)
    {
        string? text = GetString(parameters, "trigger_mode")?.ToLowerInvariant();
        return text switch
        {
            "auto" => SkillTriggerMode.Auto,
            "manual" => SkillTriggerMode.Manual,
            _ => fallback
        };
    }
}
