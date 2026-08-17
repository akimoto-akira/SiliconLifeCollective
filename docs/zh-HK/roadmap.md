# 路線圖

> **版本：v0.2.0-alpha**

[English](../en/roadmap.md) | [Deutsch](../de-DE/roadmap.md) | [中文](../zh-CN/roadmap.md) | **繁體中文** | [Español](../es-ES/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Čeština](../cs-CZ/roadmap.md) | [Русский](../ru-RU/roadmap.md)

## 雙版本路線圖

### SiliconLife.Default（預設版本）
- **定位**：預設實作，主要用於驗證架構可行性
- **當前狀態**：階段 1-10.6 已完成，系統穩定運行
- **角色說明**：作為架構驗證的基準實作，確保核心架構設計的正確性和可行性

### SiliconLife.Fast（高效能版本）
- **定位**：主推生產版本
- **當前狀態**：已完成基礎架構移植，SpeedyPack 儲存引擎和外掛程式系統已實作
- **角色說明**：在 Default 版本驗證的架構基礎上，進行深度效能優化和生產級特性增強，是實際部署的首選

**Fast 版本開發計劃**：
- ✅ 階段 1：基礎專案結構和設定系統移植
- ✅ 階段 2：Web UI 和控制器移植
- ✅ 階段 3：儲存系統優化（SpeedyPack 記憶體儲存 + 非同步持久化）
- ✅ 階段 3.5：SpeedyPack 管理工具（SiliconLife.Speedy.Manager Avalonia UI 應用）
- ✅ 階段 3.6：外掛程式系統（IPlugin 介面、安全沙箱、AssemblyLoadContext 隔離）
- ✅ 階段 4：Avalonia 窗體應用（跨平台桌面應用，Windows/macOS 系統匣，Linux 狀態視窗）

---

## 指導原則

每個階段都以**可運行、可觀察**的系統結束。沒有階段會產生「一堆基礎設施卻沒有可展示的東西」。

---

## ~~階段 1：可以聊天~~ ✅ 已完成

**目標**：控制台輸入 → AI 呼叫 → 控制台輸出。最小可驗證單元。

| # | 模組 | 描述 |
|---|--------|-------------|
| 1.1 | 解決方案和專案結構 | 建立 `SiliconLifeCollective.sln`，包含 `src/SiliconLife.Core/`（核心庫）和 `src/SiliconLife.Default/`（預設實作 + 入口點） |
| 1.2 | 設定（最小化） | 單例 + JSON 反序列化。讀取 `config.json`。如果缺失則自動產生預設值 |
| 1.3 | 在地化（最小化） | `LocalizationBase` 抽象類別，`ZhCN` 實作。在設定中新增 `Language` |
| 1.4 | OllamaClient（最小化） | `IAIClient` 介面，HTTP 呼叫本地 Ollama `/api/chat`。尚無串流傳輸，尚無工具呼叫 |
| 1.5 | 控制台 I/O | `while(true) + Console.ReadLine()`，讀取輸入 → 呼叫 AI → 列印回應 |
| 1.6 | 版權標頭 | 為所有 C# 原始檔案新增 Apache 2.0 標頭 |

**交付物**：與本地 Ollama 模型對話的控制台聊天程式。

**驗證**：執行程式，輸入「hello」，看到 AI 回應。

---

## ~~階段 2：有骨架~~ ✅ 已完成

**目標**：用框架結構替換「裸迴圈」。行為不變。

| # | 模組 | 描述 |
|---|--------|-------------|
| 2.1 | 儲存（最小化） | `IStorage` 介面（Read/Write/Exists/Delete，鍵值對）。`FileSystemStorage` 實作。實例類別（非靜態）。直接檔案系統存取 —— **AI 無法控制 IStorage** |
| 2.2 | 主迴圈 + 時鐘物件 | 無限迴圈，精確時鐘間隔（`Stopwatch` + `Thread.Sleep`）。優先級排程 |
| 2.3 | IAIClient 標準化 | `IAIClientFactory` 介面。OllamaClient 重構以實作標準介面 |
| 2.4 | 控制台遷移 | 將 `while(true)` 遷移到主迴圈驅動的時鐘物件。行為與階段 1 相同 |

