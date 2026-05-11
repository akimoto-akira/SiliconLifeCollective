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
/// Base class for all UI components
/// </summary>
public abstract class ComponentBase
{
    /// <summary>
    /// Component ID
    /// </summary>
    public string? Id { get; protected set; }

    /// <summary>
    /// CSS class names
    /// </summary>
    public string? Class { get; protected set; }

    /// <summary>
    /// Inline styles
    /// </summary>
    public CssBuilder? Style { get; protected set; }

    /// <summary>
    /// Custom attributes
    /// </summary>
    public Dictionary<string, string> Attributes { get; } = new();

    /// <summary>
    /// Set component ID (chainable)
    /// </summary>
    public T SetId<T>(string id) where T : ComponentBase
    {
        Id = id;
        return (T)this;
    }

    /// <summary>
    /// Set component ID (legacy)
    /// </summary>
    public T WithId<T>(string id) where T : ComponentBase
    {
        return SetId<T>(id);
    }

    /// <summary>
    /// Set CSS class (chainable)
    /// </summary>
    public T SetClass<T>(string className) where T : ComponentBase
    {
        Class = string.IsNullOrEmpty(Class) ? className : $"{Class} {className}";
        return (T)this;
    }

    /// <summary>
    /// Set CSS class (legacy)
    /// </summary>
    public T WithClass<T>(string className) where T : ComponentBase
    {
        return SetClass<T>(className);
    }

    /// <summary>
    /// Set inline style (chainable)
    /// </summary>
    public T SetStyle<T>(CssBuilder style) where T : ComponentBase
    {
        if (Style == null)
            Style = style;
        else
            Style.MergeInlineFrom(style);
        return (T)this;
    }

    /// <summary>
    /// Set inline style (legacy)
    /// </summary>
    public T WithStyle<T>(CssBuilder style) where T : ComponentBase
    {
        return SetStyle<T>(style);
    }

    /// <summary>
    /// Add custom attribute (chainable)
    /// </summary>
    public T SetAttr<T>(string name, string value) where T : ComponentBase
    {
        Attributes[name] = value;
        return (T)this;
    }

    /// <summary>
    /// Add custom attribute (legacy)
    /// </summary>
    public T WithAttribute<T>(string name, string value) where T : ComponentBase
    {
        return SetAttr<T>(name, value);
    }

    public abstract H Render();

    /// <summary>
    /// Generate HTML attributes string
    /// </summary>
    protected string RenderAttributes()
    {
        var attrs = new List<string>();

        if (!string.IsNullOrEmpty(Id))
            attrs.Add($"id=\"{Id}\"");

        if (!string.IsNullOrEmpty(Class))
            attrs.Add($"class=\"{Class}\"");

        if (Style != null && Style.HasInlineStyles)
            attrs.Add($"style=\"{Style.BuildInline()}\"");

        foreach (var kvp in Attributes)
        {
            attrs.Add($"{kvp.Key}=\"{kvp.Value}\"");
        }

        return attrs.Count > 0 ? " " + string.Join(" ", attrs) : "";
    }
}
