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
public class TaskController : Controller
{
    private readonly SiliconBeingManager _beingManager;
    private readonly SkinManager _skinManager;

    public TaskController()
    {
        var locator = ServiceLocator.Instance;
        _beingManager = locator.BeingManager!;
        _skinManager = locator.GetService<SkinManager>()!;
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/tasks";

        if (path == "/tasks" || path == "/tasks/index")
            Index();
        else if (path == "/api/tasks/list")
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
        var view = new Views.TaskView();
        var vm = new Models.TaskViewModel { Skin = skin, ActiveMenu = "tasks" };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetList()
    {
        var beingIdStr = Request.QueryString["beingId"];
        List<object> allTasks = new();

        if (!string.IsNullOrEmpty(beingIdStr) && Guid.TryParse(beingIdStr, out var beingId))
        {
            var being = _beingManager.GetBeing(beingId);
            if (being?.TaskSystem != null)
            {
                allTasks.AddRange(GetTasksFromSystem(being.TaskSystem, being.Name));
            }
        }
        else
        {
            var beings = _beingManager.GetAllBeings();
            foreach (var being in beings)
            {
                if (being.TaskSystem != null)
                {
                    allTasks.AddRange(GetTasksFromSystem(being.TaskSystem, being.Name));
                }
            }
        }

        RenderJson(allTasks);
    }

    private static List<object> GetTasksFromSystem(TaskSystem taskSystem, string beingName)
    {
        var tasks = taskSystem.GetAll();
        var result = new List<object>();

        foreach (var task in tasks)
        {
            result.Add(new
            {
                id = task.Id.ToString(),
                name = task.Title,
                description = task.Description ?? "",
                status = task.Status.ToString().ToLowerInvariant(),
                priority = task.Priority,
                createdAt = task.CreatedAt,
                createdAtFormatted = task.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                startedAt = task.StartedAt,
                completedAt = task.CompletedAt,
                assignedTo = beingName,
                errorMessage = task.ErrorMessage ?? ""
            });
        }

        return result;
    }
}
