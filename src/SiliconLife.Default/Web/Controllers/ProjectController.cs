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
public class ProjectController : Controller
{
    public ProjectController()
    {
    }

    public override async Task HandleAsync()
    {
        var path = Request.Url?.AbsolutePath ?? "/projects";
        
        if (path == "/projects" || path == "/projects/index")
        {
            await Index();
        }
        else if (path == "/api/projects/list")
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
                .Title("项目空间管理 - Silicon Life Collective")
                .Style(GetStyles())
                .Script(GetScripts())
            .Body()
                .Div()
                    .Class("container")
                    .Div()
                        .Class("header")
                        .H1("项目空间管理")
                    .Div()
                        .Class("projects-list")
                        .Id("projects-list")
                .Build();

        await RenderHtmlAsync(html);
    }

    private async Task GetList()
    {
        var projects = new List<object>();
        
        await RenderJsonAsync(projects);
    }

    private string GetStyles()
    {
        return @"
            * { box-sizing: border-box; margin: 0; padding: 0; }
            body { font-family: Arial, sans-serif; background: #f5f7fa; }
            .container { max-width: 1200px; margin: 0 auto; padding: 20px; }
            .header { margin-bottom: 30px; }
            .header h1 { font-size: 28px; color: #333; }
            .projects-list { background: white; padding: 20px; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); }
        ";
    }

    private string GetScripts()
    {
        return @"
            function loadProjects() {
                fetch('/api/projects/list')
                    .then(r => r.json())
                    .then(data => {
                        var list = document.getElementById('projects-list');
                        list.innerHTML = '<p>暂无项目</p>';
                    });
            }

            window.onload = function() {
                loadProjects();
            };
        ";
    }
}
