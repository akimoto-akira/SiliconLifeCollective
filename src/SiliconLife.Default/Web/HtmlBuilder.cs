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
using System.Linq;

namespace SiliconLife.Default.Web;

public class HtmlBuilder
{
    private readonly string _tagName;
    private readonly Dictionary<string, string> _attributes = new();
    private readonly List<HtmlBuilder> _children = new();
    private string? _textContent;
    private readonly HtmlBuilder? _parent;
    private readonly bool _isSelfClosing;

    private HtmlBuilder(string tagName, bool isSelfClosing = false) : this(null, tagName, isSelfClosing)
    {
    }

    private HtmlBuilder(HtmlBuilder? parent, string tagName, bool isSelfClosing = false)
    {
        _parent = parent;
        _tagName = tagName;
        _isSelfClosing = isSelfClosing;
    }

    public static HtmlBuilder Create() => new(null, "root");

    public static HtmlBuilder Root() => new(null, "root");

    public HtmlBuilder DocType(string doctype = "html")
    {
        return this;
    }

    public HtmlBuilder Html()
    {
        return AddChild("html");
    }

    public HtmlBuilder Head()
    {
        return AddChild("head");
    }

    public HtmlBuilder Title(string title)
    {
        var titleBuilder = AddChild("title");
        titleBuilder._textContent = title;
        return this;
    }

    public HtmlBuilder Meta(string name, string content)
    {
        var meta = AddChild("meta", true);
        meta.AddAttr("name", name).AddAttr("content", content);
        return this;
    }

    public HtmlBuilder MetaCharset(string charset = "utf-8")
    {
        var meta = AddChild("meta", true);
        meta.AddAttr("charset", charset);
        return this;
    }

    public HtmlBuilder MetaViewport(string viewport = "width=device-width, initial-scale=1")
    {
        var meta = AddChild("meta", true);
        meta.AddAttr("name", "viewport").AddAttr("content", viewport);
        return this;
    }

    public HtmlBuilder Link(string href, string rel = "stylesheet")
    {
        var link = AddChild("link", true);
        link.AddAttr("href", href).AddAttr("rel", rel);
        return this;
    }

    public HtmlBuilder Script(string src, bool defer = false)
    {
        var script = AddChild("script");
        script.AddAttr("src", src);
        if (defer) script.AddAttr("defer", "defer");
        return this;
    }

    public HtmlBuilder ScriptInline(string script)
    {
        var scriptBuilder = AddChild("script");
        scriptBuilder._textContent = script;
        return this;
    }

    public HtmlBuilder Style(string css)
    {
        var styleBuilder = AddChild("style");
        styleBuilder._textContent = css;
        return this;
    }

    public HtmlBuilder Body()
    {
        return AddChild("body");
    }

    public HtmlBuilder Div()
    {
        AddChild("div");
        return this;
    }

    public HtmlBuilder Div(string? text)
    {
        var div = AddChild("div");
        div._textContent = text;
        return this;
    }

    public HtmlBuilder P()
    {
        return AddChild("p");
    }

    public HtmlBuilder P(string? text)
    {
        var p = AddChild("p");
        p._textContent = text;
        return this;
    }

    public HtmlBuilder H1()
    {
        return AddChild("h1");
    }

    public HtmlBuilder H1(string? text)
    {
        var h1 = AddChild("h1");
        h1._textContent = text;
        return this;
    }

    public HtmlBuilder H2(string? text)
    {
        var h2 = AddChild("h2");
        h2._textContent = text;
        return this;
    }

    public HtmlBuilder H3(string? text)
    {
        var h3 = AddChild("h3");
        h3._textContent = text;
        return this;
    }

    public HtmlBuilder H4(string? text)
    {
        var h4 = AddChild("h4");
        h4._textContent = text;
        return this;
    }

    public HtmlBuilder H5(string? text)
    {
        var h5 = AddChild("h5");
        h5._textContent = text;
        return this;
    }

    public HtmlBuilder H6(string? text)
    {
        var h6 = AddChild("h6");
        h6._textContent = text;
        return this;
    }

