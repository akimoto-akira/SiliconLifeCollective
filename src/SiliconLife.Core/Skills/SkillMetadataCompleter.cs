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

using System.Text;
using System.Text.Json;

namespace SiliconLife.Collective;

/// <summary>
/// Calls the AI client to complete missing SkillDefinition metadata.
/// When a user submits a pure Markdown prompt (no YAML front matter), the AI
/// analyzes the body and generates structured fields such as id, description
/// and parameter_schema.
/// </summary>
public static class SkillMetadataCompleter
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger(typeof(SkillMetadataCompleter));

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Completes missing metadata by calling the AI (async variant).
    /// 1. Builds a completion prompt (the original Markdown is sent as the user message)
    /// 2. The AI returns the completed metadata as JSON
    /// 3. Merges it into the existing SkillDefinition (never overwrites user-provided fields)
    /// </summary>
    /// <param name="being">The being instance (provides the AI client)</param>
    /// <param name="rawMarkdown">The original Markdown text</param>
    /// <param name="partial">The partially parsed SkillDefinition</param>
    /// <returns>The completed SkillDefinition, or null when the AI call fails</returns>
    public static async Task<SkillDefinition?> CompleteAsync(
        SiliconBeingBase being,
        string rawMarkdown,
        SkillDefinition partial)
    {
        if (being.AIClient == null)
        {
            _logger.Warn(being.Id, "AI client not available for skill metadata completion");
            return null;
        }

        string systemPrompt = BuildCompletionPrompt(partial);

        AIResponse response = await being.AIClient.ChatAsync(systemPrompt, rawMarkdown);
        return ProcessResponse(being, partial, response);
    }

    /// <summary>
    /// Completes missing metadata by calling the AI (synchronous variant,
    /// used from synchronous ITool.Execute flows).
    /// </summary>
    public static SkillDefinition? Complete(
        SiliconBeingBase being,
        string rawMarkdown,
        SkillDefinition partial)
    {
        if (being.AIClient == null)
        {
            _logger.Warn(being.Id, "AI client not available for skill metadata completion");
            return null;
        }

        string systemPrompt = BuildCompletionPrompt(partial);

        AIResponse response = being.AIClient.Chat(systemPrompt, rawMarkdown);
        return ProcessResponse(being, partial, response);
    }

    private static SkillDefinition? ProcessResponse(SiliconBeingBase being, SkillDefinition partial, AIResponse response)
    {
        if (!response.Success || string.IsNullOrEmpty(response.Content))
        {
            _logger.Warn(being.Id, "AI metadata completion failed: {0}", response.ErrorMessage ?? "empty response");
            return null;
        }

        var completion = ParseCompletionJson(response.Content);
        if (completion == null)
        {
            _logger.Warn(being.Id, "AI returned non-JSON metadata, completion failed");
            return null;
        }

        var merged = MergePartial(partial, completion);

        // If the id is still empty, extract one from the body as a fallback
        if (string.IsNullOrEmpty(merged.Id))
        {
            string? extracted = SkillMarkdownParser.ExtractIdFromBody(partial.SystemPromptTemplate ?? "");
            if (string.IsNullOrEmpty(extracted))
            {
                _logger.Warn(being.Id, "Failed to generate skill id from content");
                return null;
            }
            merged = CloneWith(merged, id: extracted);
        }

        _logger.Info(being.Id, "Skill metadata completed by AI: id={0}, desc={1}...",
            merged.Id, merged.Description.Length > 50 ? merged.Description[..50] : merged.Description);
        return merged;
    }

    /// <summary>
    /// Builds the completion prompt sent to the AI. The AI extracts/generates
    /// structured metadata from the Markdown prompt and returns it as JSON.
    /// </summary>
    private static string BuildCompletionPrompt(SkillDefinition partial)
    {
        var sb = new StringBuilder();
        sb.AppendLine("You are a skill metadata generator for the SiliconLife Collective platform.");
        sb.AppendLine("Analyze the following Markdown content (a system prompt template for a skill) and generate the missing metadata fields.");
        sb.AppendLine();
        sb.AppendLine("Return a JSON object with the following fields (omit fields that are already provided):");
        sb.AppendLine("{");
        sb.AppendLine("  \"id\": \"snake_case_identifier\",  // short, descriptive, unique");
        sb.AppendLine("  \"description\": \"One-sentence description for AI function calling\",");
        sb.AppendLine("  \"parameter_schema\": {  // JSON Schema for parameters referenced in the prompt");
        sb.AppendLine("    \"type\": \"object\",");
        sb.AppendLine("    \"properties\": { ... },");
        sb.AppendLine("    \"required\": [ ... ]");
        sb.AppendLine("  },");
        sb.AppendLine("  \"tags\": [\"tag1\", \"tag2\"],  // 3-5 classification tags");
        sb.AppendLine("  \"tool_whitelist\": [\"tool1\", \"tool2\"]  // tools mentioned or implied in the prompt");
        sb.AppendLine("}");
        sb.AppendLine();
        sb.AppendLine("Rules:");
        sb.AppendLine("- Extract {param} placeholders from the prompt to build parameter_schema.");
        sb.AppendLine("- Infer parameter types: numbers → integer/double, true/false → boolean, others → string.");
        sb.AppendLine("- If the prompt contains no placeholders, use an empty properties object.");
        sb.AppendLine("- tool names must be lowercase identifiers like \"disk\", \"network\", \"knowledge\", \"memory\", \"work_note\".");
        sb.AppendLine("- Only output the JSON object, no explanations.");
        sb.AppendLine();
        if (!string.IsNullOrEmpty(partial.Id))
            sb.AppendLine($"Already provided id: \"{partial.Id}\" — keep it.");
        if (!string.IsNullOrEmpty(partial.Description))
            sb.AppendLine($"Already provided description: \"{partial.Description}\" — keep it.");
        if (partial.ParameterSchema.Count > 0)
            sb.AppendLine("parameter_schema already provided — keep it.");
        if (partial.ToolWhitelist.Count > 0)
            sb.AppendLine($"tool_whitelist already provided: [{string.Join(", ", partial.ToolWhitelist)}] — keep it.");
        if (partial.Tags.Count > 0)
            sb.AppendLine($"tags already provided: [{string.Join(", ", partial.Tags)}] — keep it.");

        return sb.ToString();
    }

    /// <summary>
    /// Parses the JSON metadata returned by the AI.
    /// Tolerates ```json ... ``` fenced output.
    /// </summary>
    internal static Dictionary<string, object>? ParseCompletionJson(string aiResponse)
    {
        var json = aiResponse.Trim();

        // Strip ``` ... ``` fences
        if (json.StartsWith("```"))
        {
            int firstFence = json.IndexOf('\n');
            if (firstFence > 0)
            {
                json = json[(firstFence + 1)..];
                int closing = json.LastIndexOf("```", StringComparison.Ordinal);
                if (closing >= 0) json = json[..closing];
                json = json.Trim();
            }
        }

        // Find the outermost JSON object when the AI added extra prose
        int start = json.IndexOf('{');
        int end = json.LastIndexOf('}');
        if (start < 0 || end <= start) return null;
        json = json[start..(end + 1)];

        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, object>>(json, JsonOptions);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Merges AI-completed metadata into the partial SkillDefinition.
    /// User-provided fields are never overwritten — only gaps are filled.
    /// </summary>
    private static SkillDefinition MergePartial(SkillDefinition partial, Dictionary<string, object> completion)
    {
        string id = string.IsNullOrEmpty(partial.Id)
            ? GetString(completion, "id") ?? ""
            : partial.Id;

        string description = string.IsNullOrEmpty(partial.Description)
            ? GetString(completion, "description") ?? ""
            : partial.Description;

        List<string> tags = partial.Tags.Count > 0
            ? partial.Tags
            : GetStringList(completion, "tags");

        List<string> toolWhitelist = partial.ToolWhitelist.Count > 0
            ? partial.ToolWhitelist
            : GetStringList(completion, "tool_whitelist");

        Dictionary<string, object> parameterSchema = partial.ParameterSchema.Count > 0
            ? partial.ParameterSchema
            : GetSchema(completion, "parameter_schema");

        return CloneWith(partial, id: id, description: description, tags: tags,
            toolWhitelist: toolWhitelist, parameterSchema: parameterSchema);
    }

    private static SkillDefinition CloneWith(
        SkillDefinition source,
        string? id = null,
        string? description = null,
        List<string>? tags = null,
        List<string>? toolWhitelist = null,
        Dictionary<string, object>? parameterSchema = null)
    {
        return new SkillDefinition
        {
            Id = id ?? source.Id,
            Description = description ?? source.Description,
            DisplayNameKey = source.DisplayNameKey,
            Version = source.Version,
            Tags = tags ?? source.Tags,
            ParameterSchema = parameterSchema ?? source.ParameterSchema,
            SystemPromptTemplate = source.SystemPromptTemplate,
            ToolWhitelist = toolWhitelist ?? source.ToolWhitelist,
            ToolActionRestrictions = source.ToolActionRestrictions,
            MaxToolRound = source.MaxToolRound,
            Timeout = source.Timeout,
            OnCompleteAction = source.OnCompleteAction,
            Source = source.Source,
            TriggerMode = source.TriggerMode,
            AutoTriggerCondition = source.AutoTriggerCondition,
            Metadata = source.Metadata,
        };
    }

    private static string? GetString(Dictionary<string, object> dict, string key)
    {
        if (!dict.TryGetValue(key, out var value) || value == null) return null;
        string? result = value is JsonElement el && el.ValueKind == JsonValueKind.String
            ? el.GetString()
            : value.ToString();
        return string.IsNullOrWhiteSpace(result) ? null : result.Trim();
    }

    private static List<string> GetStringList(Dictionary<string, object> dict, string key)
    {
        if (!dict.TryGetValue(key, out var value) || value == null) return new List<string>();

        if (value is JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Array)
            {
                return element.EnumerateArray()
                    .Select(e => e.ValueKind == JsonValueKind.String ? e.GetString() : e.ToString())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s!.Trim())
                    .ToList();
            }
            if (element.ValueKind == JsonValueKind.String)
            {
                string? single = element.GetString();
                return string.IsNullOrWhiteSpace(single) ? new List<string>() : new List<string> { single.Trim() };
            }
        }

        return new List<string>();
    }

    private static Dictionary<string, object> GetSchema(Dictionary<string, object> dict, string key)
    {
        if (dict.TryGetValue(key, out var value) && value is JsonElement element
            && element.ValueKind == JsonValueKind.Object)
        {
            try
            {
                var result = JsonSerializer.Deserialize<Dictionary<string, object>>(element.GetRawText(), JsonOptions);
                return result ?? new Dictionary<string, object>();
            }
            catch
            {
                return new Dictionary<string, object>();
            }
        }
        return new Dictionary<string, object>();
    }
}
