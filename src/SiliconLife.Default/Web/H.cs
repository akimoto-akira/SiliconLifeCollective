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

using System.Text;

namespace SiliconLife.Default.Web;

/// <summary>
/// XElement-style HTML element builder.
/// Structure = hierarchy. No EndBlock needed. Attributes always apply to the correct element.
/// </summary>
public class H
{
    private static readonly HashSet<string> VoidElements = new(StringComparer.OrdinalIgnoreCase)
    {
        "area", "base", "br", "col", "embed", "hr", "img", "input",
        "link", "meta", "param", "source", "track", "wbr"
    };

    private readonly string _tag;
    private readonly Dictionary<string, string> _attrs = new();
    private readonly List<object> _children = new(); // H | string | RawNode
    private bool _isVoid;
    private string? _text;

    private H(string tag, bool isVoid = false)
    {
        _tag = tag;
        _isVoid = isVoid;
    }

    // ── Factory: Tag(name, children...) ──────────────────────────────────

    /// <summary>Create an element: <c>H.Tag("div", "hello", H.Tag("span", "world"))</c></summary>
    public static H Tag(string tag, params object?[] children)
    {
        var el = new H(tag);
        el.AddChildren(children);
        return el;
    }

    // ── Convenience: H.Div(...), H.P(...), H.Span(...) etc. ─────────────

    public static H Div(params object?[] children) => Tag("div", children);
    public static H Span(params object?[] children) => Tag("span", children);
    public static H P(params object?[] children) => Tag("p", children);
    public static H A(params object?[] children) => Tag("a", children);
    public static H Ul(params object?[] children) => Tag("ul", children);
    public static H Ol(params object?[] children) => Tag("ol", children);
    public static H Li(params object?[] children) => Tag("li", children);
    public static H Nav(params object?[] children) => Tag("nav", children);
    public static H Main(params object?[] children) => Tag("main", children);
    public static H Section(params object?[] children) => Tag("section", children);
    public static H Article(params object?[] children) => Tag("article", children);
    public static H Aside(params object?[] children) => Tag("aside", children);
    public static H Header(params object?[] children) => Tag("header", children);
    public static H Footer(params object?[] children) => Tag("footer", children);
    public static H Details(params object?[] children) => Tag("details", children);
    public static H Summary(params object?[] children) => Tag("summary", children);
    public static H Form(params object?[] children) => Tag("form", children);
    public static H Fieldset(params object?[] children) => Tag("fieldset", children);
    public static H Legend(params object?[] children) => Tag("legend", children);
    public static H Pre(params object?[] children) => Tag("pre", children);
    public static H Code(params object?[] children) => Tag("code", children);
    public static H Blockquote(params object?[] children) => Tag("blockquote", children);
    public static H Table(params object?[] children) => Tag("table", children);
    public static H Thead(params object?[] children) => Tag("thead", children);
    public static H Tbody(params object?[] children) => Tag("tbody", children);
    public static H Tr(params object?[] children) => Tag("tr", children);
    public static H Th(params object?[] children) => Tag("th", children);
    public static H Td(params object?[] children) => Tag("td", children);
    public static H Button(params object?[] children) => Tag("button", children);
    public static H Select(params object?[] children) => Tag("select", children);
    public static H Option(params object?[] children) => Tag("option", children);
    public static H Optgroup(params object?[] children) => Tag("optgroup", children);
    public static H Textarea(params object?[] children) => Tag("textarea", children);
    public static H Label(params object?[] children) => Tag("label", children);
    public static H Script(params object?[] children) => Tag("script", children);
    public static H Style(params object?[] children) => Tag("style", children);
    public static H I(params object?[] children) => Tag("i", children);
    public static H B(params object?[] children) => Tag("b", children);
    public static H Em(params object?[] children) => Tag("em", children);
    public static H Strong(params object?[] children) => Tag("strong", children);
    public static H Small(params object?[] children) => Tag("small", children);
    public static H H1(params object?[] children) => Tag("h1", children);
    public static H H2(params object?[] children) => Tag("h2", children);
    public static H H3(params object?[] children) => Tag("h3", children);
    public static H H4(params object?[] children) => Tag("h4", children);
    public static H H5(params object?[] children) => Tag("h5", children);
    public static H H6(params object?[] children) => Tag("h6", children);
    public static H Dl(params object?[] children) => Tag("dl", children);
    public static H Dt(params object?[] children) => Tag("dt", children);
    public static H Dd(params object?[] children) => Tag("dd", children);
    public static H Figure(params object?[] children) => Tag("figure", children);
    public static H Figcaption(params object?[] children) => Tag("figcaption", children);
    public static H Video(params object?[] children) => Tag("video", children);
    public static H Audio(params object?[] children) => Tag("audio", children);
    public static H Canvas(params object?[] children) => Tag("canvas", children);
    public static H Iframe(params object?[] children) => Tag("iframe", children);
    public static H Body(params object?[] children) => Tag("body", children);

