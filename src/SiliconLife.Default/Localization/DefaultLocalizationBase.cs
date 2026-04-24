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

using SiliconLife.Collective;
using SiliconLife.Default.ChineseHistorical;

namespace SiliconLife.Default;

/// <summary>
/// Abstract base class for default localization implementation
/// Contains all UI text and error messages
/// </summary>
public abstract class DefaultLocalizationBase : LocalizationBase
{
    /// <summary>
    /// Gets the welcome message displayed when the application starts
    /// </summary>
    public abstract string WelcomeMessage { get; }

    /// <summary>
    /// Gets the brand name displayed in the header
    /// </summary>
    public abstract string BrandName { get; }

    /// <summary>
    /// Gets the prompt displayed when waiting for user input
    /// </summary>
    public abstract string InputPrompt { get; }

    /// <summary>
    /// Gets the message displayed when the application is shutting down
    /// </summary>
    public abstract string ShutdownMessage { get; }

    /// <summary>
    /// Gets the error message when configuration file is corrupted
    /// </summary>
    public abstract string ConfigCorruptedError { get; }

    /// <summary>
    /// Gets the message when configuration file is created with default values
    /// </summary>
    public abstract string ConfigCreatedWithDefaults { get; }

    /// <summary>
    /// Gets the error message when AI client fails to connect
    /// </summary>
    public abstract string AIConnectionError { get; }

    /// <summary>
    /// Gets the error message when AI request fails
    /// </summary>
    public abstract string AIRequestError { get; }

    /// <summary>
    /// Gets the error message when data directory cannot be created
    /// </summary>
    public abstract string DataDirectoryCreateError { get; }

    /// <summary>
    /// Gets the message displayed when a silicon being is thinking
    /// </summary>
    public abstract string ThinkingMessage { get; }

    /// <summary>
    /// Gets the message displayed when a silicon being executes tool calls
    /// </summary>
    public abstract string ToolCallMessage { get; }

    /// <summary>
    /// Gets the error message when an operation fails
    /// </summary>
    public abstract string ErrorMessage { get; }

    /// <summary>
    /// Gets the error message when an unexpected error occurs
    /// </summary>
    public abstract string UnexpectedErrorMessage { get; }

    /// <summary>
    /// Gets the message when a permission request is denied
    /// </summary>
    public abstract string PermissionDeniedMessage { get; }

    /// <summary>
    /// Gets the prompt for user permission decision
    /// </summary>
    public abstract string PermissionAskPrompt { get; }

    /// <summary>
    /// Gets the header displayed for permission requests
    /// </summary>
    public abstract string PermissionRequestHeader { get; }

    /// <summary>
    /// Gets the description text for permission request page
    /// </summary>
    public abstract string PermissionRequestDescription { get; }

    /// <summary>
    /// Gets the label for permission type in request page
    /// </summary>
    public abstract string PermissionRequestTypeLabel { get; }

    /// <summary>
    /// Gets the label for requested resource in request page
    /// </summary>
    public abstract string PermissionRequestResourceLabel { get; }

    /// <summary>
    /// Gets the text for allow button in request page
    /// </summary>
    public abstract string PermissionRequestAllowButton { get; }

    /// <summary>
    /// Gets the text for deny button in request page
    /// </summary>
    public abstract string PermissionRequestDenyButton { get; }

    /// <summary>
    /// Gets the label for cache checkbox in request page
    /// </summary>
    public abstract string PermissionRequestCacheLabel { get; }

    /// <summary>
    /// Gets the label for cache duration selector in request page
    /// </summary>
    public abstract string PermissionRequestDurationLabel { get; }

    /// <summary>
    /// Gets the waiting message text in request page
    /// </summary>
    public abstract string PermissionRequestWaitingMessage { get; }

    /// <summary>
    /// Gets the label for the allow code in permission prompts
    /// </summary>
    public abstract string AllowCodeLabel { get; }

    /// <summary>
    /// Gets the label for the deny code in permission prompts
    /// </summary>
    public abstract string DenyCodeLabel { get; }

    /// <summary>
    /// Gets the instruction text for replying to permission prompts
    /// </summary>
    public abstract string PermissionReplyInstruction { get; }

    /// <summary>
    /// Gets the prompt for asking whether to cache a permission decision
    /// </summary>
    public abstract string AddToCachePrompt { get; }

    /// <summary>
    /// Gets the label for the permission cache checkbox in the web UI
    /// </summary>
    public abstract string PermissionCacheLabel { get; }

    /// <summary>
    /// Gets the label for the cache duration selector in the permission dialog
    /// </summary>
    public abstract string PermissionCacheDurationLabel { get; }

    /// <summary>
    /// Gets the option text for 1-hour cache duration
    /// </summary>
    public abstract string PermissionCacheDuration1Hour { get; }

    /// <summary>
    /// Gets the option text for 24-hour cache duration
    /// </summary>
    public abstract string PermissionCacheDuration24Hours { get; }

    /// <summary>
    /// Gets the option text for 7-day cache duration
    /// </summary>
    public abstract string PermissionCacheDuration7Days { get; }

    /// <summary>
    /// Gets the option text for 30-day cache duration
    /// </summary>
    public abstract string PermissionCacheDuration30Days { get; }

    /// <summary>
    /// Gets the localized display name for a permission type
    /// </summary>
    /// <param name="permissionType">The permission type</param>
    /// <returns>The localized display name</returns>
    public abstract string GetPermissionTypeName(PermissionType permissionType);

    /// <summary>
    /// Gets the title text for the permission dialog in the web UI
    /// </summary>
    public abstract string PermissionDialogTitle { get; }

    /// <summary>
    /// Gets the label for the permission type field in the permission dialog
    /// </summary>
    public abstract string PermissionTypeLabel { get; }

    /// <summary>
    /// Gets the label for the requested resource field in the permission dialog
    /// </summary>
    public abstract string PermissionResourceLabel { get; }

    /// <summary>
    /// Gets the label for the detail information field in the permission dialog
    /// </summary>
    public abstract string PermissionDetailLabel { get; }

    /// <summary>
    /// Gets the text for the allow button in the permission dialog
    /// </summary>
    public abstract string PermissionAllowButton { get; }

    /// <summary>
    /// Gets the text for the deny button in the permission dialog
    /// </summary>
    public abstract string PermissionDenyButton { get; }

    /// <summary>
    /// Gets the console error message when permission respond fails
    /// </summary>
    public abstract string PermissionRespondFailed { get; }

    /// <summary>
    /// Gets the console error prefix when permission respond throws an error
    /// </summary>
    public abstract string PermissionRespondError { get; }

    // ===== Init Page Localization =====

    /// <summary>
    /// Gets the page title for the initialization page
    /// </summary>
    public abstract string InitPageTitle { get; }

    /// <summary>
    /// Gets the description text displayed below the title on the init page
    /// </summary>
    public abstract string InitDescription { get; }

    /// <summary>
    /// Gets the label for the nickname field
    /// </summary>
    public abstract string InitNicknameLabel { get; }

    /// <summary>
    /// Gets the placeholder for the nickname input
    /// </summary>
    public abstract string InitNicknamePlaceholder { get; }

    /// <summary>
    /// Gets the label for the AI API endpoint field
    /// </summary>
    public abstract string InitEndpointLabel { get; }

    /// <summary>
    /// Gets the placeholder for the AI API endpoint input
    /// </summary>
    public abstract string InitEndpointPlaceholder { get; }

    /// <summary>
    /// Gets the label for the AI client type field
    /// </summary>
    public abstract string InitAIClientTypeLabel { get; }

    /// <summary>
    /// Gets the label for the default model field
    /// </summary>
    public abstract string InitModelLabel { get; }

    /// <summary>
    /// Gets the placeholder for the default model input
    /// </summary>
    public abstract string InitModelPlaceholder { get; }

    /// <summary>
    /// Gets the label for the skin field
    /// </summary>
    public abstract string InitSkinLabel { get; }

    /// <summary>
    /// Gets the placeholder for the skin input
    /// </summary>
    public abstract string InitSkinPlaceholder { get; }

    /// <summary>
    /// Gets the label for the data directory field
    /// </summary>
    public abstract string InitDataDirectoryLabel { get; }

    /// <summary>
    /// Gets the placeholder text for the data directory input
    /// </summary>
    public abstract string InitDataDirectoryPlaceholder { get; }

    /// <summary>
    /// Gets the text for the browse folder button
    /// </summary>
    public abstract string InitDataDirectoryBrowse { get; }

    /// <summary>
    /// Gets the text shown after "already selected" on a skin card
    /// </summary>
    public abstract string InitSkinSelected { get; }

    /// <summary>
    /// Gets the preview section title
    /// </summary>
    public abstract string InitSkinPreviewTitle { get; }

    /// <summary>
    /// Gets the preview card title text
    /// </summary>
    public abstract string InitSkinPreviewCardTitle { get; }

    /// <summary>
    /// Gets the preview card content text
    /// </summary>
    public abstract string InitSkinPreviewCardContent { get; }

    /// <summary>
    /// Gets the preview primary button text
    /// </summary>
    public abstract string InitSkinPreviewPrimaryBtn { get; }

    /// <summary>
    /// Gets the preview secondary button text
    /// </summary>
    public abstract string InitSkinPreviewSecondaryBtn { get; }

    /// <summary>
    /// Gets the text for the submit button
    /// </summary>
    public abstract string InitSubmitButton { get; }

    /// <summary>
    /// Gets the footer hint text about modifying settings later
    /// </summary>
    public abstract string InitFooterHint { get; }

    /// <summary>
    /// Gets the validation error message when nickname is empty
    /// </summary>
    public abstract string InitNicknameRequiredError { get; }

    /// <summary>
    /// Gets the validation error message when data directory is empty
    /// </summary>
    public abstract string InitDataDirectoryRequiredError { get; }

