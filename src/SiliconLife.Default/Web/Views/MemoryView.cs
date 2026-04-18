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

public class MemoryView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as MemoryViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.PageTitleMemory, "memory", vm.Localization, body, GetScripts(vm.Localization));
    }

    private static H RenderBody(MemoryViewModel vm)
    {
        return H.Div(
            H.Div(H.H1(vm.Localization.MemoryPageHeader)).Class("page-header"),
            H.Div(
                H.Div(
                    H.Select().Id("being-selector").Class("form-select").Style("width: 200px; margin-right: 10px;"),
                    H.Input().Id("search-input").Class("form-input")
                        .Attr("type", "text")
                        .Attr("placeholder", vm.Localization.MemorySearchPlaceholder)
                        .Style("width: 300px; margin-right: 10px;"),
                    H.Button(vm.Localization.MemorySearchButton).Id("search-btn").Class("btn btn-primary")
                ).Class("toolbar").Style("display: flex; align-items: center; margin-bottom: 20px; padding: 15px; background: var(--bg-secondary); border-radius: 8px;")
            ).Class("card"),
            H.Div(
                H.Div(H.Div(vm.Localization.MemoryStatTotal).Class("stat-label"), H.Div().Id("stat-total").Class("stat-value").Text("0")).Class("stat-card"),
                H.Div(H.Div(vm.Localization.MemoryStatOldest).Class("stat-label"), H.Div().Id("stat-oldest").Class("stat-value").Text("-")).Class("stat-card"),
                H.Div(H.Div(vm.Localization.MemoryStatNewest).Class("stat-label"), H.Div().Id("stat-newest").Class("stat-value").Text("-")).Class("stat-card")
            ).Class("stats-grid").Style("display: grid; grid-template-columns: repeat(3, 1fr); gap: 15px; margin-bottom: 20px;"),
            H.Div(H.Div().Id("memory-list").Class("memory-list")).Class("card"),
            H.Div(
                H.Button(vm.Localization.MemoryPaginationPrev).Id("prev-btn").Class("btn btn-secondary").Style("margin-right: 10px;"),
                H.Span().Id("page-info").Text("Page 1 of 1").Style("margin-right: 10px;"),
                H.Button(vm.Localization.MemoryPaginationNext).Id("next-btn").Class("btn btn-secondary")
            ).Class("pagination").Style("display: flex; align-items: center; justify-content: center; margin-top: 20px; padding: 15px;")
        ).Class("page-content");
    }

    private static JsSyntax GetScripts(DefaultLocalizationBase loc)
    {
        // Then bodies - defined before use (pattern: AuditView line 275)
        var loadBeingsThenBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
                {
                    Js.Id(() => "loadMemories").Invoke().Stmt(),
                    Js.Id(() => "loadStats").Invoke().Stmt()
                })
            }));

        var loadMemoriesThenBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
                {
                    Js.Id(() => "renderMemories").Invoke(() => Js.Id(() => "result").Prop(() => "data")).Stmt(),
                    Js.Assign(() => Js.Id(() => "totalCount"), () => Js.Id(() => "result").Prop(() => "pagination").Prop(() => "totalCount")),
                    Js.Id(() => "updatePagination").Invoke().Stmt()
                })
            }));

        var loadStatsThenBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "stat-total")).Prop(() => "textContent"), () => Js.Id(() => "result").Prop(() => "data").Prop(() => "totalEntries")),
                    Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "stat-oldest")).Prop(() => "textContent"), () => Js.Id(() => "result").Prop(() => "data").Prop(() => "oldestEntry")),
                    Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "stat-newest")).Prop(() => "textContent"), () => Js.Id(() => "result").Prop(() => "data").Prop(() => "newestEntry"))
                })
            }));

        // Card block for forEach (pattern: AuditView line 427)
        var cardBlock = Js.Block()
            .Add(() => Js.Const(() => "card", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "className"), () => Js.Str(() => "memory-card")))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "style").Prop(() => "cssText"), () => Js.Str(() => "padding: 15px; margin-bottom: 10px; border: 1px solid var(--border-color); border-radius: 8px;")))
            .Add(() => Js.Let(() => "html", () => Js.Str(() => "<div style='font-size: 12px; color: var(--text-secondary); margin-bottom: 5px;'>").Op(() => "+", () => Js.Id(() => "memory").Prop(() => "timestampDisplay")).Op(() => "+", () => Js.Str(() => "</div>"))))
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Op(() => "+", () => Js.Str(() => "<div style='margin-bottom: 5px;'>").Op(() => "+", () => Js.Id(() => "memory").Prop(() => "content")).Op(() => "+", () => Js.Str(() => "</div>")))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "memory").Prop(() => "isSummary"), new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Op(() => "+", () => Js.Str(() => $"<span style='background: var(--accent-color); color: white; padding: 2px 8px; border-radius: 4px; font-size: 11px;'>{loc.MemoryIsSummaryBadge}</span>")))
                })
            }))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "innerHTML"), () => Js.Id(() => "html")))
            .Add(() => Js.Id(() => "list").Call(() => "appendChild", () => Js.Id(() => "card")).Stmt());

        // Function bodies (pattern: AuditView line 420)
        var loadBeingsBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/memory/beings")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => (JsSyntax)loadBeingsThenBody)).Stmt());

        var loadMemoriesBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "currentBeingId").Op(() => "===", () => Js.Null()), new List<JsSyntax>
                {
                    Js.Return(() => Js.Id(() => "undefined"))
                })
            }))
            .Add(() => Js.Const(() => "params", () => Js.New(() => Js.Id(() => "URLSearchParams"))))
            .Add(() => Js.Id(() => "params").Call(() => "set", () => Js.Str(() => "beingId"), () => Js.Id(() => "currentBeingId")).Stmt())
            .Add(() => Js.Id(() => "params").Call(() => "set", () => Js.Str(() => "page"), () => Js.Id(() => "currentPage")).Stmt())
            .Add(() => Js.Id(() => "params").Call(() => "set", () => Js.Str(() => "pageSize"), () => Js.Id(() => "pageSize")).Stmt())
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/memory/list?").Op(() => "+", () => Js.Id(() => "params").Call(() => "toString"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => (JsSyntax)loadMemoriesThenBody)).Stmt());

        var loadStatsBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "currentBeingId").Op(() => "===", () => Js.Null()), new List<JsSyntax>
                {
                    Js.Return(() => Js.Id(() => "undefined"))
                })
            }))
            .Add(() => Js.Const(() => "params", () => Js.New(() => Js.Id(() => "URLSearchParams"))))
            .Add(() => Js.Id(() => "params").Call(() => "set", () => Js.Str(() => "beingId"), () => Js.Id(() => "currentBeingId")).Stmt())
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/memory/stats?").Op(() => "+", () => Js.Id(() => "params").Call(() => "toString"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => (JsSyntax)loadStatsThenBody)).Stmt());

        var renderMemoriesBody = Js.Block()
            .Add(() => Js.Const(() => "list", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "memory-list"))))
            .Add(() => Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "memories").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => $"<p style='text-align: center; padding: 40px; color: var(--text-secondary);'>{loc.MemoryEmptyState}</p>")),
                    Js.Return(() => Js.Id(() => "undefined"))
                })
            }))
            .Add(() => Js.Id(() => "memories").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "memory" }, () => (JsSyntax)cardBlock)).Stmt());

        var updatePaginationBody = Js.Block()
            .Add(() => Js.Const(() => "totalPages", () => Js.Id(() => "Math").Call(() => "ceil", () => Js.Id(() => "totalCount").Op(() => "/", () => Js.Id(() => "pageSize")))))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "page-info")).Prop(() => "textContent"), () => Js.Str(() => "Page ").Op(() => "+", () => Js.Id(() => "currentPage")).Op(() => "+", () => Js.Str(() => " of ")).Op(() => "+", () => Js.Id(() => "totalPages"))));

        // Init block
        var initBlock = Js.Block()
            .Add(() => Js.Id(() => "loadBeings").Invoke().Stmt())
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "being-selector")).Call(() => "addEventListener", () => Js.Str(() => "change"), () => Js.Arrow(() => new List<string> { "e" }, () => (JsSyntax)Js.Block()
                .Add(() => Js.Assign(() => Js.Id(() => "currentBeingId"), () => Js.Id(() => "e").Prop(() => "target").Prop(() => "value")))
                .Add(() => Js.Assign(() => Js.Id(() => "currentPage"), () => Js.Num(() => "1")))
                .Add(() => Js.Id(() => "loadMemories").Invoke().Stmt())
                .Add(() => Js.Id(() => "loadStats").Invoke().Stmt())
            )).Stmt())
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "search-btn")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string>(), () => (JsSyntax)Js.Block()
                .Add(() => Js.Assign(() => Js.Id(() => "currentPage"), () => Js.Num(() => "1")))
                .Add(() => Js.Id(() => "loadMemories").Invoke().Stmt())
            )).Stmt())
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "prev-btn")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string>(), () => (JsSyntax)Js.Block()
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "currentPage").Op(() => ">", () => Js.Num(() => "1")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "currentPage"), () => Js.Id(() => "currentPage").Op(() => "-", () => Js.Num(() => "1"))),
                        Js.Id(() => "loadMemories").Invoke().Stmt()
                    })
                }))
            )).Stmt())
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "next-btn")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string>(), () => (JsSyntax)Js.Block()
                .Add(() => Js.Const(() => "totalPages", () => Js.Id(() => "Math").Call(() => "ceil", () => Js.Id(() => "totalCount").Op(() => "/", () => Js.Id(() => "pageSize")))))
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "currentPage").Op(() => "<", () => Js.Id(() => "totalPages")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "currentPage"), () => Js.Id(() => "currentPage").Op(() => "+", () => Js.Num(() => "1"))),
                        Js.Id(() => "loadMemories").Invoke().Stmt()
                    })
                }))
            )).Stmt());

        return Js.Block()
            .Add(() => Js.Let(() => "currentBeingId", () => Js.Null()))
            .Add(() => Js.Let(() => "currentPage", () => Js.Num(() => "1")))
            .Add(() => Js.Const(() => "pageSize", () => Js.Num(() => "20")))
            .Add(() => Js.Let(() => "totalCount", () => Js.Num(() => "0")))
            .Add(() => Js.Func(() => "loadBeings", () => new List<string>(), () => loadBeingsBody))
            .Add(() => Js.Func(() => "loadMemories", () => new List<string>(), () => loadMemoriesBody))
            .Add(() => Js.Func(() => "loadStats", () => new List<string>(), () => loadStatsBody))
            .Add(() => Js.Func(() => "renderMemories", () => new List<string> { "memories" }, () => renderMemoriesBody))
            .Add(() => Js.Func(() => "updatePagination", () => new List<string>(), () => updatePaginationBody))
            .Add(() => Js.Id(() => "document").Call(() => "addEventListener", () => Js.Str(() => "DOMContentLoaded"), () => Js.Arrow(() => new List<string>(), () => (JsSyntax)initBlock)).Stmt());
    }
}
