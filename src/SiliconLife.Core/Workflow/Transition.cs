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
/// State transition rule: defines how to transition from one state to another.
/// </summary>
public class Transition
{
    /// <summary>
    /// Gets or sets the transition name (for logging and auditing).
    /// </summary>
    public string TransitionName { get; set; } = "";

    /// <summary>
    /// Gets or sets the source state.
    /// </summary>
    public string FromState { get; set; } = "";

    /// <summary>
    /// Gets or sets the target state.
    /// </summary>
    public string ToState { get; set; } = "";

    /// <summary>
    /// Gets or sets the transition condition: returns true to allow transition.
    /// Supports async operations (query database, call API, check files, etc.).
    /// </summary>
    public Func<WorkflowInstance, IServiceProvider, Task<bool>> Condition { get; set; } 
        = async (_, _) => true;

    /// <summary>
    /// Gets or sets the transition action: operations executed after state switch.
    /// Supports async operations (send notification, update permissions, log, etc.).
    /// </summary>
    public Func<WorkflowInstance, IServiceProvider, Task> Action { get; set; } 
        = async (_, _) => { };

    /// <summary>
    /// Gets or sets the timeout days (triggers exception flow if no progress exceeds this days).
    /// 0 means timeout check is disabled.
    /// </summary>
    public int TimeoutDays { get; set; } = 0;

    /// <summary>
    /// Gets or sets the transition priority (lower value = higher priority, used for multi-transition competition).
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// Gets or sets the extension fields (for plugin use).
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}
