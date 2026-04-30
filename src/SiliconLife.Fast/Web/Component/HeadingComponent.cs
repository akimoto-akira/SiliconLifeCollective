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
/// Heading component (h1-h6)
/// </summary>
public class HeadingComponent : ComponentBase
{
    private readonly string _tag;
    private string _text = "";

    public HeadingComponent(string tag)
    {
        _tag = tag;
    }

    /// <summary>
    /// Set heading text
    /// </summary>
    public HeadingComponent Text(string text)
    {
        _text = text;
        return this;
    }

    public override string Render()
    {
        var heading = H.Create(_tag);

        if (!string.IsNullOrEmpty(Id))
            heading.Id(Id);

        if (!string.IsNullOrEmpty(Class))
            heading.Class(Class);

        if (!string.IsNullOrEmpty(Style))
            heading.Style(Style);

        foreach (var kvp in Attributes)
        {
            heading.Attr(kvp.Key, kvp.Value);
        }

        if (!string.IsNullOrEmpty(_text))
        {
            heading.Add(new RawHtml(H.Escape(_text)));
        }

        return heading.Build();
    }
}
