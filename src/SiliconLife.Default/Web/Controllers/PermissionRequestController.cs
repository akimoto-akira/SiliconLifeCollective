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

    public PermissionRequestController(Func<Guid, TaskCompletionSource<AskPermissionResult>> getPermissionTcs)
    {
        _getPermissionTcs = getPermissionTcs;
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

        var html = HtmlBuilder.Create()
            .DocType()
            .Html()
            .Head()
                .MetaCharset()
                .MetaViewport()
                .Title("权限请求 - Silicon Life Collective")
                .Style(GetStyles())
                .Script(GetScripts(userId.ToString()))
            .EndBlock()
            .Body()
                .Div()
                    .Class("permission-container")
                    .Div()
                        .Class("permission-box")
                        .H1("权限请求")
                        .P("一个硅基人请求您的授权：")
                        .Raw($@"
                            <div class=""permission-details"">
                                <div class=""detail-row"">
                                    <span class=""label"">权限类型:</span>
                                    <span class=""value"">{permissionType}</span>
                                </div>
                                <div class=""detail-row"">
                                    <span class=""label"">请求资源:</span>
                                    <span class=""value"">{resource}</span>
                                </div>
                            </div>
                            <div class=""permission-buttons"">
                                <button class=""btn-allow"" onclick=""respond(true)"">允许</button>
                                <button class=""btn-deny"" onclick=""respond(false)"">拒绝</button>
                            </div>
                            <div class=""auto-close"">等待响应...</div>
                        ")
                    .EndBlock()
                .EndBlock()
            .EndBlock()
            .Build();

        RenderHtml(html);
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

        if (!Guid.TryParse(userIdStr, out var userId) || !bool.TryParse(allowedStr, out var allowed))
        {
            Response.StatusCode = 400;
            RenderText("Invalid parameters");
            return;
        }

        var tcs = _getPermissionTcs(userId);
        if (tcs != null)
        {
            tcs.SetResult(new AskPermissionResult { Allowed = allowed });
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
            .auto-close { font-size: 12px; color: #999; }
        ";
    }

    private string GetScripts(string userId)
    {
        return $@"
            function respond(allowed) {{
                fetch('/permission/respond?userId={userId}&allowed=' + allowed)
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
