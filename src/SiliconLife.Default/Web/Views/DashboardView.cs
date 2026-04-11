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

public class DashboardView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as DashboardViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.PageTitleDashboard, "dashboard", vm.Localization, body, GetScripts(), GetStyles());
    }

    private H RenderBody(DashboardViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1(vm.Localization.DashboardPageHeader)
            ).Class("page-header"),
            H.Div(
                H.Div(
                    H.H3(vm.Localization.DashboardStatTotalBeings),
                    H.Div("0").Id("being-count").Class("stat-value")
                ).Class("stat-card"),
                H.Div(
                    H.H3(vm.Localization.DashboardStatActiveBeings),
                    H.Div("0").Id("active-beings").Class("stat-value")
                ).Class("stat-card"),
                H.Div(
                    H.H3(vm.Localization.DashboardStatUptime),
                    H.Div("00:00:00").Id("uptime").Class("stat-value")
                ).Class("stat-card"),
                H.Div(
                    H.H3(vm.Localization.DashboardStatMemory),
                    H.Div("0 MB").Id("memory").Class("stat-value")
                ).Class("stat-card")
            ).Class("stats-grid"),
            H.Div(
                H.H3(vm.Localization.DashboardChartMessageFrequency),
                H.Div(
                    H.Svg().Id("message-chart").Attr("viewBox", "0 0 800 300").Attr("preserveAspectRatio", "xMidYMid meet")
                ).Class("chart-container")
            ).Class("card")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".chart-container")
                .Property("width", "100%")
            .EndSelector()
            .Selector(".chart-container svg")
                .Property("width", "100%")
                .Property("height", "300px")
            .EndSelector();
    }

    private static JsSyntax GetScripts()
    {
        var updateStatsThenBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "being-count")).Prop(() => "textContent"), () => Js.Id(() => "data").Prop(() => "beingCount")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "active-beings")).Prop(() => "textContent"), () => Js.Id(() => "data").Prop(() => "activeBeings")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "uptime")).Prop(() => "textContent"), () => Js.Id(() => "data").Prop(() => "uptime")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "memory")).Prop(() => "textContent"), () => Js.Id(() => "data").Prop(() => "memoryMB").Op(() => "+", () => (JsSyntax)Js.Str(() => " MB"))));

        var updateStatsBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/dashboard/stats")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => updateStatsThenBody)).Stmt());

        var drawBarBody = Js.Block()
            .Add(() => Js.Let(() => "maxVal", () => Js.Id(() => "Math").Call(() => "max", () => Js.Num(() => "1"), () => Js.Id(() => "data").Prop(() => "messageCounts").Call(() => "reduce", () => Js.Arrow(() => new List<string> { "a", "b" }, () => Js.Id(() => "Math").Call(() => "max", () => Js.Id(() => "a"), () => Js.Id(() => "b"))), () => Js.Num(() => "0")))))
            .Add(() => Js.Const(() => "barWidth", () => Js.Num(() => "35")))
            .Add(() => Js.Const(() => "gap", () => Js.Num(() => "5")))
            .Add(() => Js.Const(() => "chartHeight", () => Js.Num(() => "250")))
            .Add(() => Js.Const(() => "yOffset", () => Js.Num(() => "20")))
            .Add(() => Js.Const(() => "x", () => Js.Id(() => "i").Op(() => "*", () => (JsSyntax)Js.Id(() => "barWidth").Op(() => "+", () => Js.Id(() => "gap")))))
            .Add(() => Js.Const(() => "height", () => (JsSyntax)Js.Id(() => "val").Op(() => "/", () => Js.Id(() => "maxVal")).Op(() => "*", () => Js.Id(() => "chartHeight"))))
            .Add(() => Js.Const(() => "y", () => (JsSyntax)Js.Id(() => "chartHeight").Op(() => "-", () => Js.Id(() => "height")).Op(() => "+", () => Js.Id(() => "yOffset"))))
            .Add(() => Js.Const(() => "rect", () => Js.Id(() => "document").Call(() => "createElementNS", () => Js.Str(() => "http://www.w3.org/2000/svg"), () => Js.Str(() => "rect"))))
            .Add(() => Js.Id(() => "rect").Call(() => "setAttribute", () => Js.Str(() => "x"), () => Js.Id(() => "x")).Stmt())
            .Add(() => Js.Id(() => "rect").Call(() => "setAttribute", () => Js.Str(() => "y"), () => Js.Id(() => "y")).Stmt())
            .Add(() => Js.Id(() => "rect").Call(() => "setAttribute", () => Js.Str(() => "width"), () => Js.Id(() => "barWidth")).Stmt())
            .Add(() => Js.Id(() => "rect").Call(() => "setAttribute", () => Js.Str(() => "height"), () => Js.Id(() => "height")).Stmt())
            .Add(() => Js.Id(() => "rect").Call(() => "setAttribute", () => Js.Str(() => "fill"), () => Js.Str(() => "#4a90d9")).Stmt())
            .Add(() => Js.Id(() => "svg").Call(() => "appendChild", () => Js.Id(() => "rect")).Stmt());

        var updateChartThenBody = Js.Block()
            .Add(() => Js.Const(() => "svg", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "message-chart"))))
            .Add(() => Js.Assign(() => Js.Id(() => "svg").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.Id(() => "data").Prop(() => "messageCounts").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "val", "i" }, () => drawBarBody)).Stmt());

        var updateChartBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/dashboard/metrics")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => updateChartThenBody)).Stmt());

        return Js.Block()
            .Add(() => Js.Func(() => "updateStats", () => new List<string>(), () => updateStatsBody))
            .Add(() => Js.Func(() => "updateChart", () => new List<string>(), () => updateChartBody))
            .Add(() => Js.Id(() => "setInterval").Invoke(() => Js.Id(() => "updateStats"), () => Js.Num(() => "5000")).Stmt())
            .Add(() => Js.Id(() => "setInterval").Invoke(() => Js.Id(() => "updateChart"), () => Js.Num(() => "10000")).Stmt())
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Block().Add(() => Js.Id(() => "updateStats").Invoke().Stmt()).Add(() => Js.Id(() => "updateChart").Invoke().Stmt()))));
    }
}
