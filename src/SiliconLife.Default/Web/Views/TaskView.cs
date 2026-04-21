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

public class TaskView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as TaskViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.PageTitleTasks, "tasks", vm.Localization, body, GetScripts(vm.Localization), GetStyles());
    }

    private static H RenderBody(TaskViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1(vm.Localization.TasksPageHeader)
            ).Class("page-header"),
            H.Div(
                H.Div().Id("tasks-list").Class("tasks-list")
            ).Class("card")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".tasks-list")
                .Property("min-height", "100px")
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
            .Add(() => Js.Const(() => "item", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "className"), () => Js.Str(() => "task-item")))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "innerHTML"), () => Js.Str(() => "<strong>").Op(() => "+", () => (JsSyntax)Js.Id(() => "t").Prop(() => "name")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</strong><p>").Op(() => "+", () => (JsSyntax)Js.Id(() => "t").Prop(() => "description")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</p>")))))
            .Add(() => Js.Id(() => "list").Call(() => "appendChild", () => Js.Id(() => "item")).Stmt());

        var thenBody = Js.Block()
            .Add(() => Js.Const(() => "list", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "tasks-list"))))
            .Add(() => Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "data").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => $"<div class='empty-state'>{loc.TasksEmptyState}</div>"))
                    }
                )},
                { (null, new List<JsSyntax>
                    {
                        Js.Id(() => "data").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "t" }, () => forEachBody)).Stmt()
                    }
                )}
            }));

        var loadTasksBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/tasks/list")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => thenBody)).Stmt());

        return Js.Block()
            .Add(() => Js.Func(() => "loadTasks", () => new List<string>(), () => loadTasksBody))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadTasks").Invoke())));
    }
}