**交付物**：主迴圈運行時鐘，控制台聊天仍然工作。

**驗證**：註冊一個測試時鐘物件，每秒列印時鐘計數；控制台聊天仍然工作。

---

## ~~階段 3：有靈魂~~ ✅ 已完成

**目標**：第一個矽基生命體在框架中存活。

| # | 模組 | 描述 |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | 抽象基底類別，包含 Id、Name、ToolManager、AIClient、ChatService、Storage、PermissionService。抽象 `Tick()` 和 `ExecuteOneRound()` |
| 3.2 | 靈魂檔案載入 | `SoulFileManager`：從生命體資料目錄讀取 `soul.md` |
| 3.3 | ContextManager（最小化） | 連接靈魂檔案 + 最近訊息 → 呼叫 AI → 取得回應。尚無工具呼叫，尚無持久化 |
| 3.4 | ISiliconBeingFactory | 用於建立生命體實例的工廠介面 |
| 3.5 | SiliconBeingManager（最小化） | 繼承時鐘物件（優先級=0）。迭代所有生命體，依次呼叫它們的 Tick |
| 3.6 | DefaultSiliconBeing | 標準行為實作。檢查未讀訊息 → 建立 ContextManager → ExecuteOneRound → 輸出 |
| 3.7 | 生命體目錄結構 | `DataDirectory/SiliconManager/{GUID}/`，包含 `soul.md` 和 `state.json` |

**交付物**：由主迴圈驅動的矽基生命體，接收控制台輸入，載入靈魂檔案，呼叫 AI。

**驗證**：控制台輸入 → 主迴圈時鐘觸發 → 生命體處理（帶靈魂檔案指導的行為） → AI 回應。回應風格應與階段 1 不同。

---

## ~~階段 4：有記憶~~ ✅ 已完成

**目標**：對話在重啟後持久化。

| # | 模組 | 描述 |
|---|--------|-------------|
| 4.1 | ChatSystem | 頻道概念（兩個 GUID = 一個頻道）。帶持久化的訊息模型。尚無群聊 |
| 4.2 | IIMProvider + IMManager | `IIMProvider` 介面。`ConsoleProvider` 作為正式即時通訊頻道。`IMManager` 路由訊息 |
| 4.3 | ContextManager 增強 | 從聊天系統拉取歷史。持久化 AI 回應。支援多輪工具呼叫延續 |
| 4.4 | IMessage 模型 | 聊天系統和即時通訊管理器共享的統一訊息模型 |

**交付物**：具有持久化記憶的聊天系統。

**驗證**：聊天幾輪 → 退出 → 重啟 → 問「我們聊了什麼？」 → 生命體可以回答。

---

## ~~階段 5：可以行動（工具系統）~~ ✅ 已完成

**目標**：矽基生命體可以執行操作，而不僅僅是聊天。

| # | 模組 | 描述 |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | `ITool` 介面，包含 Name、Description、Execute。`ToolResult` 包含 Success、Message、Data |
| 5.2 | ToolManager | 每個生命體的實例。基於反射的工具發現。`[SiliconManagerOnly]` 屬性支援 |
| 5.3 | IAIClient：工具呼叫支援 | 解析 AI tool_calls。迴圈：執行工具 → 傳送結果回去 → AI 繼續 → 直到純文字 |
| 5.4 | 執行器基底類別 | 抽象基底類別，具有獨立排程執行緒、請求佇列、逾時控制 |
| 5.5 | NetworkExecutor | 透過執行器進行 HTTP 請求。逾時、排隊 |
| 5.6 | CommandLineExecutor | 透過執行器進行 Shell 執行。跨平台分隔符偵測 |
| 5.7 | DiskExecutor | 透過執行器進行檔案操作。尚無權限檢查（階段 6） |
| 5.8–5.12 | 內建工具 | CalendarTool、SystemTool、NetworkTool、ChatTool、DiskTool |

**交付物**：矽基生命體可以呼叫工具執行操作。

**驗證**：問「今天星期幾」 → CalendarTool 回答；問「檢查程序」 → SystemTool 執行；告訴生命體給另一個生命體發訊息 → ChatTool 工作。

---

## ~~階段 6：遵守規則（權限系統）~~ ✅ 已完成

