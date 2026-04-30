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
/// Select dropdown component
/// </summary>
public class SelectComponent : ComponentBase
{
    private readonly List<(string Value, string Text, bool Disabled)> _options = new();
    private string? _selectedValue;
    private bool _multiple = false;
    private bool _required = false;

    /// <summary>
    /// Add an option
    /// </summary>
    public SelectComponent AddOption(string value, string text, bool disabled = false)
    {
        _options.Add((value, text, disabled));
        return this;
    }

    /// <summary>
    /// Add multiple options
    /// </summary>
    public SelectComponent AddOptions(IEnumerable<(string Value, string Text)> options)
    {
        foreach (var (value, text) in options)
        {
            _options.Add((value, text, false));
        }
        return this;
    }

    /// <summary>
    /// Set selected value
    /// </summary>
    public SelectComponent Selected(string value)
    {
        _selectedValue = value;
        return this;
    }

    /// <summary>
    /// Set multiple selection mode
    /// </summary>
    public SelectComponent Multiple(bool multiple = true)
    {
        _multiple = multiple;
        return this;
    }

    /// <summary>
    /// Set required
    /// </summary>
    public SelectComponent Required(bool required = true)
    {
        _required = required;
        return this;
    }

    public override string Render()
    {
        var select = H.Select();

        if (!string.IsNullOrEmpty(Id))
            select.Id(Id);

        var classes = new List<string>();
        if (!string.IsNullOrEmpty(Class))
            classes.Add(Class);
        
        if (classes.Count > 0)
            select.Class(string.Join(" ", classes));

        if (!string.IsNullOrEmpty(Style))
            select.Style(Style);

        if (_multiple)
            select.Attr("multiple", "multiple");

        if (_required)
            select.Attr("required", "required");

        foreach (var kvp in Attributes)
        {
            select.Attr(kvp.Key, kvp.Value);
        }

        // Add options
        foreach (var (value, text, disabled) in _options)
        {
            var option = H.Option()
                .Attr("value", H.Escape(value))
                .Text(H.Escape(text));

            if (value == _selectedValue)
                option.Attr("selected", "selected");

            if (disabled)
                option.Attr("disabled", "disabled");

            select.Add(option);
        }

        return select.Build();
    }
}
