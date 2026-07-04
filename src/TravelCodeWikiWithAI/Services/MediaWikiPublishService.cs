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
//   HttpWebRequest → 已移除
//   MultipartFormDataContent → NetworkExecutor http_post_multipart 类型
//   会话Cookie维持 → 通过 login token + CSRF token 机制（MediaWiki API 支持无 Cookie 模式）
//
// 安全扫描合规说明：
//   本文件不再直接引用 System.Net.Http / System.Net 命名空间中的任何类型，
//   所有网络请求通过 SiliconLife.Collective.NetworkExecutor 发起，
//   符合插件安全扫描规则（Rule 2: Network access 必须通过 NetworkExecutor）。

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using SiliconLife.Collective;
using TravelCodeWikiWithAI.Data;

namespace TravelCodeWikiWithAI.Services;

/// <summary>
/// MediaWiki API 发布服务。
/// 封装与 MediaWiki 站点的交互：登录、编辑页面、上传媒体文件。
/// 所有 HTTP 请求通过 NetworkExecutor 发起（遵循 Executor 安全模式），符合插件安全扫描规则。
/// 对应7步流程：步骤7（发布到MediaWiki）
/// </summary>
public class MediaWikiPublishService
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger(typeof(MediaWikiPublishService));

    /// <summary>
    /// 发布结果
    /// </summary>
    public class PublishResult
    {
        public bool Success { get; set; }
        public int PagesPublished { get; set; }
        public int PagesSkipped { get; set; }
        public int PagesFailed { get; set; }
        public int FilesUploaded { get; set; }
        public int FilesSkipped { get; set; }
        public int FilesFailed { get; set; }
        public List<string> Errors { get; set; } = new();
        public override string ToString() =>
            $"Success={Success}, Pages={PagesPublished}+{PagesSkipped}skip/{PagesFailed}fail, Files={FilesUploaded}+{FilesSkipped}skip/{FilesFailed}fail";
    }

    // ===== 配置 =====
    public string ApiUrl { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsEnabled => !string.IsNullOrEmpty(ApiUrl) && !string.IsNullOrEmpty(Username);

    // ===== 会话状态 =====
    private string? _csrfToken;
    private bool _isLoggedIn;
    private readonly Dictionary<string, string> _cachedPages = new();
    private readonly Dictionary<string, string> _cachedFileSha1 = new();
    private Guid _callerId;

    public void SetCallerId(Guid callerId) => _callerId = callerId;

    // ==================== 公共 API ====================

    /// <summary>
    /// 发布一个实体的所有文档到 MediaWiki 站点。
    /// 先调用 BuildDocument 生成页面字典，再逐一上传。
    /// </summary>
    public PublishResult PublishEntity(GeoLocation location)
    {
        var result = new PublishResult();
        if (!IsEnabled)
        {
            result.Errors.Add("MediaWiki publish service not configured");
            return result;
        }

        try
        {
            if (!EnsureLoggedIn())
            {
                result.Errors.Add("Login failed");
                return result;
            }

            // 首次发布时缓存已有页面
            if (_cachedPages.Count == 0) CacheExistingPages();

            // BuildDocument 生成页面
            var files = new Dictionary<string, byte[]>();
            Dictionary<string, string> documents;
            try { documents = location.BuildDocument(files); }
            catch (NotImplementedException ex)
            {
                result.Errors.Add($"BuildDocument not implemented: {ex.Message}");
                return result;
            }

            if (documents.Count == 0)
            {
                result.Success = true;
                return result;
            }

            if (!RefreshCsrfToken())
            {
                result.Errors.Add("Failed to get CSRF token");
                return result;
            }

            // 发布页面
            foreach (var kvp in documents)
            {
                var editRes = EditPage(kvp.Key, kvp.Value);
                if (editRes.Success && !editRes.NoChange) result.PagesPublished++;
                else if (editRes.NoChange) result.PagesSkipped++;
                else { result.PagesFailed++; result.Errors.Add($"Page '{kvp.Key}': {editRes.Error}"); }
            }

            // 上传文件
            foreach (var kvp in files)
            {
                var uploadRes = UploadFile(kvp.Key, kvp.Value);
                if (uploadRes == UploadStatus.Uploaded) result.FilesUploaded++;
                else if (uploadRes == UploadStatus.Skipped) result.FilesSkipped++;
                else { result.FilesFailed++; result.Errors.Add($"File '{kvp.Key}' upload failed"); }
            }

            result.Success = result.PagesFailed == 0 && result.FilesFailed == 0;

            if (result.Success) SetEntityPublished(location, true);
            _logger.Info(null, "PublishEntity: {0}", result);
        }
        catch (Exception ex)
        {
            result.Errors.Add(ex.Message);
            _logger.Error(null, "PublishEntity failed: {0}", ex);
        }
        return result;
    }

    /// <summary>
    /// 批量发布多个实体
    /// </summary>
    public PublishResult PublishEntities(IEnumerable<GeoLocation> locations)
    {
        var total = new PublishResult();
        foreach (var loc in locations)
        {
            var r = PublishEntity(loc);
            total.PagesPublished += r.PagesPublished;
            total.PagesSkipped += r.PagesSkipped;
            total.PagesFailed += r.PagesFailed;
            total.FilesUploaded += r.FilesUploaded;
            total.FilesSkipped += r.FilesSkipped;
            total.FilesFailed += r.FilesFailed;
            total.Errors.AddRange(r.Errors);
        }
        total.Success = total.PagesFailed == 0 && total.FilesFailed == 0;
        return total;
    }

    /// <summary>
    /// 测试站点连通性
    /// </summary>
    public bool TestConnection()
    {
        if (string.IsNullOrEmpty(ApiUrl)) return false;
        try
        {
            string url = BuildQueryUrl(new Dictionary<string, string>
            {
                ["action"] = "query", ["meta"] = "siteinfo", ["siprop"] = "general", ["format"] = "json"
            });
            string? resp = ExecuteNetworkGet(url);
            if (resp == null) return false;
            using var doc = JsonDocument.Parse(resp);
            return doc.RootElement.TryGetProperty("query", out _);
        }
        catch { return false; }
    }

    // ==================== 登录流程 ====================

    private bool EnsureLoggedIn()
    {
        if (_isLoggedIn) return true;
        try
        {
            // 获取 login token
            string? loginToken = GetToken("login");
            if (loginToken == null) { _logger.Error(null, "Failed to get login token"); return false; }

            // 登录 — 通过 NetworkExecutor POST
            var formData = new Dictionary<string, string>
            {
                ["action"] = "login", ["lgname"] = Username, ["lgpassword"] = Password,
                ["lgtoken"] = loginToken, ["format"] = "json"
            };
            string? resp = ExecutePostForm(formData);
            if (resp == null) return false;

            using var doc = JsonDocument.Parse(resp);
            string? loginResult = GetString(doc.RootElement, "login", "result");
            if (loginResult == "Success")
            {
                _isLoggedIn = true;
                _logger.Info(null, "Logged in to MediaWiki as {0}", Username);
                return true;
            }
            _logger.Error(null, "Login failed: {0}", loginResult);
            return false;
        }
        catch (Exception ex) { _logger.Error(null, "Login exception: {0}", ex.Message); return false; }
    }

    private string? GetToken(string type)
    {
        string url = BuildQueryUrl(new Dictionary<string, string>
        {
            ["action"] = "query", ["meta"] = "tokens", ["type"] = type, ["format"] = "json"
        });
        string? resp = ExecuteNetworkGet(url);
        if (resp == null) return null;
        using var doc = JsonDocument.Parse(resp);
        return GetString(doc.RootElement, "query", "tokens", type + "token");
    }

    private bool RefreshCsrfToken()
    {
        string? token = GetToken("csrf");
        if (token != null) { _csrfToken = token; return true; }
        _logger.Error(null, "Failed to get CSRF token");
        return false;
    }

    // ==================== 页面编辑 ====================

    private (bool Success, bool NoChange, string? Error) EditPage(string title, string content)
    {
        string normalizedTitle = title.Replace("_", " ");
        string normalizedContent = content.Replace("\r\n", "\n").TrimEnd('\n');

        // 缓存比对
        if (_cachedPages.TryGetValue(normalizedTitle, out var cached) && StringEquals(cached, normalizedContent))
            return (true, true, null);

        var resp = ExecutePostForm(new Dictionary<string, string>
        {
            ["action"] = "edit", ["title"] = title, ["text"] = normalizedContent,
            ["token"] = _csrfToken ?? "", ["format"] = "json"
        });
        if (resp == null) return (false, false, "No response");

        try
        {
            using var doc = JsonDocument.Parse(resp);
            if (doc.RootElement.TryGetProperty("error", out var err))
                return (false, false, $"{GetString(err, "code")}: {GetString(err, "info")}");

            if (doc.RootElement.TryGetProperty("edit", out var edit))
            {
                string? editResult = GetString(edit, "result");
                if (editResult == "Success")
                {
                    bool noChange = edit.TryGetProperty("nochange", out _);
                    return (true, noChange, null);
                }
                return (false, false, $"Edit result: {editResult}");
            }
            return (false, false, "No edit result in response");
        }
        catch (Exception ex) { return (false, false, ex.Message); }
    }

    // ==================== 文件上传 ====================

    private enum UploadStatus { Uploaded, Skipped, Failed }

    private UploadStatus UploadFile(string filename, byte[] fileData)
    {
        if (fileData == null || fileData.Length == 0) return UploadStatus.Failed;

        // SHA1 比对：相同则跳过
        string hashStr = Convert.ToHexString(SHA1.HashData(fileData)).ToLowerInvariant();
        if (_cachedFileSha1.TryGetValue("File:" + filename, out var oldHash) && hashStr == oldHash)
            return UploadStatus.Skipped;

        // 通过 NetworkExecutor 的 http_post_multipart 类型上传文件
        try
        {
            var request = new ExecutorRequest(_callerId, ApiUrl, "http_post_multipart",
                new Dictionary<string, object>
                {
                    ["method"] = "POST",
                    ["fields"] = new Dictionary<string, string>
                    {
                        ["action"] = "upload",
                        ["filename"] = filename,
                        ["token"] = _csrfToken ?? "",
                        ["format"] = "json"
                    },
                    ["file_field_name"] = "file",
                    ["file_name"] = filename,
                    ["file_data"] = Convert.ToBase64String(fileData)
                });
            var result = NetworkExecutor.Execute(request, TimeSpan.FromSeconds(100));
            if (!result.Success)
            {
                _logger.Error(null, "Upload failed: {0} - {1}", filename, result.Error);
                return UploadStatus.Failed;
            }

            string responseBody = result.Output ?? "";
            using var doc = JsonDocument.Parse(responseBody);
            if (doc.RootElement.TryGetProperty("error", out _))
                return UploadStatus.Failed;

            if (doc.RootElement.TryGetProperty("upload", out var upload))
            {
                string? uploadResult = GetString(upload, "result");
                return uploadResult == "Success" ? UploadStatus.Uploaded : UploadStatus.Failed;
            }
            return UploadStatus.Failed;
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Upload failed: {0} - {1}", filename, ex.Message);
            return UploadStatus.Failed;
        }
    }

    // ==================== 缓存管理 ====================

    /// <summary>
    /// 缓存已有页面内容（用于比对变更，避免无意义编辑）。
    /// 沿用原工程 MediaWikiSite.CacheDoc 的逻辑。
    /// </summary>
    private void CacheExistingPages()
    {
        _cachedPages.Clear();
        _cachedFileSha1.Clear();
        _logger.Info(null, "Caching existing pages from MediaWiki...");

        try
        {
            // 获取所有页面标题
            var allPages = ListAllPages();
            _logger.Info(null, "Found {0} existing pages", allPages.Count);

            // 批量获取页面内容（每次50个）
            for (int i = 0; i < allPages.Count; i += 50)
            {
                var batch = allPages.Skip(i).Take(50).ToList();
                var pageContents = GetPageContents(batch);
                foreach (var kvp in pageContents)
                {
                    _cachedPages[kvp.Key.Replace("_", " ")] = kvp.Value;
                }
            }

            _logger.Info(null, "Cached {0} pages", _cachedPages.Count);
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to cache existing pages: {0}", ex.Message);
        }
    }

    private List<string> ListAllPages()
    {
        var pages = new List<string>();
        string? apcontinue = null;

        do
        {
            var param = new Dictionary<string, string>
            {
                ["action"] = "query", ["list"] = "allpages", ["aplimit"] = "max", ["format"] = "json"
            };
            if (apcontinue != null) param["apcontinue"] = apcontinue;

            string url = BuildQueryUrl(param);
            string? resp = ExecuteNetworkGet(url);
            if (resp == null) break;

            using var doc = JsonDocument.Parse(resp);
            if (doc.RootElement.TryGetProperty("query", out var query)
                && query.TryGetProperty("allpages", out var allPagesArr))
            {
                foreach (var page in allPagesArr.EnumerateArray())
                {
                    if (page.TryGetProperty("title", out var titleElem))
                        pages.Add(titleElem.GetString() ?? "");
                }
            }

            apcontinue = doc.RootElement.TryGetProperty("continue", out var cont)
                ? GetString(cont, "apcontinue") : null;
        } while (apcontinue != null);

        return pages;
    }

    private Dictionary<string, string> GetPageContents(List<string> titles)
    {
        var result = new Dictionary<string, string>();
        if (titles.Count == 0) return result;

        var param = new Dictionary<string, string>
        {
            ["action"] = "query", ["prop"] = "revisions",
            ["titles"] = string.Join("|", titles),
            ["rvprop"] = "content", ["format"] = "json"
        };

        string url = BuildQueryUrl(param);
        string? resp = ExecuteNetworkGet(url);
        if (resp == null) return result;

        try
        {
            using var doc = JsonDocument.Parse(resp);
            if (!doc.RootElement.TryGetProperty("query", out var query)
                || !query.TryGetProperty("pages", out var pagesObj)) return result;

            foreach (var page in pagesObj.EnumerateObject())
            {
                if (!page.Value.TryGetProperty("revisions", out var revs)) continue;
                foreach (var rev in revs.EnumerateArray())
                {
                    string? content = GetString(rev, "*") ?? GetString(rev, "content");
                    string? title = GetString(page.Value, "title");
                    if (title != null && content != null)
                        result[title] = content;
                }
            }
        }
        catch { /* ignore parsing errors for cache */ }

        return result;
    }

    // ==================== 网络请求（全部通过 NetworkExecutor） ====================

    /// <summary>
    /// 通过 NetworkExecutor 发起 GET 请求
    /// </summary>
    private string? ExecuteNetworkGet(string url)
    {
        var request = new ExecutorRequest(_callerId, url, "http_get",
            new Dictionary<string, object> { ["method"] = "GET" });
        var result = NetworkExecutor.Execute(request, TimeSpan.FromSeconds(100));
        return result.Success ? result.Output : null;
    }

    /// <summary>
    /// 通过 NetworkExecutor 发起 POST application/x-www-form-urlencoded 请求。
    /// 
    /// MediaWiki API 的登录/编辑需要会话维持（Cookie 或 Token 机制）。
    /// 本实现采用 Token 机制（login token + CSRF token），
    /// 不依赖 Cookie，因此可以使用无状态的 NetworkExecutor。
    /// NetworkExecutor 内部持有静态 HttpClient 实例，会自动处理 Cookie 传递。
    /// </summary>
    private string? ExecutePostForm(Dictionary<string, string> formData)
    {
        try
        {
            // 将 formData 编码为 application/x-www-form-urlencoded body
            var bodyBuilder = new StringBuilder();
            bool first = true;
            foreach (var kvp in formData)
            {
                if (!first) bodyBuilder.Append('&');
                bodyBuilder.Append(Uri.EscapeDataString(kvp.Key));
                bodyBuilder.Append('=');
                bodyBuilder.Append(Uri.EscapeDataString(kvp.Value));
                first = false;
            }

            var request = new ExecutorRequest(_callerId, ApiUrl, "http_post",
                new Dictionary<string, object>
                {
                    ["method"] = "POST",
                    ["body"] = bodyBuilder.ToString(),
                    ["headers"] = new Dictionary<string, string>
                    {
                        ["Content-Type"] = "application/x-www-form-urlencoded"
                    }
                });
            var result = NetworkExecutor.Execute(request, TimeSpan.FromSeconds(100));
            return result.Success ? result.Output : null;
        }
        catch (Exception ex)
        {
            _logger.Error(null, "POST request failed: {0}", ex.Message);
            return null;
        }
    }

    /// <summary>
    /// 构建 GET 请求 URL（含查询参数）
    /// </summary>
    private string BuildQueryUrl(Dictionary<string, string> parameters)
    {
        var sb = new StringBuilder(ApiUrl);
        sb.Append('?');
        bool first = true;
        foreach (var kvp in parameters)
        {
            if (!first) sb.Append('&');
            sb.Append(Uri.EscapeDataString(kvp.Key));
            sb.Append('=');
            sb.Append(Uri.EscapeDataString(kvp.Value));
            first = false;
        }
        return sb.ToString();
    }

    // ==================== 实体状态回写 ====================

    /// <summary>
    /// 回写发布状态到实体。使用 GeoDataBase 的现有机制存储。
    /// </summary>
    private static void SetEntityPublished(GeoLocation location, bool published)
    {
        location.SetPublished(published);
    }

    // ==================== 工具方法 ====================

    private static string? GetString(JsonElement elem, params string[] path)
    {
        JsonElement current = elem;
        foreach (string key in path)
        {
            if (!current.TryGetProperty(key, out current)) return null;
        }
        return current.ValueKind == JsonValueKind.String ? current.GetString() : null;
    }

    private static string? GetString(JsonElement elem, string key)
    {
        return elem.TryGetProperty(key, out var child) && child.ValueKind == JsonValueKind.String
            ? child.GetString() : null;
    }

    private static bool StringEquals(string a, string b)
    {
        if (a.Length != b.Length) return false;
        for (int i = 0; i < a.Length; i++)
            if (a[i] != b[i]) return false;
        return true;
    }

    private static string RandomString(int count = 16)
    {
        const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var result = new char[count];
        var rng = Random.Shared;
        for (int i = 0; i < count; i++)
            result[i] = chars[rng.Next(chars.Length)];
        return new string(result);
    }
}
