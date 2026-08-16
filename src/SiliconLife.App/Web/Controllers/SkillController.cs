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

using SiliconLife.Collective;
using SiliconLife.App.Web;

namespace SiliconLife.App.Web.Controllers;

/// <summary>
/// Web controller for the skill management page and skill REST API.
/// All endpoints operate on the skill library of the being identified by
/// the 'beingId' parameter. Changes made here are persisted as Markdown
/// files in the being's skills/ directory (hot-reloaded by the being).
/// </summary>
[WebCode]
public class SkillController : Controller
{
    private readonly SkinManager _skinManager;
    private readonly SiliconBeingManager _beingManager;

    private static readonly JsonSerializerOptions SkillJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public SkillController()
    {
        _skinManager = ServiceLocator.Instance.GetService<SkinManager>()!;
        _beingManager = ServiceLocator.Instance.BeingManager!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/skill";

        if (path == "/skill" || path == "/skill/index")
            Index();
        else if (path == "/api/skills/list")
            List();
        else if (path == "/api/skills/get-md")
            GetMarkdown();
        else if (path == "/api/skills/update-md")
            UpdateFromMarkdown();
        else if (path == "/api/skills/import-md")
            ImportMarkdown();
        else if (path == "/api/skills/import")
            ImportJson();
        else if (path == "/api/skills/delete")
            Delete();
        else if (path == "/api/skills/export")
            ExportJson();
        else if (path == "/api/skills/export-md")
            ExportMarkdown();
        else if (path == "/api/skills/test")
            Test();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        if (!TryGetBeing(out var being))
        {
            return;
        }

        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.SkillView();
        var vm = new Models.SkillViewModel
        {
            Skin = skin,
            ActiveMenu = "beings",
            BeingId = being.Id,
            BeingName = being.Name,
            SkillCount = being.SkillManager?.SkillCount ?? 0,
            CustomSkillCount = being.SkillManager?.CustomSkillCount ?? 0,
            MaxCustomSkills = SkillManager.MaxCustomSkills,
        };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void List()
    {
        if (!TryGetBeing(out var being) || !RequireSkillManager(being, out var skillManager))
        {
            return;
        }

        try
        {
            var skills = skillManager.GetAllSkills()
                .OrderBy(s => s.Id, StringComparer.Ordinal)
                .Select(s => new
                {
                    id = s.Id,
                    description = s.Description,
                    version = s.Version,
                    tags = s.Tags,
                    source = s.Source.ToString().ToLowerInvariant(),
                    triggerMode = s.TriggerMode.ToString().ToLowerInvariant(),
                    toolWhitelist = s.ToolWhitelist,
                    maxToolRound = s.MaxToolRound,
                    timeoutSeconds = (int)s.Timeout.TotalSeconds,
                    parameterCount = s.ParameterSchema.Count,
                })
                .ToList();

            RenderJson(new { success = true, data = skills, total = skills.Count });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void GetMarkdown()
    {
        if (!TryGetBeing(out var being) || !RequireSkillManager(being, out var skillManager))
        {
            return;
        }

        string skillId = GetQueryValue("skillId");
        if (string.IsNullOrWhiteSpace(skillId))
        {
            RenderJson(new { success = false, error = "Missing skillId parameter" });
            return;
        }

        var skill = skillManager.GetSkill(skillId);
        if (skill == null)
        {
            RenderJson(new { success = false, error = $"Skill '{skillId}' not found" });
            return;
        }

        RenderJson(new { success = true, data = new { skillId = skill.Id, markdown = SkillMarkdownParser.Serialize(skill) } });
    }

    private void UpdateFromMarkdown()
    {
        if (!TryGetBeing(out var being) || !RequireSkillManager(being, out var skillManager))
        {
            return;
        }

        try
        {
            var body = GetJsonBody<MarkdownBody>();
            if (body == null || string.IsNullOrWhiteSpace(body.Markdown))
            {
                RenderJson(new { success = false, error = "Missing 'markdown' in request body" });
                return;
            }

            string? declaredId = string.IsNullOrWhiteSpace(body.SkillId) ? null : body.SkillId;
            var result = UpsertFromMarkdown(being, skillManager, body.Markdown, declaredId);
            if (result == null)
            {
                RenderJson(new { success = false, error = "Failed to parse Markdown or generate skill id — please provide an 'id' in the YAML front matter" });
                return;
            }

            RenderJson(new { success = true, data = new { skillId = result.Id, version = result.Version, message = $"Skill '{result.Id}' saved (v{result.Version})" } });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void ImportMarkdown()
    {
        if (!TryGetBeing(out var being) || !RequireSkillManager(being, out var skillManager))
        {
            return;
        }

        try
        {
            var body = GetJsonBody<MarkdownBody>();
            if (body == null || string.IsNullOrWhiteSpace(body.Markdown))
            {
                RenderJson(new { success = false, error = "Missing 'markdown' in request body" });
                return;
            }

            if (skillManager.CustomSkillCount >= SkillManager.MaxCustomSkills)
            {
                RenderJson(new { success = false, error = $"Maximum custom skill limit reached ({SkillManager.MaxCustomSkills})" });
                return;
            }

            var result = UpsertFromMarkdown(being, skillManager, body.Markdown, null);
            if (result == null)
            {
                RenderJson(new { success = false, error = "Failed to parse Markdown or generate skill id — please provide an 'id' in the YAML front matter" });
                return;
            }

            RenderJson(new { success = true, data = new { skillId = result.Id, version = result.Version, message = $"Skill '{result.Id}' imported (v{result.Version})" } });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void ImportJson()
    {
        if (!TryGetBeing(out var being) || !RequireSkillManager(being, out var skillManager))
        {
            return;
        }

        try
        {
            var body = GetJsonBody<JsonBody>();
            if (body == null || string.IsNullOrWhiteSpace(body.Json))
            {
                RenderJson(new { success = false, error = "Missing 'json' in request body" });
                return;
            }

            var skill = SkillFileManager.ImportSkill(body.Json);
            if (skill == null || string.IsNullOrWhiteSpace(skill.Id))
            {
                RenderJson(new { success = false, error = "Invalid SkillDefinition JSON" });
                return;
            }

            if (skillManager.CustomSkillCount >= SkillManager.MaxCustomSkills && skillManager.GetSkill(skill.Id) == null)
            {
                RenderJson(new { success = false, error = $"Maximum custom skill limit reached ({SkillManager.MaxCustomSkills})" });
                return;
            }

            skillManager.RegisterSkill(skill);
            if (being.Storage != null)
            {
                SkillFileManager.SaveSkillAsMarkdown(being.Storage, skill);
            }

            RenderJson(new { success = true, data = new { skillId = skill.Id, version = skill.Version, message = $"Skill '{skill.Id}' imported (v{skill.Version})" } });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void Delete()
    {
        if (!TryGetBeing(out var being) || !RequireSkillManager(being, out var skillManager))
        {
            return;
        }

        try
        {
            var body = GetJsonBody<SkillIdBody>();
            string skillId = body?.SkillId ?? "";
            if (string.IsNullOrWhiteSpace(skillId))
            {
                RenderJson(new { success = false, error = "Missing 'skillId' in request body" });
                return;
            }

            var skill = skillManager.GetSkill(skillId);
            if (skill == null)
            {
                RenderJson(new { success = false, error = $"Skill '{skillId}' not found" });
                return;
            }

            skillManager.UnregisterSkill(skillId);
            if (being.Storage != null)
            {
                SkillFileManager.DeleteSkill(being.Storage, skillId);
            }

            RenderJson(new { success = true, data = new { message = $"Skill '{skillId}' deleted" } });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void ExportJson()
    {
        if (!TryGetBeing(out var being) || !RequireSkillManager(being, out var skillManager))
        {
            return;
        }

        string skillId = GetQueryValue("skillId");
        var skill = skillManager.GetSkill(skillId);
        if (skill == null)
        {
            RenderJson(new { success = false, error = $"Skill '{skillId}' not found" });
            return;
        }

        SetHeader("Content-Disposition", $"attachment; filename=\"{skill.Id}.json\"");
        RenderText(SkillFileManager.ExportSkill(skill), "application/json; charset=utf-8");
    }

    private void ExportMarkdown()
    {
        if (!TryGetBeing(out var being) || !RequireSkillManager(being, out var skillManager))
        {
            return;
        }

        string skillId = GetQueryValue("skillId");
        var skill = skillManager.GetSkill(skillId);
        if (skill == null)
        {
            RenderJson(new { success = false, error = $"Skill '{skillId}' not found" });
            return;
        }

        SetHeader("Content-Disposition", $"attachment; filename=\"{skill.Id}.md\"");
        RenderText(SkillMarkdownParser.Serialize(skill), "text/markdown; charset=utf-8");
    }

    private void Test()
    {
        if (!TryGetBeing(out var being) || !RequireSkillManager(being, out var skillManager))
        {
            return;
        }

        try
        {
            var body = GetJsonBody<TestBody>();
            if (body == null || string.IsNullOrWhiteSpace(body.SkillId))
            {
                RenderJson(new { success = false, error = "Missing 'skillId' in request body" });
                return;
            }

            // Parameters are received as a raw JSON string and parsed here
            // (the browser prompt input cannot rely on client-side parsing).
            Dictionary<string, object>? parameters = null;
            if (!string.IsNullOrWhiteSpace(body.ParametersJson))
            {
                try
                {
                    var parsed = JsonSerializer.Deserialize<Dictionary<string, object>>(
                        body.ParametersJson, SkillJsonOptions);
                    if (parsed != null && parsed.Count > 0)
                    {
                        parameters = new Dictionary<string, object>(parsed, StringComparer.Ordinal);
                    }
                }
                catch (JsonException)
                {
                    RenderJson(new { success = false, error = "Invalid parameters JSON" });
                    return;
                }
            }

            var result = skillManager.ExecuteSkill(body.SkillId, parameters, being);
            if (result.Success)
            {
                RenderJson(new { success = true, data = new { message = result.Message } });
            }
            else
            {
                RenderJson(new { success = false, error = result.Message });
            }
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Parses Markdown into a SkillDefinition, completes missing metadata via
    /// AI when needed, and registers + persists it (update-or-insert).
    /// Skills edited through the Web UI are tagged with Source=User.
    /// </summary>
    private static SkillDefinition? UpsertFromMarkdown(
        SiliconBeingBase being,
        SkillManager skillManager,
        string markdown,
        string? declaredId)
    {
        var skill = SkillMarkdownParser.Parse(markdown);
        if (skill == null)
        {
            return null;
        }

        string targetId = string.IsNullOrWhiteSpace(declaredId) ? skill.Id : declaredId!;

        var existing = string.IsNullOrEmpty(targetId) ? null : skillManager.GetSkill(targetId);
        if (existing == null && string.IsNullOrEmpty(targetId))
        {
            // New skill without id — try AI completion first, then deterministic extraction
            if (SkillMarkdownParser.NeedsCompletion(skill))
            {
                skill = CompleteOrNull(being, markdown, skill);
            }
            if (skill == null)
            {
                return null;
            }
            targetId = skill.Id;
            existing = skillManager.GetSkill(targetId);
        }

        if (SkillMarkdownParser.NeedsCompletion(skill))
        {
            skill = CompleteOrNull(being, markdown, skill) ?? skill;
        }

        if (string.IsNullOrEmpty(skill!.Id) || string.IsNullOrEmpty(targetId))
        {
            return null;
        }

        // Resolve id and ownership
        skill = CloneWithId(skill, targetId, existing == null || existing.Source == SkillSource.Builtin ? SkillSource.User : existing.Source);

        if (existing != null && being.Storage != null)
        {
            SkillFileManager.ArchiveSkill(being.Storage, existing);
        }

        if (existing != null && existing.Id != skill.Id)
        {
            skillManager.UnregisterSkill(existing.Id);
            if (being.Storage != null)
            {
                SkillFileManager.DeleteSkill(being.Storage, existing.Id);
            }
        }

        skillManager.RegisterSkill(skill);
        if (being.Storage != null)
        {
            SkillFileManager.SaveSkillAsMarkdown(being.Storage, skill);
        }
        return skill;
    }

    private static SkillDefinition? CompleteOrNull(SiliconBeingBase being, string markdown, SkillDefinition partial)
    {
        try
        {
            var completed = SkillMetadataCompleter.Complete(being, markdown, partial);
            if (completed != null)
            {
                return completed;
            }
        }
        catch
        {
            // Fall through to deterministic id extraction
        }

        if (string.IsNullOrEmpty(partial.Id))
        {
            string? extracted = SkillMarkdownParser.ExtractIdFromBody(partial.SystemPromptTemplate ?? "");
            if (string.IsNullOrEmpty(extracted))
            {
                return null;
            }
            return CloneWithId(partial, extracted, null);
        }
        return partial;
    }

    private static SkillDefinition CloneWithId(SkillDefinition source, string id, SkillSource? newSource)
    {
        return new SkillDefinition
        {
            Id = id,
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

    private bool TryGetBeing(out SiliconBeingBase being)
    {
        being = null!;

        string beingId = GetQueryValue("beingId");
        if (string.IsNullOrWhiteSpace(beingId) || !Guid.TryParse(beingId, out Guid beingGuid))
        {
            Response.StatusCode = 400;
            RenderJson(new { success = false, error = "Missing or invalid beingId parameter" });
            return false;
        }

        being = _beingManager.GetBeing(beingGuid)!;
        if (being == null)
        {
            Response.StatusCode = 404;
            RenderJson(new { success = false, error = "Silicon being not found" });
            return false;
        }
        return true;
    }

    private bool RequireSkillManager(SiliconBeingBase being, out SkillManager skillManager)
    {
        skillManager = being.SkillManager!;
        if (skillManager == null)
        {
            RenderJson(new { success = false, error = "Skill system is not available" });
            return false;
        }
        return true;
    }

    // ===== Request bodies =====

    private class MarkdownBody
    {
        public string Markdown { get; set; } = string.Empty;
        public string? SkillId { get; set; }
    }

    private class JsonBody
    {
        public string Json { get; set; } = string.Empty;
    }

    private class SkillIdBody
    {
        public string SkillId { get; set; } = string.Empty;
    }

    private class TestBody
    {
        public string SkillId { get; set; } = string.Empty;
        public string? ParametersJson { get; set; }
    }
}
