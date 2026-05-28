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
/// Abstract base class for localization
/// Contains only core language information
/// </summary>
public abstract class LocalizationBase
{
    /// <summary>
    /// Gets the language code (e.g., "zh-CN", "en-US")
    /// </summary>
    public abstract string LanguageCode { get; }

    /// <summary>
    /// Gets the language name (e.g., "Simplified Chinese", "English")
    /// </summary>
    public abstract string LanguageName { get; }

    /// <summary>
    /// Gets the system prompt for memory compression
    /// </summary>
    public abstract string MemoryCompressionSystemPrompt { get; }

    /// <summary>
    /// Gets the common system prompt appended to all silicon beings.
    /// Defines behavioral guidelines for proactive assistance.
    /// </summary>
    public abstract string CommonSystemPrompt { get; }

    /// <summary>
    /// Gets the user prompt template for memory compression
    /// </summary>
    /// <param name="levelDesc">The level description (e.g., "hour", "day")</param>
    /// <param name="rangeDesc">The time range description</param>
    /// <param name="contentText">The memory content to compress</param>
    /// <returns>The formatted user prompt</returns>
    public abstract string GetMemoryCompressionUserPrompt(string levelDesc, string rangeDesc, string contentText);

    // ===== Chat Localization =====

    /// <summary>
    /// Format string for single chat session display name.
    /// {0} is replaced with the partner's display name.
    /// </summary>
    public abstract string SingleChatNameFormat { get; }

    /// <summary>
    /// Get a formatted single chat display name with the given partner name.
    /// </summary>
    /// <param name="partnerName">The partner's display name</param>
    /// <returns>The formatted session name</returns>
    public string GetSingleChatDisplayName(string partnerName) =>
        string.Format(SingleChatNameFormat, partnerName);

    /// <summary>
    /// Gets the localized display name for a log level.
    /// </summary>
    /// <param name="logLevel">The log level</param>
    /// <returns>The localized display name</returns>
    public abstract string GetLogLevelName(LogLevel logLevel);

    // ===== Being Activity Localization =====

    /// <summary>
    /// Gets the localized display name for a silicon being activity state.
    /// </summary>
    /// <param name="activity">The activity state</param>
    /// <returns>The localized display name (e.g., "Idle", "Thinking...", "Executing Timer")</returns>
    public abstract string GetBeingActivityName(BeingActivity activity);

    // ===== Memory Event Localization =====

    /// <summary>
    /// Formats a memory record for a single-chat AI response event.
    /// </summary>
    /// <param name="speakerName">Display name of the speaker</param>
    /// <param name="listenerName">Display name of the listener</param>
    /// <param name="content">The message content</param>
    public abstract string FormatMemoryEventSingleChat(string speakerName, string listenerName, string content);

    /// <summary>
    /// Formats a memory record for a group-chat AI response event.
    /// </summary>
    /// <param name="sessionId">The group session identifier</param>
    /// <param name="content">The AI response content (may be truncated)</param>
    public abstract string FormatMemoryEventGroupChat(string sessionId, string content);

    /// <summary>
    /// Formats a memory record for a tool-call round.
    /// </summary>
    /// <param name="toolNames">Comma-separated list of tool names that were called</param>
    public abstract string FormatMemoryEventToolCall(string toolNames);

    /// <summary>
    /// Formats a memory record for a task execution event.
    /// </summary>
    /// <param name="content">The AI response content for the task</param>
    public abstract string FormatMemoryEventTask(string content);

    /// <summary>
    /// Formats a memory record for a project thinking event.
    /// </summary>
    /// <param name="content">The AI response content for the project</param>
    public abstract string FormatMemoryEventProject(string content);

    /// <summary>
    /// Formats a memory record for a timer-triggered event.
    /// </summary>
    /// <param name="content">The AI response content triggered by the timer</param>
    public abstract string FormatMemoryEventTimer(string content);

    /// <summary>
    /// Formats a memory record for a timer error event.
    /// </summary>
    /// <param name="timerName">The name of the timer that failed</param>
    /// <param name="error">The error message</param>
    public abstract string FormatMemoryEventTimerError(string timerName, string error);

    // ===== Timer Notification Localization =====

    /// <summary>
    /// Formats a notification message when a timer starts execution.
    /// </summary>
    /// <param name="timerName">The name of the timer</param>
    public abstract string FormatTimerStartNotification(string timerName);