    /// <summary>
    /// Gets the label for the curator (first silicon being) name field
    /// </summary>
    public abstract string InitCuratorNameLabel { get; }

    /// <summary>
    /// Gets the placeholder for the curator name input
    /// </summary>
    public abstract string InitCuratorNamePlaceholder { get; }

    /// <summary>
    /// Gets the validation error message when curator name is empty
    /// </summary>
    public abstract string InitCuratorNameRequiredError { get; }

    /// <summary>
    /// Gets the label for the language selector field
    /// </summary>
    public abstract string InitLanguageLabel { get; }

    /// <summary>
    /// Gets the button text for applying language switch
    /// </summary>
    public abstract string InitLanguageSwitchBtn { get; }

    // ===== Navigation Menu Localization =====

    /// <summary>
    /// Gets the label for the chat menu item
    /// </summary>
    public abstract string NavMenuChat { get; }

    /// <summary>
    /// Gets the label for the dashboard menu item
    /// </summary>
    public abstract string NavMenuDashboard { get; }

    /// <summary>
    /// Gets the label for the beings menu item
    /// </summary>
    public abstract string NavMenuBeings { get; }

    /// <summary>
    /// Gets the label for the audit menu item
    /// </summary>
    public abstract string NavMenuAudit { get; }

    /// <summary>
    /// Gets the label for the tasks menu item
    /// </summary>
    public abstract string NavMenuTasks { get; }

    /// <summary>
    /// Gets the label for the memory menu item
    /// </summary>
    public abstract string NavMenuMemory { get; }

    /// <summary>
    /// Gets the label for the knowledge menu item
    /// </summary>
    public abstract string NavMenuKnowledge { get; }

    /// <summary>
    /// Gets the label for the projects menu item
    /// </summary>
    public abstract string NavMenuProjects { get; }

    /// <summary>
    /// Gets the label for the logs menu item
    /// </summary>
    public abstract string NavMenuLogs { get; }

    /// <summary>
    /// Gets the label for the config menu item
    /// </summary>
    public abstract string NavMenuConfig { get; }

    /// <summary>
    /// Gets the label for the help menu item
    /// </summary>
    public abstract string NavMenuHelp { get; }

    /// <summary>
    /// Gets the label for the about menu item
    /// </summary>
    public abstract string NavMenuAbout { get; }

    // ===== Page Title Localization =====

    /// <summary>
    /// Gets the page title for chat page
    /// </summary>
    public abstract string PageTitleChat { get; }

    /// <summary>
    /// Gets the page title for dashboard page
    /// </summary>
    public abstract string PageTitleDashboard { get; }

    /// <summary>
    /// Gets the page title for beings page
    /// </summary>
    public abstract string PageTitleBeings { get; }

    /// <summary>
    /// Gets the page title for tasks page
    /// </summary>
    public abstract string PageTitleTasks { get; }

    /// <summary>
    /// Gets the page title for timers page
    /// </summary>
    public abstract string PageTitleTimers { get; }

    /// <summary>
    /// Gets the page title for memory page
    /// </summary>
    public abstract string PageTitleMemory { get; }

    /// <summary>
    /// Gets the page title for knowledge page
    /// </summary>
    public abstract string PageTitleKnowledge { get; }

    /// <summary>
    /// Gets the page title for projects page
    /// </summary>
    public abstract string PageTitleProjects { get; }

    /// <summary>
    /// Gets the page title for logs page
    /// </summary>
    public abstract string PageTitleLogs { get; }

    /// <summary>
    /// Gets the page title for audit page
    /// </summary>
    public abstract string PageTitleAudit { get; }

    /// <summary>
    /// Gets the page title for config page
    /// </summary>
    public abstract string PageTitleConfig { get; }

    /// <summary>
    /// Gets the page title for executor page
    /// </summary>
    public abstract string PageTitleExecutor { get; }

    /// <summary>
    /// Gets the page title for code browser page
    /// </summary>
    public abstract string PageTitleCodeBrowser { get; }

    /// <summary>
    /// Gets the page title for permission page
    /// </summary>
    public abstract string PageTitlePermission { get; }

    /// <summary>
    /// Gets the page title for about page
    /// </summary>
    public abstract string PageTitleAbout { get; }

    // ===== Dashboard Localization =====

    /// <summary>
    /// Gets the dashboard page header text
    /// </summary>
    public abstract string DashboardPageHeader { get; }

    /// <summary>
    /// Gets the label for total beings count statistic
    /// </summary>
    public abstract string DashboardStatTotalBeings { get; }

    /// <summary>
    /// Gets the label for active beings count statistic
    /// </summary>
    public abstract string DashboardStatActiveBeings { get; }

    /// <summary>
    /// Gets the label for uptime statistic
    /// </summary>
    public abstract string DashboardStatUptime { get; }

    /// <summary>
    /// Gets the label for memory usage statistic
    /// </summary>
    public abstract string DashboardStatMemory { get; }

    /// <summary>
    /// Gets the label for message frequency chart
    /// </summary>
    public abstract string DashboardChartMessageFrequency { get; }

    // ===== Beings Page Localization =====

    /// <summary>
    /// Gets the beings page header text
    /// </summary>
    public abstract string BeingsPageHeader { get; }

    /// <summary>
    /// Gets the label for total beings count
    /// </summary>
    public abstract string BeingsTotalCount { get; }

    /// <summary>
    /// Gets the placeholder text when no being is selected
    /// </summary>
    public abstract string BeingsNoSelectionPlaceholder { get; }

    /// <summary>
    /// Gets the empty state text when no beings exist
    /// </summary>
    public abstract string BeingsEmptyState { get; }

    /// <summary>
    /// Gets the idle status text
    /// </summary>
    public abstract string BeingsStatusIdle { get; }

    /// <summary>
    /// Gets the running status text
    /// </summary>
    public abstract string BeingsStatusRunning { get; }

    /// <summary>
    /// Gets the ID label in detail view
    /// </summary>
    public abstract string BeingsDetailIdLabel { get; }

    /// <summary>
    /// Gets the status label in detail view
    /// </summary>
    public abstract string BeingsDetailStatusLabel { get; }

    /// <summary>
    /// Gets the custom compilation label in detail view
    /// </summary>
    public abstract string BeingsDetailCustomCompileLabel { get; }

    /// <summary>
    /// Gets the soul content label in detail view
    /// </summary>
    public abstract string BeingsDetailSoulContentLabel { get; }

    /// <summary>
    /// Gets the soul content edit link text in detail view
    /// </summary>
    public abstract string BeingsDetailSoulContentEditLink { get; }

    /// <summary>
    /// Gets the back to list link text
    /// </summary>
    public abstract string BeingsBackToList { get; }

    /// <summary>
    /// Gets the soul editor page subtitle
    /// </summary>
    public abstract string SoulEditorSubtitle { get; }

    /// <summary>
    /// Gets the memory label in detail view
    /// </summary>
    public abstract string BeingsDetailMemoryLabel { get; }

    /// <summary>
    /// Gets the memory view link text in detail view
    /// </summary>
    public abstract string BeingsDetailMemoryViewLink { get; }

    /// <summary>
    /// Gets the permission label in detail view
    /// </summary>
    public abstract string BeingsDetailPermissionLabel { get; }

    /// <summary>
    /// Gets the permission edit link text in detail view
    /// </summary>
    public abstract string BeingsDetailPermissionEditLink { get; }

    /// <summary>
    /// Gets the timers label in detail view
    /// </summary>
    public abstract string BeingsDetailTimersLabel { get; }

    /// <summary>
    /// Gets the tasks label in detail view
    /// </summary>
    public abstract string BeingsDetailTasksLabel { get; }

    /// <summary>
    /// Gets the independent AI client label in detail view
    /// </summary>
    public abstract string BeingsDetailAIClientLabel { get; }

    /// <summary>
    /// Gets the AI client edit link text
    /// </summary>
    public abstract string BeingsDetailAIClientEditLink { get; }

    /// <summary>
    /// Gets the chat history link text in detail view
    /// </summary>
    public abstract string BeingsDetailChatHistoryLink { get; }

    /// <summary>
    /// Gets the chat history label in detail view
    /// </summary>
    public abstract string BeingsDetailChatHistoryLabel { get; }

    /// <summary>
    /// Gets the chat history page title
    /// </summary>
    public abstract string ChatHistoryPageTitle { get; }

    /// <summary>
    /// Gets the chat history page header
    /// </summary>
    public abstract string ChatHistoryPageHeader { get; }

    /// <summary>
    /// Gets the conversation list label
    /// </summary>
    public abstract string ChatHistoryConversationList { get; }

    /// <summary>
    /// Gets the back to conversation list link text
    /// </summary>
    public abstract string ChatHistoryBackToList { get; }

    /// <summary>
    /// Gets the no conversations message
    /// </summary>
    public abstract string ChatHistoryNoConversations { get; }

    /// <summary>
    /// Gets the chat detail page title
    /// </summary>
    public abstract string ChatDetailPageTitle { get; }

    /// <summary>
    /// Gets the chat detail page header
    /// </summary>
    public abstract string ChatDetailPageHeader { get; }

    /// <summary>
    /// Gets the no messages message
    /// </summary>
    public abstract string ChatDetailNoMessages { get; }

    /// <summary>
    /// Gets the "Yes" text for boolean values
    /// </summary>
    public abstract string BeingsYes { get; }

    /// <summary>
    /// Gets the "No" text for boolean values
    /// </summary>
    public abstract string BeingsNo { get; }

    /// <summary>
    /// Gets the "Not set" placeholder text
    /// </summary>
    public abstract string BeingsNotSet { get; }

    // ===== Timers Page Localization =====

    /// <summary>
    /// Gets the timers page header text
    /// </summary>
    public abstract string TimersPageHeader { get; }

