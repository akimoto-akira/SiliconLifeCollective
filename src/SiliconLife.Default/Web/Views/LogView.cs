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
        return RenderPage(vm.Skin, vm.Localization.PageTitleLogs, "logs", vm.Localization, body, GetScripts());
    }

    private static H RenderBody(LogViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1("日志查询")
            ).Class("page-header"),
            H.Div(
                H.Select(
                    H.Option("全部级别").Value(""),
                    H.Option("Info").Value("Info"),
                    H.Option("Warning").Value("Warning"),
                    H.Option("Error").Value("Error")
                ).Id("log-level"),
                H.Input().Attr("type", "date").Id("log-date"),
                H.Button("查询").OnClick("loadLogs()")
            ).Class("filter-bar"),
            H.Div(
                H.Div().Id("logs-list").Class("logs-list")
            ).Class("card")
        ).Class("page-content");
    }

    private static JsSyntax GetScripts()
    {
        var thenBody = Js.Block()
            .Add(() => Js.Const(() => "list", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "logs-list"))))
            .Add(() => Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => "<p>暂无日志</p>")));

        var loadLogsBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/logs/list")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => thenBody)).Stmt());

        return Js.Block()
            .Add(() => Js.Func(() => "loadLogs", () => new List<string>(), () => loadLogsBody))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadLogs").Invoke())));
    }
}