    // ── Void (self-closing) elements ─────────────────────────────────────

    public static H Br() => new("br", true);
    public static H Hr() => new("hr", true);
    public static H Img() => new("img", true);
    public static H Input() => new("input", true);
    public static H Meta() => new("meta", true);
    public static H Link() => new("link", true);

    // ── Special: text node shorthand ─────────────────────────────────────

    /// <summary>Create a plain text node (no wrapping tag).</summary>
    public static TextNode Text(string text) => new(text);

    /// <summary>Create a raw HTML node (no escaping).</summary>
    public static RawNode Raw(string html) => new(html);

    /// <summary>Create an HTML comment node.</summary>
    public static CommentNode Comment(string text) => new(text);

    /// <summary>Conditional: if true, include the node; otherwise null.</summary>
    public static object? When(bool condition, object? node) => condition ? node : null;

    // ── Attribute setters (all return this) ──────────────────────────────

    public H Attr(string name, object? value)
    {
        if (value != null)
            _attrs[name] = value.ToString()!;
        return this;
    }

    public H Id(string id) => Attr("id", id);

    public H Class(string className)
    {
        if (_attrs.TryGetValue("class", out var existing))
            _attrs["class"] = existing + " " + className;
        else
            _attrs["class"] = className;
        return this;
    }

    public H Cls(params string[] classes)
    {
        if (classes.Length > 0)
            _attrs["class"] = string.Join(" ", classes);
        return this;
    }

    public H Data(string key, object value) => Attr($"data-{key}", value);

    public H Href(string href) => Attr("href", href);

    public H Src(string src) => Attr("src", src);

    public H Alt(string alt) => Attr("alt", alt);

    public H Type(string type) => Attr("type", type);

    public H Name(string name) => Attr("name", name);

    public H Value(string value) => Attr("value", value);

    public H Placeholder(string ph) => Attr("placeholder", ph);

    public H For(string @for) => Attr("for", @for);

    public H Action(string action) => Attr("action", action);

    public H Method(string method) => Attr("method", method);

    public H Rel(string rel) => Attr("rel", rel);

    public H Charset(string charset) => Attr("charset", charset);

    public H Defer() => Attr("defer", "defer");

    public H OnClick(string handler) => Attr("onclick", handler);

    public H OnSubmit(string handler) => Attr("onsubmit", handler);

    public H OnChange(string handler) => Attr("onchange", handler);

    public H Style(string style) => Attr("style", style);

    public H Title(string title) => Attr("title", title);

    public H Checked() => Attr("checked", "checked");

    public H Selected() => Attr("selected", "selected");

    public H Disabled() => Attr("disabled", "disabled");

    public H Readonly() => Attr("readonly", "readonly");

    public H Required() => Attr("required", "required");

    public H Hidden() => Attr("hidden", "hidden");

    public H Role(string role) => Attr("role", role);

    // ── Convenience shortcuts for common elements ────────────────────────

    public static H InputText(string? name = null, string? placeholder = null, string? value = null)
        => Input().Type("text").Name(name ?? "").Placeholder(placeholder ?? "").Value(value ?? "");

    public static H InputPassword(string? name = null, string? placeholder = null)
        => Input().Type("password").Name(name ?? "").Placeholder(placeholder ?? "");

    public static H InputHidden(string? name = null, string? value = null)
        => Input().Type("hidden").Name(name ?? "").Value(value ?? "");

    public static H InputEmail(string? name = null, string? placeholder = null)
        => Input().Type("email").Name(name ?? "").Placeholder(placeholder ?? "");

    public static H InputNumber(string? name = null, string? placeholder = null, string? value = null)
        => Input().Type("number").Name(name ?? "").Placeholder(placeholder ?? "").Value(value ?? "");

    public static H InputCheckbox(string? name = null, string? value = null, bool @checked = false)
    {
        var input = Input().Type("checkbox").Name(name ?? "").Value(value ?? "");
        if (@checked) input.Checked();
        return input;
    }

    public static H InputRadio(string? name = null, string? value = null, bool @checked = false)
    {
        var input = Input().Type("radio").Name(name ?? "").Value(value ?? "");
        if (@checked) input.Checked();
        return input;
    }

    public static H InputSubmit(string? value = null, string? name = null)
        => Input().Type("submit").Value(value ?? "").Name(name ?? "");

    public static H InputFile(string? name = null)
        => Input().Type("file").Name(name ?? "");

    public static H InputSearch(string? name = null, string? placeholder = null)
        => Input().Type("search").Name(name ?? "").Placeholder(placeholder ?? "");

    public static H InputDate(string? name = null, string? value = null)
        => Input().Type("date").Name(name ?? "").Value(value ?? "");

    public static H InputTime(string? name = null, string? value = null)
        => Input().Type("time").Name(name ?? "").Value(value ?? "");

