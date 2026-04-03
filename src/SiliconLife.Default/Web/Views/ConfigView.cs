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

public class ConfigView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as ConfigViewModel;
        if (vm == null) return string.Empty;

        Sb.Append(RenderCommonHead(vm));
        Sb.AppendLine("<div class=\"dashboard-container\">");
        Sb.AppendLine(RenderSidebar());
        Sb.AppendLine("<div class=\"main-content\">");
        Sb.AppendLine("<div class=\"header\"><h1>系统配置</h1></div>");
        
        foreach (var section in vm.Sections)
        {
            Sb.AppendLine($"<div class=\"card\">");
            Sb.AppendLine($"<h3>{EscapeHtml(section.Value.Name)}</h3>");
            Sb.AppendLine("<table>");
            foreach (var kv in section.Value.Values)
            {
                Sb.AppendLine("<tr>");
                Sb.AppendLine($"<td><label>{EscapeHtml(kv.Key)}</label></td>");
                Sb.AppendLine($"<td><input type=\"text\" name=\"{EscapeHtml(kv.Key)}\" value=\"{EscapeHtml(kv.Value)}\"></td>");
                Sb.AppendLine("</tr>");
            }
            Sb.AppendLine("</table>");
            Sb.AppendLine($"<button class=\"btn\" onclick=\"saveConfig('{section.Key}')\">保存</button>");
            Sb.AppendLine("</div>");
        }
        
        Sb.AppendLine("</div>");
        Sb.AppendLine("</div>");
        Sb.AppendLine(RenderFooter());
        
        return Sb.ToString();
    }
}
