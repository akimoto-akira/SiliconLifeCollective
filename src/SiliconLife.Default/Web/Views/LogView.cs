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

public class LogView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as LogViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, "日志查询 - Silicon Life Collective", "logs", body, GetScripts());
    }

    private static string RenderBody(LogViewModel vm)
    {
        return @"
<div class=""page-content"">
    <div class=""page-header""><h1>日志查询</h1></div>
    <div class=""filter-bar"">
        <select id=""log-level"">
            <option value="""">全部级别</option>
            <option value=""Info"">Info</option>
            <option value=""Warning"">Warning</option>
            <option value=""Error"">Error</option>
        </select>
        <input type=""date"" id=""log-date"" />
        <button onclick=""loadLogs()"">查询</button>
    </div>
    <div class=""card"">
        <div class=""logs-list"" id=""logs-list""></div>
    </div>
</div>";
    }

    private static string GetScripts()
    {
        return @"
function loadLogs() {
    fetch('/api/logs/list').then(function(r) { return r.json(); }).then(function(data) {
        var list = document.getElementById('logs-list');
        list.innerHTML = '<p>暂无日志</p>';
    });
}
window.onload = function() { loadLogs(); };";
    }
}
