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

public class PermissionView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as PermissionViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, "权限管理 - Silicon Life Collective", "beings", body, GetScripts());
    }

    private static string RenderBody(PermissionViewModel vm)
    {
        return @"
<div class=""page-content"">
    <div class=""page-header""><h1>权限管理</h1></div>
    <div class=""card"">
        <div class=""permissions-list"" id=""permissions-list""></div>
    </div>
</div>";
    }

    private static string GetScripts()
    {
        return @"
function loadPermissions() {
    fetch('/api/permissions/list').then(function(r) { return r.json(); }).then(function(data) {
        var list = document.getElementById('permissions-list');
        list.innerHTML = '<p>暂无权限记录</p>';
    });
}
window.onload = function() { loadPermissions(); };";
    }
}
