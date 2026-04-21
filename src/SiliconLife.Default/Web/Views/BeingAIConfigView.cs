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

using System.Text.Json;
using SiliconLife.Collective;
using SiliconLife.Default.Web.Models;

namespace SiliconLife.Default.Web.Views;

public class BeingAIConfigView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as BeingAIConfigViewModel;
        if (vm == null) return string.Empty;

        var body = RenderBody(vm);
        return RenderPage(vm.Skin, $"{vm.BeingName} - AI Config", "beings", vm.Localization, body, GetScripts(vm), GetStyles());
    }

    private static H RenderBody(BeingAIConfigViewModel vm)
    {
        var typeOptions = new List<object>();
        foreach (var opt in vm.AIClientTypeOptions)
        {
            var option = H.Create("option", opt.DisplayName).Attr("value", opt.TypeName);
            if (opt.TypeName == vm.CurrentAIClientType)
                option.Attr("selected", "selected");
            typeOptions.Add(option);
        }

        return H.Div(
            H.Div(
                H.A("← " + vm.Localization.BeingsBackToList).Href("/beings").Class("ai-config-back-link"),
                H.H1(vm.BeingName).Class("ai-config-title"),
                H.P(vm.Localization.BeingsDetailAIClientLabel).Class("ai-config-subtitle")
            ).Class("ai-config-header"),
            H.Div(
                H.Div(
                    H.Create("label",
                        H.Input().Attr("type", "checkbox").Attr("id", "useIndependent").Attr("name", "useIndependent")
                            .Attr(vm.HasIndependentConfig ? "checked" : "", "checked")
                            .Attr("onchange", "toggleIndependent()")
                    ).Class("toggle-label"),
                    H.Span("使用独立AI配置").Class("toggle-text")
                ).Class("toggle-row"),
                H.Div(
                    H.Create("label", vm.Localization.InitAIClientTypeLabel).Attr("for", "aiClientType"),
                    H.Create("select", typeOptions.ToArray())
                        .Attr("name", "aiClientType")
                        .Attr("id", "aiClientTypeSelect")
                        .Attr("onchange", "onClientTypeChange(this.value)")
                ).Class("form-group"),
                H.Div(H.Create("div").Attr("id", "aiConfigFields")).Attr("id", "aiConfigContainer").Class("form-group"),
                H.Div(
                    H.Create("button", vm.Localization.ConfigSaveButton).Attr("type", "button")
                        .Class("btn").Attr("onclick", "saveAIConfig()")
                ).Class("form-actions")
            ).Attr("id", "aiConfigForm").Class("ai-config-form")
        ).Class("page-content");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".ai-config-header")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".ai-config-back-link")
                .Property("display", "inline-block")
                .Property("margin-bottom", "12px")
                .Property("color", "var(--accent-primary)")
                .Property("text-decoration", "none")
                .Property("font-size", "14px")
                .Property("transition", "color 0.2s")
            .EndSelector()
            .Selector(".ai-config-back-link:hover")
                .Property("color", "var(--accent-secondary, var(--accent-primary))")
                .Property("text-decoration", "underline")
            .EndSelector()
            .Selector(".ai-config-title")
                .Property("font-size", "24px")
                .Property("font-weight", "bold")
                .Property("color", "var(--text-primary)")
                .Property("margin", "0 0 8px 0")
            .EndSelector()
            .Selector(".ai-config-subtitle")
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
                .Property("margin", "0 0 20px 0")
            .EndSelector()
            .Selector(".ai-config-form")
                .Property("max-width", "600px")
            .EndSelector()
            .Selector(".toggle-row")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "10px")
                .Property("margin-bottom", "20px")
                .Property("padding", "12px 16px")
                .Property("background", "var(--bg-card)")
                .Property("border-radius", "8px")
                .Property("border", "1px solid var(--border)")
            .EndSelector()
            .Selector(".toggle-label")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "8px")
                .Property("cursor", "pointer")
                .Property("font-size", "14px")
            .EndSelector()
            .Selector(".toggle-label input")
                .Property("width", "18px")
                .Property("height", "18px")
                .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".toggle-text")
                .Property("font-size", "14px")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".form-group")
                .Property("margin-bottom", "16px")
            .EndSelector()
            .Selector(".form-group label")
                .Property("display", "block")
                .Property("margin-bottom", "6px")
                .Property("font-size", "13px")
                .Property("font-weight", "500")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".form-group select")
                .Property("width", "100%")
                .Property("padding", "10px 14px")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-primary)")
                .Property("font-size", "14px")
                .Property("outline", "none")
            .EndSelector()
            .Selector(".form-group select:focus")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".ai-config-field")
                .Property("margin-bottom", "12px")
            .EndSelector()
            .Selector(".ai-config-field label")
                .Property("display", "block")
                .Property("font-size", "13px")
                .Property("font-weight", "500")
                .Property("color", "var(--text-secondary)")
                .Property("margin-bottom", "4px")
            .EndSelector()
            .Selector(".ai-config-field input")
                .Property("width", "100%")
                .Property("padding", "10px 14px")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-primary)")
                .Property("font-size", "14px")
                .Property("outline", "none")
            .EndSelector()
            .Selector(".ai-config-field input:focus")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".ai-config-field select")
                .Property("width", "100%")
                .Property("padding", "10px 14px")
                .Property("border", "1px solid var(--border)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-primary)")
                .Property("font-size", "14px")
                .Property("outline", "none")
                .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".ai-config-field select:focus")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".form-actions")
                .Property("margin-top", "20px")
                .Property("display", "flex")
                .Property("gap", "10px")
            .EndSelector()
            .Selector(".alert")
                .Property("padding", "12px 16px")
                .Property("border-radius", "6px")
                .Property("margin-bottom", "16px")
                .Property("font-size", "14px")
                .Property("display", "none")
            .EndSelector()
            .Selector(".alert-success")
                .Property("background", "rgba(107,203,119,0.15)")
                .Property("color", "var(--accent-success)")
                .Property("border", "1px solid rgba(107,203,119,0.3)")
            .EndSelector()
            .Selector(".alert-error")
                .Property("background", "rgba(255,107,107,0.15)")
                .Property("color", "var(--accent-error)")
                .Property("border", "1px solid rgba(255,107,107,0.3)")
            .EndSelector();
    }

    private static JsSyntax GetScripts(BeingAIConfigViewModel vm)
    {
        var js = Js.Block();

        // Build aiConfigMetadata via reflection (same style as InitController)
        var aiConfigData = Js.Obj();
        var currentLanguage = Enum.TryParse<Language>(vm.Localization.LanguageCode.Replace("-", ""), ignoreCase: true, out var lang)
            ? lang
            : ((DefaultConfigData)Config.Instance.Data).Language;

        var allFactoryTypes = AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly => assembly.GetName().Name?.StartsWith("SiliconLife") == true)
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IAIClientFactory).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .ToList();

        foreach (var factoryType in allFactoryTypes)
        {
            try
            {
                var factory = (IAIClientFactory)Activator.CreateInstance(factoryType)!;
                var metadata = factory.GetConfigKeysMetadata(currentLanguage);
                var configObj = Js.Obj();
                foreach (var kvp in metadata)
                {
                    var currentConfig = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(vm.CurrentAIConfigJson) && vm.CurrentAIConfigJson != "{}")
                    {
                        try
                        {
                            var parsed = JsonSerializer.Deserialize<Dictionary<string, object>>(vm.CurrentAIConfigJson);
                            if (parsed != null)
                            {
                                foreach (var cfg in parsed)
                                    currentConfig[cfg.Key] = cfg.Value;
                            }
                        }
                        catch { }
                    }

                    var options = factory.GetConfigKeyOptions(kvp.Key, currentConfig, currentLanguage);
                    var fieldObj = Js.Obj().Prop(() => "label", () => Js.Str(() => kvp.Value));
                    if (options != null && options.Count > 0)
                    {
                        var optionsObj = Js.Obj();
                        foreach (var opt in options)
                            optionsObj.Prop(() => opt.Key, () => Js.Str(() => opt.Value));
                        fieldObj.Prop(() => "options", () => optionsObj);
                    }
                    configObj.Prop(() => kvp.Key, () => fieldObj);
                }
                aiConfigData.Prop(() => factoryType.Name, () => configObj);
            }
            catch
            {
                // Skip factories that cannot be instantiated
            }
        }
        js.Add(() => Js.Const(() => "aiConfigMetadata", () => aiConfigData));

        // Store current config values for initialization
        js.Add(() => Js.Const(() => "currentConfigValues", () => Js.Str(() => vm.CurrentAIConfigJson)));
        js.Add(() => Js.Const(() => "beingId", () => Js.Str(() => vm.BeingId.ToString())));
        js.Add(() => Js.Const(() => "hasIndependentConfig", () => Js.Bool(() => vm.HasIndependentConfig)));

        // getCurrentAIConfigValues function
        var getCurrentValuesBody = Js.Block()
            .Add(() => Js.Const(() => "values", () => Js.Obj()))
            .Add(() => Js.Const(() => "inputs", () => Js.Id(() => "document").Call(() => "querySelectorAll", () => Js.Str(() => "#aiConfigFields input, #aiConfigFields select"))))
            .Add(() => Js.Id(() => "inputs").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "input" }, () => Js.Block()
                .Add(() => Js.Assign(() => Js.Id(() => "values").Index(() => Js.Id(() => "input").Prop(() => "name").Call(() => "substring", () => Js.Num(() => "3"))), () => Js.Id(() => "input").Prop(() => "value")))
            )).Stmt())
            .Add(() => Js.Return(() => Js.Id(() => "values")));
        js.Add(() => Js.Func(() => "getCurrentAIConfigValues", () => new List<string>(), () => getCurrentValuesBody));

        // onClientTypeChange function
        var onClientTypeChangeBody = Js.Block()
            .Add(() => Js.Const(() => "metadata", () => Js.Id(() => "aiConfigMetadata").Index(() => Js.Id(() => "clientType"))))
            .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "aiConfigFields"))))
            .Add(() => Js.Assign(() => Js.Id(() => "container").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "metadata").Op(() => "==", () => Js.Null()), new List<JsSyntax> { Js.Return(() => Js.Str(() => "")) }) }
            }));

        // Parse current config values from JSON
        onClientTypeChangeBody.Add(() => Js.Let(() => "currentValues", () => Js.Obj()));
        onClientTypeChangeBody.Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
        {
            { (Js.Id(() => "currentConfigValues").Op(() => "!==", () => Js.Str(() => "{}")), new List<JsSyntax> { Js.Assign(() => Js.Id(() => "currentValues"), () => Js.Id(() => "JSON").Call(() => "parse", () => Js.Id(() => "currentConfigValues"))) }) }
        }));

        // forEach loop body - render select or input based on whether options exist
        var forEachBody = Js.Block()
            .Add(() => Js.Const(() => "fieldMeta", () => Js.Id(() => "metadata").Index(() => Js.Id(() => "key"))))
            .Add(() => Js.Const(() => "label", () => Js.Id(() => "fieldMeta").Prop(() => "label")))
            .Add(() => Js.Const(() => "value", () => Js.Id(() => "currentValues").Index(() => Js.Id(() => "key")).Op(() => "||", () => (JsSyntax)Js.Str(() => ""))))
            .Add(() => Js.Const(() => "div", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "div").Prop(() => "className"), () => Js.Str(() => "ai-config-field")))
            .Add(() => Js.Const(() => "labelEl", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "label"))))
            .Add(() => Js.Assign(() => Js.Id(() => "labelEl").Prop(() => "htmlFor"), () => Js.Str(() => "ai_").Op(() => "+", () => (JsSyntax)Js.Id(() => "key"))))
            .Add(() => Js.Assign(() => Js.Id(() => "labelEl").Prop(() => "textContent"), () => Js.Id(() => "label")))
            .Add(() => Js.Id(() => "div").Call(() => "appendChild", () => Js.Id(() => "labelEl")).Stmt());

        // If has options, render select dropdown; otherwise render input text box
        var optionForEachBody = Js.Block()
            .Add(() => Js.Const(() => "option", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "option"))))
            .Add(() => Js.Assign(() => Js.Id(() => "option").Prop(() => "value"), () => Js.Id(() => "optKey")))
            .Add(() => Js.Assign(() => Js.Id(() => "option").Prop(() => "textContent"), () => Js.Id(() => "fieldMeta").Prop(() => "options").Index(() => Js.Id(() => "optKey"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "optKey").Op(() => "===", () => (JsSyntax)Js.Id(() => "value")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "option").Prop(() => "selected"), () => Js.Bool(() => true))
                    })
                }
            }))
            .Add(() => Js.Id(() => "select").Call(() => "appendChild", () => Js.Id(() => "option")).Stmt());

        var selectRenderingBody = Js.Block()
            .Add(() => Js.Const(() => "select", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "select"))))
            .Add(() => Js.Assign(() => Js.Id(() => "select").Prop(() => "name"), () => Js.Str(() => "ai_").Op(() => "+", () => (JsSyntax)Js.Id(() => "key"))))
            .Add(() => Js.Assign(() => Js.Id(() => "select").Prop(() => "id"), () => Js.Str(() => "ai_").Op(() => "+", () => (JsSyntax)Js.Id(() => "key"))))
            .Add(() => Js.Id(() => "Object").Call(() => "keys", () => Js.Id(() => "fieldMeta").Prop(() => "options")).Call(() => "forEach", () => Js.Arrow(() => new List<string> { "optKey" }, () => optionForEachBody)).Stmt())
            .Add(() => Js.Id(() => "div").Call(() => "appendChild", () => Js.Id(() => "select")).Stmt());

        var inputRenderingBody = Js.Block()
            .Add(() => Js.Const(() => "input", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "input"))))
            .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "type"), () => Js.Str(() => "text")))
            .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "name"), () => Js.Str(() => "ai_").Op(() => "+", () => (JsSyntax)Js.Id(() => "key"))))
            .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "id"), () => Js.Str(() => "ai_").Op(() => "+", () => (JsSyntax)Js.Id(() => "key"))))
            .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "placeholder"), () => Js.Id(() => "label")))
            .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "value"), () => Js.Id(() => "value")))
            .Add(() => Js.Id(() => "div").Call(() => "appendChild", () => Js.Id(() => "input")).Stmt());

        forEachBody.Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
        {
            { (Js.Id(() => "fieldMeta").Prop(() => "options"), new List<JsSyntax> { selectRenderingBody }) },
            { (null, new List<JsSyntax> { inputRenderingBody }) }
        }));

        forEachBody.Add(() => Js.Id(() => "container").Call(() => "appendChild", () => Js.Id(() => "div")).Stmt());

        onClientTypeChangeBody.Add(() => Js.Id(() => "Object").Call(() => "keys", () => Js.Id(() => "metadata")).Call(() => "forEach", () => Js.Arrow(() => new List<string> { "key" }, () => forEachBody)).Stmt());

        // Attach listeners for DashScope dynamic model loading + immediate fetch
        var attachListenersBody = Js.Block()
            .Add(() => Js.Const(() => "apiKeyEl", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "ai_apiKey"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)> { { (Js.Id(() => "apiKeyEl"), new List<JsSyntax> { Js.Id(() => "apiKeyEl").Call(() => "addEventListener", () => Js.Str(() => "change"), () => Js.Id(() => "updateDashScopeModels")).Stmt() }) } }))
            .Add(() => Js.Const(() => "regionEl", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "ai_region"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)> { { (Js.Id(() => "regionEl"), new List<JsSyntax> { Js.Id(() => "regionEl").Call(() => "addEventListener", () => Js.Str(() => "change"), () => Js.Id(() => "updateDashScopeModels")).Stmt() }) } }))
            .Add(() => Js.Id(() => "updateDashScopeModels").Invoke().Stmt());

        onClientTypeChangeBody.Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)> { { (Js.Id(() => "clientType").Op(() => "===", () => (JsSyntax)Js.Str(() => "DashScopeClientFactory")), new List<JsSyntax> { attachListenersBody }) } }));

        js.Add(() => Js.Func(() => "onClientTypeChange", () => new List<string> { "clientType" }, () => onClientTypeChangeBody));

        // updateDashScopeModels function
        var updateModelsBody = Js.Block()
            .Add(() => Js.Const(() => "apiKeyEl", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "ai_apiKey"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)> { { (Js.Id(() => "apiKeyEl").Not(), new List<JsSyntax> { Js.Return(() => Js.Str(() => "")) }) } }))
            .Add(() => Js.Const(() => "apiKey", () => Js.Id(() => "apiKeyEl").Prop(() => "value")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)> { { (Js.Id(() => "apiKey").Not(), new List<JsSyntax> { Js.Return(() => Js.Str(() => "")) }) } }))
            .Add(() => Js.Const(() => "regionEl", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "ai_region"))))
            .Add(() => Js.Let(() => "region", () => Js.Str(() => "")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)> { { (Js.Id(() => "regionEl"), new List<JsSyntax> { Js.Assign(() => Js.Id(() => "region"), () => Js.Id(() => "regionEl").Prop(() => "value")) }) } }))
            .Add(() => Js.Const(() => "url", () => Js.Str(() => "/api/beings/ai-config/models?clientType=DashScopeClientFactory&apiKey=").Op(() => "+", () => (JsSyntax)Js.Id(() => "encodeURIComponent").Invoke(() => Js.Id(() => "apiKey"))).Op(() => "+", () => (JsSyntax)Js.Str(() => "&region=")).Op(() => "+", () => (JsSyntax)Js.Id(() => "encodeURIComponent").Invoke(() => Js.Id(() => "region")))));

        var foreachModelBody = Js.Block()
            .Add(() => Js.Const(() => "opt", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "option"))))
            .Add(() => Js.Assign(() => Js.Id(() => "opt").Prop(() => "value"), () => Js.Id(() => "m").Prop(() => "key")))
            .Add(() => Js.Assign(() => Js.Id(() => "opt").Prop(() => "textContent"), () => Js.Id(() => "m").Prop(() => "value")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)> { { (Js.Id(() => "m").Prop(() => "key").Op(() => "===", () => (JsSyntax)Js.Id(() => "currentValue")), new List<JsSyntax> { Js.Assign(() => Js.Id(() => "opt").Prop(() => "selected"), () => Js.Bool(() => true)) }) } }))
            .Add(() => Js.Id(() => "select").Call(() => "appendChild", () => Js.Id(() => "opt")).Stmt());

        var dataBlock = Js.Block()
            .Add(() => Js.Const(() => "select", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "ai_model"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)> { { (Js.Id(() => "select").Not(), new List<JsSyntax> { Js.Return(() => Js.Str(() => "")) }) } }))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)> { { (Js.Id(() => "data").Prop(() => "models").Not(), new List<JsSyntax> { Js.Return(() => Js.Str(() => "")) }) } }))
            .Add(() => Js.Const(() => "currentValue", () => Js.Id(() => "select").Prop(() => "value")))
            .Add(() => Js.Assign(() => Js.Id(() => "select").Prop(() => "innerHTML"), () => Js.Str(() => "")))
            .Add(() => Js.Id(() => "data").Prop(() => "models").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "m" }, () => foreachModelBody)).Stmt());

        var fetchThenArrow = Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json"));
        var fetchThenDataArrow = Js.Arrow(() => new List<string> { "data" }, () => dataBlock);

        updateModelsBody.Add(() => Js.Id(() => "fetch").Invoke(() => Js.Id(() => "url")).Call(() => "then", () => fetchThenArrow).Call(() => "then", () => fetchThenDataArrow).Stmt());

        js.Add(() => Js.Func(() => "updateDashScopeModels", () => new List<string>(), () => updateModelsBody));

        // toggleIndependent function
        var toggleBody = Js.Block()
            .Add(() => Js.Const(() => "checkbox", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "useIndependent"))))
            .Add(() => Js.Const(() => "form", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "aiConfigForm"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "checkbox").Prop(() => "checked"), new List<JsSyntax>
                    {
                        Js.Id(() => "form").Prop(() => "classList").Call(() => "remove", () => Js.Str(() => "disabled")).Stmt(),
                        Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "aiClientTypeSelect")).Prop(() => "disabled"), () => Js.Bool(() => false)),
                        Js.Id(() => "onClientTypeChange").Invoke(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "aiClientTypeSelect")).Prop(() => "value")).Stmt()
                    }
                )},
                { (null, new List<JsSyntax>
                    {
                        Js.Id(() => "form").Prop(() => "classList").Call(() => "add", () => Js.Str(() => "disabled")).Stmt(),
                        Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "aiClientTypeSelect")).Prop(() => "disabled"), () => Js.Bool(() => true)),
                        Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "aiConfigFields")).Prop(() => "innerHTML"), () => Js.Str(() => ""))
                    }
                )}
            }));
        js.Add(() => Js.Func(() => "toggleIndependent", () => new List<string>(), () => toggleBody));

        // saveAIConfig function
        var saveBody = Js.Block()
            .Add(() => Js.Const(() => "checkbox", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "useIndependent"))))
            .Add(() => Js.Const(() => "clientType", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "aiClientTypeSelect")).Prop(() => "value")))
            .Add(() => Js.Const(() => "payload", () => Js.Obj()
                .Prop(() => "beingId", () => Js.Id(() => "beingId"))
                .Prop(() => "useIndependent", () => Js.Id(() => "checkbox").Prop(() => "checked"))
                .Prop(() => "aiClientType", () => Js.Id(() => "clientType"))
                .Prop(() => "aiConfig", () => Js.Id(() => "getCurrentAIConfigValues").Invoke())))
            .Add(() => Js.Id(() => "fetch").Invoke(() => Js.Str(() => "/api/beings/ai-config/save"), () => Js.Obj()
                .Prop(() => "method", () => Js.Str(() => "POST"))
                .Prop(() => "headers", () => Js.Obj().Prop(() => "Content-Type", () => Js.Str(() => "application/json")))
                .Prop(() => "body", () => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Id(() => "payload"))))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "r" }, () => Js.Id(() => "r").Call(() => "json")))
                .Call(() => "then", () => Js.Arrow(() => new List<string> { "data" }, () => Js.Block()
                    .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
                    {
                        { (Js.Id(() => "data").Prop(() => "success"), new List<JsSyntax>
                            {
                                Js.Assign(() => Js.Id(() => "window").Prop(() => "location").Prop(() => "href"), () => Js.Str(() => "/beings"))
                            }
                        )},
                        { (null, new List<JsSyntax>
                            {
                                Js.Assign(() => Js.Id(() => "alert").Invoke(() => Js.Id(() => "data").Prop(() => "error").Op(() => "||", () => (JsSyntax)Js.Str(() => "Save failed"))), () => Js.Id(() => "data").Prop(() => "error"))
                            }
                        )}
                    }))
                )).Stmt());
        js.Add(() => Js.Func(() => "saveAIConfig", () => new List<string>(), () => saveBody));

        // Initialize on page load
        var initBody = Js.Block()
            .Add(() => Js.Const(() => "initialType", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "aiClientTypeSelect")).Prop(() => "value")))
            .Add(() => Js.Id(() => "toggleIndependent").Invoke().Stmt());
        js.Add(() => Js.Id(() => "window").Prop(() => "addEventListener").Invoke(
            () => Js.Str(() => "load"),
            () => Js.Arrow(() => new List<string>(), () => initBody)
        ));

        return js;
    }
}
