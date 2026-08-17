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

using SiliconLife.App.Web.Component;
using SiliconLife.App.Web.Models;

using SiliconLife.Common.Localization;

namespace SiliconLife.App.Web.Views;

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
            H.Create("div").Add(SelectComponent.GetSearchableGlobalScript()).Style(new CssBuilder().InlineProperty("display", "none")),
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
                        ).Class("form-group").Id("inputNumber").Style(new CssBuilder().InlineProperty("display", "none")),
                        H.Div(
                            H.Label(loc.ConfigEditValueLabel).Attr("for", "editValueBool"),
                            H.Input().Attr("type", "checkbox").Id("editValueBool")
                        ).Class("form-group").Id("inputBool").Style(new CssBuilder().InlineProperty("display", "none")),
                        H.Div(
                            H.Label(loc.ConfigEditValueLabel).Attr("for", "editValueDirectory"),
                            H.Div(
                                H.Input().Attr("type", "text").Id("editValueDirectory").Attr("readonly", "readonly"),
                                H.Button(loc.ConfigBrowseButton).Class("btn btn-sm").Id("btnBrowseDir")
                            ).Class("input-with-btn"),
                            H.Div().Id("dirBrowser").Class("dir-browser").Style(new CssBuilder().InlineProperty("display", "none"))
                        ).Class("form-group").Id("inputDirectory").Style(new CssBuilder().InlineProperty("display", "none")),
                        H.Div(
                            H.Label(loc.ConfigEditValueLabel).Attr("for", "editValueDatetime"),
                            H.Input().Attr("type", "datetime-local").Id("editValueDatetime")
                        ).Class("form-group").Id("inputDatetime").Style(new CssBuilder().InlineProperty("display", "none")),
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
                        ).Class("form-group").Id("inputTimespan").Style(new CssBuilder().InlineProperty("display", "none")),
                        H.Div(
                            H.Label(loc.ConfigEditValueLabel).Attr("for", "editValueEnumContainer"),
                            H.Div().Id("editValueEnumContainer")
                        ).Class("form-group").Id("inputEnum").Style(new CssBuilder().InlineProperty("display", "none")),
                        H.Div(
                            H.Label(loc.ConfigDictionaryLabel),
                            H.Div().Id("dictEditorContainer").Class("dict-editor"),
                            H.Div(
                                H.Button(loc.ConfigDictAddButton).Class("btn btn-add").Id("btnAddDictRow")
                            ).Class("dict-actions")
                        ).Class("form-group").Id("inputDictionary").Style(new CssBuilder().InlineProperty("display", "none")),
                        H.Div(
                            H.Label(loc.ConfigPluginDirectoriesLabel),
                            H.Div().Id("dirListEditorContainer").Class("dir-list-editor"),
                            H.Div(
                                H.Button(loc.ConfigPluginDirAddButton).Class("btn btn-add").Id("btnAddDirListRow")
                            ).Class("dir-list-actions")
                        ).Class("form-group").Id("inputDirectoryList").Style(new CssBuilder().InlineProperty("display", "none")),
                        H.Div(
                            H.Label(loc.GetConfigDisplayName("IMPlatforms", out _)),
                            H.Div().Id("imPlatformListContainer").Class("im-platform-list-editor"),
                            H.Div(
                                H.Button(loc.GetConfigDisplayName("IMAddPlatform", out _)).Class("btn btn-add").Id("btnAddIMPlatformRow")
                            ).Class("dict-actions")
                        ).Class("form-group").Id("inputIMPlatformList").Style(new CssBuilder().InlineProperty("display", "none")),
                        H.Div(
                            H.Label(loc.GetConfigDisplayName("McpServers", out _)),
                            H.Div().Id("mcpServerListContainer").Class("im-platform-list-editor"),
                            H.Div(
                                H.Button(loc.McpBtnAddServer).Class("btn btn-add").Id("btnAddMcpServerRow")
                            ).Class("dict-actions")
                        ).Class("form-group").Id("inputMcpServerList").Style(new CssBuilder().InlineProperty("display", "none")),
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
            .EndSelector()
            .Selector(".dir-list-editor")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "4px")
                .Property("padding", "12px")
                .Property("background", "var(--bg-primary)")
                .Property("max-height", "250px")
                .Property("overflow-y", "auto")
            .EndSelector()
            .Selector(".dir-list-row")
                .Property("display", "flex")
                .Property("gap", "8px")
                .Property("align-items", "center")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".dir-list-path")
                .Property("flex", "1")
                .Property("padding", "6px 10px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "4px")
                .Property("background", "var(--bg-tertiary)")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".btn-dir-list-browse")
                .Property("padding", "6px 12px")
                .Property("background", "var(--bg-tertiary)")
                .Property("color", "var(--text-primary)")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "4px")
                .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".dir-list-actions")
                .Property("margin-top", "8px")
                .Property("display", "flex")
                .Property("justify-content", "flex-end")
            .EndSelector()
            .Selector(".dir-list-popup")
                .Property("margin-top", "4px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "4px")
                .Property("background", "var(--bg-tertiary)")
                .Property("max-height", "240px")
                .Property("overflow-y", "auto")
            .EndSelector()
            .Selector(".dir-list-footer")
                .Property("display", "flex")
                .Property("gap", "8px")
                .Property("padding", "8px 12px")
                .Property("border-top", "1px solid var(--border-color)")
                .Property("justify-content", "flex-end")
            .EndSelector()
            .Selector(".btn-dir-list-confirm")
                .Property("padding", "4px 12px")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#fff")
                .Property("border", "none")
                .Property("border-radius", "4px")
                .Property("cursor", "pointer")
                .Property("font-size", "12px")
            .EndSelector()
            .Selector(".btn-dir-list-confirm:hover")
                .Property("opacity", "0.9")
            .EndSelector()
            .Selector(".btn-dir-list-cancel")
                .Property("padding", "4px 12px")
                .Property("background", "var(--bg-secondary)")
                .Property("color", "var(--text-secondary)")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "4px")
                .Property("cursor", "pointer")
                .Property("font-size", "12px")
            .EndSelector()
            .Selector(".dir-loading")
                .Property("padding", "12px")
                .Property("text-align", "center")
                .Property("color", "var(--text-muted)")
                .Property("font-size", "13px")
            .EndSelector()
            .Selector(".im-platform-list-editor")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "4px")
                .Property("padding", "12px")
                .Property("background", "var(--bg-primary)")
                .Property("max-height", "400px")
                .Property("overflow-y", "auto")
            .EndSelector()
            .Selector(".im-platform-item")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "4px")
                .Property("padding", "12px")
                .Property("margin-bottom", "10px")
                .Property("background", "var(--bg-tertiary)")
            .EndSelector()
            .Selector(".im-platform-item-header")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "8px")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".im-platform-item-header label")
                .Property("font-size", "13px")
                .Property("color", "var(--text-secondary)")
                .Property("margin", "0")
            .EndSelector()
            .Selector(".im-platform-item-content")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "8px")
            .EndSelector()
            .Selector(".im-platform-row")
                .Property("display", "flex")
                .Property("gap", "8px")
                .Property("align-items", "center")
            .EndSelector()
            .Selector(".im-platform-row label")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
                .Property("min-width", "80px")
            .EndSelector()
            .Selector(".im-platform-row select, .im-platform-row input")
                .Property("flex", "1")
                .Property("padding", "6px 10px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "4px")
                .Property("background", "var(--bg-primary)")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".im-platform-config-fields")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "8px")
                .Property("margin-top", "8px")
            .EndSelector()
            .Selector(".im-platform-field-row")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "4px")
            .EndSelector()
            .Selector(".im-platform-field-row label")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".im-platform-field-row input")
                .Property("padding", "6px 10px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "4px")
                .Property("background", "var(--bg-primary)")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".im-platform-enabled")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "6px")
            .EndSelector()
            .Selector(".btn-im-platform-delete")
                .Property("padding", "4px 10px")
                .Property("background", "#dc3545")
                .Property("color", "#fff")
                .Property("border", "none")
                .Property("border-radius", "4px")
                .Property("cursor", "pointer")
                .Property("font-size", "12px")
            .EndSelector()
            .Selector(".btn-im-platform-delete:hover")
                .Property("background", "#c82333")
            .EndSelector()
            .Selector(".im-auth-section")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "8px")
                .Property("margin-top", "4px")
            .EndSelector()
            .Selector(".im-oauth-panel")
                .Property("display", "none")
                .Property("flex-direction", "column")
                .Property("gap", "8px")
            .EndSelector()
            .Selector(".im-oauth-hint")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
                .Property("font-style", "italic")
            .EndSelector()
            .Selector(".im-oauth-action-row")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "8px")
            .EndSelector()
            .Selector(".btn-im-scan-auth")
                .Property("padding", "6px 16px")
                .Property("background", "var(--accent-color)")
                .Property("color", "#fff")
                .Property("border", "none")
                .Property("border-radius", "4px")
                .Property("cursor", "pointer")
                .Property("font-size", "12px")
            .EndSelector()
            .Selector(".btn-im-scan-auth:hover")
                .Property("opacity", "0.9")
            .EndSelector()
            .Selector(".im-auth-status")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".im-platform-empty")
                .Property("text-align", "center")
                .Property("color", "var(--text-secondary)")
                .Property("padding", "20px")
                .Property("font-style", "italic")
            .EndSelector()
            .Selector(".im-help-details")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "4px")
                .Property("padding", "6px 10px")
                .Property("background", "var(--bg-primary)")
            .EndSelector()
            .Selector(".im-help-details summary")
                .Property("cursor", "pointer")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".im-help-text")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-top", "6px")
                .Property("line-height", "1.6")
                .Property("white-space", "pre-wrap")
            .EndSelector()
            .Selector(".im-help-link")
                .Property("display", "inline-block")
                .Property("font-size", "12px")
                .Property("color", "var(--accent-color)")
                .Property("margin-top", "6px")
            .EndSelector();
    }

    /// <summary>
    /// 解析 IM 平台接入帮助文案：HelpKey 为空或当前语言未注册该键时返回空串（前端据此隐藏帮助区）。
    /// </summary>
    private static string ResolveIMHelpText(DefaultLocalizationBase loc, string? helpKey)
    {
        if (string.IsNullOrEmpty(helpKey))
            return string.Empty;
        var text = loc.GetConfigDisplayName(helpKey, out var found);
        return found ? text : string.Empty;
    }

    private static JsSyntax GetScripts(DefaultLocalizationBase loc)
    {
        var js = Js.Block();

        // IM 编辑器文案（经 ConfigDisplayNames 本地化契约键获取）
        var locIMPlatformType = loc.GetConfigDisplayName("IMPlatformType", out _);
        var locIMEnabled = loc.GetConfigDisplayName("IMEnabledLabel", out _);
        var locIMDelete = loc.GetConfigDisplayName("IMDeleteLabel", out _);
        var locIMAuthMode = loc.GetConfigDisplayName("IMAuthModeLabel", out _);
        var locIMManualMode = loc.GetConfigDisplayName("IMManualMode", out _);
        var locIMScanMode = loc.GetConfigDisplayName("IMScanMode", out _);
        var locIMScanBtn = loc.GetConfigDisplayName("IMScanAuthorizeBtn", out _);
        var locIMWaiting = loc.GetConfigDisplayName("IMWaitingAuth", out _);
        var locIMAuthorized = loc.GetConfigDisplayName("IMAuthorizedStatus", out _);
        var locIMAuthFailed = loc.GetConfigDisplayName("IMAuthFailedStatus", out _);
        var locIMAuthTimeout = loc.GetConfigDisplayName("IMAuthTimeoutStatus", out _);
        var locIMRedirectBase = loc.GetConfigDisplayName("IMRedirectBaseUrlLabel", out _);
        var locIMCallbackHint = loc.GetConfigDisplayName("IMPublicCallbackHint", out _);
        var locIMHelpTitle = loc.GetConfigDisplayName("IMHelpTitle", out _);
        var locIMHelpOfficialDoc = loc.GetConfigDisplayName("IMHelpOfficialDoc", out _);

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
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputDictionary")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "none")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputDirectoryList")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "none")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputIMPlatformList")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "none")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputMcpServerList")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "none")));
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
                        Js.Const(() => "dictValWrapper", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))).Stmt(),
                        Js.Id(() => "dictValWrapper").Prop(() => "style").Prop(() => "flex").Assign(() => Js.Str(() => "1")).Stmt(),
                        Js.Id(() => "valueInput").Call(() => "replaceWith", () => Js.Id(() => "dictValWrapper")).Stmt(),
                        Js.Id(() => "slSelectSearch_create").Invoke(() => Js.Id(() => "dictValWrapper"), () => Js.Obj()
                            .Prop(() => "value", () => Js.Id(() => "valueInput").Prop(() => "value"))
                            .Prop(() => "options", () => Js.Id(() => "d").Prop(() => "options"))
                        ).Stmt()
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

        // Directory list editor functions (for PluginDirectories)
        var initDirListEditorBlock = Js.Block()
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dirListEditorContainer")).Prop(() => "innerHTML").Assign(() => Js.Str(() => "")))
            .Add(() => Js.Let(() => "dirs", () => Js.Ternary(() => Js.Id(() => "jsonStr"), () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "jsonStr")), () => Js.Array())));
        var initDirListForEachBlock = Js.Block()
            .Add(() => Js.Id(() => "addDirListRow").Invoke(() => Js.Id(() => "d")));
        initDirListEditorBlock.Add(() => Js.Id(() => "dirs").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "d" }, () => initDirListForEachBlock)));
        js.Add(() => Js.Func(() => "initDirListEditor", () => new List<string> { "jsonStr" }, () => initDirListEditorBlock));

        var addDirListRowBlock = Js.Block()
            .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dirListEditorContainer"))))
            .Add(() => Js.Const(() => "row", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))));
        addDirListRowBlock.Add(() => Js.Id(() => "row").Prop(() => "className").Assign(() => Js.Str(() => "dir-list-row")));
        addDirListRowBlock.Add(() => Js.Const(() => "pathInput", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "input"))))
            .Add(() => Js.Id(() => "pathInput").Prop(() => "type").Assign(() => Js.Str(() => "text")))
            .Add(() => Js.Id(() => "pathInput").Prop(() => "value").Assign(() => Js.Id(() => "path")))
            .Add(() => Js.Id(() => "pathInput").Prop(() => "readonly").Assign(() => Js.Str(() => "true")))
            .Add(() => Js.Id(() => "pathInput").Prop(() => "className").Assign(() => Js.Str(() => "dir-list-path")));
        addDirListRowBlock.Add(() => Js.Const(() => "browseBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Id(() => "browseBtn").Prop(() => "textContent").Assign(() => Js.Str(() => loc.ConfigBrowseButton)))
            .Add(() => Js.Id(() => "browseBtn").Prop(() => "className").Assign(() => Js.Str(() => "btn btn-sm btn-dir-list-browse")))
            .Add(() => Js.Id(() => "browseBtn").Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string> { }, () => Js.Id(() => "browseDirListDir").Invoke(() => Js.Id(() => "pathInput")).Stmt())));
        addDirListRowBlock.Add(() => Js.Const(() => "deleteBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Id(() => "deleteBtn").Prop(() => "textContent").Assign(() => Js.Str(() => loc.ConfigDictDeleteButton)))
            .Add(() => Js.Id(() => "deleteBtn").Prop(() => "className").Assign(() => Js.Str(() => "btn-delete")))
            .Add(() => Js.Id(() => "deleteBtn").Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string> { }, () => Js.Id(() => "row").Prop(() => "remove").Invoke().Stmt())));
        addDirListRowBlock.Add(() => Js.Id(() => "row").Call(() => "appendChild", () => Js.Id(() => "pathInput")))
            .Add(() => Js.Id(() => "row").Call(() => "appendChild", () => Js.Id(() => "browseBtn")))
            .Add(() => Js.Id(() => "row").Call(() => "appendChild", () => Js.Id(() => "deleteBtn")))
            .Add(() => Js.Id(() => "container").Call(() => "appendChild", () => Js.Id(() => "row")));
        js.Add(() => Js.Func(() => "addDirListRow", () => new List<string> { "path" }, () => addDirListRowBlock));

        var getDirListJsonBlock = Js.Block()
            .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dirListEditorContainer"))))
            .Add(() => Js.Const(() => "rows", () => Js.Id(() => "container").Call(() => "querySelectorAll", () => Js.Str(() => ".dir-list-row"))))
            .Add(() => Js.Const(() => "dirs", () => Js.Array()));
        var processDirListRowBlock = Js.Block()
            .Add(() => Js.Const(() => "input", () => Js.Id(() => "row").Call(() => "querySelector", () => Js.Str(() => ".dir-list-path"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "input").Prop(() => "value"), new List<JsSyntax>
                    {
                        Js.Id(() => "dirs").Call(() => "push", () => Js.Id(() => "input").Prop(() => "value"))
                    })
                }
            }));
        getDirListJsonBlock.Add(() => Js.Id(() => "rows").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "row" }, () => processDirListRowBlock)));
        getDirListJsonBlock.Add(() => Js.Return(() => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Id(() => "dirs"))));
        js.Add(() => Js.Func(() => "getDirListJson", () => new List<string> { }, () => getDirListJsonBlock));

        // --- Directory list browser state ---
        js.Add(() => Js.Let(() => "currentDirListPathInput", () => Js.Null()));
        js.Add(() => Js.Let(() => "dirListCurrentPath", () => Js.Str(() => "/")));

        // escapeHtml helper (reused)
        var dlEscapeHtml = Js.Id(() => "s")
            .Call(() => "replace", () => Js.Regex(() => @"\&", () => "g"), () => Js.Str(() => "&amp;"))
            .Call(() => "replace", () => Js.Regex(() => "\"", () => "g"), () => Js.Str(() => "&quot;"))
            .Call(() => "replace", () => Js.Regex(() => "<", () => "g"), () => Js.Str(() => "&lt;"))
            .Call(() => "replace", () => Js.Regex(() => ">", () => "g"), () => Js.Str(() => "&gt;"));

        // navigateDirList(path) — fetch directory listing and render into popup
        var navDirListForEachBlock = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Call(() => "concat", () => Js.Str(() => "<div class=\"dir-item")).Call(() => "concat", () => Js.Ternary(() => Js.Id(() => "d").Prop(() => "isParent"), () => Js.Str(() => " dir-parent"), () => Js.Str(() => ""))).Call(() => "concat", () => Js.Str(() => "\" data-path=\"")).Call(() => "concat", () => Js.Id(() => "h").Invoke(() => Js.Id(() => "d").Prop(() => "path"))).Call(() => "concat", () => Js.Str(() => "\">"))))
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Call(() => "concat", () => Js.Str(() => "<span class=\"dir-icon\">")).Call(() => "concat", () => Js.Ternary(() => Js.Id(() => "d").Prop(() => "isParent"), () => Js.Str(() => "📁"), () => Js.Str(() => "📂"))).Call(() => "concat", () => Js.Str(() => "</span>"))))
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Call(() => "concat", () => Js.Id(() => "h").Invoke(() => Js.Id(() => "d").Prop(() => "name"))).Call(() => "concat", () => Js.Str(() => "</div>"))));
        var navDirListInnerBlock = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "dirListCurrentPath"), () => Js.Id(() => "data").Prop(() => "currentPath")))
            .Add(() => Js.Const(() => "h", () => Js.Arrow(() => new List<string> { "s" }, () => dlEscapeHtml)))
            .Add(() => Js.Let(() => "html", () => Js.Str(() => "<div class=\"dir-header\"><span class=\"dir-current\">")))
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Call(() => "concat", () => Js.Id(() => "h").Invoke(() => Js.Id(() => "data").Prop(() => "currentPath")))))
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Call(() => "concat", () => Js.Str(() => "</span></div><div class=\"dir-list\">"))))
            .Add(() => Js.Id(() => "data").Prop(() => "directories").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "d" }, () => navDirListForEachBlock)))
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Call(() => "concat", () => Js.Str(() => "</div>"))))
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Call(() => "concat", () => Js.Str(() => "<div class=\"dir-list-footer\"><button class=\"btn btn-sm btn-dir-list-confirm\">")).Call(() => "concat", () => Js.Str(() => loc.ConfigSaveButton)).Call(() => "concat", () => Js.Str(() => "</button><button class=\"btn btn-sm btn-dir-list-cancel\">")).Call(() => "concat", () => Js.Str(() => loc.ConfigCancelButton)).Call(() => "concat", () => Js.Str(() => "</button></div>"))))
            .Add(() => Js.Id(() => "popup").Prop(() => "innerHTML").Assign(() => Js.Id(() => "html")));
        var navDirListBlock = Js.Block()
            .Add(() => Js.Const(() => "popup", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dirListPopup"))));
        navDirListBlock.Add(() => Js.Const(() => "url", () => Js.Str(() => "/config/browse?dir=").Op(() => "+", () => (JsSyntax)Js.Id(() => "encodeURIComponent").Invoke(() => Js.Id(() => "path")))));
        navDirListBlock.Add(() => Js.Id(() => "fetch").Invoke(() => Js.Id(() => "url")).Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"))).Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => navDirListInnerBlock)));
        js.Add(() => Js.Func(() => "navigateDirList", () => new List<string> { "path" }, () => navDirListBlock));

        // browseDirListDir(pathInput) — open popup and navigate to initial path
        var browseDirListDirBlock = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "currentDirListPathInput"), () => Js.Id(() => "pathInput")))
            .Add(() => Js.Const(() => "popup", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dirListPopup"))));
        browseDirListDirBlock.Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
        {
            { (Js.Id(() => "popup"), new List<JsSyntax> { Js.Id(() => "popup").Prop(() => "remove").Invoke().Stmt() }) }
        }));
        browseDirListDirBlock.Add(() => Js.Id(() => "pathInput").Prop(() => "parentElement").Call(() => "insertAdjacentHTML", () => Js.Str(() => "afterend"), () => Js.Str(() => "<div class=\"dir-list-popup\" id=\"dirListPopup\"><div class=\"dir-loading\">...</div></div>")));
        browseDirListDirBlock.Add(() => Js.Id(() => "navigateDirList").Invoke(() => Js.Id(() => "pathInput").Prop(() => "value").Op(() => "||", () => Js.Str(() => "/"))).Stmt());
        // Attach click event delegation on the popup after it's created
        browseDirListDirBlock.Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dirListPopup")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string> { "e" }, () => Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "e").Prop(() => "target").Call(() => "closest", () => Js.Str(() => ".dir-item")), new List<JsSyntax>
                    {
                        Js.Id(() => "navigateDirList").Invoke(() => Js.Id(() => "e").Prop(() => "target").Call(() => "closest", () => Js.Str(() => ".dir-item")).Prop(() => "dataset").Prop(() => "path")).Stmt()
                    })
                },
                { (Js.Id(() => "e").Prop(() => "target").Call(() => "closest", () => Js.Str(() => ".btn-dir-list-confirm")), new List<JsSyntax>
                    {
                        Js.Id(() => "confirmDirListDir").Invoke().Stmt()
                    })
                },
                { (Js.Id(() => "e").Prop(() => "target").Call(() => "closest", () => Js.Str(() => ".btn-dir-list-cancel")), new List<JsSyntax>
                    {
                        Js.Id(() => "closeDirListPopup").Invoke().Stmt()
                    })
                }
            })))));
        js.Add(() => Js.Func(() => "browseDirListDir", () => new List<string> { "pathInput" }, () => browseDirListDirBlock));

        // confirmDirListDir() — confirm selection: fill pathInput and close popup
        var confirmDirListDirBlock = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "currentDirListPathInput"), new List<JsSyntax>
                    {
                        Js.Id(() => "currentDirListPathInput").Prop(() => "value").Assign(() => Js.Id(() => "dirListCurrentPath")).Stmt()
                    })
                }
            }))
            .Add(() => Js.Const(() => "popup", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dirListPopup"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "popup"), new List<JsSyntax> { Js.Id(() => "popup").Prop(() => "remove").Invoke().Stmt() }) }
            }));
        js.Add(() => Js.Func(() => "confirmDirListDir", () => new List<string> { }, () => confirmDirListDirBlock));

        // closeDirListPopup() — cancel: close popup without changing pathInput
        var closeDirListPopupBlock = Js.Block()
            .Add(() => Js.Const(() => "popup", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dirListPopup"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "popup"), new List<JsSyntax> { Js.Id(() => "popup").Prop(() => "remove").Invoke().Stmt() }) }
            }));
        js.Add(() => Js.Func(() => "closeDirListPopup", () => new List<string> { }, () => closeDirListPopupBlock));

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

        var initIMPlatformListEditorBlock = Js.Block()
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "imPlatformListContainer")).Prop(() => "innerHTML").Assign(() => Js.Str(() => "")))
            .Add(() => Js.Let(() => "platforms", () => Js.Ternary(() => Js.Id(() => "jsonStr"), () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "jsonStr")), () => Js.Array())));

        var forEachPlatformBlock = Js.Block()
            .Add(() => Js.Id(() => "addIMPlatformRow").Invoke(() => Js.Id(() => "p").Prop(() => "platform"), () => Js.Id(() => "p").Prop(() => "enabled"), () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Id(() => "p").Prop(() => "config"))));

        var elseBlock = Js.Block()
            .Add(() => Js.Const(() => "row", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Id(() => "row").Prop(() => "className").Assign(() => Js.Str(() => "im-platform-empty")))
            .Add(() => Js.Id(() => "row").Prop(() => "textContent").Assign(() => Js.Str(() => "暂无已配置的 IM 平台，点击下方按钮添加")))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "imPlatformListContainer")).Call(() => "appendChild", () => Js.Id(() => "row")));

        initIMPlatformListEditorBlock.Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
        {
            { (Js.Id(() => "platforms").Prop(() => "length").Op(() => ">", () => Js.Num(() => "0")), new List<JsSyntax>
                {
                    Js.Id(() => "platforms").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "p" }, () => forEachPlatformBlock))
                })
            },
            { (null, new List<JsSyntax>
                {
                    elseBlock
                })
            }
        }));
        
        js.Add(() => Js.Func(() => "initIMPlatformListEditor", () => new List<string> { "jsonStr" }, () => initIMPlatformListEditorBlock));

        // 平台配置字段 schema（数据源：IMProviderRegistry，服务端序列化后注入前端）
        var imSchemaData = SiliconLife.Common.IM.IMProviderRegistry.GetAll()
            .ToDictionary(
                m => m.PlatformId,
                m => m.ConfigFields.Select(f => (object)new
                {
                    key = f.Key,
                    label = f.Label,
                    type = f.Type,
                    required = f.Required,
                    placeholder = f.Placeholder ?? ""
                }).ToArray());
        var imSchemaJson = System.Text.Json.JsonSerializer.Serialize(imSchemaData);
        js.Add(() => Js.Const(() => "imPlatformSchemas", () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Str(() => imSchemaJson))));

        // 平台授权元数据（authModes / needsPublicCallback / 接入帮助，数据源：IMProviderRegistry）
        var imMetaData = SiliconLife.Common.IM.IMProviderRegistry.GetAll()
            .ToDictionary(
                m => m.PlatformId,
                m => (object)new
                {
                    authModes = m.AuthModes,
                    needsPublicCallback = m.NeedsPublicCallback,
                    // 帮助文案经本地化解析，未注册时为空串（前端据此隐藏帮助区）
                    help = ResolveIMHelpText(loc, m.HelpKey),
                    helpUrl = m.HelpUrl ?? ""
                });
        var imMetaJson = System.Text.Json.JsonSerializer.Serialize(imMetaData);
        js.Add(() => Js.Const(() => "imPlatformMeta", () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Str(() => imMetaJson))));
        js.Add(() => Js.Let(() => "imAuthEventSource", () => Js.Null()));

        // renderPlatformFields(platform, container, existingConfig)
        var renderFieldBlock = Js.Block()
            .Add(() => Js.Const(() => "row", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Id(() => "row").Prop(() => "className").Assign(() => Js.Str(() => "im-platform-field-row")))
            .Add(() => Js.Const(() => "labelEl", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "label"))))
            .Add(() => Js.Id(() => "labelEl").Prop(() => "textContent").Assign(() => Js.Id(() => "f").Prop(() => "label").Op(() => "+", () => Js.Ternary(() => Js.Id(() => "f").Prop(() => "required"), () => Js.Str(() => " *"), () => Js.Str(() => "")))))
            .Add(() => Js.Id(() => "row").Call(() => "appendChild", () => Js.Id(() => "labelEl")))
            .Add(() => Js.Const(() => "input", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "input"))))
            .Add(() => Js.Id(() => "input").Prop(() => "type").Assign(() => Js.Id(() => "f").Prop(() => "type")))
            .Add(() => Js.Id(() => "input").Prop(() => "className").Assign(() => Js.Str(() => "im-platform-config-field")))
            .Add(() => Js.Id(() => "input").Prop(() => "dataset").Prop(() => "key").Assign(() => Js.Id(() => "f").Prop(() => "key")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "f").Prop(() => "placeholder"), new List<JsSyntax>
                    {
                        Js.Id(() => "input").Prop(() => "placeholder").Assign(() => Js.Id(() => "f").Prop(() => "placeholder"))
                    })
                }
            }))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "existingConfig").Op(() => "&&", () => Js.Id(() => "existingConfig").Index(() => Js.Id(() => "f").Prop(() => "key")).Op(() => "!==", () => Js.Id(() => "undefined"))), new List<JsSyntax>
                    {
                        Js.Id(() => "input").Prop(() => "value").Assign(() => Js.Id(() => "existingConfig").Index(() => Js.Id(() => "f").Prop(() => "key")))
                    })
                }
            }))
            .Add(() => Js.Id(() => "row").Call(() => "appendChild", () => Js.Id(() => "input")))
            .Add(() => Js.Id(() => "container").Call(() => "appendChild", () => Js.Id(() => "row")));

        var renderPlatformFieldsBlock = Js.Block()
            .Add(() => Js.Id(() => "container").Prop(() => "innerHTML").Assign(() => Js.Str(() => "")))
            .Add(() => Js.Const(() => "fields", () => Js.Id(() => "imPlatformSchemas").Index(() => Js.Id(() => "platform")).Op(() => "||", () => Js.Array())))
            .Add(() => Js.Id(() => "fields").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "f" }, () => renderFieldBlock)));
        js.Add(() => Js.Func(() => "renderPlatformFields", () => new List<string> { "platform", "container", "existingConfig" }, () => renderPlatformFieldsBlock));

        // renderHelpSection(platform, helpContainer) — 可折叠接入帮助区（帮助文案 + 官方文档链接）
        var renderHelpSectionBlock = Js.Block()
            .Add(() => Js.Id(() => "helpContainer").Prop(() => "innerHTML").Assign(() => Js.Str(() => "")))
            .Add(() => Js.Const(() => "meta", () => Js.Id(() => "imPlatformMeta").Index(() => Js.Id(() => "platform"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "meta").Not().Op(() => "||", () => Js.Id(() => "meta").Prop(() => "help").Not().Op(() => "&&", () => Js.Id(() => "meta").Prop(() => "helpUrl").Not())), new List<JsSyntax>
                    {
                        Js.Return(() => Js.Id(() => "undefined"))
                    })
                }
            }))
            .Add(() => Js.Const(() => "details", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "details"))))
            .Add(() => Js.Id(() => "details").Prop(() => "className").Assign(() => Js.Str(() => "im-help-details")))
            .Add(() => Js.Const(() => "summary", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "summary"))))
            .Add(() => Js.Id(() => "summary").Prop(() => "textContent").Assign(() => Js.Str(() => locIMHelpTitle)))
            .Add(() => Js.Id(() => "details").Call(() => "appendChild", () => Js.Id(() => "summary")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "meta").Prop(() => "help"), new List<JsSyntax>
                    {
                        Js.Const(() => "helpText", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))).Stmt(),
                        Js.Id(() => "helpText").Prop(() => "className").Assign(() => Js.Str(() => "im-help-text")).Stmt(),
                        Js.Id(() => "helpText").Prop(() => "textContent").Assign(() => Js.Id(() => "meta").Prop(() => "help")).Stmt(),
                        Js.Id(() => "details").Call(() => "appendChild", () => Js.Id(() => "helpText")).Stmt()
                    })
                }
            }))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "meta").Prop(() => "helpUrl"), new List<JsSyntax>
                    {
                        Js.Const(() => "helpLink", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "a"))).Stmt(),
                        Js.Id(() => "helpLink").Prop(() => "className").Assign(() => Js.Str(() => "im-help-link")).Stmt(),
                        Js.Id(() => "helpLink").Prop(() => "href").Assign(() => Js.Id(() => "meta").Prop(() => "helpUrl")).Stmt(),
                        Js.Id(() => "helpLink").Prop(() => "target").Assign(() => Js.Str(() => "_blank")).Stmt(),
                        Js.Id(() => "helpLink").Prop(() => "rel").Assign(() => Js.Str(() => "noopener")).Stmt(),
                        Js.Id(() => "helpLink").Prop(() => "textContent").Assign(() => Js.Str(() => locIMHelpOfficialDoc)).Stmt(),
                        Js.Id(() => "details").Call(() => "appendChild", () => Js.Id(() => "helpLink")).Stmt()
                    })
                }
            }))
            .Add(() => Js.Id(() => "helpContainer").Call(() => "appendChild", () => Js.Id(() => "details")));
        js.Add(() => Js.Func(() => "renderHelpSection", () => new List<string> { "platform", "helpContainer" }, () => renderHelpSectionBlock));

        // imAuthStatusText(status, message) — 授权状态本地化文案
        var imAuthStatusTextBlock = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "status").Op(() => "===", () => Js.Str(() => "success")), new List<JsSyntax>
                    {
                        Js.Return(() => Js.Str(() => locIMAuthorized))
                    })
                }
            }))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "status").Op(() => "===", () => Js.Str(() => "timeout")), new List<JsSyntax>
                    {
                        Js.Return(() => Js.Str(() => locIMAuthTimeout))
                    })
                }
            }))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "status").Op(() => "===", () => Js.Str(() => "pending")), new List<JsSyntax>
                    {
                        Js.Return(() => Js.Str(() => locIMWaiting))
                    })
                }
            }))
            .Add(() => Js.Return(() => Js.Str(() => locIMAuthFailed).Op(() => "+", () => Js.Ternary(() => Js.Id(() => "message"), () => Js.Str(() => ": ").Op(() => "+", () => Js.Id(() => "message")), () => Js.Str(() => "")))));
        js.Add(() => Js.Func(() => "imAuthStatusText", () => new List<string> { "status", "message" }, () => imAuthStatusTextBlock));

        // imAuthSSEConnect() — 懒创建共享 SSE 连接并监听 im_auth_status 事件（platform 匹配才处理）
        var imAuthSSEHandlerBlock = Js.Block()
            .Add(() => Js.Const(() => "d", () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "e").Prop(() => "data"))))
            .Add(() => Js.Id(() => "document").Call(() => "querySelectorAll", () => Js.Str(() => ".im-platform-item")).Call(() => "forEach", () => Js.Arrow(() => new List<string> { "item" }, () => (JsSyntax)Js.Block()
                .Add(() => Js.Const(() => "sel", () => Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => ".im-platform-select"))))
                .Add(() => Js.Const(() => "statusEl", () => Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => ".im-auth-status"))))
                .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                {
                    { (Js.Id(() => "sel").Op(() => "&&", () => Js.Id(() => "statusEl")).Op(() => "&&", () => Js.Id(() => "sel").Prop(() => "value").Op(() => "===", () => Js.Id(() => "d").Prop(() => "platform"))), new List<JsSyntax>
                        {
                            Js.Id(() => "statusEl").Prop(() => "textContent").Assign(() => Js.Id(() => "imAuthStatusText").Invoke(() => Js.Id(() => "d").Prop(() => "status"), () => Js.Id(() => "d").Prop(() => "message"))).Stmt()
                        })
                    }
                })))));
        var imAuthSSEConnectBlock = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "imAuthEventSource"), new List<JsSyntax>
                    {
                        Js.Return(() => Js.Id(() => "undefined"))
                    })
                }
            }))
            .Add(() => Js.Assign(() => Js.Id(() => "imAuthEventSource"), () => Js.New(() => Js.Id(() => "EventSource"), () => Js.Str(() => "/api/chat/stream"))))
            .Add(() => Js.Id(() => "imAuthEventSource").Call(() => "addEventListener", () => Js.Str(() => "im_auth_status"), () => Js.Arrow(() => new List<string> { "e" }, () => (JsSyntax)imAuthSSEHandlerBlock)));
        js.Add(() => Js.Func(() => "imAuthSSEConnect", () => new List<string> { }, () => imAuthSSEConnectBlock));

        // startImAuth(platform, statusEl) — 发起授权：连 SSE → 置等待 → fetch /im/{platform}/authorize
        var startImAuthThenBlock = Js.Block()
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "d").Prop(() => "success").Not(), new List<JsSyntax>
                    {
                        Js.Id(() => "statusEl").Prop(() => "textContent").Assign(() => Js.Str(() => locIMAuthFailed).Op(() => "+", () => Js.Str(() => ": ")).Op(() => "+", () => Js.Id(() => "d").Prop(() => "message"))).Stmt()
                    })
                }
            }));
        var startImAuthBlock = Js.Block()
            .Add(() => Js.Id(() => "imAuthSSEConnect").Invoke().Stmt())
            .Add(() => Js.Id(() => "statusEl").Prop(() => "textContent").Assign(() => Js.Str(() => locIMWaiting)))
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/im/").Op(() => "+", () => Js.Id(() => "encodeURIComponent").Invoke(() => Js.Id(() => "platform"))).Op(() => "+", () => Js.Str(() => "/authorize")))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json")))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "d" }, () => (JsSyntax)startImAuthThenBlock))
                .Call(() => "catch", () => Js.Arrow(() => new List<string> { }, () => Js.Id(() => "statusEl").Prop(() => "textContent").Assign(() => Js.Str(() => locIMAuthFailed)).Stmt())).Stmt());
        js.Add(() => Js.Func(() => "startImAuth", () => new List<string> { "platform", "statusEl" }, () => startImAuthBlock));

        // 接入方式选项 HTML（服务端拼接，文案经 loc）
        var authModeOptionsHtml = $"<option value=\"manual\">{locIMManualMode}</option><option value=\"oauth\">{locIMScanMode}</option>";

        // renderAuthSection(platform, authContainer, existingConfig) — 行内授权区
        var applyImAuthModeBlock = Js.Block()
            .Add(() => Js.Id(() => "panel").Prop(() => "style").Prop(() => "display").Assign(() => Js.Ternary(() => Js.Id(() => "modeSelect").Prop(() => "value").Op(() => "===", () => Js.Str(() => "oauth")), () => Js.Str(() => "flex"), () => Js.Str(() => "none"))));
        var renderAuthSectionBlock = Js.Block()
            .Add(() => Js.Id(() => "authContainer").Prop(() => "innerHTML").Assign(() => Js.Str(() => "")))
            .Add(() => Js.Const(() => "meta", () => Js.Id(() => "imPlatformMeta").Index(() => Js.Id(() => "platform"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "meta").Not().Op(() => "||", () => Js.Id(() => "meta").Prop(() => "authModes").Call(() => "indexOf", () => Js.Str(() => "oauth")).Op(() => "<", () => Js.Num(() => "0"))), new List<JsSyntax>
                    {
                        Js.Return(() => Js.Id(() => "undefined"))
                    })
                }
            }))
            .Add(() => Js.Const(() => "modeRow", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Id(() => "modeRow").Prop(() => "className").Assign(() => Js.Str(() => "im-platform-row")))
            .Add(() => Js.Const(() => "modeLabel", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "label"))))
            .Add(() => Js.Id(() => "modeLabel").Prop(() => "textContent").Assign(() => Js.Str(() => locIMAuthMode)))
            .Add(() => Js.Const(() => "modeSelect", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "select"))))
            .Add(() => Js.Id(() => "modeSelect").Prop(() => "className").Assign(() => Js.Str(() => "im-auth-mode-select")))
            .Add(() => Js.Id(() => "modeSelect").Call(() => "insertAdjacentHTML", () => Js.Str(() => "beforeend"), () => Js.Str(() => authModeOptionsHtml)))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "existingConfig").Op(() => "&&", () => Js.Id(() => "existingConfig").Prop(() => "authMode")), new List<JsSyntax>
                    {
                        Js.Id(() => "modeSelect").Prop(() => "value").Assign(() => Js.Id(() => "existingConfig").Prop(() => "authMode")).Stmt()
                    })
                }
            }))
            .Add(() => Js.Id(() => "modeRow").Call(() => "appendChild", () => Js.Id(() => "modeLabel")))
            .Add(() => Js.Id(() => "modeRow").Call(() => "appendChild", () => Js.Id(() => "modeSelect")))
            .Add(() => Js.Id(() => "authContainer").Call(() => "appendChild", () => Js.Id(() => "modeRow")))
            .Add(() => Js.Const(() => "panel", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Id(() => "panel").Prop(() => "className").Assign(() => Js.Str(() => "im-oauth-panel")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "meta").Prop(() => "needsPublicCallback"), new List<JsSyntax>
                    {
                        Js.Const(() => "rbRow", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))).Stmt(),
                        Js.Id(() => "rbRow").Prop(() => "className").Assign(() => Js.Str(() => "im-platform-field-row")).Stmt(),
                        Js.Const(() => "rbLabel", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "label"))).Stmt(),
                        Js.Id(() => "rbLabel").Prop(() => "textContent").Assign(() => Js.Str(() => locIMRedirectBase)).Stmt(),
                        Js.Const(() => "rbInput", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "input"))).Stmt(),
                        Js.Id(() => "rbInput").Prop(() => "type").Assign(() => Js.Str(() => "text")).Stmt(),
                        Js.Id(() => "rbInput").Prop(() => "className").Assign(() => Js.Str(() => "im-redirect-base-url")).Stmt(),
                        Js.Id(() => "rbInput").Prop(() => "placeholder").Assign(() => Js.Str(() => "https://example.com")).Stmt(),
                        Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                        {
                            { (Js.Id(() => "existingConfig").Op(() => "&&", () => Js.Id(() => "existingConfig").Prop(() => "redirectBaseUrl")), new List<JsSyntax>
                                {
                                    Js.Id(() => "rbInput").Prop(() => "value").Assign(() => Js.Id(() => "existingConfig").Prop(() => "redirectBaseUrl")).Stmt()
                                })
                            }
                        }),
                        Js.Id(() => "rbRow").Call(() => "appendChild", () => Js.Id(() => "rbLabel")).Stmt(),
                        Js.Id(() => "rbRow").Call(() => "appendChild", () => Js.Id(() => "rbInput")).Stmt(),
                        Js.Id(() => "panel").Call(() => "appendChild", () => Js.Id(() => "rbRow")).Stmt(),
                        Js.Const(() => "hint", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))).Stmt(),
                        Js.Id(() => "hint").Prop(() => "className").Assign(() => Js.Str(() => "im-oauth-hint")).Stmt(),
                        Js.Id(() => "hint").Prop(() => "textContent").Assign(() => Js.Str(() => locIMCallbackHint)).Stmt(),
                        Js.Id(() => "panel").Call(() => "appendChild", () => Js.Id(() => "hint")).Stmt()
                    })
                }
            }))
            .Add(() => Js.Const(() => "actionRow", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Id(() => "actionRow").Prop(() => "className").Assign(() => Js.Str(() => "im-oauth-action-row")))
            .Add(() => Js.Const(() => "authBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Id(() => "authBtn").Prop(() => "type").Assign(() => Js.Str(() => "button")))
            .Add(() => Js.Id(() => "authBtn").Prop(() => "textContent").Assign(() => Js.Str(() => locIMScanBtn)))
            .Add(() => Js.Id(() => "authBtn").Prop(() => "className").Assign(() => Js.Str(() => "btn-im-scan-auth")))
            .Add(() => Js.Const(() => "authStatus", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "span"))))
            .Add(() => Js.Id(() => "authStatus").Prop(() => "className").Assign(() => Js.Str(() => "im-auth-status")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "existingConfig").Op(() => "&&", () => Js.Id(() => "existingConfig").Prop(() => "accessToken")), new List<JsSyntax>
                    {
                        Js.Id(() => "authStatus").Prop(() => "textContent").Assign(() => Js.Str(() => locIMAuthorized)).Stmt()
                    })
                }
            }))
            .Add(() => Js.Id(() => "authBtn").Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string> { }, () => Js.Id(() => "startImAuth").Invoke(() => Js.Id(() => "platform"), () => Js.Id(() => "authStatus")).Stmt())))
            .Add(() => Js.Id(() => "actionRow").Call(() => "appendChild", () => Js.Id(() => "authBtn")))
            .Add(() => Js.Id(() => "actionRow").Call(() => "appendChild", () => Js.Id(() => "authStatus")))
            .Add(() => Js.Id(() => "panel").Call(() => "appendChild", () => Js.Id(() => "actionRow")))
            .Add(() => Js.Const(() => "applyAuthMode", () => Js.Arrow(() => new List<string> { }, () => (JsSyntax)applyImAuthModeBlock)))
            .Add(() => Js.Id(() => "modeSelect").Call(() => "addEventListener", () => Js.Str(() => "change"), () => Js.Id(() => "applyAuthMode")))
            .Add(() => Js.Id(() => "applyAuthMode").Invoke().Stmt())
            .Add(() => Js.Id(() => "authContainer").Call(() => "appendChild", () => Js.Id(() => "panel")));
        js.Add(() => Js.Func(() => "renderAuthSection", () => new List<string> { "platform", "authContainer", "existingConfig" }, () => renderAuthSectionBlock));

        var addIMPlatformRowBlock = Js.Block()
            .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "imPlatformListContainer"))))
            .Add(() => Js.Const(() => "row", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Id(() => "row").Prop(() => "className").Assign(() => Js.Str(() => "im-platform-item")));
        
        addIMPlatformRowBlock.Add(() => Js.Const(() => "header", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Id(() => "header").Prop(() => "className").Assign(() => Js.Str(() => "im-platform-item-header")));
        
        addIMPlatformRowBlock.Add(() => Js.Const(() => "platformLabel", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "label"))))
            .Add(() => Js.Id(() => "platformLabel").Prop(() => "textContent").Assign(() => Js.Str(() => locIMPlatformType)));
        addIMPlatformRowBlock.Add(() => Js.Id(() => "header").Call(() => "appendChild", () => Js.Id(() => "platformLabel")));
        
        addIMPlatformRowBlock.Add(() => Js.Const(() => "platformSelect", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "select"))))
            .Add(() => Js.Id(() => "platformSelect").Prop(() => "className").Assign(() => Js.Str(() => "im-platform-select")));
        
        // 平台 select 选项（数据源：IMProviderRegistry）
        var imPlatformOptionsHtml = string.Join("", SiliconLife.Common.IM.IMProviderRegistry.GetAll()
            .Select(m => $"<option value=\"{m.PlatformId}\">{m.DisplayName}</option>"));
        addIMPlatformRowBlock.Add(() => Js.Id(() => "platformSelect").Call(() => "insertAdjacentHTML", () => Js.Str(() => "beforeend"), () => Js.Str(() => imPlatformOptionsHtml)));
        
        addIMPlatformRowBlock.Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
        {
            { (Js.Id(() => "platform"), new List<JsSyntax>
                {
                    Js.Id(() => "platformSelect").Prop(() => "value").Assign(() => Js.Id(() => "platform"))
                })
            }
        }));
        
        addIMPlatformRowBlock.Add(() => Js.Id(() => "header").Call(() => "appendChild", () => Js.Id(() => "platformSelect")));
        
        addIMPlatformRowBlock.Add(() => Js.Const(() => "enabledDiv", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Id(() => "enabledDiv").Prop(() => "className").Assign(() => Js.Str(() => "im-platform-enabled")));
        
        addIMPlatformRowBlock.Add(() => Js.Const(() => "enabledCheckbox", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "input"))))
            .Add(() => Js.Id(() => "enabledCheckbox").Prop(() => "type").Assign(() => Js.Str(() => "checkbox")))
            .Add(() => Js.Id(() => "enabledCheckbox").Prop(() => "className").Assign(() => Js.Str(() => "im-platform-enabled-check")));
        
        addIMPlatformRowBlock.Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
        {
            { (Js.Id(() => "enabled").Op(() => "!==", () => Js.Bool(() => false)), new List<JsSyntax>
                {
                    Js.Id(() => "enabledCheckbox").Prop(() => "checked").Assign(() => Js.Bool(() => true))
                })
            }
        }));
        addIMPlatformRowBlock.Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
        {
            { (Js.Id(() => "enabled").Op(() => "===", () => Js.Bool(() => false)), new List<JsSyntax>
                {
                    Js.Id(() => "enabledCheckbox").Prop(() => "checked").Assign(() => Js.Bool(() => false))
                })
            }
        }));
        
        addIMPlatformRowBlock.Add(() => Js.Const(() => "enabledLabel", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "label"))))
            .Add(() => Js.Id(() => "enabledLabel").Prop(() => "textContent").Assign(() => Js.Str(() => locIMEnabled)));
        
        addIMPlatformRowBlock.Add(() => Js.Id(() => "enabledDiv").Call(() => "appendChild", () => Js.Id(() => "enabledCheckbox")))
            .Add(() => Js.Id(() => "enabledDiv").Call(() => "appendChild", () => Js.Id(() => "enabledLabel")));
        
        addIMPlatformRowBlock.Add(() => Js.Id(() => "header").Call(() => "appendChild", () => Js.Id(() => "enabledDiv")));
        
        addIMPlatformRowBlock.Add(() => Js.Const(() => "deleteBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Id(() => "deleteBtn").Prop(() => "textContent").Assign(() => Js.Str(() => locIMDelete)))
            .Add(() => Js.Id(() => "deleteBtn").Prop(() => "className").Assign(() => Js.Str(() => "btn-im-platform-delete")))
            .Add(() => Js.Id(() => "deleteBtn").Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string> { }, () => Js.Id(() => "row").Prop(() => "remove").Invoke().Stmt())));
        
        addIMPlatformRowBlock.Add(() => Js.Id(() => "header").Call(() => "appendChild", () => Js.Id(() => "deleteBtn")));
        
        addIMPlatformRowBlock.Add(() => Js.Id(() => "row").Call(() => "appendChild", () => Js.Id(() => "header")));
        
        addIMPlatformRowBlock.Add(() => Js.Const(() => "content", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Id(() => "content").Prop(() => "className").Assign(() => Js.Str(() => "im-platform-item-content")));
        
        addIMPlatformRowBlock.Add(() => Js.Const(() => "helpSection", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Id(() => "helpSection").Prop(() => "className").Assign(() => Js.Str(() => "im-help-section")));

        addIMPlatformRowBlock.Add(() => Js.Const(() => "configFields", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Id(() => "configFields").Prop(() => "className").Assign(() => Js.Str(() => "im-platform-config-fields")));

        addIMPlatformRowBlock.Add(() => Js.Const(() => "authSection", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Id(() => "authSection").Prop(() => "className").Assign(() => Js.Str(() => "im-auth-section")));

        addIMPlatformRowBlock.Add(() => Js.Id(() => "content").Call(() => "appendChild", () => Js.Id(() => "helpSection")));
        addIMPlatformRowBlock.Add(() => Js.Id(() => "content").Call(() => "appendChild", () => Js.Id(() => "configFields")));
        addIMPlatformRowBlock.Add(() => Js.Id(() => "content").Call(() => "appendChild", () => Js.Id(() => "authSection")));

        addIMPlatformRowBlock.Add(() => Js.Id(() => "row").Call(() => "appendChild", () => Js.Id(() => "content")));
        addIMPlatformRowBlock.Add(() => Js.Id(() => "container").Call(() => "appendChild", () => Js.Id(() => "row")));

        // 平台切换时重新渲染帮助区、字段与授权区，并清空暂存 token
        var platformChangeBlock = Js.Block()
            .Add(() => Js.Id(() => "renderHelpSection").Invoke(() => Js.Id(() => "platformSelect").Prop(() => "value"), () => Js.Id(() => "helpSection")).Stmt())
            .Add(() => Js.Id(() => "renderPlatformFields").Invoke(() => Js.Id(() => "platformSelect").Prop(() => "value"), () => Js.Id(() => "configFields"), () => Js.Id(() => "null")).Stmt())
            .Add(() => Js.Id(() => "renderAuthSection").Invoke(() => Js.Id(() => "platformSelect").Prop(() => "value"), () => Js.Id(() => "authSection"), () => Js.Id(() => "null")).Stmt())
            .Add(() => Js.Id(() => "row").Prop(() => "dataset").Prop(() => "imTokens").Assign(() => Js.Str(() => "{}")));
        addIMPlatformRowBlock.Add(() => Js.Id(() => "platformSelect").Call(() => "addEventListener", () => Js.Str(() => "change"), () => Js.Arrow(() => new List<string> { }, () => (JsSyntax)platformChangeBlock)));

        // 初始渲染：解析已有配置并渲染帮助区、对应字段与授权区
        addIMPlatformRowBlock.Add(() => Js.Const(() => "existingConfig", () => Js.Ternary(() => Js.Id(() => "configJson"), () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "configJson")), () => Js.Id(() => "null"))));
        addIMPlatformRowBlock.Add(() => Js.Id(() => "renderHelpSection").Invoke(() => Js.Id(() => "platformSelect").Prop(() => "value"), () => Js.Id(() => "helpSection")).Stmt());
        addIMPlatformRowBlock.Add(() => Js.Id(() => "renderPlatformFields").Invoke(() => Js.Id(() => "platformSelect").Prop(() => "value"), () => Js.Id(() => "configFields"), () => Js.Id(() => "existingConfig")).Stmt());
        addIMPlatformRowBlock.Add(() => Js.Id(() => "renderAuthSection").Invoke(() => Js.Id(() => "platformSelect").Prop(() => "value"), () => Js.Id(() => "authSection"), () => Js.Id(() => "existingConfig")).Stmt());

        // 暂存已有 token 键（重新编辑保存时不丢失）
        addIMPlatformRowBlock.Add(() => Js.Const(() => "savedTokens", () => Js.Obj()));
        addIMPlatformRowBlock.Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
        {
            { (Js.Id(() => "existingConfig").Op(() => "&&", () => Js.Id(() => "existingConfig").Prop(() => "accessToken")), new List<JsSyntax>
                {
                    Js.Id(() => "savedTokens").Prop(() => "accessToken").Assign(() => Js.Id(() => "existingConfig").Prop(() => "accessToken")).Stmt()
                })
            }
        }));
        addIMPlatformRowBlock.Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
        {
            { (Js.Id(() => "existingConfig").Op(() => "&&", () => Js.Id(() => "existingConfig").Prop(() => "refreshToken")), new List<JsSyntax>
                {
                    Js.Id(() => "savedTokens").Prop(() => "refreshToken").Assign(() => Js.Id(() => "existingConfig").Prop(() => "refreshToken")).Stmt()
                })
            }
        }));
        addIMPlatformRowBlock.Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
        {
            { (Js.Id(() => "existingConfig").Op(() => "&&", () => Js.Id(() => "existingConfig").Prop(() => "tokenExpiresAt")), new List<JsSyntax>
                {
                    Js.Id(() => "savedTokens").Prop(() => "tokenExpiresAt").Assign(() => Js.Id(() => "existingConfig").Prop(() => "tokenExpiresAt")).Stmt()
                })
            }
        }));
        addIMPlatformRowBlock.Add(() => Js.Id(() => "row").Prop(() => "dataset").Prop(() => "imTokens").Assign(() => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Id(() => "savedTokens"))));

        js.Add(() => Js.Func(() => "addIMPlatformRow", () => new List<string> { "platform", "enabled", "configJson" }, () => addIMPlatformRowBlock));

        var getIMPlatformListJsonBlock = Js.Block()
            .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "imPlatformListContainer"))))
            .Add(() => Js.Const(() => "items", () => Js.Id(() => "container").Call(() => "querySelectorAll", () => Js.Str(() => ".im-platform-item"))))
            .Add(() => Js.Const(() => "platforms", () => Js.Array()));
        
        var processItemBlock = Js.Block()
            .Add(() => Js.Const(() => "select", () => Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => ".im-platform-select"))))
            .Add(() => Js.Const(() => "checkbox", () => Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => ".im-platform-enabled-check"))))
            .Add(() => Js.Const(() => "fieldInputs", () => Js.Id(() => "item").Call(() => "querySelectorAll", () => Js.Str(() => ".im-platform-config-field"))))
            .Add(() => Js.Const(() => "config", () => Js.Obj()))
            .Add(() => Js.Id(() => "fieldInputs").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "inp" }, () => Js.Block()
                .Add(() => Js.Id(() => "config").Index(() => Js.Id(() => "inp").Prop(() => "dataset").Prop(() => "key")).Assign(() => Js.Id(() => "inp").Prop(() => "value"))))))
            // 合并暂存 token 键（accessToken/refreshToken/tokenExpiresAt）
            .Add(() => Js.Const(() => "savedTokens", () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "item").Prop(() => "dataset").Prop(() => "imTokens").Op(() => "||", () => Js.Str(() => "{}")))))
            .Add(() => Js.Id(() => "Object").Prop(() => "keys").Invoke(() => Js.Id(() => "savedTokens")).Call(() => "forEach", () => Js.Arrow(() => new List<string> { "k" }, () => Js.Id(() => "config").Index(() => Js.Id(() => "k")).Assign(() => Js.Id(() => "savedTokens").Index(() => Js.Id(() => "k"))).Stmt())))
            // 收集接入方式与回调基础地址（仅含 oauth 的平台才有这些控件）
            .Add(() => Js.Const(() => "authSel", () => Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => ".im-auth-mode-select"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "authSel"), new List<JsSyntax>
                    {
                        Js.Id(() => "config").Index(() => Js.Str(() => "authMode")).Assign(() => Js.Id(() => "authSel").Prop(() => "value")).Stmt()
                    })
                }
            }))
            .Add(() => Js.Const(() => "rbInput", () => Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => ".im-redirect-base-url"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "rbInput").Op(() => "&&", () => Js.Id(() => "rbInput").Prop(() => "value")), new List<JsSyntax>
                    {
                        Js.Id(() => "config").Index(() => Js.Str(() => "redirectBaseUrl")).Assign(() => Js.Id(() => "rbInput").Prop(() => "value")).Stmt()
                    })
                }
            }))
            .Add(() => Js.Id(() => "platforms").Call(() => "push", () => Js.Obj()
                .Prop(() => "platform", () => Js.Id(() => "select").Prop(() => "value"))
                .Prop(() => "enabled", () => Js.Id(() => "checkbox").Prop(() => "checked"))
                .Prop(() => "config", () => Js.Id(() => "config"))));
        
        getIMPlatformListJsonBlock.Add(() => Js.Id(() => "items").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "item" }, () => processItemBlock)));
        getIMPlatformListJsonBlock.Add(() => Js.Return(() => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Id(() => "platforms"))));
        
        js.Add(() => Js.Func(() => "getIMPlatformListJson", () => new List<string> { }, () => getIMPlatformListJsonBlock));

        // ===== MCP server list editor (inside editModal, no extra popups) =====
        var locMcpRemove = loc.McpBtnRemove;
        var locMcpEmpty = loc.McpsEmptyState;
        var locMcpEnabled = loc.McpBtnToggleOn;
        var locMcpPromptName = loc.McpPromptName;
        var locMcpPromptCommand = loc.McpPromptCommand;
        var locMcpPromptArgs = loc.McpPromptArgs;
        var locMcpPromptUrl = loc.McpPromptUrl;

        // 生成一个字段行（label + input/select），追加到 fields 容器；tag 保证 JS 变量名唯一
        static List<Func<JsSyntax>> McpFieldRow(string tag, string rowClass, string labelText, string inputClass,
            string? placeholder, Func<string, Func<JsSyntax>>? valueSetter, bool isSelect)
        {
            string rowV = $"frow{tag}", labV = $"flab{tag}", inpV = $"finp{tag}";
            var stmts = new List<Func<JsSyntax>>
            {
                () => Js.Const(() => rowV, () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))),
                () => Js.Id(() => rowV).Prop(() => "className").Assign(() => Js.Str(() => rowClass)),
                () => Js.Const(() => labV, () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "label"))),
                () => Js.Id(() => labV).Prop(() => "textContent").Assign(() => Js.Str(() => labelText)),
                () => Js.Id(() => rowV).Call(() => "appendChild", () => Js.Id(() => labV))
            };
            if (isSelect)
            {
                stmts.Add(() => Js.Const(() => inpV, () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "select"))));
                stmts.Add(() => Js.Id(() => inpV).Call(() => "insertAdjacentHTML", () => Js.Str(() => "beforeend"), () => Js.Str(() => "<option value=\"stdio\">stdio</option><option value=\"http\">http</option>")).Stmt());
            }
            else
            {
                stmts.Add(() => Js.Const(() => inpV, () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "input"))));
                stmts.Add(() => Js.Id(() => inpV).Prop(() => "type").Assign(() => Js.Str(() => "text")));
                if (placeholder != null)
                    stmts.Add(() => Js.Id(() => inpV).Prop(() => "placeholder").Assign(() => Js.Str(() => placeholder)));
            }
            stmts.Add(() => Js.Id(() => inpV).Prop(() => "className").Assign(() => Js.Str(() => inputClass)));
            if (valueSetter != null)
                stmts.Add(valueSetter(inpV));
            stmts.Add(() => Js.Id(() => rowV).Call(() => "appendChild", () => Js.Id(() => inpV)));
            stmts.Add(() => Js.Id(() => "fields").Call(() => "appendChild", () => Js.Id(() => rowV)));
            return stmts;
        }

        var addMcpServerRowBlock = Js.Block()
            .Add(() => Js.Const(() => "src", () => Js.Id(() => "cfg").Op(() => "||", () => Js.Obj())))
            .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "mcpServerListContainer"))))
            .Add(() => Js.Const(() => "emptyEl", () => Js.Id(() => "container").Call(() => "querySelector", () => Js.Str(() => ".im-platform-empty"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "emptyEl"), new List<JsSyntax> { Js.Id(() => "emptyEl").Prop(() => "remove").Invoke().Stmt() }) }
            }))
            .Add(() => Js.Const(() => "row", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Id(() => "row").Prop(() => "className").Assign(() => Js.Str(() => "im-platform-item mcp-server-item")))
            // header: Id + Enabled + Delete
            .Add(() => Js.Const(() => "header", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Id(() => "header").Prop(() => "className").Assign(() => Js.Str(() => "im-platform-item-header")))
            .Add(() => Js.Const(() => "idLabel", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "label"))))
            .Add(() => Js.Id(() => "idLabel").Prop(() => "textContent").Assign(() => Js.Str(() => "Id")))
            .Add(() => Js.Id(() => "header").Call(() => "appendChild", () => Js.Id(() => "idLabel")))
            .Add(() => Js.Const(() => "idInput", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "input"))))
            .Add(() => Js.Id(() => "idInput").Prop(() => "type").Assign(() => Js.Str(() => "text")))
            .Add(() => Js.Id(() => "idInput").Prop(() => "className").Assign(() => Js.Str(() => "mcp-server-id")))
            .Add(() => Js.Id(() => "idInput").Prop(() => "style").Prop(() => "flex").Assign(() => Js.Str(() => "1")))
            .Add(() => Js.Id(() => "idInput").Prop(() => "value").Assign(() => Js.Id(() => "src").Prop(() => "Id").Op(() => "||", () => Js.Str(() => ""))))
            .Add(() => Js.Id(() => "header").Call(() => "appendChild", () => Js.Id(() => "idInput")))
            .Add(() => Js.Const(() => "enabledDiv", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Id(() => "enabledDiv").Prop(() => "className").Assign(() => Js.Str(() => "im-platform-enabled")))
            .Add(() => Js.Const(() => "enabledCheckbox", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "input"))))
            .Add(() => Js.Id(() => "enabledCheckbox").Prop(() => "type").Assign(() => Js.Str(() => "checkbox")))
            .Add(() => Js.Id(() => "enabledCheckbox").Prop(() => "className").Assign(() => Js.Str(() => "mcp-server-enabled")))
            .Add(() => Js.Id(() => "enabledCheckbox").Prop(() => "checked").Assign(() => Js.Id(() => "src").Prop(() => "Enabled").Op(() => "===", () => Js.Bool(() => true))))
            .Add(() => Js.Const(() => "enabledLabel", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "label"))))
            .Add(() => Js.Id(() => "enabledLabel").Prop(() => "textContent").Assign(() => Js.Str(() => locMcpEnabled)))
            .Add(() => Js.Id(() => "enabledDiv").Call(() => "appendChild", () => Js.Id(() => "enabledCheckbox")))
            .Add(() => Js.Id(() => "enabledDiv").Call(() => "appendChild", () => Js.Id(() => "enabledLabel")))
            .Add(() => Js.Id(() => "header").Call(() => "appendChild", () => Js.Id(() => "enabledDiv")))
            .Add(() => Js.Const(() => "deleteBtn", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "button"))))
            .Add(() => Js.Id(() => "deleteBtn").Prop(() => "textContent").Assign(() => Js.Str(() => locMcpRemove)))
            .Add(() => Js.Id(() => "deleteBtn").Prop(() => "className").Assign(() => Js.Str(() => "btn-im-platform-delete")))
            .Add(() => Js.Id(() => "deleteBtn").Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "row").Prop(() => "remove").Invoke().Stmt())))
            .Add(() => Js.Id(() => "header").Call(() => "appendChild", () => Js.Id(() => "deleteBtn")))
            .Add(() => Js.Id(() => "row").Call(() => "appendChild", () => Js.Id(() => "header")))
            // fields: Name / Transport / Command / Args / URL
            .Add(() => Js.Const(() => "fields", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Id(() => "fields").Prop(() => "className").Assign(() => Js.Str(() => "im-platform-config-fields")));

        foreach (var stmt in McpFieldRow("Name", "im-platform-field-row", "Name", "mcp-server-name", locMcpPromptName,
            v => () => Js.Id(() => v).Prop(() => "value").Assign(() => Js.Id(() => "src").Prop(() => "Name").Op(() => "||", () => Js.Str(() => ""))), false))
            addMcpServerRowBlock.Add(stmt);

        foreach (var stmt in McpFieldRow("Transport", "im-platform-field-row", "Transport", "mcp-server-transport", null,
            v => () => Js.Id(() => v).Prop(() => "value").Assign(() => Js.Ternary(
                () => Js.Id(() => "src").Prop(() => "Transport").Op(() => "===", () => Js.Num(() => "1")).Op(() => "||", () => (JsSyntax)Js.Id(() => "src").Prop(() => "Transport").Op(() => "===", () => Js.Str(() => "Http"))).Op(() => "||", () => (JsSyntax)Js.Id(() => "src").Prop(() => "Transport").Op(() => "===", () => Js.Str(() => "http"))),
                () => Js.Str(() => "http"), () => Js.Str(() => "stdio"))), true))
            addMcpServerRowBlock.Add(stmt);

        foreach (var stmt in McpFieldRow("Command", "im-platform-field-row mcp-stdio-only", "Command", "mcp-server-command", locMcpPromptCommand,
            v => () => Js.Id(() => v).Prop(() => "value").Assign(() => Js.Id(() => "src").Prop(() => "Command").Op(() => "||", () => Js.Str(() => ""))), false))
            addMcpServerRowBlock.Add(stmt);

        foreach (var stmt in McpFieldRow("Args", "im-platform-field-row mcp-stdio-only", "Args", "mcp-server-args", locMcpPromptArgs,
            v => () => Js.Id(() => v).Prop(() => "value").Assign(() => Js.Id(() => "src").Prop(() => "Args").Op(() => "||", () => Js.Array()).Call(() => "join", () => Js.Str(() => " "))), false))
            addMcpServerRowBlock.Add(stmt);

        foreach (var stmt in McpFieldRow("Url", "im-platform-field-row mcp-http-only", "URL", "mcp-server-url", locMcpPromptUrl,
            v => () => Js.Id(() => v).Prop(() => "value").Assign(() => Js.Id(() => "src").Prop(() => "Url").Op(() => "||", () => Js.Str(() => ""))), false))
            addMcpServerRowBlock.Add(stmt);

        addMcpServerRowBlock
            .Add(() => Js.Id(() => "row").Call(() => "appendChild", () => Js.Id(() => "fields")))
            .Add(() => Js.Id(() => "row").Prop(() => "dataset").Prop(() => "mcpOrig").Assign(() => Js.Ternary(() => Js.Id(() => "cfg"), () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Id(() => "cfg")), () => Js.Str(() => "{}"))))
            .Add(() => Js.Id(() => "container").Call(() => "appendChild", () => Js.Id(() => "row")))
            .Add(() => Js.Id(() => "applyMcpTransport").Invoke(() => Js.Id(() => "row")).Stmt())
            .Add(() => Js.Id(() => "row").Call(() => "querySelector", () => Js.Str(() => ".mcp-server-transport")).Call(() => "addEventListener", () => Js.Str(() => "change"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "applyMcpTransport").Invoke(() => Js.Id(() => "row")))).Stmt());
        js.Add(() => Js.Func(() => "addMcpServerRow", () => new List<string> { "cfg" }, () => addMcpServerRowBlock));

        // applyMcpTransport(row) — 按 transport 显隐 stdio / http 字段
        var applyMcpTransportForEachBlock = Js.Block()
            .Add(() => Js.Id(() => "el").Prop(() => "style").Prop(() => "display").Assign(() => Js.Ternary(() => Js.Id(() => "t").Op(() => "===", () => Js.Str(() => "stdio")), () => Js.Str(() => ""), () => Js.Str(() => "none"))));
        var applyMcpTransportForEachBlock2 = Js.Block()
            .Add(() => Js.Id(() => "el").Prop(() => "style").Prop(() => "display").Assign(() => Js.Ternary(() => Js.Id(() => "t").Op(() => "===", () => Js.Str(() => "http")), () => Js.Str(() => ""), () => Js.Str(() => "none"))));
        var applyMcpTransportBlock = Js.Block()
            .Add(() => Js.Const(() => "t", () => Js.Id(() => "row").Call(() => "querySelector", () => Js.Str(() => ".mcp-server-transport")).Prop(() => "value")))
            .Add(() => Js.Id(() => "row").Call(() => "querySelectorAll", () => Js.Str(() => ".mcp-stdio-only")).Call(() => "forEach", () => Js.Arrow(() => new List<string> { "el" }, () => applyMcpTransportForEachBlock)))
            .Add(() => Js.Id(() => "row").Call(() => "querySelectorAll", () => Js.Str(() => ".mcp-http-only")).Call(() => "forEach", () => Js.Arrow(() => new List<string> { "el" }, () => applyMcpTransportForEachBlock2)));
        js.Add(() => Js.Func(() => "applyMcpTransport", () => new List<string> { "row" }, () => applyMcpTransportBlock));

        // initMcpServerListEditor(jsonStr)
        var initMcpServerListEditorBlock = Js.Block()
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "mcpServerListContainer")).Prop(() => "innerHTML").Assign(() => Js.Str(() => "")))
            .Add(() => Js.Let(() => "servers", () => Js.Ternary(() => Js.Id(() => "jsonStr"), () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "jsonStr")), () => Js.Array())));
        var mcpForEachBlock = Js.Block()
            .Add(() => Js.Id(() => "addMcpServerRow").Invoke(() => Js.Id(() => "s")).Stmt());
        var mcpEmptyBlock = Js.Block()
            .Add(() => Js.Const(() => "emptyRow", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Id(() => "emptyRow").Prop(() => "className").Assign(() => Js.Str(() => "im-platform-empty")))
            .Add(() => Js.Id(() => "emptyRow").Prop(() => "textContent").Assign(() => Js.Str(() => locMcpEmpty)))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "mcpServerListContainer")).Call(() => "appendChild", () => Js.Id(() => "emptyRow")));
        initMcpServerListEditorBlock.Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
        {
            { (Js.Id(() => "servers").Prop(() => "length").Op(() => ">", () => Js.Num(() => "0")), new List<JsSyntax>
                {
                    Js.Id(() => "servers").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "s" }, () => mcpForEachBlock))
                })
            },
            { (null, new List<JsSyntax> { mcpEmptyBlock }) }
        }));
        js.Add(() => Js.Func(() => "initMcpServerListEditor", () => new List<string> { "jsonStr" }, () => initMcpServerListEditorBlock));

        // getMcpServerListJson() — 收集所有行（保留未展示字段），输出 PascalCase JSON
        var collectMcpItemBlock = Js.Block()
            .Add(() => Js.Const(() => "cfg", () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "item").Prop(() => "dataset").Prop(() => "mcpOrig").Op(() => "||", () => Js.Str(() => "{}")))))
            .Add(() => Js.Assign(() => Js.Id(() => "cfg").Prop(() => "Id"), () => Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => ".mcp-server-id")).Prop(() => "value").Call(() => "trim")))
            .Add(() => Js.Assign(() => Js.Id(() => "cfg").Prop(() => "Name"), () => Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => ".mcp-server-name")).Prop(() => "value").Call(() => "trim")))
            .Add(() => Js.Assign(() => Js.Id(() => "cfg").Prop(() => "Transport"), () => Js.Ternary(() => Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => ".mcp-server-transport")).Prop(() => "value").Op(() => "===", () => Js.Str(() => "http")), () => Js.Str(() => "Http"), () => Js.Str(() => "Stdio"))))
            .Add(() => Js.Assign(() => Js.Id(() => "cfg").Prop(() => "Enabled"), () => Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => ".mcp-server-enabled")).Prop(() => "checked")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "cfg").Prop(() => "Transport").Op(() => "===", () => Js.Str(() => "Stdio")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "cfg").Prop(() => "Command"), () => Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => ".mcp-server-command")).Prop(() => "value").Call(() => "trim")).Stmt(),
                        Js.Assign(() => Js.Id(() => "cfg").Prop(() => "Args"), () => Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => ".mcp-server-args")).Prop(() => "value").Call(() => "split", () => Js.Regex(() => "\\s+", () => "")).Call(() => "filter", () => Js.Arrow(() => new List<string> { "a" }, () => (JsSyntax)Js.Id(() => "a").Prop(() => "length").Op(() => ">", () => Js.Num(() => "0"))))).Stmt()
                    })
                },
                { (null, new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "cfg").Prop(() => "Url"), () => Js.Id(() => "item").Call(() => "querySelector", () => Js.Str(() => ".mcp-server-url")).Prop(() => "value").Call(() => "trim")).Stmt()
                    })
                }
            }))
            .Add(() => Js.Id(() => "servers").Call(() => "push", () => Js.Id(() => "cfg")));
        var getMcpServerListJsonBlock = Js.Block()
            .Add(() => Js.Const(() => "items", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "mcpServerListContainer")).Call(() => "querySelectorAll", () => Js.Str(() => ".mcp-server-item"))))
            .Add(() => Js.Const(() => "servers", () => Js.Array()))
            .Add(() => Js.Id(() => "items").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "item" }, () => collectMcpItemBlock)))
            .Add(() => Js.Return(() => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Id(() => "servers"))));
        js.Add(() => Js.Func(() => "getMcpServerListJson", () => new List<string> { }, () => getMcpServerListJsonBlock));

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
                        Js.Const(() => "enumContainer", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "editValueEnumContainer"))).Stmt(),
                        Js.Id(() => "enumContainer").Prop(() => "innerHTML").Assign(() => Js.Str(() => "")).Stmt(),
                        Js.Const(() => "enumOpts", () => Js.Obj()).Stmt(),
                        Js.Id(() => "enumValues").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "v", "i" }, () => Js.Block()
                            .Add(() => Js.Assign(() => Js.Id(() => "enumOpts").Index(() => Js.Id(() => "v")), () => Js.Id(() => "enumDisplayNames").Index(() => Js.Id(() => "i"))))
                        )).Stmt(),
                        Js.Id(() => "slSelectSearch_create").Invoke(() => Js.Id(() => "enumContainer"), () => Js.Obj()
                            .Prop(() => "id", () => Js.Str(() => "editValueEnum"))
                            .Prop(() => "name", () => Js.Str(() => "editValueEnum"))
                            .Prop(() => "value", () => Js.Id(() => "value"))
                            .Prop(() => "options", () => Js.Id(() => "enumOpts"))
                            .Prop(() => "hint", () => Js.Str(() => loc.SelectSearchHint))
                        ).Stmt(),
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
                { (Js.Str(() => "directoryList"), new List<JsSyntax>
                    {
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputDirectoryList")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "block")),
                        Js.Id(() => "initDirListEditor").Invoke(() => Js.Id(() => "value")),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "imPlatformList"), new List<JsSyntax>
                    {
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputIMPlatformList")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "block")),
                        Js.Id(() => "initIMPlatformListEditor").Invoke(() => Js.Id(() => "value")),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "mcpServerList"), new List<JsSyntax>
                    {
                        Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "inputMcpServerList")).Prop(() => "style").Prop(() => "display").Assign(() => Js.Str(() => "block")),
                        Js.Id(() => "initMcpServerListEditor").Invoke(() => Js.Id(() => "value")),
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
                { (Js.Str(() => "directoryList"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "value"), () => Js.Id(() => "getDirListJson").Invoke()),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "imPlatformList"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "value"), () => Js.Id(() => "getIMPlatformListJson").Invoke()),
                        Js.Break()
                    })
                },
                { (Js.Str(() => "mcpServerList"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "value"), () => Js.Id(() => "getMcpServerListJson").Invoke()),
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
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "btnAddDictRow")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string> { }, () => Js.Id(() => "addDictRow").Invoke(() => Js.Str(() => ""), () => Js.Str(() => "")).Stmt())))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "btnAddDirListRow")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string> { }, () => Js.Id(() => "addDirListRow").Invoke(() => Js.Str(() => "")).Stmt())))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "btnAddIMPlatformRow")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "addIMPlatformRow").Invoke().Stmt())))
            .Add(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "btnAddMcpServerRow")).Call(() => "addEventListener", () => Js.Str(() => "click"), () => Js.Arrow(() => new List<string>(), () => Js.Id(() => "addMcpServerRow").Invoke().Stmt())));
        
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
