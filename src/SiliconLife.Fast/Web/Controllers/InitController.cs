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

using System.Text;
using SiliconLife.Collective;
using SiliconLife.Common.AI;
using SiliconLife.Fast.Help;
using SiliconLife.Fast.Web.Component;
using SiliconLife.Common.Localization;

namespace SiliconLife.Fast.Web;

public class InitController : Controller
{
    private readonly DefaultConfigData _configData;
    private DefaultLocalizationBase _localization;
    private readonly SkinManager _skinManager;
    private readonly Router _router;

    public InitController()
    {
        _configData = (DefaultConfigData)Config.Instance.Data;
        _localization = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(_configData.Language);
        _skinManager = ServiceLocator.Instance.GetService<SkinManager>()!;
        _router = ServiceLocator.Instance.Get<Router>()!;
    }

    public override void Handle()
    {
        if (Request.HttpMethod == "POST")
        {
            HandlePost();
        }
        else if (Request.Url?.AbsolutePath == "/init/browse" || Request.Url?.AbsolutePath == "/init/browse/")
        {
            HandleBrowse();
        }
        else if (Request.Url?.AbsolutePath == "/init/ai-config-metadata" || Request.Url?.AbsolutePath == "/init/ai-config-metadata/")
        {
            HandleGetAIConfigMetadata();
        }
        else
        {
            ResolveLocalizationFromQuery();
            ShowForm();
        }
    }

    private void ResolveLocalizationFromQuery()
    {
        var langParam = GetQueryValue("lang");
        if (!string.IsNullOrEmpty(langParam) && Enum.TryParse<Language>(langParam, ignoreCase: true, out var lang))
        {
            var resolved = LocalizationManager.Instance.GetLocalization(lang) as DefaultLocalizationBase;
            if (resolved != null)
                _localization = resolved;
        }
    }

    private void ShowForm()
    {
        ShowFormInternal(null);
    }

    private void ShowFormWithError(string error)
    {
        ShowFormInternal(error);
    }

    private void ShowFormInternal(string? error)
    {
        var form = new FormComponent()
            .Action("/init")
            .Method("POST")
            .Class("init-form")
            .Attr("onsubmit", "return validateInitForm()");

        // Language form group
        form.Add(BuildLanguageFormGroup());

        // Nickname field
        form.Add(new FormGroupComponent()
            .Add(new LabelComponent().For("nickname").Text(_localization.InitNicknameLabel))
            .Add(new InputComponent()
                .Type("text")
                .Name("nickname")
                .Placeholder(_localization.InitNicknamePlaceholder)
                .Value(_configData.UserNickname)
                .Required()));

        // Curator name field
        form.Add(new FormGroupComponent()
            .Add(new LabelComponent().For("curatorName").Text(_localization.InitCuratorNameLabel))
            .Add(new InputComponent()
                .Type("text")
                .Name("curatorName")
                .Placeholder(_localization.InitCuratorNamePlaceholder)
                .Required()));

        // AI client type select
        form.Add(new FormGroupComponent()
            .Add(new LabelComponent().For("aiClientType").Text(_localization.InitAIClientTypeLabel))
            .Add(BuildAIClientTypeSelect()));

        // AI client help link container
        form.Add(new DivComponent().Id("aiClientHelpContainer").Class("form-group")
            .Add(new DivComponent().Id("aiClientHelpLink")));

        // AI config fields container
        form.Add(new DivComponent().Id("aiConfigContainer").Class("form-group")
            .Add(new DivComponent().Id("aiConfigFields")));

        // Data directory field
        form.Add(BuildDataDirectoryFormGroup());

        // Skin selection
        form.Add(BuildSkinFormGroup());

        // Submit button
        form.Add(new DivComponent().Class("form-actions")
            .Add(new ButtonComponent()
                .Text(_localization.InitSubmitButton)
                .Type("submit")));

        // Build page HTML
        var page = new DivComponent().Class("init-container")
            .Add(new DivComponent().Class("init-card")
                .Add(new HeadingComponent("h1").Text("Silicon Life Collective"))
                .Add(new PComponent(_localization.InitDescription))
                .Add(error != null ? new DivComponent().Class("form-error")
                    .Add(new PComponent(error)) : null)
                .Add(form)
                .Add(new DivComponent().Class("init-footer")
                    .Add(new PComponent(_localization.InitFooterHint))
                    .Add(new AComponent(_localization.InitHelpLink)
                        .Href("/help")
                        .Class("init-help-link"))));

        var html = H.DocType() +
            H.Html()
                .Add(H.Head()
                    .Add(H.Meta().Attr("charset", "utf-8"))
                    .Add(H.Meta().Attr("name", "viewport").Attr("content", "width=device-width, initial-scale=1"))
                    .Add(H.Title($"{_localization.InitPageTitle} - Silicon Life Collective"))
                    .Add(H.Style(GetStyles()))
                    .Add(H.Script(GetSkinSwitchScript().Build())))
                .Add(H.Body()
                    .AddRendered(page.Render())
                )
                .Build();

        RenderHtml(html);
    }

    private ComponentBase BuildLanguageFormGroup()
    {
        var currentLang = _localization.LanguageCode;
        var select = new SelectComponent().Name("language").Id("languageSelect")
            .Attr("data-current", currentLang)
            .Attr("onchange", "switchLanguage(this.value)");

        foreach (var lang in LocalizationManager.Instance.GetRegisteredLanguages())
        {
            var loc = LocalizationManager.Instance.GetLocalization(lang);
            var optionText = loc.LanguageName;
            var optionValue = lang.ToString();
            var isSelected = loc.LanguageCode == currentLang;
            select.AddOption(optionValue, optionText);
            if (isSelected)
            {
                select.Selected(optionValue);
            }
        }

        var switchBtn = new ButtonComponent()
            .Text(_localization.InitLanguageSwitchBtn)
            .Type("button")
            .Class("lang-switch-btn")
            .Style("display:none;")
            .Attr("onclick", "applyLanguage()");

        return new FormGroupComponent()
            .Add(new LabelComponent().For("language").Text(_localization.InitLanguageLabel))
            .Add(new DivComponent().Class("lang-selector-row")
                .Add(select)
                .Add(switchBtn));
    }

    private ComponentBase BuildDataDirectoryFormGroup()
    {
        var browseBtn = new ButtonComponent()
            .Text(_localization.InitDataDirectoryBrowse)
            .Type("button")
            .Class("dir-browse-btn")
            .Attr("onclick", "openDirBrowser()");

        var inputRow = new DivComponent().Class("dir-input-row")
            .Add(new InputComponent()
                .Type("text")
                .Name("dataDirectory")
                .Placeholder(_localization.InitDataDirectoryPlaceholder)
                .Value(_configData.DataDirectory.FullName)
                .Id("dataDirInput"))
            .Add(browseBtn);

        var browser = new DivComponent().Id("dirBrowser").Class("dir-browser").Style("display:none;");

        return new FormGroupComponent()
            .Add(new LabelComponent().For("dataDirectory").Text(_localization.InitDataDirectoryLabel))
            .Add(inputRow)
            .Add(browser);
    }

