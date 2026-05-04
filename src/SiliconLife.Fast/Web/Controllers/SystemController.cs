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

using System.Net;
using SiliconLife.Collective;

namespace SiliconLife.Fast.Web;

/// <summary>
/// System-level control endpoints (graceful shutdown, etc.).
/// Only accessible from localhost for safety.
/// </summary>
public class SystemController : Controller
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<SystemController>();

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? string.Empty;
        var method = Request.HttpMethod;

        if (path == "/api/system/shutdown" && method == "POST")
        {
            HandleShutdown();
        }
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void HandleShutdown()
    {
        try
        {
            // Safety: only allow shutdown requests from localhost
            var remoteAddr = Request.RemoteEndPoint?.Address;
            if (remoteAddr == null || !IPAddress.IsLoopback(remoteAddr))
            {
                _logger.Warn(null, "Shutdown request blocked from non-loopback address: {0}", remoteAddr?.ToString() ?? "<unknown>");
                RenderJson(new { error = "Forbidden: only localhost allowed" }, 403);
                return;
            }

            _logger.Info(null, "Graceful shutdown requested via /api/system/shutdown");

            // Respond first, then trigger shutdown after a short delay so the
            // HTTP response can be flushed before the listener shuts down.
            RenderJson(new
            {
                status = "shutting_down",
                message = "Application is shutting down gracefully"
            }, 200);

            // Fire-and-forget: let the caller receive the response, then exit.
            _ = Task.Run(async () =>
            {
                await Task.Delay(200);
                Program.RequestExit();
            });
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to process shutdown request", ex);
            try { RenderJson(new { error = ex.Message }, 500); } catch { }
        }
    }
}
