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
    public override string PageTitleConfig => "系統設定 - 矽基生命群";
    public override string PageTitleExecutor => "執行器監控 - 矽基生命群";
    public override string PageTitleCodeBrowser => "程式碼瀏覽 - 矽基生命群";
    public override string PageTitlePermission => "權限管理 - 矽基生命群";
    public override string PageTitleAbout => "關於 - 矽基生命群";

    // ===== Chat =====

    public override string SingleChatNameFormat => "與{0}聊天";
    public override string ChatConversationsHeader => "對話";
    public override string ChatNoConversationSelected => "選擇對話開始聊天";
    public override string ChatMessageInputPlaceholder => "輸入訊息...";
    public override string ChatSendButton => "傳送";
    public override string ChatUserDisplayName => "我";
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
    public override string BeingsDetailMemoryLabel => "記憶：";
    public override string BeingsDetailMemoryViewLink => "查看";
    public override string BeingsDetailPermissionLabel => "權限：";
    public override string BeingsDetailPermissionEditLink => "編輯";
    public override string BeingsDetailTimersLabel => "計時器：";
    public override string BeingsDetailTasksLabel => "任務：";
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
        ["OllamaEndpoint"] = "Ollama 端點",
        ["DefaultModel"] = "預設模型",
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
        ["WebPort"] = "Web 伺服器連接埠",
        ["AllowIntranetAccess"] = "允許內網存取（需要管理員權限）",
        ["WebSkin"] = "Web 主題名稱",
        ["UserNickname"] = "人類用戶的暱稱"
    };

    public override string GetConfigGroupName(string groupKey) =>
        ConfigGroupNames.GetValueOrDefault(groupKey, groupKey);

    public override string GetConfigDisplayName(string displayNameKey) =>
        ConfigDisplayNames.GetValueOrDefault(displayNameKey, displayNameKey);

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
        ["timer"] = "計時器"
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
}
