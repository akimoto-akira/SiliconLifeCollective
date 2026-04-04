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

public class CodeBrowserView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as CodeBrowserViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, "代码浏览 - Silicon Life Collective", "beings", body, GetScripts(), GetStyles());
    }

    private static string RenderBody(CodeBrowserViewModel vm)
    {
        return @"
<div class=""page-content"">
    <div class=""page-header""><h1>代码浏览</h1></div>
    <div class=""code-browser"">
        <div class=""types-list"" id=""types-list""></div>
        <div class=""type-detail"" id=""type-detail""></div>
    </div>
</div>";
    }

    private static string GetStyles()
    {
        return @"
.code-browser { display: flex; gap: 20px; }
.types-list { width: 300px; background: var(--bg-card); padding: 20px; border-radius: 12px; border: 1px solid var(--border); overflow-y: auto; }
.type-item { padding: 10px; cursor: pointer; border-radius: 6px; color: var(--text-primary); }
.type-item:hover { background: var(--bg-secondary, rgba(255,255,255,0.05)); }
.type-detail { flex: 1; background: var(--bg-card); padding: 20px; border-radius: 12px; border: 1px solid var(--border); overflow-y: auto; }
.type-name { font-size: 20px; font-weight: bold; color: var(--text-primary); margin-bottom: 15px; }
.type-section { margin-bottom: 20px; }
.type-section h3 { font-size: 16px; color: var(--text-secondary); margin-bottom: 10px; }
.member-item { padding: 8px 0; border-bottom: 1px solid var(--border); color: var(--text-primary); }";
    }

    private static string GetScripts()
    {
        return @"
function loadTypes() {
    fetch('/api/code/types').then(function(r) { return r.json(); }).then(function(data) {
        var list = document.getElementById('types-list');
        list.innerHTML = '';
        data.forEach(function(t) {
            var item = document.createElement('div');
            item.className = 'type-item';
            item.textContent = t.name;
            item.onclick = function() { showType(t.fullName); };
            list.appendChild(item);
        });
    });
}
function showType(fullName) {
    fetch('/api/code/detail?name=' + encodeURIComponent(fullName)).then(function(r) { return r.json(); }).then(function(data) {
        if (data.error) {
            document.getElementById('type-detail').innerHTML = '<p>' + data.error + '</p>';
            return;
        }
        var html = '<div class=""type-name"">' + data.name + '</div><p>' + (data.description || '') + '</p>';
        if (data.properties && data.properties.length > 0) {
            html += '<div class=""type-section""><h3>属性</h3>';
            data.properties.forEach(function(p) { html += '<div class=""member-item"">' + p.type + ' ' + p.name + '</div>'; });
            html += '</div>';
        }
        if (data.methods && data.methods.length > 0) {
            html += '<div class=""type-section""><h3>方法</h3>';
            data.methods.forEach(function(m) { html += '<div class=""member-item"">' + m.returnType + ' ' + m.name + '(' + m.parameters.join(', ') + ')</div>'; });
            html += '</div>';
        }
        document.getElementById('type-detail').innerHTML = html;
    });
}
window.onload = function() { loadTypes(); };";
    }
}
