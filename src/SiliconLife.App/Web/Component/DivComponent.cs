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
/// Div container component
/// </summary>
public class DivComponent : ComponentBase
{
    private readonly List<ComponentBase> _children = new();
    private string _innerHTML = "";

    /// <summary>
    /// Set ID (returns DivComponent for chaining)
    /// </summary>
    public new DivComponent Id(string id)
    {
        base.Id = id;
        return this;
    }

    /// <summary>
    /// Set Class (returns DivComponent for chaining)
    /// </summary>
    public new DivComponent Class(string className)
    {
        base.Class = string.IsNullOrEmpty(base.Class) ? className : $"{base.Class} {className}";
        return this;
    }

    /// <summary>
    /// Set Style (returns DivComponent for chaining)
    /// </summary>
    public new DivComponent Style(string style)
    {
        base.Style = string.IsNullOrEmpty(base.Style) ? style : $"{base.Style};{style}";
        return this;
    }

    /// <summary>
    /// Set Attribute (returns DivComponent for chaining)
    /// </summary>
    public new DivComponent Attr(string name, string value)
    {
        base.Attributes[name] = value;
        return this;
    }

    /// <summary>
    /// Add child component
    /// </summary>
    public DivComponent Add(ComponentBase child)
    {
        if (child != null)
            _children.Add(child);
        return this;
    }

    /// <summary>
    /// Add multiple child components
    /// </summary>
    public DivComponent AddRange(IEnumerable<ComponentBase> children)
    {
        _children.AddRange(children);
        return this;
    }

    /// <summary>
    /// Set inner HTML (not recommended, prefer Add)
    /// </summary>
    public DivComponent InnerHtml(string html)
    {
        _innerHTML = html;
        return this;
    }

    public override string Render()
    {
        var div = H.Div();

        if (!string.IsNullOrEmpty(base.Id))
            div.Attr("id", base.Id);

        if (!string.IsNullOrEmpty(base.Class))
            div.Attr("class", base.Class);

        if (!string.IsNullOrEmpty(base.Style))
            div.Attr("style", base.Style);

        foreach (var kvp in Attributes)
        {
            div.Attr(kvp.Key, kvp.Value);
        }

        if (!string.IsNullOrEmpty(_innerHTML))
        {
            div.AddRendered(_innerHTML);
        }
        else
        {
            foreach (var child in _children)
            {
                div.AddRendered(child.Render());
            }
        }

        return div.Build();
    }
}
