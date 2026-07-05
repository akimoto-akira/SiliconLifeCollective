using System.Reflection;
using SiliconLife.Collective;
using CollectiveTaskStatus = SiliconLife.Collective.TaskStatus;
using SiliconLife.Common;
using TravelCodeWikiWithAI.Cldr;
using TravelCodeWikiWithAI.Data;
using TravelCodeWikiWithAI.Data.OSM;
using TravelCodeWikiWithAI.TCWTool;

namespace TravelCodeWikiWithAI;

/// <summary>
/// 工作流7步调度中枢。
/// 每帧按优先级扫描地理实体树，创建一个最高优先级的任务。
/// 
/// 扫描优先级：
/// 1) 步骤4：发现ID为空或低质量 → 创建[编码分配]任务（CodeAssigner）
/// 2) 步骤3：发现未归类POI → 创建[POI分类]任务（POIClassifier）
/// 3) 步骤5：发现内容字段为空 → 创建[内容编写]任务（ContentWriter）
/// 4) 步骤6：发现LanguageData缺失语言 → 创建[翻译]任务（Translator）
/// 5) 步骤7：发现文档就绪 → 触发BuildDocument+发布
/// 
/// 约束：每帧只创建一个任务，防止任务爆炸。
/// 前置条件：OsmOnlineApiService.OK == true
/// 任务创建时从角色池取执行者（依赖task-350）。
/// </summary>
public class WikiPublicationTick : TickObject
{
    /// <summary>
    /// 记录已尝试展开但无法再展开的父节点 OSM ID，防止 FindEmptyParent 反复返回同一死节点。
    /// </summary>
    private readonly HashSet<long> _fullyExpandedParents = new();
    private OsmOnlineApiService? _osmApi;

    public WikiPublicationTick() : base(TimeSpan.FromSeconds(2), autoRegister: true)
    {
    }

    protected override void OnTick(TimeSpan deltaTime)
    {
        if (!OsmOnlineApiService.OK)
        {
            return;
        }

        // 初始化 OsmOnlineApiService 实例（使用 Curator 的 ID 作为 callerId）
        var curator = SiliconBeingManager.GetCuratorBeing();
        if (curator != null)
        {
            _osmApi = new OsmOnlineApiService(curator.Id);
        }
        else if (_osmApi == null)
        {
            return; // 没有 Curator 且尚未初始化
        }

        ProcessOneTaskPerTick();
    }

    /// <summary>
    /// 每帧只做一件事：按优先级扫描，创建一个最高优先级的任务。
    /// </summary>
    private void ProcessOneTaskPerTick()
    {
        GeoProject? geoProject = TravelCodeWikiWithAIPlugin._geoProject;
        if (geoProject == null)
        {
            return;
        }

        GeoWorld? world = geoProject.World;

        List<ProjectTaskSystem> taskSystems = GetProjectTaskSystems();
        if (taskSystems.Count == 0)
        {
            return;
        }

        string[] baseLanguages = SysTool.GetBaseLanguage();

        foreach (var taskSystem in taskSystems)
        {
            // 优先级1: 步骤4 — 编码分配
            if (world != null && ScanForCodeAssignment(world, taskSystem))
            {
                return;
            }

            if (world.Continents.Count == 0)
            {
                return;
            }
            // 优先级2: 步骤3 — POI分类
            if (ScanForPOIClassification(geoProject, taskSystem))
            {
                return;
            }

            // 优先级3: 步骤5 — 内容编写
            if (ScanForContentCreation(geoProject, taskSystem, baseLanguages))
            {
                return;
            }

            // 优先级4: 步骤6 — 翻译
            if (ScanForTranslation(geoProject, taskSystem, baseLanguages))
            {
                return;
            }

            // 优先级5: 步骤7 — 文档就绪，触发BuildDocument+发布
            if (ScanForPublishing(geoProject, taskSystem, baseLanguages))
            {
                return;
            }
        }
    }

    // ========== 步骤4：编码分配（最高优先级） ==========

