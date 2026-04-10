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

public class CodeBrowserView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as CodeBrowserViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.PageTitleCodeBrowser, "beings", vm.Localization, body, GetScripts(), GetStyles());
    }

    private static H RenderBody(CodeBrowserViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1("代码浏览")
            ).Class("page-header"),
            H.Div(
                H.Div().Id("types-list").Class("types-list"),
                H.Div().Id("type-detail").Class("type-detail")
            ).Class("code-browser")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".code-browser")
                .Property("display", "flex")
                .Property("gap", "20px")
            .EndSelector()
            .Selector(".types-list")
                .Property("width", "300px")
                .Property("background", "var(--bg-card)")
                .Property("padding", "20px")
                .Property("border-radius", "12px")
                .Property("border", "1px solid var(--border)")
                .Property("overflow-y", "auto")
            .EndSelector()
            .Selector(".type-item")
                .Property("padding", "10px")
                .Property("cursor", "pointer")
                .Property("border-radius", "6px")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".type-item:hover")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.05))")
            .EndSelector()
            .Selector(".type-detail")
                .Property("flex", "1")
                .Property("background", "var(--bg-card)")
                .Property("padding", "20px")
                .Property("border-radius", "12px")
                .Property("border", "1px solid var(--border)")
                .Property("overflow-y", "auto")
            .EndSelector()
            .Selector(".type-name")
                .Property("font-size", "20px")
                .Property("font-weight", "bold")
                .Property("color", "var(--text-primary)")
                .Property("margin-bottom", "15px")
            .EndSelector()
            .Selector(".type-section")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".type-section h3")
                .Property("font-size", "16px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-bottom", "10px")
            .EndSelector()
            .Selector(".member-item")
                .Property("padding", "8px 0")
                .Property("border-bottom", "1px solid var(--border)")
                .Property("color", "var(--text-primary)")
            .EndSelector();
    }

    private static JsSyntax GetScripts()
    {
        var forEachBody = Js.Block()
            .Add(() => Js.Const(() => "item", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "className"), () => Js.Str(() => "type-item")))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "textContent"), () => Js.Id(() => "t").Prop(() => "name")))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "onclick"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "showType").Invoke(() => Js.Id(() => "t").Prop(() => "fullName")))))
            .Add(() => Js.Id(() => "list").Call(() => "appendChild", () => Js.Id(() => "item")).Stmt());

        var loadTypesThenBody = Js.Block()
            .Add(() => Js.Const(() => "list", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "types-list"))))
            .Add(() => Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.Id(() => "data").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "t" }, () => forEachBody)).Stmt());

        var loadTypesBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/code/types")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => loadTypesThenBody)).Stmt());

        var showTypeThenBody = Js.Block()
            .Add(() => Js.Const(() => "html", () => Js.Str(() => "'<div class=\"type-name\">' + data.name + '</div>'")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "type-detail")).Prop(() => "innerHTML"), () => Js.Id(() => "html")));

        var showTypeBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/code/detail?name=").Op(() => "+", () => (JsSyntax)Js.Id(() => "encodeURIComponent").Invoke(() => Js.Id(() => "fullName")))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => showTypeThenBody)).Stmt());

        return Js.Block()
            .Add(() => Js.Func(() => "loadTypes", () => new List<string>(), () => loadTypesBody))
            .Add(() => Js.Func(() => "showType", () => new List<string> { "fullName" }, () => showTypeBody))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadTypes").Invoke())));
    }
}
