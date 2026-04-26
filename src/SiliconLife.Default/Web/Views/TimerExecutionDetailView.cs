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

public class TimerExecutionDetailView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as TimerExecutionDetailViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        var scripts = GetScripts(vm);
        var styles = GetStyles();
        
        return RenderPage(vm.Skin, vm.Localization.TimerExecutionDetailTitle, "timer-execution-detail", vm.Localization, body, scripts, styles, "timer");
    }

    private static H RenderBody(TimerExecutionDetailViewModel vm)
    {
        return H.Div(
            H.Div(
                H.A("← Back to Execution History").Href($"/timer-executions/{vm.TimerId}").Class("back-link"),
                H.H1(vm.Localization.TimerExecutionDetailHeader),
                H.P($"{vm.Localization.TimerExecutionTimerName.Replace("{0}", vm.TimerName)} | Execution: {vm.ExecutionId}").Class("page-subtitle")
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
        // Reuse ChatHistoryDetailView styles
        return ChatHistoryDetailView.GetStylesInternal();
    }

    private static JsSyntax GetScripts(TimerExecutionDetailViewModel vm)
    {
        // Reuse ChatHistoryDetailView scripts logic
        var apiUrl = $"/api/timer-execution/messages?executionId={vm.ExecutionId}&timerId={vm.TimerId}";
        return ChatHistoryDetailView.GetScriptsStatic(vm.ToolDisplayNames, apiUrl, "No messages in this execution");
    }
}
