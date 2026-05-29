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

namespace SiliconLife.App.Web.Views;

public class ProjectThinkDetailView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as ProjectThinkDetailViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        var scripts = GetScripts(vm);
        var styles = GetStyles();

        return RenderPage(vm.Skin, vm.Localization.ProjectThinkDetailTitle, "project-think-detail", vm.Localization, body, scripts, styles, "projects");
    }

    private static H RenderBody(ProjectThinkDetailViewModel vm)
    {
        var (stateText, stateCssClass) = GetStateDisplay(vm);
        var createdAtText = $"{vm.Localization.ProjectThinkCreatedAt}{vm.CreatedAt}";
        var completedAtText = vm.CompletedAt != null ? $"{vm.Localization.ProjectThinkCompletedAt}{vm.CompletedAt}" : null;

        return H.Div(
            H.Div(
                H.A(vm.Localization.ProjectThinkBackToProjects).Href($"/project/{vm.ProjectId}/think-history").Class("back-link"),
                H.H1(vm.Localization.ProjectThinkDetailHeader),
                H.P(
                    $"{string.Format(vm.Localization.ProjectThinkProjectName, vm.ProjectName)} | {vm.Localization.ProjectThinkRoundLabel}{vm.CurrentRound}/{vm.MaxRounds}"
                ).Class("page-subtitle"),
                H.Div(
                    H.Span(stateText).Class($"execution-state {stateCssClass}"),
                    H.Span(createdAtText).Class("session-meta"),
                    H.Span(completedAtText ?? "").When(completedAtText != null, H.Span(completedAtText!)).Class("session-meta")
                ).Class("session-info")
            ).Class("page-header"),
            H.Div().Id("message-list").Class("message-list"),
            H.Div(
                H.Div("").Class("loading-spinner"),
                H.Div(vm.Localization.ChatLoading).Class("loading-text")
            ).Id("loading-indicator").Class("loading-indicator")
        ).Class("page-content");
    }

    private static (string text, string cssClass) GetStateDisplay(ProjectThinkDetailViewModel vm)
    {
        return vm.State?.ToLowerInvariant() switch
        {
            "started" => (vm.Localization.ProjectThinkStateStarted, "started"),
            "executing" => (vm.Localization.ProjectThinkStateExecuting, "executing"),
            "completed" => (vm.Localization.ProjectThinkStateCompleted, "completed"),
            "failed" => (vm.Localization.ProjectThinkStateFailed, "failed"),
            _ => (vm.State ?? "", "started")
        };
    }

    private static CssBuilder GetStyles()
    {
        return ChatHistoryDetailView.GetStylesInternal()
            .Selector(".session-info")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "16px")
                .Property("margin-top", "8px")
                .Property("flex-wrap", "wrap")
            .EndSelector()
            .Selector(".session-meta")
                .Property("font-size", "13px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".execution-state")
                .Property("display", "inline-block")
                .Property("padding", "4px 12px")
                .Property("border-radius", "12px")
                .Property("font-size", "12px")
                .Property("font-weight", "bold")
            .EndSelector()
            .Selector(".execution-state.started")
                .Property("background", "rgba(77,150,255,0.15)")
                .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".execution-state.executing")
                .Property("background", "rgba(255,193,7,0.15)")
                .Property("color", "#ffc107")
            .EndSelector()
            .Selector(".execution-state.completed")
                .Property("background", "rgba(107,203,119,0.15)")
                .Property("color", "var(--accent-success)")
            .EndSelector()
            .Selector(".execution-state.failed")
                .Property("background", "rgba(255,82,82,0.15)")
                .Property("color", "var(--accent-error, #ff5252)")
            .EndSelector()
            // Cycle collapsible styles
            .Selector(".cycle-collapsible")
                .Property("margin", "8px 0")
                .Property("border", "1px solid var(--border-color, rgba(255,255,255,0.1))")
                .Property("border-radius", "8px")
                .Property("overflow", "hidden")
            .EndSelector()
            .Selector(".cycle-collapsible summary")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "8px")
                .Property("padding", "8px 12px")
                .Property("background", "rgba(77,150,255,0.08)")
                .Property("font-size", "13px")
                .Property("font-weight", "600")
                .Property("color", "var(--accent-primary)")
                .Property("cursor", "pointer")
                .Property("user-select", "none")
                .Property("list-style", "none")
            .EndSelector()
            .Selector(".cycle-collapsible summary:hover")
                .Property("background", "rgba(77,150,255,0.15)")
            .EndSelector()
            .Selector(".cycle-collapsible summary::-webkit-details-marker")
                .Property("display", "none")
            .EndSelector()
            .Selector(".cycle-collapsible summary::marker")
                .Property("display", "none")
                .Property("content", "")
            .EndSelector()
            .Selector(".cycle-collapsible summary .cycle-arrow")
                .Property("transition", "transform 0.2s ease")
                .Property("font-size", "10px")
            .EndSelector()
            .Selector(".cycle-collapsible[open] summary .cycle-arrow")
                .Property("transform", "none")
            .EndSelector()
            .Selector(".cycle-collapsible:not([open]) summary .cycle-arrow")
                .Property("transform", "rotate(-90deg)")
            .EndSelector()
            .Selector(".cycle-status")
                .Property("font-size", "11px")
                .Property("font-weight", "normal")
                .Property("color", "var(--text-secondary)")
                .Property("margin-left", "4px")
            .EndSelector();
    }

    private static JsSyntax GetScripts(ProjectThinkDetailViewModel vm)
    {
        var apiUrl = $"/api/projects/{vm.ProjectId}/think-sessions/detail?sessionId={vm.SessionId}";
        return ChatHistoryDetailView.GetScriptsStatic(
            vm.ToolDisplayNames, apiUrl, vm.Localization.ProjectThinkNoRecords,
            includeCycleData: true,
            cycleLabel: vm.Localization.ProjectThinkCycleLabel,
            cycleRoundFormat: vm.Localization.ProjectThinkRoundN);
    }
}
