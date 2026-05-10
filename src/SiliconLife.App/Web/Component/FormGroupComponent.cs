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
/// Form group component - wraps label and input with form-group class
/// </summary>
public class FormGroupComponent : ComponentBase
{
    private readonly List<ComponentBase> _children = new();

    /// <summary>
    /// Set ID (returns FormGroupComponent for chaining)
    /// </summary>
    public new FormGroupComponent Id(string id)
    {
        base.Id = id;
        return this;
    }

    /// <summary>
    /// Set Class (returns FormGroupComponent for chaining)
    /// </summary>
    public new FormGroupComponent Class(string className)
    {
        base.Class = string.IsNullOrEmpty(base.Class) ? className : $"{base.Class} {className}";
        return this;
    }

    /// <summary>
    /// Set Style (returns FormGroupComponent for chaining)
    /// </summary>
    public new FormGroupComponent Style(CssBuilder style)
    {
        if (base.Style == null) base.Style = style; else base.Style.MergeInlineFrom(style);
        return this;
    }

    /// <summary>
    /// Set Attribute (returns FormGroupComponent for chaining)
    /// </summary>
    public new FormGroupComponent Attr(string name, string value)
    {
        base.Attributes[name] = value;
        return this;
    }

    /// <summary>
    /// Add child component
    /// </summary>
    public FormGroupComponent Add(ComponentBase child)
    {
        if (child != null)
            _children.Add(child);
        return this;
    }

    public override string Render()
    {
        var div = H.Div();

        if (!string.IsNullOrEmpty(base.Id))
            div.Attr("id", base.Id);

        var classes = new List<string> { "form-group" };
        if (!string.IsNullOrEmpty(base.Class))
            classes.Add(base.Class);
        div.Class(string.Join(" ", classes));

        if (base.Style != null && base.Style.HasInlineStyles)
            div.Attr("style", base.Style.BuildInline());

        foreach (var kvp in Attributes)
        {
            div.Attr(kvp.Key, kvp.Value);
        }

        foreach (var child in _children)
        {
            div.AddRendered(child.Render());
        }

        return div.Build();
    }
}
