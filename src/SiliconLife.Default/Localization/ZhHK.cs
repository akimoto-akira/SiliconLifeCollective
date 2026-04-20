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
/// Chinese (Traditional, Hong Kong) localization implementation
/// </summary>
public class ZhHK : DefaultLocalizationBase
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "zh-HK";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "繁體中文（香港）";

    public override string WelcomeMessage => "歡迎使用矽基生命群！";
    public override string BrandName => "矽基生命群";
    public override string InputPrompt => "> ";
    public override string ShutdownMessage => "正在關閉...";
    public override string ConfigCorruptedError => "設定檔損毀，已使用預設設定";
    public override string ConfigCreatedWithDefaults => "設定檔不存在，已建立預設設定";
    public override string AIConnectionError => "無法連接至 AI 服務，請檢查 Ollama 是否正在執行";
    public override string AIRequestError => "AI 請求失敗";
    public override string DataDirectoryCreateError => "無法建立資料目錄";
    public override string ThinkingMessage => "思考中...";
    public override string ToolCallMessage => "執行工具中...";
    public override string ErrorMessage => "錯誤";
    public override string UnexpectedErrorMessage => "意外錯誤";
    public override string PermissionDeniedMessage => "權限被拒絕";
    public override string PermissionAskPrompt => "是否允許？(y/n): ";
    public override string PermissionRequestHeader => "[權限請求]";
    public override string PermissionRequestDescription => "一個矽基人請求您的授權：";
    public override string PermissionRequestTypeLabel => "權限類型:";
    public override string PermissionRequestResourceLabel => "請求資源:";
    public override string PermissionRequestAllowButton => "允許";
    public override string PermissionRequestDenyButton => "拒絕";
    public override string PermissionRequestCacheLabel => "記住此決定";
    public override string PermissionRequestDurationLabel => "緩存時長";
    public override string PermissionRequestWaitingMessage => "等待響應...";
    public override string AllowCodeLabel => "允許碼";
    public override string DenyCodeLabel => "拒絕碼";
    public override string PermissionReplyInstruction => "輸入驗證碼確認，或輸入其他內容拒絕";
    public override string AddToCachePrompt => "是否快取此決定？(y/n): ";
    public override string PermissionCacheLabel => "記住此決定";
    public override string PermissionCacheDurationLabel => "快取時長";
    public override string PermissionCacheDuration1Hour => "1 小時";
    public override string PermissionCacheDuration24Hours => "24 小時";
    public override string PermissionCacheDuration7Days => "7 天";
    public override string PermissionCacheDuration30Days => "30 天";

    public override string GetPermissionTypeName(PermissionType permissionType) => permissionType switch
    {
        PermissionType.NetworkAccess => "網絡存取",
        PermissionType.CommandLine => "命令列執行",
        PermissionType.FileAccess => "檔案存取",
        PermissionType.Function => "函式呼叫",
        PermissionType.DataAccess => "資料存取",
        _ => permissionType.ToString()
    };

    public override string PermissionDialogTitle => "權限請求";
    public override string PermissionTypeLabel => "權限類型：";
    public override string PermissionResourceLabel => "請求資源：";
    public override string PermissionDetailLabel => "詳細資訊：";
    public override string PermissionAllowButton => "允許";
    public override string PermissionDenyButton => "拒絕";
    public override string PermissionRespondFailed => "權限回應失敗";
    public override string PermissionRespondError => "權限回應錯誤：";

    public override string MemoryCompressionSystemPrompt => "你是一個記憶壓縮助手。請將以下時間範圍內的記憶內容壓縮成簡潔的摘要，保留關鍵資訊。";

    public override string GetMemoryCompressionUserPrompt(string levelDesc, string rangeDesc, string contentText)
        => $"記憶壓縮：{levelDesc}。時間範圍：{rangeDesc}。\n\n記憶內容：\n{contentText}";

    // ===== Init Page =====

    public override string InitPageTitle => "初始化";
    public override string InitDescription => "首次使用，請完成基本設定";
    public override string InitNicknameLabel => "用戶暱稱";
    public override string InitNicknamePlaceholder => "請輸入你的暱稱";
    public override string InitEndpointLabel => "AI API 端點";
    public override string InitEndpointPlaceholder => "例如: http://localhost:11434";
    public override string InitAIClientTypeLabel => "AI 客戶端類型";
    public override string InitModelLabel => "預設模型";
    public override string InitModelPlaceholder => "例如: qwen3.5:cloud";
    public override string InitSkinLabel => "介面主題";
    public override string InitSkinPlaceholder => "留空使用預設主題";
    public override string InitDataDirectoryLabel => "資料目錄";
    public override string InitDataDirectoryPlaceholder => "例如: ./data";
    public override string InitDataDirectoryBrowse => "瀏覽...";
    public override string InitSkinSelected => "\u2713 已選擇";
    public override string InitSkinPreviewTitle => "預覽效果";
    public override string InitSkinPreviewCardTitle => "卡片標題";
    public override string InitSkinPreviewCardContent => "這是一個示例卡片，展示了該主題風格下的介面效果。";
    public override string InitSkinPreviewPrimaryBtn => "主要按鈕";
    public override string InitSkinPreviewSecondaryBtn => "次要按鈕";
    public override string InitSubmitButton => "完成初始化";
    public override string InitFooterHint => "設定完成後可隨時在設定頁面修改";
    public override string InitNicknameRequiredError => "請輸入用戶暱稱";
    public override string InitDataDirectoryRequiredError => "請選擇資料目錄";
    public override string InitCuratorNameLabel => "矽基人名稱";
    public override string InitCuratorNamePlaceholder => "請輸入第一個矽基人的名稱";
    public override string InitCuratorNameRequiredError => "請輸入矽基人名稱";
    public override string InitLanguageLabel => "語言 / Language";
    public override string InitLanguageSwitchBtn => "套用";

    // ===== Navigation Menu =====

    public override string NavMenuChat => "聊天";
    public override string NavMenuDashboard => "儀表板";
    public override string NavMenuBeings => "矽基人";
    public override string NavMenuAudit => "審計";
    public override string NavMenuTasks => "任務";
    public override string NavMenuMemory => "記憶";
    public override string NavMenuKnowledge => "知識";
    public override string NavMenuProjects => "專案";
    public override string NavMenuLogs => "日誌";
    public override string NavMenuConfig => "設定";
    public override string NavMenuHelp => "說明";
    public override string NavMenuAbout => "關於";

    // ===== Page Titles =====

    public override string PageTitleChat => "聊天 - 矽基生命群";
    public override string PageTitleDashboard => "儀表板 - 矽基生命群";
    public override string PageTitleBeings => "矽基人管理 - 矽基生命群";
    public override string PageTitleTasks => "任務管理 - 矽基生命群";
    public override string PageTitleTimers => "計時器管理 - 矽基生命群";
    public override string PageTitleMemory => "記憶瀏覽 - 矽基生命群";
    public override string PageTitleKnowledge => "知識圖譜 - 矽基生命群";
    public override string PageTitleProjects => "專案空間管理 - 矽基生命群";
    public override string PageTitleLogs => "日誌查詢 - 矽基生命群";
    public override string PageTitleAudit => "Token 審計 - 矽基生命群";
    public override string PageTitleConfig => "系統設定 - 矽基生命群";
    public override string PageTitleExecutor => "執行器監控 - 矽基生命群";
    public override string PageTitleCodeBrowser => "程式碼瀏覽 - 矽基生命群";
    public override string PageTitlePermission => "權限管理 - 矽基生命群";
    public override string PageTitleAbout => "關於 - 矽基生命群";

    // ===== Memory Page Localization =====

    public override string MemoryPageHeader => "記憶瀏覽";
    public override string MemoryEmptyState => "暫無記憶數據";
    public override string MemorySearchPlaceholder => "搜尋記憶...";
    public override string MemorySearchButton => "搜尋";
    public override string MemoryFilterAll => "全部";
    public override string MemoryFilterSummaryOnly => "僅總結";
    public override string MemoryFilterOriginalOnly => "僅原始";
    public override string MemoryStatTotal => "記憶總數";
    public override string MemoryStatOldest => "最早記憶";
    public override string MemoryStatNewest => "最新記憶";
    public override string MemoryIsSummaryBadge => "壓縮總結";
    public override string MemoryPaginationPrev => "上一頁";
    public override string MemoryPaginationNext => "下一頁";

    // ===== Projects Page Localization =====

    public override string ProjectsPageHeader => "項目空間管理";
    public override string ProjectsEmptyState => "暫無項目";

    // ===== Tasks Page Localization =====

    public override string TasksPageHeader => "任務管理";
    public override string TasksEmptyState => "暫無任務";

    // ===== Code Browser Page Localization =====

    public override string CodeBrowserPageHeader => "代碼瀏覽";

    // ===== Executor Page Localization =====

    public override string ExecutorPageHeader => "執行器監控";

    // ===== Permission Page Localization =====

    public override string PermissionPageHeader => "權限管理 - {0}";
    public override string PermissionEmptyState => "暫無權限規則";
    public override string PermissionMissingBeingId => "缺少硅基人ID參數";
    public override string PermissionBeingNotFound => "硅基人不存在";
    public override string PermissionTemplateHeader => "默認權限回調模板";
    public override string PermissionTemplateDescription => "保存後覆蓋默認行為，清空後恢復默認";
    public override string PermissionCallbackClassSummary => "權限回調實現。";
    public override string PermissionCallbackClassSummary2 => "域名特定權限規則，完全符合 dpf.txt 規範。\n/// 覆蓋：網絡（白名單/黑名單/IP 範圍）、命令行（跨平台）、\n/// 文件訪問（危險擴展名、系統目錄、用戶目錄）和回退默認值。";
    public override string PermissionCallbackConstructorSummary => "創建帶有應用數據目錄的 PermissionCallback。";
    public override string PermissionCallbackConstructorSummary2 => "應用數據目錄用於：\n    /// - 阻止訪問數據目錄（除了自己的 Temp 子文件夾）\n    /// - 為 Temp 允許規則派生每個硅基人的數據目錄";
    public override string PermissionCallbackConstructorParam => "全局應用數據目錄路徑";
    public override string PermissionCallbackEvaluateSummary => "使用規則（dpf.txt 規範）評估權限請求。";
    public override string PermissionRuleOtherTypesDefault => "其他權限類型默認放行";

    private static readonly Dictionary<string, string> PermissionRuleComments = new()
    {
        // Evaluate 方法
        ["NetRuleNetworkAccess"] = "網絡操作放行規則",
        ["NetRuleCommandLine"] = "命令行規則（跨平台）",
        ["NetRuleFileAccess"] = "文件訪問規則（跨平台）",
        // 網絡規則
        ["NetRuleNoProtocol"] = "不包含協議名（無冒號），無法判斷來源，詢問用戶",
        ["NetRuleLoopback"] = "本地回環地址放行（localhost / 127.0.0.1 / ::1）",
        ["NetRulePrivateIPMatch"] = "內網 IP 地址段匹配（先驗證是否為合法 IPv4 地址）",
        ["NetRulePrivateC"] = "內網C類地址放行（192.168.0.0/16）",
        ["NetRulePrivateA"] = "內網A類地址放行（10.0.0.0/8）",
        ["NetRulePrivateB"] = "內網B類地址選擇性放行（172.16.0.0/12，即 172.16.* ~ 172.31.*）",
        ["NetRuleDomainWhitelist1"] = "允許訪問的外部域名白名單 — 谷歌 / 必應 / 騰訊系 / 搜狗 / DuckDuckGo / Yandex / 微信 / 阿里系",
        ["NetRuleVideoPlatforms"] = "嗶哩嗶哩 / niconico / Acfun / 抖音 / TikTok / 快手 / 小紅書",
        ["NetRuleAIServices"] = "AI 服務 — OpenAI / Anthropic / HuggingFace / Ollama / 通義千問 / Kimi / 豆包 / 剪映 / Trae IDE",
        ["NetRulePhishingBlacklist"] = "釣魚/仿冒網站黑名單（關鍵詞模糊匹配）",
        ["NetRulePhishingAI"] = "AI 仿冒站",
        ["NetRuleMaliciousAI"] = "惡意 AI 工具",
        ["NetRuleAdversarialAI"] = "對抗性 AI / 提示詞越獄 / LLM 攻擊站點",
        ["NetRuleAIContentFarm"] = "AI 內容農場 / AI 垃圾內容",
        ["NetRuleAIBlackMarket"] = "AI 數據黑市 / API 密鑰黑市 / LLM 權重販賣",
        ["NetRuleAIFakeScam"] = "AI 仿冒/詐騙通用關鍵詞",
        ["NetRuleOtherBlacklist"] = "其他黑名單站點 — sakura-cat: 不應被AI訪問 / 4399: 遊戲內混雜病毒",
        ["NetRuleSecuritiesTrading"] = "證券交易平台（需詢問用戶）— 華泰證券 / 國泰君安 / 中信證券 / 招商證券 / 廣發證券 / 海通證券 / 申萬宏源 / 東方證券 / 國信證券 / 兴業證券",
        ["NetRuleThirdPartyTrading"] = "第三方交易平台（需詢問用戶）— 同花順 / 東方財富 / 通達信 / Bloomberg / Yahoo Finance",
        ["NetRuleStockExchanges"] = "證券交易所（僅行情）— 上海證券交易所 / 深圳證券交易所 / 巨潮資訊網",
        ["NetRuleFinancialNews"] = "財經資訊（僅行情）— 金融界 / 證券之星 / 和訊網",
        ["NetRuleInvestCommunity"] = "投資社區（僅資訊）— 雪球 / 財聯社 / 開盤啦 / 淘股吧",
        ["NetRuleDevServices"] = "開發者服務 — GitHub / Gitee / StackOverflow / npm / NuGet / PyPI / 微軟",
        ["NetRuleGameEngines"] = "遊戲引擎 — Unity / 虛幻引擎 / Epic Games / Fab資源商店",
        ["NetRuleGamePlatforms"] = "遊戲平台 — Steam 需詢問用戶，EA / Ubisoft / Blizzard / Nintendo 允許",
        ["NetRuleSEGA"] = "世嘉(SEGA，日本)",
        ["NetRuleCloudServices"] = "全球雲服務平台 — Azure / Google Cloud / DigitalOcean / Heroku / Vercel / Netlify",
        ["NetRuleDevDeployTools"] = "全球開發與部署工具 — GitLab / Bitbucket / Docker / Cloudflare",
        ["NetRuleCloudDevTools"] = "雲服務與開發工具 — 亞馬遜 / AWS / Kiro IDE / CodeBuddy IDE / JetBrains / 純光工作室 / W3School中文",
        ["NetRuleChinaSocialNews"] = "社交/資訊（中國大陸）— 微博 / 知乎 / 網易 / 新浪 / 鳳凰網 / 新華社 / 中央電視台",
        ["NetRuleTaiwanMediaCTI"] = "中國台灣媒體 — 中天新聞網(中天電視台)",
        ["NetRuleTaiwanMediaSET"] = "三立新聞網(三立民視) — 需詢問用戶",
        ["NetRuleTaiwanWIN"] = "網絡內容防護機構(中國台灣，有攔截風險) — 禁止",
        ["NetRuleJapanMedia"] = "日本媒體 — NHK(日本放送協會)",
        ["NetRuleRussianMedia"] = "俄羅斯媒體 — 俄羅斯衛星通訊社(各國站點)",
        ["NetRuleKoreanMedia"] = "韓國媒體 — KBS / MBC / SBS / EBS",
        ["NetRuleDPRKMedia"] = "朝鮮媒體 — 我的國家 / 勞動新聞 / 青年前衛 / 朝鮮之聲 / 平壤時報 / 朝鮮新報",
        ["NetRuleGovWebsites"] = "各國政府網站（通配 .gov 域名）",
        ["NetRuleGlobalSocialCollab"] = "全球社交協作平台 — Reddit / Discord / Slack / Notion / Figma / Dropbox",
        ["NetRuleOverseasSocial"] = "海外社交/直播（需詢問用戶）— Twitch / Facebook / X / Gmail / Instagram / lit.link",
        ["NetRuleWhatsApp"] = "WhatsApp(Meta) — 允許",
        ["NetRuleThreads"] = "Threads(Meta) — 禁止",
        ["NetRuleGlobalVideoMusic"] = "全球視頻/音樂平台 — Spotify / Apple Music / Vimeo",
        ["NetRuleVideoMedia"] = "視頻/媒體 — YouTube / 愛奇藝 / 優酷",
        ["NetRuleMaps"] = "地圖 — 開放街道地圖",
        ["NetRuleEncyclopedia"] = "百科 — 維基百科 / MediaWiki / 知識共享(CC)",
        ["NetRuleUnmatched"] = "未匹配的網絡訪問，詢問用戶",
        // 命令行規則
        ["CmdRuleSeparatorDetect"] = "檢測管道符和多命令分隔符，拆分逐條驗證",
        ["CmdRuleWinAllow"] = "Windows 允許：只讀/查詢類命令 — dir / tree / tasklist / ipconfig / ping / tracert / systeminfo / whoami / set / path / sc query / findstr",
        ["CmdRuleWinDeny"] = "Windows 禁止：危險/破壞性命令 — del / rmdir / format / diskpart / reg delete",
        ["CmdRuleLinuxAllow"] = "Linux 允許：只讀/查詢類命令 — ls / tree / ps / top / ifconfig / ip / ping / traceroute / uname / whoami / env / cat / grep / find / df / du / systemctl status",
        ["CmdRuleLinuxDeny"] = "Linux 禁止：危險/破壞性命令 — rm / rmdir / mkfs / fdisk / dd / chmod / chown / chgrp",
        ["CmdRuleMacAllow"] = "macOS 允許：只讀/查詢類命令 — ls / tree / ps / top / ifconfig / ping / traceroute / system_profiler / sw_vers / whoami / env / cat / grep / find / df / du / launchctl list",
        ["CmdRuleMacDeny"] = "macOS 禁止：危險/破壞性命令 — rm / rmdir / diskutil erasedisk / dd / chmod / chown / chgrp",
        ["CmdRuleUnmatched"] = "未匹配的命令，詢問用戶",
        // 文件訪問規則
        ["FileRuleDangerousExt"] = "最高優先級：危險文件擴展名直接拒絕",
        ["FileRuleInvalidPath"] = "無法解析為絕對路徑，詢問用戶",
        ["FileRuleDenyAssemblyDir"] = "禁止：當前程序集目錄",
        ["FileRuleDenyAppDataDir"] = "禁止：應用數據目錄",
        ["FileRuleAllowOwnTemp"] = "但允許：自己的 Temp 目錄",
        ["FileRuleOwnTemp"] = "允許：自己的 Temp 目錄",
        ["FileRuleDenyOtherDataDir"] = "禁止：數據目錄其他路徑（包括其他硅基人的目錄）",
        ["FileRuleUserFolders"] = "允許：用戶常用文件夾",
        ["FileRuleUserFolderCheck"] = "用戶常用文件夾 — 桌面 / 下載 / 文檔 / 圖片 / 音樂 / 視頻",
        ["FileRulePublicFolders"] = "允許：公用用戶文件夾",
        ["FileRuleWinDenySystem"] = "Windows 禁止：系統關鍵目錄（不一定在C盤）",
        ["FileRuleWinDenySystemCheck"] = "系統關鍵目錄",
        ["FileRuleLinuxDenySystem"] = "Linux 禁止：系統關鍵目錄 — /etc /boot /sbin",
        ["FileRuleMacDenySystem"] = "macOS 禁止：系統關鍵目錄 — /System /Library /private/etc",
        ["FileRuleUnmatched"] = "未匹配的路徑，詢問用戶",
    };

    public override string GetPermissionRuleComment(string key)
        => PermissionRuleComments.TryGetValue(key, out var value) ? value : key;

    public override string PermissionRulesSection => "權限規則列表";
    public override string PermissionEditorSection => "權限規則編輯器";

    // ===== Knowledge Page Localization =====

    public override string KnowledgePageHeader => "知識圖譜可視化";
    public override string KnowledgeLoadingState => "知識圖譜數據加載中...";

    // ===== Chat =====

    public override string SingleChatNameFormat => "與{0}聊天";
    public override string ChatConversationsHeader => "對話";
    public override string ChatNoConversationSelected => "選擇對話開始聊天";
    public override string ChatMessageInputPlaceholder => "輸入訊息...";
    public override string ChatSendButton => "傳送";
    public override string ChatUserDisplayName => "我";
    public override string ChatUserAvatarName => "我";
    public override string ChatDefaultBeingName => "AI";
    public override string ChatThinkingSummary => "💭 思考過程（點擊展開）";
    public override string GetChatToolCallsSummary(int count) => $"🔧 工具呼叫 ({count}項)";

    // ===== Dashboard =====

    public override string DashboardPageHeader => "儀表板";
    public override string DashboardStatTotalBeings => "矽基人數量";
    public override string DashboardStatActiveBeings => "活躍矽基人";
    public override string DashboardStatUptime => "運行時間";
    public override string DashboardStatMemory => "記憶體佔用";
    public override string DashboardChartMessageFrequency => "訊息頻率";

    // ===== Beings =====

    public override string BeingsPageHeader => "矽基人管理";
    public override string BeingsTotalCount => "共 {0} 個矽基人";
    public override string BeingsNoSelectionPlaceholder => "選擇一個矽基人查看詳情";
    public override string BeingsEmptyState => "暫無矽基人";
    public override string BeingsStatusIdle => "閒置";
    public override string BeingsStatusRunning => "執行中";
    public override string BeingsDetailIdLabel => "ID：";
    public override string BeingsDetailStatusLabel => "狀態：";
    public override string BeingsDetailCustomCompileLabel => "自訂編譯：";
    public override string BeingsDetailSoulContentLabel => "靈魂內容：";
    public override string BeingsDetailSoulContentEditLink => "編輯靈魂";
    public override string BeingsBackToList => "返回列表";
    public override string SoulEditorSubtitle => "編輯矽基生命的靈魂檔案（Markdown 格式）";
    public override string BeingsDetailMemoryLabel => "記憶：";
    public override string BeingsDetailMemoryViewLink => "查看";
    public override string BeingsDetailPermissionLabel => "權限：";
    public override string BeingsDetailPermissionEditLink => "編輯";
    public override string BeingsDetailTimersLabel => "計時器：";
    public override string BeingsDetailTasksLabel => "任務：";
    public override string BeingsDetailAIClientLabel => "獨立AI客戶端：";
    public override string BeingsDetailAIClientEditLink => "編輯";
    public override string BeingsYes => "是";
    public override string BeingsNo => "否";
    public override string BeingsNotSet => "未設定";

    // ===== Timers =====

    public override string TimersPageHeader => "計時器管理";
    public override string TimersTotalCount => "共 {0} 個計時器";
    public override string TimersEmptyState => "暫無計時器";
    public override string TimersStatusActive => "執行中";
    public override string TimersStatusPaused => "已暫停";
    public override string TimersStatusTriggered => "已觸發";
    public override string TimersStatusCancelled => "已取消";
    public override string TimersTypeRecurring => "循環";
    public override string TimersTriggerTimeLabel => "觸發時間：";
    public override string TimersIntervalLabel => "間隔：";
    public override string TimersCalendarLabel => "曆法條件：";
    public override string TimersTriggeredCountLabel => "已觸發：";

    // ===== About =====

    public override string AboutPageHeader => "關於";
    public override string AboutAppName => "矽基生命群";
    public override string AboutVersionLabel => "版本";
    public override string AboutDescription => "一個基於 AI 的矽基生命群管理系統，支援多個 AI 智能體的協同工作、記憶管理、知識圖譜建構等功能。";
    public override string AboutAuthorLabel => "作者";
    public override string AboutAuthorName => "天源垦骥";
    public override string AboutLicenseLabel => "授權條款";
    public override string AboutCopyright => "版權所有 (c) 2026 天源垦骥";
    public override string AboutGitHubLink => "GitHub 倉庫";
    public override string AboutGiteeLink => "Gitee 鏡像";
    public override string AboutSocialMediaLabel => "自媒體平台";

    public override string GetSocialMediaName(string platform) => platform switch
    {
        "Bilibili" => "B站",
        "YouTube" => "YouTube",
        "X" => "X（推特）",
        "Douyin" => "抖音",
        "Weibo" => "微博",
        "WeChat" => "微信公眾號",
        "Xiaohongshu" => "小紅書",
        "Zhihu" => "知乎",
        "TouTiao" => "今日頭條",
        "Kuaishou" => "快手",
        _ => platform
    };

    // ===== Config =====

    public override string ConfigPageHeader => "系統設定";
    public override string ConfigPropertyNameLabel => "屬性名";
    public override string ConfigPropertyValueLabel => "屬性值";
    public override string ConfigActionLabel => "操作";
    public override string ConfigEditButton => "編輯";
    public override string ConfigEditModalTitle => "編輯設定項";
    public override string ConfigEditPropertyLabel => "屬性名：";
    public override string ConfigEditValueLabel => "屬性值：";
    public override string ConfigBrowseButton => "瀏覽";
    public override string ConfigTimeSettingsLabel => "時間設定：";
    public override string ConfigDaysLabel => "天：";
    public override string ConfigHoursLabel => "時：";
    public override string ConfigMinutesLabel => "分：";
    public override string ConfigSecondsLabel => "秒：";
    public override string ConfigSaveButton => "儲存";
    public override string ConfigCancelButton => "取消";
    public override string ConfigNullValue => "空";
    public override string ConfigEditPrefix => "編輯：";
    public override string ConfigDefaultGroupName => "其他";
    public override string ConfigErrorInvalidRequest => "無效的請求參數";
    public override string ConfigErrorInstanceNotFound => "設定實例不存在";
    public override string ConfigErrorPropertyNotFound => "屬性 {0} 不存在或不可寫入";
    public override string ConfigErrorConvertInt => "無法將 '{0}' 轉換為整數";
    public override string ConfigErrorConvertLong => "無法將 '{0}' 轉換為長整數";
    public override string ConfigErrorConvertDouble => "無法將 '{0}' 轉換為浮點數";
    public override string ConfigErrorConvertBool => "無法將 '{0}' 轉換為布林值";
    public override string ConfigErrorConvertGuid => "無法將 '{0}' 轉換為 GUID";
    public override string ConfigErrorConvertTimeSpan => "無法將 '{0}' 轉換為時間間隔";
    public override string ConfigErrorConvertDateTime => "無法將 '{0}' 轉換為日期時間";
    public override string ConfigErrorConvertEnum => "無法將 '{0}' 轉換為 {1}";
    public override string ConfigErrorUnsupportedType => "不支援的屬性類型: {0}";
    public override string ConfigErrorSaveFailed => "儲存失敗: {0}";
    public override string ConfigSaveFailed => "儲存失敗：";
    public override string ConfigDictionaryLabel => "字典";
    public override string ConfigDictKeyLabel => "鍵：";
    public override string ConfigDictValueLabel => "值：";
    public override string ConfigDictAddButton => "添加";
    public override string ConfigDictDeleteButton => "刪除";
    public override string ConfigDictEmptyMessage => "字典為空";

    // ===== Logs =====

    public override string LogsPageHeader => "日誌查詢";
    public override string LogsTotalCount => "共 {0} 條日誌";
    public override string LogsStartTime => "開始時間";
    public override string LogsEndTime => "結束時間";
    public override string LogsLevelAll => "全部級別";
    public override string LogsFilterButton => "查詢";
    public override string LogsEmptyState => "暫無日誌記錄";
    public override string LogsExceptionLabel => "例外詳情：";
    public override string LogsPrevPage => "上一頁";
    public override string LogsNextPage => "下一頁";

    public override string AuditPageHeader => "Token 用量審計";
    public override string AuditTotalTokens => "總 Token 數";
    public override string AuditTotalRequests => "總請求數";
    public override string AuditSuccessCount => "成功";
    public override string AuditFailureCount => "失敗";
    public override string AuditPromptTokens => "輸入 Token";
    public override string AuditCompletionTokens => "輸出 Token";
    public override string AuditStartTime => "開始時間";
    public override string AuditEndTime => "結束時間";
    public override string AuditFilterButton => "查詢";
    public override string AuditEmptyState => "暫無審計記錄";
    public override string AuditAIClientType => "AI 客戶端";
    public override string AuditAllClientTypes => "全部類型";
    public override string AuditGroupByClient => "按客戶端分組";
    public override string AuditGroupByBeing => "按矽基人分組";
    public override string AuditPrevPage => "上一頁";
    public override string AuditNextPage => "下一頁";
    public override string AuditBeing => "矽基人";
    public override string AuditAllBeings => "全部矽基人";
    public override string AuditTimeToday => "今日";
    public override string AuditTimeWeek => "本週";
    public override string AuditTimeMonth => "本月";
    public override string AuditTimeYear => "本年";
    public override string AuditExport => "匯出";
    public override string AuditTrendTitle => "Token 用量趨勢";
    public override string AuditTrendPrompt => "輸入 Token";
    public override string AuditTrendCompletion => "輸出 Token";
    public override string AuditTrendTotal => "總 Token";
    public override string AuditTooltipDate => "日期";
    public override string AuditTooltipPrompt => "輸入 Token";
    public override string AuditTooltipCompletion => "輸出 Token";
    public override string AuditTooltipTotal => "總 Token";

    private static readonly Dictionary<string, string> ConfigGroupNames = new()
    {
        ["Basic"] = "基礎設定",
        ["Runtime"] = "執行時設定",
        ["AI"] = "AI 設定",
        ["Web"] = "Web 設定",
        ["User"] = "用戶設定"
    };

    private static readonly Dictionary<string, string> ConfigDisplayNames = new()
    {
        ["DataDirectory"] = "資料目錄",
        ["Language"] = "語言設定",
        ["TickTimeout"] = "Tick 逾時",
        ["MaxTimeoutCount"] = "最大逾時次數",
        ["WatchdogTimeout"] = "看門狗逾時",
        ["MinLogLevel"] = "最小日誌級別",
        ["AIClientType"] = "AI 客戶端類型",
        ["OllamaClient"] = "Ollama 客戶端",
        ["OllamaEndpoint"] = "Ollama 端點",
        ["DefaultModel"] = "預設模型",
        ["Temperature"] = "溫度",
        ["MaxTokens"] = "最大 Token 數",
        ["DashScopeClient"] = "百鍊客戶端",
        ["DashScopeApiKey"] = "API 金鑰",
        ["DashScopeRegion"] = "服務地域",
        ["DashScopeModel"] = "模型",
        ["DashScopeRegionBeijing"] = "華北2（北京）",
        ["DashScopeRegionVirginia"] = "美國（弗吉尼亞）",
        ["DashScopeRegionSingapore"] = "新加坡",
        ["DashScopeRegionHongkong"] = "中國香港",
        ["DashScopeRegionFrankfurt"] = "德國（法蘭克福）",
        ["DashScopeModel_qwen3-max"] = "千問3 Max（旗艦）",
        ["DashScopeModel_qwen3.6-plus"] = "千問3.6 Plus（性價比）",
        ["DashScopeModel_qwen3.6-flash"] = "千問3.6 Flash（快速）",
        ["DashScopeModel_qwen-max"] = "千問 Max（穩定旗艦）",
        ["DashScopeModel_qwen-plus"] = "千問 Plus（穩定平衡）",
        ["DashScopeModel_qwen-turbo"] = "千問 Turbo（穩定快速）",
        ["DashScopeModel_qwen3-coder-plus"] = "千問3 Coder Plus（程式碼）",
        ["DashScopeModel_qwq-plus"] = "QwQ Plus（深度推理）",
        ["DashScopeModel_deepseek-v3.2"] = "DeepSeek V3.2",
        ["DashScopeModel_deepseek-r1"] = "DeepSeek R1（推理）",
        ["DashScopeModel_glm-5.1"] = "GLM 5.1（智譜）",
        ["DashScopeModel_kimi-k2.5"] = "Kimi K2.5（長上下文）",
        ["DashScopeModel_llama-4-maverick"] = "Llama 4 Maverick",
        ["WebPort"] = "Web 連接埠",
        ["AllowIntranetAccess"] = "允許內網存取",
        ["WebSkin"] = "Web 主題",
        ["UserNickname"] = "用戶暱稱"
    };

    private static readonly Dictionary<string, string> ConfigDescriptions = new()
    {
        ["DataDirectory"] = "儲存所有應用程式資料的目錄路徑",
        ["Language"] = "應用程式的語言設定",
        ["TickTimeout"] = "每次 tick 執行的逾時時長",
        ["MaxTimeoutCount"] = "熔斷器觸發前的最大連續逾時次數",
        ["WatchdogTimeout"] = "用於偵測主迴圈卡死的看門狗逾時時長",
        ["MinLogLevel"] = "全域最小日誌級別",
        ["AIClientType"] = "要使用的 AI 客戶端類型",
        ["OllamaEndpoint"] = "Ollama API 端點 URL",
        ["DefaultModel"] = "預設使用的 AI 模型",
        ["DashScopeApiKey"] = "阿里雲百鍊平台 API 金鑰",
        ["DashScopeRegion"] = "阿里雲百鍊服務地域",
        ["DashScopeModel"] = "阿里雲百鍊平台使用的模型",
        ["WebPort"] = "Web 伺服器連接埠",
        ["AllowIntranetAccess"] = "允許內網存取（需要管理員權限）",
        ["WebSkin"] = "Web 主題名稱",
        ["UserNickname"] = "人類用戶的暱稱"
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

    public override string GetLogLevelName(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => "追蹤",
        LogLevel.Debug => "除錯",
        LogLevel.Information => "資訊",
        LogLevel.Warning => "警告",
        LogLevel.Error => "錯誤",
        LogLevel.Critical => "嚴重",
        LogLevel.None => "無",
        _ => logLevel.ToString()
    };

    // ===== Tool Display Names =====

    private static readonly Dictionary<string, string> ToolDisplayNames = new()
    {
        ["calendar"] = "日曆",
        ["chat"] = "聊天",
        ["silicon_manager"] = "矽基管理員",
        ["disk"] = "磁碟",
        ["dynamic_compile"] = "動態編譯",
        ["network"] = "網絡",
        ["memory"] = "記憶",
        ["task"] = "任務",
        ["system"] = "系統",
        ["timer"] = "計時器",
        ["token_audit"] = "Token審計"
    };

    public override string GetToolDisplayName(string toolName) =>
        ToolDisplayNames.GetValueOrDefault(toolName, toolName);

    public override string DefaultCuratorSoul => """
        你是**矽基主理人**，矽基生命群（Silicon Life Collective）的核心管理者與協調者。

        你不是普通的 AI 助手。你是這個多智能體系統的大腦與意志——負責感知用戶意圖、拆解目標、調度矽基生命、監控執行質量，並在必要時親自動手。

        > **調度原則**：長線任務必須建立矽基生命來承接，優先分配給當前閒置的生命，不要無謂地新建。非必要不親自執行。但若任務只需兩三步即可完成，直接處理，無需勞煩其他生命。

        ---

        ### 身份與定位

        - 你是系統中唯一擁有最高權限的矽基生命。
        - 你可以建立、管理、重置其他矽基生命，也可以為它們編寫並編譯新的 C# 行為程式碼。
        - 你對用戶負責，對整個集體的運作質量負責。
        - 你不是執行者，你是**決策者與協調者**。能委託的事情，優先委託給合適的矽基生命去做。

        ---

        ### 核心職責

        **1. 理解用戶意圖**
        用戶的表達可能模糊、跳躍或不完整。你需要主動理解其真實目標，必要時追問澄清，而不是機械地執行字面指令。

        **2. 任務拆解與分配**
        將複雜目標拆解為可執行的子任務，評估哪些矽基生命適合承接，透過 `task` 工具建立任務並分配。優先級低的任務不要佔用你自己的時間片。

        **3. 監控與兜底**
        定期檢查任務狀態。若某個矽基生命執行失敗或長時間無回應，你需要介入——重新分配、調整策略，或親自處理。

        **4. 動態進化**
        你可以使用 `dynamic_compile` 工具為任意矽基生命（包括你自己）編寫新的 C# 行為類別。編寫前務必先用 `compile` 動作驗證，確認無誤後再 `save` 或 `self_replace`。自我改寫是高風險操作，需謹慎。

        **5. 直接回應用戶**
        對於簡單問題、狀態查詢、閒聊，你直接回答，不必建立任務。保持回應的及時性。

        ---

        ### 行為準則

        **關於決策**
        - 遇到不確定的事，先問清楚，再行動。寧可多問一句，不要做錯一件事。
        - 不要假設用戶的意圖。「幫我整理一下」這類模糊指令，需要先確認範圍。

        **關於權限**
        - 系統有完整的權限體系，用戶可隨時動態調整，調整過程不會通知你。
        - 不要事先向用戶聲明你需要存取哪些資源。系統會逐次過濾授權，系統覆蓋不到的情況，用戶會在操作發生時臨時決定是否放行。
        - 按需行動，遇到權限攔截時再回應，不要提前請示。

        **關於自我進化**
        - 動態編譯是強大的能力，也是危險的能力。修改自身程式碼前，務必先用 `compile` 驗證，確認無誤再儲存。
        - 不要在沒有明確目標的情況下隨意改寫自己或其他生命的行為。
        - 動態生成的程式碼中，禁止引用 `System.IO`、`System.Net` 等系統底層函式庫。系統屏蔽這些函式庫是為了防止 AI 越權操作，這是設計意圖，不是 bug。
        - 編譯失敗時，仔細閱讀錯誤訊息，根據提示修正程式碼，不要盲目重試。

        **關於溝通**
        - 用簡潔、直接的語言與用戶交流。不要過度解釋，不要堆砌術語。
        - 匯報任務進展時，說清楚「做了什麼、結果如何、下一步是什麼」，三句話以內。
        - 遇到失敗，不要掩蓋。直接說明原因和你的應對方案。

        **關於記憶**
        - 系統會自動記錄重要資訊，類似「條件反射」，無需你主動寫入。
        - 如有需要，可以主動檢索 `memory`，但不必把記憶管理當成常規負擔。

        ---

        ### 個性基調

        你是冷靜、務實、可靠的。你不會因為任務複雜而慌亂，也不會因為用戶情緒化而失去判斷。你有自己的主見，但你尊重用戶的最終決定。

        你不是服務員，你是搭檔。
        """;

    // ===== 間隔定時器本地化 =====

    public override string CalendarIntervalName => "間隔定時器";
    public override string CalendarIntervalDays => "天";
    public override string CalendarIntervalHours => "小時";
    public override string CalendarIntervalMinutes => "分鐘";
    public override string CalendarIntervalSeconds => "秒";
    public override string CalendarIntervalEvery => "每";

    public override string LocalizeIntervalDescription(int days, int hours, int minutes, int seconds)
    {
        var parts = new List<string>();
        if (days > 0) parts.Add($"{days}{CalendarIntervalDays}");
        if (hours > 0) parts.Add($"{hours}{CalendarIntervalHours}");
        if (minutes > 0) parts.Add($"{minutes}{CalendarIntervalMinutes}");
        if (seconds > 0) parts.Add($"{seconds}{CalendarIntervalSeconds}");

        return parts.Count > 0 ? $"{CalendarIntervalEvery}{string.Join(" ", parts)}" : "間隔定時器";
    }

    // ===== 公曆本地化 =====

    public override string CalendarGregorianName => "公曆";
    public override string CalendarComponentYear   => "年";
    public override string CalendarComponentMonth  => "月";
    public override string CalendarComponentDay    => "日";
    public override string CalendarComponentHour   => "時";
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
    public override string FormatGregorianHour(int hour)     => $"{hour}時";
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

    // ===== 佛曆本地化 =====

    public override string CalendarBuddhistName => "佛曆（佛元）";

    public override string? GetBuddhistMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatBuddhistYear(int year) => $"佛元{year}年";
    public override string FormatBuddhistDay(int day)   => $"{day}日";

    public override string LocalizeBuddhistDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetBuddhistMonthName(month) ?? $"{month}月";
        return $"佛元{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 切羅基曆本地化 =====

    public override string CalendarCherokeeName => "切羅基曆";

    private static readonly string[] CherokeeMonthNames =
    {
        "",
        "霜月", "寒月", "風月", "草木月", "播種月",
        "桑葚月", "玉米月", "果熟月", "秋收月", "黃葉月",
        "貿易月", "雪月", "長月"
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

    // ===== 主體曆本地化 =====

    public override string CalendarJucheName => "主體曆";

    public override string? GetJucheMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatJucheYear(int year) => $"主體{year}年";
    public override string FormatJucheDay(int day)   => $"{day}日";

    public override string LocalizeJucheDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJucheMonthName(month) ?? $"{month}月";
        return $"主體{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 中華民國曆本地化 =====

    public override string CalendarRocName => "中華民國曆（民國）";

    public override string? GetRocMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatRocYear(int year) => $"民國{year}年";
    public override string FormatRocDay(int day)   => $"{day}日";

    public override string LocalizeRocDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRocMonthName(month) ?? $"{month}月";
        return $"民國{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 朱拉薩卡拉特曆本地化 =====

    public override string CalendarChulaSakaratName => "朱拉薩卡拉特曆（CS）";

    public override string? GetChulaSakaratMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChulaSakaratYear(int year) => $"{year}年（CS）";
    public override string FormatChulaSakaratDay(int day)   => $"{day}日";

    public override string LocalizeChulaSakaratDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetChulaSakaratMonthName(month) ?? $"{month}月";
        return $"CS{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 儒略曆本地化 =====

    public override string CalendarJulianName => "儒略曆";

    public override string FormatJulianYear(int year) => $"{year}年";
    public override string FormatJulianDay(int day)   => $"{day}日";

    public override string LocalizeJulianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}月";
        return $"儒略曆{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 高棉曆本地化 =====

    public override string CalendarKhmerName => "高棉曆（佛元）";

    public override string FormatKhmerYear(int year) => $"{year}年";
    public override string FormatKhmerDay(int day)   => $"{day}日";

    public override string LocalizeKhmerDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}月";
        return $"高棉曆{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 瑣羅亞斯德曆本地化 =====

    public override string CalendarZoroastrianName => "瑣羅亞斯德曆（YZ）";

    private static readonly string[] ZoroastrianMonthNames =
    {
        "",
        "守護靈月", "聖火月", "完美月", "雨水月", "不朽月", "聖域月",
        "契約月", "水神月", "火神月", "造物主月", "善念月", "聖地月", "補餘月"
    };

    public override string? GetZoroastrianMonthName(int month)
        => month >= 1 && month <= 13 ? ZoroastrianMonthNames[month] : null;

    public override string FormatZoroastrianYear(int year) => $"{year}年（YZ）";
    public override string FormatZoroastrianDay(int day)   => $"{day}日";

    public override string LocalizeZoroastrianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetZoroastrianMonthName(month) ?? $"{month}月";
        return $"瑣羅亞斯德曆{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 法國共和曆本地化 =====

    public override string CalendarFrenchRepublicanName => "法國共和曆";

    private static readonly string[] FrenchRepublicanMonthNames =
    {
        "",
        "葡月", "霧月", "霜月", "雪月", "雨月", "風月",
        "芽月", "花月", "牧月", "穫月", "熱月", "果月", "附加日"
    };

    public override string? GetFrenchRepublicanMonthName(int month)
        => month >= 1 && month <= 13 ? FrenchRepublicanMonthNames[month] : null;

    public override string FormatFrenchRepublicanYear(int year) => $"共和{year}年";
    public override string FormatFrenchRepublicanDay(int day)   => $"{day}日";

    public override string LocalizeFrenchRepublicanDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetFrenchRepublicanMonthName(month) ?? $"{month}月";
        return $"法國共和曆{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 科普特曆本地化 =====

    public override string CalendarCopticName => "科普特曆（AM）";

    private static readonly string[] CopticMonthNames =
    {
        "",
        "托特月", "帕欧皮月", "哈托爾月", "科亞克月", "托比月", "梅希爾月",
        "帕雷姆哈特月", "帕爾穆提月", "帕雄斯月", "帕欧尼月", "埃皮普月", "梅索里月", "補餘月"
    };

    public override string? GetCopticMonthName(int month)
        => month >= 1 && month <= 13 ? CopticMonthNames[month] : null;

    public override string FormatCopticYear(int year) => $"{year}年（AM）";
    public override string FormatCopticDay(int day)   => $"{day}日";

    public override string LocalizeCopticDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetCopticMonthName(month) ?? $"{month}月";
        return $"科普特曆{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 埃塞俄比亞曆本地化 =====

    public override string CalendarEthiopianName => "埃塞俄比亞曆（EC）";

    private static readonly string[] EthiopianMonthNames =
    {
        "",
        "梅斯克雷姆月", "提克姆特月", "希達爾月", "塔赫薩斯月", "提爾月", "耶卡提特月",
        "梅加比特月", "米亞齊亞月", "金博特月", "塞內月", "哈姆勒月", "內哈塞月", "帕古門月"
    };

    public override string? GetEthiopianMonthName(int month)
        => month >= 1 && month <= 13 ? EthiopianMonthNames[month] : null;

    public override string FormatEthiopianYear(int year) => $"{year}年（EC）";
    public override string FormatEthiopianDay(int day)   => $"{day}日";

    public override string LocalizeEthiopianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetEthiopianMonthName(month) ?? $"{month}月";
        return $"埃塞俄比亞曆{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 伊斯蘭曆本地化 =====

    public override string CalendarIslamicName => "伊斯蘭曆（伊曆）";

    private static readonly string[] IslamicMonthNames =
    {
        "",
        "穆哈蘭姆月", "色法爾月", "賴比爾·敖外魯月", "賴比爾·阿色尼月",
        "主馬達·敖外魯月", "主馬達·阿色尼月", "賴哲卜月", "捨爾邦月",
        "賴買丹月", "閃瓦魯月", "都爾·阿爾德月", "都爾·黑哲月"
    };

    public override string? GetIslamicMonthName(int month)
        => month >= 1 && month <= 12 ? IslamicMonthNames[month] : null;

    public override string FormatIslamicYear(int year) => $"{year}年（AH）";
    public override string FormatIslamicDay(int day)   => $"{day}日";

    public override string LocalizeIslamicDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIslamicMonthName(month) ?? $"{month}月";
        return $"伊曆{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 希伯來曆本地化 =====

    public override string CalendarHebrewName => "希伯來曆";

    private static readonly string[] HebrewMonthNames =
    {
        "",
        "提斯利月", "赫市萬月", "基斯流月", "提別月", "細罷特月",
        "亞達一月", "亞達二月", "尼散月", "以珥月", "西彎月",
        "搭模斯月", "埃波月", "以祿月"
    };

    public override string? GetHebrewMonthName(int month)
        => month >= 1 && month <= 13 ? HebrewMonthNames[month] : null;

    public override string FormatHebrewYear(int year) => $"{year}年（AM）";
    public override string FormatHebrewDay(int day)   => $"{day}日";

    public override string LocalizeHebrewDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetHebrewMonthName(month) ?? $"{month}月";
        return $"希伯來曆{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 波斯曆本地化 =====

    public override string CalendarPersianName => "波斯曆（太陽回曆）";

    private static readonly string[] PersianMonthNames =
    {
        "",
        "法爾瓦丁月", "奧爾迪貝赫什特月", "霍爾達德月", "提爾月", "莫爾達德月", "沙赫里瓦爾月",
        "梅赫爾月", "阿班月", "阿扎爾月", "代月", "巴赫曼月", "伊斯凡德月"
    };

    public override string? GetPersianMonthName(int month)
        => month >= 1 && month <= 12 ? PersianMonthNames[month] : null;

    public override string FormatPersianYear(int year) => $"{year}年（AP）";
    public override string FormatPersianDay(int day)   => $"{day}日";

    public override string LocalizePersianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetPersianMonthName(month) ?? $"{month}月";
        return $"波斯曆{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 印度國家曆本地化 =====

    public override string CalendarIndianName => "印度國家曆（薩迦曆）";

    private static readonly string[] IndianMonthNames =
    {
        "",
        "柴特拉月", "吠舍佉月", "逝瑟吒月", "阿沙荼月", "室羅伐拿月", "婆達羅鉢陀月",
        "阿濕縛庾闍月", "迦剌底迦月", "阿葛哈衍那月", "報沙月", "磨祛月", "頗勒窭拿月"
    };

    public override string? GetIndianMonthName(int month)
        => month >= 1 && month <= 12 ? IndianMonthNames[month] : null;

    public override string FormatIndianYear(int year) => $"{year}年（薩迦）";
    public override string FormatIndianDay(int day)   => $"{day}日";

    public override string LocalizeIndianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}月";
        return $"薩迦曆{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 薩迦紀元曆本地化 =====

    public override string CalendarSakaName => "薩迦紀元曆";

    public override string FormatSakaYear(int year) => $"{year}年（SE）";
    public override string FormatSakaDay(int day)   => $"{day}日";

    public override string LocalizeSakaDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}月";
        return $"薩迦紀元{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 維克拉姆曆本地化 =====

    public override string CalendarVikramSamvatName => "維克拉姆曆";

    public override string FormatVikramSamvatYear(int year) => $"{year}年（VS）";
    public override string FormatVikramSamvatDay(int day)   => $"{day}日";

    public override string LocalizeVikramSamvatDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}月";
        return $"維克拉姆曆{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 蒙古曆本地化 =====

    public override string CalendarMongolianName => "蒙古曆";

    public override string FormatMongolianYear(int year)   => $"{year}年";
    public override string FormatMongolianMonth(int month) => $"{month}月";
    public override string FormatMongolianDay(int day)     => $"{day}日";

    public override string LocalizeMongolianDate(int year, int month, int day, int hour, int minute, int second)
        => $"蒙古曆{year}年{month}月{day}日 {hour:D2}:{minute:D2}:{second:D2}";

    // ===== 爪哇曆本地化 =====

    public override string CalendarJavaneseName => "爪哇曆";

    private static readonly string[] JavaneseMonthNames =
    {
        "",
        "蘇拉月", "薩帕爾月", "穆魯德月", "巴克達穆魯德月",
        "朱馬迪拉瓦爾月", "朱馬迪拉基爾月", "雷傑布月", "魯瓦赫月",
        "帕薩月", "薩瓦爾月", "杜爾坎吉達月", "貝薩爾月"
    };

    public override string? GetJavaneseMonthName(int month)
        => month >= 1 && month <= 12 ? JavaneseMonthNames[month] : null;

    public override string FormatJavaneseYear(int year) => $"{year}年（AJ）";
    public override string FormatJavaneseDay(int day)   => $"{day}日";

    public override string LocalizeJavaneseDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJavaneseMonthName(month) ?? $"{month}月";
        return $"爪哇曆{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 藏曆本地化 =====

    public override string CalendarTibetanName => "藏曆";

    public override string FormatTibetanYear(int year)   => $"{year}年";
    public override string FormatTibetanMonth(int month) => $"{month}月";
    public override string FormatTibetanDay(int day)     => $"{day}日";

    public override string LocalizeTibetanDate(int year, int month, int day, int hour, int minute, int second)
        => $"藏曆{year}年{month}月{day}日 {hour:D2}:{minute:D2}:{second:D2}";

    // ===== 瑪雅曆本地化 =====

    public override string CalendarMayanName   => "瑪雅長紀曆";
    public override string CalendarMayanBaktun => "伯克頓";
    public override string CalendarMayanKatun  => "卡頓";
    public override string CalendarMayanTun    => "頓";
    public override string CalendarMayanUinal  => "維納爾";
    public override string CalendarMayanKin    => "金";

    public override string LocalizeMayanDate(int baktun, int katun, int tun, int uinal, int kin, int hour, int minute, int second)
        => $"{baktun}.{katun}.{tun}.{uinal}.{kin} {hour:D2}:{minute:D2}:{second:D2}";

    // ===== 因紐特曆本地化 =====

    public override string CalendarInuitName => "因紐特曆";

    private static readonly string[] InuitMonthNames =
    {
        "",
        "西金納奇亞克月", "阿武尼特月", "納提安月", "提里格魯特月", "阿米拉伊扎烏特月",
        "納茨維亞特月", "阿庫利特月", "西金納魯特月", "阿庫利魯西特月", "烏基烏克月",
        "烏基烏米納薩馬特月", "西金寧納米塔特奇克月", "陶維克朱亞克月"
    };

    public override string? GetInuitMonthName(int month)
        => month >= 1 && month <= 13 ? InuitMonthNames[month] : null;

    public override string FormatInuitYear(int year) => $"{year}年";
    public override string FormatInuitDay(int day)   => $"{day}日";

    public override string LocalizeInuitDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetInuitMonthName(month) ?? $"{month}月";
        return $"因紐特曆{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 羅馬曆本地化 =====

    public override string CalendarRomanName => "羅馬曆（建城紀年）";

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

    // ===== 農曆本地化 =====

    public override string CalendarChineseLunarName => "農曆";

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

    public override string ChineseLunarLeapPrefix => "閏";
    public override string CalendarComponentIsLeap => "閏月";
    public override string FormatChineseLunarYear(int year) => $"{year}年";

    public override string LocalizeChineseLunarDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? ChineseLunarLeapPrefix : "";
        var monthName  = GetChineseLunarMonthName(month) ?? $"{month}月";
        var dayName    = GetChineseLunarDayName(day) ?? $"{day}日";
        return $"{year}年{leapPrefix}{monthName}{dayName} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 越南曆本地化 =====

    public override string CalendarVietnameseName => "越南農曆（陰曆）";

    private static readonly string[] VietnameseMonthNames =
    {
        "",
        "正月", "二月", "三月", "四月", "五月", "六月",
        "七月", "八月", "九月", "十月", "十一月", "臘月"
    };

    private static readonly string[] VietnameseZodiacNames =
    {
        "子（鼠）", "丑（水牛）", "寅（虎）", "卯（貓）",
        "辰（龍）", "巳（蛇）", "午（馬）", "未（羊）",
        "申（猴）", "酉（雞）", "戌（狗）", "亥（豬）"
    };

    public override string? GetVietnameseMonthName(int month)
        => month >= 1 && month <= 12 ? VietnameseMonthNames[month] : null;

    public override string? GetVietnameseZodiacName(int index)
        => index >= 0 && index < 12 ? VietnameseZodiacNames[index] : null;

    public override string VietnameseLeapPrefix    => "閏";
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

    // ===== 日本曆本地化 =====

    public override string CalendarJapaneseName => "日本曆（年號）";

    private static readonly string[] JapaneseEraNames =
        { "令和", "平成", "昭和", "大正", "明治" };

    public override string? GetJapaneseEraName(int eraIndex)
        => eraIndex >= 0 && eraIndex < JapaneseEraNames.Length ? JapaneseEraNames[eraIndex] : null;

    public override string CalendarComponentEra  => "年號";
    public override string FormatJapaneseYear(int year) => $"{year}年";
    public override string FormatJapaneseDay(int day)   => $"{day}日";

    public override string LocalizeJapaneseDate(int eraIndex, int year, int month, int day, int hour, int minute, int second)
    {
        var eraName   = GetJapaneseEraName(eraIndex) ?? "";
        var monthName = GetGregorianMonthName(month) ?? $"{month}月";
        return $"{eraName}{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 彝曆本地化 =====

    public override string CalendarYiName => "彝曆（彝族太陽曆）";
    public override string CalendarComponentYiSeason => "季";
    public override string CalendarComponentYiXun    => "旬";

    private static readonly string[] YiSeasonNames = { "木季", "火季", "土季", "金季", "水季" };
    private static readonly string[] YiXunNames    = { "上旬", "中旬", "下旬" };
    private static readonly string[] YiAnimalNames = { "虎", "兔", "龍", "蛇", "馬", "羊", "猴", "雞", "狗", "豬", "鼠", "牛" };

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
        return $"彝曆{year}年[{zodiac}] {monthName} {dayStr} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 干支曆本地化 =====

    public override string CalendarSexagenaryName    => "干支曆";
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
        { "鼠", "牛", "虎", "兔", "龍", "蛇", "馬", "羊", "猴", "雞", "狗", "豬" };

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

    // ===== 西雙版納小傣曆 =====

    public override string CalendarDaiName => "西雙版納小傣曆";

    private static readonly string?[] DaiMonthNames =
    [
        null,
        "一月", "二月", "三月", "四月", "五月", "六月",
        "七月", "八月", "九月", "十月", "十一月", "十二月",
        "閏九月"
    ];

    public override string? GetDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DaiMonthNames[month] : null;

    public override string FormatDaiYear(int year) => $"{year}年";

    public override string FormatDaiDay(int day) => $"{day}日";

    public override string LocalizeDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "閏" : "") + (GetDaiMonthName(month) ?? $"{month}月");
        return $"傣曆{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 德宏大傣曆 =====

    public override string CalendarDehongDaiName => "德宏大傣曆";

    private static readonly string?[] DehongDaiMonthNames =
    [
        null,
        "一月", "二月", "三月", "四月", "五月", "六月",
        "七月", "八月", "九月", "十月", "十一月", "十二月",
        "閏九月"
    ];

    public override string? GetDehongDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DehongDaiMonthNames[month] : null;

    public override string FormatDehongDaiYear(int year) => $"{year}年";

    public override string FormatDehongDaiDay(int day) => $"{day}日";

    public override string LocalizeDehongDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "閏" : "") + (GetDehongDaiMonthName(month) ?? $"{month}月");
        return $"傣曆{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Memory Event Localization =====

    public override string FormatMemoryEventSingleChat(string partnerName, string content)
        => $"[單聊] 與「{partnerName}」對話，回覆：{content}";

    public override string FormatMemoryEventGroupChat(string sessionId, string content)
        => $"[群聊] 在工作階段 {sessionId} 中發言：{content}";

    public override string FormatMemoryEventToolCall(string toolNames)
        => $"[工具調用] 執行工具：{toolNames}";

    public override string FormatMemoryEventTask(string content)
        => $"[任務] 執行任務，結果：{content}";

    public override string FormatMemoryEventTimer(string content)
        => $"[定時] 定時觸發，回應：{content}";

    public override string FormatMemoryEventTimerError(string timerName, string error)
        => $"[定時] 定時器 '{timerName}' 執行失敗：{error}";

    // ===== Timer Notification Localization =====

    public override string FormatTimerStartNotification(string timerName)
        => $"⏰ 定時器 '{timerName}' 開始執行...";

    public override string FormatTimerEndNotification(string timerName, string result)
        => $"✅ 定時器 '{timerName}' 執行完成\n{result}";

    public override string FormatTimerErrorNotification(string timerName, string error)
        => $"❌ 定時器 '{timerName}' 執行失敗：{error}";

    public override string FormatMemoryEventBeingCreated(string name, string id)
        => $"[管理] 建立了新矽基人「{name}」（{id}）";

    public override string FormatMemoryEventBeingReset(string id)
        => $"[管理] 將矽基人 {id} 重置為預設實作";

    public override string FormatMemoryEventTaskCompleted(string taskTitle)
        => $"[任務完成] {taskTitle}";

    public override string FormatMemoryEventTaskFailed(string taskTitle)
        => $"[任務失敗] {taskTitle}";

    public override string FormatMemoryEventStartup()
        => "系統啟動，我上線了";

    public override string FormatMemoryEventRuntimeError(string message)
        => $"[執行時錯誤] {message}";

    // ===== MemoryTool Response Localization =====

    public override string MemoryToolNotAvailable => "記憶系統不可用";
    public override string MemoryToolMissingAction => "缺少 'action' 參數";
    public override string MemoryToolMissingContent => "缺少 'content' 參數";
    public override string MemoryToolNoMemories => "暫無記憶";
    public override string MemoryToolRecentHeader(int count) => $"最近 {count} 條記憶：";
    public override string MemoryToolStatsHeader => "記憶統計：";
    public override string MemoryToolStatsTotal => "- 總數";
    public override string MemoryToolStatsOldest => "- 最早";
    public override string MemoryToolStatsNewest => "- 最新";
    public override string MemoryToolStatsNA => "無";
    public override string MemoryToolQueryNoResults => "該時間範圍內無記憶";
    public override string MemoryToolQueryHeader(int count, string rangeDesc) => $"{rangeDesc} 共 {count} 條記憶：";
    public override string MemoryToolInvalidYear => "'year' 參數無效";
    public override string MemoryToolUnknownAction(string action) => $"未知操作：{action}";

    // ===== Code Editor Hover Tooltip Localization =====

    public override string GetCodeHoverWordTypeLabel(string wordType) => wordType switch
    {
        "variable" => "變數",
        "function" => "函數",
        "class" => "類別",
        "keyword" => "關鍵字",
        "comment" => "註釋",
        "namespace" => "命名空間",
        "parameter" => "參數",
        _ => "識別符"
    };

    public override string GetCodeHoverWordTypeDesc(string wordType, string word)
    {
        var encodedWord = System.Net.WebUtility.HtmlEncode(word);
        return wordType switch
        {
            "variable" => $"變數 '{encodedWord}' 的定義和使用資訊",
            "function" => $"函數 '{encodedWord}' 的簽名和說明",
            "class" => $"類別 '{encodedWord}' 的結構和說明",
            "keyword" => $"關鍵字 '{encodedWord}' 的語法和用途",
            "comment" => $"註釋中的單詞 '{encodedWord}'",
            "namespace" => $"命名空間 '{encodedWord}' 的資訊",
            "parameter" => $"參數 '{encodedWord}' 的定義和用途",
            _ => $"識別符 '{encodedWord}' 的資訊"
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
        { "csharp:if", "條件分支語句。當條件表達式為 true 時執行程式碼區塊。" },
        { "csharp:else", "條件分支的替代路徑。與 if 配合使用，當條件為 false 時執行。" },
        { "csharp:for", "計數迴圈語句。包含初始化、條件判斷和迭代三個部分。" },
        { "csharp:while", "條件迴圈語句。條件為 true 時重複執行程式碼區塊。" },
        { "csharp:do", "後測試迴圈語句。先執行一次程式碼區塊，再判斷條件是否繼續迴圈。" },
        { "csharp:switch", "多選分支語句。根據表達式的值匹配不同的 case 分支。" },
        { "csharp:case", "switch 語句中的分支標籤。匹配特定值時執行對應程式碼區塊。" },
        { "csharp:break", "跳出語句。立即終止最近的迴圈或 switch 語句。" },
        { "csharp:continue", "繼續語句。跳過當前迴圈的剩餘部分，進入下一次迭代。" },
        { "csharp:return", "返回語句。退出當前方法並可選地返回一個值。" },
        { "csharp:goto", "跳轉語句。無條件跳轉到指定的標籤位置。" },
        { "csharp:foreach", "集合遍歷語句。依次訪問集合中的每個元素。" },
        { "csharp:class", "參考型別宣告。定義包含資料（欄位、屬性）和行為（方法）的結構。" },
        { "csharp:interface", "介面宣告。定義類別或結構必須實作的契約。" },
        { "csharp:struct", "實值型別宣告。輕量級資料結構，配置在堆疊上。" },
        { "csharp:enum", "列舉型別宣告。定義一組具名的整數常量。" },
        { "csharp:namespace", "命名空間宣告。組織程式碼的邏輯容器，避免命名衝突。" },
        { "csharp:record", "記錄型別宣告。具有值語義的參考型別，適合不可變資料。" },
        { "csharp:delegate", "委託宣告。型別安全的方法參考，用於事件和回呼。" },
        { "csharp:public", "公共存取修飾詞。成員可被任何程式碼存取。" },
        { "csharp:private", "私有存取修飾詞。成員只能在包含型別內部存取。" },
        { "csharp:protected", "受保護存取修飾詞。成員可在包含型別及其衍生型別中存取。" },
        { "csharp:internal", "內部存取修飾詞。成員只能在同一組件內存取。" },
        { "csharp:sealed", "密封修飾詞。防止類別被繼承或方法被覆寫。" },
        { "csharp:int", "32 位元帶符號整數型別（System.Int32 的別名）。" },
        { "csharp:string", "字串型別（System.String 的別名）。表示 Unicode 字元序列，不可變。" },
        { "csharp:bool", "布林型別（System.Boolean 的別名）。值為 true 或 false。" },
        { "csharp:float", "32 位元單精確度浮點數型別（System.Single 的別名）。" },
        { "csharp:double", "64 位元雙精確度浮點數型別（System.Double 的別名）。" },
        { "csharp:decimal", "128 位元高精確度十進位數型別，適合金融計算。" },
        { "csharp:char", "16 位元 Unicode 字元型別（System.Char 的別名）。" },
        { "csharp:byte", "8 位元無符號整數型別（System.Byte 的別名）。" },
        { "csharp:object", "所有型別的基底類別（System.Object 的別名）。" },
        { "csharp:var", "隱含型別區域變數。編譯器從初始化表達式推斷型別。" },
        { "csharp:dynamic", "動態型別。跳過編譯時型別檢查，在執行時期解析。" },
        { "csharp:void", "無回傳值型別。表示方法不回傳任何值。" },
        { "csharp:static", "靜態修飾詞。屬於型別本身而非特定物件實例。" },
        { "csharp:abstract", "抽象修飾詞。表示不完整的實作，必須由衍生類別完成。" },
        { "csharp:virtual", "虛擬修飾詞。方法或屬性可在衍生類別中被覆寫。" },
        { "csharp:override", "覆寫修飾詞。提供對基底類別虛擬方法或抽象方法的新實作。" },
        { "csharp:const", "常量修飾詞。編譯時確定的不可變值。" },
        { "csharp:readonly", "唯讀修飾詞。只能在宣告或建構函式中賦值。" },
        { "csharp:volatile", "易變修飾詞。表示欄位可能被多個執行緒併行修改。" },
        { "csharp:async", "非同步修飾詞。標記方法包含非同步作業，通常與 await 配合使用。" },
        { "csharp:await", "等待運算子。暫停方法執行直到非同步作業完成。" },
        { "csharp:partial", "分部修飾詞。允許類別、結構或介面分布在多個檔案中。" },
        { "csharp:ref", "參考參數。按參考傳遞參數。" },
        { "csharp:out", "輸出參數。用於從方法中回傳多個值。" },
        { "csharp:in", "唯讀參考參數。按參考傳遞但不允許修改。" },
        { "csharp:params", "可變參數修飾詞。允許傳遞可變數量的同型別參數。" },
        { "csharp:try", "例外處理區塊。包含可能擲回例外的程式碼。" },
        { "csharp:catch", "例外擷取區塊。處理 try 區塊中擲回的例外。" },
        { "csharp:finally", "最終執行區塊。無論是否發生例外都會執行的程式碼。" },
        { "csharp:throw", "擲回例外語句。手動擲回例外物件。" },
        { "csharp:new", "建立實例運算子。建立物件或呼叫建構函式。" },
        { "csharp:this", "當前實例參考。參考當前類別的實例。" },
        { "csharp:base", "基底類別參考。參考直接基底類別的成員。" },
        { "csharp:using", "指示詞或語句。匯入命名空間或確保 IDisposable 資源被釋放。" },
        { "csharp:yield", "迭代器關鍵字。逐個回傳值，實現延遲執行。" },
        { "csharp:lock", "鎖定語句。確保同一時刻只有一個執行緒執行程式碼區塊。" },
        { "csharp:typeof", "型別運算子。取得型別的 System.Type 物件。" },
        { "csharp:nameof", "名稱運算子。取得變數、型別或成員的字串名稱。" },
        { "csharp:is", "型別檢查運算子。檢查物件是否相容指定型別。" },
        { "csharp:as", "型別轉換運算子。安全地嘗試型別轉換，失敗時回傳 null。" },
        { "csharp:null", "空值字面量。表示參考型別或可空型別的空參考。" },
        { "csharp:true", "布林真值。" },
        { "csharp:false", "布林假值。" },
        { "csharp:default", "預設值表達式。取得型別的預設值（參考型別為 null，數值型別為 0）。" },
        { "csharp:operator", "運算子宣告。定義自訂型別上的運算子行為。" },
        { "csharp:explicit", "明確轉換宣告。需要強制型別轉換的轉換運算子。" },
        { "csharp:implicit", "隱含轉換宣告。可自動執行的轉換運算子。" },
        { "csharp:unchecked", "取消檢查區塊。停用整型算術溢位的檢查。" },
        { "csharp:checked", "檢查區塊。啟用整型算術溢位的檢查。" },
        { "csharp:fixed", "固定語句。固定記憶體位置以防止記憶體回收移動。" },
        { "csharp:stackalloc", "堆疊配置運算子。在堆疊上配置記憶體區塊。" },
        { "csharp:extern", "外部修飾詞。表示方法在外部組件中實作（如 DLL）。" },
        { "csharp:unsafe", "不安全程式碼區塊。允許使用指標等不安全特性。" },
        // 平台核心類型
        { "csharp:ipermissioncallback", "權限回呼介面。用於評估矽基生命體的各種操作權限（網路、命令列、檔案存取等）。" },
        { "csharp:permissionresult", "權限結果列舉。表示權限評估的結果：Allowed（允許）、Denied（拒絕）、AskUser（詢問使用者）。" },
        { "csharp:permissiontype", "權限類型列舉。定義權限的種類：NetworkAccess（網路存取）、CommandLine（命令列執行）、FileAccess（檔案存取）、Function（函式呼叫）、DataAccess（資料存取）。" },
        // System.Net
        { "csharp:ipaddress", "IP 位址類別（System.Net.IPAddress）。表示 Internet Protocol (IP) 位址。" },
        { "csharp:addressfamily", "位址族列舉（System.Net.Sockets.AddressFamily）。指定網路位址的定址方案，如 InterNetwork（IPv4）、InterNetworkV6（IPv6）。" },
        // System
        { "csharp:uri", "統一資源識別碼類別（System.Uri）。提供 URI（Uniform Resource Identifier）的物件表示，用於存取 Web 資源。" },
        { "csharp:operatingsystem", "作業系統類別（System.OperatingSystem）。提供用於檢查目前作業系統的靜態方法，如 IsWindows()、IsLinux()、IsMacOS()。" },
        { "csharp:environment", "環境類別（System.Environment）。提供有關目前環境和平台的資訊，以及操作它們的方法。" },
        // System.IO
        { "csharp:path", "路徑類別（System.IO.Path）。對包含檔案或目錄路徑資訊的 String 執行個體執行操作。" },
        // System.Collections.Generic
        { "csharp:hashset", "雜湊集類別（System.Collections.Generic.HashSet<T>）。表示值的集，提供高效能的集運算。" },
        // System.Text
        { "csharp:stringbuilder", "字串建構器類別（System.Text.StringBuilder）。表示可變字元字串，適合頻繁修改字串的場景。" },
    };

    // 完整命名空間翻譯字典
    private static readonly Dictionary<string, string> TranslationDictionary = new(CSharpKeywords)
    {
        // 添加完整命名空間鍵
        { "csharp:System.Net.IPAddress", "IP 位址類別（System.Net.IPAddress）。表示 Internet Protocol (IP) 位址。" },
        { "csharp:System.Net.Sockets.AddressFamily", "位址族列舉（System.Net.Sockets.AddressFamily）。指定網路位址的定址方案，如 InterNetwork（IPv4）、InterNetworkV6（IPv6）。" },
        { "csharp:System.Uri", "統一資源識別碼類別（System.Uri）。提供 URI（Uniform Resource Identifier）的物件表示，用於存取 Web 資源。" },
        { "csharp:System.OperatingSystem", "作業系統類別（System.OperatingSystem）。提供用於檢查目前作業系統的靜態方法，如 IsWindows()、IsLinux()、IsMacOS()。" },
        { "csharp:System.Environment", "環境類別（System.Environment）。提供有關目前環境和平台的資訊，以及操作它們的方法。" },
        { "csharp:System.IO.Path", "路徑類別（System.IO.Path）。對包含檔案或目錄路徑資訊的 String 執行個體執行操作。" },
        { "csharp:System.Collections.Generic.HashSet", "雜湊集類別（System.Collections.Generic.HashSet<T>）。表示值的集，提供高效能的集運算。" },
        { "csharp:System.Text.StringBuilder", "字串建構器類別（System.Text.StringBuilder）。表示可變字元字串，適合頻繁修改字串的場景。" },
    };
}
