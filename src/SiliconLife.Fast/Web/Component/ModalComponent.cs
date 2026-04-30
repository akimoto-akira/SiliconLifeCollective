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
/// Modal dialog component
/// </summary>
public class ModalComponent : ComponentBase
{
    private string _title = "";
    private ComponentBase? _body;
    private ComponentBase? _footer;
    private string _size = ""; // sm, lg, xl
    private bool _staticBackdrop = false;

    /// <summary>
    /// Set modal title
    /// </summary>
    public ModalComponent Title(string title)
    {
        _title = title;
        return this;
    }

    /// <summary>
    /// Set modal body content
    /// </summary>
    public ModalComponent Body(ComponentBase body)
    {
        _body = body;
        return this;
    }

    /// <summary>
    /// Set modal footer content
    /// </summary>
    public ModalComponent Footer(ComponentBase footer)
    {
        _footer = footer;
        return this;
    }

    /// <summary>
    /// Set modal size (sm/lg/xl)
    /// </summary>
    public ModalComponent Size(string size)
    {
        _size = size;
        return this;
    }

    /// <summary>
    /// Set click background to not close
    /// </summary>
    public ModalComponent StaticBackdrop(bool static_ = true)
    {
        _staticBackdrop = static_;
        return this;
    }

    public override string Render()
    {
        var modal = H.Div();

        if (!string.IsNullOrEmpty(Id))
            modal.Id(Id);

        var classes = new List<string> { "modal" };
        if (!string.IsNullOrEmpty(Class))
            classes.Add(Class);
        modal.Class(string.Join(" ", classes));

        modal.Attr("tabindex", "-1");

        if (_staticBackdrop)
            modal.Attr("data-bs-backdrop", "static");

        if (!string.IsNullOrEmpty(Style))
            modal.Style(Style);

        foreach (var kvp in Attributes)
        {
            modal.Attr(kvp.Key, kvp.Value);
        }

        // Dialog container
        var sizeClass = !string.IsNullOrEmpty(_size) ? $" modal-{_size}" : "";
        var dialogDiv = H.Div().Class($"modal-dialog{sizeClass}");
        var contentDiv = H.Div().Class("modal-content");

        // Header
        var headerDiv = H.Div().Class("modal-header");
        headerDiv.Add(H.Create("h5", H.Escape(_title)).Class("modal-title"));
        headerDiv.Add(H.Button()
            .Attr("type", "button")
            .Class("btn-close")
            .Attr("data-bs-dismiss", "modal")
            .Attr("aria-label", "Close"));
        contentDiv.Add(headerDiv);

        // Body
        if (_body != null)
        {
            var bodyDiv = H.Div().Class("modal-body");
            bodyDiv.AddRendered(_body.Render());
            contentDiv.Add(bodyDiv);
        }

        // Footer
        if (_footer != null)
        {
            var footerDiv = H.Div().Class("modal-footer");
            footerDiv.AddRendered(_footer.Render());
            contentDiv.Add(footerDiv);
        }

        dialogDiv.Add(contentDiv);
        modal.Add(dialogDiv);

        return modal.Build();
    }
}
