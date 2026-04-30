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
/// Textarea multi-line input component
/// </summary>
public class TextareaComponent : ComponentBase
{
    private string _placeholder = "";
    private string _value = "";
    private int _rows = 4;
    private bool _required = false;
    private bool _readonly = false;

    /// <summary>
    /// Set placeholder
    /// </summary>
    public TextareaComponent Placeholder(string placeholder)
    {
        _placeholder = placeholder;
        return this;
    }

    /// <summary>
    /// Set value
    /// </summary>
    public TextareaComponent Value(string value)
    {
        _value = value;
        return this;
    }

    /// <summary>
    /// Set number of rows
    /// </summary>
    public TextareaComponent Rows(int rows)
    {
        _rows = rows;
        return this;
    }

    /// <summary>
    /// Set required
    /// </summary>
    public TextareaComponent Required(bool required = true)
    {
        _required = required;
        return this;
    }

    /// <summary>
    /// Set readonly
    /// </summary>
    public TextareaComponent Readonly(bool readonly_ = true)
    {
        _readonly = readonly_;
        return this;
    }

    public override string Render()
    {
        var textarea = H.Textarea()
            .Text(H.Escape(_value));

        if (!string.IsNullOrEmpty(_placeholder))
            textarea.Attr("placeholder", H.Escape(_placeholder));

        textarea.Attr("rows", _rows.ToString());

        if (_required)
            textarea.Attr("required", "required");

        if (_readonly)
            textarea.Attr("readonly", "readonly");

        if (!string.IsNullOrEmpty(Id))
            textarea.Id(Id);

        if (!string.IsNullOrEmpty(Class))
            textarea.Class(Class);

        if (!string.IsNullOrEmpty(Style))
            textarea.Style(Style);

        foreach (var kvp in Attributes)
        {
            textarea.Attr(kvp.Key, kvp.Value);
        }

        return textarea.Build();
    }
}
