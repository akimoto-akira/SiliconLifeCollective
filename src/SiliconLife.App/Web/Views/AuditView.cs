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

public class AuditView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as AuditViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.PageTitleAudit, "audit", vm.Localization, body, GetScripts(vm.Localization), GetStyles(), helpTopicId: "audit-log");
    }

    private static H RenderBody(AuditViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1(vm.Localization.AuditPageHeader)
            ).Class("page-header"),
            H.Div(
                H.Div(
                    H.Span(vm.Localization.AuditTotalEntries).Class("stat-label"),
                    H.Span("").Id("stat-total").Class("stat-value")
                ).Class("stat-item"),
                H.Div(
                    H.Span(vm.Localization.AuditAllowedCount).Class("stat-label"),
                    H.Span("").Id("stat-allowed").Class("stat-value")
                ).Class("stat-item"),
                H.Div(
                    H.Span(vm.Localization.AuditDeniedCount).Class("stat-label"),
                    H.Span("").Id("stat-denied").Class("stat-value")
                ).Class("stat-item"),
                H.Div(
                    H.Span(vm.Localization.AuditAskUserCount).Class("stat-label"),
                    H.Span("").Id("stat-askuser").Class("stat-value")
                ).Class("stat-item")
            ).Class("stats-bar"),
            H.Div(
                H.Div(
                    H.Label(vm.Localization.AuditPermissionType).Attr("for", "permission-type").Class("filter-label"),
                    H.Select(
                        H.Option(vm.Localization.AuditAllPermissionTypes).Value(""),
                        H.Option("NetworkAccess").Value("NetworkAccess"),
                        H.Option("CommandLine").Value("CommandLine"),
                        H.Option("FileAccess").Value("FileAccess"),
                        H.Option("Function").Value("Function"),
                        H.Option("DataAccess").Value("DataAccess")
                    ).Id("permission-type").Class("filter-select")
                ).Class("filter-group"),
                H.Div(
                    H.Label(vm.Localization.AuditResult).Attr("for", "result-filter").Class("filter-label"),
                    H.Select(
                        H.Option(vm.Localization.AuditAllResults).Value(""),
                        H.Option("Allowed").Value("Allowed"),
                        H.Option("Denied").Value("Denied"),
                        H.Option("AskUser").Value("AskUser")
                    ).Id("result-filter").Class("filter-select")
                ).Class("filter-group"),
                H.Div(
                    H.Label(vm.Localization.AuditBeing).Attr("for", "being-filter").Class("filter-label"),
                    H.Select(
                        H.Option(vm.Localization.AuditAllBeings).Value("")
                    ).Id("being-filter").Class("filter-select")
                ).Class("filter-group"),
                H.Div(
                    H.Label(vm.Localization.AuditStartTime).Attr("for", "start-date").Class("filter-label"),
                    H.Input().Attr("type", "datetime-local").Id("start-date").Class("filter-datetime")
                ).Class("filter-group"),
                H.Div(
                    H.Label(vm.Localization.AuditEndTime).Attr("for", "end-date").Class("filter-label"),
                    H.Input().Attr("type", "datetime-local").Id("end-date").Class("filter-datetime")
                ).Class("filter-group"),
                H.Button(vm.Localization.AuditFilterButton).OnClick("loadAudit()").Class("filter-btn")
            ).Class("filter-bar"),
            H.Div(
                H.Table(
                    H.Thead(
                        H.Tr(
                            H.Th(vm.Localization.AuditColumnTimestamp),
                            H.Th(vm.Localization.AuditColumnCaller),
                            H.Th(vm.Localization.AuditColumnPermissionType),
                            H.Th(vm.Localization.AuditColumnResource),
                            H.Th(vm.Localization.AuditColumnResult),
                            H.Th(vm.Localization.AuditColumnReason)
                        )
                    ),
                    H.Tbody(
                        H.Tr(
                            H.Td(vm.Localization.AuditEmptyState).Attr("colspan", "6")
                        ).Id("empty-row").Class("empty-row")
                    )
                ).Class("audit-table")
            ).Class("card"),
            H.Div(
                H.Button(vm.Localization.AuditPrevPage).Id("prev-btn").OnClick("prevPage()").Class("page-btn"),
                H.Span("").Id("page-info").Class("page-info"),
                H.Button(vm.Localization.AuditNextPage).Id("next-btn").OnClick("nextPage()").Class("page-btn")
            ).Class("pagination")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".page-header")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".stats-bar")
                .Property("display", "flex")
                .Property("gap", "20px")
                .Property("flex-wrap", "wrap")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".stat-item")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "4px")
                .Property("padding", "12px 20px")
                .Property("background", "var(--bg-card)")
                .Property("border-radius", "8px")
                .Property("border", "1px solid var(--border)")
                .Property("min-width", "120px")
                .Property("flex", "1")
            .EndSelector()
            .Selector(".stat-label")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".stat-value")
                .Property("font-size", "20px")
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
            .Selector(".audit-table")
                .Property("width", "100%")
                .Property("border-collapse", "collapse")
            .EndSelector()
            .Selector(".audit-table th")
                .Property("padding", "12px 16px")
                .Property("text-align", "left")
                .Property("font-size", "12px")
                .Property("font-weight", "600")
                .Property("color", "var(--text-muted)")
                .Property("text-transform", "uppercase")
                .Property("letter-spacing", "0.5px")
                .Property("border-bottom", "1px solid var(--border)")
            .EndSelector()
            .Selector(".audit-table td")
                .Property("padding", "10px 16px")
                .Property("font-size", "14px")
                .Property("border-bottom", "1px solid var(--border)")
                .Property("word-break", "break-all")
            .EndSelector()
            .Selector(".audit-table tbody tr:hover")
                .Property("background", "var(--bg-card)")
            .EndSelector()
            .Selector(".empty-row td")
                .Property("text-align", "center")
                .Property("padding", "60px 20px")
                .Property("color", "var(--text-secondary)")
                .Property("font-size", "16px")
            .EndSelector()
            .Selector(".result-allowed")
                .Property("color", "var(--accent-success)")
                .Property("font-weight", "600")
            .EndSelector()
            .Selector(".result-denied")
                .Property("color", "var(--accent-danger, var(--accent-error))")
                .Property("font-weight", "600")
            .EndSelector()
            .Selector(".result-askuser")
                .Property("color", "var(--accent-warning)")
                .Property("font-weight", "600")
            .EndSelector()
            .Selector(".resource-cell")
                .Property("max-width", "300px")
                .Property("overflow", "hidden")
                .Property("text-overflow", "ellipsis")
                .Property("white-space", "nowrap")
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
            .EndSelector();
    }

    private static JsSyntax GetScripts(DefaultLocalizationBase loc)
    {
        var loadSummaryThenBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "stat-total")).Prop(() => "textContent"), () => Js.Id(() => "summary").Prop(() => "total").Call(() => "toLocaleString")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "stat-allowed")).Prop(() => "textContent"), () => Js.Id(() => "summary").Prop(() => "allowed").Call(() => "toLocaleString")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "stat-denied")).Prop(() => "textContent"), () => Js.Id(() => "summary").Prop(() => "denied").Call(() => "toLocaleString")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "stat-askuser")).Prop(() => "textContent"), () => Js.Id(() => "summary").Prop(() => "askUser").Call(() => "toLocaleString")));

        var buildParamsExpr = Js.Str(() => "?page=").Op(() => "+", () => (JsSyntax)Js.Id(() => "currentPage"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "&permissionType=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "permissionType"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "&result=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "resultFilter"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "&beingId=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "beingId"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "&startDate=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "startDate"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "&endDate=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "endDate"));

        var forEachBody = Js.Block()
            .Add(() => Js.Const(() => "row", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "tr"))))
            .Add(() => Js.Assign(() => Js.Id(() => "row").Prop(() => "innerHTML"), () => BuildAuditRowHtml()))
            .Add(() => Js.Id(() => "tbody").Call(() => "appendChild", () => Js.Id(() => "row")).Stmt());

        var thenBody = Js.Block()
            .Add(() => Js.Const(() => "tbody", () => Js.Id(() => "document").Call(() => "querySelector", () => Js.Str(() => ".audit-table tbody"))))
            .Add(() => Js.Assign(() => Js.Id(() => "tbody").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.Assign(() => Js.Id(() => "currentPage"), () => Js.Id(() => "data").Prop(() => "page")))
            .Add(() => Js.Assign(() => Js.Id(() => "totalPages"), () => Js.Id(() => "data").Prop(() => "totalPages")))
            .Add(() => Js.Id(() => "updatePagination").Invoke().Stmt())
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "data").Prop(() => "entries").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "tbody").Prop(() => "innerHTML"), () => Js.Str(() => $"<tr class=\"empty-row\"><td colspan=\"6\">{loc.AuditEmptyState}</td></tr>"))
                })
            }))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "data").Prop(() => "entries").Prop(() => "length").Op(() => ">", () => Js.Num(() => "0")), new List<JsSyntax>
                {
                    Js.Id(() => "data").Prop(() => "entries").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "entry" }, () => forEachBody)).Stmt()
                })
            }));

        var loadAuditBody = Js.Block()
            .Add(() => Js.Const(() => "permissionType", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "permission-type")).Prop(() => "value")))
            .Add(() => Js.Const(() => "resultFilter", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "result-filter")).Prop(() => "value")))
            .Add(() => Js.Const(() => "beingId", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "being-filter")).Prop(() => "value")))
            .Add(() => Js.Const(() => "startDate", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "start-date")).Prop(() => "value")))
            .Add(() => Js.Const(() => "endDate", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "end-date")).Prop(() => "value")))
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/audit/list").Op(() => "+", () => (JsSyntax)buildParamsExpr)).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => thenBody)).Stmt())
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/audit/summary").Op(() => "+", () => (JsSyntax)Js.Str(() => "?beingId=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "beingId")).Op(() => "+", () => (JsSyntax)Js.Str(() => "&startDate=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "startDate")).Op(() => "+", () => (JsSyntax)Js.Str(() => "&endDate=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "endDate"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "summary" }, () => loadSummaryThenBody)).Stmt());

        var updatePaginationBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "page-info")).Prop(() => "textContent"), () => Js.Str(() => "").Op(() => "+", () => (JsSyntax)Js.Id(() => "currentPage")).Op(() => "+", () => (JsSyntax)Js.Str(() => " / ")).Op(() => "+", () => (JsSyntax)Js.Id(() => "totalPages"))))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "prev-btn")).Prop(() => "disabled"), () => Js.Id(() => "currentPage").Op(() => "<=", () => Js.Num(() => "1"))))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "next-btn")).Prop(() => "disabled"), () => Js.Id(() => "currentPage").Op(() => ">=", () => Js.Id(() => "totalPages"))));

        var prevPageBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "currentPage").Op(() => ">", () => Js.Num(() => "1")), new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "currentPage"), () => Js.Id(() => "currentPage").Op(() => "-", () => Js.Num(() => "1"))),
                    Js.Id(() => "loadAudit").Invoke().Stmt()
                })
            }));

        var nextPageBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "currentPage").Op(() => "<", () => Js.Id(() => "totalPages")), new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "currentPage"), () => Js.Id(() => "currentPage").Op(() => "+", () => Js.Num(() => "1"))),
                    Js.Id(() => "loadAudit").Invoke().Stmt()
                })
            }));

        return Js.Block()
            .Add(() => Js.Let(() => "currentPage", () => Js.Num(() => "1")))
            .Add(() => Js.Let(() => "totalPages", () => Js.Num(() => "1")))
            .Add(() => Js.Func(() => "loadAudit", () => new List<string>(), () => loadAuditBody))
            .Add(() => Js.Func(() => "loadBeings", () => new List<string>(), () => Js.Block()
                .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/audit/beings"))
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
            .Add(() => Js.Func(() => "updatePagination", () => new List<string>(), () => updatePaginationBody))
            .Add(() => Js.Func(() => "prevPage", () => new List<string>(), () => prevPageBody))
            .Add(() => Js.Func(() => "nextPage", () => new List<string>(), () => nextPageBody))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Block()
                .Add(() => Js.Id(() => "loadBeings").Invoke().Stmt())
                .Add(() => Js.Id(() => "loadAudit").Invoke().Stmt())
            )));
    }

    private static JsSyntax BuildAuditRowHtml()
    {
        var resultClass = Js.Ternary(
            () => Js.Id(() => "entry").Prop(() => "result").Op(() => "===", () => Js.Str(() => "Allowed")),
            () => Js.Str(() => "result-allowed"),
            () => Js.Ternary(
                () => Js.Id(() => "entry").Prop(() => "result").Op(() => "===", () => Js.Str(() => "Denied")),
                () => Js.Str(() => "result-denied"),
                () => Js.Str(() => "result-askuser")));

        return Js.Str(() => "<td>").Op(() => "+", () => (JsSyntax)Js.Id(() => "entry").Prop(() => "timestamp"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</td><td>")).Op(() => "+", () => (JsSyntax)Js.Id(() => "entry").Prop(() => "callerName"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</td><td>")).Op(() => "+", () => (JsSyntax)Js.Id(() => "entry").Prop(() => "permissionType"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</td><td class=\"resource-cell\" title=\"")).Op(() => "+", () => (JsSyntax)Js.Id(() => "entry").Prop(() => "resource"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "\">")).Op(() => "+", () => (JsSyntax)Js.Id(() => "entry").Prop(() => "resource"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</td><td class=\"")).Op(() => "+", () => (JsSyntax)resultClass)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "\">")).Op(() => "+", () => (JsSyntax)Js.Id(() => "entry").Prop(() => "result"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</td><td>")).Op(() => "+", () => (JsSyntax)Js.Id(() => "entry").Prop(() => "reason"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</td>"));
    }
}
