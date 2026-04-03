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

public class DevSkin : ISkin
{
    public string Name => "Dev";

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
            .Title("Error - Silicon Life Collective")
            .Style(GetStyles() + GetThemeCss().Build())
            .EndBlock()
            .Body()
            .Div().Class("container")
            .Div().Class("error")
            .H1().Text("ERROR").EndBlock()
            .P().Text(message).EndBlock()
            .A().Text("Back to Home").Href("/").EndBlock()
            .EndBlock().EndBlock().EndBlock().EndBlock();
    }

    public CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector("*")
            .Property("box-sizing", "border-box")
            .Property("margin", "0")
            .Property("padding", "0")
            .EndSelector()
            .Selector("body")
            .Property("font-family", "'SF Mono', 'Consolas', 'Monaco', monospace")
            .Property("background", "var(--bg-primary)")
            .Property("color", "var(--text-primary)")
            .Property("font-size", "13px")
            .Property("padding", "24px")
            .EndSelector()
            .Selector(".container")
            .Property("max-width", "1200px")
            .Property("margin", "0 auto")
            .EndSelector()
            .Selector(".error")
            .Property("background", "var(--bg-secondary)")
            .Property("padding", "40px")
            .Property("border-radius", "6px")
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
        var cls = "btn";
        if (variant == "primary") cls += " btn-primary";
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
        var builder = HtmlBuilder.Create().Select().Class("select");
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
            .Div().Class("card-header").Raw($"// {title}").EndBlock()
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
            .Div().Text($"// {text}").Class($"bubble{(isMine ? " mine" : string.Empty)}").EndBlock();
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
            .Div().Class($"progress-bar progress-{variant}").Attr("style", $"width: {value}%").EndBlock()
            .EndBlock();
    }

    public HtmlBuilder RenderTabs(IEnumerable<string> tabs, int activeIndex = 0)
    {
        var builder = HtmlBuilder.Create();
        var idx = 0;
        foreach (var tab in tabs)
        {
            builder.Span().Text($"/{tab.ToLower()}").Class($"tab{(idx == activeIndex ? " active" : string.Empty)}").EndBlock();
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
        
        builder.Span().Text(title).EndBlock();
        
        if (!string.IsNullOrEmpty(subtitle))
        {
            builder.Span().Text(subtitle).Class("badge badge-success").EndBlock();
        }
        
        return builder.EndBlock();
    }

    public HtmlBuilder RenderDivider()
    {
        return HtmlBuilder.Create().Hr();
    }

    public HtmlBuilder RenderCode(string code)
    {
        return HtmlBuilder.Create()
            .Pre().Code().Text(code).Class("code").EndBlock().EndBlock();
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
        builder.Button().Text(triggerText + " ▼").Class("btn").EndBlock();
        
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
            .WithVariable("bg-primary", "#0d1117")
            .WithVariable("bg-secondary", "#161b22")
            .WithVariable("bg-tertiary", "#21262d")
            .WithVariable("border", "#30363d")
            .WithVariable("text-primary", "#c9d1d9")
            .WithVariable("text-secondary", "#8b949e")
            .WithVariable("accent-primary", "#58a6ff")
            .WithVariable("accent-success", "#3fb950")
            .WithVariable("accent-warning", "#d29922")
            .WithVariable("accent-error", "#f85149")
            .WithVariable("accent-info", "#58a6ff")
            .Selector(".btn")
            .Property("padding", "6px 12px")
            .Property("border-radius", "4px")
            .Property("border", "1px solid var(--border)")
            .Property("background", "var(--bg-tertiary)")
            .Property("color", "var(--text-primary)")
            .Property("cursor", "pointer")
            .Property("font-size", "12px")
            .Property("font-family", "inherit")
            .EndSelector()
            .Selector(".btn-primary")
            .Property("background", "var(--accent-primary)")
            .Property("border-color", "var(--accent-primary)")
            .Property("color", "#0d1117")
            .EndSelector()
            .Selector(".input")
            .Property("background", "var(--bg-primary)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "4px")
            .Property("padding", "8px 10px")
            .Property("color", "var(--text-primary)")
            .Property("font-size", "12px")
            .Property("font-family", "inherit")
            .EndSelector()
            .Selector(".card")
            .Property("background", "var(--bg-primary)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "6px")
            .Property("padding", "12px")
            .EndSelector()
            .Selector(".badge")
            .Property("display", "inline-block")
            .Property("padding", "2px 8px")
            .Property("border-radius", "10px")
            .Property("font-size", "11px")
            .EndSelector()
            .Selector(".badge-primary")
            .Property("background", "rgba(88, 166, 255, 0.2)")
            .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".badge-success")
            .Property("background", "rgba(63, 185, 80, 0.2)")
            .Property("color", "var(--accent-success)")
            .EndSelector()
            .Selector(".avatar")
            .Property("width", "32px")
            .Property("height", "32px")
            .Property("border-radius", "4px")
            .Property("background", "var(--bg-tertiary)")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("justify-content", "center")
            .Property("font-size", "14px")
            .EndSelector()
            .Selector(".bubble")
            .Property("background", "var(--bg-tertiary)")
            .Property("padding", "8px 12px")
            .Property("border-radius", "6px")
            .Property("font-size", "12px")
            .Property("max-width", "280px")
            .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".bubble.mine")
            .Property("background", "#1c2a3a")
            .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".switch")
            .Property("width", "36px")
            .Property("height", "20px")
            .Property("background", "var(--bg-tertiary)")
            .Property("border-radius", "10px")
            .Property("position", "relative")
            .Property("cursor", "pointer")
            .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".progress")
            .Property("height", "6px")
            .Property("background", "var(--bg-tertiary)")
            .Property("border-radius", "3px")
            .Property("overflow", "hidden")
            .EndSelector()
            .Selector(".progress-bar")
            .Property("height", "100%")
            .Property("background", "var(--accent-primary)")
            .Property("border-radius", "3px")
            .EndSelector()
            .Selector(".progress-success .progress-bar")
            .Property("background", "var(--accent-success)")
            .EndSelector()
            .Selector(".tab")
            .Property("padding", "8px 12px")
            .Property("border-bottom", "2px solid transparent")
            .Property("cursor", "pointer")
            .Property("font-size", "12px")
            .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".tag")
            .Property("display", "inline-flex")
            .Property("align-items", "center")
            .Property("gap", "4px")
            .Property("padding", "2px 6px")
            .Property("background", "var(--bg-tertiary)")
            .Property("border-radius", "4px")
            .Property("font-size", "11px")
            .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".list-item")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("gap", "8px")
            .Property("padding", "6px 8px")
            .Property("border-radius", "4px")
            .Property("cursor", "pointer")
            .Property("font-size", "12px")
            .EndSelector()
            .Selector(".list-item:hover")
            .Property("background", "var(--bg-tertiary)")
            .EndSelector()
            .Selector(".status-dot")
            .Property("width", "8px")
            .Property("height", "8px")
            .Property("border-radius", "50%")
            .Property("display", "inline-block")
            .EndSelector()
            .Selector(".status-online")
            .Property("background", "var(--accent-success)")
            .EndSelector()
            .Selector(".status-offline")
            .Property("background", "var(--accent-error)")
            .EndSelector()
            .Selector(".status-busy")
            .Property("background", "var(--accent-warning)")
            .EndSelector();
    }
}
