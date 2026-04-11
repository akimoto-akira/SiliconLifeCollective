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

using System.Text;
using SiliconLife.Default.Web.Models;

namespace SiliconLife.Default.Web.Views;

public abstract class ViewBase
{
    protected readonly StringBuilder Sb = new();

    public abstract string Render(object model);

    protected string EscapeHtml(string text)
    {
        return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#39;");
    }

    protected string RenderPage(ISkin skin, string title, string activeMenu, DefaultLocalizationBase localization, H bodyContent,
        JsSyntax? inlineScripts = null, CssBuilder? inlineStyles = null)
    {
        var themeCss = skin.GetThemeCss().Build();
        var baseCss = skin.GetStyles().Build();
        var shellCss = GetShellCss().Build();
        var commonCss = GetCommonCss().Build();

        var headChildren = new List<object>
        {
            H.Meta().Attr("charset", "utf-8"),
            H.Meta().Attr("name", "viewport").Attr("content", "width=device-width, initial-scale=1"),
            H.Title(title),
            H.Style(baseCss),
            H.Style(shellCss + commonCss)
        };

        if (!string.IsNullOrEmpty(themeCss))
            headChildren.Add(H.Style(themeCss));

        if (inlineStyles != null)
            headChildren.Add(H.Style(inlineStyles));

        var bodyChildren = new List<object>
        {
            H.Div(
                RenderHeader(localization),
                H.Div(
                    RenderSidebar(activeMenu, localization),
                    H.MainElement(bodyContent).Class("shell-content")
                ).Class("shell-body")
            ).Class("shell")
        };

        if (inlineScripts != null)
            bodyChildren.Add(H.Script(inlineScripts));

        var html = H.Html(
            H.Head(headChildren.ToArray()),
            H.Body(bodyChildren.ToArray())
        );

        return H.DocType() + "\n" + html.Build();
    }

    private static H RenderHeader(DefaultLocalizationBase localization)
    {
        return H.Header(
            H.Div($"🜲 {localization.BrandName}").Class("shell-brand"),
            H.Div(
                H.A("⚙").Class("shell-header-link").Href("/config")
            ).Class("shell-header-actions")
        ).Class("shell-header");
    }

    private static H RenderSidebar(string activeMenu, DefaultLocalizationBase localization)
    {
        var items = new (string Id, string Icon, string Label, string Href)[]
        {
            ("chat", "💬", localization.NavMenuChat, "/chat"),
            ("dashboard", "📊", localization.NavMenuDashboard, "/dashboard"),
            ("beings", "🧠", localization.NavMenuBeings, "/beings"),
            ("tasks", "📋", localization.NavMenuTasks, "/tasks"),
            ("memory", "📂", localization.NavMenuMemory, "/memory"),
            ("knowledge", "📚", localization.NavMenuKnowledge, "/knowledge"),
            ("projects", "📁", localization.NavMenuProjects, "/project"),
            ("logs", "📝", localization.NavMenuLogs, "/logs"),
            ("config", "⚙", localization.NavMenuConfig, "/config"),
        };

        var menuItems = new List<H>();
        foreach (var (id, icon, label, href) in items)
        {
            var activeClass = id == activeMenu ? "shell-menu-item active" : "shell-menu-item";
            menuItems.Add(H.A(
                H.Span(icon).Class("shell-menu-icon"),
                H.Span(label).Class("shell-menu-text")
            ).Class(activeClass).Href(href));
        }

        return H.Aside(menuItems.ToArray()).Class("shell-sidebar");
    }