    private ComponentBase BuildSkinFormGroup()
    {
        var skins = _skinManager.GetAvailableSkins()
            .Select(c => (Code: c, Skin: _skinManager.GetSkin(c)!))
            .OrderBy(s => s.Code)
            .ToList();
        var currentSkin = _configData.WebSkin ?? skins.FirstOrDefault().Code;

        var skinGrid = new DivComponent().Class("skin-grid");
        foreach (var (code, skin) in skins)
        {
            var p = skin.PreviewInfo;
            var gradient = $"linear-gradient(135deg,{p.BackgroundColor} 0%,{p.CardColor} 100%)";
            var isSelected = code == currentSkin;

            var card = new DivComponent()
                .Class("skin-option" + (isSelected ? " selected" : ""))
                .Attr("data-skin", code)
                .Attr("onclick", $"selectSkin('{code}')")
                .Attr("style", $"border-color:{p.BorderColor};background:{gradient};color:{p.TextColor};")
                .Add(new SpanComponent(p.Icon).Class("skin-icon"))
                .Add(new SpanComponent(skin.Name).Class("skin-desc"))
                .Add(new SpanComponent(p.Description).Class("skin-desc"))
                .Add(new DivComponent()
                    .Class("skin-colors")
                    .Add(new SpanComponent().Class("color-dot").Attr("style", $"background:{p.BackgroundColor};"))
                    .Add(new SpanComponent().Class("color-dot").Attr("style", $"background:{p.CardColor};"))
                    .Add(new SpanComponent().Class("color-dot").Attr("style", $"background:{p.AccentColor};")));

            skinGrid.Add(card);
        }

        var previewP = _skinManager.GetSkin(currentSkin)?.PreviewInfo ?? skins.First().Skin.PreviewInfo;
        var previewSection = new DivComponent().Id("preview").Class("skin-preview-box")
            .Attr("style", $"background:{previewP.BackgroundColor};color:{previewP.TextColor};border-color:{previewP.BorderColor};")
            .Add(new HeadingComponent("h3").Text("Preview").Class("skin-preview-title"))
            .Add(new DivComponent().Class("preview-inner")
                .Add(new DivComponent()
                    .Class("preview-card")
                    .Attr("style", $"background:{previewP.CardColor};border-color:{previewP.BorderColor};")
                    .Add(new HeadingComponent("h4").Text(_localization.InitSkinPreviewCardTitle))
                    .Add(new PComponent(_localization.InitSkinPreviewCardContent)))
                .Add(new DivComponent().Class("preview-btns")
                    .Add(new ButtonComponent()
                        .Text(_localization.InitSkinPreviewPrimaryBtn)
                        .Type("button")
                        .Class("preview-btn")
                        .Attr("style", $"background:{previewP.AccentColor};color:#fff;"))
                    .Add(new ButtonComponent()
                        .Text(_localization.InitSkinPreviewSecondaryBtn)
                        .Type("button")
                        .Class("preview-btn")
                        .Attr("style", $"background:{previewP.AccentColor};opacity:0.6;color:#fff;"))))
            .Add(new InputComponent()
                .Type("hidden")
                .Name("webSkin")
                .Value(currentSkin)
                .Id("skinInput"));

        return new FormGroupComponent()
            .Add(new LabelComponent().For("webSkin").Text(_localization.InitSkinLabel))
            .Add(skinGrid)
            .Add(new DivComponent().Class("skin-preview-section")
                .Add(previewSection));
    }

    private ComponentBase BuildAIClientTypeSelect()
    {
        // Auto-discover all types that implement IAIClientFactory via reflection
        var factoryTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IAIClientFactory).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .ToList();

        var select = new SelectComponent()
            .Name("aiClientType")
            .Id("aiClientTypeSelect")
            .Attr("onchange", "onClientTypeChange(this.value)");

        string? currentType = _configData.AIClientType;

        foreach (var type in factoryTypes)
        {
            try
            {
                // Create factory instance to get metadata
                var factory = (IAIClientFactory)Activator.CreateInstance(type)!;
                var typeName = type.Name.Replace("Factory", ""); // OllamaClientFactory -> OllamaClient
                
                // Use current _localization to get localized display name
                var displayName = _localization.GetConfigDisplayName(typeName, out _) ?? typeName;
                
                select.AddOption(type.Name, displayName);
                
                // Set current selected item
                if (type.Name == currentType)
                {
                    select.Selected(type.Name);
                }
            }
            catch
            {
                // Skip types that cannot be instantiated
            }
        }

        // If no type selected, select first one
        if (string.IsNullOrEmpty(currentType) && factoryTypes.Count > 0)
        {
            select.Selected(factoryTypes[0].Name);
        }

