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
public class ConfigController : Controller
{
    private readonly SkinManager _skinManager;

    public ConfigController(SkinManager skinManager)
    {
        _skinManager = skinManager;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/config";

        if (path == "/config" || path == "/config/index")
            Index();
        else if (path == "/api/config/get")
            GetConfig();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.ConfigView();
        var vm = new Models.ConfigViewModel { Skin = skin, ActiveMenu = "config" };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetConfig()
    {
        var config = Config.Instance?.Data as DefaultConfigData;
        if (config == null)
        {
            RenderJson(new { error = "Config not available" });
            return;
        }

        RenderJson(new Dictionary<string, object?>
        {
            { "ConfigType", config.ConfigType },
            { "DataDirectory", config.DataDirectory },
            { "CuratorGuid", config.CuratorGuid.ToString() },
            { "Language", config.Language.ToString() },
            { "TickTimeout", config.TickTimeout.ToString() },
            { "MaxTimeoutCount", config.MaxTimeoutCount },
            { "WatchdogTimeout", config.WatchdogTimeout.ToString() },
            { "OllamaEndpoint", config.OllamaEndpoint },
            { "DefaultModel", config.DefaultModel },
            { "WebPort", config.WebPort }
        });
    }
}
