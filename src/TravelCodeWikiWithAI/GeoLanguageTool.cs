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

using System.Reflection;
using SiliconLife.Collective;
using TravelCodeWikiWithAI.Data;
using TravelCodeWikiWithAI.TCWTool;

namespace TravelCodeWikiWithAI;

/// <summary>
/// 地理语言工具 - 供硅基人填写和查询地理实体的多语言数据
/// Geographic language tool - for silicon beings to fill and query multilingual data of geographic entities
/// 
/// 动作：
/// 1) set_language — 设置指定 ObjectPath 处的 LanguageData 翻译
/// 2) get_language — 获取指定 ObjectPath 处的 LanguageData 翻译
/// 3) list_entities — 列出可用地理实体
/// 4) set_word — 设置 GeoWordTable 词条（中外文对照翻译词典）
/// 
/// ObjectPath 格式：geo/root.World.Continents[0].Countries[2].Name
/// 支持 . 分隔的属性名和 [index] 数组索引
/// </summary>
public class GeoLanguageTool : ITool
{
    /// <summary>
    /// 工具名称 / Tool name
    /// </summary>
    public string Name => "geo_language";

    /// <summary>
    /// 工具描述 / Tool description
    /// </summary>
    public string Description =>
        "Manage multilingual data for geographic entities. " +
        "Use 'set_language' to add or update a language translation via ObjectPath, " +
        "'get_language' to retrieve language data via ObjectPath, " +
        "'list_entities' to list available geo entities, " +
        "'set_word' to set GeoWordTable entries (Chinese-foreign translation dictionary).";

    /// <summary>
    /// 获取工具的本地化显示名称 / Get localized display name
    /// </summary>
    public string GetDisplayName(Language language) => language switch
    {
        Language.ZhCN or Language.ZhSG or Language.ZhMY => "地理语言工具",
        Language.ZhHK or Language.ZhMO or Language.ZhTW => "地理語言工具",
        Language.JaJP => "地理言語ツール",
        Language.KoKR => "지리 언어 도구",
        _ => "Geo Language Tool"
    };

