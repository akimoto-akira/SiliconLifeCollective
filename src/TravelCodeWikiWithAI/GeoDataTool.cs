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
using TravelCodeWikiWithAI.Data.OSM;

namespace TravelCodeWikiWithAI;

/// <summary>
/// 地理数据工具 — OSM数据操作与POI归类
/// Geographic data tool — OSM data operations and POI classification
/// 
/// 封装 OSM 数据操作能力，使硅基人能浏览POI数据、归类挂载、分配编码。
/// Encapsulates OSM data operations, enabling silicon beings to browse POI data,
/// classify and mount POIs, and assign codes.
/// 
/// 对应7步流程：步骤3（POI归类）+ 步骤4（编码分配）
/// Corresponds to 7-step workflow: Step 3 (POI classification) + Step 4 (code assignment)
/// </summary>
public class GeoDataTool : ITool
{
    /// <summary>
    /// 工具名称 / Tool name
    /// </summary>
    public string Name => "geo_data";

    /// <summary>
    /// 工具描述 / Tool description
    /// </summary>
    public string Description =>
        "OSM data operations and POI classification tool. " +
        "Use 'list_osm_pois' to list OSM POI data in an area, " +
        "'classify_poi' to classify a POI and mount it to the geo entity tree, " +
        "'assign_code' to assign an identifier code to a geo entity, " +
        "'refresh_osm' to refresh an entity's OSM data, " +
        "'expand_children' to expand an entity's child areas from PBF data.";

    /// <summary>
    /// 获取工具支持的动作列表 / Get supported action list
    /// </summary>
    public string[] Actions => new[] { "list_osm_pois", "classify_poi", "assign_code", "refresh_osm", "expand_children" };

