﻿// Copyright (c) 2026 Hoshino Kennji
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

public class MemoryView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as MemoryViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        var scripts = GetScripts(vm.Localization);
        var beingNameScript = GetBeingNameScript();
        // Merge scripts: declare variables first, then execute init logic, finally define functions
        var combinedScripts = Js.Block()
            .Add(() => Js.Let(() => "currentBeingId", () => Js.Null()))
            .Add(() => beingNameScript)
            .Add(() => scripts);
        return RenderPage(vm.Skin, vm.Localization.PageTitleMemory, "memory", vm.Localization, body, combinedScripts, helpTopicId: "memory");
    }

    private static H RenderBody(MemoryViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1(vm.Localization.MemoryPageHeader).Id("page-title"),
                H.Div().Id("being-name").Class("being-name").Style("font-size: 14px; color: var(--text-secondary); margin-top: 5px;")
            ).Class("page-header"),
            // Stats panel first row
            H.Div(
                H.Div(H.Div("📊 " + vm.Localization.MemoryStatTotal).Class("stat-label"), H.Div().Id("stat-total").Class("stat-value").Text("0")).Class("stat-card"),
                H.Div(H.Div("📝 " + vm.Localization.MemoryFilterSummaryOnly).Class("stat-label"), H.Div().Id("stat-summary").Class("stat-value").Text("0")).Class("stat-card"),
                H.Div(H.Div("📄 " + vm.Localization.MemoryFilterOriginalOnly).Class("stat-label"), H.Div().Id("stat-original").Class("stat-value").Text("0")).Class("stat-card"),
                H.Div(H.Div("📅 " + vm.Localization.MemoryStatOldest).Class("stat-label"), H.Div().Id("stat-oldest").Class("stat-value").Text("-")).Class("stat-card"),
                H.Div(H.Div("🕐 " + vm.Localization.MemoryStatNewest).Class("stat-label"), H.Div().Id("stat-newest").Class("stat-value").Text("-")).Class("stat-card")
            ).Class("stats-grid").Style("display: grid; grid-template-columns: repeat(5, 1fr); gap: 15px; margin-bottom: 15px;"),
            // Timeline
            H.Div(H.Div().Id("memory-timeline").Class("memory-timeline")).Class("card"),
            // Detail modal
            H.Div(
                H.Div(
                    H.Div(
                        H.H3(vm.Localization.MemoryDetailTitle).Style("margin: 0;"),
                        H.Button("×").Id("detail-close").Class("btn btn-secondary").Style("font-size: 20px; padding: 0 8px;")
                    ).Style("display: flex; justify-content: space-between; align-items: center; margin-bottom: 15px;"),
                    H.Div().Id("detail-content")
                ).Class("modal-content").Style("background: var(--bg-primary); padding: 20px; border-radius: 8px; max-width: 600px; width: 90%; max-height: 80vh; overflow-y: auto;")
            ).Id("detail-modal").Class("modal").Style("display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0,0,0,0.5); z-index: 1000; justify-content: center; align-items: center;")
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
                    Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "memory-timeline")).Prop(() => "innerHTML"), () => Js.Id(() => "result").Prop(() => "html"))
                })
            }));

        var loadStatsThenBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "stat-total")).Prop(() => "textContent"), () => Js.Id(() => "result").Prop(() => "data").Prop(() => "totalEntries")),
                    Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "stat-summary")).Prop(() => "textContent"), () => Js.Id(() => "result").Prop(() => "data").Prop(() => "summaryCount")),
                    Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "stat-original")).Prop(() => "textContent"), () => Js.Id(() => "result").Prop(() => "data").Prop(() => "originalCount")),
                    Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "stat-oldest")).Prop(() => "textContent"), () => Js.Id(() => "result").Prop(() => "data").Prop(() => "oldestEntry")),
                    Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "stat-newest")).Prop(() => "textContent"), () => Js.Id(() => "result").Prop(() => "data").Prop(() => "newestEntry"))
                })
            }));

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
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/memory/timeline-html?").Op(() => "+", () => Js.Id(() => "params").Call(() => "toString"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => (JsSyntax)loadMemoriesThenBody)).Stmt());

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

        var showDetailBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "currentBeingId").Op(() => "===", () => Js.Null()), new List<JsSyntax>
                {
                    Js.Return(() => Js.Id(() => "undefined"))
                })
            }))
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/memory/detail/").Op(() => "+", () => Js.Id(() => "id")).Op(() => "+", () => Js.Str(() => "?beingId=")).Op(() => "+", () => Js.Id(() => "currentBeingId"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => (JsSyntax)Js.Block()
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
                    {
                        Js.Const(() => "d", () => Js.Id(() => "result").Prop(() => "data")),
                        Js.Let(() => "detailHtml", () => Js.Str(() => $"<div style='margin-bottom: 10px;'><strong>{loc.MemoryDetailId}:</strong> ").Op(() => "+", () => Js.Id(() => "d").Prop(() => "id")).Op(() => "+", () => Js.Str(() => "</div>"))),
                        Js.Assign(() => Js.Id(() => "detailHtml"), () => Js.Id(() => "detailHtml").Op(() => "+", () => Js.Str(() => $"<div style='margin-bottom: 10px;'><strong>{loc.MemoryDetailCreatedAt}:</strong> ").Op(() => "+", () => Js.Id(() => "d").Prop(() => "timestampDisplay")).Op(() => "+", () => Js.Str(() => "</div>")))),
                        Js.Assign(() => Js.Id(() => "detailHtml"), () => Js.Id(() => "detailHtml").Op(() => "+", () => Js.Str(() => $"<div style='margin-bottom: 10px;'><strong>{loc.MemoryDetailContent}:</strong></div>"))),
                        Js.Assign(() => Js.Id(() => "detailHtml"), () => Js.Id(() => "detailHtml").Op(() => "+", () => Js.Str(() => "<div style='background: var(--bg-secondary); padding: 12px; border-radius: 6px; line-height: 1.6; margin-bottom: 10px;'>").Op(() => "+", () => Js.Id(() => "d").Prop(() => "content")).Op(() => "+", () => Js.Str(() => "</div>")))),
                        Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            (Js.Id(() => "d").Prop(() => "keywords").Prop(() => "length").Op(() => ">", () => Js.Num(() => "0")), new List<JsSyntax>
                            {
                                Js.Assign(() => Js.Id(() => "detailHtml"), () => Js.Id(() => "detailHtml").Op(() => "+", () => Js.Str(() => $"<div style='margin-bottom: 10px;'><strong>{loc.MemoryDetailKeywords}:</strong> ").Op(() => "+", () => Js.Id(() => "d").Prop(() => "keywords").Call(() => "join", () => Js.Str(() => ", "))).Op(() => "+", () => Js.Str(() => "</div>"))))
                            })
                        }),
                        Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            (Js.Id(() => "d").Prop(() => "relatedBeings").Prop(() => "length").Op(() => ">", () => Js.Num(() => "0")), new List<JsSyntax>
                            {
                                Js.Assign(() => Js.Id(() => "detailHtml"), () => Js.Id(() => "detailHtml").Op(() => "+", () => Js.Str(() => $"<div style='margin-bottom: 10px;'><strong>{loc.MemoryDetailRelatedBeings}:</strong> ").Op(() => "+", () => Js.Id(() => "d").Prop(() => "relatedBeings").Prop(() => "length")).Op(() => "+", () => Js.Str(() => "</div>"))))
                            })
                        }),
                        Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "detail-content")).Prop(() => "innerHTML"), () => Js.Id(() => "detailHtml")),
                        Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "detail-modal")).Prop(() => "style").Prop(() => "display"), () => Js.Str(() => "flex"))
                    })
                }))
            )).Stmt());

        // Show summary detail in modal
        var showSummaryDetailBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "currentBeingId").Op(() => "===", () => Js.Null()), new List<JsSyntax>
                {
                    Js.Return(() => Js.Id(() => "undefined"))
                })
            }))
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/memory/detail/").Op(() => "+", () => Js.Id(() => "id")).Op(() => "+", () => Js.Str(() => "?beingId=")).Op(() => "+", () => Js.Id(() => "currentBeingId"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => (JsSyntax)Js.Block()
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
                    {
                        Js.Const(() => "d", () => Js.Id(() => "result").Prop(() => "data")),
                        Js.Let(() => "detailHtml", () => Js.Str(() => $"<div style='margin-bottom: 10px;'><strong>{loc.MemoryDetailCreatedAt}:</strong> ").Op(() => "+", () => Js.Id(() => "d").Prop(() => "timestampDisplay")).Op(() => "+", () => Js.Str(() => "</div>"))),
                        Js.Assign(() => Js.Id(() => "detailHtml"), () => Js.Id(() => "detailHtml").Op(() => "+", () => Js.Str(() => $"<div style='margin-bottom: 10px;'><strong>{loc.MemoryDetailContent}:</strong></div>"))),
                        Js.Assign(() => Js.Id(() => "detailHtml"), () => Js.Id(() => "detailHtml").Op(() => "+", () => Js.Str(() => "<div style='background: var(--bg-secondary); padding: 16px; border-radius: 8px; line-height: 1.8; font-size: 14px;'>").Op(() => "+", () => Js.Id(() => "d").Prop(() => "content")).Op(() => "+", () => Js.Str(() => "</div>")))),
                        Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            (Js.Id(() => "d").Prop(() => "keywords").Prop(() => "length").Op(() => ">", () => Js.Num(() => "0")), new List<JsSyntax>
                            {
                                Js.Assign(() => Js.Id(() => "detailHtml"), () => Js.Id(() => "detailHtml").Op(() => "+", () => Js.Str(() => "<div style='margin-top: 15px; margin-bottom: 10px;'><strong>" + loc.MemoryDetailKeywords + ":</strong> ")).Op(() => "+", () => Js.Id(() => "d").Prop(() => "keywords").Call(() => "join", () => Js.Str(() => ", "))).Op(() => "+", () => Js.Str(() => "</div>")))
                            })
                        }),
                        Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "detail-content")).Prop(() => "innerHTML"), () => Js.Id(() => "detailHtml")),
                        Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "detail-modal")).Prop(() => "style").Prop(() => "display"), () => Js.Str(() => "flex"))
                    })
                }))
            )).Stmt());

        var resetFiltersBody = Js.Block()
            .Add(() => Js.Id(() => "loadMemories").Invoke().Stmt());

        // Init block
        var initBlock = Js.Block()
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "detail-close")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string>(), () => (JsSyntax)Js.Block()
                .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "detail-modal")).Prop(() => "style").Prop(() => "display"), () => Js.Str(() => "none")))
            )).Stmt())
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "detail-modal")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string> { "e" }, () => (JsSyntax)Js.Block()
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "e").Prop(() => "target").Op(() => "===", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "detail-modal"))), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "detail-modal")).Prop(() => "style").Prop(() => "display"), () => Js.Str(() => "none"))
                    })
                }))
            )).Stmt())
            // Event delegation: click memory card to show detail
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "memory-timeline")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string> { "e" }, () => (JsSyntax)Js.Block()
                .Add(() => Js.Const(() => "card", () => Js.Id(() => "e").Prop(() => "target").Call(() => "closest", () => Js.Str(() => ".memory-card"))))
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "card"), new List<JsSyntax>
                    {
                        Js.Id(() => "showDetail").Invoke(() => Js.Id(() => "card").Call(() => "getAttribute", () => Js.Str(() => "data-id"))).Stmt()
                    })
                }))
            )).Stmt());

        return Js.Block()
            .Add(() => Js.Func(() => "loadBeings", () => new List<string>(), () => loadBeingsBody))
            .Add(() => Js.Func(() => "loadMemories", () => new List<string>(), () => loadMemoriesBody))
            .Add(() => Js.Func(() => "loadStats", () => new List<string>(), () => loadStatsBody))
            .Add(() => Js.Func(() => "showDetail", () => new List<string> { "id" }, () => showDetailBody))
            .Add(() => Js.Func(() => "showSummaryDetail", () => new List<string> { "id" }, () => showSummaryDetailBody))
            .Add(() => Js.Func(() => "resetFilters", () => new List<string>(), () => resetFiltersBody))
            .Add(() => Js.Id(() => "document").Call(() => "addEventListener", () => Js.Str(() => "DOMContentLoaded"), () => Js.Arrow(() => new List<string>(), () => (JsSyntax)initBlock)).Stmt());
    }

    private static JsSyntax GetBeingNameScript()
    {
        // Read beingId from URL and display silicon being name
        return Js.Block()
            .Add(() => Js.Const(() => "params", () => Js.New(() => Js.Id(() => "URLSearchParams"), () => Js.Id(() => "window").Prop(() => "location").Prop(() => "search"))))
            .Add(() => Js.Assign(() => Js.Id(() => "currentBeingId"), () => Js.Id(() => "params").Call(() => "get", () => Js.Str(() => "beingId"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "currentBeingId").Op(() => "===", () => Js.Null()), new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "window").Prop(() => "location").Prop(() => "href"), () => Js.Str(() => "/"))
                })
            }))
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/beings")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "beings" }, () => (JsSyntax)Js.Block()
                .Add(() => Js.Const(() => "being", () => Js.Id(() => "beings").Call(() => "find", () => Js.Arrow(() => new List<string> { "b" }, () => (JsSyntax)Js.Id(() => "b").Prop(() => "id").Op(() => "===", () => Js.Id(() => "currentBeingId"))))))
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "being").Op(() => "!=", () => Js.Null()), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "being-name")).Prop(() => "textContent"), () => Js.Str(() => "硅基人: ").Op(() => "+", () => Js.Id(() => "being").Prop(() => "name")))
                    })
                }))
                .Add(() => Js.Id(() => "loadMemories").Invoke().Stmt())
                .Add(() => Js.Id(() => "loadStats").Invoke().Stmt())
            )).Stmt());
    }
}
