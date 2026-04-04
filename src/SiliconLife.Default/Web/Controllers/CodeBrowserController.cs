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
public class CodeBrowserController : Controller
{
    private readonly WebCodeBrowser _codeBrowser;

    public CodeBrowserController(WebCodeBrowser codeBrowser)
    {
        _codeBrowser = codeBrowser;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/code";
        
        if (path == "/code" || path == "/code/index")
        {
            Index();
        }
        else if (path == "/api/code/types")
        {
            GetTypes();
        }
        else if (path == "/api/code/detail")
        {
            GetDetail();
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
                .Title("代码浏览 - Silicon Life Collective")
                .Style(GetStyles())
                .Script(GetScripts())
            .EndBlock()
            .Body()
                .Div()
                    .Class("container")
                    .Div()
                        .Class("header")
                        .H1("代码浏览")
                    .EndBlock()
                    .Div()
                        .Class("code-browser")
                        .Div()
                            .Class("types-list")
                            .Id("types-list")
                        .EndBlock()
                        .Div()
                            .Class("type-detail")
                            .Id("type-detail")
                        .EndBlock()
                    .EndBlock()
                .EndBlock()
            .EndBlock()
            .Build();

        RenderHtml(html);
    }

    private void GetTypes()
    {
        var types = _codeBrowser.GetAllTypes();
        RenderJson(types);
    }

    private void GetDetail()
    {
        var fullName = Request.QueryString["name"];
        if (string.IsNullOrEmpty(fullName))
        {
            RenderJson(new { error = "Name required" });
            return;
        }

        var detail = _codeBrowser.GetType(fullName);
        if (detail == null)
        {
            RenderJson(new { error = "Type not found" });
            return;
        }

        RenderJson(detail);
    }

    private string GetStyles()
    {
        return @"
            * { box-sizing: border-box; margin: 0; padding: 0; }
            body { font-family: Arial, sans-serif; background: #f5f7fa; }
            .container { max-width: 1200px; margin: 0 auto; padding: 20px; }
            .header { margin-bottom: 30px; }
            .header h1 { font-size: 28px; color: #333; }
            .code-browser { display: flex; gap: 20px; }
            .types-list { width: 300px; background: white; padding: 20px; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); }
            .type-item { padding: 10px; cursor: pointer; border-radius: 6px; }
            .type-item:hover { background: #f0f4f8; }
            .type-detail { flex: 1; background: white; padding: 20px; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); }
            .type-name { font-size: 20px; font-weight: bold; color: #333; margin-bottom: 15px; }
            .type-section { margin-bottom: 20px; }
            .type-section h3 { font-size: 16px; color: #666; margin-bottom: 10px; }
            .member-item { padding: 8px 0; border-bottom: 1px solid #eee; }
        ";
    }

    private string GetScripts()
    {
        return @"
            function loadTypes() {
                fetch('/api/code/types')
                    .then(r => r.json())
                    .then(data => {
                        var list = document.getElementById('types-list');
                        list.innerHTML = '';
                        data.forEach(t => {
                            var item = document.createElement('div');
                            item.className = 'type-item';
                            item.textContent = t.name;
                            item.onclick = () => showType(t.fullName);
                            list.appendChild(item);
                        });
                    });
            }

            function showType(fullName) {
                fetch('/api/code/detail?name=' + encodeURIComponent(fullName))
                    .then(r => r.json())
                    .then(data => {
                        if (data.error) {
                            document.getElementById('type-detail').innerHTML = '<p>' + data.error + '</p>';
                            return;
                        }
                        
                        var html = '<div class=""type-name"">' + data.name + '</div>';
                        html += '<p>' + (data.description || '') + '</p>';
                        
                        if (data.properties && data.properties.length > 0) {
                            html += '<div class=""type-section""><h3>属性</h3>';
                            data.properties.forEach(p => {
                                html += '<div class=""member-item"">' + p.type + ' ' + p.name + '</div>';
                            });
                            html += '</div>';
                        }
                        
                        if (data.methods && data.methods.length > 0) {
                            html += '<div class=""type-section""><h3>方法</h3>';
                            data.methods.forEach(m => {
                                html += '<div class=""member-item"">' + m.returnType + ' ' + m.name + '(' + m.parameters.join(', ') + ')</div>';
                            });
                            html += '</div>';
                        }
                        
                        document.getElementById('type-detail').innerHTML = html;
                    });
            }

            window.onload = function() {
                loadTypes();
            };
        ";
    }
}
