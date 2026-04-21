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
using SiliconLife.Default.Web.Models;
using SiliconLife.Default.Web.Views;

namespace SiliconLife.Default.Web;

[WebCode]
public class BeingController : Controller
{
    private readonly SiliconBeingManager _beingManager;
    private readonly SkinManager _skinManager;

    public BeingController()
    {
        var locator = ServiceLocator.Instance;
        _beingManager = locator.BeingManager!;
        _skinManager = locator.GetService<SkinManager>()!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/beings";

        if (path == "/beings" || path == "/beings/index")
            Index();
        else if (path == "/api/beings/list")
            GetList();
        else if (path == "/api/beings/detail")
            GetDetail();
        else if (path.StartsWith("/beings/soul"))
            SoulEditor();
        else if (path == "/beings/ai-config")
            AIConfigEditor();
        else if (path == "/api/beings/ai-config/save")
            SaveAIConfig();
        else if (path == "/api/beings/ai-config/models")
            GetAIModels();
        else if (path == "/api/beings/soul/save")
            SaveSoul();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.BeingView();
        var vm = new Models.BeingViewModel { Skin = skin, ActiveMenu = "beings" };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetList()
    {
        var beings = _beingManager.GetAllBeings();
        var list = beings.Select(b => new
        {
            id = b.Id.ToString(),
            name = b.Name,
            isIdle = b.IsIdle,
            isCustomCompiled = b.IsCustomCompiled,
            customTypeName = b.CustomTypeName ?? ""
        }).ToList();

        RenderJson(list);
    }

    private void GetDetail()
    {
        var idStr = Request.QueryString["id"];
        if (string.IsNullOrEmpty(idStr) || !Guid.TryParse(idStr, out var id))
        {
            RenderJson(new { error = "Invalid ID" });
            return;
        }

        var being = _beingManager.GetBeing(id);
        if (being == null)
        {
            RenderJson(new { error = "Not found" });
            return;
        }

        RenderJson(new
        {
            id = being.Id.ToString(),
            name = being.Name,
            isIdle = being.IsIdle,
            isCustomCompiled = being.IsCustomCompiled,
            customTypeName = being.CustomTypeName ?? "",
            soulContent = being.SoulContent ?? "",
            timerCount = being.TimerSystem?.Count ?? 0,
            taskCount = being.TaskSystem?.Count ?? 0,
            aiClientType = being.AIClientType ?? "",
            aiClientConfig = (being.AIClientConfig != null && being.AIClientConfig.Count > 0) 
                ? System.Text.Json.JsonSerializer.Serialize(being.AIClientConfig) 
                : null
        });
    }

    private void SoulEditor()
    {
        var idStr = Request.QueryString["beingId"];
        if (string.IsNullOrEmpty(idStr) || !Guid.TryParse(idStr, out var id))
        {
            Response.StatusCode = 400;
            RenderHtml("<h1>Invalid Being ID</h1>");
            return;
        }

        var being = _beingManager.GetBeing(id);
        if (being == null)
        {
            Response.StatusCode = 404;
            RenderHtml("<h1>Being Not Found</h1>");
            return;
        }

        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.SoulEditorView();
        var vm = new Models.SoulEditorViewModel
        {
            Skin = skin,
            ActiveMenu = "beings",
            BeingId = being.Id,
            BeingName = being.Name,
            SoulContent = being.SoulContent ?? ""
        };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void SaveSoul()
    {
        var idStr = Request.QueryString["beingId"];
        if (string.IsNullOrEmpty(idStr) || !Guid.TryParse(idStr, out var id))
        {
            RenderJson(new { success = false, error = "Invalid Being ID" });
            return;
        }

        var being = _beingManager.GetBeing(id);
        if (being == null)
        {
            RenderJson(new { success = false, error = "Being Not Found" });
            return;
        }

        try
        {
            var body = new System.IO.StreamReader(Request.InputStream).ReadToEnd();
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true  // 忽略属性名大小写
            };
            var requestData = System.Text.Json.JsonSerializer.Deserialize<SaveSoulRequest>(body, options);
            
            if (requestData == null)
            {
                RenderJson(new { success = false, error = "Invalid request data" });
                return;
            }

            being.SoulContent = requestData.Markdown ?? "";
            
            RenderJson(new { success = true });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void AIConfigEditor()
    {
        var idStr = Request.QueryString["beingId"];
        if (string.IsNullOrEmpty(idStr) || !Guid.TryParse(idStr, out var id))
        {
            Response.StatusCode = 400;
            RenderHtml("<h1>Invalid Being ID</h1>");
            return;
        }

        var being = _beingManager.GetBeing(id);
        if (being == null)
        {
            Response.StatusCode = 404;
            RenderHtml("<h1>Being Not Found</h1>");
            return;
        }

        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new BeingAIConfigView();
        var vm = BuildAIConfigViewModel(being, skin);
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private BeingAIConfigViewModel BuildAIConfigViewModel(SiliconBeingBase being, ISkin skin)
    {
        var configData = (DefaultConfigData)Config.Instance.Data;
        var localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(configData.Language);

        // Auto-discover all AI client factory types
        var factoryTypes = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.GetName().Name?.StartsWith("SiliconLife") == true)
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IAIClientFactory).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToList();

        var options = new List<AIClientTypeOption>();
        foreach (var type in factoryTypes)
        {
            try
            {
                var factory = (IAIClientFactory)Activator.CreateInstance(type)!;
                var typeName = type.Name.Replace("Factory", "");
                var displayName = localization.GetConfigDisplayName(typeName, out _) ?? typeName;
                options.Add(new AIClientTypeOption
                {
                    TypeName = type.Name,
                    DisplayName = displayName
                });
            }
            catch
            {
                // Skip types that cannot be instantiated
            }
        }

        var currentType = being.AIClientType ?? configData.AIClientType ?? "OllamaClientFactory";
        if (string.IsNullOrEmpty(currentType))
            currentType = "OllamaClientFactory";

        // Normalize: ensure currentType has Factory suffix to match discovered type names
        if (!currentType.EndsWith("Factory"))
            currentType += "Factory";

        // Ensure the current type is in options
        if (!options.Any(o => o.TypeName == currentType))
        {
            options.Add(new AIClientTypeOption
            {
                TypeName = currentType,
                DisplayName = currentType.Replace("Factory", "")
            });
        }

        var hasIndependentConfig = being.AIClientConfig != null && being.AIClientConfig.Count > 0;

        return new BeingAIConfigViewModel
        {
            Skin = skin,
            ActiveMenu = "beings",
            BeingId = being.Id,
            BeingName = being.Name,
            CurrentAIClientType = currentType,
            CurrentAIConfigJson = hasIndependentConfig
                ? JsonSerializer.Serialize(being.AIClientConfig)
                : "{}",
            HasIndependentConfig = hasIndependentConfig,
            GlobalAIClientType = configData.AIClientType ?? "OllamaClient",
            GlobalAIConfigJson = configData.AIConfig != null
                ? JsonSerializer.Serialize(configData.AIConfig)
                : "{}",
            AIClientTypeOptions = options
        };
    }

    private void SaveAIConfig()
    {
        try
        {
            var body = new System.IO.StreamReader(Request.InputStream).ReadToEnd();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var requestData = JsonSerializer.Deserialize<SaveAIConfigRequest>(body, options);

            if (requestData == null || !Guid.TryParse(requestData.BeingId, out var id))
            {
                RenderJson(new { success = false, error = "Invalid request data" });
                return;
            }

            var being = _beingManager.GetBeing(id);
            if (being == null)
            {
                RenderJson(new { success = false, error = "Being Not Found" });
                return;
            }

            if (requestData.UseIndependent)
            {
                being.AIClientType = requestData.AIClientType;
                being.AIClientConfig = requestData.AIConfig ?? new Dictionary<string, object>();
            }
            else
            {
                being.AIClientType = null;
                being.AIClientConfig = null;
            }

            // Save state if it's a DefaultSiliconBeing
            if (being is DefaultSiliconBeing defaultBeing)
            {
                defaultBeing.SaveState();
            }

            RenderJson(new { success = true });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void GetAIModels()
    {
        var clientType = Request.QueryString["clientType"];
        var apiKey = Request.QueryString["apiKey"];
        var region = Request.QueryString["region"] ?? "";

        if (string.IsNullOrEmpty(clientType) || string.IsNullOrEmpty(apiKey))
        {
            RenderJson(new { models = new List<object>() });
            return;
        }

        var factoryType = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.GetName().Name?.StartsWith("SiliconLife") == true)
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name == clientType && typeof(IAIClientFactory).IsAssignableFrom(t));

        if (factoryType == null)
        {
            RenderJson(new { models = new List<object>() });
            return;
        }

        try
        {
            var factory = (IAIClientFactory)Activator.CreateInstance(factoryType)!;
            var configData = (DefaultConfigData)Config.Instance.Data;
            var currentLanguage = configData.Language;

            var currentConfig = new Dictionary<string, object>
            {
                ["apiKey"] = apiKey,
                ["region"] = region
            };

            var options = factory.GetConfigKeyOptions("model", currentConfig, currentLanguage);
            var models = new List<object>();
            if (options != null)
            {
                foreach (var o in options)
                    models.Add(new { key = o.Key, value = o.Value });
            }

            RenderJson(new { models });
        }
        catch
        {
            RenderJson(new { models = new List<object>() });
        }
    }

    private class SaveSoulRequest
    {
        public string Markdown { get; set; } = string.Empty;
    }

    private class SaveAIConfigRequest
    {
        public string BeingId { get; set; } = string.Empty;
        public bool UseIndependent { get; set; }
        public string AIClientType { get; set; } = string.Empty;
        public Dictionary<string, object> AIConfig { get; set; } = new();
    }
}
