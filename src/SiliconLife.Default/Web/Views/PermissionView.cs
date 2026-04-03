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

public class PermissionView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as PermissionViewModel;
        if (vm == null) return string.Empty;

        Sb.Append(RenderCommonHead(vm));
        Sb.AppendLine("<div class=\"dashboard-container\">");
        Sb.AppendLine(RenderSidebar());
        Sb.AppendLine("<div class=\"main-content\">");
        Sb.AppendLine("<div class=\"header\"><h1>权限管理</h1></div>");
        
        Sb.AppendLine("<div class=\"card\">");
        if (vm.Permissions.Count == 0)
        {
            Sb.AppendLine("<p>暂无权限配置</p>");
        }
        else
        {
            Sb.AppendLine("<table>");
            Sb.AppendLine("<thead><tr><th>名称</th><th>描述</th><th>类型</th><th>状态</th><th>过期时间</th><th>操作</th></tr></thead>");
            Sb.AppendLine("<tbody>");
            foreach (var perm in vm.Permissions)
            {
                Sb.AppendLine($"<tr>");
                Sb.AppendLine($"<td>{EscapeHtml(perm.Name)}</td>");
                Sb.AppendLine($"<td>{EscapeHtml(perm.Description)}</td>");
                Sb.AppendLine($"<td>{EscapeHtml(perm.Type)}</td>");
                Sb.AppendLine($"<td><span class=\"badge {(perm.IsGranted ? "badge-success" : "")}\">{EscapeHtml(perm.IsGranted ? "已授权" : "未授权")}</span></td>");
                Sb.AppendLine($"<td>{(perm.ExpiresAt.HasValue ? perm.ExpiresAt.Value.ToString("yyyy-MM-dd HH:mm") : "永久")}</td>");
                Sb.AppendLine($"<td><button class=\"btn\" onclick=\"togglePermission('{perm.Id}')\">{(perm.IsGranted ? "撤销" : "授权")}</button></td>");
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