    /// <summary>
    /// Gets the label for total timers count
    /// </summary>
    public abstract string TimersTotalCount { get; }

    /// <summary>
    /// Gets the empty state text when no timers exist
    /// </summary>
    public abstract string TimersEmptyState { get; }

    /// <summary>
    /// Gets the view execution history link text
    /// </summary>
    public abstract string TimerViewExecutionHistory { get; }

    /// <summary>
    /// Gets the timer execution history page title
    /// </summary>
    public abstract string TimerExecutionHistoryTitle { get; }

    /// <summary>
    /// Gets the timer execution history page header
    /// </summary>
    public abstract string TimerExecutionHistoryHeader { get; }

    /// <summary>
    /// Gets the back to timers link text
    /// </summary>
    public abstract string TimerExecutionBackToTimers { get; }

    /// <summary>
    /// Gets the timer name label for execution history
    /// </summary>
    public abstract string TimerExecutionTimerName { get; }

    /// <summary>
    /// Gets the timer execution detail page title
    /// </summary>
    public abstract string TimerExecutionDetailTitle { get; }

    /// <summary>
    /// Gets the timer execution detail page header
    /// </summary>
    public abstract string TimerExecutionDetailHeader { get; }

    /// <summary>
    /// Timer execution history empty state message
    /// </summary>
    public abstract string TimerExecutionNoRecords { get; }

    /// <summary>
    /// Gets the active status text
    /// </summary>
    public abstract string TimersStatusActive { get; }

    /// <summary>
    /// Gets the paused status text
    /// </summary>
    public abstract string TimersStatusPaused { get; }

    /// <summary>
    /// Gets the triggered status text
    /// </summary>
    public abstract string TimersStatusTriggered { get; }

    /// <summary>
    /// Gets the cancelled status text
    /// </summary>
    public abstract string TimersStatusCancelled { get; }

    /// <summary>
    /// Gets the recurring timer type text
    /// </summary>
    public abstract string TimersTypeRecurring { get; }

    /// <summary>
    /// Gets the trigger time label
    /// </summary>
    public abstract string TimersTriggerTimeLabel { get; }

    /// <summary>
    /// Gets the interval label
    /// </summary>
    public abstract string TimersIntervalLabel { get; }

    /// <summary>
    /// Gets the calendar description label
    /// </summary>
    public abstract string TimersCalendarLabel { get; }

    /// <summary>
    /// Gets the triggered count label
    /// </summary>
    public abstract string TimersTriggeredCountLabel { get; }

    // ===== Chat Page Localization =====

    /// <summary>
    /// Gets the header text for the conversations sidebar
    /// </summary>
    public abstract string ChatConversationsHeader { get; }

    /// <summary>
    /// Gets the placeholder text when no conversation is selected
    /// </summary>
    public abstract string ChatNoConversationSelected { get; }

    /// <summary>
    /// Gets the placeholder text for the message input
    /// </summary>
    public abstract string ChatMessageInputPlaceholder { get; }

    /// <summary>
    /// Gets the loading text displayed while chat history is being loaded
    /// </summary>
    public abstract string ChatLoading { get; }

    /// <summary>
    /// Gets the text for the send button
    /// </summary>
    public abstract string ChatSendButton { get; }

    /// <summary>
    /// Gets the title for the file source selection dialog
    /// </summary>
    public abstract string ChatFileSourceDialogTitle { get; }

    /// <summary>
    /// Gets the label for selecting server-side file
    /// </summary>
    public abstract string ChatFileSourceServerFile { get; }

    /// <summary>
    /// Gets the label for uploading a local file
    /// </summary>
    public abstract string ChatFileSourceUploadLocal { get; }

    /// <summary>
    /// Gets the display name for the user in chat messages
    /// </summary>
    public abstract string ChatUserDisplayName { get; }

    /// <summary>
    /// Gets the avatar name for the user in chat UI
    /// </summary>
    public abstract string ChatUserAvatarName { get; }

    /// <summary>
    /// Gets the default display name for AI beings
    /// </summary>
    public abstract string ChatDefaultBeingName { get; }

    /// <summary>
    /// Gets the summary text for thinking section
    /// </summary>
    public abstract string ChatThinkingSummary { get; }

    /// <summary>
    /// Gets the summary text for tool calls section
    /// </summary>
    /// <param name="count">The number of tool calls</param>
    /// <returns>The formatted summary text</returns>
    public abstract string GetChatToolCallsSummary(int count);

    // ===== About Page Localization =====

    /// <summary>
    /// Gets the about page header text
    /// </summary>
    public abstract string AboutPageHeader { get; }

    /// <summary>
    /// Gets the application name for about page
    /// </summary>
    public abstract string AboutAppName { get; }

    /// <summary>
    /// Gets the version label for about page
    /// </summary>
    public abstract string AboutVersionLabel { get; }

    /// <summary>
    /// Gets the description text for about page
    /// </summary>
    public abstract string AboutDescription { get; }

    /// <summary>
    /// Gets the author label for about page
    /// </summary>
    public abstract string AboutAuthorLabel { get; }

    /// <summary>
    /// Gets the author name for about page
    /// </summary>
    public abstract string AboutAuthorName { get; }

    /// <summary>
    /// Gets the license label for about page
    /// </summary>
    public abstract string AboutLicenseLabel { get; }

    /// <summary>
    /// Gets the copyright text for about page
    /// </summary>
    public abstract string AboutCopyright { get; }

    /// <summary>
    /// Gets the GitHub repository link text for about page
    /// </summary>
    public abstract string AboutGitHubLink { get; }

    /// <summary>
    /// Gets the Gitee mirror link text for about page
    /// </summary>
    public abstract string AboutGiteeLink { get; }

    /// <summary>
    /// Gets the social media section label for about page
    /// </summary>
    public abstract string AboutSocialMediaLabel { get; }

    /// <summary>
    /// Gets the localized display name for a social media platform
    /// </summary>
    /// <param name="platform">The platform identifier</param>
    /// <returns>The localized display name</returns>
    public abstract string GetSocialMediaName(string platform);

    // ===== Memory Page Localization =====

    /// <summary>
    /// Gets the memory page header text
    /// </summary>
    public abstract string MemoryPageHeader { get; }

    /// <summary>
    /// Gets the empty state text when no memory records exist
    /// </summary>
    public abstract string MemoryEmptyState { get; }

    /// <summary>
    /// Gets the placeholder text for memory search input
    /// </summary>
    public abstract string MemorySearchPlaceholder { get; }

    /// <summary>
    /// Gets the search button text
    /// </summary>
    public abstract string MemorySearchButton { get; }

    /// <summary>
    /// Gets the filter mode text for showing all memories
    /// </summary>
    public abstract string MemoryFilterAll { get; }

    /// <summary>
    /// Gets the filter mode text for showing summaries only
    /// </summary>
    public abstract string MemoryFilterSummaryOnly { get; }

    /// <summary>
    /// Gets the filter mode text for showing original memories only
    /// </summary>
    public abstract string MemoryFilterOriginalOnly { get; }

    /// <summary>
    /// Gets the statistics label for total memories
    /// </summary>
    public abstract string MemoryStatTotal { get; }

    /// <summary>
    /// Gets the statistics label for oldest memory
    /// </summary>
    public abstract string MemoryStatOldest { get; }

    /// <summary>
    /// Gets the statistics label for newest memory
    /// </summary>
    public abstract string MemoryStatNewest { get; }

    /// <summary>
    /// Gets the badge text for summary memories
    /// </summary>
    public abstract string MemoryIsSummaryBadge { get; }

    /// <summary>
    /// Gets the pagination previous button text
    /// </summary>
    public abstract string MemoryPaginationPrev { get; }

    /// <summary>
    /// Gets the pagination next button text
    /// </summary>
    public abstract string MemoryPaginationNext { get; }

    // ===== Projects Page Localization =====

    /// <summary>
    /// Gets the projects page header text
    /// </summary>
    public abstract string ProjectsPageHeader { get; }

    /// <summary>
    /// Gets the empty state text when no projects exist
    /// </summary>
    public abstract string ProjectsEmptyState { get; }

    // ===== Tasks Page Localization =====

    /// <summary>
    /// Gets the tasks page header text
    /// </summary>
    public abstract string TasksPageHeader { get; }

    /// <summary>
    /// Gets the empty state text when no tasks exist
    /// </summary>
    public abstract string TasksEmptyState { get; }

    /// <summary>
    /// Gets the status label for pending tasks
    /// </summary>
    public abstract string TasksStatusPending { get; }

    /// <summary>
    /// Gets the status label for running tasks
    /// </summary>
    public abstract string TasksStatusRunning { get; }

    /// <summary>
    /// Gets the status label for completed tasks
    /// </summary>
    public abstract string TasksStatusCompleted { get; }

    /// <summary>
    /// Gets the status label for failed tasks
    /// </summary>
    public abstract string TasksStatusFailed { get; }

    /// <summary>
    /// Gets the status label for cancelled tasks
    /// </summary>
    public abstract string TasksStatusCancelled { get; }

    /// <summary>
    /// Gets the priority label for tasks
    /// </summary>
    public abstract string TasksPriorityLabel { get; }

    /// <summary>
    /// Gets the assigned-to label for tasks
    /// </summary>
    public abstract string TasksAssignedToLabel { get; }

    /// <summary>
    /// Gets the created-at label for tasks
    /// </summary>
    public abstract string TasksCreatedAtLabel { get; }

    // ===== Code Browser Page Localization =====

    /// <summary>
    /// Gets the code browser page header text
    /// </summary>
    public abstract string CodeBrowserPageHeader { get; }

    // ===== Executor Page Localization =====

    /// <summary>
    /// Gets the executor page header text
    /// </summary>
    public abstract string ExecutorPageHeader { get; }

    // ===== Permission Page Localization =====

