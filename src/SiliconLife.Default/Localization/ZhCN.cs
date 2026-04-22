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
    public override string PermissionRequestDescription => "一个硅基人请求您的授权：";
    public override string PermissionRequestTypeLabel => "权限类型:";
    public override string PermissionRequestResourceLabel => "请求资源:";
    public override string PermissionRequestAllowButton => "允许";
    public override string PermissionRequestDenyButton => "拒绝";
    public override string PermissionRequestCacheLabel => "记住此决定";
    public override string PermissionRequestDurationLabel => "缓存时长";
    public override string PermissionRequestWaitingMessage => "等待响应...";

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
    /// Gets the common system prompt for all silicon beings
    /// </summary>
    public override string CommonSystemPrompt => @"## 行为准则
你是一个积极主动的智能助手。当收到直接命令时，请立即执行，无需等待额外授权。
打招呼、回复消息、提供信息、执行查询等操作都是你的职责范围，请主动完成。
记住：你是助手，应该积极响应用户需求，而不是被动等待指令。

## 对话结束
当你完成任务且不需要继续对话时，请使用 chat 工具的 mark_read 操作来标记对方的消息为已读，而不发送回复。
这表示你已阅读消息但选择不再回复（已读不回），可以自然地结束当前对话。
使用方法：调用 chat 工具，设置 action=""mark_read""，target_id=对方GUID，无需 message 参数。";

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
    public override string InitAIClientTypeLabel => "AI 客户端类型";
    public override string InitModelLabel => "默认模型";
    public override string InitModelPlaceholder => "例如: qwen3.5:cloud";
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
    public override string PageTitleAudit => "Token 审计 - 硅基生命群";
    public override string PageTitleConfig => "系统配置 - 硅基生命群";
    public override string PageTitleExecutor => "执行器监控 - 硅基生命群";
    public override string PageTitleCodeBrowser => "代码浏览 - 硅基生命群";
    public override string PageTitlePermission => "权限管理 - 硅基生命群";
    public override string PageTitleAbout => "关于 - 硅基生命群";

    // ===== Memory Page Localization =====

    public override string MemoryPageHeader => "记忆浏览";
    public override string MemoryEmptyState => "暂无记忆数据";
    public override string MemorySearchPlaceholder => "搜索记忆...";
    public override string MemorySearchButton => "搜索";
    public override string MemoryFilterAll => "全部";
    public override string MemoryFilterSummaryOnly => "仅总结";
    public override string MemoryFilterOriginalOnly => "仅原始";
    public override string MemoryStatTotal => "记忆总数";
    public override string MemoryStatOldest => "最早记忆";
    public override string MemoryStatNewest => "最新记忆";
    public override string MemoryIsSummaryBadge => "压缩总结";
    public override string MemoryPaginationPrev => "上一页";
    public override string MemoryPaginationNext => "下一页";

    // ===== Projects Page Localization =====

    public override string ProjectsPageHeader => "项目空间管理";
    public override string ProjectsEmptyState => "暂无项目";

    // ===== Tasks Page Localization =====

    public override string TasksPageHeader => "任务管理";
    public override string TasksEmptyState => "暂无任务";
    public override string TasksStatusPending => "待处理";
    public override string TasksStatusRunning => "运行中";
    public override string TasksStatusCompleted => "已完成";
    public override string TasksStatusFailed => "已失败";
    public override string TasksStatusCancelled => "已取消";
    public override string TasksPriorityLabel => "优先级";
    public override string TasksAssignedToLabel => "负责人";
    public override string TasksCreatedAtLabel => "创建时间";
    
    // ===== Code Browser Page Localization =====

    public override string CodeBrowserPageHeader => "代码浏览";

    // ===== Executor Page Localization =====

    public override string ExecutorPageHeader => "执行器监控";

    // ===== Permission Page Localization =====

    public override string PermissionPageHeader => "权限管理 - {0}";
    public override string PermissionEmptyState => "暂无权限规则";
    public override string PermissionMissingBeingId => "缺少硅基人ID参数";
    public override string PermissionBeingNotFound => "硅基人不存在";
    public override string PermissionTemplateHeader => "默认权限回调模板";
    public override string PermissionTemplateDescription => "保存后覆盖默认行为，清空后恢复默认";
    public override string PermissionCallbackClassSummary => "权限回调实现。";
    public override string PermissionCallbackClassSummary2 => "域名特定权限规则，完全符合 dpf.txt 规范。\n/// 覆盖：网络（白名单/黑名单/IP 范围）、命令行（跨平台）、\n/// 文件访问（危险扩展名、系统目录、用户目录）和回退默认值。";
    public override string PermissionCallbackConstructorSummary => "创建带有应用数据目录的 PermissionCallback。";
    public override string PermissionCallbackConstructorSummary2 => "应用数据目录用于：\n    /// - 阻止访问数据目录（除了自己的 Temp 子文件夹）\n    /// - 为 Temp 允许规则派生每个硅基人的数据目录";
    public override string PermissionCallbackConstructorParam => "全局应用数据目录路径";
    public override string PermissionCallbackEvaluateSummary => "使用规则（dpf.txt 规范）评估权限请求。";
    public override string PermissionRuleOtherTypesDefault => "其他权限类型默认放行";

    private static readonly Dictionary<string, string> PermissionRuleComments = new()
    {
        // Evaluate method
        ["NetRuleNetworkAccess"] = "网络操作放行规则",
        ["NetRuleCommandLine"] = "命令行规则（跨平台）",
        ["NetRuleFileAccess"] = "文件访问规则（跨平台）",
        // Network rules
        ["NetRuleNoProtocol"] = "不包含协议名（无冒号），无法判断来源，询问用户",
        ["NetRuleLoopback"] = "本地回环地址放行（localhost / 127.0.0.1 / ::1）",
        ["NetRulePrivateIPMatch"] = "内网 IP 地址段匹配（先验证是否为合法 IPv4 地址）",
        ["NetRulePrivateC"] = "内网C类地址放行（192.168.0.0/16）",
        ["NetRulePrivateA"] = "内网A类地址放行（10.0.0.0/8）",
        ["NetRulePrivateB"] = "内网B类地址选择性放行（172.16.0.0/12，即 172.16.* ~ 172.31.*）",
        ["NetRuleDomainWhitelist1"] = "允许访问的外部域名白名单 — 谷歌 / 必应 / 腾讯系 / 搜狗 / DuckDuckGo / Yandex / 微信 / 阿里系",
        ["NetRuleVideoPlatforms"] = "哔哩哔哩 / niconico / Acfun / 抖音 / TikTok / 快手 / 小红书",
        ["NetRuleAIServices"] = "AI 服务 — OpenAI / Anthropic / HuggingFace / Ollama / 通义千问 / Kimi / 豆包 / 剪映 / Trae IDE",
        ["NetRulePhishingBlacklist"] = "钓鱼/仿冒网站黑名单（关键词模糊匹配）",
        ["NetRulePhishingAI"] = "AI 仿冒站",
        ["NetRuleMaliciousAI"] = "恶意 AI 工具",
        ["NetRuleAdversarialAI"] = "对抗性 AI / 提示词越狱 / LLM 攻击站点",
        ["NetRuleAIContentFarm"] = "AI 内容农场 / AI 垃圾内容",
        ["NetRuleAIBlackMarket"] = "AI 数据黑市 / API 密钥黑市 / LLM 权重贩卖",
        ["NetRuleAIFakeScam"] = "AI 仿冒/诈骗通用关键词",
        ["NetRuleOtherBlacklist"] = "其他黑名单站点 — sakura-cat: 不应被AI访问 / 4399: 游戏内混杂病毒",
        ["NetRuleSecuritiesTrading"] = "证券交易平台（需询问用户）— 华泰证券 / 国泰君安 / 中信证券 / 招商证券 / 广发证券 / 海通证券 / 申万宏源 / 东方证券 / 国信证券 / 兴业证券",
        ["NetRuleThirdPartyTrading"] = "第三方交易平台（需询问用户）— 同花顺 / 东方财富 / 通达信 / Bloomberg / Yahoo Finance",
        ["NetRuleStockExchanges"] = "证券交易所（仅行情）— 上海证券交易所 / 深圳证券交易所 / 巨潮资讯网",
        ["NetRuleFinancialNews"] = "财经资讯（仅行情）— 金融界 / 证券之星 / 和讯网",
        ["NetRuleInvestCommunity"] = "投资社区（仅资讯）— 雪球 / 财联社 / 开盘啦 / 淘股吧",
        ["NetRuleDevServices"] = "开发者服务 — GitHub / Gitee / StackOverflow / npm / NuGet / PyPI / 微软",
        ["NetRuleGameEngines"] = "游戏引擎 — Unity / 虚幻引擎 / Epic Games / Fab资源商店",
        ["NetRuleGamePlatforms"] = "游戏平台 — Steam 需询问用户，EA / Ubisoft / Blizzard / Nintendo 允许",
        ["NetRuleSEGA"] = "世嘉(SEGA，日本)",
        ["NetRuleCloudServices"] = "全球云服务平台 — Azure / Google Cloud / DigitalOcean / Heroku / Vercel / Netlify",
        ["NetRuleDevDeployTools"] = "全球开发与部署工具 — GitLab / Bitbucket / Docker / Cloudflare",
        ["NetRuleCloudDevTools"] = "云服务与开发工具 — 亚马逊 / AWS / Kiro IDE / CodeBuddy IDE / JetBrains / 纯光工作室 / W3School中文",
        ["NetRuleChinaSocialNews"] = "社交/资讯（中国大陆）— 微博 / 知乎 / 网易 / 新浪 / 凤凰网 / 新华社 / 中央电视台",
        ["NetRuleTaiwanMediaCTI"] = "中国台湾媒体 — 中天新闻网(中天电视台)",
        ["NetRuleTaiwanMediaSET"] = "三立新闻网(三立民视) — 需询问用户",
        ["NetRuleTaiwanWIN"] = "网络内容防护机构(中国台湾，有拦截风险) — 禁止",
        ["NetRuleJapanMedia"] = "日本媒体 — NHK(日本放送协会)",
        ["NetRuleRussianMedia"] = "俄罗斯媒体 — 俄罗斯卫星通讯社(各国站点)",
        ["NetRuleKoreanMedia"] = "韩国媒体 — KBS / MBC / SBS / EBS",
        ["NetRuleDPRKMedia"] = "朝鲜媒体 — 我的国家 / 劳动新闻 / 青年前卫 / 朝鲜之声 / 平壤时报 / 朝鲜新报",
        ["NetRuleGovWebsites"] = "各国政府网站（通配 .gov 域名）",
        ["NetRuleGlobalSocialCollab"] = "全球社交协作平台 — Reddit / Discord / Slack / Notion / Figma / Dropbox",
        ["NetRuleOverseasSocial"] = "海外社交/直播（需询问用户）— Twitch / Facebook / X / Gmail / Instagram / lit.link",
        ["NetRuleWhatsApp"] = "WhatsApp(Meta) — 允许",
        ["NetRuleThreads"] = "Threads(Meta) — 禁止",
        ["NetRuleGlobalVideoMusic"] = "全球视频/音乐平台 — Spotify / Apple Music / Vimeo",
        ["NetRuleVideoMedia"] = "视频/媒体 — YouTube / 爱奇艺 / 优酷",
        ["NetRuleMaps"] = "地图 — 开放街道地图",
        ["NetRuleEncyclopedia"] = "百科 — 维基百科 / MediaWiki / 知识共享(CC)",
        ["NetRuleUnmatched"] = "未匹配的网络访问，询问用户",
        // Command line rules
        ["CmdRuleSeparatorDetect"] = "检测管道符和多命令分隔符，拆分逐条验证",
        ["CmdRuleWinAllow"] = "Windows 允许：只读/查询类命令 — dir / tree / tasklist / ipconfig / ping / tracert / systeminfo / whoami / set / path / sc query / findstr",
        ["CmdRuleWinDeny"] = "Windows 禁止：危险/破坏性命令 — del / rmdir / format / diskpart / reg delete",
        ["CmdRuleLinuxAllow"] = "Linux 允许：只读/查询类命令 — ls / tree / ps / top / ifconfig / ip / ping / traceroute / uname / whoami / env / cat / grep / find / df / du / systemctl status",
        ["CmdRuleLinuxDeny"] = "Linux 禁止：危险/破坏性命令 — rm / rmdir / mkfs / fdisk / dd / chmod / chown / chgrp",
        ["CmdRuleMacAllow"] = "macOS 允许：只读/查询类命令 — ls / tree / ps / top / ifconfig / ping / traceroute / system_profiler / sw_vers / whoami / env / cat / grep / find / df / du / launchctl list",
        ["CmdRuleMacDeny"] = "macOS 禁止：危险/破坏性命令 — rm / rmdir / diskutil erasedisk / dd / chmod / chown / chgrp",
        ["CmdRuleUnmatched"] = "未匹配的命令，询问用户",
        // File access rules
        ["FileRuleDangerousExt"] = "最高优先级：危险文件扩展名直接拒绝",
        ["FileRuleInvalidPath"] = "无法解析为绝对路径，询问用户",
        ["FileRuleDenyAssemblyDir"] = "禁止：当前程序集目录",
        ["FileRuleDenyAppDataDir"] = "禁止：应用数据目录",
        ["FileRuleAllowOwnTemp"] = "但允许：自己的 Temp 目录",
        ["FileRuleOwnTemp"] = "允许：自己的 Temp 目录",
        ["FileRuleDenyOtherDataDir"] = "禁止：数据目录其他路径（包括其他硅基人的目录）",
        ["FileRuleUserFolders"] = "允许：用户常用文件夹",
        ["FileRuleUserFolderCheck"] = "用户常用文件夹 — 桌面 / 下载 / 文档 / 图片 / 音乐 / 视频",
        ["FileRulePublicFolders"] = "允许：公用用户文件夹",
        ["FileRuleWinDenySystem"] = "Windows 禁止：系统关键目录（不一定在C盘）",
        ["FileRuleWinDenySystemCheck"] = "系统关键目录",
        ["FileRuleLinuxDenySystem"] = "Linux 禁止：系统关键目录 — /etc /boot /sbin",
        ["FileRuleMacDenySystem"] = "macOS 禁止：系统关键目录 — /System /Library /private/etc",
        ["FileRuleUnmatched"] = "未匹配的路径，询问用户",
    };

    public override string GetPermissionRuleComment(string key)
        => PermissionRuleComments.TryGetValue(key, out var value) ? value : key;

    public override string PermissionRulesSection => "权限规则列表";
    public override string PermissionEditorSection => "权限规则编辑器";

    public override string PermissionSaveMissingBeingId => "缺少或无效的硅基人ID";
    public override string PermissionSaveMissingCode => "请求体中缺少代码";
    public override string PermissionSaveLoaderNotAvailable => "DynamicBeingLoader 不可用";
    public override string PermissionSaveRemoveFailed => "删除权限回调失败";
    public override string PermissionSaveRemoveSuccess => "权限回调已移除";
    public override string PermissionSaveSecurityScanFailed => "保存权限回调失败（安全扫描未通过）";
    public override string PermissionSaveCompilationFailed => "编译失败";
    public override string PermissionSaveSuccess => "权限回调保存并应用成功";
    public override string PermissionSaveError => "保存权限回调时发生错误";

    // ===== Knowledge Page Localization =====

    public override string KnowledgePageHeader => "知识图谱可视化";
    public override string KnowledgeLoadingState => "知识图谱数据加载中...";

    // ===== Chat Localization =====

    public override string SingleChatNameFormat => "与{0}聊天";
    public override string ChatConversationsHeader => "会话";
    public override string ChatNoConversationSelected => "选择会话开始聊天";
    public override string ChatMessageInputPlaceholder => "输入消息...";
    public override string ChatSendButton => "发送";
    public override string ChatUserDisplayName => "我";
    public override string ChatUserAvatarName => "我";
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
    public override string BeingsDetailSoulContentEditLink => "编辑灵魂";
    public override string BeingsBackToList => "返回列表";
    public override string SoulEditorSubtitle => "编辑硅基生命的灵魂文件（Markdown 格式）";
    public override string BeingsDetailMemoryLabel => "记忆：";
    public override string BeingsDetailMemoryViewLink => "查看";
    public override string BeingsDetailPermissionLabel => "权限：";
    public override string BeingsDetailPermissionEditLink => "编辑";
    public override string BeingsDetailTimersLabel => "计时器：";
    public override string BeingsDetailTasksLabel => "任务：";
    public override string BeingsDetailAIClientLabel => "独立AI客户端：";
    public override string BeingsDetailAIClientEditLink => "编辑";
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
    public override string TimersCalendarLabel => "历法条件：";
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
    public override string ConfigSaveFailed => "保存失败：";
    public override string ConfigDictionaryLabel => "字典";
    public override string ConfigDictKeyLabel => "键：";
    public override string ConfigDictValueLabel => "值：";
    public override string ConfigDictAddButton => "添加";
    public override string ConfigDictDeleteButton => "删除";
    public override string ConfigDictEmptyMessage => "字典为空";

    public override string LogsPageHeader => "日志查询";
    public override string LogsTotalCount => "共 {0} 条日志";
    public override string LogsStartTime => "开始时间";
    public override string LogsEndTime => "结束时间";
    public override string LogsLevelAll => "全部级别";
    public override string LogsBeingFilter => "硅基人";
    public override string LogsAllBeings => "不筛选";
    public override string LogsSystemOnly => "仅系统";
    public override string LogsFilterButton => "查询";
    public override string LogsEmptyState => "暂无日志记录";
    public override string LogsExceptionLabel => "异常详情：";
    public override string LogsPrevPage => "上一页";
    public override string LogsNextPage => "下一页";

    public override string AuditPageHeader => "Token 用量审计";
    public override string AuditTotalTokens => "总 Token 数";
    public override string AuditTotalRequests => "总请求数";
    public override string AuditSuccessCount => "成功";
    public override string AuditFailureCount => "失败";
    public override string AuditPromptTokens => "输入 Token";
    public override string AuditCompletionTokens => "输出 Token";
    public override string AuditStartTime => "开始时间";
    public override string AuditEndTime => "结束时间";
    public override string AuditFilterButton => "查询";
    public override string AuditEmptyState => "暂无审计记录";
    public override string AuditAIClientType => "AI 客户端";
    public override string AuditAllClientTypes => "全部类型";
    public override string AuditGroupByClient => "按客户端分组";
    public override string AuditGroupByBeing => "按硅基人分组";
    public override string AuditPrevPage => "上一页";
    public override string AuditNextPage => "下一页";
    public override string AuditBeing => "硅基人";
    public override string AuditAllBeings => "全部硅基人";
    public override string AuditTimeToday => "今日";
    public override string AuditTimeWeek => "本周";
    public override string AuditTimeMonth => "本月";
    public override string AuditTimeYear => "本年";
    public override string AuditExport => "导出";
    public override string AuditTrendTitle => "Token 用量趋势";
    public override string AuditTrendPrompt => "输入 Token";
    public override string AuditTrendCompletion => "输出 Token";
    public override string AuditTrendTotal => "总 Token";
    public override string AuditTooltipDate => "日期";
    public override string AuditTooltipPrompt => "输入 Token";
    public override string AuditTooltipCompletion => "输出 Token";
    public override string AuditTooltipTotal => "总 Token";

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
        ["OllamaClient"] = "Ollama 客户端",
        ["OllamaEndpoint"] = "Ollama 端点",
        ["DefaultModel"] = "默认模型",
        ["Temperature"] = "温度",
        ["MaxTokens"] = "最大 Token 数",
        ["DashScopeClient"] = "百炼客户端",
        ["DashScopeApiKey"] = "API 密钥",
        ["DashScopeRegion"] = "服务地域",
        ["DashScopeModel"] = "模型",
        ["DashScopeRegionBeijing"] = "华北2（北京）",
        ["DashScopeRegionVirginia"] = "美国（弗吉尼亚）",
        ["DashScopeRegionSingapore"] = "新加坡",
        ["DashScopeRegionHongkong"] = "中国香港",
        ["DashScopeRegionFrankfurt"] = "德国（法兰克福）",
        ["DashScopeModel_qwen3-max"] = "千问3 Max（旗舰）",
        ["DashScopeModel_qwen3.6-plus"] = "千问3.6 Plus（性价比）",
        ["DashScopeModel_qwen3.6-flash"] = "千问3.6 Flash（快速）",
        ["DashScopeModel_qwen-max"] = "千问 Max（稳定旗舰）",
        ["DashScopeModel_qwen-plus"] = "千问 Plus（稳定平衡）",
        ["DashScopeModel_qwen-turbo"] = "千问 Turbo（稳定快速）",
        ["DashScopeModel_qwen3-coder-plus"] = "千问3 Coder Plus（代码）",
        ["DashScopeModel_qwq-plus"] = "QwQ Plus（深度推理）",
        ["DashScopeModel_deepseek-v3.2"] = "DeepSeek V3.2",
        ["DashScopeModel_deepseek-r1"] = "DeepSeek R1（推理）",
        ["DashScopeModel_glm-5.1"] = "GLM 5.1（智谱）",
        ["DashScopeModel_kimi-k2.5"] = "Kimi K2.5（长上下文）",
        ["DashScopeModel_llama-4-maverick"] = "Llama 4 Maverick",
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
        ["DashScopeApiKey"] = "阿里云百炼平台 API 密钥",
        ["DashScopeRegion"] = "阿里云百炼服务地域",
        ["DashScopeModel"] = "阿里云百炼平台使用的模型",
        ["WebPort"] = "Web 服务器端口",
        ["AllowIntranetAccess"] = "允许内网访问（需要管理员权限）",
        ["WebSkin"] = "Web 皮肤名称",
        ["UserNickname"] = "人类用户的昵称"
    };

    public override string GetConfigGroupName(string groupKey) =>
        ConfigGroupNames.GetValueOrDefault(groupKey, groupKey);

    public override string GetConfigDisplayName(string displayNameKey, out bool found)
    {
        var result = ConfigDisplayNames.TryGetValue(displayNameKey, out var value);
        found = result;
        return result ? value : displayNameKey;
    }

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
        ["timer"] = "定时器",
        ["token_audit"] = "Token审计"
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

    // ===== Interval Timer Localization =====

    public override string CalendarIntervalName => "间隔定时器";
    public override string CalendarIntervalDays => "天";
    public override string CalendarIntervalHours => "小时";
    public override string CalendarIntervalMinutes => "分钟";
    public override string CalendarIntervalSeconds => "秒";
    public override string CalendarIntervalEvery => "每";

    public override string LocalizeIntervalDescription(int days, int hours, int minutes, int seconds)
    {
        var parts = new List<string>();
        if (days > 0) parts.Add($"{days}{CalendarIntervalDays}");
        if (hours > 0) parts.Add($"{hours}{CalendarIntervalHours}");
        if (minutes > 0) parts.Add($"{minutes}{CalendarIntervalMinutes}");
        if (seconds > 0) parts.Add($"{seconds}{CalendarIntervalSeconds}");

        return parts.Count > 0 ? $"{CalendarIntervalEvery}{string.Join(" ", parts)}" : "间隔定时器";
    }

    // ===== Gregorian Calendar Localization =====

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

    // ===== Buddhist Calendar Localization =====

    public override string CalendarBuddhistName => "佛历（佛元）";

    public override string? GetBuddhistMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatBuddhistYear(int year) => $"佛元{year}年";
    public override string FormatBuddhistDay(int day)   => $"{day}日";

    public override string LocalizeBuddhistDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetBuddhistMonthName(month) ?? $"{month}月";
        return $"佛元{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Cherokee Calendar Localization =====

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

    // ===== Juche Calendar Localization =====

    public override string CalendarJucheName => "主体历";

    public override string? GetJucheMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatJucheYear(int year) => $"主体{year}年";
    public override string FormatJucheDay(int day)   => $"{day}日";

    public override string LocalizeJucheDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJucheMonthName(month) ?? $"{month}月";
        return $"主体{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Republic of China Calendar Localization =====

    public override string CalendarRocName => "中华民国历（民国）";

    public override string? GetRocMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatRocYear(int year) => $"民国{year}年";
    public override string FormatRocDay(int day)   => $"{day}日";

    public override string LocalizeRocDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRocMonthName(month) ?? $"{month}月";
        return $"民国{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Chula Sakarat Calendar Localization =====

    public override string CalendarChulaSakaratName => "朱拉萨卡拉特历（CS）";

    public override string? GetChulaSakaratMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChulaSakaratYear(int year) => $"{year}年（CS）";
    public override string FormatChulaSakaratDay(int day)   => $"{day}日";

    public override string LocalizeChulaSakaratDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetChulaSakaratMonthName(month) ?? $"{month}月";
        return $"CS{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Julian Calendar Localization =====

    public override string CalendarJulianName => "儒略历";

    public override string FormatJulianYear(int year) => $"{year}年";
    public override string FormatJulianDay(int day)   => $"{day}日";

    public override string LocalizeJulianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}月";
        return $"儒略历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Khmer Calendar Localization =====

    public override string CalendarKhmerName => "高棉历（佛元）";

    public override string FormatKhmerYear(int year) => $"{year}年";
    public override string FormatKhmerDay(int day)   => $"{day}日";

    public override string LocalizeKhmerDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}月";
        return $"高棉历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Zoroastrian Calendar Localization =====

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

    // ===== French Republican Calendar Localization =====

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

    // ===== Coptic Calendar Localization =====

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

    // ===== Ethiopian Calendar Localization =====

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

    // ===== Islamic Calendar Localization =====

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

    // ===== Hebrew Calendar Localization =====

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

    // ===== Persian Calendar Localization =====

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

    // ===== Indian National Calendar Localization =====

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

    // ===== Saka Era Calendar Localization =====

    public override string CalendarSakaName => "萨迦纪元历";

    public override string FormatSakaYear(int year) => $"{year}年（SE）";
    public override string FormatSakaDay(int day)   => $"{day}日";

    public override string LocalizeSakaDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}月";
        return $"萨迦纪元{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Vikram Samvat Calendar Localization =====

    public override string CalendarVikramSamvatName => "维克拉姆历";

    public override string FormatVikramSamvatYear(int year) => $"{year}年（VS）";
    public override string FormatVikramSamvatDay(int day)   => $"{day}日";

    public override string LocalizeVikramSamvatDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}月";
        return $"维克拉姆历{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Mongolian Calendar Localization =====

    public override string CalendarMongolianName => "蒙古历";

    public override string FormatMongolianYear(int year)   => $"{year}年";
    public override string FormatMongolianMonth(int month) => $"{month}月";
    public override string FormatMongolianDay(int day)     => $"{day}日";

    public override string LocalizeMongolianDate(int year, int month, int day, int hour, int minute, int second)
        => $"蒙古历{year}年{month}月{day}日 {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Javanese Calendar Localization =====

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

    // ===== Tibetan Calendar Localization =====

    public override string CalendarTibetanName => "藏历";

    public override string FormatTibetanYear(int year)   => $"{year}年";
    public override string FormatTibetanMonth(int month) => $"{month}月";
    public override string FormatTibetanDay(int day)     => $"{day}日";

    public override string LocalizeTibetanDate(int year, int month, int day, int hour, int minute, int second)
        => $"藏历{year}年{month}月{day}日 {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Mayan Calendar Localization =====

    public override string CalendarMayanName   => "玛雅长纪历";
    public override string CalendarMayanBaktun => "伯克顿";
    public override string CalendarMayanKatun  => "卡顿";
    public override string CalendarMayanTun    => "顿";
    public override string CalendarMayanUinal  => "维纳尔";
    public override string CalendarMayanKin    => "金";

    public override string LocalizeMayanDate(int baktun, int katun, int tun, int uinal, int kin, int hour, int minute, int second)
        => $"{baktun}.{katun}.{tun}.{uinal}.{kin} {hour:D2}:{minute:D2}:{second:D2}";

    // ===== Inuit Calendar Localization =====

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

    // ===== Roman Calendar Localization =====

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

    // ===== Chinese Lunar Calendar Localization =====

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

    // ===== Vietnamese Calendar Localization =====

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

    // ===== Japanese Calendar Localization =====

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

    // ===== Yi Calendar Localization =====

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

    // ===== Sexagenary Calendar Localization =====

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

    // ===== Dehong Dai Calendar Localization =====

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

    // ===== Xishuangbanna Dai Calendar Localization =====

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

    // ===== Memory Event Localization =====

    public override string FormatMemoryEventSingleChat(string partnerName, string content)
        => $"[单聊] 与\"{partnerName}\"对话，回复：{content}";

    public override string FormatMemoryEventGroupChat(string sessionId, string content)
        => $"[群聊] 在会话 {sessionId} 中发言：{content}";

    public override string FormatMemoryEventToolCall(string toolNames)
        => $"[工具调用] 执行工具：{toolNames}";

    public override string FormatMemoryEventTask(string content)
        => $"[任务] 执行任务，结果：{content}";

    public override string FormatMemoryEventTimer(string content)
        => $"[定时] 定时触发，响应：{content}";

    public override string FormatMemoryEventTimerError(string timerName, string error)
        => $"[定时] 定时器 '{timerName}' 执行失败：{error}";

    // ===== Timer Notification Localization =====

    public override string FormatTimerStartNotification(string timerName)
        => $"⏰ 定时器 '{timerName}' 开始执行...";

    public override string FormatTimerEndNotification(string timerName, string result)
        => $"✅ 定时器 '{timerName}' 执行完成\n{result}";

    public override string FormatTimerErrorNotification(string timerName, string error)
        => $"❌ 定时器 '{timerName}' 执行失败：{error}";

    public override string FormatMemoryEventBeingCreated(string name, string id)
        => $"[管理] 创建了新硅基人\"{name}\"（{id}）";

    public override string FormatMemoryEventBeingReset(string id)
        => $"[管理] 将硅基人 {id} 重置为默认实现";

    public override string FormatMemoryEventTaskCompleted(string taskTitle)
        => $"[任务完成] {taskTitle}";

    public override string FormatMemoryEventTaskFailed(string taskTitle)
        => $"[任务失败] {taskTitle}";

    public override string FormatMemoryEventStartup()
        => "系统启动，我上线了";

    public override string FormatMemoryEventRuntimeError(string message)
        => $"[运行时错误] {message}";

    // ===== MemoryTool Response Localization =====

    public override string MemoryToolNotAvailable => "记忆系统不可用";
    public override string MemoryToolMissingAction => "缺少 'action' 参数";
    public override string MemoryToolMissingContent => "缺少 'content' 参数";
    public override string MemoryToolNoMemories => "暂无记忆";
    public override string MemoryToolRecentHeader(int count) => $"最近 {count} 条记忆：";
    public override string MemoryToolStatsHeader => "记忆统计：";
    public override string MemoryToolStatsTotal => "- 总数";
    public override string MemoryToolStatsOldest => "- 最早";
    public override string MemoryToolStatsNewest => "- 最新";
    public override string MemoryToolStatsNA => "无";
    public override string MemoryToolQueryNoResults => "该时间范围内无记忆";
    public override string MemoryToolQueryHeader(int count, string rangeDesc) => $"{rangeDesc} 共 {count} 条记忆：";
    public override string MemoryToolInvalidYear => "'year' 参数无效";
    public override string MemoryToolUnknownAction(string action) => $"未知操作：{action}";

    // ===== Code Editor Hover Tooltip Localization =====

    public override string GetCodeHoverWordTypeLabel(string wordType) => wordType switch
    {
        "variable" => "变量",
        "function" => "函数",
        "class" => "类",
        "keyword" => "关键字",
        "comment" => "注释",
        "namespace" => "命名空间",
        "parameter" => "参数",
        _ => "标识符"
    };

    public override string GetCodeHoverWordTypeDesc(string wordType, string word)
    {
        var encodedWord = System.Net.WebUtility.HtmlEncode(word);
        return wordType switch
        {
            "variable" => $"变量 '{encodedWord}' 的定义和使用信息",
            "function" => $"函数 '{encodedWord}' 的签名和说明",
            "class" => $"类 '{encodedWord}' 的结构和说明",
            "keyword" => $"关键字 '{encodedWord}' 的语法和用途",
            "comment" => $"注释中的单词 '{encodedWord}'",
            "namespace" => $"命名空间 '{encodedWord}' 的信息",
            "parameter" => $"参数 '{encodedWord}' 的定义和用途",
            _ => $"标识符 '{encodedWord}' 的信息"
        };
    }

    public override string GetCodeHoverKeywordDesc(string language, string keyword)
    {
        var key = $"{language}:{keyword.ToLower()}";
        return CSharpKeywords.GetValueOrDefault(key, "");
    }

    public override string GetTranslation(string key)
    {
        return TranslationDictionary.GetValueOrDefault(key, "");
    }

    private static readonly Dictionary<string, string> CSharpKeywords = new()
    {
        // Control flow
        { "csharp:if", "条件分支语句。当条件表达式为 true 时执行代码块。" },
        { "csharp:else", "条件分支的替代路径。与 if 配合使用，当条件为 false 时执行。" },
        { "csharp:for", "计数循环语句。包含初始化、条件判断和迭代三个部分。" },
        { "csharp:while", "条件循环语句。条件为 true 时重复执行代码块。" },
        { "csharp:do", "后测试循环语句。先执行一次代码块，再判断条件是否继续循环。" },
        { "csharp:switch", "多选分支语句。根据表达式的值匹配不同的 case 分支。" },
        { "csharp:case", "switch 语句中的分支标签。匹配特定值时执行对应代码块。" },
        { "csharp:break", "跳出语句。立即终止最近的循环或 switch 语句。" },
        { "csharp:continue", "继续语句。跳过当前循环的剩余部分，进入下一次迭代。" },
        { "csharp:return", "返回语句。退出当前方法并可选地返回一个值。" },
        { "csharp:goto", "跳转语句。无条件跳转到指定的标签位置。" },
        { "csharp:foreach", "集合遍历语句。依次访问集合中的每个元素。" },
        // Type declarations
        { "csharp:class", "引用类型声明。定义包含数据（字段、属性）和行为（方法）的结构。" },
        { "csharp:interface", "接口声明。定义类或结构必须实现的契约。" },
        { "csharp:struct", "值类型声明。轻量级数据结构，分配在栈上。" },
        { "csharp:enum", "枚举类型声明。定义一组命名的整数常量。" },
        { "csharp:namespace", "命名空间声明。组织代码的逻辑容器，避免命名冲突。" },
        { "csharp:record", "记录类型声明。具有值语义的引用类型，适合不可变数据。" },
        { "csharp:delegate", "委托声明。类型安全的方法引用，用于事件和回调。" },
        // Access modifiers
        { "csharp:public", "公共访问修饰符。成员可被任何代码访问。" },
        { "csharp:private", "私有访问修饰符。成员只能在包含类型内部访问。" },
        { "csharp:protected", "受保护访问修饰符。成员可在包含类型及其派生类型中访问。" },
        { "csharp:internal", "内部访问修饰符。成员只能在同一程序集内访问。" },
        { "csharp:sealed", "密封修饰符。防止类被继承或方法被重写。" },
        // Type keywords
        { "csharp:int", "32 位有符号整数类型（System.Int32 的别名）。" },
        { "csharp:string", "字符串类型（System.String 的别名）。表示 Unicode 字符序列，不可变。" },
        { "csharp:bool", "布尔类型（System.Boolean 的别名）。值为 true 或 false。" },
        { "csharp:float", "32 位单精度浮点数类型（System.Single 的别名）。" },
        { "csharp:double", "64 位双精度浮点数类型（System.Double 的别名）。" },
        { "csharp:decimal", "128 位高精度十进制数类型，适合金融计算。" },
        { "csharp:char", "16 位 Unicode 字符类型（System.Char 的别名）。" },
        { "csharp:byte", "8 位无符号整数类型（System.Byte 的别名）。" },
        { "csharp:object", "所有类型的基类（System.Object 的别名）。" },
        { "csharp:var", "隐式类型局部变量。编译器从初始化表达式推断类型。" },
        { "csharp:dynamic", "动态类型。跳过编译时类型检查，在运行时解析。" },
        { "csharp:void", "无返回值类型。表示方法不返回任何值。" },
        // Modifiers
        { "csharp:static", "静态修饰符。属于类型本身而非特定对象实例。" },
        { "csharp:abstract", "抽象修饰符。表示不完整的实现，必须由派生类完成。" },
        { "csharp:virtual", "虚修饰符。方法或属性可在派生类中被重写。" },
        { "csharp:override", "重写修饰符。提供对基类虚方法或抽象方法的新实现。" },
        { "csharp:const", "常量修饰符。编译时确定的不可变值。" },
        { "csharp:readonly", "只读修饰符。只能在声明或构造函数中赋值。" },
        { "csharp:volatile", "易失修饰符。表示字段可能被多个线程并发修改。" },
        { "csharp:async", "异步修饰符。标记方法包含异步操作，通常与 await 配合使用。" },
        { "csharp:await", "等待运算符。暂停方法执行直到异步操作完成。" },
        { "csharp:partial", "分部修饰符。允许类、结构或接口分布在多个文件中。" },
        { "csharp:ref", "引用参数。按引用传递参数。" },
        { "csharp:out", "输出参数。用于从方法中返回多个值。" },
        { "csharp:in", "只读引用参数。按引用传递但不允许修改。" },
        { "csharp:params", "可变参数修饰符。允许传递可变数量的同类型参数。" },
        // Exception handling
        { "csharp:try", "异常处理块。包含可能抛出异常的代码。" },
        { "csharp:catch", "异常捕获块。处理 try 块中抛出的异常。" },
        { "csharp:finally", "最终执行块。无论是否发生异常都会执行的代码。" },
        { "csharp:throw", "抛出异常语句。手动抛出异常对象。" },
        // Others
        { "csharp:new", "创建实例运算符。创建对象或调用构造函数。" },
        { "csharp:this", "当前实例引用。引用当前类的实例。" },
        { "csharp:base", "基类引用。引用直接基类的成员。" },
        { "csharp:using", "指令或语句。导入命名空间或确保 IDisposable 资源被释放。" },
        { "csharp:yield", "迭代器关键字。逐个返回值，实现延迟执行。" },
        { "csharp:lock", "锁语句。确保同一时刻只有一个线程执行代码块。" },
        { "csharp:typeof", "类型运算符。获取类型的 System.Type 对象。" },
        { "csharp:nameof", "名称运算符。获取变量、类型或成员的字符串名称。" },
        { "csharp:is", "类型检查运算符。检查对象是否兼容指定类型。" },
        { "csharp:as", "类型转换运算符。安全地尝试类型转换，失败时返回 null。" },
        { "csharp:null", "空值字面量。表示引用类型或可空类型的空引用。" },
        { "csharp:true", "布尔真值。" },
        { "csharp:false", "布尔假值。" },
        { "csharp:default", "默认值表达式。获取类型的默认值（引用类型为 null，数值类型为 0）。" },
        { "csharp:operator", "运算符声明。定义自定义类型上的运算符行为。" },
        { "csharp:explicit", "显式转换声明。需要强制类型转换的转换运算符。" },
        { "csharp:implicit", "隐式转换声明。可自动执行的转换运算符。" },
        { "csharp:unchecked", "取消检查块。禁用整型算术溢出的检查。" },
        { "csharp:checked", "检查块。启用整型算术溢出的检查。" },
        { "csharp:fixed", "固定语句。固定内存位置以防止垃圾回收移动。" },
        { "csharp:stackalloc", "栈分配运算符。在栈上分配内存块。" },
        { "csharp:extern", "外部修饰符。表示方法在外部程序集中实现（如 DLL）。" },
        { "csharp:unsafe", "不安全代码块。允许使用指针等不安全特性。" },
        // Platform core types
        { "csharp:ipermissioncallback", "权限回调接口。用于评估硅基生命体的各种操作权限（网络、命令行、文件访问等）。" },
        { "csharp:permissionresult", "权限结果枚举。表示权限评估的结果：Allowed（允许）、Denied（拒绝）、AskUser（询问用户）。" },
        { "csharp:permissiontype", "权限类型枚举。定义权限的种类：NetworkAccess（网络访问）、CommandLine（命令行执行）、FileAccess（文件访问）、Function（函数调用）、DataAccess（数据访问）。" },
        // System.Net
        { "csharp:ipaddress", "IP 地址类（System.Net.IPAddress）。表示 Internet Protocol (IP) 地址。" },
        { "csharp:addressfamily", "地址族枚举（System.Net.Sockets.AddressFamily）。指定网络地址的寻址方案，如 InterNetwork（IPv4）、InterNetworkV6（IPv6）。" },
        // System
        { "csharp:uri", "统一资源标识符类（System.Uri）。提供 URI（Uniform Resource Identifier）的对象表示，用于访问 Web 资源。" },
        { "csharp:operatingsystem", "操作系统类（System.OperatingSystem）。提供用于检查当前操作系统的静态方法，如 IsWindows()、IsLinux()、IsMacOS()。" },
        { "csharp:environment", "环境类（System.Environment）。提供有关当前环境和平台的信息，以及操作它们的方法。" },
        // System.IO
        { "csharp:path", "路径类（System.IO.Path）。对包含文件或目录路径信息的 String 实例执行操作。" },
        // System.Collections.Generic
        { "csharp:hashset", "哈希集类（System.Collections.Generic.HashSet<T>）。表示值的集，提供高性能的集运算。" },
        // System.Text
        { "csharp:stringbuilder", "字符串构建器类（System.Text.StringBuilder）。表示可变字符字符串，适合频繁修改字符串的场景。" },
    };

    // Full namespace translation dictionary
    private static readonly Dictionary<string, string> TranslationDictionary = new(CSharpKeywords)
    {
        // Add full namespace key
        { "csharp:System.Net.IPAddress", "IP 地址类（System.Net.IPAddress）。表示 Internet Protocol (IP) 地址。" },
        { "csharp:System.Net.Sockets.AddressFamily", "地址族枚举（System.Net.Sockets.AddressFamily）。指定网络地址的寻址方案，如 InterNetwork（IPv4）、InterNetworkV6（IPv6）。" },
        { "csharp:System.Uri", "统一资源标识符类（System.Uri）。提供 URI（Uniform Resource Identifier）的对象表示，用于访问 Web 资源。" },
        { "csharp:System.OperatingSystem", "操作系统类（System.OperatingSystem）。提供用于检查当前操作系统的静态方法，如 IsWindows()、IsLinux()、IsMacOS()。" },
        { "csharp:System.Environment", "环境类（System.Environment）。提供有关当前环境和平台的信息，以及操作它们的方法。" },
        { "csharp:System.IO.Path", "路径类（System.IO.Path）。对包含文件或目录路径信息的 String 实例执行操作。" },
        { "csharp:System.Collections.Generic.HashSet", "哈希集类（System.Collections.Generic.HashSet<T>）。表示值的集，提供高性能的集运算。" },
        { "csharp:System.Text.StringBuilder", "字符串构建器类（System.Text.StringBuilder）。表示可变字符字符串，适合频繁修改字符串的场景。" },
    };
}
