﻿// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using SiliconLife.App.Web.Models;

using SiliconLife.Common.Localization;

namespace SiliconLife.App.Web.Views;

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
        var loc = vm.Localization;
        return H.Div(
            H.Div(
                H.H1(loc.ProjectsPageHeader),
                H.Div(
                    H.Span($"{loc.ProjectsActiveLabel}: {vm.ActiveCount}").Class("badge"),
                    H.Span($"{loc.ProjectsArchivedLabel}: {vm.ArchivedCount}").Class("badge archived"),
                    H.Button(loc.ProjectCreateButton).Class("btn btn-primary btn-create").Id("btnCreateProject")
                ).Class("project-stats")
            ).Class("page-header"),
            H.Div(
                H.Div().Id("projects-list").Class("projects-list")
            ).Class("card"),
            H.Div(
                H.Div(
                    H.Div(
                        H.H3(loc.ProjectCreateModalTitle),
                        H.Div(
                            H.Label(loc.ProjectCreateNameLabel).Attr("for", "createProjectName"),
                            H.Input().Attr("type", "text").Id("createProjectName").Attr("placeholder", loc.ProjectCreateNameLabel)
                        ).Class("form-group"),
                        H.Div(
                            H.Label(loc.ProjectCreateDescriptionLabel).Attr("for", "createProjectDesc"),
                            H.Textarea().Id("createProjectDesc").Attr("placeholder", loc.ProjectCreateDescriptionLabel).Attr("rows", "3")
                        ).Class("form-group"),
                        H.Div(
                            H.Label(loc.ProjectCreateWorkflowLabel).Attr("for", "createProjectWorkflow"),
                            H.Select(
                                H.Option(loc.ProjectCreateNoWorkflow).Attr("value", "")
                            ).Id("createProjectWorkflow")
                        ).Class("form-group"),
                        H.Div(
                            H.Button(loc.ProjectCreateSubmitButton).Class("btn btn-primary").Id("btnSubmitCreate"),
                            H.Button(loc.ProjectCreateCancelButton).Class("btn btn-secondary").Id("btnCancelCreate")
                        ).Class("form-actions")
                    ).Class("modal-content")
                ).Class("modal-dialog")
            ).Id("createProjectModal").Class("modal")
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
            .Selector(".workflow-names")
                .Property("background", "rgba(16,185,129,0.12)")
                .Property("color", "#10b981")
                .Property("cursor", "default")
            .EndSelector()
            .Selector(".groupchat-link")
                .Property("background", "rgba(245,158,11,0.12)")
                .Property("color", "#f59e0b")
            .EndSelector()
            .Selector(".broadcast-link")
                .Property("background", "rgba(236,72,153,0.12)")
                .Property("color", "#ec4899")
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
            .EndSelector()
            .Selector(".btn-create")
                .Property("margin-left", "auto")
                .Property("font-size", "13px")
                .Property("padding", "6px 16px")
            .EndSelector()
            .Selector(".modal")
                .Property("display", "none")
                .Property("position", "fixed")
                .Property("top", "0")
                .Property("left", "0")
                .Property("width", "100%")
                .Property("height", "100%")
                .Property("background", "rgba(0,0,0,0.5)")
                .Property("z-index", "1000")
                .Property("justify-content", "center")
                .Property("align-items", "center")
            .EndSelector()
            .Selector(".modal.active")
                .Property("display", "flex")
            .EndSelector()
            .Selector(".modal-dialog")
                .Property("width", "100%")
                .Property("max-width", "500px")
                .Property("padding", "20px")
            .EndSelector()
            .Selector(".modal-content")
                .Property("background", "var(--bg-primary)")
                .Property("border-radius", "12px")
                .Property("padding", "24px")
                .Property("box-shadow", "0 20px 60px rgba(0,0,0,0.3)")
            .EndSelector()
            .Selector(".modal-content h3")
                .Property("margin", "0 0 20px 0")
                .Property("font-size", "18px")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".form-group")
                .Property("margin-bottom", "16px")
            .EndSelector()
            .Selector(".form-group label")
                .Property("display", "block")
                .Property("margin-bottom", "6px")
                .Property("font-size", "13px")
                .Property("font-weight", "500")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".form-group input, .form-group textarea, .form-group select")
                .Property("width", "100%")
                .Property("padding", "8px 12px")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "6px")
                .Property("font-size", "14px")
                .Property("background", "var(--bg-secondary)")
                .Property("color", "var(--text-primary)")
                .Property("box-sizing", "border-box")
            .EndSelector()
            .Selector(".form-group textarea")
                .Property("resize", "vertical")
            .EndSelector()
            .Selector(".form-actions")
                .Property("display", "flex")
                .Property("gap", "10px")
                .Property("justify-content", "flex-end")
                .Property("margin-top", "20px")
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
            .Add(() => Js.Const(() => "wfHtml", () => Js.Id(() => "p").Prop(() => "workflowTemplateName").Op(() => "!==", () => Js.Str(() => ""))
                .Op(() => "?", () => Js.Str(() => $"<span class='project-link workflow-names'>{loc.ProjectWorkflowsLinkLabel}: "))
                .Op(() => "+", () => Js.Id(() => "p").Prop(() => "workflowTemplateName"))
                .Op(() => "+", () => Js.Str(() => "</span>"))
                .Op(() => ":", () => Js.Str(() => ""))))
            .Add(() => Js.Const(() => "gcHtml", () => Js.Id(() => "p").Prop(() => "groupChatSessionId")
                .Op(() => "?", () => Js.Str(() => "<a class='project-link groupchat-link' href='/group-chat-history-detail?sessionId="))
                .Op(() => "+", () => Js.Id(() => "p").Prop(() => "groupChatSessionId"))
                .Op(() => "+", () => Js.Str(() => $"'>{loc.ProjectGroupChatLinkLabel}</a>"))
                .Op(() => ":", () => Js.Str(() => ""))))
            .Add(() => Js.Const(() => "bcHtml", () => Js.Id(() => "p").Prop(() => "broadcastChannelId")
                .Op(() => "?", () => Js.Str(() => "<a class='project-link broadcast-link' href='/broadcast-history-detail?sessionId="))
                .Op(() => "+", () => Js.Id(() => "p").Prop(() => "broadcastChannelId"))
                .Op(() => "+", () => Js.Str(() => $"'>{loc.ProjectBroadcastLinkLabel}</a>"))
                .Op(() => ":", () => Js.Str(() => ""))))
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
                .Op(() => "+", () => Js.Str(() => "</a>"))
                .Op(() => "+", () => Js.Id(() => "wfHtml"))
                .Op(() => "+", () => Js.Str(() => "<a class='project-link notes-link' href='/project/"))
                .Op(() => "+", () => Js.Id(() => "p").Prop(() => "id"))
                .Op(() => "+", () => Js.Str(() => "/work-notes'>"))
                .Op(() => "+", () => Js.Str(() => loc.ProjectWorkNotesLinkLabel))
                .Op(() => "+", () => Js.Str(() => "</a>"))
                .Op(() => "+", () => Js.Str(() => "<a class='project-link tool-perms-link' href='/project/"))
                .Op(() => "+", () => Js.Id(() => "p").Prop(() => "id"))
                .Op(() => "+", () => Js.Str(() => "/tool-permissions'>"))
                .Op(() => "+", () => Js.Str(() => loc.ToolAuthPageTitle))
                .Op(() => "+", () => Js.Str(() => "</a>"))
                .Op(() => "+", () => Js.Id(() => "gcHtml"))
                .Op(() => "+", () => Js.Id(() => "bcHtml"))
                .Op(() => "+", () => Js.Str(() => "</div>"))
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

        var loadTemplatesThenBody = Js.Block()
            .Add(() => Js.Const(() => "select", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "createProjectWorkflow"))))
            .Add(() => Js.For(
                () => Js.Let(() => "i", () => Js.Num(() => "0")),
                () => Js.Id(() => "i").Op(() => "<", () => Js.Id(() => "data").Prop(() => "data").Prop(() => "length")),
                () => Js.Assign(() => Js.Id(() => "i"), () => Js.Id(() => "i").Op(() => "+", () => Js.Num(() => "1"))),
                () => Js.Block()
                    .Add(() => Js.Const(() => "opt", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "option"))))
                    .Add(() => Js.Assign(() => Js.Id(() => "opt").Prop(() => "value"), () => Js.Id(() => "data").Prop(() => "data").Index(() => Js.Id(() => "i")).Prop(() => "name")))
                    .Add(() => Js.Assign(() => Js.Id(() => "opt").Prop(() => "textContent"), () => Js.Id(() => "data").Prop(() => "data").Index(() => Js.Id(() => "i")).Prop(() => "name")))
                    .Add(() => Js.Id(() => "select").Call(() => "appendChild", () => Js.Id(() => "opt")).Stmt())
            ));

        var loadTemplatesBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/projects/list-workflow-templates")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => loadTemplatesThenBody)).Stmt());

        var createProjectBody = Js.Block()
            .Add(() => Js.Const(() => "name", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "createProjectName")).Prop(() => "value").Call(() => "trim")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "name").Op(() => "===", () => Js.Str(() => "")), new List<JsSyntax>
                {
                    Js.Id(() => "alert").Invoke(() => Js.Str(() => loc.ProjectCreateNameRequired)).Stmt(),
                    Js.Return(() => Js.Id(() => "undefined"))
                })
            }))
            .Add(() => Js.Const(() => "description", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "createProjectDesc")).Prop(() => "value")))
            .Add(() => Js.Const(() => "workflowTemplate", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "createProjectWorkflow")).Prop(() => "value")))
            .Add(() => Js.Const(() => "reqBody", () => Js.Obj()
                .Prop(() => "name", () => Js.Id(() => "name"))
                .Prop(() => "description", () => Js.Id(() => "description"))
                .Prop(() => "workflowTemplate", () => Js.Id(() => "workflowTemplate"))))
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/projects/create"), () => Js.Obj()
                .Prop(() => "method", () => Js.Str(() => "POST"))
                .Prop(() => "headers", () => Js.Obj().Prop(() => "Content-Type", () => Js.Str(() => "application/json")))
                .Prop(() => "body", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Id(() => "reqBody")))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => Js.Block()
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
                    {
                        Js.Id(() => "alert").Invoke(() => Js.Str(() => loc.ProjectCreateSuccess)).Stmt(),
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "createProjectModal")).Prop(() => "classList").Call(() => "remove", () => Js.Str(() => "active")).Stmt(),
                        Js.Id(() => "loadProjects").Invoke().Stmt()
                    }),
                    (null, new List<JsSyntax>
                    {
                        Js.Id(() => "alert").Invoke(() => Js.Id(() => "result").Prop(() => "error").Op(() => "||", () => Js.Str(() => "Error"))).Stmt()
                    })
                }))
            )).Stmt());

        var openModalBody = Js.Block()
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "createProjectName")).Prop(() => "value").Assign(() => Js.Str(() => "")).Stmt())
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "createProjectDesc")).Prop(() => "value").Assign(() => Js.Str(() => "")).Stmt())
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "createProjectWorkflow")).Prop(() => "selectedIndex").Assign(() => Js.Num(() => "0")).Stmt())
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "createProjectModal")).Prop(() => "classList").Call(() => "add", () => Js.Str(() => "active")).Stmt())
            .Add(() => Js.Id(() => "loadTemplates").Invoke().Stmt());

        return Js.Block()
            .Add(() => renderItem)
            .Add(() => Js.Func(() => "loadProjects", () => new List<string>(), () => loadProjectsBody))
            .Add(() => Js.Func(() => "loadTemplates", () => new List<string>(), () => loadTemplatesBody))
            .Add(() => Js.Func(() => "createProject", () => new List<string>(), () => createProjectBody))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "btnCreateProject")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string>(), () => openModalBody)).Stmt())
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "btnCancelCreate")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "createProjectModal")).Prop(() => "classList").Call(() => "remove", () => Js.Str(() => "active")))).Stmt())
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "btnSubmitCreate")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "createProject").Invoke())).Stmt())
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadProjects").Invoke())));
    }
}
