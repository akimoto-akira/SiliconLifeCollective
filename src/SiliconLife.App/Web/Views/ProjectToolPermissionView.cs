// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using SiliconLife.App.Web.Models;
using SiliconLife.App.Web;
using SiliconLife.Common.Localization;

namespace SiliconLife.App.Web.Views;

/// <summary>
/// View for project-level tool action permission configuration.
/// Shows the full tool/action matrix for project-level settings.
/// Project permissions are a single unified config — at runtime,
/// effective permissions = BeingGlobalAllowed ∩ ProjectAllowed.
/// </summary>
public class ProjectToolPermissionView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as ProjectToolPermissionViewModel;
        if (vm == null) return string.Empty;

        var body = RenderBody(vm);
        return RenderPage(vm.Skin, $"{vm.ProjectName} - {vm.Localization.ToolAuthPageTitle}", "projects",
            vm.Localization, body, GetScripts(vm), GetStyles(), helpTopicId: "projects");
    }

    private static H RenderBody(ProjectToolPermissionViewModel vm)
    {
        var loc = vm.Localization;

        return H.Div(
            H.Div(
                H.A("← " + (loc.ProjectsPageHeader ?? "Projects")).Href("/project").Class("tool-auth-back-link"),
                H.H1(vm.ProjectName).Class("tool-auth-title"),
                H.P(loc.ToolAuthPageHeader).Class("tool-auth-subtitle")
            ).Class("tool-auth-header"),
            H.Div().Id("alert").Class("alert"),
            H.Div().Id("permission-matrix").Class("permission-matrix"),
            H.Div(
                H.Create("button", loc.ToolAuthSaveButton).Class("btn btn-primary")
                    .Attr("type", "button").Attr("onclick", "savePermissions()")
            ).Class("form-actions")
        ).Class("page-content");
    }

    private static JsSyntax GetScripts(ProjectToolPermissionViewModel vm)
    {
        var loc = vm.Localization;
        var js = Js.Block();

        js.Add(() => Js.Const(() => "projectId", () => Js.Str(() => vm.ProjectId.ToString())));
        js.Add(() => Js.Let(() => "permissionData", () => Js.Null()));

        // loadPermissions
        var loadDataBlock = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "data").Prop(() => "success"), new List<JsSyntax>
                    { Js.Assign(() => Js.Id(() => "permissionData"), () => Js.Id(() => "data")), Js.Id(() => "renderMatrix").Invoke().Stmt() }
                )},
                { (null, new List<JsSyntax>
                    { Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "Failed:"), () => Js.Id(() => "data").Prop(() => "error")).Stmt() }
                )}
            }));
        var loadBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/projects/").Op(() => "+", () => (JsSyntax)Js.Id(() => "projectId")).Op(() => "+", () => (JsSyntax)Js.Str(() => "/tool-permissions")))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json")))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => loadDataBlock)).Stmt());
        js.Add(() => Js.Func(() => "loadPermissions", () => new List<string>(), () => loadBody));

        BuildRenderMatrix(js, loc);
        BuildOnActionToggle(js);
        BuildToggleToolActions(js);
        BuildUpdateToolGroupStatus(js, loc);
        BuildSavePermissions(js, loc);
        BuildShowAlert(js);

        js.Add(() => Js.Id(() => "window").Prop(() => "addEventListener").Invoke(
            () => Js.Str(() => "load"),
            () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadPermissions").Invoke())));

        return js;
    }

    private static void BuildRenderMatrix(JsBlock js, DefaultLocalizationBase loc)
    {
        var forEachActionBody = Js.Block()
            .Add(() => Js.Const(() => "item", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "className"), () => Js.Ternary(() => Js.Id(() => "action").Prop(() => "enabled"), () => Js.Str(() => "tool-action-item"), () => Js.Str(() => "tool-action-item disabled"))))
            .Add(() => Js.Id(() => "item").Call(() => "setAttribute", () => Js.Str(() => "data-tool"), () => Js.Id(() => "tool").Prop(() => "toolName")).Stmt())
            .Add(() => Js.Id(() => "item").Call(() => "setAttribute", () => Js.Str(() => "data-action"), () => Js.Id(() => "action").Prop(() => "name")).Stmt())
            .Add(() => Js.Const(() => "cb", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "input"))))
            .Add(() => Js.Assign(() => Js.Id(() => "cb").Prop(() => "type"), () => Js.Str(() => "checkbox")))
            .Add(() => Js.Assign(() => Js.Id(() => "cb").Prop(() => "checked"), () => Js.Id(() => "action").Prop(() => "enabled")))
            .Add(() => Js.Assign(() => Js.Id(() => "cb").Prop(() => "onchange"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "onActionToggle").Invoke(() => Js.Id(() => "tool").Prop(() => "toolName"), () => Js.Id(() => "action").Prop(() => "name"), () => Js.Id(() => "cb").Prop(() => "checked")))))
            .Add(() => Js.Id(() => "item").Call(() => "appendChild", () => Js.Id(() => "cb")).Stmt())
            .Add(() => Js.Const(() => "lbl", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "span"))))
            .Add(() => Js.Assign(() => Js.Id(() => "lbl").Prop(() => "className"), () => Js.Ternary(() => Js.Id(() => "action").Prop(() => "enabled"), () => Js.Str(() => "tool-action-name"), () => Js.Str(() => "tool-action-name is-disabled"))))
            .Add(() => Js.Assign(() => Js.Id(() => "lbl").Prop(() => "textContent"), () => Js.Id(() => "action").Prop(() => "name")))
            .Add(() => Js.Id(() => "item").Call(() => "appendChild", () => Js.Id(() => "lbl")).Stmt())
            .Add(() => Js.Id(() => "actionList").Call(() => "appendChild", () => Js.Id(() => "item")).Stmt());

        var forEachToolBody = Js.Block()
            .Add(() => Js.Const(() => "group", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Const(() => "disabledCount", () => Js.Id(() => "tool").Prop(() => "actions").Call(() => "filter", () => Js.Arrow(() => new List<string> { "a" }, () => Js.Id(() => "a").Prop(() => "enabled").Op(() => "===", () => Js.Bool(() => false)))).Prop(() => "length")))
            .Add(() => Js.Assign(() => Js.Id(() => "group").Prop(() => "className"), () => Js.Str(() => "tool-group").Op(() => "+", () => (JsSyntax)Js.Ternary(() => Js.Id(() => "disabledCount").Op(() => ">", () => Js.Num(() => "0")), () => Js.Str(() => " has-restrictions"), () => Js.Str(() => "")))))
            .Add(() => Js.Id(() => "group").Call(() => "setAttribute", () => Js.Str(() => "data-tool"), () => Js.Id(() => "tool").Prop(() => "toolName")).Stmt())
            .Add(() => Js.Const(() => "header", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "header").Prop(() => "className"), () => Js.Str(() => "tool-group-header")))
            .Add(() => Js.Const(() => "nameSpan", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "span"))))
            .Add(() => Js.Assign(() => Js.Id(() => "nameSpan").Prop(() => "className"), () => Js.Str(() => "tool-group-name")))
            .Add(() => Js.Assign(() => Js.Id(() => "nameSpan").Prop(() => "textContent"), () => Js.Id(() => "tool").Prop(() => "displayName")))
            .Add(() => Js.Id(() => "header").Call(() => "appendChild", () => Js.Id(() => "nameSpan")).Stmt())
            .Add(() => Js.Const(() => "idSpan", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "span"))))
            .Add(() => Js.Assign(() => Js.Id(() => "idSpan").Prop(() => "className"), () => Js.Str(() => "tool-group-id")))
            .Add(() => Js.Assign(() => Js.Id(() => "idSpan").Prop(() => "textContent"), () => Js.Id(() => "tool").Prop(() => "toolName")))
            .Add(() => Js.Id(() => "header").Call(() => "appendChild", () => Js.Id(() => "idSpan")).Stmt())
            .Add(() => Js.Const(() => "actionsDiv", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "actionsDiv").Prop(() => "className"), () => Js.Str(() => "tool-group-actions")))
            .Add(() => Js.Const(() => "statusSpan", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "span"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "disabledCount").Op(() => ">", () => Js.Num(() => "0")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "statusSpan").Prop(() => "className"), () => Js.Str(() => "tool-group-status has-restrictions")),
                        Js.Assign(() => Js.Id(() => "statusSpan").Prop(() => "textContent"), () => Js.Str(() => loc.ToolAuthHasRestrictions + " (").Op(() => "+", () => (JsSyntax)Js.Id(() => "disabledCount")).Op(() => "+", () => (JsSyntax)Js.Str(() => ")"))),
                        Js.Id(() => "group").Prop(() => "classList").Call(() => "add", () => Js.Str(() => "has-restrictions")).Stmt()
                    }
                )},
                { (null, new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "statusSpan").Prop(() => "className"), () => Js.Str(() => "tool-group-status no-restrictions")),
                        Js.Assign(() => Js.Id(() => "statusSpan").Prop(() => "textContent"), () => Js.Str(() => loc.ToolAuthNoRestrictions))
                    }
                )}
            }))
            .Add(() => Js.Id(() => "actionsDiv").Call(() => "appendChild", () => Js.Id(() => "statusSpan")).Stmt())
            .Add(() => Js.Const(() => "selectAllBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Assign(() => Js.Id(() => "selectAllBtn").Prop(() => "className"), () => Js.Str(() => "btn-sm")))
            .Add(() => Js.Assign(() => Js.Id(() => "selectAllBtn").Prop(() => "textContent"), () => Js.Str(() => loc.ToolAuthSelectAll)))
            .Add(() => Js.Assign(() => Js.Id(() => "selectAllBtn").Prop(() => "onclick"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "toggleToolActions").Invoke(() => Js.Id(() => "tool").Prop(() => "toolName"), () => Js.Bool(() => true)))))
            .Add(() => Js.Id(() => "actionsDiv").Call(() => "appendChild", () => Js.Id(() => "selectAllBtn")).Stmt())
            .Add(() => Js.Const(() => "deselectAllBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Assign(() => Js.Id(() => "deselectAllBtn").Prop(() => "className"), () => Js.Str(() => "btn-sm")))
            .Add(() => Js.Assign(() => Js.Id(() => "deselectAllBtn").Prop(() => "textContent"), () => Js.Str(() => loc.ToolAuthDeselectAll)))
            .Add(() => Js.Assign(() => Js.Id(() => "deselectAllBtn").Prop(() => "onclick"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "toggleToolActions").Invoke(() => Js.Id(() => "tool").Prop(() => "toolName"), () => Js.Bool(() => false)))))
            .Add(() => Js.Id(() => "actionsDiv").Call(() => "appendChild", () => Js.Id(() => "deselectAllBtn")).Stmt())
            .Add(() => Js.Id(() => "header").Call(() => "appendChild", () => Js.Id(() => "actionsDiv")).Stmt())
            .Add(() => Js.Id(() => "group").Call(() => "appendChild", () => Js.Id(() => "header")).Stmt())
            .Add(() => Js.Const(() => "actionList", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "actionList").Prop(() => "className"), () => Js.Str(() => "tool-action-list")))
            .Add(() => Js.Id(() => "tool").Prop(() => "actions").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "action" }, () => forEachActionBody)).Stmt())
            .Add(() => Js.Id(() => "group").Call(() => "appendChild", () => Js.Id(() => "actionList")).Stmt())
            .Add(() => Js.Id(() => "container").Call(() => "appendChild", () => Js.Id(() => "group")).Stmt());

        var renderBody = Js.Block()
            .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "permission-matrix"))))
            .Add(() => Js.Assign(() => Js.Id(() => "container").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.Id(() => "permissionData").Prop(() => "permissions").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "tool" }, () => forEachToolBody)).Stmt());
        js.Add(() => Js.Func(() => "renderMatrix", () => new List<string>(), () => renderBody));
    }

    private static void BuildOnActionToggle(JsBlock js)
    {
        var onToggleBody = Js.Block()
            .Add(() => Js.Const(() => "item", () => Js.Id(() => "document").Call(() => "querySelector",
                () => Js.Str(() => ".tool-action-item[data-tool='").Op(() => "+", () => (JsSyntax)Js.Id(() => "toolName")).Op(() => "+", () => (JsSyntax)Js.Str(() => "'][data-action='")).Op(() => "+", () => (JsSyntax)Js.Id(() => "actionName")).Op(() => "+", () => (JsSyntax)Js.Str(() => "']")))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "item"), new List<JsSyntax>
                    {
                        Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            { (Js.Id(() => "enabled"), new List<JsSyntax>
                                {
                                    Js.Id(() => "item").Prop(() => "classList").Call(() => "remove", () => Js.Str(() => "disabled")).Stmt(),
                                    Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => ".tool-action-name")).Prop(() => "classList").Call(() => "remove", () => Js.Str(() => "is-disabled")).Stmt()
                                }
                            )},
                            { (null, new List<JsSyntax>
                                {
                                    Js.Id(() => "item").Prop(() => "classList").Call(() => "add", () => Js.Str(() => "disabled")).Stmt(),
                                    Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => ".tool-action-name")).Prop(() => "classList").Call(() => "add", () => Js.Str(() => "is-disabled")).Stmt()
                                }
                            )}
                        }),
                        Js.Id(() => "updateToolGroupStatus").Invoke(() => Js.Id(() => "toolName")).Stmt()
                    }
                )}
            }));
        js.Add(() => Js.Func(() => "onActionToggle", () => new List<string> { "toolName", "actionName", "enabled" }, () => onToggleBody));
    }

    private static void BuildToggleToolActions(JsBlock js)
    {
        var toggleToolBody = Js.Block()
            .Add(() => Js.Const(() => "checkboxes", () => Js.Id(() => "document").Call(() => "querySelectorAll",
                () => Js.Str(() => ".tool-action-item[data-tool='").Op(() => "+", () => (JsSyntax)Js.Id(() => "toolName")).Op(() => "+", () => (JsSyntax)Js.Str(() => "'] input[type='checkbox']")))))
            .Add(() => Js.Id(() => "checkboxes").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "cb" }, () => Js.Block()
                .Add(() => Js.Assign(() => Js.Id(() => "cb").Prop(() => "checked"), () => Js.Id(() => "enabled")))
                .Add(() => Js.Id(() => "onActionToggle").Invoke(() => Js.Id(() => "toolName"), () => Js.Id(() => "cb").Prop(() => "parentElement").Call(() => "getAttribute", () => Js.Str(() => "data-action")), () => Js.Id(() => "enabled")).Stmt())
            )).Stmt());
        js.Add(() => Js.Func(() => "toggleToolActions", () => new List<string> { "toolName", "enabled" }, () => toggleToolBody));
    }

    private static void BuildUpdateToolGroupStatus(JsBlock js, DefaultLocalizationBase loc)
    {
        var updateStatusBody = Js.Block()
            .Add(() => Js.Const(() => "group", () => Js.Id(() => "document").Call(() => "querySelector",
                () => Js.Str(() => ".tool-group[data-tool='").Op(() => "+", () => (JsSyntax)Js.Id(() => "toolName")).Op(() => "+", () => (JsSyntax)Js.Str(() => "']")))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "group"), new List<JsSyntax>
                    {
                        Js.Const(() => "total", () => Js.Id(() => "group").Call(() => "querySelectorAll", () => Js.Str(() => "input[type='checkbox']")).Prop(() => "length")),
                        Js.Const(() => "checked", () => Js.Id(() => "group").Call(() => "querySelectorAll", () => Js.Str(() => "input[type='checkbox']:checked")).Prop(() => "length")),
                        Js.Const(() => "disabledCount", () => Js.Id(() => "total").Op(() => "-", () => (JsSyntax)Js.Id(() => "checked"))),
                        Js.Const(() => "statusSpan", () => Js.Id(() => "group").Call(() => "querySelector", () => Js.Str(() => ".tool-group-status"))),
                        Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            { (Js.Id(() => "disabledCount").Op(() => ">", () => Js.Num(() => "0")), new List<JsSyntax>
                                {
                                    Js.Assign(() => Js.Id(() => "statusSpan").Prop(() => "className"), () => Js.Str(() => "tool-group-status has-restrictions")),
                                    Js.Assign(() => Js.Id(() => "statusSpan").Prop(() => "textContent"), () => Js.Str(() => loc.ToolAuthHasRestrictions + " (").Op(() => "+", () => (JsSyntax)Js.Id(() => "disabledCount")).Op(() => "+", () => (JsSyntax)Js.Str(() => ")"))),
                                    Js.Id(() => "group").Prop(() => "classList").Call(() => "add", () => Js.Str(() => "has-restrictions")).Stmt()
                                }
                            )},
                            { (null, new List<JsSyntax>
                                {
                                    Js.Assign(() => Js.Id(() => "statusSpan").Prop(() => "className"), () => Js.Str(() => "tool-group-status no-restrictions")),
                                    Js.Assign(() => Js.Id(() => "statusSpan").Prop(() => "textContent"), () => Js.Str(() => loc.ToolAuthNoRestrictions)),
                                    Js.Id(() => "group").Prop(() => "classList").Call(() => "remove", () => Js.Str(() => "has-restrictions")).Stmt()
                                }
                            )}
                        })
                    }
                )}
            }));
        js.Add(() => Js.Func(() => "updateToolGroupStatus", () => new List<string> { "toolName" }, () => updateStatusBody));
    }

    private static void BuildSavePermissions(JsBlock js, DefaultLocalizationBase loc)
    {
        var saveForEachBody = Js.Block()
            .Add(() => Js.Const(() => "cb", () => Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => "input[type='checkbox']"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "cb").Prop(() => "checked").Op(() => "===", () => Js.Bool(() => false)), new List<JsSyntax>
                    {
                        Js.Const(() => "tool", () => Js.Id(() => "item").Call(() => "getAttribute", () => Js.Str(() => "data-tool"))),
                        Js.Const(() => "action", () => Js.Id(() => "item").Call(() => "getAttribute", () => Js.Str(() => "data-action"))),
                        Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            { (Js.Id(() => "disabledActions").Index(() => Js.Id(() => "tool")).Op(() => "===", () => Js.Id(() => "undefined")), new List<JsSyntax>
                                { Js.Assign(() => Js.Id(() => "disabledActions").Index(() => Js.Id(() => "tool")), () => Js.Obj()) }
                            )}
                        }),
                        Js.Id(() => "disabledActions").Index(() => Js.Id(() => "tool")).Call(() => "push", () => Js.Id(() => "action")).Stmt()
                    }
                )}
            }));

        var saveDataBlock = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "data").Prop(() => "success"), new List<JsSyntax>
                    { Js.Id(() => "showAlert").Invoke(() => Js.Str(() => loc.ToolAuthSaveButton + " ✓"), () => Js.Str(() => "success")).Stmt() }
                )},
                { (null, new List<JsSyntax>
                    { Js.Id(() => "showAlert").Invoke(() => Js.Id(() => "data").Prop(() => "error").Op(() => "||", () => (JsSyntax)Js.Str(() => "Error")), () => Js.Str(() => "error")).Stmt() }
                )}
            }));

        var saveBody = Js.Block()
            .Add(() => Js.Const(() => "disabledActions", () => Js.Obj()))
            .Add(() => Js.Id(() => "document").Call(() => "querySelectorAll", () => Js.Str(() => ".tool-action-item")).Call(() => "forEach", () => Js.Arrow(() => new List<string> { "item" }, () => saveForEachBody)).Stmt())
            .Add(() => Js.Const(() => "payload", () => Js.Obj()
                .Prop(() => "disabledActions", () => Js.Id(() => "disabledActions"))))
            .Add(() => Js.Id(() => "fetch").Invoke(
                () => Js.Str(() => "/api/projects/").Op(() => "+", () => (JsSyntax)Js.Id(() => "projectId")).Op(() => "+", () => (JsSyntax)Js.Str(() => "/tool-permissions")),
                () => Js.Obj()
                    .Prop(() => "method", () => Js.Str(() => "PUT"))
                    .Prop(() => "headers", () => Js.Obj().Prop(() => "Content-Type", () => Js.Str(() => "application/json")))
                    .Prop(() => "body", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Id(() => "payload")))
            ).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json")))
             .Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => saveDataBlock)).Stmt());
        js.Add(() => Js.Func(() => "savePermissions", () => new List<string>(), () => saveBody));
    }

    private static void BuildShowAlert(JsBlock js)
    {
        var showAlertBody = Js.Block()
            .Add(() => Js.Const(() => "el", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "alert"))))
            .Add(() => Js.Assign(() => Js.Id(() => "el").Prop(() => "className"), () => Js.Str(() => "alert alert-").Op(() => "+", () => (JsSyntax)Js.Id(() => "type"))))
            .Add(() => Js.Assign(() => Js.Id(() => "el").Prop(() => "textContent"), () => Js.Id(() => "message")))
            .Add(() => Js.Id(() => "setTimeout").Invoke(
                () => Js.Arrow(() => new List<string>(), () => Js.Assign(() => Js.Id(() => "el").Prop(() => "className"), () => Js.Str(() => "alert"))),
                () => Js.Num(() => "3000")).Stmt());
        js.Add(() => Js.Func(() => "showAlert", () => new List<string> { "message", "type" }, () => showAlertBody));
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".tool-auth-header")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".tool-auth-back-link")
                .Property("display", "inline-block")
                .Property("margin-bottom", "12px")
                .Property("color", "var(--accent-primary)")
                .Property("text-decoration", "none")
                .Property("font-size", "14px")
                .Property("transition", "color 0.2s")
            .EndSelector()
            .Selector(".tool-auth-back-link:hover")
                .Property("color", "var(--accent-secondary, var(--accent-primary))")
                .Property("text-decoration", "underline")
            .EndSelector()
            .Selector(".tool-auth-title")
                .Property("font-size", "24px")
                .Property("font-weight", "bold")
                .Property("color", "var(--text-primary)")
                .Property("margin", "0 0 8px 0")
            .EndSelector()
            .Selector(".tool-auth-subtitle")
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
                .Property("margin", "0 0 20px 0")
            .EndSelector()
            .Selector(".permission-matrix")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "16px")
                .Property("margin-bottom", "24px")
            .EndSelector()
            .Selector(".tool-group")
                .Property("background", "var(--bg-secondary)")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "8px")
                .Property("overflow", "hidden")
            .EndSelector()
            .Selector(".tool-group.has-restrictions")
                .Property("border-color", "var(--accent-warning, #f59e0b)")
            .EndSelector()
            .Selector(".tool-group-header")
                .Property("display", "flex")
                .Property("justify-content", "space-between")
                .Property("align-items", "center")
                .Property("padding", "12px 16px")
                .Property("background", "var(--bg-card)")
                .Property("border-bottom", "1px solid var(--border)")
            .EndSelector()
            .Selector(".tool-group-name")
                .Property("font-size", "15px")
                .Property("font-weight", "600")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".tool-group-id")
                .Property("font-size", "12px")
                .Property("color", "var(--text-muted)")
                .Property("margin-left", "8px")
                .Property("font-family", "monospace")
            .EndSelector()
            .Selector(".tool-group-actions")
                .Property("display", "flex")
                .Property("gap", "8px")
                .Property("align-items", "center")
            .EndSelector()
            .Selector(".tool-group-status")
                .Property("font-size", "12px")
                .Property("padding", "2px 8px")
                .Property("border-radius", "4px")
            .EndSelector()
            .Selector(".tool-group-status.no-restrictions")
                .Property("background", "rgba(16,185,129,0.15)")
                .Property("color", "var(--accent-secondary)")
            .EndSelector()
            .Selector(".tool-group-status.has-restrictions")
                .Property("background", "rgba(245,158,11,0.15)")
                .Property("color", "var(--accent-warning, #f59e0b)")
            .EndSelector()
            .Selector(".btn-sm")
                .Property("padding", "4px 10px")
                .Property("font-size", "12px")
                .Property("border-radius", "4px")
                .Property("border", "1px solid var(--border)")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-secondary)")
                .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".btn-sm:hover")
                .Property("background", "var(--accent-primary)")
                .Property("color", "white")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".tool-action-list")
                .Property("display", "grid")
                .Property("grid-template-columns", "repeat(auto-fill, minmax(180px, 1fr))")
                .Property("gap", "8px")
                .Property("padding", "12px 16px")
            .EndSelector()
            .Selector(".tool-action-item")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "8px")
                .Property("padding", "6px 10px")
                .Property("border-radius", "6px")
                .Property("transition", "background 0.15s")
            .EndSelector()
            .Selector(".tool-action-item:hover")
                .Property("background", "var(--bg-card)")
            .EndSelector()
            .Selector(".tool-action-item.disabled")
                .Property("opacity", "0.5")
            .EndSelector()
            .Selector(".tool-action-item input[type='checkbox']")
                .Property("width", "16px")
                .Property("height", "16px")
                .Property("cursor", "pointer")
                .Property("accent-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".tool-action-name")
                .Property("font-size", "13px")
                .Property("color", "var(--text-primary)")
                .Property("font-family", "monospace")
            .EndSelector()
            .Selector(".tool-action-name.is-disabled")
                .Property("text-decoration", "line-through")
                .Property("color", "var(--text-muted)")
            .EndSelector()
            .Selector(".form-actions")
                .Property("margin-top", "20px")
                .Property("display", "flex")
                .Property("gap", "10px")
            .EndSelector()
            .Selector(".btn")
                .Property("padding", "10px 20px")
                .Property("border-radius", "6px")
                .Property("border", "1px solid var(--border)")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-primary)")
                .Property("font-size", "14px")
                .Property("cursor", "pointer")
                .Property("transition", "all 0.2s")
            .EndSelector()
            .Selector(".btn:hover")
                .Property("background", "var(--bg-secondary)")
            .EndSelector()
            .Selector(".btn-primary")
                .Property("background", "var(--accent-primary)")
                .Property("color", "white")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".btn-primary:hover")
                .Property("background", "var(--accent-secondary, var(--accent-primary))")
                .Property("border-color", "var(--accent-secondary, var(--accent-primary))")
            .EndSelector()
            .Selector(".alert")
                .Property("padding", "12px 16px")
                .Property("border-radius", "6px")
                .Property("margin-bottom", "16px")
                .Property("font-size", "14px")
                .Property("display", "none")
            .EndSelector()
            .Selector(".alert-success")
                .Property("display", "block")
                .Property("background", "rgba(107,203,119,0.15)")
                .Property("color", "var(--accent-success)")
                .Property("border", "1px solid rgba(107,203,119,0.3)")
            .EndSelector()
            .Selector(".alert-error")
                .Property("display", "block")
                .Property("background", "rgba(255,107,107,0.15)")
                .Property("color", "var(--accent-error)")
                .Property("border", "1px solid rgba(255,107,107,0.3)")
            .EndSelector();
    }
}
