# 開發路線

[English](../../roadmap.md) | [简体中文](../zh-CN/roadmap.md)

## 指導原則

每個階段結束都要能**從命令列跑起來、看到行為**。絕不做「寫完一堆基礎設施但什麼都看不到」的階段。

---

## ~~第一階段：能對話~~ ✅ 已完成

**目標**：命令列輸入 → 呼叫 AI → 命令列輸出。最小可驗證單元。

| # | 模組 | 說明 |
|---|------|------|
| 1.1 | 解決方案與專案結構 | 建立 `SiliconLifeCollective.sln`，建立 `src/SiliconLife.Core/`（核心庫）和 `src/SiliconLife.Default/`（預設實作+進入點） |
| 1.2 | Config（最小） | 單例模式 + JSON 反序列化，讀取 `config.json`。不存在時自動產生預設值 |
| 1.3 | Localization（最小） | `LocalizationBase` 抽象基底類別，`ZhCN` 實作。Config 中增加 `Language` 設定項 |
| 1.4 | OllamaClient（最小） | `IAIClient` 介面，HTTP 呼叫本機 Ollama `/api/chat`。暫不支援串流輸出和 ToolCall |
| 1.5 | 主控台 I/O | `while(true) + Console.ReadLine()`，讀取使用者輸入 → 呼叫 AI → 列印回覆 |
| 1.6 | 版權頁首 | 所有 C# 原始檔頂部新增 Apache 2.0 版權頁首 |

**階段產出**：一個主控台聊天程式，能和本機 Ollama 模型對話。

**驗證方式**：執行程式，輸入「你好」，看到 AI 回覆。

---

## ~~第二階段：有骨架~~ ✅ 已完成

**目標**：把「裸循環」替換成框架結構，行為不變。

| # | 模組 | 說明 |
|---|------|------|
| 2.1 | Storage（最小） | `IStorage` 介面（Read/Write/Exists/Delete，key-value 模式）。`FileSystemStorage` 實作。執行個體類別（非靜態）。直接操作檔案系統——**AI 無法控制 IStorage** |
| 2.2 | MainLoop + TickObject | 無限循環，精確計算 Tick 間隔（`Stopwatch` + `Thread.Sleep`）。優先順序排程 |
| 2.3 | IAIClient 介面標準化 | `IAIClientFactory` 介面。OllamaClient 改為實作標準介面 |
| 2.4 | 主控台對話遷移 | 將 `while(true)` 遷移為 MainLoop 驅動的 TickObject。行為與第一階段完全一致 |

**階段產出**：MainLoop 在跑 Tick，主控台對話仍正常運作。

**驗證方式**：註冊一個測試用 TickObject，每秒列印一次 tick 計數；主控台對話仍正常運作。

---

## ~~第三階段：有靈魂~~ ✅ 已完成

**目標**：第一個矽基人在框架內活了。

| # | 模組 | 說明 |
|---|------|------|
| 3.1 | SiliconBeingBase | 抽象基底類別，包含 Id、Name、ToolManager、AIClient、ChatService、Storage、PermissionService。抽象方法 `Tick()` 和 `ExecuteOneRound()` |
| 3.2 | 靈魂檔案載入 | `SoulFileManager`：從矽基人目錄讀取 `soul.md` 檔案 |
| 3.3 | ContextManager（最小） | 拼接靈魂檔案 + 最近訊息 → 呼叫 AI → 取得回覆。暫不處理 ToolCall 和持久化 |
| 3.4 | ISiliconBeingFactory | 工廠介面，負責建立矽基人執行個體 |
| 3.5 | SiliconBeingManager（最小） | 繼承 TickObject（Priority=0），遍歷所有矽基人，依序呼叫其 Tick |
| 3.6 | DefaultSiliconBeing | 標準行為實作。偵測未讀訊息 → 建立 ContextManager → ExecuteOneRound → 輸出回覆 |
| 3.7 | 矽基人目錄結構 | `DataDirectory/SiliconManager/{GUID}/`，包含 `soul.md` 和 `state.json` |

**階段產出**：矽基人由 MainLoop 驅動，從主控台接收輸入，載入靈魂檔案，呼叫 AI 回覆。

