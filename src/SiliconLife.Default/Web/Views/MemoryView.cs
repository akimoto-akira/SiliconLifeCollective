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

public class MemoryView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as MemoryViewModel;
        if (vm == null) return string.Empty;

        Sb.Append(RenderCommonHead(vm));
        Sb.AppendLine("<div class=\"dashboard-container\">");
        Sb.AppendLine(RenderSidebar());
        Sb.AppendLine("<div class=\"main-content\">");
        Sb.AppendLine("<div class=\"header\"><h1>记忆管理</h1></div>");
        
        Sb.AppendLine("<div class=\"card\">");
        Sb.AppendLine("<div class=\"search-bar\">");
        Sb.AppendLine($"<input type=\"text\" id=\"search\" placeholder=\"搜索记忆...\" value=\"{EscapeHtml(vm.SearchQuery)}\">");
        Sb.AppendLine("<button class=\"btn\" onclick=\"search()\">搜索</button>");
        Sb.AppendLine("</div>");
        
        if (vm.Memories.Count == 0)
        {
            Sb.AppendLine("<p>暂无记忆</p>");
        }
        else
        {
            Sb.AppendLine("<table>");
            Sb.AppendLine("<thead><tr><th>类型</th><th>内容</th><th>重要性</th><th>创建时间</th><th>最后访问</th></tr></thead>");
            Sb.AppendLine("<tbody>");
            foreach (var mem in vm.Memories)
            {
                Sb.AppendLine("<tr>");
                Sb.AppendLine($"<td><span class=\"badge\">{EscapeHtml(mem.Type)}</span></td>");
                Sb.AppendLine($"<td>{EscapeHtml(mem.Content.Length > 50 ? mem.Content.Substring(0, 50) + "..." : mem.Content)}</td>");
                Sb.AppendLine($"<td>{mem.Importance:P0}</td>");
                Sb.AppendLine($"<td>{mem.CreatedAt:yyyy-MM-dd HH:mm}</td>");
                Sb.AppendLine($"<td>{mem.LastAccessedAt:yyyy-MM-dd HH:mm}</td>");
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
