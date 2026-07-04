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

// 迁移变更（从原始 HUDSequenceGenerator 项目迁移，适配 NetworkExecutor 权限体系）：
//   HttpClient/CookieContainer/HttpClientHandler → NetworkExecutor（全部HTTP请求通过安全执行器）
//   System.IO.File / Directory → PermissionedStreamFactory / SafePath（文件IO通过权限体系）
//   Cookie 预热 → 通过 NetworkExecutor 预热（NetworkExecutor 静态 HttpClient 自动维持 Cookie）
//
// 安全扫描合规说明：
//   本文件不直接引用 System.Net.Http / System.Net 命名空间中的任何类型，
//   所有网络请求通过 SiliconLife.Collective.NetworkExecutor 发起，
//   所有文件IO通过 SiliconLife.Collective.PermissionedStreamFactory / SafePath 操作，
//   符合插件安全扫描规则。

using SiliconLife.Collective;
using System.Xml.Linq;

namespace TravelCodeWikiWithAI.Data.OSM;

/// <summary>
/// 在线瓦片服务 - 从 TMS/XYZ 瓦片服务器获取预渲染地图瓦片（通过 NetworkExecutor 合规版）
///
/// 功能：
/// - 经纬度/缩放级别 ↔ 瓦片坐标转换（标准 Slippy Map 公式）
/// - 按需下载瓦片并缓存到本地磁盘（通过 PermissionedStreamFactory）
/// - 计算视口所需瓦片并拼接为完整底图
/// - Cookie 预热机制（首次请求前访问 OSM 主站获取 Cookie）
///
/// 缓存目录结构（由调用方指定）：
///   {cacheDir}/{z}/{x}/{y}.png
///
/// OSM 瓦片请求流程：
///   1. 首次使用前通过 NetworkExecutor 访问 openstreetmap.org 主站获取 cookies
///   2. 后续瓦片请求通过 NetworkExecutor 携带 cookies + Referer + 浏览器级 Accept 头部
///
/// 原始来源：D:\跟着AI去穷游\src\HUDSequenceGenerator\Services\TileService.cs
/// 迁移变更：HttpClient → NetworkExecutor, File IO → PermissionedStreamFactory
/// </summary>
public class TileService
{
    private const int TileSize = 256;

    // Cookie 预热状态
    private static int _warmedUp; // 0=未预热, 1=已预热

    private readonly string _cacheDir;
    private readonly TileSource _source;
    private readonly string? _customUrlTemplate;
    private readonly Guid _callerId;

    /// <summary>
    /// 创建瓦片服务实例
    /// </summary>
    /// <param name="callerId">调用者硅基人 ID，用于权限检查</param>
    /// <param name="cacheDir">瓦片缓存目录（绝对路径，如 osmcache/tiles）</param>
    /// <param name="source">瓦片源</param>
    /// <param name="customUrlTemplate">自定义URL模板（source=Custom时使用，如 https://example.com/{z}/{x}/{y}.png）</param>
    public TileService(Guid callerId, string cacheDir, TileSource source = TileSource.OpenStreetMap, string? customUrlTemplate = null)
    {
        _callerId = callerId;
        _cacheDir = cacheDir;
        _source = source;
        _customUrlTemplate = customUrlTemplate;
    }

    #region 坐标转换（标准 Slippy Map / Web Mercator）

    /// <summary>
    /// 经度 → 瓦片 X 坐标（可含小数，表示瓦片内偏移）
    /// </summary>
    public static double LonToTileX(double lon, double zoom)
    {
        return (lon + 180.0) / 360.0 * Math.Pow(2, zoom);
    }

    /// <summary>
    /// 纬度 → 瓦片 Y 坐标（可含小数，表示瓦片内偏移）
    /// </summary>
    public static double LatToTileY(double lat, double zoom)
    {
        double latRad = lat * Math.PI / 180.0;
        return (1.0 - Math.Log(Math.Tan(latRad) + 1.0 / Math.Cos(latRad)) / Math.PI) / 2.0 * Math.Pow(2, zoom);
    }

    /// <summary>
    /// 瓦片 X 坐标左边界 → 经度
    /// </summary>
    public static double TileXToLon(int x, double zoom)
    {
        return x / Math.Pow(2, zoom) * 360.0 - 180.0;
    }

    /// <summary>
    /// 瓦片 Y 坐标上边界 → 纬度
    /// </summary>
    public static double TileYToLat(int y, double zoom)
    {
        double n = Math.PI - 2.0 * Math.PI * y / Math.Pow(2, zoom);
        return 180.0 / Math.PI * Math.Atan(0.5 * (Math.Exp(n) - Math.Exp(-n)));
    }

    #endregion

    #region 瓦片URL生成

