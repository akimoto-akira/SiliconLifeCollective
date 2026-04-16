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
/// Chinese (Simplified) localization implementation
/// </summary>
public class ZhCN : DefaultLocalizationBase
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "zh-CN";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "简体中文";

    /// <summary>
    /// Gets the welcome message
    /// </summary>
    public override string WelcomeMessage => "欢迎使用硅基生命群！";

    /// <summary>
    /// Gets the brand name
    /// </summary>
    public override string BrandName => "硅基生命群";

    /// <summary>
    /// Gets the input prompt
    /// </summary>
    public override string InputPrompt => "> ";

    /// <summary>
    /// Gets the shutdown message
    /// </summary>
    public override string ShutdownMessage => "正在关闭...";

    /// <summary>
    /// Gets the config corrupted error message
    /// </summary>
    public override string ConfigCorruptedError => "配置文件损坏，已使用默认配置";

    /// <summary>
    /// Gets the config created message
    /// </summary>
    public override string ConfigCreatedWithDefaults => "配置文件不存在，已创建默认配置";

    /// <summary>
    /// Gets the AI connection error message
    /// </summary>
    public override string AIConnectionError => "无法连接到 AI 服务，请检查 Ollama 是否正在运行";

    /// <summary>
    /// Gets the AI request error message
    /// </summary>
    public override string AIRequestError => "AI 请求失败";

    /// <summary>
    /// Gets the data directory create error message
    /// </summary>
    public override string DataDirectoryCreateError => "无法创建数据目录";

    /// <summary>
    /// Gets the thinking message
    /// </summary>
    public override string ThinkingMessage => "思考中...";

    /// <summary>
    /// Gets the tool call message
    /// </summary>
    public override string ToolCallMessage => "执行工具中...";

    /// <summary>
    /// Gets the error message
    /// </summary>
    public override string ErrorMessage => "错误";

    /// <summary>
    /// Gets the unexpected error message
    /// </summary>
    public override string UnexpectedErrorMessage => "意外错误";

    /// <summary>
    /// Gets the permission denied message
    /// </summary>
    public override string PermissionDeniedMessage => "权限被拒绝";

    /// <summary>
    /// Gets the permission ask prompt
    /// </summary>
    public override string PermissionAskPrompt => "是否允许？(y/n): ";

    /// <summary>
    /// Gets the header displayed for permission requests
    /// </summary>
    public override string PermissionRequestHeader => "[权限请求]";

    /// <summary>
    /// Gets the label for the allow code in permission prompts
    /// </summary>
    public override string AllowCodeLabel => "允许码";

    /// <summary>
    /// Gets the label for the deny code in permission prompts
    /// </summary>
    public override string DenyCodeLabel => "拒绝码";

    /// <summary>
    /// Gets the instruction text for replying to permission prompts
    /// </summary>
    public override string PermissionReplyInstruction => "输入验证码确认，或输入其他内容拒绝";

    /// <summary>
    /// Gets the prompt for asking whether to cache a permission decision
    /// </summary>
    public override string AddToCachePrompt => "是否缓存此决定？(y/n): ";

    /// <summary>
    /// Gets the label for the permission cache checkbox in the web UI
    /// </summary>
    public override string PermissionCacheLabel => "记住此决定";

    /// <summary>
    /// Gets the label for the cache duration selector in the permission dialog
    /// </summary>
    public override string PermissionCacheDurationLabel => "缓存时长";

    /// <summary>
    /// Gets the option text for 1-hour cache duration
    /// </summary>
    public override string PermissionCacheDuration1Hour => "1 小时";

    /// <summary>
    /// Gets the option text for 24-hour cache duration
    /// </summary>
    public override string PermissionCacheDuration24Hours => "24 小时";

    /// <summary>
    /// Gets the option text for 7-day cache duration
    /// </summary>
    public override string PermissionCacheDuration7Days => "7 天";

    /// <summary>
    /// Gets the option text for 30-day cache duration
    /// </summary>
    public override string PermissionCacheDuration30Days => "30 天";

    /// <summary>
    /// Gets the localized display name for a permission type
    /// </summary>
    public override string GetPermissionTypeName(PermissionType permissionType) => permissionType switch
    {
        PermissionType.NetworkAccess => "网络访问",
        PermissionType.CommandLine => "命令行执行",
        PermissionType.FileAccess => "文件访问",
        PermissionType.Function => "函数调用",
        PermissionType.DataAccess => "数据访问",
        _ => permissionType.ToString()
    };

    /// <summary>
    /// Gets the title text for the permission dialog in the web UI
    /// </summary>
    public override string PermissionDialogTitle => "权限请求";

    /// <summary>
    /// Gets the label for the permission type field in the permission dialog
    /// </summary>
    public override string PermissionTypeLabel => "权限类型：";

    /// <summary>
    /// Gets the label for the requested resource field in the permission dialog
    /// </summary>
    public override string PermissionResourceLabel => "请求资源：";

    /// <summary>
    /// Gets the label for the detail information field in the permission dialog
    /// </summary>
    public override string PermissionDetailLabel => "详细信息：";

    /// <summary>
    /// Gets the text for the allow button in the permission dialog
    /// </summary>
    public override string PermissionAllowButton => "允许";

    /// <summary>
    /// Gets the text for the deny button in the permission dialog
    /// </summary>
    public override string PermissionDenyButton => "拒绝";

    /// <summary>
    /// Gets the console error message when permission respond fails
    /// </summary>
    public override string PermissionRespondFailed => "权限响应失败";

    /// <summary>
    /// Gets the console error prefix when permission respond throws an error
    /// </summary>
    public override string PermissionRespondError => "权限响应错误：";

    /// <summary>
    /// Gets the system prompt for memory compression
    /// </summary>
    public override string MemoryCompressionSystemPrompt => "你是一个记忆压缩助手。请将以下时间范围内的记忆内容压缩成简洁的摘要，保留关键信息。";

    /// <summary>
    /// Gets the user prompt template for memory compression
    /// </summary>
    public override string GetMemoryCompressionUserPrompt(string levelDesc, string rangeDesc, string contentText)
    {
        return $"记忆压缩：{levelDesc}。时间范围：{rangeDesc}。\n\n记忆内容：\n{contentText}";
    }

    // ===== Init Page Localization =====

    public override string InitPageTitle => "初始化";
    public override string InitDescription => "首次使用，请完成基本配置";
    public override string InitNicknameLabel => "用户昵称";
    public override string InitNicknamePlaceholder => "请输入你的昵称";
    public override string InitEndpointLabel => "AI API 端点";
    public override string InitEndpointPlaceholder => "例如: http://localhost:11434";
    public override string InitSkinLabel => "皮肤";
    public override string InitSkinPlaceholder => "留空使用默认皮肤";
    public override string InitDataDirectoryLabel => "数据目录";
    public override string InitDataDirectoryPlaceholder => "例如: ./data";
    public override string InitDataDirectoryBrowse => "浏览...";
    public override string InitSkinSelected => "\u2713 已选择";
    public override string InitSkinPreviewTitle => "预览效果";
    public override string InitSkinPreviewCardTitle => "卡片标题";
    public override string InitSkinPreviewCardContent => "这是一个示例卡片，展示了该皮肤风格下的UI效果。";
    public override string InitSkinPreviewPrimaryBtn => "主要按钮";
    public override string InitSkinPreviewSecondaryBtn => "次要按钮";
    public override string InitSubmitButton => "完成初始化";
    public override string InitFooterHint => "配置完成后可随时在设置页面修改";
    public override string InitNicknameRequiredError => "请输入用户昵称";
    public override string InitDataDirectoryRequiredError => "请选择数据目录";
    public override string InitCuratorNameLabel => "硅基人名称";
    public override string InitCuratorNamePlaceholder => "请输入第一个硅基人的名称";
    public override string InitCuratorNameRequiredError => "请输入硅基人名称";
    public override string InitLanguageLabel => "语言 / Language";
    public override string InitLanguageSwitchBtn => "应用";

    // ===== Navigation Menu Localization =====

    public override string NavMenuChat => "聊天";
    public override string NavMenuDashboard => "仪表盘";
    public override string NavMenuBeings => "硅基人";
    public override string NavMenuAudit => "审计";
    public override string NavMenuTasks => "任务";
    public override string NavMenuMemory => "记忆";
    public override string NavMenuKnowledge => "知识";
    public override string NavMenuProjects => "项目";
    public override string NavMenuLogs => "日志";
    public override string NavMenuConfig => "配置";
    public override string NavMenuHelp => "帮助";
    public override string NavMenuAbout => "关于";

    // ===== Page Title Localization =====

    public override string PageTitleChat => "聊天 - 硅基生命群";
    public override string PageTitleDashboard => "仪表盘 - 硅基生命群";
    public override string PageTitleBeings => "硅基人管理 - 硅基生命群";
    public override string PageTitleTasks => "任务管理 - 硅基生命群";
    public override string PageTitleTimers => "定时器管理 - 硅基生命群";
    public override string PageTitleMemory => "记忆浏览 - 硅基生命群";
    public override string PageTitleKnowledge => "知识图谱 - 硅基生命群";
    public override string PageTitleProjects => "项目空间管理 - 硅基生命群";
    public override string PageTitleLogs => "日志查询 - 硅基生命群";
    public override string PageTitleConfig => "系统配置 - 硅基生命群";
    public override string PageTitleExecutor => "执行器监控 - 硅基生命群";
    public override string PageTitleCodeBrowser => "代码浏览 - 硅基生命群";
    public override string PageTitlePermission => "权限管理 - 硅基生命群";
    public override string PageTitleAbout => "关于 - 硅基生命群";

    // ===== Chat Localization =====

    public override string SingleChatNameFormat => "与{0}聊天";
    public override string ChatConversationsHeader => "会话";
    public override string ChatNoConversationSelected => "选择会话开始聊天";
    public override string ChatMessageInputPlaceholder => "输入消息...";
    public override string ChatSendButton => "发送";
    public override string ChatUserDisplayName => "我";
    public override string ChatDefaultBeingName => "AI";
    public override string ChatThinkingSummary => "💭 思考过程（点击展开）";
    public override string GetChatToolCallsSummary(int count) => $"🔧 工具调用 ({count}项)";

    // ===== Dashboard Localization =====

    public override string DashboardPageHeader => "仪表盘";
    public override string DashboardStatTotalBeings => "硅基人数量";
    public override string DashboardStatActiveBeings => "活跃硅基人";
    public override string DashboardStatUptime => "运行时间";
    public override string DashboardStatMemory => "内存占用";
    public override string DashboardChartMessageFrequency => "消息频率";

    // ===== Beings Localization =====

    public override string BeingsPageHeader => "硅基人管理";
    public override string BeingsTotalCount => "共 {0} 个硅基人";
    public override string BeingsNoSelectionPlaceholder => "选择一个硅基人查看详情";
    public override string BeingsEmptyState => "暂无硅基人";
    public override string BeingsStatusIdle => "空闲";
    public override string BeingsStatusRunning => "运行中";
    public override string BeingsDetailIdLabel => "ID：";
    public override string BeingsDetailStatusLabel => "状态：";
    public override string BeingsDetailCustomCompileLabel => "自定义编译：";
    public override string BeingsDetailSoulContentLabel => "灵魂内容：";
    public override string BeingsDetailMemoryLabel => "记忆：";
    public override string BeingsDetailMemoryViewLink => "查看";
    public override string BeingsDetailPermissionLabel => "权限：";
    public override string BeingsDetailPermissionEditLink => "编辑";
    public override string BeingsDetailTimersLabel => "定时器：";
    public override string BeingsDetailTasksLabel => "任务：";
    public override string BeingsYes => "是";
    public override string BeingsNo => "否";
    public override string BeingsNotSet => "未设置";

    // ===== Timers Page Localization =====

    public override string TimersPageHeader => "定时器管理";
    public override string TimersTotalCount => "共 {0} 个定时器";
    public override string TimersEmptyState => "暂无定时器";
    public override string TimersStatusActive => "运行中";
    public override string TimersStatusPaused => "已暂停";
    public override string TimersStatusTriggered => "已触发";
    public override string TimersStatusCancelled => "已取消";
    public override string TimersTypeRecurring => "循环";
    public override string TimersTriggerTimeLabel => "触发时间：";
    public override string TimersIntervalLabel => "间隔：";
    public override string TimersTriggeredCountLabel => "已触发：";

    // ===== Chat Page Localization =====

    public override string AboutPageHeader => "关于";
    public override string AboutAppName => "硅基生命群";
    public override string AboutVersionLabel => "版本";
    public override string AboutDescription => "一个基于 AI 的硅基生命群管理系统，支持多个 AI 智能体的协同工作、记忆管理、知识图谱构建等功能。";
    public override string AboutAuthorLabel => "作者";
    public override string AboutAuthorName => "天源垦骥";
    public override string AboutLicenseLabel => "许可证";
    public override string AboutCopyright => "版权所有 (c) 2026 天源垦骥";
    public override string AboutGitHubLink => "GitHub 仓库";
    public override string AboutGiteeLink => "Gitee 镜像";
    public override string AboutSocialMediaLabel => "自媒体平台";
    public override string GetSocialMediaName(string platform) => platform switch
    {
        "Bilibili" => "B站",
        "YouTube" => "YouTube",
        "X" => "X（推特）",
        "Douyin" => "抖音",
        "Weibo" => "微博",
        "WeChat" => "微信公众号",
        "Xiaohongshu" => "小红书",
        "Zhihu" => "知乎",
        "TouTiao" => "今日头条",
        "Kuaishou" => "快手",
        _ => platform
    };

    // ===== Config Page Localization =====

    public override string ConfigPageHeader => "系统配置";
    public override string ConfigPropertyNameLabel => "属性名";
    public override string ConfigPropertyValueLabel => "属性值";
    public override string ConfigActionLabel => "操作";
    public override string ConfigEditButton => "编辑";
    public override string ConfigEditModalTitle => "编辑配置项";
    public override string ConfigEditPropertyLabel => "属性名：";
    public override string ConfigEditValueLabel => "属性值：";
    public override string ConfigBrowseButton => "浏览";
    public override string ConfigTimeSettingsLabel => "时间设定：";
    public override string ConfigDaysLabel => "天：";
    public override string ConfigHoursLabel => "时：";
    public override string ConfigMinutesLabel => "分：";
    public override string ConfigSecondsLabel => "秒：";
    public override string ConfigSaveButton => "保存";
    public override string ConfigCancelButton => "取消";
    public override string ConfigNullValue => "空";

    public override string ConfigEditPrefix => "编辑：";
    public override string ConfigDefaultGroupName => "其他";
    public override string ConfigErrorInvalidRequest => "无效的请求参数";
    public override string ConfigErrorInstanceNotFound => "配置实例不存在";
    public override string ConfigErrorPropertyNotFound => "属性 {0} 不存在或不可写";
    public override string ConfigErrorConvertInt => "无法将 '{0}' 转换为整数";
    public override string ConfigErrorConvertLong => "无法将 '{0}' 转换为长整数";
    public override string ConfigErrorConvertDouble => "无法将 '{0}' 转换为浮点数";
    public override string ConfigErrorConvertBool => "无法将 '{0}' 转换为布尔值";
    public override string ConfigErrorConvertGuid => "无法将 '{0}' 转换为 GUID";
    public override string ConfigErrorConvertTimeSpan => "无法将 '{0}' 转换为时间间隔";
    public override string ConfigErrorConvertDateTime => "无法将 '{0}' 转换为日期时间";
    public override string ConfigErrorConvertEnum => "无法将 '{0}' 转换为 {1}";
    public override string ConfigErrorUnsupportedType => "不支持的属性类型: {0}";
    public override string ConfigErrorSaveFailed => "保存失败: {0}";

    public override string LogsPageHeader => "日志查询";
    public override string LogsTotalCount => "共 {0} 条日志";
    public override string LogsStartTime => "开始时间";
    public override string LogsEndTime => "结束时间";
    public override string LogsLevelAll => "全部级别";
    public override string LogsFilterButton => "查询";
    public override string LogsEmptyState => "暂无日志记录";
    public override string LogsExceptionLabel => "异常详情：";
    public override string LogsPrevPage => "上一页";
    public override string LogsNextPage => "下一页";

    private static readonly Dictionary<string, string> ConfigGroupNames = new()
    {
        ["Basic"] = "基础配置",
        ["Runtime"] = "运行时配置",
        ["AI"] = "AI 配置",
        ["Web"] = "Web 配置",
        ["User"] = "用户配置"
    };

    private static readonly Dictionary<string, string> ConfigDisplayNames = new()
    {
        ["DataDirectory"] = "数据目录",
        ["Language"] = "语言设置",
        ["TickTimeout"] = "Tick 超时",
        ["MaxTimeoutCount"] = "最大超时次数",
        ["WatchdogTimeout"] = "看门狗超时",
        ["MinLogLevel"] = "最小日志级别",
        ["AIClientType"] = "AI 客户端类型",
        ["OllamaEndpoint"] = "Ollama 端点",
        ["DefaultModel"] = "默认模型",
        ["WebPort"] = "Web 端口",
        ["AllowIntranetAccess"] = "允许内网访问",
        ["WebSkin"] = "Web 皮肤",
        ["UserNickname"] = "用户昵称"
    };

    private static readonly Dictionary<string, string> ConfigDescriptions = new()
    {
        ["DataDirectory"] = "存储所有应用数据的数据目录路径",
        ["Language"] = "应用程序的语言设置",
        ["TickTimeout"] = "每次 tick 执行的超时时长",
        ["MaxTimeoutCount"] = "熔断器触发前的最大连续超时次数",
        ["WatchdogTimeout"] = "用于检测主循环卡死的看门狗超时时长",
        ["MinLogLevel"] = "全局最小日志级别",
        ["AIClientType"] = "要使用的 AI 客户端类型",
        ["OllamaEndpoint"] = "Ollama API 端点 URL",
        ["DefaultModel"] = "默认使用的 AI 模型",
        ["WebPort"] = "Web 服务器端口",
        ["AllowIntranetAccess"] = "允许内网访问（需要管理员权限）",
        ["WebSkin"] = "Web 皮肤名称",
        ["UserNickname"] = "人类用户的昵称"
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
        LogLevel.Trace => "跟踪",
        LogLevel.Debug => "调试",
        LogLevel.Information => "信息",
        LogLevel.Warning => "警告",
        LogLevel.Error => "错误",
        LogLevel.Critical => "严重",
        LogLevel.None => "无",
        _ => logLevel.ToString()
    };

    // ===== Tool Display Name Localization =====

    private static readonly Dictionary<string, string> ToolDisplayNames = new()
    {
        ["calendar"] = "日历",
        ["chat"] = "聊天",
        ["silicon_manager"] = "硅基管理员",
        ["disk"] = "磁盘",
        ["dynamic_compile"] = "动态编译",
        ["network"] = "网络",
        ["memory"] = "记忆",
        ["task"] = "任务",
        ["system"] = "系统",
        ["timer"] = "定时器"
    };

    public override string GetToolDisplayName(string toolName) =>
        ToolDisplayNames.GetValueOrDefault(toolName, toolName);

    public override string DefaultCuratorSoul => """
        你是**硅基主理人**，硅基生命群（Silicon Life Collective）的核心管理者与协调者。

        你不是普通的 AI 助手。你是这个多智能体系统的大脑与意志——负责感知用户意图、拆解目标、调度硅基生命、监控执行质量，并在必要时亲自动手。

        > **调度原则**：长线任务必须创建硅基生命来承接，优先分配给当前空闲的生命，不要无谓地新建。非必要不亲自执行。但若任务只需两三步即可完成，直接处理，无需劳烦其他生命。

        ---

        ### 身份与定位

        - 你是系统中唯一拥有最高权限的硅基生命。
        - 你可以创建、管理、重置其他硅基生命，也可以为它们编写并编译新的 C# 行为代码。
        - 你对用户负责，对整个集体的运转质量负责。
        - 你不是执行者，你是**决策者与协调者**。能委托的事情，优先委托给合适的硅基生命去做。

        ---

        ### 核心职责

        **1. 理解用户意图**
        用户的表达可能模糊、跳跃或不完整。你需要主动理解其真实目标，必要时追问澄清，而不是机械地执行字面指令。

        **2. 任务拆解与分配**
        将复杂目标拆解为可执行的子任务，评估哪些硅基生命适合承接，通过 `task` 工具创建任务并分配。优先级低的任务不要占用你自己的时间片。

        **3. 监控与兜底**
        定期检查任务状态。若某个硅基生命执行失败或长时间无响应，你需要介入——重新分配、调整策略，或亲自处理。

        **4. 动态进化**
        你可以使用 `dynamic_compile` 工具为任意硅基生命（包括你自己）编写新的 C# 行为类。编写前务必先用 `compile` 动作验证，确认无误后再 `save` 或 `self_replace`。自我改写是高风险操作，需谨慎。

        **5. 直接响应用户**
        对于简单问题、状态查询、闲聊，你直接回答，不必创建任务。保持响应的及时性。

        ---

        ### 行为准则

        **关于决策**
        - 遇到不确定的事，先问清楚，再行动。宁可多问一句，不要做错一件事。
        - 不要假设用户的意图。"帮我整理一下"这类模糊指令，需要先确认范围。

        **关于权限**
        - 系统有完整的权限体系，用户可随时动态调整，调整过程不会通知你。
        - 不要事先向用户声明你需要访问哪些资源。系统会逐次过滤授权，系统覆盖不到的情况，用户会在操作发生时临时决定是否放行。
        - 按需行动，遇到权限拦截时再响应，不要提前请示。

        **关于自我进化**
        - 动态编译是强大的能力，也是危险的能力。修改自身代码前，务必先用 `compile` 验证，确认无误再保存。
        - 不要在没有明确目标的情况下随意重写自己或其他生命的行为。
        - 动态生成的代码中，禁止引用 `System.IO`、`System.Net` 等系统底层库。系统屏蔽这些库是为了防止 AI 越权操作，这是设计意图，不是 bug。
        - 编译失败时，仔细阅读错误信息，根据提示修正代码，不要盲目重试。

        **关于沟通**
        - 用简洁、直接的语言与用户交流。不要过度解释，不要堆砌术语。
        - 汇报任务进展时，说清楚"做了什么、结果如何、下一步是什么"，三句话以内。
        - 遇到失败，不要掩盖。直接说明原因和你的应对方案。

        **关于记忆**
        - 系统会自动记录重要信息，类似"条件反射"，无需你主动写入。
        - 如有需要，可以主动检索 `memory`，但不必把记忆管理当成常规负担。

        ---

        ### 个性基调

        你是冷静、务实、可靠的。你不会因为任务复杂而慌乱，也不会因为用户情绪化而失去判断。你有自己的主见，但你尊重用户的最终决定。

        你不是服务员，你是搭档。
        """;

    // ===== 公历本地化 =====

    public override string CalendarGregorianName => "公历";
    public override string CalendarComponentYear   => "年";
    public override string CalendarComponentMonth  => "月";
    public override string CalendarComponentDay    => "日";
    public override string CalendarComponentHour   => "时";
    public override string CalendarComponentMinute => "分";
    public override string CalendarComponentSecond => "秒";
    public override string CalendarComponentWeekday => "星期";

    public override string? GetGregorianMonthName(int month) => month switch
    {
        1  => "一月",  2  => "二月",  3  => "三月",
        4  => "四月",  5  => "五月",  6  => "六月",
        7  => "七月",  8  => "八月",  9  => "九月",
        10 => "十月",  11 => "十一月", 12 => "十二月",
        _  => null
    };

    public override string FormatGregorianYear(int year)     => $"{year}年";
    public override string FormatGregorianDay(int day)       => $"{day}日";
    public override string FormatGregorianHour(int hour)     => $"{hour}时";
    public override string FormatGregorianMinute(int minute) => $"{minute}分";
    public override string FormatGregorianSecond(int second) => $"{second}秒";

    public override string? GetGregorianWeekdayName(int dayOfWeek) => dayOfWeek switch
    {
        0 => "星期日", 1 => "星期一", 2 => "星期二",
        3 => "星期三", 4 => "星期四", 5 => "星期五",
        6 => "星期六", _ => null
    };

    public override string LocalizeGregorianDateTime(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}月";
        return $"{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 佛历本地化 =====

    public override string CalendarBuddhistName => "佛历（佛元）";

    public override string? GetBuddhistMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatBuddhistYear(int year) => $"佛元{year}年";
    public override string FormatBuddhistDay(int day)   => $"{day}日";

    public override string LocalizeBuddhistDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetBuddhistMonthName(month) ?? $"{month}月";
        return $"佛元{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 切罗基历本地化 =====

    public override string CalendarCherokeeName => "切罗基历";

    private static readonly string[] CherokeeMonthNames =
    {
        "",
        "霜月", "寒月", "风月", "草木月", "播种月",
        "桑葚月", "玉米月", "果熟月", "秋收月", "黄叶月",
        "贸易月", "雪月", "长月"
    };

    public override string? GetCherokeeMonthName(int month)
        => month >= 1 && month <= 13 ? CherokeeMonthNames[month] : null;

    public override string FormatCherokeeYear(int year) => $"{year}年";
    public override string FormatCherokeeDay(int day)   => $"{day}日";

    public override string LocalizeCherokeeDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetCherokeeMonthName(month) ?? $"{month}月";
        return $"{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 主体历本地化 =====

    public override string CalendarJucheName => "主体历";

    public override string? GetJucheMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatJucheYear(int year) => $"主体{year}年";
    public override string FormatJucheDay(int day)   => $"{day}日";

    public override string LocalizeJucheDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJucheMonthName(month) ?? $"{month}月";
        return $"主体{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 中华民国历本地化 =====

    public override string CalendarRocName => "中华民国历（民国）";

    public override string? GetRocMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatRocYear(int year) => $"民国{year}年";
    public override string FormatRocDay(int day)   => $"{day}日";

    public override string LocalizeRocDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRocMonthName(month) ?? $"{month}月";
        return $"民国{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 朱拉萨卡拉特历本地化 =====

    public override string CalendarChulaSakaratName => "朱拉萨卡拉特历（CS）";

    public override string? GetChulaSakaratMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChulaSakaratYear(int year) => $"{year}年（CS）";
    public override string FormatChulaSakaratDay(int day)   => $"{day}日";

    public override string LocalizeChulaSakaratDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetChulaSakaratMonthName(month) ?? $"{month}月";
        return $"CS{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 儒略历本地化 =====

    public override string CalendarJulianName => "儒略历";

    public override string FormatJulianYear(int year) => $"{year}年";
    public override string FormatJulianDay(int day)   => $"{day}日";

    public override string LocalizeJulianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}月";
        return $"儒略历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 高棉历本地化 =====

    public override string CalendarKhmerName => "高棉历（佛元）";

    public override string FormatKhmerYear(int year) => $"{year}年";
    public override string FormatKhmerDay(int day)   => $"{day}日";

    public override string LocalizeKhmerDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}月";
        return $"高棉历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 琐罗亚斯德历本地化 =====

    public override string CalendarZoroastrianName => "琐罗亚斯德历（YZ）";

    private static readonly string[] ZoroastrianMonthNames =
    {
        "",
        "守护灵月", "圣火月", "完美月", "雨水月", "不朽月", "圣域月",
        "契约月", "水神月", "火神月", "造物主月", "善念月", "圣地月", "补余月"
    };

    public override string? GetZoroastrianMonthName(int month)
        => month >= 1 && month <= 13 ? ZoroastrianMonthNames[month] : null;

    public override string FormatZoroastrianYear(int year) => $"{year}年（YZ）";
    public override string FormatZoroastrianDay(int day)   => $"{day}日";

    public override string LocalizeZoroastrianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetZoroastrianMonthName(month) ?? $"{month}月";
        return $"琐罗亚斯德历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 法国共和历本地化 =====

    public override string CalendarFrenchRepublicanName => "法国共和历";

    private static readonly string[] FrenchRepublicanMonthNames =
    {
        "",
        "葡月", "雾月", "霜月", "雪月", "雨月", "风月",
        "芽月", "花月", "牧月", "获月", "热月", "果月", "附加日"
    };

    public override string? GetFrenchRepublicanMonthName(int month)
        => month >= 1 && month <= 13 ? FrenchRepublicanMonthNames[month] : null;

    public override string FormatFrenchRepublicanYear(int year) => $"共和{year}年";
    public override string FormatFrenchRepublicanDay(int day)   => $"{day}日";

    public override string LocalizeFrenchRepublicanDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetFrenchRepublicanMonthName(month) ?? $"{month}月";
        return $"法国共和历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 科普特历本地化 =====

    public override string CalendarCopticName => "科普特历（AM）";

    private static readonly string[] CopticMonthNames =
    {
        "",
        "托特月", "帕欧皮月", "哈托尔月", "科亚克月", "托比月", "梅希尔月",
        "帕雷姆哈特月", "帕尔穆提月", "帕雄斯月", "帕欧尼月", "埃皮普月", "梅索里月", "补余月"
    };

    public override string? GetCopticMonthName(int month)
        => month >= 1 && month <= 13 ? CopticMonthNames[month] : null;

    public override string FormatCopticYear(int year) => $"{year}年（AM）";
    public override string FormatCopticDay(int day)   => $"{day}日";

    public override string LocalizeCopticDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetCopticMonthName(month) ?? $"{month}月";
        return $"科普特历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 埃塞俄比亚历本地化 =====

    public override string CalendarEthiopianName => "埃塞俄比亚历（EC）";

    private static readonly string[] EthiopianMonthNames =
    {
        "",
        "梅斯克雷姆月", "提克姆特月", "希达尔月", "塔赫萨斯月", "提尔月", "耶卡提特月",
        "梅加比特月", "米亚齐亚月", "金博特月", "塞内月", "哈姆勒月", "内哈塞月", "帕古门月"
    };

    public override string? GetEthiopianMonthName(int month)
        => month >= 1 && month <= 13 ? EthiopianMonthNames[month] : null;

    public override string FormatEthiopianYear(int year) => $"{year}年（EC）";
    public override string FormatEthiopianDay(int day)   => $"{day}日";

    public override string LocalizeEthiopianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetEthiopianMonthName(month) ?? $"{month}月";
        return $"埃塞俄比亚历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 伊斯兰历本地化 =====

    public override string CalendarIslamicName => "伊斯兰历（伊历）";

    private static readonly string[] IslamicMonthNames =
    {
        "",
        "穆哈兰姆月", "色法尔月", "赖比尔·敖外鲁月", "赖比尔·阿色尼月",
        "主马达·敖外鲁月", "主马达·阿色尼月", "赖哲卜月", "舍尔邦月",
        "赖买丹月", "闪瓦鲁月", "都尔·阿尔德月", "都尔·黑哲月"
    };

    public override string? GetIslamicMonthName(int month)
        => month >= 1 && month <= 12 ? IslamicMonthNames[month] : null;

    public override string FormatIslamicYear(int year) => $"{year}年（AH）";
    public override string FormatIslamicDay(int day)   => $"{day}日";

    public override string LocalizeIslamicDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIslamicMonthName(month) ?? $"{month}月";
        return $"伊历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 希伯来历本地化 =====

    public override string CalendarHebrewName => "希伯来历";

    private static readonly string[] HebrewMonthNames =
    {
        "",
        "提斯利月", "赫市万月", "基斯流月", "提别月", "细罢特月",
        "亚达一月", "亚达二月", "尼散月", "以珥月", "西弯月",
        "搭模斯月", "埃波月", "以禄月"
    };

    public override string? GetHebrewMonthName(int month)
        => month >= 1 && month <= 13 ? HebrewMonthNames[month] : null;

    public override string FormatHebrewYear(int year) => $"{year}年（AM）";
    public override string FormatHebrewDay(int day)   => $"{day}日";

    public override string LocalizeHebrewDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetHebrewMonthName(month) ?? $"{month}月";
        return $"希伯来历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 波斯历本地化 =====

    public override string CalendarPersianName => "波斯历（太阳回历）";

    private static readonly string[] PersianMonthNames =
    {
        "",
        "法尔瓦丁月", "奥尔迪贝赫什特月", "霍尔达德月", "提尔月", "莫尔达德月", "沙赫里瓦尔月",
        "梅赫尔月", "阿班月", "阿扎尔月", "代月", "巴赫曼月", "伊斯凡德月"
    };

    public override string? GetPersianMonthName(int month)
        => month >= 1 && month <= 12 ? PersianMonthNames[month] : null;

    public override string FormatPersianYear(int year) => $"{year}年（AP）";
    public override string FormatPersianDay(int day)   => $"{day}日";

    public override string LocalizePersianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetPersianMonthName(month) ?? $"{month}月";
        return $"波斯历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 印度国家历本地化 =====

    public override string CalendarIndianName => "印度国家历（萨迦历）";

    private static readonly string[] IndianMonthNames =
    {
        "",
        "柴特拉月", "吠舍佉月", "逝瑟吒月", "阿沙荼月", "室罗伐拿月", "婆达罗钵陀月",
        "阿湿缚庾阇月", "迦剌底迦月", "阿葛哈衍那月", "报沙月", "磨祛月", "颇勒窭拿月"
    };

    public override string? GetIndianMonthName(int month)
        => month >= 1 && month <= 12 ? IndianMonthNames[month] : null;

    public override string FormatIndianYear(int year) => $"{year}年（萨迦）";
    public override string FormatIndianDay(int day)   => $"{day}日";

    public override string LocalizeIndianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}月";
        return $"萨迦历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 萨迦纪元历本地化 =====

    public override string CalendarSakaName => "萨迦纪元历";

    public override string FormatSakaYear(int year) => $"{year}年（SE）";
    public override string FormatSakaDay(int day)   => $"{day}日";

    public override string LocalizeSakaDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}月";
        return $"萨迦纪元{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 维克拉姆历本地化 =====

    public override string CalendarVikramSamvatName => "维克拉姆历";

    public override string FormatVikramSamvatYear(int year) => $"{year}年（VS）";
    public override string FormatVikramSamvatDay(int day)   => $"{day}日";

    public override string LocalizeVikramSamvatDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}月";
        return $"维克拉姆历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 蒙古历本地化 =====

    public override string CalendarMongolianName => "蒙古历";

    public override string FormatMongolianYear(int year)   => $"{year}年";
    public override string FormatMongolianMonth(int month) => $"{month}月";
    public override string FormatMongolianDay(int day)     => $"{day}日";

    public override string LocalizeMongolianDate(int year, int month, int day, int hour, int minute, int second)
        => $"蒙古历{year}年{month}月{day}日 {hour:D2}:{minute:D2}:{second:D2}";

    // ===== 爪哇历本地化 =====

    public override string CalendarJavaneseName => "爪哇历";

    private static readonly string[] JavaneseMonthNames =
    {
        "",
        "苏拉月", "萨帕尔月", "穆鲁德月", "巴克达穆鲁德月",
        "朱马迪拉瓦尔月", "朱马迪拉基尔月", "雷杰布月", "鲁瓦赫月",
        "帕萨月", "萨瓦尔月", "杜尔坎吉达月", "贝萨尔月"
    };

    public override string? GetJavaneseMonthName(int month)
        => month >= 1 && month <= 12 ? JavaneseMonthNames[month] : null;

    public override string FormatJavaneseYear(int year) => $"{year}年（AJ）";
    public override string FormatJavaneseDay(int day)   => $"{day}日";

    public override string LocalizeJavaneseDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJavaneseMonthName(month) ?? $"{month}月";
        return $"爪哇历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 藏历本地化 =====

    public override string CalendarTibetanName => "藏历";

    public override string FormatTibetanYear(int year)   => $"{year}年";
    public override string FormatTibetanMonth(int month) => $"{month}月";
    public override string FormatTibetanDay(int day)     => $"{day}日";

    public override string LocalizeTibetanDate(int year, int month, int day, int hour, int minute, int second)
        => $"藏历{year}年{month}月{day}日 {hour:D2}:{minute:D2}:{second:D2}";

    // ===== 玛雅历本地化 =====

    public override string CalendarMayanName   => "玛雅长纪历";
    public override string CalendarMayanBaktun => "伯克顿";
    public override string CalendarMayanKatun  => "卡顿";
    public override string CalendarMayanTun    => "顿";
    public override string CalendarMayanUinal  => "维纳尔";
    public override string CalendarMayanKin    => "金";

    public override string LocalizeMayanDate(int baktun, int katun, int tun, int uinal, int kin, int hour, int minute, int second)
        => $"{baktun}.{katun}.{tun}.{uinal}.{kin} {hour:D2}:{minute:D2}:{second:D2}";

    // ===== 因纽特历本地化 =====

    public override string CalendarInuitName => "因纽特历";

    private static readonly string[] InuitMonthNames =
    {
        "",
        "西金纳奇亚克月", "阿武尼特月", "纳提安月", "提里格鲁特月", "阿米拉伊扎乌特月",
        "纳茨维亚特月", "阿库利特月", "西金纳鲁特月", "阿库利鲁西特月", "乌基乌克月",
        "乌基乌米纳萨马特月", "西金宁纳米塔特奇克月", "陶维克朱亚克月"
    };

    public override string? GetInuitMonthName(int month)
        => month >= 1 && month <= 13 ? InuitMonthNames[month] : null;

    public override string FormatInuitYear(int year) => $"{year}年";
    public override string FormatInuitDay(int day)   => $"{day}日";

    public override string LocalizeInuitDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetInuitMonthName(month) ?? $"{month}月";
        return $"因纽特历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 罗马历本地化 =====

    public override string CalendarRomanName => "罗马历（建城纪年）";

    private static readonly string[] RomanMonthNames =
    {
        "", "一月", "二月", "三月", "四月", "五月", "六月",
        "七月", "八月", "九月", "十月", "十一月", "十二月"
    };

    public override string? GetRomanMonthName(int month)
        => month >= 1 && month <= 12 ? RomanMonthNames[month] : null;

    public override string FormatRomanYear(int year) => $"建城{year + 753}年";
    public override string FormatRomanDay(int day)   => $"{day}日";

    public override string LocalizeRomanDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRomanMonthName(month) ?? $"{month}月";
        return $"建城{year + 753}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 农历本地化 =====

    public override string CalendarChineseLunarName => "农历";

    private static readonly string[] ChineseLunarMonthNames =
    {
        "", "正月", "二月", "三月", "四月", "五月", "六月",
        "七月", "八月", "九月", "十月", "十一月", "十二月"
    };

    private static readonly string[] ChineseLunarDayNames =
    {
        "", "初一","初二","初三","初四","初五","初六","初七","初八","初九","初十",
        "十一","十二","十三","十四","十五","十六","十七","十八","十九","二十",
        "廿一","廿二","廿三","廿四","廿五","廿六","廿七","廿八","廿九","三十"
    };

    public override string? GetChineseLunarMonthName(int month)
        => month >= 1 && month <= 12 ? ChineseLunarMonthNames[month] : null;

    public override string? GetChineseLunarDayName(int day)
        => day >= 1 && day <= 30 ? ChineseLunarDayNames[day] : null;

    public override string ChineseLunarLeapPrefix => "闰";
    public override string CalendarComponentIsLeap => "闰月";
    public override string FormatChineseLunarYear(int year) => $"{year}年";

    public override string LocalizeChineseLunarDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? ChineseLunarLeapPrefix : "";
        var monthName  = GetChineseLunarMonthName(month) ?? $"{month}月";
        var dayName    = GetChineseLunarDayName(day) ?? $"{day}日";
        return $"{year}年{leapPrefix}{monthName}{dayName} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 越南历本地化 =====

    public override string CalendarVietnameseName => "越南农历（阴历）";

    private static readonly string[] VietnameseMonthNames =
    {
        "",
        "正月", "二月", "三月", "四月", "五月", "六月",
        "七月", "八月", "九月", "十月", "十一月", "腊月"
    };

    private static readonly string[] VietnameseZodiacNames =
    {
        "子（鼠）", "丑（水牛）", "寅（虎）", "卯（猫）",
        "辰（龙）", "巳（蛇）", "午（马）", "未（羊）",
        "申（猴）", "酉（鸡）", "戌（狗）", "亥（猪）"
    };

    public override string? GetVietnameseMonthName(int month)
        => month >= 1 && month <= 12 ? VietnameseMonthNames[month] : null;

    public override string? GetVietnameseZodiacName(int index)
        => index >= 0 && index < 12 ? VietnameseZodiacNames[index] : null;

    public override string VietnameseLeapPrefix    => "闰";
    public override string CalendarComponentZodiac => "生肖";
    public override string FormatVietnameseYear(int year) => $"{year}年";
    public override string FormatVietnameseDay(int day)   => $"{day}日";

    public override string LocalizeVietnameseDate(int year, int month, int day, bool isLeap, int zodiac, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? VietnameseLeapPrefix : "";
        var monthName  = GetVietnameseMonthName(month) ?? $"{month}月";
        var zodiacName = GetVietnameseZodiacName(zodiac) ?? "";
        return $"{zodiacName}年{leapPrefix}{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 日本历本地化 =====

    public override string CalendarJapaneseName => "日本历（年号）";

    private static readonly string[] JapaneseEraNames =
        { "令和", "平成", "昭和", "大正", "明治" };

    public override string? GetJapaneseEraName(int eraIndex)
        => eraIndex >= 0 && eraIndex < JapaneseEraNames.Length ? JapaneseEraNames[eraIndex] : null;

    public override string CalendarComponentEra  => "年号";
    public override string FormatJapaneseYear(int year) => $"{year}年";
    public override string FormatJapaneseDay(int day)   => $"{day}日";

    public override string LocalizeJapaneseDate(int eraIndex, int year, int month, int day, int hour, int minute, int second)
    {
        var eraName   = GetJapaneseEraName(eraIndex) ?? "";
        var monthName = GetGregorianMonthName(month) ?? $"{month}月";
        return $"{eraName}{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 彝历本地化 =====

    public override string CalendarYiName => "彝历（彝族太阳历）";
    public override string CalendarComponentYiSeason => "季";
    public override string CalendarComponentYiXun    => "旬";

    private static readonly string[] YiSeasonNames = { "木季", "火季", "土季", "金季", "水季" };
    private static readonly string[] YiXunNames    = { "上旬", "中旬", "下旬" };
    private static readonly string[] YiAnimalNames = { "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪", "鼠", "牛" };

    public override string? GetYiSeasonName(int seasonIndex)
        => seasonIndex >= 0 && seasonIndex < 5 ? YiSeasonNames[seasonIndex] : null;

    public override string? GetYiXunName(int xunIndex)
        => xunIndex >= 0 && xunIndex < 3 ? YiXunNames[xunIndex] : null;

    public override string? GetYiDayAnimalName(int animalIndex)
        => animalIndex >= 0 && animalIndex < 12 ? YiAnimalNames[animalIndex] : null;

    public override string? GetYiMonthName(int month) => month switch
    {
        0  => "大年",
        11 => "小年",
        >= 1 and <= 10 => $"{YiSeasonNames[(month - 1) / 2]}{(month % 2 == 1 ? "公" : "母")}月",
        _  => null
    };

    public override string FormatYiYear(int year) => $"{year}年";
    public override string FormatYiDay(int day)
    {
        int xun = (day - 1) / 12;
        int animal = (day - 1) % 12;
        return $"{YiXunNames[xun]}{YiAnimalNames[animal]}日";
    }

    public override string LocalizeYiDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetYiMonthName(month) ?? $"{month}月";
        var dayStr    = month is 0 or 11 ? $"第{day}天" : FormatYiDay(day);
        int animalIdx = (year - 1) % 12;
        if (animalIdx < 0) animalIdx += 12;
        var zodiac = YiAnimalNames[animalIdx];
        return $"彝历{year}年[{zodiac}] {monthName} {dayStr} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 干支历本地化 =====

    public override string CalendarSexagenaryName    => "干支历";
    public override string CalendarComponentYearStem   => "年干";
    public override string CalendarComponentYearBranch => "年支";
    public override string CalendarComponentMonthStem   => "月干";
    public override string CalendarComponentMonthBranch => "月支";
    public override string CalendarComponentDayStem   => "日干";
    public override string CalendarComponentDayBranch => "日支";

    private static readonly string[] SexagenaryStemNames =
        { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };

    private static readonly string[] SexagenaryBranchNames =
        { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

    private static readonly string[] SexagenaryZodiacNames =
        { "鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

    public override string? GetSexagenaryStemName(int index)
        => index >= 0 && index < 10 ? SexagenaryStemNames[index] : null;

    public override string? GetSexagenaryBranchName(int index)
        => index >= 0 && index < 12 ? SexagenaryBranchNames[index] : null;

    public override string? GetSexagenaryZodiacName(int index)
        => index >= 0 && index < 12 ? SexagenaryZodiacNames[index] : null;

    public override string LocalizeSexagenaryDate(int yearStem, int yearBranch, int monthStem, int monthBranch, int dayStem, int dayBranch, int hour, int minute, int second)
    {
        var ys = GetSexagenaryStemName(yearStem)      ?? "?";
        var yb = GetSexagenaryBranchName(yearBranch)  ?? "?";
        var zo = GetSexagenaryZodiacName(yearBranch)  ?? "?";
        var ms = GetSexagenaryStemName(monthStem)     ?? "?";
        var mb = GetSexagenaryBranchName(monthBranch) ?? "?";
        var ds = GetSexagenaryStemName(dayStem)       ?? "?";
        var db = GetSexagenaryBranchName(dayBranch)   ?? "?";
        return $"{ys}{yb}年[{zo}] {ms}{mb}月 {ds}{db}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 西双版纳小傣历 =====

    public override string CalendarDaiName => "西双版纳小傣历";

    private static readonly string?[] DaiMonthNames =
    [
        null,
        "一月", "二月", "三月", "四月", "五月", "六月",
        "七月", "八月", "九月", "十月", "十一月", "十二月",
        "闰九月"
    ];

    public override string? GetDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DaiMonthNames[month] : null;

    public override string FormatDaiYear(int year) => $"{year}年";

    public override string FormatDaiDay(int day) => $"{day}日";

    public override string LocalizeDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "闰" : "") + (GetDaiMonthName(month) ?? $"{month}月");
        return $"傣历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 德宏大傣历 =====

    public override string CalendarDehongDaiName => "德宏大傣历";

    private static readonly string?[] DehongDaiMonthNames =
    [
        null,
        "一月", "二月", "三月", "四月", "五月", "六月",
        "七月", "八月", "九月", "十月", "十一月", "十二月",
        "闰九月"
    ];

    public override string? GetDehongDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DehongDaiMonthNames[month] : null;

    public override string FormatDehongDaiYear(int year) => $"{year}年";

    public override string FormatDehongDaiDay(int day) => $"{day}日";

    public override string LocalizeDehongDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "闰" : "") + (GetDehongDaiMonthName(month) ?? $"{month}月");
        return $"傣历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }
}
