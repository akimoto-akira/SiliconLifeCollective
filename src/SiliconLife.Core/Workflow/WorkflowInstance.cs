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

namespace SiliconLife.Collective;

/// <summary>
/// Workflow execution instance: each instance represents a specific workflow execution.
/// </summary>
public class WorkflowInstance
{
    /// <summary>
    /// Gets or sets the instance unique ID.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the project ID this workflow belongs to.
    /// </summary>
    public Guid ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the template name used.
    /// </summary>
    public string TemplateName { get; set; } = "";

    /// <summary>
    /// Gets or sets the business key (e.g., PR number, Issue ID, project ID).
    /// </summary>
    public string BusinessKey { get; set; } = "";

    /// <summary>
    /// Gets or sets the current state.
    /// </summary>
    public string CurrentState { get; set; } = "";

    /// <summary>
    /// Gets or sets the list of task IDs for the current stage (linked to TaskSystem).
    /// </summary>
    public List<Guid> CurrentTaskIds { get; set; } = new();

    /// <summary>
    /// Gets or sets the stage output data (JSON embedded).
    /// Key: stage name, Value: output data (any type).
    /// </summary>
    public Dictionary<string, object> StageOutputs { get; set; } = new();

    /// <summary>
    /// Gets or sets the workflow status (InProgress, Completed, Blocked, Cancelled).
    /// </summary>
    public string Status { get; set; } = "InProgress";

    /// <summary>
    /// Gets or sets the creation time.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the last update time.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the last progress time (for timeout check).
    /// </summary>
    public DateTime LastProgressAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the GUID of the being who created this workflow.
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the extension fields (for plugin use).
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Marks progress as updated (automatically updates UpdatedAt and LastProgressAt).
    /// </summary>
    public void MarkProgress()
    {
        UpdatedAt = DateTime.UtcNow;
        LastProgressAt = DateTime.UtcNow;
    }
}
