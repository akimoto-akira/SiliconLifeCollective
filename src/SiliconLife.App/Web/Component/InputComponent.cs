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
/// Input component
/// </summary>
public class InputComponent : ComponentBase
{
    private string _type = "text";
    private string _name = "";
    private string _placeholder = "";
    private string _value = "";
    private bool _required = false;
    private bool _readonly = false;
    private string? _onchange;
    private string? _oninput;

    /// <summary>
    /// Set ID (returns InputComponent for chaining)
    /// </summary>
    public new InputComponent Id(string id)
    {
        base.Id = id;
        return this;
    }

    /// <summary>
    /// Set Class (returns InputComponent for chaining)
    /// </summary>
    public new InputComponent Class(string className)
    {
        base.Class = string.IsNullOrEmpty(base.Class) ? className : $"{base.Class} {className}";
        return this;
    }

    /// <summary>
    /// Set Style (returns InputComponent for chaining)
    /// </summary>
    public new InputComponent Style(CssBuilder style)
    {
        if (base.Style == null) base.Style = style; else base.Style.MergeInlineFrom(style);
        return this;
    }

    /// <summary>
    /// Set Attribute (returns InputComponent for chaining)
    /// </summary>
    public new InputComponent Attr(string name, string value)
    {
        base.Attributes[name] = value;
        return this;
    }

    /// <summary>
    /// Set input name
    /// </summary>
    public InputComponent Name(string name)
    {
        _name = name;
        return this;
    }

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

    public override H Render()
    {
        var input = H.Input()
            .Attr("type", _type);

        if (!string.IsNullOrEmpty(_name))
            input.Attr("name", _name);

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

        if (!string.IsNullOrEmpty(base.Id))
            input.Attr("id", base.Id);

        if (!string.IsNullOrEmpty(base.Class))
            input.Attr("class", base.Class);

        if (base.Style != null && base.Style.HasInlineStyles)
            input.Attr("style", base.Style.BuildInline());

        foreach (var kvp in Attributes)
        {
            input.Attr(kvp.Key, kvp.Value);
        }

        return input;
    }
}
