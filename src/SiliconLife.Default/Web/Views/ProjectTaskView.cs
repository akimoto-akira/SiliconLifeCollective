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

public class ProjectTaskView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as ProjectTaskViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.ProjectTasksPageHeader, "project-tasks", vm.Localization, body, GetScripts(vm.Localization, vm.ProjectId), GetStyles());
    }

    private static H RenderBody(ProjectTaskViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1(vm.Localization.ProjectTasksPageHeader),
                H.Div(
                    H.A(vm.Localization.ProjectTasksBackToProjects).Href("/project").Class("back-link"),
                    H.Span(vm.ProjectName).Class("project-name")
                ).Class("page-subtitle")
            ).Class("page-header"),
            H.Div(
                H.Div().Id("tasks-list").Class("tasks-list")
            ).Class("card")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".page-subtitle")
                .Property("margin-top", "8px")
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "6px")
            .EndSelector()
            .Selector(".project-name")
                .Property("font-weight", "bold")
                .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".tasks-list")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "16px")
                .Property("min-height", "100px")
                .Property("padding", "4px")
            .EndSelector()
            .Selector(".task-item")
                .Property("background", "var(--bg-card, var(--bg-secondary))")
                .Property("border", "1px solid var(--border-color, var(--border))")
                .Property("border-radius", "12px")
                .Property("padding", "18px")
                .Property("transition", "all 0.25s ease")
                .Property("position", "relative")
                .Property("overflow", "hidden")
            .EndSelector()
            .Selector(".task-item:hover")
                .Property("border-color", "var(--primary-color, var(--accent-primary))")
                .Property("box-shadow", "0 4px 20px rgba(0,0,0,0.08)")
                .Property("transform", "translateY(-2px)")
            .EndSelector()
            .Selector(".task-item.running")
                .Property("border-left", "4px solid #3b82f6")
                .Property("background", "linear-gradient(90deg, rgba(59,130,246,0.04) 0%, var(--bg-card) 100%)")
                .Property("box-shadow", "0 0 0 1px rgba(59,130,246,0.15), 0 4px 20px rgba(59,130,246,0.08)")
            .EndSelector()
            .Selector(".task-item.running:hover")
                .Property("box-shadow", "0 0 0 1px rgba(59,130,246,0.25), 0 6px 24px rgba(59,130,246,0.12)")
            .EndSelector()
            .Selector(".task-status-dot")
                .Property("width", "8px")
                .Property("height", "8px")
                .Property("border-radius", "50%")
                .Property("display", "inline-block")
                .Property("margin-right", "8px")
            .EndSelector()
            .Selector(".task-status-dot.running")
                .Property("background", "#3b82f6")
                .Property("animation", "task-pulse 2s infinite")
            .EndSelector()
            .Keyframes("task-pulse", b => b
                .At("0%", p => p.Property("transform", "scale(1)").Property("opacity", "1"))
                .At("50%", p => p.Property("transform", "scale(1.4)").Property("opacity", "0.6"))
                .At("100%", p => p.Property("transform", "scale(1)").Property("opacity", "1")))
            .Selector(".task-header")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("justify-content", "space-between")
                .Property("margin-bottom", "10px")
            .EndSelector()
            .Selector(".task-title-row")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "10px")
            .EndSelector()
            .Selector(".task-header strong")
                .Property("font-size", "17px")
                .Property("color", "var(--text-primary)")
                .Property("font-weight", "600")
            .EndSelector()
            .Selector(".task-desc")
                .Property("color", "var(--text-secondary)")
                .Property("font-size", "14px")
                .Property("line-height", "1.6")
                .Property("margin-bottom", "14px")
            .EndSelector()
            .Selector(".task-meta")
                .Property("display", "flex")
                .Property("gap", "14px")
                .Property("flex-wrap", "wrap")
                .Property("font-size", "12px")
                .Property("color", "var(--text-muted)")
                .Property("align-items", "center")
            .EndSelector()
            .Selector(".meta-item")
                .Property("display", "inline-flex")
                .Property("align-items", "center")
                .Property("gap", "5px")
                .Property("padding", "3px 10px")
                .Property("background", "var(--bg-tertiary, rgba(0,0,0,0.03))")
                .Property("border-radius", "6px")
            .EndSelector()
            .Selector(".priority-badge")
                .Property("padding", "2px 10px")
                .Property("border-radius", "12px")
                .Property("font-size", "11px")
                .Property("font-weight", "bold")
                .Property("color", "white")
            .EndSelector()
            .Selector(".priority-high")
                .Property("background", "#ef4444")
            .EndSelector()
            .Selector(".priority-medium")
                .Property("background", "#f59e0b")
            .EndSelector()
            .Selector(".priority-low")
                .Property("background", "#10b981")
            .EndSelector()
            .Selector(".status-badge")
                .Property("padding", "3px 10px")
                .Property("border-radius", "6px")
                .Property("font-size", "12px")
                .Property("font-weight", "500")
                .Property("color", "white")
                .Property("display", "inline-flex")
                .Property("align-items", "center")
            .EndSelector()
            .Selector(".empty-state")
                .Property("text-align", "center")
                .Property("padding", "60px 20px")
                .Property("color", "var(--text-secondary)")
                .Property("font-size", "15px")
            .EndSelector();
    }

    private static JsSyntax GetScripts(DefaultLocalizationBase loc, Guid projectId)
    {
        var assigneeNameExpr = Js.Ternary(
            () => Js.Id(() => "t").Prop(() => "assigneeNames").Op(() => "&&", () => Js.Id(() => "t").Prop(() => "assigneeNames").Index(() => Js.Id(() => "i"))),
            () => Js.Id(() => "t").Prop(() => "assigneeNames").Index(() => Js.Id(() => "i")),
            () => Js.Id(() => "g").Call(() => "substring", () => Js.Num(() => "0"), () => Js.Num(() => "8")));
        var assigneesHtmlExpr = Js.Ternary(
            () => Js.Id(() => "t").Prop(() => "assigneeGuids").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")),
            () => Js.Str(() => loc.ProjectTasksNoAssigneesLabel),
            () => Js.Id(() => "t").Prop(() => "assigneeGuids").Call(() => "map", () => Js.Arrow(() => new List<string> { "g", "i" }, () =>
                Js.Str(() => "<span title='").Op(() => "+", () => assigneeNameExpr).Op(() => "+", () => Js.Str(() => "（")).Op(() => "+", () => Js.Id(() => "g")).Op(() => "+", () => Js.Str(() => "）'>")).Op(() => "+", () => assigneeNameExpr).Op(() => "+", () => Js.Str(() => "</span>"))
            )).Call(() => "join", () => Js.Str(() => ", ")));
        var creatorNameExpr = Js.Ternary(
            () => Js.Id(() => "t").Prop(() => "createdByName"),
            () => Js.Id(() => "t").Prop(() => "createdByName"),
            () => Js.Id(() => "t").Prop(() => "createdByGuid").Call(() => "substring", () => Js.Num(() => "0"), () => Js.Num(() => "8")));
        var creatorHtmlExpr = Js.Str(() => "<span title='").Op(() => "+", () => creatorNameExpr).Op(() => "+", () => Js.Str(() => "（")).Op(() => "+", () => Js.Id(() => "t").Prop(() => "createdByGuid")).Op(() => "+", () => Js.Str(() => "）'>")).Op(() => "+", () => creatorNameExpr).Op(() => "+", () => Js.Str(() => "</span>"));

        var forEachBody = Js.Block()
            .Add(() => Js.Const(() => "item", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Const(() => "statusClass", () => Js.Id(() => "t").Prop(() => "status")))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "className"), () => Js.Str(() => "task-item ").Op(() => "+", () => Js.Id(() => "statusClass"))))
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
            .Add(() => Js.Const(() => "dotHtml", () => Js.Ternary(
                () => Js.Id(() => "t").Prop(() => "status").Op(() => "===", () => Js.Str(() => "running")),
                () => Js.Str(() => "<span class='task-status-dot running'></span>"),
                () => Js.Str(() => "<span class='task-status-dot' style='background:").Op(() => "+", () => Js.Id(() => "color")).Op(() => "+", () => Js.Str(() => "'></span>")))))
            .Add(() => Js.Const(() => "statusHtml", () =>
                Js.Str(() => "<span class='status-badge' style='background:")
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "color"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => "'>"))
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "dotHtml"))
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "label"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => "</span>"))))
            .Add(() => Js.Const(() => "prioClass", () => Js.Ternary(
                () => Js.Id(() => "t").Prop(() => "priority").Op(() => "<=", () => Js.Num(() => "50")),
                () => Js.Str(() => "priority-high"),
                () => Js.Ternary(
                    () => Js.Id(() => "t").Prop(() => "priority").Op(() => "<=", () => Js.Num(() => "100")),
                    () => Js.Str(() => "priority-medium"),
                    () => Js.Str(() => "priority-low")))))
            .Add(() => Js.Const(() => "prioBadge", () => Js.Str(() => "<span class='priority-badge ").Op(() => "+", () => Js.Id(() => "prioClass")).Op(() => "+", () => Js.Str(() => "'>P")).Op(() => "+", () => Js.Id(() => "t").Prop(() => "priority")).Op(() => "+", () => Js.Str(() => "</span>"))))
            .Add(() => Js.Const(() => "assigneesHtml", () => assigneesHtmlExpr))
            .Add(() => Js.Const(() => "creatorHtml", () => creatorHtmlExpr))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "innerHTML"), () =>
                Js.Str(() => "<div class='task-header'><div class='task-title-row'><strong>")
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "t").Prop(() => "title"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => "</strong>"))
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "prioBadge"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>"))
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "statusHtml"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div><div class='task-desc'>"))
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "t").Prop(() => "description"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div><div class='task-meta'><span class='meta-item'>"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => loc.ProjectTasksAssigneesLabel))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => ": "))
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "assigneesHtml"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => "</span><span class='meta-item'>"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => loc.ProjectTasksCreatedByLabel))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => ": "))
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "creatorHtml"))
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
                { (Js.Id(() => "data").Prop(() => "data").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => $"<div class='empty-state'>{loc.ProjectTasksEmptyState}</div>"))
                    }
                )},
                { (null, new List<JsSyntax>
                    {
                        Js.Id(() => "data").Prop(() => "data").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "t" }, () => forEachBody)).Stmt()
                    }
                )}
            }));

        var loadTasksBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/projects/").Op(() => "+", () => Js.Id(() => "projectId")).Op(() => "+", () => Js.Str(() => "/tasks/list"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => thenBody)).Stmt());

        return Js.Block()
            .Add(() => Js.Const(() => "projectId", () => Js.Str(() => projectId.ToString())))
            .Add(() => Js.Func(() => "loadTasks", () => new List<string>(), () => loadTasksBody))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadTasks").Invoke())));
    }
}