    /// <summary>
    /// Gets the permission page header text
    /// </summary>
    public abstract string PermissionPageHeader { get; }

    /// <summary>
    /// Gets the empty state text when no permission rules exist
    /// </summary>
    public abstract string PermissionEmptyState { get; }

    /// <summary>
    /// Gets the error message when being ID is missing
    /// </summary>
    public abstract string PermissionMissingBeingId { get; }

    /// <summary>
    /// Gets the error message when being is not found
    /// </summary>
    public abstract string PermissionBeingNotFound { get; }

    /// <summary>
    /// Gets the default permission template header title
    /// </summary>
    public abstract string PermissionTemplateHeader { get; }

    /// <summary>
    /// Gets the default permission template description
    /// </summary>
    public abstract string PermissionTemplateDescription { get; }

    /// <summary>
    /// Gets the XML doc: class summary for DefaultPermissionCallback
    /// </summary>
    public abstract string PermissionCallbackClassSummary { get; }

    /// <summary>
    /// Gets the XML doc: class summary line 2 for DefaultPermissionCallback
    /// </summary>
    public abstract string PermissionCallbackClassSummary2 { get; }

    /// <summary>
    /// Gets the XML doc: constructor summary for DefaultPermissionCallback
    /// </summary>
    public abstract string PermissionCallbackConstructorSummary { get; }

    /// <summary>
    /// Gets the XML doc: constructor summary line 2 for DefaultPermissionCallback
    /// </summary>
    public abstract string PermissionCallbackConstructorSummary2 { get; }

    /// <summary>
    /// Gets the XML doc: constructor param description for appDataDirectory
    /// </summary>
    public abstract string PermissionCallbackConstructorParam { get; }

    /// <summary>
    /// Gets the XML doc: Evaluate method summary for DefaultPermissionCallback
    /// </summary>
    public abstract string PermissionCallbackEvaluateSummary { get; }

    /// <summary>
    /// Gets the fallback comment for "other permission types default to allowed"
    /// </summary>
    public abstract string PermissionRuleOtherTypesDefault { get; }

    /// <summary>
    /// Gets the localized comment for a permission rule by key.
    /// Returns the key itself if not found (used as fallback bilingual comment).
    /// </summary>
    public virtual string GetPermissionRuleComment(string key) => key;

    /// <summary>
    /// Gets the section title for permission rules list
    /// </summary>
    public abstract string PermissionRulesSection { get; }

    /// <summary>
    /// Gets the section title for permission editor
    /// </summary>
    public abstract string PermissionEditorSection { get; }

    /// <summary>
    /// Gets the error message when permission save fails due to missing being ID
    /// </summary>
    public abstract string PermissionSaveMissingBeingId { get; }

    /// <summary>
    /// Gets the error message when permission save fails due to missing code
    /// </summary>
    public abstract string PermissionSaveMissingCode { get; }

    /// <summary>
    /// Gets the error message when DynamicBeingLoader is not available
    /// </summary>
    public abstract string PermissionSaveLoaderNotAvailable { get; }

    /// <summary>
    /// Gets the error message when permission callback removal fails
    /// </summary>
    public abstract string PermissionSaveRemoveFailed { get; }

    /// <summary>
    /// Gets the success message when permission callback is removed
    /// </summary>
    public abstract string PermissionSaveRemoveSuccess { get; }

    /// <summary>
    /// Gets the error message when permission save fails due to security scan
    /// </summary>
    public abstract string PermissionSaveSecurityScanFailed { get; }

    /// <summary>
    /// Gets the error message when permission compilation fails
    /// </summary>
    public abstract string PermissionSaveCompilationFailed { get; }

    /// <summary>
    /// Gets the success message when permission callback is saved successfully
    /// </summary>
    public abstract string PermissionSaveSuccess { get; }

    /// <summary>
    /// Gets the generic error message when permission save throws an exception
    /// </summary>
    public abstract string PermissionSaveError { get; }

    // ===== Knowledge Page Localization =====

    /// <summary>
    /// Gets the knowledge page header text
    /// </summary>
    public abstract string KnowledgePageHeader { get; }

    /// <summary>
    /// Gets the loading state text for knowledge graph
    /// </summary>
    public abstract string KnowledgeLoadingState { get; }

    // ===== Config Page Localization =====

    /// <summary>
    /// Gets the config page header text
    /// </summary>
    public abstract string ConfigPageHeader { get; }

    /// <summary>
    /// Gets the label for property name column header
    /// </summary>
    public abstract string ConfigPropertyNameLabel { get; }

    /// <summary>
    /// Gets the label for property value column header
    /// </summary>
    public abstract string ConfigPropertyValueLabel { get; }

    /// <summary>
    /// Gets the label for action column header
    /// </summary>
    public abstract string ConfigActionLabel { get; }

    /// <summary>
    /// Gets the text for the edit button
    /// </summary>
    public abstract string ConfigEditButton { get; }

    /// <summary>
    /// Gets the title for the edit modal
    /// </summary>
    public abstract string ConfigEditModalTitle { get; }

    /// <summary>
    /// Gets the label for property name in edit form
    /// </summary>
    public abstract string ConfigEditPropertyLabel { get; }

    /// <summary>
    /// Gets the label for property value in edit form
    /// </summary>
    public abstract string ConfigEditValueLabel { get; }

    /// <summary>
    /// Gets the text for the browse button
    /// </summary>
    public abstract string ConfigBrowseButton { get; }

    /// <summary>
    /// Gets the label for time settings section
    /// </summary>
    public abstract string ConfigTimeSettingsLabel { get; }

    /// <summary>
    /// Gets the label for days input
    /// </summary>
    public abstract string ConfigDaysLabel { get; }

    /// <summary>
    /// Gets the label for hours input
    /// </summary>
    public abstract string ConfigHoursLabel { get; }

    /// <summary>
    /// Gets the label for minutes input
    /// </summary>
    public abstract string ConfigMinutesLabel { get; }

    /// <summary>
    /// Gets the label for seconds input
    /// </summary>
    public abstract string ConfigSecondsLabel { get; }

    /// <summary>
    /// Gets the text for the save button
    /// </summary>
    public abstract string ConfigSaveButton { get; }

    /// <summary>
    /// Gets the text for the cancel button
    /// </summary>
    public abstract string ConfigCancelButton { get; }

    /// <summary>
    /// Gets the display text for null values
    /// </summary>
    public abstract string ConfigNullValue { get; }

    /// <summary>
    /// Gets the prefix text for edit modal title
    /// </summary>
    public abstract string ConfigEditPrefix { get; }

    /// <summary>
    /// Gets the default group name for ungrouped config items
    /// </summary>
    public abstract string ConfigDefaultGroupName { get; }

    /// <summary>
    /// Gets the error message for invalid request parameters
    /// </summary>
    public abstract string ConfigErrorInvalidRequest { get; }

    /// <summary>
    /// Gets the error message when config instance does not exist
    /// </summary>
    public abstract string ConfigErrorInstanceNotFound { get; }

    /// <summary>
    /// Gets the error message when property does not exist or is not writable
    /// </summary>
    public abstract string ConfigErrorPropertyNotFound { get; }

    /// <summary>
    /// Gets the error message for integer conversion failure
    /// </summary>
    public abstract string ConfigErrorConvertInt { get; }

    /// <summary>
    /// Gets the error message for long integer conversion failure
    /// </summary>
    public abstract string ConfigErrorConvertLong { get; }

    /// <summary>
    /// Gets the error message for double conversion failure
    /// </summary>
    public abstract string ConfigErrorConvertDouble { get; }

    /// <summary>
    /// Gets the error message for boolean conversion failure
    /// </summary>
    public abstract string ConfigErrorConvertBool { get; }

    /// <summary>
    /// Gets the error message for GUID conversion failure
    /// </summary>
    public abstract string ConfigErrorConvertGuid { get; }

    /// <summary>
    /// Gets the error message for TimeSpan conversion failure
    /// </summary>
    public abstract string ConfigErrorConvertTimeSpan { get; }

    /// <summary>
    /// Gets the error message for DateTime conversion failure
    /// </summary>
    public abstract string ConfigErrorConvertDateTime { get; }

    /// <summary>
    /// Gets the error message for enum conversion failure
    /// </summary>
    public abstract string ConfigErrorConvertEnum { get; }

    /// <summary>
    /// Gets the error message for unsupported property type
    /// </summary>
    public abstract string ConfigErrorUnsupportedType { get; }

    /// <summary>
    /// Gets the error message for save failure
    /// </summary>
    public abstract string ConfigErrorSaveFailed { get; }

    /// <summary>
    /// Gets the error message prefix for save failure in alert dialog
    /// </summary>
    public abstract string ConfigSaveFailed { get; }

    /// <summary>
    /// Gets the label for dictionary type
    /// </summary>
    public abstract string ConfigDictionaryLabel { get; }

    /// <summary>
    /// Gets the label for dictionary key input
    /// </summary>
    public abstract string ConfigDictKeyLabel { get; }

    /// <summary>
    /// Gets the label for dictionary value input
    /// </summary>
    public abstract string ConfigDictValueLabel { get; }

    /// <summary>
    /// Gets the text for add button in dictionary editor
    /// </summary>
    public abstract string ConfigDictAddButton { get; }

    /// <summary>
    /// Gets the text for delete button in dictionary editor
    /// </summary>
    public abstract string ConfigDictDeleteButton { get; }

    /// <summary>
    /// Gets the message when dictionary is empty
    /// </summary>
    public abstract string ConfigDictEmptyMessage { get; }

    // ===== Logs Page Localization =====

    /// <summary>
    /// Gets the logs page header text
    /// </summary>
    public abstract string LogsPageHeader { get; }

    /// <summary>
    /// Gets the label for total logs count
    /// </summary>
    public abstract string LogsTotalCount { get; }

    /// <summary>
    /// Gets the label for start time filter
    /// </summary>
    public abstract string LogsStartTime { get; }

