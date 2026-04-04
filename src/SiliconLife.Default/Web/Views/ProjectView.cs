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

public class ProjectView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as ProjectViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, "项目空间管理 - Silicon Life Collective", "projects", body, GetScripts());
    }

    private static string RenderBody(ProjectViewModel vm)
    {
        return @"
<div class=""page-content"">
    <div class=""page-header""><h1>项目空间管理</h1></div>
    <div class=""card"">
        <div class=""projects-list"" id=""projects-list""></div>
    </div>
</div>";
    }

    private static string GetScripts()
    {
        return @"
function loadProjects() {
    fetch('/api/projects/list').then(function(r) { return r.json(); }).then(function(data) {
        var list = document.getElementById('projects-list');
        list.innerHTML = '<p>暂无项目</p>';
    });
}
window.onload = function() { loadProjects(); };";
    }
}