        return select;
    }

    private Dictionary<string, string> GetAIClientHelpTopicMapping()
    {
        // Auto-discover all factory types and their help topic IDs
        var factoryTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IAIClientFactory).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .ToList();

        var helpMapping = new Dictionary<string, string>();
        foreach (var type in factoryTypes)
        {
            try
            {
                var factory = Activator.CreateInstance(type);
                if (factory is IAIClientFactoryHelp helpProvider)
                {
                    var helpTopicId = helpProvider.GetHelpTopicId();
                    if (!string.IsNullOrEmpty(helpTopicId))
                    {
                        helpMapping[type.Name] = helpTopicId;
                    }
                }
            }
            catch
            {
                // Skip factories that cannot be instantiated
            }
        }

        return helpMapping;
    }

    private IAIClientFactory CreateFactoryByType(string clientType)
    {
        // Dynamically create factory instance via reflection
        var factoryType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .FirstOrDefault(type => type.Name == clientType && typeof(IAIClientFactory).IsAssignableFrom(type));

        if (factoryType == null)
        {
            throw new NotSupportedException($"AI client factory type '{clientType}' not found");
        }

        return (IAIClientFactory)Activator.CreateInstance(factoryType)!;
    }

    private void HandleBrowse()
    {
        var dir = GetQueryValue("dir");
        string? basePath;
        bool showDrives = false;

        if (string.IsNullOrEmpty(dir) || dir == "/")
        {
            basePath = AppDomain.CurrentDomain.BaseDirectory;
        }
        else if (dir == "__drives__")
        {
            basePath = null;
            showDrives = true;
        }
        else
        {
            basePath = Path.GetFullPath(dir);
        }

        var result = new List<object>();
        try
        {
            if (showDrives)
            {
                foreach (var drive in DriveInfo.GetDrives()
                    .Where(d => d.IsReady && d.DriveType == DriveType.Fixed || d.DriveType == DriveType.Removable || d.DriveType == DriveType.Network)
                    .OrderBy(d => d.Name))
                {
                    var root = drive.RootDirectory.FullName;
                    result.Add(new { name = drive.Name.TrimEnd('\\'), path = root.Replace('\\', '/'), isParent = false });
                }
            }
            else if (Directory.Exists(basePath))
            {
                var isRoot = Path.GetPathRoot(basePath) == basePath;
                if (!isRoot)
                {
                    var parent = Directory.GetParent(basePath)?.FullName;
                    if (parent != null)
                    {
                        result.Add(new { name = "..", path = parent.Replace('\\', '/'), isParent = true });
                    }
                }
                else
                {
                    result.Add(new { name = "..", path = "__drives__", isParent = true });
                }

                foreach (var d in Directory.GetDirectories(basePath)
                    .OrderBy(d => d)
                    .Take(50))
                {
                    var name = Path.GetFileName(d);
                    result.Add(new { name, path = d.Replace('\\', '/'), isParent = false });
                }
            }
        }
        catch
        {
            // ignore access errors
        }

        RenderJson(new { directories = result, currentPath = basePath?.Replace('\\', '/') ?? "" });
    }

    private void HandleGetAIConfigMetadata()
    {
        var clientType = GetQueryValue("clientType");
        if (string.IsNullOrEmpty(clientType))
        {
            RenderJson(new { success = false, message = "clientType parameter is required" });
            return;
        }

        try
        {
            var factory = CreateFactoryByType(clientType);
            if (factory == null)
            {
                RenderJson(new { success = false, message = $"Factory type '{clientType}' not found" });
                return;
            }

            // Get current language
            var currentLanguage = Enum.TryParse<Language>(_localization.LanguageCode.Replace("-", ""), ignoreCase: true, out var lang)
                ? lang
                : _configData.Language;

            // Get current config values from query string (optional)
            var valuesParam = GetQueryValue("values");
            var currentConfig = new Dictionary<string, object>();
            
            if (!string.IsNullOrEmpty(valuesParam))
            {
                // Parse JSON values from frontend
                try
                {
                    var jsonDoc = System.Text.Json.JsonDocument.Parse(Uri.UnescapeDataString(valuesParam));
                    foreach (var property in jsonDoc.RootElement.EnumerateObject())
                    {
                        // Remove "ai_" prefix if present
                        var key = property.Name.StartsWith("ai_") ? property.Name.Substring(3) : property.Name;
                        currentConfig[key] = property.Value.GetString() ?? "";
                    }
                }
                catch
                {
                    // If parsing fails, fall back to config file values
                    currentConfig = _configData.AIConfig.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);
                }
            }
            else
            {
                // Use values from config file
                currentConfig = _configData.AIConfig.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);
            }

            // Get config keys metadata
            var configKeys = factory.GetConfigKeysMetadata(currentLanguage);
            var fields = new List<object>();

            // Build complete field data with current values
            foreach (var kvp in configKeys)
            {
                var configKey = kvp.Key;
                var label = kvp.Value;
                var currentValue = currentConfig.ContainsKey(configKey) ? currentConfig[configKey].ToString() : "";
                var options = factory.GetConfigKeyOptions(configKey, currentConfig, currentLanguage);

                var fieldData = new Dictionary<string, object>
                {
                    ["key"] = configKey,
                    ["label"] = label,
                    ["value"] = currentValue
                };

                if (options != null && options.Count > 0)
                {
                    fieldData["options"] = options;
                }

                fields.Add(fieldData);
            }

            RenderJson(new
            {
                success = true,
                clientType = clientType,
                fields = fields
            });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, message = ex.Message });
        }
    }

    private void HandlePost()
    {
        var form = ParseFormData();

        var nickname = form.GetValueOrDefault("nickname", "").Trim();
        if (string.IsNullOrEmpty(nickname))
        {
            ShowFormWithError(_localization.InitNicknameRequiredError);
            return;
        }

        var dataDir = form.GetValueOrDefault("dataDirectory", "").Trim();
        if (string.IsNullOrEmpty(dataDir))
        {
            ShowFormWithError(_localization.InitDataDirectoryRequiredError);
            return;
        }

        _configData.UserNickname = nickname;
        _configData.DataDirectory = new DirectoryInfo(dataDir);

        var curatorName = form.GetValueOrDefault("curatorName", "").Trim();
        if (string.IsNullOrEmpty(curatorName))
        {
            ShowFormWithError(_localization.InitCuratorNameRequiredError);
            return;
        }

        var aiClientType = form.GetValueOrDefault("aiClientType", "OllamaClient").Trim();
        _configData.AIClientType = string.IsNullOrEmpty(aiClientType) ? "OllamaClient" : aiClientType;

        // Dynamically save AI configuration fields (fields starting with ai_)
        foreach (var kvp in form)
        {
            if (kvp.Key.StartsWith("ai_") && kvp.Value.Length > 0)
            {
                var configKey = kvp.Key.Substring(3); // Remove "ai_" prefix
                _configData.AIConfig[configKey] = kvp.Value.Trim();
            }
        }

        var skin = form.GetValueOrDefault("webSkin", "").Trim();
        _configData.WebSkin = string.IsNullOrEmpty(skin) ? null! : skin;

        var language = form.GetValueOrDefault("language", "").Trim();
        if (!string.IsNullOrEmpty(language) && Enum.TryParse<Language>(language, ignoreCase: true, out var lang))
        {
            _configData.Language = lang;
        }

        _configData.SaveConfig();
        _router.NotifyInitialized(curatorName);

        Redirect("/");
    }

    private JsSyntax GetSkinSwitchScript()
    {
        var skins = _skinManager.GetAvailableSkins()
            .Select(c => (Code: c, Skin: _skinManager.GetSkin(c)!))
            .OrderBy(s => s.Code)
            .ToList();

        var js = Js.Block()
            .Add(() => Js.Const(() => "skinData", () => Js.Obj()));

        // Get help localization for current language
        var helpLocalization = HelpLocalizationFactory.Create(_configData.Language);
        
        // Get AI client help topic mapping
        var helpTopicMapping = GetAIClientHelpTopicMapping();
        
        // Build help titles mapping from HelpTopics
        var helpTitlesData = Js.Obj();
        foreach (var kvp in helpTopicMapping)
        {
            var topicId = kvp.Value;
            var topic = HelpTopics.GetById(topicId);
            if (topic != null && helpLocalization != null)
            {
                var titleProperty = helpLocalization.GetType().GetProperty(topic.TitlePropertyName);
                var title = titleProperty?.GetValue(helpLocalization) as string ?? topicId;
                helpTitlesData.Prop(() => kvp.Key, () => Js.Str(() => title));
            }
        }

        foreach (var (skinCode, skin) in skins)
        {
            var p = skin.PreviewInfo;
            var code = skinCode;
            var bgColor = p.BackgroundColor;
            var cardColor = p.CardColor;
            var accentColor = p.AccentColor;
            var textColor = p.TextColor;
            var borderColor = p.BorderColor;
            var obj = Js.Obj()
                .Prop(() => "bg", () => Js.Str(() => bgColor))
                .Prop(() => "card", () => Js.Str(() => cardColor))
                .Prop(() => "accent", () => Js.Str(() => accentColor))
                .Prop(() => "text", () => Js.Str(() => textColor))
                .Prop(() => "border", () => Js.Str(() => borderColor));
            js.Add(() => Js.Assign(() => Js.Id(() => "skinData").Index(() => Js.Str(() => code)), () => obj));
        }

        var removeSelectedArrow = Js.Arrow(() => new List<string> { "el" }, () => Js.Id(() => "el").Prop(() => "classList").Call(() => "remove", () => Js.Str(() => "selected")));
        var selectSkinBody = Js.Block()
            .Add(() => Js.Id(() => "document").Call(() => "querySelectorAll", () => Js.Str(() => ".skin-option")).Call(() => "forEach", () => removeSelectedArrow).Stmt())
            .Add(() => Js.Const(() => "selectedEl", () => Js.Id(() => "document").Call(() => "querySelector", () => Js.Str(() => "[data-skin='").Op(() => "+", () => (JsSyntax)Js.Id(() => "code")).Op(() => "+", () => (JsSyntax)Js.Str(() => "']")))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "selectedEl"), new List<JsSyntax>
                    {
                        Js.Id(() => "selectedEl").Prop(() => "classList").Call(() => "add", () => Js.Str(() => "selected")).Stmt()
                    })
                }
            }))
            .Add(() => Js.Assign(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "skinInput")).Prop(() => "value"), () => Js.Id(() => "code")))
            .Add(() => Js.Id(() => "updatePreview").Invoke(() => Js.Id(() => "code")).Stmt());
        js.Add(() => Js.Func(() => "selectSkin", () => new List<string> { "code" }, () => selectSkinBody));

        var updateCardArrow = Js.Arrow(() => new List<string> { "c" }, () => Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "c").Prop(() => "style").Prop(() => "background"), () => Js.Id(() => "s").Prop(() => "card")))
            .Add(() => Js.Assign(() => Js.Id(() => "c").Prop(() => "style").Prop(() => "borderColor"), () => Js.Id(() => "s").Prop(() => "border"))));
        var updateBtnArrow = Js.Arrow(() => new List<string> { "b" }, () => Js.Assign(() => Js.Id(() => "b").Prop(() => "style").Prop(() => "background"), () => Js.Id(() => "s").Prop(() => "accent")));
        var updatePreviewBody = Js.Block()
            .Add(() => Js.Const(() => "s", () => Js.Id(() => "skinData").Index(() => Js.Id(() => "code"))))
            .Add(() => Js.Const(() => "pv", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "preview"))))
            .Add(() => Js.Assign(() => Js.Id(() => "pv").Prop(() => "style").Prop(() => "background"), () => Js.Id(() => "s").Prop(() => "bg")))
            .Add(() => Js.Assign(() => Js.Id(() => "pv").Prop(() => "style").Prop(() => "color"), () => Js.Id(() => "s").Prop(() => "text")))
            .Add(() => Js.Assign(() => Js.Id(() => "pv").Prop(() => "style").Prop(() => "borderColor"), () => Js.Id(() => "s").Prop(() => "border")))
            .Add(() => Js.Id(() => "pv").Call(() => "querySelectorAll", () => Js.Str(() => ".preview-card")).Call(() => "forEach", () => updateCardArrow).Stmt())
            .Add(() => Js.Id(() => "pv").Call(() => "querySelectorAll", () => Js.Str(() => ".preview-btn")).Call(() => "forEach", () => updateBtnArrow).Stmt());
        js.Add(() => Js.Func(() => "updatePreview", () => new List<string> { "code" }, () => updatePreviewBody));

        var validateBody = Js.Block()
            .Add(() => Js.Const(() => "n", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "nickname"))))
            .Add(() => Js.Const(() => "d", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dataDirInput"))))
            .Add(() => Js.Const(() => "c", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "curatorName"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "n").Prop(() => "value").Call(() => "trim").Not(), new List<JsSyntax> { Js.Id(() => "n").Call(() => "focus").Stmt(), Js.Return(() => Js.Bool(() => false)) }) },
                { (Js.Id(() => "c").Prop(() => "value").Call(() => "trim").Not(), new List<JsSyntax> { Js.Id(() => "c").Call(() => "focus").Stmt(), Js.Return(() => Js.Bool(() => false)) }) },
                { (Js.Id(() => "d").Prop(() => "value").Call(() => "trim").Not(), new List<JsSyntax> { Js.Id(() => "d").Call(() => "focus").Stmt(), Js.Return(() => Js.Bool(() => false)) }) },
                { (null, new List<JsSyntax> { Js.Return(() => Js.Bool(() => true)) }) }
            }));
        js.Add(() => Js.Func(() => "validateInitForm", () => new List<string>(), () => validateBody));

        js.Add(() => Js.Let(() => "dirBrowserOpen", () => Js.Bool(() => false)));

        var openDirBody = Js.Block()
            .Add(() => Js.Const(() => "db", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dirBrowser"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "dirBrowserOpen"), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "db").Prop(() => "style").Prop(() => "display"), () => Js.Str(() => "none")),
                        Js.Assign(() => Js.Id(() => "dirBrowserOpen"), () => Js.Bool(() => false)),
                        Js.Return(() => Js.Str(() => ""))
                    })
                }
            }))
            .Add(() => Js.Assign(() => Js.Id(() => "dirBrowserOpen"), () => Js.Bool(() => true)))
            .Add(() => Js.Assign(() => Js.Id(() => "db").Prop(() => "style").Prop(() => "display"), () => Js.Str(() => "block")))
            .Add(() => Js.Id(() => "browseDir").Invoke(() => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dataDirInput")).Prop(() => "value").Op(() => "||", () => (JsSyntax)Js.Str(() => "/"))).Stmt());
        js.Add(() => Js.Func(() => "openDirBrowser", () => new List<string>(), () => openDirBody));

        var browseDirBody = Js.Block()
            .Add(() => Js.Const(() => "url", () => Js.Str(() => "/init/browse?dir=").Op(() => "+", () => (JsSyntax)Js.Id(() => "encodeURIComponent").Invoke(() => Js.Id(() => "path")))))
            .Add(() => Js.Const(() => "db", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "dirBrowser"))));

        var escapeHtml = Js.Id(() => "s")
            .Call(() => "replace", () => Js.Regex(() => @"\&", () => "g"), () => Js.Str(() => "&amp;"))
            .Call(() => "replace", () => Js.Regex(() => "\"", () => "g"), () => Js.Str(() => "&quot;"))
            .Call(() => "replace", () => Js.Regex(() => "<", () => "g"), () => Js.Str(() => "&lt;"))
            .Call(() => "replace", () => Js.Regex(() => ">", () => "g"), () => Js.Str(() => "&gt;"));
        browseDirBody.Add(() => Js.Const(() => "h", () => Js.Arrow(() => new List<string> { "s" }, () => escapeHtml)));

        var forEachBlock = Js.Block()
            .Add(() => Js.Assign(() => Js.Id(() => "html"), () => Js.Id(() => "html").Call(() => "concat", () => Js.Str(() => "<div class=\"dir-item\" onclick=\"browseDir('")).Call(() => "concat", () => Js.Id(() => "d").Prop(() => "path")).Call(() => "concat", () => Js.Str(() => "')\">"))))
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

        var switchLangBody = Js.Block()
            .Add(() => Js.Const(() => "current", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "languageSelect")).Prop(() => "dataset").Prop(() => "current")))
            .Add(() => Js.Const(() => "btn", () => Js.Id(() => "document").Call(() => "querySelector", () => Js.Str(() => ".lang-switch-btn"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "lang").Op(() => "===", () => (JsSyntax)Js.Id(() => "current")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "btn").Prop(() => "style").Prop(() => "display"), () => Js.Str(() => "none"))
                    })
                },
                { (null, new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "btn").Prop(() => "style").Prop(() => "display"), () => Js.Str(() => "inline-block"))
                    })
                }
            }));
        js.Add(() => Js.Func(() => "switchLanguage", () => new List<string> { "lang" }, () => switchLangBody));

        var applyLangBody = Js.Block()
            .Add(() => Js.Const(() => "selectedLang", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "languageSelect")).Prop(() => "value")))
            .Add(() => Js.Assign(() => Js.Id(() => "window").Prop(() => "location").Prop(() => "href"), () => Js.Str(() => "/init?lang=").Op(() => "+", () => (JsSyntax)Js.Id(() => "selectedLang"))));
        js.Add(() => Js.Func(() => "applyLanguage", () => new List<string>(), () => applyLangBody));

        // AI client type switching logic - dynamically discover all factories
        var aiConfigData = Js.Obj();
        
        // Get current language (infer from _localization.LanguageCode, need to remove hyphen)
        var currentLanguage = Enum.TryParse<Language>(_localization.LanguageCode.Replace("-", ""), ignoreCase: true, out var lang) 
            ? lang 
            : _configData.Language; // Fallback to config file language if parsing fails
        
        // Get all factory types via reflection and generate metadata
        var allFactoryTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IAIClientFactory).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .ToList();
        
        foreach (var factoryType in allFactoryTypes)
        {
            try
            {
                var factory = (IAIClientFactory)Activator.CreateInstance(factoryType)!;
                // Pass current language to factory
                var metadata = factory.GetConfigKeysMetadata(currentLanguage);
                var configObj = Js.Obj();
                foreach (var kvp in metadata)
                {
                    // Get current config value (from _configData.AIConfig)
                    var currentConfig = new Dictionary<string, object>();
                    foreach (var cfg in _configData.AIConfig)
                    {
                        currentConfig[cfg.Key] = cfg.Value;
                    }
                    
                    // Call GetConfigKeyOptions to get options
                    var options = factory.GetConfigKeyOptions(kvp.Key, currentConfig, currentLanguage);
                    
                    // Build field metadata object: contains label and optional options
                    var fieldObj = Js.Obj()
                        .Prop(() => "label", () => Js.Str(() => kvp.Value));
                    
                    if (options != null && options.Count > 0)
                    {
                        var optionsObj = Js.Obj();
                        foreach (var opt in options)
                        {
                            optionsObj.Prop(() => opt.Key, () => Js.Str(() => opt.Value));
                        }
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
        
        // Store initial metadata (for fallback)
        js.Add(() => Js.Const(() => "aiConfigMetadata", () => aiConfigData));
        
        // Add AI help titles mapping
        js.Add(() => Js.Const(() => "aiHelpTitles", () => helpTitlesData));
        
        // Add AI help topics mapping (factory type -> help topic ID)
        var helpTopicsData = Js.Obj();
        foreach (var kvp in helpTopicMapping)
        {
            helpTopicsData.Prop(() => kvp.Key, () => Js.Str(() => kvp.Value));
        }
        js.Add(() => Js.Const(() => "aiClientHelpTopics", () => helpTopicsData));
        
        // Add help prefix text
        js.Add(() => Js.Const(() => "aiHelpPrefix", () => Js.Str(() => _localization.InitAIClientHelpPrefix)));
        
        // Add current language for help links
        var currentLangCode = _localization.LanguageCode.Replace("-", ""); // Remove hyphen for consistency
        js.Add(() => Js.Const(() => "initCurrentLang", () => Js.Str(() => currentLangCode)));

        // getCurrentAIConfigValues function
        var getCurrentValuesBody = Js.Block()
            .Add(() => Js.Const(() => "values", () => Js.Obj()))
            .Add(() => Js.Const(() => "inputs", () => Js.Id(() => "document").Call(() => "querySelectorAll", () => Js.Str(() => "#aiConfigFields input"))))
            .Add(() => Js.Id(() => "inputs").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "input" }, () => Js.Block()
                .Add(() => Js.Assign(() => Js.Id(() => "values").Index(() => Js.Id(() => "input").Prop(() => "name").Call(() => "substring", () => Js.Str(() => "3"))), () => Js.Id(() => "input").Prop(() => "value")))
            )))
            .Add(() => Js.Return(() => Js.Id(() => "values")));
        js.Add(() => Js.Func(() => "getCurrentAIConfigValues", () => new List<string>(), () => getCurrentValuesBody));

        // onClientTypeChange function - now fetches metadata from API for dynamic dropdown refresh
        var onClientTypeChangeBody = Js.Block()
            .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "aiConfigFields"))));
        
        // Fetch metadata from API endpoint
        var fetchUrl = Js.Str(() => "/init/ai-config-metadata?clientType=").Op(() => "+", () => (JsSyntax)Js.Id(() => "encodeURIComponent").Invoke(() => Js.Id(() => "clientType")));
        
        var fetchSuccessBody = Js.Block()
            .Add(() => Js.Const(() => "result", () => Js.Id(() => "data")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "result").Prop(() => "success").Op(() => "===", () => (JsSyntax)Js.Bool(() => true)), new List<JsSyntax>
                    {
                        // Render config fields with new data
                        Js.Id(() => "renderAIConfigFields").Invoke(
                            () => Js.Id(() => "result").Prop(() => "fields")
                        ).Stmt(),
                        // Update help link
                        Js.Id(() => "updateAIclientHelp").Invoke(() => Js.Id(() => "clientType")).Stmt()
                    })
                },
                { (null, new List<JsSyntax>
                    {
                        // Show error message without clearing fields
                        Js.Id(() => "alert").Invoke(() => Js.Str(() => "Failed to load configuration: ").Op(() => "+", () => (JsSyntax)Js.Id(() => "result").Prop(() => "message"))).Stmt()
                    })
                }
            }));
        
        var fetchErrorBody = Js.Block()
            // Show error without clearing fields
            .Add(() => Js.Id(() => "alert").Invoke(() => Js.Str(() => "Failed to load configuration. Please try again.")).Stmt());
        
        // Wrap fetchSuccessBody in an arrow function with 'data' parameter
        var aiConfigJsonThenArrow = Js.Arrow(() => new List<string> { "data" }, () => fetchSuccessBody);
        
        var aiConfigFetchThenArrow = Js.Arrow(() => new List<string> { "response" }, () => Js.Block()
            .Add(() => Js.Id(() => "response").Call(() => "json").Call(() => "then", () => aiConfigJsonThenArrow).Stmt()));
        
        var aiConfigFetchCatchArrow = Js.Arrow(() => new List<string> { "error" }, () => fetchErrorBody);
        
        onClientTypeChangeBody.Add(() => Js.Id(() => "fetch").Invoke(() => fetchUrl)
            .Call(() => "then", () => aiConfigFetchThenArrow)
            .Call(() => "catch", () => aiConfigFetchCatchArrow).Stmt());
        
        js.Add(() => Js.Func(() => "onClientTypeChange", () => new List<string> { "clientType" }, () => onClientTypeChangeBody));
        
        // renderAIConfigFields function - renders config fields from API response
        var renderFieldsBody = Js.Block()
            .Add(() => Js.Const(() => "container", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "aiConfigFields"))))
            .Add(() => Js.Assign(() => Js.Id(() => "container").Prop(() => "innerHTML"), () => Js.Str(() => "")));
        
        // forEach loop body - render each field
        var forEachBody = Js.Block()
            .Add(() => Js.Const(() => "key", () => Js.Id(() => "field").Prop(() => "key")))
            .Add(() => Js.Const(() => "label", () => Js.Id(() => "field").Prop(() => "label")))
            .Add(() => Js.Const(() => "value", () => Js.Id(() => "field").Prop(() => "value")))
            .Add(() => Js.Const(() => "div", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "div"))))
            .Add(() => Js.Assign(() => Js.Id(() => "div").Prop(() => "className"), () => Js.Str(() => "ai-config-field")))
            .Add(() => Js.Const(() => "labelEl", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "label"))))
            .Add(() => Js.Assign(() => Js.Id(() => "labelEl").Prop(() => "htmlFor"), () => Js.Str(() => "ai_").Op(() => "+", () => (JsSyntax)Js.Id(() => "key"))))
            .Add(() => Js.Assign(() => Js.Id(() => "labelEl").Prop(() => "textContent"), () => Js.Id(() => "label")))
            .Add(() => Js.Id(() => "div").Call(() => "appendChild", () => Js.Id(() => "labelEl")));
        
        // If has options, render select dropdown; otherwise render input text box
        var optionForEachBody = Js.Block()
            .Add(() => Js.Const(() => "option", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "option"))))
            .Add(() => Js.Assign(() => Js.Id(() => "option").Prop(() => "value"), () => Js.Id(() => "optKey")))
            .Add(() => Js.Assign(() => Js.Id(() => "option").Prop(() => "textContent"), () => Js.Id(() => "field").Prop(() => "options").Index(() => Js.Id(() => "optKey"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "optKey").Op(() => "===", () => (JsSyntax)Js.Id(() => "value")), new List<JsSyntax>
                    {
                        Js.Assign(() => Js.Id(() => "option").Prop(() => "selected"), () => Js.Bool(() => true))
                    })
                }
            }))
            .Add(() => Js.Id(() => "select").Call(() => "appendChild", () => Js.Id(() => "option")));
                
        var selectRenderingBody = Js.Block()
            .Add(() => Js.Const(() => "select", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "select"))))
            .Add(() => Js.Assign(() => Js.Id(() => "select").Prop(() => "name"), () => Js.Str(() => "ai_").Op(() => "+", () => (JsSyntax)Js.Id(() => "key"))))
            .Add(() => Js.Assign(() => Js.Id(() => "select").Prop(() => "id"), () => Js.Str(() => "ai_").Op(() => "+", () => (JsSyntax)Js.Id(() => "key"))))
            .Add(() => Js.Id(() => "Object").Call(() => "keys", () => Js.Id(() => "field").Prop(() => "options")).Call(() => "forEach", () => Js.Arrow(() => new List<string> { "optKey" }, () => optionForEachBody)).Stmt())
            // Bind onchange event to trigger refresh with current values
            .Add(() => Js.Id(() => "select").Call(() => "addEventListener", () => Js.Str(() => "change"), () => Js.Arrow(() => new List<string>(), () => Js.Block()
                .Add(() => Js.Id(() => "refreshAIConfigFields").Invoke().Stmt())
            )))
            .Add(() => Js.Id(() => "div").Call(() => "appendChild", () => Js.Id(() => "select")));
                
        var inputRenderingBody = Js.Block()
            .Add(() => Js.Const(() => "input", () => Js.Id(() => "document").Call(() => "createElement", () => Js.Str(() => "input"))))
            .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "type"), () => Js.Str(() => "text")))
            .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "name"), () => Js.Str(() => "ai_").Op(() => "+", () => (JsSyntax)Js.Id(() => "key"))))
            .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "id"), () => Js.Str(() => "ai_").Op(() => "+", () => (JsSyntax)Js.Id(() => "key"))))
            .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "placeholder"), () => Js.Id(() => "label")))
            .Add(() => Js.Assign(() => Js.Id(() => "input").Prop(() => "value"), () => Js.Id(() => "value")))
            // Bind onchange event to trigger refresh with current values
            .Add(() => Js.Id(() => "input").Call(() => "addEventListener", () => Js.Str(() => "change"), () => Js.Arrow(() => new List<string>(), () => Js.Block()
                .Add(() => Js.Id(() => "refreshAIConfigFields").Invoke().Stmt())
            )))
            .Add(() => Js.Id(() => "div").Call(() => "appendChild", () => Js.Id(() => "input")));
        
        forEachBody.Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
        {
            { (Js.Id(() => "field").Prop(() => "options"), new List<JsSyntax> { selectRenderingBody }) },
            { (null, new List<JsSyntax> { inputRenderingBody }) }
        }));
        
        forEachBody.Add(() => Js.Id(() => "container").Call(() => "appendChild", () => Js.Id(() => "div")));
        
        renderFieldsBody.Add(() => Js.Id(() => "fields").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "field" }, () => forEachBody)));
        
        js.Add(() => Js.Func(() => "renderAIConfigFields", () => new List<string> { "fields" }, () => renderFieldsBody));
        
        // refreshAIConfigFields function - collects current values and re-fetches from API
        var refreshBody = Js.Block()
            // Get current client type
            .Add(() => Js.Const(() => "clientType", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "aiClientTypeSelect")).Prop(() => "value")))
            // Collect all current AI config values
            .Add(() => Js.Const(() => "currentValues", () => Js.Obj()))
            .Add(() => Js.Const(() => "allInputs", () => Js.Id(() => "document").Call(() => "querySelectorAll", () => Js.Str(() => "#aiConfigFields input, #aiConfigFields select"))))
            .Add(() => Js.Id(() => "allInputs").Call(() => "forEach", () => Js.Arrow(() => new List<string> { "el" }, () => Js.Block()
                .Add(() => Js.Assign(() => Js.Id(() => "currentValues").Index(() => Js.Id(() => "el").Prop(() => "name")), () => Js.Id(() => "el").Prop(() => "value")))
            )));
        
        // Build URL with current values as JSON in query string
        var refreshFetchUrl = Js.Str(() => "/init/ai-config-metadata?clientType=").Op(() => "+", () => (JsSyntax)Js.Id(() => "encodeURIComponent").Invoke(() => Js.Id(() => "clientType")))
            .Op(() => "+", () => (JsSyntax)Js.Str(() => "&values="))
            .Op(() => "+", () => (JsSyntax)Js.Id(() => "encodeURIComponent").Invoke(() => Js.Id(() => "JSON").Call(() => "stringify", () => Js.Id(() => "currentValues"))));
        
        var refreshSuccessBody = Js.Block()
            .Add(() => Js.Const(() => "result", () => Js.Id(() => "data")))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "result").Prop(() => "success").Op(() => "===", () => (JsSyntax)Js.Bool(() => true)), new List<JsSyntax>
                    {
                        // Re-render fields with updated data
                        Js.Id(() => "renderAIConfigFields").Invoke(
                            () => Js.Id(() => "result").Prop(() => "fields")
                        ).Stmt()
                    })
                },
                { (null, new List<JsSyntax>
                    {
                        // Show error but keep current fields
                        Js.Id(() => "alert").Invoke(() => Js.Str(() => "Failed to update: ").Op(() => "+", () => (JsSyntax)Js.Id(() => "result").Prop(() => "message"))).Stmt()
                    })
                }
            }));
        
        var refreshErrorBody = Js.Block()
            // Show error but keep current fields
            .Add(() => Js.Id(() => "alert").Invoke(() => Js.Str(() => "Failed to update configuration.")).Stmt());
        
        var refreshJsonThenArrow = Js.Arrow(() => new List<string> { "data" }, () => refreshSuccessBody);
        var refreshFetchThenArrow = Js.Arrow(() => new List<string> { "response" }, () => Js.Block()
            .Add(() => Js.Id(() => "response").Call(() => "json").Call(() => "then", () => refreshJsonThenArrow).Stmt()));
        var refreshFetchCatchArrow = Js.Arrow(() => new List<string> { "error" }, () => refreshErrorBody);
        
        refreshBody.Add(() => Js.Id(() => "fetch").Invoke(() => refreshFetchUrl)
            .Call(() => "then", () => refreshFetchThenArrow)
            .Call(() => "catch", () => refreshFetchCatchArrow).Stmt());
        
        js.Add(() => Js.Func(() => "refreshAIConfigFields", () => new List<string>(), () => refreshBody));

        // updateAIclientHelp function - updates help link based on selected AI client type
        var updateHelpBody = Js.Block()
            .Add(() => Js.Const(() => "helpContainer", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "aiClientHelpLink"))))
            .Add(() => Js.Const(() => "helpTopic", () => Js.Id(() => "aiClientHelpTopics").Index(() => Js.Id(() => "clientType"))))
            .Add(() => Js.Const(() => "helpTitle", () => Js.Id(() => "aiHelpTitles").Index(() => Js.Id(() => "clientType"))))
            .Add(() => Js.If(() => new List<(JsSyntax?, List<JsSyntax>)>
            {
                { (Js.Id(() => "helpTopic").Op(() => "&&", () => (JsSyntax)Js.Id(() => "helpTitle")), new List<JsSyntax>
                    {
                        // Has help documentation, show link with language parameter from backend
                        Js.Assign(() => Js.Id(() => "helpContainer").Prop(() => "innerHTML"), 
                            () => Js.Str(() => "<a href='/help/")
                                .Op(() => "+", () => (JsSyntax)Js.Id(() => "helpTopic"))
                                .Op(() => "+", () => (JsSyntax)Js.Str(() => "?lang="))
                                .Op(() => "+", () => (JsSyntax)Js.Id(() => "initCurrentLang"))
                                .Op(() => "+", () => (JsSyntax)Js.Str(() => "' class='ai-help-link'>"))
                                .Op(() => "+", () => (JsSyntax)Js.Id(() => "aiHelpPrefix"))
                                .Op(() => "+", () => (JsSyntax)Js.Id(() => "helpTitle"))
                                .Op(() => "+", () => (JsSyntax)Js.Str(() => "</a>")))
                    })
                },
                { (null, new List<JsSyntax>
                    {
                        // No help documentation, clear container
                        Js.Assign(() => Js.Id(() => "helpContainer").Prop(() => "innerHTML"), () => Js.Str(() => ""))
                    })
                }
            }));
        js.Add(() => Js.Func(() => "updateAIclientHelp", () => new List<string> { "clientType" }, () => updateHelpBody));

        // Initialize on page load - just call init once
        var initBody = Js.Block()
            .Add(() => Js.Const(() => "selectEl", () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "aiClientTypeSelect"))))
            .Add(() => Js.Const(() => "initialType", () => Js.Id(() => "selectEl").Prop(() => "value")))
            .Add(() => Js.Id(() => "onClientTypeChange").Invoke(() => Js.Id(() => "initialType")).Stmt())
            .Add(() => Js.Id(() => "updateAIclientHelp").Invoke(() => Js.Id(() => "initialType")).Stmt());
        
        var initFunc = Js.Func(() => "initPage", () => new List<string>(), () => initBody);
        js.Add(() => initFunc);
        // Call initPage after DOM is loaded (script is in <head>, body not yet parsed when script runs)
        js.Add(() => Js.Id(() => "document").Call(() => "addEventListener", () => Js.Str(() => "DOMContentLoaded"), () => Js.Id(() => "initPage")).Stmt());

        return js;
    }

    private Dictionary<string, string> ParseFormData()
    {
        var result = new Dictionary<string, string>();
        var body = GetRequestBody();

        if (string.IsNullOrEmpty(body))
            return result;

        foreach (var pair in body.Split('&'))
        {
            var parts = pair.Split('=', 2);
            if (parts.Length == 2)
            {
                var key = Uri.UnescapeDataString(parts[0]);
                var value = Uri.UnescapeDataString(parts[1]);
                result[key] = value;
            }
        }

        return result;
    }

    private static string GetStyles()
    {
        return CssBuilder.Create()
            // CSS Variables - Enterprise Design System
            .WithVariable("bg-primary", "#0f172a")
            .WithVariable("bg-secondary", "#1e293b")
            .WithVariable("bg-card", "#334155")
            .WithVariable("bg-hover", "#475569")
            .WithVariable("border-color", "#475569")
            .WithVariable("text-primary", "#f1f5f9")
            .WithVariable("text-secondary", "#94a3b8")
            .WithVariable("text-muted", "#64748b")
            .WithVariable("accent-primary", "#3b82f6")
            .WithVariable("accent-secondary", "#10b981")
            .WithVariable("accent-warning", "#f59e0b")
            .WithVariable("accent-danger", "#ef4444")
            .WithVariable("card-radius", "8px")
            .WithVariable("transition", "0.2s ease")
            
            // Base Reset
            .Selector("*")
                .Property("box-sizing", "border-box")
                .Property("margin", "0")
                .Property("padding", "0")
            .EndSelector()
            .Selector("body")
                .Property("font-family", "-apple-system, BlinkMacSystemFont, 'Segoe UI', 'Microsoft YaHei', sans-serif")
                .Property("background", "var(--bg-primary)")
                .Property("min-height", "100vh")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("justify-content", "center")
                .Property("color", "var(--text-primary)")
                .Property("line-height", "1.5")
            .EndSelector()
            
            // Init Container
            .Selector(".init-container")
                .Property("width", "100%")
                .Property("max-width", "600px")
                .Property("padding", "24px")
            .EndSelector()
            
            // Init Card
            .Selector(".init-card")
                .Property("background", "var(--bg-secondary)")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "var(--card-radius)")
                .Property("padding", "40px 32px")
                .Property("max-width", "600px")
                .Property("width", "100%")
            .EndSelector()
            .Selector(".init-card h1")
                .Property("font-size", "24px")
                .Property("font-weight", "700")
                .Property("margin-bottom", "8px")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            .Selector(".init-card > p")
                .Property("font-size", "14px")
                .Property("color", "var(--text-secondary)")
                .Property("margin-bottom", "32px")
            .EndSelector()
            
            // Form
            .Selector(".init-form")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "20px")
            .EndSelector()
            .Selector(".form-group")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "6px")
            .EndSelector()
            .Selector(".form-group label")
                .Property("font-size", "13px")
                .Property("font-weight", "500")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            
            // Input Fields
            .Selector(".form-group input[type='text']")
                .Property("width", "100%")
                .Property("padding", "10px 14px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-primary)")
                .Property("color", "var(--text-primary)")
                .Property("font-size", "14px")
                .Property("outline", "none")
                .Property("transition", "var(--transition)")
            .EndSelector()
            .Selector(".form-group input::placeholder")
                .Property("color", "var(--text-muted)")
            .EndSelector()
            .Selector(".form-group input:focus")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            
            // Select Fields
            .Selector(".form-group select")
                .Property("width", "100%")
                .Property("padding", "10px 14px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-primary)")
                .Property("color", "var(--text-primary)")
                .Property("font-size", "14px")
                .Property("outline", "none")
                .Property("transition", "var(--transition)")
                .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".form-group select:focus")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".form-group select option")
                .Property("background", "var(--bg-secondary)")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            
            // Directory Browser
            .Selector(".dir-input-row")
                .Property("display", "flex")
                .Property("gap", "8px")
            .EndSelector()
            .Selector(".dir-input-row input")
                .Property("flex", "1")
            .EndSelector()
            .Selector(".dir-browse-btn")
                .Property("padding", "10px 16px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-secondary)")
                .Property("font-size", "13px")
                .Property("cursor", "pointer")
                .Property("transition", "var(--transition)")
                .Property("white-space", "nowrap")
            .EndSelector()
            .Selector(".dir-browse-btn:hover")
                .Property("background", "var(--bg-hover)")
            .EndSelector()
            .Selector(".dir-browser")
                .Property("margin-top", "8px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-primary)")
                .Property("max-height", "220px")
                .Property("overflow-y", "auto")
            .EndSelector()
            
            // Scrollbar
            .Selector(".dir-browser::-webkit-scrollbar")
                .Property("width", "6px")
            .EndSelector()
            .Selector(".dir-browser::-webkit-scrollbar-track")
                .Property("background", "var(--bg-secondary)")
            .EndSelector()
            .Selector(".dir-browser::-webkit-scrollbar-thumb")
                .Property("background", "var(--bg-hover)")
                .Property("border-radius", "3px")
            .EndSelector()
            
            // Directory Items
            .Selector(".dir-header")
                .Property("padding", "10px 12px")
                .Property("border-bottom", "1px solid var(--border-color)")
                .Property("position", "sticky")
                .Property("top", "0")
                .Property("background", "var(--bg-primary)")
            .EndSelector()
            .Selector(".dir-current")
                .Property("font-size", "12px")
                .Property("color", "var(--text-secondary)")
                .Property("font-family", "'SF Mono', Consolas, monospace")
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
                .Property("padding", "8px 12px")
                .Property("cursor", "pointer")
                .Property("font-size", "13px")
                .Property("color", "var(--text-secondary)")
                .Property("transition", "var(--transition)")
            .EndSelector()
            .Selector(".dir-item:hover")
                .Property("background", "rgba(59, 130, 246, 0.1)")
            .EndSelector()
            
            // Skin Grid
            .Selector(".skin-grid")
                .Property("display", "grid")
                .Property("grid-template-columns", "repeat(4, 1fr)")
                .Property("gap", "12px")
                .Property("margin-top", "6px")
            .EndSelector()
            .Selector(".skin-option")
                .Property("cursor", "pointer")
                .Property("transition", "var(--transition)")
                .Property("text-align", "center")
                .Property("padding", "14px 8px")
                .Property("border-radius", "var(--card-radius)")
                .Property("border", "2px solid transparent")
                .Property("position", "relative")
                .Property("user-select", "none")
            .EndSelector()
            .Selector(".skin-option:hover")
                .Property("transform", "translateY(-3px)")
                .Property("box-shadow", "0 6px 16px rgba(0,0,0,0.25)")
            .EndSelector()
            .Selector(".skin-option.selected::after")
                .Property("content", "\"\\2713\"")
                .Property("position", "absolute")
                .Property("top", "6px")
                .Property("right", "8px")
                .Property("font-size", "14px")
                .Property("width", "20px")
                .Property("height", "20px")
                .Property("border-radius", "50%")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("justify-content", "center")
            .EndSelector()
            .Selector(".skin-icon")
                .Property("font-size", "24px")
                .Property("margin-bottom", "6px")
            .EndSelector()
            .Selector(".skin-name")
                .Property("font-weight", "600")
                .Property("font-size", "13px")
                .Property("margin-bottom", "2px")
            .EndSelector()
            .Selector(".skin-desc")
                .Property("font-size", "11px")
                .Property("opacity", "0.7")
            .EndSelector()
            .Selector(".skin-colors")
                .Property("margin-top", "8px")
                .Property("display", "flex")
                .Property("justify-content", "center")
                .Property("gap", "4px")
            .EndSelector()
            .Selector(".color-dot")
                .Property("width", "10px")
                .Property("height", "10px")
                .Property("border-radius", "50%")
                .Property("display", "inline-block")
            .EndSelector()
            
            // Skin Preview
            .Selector(".skin-preview-section")
                .Property("margin-top", "12px")
            .EndSelector()
            .Selector(".skin-preview-title")
                .Property("font-size", "13px")
                .Property("font-weight", "500")
                .Property("color", "var(--text-secondary)")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".skin-preview-box")
                .Property("border-radius", "var(--card-radius)")
                .Property("border", "1px solid")
                .Property("padding", "20px")
                .Property("transition", "var(--transition)")
                .Property("min-height", "160px")
            .EndSelector()
            .Selector(".preview-inner")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "12px")
            .EndSelector()
            .Selector(".preview-card")
                .Property("padding", "14px")
                .Property("border-radius", "6px")
                .Property("border", "1px solid")
                .Property("transition", "var(--transition)")
            .EndSelector()
            .Selector(".preview-card h4")
                .Property("font-size", "14px")
                .Property("font-weight", "600")
                .Property("margin-bottom", "6px")
            .EndSelector()
            .Selector(".preview-card p")
                .Property("font-size", "12px")
                .Property("opacity", "0.8")
                .Property("line-height", "1.5")
            .EndSelector()
            .Selector(".preview-btns")
                .Property("display", "flex")
                .Property("gap", "8px")
                .Property("flex-wrap", "wrap")
            .EndSelector()
            .Selector(".preview-btn")
                .Property("padding", "6px 14px")
                .Property("border-radius", "6px")
                .Property("border", "none")
                .Property("cursor", "pointer")
                .Property("font-size", "12px")
                .Property("transition", "var(--transition)")
            .EndSelector()
            
            // Error Message
            .Selector(".form-error")
                .Property("background", "rgba(239, 68, 68, 0.1)")
                .Property("border", "1px solid var(--accent-danger)")
                .Property("border-radius", "6px")
                .Property("padding", "10px 14px")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector(".form-error p")
                .Property("font-size", "13px")
                .Property("color", "#fca5a5")
            .EndSelector()
            
            // Submit Button
            .Selector(".form-actions")
                .Property("margin-top", "8px")
            .EndSelector()
            .Selector(".form-actions button")
                .Property("width", "100%")
                .Property("padding", "12px")
                .Property("border", "none")
                .Property("border-radius", "6px")
                .Property("background", "var(--accent-primary)")
                .Property("color", "#ffffff")
                .Property("font-size", "15px")
                .Property("font-weight", "500")
                .Property("cursor", "pointer")
                .Property("transition", "var(--transition)")
            .EndSelector()
            .Selector(".form-actions button:hover")
                .Property("background", "#2563eb")
            .EndSelector()
            
            // Footer
            .Selector(".init-footer")
                .Property("margin-top", "24px")
                .Property("text-align", "center")
            .EndSelector()
            .Selector(".init-footer p")
                .Property("font-size", "12px")
                .Property("color", "var(--text-muted)")
            .EndSelector()
            .Selector(".init-help-link")
                .Property("display", "inline-block")
                .Property("margin-top", "12px")
                .Property("font-size", "13px")
                .Property("color", "var(--accent-primary)")
                .Property("text-decoration", "none")
                .Property("transition", "var(--transition)")
            .EndSelector()
            .Selector(".init-help-link:hover")
                .Property("color", "#2563eb")
                .Property("text-decoration", "underline")
            .EndSelector()
            
            // Language Selector
            .Selector(".lang-selector-row")
                .Property("display", "flex")
                .Property("gap", "8px")
                .Property("align-items", "center")
                .Property("width", "100%")
            .EndSelector()
            .Selector(".lang-selector-row select")
                .Property("flex", "1")
            .EndSelector()
            .Selector(".lang-switch-btn")
                .Property("padding", "10px 16px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-card)")
                .Property("color", "var(--text-secondary)")
                .Property("font-size", "13px")
                .Property("cursor", "pointer")
                .Property("transition", "var(--transition)")
                .Property("white-space", "nowrap")
            .EndSelector()
            .Selector(".lang-switch-btn:hover")
                .Property("background", "var(--bg-hover)")
            .EndSelector()
            
            // AI Config Fields
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
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-primary)")
                .Property("color", "var(--text-primary)")
                .Property("font-size", "14px")
                .Property("outline", "none")
                .Property("transition", "var(--transition)")
            .EndSelector()
            .Selector(".ai-config-field input:focus")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".ai-config-field select")
                .Property("width", "100%")
                .Property("padding", "10px 14px")
                .Property("border", "1px solid var(--border-color)")
                .Property("border-radius", "6px")
                .Property("background", "var(--bg-primary)")
                .Property("color", "var(--text-primary)")
                .Property("font-size", "14px")
                .Property("outline", "none")
                .Property("transition", "var(--transition)")
                .Property("cursor", "pointer")
            .EndSelector()
            .Selector(".ai-config-field select:focus")
                .Property("border-color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".ai-config-field select option")
                .Property("background", "var(--bg-secondary)")
                .Property("color", "var(--text-primary)")
            .EndSelector()
            
            // Loading Text
            .Selector(".loading-text")
                .Property("padding", "12px")
                .Property("text-align", "center")
                .Property("color", "var(--text-muted)")
                .Property("font-size", "13px")
            .EndSelector()
            
            // AI Help Link
            .Selector("#aiClientHelpContainer")
                .Property("margin-top", "-12px")
                .Property("margin-bottom", "8px")
            .EndSelector()
            .Selector("#aiClientHelpLink")
                .Property("font-size", "13px")
            .EndSelector()
            .Selector(".ai-help-link")
                .Property("color", "var(--accent-primary)")
                .Property("text-decoration", "none")
                .Property("transition", "var(--transition)")
            .EndSelector()
            .Selector(".ai-help-link:hover")
                .Property("color", "#2563eb")
                .Property("text-decoration", "underline")
            .EndSelector()
            
            // Responsive
            .Media("(max-width: 480px)")
                .Selector(".skin-grid")
                    .Property("grid-template-columns", "repeat(2, 1fr)")
                .EndSelector()
            .EndMedia()
            
            .Build();
    }
}
