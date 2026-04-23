// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");

using System.Text;

namespace SiliconLife.Default.Web;

public class H
{
    private static readonly HashSet<string> VoidElements = new(StringComparer.OrdinalIgnoreCase)
    {
        "area", "base", "br", "col", "embed", "hr", "img", "input",
        "link", "meta", "param", "source", "track", "wbr"
    };

    public string TagName { get; }
    private readonly Dictionary<string, string> _attrs = new();
    private readonly List<object> _children = new();
    private string? _text;
    private bool _isVoid;

    private H(string tagName, bool isVoid = false)
    {
        TagName = tagName;
        _isVoid = isVoid;
    }

    public static H Create(string tagName, params object[] children)
    {
        var el = new H(tagName);
        AddChildren(el, children);
        return el;
    }

    private static void AddChildren(H el, System.Collections.IEnumerable children)
    {
        foreach (var child in children)
        {
            if (child is null) continue;
            if (child is H h) el._children.Add(h);
            else if (child is string s) el._children.Add(s);
            else if (child is JsSyntax js) el._children.Add(js);
            else if (child is CssBuilder css) el._children.Add(css);
            else if (child is RawHtml raw) el._children.Add(raw);
            else if (child is object[] arr) AddChildren(el, arr);
            else if (child is System.Collections.IEnumerable nested) AddChildren(el, nested);
        }
    }

    public static H Create(string tagName, string text)
    {
        var el = new H(tagName);
        el._text = text;
        return el;
    }

    public static H Br() => new("br", true);
    public static H Hr() => new("hr", true);
    public static H Img() => new("img", true);
    public static H Input() => new("input", true);
    public static H Meta() => new("meta", true);
    public static H Link() => new("link", true);
    public static H Div(params object[] children) => Create("div", children);
    public static H Span(params object[] children) => Create("span", children);
    public static H P(params object[] children) => Create("p", children);
    public static H A(params object[] children) => Create("a", children);
    public static H H1(params object[] children) => Create("h1", children);
    public static H H2(params object[] children) => Create("h2", children);
    public static H H3(params object[] children) => Create("h3", children);
    public static H H4(params object[] children) => Create("h4", children);
    public static H Button(params object[] children) => Create("button", children);
    public static H InputText(params object[] children) => Create("input", children);
    public static H Textarea(params object[] children) => Create("textarea", children);
    public static H Select(params object[] children) => Create("select", children);
    public static H Option(params object[] children) => Create("option", children);
    public static H Table(params object[] children) => Create("table", children);
    public static H Tr(params object[] children) => Create("tr", children);
    public static H Th(params object[] children) => Create("th", children);
    public static H Td(params object[] children) => Create("td", children);
    public static H Thead(params object[] children) => Create("thead", children);
    public static H Tbody(params object[] children) => Create("tbody", children);
    public static H Pre(params object[] children) => Create("pre", children);
    public static H Code(params object[] children) => Create("code", children);
    public static H Blockquote(params object[] children) => Create("blockquote", children);
    public static H Ul(params object[] children) => Create("ul", children);
    public static H Li(params object[] children) => Create("li", children);
    public static H Label(params object[] children) => Create("label", children);
    public static H Form(params object[] children) => Create("form", children);
    public static H Script(params object[] children) => Create("script", children);
    public static H Style(params object[] children) => Create("style", children);
    public static H Details(params object[] children) => Create("details", children);
    public static H Summary(params object[] children) => Create("summary", children);
    public static H Header(params object[] children) => Create("header", children);
    public static H Aside(params object[] children) => Create("aside", children);
    public static H MainElement(params object[] children) => Create("main", children);
    public static H Nav(params object[] children) => Create("nav", children);
    public static H Section(params object[] children) => Create("section", children);
    public static H Article(params object[] children) => Create("article", children);
    public static H Footer(params object[] children) => Create("footer", children);
    public static H Svg(params object[] children) => Create("svg", children);
    public static H Html(params object[] children) => Create("html", children);
    public static H Head(params object[] children) => Create("head", children);
    public static H Body(params object[] children) => Create("body", children);
    public static H Title(params object[] children) => Create("title", children);
    public static H PageElement(string title, params object[] children) => Create("html", 
        Create("head", Create("title", title)),
        Create("body", children)
    );

