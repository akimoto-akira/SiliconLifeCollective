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
using SiliconLife.Collective;

namespace SiliconLife.Default.Web;

[WebCode]
public class ConfigController : Controller
{
    public ConfigController()
    {
    }

    public override async Task HandleAsync()
    {
        var path = Request.Url?.AbsolutePath ?? "/config";
        
        if (path == "/config" || path == "/config/index")
        {
            await Index();
        }
        else if (path == "/api/config/get")
        {
            await GetConfig();
        }
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private async Task Index()
    {
        var html = HtmlBuilder.Create()
            .DocType()
            .Html()
            .Head()
                .MetaCharset()
                .MetaViewport()
                .Title("系统配置 - Silicon Life Collective")
                .Style(GetStyles())
                .Script(GetScripts())
            .Body()
                .Div()
                    .Class("container")
                    .Div()
                        .Class("header")
                        .H1("系统配置")
                    .Div()
                        .Class("config-list")
                        .Id("config-list")
                .Build();

        await RenderHtmlAsync(html);
    }

    private async Task GetConfig()
    {
        var config = Config.Instance?.Data as DefaultConfigData;
        if (config == null)
        {
            await RenderJsonAsync(new { error = "Config not available" });
            return;
        }

        var result = new Dictionary<string, object?>
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
        };
        
        await RenderJsonAsync(result);
    }

    private string GetStyles()
    {
        return @"
            * { box-sizing: border-box; margin: 0; padding: 0; }
            body { font-family: Arial, sans-serif; background: #f5f7fa; }
            .container { max-width: 800px; margin: 0 auto; padding: 20px; }
            .header { margin-bottom: 30px; }
            .header h1 { font-size: 28px; color: #333; }
            .config-list { background: white; padding: 20px; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); }
            .config-item { display: flex; padding: 15px 0; border-bottom: 1px solid #eee; }
            .config-label { font-weight: bold; color: #666; width: 200px; }
            .config-value { color: #333; flex: 1; }
        ";
    }

    private string GetScripts()
    {
        return @"
            function loadConfig() {
                fetch('/api/config/get')
                    .then(r => r.json())
                    .then(data => {
                        var list = document.getElementById('config-list');
                        var html = '';
                        for (var key in data) {
                            html += '<div class=""config-item""><span class=""config-label"">' + key + '</span><span class=""config-value"">' + data[key] + '</span></div>';
                        }
                        list.innerHTML = html || '<p>暂无配置</p>';
                    });
            }

            window.onload = function() {
                loadConfig();
            };
        ";
    }
}
