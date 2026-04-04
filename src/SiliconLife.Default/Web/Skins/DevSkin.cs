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
    public string Code => "dev";
    public string Name => "Dev";

    public SkinPreviewInfo PreviewInfo => new()
    {
        Icon = "\u2699\ufe0f",
        Description = "Geek dark theme",
        BackgroundColor = "#0d1117",
        CardColor = "#161b22",
        AccentColor = "#58a6ff",
        TextColor = "#c9d1d9",
        BorderColor = "#30363d"
    };

    public H RenderHtml(H content)
    {
        return H.PageElement("Silicon Life Collective",
            new object[]
            {
                H.Style(GetStyles() + GetThemeCss().Build()),
                H.Script(GetScripts().Build()),
            },
            new object[]
            {
                H.Div(content).Class("container"),
            });
    }

    public H RenderError(H message)
    {
        return H.PageElement("Error - Silicon Life Collective",
            new object[]
            {
                H.Style(GetStyles() + GetThemeCss().Build()),
            },
            new object[]
            {
                H.Div(
                    H.Div(
                        H.H1("ERROR"),
                        message,
                        H.A("Back to Home").Href("/")
                    ).Class("error")
                ).Class("container"),
            });
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

    public H RenderButton(string text, string variant = "primary", string size = "medium")
    {
        var cls = "btn";
        if (variant == "primary") cls += " btn-primary";
        if (size != "medium") cls += $" btn-{size}";

        return H.Button(text).Class(cls);
    }

    public H RenderInput(string placeholder = "", string size = "medium", string? value = null)
    {
        var cls = "input";
        if (size != "medium") cls += $" input-{size}";

        return H.InputText(placeholder: placeholder, value: value).Class(cls);
    }

    public H RenderTextarea(string placeholder = "", int rows = 4)
    {
        return H.Textarea().Placeholder(placeholder).Attr("rows", rows).Class("input textarea");
    }

    public H RenderSelect(IEnumerable<string> options, string? selected = null)
    {
        var select = H.Select();
        foreach (var opt in options)
        {
            var option = H.Option(opt).Value(opt);
            if (opt == selected) option.Selected();
            select.Add(option);
        }
        return select.Class("select");
    }

    public H RenderCheckbox(string label, bool isChecked = false)
    {
        var checkMark = isChecked ? "✓" : string.Empty;
        var checkboxBoxClass = "checkbox-box" + (isChecked ? " checked" : string.Empty);
        return H.Div(
            H.Div(checkMark).Class(checkboxBoxClass),
            H.Span(label)
        ).Class("checkbox");
    }

    public H RenderBadge(string text, string variant = "primary")
    {
        return H.Span(text).Class($"badge badge-{variant}");
    }

    public H RenderTag(string text)
    {
        return H.Span(text).Class("tag");
    }

    public H RenderCard(string title, string content)
    {
        return H.Div(
            H.Div(H.Raw($"// {title}")).Class("card-header"),
            H.Div(H.Raw(content)).Class("card-body")
        ).Class("card");
    }

    public H RenderAvatar(string text, string size = "medium")
    {
        return H.Div(text).Class($"avatar avatar-{size}");
    }

    public H RenderBubble(string text, bool isMine = false)
    {
        return H.Div($"// {text}").Class($"bubble{(isMine ? " mine" : string.Empty)}");
    }

    public H RenderSwitch(bool isChecked = false)
    {
        var switchClass = "switch" + (isChecked ? " active" : string.Empty);
        return H.Div().Class(switchClass);
    }

    public H RenderProgress(double value, string variant = "primary")
    {
        return H.Div(
            H.Div().Class($"progress-bar progress-{variant}").Style($"width: {value}%")
        ).Class("progress");
    }

    public H RenderTabs(IEnumerable<string> tabs, int activeIndex = 0)
    {
        var items = new List<object>();
        var idx = 0;
        foreach (var tab in tabs)
        {
            items.Add(H.Span($"/{tab.ToLower()}").Class($"tab{(idx == activeIndex ? " active" : string.Empty)}"));
            idx++;
        }
        return H.Div(items);
    }

    public H RenderListItem(string title, string? subtitle = null, string? avatar = null, bool active = false)
    {
        var children = new List<object>();
        if (!string.IsNullOrEmpty(avatar))
        {
            children.Add(H.Div(avatar).Class("avatar"));
        }
        children.Add(H.Span(title));
        if (!string.IsNullOrEmpty(subtitle))
        {
            children.Add(H.Span(subtitle).Class("badge badge-success"));
        }
        return H.Div(children).Class($"list-item{(active ? " active" : string.Empty)}");
    }

    public H RenderDivider()
    {
        return H.Hr();
    }

    public H RenderCode(string code)
    {
        return H.Pre(H.Code(code).Class("code"));
    }

    public H RenderStatCard(string label, string value, string variant = "primary")
    {
        return H.Div(
            H.Div(value).Class("stat-value").Style($"color: var(--accent-{variant})"),
            H.Div(label).Class("stat-label")
        ).Class("stat-card");
    }

    public H RenderBreadcrumb(IEnumerable<string> items)
    {
        var children = new List<object>();
        var isFirst = true;
        foreach (var item in items)
        {
            if (!isFirst) children.Add(H.Raw(" / "));
            children.Add(H.Span(item));
            isFirst = false;
        }
        return H.Div(children).Class("breadcrumb");
    }

    public H RenderTable(IEnumerable<string> headers, IEnumerable<IEnumerable<string>> rows)
    {
        var thCells = new List<object>();
        foreach (var header in headers)
        {
            thCells.Add(H.Th(header));
        }

        var bodyRows = new List<object>();
        foreach (var row in rows)
        {
            var cells = new List<object>();
            foreach (var cell in row)
            {
                cells.Add(H.Td(H.Raw(cell)));
            }
            bodyRows.Add(H.Tr(cells));
        }

        return H.Table(
            H.Thead(H.Tr(thCells)),
            H.Tbody(bodyRows)
        );
    }

    public H RenderPagination(int totalPages, int currentPage = 1)
    {
        var items = new List<object>
        {
            H.Div("‹").Class("page-btn"),
        };

        for (int i = 1; i <= totalPages; i++)
        {
            items.Add(H.Div(i.ToString()).Class($"page-btn{(i == currentPage ? " active" : string.Empty)}"));
        }

        items.Add(H.Div("›").Class("page-btn"));
        return H.Div(items).Class("pagination");
    }

    public H RenderDropdown(string triggerText, IEnumerable<string> items)
    {
        var menuItems = new List<object>();
        foreach (var item in items)
        {
            menuItems.Add(H.Div(item).Class("dropdown-item"));
        }

        return H.Div(
            H.Button($"{triggerText} ▼").Class("btn"),
            H.Div(menuItems).Class("dropdown-menu")
        ).Class("dropdown");
    }

    public H RenderStatusIndicator(string status)
    {
        var statusClass = status.ToLower() switch
        {
            "online" => "status-online",
            "offline" => "status-offline",
            "busy" => "status-busy",
            _ => "status-offline"
        };

        return H.Span().Class($"status-dot {statusClass}");
    }

    public H RenderQuote(string text)
    {
        return H.Blockquote(text);
    }

    public H RenderInspirationCard(string icon, string text)
    {
        return H.Div(
            H.Div(icon).Class("inspiration-icon"),
            H.Div($"\"{text}\"").Class("inspiration-text")
        ).Class("inspiration-card");
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
