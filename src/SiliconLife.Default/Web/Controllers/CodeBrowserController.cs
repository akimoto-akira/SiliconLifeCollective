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
public class CodeBrowserController : Controller
{
    private readonly WebCodeBrowser _codeBrowser;
    private readonly SkinManager _skinManager;

    public CodeBrowserController(WebCodeBrowser codeBrowser, SkinManager skinManager)
    {
        _codeBrowser = codeBrowser;
        _skinManager = skinManager;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/code";

        if (path == "/code" || path == "/code/index")
            Index();
        else if (path == "/api/code/types")
            GetTypes();
        else if (path == "/api/code/detail")
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
        var view = new Views.CodeBrowserView();
        var vm = new Models.CodeBrowserViewModel { Skin = skin, ActiveMenu = "beings" };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetTypes()
    {
        var types = _codeBrowser.GetAllTypes();
        RenderJson(types);
    }

    private void GetDetail()
    {
        var fullName = Request.QueryString["name"];
        if (string.IsNullOrEmpty(fullName))
        {
            RenderJson(new { error = "Name required" });
            return;
        }

        var detail = _codeBrowser.GetType(fullName);
        if (detail == null)
        {
            RenderJson(new { error = "Type not found" });
            return;
        }

        RenderJson(detail);
    }
}
