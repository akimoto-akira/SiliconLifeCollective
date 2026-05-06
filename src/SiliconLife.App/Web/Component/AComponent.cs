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

namespace SiliconLife.App.Web.Component;

/// <summary>
/// Anchor link component
/// </summary>
public class AComponent : ComponentBase
{
    private string _text = "";
    private string _href = "";

    public AComponent() { }

    public AComponent(string text)
    {
        _text = text;
    }

    /// <summary>
    /// Set ID (returns AComponent for chaining)
    /// </summary>
    public new AComponent Id(string id)
    {
        base.Id = id;
        return this;
    }

    /// <summary>
    /// Set Class (returns AComponent for chaining)
    /// </summary>
    public new AComponent Class(string className)
    {
        base.Class = string.IsNullOrEmpty(base.Class) ? className : $"{base.Class} {className}";
        return this;
    }

    /// <summary>
    /// Set Style (returns AComponent for chaining)
    /// </summary>
    public new AComponent Style(string style)
    {
        base.Style = string.IsNullOrEmpty(base.Style) ? style : $"{base.Style};{style}";
        return this;
    }

    /// <summary>
    /// Set Attribute (returns AComponent for chaining)
    /// </summary>
    public new AComponent Attr(string name, string value)
    {
        base.Attributes[name] = value;
        return this;
    }

    /// <summary>
    /// Set link text
    /// </summary>
    public AComponent Text(string text)
    {
        _text = text;
        return this;
    }

    /// <summary>
    /// Set href URL
    /// </summary>
    public AComponent Href(string href)
    {
        _href = href;
        return this;
    }

    public override string Render()
    {
        var a = H.A();

        if (!string.IsNullOrEmpty(_href))
            a.Attr("href", _href);

        if (!string.IsNullOrEmpty(base.Id))
            a.Attr("id", base.Id);

        if (!string.IsNullOrEmpty(base.Class))
            a.Attr("class", base.Class);

        if (!string.IsNullOrEmpty(base.Style))
            a.Attr("style", base.Style);

        foreach (var kvp in Attributes)
        {
            a.Attr(kvp.Key, kvp.Value);
        }

        if (!string.IsNullOrEmpty(_text))
        {
            a.Add(H.Span(H.Escape(_text)));
        }

        return a.Build();
    }
}
