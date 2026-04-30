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
/// Form group component - wraps label and input with form-group class
/// </summary>
public class FormGroupComponent : ComponentBase
{
    private readonly List<ComponentBase> _children = new();

    /// <summary>
    /// Add child component
    /// </summary>
    public FormGroupComponent Add(ComponentBase child)
    {
        _children.Add(child);
        return this;
    }

    public override string Render()
    {
        var div = H.Div();

        if (!string.IsNullOrEmpty(Id))
            div.Id(Id);

        var classes = new List<string> { "form-group" };
        if (!string.IsNullOrEmpty(Class))
            classes.Add(Class);
        div.Class(string.Join(" ", classes));

        if (!string.IsNullOrEmpty(Style))
            div.Style(Style);

        foreach (var kvp in Attributes)
        {
            div.Attr(kvp.Key, kvp.Value);
        }

        foreach (var child in _children)
        {
            div.Add(new RawHtml(child.Render()));
        }

        return div.Build();
    }
}
