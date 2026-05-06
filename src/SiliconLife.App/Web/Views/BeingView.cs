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

using SiliconLife.App.Web.Models;
using SiliconLife.App.Web;
using SiliconLife.App.Web.Component;

using SiliconLife.Common.Localization;
using SiliconLife.Collective;

namespace SiliconLife.App.Web.Views;

public class BeingView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as BeingViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.PageTitleBeings, "beings", vm.Localization, body, GetScripts(vm.Localization), GetStyles(), helpTopicId: "being-management");
    }

    private static H RenderBody(BeingViewModel vm)
    {
        var pageHeader = new DivComponent()
            .Class("page-header")
            .Add(new HeadingComponent("h1").Text(vm.Localization.BeingsPageHeader))
            .Add(new DivComponent()
                .Class("page-stat")
                .Add(new SpanComponent()
                    .Id("total-count")
                    .Class("stat-value")
                    .Text(string.Format(vm.Localization.BeingsTotalCount, ""))));

        var beingsGrid = new DivComponent()
            .Id("beings-grid")
            .Class("beings-grid");

        var detailPanel = new DivComponent()
            .Id("detail-panel")
            .Class("detail-panel")
            .Add(new DivComponent()
                .Id("detail-content")
                .Class("detail-content")
                .Add(new PComponent().Text(vm.Localization.BeingsNoSelectionPlaceholder)));

        var pageContent = new DivComponent()
            .Class("page-content")
            .Add(pageHeader)
            .Add(beingsGrid)
            .Add(detailPanel);

        return H.Div().AddRendered(pageContent.Render());
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            // Page stats
            .Selector(".page-stat")
                .Property("margin-left", "16px")
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".stat-value")
                .Property("font-weight", "600")
                .Property("color", "var(--accent-primary)")
            .EndSelector()
            // Being card grid
            .Selector(".beings-grid")
                .Property("display", "grid")
                .Property("grid-template-columns", "repeat(auto-fill, minmax(280px, 1fr))")
                .Property("gap", "var(--card-gap, 16px)")
                .Property("margin-bottom", "24px")
            .EndSelector()
            // Being card
            .Selector(".being-card")
                .Property("background", "var(--bg-secondary)")
                .Property("padding", "20px")
                .Property("border-radius", "var(--card-radius, 8px)")
                .Property("border", "1px solid var(--border-color)")
                .Property("cursor", "pointer")
                .Property("transition", "var(--transition, 0.2s ease)")
                .Property("position", "relative")
            .EndSelector()
            .Selector(".being-card:hover")
                .Property("border-color", "var(--accent-primary)")
                .Property("transform", "translateY(-2px)")
            .EndSelector()
            .Selector(".being-card.selected")
                .Property("border", "2px solid var(--accent-primary)")
            .EndSelector()
            // Card header
            .Selector(".being-header")
                .Property("display", "flex")
                .Property("justify-content", "space-between")
                .Property("align-items", "flex-start")
                .Property("margin-bottom", "12px")
            .EndSelector()
            .Selector(".being-name")
                .Property("font-size", "16px")
                .Property("font-weight", "600")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            // Status badge
            .Selector(".being-status")
                .Property("display", "inline-flex")
                .Property("align-items", "center")
                .Property("gap", "6px")
                .Property("padding", "4px 10px")
                .Property("border-radius", "12px")
                .Property("font-size", "12px")
                .Property("font-weight", "500")
            .EndSelector()
            .Selector(".being-status.idle")
                .Property("background", "rgba(245, 158, 11, 0.15)")
                .Property("color", "var(--status-warning, var(--accent-warning))")
            .EndSelector()
            .Selector(".being-status.active")
                .Property("background", "rgba(16, 185, 129, 0.15)")
                .Property("color", "var(--status-active, var(--accent-secondary))")
            .EndSelector()
            .Selector(".status-dot")
                .Property("width", "6px")
                .Property("height", "6px")
                .Property("border-radius", "50%")
                .Property("background", "currentColor")
            .EndSelector()
            // Type badge
            .Selector(".being-type-badge")
                .Property("display", "inline-block")
                .Property("padding", "2px 8px")
                .Property("border-radius", "4px")
                .Property("font-size", "11px")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-secondary)")
                .Property("margin-left", "8px")
            .EndSelector()
            // Detail panel
            .Selector(".detail-panel")
                .Property("background", "var(--bg-secondary)")
                .Property("padding", "24px")
                .Property("border-radius", "var(--card-radius, 8px)")
                .Property("border", "1px solid var(--border-color)")
            .EndSelector()
            .Selector(".detail-content h2")
                .Property("font-size", "18px")
                .Property("font-weight", "600")
                .Property("color", "var(--text-primary)")
                .Property("margin-bottom", "16px")
            .EndSelector()
            .Selector(".detail-row")
                .Property("display", "flex")
                .Property("margin-bottom", "12px")
                .Property("align-items", "flex-start")
            .EndSelector()
            .Selector(".detail-label")
                .Property("font-size", "13px")
                .Property("font-weight", "600")
                .Property("color", "var(--text-muted)")
                .Property("width", "100px")
                .Property("flex-shrink", "0")
                .Property("text-transform", "uppercase")
                .Property("letter-spacing", "0.5px")
            .EndSelector()
            .Selector(".detail-value")
                .Property("font-size", "14px")
                .Property("color", "var(--text-primary)")
                .Property("font-weight", "500")
                .Property("word-break", "break-all")
            .EndSelector()
            .Selector(".detail-value.idle")
                .Property("color", "var(--status-warning, var(--accent-warning))")
            .EndSelector()
            .Selector(".detail-value.active")
                .Property("color", "var(--status-active, var(--accent-secondary))")
            .EndSelector()
            // Soul content area
            .Selector(".soul-content")
                .Property("background", "var(--bg-card)")
                .Property("padding", "16px")
                .Property("border-radius", "6px")
                .Property("font-size", "13px")
                .Property("line-height", "1.6")
                .Property("color", "var(--text-secondary)")
                .Property("max-height", "200px")
                .Property("overflow-y", "auto")
                .Property("white-space", "pre-wrap")
            .EndSelector()
            // Detail link
            .Selector(".detail-link")
                .Property("color", "var(--accent-primary)")
                .Property("text-decoration", "none")
                .Property("font-weight", "500")
                .Property("transition", "var(--transition, 0.2s ease)")
            .EndSelector()
            .Selector(".detail-link:hover")
                .Property("color", "var(--accent-secondary, var(--accent-primary))")
                .Property("text-decoration", "underline")
            .EndSelector()
            // Empty state
            .Selector(".empty-state")
                .Property("text-align", "center")
                .Property("padding", "40px")
                .Property("color", "var(--text-muted)")
                .Property("font-size", "14px")
            .EndSelector();
    }

    private static JsSyntax GetScripts(DefaultLocalizationBase loc)
    {
        var forEachBody = Js.Block()
            .Add(() => Js.Const(() => "card", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Const(() => "statusClass", () => Js.Ternary(() => Js.Id(() => "b").Prop(() => "activity").Op(() => "===", () => Js.Str(() => "Idle")), () => Js.Str(() => "idle"), () => Js.Str(() => "active"))))
            .Add(() => Js.Const(() => "statusText", () => Js.Id(() => "activityNameMap").Index(() => Js.Id(() => "b").Prop(() => "activity"))))
            .Add(() => Js.Const(() => "isSelected", () => Js.Id(() => "selectedBeingId").Op(() => "===", () => Js.Id(() => "b").Prop(() => "id"))))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "className"), () => Js.Ternary(() => Js.Id(() => "isSelected"), () => Js.Str(() => "being-card selected"), () => Js.Str(() => "being-card"))))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "onclick"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "selectBeing").Invoke(() => Js.Id(() => "b").Prop(() => "id"), () => Js.Id(() => "b").Prop(() => "name")))))
            .Add(() => Js.Id(() => "card").Call(() => "setAttribute", () => Js.Str(() => "data-id"), () => Js.Id(() => "b").Prop(() => "id")).Stmt())
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "innerHTML"), () => BuildCardHtml()))
            .Add(() => Js.Id(() => "grid").Call(() => "appendChild", () => Js.Id(() => "card")).Stmt());

        var thenBody = Js.Block()
            .Add(() => Js.Const(() => "grid", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "beings-grid"))))
            .Add(() => Js.Assign(() => Js.Id(() => "grid").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "total-count")).Prop(() => "textContent"), () => Js.Id(() => "data").Prop(() => "length")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "data").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "grid").Prop(() => "innerHTML"), () => Js.Str(() => $"<div class='empty-state'>{loc.BeingsEmptyState}</div>"))
                    }
                )},
                { (null, new List<JsSyntax>
                    {
                        Js.Id(() => "data").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "b" }, () => forEachBody)).Stmt()
                    }
                )}
            }));

        var loadBeingsBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/beings/list")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => thenBody)).Stmt());

        var selectThenBody = Js.Block()
            .Add(() => Js.Const(() => "statusClass", () => Js.Ternary(() => Js.Id(() => "data").Prop(() => "activity").Op(() => "===", () => Js.Str(() => "Idle")), () => Js.Str(() => "idle"), () => Js.Str(() => "active"))))
            .Add(() => Js.Const(() => "statusText", () => Js.Id(() => "activityNameMap").Index(() => Js.Id(() => "data").Prop(() => "activity"))))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "detail-content")).Prop(() => "innerHTML"), () => BuildDetailHtml(loc)));

        var selectBeingBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "selectedBeingId"), () => Js.Id(() => "id")))
            .Add(() => Js.Id(() => "loadBeings").Invoke().Stmt())
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/beings/detail?id=").Op(() => "+", () => Js.Id(() => "id"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => selectThenBody)).Stmt());

        return Js.Block()
            .Add(() =>
            {
                var map = Js.Obj()
                    .Prop(() => "Idle", () => Js.Str(() => loc.BeingsStatusIdle))
                    .Prop(() => "SingleChat", () => Js.Str(() => loc.GetBeingActivityName(BeingActivity.SingleChat)))
                    .Prop(() => "GroupChat", () => Js.Str(() => loc.GetBeingActivityName(BeingActivity.GroupChat)))
                    .Prop(() => "Task", () => Js.Str(() => loc.GetBeingActivityName(BeingActivity.Task)))
                    .Prop(() => "Timer", () => Js.Str(() => loc.GetBeingActivityName(BeingActivity.Timer)))
                    .Prop(() => "MemoryCompression", () => Js.Str(() => loc.GetBeingActivityName(BeingActivity.MemoryCompression)));
                return Js.Const(() => "activityNameMap", () => map);
            })
            .Add(() => Js.Let(() => "selectedBeingId", () => Js.Null()))
            .Add(() => Js.Func(() => "loadBeings", () => new List<string>(), () => loadBeingsBody))
            .Add(() => Js.Func(() => "selectBeing", () => new List<string> { "id", "name" }, () => selectBeingBody))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadBeings").Invoke())));
    }

    private static JsSyntax BuildCardHtml()
    {
        var typeBadge = Js.Ternary(
            () => Js.Id(() => "b").Prop(() => "isCustomCompiled"),
            () => Js.Str(() => "<span class='being-type-badge'>").Op(() => "+", () => (JsSyntax)Js.Id(() => "b").Prop(() => "customTypeName")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</span>")),
            () => Js.Str(() => ""));

        // Build status-badge structure (with status dot)
        var statusBadge = Js.Str(() => "<span class='being-status ")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "statusClass"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "'><span class='status-dot'></span>"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "statusText"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</span>"));

        return Js.Str(() => "<div class='being-header'><span class='being-name'>")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "b").Prop(() => "name"))
            .Op(() => "+", () => (JsSyntax)typeBadge)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</span>"))
            .Op(() => "+", () => (JsSyntax)statusBadge)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>"));
    }

    private static JsSyntax BuildDetailHtml(DefaultLocalizationBase loc)
    {
        // Status badge (with status dot)
        var statusValue = Js.Str(() => "<span class=\"detail-value ")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "statusClass"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "\'>"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "statusText"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</span>"));

        var compiledValue = Js.Ternary(
            () => Js.Id(() => "data").Prop(() => "isCustomCompiled"),
            () => Js.Str(() => $"{loc.BeingsYes} (").Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "customTypeName")).Op(() => "+", () => (JsSyntax)Js.Str(() => ")")),
            () => Js.Str(() => loc.BeingsNo));

        var soulHtml = Js.Str(() => "<a class='detail-link' href='/beings/soul?beingId=")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "id"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "'>"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => loc.BeingsDetailSoulContentEditLink))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</a>"));

        var aiClientValue = Js.Ternary(
            () => Js.Id(() => "data").Prop(() => "aiClientConfig"),
            () => Js.Str(() => "<a class='detail-link' href='/beings/ai-config?beingId=")
                .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "id"))
                .Op(() => "+", () => (JsSyntax)Js.Str(() => "'>"))
                .Op(() => "+", () => (JsSyntax)Js.Str(() => loc.BeingsDetailAIClientEditLink))
                .Op(() => "+", () => (JsSyntax)Js.Str(() => "</a> "))
                .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "aiClientConfig")),
            () => Js.Str(() => "<a class='detail-link' href='/beings/ai-config?beingId=")
                .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "id"))
                .Op(() => "+", () => (JsSyntax)Js.Str(() => "'>"))
                .Op(() => "+", () => (JsSyntax)Js.Str(() => loc.BeingsDetailAIClientEditLink))
                .Op(() => "+", () => (JsSyntax)Js.Str(() => "</a> "))
                .Op(() => "+", () => (JsSyntax)Js.Str(() => loc.BeingsNotSet)));

        var timerLink = Js.Str(() => "<a class='detail-link' href='/timers?beingId=")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "id"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "'>"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "timerCount"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</a>"));

        var taskLink = Js.Str(() => "<a class='detail-link' href='/tasks?beingId=")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "id"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "'>"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "taskCount"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</a>"));

        var memoryLink = Js.Str(() => "<a class='detail-link' href='/memory?beingId=")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "id"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "'>"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => loc.BeingsDetailMemoryViewLink))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</a>"));

        var permissionLink = Js.Str(() => "<a class='detail-link' href='/permissions?beingId=")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "id"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "'>"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => loc.BeingsDetailPermissionEditLink))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</a>"));

        var chatHistoryLink = Js.Str(() => "<a class='detail-link' href='/chat-history?beingId=")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "id"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "'>"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => loc.BeingsDetailChatHistoryLink))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</a>"));

        var workNoteLink = Js.Str(() => "<a class='detail-link' href='/work-notes?beingId=")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "id"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "'>"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => loc.BeingsDetailWorkNoteLink))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</a>"));

        return Js.Str(() => "<h2>")
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "name"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</h2><div class=\"detail-row\"><span class=\"detail-label\">{loc.BeingsDetailIdLabel}</span><span class=\"detail-value\">"))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "data").Prop(() => "id"))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</span></div><div class=\"detail-row\"><span class=\"detail-label\">{loc.BeingsDetailStatusLabel}</span>"))
            .Op(() => "+", () => (JsSyntax)statusValue)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</span></div><div class=\"detail-row\"><span class=\"detail-label\">{loc.BeingsDetailCustomCompileLabel}</span><span class=\"detail-value\">"))
            .Op(() => "+", () => (JsSyntax)compiledValue)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</span></div><div class=\"detail-row\"><span class=\"detail-label\">{loc.BeingsDetailTimersLabel}</span>"))
            .Op(() => "+", () => (JsSyntax)timerLink)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</div><div class=\"detail-row\"><span class=\"detail-label\">{loc.BeingsDetailTasksLabel}</span>"))
            .Op(() => "+", () => (JsSyntax)taskLink)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</div><div class=\"detail-row\"><span class=\"detail-label\">{loc.BeingsDetailMemoryLabel}</span>"))
            .Op(() => "+", () => (JsSyntax)memoryLink)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</div><div class=\"detail-row\"><span class=\"detail-label\">{loc.BeingsDetailPermissionLabel}</span>"))
            .Op(() => "+", () => (JsSyntax)permissionLink)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</div><div class=\"detail-row\"><span class=\"detail-label\">{loc.BeingsDetailChatHistoryLabel}</span>"))
            .Op(() => "+", () => (JsSyntax)chatHistoryLink)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</div><div class=\"detail-row\"><span class=\"detail-label\">{loc.BeingsDetailWorkNoteLabel}</span>"))
            .Op(() => "+", () => (JsSyntax)workNoteLink)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</div><div class=\"detail-row\"><span class=\"detail-label\">{loc.BeingsDetailAIClientLabel}</span><span class=\"detail-value\">"))
            .Op(() => "+", () => (JsSyntax)aiClientValue)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => $"</span></div><div class=\"detail-row\"><span class=\"detail-label\">{loc.BeingsDetailSoulContentLabel}</span>"))
            .Op(() => "+", () => (JsSyntax)soulHtml)
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>"));
    }
}
