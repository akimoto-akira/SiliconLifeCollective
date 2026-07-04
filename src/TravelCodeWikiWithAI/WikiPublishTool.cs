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

using SiliconLife.Collective;
using SiliconLife.Common;
using TravelCodeWikiWithAI.Data;
using TravelCodeWikiWithAI.Services;

namespace TravelCodeWikiWithAI;

/// <summary>
/// Wiki 发布工具 — 将生成的 wiki 页面发布到 MediaWiki 站点
/// Wiki publish tool — publishes generated wiki pages to MediaWiki site
/// 
/// 封装 MediaWikiPublishService 的调用能力，使硅基人能发布地理实体文档。
/// Encapsulates MediaWikiPublishService invocation, enabling silicon beings to publish geo entity documents.
/// 
/// 对应7步流程：步骤7（发布到MediaWiki）
/// Corresponds to 7-step workflow: Step 7 (publish to MediaWiki)
/// </summary>
[ToolAction("publish", "test_connection", "status")]
[ToolScenario(ToolScenarioFlag.Chat | ToolScenarioFlag.Task)]
public class WikiPublishTool : ITool
{
    public string Name => "wiki_publish";

    public string Description =>
        "Wiki publish tool for publishing generated wiki pages to a MediaWiki site. " +
        "Use 'publish' to publish a geo entity's documents, " +
        "'test_connection' to test MediaWiki site connectivity, " +
        "'status' to check publish service configuration status.";

    public string[] Actions => new[] { "publish", "test_connection", "status" };

    public string GetDisplayName(Language language) => language switch
    {
        Language.ZhCN or Language.ZhSG or Language.ZhMY => "Wiki发布工具",
        Language.ZhHK or Language.ZhMO or Language.ZhTW => "Wiki發佈工具",
        Language.JaJP => "Wiki公開ツール",
        Language.KoKR => "Wiki 게시 도구",
        _ => "Wiki Publish Tool"
    };

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
                    ["enum"] = Actions,
                    ["description"] = "Action: publish | test_connection | status"
                },
                ["entity_path"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Full ID path of the geo entity to publish (e.g., 'world/CN/BJ'). Required for 'publish' action."
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out var actionObj) || actionObj is not string action)
        {
            return ToolResult.Failed("Missing required parameter: action");
        }

        return action switch
        {
            "publish" => ExecutePublish(callerId, parameters),
            "test_connection" => ExecuteTestConnection(callerId),
            "status" => ExecuteStatus(),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    private ToolResult ExecutePublish(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("entity_path", out var epObj) || epObj is not string entityPath || string.IsNullOrEmpty(entityPath))
        {
            return ToolResult.Failed("Missing required parameter: entity_path");
        }

        var service = TravelCodeWikiWithAIPlugin._publishService;
        if (service == null)
        {
            return ToolResult.Failed("Publish service not available");
        }

        if (!service.IsEnabled)
        {
            return ToolResult.Failed("Publish service is not configured (missing API URL or credentials). Use 'status' action to check configuration.");
        }

        var project = TravelCodeWikiWithAIPlugin._geoProject;
        if (project == null)
        {
            return ToolResult.Failed("Geo project data not available");
        }

        var entity = project.GetObject(entityPath) as GeoLocation;
        if (entity == null)
        {
            return ToolResult.Failed($"Entity not found: {entityPath}");
        }

        service.SetCallerId(callerId);
        var result = service.PublishEntity(entity);

        return result.Success
            ? ToolResult.Successful(
                $"Published {entityPath}: {result.PagesPublished} pages, {result.FilesUploaded} files. " +
                $"({result.PagesSkipped} pages skipped, {result.FilesSkipped} files skipped)",
                new Dictionary<string, object?>
                {
                    ["entity_path"] = entityPath,
                    ["pages_published"] = result.PagesPublished,
                    ["pages_skipped"] = result.PagesSkipped,
                    ["pages_failed"] = result.PagesFailed,
                    ["files_uploaded"] = result.FilesUploaded,
                    ["files_skipped"] = result.FilesSkipped,
                    ["files_failed"] = result.FilesFailed,
                    ["errors"] = result.Errors.Count > 0 ? result.Errors : null
                })
            : ToolResult.Failed(
                $"Failed to publish {entityPath}: {string.Join("; ", result.Errors)}");
    }

    private ToolResult ExecuteTestConnection(Guid callerId)
    {
        var service = TravelCodeWikiWithAIPlugin._publishService;
        if (service == null)
        {
            return ToolResult.Failed("Publish service not available");
        }

        if (!service.IsEnabled)
        {
            return ToolResult.Failed("Publish service is not configured");
        }

        service.SetCallerId(callerId);
        bool connected = service.TestConnection();

        return connected
            ? ToolResult.Successful("MediaWiki site connection successful", new Dictionary<string, object?> { ["connected"] = true })
            : ToolResult.Failed("MediaWiki site connection failed");
    }

    private ToolResult ExecuteStatus()
    {
        var service = TravelCodeWikiWithAIPlugin._publishService;
        if (service == null)
        {
            return ToolResult.Successful("Publish service not initialized", new Dictionary<string, object?>
            {
                ["initialized"] = false,
                ["enabled"] = false
            });
        }

        return ToolResult.Successful(
            service.IsEnabled ? "Publish service is configured and enabled" : "Publish service is not configured",
            new Dictionary<string, object?>
            {
                ["initialized"] = true,
                ["enabled"] = service.IsEnabled,
                ["api_url_configured"] = !string.IsNullOrEmpty(service.ApiUrl),
                ["username_configured"] = !string.IsNullOrEmpty(service.Username)
            });
    }
}