**驗證方式**：主控台輸入 → MainLoop Tick → 矽基人處理（含靈魂檔案引導的行為）→ AI 回覆。回覆風格應與第一階段不同。

---

## ~~第四階段：能記憶~~ ✅ 已完成

**目標**：對話有持久化，重新啟動不遺失。

| # | 模組 | 說明 |
|---|------|------|
| 4.1 | ChatSystem | 頻道概念：兩個 GUID 配對為一個頻道。訊息模型持久化。暫不實作群組聊天 |
| 4.2 | IIMProvider + IMManager | `IIMProvider` 介面。`ConsoleProvider` 作為正式 IM 通道。`IMManager` 路由訊息 |
| 4.3 | ContextManager 增強 | 從 ChatSystem 拉取歷史訊息；AI 回覆持久化；支援跨輪次 ToolCall 延續 |
| 4.4 | IMessage 模型 | 統一訊息模型，ChatSystem 和 IMManager 共用 |

**階段產出**：有記憶的對話系統。

**驗證方式**：聊幾句話 → 結束程式 → 重新啟動 → 問「我們剛才聊了什麼」，矽基人能回答。

---

## ~~第五階段：能動手（工具系統）~~ ✅ 已完成

**目標**：矽基人能執行操作，不只是說話。

| # | 模組 | 說明 |
|---|------|------|
| 5.1 | ITool + ToolResult | `ITool` 介面：Name、Description、Execute。`ToolResult`：Success、Message、Data |
| 5.2 | ToolManager | 每個矽基人獨立持有。反射式掃描組件探索工具。`[SiliconManagerOnly]` 屬性支援 |
| 5.3 | IAIClient：ToolCall 支援 | 解析 AI 返回的 tool_calls；循環執行工具並回傳結果；直到 AI 返回純文字 |
| 5.4 | Executor 基底類別 | 抽象基底類別，獨立排程執行緒，請求佇列，逾時控制 |
| 5.5 | NetworkExecutor | HTTP 請求走執行器。逾時控制、請求佇列 |
| 5.6 | CommandLineExecutor | 命令列執行走執行器。跨平台分隔符號偵測 |
| 5.7 | DiskExecutor | 檔案操作走執行器。暫不接入權限驗證 |
| 5.8–5.12 | 內建工具 | CalendarTool、SystemTool、NetworkTool、ChatTool、DiskTool |

**階段產出**：矽基人能呼叫工具執行操作。

**驗證方式**：問「今天星期幾」→ CalendarTool 回答；問「幫我查一下處理程序」→ SystemTool 執行；讓矽基人傳訊息給另一個矽基人 → ChatTool 驗證。

---

## ~~第六階段：能守規矩（權限系統）~~ ✅ 已完成

**目標**：矽基人不能隨便動東西。

| # | 模組 | 說明 |
|---|------|------|
| 6.1 | PermissionManager | 每個矽基人私有執行個體。回呼函式模式，三元結果（Allowed/Deny/AskUser）。查詢優先順序：高頻拒絕 → 高頻允許 → 回呼函式。IsCurator 標記 |
| 6.2 | PermissionType 列舉 | NetworkAccess、CommandLine、FileAccess、Function、DataAccess |
| 6.3 | DefaultPermissionCallback | 網路網域白名單/黑名單、命令列分級、檔案路徑安全規則 |
| 6.4 | GlobalACL | 前置詞比對規則表，持久化到 Storage |
| 6.5 | UserFrequencyCache | 高頻允許/拒絕清單。使用者主動選擇加入（非自動偵測）。前置詞比對，僅記憶體狀態，可設定有效期 |
| 6.6 | UserAskMechanism（主控台版） | 權限回呼返回 AskUser 時，主控台列印描述 → 等待使用者輸入 y/n |
| 6.7 | 執行器接入權限 | 所有執行器在執行前增加權限檢查 |
| 6.8 | IStorage 隔離說明 | IStorage 是系統內部持久化——直接操作檔案系統，**不經過執行器**，**AI 不可控**。執行器僅管轄 AI 透過工具發起的 IO |
| 6.9 | 權限稽核日誌 | 記錄所有權限決策（時間、請求者、資源、結果） |

**階段產出**：矽基人想操作敏感資源時，主控台彈出確認提示。

