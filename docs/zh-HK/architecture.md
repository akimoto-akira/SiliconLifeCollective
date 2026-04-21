# 架構

[English](architecture.md) | [中文](docs/zh-CN/architecture.md) | [繁體中文](docs/zh-HK/architecture.md) | [日本語](docs/ja-JP/architecture.md) | [한국어](docs/ko-KR/architecture.md)

## 核心概念

### 硅基生命体

系統中的每個 AI 智能体都是一個**硅基生命体** —— 一個具有自身身份、個性和能力的自主实体。每個硅基生命体都由一個**靈魂檔案**（Markdown 提示词）驅動程式，定義其行為模式。

### 硅基主理人

**硅基主理人**是一個具有最高系統權限的特殊硅基生命体。它充當系統管理員：

- 建立和管理其他硅基生命体
- 分析使用者要求並将其分解為工作
- 将工作分發给适當的硅基生命体
- 監控執行质量並處理失敗
- 使用**優先排程**回應使用者訊息（見下文）

### 靈魂檔案

儲存在每個硅基生命体資料目錄中的 Markdown 檔案（`soul.md`）。它作為系統提示词注入到每個 AI 要求中，定義生命体的個性、決策模式和行為約束。

---

## 排程：时隙公平排程

### 主循环 + 时钟物件

系統在專用後臺线程上執行一個**时钟驅動程式的主循环**：

```
主循环（專用线程，看門狗 + 熔断器）
  └── 时钟物件 A（優先級=0，间隔=100ms）
  └── 时钟物件 B（優先級=1，间隔=500ms）
  └── 硅基生命体管理器（由主循环直接时钟触發）
        └── 硅基生命体執行器 → 硅基生命体 1 → 时钟触發 → 執行一轮
        └── 硅基生命体執行器 → 硅基生命体 2 → 时钟触發 → 執行一轮
        └── 硅基生命体執行器 → 硅基生命体 3 → 时钟触發 → 執行一轮
        └── ...
```

關键設計決策：

- **硅基生命体不继承时钟物件。** 它們有自己的 `Tick()` 方法，由 `SiliconBeingManager` 通過 `SiliconBeingRunner` 调用，而不是直接註冊到主循环。
- **硅基生命体管理器**由主循环直接时钟触發，並作為所有生命体的单一代理。
- **硅基生命体執行器**在臨時綫程上包裝每個生命体的 `Tick()`，具有超時和每個生命体的斷路器（連续 3 次超時 → 1 分钟冷卻）。
- 每個生命体的執行限制為每次时钟触發**一轮** AI 要求 + 工具调用，确保没有生命体可以垄断主循环。
- **效能監控器**跟踪时钟執行時間以實現可觀察性。

### 主理人優先回應

當使用者向硅基主理人發送訊息时：

1. 當前生命体（例如生命体 A）完成其當前轮次 —— **不中断**。
2. 管理器**跳過剩餘隊列**。
3. 循环**從主理人重新開始**，使其立即執行。

这确保了回應使用者交互，同时不幹擾進行中的工作。

---

## 元件架構

