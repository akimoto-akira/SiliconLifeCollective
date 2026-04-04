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
public class BeingController : Controller
{
    private readonly SiliconBeingManager _beingManager;

    public BeingController(SiliconBeingManager beingManager)
    {
        _beingManager = beingManager;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/beings";
        
        if (path == "/beings" || path == "/beings/index")
        {
            Index();
        }
        else if (path == "/api/beings/list")
        {
            GetList();
        }
        else if (path == "/api/beings/detail")
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
                .Title("硅基人管理 - Silicon Life Collective")
                .Style(GetBeingStyles())
                .Script(GetBeingScripts())
            .EndBlock()
            .Body()
                .Div()
                    .Class("container")
                    .Div()
                        .Class("header")
                        .H1("硅基人管理")
                    .EndBlock()
                    .Div()
                        .Class("beings-grid")
                        .Id("beings-grid")
                    .EndBlock()
                    .Div()
                        .Class("detail-panel")
                        .Id("detail-panel")
                        .Raw(@"
                            <div class=""detail-content"" id=""detail-content"">
                                <p>选择一个硅基人查看详情</p>
                            </div>
                        ")
                    .EndBlock()
                .EndBlock()
            .EndBlock()
            .Build();

        RenderHtml(html);
    }

    private void GetList()
    {
        var beings = _beingManager.GetAllBeings();
        var list = beings.Select(b => new
        {
            id = b.Id.ToString(),
            name = b.Name,
            userId = b.UserId.ToString(),
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

        var detail = new
        {
            id = being.Id.ToString(),
            name = being.Name,
            userId = being.UserId.ToString(),
            isIdle = being.IsIdle,
            isCustomCompiled = being.IsCustomCompiled,
            customTypeName = being.CustomTypeName ?? "",
            soulContent = being.SoulContent ?? ""
        };
        
        RenderJson(detail);
    }

    private string GetBeingStyles()
    {
        return @"
            * { box-sizing: border-box; margin: 0; padding: 0; }
            body { font-family: Arial, sans-serif; background: #f5f7fa; }
            .container { max-width: 1200px; margin: 0 auto; padding: 20px; }
            .header { margin-bottom: 30px; }
            .header h1 { font-size: 28px; color: #333; }
            .beings-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: 20px; margin-bottom: 30px; }
            .being-card { background: white; padding: 20px; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); cursor: pointer; transition: transform 0.2s, box-shadow 0.2s; }
            .being-card:hover { transform: translateY(-4px); box-shadow: 0 4px 16px rgba(0,0,0,0.12); }
            .being-card.selected { border: 2px solid #2196F3; }
            .being-name { font-size: 18px; font-weight: bold; color: #333; margin-bottom: 8px; }
            .being-status { display: inline-block; padding: 4px 12px; border-radius: 12px; font-size: 12px; }
            .being-status.idle { background: #e8f5e9; color: #2e7d32; }
            .being-status.running { background: #e3f2fd; color: #1565c0; }
            .being-info { font-size: 12px; color: #666; margin-top: 8px; }
            .detail-panel { background: white; padding: 20px; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); }
            .detail-content h2 { font-size: 22px; color: #333; margin-bottom: 15px; }
            .detail-row { display: flex; margin-bottom: 10px; }
            .detail-label { font-weight: bold; color: #666; width: 120px; }
            .detail-value { color: #333; }
            .action-buttons { margin-top: 20px; }
            .action-btn { padding: 10px 20px; margin-right: 10px; border: none; border-radius: 6px; cursor: pointer; font-size: 14px; }
            .action-btn.pause { background: #ff9800; color: white; }
            .action-btn.resume { background: #4caf50; color: white; }
            .action-btn:hover { opacity: 0.9; }
        ";
    }

    private string GetBeingScripts()
    {
        return @"
            let selectedBeingId = null;

            function loadBeings() {
                fetch('/api/beings/list')
                    .then(r => r.json())
                    .then(data => {
                        var grid = document.getElementById('beings-grid');
                        grid.innerHTML = '';
                        data.forEach(b => {
                            var card = document.createElement('div');
                            card.className = 'being-card' + (b.id === selectedBeingId ? ' selected' : '');
                            card.onclick = () => selectBeing(b.id, b.name);
                            card.innerHTML = '
                                <div class=""being-name"">' + b.name + '</div>
                                <span class=""being-status ' + (b.isIdle ? 'idle' : 'running') + '"">' + (b.isIdle ? '空闲' : '运行中') + '</span>
                                <div class=""being-info"">' + (b.customTypeName || '标准硅基人') + '</div>
                            ';
                            grid.appendChild(card);
                        });
                    });
            }

            function selectBeing(id, name) {
                selectedBeingId = id;
                loadBeings();
                
                fetch('/api/beings/detail?id=' + id)
                    .then(r => r.json())
                    .then(data => {
                        if (data.error) {
                            document.getElementById('detail-content').innerHTML = '<p>' + data.error + '</p>';
                            return;
                        }
                        
                        var content = '<h2>' + data.name + '</h2>';
                        content += '<div class=""detail-row""><span class=""detail-label"">状态:</span><span class=""detail-value"">' + (data.isIdle ? '空闲' : '运行中') + '</span></div>';
                        content += '<div class=""detail-row""><span class=""detail-label"">用户ID:</span><span class=""detail-value"">' + data.userId + '</span></div>';
                        content += '<div class=""detail-row""><span class=""detail-label"">类型:</span><span class=""detail-value"">' + (data.customTypeName || '标准') + '</span></div>';
                        content += '<div class=""detail-row""><span class=""detail-label"">灵魂:</span><span class=""detail-value"">' + (data.soulContent || '无') + '</span></div>';
                        
                        document.getElementById('detail-content').innerHTML = content;
                    });
            }

            window.onload = function() {
                loadBeings();
            };
        ";
    }
}
