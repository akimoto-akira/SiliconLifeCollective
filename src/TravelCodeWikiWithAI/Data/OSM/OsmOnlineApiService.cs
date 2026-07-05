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

// 迁移变更（安全扫描合规版）：
//   HttpClient/CookieContainer/HttpClientHandler → NetworkExecutor（全部HTTP请求通过安全执行器）
//   本地 XML 文件缓存 → TravelCodeWikiWithAIPlugin.SpeedyPack（统一通过 SpeedyPack 读写）
//   会话Cookie维持 → NetworkExecutor 静态 HttpClient 自动处理（OSM API v0.6 不需要 Cookie）
//
// 安全扫描合规说明：
//   本文件不再直接引用 System.Net.Http / System.Net 命名空间中的任何类型，
//   所有网络请求通过 SiliconLife.Collective.NetworkExecutor 发起，
//   缓存数据通过 TravelCodeWikiWithAIPlugin.SpeedyPack 统一持久化，
//   符合插件安全扫描规则（Rule 2: Network access 必须通过 NetworkExecutor）。

using SiliconLife.Collective;
using SiliconLife.Speedy;
using System.Xml.Linq;

namespace TravelCodeWikiWithAI.Data.OSM;

/// <summary>
/// OSM 在线 API 服务（同步封装，通过 NetworkExecutor 合规版）。
/// 按需从 OSM API v0.6 查询元素数据，并基于版本历史自适应刷新缓存。
///
/// 设计要点：
/// - 全部方法同步返回，内部通过 NetworkExecutor 发起请求
/// - 双端点 failover（主站 + api 子域名）
/// - 本地多版本 XML 缓存写入 TravelCodeWikiWithAIPlugin.SpeedyPack
///   key 格式：{CacheDir}/{type}/{id}/v{version}.xml
/// - 缓存刷新策略：
///   · 无缓存：请求当前最新版并缓存
///   · 只有 v1：7 天内直接返回，超过 7 天查历史
///   · 只有非 v1：查历史，无新版本则补前一个版本
///   · 两个及以上版本：用最近两次编辑间隔作为刷新周期
/// - 请求头通过 ExecutorRequest.Parameters["headers"] 传递
/// - callerId 由调用方传入，确保权限检查按硅基人粒度执行
/// </summary>
public class OsmOnlineApiService
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger(typeof(OsmOnlineApiService));

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
    /// XML 缓存 key 前缀。设为 null 则不缓存。
    /// 实际 key 格式：{CacheDir}/{type}/{id}/v{version}.xml
    /// </summary>
    public static string? CacheDir { get; set; } = "osmcache/api";

    /// <summary>
    /// 是否可用（初始化后置为 true）
    /// </summary>
    public static bool OK { get; set; }

    /// <summary>
    /// 当前调用者 ID（硅基人 GUID）
    /// </summary>
    private readonly Guid _callerId;

    /// <summary>
    /// 创建 OsmOnlineApiService 实例
    /// </summary>
    /// <param name="callerId">调用者硅基人 ID，用于权限检查</param>
    public OsmOnlineApiService(Guid callerId)
    {
        _callerId = callerId;
    }

    // ========== 公开方法 ==========

    /// <summary>
    /// 查询 Relation 的 tags（轻量，不拉 full）
    /// </summary>
    public Dictionary<string, string> GetRelationTags(long osmId)
    {
        var xml = FetchElement("relation", osmId);
        var relation = xml.Element("osm")?.Element("relation");
        if (relation == null) return new Dictionary<string, string>();

        return ParseTags(relation);
    }

    /// <summary>
    /// 查询 Relation 的一级子关系成员（不拉 full，只取 relation 元素本身的 members）
    /// </summary>
    public OSMRelations? GetRelationInfo(long osmId)
    {
        var xml = FetchElement("relation", osmId);
        var relation = xml.Element("osm")?.Element("relation");
        if (relation == null) return null;

        var result = new OSMRelations
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

            result.Refs.Add(new OSMRelationRef(refId, OSMRelationRefType.Relations, role));
        }

        return result;
    }

    /// <summary>
    /// 查询 Relation 的所有成员（node/way/relation），含 tags
    /// </summary>
    public OSMRelations? GetRelationDetail(long osmId)
    {
        var xml = FetchElement("relation", osmId);
        var relation = xml.Element("osm")?.Element("relation");
        if (relation == null) return null;

        var result = new OSMRelations
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

            result.Refs.Add(new OSMRelationRef(refId, ParseRefType(type), role));
        }

        return result;
    }

    /// <summary>
    /// 查询 Node 的 tags 和坐标
    /// </summary>
    public OSMNode? GetNodeInfo(long osmId)
    {
        var xml = FetchElement("node", osmId);
        var node = xml.Element("osm")?.Element("node");
        if (node == null) return null;

        double.TryParse(node.Attribute("lat")?.Value, System.Globalization.NumberStyles.Float,
            System.Globalization.CultureInfo.InvariantCulture, out var lat);
        double.TryParse(node.Attribute("lon")?.Value, System.Globalization.NumberStyles.Float,
            System.Globalization.CultureInfo.InvariantCulture, out var lon);

        return new OSMNode
        {
            Id = osmId,
            LngLat = new Vector2DD(lon, lat),
            Tags = ParseTags(node)
        };
    }

    /// <summary>
    /// 查询 Node 的 tags（轻量，不拉坐标）
    /// </summary>
    public Dictionary<string, string> GetNodeTags(long osmId)
    {
        var xml = FetchElement("node", osmId);
        var node = xml.Element("osm")?.Element("node");
        if (node == null) return new Dictionary<string, string>();

        return ParseTags(node);
    }

    /// <summary>
    /// 查询 Way 的 tags
    /// </summary>
    public Dictionary<string, string> GetWayTags(long osmId)
    {
        var xml = FetchElement("way", osmId);
        var way = xml.Element("osm")?.Element("way");
        if (way == null) return new Dictionary<string, string>();

        return ParseTags(way);
    }

    // ========== 内部 HTTP（通过 NetworkExecutor） ==========

    /// <summary>
    /// 默认请求头（模拟浏览器，对 OSM API 是必要的）
    /// </summary>
    private static readonly Dictionary<string, string> DefaultHeaders = new()
    {
        ["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36",
        ["Accept"] = "application/xml,text/xml,*/*",
        ["Accept-Language"] = "zh-CN,zh;q=0.9,en;q=0.8"
    };

    private XDocument FetchFromNetwork(string relativePath)
    {
        Exception? lastException = null;

        for (int attempt = 0; attempt < BaseUrls.Length; attempt++)
        {
            string url = BaseUrls[attempt] + "/" + relativePath;

            try
            {
                var request = new ExecutorRequest(_callerId, url, "http_get",
                    new Dictionary<string, object>
                    {
                        ["method"] = "GET",
                        ["headers"] = DefaultHeaders
                    });
                var result = NetworkExecutor.Execute(request, TimeSpan.FromSeconds(30));

                if (result.Success && result.Output != null)
                {
                    // 记住可用端点
                    _workingIndex = attempt;
                    return XDocument.Parse(result.Output);
                }

                // 非 SSL/连接错误，直接失败（不再尝试下一个端点）
                lastException = new InvalidOperationException(result.Error ?? $"HTTP request failed for {url}");
            }
            catch (Exception ex) when (IsConnectionOrSslError(ex))
            {
                lastException = ex;
                continue;
            }
        }

        throw new InvalidOperationException(
            $"无法连接 OSM API（已尝试 {BaseUrls.Length} 个端点），路径：{relativePath}。" +
            $"最后错误：{lastException?.Message}", lastException);
    }

    private XDocument FetchHistory(string type, long id)
    {
        return FetchFromNetwork($"{type}/{id}/history");
    }

    private XDocument FetchElement(string type, long id)
    {
        var pack = TravelCodeWikiWithAIPlugin.SpeedyPack;
        var cachedVersions = ListCachedVersions(type, id);

        // A. 无缓存：请求当前最新版并缓存
        if (cachedVersions.Count == 0)
        {
            var doc = FetchFromNetwork($"{type}/{id}");
            WriteVersionCache(type, id, doc, pack);
            return doc;
        }

        int latestCached = cachedVersions.Max();
        var latestDoc = ReadCachedVersion(type, id, latestCached);
        if (latestDoc == null)
        {
            // 缓存损坏，重新请求当前最新版
            var doc = FetchFromNetwork($"{type}/{id}");
            WriteVersionCache(type, id, doc, pack);
            return doc;
        }

        var (parsedVersion, latestTime) = TryParseVersionTimestamp(latestDoc, type);
        if (parsedVersion <= 0 || latestTime == DateTime.MinValue)
        {
            // 解析失败，重新请求当前最新版
            var doc = FetchFromNetwork($"{type}/{id}");
            WriteVersionCache(type, id, doc, pack);
            return doc;
        }

        // B. 只有一个缓存版本
        if (cachedVersions.Count == 1)
        {
            if (latestCached == 1)
            {
                // v1 兜底 7 天
                if (DateTime.UtcNow - latestTime <= TimeSpan.FromDays(7))
                    return latestDoc;
            }

            // 请求历史检查更新
            var history = FetchHistory(type, id);
            int historyLatest = GetLatestVersionFromHistory(history, type);

            if (historyLatest > latestCached)
            {
                var newDoc = ExtractVersionFromHistory(history, type, historyLatest);
                WriteVersionCache(type, id, newDoc, pack);
                return newDoc;
            }

            // 无新版本且 v1：直接返回
            if (latestCached == 1)
                return latestDoc;

            // 无新版本且非 v1：补前一个版本
            var prevHistoryDoc = ExtractVersionFromHistory(history, type, latestCached - 1);
            WriteVersionCache(type, id, prevHistoryDoc, pack);
            return latestDoc;
        }

        // C. 两个及以上缓存版本
        int prevVersion = cachedVersions.OrderDescending().Skip(1).First();
        var prevDoc = ReadCachedVersion(type, id, prevVersion);
        if (prevDoc == null)
        {
            // 前一个版本缓存损坏，重新请求当前最新版
            var doc = FetchFromNetwork($"{type}/{id}");
            WriteVersionCache(type, id, doc, pack);
            return doc;
        }

        var (_, prevTime) = TryParseVersionTimestamp(prevDoc, type);
        if (prevTime == DateTime.MinValue)
        {
            var doc = FetchFromNetwork($"{type}/{id}");
            WriteVersionCache(type, id, doc, pack);
            return doc;
        }

        TimeSpan editInterval = latestTime - prevTime;

        if (DateTime.UtcNow - latestTime <= editInterval)
            return latestDoc;

        var history2 = FetchHistory(type, id);
        int historyLatest2 = GetLatestVersionFromHistory(history2, type);

        if (historyLatest2 > latestCached)
        {
            var newDoc = ExtractVersionFromHistory(history2, type, historyLatest2);
            WriteVersionCache(type, id, newDoc, pack);
            return newDoc;
        }

        return latestDoc;
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

    // ========== 缓存（通过 TravelCodeWikiWithAIPlugin.SpeedyPack，多版本） ==========

    private static string? GetVersionKey(string type, long id, int version)
    {
        if (string.IsNullOrEmpty(CacheDir)) return null;
        return $"{CacheDir.TrimEnd('/')}/{type}/{id}/v{version}.xml";
    }

    private static string GetElementBaseKey(string type, long id)
    {
        return $"{CacheDir?.TrimEnd('/')}/{type}/{id}";
    }

    private List<int> ListCachedVersions(string type, long id)
    {
        var pack = TravelCodeWikiWithAIPlugin.SpeedyPack;
        if (pack == null || string.IsNullOrEmpty(CacheDir)) return new List<int>();

        try
        {
            var entries = pack.ListEntries(GetElementBaseKey(type, id));
            var versions = new List<int>();
            foreach (var entry in entries)
            {
                string fileName = System.IO.Path.GetFileName(entry);
                if (fileName.StartsWith("v") && fileName.EndsWith(".xml"))
                {
                    string versionStr = fileName.Substring(1, fileName.Length - ".xml".Length - 1);
                    if (int.TryParse(versionStr, out int version))
                    {
                        versions.Add(version);
                    }
                }
            }
            return versions;
        }
        catch { return new List<int>(); }
    }

    private XDocument? ReadCachedVersion(string type, long id, int version)
    {
        var pack = TravelCodeWikiWithAIPlugin.SpeedyPack;
        string? key = GetVersionKey(type, id, version);
        if (pack == null || key == null) return null;

        try
        {
            var bytes = pack.Read(key);
            if (bytes == null) return null;
            using var ms = new MemoryStream(bytes);
            return XDocument.Load(ms);
        }
        catch { return null; }
    }

    private static (int version, DateTime timestamp) TryParseVersionTimestamp(XDocument doc, string type)
    {
        var element = doc.Element("osm")?.Element(type);
        if (element == null) return (0, DateTime.MinValue);

        int version = int.TryParse(element.Attribute("version")?.Value, out var v) ? v : 0;
        DateTime timestamp = DateTime.TryParse(element.Attribute("timestamp")?.Value, out var t) ? t : DateTime.MinValue;
        return (version, timestamp);
    }

    private static int GetLatestVersionFromHistory(XDocument history, string type)
    {
        var elements = history.Element("osm")?.Elements(type);
        if (elements == null) return 0;

        int maxVersion = 0;
        foreach (var element in elements)
        {
            int version = int.TryParse(element.Attribute("version")?.Value, out var v) ? v : 0;
            if (version > maxVersion) maxVersion = version;
        }
        return maxVersion;
    }

    private static XDocument ExtractVersionFromHistory(XDocument history, string type, int version)
    {
        var element = history.Element("osm")?.Elements(type)
            .FirstOrDefault(e => int.TryParse(e.Attribute("version")?.Value, out var v) && v == version);

        if (element == null)
            throw new InvalidOperationException($"历史记录中不存在 {type} 版本 {version}");

        return new XDocument(new XElement("osm", element));
    }

    private static void WriteVersionCache(string type, long id, XDocument doc, SpeedyPack? pack)
    {
        if (pack == null || string.IsNullOrEmpty(CacheDir)) return;

        var (version, _) = TryParseVersionTimestamp(doc, type);
        if (version <= 0) return;

        string? key = GetVersionKey(type, id, version);
        if (key == null) return;

        try
        {
            using var ms = new MemoryStream();
            doc.Save(ms);
            pack.Write(key, ms.ToArray(), "text");
        }
        catch { /* 缓存写入失败不影响主流程 */ }
    }

    // ========== XML 解析 ==========

    private static OSMRelationRefType ParseRefType(string type) => type switch
    {
        "node" => OSMRelationRefType.Node,
        "way" => OSMRelationRefType.Way,
        "relation" => OSMRelationRefType.Relations,
        _ => OSMRelationRefType.None
    };

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
