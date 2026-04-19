// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using SiliconLife.Default.Web.Models;

namespace SiliconLife.Default.Web.Views;

public class PermissionView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as PermissionViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.PageTitlePermission, "permissions", vm.Localization, body, GetScripts(vm.Localization, vm.BeingId), GetStyles());
    }

    private static H RenderBody(PermissionViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1(string.Format(vm.Localization.PermissionPageHeader, vm.BeingId.ToString()))
            ).Class("page-header"),
            H.Div(
                CodeEditorView.RenderWidget(
                    "permissionCallbackEditor",
                    vm.CurrentCallbackCode ?? "",
                    "csharp",
                    $"Permission Callback - Being {vm.BeingId}",
                    readOnly: false,
                    theme: "vs-dark",
                    minimap: true,
                    lineNumbers: true,
                    wordWrap: true,
                    saveEndpoint: $"/api/permissions/save?beingId={vm.BeingId}"
                )
            ).Class("card")
        ).Class("page-content");
    }

    private static JsSyntax GetScripts(DefaultLocalizationBase loc, Guid beingId)
    {
        var editorInitBody = Js.Block()
            .Add(() => Js.Id(() => $"codeEditorInit_permissionCallbackEditor").Invoke().Stmt());
    
        return Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Block()
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    (Js.Id(() => "typeof").Invoke(() => Js.Id(() => "require")).Op(() => "!==", () => Js.Str(() => "undefined")), new List<JsSyntax>
                    {
                        Js.Id(() => "editorInitBody").Invoke().Stmt()
                    }),
                    (null, new List<JsSyntax>
                    {
                        Js.Let(() => "loaderScript", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "script"))),
                        Js.Assign(() => Js.Id(() => "loaderScript").Prop(() => "src"), () => Js.Str(() => "https://cdn.jsdelivr.net/npm/monaco-editor@0.52.2/min/vs/loader.js")),
                        Js.Assign(() => Js.Id(() => "loaderScript").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "editorInitBody").Invoke())),
                        Js.Id(() => "document").Prop(() => "head").Call(() => "appendChild", () => Js.Id(() => "loaderScript")).Stmt()
                    })
                }))
            )));
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".code-editor-widget")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "8px")
                .Property("overflow", "hidden")
                .Property("background", "var(--bg-card)")
            .EndSelector()
            .Selector(".code-editor-toolbar")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "10px")
                .Property("padding", "8px 14px")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.05))")
                .Property("border-bottom", "1px solid var(--border)")
                .Property("font-size", "13px")
            .EndSelector()
            .Selector(".code-editor-filename")
                .Property("color", "var(--text-primary)")
                .Property("font-weight", "500")
                .Property("flex", "1")
                .Property("overflow", "hidden")
                .Property("text-overflow", "ellipsis")
                .Property("white-space", "nowrap")
            .EndSelector()
            .Selector(".code-editor-lang-badge")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#fff")
                .Property("padding", "2px 8px")
                .Property("border-radius", "4px")
                .Property("font-size", "11px")
                .Property("font-weight", "600")
                .Property("letter-spacing", "0.5px")
            .EndSelector()
            .Selector(".code-editor-btn-save")
                .Property("background", "none")
                .Property("border", "none")
                .Property("cursor", "pointer")
                .Property("font-size", "18px")
                .Property("padding", "2px 6px")
                .Property("border-radius", "4px")
                .Property("transition", "background 0.2s")
            .EndSelector()
            .Selector(".code-editor-btn-save:hover")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.1))")
            .EndSelector()
            .Selector(".code-editor-container")
                .Property("flex", "1")
                .Property("min-height", "500px")
                .Property("width", "100%")
            .EndSelector();
    }
}
