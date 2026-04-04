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

public class KnowledgeView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as KnowledgeViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, "知识图谱 - Silicon Life Collective", "knowledge", body, GetScripts(), GetStyles());
    }

    private static string RenderBody(KnowledgeViewModel vm)
    {
        return @"
<div class=""page-content"">
    <div class=""page-header""><h1>知识图谱可视化</h1></div>
    <div class=""graph-container"" id=""graph-container""></div>
</div>";
    }

    private static string GetStyles()
    {
        return ".graph-container { background: var(--bg-card); padding: 20px; border-radius: 12px; border: 1px solid var(--border); min-height: 500px; }";
    }

    private static string GetScripts()
    {
        return @"
function loadGraph() {
    fetch('/api/knowledge/graph').then(function(r) { return r.json(); }).then(function(data) {
        var container = document.getElementById('graph-container');
        container.innerHTML = '<p>知识图谱数据加载中...</p>';
    });
}
window.onload = function() { loadGraph(); };";
    }
}
