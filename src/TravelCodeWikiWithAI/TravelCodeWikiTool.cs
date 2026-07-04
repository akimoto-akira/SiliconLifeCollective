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
/// 旅游编码维基工具 — 地理实体的信息入口
/// 提供实体列表、实体详情、实体完成度查询等能力
/// </summary>
public class TravelCodeWikiTool : ITool
{
    /// <summary>
    /// 工具名称
    /// </summary>
    public string Name => "travel_code_wiki";

    /// <summary>
    /// 工具描述
    /// </summary>
    public string Description =>
        "Entry tool for geographic entity data. " +
        "Use 'list_entities' to list geographic entities with filtering, " +
        "'get_entity' to get entity details, " +
        "'get_entity_status' to check entity completion status, " +
        "'get_all_languages' to get all supported languages.";

    /// <summary>
    /// 获取工具支持的动作列表
    /// </summary>
    public string[] Actions => new[] { "list_entities", "get_entity", "get_entity_status", "get_all_languages" };

    /// <summary>
    /// 获取工具的本地化显示名称
    /// </summary>
    public string GetDisplayName(Language language) => language switch
    {
        Language.ZhCN or Language.ZhSG or Language.ZhMY => "旅游编码维基工具",
        Language.ZhHK or Language.ZhMO or Language.ZhTW => "旅遊編碼維基工具",
        Language.JaJP => "旅行コードウィキツール",
        Language.KoKR => "여행 코드 위키 도구",
        _ => "Travel Code Wiki Tool"
    };

