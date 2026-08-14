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

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

using SiliconLife.App.Data;
using SiliconLife.App.IM;
using SiliconLife.Collective;
using SiliconLife.Common.IM;

namespace SiliconLife.App.Web;

/// <summary>
/// IM 平台"本地优先"OAuth 授权服务：负责 state 生成/校验、授权 URL 拼装、
/// code→token 交换、token 写回配置以及通过共享 SSE 推送 im_auth_status 事件。
/// 端点模板来自 <see cref="IMProviderRegistry"/>；模板为 null 的平台视为暂不支持一键授权。
/// </summary>
public sealed class ImOAuthService
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<ImOAuthService>();
    private static readonly Lazy<ImOAuthService> _instance = new(() => new ImOAuthService());
    private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(30) };

    /// <summary>授权等待超时时长（超过后经 SSE 推送 timeout 状态）。</summary>
    private static readonly TimeSpan AuthTimeout = TimeSpan.FromMinutes(5);

    public static ImOAuthService Instance => _instance.Value;

    private ImOAuthService() { }

    /// <summary>单个平台的进行中授权会话（内存缓存，按平台一份）。</summary>
    private sealed class PendingAuth
    {
        public string State { get; init; } = string.Empty;
        public string RedirectUri { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public string Status { get; set; } = "pending";
        public CancellationTokenSource Cts { get; init; } = new();
    }

    private readonly Dictionary<string, PendingAuth> _pending = new(StringComparer.OrdinalIgnoreCase);
    private readonly object _lock = new();

    /// <summary>
    /// 发起授权：校验平台元数据与模板，生成加密随机 state，
    /// 登记 pending 会话并返回拼装好的授权页 URL。
    /// </summary>
    public (bool Success, string Message, string? Url) StartAuthorization(string platform)
    {
        var metadata = IMProviderRegistry.Get(platform);
        if (metadata == null)
        {
            return (false, $"未注册的 IM 平台: {platform}", null);
        }

        if (!metadata.AuthModes.Contains("oauth"))
        {
            return (false, "该平台不支持 OAuth 授权", null);
        }

        if (string.IsNullOrEmpty(metadata.AuthorizeUrlTemplate) || string.IsNullOrEmpty(metadata.TokenUrlTemplate))
        {
            return (false, "该平台暂不支持一键授权", null);
        }

        var platformConfig = FindPlatformConfig(platform);
        if (platformConfig == null)
        {
            return (false, "请先在配置中添加该平台并保存", null);
        }

        var clientId = GetConfigString(platformConfig.Config, "appId", "appKey", "corpId", "clientId");
        if (string.IsNullOrEmpty(clientId))
        {
            return (false, "请先填写并保存该平台的应用凭证（App ID / App Key）", null);
        }

        // 加密随机 state（防 CSRF）
        var state = Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
        var redirectBase = GetRedirectBaseUrl(platformConfig);
        var redirectUri = $"{redirectBase.TrimEnd('/')}/im/{platform.ToLowerInvariant()}/callback";

        var url = metadata.AuthorizeUrlTemplate
            .Replace("{clientId}", Uri.EscapeDataString(clientId))
            .Replace("{redirectUri}", Uri.EscapeDataString(redirectUri))
            .Replace("{state}", state);

        PendingAuth pending;
        lock (_lock)
        {
            // 覆盖旧会话时先取消其超时任务
            if (_pending.TryGetValue(platform, out var old))
            {
                old.Cts.Cancel();
            }
            pending = new PendingAuth { State = state, RedirectUri = redirectUri };
            _pending[platform] = pending;
        }

        // 5 分钟超时监视：未被回调取消则置 timeout 并经 SSE 推送
        var token = pending.Cts.Token;
        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(AuthTimeout, token);
                lock (_lock)
                {
                    if (_pending.TryGetValue(platform, out var p) && ReferenceEquals(p, pending) && p.Status == "pending")
                    {
                        p.Status = "timeout";
                    }
                    else
                    {
                        return;
                    }
                }
                PushStatus(platform, "timeout", "授权超时");
            }
            catch (TaskCanceledException)
            {
                // 回调已到达，正常取消
            }
        }, CancellationToken.None);

        _logger.Info(null, "IM OAuth authorization started for platform {0}", platform);
        return (true, "已打开系统浏览器，请在浏览器中完成授权", url);
    }

    /// <summary>
    /// 处理授权回调：校验 state → 换取 token → 写回配置并保存 → SSE 推送结果。
    /// </summary>
    public async Task<(bool Success, string Message)> HandleCallbackAsync(string platform, string code, string state)
    {
        PendingAuth? pending;
        lock (_lock)
        {
            _pending.TryGetValue(platform, out pending);
        }

        if (pending == null || pending.Status != "pending" || string.IsNullOrEmpty(state) || pending.State != state)
        {
            MarkFailed(platform, pending, "state 校验失败");
            return (false, "state 校验失败，请重新发起授权");
        }

        if (string.IsNullOrEmpty(code))
        {
            MarkFailed(platform, pending, "未收到授权码");
            return (false, "未收到授权码（code），请重新发起授权");
        }

        var metadata = IMProviderRegistry.Get(platform);
        var platformConfig = FindPlatformConfig(platform);
        if (metadata == null || string.IsNullOrEmpty(metadata.TokenUrlTemplate) || platformConfig == null)
        {
            MarkFailed(platform, pending, "平台配置缺失");
            return (false, "平台配置缺失，无法换取令牌");
        }

        try
        {
            var result = await ExchangeTokenAsync(metadata, platformConfig, code, pending.RedirectUri);
            if (!result.Success)
            {
                MarkFailed(platform, pending, result.Message);
                return (false, result.Message);
            }

            // token 写入 Config 字典（不改 IMPlatformConfig 类结构）并持久化
            platformConfig.Config["accessToken"] = result.AccessToken!;
            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                platformConfig.Config["refreshToken"] = result.RefreshToken;
            }
            platformConfig.Config["tokenExpiresAt"] = DateTime.UtcNow.AddSeconds(result.ExpiresIn).ToString("O");
            platformConfig.Config["authMode"] = "oauth";
            Config.Instance?.SaveConfig();

            lock (_lock)
            {
                pending.Status = "success";
                pending.Cts.Cancel();
            }
            PushStatus(platform, "success", "授权成功");
            _logger.Info(null, "IM OAuth authorization succeeded for platform {0}", platform);
            return (true, "授权成功");
        }
        catch (Exception ex)
        {
            _logger.Error(null, "IM OAuth token exchange failed for platform {0}: {1}", platform, ex.Message);
            MarkFailed(platform, pending, ex.Message);
            return (false, $"换取令牌失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 查询平台当前授权状态（pending/success/failed/timeout/none），供前端轮询兜底。
    /// </summary>
    public object GetStatus(string platform)
    {
        string status;
        lock (_lock)
        {
            if (_pending.TryGetValue(platform, out var pending))
            {
                status = pending.Status;
            }
            else
            {
                var cfg = FindPlatformConfig(platform);
                var hasToken = cfg != null && !string.IsNullOrEmpty(GetConfigString(cfg.Config, "accessToken"));
                status = hasToken ? "success" : "none";
            }
        }

        var tokenExpiresAt = FindPlatformConfig(platform) is { } pc
            ? GetConfigString(pc.Config, "tokenExpiresAt")
            : null;
        return new { platform, status, tokenExpiresAt };
    }

    /// <summary>token 交换结果。</summary>
    private sealed record TokenResult(bool Success, string Message, string? AccessToken = null, string? RefreshToken = null, long ExpiresIn = 0);

    /// <summary>
    /// code→token 交换。目前实现飞书完整流程（JSON POST authen/v2/oauth/token）；
    /// 其余平台按通用 JSON 响应解析 access_token 字段。
    /// </summary>
    private static async Task<TokenResult> ExchangeTokenAsync(IMProviderMetadata metadata, IMPlatformConfig platformConfig, string code, string redirectUri)
    {
        var clientId = GetConfigString(platformConfig.Config, "appId", "appKey", "corpId", "clientId") ?? "";
        var clientSecret = GetConfigString(platformConfig.Config, "appSecret", "clientSecret") ?? "";

        var payload = JsonSerializer.Serialize(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["code"] = code,
            ["redirect_uri"] = redirectUri
        });

        using var content = new StringContent(payload, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync(metadata.TokenUrlTemplate, content);
        var body = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(body);
        var root = doc.RootElement;

        // 飞书风格错误码：code != 0 即失败；通用兜底：无 access_token 也视为失败
        if (root.TryGetProperty("code", out var codeEl) && codeEl.ValueKind == JsonValueKind.Number && codeEl.GetInt32() != 0)
        {
            var errMsg = root.TryGetProperty("error_description", out var descEl) ? descEl.GetString()
                : root.TryGetProperty("msg", out var msgEl) ? msgEl.GetString()
                : $"错误码 {codeEl.GetInt32()}";
            return new TokenResult(false, $"平台返回错误: {errMsg}");
        }

        if (!root.TryGetProperty("access_token", out var tokenEl) || string.IsNullOrEmpty(tokenEl.GetString()))
        {
            return new TokenResult(false, $"响应中未找到 access_token: {Truncate(body, 200)}");
        }

        var refreshToken = root.TryGetProperty("refresh_token", out var rtEl) ? rtEl.GetString() : null;
        long expiresIn = root.TryGetProperty("expires_in", out var expEl) && expEl.ValueKind == JsonValueKind.Number
            ? expEl.GetInt64()
            : 7200;
        return new TokenResult(true, "授权成功", tokenEl.GetString(), refreshToken, expiresIn);
    }

    private void MarkFailed(string platform, PendingAuth? pending, string message)
    {
        lock (_lock)
        {
            if (pending != null && pending.Status == "pending")
            {
                pending.Status = "failed";
                pending.Cts.Cancel();
            }
        }
        PushStatus(platform, "failed", message);
    }

    /// <summary>
    /// 经共享 SSE 推送 im_auth_status 事件（负载：platform/status/message）。
    /// 本地单用户场景使用 SendToAllAsync；provider 不可用时静默跳过（前端有 /status 轮询兜底）。
    /// </summary>
    private static void PushStatus(string platform, string status, string message)
    {
        try
        {
            var provider = ServiceLocator.Instance.Get<IIMProvider>() as WebUIProvider;
            if (provider != null)
            {
                _ = provider.SSEHandler.SendToAllAsync("im_auth_status", new { platform, status, message });
            }
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to push im_auth_status via SSE: {0}", ex.Message);
        }
    }

    private static IMPlatformConfig? FindPlatformConfig(string platform)
    {
        return Config.Instance?.Data.IMPlatforms.FirstOrDefault(p =>
            string.Equals(p.Platform, platform, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>回调基础地址：Config.redirectBaseUrl 优先，缺省 http://localhost:{WebPort}。</summary>
    private static string GetRedirectBaseUrl(IMPlatformConfig platformConfig)
    {
        var custom = GetConfigString(platformConfig.Config, "redirectBaseUrl");
        if (!string.IsNullOrEmpty(custom))
        {
            return custom;
        }
        var port = (Config.Instance?.Data as AppConfigData)?.WebPort ?? 8080;
        return $"http://localhost:{port}";
    }

    /// <summary>按候选键顺序取字符串配置值（兼容 JSON 反序列化产生的 JsonElement）。</summary>
    private static string? GetConfigString(Dictionary<string, object> config, params string[] keys)
    {
        foreach (var key in keys)
        {
            if (!config.TryGetValue(key, out var value) || value == null)
            {
                continue;
            }
            var str = value is JsonElement je
                ? (je.ValueKind == JsonValueKind.String ? je.GetString() : je.ToString())
                : value.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                return str;
            }
        }
        return null;
    }

    private static string Truncate(string value, int maxLength)
    {
        return value.Length <= maxLength ? value : value[..maxLength] + "…";
    }
}
