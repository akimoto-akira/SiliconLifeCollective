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

using System.ComponentModel;
using SiliconLife.Collective;
using SiliconLife.Common.AI;
using SiliconLife.App.Web.Models;
using SiliconLife.App.Data;
using SiliconLife.Common.Localization;

namespace SiliconLife.App.Web.Controllers;

[WebCode]
public class ConfigController : Controller
{
    // Transport 等枚举以前端字符串（"Stdio"/"Http"）与配置文件序列化形式流转，
    // 与 Config.cs / McpController 的 JsonStringEnumConverter 约定保持一致。
    private static readonly System.Text.Json.JsonSerializerOptions McpListJsonOptions = new()
    {
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
    };

    private readonly SkinManager _skinManager;
    private readonly DefaultLocalizationBase _loc;

    public ConfigController()
    {
        _skinManager = ServiceLocator.Instance.GetService<SkinManager>()!;
        _loc = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(Config.Instance!.Data.Language);
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/config";

        if (path == "/config" || path == "/config/index")
            Index();
        else if (path == "/config/save")
            Save();
        else if (path == "/config/browse" || path == "/config/browse/")
            HandleBrowse();
        else if (path == "/config/aioptions")
            GetAIConfigOptions();
        else if (path == "/config/imoptions")
            GetIMPlatformOptions();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.ConfigView();
        var vm = new ConfigViewModel
        {
            Skin = skin,
            ActiveMenu = "config",
            Groups = GetConfigGroups(),
            AvailableAIClients = DiscoverAIClients()
        };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void Save()
    {
        try
        {
            using var reader = new System.IO.StreamReader(Request.InputStream);
            var body = reader.ReadToEnd();
            var data = System.Text.Json.JsonSerializer.Deserialize<SaveRequest>(body);
            
            if (data == null || string.IsNullOrEmpty(data.key))
            {
                RenderJson(new { success = false, message = _loc.ConfigErrorInvalidRequest });
                return;
            }

            var config = Config.Instance?.Data as AppConfigData;
            if (config == null)
            {
                RenderJson(new { success = false, message = _loc.ConfigErrorInstanceNotFound });
                return;
            }

            // Handle AI config items (keys starting with "AIConfig_")
            if (data.key.StartsWith("AIConfig_"))
            {
                SaveAIConfigItem(config, data.key, data.value);
                return;
            }

            // Handle plugin settings items (keys starting with "plugin.<pluginId>.<settingKey>")
            if (data.key.StartsWith("plugin."))
            {
                SavePluginSettingItem(data.key, data.value);
                return;
            }

            var prop = typeof(AppConfigData).GetProperty(data.key);
            if (prop == null || !prop.CanWrite)
            {
                RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorPropertyNotFound, data.key) });
                return;
            }

            var propType = prop.PropertyType;
            object? value = null;