**目標**：矽基生命體未經授權無法存取敏感資源。

| # | 模組 | 描述 |
|---|--------|-------------|
| 6.1 | PermissionManager | 每個生命體的私有實例。基於回呼，三元結果（Allowed/Deny/AskUser）。查詢優先級：HighDeny → HighAllow → Callback。IsCurator 標誌 |
| 6.2 | PermissionType 列舉 | NetworkAccess、CommandLine、FileAccess、Function、DataAccess |
| 6.3 | DefaultPermissionCallback | 網路白名單/黑名單、CLI 分類、檔案路徑安全規則 |
| 6.4 | GlobalACL | 前綴匹配規則表，持久化到儲存 |
| 6.5 | UserFrequencyCache | HighAllow/HighDeny 列表。使用者選擇（非自動偵測）。前綴匹配，僅記憶體，可設定過期 |
| 6.6 | UserAskMechanism（控制台） | 當回傳 AskUser 時控制台提示 y/n |
| 6.7 | 執行器權限整合 | 所有執行器在執行前檢查權限 |
| 6.8 | IStorage 隔離說明 | IStorage 是系統內部持久化 —— 直接檔案存取，**不**透過執行器路由，**不**可由 AI 控制。執行器僅管理 AI 工具發起的 IO |
| 6.9 | 審計日誌 | 記錄所有權限決策，帶時間戳、請求者、資源、結果 |

**交付物**：當生命體嘗試敏感操作時出現權限提示。

**驗證**：告訴生命體刪除檔案 → 控制台顯示權限提示 → 輸入 `n` → 操作被拒絕。告訴生命體存取白名單網站 → 立即允許。

---

## ~~階段 7：可以進化（動態編譯）~~ ✅ 已完成

**目標**：矽基生命體可以重寫自己的程式碼。

| # | 模組 | 描述 |
|---|--------|-------------|
| 7.1 | CodeEncryption | AES-256 加密/解密。從 GUID 衍生 PBKDF2 金鑰 |
| 7.2 | DynamicCompilationExecutor | 基於 Roslyn 的記憶體編譯沙箱。編譯時裝配引用控制（主要防禦：排除 System.IO、Reflection 等） |
| 7.3 | 安全掃描 | 執行時靜態分析危險程式碼模式（次要防禦）。如果掃描失敗則阻止載入 |
| 7.4 | 生命體生命週期增強 | 載入：解密 → 掃描 → 編譯 → 實例化。執行時：在記憶體中編譯 → 原子替換 → 持久化加密 |
| 7.5 | SiliconCurator | 主理人抽象基底類別。IsCurator=true。最高權限 |
| 7.6 | DefaultCurator | 預設主理人實作，帶內建靈魂檔案和管理工具 |
| 7.7 | CuratorTool | `[SiliconManagerOnly]` 工具：list_beings、create_being、get_code、reset |
| 7.8 | 權限回呼覆蓋 | 生命體可以編譯自訂權限回呼 |
| 7.9 | SiliconBeingManager 增強 | Replace 方法（執行時實例交換）。MigrateState（在舊實例和新實例之間轉移狀態） |

**交付物**：矽基生命體可以透過 AI 產生新程式碼，編譯並替換自己。

**驗證**：告訴生命體「給自己新增一個新功能」 → 觀察編譯 → 重啟 → 新功能工作。

---

## ~~階段 8：記憶和計劃~~ ✅ 已完成

**目標**：長期記憶、任務管理、定時觸發。

| # | 模組 | 描述 |
|---|--------|-------------|
| 8.1 | FileSystemMemory | 短期/長期分段儲存。時間衰減。壓縮（合併相似記憶）。多維搜尋 |
| 8.2 | TaskSystem | 一次性 + DAG 依賴任務。優先級排程。狀態追蹤 |
| 8.3 | TimerSystem | 一次性鬧鐘 + 週期定時器。毫秒精度。持久化到儲存 |
| 8.4 | IncompleteDate | 模糊日期範圍結構（例如「2026 年 4 月」、「2026 年春」） |
| 8.5–8.7 | 記憶/任務/定時器工具 | 生命體查詢記憶、管理任務、設定定時器的工具 |

**交付物**：生命體可以記住關鍵點、建立/追蹤任務、設定鬧鐘。

