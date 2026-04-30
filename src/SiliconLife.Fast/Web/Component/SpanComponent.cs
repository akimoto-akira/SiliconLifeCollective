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

namespace SiliconLife.Fast.Web.Component;

/// <summary>
/// Span inline container component
/// </summary>
public class SpanComponent : ComponentBase
{
    private readonly List<ComponentBase> _children = new();
    private string _text = "";

    public SpanComponent() { }

    public SpanComponent(string text)
    {
        _text = text;
    }

    /// <summary>
    /// Set ID (returns SpanComponent for chaining)
    /// </summary>
    public new SpanComponent Id(string id)
    {
        base.Id = id;
        return this;
    }

    /// <summary>
    /// Set Class (returns SpanComponent for chaining)
    /// </summary>
    public new SpanComponent Class(string className)
    {
        base.Class = string.IsNullOrEmpty(base.Class) ? className : $"{base.Class} {className}";
        return this;
    }

    /// <summary>
    /// Set Style (returns SpanComponent for chaining)
    /// </summary>
    public new SpanComponent Style(string style)
    {
        base.Style = string.IsNullOrEmpty(base.Style) ? style : $"{base.Style};{style}";
        return this;
    }

    /// <summary>
    /// Set Attribute (returns SpanComponent for chaining)
    /// </summary>
    public new SpanComponent Attr(string name, string value)
    {
        base.Attributes[name] = value;
        return this;
    }

    /// <summary>
    /// Set text content
    /// </summary>
    public SpanComponent Text(string text)
    {
        _text = text;
        return this;
    }

    /// <summary>
    /// Add child component
    /// </summary>
    public SpanComponent Add(ComponentBase child)
    {
        if (child != null)
            _children.Add(child);
        return this;
    }

    public override string Render()
    {
        var span = H.Span();

        if (!string.IsNullOrEmpty(base.Id))
            span.Attr("id", base.Id);

        if (!string.IsNullOrEmpty(base.Class))
            span.Attr("class", base.Class);

        if (!string.IsNullOrEmpty(base.Style))
            span.Attr("style", base.Style);

        foreach (var kvp in Attributes)
        {
            span.Attr(kvp.Key, kvp.Value);
        }

        if (!string.IsNullOrEmpty(_text))
        {
            span.Text(H.Escape(_text));
        }
        else
        {
            foreach (var child in _children)
            {
                span.AddRendered(child.Render());
            }
        }

        return span.Build();
    }
}