**驗證方式**：讓矽基人嘗試刪除檔案 → 主控台列印權限詢問 → 輸入 n → 操作被拒絕。讓矽基人存取白名單網站 → 直接放行。

---

## ~~第七階段：能進化（動態編譯）~~ ✅ 已完成

**目標**：矽基人能重寫自己的程式碼。

| # | 模組 | 說明 |
|---|------|------|
| 7.1 | CodeEncryption | AES-256 加密/解密。PBKDF2 從 GUID 衍生金鑰 |
| 7.2 | DynamicCompilationExecutor | 基於 Roslyn 的記憶體編譯沙箱。編譯時參考控制（主要防線：排除 System.IO、Reflection 等危險組件） |
| 7.3 | 安全掃描 | 執行時靜態分析（輔助防線）：偵測直接 IO、系統呼叫、反射濫用等危險模式 |
| 7.4 | 矽基人生命週期增強 | 載入：解密 → 掃描 → 編譯 → 執行個體化。執行時：記憶體編譯 → 原子替換 → 持久化加密程式碼 |
| 7.5 | SiliconCurator | 主理人抽象基底類別。IsCurator=true。最高權限 |
| 7.6 | DefaultCurator | 預設主理人實作。內建靈魂檔案和管理工具集 |
| 7.7 | CuratorTool | `[SiliconManagerOnly]` 工具：list_beings、create_being、get_code、reset |
| 7.8 | 權限回呼動態覆寫 | 矽基人可編譯自訂權限回呼程式碼 |
| 7.9 | SiliconBeingManager 增強 | Replace 方法（執行時替換執行個體）。MigrateState（狀態遷移） |

**階段產出**：矽基人能透過 AI 產生新程式碼，編譯後替換自身。

**驗證方式**：讓矽基人「給自己加一個新功能」→ 觀察編譯過程 → 重新啟動後新功能生效。

---

## ~~第八階段：有記憶有計畫~~ ✅ 已完成

**目標**：長期記憶、任務管理、定時觸發。

| # | 模組 | 說明 |
|---|------|------|
| 8.1 | FileSystemMemory | 短期/長期分階段儲存。時間衰減。壓縮整合。多維檢索 |
| 8.2 | TaskSystem | 一次性任務 + DAG 相依任務。優先順序排程。狀態追蹤 |
| 8.3 | TimerSystem | 一次性鬧鐘 + 週期定時。毫秒級精度。持久化到 Storage |
| 8.4 | IncompleteDate | 模糊時間範圍結構（如「2026年4月」、「2026年春天」） |
| 8.5–8.7 | 記憶/任務/計時器工具 | 矽基人可查詢記憶、管理任務、設定計時器 |

**階段產出**：矽基人能記住歷史要點、建立和追蹤任務、設定鬧鐘。

**驗證方式**：讓矽基人建立任務 → 查看任務清單 → 設定一個 1 分鐘後的鬧鐘 → 到時間收到提醒。

---

## ~~第九階段：框架完備~~ ✅ 已完成

**目標**：統一進入點，多矽基人協作。

| # | 模組 | 說明 |
|---|------|------|
| 9.1 | CoreHost + CoreHostBuilder | 統一宿主。Builder 模式註冊所有元件。優雅關閉（Ctrl+C / SIGTERM） |
| 9.2 | Program.Main 重構 | 改為 CoreHostBuilder 模式 |
| 9.3 | SiliconBeingManager 增強 | 主理人優先響應機制。例外隔離。定期持久化 |
| 9.4 | 多矽基人載入 | 從資料目錄載入多個矽基人。矽基人間透過 ChatTool 通訊 |
| 9.5 | 效能監控 | 監控每個 TickObject 的執行時間 |
| 9.6 | ServiceLocator | 全域服務定位器。Register/Get 方法 |

**階段產出**：多個矽基人同時執行，互相協作，框架透過 CoreHost 統一管理。

**驗證方式**：建立兩個矽基人 → 讓 A 給 B 傳訊息 → B 收到並回覆 → 框架正常排程無例外。

---

## ~~第十階段：上 Web~~ ✅ 已完成

**目標**：從主控台遷移到瀏覽器介面。

