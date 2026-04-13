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
        return RenderPage(vm.Skin, vm.Localization.PageTitleChat, vm.ActiveMenu, vm.Localization, body,
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
            msgItems.Add(RenderMessage(msg, vm.Localization));
        }

        return H.Div(
            H.Div(
                H.Div(vm.Localization.ChatConversationsHeader).Class("chat-conversations-header"),
                H.Div(sessionItems.ToArray()).Id("beings-list").Class("chat-conversations-list")
            ).Class("chat-conversations"),
            H.Div(
                H.Div(
                    H.Span(string.IsNullOrEmpty(vm.CurrentBeingName) ? vm.Localization.ChatNoConversationSelected : vm.CurrentBeingName)
                        .Class("chat-header-name")
                ).Id("chat-header").Class("chat-header"),
                H.Div(msgItems.ToArray()).Id("chat-messages").Class("chat-messages"),
                H.Div(
                    H.Textarea().Id("message-input").Placeholder(vm.Localization.ChatMessageInputPlaceholder),
                    H.Div(H.Button(vm.Localization.ChatSendButton).OnClick("sendMessage()")).Class("send-button")
                ).Class("chat-input-area")
            ).Class("chat-main"),
            H.Div(
                H.Div(
                    H.Div(
                        H.H1("权限请求").Class("permission-dialog-title"),
                        H.Div(
                            H.Div(
                                H.Span("权限类型:").Class("permission-label"),
                                H.Span("").Id("permission-type").Class("permission-value")
                            ).Class("permission-detail-row"),
                            H.Div(
                                H.Span("请求资源:").Class("permission-label"),
                                H.Span("").Id("permission-resource").Class("permission-value")
                            ).Class("permission-detail-row"),
                            H.Div(
                                H.Span("详细信息:").Class("permission-label"),
                                H.Div("").Id("permission-content").Class("permission-content-text")
                            ).Class("permission-detail-row")
                        ).Class("permission-details"),
                        H.Div(
                            H.Button("允许").Class("btn-permission-allow").OnClick("respondPermission(true)"),
                            H.Button("拒绝").Class("btn-permission-deny").OnClick("respondPermission(false)")
                        ).Class("permission-buttons")
                    ).Class("permission-dialog")
                ).Id("permission-overlay").Class("permission-overlay")
            ).Class("permission-overlay-wrapper")
        ).Class("chat-layout");
    }

    private static H RenderMessage(ChatMessage msg, DefaultLocalizationBase localization)
    {
        if (msg.IsUser)
        {
            var bubble = H.Div(
                msg.Text ?? ""
            ).Class("msg-user-bubble");
            var content = H.Div(bubble).Class("msg-user-content");
            if (!string.IsNullOrEmpty(msg.Time))
                content.Add(H.Div(msg.Time).Class("msg-time"));
            var avatar = H.Div(H.Div("U").Class("msg-avatar-icon"), H.Div(localization.ChatUserDisplayName).Class("msg-avatar-name")).Class("msg-user-avatar");
            return H.Div(content, avatar).Class("msg-user");
        }

        var beingDisplayName = !string.IsNullOrEmpty(msg.SenderName) ? msg.SenderName : localization.ChatDefaultBeingName;
        var avatar2 = H.Div(H.Div(beingDisplayName.Substring(0, 1)).Class("msg-avatar-icon"), H.Div(beingDisplayName).Class("msg-avatar-name")).Class("msg-being-avatar");

        var children = new List<object>();

        if (!string.IsNullOrEmpty(msg.SenderName))
            children.Add(H.Div(msg.SenderName).Class("msg-being-sender"));

        var bodyChildren = new List<object>();

        var thinkContent = msg.Thinking ?? "";
        bodyChildren.Add(H.Details(
            H.Summary(localization.ChatThinkingSummary),
            H.Div(thinkContent).Class("msg-thinking-content")
        ).Class("msg-collapsible"));

        if (!string.IsNullOrEmpty(msg.Text))
            bodyChildren.Add(H.Div(msg.Text).Class("msg-being-text"));

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
                H.Summary(localization.GetChatToolCallsSummary(tools.Count)),
                H.Div(toolItems.ToArray()).Class("msg-tools-list")
            ).Class("msg-collapsible"));
        }

        children.Add(H.Div(bodyChildren.ToArray()).Class("msg-being-body"));

        if (!string.IsNullOrEmpty(msg.Time))
            children.Add(H.Div(msg.Time).Class("msg-time"));

        var content2 = H.Div(
            H.Div(children.ToArray()).Class("msg-being-card")
        ).Class("msg-being-content");

        return H.Div(avatar2, content2).Class("msg-being");
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
                .Property("align-items", "flex-start")
                .Property("gap", "10px")
            .EndSelector()
            .Selector(".msg-user-avatar")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("align-items", "center")
                .Property("gap", "4px")
                .Property("flex-shrink", "0")
            .EndSelector()
            .Selector(".msg-being-avatar")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("align-items", "center")
                .Property("gap", "4px")
                .Property("flex-shrink", "0")
            .EndSelector()
            .Selector(".msg-avatar-icon")
                .Property("width", "36px")
                .Property("height", "36px")
                .Property("border-radius", "50%")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("justify-content", "center")
                .Property("font-weight", "600")
                .Property("flex-shrink", "0")
            .EndSelector()
            .Selector(".msg-user-avatar .msg-avatar-icon")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#fff")
                .Property("font-size", "14px")
            .EndSelector()
            .Selector(".msg-being-avatar .msg-avatar-icon")
                .Property("background", "linear-gradient(135deg, #a855f7, #6366f1)")
                .Property("color", "#fff")
                .Property("font-size", "11px")
            .EndSelector()
            .Selector(".msg-avatar-name")
                .Property("font-size", "11px")
                .Property("color", "var(--text-secondary)")
                .Property("white-space", "nowrap")
                .Property("max-width", "48px")
                .Property("overflow", "hidden")
                .Property("text-overflow", "ellipsis")
                .Property("text-align", "center")
            .EndSelector()
            .Selector(".msg-user-content")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("align-items", "flex-end")
            .EndSelector()
            .Selector(".msg-user-bubble")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#fff")
                .Property("padding", "10px 16px")
                .Property("border-radius", "16px 16px 4px 16px")
                .Property("font-size", "14px")
                .Property("line-height", "1.6")
                .Property("word-break", "break-word")
            .EndSelector()

            .Comment("Being Message")
            .Selector(".msg-being")
                .Property("display", "flex")
                .Property("justify-content", "flex-start")
                .Property("align-items", "flex-start")
                .Property("gap", "10px")
            .EndSelector()
            .Selector(".msg-being-content")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("align-items", "flex-start")
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

            .Comment("Tool Call Message")
            .Selector(".msg-tool")
                .Property("margin", "8px 0")
            .EndSelector()
            .Selector(".msg-tool .msg-collapsible")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.03))")
                .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".msg-tool-content")
                .Property("padding", "0 12px 10px")
                .Property("border-top", "1px solid var(--border)")
                .Property("padding-top", "8px")
            .EndSelector()
            .Selector(".msg-tool-section")
                .Property("margin-top", "8px")
            .EndSelector()
            .Selector(".msg-tool-section:first-child")
                .Property("margin-top", "0")
            .EndSelector()
            .Selector(".msg-tool-label")
                .Property("font-size", "12px")
                .Property("font-weight", "600")
                .Property("color", "var(--text-secondary)")
                .Property("margin-bottom", "4px")
            .EndSelector()
            .Selector(".msg-tool-code")
                .Property("margin", "0")
                .Property("padding", "8px 10px")
                .Property("background", "var(--bg-primary)")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "6px")
                .Property("font-size", "12px")
                .Property("font-family", "monospace")
                .Property("color", "var(--text-primary)")
                .Property("white-space", "pre-wrap")
                .Property("word-break", "break-word")
                .Property("overflow-x", "auto")
                .Property("max-height", "300px")
                .Property("overflow-y", "auto")
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
            .EndMedia()

            .Comment("Permission Overlay")
            .Selector(".permission-overlay-wrapper")
                .Property("position", "fixed")
                .Property("top", "0")
                .Property("left", "0")
                .Property("width", "100%")
                .Property("height", "100%")
                .Property("pointer-events", "none")
                .Property("z-index", "1000")
            .EndSelector()
            .Selector(".permission-overlay")
                .Property("display", "none")
                .Property("position", "fixed")
                .Property("top", "0")
                .Property("left", "0")
                .Property("width", "100%")
                .Property("height", "100%")
                .Property("background", "rgba(0,0,0,0.5)")
                .Property("align-items", "center")
                .Property("justify-content", "center")
                .Property("pointer-events", "auto")
                .Property("z-index", "1001")
            .EndSelector()
            .Selector(".permission-dialog")
                .Property("background", "var(--bg-card)")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "12px")
                .Property("padding", "24px")
                .Property("min-width", "360px")
                .Property("max-width", "500px")
                .Property("box-shadow", "0 8px 32px rgba(0,0,0,0.3)")
            .EndSelector()
            .Selector(".permission-dialog-title")
                .Property("font-size", "18px")
                .Property("font-weight", "600")
                .Property("color", "var(--text-primary)")
                .Property("margin-bottom", "16px")
            .EndSelector()
            .Selector(".permission-details")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.03))")
                .Property("border-radius", "8px")
                .Property("padding", "12px 16px")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".permission-detail-row")
                .Property("display", "flex")
                .Property("justify-content", "space-between")
                .Property("padding", "8px 0")
                .Property("border-bottom", "1px solid var(--border)")
            .EndSelector()
            .Selector(".permission-detail-row:last-child")
                .Property("border-bottom", "none")
                .Property("flex-direction", "column")
            .EndSelector()
            .Selector(".permission-label")
                .Property("font-size", "13px")
                .Property("font-weight", "600")
                .Property("color", "var(--text-secondary)")
                .Property("flex-shrink", "0")
                .Property("margin-right", "12px")
            .EndSelector()
            .Selector(".permission-value")
                .Property("font-size", "13px")
                .Property("color", "var(--text-primary)")
                .Property("word-break", "break-all")
            .EndSelector()
            .Selector(".permission-content-text")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
                .Property("white-space", "pre-wrap")
                .Property("word-break", "break-word")
                .Property("margin-top", "4px")
            .EndSelector()
            .Selector(".permission-buttons")
                .Property("display", "flex")
                .Property("gap", "12px")
                .Property("justify-content", "flex-end")
            .EndSelector()
            .Selector(".btn-permission-allow")
                .Property("padding", "8px 24px")
                .Property("background", "var(--accent-success, #4CAF50)")
                .Property("color", "#fff")
                .Property("border", "none")
                .Property("border-radius", "6px")
                .Property("cursor", "pointer")
                .Property("font-size", "14px")
                .Property("font-weight", "500")
            .EndSelector()
            .Selector(".btn-permission-allow:hover")
                .Property("opacity", "0.85")
            .EndSelector()
            .Selector(".btn-permission-deny")
                .Property("padding", "8px 24px")
                .Property("background", "var(--accent-error, #f44336)")
                .Property("color", "#fff")
                .Property("border", "none")
                .Property("border-radius", "6px")
                .Property("cursor", "pointer")
                .Property("font-size", "14px")
                .Property("font-weight", "500")
            .EndSelector()
            .Selector(".btn-permission-deny:hover")
                .Property("opacity", "0.85")
            .EndSelector();
    }

    private static JsSyntax GetScripts(ChatViewModel vm)
    {
        var currentSessionId = vm.CurrentBeingId?.ToString() ?? "";
        var userId = vm.UserId.ToString();
        var beingName = vm.CurrentBeingName ?? "AI";

        var js = Js.Block()
            .Add(() => Js.Let(() => "currentSessionId", () => currentSessionId.Length > 0 ? Js.Str(() => currentSessionId) : Js.Null()))
            .Add(() => Js.Let(() => "beingName", () => Js.Str(() => beingName)))
            .Add(() => Js.Let(() => "eventSource", () => Js.Null()))
            .Add(() => Js.Let(() => "currentStreamId", () => Js.Null()))
            .Add(() => Js.Let(() => "streamingMessage", () => Js.Null()))
            .Add(() => Js.Let(() => "messageCache", () => Js.New(() => Js.Id(() => "Array"))))
            .Add(() => Js.Let(() => "toolCallMap", () => Js.New(() => Js.Id(() => "Map"))));

        var autoResizeBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "textarea").Prop(() => "style").Prop(() => "height"), () => Js.Str(() => "auto")))
            .Add(() => Js.Assign(() => Js.Id(() => "textarea").Prop(() => "style").Prop(() => "height"), () => Js.Id(() => "Math").Call(() => "min", () => Js.Id(() => "textarea").Prop(() => "scrollHeight"), () => Js.Num(() => "120")).Op(() => "+", () => (JsSyntax)Js.Str(() => "px"))));
        js.Add(() => Js.Func(() => "autoResize", () => new List<string> { "textarea" }, () => autoResizeBody));

        var historyHandlerBody = Js.Block()
            .Add(() => Js.Id(() => "console").Call(() => "log", () => Js.Str(() => "history event received"), () => Js.Id(() => "event").Prop(() => "data")).Stmt())
            .Add(() => Js.Const(() => "data", () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "event").Prop(() => "data"))))
            .Add(() => Js.Id(() => "handleHistory").Invoke(() => Js.Id(() => "data")).Stmt());
        var messageHandlerBody = Js.Block()
            .Add(() => Js.Id(() => "console").Call(() => "log", () => Js.Str(() => "message event received"), () => Js.Id(() => "event").Prop(() => "data")).Stmt())
            .Add(() => Js.Const(() => "data", () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "event").Prop(() => "data"))))
            .Add(() => Js.Id(() => "handleMessage").Invoke(() => Js.Id(() => "data")).Stmt());
        var streamingHandlerBody = Js.Block()
            .Add(() => Js.Const(() => "data", () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "event").Prop(() => "data"))))
            .Add(() => Js.Id(() => "handleStreaming").Invoke(() => Js.Id(() => "data")).Stmt());

        var toolHandlerBody = Js.Block()
            .Add(() => Js.Const(() => "data", () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "event").Prop(() => "data"))))
            .Add(() => Js.Id(() => "handleTool").Invoke(() => Js.Id(() => "data")).Stmt());

        var permissionHandlerBody = Js.Block()
            .Add(() => Js.Const(() => "data", () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "event").Prop(() => "data"))))
            .Add(() => Js.Id(() => "handlePermission").Invoke(() => Js.Id(() => "data")).Stmt());

        var connectSSEBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "eventSource"), new List<JsSyntax>
                    {
                        Js.Id(() => "eventSource").Call(() => "close").Stmt()
                    }
                )}
            }))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "currentSessionId").Not(), new List<JsSyntax>
                    {
                        Js.Return(() => Js.Id(() => "undefined"))
                    }
                )}
            }))
            .Add(() => Js.Const(() => "url", () => Js.Str(() => "/api/chat/stream?channelId=").Op(() => "+", () => (JsSyntax)Js.Id(() => "currentSessionId"))))
            .Add(() => Js.Assign(() => Js.Id(() => "eventSource"), () => Js.New(() => Js.Id(() => "EventSource"), () => Js.Id(() => "url"))))
            .Add(() => Js.Id(() => "eventSource").Prop(() => "onopen").Assign(() => Js.Arrow(() => new List<string>(), () => Js.Block()
                .Add(() => Js.Id(() => "console").Call(() => "log", () => Js.Str(() => "SSE connected")).Stmt())
            )))
            .Add(() => Js.Id(() => "eventSource").Prop(() => "onerror").Assign(() => Js.Arrow(() => new List<string> { "err" }, () => Js.Block()
                .Add(() => Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "SSE error:"), () => Js.Id(() => "err")).Stmt())
                .Add(() => Js.Id(() => "eventSource").Call(() => "close").Stmt())
                .Add(() => Js.Id(() => "setTimeout").Invoke(() => Js.Id(() => "connectSSE"), () => Js.Num(() => "3000")).Stmt())
            )))
            .Add(() => Js.Id(() => "eventSource").Call(() => "addEventListener", () => Js.Str(() => "history"), () => Js.Arrow(() => new List<string> { "event" }, () => historyHandlerBody)).Stmt())
            .Add(() => Js.Id(() => "eventSource").Call(() => "addEventListener", () => Js.Str(() => "message"), () => Js.Arrow(() => new List<string> { "event" }, () => messageHandlerBody)).Stmt())
            .Add(() => Js.Id(() => "eventSource").Call(() => "addEventListener", () => Js.Str(() => "streaming"), () => Js.Arrow(() => new List<string> { "event" }, () => streamingHandlerBody)).Stmt())
            .Add(() => Js.Id(() => "eventSource").Call(() => "addEventListener", () => Js.Str(() => "tool"), () => Js.Arrow(() => new List<string> { "event" }, () => toolHandlerBody)).Stmt())
            .Add(() => Js.Id(() => "eventSource").Call(() => "addEventListener", () => Js.Str(() => "permission"), () => Js.Arrow(() => new List<string> { "event" }, () => permissionHandlerBody)).Stmt());
        js.Add(() => Js.Func(() => "connectSSE", () => new List<string>(), () => connectSSEBody));

        var handleHistoryBody = Js.Block()
            .Add(() => Js.Id(() => "console").Call(() => "log", () => Js.Str(() => "handleHistory called with"), () => Js.Id(() => "data")).Stmt())
            .Add(() => Js.Const(() => "messages", () => Js.Id(() => "data").Prop(() => "messages")))
            .Add(() => Js.Id(() => "console").Call(() => "log", () => Js.Str(() => "messages count"), () => Js.Id(() => "messages").Prop(() => "length")).Stmt())
            .Add(() => Js.Assign(() => Js.Id(() => "messageCache"), () => Js.New(() => Js.Id(() => "Array"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "messages"), new List<JsSyntax>
                    {
                        Js.Id(() => "messages").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "m" }, () => Js.Id(() => "messageCache").Call(() => "push", () => Js.Id(() => "m")))).Stmt()
                    }
                )}
            }))
            .Add(() => Js.Id(() => "renderMessages").Invoke().Stmt());
        js.Add(() => Js.Func(() => "handleHistory", () => new List<string> { "data" }, () => handleHistoryBody));

        var handleMessageBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "data").Prop(() => "channelId").Op(() => "!==", () => Js.Id(() => "currentSessionId")), new List<JsSyntax> { Js.Return(() => Js.Id(() => "undefined")) }) }
            }))
            .Add(() => Js.Const(() => "isCurrentUser", () => Js.Id(() => "data").Prop(() => "senderId").Op(() => "===", () => (JsSyntax)Js.Str(() => userId))))
            .Add(() => Js.Id(() => "appendMessage").Invoke(() => Js.Obj()
                .Prop(() => "isUser", () => Js.Id(() => "isCurrentUser"))
                .Prop(() => "text", () => Js.Id(() => "data").Prop(() => "content"))
                .Prop(() => "thinking", () => Js.Id(() => "data").Prop(() => "thinking"))
                .Prop(() => "senderName", () => Js.Id(() => "data").Prop(() => "senderName"))).Stmt());
        js.Add(() => Js.Func(() => "handleMessage", () => new List<string> { "data" }, () => handleMessageBody));

        var handleToolBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "data").Prop(() => "channelId").Op(() => "!==", () => Js.Id(() => "currentSessionId")), new List<JsSyntax> { Js.Return(() => Js.Id(() => "undefined")) }) }
            }))
            .Add(() => Js.Id(() => "messageCache").Call(() => "push", () => Js.Obj()
                .Prop(() => "role", () => Js.Id(() => "data").Prop(() => "role"))
                .Prop(() => "content", () => Js.Id(() => "data").Prop(() => "content"))
                .Prop(() => "thinking", () => Js.Id(() => "data").Prop(() => "thinking"))
                .Prop(() => "senderName", () => Js.Id(() => "data").Prop(() => "senderName"))
                .Prop(() => "toolCallsJson", () => Js.Id(() => "data").Prop(() => "toolCallsJson"))
                .Prop(() => "toolCallId", () => Js.Id(() => "data").Prop(() => "toolCallId"))).Stmt())
            .Add(() => Js.Id(() => "buildToolCallMap").Invoke().Stmt())
            .Add(() => Js.Id(() => "appendMessage").Invoke(() => Js.Obj()
                .Prop(() => "isUser", () => Js.Bool(() => false))
                .Prop(() => "text", () => Js.Id(() => "data").Prop(() => "content"))
                .Prop(() => "thinking", () => Js.Id(() => "data").Prop(() => "thinking"))
                .Prop(() => "senderName", () => Js.Id(() => "data").Prop(() => "senderName"))
                .Prop(() => "role", () => Js.Id(() => "data").Prop(() => "role"))
                .Prop(() => "toolCallsJson", () => Js.Id(() => "data").Prop(() => "toolCallsJson"))
                .Prop(() => "toolCallId", () => Js.Id(() => "data").Prop(() => "toolCallId"))).Stmt());
        js.Add(() => Js.Func(() => "handleTool", () => new List<string> { "data" }, () => handleToolBody));

        var handlePermissionBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "data").Prop(() => "type").Op(() => "!==", () => Js.Str(() => "permission_ask")), new List<JsSyntax> { Js.Return(() => Js.Id(() => "undefined")) }) }
            }))
            .Add(() => Js.Id(() => "showPermissionDialog").Invoke(() => Js.Id(() => "data")).Stmt());
        js.Add(() => Js.Func(() => "handlePermission", () => new List<string> { "data" }, () => handlePermissionBody));

        var showPermissionDialogBody = Js.Block()
            .Add(() => Js.Const(() => "overlay", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "permission-overlay"))))
            .Add(() => Js.Const(() => "typeEl", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "permission-type"))))
            .Add(() => Js.Const(() => "resourceEl", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "permission-resource"))))
            .Add(() => Js.Const(() => "contentEl", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "permission-content"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "overlay").Not(), new List<JsSyntax> { Js.Return(() => Js.Id(() => "undefined")) }) }
            }))
            .Add(() => Js.Assign(() => Js.Id(() => "typeEl").Prop(() => "textContent"), () => Js.Id(() => "data").Prop(() => "permissionType")))
            .Add(() => Js.Assign(() => Js.Id(() => "resourceEl").Prop(() => "textContent"), () => Js.Id(() => "data").Prop(() => "resource")))
            .Add(() => Js.Assign(() => Js.Id(() => "contentEl").Prop(() => "textContent"), () => Js.Id(() => "data").Prop(() => "content")))
            .Add(() => Js.Assign(() => Js.Id(() => "overlay").Prop(() => "style").Prop(() => "display"), () => Js.Str(() => "flex")));
        js.Add(() => Js.Func(() => "showPermissionDialog", () => new List<string> { "data" }, () => showPermissionDialogBody));

        var respondPermissionBody = Js.Block()
            .Add(() => Js.Const(() => "overlay", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "permission-overlay"))))
            .Add(() => Js.Assign(() => Js.Id(() => "overlay").Prop(() => "style").Prop(() => "display"), () => Js.Str(() => "none")))
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/permission/respond").Op(() => "+", () => (JsSyntax)Js.Str(() => "?userId=")).Op(() => "+", () => (JsSyntax)Js.Str(() => userId)).Op(() => "+", () => (JsSyntax)Js.Str(() => "&allowed=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "allowed")), () => Js.Obj()
                .Prop(() => "method", () => Js.Str(() => "GET"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => Js.Block()
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    { (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
                        {
                            Js.Id(() => "appendMessage").Invoke(() => Js.Obj()
                                .Prop(() => "isUser", () => Js.Bool(() => true))
                                .Prop(() => "text", () => Js.Ternary(() => Js.Id(() => "allowed"), () => Js.Str(() => "[Permission Allowed]"), () => Js.Str(() => "[Permission Denied]")))).Stmt()
                        }
                    )}
                }))
            )).Call(() => "catch", () => Js.Arrow(() => new List<string> { "err" }, () => Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "Permission respond error:"), () => Js.Id(() => "err")))).Stmt());
        js.Add(() => Js.Func(() => "respondPermission", () => new List<string> { "allowed" }, () => respondPermissionBody));

        var handleStreamingBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "data").Prop(() => "channelId").Op(() => "!==", () => Js.Id(() => "currentSessionId")), new List<JsSyntax> { Js.Return(() => Js.Id(() => "undefined")) }) }
            }))
            .Add(() => Js.Const(() => "streamContent", () => Js.Id(() => "data").Prop(() => "content")))
            .Add(() => Js.Const(() => "streamThinking", () => Js.Id(() => "data").Prop(() => "thinking")))
            .Add(() => Js.Const(() => "streamId", () => Js.Id(() => "data").Prop(() => "streamId")))
            .Add(() => Js.Const(() => "isFinal", () => Js.Id(() => "data").Prop(() => "isFinal")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "streamContent").Not().Op(() => "&&", () => (JsSyntax)Js.Id(() => "streamThinking").Not()).Op(() => "&&", () => (JsSyntax)Js.Id(() => "isFinal")), new List<JsSyntax>
                    {
                        Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            { (Js.Id(() => "currentStreamId"), new List<JsSyntax>
                                {
                                    Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                                    {
                                        { (Js.Id(() => "streamingMessage").Prop(() => "elementId"), new List<JsSyntax>
                                            {
                                                Js.Const(() => "removeEl", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Id(() => "streamingMessage").Prop(() => "elementId"))),
                                                Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                                                {
                                                    { (Js.Id(() => "removeEl"), new List<JsSyntax>
                                                        {
                                                            Js.Id(() => "removeEl").Call(() => "remove").Stmt()
                                                        }
                                                    )}
                                                })
                                            }
                                        )}
                                    }),
                                    Js.Assign(() => Js.Id(() => "currentStreamId"), () => Js.Null()),
                                    Js.Assign(() => Js.Id(() => "streamingMessage"), () => Js.Null())
                                }
                            )}
                        }),
                        Js.Return(() => Js.Id(() => "undefined"))
                    }
                )}
            }))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "currentStreamId").Not().Op(() => "||", () => (JsSyntax)Js.Id(() => "currentStreamId").Op(() => "!==", () => Js.Id(() => "streamId"))), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "currentStreamId"), () => Js.Id(() => "streamId")),
                        Js.Assign(() => Js.Id(() => "streamingMessage"), () => Js.Obj()
                            .Prop(() => "isUser", () => Js.Bool(() => false))
                            .Prop(() => "text", () => Js.Str(() => ""))
                            .Prop(() => "thinking", () => Js.Str(() => ""))
                            .Prop(() => "elementId", () => Js.Str(() => "stream-").Op(() => "+", () => (JsSyntax)Js.Id(() => "streamId")))),
                        Js.Id(() => "appendMessage").Invoke(() => Js.Id(() => "streamingMessage")).Stmt()
                    }
                )}
            }))
            .Add(() => Js.Const(() => "streamEl", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Id(() => "streamingMessage").Prop(() => "elementId"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "streamEl"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "streamingMessage").Prop(() => "text"), () => Js.Id(() => "streamContent")),
                        Js.Assign(() => Js.Id(() => "streamEl").Call(() => "querySelector", () => Js.Str(() => ".msg-being-text")).Prop(() => "textContent"), () => Js.Id(() => "streamContent")),
                        Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            { (Js.Id(() => "streamThinking"), new List<JsSyntax>
                                {
                                    Js.Assign(() => Js.Id(() => "streamingMessage").Prop(() => "thinking"), () => Js.Id(() => "streamThinking")),
                                    Js.Assign(() => Js.Id(() => "streamEl").Call(() => "querySelector", () => Js.Str(() => ".msg-thinking-content")).Prop(() => "textContent"), () => Js.Id(() => "streamThinking"))
                                }
                            )}
                        })
                    }
                )}
            }));
        js.Add(() => Js.Func(() => "handleStreaming", () => new List<string> { "data" }, () => handleStreamingBody));

        var buildToolCallMapBody = Js.Block()
            .Add(() => Js.Id(() => "toolCallMap").Call(() => "clear").Stmt())
            .Add(() => Js.Id(() => "messageCache").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "m" }, () => Js.Block()
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    { (Js.Id(() => "m").Prop(() => "toolCallsJson"), new List<JsSyntax>
                        {
                            Js.Const(() => "toolCalls", () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "m").Prop(() => "toolCallsJson"))),
                            Js.Id(() => "toolCalls").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "tc" }, () => Js.Block()
                                .Add(() => Js.Id(() => "toolCallMap").Call(() => "set", () => Js.Id(() => "tc").Prop(() => "Id"), () => Js.Id(() => "tc"))).Stmt()
                            )).Stmt()
                        }
                    )}
                }))
            )));
        js.Add(() => Js.Func(() => "buildToolCallMap", () => new List<string>(), () => buildToolCallMapBody));

        var renderMessagesBody = Js.Block()
            .Add(() => Js.Id(() => "console").Call(() => "log", () => Js.Str(() => "renderMessages called, cache length:"), () => Js.Id(() => "messageCache").Prop(() => "length")).Stmt())
            .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "chat-messages"))))
            .Add(() => Js.Assign(() => Js.Id(() => "container").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.Id(() => "buildToolCallMap").Invoke().Stmt())
            .Add(() => Js.Id(() => "messageCache").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "m" }, () => Js.Block()
                .Add(() => Js.Id(() => "appendMessage").Invoke(() => Js.Obj()
                    .Prop(() => "isUser", () => Js.Id(() => "m").Prop(() => "role").Op(() => "===", () => Js.Str(() => "User")))
                    .Prop(() => "text", () => Js.Id(() => "m").Prop(() => "content"))
                    .Prop(() => "thinking", () => Js.Id(() => "m").Prop(() => "thinking"))
                    .Prop(() => "senderName", () => Js.Id(() => "m").Prop(() => "senderName"))
                    .Prop(() => "role", () => Js.Id(() => "m").Prop(() => "role"))
                    .Prop(() => "toolCallsJson", () => Js.Id(() => "m").Prop(() => "toolCallsJson"))
                    .Prop(() => "toolCallId", () => Js.Id(() => "m").Prop(() => "toolCallId"))).Stmt())
            )))
            .Add(() => Js.Id(() => "container").Call(() => "scrollTo", () => Js.Obj().Prop(() => "top", () => Js.Id(() => "container").Prop(() => "scrollHeight")).Prop(() => "behavior", () => Js.Str(() => "smooth"))).Stmt());
        js.Add(() => Js.Func(() => "renderMessages", () => new List<string>(), () => renderMessagesBody));

        var textEmptyCond = Js.Id(() => "text").Not().Op(() => "||", () => (JsSyntax)Js.Id(() => "currentSessionId").Not());
        var sendMessageBody = Js.Block()
            .Add(() => Js.Const(() => "input", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "message-input"))))
            .Add(() => Js.Const(() => "text", () => Js.Id(() => "input").Prop(() => "value").Call(() => "trim")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (textEmptyCond, new List<JsSyntax> { Js.Return(() => Js.Id(() => "undefined")) }) }
            }))
            .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "value"), () => Js.Str(() => "")))
            .Add(() => Js.Id(() => "autoResize").Invoke(() => Js.Id(() => "input")).Stmt())
            .Add(() => Js.Id(() => "appendMessage").Invoke(() => Js.Obj()
                .Prop(() => "isUser", () => Js.Bool(() => true))
                .Prop(() => "text", () => Js.Id(() => "text"))).Stmt())
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/chat/send"), () => Js.Obj()
                .Prop(() => "method", () => Js.Str(() => "POST"))
                .Prop(() => "headers", () => Js.Obj().Prop(() => "Content-Type", () => Js.Str(() => "application/json")))
                .Prop(() => "body", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Obj()
                    .Prop(() => "channelId", () => Js.Id(() => "currentSessionId"))
                    .Prop(() => "content", () => Js.Id(() => "text"))))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => Js.Block()
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    { (Js.Id(() => "result").Prop(() => "success").Not(), new List<JsSyntax>
                        {
                            Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "Send failed:"), () => Js.Id(() => "result").Prop(() => "error")).Stmt()
                        }
                    )}
                }))
            )).Call(() => "catch", () => Js.Arrow(() => new List<string> { "err" }, () => Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "Send error:"), () => Js.Id(() => "err")))).Stmt());
        js.Add(() => Js.Func(() => "sendMessage", () => new List<string>(), () => sendMessageBody));

        var selectSessionBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "currentSessionId"), () => Js.Id(() => "sessionId")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "querySelector", () => Js.Str(() => ".chat-header-name")).Prop(() => "textContent"), () => Js.Id(() => "name")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "chat-messages")).Call(() => "setAttribute", () => Js.Str(() => "data-session-id"), () => Js.Id(() => "sessionId")).Stmt())
            .Add(() => Js.Assign(() => Js.Id(() => "messageCache"), () => Js.New(() => Js.Id(() => "Array"))))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "chat-messages")).Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.Id(() => "connectSSE").Invoke().Stmt());
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
        var msgIsToolCond = Js.Id(() => "msg").Prop(() => "role").Op(() => "===", () => Js.Str(() => "Tool"));
        var toolCallRequestExpr = Js.Ternary(() => Js.Id(() => "toolCallMap").Call(() => "has", () => Js.Id(() => "msg").Prop(() => "toolCallId")), () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Id(() => "toolCallMap").Call(() => "get", () => Js.Id(() => "msg").Prop(() => "toolCallId")), () => Js.Null(), () => Js.Num(() => "2")), () => Js.Str(() => "{}"));
        var userMsgBody = new List<JsSyntax>
        {
            Js.Assign(() => Js.Id(() => "div").Prop(() => "className"), () => Js.Str(() => "msg-user")),
            Js.Assign(() => Js.Id(() => "div").Prop(() => "innerHTML"), () => Js.Str(() => "<div class=\"msg-user-content\"><div class=\"msg-user-bubble\">").Op(() => "+", () => (JsSyntax)Js.Id(() => "msg").Prop(() => "text")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div></div><div class=\"msg-user-avatar\"><div class=\"msg-avatar-icon\">U</div><div class=\"msg-avatar-name\">我</div></div>")))
        };
        var toolMsgBody = new List<JsSyntax>
        {
            Js.Assign(() => Js.Id(() => "div").Prop(() => "className"), () => Js.Str(() => "msg-tool")),
            Js.Assign(() => Js.Id(() => "div").Prop(() => "innerHTML"), () => Js.Str(() => "<details class=\"msg-collapsible\"><summary>🔧 Tool Call</summary><div class=\"msg-tool-content\"><div class=\"msg-tool-section\"><div class=\"msg-tool-label\">Request:</div><pre class=\"msg-tool-code\">").Op(() => "+", () => (JsSyntax)toolCallRequestExpr).Op(() => "+", () => (JsSyntax)Js.Str(() => "</pre></div><div class=\"msg-tool-section\"><div class=\"msg-tool-label\">Response:</div><pre class=\"msg-tool-code\">")).Op(() => "+", () => (JsSyntax)Js.Id(() => "msg").Prop(() => "text")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</pre></div></div></details>"))),
        };
        var beingMsgBody = new List<JsSyntax>
        {
            Js.Assign(() => Js.Id(() => "div").Prop(() => "className"), () => Js.Str(() => "msg-being")),
            Js.Assign(() => Js.Id(() => "div").Prop(() => "id"), () => Js.Id(() => "msg").Prop(() => "elementId").Op(() => "||", () => (JsSyntax)Js.Str(() => ""))),
            Js.Assign(() => Js.Id(() => "div").Prop(() => "innerHTML"), () => Js.Str(() => "<div class=\"msg-being-avatar\"><div class=\"msg-avatar-icon\">").Op(() => "+", () => (JsSyntax)Js.Id(() => "msg").Prop(() => "senderName").Op(() => "||", () => (JsSyntax)Js.Id(() => "beingName")).Paren().Call(() => "charAt", () => Js.Num(() => "0"))).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div><div class=\"msg-avatar-name\">")).Op(() => "+", () => (JsSyntax)Js.Id(() => "msg").Prop(() => "senderName").Op(() => "||", () => (JsSyntax)Js.Id(() => "beingName")).Paren()).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div></div><div class=\"msg-being-content\"><div class=\"msg-being-card\"><div class=\"msg-being-body\"><details class=\"msg-collapsible\"><summary>💭 Think</summary><div class=\"msg-thinking-content\">")).Op(() => "+", () => (JsSyntax)Js.Id(() => "msg").Prop(() => "thinking").Op(() => "||", () => (JsSyntax)Js.Str(() => "")).Paren()).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div></details><div class=\"msg-being-text\">")).Op(() => "+", () => (JsSyntax)Js.Id(() => "msg").Prop(() => "text")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div></div></div></div>")))
        };
        var appendMessageBody = Js.Block()
            .Add(() => Js.Const(() => "messages", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "chat-messages"))))
            .Add(() => Js.Const(() => "div", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (msgIsUserCond, userMsgBody) },
                { (msgIsToolCond, toolMsgBody) },
                { (null, beingMsgBody) }
            }))
            .Add(() => Js.Id(() => "messages").Call(() => "appendChild", () => Js.Id(() => "div")).Stmt())
            .Add(() => Js.Id(() => "messages").Call(() => "scrollTo", () => Js.Obj().Prop(() => "top", () => Js.Id(() => "messages").Prop(() => "scrollHeight")).Prop(() => "behavior", () => Js.Str(() => "smooth"))).Stmt());
        js.Add(() => Js.Func(() => "appendMessage", () => new List<string> { "msg" }, () => appendMessageBody));

        js.Add(() => Js.Id(() => "initInput").Invoke().Stmt());
        js.Add(() => Js.Id(() => "initSessionList").Invoke().Stmt());
        js.Add(() => Js.Id(() => "connectSSE").Invoke().Stmt());
        js.Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "chat-messages")).Call(() => "scrollTo", () => Js.Obj().Prop(() => "top", () => Js.Num(() => "999999")).Prop(() => "behavior", () => Js.Str(() => "auto"))).Stmt());

        return js;
    }
}