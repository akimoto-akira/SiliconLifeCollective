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
/// English (United States) localization implementation
/// </summary>
public class EnUS : DefaultLocalizationBase
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "en-US";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "English (US)";

    /// <summary>
    /// Gets the welcome message
    /// </summary>
    public override string WelcomeMessage => "Welcome to Silicon Life Collective!";

    /// <summary>
    /// Gets the brand name
    /// </summary>
    public override string BrandName => "Silicon Life Collective";

    /// <summary>
    /// Gets the input prompt
    /// </summary>
    public override string InputPrompt => "> ";

    /// <summary>
    /// Gets the shutdown message
    /// </summary>
    public override string ShutdownMessage => "Shutting down...";

    /// <summary>
    /// Gets the config corrupted error message
    /// </summary>
    public override string ConfigCorruptedError => "Configuration file corrupted, using default configuration";

    /// <summary>
    /// Gets the config created message
    /// </summary>
    public override string ConfigCreatedWithDefaults => "Configuration file not found, created with default values";

    /// <summary>
    /// Gets the AI connection error message
    /// </summary>
    public override string AIConnectionError => "Cannot connect to AI service, please check if Ollama is running";

    /// <summary>
    /// Gets the AI request error message
    /// </summary>
    public override string AIRequestError => "AI request failed";

    /// <summary>
    /// Gets the data directory create error message
    /// </summary>
    public override string DataDirectoryCreateError => "Cannot create data directory";

    /// <summary>
    /// Gets the thinking message
    /// </summary>
    public override string ThinkingMessage => "Thinking...";

    /// <summary>
    /// Gets the tool call message
    /// </summary>
    public override string ToolCallMessage => "Executing tools...";

    /// <summary>
    /// Gets the error message
    /// </summary>
    public override string ErrorMessage => "Error";

    /// <summary>
    /// Gets the unexpected error message
    /// </summary>
    public override string UnexpectedErrorMessage => "Unexpected error";

    /// <summary>
    /// Gets the permission denied message
    /// </summary>
    public override string PermissionDeniedMessage => "Permission denied";

    /// <summary>
    /// Gets the permission ask prompt
    /// </summary>
    public override string PermissionAskPrompt => "Allow? (y/n): ";

    /// <summary>
    /// Gets the header displayed for permission requests
    /// </summary>
    public override string PermissionRequestHeader => "[PERMISSION]";

    /// <summary>
    /// Gets the label for the allow code in permission prompts
    /// </summary>
    public override string AllowCodeLabel => "Allow code";

    /// <summary>
    /// Gets the label for the deny code in permission prompts
    /// </summary>
    public override string DenyCodeLabel => "Deny code";

    /// <summary>
    /// Gets the instruction text for replying to permission prompts
    /// </summary>
    public override string PermissionReplyInstruction => "Reply code to confirm, or anything else to deny";

    /// <summary>
    /// Gets the prompt for asking whether to cache a permission decision
    /// </summary>
    public override string AddToCachePrompt => "Add to cache? (y/n): ";

    /// <summary>
    /// Gets the localized display name for a permission type
    /// </summary>
    public override string GetPermissionTypeName(PermissionType permissionType) => permissionType switch
    {
        PermissionType.NetworkAccess => "Network access",
        PermissionType.CommandLine => "Command execution",
        PermissionType.FileAccess => "File access",
        PermissionType.Function => "Function invocation",
        PermissionType.DataAccess => "Data access",
        _ => permissionType.ToString()
    };

    /// <summary>
    /// Gets the system prompt for memory compression
    /// </summary>
    public override string MemoryCompressionSystemPrompt => "You are a memory compression assistant. Please compress the following memories into a concise summary while retaining key information.";

    /// <summary>
    /// Gets the user prompt template for memory compression
    /// </summary>
    public override string GetMemoryCompressionUserPrompt(string levelDesc, string rangeDesc, string contentText)
    {
        return $"Memory compression: {levelDesc}. Time range: {rangeDesc}.\n\nMemory content:\n{contentText}";
    }

    // ===== Init Page Localization =====

    public override string InitPageTitle => "Setup";
    public override string InitDescription => "First time setup, please complete the basic configuration";
    public override string InitNicknameLabel => "Nickname";
    public override string InitNicknamePlaceholder => "Enter your nickname";
    public override string InitEndpointLabel => "AI API Endpoint";
    public override string InitEndpointPlaceholder => "e.g. http://localhost:11434";
    public override string InitSkinLabel => "Skin";
    public override string InitSkinPlaceholder => "Leave empty for default skin";
    public override string InitDataDirectoryLabel => "Data Directory";
    public override string InitDataDirectoryPlaceholder => "e.g. ./data";
    public override string InitDataDirectoryBrowse => "Browse...";
    public override string InitSkinSelected => "\u2713 Selected";
    public override string InitSkinPreviewTitle => "Preview";
    public override string InitSkinPreviewCardTitle => "Card Title";
    public override string InitSkinPreviewCardContent => "This is a sample card showing the UI style of this skin.";
    public override string InitSkinPreviewPrimaryBtn => "Primary";
    public override string InitSkinPreviewSecondaryBtn => "Secondary";
    public override string InitSubmitButton => "Complete Setup";
    public override string InitFooterHint => "You can modify settings at any time on the settings page";
    public override string InitNicknameRequiredError => "Please enter a nickname";
    public override string InitDataDirectoryRequiredError => "Please select a data directory";
    public override string InitCuratorNameLabel => "Silicon Being Name";
    public override string InitCuratorNamePlaceholder => "Enter a name for the first silicon being";
    public override string InitCuratorNameRequiredError => "Please enter a silicon being name";
    public override string InitLanguageLabel => "Language / 语言";
    public override string InitLanguageSwitchBtn => "Apply";

    // ===== Navigation Menu Localization =====

    public override string NavMenuChat => "Chat";
    public override string NavMenuDashboard => "Dashboard";
    public override string NavMenuBeings => "Beings";
    public override string NavMenuTasks => "Tasks";
    public override string NavMenuMemory => "Memory";
    public override string NavMenuKnowledge => "Knowledge";
    public override string NavMenuProjects => "Projects";
    public override string NavMenuLogs => "Logs";
    public override string NavMenuConfig => "Config";
    public override string NavMenuHelp => "Help";
    public override string NavMenuAbout => "About";

    // ===== Page Title Localization =====

    public override string PageTitleChat => "Chat - Silicon Life Collective";
    public override string PageTitleDashboard => "Dashboard - Silicon Life Collective";
    public override string PageTitleBeings => "Beings - Silicon Life Collective";
    public override string PageTitleTasks => "Tasks - Silicon Life Collective";
    public override string PageTitleTimers => "Timers - Silicon Life Collective";
    public override string PageTitleMemory => "Memory - Silicon Life Collective";
    public override string PageTitleKnowledge => "Knowledge - Silicon Life Collective";
    public override string PageTitleProjects => "Projects - Silicon Life Collective";
    public override string PageTitleLogs => "Logs - Silicon Life Collective";
    public override string PageTitleConfig => "Config - Silicon Life Collective";
    public override string PageTitleExecutor => "Executor - Silicon Life Collective";
    public override string PageTitleCodeBrowser => "Code Browser - Silicon Life Collective";
    public override string PageTitlePermission => "Permission - Silicon Life Collective";
    public override string PageTitleAbout => "About - Silicon Life Collective";

    // ===== Chat Localization =====

    public override string SingleChatNameFormat => "Chat with {0}";
    public override string ChatConversationsHeader => "Conversations";
    public override string ChatNoConversationSelected => "Select a conversation to start chatting";
    public override string ChatMessageInputPlaceholder => "Type a message...";
    public override string ChatSendButton => "Send";
    public override string ChatUserDisplayName => "Me";
    public override string ChatDefaultBeingName => "AI";
    public override string ChatThinkingSummary => "💭 Think";
    public override string GetChatToolCallsSummary(int count) => $"🔧 Tool Calls ({count})";

    // ===== Dashboard Localization =====

    public override string DashboardPageHeader => "Dashboard";
    public override string DashboardStatTotalBeings => "Total Beings";
    public override string DashboardStatActiveBeings => "Active Beings";
    public override string DashboardStatUptime => "Uptime";
    public override string DashboardStatMemory => "Memory Usage";
    public override string DashboardChartMessageFrequency => "Message Frequency";

    // ===== Beings Localization =====

    public override string BeingsPageHeader => "Silicon Beings Management";
    public override string BeingsTotalCount => "Total {0} beings";
    public override string BeingsNoSelectionPlaceholder => "Select a being to view details";
    public override string BeingsEmptyState => "No beings found";
    public override string BeingsStatusIdle => "Idle";
    public override string BeingsStatusRunning => "Running";
    public override string BeingsDetailIdLabel => "ID: ";
    public override string BeingsDetailStatusLabel => "Status: ";
    public override string BeingsDetailCustomCompileLabel => "Custom Compilation: ";
    public override string BeingsDetailSoulContentLabel => "Soul Content: ";
    public override string BeingsDetailTimersLabel => "Timers: ";
    public override string BeingsDetailTasksLabel => "Tasks: ";
    public override string BeingsYes => "Yes";
    public override string BeingsNo => "No";
    public override string BeingsNotSet => "Not set";

    // ===== Timers Page Localization =====

    public override string TimersPageHeader => "Timer Management";
    public override string TimersTotalCount => "Total {0} timers";
    public override string TimersEmptyState => "No timers";
    public override string TimersStatusActive => "Active";
    public override string TimersStatusPaused => "Paused";
    public override string TimersStatusTriggered => "Triggered";
    public override string TimersStatusCancelled => "Cancelled";
    public override string TimersTypeRecurring => "Recurring";
    public override string TimersTriggerTimeLabel => "Trigger Time: ";
    public override string TimersIntervalLabel => "Interval: ";
    public override string TimersTriggeredCountLabel => "Triggered: ";

    // ===== Chat Page Localization =====

    public override string AboutPageHeader => "About";
    public override string AboutAppName => "Silicon Life Collective";
    public override string AboutVersionLabel => "Version";
    public override string AboutDescription => "An AI-based silicon life collective management system that supports collaborative work of multiple AI agents, memory management, knowledge graph construction, and more.";
    public override string AboutAuthorLabel => "Author";
    public override string AboutAuthorName => "Hoshino Kennji";
    public override string AboutLicenseLabel => "License";
    public override string AboutCopyright => "Copyright (c) 2026 Hoshino Kennji";
    public override string AboutGitHubLink => "GitHub Repository";
    public override string AboutGiteeLink => "Gitee Mirror";
    public override string AboutSocialMediaLabel => "Social Media";
    public override string GetSocialMediaName(string platform) => platform switch
    {
        "Bilibili" => "Bilibili",
        "YouTube" => "YouTube",
        "X" => "X (Twitter)",
        "Douyin" => "Douyin",
        "Weibo" => "Weibo",
        "WeChat" => "WeChat Official Account",
        "Xiaohongshu" => "Xiaohongshu",
        "Zhihu" => "Zhihu",
        "TouTiao" => "Toutiao",
        "Kuaishou" => "Kuaishou",
        _ => platform
    };

    // ===== Config Page Localization =====

    public override string ConfigPageHeader => "System Configuration";
    public override string ConfigPropertyNameLabel => "Property Name";
    public override string ConfigPropertyValueLabel => "Property Value";
    public override string ConfigActionLabel => "Action";
    public override string ConfigEditButton => "Edit";
    public override string ConfigEditModalTitle => "Edit Configuration";
    public override string ConfigEditPropertyLabel => "Property Name: ";
    public override string ConfigEditValueLabel => "Property Value: ";
    public override string ConfigBrowseButton => "Browse";
    public override string ConfigTimeSettingsLabel => "Time Settings: ";
    public override string ConfigDaysLabel => "Days: ";
    public override string ConfigHoursLabel => "Hours: ";
    public override string ConfigMinutesLabel => "Minutes: ";
    public override string ConfigSecondsLabel => "Seconds: ";
    public override string ConfigSaveButton => "Save";
    public override string ConfigCancelButton => "Cancel";
    public override string ConfigNullValue => "null";

    public override string ConfigEditPrefix => "Edit: ";
    public override string ConfigDefaultGroupName => "Other";
    public override string ConfigErrorInvalidRequest => "Invalid request parameters";
    public override string ConfigErrorInstanceNotFound => "Configuration instance not found";
    public override string ConfigErrorPropertyNotFound => "Property {0} does not exist or is not writable";
    public override string ConfigErrorConvertInt => "Cannot convert '{0}' to integer";
    public override string ConfigErrorConvertLong => "Cannot convert '{0}' to long integer";
    public override string ConfigErrorConvertDouble => "Cannot convert '{0}' to floating point number";
    public override string ConfigErrorConvertBool => "Cannot convert '{0}' to boolean";
    public override string ConfigErrorConvertGuid => "Cannot convert '{0}' to GUID";
    public override string ConfigErrorConvertTimeSpan => "Cannot convert '{0}' to time span";
    public override string ConfigErrorConvertDateTime => "Cannot convert '{0}' to date time";
    public override string ConfigErrorConvertEnum => "Cannot convert '{0}' to {1}";
    public override string ConfigErrorUnsupportedType => "Unsupported property type: {0}";
    public override string ConfigErrorSaveFailed => "Save failed: {0}";

    public override string LogsPageHeader => "Log Query";
    public override string LogsTotalCount => "{0} logs total";
    public override string LogsStartTime => "Start Time";
    public override string LogsEndTime => "End Time";
    public override string LogsLevelAll => "All Levels";
    public override string LogsFilterButton => "Filter";
    public override string LogsEmptyState => "No log entries found";
    public override string LogsExceptionLabel => "Exception: ";
    public override string LogsPrevPage => "Previous";
    public override string LogsNextPage => "Next";

    private static readonly Dictionary<string, string> ConfigGroupNames = new()
    {
        ["Basic"] = "Basic Settings",
        ["Runtime"] = "Runtime Settings",
        ["AI"] = "AI Settings",
        ["Web"] = "Web Settings",
        ["User"] = "User Settings"
    };

    private static readonly Dictionary<string, string> ConfigDisplayNames = new()
    {
        ["DataDirectory"] = "Data Directory",
        ["Language"] = "Language",
        ["TickTimeout"] = "Tick Timeout",
        ["MaxTimeoutCount"] = "Max Timeout Count",
        ["WatchdogTimeout"] = "Watchdog Timeout",
        ["MinLogLevel"] = "Min Log Level",
        ["AIClientType"] = "AI Client Type",
        ["OllamaEndpoint"] = "Ollama Endpoint",
        ["DefaultModel"] = "Default Model",
        ["WebPort"] = "Web Port",
        ["AllowIntranetAccess"] = "Allow Intranet Access",
        ["WebSkin"] = "Web Skin",
        ["UserNickname"] = "User Nickname"
    };

    private static readonly Dictionary<string, string> ConfigDescriptions = new()
    {
        ["DataDirectory"] = "Data directory path for storing all application data",
        ["Language"] = "Language setting for the application",
        ["TickTimeout"] = "Timeout duration for each tick execution",
        ["MaxTimeoutCount"] = "Maximum consecutive timeouts before circuit breaker triggers",
        ["WatchdogTimeout"] = "Watchdog timeout duration for detecting hung main loop",
        ["MinLogLevel"] = "Global minimum log level",
        ["AIClientType"] = "AI client type to use",
        ["OllamaEndpoint"] = "Ollama API endpoint URL",
        ["DefaultModel"] = "Default AI model to use",
        ["WebPort"] = "Web server port",
        ["AllowIntranetAccess"] = "Allow intranet access (requires admin)",
        ["WebSkin"] = "Web skin name",
        ["UserNickname"] = "Nickname of the human user"
    };

    public override string GetConfigGroupName(string groupKey) =>
        ConfigGroupNames.GetValueOrDefault(groupKey, groupKey);

    public override string GetConfigDisplayName(string displayNameKey) =>
        ConfigDisplayNames.GetValueOrDefault(displayNameKey, displayNameKey);

    public override string? GetConfigDescription(string descriptionKey) =>
        ConfigDescriptions.GetValueOrDefault(descriptionKey);

    /// <summary>
    /// Gets the localized display name for a log level
    /// </summary>
    public override string GetLogLevelName(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => "Trace",
        LogLevel.Debug => "Debug",
        LogLevel.Information => "Information",
        LogLevel.Warning => "Warning",
        LogLevel.Error => "Error",
        LogLevel.Critical => "Critical",
        LogLevel.None => "None",
        _ => logLevel.ToString()
    };
}
