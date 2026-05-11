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
/// Label component
/// </summary>
public class LabelComponent : ComponentBase
{
    private string _text = "";
    private string? _for;

    /// <summary>
    /// Set ID (returns LabelComponent for chaining)
    /// </summary>
    public new LabelComponent Id(string id)
    {
        base.Id = id;
        return this;
    }

    /// <summary>
    /// Set Class (returns LabelComponent for chaining)
    /// </summary>
    public new LabelComponent Class(string className)
    {
        base.Class = string.IsNullOrEmpty(base.Class) ? className : $"{base.Class} {className}";
        return this;
    }

    /// <summary>
    /// Set Style (returns LabelComponent for chaining)
    /// </summary>
    public new LabelComponent Style(CssBuilder style)
    {
        if (base.Style == null) base.Style = style; else base.Style.MergeInlineFrom(style);
        return this;
    }

    /// <summary>
    /// Set Attribute (returns LabelComponent for chaining)
    /// </summary>
    public new LabelComponent Attr(string name, string value)
    {
        base.Attributes[name] = value;
        return this;
    }

    /// <summary>
    /// Set label text
    /// </summary>
    public LabelComponent Text(string text)
    {
        _text = text;
        return this;
    }

    /// <summary>
    /// Set associated form element ID
    /// </summary>
    public LabelComponent For(string forId)
    {
        _for = forId;
        return this;
    }

    public override H Render()
    {
        var label = H.Label()
            .Text(H.Escape(_text));

        if (!string.IsNullOrEmpty(_for))
            label.Attr("for", _for);

        if (!string.IsNullOrEmpty(base.Id))
            label.Attr("id", base.Id);

        if (!string.IsNullOrEmpty(base.Class))
            label.Attr("class", base.Class);

        if (base.Style != null && base.Style.HasInlineStyles)
            label.Attr("style", base.Style.BuildInline());

        foreach (var kvp in Attributes)
        {
            label.Attr(kvp.Key, kvp.Value);
        }

        return label;
    }
}
