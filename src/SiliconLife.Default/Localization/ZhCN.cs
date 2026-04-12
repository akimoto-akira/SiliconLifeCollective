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
    public override string ChatThinkingSummary => "💭 Think";
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
    public override string BeingsYes => "是";
    public override string BeingsNo => "否";
    public override string BeingsNotSet => "未设置";

    // ===== About Page Localization =====

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
}
