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
public class ExecutorController : Controller
{
    public ExecutorController()
    {
    }

    public override async Task HandleAsync()
    {
        var path = Request.Url?.AbsolutePath ?? "/executors";
        
        if (path == "/executors" || path == "/executors/index")
        {
            await Index();
        }
        else if (path == "/api/executors/status")
        {
            await GetStatus();
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
                .Title("执行器监控 - Silicon Life Collective")
                .Style(GetStyles())
                .Script(GetScripts())
            .Body()
                .Div()
                    .Class("container")
                    .Div()
                        .Class("header")
                        .H1("执行器监控")
                    .Div()
                        .Class("executors-grid")
                        .Id("executors-grid")
                .Build();

        await RenderHtmlAsync(html);
    }

    private async Task GetStatus()
    {
        var status = new[]
        {
            new { name = "DiskExecutor", status = "Idle", queueCount = 0 },
            new { name = "NetworkExecutor", status = "Idle", queueCount = 0 },
            new { name = "CommandLineExecutor", status = "Idle", queueCount = 0 }
        };
        
        await RenderJsonAsync(status);
    }

    private string GetStyles()
    {
        return @"
            * { box-sizing: border-box; margin: 0; padding: 0; }
            body { font-family: Arial, sans-serif; background: #f5f7fa; }
            .container { max-width: 1200px; margin: 0 auto; padding: 20px; }
            .header { margin-bottom: 30px; }
            .header h1 { font-size: 28px; color: #333; }
            .executors-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: 20px; }
            .executor-card { background: white; padding: 20px; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); }
            .executor-name { font-size: 18px; font-weight: bold; color: #333; margin-bottom: 10px; }
            .executor-status { display: inline-block; padding: 4px 12px; border-radius: 12px; font-size: 12px; background: #e8f5e9; color: #2e7d32; }
            .executor-queue { font-size: 14px; color: #666; margin-top: 8px; }
        ";
    }

    private string GetScripts()
    {
        return @"
            function loadExecutors() {
                fetch('/api/executors/status')
                    .then(r => r.json())
                    .then(data => {
                        var grid = document.getElementById('executors-grid');
                        grid.innerHTML = '';
                        data.forEach(e => {
                            var card = document.createElement('div');
                            card.className = 'executor-card';
                            card.innerHTML = '<div class=""executor-name"">' + e.name + '</div><span class=""executor-status"">' + e.status + '</span><div class=""executor-queue"">队列: ' + e.queueCount + '</div>';
                            grid.appendChild(card);
                        });
                    });
            }

            setInterval(loadExecutors, 5000);
            
            window.onload = function() {
                loadExecutors();
            };
        ";
    }
}
