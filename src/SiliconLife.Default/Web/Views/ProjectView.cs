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

public class ProjectView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as ProjectViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.PageTitleProjects, "projects", vm.Localization, body, GetScripts(vm.Localization), GetStyles(), "projects");
    }

    private static H RenderBody(ProjectViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1(vm.Localization.ProjectsPageHeader),
                H.Div(
                    H.Span($"{vm.Localization.ProjectsActiveLabel}: {vm.ActiveCount}").Class("badge"),
                    H.Span($"{vm.Localization.ProjectsArchivedLabel}: {vm.ArchivedCount}").Class("badge archived")
                ).Class("project-stats")
            ).Class("page-header"),
            H.Div(
                H.Div().Id("projects-list").Class("projects-list")
            ).Class("card")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".project-links")
                .Property("display", "flex")
                .Property("gap", "10px")
                .Property("margin-top", "12px")
                .Property("padding-top", "12px")
                .Property("border-top", "1px solid var(--border)")
            .EndSelector()
            .Selector(".project-link")
                .Property("display", "inline-flex")
                .Property("align-items", "center")
                .Property("gap", "4px")
                .Property("padding", "6px 14px")
                .Property("border-radius", "6px")
                .Property("font-size", "13px")
                .Property("font-weight", "500")
                .Property("text-decoration", "none")
                .Property("transition", "all 0.2s")
            .EndSelector()
            .Selector(".project-link:hover")
                .Property("opacity", "0.85")
                .Property("transform", "translateY(-1px)")
            .EndSelector()
            .Selector(".tasks-link")
                .Property("background", "rgba(59,130,246,0.12)")
                .Property("color", "#3b82f6")
            .EndSelector()
            .Selector(".notes-link")
                .Property("background", "rgba(139,92,246,0.12)")
                .Property("color", "#8b5cf6")
            .EndSelector()
            .Selector(".project-status-badge")
                .Property("display", "inline-flex")
                .Property("align-items", "center")
                .Property("gap", "6px")
                .Property("padding", "4px 10px")
                .Property("border-radius", "12px")
                .Property("font-size", "12px")
                .Property("font-weight", "500")
            .EndSelector()
            .Selector(".project-status-dot")
                .Property("width", "8px")
                .Property("height", "8px")
                .Property("border-radius", "50%")
                .Property("display", "inline-block")
            .EndSelector();
    }

    private static JsSyntax GetScripts(DefaultLocalizationBase loc)
    {
        var renderItemBody = Js.Block()
            .Add(() => Js.Const(() => "item", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "className"), () => Js.Str(() => "project-item")))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "dataset").Prop(() => "id"), () => Js.Id(() => "p").Prop(() => "id")))
            .Add(() => Js.Const(() => "statusColors", () => Js.Obj()
                .Prop(() => "active", () => Js.Str(() => "#22c55e"))
                .Prop(() => "archived", () => Js.Str(() => "#f59e0b"))
                .Prop(() => "destroyed", () => Js.Str(() => "#ef4444"))))
            .Add(() => Js.Const(() => "statusLabels", () => Js.Obj()
                .Prop(() => "active", () => Js.Str(() => loc.ProjectStatusActiveLabel))
                .Prop(() => "archived", () => Js.Str(() => loc.ProjectStatusArchivedLabel))
                .Prop(() => "destroyed", () => Js.Str(() => loc.ProjectStatusDestroyedLabel))))
            .Add(() => Js.Const(() => "statusColor", () => Js.Id(() => "statusColors").Index(() => Js.Id(() => "p").Prop(() => "status")).Op(() => "||", () => Js.Str(() => "#6b7280"))))
            .Add(() => Js.Const(() => "statusLabel", () => Js.Id(() => "statusLabels").Index(() => Js.Id(() => "p").Prop(() => "status")).Op(() => "||", () => Js.Id(() => "p").Prop(() => "status"))))
            .Add(() => Js.Const(() => "statusHtml", () => Js.Str(() => "<span class='project-status-badge' style='background:")
                .Op(() => "+", () => Js.Id(() => "statusColor"))
                .Op(() => "+", () => Js.Str(() => "15;color:"))
                .Op(() => "+", () => Js.Id(() => "statusColor"))
                .Op(() => "+", () => Js.Str(() => "'><span class='project-status-dot' style='background:"))
                .Op(() => "+", () => Js.Id(() => "statusColor"))
                .Op(() => "+", () => Js.Str(() => "'></span>"))
                .Op(() => "+", () => Js.Id(() => "statusLabel"))
                .Op(() => "+", () => Js.Str(() => "</span>"))))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "innerHTML"), () =>
                Js.Str(() => "<div class='project-header'><h3>")
                .Op(() => "+", () => Js.Id(() => "p").Prop(() => "name"))
                .Op(() => "+", () => Js.Str(() => "</h3>"))
                .Op(() => "+", () => Js.Id(() => "statusHtml"))
                .Op(() => "+", () => Js.Str(() => "</div><p class='project-desc'>"))
                .Op(() => "+", () => Js.Id(() => "p").Prop(() => "description"))
                .Op(() => "+", () => Js.Str(() => "</p><div class='project-meta'><span>"))
                .Op(() => "+", () => Js.Id(() => "p").Prop(() => "beingCount"))
                .Op(() => "+", () => Js.Str(() => " beings</span><span>"))
                .Op(() => "+", () => Js.Id(() => "p").Prop(() => "updatedAt"))
                .Op(() => "+", () => Js.Str(() => "</span></div><div class='project-links'><a class='project-link tasks-link' href='/project/"))
                .Op(() => "+", () => Js.Id(() => "p").Prop(() => "id"))
                .Op(() => "+", () => Js.Str(() => "/tasks'>"))
                .Op(() => "+", () => Js.Str(() => loc.ProjectTasksLinkLabel))
                .Op(() => "+", () => Js.Str(() => "</a><a class='project-link notes-link' href='/project/"))
                .Op(() => "+", () => Js.Id(() => "p").Prop(() => "id"))
                .Op(() => "+", () => Js.Str(() => "/work-notes'>"))
                .Op(() => "+", () => Js.Str(() => loc.ProjectWorkNotesLinkLabel))
                .Op(() => "+", () => Js.Str(() => "</a></div>"))
            ))
            .Add(() => Js.Return(() => Js.Id(() => "item")));

        var renderItem = Js.Func(() => "renderProject", () => new List<string> { "p" }, () => renderItemBody);

        var forBody = Js.Block()
            .Add(() => Js.Id(() => "list").Call(() => "appendChild", () => Js.Id(() => "renderProject").Invoke(() => Js.Id(() => "data").Prop(() => "data").Index(() => Js.Id(() => "i")))).Stmt());

        var thenBody = Js.Block()
            .Add(() => Js.Const(() => "list", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "projects-list"))))
            .Add(() => Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "data").Prop(() => "data").Prop(() => "length").Op(() => ">", () => Js.Num(() => "0")), new List<JsSyntax>
                {
                    Js.For(() => Js.Let(() => "i", () => Js.Num(() => "0")), () => Js.Id(() => "i").Op(() => "<", () => Js.Id(() => "data").Prop(() => "data").Prop(() => "length")), () => Js.Assign(() => Js.Id(() => "i"), () => Js.Id(() => "i").Op(() => "+", () => Js.Num(() => "1"))), () => forBody)
                }),
                (null, new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => $"<p class='empty-state'>{loc.ProjectsEmptyState}</p>"))
                })
            }));

        var loadProjectsBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/projects/list")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => thenBody)).Stmt());

        return Js.Block()
            .Add(() => renderItem)
            .Add(() => Js.Func(() => "loadProjects", () => new List<string>(), () => loadProjectsBody))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadProjects").Invoke())));
    }
}