```
┌─────────────────────────────────────────────────────────┐
│                        核心主機                          │
│  （统一主機 —— 装配和管理所有元件）                        │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ 主循环    │  │ 服務定位器    │  │      設定         │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │           硅基生命体管理器（时钟物件）               │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │主理人    │ │生命体 A  │ │生命体 B  │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              共享服務                              │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │聊天系統   │  │ 儲存     │  │  權限管理器       │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │ AI 客戶端 │  │執行器     │  │   工具管理器      │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  執行器                            │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │  磁碟     │  │ 網路     │  │  命令行          │  │   │
│  │  │執行器     │  │執行器     │  │  執行器          │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              即时通訊提供者                        │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ 控制臺    │  │  Web     │  │  飛書 / ...      │  │   │
│  │  │提供者     │  │提供者     │  │  提供者          │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## 服務定位器

`ServiceLocator` 是一個线程安全的单例註冊表，提供對所有核心服務的訪問：

| 屬性 | 類型 | 描述 |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | 中央聊天會话管理器 |
| `IMManager` | `IMManager` | 即时通訊提供者路由器 |
| `AuditLogger` | `AuditLogger` | 權限稽核跟踪 |
| `GlobalAcl` | `GlobalACL` | 全局訪問控制列表 |
| `BeingFactory` | `ISiliconBeingFactory` | 建立生命体的工廠 |
| `BeingManager` | `SiliconBeingManager` | 活動生命体生命週期管理器 |
| `DynamicBeingLoader` | `DynamicBeingLoader` | 動态編譯載入器 |
| `TokenUsageAudit` | `ITokenUsageAudit` | Token 使用跟踪 |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Token 使用報告 |

它還維護每個生命体的 `PermissionManager` 註冊表，以生命体 GUID 為键。

---

## 聊天系統

### 會话類型

聊天系統通過 `SessionBase` 支援三种會话類型：

| 類型 | 類別 | 描述 |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | 两個参與者之间的一對一對话 |
| `GroupChat` | `GroupChatSession` | 多参與者群聊 |
| `Broadcast` | `BroadcastChannel` | 具有固定 ID 的開放频道；生命体動态訂阅，僅在訂阅後接收訊息 |

### 廣播频道

`BroadcastChannel` 是一种特殊的會话類型，用於系統范围的公告：

- **固定频道 ID** —— 與 `SingleChatSession` 和 `GroupChatSession` 不同，频道 ID 是眾所周知的常量，而不是從成员 GUID 派生。
- **動态訂阅** —— 生命体在執行时訂阅/取消訂阅；它們只接收訂阅後發佈的訊息。
- **待處理訊息過濾** —— `GetPendingMessages()` 僅返回在生命体訂阅時間之後發佈且尚未读取的訊息。
- **由聊天系統管理** —— `GetOrCreateBroadcastChannel()`、`Broadcast()`、`GetPendingBroadcasts()`。

### 聊天訊息

`ChatMessage` 模型包含 AI 對话上下文和 token 跟踪的字段：

| 字段 | 類型 | 描述 |
|-------|------|-------------|
| `Id` | `Guid` | 唯一訊息标识符 |
| `SenderId` | `Guid` | 發送者的唯一标识符 |
| `ChannelId` | `Guid` | 频道/對话标识符 |
| `Content` | `string` | 訊息內容 |
| `Timestamp` | `DateTime` | 訊息發送時間 |
| `Type` | `MessageType` | 文本、圖片、檔案或系統通知 |
| `ReadBy` | `List<Guid>` | 已阅读此訊息的参與者 ID |
| `Role` | `MessageRole` | AI 對话角色（使用者、助手、工具） |
| `ToolCallId` | `string?` | 工具結果訊息的工具调用 ID |
| `ToolCallsJson` | `string?` | 助手訊息的序列化工具调用 JSON |
| `Thinking` | `string?` | AI 的思维链推理 |
| `PromptTokens` | `int?` | 提示词中的 token 数量（输入） |
| `CompletionTokens` | `int?` | 补全中的 token 数量（输出） |
| `TotalTokens` | `int?` | 使用的总 token 数量（输入 + 输出） |

---

## AI 客戶端系統

系統通過 `IAIClient` 介面支援多個 AI 後端：

### OllamaClient

- **類型**：本地 AI 服務
- **協定**：原生 Ollama HTTP API（`/api/chat`、`/api/generate`）
- **功能**：流式傳输、工具调用、本地模型托管
- **設定**：`endpoint`、`model`、`temperature`、`maxTokens`

### DashScopeClient（阿里雲百炼）

- **類型**：雲端 AI 服務
- **協定**：相容 OpenAI 的 API（`/compatible-mode/v1/chat/completions`）
- **認證**：Bearer token（API 金鑰）
- **功能**：流式傳输、工具调用、推理內容（思维链）、多區域部署
- **支援的區域**：
  - `beijing` —— 華北2（北京）
  - `virginia` —— 美国（弗吉尼亞）
  - `singapore` —— 新加坡
  - `hongkong` —— 中国香港
  - `frankfurt` —— 德国（法蘭克福）
- **支援的模型**（通過 API 動态發現，带有回退列表）：
  - **通義千問系列**：qwen3-max、qwen3.6-plus、qwen3.6-flash、qwen-max、qwen-plus、qwen-turbo、qwen3-coder-plus
  - **推理**：qwq-plus
  - **第三方**：deepseek-v3.2、deepseek-r1、glm-5.1、kimi-k2.5、llama-4-maverick
- **設定**：`apiKey`、`region`、`model`
- **模型發現**：執行时從百炼 API 获取可用模型；網路故障时回退到精选列表

### 客戶端工廠模式

每种 AI 客戶端類型都有相应的工廠實現 `IAIClientFactory`：

- `OllamaClientFactory` —— 建立 OllamaClient 实例
- `DashScopeClientFactory` —— 建立 DashScopeClient 实例

工廠提供：
- `CreateClient(Dictionary<string, object> config)` —— 從設定实例化客戶端
- `GetConfigKeyOptions(string key, ...)` —— 返回設定键的動态選項（例如可用模型、區域）
- `GetDisplayName()` —— 客戶端類型的在地化顯示名称

### AI平台支援清單

#### 狀態說明
- ✅ 已實現
- 🚧 開發中
- 📋 計劃中
- 💡 考慮中

*註：受開發者所在網絡環境影響，對接[考慮中]的海外雲端AI服務可能需要借助網絡代理工具進行訪問，調試過程可能存在不穩定性。*

#### 平台列表

| 平台 | 狀態 | 類型 | 說明 |
|------|------|------|------|
| Ollama | ✅ | 本地 | 本地AI服務，支援本地模型部署 |
| DashScope（阿里雲百煉） | ✅ | 雲端 | 阿里雲百煉AI服務，支援多區域部署 |
| 百度千帆（文心一言） | 📋 | 雲端 | 百度文心一言AI服務 |
| 智普AI（GLM） | 📋 | 雲端 | 智譜清言AI服務 |
| 月之暗面（Kimi） | 📋 | 雲端 | 月之暗面Kimi AI服務 |
| 火山方舟引擎.豆包 | 📋 | 雲端 | 字節跳動豆包AI服務 |
| DeepSeek（直連） | 📋 | 雲端 | 深度求索AI服務 |
| 零一萬物 | 📋 | 雲端 | 零一萬物AI服務 |
| 騰訊混元 | 📋 | 雲端 | 騰訊混元AI服務 |
| 矽基流動 | 📋 | 雲端 | 矽基流動AI服務 |
| MiniMax | 📋 | 雲端 | MiniMax AI服務 |
| OpenAI | 💡 | 雲端 | OpenAI API服務（GPT系列） |
| Anthropic | 💡 | 雲端 | Anthropic Claude AI服務 |
| Google DeepMind | 💡 | 雲端 | Google Gemini AI服務 |
| Mistral AI | 💡 | 雲端 | Mistral AI服務 |
| Groq | 💡 | 雲端 | Groq高速AI推理服務 |
| Together AI | 💡 | 雲端 | Together AI開源模型服務 |
| xAI | 💡 | 雲端 | xAI Grok服務 |
| Cohere | 💡 | 雲端 | Cohere企業級NLP服務 |
| Replicate | 💡 | 雲端 | Replicate開源模型託管平台 |
| Hugging Face | 💡 | 雲端 | Hugging Face開源AI社區和模型平台 |
| Cerebras | 💡 | 雲端 | Cerebras AI推理優化服務 |
| Databricks | 💡 | 雲端 | Databricks企業AI平台（MosaicML） |
| Perplexity AI | 💡 | 雲端 | Perplexity AI搜索問答服務 |
| NVIDIA NIM | 💡 | 雲端 | NVIDIA AI推理微服務 |

---

## 關键設計決策

### 儲存作為实例類別（而非静态）

`IStorage` 被設計為可注入的实例，而不是静态工具。这确保：

- 直接檔案系統訪問 —— IStorage 是系統的內部持久化通道，**不**通過執行器路由。
- **AI 无法控制 IStorage** —— 執行器管理 AI 工具發起的 IO；IStorage 管理架構自身的內部資料读寫。这些是根本不同的追蹤点。
- 可使用模擬實現進行測試。
- 未來支援不同的儲存後端，无需修改消费者。

### 執行器作為安全邊界

執行器是 I/O 操作的**唯一**路徑。需要磁碟、網路或命令行訪問的工具**必須**通過執行器。此設計强制執行：

- 每個執行器拥有**独立的排程线程**，带有用於權限驗證的线程锁定。
- 集中式權限檢查 —— 執行器查詢生命体的**私有權限管理器**。
- 支援優先級和超时控制的要求隊列。
- 所有外部操作的稽核記錄。
- 例外隔离 —— 一個執行器的失敗不影響其他執行器。
- 熔断器 —— 連续失敗暂时停止執行器以防止級联失敗。

### ContextManager 作為轻量級物件

每次 `ExecuteOneRound()` 建立一個新的 `ContextManager` 实例：

1. 載入靈魂檔案 + 最近的聊天歷史。
2. 将要求發送到 AI 客戶端。
3. 循环處理工具调用，直到 AI 返回纯文本。
4. 将回應持久化到聊天系統。
5. 释放。

这使每轮保持隔离和无狀態。

### 通過類別重寫實現自我進化

硅基生命体可以在執行时重寫自己的 C# 類別：

1. AI 生成新類別程式碼（必須继承 `SiliconBeingBase`）。
2. **編譯时引用控制**（主要防御）：編譯器只获得允許的装配列表 —— `System.IO`、`System.Reflection` 等被排除，因此危險程式碼在類型級别是不可能的。
3. **執行时静态分析**（次要防御）：`SecurityScanner` 在成功編譯後掃描程式碼中的危險模式。
4. Roslyn 在記憶體中編譯程式碼。
5. 成功时：`SiliconBeingManager.ReplaceBeing()` 交换當前实例，遷移狀態，並将加密程式碼持久化到磁碟。
6. 失敗时：丟弃新程式碼，保留现有實現。

自定義 `IPermissionCallback` 實現也可以通過 `ReplacePermissionCallback()` 編譯和注入，允許生命体自定義自己的權限逻辑。

程式碼在磁碟上以 AES-256 加密儲存。加密金鑰從生命体的 GUID（大寫）通過 PBKDF2 派生。

---

## Token 使用稽核

`TokenUsageAuditManager` 跟踪所有生命体的 AI token 消耗：

- `TokenUsageRecord` —— 每次要求的記录（生命体 ID、模型、提示词 token、补全 token、時間戳）
- `TokenUsageSummary` —— 聚合統計
- `TokenUsageQuery` —— 用於過濾記录的查詢參數
- 通過 `ITimeStorage` 持久化以進行時間序列查詢
- 可通過 Web UI（AuditController）和 `TokenAuditTool`（僅主理人）訪問

---

## 日歷系統

系統包含 **32 种日歷實現**，派生自抽象 `CalendarBase` 類別，涵盖世界主要日歷系統：

| 日歷 | ID | 描述 |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | 佛歷（BE），年份 + 543 |
| CherokeeCalendar | `cherokee` | 切罗基日歷系統 |
| ChineseLunarCalendar | `lunar` | 中国農歷，带闰月 |
| ChulaSakaratCalendar | `chula_sakarat` | 朱拉萨卡拉特歷（CS），年份 - 638 |
| CopticCalendar | `coptic` | 科普特歷 |
| DaiCalendar | `dai` | 傣歷，带完整農歷計算 |
| DehongDaiCalendar | `dehong_dai` | 德宏傣歷变体 |
| EthiopianCalendar | `ethiopian` | 埃塞俄比亞歷 |
| FrenchRepublicanCalendar | `french_republican` | 法国共和歷 |
| GregorianCalendar | `gregorian` | 標準公歷 |
| HebrewCalendar | `hebrew` | 希伯來（犹太）歷 |
| IndianCalendar | `indian` | 印度国歷 |
| InuitCalendar | `inuit` | 因纽特日歷系統 |
| IslamicCalendar | `islamic` | 伊斯蘭回歷 |
| JapaneseCalendar | `japanese` | 日本年號（Nengo）歷 |
| JavaneseCalendar | `javanese` | 爪哇伊斯蘭歷 |
| JucheCalendar | `juche` | 主体歷（朝鮮），年份 - 1911 |
| JulianCalendar | `julian` | 儒略歷 |
| KhmerCalendar | `khmer` | 高棉歷 |
| MayanCalendar | `mayan` | 瑪雅長計歷 |
| MongolianCalendar | `mongolian` | 蒙古歷 |
| PersianCalendar | `persian` | 波斯（太陽回歷）歷 |
| RepublicOfChinaCalendar | `roc` | 中華民国（民国）歷，年份 - 1911 |
| RomanCalendar | `roman` | 罗馬歷 |
| SakaCalendar | `saka` | 萨卡歷（印度尼西亞） |
| SexagenaryCalendar | `sexagenary` | 中国幹支歷（Ganzhi） |
| TibetanCalendar | `tibetan` | 藏歷 |
| VietnameseCalendar | `vietnamese` | 越南農歷（猫生肖变体） |
| VikramSamvatCalendar | `vikram_samvat` | 维克拉姆桑巴特歷 |
| YiCalendar | `yi` | 彝歷系統 |
| ZoroastrianCalendar | `zoroastrian` | 祆歷 |

`CalendarTool` 提供操作：`now`、`format`、`add_days`、`diff`、`list_calendars`、`get_components`、`get_now_components`、`convert`（跨日歷日期转换）。

---

## Web UI 架構

### 皮肤系統

Web UI 具有**可插拔的皮肤系統**，允許完整的 UI 定制，无需更改應用程式程式逻辑：

- **ISkin 介面** —— 定義所有皮肤的契約，包括：
  - 核心渲染方法（`RenderHtml`、`RenderError`）
  - 20+ UI 元件方法（按钮、输入、卡片、表格、徽章、氣泡、進度、標籤等）
  - 通過 `CssBuilder` 生成主題 CSS
  - `SkinPreviewInfo` —— 初始化頁面皮肤选择器的调色板和图标

- **內置皮肤** —— 4 种生產就绪的皮肤：
  - **Admin** —— 專業、資料聚焦的系統管理界面
  - **Chat** —— 對话式、以訊息為中心的設計，用於 AI 交互
  - **Creative** —— 藝術性、视觉豐富的創意工作流程布局
  - **Dev** —— 以開發者為中心、以程式碼為中心的界面，带语法高亮

- **皮肤發現** —— `SkinManager` 通過反射自動發現和註冊所有 `ISkin` 實現

### HTML / CSS / JS 构建器

Web UI 完全避免模板檔案，在 C# 中生成所有标記：

- **`H`** —— 流式 HTML 构建器 DSL，用於在程式碼中构建 HTML 树
- **`CssBuilder`** —— CSS 构建器，支援选择器和媒体查詢
- **`JsBuilder`（`JsSyntax`）** —— JavaScript 构建器，用於內联脚本

### 控制器系統

Web UI 遵循**類別 MVC 模式**，17 個控制器處理不同方面：

| 控制器 | 用途 |
|------------|---------|
| About | 關於頁面和項目資訊 |
| Audit | Token 使用稽核儀表板，带趋势图和導出 |
| Being | 硅基生命体管理和狀態 |
| Chat | 带 SSE 的实时聊天界面 |
| CodeBrowser | 程式碼查看和编辑 |
| Config | 系統設定管理 |
| Dashboard | 系統概览和指标 |
| Executor | 執行器狀態和管理 |
| Init | 首次執行初始化向導 |
| Knowledge | 知识图谱可视化（占位符） |
| Log | 系統記錄查看器 |
| Memory | 長期記憶瀏覽器 |
| Permission | 權限管理 |
| PermissionRequest | 權限要求隊列 |
| Project | 項目管理（占位符） |
| Task | 工作系統界面 |
| Timer | 定时器系統管理 |

### 实时更新

- **SSE（伺服器發送事件）** —— 通過 `SSEHandler` 推送聊天訊息、生命体狀態和系統事件的更新
- **无需 WebSocket** —— 使用 SSE 满足大多数实时需求的更简单架構
- **自動重連** —— 客戶端重連逻辑實現弹性連接

### 在地化

內置三种语言环境：`ZhCN`（简体中文）、`ZhHK`（繁体中文）和 `EnUS`（英文）。通過 `DefaultConfigData.Language` 选择活動语言环境，並通過 `LocalizationManager` 解析。

---

## 資料目錄結構

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # 主理人的靈魂檔案
    │   ├── state.json       # 執行时狀態
    │   ├── code.enc         # AES 加密的自定義類別程式碼
    │   └── permission.enc   # AES 加密的自定義權限回调
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```
