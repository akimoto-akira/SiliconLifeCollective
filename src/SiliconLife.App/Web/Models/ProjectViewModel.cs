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

namespace SiliconLife.App.Web.Models;

public class ProjectViewModel : ViewModelBase
{
    public List<ProjectItem> Projects { get; set; } = new();
    public int TotalCount => Projects.Count;
    public int ActiveCount => Projects.Count(p => p.Status == "active");
    public int ArchivedCount => Projects.Count(p => p.Status == "archived");
}

public class ProjectItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Status { get; set; } = "active";
    public int BeingCount { get; set; }
    public string WorkflowTemplateName { get; set; } = string.Empty;
    public Guid? GroupChatSessionId { get; set; }
    public Guid? BroadcastChannelId { get; set; }
}

public class WorkflowDetailViewModel : ViewModelBase
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = "";
    public string WorkflowTemplateName { get; set; } = "";
}

public class WorkflowRoleItem
{
    public string RoleName { get; set; } = "";
    public string Description { get; set; } = "";
    public int MinCount { get; set; }
    public int MaxCount { get; set; }
    public List<WorkflowBeingItem> AssignedBeings { get; set; } = new();
}

public class WorkflowBeingItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string? RoleName { get; set; }
}

public class WorkflowTransitionItem
{
    public string TransitionName { get; set; } = "";
    public string FromState { get; set; } = "";
    public string ToState { get; set; } = "";
}

public class WorkflowStateItem
{
    public string Name { get; set; } = "";
    public bool IsInitial { get; set; }
    public bool IsTerminal { get; set; }
}

public class ProjectThinkHistoryViewModel : ViewModelBase
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = "";
}

public class ProjectThinkSessionItem
{
    public string SessionId { get; set; } = "";
    public string BeingId { get; set; } = "";
    public string BeingName { get; set; } = "";
    public string State { get; set; } = "";
    public string? FailureReason { get; set; }
    public string CreatedAt { get; set; } = "";
    public string? CompletedAt { get; set; }
    public int CycleCount { get; set; }
    public int MessageCount { get; set; }
}

public class ProjectThinkDetailViewModel : ViewModelBase
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = "";
    public string SessionId { get; set; } = "";
    public string State { get; set; } = "";
    public string? FailureReason { get; set; }
    public string CreatedAt { get; set; } = "";
    public string? CompletedAt { get; set; }
    public Dictionary<string, string> ToolDisplayNames { get; set; } = new();
}
