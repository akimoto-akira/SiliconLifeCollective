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
    public string Code => "creative";
    public string Name => "Creative";

    public SkinPreviewInfo PreviewInfo => new()
    {
        Icon = "\u270f\ufe0f",
        Description = "Warm artistic",
        BackgroundColor = "#fdf6e3",
        CardColor = "#fffef9",
        AccentColor = "#d4a574",
        TextColor = "#5c4b37",
        BorderColor = "#e8dfd0"
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

    public JsSyntax GetScripts()
    {
        return new JsBlock();
    }

    public H RenderButton(string text, string variant = "primary", string size = "medium")
    {
        var cls = "btn";
        if (variant == "primary") cls = "btn btn-primary";
        else if (variant == "secondary") cls = "btn btn-secondary";
        else if (variant == "success") cls = "btn btn-success";
        else if (variant == "outline") cls = "btn btn-outline";

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
            children.Add(H.Div(avatar).Class("avatar"));
        }

        var innerChildren = new List<object> { title };
        if (!string.IsNullOrEmpty(subtitle))
        {
            innerChildren.Add(H.Div(subtitle).Style("font-size: 12px; color: var(--text-secondary);"));
        }
        children.Add(H.Div(innerChildren));

        return H.Div(children).Class($"list-item{(active ? " active" : string.Empty)}");
    }

    public H RenderDivider()
    {
        return H.Hr();
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
        ).Class("card-elevated");
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
        return H.Div($"\"{text}\"").Class("quote");
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
