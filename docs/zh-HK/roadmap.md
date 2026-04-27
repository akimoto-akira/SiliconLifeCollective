# 路線圖

> **版本：v0.1.0-alpha**

[English](../en/roadmap.md) | [中文](../zh-CN/roadmap.md) | **繁體中文** | [Español](../es-ES/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Čeština](../cs-CZ/roadmap.md)

## 指導原則

每個階段都以**可執行、可觀察**的系統结束。没有階段會產生"一堆基础設施卻没有可展示的東西"。

---

## ~~階段 1：可以聊天~~ ✅ 已完成

**目标**：控制臺输入 → AI 调用 → 控制臺输出。最小可驗證单元。

| # | 模組 | 描述 |
|---|--------|-------------|
| 1.1 | 解決方案和項目結構 | 建立 `SiliconLifeCollective.sln`，包含 `src/SiliconLife.Core/`（核心程式庫）和 `src/SiliconLife.Default/`（默認實現 + 入口点） |
| 1.2 | 設定（最小化） | 单例 + JSON 反序列化。读取 `config.json`。如果缺失則自動生成默認值 |
| 1.3 | 在地化（最小化） | `LocalizationBase` 抽象類別，`ZhCN` 實現。在設定中添加 `Language` |
| 1.4 | OllamaClient（最小化） | `IAIClient` 介面，HTTP 调用本地 Ollama `/api/chat`。尚无流式傳输，尚无工具调用 |
| 1.5 | 控制臺 I/O | `while(true) + Console.ReadLine()`，读取输入 → 调用 AI → 打印回應 |
| 1.6 | 版權頭 | 為所有 C# 源檔案添加 Apache 2.0 頭 |

**交付物**：與本地 Ollama 模型對话的控制臺聊天程式。

**驗證**：執行程式，输入"hello"，看到 AI 回應。

---

## ~~階段 2：有骨架~~ ✅ 已完成

**目标**：用架構結構替换"裸循环"。行為不变。

| # | 模組 | 描述 |
|---|--------|-------------|
| 2.1 | 儲存（最小化） | `IStorage` 介面（Read/Write/Exists/Delete，键值對）。`FileSystemStorage` 實現。实例類別（非静态）。直接檔案系統訪問 —— **AI 无法控制 IStorage** | [Deutsch](../de-DE/roadmap.md) |
| 2.2 | 主循环 + 时钟物件 | 无限循环，精确时钟间隔（`Stopwatch` + `Thread.Sleep`）。優先級排程 |
| 2.3 | IAIClient 標準化 | `IAIClientFactory` 介面。OllamaClient 重构以實現標準介面 |
| 2.4 | 控制臺遷移 | 将 `while(true)` 遷移到主循环驅動程式的时钟物件。行為與階段 1 相同 |

**交付物**：主循环執行时钟，控制臺聊天仍然工作。

**驗證**：註冊一個測試时钟物件，每秒打印时钟計数；控制臺聊天仍然工作。

---

## ~~階段 3：有灵魂~~ ✅ 已完成

**目标**：第一個硅基生命体在架構中存活。

| # | 模組 | 描述 |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | 抽象基類別，包含 Id、Name、ToolManager、AIClient、ChatService、Storage、PermissionService。抽象 `Tick()` 和 `ExecuteOneRound()` |
| 3.2 | 灵魂檔案載入 | `SoulFileManager`：從生命体資料目錄读取 `soul.md` |
| 3.3 | ContextManager（最小化） | 連接灵魂檔案 + 最近訊息 → 调用 AI → 获取回應。尚无工具调用，尚无持久化 |
| 3.4 | ISiliconBeingFactory | 用於建立生命体实例的工廠介面 |
| 3.5 | SiliconBeingManager（最小化） | 继承时钟物件（優先級=0）。迭代所有生命体，依次调用它們的 Tick |
| 3.6 | DefaultSiliconBeing | 標準行為實現。檢查未读訊息 → 建立 ContextManager → ExecuteOneRound → 输出 |
| 3.7 | 生命体目錄結構 | `DataDirectory/SiliconManager/{GUID}/`，包含 `soul.md` 和 `state.json` |

**交付物**：由主循环驅動程式的硅基生命体，接收控制臺输入，載入灵魂檔案，调用 AI。

