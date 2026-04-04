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

namespace SiliconLife.Default.Web.Skins;

public class ChatSkin : ISkin
{
    public string Code => "chat";
    public string Name => "Chat";

    public SkinPreviewInfo PreviewInfo => new()
    {
        Icon = "\U0001f4ac",
        Description = "Modern dark blue",
        BackgroundColor = "#1a1a2e",
        CardColor = "#16213e",
        AccentColor = "#4d96ff",
        TextColor = "#eaeaea",
        BorderColor = "#0f3460"
    };

    public HtmlBuilder RenderHtml(string content)
    {
        return HtmlBuilder.Create()
            .DocType("html")
            .Html()
            .Head()
            .MetaCharset("utf-8")
            .MetaViewport("width=device-width, initial-scale=1")
            .Title("Silicon Life Collective")
            .Style(GetStyles() + GetThemeCss().Build())
            .ScriptInline(GetScripts().Build())
            .EndBlock()
            .Body()
            .Div().Class("container").Raw(content).EndBlock().EndBlock();
    }

    public HtmlBuilder RenderError(string message)
    {
        return HtmlBuilder.Create()
            .DocType("html")
            .Html()
            .Head()
            .MetaCharset("utf-8")
            .MetaViewport("width=device-width, initial-scale=1")
            .Title("错误 - Silicon Life Collective")
            .Style(GetStyles() + GetThemeCss().Build())
            .EndBlock()
            .Body()
            .Div().Class("container")
            .Div().Class("error")
            .H1().Text("出错了").EndBlock()
            .P().Text(message).EndBlock()
            .A().Text("返回首页").Href("/").EndBlock()
            .EndBlock().EndBlock().EndBlock().EndBlock();
    }

