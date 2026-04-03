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

public class CreativeSkin : ISkin
{
    public string Name => "Creative";

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
            .Selector("*")
            .Property("box-sizing", "border-box")
            .Property("margin", "0")
            .Property("padding", "0")
            .EndSelector()
            .Selector("body")
            .Property("font-family", "'Georgia', 'Songti SC', serif")
            .Property("background", "var(--bg-primary)")
            .Property("color", "var(--text-primary)")
            .Property("font-size", "15px")
            .Property("padding", "24px")
            .EndSelector()
            .Selector(".container")
            .Property("max-width", "1200px")
            .Property("margin", "0 auto")
            .EndSelector()
            .Selector(".error")
            .Property("background", "var(--bg-card)")
            .Property("padding", "40px")
            .Property("border-radius", "16px")
            .Property("text-align", "center")
            .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".error h1")
            .Property("color", "var(--accent-primary)")
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
        if (variant == "primary") cls = "btn btn-primary";
        else if (variant == "secondary") cls = "btn btn-secondary";
        else if (variant == "success") cls = "btn btn-success";
        else if (variant == "outline") cls = "btn btn-outline";
        
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
            .Div().Class("progress-bar").Attr("style", $"width: {value}%").EndBlock()
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
            .Div().Class("card-elevated")
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
            .Div().Class("quote").Raw($"\"{text}\"").EndBlock();
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
            .WithVariable("bg-primary", "#fdf6e3")
            .WithVariable("bg-card", "#fffef9")
            .WithVariable("bg-sidebar", "#f5ebe0")
            .WithVariable("border", "#e8dfd0")
            .WithVariable("text-primary", "#5c4b37")
            .WithVariable("text-secondary", "#8b7355")
            .WithVariable("accent-primary", "#d4a574")
            .WithVariable("accent-secondary", "#c9956c")
            .WithVariable("accent-warm", "#e8c9a0")
            .WithVariable("accent-success", "#8fbc8f")
            .Selector(".btn")
            .Property("padding", "12px 24px")
            .Property("border-radius", "12px")
            .Property("border", "none")
            .Property("cursor", "pointer")
            .Property("font-size", "14px")
            .Property("font-weight", "500")
            .Property("transition", "all 0.2s")
            .Property("font-family", "inherit")
            .EndSelector()
            .Selector(".btn-primary")
            .Property("background", "var(--accent-primary)")
            .Property("color", "white")
            .Property("box-shadow", "0 2px 8px rgba(212,165,116,0.3)")
            .EndSelector()
            .Selector(".btn-primary:hover")
            .Property("transform", "translateY(-2px)")
            .Property("box-shadow", "0 4px 12px rgba(212,165,116,0.4)")
            .EndSelector()
            .Selector(".btn-secondary")
            .Property("background", "var(--bg-sidebar)")
            .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".btn-success")
            .Property("background", "var(--accent-success)")
            .Property("color", "white")
            .EndSelector()
            .Selector(".btn-outline")
            .Property("background", "transparent")
            .Property("border", "2px solid var(--accent-primary)")
            .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".input")
            .Property("background", "var(--bg-primary)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "12px")
            .Property("padding", "14px 18px")
            .Property("color", "var(--text-primary)")
            .Property("font-size", "15px")
            .Property("font-family", "inherit")
            .Property("transition", "all 0.2s")
            .EndSelector()
            .Selector(".input:focus")
            .Property("border-color", "var(--accent-primary)")
            .Property("box-shadow", "0 0 0 3px rgba(212,165,116,0.2)")
            .EndSelector()
            .Selector(".textarea")
            .Property("min-height", "100px")
            .Property("resize", "vertical")
            .Property("border-radius", "16px")
            .EndSelector()
            .Selector(".select")
            .Property("background", "var(--bg-primary)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "12px")
            .Property("padding", "14px 18px")
            .Property("color", "var(--text-primary)")
            .Property("font-size", "15px")
            .Property("font-family", "inherit")
            .EndSelector()
            .Selector(".card")
            .Property("background", "var(--bg-card)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "16px")
            .Property("padding", "20px")
            .Property("box-shadow", "0 2px 12px rgba(0,0,0,0.04)")
            .EndSelector()
            .Selector(".card-header")
            .Property("font-size", "17px")
            .Property("font-weight", "600")
            .Property("margin-bottom", "12px")
            .EndSelector()
            .Selector(".card-body")
            .Property("font-size", "14px")
            .Property("color", "var(--text-secondary)")
            .Property("line-height", "1.7")
            .EndSelector()
            .Selector(".card-elevated")
            .Property("background", "var(--bg-card)")
            .Property("border-radius", "20px")
            .Property("padding", "24px")
            .Property("box-shadow", "0 4px 20px rgba(0,0,0,0.06)")
            .EndSelector()
            .Selector(".badge")
            .Property("display", "inline-block")
            .Property("padding", "6px 14px")
            .Property("border-radius", "20px")
            .Property("font-size", "12px")
            .Property("font-weight", "500")
            .EndSelector()
            .Selector(".badge-primary")
            .Property("background", "var(--accent-warm)")
            .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".badge-success")
            .Property("background", "var(--accent-success)")
            .Property("color", "white")
            .EndSelector()
            .Selector(".avatar")
            .Property("width", "48px")
            .Property("height", "48px")
            .Property("border-radius", "50%")
            .Property("background", "var(--bg-card)")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("justify-content", "center")
            .Property("font-size", "22px")
            .Property("box-shadow", "0 2px 10px rgba(0,0,0,0.08)")
            .EndSelector()
            .Selector(".bubble")
            .Property("background", "var(--bg-card)")
            .Property("padding", "16px 20px")
            .Property("border-radius", "24px")
            .Property("font-size", "15px")
            .Property("max-width", "320px")
            .Property("line-height", "1.7")
            .Property("box-shadow", "0 2px 10px rgba(0,0,0,0.04)")
            .EndSelector()
            .Selector(".bubble.mine")
            .Property("background", "var(--accent-warm)")
            .EndSelector()
            .Selector(".switch")
            .Property("width", "52px")
            .Property("height", "28px")
            .Property("background", "var(--border)")
            .Property("border-radius", "14px")
            .Property("position", "relative")
            .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".progress")
            .Property("height", "10px")
            .Property("background", "var(--bg-sidebar)")
            .Property("border-radius", "5px")
            .Property("overflow", "hidden")
            .EndSelector()
            .Selector(".progress-bar")
            .Property("height", "100%")
            .Property("background", "linear-gradient(90deg, var(--accent-primary), var(--accent-warm))")
            .Property("border-radius", "5px")
            .EndSelector()
            .Selector(".tab")
            .Property("padding", "12px 20px")
            .Property("border-bottom", "3px solid transparent")
            .Property("cursor", "pointer")
            .Property("font-size", "14px")
            .Property("color", "var(--text-secondary)")
            .Property("transition", "all 0.2s")
            .EndSelector()
            .Selector(".tag")
            .Property("display", "inline-flex")
            .Property("align-items", "center")
            .Property("gap", "4px")
            .Property("padding", "6px 12px")
            .Property("background", "var(--bg-sidebar)")
            .Property("border-radius", "20px")
            .Property("font-size", "12px")
            .EndSelector()
            .Selector(".list-item")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("gap", "14px")
            .Property("padding", "14px")
            .Property("border-radius", "14px")
            .Property("cursor", "pointer")
            .Property("transition", "all 0.2s")
            .EndSelector()
            .Selector(".list-item:hover")
            .Property("background", "rgba(212,165,116,0.15)")
            .EndSelector()
            .Selector(".inspiration-card")
            .Property("background", "linear-gradient(135deg, var(--bg-card) 0%, var(--bg-sidebar) 100%)")
            .Property("border-radius", "20px")
            .Property("padding", "24px")
            .EndSelector()
            .Selector(".inspiration-icon")
            .Property("font-size", "32px")
            .Property("margin-bottom", "12px")
            .EndSelector()
            .Selector(".inspiration-text")
            .Property("font-style", "italic")
            .Property("font-size", "16px")
            .Property("line-height", "1.8")
            .EndSelector()
            .Selector(".quote")
            .Property("border-left", "4px solid var(--accent-primary)")
            .Property("padding-left", "16px")
            .Property("margin", "16px 0")
            .Property("font-style", "italic")
            .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".checkbox-box")
            .Property("width", "22px")
            .Property("height", "22px")
            .Property("border", "2px solid var(--border)")
            .Property("border-radius", "6px")
            .Property("background", "var(--bg-card)")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("justify-content", "center")
            .EndSelector()
            .Selector(".checkbox-box.checked")
            .Property("background", "var(--accent-primary)")
            .Property("border-color", "var(--accent-primary)")
            .Property("color", "white")
            .EndSelector();
    }
}
