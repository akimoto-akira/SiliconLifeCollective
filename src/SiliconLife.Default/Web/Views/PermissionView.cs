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
        // 使用 CodeEditorView 生成的脚本
        return CodeEditorView.GetWidgetScripts(
            "permissionCallbackEditor",
            "csharp",
            "vs-dark",
            readOnly: false,
            minimap: true,
            lineNumbers: true,
            wordWrap: true,
            saveEndpoint: $"/api/permissions/save?beingId={beingId}"
        );
    }

    private static CssBuilder GetStyles()
    {
        // 使用 CodeEditorView 提供的样式
        return CodeEditorView.GetWidgetStyles();
    }
}
