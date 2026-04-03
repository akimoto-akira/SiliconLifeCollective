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

namespace SiliconLife.Default.Web;

[WebCode]
public class LogController : Controller
{
    public LogController()
    {
    }

    public override async Task HandleAsync()
    {
        var path = Request.Url?.AbsolutePath ?? "/logs";
        
        if (path == "/logs" || path == "/logs/index")
        {
            await Index();
        }
        else if (path == "/api/logs/list")
        {
            await GetList();
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
                .Title("日志查询 - Silicon Life Collective")
                .Style(GetStyles())
                .Script(GetScripts())
            .Body()
                .Div()
                    .Class("container")
                    .Div()
                        .Class("header")
                        .H1("日志查询")
                    .Div()
                        .Class("filters")
                        .Raw(@"
                            <select id=""log-level"">
                                <option value="""">全部级别</option>
                                <option value=""Info"">Info</option>
                                <option value=""Warning"">Warning</option>
                                <option value=""Error"">Error</option>
                            </select>
                            <input type=""date"" id=""log-date"" />
                            <button onclick=""loadLogs()"">查询</button>
                        ")
                    .Div()
                        .Class("logs-list")
                        .Id("logs-list")
                .Build();

        await RenderHtmlAsync(html);
    }

    private async Task GetList()
    {
        var logs = new List<object>();
        
        await RenderJsonAsync(logs);
    }

    private string GetStyles()
    {
        return @"
            * { box-sizing: border-box; margin: 0; padding: 0; }
            body { font-family: Arial, sans-serif; background: #f5f7fa; }
            .container { max-width: 1200px; margin: 0 auto; padding: 20px; }
            .header { margin-bottom: 20px; }
            .header h1 { font-size: 28px; color: #333; }
            .filters { margin-bottom: 20px; }
            .filters select, .filters input, .filters button { padding: 8px 12px; margin-right: 10px; border: 1px solid #ddd; border-radius: 6px; }
            .logs-list { background: white; padding: 20px; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); }
        ";
    }

    private string GetScripts()
    {
        return @"
            function loadLogs() {
                fetch('/api/logs/list')
                    .then(r => r.json())
                    .then(data => {
                        var list = document.getElementById('logs-list');
                        list.innerHTML = '<p>暂无日志</p>';
                    });
            }

            window.onload = function() {
                loadLogs();
            };
        ";
    }
}
