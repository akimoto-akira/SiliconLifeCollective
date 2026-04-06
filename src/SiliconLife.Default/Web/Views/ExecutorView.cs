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

public class ExecutorView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as ViewModelBase;
        if (vm == null) return string.Empty;
        var body = RenderBody();
        return RenderPage(vm.Skin, "执行器监控 - Silicon Life Collective", "beings", body, GetScripts(), GetStyles());
    }

    private static H RenderBody()
    {
        return H.Div(
            H.Div(
                H.H1("执行器监控")
            ).Class("page-header"),
            H.Div().Id("executors-grid").Class("executors-grid")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".executors-grid")
                .Property("display", "grid")
                .Property("grid-template-columns", "repeat(auto-fill, minmax(280px, 1fr))")
                .Property("gap", "20px")
            .EndSelector()
            .Selector(".executor-card")
                .Property("background", "var(--bg-card)")
                .Property("padding", "20px")
                .Property("border-radius", "12px")
                .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".executor-name")
                .Property("font-size", "18px")
                .Property("font-weight", "bold")
                .Property("color", "var(--text-primary)")
                .Property("margin-bottom", "10px")
            .EndSelector()
            .Selector(".executor-status")
                .Property("display", "inline-block")
                .Property("padding", "4px 12px")
                .Property("border-radius", "12px")
                .Property("font-size", "12px")
                .Property("background", "rgba(107,203,119,0.15)")
                .Property("color", "var(--accent-success)")
            .EndSelector()
            .Selector(".executor-queue")
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-top", "8px")
            .EndSelector();
    }

    private static JsSyntax GetScripts()
    {
        var forEachBody = Js.Block()
            .Add(() => Js.Const(() => "card", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "className"), () => Js.Str(() => "executor-card")))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "innerHTML"), () => Js.Str(() => "'<div class=\"executor-name\">' + e.name + '</div>'")))
            .Add(() => Js.Id(() => "grid").Call(() => "appendChild", () => Js.Id(() => "card")).Stmt());

        var thenBody = Js.Block()
            .Add(() => Js.Const(() => "grid", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "executors-grid"))))
            .Add(() => Js.Assign(() => Js.Id(() => "grid").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.Id(() => "data").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "e" }, () => forEachBody)).Stmt());

        var loadExecutorsBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/executors/status")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => thenBody)).Stmt());

        return Js.Block()
            .Add(() => Js.Func(() => "loadExecutors", () => new List<string>(), () => loadExecutorsBody))
            .Add(() => Js.Id(() => "setInterval").Invoke(() => Js.Id(() => "loadExecutors"), () => Js.Num(() => "5000")).Stmt())
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadExecutors").Invoke())));
    }
}