            if (string.IsNullOrEmpty(data.value))
            {
                value = propType.IsValueType ? Activator.CreateInstance(propType) : null;
            }
            else if (propType == typeof(string))
            {
                value = data.value;
            }
            else if (propType == typeof(int))
            {
                if (int.TryParse(data.value, out var intVal))
                    value = intVal;
                else
                {
                    RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorConvertInt, data.value) });
                    return;
                }
            }
            else if (propType == typeof(long))
            {
                if (long.TryParse(data.value, out var longVal))
                    value = longVal;
                else
                {
                    RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorConvertLong, data.value) });
                    return;
                }
            }
            else if (propType == typeof(double))
            {
                if (double.TryParse(data.value, out var doubleVal))
                    value = doubleVal;
                else
                {
                    RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorConvertDouble, data.value) });
                    return;
                }
            }
            else if (propType == typeof(bool))
            {
                if (bool.TryParse(data.value, out var boolVal))
                    value = boolVal;
                else
                {
                    RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorConvertBool, data.value) });
                    return;
                }
            }
            else if (propType == typeof(Guid))
            {
                if (Guid.TryParse(data.value, out var guidVal))
                    value = guidVal;
                else
                {
                    RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorConvertGuid, data.value) });
                    return;
                }
            }
            else if (propType == typeof(TimeSpan))
            {
                if (TimeSpan.TryParse(data.value, out var timeSpanVal))
                    value = timeSpanVal;
                else
                {
                    RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorConvertTimeSpan, data.value) });
                    return;
                }
            }
            else if (propType == typeof(DateTime))
            {
                if (DateTime.TryParse(data.value, out var dateTimeVal))
                    value = dateTimeVal;
                else
                {
                    RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorConvertDateTime, data.value) });
                    return;
                }
            }
            else if (propType == typeof(System.IO.DirectoryInfo))
            {
                value = new System.IO.DirectoryInfo(data.value);
            }
            else if (propType == typeof(List<string>))
            {
                // Handle List<string> type - value should be JSON array string
                try
                {
                    var list = System.Text.Json.JsonSerializer.Deserialize<List<string>>(data.value);
                    if (list == null)
                    {
                        RenderJson(new { success = false, message = _loc.ConfigErrorInvalidRequest });
                        return;
                    }
                    value = list;
                }
                catch (Exception ex)
                {
                    RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorSaveFailed, $"JSON parse error: {ex.Message}") });
                    return;
                }
            }
            else if (propType == typeof(List<IMPlatformConfig>))
            {
                try
                {
                    var list = System.Text.Json.JsonSerializer.Deserialize<List<IMPlatformConfig>>(data.value);
                    if (list == null)
                    {
                        RenderJson(new { success = false, message = _loc.ConfigErrorInvalidRequest });
                        return;
                    }
                    value = list;
                }
                catch (Exception ex)
                {
                    RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorSaveFailed, $"JSON parse error: {ex.Message}") });
                    return;
                }
            }
            else if (propType == typeof(List<McpServerConfig>))
            {
                try
                {
                    var list = System.Text.Json.JsonSerializer.Deserialize<List<McpServerConfig>>(data.value, McpListJsonOptions);
                    if (list == null)
                    {
                        RenderJson(new { success = false, message = _loc.ConfigErrorInvalidRequest });
                        return;
                    }
                    value = list;
                }
                catch (Exception ex)
                {
                    RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorSaveFailed, $"JSON parse error: {ex.Message}") });
                    return;
                }
            }
            else if (propType.IsEnum)
            {
                if (Enum.TryParse(propType, data.value, out var enumVal))
                    value = enumVal;
                else
                {
                    RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorConvertEnum, data.value, propType.Name) });
                    return;
                }
            }
            else if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                // Handle Dictionary type - value should be JSON string
                try
                {
                    var dictType = typeof(Dictionary<string, object>);
                    value = System.Text.Json.JsonSerializer.Deserialize(data.value, dictType);
                    
                    if (value == null)
                    {
                        RenderJson(new { success = false, message = _loc.ConfigErrorInvalidRequest });
                        return;
                    }
                    
                    // Filter AIConfig dictionary to only keep keys supported by the target AI client
                    if (data.key == "AIConfig" && value is Dictionary<string, object> aiDict)
                    {
                        var clientType = config.AIClientType;
                        var factory = CreateAIClientFactory(clientType);
                        if (factory != null)
                        {
                            var supportedKeys = factory.GetConfigKeysMetadata(config.Language);
                            var filteredDict = new Dictionary<string, object>();
                            foreach (var kvp in aiDict)
                            {
                                if (supportedKeys.ContainsKey(kvp.Key))
                                {
                                    filteredDict[kvp.Key] = kvp.Value;
                                }
                            }
                            value = filteredDict;
                        }
                    }
                }
                catch (Exception ex)
                {
                    RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorSaveFailed, $"JSON parse error: {ex.Message}") });
                    return;
                }
            }
            else
            {
                RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorUnsupportedType, propType.Name) });
                return;
            }

            prop.SetValue(config, value);
            Config.Instance?.SaveConfig();
            
            RenderJson(new { success = true });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorSaveFailed, ex.Message) });
        }
    }

    private void SaveAIConfigItem(AppConfigData config, string key, string? value)
    {
        try
        {
            // Extract the actual config key (remove "AIConfig_" prefix)
            var configKey = key.Substring(9); // "AIConfig_".Length = 9
            
            // Update the AIConfig dictionary
            if (string.IsNullOrEmpty(value))
            {
                config.AIConfig.Remove(configKey);
            }
            else
            {
                config.AIConfig[configKey] = value;
            }
            
            Config.Instance?.SaveConfig();
            RenderJson(new { success = true });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorSaveFailed, ex.Message) });
        }
    }

    private class SaveRequest
    {
        public string? key { get; set; }
        public string? value { get; set; }
    }

    private List<ConfigGroup> GetConfigGroups()
    {
            var config = Config.Instance?.Data as AppConfigData;
        if (config == null)
            return new List<ConfigGroup>();

            var type = typeof(AppConfigData);
        var properties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var groups = new Dictionary<string, ConfigGroup>();

        // 默认值实例（"恢复默认值"按钮的数据来源）：反射创建同类型实例，属性初始化器即出厂默认值
        AppConfigData? defaults = null;
        try
        {
            defaults = (AppConfigData?)Activator.CreateInstance(config.GetType());
        }
        catch
        {
            // 具体类型缺少无参构造等极端情况：该项不提供恢复默认值
        }

        foreach (var prop in properties)
        {
            if (!prop.CanRead)
                continue;

            if (prop.GetCustomAttributes(typeof(ConfigIgnoreAttribute), true).Any())
                continue;

            var attr = prop.GetCustomAttributes(typeof(ConfigGroupAttribute), true).FirstOrDefault() as ConfigGroupAttribute;
            var groupKey = attr?.GroupKey ?? "Other";
            var groupName = _loc.GetConfigGroupName(groupKey);
            var displayName = attr?.DisplayNameKey != null 
                ? _loc.GetConfigDisplayName(attr.DisplayNameKey, out _)
                : _loc.GetConfigDisplayName(prop.Name, out _);
            var description = attr?.DescriptionKey != null 
                ? _loc.GetConfigDescription(attr.DescriptionKey) 
                : null;
            var order = attr?.Order ?? 0;

            var value = prop.GetValue(config);
            string? displayValue;
            if (value is System.Collections.IDictionary dict)
            {
                if (prop.Name == "AIConfig")
                {
                    var filteredDict = FilterAIConfigByClientType(dict, config.AIClientType, config.Language);
                    displayValue = System.Text.Json.JsonSerializer.Serialize(filteredDict);
                }
                else
                {
                    displayValue = System.Text.Json.JsonSerializer.Serialize(dict);
                }
            }
            else
            {
                displayValue = SerializeConfigValue(value);
            }

            // 出厂默认值（AIConfig 不做客户端过滤：代码初始化值本身即完整出厂配置）
            string? defaultValue = defaults != null
                ? SerializeConfigValue(prop.GetValue(defaults))
                : null;

            List<string>? enumValues = null;
            List<string>? enumDisplayNames = null;
            if (prop.PropertyType.IsEnum)
            {
                enumValues = Enum.GetNames(prop.PropertyType).ToList();
                enumDisplayNames = new List<string>();
                
                if (prop.PropertyType == typeof(Language))
                {
                    foreach (var enumName in enumValues)
                    {
                        if (Enum.TryParse<Language>(enumName, out var lang))
                        {
                            if (LocalizationManager.Instance.TryGetLocalization(lang, out var localization))
                            {
                                enumDisplayNames.Add(localization!.LanguageName);
                            }
                            else
                            {
                                enumDisplayNames.Add(enumName);
                            }
                        }
                        else
                        {
                            enumDisplayNames.Add(enumName);
                        }
                    }
                }
                else if (prop.PropertyType == typeof(LogLevel))
                {
                    if (LocalizationManager.Instance.TryGetLocalization(config.Language, out var localization))
                    {
                        foreach (var enumName in enumValues)
                        {
                            if (Enum.TryParse<LogLevel>(enumName, out var logLevel))
                            {
                                enumDisplayNames.Add(localization!.GetLogLevelName(logLevel));
                            }
                            else
                            {
                                enumDisplayNames.Add(enumName);
                            }
                        }
                    }
                    else
                    {
                        enumDisplayNames.AddRange(enumValues);
                    }
                }
                else
                {
                    foreach (var enumName in enumValues)
                    {
                        var field = prop.PropertyType.GetField(enumName);
                        var descAttr = field?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                        enumDisplayNames.Add(descAttr?.Description ?? enumName);
                    }
                }
            }

            if (prop.Name == "WebSkin")
            {
                var skins = _skinManager.GetAvailableSkins().ToList();
                enumValues = skins;
                enumDisplayNames = new List<string>();
                foreach (var skinCode in skins)
                {
                    var skinName = _skinManager.GetSkinName(skinCode);
                    enumDisplayNames.Add(skinName ?? skinCode);
                }
            }

            if (prop.Name == "AIClientType")
            {
                var clients = DiscoverAIClients();
                enumValues = clients.Select(c => c.TypeName).ToList();
                enumDisplayNames = clients.Select(c => _loc.GetConfigDisplayName(c.TypeName, out _)).ToList();
                // Normalize displayValue: config may store "DashScopeClientFactory" but enum uses "DashScopeClient"
                if (displayValue != null && displayValue.EndsWith("Factory"))
                {
                    displayValue = displayValue.Substring(0, displayValue.Length - 7);
                }
            }

            string? dependsOn = null;
            string? dependsOnValue = null;
            var aiClientAttr = prop.GetCustomAttributes(typeof(AIClientConfigAttribute), true).FirstOrDefault() as AIClientConfigAttribute;
            if (aiClientAttr != null)
            {
                dependsOn = "AIClientType";
                dependsOnValue = aiClientAttr.ClientType;
            }

            if (!groups.ContainsKey(groupName))
            {
                groups[groupName] = new ConfigGroup { Name = groupName };
            }

            groups[groupName].Items.Add(new ConfigItem
            {
                PropertyName = prop.Name,
                DisplayName = displayName,
                Value = displayValue,
                DefaultValue = defaultValue,
                Description = description,
                Order = order,
                PropertyType = prop.Name == "WebSkin" || prop.Name == "AIClientType" ? "enum" : GetSimpleTypeName(prop.PropertyType),
                EnumValues = enumValues,
                EnumDisplayNames = enumDisplayNames,
                DependsOn = dependsOn,
                DependsOnValue = dependsOnValue
            });
        }

        // AIConfig is rendered as a dictionary editor, not expanded into individual items
        // The dictionary editor will intelligently render dropdowns for keys like "region", "model", etc.
        // by calling IAIClientFactory.GetConfigKeyOptions()

        foreach (var group in groups.Values)
        {
            group.Items = group.Items.OrderBy(i => i.Order).ToList();
        }

        // 插件设置分组：实现 IPluginSettings 的插件声明项，渲染为独立的"插件"分组
        AddPluginSettingGroups(groups, config);

        return groups.Values.ToList();
    }

    /// <summary>
    /// 追加插件设置分组：遍历 PluginLoader 中实现 <see cref="IPluginSettings"/> 的插件，
    /// 每个插件渲染为一个配置组（组名 = 插件名），项来自
    /// <see cref="IPluginSettings.GetSettingDefinitions"/> 与当前值。
    /// 保存时以前缀 "plugin.&lt;pluginId&gt;.&lt;settingKey&gt;" 路由（见 <see cref="SavePluginSettingItem"/>）。
    /// </summary>
    private void AddPluginSettingGroups(Dictionary<string, ConfigGroup> groups, AppConfigData config)
    {
        var pluginLoader = ServiceLocator.Instance.GetService<PluginLoader>();
        if (pluginLoader == null)
            return;

        foreach (var plugin in pluginLoader.PluginsWithSettings)
        {
            var pluginMeta = (IPlugin)plugin;
            var groupName = pluginMeta.GetName(config.Language);
            if (!groups.ContainsKey(groupName))
                groups[groupName] = new ConfigGroup { Name = groupName };

            foreach (var def in plugin.GetSettingDefinitions())
            {
                if (string.IsNullOrEmpty(def.Key))
                    continue;

                object? current = plugin.GetSettingValue(def.Key) ?? def.DefaultValue;
                groups[groupName].Items.Add(new ConfigItem
                {
                    PropertyName = $"plugin.{pluginMeta.Id}.{def.Key}",
                    DisplayName = def.DisplayName,
                    Value = SerializeSettingValue(current),
                    DefaultValue = SerializeSettingValue(def.DefaultValue),
                    Description = def.Description,
                    Order = def.Order,
                    PropertyType = def.Type,
                    EnumValues = def.EnumValues,
                    EnumDisplayNames = def.EnumDisplayNames
                });
            }
        }
    }

    /// <summary>处理插件设置保存：解析 "plugin.&lt;pluginId&gt;.&lt;settingKey&gt;" 并路由到对应插件。</summary>
    private void SavePluginSettingItem(string fullKey, string? value)
    {
        try
        {
            var plugin = ResolvePluginSetting(fullKey, out var settingKey);
            if (plugin == null)
            {
                RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorPropertyNotFound, fullKey) });
                return;
            }

            var def = plugin.GetSettingDefinitions().FirstOrDefault(d => d.Key == settingKey);
            if (def == null)
            {
                RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorPropertyNotFound, fullKey) });
                return;
            }

            // 空串 → null：插件端回退默认值（"清空输入 = 恢复默认"语义）
            object? settingValue = null;
            if (!string.IsNullOrEmpty(value))
            {
                switch (def.Type)
                {
                    case PluginSettingTypes.String:
                        settingValue = value;
                        break;
                    case PluginSettingTypes.Int:
                        if (int.TryParse(value, out var intVal)) settingValue = intVal;
                        else { RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorConvertInt, value) }); return; }
                        break;
                    case PluginSettingTypes.Double:
                        if (double.TryParse(value, out var doubleVal)) settingValue = doubleVal;
                        else { RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorConvertDouble, value) }); return; }
                        break;
                    case PluginSettingTypes.Bool:
                        if (bool.TryParse(value, out var boolVal)) settingValue = boolVal;
                        else { RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorConvertBool, value) }); return; }
                        break;
                    case PluginSettingTypes.Enum:
                        if (def.EnumValues != null && def.EnumValues.Contains(value)) settingValue = value;
                        else { RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorConvertEnum, value, def.Type) }); return; }
                        break;
                    default:
                        RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorUnsupportedType, def.Type) });
                        return;
                }
            }

            plugin.SetSettingValue(def.Key, settingValue);
            RenderJson(new { success = true });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, message = string.Format(_loc.ConfigErrorSaveFailed, ex.Message) });
        }
    }

    /// <summary>解析 "plugin.&lt;pluginId&gt;.&lt;settingKey&gt;"：返回插件实例与设置键。</summary>
    private IPluginSettings? ResolvePluginSetting(string fullKey, out string settingKey)
    {
        settingKey = "";
        const string prefix = "plugin.";
        if (!fullKey.StartsWith(prefix, StringComparison.Ordinal))
            return null;

        var rest = fullKey[prefix.Length..];
        var idx = rest.IndexOf('.');
        if (idx <= 0 || idx == rest.Length - 1)
            return null;

        var pluginId = rest[..idx];
        settingKey = rest[(idx + 1)..];

        var pluginLoader = ServiceLocator.Instance.GetService<PluginLoader>();
        return pluginLoader?.PluginsWithSettings.FirstOrDefault(p => ((IPlugin)p).Id == pluginId);
    }

    /// <summary>序列化插件设置值用于前端展示/编辑（与 <see cref="SerializeConfigValue"/> 的显示格式一致）。</summary>
    private static string? SerializeSettingValue(object? value)
    {
        return value switch
        {
            null => null,
            bool b => b.ToString(),
            _ => value.ToString()
        };
    }

    private void AddAIConfigItems(Dictionary<string, ConfigGroup> groups, AppConfigData config)
    {
        var aiGroupName = _loc.GetConfigGroupName("AI");
        if (!groups.ContainsKey(aiGroupName))
        {
            groups[aiGroupName] = new ConfigGroup { Name = aiGroupName };
        }

        var currentClientType = config.AIClientType;
        var factory = CreateAIClientFactory(currentClientType);
        if (factory == null)
            return;

        // Get metadata and options for current AI client
        var metadata = factory.GetConfigKeysMetadata(config.Language);
        var currentConfig = config.AIConfig.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        int order = 100; // Start after existing AI items
        foreach (var kvp in metadata)
        {
            var configKey = kvp.Key;
            var displayName = kvp.Value;
            
            // Get current value from AIConfig
            var currentValue = config.AIConfig.TryGetValue(configKey, out var val) 
                ? val?.ToString() ?? "" 
                : "";

            // Get options if available
            var options = factory.GetConfigKeyOptions(configKey, currentConfig, config.Language);
            
            List<string>? enumValues = null;
            List<string>? enumDisplayNames = null;
            string propertyType = "string";
            
            if (options != null && options.Count > 0)
            {
                enumValues = options.Keys.ToList();
                enumDisplayNames = options.Values.ToList();
                propertyType = "enum";
            }

            groups[aiGroupName].Items.Add(new ConfigItem
            {
                PropertyName = $"AIConfig_{configKey}",
                DisplayName = displayName,
                Value = currentValue,
                Description = null,
                Order = order++,
                PropertyType = propertyType,
                EnumValues = enumValues,
                EnumDisplayNames = enumDisplayNames,
                DependsOn = "AIClientType",
                DependsOnValue = currentClientType,
                IsAIConfigItem = true,
                AIConfigKey = configKey,
                AIClientType = currentClientType
            });
        }
    }

    private Dictionary<string, object> FilterAIConfigByClientType(System.Collections.IDictionary aiConfig, string clientType, Language language)
    {
        var factory = CreateAIClientFactory(clientType);
        if (factory == null)
        {
            var result = new Dictionary<string, object>();
            foreach (System.Collections.DictionaryEntry entry in aiConfig)
            {
                var key = entry.Key?.ToString();
                if (key != null)
                    result[key] = entry.Value!;
            }
            return result;
        }

        var supportedKeys = factory.GetConfigKeysMetadata(language);
        var filtered = new Dictionary<string, object>();
        foreach (System.Collections.DictionaryEntry entry in aiConfig)
        {
            var key = entry.Key?.ToString();
            if (key != null && supportedKeys.ContainsKey(key))
            {
                filtered[key] = entry.Value!;
            }
        }
        return filtered;
    }

    private IAIClientFactory? CreateAIClientFactory(string clientType)
    {
        // Try to find the factory - handle both "OllamaClient" and "OllamaClientFactory" formats
        string factoryTypeName = clientType.EndsWith("Factory") ? clientType : clientType + "Factory";
        
        var factoryType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name == factoryTypeName && typeof(IAIClientFactory).IsAssignableFrom(t));

        if (factoryType == null)
            return null;

        try
        {
            return (IAIClientFactory)Activator.CreateInstance(factoryType)!;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 将配置属性值序列化为前端展示/编辑用的字符串（字典、列表序列化为 JSON，其余按类型格式化）。
    /// 当前值与出厂默认值共用此格式，保证"恢复默认值"重填表单的格式一致。
    /// </summary>
    private static string? SerializeConfigValue(object? value)
    {
        return value switch
        {
            null => null,
            System.Collections.IDictionary dict => System.Text.Json.JsonSerializer.Serialize(dict),
            System.Collections.IList list => System.Text.Json.JsonSerializer.Serialize(list),
            Guid guid => guid.ToString(),
            TimeSpan ts => ts.ToString(),
            DateTime dt => dt.ToString("O"),
            _ => value.ToString()
        };
    }

    private static string GetSimpleTypeName(Type type)
    {
        if (type == typeof(string)) return "string";
        if (type == typeof(int)) return "int";
        if (type == typeof(long)) return "long";
        if (type == typeof(double)) return "double";
        if (type == typeof(bool)) return "bool";
        if (type == typeof(Guid)) return "guid";
        if (type == typeof(TimeSpan)) return "timespan";
        if (type == typeof(DateTime)) return "datetime";
        if (type == typeof(System.IO.DirectoryInfo)) return "directory";
        if (type == typeof(List<string>)) return "directoryList";
        if (type == typeof(List<IMPlatformConfig>)) return "imPlatformList";
        if (type == typeof(List<McpServerConfig>)) return "mcpServerList";
        if (type.IsEnum) return "enum";
        
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            return "dictionary";
        }
        
        return type.Name.ToLower();
    }

    private List<AIClientInfo> DiscoverAIClients()
    {
        var clients = new List<AIClientInfo>();

        var clientTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IAIClient).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

        foreach (var type in clientTypes)
        {
            var displayName = type.Name;
            if (displayName.EndsWith("Client"))
            {
                displayName = displayName.Substring(0, displayName.Length - 6);
            }

            var descAttr = type.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

            clients.Add(new AIClientInfo
            {
                TypeName = type.Name,
                DisplayName = displayName,
                Description = descAttr?.Description
            });
        }

        return clients;
    }

    private void GetAIConfigOptions()
    {
        try
        {
            var config = Config.Instance?.Data as AppConfigData;
            if (config == null)
            {
                RenderJson(new { success = false, message = "Config not found" });
                return;
            }

            // Parse query parameters
            var query = Request.Url?.Query ?? "";
            var queryParams = System.Web.HttpUtility.ParseQueryString(query);
            var configKey = queryParams["key"] ?? "";
            var clientType = queryParams["clientType"] ?? config.AIClientType;

            var factory = CreateAIClientFactory(clientType);
            if (factory == null)
            {
                RenderJson(new { success = false, message = "Factory not found" });
                return;
            }

            var currentConfig = config.AIConfig.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            var options = factory.GetConfigKeyOptions(configKey, currentConfig, config.Language);

            if (options == null)
            {
                RenderJson(new { success = true, hasOptions = false });
            }
            else
            {
                RenderJson(new { 
                    success = true, 
                    hasOptions = true, 
                    options = options 
                });
            }
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, message = ex.Message });
        }
    }

    private void HandleBrowse()
    {
        try
        {
            var dir = GetQueryValue("dir", "/");

            if (string.IsNullOrEmpty(dir) || dir == "/")
            {
                var directories = new List<object>();
                if (System.IO.Path.DirectorySeparatorChar == '\\')
                {
                    foreach (var drive in System.IO.DriveInfo.GetDrives())
                    {
                        if (drive.IsReady)
                        {
                            directories.Add(new { name = $"{drive.Name} ({drive.DriveType})", path = drive.RootDirectory.FullName, isParent = false });
                        }
                    }
                }
                else
                {
                    try
                    {
                        foreach (var d in System.IO.Directory.GetDirectories("/"))
                        {
                            var info = new System.IO.DirectoryInfo(d);
                            directories.Add(new { name = info.Name, path = info.FullName, isParent = false });
                        }
                    }
                    catch { }
                }
                RenderJson(new { currentPath = "/", directories });
                return;
            }

            var dirInfo = new System.IO.DirectoryInfo(dir);
            if (!dirInfo.Exists)
            {
                RenderJson(new { success = false, message = $"Directory not found: {dir}" });
                return;
            }

            var dirs = new List<object>();
            if (dirInfo.Parent != null)
            {
                dirs.Add(new { name = "..", path = dirInfo.Parent.FullName, isParent = true });
            }
            else if (System.IO.Path.DirectorySeparatorChar == '\\' && dir.Length > 3)
            {
                var root = System.IO.Path.GetPathRoot(dir);
                if (root != null && root != dir)
                    dirs.Add(new { name = "..", path = root, isParent = true });
            }

            try
            {
                foreach (var subDir in dirInfo.GetDirectories())
                {
                    try { dirs.Add(new { name = subDir.Name, path = subDir.FullName, isParent = false }); }
                    catch (System.UnauthorizedAccessException) { }
                }
            }
            catch (System.UnauthorizedAccessException) { }

            RenderJson(new { currentPath = dir, directories = dirs });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, message = ex.Message });
        }
    }

    private void GetIMPlatformOptions()
    {
        try
        {
            // 平台选项、字段 schema、授权模式统一来自 IMProviderRegistry（替代前端硬编码 schema）
            var platforms = SiliconLife.Common.IM.IMProviderRegistry.GetAll()
                .Select(m => new
                {
                    value = m.PlatformId,
                    display = m.DisplayName,
                    authModes = m.AuthModes,
                    needsPublicCallback = m.NeedsPublicCallback,
                    // 接入帮助：文案经当前语言本地化解析，未注册时为空串（前端据此隐藏帮助区）
                    help = ResolveIMHelpText(m.HelpKey),
                    helpUrl = m.HelpUrl ?? "",
                    fields = m.ConfigFields.Select(f => new
                    {
                        key = f.Key,
                        label = f.Label,
                        type = f.Type,
                        required = f.Required,
                        placeholder = f.Placeholder ?? "",
                        isSecret = f.IsSecret
                    }).ToArray()
                })
                .ToArray();

            RenderJson(new
            {
                success = true,
                platforms = platforms
            });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// 解析 IM 平台接入帮助文案：HelpKey 为空或当前语言未注册该键时返回空串。
    /// </summary>
    private string ResolveIMHelpText(string? helpKey)
    {
        if (string.IsNullOrEmpty(helpKey))
            return string.Empty;
        var text = _loc.GetConfigDisplayName(helpKey, out var found);
        return found ? text : string.Empty;
    }
}