**驗證**：控制臺输入 → 主循环时钟触發 → 生命体處理（带灵魂檔案指導的行為） → AI 回應。回應風格应與階段 1 不同。

---

## ~~階段 4：有記憶~~ ✅ 已完成

**目标**：對话在重啟後持久化。

| # | 模組 | 描述 |
|---|--------|-------------|
| 4.1 | ChatSystem | 频道概念（两個 GUID = 一個频道）。带持久化的訊息模型。尚无群聊 |
| 4.2 | IIMProvider + IMManager | `IIMProvider` 介面。`ConsoleProvider` 作為正式即时通訊频道。`IMManager` 路由訊息 |
| 4.3 | ContextManager 增強 | 從聊天系統拉取歷史。持久化 AI 回應。支援多轮工具调用延续 |
| 4.4 | IMessage 模型 | 聊天系統和即时通訊管理器共享的统一訊息模型 |

**交付物**：具有持久化記憶的聊天系統。

**驗證**：聊天幾轮 → 退出 → 重啟 → 問"我們聊了什麼？" → 生命体可以回答。

---

## ~~階段 5：可以行動（工具系統）~~ ✅ 已完成

**目标**：硅基生命体可以執行操作，而不僅僅是聊天。

| # | 模組 | 描述 |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | `ITool` 介面，包含 Name、Description、Execute。`ToolResult` 包含 Success、Message、Data |
| 5.2 | ToolManager | 每個生命体的实例。基於反射的工具發現。`[SiliconManagerOnly]` 屬性支援 |
| 5.3 | IAIClient：工具调用支援 | 解析 AI tool_calls。循环：執行工具 → 發送結果回去 → AI 继续 → 直到纯文本 |
| 5.4 | 執行器基類別 | 抽象基類別，具有独立排程线程、要求隊列、超时控制 |
| 5.5 | NetworkExecutor | 通過執行器進行 HTTP 要求。超时、排隊 |
| 5.6 | CommandLineExecutor | 通過執行器進行 Shell 執行。跨平台分隔符检测 |
| 5.7 | DiskExecutor | 通過執行器進行檔案操作。尚无權限檢查（階段 6） |
| 5.8–5.12 | 內置工具 | CalendarTool、SystemTool、NetworkTool、ChatTool、DiskTool |

**交付物**：硅基生命体可以调用工具執行操作。

**驗證**：問"今天星期幾" → CalendarTool 回答；問"檢查進程" → SystemTool 執行；告诉生命体给另一個生命体發訊息 → ChatTool 工作。

---

## ~~階段 6：遵守規則（權限系統）~~ ✅ 已完成

**目标**：硅基生命体未经授權无法訪問敏感資源。

| # | 模組 | 描述 |
|---|--------|-------------|
| 6.1 | PermissionManager | 每個生命体的私有实例。基於回调，三元結果（Allowed/Deny/AskUser）。查詢優先級：HighDeny → HighAllow → Callback。IsCurator 标志 |
| 6.2 | PermissionType 枚举 | NetworkAccess、CommandLine、FileAccess、Function、DataAccess |
| 6.3 | DefaultPermissionCallback | 網路白名单/黑名单、CLI 分類別、檔案路徑安全規則 |
| 6.4 | GlobalACL | 前缀匹配規則表，持久化到儲存 |
| 6.5 | UserFrequencyCache | HighAllow/HighDeny 列表。使用者选择（非自動检测）。前缀匹配，僅記憶體，可設定過期 |
| 6.6 | UserAskMechanism（控制臺） | 當返回 AskUser 时控制臺提示 y/n |
| 6.7 | 執行器權限集成 | 所有執行器在執行前檢查權限 |
| 6.8 | IStorage 隔离說明 | IStorage 是系統內部持久化 —— 直接檔案訪問，**不**通過執行器路由，**不**可由 AI 控制。執行器僅管理 AI 工具發起的 IO |
| 6.9 | 稽核記錄 | 記录所有權限決策，带時間戳、要求者、資源、結果 |

**交付物**：當生命体尝试敏感操作时出现權限提示。

**驗證**：告诉生命体刪除檔案 → 控制臺顯示權限提示 → 输入 `n` → 操作被拒絕。告诉生命体訪問白名单網站 → 立即允許。

---

## ~~階段 7：可以進化（動态編譯）~~ ✅ 已完成

**目标**：硅基生命体可以重寫自己的程式碼。

