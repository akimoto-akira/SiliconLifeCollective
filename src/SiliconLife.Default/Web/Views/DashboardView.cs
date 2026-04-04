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

public class DashboardView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as DashboardViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, "仪表盘 - Silicon Life Collective", "dashboard", body, GetScripts(), GetStyles());
    }

    private static string RenderBody(DashboardViewModel vm)
    {
        return @"
<div class=""page-content"">
    <div class=""page-header""><h1>仪表盘</h1></div>
    <div class=""stats-grid"">
        <div class=""stat-card""><h3>硅基人数量</h3><div class=""stat-value"" id=""being-count"">0</div></div>
        <div class=""stat-card""><h3>活跃硅基人</h3><div class=""stat-value"" id=""active-beings"">0</div></div>
        <div class=""stat-card""><h3>运行时间</h3><div class=""stat-value"" id=""uptime"">00:00:00</div></div>
        <div class=""stat-card""><h3>内存占用</h3><div class=""stat-value"" id=""memory"">0 MB</div></div>
    </div>
    <div class=""card"">
        <h3>消息频率</h3>
        <div class=""chart-container""><svg id=""message-chart"" viewBox=""0 0 800 300"" preserveAspectRatio=""xMidYMid meet""></svg></div>
    </div>
</div>";
    }

    private static string GetStyles()
    {
        return @"
.chart-container { width: 100%; }
.chart-container svg { width: 100%; height: 300px; }";
    }

    private static string GetScripts()
    {
        var sb = new StringBuilder();
        sb.AppendLine("function updateStats() {");
        sb.AppendLine("    fetch('/api/dashboard/stats').then(function(r) { return r.json(); }).then(function(data) {");
        sb.AppendLine("        document.getElementById('being-count').textContent = data.beingCount;");
        sb.AppendLine("        document.getElementById('uptime').textContent = data.uptime;");
        sb.AppendLine("        document.getElementById('memory').textContent = data.memoryMB + ' MB';");
        sb.AppendLine("    });");
        sb.AppendLine("}");
        sb.AppendLine("function updateChart() {");
        sb.AppendLine("    fetch('/api/dashboard/metrics').then(function(r) { return r.json(); }).then(function(data) {");
        sb.AppendLine("        var svg = document.getElementById('message-chart');");
        sb.AppendLine("        var width = 800, height = 300, padding = 40;");
        sb.AppendLine("        var maxVal = Math.max.apply(null, data.messageCounts.concat([10]));");
        sb.AppendLine("        var xStep = (width - padding * 2) / (data.timestamps.length - 1);");
        sb.AppendLine("        var points = data.messageCounts.map(function(val, i) {");
        sb.AppendLine("            return (padding + i * xStep) + ',' + (height - padding - (val / maxVal) * (height - padding * 2));");
        sb.AppendLine("        }).join(' ');");
        sb.AppendLine("        var polyline = '<polyline points=\"' + points + '\" fill=\"none\" stroke=\"var(--accent-primary)\" stroke-width=\"2\"/>';");
        sb.AppendLine("        var axes = '<line x1=\"' + padding + '\" y1=\"' + (height-padding) + '\" x2=\"' + (width-padding) + '\" y2=\"' + (height-padding) + '\" stroke=\"var(--border)\" stroke-width=\"1\"/>';");
        sb.AppendLine("        axes += '<line x1=\"' + padding + '\" y1=\"' + padding + '\" x2=\"' + padding + '\" y2=\"' + (height-padding) + '\" stroke=\"var(--border)\" stroke-width=\"1\"/>';");
        sb.AppendLine("        svg.innerHTML = axes + polyline;");
        sb.AppendLine("    });");
        sb.AppendLine("}");
        sb.AppendLine("setInterval(updateStats, 5000);");
        sb.AppendLine("setInterval(updateChart, 10000);");
        sb.AppendLine("window.onload = function() { updateStats(); updateChart(); };");
        return sb.ToString();
    }
}
