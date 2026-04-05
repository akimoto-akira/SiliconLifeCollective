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

using SiliconLife.Default.Web.Models;

namespace SiliconLife.Default.Web.Views;

public class ChatView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as ChatViewModel;
        if (vm == null) return string.Empty;

        var body = RenderChatBody(vm);
        return RenderPage(vm.Skin, "聊天 - Silicon Life Collective", vm.ActiveMenu, body,
            GetScripts(vm).Build(), GetStyles().Build());
    }

    private static string RenderChatBody(ChatViewModel vm)
    {
        var sessionItems = new List<object>();
        foreach (var session in vm.Sessions)
        {
            var isActive = vm.CurrentBeingId.HasValue && session.Id == vm.CurrentBeingId.Value;
            sessionItems.Add(H.Div(
                H.Div(session.Name).Class("being-name"),
                H.Div(session.LastMessage).Class("being-last-message")
            ).Class("being-item" + (isActive ? " active" : "")).Data("id", session.Id));
        }

        var msgItems = new List<object>();
        foreach (var msg in vm.Messages)
        {
            msgItems.Add(RenderMessage(msg));
        }

        var html = H.Div(
            H.Div(
                H.Div("会话").Class("chat-conversations-header"),
                H.Div(sessionItems.ToArray()).Id("beings-list").Class("chat-conversations-list")
            ).Class("chat-conversations"),
            H.Div(
                H.Div(
                    H.Span(string.IsNullOrEmpty(vm.CurrentBeingName) ? "选择会话开始聊天" : vm.CurrentBeingName)
                        .Class("chat-header-name")
                ).Id("chat-header").Class("chat-header"),
                H.Div(msgItems.ToArray()).Id("chat-messages").Class("chat-messages"),
                H.Div(
                    H.Textarea().Id("message-input").Placeholder("输入消息..."),
                    H.Div(H.Button("发送").OnClick("sendMessage()")).Class("send-button")
                ).Class("chat-input-area")
            ).Class("chat-main")
        ).Class("chat-layout");

        return html.Build();
    }

    private static H RenderMessage(ChatMessage msg)
    {
        if (msg.IsUser)
        {
            return H.Div(
                H.Div(
                    msg.Text ?? "",
                    H.When(!string.IsNullOrEmpty(msg.Time), H.Div(msg.Time!).Class("msg-time"))
                ).Class("msg-user-bubble")
            ).Class("msg-user");
        }

        var children = new List<object>();

        if (!string.IsNullOrEmpty(msg.SenderName))
            children.Add(H.Div(msg.SenderName).Class("msg-being-sender"));

        var bodyChildren = new List<object>();
        if (!string.IsNullOrEmpty(msg.Text))
            bodyChildren.Add(H.Div(msg.Text).Class("msg-being-text"));

        if (!string.IsNullOrEmpty(msg.Thinking))
        {
            bodyChildren.Add(H.Details(
                H.Summary("💭 思考过程"),
                H.Div(msg.Thinking).Class("msg-thinking-content")
            ).Class("msg-collapsible"));
        }

        var tools = msg.ToolCalls?.ToList();
        if (tools != null && tools.Count > 0)
        {
            var toolItems = new List<object>();
            foreach (var tool in tools)
            {
                var icon = tool.Success ? "✓" : "✗";
                var cls = tool.Success ? "msg-tool-success" : "msg-tool-fail";
                var target = !string.IsNullOrEmpty(tool.Target) ? $" · {tool.Target}" : "";
                toolItems.Add(H.Div(
                    H.Span(icon).Class($"tool-status {cls}"),
                    H.Span($"{tool.Name}{target}")
                ).Class("msg-tool-item"));
            }
            bodyChildren.Add(H.Details(
                H.Summary($"🔧 工具调用 ({tools.Count}项)"),
                H.Div(toolItems.ToArray()).Class("msg-tools-list")
            ).Class("msg-collapsible"));
        }

        children.Add(H.Div(bodyChildren.ToArray()).Class("msg-being-body"));

        if (!string.IsNullOrEmpty(msg.Time))
            children.Add(H.Div(msg.Time).Class("msg-time"));

        return H.Div(
            H.Div(children.ToArray()).Class("msg-being-card")
        ).Class("msg-being");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Comment("Remove body whitespace")
            .Selector("body")
                .Property("margin", "0")
                .Property("padding", "0")
            .EndSelector()

            .Comment("Chat Layout")
            .Selector(".chat-layout")
                .Property("display", "flex")
                .Property("height", "100%")
                .Property("overflow", "hidden")
            .EndSelector()

            .Comment("Conversation Sidebar")
            .Selector(".chat-conversations")
                .Property("width", "280px")
                .Property("background", "var(--bg-card)")
                .Property("border-right", "1px solid var(--border)")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("flex-shrink", "0")
            .EndSelector()
            .Selector(".chat-conversations-header")
                .Property("padding", "16px 20px")
                .Property("font-weight", "600")
                .Property("font-size", "15px")
                .Property("border-bottom", "1px solid var(--border)")
            .EndSelector()
            .Selector(".chat-conversations-list")
                .Property("flex", "1")
                .Property("overflow-y", "auto")
            .EndSelector()
            .Selector(".being-item")
                .Property("padding", "12px 20px")
                .Property("cursor", "pointer")
                .Property("border-bottom", "1px solid var(--border)")
                .Property("transition", "background 0.15s")
            .EndSelector()
            .Selector(".being-item:hover")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.05))")
            .EndSelector()
            .Selector(".being-item.active")
                .Property("background", "rgba(77,150,255,0.1)")
                .Property("border-left", "3px solid var(--accent-primary)")
            .EndSelector()
            .Selector(".being-name")
                .Property("font-size", "14px")
                .Property("font-weight", "500")
                .Property("color", "var(--text-primary)")
                .Property("margin-bottom", "4px")
            .EndSelector()
            .Selector(".being-last-message")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
                .Property("overflow", "hidden")
                .Property("text-overflow", "ellipsis")
                .Property("white-space", "nowrap")
            .EndSelector()

            .Comment("Chat Main Area")
            .Selector(".chat-main")
                .Property("flex", "1")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("overflow", "hidden")
            .EndSelector()
            .Selector(".chat-header")
                .Property("padding", "14px 20px")
                .Property("border-bottom", "1px solid var(--border)")
                .Property("background", "var(--bg-card)")
                .Property("flex-shrink", "0")
            .EndSelector()
            .Selector(".chat-header-name")
                .Property("font-size", "15px")
                .Property("font-weight", "600")
                .Property("color", "var(--text-primary)")
            .EndSelector()

            .Comment("Messages")
            .Selector(".chat-messages")
                .Property("flex", "1")
                .Property("overflow-y", "auto")
                .Property("padding", "20px")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "16px")
            .EndSelector()

            .Comment("User Message")
            .Selector(".msg-user")
                .Property("display", "flex")
                .Property("justify-content", "flex-end")
            .EndSelector()
            .Selector(".msg-user-bubble")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#fff")
                .Property("padding", "10px 16px")
                .Property("border-radius", "16px 16px 4px 16px")
                .Property("max-width", "70%")
                .Property("font-size", "14px")
                .Property("line-height", "1.6")
                .Property("word-break", "break-word")
            .EndSelector()

            .Comment("Being Message")
            .Selector(".msg-being")
                .Property("display", "flex")
                .Property("justify-content", "flex-start")
            .EndSelector()
            .Selector(".msg-being-card")
                .Property("background", "var(--bg-card)")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "12px")
                .Property("max-width", "80%")
                .Property("overflow", "hidden")
            .EndSelector()
            .Selector(".msg-being-sender")
                .Property("padding", "10px 16px 0")
                .Property("font-size", "13px")
                .Property("font-weight", "600")
                .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".msg-being-body")
                .Property("padding", "8px 16px 12px")
            .EndSelector()
            .Selector(".msg-being-text")
                .Property("font-size", "14px")
                .Property("line-height", "1.7")
                .Property("color", "var(--text-primary)")
                .Property("white-space", "pre-wrap")
                .Property("word-break", "break-word")
            .EndSelector()

            .Comment("Collapsible sections")
            .Selector(".msg-collapsible")
                .Property("margin-top", "8px")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "8px")
                .Property("font-size", "13px")
            .EndSelector()
            .Selector(".msg-collapsible summary")
                .Property("padding", "8px 12px")
                .Property("cursor", "pointer")
                .Property("color", "var(--text-secondary)")
                .Property("font-weight", "500")
                .Property("user-select", "none")
            .EndSelector()
            .Selector(".msg-collapsible summary:hover")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".msg-thinking-content")
                .Property("padding", "0 12px 10px")
                .Property("color", "var(--text-secondary)")
                .Property("font-size", "13px")
                .Property("line-height", "1.6")
                .Property("white-space", "pre-wrap")
                .Property("border-top", "1px solid var(--border)")
                .Property("padding-top", "8px")
            .EndSelector()

            .Comment("Tool Calls")
            .Selector(".msg-tools-list")
                .Property("padding", "0 12px 10px")
                .Property("border-top", "1px solid var(--border)")
                .Property("padding-top", "8px")
            .EndSelector()
            .Selector(".msg-tool-item")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "8px")
                .Property("padding", "4px 0")
                .Property("font-size", "13px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".tool-status")
                .Property("font-size", "12px")
                .Property("font-weight", "bold")
                .Property("width", "18px")
                .Property("text-align", "center")
            .EndSelector()
            .Selector(".msg-tool-success")
                .Property("color", "var(--accent-success)")
            .EndSelector()
            .Selector(".msg-tool-fail")
                .Property("color", "var(--accent-error)")
            .EndSelector()

            .Comment("Message Time")
            .Selector(".msg-time")
                .Property("font-size", "11px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-top", "6px")
                .Property("text-align", "right")
            .EndSelector()

            .Comment("Input Area")
            .Selector(".chat-input-area")
                .Property("padding", "16px 20px")
                .Property("border-top", "1px solid var(--border)")
                .Property("background", "var(--bg-card)")
                .Property("display", "flex")
                .Property("gap", "12px")
                .Property("align-items", "flex-end")
                .Property("flex-shrink", "0")
            .EndSelector()
            .Selector(".chat-input-area textarea")
                .Property("flex", "1")
                .Property("padding", "10px 14px")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "8px")
                .Property("background", "var(--bg-primary)")
                .Property("color", "var(--text-primary)")
                .Property("font-size", "14px")
                .Property("resize", "none")
                .Property("min-height", "40px")
                .Property("max-height", "120px")
                .Property("font-family", "inherit")
                .Property("line-height", "1.5")
            .EndSelector()
            .Selector(".chat-input-area textarea:focus")
                .Property("outline", "none")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".send-button button")
                .Property("padding", "10px 20px")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#fff")
                .Property("border", "none")
                .Property("border-radius", "8px")
                .Property("cursor", "pointer")
                .Property("font-size", "14px")
                .Property("font-weight", "500")
                .Property("white-space", "nowrap")
                .Property("transition", "opacity 0.2s")
            .EndSelector()
            .Selector(".send-button button:hover")
                .Property("opacity", "0.85")
            .EndSelector()

            .Comment("Responsive")
            .Media("(max-width: 768px)")
                .Selector(".chat-conversations")
                    .Property("width", "220px")
                .EndSelector()
            .EndMedia()
            .Media("(max-width: 480px)")
                .Selector(".chat-conversations")
                    .Property("display", "none")
                .EndSelector()
            .EndMedia();
    }

    private static JsBuilder GetScripts(ChatViewModel vm)
    {
        var js = JsBuilder.Create();
        var currentSessionId = vm.CurrentBeingId?.ToString() ?? "null";

        js.Comment("Chat page initialization")
            .Let("currentSessionId", currentSessionId)
            .EmptyLine()
            .Comment("Auto-resize textarea")
            .Function("autoResize", fb => fb
                .Param("textarea")
                .EndParams()
                .Assign("textarea.style.height", "'auto'")
                .Assign("textarea.style.height", "Math.min(textarea.scrollHeight, 120) + 'px'")
                .EndBlock()
            )
            .EmptyLine()
            .Comment("Send message")
            .Function("sendMessage", fb => fb
                .EndParams()
                .Let("input", "document.getElementById('message-input')")
                .Let("text", "input.value.trim()")
                .If("!text || !currentSessionId")
                    .Return()
                .EndBlock()
                .Let("messages", "document.getElementById('chat-messages')")
                .Let("userDiv", "document.createElement('div')")
                .Assign("userDiv.className", "'msg-user'")
                .Assign("userDiv.innerHTML", "'<div class=\"msg-user-bubble\">' + text + '</div>'")
                .CallMethod("messages", "appendChild", "userDiv")
                .Assign("input.value", "''")
                .Call("autoResize", "input")
                .CallMethod("messages", "scrollTo",
                    "{ top: messages.scrollHeight, behavior: 'smooth' }")
                .EmptyLine()
                .Comment("Send to server")
                .Fetch("'/api/chat/send'",
                    "{ method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ sessionId: currentSessionId, message: text }) }",
                    chain => chain
                        .ThenReturn("r", "r.json()")
                        .Then("data", d => d
                            .If("data.reply")
                                .Call("appendMessage", "data.reply")
                            .EndBlock()
                        )
                        .CatchLog("err", "'Send failed:'")
                )
                .EndBlock()
            )
            .EmptyLine()
            .Comment("Append a message (user or being)")
            .Function("appendMessage", fb => fb
                .Param("msg")
                .EndParams()
                .Let("messages", "document.getElementById('chat-messages')")
                .If("msg.isUser")
                    .Let("div", "document.createElement('div')")
                    .Assign("div.className", "'msg-user'")
                    .Let("bubble", "document.createElement('div')")
                    .Assign("bubble.className", "'msg-user-bubble'")
                    .Assign("bubble.textContent", "msg.text || ''")
                    .CallMethod("div", "appendChild", "bubble")
                    .If("msg.time")
                        .Let("timeEl", "document.createElement('div')")
                        .Assign("timeEl.className", "'msg-time'")
                        .Assign("timeEl.textContent", "msg.time")
                        .CallMethod("div", "appendChild", "timeEl")
                    .EndBlock()
                    .CallMethod("messages", "appendChild", "div")
                .Else()
                    .Let("div", "document.createElement('div')")
                    .Assign("div.className", "'msg-being'")
                    .Let("html", "'<div class=\"msg-being-card\"><div class=\"msg-being-body\">'")
                    .If("msg.senderName")
                        .AppendAssign("html", "'<div class=\"msg-being-sender\">' + msg.senderName + '</div>'")
                    .EndBlock()
                    .AppendAssign("html", "'<div class=\"msg-being-text\">' + (msg.text || '') + '</div>'")
                    .If("msg.thinking")
                        .AppendAssign("html", "'<details class=\"msg-collapsible\"><summary>💭 思考过程</summary><div class=\"msg-thinking-content\">' + msg.thinking + '</div></details>'")
                    .EndBlock()
                    .If("msg.toolCalls && msg.toolCalls.length > 0")
                        .AppendAssign("html", "'<details class=\"msg-collapsible\"><summary>🔧 工具调用 (' + msg.toolCalls.length + '项)</summary><div class=\"msg-tools-list\">'")
                        .ForEach("msg.toolCalls", "t", t => t
                            .Const("icon", "t.success ? '✓' : '✗'")
                            .Const("cls", "t.success ? 'msg-tool-success' : 'msg-tool-fail'")
                            .AppendAssign("html", "'<div class=\"msg-tool-item\"><span class=\"tool-status ' + cls + '\">' + icon + '</span><span>' + t.name + (t.target ? ' · ' + t.target : '') + '</span></div>'")
                            .EndBlock()
                        )
                        .AppendAssign("html", "'</div></details>'")
                    .EndBlock()
                    .AppendAssign("html", "'</div></div>'")
                    .If("msg.time")
                        .AppendAssign("html", "'<div class=\"msg-time\">' + msg.time + '</div>'")
                    .EndBlock()
                    .Assign("div.innerHTML", "html")
                    .CallMethod("messages", "appendChild", "div")
                .EndBlock()
                .CallMethod("messages", "scrollTo",
                    "{ top: messages.scrollHeight, behavior: 'smooth' }")
                .EndBlock()
            )
            .EmptyLine()
            .Comment("Session selection")
            .Function("selectSession", fb => fb
                .Param("sessionId").Param("name")
                .EndParams()
                .Assign("currentSessionId", "sessionId")
                .Assign("document.querySelector('.chat-header-name').textContent", "name")
                .CallMethod("document.getElementById('chat-messages')", "setAttribute",
                    "'data-session-id'", "sessionId")
                .Comment("Load history for selected session")
                .Fetch("'/api/chat/history?sessionId=' + sessionId", chain: chain =>
                    chain.ThenReturn("r", "r.json()")
                    .Then("data", d => d
                        .Const("container", "document.getElementById('chat-messages')")
                        .Assign("container.innerHTML", "''")
                        .ForEachExpr("data.messages", "m", "appendMessage(m)")
                        .Assign("container.scrollTop", "container.scrollHeight")
                    )
                    .CatchLog("err", "'Load history failed:'")
                )
                .EndBlock()
            )
            .EmptyLine()
            .Comment("Keyboard shortcut: Enter to send")
            .Function("initInput", fb => fb
                .EndParams()
                .Let("input", "document.getElementById('message-input')")
                .On("input", "keydown", "e", e => e
                    .If("e.key === 'Enter' && !e.shiftKey")
                        .CallMethod("e", "preventDefault")
                        .Call("sendMessage")
                    .EndBlock()
                    .EndBlock()
                )
                .Call("autoResize", "input")
                .EndBlock()
            )
            .EmptyLine()
            .Comment("Bind click events to session items")
            .Function("initSessionList", fb => fb
                .EndParams()
                .ForEach("document.querySelectorAll('.being-item')", "item", item => item
                    .On("item", "click", "e", body => body
                        .ForEachExpr("document.querySelectorAll('.being-item')", "el", "el.classList.remove('active')")
                        .CallMethod("item.classList", "add", "'active'")
                        .Call("selectSession", "item.dataset.id", "item.querySelector('.being-name').textContent")
                        .EndBlock()
                    )
                    .EndBlock()
                )
                .EndBlock()
            )
            .EmptyLine()
            .Comment("Load conversation list from API")
            .Function("loadConversations", fb => fb
                .EndParams()
                .Fetch("'/api/chat/conversations'", chain: chain =>
                    chain.Then("r", r => r
                        .If("!r.ok")
                            .Throw("'HTTP ' + r.status")
                        .EndBlock()
                        .Return("r.json()")
                    )
                    .Then("data", d => d
                        .If("data.error")
                            .Assign("document.getElementById('beings-list').innerHTML", "'<div style=\"padding:12px 20px;color:var(--text-secondary);font-size:12px;\">Error: ' + data.error + '</div>'")
                            .Return()
                        .EndBlock()
                        .Let("list", "document.getElementById('beings-list')")
                        .Assign("list.innerHTML", "''")
                        .ForEach("data", "item", item => item
                            .Let("div", "document.createElement('div')")
                            .Assign("div.className", "'being-item'")
                            .Assign("div.dataset.id", "item.sessionId")
                            .Assign("div.innerHTML", "'<div class=\"being-name\">' + item.displayName + '</div>' + '<div class=\"being-last-message\">' + (item.lastMessage || '') + '</div>'")
                            .CallMethod("list", "appendChild", "div")
                            .EndBlock()
                        )
                        .Call("initSessionList")
                    )
                    .Catch("err", e => e
                        .ConsoleError("'Load conversations failed:'", "err")
                        .Assign("document.getElementById('beings-list').innerHTML", "'<div style=\"padding:12px 20px;color:var(--text-secondary);font-size:12px;\">Failed to load conversations</div>'")
                    )
                )
                .EndBlock()
            )
            .EmptyLine()
            .Comment("Init on load")
            .Call("initInput")
            .Call("loadConversations")
            .CallMethod("document.getElementById('chat-messages')", "scrollTo",
                "{ top: 999999, behavior: 'auto' }");

        return js;
    }
}
