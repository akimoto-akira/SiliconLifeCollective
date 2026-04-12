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
public class BeingController : Controller
{
    private readonly SiliconBeingManager _beingManager;
    private readonly SkinManager _skinManager;

    public BeingController()
    {
        var locator = ServiceLocator.Instance;
        _beingManager = locator.BeingManager!;
        _skinManager = locator.GetService<SkinManager>()!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/beings";

        if (path == "/beings" || path == "/beings/index")
            Index();
        else if (path == "/api/beings/list")
            GetList();
        else if (path == "/api/beings/detail")
            GetDetail();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.BeingView();
        var vm = new Models.BeingViewModel { Skin = skin, ActiveMenu = "beings" };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetList()
    {
        var beings = _beingManager.GetAllBeings();
        var list = beings.Select(b => new
        {
            id = b.Id.ToString(),
            name = b.Name,
            isIdle = b.IsIdle,
            isCustomCompiled = b.IsCustomCompiled,
            customTypeName = b.CustomTypeName ?? ""
        }).ToList();

        RenderJson(list);
    }

    private void GetDetail()
    {
        var idStr = Request.QueryString["id"];
        if (string.IsNullOrEmpty(idStr) || !Guid.TryParse(idStr, out var id))
        {
            RenderJson(new { error = "Invalid ID" });
            return;
        }

        var being = _beingManager.GetBeing(id);
        if (being == null)
        {
            RenderJson(new { error = "Not found" });
            return;
        }

        RenderJson(new
        {
            id = being.Id.ToString(),
            name = being.Name,
            isIdle = being.IsIdle,
            isCustomCompiled = being.IsCustomCompiled,
            customTypeName = being.CustomTypeName ?? "",
            soulContent = being.SoulContent ?? "",
            timerCount = being.TimerSystem?.Count ?? 0,
            taskCount = being.TaskSystem?.Count ?? 0
        });
    }
}
