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

public class LogView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as LogViewModel;
        if (vm == null) return string.Empty;

        Sb.Append(RenderCommonHead(vm));
        Sb.AppendLine("<div class=\"dashboard-container\">");
        Sb.AppendLine(RenderSidebar());
        Sb.AppendLine("<div class=\"main-content\">");
        Sb.AppendLine("<div class=\"header\"><h1>日志查看</h1></div>");
        
        Sb.AppendLine("<div class=\"card\">");
        Sb.AppendLine($"<div class=\"filter-bar\">");
        Sb.AppendLine($"<select id=\"level-filter\" onchange=\"filterLogs()\">");
        Sb.AppendLine($"<option value=\"all\" {(vm.LevelFilter == "all" ? "selected" : "")}>全部</option>");
        Sb.AppendLine($"<option value=\"info\" {(vm.LevelFilter == "info" ? "selected" : "")}>Info</option>");
        Sb.AppendLine($"<option value=\"warning\" {(vm.LevelFilter == "warning" ? "selected" : "")}>Warning</option>");
        Sb.AppendLine($"<option value=\"error\" {(vm.LevelFilter == "error" ? "selected" : "")}>Error</option>");
        Sb.AppendLine($"</select>");
        Sb.AppendLine($"<input type=\"date\" id=\"start-date\" value=\"{(vm.StartDate?.ToString("yyyy-MM-dd") ?? "")}\">");
        Sb.AppendLine($"<input type=\"date\" id=\"end-date\" value=\"{(vm.EndDate?.ToString("yyyy-MM-dd") ?? "")}\">");
        Sb.AppendLine($"</div>");
        
        if (vm.Logs.Count == 0)
        {
            Sb.AppendLine("<p>暂无日志</p>");
        }
        else
        {
            Sb.AppendLine("<table>");
            Sb.AppendLine("<thead><tr><th>时间</th><th>级别</th><th>来源</th><th>消息</th></tr></thead>");
            Sb.AppendLine("<tbody>");
            foreach (var log in vm.Logs)
            {
                var levelClass = log.Level.ToLower() switch
                {
                    "error" => "log-error",
                    "warning" => "log-warning",
                    _ => "log-info"
                };
                Sb.AppendLine($"<tr class=\"{levelClass}\">");
                Sb.AppendLine($"<td>{log.Timestamp:yyyy-MM-dd HH:mm:ss}</td>");
                Sb.AppendLine($"<td><span class=\"badge\">{EscapeHtml(log.Level)}</span></td>");
                Sb.AppendLine($"<td>{EscapeHtml(log.Source)}</td>");
                Sb.AppendLine($"<td>{EscapeHtml(log.Message)}</td>");
                Sb.AppendLine($"</tr>");
            }
            Sb.AppendLine("</tbody>");
            Sb.AppendLine("</table>");
            
            Sb.AppendLine("<div class=\"pagination\">");
            if (vm.Page > 1)
                Sb.AppendLine($"<a href=\"/logs?page={vm.Page - 1}\" class=\"btn\">上一页</a>");
            Sb.AppendLine($"<span>第 {vm.Page} / {vm.TotalPages} 页</span>");
            if (vm.Page < vm.TotalPages)
                Sb.AppendLine($"<a href=\"/logs?page={vm.Page + 1}\" class=\"btn\">下一页</a>");
            Sb.AppendLine("</div>");
        }
        Sb.AppendLine("</div>");
        
        Sb.AppendLine("</div>");
        Sb.AppendLine("</div>");
        Sb.AppendLine(RenderFooter());
        
        return Sb.ToString();
    }
}
