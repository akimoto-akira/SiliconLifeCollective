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

    public JsSyntax GetScripts()
    {
        return new JsBlock();
    }

    public H RenderButton(string text, string variant = "primary", string size = "medium")
    {
        var cls = $"btn btn-{variant}";
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
        return select;
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
            .WithVariable("bg-primary", "#1a1a2e")
            .WithVariable("bg-card", "#16213e")
            .WithVariable("bg-secondary", "#1e3a5f")
            .WithVariable("bg-tertiary", "#0f3460")
            .WithVariable("border", "#0f3460")
            .WithVariable("border-color", "#0f3460")
            .WithVariable("text-primary", "#eaeaea")
            .WithVariable("text-secondary", "#a0a0a0")
            .WithVariable("accent-primary", "#4d96ff")
            .WithVariable("accent-color", "#4d96ff")
            .WithVariable("accent-success", "#6bcb77")
            .WithVariable("accent-warning", "#ffd93d")
            .WithVariable("accent-error", "#ff6b6b")
            .WithVariable("accent-info", "#4ecdc4");
    }
}
