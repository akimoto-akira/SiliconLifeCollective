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

using SiliconLife.Default.Web.Models;

namespace SiliconLife.Default.Web.Views;

public class ChatHistoryDetailView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as ChatHistoryDetailViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        var scripts = GetScripts(vm);
        var styles = GetStyles();
        
        return RenderPage(vm.Skin, vm.Localization.ChatDetailPageTitle, "chat-detail", vm.Localization, body, scripts, styles);
    }

    private static H RenderBody(ChatHistoryDetailViewModel vm)
    {
        return H.Div(
            H.Div(
                H.Div(
                    H.A(vm.Localization.ChatHistoryBackToList).Href($"/chat-history?beingId={vm.BeingId}").Class("back-link")
                ).Class("back-nav"),
                H.H1(vm.Localization.ChatDetailPageHeader),
                H.P($"会话ID: {vm.SessionId}").Class("page-subtitle")
            ).Class("page-header"),
            H.Div().Id("message-list").Class("message-list")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".back-nav")
                .Property("margin-bottom", "16px")
            .EndSelector()
            .Selector(".back-link")
                .Property("color", "var(--accent-primary)")
                .Property("text-decoration", "none")
                .Property("font-weight", "bold")
                .Property("transition", "color 0.2s")
            .EndSelector()
            .Selector(".back-link:hover")
                .Property("color", "var(--accent-secondary, var(--accent-primary))")
                .Property("text-decoration", "underline")
            .EndSelector()
            .Selector(".page-subtitle")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-top", "8px")
                .Property("font-family", "monospace")
            .EndSelector()
            .Selector(".message-list")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "16px")
                .Property("margin-top", "20px")
            .EndSelector()
            .Selector(".message-item")
                .Property("background", "var(--bg-card)")
                .Property("padding", "16px")
                .Property("border-radius", "12px")
                .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".message-time")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-bottom", "8px")
                .Property("font-family", "monospace")
            .EndSelector()
            .Selector(".message-sender")
                .Property("font-size", "14px")
                .Property("font-weight", "bold")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".user-sender")
                .Property("color", "var(--accent-secondary, var(--accent-primary))")
            .EndSelector()
            .Selector(".being-sender")
                .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".message-body")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "8px")
            .EndSelector()
            .Selector(".message-thinking")
                .Property("font-size", "13px")
                .Property("color", "var(--text-secondary)")
                .Property("padding", "12px")
                .Property("background", "var(--bg-secondary, rgba(0,0,0,0.1))")
                .Property("border-radius", "6px")
                .Property("white-space", "pre-wrap")
                .Property("border-left", "3px solid var(--accent-secondary, var(--accent-primary))")
            .EndSelector()
            .Selector(".message-content")
                .Property("font-size", "14px")
                .Property("color", "var(--text-primary)")
                .Property("line-height", "1.6")
            .EndSelector()
            .Selector(".message-tool-calls")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
                .Property("padding", "12px")
                .Property("background", "var(--bg-secondary, rgba(0,0,0,0.1))")
                .Property("border-radius", "6px")
                .Property("font-family", "monospace")
                .Property("white-space", "pre-wrap")
                .Property("overflow-x", "auto")
            .EndSelector()
            .Selector(".tool-message")
                .Property("background", "var(--bg-secondary, rgba(0,0,0,0.05))")
                .Property("border-left", "4px solid var(--accent-secondary, var(--accent-primary))")
            .EndSelector()
            .Selector(".message-tool-content")
                .Property("padding", "12px")
                .Property("background", "var(--bg-card)")
                .Property("border-radius", "6px")
                .Property("margin-top", "8px")
            .EndSelector()
            .Selector(".msg-tool-section")
                .Property("margin-bottom", "12px")
            .EndSelector()
            .Selector(".msg-tool-section:last-child")
                .Property("margin-bottom", "0")
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
            .Selector(".tool-message")
                .Property("margin", "8px 0")
            .EndSelector()
            .Selector(".tool-message .msg-collapsible")
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
            .Selector(".empty-state")
                .Property("text-align", "center")
                .Property("padding", "40px")
                .Property("color", "var(--text-secondary)")
                .Property("font-size", "14px")
            .EndSelector();
    }

    private static JsSyntax GetScripts(ChatHistoryDetailViewModel vm)
    {
        // Serialize the tool display name map as a JS object literal
        var toolDisplayNamesLiteral = "{" + string.Join(",",
            vm.ToolDisplayNames.Select(kv =>
                $"\"{kv.Key}\":\"{kv.Value}\"")) + "}";
        
        // Global tool display names dictionary
        var toolDisplayNamesDecl = Js.Const(() => "toolDisplayNames", () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Str(() => toolDisplayNamesLiteral)));
        
        // decodeUnicode function
        var decodeUnicodeBody = Js.Block()
            .Add(() => Js.Return(() => Js.Id(() => "str")
                .Call(() => "replace",
                    () => Js.Regex(() => @"\\u([0-9a-fA-F]{4})", () => "g"),
                    () => Js.Arrow(
                        () => new List<string> { "_", "code" },
                        () => Js.Id(() => "String")
                            .Prop(() => "fromCharCode")
                            .Invoke(
                                () => Js.Id(() => "parseInt")
                                    .Invoke(
                                        () => Js.Id(() => "code"),
                                        () => Js.Num(() => "16")
                                    )
                            )
                    )
                )));
        var decodeUnicodeFunc = Js.Func(() => "decodeUnicode", () => new List<string> { "str" }, () => decodeUnicodeBody);
        
        // getToolSummary function
        var getToolSummaryBody = Js.Block()
            .Add(() => Js.Let(() => "defaultLabel", () => Js.Str(() => "🔧 Tool Call")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "msg").Prop(() => "toolCallsJson"), new List<JsSyntax>
                {
                    Js.Let(() => "tcs", () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "msg").Prop(() => "toolCallsJson"))),
                    Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "tcs").Prop(() => "length").Op(() => ">", () => Js.Num(() => "0")), new List<JsSyntax>
                        {
                            Js.Let(() => "n", () => Js.Id(() => "tcs").Index(() => Js.Num(() => "0")).Prop(() => "Name")),
                            Js.Return(() => Js.Str(() => "🔧 ").Op(() => "+",
                                () => Js.Id(() => "toolDisplayNames")
                                    .Index(() => Js.Id(() => "n"))
                                    .Op(() => "||", () => Js.Id(() => "n"))))
                        })
                    })
                })
            }))
            .Add(() => Js.Return(() => Js.Id(() => "defaultLabel")));
        var getToolSummaryFunc = Js.Func(() => "getToolSummary", () => new List<string> { "msg" }, () => getToolSummaryBody);
        
        // renderToolMessage function
        var renderToolMessageBody = Js.Block()
            .Add(() => Js.Let(() => "html", () => Js.Str(() => "<div class='message-item tool-message'>")))
            .Add(() =>
            {
                var timestampPart = Js.Id(() => "msg").Prop(() => "timestamp");
                return Js.Assign(
                    () => Js.Id(() => "html"),
                    () => Js.Id(() => "html")
                        .Op(() => "+", () => Js.Str(() => "<div class='message-time'>🕐 "))
                        .Op(() => "+", () => timestampPart)
                        .Op(() => "+", () => Js.Str(() => "</div>"))
                );
            })
            .Add(() =>
            {
                var summaryCall = Js.Id(() => "getToolSummary").Invoke(() => Js.Id(() => "msg"));
                return Js.Assign(
                    () => Js.Id(() => "html"),
                    () => Js.Id(() => "html")
                        .Op(() => "+", () => Js.Str(() => "<details class='msg-collapsible'><summary>"))
                        .Op(() => "+", () => summaryCall)
                        .Op(() => "+", () => Js.Str(() => "</summary>"))
                );
            })
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Op(() => "+", () => Js.Str(() => "<div class='message-tool-content'>"))))
            // Request section
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Op(() => "+", () => Js.Str(() => "<div class='msg-tool-section'><div class='msg-tool-label'>Request:</div>"))))
            .Add(() => Js.Let(() => "requestJson", () => Js.Ternary(
                () => Js.Id(() => "msg").Prop(() => "toolCallsJson"),
                () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "msg").Prop(() => "toolCallsJson")),
                () => Js.Obj()
            )))
            .Add(() =>
            {
                var stringifyCall = Js.Id(() => "decodeUnicode").Invoke(
                    () => Js.Id(() => "JSON")
                        .Call(() => "stringify",
                            () => Js.Id(() => "requestJson"),
                            () => Js.Null(),
                            () => Js.Num(() => "2")
                        )
                );
                return Js.Assign(
                    () => Js.Id(() => "html"),
                    () => Js.Id(() => "html")
                        .Op(() => "+", () => Js.Str(() => "<pre class='msg-tool-code'>"))
                        .Op(() => "+", () => stringifyCall)
                        .Op(() => "+", () => Js.Str(() => "</pre>"))
                );
            })
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Op(() => "+", () => Js.Str(() => "</div>"))))
            // Response section
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "msg").Prop(() => "toolResult"), new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html")
                        .Op(() => "+", () => Js.Str(() => "<div class='msg-tool-section'><div class='msg-tool-label'>Response:</div><pre class='msg-tool-code'>"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html")
                        .Op(() => "+", () => Js.Id(() => "decodeUnicode").Invoke(() => Js.Id(() => "msg").Prop(() => "toolResult")))).Stmt(),
                    Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html")
                        .Op(() => "+", () => Js.Str(() => "</pre></div>"))).Stmt()
                })
            }))
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Op(() => "+", () => Js.Str(() => "</div></details></div>"))))
            .Add(() => Js.Return(() => Js.Id(() => "html")));
        var renderToolMessageFunc = Js.Func(() => "renderToolMessage", () => new List<string> { "msg" }, () => renderToolMessageBody);
        
        // renderMessageItem function
        var renderMessageItemBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "msg").Prop(() => "toolCallsJson"), new List<JsSyntax>
                {
                    Js.Return(() => Js.Id(() => "renderToolMessage").Invoke(() => Js.Id(() => "msg")))
                })
            }))
            .Add(() => Js.Let(() => "html", () => Js.Str(() => "<div class='message-item'>")))
            .Add(() =>
            {
                var timestampPart = Js.Id(() => "msg").Prop(() => "timestamp");
                return Js.Assign(
                    () => Js.Id(() => "html"),
                    () => Js.Id(() => "html")
                        .Op(() => "+", () => Js.Str(() => "<div class='message-time'>🕐 "))
                        .Op(() => "+", () => timestampPart)
                        .Op(() => "+", () => Js.Str(() => "</div>"))
                );
            })
            // Sender
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "msg").Prop(() => "role").Op(() => "===", () => Js.Str(() => "User")), new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html")
                        .Op(() => "+", () => Js.Str(() => "<div class='message-sender user-sender'>👤 "))
                        .Op(() => "+", () => Js.Id(() => "msg").Prop(() => "senderName"))
                        .Op(() => "+", () => Js.Str(() => "</div>"))).Stmt()
                }),
                (null, new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html")
                        .Op(() => "+", () => Js.Str(() => "<div class='message-sender being-sender'>🤖 "))
                        .Op(() => "+", () => Js.Id(() => "msg").Prop(() => "senderName"))
                        .Op(() => "+", () => Js.Str(() => "</div>"))).Stmt()
                })
            }))
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Op(() => "+", () => Js.Str(() => "<div class='message-body'>"))))
            // Thinking
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "msg").Prop(() => "thinking"), new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html")
                        .Op(() => "+", () => Js.Str(() => "<details class='msg-collapsible'><summary>💭 思考过程</summary><div class='message-thinking'>"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html")
                        .Op(() => "+", () => Js.Id(() => "msg").Prop(() => "thinking"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html")
                        .Op(() => "+", () => Js.Str(() => "</div></details>"))).Stmt()
                })
            }))
            // Content with markdown - escape quotes
            .Add(() =>
            {
                var escapedContent = Js.Id(() => "msg").Prop(() => "content")
                    .Call(() => "replace",
                        () => Js.Regex(() => "\"", () => "g"),
                        () => Js.Str(() => "&quot;")
                    );
                var divStart = Js.Str(() => "<div class='message-content markdown-body' data-md-raw='");
                var divEnd = Js.Str(() => "'></div>");
                return Js.Assign(
                    () => Js.Id(() => "html"),
                    () => Js.Id(() => "html")
                        .Op(() => "+", () => divStart)
                        .Op(() => "+", () => escapedContent)
                        .Op(() => "+", () => divEnd)
                );
            })
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Op(() => "+", () => Js.Str(() => "</div></div>"))))
            .Add(() => Js.Return(() => Js.Id(() => "html")));
        var renderMessageItemFunc = Js.Func(() => "renderMessageItem", () => new List<string> { "msg" }, () => renderMessageItemBody);
        
        // Markdown rendering functions
        var renderMdElementBody = Js.Block()
            .Add(() => Js.Const(() => "raw", () => Js.Id(() => "el").Call(() => "getAttribute", () => Js.Str(() => "data-md-raw"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "raw"), new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "el").Prop(() => "innerHTML"), () => Js.Id(() => "marked").Call(() => "parse", () => Js.Id(() => "raw"))).Stmt()
                })
            }));
            
        var renderMarkdownBodyBody = Js.Block()
            .Add(() => Js.Const(() => "elements", () => Js.Id(() => "root").Call(() => "querySelectorAll", () => Js.Str(() => ".markdown-body[data-md-raw]"))))
            .Add(() => Js.Id(() => "elements").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "el" }, () => renderMdElementBody)).Stmt());
    
        var renderMarkdownBodyFunc = Js.Func(() => "renderMarkdownBody", () => new List<string> { "root" }, () => renderMarkdownBodyBody);
        
        // Marked.js loading logic
        var markedLoadCheck = Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
        {
            (Js.Id(() => "typeof").Invoke(() => Js.Id(() => "marked")).Op(() => "===", () => Js.Str(() => "undefined")), new List<JsSyntax>
            {
                Js.Let(() => "mdScript", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "script"))),
                Js.Assign(() => Js.Id(() => "mdScript").Prop(() => "src"), () => Js.Str(() => "https://cdn.jsdelivr.net/npm/marked@15.0.12/marked.min.js")),
                Js.Assign(() => Js.Id(() => "mdScript").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "renderMarkdownBody").Invoke(() => Js.Id(() => "document")))),
                Js.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => Js.Id(() => "mdScript")).Stmt()
            })
        });
        
        // window.onload - fetch and render messages
        var onloadBody = Js.Block()
            .Add(() => Js.Id(() => "fetch")
                .Invoke(() => Js.Str(() => $"/api/chat-history/messages?sessionId={vm.SessionId}"))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json")))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => Js.Block()
                    .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "message-list"))))
                    .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "data").Prop(() => "messages").Not().Op(() => "||", () => Js.Id(() => "data").Prop(() => "messages").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0"))), new List<JsSyntax>
                        {
                            Js.Assign(() => Js.Id(() => "container").Prop(() => "innerHTML"), () => Js.Str(() => $"<div class='empty-state'>{vm.Localization.ChatDetailNoMessages}</div>")),
                            Js.Return(() => Js.Null())
                        })
                    }))
                    .Add(() => Js.Assign(() => Js.Id(() => "container").Prop(() => "innerHTML"), () => Js.Id(() => "data").Prop(() => "messages").Call(() => "map", () => Js.Id(() => "renderMessageItem")).Call(() => "join", () => Js.Str(() => ""))))
                    .Add(() => Js.Id(() => "renderMarkdownBody").Invoke(() => Js.Id(() => "container")).Stmt())
                )));
        
        var onloadAssign = Js.Assign(
            () => Js.Id(() => "window").Prop(() => "onload"),
            () => Js.Arrow(() => new List<string>(), () => onloadBody)
        );
        
        return Js.Block()
            .Add(() => toolDisplayNamesDecl)
            .Add(() => decodeUnicodeFunc)
            .Add(() => getToolSummaryFunc)
            .Add(() => renderToolMessageFunc)
            .Add(() => renderMessageItemFunc)
            .Add(() => renderMarkdownBodyFunc)
            .Add(() => markedLoadCheck)
            .Add(() => onloadAssign);
    }
}
