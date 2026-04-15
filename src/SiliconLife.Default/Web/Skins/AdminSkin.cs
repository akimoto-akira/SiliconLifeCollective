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
        return H.PageElement("错误 - Silicon Life Collective",
            new object[]
            {
                H.Style(GetStyles() + GetThemeCss().Build()),
            },
            new object[]
            {
                H.Div(
                    H.Div(
                        H.H1("出错了"),
                        message,
                        H.A("返回首页").Href("/")
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

    public JsSyntax GetScripts()
    {
        return new JsBlock();
    }

    public H RenderButton(string text, string variant = "primary", string size = "medium")
    {
        var cls = "btn";
        if (variant != "secondary") cls += $" btn-{variant}";
        if (variant == "outline") cls = "btn btn-outline";
        if (size != "medium") cls += $" btn-{size}";

        return H.Button(text).Class(cls);
    }

    public H RenderInput(string placeholder = "", string size = "medium", string? value = null)
    {
        var cls = "input";
        if (size != "medium") cls += $" input-{size}";

        return H.InputText().Placeholder(placeholder).Value(value ?? "").Class(cls);
    }

    public H RenderTextarea(string placeholder = "", int rows = 4)
    {
        return H.Textarea().Placeholder(placeholder).Attr("rows", rows.ToString()).Class("input textarea");
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
            H.Div(title).Class("card-header"),
            H.Div(content).Class("card-body")
        ).Class("card");
    }

    public H RenderAvatar(string text, string size = "medium")
    {
        return H.Div(text).Class($"avatar avatar-{size}");
    }

    public H RenderBubble(string text, bool isMine = false)
    {
        return H.Div(text).Class($"bubble{(isMine ? " mine" : string.Empty)}");
    }

    public H RenderSwitch(bool isChecked = false)
    {
        var switchClass = "switch" + (isChecked ? " active" : string.Empty);
        return H.Div().Class(switchClass);
    }

    public H RenderProgress(double value, string variant = "primary")
    {
        return H.Div(
            H.Div().Class("progress-bar").Style($"width: {value}%")
        ).Class("progress");
    }

    public H RenderTabs(IEnumerable<string> tabs, int activeIndex = 0)
    {
        var items = new List<object>();
        var idx = 0;
        foreach (var tab in tabs)
        {
            items.Add(H.Span(tab).Class($"tab{(idx == activeIndex ? " active" : string.Empty)}"));
            idx++;
        }
        return H.Div(items);
    }

    public H RenderListItem(string title, string? subtitle = null, string? avatar = null, bool active = false)
    {
        var children = new List<object>();
        if (!string.IsNullOrEmpty(avatar))
        {
            children.Add(avatar + " ");
        }
        children.Add(title);
        return H.Div(children).Class($"nav-item{(active ? " active" : string.Empty)}");
    }

    public H RenderDivider()
    {
        return H.Hr().Class("divider");
    }

    public H RenderCode(string code)
    {
        return H.Pre(H.Code(code));
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
            if (!isFirst) children.Add(" / ");
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
                cells.Add(H.Td(cell));
            }
            bodyRows.Add(H.Tr(cells));
        }

        return H.Table(
            H.Thead(H.Tr(thCells)),
            H.Tbody(bodyRows)
        ).Class("table");
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
            H.Button($"{triggerText} ▼").Class("btn btn-primary"),
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
            .WithVariable("bg-primary", "#f4f6f8")
            .WithVariable("bg-secondary", "#ffffff")
            .WithVariable("bg-tertiary", "#e9ecef")
            .WithVariable("bg-white", "#ffffff")
            .WithVariable("bg-card", "#ffffff")
            .WithVariable("bg-sidebar", "#1a1a2e")
            .WithVariable("border", "#e1e4e8")
            .WithVariable("border-color", "#e1e4e8")
            .WithVariable("text-primary", "#24292e")
            .WithVariable("text-secondary", "#586069")
            .WithVariable("text-inverse", "#ffffff")
            .WithVariable("accent-primary", "#0366d6")
            .WithVariable("accent-color", "#0366d6")
            .WithVariable("accent-success", "#28a745")
            .WithVariable("accent-warning", "#f9a825")
            .WithVariable("accent-danger", "#d32f2f")
            .WithVariable("accent-error", "#d32f2f")
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
