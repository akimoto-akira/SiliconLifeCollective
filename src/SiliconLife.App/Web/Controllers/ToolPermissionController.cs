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

using System.Text.Json;
using SiliconLife.Collective;
using SiliconLife.Common.Localization;
using SiliconLife.Common.SiliconBeing;

namespace SiliconLife.App.Web.Controllers;

/// <summary>
/// Controller for tool action permission management API endpoints.
/// Provides CRUD operations for both being-level and project-level tool permissions,
/// as well as permission templates.
/// </summary>
[WebCode]
public class ToolPermissionController : Controller
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<ToolPermissionController>();
    private readonly SiliconBeingManager _beingManager;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };

    public ToolPermissionController()
    {
        var locator = ServiceLocator.Instance;
        _beingManager = locator.BeingManager!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "";

        if (path == "/api/beings/tool-permissions")
            HandleBeingToolPermissions();
        else if (path == "/api/beings/tool-permissions/templates")
            GetTemplates();
        else if (path == "/api/beings/tool-permissions/apply-template")
            ApplyTemplate();
        else if (path.StartsWith("/api/projects/") && path.EndsWith("/tool-permissions"))
            HandleProjectToolPermissions();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    /// <summary>
    /// GET /api/beings/tool-permissions?beingId=xxx — Get being's tool permission matrix
    /// PUT /api/beings/tool-permissions?beingId=xxx — Update being's global tool permissions
    /// </summary>
    private void HandleBeingToolPermissions()
    {
        if (Request.HttpMethod == "GET")
        {
            GetBeingToolPermissions();
        }
        else if (Request.HttpMethod == "PUT")
        {
            UpdateBeingToolPermissions();
        }
        else
        {
            Response.StatusCode = 405;
            RenderJson(new { success = false, error = "Method not allowed" });
        }
    }

    /// <summary>
    /// GET: Returns the tool permission matrix for a specific being.
    /// The matrix includes: for each tool with ToolActionAttribute,
    /// the list of all declared actions and their enabled/disabled status.
    /// </summary>
    private void GetBeingToolPermissions()
    {
        string? beingIdStr = GetQueryParam("beingId");
        if (!Guid.TryParse(beingIdStr, out Guid beingId))
        {
            RenderJson(new { success = false, error = "Missing or invalid beingId parameter" });
            return;
        }

        var being = _beingManager.GetBeing(beingId);
        if (being == null)
        {
            RenderJson(new { success = false, error = "Being not found" });
            return;
        }

        var toolManager = being.ToolManager;
        if (toolManager == null)
        {
            RenderJson(new { success = false, error = "Being has no tool manager" });
            return;
        }

        var allActions = toolManager.GetAllDeclaredActions();
        var permissions = being.ToolActionPermissions;
        var currentLanguage = Config.Instance.Data.Language;
        var matrix = new List<object>();

        foreach (var kvp in allActions)
        {
            string toolName = kvp.Key;
            string[] actions = kvp.Value;
            var tool = toolManager.GetTool(toolName);
            string displayName = tool?.GetDisplayName(currentLanguage) ?? toolName;
            var actionList = actions.Select(a => new
            {
                name = a,
                enabled = permissions?.IsActionAllowed(toolName, a) ?? true
            }).ToList();

            matrix.Add(new
            {
                toolName,
                displayName,
                actions = actionList
            });
        }

        RenderJson(new
        {
            success = true,
            beingId = beingId.ToString(),
            beingName = being.Name,
            permissions = matrix
        });
    }

    /// <summary>
    /// PUT: Updates the being's global tool action permissions.
    /// Body: { "beingId": "...", "permissions": { "toolName": ["disabledAction1", "disabledAction2"], ... } }
    /// </summary>
    private void UpdateBeingToolPermissions()
    {
        string? beingIdStr = GetQueryParam("beingId");
        if (!Guid.TryParse(beingIdStr, out Guid beingId))
        {
            RenderJson(new { success = false, error = "Missing or invalid beingId parameter" });
            return;
        }

        var being = _beingManager.GetBeing(beingId);
        if (being == null)
        {
            RenderJson(new { success = false, error = "Being not found" });
            return;
        }

        try
        {
            string body = new System.IO.StreamReader(Request.InputStream).ReadToEnd();
            var requestData = JsonSerializer.Deserialize<UpdatePermissionsRequest>(body, _jsonOptions);
            if (requestData == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            var config = new ToolActionPermissionConfig();
            if (requestData.DisabledActions != null)
            {
                foreach (var kvp in requestData.DisabledActions)
                {
                    foreach (var action in kvp.Value)
                    {
                        config.DisableAction(kvp.Key, action);
                    }
                }
            }

            being.ToolActionPermissions = config;

            // Persist to state
            if (being is DefaultSiliconBeing defaultBeing)
            {
                defaultBeing.SaveState();
            }

            _logger.Info(beingId, "Updated tool action permissions for being {0}", being.Name);
            RenderJson(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.Error(beingId, "Failed to update tool permissions: {0}", ex.Message);
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// GET /api/beings/tool-permissions/templates — Get preset permission templates
    /// </summary>
    private void GetTemplates()
    {
        var templates = new[]
        {
            new
            {
                id = "chat_only",
                name = "Only Chat",
                nameZh = "只聊天",
                description = "Only allow sending and reading chat messages",
                descriptionZh = "只允许发送和阅读聊天消息",
                disabledActions = GetChatOnlyDisabledActions()
            },
            new
            {
                id = "task_execution",
                name = "Task Execution Only",
                nameZh = "只执行任务",
                description = "Allow task management, memory, and chat, but deny file/disk/network operations",
                descriptionZh = "允许任务管理、记忆和聊天，但禁止文件/磁盘/网络操作",
                disabledActions = GetTaskExecutionDisabledActions()
            },
            new
            {
                id = "full_control",
                name = "Full Control",
                nameZh = "完全控制",
                description = "All actions allowed (remove all restrictions)",
                descriptionZh = "所有动作均允许（移除所有限制）",
                disabledActions = new Dictionary<string, string[]>()
            }
        };

        RenderJson(new { success = true, templates });
    }

    /// <summary>
    /// POST /api/beings/tool-permissions/apply-template — Apply a preset template
    /// Body: { "beingId": "...", "templateId": "..." } or { "beingIds": ["..."], "templateId": "..." }
    /// </summary>
    private void ApplyTemplate()
    {
        try
        {
            string body = new System.IO.StreamReader(Request.InputStream).ReadToEnd();
            var requestData = JsonSerializer.Deserialize<ApplyTemplateRequest>(body, _jsonOptions);
            if (requestData == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            var beingIds = new List<Guid>();
            if (!string.IsNullOrEmpty(requestData.BeingId) && Guid.TryParse(requestData.BeingId, out Guid singleId))
            {
                beingIds.Add(singleId);
            }
            else if (requestData.BeingIds != null)
            {
                foreach (var idStr in requestData.BeingIds)
                {
                    if (Guid.TryParse(idStr, out Guid id))
                        beingIds.Add(id);
                }
            }

            if (beingIds.Count == 0)
            {
                RenderJson(new { success = false, error = "No valid being IDs provided" });
                return;
            }

            var config = GetConfigFromTemplateId(requestData.TemplateId);
            int successCount = 0;
            var errors = new List<string>();

            foreach (var beingId in beingIds)
            {
                var being = _beingManager.GetBeing(beingId);
                if (being == null)
                {
                    errors.Add($"Being {beingId} not found");
                    continue;
                }

                being.ToolActionPermissions = config;
                if (being is DefaultSiliconBeing defaultBeing)
                {
                    defaultBeing.SaveState();
                }
                successCount++;
            }

            _logger.Info(null, "Applied template '{0}' to {1} being(s)", requestData.TemplateId, successCount);
            RenderJson(new { success = true, appliedCount = successCount, errors });
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to apply template: {0}", ex.Message);
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// GET/PUT /api/projects/{id}/tool-permissions
    /// Project-level tool permissions are a single unified config (not per-being).
    /// At runtime, effective permissions = BeingGlobalDisabled ∪ ProjectDisabled.
    /// </summary>
    private void HandleProjectToolPermissions()
    {
        var path = Request.Url?.AbsolutePath ?? "";
        // Extract project ID from path: /api/projects/{projectId}/tool-permissions
        var parts = path.Split('/');
        if (parts.Length < 4 || !Guid.TryParse(parts[3], out Guid projectId))
        {
            RenderJson(new { success = false, error = "Invalid project ID" });
            return;
        }

        var projectManager = ServiceLocator.Instance.ProjectManager;
        if (projectManager == null)
        {
            RenderJson(new { success = false, error = "Project manager not available" });
            return;
        }

        var project = projectManager.GetProject(projectId);
        if (project == null)
        {
            RenderJson(new { success = false, error = "Project not found" });
            return;
        }

        if (Request.HttpMethod == "GET")
        {
            GetProjectToolPermissions(project);
        }
        else if (Request.HttpMethod == "PUT")
        {
            UpdateProjectToolPermissions(project);
        }
        else
        {
            Response.StatusCode = 405;
            RenderJson(new { success = false, error = "Method not allowed" });
        }
    }

    /// <summary>
    /// GET: Get project-level tool permission matrix.
    /// Returns the full tool/action matrix with the project-level enabled/disabled status.
    /// No being-specific info — project permissions are unified.
    /// </summary>
    private void GetProjectToolPermissions(ProjectSpace project)
    {
        var allActions = GetAllDeclaredActionsFromAnyBeing();
        var permissions = project.ToolActionPermissions;
        var currentLanguage = Config.Instance.Data.Language;
        var matrix = new List<object>();

        foreach (var kvp in allActions)
        {
            string toolName = kvp.Key;
            string[] actions = kvp.Value;
            string displayName = GetToolDisplayName(toolName, currentLanguage);
            var actionList = actions.Select(a => new
            {
                name = a,
                enabled = permissions?.IsActionAllowed(toolName, a) ?? true
            }).ToList();

            matrix.Add(new
            {
                toolName,
                displayName,
                actions = actionList
            });
        }

        RenderJson(new
        {
            success = true,
            projectId = project.Id.ToString(),
            projectName = project.Name,
            permissions = matrix
        });
    }

    /// <summary>
    /// PUT: Update project-level tool permissions.
    /// Body: { "disabledActions": { "toolName": ["action1", "action2"] } }
    /// </summary>
    private void UpdateProjectToolPermissions(ProjectSpace project)
    {
        try
        {
            string body = new System.IO.StreamReader(Request.InputStream).ReadToEnd();
            var requestData = JsonSerializer.Deserialize<UpdateProjectPermissionsRequest>(body, _jsonOptions);
            if (requestData == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            var config = new ToolActionPermissionConfig();
            if (requestData.DisabledActions != null)
            {
                foreach (var kvp in requestData.DisabledActions)
                {
                    foreach (var action in kvp.Value)
                    {
                        config.DisableAction(kvp.Key, action);
                    }
                }
            }

            project.ToolActionPermissions = config.GetRestrictedToolNames().Count == 0 ? null : config;
            project.UpdatedAt = DateTime.UtcNow;

            _logger.Info(null, "Updated project tool permissions for project {0}", project.Name);
            RenderJson(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to update project tool permissions: {0}", ex.Message);
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Gets all declared tool actions by borrowing from any available being's tool manager.
    /// Tool declarations are the same across all beings (they scan the same assemblies).
    /// </summary>
    private Dictionary<string, string[]> GetAllDeclaredActionsFromAnyBeing()
    {
        var beings = _beingManager.GetAllBeings();
        foreach (var being in beings)
        {
            if (being.ToolManager != null)
            {
                return being.ToolManager.GetAllDeclaredActions();
            }
        }
        return new Dictionary<string, string[]>();
    }

    /// <summary>
    /// Gets the localized display name for a tool by searching all beings.
    /// </summary>
    private string GetToolDisplayName(string toolName, Language language)
    {
        var beings = _beingManager.GetAllBeings();
        foreach (var being in beings)
        {
            var tool = being.ToolManager?.GetTool(toolName);
            if (tool != null)
            {
                return tool.GetDisplayName(language) ?? toolName;
            }
        }
        return toolName;
    }

    // ===== Template Helpers =====

    private static Dictionary<string, string[]> GetChatOnlyDisabledActions()
    {
        // Everything except chat's send and mark_read actions is disabled
        return new Dictionary<string, string[]>
        {
            ["task"] = new[] { "create", "list", "get", "complete", "fail", "cancel", "delete", "update_priority", "add_dependency", "submit_for_review", "stats" },
            ["timer"] = new[] { "create_once", "create_recurring", "list", "get", "pause", "resume", "cancel", "delete", "stats", "tick" },
            ["memory"] = new[] { "add", "query", "stats" },
            ["disk"] = new[] { "read_file", "write_file", "list_directory", "delete_file", "create_directory", "exists", "get_file_info", "count_lines", "read_lines", "clear_file", "replace_lines", "replace_text", "replace_text_all", "list_drives", "search_files", "search_content" },
            ["network"] = new[] { "GET", "POST" },
            ["system"] = new[] { "list_processes", "find_process", "get_env", "get_env_all", "system_info", "resource_usage" },
            ["knowledge"] = new[] { "add", "query", "update", "delete", "search", "get_path", "get_neighbors", "get_degree", "degree_distribution", "traverse", "has_cycle", "validate", "stats" },
            ["work_note"] = new[] { "create", "read", "update", "delete", "list", "directory", "search" },
            ["dynamic_compile"] = new[] { "compile", "save", "self_replace", "activate", "preview_saved", "clear_saved" },
            ["calendar"] = new[] { "now", "format", "add_days", "diff", "list_calendars", "get_components", "get_now_components", "convert" },
            ["webview_browser"] = new[] { "open", "close", "navigate", "click", "input", "scroll", "execute_script", "get_page_text", "get_screenshot", "wait_for_element", "get_element_info", "upload_file", "get_browser_status", "set_timeout", "clear_session" },
            ["log"] = new[] { "query_operations", "query_tool_calls", "query_conversations", "export", "get_system_info" },
            ["project_task"] = new[] { "create", "list", "get", "update", "assign", "remove_assignee", "start", "complete", "fail", "cancel", "delete", "stats" },
            ["project_work_note"] = new[] { "create", "read", "update", "delete", "list", "directory", "search" }
        };
    }

    private static Dictionary<string, string[]> GetTaskExecutionDisabledActions()
    {
        // Deny dangerous file/disk/network operations, but allow task, chat, memory, etc.
        return new Dictionary<string, string[]>
        {
            ["disk"] = new[] { "write_file", "delete_file", "clear_file", "replace_lines", "replace_text", "replace_text_all" },
            ["network"] = new[] { "POST" },
            ["system"] = new[] { "list_processes", "find_process", "get_env_all", "resource_usage" },
            ["dynamic_compile"] = new[] { "save", "self_replace", "activate", "clear_saved" },
            ["webview_browser"] = new[] { "execute_script", "upload_file" }
        };
    }

    private static ToolActionPermissionConfig GetConfigFromTemplateId(string? templateId)
    {
        var config = new ToolActionPermissionConfig();
        var disabledMap = templateId switch
        {
            "chat_only" => GetChatOnlyDisabledActions(),
            "task_execution" => GetTaskExecutionDisabledActions(),
            "full_control" => new Dictionary<string, string[]>(),
            _ => new Dictionary<string, string[]>()
        };

        foreach (var kvp in disabledMap)
        {
            foreach (var action in kvp.Value)
            {
                config.DisableAction(kvp.Key, action);
            }
        }

        return config;
    }

    // ===== Request Models =====

    private class UpdatePermissionsRequest
    {
        public string? BeingId { get; set; }
        public Dictionary<string, string[]>? DisabledActions { get; set; }
    }

    private class UpdateProjectPermissionsRequest
    {
        public Dictionary<string, string[]>? DisabledActions { get; set; }
    }

    private class ApplyTemplateRequest
    {
        public string? BeingId { get; set; }
        public List<string>? BeingIds { get; set; }
        public string? TemplateId { get; set; }
    }

    // ===== Utility Methods =====

    private string? GetQueryParam(string name)
    {
        var query = Request.Url?.Query ?? "";
        var pairs = query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries);
        foreach (var pair in pairs)
        {
            var parts = pair.Split('=', 2);
            if (parts[0] == name && parts.Length > 1)
                return Uri.UnescapeDataString(parts[1]);
        }
        return null;
    }
}
