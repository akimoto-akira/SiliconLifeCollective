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

public class BeingView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as BeingViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.PageTitleBeings, "beings", vm.Localization, body, GetScripts(vm.Localization), GetStyles());
    }

    private static H RenderBody(BeingViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1(vm.Localization.BeingsPageHeader),
                H.Div(
                    H.Span(string.Format(vm.Localization.BeingsTotalCount, "")).Id("total-count").Class("stat-value")
                ).Class("page-stat")
            ).Class("page-header"),
            H.Div().Id("beings-grid").Class("beings-grid"),
            H.Div(
                H.Div(
                    H.P(vm.Localization.BeingsNoSelectionPlaceholder)
                ).Id("detail-content").Class("detail-content")
            ).Id("detail-panel").Class("detail-panel")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".page-stat")
                .Property("margin-left", "16px")
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".stat-value")
                .Property("font-weight", "bold")
                .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".beings-grid")
                .Property("display", "grid")
                .Property("grid-template-columns", "repeat(auto-fill, minmax(280px, 1fr))")
                .Property("gap", "20px")
                .Property("margin-bottom", "30px")
            .EndSelector()
            .Selector(".being-card")
                .Property("background", "var(--bg-card)")
                .Property("padding", "20px")
                .Property("border-radius", "12px")
                .Property("border", "1px solid var(--border)")
                .Property("cursor", "pointer")
                .Property("transition", "transform 0.2s, box-shadow 0.2s")
            .EndSelector()
            .Selector(".being-card:hover")
                .Property("transform", "translateY(-4px)")
                .Property("box-shadow", "0 4px 16px rgba(0,0,0,0.12)")
            .EndSelector()
            .Selector(".being-card.selected")
                .Property("border", "2px solid var(--accent-primary)")
            .EndSelector()
            .Selector(".being-name")
                .Property("font-size", "18px")
                .Property("font-weight", "bold")
                .Property("color", "var(--text-primary)")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".being-status")
                .Property("display", "inline-block")
                .Property("padding", "4px 12px")
                .Property("border-radius", "12px")
                .Property("font-size", "12px")
            .EndSelector()
            .Selector(".being-status.idle")
                .Property("background", "rgba(107,203,119,0.15)")
                .Property("color", "var(--accent-success)")
            .EndSelector()
            .Selector(".being-status.running")
                .Property("background", "rgba(77,150,255,0.15)")
                .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".being-info")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-top", "8px")
            .EndSelector()
            .Selector(".being-type-badge")
                .Property("display", "inline-block")
                .Property("padding", "2px 8px")
                .Property("border-radius", "4px")
                .Property("font-size", "11px")
                .Property("background", "rgba(156,136,255,0.15)")
                .Property("color", "#9c88ff")
                .Property("margin-left", "8px")
            .EndSelector()
            .Selector(".detail-panel")
                .Property("background", "var(--bg-card)")
                .Property("padding", "20px")
                .Property("border-radius", "12px")
                .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".detail-content h2")
                .Property("font-size", "22px")
                .Property("color", "var(--text-primary)")
                .Property("margin-bottom", "15px")
            .EndSelector()
            .Selector(".detail-row")
                .Property("display", "flex")
                .Property("margin-bottom", "12px")
                .Property("align-items", "flex-start")
            .EndSelector()
            .Selector(".detail-label")
                .Property("font-weight", "bold")
                .Property("color", "var(--text-secondary)")
                .Property("width", "100px")
                .Property("flex-shrink", "0")
            .EndSelector()
            .Selector(".detail-value")
                .Property("color", "var(--text-primary)")
                .Property("word-break", "break-all")
            .EndSelector()
            .Selector(".detail-value.idle")
                .Property("color", "var(--accent-success)")
            .EndSelector()
            .Selector(".detail-value.running")
                .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".soul-content")
                .Property("background", "var(--bg-secondary, rgba(0,0,0,0.1))")
                .Property("padding", "12px")
                .Property("border-radius", "8px")
                .Property("font-size", "13px")
                .Property("line-height", "1.6")
                .Property("max-height", "200px")
                .Property("overflow-y", "auto")
                .Property("white-space", "pre-wrap")
            .EndSelector()
            .Selector(".detail-link")
                .Property("color", "var(--accent-primary)")
                .Property("text-decoration", "none")
                .Property("font-weight", "bold")
                .Property("transition", "color 0.2s")
            .EndSelector()
            .Selector(".detail-link:hover")
                .Property("color", "var(--accent-secondary, var(--accent-primary))")
                .Property("text-decoration", "underline")
            .EndSelector()
            .Selector(".empty-state")
                .Property("text-align", "center")
                .Property("padding", "40px")
                .Property("color", "var(--text-secondary)")
            .EndSelector();
    }

    private static JsSyntax GetScripts(DefaultLocalizationBase loc)
    {
        var forEachBody = Js.Block()
            .Add(() => Js.Const(() => "card", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Const(() => "statusClass", () => Js.Ternary(() => Js.Id(() => "b").Prop(() => "isIdle"), () => Js.Str(() => "idle"), () => Js.Str(() => "running"))))
            .Add(() => Js.Const(() => "statusText", () => Js.Ternary(() => Js.Id(() => "b").Prop(() => "isIdle"), () => Js.Str(() => loc.BeingsStatusIdle), () => Js.Str(() => loc.BeingsStatusRunning))))
            .Add(() => Js.Const(() => "isSelected", () => Js.Id(() => "selectedBeingId").Op(() => "===", () => Js.Id(() => "b").Prop(() => "id"))))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "className"), () => Js.Ternary(() => Js.Id(() => "isSelected"), () => Js.Str(() => "being-card selected"), () => Js.Str(() => "being-card"))))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "onclick"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "selectBeing").Invoke(() => Js.Id(() => "b").Prop(() => "id"), () => Js.Id(() => "b").Prop(() => "name")))))
            .Add(() => Js.Id(() => "card").Call(() => "setAttribute", () => Js.Str(() => "data-id"), () => Js.Id(() => "b").Prop(() => "id")).Stmt())
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "innerHTML"), () => BuildCardHtml()))
            .Add(() => Js.Id(() => "grid").Call(() => "appendChild", () => Js.Id(() => "card")).Stmt());

        var thenBody = Js.Block()
            .Add(() => Js.Const(() => "grid", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "beings-grid"))))
            .Add(() => Js.Assign(() => Js.Id(() => "grid").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "total-count")).Prop(() => "textContent"), () => Js.Id(() => "data").Prop(() => "length")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "data").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "grid").Prop(() => "innerHTML"), () => Js.Str(() => $"<div class='empty-state'>{loc.BeingsEmptyState}</div>"))
                    }
                )},
                { (null, new List<JsSyntax>
                    {
                        Js.Id(() => "data").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "b" }, () => forEachBody)).Stmt()
                    }
                )}
            }));

        var loadBeingsBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/beings/list")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => thenBody)).Stmt());

        var selectThenBody = Js.Block()
            .Add(() => Js.Const(() => "statusClass", () => Js.Ternary(() => Js.Id(() => "data").Prop(() => "isIdle"), () => Js.Str(() => "idle"), () => Js.Str(() => "running"))))
            .Add(() => Js.Const(() => "statusText", () => Js.Ternary(() => Js.Id(() => "data").Prop(() => "isIdle"), () => Js.Str(() => loc.BeingsStatusIdle), () => Js.Str(() => loc.BeingsStatusRunning))))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "detail-content")).Prop(() => "innerHTML"), () => BuildDetailHtml(loc)));

        var selectBeingBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "selectedBeingId"), () => Js.Id(() => "id")))
            .Add(() => Js.Id(() => "loadBeings").Invoke().Stmt())
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/beings/detail?id=").Op(() => "+", () => Js.Id(() => "id"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => selectThenBody)).Stmt());

        return Js.Block()
            .Add(() => Js.Let(() => "selectedBeingId", () => Js.Null()))
            .Add(() => Js.Func(() => "loadBeings", () => new List<string>(), () => loadBeingsBody))
            .Add(() => Js.Func(() => "selectBeing", () => new List<string> { "id", "name" }, () => selectBeingBody))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadBeings").Invoke())));
    }

    private static JsSyntax BuildCardHtml()
    {
        var typeBadge = Js.Ternary(
            () => Js.Id(() => "b").Prop(() => "isCustomCompiled"),
            () => Js.Str(() => "<span class='being-type-badge'>").Op(() => "+", () => (JsSyntax)Js.Id(() => "b").Prop(() => "customTypeName")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</span>")),
            () => Js.Str(() => ""));

        return Js.Str(() => "<div class=\"being-name\">")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "b").Prop(() => "name"))
            .Op(() => "+", () => (JsSyntax)typeBadge)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div><span class=\"being-status "))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "statusClass"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "\">"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "statusText"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</span>"));
    }

    private static JsSyntax BuildDetailHtml(DefaultLocalizationBase loc)
    {
        var statusValue = Js.Str(() => "<span class=\"detail-value ")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "statusClass"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "\">"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "statusText"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</span>"));

        var compiledValue = Js.Ternary(
            () => Js.Id(() => "data").Prop(() => "isCustomCompiled"),
            () => Js.Str(() => $"{loc.BeingsYes} (").Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "customTypeName")).Op(() => "+", () => (JsSyntax)Js.Str(() => ")")),
            () => Js.Str(() => loc.BeingsNo));

        var soulHtml = Js.Ternary(
            () => Js.Id(() => "data").Prop(() => "soulContent"),
            () => Js.Str(() => "<div class='soul-content'>").Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "soulContent")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>")),
            () => Js.Str(() => $"<span style='color: var(--text-secondary)'>{loc.BeingsNotSet}</span>"));

        var timerLink = Js.Str(() => "<a class='detail-link' href='/timers?beingId=")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "id"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "'>"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "timerCount"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</a>"));

        var taskLink = Js.Str(() => "<a class='detail-link' href='/tasks?beingId=")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "id"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "'>"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "taskCount"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</a>"));

        return Js.Str(() => "<h2>")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "name"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</h2><div class=\"detail-row\"><span class=\"detail-label\">{loc.BeingsDetailIdLabel}</span><span class=\"detail-value\">"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "id"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</span></div><div class=\"detail-row\"><span class=\"detail-label\">{loc.BeingsDetailStatusLabel}</span>"))
            .Op(() => "+", () => (JsSyntax)statusValue)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</div><div class=\"detail-row\"><span class=\"detail-label\">{loc.BeingsDetailCustomCompileLabel}</span><span class=\"detail-value\">"))
            .Op(() => "+", () => (JsSyntax)compiledValue)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</span></div><div class=\"detail-row\"><span class=\"detail-label\">{loc.BeingsDetailTimersLabel}</span>"))
            .Op(() => "+", () => (JsSyntax)timerLink)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</div><div class=\"detail-row\"><span class=\"detail-label\">{loc.BeingsDetailTasksLabel}</span>"))
            .Op(() => "+", () => (JsSyntax)taskLink)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</div><div class=\"detail-row\"><span class=\"detail-label\">{loc.BeingsDetailSoulContentLabel}</span></div><div class=\"detail-row\">"))
            .Op(() => "+", () => (JsSyntax)soulHtml)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>"));
    }
}
