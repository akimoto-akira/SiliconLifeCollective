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

public class ConfigView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as ConfigViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, "系统配置 - Silicon Life Collective", "config", body, GetScripts());
    }

    private static string RenderBody(ConfigViewModel vm)
    {
        return @"
<div class=""page-content"">
    <div class=""page-header""><h1>系统配置</h1></div>
    <div class=""config-list"" id=""config-list""></div>
</div>";
    }

    private static string GetScripts()
    {
        return @"
function loadConfig() {
    fetch('/api/config/get').then(function(r) { return r.json(); }).then(function(data) {
        var list = document.getElementById('config-list');
        var html = '';
        for (var key in data) {
            html += '<div class=""config-item""><span class=""config-label"">' + key + '</span><span class=""config-value"">' + data[key] + '</span></div>';
        }
        list.innerHTML = html || '<p>暂无配置</p>';
    });
}
window.onload = function() { loadConfig(); };";
    }
}
