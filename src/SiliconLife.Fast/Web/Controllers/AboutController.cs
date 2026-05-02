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
using SiliconLife.Fast.Web.Models;

namespace SiliconLife.Fast.Web;

[WebCode]
public class AboutController : Controller
{
    private readonly SkinManager _skinManager;

    public AboutController()
    {
        _skinManager = ServiceLocator.Instance.GetService<SkinManager>()!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/about";

        if (path == "/about" || path == "/about/index")
            Index();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.AboutView();
        var vm = new Models.AboutViewModel 
        { 
            Skin = skin, 
            ActiveMenu = "about",
            PluginList = GetPluginList()
        };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    /// <summary>
    /// Gets the plugin list from PluginLoader
    /// </summary>
    private static Dictionary<string, AboutViewModel.PluginInfo> GetPluginList()
    {
        var result = new Dictionary<string, AboutViewModel.PluginInfo>();
        var language = Config.Instance?.Data?.Language ?? Language.ZhCN;
        
        // Get PluginLoader from ServiceLocator
        var pluginLoader = ServiceLocator.Instance.GetService<PluginLoader>();
        if (pluginLoader == null)
        {
            return result;
        }
        
        // Get all loaded plugins
        foreach (var plugin in pluginLoader.Plugins)
        {
            result[plugin.Id] = new AboutViewModel.PluginInfo
            {
                Name = plugin.GetName(language),
                Version = plugin.Version,
                Description = plugin.GetDescription(language),
                Author = plugin.GetAuthor(language)
            };
        }
        
        return result;
    }
}
