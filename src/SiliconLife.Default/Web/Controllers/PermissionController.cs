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

using SiliconLife.Collective;

namespace SiliconLife.Default.Web;

[WebCode]
public class PermissionController : Controller
{
    private readonly SkinManager _skinManager;

    public PermissionController()
    {
        _skinManager = ServiceLocator.Instance.GetService<SkinManager>()!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/permissions";

        if (path == "/permissions" || path == "/permissions/index")
            Index();
        else if (path == "/api/permissions/list")
            GetList();
        else if (path == "/api/permissions/save")
            Save();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        // 强制验证硅基人ID
        var beingIdStr = Request.QueryString?["beingId"];
        if (string.IsNullOrEmpty(beingIdStr) || !Guid.TryParse(beingIdStr, out Guid beingId))
        {
            Response.StatusCode = 400;
            var loc = LocalizationManager.Instance.GetLocalization(Config.Instance?.Data?.Language ?? Language.ZhCN);
            string errorMsg = "缺少硅基人ID参数";
            if (loc is DefaultLocalizationBase defaultLoc)
            {
                errorMsg = defaultLoc.PermissionMissingBeingId;
            }
            RenderHtml($"<html><body><h1>{errorMsg}</h1></body></html>");
            return;
        }

        // 验证硅基人是否存在
        var beingManager = ServiceLocator.Instance.BeingManager;
        if (beingManager == null || beingManager.GetBeing(beingId) == null)
        {
            Response.StatusCode = 404;
            var loc = LocalizationManager.Instance.GetLocalization(Config.Instance?.Data?.Language ?? Language.ZhCN);
            string errorMsg = "硅基人不存在";
            if (loc is DefaultLocalizationBase defaultLoc)
            {
                errorMsg = defaultLoc.PermissionBeingNotFound;
            }
            RenderHtml($"<html><body><h1>{errorMsg}</h1></body></html>");
            return;
        }

        // 加载当前权限回调代码
        var dataDirectory = Config.Instance?.Data?.DataDirectory?.FullName 
            ?? Path.Combine(Environment.CurrentDirectory, "data");
        string beingDirectory = Path.Combine(dataDirectory, "SiliconManager", beingId.ToString());
        
        var dynamicLoader = ServiceLocator.Instance.GetService<DynamicBeingLoader>();
        string callbackCode = dynamicLoader?.GetPermissionCallbackSourceCode(beingId, beingDirectory) ?? "";

        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.PermissionView();
        var vm = new Models.PermissionViewModel { Skin = skin, BeingId = beingId, CurrentCallbackCode = callbackCode };
        var html = view.Render(vm);
        RenderHtml(html);
    }



    private void GetList()
    {
        // 验证硅基人ID
        var beingIdStr = Request.QueryString?["beingId"];
        if (string.IsNullOrEmpty(beingIdStr) || !Guid.TryParse(beingIdStr, out Guid beingId))
        {
            RenderJson(new { error = "Missing or invalid beingId" });
            return;
        }

        // 获取该硅基人的PermissionManager
        var permManager = ServiceLocator.Instance.GetPermissionManager(beingId);
        if (permManager == null)
        {
            RenderJson(new { error = "Permission manager not found for this being" });
            return;
        }

        GlobalACL? acl = permManager.GlobalAcl;
        if (acl == null)
        {
            RenderJson(new List<object>());
            return;
        }

        List<AclRule> rules = acl.GetAllRules();
        var result = rules.Select(r => new
        {
            permissionType = r.PermissionType.ToString(),
            resourcePrefix = r.ResourcePrefix,
            result = r.Result.ToString(),
            description = r.Description ?? ""
        }).ToList();

        RenderJson(result);
    }

    private void Save()
    {
        try
        {
            // 验证硅基人ID
            var beingIdStr = Request.QueryString?["beingId"];
            if (string.IsNullOrEmpty(beingIdStr) || !Guid.TryParse(beingIdStr, out Guid beingId))
            {
                RenderJson(new { success = false, error = "Missing or invalid beingId" });
                return;
            }

            // 读取请求体中的代码
            using var reader = new StreamReader(Request.InputStream);
            string body = reader.ReadToEnd();
            var requestData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(body);
            
            if (requestData == null || !requestData.ContainsKey("code"))
            {
                RenderJson(new { success = false, error = "Missing code in request body" });
                return;
            }

            string sourceCode = requestData["code"];

            // 使用 DynamicBeingLoader 获取硅基人目录
            var dataDirectory = Config.Instance?.Data?.DataDirectory?.FullName 
                ?? Path.Combine(Environment.CurrentDirectory, "data");
            string beingDirectory = Path.Combine(dataDirectory, "SiliconManager", beingId.ToString());
            
            var dynamicLoader = ServiceLocator.Instance.GetService<DynamicBeingLoader>();
            if (dynamicLoader == null)
            {
                RenderJson(new { success = false, error = "DynamicBeingLoader not available" });
                return;
            }

            // 如果代码为空，删除自定义权限回调
            if (string.IsNullOrWhiteSpace(sourceCode))
            {
                DynamicBeingLoader.DeleteCustomPermissionCallback(beingDirectory);
                MainLoop.BeingManager?.ResetPermissionCallback(beingId);
                RenderJson(new { success = true, message = "Permission callback removed" });
                return;
            }

            // 保存并编译权限回调代码
            bool saved = dynamicLoader.SavePermissionCallback(beingId, beingDirectory, sourceCode);
            if (!saved)
            {
                RenderJson(new { success = false, error = "Failed to save permission callback (security scan failed)" });
                return;
            }

            // 加载并应用新的权限回调
            CompilationResult result = dynamicLoader.LoadPermissionCallback(beingId, beingDirectory);
            if (result.Success && result.CompiledType != null)
            {
                MainLoop.BeingManager?.ReplacePermissionCallback(beingId, result.CompiledType);
                RenderJson(new { success = true, message = "Permission callback saved and applied successfully" });
            }
            else
            {
                string errors = string.Join("\n", result.Errors ?? new List<string>());
                RenderJson(new { success = false, error = "Compilation failed", details = errors });
            }
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }
}