**驗證**：建立任務 → 檢查任務列表 → 設定 1 分鐘鬧鐘 → 時間到時接收通知。

---

## ~~階段 9：框架完成~~ ✅ 已完成

**目標**：統一入口點，多生命體協作。

| # | 模組 | 描述 |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | 使用建構器模式的統一主機。優雅關閉（Ctrl+C / SIGTERM） |
| 9.2 | Program.Main 重構 | 遷移到 CoreHostBuilder 模式 |
| 9.3 | SiliconBeingManager 增強 | 主理人優先回應。異常隔離。定期持久化 |
| 9.4 | 多生命體載入 | 從資料目錄載入多個生命體。透過 ChatTool 進行生命體間通訊 |
| 9.5 | 效能監視器 | 每個時鐘物件執行時間追蹤 |
| 9.6 | ServiceLocator | 全域服務定位器，帶 Register/Get 方法 |

**交付物**：多個生命體同時運行，協作，由 CoreHost 管理。

**驗證**：建立兩個生命體 → A 給 B 發訊息 → B 接收並回覆 → 框架排程無錯誤。當使用者訊息到達時主理人優先回應。

---

## ~~階段 10：走向 Web~~ ✅ 已完成

**目標**：從控制台遷移到瀏覽器介面。

| # | 模組 | 描述 |
|---|--------|-------------|
| 10.1 | Router | HTTP 請求路由器。序列參數路由和靜態檔案服務 |
| 10.2 | Controller 基底類別 | 請求/回應上下文。HTML 和 JSON 回應支援 |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | C# 伺服器端建構器。零前端框架依賴 |
| 10.6 | SSE（伺服器傳送事件） | 推送式即時更新聊天、生命體狀態和系統事件。比 WebSocket 更簡單，帶自動用戶端重連 |
| 10.7 | WebUIProvider | 基於 SSE 的即時即時通訊頻道。替換控制台作為主要介面 |
| 10.8 | Web 安全 | IP 黑名單/白名單。`[WebCode]` 屬性。動態更新 |
| 10.9–10.17 | Web 控制器 | 聊天、儀表板、生命體、任務、權限、權限請求、執行器、日誌、設定、記憶、定時器、初始化、關於、程式碼瀏覽器、知識、專案、審計 |

**交付物**：可從瀏覽器存取的完整 Web UI。

**驗證**：開啟瀏覽器 → 與生命體聊天 → 檢視儀表板 → 管理權限 → 全部功能正常。

---

## ~~階段 10.5：增量增強~~ ✅ 已完成

**目標**：使用開發過程中發現的新功能增強現有系統。

| # | 模組 | 描述 |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | 用於系統範圍公告的新會話類型。固定頻道 ID、動態訂閱、待處理訊息過濾 |
| 10.5.2 | ChatMessage 增強 | ToolCallId、ToolCallsJson、Thinking 欄位用於 AI 上下文；PromptTokens、CompletionTokens、TotalTokens 用於 token 追蹤；SystemNotification 訊息類型 |
| 10.5.3 | TokenUsageAuditManager | 跨所有生命體的每次請求 token 消耗追蹤。聚合統計、時間序列查詢、持久化儲存 |
| 10.5.4 | TokenAuditTool | `[SiliconManagerOnly]` 工具，供主理人查詢和彙總 token 使用 |
| 10.5.5 | ConfigTool | `[SiliconManagerOnly]` 工具，供主理人讀取和修改系統設定 |
| 10.5.6 | AuditController | Web 儀表板用於 token 使用審計，帶趨勢圖和資料匯出 |
| 10.5.7 | 日曆系統擴展 | 32 種日曆實作，涵蓋世界日曆系統（佛曆、農曆、伊斯蘭曆、希伯來曆、日本曆、波斯曆、瑪雅曆等） |
| 10.5.8 | DiskTool 增強 | 新操作：count_lines、read_lines、clear_file、replace_lines、replace_text、replace_text_all、list_drives |
| 10.5.9 | SystemTool 增強 | 新操作：find_process（支援萬用字元）、resource_usage |
| 10.5.10 | CalendarTool 增強 | 新操作：diff、list_calendars、get_components、get_now_components、convert（跨日曆轉換） |
| 10.5.11 | DashScopeClient | 阿里雲百鍊 AI 用戶端，相容 OpenAI API。支援串流傳輸、工具呼叫、推理內容 |
| 10.5.12 | DashScopeClientFactory | 用於建立百鍊用戶端的工廠。透過 API 動態模型發現。多區域支援（北京、弗吉尼亞、新加坡、香港、法蘭克福） |
| 10.5.13 | AI 用戶端設定系統 | 每個生命體的 AI 用戶端設定。動態設定鍵選項（模型、區域）。在地化顯示名稱 |
| 10.5.14 | 在地化擴展 | 簡體中文、繁體中文、英文和日語在地化，用於百鍊設定選項、模型名稱和區域名稱 |

