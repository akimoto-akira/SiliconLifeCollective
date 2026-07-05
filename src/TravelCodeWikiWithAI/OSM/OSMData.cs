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
    /// OSM元素类型 / OSM element type
    /// </summary>
    public enum OSMElementType
    {
        /// <summary>
        /// 关系 / Relation
        /// </summary>
        Relation,

        /// <summary>
        /// 节点 / Node
        /// </summary>
        Node,

        /// <summary>
        /// 路径 / Way
        /// </summary>
        Way
    }

    /// <summary>
    /// 固定 OSM ID 映射结构 / Fixed OSM ID mapping structure
    /// </summary>
    public struct FixedOsmMapping
    {
        /// <summary>
        /// OSM ID / OSM identifier
        /// </summary>
        public long OsmId { get; set; }

        /// <summary>
        /// 实体编码 / Entity code
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 简体中文名称 / Simplified Chinese name
        /// </summary>
        public string ZhCn { get; set; }

        /// <summary>
        /// 英文名称 / English name
        /// </summary>
        public string En { get; set; }

        /// <summary>
        /// OSM 元素类型 / OSM element type
        /// </summary>
        public OSMElementType ElementType { get; set; }

        /// <summary>
        /// 构造函数 / Constructor
        /// </summary>
        public FixedOsmMapping(long osmId, string id, string zhCn, string en, OSMElementType elementType)
        {
            OsmId = osmId;
            Id = id;
            ZhCn = zhCn;
            En = en;
            ElementType = elementType;
        }
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
}