    /// <summary>
    /// 获取参数 JSON Schema / Get parameter JSON schema
    /// </summary>
    public Dictionary<string, object> GetParameterSchema()
    {
        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["enum"] = new[] { "set_language", "get_language", "list_entities", "set_word" },
                    ["description"] = "Action: set_language - Set language translation via ObjectPath, get_language - Get language data via ObjectPath, list_entities - List geo entities, set_word - Set GeoWordTable entry"
                },
                ["object_path"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "ObjectPath to locate a LanguageData property (e.g., 'geo/root.World.Continents[0].Countries[2].Name' or 'geo/root.World.Continents[0].Understand.Sections[0].Content'). Used with set_language and get_language."
                },
                ["entity_path"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Legacy entity path (e.g., 'world/CN', 'world/CN/provinces/BJ'). Used with list_entities and as fallback for set_language/get_language."
                },
                ["language_code"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Language code (e.g., 'zh-cn', 'en', 'ja'). Used with set_language, get_language, and set_word."
                },
                ["value"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The language value to set. Used with set_language and set_word (for foreign language translation content)."
                },
                ["chinese_term"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Chinese term for GeoWordTable entry. Used with set_word action."
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    /// <summary>
    /// 执行工具 / Execute tool
    /// </summary>
    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out var actionObj) || actionObj is not string action)
        {
            return ToolResult.Failed("Missing required parameter: action");
        }

        return action switch
        {
            "set_language" => ExecuteSetLanguage(callerId, parameters),
            "get_language" => ExecuteGetLanguage(callerId, parameters),
            "list_entities" => ExecuteListEntities(callerId, parameters),
            "set_word" => ExecuteSetWord(callerId, parameters),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    // =====================================================================
    // set_language — 支持 ObjectPath 定位任意 LanguageData
    // =====================================================================

    /// <summary>
    /// 设置地理实体的语言数据（支持 ObjectPath）
    /// </summary>
    private ToolResult ExecuteSetLanguage(Guid callerId, Dictionary<string, object> parameters)
    {
        // 优先使用 object_path，回退到 entity_path（兼容旧接口）
        string? objectPath = parameters.TryGetValue("object_path", out var opObj) && opObj is string s ? s : null;
        string? entityPath = parameters.TryGetValue("entity_path", out var epObj) && epObj is string s2 ? s2 : null;

        if (string.IsNullOrEmpty(objectPath) && string.IsNullOrEmpty(entityPath))
        {
            return ToolResult.Failed("Missing required parameter: object_path or entity_path");
        }

        if (!parameters.TryGetValue("language_code", out var codeObj) || codeObj is not string languageCode)
        {
            return ToolResult.Failed("Missing required parameter: language_code");
        }

        if (!parameters.TryGetValue("value", out var valueObj) || valueObj is not string value)
        {
            return ToolResult.Failed("Missing required parameter: value");
        }

        try
        {
            var geoProject = TravelCodeWikiWithAIPlugin._geoProject;
            if (geoProject == null)
            {
                return ToolResult.Failed("GeoProject not initialized");
            }

            // 优先通过 ObjectPath 定位 LanguageData
            if (!string.IsNullOrEmpty(objectPath))
            {
                var langData = ResolveLanguageData(geoProject, objectPath);
                if (langData == null)
                {
                    return ToolResult.Failed($"Cannot resolve LanguageData at path: {objectPath}");
                }

                langData[languageCode] = value;
                return ToolResult.Successful(
                    $"Set {languageCode} = '{value}' at {objectPath}",
                    new Dictionary<string, string> { ["object_path"] = objectPath, ["language_code"] = languageCode, ["value"] = value });
            }

            // 回退：通过 entity_path + IStorage（兼容旧接口）
            var storage = ServiceLocator.Instance.GetService<Func<string, IStorage>>();
            if (storage == null)
            {
                return ToolResult.Failed("Storage service not available");
            }

            IStorage st = storage("TravelCodeWiki");
            string key = $"geo/lang/{entityPath}";
            var existing = st.Read<Dictionary<string, string>>(key);
            var data = existing.Length > 0 ? existing[0] : new Dictionary<string, string>();
            data[languageCode] = value;
            st.Write(key, data);

            return ToolResult.Successful($"Set {languageCode} = '{value}' for {entityPath} (legacy mode)");
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to set language: {ex.Message}");
        }
    }

    // =====================================================================
    // get_language — 支持 ObjectPath 定位任意 LanguageData
    // =====================================================================

    /// <summary>
    /// 获取地理实体的语言数据（支持 ObjectPath）
    /// </summary>
    private ToolResult ExecuteGetLanguage(Guid callerId, Dictionary<string, object> parameters)
    {
        string? objectPath = parameters.TryGetValue("object_path", out var opObj) && opObj is string s ? s : null;
        string? entityPath = parameters.TryGetValue("entity_path", out var epObj) && epObj is string s2 ? s2 : null;

        if (string.IsNullOrEmpty(objectPath) && string.IsNullOrEmpty(entityPath))
        {
            return ToolResult.Failed("Missing required parameter: object_path or entity_path");
        }

        try
        {
            var geoProject = TravelCodeWikiWithAIPlugin._geoProject;
            if (geoProject == null)
            {
                return ToolResult.Failed("GeoProject not initialized");
            }

            // 优先通过 ObjectPath 定位 LanguageData
            if (!string.IsNullOrEmpty(objectPath))
            {
                var langData = ResolveLanguageData(geoProject, objectPath);
                if (langData == null)
                {
                    return ToolResult.Failed($"Cannot resolve LanguageData at path: {objectPath}");
                }

                // 如果指定了语言代码，只返回该语言的值
                if (parameters.TryGetValue("language_code", out var codeObj) && codeObj is string languageCode)
                {
                    var val = langData[languageCode];
                    if (val != null)
                    {
                        return ToolResult.Successful(
                            $"{objectPath}[{languageCode}] = {val}",
                            new Dictionary<string, string> { ["object_path"] = objectPath, ["language_code"] = languageCode, ["value"] = val });
                    }
                    else
                    {
                        return ToolResult.Successful(
                            $"Language '{languageCode}' not found at {objectPath}",
                            new Dictionary<string, string> { ["object_path"] = objectPath, ["language_code"] = languageCode });
                    }
                }

                // 返回所有语言数据
                var allData = new Dictionary<string, string>();
                foreach (var langKey in langData.Keys)
                {
                    allData[langKey] = langData[langKey] ?? "";
                }

                return ToolResult.Successful(
                    $"Retrieved language data at {objectPath} ({allData.Count} languages)",
                    allData);
            }

            // 回退：通过 entity_path + IStorage（兼容旧接口）
            var storage = ServiceLocator.Instance.GetService<Func<string, IStorage>>();
            if (storage == null)
            {
                return ToolResult.Failed("Storage service not available");
            }

            IStorage st = storage("TravelCodeWiki");
            string key = $"geo/lang/{entityPath}";
            var data = st.Read<Dictionary<string, string>>(key);
            if (data.Length == 0)
            {
                return ToolResult.Successful($"No language data found for {entityPath}", new Dictionary<string, string>());
            }

            if (parameters.TryGetValue("language_code", out var codeObj2) && codeObj2 is string languageCode2)
            {
                if (data[0].TryGetValue(languageCode2, out var value))
                {
                    return ToolResult.Successful($"{entityPath}[{languageCode2}] = {value}", new Dictionary<string, string> { [languageCode2] = value });
                }
                return ToolResult.Successful($"Language '{languageCode2}' not found for {entityPath}", new Dictionary<string, string>());
            }

            return ToolResult.Successful($"Retrieved language data for {entityPath} (legacy mode)", data[0]);
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to get language: {ex.Message}");
        }
    }

    // =====================================================================
    // list_entities — 列出可用的地理实体
    // =====================================================================

    /// <summary>
    /// 列出可用的地理实体 / List available geographic entities
    /// </summary>
    private ToolResult ExecuteListEntities(Guid callerId, Dictionary<string, object> parameters)
    {
        try
        {
            var geoProject = TravelCodeWikiWithAIPlugin._geoProject;
            if (geoProject == null)
            {
                return ToolResult.Failed("GeoProject not initialized");
            }

            // 从 GeoProject 对象树中收集所有包含 LanguageData 的实体
            var entities = new List<Dictionary<string, object>>();
            CollectLanguageEntities(geoProject, entities);

            // 也从 IStorage 收集（兼容旧数据）
            var storage = ServiceLocator.Instance.GetService<Func<string, IStorage>>();
            List<string> storageKeys = new();
            if (storage != null)
            {
                IStorage st = storage("TravelCodeWiki");
                storageKeys = st.ListKeys("geo/lang/").ToList();
            }

            var languages = SysTool.GetAllLanguage();

            return ToolResult.Successful(
                $"Found {entities.Count} entities with LanguageData in object tree, {storageKeys.Count} in storage. Supported languages: {languages.Count}",
                new Dictionary<string, object>
                {
                    ["entity_count"] = entities.Count,
                    ["entities"] = entities,
                    ["storage_entity_count"] = storageKeys.Count,
                    ["supported_languages"] = languages.Keys.ToList()
                });
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to list entities: {ex.Message}");
        }
    }

    // =====================================================================
    // set_word — 设置 GeoWordTable 词条
    // =====================================================================

    /// <summary>
    /// 设置 GeoWordTable 词条（中外文对照翻译词典）
    /// </summary>
    private ToolResult ExecuteSetWord(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("chinese_term", out var zhObj) || zhObj is not string chineseTerm)
        {
            return ToolResult.Failed("Missing required parameter: chinese_term");
        }

        if (!parameters.TryGetValue("language_code", out var codeObj) || codeObj is not string languageCode)
        {
            return ToolResult.Failed("Missing required parameter: language_code");
        }

        if (!parameters.TryGetValue("value", out var valueObj) || valueObj is not string foreignValue)
        {
            return ToolResult.Failed("Missing required parameter: value");
        }

        try
        {
            var geoProject = TravelCodeWikiWithAIPlugin._geoProject;
            if (geoProject == null)
            {
                return ToolResult.Failed("GeoProject not initialized");
            }

            if (geoProject.WordTable == null)
            {
                geoProject.WordTable = new GeoWordTable(geoProject);
            }

            geoProject.WordTable.Updata(chineseTerm, languageCode, foreignValue);

            return ToolResult.Successful(
                $"Set GeoWordTable: '{chineseTerm}' [{languageCode}] = '{foreignValue}'",
                new Dictionary<string, string>
                {
                    ["chinese_term"] = chineseTerm,
                    ["language_code"] = languageCode,
                    ["value"] = foreignValue
                });
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to set word: {ex.Message}");
        }
    }

    // =====================================================================
    // ObjectPath 解析器
    // =====================================================================

    /// <summary>
    /// 通过 ObjectPath 定位 LanguageData 对象。
    /// 
    /// ObjectPath 格式：geo/root.World.Continents[0].Countries[2].Name
    /// 解析步骤：
    /// 1) "geo/root" → GeoProject 实例（通过 IStorage key 匹配）
    /// 2) "." 分隔的属性名链
    /// 3) [index] 数组索引
    /// 4) 最后一节属性必须为 LanguageData 类型
    /// </summary>
    private LanguageData? ResolveLanguageData(GeoProject root, string objectPath)
    {
        // 去掉 "geo/root" 前缀
        string path = objectPath;
        if (path.StartsWith("geo/root"))
        {
            path = path.Substring("geo/root".Length);
        }
        else if (path.StartsWith("geo/"))
        {
            path = path.Substring("geo/".Length);
            // 跳过 "root" 部分
            if (path.StartsWith("root"))
            {
                path = path.Substring("root".Length);
            }
        }

        if (path.StartsWith("."))
        {
            path = path.Substring(1);
        }

        if (string.IsNullOrEmpty(path))
        {
            // 路径就是 root 本身，没有 LanguageData
            return null;
        }

        // 将路径拆分为段：属性名和数组索引
        var segments = ParsePathSegments(path);

        // 从 GeoProject 开始遍历
        object? current = root;
        LanguageData? result = null;

        for (int i = 0; i < segments.Count; i++)
        {
            if (current == null) return null;

            var seg = segments[i];

            if (seg.IsIndex)
            {
                // 数组索引
                if (current is System.Collections.IList list && seg.Index < list.Count)
                {
                    current = list[seg.Index]!;
                }
                else if (current is System.Collections.IEnumerable enumerable && !(current is string))
                {
                    // 尝试通过索引访问
                    var items = enumerable.Cast<object?>().ToList();
                    if (seg.Index < items.Count)
                    {
                        current = items[seg.Index];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                // 属性名
                var prop = current.GetType().GetProperty(seg.Name,
                    BindingFlags.Public | BindingFlags.Instance);

                if (prop != null && prop.CanRead)
                {
                    // 检查是否为 LanguageData 类型的属性
                    if (prop.PropertyType == typeof(LanguageData))
                    {
                        var langData = prop.GetValue(current) as LanguageData;
                        if (langData == null)
                        {
                            // 自动创建 LanguageData 实例
                            langData = new LanguageData();
                            if (prop.CanWrite)
                            {
                                prop.SetValue(current, langData);
                            }
                        }

                        // 如果这是路径的最后一节，返回 LanguageData
                        if (i == segments.Count - 1)
                        {
                            result = langData;
                        }
                        else
                        {
                            // LanguageData 后面不应有更多路径段
                            // 除非是 LanguageData 内部的键访问，但那用 language_code 参数处理
                            return null;
                        }

                        current = langData;
                    }
                    else
                    {
                        current = prop.GetValue(current);
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 将 ObjectPath 字符串解析为路径段列表。
    /// 支持：属性名（.Name）、数组索引（[0]）
    /// </summary>
    private List<PathSegment> ParsePathSegments(string path)
    {
        var segments = new List<PathSegment>();
        int i = 0;

        while (i < path.Length)
        {
            if (path[i] == '.')
            {
                i++; // skip dot separator
                continue;
            }

            if (path[i] == '[')
            {
                // 数组索引
                int end = path.IndexOf(']', i);
                if (end < 0) break;

                string indexStr = path.Substring(i + 1, end - i - 1);
                if (int.TryParse(indexStr, out int index))
                {
                    segments.Add(new PathSegment { IsIndex = true, Index = index });
                }

                i = end + 1;
            }
            else
            {
                // 属性名：读取到下一个 . 或 [ 为止
                int end = i;
                while (end < path.Length && path[end] != '.' && path[end] != '[')
                {
                    end++;
                }

                string name = path.Substring(i, end - i);
                segments.Add(new PathSegment { IsIndex = false, Name = name });

                i = end;
            }
        }

        return segments;
    }

    /// <summary>
    /// 路径段结构 / Path segment structure
    /// </summary>
    private struct PathSegment
    {
        public bool IsIndex;
        public string Name;
        public int Index;
    }

    // =====================================================================
    // 实体收集辅助方法
    // =====================================================================

    /// <summary>
    /// 递归收集对象树中所有包含 LanguageData 属性的实体信息
    /// </summary>
    private void CollectLanguageEntities(object obj, List<Dictionary<string, object>> results, int depth = 0)
    {
        if (obj == null || depth > 10) return;

        var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in props)
        {
            if (!prop.CanRead) continue;

            // 检查 LanguageData 属性
            if (prop.PropertyType == typeof(LanguageData))
            {
                var langData = prop.GetValue(obj) as LanguageData;
                if (langData != null)
                {
                    // 尝试获取该对象的 BasePath
                    string basePath = "";
                    if (obj is GeoDataBase gdb)
                    {
                        basePath = gdb.BasePath ?? "";
                    }

                    results.Add(new Dictionary<string, object>
                    {
                        ["path"] = string.IsNullOrEmpty(basePath) ? prop.Name : $"{basePath}.{prop.Name}",
                        ["property"] = prop.Name,
                        ["language_count"] = langData.Count,
                        ["languages"] = langData.Keys.ToList(),
                        ["has_all"] = langData.HasAllCode
                    });
                }
            }

            // 递归进入 GeoDataBase 子属性
            if (prop.PropertyType.IsSubclassOf(typeof(GeoDataBase)) && prop.CanRead)
            {
                try
                {
                    var child = prop.GetValue(obj) as GeoDataBase;
                    if (child != null && !ReferenceEquals(child, obj))
                    {
                        CollectLanguageEntities(child, results, depth + 1);
                    }
                }
                catch
                {
                    // 忽略属性访问异常
                }
            }
        }
    }
}
