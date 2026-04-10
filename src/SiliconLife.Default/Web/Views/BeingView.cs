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
        return RenderPage(vm.Skin, vm.Localization.PageTitleBeings, "beings", vm.Localization, body, GetScripts(), GetStyles());
    }

    private static H RenderBody(BeingViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1("硅基人管理")
            ).Class("page-header"),
            H.Div().Id("beings-grid").Class("beings-grid"),
            H.Div(
                H.Div(
                    H.P("选择一个硅基人查看详情")
                ).Id("detail-content").Class("detail-content")
            ).Id("detail-panel").Class("detail-panel")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
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
                .Property("margin-bottom", "10px")
            .EndSelector()
            .Selector(".detail-label")
                .Property("font-weight", "bold")
                .Property("color", "var(--text-secondary)")
                .Property("width", "120px")
            .EndSelector()
            .Selector(".detail-value")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".action-buttons")
                .Property("margin-top", "20px")
                .Property("display", "flex")
                .Property("gap", "10px")
            .EndSelector()
            .Selector(".action-btn")
                .Property("padding", "10px 20px")
                .Property("border", "none")
                .Property("border-radius", "6px")
                .Property("cursor", "pointer")
                .Property("font-size", "14px")
                .Property("color", "#fff")
            .EndSelector()
            .Selector(".action-btn.pause")
                .Property("background", "var(--accent-warning)")
            .EndSelector()
            .Selector(".action-btn.resume")
                .Property("background", "var(--accent-success)")
            .EndSelector();
    }

    private static JsSyntax GetScripts()
    {
        var forEachBody = Js.Block()
            .Add(() => Js.Const(() => "card", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "className"), () => Js.Str(() => "being-card")))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "onclick"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "selectBeing").Invoke(() => Js.Id(() => "b").Prop(() => "id"), () => Js.Id(() => "b").Prop(() => "name")))))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "innerHTML"), () => Js.Str(() => "'<div class=\"being-name\">' + b.name + '</div>'")))
            .Add(() => Js.Id(() => "grid").Call(() => "appendChild", () => Js.Id(() => "card")).Stmt());

        var thenBody = Js.Block()
            .Add(() => Js.Const(() => "grid", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "beings-grid"))))
            .Add(() => Js.Assign(() => Js.Id(() => "grid").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.Id(() => "data").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "b" }, () => forEachBody)).Stmt());

        var loadBeingsBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/beings/list")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => thenBody)).Stmt());

        var selectThenBody = Js.Block()
            .Add(() => Js.Const(() => "content", () => Js.Str(() => "'<h2>' + data.name + '</h2>'")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "detail-content")).Prop(() => "innerHTML"), () => Js.Id(() => "content")));

        var selectBeingBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "selectedBeingId"), () => Js.Id(() => "id")))
            .Add(() => Js.Id(() => "loadBeings").Invoke().Stmt())
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/beings/detail?id=").Op(() => "+", () => (JsSyntax)Js.Id(() => "id"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => selectThenBody)).Stmt());

        return Js.Block()
            .Add(() => Js.Let(() => "selectedBeingId", () => Js.Id(() => "null")))
            .Add(() => Js.Func(() => "loadBeings", () => new List<string>(), () => loadBeingsBody))
            .Add(() => Js.Func(() => "selectBeing", () => new List<string> { "id", "name" }, () => selectBeingBody))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadBeings").Invoke())));
    }
}