    /// <summary>
    /// 获取工具的本地化显示名称 / Get localized display name
    /// </summary>
    public string GetDisplayName(Language language) => language switch
    {
        Language.ZhCN or Language.ZhSG or Language.ZhMY => "地理数据工具",
        Language.ZhHK or Language.ZhMO or Language.ZhTW => "地理數據工具",
        Language.JaJP => "地理データツール",
        Language.KoKR => "지리 데이터 도구",
        _ => "Geo Data Tool"
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
                    ["description"] = "Action: list_osm_pois | classify_poi | assign_code | refresh_osm | expand_children"
                },
                ["entity_path"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Full ID path of the geo entity (e.g., 'world/CN/BJ'). Required for most actions."
                },
                ["poi_type"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["enum"] = new[] { "attraction", "airport", "port" },
                    ["description"] = "POI classification type for classify_poi action: attraction (GeoAttraction), airport (GeoAirport), port (GeoPort)"
                },
                ["osm_id"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "OSM element ID. Used with classify_poi to specify which OSM element to classify."
                },
                ["osm_type"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["enum"] = new[] { "node", "way", "relation" },
                    ["description"] = "OSM element type for osm_id. Default: 'relation'."
                },
                ["code"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Identifier code to assign (max 5 letters, uppercase). Used with assign_code action."
                },
                ["tag_filter"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Filter OSM POIs by tag key prefix (e.g., 'tourism', 'aeroway', 'harbour'). Used with list_osm_pois."
                },
                ["page"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Page number for list_osm_pois (1-based, default 1)",
                    ["default"] = 1
                },
                ["page_size"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Page size for list_osm_pois (default 20, max 100)",
                    ["default"] = 20
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
            "list_osm_pois" => ExecuteListOsmPois(parameters),
            "classify_poi" => ExecuteClassifyPoi(parameters),
            "assign_code" => ExecuteAssignCode(parameters),
            "refresh_osm" => ExecuteRefreshOsm(parameters),
            "expand_children" => ExecuteExpandChildren(parameters),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    // ===== list_osm_pois =====

    /// <summary>
    /// 列出指定区域内的 OSM POI 数据（名称、类型标签、坐标）
    /// </summary>
    private ToolResult ExecuteListOsmPois(Dictionary<string, object> parameters)
    {
        try
        {
            if (!OsmOnlineApiService.OK)
            {
                return ToolResult.Failed("Online OSM API not available.");
            }

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

            // If entity has OSMID, find it in PBF data
            string? tagFilter = parameters.TryGetValue("tag_filter", out var tf) ? tf?.ToString() : null;

            // Collect POI candidates from OSM data
            var pois = new List<Dictionary<string, object?>>();

            // Strategy 1: If entity has an OSMID, look up its OSM relation for sub-elements
            if (entity.OSMID > 0)
            {
                CollectPOIsFromRelation(entity.OSMID, pois, tagFilter);
            }

            // Strategy 2: Scan OSM nodes within the entity's MapInfo bounding box
            if (entity.MapInfo.HasValue)
            {
                CollectPOIsFromBoundingBox(entity.MapInfo.Value, pois, tagFilter);
            }

            // Pagination
            int page = parameters.TryGetValue("page", out var p) && int.TryParse(p?.ToString(), out var pv) ? pv : 1;
            int pageSize = parameters.TryGetValue("page_size", out var ps) && int.TryParse(ps?.ToString(), out var psv) ? Math.Min(psv, 100) : 20;
            page = Math.Max(1, page);

            int totalCount = pois.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var paged = pois.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var result = new Dictionary<string, object?>
            {
                ["entity_path"] = entityPath,
                ["entity_name"] = entity.Name?.ToString(),
                ["total_count"] = totalCount,
                ["page"] = page,
                ["page_size"] = pageSize,
                ["total_pages"] = totalPages,
                ["items"] = paged
            };

            return ToolResult.Successful(
                $"Found {totalCount} POIs for {entityPath} (page {page}/{totalPages})",
                result);
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to list OSM POIs: {ex.Message}");
        }
    }

    // ===== classify_poi =====

    /// <summary>
    /// 将 POI 归类并挂载到地理实体树（创建 GeoAttraction/GeoAirport/GeoPort 节点）
    /// Classify a POI and mount it to the geo entity tree
    /// 
    /// 根据指定的 POI 类型，在父实体下创建对应的子节点，
    /// 并从 OSM 标签中提取名称、坐标等信息填充到新节点。
    /// </summary>
    private ToolResult ExecuteClassifyPoi(Dictionary<string, object> parameters)
    {
        try
        {
            if (!OsmOnlineApiService.OK)
            {
                return ToolResult.Failed("Online OSM api not available");
            }

            if (!parameters.TryGetValue("entity_path", out var epObj) || epObj is not string entityPath || string.IsNullOrEmpty(entityPath))
            {
                return ToolResult.Failed("Missing required parameter: entity_path");
            }

            if (!parameters.TryGetValue("poi_type", out var ptObj) || ptObj is not string poiType || string.IsNullOrEmpty(poiType))
            {
                return ToolResult.Failed("Missing required parameter: poi_type (attraction|airport|port)");
            }

            // OSM ID is optional — if not provided, create a bare POI node
            long osmId = 0;
            if (parameters.TryGetValue("osm_id", out var osmIdObj) && long.TryParse(osmIdObj?.ToString(), out var oid))
            {
                osmId = oid;
            }

            string osmType = parameters.TryGetValue("osm_type", out var otObj) ? otObj?.ToString() ?? "relation" : "relation";

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

            // Get OSM data for the POI
            Dictionary<string, string>? tags = null;
            Vector2DD location = default;

            if (osmId > 0)
            {
                tags = GetOSMTags(osmId, osmType, out location);
            }

            // Create the appropriate GeoLocation sub-type
            GeoLocation newPoi = poiType.ToLowerInvariant() switch
            {
                "attraction" => CreateAttraction(entity, tags, location, osmId),
                "airport" => CreateAirport(entity, tags, location, osmId),
                "port" => CreatePort(entity, tags, location, osmId),
                _ => throw new ArgumentException($"Unknown poi_type: {poiType}. Must be: attraction, airport, or port.")
            };

            // Mount the POI to the parent entity
            MountPOIToEntity(entity, newPoi, poiType.ToLowerInvariant());

            var result = new Dictionary<string, object?>
            {
                ["entity_path"] = entityPath,
                ["poi_type"] = poiType,
                ["poi_name"] = newPoi.Name?.ToString(),
                ["poi_id"] = newPoi.ID,
                ["osm_id"] = osmId,
                ["full_id"] = newPoi.FullID,
            };

            return ToolResult.Successful(
                $"Classified POI '{newPoi.Name?.ToString() ?? newPoi.ID}' as {poiType} under {entityPath}",
                result);
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to classify POI: {ex.Message}");
        }
    }

    // ===== assign_code =====

    /// <summary>
    /// 为无 ISO 代码的地理实体分配不超过5字母的标识编码
    /// Assign an identifier code (max 5 letters) to a geo entity that lacks an ISO code
    /// </summary>
    private ToolResult ExecuteAssignCode(Dictionary<string, object> parameters)
    {
        try
        {
            if (!parameters.TryGetValue("entity_path", out var epObj) || epObj is not string entityPath || string.IsNullOrEmpty(entityPath))
            {
                return ToolResult.Failed("Missing required parameter: entity_path");
            }

            if (!parameters.TryGetValue("code", out var codeObj) || codeObj is not string code || string.IsNullOrEmpty(code))
            {
                return ToolResult.Failed("Missing required parameter: code");
            }

            // Validate code: max 5 letters, uppercase
            string cleanCode = code.Trim().ToUpperInvariant();
            if (cleanCode.Length > 5)
            {
                return ToolResult.Failed($"Code '{code}' exceeds 5-letter limit. Please use a shorter code.");
            }

            if (!cleanCode.All(char.IsLetter))
            {
                return ToolResult.Failed($"Code '{code}' must contain only letters (A-Z).");
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

            // Check for duplicate codes among siblings
            string? duplicateEntity = FindDuplicateCode(entity, cleanCode);
            if (duplicateEntity != null)
            {
                return ToolResult.Failed($"Code '{cleanCode}' is already used by entity: {duplicateEntity}. Please choose a different code.");
            }

            // Assign the code
            string oldId = entity.ID;
            entity.ID = cleanCode;

            var result = new Dictionary<string, object?>
            {
                ["entity_path"] = entityPath,
                ["entity_name"] = entity.Name?.ToString(),
                ["old_id"] = oldId,
                ["new_id"] = cleanCode,
                ["full_id"] = entity.FullID,
            };

            return ToolResult.Successful(
                $"Assigned code '{cleanCode}' to {entityPath} (was: '{oldId}')",
                result);
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to assign code: {ex.Message}");
        }
    }

    // ===== refresh_osm =====

    /// <summary>
    /// 刷新实体的 OSM 数据（从在线 API 重新获取）
    /// </summary>
    private ToolResult ExecuteRefreshOsm(Dictionary<string, object> parameters)
    {
        try
        {
            if (!OsmOnlineApiService.OK)
            {
                return ToolResult.Failed("Online OSM api not available");
            }

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

            // Refresh OSM data from online API
            var changes = new List<string>();

            if (entity.OSMID > 0)
            {
                var tags = OsmOnlineApiService.GetRelationTags(entity.OSMID);
                if (tags.Count > 0)
                {
                    // Update Name from OSM tags if current Name is null
                    if (entity.Name == null || entity.Name.Count == 0)
                    {
                        var nameFromTags = LanguageData.CreateWithTags(tags);
                        if (nameFromTags != null)
                        {
                            entity.Name = nameFromTags;
                            entity.Name.Parent = entity;
                            changes.Add("Name updated from OSM tags");
                        }
                    }

                    // Update wikidata if missing
                    if (string.IsNullOrEmpty(entity.wikidata) && tags.TryGetValue("wikidata", out var wd))
                    {
                        entity.wikidata = wd;
                        changes.Add($"wikidata set to {wd}");
                    }

                    // Update MapInfo from OSM tags if missing
                    if (!entity.MapInfo.HasValue)
                    {
                        if (tags.TryGetValue("min_lat", out var minLat) && tags.TryGetValue("max_lat", out var maxLat) &&
                            tags.TryGetValue("min_lon", out var minLon) && tags.TryGetValue("max_lon", out var maxLon))
                        {
                            double centerLon = (double.Parse(minLon) + double.Parse(maxLon)) / 2;
                            double centerLat = (double.Parse(minLat) + double.Parse(maxLat)) / 2;
                            entity.MapInfo = new MapInfo(centerLon, centerLat, 10);
                            changes.Add("MapInfo updated from OSM bounding box");
                        }
                    }

                    // Trigger entity's own refresh logic
                    entity.FlushOSMData();
                    changes.Add("FlushOSMData() called");
                }
                else
                {
                    changes.Add($"No OSM relation found with ID {entity.OSMID}");
                }
            }
            else
            {
                changes.Add("Entity has no OSMID — cannot refresh from online API");
            }

            var result = new Dictionary<string, object?>
            {
                ["entity_path"] = entityPath,
                ["entity_name"] = entity.Name?.ToString(),
                ["osm_id"] = entity.OSMID,
                ["changes"] = changes
            };

            return ToolResult.Successful(
                $"Refreshed OSM data for {entityPath}: {string.Join(", ", changes)}",
                result);
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to refresh OSM data: {ex.Message}");
        }
    }

    // ===== expand_children =====

    /// <summary>
    /// 展开实体的子区域（从在线 API 获取）
    /// </summary>
    private ToolResult ExecuteExpandChildren(Dictionary<string, object> parameters)
    {
        try
        {
            if (!OsmOnlineApiService.OK)
            {
                return ToolResult.Failed("Online OSM API not available");
            }

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

            if (entity.OSMID <= 0)
            {
                return ToolResult.Failed($"Entity {entityPath} has no OSMID — cannot expand children");
            }

            // Find the OSM relation for this entity
            var relInfo = OsmOnlineApiService.GetRelationInfo(entity.OSMID);
            if (relInfo == null)
            {
                return ToolResult.Failed($"No OSM relation found for OSMID {entity.OSMID}");
            }

            var addedChildren = new List<Dictionary<string, object?>>();

            // Iterate over relation members to find sub-areas
            foreach (var member in relInfo.SubRelations)
            {
                string role = member.Role ?? "";
                if (role != "" && role != "admin" && role != "label" && role != "subarea" && role != "child")
                    continue;

                // Check if we already have this child in the entity's sub-area
                var existingSubArea = entity.GetSubArea();
                bool alreadyExists = false;
                foreach (GeoLocation child in existingSubArea)
                {
                    if (child.OSMID == member.Id && child.OSCType == OSMRelationRefType.Relations)
                    {
                        alreadyExists = true;
                        break;
                    }
                }

                if (alreadyExists)
                    continue;

                // Look up the child relation tags from online API
                var childTags = OsmOnlineApiService.GetRelationTags(member.Id);
                if (childTags.Count == 0)
                    continue;

                // Determine admin level and create appropriate GeoLocation subclass
                GeoLocation childEntity = CreateGeoLocationFromTags(entity, childTags, member.Id);

                // Add to parent's sub-area
                MountSubAreaToEntity(entity, childEntity);

                addedChildren.Add(new Dictionary<string, object?>
                {
                    ["name"] = childEntity.Name?.ToString(),
                    ["id"] = childEntity.ID,
                    ["osm_id"] = childEntity.OSMID,
                    ["type"] = childEntity.GetType().Name,
                    ["full_id"] = childEntity.FullID,
                    ["role"] = role
                });
            }

            var result = new Dictionary<string, object?>
            {
                ["entity_path"] = entityPath,
                ["entity_name"] = entity.Name?.ToString(),
                ["children_added"] = addedChildren.Count,
                ["children"] = addedChildren
            };

            return ToolResult.Successful(
                $"Expanded {addedChildren.Count} children for {entityPath}",
                result);
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Failed to expand children: {ex.Message}");
        }
    }

    // ========== Helper methods ==========

    private static GeoProject? GetGeoProject() => TravelCodeWikiWithAIPlugin._geoProject;

    /// <summary>
    /// 从在线 API 获取 Relation 的所有成员（node/way），筛选出 POI
    /// </summary>
    private void CollectPOIsFromRelation(long osmId, List<Dictionary<string, object?>> pois, string? tagFilter)
    {
        var detail = OsmOnlineApiService.GetRelationDetail(osmId);
        if (detail == null) return;

        // Check if the relation itself is a POI
        if (IsPOITag(detail.Tags, tagFilter))
        {
            pois.Add(BuildPOIEntry(detail.Id, "relation", detail.Tags, default));
        }

        // Examine member nodes/ways for POI data
        foreach (var member in detail.Members)
        {
            if (member.Type == "node")
            {
                var nodeInfo = OsmOnlineApiService.GetNodeInfo(member.Id);
                if (nodeInfo == null) continue;

                if (IsPOITag(nodeInfo.Tags, tagFilter))
                {
                    pois.Add(BuildPOIEntry(member.Id, "node", nodeInfo.Tags,
                        new Vector2DD(nodeInfo.Lon, nodeInfo.Lat)));
                }
            }
            else if (member.Type == "way")
            {
                var wayTags = OsmOnlineApiService.GetWayTags(member.Id);
                if (IsPOITag(wayTags, tagFilter))
                {
                    pois.Add(BuildPOIEntry(member.Id, "way", wayTags, default));
                }
            }
        }
    }

    /// <summary>
    /// 在线 API 不支持边界框扫描，已废弃
    /// </summary>
    private void CollectPOIsFromBoundingBox(MapInfo mapInfo, List<Dictionary<string, object?>> pois, string? tagFilter)
    {
        // 在线 API 不支持按边界框扫描所有节点，跳过此策略
    }

    /// <summary>
    /// Check if OSM tags indicate a Point of Interest
    /// </summary>
    private static bool IsPOITag(Dictionary<string, string> tags, string? tagFilter)
    {
        // If a specific tag filter is provided, check for it
        if (!string.IsNullOrEmpty(tagFilter))
        {
            return tags.ContainsKey(tagFilter) || tags.Keys.Any(k => k.StartsWith(tagFilter + ":"));
        }

        // Default: check for common POI tags
        string[] poiTagKeys = {
            "tourism", "aeroway", "harbour", "amenity",
            "historic", "leisure", "natural", "man_made",
            "shop", "office", "healthcare"
        };

        foreach (var key in poiTagKeys)
        {
            if (tags.ContainsKey(key))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Build a POI entry dictionary from OSM data
    /// </summary>
    private static Dictionary<string, object?> BuildPOIEntry(long id, string osmType, Dictionary<string, string> tags, Vector2DD location)
    {
        // Extract name from tags
        string? name = tags.TryGetValue("name", out var n) ? n : null;

        // Determine POI category from tags
        string? category = DeterminePOICategory(tags);

        // Collect relevant tags (not all, just the meaningful ones)
        var relevantTags = new Dictionary<string, string>();
        string[] importantKeys = { "tourism", "aeroway", "harbour", "amenity", "historic", "leisure",
                                    "natural", "man_made", "shop", "name", "name:en", "name:zh",
                                    "wikidata", "opening_hours", "website", "phone", "cuisine" };

        foreach (var key in importantKeys)
        {
            if (tags.TryGetValue(key, out var val))
            {
                relevantTags[key] = val;
            }
        }

        var entry = new Dictionary<string, object?>
        {
            ["osm_id"] = id,
            ["osm_type"] = osmType,
            ["name"] = name,
            ["category"] = category,
            ["tags"] = relevantTags,
        };

        if (location.X != 0 || location.Y != 0)
        {
            entry["longitude"] = location.X;
            entry["latitude"] = location.Y;
        }

        return entry;
    }

    /// <summary>
    /// Determine POI category from OSM tags
    /// </summary>
    private static string? DeterminePOICategory(Dictionary<string, string> tags)
    {
        if (tags.TryGetValue("tourism", out var tourism)) return $"tourism={tourism}";
        if (tags.TryGetValue("aeroway", out var aeroway)) return $"aeroway={aeroway}";
        if (tags.TryGetValue("harbour", out var harbour)) return $"harbour={harbour}";
        if (tags.TryGetValue("amenity", out var amenity)) return $"amenity={amenity}";
        if (tags.TryGetValue("historic", out var historic)) return $"historic={historic}";
        if (tags.TryGetValue("leisure", out var leisure)) return $"leisure={leisure}";
        if (tags.TryGetValue("natural", out var natural)) return $"natural={natural}";
        if (tags.TryGetValue("shop", out var shop)) return $"shop={shop}";
        return null;
    }

    /// <summary>
    /// 从在线 API 获取 OSM 元素的 tags 和坐标
    /// </summary>
    private Dictionary<string, string>? GetOSMTags(long osmId, string osmType, out Vector2DD location)
    {
        location = default;
        if (!OsmOnlineApiService.OK) return null;

        switch (osmType.ToLowerInvariant())
        {
            case "node":
                var nodeInfo = OsmOnlineApiService.GetNodeInfo(osmId);
                if (nodeInfo != null)
                {
                    location = new Vector2DD(nodeInfo.Lon, nodeInfo.Lat);
                    return nodeInfo.Tags;
                }
                break;

            case "way":
                return OsmOnlineApiService.GetWayTags(osmId);

            case "relation":
                var tags = OsmOnlineApiService.GetRelationTags(osmId);
                if (tags.Count > 0) return tags;
                break;
        }

        return null;
    }

    /// <summary>
    /// Create a GeoAttraction instance from OSM tags
    /// </summary>
    private GeoAttraction CreateAttraction(GeoLocation parent, Dictionary<string, string>? tags, Vector2DD location, long osmId)
    {
        var attraction = new GeoAttraction(parent);

        if (tags != null)
        {
            // Set name from OSM tags
            var name = LanguageData.CreateWithTags(tags);
            if (name != null)
            {
                attraction.attractionName = name;
                attraction.attractionName.Parent = attraction;
            }

            attraction.Name = name;
            if (attraction.Name != null)
                attraction.Name.Parent = attraction;

            // Set attraction type
            if (tags.TryGetValue("tourism", out var tourism))
                attraction.attractionType = tourism;
            else if (tags.TryGetValue("historic", out var historic))
                attraction.attractionType = historic;
            else if (tags.TryGetValue("leisure", out var leisure))
                attraction.attractionType = leisure;

            // Set coordinates
            attraction.longitude = location.X;
            attraction.latitude = location.Y;

            // Set other fields from tags
            if (tags.TryGetValue("opening_hours", out var hours))
                attraction.openingHours = hours;
            if (tags.TryGetValue("phone", out var phone))
                attraction.contactPhone = phone;
            if (tags.TryGetValue("website", out var website))
                attraction.officialWebsite = website;
            if (tags.TryGetValue("wikidata", out var wd))
                attraction.wikidata = wd;
        }

        attraction.OSMID = osmId;
        attraction.OSCType = OSMRelationRefType.Relations;
        attraction.ID = GenerateEntityID(tags, attraction.Name?.ToString() ?? "UNKNW", parent);

        return attraction;
    }

    /// <summary>
    /// Create a GeoAirport instance from OSM tags
    /// </summary>
    private GeoAirport CreateAirport(GeoLocation parent, Dictionary<string, string>? tags, Vector2DD location, long osmId)
    {
        var airport = new GeoAirport(parent);

        if (tags != null)
        {
            var name = LanguageData.CreateWithTags(tags);
            if (name != null)
            {
                airport.facilityName = name;
                airport.facilityName.Parent = airport;
            }

            airport.Name = name;
            if (airport.Name != null)
                airport.Name.Parent = airport;

            // Set IATA/ICAO codes
            if (tags.TryGetValue("iata", out var iata))
                airport.iataCode = iata;
            if (tags.TryGetValue("icao", out var icao))
                airport.icaoCode = icao;

            airport.longitude = location.X;
            airport.latitude = location.Y;

            if (tags.TryGetValue("wikidata", out var wd))
                airport.wikidata = wd;
        }

        airport.OSMID = osmId;
        airport.OSCType = OSMRelationRefType.Relations;
        airport.ID = GenerateEntityID(tags, airport.Name?.ToString() ?? "UNKNW", parent);

        return airport;
    }

    /// <summary>
    /// Create a GeoPort instance from OSM tags
    /// </summary>
    private GeoPort CreatePort(GeoLocation parent, Dictionary<string, string>? tags, Vector2DD location, long osmId)
    {
        var port = new GeoPort(parent);

        if (tags != null)
        {
            var name = LanguageData.CreateWithTags(tags);
            if (name != null)
            {
                port.facilityName = name;
                port.facilityName.Parent = port;
            }

            port.Name = name;
            if (port.Name != null)
                port.Name.Parent = port;

            port.longitude = location.X;
            port.latitude = location.Y;

            if (tags.TryGetValue("wikidata", out var wd))
                port.wikidata = wd;
        }

        port.OSMID = osmId;
        port.OSCType = OSMRelationRefType.Relations;
        port.ID = GenerateEntityID(tags, port.Name?.ToString() ?? "UNKNW", parent);

        return port;
    }

    /// <summary>
    /// Create a GeoLocation subclass from OSM tags based on admin_level
    /// </summary>
    private GeoLocation CreateGeoLocationFromTags(GeoLocation parent, Dictionary<string, string> tags, long osmId)
    {
        // Determine admin level
        AdminLevel adminLevel = AdminLevel.Township; // default
        if (tags.TryGetValue("admin_level", out var adminLevelStr) && int.TryParse(adminLevelStr, out var al))
        {
            if (Enum.IsDefined(typeof(AdminLevel), al))
            {
                adminLevel = (AdminLevel)al;
            }
        }

        // Create appropriate GeoLocation subclass based on admin level
        GeoLocation child = adminLevel switch
        {
            AdminLevel.National => new GeoCountry(parent),
            AdminLevel.Provincial or AdminLevel.Municipality or AdminLevel.AutonomousRegion or AdminLevel.State => new GeoProvince(parent),
            AdminLevel.Prefecture => new GeoCity(parent),
            AdminLevel.County => new GeoCounty(parent),
            _ => new GeoCounty(parent) // default to county for unknown levels
        };

        // Set basic properties from OSM tags
        child.OSMID = osmId;
        child.OSCType = OSMRelationRefType.Relations;

        // Set Name
        var name = LanguageData.CreateWithTags(tags);
        if (name != null)
        {
            child.Name = name;
            child.Name.Parent = child;
        }

        // Set ID from tags (ISO codes) or generate fallback
        child.ID = child.GetID(tags);

        // Set wikidata
        if (tags.TryGetValue("wikidata", out var wd))
        {
            child.wikidata = wd;
        }

        // Set AreaType
        if (tags.TryGetValue("boundary", out var boundary))
        {
            child.AreaType = boundary;
        }

        return child;
    }

    /// <summary>
    /// Mount a POI (attraction/airport/port) to the appropriate list on the parent entity
    /// </summary>
    private void MountPOIToEntity(GeoLocation parent, GeoLocation poi, string poiType)
    {
        // POIs are typically mounted under a GeoCounty or GeoCity
        // We need to find or create the appropriate list on the parent

        // For counties/cities, mount directly
        switch (poiType)
        {
            case "attraction":
                MountAttraction(parent, poi as GeoAttraction ??
                    throw new ArgumentException("Expected GeoAttraction"));
                break;
            case "airport":
                MountAirport(parent, poi as GeoAirport ??
                    throw new ArgumentException("Expected GeoAirport"));
                break;
            case "port":
                MountPort(parent, poi as GeoPort ??
                    throw new ArgumentException("Expected GeoPort"));
                break;
        }
    }

    /// <summary>
    /// Mount an attraction to the entity's attractions list
    /// </summary>
    private void MountAttraction(GeoLocation parent, GeoAttraction attraction)
    {
        try
        {
            var attractionsList = parent.GetAttractions();
            if (attractionsList is GeoList<GeoAttraction> list)
            {
                list.Add(attraction);
                return;
            }
        }
        catch { /* GetAttractions may throw NotImplementedException */ }

        // If GetAttractions() is not implemented, try using SubArea
        MountToSubArea(parent, attraction);
    }

    /// <summary>
    /// Mount an airport to the entity
    /// </summary>
    private void MountAirport(GeoLocation parent, GeoAirport airport)
    {
        MountToSubArea(parent, airport);
    }

    /// <summary>
    /// Mount a port to the entity
    /// </summary>
    private void MountPort(GeoLocation parent, GeoPort port)
    {
        MountToSubArea(parent, port);
    }

    /// <summary>
    /// Generic mounting: add child to parent's SubArea list
    /// </summary>
    private void MountToSubArea(GeoLocation parent, GeoLocation child)
    {
        try
        {
            var subArea = parent.GetSubArea();
            if (subArea is IList<GeoLocation> list)
            {
                list.Add(child);
                return;
            }
        }
        catch { /* GetSubArea may throw NotImplementedException */ }

        // Try setting SubArea property directly for types that have it
        var subAreaProp = parent.GetType().GetProperty("SubArea");
        if (subAreaProp != null && subAreaProp.CanRead)
        {
            var subAreaValue = subAreaProp.GetValue(parent);
            if (subAreaValue is IList<GeoLocation> list)
            {
                list.Add(child);
            }
            else if (subAreaValue == null && subAreaProp.CanWrite)
            {
                // Create a new list via IObjectFactory (avoid Activator.CreateInstance — forbidden by plugin security scan)
                var newList = CreateGeoList(parent);
                if (newList != null)
                {
                    newList.Add(child);
                    subAreaProp.SetValue(parent, newList);
                }
            }
        }
    }

    /// <summary>
    /// Mount a sub-area child to the parent entity
    /// </summary>
    private void MountSubAreaToEntity(GeoLocation parent, GeoLocation child)
    {
        var subAreaProp = parent.GetType().GetProperty("SubArea");
        if (subAreaProp != null)
        {
            var subAreaValue = subAreaProp.GetValue(parent);
            if (subAreaValue is IList<GeoLocation> list)
            {
                list.Add(child);
            }
            else if (subAreaValue == null && subAreaProp.CanWrite)
            {
                // Create a new list via IObjectFactory (avoid Activator.CreateInstance — forbidden by plugin security scan)
                var newList = CreateGeoList(parent);
                if (newList != null)
                {
                    newList.Add(child);
                    subAreaProp.SetValue(parent, newList);
                }
            }
        }
        else
        {
            // Fallback: try GetSubArea()
            try
            {
                var subArea = parent.GetSubArea();
                if (subArea is IList<GeoLocation> list)
                {
                    list.Add(child);
                }
            }
            catch { }
        }
    }

    /// <summary>
    /// Generate an entity ID from OSM tags (ISO codes) or fallback
    /// </summary>
    private string GenerateEntityID(Dictionary<string, string>? tags, string fallbackName, GeoLocation parent)
    {
        if (tags != null)
        {
            // Try ISO codes first
            if (tags.ContainsKey("ISO3166-1"))
                return tags["ISO3166-1"];

            if (tags.ContainsKey("ISO3166-2"))
            {
                string iso = tags["ISO3166-2"];
                string[] parts = iso.Split('-');
                if (parts.Length == 2)
                    return parts[1];
            }
        }

        // Generate a fallback ID from the name
        string baseId = new string(fallbackName.Where(char.IsLetter).ToArray()).ToUpper();
        if (baseId.Length >= 5)
            baseId = baseId.Substring(0, 5);
        else if (baseId.Length > 0)
            baseId = baseId.PadRight(5, 'X');
        else
            baseId = "UNKNW";

        // Check for duplicate IDs among siblings
        try
        {
            var subArea = parent.GetSubArea();
            var existingIds = new List<string>();
            foreach (GeoLocation sibling in subArea)
            {
                if (!string.IsNullOrEmpty(sibling.ID))
                    existingIds.Add(sibling.ID);
            }

            string finalId = baseId;
            int counter = 1;
            while (existingIds.Contains(finalId))
            {
                char lastChar = (char)('A' + (counter % 26));
                finalId = baseId.Substring(0, Math.Min(4, baseId.Length)) + lastChar;
                counter++;
                if (counter > 26)
                {
                    finalId = "UNKNW";
                    break;
                }
            }

            return finalId;
        }
        catch
        {
            return baseId;
        }
    }

    /// <summary>
    /// Creates a GeoList&lt;GeoLocation&gt; instance via IObjectFactory.
    /// Avoids Activator.CreateInstance which is forbidden by the plugin security scanner
    /// (ForbiddenMember: System.Actator.CreateInstance).
    /// Falls back to direct construction for GeoList&lt;GeoLocation&gt; since the type is known at compile time.
    /// </summary>
    private static GeoList<GeoLocation>? CreateGeoList(GeoLocation parent)
    {
        // Prefer IObjectFactory if registered (consistent with plugin architecture)
        var objectFactory = ServiceLocator.Instance.ObjectFactory;
        if (objectFactory != null && objectFactory.IsRegistered(typeof(GeoList<GeoLocation>)))
        {
            return objectFactory.CreateInstance<GeoList<GeoLocation>>(parent);
        }

        // Direct construction: GeoList<GeoLocation> is a known type with a (GeoLocation) constructor.
        // This is safe because we're not using Activator.CreateInstance — the type is statically known.
        return new GeoList<GeoLocation>(parent);
    }

    /// <summary>
    /// Check if a code is already used by a sibling entity
    /// </summary>
    private string? FindDuplicateCode(GeoLocation entity, string code)
    {
        // Get the parent entity
        var parent = entity.GetParent() as GeoLocation;
        if (parent == null) return null;

        try
        {
            var subArea = parent.GetSubArea();
            foreach (GeoLocation sibling in subArea)
            {
                if (sibling.ID == code && sibling != entity)
                {
                    return sibling.FullID;
                }
            }
        }
        catch { }

        return null;
    }
}
