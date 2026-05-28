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
/// Represents the execution state of a project think session.
/// Mirrors TimerExecutionState for consistency across the codebase.
/// </summary>
public enum ProjectThinkState
{
    /// <summary>
    /// Not started.
    /// </summary>
    Idle,

    /// <summary>
    /// Start notification sent, first AI call in progress.
    /// </summary>
    Started,

    /// <summary>
    /// Tool loop in progress (AI returned tool calls, awaiting continuation).
    /// </summary>
    Executing,

    /// <summary>
    /// Successfully completed.
    /// </summary>
    Completed,

    /// <summary>
    /// Execution failed.
    /// </summary>
    Failed
}

/// <summary>
/// Represents a pseudo-session for multi-round ThinkOnProject execution.
/// Allows the curator AI to continue interacting (e.g., creating beings → assigning roles)
/// within the same logical session across multiple Ticks, preventing the AI from
/// re-seeing "empty role pool" and creating duplicate beings.
/// Modeled after TimerItem's ExecutionState + ChatHistoryCycle pattern.
/// </summary>
public sealed class ProjectThinkSession
{
    /// <summary>
    /// Gets or sets the unique identifier for this think session.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the GUID of the curator being executing this session.
    /// </summary>
    public Guid BeingId { get; set; }

    /// <summary>
    /// Gets or sets the GUID of the project this session belongs to.
    /// </summary>
    public Guid ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the current execution state of this session.
    /// </summary>
    public ProjectThinkState State { get; set; } = ProjectThinkState.Idle;

    /// <summary>
    /// Gets or sets the current AI interaction round within this session.
    /// Each round represents one AI call (which may include tool calls that need follow-up).
    /// </summary>
    public int CurrentRound { get; set; } = 0;

    /// <summary>
    /// Gets or sets the maximum AI interaction rounds allowed per session (prevent infinite loops).
    /// </summary>
    public int MaxRounds { get; set; } = 10;

    /// <summary>
    /// Gets or sets the chat history cycles for this session.
    /// Each cycle represents one AI call round with its messages.
    /// </summary>
    public List<ChatHistoryCycle> ChatHistory { get; set; } = new();

    /// <summary>
    /// Gets or sets the creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the completion timestamp (null if not completed).
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Creates a new ProjectThinkSession with default values.
    /// </summary>
    public ProjectThinkSession()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    /// <summary>
    /// Gets the current (latest) chat history cycle, creating one if none exists.
    /// </summary>
    public ChatHistoryCycle GetCurrentCycle()
    {
        if (ChatHistory.Count == 0)
        {
            ChatHistory.Add(new ChatHistoryCycle(ProjectThinkState.Started));
        }
        return ChatHistory[^1];
    }

    /// <summary>
    /// Seals the current cycle with the given end status.
    /// </summary>
    public void SealCurrentCycle(ProjectThinkState endStatus)
    {
        if (ChatHistory.Count == 0) return;
        ChatHistory[^1].SetEndStatus(endStatus);
    }

    /// <summary>
    /// Appends a new cycle to the chat history.
    /// </summary>
    public void AppendNewCycle()
    {
        ChatHistory.Add(new ChatHistoryCycle(ProjectThinkState.Started));
    }

    /// <summary>
    /// Determines whether this session needs a continuation (next Tick should process it).
    /// True when the last message in the current cycle is a Tool result,
    /// meaning the AI made tool calls and needs to see the results.
    /// </summary>
    public bool NeedsContinuation()
    {
        if (State != ProjectThinkState.Executing && State != ProjectThinkState.Started)
            return false;
        if (ChatHistory.Count == 0) return false;
        var lastCycle = ChatHistory[^1];
        if (lastCycle.EndStatus != null) return false;
        if (lastCycle.Messages.Count == 0) return false;
        return lastCycle.Messages[^1].Role == MessageRole.Tool;
    }

    /// <summary>
    /// Marks this session as completed with the given state.
    /// </summary>
    public void Complete(ProjectThinkState finalState = ProjectThinkState.Completed)
    {
        State = finalState;
        CompletedAt = DateTime.Now;
        SealCurrentCycle(finalState);
    }
}
