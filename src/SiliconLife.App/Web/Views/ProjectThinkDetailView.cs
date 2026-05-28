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
        return H.Div(
            H.Div(
                H.A(vm.Localization.ProjectThinkBackToProjects).Href($"/project/{vm.ProjectId}/think-history").Class("back-link"),
                H.H1(vm.Localization.ProjectThinkDetailHeader),
                H.P($"{string.Format(vm.Localization.ProjectThinkProjectName, vm.ProjectName)} | {vm.Localization.ProjectThinkRoundLabel}{vm.CurrentRound}/{vm.MaxRounds}").Class("page-subtitle")
            ).Class("page-header"),
            H.Div().Id("message-list").Class("message-list"),
            H.Div(
                H.Div("").Class("loading-spinner"),
                H.Div("Loading messages...").Class("loading-text")
            ).Id("loading-indicator").Class("loading-indicator")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return ChatHistoryDetailView.GetStylesInternal();
    }

    private static JsSyntax GetScripts(ProjectThinkDetailViewModel vm)
    {
        var apiUrl = $"/api/projects/{vm.ProjectId}/think-sessions/detail?sessionId={vm.SessionId}";
        return ChatHistoryDetailView.GetScriptsStatic(vm.ToolDisplayNames, apiUrl, vm.Localization.ProjectThinkNoRecords);
    }
}
