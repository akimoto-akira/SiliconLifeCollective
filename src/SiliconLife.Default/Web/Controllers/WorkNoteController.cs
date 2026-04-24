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

using SiliconLife.Collective;

namespace SiliconLife.Default.Web;

[WebCode]
public class WorkNoteController : Controller
{
    private readonly SkinManager _skinManager;
    private readonly SiliconBeingManager _beingManager;

    public WorkNoteController()
    {
        _skinManager = ServiceLocator.Instance.GetService<SkinManager>()!;
        _beingManager = ServiceLocator.Instance.BeingManager!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/work-notes";

        if (path == "/work-notes" || path == "/work-notes/index")
            Index();
        else if (path == "/api/work-notes/list")
            GetList();
        else if (path == "/api/work-notes/read")
            ReadNote();
        else if (path == "/api/work-notes/directory")
            GetDirectory();
        else if (path == "/api/work-notes/search")
            Search();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var beingId = GetQueryValue("beingId");
        if (string.IsNullOrWhiteSpace(beingId) || !Guid.TryParse(beingId, out Guid beingGuid))
        {
            Response.StatusCode = 400;
            RenderJson(new { error = "Missing or invalid beingId parameter" });
            return;
        }

        var being = _beingManager.GetBeing(beingGuid);
        if (being == null)
        {
            Response.StatusCode = 404;
            RenderJson(new { error = "Silicon being not found" });
            return;
        }

        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.WorkNoteView();
        var vm = new Models.WorkNoteViewModel
        {
            Skin = skin,
            ActiveMenu = "work-notes",
            BeingId = beingGuid,
            BeingName = being.Name,
            TotalPages = being.WorkNoteSystem?.PageCount ?? 0
        };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetList()
    {
        try
        {
            var beingId = GetQueryValue("beingId");
            if (string.IsNullOrWhiteSpace(beingId) || !Guid.TryParse(beingId, out Guid beingGuid))
            {
                RenderJson(new { success = false, error = "Missing or invalid beingId parameter", data = new List<object>() });
                return;
            }

            var being = _beingManager.GetBeing(beingGuid);
            if (being?.WorkNoteSystem == null)
            {
                RenderJson(new { success = false, error = "Work note system not available", data = new List<object>() });
                return;
            }

            var notes = being.WorkNoteSystem.ListNotes();
            var noteList = notes.Select(n => new
            {
                id = n.Id,
                pageNumber = n.PageNumber,
                summary = n.Summary,
                keywords = n.Keywords,
                version = n.Version,
                createdAt = n.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                updatedAt = n.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();

            RenderJson(new { success = true, data = noteList, total = noteList.Count });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message, data = new List<object>() });
        }
    }

    private void ReadNote()
    {
        try
        {
            var beingId = GetQueryValue("beingId");
            var pageNumStr = GetQueryValue("pageNumber");

            if (string.IsNullOrWhiteSpace(beingId) || !Guid.TryParse(beingId, out Guid beingGuid))
            {
                RenderJson(new { success = false, error = "Missing or invalid beingId parameter" });
                return;
            }

            if (string.IsNullOrWhiteSpace(pageNumStr) || !int.TryParse(pageNumStr, out int pageNum))
            {
                RenderJson(new { success = false, error = "Missing or invalid pageNumber parameter" });
                return;
            }

            var being = _beingManager.GetBeing(beingGuid);
            if (being?.WorkNoteSystem == null)
            {
                RenderJson(new { success = false, error = "Work note system not available" });
                return;
            }

            var note = being.WorkNoteSystem.ReadNote(pageNum);
            if (note == null)
            {
                RenderJson(new { success = false, error = $"Note page {pageNum} not found" });
                return;
            }

            RenderJson(new
            {
                success = true,
                data = new
                {
                    id = note.Id,
                    pageNumber = note.PageNumber,
                    summary = note.Summary,
                    content = note.Content,
                    keywords = note.Keywords,
                    version = note.Version,
                    createdAt = note.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    updatedAt = note.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                }
            });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void GetDirectory()
    {
        try
        {
            var beingId = GetQueryValue("beingId");
            if (string.IsNullOrWhiteSpace(beingId) || !Guid.TryParse(beingId, out Guid beingGuid))
            {
                RenderJson(new { error = "Missing or invalid beingId parameter" });
                return;
            }

            var being = _beingManager.GetBeing(beingGuid);
            if (being?.WorkNoteSystem == null)
            {
                RenderJson(new { error = "Work note system not available" });
                return;
            }

            var directory = being.WorkNoteSystem.GenerateDirectory();
            RenderJson(new { success = true, data = directory });
        }
        catch (Exception ex)
        {
            RenderJson(new { error = ex.Message });
        }
    }

    private void Search()
    {
        try
        {
            var beingId = GetQueryValue("beingId");
            var keyword = GetQueryValue("keyword");
            var maxResultsStr = GetQueryValue("maxResults");
            if (string.IsNullOrWhiteSpace(maxResultsStr)) maxResultsStr = "0";

            if (string.IsNullOrWhiteSpace(beingId) || !Guid.TryParse(beingId, out Guid beingGuid))
            {
                RenderJson(new { error = "Missing or invalid beingId parameter", data = new List<object>() });
                return;
            }

            if (string.IsNullOrWhiteSpace(keyword))
            {
                RenderJson(new { error = "Missing keyword parameter", data = new List<object>() });
                return;
            }

            int maxResults = int.TryParse(maxResultsStr, out int max) ? max : 0;

            var being = _beingManager.GetBeing(beingGuid);
            if (being?.WorkNoteSystem == null)
            {
                RenderJson(new { error = "Work note system not available", data = new List<object>() });
                return;
            }

            var results = being.WorkNoteSystem.SearchNotes(keyword, maxResults);
            var resultList = results.Select(n => new
            {
                id = n.Id,
                pageNumber = n.PageNumber,
                summary = n.Summary,
                keywords = n.Keywords,
                version = n.Version,
                updatedAt = n.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();

            RenderJson(new { success = true, data = resultList, total = resultList.Count });
        }
        catch (Exception ex)
        {
            RenderJson(new { error = ex.Message, data = new List<object>() });
        }
    }
}
