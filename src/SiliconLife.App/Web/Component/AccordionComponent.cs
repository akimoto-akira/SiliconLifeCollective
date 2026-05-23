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
/// Accordion component
/// </summary>
public class AccordionComponent : ComponentBase
{
    private readonly List<(string Id, string Title, ComponentBase Content, bool IsOpen)> _items = new();

    /// <summary>
    /// Add an accordion item
    /// </summary>
    public AccordionComponent AddItem(string id, string title, ComponentBase content, bool isOpen = false)
    {
        _items.Add((id, title, content, isOpen));
        return this;
    }

    public override H Render()
    {
        var accordion = H.Div();

        if (!string.IsNullOrEmpty(Id))
            accordion.Id(Id);

        var classes = new List<string> { "accordion" };
        if (!string.IsNullOrEmpty(Class))
            classes.Add(Class);
        accordion.Class(string.Join(" ", classes));

        if (Style != null && Style.HasInlineStyles)
            accordion.Style(Style);

        foreach (var kvp in Attributes)
        {
            accordion.Attr(kvp.Key, kvp.Value);
        }

        foreach (var item in _items)
        {
            var itemClass = item.IsOpen ? "" : " collapsed";

            var itemDiv = H.Div().Class("accordion-item");

            var headerH2 = H.H2().Class("accordion-header").Id($"heading-{item.Id}");
            var button = H.Button()
                .Class($"accordion-button{itemClass}")
                .Attr("type", "button")
                .Attr("data-bs-toggle", "collapse")
                .Attr("data-bs-target", $"#collapse-{item.Id}")
                .Attr("aria-expanded", item.IsOpen ? "true" : "false")
                .Attr("aria-controls", $"collapse-{item.Id}")
                .Text(H.Escape(item.Title));
            headerH2.Add(button);
            itemDiv.Add(headerH2);

            var showClass = item.IsOpen ? " show" : "";
            var collapseDiv = H.Div()
                .Id($"collapse-{item.Id}")
                .Class($"accordion-collapse collapse{showClass}")
                .Attr("aria-labelledby", $"heading-{item.Id}")
                .Attr("data-bs-parent", $"#{Id}");
            var bodyDiv = H.Div().Class("accordion-body");
            bodyDiv.Add(item.Content.Render());
            collapseDiv.Add(bodyDiv);
            itemDiv.Add(collapseDiv);

            accordion.Add(itemDiv);
        }

        return accordion;
    }
}
