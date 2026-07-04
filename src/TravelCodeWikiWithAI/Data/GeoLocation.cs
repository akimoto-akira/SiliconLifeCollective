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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\GeoLocation.cs
// 迁移变更：去除 OllamaClient 依赖（DetermineAdminLevelWithAI/GenerateIDWithOllama 已移除）
//           去除 OSMapi 依赖（FlushOSMData 改为空壳，待后续通过 ITool 实现）
//           去除 Http/FlushWikiData（待后续通过 ITool 实现）
//           去除 MediaWikiTool 依赖（BuildDocument/GetCurrentLocation 等 MediaWiki 方法待重写）
//           去除 CoreTools 依赖（GetID 中对 CoreSetting/Ollama 的调用已移除）
//           去除 OutPut 依赖（旧 CLI 日志系统）
//           去除 XMLBase/ICLIOutput 依赖
//           保留地理层级结构、AdminLevel 枚举、IGeoList/GeoList
//           保留 LanguageData.CreateWithTags OSM 标签解析

using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml;
using SiliconLife.Collective;
using SiliconLife.Speedy;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 行政区域级别枚举 / Administrative level enumeration
/// </summary>
public enum AdminLevel
{
    National = 2,
    SpecialAdministrativeRegion = 3,
    Provincial = 4,
    Municipality = 41,
    AutonomousRegion = 42,
    State = 43,
    Prefecture = 5,
    County = 6,
    SpecialZone = 7,
    Township = 8,
    TownshipLike = 9,
    Village = 10
}

/// <summary>
/// 地理位置基类 / Geographic location base class
/// 迁移自旧项目 GeoLocation，去除 Ollama/OSMapi/MediaWikiTool/Http 依赖
/// </summary>
public abstract class GeoLocation : GeoDataBase
{
    /// <summary>
    /// 构造函数 / Constructor
    /// </summary>
    public GeoLocation(GeoLocation? parent) : base(null)
    {
        Parent = parent;
        _parent = parent;
    }

    /// <summary>
    /// 无参构造函数 / Parameterless constructor
    /// </summary>
    public GeoLocation() : base(null) { }

    /// <summary>
    /// 父地理位置对象 / Parent geographic location object
    /// </summary>
    protected GeoLocation? Parent;

    /// <summary>
    /// 地理位置标识符 / Geographic location identifier
    /// </summary>
    public abstract string ID { get; set; }

    /// <summary>
    /// 完整标识符 / Full identifier
    /// </summary>
    public virtual string FullID
    {
        get
        {
            if (Parent == null)
            {
                if (BasePath == "world")
                {
                    return "world";
                }
                else
                {
                    return ID;
                }
            }
            else
            {
                return BasePath + "/" + ID;
            }
        }
    }

    /// <summary>
    /// OSM 类型 / OSM type
    /// </summary>
    public Data.OSM.OSMRelationRefType OSCType { get; set; }

    /// <summary>
    /// OSM ID
    /// </summary>
    public long OSMID { get; set; }

    /// <summary>
    /// 维基数据 ID / Wikidata identifier
    /// </summary>
    public string wikidata { get; set; } = string.Empty;

    /// <summary>
    /// 地区分类代码 / Area type code
    /// </summary>
    public string AreaType { get; set; } = string.Empty;

    /// <summary>
    /// 子地区代码 / Sub-area types
    /// </summary>
    public Dictionary<string, string>? SubAreaTypes { get; set; } = null;

    /// <summary>
    /// 基本了解内容 / Basic understanding content
    /// </summary>
    public WordBase? Understand { get; set; } = null;

    /// <summary>
    /// 名称 / Name
    /// </summary>
    public LanguageData? Name { get; set; } = null;

    /// <summary>
    /// 完整名称 / Full name
    /// </summary>
    public WordBase? FullName { get; set; } = null;

    /// <summary>
    /// 地图信息 / Map info
    /// </summary>
    public Data.OSM.MapInfo? MapInfo { get; set; }

    /// <summary>
    /// 下次检查时间 / Next check time
    /// </summary>
    public DateTime CheckTime { get; set; }

    /// <summary>
    /// 是否已发布到 MediaWiki / Whether published to MediaWiki
    /// </summary>
    public bool Published { get; set; } = false;

