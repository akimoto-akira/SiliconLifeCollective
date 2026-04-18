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

using System.Text.Json;
using SiliconLife.Collective;

namespace SiliconLife.Default.Web;

[WebCode]
public class PermissionRequestController : Controller
{
    private readonly Func<Guid, TaskCompletionSource<AskPermissionResult>> _getPermissionTcs;

    public PermissionRequestController()
    {
        _getPermissionTcs = ServiceLocator.Instance.Get<Func<Guid, TaskCompletionSource<AskPermissionResult>>>()!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/permission/request";
        
        if (path == "/permission/request" || path == "/permission/request/index")
        {
            Index();
        }
        else if (path == "/permission/check")
        {
            CheckPending();
        }
        else if (path == "/permission/respond")
        {
            Respond();
        }
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var userIdStr = Request.QueryString["userId"];
        var permissionType = Request.QueryString["type"] ?? "Unknown";
        var resource = Request.QueryString["resource"] ?? "/";
        var allowCode = Request.QueryString["allowCode"] ?? "";
        var denyCode = Request.QueryString["denyCode"] ?? "";

        if (!Guid.TryParse(userIdStr, out var userId))
        {
            Response.StatusCode = 400;
            RenderText("Invalid user ID");
            return;
        }

        // 获取本地化服务
        var loc = ServiceLocator.Instance.Get<DefaultLocalizationBase>()!;

        var html = H.PageElement($"{loc.PermissionRequestHeader} - Silicon Life Collective",
            new object[]
            {
                H.Style(GetStyles()),
                H.Script(GetScripts(userId.ToString())),
            },
            new object[]
            {
                H.Div(
                    H.Div(
                        H.H1(loc.PermissionRequestHeader),
                        H.P(loc.PermissionRequestDescription),
                        H.Div(
                            H.Div(
                                H.Div(
                                    H.Span(loc.PermissionRequestTypeLabel).Class("label"),
                                    H.Span(permissionType).Class("value")
                                ).Class("detail-row"),
                                H.Div(
                                    H.Span(loc.PermissionRequestResourceLabel).Class("label"),
                                    H.Span(resource).Class("value")
                                ).Class("detail-row")
                            ).Class("permission-details"),
                            H.Div(
                                H.Button(loc.PermissionRequestAllowButton).Class("btn-allow").OnClick("respond(true)"),
                                H.Button(loc.PermissionRequestDenyButton).Class("btn-deny").OnClick("respond(false)")
                            ).Class("permission-buttons"),
                            H.Div(
                                H.Input().Attr("type", "checkbox").Id("cache-checkbox").Class("cache-checkbox"),
                                H.Label(loc.PermissionRequestCacheLabel).Attr("for", "cache-checkbox").Class("cache-label")
                            ).Class("cache-row"),
                            H.Div(
                                H.Label(loc.PermissionRequestDurationLabel).Attr("for", "cache-duration").Class("duration-label"),
                                H.Select(
                                    H.Option(loc.PermissionCacheDuration1Hour).Attr("value", "1"),
                                    H.Option(loc.PermissionCacheDuration24Hours).Attr("value", "24"),
                                    H.Option(loc.PermissionCacheDuration7Days).Attr("value", "168"),
                                    H.Option(loc.PermissionCacheDuration30Days).Attr("value", "720")
                                ).Id("cache-duration").Class("duration-select")
                            ).Class("duration-row"),
                            H.Div(loc.PermissionRequestWaitingMessage).Class("auto-close")
                        )
                    ).Class("permission-box")
                ).Class("permission-container"),
            });

        RenderHtml(html.Build());
    }

    private void CheckPending()
    {
        var userIdStr = Request.QueryString["userId"];
        if (!Guid.TryParse(userIdStr, out var userId))
        {
            RenderJson(new { pending = false });
            return;
        }

        var hasPending = _getPermissionTcs(userId) != null;
        RenderJson(new { pending = hasPending });
    }

