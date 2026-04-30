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

        if (!string.IsNullOrEmpty(Id))
            a.Id(Id);

        if (!string.IsNullOrEmpty(Class))
            a.Class(Class);

        if (!string.IsNullOrEmpty(Style))
            a.Style(Style);

        foreach (var kvp in Attributes)
        {
            a.Attr(kvp.Key, kvp.Value);
        }

        if (!string.IsNullOrEmpty(_text))
        {
            a.Add(new RawHtml(H.Escape(_text)));
        }

        return a.Build();
    }
}
