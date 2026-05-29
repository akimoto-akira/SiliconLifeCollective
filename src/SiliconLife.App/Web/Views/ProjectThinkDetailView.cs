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
                    H.Span(completedAtText ?? "").When(completedAtText != null).Class("session-meta")
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
            .EndSelector();
    }

    private static JsSyntax GetScripts(ProjectThinkDetailViewModel vm)
    {
        var apiUrl = $"/api/projects/{vm.ProjectId}/think-sessions/detail?sessionId={vm.SessionId}";
        return ChatHistoryDetailView.GetScriptsStatic(vm.ToolDisplayNames, apiUrl, vm.Localization.ProjectThinkNoRecords);
    }
}
