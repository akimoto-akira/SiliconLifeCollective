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
/// Button component
/// </summary>
public class ButtonComponent : ComponentBase
{
    private string _text = "";
    private string _type = "button";
    private bool _disabled = false;
    private string? _onclick;

    /// <summary>
    /// Set button text
    /// </summary>
    public ButtonComponent Text(string text)
    {
        _text = text;
        return this;
    }

    /// <summary>
    /// Set button type (button/submit/reset)
    /// </summary>
    public ButtonComponent Type(string type)
    {
        _type = type;
        return this;
    }

    /// <summary>
    /// Set disabled state
    /// </summary>
    public ButtonComponent Disabled(bool disabled = true)
    {
        _disabled = disabled;
        return this;
    }

    /// <summary>
    /// Set click behavior
    /// </summary>
    public ButtonComponent OnClick(Behavior behavior)
    {
        _onclick = behavior.Build();
        return this;
    }

    /// <summary>
    /// Set click event (simple JS)
    /// </summary>
    public ButtonComponent OnClickRaw(string js)
    {
        _onclick = js;
        return this;
    }

    public override string Render()
    {
        var button = H.Button()
            .Attr("type", _type)
            .Text(H.Escape(_text));

        if (!string.IsNullOrEmpty(Id))
            button.Id(Id);

        if (!string.IsNullOrEmpty(Class))
            button.Class(Class);

        if (!string.IsNullOrEmpty(Style))
            button.Style(Style);

        foreach (var kvp in Attributes)
        {
            button.Attr(kvp.Key, kvp.Value);
        }

        if (_disabled)
            button.Disabled();

        if (!string.IsNullOrEmpty(_onclick))
            button.OnClick(_onclick);

        return button.Build();
    }
}
