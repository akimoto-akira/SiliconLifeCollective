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
/// Tabs component
/// </summary>
public class TabsComponent : ComponentBase
{
    private readonly List<(string Id, string Label, ComponentBase Content)> _tabs = new();
    private string _activeTabId = "";

    /// <summary>
    /// Add a tab
    /// </summary>
    public TabsComponent AddTab(string id, string label, ComponentBase content)
    {
        _tabs.Add((id, label, content));
        if (string.IsNullOrEmpty(_activeTabId))
        {
            _activeTabId = id;
        }
        return this;
    }

    /// <summary>
    /// Set default active tab
    /// </summary>
    public TabsComponent Active(string tabId)
    {
        _activeTabId = tabId;
        return this;
    }

    public override string Render()
    {
        var container = H.Div();

        if (!string.IsNullOrEmpty(Id))
            container.Id(Id);

        if (!string.IsNullOrEmpty(Class))
            container.Class(Class);

        if (!string.IsNullOrEmpty(Style))
            container.Style(Style);

        foreach (var kvp in Attributes)
        {
            container.Attr(kvp.Key, kvp.Value);
        }

        // Tab navigation
        var navUl = H.Ul().Class("nav nav-tabs").Attr("role", "tablist");
        foreach (var tab in _tabs)
        {
            var isActive = tab.Id == _activeTabId;
            var activeClass = isActive ? " active" : "";
            var ariaSelected = isActive ? "true" : "false";

            var li = H.Li().Class("nav-item").Attr("role", "presentation");
            var button = H.Button()
                .Class($"nav-link{activeClass}")
                .Id($"{tab.Id}-tab")
                .Attr("data-bs-toggle", "tab")
                .Attr("data-bs-target", $"#{tab.Id}")
                .Attr("type", "button")
                .Attr("role", "tab")
                .Attr("aria-controls", tab.Id)
                .Attr("aria-selected", ariaSelected)
                .Text(H.Escape(tab.Label));
            li.Add(button);
            navUl.Add(li);
        }
        container.Add(navUl);

        // Tab content
        var contentDiv = H.Div().Class("tab-content");
        foreach (var tab in _tabs)
        {
            var isActive = tab.Id == _activeTabId;
            var activeClass = isActive ? " show active" : "";

            var paneDiv = H.Div()
                .Class($"tab-pane fade{activeClass}")
                .Id(tab.Id)
                .Attr("role", "tabpanel")
                .Attr("aria-labelledby", $"{tab.Id}-tab");
            paneDiv.Add(new RawHtml(tab.Content.Render()));
            contentDiv.Add(paneDiv);
        }
        container.Add(contentDiv);

        return container.Build();
    }
}