    /// <summary>
    /// 每帧找一个空的父地理实体（无子级），为其展开完整一层子级。
    /// 有 ISO 或固定 id 的直接创建实体，无 ID 的发布 AI 编码分配任务。
    /// 正在由 AI 处理的子实体跳过，继续处理其余的。
    /// </summary>
    private bool ScanForCodeAssignment(GeoWorld world, ProjectTaskSystem taskSystem)
    {
        if (!OsmOnlineApiService.OK)
        {
            return false;
        }

        var emptyParent = FindEmptyParent(world);
        if (emptyParent == null)
        {
            return false;
        }

        long parentOsmId = (emptyParent == world) ? -1 : emptyParent.OSMID;
        bool expanded = ExpandOneLayer(emptyParent, parentOsmId, taskSystem);
        if (!expanded)
        {
            // 该节点无法再展开，标记为已完成，下次跳过
            _fullyExpandedParents.Add(parentOsmId);
        }
        return expanded;
    }

    /// <summary>
    /// 在地理实体树中找到第一个无子级的实体（有 OSM ID 的叶子节点）。
    /// world 根节点也视为候选（用于初始展开大洲层）。
    /// </summary>
    private GeoLocation FindEmptyParent(GeoLocation location)
    {
        bool hasChildren = false;
        try
        {
            var subArea = location.GetSubArea();
            if (subArea != null)
            {
                var enumerator = subArea.GetEnumerator();
                hasChildren = enumerator.MoveNext();
            }
        }
        catch { }

        if (!hasChildren)
        {
            if (location is GeoWorld)
            {
                // world 用 -1 标识，若已标记为完成则跳过
                if (_fullyExpandedParents.Contains(-1)) return null;
                return location;
            }
            if (location.OSMID > 0 && !_fullyExpandedParents.Contains(location.OSMID))
            {
                return location;
            }
            return null;
        }

        try
        {
            var subArea = location.GetSubArea();
            foreach (GeoLocation child in subArea)
            {
                var result = FindEmptyParent(child);
                if (result != null)
                {
                    return result;
                }
            }
        }
        catch { }

        return null;
    }

    /// <summary>
    /// 固定 OSM ID 映射。由于地图文件体积限制，部分行政区域的 OSM ID 与名称需要预设。
    /// 参数 parentOsmId 为父实体的 OSM ID，-1 代表顶级（世界根节点）。
    /// 返回值为该父实体下预设的子实体结构体数组。
    /// </summary>
    private FixedOsmMapping[] GetFixedOsmIdMapping(long parentOsmId)
    {
        if (parentOsmId == -1)
        {
            return
            [
                new FixedOsmMapping(36966065, "AS", "亚洲", "Asia", OSMElementType.Node),
                new FixedOsmMapping(25871341, "EU", "欧洲", "Europe", OSMElementType.Node),
                new FixedOsmMapping(36966057, "AF", "非洲", "Africa", OSMElementType.Node),
                new FixedOsmMapping(36966063, "NA", "北美洲", "North America", OSMElementType.Node),
                new FixedOsmMapping(36966069, "SA", "南美洲", "South America", OSMElementType.Node),
                new FixedOsmMapping(249399679, "OC", "大洋洲", "Oceania", OSMElementType.Node),
                new FixedOsmMapping(36966060, "AN", "南极洲", "Antarctica", OSMElementType.Node),
            ];
        }

        if (parentOsmId == 36966065)
        {
            return
            [
                new FixedOsmMapping(270056, "CN", "中国", "China", OSMElementType.Relation),
            ];
        }

        return Array.Empty<FixedOsmMapping>();
    }