    /// <summary>
    /// Formats a notification message when a timer completes execution.
    /// </summary>
    /// <param name="timerName">The name of the timer</param>
    /// <param name="result">The execution result summary</param>
    public abstract string FormatTimerEndNotification(string timerName, string result);

    /// <summary>
    /// Formats a notification message when a timer execution fails.
    /// </summary>
    /// <param name="timerName">The name of the timer</param>
    /// <param name="error">The error message</param>
    public abstract string FormatTimerErrorNotification(string timerName, string error);

    // ===== Project Info Context Localization =====

    /// <summary>
    /// Gets the localized header for the project affiliation info section injected into AI context.
    /// </summary>
    public abstract string ProjectInfoHeader { get; }

    /// <summary>
    /// Gets the localized label for the role of a being within a project.
    /// </summary>
    public abstract string ProjectInfoRoleLabel { get; }

    /// <summary>
    /// Gets the localized label for the goal/description of a project.
    /// </summary>
    public abstract string ProjectInfoGoalLabel { get; }

    // ===== Project Role Context Localization =====

    /// <summary>
    /// Gets the localized section header for role definitions in project scenario context.
    /// </summary>
    public abstract string ProjectRoleDefinitionsHeader { get; }

    /// <summary>
    /// Gets the localized section header for role assignments in project scenario context.
    /// </summary>
    public abstract string ProjectRoleAssignmentsHeader { get; }

    /// <summary>
    /// Gets the localized message when no workflow template is assigned to a project.
    /// </summary>
    public abstract string ProjectNoWorkflowTemplate { get; }

    /// <summary>
    /// Gets the localized message when role pool is not fully staffed and needs curator attention.
    /// {0} is replaced with the number of understaffed roles.
    /// </summary>
    public abstract string ProjectRoleNeedsAttentionFormat { get; }

    /// <summary>
    /// Formats a message indicating that some roles need attention (understaffed).
    /// </summary>
    /// <param name="understaffedCount">Number of roles that are understaffed</param>
    /// <returns>The formatted attention message</returns>
    public string FormatProjectRoleNeedsAttention(int understaffedCount) =>
        string.Format(ProjectRoleNeedsAttentionFormat, understaffedCount);

    /// <summary>
    /// Gets the localized header for the staffing action plan section.
    /// </summary>
    public abstract string StaffingActionPlanHeader { get; }

    /// <summary>
    /// Gets the localized format string for total beings needed to create.
    /// {0} is replaced with the total count.
    /// </summary>
    public abstract string TotalBeingsNeededFormat { get; }

    /// <summary>
    /// Formats a message indicating the total number of beings to create.
    /// </summary>
    /// <param name="count">Total beings needed</param>
    /// <returns>The formatted message</returns>
    public string FormatTotalBeingsNeeded(int count) =>
        string.Format(TotalBeingsNeededFormat, count);

    /// <summary>
    /// Gets the localized header for the per-role shortage breakdown section.
    /// </summary>
    public abstract string StaffingRoleBreakdownHeader { get; }

    /// <summary>
    /// Gets the localized format string for per-role shortage detail.
    /// {0}=roleName, {1}=minCount, {2}=assignedCount, {3}=shortage
    /// </summary>
    public abstract string RoleShortageDetailFormat { get; }

    /// <summary>
    /// Formats a per-role shortage detail line.
    /// </summary>
    /// <param name="roleName">The role name</param>
    /// <param name="minCount">Minimum required count</param>
    /// <param name="assignedCount">Currently assigned count</param>
    /// <param name="shortage">Number of additional beings needed</param>
    /// <returns>The formatted detail line</returns>
    public string FormatRoleShortageDetail(string roleName, int minCount, int assignedCount, int shortage) =>
        string.Format(RoleShortageDetailFormat, roleName, minCount, assignedCount, shortage);

    /// <summary>
    /// Gets the localized header for the suggested action steps section.
    /// </summary>
    public abstract string StaffingActionStepsHeader { get; }

    /// <summary>
    /// Gets the localized step 1: create beings using silicon_manager.
    /// {0} is replaced with the total count of beings to create.
    /// </summary>
    public abstract string StaffingStepCreateBeingsFormat { get; }

    /// <summary>
    /// Formats step 1 instruction.
    /// </summary>
    /// <param name="count">Total beings to create</param>
    /// <returns>The formatted step instruction</returns>
    public string FormatStaffingStepCreateBeings(int count) =>
        string.Format(StaffingStepCreateBeingsFormat, count);

