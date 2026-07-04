using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;

namespace TravelCodeWikiWithAI.Data.OSM
{
    public struct MapInfo
    {
        /// <summary>
        /// 地图中心点经纬度 / Map center point longitude and latitude
        /// </summary>
        public Vector2DD Center { get; set; }

        /// <summary>
        /// 地图缩放级别 / Map zoom level
        /// </summary>
        public int Zoom { get; set; }

        /// <summary>
        /// 构造函数 / Constructor
        /// </summary>
        /// <param name="center">中心点经纬度 / Center point longitude and latitude</param>
        /// <param name="zoom">缩放级别 / Zoom level</param>
        public MapInfo(Vector2DD center, int zoom)
        {
            Center = center;
            Zoom = zoom;
        }

        /// <summary>
        /// 构造函数（使用经纬度坐标）/ Constructor (using longitude and latitude coordinates)
        /// </summary>
        /// <param name="longitude">经度 / Longitude</param>
        /// <param name="latitude">纬度 / Latitude</param>
        /// <param name="zoom">缩放级别 / Zoom level</param>
        public MapInfo(double longitude, double latitude, int zoom)
        {
            Center = new Vector2DD(longitude, latitude);
            Zoom = zoom;
        }

        /// <summary>
        /// 返回字符串表示 / Return string representation
        /// </summary>
        /// <returns>字符串表示 / String representation</returns>
        public override string ToString()
        {
            return $"Center=({Center}), Zoom={Zoom}";
        }

        /// <summary>
        /// 从字符串解析 MapInfo / Parse MapInfo from string
        /// </summary>
        /// <param name="str">字符串格式："Center=(X=value; Y=value), Zoom=value" / String format: "Center=(X=value; Y=value), Zoom=value"</param>
        /// <returns>解析后的 MapInfo / Parsed MapInfo</returns>
        /// <exception cref="ArgumentException">字符串格式不正确时抛出 / Thrown when string format is incorrect</exception>
        public static MapInfo Parse(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException("输入字符串不能为空 / Input string cannot be null or empty", nameof(str));
            }

            // 分割字符串 / Split string
            string[] parts = str.Split(new[] { ", Zoom=" }, StringSplitOptions.None);
            if (parts.Length != 2)
            {
                throw new ArgumentException(
                    "字符串格式不正确，应为 'Center=(X=value; Y=value), Zoom=value' / String format is incorrect", nameof(str));
            }

            // 解析 Center / Parse Center
            string centerPart = parts[0].Trim();
            if (!centerPart.StartsWith("Center=(") || !centerPart.EndsWith(")"))
            {
                throw new ArgumentException("Center 格式不正确 / Center format is incorrect", nameof(str));
            }

            string centerValue = centerPart.Substring(8, centerPart.Length - 9);
            Vector2DD center = new Vector2DD();
            center = (Vector2DD)center.Parse(centerValue);

            // 解析 Zoom / Parse Zoom
            if (!int.TryParse(parts[1].Trim(), out int zoom))
            {
                throw new ArgumentException("Zoom 值无法解析 / Zoom value cannot be parsed", nameof(str));
            }

