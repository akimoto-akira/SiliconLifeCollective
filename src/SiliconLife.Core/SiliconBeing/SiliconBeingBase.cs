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
/// Abstract base class for all silicon beings
/// </summary>
public abstract class SiliconBeingBase
{
    /// <summary>
    /// Gets the unique identifier for this silicon being
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Gets the name of this silicon being
    /// </summary>
    public string Name { get; protected set; }

    /// <summary>
    /// Gets or sets the AI client for this silicon being
    /// </summary>
    public IAIClient? AIClient { get; set; }

    /// <summary>
    /// Gets or sets the soul content (system prompt) for this silicon being
    /// </summary>
    public string? SoulContent { get; set; }

    /// <summary>
    /// Gets or sets the user ID associated with this silicon being
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the tool manager for this silicon being.
    /// Each being holds its own ToolManager instance.
    /// </summary>
    public ToolManager? ToolManager { get; set; }

    /// <summary>
    /// Gets or sets the permission manager for this silicon being.
    /// Each being holds its own PermissionManager instance.
    /// </summary>
    public PermissionManager? PermissionManager { get; set; }

    /// <summary>
    /// Gets or sets the time storage for this silicon being.
    /// Used by Memory for time-indexed queries.
    /// </summary>
    public ITimeStorage? TimeStorage { get; set; }

    /// <summary>
    /// Gets or sets the memory system for this silicon being.
    /// Each being holds its own Memory instance.
    /// </summary>
    public Memory? Memory { get; set; }

    /// <summary>
    /// Gets or sets the task system for this silicon being.
    /// Each being holds its own TaskSystem instance.
    /// </summary>
    public TaskSystem? TaskSystem { get; set; }

    /// <summary>
    /// Gets or sets the timer system for this silicon being.
    /// Each being holds its own TimerSystem instance.
    /// </summary>
    public TimerSystem? TimerSystem { get; set; }

    /// <summary>
    /// Gets whether this silicon being is a curator (highest privilege level).
    /// Determined by comparing Id with Config.CuratorGuid.
    /// </summary>
    public bool IsCurator => Id == (Config.Instance?.Data?.CuratorGuid ?? Guid.Empty);

    /// <summary>
    /// Gets whether this silicon being is idle (no pending tasks)
    /// </summary>
    public abstract bool IsIdle { get; }

    /// <summary>
    /// Gets whether this silicon being is using a custom compiled implementation
    /// </summary>
    public bool IsCustomCompiled { get; protected set; }

    /// <summary>
    /// Gets the custom type name if this is a custom compiled being
    /// </summary>
    public string? CustomTypeName { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the SiliconBeingBase class
    /// </summary>
    /// <param name="id">The unique identifier</param>
    /// <param name="name">The name of the silicon being</param>
    protected SiliconBeingBase(Guid id, string name)
    {
        Id = id;
        Name = name;
        IsCustomCompiled = false;
        CustomTypeName = null;
    }

    /// <summary>
    /// Called by SiliconBeingManager on each tick
    /// </summary>
    /// <param name="deltaTime">Time elapsed since the last tick</param>
    public abstract void Tick(TimeSpan deltaTime);
}
