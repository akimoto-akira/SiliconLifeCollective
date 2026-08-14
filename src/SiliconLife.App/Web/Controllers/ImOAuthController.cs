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

using System.Diagnostics;
using System.Net;

using SiliconLife.Collective;

using SiliconLife.Common.Localization;

namespace SiliconLife.App.Web.Controllers;

/// <summary>
/// IM 平台 OAuth 授权向导控制器：
/// GET /im/{platform}/authorize — 发起授权并打开系统浏览器；
/// GET /im/{platform}/callback  — 接收授权回调（code+state），换 token 并保存；
/// GET /im/{platform}/status    — 查询授权状态（前端轮询兜底）。
/// </summary>
[WebCode]
public class ImOAuthController : Controller
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<ImOAuthController>();
    private readonly DefaultLocalizationBase _loc;

    public ImOAuthController()
    {
        _loc = (DefaultLocalizationBase)LocalizationManager.Instance.GetLocalization(Config.Instance!.Data.Language);
    }

    public override void Handle()
    {
        var platform = Parameters.TryGetValue("platform", out var p) ? p : string.Empty;
        if (string.IsNullOrEmpty(platform))
        {
            RenderJson(new { success = false, message = "缺少平台参数" }, 400);
            return;
        }

        var path = Request.Url?.AbsolutePath ?? string.Empty;
        if (path.EndsWith("/authorize", StringComparison.OrdinalIgnoreCase))
        {
            Authorize(platform);
        }
        else if (path.EndsWith("/callback", StringComparison.OrdinalIgnoreCase))
        {
            Callback(platform);
        }
        else if (path.EndsWith("/status", StringComparison.OrdinalIgnoreCase))
        {
            Status(platform);
        }
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Authorize(string platform)
    {
        var (success, message, url) = ImOAuthService.Instance.StartAuthorization(platform);
        if (!success || string.IsNullOrEmpty(url))
        {
            RenderJson(new { success = false, message });
            return;
        }

        try
        {
            // 打开系统默认浏览器完成授权（本地优先方案，无需二维码库）
            Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to open system browser for IM OAuth: {0}", ex.Message);
            RenderJson(new { success = false, message = $"无法打开系统浏览器: {ex.Message}" });
            return;
        }

        RenderJson(new { success = true, message });
    }

    private void Callback(string platform)
    {
        var code = GetQueryValue("code");
        var state = GetQueryValue("state");

        var (success, message) = ImOAuthService.Instance
            .HandleCallbackAsync(platform, code, state)
            .GetAwaiter()
            .GetResult();

        if (success)
        {
            var title = _loc.GetConfigDisplayName("IMAuthorizedStatus", out _);
            RenderHtml(BuildResultPage(title, "授权成功，可关闭此页面", "#2e9e5b"));
        }
        else
        {
            var title = _loc.GetConfigDisplayName("IMAuthFailedStatus", out _);
            RenderHtml(BuildResultPage(title, WebUtility.HtmlEncode(message), "#dc3545"));
        }
    }

    private void Status(string platform)
    {
        RenderJson(ImOAuthService.Instance.GetStatus(platform));
    }

    /// <summary>简洁的授权结果页（回调落地页，浏览器内显示后即可关闭）。</summary>
    private static string BuildResultPage(string title, string message, string accentColor)
    {
        return "<!DOCTYPE html><html><head><meta charset=\"utf-8\"><title>" + WebUtility.HtmlEncode(title) + "</title>" +
               "<style>body{font-family:'Segoe UI','Microsoft YaHei',sans-serif;display:flex;align-items:center;justify-content:center;height:100vh;margin:0;background:#f5f6f8;}" +
               ".box{text-align:center;background:#fff;padding:40px 56px;border-radius:12px;box-shadow:0 4px 20px rgba(0,0,0,0.08);}" +
               ".box h2{margin:0 0 12px 0;color:" + accentColor + ";}" +
               ".box p{margin:0;color:#555;}</style></head>" +
               "<body><div class=\"box\"><h2>" + WebUtility.HtmlEncode(title) + "</h2><p>" + message + "</p></div></body></html>";
    }
}