| # | 模組 | 描述 |
|---|--------|-------------|
| 7.1 | CodeEncryption | AES-256 加密/解密。從 GUID 派生 PBKDF2 金鑰 |
| 7.2 | DynamicCompilationExecutor | 基於 Roslyn 的記憶體編譯沙箱。編譯时装配引用控制（主要防御：排除 System.IO、Reflection 等） |
| 7.3 | 安全掃描 | 執行时静态分析危險程式碼模式（次要防御）。如果掃描失敗則阻止載入 |
| 7.4 | 生命体生命週期增強 | 載入：解密 → 掃描 → 編譯 → 实例化。執行时：在記憶體中編譯 → 原子替换 → 持久化加密 |
| 7.5 | SiliconCurator | 館長抽象基類別。IsCurator=true。最高權限 |
| 7.6 | DefaultCurator | 默認館長實現，带內置灵魂檔案和管理工具 |
| 7.7 | CuratorTool | `[SiliconManagerOnly]` 工具：list_beings、create_being、get_code、reset |
| 7.8 | 權限回调覆盖 | 生命体可以編譯自定義權限回调 |
| 7.9 | SiliconBeingManager 增強 | Replace 方法（執行时实例交换）。MigrateState（在舊实例和新实例之间转移狀態） |

**交付物**：硅基生命体可以通過 AI 生成新程式碼，編譯並替换自己。

**驗證**：告诉生命体"给自己添加一個新功能" → 觀察編譯 → 重啟 → 新功能工作。

---

## ~~階段 8：記憶和計畫~~ ✅ 已完成

**目标**：長期記憶、工作管理、定时触發。

| # | 模組 | 描述 |
|---|--------|-------------|
| 8.1 | FileSystemMemory | 短期/長期分段儲存。時間衰减。壓缩（合並相似記憶）。多维搜尋 |
| 8.2 | TaskSystem | 一次性 + DAG 依赖工作。優先級排程。狀態跟踪 |
| 8.3 | TimerSystem | 一次性闹钟 + 周期定时器。毫秒精度。持久化到儲存 |
| 8.4 | IncompleteDate | 模糊日期范围結構（例如"2026 年 4 月"、"2026 年春"） |
| 8.5–8.7 | 記憶/工作/定时器工具 | 生命体查詢記憶、管理工作、設定定时器的工具 |

**交付物**：生命体可以記住關键点、建立/跟踪工作、設定闹钟。

**驗證**：建立工作 → 檢查工作列表 → 設定 1 分钟闹钟 → 時間到时接收通知。

---

## ~~階段 9：架構完成~~ ✅ 已完成

**目标**：统一入口点，多生命体協作。

| # | 模組 | 描述 |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | 使用构建器模式的统一主機。優雅關閉（Ctrl+C / SIGTERM） |
| 9.2 | Program.Main 重构 | 遷移到 CoreHostBuilder 模式 |
| 9.3 | SiliconBeingManager 增強 | 館長優先回應。例外隔离。定期持久化 |
| 9.4 | 多生命体載入 | 從資料目錄載入多個生命体。通過 ChatTool 進行生命体间通信 |
| 9.5 | 效能監控 | 每個时钟物件執行時間跟踪 |
| 9.6 | ServiceLocator | 全局服務定位器，带 Register/Get 方法 |

**交付物**：多個生命体同时執行，協作，由 CoreHost 管理。

**驗證**：建立两個生命体 → A 给 B 發訊息 → B 接收並回复 → 架構排程无錯誤。當使用者訊息到達时館長優先回應。

---

## ~~階段 10：走向 Web~~ ✅ 已完成

**目标**：從控制臺遷移到瀏覽器界面。

| # | 模組 | 描述 |
|---|--------|-------------|
| 10.1 | Router | HTTP 要求路由器。序列參數路由和静态檔案服務 |
| 10.2 | Controller 基類別 | 要求/回應上下文。HTML 和 JSON 回應支援 |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | C# 伺服器端构建器。零前端架構依赖 |
| 10.6 | SSE（伺服器發送事件） | 推送式实时更新聊天、生命体狀態和系統事件。比 WebSocket 更简单，带自動客戶端重連 |
| 10.7 | WebUIProvider | 基於 SSE 的实时即时通訊频道。替换控制臺作為主要界面 |
| 10.8 | Web 安全 | IP 黑名单/白名单。`[WebCode]` 屬性。動态更新 |
| 10.9–10.17 | Web 控制器 | 聊天、儀表板、生命体、工作、權限、權限要求、執行器、記錄、設定、記憶、定时器、初始化、關於、程式碼瀏覽器、知识、項目、稽核 |

