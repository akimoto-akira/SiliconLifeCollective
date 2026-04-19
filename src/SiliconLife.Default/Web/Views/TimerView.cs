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

public class TimerView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as TimerViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.PageTitleTimers, "timers", vm.Localization, body, GetScripts(vm.Localization), GetStyles());
    }

    private static H RenderBody(TimerViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1(vm.Localization.TimersPageHeader),
                H.Div(
                    H.Span(string.Format(vm.Localization.TimersTotalCount, "")).Id("total-count").Class("stat-value")
                ).Class("page-stat")
            ).Class("page-header"),
            H.Div().Id("timers-grid").Class("timers-grid")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".page-stat")
                .Property("margin-left", "16px")
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".stat-value")
                .Property("font-weight", "bold")
                .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".timers-grid")
                .Property("display", "grid")
                .Property("grid-template-columns", "repeat(auto-fill, minmax(320px, 1fr))")
                .Property("gap", "20px")
            .EndSelector()
            .Selector(".timer-card")
                .Property("background", "var(--bg-card)")
                .Property("padding", "20px")
                .Property("border-radius", "12px")
                .Property("border", "1px solid var(--border)")
                .Property("transition", "transform 0.2s, box-shadow 0.2s")
            .EndSelector()
            .Selector(".timer-card:hover")
                .Property("transform", "translateY(-2px)")
                .Property("box-shadow", "0 4px 12px rgba(0,0,0,0.1)")
            .EndSelector()
            .Selector(".timer-header")
                .Property("display", "flex")
                .Property("justify-content", "space-between")
                .Property("align-items", "center")
                .Property("margin-bottom", "12px")
            .EndSelector()
            .Selector(".timer-name")
                .Property("font-size", "16px")
                .Property("font-weight", "bold")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".timer-status")
                .Property("display", "inline-block")
                .Property("padding", "4px 12px")
                .Property("border-radius", "12px")
                .Property("font-size", "12px")
            .EndSelector()
            .Selector(".timer-status.active")
                .Property("background", "rgba(107,203,119,0.15)")
                .Property("color", "var(--accent-success)")
            .EndSelector()
            .Selector(".timer-status.paused")
                .Property("background", "rgba(255,193,7,0.15)")
                .Property("color", "#ffc107")
            .EndSelector()
            .Selector(".timer-status.triggered")
                .Property("background", "rgba(156,136,255,0.15)")
                .Property("color", "#9c88ff")
            .EndSelector()
            .Selector(".timer-status.cancelled")
                .Property("background", "rgba(158,158,158,0.15)")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".timer-info")
                .Property("font-size", "13px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".timer-info-label")
                .Property("font-weight", "bold")
                .Property("margin-right", "8px")
            .EndSelector()
            .Selector(".timer-type-badge")
                .Property("display", "inline-block")
                .Property("padding", "2px 8px")
                .Property("border-radius", "4px")
                .Property("font-size", "11px")
                .Property("background", "rgba(77,150,255,0.15)")
                .Property("color", "var(--accent-primary)")
                .Property("margin-left", "8px")
            .EndSelector()
            .Selector(".calendar-badge")
                .Property("display", "inline-block")
                .Property("padding", "2px 8px")
                .Property("border-radius", "4px")
                .Property("font-size", "11px")
                .Property("background", "rgba(255,152,0,0.15)")
                .Property("color", "#ff9800")
                .Property("margin-left", "8px")
            .EndSelector()
            .Selector(".empty-state")
                .Property("text-align", "center")
                .Property("padding", "40px")
                .Property("color", "var(--text-secondary)")
            .EndSelector();
    }

    private static JsSyntax GetScripts(DefaultLocalizationBase loc)
    {
        var forEachBody = Js.Block()
            .Add(() => Js.Const(() => "card", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Const(() => "statusClass", () => Js.Ternary(() => Js.Id(() => "t").Prop(() => "status").Op(() => "===", () => Js.Str(() => "active")), () => Js.Str(() => "active"),
                () => Js.Ternary(() => Js.Id(() => "t").Prop(() => "status").Op(() => "===", () => Js.Str(() => "paused")), () => Js.Str(() => "paused"),
                () => Js.Ternary(() => Js.Id(() => "t").Prop(() => "status").Op(() => "===", () => Js.Str(() => "triggered")), () => Js.Str(() => "triggered"), () => Js.Str(() => "cancelled"))))))
            .Add(() => Js.Const(() => "statusText", () => Js.Ternary(() => Js.Id(() => "t").Prop(() => "status").Op(() => "===", () => Js.Str(() => "active")), () => Js.Str(() => loc.TimersStatusActive),
                () => Js.Ternary(() => Js.Id(() => "t").Prop(() => "status").Op(() => "===", () => Js.Str(() => "paused")), () => Js.Str(() => loc.TimersStatusPaused),
                () => Js.Ternary(() => Js.Id(() => "t").Prop(() => "status").Op(() => "===", () => Js.Str(() => "triggered")), () => Js.Str(() => loc.TimersStatusTriggered), () => Js.Str(() => loc.TimersStatusCancelled))))))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "className"), () => Js.Str(() => "timer-card")))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "innerHTML"), () => BuildCardHtml(loc)))
            .Add(() => Js.Id(() => "grid").Call(() => "appendChild", () => Js.Id(() => "card")).Stmt());

        var thenBody = Js.Block()
            .Add(() => Js.Const(() => "grid", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "timers-grid"))))
            .Add(() => Js.Assign(() => Js.Id(() => "grid").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "total-count")).Prop(() => "textContent"), () => Js.Id(() => "data").Prop(() => "length")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "data").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "grid").Prop(() => "innerHTML"), () => Js.Str(() => $"<div class='empty-state'>{loc.TimersEmptyState}</div>"))
                    }
                )},
                { (null, new List<JsSyntax>
                    {
                        Js.Id(() => "data").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "t" }, () => forEachBody)).Stmt()
                    }
                )}
            }));

        var loadTimersBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/timers/list")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => thenBody)).Stmt());

        return Js.Block()
            .Add(() => Js.Func(() => "loadTimers", () => new List<string>(), () => loadTimersBody))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadTimers").Invoke())));
    }

    private static JsSyntax BuildCardHtml(DefaultLocalizationBase loc)
    {
        var typeBadge = Js.Ternary(
            () => Js.Id(() => "t").Prop(() => "type").Op(() => "===", () => Js.Str(() => "recurring")),
            () => Js.Str(() => "<span class='timer-type-badge'>").Op(() => "+", () => (JsSyntax)Js.Str(() => loc.TimersTypeRecurring)).Op(() => "+", () => (JsSyntax)Js.Str(() => "</span>")),
            () => Js.Str(() => ""));

        var calendarBadge = Js.Str(() => "<span class='calendar-badge'>")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "t").Prop(() => "calendarName"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</span>"));

        var calendarInfo = Js.Ternary(
            () => Js.Id(() => "t").Prop(() => "calendarDescription"),
            () => Js.Str(() => $"<div class='timer-info'><span class='timer-info-label'>{loc.TimersCalendarLabel}</span>").Op(() => "+", () => (JsSyntax)Js.Id(() => "t").Prop(() => "calendarDescription")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>")),
            () => Js.Str(() => ""));

        return Js.Str(() => "<div class='timer-header'><div class='timer-name'>")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "t").Prop(() => "name"))
            .Op(() => "+", () => (JsSyntax)typeBadge)
            .Op(() => "+", () => (JsSyntax)calendarBadge)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div><span class='timer-status "))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "statusClass"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "'>"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "statusText"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</span></div><div class='timer-info'><span class='timer-info-label'>{loc.TimersTriggerTimeLabel}</span>"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "t").Prop(() => "triggerTimeFormatted"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>"))
            .Op(() => "+", () => (JsSyntax)calendarInfo)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"<div class='timer-info'><span class='timer-info-label'>{loc.TimersTriggeredCountLabel}</span>"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "t").Prop(() => "timesTriggered"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>"));
    }
}
