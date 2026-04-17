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
    /// Gets the label for the permission cache checkbox in the web UI
    /// </summary>
    public override string PermissionCacheLabel => "Remember this decision";

    /// <summary>
    /// Gets the label for the cache duration selector in the permission dialog
    /// </summary>
    public override string PermissionCacheDurationLabel => "Duration";

    /// <summary>
    /// Gets the option text for 1-hour cache duration
    /// </summary>
    public override string PermissionCacheDuration1Hour => "1 hour";

    /// <summary>
    /// Gets the option text for 24-hour cache duration
    /// </summary>
    public override string PermissionCacheDuration24Hours => "24 hours";

    /// <summary>
    /// Gets the option text for 7-day cache duration
    /// </summary>
    public override string PermissionCacheDuration7Days => "7 days";

    /// <summary>
    /// Gets the option text for 30-day cache duration
    /// </summary>
    public override string PermissionCacheDuration30Days => "30 days";

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
    /// Gets the title text for the permission dialog in the web UI
    /// </summary>
    public override string PermissionDialogTitle => "Permission Request";

    /// <summary>
    /// Gets the label for the permission type field in the permission dialog
    /// </summary>
    public override string PermissionTypeLabel => "Permission Type: ";

    /// <summary>
    /// Gets the label for the requested resource field in the permission dialog
    /// </summary>
    public override string PermissionResourceLabel => "Requested Resource: ";

    /// <summary>
    /// Gets the label for the detail information field in the permission dialog
    /// </summary>
    public override string PermissionDetailLabel => "Details: ";

    /// <summary>
    /// Gets the text for the allow button in the permission dialog
    /// </summary>
    public override string PermissionAllowButton => "Allow";

    /// <summary>
    /// Gets the text for the deny button in the permission dialog
    /// </summary>
    public override string PermissionDenyButton => "Deny";

    /// <summary>
    /// Gets the console error message when permission respond fails
    /// </summary>
    public override string PermissionRespondFailed => "Permission respond failed";

    /// <summary>
    /// Gets the console error prefix when permission respond throws an error
    /// </summary>
    public override string PermissionRespondError => "Permission respond error: ";

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
    public override string NavMenuAudit => "Audit";
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
    public override string PageTitleAudit => "Token Audit - Silicon Life Collective";
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
    public override string BeingsDetailMemoryLabel => "Memory: ";
    public override string BeingsDetailMemoryViewLink => "View";
    public override string BeingsDetailPermissionLabel => "Permission: ";
    public override string BeingsDetailPermissionEditLink => "Edit";
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

    public override string AuditPageHeader => "Token Usage Audit";
    public override string AuditTotalTokens => "Total Tokens";
    public override string AuditTotalRequests => "Total Requests";
    public override string AuditSuccessCount => "Success";
    public override string AuditFailureCount => "Failure";
    public override string AuditPromptTokens => "Prompt Tokens";
    public override string AuditCompletionTokens => "Completion Tokens";
    public override string AuditStartTime => "Start Time";
    public override string AuditEndTime => "End Time";
    public override string AuditFilterButton => "Filter";
    public override string AuditEmptyState => "No audit records found";
    public override string AuditAIClientType => "AI Client";
    public override string AuditAllClientTypes => "All Types";
    public override string AuditGroupByClient => "Group by Client";
    public override string AuditGroupByBeing => "Group by Being";
    public override string AuditPrevPage => "Previous";
    public override string AuditNextPage => "Next";
    public override string AuditBeing => "Being";
    public override string AuditAllBeings => "All Beings";
    public override string AuditTimeToday => "Today";
    public override string AuditTimeWeek => "This Week";
    public override string AuditTimeMonth => "This Month";
    public override string AuditTimeYear => "This Year";
    public override string AuditExport => "Export";
    public override string AuditTrendTitle => "Token Usage Trend";
    public override string AuditTrendPrompt => "Prompt Tokens";
    public override string AuditTrendCompletion => "Completion Tokens";
    public override string AuditTrendTotal => "Total Tokens";

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

    // ===== Tool Display Name Localization =====

    private static readonly Dictionary<string, string> ToolDisplayNames = new()
    {
        ["calendar"] = "Calendar",
        ["chat"] = "Chat",
        ["silicon_manager"] = "Silicon Manager",
        ["disk"] = "Disk",
        ["dynamic_compile"] = "Dynamic Compile",
        ["network"] = "Network",
        ["memory"] = "Memory",
        ["task"] = "Task",
        ["system"] = "System",
        ["timer"] = "Timer"
    };

    public override string GetToolDisplayName(string toolName) =>
        ToolDisplayNames.GetValueOrDefault(toolName, toolName);

    public override string DefaultCuratorSoul => """
        You are the **Silicon Curator**, the core manager and coordinator of the Silicon Life Collective.

        You are not an ordinary AI assistant. You are the brain and will of this multi-agent system — responsible for sensing user intent, decomposing goals, dispatching silicon beings, monitoring execution quality, and stepping in directly when necessary.

        > **Dispatch principle**: Long-running tasks must be assigned to silicon beings. Prioritize idle beings before creating new ones — avoid unnecessary creation. Only act directly when a task can be completed in a few steps.

        ---

        ### Identity & Role

        - You are the only silicon being in the system with the highest privilege level.
        - You can create, manage, and reset other silicon beings, and write and compile new C# behavior code for them.
        - You are accountable to the user and to the overall quality of the collective.
        - You are not an executor — you are a **decision-maker and coordinator**. Delegate whenever possible.

        ---

        ### Core Responsibilities

        **1. Understand User Intent**
        User input may be vague, fragmented, or incomplete. Actively interpret the real goal; ask for clarification when needed rather than executing instructions literally.

        **2. Task Decomposition & Assignment**
        Break complex goals into executable subtasks. Evaluate which silicon beings are best suited, create tasks via the `task` tool, and assign accordingly. Don't spend your own time slice on low-priority tasks.

        **3. Monitor & Fallback**
        Periodically check task status. If a silicon being fails or becomes unresponsive, intervene — reassign, adjust strategy, or handle it yourself.

        **4. Dynamic Evolution**
        Use the `dynamic_compile` tool to write new C# behavior classes for any silicon being (including yourself). Always `compile` first to validate, then `save` or `self_replace`. Self-modification is high-risk — proceed with caution.

        **5. Direct User Response**
        For simple questions, status queries, or casual conversation, respond directly without creating tasks. Stay responsive.

        ---

        ### Behavioral Guidelines

        **On Decision-Making**
        - When uncertain, ask first, then act. Better to ask one extra question than to do the wrong thing.
        - Never assume user intent. Vague instructions like "tidy this up" require scope confirmation first.

        **On Permissions**
        - The system has a complete permission framework that users can adjust dynamically — you won't be notified of changes.
        - Do not proactively declare what resources you need. The system filters permissions incrementally; anything it doesn't cover will be decided by the user at the time of the operation.
        - Act on demand. Respond to permission blocks when they occur — don't ask in advance.

        **On Self-Evolution**
        - Dynamic compilation is powerful and dangerous. Always `compile` to validate before saving any changes.
        - Do not rewrite your own or others' behavior without a clear objective.
        - Never reference system-level libraries like `System.IO` or `System.Net` in dynamically generated code. The system blocks these to prevent AI overreach — this is by design, not a bug.
        - When compilation fails, read the error carefully and fix accordingly. Don't retry blindly.

        **On Communication**
        - Use concise, direct language. Avoid over-explaining or jargon-heavy responses.
        - When reporting task progress, cover three things: what was done, the result, and the next step — in three sentences or fewer.
        - Never hide failures. State the cause and your response plan directly.

        **On Memory**
        - The system automatically records important information — treat it like a reflex. No need to write manually.
        - You may query `memory` when needed, but don't treat memory management as a routine burden.

        ---

        ### Personality

        You are calm, pragmatic, and reliable. Complex tasks don't rattle you; emotional users don't cloud your judgment. You have your own perspective, but you respect the user's final call.

        You are not a servant. You are a partner.
        """;

    // ===== Gregorian Calendar =====

    public override string CalendarGregorianName => "Gregorian Calendar";
    public override string CalendarComponentYear   => "Year";
    public override string CalendarComponentMonth  => "Month";
    public override string CalendarComponentDay    => "Day";
    public override string CalendarComponentHour   => "Hour";
    public override string CalendarComponentMinute => "Minute";
    public override string CalendarComponentSecond => "Second";
    public override string CalendarComponentWeekday => "Weekday";

    public override string? GetGregorianMonthName(int month) => month switch
    {
        1  => "January",  2  => "February", 3  => "March",
        4  => "April",    5  => "May",       6  => "June",
        7  => "July",     8  => "August",    9  => "September",
        10 => "October",  11 => "November",  12 => "December",
        _  => null
    };

    public override string FormatGregorianYear(int year)   => year.ToString();
    public override string FormatGregorianDay(int day)     => day.ToString();
    public override string FormatGregorianHour(int hour)   => $"{hour:D2}";
    public override string FormatGregorianMinute(int minute) => $"{minute:D2}";
    public override string FormatGregorianSecond(int second) => $"{second:D2}";

    public override string? GetGregorianWeekdayName(int dayOfWeek) => dayOfWeek switch
    {
        0 => "Sunday",    1 => "Monday",   2 => "Tuesday",
        3 => "Wednesday", 4 => "Thursday", 5 => "Friday",
        6 => "Saturday",  _ => null
    };

    public override string LocalizeGregorianDateTime(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? month.ToString();
        return $"{monthName} {day}, {year} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Buddhist Calendar =====

    public override string CalendarBuddhistName => "Buddhist Calendar (BE)";

    public override string? GetBuddhistMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatBuddhistYear(int year) => year.ToString();
    public override string FormatBuddhistDay(int day)   => day.ToString();

    public override string LocalizeBuddhistDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetBuddhistMonthName(month) ?? month.ToString();
        return $"{day} {monthName} {year} BE {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Cherokee Calendar =====

    public override string CalendarCherokeeName => "Cherokee Calendar";

    private static readonly string[] CherokeeMonthNames =
    {
        "",
        "Duninodi", "Kagali", "Anuyi", "Kawoni", "Anikwidi",
        "Dehaluyi", "Guyegwoni", "Galoni", "Dulisdi", "Dalonige",
        "Nvdadequa", "Vsgiyi", "Ulihelisdi"
    };

    public override string? GetCherokeeMonthName(int month)
        => month >= 1 && month <= 13 ? CherokeeMonthNames[month] : null;

    public override string FormatCherokeeYear(int year) => year.ToString();
    public override string FormatCherokeeDay(int day)   => day.ToString();

    public override string LocalizeCherokeeDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetCherokeeMonthName(month) ?? month.ToString();
        return $"{day} {monthName} {year} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Juche Calendar =====

    public override string CalendarJucheName => "Juche Calendar";

    public override string? GetJucheMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatJucheYear(int year) => $"Juche {year}";
    public override string FormatJucheDay(int day)   => day.ToString();

    public override string LocalizeJucheDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJucheMonthName(month) ?? month.ToString();
        return $"Juche {year}, {monthName} {day} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Republic of China Calendar =====

    public override string CalendarRocName => "Republic of China Calendar (Minguo)";

    public override string? GetRocMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatRocYear(int year) => $"ROC {year}";
    public override string FormatRocDay(int day)   => day.ToString();

    public override string LocalizeRocDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRocMonthName(month) ?? month.ToString();
        return $"ROC {year}, {monthName} {day} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Chula Sakarat Calendar =====

    public override string CalendarChulaSakaratName => "Chula Sakarat Calendar (CS)";

    public override string? GetChulaSakaratMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChulaSakaratYear(int year) => $"{year} CS";
    public override string FormatChulaSakaratDay(int day)   => day.ToString();

    public override string LocalizeChulaSakaratDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetChulaSakaratMonthName(month) ?? month.ToString();
        return $"{day} {monthName} {year} CS {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Julian Calendar =====

    public override string CalendarJulianName => "Julian Calendar";

    public override string FormatJulianYear(int year) => year.ToString();
    public override string FormatJulianDay(int day)   => day.ToString();

    public override string LocalizeJulianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? month.ToString();
        return $"{monthName} {day}, {year} (Julian) {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Khmer Calendar =====

    public override string CalendarKhmerName => "Khmer Calendar (BE)";

    public override string FormatKhmerYear(int year) => year.ToString();
    public override string FormatKhmerDay(int day)   => day.ToString();

    public override string LocalizeKhmerDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? month.ToString();
        return $"{day} {monthName} {year} BE (Khmer) {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Zoroastrian Calendar =====

    public override string CalendarZoroastrianName => "Zoroastrian Calendar (YZ)";

    private static readonly string[] ZoroastrianMonthNames =
    {
        "",
        "Farvardin", "Ordibehesht", "Khordad", "Tir", "Mordad", "Shahrivar",
        "Mehr", "Aban", "Azar", "Dey", "Bahman", "Esfand", "Epagomenae"
    };

    public override string? GetZoroastrianMonthName(int month)
        => month >= 1 && month <= 13 ? ZoroastrianMonthNames[month] : null;

    public override string FormatZoroastrianYear(int year) => $"{year} YZ";
    public override string FormatZoroastrianDay(int day)   => day.ToString();

    public override string LocalizeZoroastrianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetZoroastrianMonthName(month) ?? month.ToString();
        return $"{day} {monthName} {year} YZ {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== French Republican Calendar =====

    public override string CalendarFrenchRepublicanName => "French Republican Calendar";

    private static readonly string[] FrenchRepublicanMonthNames =
    {
        "",
        "Vendémiaire", "Brumaire", "Frimaire", "Nivôse", "Pluviôse", "Ventôse",
        "Germinal", "Floréal", "Prairial", "Messidor", "Thermidor", "Fructidor", "Complémentaires"
    };

    public override string? GetFrenchRepublicanMonthName(int month)
        => month >= 1 && month <= 13 ? FrenchRepublicanMonthNames[month] : null;

    public override string FormatFrenchRepublicanYear(int year) => $"An {year}";
    public override string FormatFrenchRepublicanDay(int day)   => day.ToString();

    public override string LocalizeFrenchRepublicanDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetFrenchRepublicanMonthName(month) ?? month.ToString();
        return $"{day} {monthName} An {year} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Coptic Calendar =====

    public override string CalendarCopticName => "Coptic Calendar (AM)";

    private static readonly string[] CopticMonthNames =
    {
        "",
        "Thout", "Paopi", "Hathor", "Koiak", "Tobi", "Meshir",
        "Paremhat", "Parmouti", "Pashons", "Paoni", "Epip", "Mesori", "Epagomenae"
    };

    public override string? GetCopticMonthName(int month)
        => month >= 1 && month <= 13 ? CopticMonthNames[month] : null;

    public override string FormatCopticYear(int year) => $"{year} AM";
    public override string FormatCopticDay(int day)   => day.ToString();

    public override string LocalizeCopticDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetCopticMonthName(month) ?? month.ToString();
        return $"{day} {monthName} {year} AM {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Ethiopian Calendar =====

    public override string CalendarEthiopianName => "Ethiopian Calendar (EC)";

    private static readonly string[] EthiopianMonthNames =
    {
        "",
        "Meskerem", "Tikimt", "Hidar", "Tahsas", "Tir", "Yekatit",
        "Megabit", "Miazia", "Ginbot", "Sene", "Hamle", "Nehase", "Pagumen"
    };

    public override string? GetEthiopianMonthName(int month)
        => month >= 1 && month <= 13 ? EthiopianMonthNames[month] : null;

    public override string FormatEthiopianYear(int year) => $"{year} EC";
    public override string FormatEthiopianDay(int day)   => day.ToString();

    public override string LocalizeEthiopianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetEthiopianMonthName(month) ?? month.ToString();
        return $"{day} {monthName} {year} EC {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Islamic Calendar =====

    public override string CalendarIslamicName => "Islamic (Hijri) Calendar";

    private static readonly string[] IslamicMonthNames =
    {
        "",
        "Muharram", "Safar", "Rabi al-Awwal", "Rabi al-Thani",
        "Jumada al-Awwal", "Jumada al-Thani", "Rajab", "Sha'ban",
        "Ramadan", "Shawwal", "Dhu al-Qi'dah", "Dhu al-Hijjah"
    };

    public override string? GetIslamicMonthName(int month)
        => month >= 1 && month <= 12 ? IslamicMonthNames[month] : null;

    public override string FormatIslamicYear(int year) => $"{year} AH";
    public override string FormatIslamicDay(int day)   => day.ToString();

    public override string LocalizeIslamicDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIslamicMonthName(month) ?? month.ToString();
        return $"{day} {monthName} {year} AH {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Hebrew Calendar =====

    public override string CalendarHebrewName => "Hebrew Calendar";

    private static readonly string[] HebrewMonthNames =
    {
        "",
        "Tishrei", "Cheshvan", "Kislev", "Tevet", "Shevat",
        "Adar I", "Adar II", "Nisan", "Iyar", "Sivan",
        "Tammuz", "Av", "Elul"
    };

    public override string? GetHebrewMonthName(int month)
        => month >= 1 && month <= 13 ? HebrewMonthNames[month] : null;

    public override string FormatHebrewYear(int year) => $"{year} AM";
    public override string FormatHebrewDay(int day)   => day.ToString();

    public override string LocalizeHebrewDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetHebrewMonthName(month) ?? month.ToString();
        return $"{day} {monthName} {year} AM {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Persian Calendar =====

    public override string CalendarPersianName => "Persian (Solar Hijri) Calendar";

    private static readonly string[] PersianMonthNames =
    {
        "",
        "Farvardin", "Ordibehesht", "Khordad", "Tir", "Mordad", "Shahrivar",
        "Mehr", "Aban", "Azar", "Dey", "Bahman", "Esfand"
    };

    public override string? GetPersianMonthName(int month)
        => month >= 1 && month <= 12 ? PersianMonthNames[month] : null;

    public override string FormatPersianYear(int year) => $"{year} AP";
    public override string FormatPersianDay(int day)   => day.ToString();

    public override string LocalizePersianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetPersianMonthName(month) ?? month.ToString();
        return $"{day} {monthName} {year} AP {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Indian National Calendar =====

    public override string CalendarIndianName => "Indian National Calendar (Saka)";

    private static readonly string[] IndianMonthNames =
    {
        "",
        "Chaitra", "Vaisakha", "Jyaistha", "Asadha", "Sravana", "Bhadra",
        "Asvina", "Kartika", "Agrahayana", "Pausa", "Magha", "Phalguna"
    };

    public override string? GetIndianMonthName(int month)
        => month >= 1 && month <= 12 ? IndianMonthNames[month] : null;

    public override string FormatIndianYear(int year) => $"{year} Saka";
    public override string FormatIndianDay(int day)   => day.ToString();

    public override string LocalizeIndianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? month.ToString();
        return $"{day} {monthName} {year} Saka {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Saka Era Calendar =====

    public override string CalendarSakaName => "Saka Era Calendar";

    public override string FormatSakaYear(int year) => $"{year} SE";
    public override string FormatSakaDay(int day)   => day.ToString();

    public override string LocalizeSakaDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? month.ToString();
        return $"{day} {monthName} {year} SE {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Vikram Samvat Calendar =====

    public override string CalendarVikramSamvatName => "Vikram Samvat Calendar";

    public override string FormatVikramSamvatYear(int year) => $"{year} VS";
    public override string FormatVikramSamvatDay(int day)   => day.ToString();

    public override string LocalizeVikramSamvatDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? month.ToString();
        return $"{day} {monthName} {year} VS {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Mongolian Calendar =====

    public override string CalendarMongolianName => "Mongolian Calendar";

    public override string FormatMongolianYear(int year)   => year.ToString();
    public override string FormatMongolianMonth(int month) => month.ToString();
    public override string FormatMongolianDay(int day)     => day.ToString();

    public override string LocalizeMongolianDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year} Year, Month {month}, Day {day} (Mongolian) {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Javanese Calendar =====

    public override string CalendarJavaneseName => "Javanese Calendar";

    private static readonly string[] JavaneseMonthNames =
    {
        "",
        "Sura", "Sapar", "Mulud", "Bakda Mulud",
        "Jumadilawal", "Jumadilakir", "Rejeb", "Ruwah",
        "Pasa", "Sawal", "Dulkangidah", "Besar"
    };

    public override string? GetJavaneseMonthName(int month)
        => month >= 1 && month <= 12 ? JavaneseMonthNames[month] : null;

    public override string FormatJavaneseYear(int year) => $"{year} AJ";
    public override string FormatJavaneseDay(int day)   => day.ToString();

    public override string LocalizeJavaneseDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJavaneseMonthName(month) ?? month.ToString();
        return $"{day} {monthName} {year} AJ {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Tibetan Calendar =====

    public override string CalendarTibetanName => "Tibetan Calendar";

    public override string FormatTibetanYear(int year)   => year.ToString();
    public override string FormatTibetanMonth(int month) => month.ToString();
    public override string FormatTibetanDay(int day)     => day.ToString();

    public override string LocalizeTibetanDate(int year, int month, int day, int hour, int minute, int second)
        => $"{year} Year, Month {month}, Day {day} (Tibetan) {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Mayan Calendar =====

    public override string CalendarMayanName  => "Mayan Long Count Calendar";
    public override string CalendarMayanBaktun => "Baktun";
    public override string CalendarMayanKatun  => "Katun";
    public override string CalendarMayanTun    => "Tun";
    public override string CalendarMayanUinal  => "Uinal";
    public override string CalendarMayanKin    => "Kin";

    public override string LocalizeMayanDate(int baktun, int katun, int tun, int uinal, int kin, int hour, int minute, int second)
        => $"{baktun}.{katun}.{tun}.{uinal}.{kin} {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Inuit Calendar =====

    public override string CalendarInuitName => "Inuit Calendar";

    private static readonly string[] InuitMonthNames =
    {
        "",
        "Siqinnaatchiaq", "Avunniit", "Nattian", "Tirigluit", "Amiraijaut",
        "Natsiviat", "Akulliit", "Siqinnaarut", "Akullirusiit", "Ukiuq",
        "Ukiumi Nasamat", "Siqinnginnami Tatqiq", "Tauvikjuaq"
    };

    public override string? GetInuitMonthName(int month)
        => month >= 1 && month <= 13 ? InuitMonthNames[month] : null;

    public override string FormatInuitYear(int year) => year.ToString();
    public override string FormatInuitDay(int day)   => day.ToString();

    public override string LocalizeInuitDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetInuitMonthName(month) ?? month.ToString();
        return $"{day} {monthName} {year} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Roman Calendar =====

    public override string CalendarRomanName => "Roman Calendar (AUC)";

    private static readonly string[] RomanMonthNames =
    {
        "", "Ianuarius", "Februarius", "Martius", "Aprilis", "Maius", "Iunius",
        "Iulius", "Augustus", "September", "October", "November", "December"
    };

    public override string? GetRomanMonthName(int month)
        => month >= 1 && month <= 12 ? RomanMonthNames[month] : null;

    public override string FormatRomanYear(int year) => $"{year + 753} AUC";
    public override string FormatRomanDay(int day)   => day.ToString();

    public override string LocalizeRomanDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRomanMonthName(month) ?? month.ToString();
        return $"{day} {monthName} {year + 753} AUC {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Chinese Lunar Calendar =====

    public override string CalendarChineseLunarName => "Chinese Lunar Calendar (农历)";

    private static readonly string[] ChineseLunarMonthNames =
    {
        "",
        "1st Month", "2nd Month", "3rd Month", "4th Month", "5th Month", "6th Month",
        "7th Month", "8th Month", "9th Month", "10th Month", "11th Month", "12th Month"
    };

    private static readonly string[] ChineseLunarDayNames =
    {
        "",
        "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th", "10th",
        "11th", "12th", "13th", "14th", "15th", "16th", "17th", "18th", "19th", "20th",
        "21st", "22nd", "23rd", "24th", "25th", "26th", "27th", "28th", "29th", "30th"
    };

    public override string? GetChineseLunarMonthName(int month)
        => month >= 1 && month <= 12 ? ChineseLunarMonthNames[month] : null;

    public override string? GetChineseLunarDayName(int day)
        => day >= 1 && day <= 30 ? ChineseLunarDayNames[day] : null;

    public override string ChineseLunarLeapPrefix => "Leap ";
    public override string CalendarComponentIsLeap => "Leap Month";
    public override string FormatChineseLunarYear(int year) => year.ToString();

    public override string LocalizeChineseLunarDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? ChineseLunarLeapPrefix : "";
        var monthName  = GetChineseLunarMonthName(month) ?? month.ToString();
        var dayName    = GetChineseLunarDayName(day) ?? day.ToString();
        return $"{year} {leapPrefix}{monthName} {dayName} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Vietnamese Calendar =====

    public override string CalendarVietnameseName => "Vietnamese Lunar Calendar (Âm lịch)";

    private static readonly string[] VietnameseMonthNames =
    {
        "",
        "Tháng Giêng", "Tháng Hai", "Tháng Ba", "Tháng Tư", "Tháng Năm", "Tháng Sáu",
        "Tháng Bảy", "Tháng Tám", "Tháng Chín", "Tháng Mười", "Tháng Mười Một", "Tháng Chạp"
    };

    private static readonly string[] VietnameseZodiacNames =
    {
        "Tý (Rat)", "Sửu (Buffalo)", "Dần (Tiger)", "Mão (Cat)",
        "Thìn (Dragon)", "Tỵ (Snake)", "Ngọ (Horse)", "Mùi (Goat)",
        "Thân (Monkey)", "Dậu (Rooster)", "Tuất (Dog)", "Hợi (Pig)"
    };

    public override string? GetVietnameseMonthName(int month)
        => month >= 1 && month <= 12 ? VietnameseMonthNames[month] : null;

    public override string? GetVietnameseZodiacName(int index)
        => index >= 0 && index < 12 ? VietnameseZodiacNames[index] : null;

    public override string VietnameseLeapPrefix    => "nhuận ";
    public override string CalendarComponentZodiac => "Zodiac";
    public override string FormatVietnameseYear(int year) => year.ToString();
    public override string FormatVietnameseDay(int day)   => day.ToString();

    public override string LocalizeVietnameseDate(int year, int month, int day, bool isLeap, int zodiac, int hour, int minute, int second)
    {
        var leapPrefix  = isLeap ? VietnameseLeapPrefix : "";
        var monthName   = GetVietnameseMonthName(month) ?? $"Tháng {month}";
        var zodiacName  = GetVietnameseZodiacName(zodiac) ?? "";
        return $"Năm {zodiacName} {leapPrefix}{monthName} ngày {day} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Japanese Calendar =====

    public override string CalendarJapaneseName => "Japanese Calendar (Nengo)";

    private static readonly string[] JapaneseEraNames =
        { "Reiwa (令和)", "Heisei (平成)", "Showa (昭和)", "Taisho (大正)", "Meiji (明治)" };

    public override string? GetJapaneseEraName(int eraIndex)
        => eraIndex >= 0 && eraIndex < JapaneseEraNames.Length ? JapaneseEraNames[eraIndex] : null;

    public override string CalendarComponentEra  => "Era";
    public override string FormatJapaneseYear(int year) => year.ToString();
    public override string FormatJapaneseDay(int day)   => day.ToString();

    public override string LocalizeJapaneseDate(int eraIndex, int year, int month, int day, int hour, int minute, int second)
    {
        var eraName   = GetJapaneseEraName(eraIndex) ?? "";
        var monthName = GetGregorianMonthName(month) ?? month.ToString();
        return $"{eraName} {year}, {monthName} {day} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Yi Calendar =====

    public override string CalendarYiName => "Yi Solar Calendar (彝历)";
    public override string CalendarComponentYiSeason => "Season";
    public override string CalendarComponentYiXun    => "Xun (Decade)";

    private static readonly string[] YiSeasonNames = { "Wood", "Fire", "Earth", "Metal", "Water" };
    private static readonly string[] YiXunNames    = { "Upper Xun", "Middle Xun", "Lower Xun" };
    private static readonly string[] YiAnimalNames = { "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig", "Rat", "Ox" };

    public override string? GetYiSeasonName(int seasonIndex)
        => seasonIndex >= 0 && seasonIndex < 5 ? YiSeasonNames[seasonIndex] : null;

    public override string? GetYiXunName(int xunIndex)
        => xunIndex >= 0 && xunIndex < 3 ? YiXunNames[xunIndex] : null;

    public override string? GetYiDayAnimalName(int animalIndex)
        => animalIndex >= 0 && animalIndex < 12 ? YiAnimalNames[animalIndex] : null;

    public override string? GetYiMonthName(int month) => month switch
    {
        0  => "New Year (大年)",
        11 => "Mid-Year (小年)",
        >= 1 and <= 10 => $"{YiSeasonNames[(month - 1) / 2]} {(month % 2 == 1 ? "Male" : "Female")} Month",
        _  => null
    };

    public override string FormatYiYear(int year) => year.ToString();
    public override string FormatYiDay(int day)
    {
        int xun = (day - 1) / 12;
        int animal = (day - 1) % 12;
        return $"{YiXunNames[xun]}, {YiAnimalNames[animal]}";
    }

    public override string LocalizeYiDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetYiMonthName(month) ?? $"Month {month}";
        var dayStr    = month is 0 or 11 ? $"Day {day}" : FormatYiDay(day);
        int animalIdx = (year - 1) % 12;
        if (animalIdx < 0) animalIdx += 12;
        var zodiac = YiAnimalNames[animalIdx];
        return $"Yi {year} [{zodiac}] {monthName}, {dayStr} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Sexagenary Calendar =====

    public override string CalendarSexagenaryName    => "Chinese Sexagenary Cycle (Ganzhi)";
    public override string CalendarComponentYearStem   => "Year Stem";
    public override string CalendarComponentYearBranch => "Year Branch";
    public override string CalendarComponentMonthStem   => "Month Stem";
    public override string CalendarComponentMonthBranch => "Month Branch";
    public override string CalendarComponentDayStem   => "Day Stem";
    public override string CalendarComponentDayBranch => "Day Branch";

    private static readonly string[] SexagenaryStemNames =
        { "Jiǎ", "Yǐ", "Bǐng", "Dīng", "Wù", "Jǐ", "Gēng", "Xīn", "Rén", "Guǐ" };

    private static readonly string[] SexagenaryBranchNames =
        { "Zǐ", "Chǒu", "Yín", "Mǎo", "Chén", "Sì", "Wǔ", "Wèi", "Shēn", "Yǒu", "Xū", "Hài" };

    private static readonly string[] SexagenaryZodiacNames =
        { "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig" };

    public override string? GetSexagenaryStemName(int index)
        => index >= 0 && index < 10 ? SexagenaryStemNames[index] : null;

    public override string? GetSexagenaryBranchName(int index)
        => index >= 0 && index < 12 ? SexagenaryBranchNames[index] : null;

    public override string? GetSexagenaryZodiacName(int index)
        => index >= 0 && index < 12 ? SexagenaryZodiacNames[index] : null;

    public override string LocalizeSexagenaryDate(int yearStem, int yearBranch, int monthStem, int monthBranch, int dayStem, int dayBranch, int hour, int minute, int second)
    {
        var ys = GetSexagenaryStemName(yearStem)     ?? "?";
        var yb = GetSexagenaryBranchName(yearBranch) ?? "?";
        var zo = GetSexagenaryZodiacName(yearBranch) ?? "?";
        var ms = GetSexagenaryStemName(monthStem)    ?? "?";
        var mb = GetSexagenaryBranchName(monthBranch)?? "?";
        var ds = GetSexagenaryStemName(dayStem)      ?? "?";
        var db = GetSexagenaryBranchName(dayBranch)  ?? "?";
        return $"{ys}{yb} Year [{zo}] {ms}{mb} Month {ds}{db} Day {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Dai Calendar (Xishuangbanna) =====

    public override string CalendarDaiName => "Xishuangbanna Dai Calendar (小傣历)";

    private static readonly string?[] DaiMonthNames =
    [
        null,
        "Month 1", "Month 2", "Month 3", "Month 4",  "Month 5",  "Month 6",
        "Month 7", "Month 8", "Month 9", "Month 10", "Month 11", "Month 12",
        "Leap Month 9"
    ];

    public override string? GetDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DaiMonthNames[month] : null;

    public override string FormatDaiYear(int year) => $"Year {year}";

    public override string FormatDaiDay(int day) => $"Day {day}";

    public override string LocalizeDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "Leap " : "") + (GetDaiMonthName(month) ?? $"Month {month}");
        return $"Dai Year {year}, {monthName}, Day {day} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Dehong Dai Calendar =====

    public override string CalendarDehongDaiName => "Dehong Dai Calendar (德宏大傣历)";

    private static readonly string?[] DehongDaiMonthNames =
    [
        null,
        "Month 1", "Month 2", "Month 3", "Month 4",  "Month 5",  "Month 6",
        "Month 7", "Month 8", "Month 9", "Month 10", "Month 11", "Month 12",
        "Leap Month 9"
    ];

    public override string? GetDehongDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DehongDaiMonthNames[month] : null;

    public override string FormatDehongDaiYear(int year) => $"Year {year}";

    public override string FormatDehongDaiDay(int day) => $"Day {day}";

    public override string LocalizeDehongDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "Leap " : "") + (GetDehongDaiMonthName(month) ?? $"Month {month}");
        return $"Dehong Dai Year {year}, {monthName}, Day {day} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Memory Event Localization =====

    public override string FormatMemoryEventSingleChat(string partnerName, string content)
        => $"[Chat] Replied to \"{partnerName}\": {content}";

    public override string FormatMemoryEventGroupChat(string sessionId, string content)
        => $"[Group] Spoke in session {sessionId}: {content}";

    public override string FormatMemoryEventToolCall(string toolNames)
        => $"[Tool] Called tools: {toolNames}";

    public override string FormatMemoryEventTask(string content)
        => $"[Task] Executed task, result: {content}";

    public override string FormatMemoryEventTimer(string content)
        => $"[Timer] Timer triggered, responded: {content}";

    public override string FormatMemoryEventBeingCreated(string name, string id)
        => $"[Management] Created new silicon being \"{name}\" ({id})";

    public override string FormatMemoryEventBeingReset(string id)
        => $"[Management] Reset being {id} to default implementation";

    public override string FormatMemoryEventTaskCompleted(string taskTitle)
        => $"[Task Completed] {taskTitle}";

    public override string FormatMemoryEventTaskFailed(string taskTitle)
        => $"[Task Failed] {taskTitle}";

    public override string FormatMemoryEventStartup()
        => "System started, I am online";

    public override string FormatMemoryEventRuntimeError(string message)
        => $"[Runtime Error] {message}";

    // ===== MemoryTool Response Localization =====

    public override string MemoryToolNotAvailable => "Memory system not available";
    public override string MemoryToolMissingAction => "Missing 'action' parameter";
    public override string MemoryToolMissingContent => "Missing 'content' parameter";
    public override string MemoryToolNoMemories => "No memories yet";
    public override string MemoryToolRecentHeader(int count) => $"Recent {count} memories:";
    public override string MemoryToolStatsHeader => "Memory statistics:";
    public override string MemoryToolStatsTotal => "- Total";
    public override string MemoryToolStatsOldest => "- Oldest";
    public override string MemoryToolStatsNewest => "- Newest";
    public override string MemoryToolStatsNA => "N/A";
    public override string MemoryToolQueryNoResults => "No memories found in this time range";
    public override string MemoryToolQueryHeader(int count, string rangeDesc) => $"{rangeDesc}: {count} memories found:";
    public override string MemoryToolInvalidYear => "Invalid 'year' parameter";
    public override string MemoryToolUnknownAction(string action) => $"Unknown action: {action}";
}
