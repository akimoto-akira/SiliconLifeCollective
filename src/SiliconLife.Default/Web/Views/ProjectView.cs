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

public class ProjectView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as ProjectViewModel;
        if (vm == null) return string.Empty;

        Sb.Append(RenderCommonHead(vm));
        Sb.AppendLine("<div class=\"dashboard-container\">");
        Sb.AppendLine(RenderSidebar());
        Sb.AppendLine("<div class=\"main-content\">");
        Sb.AppendLine("<div class=\"header\"><h1>项目空间管理</h1></div>");
        
        Sb.AppendLine("<div class=\"card\">");
        if (vm.Projects.Count == 0)
        {
            Sb.AppendLine("<p>暂无项目</p>");
        }
        else
        {
            Sb.AppendLine("<table>");
            Sb.AppendLine("<thead><tr><th>名称</th><th>描述</th><th>状态</th><th>创建时间</th><th>操作</th></tr></thead>");
            Sb.AppendLine("<tbody>");
            foreach (var project in vm.Projects)
            {
                Sb.AppendLine($"<tr>");
                Sb.AppendLine($"<td>{EscapeHtml(project.Name)}</td>");
                Sb.AppendLine($"<td>{EscapeHtml(project.Description)}</td>");
                Sb.AppendLine($"<td><span class=\"badge\">{EscapeHtml(project.Status)}</span></td>");
                Sb.AppendLine($"<td>{project.CreatedAt:yyyy-MM-dd HH:mm}</td>");
                Sb.AppendLine($"<td><a href=\"/projects/edit/{project.Id}\" class=\"btn\">编辑</a></td>");
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
