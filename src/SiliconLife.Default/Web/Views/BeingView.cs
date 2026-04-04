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

using System.Text;
using SiliconLife.Default.Web.Models;

namespace SiliconLife.Default.Web.Views;

public class BeingView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as BeingViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, "硅基人管理 - Silicon Life Collective", "beings", body, GetScripts(), GetStyles());
    }

    private static string RenderBody(BeingViewModel vm)
    {
        return @"
<div class=""page-content"">
    <div class=""page-header""><h1>硅基人管理</h1></div>
    <div class=""beings-grid"" id=""beings-grid""></div>
    <div class=""detail-panel"" id=""detail-panel"">
        <div class=""detail-content"" id=""detail-content""><p>选择一个硅基人查看详情</p></div>
    </div>
</div>";
    }

    private static string GetStyles()
    {
        return @"
.beings-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: 20px; margin-bottom: 30px; }
.being-card { background: var(--bg-card); padding: 20px; border-radius: 12px; border: 1px solid var(--border); cursor: pointer; transition: transform 0.2s, box-shadow 0.2s; }
.being-card:hover { transform: translateY(-4px); box-shadow: 0 4px 16px rgba(0,0,0,0.12); }
.being-card.selected { border: 2px solid var(--accent-primary); }
.being-name { font-size: 18px; font-weight: bold; color: var(--text-primary); margin-bottom: 8px; }
.being-status { display: inline-block; padding: 4px 12px; border-radius: 12px; font-size: 12px; }
.being-status.idle { background: rgba(107,203,119,0.15); color: var(--accent-success); }
.being-status.running { background: rgba(77,150,255,0.15); color: var(--accent-primary); }
.being-info { font-size: 12px; color: var(--text-secondary); margin-top: 8px; }
.detail-panel { background: var(--bg-card); padding: 20px; border-radius: 12px; border: 1px solid var(--border); }
.detail-content h2 { font-size: 22px; color: var(--text-primary); margin-bottom: 15px; }
.detail-row { display: flex; margin-bottom: 10px; }
.detail-label { font-weight: bold; color: var(--text-secondary); width: 120px; }
.detail-value { color: var(--text-primary); }
.action-buttons { margin-top: 20px; display: flex; gap: 10px; }
.action-btn { padding: 10px 20px; border: none; border-radius: 6px; cursor: pointer; font-size: 14px; color: #fff; }
.action-btn.pause { background: var(--accent-warning); }
.action-btn.resume { background: var(--accent-success); }";
    }

    private static string GetScripts()
    {
        var sb = new StringBuilder();
        sb.AppendLine("var selectedBeingId = null;");
        sb.AppendLine("function loadBeings() {");
        sb.AppendLine("    fetch('/api/beings/list').then(function(r) { return r.json(); }).then(function(data) {");
        sb.AppendLine("        var grid = document.getElementById('beings-grid');");
        sb.AppendLine("        grid.innerHTML = '';");
        sb.AppendLine("        data.forEach(function(b) {");
        sb.AppendLine("            var card = document.createElement('div');");
        sb.AppendLine("            card.className = 'being-card' + (b.id === selectedBeingId ? ' selected' : '');");
        sb.AppendLine("            card.onclick = function() { selectBeing(b.id, b.name); };");
        sb.AppendLine(@"            card.innerHTML = '<div class=""being-name"">' + b.name + '</div>'");
        sb.AppendLine("                + '<span class=\"being-status ' + (b.isIdle ? 'idle' : 'running') + \"' + \">\" + (b.isIdle ? '空闲' : '运行中') + '</span>'");
        sb.AppendLine(@"            + '<div class=""being-info"">' + (b.customTypeName || '标准硅基人') + '</div>';");
        sb.AppendLine("            grid.appendChild(card);");
        sb.AppendLine("        });");
        sb.AppendLine("    });");
        sb.AppendLine("}");
        sb.AppendLine("function selectBeing(id, name) {");
        sb.AppendLine("    selectedBeingId = id;");
        sb.AppendLine("    loadBeings();");
        sb.AppendLine("    fetch('/api/beings/detail?id=' + id).then(function(r) { return r.json(); }).then(function(data) {");
        sb.AppendLine("        if (data.error) {");
        sb.AppendLine("            document.getElementById('detail-content').innerHTML = '<p>' + data.error + '</p>';");
        sb.AppendLine("            return;");
        sb.AppendLine("        }");
        sb.AppendLine("        var content = '<h2>' + data.name + '</h2>';");
        sb.AppendLine(@"        content += '<div class=""detail-row""><span class=""detail-label"">状态:</span><span class=""detail-value"">' + (data.isIdle ? '空闲' : '运行中') + '</span></div>';");
        sb.AppendLine(@"        content += '<div class=""detail-row""><span class=""detail-label"">用户ID:</span><span class=""detail-value"">' + data.userId + '</span></div>';");
        sb.AppendLine(@"        content += '<div class=""detail-row""><span class=""detail-label"">类型:</span><span class=""detail-value"">' + (data.customTypeName || '标准') + '</span></div>';");
        sb.AppendLine(@"        content += '<div class=""detail-row""><span class=""detail-label"">灵魂:</span><span class=""detail-value"">' + (data.soulContent || '无') + '</span></div>';");
        sb.AppendLine("        document.getElementById('detail-content').innerHTML = content;");
        sb.AppendLine("    });");
        sb.AppendLine("}");
        sb.AppendLine("window.onload = function() { loadBeings(); };");
        return sb.ToString();
    }
}