    /// <summary>
    /// 为指定父实体展开完整一层子级：查询固定映射和在线 OSM Relation，
    /// 处理所有子 OSM（创建实体或发布任务），而非只处理一个。
    /// </summary>
    private bool ExpandOneLayer(GeoLocation parent, long parentOsmId, ProjectTaskSystem taskSystem)
    {
        bool anyAction = false;

        var fixedChildren = GetFixedOsmIdMapping(parentOsmId);
        foreach (var childInfo in fixedChildren)
        {
            long childOsmId = childInfo.OsmId;

            if (IsOsmIdMounted(parent, childOsmId))
                continue;

            string objectKey = $"OSM-{childInfo.ElementType}-{childInfo.OsmId}";
            if (HasExistingTask(taskSystem, objectKey, "CodeAssignment"))
                continue;

            string fixedId = childInfo.Id;
            var childTags = GetOsmElementTags(childOsmId, childInfo.ElementType);
            string resolvedId = ResolveEntityId(childTags, fixedId);

            if (resolvedId != null)
            {
                CreateAndMountEntity(parent, childTags, childInfo, childOsmId, resolvedId);
                anyAction = true;
            }
            else
            {
                PublishCodeAssignmentTask(taskSystem, childOsmId, childTags, childInfo, objectKey, parent.FullID);
                anyAction = true;
            }
        }

        if (parentOsmId > 0)
        {
            var relInfo = _osmApi?.GetRelationInfo(parentOsmId);
            if (relInfo != null)
            {
                foreach (var member in relInfo.Refs)
                {
                    string role = member.Role ?? "";
                    if (role != "" && role != "admin" && role != "label" && role != "subarea" && role != "child")
                        continue;

                    if (IsOsmIdMounted(parent, member.Id))
                        continue;

                    if (fixedChildren.Any(c => c.OsmId == member.Id))
                        continue;

                    string objectKey = $"OSM-{member.Id}";
                    if (HasExistingTask(taskSystem, objectKey, "CodeAssignment"))
                        continue;

                    var childTags = GetOsmElementTags(member.Id, OSMElementType.Relation);
                    if (childTags == null || childTags.Count == 0)
                        continue;

                    if (!childTags.TryGetValue("boundary", out string boundary) || boundary != "administrative")
                        continue;

                    string resolvedId = ResolveEntityId(childTags, null);

                    if (resolvedId != null)
                    {
                        CreateAndMountEntity(parent, childTags, null, member.Id, resolvedId);
                        anyAction = true;
                    }
                    else
                    {
                        PublishCodeAssignmentTask(taskSystem, member.Id, childTags, null, objectKey, parent.FullID);
                        anyAction = true;
                    }
                }
            }
        }

        return anyAction;
    }

    /// <summary>
    /// 根据元素类型从在线 API 获取 OSM 实体的 tags，找不到返回空字典
    /// </summary>
    private Dictionary<string, string> GetOsmElementTags(long osmId, OSMElementType elementType)
    {
        try
        {
            return elementType switch
            {
                OSMElementType.Node => _osmApi?.GetNodeTags(osmId) ?? new Dictionary<string, string>(),
                OSMElementType.Way => _osmApi?.GetWayTags(osmId) ?? new Dictionary<string, string>(),
                _ => _osmApi?.GetRelationTags(osmId) ?? new Dictionary<string, string>(),
            };
        }
        catch
        {
            return new Dictionary<string, string>();
        }
    }

    /// <summary>
    /// 解析实体 ID：ISO3166 标签 > 固定映射 id > null（需 AI 分配）
    /// </summary>
    private string ResolveEntityId(Dictionary<string, string> tags, string fixedId)
    {
        if (tags != null && tags.Count > 0)
        {
            if (tags.ContainsKey("ISO3166-1"))
            {
                return tags["ISO3166-1"];
            }
            if (tags.ContainsKey("ISO3166-2"))
            {
                string iso2 = tags["ISO3166-2"];
                string[] parts = iso2.Split('-');
                if (parts.Length == 2)
                {
                    return parts[1];
                }
                return iso2;
            }
        }
        return fixedId;
    }

    /// <summary>
    /// 创建地理实体并挂载到父实体。
    /// 实体包含 ID（不超过5字母）和 OSM ID（long 格式），以 world 为根形成树。
    /// </summary>
    private void CreateAndMountEntity(GeoLocation parent, Dictionary<string, string> tags, FixedOsmMapping? fixedInfo, long osmId, string resolvedId)
    {
        bool hasTags = tags != null && tags.Count > 0;

        AdminLevel adminLevel = AdminLevel.Township;
        if (hasTags && tags.TryGetValue("admin_level", out var adminLevelStr) && int.TryParse(adminLevelStr, out var al))
        {
            if (Enum.IsDefined(typeof(AdminLevel), al))
            {
                adminLevel = (AdminLevel)al;
            }
        }

        GeoLocation child;
        if (parent is GeoWorld)
        {
            child = new GeoContinent(parent);
        }
        else
        {
            child = adminLevel switch
            {
                AdminLevel.National => new GeoCountry(parent),
                AdminLevel.Provincial or AdminLevel.Municipality or AdminLevel.AutonomousRegion or AdminLevel.State => new GeoProvince(parent),
                AdminLevel.Prefecture => new GeoCity(parent),
                AdminLevel.County => new GeoCounty(parent),
                _ => new GeoCounty(parent)
            };
        }

        child.OSMID = osmId;
        child.OSCType = fixedInfo.HasValue
            ? fixedInfo.Value.ElementType switch
            {
                OSMElementType.Node => OSMRelationRefType.Node,
                OSMElementType.Way => OSMRelationRefType.Way,
                _ => OSMRelationRefType.Relations,
            }
            : OSMRelationRefType.Relations;
        child.ID = resolvedId;

        if (hasTags)
        {
            var name = LanguageData.CreateWithTags(tags);
            if (name != null)
            {
                child.Name = name;
                child.Name.Parent = child;
            }

            if (tags.TryGetValue("wikidata", out var wd))
            {
                child.wikidata = wd;
            }

            if (tags.TryGetValue("wikipedia", out var wp))
            {
                child.wikipedia = wp;
            }

            if (tags.TryGetValue("boundary", out var boundaryTag))
            {
                child.AreaType = boundaryTag;
            }
        }
        else if (fixedInfo.HasValue)
        {
            var mapping = fixedInfo.Value;
            var name = new LanguageData();
            if (!string.IsNullOrEmpty(mapping.ZhCn))
                name["zh-cn"] = mapping.ZhCn;
            if (!string.IsNullOrEmpty(mapping.En))
                name["en"] = mapping.En;
            if (name.Count > 0)
            {
                child.Name = name;
                child.Name.Parent = child;
            }
        }

        MountSubAreaToEntity(parent, child);
    }

