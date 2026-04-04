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
public class KnowledgeController : Controller
{
    public KnowledgeController()
    {
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/knowledge";
        
        if (path == "/knowledge" || path == "/knowledge/index")
        {
            Index();
        }
        else if (path == "/api/knowledge/graph")
        {
            GetGraph();
        }
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var html = HtmlBuilder.Create()
            .DocType()
            .Html()
            .Head()
                .MetaCharset()
                .MetaViewport()
                .Title("知识图谱 - Silicon Life Collective")
                .Style(GetStyles())
                .Script(GetScripts())
            .EndBlock()
            .Body()
                .Div()
                    .Class("container")
                    .Div()
                        .Class("header")
                        .H1("知识图谱可视化")
                    .EndBlock()
                    .Div()
                        .Class("graph-container")
                        .Id("graph-container")
                    .EndBlock()
                .EndBlock()
            .EndBlock()
            .Build();

        RenderHtml(html);
    }

    private void GetGraph()
    {
        var graph = new
        {
            nodes = new[] { new { id = "1", label = "中心" } },
            edges = new[] { new { from = "1", to = "1" } }
        };
        
        RenderJson(graph);
    }

    private string GetStyles()
    {
        return @"
            * { box-sizing: border-box; margin: 0; padding: 0; }
            body { font-family: Arial, sans-serif; background: #f5f7fa; }
            .container { max-width: 1200px; margin: 0 auto; padding: 20px; }
            .header { margin-bottom: 30px; }
            .header h1 { font-size: 28px; color: #333; }
            .graph-container { background: white; padding: 20px; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); min-height: 500px; }
        ";
    }

    private string GetScripts()
    {
        return @"
            function loadGraph() {
                fetch('/api/knowledge/graph')
                    .then(r => r.json())
                    .then(data => {
                        var container = document.getElementById('graph-container');
                        container.innerHTML = '<p>知识图谱数据加载中...</p>';
                    });
            }

            window.onload = function() {
                loadGraph();
            };
        ";
    }
}
