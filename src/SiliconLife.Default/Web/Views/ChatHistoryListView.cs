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

public class ChatHistoryListView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as ChatHistoryListViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.ChatHistoryPageTitle, "chat-history", vm.Localization, body, GetScripts(vm), GetStyles(), helpTopicId: "chat-system");
    }

    private static H RenderBody(ChatHistoryListViewModel vm)
    {
        return H.Div(
            H.Div(
                H.Div(
                    H.A(vm.Localization.BeingsBackToList).Href("/beings").Class("back-link")
                ).Class("back-nav"),
                H.H1($"{vm.BeingName} - {vm.Localization.ChatHistoryPageHeader}"),
                H.P(vm.Localization.ChatHistoryConversationList).Class("page-subtitle")
            ).Class("page-header"),
            H.Div().Id("conversation-list").Class("conversation-list"),
            H.Div(
                H.Div("").Class("loading-spinner"),
                H.Div(vm.Localization.ChatLoading).Class("loading-text")
            ).Id("loading-indicator").Class("loading-indicator")
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
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-top", "8px")
            .EndSelector()
            .Selector(".conversation-list")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "16px")
                .Property("margin-top", "20px")
            .EndSelector()
            .Selector(".conversation-card")
                .Property("background", "var(--bg-card)")
                .Property("padding", "20px")
                .Property("border-radius", "12px")
                .Property("border", "1px solid var(--border)")
                .Property("cursor", "pointer")
                .Property("transition", "transform 0.2s, box-shadow 0.2s")
            .EndSelector()
            .Selector(".conversation-card:hover")
                .Property("transform", "translateY(-2px)")
                .Property("box-shadow", "0 4px 12px rgba(0,0,0,0.12)")
            .EndSelector()
            .Selector(".conversation-title")
                .Property("font-size", "16px")
                .Property("font-weight", "bold")
                .Property("color", "var(--text-primary)")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".conversation-participants")
                .Property("font-size", "14px")
                .Property("color", "var(--accent-primary)")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".conversation-meta")
                .Property("display", "flex")
                .Property("gap", "16px")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".conversation-last-msg")
                .Property("font-size", "13px")
                .Property("color", "var(--text-primary)")
                .Property("padding", "8px")
                .Property("background", "var(--bg-secondary, rgba(0,0,0,0.1))")
                .Property("border-radius", "6px")
                .Property("white-space", "nowrap")
                .Property("overflow", "hidden")
                .Property("text-overflow", "ellipsis")
            .EndSelector()
            .Selector(".empty-state")
                .Property("text-align", "center")
                .Property("padding", "40px")
                .Property("color", "var(--text-secondary)")
                .Property("font-size", "14px")
            .EndSelector()
            .Selector(".loading-indicator")
                .Property("display", "none")
                .Property("flex-direction", "column")
                .Property("align-items", "center")
                .Property("justify-content", "center")
                .Property("padding", "40px 20px")
                .Property("color", "var(--text-secondary)")
                .Property("gap", "16px")
            .EndSelector()
            .Selector(".loading-indicator.active")
                .Property("display", "flex")
            .EndSelector()
            .Selector(".loading-spinner")
                .Property("width", "40px")
                .Property("height", "40px")
                .Property("border", "3px solid var(--border)")
                .Property("border-top-color", "var(--accent-primary)")
                .Property("border-radius", "50%")
                .Property("animation", "spin 1s linear infinite")
            .EndSelector()
            .Selector(".loading-text")
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Keyframes("spin", kf => kf
                .At("0%", p => p.Property("transform", "rotate(0deg)"))
                .At("100%", p => p.Property("transform", "rotate(360deg)")));
    }

    private static JsSyntax GetScripts(ChatHistoryListViewModel vm)
    {
        var cardHtml = Js.Str(() => "<div class='conversation-card' onclick=\"window.location='/chat-history-detail?sessionId=")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "c").Prop(() => "id"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"&beingId={vm.BeingId}'\">"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "<div class='conversation-title'>会话: </div>")
                .Op(() => "+", () => (JsSyntax)Js.Id(() => "c").Prop(() => "id").Call(() => "substring", () => Js.Num(() => "0"), () => Js.Num(() => "8"))))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "<div class='conversation-participants'>参与方: </div>")
                .Op(() => "+", () => (JsSyntax)Js.Id(() => "c").Prop(() => "participants")))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "<div class='conversation-meta'><span>最后消息: </span>")
                .Op(() => "+", () => (JsSyntax)Js.Id(() => "c").Prop(() => "lastMessageTime"))
                .Op(() => "+", () => (JsSyntax)Js.Str(() => "<span>消息数: </span>")
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "c").Prop(() => "messageCount"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => "</span></div>"))))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "<div class='conversation-last-msg'>")
                .Op(() => "+", () => (JsSyntax)Js.Id(() => "c").Prop(() => "lastMessage"))
                .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div></div>")));
    
        var thenBody = Js.Block()
            .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "conversation-list"))))
            .Add(() => Js.Const(() => "indicator", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "loading-indicator"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "indicator"), new List<JsSyntax>
                    {
                        Js.Id(() => "indicator").Prop(() => "classList").Call(() => "remove", () => Js.Str(() => "active")).Stmt()
                    }
                )}
            }))
            .Add(() => Js.Assign(() => Js.Id(() => "container").Prop(() => "innerHTML"),
                () => Js.Id(() => "data").Prop(() => "conversations").Call(() => "map", () => Js.Arrow(() => new List<string> { "c" },
                    () => cardHtml))));
    
        var onloadBody = Js.Block()
            .Add(() => Js.Const(() => "indicator", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "loading-indicator"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "indicator"), new List<JsSyntax>
                    {
                        Js.Id(() => "indicator").Prop(() => "classList").Call(() => "add", () => Js.Str(() => "active")).Stmt()
                    }
                )}
            }))
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => $"/api/chat-history/conversations?beingId={vm.BeingId}")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => thenBody)).Stmt());
    
        return Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => onloadBody)));
    }
}
