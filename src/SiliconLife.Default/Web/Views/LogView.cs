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

public class LogView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as LogViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.PageTitleLogs, "logs", vm.Localization, body, GetScripts(vm.Localization), GetStyles(), helpTopicId: "logging");
    }

    private static H RenderBody(LogViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1(vm.Localization.LogsPageHeader),
                H.Div(
                    H.Span(string.Format(vm.Localization.LogsTotalCount, "")).Id("total-count").Class("stat-value")
                ).Class("page-stat")
            ).Class("page-header"),
            H.Div(
                H.Div(
                    H.Label(vm.Localization.LogsStartTime).Attr("for", "start-date").Class("filter-label"),
                    H.Input().Attr("type", "datetime-local").Id("start-date").Class("filter-datetime")
                ).Class("filter-group"),
                H.Div(
                    H.Label(vm.Localization.LogsEndTime).Attr("for", "end-date").Class("filter-label"),
                    H.Input().Attr("type", "datetime-local").Id("end-date").Class("filter-datetime")
                ).Class("filter-group"),
                H.Div(
                    H.Label(vm.Localization.LogsLevelAll).Attr("for", "log-level").Class("filter-label"),
                    H.Select(
                        H.Option(vm.Localization.LogsLevelAll).Value("")
                    ).Id("log-level").Class("filter-select")
                ).Class("filter-group"),
                H.Div(
                    H.Label(vm.Localization.LogsBeingFilter).Attr("for", "being-filter").Class("filter-label"),
                    H.Select(
                        H.Option(vm.Localization.LogsAllBeings).Value(""),
                        H.Option(vm.Localization.LogsSystemOnly).Value("system")
                    ).Id("being-filter").Class("filter-select")
                ).Class("filter-group"),
                H.Button(vm.Localization.LogsFilterButton).OnClick("loadLogs()").Class("filter-btn")
            ).Class("filter-bar"),
            H.Div(
                H.Div().Id("logs-list").Class("logs-list")
            ).Class("card logs-card"),
            H.Div(
                H.Button(vm.Localization.LogsPrevPage).Id("prev-btn").OnClick("prevPage()").Class("page-btn"),
                H.Span("").Id("page-info").Class("page-info"),
                H.Button(vm.Localization.LogsNextPage).Id("next-btn").OnClick("nextPage()").Class("page-btn")
            ).Class("pagination")
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
            .Selector(".filter-bar")
                .Property("display", "flex")
                .Property("gap", "12px")
                .Property("margin-bottom", "20px")
                .Property("flex-wrap", "wrap")
                .Property("align-items", "flex-end")
            .EndSelector()
            .Selector(".filter-group")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "4px")
            .EndSelector()
            .Selector(".filter-label")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".filter-select, .filter-datetime")
                .Property("padding", "8px 12px")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-primary)")
                .Property("font-size", "14px")
            .EndSelector()
            .Selector(".filter-btn")
                .Property("padding", "8px 16px")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#fff")
                .Property("border", "none")
                .Property("border-radius", "6px")
                .Property("cursor", "pointer")
                .Property("font-size", "14px")
            .EndSelector()
            .Selector(".filter-btn:hover")
                .Property("opacity", "0.9")
            .EndSelector()
            .Selector(".logs-card")
                .Property("min-height", "400px")
            .EndSelector()
            .Selector(".logs-list")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "8px")
            .EndSelector()
            .Selector(".log-item")
                .Property("padding", "12px 16px")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "8px")
                .Property("background", "var(--bg-card)")
                .Property("cursor", "pointer")
                .Property("transition", "background 0.2s")
            .EndSelector()
            .Selector(".log-item:hover")
                .Property("background", "var(--bg-secondary, rgba(0,0,0,0.05))")
            .EndSelector()
            .Selector(".log-header")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "12px")
                .Property("margin-bottom", "6px")
            .EndSelector()
            .Selector(".log-level")
                .Property("padding", "2px 8px")
                .Property("border-radius", "4px")
                .Property("font-size", "11px")
                .Property("font-weight", "bold")
                .Property("text-transform", "uppercase")
            .EndSelector()
            .Selector(".log-level.trace")
                .Property("background", "rgba(128,128,128,0.2)")
                .Property("color", "#888")
            .EndSelector()
            .Selector(".log-level.debug")
                .Property("background", "rgba(77,150,255,0.15)")
                .Property("color", "#4d96ff")
            .EndSelector()
            .Selector(".log-level.info")
                .Property("background", "rgba(107,203,119,0.15)")
                .Property("color", "var(--accent-success)")
            .EndSelector()
            .Selector(".log-level.warning")
                .Property("background", "rgba(255,193,7,0.15)")
                .Property("color", "#ffc107")
            .EndSelector()
            .Selector(".log-level.error")
                .Property("background", "rgba(255,82,82,0.15)")
                .Property("color", "#ff5252")
            .EndSelector()
            .Selector(".log-time")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".log-category")
                .Property("font-size", "11px")
                .Property("color", "var(--text-secondary)")
                .Property("background", "rgba(156,136,255,0.15)")
                .Property("padding", "2px 6px")
                .Property("border-radius", "4px")
            .EndSelector()
            .Selector(".log-message")
                .Property("font-size", "14px")
                .Property("color", "var(--text-primary)")
                .Property("word-break", "break-word")
            .EndSelector()
            .Selector(".log-exception")
                .Property("margin-top", "8px")
                .Property("padding", "8px 12px")
                .Property("background", "rgba(255,82,82,0.1)")
                .Property("border-radius", "4px")
                .Property("font-size", "12px")
                .Property("color", "#ff5252")
                .Property("font-family", "monospace")
                .Property("white-space", "pre-wrap")
                .Property("word-break", "break-all")
                .Property("max-height", "150px")
                .Property("overflow-y", "auto")
                .Property("display", "none")
            .EndSelector()
            .Selector(".log-exception.expanded")
                .Property("display", "block")
            .EndSelector()
            .Selector(".pagination")
                .Property("display", "flex")
                .Property("justify-content", "center")
                .Property("align-items", "center")
                .Property("gap", "16px")
                .Property("margin-top", "20px")
            .EndSelector()
            .Selector(".page-btn")
                .Property("padding", "8px 16px")
                .Property("background", "var(--bg-card)")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "6px")
                .Property("cursor", "pointer")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".page-btn:disabled")
                .Property("opacity", "0.5")
                .Property("cursor", "not-allowed")
            .EndSelector()
            .Selector(".page-info")
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".empty-state")
                .Property("text-align", "center")
                .Property("padding", "60px 20px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".empty-icon")
                .Property("font-size", "48px")
                .Property("margin-bottom", "16px")
            .EndSelector();
    }

    private static JsSyntax GetScripts(DefaultLocalizationBase loc)
    {
        var forEachBody = Js.Block()
            .Add(() => Js.Const(() => "item", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "className"), () => Js.Str(() => "log-item")))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "onclick"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "toggleException").Invoke(() => Js.Id(() => "item")).Stmt())))
            .Add(() => Js.Assign(() => Js.Id(() => "item").Prop(() => "innerHTML"), () => BuildLogItemHtml(loc)))
            .Add(() => Js.Id(() => "list").Call(() => "appendChild", () => Js.Id(() => "item")).Stmt());

        var thenBody = Js.Block()
            .Add(() => Js.Const(() => "list", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "logs-list"))))
            .Add(() => Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "total-count")).Prop(() => "textContent"), () => Js.Id(() => "data").Prop(() => "total")))
            .Add(() => Js.Assign(() => Js.Id(() => "currentPage"), () => Js.Id(() => "data").Prop(() => "page")))
            .Add(() => Js.Assign(() => Js.Id(() => "totalPages"), () => Js.Id(() => "data").Prop(() => "totalPages")))
            .Add(() => Js.Id(() => "updatePagination").Invoke().Stmt())
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "data").Prop(() => "logs").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => $"<div class='empty-state'><div class='empty-icon'>📋</div><div>{loc.LogsEmptyState}</div></div>"))
                    }
                )},
                { (null, new List<JsSyntax>
                    {
                        Js.Id(() => "data").Prop(() => "logs").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "log" }, () => forEachBody)).Stmt()
                    }
                )}
            }));

        var loadLogsBody = Js.Block()
            .Add(() => Js.Const(() => "level", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "log-level")).Prop(() => "value")))
            .Add(() => Js.Const(() => "beingId", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "being-filter")).Prop(() => "value")))
            .Add(() => Js.Const(() => "startDate", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "start-date")).Prop(() => "value")))
            .Add(() => Js.Const(() => "endDate", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "end-date")).Prop(() => "value")))
            .Add(() => Js.Const(() => "params", () => Js.Str(() => "?page=").Op(() => "+", () => (JsSyntax)Js.Id(() => "currentPage")).Op(() => "+", () => (JsSyntax)Js.Str(() => "&level=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "level")).Op(() => "+", () => (JsSyntax)Js.Str(() => "&beingId=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "beingId")).Op(() => "+", () => (JsSyntax)Js.Str(() => "&startDate=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "startDate")).Op(() => "+", () => (JsSyntax)Js.Str(() => "&endDate=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "endDate"))))
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/logs/list").Op(() => "+", () => (JsSyntax)Js.Id(() => "params"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => thenBody)).Stmt());

        var updatePaginationBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "page-info")).Prop(() => "textContent"), () => Js.Str(() => "").Op(() => "+", () => (JsSyntax)Js.Id(() => "currentPage")).Op(() => "+", () => (JsSyntax)Js.Str(() => " / ")).Op(() => "+", () => (JsSyntax)Js.Id(() => "totalPages"))))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "prev-btn")).Prop(() => "disabled"), () => Js.Id(() => "currentPage").Op(() => "<=", () => Js.Num(() => "1"))))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "next-btn")).Prop(() => "disabled"), () => Js.Id(() => "currentPage").Op(() => ">=", () => Js.Id(() => "totalPages"))));

        var prevPageBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "currentPage").Op(() => ">", () => Js.Num(() => "1")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "currentPage"), () => Js.Id(() => "currentPage").Op(() => "-", () => Js.Num(() => "1"))),
                        Js.Id(() => "loadLogs").Invoke().Stmt()
                    }
                )}
            }));

        var nextPageBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "currentPage").Op(() => "<", () => Js.Id(() => "totalPages")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "currentPage"), () => Js.Id(() => "currentPage").Op(() => "+", () => Js.Num(() => "1"))),
                        Js.Id(() => "loadLogs").Invoke().Stmt()
                    }
                )}
            }));

        var toggleExceptionBody = Js.Block()
            .Add(() => Js.Const(() => "exception", () => Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => ".log-exception"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "exception"), new List<JsSyntax>
                    {
                        Js.Id(() => "exception").Prop(() => "classList").Call(() => "toggle", () => Js.Str(() => "expanded")).Stmt()
                    }
                )}
            }));

        return Js.Block()
            .Add(() => Js.Let(() => "currentPage", () => Js.Num(() => "1")))
            .Add(() => Js.Let(() => "totalPages", () => Js.Num(() => "1")))
            .Add(() => Js.Func(() => "loadLogs", () => new List<string>(), () => loadLogsBody))
            .Add(() => Js.Func(() => "loadBeings", () => new List<string>(), () => Js.Block()
                .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/logs/beings"))
                    .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json")))
                    .Call(() => "then", () => Js.Arrow(() => new List<string> { "beings" }, () => Js.Block()
                        .Add(() => Js.Const(() => "select", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "being-filter"))))
                        .Add(() => Js.Id(() => "beings").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "being" }, () => Js.Block()
                            .Add(() => Js.Const(() => "option", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "option"))))
                            .Add(() => Js.Assign(() => Js.Id(() => "option").Prop(() => "value"), () => Js.Id(() => "being").Prop(() => "id")))
                            .Add(() => Js.Assign(() => Js.Id(() => "option").Prop(() => "textContent"), () => Js.Id(() => "being").Prop(() => "displayName")))
                            .Add(() => Js.Id(() => "select").Call(() => "appendChild", () => Js.Id(() => "option")).Stmt())
                        )).Stmt())
                    )).Stmt())
            ))
            .Add(() => Js.Func(() => "loadLevels", () => new List<string>(), () => Js.Block()
                .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/logs/levels"))
                    .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json")))
                    .Call(() => "then", () => Js.Arrow(() => new List<string> { "levels" }, () => Js.Block()
                        .Add(() => Js.Const(() => "select", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "log-level"))))
                        .Add(() => Js.Id(() => "levels").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "level" }, () => Js.Block()
                            .Add(() => Js.Const(() => "option", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "option"))))
                            .Add(() => Js.Assign(() => Js.Id(() => "option").Prop(() => "value"), () => Js.Id(() => "level").Prop(() => "value")))
                            .Add(() => Js.Assign(() => Js.Id(() => "option").Prop(() => "textContent"), () => Js.Id(() => "level").Prop(() => "displayName")))
                            .Add(() => Js.Id(() => "select").Call(() => "appendChild", () => Js.Id(() => "option")).Stmt())
                        )).Stmt())
                    )).Stmt())
            ))
            .Add(() => Js.Func(() => "updatePagination", () => new List<string>(), () => updatePaginationBody))
            .Add(() => Js.Func(() => "prevPage", () => new List<string>(), () => prevPageBody))
            .Add(() => Js.Func(() => "nextPage", () => new List<string>(), () => nextPageBody))
            .Add(() => Js.Func(() => "toggleException", () => new List<string> { "item" }, () => toggleExceptionBody))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Block()
                .Add(() => Js.Id(() => "loadLevels").Invoke().Stmt())
                .Add(() => Js.Id(() => "loadBeings").Invoke().Stmt())
                .Add(() => Js.Id(() => "loadLogs").Invoke().Stmt())
            )));
    }

    private static JsSyntax BuildLogItemHtml(DefaultLocalizationBase loc)
    {
        var levelClass = Js.Str(() => "log-level ")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "log").Prop(() => "level").Call(() => "toLowerCase"));

        var timeStr = Js.Id(() => "log").Prop(() => "timestamp")
            .Call(() => "replace", () => Js.Str(() => "T"), () => Js.Str(() => " "))
            .Call(() => "substring", () => Js.Num(() => "0"), () => Js.Num(() => "19"));

        var exceptionHtml = Js.Ternary(
            () => Js.Id(() => "log").Prop(() => "exception"),
            () => Js.Str(() => $"<div class='log-exception'><strong>{loc.LogsExceptionLabel}</strong><br>").Op(() => "+", () => (JsSyntax)Js.Id(() => "log").Prop(() => "exception")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>")),
            () => Js.Str(() => ""));

        return Js.Str(() => "<div class=\"log-header\"><span class=\"")
            .Op(() => "+", () => (JsSyntax)levelClass)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "\">"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "log").Prop(() => "level"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</span><span class=\"log-time\">"))
            .Op(() => "+", () => (JsSyntax)timeStr)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</span><span class=\"log-category\">"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "log").Prop(() => "category"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</span></div><div class=\"log-message\">"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "log").Prop(() => "message"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>"))
            .Op(() => "+", () => (JsSyntax)exceptionHtml);
    }
}
