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

using System.ComponentModel;
using System.Reflection;
using System.Text;
using SiliconLife.Collective;
using TravelCodeWikiWithAI.Data;

namespace TravelCodeWikiWithAI;

/// <summary>
/// 地理内容工具 — MediaWikiWord 内容读写
/// Geographic content tool — MediaWikiWord content read/write
/// 
/// 封装 MediaWikiWord 内容的读写能力，使硅基人能为地理实体编写结构化富文本文章。
/// Encapsulates MediaWikiWord content read/write, enabling silicon beings to author
/// structured rich-text articles for geographic entities.
/// 
/// 对应7步流程：步骤5（AI编写文章）
/// Corresponds to 7-step workflow: Step 5 (AI content authoring)
/// </summary>
public class GeoContentTool : ITool
{
    /// <summary>
    /// 工具名称 / Tool name
    /// </summary>
    public string Name => "geo_content";

    /// <summary>
    /// 工具描述 / Tool description
    /// </summary>
    public string Description =>
        "MediaWikiWord content read/write tool for geographic entities. " +
        "Use 'list_sections' to list all content fields and their status, " +
        "'read_section' to read a content field's structure, " +
        "'write_section' to write structured MediaWikiWord content to a field, " +
        "'preview_wiki' to preview the entity's MediaWiki markup output.";

    /// <summary>
    /// 获取工具支持的动作列表 / Get supported action list
    /// </summary>
    public string[] Actions => new[] { "list_sections", "read_section", "write_section", "preview_wiki" };

    /// <summary>
    /// 获取工具的本地化显示名称 / Get localized display name
    /// </summary>
    public string GetDisplayName(Language language) => language switch
    {
        Language.ZhCN or Language.ZhSG or Language.ZhMY => "地理内容工具",
        Language.ZhHK or Language.ZhMO or Language.ZhTW => "地理內容工具",
        Language.JaJP => "地理コンテンツツール",
        Language.KoKR => "지리 콘텐츠 도구",
        _ => "Geo Content Tool"
    };

