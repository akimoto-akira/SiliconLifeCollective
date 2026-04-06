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

public class MemoryView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as MemoryViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, "记忆浏览 - Silicon Life Collective", "memory", body, GetScripts());
    }

    private static H RenderBody(MemoryViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1("记忆浏览")
            ).Class("page-header"),
            H.Div(
                H.Div().Id("memory-list").Class("memory-list")
            ).Class("card")
        ).Class("page-content");
    }

    private static JsSyntax GetScripts()
    {
        var thenBody = Js.Block()
            .Add(() => Js.Const(() => "list", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "memory-list"))))
            .Add(() => Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => "<p>暂无记忆数据</p>")));

        var loadMemoriesBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/memory/list")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => thenBody)).Stmt());

        return Js.Block()
            .Add(() => Js.Func(() => "loadMemories", () => new List<string>(), () => loadMemoriesBody))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadMemories").Invoke())));
    }
}
