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
/// File upload component
/// </summary>
public class FileUploadComponent : ComponentBase
{
    private string _accept = "";
    private bool _multiple = false;
    private bool _required = false;
    private string _label = "选择文件";

    /// <summary>
    /// Set accepted file types
    /// </summary>
    public FileUploadComponent Accept(string accept)
    {
        _accept = accept;
        return this;
    }

    /// <summary>
    /// Allow multiple file selection
    /// </summary>
    public FileUploadComponent Multiple(bool multiple = true)
    {
        _multiple = multiple;
        return this;
    }

    /// <summary>
    /// Set required
    /// </summary>
    public FileUploadComponent Required(bool required = true)
    {
        _required = required;
        return this;
    }

    /// <summary>
    /// Set upload button label
    /// </summary>
    public FileUploadComponent Label(string label)
    {
        _label = label;
        return this;
    }

    public override string Render()
    {
        var upload = H.Div();

        if (!string.IsNullOrEmpty(Id))
            upload.Id(Id);

        var classes = new List<string> { "file-upload" };
        if (!string.IsNullOrEmpty(Class))
            classes.Add(Class);
        upload.Class(string.Join(" ", classes));

        if (Style != null && Style.HasInlineStyles)
            upload.Style(Style);

        foreach (var kvp in Attributes)
        {
            upload.Attr(kvp.Key, kvp.Value);
        }

        // File input
        var input = H.Input()
            .Attr("type", "file")
            .Class("file-upload-input");

        if (!string.IsNullOrEmpty(_accept))
            input.Attr("accept", _accept);

        if (_multiple)
            input.Attr("multiple", "multiple");

        if (_required)
            input.Attr("required", "required");

        upload.Add(input);

        // Upload button label
        upload.Add(H.Span(H.Escape(_label)).Class("file-upload-label"));

        // File list display area
        upload.Add(H.Div().Class("file-upload-list"));

        return upload.Build();
    }
}