    public HtmlBuilder A()
    {
        return AddChild("a");
    }

    public HtmlBuilder A(string? text, string? href)
    {
        var a = AddChild("a");
        a._textContent = text;
        if (!string.IsNullOrEmpty(href))
            a.AddAttr("href", href);
        return this;
    }

    public HtmlBuilder Img(string src, string? alt = null)
    {
        var img = AddChild("img", true);
        img.AddAttr("src", src);
        if (!string.IsNullOrEmpty(alt))
            img.AddAttr("alt", alt);
        return this;
    }

    public HtmlBuilder Ul()
    {
        return AddChild("ul");
    }

    public HtmlBuilder Ol()
    {
        return AddChild("ol");
    }

    public HtmlBuilder Li(string? text)
    {
        var li = AddChild("li");
        li._textContent = text;
        return this;
    }

    public HtmlBuilder Form(string? action = null, string method = "post")
    {
        var form = AddChild("form");
        form.AddAttr("method", method);
        if (!string.IsNullOrEmpty(action))
            form.AddAttr("action", action);
        return this;
    }

    public HtmlBuilder Input(string? name = null, string type = "text", string? value = null)
    {
        var input = AddChild("input", true);
        input.AddAttr("type", type);
        if (!string.IsNullOrEmpty(name))
            input.AddAttr("name", name);
        if (!string.IsNullOrEmpty(value))
            input.AddAttr("value", value);
        return this;
    }

    public HtmlBuilder Textarea(string? name = null, string? placeholder = null)
    {
        var textarea = AddChild("textarea");
        if (!string.IsNullOrEmpty(name))
            textarea.AddAttr("name", name);
        if (!string.IsNullOrEmpty(placeholder))
            textarea.AddAttr("placeholder", placeholder);
        return this;
    }

    public HtmlBuilder Button()
    {
        return AddChild("button");
    }

    public HtmlBuilder Button(string? text, string type = "submit")
    {
        var button = AddChild("button");
        button.AddAttr("type", type);
        button._textContent = text;
        return this;
    }

    public HtmlBuilder Select(string? name = null)
    {
        var select = AddChild("select");
        if (!string.IsNullOrEmpty(name))
            select.AddAttr("name", name);
        return select;
    }

    public HtmlBuilder Option(string? text, string? value = null, bool selected = false)
    {
        var option = AddChild("option");
        option._textContent = text;
        if (!string.IsNullOrEmpty(value))
            option.AddAttr("value", value);
        if (selected)
            option.AddAttr("selected", "selected");
        return this;
    }

    public HtmlBuilder Label(string? text, string? forAttr = null)
    {
        var label = AddChild("label");
        label._textContent = text;
        if (!string.IsNullOrEmpty(forAttr))
            label.AddAttr("for", forAttr);
        return this;
    }

    public HtmlBuilder Span()
    {
        return AddChild("span");
    }

    public HtmlBuilder Span(string? text)
    {
        var span = AddChild("span");
        span._textContent = text;
        return span;
    }

    public HtmlBuilder Strong(string? text)
    {
        var strong = AddChild("strong");
        strong._textContent = text;
        return strong;
    }

    public HtmlBuilder Em(string? text)
    {
        var em = AddChild("em");
        em._textContent = text;
        return em;
    }

    public HtmlBuilder Br()
    {
        return AddChild("br", true);
    }

    public HtmlBuilder Hr()
    {
        return AddChild("hr", true);
    }

    public HtmlBuilder Table()
    {
        return AddChild("table");
    }

    public HtmlBuilder Thead()
    {
        return AddChild("thead");
    }

    public HtmlBuilder Tbody()
    {
        return AddChild("tbody");
    }

    public HtmlBuilder Tr()
    {
        return AddChild("tr");
    }

    public HtmlBuilder Th()
    {
        return AddChild("th");
    }

    public HtmlBuilder Th(string? text)
    {
        var th = AddChild("th");
        th._textContent = text;
        return th;
    }

    public HtmlBuilder Td()
    {
        return AddChild("td");
    }