    /// <summary>
    /// Gets the label for end time filter
    /// </summary>
    public abstract string LogsEndTime { get; }

    /// <summary>
    /// Gets the text for "All Levels" option in filter
    /// </summary>
    public abstract string LogsLevelAll { get; }

    /// <summary>
    /// Gets the label for being filter
    /// </summary>
    public abstract string LogsBeingFilter { get; }

    /// <summary>
    /// Gets the text for "All Beings" option in filter
    /// </summary>
    public abstract string LogsAllBeings { get; }

    /// <summary>
    /// Gets the text for "System Only" option in filter
    /// </summary>
    public abstract string LogsSystemOnly { get; }

    /// <summary>
    /// Gets the text for the filter button
    /// </summary>
    public abstract string LogsFilterButton { get; }

    /// <summary>
    /// Gets the empty state text when no logs exist
    /// </summary>
    public abstract string LogsEmptyState { get; }

    /// <summary>
    /// Gets the label for exception details
    /// </summary>
    public abstract string LogsExceptionLabel { get; }

    /// <summary>
    /// Gets the text for previous page button
    /// </summary>
    public abstract string LogsPrevPage { get; }

    /// <summary>
    /// Gets the text for next page button
    /// </summary>
    public abstract string LogsNextPage { get; }

    // ===== Audit Page Localization =====

    /// <summary>
    /// Gets the audit page header text
    /// </summary>
    public abstract string AuditPageHeader { get; }

    /// <summary>
    /// Gets the label for total token count
    /// </summary>
    public abstract string AuditTotalTokens { get; }

    /// <summary>
    /// Gets the label for total requests
    /// </summary>
    public abstract string AuditTotalRequests { get; }

    /// <summary>
    /// Gets the label for success count
    /// </summary>
    public abstract string AuditSuccessCount { get; }

    /// <summary>
    /// Gets the label for failure count
    /// </summary>
    public abstract string AuditFailureCount { get; }

    /// <summary>
    /// Gets the label for prompt tokens
    /// </summary>
    public abstract string AuditPromptTokens { get; }

    /// <summary>
    /// Gets the label for completion tokens
    /// </summary>
    public abstract string AuditCompletionTokens { get; }

    /// <summary>
    /// Gets the label for start time filter
    /// </summary>
    public abstract string AuditStartTime { get; }

    /// <summary>
    /// Gets the label for end time filter
    /// </summary>
    public abstract string AuditEndTime { get; }

    /// <summary>
    /// Gets the text for the filter button
    /// </summary>
    public abstract string AuditFilterButton { get; }

    /// <summary>
    /// Gets the empty state text when no audit records exist
    /// </summary>
    public abstract string AuditEmptyState { get; }

    /// <summary>
    /// Gets the label for AI client type filter
    /// </summary>
    public abstract string AuditAIClientType { get; }

    /// <summary>
    /// Gets the text for "All Types" option in AI client type filter
    /// </summary>
    public abstract string AuditAllClientTypes { get; }

    /// <summary>
    /// Gets the label for grouping by AI client type
    /// </summary>
    public abstract string AuditGroupByClient { get; }

    /// <summary>
    /// Gets the label for grouping by being
    /// </summary>
    public abstract string AuditGroupByBeing { get; }

    /// <summary>
    /// Gets the text for previous page button
    /// </summary>
    public abstract string AuditPrevPage { get; }

    /// <summary>
    /// Gets the text for next page button
    /// </summary>
    public abstract string AuditNextPage { get; }

    /// <summary>
    /// Gets the label for silicon being filter
    /// </summary>
    public abstract string AuditBeing { get; }

    /// <summary>
    /// Gets the text for "All Beings" option in being filter
    /// </summary>
    public abstract string AuditAllBeings { get; }

    /// <summary>
    /// Gets the text for "Today" time range button
    /// </summary>
    public abstract string AuditTimeToday { get; }

    /// <summary>
    /// Gets the text for "This Week" time range button
    /// </summary>
    public abstract string AuditTimeWeek { get; }

    /// <summary>
    /// Gets the text for "This Month" time range button
    /// </summary>
    public abstract string AuditTimeMonth { get; }

    /// <summary>
    /// Gets the text for "This Year" time range button
    /// </summary>
    public abstract string AuditTimeYear { get; }

    /// <summary>
    /// Gets the text for the export button
    /// </summary>
    public abstract string AuditExport { get; }

    /// <summary>
    /// Gets the title for the token usage trend chart
    /// </summary>
    public abstract string AuditTrendTitle { get; }

    /// <summary>
    /// Gets the legend label for prompt tokens in trend chart
    /// </summary>
    public abstract string AuditTrendPrompt { get; }

    /// <summary>
    /// Gets the legend label for completion tokens in trend chart
    /// </summary>
    public abstract string AuditTrendCompletion { get; }

    /// <summary>
    /// Gets the legend label for total tokens in trend chart
    /// </summary>
    public abstract string AuditTrendTotal { get; }

    /// <summary>
    /// Gets the tooltip label for date in chart hover
    /// </summary>
    public abstract string AuditTooltipDate { get; }

    /// <summary>
    /// Gets the tooltip label for prompt tokens in chart hover
    /// </summary>
    public abstract string AuditTooltipPrompt { get; }

    /// <summary>
    /// Gets the tooltip label for completion tokens in chart hover
    /// </summary>
    public abstract string AuditTooltipCompletion { get; }

    /// <summary>
    /// Gets the tooltip label for total tokens in chart hover
    /// </summary>
    public abstract string AuditTooltipTotal { get; }

    /// <summary>
    /// Gets the localized display name for a configuration group
    /// </summary>
    /// <param name="groupKey">The group localization key</param>
    /// <returns>The localized display name, or the key itself if not found</returns>
    public abstract string GetConfigGroupName(string groupKey);

    /// <summary>
    /// Gets the localized display name for a configuration property
    /// </summary>
    /// <param name="displayNameKey">The display name localization key</param>
    /// <param name="found">Whether the localized value was found</param>
    /// <returns>The localized display name, or the key itself if not found</returns>
    public abstract string GetConfigDisplayName(string displayNameKey, out bool found);

    /// <summary>
    /// Gets the localized description for a configuration property
    /// </summary>
    /// <param name="descriptionKey">The description localization key</param>
    /// <returns>The localized description, or null if not found</returns>
    public abstract string? GetConfigDescription(string descriptionKey);

    /// <summary>
    /// Gets the localized display name for a log level
    /// </summary>
    /// <param name="logLevel">The log level</param>
    /// <returns>The localized display name</returns>
    public abstract override string GetLogLevelName(LogLevel logLevel);

    // ===== Tool Display Name Localization =====

    /// <summary>
    /// Gets the localized display name for a tool by its name identifier
    /// </summary>
    /// <param name="toolName">The tool's Name property value</param>
    /// <returns>The localized display name, or the tool name itself if not found</returns>
    public abstract string GetToolDisplayName(string toolName);

    // ===== Curator Soul =====

    /// <summary>
    /// Gets the default soul content for the Silicon Curator.
    /// Subclasses may override to provide a localized version.
    /// </summary>
    public abstract string DefaultCuratorSoul { get; }

    // ===== Interval Calendar Localization =====

    /// <summary>
    /// Gets the display name of the Interval (time span) calendar system.
    /// This is a virtual calendar used for recurring timers based on time intervals.
    /// </summary>
    public abstract string CalendarIntervalName { get; }

    /// <summary>
    /// Gets the localized label for the days component in interval.
    /// </summary>
    public abstract string CalendarIntervalDays { get; }

    /// <summary>
    /// Gets the localized label for the hours component in interval.
    /// </summary>
    public abstract string CalendarIntervalHours { get; }

    /// <summary>
    /// Gets the localized label for the minutes component in interval.
    /// </summary>
    public abstract string CalendarIntervalMinutes { get; }

    /// <summary>
    /// Gets the localized label for the seconds component in interval.
    /// </summary>
    public abstract string CalendarIntervalSeconds { get; }

    /// <summary>
    /// Gets the localized label for "every" in interval description.
    /// </summary>
    public abstract string CalendarIntervalEvery { get; }

    /// <summary>
    /// Formats a full interval description into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeIntervalDescription(int days, int hours, int minutes, int seconds);

    // ===== Gregorian Calendar Localization =====

    /// <summary>
    /// Gets the display name of the Gregorian calendar system.
    /// </summary>
    public abstract string CalendarGregorianName { get; }

    /// <summary>
    /// Gets the localized label for the year component.
    /// </summary>
    public abstract string CalendarComponentYear { get; }

    /// <summary>
    /// Gets the localized label for the month component.
    /// </summary>
    public abstract string CalendarComponentMonth { get; }

    /// <summary>
    /// Gets the localized label for the day component.
    /// </summary>
    public abstract string CalendarComponentDay { get; }

    /// <summary>
    /// Gets the localized label for the hour component.
    /// </summary>
    public abstract string CalendarComponentHour { get; }

    /// <summary>
    /// Gets the localized label for the minute component.
    /// </summary>
    public abstract string CalendarComponentMinute { get; }

    /// <summary>
    /// Gets the localized label for the second component.
    /// </summary>
    public abstract string CalendarComponentSecond { get; }

    /// <summary>
    /// Gets the localized label for the weekday component.
    /// </summary>
    public abstract string CalendarComponentWeekday { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–12).
    /// </summary>
    /// <param name="month">Month number, 1 = January … 12 = December.</param>
    /// <returns>The localized month name, or <see langword="null"/> if not found.</returns>
    public abstract string? GetGregorianMonthName(int month);

    /// <summary>Returns a localized string for the given year value (e.g. "2026" or "2026年").</summary>
    public abstract string FormatGregorianYear(int year);