    protected static CssBuilder GetShellCss()
    {
        return CssBuilder.Create()
            .Selector("body")
                .Property("margin", "0")
                .Property("padding", "0")
            .EndSelector()
            .Selector(".shell")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("height", "100vh")
                .Property("overflow", "hidden")
                .Property("background", "var(--bg-primary)")
                .Property("color", "var(--text-primary)")
                .Property("font-family", "-apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif")
            .EndSelector()
            .Selector(".shell-header")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("justify-content", "space-between")
                .Property("padding", "0 20px")
                .Property("height", "48px")
                .Property("background", "var(--bg-card)")
                .Property("border-bottom", "1px solid var(--border)")
                .Property("flex-shrink", "0")
            .EndSelector()
            .Selector(".shell-brand")
                .Property("font-size", "16px")
                .Property("font-weight", "bold")
            .EndSelector()
            .Selector(".shell-header-actions")
                .Property("display", "flex")
                .Property("gap", "12px")
                .Property("align-items", "center")
            .EndSelector()
            .Selector(".shell-header-link")
                .Property("color", "var(--text-secondary)")
                .Property("text-decoration", "none")
                .Property("font-size", "18px")
                .Property("cursor", "pointer")
                .Property("transition", "color 0.2s")
            .EndSelector()
            .Selector(".shell-header-link:hover")
                .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".shell-body")
                .Property("display", "flex")
                .Property("flex", "1")
                .Property("overflow", "hidden")
            .EndSelector()
            .Selector(".shell-sidebar")
                .Property("width", "200px")
                .Property("background", "var(--bg-card)")
                .Property("border-right", "1px solid var(--border)")
                .Property("padding", "12px 0")
                .Property("overflow-y", "auto")
                .Property("flex-shrink", "0")
            .EndSelector()
            .Selector(".shell-content")
                .Property("flex", "1")
                .Property("overflow", "hidden")
                .Property("display", "flex")
                .Property("flex-direction", "column")
            .EndSelector()
            .Selector(".shell-menu-item")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "10px")
                .Property("padding", "10px 20px")
                .Property("color", "var(--text-secondary)")
                .Property("text-decoration", "none")
                .Property("font-size", "14px")
                .Property("cursor", "pointer")
                .Property("transition", "all 0.2s")
                .Property("border-left", "3px solid transparent")
            .EndSelector()
            .Selector(".shell-menu-item:hover")
                .Property("color", "var(--text-primary)")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.05))")
            .EndSelector()
            .Selector(".shell-menu-item.active")
                .Property("color", "var(--accent-primary)")
                .Property("border-left-color", "var(--accent-primary)")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.05))")
            .EndSelector()
            .Selector(".shell-menu-icon")
                .Property("width", "20px")
                .Property("text-align", "center")
                .Property("flex-shrink", "0")
            .EndSelector()
            .Media("(max-width: 768px)")
                .Selector(".shell-sidebar")
                    .Property("width", "60px")
                    .Property("padding", "8px 0")
                .EndSelector()
                .Selector(".shell-menu-text")
                    .Property("display", "none")
                .EndSelector()
                .Selector(".shell-menu-item")
                    .Property("justify-content", "center")
                    .Property("padding", "10px")
                .EndSelector()
                .Selector(".shell-menu-icon")
                    .Property("width", "auto")
                .EndSelector()
            .EndMedia()
            .Media("(max-width: 480px)")
                .Selector(".shell-sidebar")
                    .Property("display", "none")
                .EndSelector()
            .EndMedia();
    }

    protected static CssBuilder GetCommonCss()
    {
        return CssBuilder.Create()
            .Selector(".page-content")
                .Property("flex", "1")
                .Property("overflow-y", "auto")
                .Property("padding", "24px")
            .EndSelector()
            .Selector(".page-header")
                .Property("margin-bottom", "24px")
            .EndSelector()
            .Selector(".page-header h1")
                .Property("font-size", "24px")
                .Property("font-weight", "600")
            .EndSelector()
            .Selector(".page-header-actions")
                .Property("display", "flex")
                .Property("gap", "10px")
                .Property("margin-top", "12px")
            .EndSelector()
            .Selector(".card")
                .Property("background", "var(--bg-card)")
                .Property("padding", "20px")
                .Property("border-radius", "12px")
                .Property("border", "1px solid var(--border)")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".card h3")
                .Property("font-size", "16px")
                .Property("margin-bottom", "12px")
            .EndSelector()
            .Selector(".btn")
                .Property("display", "inline-block")
                .Property("padding", "10px 20px")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#fff")
                .Property("border", "none")
                .Property("border-radius", "6px")
                .Property("cursor", "pointer")
                .Property("text-decoration", "none")
                .Property("font-size", "14px")
                .Property("transition", "opacity 0.2s")
            .EndSelector()
            .Selector(".btn:hover")
                .Property("opacity", "0.85")
            .EndSelector()
            .Selector(".btn-danger")
                .Property("background", "var(--accent-error, #ff6b6b)")
            .EndSelector()
            .Selector(".btn-sm")
                .Property("padding", "6px 14px")
                .Property("font-size", "13px")
            .EndSelector()
            .Selector(".form-group")
                .Property("margin-bottom", "15px")
            .EndSelector()
            .Selector(".form-group label")
                .Property("display", "block")
                .Property("margin-bottom", "5px")
                .Property("font-weight", "500")
            .EndSelector()
            .Selector(".form-group input, .form-group textarea, .form-group select")
                .Property("width", "100%")
                .Property("padding", "10px")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-primary)")
                .Property("box-sizing", "border-box")
                .Property("font-size", "14px")
            .EndSelector()
            .Selector(".form-group input:focus, .form-group textarea:focus, .form-group select:focus")
                .Property("outline", "none")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector("table")
                .Property("width", "100%")
                .Property("border-collapse", "collapse")
            .EndSelector()
            .Selector("table th, table td")
                .Property("padding", "12px")
                .Property("text-align", "left")
                .Property("border-bottom", "1px solid var(--border)")
            .EndSelector()
            .Selector("table th")
                .Property("font-weight", "600")
            .EndSelector()
            .Selector("table tbody tr:hover")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.03))")
            .EndSelector()
            .Selector(".badge")
                .Property("display", "inline-block")
                .Property("padding", "4px 12px")
                .Property("border-radius", "12px")
                .Property("font-size", "12px")
                .Property("font-weight", "500")
            .EndSelector()
            .Selector(".badge-success")
                .Property("background", "rgba(107,203,119,0.15)")
                .Property("color", "var(--accent-success, #6bcb77)")
            .EndSelector()
            .Selector(".badge-warning")
                .Property("background", "rgba(255,217,61,0.15)")
                .Property("color", "var(--accent-warning, #ffd93d)")
            .EndSelector()
            .Selector(".badge-error")
                .Property("background", "rgba(255,107,107,0.15)")
                .Property("color", "var(--accent-error, #ff6b6b)")
            .EndSelector()
            .Selector(".filter-bar")
                .Property("display", "flex")
                .Property("gap", "10px")
                .Property("margin-bottom", "20px")
                .Property("align-items", "center")
                .Property("flex-wrap", "wrap")
            .EndSelector()
            .Selector(".filter-bar select, .filter-bar input, .filter-bar button")
                .Property("padding", "8px 12px")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-primary)")
                .Property("font-size", "14px")
            .EndSelector()
            .Selector(".filter-bar button")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#fff")
                .Property("border", "none")
                .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".pagination")
                .Property("display", "flex")
                .Property("gap", "8px")
                .Property("margin-top", "20px")
                .Property("align-items", "center")
                .Property("justify-content", "center")
            .EndSelector()
            .Selector(".pagination a, .pagination span")
                .Property("padding", "8px 14px")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "6px")
                .Property("text-decoration", "none")
                .Property("color", "var(--text-primary)")
                .Property("background", "var(--bg-card)")
            .EndSelector()
            .Selector(".pagination a:hover")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#fff")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".search-bar")
                .Property("display", "flex")
                .Property("gap", "10px")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".search-bar input")
                .Property("flex", "1")
                .Property("padding", "10px 14px")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".search-bar button")
                .Property("padding", "10px 20px")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#fff")
                .Property("border", "none")
                .Property("border-radius", "6px")
                .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".stats-grid")
                .Property("display", "grid")
                .Property("grid-template-columns", "repeat(auto-fit, minmax(180px, 1fr))")
                .Property("gap", "16px")
                .Property("margin-bottom", "24px")
            .EndSelector()
            .Selector(".stat-card")
                .Property("background", "var(--bg-card)")
                .Property("padding", "20px")
                .Property("border-radius", "12px")
                .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".stat-card h3")
                .Property("font-size", "13px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".stat-value")
                .Property("font-size", "28px")
                .Property("font-weight", "bold")
            .EndSelector()
            .Selector(".alert")
                .Property("padding", "15px")
                .Property("border-radius", "6px")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".alert-success")
                .Property("background", "var(--accent-success, #6bcb77)")
                .Property("color", "#fff")
            .EndSelector()
            .Selector(".alert-error")
                .Property("background", "var(--accent-error, #ff6b6b)")
                .Property("color", "#fff")
            .EndSelector()
            .Selector(".alert-warning")
                .Property("background", "var(--accent-warning, #ffd93d)")
                .Property("color", "#333")
            .EndSelector();
    }
}