    public HtmlBuilder Td(string? text)
    {
        var td = AddChild("td");
        td._textContent = text;
        return td;
    }

    public HtmlBuilder Nav()
    {
        return AddChild("nav");
    }

    public HtmlBuilder Header()
    {
        return AddChild("header");
    }

    public HtmlBuilder Footer()
    {
        return AddChild("footer");
    }

    public HtmlBuilder Main()
    {
        return AddChild("main");
    }

    public HtmlBuilder Section()
    {
        return AddChild("section");
    }

    public HtmlBuilder Article()
    {
        return AddChild("article");
    }

    public HtmlBuilder Aside()
    {
        return AddChild("aside");
    }

    public HtmlBuilder Details()
    {
        return AddChild("details");
    }

    public HtmlBuilder Summary(string? text)
    {
        var summary = AddChild("summary");
        summary._textContent = text;
        return summary;
    }

    public HtmlBuilder Script()
    {
        return AddChild("script");
    }

    public HtmlBuilder Style()
    {
        return AddChild("style");
    }

    public HtmlBuilder I(string? text)
    {
        var i = AddChild("i");
        i._textContent = text;
        return i;
    }

    public HtmlBuilder B(string? text)
    {
        var b = AddChild("b");
        b._textContent = text;
        return b;
    }

    public HtmlBuilder U(string? text)
    {
        var u = AddChild("u");
        u._textContent = text;
        return u;
    }

    public HtmlBuilder S(string? text)
    {
        var s = AddChild("s");
        s._textContent = text;
        return s;
    }

    public HtmlBuilder Code()
    {
        return AddChild("code");
    }

    public HtmlBuilder Code(string? text)
    {
        var code = AddChild("code");
        code._textContent = text;
        return code;
    }

    public HtmlBuilder Pre()
    {
        return AddChild("pre");
    }

    public HtmlBuilder Blockquote()
    {
        return AddChild("blockquote");
    }

    public HtmlBuilder Blockquote(string? text)
    {
        var blockquote = AddChild("blockquote");
        blockquote._textContent = text;
        return blockquote;
    }

    public HtmlBuilder Fieldset(string? legend = null)
    {
        var fieldset = AddChild("fieldset");
        if (!string.IsNullOrEmpty(legend))
        {
            var legendBuilder = fieldset.AddChild("legend");
            legendBuilder._textContent = legend;
        }
        return fieldset;
    }

    public HtmlBuilder Legend(string? text)
    {
        var legend = AddChild("legend");
        legend._textContent = text;
        return legend;
    }

    public HtmlBuilder InputHidden(string? name = null, string? value = null)
    {
        var input = AddChild("input", true);
        input.AddAttr("type", "hidden");
        if (!string.IsNullOrEmpty(name))
            input.AddAttr("name", name);
        if (!string.IsNullOrEmpty(value))
            input.AddAttr("value", value);
        return this;
    }

    public HtmlBuilder InputText(string? name = null, string? placeholder = null, string? value = null)
    {
        var input = AddChild("input", true);
        input.AddAttr("type", "text");
        if (!string.IsNullOrEmpty(name))
            input.AddAttr("name", name);
        if (!string.IsNullOrEmpty(placeholder))
            input.AddAttr("placeholder", placeholder);
        if (!string.IsNullOrEmpty(value))
            input.AddAttr("value", value);
        return this;
    }

    public HtmlBuilder InputPassword(string? name = null, string? placeholder = null)
    {
        var input = AddChild("input", true);
        input.AddAttr("type", "password");
        if (!string.IsNullOrEmpty(name))
            input.AddAttr("name", name);
        if (!string.IsNullOrEmpty(placeholder))
            input.AddAttr("placeholder", placeholder);
        return this;
    }

    public HtmlBuilder InputEmail(string? name = null, string? placeholder = null, string? value = null)
    {
        var input = AddChild("input", true);
        input.AddAttr("type", "email");
        if (!string.IsNullOrEmpty(name))
            input.AddAttr("name", name);
        if (!string.IsNullOrEmpty(placeholder))
            input.AddAttr("placeholder", placeholder);
        if (!string.IsNullOrEmpty(value))
            input.AddAttr("value", value);
        return this;
    }