    /// <summary>
    /// 获取参数JSON Schema
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
                    ["description"] = "Action: list_entities | get_entity | get_entity_status | get_all_languages"
                },
                ["entity_path"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Full ID path of the entity (e.g., 'world/CN/BJ' for Beijing). Required for get_entity and get_entity_status."
                },
                ["level"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["enum"] = new[] { "world", "continent", "country", "province", "city", "county", "attraction", "airport", "port" },
                    ["description"] = "Filter by entity level for list_entities"
                },
                ["type_filter"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["enum"] = new[] { "GeoCountry", "GeoProvince", "GeoCity", "GeoCounty", "GeoContinent", "GeoAttraction", "GeoAirport", "GeoPort", "GeoSpecialAdministrativeRegion" },
                    ["description"] = "Filter by entity type (class name) for list_entities"
                },
                ["parent_path"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Parent entity path to list children under (for list_entities). e.g., 'world/CN' to list provinces of China."
                },
                ["page"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Page number for list_entities (1-based, default 1)",
                    ["default"] = 1
                },
                ["page_size"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Page size for list_entities (default 20, max 100)",
                    ["default"] = 20
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    /// <summary>
    /// 执行工具
    /// </summary>
    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out var actionObj) || actionObj is not string action)
        {
            return ToolResult.Failed("Missing required parameter: action");
        }

        return action switch
        {
            "list_entities" => ExecuteListEntities(parameters),
            "get_entity" => ExecuteGetEntity(parameters),
            "get_entity_status" => ExecuteGetEntityStatus(parameters),
            "get_all_languages" => ExecuteGetAllLanguages(),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    // ===== list_entities =====

    private ToolResult ExecuteListEntities(Dictionary<string, object> parameters)
    {
        try
        {
            var project = GetGeoProject();
            if (project?.World == null)
                return ToolResult.Failed("Geo project data not available");

            // Collect all GeoLocation entities from the tree
            var allEntities = new List<GeoLocation>();
            CollectEntities(project.World, allEntities);

            // Apply filters
            string? typeFilter = parameters.TryGetValue("type_filter", out var tf) ? tf?.ToString() : null;
            string? levelFilter = parameters.TryGetValue("level", out var lf) ? lf?.ToString() : null;
            string? parentPath = parameters.TryGetValue("parent_path", out var pp) ? pp?.ToString() : null;

            var filtered = allEntities.AsEnumerable();

            if (!string.IsNullOrEmpty(typeFilter))
                filtered = filtered.Where(e => e.GetType().Name == typeFilter);

            if (!string.IsNullOrEmpty(levelFilter))
                filtered = filtered.Where(e => MatchesLevel(e, levelFilter));

            if (!string.IsNullOrEmpty(parentPath))
                filtered = filtered.Where(e => e.BasePath == parentPath);

            var resultList = filtered.ToList();

            // Pagination
            int page = parameters.TryGetValue("page", out var p) && int.TryParse(p?.ToString(), out var pv) ? pv : 1;
            int pageSize = parameters.TryGetValue("page_size", out var ps) && int.TryParse(ps?.ToString(), out var psv) ? Math.Min(psv, 100) : 20;
            page = Math.Max(1, page);

            int totalCount = resultList.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var paged = resultList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var items = paged.Select(e => new Dictionary<string, object?>
            {
                ["full_id"] = e.FullID,
                ["name"] = e.Name?.ToString(),
                ["type"] = e.GetType().Name,
                ["id"] = e.ID,
                ["parent_path"] = e.BasePath
            }).ToList();

            var result = new Dictionary<string, object?>
            {
                ["total_count"] = totalCount,
                ["page"] = page,
                ["page_size"] = pageSize,
                ["total_pages"] = totalPages,
                ["items"] = items
            };

            return ToolResult.Successful(
                $"Listed {items.Count} of {totalCount} entities (page {page}/{totalPages})",
                result);
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to list entities: {ex.Message}");
        }
    }

    // ===== get_entity =====

    private ToolResult ExecuteGetEntity(Dictionary<string, object> parameters)
    {
        try
        {
            if (!parameters.TryGetValue("entity_path", out var epObj) || epObj is not string entityPath || string.IsNullOrEmpty(entityPath))
                return ToolResult.Failed("Missing required parameter: entity_path");

            var project = GetGeoProject();
            if (project == null)
                return ToolResult.Failed("Geo project data not available");

            var entity = project.GetObject(entityPath) as GeoLocation;
            if (entity == null)
                return ToolResult.Failed($"Entity not found: {entityPath}");

            var result = new Dictionary<string, object?>
            {
                ["full_id"] = entity.FullID,
                ["id"] = entity.ID,
                ["type"] = entity.GetType().Name,
                ["name"] = entity.Name?.ToString(),
                ["parent_path"] = entity.BasePath,
                ["osm_id"] = entity.OSMID,
                ["osm_type"] = entity.OSCType.ToString(),
                ["wikidata"] = entity.wikidata,
                ["area_type"] = entity.AreaType,
                ["has_map_info"] = entity.MapInfo != null,
                ["has_understand"] = entity.Understand != null,
                ["content_fields_count"] = CountContentFields(entity),
                ["filled_content_fields_count"] = CountFilledContentFields(entity),
            };

            // Add MapInfo summary if available
            if (entity.MapInfo.HasValue)
            {
                result["map_info_center"] = entity.MapInfo.Value.Center.ToString();
                result["map_info_zoom"] = entity.MapInfo.Value.Zoom;
            }

            // Add sub-area count if available
            try
            {
                var subArea = entity.GetSubArea();
                result["sub_area_count"] = subArea?.Count ?? 0;
            }
            catch { /* some entities may not support GetSubArea */ }

            // Add attractions count if available
            try
            {
                var attractions = entity.GetAttractions();
                result["attractions_count"] = attractions?.Count ?? 0;
            }
            catch { /* some entities may not support GetAttractions */ }

            return ToolResult.Successful($"Entity details: {entity.FullID}", result);
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to get entity: {ex.Message}");
        }
    }

    // ===== get_entity_status =====

    private ToolResult ExecuteGetEntityStatus(Dictionary<string, object> parameters)
    {
        try
        {
            if (!parameters.TryGetValue("entity_path", out var epObj) || epObj is not string entityPath || string.IsNullOrEmpty(entityPath))
                return ToolResult.Failed("Missing required parameter: entity_path");

            var project = GetGeoProject();
            if (project == null)
                return ToolResult.Failed("Geo project data not available");

            var entity = project.GetObject(entityPath) as GeoLocation;
            if (entity == null)
                return ToolResult.Failed($"Entity not found: {entityPath}");

            // Check ID status
            bool hasId = !string.IsNullOrEmpty(entity.ID);
            bool hasWikidata = !string.IsNullOrEmpty(entity.wikidata);

            // Check content fields
            var emptyFields = new List<string>();
            var filledFields = new List<Dictionary<string, object>>();
            InspectContentFields(entity, emptyFields, filledFields);

            // Check language coverage for Name
            var missingLanguages = new List<string>();
            var availableLanguages = new List<string>();
            if (entity.Name != null)
            {
                var baseLangs = SysTool.GetBaseLanguage();
                foreach (var lang in baseLangs)
                {
                    if (entity.Name.ContainsKey(lang))
                        availableLanguages.Add(lang);
                    else
                        missingLanguages.Add(lang);
                }
            }

            float completionRate = 0f;
            int totalFields = emptyFields.Count + filledFields.Count;
            if (totalFields > 0)
                completionRate = (float)filledFields.Count / totalFields * 100;

            var result = new Dictionary<string, object?>
            {
                ["full_id"] = entity.FullID,
                ["name"] = entity.Name?.ToString(),
                ["type"] = entity.GetType().Name,
                ["id_status"] = hasId ? "assigned" : "missing",
                ["id_value"] = entity.ID,
                ["wikidata_status"] = hasWikidata ? "assigned" : "missing",
                ["completion_rate"] = Math.Round(completionRate, 1),
                ["total_content_fields"] = totalFields,
                ["filled_content_fields"] = filledFields.Count,
                ["empty_content_fields"] = emptyFields,
                ["filled_field_details"] = filledFields,
                ["name_available_languages"] = availableLanguages,
                ["name_missing_languages"] = missingLanguages,
            };

            return ToolResult.Successful(
                $"Entity status: {entity.FullID} — completion {completionRate:F1}%, ID={entity.ID}, {emptyFields.Count} empty fields",
                result);
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to get entity status: {ex.Message}");
        }
    }

    // ===== get_all_languages (preserved) =====

    private ToolResult ExecuteGetAllLanguages()
    {
        try
        {
            var languages = SysTool.GetAllLanguage();
            return ToolResult.Successful($"Retrieved {languages.Count} languages", languages);
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to get languages: {ex.Message}");
        }
    }

    // ===== Helper methods =====

    private static GeoProject? GetGeoProject() => TravelCodeWikiWithAIPlugin._geoProject;

    /// <summary>
    /// Recursively collect all GeoLocation entities from the tree
    /// </summary>
    private static void CollectEntities(GeoLocation parent, List<GeoLocation> result)
    {
        result.Add(parent);

        try
        {
            var subArea = parent.GetSubArea();
            if (subArea != null)
            {
                foreach (GeoLocation child in subArea)
                {
                    CollectEntities(child, result);
                }
            }
        }
        catch { /* GetSubArea may not be implemented for some types */ }

        try
        {
            var attractions = parent.GetAttractions();
            if (attractions != null)
            {
                foreach (GeoLocation child in attractions)
                {
                    CollectEntities(child, result);
                }
            }
        }
        catch { /* GetAttractions may not be implemented for some types */ }
    }

    /// <summary>
    /// Check if entity matches the given level name
    /// </summary>
    private static bool MatchesLevel(GeoLocation entity, string level)
    {
        return level.ToLowerInvariant() switch
        {
            "world" => entity is GeoWorld,
            "continent" => entity is GeoContinent,
            "country" => entity is GeoCountry,
            "province" => entity is GeoProvince,
            "city" => entity is GeoCity,
            "county" => entity is GeoCounty,
            "attraction" => entity is GeoAttraction,
            "airport" => entity is GeoAirport,
            "port" => entity is GeoPort,
            _ => entity.GetType().Name.Equals(level, StringComparison.OrdinalIgnoreCase)
        };
    }

    /// <summary>
    /// Count total WordBase content fields on a GeoLocation entity
    /// </summary>
    private static int CountContentFields(GeoLocation entity)
    {
        int count = 0;
        var props = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in props)
        {
            if (IsWordBaseType(prop.PropertyType))
                count++;
        }
        return count;
    }

    /// <summary>
    /// Count filled (non-null) WordBase content fields on a GeoLocation entity
    /// </summary>
    private static int CountFilledContentFields(GeoLocation entity)
    {
        int count = 0;
        var props = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in props)
        {
            if (IsWordBaseType(prop.PropertyType))
            {
                var val = prop.GetValue(entity);
                if (val != null)
                    count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Inspect content fields, collecting empty and filled field info
    /// </summary>
    private static void InspectContentFields(GeoLocation entity, List<string> emptyFields, List<Dictionary<string, object>> filledFields)
    {
        var props = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in props)
        {
            if (IsWordBaseType(prop.PropertyType))
            {
                var val = prop.GetValue(entity) as WordBase;
                if (val == null)
                {
                    emptyFields.Add(prop.Name);
                }
                else
                {
                    var detail = new Dictionary<string, object?>
                    {
                        ["field"] = prop.Name,
                        ["has_content"] = true
                    };
                    filledFields.Add(detail);
                }
            }
        }
    }

    /// <summary>
    /// Check if a type is WordBase or Nullable&lt;WordBase&gt;
    /// </summary>
    private static bool IsWordBaseType(Type type)
    {
        return type == typeof(WordBase)
            || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(type) == typeof(WordBase));
    }
}
