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

        if (!string.IsNullOrEmpty(Id))
            p.Id(Id);

        if (!string.IsNullOrEmpty(Class))
            p.Class(Class);

        if (!string.IsNullOrEmpty(Style))
            p.Style(Style);

        foreach (var kvp in Attributes)
        {
            p.Attr(kvp.Key, kvp.Value);
        }

        if (!string.IsNullOrEmpty(_text))
        {
            p.Add(new RawHtml(H.Escape(_text)));
        }

        return p.Build();
    }
}