    /// <summary>
    /// 获取参数JSON Schema / Get parameter JSON schema
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
                    ["enum"] = Actions,
                    ["description"] = "Action: list_sections | read_section | write_section | preview_wiki"
                },
                ["entity_path"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Full ID path of the geo entity (e.g., 'world/CN/BJ'). Required for all actions."
                },
                ["section_name"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Name of the content field/section property (e.g., 'Understand', 'capital', 'food'). Required for read_section and write_section."
                },
                ["content"] = new Dictionary<string, object>
                {
                    ["type"] = "object",
                    ["description"] = "Structured MediaWikiWord content for write_section. A JSON object describing the word tree to create. " +
                                     "Supported types: NoLanguage, Language, Section, Bold, Link, ExternalLink, NoSortList, SortList, " +
                                     "NewLine, Table, Geo, Map, Image, TemplateInclude, SystemMessage. " +
                                     "Each node has 'type' and type-specific fields.",
                    ["properties"] = new Dictionary<string, object>
                    {
                        ["type"] = new Dictionary<string, object>
                        {
                            ["type"] = "string",
                            ["description"] = "MediaWikiWord type name"
                        },
                        ["content"] = new Dictionary<string, object>
                        {
                            ["type"] = "string",
                            ["description"] = "Text content (for NoLanguage type)"
                        },
                        ["languages"] = new Dictionary<string, object>
                        {
                            ["type"] = "object",
                            ["description"] = "Language map {langCode: text} (for Language type)"
                        },
                        ["title"] = new Dictionary<string, object>
                        {
                            ["type"] = "object",
                            ["description"] = "Section title (for Section type, same structure as content)"
                        },
                        ["items"] = new Dictionary<string, object>
                        {
                            ["type"] = "array",
                            ["description"] = "Child items (for Section content, NoSortList, SortList, etc.)"
                        },
                        ["doc_title"] = new Dictionary<string, object>
                        {
                            ["type"] = "object",
                            ["description"] = "Link target (for Link/ExternalLink type)"
                        },
                        ["display"] = new Dictionary<string, object>
                        {
                            ["type"] = "object",
                            ["description"] = "Link display text (for Link/ExternalLink type)"
                        },
                        ["url"] = new Dictionary<string, object>
                        {
                            ["type"] = "object",
                            ["description"] = "URL content (for ExternalLink type)"
                        },
                        ["rows"] = new Dictionary<string, object>
                        {
                            ["type"] = "array",
                            ["description"] = "Table rows (for Table type)"
                        },
                        ["template_name"] = new Dictionary<string, object>
                        {
                            ["type"] = "string",
                            ["description"] = "Template name (for TemplateInclude type)"
                        },
                        ["template_params"] = new Dictionary<string, object>
                        {
                            ["type"] = "object",
                            ["description"] = "Template parameters (for TemplateInclude type)"
                        },
                        ["longitude"] = new Dictionary<string, object>
                        {
                            ["type"] = "number",
                            ["description"] = "Longitude (for Geo/Map type)"
                        },
                        ["latitude"] = new Dictionary<string, object>
                        {
                            ["type"] = "number",
                            ["description"] = "Latitude (for Geo/Map type)"
                        },
                        ["zoom"] = new Dictionary<string, object>
                        {
                            ["type"] = "integer",
                            ["description"] = "Map zoom level (for Geo/Map type)"
                        }
                    }
                },
                ["language_code"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Language code for preview_wiki (e.g., 'zh-cn', 'en'). Default: all base languages."
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
            "list_sections" => ExecuteListSections(parameters),
            "read_section" => ExecuteReadSection(parameters),
            "write_section" => ExecuteWriteSection(parameters),
            "preview_wiki" => ExecutePreviewWiki(parameters),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    // ===== list_sections =====

    /// <summary>
    /// 列出实体的所有内容字段及其状态（空/已填充/字数）
    /// List all content fields of an entity and their status (empty/filled/word count)
    /// </summary>
    private ToolResult ExecuteListSections(Dictionary<string, object> parameters)
    {
        try
        {
            if (!parameters.TryGetValue("entity_path", out var epObj) || epObj is not string entityPath || string.IsNullOrEmpty(entityPath))
            {
                return ToolResult.Failed("Missing required parameter: entity_path");
            }

            var project = GetGeoProject();
            if (project == null)
            {
                return ToolResult.Failed("Geo project data not available");
            }

            var entity = project.GetObject(entityPath) as GeoLocation;
            if (entity == null)
            {
                return ToolResult.Failed($"Entity not found: {entityPath}");
            }

            // Scan all WordBase properties via reflection
            var sections = new List<Dictionary<string, object?>>();
            var props = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                if (!typeof(WordBase).IsAssignableFrom(prop.PropertyType) && !typeof(WordBaseWithChild).IsAssignableFrom(prop.PropertyType))
                    continue;

                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                var value = prop.GetValue(entity) as WordBase;
                var descAttr = prop.GetCustomAttribute<DescriptionAttribute>();

                var sectionInfo = new Dictionary<string, object?>
                {
                    ["name"] = prop.Name,
                    ["description"] = descAttr?.Description ?? prop.Name,
                    ["type"] = value?.GetType().Name ?? prop.PropertyType.Name,
                    ["is_null"] = value == null,
                    ["word_count"] = value != null ? EstimateWordCount(value) : 0,
                    ["has_language_data"] = HasLanguageData(value),
                    ["status"] = value == null ? "empty" : (EstimateWordCount(value) > 0 ? "filled" : "empty_structure")
                };

                sections.Add(sectionInfo);
            }

            var result = new Dictionary<string, object?>
            {
                ["entity_path"] = entityPath,
                ["entity_name"] = entity.Name?.ToString(),
                ["entity_type"] = entity.GetType().Name,
                ["total_sections"] = sections.Count,
                ["filled_sections"] = sections.Count(s => s["status"]?.ToString() == "filled"),
                ["empty_sections"] = sections.Count(s => s["status"]?.ToString() != "filled"),
                ["sections"] = sections
            };

            return ToolResult.Successful(
                $"Entity {entityPath} has {sections.Count} content fields ({sections.Count(s => s["status"]?.ToString() == "filled")} filled, {sections.Count(s => s["status"]?.ToString() != "filled")} empty)",
                result);
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to list sections: {ex.Message}");
        }
    }

    // ===== read_section =====

    /// <summary>
    /// 读取实体的某个内容字段（返回 MediaWikiWord 树的结构描述）
    /// Read a content field of an entity (return structural description of the MediaWikiWord tree)
    /// </summary>
    private ToolResult ExecuteReadSection(Dictionary<string, object> parameters)
    {
        try
        {
            if (!parameters.TryGetValue("entity_path", out var epObj) || epObj is not string entityPath || string.IsNullOrEmpty(entityPath))
            {
                return ToolResult.Failed("Missing required parameter: entity_path");
            }

            if (!parameters.TryGetValue("section_name", out var snObj) || snObj is not string sectionName || string.IsNullOrEmpty(sectionName))
            {
                return ToolResult.Failed("Missing required parameter: section_name");
            }

            var project = GetGeoProject();
            if (project == null)
            {
                return ToolResult.Failed("Geo project data not available");
            }

            var entity = project.GetObject(entityPath) as GeoLocation;
            if (entity == null)
            {
                return ToolResult.Failed($"Entity not found: {entityPath}");
            }

            // Find the property
            var prop = entity.GetType().GetProperty(sectionName, BindingFlags.Public | BindingFlags.Instance);
            if (prop == null || !typeof(WordBase).IsAssignableFrom(prop.PropertyType))
            {
                // List available sections to help the caller
                var available = GetContentPropertyNames(entity);
                return ToolResult.Failed($"Section '{sectionName}' not found. Available sections: {string.Join(", ", available)}");
            }

            var value = prop.GetValue(entity) as WordBase;
            if (value == null)
            {
                var descAttr = prop.GetCustomAttribute<DescriptionAttribute>();
                return ToolResult.Successful(
                    $"Section '{sectionName}' is empty (null). You can write content to it using write_section.",
                    new Dictionary<string, object?>
                    {
                        ["entity_path"] = entityPath,
                        ["section_name"] = sectionName,
                        ["description"] = descAttr?.Description ?? sectionName,
                        ["status"] = "empty",
                        ["structure"] = null
                    });
            }

            // Describe the word tree structure
            var structure = DescribeWordTree(value);

            var result = new Dictionary<string, object?>
            {
                ["entity_path"] = entityPath,
                ["section_name"] = sectionName,
                ["status"] = "filled",
                ["structure"] = structure
            };

            return ToolResult.Successful(
                $"Read section '{sectionName}' of {entityPath}",
                result);
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to read section: {ex.Message}");
        }
    }

    // ===== write_section =====

    /// <summary>
    /// 向实体的某个内容字段写入 MediaWikiWord 富文本对象树
    /// Write a MediaWikiWord rich-text object tree to a content field of an entity
    /// 
    /// write_section 的输入是结构化的 MediaWikiWord 描述（Section/Language/Bold/Link/List等），不是纯文本。
    /// The input for write_section is a structured MediaWikiWord description (Section/Language/Bold/Link/List, etc.), not plain text.
    /// </summary>
    private ToolResult ExecuteWriteSection(Dictionary<string, object> parameters)
    {
        try
        {
            if (!parameters.TryGetValue("entity_path", out var epObj) || epObj is not string entityPath || string.IsNullOrEmpty(entityPath))
            {
                return ToolResult.Failed("Missing required parameter: entity_path");
            }

            if (!parameters.TryGetValue("section_name", out var snObj) || snObj is not string sectionName || string.IsNullOrEmpty(sectionName))
            {
                return ToolResult.Failed("Missing required parameter: section_name");
            }

            if (!parameters.TryGetValue("content", out var contentObj) || contentObj is not Dictionary<string, object> contentDict)
            {
                return ToolResult.Failed("Missing required parameter: content (must be a JSON object with 'type' field)");
            }

            var project = GetGeoProject();
            if (project == null)
            {
                return ToolResult.Failed("Geo project data not available");
            }

            var entity = project.GetObject(entityPath) as GeoLocation;
            if (entity == null)
            {
                return ToolResult.Failed($"Entity not found: {entityPath}");
            }

            // Find the property
            var prop = entity.GetType().GetProperty(sectionName, BindingFlags.Public | BindingFlags.Instance);
            if (prop == null || !typeof(WordBase).IsAssignableFrom(prop.PropertyType))
            {
                var available = GetContentPropertyNames(entity);
                return ToolResult.Failed($"Section '{sectionName}' not found. Available sections: {string.Join(", ", available)}");
            }

            // Build the MediaWikiWord tree from the content description
            var word = BuildWordTree(contentDict, entity);

            // Set the property
            prop.SetValue(entity, word);

            // Ensure parent chain is correct
            if (word != null)
            {
                word._parent = entity;
                word.CheckParent();
            }

            var result = new Dictionary<string, object?>
            {
                ["entity_path"] = entityPath,
                ["section_name"] = sectionName,
                ["word_type"] = word?.GetType().Name,
                ["status"] = "written"
            };

            return ToolResult.Successful(
                $"Written content to section '{sectionName}' of {entityPath} (type: {word?.GetType().Name ?? "null"})",
                result);
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to write section: {ex.Message}");
        }
    }

    // ===== preview_wiki =====

    /// <summary>
    /// 预览实体的 MediaWiki 标记输出（调用 BuildDocument）
    /// Preview the entity's MediaWiki markup output (calls BuildDocument)
    /// </summary>
    private ToolResult ExecutePreviewWiki(Dictionary<string, object> parameters)
    {
        try
        {
            if (!parameters.TryGetValue("entity_path", out var epObj) || epObj is not string entityPath || string.IsNullOrEmpty(entityPath))
            {
                return ToolResult.Failed("Missing required parameter: entity_path");
            }

            var project = GetGeoProject();
            if (project == null)
            {
                return ToolResult.Failed("Geo project data not available");
            }

            var entity = project.GetObject(entityPath) as GeoLocation;
            if (entity == null)
            {
                return ToolResult.Failed($"Entity not found: {entityPath}");
            }

            // Optional language code filter
            string? languageCode = parameters.TryGetValue("language_code", out var lcObj) ? lcObj?.ToString() : null;

            var files = new Dictionary<string, byte[]>();
            Dictionary<string, string> documents;

            try
            {
                documents = entity.BuildDocument(files);
            }
            catch (NotImplementedException)
            {
                return ToolResult.Failed($"BuildDocument not implemented for {entity.GetType().Name}. GetWikiDocuments() needs to be implemented first (see task-357).");
            }

            if (documents.Count == 0)
            {
                return ToolResult.Successful(
                    $"No wiki documents generated for {entityPath}. Content fields may be empty.",
                    new Dictionary<string, object?>
                    {
                        ["entity_path"] = entityPath,
                        ["document_count"] = 0,
                        ["pages"] = new Dictionary<string, string>()
                    });
            }

            // Filter by language code if specified
            if (!string.IsNullOrEmpty(languageCode))
            {
                var filtered = new Dictionary<string, string>();
                foreach (var kvp in documents)
                {
                    if (kvp.Key.EndsWith("/" + languageCode) || kvp.Key == languageCode)
                    {
                        filtered[kvp.Key] = kvp.Value;
                    }
                }
                documents = filtered;
            }

            // Truncate very long pages for preview
            var previewPages = new Dictionary<string, string>();
            foreach (var kvp in documents)
            {
                string content = kvp.Value;
                if (content.Length > 2000)
                {
                    content = content.Substring(0, 2000) + "\n... (truncated, total " + kvp.Value.Length + " chars)";
                }
                previewPages[kvp.Key] = content;
            }

            var result = new Dictionary<string, object?>
            {
                ["entity_path"] = entityPath,
                ["entity_name"] = entity.Name?.ToString(),
                ["document_count"] = documents.Count,
                ["pages"] = previewPages,
                ["file_count"] = files.Count
            };

            return ToolResult.Successful(
                $"Preview: {documents.Count} wiki pages generated for {entityPath}",
                result);
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to preview wiki: {ex.Message}");
        }
    }

    // ========== Helper methods ==========

    private static GeoProject? GetGeoProject() => TravelCodeWikiWithAIPlugin._geoProject;

    /// <summary>
    /// Get all content property names of a GeoLocation entity
    /// </summary>
    private static List<string> GetContentPropertyNames(GeoLocation entity)
    {
        var names = new List<string>();
        var props = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in props)
        {
            if (typeof(WordBase).IsAssignableFrom(prop.PropertyType) && prop.CanRead && prop.CanWrite)
            {
                names.Add(prop.Name);
            }
        }
        return names;
    }

    /// <summary>
    /// Estimate the word count of a WordBase tree
    /// </summary>
    private static int EstimateWordCount(WordBase word)
    {
        if (word == null) return 0;

        // For WordBaseWithChild, count children
        if (word is WordBaseWithChild childList)
        {
            int total = 0;
            foreach (WordBase child in childList)
            {
                total += EstimateWordCount(child);
            }
            return total;
        }

        // For MediaWikiSection, count title + content
        if (word is MediaWikiSection section)
        {
            int total = 0;
            if (section.Title != null) total += EstimateWordCount(section.Title);
            if (section.Content != null) total += EstimateWordCount(section.Content);
            return total;
        }

        // For MediaWikiLanguage, count language entries
        if (word is MediaWikiLanguage langWord)
        {
            if (langWord.LanguageData != null)
            {
                int count = 0;
                foreach (string val in langWord.LanguageData.Values)
                {
                    if (!string.IsNullOrEmpty(val)) count++;
                }
                return count;
            }
            return 0;
        }

        // For MediaWikiNoLanguage, check if content is non-empty
        if (word is MediaWikiNoLanguage noLang)
        {
            return string.IsNullOrEmpty(noLang.Content) ? 0 : noLang.Content.Length;
        }

        // For MediaWikiBold, count content
        if (word is MediaWikiBold bold)
        {
            return bold.Content != null ? EstimateWordCount(bold.Content) : 0;
        }

        // For MediaWikiLink, count display text
        if (word is MediaWikiLink link)
        {
            return link.Display != null ? EstimateWordCount(link.Display) : 0;
        }

        // For LanguageData in other properties, count non-empty values
        var langDataProps = word.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        int totalLang = 0;
        foreach (var prop in langDataProps)
        {
            if (prop.PropertyType == typeof(LanguageData) && prop.CanRead)
            {
                var ld = prop.GetValue(word) as LanguageData;
                if (ld != null)
                {
                    foreach (string val in ld.Values)
                    {
                        if (!string.IsNullOrEmpty(val)) totalLang++;
                    }
                }
            }
        }
        if (totalLang > 0) return totalLang;

        // Fallback: use ToString() length
        string? str = word.ToString();
        return string.IsNullOrEmpty(str) ? 0 : str.Length;
    }

    /// <summary>
    /// Check if a WordBase contains LanguageData
    /// </summary>
    private static bool HasLanguageData(WordBase? word)
    {
        if (word == null) return false;
        if (word is MediaWikiLanguage) return true;

        // Check if any property is LanguageData type
        var props = word.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in props)
        {
            if (prop.PropertyType == typeof(LanguageData) && prop.CanRead)
            {
                var ld = prop.GetValue(word) as LanguageData;
                if (ld != null && ld.Count > 0) return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Describe a WordBase tree as a nested dictionary structure
    /// </summary>
    private static Dictionary<string, object?> DescribeWordTree(WordBase word)
    {
        var desc = new Dictionary<string, object?>
        {
            ["type"] = word.GetType().Name,
            ["toString"] = word.ToString()
        };

        // Describe MediaWikiWord-specific properties
        switch (word)
        {
            case MediaWikiNoLanguage noLang:
                desc["content"] = noLang.Content;
                break;

            case MediaWikiLanguage lang:
                if (lang.LanguageData != null)
                {
                    var langs = new Dictionary<string, string>();
                    foreach (string key in lang.LanguageData.Keys)
                    {
                        string? val = lang.LanguageData[key];
                        if (!string.IsNullOrEmpty(val)) langs[key] = val;
                    }
                    desc["languages"] = langs;
                }
                if (lang.Parameters != null)
                {
                    desc["parameters"] = string.Join(", ", lang.Parameters.Keys);
                }
                break;

            case MediaWikiSection section:
                if (section.Title != null)
                {
                    desc["title"] = DescribeWordTree(section.Title);
                }
                if (section.Content != null)
                {
                    var children = new List<Dictionary<string, object?>>();
                    foreach (WordBase child in section.Content)
                    {
                        children.Add(DescribeWordTree(child));
                    }
                    desc["items"] = children;
                }
                break;

            case MediaWikiBold bold:
                if (bold.Content != null)
                {
                    desc["content"] = DescribeWordTree(bold.Content);
                }
                break;

            case MediaWikiLink link:
                if (link.DocTitle != null) desc["docTitle"] = DescribeWordTree(link.DocTitle);
                if (link.Display != null) desc["display"] = DescribeWordTree(link.Display);
                break;

            case MediaWikiExternalLink extLink:
                if (extLink.URL != null) desc["url"] = DescribeWordTree(extLink.URL);
                if (extLink.Display != null) desc["display"] = DescribeWordTree(extLink.Display);
                break;

            case MediaWikiNoSortList noSortList:
                {
                    var items = new List<Dictionary<string, object?>>();
                    foreach (WordBase child in noSortList)
                    {
                        items.Add(DescribeWordTree(child));
                    }
                    desc["items"] = items;
                }
                break;

            case MediaWikiSortList sortList:
                {
                    var items = new List<Dictionary<string, object?>>();
                    foreach (WordBase child in sortList)
                    {
                        items.Add(DescribeWordTree(child));
                    }
                    desc["items"] = items;
                }
                break;

            case MediaWikiNewLine:
                desc["content"] = "\\n";
                break;

            case MediaWikiTable table:
                desc["tableClass"] = table.TableClass;
                desc["tableStyle"] = table.TableStyle;
                if (table.Caption != null) desc["caption"] = DescribeWordTree(table.Caption);
                if (table.HeadRow != null) desc["hasHeader"] = true;
                if (table.Rows != null) desc["rowCount"] = table.Rows.Count;
                break;

            case MediaWikiTemplateInclude template:
                if (template.TemplateName != null) desc["templateName"] = DescribeWordTree(template.TemplateName);
                if (template.Parameters != null)
                {
                    var paramDesc = new Dictionary<string, object?>();
                    foreach (var kvp in template.Parameters)
                    {
                        paramDesc[kvp.Key] = kvp.Value?.ToString();
                    }
                    desc["templateParameters"] = paramDesc;
                }
                break;

            case MediaWikiGeo geo:
                desc["longitude"] = geo.Longitude;
                desc["latitude"] = geo.Latitude;
                desc["zoom"] = geo.Zoom;
                break;

            case MediaWikiMap map:
                desc["longitude"] = map.Longitude;
                desc["latitude"] = map.Latitude;
                desc["zoom"] = map.Zoom;
                break;

            case MediaWikiImage image:
                desc["size"] = image.Size;
                desc["option"] = image.Option.ToString();
                desc["hasImageData"] = image.ImageData != null && image.ImageData.Length > 0;
                break;

            case MediaWikiIgnoreLanguage ignoreLang:
                if (ignoreLang.Content != null)
                {
                    var langs = new Dictionary<string, string>();
                    foreach (string key in ignoreLang.Content.Keys)
                    {
                        string? val = ignoreLang.Content[key];
                        if (!string.IsNullOrEmpty(val)) langs[key] = val;
                    }
                    desc["languages"] = langs;
                }
                desc["ignoredLanguages"] = ignoreLang.IgnoredLanguages;
                break;

            case MediaWikiChildWord childWord:
                {
                    var items = new List<Dictionary<string, object?>>();
                    foreach (WordBase child in childWord)
                    {
                        items.Add(DescribeWordTree(child));
                    }
                    desc["items"] = items;
                }
                break;

            default:
                // Generic: scan all WordBase properties
                var subProps = word.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var sp in subProps)
                {
                    if (typeof(WordBase).IsAssignableFrom(sp.PropertyType) && sp.CanRead)
                    {
                        var subValue = sp.GetValue(word) as WordBase;
                        if (subValue != null)
                        {
                            desc[sp.Name] = DescribeWordTree(subValue);
                        }
                    }
                    else if (typeof(string) == sp.PropertyType && sp.CanRead)
                    {
                        var strVal = sp.GetValue(word) as string;
                        if (!string.IsNullOrEmpty(strVal))
                        {
                            desc[sp.Name] = strVal;
                        }
                    }
                }
                break;
        }

        return desc;
    }

    /// <summary>
    /// Build a MediaWikiWord tree from a structured content description dictionary
    /// </summary>
    private WordBase? BuildWordTree(Dictionary<string, object> desc, GeoDataBase parent)
    {
        if (!desc.TryGetValue("type", out var typeObj) || typeObj is not string typeName)
        {
            return null;
        }

        switch (typeName)
        {
            case "NoLanguage":
                {
                    var word = new MediaWikiNoLanguage(parent);
                    if (desc.TryGetValue("content", out var c)) word.Content = c?.ToString() ?? "";
                    return word;
                }

            case "Language":
                {
                    var word = new MediaWikiLanguage(parent);
                    var ld = new LanguageData();
                    if (desc.TryGetValue("languages", out var langsObj) && langsObj is Dictionary<string, object> langs)
                    {
                        foreach (var kvp in langs)
                        {
                            ld[kvp.Key] = kvp.Value?.ToString() ?? "";
                        }
                    }
                    word.LanguageData = ld;
                    word.LanguageData.Parent = word;
                    return word;
                }

            case "Section":
                {
                    var word = new MediaWikiSection(parent);
                    if (desc.TryGetValue("title", out var titleObj) && titleObj is Dictionary<string, object> titleDict)
                    {
                        word.Title = BuildWordTree(titleDict, word) as MediaWikiWord ?? new MediaWikiNoLanguage(word) { Content = "Untitled" };
                    }
                    else
                    {
                        word.Title = new MediaWikiNoLanguage(word) { Content = desc.TryGetValue("title_text", out var tt) ? tt?.ToString() ?? "Untitled" : "Untitled" };
                    }

                    if (desc.TryGetValue("items", out var itemsObj) && itemsObj is List<object> items)
                    {
                        word.Content = new MediaWikiChildWord(word);
                        foreach (Dictionary<string, object> item in items)
                        {
                            var childWord = BuildWordTree(item, word.Content);
                            if (childWord != null)
                            {
                                word.Content.Add(childWord);
                            }
                        }
                    }
                    return word;
                }

            case "Bold":
                {
                    var word = new MediaWikiBold(parent);
                    if (desc.TryGetValue("content", out var cObj) && cObj is Dictionary<string, object> contentDict)
                    {
                        word.Content = BuildWordTree(contentDict, word) as MediaWikiWord;
                    }
                    else
                    {
                        word.Content = new MediaWikiNoLanguage(word) { Content = cObj?.ToString() ?? "" };
                    }
                    return word;
                }

            case "Link":
                {
                    var word = new MediaWikiLink(parent);
                    if (desc.TryGetValue("doc_title", out var dtObj) && dtObj is Dictionary<string, object> dtDict)
                    {
                        word.DocTitle = BuildWordTree(dtDict, word) as MediaWikiWord;
                    }
                    else
                    {
                        word.DocTitle = new MediaWikiNoLanguage(word) { Content = dtObj?.ToString() ?? "" };
                    }

                    if (desc.TryGetValue("display", out var dispObj) && dispObj is Dictionary<string, object> dispDict)
                    {
                        word.Display = BuildWordTree(dispDict, word) as MediaWikiWord;
                    }
                    else
                    {
                        word.Display = new MediaWikiNoLanguage(word) { Content = dispObj?.ToString() ?? "" };
                    }
                    return word;
                }

            case "ExternalLink":
                {
                    var word = new MediaWikiExternalLink(parent);
                    if (desc.TryGetValue("url", out var urlObj) && urlObj is Dictionary<string, object> urlDict)
                    {
                        word.URL = BuildWordTree(urlDict, word) as MediaWikiWord;
                    }
                    else
                    {
                        word.URL = new MediaWikiNoLanguage(word) { Content = urlObj?.ToString() ?? "" };
                    }

                    if (desc.TryGetValue("display", out var dispObj) && dispObj is Dictionary<string, object> dispDict)
                    {
                        word.Display = BuildWordTree(dispDict, word) as MediaWikiWord;
                    }
                    else
                    {
                        word.Display = new MediaWikiNoLanguage(word) { Content = dispObj?.ToString() ?? "" };
                    }
                    return word;
                }

            case "NoSortList":
                {
                    var word = new MediaWikiNoSortList(parent);
                    if (desc.TryGetValue("items", out var itemsObj) && itemsObj is List<object> items)
                    {
                        foreach (Dictionary<string, object> item in items)
                        {
                            var childWord = BuildWordTree(item, word);
                            if (childWord != null)
                            {
                                word.Add(childWord);
                            }
                        }
                    }
                    return word;
                }

            case "SortList":
                {
                    var word = new MediaWikiSortList(parent);
                    if (desc.TryGetValue("items", out var itemsObj) && itemsObj is List<object> items)
                    {
                        foreach (Dictionary<string, object> item in items)
                        {
                            var childWord = BuildWordTree(item, word);
                            if (childWord != null)
                            {
                                word.Add(childWord);
                            }
                        }
                    }
                    return word;
                }

            case "NewLine":
                return new MediaWikiNewLine(parent);

            case "Geo":
                {
                    var word = new MediaWikiGeo(parent);
                    if (desc.TryGetValue("longitude", out var lng)) word.Longitude = Convert.ToDouble(lng);
                    if (desc.TryGetValue("latitude", out var lat)) word.Latitude = Convert.ToDouble(lat);
                    if (desc.TryGetValue("zoom", out var z)) word.Zoom = Convert.ToInt32(z);
                    if (desc.TryGetValue("location_name_zh", out var ln)) word.LocationName = new LanguageData(); // simplified
                    return word;
                }

            case "Map":
                {
                    var word = new MediaWikiMap(parent);
                    if (desc.TryGetValue("longitude", out var lng)) word.Longitude = Convert.ToDouble(lng);
                    if (desc.TryGetValue("latitude", out var lat)) word.Latitude = Convert.ToDouble(lat);
                    if (desc.TryGetValue("zoom", out var z)) word.Zoom = Convert.ToInt32(z);
                    if (desc.TryGetValue("map_width", out var mw)) word.MapWidth = Convert.ToInt32(mw);
                    if (desc.TryGetValue("map_height", out var mh)) word.MapHeight = Convert.ToInt32(mh);
                    return word;
                }

            case "Table":
                {
                    var word = new MediaWikiTable(parent);
                    if (desc.TryGetValue("table_class", out var tc)) word.TableClass = tc?.ToString();
                    if (desc.TryGetValue("table_style", out var ts)) word.TableStyle = ts?.ToString();
                    if (desc.TryGetValue("caption", out var capObj) && capObj is Dictionary<string, object> capDict)
                    {
                        word.Caption = BuildWordTree(capDict, word) as MediaWikiWord;
                    }
                    // Table rows can be added separately; for now support basic creation
                    return word;
                }

            case "TemplateInclude":
                {
                    var word = new MediaWikiTemplateInclude(parent);
                    if (desc.TryGetValue("template_name", out var tn))
                    {
                        word.TemplateName = new MediaWikiNoLanguage(word) { Content = tn?.ToString() ?? "" };
                    }
                    if (desc.TryGetValue("template_params", out var tpObj) && tpObj is Dictionary<string, object> tp)
                    {
                        word.Parameters = new Dictionary<string, MediaWikiWord>();
                        foreach (var kvp in tp)
                        {
                            if (kvp.Value is Dictionary<string, object> paramDict)
                            {
                                word.Parameters[kvp.Key] = BuildWordTree(paramDict, word) as MediaWikiWord ?? new MediaWikiNoLanguage(word) { Content = kvp.Value?.ToString() ?? "" };
                            }
                            else
                            {
                                word.Parameters[kvp.Key] = new MediaWikiNoLanguage(word) { Content = kvp.Value?.ToString() ?? "" };
                            }
                        }
                    }
                    return word;
                }

            case "SystemMessage":
                {
                    var word = new MediaWikiSystemMessage(parent);
                    if (desc.TryGetValue("message_name", out var mn)) word.SystemMessageName = mn?.ToString() ?? "";
                    return word;
                }

            case "IgnoreLanguage":
                {
                    var word = new MediaWikiIgnoreLanguage(parent);
                    var ld = new LanguageData();
                    if (desc.TryGetValue("languages", out var langsObj) && langsObj is Dictionary<string, object> langs)
                    {
                        foreach (var kvp in langs)
                        {
                            ld[kvp.Key] = kvp.Value?.ToString() ?? "";
                        }
                    }
                    word.Content = ld;
                    if (desc.TryGetValue("ignored_languages", out var il)) word.IgnoredLanguages = il?.ToString() ?? "";
                    return word;
                }

            default:
                throw new ArgumentException($"Unknown MediaWikiWord type: {typeName}. Supported types: NoLanguage, Language, Section, Bold, Link, ExternalLink, NoSortList, SortList, NewLine, Geo, Map, Table, TemplateInclude, SystemMessage, IgnoreLanguage");
        }
    }
}
