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

namespace SiliconLife.Default.Web;

[WebCode]
public class MemoryController : Controller
{
    private readonly SkinManager _skinManager;

    public MemoryController(SkinManager skinManager)
    {
        _skinManager = skinManager;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/memory";

        if (path == "/memory" || path == "/memory/index")
            Index();
        else if (path == "/api/memory/list")
            GetList();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.MemoryView();
        var vm = new Models.MemoryViewModel { Skin = skin, ActiveMenu = "memory" };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetList()
    {
        RenderJson(new List<object>());
    }
}
