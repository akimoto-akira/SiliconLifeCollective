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
/// Paragraph component
/// </summary>
public class PComponent : ComponentBase
{
    private string _text = "";

    public PComponent() { }

    public PComponent(string text)
    {
        _text = text;
    }

    /// <summary>
    /// Set ID (returns PComponent for chaining)
    /// </summary>
    public new PComponent Id(string id)
    {
        base.Id = id;
        return this;
    }

    /// <summary>
    /// Set Class (returns PComponent for chaining)
    /// </summary>
    public new PComponent Class(string className)
    {
        base.Class = string.IsNullOrEmpty(base.Class) ? className : $"{base.Class} {className}";
        return this;
    }

    /// <summary>
    /// Set Style (returns PComponent for chaining)
    /// </summary>
    public new PComponent Style(CssBuilder style)
    {
        if (base.Style == null) base.Style = style; else base.Style.MergeInlineFrom(style);
        return this;
    }

    /// <summary>
    /// Set Attribute (returns PComponent for chaining)
    /// </summary>
    public new PComponent Attr(string name, string value)
    {
        base.Attributes[name] = value;
        return this;
    }

    /// <summary>
    /// Set paragraph text
    /// </summary>
    public PComponent Text(string text)
    {
        _text = text;
        return this;
    }

    public override string Render()
    {
        var p = H.P();

        if (!string.IsNullOrEmpty(base.Id))
            p.Attr("id", base.Id);

        if (!string.IsNullOrEmpty(base.Class))
            p.Attr("class", base.Class);

        if (base.Style != null && base.Style.HasInlineStyles)
            p.Attr("style", base.Style.BuildInline());

        foreach (var kvp in Attributes)
        {
            p.Attr(kvp.Key, kvp.Value);
        }

        if (!string.IsNullOrEmpty(_text))
        {
            p.Add(H.Span(H.Escape(_text)));
        }

        return p.Build();
    }
}
