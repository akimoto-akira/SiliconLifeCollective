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

        // Get localization service
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
        var css = CssBuilder.Create()
            // Global reset
            .Selector("*")
                .Property("box-sizing", "border-box")
                .Property("margin", "0")
                .Property("padding", "0")
            .EndSelector()
            // Body styles
            .Selector("body")
                .Property("font-family", "Arial, sans-serif")
                .Property("background", "linear-gradient(135deg, #667eea 0%, #764ba2 100%)")
                .Property("min-height", "100vh")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("justify-content", "center")
            .EndSelector()
            // Permission container
            .Selector(".permission-container")
                .Property("width", "100%")
                .Property("max-width", "500px")
                .Property("padding", "20px")
            .EndSelector()
            // Permission box
            .Selector(".permission-box")
                .Property("background", "white")
                .Property("padding", "40px")
                .Property("border-radius", "16px")
                .Property("box-shadow", "0 10px 40px rgba(0,0,0,0.2)")
                .Property("text-align", "center")
            .EndSelector()
            .Selector(".permission-box h1")
                .Property("font-size", "28px")
                .Property("color", "#333")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".permission-box p")
                .Property("font-size", "16px")
                .Property("color", "#666")
                .Property("margin-bottom", "20px")
            .EndSelector()
            // Permission details
            .Selector(".permission-details")
                .Property("background", "#f8f9fa")
                .Property("padding", "20px")
                .Property("border-radius", "8px")
                .Property("margin-bottom", "30px")
            .EndSelector()
            // Detail rows
            .Selector(".detail-row")
                .Property("display", "flex")
                .Property("justify-content", "space-between")
                .Property("padding", "10px 0")
                .Property("border-bottom", "1px solid #eee")
            .EndSelector()
            .Selector(".detail-row:last-child")
                .Property("border-bottom", "none")
            .EndSelector()
            .Selector(".label")
                .Property("font-weight", "bold")
                .Property("color", "#666")
            .EndSelector()
            .Selector(".value")
                .Property("color", "#333")
            .EndSelector()
            // Buttons
            .Selector(".permission-buttons")
                .Property("display", "flex")
                .Property("gap", "15px")
                .Property("justify-content", "center")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".btn-allow, .btn-deny")
                .Property("padding", "12px 40px")
                .Property("font-size", "16px")
                .Property("border", "none")
                .Property("border-radius", "8px")
                .Property("cursor", "pointer")
                .Property("transition", "all 0.3s")
            .EndSelector()
            .Selector(".btn-allow")
                .Property("background", "#4CAF50")
                .Property("color", "white")
            .EndSelector()
            .Selector(".btn-allow:hover")
                .Property("background", "#45a049")
                .Property("transform", "translateY(-2px)")
            .EndSelector()
            .Selector(".btn-deny")
                .Property("background", "#f44336")
                .Property("color", "white")
            .EndSelector()
            .Selector(".btn-deny:hover")
                .Property("background", "#da190b")
                .Property("transform", "translateY(-2px)")
            .EndSelector()
            // Cache row
            .Selector(".cache-row")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "6px")
                .Property("justify-content", "center")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".cache-checkbox")
                .Property("cursor", "pointer")
                .Property("accent-color", "#4CAF50")
                .Property("width", "16px")
                .Property("height", "16px")
            .EndSelector()
            .Selector(".cache-label")
                .Property("font-size", "14px")
                .Property("color", "#666")
                .Property("cursor", "pointer")
                .Property("user-select", "none")
            .EndSelector()
            // Duration row
            .Selector(".duration-row")
                .Property("display", "none")
                .Property("align-items", "center")
                .Property("gap", "8px")
                .Property("justify-content", "center")
                .Property("margin-bottom", "20px")
            .EndSelector()
            .Selector(".duration-label")
                .Property("font-size", "14px")
                .Property("color", "#666")
            .EndSelector()
            .Selector(".duration-select")
                .Property("font-size", "14px")
                .Property("padding", "4px 8px")
                .Property("border", "1px solid #ddd")
                .Property("border-radius", "4px")
                .Property("cursor", "pointer")
            .EndSelector()
            // Auto close message
            .Selector(".auto-close")
                .Property("font-size", "12px")
                .Property("color", "#999")
            .EndSelector();

        return css.Build();
    }

    private string GetScripts(string userId)
    {
        var jsBlock = Js.Block()
            // Cache checkbox change event listener
            .Add(() => Js.Invoke(
                () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "cache-checkbox"))
                    .Call(() => "addEventListener", () => Js.Str(() => "change"),
                        () => Js.Arrow(
                            () => new List<string>(),
                            () => Js.Block()
                                .Add(() => Js.Assign(
                                    () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "duration-row")).Prop(() => "style").Prop(() => "display"),
                                    () => Js.Ternary(
                                        () => Js.Id(() => "this").Prop(() => "checked"),
                                        () => Js.Str(() => "flex"),
                                        () => Js.Str(() => "none")
                                    )
                                ))
                        )
                    )
            ))
            // respond function
            .Add(() => Js.Func(
                () => "respond",
                () => new List<string> { "allowed" },
                () => Js.Block()
                    .Add(() => Js.Const(
                        () => "addToCache",
                        () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "cache-checkbox")).Prop(() => "checked")
                    ))
                    .Add(() => Js.Const(
                        () => "cacheDuration",
                        () => Js.Id(() => "document").Call(() => "getElementById", () => Js.Str(() => "cache-duration")).Prop(() => "value")
                    ))
                    .Add(() => Js.Expr(
                        () => Js.Invoke(
                            () => Js.Id(() => "fetch"),
                            () => Js.Op(
                                () => Js.Op(
                                    () => Js.Op(
                                        () => Js.Op(
                                            () => Js.Str(() => $"/permission/respond?userId={userId}&allowed="),
                                            () => "+",
                                            () => Js.Id(() => "allowed")
                                        ),
                                        () => "+",
                                        () => Js.Str(() => "&addToCache=")
                                    ),
                                    () => "+",
                                    () => Js.Id(() => "addToCache")
                                ),
                                () => "+",
                                () => Js.Op(
                                    () => Js.Str(() => "&cacheDuration="),
                                    () => "+",
                                    () => Js.Id(() => "cacheDuration")
                                )
                            )
                        )
                        .Call(() => "then",
                            () => Js.Arrow(
                                () => new List<string> { "r" },
                                () => Js.Id(() => "r").Call(() => "json")
                            )
                        )
                        .Call(() => "then",
                            () => Js.Arrow(
                                () => new List<string> { "data" },
                                () => Js.If(
                                    () => new List<(JsSyntax? Condition, List<JsSyntax> Body)>
                                    {
                                        (
                                            Js.Id(() => "data").Prop(() => "success"),
                                            new List<JsSyntax>
                                            {
                                                Js.Invoke(() => Js.Id(() => "window").Call(() => "close"))
                                            }
                                        )
                                    }
                                )
                            )
                        )
                    ))
            ))
            // Auto close timeout
            .Add(() => Js.Expr(
                () => Js.Invoke(
                    () => Js.Id(() => "setTimeout"),
                    () => Js.Arrow(
                        () => new List<string>(),
                        () => Js.Invoke(() => Js.Id(() => "window").Call(() => "close"))
                    ),
                    () => Js.Num(() => "60000")
                )
            ));

        return jsBlock.Build();
    }
}