    /// <summary>
    /// 将子实体挂载到父实体的子级列表。
    /// 不同父类型使用不同的子级属性：GeoWorld→Continents, GeoContinent→Countries, 其他→SubArea。
    /// </summary>
    private void MountSubAreaToEntity(GeoLocation parent, GeoLocation child)
    {
        if (parent is GeoWorld world)
        {
            if (child is GeoContinent continent)
            {
                world.Continents.Add(continent);
            }
            return;
        }

        if (parent is GeoContinent geoContinent)
        {
            if (child is GeoCountry country)
            {
                geoContinent.Countries ??= new GeoList<GeoCountry>(geoContinent);
                geoContinent.Countries.Add(country);
            }
            return;
        }

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
    /// 通过 IObjectFactory 创建 GeoList 实例
    /// </summary>
    private IList<GeoLocation>? CreateGeoList(GeoLocation parent)
    {
        try
        {
            var objectFactory = ServiceLocator.Instance.ObjectFactory;
            if (objectFactory != null && objectFactory.IsRegistered(typeof(GeoList<GeoLocation>)))
            {
                var list = objectFactory.CreateInstance<GeoList<GeoLocation>>(parent);
                if (list != null)
                {
                    return list;
                }
            }
        }
        catch { }

        return new GeoList<GeoLocation>(parent);
    }

    /// <summary>
    /// 检查指定 OSM ID 是否已挂载为 location 的子级
    /// </summary>
    private bool IsOsmIdMounted(GeoLocation location, long osmId)
    {
        try
        {
            var subArea = location.GetSubArea();
            if (subArea == null) return false;
            foreach (GeoLocation child in subArea)
            {
                if (child.OSMID == osmId)
                {
                    return true;
                }
            }
        }
        catch
        {
        }

        return false;
    }

    /// <summary>
    /// 发布编码分配任务：由 AI 参考当地读音/方言分配不超过5个大写英文字母的 ID
    /// </summary>
    private void PublishCodeAssignmentTask(ProjectTaskSystem taskSystem, long osmId, Dictionary<string, string> tags, FixedOsmMapping? fixedInfo, string objectKey, string parentPath)
    {
        string entityName;
        string nameList;
        string adminLevelStr = "?";

        if (tags != null && tags.Count > 0)
        {
            entityName = tags.TryGetValue("name", out string n) ? n :
                         tags.TryGetValue("name:en", out string ne) ? ne :
                         $"OSM-{osmId}";
            nameList = string.Join("、", tags
                .Where(kv => kv.Key.StartsWith("name"))
                .Select(kv => $"{kv.Key}:{kv.Value}"));
            if (tags.TryGetValue("admin_level", out var al))
            {
                adminLevelStr = al;
            }
        }
        else if (fixedInfo.HasValue)
        {
            var mapping = fixedInfo.Value;
            entityName = !string.IsNullOrEmpty(mapping.ZhCn) ? mapping.ZhCn :
                         !string.IsNullOrEmpty(mapping.En) ? mapping.En :
                         $"OSM-{osmId}";
            var parts = new List<string>();
            if (!string.IsNullOrEmpty(mapping.ZhCn))
                parts.Add($"zh-cn:{mapping.ZhCn}");
            if (!string.IsNullOrEmpty(mapping.En))
                parts.Add($"en:{mapping.En}");
            nameList = string.Join("、", parts);
        }
        else
        {
            entityName = $"OSM-{osmId}";
            nameList = "";
        }

        string title = $"为 {entityName} 分配标识编码";
        string description = $"OSM Relation #{osmId}（{entityName}，admin_level={adminLevelStr}）需要分配标识编码。\n\n" +
                             $"父实体路径：{parentPath}\n" +
                             $"多语言名称：{nameList}\n" +
                             $"OSM ID：{osmId}\n\n" +
                             $"请参考当地读音或方言，使用 GeoDataTool 的 assign_code 动作为该实体分配不超过5个大写英文字母的标识编码";

        var executorGuid = SelectExecutorFromRole(taskSystem, "CodeAssigner");
        var task = taskSystem.Create(title, description, executorGuid ?? Guid.Empty, executorGuid ?? Guid.Empty, priority: 40);
        task.Metadata["TaskType"] = "CodeAssignment";
        task.Metadata["ObjectPath"] = objectKey;
        task.Metadata["OsmId"] = osmId.ToString();
        task.Metadata["ParentPath"] = parentPath;
        task.Metadata["Step"] = "4";
        TaskCenter.Instance.UpdateTask(task);
    }

    // ========== 步骤3：POI分类 ==========

    /// <summary>
    /// 扫描地理实体树，发现未归类的OSM POI数据，创建POI分类任务。
    /// </summary>
    private bool ScanForPOIClassification(GeoDataBase geoData, ProjectTaskSystem taskSystem)
    {
        // 检查该实体下是否有未归类的POI（有OSMID但没有被归类到具体子类型）
        if (geoData is GeoLocation location)
        {
            // 对于有OSM数据的实体，检查其子区域是否有未归类的POI
            if (location.OSMID > 0 && HasUnclassifiedPOIs(location))
            {
                string objectPath = location.FullID;
                if (!string.IsNullOrEmpty(objectPath) && !HasExistingTask(taskSystem, objectPath, "POIClassification"))
                {
                    PublishPOIClassificationTask(taskSystem, location, objectPath);
                    return true;
                }
            }
        }

        // 递归扫描子对象
        PropertyInfo[] pis = geoData.GetType().GetProperties();
        foreach (PropertyInfo pi in pis)
        {
            if (!pi.CanRead || !pi.CanWrite) continue;
            MethodInfo[] mis = pi.GetAccessors();
            if (mis[0].IsStatic) continue;

            Type b = pi.PropertyType;
            if (b.IsSubclassOf(typeof(GeoDataBase)))
            {
                GeoDataBase? child = pi.GetValue(geoData) as GeoDataBase;
                if (child == null) continue;

                if (ScanForPOIClassification(child, taskSystem))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 检查实体是否有未归类的POI。
    /// 判断标准：实体有OSM数据（OSMID > 0），但其子区域列表中仍有未挂载到具体类型的POI。
    /// 简化实现：检查景点列表是否为空但有OSM数据。
    /// </summary>
    private bool HasUnclassifiedPOIs(GeoLocation location)
    {
        // 检查该实体的子区域是否为空（意味着还没有从OSM数据中分类POI）
        try
        {
            var subArea = location.GetSubArea();
            var attractions = location.GetAttractions();

            // 如果有OSM数据但景点列表为空，可能有未归类的POI
            // 这里的逻辑是简化版，实际需要检查OSM数据中是否有未挂载的POI节点
            if (location.OSMID > 0 && attractions != null && attractions.Count == 0)
            {
                return true;
            }
        }
        catch
        {
            // GetSubArea/GetAttractions 可能抛出 NotImplementedException
        }

        return false;
    }

    private void PublishPOIClassificationTask(ProjectTaskSystem taskSystem, GeoLocation location, string objectPath)
    {
        string entityName = location.Name?.ToString() ?? location.ID ?? objectPath;
        string title = $"为 {entityName} 归类POI数据";
        string description = $"地理实体 \"{entityName}\"（路径：{objectPath}）有OSM数据但POI尚未归类挂载。\n\n" +
                             $"请使用 GeoDataTool 的 list_osm_pois 动作列出该区域内的OSM POI数据，\n" +
                             $"然后使用 classify_poi 动作将POI归类并挂载到地理实体树（创建 GeoAttraction/GeoAirport/GeoPort 节点）。\n" +
                             $"最后使用 expand_children 展开实体的子区域（如有必要）。";

        var executorGuid = SelectExecutorFromRole(taskSystem, "POIClassifier");
        var task = taskSystem.Create(title, description, executorGuid ?? Guid.Empty, executorGuid ?? Guid.Empty, priority: 30);
        task.Metadata["TaskType"] = "POIClassification";
        task.Metadata["ObjectPath"] = objectPath;
        task.Metadata["Step"] = "3";
        TaskCenter.Instance.UpdateTask(task);
    }

    // ========== 步骤5：内容编写 ==========

    /// <summary>
    /// 扫描地理实体树，发现内容字段为空的实体，创建内容编写任务。
    /// 内容字段包括：Understand（基本了解）等 WordBase 类型属性。
    /// </summary>
    private bool ScanForContentCreation(GeoDataBase geoData, ProjectTaskSystem taskSystem, string[] baseLanguages)
    {
        if (geoData is GeoLocation location)
        {
            // 检查 Understand 字段是否为空（这是最基本的内容字段）
            if (location.Understand == null && location.OSMID > 0)
            {
                string objectPath = location.FullID;
                if (!string.IsNullOrEmpty(objectPath) && !HasExistingTask(taskSystem, objectPath, "ContentCreation"))
                {
                    PublishContentCreationTask(taskSystem, location, objectPath);
                    return true;
                }
            }
        }

        // 递归扫描子对象
        PropertyInfo[] pis = geoData.GetType().GetProperties();
        foreach (PropertyInfo pi in pis)
        {
            if (!pi.CanRead || !pi.CanWrite) continue;
            MethodInfo[] mis = pi.GetAccessors();
            if (mis[0].IsStatic) continue;

            Type b = pi.PropertyType;
            if (b.IsSubclassOf(typeof(GeoDataBase)))
            {
                GeoDataBase? child = pi.GetValue(geoData) as GeoDataBase;
                if (child == null) continue;

                if (ScanForContentCreation(child, taskSystem, baseLanguages))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void PublishContentCreationTask(ProjectTaskSystem taskSystem, GeoLocation location, string objectPath)
    {
        string entityName = location.Name?.ToString() ?? location.ID ?? objectPath;
        string title = $"为 {entityName} 编写内容文章";
        string description = $"地理实体 \"{entityName}\"（路径：{objectPath}）的内容字段为空，需要编写结构化MediaWiki富文本文章。\n\n" +
                             $"请使用 GeoContentTool 的工具为该实体编写内容：\n" +
                             $"1) 先用 list_sections 查看实体的所有内容字段及状态\n" +
                             $"2) 用 write_section 向空字段写入 MediaWikiWord 富文本对象树\n" +
                             $"3) 用 preview_wiki 预览生成的 wiki 标记输出\n\n" +
                             $"实体AI路径：{location.GetAIPath()}";

        var executorGuid = SelectExecutorFromRole(taskSystem, "ContentWriter");
        var task = taskSystem.Create(title, description, executorGuid ?? Guid.Empty, executorGuid ?? Guid.Empty, priority: 20);
        task.Metadata["TaskType"] = "ContentCreation";
        task.Metadata["ObjectPath"] = objectPath;
        task.Metadata["Step"] = "5";
        TaskCenter.Instance.UpdateTask(task);
    }

    // ========== 步骤6：翻译（保留已有逻辑，优化重构） ==========

    /// <summary>
    /// 扫描地理实体树，发现LanguageData缺失语言的实体，创建翻译任务。
    /// 保留原有逻辑，但增加步骤标记。
    /// </summary>
    private bool ScanForTranslation(GeoDataBase geoData, ProjectTaskSystem taskSystem, string[] baseLanguages)
    {
        PropertyInfo[] pis = geoData.GetType().GetProperties();
        List<PropertyInfo> ps = [];
        foreach (PropertyInfo pi in pis)
        {
            if (pi.CanRead && pi.CanWrite)
            {
                MethodInfo[] mis = pi.GetAccessors();
                if (!mis[0].IsStatic)
                {
                    ps.Add(pi);
                }
            }
        }

        foreach (PropertyInfo a in ps)
        {
            Type b = a.PropertyType;
            if (b.IsSubclassOf(typeof(GeoDataBase)))
            {
                GeoDataBase? c = (GeoDataBase?)a.GetValue(geoData);
                if (c == null)
                {
                    continue;
                }

                if (ScanForTranslation(c, taskSystem, baseLanguages))
                {
                    return true;
                }
            }
            else if (b == typeof(LanguageData))
            {
                LanguageData? d = (LanguageData?)a.GetValue(geoData);
                if (d == null)
                {
                    continue;
                }

                if (d.NoAutoSet)
                {
                    continue;
                }

                foreach (string lang in baseLanguages)
                {
                    if (!d.ContainsKey(lang))
                    {
                        string objectPath = geoData.GetObjectPath(d);

                        if (HasExistingTask(taskSystem, objectPath, lang))
                        {
                            continue;
                        }

                        PublishTranslationTask(taskSystem, d, objectPath, lang);
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void PublishTranslationTask(ProjectTaskSystem taskSystem, LanguageData languageData, string objectPath, string missingLanguage)
    {
        string chineseLanguage = GetChineseLanguage(missingLanguage);
        string title = $"翻译 {objectPath} 的 {chineseLanguage} 语言";
        string description = $"为路径 \"{objectPath}\" 的语言数据补充 \"{missingLanguage}（{chineseLanguage}）\" 语言翻译。\n\n" +
                             $"请使用 GeoLanguageTool 的 set_language 动作完成翻译。";

        if (languageData.ContainsKey("*"))
        {
            description += $"\n\n原始文本（通配符）: {languageData["*"]}";
        }

        if (languageData.ContainsKey("zh-cn"))
        {
            description += $"\n\n简体中文: {languageData["zh-cn"]}";
        }

        if (languageData.ContainsKey("en"))
        {
            description += $"\n\n英文: {languageData["en"]}";
        }

        var executorGuid = SelectExecutorFromRole(taskSystem, "Translator");

        var task = taskSystem.Create(
            title: title,
            description: description,
            assigneeGuid: executorGuid ?? Guid.Empty,
            executorGuid: executorGuid ?? Guid.Empty,
            priority: 10
        );
        task.Metadata["TaskType"] = "LanguageTranslation";
        task.Metadata["ObjectPath"] = objectPath;
        task.Metadata["MissingLanguage"] = missingLanguage;
        task.Metadata["Step"] = "6";
        TaskCenter.Instance.UpdateTask(task);
    }

    // ========== 步骤7：文档发布 ==========

    /// <summary>
    /// 扫描地理实体树，发现文档就绪的实体（文章+语言齐备），触发BuildDocument+发布。
    /// </summary>
    private bool ScanForPublishing(GeoDataBase geoData, ProjectTaskSystem taskSystem, string[] baseLanguages)
    {
        if (geoData is GeoLocation location)
        {
            if (IsDocumentReady(location, baseLanguages))
            {
                string objectPath = location.FullID;
                if (!string.IsNullOrEmpty(objectPath) && !HasExistingTask(taskSystem, objectPath, "WikiPublish"))
                {
                    PublishWikiPublishTask(taskSystem, location, objectPath);
                    return true;
                }
            }
        }

        // 递归扫描子对象
        PropertyInfo[] pis = geoData.GetType().GetProperties();
        foreach (PropertyInfo pi in pis)
        {
            if (!pi.CanRead || !pi.CanWrite) continue;
            MethodInfo[] mis = pi.GetAccessors();
            if (mis[0].IsStatic) continue;

            Type b = pi.PropertyType;
            if (b.IsSubclassOf(typeof(GeoDataBase)))
            {
                GeoDataBase? child = pi.GetValue(geoData) as GeoDataBase;
                if (child == null) continue;

                if (ScanForPublishing(child, taskSystem, baseLanguages))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 判断实体文档是否就绪。
    /// 条件：有ID、有内容（Understand不为空）、语言数据完整。
    /// </summary>
    private bool IsDocumentReady(GeoLocation location, string[] baseLanguages)
    {
        // 已发布的实体不再触发发布
        if (location.Published)
        {
            return false;
        }

        // 必须有有效ID
        if (string.IsNullOrEmpty(location.ID) || location.ID.StartsWith("UNKNW"))
        {
            return false;
        }

        // 必须有内容
        if (location.Understand == null)
        {
            return false;
        }

        // 名称的语言数据必须覆盖所有基础语言
        if (location.Name == null)
        {
            return false;
        }

        foreach (string lang in baseLanguages)
        {
            if (!location.Name.ContainsKey(lang))
            {
                return false;
            }
        }

        return true;
    }

    private void PublishWikiPublishTask(ProjectTaskSystem taskSystem, GeoLocation location, string objectPath)
    {
        string entityName = location.Name?.ToString() ?? location.ID ?? objectPath;
        string title = $"发布 {entityName} 到MediaWiki";
        string description = $"地理实体 \"{entityName}\"（路径：{objectPath}）的文档已就绪（文章+语言齐备），可以发布到MediaWiki。\n\n" +
                             $"请执行以下步骤：\n" +
                             $"1) 使用 GeoContentTool 的 preview_wiki 预览生成的 wiki 标记\n" +
                             $"2) 确认内容正确后使用 WikiPublishTool 的 publish 动作触发发布\n\n" +
                             $"实体AI路径：{location.GetAIPath()}";

        // 发布任务不需要特定角色，由系统执行
        var curator = SiliconBeingManager.GetCuratorBeing();
        Guid curatorId = curator?.Id ?? Guid.Empty;

        var task = taskSystem.Create(title, description, curatorId, curatorId, priority: 5);
        task.Metadata["TaskType"] = "WikiPublish";
        task.Metadata["ObjectPath"] = objectPath;
        task.Metadata["Step"] = "7";
        TaskCenter.Instance.UpdateTask(task);
    }

    // ========== 公共工具方法 ==========

    private List<ProjectTaskSystem> GetProjectTaskSystems()
    {
        var result = new List<ProjectTaskSystem>();
        var projectManager = ServiceLocator.Instance.ProjectManager;
        if (projectManager == null)
        {
            return result;
        }

        foreach (var project in projectManager.ListProjects())
        {
            if (project.WorkflowTemplateName == "TravelCodeWikiPublish")
            {
                var ts = projectManager.GetTaskSystem(project.Id);
                if (ts != null)
                {
                    result.Add(ts);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 检查是否已存在相同类型和目标的活动任务。
    /// 支持两种检查模式：
    /// 1) 按任务类型+ObjectPath+MissingLanguage检查（翻译任务）
    /// 2) 按任务类型+ObjectPath检查（其他任务）
    /// </summary>
    private bool HasExistingTask(ProjectTaskSystem taskSystem, string objectPath, string taskTypeOrLanguage)
    {
        foreach (TaskItem task in taskSystem.GetAll())
        {
            if (task.Status == CollectiveTaskStatus.Completed ||
                task.Status == CollectiveTaskStatus.Failed ||
                task.Status == CollectiveTaskStatus.Cancelled)
            {
                continue;
            }

            if (!task.Metadata.TryGetValue("ObjectPath", out string? path) || path != objectPath)
            {
                continue;
            }

            // 对于翻译任务，检查MissingLanguage
            if (task.Metadata.TryGetValue("MissingLanguage", out string? lang) && lang == taskTypeOrLanguage)
            {
                return true;
            }

            // 对于其他任务，检查TaskType
            if (task.Metadata.TryGetValue("TaskType", out string? type) && type == taskTypeOrLanguage)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 从项目角色池中选择执行者（轮询策略）。
    /// 与 TravelCodeWikiPublishWorkflow.SelectExecutorFromRole 逻辑一致，
    /// 但此处直接使用 ProjectSpace.RoleAssignments。
    /// </summary>
    private Guid? SelectExecutorFromRole(ProjectTaskSystem taskSystem, string roleName)
    {
        var projectManager = ServiceLocator.Instance.ProjectManager;
        if (projectManager == null) return null;

        // 找到关联的项目
        var project = projectManager.ListProjects()
            .FirstOrDefault(p => p.WorkflowTemplateName == "TravelCodeWikiPublish");

        if (project == null) return null;

        if (!project.RoleAssignments.TryGetValue(roleName, out var beings) || beings.Count == 0)
            return null;

        // 简单轮询：使用任务数量作为计数器（每次选任务最少的执行者）
        Guid? selected = null;
        int minTaskCount = int.MaxValue;

        foreach (var beingId in beings)
        {
            int taskCount = taskSystem.GetAll().Count(t =>
                t.ExecutorGuid == beingId &&
                t.Status != CollectiveTaskStatus.Completed &&
                t.Status != CollectiveTaskStatus.Failed &&
                t.Status != CollectiveTaskStatus.Cancelled);

            if (taskCount < minTaskCount)
            {
                minTaskCount = taskCount;
                selected = beingId;
            }
        }

        return selected;
    }

    private string GetChineseLanguage(string code)
    {
        if (code == "*")
        {
            return "英语";
        }
        return TravelCodeWikiWithAIPlugin._cldrProvider?.GetLanguageDisplayName(code, "zh-CN") ?? code;
    }
}
