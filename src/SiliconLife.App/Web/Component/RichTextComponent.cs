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
/// Rich text editor component (placeholder for editor library integration)
/// </summary>
public class RichTextComponent : ComponentBase
{
    private string _content = "";
    private string _placeholder = "请输入内容...";
    private bool _readonly = false;

    /// <summary>
    /// Set initial content
    /// </summary>
    public RichTextComponent Content(string content)
    {
        _content = content;
        return this;
    }

    /// <summary>
    /// Set placeholder text
    /// </summary>
    public RichTextComponent Placeholder(string placeholder)
    {
        _placeholder = placeholder;
        return this;
    }

    /// <summary>
    /// Set readonly mode
    /// </summary>
    public RichTextComponent Readonly(bool readonly_ = true)
    {
        _readonly = readonly_;
        return this;
    }

    public override H Render()
    {
        var editor = H.Div();

        if (!string.IsNullOrEmpty(Id))
            editor.Id(Id);

        var classes = new List<string> { "rich-text-editor" };
        if (!string.IsNullOrEmpty(Class))
            classes.Add(Class);
        editor.Class(string.Join(" ", classes));

        if (Style != null && Style.HasInlineStyles)
            editor.Style(Style);

        foreach (var kvp in Attributes)
        {
            editor.Attr(kvp.Key, kvp.Value);
        }

        var toolbar = H.Div().Class("rich-text-toolbar");
        toolbar.Add(H.Button().Text("B").Class("toolbar-btn").Attr("data-command", "bold"));
        toolbar.Add(H.Button().Text("I").Class("toolbar-btn").Attr("data-command", "italic"));
        toolbar.Add(H.Button().Text("U").Class("toolbar-btn").Attr("data-command", "underline"));
        toolbar.Add(H.Button().Text("H1").Class("toolbar-btn").Attr("data-command", "heading1"));
        toolbar.Add(H.Button().Text("H2").Class("toolbar-btn").Attr("data-command", "heading2"));
        toolbar.Add(H.Button().Text("List").Class("toolbar-btn").Attr("data-command", "insertUnorderedList"));
        editor.Add(toolbar);

        var contentArea = H.Div()
            .Class("rich-text-content")
            .Attr("contenteditable", _readonly ? "false" : "true")
            .Attr("data-placeholder", _placeholder);

        if (!string.IsNullOrEmpty(_content))
        {
            contentArea.AddRendered(_content);
        }

        editor.Add(contentArea);

        return editor;
    }
}
