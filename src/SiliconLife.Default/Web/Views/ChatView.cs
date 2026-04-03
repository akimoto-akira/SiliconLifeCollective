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

public class ChatView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as ChatViewModel;
        if (vm == null) return string.Empty;

        Sb.Append(RenderCommonHead(vm));
        Sb.AppendLine("<div class=\"dashboard-container\">");
        Sb.AppendLine(RenderSidebar());
        Sb.AppendLine("<div class=\"main-content\">");
        Sb.AppendLine("<div class=\"header\"><h1>聊天</h1></div>");
        
        Sb.AppendLine("<div class=\"chat-container\">");
        Sb.AppendLine("<div class=\"chat-sidebar\">");
        Sb.AppendLine("<h3>会话列表</h3>");
        if (vm.Sessions.Count == 0)
        {
            Sb.AppendLine("<p>暂无会话</p>");
        }
        else
        {
            Sb.AppendLine("<ul class=\"session-list\">");
            foreach (var session in vm.Sessions)
            {
                Sb.AppendLine($"<li class=\"session-item\" data-id=\"{session.Id}\">");
                Sb.AppendLine($"<div class=\"session-name\">{EscapeHtml(session.Name)}</div>");
                Sb.AppendLine($"<div class=\"session-preview\">{EscapeHtml(session.LastMessage)}</div>");
                Sb.AppendLine($"</li>");
            }
            Sb.AppendLine("</ul>");
        }
        Sb.AppendLine("</div>");
        
        Sb.AppendLine("<div class=\"chat-main\">");
        Sb.AppendLine("<div class=\"chat-messages\" id=\"chat-messages\"></div>");
        Sb.AppendLine("<div class=\"chat-input\">");
        Sb.AppendLine("<textarea id=\"message-input\" placeholder=\"输入消息...\" rows=\"3\"></textarea>");
        Sb.AppendLine("<button class=\"btn\" id=\"send-btn\">发送</button>");
        Sb.AppendLine("</div>");
        Sb.AppendLine("</div>");
        
        Sb.AppendLine("</div>");
        Sb.AppendLine("</div>");
        Sb.AppendLine("</div>");
        Sb.AppendLine(RenderFooter());
        
        return Sb.ToString();
    }
}
