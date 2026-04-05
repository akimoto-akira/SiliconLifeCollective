// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Diagnostics;
using SiliconLife.Collective;

namespace SiliconLife.Default.Web;

[WebCode]
public class DashboardController : Controller
{
    private readonly SiliconBeingManager _beingManager;
    private readonly ChatSystem _chatSystem;
    private readonly SkinManager _skinManager;

    public DashboardController()
    {
        var locator = ServiceLocator.Instance;
        _beingManager = locator.BeingManager!;
        _chatSystem = locator.ChatSystem!;
        _skinManager = locator.GetService<SkinManager>()!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/dashboard";

        if (path == "/dashboard" || path == "/dashboard/index")
            Index();
        else if (path == "/api/dashboard/stats")
            GetStats();
        else if (path == "/api/dashboard/metrics")
            GetMetrics();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.DashboardView();
        var vm = new Models.DashboardViewModel { Skin = skin, ActiveMenu = "dashboard" };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetStats()
    {
        var beings = _beingManager.GetAllBeings();
        var process = Process.GetCurrentProcess();
        var uptime = DateTime.Now - process.StartTime;

        RenderJson(new
        {
            beingCount = beings.Count,
            activeBeings = beings.Count(b => !b.IsIdle),
            uptime = uptime.ToString(@"hh\:mm\:ss"),
            memoryMB = Math.Round(process.WorkingSet64 / 1024.0 / 1024.0, 2)
        });
    }

    private void GetMetrics()
    {
        RenderJson(new
        {
            timestamps = Enumerable.Range(0, 20).Select(i => DateTime.Now.AddMinutes(-19 + i).ToString("HH:mm")).ToList(),
            messageCounts = Enumerable.Range(0, 20).Select(_ => Random.Shared.Next(0, 100)).ToList()
        });
    }
}