    public static H InputColor(string? name = null, string? value = null)
        => Input().Type("color").Name(name ?? "").Value(value ?? "");

    public static H MetaCharset(string charset = "utf-8")
        => Meta().Charset(charset);

    public static H MetaViewport(string content = "width=device-width, initial-scale=1")
        => Meta().Attr("name", "viewport").Attr("content", content);

    public static H MetaName(string name, string content)
        => Meta().Attr("name", name).Attr("content", content);

    public static H Stylesheet(string href)
        => Link().Rel("stylesheet").Href(href);

    public static H ScriptSrc(string src, bool defer = false)
    {
        var s = Script().Src(src);
        if (defer) s.Defer();
        return s;
    }

    // ── DocType + HTML page skeleton ─────────────────────────────────────

    public static DocTypeNode DocType(string doctype = "html") => new(doctype);

    /// <summary>Build an HTML page element (without DOCTYPE): <c>H.PageElement(title, headChildren, bodyChildren)</c></summary>
    public static H PageElement(string title, object?[] headExtras, object?[] bodyContent)
        => Tag("html",
            Tag("head",
                MetaCharset(),
                MetaViewport(),
                Tag("title", title),
                headExtras
            ),
            Tag("body", bodyContent)
        );

    /// <summary>Build a full HTML page string: <c>H.HtmlPage(title, headChildren, bodyChildren)</c></summary>
    public static string HtmlPage(string title, object?[] headExtras, object?[] bodyContent)
        => new DocTypeNode().Build() + PageElement(title, headExtras, bodyContent).Build();

    // ── Render ───────────────────────────────────────────────────────────

    public string Build()
    {
        var sb = new StringBuilder();
        BuildTo(sb);
        return sb.ToString();
    }

    public override string ToString() => Build();

    private void BuildTo(StringBuilder sb)
    {
        sb.Append('<');
        sb.Append(_tag);

        foreach (var (k, v) in _attrs)
        {
            sb.Append(' ').Append(k).Append("=\"").Append(EscapeAttr(v)).Append('"');
        }

        if (_isVoid || VoidElements.Contains(_tag))
        {
            sb.Append(" />");
            return;
        }

        sb.Append('>');

        // Direct text content
        if (_text != null)
        {
            sb.Append(EscapeText(_text));
        }

        // Children
        foreach (var child in _children)
        {
            switch (child)
            {
                case null:
                    break;
                case H h:
                    h.BuildTo(sb);
                    break;
                case string s:
                    sb.Append(EscapeText(s));
                    break;
                case TextNode tn:
                    sb.Append(EscapeText(tn.Text));
                    break;
                case RawNode rn:
                    sb.Append(rn.Html);
                    break;
                case CommentNode cn:
                    sb.Append("<!-- ").Append(cn.Text).Append(" -->");
                    break;
            }
        }

        sb.Append("</").Append(_tag).Append('>');
    }

    internal void AddChildren(object?[] children)
    {
        foreach (var child in children)
        {
            if (child is null) continue;
            if (child is IEnumerable<object> items && child is not string)
            {
                foreach (var item in items)
                    _children.Add(item);
            }
            else
            {
                _children.Add(child);
            }
        }
    }

    internal void SetText(string text) => _text = text;

    private static string EscapeText(string s) => s
        .Replace("&", "&amp;")
        .Replace("<", "&lt;")
        .Replace(">", "&gt;");

    private static string EscapeAttr(string s) => s
        .Replace("&", "&amp;")
        .Replace("<", "&lt;")
        .Replace(">", "&gt;")
        .Replace("\"", "&quot;")
        .Replace("'", "&#39;");
}

// ── Special node types ──────────────────────────────────────────────────

public sealed class TextNode
{
    public string Text { get; }
    public TextNode(string text) => Text = text;
}

public sealed class RawNode
{
    public string Html { get; }
    public RawNode(string html) => Html = html;
}

public sealed class CommentNode
{
    public string Text { get; }
    public CommentNode(string text) => Text = text;
}

public sealed class DocTypeNode
{
    private readonly string _doctype;
    public DocTypeNode(string doctype = "html") => _doctype = doctype;
    public string Build() => $"<!DOCTYPE {_doctype}>";
}

// ── Extension: conditional rendering ────────────────────────────────────

public static class HExtensions
{
    /// <summary>Conditionally add children to an existing H element.</summary>
    public static H When(this H el, bool condition, params object?[] children)
    {
        if (condition)
            el.Add(children);
        return el;
    }

    /// <summary>Add children to an existing H element (for loops etc).</summary>
    public static H Add(this H el, params object?[] children)
    {
        el.AddChildren(children);
        return el;
    }

    /// <summary>Set text content on an element.</summary>
    public static H Text(this H el, string text)
    {
        el.SetText(text);
        return el;
    }
}
