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
/// Skill definition: a reusable capability unit that encapsulates tool
/// orchestration plus a prompt template. A skill represents one "ability"
/// of a silicon being and can be invoked autonomously by the AI (via
/// function calling) or explicitly by the user / curator.
/// </summary>
public class SkillDefinition
{
    /// <summary>Unique skill identifier (e.g. "summarize_document").</summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>Functional description shown to the AI (like Tool.Description).</summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>Localization display-name key (used by the Web UI).</summary>
    public string DisplayNameKey { get; init; } = string.Empty;

    /// <summary>Semantic version (e.g. "1.0.0").</summary>
    public string Version { get; init; } = "1.0.0";

    /// <summary>Classification tags (for UI grouping).</summary>
    public List<string> Tags { get; init; } = new();

    /// <summary>
    /// Parameter JSON Schema (same format as ITool.GetParameterSchema()).
    /// The AI provides arguments according to this schema when calling the skill.
    /// </summary>
    public Dictionary<string, object> ParameterSchema { get; init; } = new();

    /// <summary>
    /// System prompt template injected into the sub AIRequest's system message.
    /// Supports {param_name} placeholders filled from skill arguments at execution time.
    /// </summary>
    public string SystemPromptTemplate { get; init; } = string.Empty;

    /// <summary>
    /// Tool whitelist restricting the tools available while the skill executes.
    /// Empty list = inherit all of the being's tools; non-empty = only these tools.
    /// </summary>
    public List<string> ToolWhitelist { get; init; } = new();

    /// <summary>
    /// Optional tool action-level restrictions (reuses ToolActionPermissionConfig).
    /// Intersected with the being's own permissions (the stricter side wins).
    /// </summary>
    public ToolActionPermissionConfig? ToolActionRestrictions { get; init; }

    /// <summary>Maximum tool-call rounds (default 5).</summary>
    public int MaxToolRound { get; init; } = 5;

    /// <summary>Execution timeout (default 60 seconds).</summary>
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(60);

    /// <summary>
    /// Post-completion action ("none" / "write_memory" / "notify_curator" / "broadcast").
    /// Defaults to "write_memory".
    /// </summary>
    public string OnCompleteAction { get; init; } = "write_memory";

    /// <summary>Skill origin.</summary>
    public SkillSource Source { get; init; } = SkillSource.Builtin;

    /// <summary>Trigger mode.</summary>
    public SkillTriggerMode TriggerMode { get; init; } = SkillTriggerMode.Manual;

    /// <summary>
    /// Auto-trigger condition (only valid when TriggerMode = Auto).
    /// Supported values: "schedule" (cron-like). Specific trigger parameters
    /// are stored in <see cref="Metadata"/>.
    /// </summary>
    public string? AutoTriggerCondition { get; init; }

    /// <summary>Extension metadata (schedule expression, timezone, etc.).</summary>
    public Dictionary<string, object> Metadata { get; init; } = new();
}

/// <summary>Skill origin.</summary>
public enum SkillSource
{
    /// <summary>Built into the framework.</summary>
    Builtin,

    /// <summary>Registered by a plugin.</summary>
    Plugin,

    /// <summary>Created by a silicon being at runtime.</summary>
    Being,

    /// <summary>Defined by the user through the Web UI.</summary>
    User,
}

/// <summary>Trigger mode.</summary>
public enum SkillTriggerMode
{
    /// <summary>The AI decides autonomously when to call (via function calling).</summary>
    Manual,

    /// <summary>Triggered automatically when a condition is met.</summary>
    Auto,
}
