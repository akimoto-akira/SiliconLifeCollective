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

using SiliconLife.Collective;

namespace SiliconLife.Default.Web;

[WebCode]
public class ExecutorController : Controller
{
    private readonly SkinManager _skinManager;

    public ExecutorController()
    {
        _skinManager = ServiceLocator.Instance.GetService<SkinManager>()!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/executors";

        if (path == "/executors" || path == "/executors/index")
            Index();
        else if (path == "/api/executors/status")
            GetStatus();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.ExecutorView();
        var vm = new Models.ExecutorViewModel { Skin = skin };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetStatus()
    {
        var status = new[]
        {
            new { name = "DiskExecutor", status = "Idle", queueCount = 0 },
            new { name = "NetworkExecutor", status = "Idle", queueCount = 0 },
            new { name = "CommandLineExecutor", status = "Idle", queueCount = 0 }
        };

        RenderJson(status);
    }
}
