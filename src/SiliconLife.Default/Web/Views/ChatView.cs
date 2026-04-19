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
                        H.H1(vm.Localization.PermissionDialogTitle).Class("permission-dialog-title"),
                        H.Div(
                            H.Div(
                                H.Span(vm.Localization.PermissionTypeLabel).Class("permission-label"),
                                H.Span("").Id("permission-type").Class("permission-value")
                            ).Class("permission-detail-row"),
                            H.Div(
                                H.Span(vm.Localization.PermissionResourceLabel).Class("permission-label"),
                                H.Span("").Id("permission-resource").Class("permission-value")
                            ).Class("permission-detail-row"),
                            H.Div(
                                H.Span(vm.Localization.PermissionDetailLabel).Class("permission-label"),
                                H.Div("").Id("permission-content").Class("permission-content-text")
                            ).Class("permission-detail-row")
                        ).Class("permission-details"),
                        H.Div(
                            H.Button(vm.Localization.PermissionAllowButton).Class("btn-permission-allow").OnClick("respondPermission(true)"),
                            H.Button(vm.Localization.PermissionDenyButton).Class("btn-permission-deny").OnClick("respondPermission(false)")
                        ).Class("permission-buttons"),
                        H.Div(
                            H.Input().Attr("type", "checkbox").Id("permission-cache-checkbox").Class("permission-cache-checkbox"),
                            H.Label(vm.Localization.PermissionCacheLabel).Attr("for", "permission-cache-checkbox").Class("permission-cache-label")
                        ).Class("permission-cache-row"),
                        H.Div(
                            H.Label(vm.Localization.PermissionCacheDurationLabel).Attr("for", "permission-cache-duration").Class("permission-duration-label"),
                            H.Select(
                                H.Option(vm.Localization.PermissionCacheDuration1Hour).Attr("value", "1"),
                                H.Option(vm.Localization.PermissionCacheDuration24Hours).Attr("value", "24"),
                                H.Option(vm.Localization.PermissionCacheDuration7Days).Attr("value", "168"),
                                H.Option(vm.Localization.PermissionCacheDuration30Days).Attr("value", "720")
                            ).Id("permission-cache-duration").Class("permission-duration-select")
                        ).Id("permission-duration-row").Class("permission-duration-row")
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
            bodyChildren.Add(H.Div().Class("msg-being-text markdown-body").Data("md-raw", msg.Text));

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

        {
            var prompt = msg.PromptTokens ?? 0;
            var completion = msg.CompletionTokens ?? 0;
            var total = msg.TotalTokens ?? 0;
            children.Add(H.Div($"Token: ↑{prompt} ↓{completion} Σ{total}").Class("msg-token-stats"));
        }

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
                .Property("word-break", "break-word")
            .EndSelector()

            .Comment("Markdown body styles for chat messages")
            .Selector(".markdown-body h1")
                .Property("font-size", "1.6em")
                .Property("margin", "0.5em 0")
                .Property("border-bottom", "1px solid var(--border)")
                .Property("padding-bottom", "0.3em")
            .EndSelector()
            .Selector(".markdown-body h2")
                .Property("font-size", "1.4em")
                .Property("margin", "0.5em 0")
                .Property("border-bottom", "1px solid var(--border)")
                .Property("padding-bottom", "0.3em")
            .EndSelector()
            .Selector(".markdown-body h3")
                .Property("font-size", "1.2em")
                .Property("margin", "0.5em 0")
            .EndSelector()
            .Selector(".markdown-body h4, .markdown-body h5, .markdown-body h6")
                .Property("margin", "0.5em 0")
            .EndSelector()
            .Selector(".markdown-body p")
                .Property("margin", "0.6em 0")
            .EndSelector()
            .Selector(".markdown-body code")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.1))")
                .Property("padding", "2px 6px")
                .Property("border-radius", "3px")
                .Property("font-size", "0.9em")
                .Property("font-family", "'JetBrains Mono', 'Fira Code', 'Consolas', monospace")
            .EndSelector()
            .Selector(".markdown-body pre")
                .Property("background", "var(--bg-secondary, rgba(0,0,0,0.3))")
                .Property("padding", "12px")
                .Property("border-radius", "6px")
                .Property("overflow-x", "auto")
                .Property("margin", "0.8em 0")
            .EndSelector()
            .Selector(".markdown-body pre code")
                .Property("background", "none")
                .Property("padding", "0")
            .EndSelector()
            .Selector(".markdown-body blockquote")
                .Property("border-left", "4px solid var(--accent-primary)")
                .Property("margin", "0.8em 0")
                .Property("padding", "0.5em 1em")
                .Property("color", "var(--text-secondary)")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.03))")
                .Property("border-radius", "0 4px 4px 0")
            .EndSelector()
            .Selector(".markdown-body ul, .markdown-body ol")
                .Property("padding-left", "2em")
                .Property("margin", "0.4em 0")
            .EndSelector()
            .Selector(".markdown-body table")
                .Property("border-collapse", "collapse")
                .Property("width", "100%")
                .Property("margin", "0.8em 0")
            .EndSelector()
            .Selector(".markdown-body th, .markdown-body td")
                .Property("border", "1px solid var(--border)")
                .Property("padding", "6px 10px")
                .Property("text-align", "left")
            .EndSelector()
            .Selector(".markdown-body th")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.05))")
                .Property("font-weight", "600")
            .EndSelector()
            .Selector(".markdown-body img")
                .Property("max-width", "100%")
                .Property("border-radius", "6px")
            .EndSelector()
            .Selector(".markdown-body a")
                .Property("color", "var(--accent-primary)")
                .Property("text-decoration", "none")
            .EndSelector()
            .Selector(".markdown-body a:hover")
                .Property("text-decoration", "underline")
            .EndSelector()
            .Selector(".markdown-body hr")
                .Property("border", "none")
                .Property("border-top", "1px solid var(--border)")
                .Property("margin", "1em 0")
            .EndSelector()

            .Comment("Collapsible sections")
            .Selector(".msg-collapsible")
                .Property("margin-top", "8px")
                .Property("border", "1px solid var(--border)")
                .Property("border-left", "3px solid var(--accent-primary)")
                .Property("border-radius", "8px")
                .Property("font-size", "13px")
            .EndSelector()
            .Selector(".msg-collapsible summary")
                .Property("padding", "8px 12px")
                .Property("cursor", "pointer")
                .Property("color", "var(--text-secondary)")
                .Property("font-weight", "500")
                .Property("user-select", "none")
                .Property("display", "flex")
                .Property("justify-content", "space-between")
                .Property("align-items", "center")
                .Property("list-style", "none")
            .EndSelector()
            .Selector(".msg-collapsible summary::-webkit-details-marker")
                .Property("display", "none")
            .EndSelector()
            .Selector(".msg-collapsible summary::marker")
                .Property("display", "none")
            .EndSelector()
            .Selector(".msg-collapsible summary::after")
                .Property("content", "\"▼\"")
                .Property("font-size", "10px")
                .Property("transition", "transform 0.2s")
                .Property("flex-shrink", "0")
                .Property("margin-left", "8px")
            .EndSelector()
            .Selector(".msg-collapsible[open] summary::after")
                .Property("transform", "rotate(180deg)")
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
                .Property("border-left", "3px solid var(--accent-warning, #f59e0b)")
                .Property("border-radius", "8px")
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

            .Comment("Token Stats")
            .Selector(".msg-token-stats")
                .Property("font-size", "11px")
                .Property("color", "var(--text-secondary)")
                .Property("opacity", "0.6")
                .Property("margin-top", "6px")
                .Property("margin-right", "4px")
                .Property("margin-bottom", "2px")
                .Property("text-align", "right")
                .Property("font-family", "'JetBrains Mono', 'Fira Code', 'Consolas', monospace")
                .Property("letter-spacing", "0.02em")
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
                .Property("background", "rgba(0,0,0,0.55)")
                .Property("align-items", "center")
                .Property("justify-content", "center")
                .Property("pointer-events", "auto")
                .Property("z-index", "1001")
            .EndSelector()
            .Selector(".permission-dialog")
                // bg-card → bg-secondary → bg-white fallback，确保任何皮肤下弹窗都不透明
                .Property("background", "var(--bg-card, var(--bg-secondary, var(--bg-white, #ffffff)))")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "12px")
                .Property("padding", "24px")
                .Property("min-width", "360px")
                .Property("max-width", "500px")
                .Property("box-shadow", "0 8px 32px rgba(0,0,0,0.35)")
            .EndSelector()
            .Selector(".permission-dialog-title")
                .Property("font-size", "18px")
                .Property("font-weight", "600")
                .Property("color", "var(--text-primary)")
                .Property("margin-bottom", "16px")
            .EndSelector()
            .Selector(".permission-details")
                // bg-tertiary → bg-secondary fallback，浅色皮肤下给一个可见的区分背景
                .Property("background", "var(--bg-tertiary, var(--bg-secondary, rgba(0,0,0,0.04)))")
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
                // accent-success 在所有皮肤中均有定义，fallback 到通用绿色
                .Property("background", "var(--accent-success, #4CAF50)")
                .Property("color", "#fff")
                .Property("border", "none")
                .Property("border-radius", "6px")
                .Property("cursor", "pointer")
                .Property("font-size", "14px")
                .Property("font-weight", "500")
                .Property("transition", "opacity 0.2s")
            .EndSelector()
            .Selector(".btn-permission-allow:hover")
                .Property("opacity", "0.85")
            .EndSelector()
            .Selector(".btn-permission-deny")
                .Property("padding", "8px 24px")
                // accent-error → accent-danger fallback（AdminSkin 用 accent-danger 而非 accent-error）
                .Property("background", "var(--accent-error, var(--accent-danger, #f44336))")
                .Property("color", "#fff")
                .Property("border", "none")
                .Property("border-radius", "6px")
                .Property("cursor", "pointer")
                .Property("font-size", "14px")
                .Property("font-weight", "500")
                .Property("transition", "opacity 0.2s")
            .EndSelector()
            .Selector(".btn-permission-deny:hover")
                .Property("opacity", "0.85")
            .EndSelector()
            .Selector(".permission-cache-row")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "6px")
                .Property("margin-top", "12px")
                .Property("justify-content", "flex-end")
            .EndSelector()
            .Selector(".permission-cache-checkbox")
                .Property("cursor", "pointer")
                .Property("accent-color", "var(--accent-success, #4CAF50)")
                .Property("width", "16px")
                .Property("height", "16px")
            .EndSelector()
            .Selector(".permission-cache-label")
                .Property("font-size", "13px")
                .Property("color", "var(--text-secondary)")
                .Property("cursor", "pointer")
                .Property("user-select", "none")
            .EndSelector()
            .Selector(".permission-duration-row")
                .Property("display", "none")
                .Property("align-items", "center")
                .Property("gap", "8px")
                .Property("margin-top", "8px")
                .Property("justify-content", "flex-end")
            .EndSelector()
            .Selector(".permission-duration-label")
                .Property("font-size", "13px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".permission-duration-select")
                .Property("font-size", "13px")
                .Property("padding", "4px 8px")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "4px")
                .Property("background", "var(--bg-card, var(--bg-secondary, #fff))")
                .Property("color", "var(--text-primary)")
                .Property("cursor", "pointer")
            .EndSelector();
    }

    private static JsSyntax GetScripts(ChatViewModel vm)
    {
        var currentSessionId = vm.CurrentBeingId?.ToString() ?? "";
        var userId = vm.UserId.ToString();
        var beingName = vm.CurrentBeingName ?? "AI";

        // Serialize the tool display name map as a JS object literal
        var toolDisplayNamesLiteral = "{" + string.Join(",",
            vm.ToolDisplayNames.Select(kv =>
                $"\"{kv.Key}\":\"{kv.Value}\"")) + "}";

        var js = Js.Block()
            .Add(() => Js.Let(() => "currentSessionId", () => currentSessionId.Length > 0 ? Js.Str(() => currentSessionId) : Js.Null()))
            .Add(() => Js.Let(() => "beingName", () => Js.Str(() => beingName)))
            .Add(() => Js.Let(() => "eventSource", () => Js.Null()))
            .Add(() => Js.Let(() => "currentStreamId", () => Js.Null()))
            .Add(() => Js.Let(() => "streamingMessage", () => Js.Null()))
            .Add(() => Js.Let(() => "lastStreamElementId", () => Js.Null()))
            .Add(() => Js.Let(() => "messageCache", () => Js.New(() => Js.Id(() => "Array"))))
            .Add(() => Js.Let(() => "toolCallMap", () => Js.New(() => Js.Id(() => "Map"))))
            .Add(() => Js.Const(() => "toolDisplayNames", () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Str(() => toolDisplayNamesLiteral))));

        var autoResizeBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "textarea").Prop(() => "style").Prop(() => "height"), () => Js.Str(() => "auto")))
            .Add(() => Js.Assign(() => Js.Id(() => "textarea").Prop(() => "style").Prop(() => "height"), () => Js.Id(() => "Math").Call(() => "min", () => Js.Id(() => "textarea").Prop(() => "scrollHeight"), () => Js.Num(() => "120")).Op(() => "+", () => (JsSyntax)Js.Str(() => "px"))));
        js.Add(() => Js.Func(() => "autoResize", () => new List<string> { "textarea" }, () => autoResizeBody));

        var renderMdElementBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "el").Prop(() => "dataset").Prop(() => "mdRaw").Op(() => "&&", () => Js.Id(() => "el").Prop(() => "dataset").Prop(() => "mdRendered").Not()), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "el").Prop(() => "innerHTML"), () => Js.Ternary(
                            () => Js.Id(() => "typeof").Invoke(() => Js.Id(() => "marked")).Op(() => "!==", () => Js.Str(() => "undefined")),
                            () => Js.Id(() => "marked").Call(() => "parse", () => Js.Id(() => "el").Prop(() => "dataset").Prop(() => "mdRaw")),
                            () => Js.Id(() => "el").Prop(() => "dataset").Prop(() => "mdRaw"))),
                        Js.Assign(() => Js.Id(() => "el").Prop(() => "dataset").Prop(() => "mdRendered"), () => Js.Str(() => "1"))
                    }
                )}
            }));

        var renderMarkdownBodyBody = Js.Block()
            .Add(() => Js.Const(() => "elements", () => Js.Id(() => "root").Call(() => "querySelectorAll", () => Js.Str(() => ".markdown-body[data-md-raw]"))))
            .Add(() => Js.Id(() => "elements").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "el" }, () => renderMdElementBody)).Stmt());
        js.Add(() => Js.Func(() => "renderMarkdownBody", () => new List<string> { "root" }, () => renderMarkdownBodyBody));

        js.Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
        {
            (Js.Id(() => "typeof").Invoke(() => Js.Id(() => "marked")).Op(() => "===", () => Js.Str(() => "undefined")), new List<JsSyntax>
            {
                Js.Let(() => "mdScript", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "script"))),
                Js.Assign(() => Js.Id(() => "mdScript").Prop(() => "src"), () => Js.Str(() => "https://cdn.jsdelivr.net/npm/marked@15.0.12/marked.min.js")),
                Js.Assign(() => Js.Id(() => "mdScript").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "renderMarkdownBody").Invoke(() => Js.Id(() => "document")))),
                Js.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => Js.Id(() => "mdScript")).Stmt()
            })
        }));

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
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "isCurrentUser").Not().Op(() => "&&", () => Js.Id(() => "lastStreamElementId")), new List<JsSyntax>
                    {
                        Js.Const(() => "streamEl", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Id(() => "lastStreamElementId"))),
                        Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            { (Js.Id(() => "streamEl"), new List<JsSyntax>
                                {
                                    Js.Const(() => "tokenEl", () => Js.Id(() => "streamEl").Call(() => "querySelector", () => Js.Str(() => ".msg-token-stats"))),
                                    Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                                    {
                                        { (Js.Id(() => "tokenEl"), new List<JsSyntax>
                                            {
                                                Js.Assign(() => Js.Id(() => "tokenEl").Prop(() => "outerHTML"), () => Js.Id(() => "getTokenStats").Invoke(() => Js.Obj()
                                                    .Prop(() => "promptTokens", () => Js.Id(() => "data").Prop(() => "promptTokens"))
                                                    .Prop(() => "completionTokens", () => Js.Id(() => "data").Prop(() => "completionTokens"))
                                                    .Prop(() => "totalTokens", () => Js.Id(() => "data").Prop(() => "totalTokens"))))
                                            }
                                        )}
                                    }),
                                    Js.Assign(() => Js.Id(() => "lastStreamElementId"), () => Js.Null()),
                                    Js.Return(() => Js.Id(() => "undefined"))
                                }
                            )}
                        }),
                        Js.Assign(() => Js.Id(() => "lastStreamElementId"), () => Js.Null())
                    }
                )}
            }))
            .Add(() => Js.Id(() => "appendMessage").Invoke(() => Js.Obj()
                .Prop(() => "isUser", () => Js.Id(() => "isCurrentUser"))
                .Prop(() => "text", () => Js.Id(() => "data").Prop(() => "content"))
                .Prop(() => "thinking", () => Js.Id(() => "data").Prop(() => "thinking"))
                .Prop(() => "senderName", () => Js.Id(() => "data").Prop(() => "senderName"))
                .Prop(() => "promptTokens", () => Js.Id(() => "data").Prop(() => "promptTokens"))
                .Prop(() => "completionTokens", () => Js.Id(() => "data").Prop(() => "completionTokens"))
                .Prop(() => "totalTokens", () => Js.Id(() => "data").Prop(() => "totalTokens"))).Stmt());
        js.Add(() => Js.Func(() => "handleMessage", () => new List<string> { "data" }, () => handleMessageBody));

        var handleToolBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "data").Prop(() => "channelId").Op(() => "!==", () => Js.Id(() => "currentSessionId")), new List<JsSyntax> { Js.Return(() => Js.Id(() => "undefined")) }) }
            }))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "lastStreamElementId"), new List<JsSyntax>
                    {
                        Js.Const(() => "streamEl", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Id(() => "lastStreamElementId"))),
                        Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            { (Js.Id(() => "streamEl"), new List<JsSyntax>
                                {
                                    Js.Id(() => "streamEl").Call(() => "remove").Stmt()
                                }
                            )}
                        }),
                        Js.Assign(() => Js.Id(() => "lastStreamElementId"), () => Js.Null())
                    }
                )}
            }))
            .Add(() => Js.Id(() => "messageCache").Call(() => "push", () => Js.Obj()
                .Prop(() => "role", () => Js.Id(() => "data").Prop(() => "role"))
                .Prop(() => "content", () => Js.Id(() => "data").Prop(() => "content"))
                .Prop(() => "thinking", () => Js.Id(() => "data").Prop(() => "thinking"))
                .Prop(() => "senderName", () => Js.Id(() => "data").Prop(() => "senderName"))
                .Prop(() => "toolCallsJson", () => Js.Id(() => "data").Prop(() => "toolCallsJson"))
                .Prop(() => "toolCallId", () => Js.Id(() => "data").Prop(() => "toolCallId"))
                .Prop(() => "promptTokens", () => Js.Id(() => "data").Prop(() => "promptTokens"))
                .Prop(() => "completionTokens", () => Js.Id(() => "data").Prop(() => "completionTokens"))
                .Prop(() => "totalTokens", () => Js.Id(() => "data").Prop(() => "totalTokens"))).Stmt())
            .Add(() => Js.Id(() => "buildToolCallMap").Invoke().Stmt())
            .Add(() => Js.Id(() => "appendMessage").Invoke(() => Js.Obj()
                .Prop(() => "isUser", () => Js.Bool(() => false))
                .Prop(() => "text", () => Js.Id(() => "data").Prop(() => "content"))
                .Prop(() => "thinking", () => Js.Id(() => "data").Prop(() => "thinking"))
                .Prop(() => "senderName", () => Js.Id(() => "data").Prop(() => "senderName"))
                .Prop(() => "role", () => Js.Id(() => "data").Prop(() => "role"))
                .Prop(() => "toolCallsJson", () => Js.Id(() => "data").Prop(() => "toolCallsJson"))
                .Prop(() => "toolCallId", () => Js.Id(() => "data").Prop(() => "toolCallId"))
                .Prop(() => "promptTokens", () => Js.Id(() => "data").Prop(() => "promptTokens"))
                .Prop(() => "completionTokens", () => Js.Id(() => "data").Prop(() => "completionTokens"))
                .Prop(() => "totalTokens", () => Js.Id(() => "data").Prop(() => "totalTokens"))).Stmt());
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
            .Add(() => Js.Assign(() => Js.Id(() => "contentEl").Prop(() => "textContent"), () => Js.Id(() => "data").Prop(() => "content").Call(() => "replace", () => Js.Regex(() => "^Allow code:.*$", () => "gm"), () => Js.Str(() => "")).Call(() => "replace", () => Js.Regex(() => "^Deny code:.*$", () => "gm"), () => Js.Str(() => "")).Call(() => "trim")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "permission-cache-checkbox")).Prop(() => "checked"), () => Js.Bool(() => false)))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "permission-cache-duration")).Prop(() => "selectedIndex"), () => Js.Num(() => "0")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "permission-duration-row")).Prop(() => "style").Prop(() => "display"), () => Js.Str(() => "none")));

        var cacheCheckboxChangeBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "permission-duration-row")).Prop(() => "style").Prop(() => "display"), () => Js.Ternary(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "permission-cache-checkbox")).Prop(() => "checked"), () => Js.Str(() => "flex"), () => Js.Str(() => "none"))));

        showPermissionDialogBody
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "permission-cache-checkbox")).Call(() => "addEventListener", () => Js.Str(() => "change"), () => Js.Arrow(() => new List<string>(), () => cacheCheckboxChangeBody)).Stmt())
            .Add(() => Js.Assign(() => Js.Id(() => "overlay").Prop(() => "style").Prop(() => "display"), () => Js.Str(() => "flex")));
        js.Add(() => Js.Func(() => "showPermissionDialog", () => new List<string> { "data" }, () => showPermissionDialogBody));

        var respondPermissionBody = Js.Block()
            .Add(() => Js.Const(() => "overlay", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "permission-overlay"))))
            .Add(() => Js.Const(() => "cacheCheckbox", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "permission-cache-checkbox"))))
            .Add(() => Js.Const(() => "addToCache", () => Js.Id(() => "cacheCheckbox").Prop(() => "checked")))
            .Add(() => Js.Const(() => "cacheDuration", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "permission-cache-duration")).Prop(() => "value")))
            .Add(() => Js.Assign(() => Js.Id(() => "overlay").Prop(() => "style").Prop(() => "display"), () => Js.Str(() => "none")))
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/permission/respond").Op(() => "+", () => (JsSyntax)Js.Str(() => "?userId=")).Op(() => "+", () => (JsSyntax)Js.Str(() => userId)).Op(() => "+", () => (JsSyntax)Js.Str(() => "&allowed=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "allowed")).Op(() => "+", () => (JsSyntax)Js.Str(() => "&addToCache=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "addToCache")).Op(() => "+", () => (JsSyntax)Js.Str(() => "&cacheDuration=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "cacheDuration")), () => Js.Obj()
                .Prop(() => "method", () => Js.Str(() => "GET"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => Js.Block()
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    { (Js.Id(() => "result").Prop(() => "success").Not(), new List<JsSyntax>
                        {
                            Js.Id(() => "console").Call(() => "error", () => Js.Str(() => vm.Localization.PermissionRespondFailed)).Stmt()
                        }
                    )}
                }))
            )).Call(() => "catch", () => Js.Arrow(() => new List<string> { "err" }, () => Js.Id(() => "console").Call(() => "error", () => Js.Str(() => vm.Localization.PermissionRespondError), () => Js.Id(() => "err")))).Stmt());
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
                { (Js.Id(() => "isFinal"), new List<JsSyntax>
                    {
                        Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            { (Js.Id(() => "streamingMessage"), new List<JsSyntax>
                                {
                                    Js.Assign(() => Js.Id(() => "streamingMessage").Prop(() => "promptTokens"), () => Js.Id(() => "data").Prop(() => "promptTokens")),
                                    Js.Assign(() => Js.Id(() => "streamingMessage").Prop(() => "completionTokens"), () => Js.Id(() => "data").Prop(() => "completionTokens")),
                                    Js.Assign(() => Js.Id(() => "streamingMessage").Prop(() => "totalTokens"), () => Js.Id(() => "data").Prop(() => "totalTokens")),
                                    Js.Const(() => "finalEl", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Id(() => "streamingMessage").Prop(() => "elementId"))),
                                    Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                                    {
                                        { (Js.Id(() => "finalEl"), new List<JsSyntax>
                                            {
                                                Js.Const(() => "tokenEl", () => Js.Id(() => "finalEl").Call(() => "querySelector", () => Js.Str(() => ".msg-token-stats"))),
                                                Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                                                {
                                                    { (Js.Id(() => "tokenEl"), new List<JsSyntax>
                                                        {
                                                            Js.Assign(() => Js.Id(() => "tokenEl").Prop(() => "innerHTML"), () => Js.Id(() => "getTokenStats").Invoke(() => Js.Id(() => "streamingMessage")).Prop(() => "innerHTML"))
                                                        }
                                                    )}
                                                })
                                            }
                                        )}
                                    })
                                }
                            )}
                        }),
                        Js.Assign(() => Js.Id(() => "lastStreamElementId"), () => Js.Id(() => "streamingMessage").Prop(() => "elementId").Op(() => "||", () => (JsSyntax)Js.Null())),
                        Js.Assign(() => Js.Id(() => "currentStreamId"), () => Js.Null()),
                        Js.Assign(() => Js.Id(() => "streamingMessage"), () => Js.Null()),
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
                            .Prop(() => "promptTokens", () => Js.Null())
                            .Prop(() => "completionTokens", () => Js.Null())
                            .Prop(() => "totalTokens", () => Js.Null())
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
                        Js.Assign(() => Js.Id(() => "streamEl").Call(() => "querySelector", () => Js.Str(() => ".msg-being-text")).Prop(() => "dataset").Prop(() => "mdRaw"), () => Js.Id(() => "streamContent")),
                        Js.Assign(() => Js.Id(() => "streamEl").Call(() => "querySelector", () => Js.Str(() => ".msg-being-text")).Prop(() => "dataset").Prop(() => "mdRendered"), () => Js.Str(() => "")),
                        Js.Id(() => "renderMarkdownBody").Invoke(() => Js.Id(() => "streamEl")).Stmt(),
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
                    .Prop(() => "toolCallId", () => Js.Id(() => "m").Prop(() => "toolCallId"))
                    .Prop(() => "promptTokens", () => Js.Id(() => "m").Prop(() => "promptTokens"))
                    .Prop(() => "completionTokens", () => Js.Id(() => "m").Prop(() => "completionTokens"))
                    .Prop(() => "totalTokens", () => Js.Id(() => "m").Prop(() => "totalTokens"))).Stmt())
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

        // Helper: build token stats HTML string from msg
        var getTokenStatsBody = Js.Block()
            .Add(() => Js.Const(() => "p", () => Js.Id(() => "msg").Prop(() => "promptTokens").Op(() => "??", () => Js.Num(() => "0"))))
            .Add(() => Js.Const(() => "c", () => Js.Id(() => "msg").Prop(() => "completionTokens").Op(() => "??", () => Js.Num(() => "0"))))
            .Add(() => Js.Const(() => "t", () => Js.Id(() => "msg").Prop(() => "totalTokens").Op(() => "??", () => Js.Num(() => "0"))))
            .Add(() => Js.Return(() => Js.Str(() => "<div class=\"msg-token-stats\">Token: \u2191").Op(() => "+", () => (JsSyntax)Js.Id(() => "p")).Op(() => "+", () => (JsSyntax)Js.Str(() => " \u2193")).Op(() => "+", () => (JsSyntax)Js.Id(() => "c")).Op(() => "+", () => (JsSyntax)Js.Str(() => " \u03a3")).Op(() => "+", () => (JsSyntax)Js.Id(() => "t")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>"))));
        js.Add(() => Js.Func(() => "getTokenStats", () => new List<string> { "msg" }, () => getTokenStatsBody));
        var decodeUnicodeBody = Js.Block()
            .Add(() => Js.Return(() => Js.Id(() => "str").Call(() => "replace",
                () => Js.New(() => Js.Id(() => "RegExp"), () => Js.Str(() => "\\\\u([0-9a-fA-F]{4})"), () => Js.Str(() => "g")),
                () => Js.Arrow(() => new List<string> { "_", "code" },
                    () => Js.Id(() => "String").Call(() => "fromCharCode",
                        () => Js.Id(() => "parseInt").Invoke(() => Js.Id(() => "code"), () => Js.Num(() => "16")))))));
        js.Add(() => Js.Func(() => "decodeUnicode", () => new List<string> { "str" }, () => decodeUnicodeBody));

        // Helper: resolve localized tool display name from msg
        var getToolSummaryBody = Js.Block()
            .Add(() => Js.Const(() => "defaultLabel", () => Js.Str(() => "🔧 Tool Call")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "msg").Prop(() => "toolCallsJson"), new List<JsSyntax>
                    {
                        Js.Const(() => "tcs", () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "msg").Prop(() => "toolCallsJson"))),
                        Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            { (Js.Id(() => "tcs").Prop(() => "length").Op(() => ">", () => Js.Num(() => "0")), new List<JsSyntax>
                                {
                                    Js.Const(() => "n", () => Js.Id(() => "tcs").Index(() => Js.Num(() => "0")).Prop(() => "Name")),
                                    Js.Return(() => Js.Str(() => "🔧 ").Op(() => "+", () => (JsSyntax)Js.Id(() => "toolDisplayNames").Index(() => Js.Id(() => "n")).Op(() => "||", () => (JsSyntax)Js.Id(() => "n"))))
                                }
                            )}
                        })
                    }
                )}
            }))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "msg").Prop(() => "toolCallId").Op(() => "&&", () => Js.Id(() => "toolCallMap").Call(() => "has", () => Js.Id(() => "msg").Prop(() => "toolCallId"))), new List<JsSyntax>
                    {
                        Js.Const(() => "tc", () => Js.Id(() => "toolCallMap").Call(() => "get", () => Js.Id(() => "msg").Prop(() => "toolCallId"))),
                        Js.Const(() => "n", () => Js.Id(() => "tc").Prop(() => "Name")),
                        Js.Return(() => Js.Str(() => "🔧 ").Op(() => "+", () => (JsSyntax)Js.Id(() => "toolDisplayNames").Index(() => Js.Id(() => "n")).Op(() => "||", () => (JsSyntax)Js.Id(() => "n"))))
                    }
                )}
            }))
            .Add(() => Js.Return(() => Js.Id(() => "defaultLabel")));
        js.Add(() => Js.Func(() => "getToolSummary", () => new List<string> { "msg" }, () => getToolSummaryBody));

        var userMsgBody = new List<JsSyntax>
        {
            Js.Assign(() => Js.Id(() => "div").Prop(() => "className"), () => Js.Str(() => "msg-user")),
        };
        
        var escapeUserText = Js.Id(() => "msg").Prop(() => "text")
            .Call(() => "replace", () => Js.Regex(() => @"&", () => "g"), () => Js.Str(() => "&amp;"))
            .Call(() => "replace", () => Js.Regex(() => "\"", () => "g"), () => Js.Str(() => "&quot;"))
            .Call(() => "replace", () => Js.Regex(() => @"<", () => "g"), () => Js.Str(() => "&lt;"))
            .Call(() => "replace", () => Js.Regex(() => @">", () => "g"), () => Js.Str(() => "&gt;"));
        
        userMsgBody.Add(Js.Assign(() => Js.Id(() => "div").Prop(() => "innerHTML"), () => Js.Str(() => "<div class=\"msg-user-content\"><div class=\"msg-user-bubble\">").Op(() => "+", () => (JsSyntax)escapeUserText).Op(() => "+", () => (JsSyntax)Js.Str(() => $"</div></div><div class=\"msg-user-avatar\"><div class=\"msg-avatar-icon\">U</div><div class=\"msg-avatar-name\">{vm.Localization.ChatUserAvatarName}</div></div>"))));
        var toolCallRequestDecoded = Js.Id(() => "decodeUnicode").Invoke(() => toolCallRequestExpr);
        var toolMsgBody = new List<JsSyntax>
        {
            Js.Assign(() => Js.Id(() => "div").Prop(() => "className"), () => Js.Str(() => "msg-tool")),
            Js.Assign(() => Js.Id(() => "div").Prop(() => "innerHTML"), () => Js.Str(() => "<details class=\"msg-collapsible\"><summary>").Op(() => "+", () => (JsSyntax)Js.Id(() => "getToolSummary").Invoke(() => Js.Id(() => "msg"))).Op(() => "+", () => (JsSyntax)Js.Str(() => "</summary><div class=\"msg-tool-content\"><div class=\"msg-tool-section\"><div class=\"msg-tool-label\">Request:</div><pre class=\"msg-tool-code\">")).Op(() => "+", () => (JsSyntax)toolCallRequestDecoded).Op(() => "+", () => (JsSyntax)Js.Str(() => "</pre></div><div class=\"msg-tool-section\"><div class=\"msg-tool-label\">Response:</div><pre class=\"msg-tool-code\">")).Op(() => "+", () => (JsSyntax)Js.Id(() => "decodeUnicode").Invoke(() => Js.Id(() => "msg").Prop(() => "text"))).Op(() => "+", () => (JsSyntax)Js.Str(() => "</pre></div></div></details>"))),
        };
        var beingMsgBody = new List<JsSyntax>
        {
            Js.Assign(() => Js.Id(() => "div").Prop(() => "className"), () => Js.Str(() => "msg-being")),
            Js.Assign(() => Js.Id(() => "div").Prop(() => "id"), () => Js.Id(() => "msg").Prop(() => "elementId").Op(() => "||", () => (JsSyntax)Js.Str(() => ""))),
        };
        
        var escapeBeingText = Js.Id(() => "msg").Prop(() => "text")
            .Call(() => "replace", () => Js.Regex(() => @"&", () => "g"), () => Js.Str(() => "&amp;"))
            .Call(() => "replace", () => Js.Regex(() => "\"", () => "g"), () => Js.Str(() => "&quot;"))
            .Call(() => "replace", () => Js.Regex(() => @"<", () => "g"), () => Js.Str(() => "&lt;"))
            .Call(() => "replace", () => Js.Regex(() => @">", () => "g"), () => Js.Str(() => "&gt;"));
        
        beingMsgBody.Add(Js.Assign(() => Js.Id(() => "div").Prop(() => "innerHTML"), () => Js.Str(() => "<div class=\"msg-being-avatar\"><div class=\"msg-avatar-icon\">").Op(() => "+", () => (JsSyntax)Js.Id(() => "msg").Prop(() => "senderName").Op(() => "||", () => (JsSyntax)Js.Id(() => "beingName")).Paren().Call(() => "charAt", () => Js.Num(() => "0"))).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div><div class=\"msg-avatar-name\">")).Op(() => "+", () => (JsSyntax)Js.Id(() => "msg").Prop(() => "senderName").Op(() => "||", () => (JsSyntax)Js.Id(() => "beingName")).Paren()).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div></div><div class=\"msg-being-content\"><div class=\"msg-being-card\"><div class=\"msg-being-body\"><details class=\"msg-collapsible\"><summary>" + vm.Localization.ChatThinkingSummary + "</summary><div class=\"msg-thinking-content\">")).Op(() => "+", () => (JsSyntax)Js.Id(() => "msg").Prop(() => "thinking").Op(() => "||", () => (JsSyntax)Js.Str(() => "")).Paren()).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div></details><div class=\"msg-being-text markdown-body\" data-md-raw=\"")).Op(() => "+", () => (JsSyntax)escapeBeingText).Op(() => "+", () => (JsSyntax)Js.Str(() => "\"></div></div>")).Op(() => "+", () => (JsSyntax)Js.Id(() => "getTokenStats").Invoke(() => Js.Id(() => "msg"))).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div></div>"))));
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
            .Add(() => Js.Id(() => "renderMarkdownBody").Invoke(() => Js.Id(() => "div")).Stmt())
            .Add(() => Js.Id(() => "messages").Call(() => "scrollTo", () => Js.Obj().Prop(() => "top", () => Js.Id(() => "messages").Prop(() => "scrollHeight")).Prop(() => "behavior", () => Js.Str(() => "smooth"))).Stmt());
        js.Add(() => Js.Func(() => "appendMessage", () => new List<string> { "msg" }, () => appendMessageBody));

        js.Add(() => Js.Id(() => "initInput").Invoke().Stmt());
        js.Add(() => Js.Id(() => "initSessionList").Invoke().Stmt());
        js.Add(() => Js.Id(() => "renderMarkdownBody").Invoke(() => Js.Id(() => "document")).Stmt());
        js.Add(() => Js.Id(() => "connectSSE").Invoke().Stmt());
        js.Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "chat-messages")).Call(() => "scrollTo", () => Js.Obj().Prop(() => "top", () => Js.Num(() => "999999")).Prop(() => "behavior", () => Js.Str(() => "auto"))).Stmt());

        return js;
    }
}