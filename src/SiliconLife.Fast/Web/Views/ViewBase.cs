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
using SiliconLife.Fast.Web.Models;

using SiliconLife.Common.Localization;

namespace SiliconLife.Fast.Web.Views;

public abstract class ViewBase
{
    protected readonly StringBuilder Sb = new();

    public abstract string Render(object model);

    protected string RenderPage(ISkin skin, string title, string activeMenu, DefaultLocalizationBase localization, H bodyContent,
        JsSyntax? inlineScripts = null, CssBuilder? inlineStyles = null, string? helpTopicId = null)
    {
        var themeCss = skin.GetThemeVariables().Build();
        var baseCss = skin.GetCustomStyles().Build();
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
                RenderHeader(localization, helpTopicId),
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

    private static H RenderHeader(DefaultLocalizationBase localization, string? helpTopicId = null)
    {
        var helpHref = string.IsNullOrEmpty(helpTopicId) 
            ? "/help" 
            : $"/help/{helpTopicId}";
        
        return H.Header(
            H.Div($"🜲 {localization.BrandName}").Class("shell-brand"),
            H.Div(
                H.A("❓").Class("shell-header-link").Href(helpHref).Attr("title", localization.NavMenuHelp)
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
            ("audit", "🔍", localization.NavMenuAudit, "/audit"),
            ("knowledge", "📚", localization.NavMenuKnowledge, "/knowledge"),
            ("projects", "📁", localization.NavMenuProjects, "/project"),
            ("logs", "📝", localization.NavMenuLogs, "/logs"),
            ("config", "⚙", localization.NavMenuConfig, "/config"),
            ("help", "❓", localization.NavMenuHelp, "/help"),
            ("about", "ℹ", localization.NavMenuAbout, "/about"),
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
            // 基础重置
            .Selector("body")
                .Property("margin", "0")
                .Property("padding", "0")
                .Property("box-sizing", "border-box")
            .EndSelector()
            .Selector("*, *::before, *::after")
                .Property("box-sizing", "border-box")
            .EndSelector()
            // 整体布局
            .Selector(".shell")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("min-height", "100vh")
                .Property("background", "var(--bg-primary)")
                .Property("color", "var(--text-primary)")
                .Property("font-family", "-apple-system, BlinkMacSystemFont, 'Segoe UI', 'Microsoft YaHei', sans-serif")
                .Property("line-height", "1.5")
            .EndSelector()
            // 头部样式
            .Selector(".shell-header")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("justify-content", "space-between")
                .Property("padding", "0 24px")
                .Property("height", "60px")
                .Property("background", "var(--bg-secondary)")
                .Property("border-bottom", "1px solid var(--border-color)")
                .Property("flex-shrink", "0")
                .Property("position", "sticky")
                .Property("top", "0")
                .Property("z-index", "50")
            .EndSelector()
            .Selector(".shell-brand")
                .Property("font-size", "16px")
                .Property("font-weight", "700")
                .Property("color", "var(--accent-primary)")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "8px")
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
                .Property("transition", "color 0.2s ease")
                .Property("padding", "8px")
                .Property("border-radius", "6px")
            .EndSelector()
            .Selector(".shell-header-link:hover")
                .Property("color", "var(--accent-primary)")
                .Property("background", "var(--bg-card)")
            .EndSelector()
            // 主体区域
            .Selector(".shell-body")
                .Property("display", "flex")
                .Property("flex", "1")
                .Property("overflow", "hidden")
            .EndSelector()
            // 侧边栏
            .Selector(".shell-sidebar")
                .Property("width", "200px")
                .Property("background", "var(--bg-secondary)")
                .Property("border-right", "1px solid var(--border-color)")
                .Property("padding", "12px 0")
                .Property("overflow-y", "auto")
                .Property("flex-shrink", "0")
            .EndSelector()
            // 菜单项
            .Selector(".shell-menu-item")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "12px")
                .Property("padding", "10px 12px")
                .Property("margin", "2px 8px")
                .Property("color", "var(--text-secondary)")
                .Property("text-decoration", "none")
                .Property("font-size", "14px")
                .Property("cursor", "pointer")
                .Property("transition", "all 0.2s ease")
                .Property("border-radius", "6px")
                .Property("border-left", "none")
            .EndSelector()
            .Selector(".shell-menu-item:hover")
                .Property("color", "var(--text-primary)")
                .Property("background", "var(--bg-card)")
            .EndSelector()
            .Selector(".shell-menu-item.active")
                .Property("color", "#ffffff")
                .Property("background", "var(--accent-primary)")
            .EndSelector()
            .Selector(".shell-menu-icon")
                .Property("width", "24px")
                .Property("text-align", "center")
                .Property("flex-shrink", "0")
                .Property("font-size", "18px")
            .EndSelector()
            // 主内容区
            .Selector(".shell-content")
                .Property("flex", "1")
                .Property("overflow-y", "auto")
                .Property("display", "flex")
                .Property("flex-direction", "column")
            .EndSelector()
            // 滚动条样式
            .Selector("::-webkit-scrollbar")
                .Property("width", "6px")
                .Property("height", "6px")
            .EndSelector()
            .Selector("::-webkit-scrollbar-track")
                .Property("background", "var(--bg-secondary)")
            .EndSelector()
            .Selector("::-webkit-scrollbar-thumb")
                .Property("background", "var(--bg-hover, var(--bg-card))")
                .Property("border-radius", "3px")
            .EndSelector()
            .Selector("::-webkit-scrollbar-thumb:hover")
                .Property("background", "var(--border-color)")
            .EndSelector()
            // 响应式设计
            .Media("(max-width: 1024px)")
                .Selector(".shell-sidebar")
                    .Property("width", "180px")
                .EndSelector()
            .EndMedia()
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
                    .Property("margin", "2px 4px")
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
            // 页面内容区
            .Selector(".page-content")
                .Property("flex", "1")
                .Property("overflow-y", "auto")
                .Property("padding", "24px")
                .Property("max-width", "1400px")
            .EndSelector()
            // 页面头部
            .Selector(".page-header")
                .Property("margin-bottom", "24px")
            .EndSelector()
            .Selector(".page-header h1")
                .Property("font-size", "24px")
                .Property("font-weight", "700")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".page-header-actions")
                .Property("display", "flex")
                .Property("gap", "10px")
                .Property("margin-top", "12px")
            .EndSelector()
            // 卡片系统
            .Selector(".card")
                .Property("background", "var(--bg-secondary)")
                .Property("padding", "20px")
                .Property("border-radius", "8px")
                .Property("border", "1px solid var(--border-color)")
                .Property("margin-bottom", "20px")
                .Property("transition", "border-color 0.2s ease")
            .EndSelector()
            .Selector(".card:hover")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".card-header")
                .Property("display", "flex")
                .Property("justify-content", "space-between")
                .Property("align-items", "center")
                .Property("margin-bottom", "16px")
            .EndSelector()
            .Selector(".card-title")
                .Property("font-size", "16px")
                .Property("font-weight", "600")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".card-actions")
                .Property("display", "flex")
                .Property("gap", "8px")
            .EndSelector()
            .Selector(".card h3")
                .Property("font-size", "16px")
                .Property("font-weight", "600")
                .Property("margin-bottom", "12px")
            .EndSelector()
            // 按钮系统
            .Selector(".btn")
                .Property("display", "inline-flex")
                .Property("align-items", "center")
                .Property("gap", "6px")
                .Property("padding", "8px 16px")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#ffffff")
                .Property("border", "none")
                .Property("border-radius", "6px")
                .Property("cursor", "pointer")
                .Property("text-decoration", "none")
                .Property("font-size", "14px")
                .Property("font-weight", "500")
                .Property("transition", "all 0.2s ease")
            .EndSelector()
            .Selector(".btn:hover")
                .Property("opacity", "0.9")
                .Property("transform", "translateY(-1px)")
            .EndSelector()
            .Selector(".btn-secondary")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-primary)")
                .Property("border", "1px solid var(--border-color)")
            .EndSelector()
            .Selector(".btn-secondary:hover")
                .Property("background", "var(--bg-hover, var(--bg-tertiary, var(--bg-card)))")
            .EndSelector()
            .Selector(".btn-danger")
                .Property("background", "var(--accent-danger, var(--accent-error))")
            .EndSelector()
            .Selector(".btn-danger:hover")
                .Property("opacity", "0.9")
            .EndSelector()
            .Selector(".btn-sm")
                .Property("padding", "6px 12px")
                .Property("font-size", "13px")
            .EndSelector()
            // 表单系统
            .Selector(".form-group")
                .Property("margin-bottom", "15px")
            .EndSelector()
            .Selector(".form-group label")
                .Property("display", "block")
                .Property("margin-bottom", "5px")
                .Property("font-weight", "500")
                .Property("font-size", "14px")
            .EndSelector()
            .Selector(".form-group input, .form-group textarea, .form-group select")
                .Property("width", "100%")
                .Property("padding", "10px 12px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-primary)")
                .Property("box-sizing", "border-box")
                .Property("font-size", "14px")
                .Property("transition", "border-color 0.2s ease")
            .EndSelector()
            .Selector(".form-group input:focus, .form-group textarea:focus, .form-group select:focus")
                .Property("outline", "none")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            // 数据表格
            .Selector("table")
                .Property("width", "100%")
                .Property("border-collapse", "collapse")
            .EndSelector()
            .Selector("table th")
                .Property("padding", "12px 16px")
                .Property("text-align", "left")
                .Property("font-size", "12px")
                .Property("font-weight", "600")
                .Property("color", "var(--text-muted)")
                .Property("text-transform", "uppercase")
                .Property("letter-spacing", "0.5px")
                .Property("border-bottom", "1px solid var(--border-color)")
            .EndSelector()
            .Selector("table td")
                .Property("padding", "12px 16px")
                .Property("font-size", "14px")
                .Property("border-bottom", "1px solid var(--border-color)")
            .EndSelector()
            .Selector("table tbody tr:hover")
                .Property("background", "var(--bg-card)")
            .EndSelector()
            // 状态标签
            .Selector(".badge")
                .Property("display", "inline-flex")
                .Property("align-items", "center")
                .Property("gap", "6px")
                .Property("padding", "4px 10px")
                .Property("border-radius", "12px")
                .Property("font-size", "12px")
                .Property("font-weight", "500")
            .EndSelector()
            .Selector(".badge-success")
                .Property("background", "rgba(16, 185, 129, 0.15)")
                .Property("color", "var(--accent-secondary, var(--accent-success))")
            .EndSelector()
            .Selector(".badge-warning")
                .Property("background", "rgba(245, 158, 11, 0.15)")
                .Property("color", "var(--accent-warning)")
            .EndSelector()
            .Selector(".badge-error")
                .Property("background", "rgba(239, 68, 68, 0.15)")
                .Property("color", "var(--accent-danger, var(--accent-error))")
            .EndSelector()
            // 筛选栏
            .Selector(".filter-bar")
                .Property("display", "flex")
                .Property("gap", "10px")
                .Property("margin-bottom", "20px")
                .Property("align-items", "center")
                .Property("flex-wrap", "wrap")
            .EndSelector()
            .Selector(".filter-bar select, .filter-bar input")
                .Property("padding", "8px 12px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-primary)")
                .Property("font-size", "14px")
            .EndSelector()
            .Selector(".filter-bar button")
                .Property("padding", "8px 16px")
            .EndSelector()
            // 分页
            .Selector(".pagination")
                .Property("display", "flex")
                .Property("gap", "8px")
                .Property("margin-top", "20px")
                .Property("align-items", "center")
                .Property("justify-content", "center")
            .EndSelector()
            .Selector(".pagination a, .pagination span")
                .Property("padding", "8px 14px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "6px")
                .Property("text-decoration", "none")
                .Property("color", "var(--text-primary)")
                .Property("background", "var(--bg-card)")
                .Property("transition", "all 0.2s ease")
            .EndSelector()
            .Selector(".pagination a:hover")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#fff")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            // 搜索栏
            .Selector(".search-bar")
                .Property("display", "flex")
                .Property("gap", "10px")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".search-bar input")
                .Property("flex", "1")
                .Property("padding", "10px 14px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            // 统计网格
            .Selector(".stats-grid")
                .Property("display", "grid")
                .Property("grid-template-columns", "repeat(auto-fit, minmax(200px, 1fr))")
                .Property("gap", "16px")
                .Property("margin-bottom", "24px")
            .EndSelector()
            .Selector(".stat-card")
                .Property("background", "var(--bg-secondary)")
                .Property("padding", "20px")
                .Property("border-radius", "8px")
                .Property("border", "1px solid var(--border-color)")
                .Property("transition", "border-color 0.2s ease")
            .EndSelector()
            .Selector(".stat-card:hover")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".stat-card h3")
                .Property("font-size", "13px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".stat-value")
                .Property("font-size", "28px")
                .Property("font-weight", "700")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            // 警告框
            .Selector(".alert")
                .Property("padding", "15px")
                .Property("border-radius", "8px")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".alert-success")
                .Property("background", "rgba(16, 185, 129, 0.15)")
                .Property("color", "var(--accent-secondary, var(--accent-success))")
                .Property("border", "1px solid var(--accent-secondary, var(--accent-success))")
            .EndSelector()
            .Selector(".alert-error")
                .Property("background", "rgba(239, 68, 68, 0.15)")
                .Property("color", "var(--accent-danger, var(--accent-error))")
                .Property("border", "1px solid var(--accent-danger, var(--accent-error))")
            .EndSelector()
            .Selector(".alert-warning")
                .Property("background", "rgba(245, 158, 11, 0.15)")
                .Property("color", "var(--accent-warning)")
                .Property("border", "1px solid var(--accent-warning)")
            .EndSelector();
    }
}