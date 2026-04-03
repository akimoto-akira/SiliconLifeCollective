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

public class TaskView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as TaskViewModel;
        if (vm == null) return string.Empty;

        Sb.Append(RenderCommonHead(vm));
        Sb.AppendLine("<div class=\"dashboard-container\">");
        Sb.AppendLine(RenderSidebar());
        Sb.AppendLine("<div class=\"main-content\">");
        Sb.AppendLine("<div class=\"header\"><h1>任务管理</h1></div>");
        
        Sb.AppendLine("<div class=\"card\">");
        Sb.AppendLine($"<div class=\"filter-bar\">");
        Sb.AppendLine($"<select id=\"filter\" onchange=\"filterTasks()\">");
        Sb.AppendLine($"<option value=\"all\" {(vm.Filter == "all" ? "selected" : "")}>全部</option>");
        Sb.AppendLine($"<option value=\"pending\" {(vm.Filter == "pending" ? "selected" : "")}>待处理</option>");
        Sb.AppendLine($"<option value=\"running\" {(vm.Filter == "running" ? "selected" : "")}>运行中</option>");
        Sb.AppendLine($"<option value=\"completed\" {(vm.Filter == "completed" ? "selected" : "")}>已完成</option>");
        Sb.AppendLine($"</select>");
        Sb.AppendLine($"</div>");
        
        if (vm.Tasks.Count == 0)
        {
            Sb.AppendLine("<p>暂无任务</p>");
        }
        else
        {
            Sb.AppendLine("<table>");
            Sb.AppendLine("<thead><tr><th>名称</th><th>状态</th><th>进度</th><th>分配给</th><th>创建时间</th><th>操作</th></tr></thead>");
            Sb.AppendLine("<tbody>");
            foreach (var task in vm.Tasks)
            {
                Sb.AppendLine($"<tr>");
                Sb.AppendLine($"<td>{EscapeHtml(task.Name)}</td>");
                Sb.AppendLine($"<td><span class=\"badge\">{EscapeHtml(task.Status)}</span></td>");
                Sb.AppendLine($"<td>{EscapeHtml(task.Progress)}</td>");
                Sb.AppendLine($"<td>{EscapeHtml(task.AssignedTo)}</td>");
                Sb.AppendLine($"<td>{task.CreatedAt:yyyy-MM-dd HH:mm}</td>");
                Sb.AppendLine($"<td><a href=\"/tasks/detail/{task.Id}\" class=\"btn\">查看</a></td>");
                Sb.AppendLine($"</tr>");
            }
            Sb.AppendLine("</tbody>");
            Sb.AppendLine("</table>");
        }
        Sb.AppendLine("</div>");
        
        Sb.AppendLine("</div>");
        Sb.AppendLine("</div>");
        Sb.AppendLine(RenderFooter());
        
        return Sb.ToString();
    }
}
