// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
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

        Sb.Append(RenderCommonHead(vm));
        Sb.AppendLine("<div class=\"dashboard-container\">");
        Sb.AppendLine(RenderSidebar());
        Sb.AppendLine("<div class=\"main-content\">");
        Sb.AppendLine("<div class=\"header\"><h1>仪表盘</h1></div>");
        
        Sb.AppendLine("<div class=\"stats-grid\">");
        Sb.AppendLine($"<div class=\"stat-card\"><h3>硅基人数量</h3><div class=\"stat-value\">{vm.TotalBeings}</div></div>");
        Sb.AppendLine($"<div class=\"stat-card\"><h3>活跃硅基人</h3><div class=\"stat-value\">{vm.ActiveBeings}</div></div>");
        Sb.AppendLine($"<div class=\"stat-card\"><h3>运行时间</h3><div class=\"stat-value\">{vm.Uptime.Days}天 {vm.Uptime.Hours}时</div></div>");
        Sb.AppendLine($"<div class=\"stat-card\"><h3>总任务数</h3><div class=\"stat-value\">{vm.TotalTasks}</div></div>");
        Sb.AppendLine($"<div class=\"stat-card\"><h3>运行中</h3><div class=\"stat-value\">{vm.RunningTasks}</div></div>");
        Sb.AppendLine($"<div class=\"stat-card\"><h3>CPU使用率</h3><div class=\"stat-value\">{vm.CpuUsage:F1}%</div></div>");
        Sb.AppendLine($"<div class=\"stat-card\"><h3>内存使用率</h3><div class=\"stat-value\">{vm.MemoryUsage:F1}%</div></div>");
        Sb.AppendLine("</div>");
        
        Sb.AppendLine("</div>");
        Sb.AppendLine("</div>");
        Sb.AppendLine(RenderFooter());
        
        return Sb.ToString();
    }
}