    public HtmlBuilder InputNumber(string? name = null, string? placeholder = null, string? value = null)
    {
        var input = AddChild("input", true);
        input.AddAttr("type", "number");
        if (!string.IsNullOrEmpty(name))
            input.AddAttr("name", name);
        if (!string.IsNullOrEmpty(placeholder))
            input.AddAttr("placeholder", placeholder);
        if (!string.IsNullOrEmpty(value))
            input.AddAttr("value", value);
        return this;
    }

    public HtmlBuilder InputCheckbox(string? name = null, string? value = null, bool @checked = false, string? label = null)
    {
        var input = AddChild("input", true);
        input.AddAttr("type", "checkbox");
        if (!string.IsNullOrEmpty(name))
            input.AddAttr("name", name);
        if (!string.IsNullOrEmpty(value))
            input.AddAttr("value", value);
        if (@checked)
            input.AddAttr("checked", "checked");
        if (!string.IsNullOrEmpty(label))
        {
            var labelBuilder = AddChild("label");
            labelBuilder._textContent = label;
        }
        return this;
    }

    public HtmlBuilder InputRadio(string? name = null, string? value = null, bool @checked = false, string? label = null)
    {
        var input = AddChild("input", true);
        input.AddAttr("type", "radio");
        if (!string.IsNullOrEmpty(name))
            input.AddAttr("name", name);
        if (!string.IsNullOrEmpty(value))
            input.AddAttr("value", value);
        if (@checked)
            input.AddAttr("checked", "checked");
        if (!string.IsNullOrEmpty(label))
        {
            var labelBuilder = AddChild("label");
            labelBuilder._textContent = label;
        }
        return this;
    }

    public HtmlBuilder InputSubmit(string? value = null, string? name = null)
    {
        var input = AddChild("input", true);
        input.AddAttr("type", "submit");
        if (!string.IsNullOrEmpty(value))
            input.AddAttr("value", value);
        if (!string.IsNullOrEmpty(name))
            input.AddAttr("name", name);
        return this;
    }

    public HtmlBuilder InputReset(string? value = null)
    {
        var input = AddChild("input", true);
        input.AddAttr("type", "reset");
        if (!string.IsNullOrEmpty(value))
            input.AddAttr("value", value);
        return this;
    }

    public HtmlBuilder InputFile(string? name = null)
    {
        var input = AddChild("input", true);
        input.AddAttr("type", "file");
        if (!string.IsNullOrEmpty(name))
            input.AddAttr("name", name);
        return this;
    }

    public HtmlBuilder InputColor(string? name = null, string? value = null)
    {
        var input = AddChild("input", true);
        input.AddAttr("type", "color");
        if (!string.IsNullOrEmpty(name))
            input.AddAttr("name", name);
        if (!string.IsNullOrEmpty(value))
            input.AddAttr("value", value);
        return this;
    }

    public HtmlBuilder InputDate(string? name = null, string? value = null)
    {
        var input = AddChild("input", true);
        input.AddAttr("type", "date");
        if (!string.IsNullOrEmpty(name))
            input.AddAttr("name", name);
        if (!string.IsNullOrEmpty(value))
            input.AddAttr("value", value);
        return this;
    }

    public HtmlBuilder InputTime(string? name = null, string? value = null)
    {
        var input = AddChild("input", true);
        input.AddAttr("type", "time");
        if (!string.IsNullOrEmpty(name))
            input.AddAttr("name", name);
        if (!string.IsNullOrEmpty(value))
            input.AddAttr("value", value);
        return this;
    }

    public HtmlBuilder InputSearch(string? name = null, string? placeholder = null)
    {
        var input = AddChild("input", true);
        input.AddAttr("type", "search");
        if (!string.IsNullOrEmpty(name))
            input.AddAttr("name", name);
        if (!string.IsNullOrEmpty(placeholder))
            input.AddAttr("placeholder", placeholder);
        return this;
    }

