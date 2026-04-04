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

public class AdminSkin : ISkin
{
    public string Code => "admin";
    public string Name => "Admin";

    public SkinPreviewInfo PreviewInfo => new()
    {
        Icon = "\U0001f4ca",
        Description = "Classic dashboard",
        BackgroundColor = "#f4f6f8",
        CardColor = "#ffffff",
        AccentColor = "#0366d6",
        TextColor = "#24292e",
        BorderColor = "#e1e4e8"
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
            .Selector("*")
            .Property("box-sizing", "border-box")
            .Property("margin", "0")
            .Property("padding", "0")
            .EndSelector()
            .Selector("body")
            .Property("font-family", "-apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif")
            .Property("background", "var(--bg-primary)")
            .Property("color", "var(--text-primary)")
            .Property("font-size", "14px")
            .Property("padding", "24px")
            .EndSelector()
            .Selector(".container")
            .Property("max-width", "1200px")
            .Property("margin", "0 auto")
            .EndSelector()
            .Selector(".error")
            .Property("background", "var(--bg-white)")
            .Property("padding", "40px")
            .Property("border-radius", "8px")
            .Property("text-align", "center")
            .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".error h1")
            .Property("color", "var(--accent-danger)")
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
        if (variant != "secondary") cls += $" btn-{variant}";
        if (variant == "outline") cls = "btn btn-outline";
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
            .Div().Class($"nav-item{(active ? " active" : string.Empty)}");
        
        if (!string.IsNullOrEmpty(avatar))
        {
            builder.Text(avatar + " ");
        }
        
        builder.Text(title);
        
