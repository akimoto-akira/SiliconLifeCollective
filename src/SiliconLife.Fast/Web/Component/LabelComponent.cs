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
/// Label component
/// </summary>
public class LabelComponent : ComponentBase
{
    private string _text = "";
    private string? _for;

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

    public override string Render()
    {
        var label = H.Label()
            .Text(H.Escape(_text));

        if (!string.IsNullOrEmpty(_for))
            label.Attr("for", _for);

        if (!string.IsNullOrEmpty(Id))
            label.Id(Id);

        if (!string.IsNullOrEmpty(Class))
            label.Class(Class);

        if (!string.IsNullOrEmpty(Style))
            label.Style(Style);

        foreach (var kvp in Attributes)
        {
            label.Attr(kvp.Key, kvp.Value);
        }

        return label.Build();
    }
}
