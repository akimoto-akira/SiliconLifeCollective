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
/// Form component
/// </summary>
public class FormComponent : ComponentBase
{
    private readonly List<ComponentBase> _children = new();
    private string _action = "";
    private string _method = "POST";
    private string? _onsubmit;

    /// <summary>
    /// Set form action URL
    /// </summary>
    public FormComponent Action(string action)
    {
        _action = action;
        return this;
    }

    /// <summary>
    /// Set form method (GET/POST)
    /// </summary>
    public FormComponent Method(string method)
    {
        _method = method.ToUpper();
        return this;
    }

    /// <summary>
    /// Add child component
    /// </summary>
    public FormComponent Add(ComponentBase child)
    {
        _children.Add(child);
        return this;
    }

    /// <summary>
    /// Set submit behavior
    /// </summary>
    public FormComponent OnSubmit(Behavior behavior)
    {
        _onsubmit = behavior.Build();
        return this;
    }

    public override string Render()
    {
        var form = H.Form();

        if (!string.IsNullOrEmpty(_action))
            form.Attr("action", _action);

        form.Attr("method", _method);

        if (!string.IsNullOrEmpty(_onsubmit))
            form.Attr("onsubmit", $"{_onsubmit}; return false;");

        if (!string.IsNullOrEmpty(Id))
            form.Id(Id);

        if (!string.IsNullOrEmpty(Class))
            form.Class(Class);

        if (!string.IsNullOrEmpty(Style))
            form.Style(Style);

        foreach (var kvp in Attributes)
        {
            form.Attr(kvp.Key, kvp.Value);
        }

        foreach (var child in _children)
        {
            form.Add(new RawHtml(child.Render()));
        }

        return form.Build();
    }
}
