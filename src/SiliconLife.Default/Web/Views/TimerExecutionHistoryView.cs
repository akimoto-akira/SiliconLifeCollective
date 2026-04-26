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

public class TimerExecutionHistoryView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as TimerExecutionHistoryViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.TimerExecutionHistoryTitle, "timer-execution-history", vm.Localization, body, GetScripts(vm), GetStyles(), "timer");
    }

    private static H RenderBody(TimerExecutionHistoryViewModel vm)
    {
        return H.Div(
            H.Div(
                H.A(vm.Localization.TimerExecutionBackToTimers).Href("/timers").Class("back-link"),
                H.H1(vm.Localization.TimerExecutionHistoryHeader),
                H.P(string.Format(vm.Localization.TimerExecutionTimerName, vm.TimerName)).Class("page-subtitle")
            ).Class("page-header"),
            H.Div().Id("execution-list").Class("execution-list")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".back-link")
                .Property("color", "var(--accent-primary)")
                .Property("text-decoration", "none")
                .Property("font-weight", "bold")
                .Property("transition", "color 0.2s")
            .EndSelector()
            .Selector(".back-link:hover")
                .Property("color", "var(--accent-secondary, var(--accent-primary))")
                .Property("text-decoration", "underline")
            .EndSelector()
            .Selector(".page-subtitle")
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-top", "8px")
            .EndSelector()
            .Selector(".execution-list")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "12px")
                .Property("margin-top", "20px")
            .EndSelector()
            .Selector(".execution-item")
                .Property("background", "var(--bg-card)")
                .Property("padding", "16px")
                .Property("border-radius", "8px")
                .Property("border", "1px solid var(--border)")
                .Property("cursor", "pointer")
                .Property("transition", "transform 0.2s, box-shadow 0.2s")
            .EndSelector()
            .Selector(".execution-item:hover")
                .Property("transform", "translateY(-2px)")
                .Property("box-shadow", "0 4px 12px rgba(0,0,0,0.1)")
            .EndSelector()
            .Selector(".execution-header")
                .Property("display", "flex")
                .Property("justify-content", "space-between")
                .Property("align-items", "center")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".execution-state")
                .Property("display", "inline-block")
                .Property("padding", "4px 12px")
                .Property("border-radius", "12px")
                .Property("font-size", "12px")
            .EndSelector()
            .Selector(".execution-state.idle")
                .Property("background", "rgba(158,158,158,0.15)")
                .Property("color", "var(--text-secondary)")
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
            .Selector(".execution-info")
                .Property("font-size", "13px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-bottom", "4px")
            .EndSelector()
            .Selector(".execution-info-label")
                .Property("font-weight", "bold")
                .Property("margin-right", "8px")
            .EndSelector()
            .Selector(".empty-state")
                .Property("text-align", "center")
                .Property("padding", "40px")
                .Property("color", "var(--text-secondary)")
            .EndSelector();
    }

    private static JsSyntax GetScripts(TimerExecutionHistoryViewModel vm)
    {
        var forEachBody = Js.Block()
            .Add(() => Js.Const(() => "item", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Const(() => "stateClass", () => Js.Id(() => "e").Prop(() => "state").Call(() => "toLowerCase")))
            .Add(() => Js.Const(() => "stateText", () => Js.Id(() => "e").Prop(() => "state").Call(() => "charAt", () => Js.Num(() => "0")).Call(() => "toUpperCase").Call(() => "concat", () => Js.Id(() => "e").Prop(() => "state").Call(() => "slice", () => Js.Num(() => "1")))))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "className"), () => Js.Str(() => "execution-item")))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "onclick"), () => Js.Arrow(
                () => new List<string>(),
                () => Js.Assign(
                    () => Js.Id(() => "window").Prop(() => "location"),
                    () => Js.Str(() => "/timer-execution/")
                        .Op(() => "+", () => Js.Id(() => "e").Prop(() => "executionId"))
                        .Op(() => "+", () => Js.Str(() => "?timerId="))
                        .Op(() => "+", () => Js.Id(() => "timerId"))
                )
            )))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "innerHTML"), () => BuildItemHtml()))
            .Add(() => Js.Id(() => "list").Call(() => "appendChild", () => Js.Id(() => "item")).Stmt());

        var thenBody = Js.Block()
            .Add(() => Js.Const(() => "list", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "execution-list"))))
            .Add(() => Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "data").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => "<div class='empty-state'>")
                        .Op(() => "+", () => (JsSyntax)Js.Id(() => "emptyMessage"))
                        .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>")))
                }),
                (null, new List<JsSyntax>
                {
                    Js.Id(() => "data").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "e" }, () => forEachBody)).Stmt()
                })
            }));

        var loadBody = Js.Block()
            .Add(() => Js.Const(() => "emptyMessage", () => Js.Str(() => vm.Localization.TimerExecutionNoRecords)))
            .Add(() => Js.Id(() => "fetch")
                .Invoke(() => Js.Str(() => "/api/timer-executions/list?timerId=").Op(() => "+", () => Js.Id(() => "timerId")))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json")))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => thenBody)).Stmt());

        return Js.Block()
            .Add(() => Js.Const(() => "timerId", () => Js.Id(() => "window").Prop(() => "location").Prop(() => "pathname").Call(() => "split", () => Js.Str(() => "/")).Index(() => Js.Num(() => "2"))))
            .Add(() => Js.Func(() => "loadExecutions", () => new List<string>(), () => loadBody))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadExecutions").Invoke())));
    }

    private static JsSyntax BuildItemHtml()
    {
        return Js.Str(() => "<div class='execution-header'><span class='execution-state ")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "stateClass"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "'>"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "stateText"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</span></div><div class='execution-info'><span class='execution-info-label'>Triggered:</span>"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "e").Prop(() => "triggeredAt"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>"))
            .Op(() => "+", () => Js.Ternary(
                () => Js.Id(() => "e").Prop(() => "completedAt"),
                () => Js.Str(() => "<div class='execution-info'><span class='execution-info-label'>Completed:</span>")
                    .Op(() => "+", () => (JsSyntax)Js.Id(() => "e").Prop(() => "completedAt"))
                    .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>")),
                () => Js.Str(() => "")
            ))
            .Op(() => "+", () => Js.Str(() => "<div class='execution-info'><span class='execution-info-label'>Steps:</span>"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "e").Prop(() => "stepCount"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => " | Messages: "))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "e").Prop(() => "messageCount"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>"));
    }
}