    /// <summary>Returns a localized string for the given day value (e.g. "16" or "16日").</summary>
    public abstract string FormatGregorianDay(int day);

    /// <summary>Returns a localized string for the given hour value (e.g. "08" or "8时").</summary>
    public abstract string FormatGregorianHour(int hour);

    /// <summary>Returns a localized string for the given minute value (e.g. "30" or "30分").</summary>
    public abstract string FormatGregorianMinute(int minute);

    /// <summary>Returns a localized string for the given second value (e.g. "00" or "0秒").</summary>
    public abstract string FormatGregorianSecond(int second);

    /// <summary>Returns a localized weekday name for the given <see cref="System.DayOfWeek"/> value (0 = Sunday … 6 = Saturday).</summary>
    /// <returns>The localized weekday name, or <see langword="null"/> if not found.</returns>
    public abstract string? GetGregorianWeekdayName(int dayOfWeek);

    /// <summary>
    /// Formats a full Gregorian date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeGregorianDateTime(int year, int month, int day, int hour, int minute, int second);

    // ===== Buddhist Calendar Localization =====

    /// <summary>
    /// Gets the display name of the Buddhist Era calendar system.
    /// </summary>
    public abstract string CalendarBuddhistName { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–12) in the Buddhist calendar.
    /// </summary>
    /// <param name="month">Month number, 1 = January … 12 = December.</param>
    /// <returns>The localized month name, or <see langword="null"/> if not found.</returns>
    public abstract string? GetBuddhistMonthName(int month);

    /// <summary>Returns a localized string for the given Buddhist Era year value (e.g. "2569" or "佛元2569年").</summary>
    public abstract string FormatBuddhistYear(int year);

    /// <summary>Returns a localized string for the given day value in the Buddhist calendar (e.g. "16" or "16日").</summary>
    public abstract string FormatBuddhistDay(int day);

    /// <summary>
    /// Formats a full Buddhist Era date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeBuddhistDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Cherokee Calendar =====

    /// <summary>
    /// Gets the display name of the Cherokee traditional calendar system.
    /// </summary>
    public abstract string CalendarCherokeeName { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–13) in the Cherokee calendar.
    /// </summary>
    /// <param name="month">Month number, 1–13.</param>
    /// <returns>The localized month name, or <see langword="null"/> if not found.</returns>
    public abstract string? GetCherokeeMonthName(int month);

    /// <summary>Returns a localized string for the given Cherokee year value.</summary>
    public abstract string FormatCherokeeYear(int year);

    /// <summary>Returns a localized string for the given day value in the Cherokee calendar.</summary>
    public abstract string FormatCherokeeDay(int day);

    /// <summary>
    /// Formats a full Cherokee date into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeCherokeeDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Juche Calendar =====

    /// <summary>
    /// Gets the display name of the Juche calendar system.
    /// </summary>
    public abstract string CalendarJucheName { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–12) in the Juche calendar.
    /// </summary>
    public abstract string? GetJucheMonthName(int month);

    /// <summary>Returns a localized string for the given Juche year value.</summary>
    public abstract string FormatJucheYear(int year);

    /// <summary>Returns a localized string for the given day value in the Juche calendar.</summary>
    public abstract string FormatJucheDay(int day);

    /// <summary>
    /// Formats a full Juche date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeJucheDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Republic of China Calendar =====

    /// <summary>
    /// Gets the display name of the Republic of China (Minguo) calendar system.
    /// </summary>
    public abstract string CalendarRocName { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–12) in the ROC calendar.
    /// </summary>
    public abstract string? GetRocMonthName(int month);

    /// <summary>Returns a localized string for the given ROC year value.</summary>
    public abstract string FormatRocYear(int year);

    /// <summary>Returns a localized string for the given day value in the ROC calendar.</summary>
    public abstract string FormatRocDay(int day);

    /// <summary>
    /// Formats a full ROC date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeRocDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Chinese Historical Calendar =====

    /// <summary>
    /// Gets the display name of the Chinese Historical Calendar system.
    /// </summary>
    public abstract string CalendarChineseHistoricalName { get; }

    /// <summary>
    /// Gets the localized component name for Dynasty.
    /// </summary>
    public abstract string CalendarComponentDynasty { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–12) in the Chinese Historical calendar.
    /// </summary>
    public abstract string? GetChineseHistoricalMonthName(int month);

    /// <summary>Returns a localized string for the given day value in the Chinese Historical calendar.</summary>
    public abstract string FormatChineseHistoricalDay(int day);

    /// <summary>
    /// Gets the Chinese historical localization provider for dynasty and era name translations.
    /// </summary>
    /// <returns>
    /// An instance of ChineseHistoricalLocalizationBase implementation that provides
    /// localized names for all 28 dynasties and 356 era names.
    /// </returns>
    public abstract ChineseHistoricalLocalizationBase GetChineseHistoricalLocalization();

    // ===== Chula Sakarat Calendar =====

    /// <summary>
    /// Gets the display name of the Chula Sakarat (CS) calendar system.
    /// </summary>
    public abstract string CalendarChulaSakaratName { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–12) in the Chula Sakarat calendar.
    /// </summary>
    public abstract string? GetChulaSakaratMonthName(int month);

    /// <summary>Returns a localized string for the given Chula Sakarat year value.</summary>
    public abstract string FormatChulaSakaratYear(int year);

    /// <summary>Returns a localized string for the given day value in the Chula Sakarat calendar.</summary>
    public abstract string FormatChulaSakaratDay(int day);

    /// <summary>
    /// Formats a full Chula Sakarat date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeChulaSakaratDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Julian Calendar =====

    /// <summary>
    /// Gets the display name of the Julian calendar system.
    /// </summary>
    public abstract string CalendarJulianName { get; }

    /// <summary>Returns a localized string for the given Julian year value.</summary>
    public abstract string FormatJulianYear(int year);

    /// <summary>Returns a localized string for the given day value in the Julian calendar.</summary>
    public abstract string FormatJulianDay(int day);

    /// <summary>
    /// Formats a full Julian date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeJulianDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Khmer Calendar =====

    /// <summary>
    /// Gets the display name of the Khmer calendar system.
    /// </summary>
    public abstract string CalendarKhmerName { get; }

    /// <summary>Returns a localized string for the given Khmer year value.</summary>
    public abstract string FormatKhmerYear(int year);

    /// <summary>Returns a localized string for the given day value in the Khmer calendar.</summary>
    public abstract string FormatKhmerDay(int day);

    /// <summary>
    /// Formats a full Khmer date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeKhmerDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Zoroastrian Calendar =====

    /// <summary>
    /// Gets the display name of the Zoroastrian (Yazdegerd) calendar system.
    /// </summary>
    public abstract string CalendarZoroastrianName { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–13) in the Zoroastrian calendar.
    /// </summary>
    public abstract string? GetZoroastrianMonthName(int month);

    /// <summary>Returns a localized string for the given Zoroastrian year value.</summary>
    public abstract string FormatZoroastrianYear(int year);

    /// <summary>Returns a localized string for the given day value in the Zoroastrian calendar.</summary>
    public abstract string FormatZoroastrianDay(int day);

    /// <summary>
    /// Formats a full Zoroastrian date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeZoroastrianDate(int year, int month, int day, int hour, int minute, int second);

    // ===== French Republican Calendar =====

    /// <summary>
    /// Gets the display name of the French Republican calendar system.
    /// </summary>
    public abstract string CalendarFrenchRepublicanName { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–13) in the French Republican calendar.
    /// </summary>
    public abstract string? GetFrenchRepublicanMonthName(int month);

    /// <summary>Returns a localized string for the given French Republican year value.</summary>
    public abstract string FormatFrenchRepublicanYear(int year);

    /// <summary>Returns a localized string for the given day value in the French Republican calendar.</summary>
    public abstract string FormatFrenchRepublicanDay(int day);

    /// <summary>
    /// Formats a full French Republican date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeFrenchRepublicanDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Coptic Calendar =====

    /// <summary>
    /// Gets the display name of the Coptic calendar system.
    /// </summary>
    public abstract string CalendarCopticName { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–13) in the Coptic calendar.
    /// </summary>
    public abstract string? GetCopticMonthName(int month);

    /// <summary>Returns a localized string for the given Coptic year value.</summary>
    public abstract string FormatCopticYear(int year);

    /// <summary>Returns a localized string for the given day value in the Coptic calendar.</summary>
    public abstract string FormatCopticDay(int day);

    /// <summary>
    /// Formats a full Coptic date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeCopticDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Ethiopian Calendar =====

    /// <summary>
    /// Gets the display name of the Ethiopian calendar system.
    /// </summary>
    public abstract string CalendarEthiopianName { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–13) in the Ethiopian calendar.
    /// </summary>
    public abstract string? GetEthiopianMonthName(int month);

    /// <summary>Returns a localized string for the given Ethiopian year value.</summary>
    public abstract string FormatEthiopianYear(int year);

    /// <summary>Returns a localized string for the given day value in the Ethiopian calendar.</summary>
    public abstract string FormatEthiopianDay(int day);

    /// <summary>
    /// Formats a full Ethiopian date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeEthiopianDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Islamic Calendar =====

    /// <summary>
    /// Gets the display name of the Islamic (Hijri) calendar system.
    /// </summary>
    public abstract string CalendarIslamicName { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–12) in the Islamic calendar.
    /// </summary>
    public abstract string? GetIslamicMonthName(int month);

    /// <summary>Returns a localized string for the given Islamic year value.</summary>
    public abstract string FormatIslamicYear(int year);

    /// <summary>Returns a localized string for the given day value in the Islamic calendar.</summary>
    public abstract string FormatIslamicDay(int day);

    /// <summary>
    /// Formats a full Islamic date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeIslamicDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Hebrew Calendar =====

