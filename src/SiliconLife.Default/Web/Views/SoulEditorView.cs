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

using SiliconLife.Default.Web.Models;

namespace SiliconLife.Default.Web.Views;

public class SoulEditorView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as SoulEditorViewModel;
        if (vm == null) return string.Empty;
        
        var body = RenderBody(vm);
        var scripts = GetScripts(vm);
        var styles = GetStyles();
        
        return RenderPage(vm.Skin, vm.Localization.PageTitleBeings, "beings", vm.Localization, body, scripts, styles, helpTopicId: "being-soul");
    }

    private static H RenderBody(SoulEditorViewModel vm)
    {
        var editorId = "soulEditor";  // Use camelCase to avoid invalid JS identifier (no hyphens)
        var editorWidget = MarkdownEditorView.RenderWidget(
            editorId,
            vm.SoulContent,
            $"soul.md ({vm.BeingName})",
            readOnly: false,
            initialMode: "edit",
            saveEndpoint: $"/api/beings/soul/save?beingId={vm.BeingId}"
        );

        return H.Div(
            H.Div(
                H.Div(
                    H.A("← " + vm.Localization.BeingsBackToList).Href("/beings").Class("soul-editor-back-link"),
                    H.H1(vm.BeingName).Class("soul-editor-title"),
                    H.P(vm.Localization.SoulEditorSubtitle).Class("soul-editor-subtitle")
                ).Class("soul-editor-header"),
                H.Div(
                    editorWidget
                ).Class("soul-editor-container")
            ).Class("page-content")
        );
    }

    private static CssBuilder GetStyles()
    {
        var markdownStyles = MarkdownEditorView.GetWidgetStyles();
        
        return markdownStyles
            .Selector(".soul-editor-header")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".soul-editor-back-link")
                .Property("display", "inline-block")
                .Property("margin-bottom", "12px")
                .Property("color", "var(--accent-primary)")
                .Property("text-decoration", "none")
                .Property("font-size", "14px")
                .Property("transition", "color 0.2s")
            .EndSelector()
            .Selector(".soul-editor-back-link:hover")
                .Property("color", "var(--accent-secondary, var(--accent-primary))")
                .Property("text-decoration", "underline")
            .EndSelector()
            .Selector(".soul-editor-title")
                .Property("font-size", "28px")
                .Property("font-weight", "bold")
                .Property("color", "var(--text-primary)")
                .Property("margin", "0 0 8px 0")
            .EndSelector()
            .Selector(".soul-editor-subtitle")
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
                .Property("margin", "0")
            .EndSelector()
            .Selector(".soul-editor-container")
                .Property("height", "calc(100vh - 200px)")
                .Property("min-height", "400px")
            .EndSelector();
    }

    private static JsSyntax GetScripts(SoulEditorViewModel vm)
    {
        var editorId = "soulEditor";  // Use camelCase to avoid invalid JS identifier (no hyphens)
        var editorScripts = MarkdownEditorView.GetWidgetScripts(
            editorId,
            initialContent: vm.SoulContent,
            readOnly: false,
            saveEndpoint: $"/api/beings/soul/save?beingId={vm.BeingId}"
        );

        return editorScripts;
    }
}
