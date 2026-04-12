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
using SiliconLife.Default.Web.Models;

namespace SiliconLife.Default.Web;

[WebCode]
public class ConfigController : Controller
{
    private readonly SkinManager _skinManager;

    public ConfigController()
    {
        _skinManager = ServiceLocator.Instance.GetService<SkinManager>()!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/config";

        if (path == "/config" || path == "/config/index")
            Index();
        else if (path == "/config/save")
            Save();
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
                RenderJson(new { success = false, message = "无效的请求参数" });
                return;
            }

            var config = Config.Instance?.Data as DefaultConfigData;
            if (config == null)
            {
                RenderJson(new { success = false, message = "配置实例不存在" });
                return;
            }

            var prop = typeof(DefaultConfigData).GetProperty(data.key);
            if (prop == null || !prop.CanWrite)
            {
                RenderJson(new { success = false, message = $"属性 {data.key} 不存在或不可写" });
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
                    RenderJson(new { success = false, message = $"无法将 '{data.value}' 转换为整数" });
                    return;
                }
            }
            else if (propType == typeof(long))
            {
                if (long.TryParse(data.value, out var longVal))
                    value = longVal;
                else
                {
                    RenderJson(new { success = false, message = $"无法将 '{data.value}' 转换为长整数" });
                    return;
                }
            }
            else if (propType == typeof(double))
            {
                if (double.TryParse(data.value, out var doubleVal))
                    value = doubleVal;
                else
                {
                    RenderJson(new { success = false, message = $"无法将 '{data.value}' 转换为浮点数" });
                    return;
                }
            }
            else if (propType == typeof(bool))
            {
                if (bool.TryParse(data.value, out var boolVal))
                    value = boolVal;
                else
                {
                    RenderJson(new { success = false, message = $"无法将 '{data.value}' 转换为布尔值" });
                    return;
                }
            }
            else if (propType == typeof(Guid))
            {
                if (Guid.TryParse(data.value, out var guidVal))
                    value = guidVal;
                else
                {
                    RenderJson(new { success = false, message = $"无法将 '{data.value}' 转换为 GUID" });
                    return;
                }
            }
            else if (propType == typeof(TimeSpan))
            {
                if (TimeSpan.TryParse(data.value, out var timeSpanVal))
                    value = timeSpanVal;
                else
                {
                    RenderJson(new { success = false, message = $"无法将 '{data.value}' 转换为时间间隔" });
                    return;
                }
            }
            else if (propType == typeof(DateTime))
            {
                if (DateTime.TryParse(data.value, out var dateTimeVal))
                    value = dateTimeVal;
                else
                {
                    RenderJson(new { success = false, message = $"无法将 '{data.value}' 转换为日期时间" });
                    return;
                }
            }
            else if (propType == typeof(System.IO.DirectoryInfo))
            {
                value = new System.IO.DirectoryInfo(data.value);
            }
            else if (propType.IsEnum)
            {
                if (Enum.TryParse(propType, data.value, out var enumVal))
                    value = enumVal;
                else
                {
                    RenderJson(new { success = false, message = $"无法将 '{data.value}' 转换为 {propType.Name}" });
                    return;
                }
            }
            else
            {
                RenderJson(new { success = false, message = $"不支持的属性类型: {propType.Name}" });
                return;
            }

            prop.SetValue(config, value);
            Config.Instance?.SaveConfig();
            
            RenderJson(new { success = true });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, message = $"保存失败: {ex.Message}" });
        }
    }

    private class SaveRequest
    {
        public string? key { get; set; }
        public string? value { get; set; }
    }

    private List<ConfigGroup> GetConfigGroups()
    {
        var config = Config.Instance?.Data as DefaultConfigData;
        if (config == null)
            return new List<ConfigGroup>();

        var type = typeof(DefaultConfigData);
        var properties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var groups = new Dictionary<string, ConfigGroup>();

        foreach (var prop in properties)
        {
            if (!prop.CanRead)
                continue;

            if (prop.GetCustomAttributes(typeof(ConfigIgnoreAttribute), true).Any())
                continue;

            var attr = prop.GetCustomAttributes(typeof(ConfigGroupAttribute), true).FirstOrDefault() as ConfigGroupAttribute;
            var groupName = attr?.GroupName ?? "其他";
            var displayName = attr?.DisplayName ?? prop.Name;
            var description = attr?.Description;
            var order = attr?.Order ?? 0;

            var value = prop.GetValue(config);
            var displayValue = value switch
            {
                Guid guid => guid.ToString(),
                TimeSpan ts => ts.ToString(),
                DateTime dt => dt.ToString("O"),
                null => null,
                _ => value.ToString()
            };

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
                enumDisplayNames = clients.Select(c => c.DisplayName).ToList();
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
                Description = description,
                Order = order,
                PropertyType = prop.Name == "WebSkin" || prop.Name == "AIClientType" ? "enum" : GetSimpleTypeName(prop.PropertyType),
                EnumValues = enumValues,
                EnumDisplayNames = enumDisplayNames,
                DependsOn = dependsOn,
                DependsOnValue = dependsOnValue
            });
        }

        foreach (var group in groups.Values)
        {
            group.Items = group.Items.OrderBy(i => i.Order).ToList();
        }

        return groups.Values.ToList();
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
        if (type.IsEnum) return "enum";
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
}