        return builder.EndBlock();
    }

    public HtmlBuilder RenderDivider()
    {
        return HtmlBuilder.Create().Hr().Class("divider");
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
        var builder = HtmlBuilder.Create().Table().Class("table");
        
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
            .WithVariable("bg-primary", "#f4f6f8")
            .WithVariable("bg-white", "#ffffff")
            .WithVariable("bg-sidebar", "#1a1a2e")
            .WithVariable("border", "#e1e4e8")
            .WithVariable("text-primary", "#24292e")
            .WithVariable("text-secondary", "#586069")
            .WithVariable("text-inverse", "#ffffff")
            .WithVariable("accent-primary", "#0366d6")
            .WithVariable("accent-success", "#28a745")
            .WithVariable("accent-warning", "#f9a825")
            .WithVariable("accent-danger", "#d32f2f")
            .Selector(".btn")
            .Property("padding", "8px 16px")
            .Property("border-radius", "6px")
            .Property("border", "1px solid var(--border)")
            .Property("background", "var(--bg-white)")
            .Property("color", "var(--text-primary)")
            .Property("cursor", "pointer")
            .Property("font-size", "13px")
            .Property("font-weight", "500")
            .EndSelector()
            .Selector(".btn-primary")
            .Property("background", "var(--accent-primary)")
            .Property("border-color", "var(--accent-primary)")
            .Property("color", "white")
            .EndSelector()
            .Selector(".btn-success")
            .Property("background", "var(--accent-success)")
            .Property("border-color", "var(--accent-success)")
            .Property("color", "white")
            .EndSelector()
            .Selector(".btn-warning")
            .Property("background", "var(--accent-warning)")
            .Property("border-color", "var(--accent-warning)")
            .Property("color", "white")
            .EndSelector()
            .Selector(".btn-danger")
            .Property("background", "var(--accent-danger)")
            .Property("border-color", "var(--accent-danger)")
            .Property("color", "white")
            .EndSelector()
            .Selector(".btn-outline")
            .Property("background", "transparent")
            .Property("border-color", "var(--border)")
            .EndSelector()
            .Selector(".input")
            .Property("background", "var(--bg-white)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "6px")
            .Property("padding", "10px 12px")
            .Property("color", "var(--text-primary)")
            .Property("font-size", "14px")
            .EndSelector()
            .Selector(".select")
            .Property("background", "var(--bg-white)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "6px")
            .Property("padding", "10px 12px")
            .Property("color", "var(--text-primary)")
            .Property("font-size", "14px")
            .EndSelector()
            .Selector(".card")
            .Property("background", "var(--bg-white)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "8px")
            .Property("padding", "16px")
            .EndSelector()
            .Selector(".stat-card")
            .Property("background", "var(--bg-white)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "8px")
            .Property("padding", "20px")
            .EndSelector()
            .Selector(".stat-value")
            .Property("font-size", "28px")
            .Property("font-weight", "600")
            .EndSelector()
            .Selector(".stat-label")
            .Property("font-size", "13px")
            .Property("color", "var(--text-secondary)")
            .Property("margin-top", "4px")
            .EndSelector()
            .Selector(".badge")
            .Property("display", "inline-block")
            .Property("padding", "4px 10px")
            .Property("border-radius", "12px")
            .Property("font-size", "12px")
            .Property("font-weight", "500")
            .EndSelector()
            .Selector(".badge-primary")
            .Property("background", "rgba(3, 102, 214, 0.1)")
            .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".badge-success")
            .Property("background", "rgba(40, 167, 69, 0.1)")
            .Property("color", "var(--accent-success)")
            .EndSelector()
            .Selector(".badge-warning")
            .Property("background", "rgba(249, 168, 37, 0.1)")
            .Property("color", "var(--accent-warning)")
            .EndSelector()
            .Selector(".badge-danger")
            .Property("background", "rgba(211, 47, 47, 0.1)")
            .Property("color", "var(--accent-danger)")
            .EndSelector()
            .Selector(".breadcrumb")
            .Property("font-size", "14px")
            .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".table")
            .Property("width", "100%")
            .Property("border-collapse", "collapse")
            .EndSelector()
            .Selector(".table th, .table td")
            .Property("padding", "12px")
            .Property("text-align", "left")
            .Property("border-bottom", "1px solid var(--border)")
            .Property("font-size", "13px")
            .EndSelector()
            .Selector(".table th")
            .Property("background", "var(--bg-primary)")
            .Property("font-weight", "600")
            .EndSelector()
            .Selector(".nav-item")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("gap", "10px")
            .Property("padding", "10px 16px")
            .Property("color", "rgba(255,255,255,0.7)")
            .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".sidebar")
            .Property("width", "200px")
            .Property("background", "var(--bg-sidebar)")
            .Property("color", "var(--text-inverse)")
            .Property("padding", "16px 0")
            .Property("border-radius", "8px")
            .EndSelector()
            .Selector(".switch")
            .Property("width", "44px")
            .Property("height", "24px")
            .Property("background", "var(--border)")
            .Property("border-radius", "12px")
            .Property("position", "relative")
            .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".progress")
            .Property("height", "8px")
            .Property("background", "var(--bg-primary)")
            .Property("border-radius", "4px")
            .Property("overflow", "hidden")
            .EndSelector()
            .Selector(".progress-bar")
            .Property("height", "100%")
            .Property("background", "var(--accent-primary)")
            .Property("border-radius", "4px")
            .EndSelector()
            .Selector(".tab")
            .Property("padding", "12px 20px")
            .Property("border-bottom", "2px solid transparent")
            .Property("cursor", "pointer")
            .Property("font-size", "14px")
            .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".tag")
            .Property("display", "inline-flex")
            .Property("align-items", "center")
            .Property("gap", "4px")
            .Property("padding", "4px 8px")
            .Property("background", "var(--bg-primary)")
            .Property("border-radius", "4px")
            .Property("font-size", "12px")
            .EndSelector()
            .Selector(".dropdown-menu")
            .Property("position", "absolute")
            .Property("top", "100%")
            .Property("left", "0")
            .Property("background", "var(--bg-white)")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "6px")
            .Property("padding", "4px 0")
            .Property("min-width", "120px")
            .EndSelector()
            .Selector(".dropdown-item")
            .Property("padding", "8px 16px")
            .Property("font-size", "13px")
            .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".page-btn")
            .Property("width", "32px")
            .Property("height", "32px")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("justify-content", "center")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "6px")
            .Property("cursor", "pointer")
            .Property("font-size", "13px")
            .EndSelector()
            .Selector(".checkbox-box")
            .Property("width", "18px")
            .Property("height", "18px")
            .Property("border", "1px solid var(--border)")
            .Property("border-radius", "4px")
            .Property("background", "var(--bg-white)")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("justify-content", "center")
            .EndSelector()
            .Selector(".avatar")
            .Property("width", "40px")
            .Property("height", "40px")
            .Property("border-radius", "50%")
            .Property("background", "var(--border)")
            .Property("display", "flex")
            .Property("align-items", "center")
            .Property("justify-content", "center")
            .EndSelector()
            .Selector(".bubble")
            .Property("background", "var(--bg-primary)")
            .Property("padding", "12px 16px")
            .Property("border-radius", "18px")
            .Property("font-size", "14px")
            .Property("max-width", "300px")
            .EndSelector()
            .Selector(".divider")
            .Property("height", "1px")
            .Property("background", "var(--border)")
            .Property("margin", "16px 0")
            .EndSelector();
    }
}