    /// <summary>
    /// 发布时间 / Publish timestamp
    /// </summary>
    public DateTime? PublishedAt { get; set; } = null;

    /// <summary>
    /// 设置发布状态 / Set published status
    /// </summary>
    public void SetPublished(bool published)
    {
        Published = published;
        if (published) PublishedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// 获取维基文档列表 / Get wiki documents list
    /// 默认实现：创建一个以 FullID 为标题的 MediaWikiDocument，
    /// 包含基本信息表格和所有 WordBase 内容段。
    /// 子类可 override 以添加自定义内容（如子区域列表）。
    /// </summary>
    public virtual DocumentBase[] GetWikiDocuments(Dictionary<string, byte[]> file)
    {
        List<DocumentBase> docs = new List<DocumentBase>();

        MediaWikiDocument doc = new MediaWikiDocument(this);
        doc.Title = FullID;
        doc.Contents = new MediaWikiChildWord(doc);

        // 基本信息：名称 + 地理坐标
        MediaWikiChildWord nameAndGeo = new MediaWikiChildWord(doc);
        if (Name != null)
        {
            MediaWikiLanguage nameWord = new MediaWikiLanguage(nameAndGeo) { LanguageData = Name };
            nameAndGeo.Add(nameWord);
        }
        nameAndGeo.Add(new MediaWikiNoLanguage(nameAndGeo) { Content = "\n\n" });
        if (MapInfo != null)
        {
            MediaWikiGeo geoWord = new MediaWikiGeo(nameAndGeo)
            {
                Longitude = MapInfo.Value.Center.X,
                Latitude = MapInfo.Value.Center.Y,
                Zoom = MapInfo.Value.Zoom,
                LocationName = Name ?? new LanguageData(),
                Page = FullID
            };
            nameAndGeo.Add(geoWord);
        }
        doc.Contents.Add(nameAndGeo);

        // 基本信息：属性表格
        MediaWikiTable infoTable = CreateBaseInfoTable(doc);
        if (infoTable != null)
        {
            doc.Contents.Add(infoTable);
        }

        // 内容段：收集所有 [Description] 标注的 WordBase 属性
        AddWordBaseSections(doc, doc.Contents);

        docs.Add(doc);
        return docs.ToArray();
    }

    /// <summary>
    /// 创建基本信息表格 / Create base information table
    /// 子类可 override 以添加更多行
    /// </summary>
    protected virtual MediaWikiTable? CreateBaseInfoTable(MediaWikiDocument doc)
    {
        MediaWikiTable table = new MediaWikiTable(doc)
        {
            TableClass = "wikitable",
            TableStyle = "float:right; width:300px; margin:0 0 1em 1em; clear:right;"
        };

        // 表头行：属性 | 值
        MediaWikiTableRow headerRow = table.AddHeaderRow();
        headerRow.AddCell(new MediaWikiNoLanguage(headerRow) { Content = "Property" });
        headerRow.AddCell(new MediaWikiNoLanguage(headerRow) { Content = "Value" });

        // 名称行
        AddInfoTableRow(table, "Name", Name != null
            ? new MediaWikiLanguage(table) { LanguageData = Name }
            : null);

        // OSM 类型
        AddInfoTableRow(table, "OSM Type", new MediaWikiNoLanguage(table) { Content = OSCType.ToString() });

        // OSM ID
        AddInfoTableRow(table, "OSM ID", new MediaWikiNoLanguage(table) { Content = OSMID.ToString() });

        // Wikidata
        if (!string.IsNullOrEmpty(wikidata))
        {
            MediaWikiExternalLink wdLink = new MediaWikiExternalLink(table)
            {
                URL = new MediaWikiNoLanguage(table) { Content = "https://www.wikidata.org/wiki/" + wikidata },
                Display = new MediaWikiNoLanguage(table) { Content = wikidata }
            };
            AddInfoTableRow(table, "Wikidata", wdLink);
        }

        return table;
    }

    /// <summary>
    /// 向信息表格添加一行 / Add a row to the info table
    /// </summary>
    protected void AddInfoTableRow(MediaWikiTable table, string label, MediaWikiWord? valueWord)
    {
        MediaWikiTableRow row = table.AddDataRow();
        row.AddCell(new MediaWikiNoLanguage(row) { Content = label });
        if (valueWord != null)
        {
            row.AddCell(valueWord);
        }
        else
        {
            row.AddCell(new MediaWikiNoLanguage(row) { Content = "" });
        }
    }

    /// <summary>
    /// 向信息表格添加一行（WordBase 重载）/ Add a row to the info table (WordBase overload)
    /// WordBase 内容会被包装为 MediaWikiChildWord 以适配表格单元格
    /// </summary>
    protected void AddInfoTableRow(MediaWikiTable table, string label, WordBase? valueWord)
    {
        if (valueWord == null)
        {
            AddInfoTableRow(table, label, (MediaWikiWord?)null);
            return;
        }
        // 将 WordBase 包装进 MediaWikiChildWord 以适配 MediaWikiWord 参数
        MediaWikiChildWord wrapper = new MediaWikiChildWord(table);
        wrapper.Add(valueWord);
        AddInfoTableRow(table, label, wrapper);
    }

    /// <summary>
    /// 收集所有 [Description] 标注的 WordBase 属性，并为每个非空属性创建 MediaWikiSection
    /// / Collect all [Description]-annotated WordBase properties and create a MediaWikiSection for each non-null one
    /// </summary>
    protected void AddWordBaseSections(MediaWikiDocument doc, WordBaseWithChild contents)
    {
        PropertyInfo[] props = GetType().GetProperties();
        foreach (PropertyInfo prop in props)
        {
            if (!prop.CanRead || !prop.CanWrite) continue;
            if (prop.PropertyType != typeof(WordBase) && !prop.PropertyType.IsSubclassOf(typeof(WordBase))) continue;

            DescriptionAttribute? desc = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;
            if (desc == null) continue;

            WordBase? value = prop.GetValue(this) as WordBase;
            if (value == null) continue;

            // 创建段标题（多语言支持）
            LanguageData sectionTitle = new LanguageData();
            sectionTitle.SetZhHans(desc.Description);

            MediaWikiSection section = new MediaWikiSection(doc)
            {
                Title = new MediaWikiLanguage(doc) { LanguageData = sectionTitle },
                Content = new MediaWikiChildWord(doc)
            };
            // 将 WordBase 添加到 Section 内容中（MediaWikiChildWord 继承 WordBaseWithChild，Add 接受 WordBase）
            section.Content.Add(value);
            contents.Add(section);
        }
    }

    /// <summary>
    /// 为 GeoLocation 创建 MediaWiki 链接词 / Create a MediaWiki link word for a GeoLocation
    /// 用于在列表中显示子区域链接
    /// </summary>
    protected MediaWikiLink CreateGeoLink(GeoDataBase parent, GeoLocation geo)
    {
        MediaWikiLink link = new MediaWikiLink(parent);
        link.DocTitle = new MediaWikiNoLanguage(link) { Content = geo.FullID };
        link.Display = geo.Name != null
            ? new MediaWikiLanguage(link) { LanguageData = geo.Name }
            : new MediaWikiNoLanguage(link) { Content = geo.ID };
        return link;
    }

    /// <summary>
    /// 获取子区域列表 / Get sub-area list
    /// </summary>
    public abstract IGeoList GetSubArea();

    /// <summary>
    /// 获取景点列表 / Get attractions list
    /// </summary>
    public abstract IGeoList GetAttractions();

    public override string BasePath
    {
        get
        {
            if (Parent == null)
            {
                return null;
            }
            return Parent.FullID;
        }
    }

    public override object? GetObject(string path)
    {
        if (FullID == path)
        {
            return this;
        }
        else
        {
            PropertyInfo[] a = GetType().GetProperties();
            foreach (PropertyInfo b in a)
            {
                if (b.CanRead && b.CanWrite)
                {
                    if (!b.PropertyType.IsValueType)
                    {
                        if (b.PropertyType.IsSubclassOf(typeof(GeoDataBase)))
                        {
                            GeoDataBase? c = b.GetValue(this) as GeoDataBase;
                            if (c != this && c != null)
                            {
                                object? d = c.GetObject(path);
                                if (d != null)
                                {
                                    return d;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }
    }

    public override GeoDataBase? GetParent()
    {
        return Parent;
    }

    public override string ToString()
    {
        if (Name != null)
        {
            return Name + "|" + GetType().Name;
        }

        if (!string.IsNullOrEmpty(FullID))
        {
            return FullID + "|" + GetType().Name;
        }

        return GetType().Name;
    }

    public virtual bool DebugNeedChild()
    {
        return true;
    }

    /// <summary>
    /// 从 OSM 标签获取 ID / Get ID from OSM tags
    /// </summary>
    public string GetID(Dictionary<string, string> tags)
    {
        if (tags.ContainsKey("ISO3166-1"))
        {
            return tags["ISO3166-1"];
        }

        if (tags.ContainsKey("ISO3166-2"))
        {
            string a = tags["ISO3166-2"];
            string[] b = a.Split('-');
            if (b.Length == 2)
            {
                return b[1];
            }
        }

        // 旧项目此处调用 Ollama 生成 ID，新项目使用备用方案
        // Legacy project called Ollama here; new project uses fallback
        return GenerateFallbackID(Name?.ToString() ?? "UNKNW", new List<string>());
    }

    /// <summary>
    /// 生成备用 ID / Generate fallback ID
    /// </summary>
    private string GenerateFallbackID(string placeName, List<string> existingIds)
    {
        string baseId = "";
        string cleanName = new string(placeName.Where(char.IsLetter).ToArray()).ToUpper();

        if (cleanName.Length >= 5)
        {
            baseId = cleanName.Substring(0, 5);
        }
        else if (cleanName.Length > 0)
        {
            baseId = cleanName.PadRight(5, 'X');
        }
        else
        {
            baseId = "UNKNW";
        }

        string finalId = baseId;
        int counter = 1;
        while (existingIds.Contains(finalId))
        {
            char lastChar = (char)('A' + (counter % 26));
            finalId = baseId.Substring(0, 4) + lastChar;
            counter++;

            if (counter > 26)
            {
                finalId = "UNKNW";
                break;
            }
        }

        return finalId;
    }

    /// <summary>
    /// 获取 AI 路径描述（用于工具调用）/ Get AI path description (for tool calls)
    /// </summary>
    public virtual string GetAIPath()
    {
        if (_parent is GeoLocation parentLoc)
        {
            string a = parentLoc.GetAIPath();
            return a + " " + (Name?.ToString() ?? ID);
        }
        return Name?.ToString() ?? ID;
    }

    public override Dictionary<string, string> BuildDocument(Dictionary<string, byte[]> files)
    {
        DocumentBase[] a = GetWikiDocuments(files);
        Dictionary<string, string> c = new Dictionary<string, string>();
        foreach (DocumentBase d in a)
        {
            Dictionary<string, string> b = d.BuildDocument(files);
            foreach (KeyValuePair<string, string> e in b)
            {
                if (c.ContainsKey(e.Key))
                {
                    throw new NotImplementedException();
                }
                else
                {
                    c.Add(e.Key, e.Value);
                }
            }
        }

        return c;
    }

    // ========== 以下方法待后续通过 ITool 实现 ==========
    // The following methods are stubs, to be implemented via ITool in future phases

    /// <summary>
    /// 刷新 OSM 数据（空壳，待 ITool 实现）/ Flush OSM data (stub, to be implemented via ITool)
    /// </summary>
    public virtual void FlushOSMData()
    {
        // 旧项目通过 OSMapi 获取数据，新项目待通过 ITool 实现
        // Legacy project fetched data via OSMapi; new project will implement via ITool
    }

    /// <summary>
    /// 刷新 OSM 子数据 / Flush OSM sub data
    /// </summary>
    public virtual void FlushOsmSub(Data.OSM.OSMBaseData data)
    {
        // 待 ITool 实现 / To be implemented via ITool
    }

    /// <summary>
    /// 刷新维基数据（空壳，待 ITool 实现）/ Flush wiki data (stub, to be implemented via ITool)
    /// </summary>
    public virtual void FlushWikiData()
    {
        // 旧项目通过 Http.Get 获取 wikidata，新项目待通过 ITool 实现
        // Legacy project fetched wikidata via Http.Get; new project will implement via ITool
    }

    /// <summary>
    /// 填充信息（空壳，待 ITool 实现）/ Fill info (stub, to be implemented via ITool)
    /// </summary>
    public virtual void FillInfo()
    {
        // 旧项目通过 Ollama 生成旅游内容，新项目待通过 ITool 让硅基人填写
        // Legacy project generated travel content via Ollama; new project will have silicon beings fill via ITool
    }
}

/// <summary>
/// 地理位置列表类 / Geographic location list class
/// </summary>
public class GeoList<T> : GeoLocation, IList, IGeoList where T : GeoLocation
{
    /// <summary>
    /// 地理位置标识符 / Geographic location identifier
    /// </summary>
    public override string ID { get; set; } = string.Empty;

    public GeoList(GeoLocation? Parent) : base(Parent)
    {
    }

    public GeoList() : base(null) { }

    private List<T> _data = new List<T>();

    public IEnumerator<T> GetEnumerator() => _data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    IEnumerator<GeoLocation> IEnumerable<GeoLocation>.GetEnumerator() => GetEnumerator();

    public void Add(T item) => _data.Add(item);
    public void Add(GeoLocation item) => _data.Add(item as T);
    public int Add(object? value) => throw new NotImplementedException();

    public bool Contains(GeoLocation item) => throw new NotImplementedException();
    public void CopyTo(GeoLocation[] array, int arrayIndex)
    {
        for (int a = 0; a < array.Length; a++)
        {
            if ((_data.Count - 1) <= a)
            {
                array[a] = _data[a + arrayIndex];
            }
        }
    }

    public bool Remove(GeoLocation item) => _data.Remove(item as T);
    public int IndexOf(GeoLocation item) => _data.IndexOf(item as T);
    public void Insert(int index, GeoLocation item) => _data.Insert(index, item as T);

    public T this[int index]
    {
        get => _data[index];
        set => throw new NotImplementedException();
    }

    GeoLocation IList<GeoLocation>.this[int index]
    {
        get => _data[index];
        set => throw new NotImplementedException();
    }

    object? IList.this[int index]
    {
        get => _data[index];
        set => throw new NotImplementedException();
    }

    public int Count => _data.Count;
    public bool IsReadOnly => false;
    public bool IsFixedSize => false;
    public bool IsSynchronized => false;
    public object SyncRoot { get; } = new object();

    public void Clear() => throw new NotImplementedException();
    public bool Contains(object? value) => throw new NotImplementedException();
    public int IndexOf(object? value) => throw new NotImplementedException();
    public void Insert(int index, object? value) => throw new NotImplementedException();
    public void Remove(object? value) => throw new NotImplementedException();
    public void RemoveAt(int index) => throw new NotImplementedException();
    public void CopyTo(Array array, int index) => throw new NotImplementedException();

    public override string[] GetPath(int deth)
    {
        List<string> b = new List<string>();
        deth = (deth == -1) ? -1 : deth - 1;
        for (int a = 0; a < Count; a++)
        {
            b.Add(BasePath + "[" + a + "]");
            if (deth > 0)
            {
                T c = _data[a];
                string[] d = c.GetPath(deth);
                b.AddRange(d);
            }
        }
        return b.ToArray();
    }

    public override string ToString() => "(" + Count + ")";

    public override IGeoList GetSubArea() => this;

    public override IGeoList GetAttractions() => throw new NotImplementedException();

    public new GeoLocation GetParent() => Parent!;
    public GeoLocation GetParentsParent() => Parent!;

    public GeoLocation[] GetGeoLocations() => _data.ToArray();

    public int IndexOfWithOSM(Data.OSM.OSMRelationRefType type, long id)
    {
        for (int i = 0; i < _data.Count; i++)
        {
            T a = _data[i];
            if (a.OSCType == type && a.OSMID == id)
            {
                return i;
            }
        }
        return -1;
    }

    public override void AutoSetLanguage()
    {
        try
        {
            foreach (T t in _data)
            {
                t.AutoSetLanguage();
            }
        }
        catch
        {
            // 新项目：翻译中断不再通过 OutPut 输出
            // In the new project: translation interruption no longer outputs via OutPut
        }
    }

    public override void FlushOSMData()
    {
        foreach (T t in _data)
        {
            t.FlushOSMData();
        }
    }

    public override Dictionary<string, string> BuildDocument(Dictionary<string, byte[]> files)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        foreach (GeoLocation gl in this)
        {
            Dictionary<string, string> a = gl.BuildDocument(files);
            foreach (KeyValuePair<string, string> b in a)
            {
                throw new NotImplementedException();
            }
        }
        return result;
    }

    public override string FullID => BasePath ?? "";

    public override object? GetObject(string path)
    {
        foreach (GeoLocation a in _data)
        {
            object? b = a.GetObject(path);
            if (b != null)
            {
                return b;
            }
        }
        return null;
    }

    public override void RemoveSame()
    {
        HashSet<T> a = new HashSet<T>(_data, new GeoLocationComparer());
        _data = new List<T>(a);
        foreach (T b in a)
        {
            b.RemoveSame();
        }
    }

    public override void CheckParent()
    {
        foreach (T a in _data)
        {
            a.CheckParent();
        }
    }

    public override void FillInfo()
    {
        foreach (T t in _data)
        {
            t.FillInfo();
        }
    }

    public override string GetAIPath() => (_parent as GeoLocation)?.GetAIPath() ?? "";

    /// <summary>
    /// 从 XML 节点加载列表数据 / Load list data from XML node
    /// 搬运自细需求 GeoList.LoadXMLWithNode，FilePath 改为 StorageRef
    /// 每个列表元素都是独立存储，列表只存引用
    /// </summary>
    public override bool LoadXMLWithNode(XmlNode node, SpeedyPack pack)
    {
        foreach (XmlNode a in node.ChildNodes)
        {
            switch (a.Name)
            {
                case "GeoList":
                    string? b = a.Attributes?["Type"]?.Value;
                    if (b == null) continue;
                    Type? c = FindType(b);
                    if (c == null) continue;

                    GeoLocation d = (GeoLocation)CreateChild(c, this);
                    XmlAttribute? refAttr = a.Attributes?["StorageRef"];
                    if (refAttr == null) continue;

                    string refKey = refAttr.Value;
                    byte[]? refData = pack.Read(refKey);
                    if (refData == null) continue;

                    XmlDocument refDoc = new XmlDocument();
                    refDoc.Load(new MemoryStream(refData));
                    XmlElement refRoot = refDoc.DocumentElement;
                    if (refRoot == null) continue;

                    if (!d.LoadXMLWithNode(refRoot, pack)) continue;
                    Add(d);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        return true;
    }

    /// <summary>
    /// 将列表数据保存到 XML 节点 / Save list data to XML node
    /// 搬运自细需求 GeoList.SaveXMLWithNode，FilePath 改为 StorageRef
    /// 每个列表元素独立存储到 SpeedyPack，列表只存引用
    /// </summary>
    public override bool SaveXMLWithNode(XmlNode node, XmlDocument document, SpeedyPack pack)
    {
        foreach (GeoLocation a in this)
        {
            XmlElement b = document.CreateElement("GeoList");
            b.SetAttribute("Type", a.GetType().FullName!);

            string? childKey = a.GetStorageKey();
            if (childKey != null)
            {
                a.SaveToPack(pack);
                b.SetAttribute("StorageRef", childKey);
            }
            else
            {
                if (!a.SaveXMLWithNode(b, document, pack))
                {
                    return false;
                }
            }

            node.AppendChild(b);
        }

        return true;
    }
}

/// <summary>
/// 地理位置列表接口 / Geographic location list interface
/// </summary>
public interface IGeoList : IList<GeoLocation>
{
    GeoLocation GetParent();
    GeoLocation GetParentsParent();
    GeoLocation[] GetGeoLocations();
    void FillInfo();
}

/// <summary>
/// 地理位置比较器 / Geo location comparer
/// </summary>
public class GeoLocationComparer : IEqualityComparer<GeoLocation>
{
    public bool Equals(GeoLocation? x, GeoLocation? y) => x?.FullID == y?.FullID;
    public int GetHashCode(GeoLocation obj) => obj.FullID.GetHashCode();
}