**交付物**：可從瀏覽器訪問的完整 Web UI。

**驗證**：打開瀏覽器 → 與生命体聊天 → 查看儀表板 → 管理權限 → 全部功能正常。

---

## ~~階段 10.5：增量增強~~ ✅ 已完成

**目标**：使用開發過程中發現的新功能增強现有系統。

| # | 模組 | 描述 |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | 用於系統范围公告的新會话類型。固定频道 ID、動态訂阅、待處理訊息過濾 |
| 10.5.2 | ChatMessage 增強 | ToolCallId、ToolCallsJson、Thinking 字段用於 AI 上下文；PromptTokens、CompletionTokens、TotalTokens 用於 token 跟踪；SystemNotification 訊息類型 |
| 10.5.3 | TokenUsageAuditManager | 跨所有生命体的每次要求 token 消耗跟踪。聚合統計、時間序列查詢、持久化儲存 |
| 10.5.4 | TokenAuditTool | `[SiliconManagerOnly]` 工具，供館長查詢和汇总 token 使用 |
| 10.5.5 | ConfigTool | `[SiliconManagerOnly]` 工具，供館長读取和修改系統設定 |
| 10.5.6 | AuditController | Web 儀表板用於 token 使用稽核，带趋势图和資料導出 |
| 10.5.7 | 日歷系統擴充 | 32 种日歷實現，涵盖世界日歷系統（佛歷、農歷、伊斯蘭歷、希伯來歷、日本歷、波斯歷、瑪雅歷等） |
| 10.5.8 | DiskTool 增強 | 新操作：count_lines、read_lines、clear_file、replace_lines、replace_text、replace_text_all、list_drives |
| 10.5.9 | SystemTool 增強 | 新操作：find_process（支援通配符）、resource_usage |
| 10.5.10 | CalendarTool 增強 | 新操作：diff、list_calendars、get_components、get_now_components、convert（跨日歷转换） |
| 10.5.11 | DashScopeClient | 阿里雲百炼 AI 客戶端，相容 OpenAI API。支援流式傳输、工具调用、推理內容 |
| 10.5.12 | DashScopeClientFactory | 用於建立百炼客戶端的工廠。通過 API 動态模型發現。多區域支援（北京、弗吉尼亞、新加坡、香港、法蘭克福） |
| 10.5.13 | AI 客戶端設定系統 | 每個生命体的 AI 客戶端設定。動态設定键選項（模型、區域）。在地化顯示名称 |
| 10.5.14 | 在地化擴充 | 简体中文、繁体中文、英文和日语在地化，用於百炼設定選項、模型名称和區域名称 |

**交付物**：增強的工具、可觀察性、日歷覆盖范围和多 AI 後端支援。

**驗證**：館長通過 TokenAuditTool 查詢 token 使用 → 稽核儀表板顯示趋势 → CalendarTool 在 32 种日歷系統之间转换日期 → 将 AI 後端切换到百炼 → 通過雲端 API 與通義千問模型聊天。

---

## 階段 11：外部即时通訊集成

**目标**：連接到外部訊息傳递平台，以更廣泛的使用者可訪問性。

| # | 模組 | 描述 |
|---|--------|-------------|
| 11.1 | FeishuProvider | 飛書（Lark）機器人集成，支援卡片 |
| 11.2 | WhatsAppProvider | WhatsApp Business API 集成 |
| 11.3 | TelegramProvider | Telegram Bot API 集成，支援內联键盘 |
| 11.4 | IMManager 增強 | 多提供者路由、统一訊息格式、跨平台權限询問處理 |

**交付物**：使用者可以通過外部即时通訊平台與硅基生命体交互。

---

## 階段 12：高級功能

**目标**：用於增強功能的可选高級功能。

| # | 模組 | 描述 |
|---|--------|-------------|
| 12.1 | 知识網路 | 使用三元結構（主谓宾）的共享知识图谱 |
| 12.2 | 外掛程式系統 | 外部外掛程式載入，带安全檢查和沙箱 |
| 12.3 | 技能生态系統 | 可重用技能市場，用於生命体能力 |