    private void Respond()
    {
        var userIdStr = Request.QueryString["userId"];
        var allowedStr = Request.QueryString["allowed"];
        var addToCacheStr = Request.QueryString["addToCache"];
        var cacheDurationStr = Request.QueryString["cacheDuration"];

        if (!Guid.TryParse(userIdStr, out var userId) || !bool.TryParse(allowedStr, out var allowed))
        {
            Response.StatusCode = 400;
            RenderText("Invalid parameters");
            return;
        }

        bool addToCache = false;
        if (!string.IsNullOrEmpty(addToCacheStr))
        {
            bool.TryParse(addToCacheStr, out addToCache);
        }

        TimeSpan? cacheDuration = null;
        if (addToCache && !string.IsNullOrEmpty(cacheDurationStr) && double.TryParse(cacheDurationStr, out double hours))
        {
            cacheDuration = TimeSpan.FromHours(hours);
        }

        var tcs = _getPermissionTcs(userId);
        if (tcs != null)
        {
            tcs.SetResult(new AskPermissionResult { Allowed = allowed, AddToCache = addToCache, CacheDuration = cacheDuration });
        }

        RenderJson(new { success = true });
    }

    private string GetStyles()
    {
        return @"
            * { box-sizing: border-box; margin: 0; padding: 0; }
            body { font-family: Arial, sans-serif; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); min-height: 100vh; display: flex; align-items: center; justify-content: center; }
            .permission-container { width: 100%; max-width: 500px; padding: 20px; }
            .permission-box { background: white; padding: 40px; border-radius: 16px; box-shadow: 0 10px 40px rgba(0,0,0,0.2); text-align: center; }
            .permission-box h1 { font-size: 28px; color: #333; margin-bottom: 20px; }
            .permission-box p { font-size: 16px; color: #666; margin-bottom: 20px; }
            .permission-details { background: #f8f9fa; padding: 20px; border-radius: 8px; margin-bottom: 30px; }
            .detail-row { display: flex; justify-content: space-between; padding: 10px 0; border-bottom: 1px solid #eee; }
            .detail-row:last-child { border-bottom: none; }
            .label { font-weight: bold; color: #666; }
            .value { color: #333; }
            .permission-buttons { display: flex; gap: 15px; justify-content: center; margin-bottom: 20px; }
            .btn-allow, .btn-deny { padding: 12px 40px; font-size: 16px; border: none; border-radius: 8px; cursor: pointer; transition: all 0.3s; }
            .btn-allow { background: #4CAF50; color: white; }
            .btn-allow:hover { background: #45a049; transform: translateY(-2px); }
            .btn-deny { background: #f44336; color: white; }
            .btn-deny:hover { background: #da190b; transform: translateY(-2px); }
            .cache-row { display: flex; align-items: center; gap: 6px; justify-content: center; margin-bottom: 20px; }
            .cache-checkbox { cursor: pointer; accent-color: #4CAF50; width: 16px; height: 16px; }
            .cache-label { font-size: 14px; color: #666; cursor: pointer; user-select: none; }
            .duration-row { display: none; align-items: center; gap: 8px; justify-content: center; margin-bottom: 20px; }
            .duration-label { font-size: 14px; color: #666; }
            .duration-select { font-size: 14px; padding: 4px 8px; border: 1px solid #ddd; border-radius: 4px; cursor: pointer; }
            .auto-close { font-size: 12px; color: #999; }
        ";
    }

    private string GetScripts(string userId)
    {
        return $@"
            document.getElementById('cache-checkbox').addEventListener('change', function() {{
                document.getElementById('duration-row').style.display = this.checked ? 'flex' : 'none';
            }});

            function respond(allowed) {{
                var addToCache = document.getElementById('cache-checkbox').checked;
                var cacheDuration = document.getElementById('cache-duration').value;
                fetch('/permission/respond?userId={userId}&allowed=' + allowed + '&addToCache=' + addToCache + '&cacheDuration=' + cacheDuration)
                    .then(r => r.json())
                    .then(data => {{
                        if (data.success) {{
                            window.close();
                        }}
                    }});
            }}

            setTimeout(function() {{
                window.close();
            }}, 60000);
        ";
    }
}
