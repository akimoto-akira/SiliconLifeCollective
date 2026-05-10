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
/// Native details/summary component.
/// The Summary content is ALWAYS visible (collapsed or expanded),
/// while children added via Add() are only visible when expanded.
/// </summary>
public class DetailsComponent : ComponentBase
{
    private readonly List<ComponentBase> _summaryChildren = new();
    private readonly List<ComponentBase> _children = new();
    private bool _open;

    /// <summary>
    /// Set ID (returns DetailsComponent for chaining)
    /// </summary>
    public new DetailsComponent Id(string id)
    {
        base.Id = id;
        return this;
    }

    /// <summary>
    /// Set Class (returns DetailsComponent for chaining)
    /// </summary>
    public new DetailsComponent Class(string className)
    {
        base.Class = string.IsNullOrEmpty(base.Class) ? className : $"{base.Class} {className}";
        return this;
    }

    /// <summary>
    /// Set Style (returns DetailsComponent for chaining)
    /// </summary>
    public new DetailsComponent Style(CssBuilder style)
    {
        if (base.Style == null) base.Style = style; else base.Style.MergeInlineFrom(style);
        return this;
    }

    /// <summary>
    /// Set Attribute (returns DetailsComponent for chaining)
    /// </summary>
    public new DetailsComponent Attr(string name, string value)
    {
        base.Attributes[name] = value;
        return this;
    }

    /// <summary>
    /// Open by default (adds the 'open' attribute)
    /// </summary>
    public DetailsComponent Open(bool open = true)
    {
        _open = open;
        return this;
    }

    /// <summary>
    /// Add content into the &lt;summary&gt; tag. Always visible.
    /// </summary>
    public DetailsComponent AddSummary(ComponentBase child)
    {
        if (child != null)
            _summaryChildren.Add(child);
        return this;
    }

    /// <summary>
    /// Add child content rendered inside &lt;details&gt; but outside &lt;summary&gt;.
    /// Only visible when the details element is expanded.
    /// </summary>
    public DetailsComponent Add(ComponentBase child)
    {
        if (child != null)
            _children.Add(child);
        return this;
    }

    public override string Render()
    {
        var details = H.Details();

        if (_open)
            details.Attr("open", "open");

        if (!string.IsNullOrEmpty(base.Id))
            details.Attr("id", base.Id);

        if (!string.IsNullOrEmpty(base.Class))
            details.Attr("class", base.Class);

        if (base.Style != null && base.Style.HasInlineStyles)
            details.Attr("style", base.Style.BuildInline());

        foreach (var kvp in Attributes)
        {
            details.Attr(kvp.Key, kvp.Value);
        }

        // <summary> block - always visible
        var summary = H.Summary();
        foreach (var child in _summaryChildren)
        {
            summary.AddRendered(child.Render());
        }
        details.Add(summary);

        // Collapsible children - visible only when expanded
        foreach (var child in _children)
        {
            details.AddRendered(child.Render());
        }

        return details.Build();
    }
}
