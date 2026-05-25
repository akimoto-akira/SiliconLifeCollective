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

using SiliconLife.App.Web.Models;

using SiliconLife.Common.Localization;

namespace SiliconLife.App.Web.Views;

public class WorkflowDetailView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as WorkflowDetailViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.WorkflowDetailPageHeader, "projects", vm.Localization, body, GetScripts(vm), GetStyles(), "projects");
    }

    private static H RenderBody(WorkflowDetailViewModel vm)
    {
        var loc = vm.Localization;
        return H.Div(
            H.Div(
                H.A($"← {loc.BackToProject}").Attr("href", "/project").Class("back-link"),
                H.H1($"{loc.WorkflowDetailPageHeader} — {vm.ProjectName}")
            ).Class("page-header"),
            H.Div(
                H.Div(
                    H.H2(loc.WorkflowRoleAssignmentsHeader).Class("section-title"),
                    H.Div().Id("role-assignments").Class("role-assignments-list")
                ).Class("card section-card"),
                H.Div(
                    H.H2(loc.WorkflowUnassignedBeingsHeader).Class("section-title"),
                    H.Div().Id("unassigned-beings").Class("unassigned-beings-list")
                ).Class("card section-card"),
                H.Div(
                    H.H2(loc.WorkflowStateTransitionsHeader).Class("section-title"),
                    H.Div().Id("state-transitions").Class("state-transitions-list")
                ).Class("card section-card")
            ).Class("workflow-detail-content")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".back-link")
                .Property("color", "var(--accent-primary)")
                .Property("text-decoration", "none")
                .Property("font-size", "14px")
                .Property("display", "inline-block")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".back-link:hover")
                .Property("text-decoration", "underline")
            .EndSelector()
            .Selector(".section-card")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".section-title")
                .Property("font-size", "18px")
                .Property("margin", "0 0 16px 0")
                .Property("color", "var(--text-primary)")
                .Property("padding-bottom", "8px")
                .Property("border-bottom", "1px solid var(--border-color)")
            .EndSelector()
            .Selector(".role-item")
                .Property("background", "var(--bg-secondary)")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "8px")
                .Property("padding", "16px")
                .Property("margin-bottom", "12px")
            .EndSelector()
            .Selector(".role-header")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("justify-content", "space-between")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".role-name")
                .Property("font-weight", "bold")
                .Property("font-size", "15px")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".role-desc")
                .Property("color", "var(--text-secondary)")
                .Property("font-size", "13px")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".role-meta")
                .Property("display", "flex")
                .Property("gap", "16px")
                .Property("font-size", "12px")
                .Property("color", "var(--text-muted)")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".role-meta-item")
                .Property("display", "inline-flex")
                .Property("align-items", "center")
                .Property("gap", "4px")
            .EndSelector()
            .Selector(".staffing-badge")
                .Property("padding", "2px 8px")
                .Property("border-radius", "4px")
                .Property("font-size", "11px")
                .Property("font-weight", "500")
            .EndSelector()
            .Selector(".staffing-understaffed")
                .Property("background", "rgba(239,68,68,0.15)")
                .Property("color", "#ef4444")
            .EndSelector()
            .Selector(".staffing-sufficient")
                .Property("background", "rgba(16,185,129,0.15)")
                .Property("color", "#10b981")
            .EndSelector()
            .Selector(".staffing-full")
                .Property("background", "rgba(59,130,246,0.15)")
                .Property("color", "#3b82f6")
            .EndSelector()
            .Selector(".staffing-overstaffed")
                .Property("background", "rgba(245,158,11,0.15)")
                .Property("color", "#f59e0b")
            .EndSelector()
            .Selector(".assigned-beings")
                .Property("display", "flex")
                .Property("flex-wrap", "wrap")
                .Property("gap", "6px")
            .EndSelector()
            .Selector(".being-chip")
                .Property("display", "inline-flex")
                .Property("align-items", "center")
                .Property("gap", "4px")
                .Property("padding", "4px 10px")
                .Property("border-radius", "12px")
                .Property("font-size", "12px")
                .Property("background", "rgba(59,130,246,0.1)")
                .Property("color", "#3b82f6")
                .Property("border", "1px solid rgba(59,130,246,0.2)")
            .EndSelector()
            .Selector(".being-chip .remove-btn")
                .Property("cursor", "pointer")
                .Property("color", "#ef4444")
                .Property("font-weight", "bold")
                .Property("margin-left", "4px")
                .Property("font-size", "14px")
                .Property("line-height", "1")
            .EndSelector()
            .Selector(".being-chip .remove-btn:hover")
                .Property("color", "#dc2626")
            .EndSelector()
            .Selector(".unassigned-being-item")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("justify-content", "space-between")
                .Property("padding", "10px 16px")
                .Property("background", "var(--bg-secondary)")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "8px")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".unassigned-being-name")
                .Property("font-size", "14px")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".btn-assign-role")
                .Property("padding", "4px 12px")
                .Property("border-radius", "4px")
                .Property("font-size", "12px")
                .Property("cursor", "pointer")
                .Property("background", "rgba(16,185,129,0.15)")
                .Property("color", "#10b981")
                .Property("border", "1px solid rgba(16,185,129,0.3)")
            .EndSelector()
            .Selector(".btn-assign-role:hover")
                .Property("background", "rgba(16,185,129,0.25)")
            .EndSelector()
            .Selector(".role-select")
                .Property("padding", "4px 8px")
                .Property("border-radius", "4px")
                .Property("font-size", "12px")
                .Property("background", "var(--bg-secondary)")
                .Property("color", "var(--text-primary)")
                .Property("border", "1px solid var(--border-color)")
                .Property("margin-right", "6px")
            .EndSelector()
            .Selector(".transition-item")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "12px")
                .Property("padding", "12px 16px")
                .Property("background", "var(--bg-secondary)")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "8px")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".state-node")
                .Property("display", "inline-flex")
                .Property("align-items", "center")
                .Property("padding", "6px 14px")
                .Property("border-radius", "16px")
                .Property("font-size", "13px")
                .Property("font-weight", "500")
            .EndSelector()
            .Selector(".state-node.state-normal")
                .Property("background", "rgba(59,130,246,0.12)")
                .Property("color", "#3b82f6")
            .EndSelector()
            .Selector(".state-node.state-initial")
                .Property("background", "rgba(16,185,129,0.12)")
                .Property("color", "#10b981")
                .Property("border", "1px solid rgba(16,185,129,0.3)")
            .EndSelector()
            .Selector(".state-node.state-terminal")
                .Property("background", "rgba(239,68,68,0.12)")
                .Property("color", "#ef4444")
                .Property("border", "1px solid rgba(239,68,68,0.3)")
            .EndSelector()
            .Selector(".transition-arrow")
                .Property("color", "var(--text-muted)")
                .Property("font-size", "18px")
            .EndSelector()
            .Selector(".transition-name")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
                .Property("font-style", "italic")
            .EndSelector()
            .Selector(".empty-state")
                .Property("text-align", "center")
                .Property("padding", "24px")
                .Property("color", "var(--text-secondary)")
                .Property("font-size", "14px")
            .EndSelector()
            .Selector(".no-template-state")
                .Property("text-align", "center")
                .Property("padding", "48px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".assign-role-row")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "6px")
            .EndSelector();
    }

    private static JsSyntax GetScripts(WorkflowDetailViewModel vm)
    {
        var loc = vm.Localization;
        var projectId = vm.ProjectId.ToString();

        // Build roles rendering body
        var roleForEachBody = Js.Block()
            .Add(() => Js.Const(() => "div", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "div").Prop(() => "className"), () => Js.Str(() => "role-item")))
            .Add(() => Js.Const(() => "statusClass", () => Js.Obj()
                .Prop(() => "Understaffed", () => Js.Str(() => "staffing-understaffed"))
                .Prop(() => "Sufficient", () => Js.Str(() => "staffing-sufficient"))
                .Prop(() => "Full", () => Js.Str(() => "staffing-full"))
                .Prop(() => "Overstaffed", () => Js.Str(() => "staffing-overstaffed"))))
            .Add(() => Js.Const(() => "statusLabels", () => Js.Obj()
                .Prop(() => "Understaffed", () => Js.Str(() => loc.RoleStaffing_Understaffed))
                .Prop(() => "Sufficient", () => Js.Str(() => loc.RoleStaffing_Sufficient))
                .Prop(() => "Full", () => Js.Str(() => loc.RoleStaffing_Full))
                .Prop(() => "Overstaffed", () => Js.Str(() => loc.RoleStaffing_Overstaffed))))
            .Add(() => Js.Const(() => "staffingClass", () => Js.Id(() => "statusClass").Index(() => Js.Id(() => "role").Prop(() => "staffingStatus")).Op(() => "||", () => Js.Str(() => ""))))
            .Add(() => Js.Const(() => "staffingLabel", () => Js.Id(() => "statusLabels").Index(() => Js.Id(() => "role").Prop(() => "staffingStatus")).Op(() => "||", () => Js.Id(() => "role").Prop(() => "staffingStatus"))))
            .Add(() => Js.Const(() => "maxText", () => Js.Id(() => "role").Prop(() => "maxCount").Op(() => ">", () => Js.Num(() => "0")).Op(() => "?", () => Js.Id(() => "role").Prop(() => "maxCount")).Op(() => ":", () => Js.Str(() => "∞"))))
            .Add(() => Js.Const(() => "beingsHtml", () => Js.Id(() => "role").Prop(() => "assignedBeings").Call(() => "map", () => Js.Arrow(() => new List<string> { "b" }, () =>
                Js.Str(() => "<span class='being-chip'>")
                .Op(() => "+", () => Js.Id(() => "b").Prop(() => "name"))
                .Op(() => "+", () => Js.Str(() => "<span class='remove-btn' data-role='"))
                .Op(() => "+", () => Js.Id(() => "role").Prop(() => "roleName"))
                .Op(() => "+", () => Js.Str(() => "' data-being-id='"))
                .Op(() => "+", () => Js.Id(() => "b").Prop(() => "id"))
                .Op(() => "+", () => Js.Str(() => $"' title='{loc.WorkflowRemoveFromRoleButton}'>×</span></span>"))))
                .Call(() => "join", () => Js.Str(() => ""))))
            .Add(() => Js.Assign(() => Js.Id(() => "div").Prop(() => "innerHTML"), () =>
                Js.Str(() => "<div class='role-header'><span class='role-name'>")
                .Op(() => "+", () => Js.Id(() => "role").Prop(() => "roleName"))
                .Op(() => "+", () => Js.Str(() => "</span><span class='staffing-badge "))
                .Op(() => "+", () => Js.Id(() => "staffingClass"))
                .Op(() => "+", () => Js.Str(() => "'>"))
                .Op(() => "+", () => Js.Id(() => "staffingLabel"))
                .Op(() => "+", () => Js.Str(() => "</span></div><div class='role-desc'>"))
                .Op(() => "+", () => Js.Id(() => "role").Prop(() => "description"))
                .Op(() => "+", () => Js.Str(() => "</div><div class='role-meta'><span class='role-meta-item'>"))
                .Op(() => "+", () => Js.Str(() => loc.WorkflowRoleRequiredCountLabel))
                .Op(() => "+", () => Js.Str(() => ": "))
                .Op(() => "+", () => Js.Id(() => "role").Prop(() => "minCount"))
                .Op(() => "+", () => Js.Str(() => "-"))
                .Op(() => "+", () => Js.Id(() => "maxText"))
                .Op(() => "+", () => Js.Str(() => "</span><span class='role-meta-item'>"))
                .Op(() => "+", () => Js.Str(() => loc.WorkflowRoleAssignedCountLabel))
                .Op(() => "+", () => Js.Str(() => ": "))
                .Op(() => "+", () => Js.Id(() => "role").Prop(() => "assignedBeings").Prop(() => "length"))
                .Op(() => "+", () => Js.Str(() => "</span></div><div class='assigned-beings'>"))
                .Op(() => "+", () => Js.Id(() => "beingsHtml"))
                .Op(() => "+", () => Js.Str(() => "</div>"))))
            .Add(() => Js.Id(() => "container").Call(() => "appendChild", () => Js.Id(() => "div")).Stmt());

        var rolesIfBranches = new List<(JsSyntax? Condition, List<JsSyntax> Body)>
        {
            (Js.Id(() => "data").Prop(() => "roles").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
            {
                Js.Assign(() => Js.Id(() => "container").Prop(() => "innerHTML"), () => Js.Str(() => $"<div class='empty-state'>{loc.WorkflowNoRoleDefinitions}</div>"))
            }),
            (null, new List<JsSyntax>
            {
                Js.Id(() => "data").Prop(() => "roles").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "role" }, () => roleForEachBody)).Stmt()
            })
        };
        var renderRolesBody = Js.Block()
            .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "role-assignments"))))
            .Add(() => Js.Assign(() => Js.Id(() => "container").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.If(() => rolesIfBranches));

        // Build unassigned beings rendering body
        var unassignedForEachBody = Js.Block()
            .Add(() => Js.Const(() => "div", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "div").Prop(() => "className"), () => Js.Str(() => "unassigned-being-item")))
            .Add(() => Js.Const(() => "roleOptions", () => Js.Id(() => "data").Prop(() => "roles").Call(() => "map", () => Js.Arrow(() => new List<string> { "r" }, () =>
                Js.Str(() => "<option value='").Op(() => "+", () => Js.Id(() => "r").Prop(() => "roleName")).Op(() => "+", () => Js.Str(() => "'>")).Op(() => "+", () => Js.Id(() => "r").Prop(() => "roleName")).Op(() => "+", () => Js.Str(() => "</option>"))))
                .Call(() => "join", () => Js.Str(() => ""))))
            .Add(() => Js.Assign(() => Js.Id(() => "div").Prop(() => "innerHTML"), () =>
                Js.Str(() => "<span class='unassigned-being-name'>")
                .Op(() => "+", () => Js.Id(() => "b").Prop(() => "name"))
                .Op(() => "+", () => Js.Str(() => "</span><div class='assign-role-row'><select class='role-select' data-being-id='"))
                .Op(() => "+", () => Js.Id(() => "b").Prop(() => "id"))
                .Op(() => "+", () => Js.Str(() => "'>"))
                .Op(() => "+", () => Js.Id(() => "roleOptions"))
                .Op(() => "+", () => Js.Str(() => $"</select><button class='btn-assign-role' data-being-id='"))
                .Op(() => "+", () => Js.Id(() => "b").Prop(() => "id"))
                .Op(() => "+", () => Js.Str(() => $"'>{loc.WorkflowAssignRoleButton}</button></div>"))))
            .Add(() => Js.Id(() => "container").Call(() => "appendChild", () => Js.Id(() => "div")).Stmt());

        var unassignedIfBranches = new List<(JsSyntax? Condition, List<JsSyntax> Body)>
        {
            (Js.Id(() => "data").Prop(() => "unassignedBeings").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
            {
                Js.Assign(() => Js.Id(() => "container").Prop(() => "innerHTML"), () => Js.Str(() => $"<div class='empty-state'>{loc.WorkflowNoUnassignedBeings}</div>"))
            }),
            (null, new List<JsSyntax>
            {
                Js.Id(() => "data").Prop(() => "unassignedBeings").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "b" }, () => unassignedForEachBody)).Stmt()
            })
        };
        var renderUnassignedBody = Js.Block()
            .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "unassigned-beings"))))
            .Add(() => Js.Assign(() => Js.Id(() => "container").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.If(() => unassignedIfBranches));

        // Build transitions rendering body
        var transitionForEachBody = Js.Block()
            .Add(() => Js.Const(() => "div", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "div").Prop(() => "className"), () => Js.Str(() => "transition-item")))
            .Add(() => Js.Assign(() => Js.Id(() => "div").Prop(() => "innerHTML"), () =>
                Js.Str(() => "<span class='state-node state-normal'>")
                .Op(() => "+", () => Js.Id(() => "t").Prop(() => "fromState"))
                .Op(() => "+", () => Js.Str(() => "</span><span class='transition-arrow'>→</span><span class='state-node state-normal'>"))
                .Op(() => "+", () => Js.Id(() => "t").Prop(() => "toState"))
                .Op(() => "+", () => Js.Str(() => "</span><span class='transition-name'>"))
                .Op(() => "+", () => Js.Id(() => "t").Prop(() => "transitionName"))
                .Op(() => "+", () => Js.Str(() => "</span>"))))
            .Add(() => Js.Id(() => "container").Call(() => "appendChild", () => Js.Id(() => "div")).Stmt());

        var transitionsIfBranches = new List<(JsSyntax? Condition, List<JsSyntax> Body)>
        {
            (Js.Id(() => "data").Prop(() => "transitions").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
            {
                Js.Assign(() => Js.Id(() => "container").Prop(() => "innerHTML"), () => Js.Id(() => "container").Prop(() => "innerHTML").Op(() => "+", () => Js.Str(() => $"<div class='empty-state'>{loc.WorkflowNoTransitions}</div>")))
            }),
            (null, new List<JsSyntax>
            {
                Js.Id(() => "data").Prop(() => "transitions").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "t" }, () => transitionForEachBody)).Stmt()
            })
        };
        var renderTransitionsBody = Js.Block()
            .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "state-transitions"))))
            .Add(() => Js.Assign(() => Js.Id(() => "container").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.If(() => transitionsIfBranches));

        // Fetch and dispatch
        var fetchIfBranches = new List<(JsSyntax? Condition, List<JsSyntax> Body)>
        {
            (Js.Id(() => "data").Prop(() => "success"), new List<JsSyntax>
            {
                Js.Id(() => "renderRoles").Invoke(() => Js.Id(() => "data")).Stmt(),
                Js.Id(() => "renderUnassigned").Invoke(() => Js.Id(() => "data")).Stmt(),
                Js.Id(() => "renderTransitions").Invoke(() => Js.Id(() => "data")).Stmt()
            }),
            (null, new List<JsSyntax>
            {
                Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "role-assignments")).Prop(() => "innerHTML"), () => Js.Str(() => $"<div class='no-template-state'>{loc.WorkflowNoTemplateMessage}</div>"))
            })
        };
        var fetchThenBody = Js.Block()
            .Add(() => Js.If(() => fetchIfBranches));

        var loadDetailBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => $"/api/projects/workflow-detail?projectId={projectId}")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => fetchThenBody)).Stmt());

        // Assign role handler
        var assignEarlyReturnBranches = new List<(JsSyntax? Condition, List<JsSyntax> Body)>
        {
            (Js.Id(() => "roleName").Op(() => "===", () => Js.Str(() => "")), new List<JsSyntax>
            {
                Js.Return(() => Js.Id(() => "undefined"))
            })
        };
        var assignResultBranches = new List<(JsSyntax? Condition, List<JsSyntax> Body)>
        {
            (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
            {
                Js.Id(() => "loadDetail").Invoke().Stmt()
            })
        };
        var assignRoleBody = Js.Block()
            .Add(() => Js.Const(() => "beingId", () => Js.Id(() => "btn").Prop(() => "dataset").Prop(() => "beingId")))
            .Add(() => Js.Const(() => "select", () => Js.Id(() => "btn").Prop(() => "previousElementSibling")))
            .Add(() => Js.Const(() => "roleName", () => Js.Id(() => "select").Prop(() => "value")))
            .Add(() => Js.If(() => assignEarlyReturnBranches))
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/projects/assign-role"), () => Js.Obj()
                .Prop(() => "method", () => Js.Str(() => "POST"))
                .Prop(() => "headers", () => Js.Obj().Prop(() => "Content-Type", () => Js.Str(() => "application/json")))
                .Prop(() => "body", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Obj()
                    .Prop(() => "projectId", () => Js.Str(() => projectId))
                    .Prop(() => "roleName", () => Js.Id(() => "roleName"))
                    .Prop(() => "beingId", () => Js.Id(() => "beingId")))))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json")))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => Js.Block()
                    .Add(() => Js.If(() => assignResultBranches)))).Stmt());

        // Remove from role handler
        var removeResultBranches = new List<(JsSyntax? Condition, List<JsSyntax> Body)>
        {
            (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
            {
                Js.Id(() => "loadDetail").Invoke().Stmt()
            })
        };
        var removeRoleBody = Js.Block()
            .Add(() => Js.Const(() => "beingId", () => Js.Id(() => "span").Prop(() => "dataset").Prop(() => "beingId")))
            .Add(() => Js.Const(() => "roleName", () => Js.Id(() => "span").Prop(() => "dataset").Prop(() => "role")))
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/projects/remove-role"), () => Js.Obj()
                .Prop(() => "method", () => Js.Str(() => "POST"))
                .Prop(() => "headers", () => Js.Obj().Prop(() => "Content-Type", () => Js.Str(() => "application/json")))
                .Prop(() => "body", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Obj()
                    .Prop(() => "projectId", () => Js.Str(() => projectId))
                    .Prop(() => "roleName", () => Js.Id(() => "roleName"))
                    .Prop(() => "beingId", () => Js.Id(() => "beingId")))))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json")))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => Js.Block()
                    .Add(() => Js.If(() => removeResultBranches)))).Stmt());

        // Delegated click handler
        var clickAssignBranches = new List<(JsSyntax? Condition, List<JsSyntax> Body)>
        {
            (Js.Id(() => "e").Prop(() => "target").Prop(() => "classList").Call(() => "contains", () => Js.Str(() => "btn-assign-role")), new List<JsSyntax>
            {
                Js.Id(() => "assignRole").Invoke(() => Js.Id(() => "e").Prop(() => "target")).Stmt()
            })
        };
        var clickRemoveBranches = new List<(JsSyntax? Condition, List<JsSyntax> Body)>
        {
            (Js.Id(() => "e").Prop(() => "target").Prop(() => "classList").Call(() => "contains", () => Js.Str(() => "remove-btn")), new List<JsSyntax>
            {
                Js.Id(() => "removeRole").Invoke(() => Js.Id(() => "e").Prop(() => "target")).Stmt()
            })
        };
        var delegatedClickBody = Js.Block()
            .Add(() => Js.If(() => clickAssignBranches))
            .Add(() => Js.If(() => clickRemoveBranches));

        return Js.Block()
            .Add(() => Js.Func(() => "renderRoles", () => new List<string> { "data" }, () => renderRolesBody))
            .Add(() => Js.Func(() => "renderUnassigned", () => new List<string> { "data" }, () => renderUnassignedBody))
            .Add(() => Js.Func(() => "renderTransitions", () => new List<string> { "data" }, () => renderTransitionsBody))
            .Add(() => Js.Func(() => "loadDetail", () => new List<string>(), () => loadDetailBody))
            .Add(() => Js.Func(() => "assignRole", () => new List<string> { "btn" }, () => assignRoleBody))
            .Add(() => Js.Func(() => "removeRole", () => new List<string> { "span" }, () => removeRoleBody))
            .Add(() => Js.Id(() => "document").Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string> { "e" }, () => delegatedClickBody)).Stmt())
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadDetail").Invoke())));
    }
}