    public HtmlBuilder Attr(string name, object? value)
    {
        if (value != null)
        {
            _attributes[name] = value.ToString() ?? "";
        }
        return this;
    }

    public HtmlBuilder Class(string className)
    {
        if (_attributes.TryGetValue("class", out var existing))
        {
            _attributes["class"] = existing + " " + className;
        }
        else
        {
            _attributes["class"] = className;
        }
        return this;
    }

    public HtmlBuilder Id(string id)
    {
        return Attr("id", id);
    }

    public HtmlBuilder Data(string key, object value)
    {
        return Attr($"data-{key}", value);
    }

    public HtmlBuilder Href(string href)
    {
        return Attr("href", href);
    }

    public HtmlBuilder OnClick(string handler)
    {
        return Attr("onclick", handler);
    }

    public HtmlBuilder OnSubmit(string handler)
    {
        return Attr("onsubmit", handler);
    }

    public HtmlBuilder OnChange(string handler)
    {
        return Attr("onchange", handler);
    }

    public HtmlBuilder Placeholder(string placeholder)
    {
        return Attr("placeholder", placeholder);
    }

    public HtmlBuilder Text(string? text)
    {
        _textContent = text;
        return this;
    }

    public HtmlBuilder Raw(string? html)
    {
        var raw = new RawHtml(this, html);
        _children.Add(raw);
        return this;
    }

    public HtmlBuilder Comment(string comment)
    {
        var commentNode = new CommentNode(this, comment);
        _children.Add(commentNode);
        return this;
    }

    public HtmlBuilder EndBlock()
    {
        return _parent ?? this;
    }

    public string Build()
    {
        var sb = new StringBuilder();
        BuildTo(sb);
        return sb.ToString();
    }

    public override string ToString() => Build();

    private HtmlBuilder AddChild(string tagName, bool isSelfClosing = false)
    {
        var child = new HtmlBuilder(this, tagName, isSelfClosing);
        _children.Add(child);
        return this;
    }

    public HtmlBuilder Child()
    {
        return _children.LastOrDefault() ?? this;
    }

    public HtmlBuilder AddAttr(string name, string value)
    {
        _attributes[name] = value;
        return this;
    }

    private void BuildTo(StringBuilder sb)
    {
        if (_tagName == "root")
        {
            foreach (var child in _children)
            {
                child.BuildTo(sb);
            }
            return;
        }

        if (this is RawHtml rawHtml)
        {
            sb.Append(rawHtml._html);
            return;
        }

        if (this is CommentNode comment)
        {
            sb.Append($"<!-- {comment._text} -->");
            return;
        }

        sb.Append('<');
        sb.Append(_tagName);

        foreach (var attr in _attributes)
        {
            sb.Append(' ');
            sb.Append(attr.Key);
            sb.Append("=\"");
            sb.Append(EscapeHtml(attr.Value));
            sb.Append('"');
        }

        if (_isSelfClosing)
        {
            sb.Append(" />");
            return;
        }

        sb.Append('>');

        if (!string.IsNullOrEmpty(_textContent))
        {
            sb.Append(EscapeHtml(_textContent));
        }

        foreach (var child in _children)
        {
            child.BuildTo(sb);
        }

        sb.Append("</");
        sb.Append(_tagName);
        sb.Append('>');
    }

    private static string EscapeHtml(string text)
    {
        return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#39;");
    }

    private class RawHtml : HtmlBuilder
    {
        public readonly string _html;
        public RawHtml(HtmlBuilder parent, string html) : base(parent, "raw")
        {
            _html = html;
        }
    }

    private class CommentNode : HtmlBuilder
    {
        public readonly string _text;
        public CommentNode(HtmlBuilder parent, string text) : base(parent, "comment")
        {
            _text = text;
        }
    }
}

public static class HtmlExtensions
{
    public static HtmlBuilder If(this HtmlBuilder builder, bool condition, Action<HtmlBuilder> action)
    {
        if (condition)
        {
            action(builder);
        }
        return builder;
    }
}
