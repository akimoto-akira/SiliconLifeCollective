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

using System.Security.Cryptography;
using System.Text;

namespace SiliconLife.Collective;

/// <summary>
/// Parses Markdown-format skill files into <see cref="SkillDefinition"/> instances.
/// Two formats are supported:
///   1. Full format: YAML front matter (metadata) + Markdown body (prompt template)
///   2. Pure Markdown format: no front matter, the entire file is the prompt template;
///      missing metadata fields are left empty and completed by the caller
///      (usually via <see cref="SkillMetadataCompleter"/>).
/// Design mirrors <see cref="SoulFileManager"/>: files are read/written through <see cref="IStorage"/>.
/// </summary>
public static class SkillMarkdownParser
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger(typeof(SkillMarkdownParser));

    /// <summary>
    /// Parses Markdown text into a SkillDefinition (best effort).
    /// 1. Tries to split YAML front matter (between --- markers) from the body.
    ///    - With YAML: metadata comes from the front matter, the body becomes SystemPromptTemplate.
    ///    - Without YAML: the whole text becomes SystemPromptTemplate, metadata fields stay empty.
    /// 2. When description is not declared, it is extracted from the first paragraph of the body.
    /// 3. When id is empty the method still returns the definition; the caller checks
    ///    <see cref="NeedsCompletion"/> to decide whether AI completion should be triggered.
    /// </summary>
    public static SkillDefinition? Parse(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return null;
        }

        try
        {
            var (frontMatter, body) = SplitFrontMatter(markdown);

            Dictionary<string, object> meta = new();
            if (!string.IsNullOrEmpty(frontMatter))
            {
                meta = ParseYaml(frontMatter);
            }
            else
            {
                // Pure Markdown mode: the entire file is the body
                body = markdown;
            }

            var skill = new SkillDefinition
            {
                Id = NormalizeScalar(meta.GetValueOrDefault("id"))?.ToString() ?? "",
                Version = NormalizeScalar(meta.GetValueOrDefault("version"))?.ToString() ?? "1.0.0",
                DisplayNameKey = NormalizeScalar(meta.GetValueOrDefault("display_name_key"))?.ToString() ?? "",
                Tags = ToStringList(meta.GetValueOrDefault("tags")),
                ToolWhitelist = ToStringList(meta.GetValueOrDefault("tool_whitelist")),
                MaxToolRound = ToInt(meta.GetValueOrDefault("max_tool_round"), 5),
                Timeout = ParseTimeSpan(NormalizeScalar(meta.GetValueOrDefault("timeout"))?.ToString() ?? "60s"),
                OnCompleteAction = NormalizeScalar(meta.GetValueOrDefault("on_complete") ?? meta.GetValueOrDefault("on_complete_action"))?.ToString() ?? "write_memory",
                Source = ParseSkillSource(NormalizeScalar(meta.GetValueOrDefault("source"))?.ToString() ?? "user"),
                TriggerMode = ParseTriggerMode(NormalizeScalar(meta.GetValueOrDefault("trigger_mode"))?.ToString() ?? "manual"),
                AutoTriggerCondition = NormalizeScalar(meta.GetValueOrDefault("auto_trigger_condition"))?.ToString(),
                SystemPromptTemplate = body.Trim(),
                Description = NormalizeScalar(meta.GetValueOrDefault("description"))?.ToString()
                    ?? ExtractFirstParagraph(body)
                    ?? "",
                ParameterSchema = new(),
                Metadata = ToMetadataDictionary(meta.GetValueOrDefault("metadata")),
            };

            // Parse parameter_schema (if declared)
            Dictionary<string, object> parameterSchema = new();
            if (meta.TryGetValue("parameter_schema", out var schema) && schema != null)
            {
                parameterSchema = ToParameterSchema(schema);
            }

            // Parse tool_action_restrictions (if declared)
            ToolActionPermissionConfig? actionRestrictions = null;
            if (meta.TryGetValue("tool_action_restrictions", out var restrictions) && restrictions != null)
            {
                actionRestrictions = ToActionRestrictions(restrictions);
            }

            if (parameterSchema.Count > 0 || actionRestrictions != null)
            {
                skill = new SkillDefinition
                {
                    Id = skill.Id,
                    Description = skill.Description,
                    DisplayNameKey = skill.DisplayNameKey,
                    Version = skill.Version,
                    Tags = skill.Tags,
                    ParameterSchema = parameterSchema,
                    SystemPromptTemplate = skill.SystemPromptTemplate,
                    ToolWhitelist = skill.ToolWhitelist,
                    ToolActionRestrictions = actionRestrictions,
                    MaxToolRound = skill.MaxToolRound,
                    Timeout = skill.Timeout,
                    OnCompleteAction = skill.OnCompleteAction,
                    Source = skill.Source,
                    TriggerMode = skill.TriggerMode,
                    AutoTriggerCondition = skill.AutoTriggerCondition,
                    Metadata = skill.Metadata,
                };
            }

            return skill;
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to parse skill Markdown: {0}", ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Checks whether a SkillDefinition needs AI metadata completion.
    /// True when any of the following holds:
    ///   - Id is empty
    ///   - Description is empty
    ///   - ParameterSchema is empty (no parameter declaration)
    /// </summary>
    public static bool NeedsCompletion(SkillDefinition skill)
    {
        return string.IsNullOrEmpty(skill.Id)
            || string.IsNullOrEmpty(skill.Description)
            || skill.ParameterSchema.Count == 0;
    }

    /// <summary>
    /// Serializes a SkillDefinition to Markdown format.
    /// Used for exporting and for saving AI-generated skills as .md files.
    /// </summary>
    public static string Serialize(SkillDefinition skill)
    {
        var yaml = BuildYamlFrontMatter(skill);
        var body = skill.SystemPromptTemplate ?? string.Empty;
        return $"---\n{yaml}\n---\n\n{body}";
    }

    /// <summary>
    /// Splits YAML front matter from the Markdown body.
    /// Supports --- or +++ as delimiters.
    /// Returns (null, original) when no front matter exists — pure Markdown mode.
    /// </summary>
    internal static (string? frontMatter, string body) SplitFrontMatter(string markdown)
    {
        string normalized = markdown.Replace("\r\n", "\n").Replace("\r", "\n");
        string[] lines = normalized.Split('\n');

        // Skip leading blank lines
        int i = 0;
        while (i < lines.Length && string.IsNullOrWhiteSpace(lines[i])) i++;

        if (i >= lines.Length) return (null, markdown);

        string first = lines[i].Trim();
        if (first != "---" && first != "+++") return (null, markdown);

        string delimiter = first;

        // Find the closing delimiter
        int end = -1;
        for (int j = i + 1; j < lines.Length; j++)
        {
            if (lines[j].Trim() == delimiter)
            {
                end = j;
                break;
            }
        }

        if (end < 0) return (null, markdown); // No closing marker — treat entire text as body

        string frontMatter = string.Join("\n", lines[(i + 1)..end]);
        string body = string.Join("\n", lines[(end + 1)..]);
        return (frontMatter, body);
    }

    /// <summary>
    /// Extracts the first paragraph of the body as the description
    /// (used when YAML does not declare one).
    /// </summary>
    private static string? ExtractFirstParagraph(string body)
    {
        var lines = body.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return lines.FirstOrDefault(l => !l.TrimStart().StartsWith('#'))?.Trim();
    }

    /// <summary>
    /// Extracts a snake_case id from the body content (fallback when AI completion fails).
    /// Uses the first heading or first line; falls back to a stable content hash
    /// when the text cannot be converted to an ASCII identifier.
    /// </summary>
    public static string? ExtractIdFromBody(string body)
    {
        var lines = body.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        string? source = lines.FirstOrDefault(l => l.TrimStart().StartsWith('#'))?.TrimStart('#', ' ')
            ?? lines.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(source)) return null;

        // Split into words and keep [a-z0-9_] parts only
        var words = source
            .ToLowerInvariant()
            .Split(new[] { ' ', '\t', ',', '.', '。', '，', ':', '：', ';', '；', '!', '！', '?', '？', '"', '\'', '(', ')', '（', '）', '{', '}', '[', ']' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(w => new string(w.Where(char.IsAsciiLetterOrDigit).ToArray()))
            .Where(w => w.Length > 0)
            .Take(6)
            .ToList();

        if (words.Count == 0)
        {
            // Non-ASCII content (e.g. Chinese) — use a stable hash suffix
            string hash = Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(body)))[..8].ToLowerInvariant();
            return $"skill_{hash}";
        }

        return string.Join("_", words);
    }

    // ===== YAML parsing (minimal subset) =====

    /// <summary>
    /// Parses a minimal YAML subset: "key: value" scalars, inline lists [a, b],
    /// block lists ("- item") and nested maps via indentation. Sufficient for
    /// skill front matter (including nested parameter_schema declarations).
    /// </summary>
    internal static Dictionary<string, object> ParseYaml(string yaml)
    {
        var lines = new List<(int Indent, string Text)>();
        foreach (var raw in yaml.Replace("\r\n", "\n").Split('\n'))
        {
            if (string.IsNullOrWhiteSpace(raw)) continue;
            if (raw.TrimStart().StartsWith('#')) continue; // comment
            int indent = raw.Length - raw.TrimStart().Length;
            lines.Add((indent, raw.Trim()));
        }

        int index = 0;
        return ParseYamlMap(lines, ref index, lines.Count > 0 ? lines[0].Indent : 0);
    }

    private static Dictionary<string, object> ParseYamlMap(List<(int Indent, string Text)> lines, ref int index, int indent)
    {
        var result = new Dictionary<string, object>();
        while (index < lines.Count)
        {
            var (lineIndent, text) = lines[index];
            if (lineIndent < indent) break;
            if (lineIndent > indent) { index++; continue; } // skip unexpected deeper line
            if (text.StartsWith("- ")) break; // list item belongs to parent list

            int colon = IndexOfUnquotedColon(text);
            if (colon <= 0) { index++; continue; }

            string key = Unquote(text[..colon].Trim());
            string valuePart = text[(colon + 1)..].Trim();

            index++;
            if (valuePart.Length == 0)
            {
                // Key with no inline value: nested map or block list follows
                if (index < lines.Count && lines[index].Indent > indent)
                {
                    if (lines[index].Text.StartsWith("- "))
                    {
                        result[key] = ParseYamlList(lines, ref index, lines[index].Indent);
                    }
                    else
                    {
                        result[key] = ParseYamlMap(lines, ref index, lines[index].Indent);
                    }
                }
                else
                {
                    result[key] = ""; // empty value
                }
            }
            else
            {
                result[key] = ParseYamlScalar(valuePart);
            }
        }
        return result;
    }

    private static List<object> ParseYamlList(List<(int Indent, string Text)> lines, ref int index, int indent)
    {
        var result = new List<object>();
        while (index < lines.Count)
        {
            var (lineIndent, text) = lines[index];
            if (lineIndent != indent || !text.StartsWith("- ")) break;

            string item = text[2..].Trim();
            index++;

            if (item.Length == 0)
            {
                result.Add("");
                continue;
            }

            int colon = IndexOfUnquotedColon(item);
            if (colon > 0)
            {
                // Inline map start inside a list item — treat "- key: value" as a nested map
                var mapLines = new List<(int Indent, string Text)> { (indent + 2, item) };
                while (index < lines.Count && lines[index].Indent > indent && !lines[index].Text.StartsWith("- "))
                {
                    mapLines.Add(lines[index]);
                    index++;
                }
                int mapIndex = 0;
                result.Add(ParseYamlMap(mapLines, ref mapIndex, indent + 2));
            }
            else
            {
                result.Add(ParseYamlScalar(item));
            }
        }
        return result;
    }

    private static object ParseYamlScalar(string value)
    {
        value = value.Trim();

        // Inline list: [a, b, c]
        if (value.StartsWith('[') && value.EndsWith(']'))
        {
            string inner = value[1..^1].Trim();
            if (inner.Length == 0) return new List<object>();
            return inner
                .Split(',')
                .Select(s => ParseYamlScalar(s.Trim()))
                .ToList();
        }

        if (value.StartsWith('"') && value.EndsWith('"') && value.Length >= 2)
        {
            return value[1..^1];
        }
        if (value.StartsWith('\'') && value.EndsWith('\'') && value.Length >= 2)
        {
            return value[1..^1];
        }

        // Strip trailing comments
        int commentIdx = value.IndexOf(" #", StringComparison.Ordinal);
        if (commentIdx >= 0) value = value[..commentIdx].Trim();

        if (value.Equals("true", StringComparison.OrdinalIgnoreCase)) return true;
        if (value.Equals("false", StringComparison.OrdinalIgnoreCase)) return false;
        if (value.Equals("null", StringComparison.OrdinalIgnoreCase) || value == "~") return "";
        if (long.TryParse(value, out long longVal)) return longVal;
        if (double.TryParse(value, System.Globalization.CultureInfo.InvariantCulture, out double doubleVal)) return doubleVal;

        return value;
    }

    private static int IndexOfUnquotedColon(string text)
    {
        bool inDouble = false, inSingle = false;
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (c == '"' && !inSingle) inDouble = !inDouble;
            else if (c == '\'' && !inDouble) inSingle = !inSingle;
            else if (c == ':' && !inDouble && !inSingle)
            {
                // A colon followed by whitespace or end-of-line is a key separator
                if (i + 1 >= text.Length || text[i + 1] == ' ') return i;
            }
        }
        return -1;
    }

    private static string Unquote(string s)
    {
        s = s.Trim();
        if (s.StartsWith('"') && s.EndsWith('"') && s.Length >= 2) return s[1..^1];
        if (s.StartsWith('\'') && s.EndsWith('\'') && s.Length >= 2) return s[1..^1];
        return s;
    }

    /// <summary>Converts a parsed YAML value into a JSON-Schema-shaped parameter dictionary.</summary>
    private static Dictionary<string, object> ToParameterSchema(object schema)
    {
        if (schema is Dictionary<string, object> dict)
        {
            var result = new Dictionary<string, object>();
            foreach (var kvp in dict)
            {
                result[kvp.Key] = kvp.Value switch
                {
                    Dictionary<string, object> nested => ToParameterSchema(nested),
                    List<object> list => list.Select(NormalizeSchemaValue).ToList(),
                    _ => NormalizeSchemaValue(kvp.Value)
                };
            }
            return result;
        }

        // Fallback: empty schema (AI completion will fill it in)
        return new Dictionary<string, object>();
    }

    private static object NormalizeSchemaValue(object value)
    {
        return value switch
        {
            Dictionary<string, object> nested => ToParameterSchema(nested),
            List<object> list => list.Select(NormalizeSchemaValue).ToList(),
            _ => value
        };
    }

    private static ToolActionPermissionConfig? ToActionRestrictions(object value)
    {
        if (value is not Dictionary<string, object> dict || dict.Count == 0) return null;

        var config = new ToolActionPermissionConfig();
        foreach (var kvp in dict)
        {
            if (kvp.Value is List<object> actions)
            {
                foreach (var action in actions)
                {
                    string? actionName = action?.ToString();
                    if (!string.IsNullOrEmpty(actionName))
                    {
                        config.DisableAction(kvp.Key, actionName);
                    }
                }
            }
        }
        return config;
    }

    private static Dictionary<string, object> ToMetadataDictionary(object value)
    {
        if (value is Dictionary<string, object> dict)
        {
            return dict.ToDictionary(k => k.Key, v => NormalizeSchemaValue(v.Value));
        }
        return new Dictionary<string, object>();
    }

    private static List<string> ToStringList(object? value)
    {
        if (value is List<object> list)
        {
            return list.Select(v => v?.ToString() ?? "").Where(s => !string.IsNullOrEmpty(s)).ToList();
        }
        if (value is string s && !string.IsNullOrEmpty(s))
        {
            return new List<string> { s };
        }
        return new List<string>();
    }

    private static int ToInt(object? value, int defaultValue)
    {
        if (value == null) return defaultValue;
        try { return Convert.ToInt32(value); }
        catch { return defaultValue; }
    }

    private static object? NormalizeScalar(object? value)
    {
        return value switch
        {
            null => null,
            string s => s,
            _ => value.ToString()
        };
    }

    /// <summary>Parses "90s" / "2m" / "1h" / "00:01:30" style time spans.</summary>
    public static TimeSpan ParseTimeSpan(string text)
    {
        text = text.Trim().ToLowerInvariant();
        if (string.IsNullOrEmpty(text)) return TimeSpan.FromSeconds(60);

        if (text.EndsWith("s")) return Seconds(double.Parse(TrimUnit(text)));
        if (text.EndsWith("m")) return TimeSpan.FromMinutes(double.Parse(TrimUnit(text)));
        if (text.EndsWith("h")) return TimeSpan.FromHours(double.Parse(TrimUnit(text)));
        if (TimeSpan.TryParse(text, out var ts)) return ts;

        return TimeSpan.FromSeconds(60);

        static string TrimUnit(string s) => s[..^1].Trim();
        static TimeSpan Seconds(double v) => TimeSpan.FromSeconds(v);
    }

    private static SkillSource ParseSkillSource(string? text)
    {
        return text?.Trim().ToLowerInvariant() switch
        {
            "builtin" => SkillSource.Builtin,
            "plugin" => SkillSource.Plugin,
            "being" => SkillSource.Being,
            "user" => SkillSource.User,
            _ => SkillSource.User,
        };
    }

    private static SkillTriggerMode ParseTriggerMode(string? text)
    {
        return text?.Trim().ToLowerInvariant() switch
        {
            "auto" => SkillTriggerMode.Auto,
            "manual" => SkillTriggerMode.Manual,
            _ => SkillTriggerMode.Manual,
        };
    }

    // ===== YAML serialization =====

    /// <summary>Builds the YAML front matter block for a SkillDefinition.</summary>
    internal static string BuildYamlFrontMatter(SkillDefinition skill)
    {
        var sb = new StringBuilder();
        sb.Append("id: ").Append(EscapeYaml(skill.Id));
        sb.Append("\nversion: ").Append(EscapeYaml(skill.Version));

        if (!string.IsNullOrEmpty(skill.DisplayNameKey))
        {
            sb.Append("\ndisplay_name_key: ").Append(EscapeYaml(skill.DisplayNameKey));
        }
        if (!string.IsNullOrEmpty(skill.Description))
        {
            sb.Append("\ndescription: ").Append(EscapeYaml(skill.Description));
        }
        if (skill.Tags.Count > 0)
        {
            sb.Append("\ntags: [").Append(string.Join(", ", skill.Tags.Select(EscapeYaml))).Append(']');
        }
        if (skill.ToolWhitelist.Count > 0)
        {
            sb.Append("\ntool_whitelist: [").Append(string.Join(", ", skill.ToolWhitelist)).Append(']');
        }
        if (skill.ToolActionRestrictions != null && skill.ToolActionRestrictions.DisabledActions.Count > 0)
        {
            sb.Append("\ntool_action_restrictions:");
            foreach (var kvp in skill.ToolActionRestrictions.DisabledActions)
            {
                sb.Append("\n  ").Append(kvp.Key).Append(':');
                foreach (var action in kvp.Value)
                {
                    sb.Append("\n    - ").Append(action);
                }
            }
        }

        sb.Append("\nmax_tool_round: ").Append(skill.MaxToolRound);
        sb.Append("\ntimeout: ").Append(SerializeTimeSpan(skill.Timeout));
        sb.Append("\non_complete: ").Append(EscapeYaml(skill.OnCompleteAction));
        sb.Append("\nsource: ").Append(skill.Source.ToString().ToLowerInvariant());
        sb.Append("\ntrigger_mode: ").Append(skill.TriggerMode.ToString().ToLowerInvariant());

        if (!string.IsNullOrEmpty(skill.AutoTriggerCondition))
        {
            sb.Append("\nauto_trigger_condition: ").Append(EscapeYaml(skill.AutoTriggerCondition!));
        }
        if (skill.Metadata.Count > 0)
        {
            sb.Append("\nmetadata:");
            AppendYamlMap(sb, skill.Metadata, 2);
        }
        if (skill.ParameterSchema.Count > 0)
        {
            sb.Append("\nparameter_schema:");
            AppendYamlMap(sb, skill.ParameterSchema, 2);
        }

        return sb.ToString();
    }

    private static void AppendYamlMap(StringBuilder sb, Dictionary<string, object> map, int indent)
    {
        string pad = new(' ', indent);
        foreach (var kvp in map)
        {
            switch (kvp.Value)
            {
                case Dictionary<string, object> nested:
                    sb.Append('\n').Append(pad).Append(kvp.Key).Append(':');
                    AppendYamlMap(sb, nested, indent + 2);
                    break;
                case List<object> list:
                    sb.Append('\n').Append(pad).Append(kvp.Key).Append(':');
                    foreach (var item in list)
                    {
                        sb.Append('\n').Append(pad).Append("  - ").Append(FormatYamlScalar(item));
                    }
                    break;
                default:
                    sb.Append('\n').Append(pad).Append(kvp.Key).Append(": ").Append(FormatYamlScalar(kvp.Value));
                    break;
            }
        }
    }

    private static string FormatYamlScalar(object value)
    {
        return value switch
        {
            bool b => b ? "true" : "false",
            null => "null",
            string s => EscapeYaml(s),
            _ => value.ToString() ?? ""
        };
    }

    private static string EscapeYaml(string s)
    {
        if (s.Contains(':') || s.Contains('#') || s.Contains('"') || s.StartsWith(' ') || s.EndsWith(' '))
        {
            return "\"" + s.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
        }
        return s;
    }

    private static string SerializeTimeSpan(TimeSpan timeout)
    {
        if (timeout.TotalSeconds > 0 && Math.Abs(timeout.TotalSeconds - Math.Round(timeout.TotalSeconds)) < 0.001
            && timeout.TotalSeconds < 3600)
        {
            return $"{(int)timeout.TotalSeconds}s";
        }
        return timeout.ToString();
    }
}