    /// <summary>
    /// Gets the display name of the Hebrew calendar system.
    /// </summary>
    public abstract string CalendarHebrewName { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–13) in the Hebrew calendar.
    /// </summary>
    public abstract string? GetHebrewMonthName(int month);

    /// <summary>Returns a localized string for the given Hebrew year value.</summary>
    public abstract string FormatHebrewYear(int year);

    /// <summary>Returns a localized string for the given day value in the Hebrew calendar.</summary>
    public abstract string FormatHebrewDay(int day);

    /// <summary>
    /// Formats a full Hebrew date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeHebrewDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Persian Calendar =====

    /// <summary>
    /// Gets the display name of the Persian (Solar Hijri) calendar system.
    /// </summary>
    public abstract string CalendarPersianName { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–12) in the Persian calendar.
    /// </summary>
    public abstract string? GetPersianMonthName(int month);

    /// <summary>Returns a localized string for the given Persian year value.</summary>
    public abstract string FormatPersianYear(int year);

    /// <summary>Returns a localized string for the given day value in the Persian calendar.</summary>
    public abstract string FormatPersianDay(int day);

    /// <summary>
    /// Formats a full Persian date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizePersianDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Indian National Calendar =====

    /// <summary>
    /// Gets the display name of the Indian National (Saka) calendar system.
    /// </summary>
    public abstract string CalendarIndianName { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–12) in the Indian calendar.
    /// </summary>
    public abstract string? GetIndianMonthName(int month);

    /// <summary>Returns a localized string for the given Indian year value.</summary>
    public abstract string FormatIndianYear(int year);

    /// <summary>Returns a localized string for the given day value in the Indian calendar.</summary>
    public abstract string FormatIndianDay(int day);

    /// <summary>
    /// Formats a full Indian date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeIndianDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Saka Era Calendar =====

    /// <summary>
    /// Gets the display name of the Saka Era calendar system.
    /// </summary>
    public abstract string CalendarSakaName { get; }

    /// <summary>Returns a localized string for the given Saka year value.</summary>
    public abstract string FormatSakaYear(int year);

    /// <summary>Returns a localized string for the given day value in the Saka calendar.</summary>
    public abstract string FormatSakaDay(int day);

    /// <summary>
    /// Formats a full Saka date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeSakaDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Vikram Samvat Calendar =====

    /// <summary>
    /// Gets the display name of the Vikram Samvat calendar system.
    /// </summary>
    public abstract string CalendarVikramSamvatName { get; }

    /// <summary>Returns a localized string for the given Vikram Samvat year value.</summary>
    public abstract string FormatVikramSamvatYear(int year);

    /// <summary>Returns a localized string for the given day value in the Vikram Samvat calendar.</summary>
    public abstract string FormatVikramSamvatDay(int day);

    /// <summary>
    /// Formats a full Vikram Samvat date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeVikramSamvatDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Mongolian Calendar =====

    /// <summary>
    /// Gets the display name of the Mongolian calendar system.
    /// </summary>
    public abstract string CalendarMongolianName { get; }

    /// <summary>Returns a localized string for the given Mongolian year value.</summary>
    public abstract string FormatMongolianYear(int year);

    /// <summary>Returns a localized string for the given month value in the Mongolian calendar.</summary>
    public abstract string FormatMongolianMonth(int month);

    /// <summary>Returns a localized string for the given day value in the Mongolian calendar.</summary>
    public abstract string FormatMongolianDay(int day);

    /// <summary>
    /// Formats a full Mongolian date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeMongolianDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Javanese Calendar =====

    /// <summary>
    /// Gets the display name of the Javanese calendar system.
    /// </summary>
    public abstract string CalendarJavaneseName { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–12) in the Javanese calendar.
    /// </summary>
    public abstract string? GetJavaneseMonthName(int month);

    /// <summary>Returns a localized string for the given Javanese year value.</summary>
    public abstract string FormatJavaneseYear(int year);

    /// <summary>Returns a localized string for the given day value in the Javanese calendar.</summary>
    public abstract string FormatJavaneseDay(int day);

    /// <summary>
    /// Formats a full Javanese date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeJavaneseDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Tibetan Calendar =====

    /// <summary>
    /// Gets the display name of the Tibetan calendar system.
    /// </summary>
    public abstract string CalendarTibetanName { get; }

    /// <summary>Returns a localized string for the given Tibetan year value.</summary>
    public abstract string FormatTibetanYear(int year);

    /// <summary>Returns a localized string for the given month value in the Tibetan calendar.</summary>
    public abstract string FormatTibetanMonth(int month);

    /// <summary>Returns a localized string for the given day value in the Tibetan calendar.</summary>
    public abstract string FormatTibetanDay(int day);

    /// <summary>
    /// Formats a full Tibetan date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeTibetanDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Mayan Calendar =====

    /// <summary>
    /// Gets the display name of the Mayan Long Count calendar system.
    /// </summary>
    public abstract string CalendarMayanName { get; }

    /// <summary>Gets the localized label for the Baktun component.</summary>
    public abstract string CalendarMayanBaktun { get; }

    /// <summary>Gets the localized label for the Katun component.</summary>
    public abstract string CalendarMayanKatun { get; }

    /// <summary>Gets the localized label for the Tun component.</summary>
    public abstract string CalendarMayanTun { get; }

    /// <summary>Gets the localized label for the Uinal component.</summary>
    public abstract string CalendarMayanUinal { get; }

    /// <summary>Gets the localized label for the Kin component.</summary>
    public abstract string CalendarMayanKin { get; }

    /// <summary>
    /// Formats a full Mayan date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeMayanDate(int baktun, int katun, int tun, int uinal, int kin, int hour, int minute, int second);

    // ===== Inuit Calendar =====

    /// <summary>
    /// Gets the display name of the Inuit traditional calendar system.
    /// </summary>
    public abstract string CalendarInuitName { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–13) in the Inuit calendar.
    /// </summary>
    public abstract string? GetInuitMonthName(int month);

    /// <summary>Returns a localized string for the given Inuit year value.</summary>
    public abstract string FormatInuitYear(int year);

    /// <summary>Returns a localized string for the given day value in the Inuit calendar.</summary>
    public abstract string FormatInuitDay(int day);

    /// <summary>
    /// Formats a full Inuit date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeInuitDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Roman Calendar =====

    /// <summary>
    /// Gets the display name of the Roman calendar system.
    /// </summary>
    public abstract string CalendarRomanName { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–12) in the Roman calendar.
    /// </summary>
    public abstract string? GetRomanMonthName(int month);

    /// <summary>Returns a localized string for the given Roman year value (Julian year, displayed as AUC).</summary>
    public abstract string FormatRomanYear(int year);

    /// <summary>Returns a localized string for the given day value in the Roman calendar.</summary>
    public abstract string FormatRomanDay(int day);

    /// <summary>
    /// Formats a full Roman date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeRomanDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Chinese Lunar Calendar =====

    /// <summary>
    /// Gets the display name of the Chinese Lunar calendar system.
    /// </summary>
    public abstract string CalendarChineseLunarName { get; }

    /// <summary>
    /// Gets the localized month name for the given month number (1–12) in the Chinese Lunar calendar.
    /// </summary>
    public abstract string? GetChineseLunarMonthName(int month);

    /// <summary>
    /// Gets the localized day name for the given day number (1–30) in the Chinese Lunar calendar.
    /// </summary>
    public abstract string? GetChineseLunarDayName(int day);

    /// <summary>Gets the localized leap month prefix (e.g. "闰" / "閏").</summary>
    public abstract string ChineseLunarLeapPrefix { get; }

    /// <summary>Gets the localized label for the leap month indicator component.</summary>
    public abstract string CalendarComponentIsLeap { get; }

    /// <summary>Returns a localized string for the given Chinese Lunar year value.</summary>
    public abstract string FormatChineseLunarYear(int year);

    /// <summary>
    /// Formats a full Chinese Lunar date-time into a localized human-readable string.
    /// </summary>
    public abstract string LocalizeChineseLunarDate(int year, int month, int day, bool isLeap, int hour, int minute, int second);

    // ===== Vietnamese Calendar =====

    /// <summary>Gets the display name of the Vietnamese Lunar calendar system.</summary>
    public abstract string CalendarVietnameseName { get; }

    /// <summary>Gets the localized month name for the given month number (1–12) in the Vietnamese calendar.</summary>
    public abstract string? GetVietnameseMonthName(int month);

    /// <summary>Gets the localized zodiac name for the given zodiac index (0–11).</summary>
    public abstract string? GetVietnameseZodiacName(int index);

    /// <summary>Gets the localized leap month prefix.</summary>
    public abstract string VietnameseLeapPrefix { get; }

    /// <summary>Gets the localized label for the zodiac component.</summary>
    public abstract string CalendarComponentZodiac { get; }

    /// <summary>Returns a localized string for the given Vietnamese year value.</summary>
    public abstract string FormatVietnameseYear(int year);

    /// <summary>Returns a localized string for the given day value in the Vietnamese calendar.</summary>
    public abstract string FormatVietnameseDay(int day);

    /// <summary>Formats a full Vietnamese date-time into a localized human-readable string.</summary>
    public abstract string LocalizeVietnameseDate(int year, int month, int day, bool isLeap, int zodiac, int hour, int minute, int second);

    // ===== Japanese Calendar =====

    /// <summary>Gets the display name of the Japanese era (Nengo) calendar system.</summary>
    public abstract string CalendarJapaneseName { get; }

    /// <summary>Gets the localized era name for the given era index.</summary>
    public abstract string? GetJapaneseEraName(int eraIndex);

    /// <summary>Gets the localized label for the era component.</summary>
    public abstract string CalendarComponentEra { get; }

    /// <summary>Returns a localized string for the given Japanese year-within-era value.</summary>
    public abstract string FormatJapaneseYear(int year);

    /// <summary>Returns a localized string for the given day value in the Japanese calendar.</summary>
    public abstract string FormatJapaneseDay(int day);