    /// <summary>
    /// 生成瓦片请求URL
    /// </summary>
    public string GetTileUrl(int x, int y, int z)
    {
        return _source switch
        {
            TileSource.OpenStreetMap => $"https://tile.openstreetmap.org/{z}/{x}/{y}.png",
            TileSource.AMap => $"https://wprd01.is.autonavi.com/appmaptile?lang=zh_cn&size=1&style=7&x={x}&y={y}&z={z}",
            TileSource.Custom when !string.IsNullOrEmpty(_customUrlTemplate) =>
                _customUrlTemplate.Replace("{z}", z.ToString())
                                  .Replace("{x}", x.ToString())
                                  .Replace("{y}", y.ToString()),
            _ => $"https://tile.openstreetmap.org/{z}/{x}/{y}.png"
        };
    }

    #endregion

    #region 瓦片请求头

    /// <summary>
    /// OSM 瓦片请求默认头（含 Referer，对 OSM 瓦片服务器是必须的）
    /// </summary>
    private static readonly Dictionary<string, string> TileHeaders = new()
    {
        ["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36",
        ["Accept"] = "image/avif,image/webp,image/apng,image/svg+xml,image/*,*/*;q=0.8",
        ["Accept-Language"] = "zh-CN,zh;q=0.9,en;q=0.8",
        ["Referer"] = "https://www.openstreetmap.org/"
    };

    #endregion

    #region 瓦片获取（缓存 + 下载）

    /// <summary>
    /// 获取单张瓦片（先查本地缓存，未命中则下载）
    /// 返回 null 表示获取失败
    /// </summary>
    public byte[]? GetTileBytes(int x, int y, int z)
    {
        if (x < 0 || y < 0 || z < 0) return null;
        int maxTile = (int)Math.Pow(2, z);
        if (x >= maxTile || y >= maxTile) return null;

        // 本地缓存路径
        string? cachePath = SafePath.Combine(_cacheDir, z.ToString(), x.ToString(), $"{y}.png");

        // 查本地缓存（通过 PermissionedStreamFactory）
        if (cachePath != null)
        {
            using var readStream = PermissionedStreamFactory.CreateReadStream(_callerId, cachePath);
            if (readStream != null)
            {
                try
                {
                    using var ms = new MemoryStream();
                    readStream.CopyTo(ms);
                    return ms.ToArray();
                }
                catch { /* 缓存损坏 */ }
            }
        }

        // 下载
        var bytes = DownloadTile(x, y, z);
        if (bytes != null)
        {
            // 写入缓存（通过 PermissionedStreamFactory）
            if (cachePath != null)
            {
                try
                {
                    using var writeStream = PermissionedStreamFactory.CreateWriteStream(_callerId, cachePath);
                    if (writeStream != null)
                    {
                        writeStream.Write(bytes, 0, bytes.Length);
                    }
                }
                catch { /* 缓存写入失败不影响使用 */ }
            }
        }

        return bytes;
    }

    private byte[]? DownloadTile(int x, int y, int z)
    {
        EnsureCookiesWarmedUp();

        var url = GetTileUrl(x, y, z);
        try
        {
            var request = new ExecutorRequest(_callerId, url, "http_get",
                new Dictionary<string, object>
                {
                    ["method"] = "GET",
                    ["headers"] = TileHeaders,
                    ["expect_binary"] = true  // 通知 NetworkExecutor 读取二进制响应
                });
            var result = NetworkExecutor.Execute(request, TimeSpan.FromSeconds(15));

            if (!result.Success) return null;

            // NetworkExecutor 现在支持二进制响应，直接获取 BinaryOutput
            return result.BinaryOutput;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 确保已通过 NetworkExecutor 访问 OSM 主站获取 cookies（首次调用时执行，后续跳过）
    /// NetworkExecutor 的静态 HttpClient 会自动维护 CookieContainer，
    /// 预热请求的 Cookie 会被后续瓦片请求自动携带。
    /// </summary>
    private void EnsureCookiesWarmedUp()
    {
        if (System.Threading.Interlocked.CompareExchange(ref _warmedUp, 1, 0) == 0)
        {
            try
            {
                // 通过 NetworkExecutor 预热 Cookie
                var request = new ExecutorRequest(_callerId, "https://www.openstreetmap.org/", "http_get",
                    new Dictionary<string, object>
                    {
                        ["method"] = "GET",
                        ["headers"] = TileHeaders
                    });
                NetworkExecutor.Execute(request, TimeSpan.FromSeconds(15));
                // 静默忽略结果，目的只是让 NetworkExecutor 的 HttpClient 收到 cookies
            }
            catch { /* 网络失败不影响后续瓦片请求 */ }
        }
    }

    #endregion
}

/// <summary>
/// 瓦片数据源
/// </summary>
public enum TileSource
{
    /// <summary>OpenStreetMap 标准瓦片（国际通用）</summary>
    OpenStreetMap,

    /// <summary>高德地图瓦片（国内路线推荐）</summary>
    AMap,

    /// <summary>自定义URL模板（含 {z}/{x}/{y} 占位符）</summary>
    Custom
}
