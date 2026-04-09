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
            GetScripts(vm), GetStyles());
    }

    private static H RenderChatBody(ChatViewModel vm)
    {
        var sessionItems = new List<object>();
        foreach (var session in vm.Sessions)
        {
            var isActive = vm.CurrentBeingId.HasValue && session.Id == vm.CurrentBeingId.Value;
            sessionItems.Add(H.Div(
                H.Div(session.Name).Class("being-name"),
                H.Div(session.LastMessage).Class("being-last-message")
            ).Class("being-item" + (isActive ? " active" : "")).Data("id", session.Id.ToString()));
        }

        var msgItems = new List<object>();
        foreach (var msg in vm.Messages)
        {
            msgItems.Add(RenderMessage(msg));
        }

        return H.Div(
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
    }

    private static H RenderMessage(ChatMessage msg)
    {
        if (msg.IsUser)
        {
            var bubble = H.Div(
                msg.Text ?? ""
            ).Class("msg-user-bubble");
            if (!string.IsNullOrEmpty(msg.Time))
                bubble.Add(H.Div(msg.Time).Class("msg-time"));
            return H.Div(bubble).Class("msg-user");
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

    private static JsSyntax GetScripts(ChatViewModel vm)
    {
        var currentSessionId = vm.CurrentBeingId?.ToString() ?? "null";
        var userId = vm.UserId.ToString();

        var js = Js.Block()
            .Add(() => Js.Let(() => "currentSessionId", () => Js.Str(() => currentSessionId)))
            .Add(() => Js.Let(() => "ws", () => Js.Null()))
            .Add(() => Js.Let(() => "currentStreamId", () => Js.Null()))
            .Add(() => Js.Let(() => "streamingMessage", () => Js.Null()));

        var autoResizeBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "textarea").Prop(() => "style").Prop(() => "height"), () => Js.Str(() => "auto")))
            .Add(() => Js.Assign(() => Js.Id(() => "textarea").Prop(() => "style").Prop(() => "height"), () => Js.Id(() => "Math").Call(() => "min", () => Js.Id(() => "textarea").Prop(() => "scrollHeight"), () => Js.Num(() => "120")).Op(() => "+", () => (JsSyntax)Js.Str(() => "px"))));
        js.Add(() => Js.Func(() => "autoResize", () => new List<string> { "textarea" }, () => autoResizeBody));

        var connectWsBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "ws"), () => Js.New(() => Js.Id(() => "WebSocket"), () => Js.Str(() => "ws://").Op(() => "+", () => (JsSyntax)Js.Id(() => "window").Prop(() => "location").Prop(() => "host")).Op(() => "+", () => (JsSyntax)Js.Str(() => "/ws")))))
            .Add(() => Js.Id(() => "ws").Prop(() => "onopen").Assign(() => Js.Arrow(() => new List<string>(), () => Js.Block()
                .Add(() => Js.Id(() => "console").Call(() => "log", () => Js.Str(() => "WebSocket connected")).Stmt())
                .Add(() => Js.Id(() => "ws").Call(() => "send", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Obj()
                    .Prop(() => "type", () => Js.Str(() => "connect"))
                    .Prop(() => "senderId", () => Js.Str(() => userId)))).Stmt())
            )))
            .Add(() => Js.Id(() => "ws").Prop(() => "onmessage").Assign(() => Js.Arrow(() => new List<string> { "event" }, () => Js.Block()
                .Add(() => Js.Const(() => "msg", () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "event").Prop(() => "data"))))
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    { (Js.Id(() => "msg").Prop(() => "type").Op(() => "===", () => (JsSyntax)Js.Str(() => "chat")), new List<JsSyntax>
                        {
                            Js.Id(() => "handleChatMessage").Invoke(() => Js.Id(() => "msg")).Stmt()
                        }
                    )},
                    { (Js.Id(() => "msg").Prop(() => "type").Op(() => "===", () => (JsSyntax)Js.Str(() => "stream_chunk")), new List<JsSyntax>
                        {
                            Js.Id(() => "handleStreamChunk").Invoke(() => Js.Id(() => "msg")).Stmt()
                        }
                    )}
                }))
            )))
            .Add(() => Js.Id(() => "ws").Prop(() => "onclose").Assign(() => Js.Arrow(() => new List<string>(), () => Js.Block()
                .Add(() => Js.Id(() => "console").Call(() => "log", () => Js.Str(() => "WebSocket disconnected, reconnecting...")).Stmt())
                .Add(() => Js.Id(() => "setTimeout").Invoke(() => Js.Id(() => "connectWebSocket"), () => Js.Num(() => "3000")).Stmt())
            )))
            .Add(() => Js.Id(() => "ws").Prop(() => "onerror").Assign(() => Js.Arrow(() => new List<string> { "err" }, () => Js.Block()
                .Add(() => Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "WebSocket error:"), () => Js.Id(() => "err")).Stmt()))));
        js.Add(() => Js.Func(() => "connectWebSocket", () => new List<string>(), () => connectWsBody));

        var handleChatMessageBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "msg").Prop(() => "channelId").Not(), new List<JsSyntax> { Js.Return(() => Js.Id(() => "undefined")) }) }
            }))
            .Add(() => Js.Const(() => "isCurrentUser", () => Js.Id(() => "msg").Prop(() => "senderId").Op(() => "===", () => (JsSyntax)Js.Str(() => userId))))
            .Add(() => Js.Id(() => "appendMessage").Invoke(() => Js.Obj()
                .Prop(() => "isUser", () => Js.Id(() => "isCurrentUser"))
                .Prop(() => "text", () => Js.Id(() => "msg").Prop(() => "content"))).Stmt());
        js.Add(() => Js.Func(() => "handleChatMessage", () => new List<string> { "msg" }, () => handleChatMessageBody));

        var handleStreamChunkBody = Js.Block()
            .Add(() => Js.Const(() => "chunk", () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "msg").Prop(() => "content"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "currentStreamId").Not().Op(() => "||", () => (JsSyntax)Js.Id(() => "currentStreamId").Op(() => "!==", () => Js.Id(() => "chunk").Prop(() => "streamId"))), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "currentStreamId"), () => Js.Id(() => "chunk").Prop(() => "streamId")),
                        Js.Assign(() => Js.Id(() => "streamingMessage"), () => Js.Obj()
                            .Prop(() => "isUser", () => Js.Bool(() => false))
                            .Prop(() => "text", () => Js.Str(() => ""))
                            .Prop(() => "elementId", () => Js.Str(() => "stream-").Op(() => "+", () => (JsSyntax)Js.Id(() => "chunk").Prop(() => "streamId")))),
                        Js.Id(() => "appendMessage").Invoke(() => Js.Id(() => "streamingMessage")).Stmt()
                    }
                )}
            }))
            .Add(() => Js.Const(() => "streamEl", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Id(() => "streamingMessage").Prop(() => "elementId"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "streamEl"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "streamingMessage").Prop(() => "text"), () => Js.Id(() => "streamingMessage").Prop(() => "text").Op(() => "+", () => (JsSyntax)Js.Id(() => "chunk").Prop(() => "content"))),
                        Js.Assign(() => Js.Id(() => "streamEl").Prop(() => "textContent"), () => Js.Id(() => "streamingMessage").Prop(() => "text"))
                    }
                )}
            }))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "chunk").Prop(() => "isFinal"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "currentStreamId"), () => Js.Null()),
                        Js.Assign(() => Js.Id(() => "streamingMessage"), () => Js.Null())
                    }
                )}
            }));
        js.Add(() => Js.Func(() => "handleStreamChunk", () => new List<string> { "msg" }, () => handleStreamChunkBody));

        var textEmptyCond = Js.Id(() => "text").Not().Op(() => "||", () => (JsSyntax)Js.Id(() => "currentSessionId").Not());
        var wsReadyCond = Js.Id(() => "ws").Op(() => "&&", () => (JsSyntax)Js.Id(() => "ws").Prop(() => "readyState").Op(() => "===", () => Js.Id(() => "WebSocket").Prop(() => "OPEN")));
        var sendMessageBody = Js.Block()
            .Add(() => Js.Const(() => "input", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "message-input"))))
            .Add(() => Js.Const(() => "text", () => Js.Id(() => "input").Prop(() => "value").Call(() => "trim")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (textEmptyCond, new List<JsSyntax> { Js.Return(() => Js.Id(() => "undefined")) }) }
            }))
            .Add(() => Js.Const(() => "messages", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "chat-messages"))))
            .Add(() => Js.Const(() => "userDiv", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "userDiv").Prop(() => "className"), () => Js.Str(() => "msg-user")))
            .Add(() => Js.Assign(() => Js.Id(() => "userDiv").Prop(() => "innerHTML"), () => Js.Str(() => "<div class=\"msg-user-bubble\">").Op(() => "+", () => (JsSyntax)Js.Id(() => "text")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>"))))
            .Add(() => Js.Id(() => "messages").Call(() => "appendChild", () => Js.Id(() => "userDiv")).Stmt())
            .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "value"), () => Js.Str(() => "")))
            .Add(() => Js.Id(() => "autoResize").Invoke(() => Js.Id(() => "input")).Stmt())
            .Add(() => Js.Id(() => "messages").Call(() => "scrollTo", () => Js.Obj().Prop(() => "top", () => Js.Id(() => "messages").Prop(() => "scrollHeight")).Prop(() => "behavior", () => Js.Str(() => "smooth"))).Stmt())
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (wsReadyCond, new List<JsSyntax>
                    {
                        Js.Id(() => "ws").Call(() => "send", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Obj()
                            .Prop(() => "type", () => Js.Str(() => "chat"))
                            .Prop(() => "senderId", () => Js.Str(() => userId))
                            .Prop(() => "channelId", () => Js.Id(() => "currentSessionId"))
                            .Prop(() => "content", () => Js.Id(() => "text")))).Stmt()
                    }
                )},
                { (null, new List<JsSyntax>
                    {
                        Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "WebSocket not connected")).Stmt()
                    }
                )}
            }));
        js.Add(() => Js.Func(() => "sendMessage", () => new List<string>(), () => sendMessageBody));

        var dataMessagesCond = Js.Id(() => "data").Prop(() => "messages");
        var forEachMsgStmt = Js.Id(() => "data").Prop(() => "messages").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "m" }, () => Js.Id(() => "appendMessage").Invoke(() => Js.Id(() => "m")))).Stmt();
        var dataMessagesBody = new List<JsSyntax> { forEachMsgStmt };
        var historyThenBody = Js.Block()
            .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "chat-messages"))))
            .Add(() => Js.Assign(() => Js.Id(() => "container").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (dataMessagesCond, dataMessagesBody) }
            }))
            .Add(() => Js.Assign(() => Js.Id(() => "container").Prop(() => "scrollTop"), () => Js.Id(() => "container").Prop(() => "scrollHeight")));
        var selectSessionBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "currentSessionId"), () => Js.Id(() => "sessionId")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "querySelector", () => Js.Str(() => ".chat-header-name")).Prop(() => "textContent"), () => Js.Id(() => "name")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "chat-messages")).Call(() => "setAttribute", () => Js.Str(() => "data-session-id"), () => Js.Id(() => "sessionId")).Stmt())
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/chat/history?sessionId=").Op(() => "+", () => (JsSyntax)Js.Id(() => "sessionId"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => historyThenBody)).Call(() => "catch", () => Js.Arrow(() => new List<string> { "err" }, () => Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "Load history failed:"), () => Js.Id(() => "err")))).Stmt());
        js.Add(() => Js.Func(() => "selectSession", () => new List<string> { "sessionId", "name" }, () => selectSessionBody));

        var keydownHandlerBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "e").Prop(() => "key").Op(() => "===", () => (JsSyntax)Js.Str(() => "Enter")).Op(() => "&&", () => (JsSyntax)Js.Id(() => "e").Prop(() => "shiftKey").Op(() => "===", () => (JsSyntax)Js.Id(() => "false"))), new List<JsSyntax>
                    {
                        Js.Id(() => "e").Call(() => "preventDefault").Stmt(),
                        Js.Id(() => "sendMessage").Invoke().Stmt()
                    })
                }
            }));
        var initInputBody = Js.Block()
            .Add(() => Js.Const(() => "input", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "message-input"))))
            .Add(() => Js.Id(() => "input").Call(() => "addEventListener", () => Js.Str(() => "keydown"), () => Js.Arrow(() => new List<string> { "e" }, () => keydownHandlerBody)).Stmt())
            .Add(() => Js.Id(() => "autoResize").Invoke(() => Js.Id(() => "input")).Stmt());
        js.Add(() => Js.Func(() => "initInput", () => new List<string>(), () => initInputBody));

        var removeActiveArrow = Js.Arrow(() => new List<string> { "el" }, () => Js.Id(() => "el").Prop(() => "classList").Call(() => "remove", () => Js.Str(() => "active")));
        var clickHandlerBody = Js.Block()
            .Add(() => Js.Id(() => "document").Call(() => "querySelectorAll", () => Js.Str(() => ".being-item")).Call(() => "forEach", () => removeActiveArrow).Stmt())
            .Add(() => Js.Id(() => "item").Prop(() => "classList").Call(() => "add", () => Js.Str(() => "active")).Stmt())
            .Add(() => Js.Id(() => "selectSession").Invoke(() => Js.Id(() => "item").Prop(() => "dataset").Prop(() => "id"), () => Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => ".being-name")).Prop(() => "textContent")).Stmt());
        var forEachItemBody = Js.Block()
            .Add(() => Js.Id(() => "item").Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string>(), () => clickHandlerBody)).Stmt());
        var initSessionListBody = Js.Block()
            .Add(() => Js.Id(() => "document").Call(() => "querySelectorAll", () => Js.Str(() => ".being-item")).Call(() => "forEach", () => Js.Arrow(() => new List<string> { "item" }, () => forEachItemBody)).Stmt());
        js.Add(() => Js.Func(() => "initSessionList", () => new List<string>(), () => initSessionListBody));

        var dataErrorCond = Js.Id(() => "data").Prop(() => "error");
        var errorDivHtml = Js.Str(() => "<div style=\"padding:12px 20px;color:var(--text-secondary);font-size:12px;\">Error: ").Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "error")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>"));
        var errorBody = new List<JsSyntax>
        {
            Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "beings-list")).Prop(() => "innerHTML"), () => errorDivHtml),
            Js.Return(() => Js.Id(() => "undefined"))
        };
        var loadConversationsThenBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (dataErrorCond, errorBody) }
            }))
            .Add(() => Js.Const(() => "list", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "beings-list"))))
            .Add(() => Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.Id(() => "data").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "item" }, () => Js.Block()
                .Add(() => Js.Const(() => "div", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
                .Add(() => Js.Assign(() => Js.Id(() => "div").Prop(() => "className"), () => Js.Str(() => "being-item")))
                .Add(() => Js.Assign(() => Js.Id(() => "div").Prop(() => "dataset").Prop(() => "id"), () => Js.Id(() => "item").Prop(() => "sessionId")))
                .Add(() => Js.Assign(() => Js.Id(() => "div").Prop(() => "innerHTML"), () => Js.Str(() => "<div class=\"being-name\">").Op(() => "+", () => (JsSyntax)Js.Id(() => "item").Prop(() => "displayName")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div><div class=\"being-last-message\">")).Op(() => "+", () => (JsSyntax)Js.Id(() => "item").Prop(() => "lastMessage")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>"))))
                .Add(() => Js.Id(() => "list").Call(() => "appendChild", () => Js.Id(() => "div")).Stmt())
            )))
            .Add(() => Js.Id(() => "initSessionList").Invoke().Stmt());
        var loadConversationsCatchBody = Js.Block()
            .Add(() => Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "Load conversations failed:"), () => Js.Id(() => "err")).Stmt())
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "beings-list")).Prop(() => "innerHTML"), () => Js.Str(() => "<div style=\"padding:12px 20px;color:var(--text-secondary);font-size:12px;\">Failed to load conversations</div>")));
        var loadConversationsBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/chat/conversations")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => loadConversationsThenBody)).Call(() => "catch", () => Js.Arrow(() => new List<string> { "err" }, () => loadConversationsCatchBody)).Stmt());
        js.Add(() => Js.Func(() => "loadConversations", () => new List<string>(), () => loadConversationsBody));

        var msgIsUserCond = Js.Id(() => "msg").Prop(() => "isUser");
        var userMsgBody = new List<JsSyntax>
        {
            Js.Assign(() => Js.Id(() => "div").Prop(() => "className"), () => Js.Str(() => "msg-user")),
            Js.Assign(() => Js.Id(() => "div").Prop(() => "innerHTML"), () => Js.Str(() => "<div class=\"msg-user-bubble\">").Op(() => "+", () => (JsSyntax)Js.Id(() => "msg").Prop(() => "text")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>")))
        };
        var beingMsgBody = new List<JsSyntax>
        {
            Js.Assign(() => Js.Id(() => "div").Prop(() => "className"), () => Js.Str(() => "msg-being")),
            Js.Assign(() => Js.Id(() => "div").Prop(() => "id"), () => Js.Id(() => "msg").Prop(() => "elementId").Op(() => "||", () => (JsSyntax)Js.Str(() => ""))),
            Js.Assign(() => Js.Id(() => "div").Prop(() => "innerHTML"), () => Js.Str(() => "<div class=\"msg-being-card\"><div class=\"msg-being-body\"><div class=\"msg-being-text\">").Op(() => "+", () => (JsSyntax)Js.Id(() => "msg").Prop(() => "text")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div></div></div>")))
        };
        var appendMessageBody = Js.Block()
            .Add(() => Js.Const(() => "messages", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "chat-messages"))))
            .Add(() => Js.Const(() => "div", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (msgIsUserCond, userMsgBody) },
                { (null, beingMsgBody) }
            }))
            .Add(() => Js.Id(() => "messages").Call(() => "appendChild", () => Js.Id(() => "div")).Stmt())
            .Add(() => Js.Id(() => "messages").Call(() => "scrollTo", () => Js.Obj().Prop(() => "top", () => Js.Id(() => "messages").Prop(() => "scrollHeight")).Prop(() => "behavior", () => Js.Str(() => "smooth"))).Stmt());
        js.Add(() => Js.Func(() => "appendMessage", () => new List<string> { "msg" }, () => appendMessageBody));

        js.Add(() => Js.Id(() => "initInput").Invoke().Stmt());
        js.Add(() => Js.Id(() => "connectWebSocket").Invoke().Stmt());
        js.Add(() => Js.Id(() => "loadConversations").Invoke().Stmt());
        js.Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "chat-messages")).Call(() => "scrollTo", () => Js.Obj().Prop(() => "top", () => Js.Num(() => "999999")).Prop(() => "behavior", () => Js.Str(() => "auto"))).Stmt());

        return js;
    }
}