    public CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .WithVariable("bg-primary", "#1a1a2e")
            .WithVariable("bg-card", "#16213e")
            .WithVariable("border", "#0f3460")
            .WithVariable("text-primary", "#eaeaea")
            .WithVariable("text-secondary", "#a0a0a0")
            .WithVariable("accent-primary", "#4d96ff")
            .WithVariable("accent-success", "#6bcb77")
            .WithVariable("accent-warning", "#ffd93d")
            .WithVariable("accent-error", "#ff6b6b")
            .WithVariable("accent-info", "#4ecdc4")
            .Selector("*")
            .Property("box-sizing", "border-box")
            .Property("margin", "0")
            .Property("padding", "0")
            .EndSelector()
            .Selector("body")
            .Property("font-family", "-apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif")
            .Property("background", "var(--bg-primary)")
            .Property("color", "var(--text-primary)")
            .Property("padding", "24px")
            .EndSelector()
            .Selector(".container")
            .Property("max-width", "1200px")
            .Property("margin", "0 auto")
            .EndSelector()
            .Selector(".error")
            .Property("background", "var(--bg-card)")
            .Property("padding", "40px")
            .Property("border-radius", "12px")
            .Property("text-align", "center")
            .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".error h1")
            .Property("color", "var(--accent-error)")
            .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".error a")
            .Property("color", "var(--accent-primary)")
            .Property("text-decoration", "none")
            .EndSelector();
    }

    public JsBuilder GetScripts()
    {
        return JsBuilder.Create();
    }

    public HtmlBuilder RenderButton(string text, string variant = "primary", string size = "medium")
    {
        var cls = $"btn btn-{variant}";
        if (size != "medium") cls += $" btn-{size}";
        
        return HtmlBuilder.Create()
            .Button().Text(text).Class(cls).EndBlock();
    }

    public HtmlBuilder RenderInput(string placeholder = "", string size = "medium", string? value = null)
    {
        var cls = "input";
        if (size != "medium") cls += $" input-{size}";
        
        return HtmlBuilder.Create()
            .InputText().Placeholder(placeholder).Class(cls).EndBlock();
    }

    public HtmlBuilder RenderTextarea(string placeholder = "", int rows = 4)
    {
        return HtmlBuilder.Create()
            .Textarea().Attr("placeholder", placeholder).Attr("rows", rows).Class("input textarea").EndBlock();
    }

    public HtmlBuilder RenderSelect(IEnumerable<string> options, string? selected = null)
    {
        var builder = HtmlBuilder.Create().Select();
        foreach (var opt in options)
        {
            builder.Option(opt, opt, opt == selected);
        }
        return builder.EndBlock();
    }

    public HtmlBuilder RenderCheckbox(string label, bool isChecked = false)
    {
        var checkMark = isChecked ? "✓" : string.Empty;
        var checkboxBoxClass = "checkbox-box" + (isChecked ? " checked" : string.Empty);
        return HtmlBuilder.Create()
            .Div().Class("checkbox")
            .Div().Class(checkboxBoxClass).Text(checkMark).EndBlock()
            .Span().Text(label).EndBlock()
            .EndBlock();
    }

    public HtmlBuilder RenderBadge(string text, string variant = "primary")
    {
        return HtmlBuilder.Create()
            .Span().Text(text).Class($"badge badge-{variant}").EndBlock();
    }

    public HtmlBuilder RenderTag(string text)
    {
        return HtmlBuilder.Create()
            .Span().Text(text).Class("tag").EndBlock();
    }

    public HtmlBuilder RenderCard(string title, string content)
    {
        return HtmlBuilder.Create()
            .Div().Class("card")
            .Div().Class("card-header").Text(title).EndBlock()
            .Div().Class("card-body").Raw(content).EndBlock()
            .EndBlock();
    }

    public HtmlBuilder RenderAvatar(string text, string size = "medium")
    {
        return HtmlBuilder.Create()
            .Div().Text(text).Class($"avatar avatar-{size}").EndBlock();
    }

    public HtmlBuilder RenderBubble(string text, bool isMine = false)
    {
        return HtmlBuilder.Create()
            .Div().Text(text).Class($"bubble{(isMine ? " mine" : string.Empty)}").EndBlock();
    }

    public HtmlBuilder RenderSwitch(bool isChecked = false)
    {
        var switchClass = "switch" + (isChecked ? " active" : string.Empty);
        return HtmlBuilder.Create()
            .Div().Class(switchClass).EndBlock();
    }

    public HtmlBuilder RenderProgress(double value, string variant = "primary")
    {
        return HtmlBuilder.Create()
            .Div().Class("progress")
            .Div().Class($"progress-bar").Attr("style", $"width: {value}%").EndBlock()
            .EndBlock();
    }

    public HtmlBuilder RenderTabs(IEnumerable<string> tabs, int activeIndex = 0)
    {
        var builder = HtmlBuilder.Create();
        var idx = 0;
        foreach (var tab in tabs)
        {
            builder.Span().Text(tab).Class($"tab{(idx == activeIndex ? " active" : string.Empty)}").EndBlock();
            idx++;
        }
        return builder;
    }

    public HtmlBuilder RenderListItem(string title, string? subtitle = null, string? avatar = null, bool active = false)
    {
        var builder = HtmlBuilder.Create()
            .Div().Class($"list-item{(active ? " active" : string.Empty)}");
        
        if (!string.IsNullOrEmpty(avatar))
        {
            builder.Div().Text(avatar).Class("avatar").EndBlock();
        }
        
        builder.Div();
        builder.Text(title);
        if (!string.IsNullOrEmpty(subtitle))
        {
            builder.Raw($"<div style=\"font-size: 12px; color: var(--text-secondary);\">{subtitle}</div>");
        }
        builder.EndBlock().EndBlock();
        
        return builder;
    }

    public HtmlBuilder RenderDivider()
    {
        return HtmlBuilder.Create().Hr();
    }

    public HtmlBuilder RenderCode(string code)
    {
        return HtmlBuilder.Create()
            .Pre().Code().Text(code).EndBlock().EndBlock();
    }

    public HtmlBuilder RenderStatCard(string label, string value, string variant = "primary")
    {
        return HtmlBuilder.Create()
            .Div().Class("stat-card")
            .Div().Text(value).Class("stat-value").Attr("style", $"color: var(--accent-{variant})").EndBlock()
            .Div().Text(label).Class("stat-label").EndBlock()
            .EndBlock();
    }

    public HtmlBuilder RenderBreadcrumb(IEnumerable<string> items)
    {
        var builder = HtmlBuilder.Create().Div().Class("breadcrumb");
        var isFirst = true;
        foreach (var item in items)
        {
            if (!isFirst) builder.Raw(" / ");
            builder.Span().Text(item).EndBlock();
            isFirst = false;
        }
        return builder.EndBlock();
    }

    public HtmlBuilder RenderTable(IEnumerable<string> headers, IEnumerable<IEnumerable<string>> rows)
    {
        var builder = HtmlBuilder.Create().Table();
        
        builder.Thead();
        builder.Tr();
        foreach (var header in headers)
        {
            builder.Th().Text(header).EndBlock();
        }
        builder.EndBlock().EndBlock();
        
        builder.Tbody();
        foreach (var row in rows)
        {
            builder.Tr();
            foreach (var cell in row)
            {
                builder.Td().Raw(cell).EndBlock();
            }
            builder.EndBlock();
        }
        builder.EndBlock().EndBlock();
        
        return builder;
    }

    public HtmlBuilder RenderPagination(int totalPages, int currentPage = 1)
    {
        var builder = HtmlBuilder.Create().Div().Class("pagination");
        builder.Div().Text("‹").Class("page-btn").EndBlock();
        
        for (int i = 1; i <= totalPages; i++)
        {
            builder.Div().Text(i.ToString()).Class($"page-btn{(i == currentPage ? " active" : string.Empty)}").EndBlock();
        }
        
        builder.Div().Text("›").Class("page-btn").EndBlock();
        return builder.EndBlock();
    }

    public HtmlBuilder RenderDropdown(string triggerText, IEnumerable<string> items)
    {
        var builder = HtmlBuilder.Create().Div().Class("dropdown");
        builder.Button().Text(triggerText + " ▼").Class("btn btn-primary").EndBlock();
        
        var menuBuilder = HtmlBuilder.Create().Div().Class("dropdown-menu");
        foreach (var item in items)
        {
            menuBuilder.Div().Text(item).Class("dropdown-item").EndBlock();
        }
        
        builder.Raw(menuBuilder.Build());
        return builder.EndBlock();
    }

    public HtmlBuilder RenderStatusIndicator(string status)
    {
        var statusClass = status.ToLower() switch
        {
            "online" => "status-online",
            "offline" => "status-offline",
            "busy" => "status-busy",
            _ => "status-offline"
        };
        
        return HtmlBuilder.Create()
            .Span().Class($"status-dot {statusClass}").EndBlock();
    }

    public HtmlBuilder RenderQuote(string text)
    {
        return HtmlBuilder.Create()
            .Blockquote().Text(text).EndBlock();
    }

    public HtmlBuilder RenderInspirationCard(string icon, string text)
    {
        return HtmlBuilder.Create()
            .Div().Class("inspiration-card")
            .Div().Text(icon).Class("inspiration-icon").EndBlock()
            .Div().Text($"\"{text}\"").Class("inspiration-text").EndBlock()
            .EndBlock();
    }

    public CssBuilder GetThemeCss()
    {
        return CssBuilder.Create()
            .WithVariable("bg-primary", "#1a1a2e")
            .WithVariable("bg-card", "#16213e")
            .WithVariable("bg-secondary", "#1e3a5f")
            .WithVariable("border", "#0f3460")
            .WithVariable("text-primary", "#eaeaea")
            .WithVariable("text-secondary", "#a0a0a0")
            .WithVariable("accent-primary", "#4d96ff")
            .WithVariable("accent-success", "#6bcb77")
            .WithVariable("accent-warning", "#ffd93d")
            .WithVariable("accent-error", "#ff6b6b")
            .WithVariable("accent-info", "#4ecdc4");
    }
}
