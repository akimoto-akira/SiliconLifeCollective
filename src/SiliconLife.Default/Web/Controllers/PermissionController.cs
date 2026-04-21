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
        // Force validate silicon being ID
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

        // Validate silicon being exists
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

        // Load current permission callback code
        var dataDirectory = Config.Instance?.Data?.DataDirectory?.FullName 
            ?? Path.Combine(Environment.CurrentDirectory, "data");
        string beingDirectory = Path.Combine(dataDirectory, "SiliconManager", beingId.ToString());
                
        var dynamicLoader = ServiceLocator.Instance.GetService<DynamicBeingLoader>();
        string callbackCode = dynamicLoader?.GetPermissionCallbackSourceCode(beingId, beingDirectory) ?? "";
        
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
                
        // If no custom code, use default template
        var vm = new Models.PermissionViewModel { Skin = skin, BeingId = beingId };
        if (string.IsNullOrWhiteSpace(callbackCode))
        {
            callbackCode = vm.GenerateDefaultTemplateCode();
        }
                
        vm.CurrentCallbackCode = callbackCode;
        var view = new Views.PermissionView();
        var html = view.Render(vm);
        RenderHtml(html);
    }



    private void GetList()
    {
        // Validate silicon being ID
        var beingIdStr = Request.QueryString?["beingId"];
        if (string.IsNullOrEmpty(beingIdStr) || !Guid.TryParse(beingIdStr, out Guid beingId))
        {
            RenderJson(new { error = "Missing or invalid beingId" });
            return;
        }

        // Get PermissionManager for this silicon being
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
            // Get localization
            var loc = LocalizationManager.Instance.GetLocalization(Config.Instance?.Data?.Language ?? Language.ZhCN);
            string errorMessage = "Save failed";
            if (loc is DefaultLocalizationBase defaultLoc)
            {
                errorMessage = defaultLoc.PermissionSaveError;
            }

            // Validate silicon being ID
            var beingIdStr = Request.QueryString?["beingId"];
            if (string.IsNullOrEmpty(beingIdStr) || !Guid.TryParse(beingIdStr, out Guid beingId))
            {
                string missingBeingId = "Missing or invalid beingId";
                if (loc is DefaultLocalizationBase locBase)
                {
                    missingBeingId = locBase.PermissionSaveMissingBeingId;
                }
                RenderJson(new { success = false, error = missingBeingId });
                return;
            }

            // Read code from request body
            using var reader = new StreamReader(Request.InputStream);
            string body = reader.ReadToEnd();
            var requestData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(body);
            
            if (requestData == null || !requestData.ContainsKey("code"))
            {
                string missingCode = "Missing code in request body";
                if (loc is DefaultLocalizationBase locBase2)
                {
                    missingCode = locBase2.PermissionSaveMissingCode;
                }
                RenderJson(new { success = false, error = missingCode });
                return;
            }

            string sourceCode = requestData["code"];

            // Use DynamicBeingLoader to get silicon being directory
            var dataDirectory = Config.Instance?.Data?.DataDirectory?.FullName 
                ?? Path.Combine(Environment.CurrentDirectory, "data");
            string beingDirectory = Path.Combine(dataDirectory, "SiliconManager", beingId.ToString());
            
            var dynamicLoader = ServiceLocator.Instance.GetService<DynamicBeingLoader>();
            if (dynamicLoader == null)
            {
                string loaderNotAvailable = "DynamicBeingLoader not available";
                if (loc is DefaultLocalizationBase locBase3)
                {
                    loaderNotAvailable = locBase3.PermissionSaveLoaderNotAvailable;
                }
                RenderJson(new { success = false, error = loaderNotAvailable });
                return;
            }

            // If code is empty, delete custom permission callback
            if (string.IsNullOrWhiteSpace(sourceCode))
            {
                try
                {
                    DynamicBeingLoader.DeleteCustomPermissionCallback(beingDirectory);
                    MainLoop.BeingManager?.ResetPermissionCallback(beingId);
                    
                    string removeSuccess = "Permission callback removed";
                    if (loc is DefaultLocalizationBase locBase4)
                    {
                        removeSuccess = locBase4.PermissionSaveRemoveSuccess;
                    }
                    RenderJson(new { success = true, message = removeSuccess });
                }
                catch
                {
                    string removeFailed = "Failed to remove permission callback";
                    if (loc is DefaultLocalizationBase locBase5)
                    {
                        removeFailed = locBase5.PermissionSaveRemoveFailed;
                    }
                    RenderJson(new { success = false, error = removeFailed });
                }
                return;
            }

            // Step 1: Compile first to validate code (without saving)
            CompilationResult compileResult = dynamicLoader.CompilePermissionCallback(sourceCode);
            if (!compileResult.Success || compileResult.CompiledType == null)
            {
                string compilationFailed = "Compilation failed";
                if (loc is DefaultLocalizationBase locBase6)
                {
                    compilationFailed = locBase6.PermissionSaveCompilationFailed;
                }
                string errors = string.Join("\n", compileResult.Errors ?? new List<string>());
                RenderJson(new { success = false, error = compilationFailed, details = errors });
                return;
            }

            // Step 2: Compilation succeeded, now save to disk (includes security scan)
            bool saved = dynamicLoader.SavePermissionCallback(beingId, beingDirectory, sourceCode);
            if (!saved)
            {
                string securityFailed = "Failed to save permission callback (security scan failed)";
                if (loc is DefaultLocalizationBase locBase7)
                {
                    securityFailed = locBase7.PermissionSaveSecurityScanFailed;
                }
                RenderJson(new { success = false, error = securityFailed });
                return;
            }

            // Step 3: Apply the compiled type to the running being
            MainLoop.BeingManager?.ReplacePermissionCallback(beingId, compileResult.CompiledType);
            
            string saveSuccess = "Permission callback saved and applied successfully";
            if (loc is DefaultLocalizationBase locBase8)
            {
                saveSuccess = locBase8.PermissionSaveSuccess;
            }
            RenderJson(new { success = true, message = saveSuccess });
        }
        catch (Exception ex)
        {
            var loc = LocalizationManager.Instance.GetLocalization(Config.Instance?.Data?.Language ?? Language.ZhCN);
            string genericError = "An error occurred while saving permission callback";
            if (loc is DefaultLocalizationBase locBase)
            {
                genericError = locBase.PermissionSaveError;
            }
            RenderJson(new { success = false, error = genericError, details = ex.Message });
        }
    }
}
