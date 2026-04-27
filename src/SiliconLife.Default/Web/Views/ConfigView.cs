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

public class ConfigView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as ConfigViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.PageTitleConfig, "config", vm.Localization, body, GetScripts(vm.Localization), GetStyles(), helpTopicId: "config");
    }

    private static H RenderBody(ConfigViewModel vm)
    {
        var loc = vm.Localization;
        var groupCards = new List<H>();

        foreach (var group in vm.Groups)
        {
            var rows = new List<H>();
            foreach (var item in group.Items)
            {
                var enumValuesAttr = item.EnumValues != null 
                    ? $"[{string.Join(",", item.EnumValues.Select(v => $"\"{v}\""))}]" 
                    : "";
                
                var enumDisplayNamesAttr = item.EnumDisplayNames != null 
                    ? $"[{string.Join(",", item.EnumDisplayNames.Select(v => $"\"{v}\""))}]" 
                    : "";
                
                var editBtn = H.Button(loc.ConfigEditButton).Class("btn btn-sm btn-edit")
                    .Data("key", item.PropertyName)
                    .Data("display", item.DisplayName)
                    .Data("type", item.PropertyType);
                
                if (!string.IsNullOrEmpty(enumValuesAttr))
                {
                    editBtn = editBtn.Attr("data-enum-values", enumValuesAttr);
                }
                
                if (!string.IsNullOrEmpty(enumDisplayNamesAttr))
                {
                    editBtn = editBtn.Attr("data-enum-display-names", enumDisplayNamesAttr);
                }

                var tr = H.Tr(
                    H.Td(item.DisplayName),
                    H.Td(item.Value ?? loc.ConfigNullValue).Class("config-value").Data("key", item.PropertyName),
                    H.Td(editBtn)
                );

                if (!string.IsNullOrEmpty(item.DependsOn) && !string.IsNullOrEmpty(item.DependsOnValue))
                {
                    tr = tr.Attr("data-depends-on", item.DependsOn)
                           .Attr("data-depends-on-value", item.DependsOnValue);
                }

                rows.Add(tr);
            }

            groupCards.Add(H.Div(
                H.H3(group.Name),
                H.Table(
                    H.Thead(
                        H.Tr(
                            H.Th(loc.ConfigPropertyNameLabel),
                            H.Th(loc.ConfigPropertyValueLabel),
                            H.Th(loc.ConfigActionLabel)
                        )
                    ),
                    H.Tbody(rows.ToArray())
                )
            ).Class("card"));
        }

        return H.Div(
            H.Div(
                H.H1(loc.ConfigPageHeader)
            ).Class("page-header"),
            H.Div(groupCards.ToArray()),
            H.Div(
                H.Div(
                    H.Div(
                        H.H3(loc.ConfigEditModalTitle).Id("editModalTitle"),
                        H.Div(
                            H.Label(loc.ConfigEditPropertyLabel).Attr("for", "editKey"),
                            H.Input().Attr("type", "text").Id("editKey").Attr("readonly", "readonly")
                        ).Class("form-group"),
                        H.Div(
                            H.Label(loc.ConfigEditValueLabel).Attr("for", "editValue"),
                            H.Input().Attr("type", "text").Id("editValue")
                        ).Class("form-group").Id("inputString"),
                        H.Div(
                            H.Label(loc.ConfigEditValueLabel).Attr("for", "editValueNumber"),
                            H.Input().Attr("type", "number").Id("editValueNumber").Attr("step", "any")
                        ).Class("form-group").Id("inputNumber").Style("display:none"),
                        H.Div(
                            H.Label(loc.ConfigEditValueLabel).Attr("for", "editValueBool"),
                            H.Input().Attr("type", "checkbox").Id("editValueBool")
                        ).Class("form-group").Id("inputBool").Style("display:none"),
                        H.Div(
                            H.Label(loc.ConfigEditValueLabel).Attr("for", "editValueDirectory"),
                            H.Div(
                                H.Input().Attr("type", "text").Id("editValueDirectory").Attr("readonly", "readonly"),
                                H.Button(loc.ConfigBrowseButton).Class("btn btn-sm").Id("btnBrowseDir")
                            ).Class("input-with-btn"),
                            H.Div().Id("dirBrowser").Class("dir-browser").Style("display:none")
                        ).Class("form-group").Id("inputDirectory").Style("display:none"),
                        H.Div(
                            H.Label(loc.ConfigEditValueLabel).Attr("for", "editValueDatetime"),
                            H.Input().Attr("type", "datetime-local").Id("editValueDatetime")
                        ).Class("form-group").Id("inputDatetime").Style("display:none"),
                        H.Div(
                            H.Label(loc.ConfigTimeSettingsLabel),
                            H.Div(
                                H.Label(loc.ConfigDaysLabel).Attr("for", "editValueTimespanDays"),
                                H.Input().Attr("type", "number").Id("editValueTimespanDays").Attr("min", "0").Attr("step", "1").Class("timespan-input"),
                                H.Label(loc.ConfigHoursLabel).Attr("for", "editValueTimespanHours"),
                                H.Input().Attr("type", "number").Id("editValueTimespanHours").Attr("min", "0").Attr("max", "23").Attr("step", "1").Class("timespan-input"),
                                H.Label(loc.ConfigMinutesLabel).Attr("for", "editValueTimespanMinutes"),
                                H.Input().Attr("type", "number").Id("editValueTimespanMinutes").Attr("min", "0").Attr("max", "59").Attr("step", "1").Class("timespan-input"),
                                H.Label(loc.ConfigSecondsLabel).Attr("for", "editValueTimespanSeconds"),
                                H.Input().Attr("type", "number").Id("editValueTimespanSeconds").Attr("min", "0").Attr("max", "59").Attr("step", "1").Class("timespan-input")
                            ).Class("timespan-inputs")
                        ).Class("form-group").Id("inputTimespan").Style("display:none"),
                        H.Div(
                            H.Label(loc.ConfigEditValueLabel).Attr("for", "editValueEnum"),
                            H.Select().Id("editValueEnum")
                        ).Class("form-group").Id("inputEnum").Style("display:none"),
                        H.Div(
                            H.Label(loc.ConfigDictionaryLabel),
                            H.Div().Id("dictEditorContainer").Class("dict-editor"),
                            H.Div(
                                H.Button(loc.ConfigDictAddButton).Class("btn btn-add").Id("btnAddDictRow")
                            ).Class("dict-actions")
                        ).Class("form-group").Id("inputDictionary").Style("display:none"),
                        H.Div(
                            H.Button(loc.ConfigSaveButton).Class("btn btn-primary").Id("btnSave"),
                            H.Button(loc.ConfigCancelButton).Class("btn btn-secondary").Id("btnCancel")
                        ).Class("form-actions")
                    ).Class("modal-content")
                ).Class("modal-dialog")
            ).Id("editModal").Class("modal")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".card h3")
                .Property("margin-bottom", "16px")
                .Property("font-size", "16px")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".card table th:nth-child(1), .card table td:nth-child(1)")
                .Property("width", "180px")
                .Property("min-width", "180px")
            .EndSelector()
            .Selector(".card table th:nth-child(3), .card table td:nth-child(3)")
                .Property("width", "80px")
                .Property("min-width", "80px")
                .Property("text-align", "center")
            .EndSelector()
            .Selector(".modal")
                .Property("display", "none")
                .Property("position", "fixed")
                .Property("top", "0")
                .Property("left", "0")
                .Property("width", "100%")
                .Property("height", "100%")
                .Property("background-color", "rgba(0,0,0,0.5)")
                .Property("z-index", "1000")
            .EndSelector()
            .Selector(".modal.show")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("justify-content", "center")
            .EndSelector()
            .Selector(".modal-dialog")
                .Property("background", "var(--bg-secondary)")
                .Property("border-radius", "8px")
                .Property("box-shadow", "0 4px 20px rgba(0,0,0,0.3)")
                .Property("max-width", "400px")
                .Property("width", "90%")
            .EndSelector()
            .Selector(".modal-content")
                .Property("padding", "20px")
            .EndSelector()
            .Selector(".modal-content h3")
                .Property("margin", "0 0 16px 0")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".form-group")
                .Property("margin-bottom", "12px")
            .EndSelector()
            .Selector(".form-group label")
                .Property("display", "block")
                .Property("margin-bottom", "4px")
                .Property("color", "var(--text-secondary)")
                .Property("font-size", "14px")
            .EndSelector()
            .Selector(".form-group input")
                .Property("width", "100%")
                .Property("padding", "8px 12px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "4px")
                .Property("background", "var(--bg-primary)")
                .Property("color", "var(--text-primary)")
                .Property("box-sizing", "border-box")
            .EndSelector()
            .Selector(".form-group input[type=\"checkbox\"]")
                .Property("width", "auto")
                .Property("padding", "0")
                .Property("width", "18px")
                .Property("height", "18px")
                .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".form-group input[readonly]")
                .Property("background", "var(--bg-tertiary)")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".form-group select")
                .Property("width", "100%")
                .Property("padding", "8px 12px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "4px")
                .Property("background", "var(--bg-primary)")
                .Property("color", "var(--text-primary)")
                .Property("box-sizing", "border-box")
            .EndSelector()
            .Selector(".input-with-btn")
                .Property("display", "flex")
                .Property("gap", "8px")
            .EndSelector()
            .Selector(".input-with-btn input")
                .Property("flex", "1")
            .EndSelector()
            .Selector(".timespan-inputs")
                .Property("display", "flex")
                .Property("gap", "8px")
                .Property("align-items", "center")
            .EndSelector()
            .Selector(".timespan-inputs label")
                .Property("display", "inline")
                .Property("margin-bottom", "0")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".timespan-input")
                .Property("width", "60px")
                .Property("padding", "6px 8px")
                .Property("text-align", "center")
            .EndSelector()
            .Selector(".form-actions")
                .Property("margin-top", "20px")
                .Property("display", "flex")
                .Property("gap", "8px")
                .Property("justify-content", "flex-end")
            .EndSelector()
            .Selector(".btn-primary")
                .Property("background", "var(--accent-color)")
                .Property("color", "#fff")
            .EndSelector()
            .Selector(".btn-secondary")
                .Property("background", "var(--bg-tertiary)")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".dir-browser")
                .Property("margin-top", "8px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "4px")
                .Property("background", "var(--bg-tertiary)")
                .Property("max-height", "200px")
                .Property("overflow-y", "auto")
            .EndSelector()
            .Selector(".dir-browser::-webkit-scrollbar")
                .Property("width", "6px")
            .EndSelector()
            .Selector(".dir-browser::-webkit-scrollbar-track")
                .Property("background", "transparent")
            .EndSelector()
            .Selector(".dir-browser::-webkit-scrollbar-thumb")
                .Property("background", "var(--border-color)")
                .Property("border-radius", "3px")
            .EndSelector()
            .Selector(".dir-header")
                .Property("padding", "8px 12px")
                .Property("border-bottom", "1px solid var(--border-color)")
                .Property("position", "sticky")
                .Property("top", "0")
                .Property("background", "var(--bg-tertiary)")
            .EndSelector()
            .Selector(".dir-current")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
                .Property("font-family", "monospace")
                .Property("overflow", "hidden")
                .Property("text-overflow", "ellipsis")
                .Property("white-space", "nowrap")
                .Property("display", "block")
            .EndSelector()
            .Selector(".dir-list")
                .Property("padding", "4px 0")
            .EndSelector()
            .Selector(".dir-item")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "8px")
                .Property("padding", "6px 12px")
                .Property("cursor", "pointer")
                .Property("font-size", "13px")
                .Property("color", "var(--text-primary)")
                .Property("transition", "background 0.15s")
            .EndSelector()
            .Selector(".dir-item:hover")
                .Property("background", "var(--accent-color)")
                .Property("color", "#fff")
            .EndSelector()
            .Selector(".dir-parent")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".dir-icon")
                .Property("font-size", "14px")
            .EndSelector()
            .Selector(".dict-editor")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "4px")
                .Property("padding", "12px")
                .Property("background", "var(--bg-primary)")
            .EndSelector()
            .Selector(".dict-editor::-webkit-scrollbar")
                .Property("width", "6px")
            .EndSelector()
            .Selector(".dict-editor::-webkit-scrollbar-track")
                .Property("background", "transparent")
            .EndSelector()
            .Selector(".dict-editor::-webkit-scrollbar-thumb")
                .Property("background", "var(--border-color)")
                .Property("border-radius", "3px")
            .EndSelector()
            .Selector(".dict-row")
                .Property("display", "flex")
                .Property("gap", "8px")
                .Property("align-items", "center")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".dict-row input, .dict-row select")
                .Property("flex", "1")
                .Property("padding", "6px 10px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "4px")
                .Property("background", "var(--bg-tertiary)")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".dict-row .btn-delete")
                .Property("padding", "6px 12px")
                .Property("background", "#dc3545")
                .Property("color", "#fff")
                .Property("border", "none")
                .Property("border-radius", "4px")
                .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".dict-row .btn-delete:hover")
                .Property("background", "#c82333")
            .EndSelector()
            .Selector(".dict-empty")
                .Property("text-align", "center")
                .Property("color", "var(--text-secondary)")
                .Property("padding", "20px")
                .Property("font-style", "italic")
            .EndSelector()
            .Selector(".dict-actions")
                .Property("margin-top", "8px")
                .Property("display", "flex")
                .Property("justify-content", "flex-end")
            .EndSelector()
            .Selector(".dict-actions .btn-add")
                .Property("padding", "6px 16px")
                .Property("background", "var(--accent-color)")
                .Property("color", "#fff")
                .Property("border", "none")
                .Property("border-radius", "4px")
                .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".dict-actions .btn-add:hover")
                .Property("opacity", "0.9")
            .EndSelector();
    }

    private static JsSyntax GetScripts(DefaultLocalizationBase loc)
    {
        var js = Js.Block();

        js.Add(() => Js.Let(() => "currentType", () => Js.Str(() => "string")));
                js.Add(() => Js.Let(() => "isAIConfigDict", () => Js.Id(() => "false")));

        var hideAllInputsBlock = Js.Block()
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputString")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "none")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputNumber")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "none")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputBool")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "none")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputDirectory")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "none")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputDatetime")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "none")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputTimespan")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "none")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputEnum")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "none")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputDictionary")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "none")));
        js.Add(() => Js.Func(() => "hideAllInputs", () => new List<string> { }, () => hideAllInputsBlock));

        // Dictionary editor functions
        var initDictEditorBlock = Js.Block()
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dictEditorContainer")).Prop(() => "innerHTML").Assign(() => Js.Str(() => "")))
            .Add(() => Js.Let(() => "dict", () => Js.Ternary(() => Js.Id(() => "jsonStr"), () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "jsonStr")), () => Js.Obj())));
        
        var initDictForEachBlock = Js.Block()
            .Add(() => Js.Id(() => "addDictRow").Invoke(() => Js.Id(() => "k"), () => Js.Id(() => "dict").Index(() => Js.Id(() => "k"))));
        
        initDictEditorBlock.Add(() => Js.Id(() => "Object").Prop(() => "keys").Invoke(() => Js.Id(() => "dict")).Call(() => "forEach", () => Js.Arrow(() => new List<string> { "k" }, () => initDictForEachBlock)));
        initDictEditorBlock.Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
        {
            { (Js.Id(() => "Object").Prop(() => "keys").Invoke(() => Js.Id(() => "dict")).Prop(() => "length").Op(() => "===", () => Js.Num(() => "0")), new List<JsSyntax>
                {
                    Js.Id(() => "addDictRow").Invoke(() => Js.Str(() => ""), () => Js.Str(() => ""))
                })
            }
        }));
        
        js.Add(() => Js.Func(() => "initDictEditor", () => new List<string> { "jsonStr" }, () => initDictEditorBlock));

        var addDictRowBlock = Js.Block()
            .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dictEditorContainer"))))
            .Add(() => Js.Const(() => "row", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))));
        
        addDictRowBlock.Add(() => Js.Id(() => "row").Prop(() => "className").Assign(() => Js.Str(() => "dict-row")));
        addDictRowBlock.Add(() => Js.Const(() => "keyInput", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "input"))))
            .Add(() => Js.Id(() => "keyInput").Prop(() => "type").Assign(() => Js.Str(() => "text")))
            .Add(() => Js.Id(() => "keyInput").Prop(() => "placeholder").Assign(() => Js.Str(() => loc.ConfigDictKeyLabel)))
            .Add(() => Js.Id(() => "keyInput").Prop(() => "value").Assign(() => Js.Id(() => "key")));
        
        // Define fetch then-body for AIConfig dropdown
        var fetchThenBody = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "d").Prop(() => "success").Op(() => "&&", () => Js.Id(() => "d").Prop(() => "hasOptions")), new List<JsSyntax>
                    {
                        Js.Const(() => "sel", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "select"))).Stmt(),
                        Js.Id(() => "Object").Prop(() => "keys").Invoke(() => Js.Id(() => "d").Prop(() => "options")).Call(() => "forEach", () => Js.Arrow(() => new List<string> { "k" }, () => Js.Block()
                            .Add(() => Js.Const(() => "o", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "option"))))
                            .Add(() => Js.Id(() => "o").Prop(() => "value").Assign(() => Js.Id(() => "k")))
                            .Add(() => Js.Id(() => "o").Prop(() => "textContent").Assign(() => Js.Id(() => "d").Prop(() => "options").Index(() => Js.Id(() => "k"))))
                            .Add(() => Js.Id(() => "o").Prop(() => "selected").Assign(() => Js.Id(() => "k").Op(() => "===", () => Js.Id(() => "valueInput").Prop(() => "value"))))
                            .Add(() => Js.Id(() => "sel").Call(() => "appendChild", () => Js.Id(() => "o"))))
                        ).Stmt(),
                        Js.Id(() => "valueInput").Call(() => "replaceWith", () => Js.Id(() => "sel")).Stmt()
                    }
                )}
            }));
        
        var blurHandlerBody = Js.Block()
            .Add(() => Js.Const(() => "ck", () => Js.Id(() => "keyInput").Prop(() => "value")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "isAIConfigDict").Op(() => "&&", () => Js.Id(() => "ck")), new List<JsSyntax>
                    {
                        Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/config/aioptions?key=").Op(() => "+", () => Js.Id(() => "encodeURIComponent").Invoke(() => Js.Id(() => "ck")))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "d" }, () => (JsSyntax)fetchThenBody)).Stmt()
                    }
                )}
            }));
        
        addDictRowBlock.Add(() => Js.Id(() => "keyInput").Call(() => "addEventListener", () => Js.Str(() => "blur"), () => Js.Arrow(() => new List<string> { }, () => (JsSyntax)blurHandlerBody)));
        
        addDictRowBlock.Add(() => Js.Const(() => "valueInput", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "input"))))
            .Add(() => Js.Id(() => "valueInput").Prop(() => "type").Assign(() => Js.Str(() => "text")))
            .Add(() => Js.Id(() => "valueInput").Prop(() => "placeholder").Assign(() => Js.Str(() => loc.ConfigDictValueLabel)))
            .Add(() => Js.Id(() => "valueInput").Prop(() => "value").Assign(() => Js.Id(() => "value")));
        
        addDictRowBlock.Add(() => Js.Const(() => "deleteBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Id(() => "deleteBtn").Prop(() => "textContent").Assign(() => Js.Str(() => loc.ConfigDictDeleteButton)))
            .Add(() => Js.Id(() => "deleteBtn").Prop(() => "className").Assign(() => Js.Str(() => "btn-delete")))
            .Add(() => Js.Id(() => "deleteBtn").Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string> { }, () => Js.Id(() => "row").Prop(() => "remove").Invoke().Stmt())));
        
        addDictRowBlock.Add(() => Js.Id(() => "row").Call(() => "appendChild", () => Js.Id(() => "keyInput")))
            .Add(() => Js.Id(() => "row").Call(() => "appendChild", () => Js.Id(() => "valueInput")))
            .Add(() => Js.Id(() => "row").Call(() => "appendChild", () => Js.Id(() => "deleteBtn")))
            .Add(() => Js.Id(() => "container").Call(() => "appendChild", () => Js.Id(() => "row")));
        
        // Immediately fetch and render dropdown for existing AIConfig keys
        addDictRowBlock.Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
        {
            { (Js.Id(() => "isAIConfigDict").Op(() => "&&", () => Js.Id(() => "key")), new List<JsSyntax>
                {
                    Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/config/aioptions?key=").Op(() => "+", () => Js.Id(() => "encodeURIComponent").Invoke(() => Js.Id(() => "key")))).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "d" }, () => (JsSyntax)fetchThenBody)).Stmt()
                }
            )}
        }));
        
        js.Add(() => Js.Func(() => "addDictRow", () => new List<string> { "key", "value" }, () => addDictRowBlock));

        var getDictJsonBlock = Js.Block()
            .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dictEditorContainer"))))
            .Add(() => Js.Const(() => "rows", () => Js.Id(() => "container").Call(() => "querySelectorAll", () => Js.Str(() => ".dict-row"))))
            .Add(() => Js.Const(() => "dict", () => Js.Obj()));
        
        var processRowBlock = Js.Block()
            .Add(() => Js.Const(() => "inputs", () => Js.Id(() => "row").Call(() => "querySelectorAll", () => Js.Str(() => "input, select"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "inputs").Prop(() => "length").Op(() => ">=", () => Js.Num(() => "2")), new List<JsSyntax>
                    {
                        Js.Const(() => "k", () => Js.Id(() => "inputs").Index(() => Js.Num(() => "0")).Prop(() => "value")),
                        Js.Const(() => "v", () => Js.Id(() => "inputs").Index(() => Js.Num(() => "1")).Prop(() => "value")),
                        Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            { (Js.Id(() => "k"), new List<JsSyntax>
                                {
                                    Js.Id(() => "dict").Index(() => Js.Id(() => "k")).Assign(() => Js.Id(() => "v"))
                                })
                            }
                        })
                    })
                }
            }));
        
        getDictJsonBlock.Add(() => Js.Id(() => "rows").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "row" }, () => processRowBlock)));
        getDictJsonBlock.Add(() => Js.Return(() => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Id(() => "dict"))));
        
        js.Add(() => Js.Func(() => "getDictJson", () => new List<string> { }, () => getDictJsonBlock));

        var parseTimespanBlock = Js.Block()
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueTimespanDays")).Prop(() => "value").Assign(() => Js.Str(() => "0")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueTimespanHours")).Prop(() => "value").Assign(() => Js.Str(() => "0")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueTimespanMinutes")).Prop(() => "value").Assign(() => Js.Str(() => "0")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueTimespanSeconds")).Prop(() => "value").Assign(() => Js.Str(() => "0")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "str"), new List<JsSyntax>
                {
                    Js.Const(() => "parts", () => Js.Id(() => "str").Call(() => "split", () => Js.Str(() => "."))),
                    Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        { (Js.Id(() => "parts").Prop(() => "length").Op(() => "===", () => Js.Num(() => "2")), new List<JsSyntax>
                        {
                            Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueTimespanDays")).Prop(() => "value").Assign(() => Js.Id(() => "parts").Index(() => Js.Num(() => "0"))),
                            Js.Const(() => "timeParts", () => Js.Id(() => "parts").Index(() => Js.Num(() => "1")).Call(() => "split", () => Js.Str(() => ":"))),
                            Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueTimespanHours")).Prop(() => "value").Assign(() => Js.Id(() => "timeParts").Index(() => Js.Num(() => "0")).Op(() => "||", () => Js.Str(() => "0"))),
                            Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueTimespanMinutes")).Prop(() => "value").Assign(() => Js.Id(() => "timeParts").Index(() => Js.Num(() => "1")).Op(() => "||", () => Js.Str(() => "0"))),
                            Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueTimespanSeconds")).Prop(() => "value").Assign(() => Js.Id(() => "timeParts").Index(() => Js.Num(() => "2")).Op(() => "||", () => Js.Str(() => "0")))
                        })
                    }}),
                    Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        { (Js.Id(() => "parts").Prop(() => "length").Op(() => "===", () => Js.Num(() => "1")), new List<JsSyntax>
                        {
                            Js.Const(() => "timeParts", () => Js.Id(() => "parts").Index(() => Js.Num(() => "0")).Call(() => "split", () => Js.Str(() => ":"))),
                            Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueTimespanHours")).Prop(() => "value").Assign(() => Js.Id(() => "timeParts").Index(() => Js.Num(() => "0")).Op(() => "||", () => Js.Str(() => "0"))),
                            Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueTimespanMinutes")).Prop(() => "value").Assign(() => Js.Id(() => "timeParts").Index(() => Js.Num(() => "1")).Op(() => "||", () => Js.Str(() => "0"))),
                            Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueTimespanSeconds")).Prop(() => "value").Assign(() => Js.Id(() => "timeParts").Index(() => Js.Num(() => "2")).Op(() => "||", () => Js.Str(() => "0")))
                        })
                    }})
                })
            }}));
        js.Add(() => Js.Func(() => "parseTimespan", () => new List<string> { "str" }, () => parseTimespanBlock));

        var formatTimespanBlock = Js.Block()
            .Add(() => Js.Const(() => "days", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueTimespanDays")).Prop(() => "value").Op(() => "||", () => Js.Str(() => "0"))))
            .Add(() => Js.Const(() => "hours", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueTimespanHours")).Prop(() => "value").Op(() => "||", () => Js.Str(() => "0"))))
            .Add(() => Js.Const(() => "minutes", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueTimespanMinutes")).Prop(() => "value").Op(() => "||", () => Js.Str(() => "0"))))
            .Add(() => Js.Const(() => "seconds", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueTimespanSeconds")).Prop(() => "value").Op(() => "||", () => Js.Str(() => "0"))))
            .Add(() => Js.Return(() => Js.Str(() => "").Op(() => "+", () => Js.Id(() => "days")).Op(() => "+", () => Js.Str(() => ".")).Op(() => "+", () => Js.Id(() => "hours")).Op(() => "+", () => Js.Str(() => ":")).Op(() => "+", () => Js.Id(() => "minutes")).Op(() => "+", () => Js.Str(() => ":")).Op(() => "+", () => Js.Id(() => "seconds"))));
        js.Add(() => Js.Func(() => "formatTimespan", () => new List<string> { }, () => formatTimespanBlock));

        var openModalBlock = Js.Block()
            .Add(() => Js.Id(() => "hideAllInputs").Invoke().Stmt())
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editKey")).Prop(() => "value").Assign(() => Js.Id(() => "key")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editModalTitle")).Prop(() => "textContent").Assign(() => Js.Str(() => loc.ConfigEditPrefix).Op(() => "+", () => Js.Id(() => "display"))))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editModal")).Prop(() => "classList").Call(() => "add", () => Js.Str(() => "show")));
        
        var typeSwitchBlock = Js.Block()
            .Add(() => Js.Switch(() => Js.Id(() => "type"), () => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Str(() => "string"), new List<JsSyntax>
                    {
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputString")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "block")),
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValue")).Prop(() => "value").Assign(() => Js.Id(() => "value")),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "int"), new List<JsSyntax>
                    {
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputNumber")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "block")),
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueNumber")).Prop(() => "value").Assign(() => Js.Id(() => "value")),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "long"), new List<JsSyntax>
                    {
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputNumber")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "block")),
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueNumber")).Prop(() => "value").Assign(() => Js.Id(() => "value")),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "double"), new List<JsSyntax>
                    {
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputNumber")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "block")),
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueNumber")).Prop(() => "value").Assign(() => Js.Id(() => "value")),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "bool"), new List<JsSyntax>
                    {
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputBool")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "block")),
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueBool")).Prop(() => "checked").Assign(() => Js.Id(() => "value").Op(() => "===", () => Js.Str(() => "true"))),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "directory"), new List<JsSyntax>
                    {
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputDirectory")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "block")),
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueDirectory")).Prop(() => "value").Assign(() => Js.Id(() => "value")),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "datetime"), new List<JsSyntax>
                    {
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputDatetime")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "block")),
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueDatetime")).Prop(() => "value").Assign(() => Js.Id(() => "value")),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "timespan"), new List<JsSyntax>
                    {
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputTimespan")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "block")),
                        Js.Id(() => "parseTimespan").Invoke(() => Js.Id(() => "value")),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "enum"), new List<JsSyntax>
                    {
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputEnum")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "block")).Stmt(),
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueEnum")).Prop(() => "innerHTML").Assign(() => Js.Str(() => "")).Stmt(),
                        Js.Id(() => "enumValues").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "v", "i" }, () => Js.Block()
                            .Add(() => Js.Const(() => "opt", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "option"))))
                            .Add(() => Js.Id(() => "opt").Prop(() => "value").Assign(() => Js.Id(() => "v")))
                            .Add(() => Js.Id(() => "opt").Prop(() => "textContent").Assign(() => Js.Id(() => "enumDisplayNames").Index(() => Js.Id(() => "i"))))
                            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueEnum")).Call(() => "appendChild", () => Js.Id(() => "opt")))
                        )).Stmt(),
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueEnum")).Prop(() => "value").Assign(() => Js.Id(() => "value")).Stmt(),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "dictionary"), new List<JsSyntax>
                    {
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputDictionary")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "block")),
                        Js.Assign(() => Js.Id(() => "isAIConfigDict"), () => Js.Id(() => "key").Op(() => "===", () => Js.Str(() => "AIConfig"))),
                        Js.Id(() => "initDictEditor").Invoke(() => Js.Id(() => "value")),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "guid"), new List<JsSyntax>
                    {
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputString")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "block")),
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValue")).Prop(() => "value").Assign(() => Js.Id(() => "value")),
                        Js.Break()
                    })
                },
                { (null, new List<JsSyntax>
                    {
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputString")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "block")),
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValue")).Prop(() => "value").Assign(() => Js.Id(() => "value")),
                        Js.Break()
                    })
                }
            }));
        
        openModalBlock.Add(() => typeSwitchBlock);
        js.Add(() => Js.Func(() => "openModal", () => new List<string> { "key", "value", "display", "type", "enumValues", "enumDisplayNames" }, () => openModalBlock));

        var closeModalBlock = Js.Block()
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editModal")).Prop(() => "classList").Call(() => "remove", () => Js.Str(() => "show")));
        js.Add(() => Js.Func(() => "closeModal", () => new List<string> { }, () => closeModalBlock));

        var getValueBlock = Js.Block();
        getValueBlock.Add(() => Js.Let(() => "value", () => Js.Str(() => "")));
        
        var getValueSwitchBlock = Js.Block()
            .Add(() => Js.Switch(() => Js.Id(() => "currentType"), () => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Str(() => "string"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "value"), () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValue")).Prop(() => "value").Call(() => "trim")),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "int"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "value"), () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueNumber")).Prop(() => "value")),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "long"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "value"), () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueNumber")).Prop(() => "value")),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "double"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "value"), () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueNumber")).Prop(() => "value")),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "bool"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "value"), () => Js.Ternary(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueBool")).Prop(() => "checked"), () => Js.Str(() => "true"), () => Js.Str(() => "false"))),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "directory"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "value"), () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueDirectory")).Prop(() => "value")),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "datetime"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "value"), () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueDatetime")).Prop(() => "value")),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "timespan"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "value"), () => Js.Id(() => "formatTimespan").Invoke()),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "enum"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "value"), () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueEnum")).Prop(() => "value")),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "dictionary"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "value"), () => Js.Id(() => "getDictJson").Invoke()),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "guid"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "value"), () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValue")).Prop(() => "value").Call(() => "trim")),
                        Js.Break()
                    })
                },
                { (null, new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "value"), () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValue")).Prop(() => "value").Call(() => "trim")),
                        Js.Break()
                    })
                }
            }));
        
        var jsonBody = Js.Obj()
            .Prop(() => "key", () => Js.Id(() => "key"))
            .Prop(() => "value", () => Js.Id(() => "value"));
        
        var fetchOptions = Js.Obj()
            .Prop(() => "method", () => Js.Str(() => "POST"))
            .Prop(() => "headers", () => Js.Obj()
                .Prop(() => "Content-Type", () => Js.Str(() => "application/json")))
            .Prop(() => "body", () => Js.Id(() => "JSON").Call(() => "stringify", () => jsonBody));
        
        var saveThenBlock = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "data").Prop(() => "success"), new List<JsSyntax>
                    {
                        Js.Id(() => "closeModal").Invoke().Stmt(),
                        Js.Id(() => "location").Prop(() => "reload").Invoke().Stmt()
                    })
                },
                { (null, new List<JsSyntax>
                    {
                        Js.Id(() => "alert").Invoke(() => Js.Str(() => loc.ConfigSaveFailed).Op(() => "+", () => Js.Id(() => "data").Prop(() => "message"))).Stmt()
                    })
                }
            }));
        
        var saveConfigBlock = Js.Block()
            .Add(() => Js.Const(() => "key", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editKey")).Prop(() => "value")))
            .Add(() => getValueSwitchBlock);
        
        var fetchThenBlock = Js.Block()
            .Add(() => Js.Id(() => "response").Call(() => "json").Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => saveThenBlock)));
        
        saveConfigBlock.Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/config/save"), () => fetchOptions).Call(() => "then", () => Js.Arrow(() => new List<string> { "response" }, () => fetchThenBlock)));
        js.Add(() => Js.Func(() => "saveConfig", () => new List<string> { }, () => saveConfigBlock));

        var initBlock = Js.Block();

        var btnClickBlock = Js.Block()
            .Add(() => Js.Const(() => "key", () => Js.Id(() => "btn").Prop(() => "dataset").Prop(() => "key")))
            .Add(() => Js.Const(() => "display", () => Js.Id(() => "btn").Prop(() => "dataset").Prop(() => "display")))
            .Add(() => Js.Const(() => "type", () => Js.Id(() => "btn").Prop(() => "dataset").Prop(() => "type")))
            .Add(() => Js.Assign(() => Js.Id(() => "currentType"), () => Js.Id(() => "type")))
            .Add(() => Js.Const(() => "valueCell", () => Js.Id(() => "btn").Prop(() => "closest").Invoke(() => Js.Str(() => "tr")).Call(() => "querySelector", () => Js.Str(() => ".config-value"))))
            .Add(() => Js.Const(() => "value", () => Js.Id(() => "valueCell").Prop(() => "textContent").Call(() => "trim")))
            .Add(() => Js.Const(() => "enumValuesStr", () => Js.Id(() => "btn").Prop(() => "dataset").Prop(() => "enumValues")))
            .Add(() => Js.Const(() => "enumValues", () => Js.Ternary(() => Js.Id(() => "enumValuesStr"), () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "enumValuesStr")), () => Js.Array())))
            .Add(() => Js.Const(() => "enumDisplayNamesStr", () => Js.Id(() => "btn").Prop(() => "dataset").Prop(() => "enumDisplayNames")))
            .Add(() => Js.Const(() => "enumDisplayNames", () => Js.Ternary(() => Js.Id(() => "enumDisplayNamesStr"), () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "enumDisplayNamesStr")), () => Js.Array())))
            .Add(() => Js.Id(() => "openModal").Invoke(() => Js.Id(() => "key"), () => Js.Id(() => "value"), () => Js.Id(() => "display"), () => Js.Id(() => "type"), () => Js.Id(() => "enumValues"), () => Js.Id(() => "enumDisplayNames")).Stmt());
        
        var forEachArrow = Js.Arrow(() => new List<string> { "btn" }, () => Js.Block()
            .Add(() => Js.Id(() => "btn").Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string> { }, () => btnClickBlock))));
        
        initBlock
            .Add(() => Js.Id(() => "document").Call(() => "querySelectorAll", () => Js.Str(() => ".btn-edit")).Call(() => "forEach", () => forEachArrow))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "btnSave")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string> { }, () => Js.Id(() => "saveConfig").Invoke().Stmt())))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "btnCancel")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string> { }, () => Js.Id(() => "closeModal").Invoke().Stmt())))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "btnAddDictRow")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string> { }, () => Js.Id(() => "addDictRow").Invoke(() => Js.Str(() => ""), () => Js.Str(() => "")).Stmt())));
        
        var modalClickBlock = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "e").Prop(() => "target").Prop(() => "id").Op(() => "===", () => Js.Str(() => "editModal")), new List<JsSyntax>
                    {
                        Js.Id(() => "closeModal").Invoke().Stmt()
                    })
                }
            }));
        
        initBlock.Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editModal")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string> { "e" }, () => modalClickBlock)));

        var escapeHtml = Js.Id(() => "s")
            .Call(() => "replace", () => Js.Regex(() => @"\&", () => "g"), () => Js.Str(() => "&amp;"))
            .Call(() => "replace", () => Js.Regex(() => "\"", () => "g"), () => Js.Str(() => "&quot;"))
            .Call(() => "replace", () => Js.Regex(() => "<", () => "g"), () => Js.Str(() => "&lt;"))
            .Call(() => "replace", () => Js.Regex(() => ">", () => "g"), () => Js.Str(() => "&gt;"));

        var browseDirBody = Js.Block()
            .Add(() => Js.Const(() => "url", () => Js.Str(() => "/init/browse?dir=").Op(() => "+", () => (JsSyntax)Js.Id(() => "encodeURIComponent").Invoke(() => Js.Id(() => "path")))))
            .Add(() => Js.Const(() => "db", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dirBrowser"))))
            .Add(() => Js.Const(() => "h", () => Js.Arrow(() => new List<string> { "s" }, () => escapeHtml)));

        var forEachBlock = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Call(() => "concat", () => Js.Str(() => "<div class=\"dir-item")).Call(() => "concat", () => Js.Ternary(() => Js.Id(() => "d").Prop(() => "isParent"), () => Js.Str(() => " dir-parent"), () => Js.Str(() => ""))).Call(() => "concat", () => Js.Str(() => "\" onclick=\"")).Call(() => "concat", () => Js.Ternary(() => Js.Id(() => "d").Prop(() => "isParent"), () => Js.Str(() => "browseDir('"), () => Js.Str(() => "selectDir('"))).Call(() => "concat", () => Js.Id(() => "d").Prop(() => "path")).Call(() => "concat", () => Js.Str(() => "')\">"))))
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Call(() => "concat", () => Js.Str(() => "<span class=\"dir-icon\">")).Call(() => "concat", () => Js.Ternary(() => Js.Id(() => "d").Prop(() => "isParent"), () => Js.Str(() => "📁"), () => Js.Str(() => "📂"))).Call(() => "concat", () => Js.Str(() => "</span>"))))
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Call(() => "concat", () => Js.Id(() => "h").Invoke(() => Js.Id(() => "d").Prop(() => "name"))).Call(() => "concat", () => Js.Str(() => "</div>"))));

        var innerBlock = Js.Block()
            .Add(() => Js.Let(() => "html", () => Js.Str(() => "<div class=\"dir-header\"><span class=\"dir-current\">")))
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Call(() => "concat", () => Js.Id(() => "h").Invoke(() => Js.Id(() => "data").Prop(() => "currentPath")))))
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Call(() => "concat", () => Js.Str(() => "</span></div><div class=\"dir-list\">"))))
            .Add(() => Js.Id(() => "data").Prop(() => "directories").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "d" }, () => forEachBlock)))
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Call(() => "concat", () => Js.Str(() => "</div>"))))
            .Add(() => Js.Assign(() => Js.Id(() => "db").Prop(() => "innerHTML"), () => Js.Id(() => "html")));

        var dataArrow = Js.Arrow(() => new List<string> { "data" }, () => innerBlock);
        var responseBlock = Js.Block()
            .Add(() => Js.Id(() => "response").Call(() => "json").Call(() => "then", () => dataArrow));
        var fetchThenArrow = Js.Arrow(() => new List<string> { "response" }, () => responseBlock);

        browseDirBody.Add(() => Js.Id(() => "fetch").Invoke(() => Js.Id(() => "url")).Call(() => "then", () => fetchThenArrow));
        js.Add(() => Js.Func(() => "browseDir", () => new List<string> { "path" }, () => browseDirBody));

        var openDirBrowserBody = Js.Block()
            .Add(() => Js.Const(() => "db", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dirBrowser"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "db").Prop(() => "style").Prop(() => "display").Op(() => "===", () => Js.Str(() => "block")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "db").Prop(() => "style").Prop(() => "display"), () => Js.Str(() => "none")),
                        Js.Return(() => Js.Str(() => ""))
                    })
                }
            }))
            .Add(() => Js.Assign(() => Js.Id(() => "db").Prop(() => "style").Prop(() => "display"), () => Js.Str(() => "block")))
            .Add(() => Js.Const(() => "input", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueDirectory"))))
            .Add(() => Js.Id(() => "browseDir").Invoke(() => Js.Id(() => "input").Prop(() => "value").Op(() => "||", () => (JsSyntax)Js.Str(() => "/"))).Stmt());
        js.Add(() => Js.Func(() => "openDirBrowser", () => new List<string>(), () => openDirBrowserBody));

        var selectDirBody = Js.Block()
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueDirectory")).Prop(() => "value").Assign(() => Js.Id(() => "path")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dirBrowser")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "none")));
        js.Add(() => Js.Func(() => "selectDir", () => new List<string> { "path" }, () => selectDirBody));

        var browseDirBlock = Js.Block()
            .Add(() => Js.Id(() => "openDirBrowser").Invoke().Stmt());
        initBlock.Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "btnBrowseDir")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string> { }, () => browseDirBlock)));

        var updateAIClientConfigBlock = Js.Block()
            .Add(() => Js.Const(() => "selectedType", () => Js.Id(() => "document").Call(() => "querySelector", () => Js.Str(() => "[data-key=\"AIClientType\"]")).Prop(() => "textContent").Call(() => "trim")))
            .Add(() => Js.Id(() => "document").Call(() => "querySelectorAll", () => Js.Str(() => "[data-depends-on=\"AIClientType\"]")).Call(() => "forEach", () => Js.Arrow(() => new List<string> { "row" }, () => Js.Block()
                .Add(() => Js.Const(() => "expectedValue", () => Js.Id(() => "row").Prop(() => "dataset").Prop(() => "dependsOnValue")))
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    { (Js.Id(() => "expectedValue").Op(() => "===", () => Js.Id(() => "selectedType")), new List<JsSyntax>
                        {
                            Js.Assign(() => Js.Id(() => "row").Prop(() => "style").Prop(() => "display"), () => Js.Str(() => ""))
                        })
                    },
                    { (null, new List<JsSyntax>
                        {
                            Js.Assign(() => Js.Id(() => "row").Prop(() => "style").Prop(() => "display"), () => Js.Str(() => "none"))
                        })
                    }
                })))));

        initBlock.Add(() => Js.Id(() => "updateAIClientConfig").Invoke().Stmt());

        js.Add(() => Js.Func(() => "updateAIClientConfig", () => new List<string>(), () => updateAIClientConfigBlock));

        js.Add(() => Js.Id(() => "addEventListener").Invoke(() => Js.Str(() => "DOMContentLoaded"), () => Js.Arrow(() => new List<string> { }, () => initBlock)).Stmt());

        return js;
    }
}
