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
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "12px")
                .Property("min-height", "100px")
            .EndSelector()
            .Selector(".task-item")
                .Property("background", "var(--bg-secondary)")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "8px")
                .Property("padding", "16px")
                .Property("transition", "border-color 0.2s")
            .EndSelector()
            .Selector(".task-item:hover")
                .Property("border-color", "var(--primary-color)")
            .EndSelector()
            .Selector(".task-header")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("justify-content", "space-between")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".task-header strong")
                .Property("font-size", "16px")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".task-desc")
                .Property("color", "var(--text-secondary)")
                .Property("font-size", "14px")
                .Property("line-height", "1.5")
                .Property("margin-bottom", "12px")
            .EndSelector()
            .Selector(".task-meta")
                .Property("display", "flex")
                .Property("gap", "16px")
                .Property("flex-wrap", "wrap")
                .Property("font-size", "12px")
                .Property("color", "var(--text-muted)")
            .EndSelector()
            .Selector(".meta-item")
                .Property("display", "inline-flex")
                .Property("align-items", "center")
                .Property("gap", "4px")
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
            .Add(() => Js.Const(() => "statusColors", () => Js.Obj()
                .Prop(() => "pending", () => Js.Str(() => "#f59e0b"))
                .Prop(() => "running", () => Js.Str(() => "#3b82f6"))
                .Prop(() => "completed", () => Js.Str(() => "#10b981"))
                .Prop(() => "failed", () => Js.Str(() => "#ef4444"))
                .Prop(() => "cancelled", () => Js.Str(() => "#6b7280"))))
            .Add(() => Js.Const(() => "statusLabels", () => Js.Obj()
                .Prop(() => "pending", () => Js.Str(() => loc.TasksStatusPending))
                .Prop(() => "running", () => Js.Str(() => loc.TasksStatusRunning))
                .Prop(() => "completed", () => Js.Str(() => loc.TasksStatusCompleted))
                .Prop(() => "failed", () => Js.Str(() => loc.TasksStatusFailed))
                .Prop(() => "cancelled", () => Js.Str(() => loc.TasksStatusCancelled))))
            .Add(() => Js.Const(() => "color", () => Js.Id(() => "statusColors").Index(() => Js.Id(() => "t").Prop(() => "status"))))
            .Add(() => Js.Const(() => "label", () => Js.Id(() => "statusLabels").Index(() => Js.Id(() => "t").Prop(() => "status"))))
            .Add(() => Js.Const(() => "statusHtml", () =>
                Js.Str(() => "<span style='background:")
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "color"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => ";padding:2px 8px;border-radius:4px;font-size:12px;color:white;'>"))
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "label"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => "</span>"))))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "innerHTML"), () =>
                Js.Str(() => "<div class='task-header'><strong>")
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "t").Prop(() => "name"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => "</strong>"))
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "statusHtml"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div><div class='task-desc'>"))
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "t").Prop(() => "description"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div><div class='task-meta'><span class='meta-item'>"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => loc.TasksPriorityLabel))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => ": "))
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "t").Prop(() => "priority"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => "</span><span class='meta-item'>"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => loc.TasksAssignedToLabel))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => ": "))
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "t").Prop(() => "assignedTo"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => "</span><span class='meta-item'>"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => loc.TasksCreatedAtLabel))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => ": "))
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "t").Prop(() => "createdAtFormatted"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => "</span></div>"))))
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
