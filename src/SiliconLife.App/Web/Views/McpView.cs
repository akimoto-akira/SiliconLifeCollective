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

namespace SiliconLife.App.Web.Views;

/// <summary>
/// MCP server management page: server cards (state, transport, tool count,
/// enable/disable/reconnect/remove) with an expandable per-server tool list
/// (schema view + test call). Servers are configured globally; the page is
/// entered from the beings list (beingId kept for navigation context).
/// Cards are built with DOM APIs (no inline handlers), mirroring SkillView.
/// </summary>
public class McpView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as McpViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.PageTitleMcp, "beings", vm.Localization, body, GetScripts(vm), GetStyles());
    }

    private static H RenderBody(McpViewModel vm)
    {
        return H.Div(
            H.Div(
                H.A(vm.Localization.McpsBackToBeings).Href("/beings").Class("back-link"),
                H.H1(vm.Localization.McpPageHeader).Class("mcp-title"),
                H.P(vm.Localization.McpPageSubtitle).Class("mcp-subtitle"),
                H.Span(string.Empty).Id("mcp-stat").Class("stat-value")
            ).Class("page-header"),
            H.Div(
                H.Button(vm.Localization.McpBtnAddServer).Id("btn-add-server").Class("toolbar-btn primary"),
                H.Button(vm.Localization.McpBtnRefresh).Id("btn-refresh-servers").Class("toolbar-btn")
            ).Class("mcp-toolbar"),
            H.Div().Id("mcp-server-list").Class("mcp-server-list")
        ).Class("page-content mcp-page");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".mcp-page .page-header")
                .Property("margin-bottom", "16px")
            .EndSelector()
            .Selector(".back-link")
                .Property("display", "inline-block")
                .Property("margin-bottom", "12px")
                .Property("color", "var(--accent-primary)")
                .Property("text-decoration", "none")
                .Property("font-size", "14px")
                .Property("font-weight", "500")
            .EndSelector()
            .Selector(".back-link:hover")
                .Property("text-decoration", "underline")
            .EndSelector()
            .Selector(".mcp-title")
                .Property("font-size", "28px")
                .Property("font-weight", "bold")
                .Property("color", "var(--text-primary)")
                .Property("margin", "0 0 6px 0")
            .EndSelector()
            .Selector(".mcp-subtitle")
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
                .Property("margin", "0 0 8px 0")
            .EndSelector()
            .Selector(".stat-value")
                .Property("font-size", "13px")
                .Property("font-weight", "bold")
                .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".mcp-toolbar")
                .Property("display", "flex")
                .Property("gap", "10px")
                .Property("margin-bottom", "16px")
            .EndSelector()
            .Selector(".toolbar-btn")
                .Property("padding", "8px 14px")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-primary)")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "6px")
                .Property("cursor", "pointer")
                .Property("font-size", "13px")
                .Property("transition", "all 0.15s")
            .EndSelector()
            .Selector(".toolbar-btn:hover")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".toolbar-btn.primary")
                .Property("background", "var(--accent-primary)")
                .Property("color", "white")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".mcp-server-list")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "12px")
            .EndSelector()
            .Selector(".mcp-server-card")
                .Property("background", "var(--bg-card)")
                .Property("padding", "14px")
                .Property("border-radius", "10px")
                .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".mcp-server-card.failed")
                .Property("border-color", "#d32f2f55")
            .EndSelector()
            .Selector(".mcp-card-title")
                .Property("font-size", "15px")
                .Property("font-weight", "bold")
                .Property("color", "var(--text-primary)")
                .Property("margin-bottom", "6px")
                .Property("word-break", "break-all")
            .EndSelector()
            .Selector(".mcp-badge")
                .Property("display", "inline-block")
                .Property("font-size", "11px")
                .Property("font-weight", "normal")
                .Property("color", "var(--text-secondary)")
                .Property("background", "var(--bg-secondary, rgba(0,0,0,0.05))")
                .Property("border-radius", "4px")
                .Property("padding", "2px 6px")
                .Property("margin-left", "8px")
            .EndSelector()
            .Selector(".mcp-state")
                .Property("display", "inline-block")
                .Property("font-size", "11px")
                .Property("font-weight", "bold")
                .Property("border-radius", "4px")
                .Property("padding", "2px 8px")
                .Property("margin-left", "8px")
            .EndSelector()
            .Selector(".mcp-state.connected")
                .Property("color", "#2e7d32")
                .Property("background", "#2e7d3218")
            .EndSelector()
            .Selector(".mcp-state.failed")
                .Property("color", "#d32f2f")
                .Property("background", "#d32f2f18")
            .EndSelector()
            .Selector(".mcp-state.pending")
                .Property("color", "#f57c00")
                .Property("background", "#f57c0018")
            .EndSelector()
            .Selector(".mcp-state.disabled")
                .Property("color", "var(--text-secondary)")
                .Property("background", "var(--bg-secondary, rgba(0,0,0,0.05))")
            .EndSelector()
            .Selector(".mcp-card-endpoint")
                .Property("font-size", "12px")
                .Property("font-family", "monospace")
                .Property("color", "var(--text-secondary)")
                .Property("margin-bottom", "8px")
                .Property("word-break", "break-all")
            .EndSelector()
            .Selector(".mcp-card-error")
                .Property("font-size", "12px")
                .Property("color", "#d32f2f")
                .Property("margin-bottom", "8px")
                .Property("word-break", "break-all")
            .EndSelector()
            .Selector(".mcp-card-actions")
                .Property("display", "flex")
                .Property("gap", "6px")
                .Property("flex-wrap", "wrap")
            .EndSelector()
            .Selector(".mcp-btn")
                .Property("padding", "4px 10px")
                .Property("font-size", "12px")
                .Property("background", "var(--bg-primary)")
                .Property("color", "var(--text-primary)")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "5px")
                .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".mcp-btn:hover")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".mcp-btn.danger")
                .Property("color", "#d32f2f")
                .Property("border-color", "#d32f2f55")
            .EndSelector()
            .Selector(".mcp-btn.danger:hover")
                .Property("background", "#d32f2f11")
            .EndSelector()
            .Selector(".mcp-tools")
                .Property("margin-top", "10px")
                .Property("border-top", "1px solid var(--border)")
                .Property("padding-top", "10px")
                .Property("display", "none")
            .EndSelector()
            .Selector(".mcp-tools.open")
                .Property("display", "block")
            .EndSelector()
            .Selector(".mcp-tool-row")
                .Property("display", "flex")
                .Property("align-items", "baseline")
                .Property("gap", "8px")
                .Property("padding", "4px 0")
            .EndSelector()
            .Selector(".mcp-tool-name")
                .Property("font-size", "12px")
                .Property("font-family", "monospace")
                .Property("color", "var(--text-primary)")
                .Property("word-break", "break-all")
                .Property("flex", "0 0 auto")
            .EndSelector()
            .Selector(".mcp-tool-desc")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
                .Property("flex", "1")
                .Property("min-width", "0")
            .EndSelector()
            .Selector(".empty-state")
                .Property("text-align", "center")
                .Property("padding", "40px")
                .Property("color", "var(--text-secondary)")
            .EndSelector();
    }

    private static JsSyntax GetScripts(McpViewModel vm)
    {
        var loc = vm.Localization;

        string listUrl = "/api/mcp/list-servers";
        string listToolsUrlPrefix = "/api/mcp/list-tools?serverId=";
        string addUrl = "/api/mcp/add-server";
        string toggleUrl = "/api/mcp/toggle";
        string removeUrl = "/api/mcp/remove-server";
        string reconnectUrl = "/api/mcp/reconnect";
        string testUrl = "/api/mcp/test-tool";

        // ----- server card builder (runs per server inside forEach) -----
        var cardBlock = Js.Block()
            // card
            .Add(() => Js.Const(() => "card", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "card").Prop(() => "className"),
                () => Js.Str(() => "mcp-server-card").Op(() => "+",
                    () => Js.Id(() => "s").Prop(() => "state").Op(() => "===", () => Js.Str(() => "failed"))
                        .Op(() => "?", () => Js.Str(() => " failed"))
                        .Op(() => ":", () => Js.Str(() => "")))))
            // title + badges + state
            .Add(() => Js.Const(() => "titleEl", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "titleEl").Prop(() => "className"), () => Js.Str(() => "mcp-card-title")))
            .Add(() => Js.Assign(() => Js.Id(() => "titleEl").Prop(() => "textContent"), () => Js.Id(() => "s").Prop(() => "id")))
            .Add(() => Js.Const(() => "badge", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "span"))))
            .Add(() => Js.Assign(() => Js.Id(() => "badge").Prop(() => "className"), () => Js.Str(() => "mcp-badge")))
            .Add(() => Js.Assign(() => Js.Id(() => "badge").Prop(() => "textContent"),
                () => Js.Id(() => "s").Prop(() => "name").Op(() => "+", () => Js.Str(() => " · "))
                    .Op(() => "+", () => Js.Id(() => "s").Prop(() => "transport"))
                    .Op(() => "+", () => Js.Str(() => " · ")))
                .Op(() => "+", () => Js.Id(() => "s").Prop(() => "toolCount")))
            .Add(() => Js.Id(() => "titleEl").Call(() => "appendChild", () => Js.Id(() => "badge")).Stmt())
            .Add(() => Js.Const(() => "stateEl", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "span"))))
            .Add(() => Js.Assign(() => Js.Id(() => "stateEl").Prop(() => "className"),
                () => Js.Str(() => "mcp-state ").Op(() => "+", () => Js.Id(() => "s").Prop(() => "state"))))
            .Add(() => Js.Assign(() => Js.Id(() => "stateEl").Prop(() => "textContent"),
                () => Js.Id(() => "stateLabels").Index(() => Js.Id(() => "s").Prop(() => "state"))
                    .Op(() => "||", () => Js.Id(() => "s").Prop(() => "state"))))
            .Add(() => Js.Id(() => "titleEl").Call(() => "appendChild", () => Js.Id(() => "stateEl")).Stmt())
            .Add(() => Js.Id(() => "card").Call(() => "appendChild", () => Js.Id(() => "titleEl")).Stmt())
            // endpoint
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "s").Prop(() => "endpoint"), new List<JsSyntax>
                {
                    Js.Const(() => "endpointEl", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "endpointEl").Prop(() => "className"), () => Js.Str(() => "mcp-card-endpoint")).Stmt(),
                    Js.Assign(() => Js.Id(() => "endpointEl").Prop(() => "textContent"), () => Js.Id(() => "s").Prop(() => "endpoint")).Stmt(),
                    Js.Id(() => "card").Call(() => "appendChild", () => Js.Id(() => "endpointEl")).Stmt()
                })
            }))
            // last error
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "s").Prop(() => "lastError"), new List<JsSyntax>
                {
                    Js.Const(() => "errorEl", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "errorEl").Prop(() => "className"), () => Js.Str(() => "mcp-card-error")).Stmt(),
                    Js.Assign(() => Js.Id(() => "errorEl").Prop(() => "textContent"), () => Js.Id(() => "s").Prop(() => "lastError")).Stmt(),
                    Js.Id(() => "card").Call(() => "appendChild", () => Js.Id(() => "errorEl")).Stmt()
                })
            }))
            // actions
            .Add(() => Js.Const(() => "actions", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "actions").Prop(() => "className"), () => Js.Str(() => "mcp-card-actions")))
            // enable/disable
            .Add(() => Js.Const(() => "toggleBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Assign(() => Js.Id(() => "toggleBtn").Prop(() => "className"), () => Js.Str(() => "mcp-btn")))
            .Add(() => Js.Assign(() => Js.Id(() => "toggleBtn").Prop(() => "textContent"),
                () => Js.Id(() => "s").Prop(() => "enabled")
                    .Op(() => "?", () => Js.Str(() => loc.McpBtnToggleOff))
                    .Op(() => ":", () => Js.Str(() => loc.McpBtnToggleOn))))
            .Add(() => Js.Id(() => "toggleBtn").Call(() => "addEventListener", () => Js.Str(() => "click"),
                () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "toggleServer")
                    .Invoke(() => Js.Id(() => "s").Prop(() => "id"), () => Js.Id(() => "s").Prop(() => "enabled").Not()))).Stmt())
            .Add(() => Js.Id(() => "actions").Call(() => "appendChild", () => Js.Id(() => "toggleBtn")).Stmt())
            // tools (expandable)
            .Add(() => Js.Const(() => "toolsBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Assign(() => Js.Id(() => "toolsBtn").Prop(() => "className"), () => Js.Str(() => "mcp-btn")))
            .Add(() => Js.Assign(() => Js.Id(() => "toolsBtn").Prop(() => "textContent"), () => Js.Str(() => loc.McpBtnViewTools)))
            .Add(() => Js.Id(() => "toolsBtn").Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string>(), () => Js.Block()
                .Add(() => Js.Const(() => "toolsDiv", () => Js.Id(() => "document").Call(() => "getElementById",
                    () => Js.Str(() => "mcp-tools-").Op(() => "+", () => Js.Id(() => "s").Prop(() => "id")))))
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "toolsDiv").Not(), new List<JsSyntax>
                    {
                        Js.Return(() => Js.Null())
                    })
                }))
                .Add(() => Js.Const(() => "opened", () => Js.Id(() => "toolsDiv").Prop(() => "classList").Call(() => "toggle", () => Js.Str(() => "open"))))
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "opened").Op(() => "&&", () => Js.Id(() => "toolsDiv").Prop(() => "dataset").Prop(() => "loaded").Not()), new List<JsSyntax>
                    {
                        Js.Id(() => "loadTools").Invoke(() => Js.Id(() => "s").Prop(() => "id"), () => Js.Id(() => "toolsDiv")).Stmt()
                    })
                })))).Stmt())
            .Add(() => Js.Id(() => "actions").Call(() => "appendChild", () => Js.Id(() => "toolsBtn")).Stmt())
            // reconnect
            .Add(() => Js.Const(() => "reconnectBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Assign(() => Js.Id(() => "reconnectBtn").Prop(() => "className"), () => Js.Str(() => "mcp-btn")))
            .Add(() => Js.Assign(() => Js.Id(() => "reconnectBtn").Prop(() => "textContent"), () => Js.Str(() => loc.McpBtnReconnect)))
            .Add(() => Js.Id(() => "reconnectBtn").Call(() => "addEventListener", () => Js.Str(() => "click"),
                () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "reconnectServer").Invoke(() => Js.Id(() => "s").Prop(() => "id")))).Stmt())
            .Add(() => Js.Id(() => "actions").Call(() => "appendChild", () => Js.Id(() => "reconnectBtn")).Stmt())
            // remove
            .Add(() => Js.Const(() => "delBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Assign(() => Js.Id(() => "delBtn").Prop(() => "className"), () => Js.Str(() => "mcp-btn danger")))
            .Add(() => Js.Assign(() => Js.Id(() => "delBtn").Prop(() => "textContent"), () => Js.Str(() => loc.McpBtnRemove)))
            .Add(() => Js.Id(() => "delBtn").Call(() => "addEventListener", () => Js.Str(() => "click"),
                () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "removeServer").Invoke(() => Js.Id(() => "s").Prop(() => "id")))).Stmt())
            .Add(() => Js.Id(() => "actions").Call(() => "appendChild", () => Js.Id(() => "delBtn")).Stmt())
            .Add(() => Js.Id(() => "card").Call(() => "appendChild", () => Js.Id(() => "actions")).Stmt())
            // hidden tools container
            .Add(() => Js.Const(() => "toolsContainer", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "toolsContainer").Prop(() => "className"), () => Js.Str(() => "mcp-tools")))
            .Add(() => Js.Assign(() => Js.Id(() => "toolsContainer").Prop(() => "id"),
                () => Js.Str(() => "mcp-tools-").Op(() => "+", () => Js.Id(() => "s").Prop(() => "id"))))
            .Add(() => Js.Id(() => "card").Call(() => "appendChild", () => Js.Id(() => "toolsContainer")).Stmt())
            .Add(() => Js.Id(() => "list").Call(() => "appendChild", () => Js.Id(() => "card")).Stmt());

        // ----- loadServers: render list + stats -----
        var loadThenBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
                {
                    Js.Const(() => "list", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "mcp-server-list"))).Stmt(),
                    Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => "")).Stmt(),
                    Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "mcp-stat")).Prop(() => "textContent"),
                        () => Js.Id(() => "statFormat")
                            .Call(() => "replace", () => Js.Str(() => "{0}"), () => Js.Id(() => "result").Prop(() => "data").Prop(() => "length"))
                            .Call(() => "replace", () => Js.Str(() => "{1}"), () => Js.Id(() => "result").Prop(() => "connected"))
                            .Call(() => "replace", () => Js.Str(() => "{2}"), () => Js.Id(() => "result").Prop(() => "toolTotal"))).Stmt(),
                    Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "result").Prop(() => "data").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
                        {
                            Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"),
                                () => Js.Str(() => $"<div class='empty-state'>{loc.McpsEmptyState}</div>")).Stmt()
                        }),
                        (null, new List<JsSyntax>
                        {
                            Js.Id(() => "result").Prop(() => "data").Call(() => "forEach",
                                () => Js.Arrow(() => new List<string> { "s" }, () => (JsSyntax)cardBlock)).Stmt()
                        })
                    })
                }),
                (null, new List<JsSyntax>
                {
                    Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "mcp-server-list")).Prop(() => "innerHTML"),
                        () => Js.Str(() => $"<div class='empty-state'>{loc.McpsEmptyState}</div>")).Stmt(),
                    Js.Id(() => "window").Call(() => "alert",
                        () => Js.Str(() => loc.McpLoadFailedFormat).Call(() => "replace", () => Js.Str(() => "{0}"),
                            () => Js.Id(() => "result").Prop(() => "error").Op(() => "||", () => Js.Str(() => "")))).Stmt()
                })
            }));

        // ----- loadTools(serverId, container): fetch + render tool rows -----
        var toolRowBlock = Js.Block()
            .Add(() => Js.Id(() => "window").Index(() => Js.Str(() => "__mcpSchemas"))
                .Index(() => Js.Id(() => "t").Prop(() => "name")).Assign(() => Js.Id(() => "t").Prop(() => "schema")).Stmt())
            .Add(() => Js.Const(() => "row", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "row").Prop(() => "className"), () => Js.Str(() => "mcp-tool-row")))
            .Add(() => Js.Const(() => "nameEl", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "span"))))
            .Add(() => Js.Assign(() => Js.Id(() => "nameEl").Prop(() => "className"), () => Js.Str(() => "mcp-tool-name")))
            .Add(() => Js.Assign(() => Js.Id(() => "nameEl").Prop(() => "textContent"), () => Js.Id(() => "t").Prop(() => "name")))
            .Add(() => Js.Id(() => "row").Call(() => "appendChild", () => Js.Id(() => "nameEl")).Stmt())
            .Add(() => Js.Const(() => "descEl", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "span"))))
            .Add(() => Js.Assign(() => Js.Id(() => "descEl").Prop(() => "className"), () => Js.Str(() => "mcp-tool-desc")))
            .Add(() => Js.Assign(() => Js.Id(() => "descEl").Prop(() => "textContent"), () => Js.Id(() => "t").Prop(() => "description").Op(() => "||", () => Js.Str(() => ""))))
            .Add(() => Js.Id(() => "row").Call(() => "appendChild", () => Js.Id(() => "descEl")).Stmt())
            // schema button
            .Add(() => Js.Const(() => "schemaBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Assign(() => Js.Id(() => "schemaBtn").Prop(() => "className"), () => Js.Str(() => "mcp-btn")))
            .Add(() => Js.Assign(() => Js.Id(() => "schemaBtn").Prop(() => "textContent"), () => Js.Str(() => loc.McpBtnViewSchema)))
            .Add(() => Js.Id(() => "schemaBtn").Call(() => "addEventListener", () => Js.Str(() => "click"),
                () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "showSchema").Invoke(() => Js.Id(() => "t").Prop(() => "name")))).Stmt())
            .Add(() => Js.Id(() => "row").Call(() => "appendChild", () => Js.Id(() => "schemaBtn")).Stmt())
            // test button
            .Add(() => Js.Const(() => "testBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Assign(() => Js.Id(() => "testBtn").Prop(() => "className"), () => Js.Str(() => "mcp-btn")))
            .Add(() => Js.Assign(() => Js.Id(() => "testBtn").Prop(() => "textContent"), () => Js.Str(() => loc.McpBtnTest)))
            .Add(() => Js.Id(() => "testBtn").Call(() => "addEventListener", () => Js.Str(() => "click"),
                () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "testTool")
                    .Invoke(() => Js.Id(() => "serverId"), () => Js.Id(() => "t").Prop(() => "name")))).Stmt())
            .Add(() => Js.Id(() => "row").Call(() => "appendChild", () => Js.Id(() => "testBtn")).Stmt())
            .Add(() => Js.Id(() => "container").Call(() => "appendChild", () => Js.Id(() => "row")).Stmt());

        var loadToolsThenBody = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "container").Prop(() => "dataset").Prop(() => "loaded"), () => Js.Str(() => "1")))
            .Add(() => Js.Assign(() => Js.Id(() => "container").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "result").Prop(() => "success").Not(), new List<JsSyntax>
                {
                    Js.Id(() => "window").Call(() => "alert",
                        () => Js.Str(() => loc.McpLoadFailedFormat).Call(() => "replace", () => Js.Str(() => "{0}"),
                            () => Js.Id(() => "result").Prop(() => "error").Op(() => "||", () => Js.Str(() => "")))).Stmt(),
                    Js.Return(() => Js.Null())
                })
            }))
            .Add(() => Js.Id(() => "result").Prop(() => "data").Call(() => "forEach",
                () => Js.Arrow(() => new List<string> { "t" }, () => (JsSyntax)toolRowBlock)).Stmt());

        // ----- shared POST helper: postJson(url, bodyObj) -> refresh list on success -----
        var postJsonThenBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "result").Prop(() => "success"), new List<JsSyntax>
                {
                    Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "result").Prop(() => "data").Prop(() => "message"), new List<JsSyntax>
                        {
                            Js.Id(() => "window").Call(() => "alert", () => Js.Id(() => "result").Prop(() => "data").Prop(() => "message")).Stmt()
                        })
                    }),
                    Js.Id(() => "loadServers").Invoke().Stmt()
                }),
                (null, new List<JsSyntax>
                {
                    Js.Id(() => "window").Call(() => "alert",
                        () => Js.Id(() => "result").Prop(() => "error").Op(() => "||", () => Js.Str(() => loc.McpRequestFailed))).Stmt()
                })
            }));

        var postJson = Js.Func(() => "postJson", () => new List<string> { "url", "bodyObj" }, () => Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Id(() => "url"), () => Js.Obj()
                .Prop(() => "method", () => Js.Str(() => "POST"))
                .Prop(() => "headers", () => Js.Obj().Prop(() => "Content-Type", () => Js.Str(() => "application/json")))
                .Prop(() => "body", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Id(() => "bodyObj"))))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json")))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => (JsSyntax)postJsonThenBody))
                .Call(() => "catch", () => Js.Arrow(() => new List<string> { "err" }, () => (JsSyntax)Js.Block()
                    .Add(() => Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "Request failed:"), () => Js.Id(() => "err")).Stmt())
                    .Add(() => Js.Id(() => "window").Call(() => "alert",
                        () => Js.Str(() => loc.McpRequestFailed).Op(() => "+", () => Js.Str(() => " ")).Op(() => "+", () => Js.Id(() => "err"))).Stmt())))
                .Stmt()));

        return Js.Block()
            // constants
            .Add(() => Js.Const(() => "statFormat", () => Js.Str(() => loc.McpsStatFormat)).Stmt())
            .Add(() => Js.Assign(() => Js.Id(() => "window").Index(() => Js.Str(() => "__mcpSchemas")), () => Js.Obj()).Stmt())
            .Add(() => Js.Const(() => "stateLabels", () => Js.Obj()
                .Prop(() => "connected", () => Js.Str(() => loc.McpStatusConnected))
                .Prop(() => "failed", () => Js.Str(() => loc.McpStatusFailed))
                .Prop(() => "pending", () => Js.Str(() => loc.McpStatusPending))
                .Prop(() => "disabled", () => Js.Str(() => loc.McpStatusDisabled))).Stmt())
            // loadServers
            .Add(() => Js.Func(() => "loadServers", () => new List<string>(), () => Js.Block()
                .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => listUrl))
                    .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json")))
                    .Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => (JsSyntax)loadThenBody))
                    .Call(() => "catch", () => Js.Arrow(() => new List<string> { "err" }, () => (JsSyntax)Js.Block()
                        .Add(() => Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "Failed to load MCP servers:"), () => Js.Id(() => "err")).Stmt()))).Stmt())))
            // loadTools
            .Add(() => Js.Func(() => "loadTools", () => new List<string> { "serverId", "container" }, () => Js.Block()
                .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => listToolsUrlPrefix).Op(() => "+", () => Js.Id(() => "encodeURIComponent").Invoke(() => Js.Id(() => "serverId"))))
                    .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => (JsSyntax)Js.Id(() => "r").Call(() => "json")))
                    .Call(() => "then", () => Js.Arrow(() => new List<string> { "result" }, () => (JsSyntax)loadToolsThenBody))
                    .Call(() => "catch", () => Js.Arrow(() => new List<string> { "err" }, () => (JsSyntax)Js.Block()
                        .Add(() => Js.Id(() => "console").Call(() => "error", () => Js.Str(() => "Failed to load tools:"), () => Js.Id(() => "err")).Stmt()))).Stmt())))
            // showSchema
            .Add(() => Js.Func(() => "showSchema", () => new List<string> { "toolName" }, () => Js.Block()
                .Add(() => Js.Const(() => "schema", () => Js.Id(() => "window").Index(() => Js.Str(() => "__mcpSchemas")).Index(() => Js.Id(() => "toolName"))))
                .Add(() => Js.Id(() => "window").Call(() => "alert",
                    () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Id(() => "schema"), () => Js.Null(), () => Js.Num(() => "2"))).Stmt())))
            // toggleServer
            .Add(() => Js.Func(() => "toggleServer", () => new List<string> { "serverId", "enabled" }, () => Js.Block()
                .Add(() => Js.Id(() => "postJson").Invoke(() => Js.Str(() => toggleUrl),
                    () => Js.Obj().Prop(() => "serverId", () => Js.Id(() => "serverId")).Prop(() => "enabled", () => Js.Id(() => "enabled"))).Stmt())))
            // reconnectServer
            .Add(() => Js.Func(() => "reconnectServer", () => new List<string> { "serverId" }, () => Js.Block()
                .Add(() => Js.Id(() => "postJson").Invoke(() => Js.Str(() => reconnectUrl),
                    () => Js.Obj().Prop(() => "serverId", () => Js.Id(() => "serverId"))).Stmt())))
            // removeServer
            .Add(() => Js.Func(() => "removeServer", () => new List<string> { "serverId" }, () => Js.Block()
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "window").Call(() => "confirm",
                        () => Js.Str(() => loc.McpConfirmRemoveFormat).Call(() => "replace", () => Js.Str(() => "{0}"), () => Js.Id(() => "serverId"))).Not(), new List<JsSyntax>
                    {
                        Js.Return(() => Js.Null())
                    })
                }))
                .Add(() => Js.Id(() => "postJson").Invoke(() => Js.Str(() => removeUrl),
                    () => Js.Obj().Prop(() => "serverId", () => Js.Id(() => "serverId"))).Stmt())))
            // testTool
            .Add(() => Js.Func(() => "testTool", () => new List<string> { "serverId", "toolName" }, () => Js.Block()
                .Add(() => Js.Const(() => "raw", () => Js.Id(() => "window").Call(() => "prompt", () => Js.Str(() => loc.McpPromptTestParams))))
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "raw").Op(() => "===", () => Js.Null()), new List<JsSyntax>
                    {
                        Js.Return(() => Js.Null())
                    })
                }))
                .Add(() => Js.Id(() => "postJson").Invoke(() => Js.Str(() => testUrl),
                    () => Js.Obj()
                        .Prop(() => "serverId", () => Js.Id(() => "serverId"))
                        .Prop(() => "toolName", () => Js.Id(() => "toolName"))
                        .Prop(() => "argumentsJson", () => Js.Id(() => "raw").Call(() => "trim"))).Stmt())))
            // addServer (prompt chain)
            .Add(() => Js.Func(() => "addServer", () => new List<string>(), () => Js.Block()
                .Add(() => Js.Const(() => "id", () => Js.Id(() => "window").Call(() => "prompt", () => Js.Str(() => loc.McpPromptServerId))))
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "id").Not(), new List<JsSyntax> { Js.Return(() => Js.Null()) })
                }))
                .Add(() => Js.Const(() => "name", () => Js.Id(() => "window").Call(() => "prompt", () => Js.Str(() => loc.McpPromptName)).Op(() => "||", () => Js.Id(() => "id"))))
                .Add(() => Js.Const(() => "transport", () => Js.Id(() => "window").Call(() => "prompt", () => Js.Str(() => loc.McpPromptTransport)).Call(() => "toLowerCase")))
                .Add(() => Js.Const(() => "body", () => Js.Obj()
                    .Prop(() => "id", () => Js.Id(() => "id").Call(() => "trim"))
                    .Prop(() => "name", () => Js.Id(() => "name"))
                    .Prop(() => "transport", () => Js.Id(() => "transport").Op(() => "===", () => Js.Str(() => "http")).Op(() => "?", () => Js.Str(() => "http")).Op(() => ":", () => Js.Str(() => "stdio")))
                    .Prop(() => "args", () => Js.Id(() => "Array").Call(() => "from"))
                    .Prop(() => "enabled", () => Js.Bool(() => false))))
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "body").Prop(() => "transport").Op(() => "===", () => Js.Str(() => "stdio")), new List<JsSyntax>
                    {
                        Js.Const(() => "command", () => Js.Id(() => "window").Call(() => "prompt", () => Js.Str(() => loc.McpPromptCommand))).Stmt(),
                        Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            (Js.Id(() => "command").Not(), new List<JsSyntax> { Js.Return(() => Js.Null()) })
                        }),
                        Js.Const(() => "argsRaw", () => Js.Id(() => "window").Call(() => "prompt", () => Js.Str(() => loc.McpPromptArgs)).Op(() => "||", () => Js.Str(() => ""))).Stmt(),
                        Js.Assign(() => Js.Id(() => "body").Prop(() => "command"), () => Js.Id(() => "command")).Stmt(),
                        Js.Assign(() => Js.Id(() => "body").Prop(() => "args"), () => Js.Id(() => "argsRaw").Call(() => "split", () => Js.Str(() => "/\\s+/")).Call(() => "filter", () => Js.Arrow(() => new List<string> { "a" }, () => (JsSyntax)Js.Id(() => "a").Prop(() => "length").Op(() => ">", () => Js.Num(() => "0"))))).Stmt()
                    }),
                (null, new List<JsSyntax>
                {
                    Js.Const(() => "url", () => Js.Id(() => "window").Call(() => "prompt", () => Js.Str(() => loc.McpPromptUrl))).Stmt(),
                    Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        (Js.Id(() => "url").Not(), new List<JsSyntax> { Js.Return(() => Js.Null()) })
                    }),
                    Js.Assign(() => Js.Id(() => "body").Prop(() => "url"), () => Js.Id(() => "url")).Stmt()
                })
            }))
            .Add(() => Js.Assign(() => Js.Id(() => "body").Prop(() => "enabled"), () => Js.Id(() => "window").Call(() => "confirm", () => Js.Str(() => loc.McpPromptEnableNow))).Stmt())
            .Add(() => Js.Id(() => "postJson").Invoke(() => Js.Str(() => addUrl), () => Js.Id(() => "body")).Stmt())))
            // toolbar bindings + initial load
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                (Js.Id(() => "document").Prop(() => "readyState").Op(() => "===", () => Js.Str(() => "loading")), new List<JsSyntax>
                {
                    Js.Id(() => "document").Call(() => "addEventListener", () => Js.Str(() => "DOMContentLoaded"), () => Js.Id(() => "bindToolbar")).Stmt()
                }),
                (null, new List<JsSyntax>
                {
                    Js.Id(() => "bindToolbar").Invoke().Stmt()
                })
            }))
            .Add(() => Js.Func(() => "bindToolbar", () => new List<string>(), () => Js.Block()
                .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "btn-add-server")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Id(() => "addServer")).Stmt())
                .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "btn-refresh-servers")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Id(() => "loadServers")).Stmt())
                .Add(() => Js.Id(() => "loadServers").Invoke().Stmt())));
    }
}
