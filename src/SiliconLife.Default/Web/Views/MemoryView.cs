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

public class MemoryView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as MemoryViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, "记忆浏览 - Silicon Life Collective", "memory", body, GetScripts());
    }

    private static string RenderBody(MemoryViewModel vm)
    {
        return @"
<div class=""page-content"">
    <div class=""page-header""><h1>记忆浏览</h1></div>
    <div class=""card"">
        <div class=""memory-list"" id=""memory-list""></div>
    </div>
</div>";
    }

    private static string GetScripts()
    {
        return @"
function loadMemories() {
    fetch('/api/memory/list').then(function(r) { return r.json(); }).then(function(data) {
        var list = document.getElementById('memory-list');
        list.innerHTML = '<p>暂无记忆数据</p>';
    });
}
window.onload = function() { loadMemories(); };";
    }
}