    public static string T(string text) => text;

    public static string DocType() => "<!DOCTYPE html>";

    public H Attr(string name, string value)
    {
        _attrs[name] = value;
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
    public H OnClick(string handler) => Attr("onclick", handler);
    public H OnChange(string handler) => Attr("onchange", handler);
    public H Placeholder(string text) => Attr("placeholder", text);
    public H Data(string name, string value) => Attr($"data-{name}", value);
    public H Value(string value) => Attr("value", value);
    public H Href(string url) => Attr("href", url);
    public H Style(string style) => Attr("style", style);
    public H Selected() => Attr("selected", "selected");
    public H Checked() => Attr("checked", "checked");
    public H Disabled() => Attr("disabled", "disabled");

    public H Text(string text)
    {
        _text = text;
        return this;
    }

    public H Add(H child)
    {
        _children.Add(child);
        return this;
    }

    public H Add(string text)
    {
        _children.Add(text);
        return this;
    }

    public H Add(JsSyntax js)
    {
        _children.Add(js);
        return this;
    }

    public H Add(CssBuilder css)
    {
        _children.Add(css);
        return this;
    }

    public H Add(RawHtml raw)
    {
        _children.Add(raw);
        return this;
    }

    private const string IndentString = "    ";

    public string Build()
    {
        var sb = new StringBuilder();
        BuildTo(sb, 0);
        return sb.ToString();
    }

    public override string ToString() => Build();

    private void BuildTo(StringBuilder sb, int indentLevel)
    {
        string indent = new string(' ', indentLevel * 4);

        sb.Append(indent).Append('<').Append(TagName);

        foreach (var (k, v) in _attrs)
            sb.Append(' ').Append(k).Append("=\"").Append(EscapeAttr(v)).Append('"');

        if (_isVoid || VoidElements.Contains(TagName))
        {
            sb.Append(" />\n");
            return;
        }

        sb.Append('>');

        if (_text != null)
        {
            sb.Append(EscapeText(_text)).Append("</").Append(TagName).Append(">\n");
            return;
        }

        if (_children.Count == 0)
        {
            sb.Append("</").Append(TagName).Append(">\n");
            return;
        }

        sb.Append('\n');

        foreach (var child in _children)
        {
            switch (child)
            {
                case H h:
                    h.BuildTo(sb, indentLevel + 1);
                    break;
                case string s:
                    sb.Append(indent).Append(IndentString).Append(EscapeText(s)).Append('\n');
                    break;
                case JsSyntax js:
                    sb.Append(indent).Append(IndentString).Append(js.Build()).Append('\n');
                    break;
                case CssBuilder css:
                    sb.Append(indent).Append(IndentString).Append(css.Build()).Append('\n');
                    break;
                case RawHtml raw:
                    sb.Append(raw.Content).Append('\n');
                    break;
            }
        }

        sb.Append(indent).Append("</").Append(TagName).Append(">\n");
    }

    private static string EscapeText(string s) => s
        .Replace("&", "&amp;")
        .Replace("<", "&lt;")
        .Replace(">", "&gt;");

    private static string EscapeAttr(string s) => s
        .Replace("&", "&amp;")
        .Replace("<", "&lt;")
        .Replace(">", "&gt;")
        .Replace("\"", "&quot;");
}

public static class HExtensions
{
    public static H When(this H el, bool condition, object child)
    {
        if (condition)
        {
            if (child is H h) el.Add(h);
            else if (child is string s) el.Add(s);
            else if (child is JsSyntax js) el.Add(js);
            else if (child is CssBuilder css) el.Add(css);
            else if (child is RawHtml raw) el.Add(raw);
        }
        return el;
    }
}

public class RawHtml
{
    public string Content { get; }

    public RawHtml(string content)
    {
        Content = content;
    }

    public static RawHtml From(string content) => new(content);
}