    /// <summary>
    /// Gets the localized step 2: assign beings to project.
    /// </summary>
    public abstract string StaffingStepAssignToProject { get; }

    /// <summary>
    /// Gets the localized step 3: assign beings to roles.
    /// </summary>
    public abstract string StaffingStepAssignToRoles { get; }

    /// <summary>
    /// Gets the localized message for the empty role pool scenario,
    /// providing actionable guidance when no roles have been assigned at all.
    /// {0} is replaced with the total number of required roles.
    /// </summary>
    public abstract string EmptyRolePoolActionFormat { get; }

    /// <summary>
    /// Formats a message for the empty role pool scenario.
    /// </summary>
    /// <param name="requiredRoleCount">Number of required roles from the template</param>
    /// <returns>The formatted message</returns>
    public string FormatEmptyRolePoolAction(int requiredRoleCount) =>
        string.Format(EmptyRolePoolActionFormat, requiredRoleCount);

    /// <summary>
    /// Gets the localized label for minimum count in a role definition.
    /// </summary>
    public abstract string RoleMinCountLabel { get; }

    /// <summary>
    /// Gets the localized label for maximum count in a role definition.
    /// </summary>
    public abstract string RoleMaxCountLabel { get; }

    /// <summary>
    /// Gets the localized text for unlimited maximum count (e.g., "∞" or "Unlimited").
    /// </summary>
    public abstract string RoleMaxCountUnlimited { get; }

    /// <summary>
    /// Gets the localized label for assigned count in a role.
    /// </summary>
    public abstract string RoleAssignedCountLabel { get; }

    /// <summary>
    /// Gets the localized message for unassigned required roles.
    /// </summary>
    public abstract string ProjectUnassignedRolesLabel { get; }

    // ===== Available Beings Context Localization =====

    /// <summary>
    /// Gets the localized section header for available beings not yet assigned to the project.
    /// </summary>
    public abstract string ProjectAvailableBeingsHeader { get; }

    /// <summary>
    /// Gets the localized hint message suggesting to assign existing beings before creating new ones.
    /// </summary>
    public abstract string ProjectAvailableBeingsHint { get; }

    // ===== Project Attention Reason Localization =====

    /// <summary>
    /// Gets the localized section header for project attention reasons in project scenario context.
    /// </summary>
    public abstract string ProjectAttentionReasonsHeader { get; }

    /// <summary>
    /// Gets the localized section header for unsatisfied role details when project needs attention.
    /// </summary>
    public abstract string ProjectUnsatisfiedRolesDetailHeader { get; }

    /// <summary>
    /// Gets the localized display name for a project attention reason.
    /// </summary>
    /// <param name="reason">The attention reason</param>
    /// <returns>The localized display name (e.g., "Missing workflow template", "Empty role pool")</returns>
    public abstract string GetProjectAttentionReasonName(ProjectAttentionReason reason);

    // ===== Role Staffing Localization =====

    /// <summary>
    /// Gets the localized display name for a role staffing status.
    /// </summary>
    /// <param name="status">The staffing status</param>
    /// <returns>The localized display name (e.g., "Understaffed", "Sufficient")</returns>
    public abstract string GetRoleStaffingStatusName(RoleStaffingStatus status);

    /// <summary>
    /// Gets the localized detailed staffing status text for a role.
    /// Includes the status name with count details.
    /// </summary>
    /// <param name="status">The staffing status</param>
    /// <param name="minCount">The minimum required count</param>
    /// <param name="maxCount">The maximum allowed count (0 = unlimited)</param>
    /// <param name="assignedCount">The currently assigned count</param>
    /// <returns>A localized status text with count details</returns>
    public abstract string GetRoleStaffingStatusText(RoleStaffingStatus status, int minCount, int maxCount, int assignedCount);

    // ===== Workflow Role Notification Localization =====

    /// <summary>
    /// Formats a broadcast notification message when a workflow transition is blocked due to missing roles.
    /// </summary>
    /// <param name="projectName">The project name</param>
    /// <param name="transitionName">The blocked transition name</param>
    /// <param name="fromState">Source state</param>
    /// <param name="toState">Target state</param>
    /// <param name="missingRoles">Comma-separated list of missing roles with details</param>
    /// <returns>The formatted notification message</returns>
    public abstract string FormatWorkflowRoleBlockedNotification(
        string projectName, string transitionName, string fromState, string toState, string missingRoles);
}
