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
        _children.Add(child);
        return this;
    }

    public override string Render()
    {
        var span = H.Span();

        if (!string.IsNullOrEmpty(Id))
            span.Id(Id);

        if (!string.IsNullOrEmpty(Class))
            span.Class(Class);

        if (!string.IsNullOrEmpty(Style))
            span.Style(Style);

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
                span.Add(new RawHtml(child.Render()));
            }
        }

        return span.Build();
    }
}
