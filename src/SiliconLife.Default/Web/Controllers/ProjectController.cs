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
public class ProjectController : Controller
{
    private readonly SkinManager _skinManager;
    private readonly IProjectManager? _projectManager;

    public ProjectController()
    {
        _skinManager = ServiceLocator.Instance.GetService<SkinManager>()!;
        _projectManager = ServiceLocator.Instance.GetService<IProjectManager>();
    }

    public override void Handle()
    {
        var path = Request.Url?.AbsolutePath ?? "/project";

        if (path == "/project" || path == "/project/index")
            Index();
        else if (path.StartsWith("/project/") && path.EndsWith("/work-notes"))
            ProjectWorkNotesPage();
        else if (path == "/api/projects/list")
            GetList();
        else if (path == "/api/projects/create")
            CreateProject();
        else if (path.StartsWith("/api/projects/") && path.EndsWith("/archive"))
            ArchiveProject();
        else if (path.StartsWith("/api/projects/") && path.EndsWith("/restore"))
            RestoreProject();
        else if (path.StartsWith("/api/projects/") && path.EndsWith("/destroy"))
            DestroyProject();
        else if (path == "/api/projects/detail")
            GetProjectDetail();
        else if (path == "/api/projects/update")
            UpdateProject();
        else if (path == "/api/projects/assign")
            AssignBeing();
        else if (path == "/api/projects/remove")
            RemoveBeing();
        else if (path.StartsWith("/api/projects/") && path.Contains("/work-notes/list"))
            ListProjectWorkNotes();
        else if (path.StartsWith("/api/projects/") && path.Contains("/work-notes/read"))
            ReadProjectWorkNote();
        else if (path.StartsWith("/api/projects/") && path.EndsWith("/work-notes/create"))
            CreateProjectWorkNote();
        else if (path.StartsWith("/api/projects/") && path.EndsWith("/work-notes/update"))
            UpdateProjectWorkNote();
        else if (path.StartsWith("/api/projects/") && path.EndsWith("/work-notes/delete"))
            DeleteProjectWorkNote();
        else if (path.StartsWith("/project/") && path.EndsWith("/tasks"))
            ProjectTasksPage();
        else if (path.StartsWith("/api/projects/") && path.Contains("/tasks/list"))
            ListProjectTasks();
        else if (path.StartsWith("/api/projects/") && path.EndsWith("/tasks/create"))
            CreateProjectTask();
        else if (path.StartsWith("/api/projects/") && path.EndsWith("/tasks/update"))
            UpdateProjectTask();
        else if (path.StartsWith("/api/projects/") && path.EndsWith("/tasks/delete"))
            DeleteProjectTask();
        else if (path.StartsWith("/api/projects/") && path.EndsWith("/tasks/assign"))
            AssignProjectTask();
        else if (path.StartsWith("/api/projects/") && path.EndsWith("/tasks/remove-assignee"))
            RemoveProjectTaskAssignee();
        else if (path.StartsWith("/api/projects/") && path.EndsWith("/tasks/complete"))
            CompleteProjectTask();
        else if (path.StartsWith("/api/projects/") && path.EndsWith("/tasks/fail"))
            FailProjectTask();
        else if (path.StartsWith("/api/projects/") && path.EndsWith("/tasks/cancel"))
            CancelProjectTask();
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new Views.ProjectView();
        var projects = _projectManager?.ListProjects(includeArchived: false) ?? new List<ProjectSpace>();
        var vm = new Models.ProjectViewModel
        {
            Skin = skin,
            ActiveMenu = "projects",
            Projects = projects.Select(p => new Models.ProjectItem
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                Status = p.Status.ToString().ToLowerInvariant()
            }).ToList()
        };
        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void GetList()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available", data = new List<object>() });
                return;
            }

            string includeArchivedStr = GetQueryValue("includeArchived", "false");
            bool includeArchived = bool.TryParse(includeArchivedStr, out bool b) && b;

            var projects = _projectManager.ListProjects(includeArchived);
            var data = projects.Select(p => new
            {
                id = p.Id,
                name = p.Name,
                description = p.Description,
                status = p.Status.ToString().ToLowerInvariant(),
                createdBy = p.CreatedBy,
                createdAt = p.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                updatedAt = p.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                archivedAt = p.ArchivedAt?.ToString("yyyy-MM-dd HH:mm:ss"),
                assignedBeings = p.AssignedBeings,
                beingCount = p.AssignedBeings.Count
            }).ToList();

            RenderJson(new { success = true, data, total = data.Count });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message, data = new List<object>() });
        }
    }

    private void CreateProject()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            var body = GetJsonBody<Dictionary<string, object>>();
            if (body == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            string? name = body.TryGetValue("name", out var nameObj) ? nameObj?.ToString() : null;
            string? description = body.TryGetValue("description", out var descObj) ? descObj?.ToString() : null;

            if (string.IsNullOrWhiteSpace(name))
            {
                RenderJson(new { success = false, error = "Project name is required" });
                return;
            }

            Guid createdBy = Guid.Empty;
            if (body.TryGetValue("createdBy", out var creatorObj) && !Guid.TryParse(creatorObj?.ToString(), out createdBy))
            {
                // Try to use curator GUID from config
                createdBy = Config.Instance?.Data?.CuratorGuid ?? Guid.Empty;
            }

            if (createdBy == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Creator ID is required" });
                return;
            }

            var project = _projectManager.CreateProject(name, description ?? "", createdBy);
            RenderJson(new
            {
                success = true,
                data = new
                {
                    id = project.Id,
                    name = project.Name,
                    description = project.Description,
                    status = project.Status.ToString().ToLowerInvariant(),
                    createdAt = project.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                }
            });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void ArchiveProject()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            Guid projectId = ExtractProjectIdFromPath();
            if (projectId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Invalid project ID" });
                return;
            }

            bool result = _projectManager.ArchiveProject(projectId);
            RenderJson(new { success = result, error = result ? (string?)null : "Project not found or not active" });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void RestoreProject()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            Guid projectId = ExtractProjectIdFromPath();
            if (projectId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Invalid project ID" });
                return;
            }

            bool result = _projectManager.RestoreProject(projectId);
            RenderJson(new { success = result, error = result ? (string?)null : "Project not found or not archived" });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void DestroyProject()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            Guid projectId = ExtractProjectIdFromPath();
            if (projectId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Invalid project ID" });
                return;
            }

            bool result = _projectManager.DestroyProject(projectId);
            RenderJson(new { success = result, error = result ? (string?)null : "Project not found" });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void GetProjectDetail()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            string projectIdStr = GetQueryValue("id");
            if (!Guid.TryParse(projectIdStr, out Guid projectId))
            {
                RenderJson(new { success = false, error = "Missing or invalid project ID" });
                return;
            }

            var project = _projectManager.GetProject(projectId);
            if (project == null)
            {
                RenderJson(new { success = false, error = "Project not found" });
                return;
            }

            RenderJson(new
            {
                success = true,
                data = new
                {
                    id = project.Id,
                    name = project.Name,
                    description = project.Description,
                    status = project.Status.ToString().ToLowerInvariant(),
                    createdBy = project.CreatedBy,
                    createdAt = project.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    updatedAt = project.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    archivedAt = project.ArchivedAt?.ToString("yyyy-MM-dd HH:mm:ss"),
                    assignedBeings = project.AssignedBeings,
                    beingCount = project.AssignedBeings.Count,
                    storagePath = project.StoragePath
                }
            });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void UpdateProject()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            var body = GetJsonBody<Dictionary<string, object>>();
            if (body == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            if (!body.TryGetValue("projectId", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid projectId))
            {
                RenderJson(new { success = false, error = "Missing or invalid projectId" });
                return;
            }

            string? name = body.TryGetValue("name", out var nameObj) ? nameObj?.ToString() : null;
            string? description = body.TryGetValue("description", out var descObj) ? descObj?.ToString() : null;

            var project = _projectManager.UpdateProject(projectId, name, description);
            if (project == null)
            {
                RenderJson(new { success = false, error = "Project not found or not active" });
                return;
            }

            RenderJson(new
            {
                success = true,
                data = new
                {
                    id = project.Id,
                    name = project.Name,
                    description = project.Description,
                    updatedAt = project.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                }
            });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void AssignBeing()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            var body = GetJsonBody<Dictionary<string, object>>();
            if (body == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            if (!body.TryGetValue("projectId", out var pidObj) || !Guid.TryParse(pidObj?.ToString(), out Guid projectId))
            {
                RenderJson(new { success = false, error = "Missing or invalid projectId" });
                return;
            }

            if (!body.TryGetValue("beingId", out var bidObj) || !Guid.TryParse(bidObj?.ToString(), out Guid beingId))
            {
                RenderJson(new { success = false, error = "Missing or invalid beingId" });
                return;
            }

            bool result = _projectManager.AssignBeing(projectId, beingId);
            RenderJson(new { success = result, error = result ? (string?)null : "Project not found" });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void RemoveBeing()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            var body = GetJsonBody<Dictionary<string, object>>();
            if (body == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            if (!body.TryGetValue("projectId", out var pidObj) || !Guid.TryParse(pidObj?.ToString(), out Guid projectId))
            {
                RenderJson(new { success = false, error = "Missing or invalid projectId" });
                return;
            }

            if (!body.TryGetValue("beingId", out var bidObj) || !Guid.TryParse(bidObj?.ToString(), out Guid beingId))
            {
                RenderJson(new { success = false, error = "Missing or invalid beingId" });
                return;
            }

            bool result = _projectManager.RemoveBeing(projectId, beingId);
            RenderJson(new { success = result, error = result ? (string?)null : "Project or being not found" });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private Guid ExtractProjectIdFromPath()
    {
        var path = Request.Url?.AbsolutePath ?? "";
        // Path format: /api/projects/{id}/action or /api/projects/{id}/work-notes/action
        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length >= 3 && segments[0] == "api" && segments[1] == "projects")
        {
            if (Guid.TryParse(segments[2], out Guid projectId))
            {
                return projectId;
            }
        }
        return Guid.Empty;
    }

    private Guid ExtractProjectIdFromPagePath()
    {
        var path = Request.Url?.AbsolutePath ?? "";
        // Path format: /project/{id}/work-notes
        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length >= 2 && segments[0] == "project")
        {
            if (Guid.TryParse(segments[1], out Guid projectId))
            {
                return projectId;
            }
        }
        return Guid.Empty;
    }

    private void ProjectWorkNotesPage()
    {
        try
        {
            if (_projectManager == null)
            {
                Response.StatusCode = 500;
                RenderHtml("<p>Project manager not available</p>");
                return;
            }

            Guid projectId = ExtractProjectIdFromPagePath();
            if (projectId == Guid.Empty)
            {
                Response.StatusCode = 400;
                RenderHtml("<p>Invalid project ID</p>");
                return;
            }

            var project = _projectManager.GetProject(projectId);
            if (project == null)
            {
                Response.StatusCode = 404;
                RenderHtml("<p>Project not found</p>");
                return;
            }

            var workNoteSystem = _projectManager.GetWorkNoteSystem(projectId);
            int pageCount = workNoteSystem?.PageCount ?? 0;

            var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
            var view = new Views.WorkNoteView();
            var vm = new Models.WorkNoteViewModel
            {
                Skin = skin,
                ActiveMenu = "projects",
                BeingId = Guid.Empty,
                ProjectId = projectId,
                ProjectName = project.Name,
                TotalPages = pageCount
            };
            var html = view.Render(vm);
            RenderHtml(html);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            RenderHtml($"<p>Error: {ex.Message}</p>");
        }
    }

    #region Project Work Notes

    private void ListProjectWorkNotes()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available", data = new List<object>() });
                return;
            }

            Guid projectId = ExtractProjectIdFromPath();
            if (projectId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Invalid project ID", data = new List<object>() });
                return;
            }

            var workNoteSystem = _projectManager.GetWorkNoteSystem(projectId);
            if (workNoteSystem == null)
            {
                RenderJson(new { success = false, error = "Project work note system not available", data = new List<object>() });
                return;
            }

            var notes = workNoteSystem.ListNotes();
            var noteList = notes.Select(n => new
            {
                id = n.Id,
                pageNumber = n.PageNumber,
                summary = n.Summary,
                keywords = n.Keywords,
                version = n.Version,
                authorGuid = n.AuthorGuid,
                modifiedByGuid = n.ModifiedByGuid,
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

    private void ReadProjectWorkNote()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            Guid projectId = ExtractProjectIdFromPath();
            if (projectId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Invalid project ID" });
                return;
            }

            var pageNumStr = GetQueryValue("pageNumber");
            if (string.IsNullOrWhiteSpace(pageNumStr) || !int.TryParse(pageNumStr, out int pageNum))
            {
                RenderJson(new { success = false, error = "Missing or invalid pageNumber parameter" });
                return;
            }

            var workNoteSystem = _projectManager.GetWorkNoteSystem(projectId);
            if (workNoteSystem == null)
            {
                RenderJson(new { success = false, error = "Project work note system not available" });
                return;
            }

            var note = workNoteSystem.ReadNote(pageNum);
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
                    authorGuid = note.AuthorGuid,
                    modifiedByGuid = note.ModifiedByGuid,
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

    private void CreateProjectWorkNote()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            Guid projectId = ExtractProjectIdFromPath();
            if (projectId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Invalid project ID" });
                return;
            }

            var body = GetJsonBody<Dictionary<string, object>>();
            if (body == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            if (!body.TryGetValue("summary", out var summaryObj) || string.IsNullOrWhiteSpace(summaryObj?.ToString()))
            {
                RenderJson(new { success = false, error = "Summary is required" });
                return;
            }

            if (!body.TryGetValue("content", out var contentObj) || string.IsNullOrWhiteSpace(contentObj?.ToString()))
            {
                RenderJson(new { success = false, error = "Content is required" });
                return;
            }

            string summary = summaryObj.ToString()!;
            string content = contentObj.ToString()!;
            string keywords = body.TryGetValue("keywords", out var kwObj) ? kwObj?.ToString() ?? "" : "";

            Guid authorGuid = Guid.Empty;
            if (body.TryGetValue("authorId", out var authorObj) && !string.IsNullOrWhiteSpace(authorObj?.ToString()))
            {
                Guid.TryParse(authorObj.ToString(), out authorGuid);
            }

            var workNoteSystem = _projectManager.GetWorkNoteSystem(projectId);
            if (workNoteSystem == null)
            {
                RenderJson(new { success = false, error = "Project work note system not available" });
                return;
            }

            var note = workNoteSystem.CreateNote(summary, content, keywords, authorGuid: authorGuid);

            RenderJson(new
            {
                success = true,
                data = new
                {
                    id = note.Id,
                    pageNumber = note.PageNumber,
                    summary = note.Summary,
                    keywords = note.Keywords,
                    version = note.Version,
                    authorGuid = note.AuthorGuid,
                    createdAt = note.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                }
            });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void UpdateProjectWorkNote()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            Guid projectId = ExtractProjectIdFromPath();
            if (projectId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Invalid project ID" });
                return;
            }

            var body = GetJsonBody<Dictionary<string, object>>();
            if (body == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            if (!body.TryGetValue("pageNumber", out var pageObj) || !int.TryParse(pageObj?.ToString(), out int pageNum))
            {
                RenderJson(new { success = false, error = "Missing or invalid pageNumber" });
                return;
            }

            string? content = body.TryGetValue("content", out var contentObj) ? contentObj?.ToString() : null;
            string? summary = body.TryGetValue("summary", out var summaryObj) ? summaryObj?.ToString() : null;
            string? keywords = body.TryGetValue("keywords", out var kwObj) ? kwObj?.ToString() : null;

            Guid modifiedByGuid = Guid.Empty;
            if (body.TryGetValue("authorId", out var authorObj) && !string.IsNullOrWhiteSpace(authorObj?.ToString()))
            {
                Guid.TryParse(authorObj.ToString(), out modifiedByGuid);
            }

            var workNoteSystem = _projectManager.GetWorkNoteSystem(projectId);
            if (workNoteSystem == null)
            {
                RenderJson(new { success = false, error = "Project work note system not available" });
                return;
            }

            var note = workNoteSystem.UpdateNote(pageNum, content, summary, keywords, modifiedByGuid: modifiedByGuid);

            RenderJson(new
            {
                success = true,
                data = new
                {
                    id = note.Id,
                    pageNumber = note.PageNumber,
                    summary = note.Summary,
                    version = note.Version,
                    modifiedByGuid = note.ModifiedByGuid,
                    updatedAt = note.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                }
            });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void DeleteProjectWorkNote()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            Guid projectId = ExtractProjectIdFromPath();
            if (projectId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Invalid project ID" });
                return;
            }

            var body = GetJsonBody<Dictionary<string, object>>();
            if (body == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            if (!body.TryGetValue("pageNumber", out var pageObj) || !int.TryParse(pageObj?.ToString(), out int pageNum))
            {
                RenderJson(new { success = false, error = "Missing or invalid pageNumber" });
                return;
            }

            var workNoteSystem = _projectManager.GetWorkNoteSystem(projectId);
            if (workNoteSystem == null)
            {
                RenderJson(new { success = false, error = "Project work note system not available" });
                return;
            }

            bool deleted = workNoteSystem.DeleteNote(pageNum);
            RenderJson(new { success = deleted, error = deleted ? (string?)null : $"Note page {pageNum} not found" });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    #endregion

    #region Project Tasks

    private void ProjectTasksPage()
    {
        try
        {
            if (_projectManager == null)
            {
                Response.StatusCode = 500;
                RenderHtml("<p>Project manager not available</p>");
                return;
            }

            Guid projectId = ExtractProjectIdFromPagePath();
            if (projectId == Guid.Empty)
            {
                Response.StatusCode = 400;
                RenderHtml("<p>Invalid project ID</p>");
                return;
            }

            var project = _projectManager.GetProject(projectId);
            if (project == null)
            {
                Response.StatusCode = 404;
                RenderHtml("<p>Project not found</p>");
                return;
            }

            var taskSystem = _projectManager.GetTaskSystem(projectId);
            int taskCount = taskSystem?.Count ?? 0;

            var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
            var view = new Views.ProjectTaskView();
            var vm = new Models.ProjectTaskViewModel
            {
                Skin = skin,
                ActiveMenu = "projects",
                ProjectId = projectId,
                ProjectName = project.Name,
                TotalTasks = taskCount
            };
            var html = view.Render(vm);
            RenderHtml(html);
        }
        catch (Exception ex)
        {
            Response.StatusCode = 500;
            RenderHtml($"<p>Error: {ex.Message}</p>");
        }
    }

    private void ListProjectTasks()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available", data = new List<object>() });
                return;
            }

            Guid projectId = ExtractProjectIdFromPath();
            if (projectId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Invalid project ID", data = new List<object>() });
                return;
            }

            var taskSystem = _projectManager.GetTaskSystem(projectId);
            if (taskSystem == null)
            {
                RenderJson(new { success = false, error = "Project task system not available", data = new List<object>() });
                return;
            }

            var beingManager = ServiceLocator.Instance.BeingManager;
            var tasks = taskSystem.GetAll();
            var taskList = tasks.Select(t => new
            {
                id = t.Id,
                title = t.Title,
                description = t.Description,
                status = t.Status.ToString().ToLowerInvariant(),
                priority = t.Priority,
                createdAt = t.CreatedAt,
                createdAtFormatted = t.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                startedAt = t.StartedAt,
                completedAt = t.CompletedAt,
                assigneeGuids = t.AssigneeGuids,
                assigneeNames = t.AssigneeGuids.Select(g => beingManager?.GetBeing(g)?.Name ?? g.ToString().Substring(0, 8)).ToList(),
                createdByGuid = t.CreatedByGuid,
                createdByName = beingManager?.GetBeing(t.CreatedByGuid)?.Name ?? t.CreatedByGuid.ToString().Substring(0, 8),
                errorMessage = t.ErrorMessage ?? ""
            }).ToList();

            RenderJson(new { success = true, data = taskList, total = taskList.Count });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message, data = new List<object>() });
        }
    }

    private void CreateProjectTask()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            Guid projectId = ExtractProjectIdFromPath();
            if (projectId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Invalid project ID" });
                return;
            }

            var body = GetJsonBody<Dictionary<string, object>>();
            if (body == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            if (!body.TryGetValue("title", out var titleObj) || string.IsNullOrWhiteSpace(titleObj?.ToString()))
            {
                RenderJson(new { success = false, error = "Title is required" });
                return;
            }

            string title = titleObj.ToString()!;
            string description = body.TryGetValue("description", out var descObj) ? descObj?.ToString() ?? "" : "";
            int priority = body.TryGetValue("priority", out var priObj) && int.TryParse(priObj?.ToString(), out int p) ? p : 100;

            Guid createdBy = Guid.Empty;
            if (body.TryGetValue("createdBy", out var creatorObj) && !string.IsNullOrWhiteSpace(creatorObj?.ToString()))
            {
                Guid.TryParse(creatorObj.ToString(), out createdBy);
            }

            List<Guid>? assignees = null;
            if (body.TryGetValue("assignees", out var assignObj) && assignObj is List<object> assignList)
            {
                assignees = assignList.Select(a => Guid.TryParse(a?.ToString(), out Guid g) ? g : Guid.Empty).Where(g => g != Guid.Empty).ToList();
            }

            var taskSystem = _projectManager.GetTaskSystem(projectId);
            if (taskSystem == null)
            {
                RenderJson(new { success = false, error = "Project task system not available" });
                return;
            }

            var task = taskSystem.Create(title, description, createdBy, priority, assignees);

            RenderJson(new
            {
                success = true,
                data = new
                {
                    id = task.Id,
                    title = task.Title,
                    description = task.Description,
                    status = task.Status.ToString().ToLowerInvariant(),
                    priority = task.Priority,
                    createdAt = task.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    assigneeGuids = task.AssigneeGuids,
                    createdByGuid = task.CreatedByGuid
                }
            });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void UpdateProjectTask()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            Guid projectId = ExtractProjectIdFromPath();
            if (projectId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Invalid project ID" });
                return;
            }

            var body = GetJsonBody<Dictionary<string, object>>();
            if (body == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            if (!body.TryGetValue("taskId", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
            {
                RenderJson(new { success = false, error = "Missing or invalid taskId" });
                return;
            }

            string? title = body.TryGetValue("title", out var titleObj) ? titleObj?.ToString() : null;
            string? description = body.TryGetValue("description", out var descObj) ? descObj?.ToString() : null;
            int? priority = body.TryGetValue("priority", out var priObj) && int.TryParse(priObj?.ToString(), out int p) ? p : null;

            var taskSystem = _projectManager.GetTaskSystem(projectId);
            if (taskSystem == null)
            {
                RenderJson(new { success = false, error = "Project task system not available" });
                return;
            }

            bool updated = taskSystem.Update(taskId, title, description, priority);
            RenderJson(new { success = updated, error = updated ? (string?)null : "Task not found" });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void DeleteProjectTask()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            Guid projectId = ExtractProjectIdFromPath();
            if (projectId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Invalid project ID" });
                return;
            }

            var body = GetJsonBody<Dictionary<string, object>>();
            if (body == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            if (!body.TryGetValue("taskId", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
            {
                RenderJson(new { success = false, error = "Missing or invalid taskId" });
                return;
            }

            var taskSystem = _projectManager.GetTaskSystem(projectId);
            if (taskSystem == null)
            {
                RenderJson(new { success = false, error = "Project task system not available" });
                return;
            }

            bool deleted = taskSystem.Delete(taskId);
            RenderJson(new { success = deleted, error = deleted ? (string?)null : "Task not found" });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void AssignProjectTask()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            Guid projectId = ExtractProjectIdFromPath();
            if (projectId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Invalid project ID" });
                return;
            }

            var body = GetJsonBody<Dictionary<string, object>>();
            if (body == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            if (!body.TryGetValue("taskId", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
            {
                RenderJson(new { success = false, error = "Missing or invalid taskId" });
                return;
            }

            if (!body.TryGetValue("beingId", out var bidObj) || !Guid.TryParse(bidObj?.ToString(), out Guid beingId))
            {
                RenderJson(new { success = false, error = "Missing or invalid beingId" });
                return;
            }

            var taskSystem = _projectManager.GetTaskSystem(projectId);
            if (taskSystem == null)
            {
                RenderJson(new { success = false, error = "Project task system not available" });
                return;
            }

            bool assigned = taskSystem.Assign(taskId, beingId);
            RenderJson(new { success = assigned, error = assigned ? (string?)null : "Task not found" });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void RemoveProjectTaskAssignee()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            Guid projectId = ExtractProjectIdFromPath();
            if (projectId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Invalid project ID" });
                return;
            }

            var body = GetJsonBody<Dictionary<string, object>>();
            if (body == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            if (!body.TryGetValue("taskId", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
            {
                RenderJson(new { success = false, error = "Missing or invalid taskId" });
                return;
            }

            if (!body.TryGetValue("beingId", out var bidObj) || !Guid.TryParse(bidObj?.ToString(), out Guid beingId))
            {
                RenderJson(new { success = false, error = "Missing or invalid beingId" });
                return;
            }

            var taskSystem = _projectManager.GetTaskSystem(projectId);
            if (taskSystem == null)
            {
                RenderJson(new { success = false, error = "Project task system not available" });
                return;
            }

            bool removed = taskSystem.RemoveAssignee(taskId, beingId);
            RenderJson(new { success = removed, error = removed ? (string?)null : "Task not found" });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void CompleteProjectTask()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            Guid projectId = ExtractProjectIdFromPath();
            if (projectId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Invalid project ID" });
                return;
            }

            var body = GetJsonBody<Dictionary<string, object>>();
            if (body == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            if (!body.TryGetValue("taskId", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
            {
                RenderJson(new { success = false, error = "Missing or invalid taskId" });
                return;
            }

            var taskSystem = _projectManager.GetTaskSystem(projectId);
            if (taskSystem == null)
            {
                RenderJson(new { success = false, error = "Project task system not available" });
                return;
            }

            bool completed = taskSystem.Complete(taskId);
            RenderJson(new { success = completed, error = completed ? (string?)null : "Task not found or not running" });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void FailProjectTask()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            Guid projectId = ExtractProjectIdFromPath();
            if (projectId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Invalid project ID" });
                return;
            }

            var body = GetJsonBody<Dictionary<string, object>>();
            if (body == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            if (!body.TryGetValue("taskId", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
            {
                RenderJson(new { success = false, error = "Missing or invalid taskId" });
                return;
            }

            string error = body.TryGetValue("error", out var errObj) ? errObj?.ToString() ?? "Unknown error" : "Unknown error";

            var taskSystem = _projectManager.GetTaskSystem(projectId);
            if (taskSystem == null)
            {
                RenderJson(new { success = false, error = "Project task system not available" });
                return;
            }

            bool failed = taskSystem.Fail(taskId, error);
            RenderJson(new { success = failed, error = failed ? (string?)null : "Task not found or not running" });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private void CancelProjectTask()
    {
        try
        {
            if (_projectManager == null)
            {
                RenderJson(new { success = false, error = "Project manager not available" });
                return;
            }

            Guid projectId = ExtractProjectIdFromPath();
            if (projectId == Guid.Empty)
            {
                RenderJson(new { success = false, error = "Invalid project ID" });
                return;
            }

            var body = GetJsonBody<Dictionary<string, object>>();
            if (body == null)
            {
                RenderJson(new { success = false, error = "Invalid request body" });
                return;
            }

            if (!body.TryGetValue("taskId", out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid taskId))
            {
                RenderJson(new { success = false, error = "Missing or invalid taskId" });
                return;
            }

            var taskSystem = _projectManager.GetTaskSystem(projectId);
            if (taskSystem == null)
            {
                RenderJson(new { success = false, error = "Project task system not available" });
                return;
            }

            bool cancelled = taskSystem.Cancel(taskId);
            RenderJson(new { success = cancelled, error = cancelled ? (string?)null : "Task not found or not pending" });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    #endregion
}
