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

    private static H RenderBody(DashboardViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1("仪表盘")
            ).Class("page-header"),
            H.Div(
                H.Div(
                    H.H3("硅基人数量"),
                    H.Div("0").Id("being-count").Class("stat-value")
                ).Class("stat-card"),
                H.Div(
                    H.H3("活跃硅基人"),
                    H.Div("0").Id("active-beings").Class("stat-value")
                ).Class("stat-card"),
                H.Div(
                    H.H3("运行时间"),
                    H.Div("00:00:00").Id("uptime").Class("stat-value")
                ).Class("stat-card"),
                H.Div(
                    H.H3("内存占用"),
                    H.Div("0 MB").Id("memory").Class("stat-value")
                ).Class("stat-card")
            ).Class("stats-grid"),
            H.Div(
                H.H3("消息频率"),
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
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "uptime")).Prop(() => "textContent"), () => Js.Id(() => "data").Prop(() => "uptime")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "memory")).Prop(() => "textContent"), () => Js.Id(() => "data").Prop(() => "memoryMB").Op(() => "+", () => (JsSyntax)Js.Str(() => " MB"))));

        var updateStatsBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/dashboard/stats")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => updateStatsThenBody)).Stmt());

        var updateChartThenBody = Js.Block()
            .Add(() => Js.Const(() => "svg", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "message-chart"))))
            .Add(() => Js.Assign(() => Js.Id(() => "svg").Prop(() => "innerHTML"), () => Js.Str(() => "")));

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