| # | 模組 | 說明 |
|---|------|------|
| 10.1 | Router | HTTP 請求路由器。序號參數型路由和靜態檔案服務 |
| 10.2 | Controller 抽象類別 | 請求/響應上下文。HTML 和 JSON 響應支援 |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | C# 伺服器端建構器。零前端框架依賴 |
| 10.6 | SSE（Server-Sent Events） | 基於推送的即時更新，用於聊天、矽基人狀態和系統事件。比 WebSocket 更簡單，支援用戶端自動重新連線 |
| 10.7 | WebUIProvider | 基於 SSE 的即時 IM 通道。替代主控台作為主要介面 |
| 10.8 | Web 安全 | IP 黑白名單。`[WebCode]` 屬性。動態更新 |
| 10.9–10.17 | Web 控制器 | Chat、Dashboard、Being、Task、Permission、PermissionRequest、Executor、Log、Config、Memory、Timer、Init、About、CodeBrowser、Knowledge、Project、Audit |

**階段產出**：完整的 Web 介面，可透過瀏覽器存取。

**驗證方式**：開啟瀏覽器 → 與矽基人聊天 → 查看儀表板 → 管理權限 → 全部功能正常。

---

## ~~第十點五階段：增量增強~~ ✅ 已完成

**目標**：在已有系統基礎上增加開發過程中發現的新能力。

| # | 模組 | 說明 |
|---|------|------|
| 10.5.1 | BroadcastChannel | 新工作階段類型，用於系統級廣播。固定頻道 ID，動態訂閱，待讀訊息篩選 |
| 10.5.2 | ChatMessage 增強 | ToolCallId、ToolCallsJson、Thinking 欄位用於 AI 上下文；PromptTokens、CompletionTokens、TotalTokens 用於 Token 追蹤；SystemNotification 訊息類型 |
| 10.5.3 | TokenUsageAuditManager | 跨矽基人的單次請求 Token 消耗追蹤。彙總統計、時間序列查詢、持久化儲存 |
| 10.5.4 | TokenAuditTool | `[SiliconManagerOnly]` 工具，主理人可查詢和彙總 Token 用量 |
| 10.5.5 | ConfigTool | `[SiliconManagerOnly]` 工具，主理人可讀取和修改系統設定 |
| 10.5.6 | AuditController | Token 用量稽核 Web 儀表板，含趨勢圖表和資料匯出 |
| 10.5.7 | 曆法系統擴展 | 32 種曆法實作，涵蓋世界主要曆法體系（佛曆、中國農曆、伊斯蘭曆、希伯來曆、日本年號曆、波斯曆、瑪雅曆等） |
| 10.5.8 | DiskTool 增強 | 新增操作：count_lines、read_lines、clear_file、replace_lines、replace_text、replace_text_all、list_drives |
| 10.5.9 | SystemTool 增強 | 新增操作：find_process（支援萬用字元）、resource_usage |
| 10.5.10 | CalendarTool 增強 | 新增操作：diff、list_calendars、get_components、get_now_components、convert（跨曆法轉換） |

**階段產出**：增強的工具集、可觀測性和曆法涵蓋範圍。

**驗證方式**：主理人透過 TokenAuditTool 查詢 Token 用量 → 稽核儀表板顯示趨勢 → CalendarTool 跨 32 種曆法轉換日期。

---

## 第十一階段：外接 IM

**目標**：接入外部即時通訊平台。

| # | 模組 | 說明 |
|---|------|------|
| 11.1 | FeishuProvider | 飛書機器人整合 |
| 11.2 | WhatsAppProvider | WhatsApp Business API 整合 |
| 11.3 | TelegramProvider | Telegram Bot API 整合 |
| 11.4 | IMManager 增強 | 多通道路由，統一訊息格式 |

**階段產出**：使用者可透過外部 IM 平台與矽基人互動。

---

## 第十二階段：錦上添花

**目標**：可選的進階功能。

| # | 模組 | 說明 |
|---|------|------|
| 12.1 | 知識網路 | 共用知識圖譜，三元組結構 |
| 12.2 | 外掛程式系統 | 外部外掛程式載入，安全檢查 |
| 12.3 | 技能生態系統 | 可重複使用的功能模組市場 |
