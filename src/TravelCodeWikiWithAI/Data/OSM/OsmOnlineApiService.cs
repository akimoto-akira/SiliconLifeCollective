using System.Net;
using System.Net.Http;
using System.Xml.Linq;

namespace TravelCodeWikiWithAI.Data.OSM;

/// <summary>
/// OSM 在线 API 服务（同步封装）。
/// 替代 PBF 本地文件，按需从 OSM API v0.6 查询 Relation 数据。
///
/// 设计要点：
/// - 全部方法同步返回，内部用 .GetAwaiter().GetResult() 包装
/// - 双端点 failover（主站 + api 子域名）
/// - 本地 XML 文件缓存，避免重复请求
/// - 请求头模拟浏览器
/// </summary>
public static class OsmOnlineApiService
{
    private static readonly HttpClientHandler _handler = new()
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli,
        CookieContainer = new CookieContainer(),
        UseCookies = true
    };

    private static readonly HttpClient _http = new(_handler)
    {
        Timeout = TimeSpan.FromSeconds(30),
        DefaultRequestHeaders =
        {
            { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36" },
            { "Accept", "application/xml,text/xml,*/*" },
            { "Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8" }
        }
    };

    /// <summary>
    /// OSM API 备用端点列表（按优先级排序）。
    /// 主站在国内可能 DNS 污染，api 子域名可能独立解析。
    /// </summary>
    private static readonly string[] BaseUrls =
    [
        "https://www.openstreetmap.org/api/0.6",
        "https://api.openstreetmap.org/api/0.6"
    ];

    private static int _workingIndex = -1;

    /// <summary>
    /// 本地 XML 缓存目录。设为 null 则不缓存。
    /// </summary>
    public static string? CacheDir { get; set; } = Path.Combine(
        Environment.CurrentDirectory, "osmcache", "api");

    /// <summary>
    /// 是否可用（初始化后置为 true）
    /// </summary>
    public static bool OK { get; set; }

    // ========== 公开方法 ==========

    /// <summary>
    /// 查询 Relation 的 tags（轻量，不拉 full）
    /// </summary>
    public static Dictionary<string, string> GetRelationTags(long osmId)
    {
        var xml = FetchXml($"relation/{osmId}");
        var relation = xml.Element("osm")?.Element("relation");
        if (relation == null) return new Dictionary<string, string>();

        return ParseTags(relation);
    }

    /// <summary>
    /// 查询 Relation 的一级子关系成员（不拉 full，只取 relation 元素本身的 members）
    /// </summary>
    public static OsmRelationInfo? GetRelationInfo(long osmId)
    {
        var xml = FetchXml($"relation/{osmId}");
        var relation = xml.Element("osm")?.Element("relation");
        if (relation == null) return null;

        var result = new OsmRelationInfo
        {
            Id = osmId,
            Tags = ParseTags(relation)
        };

        foreach (var member in relation.Elements("member"))
        {
            string type = member.Attribute("type")?.Value ?? "";
            if (type != "relation") continue;

            long refId = long.TryParse(member.Attribute("ref")?.Value, out var id) ? id : 0;
            string role = member.Attribute("role")?.Value ?? "";

            if (refId == 0) continue;

            result.SubRelations.Add(new OsmMemberInfo
            {
                Id = refId,
                Role = role
            });
        }

        return result;
    }

    /// <summary>
    /// 查询 Relation 的所有成员（node/way/relation），含 tags
    /// </summary>
    public static OsmRelationDetail? GetRelationDetail(long osmId)
    {
        var xml = FetchXml($"relation/{osmId}");
        var relation = xml.Element("osm")?.Element("relation");
        if (relation == null) return null;

        var result = new OsmRelationDetail
        {
            Id = osmId,
            Tags = ParseTags(relation)
        };

        foreach (var member in relation.Elements("member"))
        {
            string type = member.Attribute("type")?.Value ?? "";
            long refId = long.TryParse(member.Attribute("ref")?.Value, out var id) ? id : 0;
            string role = member.Attribute("role")?.Value ?? "";

            if (refId == 0) continue;

            result.Members.Add(new OsmMemberDetail
            {
                Id = refId,
                Type = type,
                Role = role
            });
        }

        return result;
    }

    /// <summary>
    /// 查询 Node 的 tags 和坐标
    /// </summary>
    public static OsmNodeInfo? GetNodeInfo(long osmId)
    {
        var xml = FetchXml($"node/{osmId}");
        var node = xml.Element("osm")?.Element("node");
        if (node == null) return null;

        double.TryParse(node.Attribute("lat")?.Value, System.Globalization.NumberStyles.Float,
            System.Globalization.CultureInfo.InvariantCulture, out var lat);
        double.TryParse(node.Attribute("lon")?.Value, System.Globalization.NumberStyles.Float,
            System.Globalization.CultureInfo.InvariantCulture, out var lon);

        return new OsmNodeInfo
        {
            Id = osmId,
            Lat = lat,
            Lon = lon,
            Tags = ParseTags(node)
        };
    }

    /// <summary>
    /// 查询 Way 的 tags
    /// </summary>
    public static Dictionary<string, string> GetWayTags(long osmId)
    {
        var xml = FetchXml($"way/{osmId}");
        var way = xml.Element("osm")?.Element("way");
        if (way == null) return new Dictionary<string, string>();

        return ParseTags(way);
    }

    // ========== 内部 HTTP ==========

    private static XDocument FetchXml(string relativePath)
    {
        // 1. 查本地缓存
        string? cachePath = GetCachePath(relativePath);
        if (cachePath != null && File.Exists(cachePath))
        {
            try { return XDocument.Load(cachePath); }
            catch { /* 缓存损坏，继续网络请求 */ }
        }

        Exception? lastException = null;

        // 2. 按顺序尝试每个端点
        for (int attempt = 0; attempt < BaseUrls.Length; attempt++)
        {
            string url = BaseUrls[attempt] + "/" + relativePath;

            try
            {
                var response = _http.GetAsync(url).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();

                using var stream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
                var doc = XDocument.Load(stream);

                // 记住可用端点
                _workingIndex = attempt;

                // 写入缓存
                WriteCache(cachePath, doc);

                return doc;
            }
            catch (Exception ex) when (IsConnectionOrSslError(ex))
            {
                lastException = ex;
                continue;
            }
        }

        // 3. 所有端点失败，尝试过期缓存
        if (cachePath != null && File.Exists(cachePath))
        {
            try { return XDocument.Load(cachePath); }
            catch { /* 忽略 */ }
        }

        throw new InvalidOperationException(
            $"无法连接 OSM API（已尝试 {BaseUrls.Length} 个端点），路径：{relativePath}。" +
            $"最后错误：{lastException?.Message}", lastException);
    }

    private static bool IsConnectionOrSslError(Exception ex)
    {
        for (var current = ex; current != null; current = current.InnerException)
        {
            var typeName = current.GetType().FullName ?? current.GetType().Name;
            if (typeName.Contains("AuthenticationException") ||
                typeName.Contains("SocketException") ||
                typeName.Contains("SslStream") ||
                typeName.Contains("SslPacket"))
                return true;
            if (current is OperationCanceledException or TimeoutException)
                return true;
        }
        if (ex is AggregateException agg)
        {
            foreach (var inner in agg.InnerExceptions)
                if (IsConnectionOrSslError(inner)) return true;
        }
        return false;
    }

    // ========== 缓存 ==========

    private static string? GetCachePath(string relativePath)
    {
        if (string.IsNullOrEmpty(CacheDir)) return null;
        // relativePath 格式如 "relation/12345" 或 "relation/12345/full"
        string sanitized = relativePath.Replace('/', Path.DirectorySeparatorChar);
        return Path.Combine(CacheDir, sanitized + ".xml");
    }

    private static void WriteCache(string? cachePath, XDocument doc)
    {
        if (cachePath == null) return;
        try
        {
            var dir = Path.GetDirectoryName(cachePath)!;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            doc.Save(cachePath);
        }
        catch { /* 缓存写入失败不影响主流程 */ }
    }

    // ========== XML 解析 ==========

    private static Dictionary<string, string> ParseTags(XElement element)
    {
        var tags = new Dictionary<string, string>();
        foreach (var tag in element.Elements("tag"))
        {
            string key = tag.Attribute("k")?.Value ?? "";
            string value = tag.Attribute("v")?.Value ?? "";
            if (!string.IsNullOrEmpty(key))
                tags[key] = value;
        }
        return tags;
    }
}

/// <summary>
/// Node 的轻量信息（tags + 坐标）
/// </summary>
public class OsmNodeInfo
{
    public long Id { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public Dictionary<string, string> Tags { get; set; } = new();
}

/// <summary>
/// Relation 的轻量信息（tags + 一级子关系成员）
/// </summary>
public class OsmRelationInfo
{
    public long Id { get; set; }
    public Dictionary<string, string> Tags { get; set; } = new();
    public List<OsmMemberInfo> SubRelations { get; set; } = new();
}

/// <summary>
/// 子关系成员信息
/// </summary>
public class OsmMemberInfo
{
    public long Id { get; set; }
    public string Role { get; set; } = "";
}

/// <summary>
/// Relation 的完整信息（tags + 所有类型的成员）
/// </summary>
public class OsmRelationDetail
{
    public long Id { get; set; }
    public Dictionary<string, string> Tags { get; set; } = new();
    public List<OsmMemberDetail> Members { get; set; } = new();
}

/// <summary>
/// 成员详细信息（含类型 node/way/relation）
/// </summary>
public class OsmMemberDetail
{
    public long Id { get; set; }
    public string Type { get; set; } = "";
    public string Role { get; set; } = "";
}
