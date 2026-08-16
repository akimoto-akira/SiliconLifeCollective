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
using System.Text.Json.Serialization;

namespace SiliconLife.Collective;

/// <summary>
/// Persists skill definitions to the being's key-value storage (<see cref="IStorage"/>),
/// at the same level as soul.md / state.json. Supports both JSON and Markdown formats.
/// Markdown is the preferred format; .md files take precedence over same-id .json files.
/// </summary>
public static class SkillFileManager
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger(typeof(SkillFileManager));
    private const string SkillsDirectory = "skills";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>Saves a skill definition as a JSON file.</summary>
    public static bool SaveSkill(IStorage storage, SkillDefinition skill)
    {
        try
        {
            string path = $"{SkillsDirectory}/{skill.Id}.json";
            storage.Write(path, JsonSerializer.Serialize(skill, JsonOptions));
            _logger.Info(null, "Skill '{0}' saved as JSON", skill.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to save skill '{0}' as JSON: {1}", skill.Id, ex.Message);
            return false;
        }
    }

    /// <summary>Saves a skill definition as a Markdown file (preferred format).</summary>
    public static bool SaveSkillAsMarkdown(IStorage storage, SkillDefinition skill)
    {
        try
        {
            string path = $"{SkillsDirectory}/{skill.Id}.md";
            string markdown = SkillMarkdownParser.Serialize(skill);
            storage.Write(path, markdown);
            _logger.Info(null, "Skill '{0}' saved as Markdown", skill.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to save skill '{0}' as Markdown: {1}", skill.Id, ex.Message);
            return false;
        }
    }

    /// <summary>Loads a skill definition from its Markdown file (null if not present).</summary>
    public static SkillDefinition? LoadSkillFromMarkdown(IStorage storage, string skillId)
    {
        string path = $"{SkillsDirectory}/{skillId}.md";
        if (!storage.Exists(path)) return null;
        string markdown = storage.Read<string>(path).FirstOrDefault() ?? "";
        return SkillMarkdownParser.Parse(markdown);
    }

    /// <summary>Loads a skill definition from its JSON file (null if not present).</summary>
    public static SkillDefinition? LoadSkillFromJson(IStorage storage, string skillId)
    {
        string path = $"{SkillsDirectory}/{skillId}.json";
        if (!storage.Exists(path)) return null;
        try
        {
            string json = storage.Read<string>(path).FirstOrDefault() ?? "";
            return JsonSerializer.Deserialize<SkillDefinition>(json, JsonOptions);
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to load skill '{0}' from JSON: {1}", skillId, ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Loads all persisted skills (scanning both .md and .json files).
    /// .md files take precedence over same-id .json files (Markdown is the preferred format).
    /// </summary>
    public static List<SkillDefinition> LoadAllSkills(IStorage storage)
    {
        var result = new List<SkillDefinition>();
        var seen = new HashSet<string>();

        try
        {
            // Load .md files first (preferred format)
            foreach (var path in storage.ListKeys($"{SkillsDirectory}/"))
            {
                if (!path.EndsWith(".md", StringComparison.OrdinalIgnoreCase)) continue;
                // Skip archive files
                if (path.Contains($"{SkillsDirectory}/archive/", StringComparison.OrdinalIgnoreCase)) continue;

                string markdown = storage.Read<string>(path).FirstOrDefault() ?? "";
                if (SkillMarkdownParser.Parse(markdown) is { } skill && !string.IsNullOrEmpty(skill.Id))
                {
                    result.Add(skill);
                    seen.Add(skill.Id);
                }
                else
                {
                    _logger.Warn(null, "Skipping invalid skill Markdown file: {0}", path);
                }
            }

            // Load .json files (skip ids already covered by .md files)
            foreach (var path in storage.ListKeys($"{SkillsDirectory}/"))
            {
                if (!path.EndsWith(".json", StringComparison.OrdinalIgnoreCase)) continue;
                if (path.Contains($"{SkillsDirectory}/archive/", StringComparison.OrdinalIgnoreCase)) continue;

                string fileName = path.Split('/').Last();
                string id = fileName[..^".json".Length];
                if (seen.Contains(id)) continue;

                var skill = LoadSkillFromJson(storage, id);
                if (skill != null && !string.IsNullOrEmpty(skill.Id))
                {
                    result.Add(skill);
                    seen.Add(skill.Id);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to list skills from storage: {0}", ex.Message);
        }

        return result;
    }

    /// <summary>Deletes a skill's files (both .md and .json). Returns true if anything was deleted.</summary>
    public static bool DeleteSkill(IStorage storage, string skillId)
    {
        string mdPath = $"{SkillsDirectory}/{skillId}.md";
        string jsonPath = $"{SkillsDirectory}/{skillId}.json";
        bool deleted = false;
        if (storage.Exists(mdPath)) { storage.Delete(mdPath); deleted = true; }
        if (storage.Exists(jsonPath)) { storage.Delete(jsonPath); deleted = true; }
        return deleted;
    }

    /// <summary>Exports a skill definition as a JSON string (for sharing between beings).</summary>
    public static string ExportSkill(SkillDefinition skill)
    {
        return JsonSerializer.Serialize(skill, JsonOptions);
    }

    /// <summary>Imports a skill definition from a JSON string (null on parse failure).</summary>
    public static SkillDefinition? ImportSkill(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<SkillDefinition>(json, JsonOptions);
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to import skill from JSON: {0}", ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Archives the current version of a skill before an update,
    /// to skills/archive/{id}/{version}.md (skill evolution history).
    /// </summary>
    public static void ArchiveSkill(IStorage storage, SkillDefinition skill)
    {
        try
        {
            if (string.IsNullOrEmpty(skill.Id)) return;
            string archivePath = $"{SkillsDirectory}/archive/{skill.Id}/{skill.Version}.md";
            string markdown = SkillMarkdownParser.Serialize(skill);
            storage.Write(archivePath, markdown);
            _logger.Debug(null, "Skill '{0}' v{1} archived", skill.Id, skill.Version);
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to archive skill '{0}': {1}", skill.Id, ex.Message);
        }
    }

    /// <summary>
    /// Computes a lightweight fingerprint of the skills directory contents,
    /// used by <see cref="SkillManager.RefreshFromStorage"/> for hot-reload change detection.
    /// </summary>
    internal static string ComputeDirectoryFingerprint(IStorage storage)
    {
        var sb = new System.Text.StringBuilder();
        foreach (var path in storage.ListKeys($"{SkillsDirectory}/").OrderBy(p => p, StringComparer.Ordinal))
        {
            sb.Append(path).Append('|');
            string content = storage.Read<string>(path).FirstOrDefault() ?? "";
            sb.Append(content.Length).Append('|');
            // Cheap content checksum: XOR of char codes weighted by position
            int hash = 17;
            foreach (char c in content) hash = unchecked(hash * 31 + c);
            sb.Append(hash).Append('\n');
        }
        return sb.ToString();
    }
}
