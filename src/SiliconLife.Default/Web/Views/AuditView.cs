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

public class AuditView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as AuditViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.PageTitleAudit, "audit", vm.Localization, body, GetScripts(vm.Localization), GetStyles());
    }

    private static H RenderBody(AuditViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1(vm.Localization.AuditPageHeader),
                H.Button(vm.Localization.AuditExport).Id("export-btn").OnClick("exportAudit()").Class("export-btn")
            ).Class("page-header"),
            H.Div(
                H.Div(
                    H.Span(vm.Localization.AuditTotalRequests).Class("stat-label"),
                    H.Span("").Id("stat-request-count").Class("stat-value")
                ).Class("stat-item"),
                H.Div(
                    H.Span(vm.Localization.AuditPromptTokens).Class("stat-label"),
                    H.Span("").Id("stat-prompt-tokens").Class("stat-value")
                ).Class("stat-item"),
                H.Div(
                    H.Span(vm.Localization.AuditCompletionTokens).Class("stat-label"),
                    H.Span("").Id("stat-completion-tokens").Class("stat-value")
                ).Class("stat-item"),
                H.Div(
                    H.Span(vm.Localization.AuditTotalTokens).Class("stat-label"),
                    H.Span("").Id("stat-total-tokens").Class("stat-value")
                ).Class("stat-item")
            ).Class("stats-bar"),
            H.Div(
                H.Div(
                    H.Label(vm.Localization.AuditAIClientType).Attr("for", "client-type").Class("filter-label"),
                    H.Select(
                        H.Option(vm.Localization.AuditAllClientTypes).Value("")
                    ).Id("client-type").Class("filter-select")
                ).Class("filter-group"),
                H.Div(
                    H.Label(vm.Localization.AuditBeing).Attr("for", "being-select").Class("filter-label"),
                    H.Select(
                        H.Option(vm.Localization.AuditAllBeings).Value("")
                    ).Id("being-select").Class("filter-select")
                ).Class("filter-group")
            ).Class("filter-bar"),
            H.Div(
                H.Button(vm.Localization.AuditTimeToday).OnClick("setTimeRange('today')").Class("time-btn active").Id("btn-today"),
                H.Button(vm.Localization.AuditTimeWeek).OnClick("setTimeRange('week')").Class("time-btn").Id("btn-week"),
                H.Button(vm.Localization.AuditTimeMonth).OnClick("setTimeRange('month')").Class("time-btn").Id("btn-month"),
                H.Button(vm.Localization.AuditTimeYear).OnClick("setTimeRange('year')").Class("time-btn").Id("btn-year")
            ).Class("time-range-bar"),
            H.Div(
                H.H3(vm.Localization.AuditTrendTitle),
                H.Div(
                    H.Svg().Id("trend-chart").Attr("viewBox", "0 0 800 300").Attr("preserveAspectRatio", "xMidYMid meet")
                ).Class("chart-container"),
                H.Div(
                    H.Span("").Class("legend-color prompt-color"),
                    H.Span(vm.Localization.AuditTrendPrompt).Class("legend-label"),
                    H.Span("").Class("legend-color completion-color"),
                    H.Span(vm.Localization.AuditTrendCompletion).Class("legend-label"),
                    H.Span("").Class("legend-color total-color"),
                    H.Span(vm.Localization.AuditTrendTotal).Class("legend-label")
                ).Class("chart-legend")
            ).Class("card trend-card")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".page-header")
                .Property("display", "flex")
                .Property("justify-content", "space-between")
                .Property("align-items", "center")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".export-btn")
                .Property("padding", "8px 16px")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#fff")
                .Property("border", "none")
                .Property("border-radius", "6px")
                .Property("cursor", "pointer")
                .Property("font-size", "14px")
            .EndSelector()
            .Selector(".export-btn:hover")
                .Property("opacity", "0.9")
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
                .Property("margin-bottom", "16px")
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
            .Selector(".filter-select")
                .Property("padding", "8px 12px")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-primary)")
                .Property("font-size", "14px")
                .Property("min-width", "140px")
            .EndSelector()
            .Selector(".time-range-bar")
                .Property("display", "flex")
                .Property("gap", "8px")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".time-btn")
                .Property("padding", "6px 16px")
                .Property("background", "var(--bg-card)")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "6px")
                .Property("cursor", "pointer")
                .Property("color", "var(--text-primary)")
                .Property("font-size", "13px")
            .EndSelector()
            .Selector(".time-btn:hover")
                .Property("background", "var(--border)")
            .EndSelector()
            .Selector(".time-btn.active")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#fff")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".trend-card")
                .Property("padding", "16px")
            .EndSelector()
            .Selector(".chart-container")
                .Property("width", "100%")
            .EndSelector()
            .Selector(".chart-container svg")
                .Property("width", "100%")
                .Property("height", "300px")
            .EndSelector()
            .Selector(".chart-legend")
                .Property("display", "flex")
                .Property("gap", "16px")
                .Property("margin-top", "12px")
                .Property("justify-content", "center")
            .EndSelector()
            .Selector(".legend-color")
                .Property("display", "inline-block")
                .Property("width", "12px")
                .Property("height", "12px")
                .Property("border-radius", "2px")
                .Property("vertical-align", "middle")
            .EndSelector()
            .Selector(".legend-color.prompt-color")
                .Property("background", "#4a90d9")
            .EndSelector()
            .Selector(".legend-color.completion-color")
                .Property("background", "#e8725c")
            .EndSelector()
            .Selector(".legend-color.total-color")
                .Property("background", "#50c878")
            .EndSelector()
            .Selector(".legend-label")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-right", "8px")
            .EndSelector();
    }

    private static JsSyntax GetScripts(DefaultLocalizationBase loc)
    {
        var loadSummaryThenBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "stat-request-count")).Prop(() => "textContent"), () => Js.Id(() => "summary").Prop(() => "requestCount").Call(() => "toLocaleString")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "stat-prompt-tokens")).Prop(() => "textContent"), () => Js.Id(() => "summary").Prop(() => "totalPromptTokens").Call(() => "toLocaleString")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "stat-completion-tokens")).Prop(() => "textContent"), () => Js.Id(() => "summary").Prop(() => "totalCompletionTokens").Call(() => "toLocaleString")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "stat-total-tokens")).Prop(() => "textContent"), () => Js.Id(() => "summary").Prop(() => "totalTokens").Call(() => "toLocaleString")))
            .Add(() => Js.Id(() => "updateClientFilter").Invoke(() => Js.Id(() => "summary").Prop(() => "byClient")).Stmt())
            .Add(() => Js.Id(() => "updateBeingFilter").Invoke(() => Js.Id(() => "summary").Prop(() => "byBeing")).Stmt());

        var buildParamsExpr = Js.Str(() => "?timeRange=").Op(() => "+", () => (JsSyntax)Js.Id(() => "currentTimeRange")).Op(() => "+", () => (JsSyntax)Js.Str(() => "&clientType=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "clientType")).Op(() => "+", () => (JsSyntax)Js.Str(() => "&beingId=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "beingId"));

        var reduceArrow = Js.Arrow(() => new List<string> { "a", "b" }, () => (JsSyntax)Js.Id(() => "Math").Call(() => "max", () => Js.Id(() => "a"), () => Js.Id(() => "b").Prop(() => "totalTokens")));

        var drawTrendBody = Js.Block()
            .Add(() => Js.Const(() => "svg", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "trend-chart"))))
            .Add(() => Js.Assign(() => Js.Id(() => "svg").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "data").Prop(() => "points").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
                {
                    Js.Return(() => Js.Id(() => "undefined"))
                })
            }))
            .Add(() => Js.Const(() => "points", () => Js.Id(() => "data").Prop(() => "points")))
            .Add(() => Js.Const(() => "maxVal", () => Js.Id(() => "Math").Call(() => "max", () => Js.Num(() => "1"), () => Js.Id(() => "points").Call(() => "reduce", () => reduceArrow, () => Js.Num(() => "0")))))
            .Add(() => Js.Const(() => "chartW", () => Js.Num(() => "760")))
            .Add(() => Js.Const(() => "chartH", () => Js.Num(() => "240")))
            .Add(() => Js.Const(() => "padX", () => Js.Num(() => "40")))
            .Add(() => Js.Const(() => "padY", () => Js.Num(() => "30")))
            .Add(() => Js.Const(() => "stepX", () => Js.Id(() => "chartW").Op(() => "/", () => Js.Id(() => "Math").Call(() => "max", () => Js.Num(() => "1"), () => Js.Id(() => "points").Prop(() => "length").Op(() => "-", () => Js.Num(() => "1"))))))
            .Add(() => Js.Func(() => "yPos", () => new List<string> { "val" }, () => Js.Block()
                .Add(() => Js.Return(() => Js.Id(() => "padY").Op(() => "+", () => Js.Id(() => "chartH").Op(() => "-", () => Js.Id(() => "val").Op(() => "/", () => Js.Id(() => "maxVal")).Op(() => "*", () => Js.Id(() => "chartH"))))))))
            .Add(() => Js.Func(() => "drawLine", () => new List<string> { "key", "color" }, () => Js.Block()
                .Add(() => Js.Const(() => "coords", () => Js.Id(() => "points").Call(() => "map", () => Js.Arrow(() => new List<string> { "p", "i" }, () => (JsSyntax)Js.Str(() => "").Op(() => "+", () => (JsSyntax)Js.Id(() => "padX").Op(() => "+", () => Js.Id(() => "i").Op(() => "*", () => Js.Id(() => "stepX")))).Op(() => "+", () => (JsSyntax)Js.Str(() => ",")).Op(() => "+", () => (JsSyntax)Js.Id(() => "yPos").Invoke(() => new JsIndex(() => (JsSyntax)Js.Id(() => "p"), () => (JsSyntax)Js.Id(() => "key"))))))))
                .Add(() => Js.Const(() => "polyline", () => Js.Id(() => "document").Call(() => "createElementNS", () => Js.Str(() => "http://www.w3.org/2000/svg"), () => Js.Str(() => "polyline"))))
                .Add(() => Js.Id(() => "polyline").Call(() => "setAttribute", () => Js.Str(() => "points"), () => Js.Id(() => "coords").Call(() => "join", () => Js.Str(() => " "))).Stmt())
                .Add(() => Js.Id(() => "polyline").Call(() => "setAttribute", () => Js.Str(() => "fill"), () => Js.Str(() => "none")).Stmt())
                .Add(() => Js.Id(() => "polyline").Call(() => "setAttribute", () => Js.Str(() => "stroke"), () => Js.Id(() => "color")).Stmt())
                .Add(() => Js.Id(() => "polyline").Call(() => "setAttribute", () => Js.Str(() => "stroke-width"), () => Js.Str(() => "2")).Stmt())
                .Add(() => Js.Id(() => "svg").Call(() => "appendChild", () => Js.Id(() => "polyline")).Stmt())))
            .Add(() => Js.Id(() => "drawLine").Invoke(() => Js.Str(() => "promptTokens"), () => Js.Str(() => "#4a90d9")).Stmt())
            .Add(() => Js.Id(() => "drawLine").Invoke(() => Js.Str(() => "completionTokens"), () => Js.Str(() => "#e8725c")).Stmt())
            .Add(() => Js.Id(() => "drawLine").Invoke(() => Js.Str(() => "totalTokens"), () => Js.Str(() => "#50c878")).Stmt())
            .Add(() => Js.Id(() => "points").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "p", "i" }, () => (JsSyntax)Js.Block()
                .Add(() => Js.Const(() => "x", () => Js.Id(() => "padX").Op(() => "+", () => Js.Id(() => "i").Op(() => "*", () => Js.Id(() => "stepX")))))
                .Add(() => Js.Const(() => "label", () => Js.Id(() => "document").Call(() => "createElementNS", () => Js.Str(() => "http://www.w3.org/2000/svg"), () => Js.Str(() => "text"))))
                .Add(() => Js.Id(() => "label").Call(() => "setAttribute", () => Js.Str(() => "x"), () => Js.Id(() => "x")).Stmt())
                .Add(() => Js.Id(() => "label").Call(() => "setAttribute", () => Js.Str(() => "y"), () => Js.Num(() => "280")).Stmt())
                .Add(() => Js.Id(() => "label").Call(() => "setAttribute", () => Js.Str(() => "font-size"), () => Js.Str(() => "10")).Stmt())
                .Add(() => Js.Id(() => "label").Call(() => "setAttribute", () => Js.Str(() => "fill"), () => Js.Str(() => "var(--text-secondary)")).Stmt())
                .Add(() => Js.Id(() => "label").Call(() => "setAttribute", () => Js.Str(() => "text-anchor"), () => Js.Str(() => "middle")).Stmt())
                .Add(() => Js.Assign(() => Js.Id(() => "label").Prop(() => "textContent"), () => Js.Id(() => "p").Prop(() => "date").Call(() => "substring", () => Js.Num(() => "5"))))
                .Add(() => Js.Id(() => "svg").Call(() => "appendChild", () => Js.Id(() => "label")).Stmt())
            )).Stmt());

        var loadAuditBody = Js.Block()
            .Add(() => Js.Const(() => "clientType", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "client-type")).Prop(() => "value")))
            .Add(() => Js.Const(() => "beingId", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "being-select")).Prop(() => "value")))
            .Add(() => Js.Const(() => "params", () => buildParamsExpr))
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/audit/summary").Op(() => "+", () => (JsSyntax)Js.Id(() => "params"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "summary" }, () => (JsSyntax)loadSummaryThenBody)).Stmt())
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/audit/trend").Op(() => "+", () => (JsSyntax)Js.Id(() => "params"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => (JsSyntax)drawTrendBody)).Stmt());

        var updateClientFilterBody = Js.Block()
            .Add(() => Js.Const(() => "select", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "client-type"))))
            .Add(() => Js.Const(() => "currentValue", () => Js.Id(() => "select").Prop(() => "value")))
            .Add(() => Js.Assign(() => Js.Id(() => "select").Prop(() => "innerHTML"), () => Js.Str(() => $"<option value=''>{loc.AuditAllClientTypes}</option>")))
            .Add(() => Js.Id(() => "clients").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "c" }, () => (JsSyntax)Js.Block()
                .Add(() => Js.Const(() => "opt", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "option"))))
                .Add(() => Js.Assign(() => Js.Id(() => "opt").Prop(() => "value"), () => Js.Id(() => "c").Prop(() => "key")))
                .Add(() => Js.Assign(() => Js.Id(() => "opt").Prop(() => "textContent"), () => Js.Id(() => "c").Prop(() => "key")))
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "c").Prop(() => "key").Op(() => "===", () => Js.Id(() => "currentValue")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "opt").Prop(() => "selected"), () => Js.Bool(() => true))
                    })
                }))
                .Add(() => Js.Id(() => "select").Call(() => "appendChild", () => Js.Id(() => "opt")).Stmt())
            )).Stmt());

        var updateBeingFilterBody = Js.Block()
            .Add(() => Js.Const(() => "select", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "being-select"))))
            .Add(() => Js.Const(() => "currentValue", () => Js.Id(() => "select").Prop(() => "value")))
            .Add(() => Js.Assign(() => Js.Id(() => "select").Prop(() => "innerHTML"), () => Js.Str(() => $"<option value=''>{loc.AuditAllBeings}</option>")))
            .Add(() => Js.Id(() => "beings").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "b" }, () => (JsSyntax)Js.Block()
                .Add(() => Js.Const(() => "opt", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "option"))))
                .Add(() => Js.Assign(() => Js.Id(() => "opt").Prop(() => "value"), () => Js.Id(() => "b").Prop(() => "key")))
                .Add(() => Js.Assign(() => Js.Id(() => "opt").Prop(() => "textContent"), () => Js.Id(() => "b").Prop(() => "key")))
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "b").Prop(() => "key").Op(() => "===", () => Js.Id(() => "currentValue")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "opt").Prop(() => "selected"), () => Js.Bool(() => true))
                    })
                }))
                .Add(() => Js.Id(() => "select").Call(() => "appendChild", () => Js.Id(() => "opt")).Stmt())
            )).Stmt());

        var setTimeRangeBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "currentTimeRange"), () => Js.Id(() => "range")))
            .Add(() => Js.Id(() => "document").Call(() => "querySelectorAll", () => Js.Str(() => ".time-btn")).Call(() => "forEach", () => (JsSyntax)Js.Arrow(() => new List<string> { "btn" }, () => (JsSyntax)Js.Block()
                .Add(() => Js.Id(() => "btn").Prop(() => "classList").Call(() => "remove", () => Js.Str(() => "active")).Stmt())
            )).Stmt())
            .Add(() => { JsSyntax addActiveExpr = Js.Id(() => "document").Call(() => "getElementById", () => (JsSyntax)Js.Str(() => "btn-").Op(() => "+", () => (JsSyntax)Js.Id(() => "range"))).Prop(() => "classList").Call(() => "add", () => Js.Str(() => "active")); return addActiveExpr.Stmt(); })
            .Add(() => Js.Id(() => "loadAudit").Invoke().Stmt());

        var exportBody = Js.Block()
            .Add(() => Js.Const(() => "clientType", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "client-type")).Prop(() => "value")))
            .Add(() => Js.Const(() => "beingId", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "being-select")).Prop(() => "value")))
            .Add(() => Js.Const(() => "params", () => buildParamsExpr))
            .Add(() => { JsSyntax exportUrl = Js.Str(() => "/api/audit/export").Op(() => "+", () => (JsSyntax)Js.Id(() => "params")); JsSyntax hrefTarget = Js.Id(() => "window").Prop(() => "location").Prop(() => "href"); return (JsSyntax)Js.Assign(() => hrefTarget, () => exportUrl); });

        return Js.Block()
            .Add(() => Js.Let(() => "currentTimeRange", () => Js.Str(() => "month")))
            .Add(() => Js.Func(() => "loadAudit", () => new List<string>(), () => loadAuditBody))
            .Add(() => Js.Func(() => "updateClientFilter", () => new List<string> { "clients" }, () => updateClientFilterBody))
            .Add(() => Js.Func(() => "updateBeingFilter", () => new List<string> { "beings" }, () => updateBeingFilterBody))
            .Add(() => Js.Func(() => "setTimeRange", () => new List<string> { "range" }, () => setTimeRangeBody))
            .Add(() => Js.Func(() => "exportAudit", () => new List<string>(), () => exportBody))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => (JsSyntax)Js.Id(() => "loadAudit").Invoke())));
    }
}
