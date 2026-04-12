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
    /// Gets the localized display name for a permission type
    /// </summary>
    /// <param name="permissionType">The permission type</param>
    /// <returns>The localized display name</returns>
    public abstract string GetPermissionTypeName(PermissionType permissionType);

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
    /// Gets the text for the send button
    /// </summary>
    public abstract string ChatSendButton { get; }

    /// <summary>
    /// Gets the display name for the user in chat messages
    /// </summary>
    public abstract string ChatUserDisplayName { get; }

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
    /// <returns>The localized display name, or the key itself if not found</returns>
    public abstract string GetConfigDisplayName(string displayNameKey);

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
}
