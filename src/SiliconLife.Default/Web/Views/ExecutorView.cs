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

public class ExecutorView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as ViewModelBase;
        if (vm == null) return string.Empty;
        var body = RenderBody();
        return RenderPage(vm.Skin, "执行器监控 - Silicon Life Collective", "beings", body, GetScripts(), GetStyles());
    }

    private static string RenderBody()
    {
        return @"
<div class=""page-content"">
    <div class=""page-header""><h1>执行器监控</h1></div>
    <div class=""executors-grid"" id=""executors-grid""></div>
</div>";
    }

    private static string GetStyles()
    {
        return @"
.executors-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: 20px; }
.executor-card { background: var(--bg-card); padding: 20px; border-radius: 12px; border: 1px solid var(--border); }
.executor-name { font-size: 18px; font-weight: bold; color: var(--text-primary); margin-bottom: 10px; }
.executor-status { display: inline-block; padding: 4px 12px; border-radius: 12px; font-size: 12px; background: rgba(107,203,119,0.15); color: var(--accent-success); }
.executor-queue { font-size: 14px; color: var(--text-secondary); margin-top: 8px; }";
    }

    private static string GetScripts()
    {
        return @"
function loadExecutors() {
    fetch('/api/executors/status').then(function(r) { return r.json(); }).then(function(data) {
        var grid = document.getElementById('executors-grid');
        grid.innerHTML = '';
        data.forEach(function(e) {
            var card = document.createElement('div');
            card.className = 'executor-card';
            card.innerHTML = '<div class=""executor-name"">' + e.name + '</div><span class=""executor-status"">' + e.status + '</span><div class=""executor-queue"">队列: ' + e.queueCount + '</div>';
            grid.appendChild(card);
        });
    });
}
setInterval(loadExecutors, 5000);
window.onload = function() { loadExecutors(); };";
    }
}
