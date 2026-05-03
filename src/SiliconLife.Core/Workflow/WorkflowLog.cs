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
/// Workflow state transition log (for auditing and tracing).
/// </summary>
public class WorkflowLog
{
    /// <summary>
    /// Gets or sets the log unique ID.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the associated workflow instance ID.
    /// </summary>
    public Guid InstanceId { get; set; }

    /// <summary>
    /// Gets or sets the source state.
    /// </summary>
    public string FromState { get; set; } = "";

    /// <summary>
    /// Gets or sets the target state.
    /// </summary>
    public string ToState { get; set; } = "";

    /// <summary>
    /// Gets or sets the transition name.
    /// </summary>
    public string TransitionName { get; set; } = "";

    /// <summary>
    /// Gets or sets the creation time.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the note (extension field).
    /// </summary>
    public string? Note { get; set; }
}