**交付物**：增強的工具、可觀察性、日曆覆蓋範圍和多 AI 後端支援。

**驗證**：主理人透過 TokenAuditTool 查詢 token 使用 → 審計儀表板顯示趨勢 → CalendarTool 在 32 種日曆系統之間轉換日期 → 將 AI 後端切換到百鍊 → 透過雲端 API 與通義千問模型聊天。

---

## ~~階段 10.6：完善與優化~~ ✅ 已完成

**目標**：完善系統功能，新增新特性，優化使用者體驗。

| # | 模組 | 描述 |
|---|--------|-------------|
| 10.6.1 | WebViewBrowserTool | 基於 Playwright 的跨平台瀏覽器自動化工具，支援無頭模式、個體隔離、完整 JS/CSS 支援 |
| 10.6.2 | HelpTool | 幫助文件系統工具，支援多語言文件查詢和展示 |
| 10.6.3 | ProjectWorkNoteTool | 專案工作筆記工具，支援專案維度的工作記錄和管理 |
| 10.6.4 | ProjectTaskTool | 專案任務管理工具，支援任務分配、進度追蹤 |
| 10.6.5 | KnowledgeTool | 知識網絡工具，支援三元組知識的增刪改查和路徑發現 |
| 10.6.6 | ChatHistoryController | 聊天歷史檢視控制器，支援會話列表和訊息詳情 |
| 10.6.7 | CodeHoverController | 程式碼懸浮提示控制器，支援語法高亮和程式碼提示 |
| 10.6.8 | WorkNoteController | 工作筆記管理控制器，支援搜尋和目錄產生 |
| 10.6.9 | TimerExecutionHistory | 定時器執行歷史功能，記錄和檢視定時器觸發歷史 |
| 10.6.10 | 在地化擴展 | 新增捷克語 (cs-CZ) 在地化支援，總計 21 種語言變體 |
| 10.6.11 | Web UI 優化 | 檔案上傳支援、載入指示器、工具呼叫渲染優化、工作筆記模態框修復 |
| 10.6.12 | 記憶管理增強 | 進階過濾、統計資訊、詳情檢視、壓縮演算法優化 |
| 10.6.13 | 日誌系統重構 | 系統/矽基生命體日誌分離、日誌讀取 API、矽基生命體篩選器 |
| 10.6.14 | 權限系統增強 | 權限回呼預編譯驗證、程式集引用驗證、wttr.in 天氣服務白名單 |

**交付物**：完整的 WebView 瀏覽器自動化、幫助文件系統、專案工作區、知識網絡、聊天歷史檢視等增強功能。

**驗證**：矽基生命體可以透過 WebViewBrowserTool 操作瀏覽器 → 透過 HelpTool 取得幫助文件 → 管理專案工作筆記和任務 → 查詢知識網絡 → 檢視聊天歷史。

---

## ~~階段 10.7：專案協作與工作流~~ ✅ 已完成

**目標**：新增專案工作區、工作流引擎、記憶淡忘機制和工具權限系統。

