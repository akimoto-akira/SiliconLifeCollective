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
/// Card container component
/// </summary>
public class CardComponent : ComponentBase
{
    private ComponentBase? _header;
    private ComponentBase? _body;
    private ComponentBase? _footer;
    private string _title = "";

    /// <summary>
    /// Set card title
    /// </summary>
    public CardComponent Title(string title)
    {
        _title = title;
        return this;
    }

    /// <summary>
    /// Set card header
    /// </summary>
    public CardComponent Header(ComponentBase header)
    {
        _header = header;
        return this;
    }

    /// <summary>
    /// Set card body
    /// </summary>
    public CardComponent Body(ComponentBase body)
    {
        _body = body;
        return this;
    }

    /// <summary>
    /// Set card footer
    /// </summary>
    public CardComponent Footer(ComponentBase footer)
    {
        _footer = footer;
        return this;
    }

    public override string Render()
    {
        var card = H.Div();

        if (!string.IsNullOrEmpty(Id))
            card.Id(Id);

        var classes = new List<string> { "card" };
        if (!string.IsNullOrEmpty(Class))
            classes.Add(Class);
        card.Class(string.Join(" ", classes));

        if (!string.IsNullOrEmpty(Style))
            card.Style(Style);

        foreach (var kvp in Attributes)
        {
            card.Attr(kvp.Key, kvp.Value);
        }

        // Header
        if (_header != null || !string.IsNullOrEmpty(_title))
        {
            var headerDiv = H.Div().Class("card-header");
            if (!string.IsNullOrEmpty(_title))
            {
                headerDiv.Add(H.H3(H.Escape(_title)).Class("card-title"));
            }
            if (_header != null)
            {
                headerDiv.AddRendered(_header.Render());
            }
            card.Add(headerDiv);
        }

        // Body
        if (_body != null)
        {
            var bodyDiv = H.Div().Class("card-body");
            bodyDiv.AddRendered(_body.Render());
            card.Add(bodyDiv);
        }

        // Footer
        if (_footer != null)
        {
            var footerDiv = H.Div().Class("card-footer");
            footerDiv.AddRendered(_footer.Render());
            card.Add(footerDiv);
        }

        return card.Build();
    }
}