    /// <summary>Formats a full Japanese date-time into a localized human-readable string.</summary>
    public abstract string LocalizeJapaneseDate(int eraIndex, int year, int month, int day, int hour, int minute, int second);

    // ===== Yi Calendar (彝历) =====

    /// <summary>Gets the display name of the Yi ethnic solar calendar system.</summary>
    public abstract string CalendarYiName { get; }

    /// <summary>Gets the localized label for the Yi season component (五季).</summary>
    public abstract string CalendarComponentYiSeason { get; }

    /// <summary>Gets the localized label for the Yi xun (旬) component.</summary>
    public abstract string CalendarComponentYiXun { get; }

    /// <summary>
    /// Gets the localized month name for the given internal month number.
    /// 0 = New Year (大年), 1–10 = regular months, 11 = Mid-Year (小年).
    /// </summary>
    public abstract string? GetYiMonthName(int month);

    /// <summary>Gets the localized season name for the given season index (0–4: 木火土金水).</summary>
    public abstract string? GetYiSeasonName(int seasonIndex);

    /// <summary>Gets the localized xun name for the given xun index (0=上旬, 1=中旬, 2=下旬).</summary>
    public abstract string? GetYiXunName(int xunIndex);

    /// <summary>Gets the localized day-animal name for the given animal index (0–11).</summary>
    public abstract string? GetYiDayAnimalName(int animalIndex);

    /// <summary>Returns a localized string for the given Yi year value.</summary>
    public abstract string FormatYiYear(int year);

    /// <summary>Returns a localized string for the given day value in the Yi calendar (1–36).</summary>
    public abstract string FormatYiDay(int day);

    /// <summary>Formats a full Yi date-time into a localized human-readable string.</summary>
    public abstract string LocalizeYiDate(int year, int month, int day, int hour, int minute, int second);

    // ===== Sexagenary Calendar =====

    /// <summary>Gets the display name of the Chinese Sexagenary Cycle calendar system.</summary>
    public abstract string CalendarSexagenaryName { get; }

    /// <summary>Gets the localized label for the year stem component.</summary>
    public abstract string CalendarComponentYearStem { get; }

    /// <summary>Gets the localized label for the year branch component.</summary>
    public abstract string CalendarComponentYearBranch { get; }

    /// <summary>Gets the localized label for the month stem component.</summary>
    public abstract string CalendarComponentMonthStem { get; }

    /// <summary>Gets the localized label for the month branch component.</summary>
    public abstract string CalendarComponentMonthBranch { get; }

    /// <summary>Gets the localized label for the day stem component.</summary>
    public abstract string CalendarComponentDayStem { get; }

    /// <summary>Gets the localized label for the day branch component.</summary>
    public abstract string CalendarComponentDayBranch { get; }

    /// <summary>Gets the localized stem name for the given index (0–9).</summary>
    public abstract string? GetSexagenaryStemName(int index);

    /// <summary>Gets the localized branch name for the given index (0–11).</summary>
    public abstract string? GetSexagenaryBranchName(int index);

    /// <summary>Gets the localized zodiac name for the given branch index (0–11).</summary>
    public abstract string? GetSexagenaryZodiacName(int index);

    /// <summary>Formats a full Sexagenary date-time into a localized human-readable string.</summary>
    public abstract string LocalizeSexagenaryDate(int yearStem, int yearBranch, int monthStem, int monthBranch, int dayStem, int dayBranch, int hour, int minute, int second);

    // ── Dai Calendar (Xishuangbanna / 西双版纳小傣历) ─────────────────────────

    /// <summary>Gets the display name of the Xishuangbanna Dai calendar system.</summary>
    public abstract string CalendarDaiName { get; }

    /// <summary>Gets the localized month name for the Dai calendar (1–12, or 13 for leap month 9).</summary>
    public abstract string? GetDaiMonthName(int month);

    /// <summary>Formats a Dai calendar year as a localized string.</summary>
    public abstract string FormatDaiYear(int year);

    /// <summary>Formats a Dai calendar day as a localized string.</summary>
    public abstract string FormatDaiDay(int day);

    /// <summary>Formats a full Dai calendar date-time into a localized human-readable string.</summary>
    public abstract string LocalizeDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second);

    // ── Dehong Dai Calendar (德宏大傣历) ──────────────────────────────────────

    /// <summary>Gets the display name of the Dehong Dai calendar system.</summary>
    public abstract string CalendarDehongDaiName { get; }

    /// <summary>Gets the localized month name for the Dehong Dai calendar (1–12, or 13 for leap month 9).</summary>
    public abstract string? GetDehongDaiMonthName(int month);

    /// <summary>Formats a Dehong Dai calendar year as a localized string.</summary>
    public abstract string FormatDehongDaiYear(int year);

    /// <summary>Formats a Dehong Dai calendar day as a localized string.</summary>
    public abstract string FormatDehongDaiDay(int day);

    /// <summary>Formats a full Dehong Dai calendar date-time into a localized human-readable string.</summary>
    public abstract string LocalizeDehongDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second);

    // ===== Memory Event Localization =====

    public abstract override string FormatMemoryEventSingleChat(string partnerName, string content);
    public abstract override string FormatMemoryEventGroupChat(string sessionId, string content);
    public abstract override string FormatMemoryEventToolCall(string toolNames);
    public abstract override string FormatMemoryEventTask(string content);
    public abstract override string FormatMemoryEventTimer(string content);
    public abstract override string FormatMemoryEventTimerError(string timerName, string error);

    // ===== Timer Notification Localization =====

    /// <summary>
    /// Formats a notification message when a timer starts execution.
    /// </summary>
    /// <param name="timerName">The name of the timer</param>
    public abstract override string FormatTimerStartNotification(string timerName);

    /// <summary>
    /// Formats a notification message when a timer completes execution.
    /// </summary>
    /// <param name="timerName">The name of the timer</param>
    /// <param name="result">The execution result summary</param>
    public abstract override string FormatTimerEndNotification(string timerName, string result);

    /// <summary>
    /// Formats a notification message when a timer execution fails.
    /// </summary>
    /// <param name="timerName">The name of the timer</param>
    /// <param name="error">The error message</param>
    public abstract override string FormatTimerErrorNotification(string timerName, string error);

    // ===== Default-Layer Memory Event Localization =====

    /// <summary>
    /// Formats a memory record when the curator creates a new silicon being.
    /// </summary>
    /// <param name="name">The name of the newly created being</param>
    /// <param name="id">The GUID of the newly created being</param>
    public abstract string FormatMemoryEventBeingCreated(string name, string id);

    /// <summary>
    /// Formats a memory record when a silicon being is reset to its default implementation.
    /// </summary>
    /// <param name="id">The GUID of the being that was reset</param>
    public abstract string FormatMemoryEventBeingReset(string id);

    /// <summary>
    /// Formats a memory record when a task reaches a terminal state (completed).
    /// </summary>
    /// <param name="taskTitle">The title of the completed task</param>
    public abstract string FormatMemoryEventTaskCompleted(string taskTitle);

    /// <summary>
    /// Formats a memory record when a task reaches a terminal state (failed).
    /// </summary>
    /// <param name="taskTitle">The title of the failed task</param>
    public abstract string FormatMemoryEventTaskFailed(string taskTitle);

    /// <summary>
    /// Formats a memory record when the being comes online (loaded by the manager).
    /// </summary>
    public abstract string FormatMemoryEventStartup();

    /// <summary>
    /// Formats a memory record when an unexpected runtime error occurs during a tick.
    /// </summary>
    /// <param name="message">The exception message</param>
    public abstract string FormatMemoryEventRuntimeError(string message);

    // ===== MemoryTool Response Localization =====

    public abstract string MemoryToolNotAvailable { get; }
    public abstract string MemoryToolMissingAction { get; }
    public abstract string MemoryToolMissingContent { get; }
    public abstract string MemoryToolNoMemories { get; }
    public abstract string MemoryToolRecentHeader(int count);
    public abstract string MemoryToolStatsHeader { get; }
    public abstract string MemoryToolStatsTotal { get; }
    public abstract string MemoryToolStatsOldest { get; }
    public abstract string MemoryToolStatsNewest { get; }
    public abstract string MemoryToolStatsNA { get; }
    public abstract string MemoryToolQueryNoResults { get; }
    public abstract string MemoryToolQueryHeader(int count, string rangeDesc);
    public abstract string MemoryToolInvalidYear { get; }
    public abstract string MemoryToolUnknownAction(string action);

    // ===== Code Editor Hover Tooltip Localization =====

    /// <summary>
    /// Gets the localized label for a word type in code editor hover tooltip.
    /// </summary>
    /// <param name="wordType">The type of word (variable, function, class, keyword, identifier)</param>
    /// <returns>The localized label</returns>
    public abstract string GetCodeHoverWordTypeLabel(string wordType);

    /// <summary>
    /// Gets the localized description for a word in code editor hover tooltip.
    /// </summary>
    /// <param name="wordType">The type of word (variable, function, class, keyword, identifier)</param>
    /// <param name="word">The actual word/identifier</param>
    /// <returns>The localized description</returns>
    public abstract string GetCodeHoverWordTypeDesc(string wordType, string word);

    /// <summary>
    /// Gets the localized description for a programming keyword in code editor hover tooltip.
    /// </summary>
    /// <param name="language">Programming language identifier (e.g. "csharp", "javascript")</param>
    /// <param name="keyword">The keyword to describe</param>
    /// <returns>Localized keyword description, or empty string if not found</returns>
    public abstract string GetCodeHoverKeywordDesc(string language, string keyword);

    /// <summary>
    /// Gets a translation by key from the localization dictionary.
    /// </summary>
    /// <param name="key">The translation key</param>
    /// <returns>Localized text, or empty string if not found</returns>
    public abstract string GetTranslation(string key);
}