| # | 模組 | 描述 |
|---|--------|-------------|
| 10.7.1 | 專案角色管理 | ProjectTool 新增 assign_role、remove_role、list_roles 操作 |
| 10.7.2 | 工作流引擎 | WorkflowEngine 核心引擎，支援範本定義、狀態轉換、Tick 驅動執行 |
| 10.7.3 | 工作流範本 | WorkflowTemplate 基底類別，定義狀態集合和轉換規則 |
| 10.7.4 | 工作流實例 | WorkflowInstance 實例管理，綁定到具體專案，追蹤當前狀態 |
| 10.7.5 | 工作流日誌 | WorkflowLog 記錄狀態轉換歷史 |
| 10.7.6 | 記憶淡忘機制 | MemoryFadeService 定時衰減服務，每小時自動對記憶進行重要性衰減和歸檔 |
| 10.7.7 | 工具權限系統 | 兩級工具權限（矽基生命體級別 + 專案級別），權限範本，操作粒度控制 |
| 10.7.8 | ToolPermissionController | 工具權限管理 Web 控制器 |
| 10.7.9 | ProjectWorkTool | 專案工作操作工具（[SiliconManagerOnly]，[ToolScenario(Project)]） |
| 10.7.10 | 工具場景系統 | ToolScenarioAttribute 和 ChatOnlyAttribute，支援 Chat/Task/Timer/MemoryCompression/Project 場景過濾 |
| 10.7.11 | 在地化擴展 | 新增俄語、葡萄牙語、義大利語、荷蘭語、波蘭語、瑞典語在地化，總計 34 種語言變體 |

**交付物**：完整的專案協作系統、工作流引擎、記憶淡忘機制和工具權限管理。

**驗證**：建立專案 → 分配角色 → 綁定工作流範本 → 生命體在專案空間內協作 → 記憶自動衰減歸檔 → 工具權限隔離生效。

---

## 階段 11：外部即時通訊整合

**目標**：連接到外部訊息傳遞平台，以更廣泛的使用者可存取性。

| # | 模組 | 描述 |
|---|--------|-------------|
| 11.1 | ~~FeishuProvider~~ ✅ 已完成 | 飛書機器人整合（HTTP 回呼、簽名驗證 + AES 解密、互動卡片、OAuth 授權嚮導） |
| 11.2 | WeComProvider ✅ | 企業微信整合（WXBizMsgCrypt 加解密、範本卡片權限互動） |
| 11.3 | DingTalkProvider ✅ | 釘釘整合（Stream WebSocket / HTTP 雙模式、互動卡片） |
| 11.4 | ~~IMManager 增強~~ ✅ 已完成 | 多實例設定架構（IMPlatforms 列表、獨立啟停）、AggregateIMProvider 聚合路由、跨平台權限詢問競速 |
| 11.5 | WhatsAppProvider | WhatsApp Business API 整合（計劃中） |
| 11.6 | TelegramProvider | Telegram Bot API 整合，支援內聯鍵盤（計劃中） |

**交付物**：使用者可以透過外部即時通訊平台（飛書 / 企業微信 / 釘釘）與矽基生命體互動，多平台可同時啟用。

---

## 階段 11.5：技能系統與 MCP 整合

**目標**：可重用能力抽象層與外部工具生態接入。

| # | 模組 | 描述 |
|---|--------|-------------|
| 11.5.1 | ~~技能系統~~ ✅ 已完成 | 工具編排 + 提示詞範本的複用抽象層（SkillManager、雙觸發模式、熱重載、版本歸檔、AI 元資料補全） |
| 11.5.2 | ~~MCP 整合~~ ✅ 已完成 | 外部 MCP 伺服器工具接入（stdio/http 雙傳輸、`mcp_{serverId}_{toolName}` 命名注入、Web 管理頁面、權限矩陣整合） |

**交付物**：技能管理頁面（/skill）、MCP 管理頁面（/mcp）、`skill` 與 `mcp` 內建工具、技能/MCP 幫助文件。

---

## 階段 12：進階功能

**目標**：用於增強功能的可選進階功能。

| # | 模組 | 描述 |
|---|--------|-------------|
| 12.1 | ~~知識網絡~~ ✅ 已完成 | 三元結構（主謂賓）的知識圖譜，支援增刪改查、路徑發現、進階查詢和圖譜遍歷 |
| 12.2 | ~~外掛程式系統~~ ✅ 已完成 | 外部外掛程式載入，帶安全檢查和沙箱（IPlugin 介面、PluginLoader、AssemblyLoadContext 隔離） |
| 12.3 | 技能生態系統 | 可重用技能市場，用於生命體能力（技能系統核心已實作，見階段 11.5；剩餘：外掛程式市場、技能包分發） |
