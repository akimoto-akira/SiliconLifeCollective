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

public class PermissionRequestView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as PermissionRequestViewModel;
        if (vm == null) return string.Empty;

        Sb.Append(RenderCommonHead(vm));
        Sb.AppendLine("<div class=\"dashboard-container\">");
        Sb.AppendLine(RenderSidebar());
        Sb.AppendLine("<div class=\"main-content\">");
        Sb.AppendLine("<div class=\"header\"><h1>权限请求</h1></div>");
        
        Sb.AppendLine("<div class=\"card\">");
        Sb.AppendLine($"<div class=\"filter-bar\">");
        Sb.AppendLine($"<select id=\"status-filter\" onchange=\"filterRequests()\">");
        Sb.AppendLine($"<option value=\"pending\" {(vm.StatusFilter == "pending" ? "selected" : "")}>待处理</option>");
        Sb.AppendLine($"<option value=\"approved\" {(vm.StatusFilter == "approved" ? "selected" : "")}>已批准</option>");
        Sb.AppendLine($"<option value=\"rejected\" {(vm.StatusFilter == "rejected" ? "selected" : "")}>已拒绝</option>");
        Sb.AppendLine($"</select>");
        Sb.AppendLine($"</div>");
        
        if (vm.Requests.Count == 0)
        {
            Sb.AppendLine("<p>暂无权限请求</p>");
        }
        else
        {
            Sb.AppendLine("<table>");
            Sb.AppendLine("<thead><tr><th>申请人</th><th>权限名称</th><th>理由</th><th>状态</th><th>申请时间</th><th>操作</th></tr></thead>");
            Sb.AppendLine("<tbody>");
            foreach (var req in vm.Requests)
            {
                Sb.AppendLine("<tr>");
                Sb.AppendLine($"<td>{EscapeHtml(req.RequesterName)}</td>");
                Sb.AppendLine($"<td>{EscapeHtml(req.PermissionName)}</td>");
                Sb.AppendLine($"<td>{EscapeHtml(req.Reason)}</td>");
                Sb.AppendLine($"<td><span class=\"badge\">{EscapeHtml(req.Status)}</span></td>");
                Sb.AppendLine($"<td>{req.RequestedAt:yyyy-MM-dd HH:mm}</td>");
                if (req.Status == "pending")
                {
                    Sb.AppendLine($"<td>");
                    Sb.AppendLine($"<button class=\"btn\" onclick=\"approveRequest('{req.Id}')\">批准</button>");
                    Sb.AppendLine($"<button class=\"btn btn-danger\" onclick=\"rejectRequest('{req.Id}')\">拒绝</button>");
                    Sb.AppendLine($"</td>");
                }
                else
                {
                    Sb.AppendLine("<td>-</td>");
                }
                Sb.AppendLine("</tr>");
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
