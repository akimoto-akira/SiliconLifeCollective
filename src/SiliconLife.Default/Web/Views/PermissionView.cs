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
        return RenderPage(vm.Skin, vm.Localization.PageTitlePermission, "beings", vm.Localization, body, GetScripts(vm.Localization), GetStyles());
    }

    private static H RenderBody(PermissionViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1(vm.Localization.PermissionPageHeader)
            ).Class("page-header"),
            H.Div(
                H.Div().Id("permissions-list").Class("permissions-list")
            ).Class("card")
        ).Class("page-content");
    }

    private static JsSyntax GetScripts(DefaultLocalizationBase loc)
    {
        var forEachBody = Js.Block()
            .Add(() => Js.Const(() => "row", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "row").Prop(() => "className"), () => Js.Str(() => "permission-rule")))
            .Add(() => Js.Const(() => "resultClass", () => Js.Ternary(() => Js.Id(() => "r").Prop(() => "result").Op(() => "===", () => Js.Str(() => "Allowed")), () => Js.Str(() => "result-allowed"), () => Js.Ternary(() => Js.Id(() => "r").Prop(() => "result").Op(() => "===", () => Js.Str(() => "Denied")), () => Js.Str(() => "result-denied"), () => Js.Str(() => "result-ask")))))
            .Add(() => Js.Assign(() => Js.Id(() => "row").Prop(() => "innerHTML"), () => Js.Str(() => "<div class=\"rule-type\">").Op(() => "+", () => (JsSyntax)Js.Id(() => "r").Prop(() => "permissionType")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div><div class=\"rule-prefix\">")).Op(() => "+", () => (JsSyntax)Js.Id(() => "r").Prop(() => "resourcePrefix")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div><div class=\"rule-result ")).Op(() => "+", () => (JsSyntax)Js.Id(() => "resultClass")).Op(() => "+", () => (JsSyntax)Js.Str(() => "\">")).Op(() => "+", () => (JsSyntax)Js.Id(() => "r").Prop(() => "result")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div><div class=\"rule-desc\">")).Op(() => "+", () => (JsSyntax)Js.Id(() => "r").Prop(() => "description")).Op(() => "+", () => (JsSyntax)Js.Str(() => "</div>"))))
            .Add(() => Js.Id(() => "list").Call(() => "appendChild", () => Js.Id(() => "row")).Stmt());

        var thenBody = Js.Block()
            .Add(() => Js.Const(() => "list", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "permissions-list"))))
            .Add(() => Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "data").Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "list").Prop(() => "innerHTML"), () => Js.Str(() => $"<p>{loc.PermissionEmptyState}</p>"))
                    }
                )}
            }))
            .Add(() => Js.Id(() => "data").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "r" }, () => forEachBody)).Stmt());

        var loadPermissionsBody = Js.Block()
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/permissions/list")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => thenBody)).Stmt());

        return Js.Block()
            .Add(() => Js.Func(() => "loadPermissions", () => new List<string>(), () => loadPermissionsBody))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "onload"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "loadPermissions").Invoke())));
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".permissions-list")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "8px")
            .EndSelector()
            .Selector(".permission-rule")
                .Property("display", "grid")
                .Property("grid-template-columns", "140px 1fr 100px 1fr")
                .Property("gap", "12px")
                .Property("padding", "12px 16px")
                .Property("background", "var(--bg-secondary, rgba(255,255,255,0.03))")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "8px")
                .Property("font-size", "13px")
                .Property("align-items", "center")
            .EndSelector()
            .Selector(".rule-type")
                .Property("font-weight", "600")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".rule-prefix")
                .Property("color", "var(--text-secondary)")
                .Property("word-break", "break-all")
                .Property("font-family", "monospace")
                .Property("font-size", "12px")
            .EndSelector()
            .Selector(".rule-result")
                .Property("font-weight", "600")
                .Property("text-align", "center")
                .Property("padding", "2px 8px")
                .Property("border-radius", "4px")
                .Property("font-size", "12px")
            .EndSelector()
            .Selector(".result-allowed")
                .Property("color", "var(--accent-success, #4CAF50)")
                .Property("background", "rgba(76,175,80,0.1)")
            .EndSelector()
            .Selector(".result-denied")
                .Property("color", "var(--accent-error, #f44336)")
                .Property("background", "rgba(244,67,54,0.1)")
            .EndSelector()
            .Selector(".result-ask")
                .Property("color", "var(--accent-warning, #FF9800)")
                .Property("background", "rgba(255,152,0,0.1)")
            .EndSelector()
            .Selector(".rule-desc")
                .Property("color", "var(--text-secondary)")
                .Property("font-size", "12px")
            .EndSelector();
    }
}
