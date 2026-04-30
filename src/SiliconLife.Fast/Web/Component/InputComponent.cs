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
/// Input component
/// </summary>
public class InputComponent : ComponentBase
{
    private string _type = "text";
    private string _placeholder = "";
    private string _value = "";
    private bool _required = false;
    private bool _readonly = false;
    private string? _onchange;
    private string? _oninput;

    /// <summary>
    /// Set input type
    /// </summary>
    public InputComponent Type(string type)
    {
        _type = type;
        return this;
    }

    /// <summary>
    /// Set placeholder
    /// </summary>
    public InputComponent Placeholder(string placeholder)
    {
        _placeholder = placeholder;
        return this;
    }

    /// <summary>
    /// Set value
    /// </summary>
    public InputComponent Value(string value)
    {
        _value = value;
        return this;
    }

    /// <summary>
    /// Set required
    /// </summary>
    public InputComponent Required(bool required = true)
    {
        _required = required;
        return this;
    }

    /// <summary>
    /// Set readonly
    /// </summary>
    public InputComponent Readonly(bool readonly_ = true)
    {
        _readonly = readonly_;
        return this;
    }

    /// <summary>
    /// Set change event
    /// </summary>
    public InputComponent OnChange(Behavior behavior)
    {
        _onchange = behavior.Build();
        return this;
    }

    /// <summary>
    /// Set input event
    /// </summary>
    public InputComponent OnInput(Behavior behavior)
    {
        _oninput = behavior.Build();
        return this;
    }

    public override string Render()
    {
        var input = H.Input()
            .Attr("type", _type);

        if (!string.IsNullOrEmpty(_placeholder))
            input.Placeholder(_placeholder);

        if (!string.IsNullOrEmpty(_value))
            input.Value(_value);

        if (_required)
            input.Attr("required", "required");

        if (_readonly)
            input.Attr("readonly", "readonly");

        if (!string.IsNullOrEmpty(_onchange))
            input.OnChange(_onchange);

        if (!string.IsNullOrEmpty(_oninput))
            input.Attr("oninput", _oninput);

        if (!string.IsNullOrEmpty(Id))
            input.Id(Id);

        if (!string.IsNullOrEmpty(Class))
            input.Class(Class);

        if (!string.IsNullOrEmpty(Style))
            input.Style(Style);

        foreach (var kvp in Attributes)
        {
            input.Attr(kvp.Key, kvp.Value);
        }

        return input.Build();
    }
}