            return new MapInfo(center, zoom);
        }
    }

    /// <summary>
    /// OSM基础数据抽象类 / OSM base data abstract class
    /// </summary>
    public abstract class OSMBaseData
    {
        /// <summary>
        /// 元素ID / Element ID
        /// </summary>
        public long Id { get; set; } = -1;

        /// <summary>
        /// 版本号 / Version number
        /// </summary>
        public int Version { get; set; } = -1;

        /// <summary>
        /// 时间戳 / Timestamp
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 获取标签字典 / Get tags dictionary
        /// </summary>
        /// <returns>标签字典 / Tags dictionary</returns>
        public abstract Dictionary<string, string> GetTags();

        /// <summary>
        /// 判断对象是否相等 / Check if objects are equal
        /// </summary>
        /// <param name="obj">比较对象 / Object to compare</param>
        /// <returns>是否相等 / Whether equal</returns>
        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// 获取哈希码 / Get hash code
        /// </summary>
        /// <returns>哈希码 / Hash code</returns>
        public override int GetHashCode()
        {
            return (int)Id;
        }

        public static OSMRelationRefType ToType(OSMBaseData bd)
        {
            if (bd is OSMRelations)
            {
                return OSMRelationRefType.Relations;
            }

            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// OSM节点 / OSM node
    /// </summary>
    public class OSMNode : OSMBaseData
    {
        /// <summary>
        /// 经纬度坐标 / Longitude and latitude coordinates
        /// </summary>
        public Vector2DD LngLat { get; set; }

        /// <summary>
        /// 标签字典 / Tags dictionary
        /// </summary>
        public Dictionary<string, string> Tags { get; set; }

        /// <summary>
        /// 变更集ID / Changeset ID
        /// </summary>
        public long ChangeSet { get; set; } = -1;

        /// <summary>
        /// 用户ID / User ID
        /// </summary>
        public int Uid { get; set; } = -1;

        /// <summary>
        /// 用户名 / Username
        /// </summary>
        public string User { get; set; } = "";

        /// <summary>
        /// 是否可见 / Whether visible
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// 获取标签字典 / Get tags dictionary
        /// </summary>
        /// <returns>标签字典副本 / Copy of tags dictionary</returns>
        public override Dictionary<string, string> GetTags()
        {
            return new Dictionary<string, string>(Tags);
        }

        /// <summary>
        /// 判断对象是否相等 / Check if objects are equal
        /// </summary>
        /// <param name="obj">比较对象 / Object to compare</param>
        /// <returns>是否相等 / Whether equal</returns>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is OSMNode other)
            {
                return Id == other.Id;
            }

            return false;
        }
    }

    /// <summary>
    /// OSM路径 / OSM way
    /// </summary>
    public class OSMWay : OSMBaseData
    {
        /// <summary>
        /// 节点引用数组 / Node reference array
        /// </summary>
        public long[] Refs { get; set; } = Array.Empty<long>();

        /// <summary>
        /// 标签字典 / Tags dictionary
        /// </summary>
        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 变更集ID / Changeset ID
        /// </summary>
        public long ChangeSet { get; set; } = -1;

        /// <summary>
        /// 用户ID / User ID
        /// </summary>
        public int Uid { get; set; } = -1;

        /// <summary>
        /// 用户名 / Username
        /// </summary>
        public string User { get; set; } = "";

        /// <summary>
        /// 是否可见 / Whether visible
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// 获取标签字典 / Get tags dictionary
        /// </summary>
        /// <returns>标签字典副本 / Copy of tags dictionary</returns>
        public override Dictionary<string, string> GetTags()
        {
            return new Dictionary<string, string>(Tags);
        }
    }

    /// <summary>
    /// OSM关系 / OSM relation
    /// </summary>
    public class OSMRelations : OSMBaseData
    {
        /// <summary>
        /// 关系引用列表 / Relation reference list
        /// </summary>
        public List<OSMRelationRef> Refs { get; set; } = new List<OSMRelationRef>();

        /// <summary>
        /// 标签字典 / Tags dictionary
        /// </summary>
        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 变更集ID / Changeset ID
        /// </summary>
        public long ChangeSet { get; set; } = -1;

        /// <summary>
        /// 用户ID / User ID
        /// </summary>
        public int Uid { get; set; } = -1;

        /// <summary>
        /// 用户名 / Username
        /// </summary>
        public string User { get; set; } = "";

        /// <summary>
        /// 是否可见 / Whether visible
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// 获取标签字典 / Get tags dictionary
        /// </summary>
        /// <returns>标签字典副本 / Copy of tags dictionary</returns>
        public override Dictionary<string, string> GetTags()
        {
            return new Dictionary<string, string>(Tags);
        }

        /// <summary>
        /// 判断对象是否相等 / Check if objects are equal
        /// </summary>
        /// <param name="obj">比较对象 / Object to compare</param>
        /// <returns>是否相等 / Whether equal</returns>
        public override bool Equals(object? obj)
        {
            if (obj is OSMRelations other)
            {
                return Id == other.Id;
            }

            return false;
        }
    }

    /// <summary>
    /// OSM关系引用类型枚举 / OSM relation reference type enumeration
    /// </summary>
    public enum OSMRelationRefType
    {
        /// <summary>
        /// 无类型 / None
        /// </summary>
        None,

        /// <summary>
        /// 节点 / Node
        /// </summary>
        Node,

        /// <summary>
        /// 路径 / Way
        /// </summary>
        Way,

        /// <summary>
        /// 关系 / Relations
        /// </summary>
        Relations
    }

    /// <summary>
    /// OSM关系引用结构 / OSM relation reference structure
    /// </summary>
    public struct OSMRelationRef
    {
        /// <summary>
        /// 引用ID / Reference ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 引用类型 / Reference type
        /// </summary>
        public OSMRelationRefType Type { get; set; }

        /// <summary>
        /// 角色 / Role
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 构造函数 / Constructor
        /// </summary>
        /// <param name="id">引用ID / Reference ID</param>
        /// <param name="type">引用类型 / Reference type</param>
        /// <param name="role">角色 / Role</param>
        public OSMRelationRef(long id, OSMRelationRefType type, string role)
        {
            Id = id;
            Type = type;
            Role = role;
        }
    }

    /// <summary>
    /// OSM数据工具类 / OSM data tools class
    /// </summary>
    public static class OSMDataTools
    {
        /// <summary>
        /// 将Unix时间戳转换为DateTime / Convert Unix timestamp to DateTime
        /// </summary>
        /// <param name="unix">Unix时间戳 / Unix timestamp</param>
        /// <returns>DateTime对象 / DateTime object</returns>
        public static DateTime ConvToTime(long unix)
        {
            DateTime startTime = new DateTime(1970, 1, 1);
            DateTime result = startTime.AddSeconds(unix);
            return result;
        }

        /// <summary>
        /// 将DateTime转换为字符串 / Convert DateTime to string
        /// </summary>
        /// <param name="dt">DateTime对象 / DateTime object</param>
        /// <returns>时间字符串 / Time string</returns>
        public static string DateToString(DateTime dt)
        {
            string result = dt.ToString("yyyy-MM-ddThh:mm:ssZ");
            return result;
        }

        /// <summary>
        /// 将DateTime转换为Unix时间戳 / Convert DateTime to Unix timestamp
        /// </summary>
        /// <param name="dateTime">DateTime对象 / DateTime object</param>
        /// <returns>Unix时间戳 / Unix timestamp</returns>
        public static long ConvToLong(DateTime dateTime)
        {
            DateTime startTime = new DateTime(1970, 1, 1);
            TimeSpan ts = dateTime - startTime;
            return (long)ts.TotalSeconds;
        }
    }

    public class OsmList<T> : IList<T>, IList where T : OSMBaseData
    {
        public IEnumerator<T> GetEnumerator()
        {
            return _datas.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _datas.Add(item);
        }

        public int Add(object? value)
        {
            throw new NotImplementedException();
        }

        void IList.Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object? value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object? value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object? value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object? value)
        {
            throw new NotImplementedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize => false;

        bool IList.IsReadOnly => false;

        object? IList.this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        void ICollection<T>.Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            T[] source = _datas.ToArray();
            for (int i = 0; i < array.Length - arrayIndex; i++)
            {
                array[arrayIndex + i] = source[i];
            }
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        int ICollection.Count => _datas.Count;

        public bool IsSynchronized { get; }
        public object SyncRoot { get; }

        int ICollection<T>.Count => _datas.Count;

        bool ICollection<T>.IsReadOnly => false;

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public T this[int index]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public static OsmList<T> LoadJsonText(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
            return JsonSerializer.Deserialize<OsmList<T>>(json, options) ?? new OsmList<T>();
        }

        public static OsmList<T> LoadFromXmlDocument(XmlDocument doc)
        {
            XmlNode osm = doc.DocumentElement;
            OsmList<T> result = new OsmList<T>();
            foreach (XmlNode node in osm)
            {
                T item = LoadElement(node);
                result.Add(item);
            }

            return result;
        }

        private static T LoadElement(XmlNode node)
        {
            if (typeof(T) == typeof(OSMNode))
            {
                OSMNode a = new OSMNode();
                foreach (XmlAttribute b in node.Attributes)
                {
                    switch (b.Name)
                    {
                        case "id":
                            a.Id = long.Parse(b.Value);
                            break;
                        case "visible":
                            a.Visible = bool.Parse(b.Value);
                            break;
                        case "version":
                            a.Version = int.Parse(b.Value);
                            break;
                        case "changeset":
                            a.ChangeSet = long.Parse(b.Value);
                            break;
                        case "timestamp":
                            a.Time = ConvTime(b.Value);
                            break;
                        case "user":
                            a.User = b.Value;
                            break;
                        case "uid":
                            a.Uid = int.Parse(b.Value);
                            break;
                        case "lat":
                            Vector2DD c = a.LngLat;
                            c.Y = double.Parse(b.Value);
                            a.LngLat = c;
                            break;
                        case "lon":
                            Vector2DD d = a.LngLat;
                            d.X = double.Parse(b.Value);
                            a.LngLat = d;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }

                a.Tags = new Dictionary<string, string>();
                foreach (XmlNode e in node.ChildNodes)
                {
                    string f = e.Attributes["k"].Value;
                    string g = e.Attributes["v"].Value;
                    a.Tags.Add(f, g);
                }

                return a as T;
            }
            else if (typeof(T) == typeof(OSMRelations))
            {
                OSMRelations h = new OSMRelations();
                foreach (XmlAttribute j in node.Attributes)
                {
                    switch (j.Name)
                    {
                        case "id":
                            h.Id = long.Parse(j.Value);
                            break;
                        case "visible":
                            h.Visible = bool.Parse(j.Value);
                            break;
                        case "version":
                            h.Version = int.Parse(j.Value);
                            break;
                        case "changeset":
                            h.ChangeSet = long.Parse(j.Value);
                            break;
                        case "timestamp":
                            h.Time = ConvTime(j.Value);
                            break;
                        case "user":
                            h.User = j.Value;
                            break;
                        case "uid":
                            h.Uid = int.Parse(j.Value);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }

                h.Tags = new Dictionary<string, string>();
                OSMRelations l = h as OSMRelations;
                foreach (XmlNode i in node.ChildNodes)
                {
                    switch (i.Name)
                    {
                        case "member":
                            OSMRelationRef k = new OSMRelationRef();
                            foreach (XmlAttribute j in i.Attributes)
                            {
                                switch (j.Name)
                                {
                                    case "type":
                                        switch (j.Value)
                                        {
                                            case "way":
                                                k.Type = OSMRelationRefType.Way;
                                                break;
                                            case "node":
                                                k.Type = OSMRelationRefType.Node;
                                                break;
                                            case "relation":
                                                k.Type = OSMRelationRefType.Relations;
                                                break;
                                            default:
                                                throw new NotImplementedException();
                                        }

                                        break;
                                    case "ref":
                                        k.Id = long.Parse(j.Value);
                                        break;
                                    case "role":
                                        k.Role = j.Value;
                                        break;
                                    default:
                                        throw new NotImplementedException();
                                }
                            }

                            l.Refs.Add(k);
                            break;
                        case "tag":
                            string m = i.Attributes["k"].Value;
                            string n = i.Attributes["v"].Value;
                            h.Tags.Add(m, n);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }

                return h as T;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static DateTime ConvTime(string time)
        {
            return DateTime.Parse(time);
        }

        private List<T> _datas = new List<T>();

        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };
            return JsonSerializer.Serialize(this, options);
        }

        public T GetWithVersion(int version)
        {
            return _datas.FirstOrDefault(x => x.Version == version);
        }

        /// <summary>
        /// 创建 XML 元素节点 / Create XML element node
        /// </summary>
        /// <param name="item">OSM 数据项 / OSM data item</param>
        /// <param name="doc">XML 文档 / XML document</param>
        /// <returns>XML 元素 / XML element</returns>
        private static XmlElement CreateElementNode(T item, XmlDocument doc)
        {
            if (item is OSMNode node)
            {
                // 创建节点元素 / Create node element
                XmlElement element = doc.CreateElement("node");
                
                // 设置属性 / Set attributes
                element.SetAttribute("id", node.Id.ToString());
                element.SetAttribute("visible", node.Visible.ToString().ToLower());
                element.SetAttribute("version", node.Version.ToString());
                
                if (node.ChangeSet != -1)
                {
                    element.SetAttribute("changeset", node.ChangeSet.ToString());
                }
                
                element.SetAttribute("timestamp", OSMDataTools.DateToString(node.Time));
                
                if (!string.IsNullOrEmpty(node.User))
                {
                    element.SetAttribute("user", node.User);
                }
                
                if (node.Uid != -1)
                {
                    element.SetAttribute("uid", node.Uid.ToString());
                }
                
                element.SetAttribute("lat", node.LngLat.Y.ToString(System.Globalization.CultureInfo.InvariantCulture));
                element.SetAttribute("lon", node.LngLat.X.ToString(System.Globalization.CultureInfo.InvariantCulture));

                // 添加标签 / Add tags
                foreach (var tag in node.Tags)
                {
                    XmlElement tagElement = doc.CreateElement("tag");
                    tagElement.SetAttribute("k", tag.Key);
                    tagElement.SetAttribute("v", tag.Value);
                    element.AppendChild(tagElement);
                }

                return element;
            }
            else if (item is OSMRelations relation)
            {
                // 创建关系元素 / Create relation element
                XmlElement element = doc.CreateElement("relation");
                
                // 设置属性 / Set attributes
                element.SetAttribute("id", relation.Id.ToString());
                element.SetAttribute("visible", relation.Visible.ToString().ToLower());
                element.SetAttribute("version", relation.Version.ToString());
                
                if (relation.ChangeSet != -1)
                {
                    element.SetAttribute("changeset", relation.ChangeSet.ToString());
                }
                
                element.SetAttribute("timestamp", OSMDataTools.DateToString(relation.Time));
                
                if (!string.IsNullOrEmpty(relation.User))
                {
                    element.SetAttribute("user", relation.User);
                }
                
                if (relation.Uid != -1)
                {
                    element.SetAttribute("uid", relation.Uid.ToString());
                }

                // 添加成员 / Add members
                foreach (var refItem in relation.Refs)
                {
                    XmlElement memberElement = doc.CreateElement("member");
                    
                    // 设置成员类型 / Set member type
                    string typeValue = refItem.Type switch
                    {
                        OSMRelationRefType.Node => "node",
                        OSMRelationRefType.Way => "way",
                        OSMRelationRefType.Relations => "relation",
                        _ => throw new NotImplementedException($"未支持的关系类型: {refItem.Type}")
                    };
                    
                    memberElement.SetAttribute("type", typeValue);
                    memberElement.SetAttribute("ref", refItem.Id.ToString());
                    memberElement.SetAttribute("role", refItem.Role ?? "");
                    element.AppendChild(memberElement);
                }

                // 添加标签 / Add tags
                foreach (var tag in relation.Tags)
                {
                    XmlElement tagElement = doc.CreateElement("tag");
                    tagElement.SetAttribute("k", tag.Key);
                    tagElement.SetAttribute("v", tag.Value);
                    element.AppendChild(tagElement);
                }

                return element;
            }
            else if (item is OSMWay way)
            {
                // 创建路径元素 / Create way element
                XmlElement element = doc.CreateElement("way");
                
                // 设置属性 / Set attributes
                element.SetAttribute("id", way.Id.ToString());
                element.SetAttribute("visible", way.Visible.ToString().ToLower());
                element.SetAttribute("version", way.Version.ToString());
                
                if (way.ChangeSet != -1)
                {
                    element.SetAttribute("changeset", way.ChangeSet.ToString());
                }
                
                element.SetAttribute("timestamp", OSMDataTools.DateToString(way.Time));
                
                if (!string.IsNullOrEmpty(way.User))
                {
                    element.SetAttribute("user", way.User);
                }
                
                if (way.Uid != -1)
                {
                    element.SetAttribute("uid", way.Uid.ToString());
                }

                // 添加节点引用 / Add node references
                foreach (long refId in way.Refs)
                {
                    XmlElement ndElement = doc.CreateElement("nd");
                    ndElement.SetAttribute("ref", refId.ToString());
                    element.AppendChild(ndElement);
                }

                // 添加标签 / Add tags
                foreach (var tag in way.Tags)
                {
                    XmlElement tagElement = doc.CreateElement("tag");
                    tagElement.SetAttribute("k", tag.Key);
                    tagElement.SetAttribute("v", tag.Value);
                    element.AppendChild(tagElement);
                }

                return element;
            }
            else
            {
                throw new NotImplementedException($"不支持的类型: {typeof(T).Name}");
            }
        }

        public OsmList()
        {
            
        }

        public OsmList(T[] array)
        {
            _datas.AddRange(array);
        }
    }

    public class OSMData
    {
        public List<OSMNode> Nodes = new List<OSMNode>();

        public OSMData()
        {
        }

        public List<OSMWay> Ways = new List<OSMWay>();

        public List<OSMRelations> Relations = new List<OSMRelations>();

        public DateTime MetaTime = new DateTime(1970, 1, 1);

        public bool SortById = false;

        public bool SortGeographic = false;

        public DateTime SaveTime = new DateTime(1970, 1, 1);

        public bool HasMetadata = false;

        public string BaseURL = "";

        public long SequenceNumber = -1;

        public string source = "";

        public bool HistoricalInformation = false;

        public Version version = new Version(0, 6);

        public BoxD Bbox = new BoxD();
    }

    public interface IBox<T>
    {
        public IVector2D<T> LU { get; set; }

        public IVector2D<T> RD { get; set; }

        public IVector2D<T> Center { get; }

        public bool InBox(IVector2D<T> point);

        public string ToString()
        {
            throw new NotImplementedException();
        }
    }

    public interface IBox3D<T>
    {
        public IVector<T> LU { get; set; }

        public IVector<T> RD { get; set; }

        public IVector<T> Center { get; }
    }

    public struct BoxD : IBox<double>
    {
        public IVector2D<double> LU
        {
            get => lu;
            set { lu = value; }
        }

        public IVector2D<double> RD
        {
            get => rd;
            set { rd = value; }
        }

        public IVector2D<double> Center
        {
            get { throw new NotImplementedException(); }
        }

        private IVector2D<double> lu, rd;

        public bool InBox(IVector2D<double> point)
        {
            if (point.X < LU.X)
            {
                return false;
            }

            if (point.Y > LU.Y)
            {
                return false;
            }

            if (point.X > RD.X)
            {
                return false;
            }

            if (point.Y < RD.Y)
            {
                return false;
            }

            throw new NotImplementedException();
        }

        public BoxD(IEnumerable<IVector2D<double>> points) : this()
        {
            foreach (IVector2D<double> a in points)
            {
                if (LU == null)
                {
                    LU = new Vector2DD
                    {
                        X = a.X,
                        Y = a.Y
                    };
                    RD = new Vector2DD
                    {
                        X = a.X,
                        Y = a.Y
                    };
                }
                else
                {
                    if (a.X < LU.X)
                    {
                        LU.X = a.X;
                    }

                    if (a.Y < LU.Y)
                    {
                        LU.Y = a.Y;
                    }

                    if (a.X > RD.X)
                    {
                        RD.X = a.X;
                    }

                    if (a.Y > RD.Y)
                    {
                        RD.Y = a.Y;
                    }
                }
            }
        }

        public static BoxD operator +(BoxD a, BoxD b)
        {
            return new BoxD(new IVector2D<double>[] { a.LU, a.RD, b.LU, b.RD });
        }

        public BoxD()
        {
            lu = new Vector2DD();
            rd = new Vector2DD();
        }
    }

    public interface IVector2D<T>
    {
        public T X { get; set; }

        public T Y { get; set; }

        public IVector2D<T> Parse(string s);
    }

    public interface IVector<T> : IVector2D<T>
    {
        public T Z { get; set; }
    }

    public interface IVector4D<T> : IVector<T>
    {
        public T W { get; set; }
    }

    public struct Vector2D : IVector2D<float>
    {
        public float X { get; set; }
        public float Y { get; set; }
        public IVector2D<float> Parse(string s)
        {
            throw new NotImplementedException();
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString()
        {
            return "X=" + X + "; Y=" + Y;
        }
    }

    public struct Vector2DI : IVector2D<int>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public IVector2D<int> Parse(string s)
        {
            throw new NotImplementedException();
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString()
        {
            return "X=" + X + "; Y=" + Y;
        }

        public Vector2DI(int i)
        {
            X = i;
            Y = i;
        }

        public Vector2DI(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2DI operator *(Vector2DI a, int b)
        {
            return new Vector2DI
            {
                X = a.X * b,
                Y = a.Y * b
            };
        }

        public static Vector2DI operator +(Vector2DI a, int b)
        {
            return new Vector2DI
            {
                X = a.X + b,
                Y = a.Y + b
            };
        }

        public static Vector2DI operator -(Vector2DI a, int b)
        {
            return new Vector2DI
            {
                X = a.X - b,
                Y = a.Y - b
            };
        }
    }

    public struct Vector : IVector<float>
    {
        public float X { get; set; }
        public float Y { get; set; }
        public IVector2D<float> Parse(string s)
        {
            throw new NotImplementedException();
        }

        public float Z { get; set; }
    }

    public struct Vector4D : IVector4D<float>
    {
        public float X { get; set; }
        public float Y { get; set; }
        public IVector2D<float> Parse(string s)
        {
            throw new NotImplementedException();
        }

        public float Z { get; set; }
        public float W { get; set; }
    }

    public struct Vector2DD : IVector2D<double>
    {
        public double X { get; set; }
        public double Y { get; set; }
        public IVector2D<double> Parse(string s)
        {
            string[] a = s.Split(';');
            foreach (string b in a)
            {
                string[] c = b.Split('=');
                string d = c[0].Trim();
                double e = double.Parse(c[1].Trim());
                switch (d)
                {
                    case "X":
                        X = e;
                        break;
                    case "Y":
                        Y = e;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return this;
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString()
        {
            return "X=" + X + "; Y=" + Y;
        }

        public Vector2DD(double d)
        {
            X = d;
            Y = d;
        }

        public Vector2DD(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public struct VectorD : IVector<double>
    {
        public double X { get; set; }
        public double Y { get; set; }
        public IVector2D<double> Parse(string s)
        {
            throw new NotImplementedException();
        }

        public double Z { get; set; }
    }

    public struct Vector4DD : IVector4D<double>
    {
        public double X { get; set; }
        public double Y { get; set; }
        public IVector2D<double> Parse(string s)
        {
            throw new NotImplementedException();
        }

        public double Z { get; set; }
        public double W { get; set; }
    }
}