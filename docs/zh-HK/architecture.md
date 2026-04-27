# 架構

> **版本：v0.1.0-alpha**

[English](../en/architecture.md) | [中文](../zh-CN/architecture.md) | **繁體中文** | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Čeština](../cs-CZ/architecture.md)

## 核心概念

### 矽基生命體

系統中的每個 AI 智能體都是一個**矽基生命體** —— 一個具有自身身份、個性和能力的自主實體。每個矽基生命體都由一個**靈魂文件**（Markdown 提示詞）驅動，定義其行為模式。

### 矽基主理人

**矽基主理人**是一個具有最高系統權限的特殊矽基生命體。它充當系統管理員：

- 建立和管理其他矽基生命體
- 分析使用者請求並將其分解為任務
- 將任務分派給適當的矽基生命體
- 監控執行品質並處理失敗
- 使用**優先調度**響應使用者訊息（見下文）

### 靈魂文件

儲存在每個矽基生命體資料目錄中的 Markdown 文件（`soul.md`）。它作為系統提示詞注入到每個 AI 請求中，定義生命體的個性、決策模式和行為約束。

---

## 調度：時隙公平調度

### 主循環 + 時鐘物件

系統在專用後台執行緒上運行一個**時鐘驅動的主循環**：

```
主循環（專用執行緒，看門狗 + 熔斷器）
  └── 時鐘物件 A（優先順序=0，間隔=100ms）
  └── 時鐘物件 B（優先順序=1，間隔=500ms）
  └── 矽基生命體管理器（由主循環直接時鐘觸發）
        └── 矽基生命體執行器 → 矽基生命體 1 → 時鐘觸發 → 執行一輪
        └── 矽基生命體執行器 → 矽基生命體 2 → 時鐘觸發 → 執行一輪
        └── 矽基生命體執行器 → 矽基生命體 3 → 時鐘觸發 → 執行一輪
        └── ...
```

關鍵設計決策：

- **矽基生命體不繼承時鐘物件。** 它們有自己的 `Tick()` 方法，由 `SiliconBeingManager` 通過 `SiliconBeingRunner` 調用，而不是直接註冊到主循環。
- **矽基生命體管理器**由主循環直接時鐘觸發，並作為所有生命體的單一代理。
- **矽基生命體執行器**在臨時執行緒上包裝每個生命體的 `Tick()`，具有超時和每個生命體的熔斷器（連續 3 次超時 → 1 分鐘冷卻）。
- 每個生命體的執行限制為每次時鐘觸發**一輪** AI 請求 + 工具調用，確保沒有生命體可以壟斷主循環。
- **效能監控器**跟蹤時鐘執行時間以實現可觀察性。

### 主理人優先響應

當使用者向矽基主理人發送訊息時：

1. 當前生命體（例如生命體 A）完成其當前輪次 —— **不干擾**。
2. 管理器**跳過剩餘佇列**。
3. 循環**從主理人重新開始**，使其立即執行。

這確保了響應用者互動，同時不干擾進行中的任務。

---

## 元件架構

```
┌─────────────────────────────────────────────────────────┐
│                        核心主機                          │
│  （統一主機 —— 裝配和管理所有元件）                        │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ 主循環    │  │ 服務定位器    │  │      設定         │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │           矽基生命體管理器（時鐘物件）               │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │主理人      │ │生命體 A  │ │生命體 B  │  ...       │   │
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
│  │  │  磁碟     │  │ 網路     │  │  命令列          │  │   │
│  │  │執行器     │  │執行器     │  │  執行器          │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              即時通訊提供者                        │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ 控制台    │  │  Web     │  │  飛書 / ...      │  │   │
│  │  │提供者     │  │提供者     │  │  提供者          │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## 服務定位器

`ServiceLocator` 是一個執行緒安全的單例註冊表，提供對所有核心服務的存取：

| 屬性 | 類型 | 描述 |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | 中央聊天會話管理器 |
| `IMManager` | `IMManager` | 即時通訊提供者路由器 |
| `AuditLogger` | `AuditLogger` | 權限審計追蹤 |
| `GlobalAcl` | `GlobalACL` | 全域存取控制清單 |
| `BeingFactory` | `ISiliconBeingFactory` | 建立生命體的工廠 |
| `BeingManager` | `SiliconBeingManager` | 活動生命體生命週期管理器 |
| `DynamicBeingLoader` | `DynamicBeingLoader` | 動態編譯載入器 |
| `TokenUsageAudit` | `ITokenUsageAudit` | Token 使用跟蹤 |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Token 使用報告 |

它還維護每個生命體的 `PermissionManager` 註冊表，以生命體 GUID 為鍵。

---

## 聊天系統

### 會話類型

聊天系統通過 `SessionBase` 支援三種類話類型：

| 類型 | 類別 | 描述 |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | 兩個參與者之間的一對一對話 |
| `GroupChat` | `GroupChatSession` | 多參與者群聊 |
| `Broadcast` | `BroadcastChannel` | 具有固定 ID 的開放頻道；生命體動態訂閱，僅在訂閱後接收訊息 |

### 廣播頻道

`BroadcastChannel` 是一種特殊的會話類型，用於系統範圍的宣告：

- **固定頻道 ID** —— 與 `SingleChatSession` 和 `GroupChatSession` 不同，頻道 ID 是眾所周知的常量，而不是從成員 GUID 派生。
- **動態訂閱** —— 生命體在執行時訂閱/取消訂閱；它們只接收訂閱後發布的訊息。
- **待處理訊息過濾** —— `GetPendingMessages()` 僅返回在生命體訂閱時間之後發布且尚未讀取的訊息。
- **由聊天系統管理** —— `GetOrCreateBroadcastChannel()`、`Broadcast()`、`GetPendingBroadcasts()`。

### 聊天訊息

`ChatMessage` 模型包含 AI 對話上下文和 token 跟蹤的欄位：

| 欄位 | 類型 | 描述 |
|-------|------|-------------|
| `Id` | `Guid` | 唯一訊息標識符 |
| `SenderId` | `Guid` | 發送者的唯一標識符 |
| `ChannelId` | `Guid` | 頻道/對話標識符 |
| `Content` | `string` | 訊息內容 |
| `Timestamp` | `DateTime` | 訊息發送時間 |
| `Type` | `MessageType` | 文字、圖片、文件或系統通知 |
| `ReadBy` | `List<Guid>` | 已閱讀此訊息的參與者 ID |
| `Role` | `MessageRole` | AI 對話角色（使用者、助手、工具） |
| `ToolCallId` | `string?` | 工具結果訊息的工具調用 ID |
| `ToolCallsJson` | `string?` | 助手訊息的序列化工具調用 JSON |
| `Thinking` | `string?` | AI 的思維鏈推理 |
| `PromptTokens` | `int?` | 提示詞中的 token 數量（輸入） |
| `CompletionTokens` | `int?` | 補完中的 token 數量（輸出） |
| `TotalTokens` | `int?` | 使用的總 token 數量（輸入 + 輸出） |
| `FileMetadata` | `FileMetadata?` | 附加的文件後設資料（如果訊息包含文件） |

### 聊天訊息佇列

`ChatMessageQueue` 是一個執行緒安全的訊息佇列系統，用於管理聊天訊息的非同步處理：

- **執行緒安全** - 使用鎖定機制確保並行存取安全
- **非同步處理** - 支援非同步訊息入隊和出隊
- **訊息排序** - 保持訊息的時間順序
- **批次操作** - 支援批次獲取訊息

### 文件後設資料

`FileMetadata` 用於管理附加到聊天訊息的文件資訊：

- **文件資訊** - 檔案名、大小、類型、路徑
- **上傳時間** - 文件上傳的時間戳
- **上傳者** - 上傳文件的使用者或矽基生命體 ID

### 串流取消管理器

`StreamCancellationManager` 提供 AI 串流響應的取消機制：

- **串流控制** - 支援取消正在進行的 AI 串流響應
- **資源清理** - 取消時正確清理相關資源
- **並行安全** - 支援多個串流同時管理

### 聊天歷史檢視

新增的聊天歷史檢視功能允許使用者瀏覽矽基生命體的歷史對話：

- **會話列表** - 顯示所有歷史會話
- **訊息詳情** - 檢視完整訊息歷史
- **時間線檢視** - 按時間順序展示訊息
- **API 支援** - 提供 RESTful API 獲取會話和訊息資料

---

## AI 客戶端系統

系統通過 `IAIClient` 介面支援多個 AI 後端：

### OllamaClient

- **類型**：本地 AI 服務
- **協議**：原生 Ollama HTTP API（`/api/chat`、`/api/generate`）
- **功能**：串流傳輸、工具調用、本地模型託管
- **設定**：`endpoint`、`model`、`temperature`、`maxTokens`

### DashScopeClient（阿里雲百煉）

- **類型**：雲端 AI 服務
- **協議**：相容 OpenAI 的 API（`/compatible-mode/v1/chat/completions`）
- **認證**：Bearer token（API 金鑰）
- **功能**：串流傳輸、工具調用、推理內容（思維鏈）、多區域部署
- **支援的區域**：
  - `beijing` —— 華北2（北京）
  - `virginia` —— 美國（維吉尼亞）
  - `singapore` —— 新加坡
  - `hongkong` —— 中國香港
  - `frankfurt` —— 德國（法蘭克福）
- **支援的模型**（通過 API 動態發現，帶有回退列表）：
  - **通義千問系列**：qwen3-max、qwen3.6-plus、qwen3.6-flash、qwen-max、qwen-plus、qwen-turbo、qwen3-coder-plus
  - **推理**：qwq-plus
  - **第三方**：deepseek-v3.2、deepseek-r1、glm-5.1、kimi-k2.5、llama-4-maverick
- **設定**：`apiKey`、`region`、`model`
- **模型發現**：執行時從百煉 API 獲取可用模型；網路故障時回退到精選列表

### 客戶端工廠模式

每種 AI 客戶端類型都有相應的工廠實現 `IAIClientFactory`：

- `OllamaClientFactory` —— 建立 OllamaClient 實例
- `DashScopeClientFactory` —— 建立 DashScopeClient 實例

工廠提供：
- `CreateClient(Dictionary<string, object> config)` —— 從設定實例化客戶端
- `GetConfigKeyOptions(string key, ...)` —— 返回設定鍵的動態選項（例如可用模型、區域）
- `GetDisplayName()` —— 客戶端類型的本地化顯示名稱

### AI平台支援清單

#### 狀態說明
- ✅ 已實現
- 🚧 開發中
- 📋 計劃中
- 💡 考慮中

*註：受開發者所在網路環境影響，對接[考慮中]的海外雲端AI服務可能需要藉助網路代理工具進行存取，除錯過程可能存在不穩定性。*

#### 平台列表

| 平台 | 狀態 | 類型 | 說明 |
|------|------|------|------|
| Ollama | ✅ | 本地 | 本地AI服務，支援本地模型部署 |
| DashScope（阿里雲百煉） | ✅ | 雲端 | 阿里雲百煉AI服務，支援多區域部署 |
| 百度千帆（文心一言） | 📋 | 雲端 | 百度文心一言AI服務 |
| 智普AI（GLM） | 📋 | 雲端 | 智譜清言AI服務 |
| 月之暗面（Kimi） | 📋 | 雲端 | 月之暗面Kimi AI服務 |
| 火山方舟引擎.豆包 | 📋 | 雲端 | 位元跳動豆包AI服務 |
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
| Hugging Face | 💡 | 雲端 | Hugging Face開源AI社群和模型平台 |
| Cerebras | 💡 | 雲端 | Cerebras AI推理優化服務 |
| Databricks | 💡 | 雲端 | Databricks企業AI平台（MosaicML） |
| Perplexity AI | 💡 | 雲端 | Perplexity AI搜尋問答服務 |
| NVIDIA NIM | 💡 | 雲端 | NVIDIA AI推理微服務 |

---

## 關鍵設計決策

### 儲存作為實例類別（而非靜態）

`IStorage` 被設計為可注入的實例，而不是靜態工具。這確保：

- 直接檔案系統存取 —— IStorage 是系統的內部持久化通道，**不**通過執行器路由。
- **AI 無法控制 IStorage** —— 執行器管理 AI 工具發起的 IO；IStorage 管理框架自身的內部資料讀寫。這些是根本不同的關注點。
- 可使用模擬實現進行測試。
- 未來支援不同的儲存後端，無需修改消費者。

### 執行器作為安全邊界

執行器是 I/O 操作的**唯一**路徑。需要磁碟、網路或命令列存取的工具**必須**通過執行器。此設計強制執行：

- 每個執行器擁有**獨立的調度執行緒**，帶有用於權限驗證的執行緒鎖定。
- 集中式權限檢查 —— 執行器查詢生命體的**私有權限管理器**。
- 支援優先順序和超時控制的請求佇列。
- 所有外部操作的審計日誌。
- 異常隔離 —— 一個執行器的失敗不影響其他執行器。
- 熔斷器 —— 連續失敗暫時停止執行器以防止級聯失敗。

### ContextManager 作為輕量級物件

每次 `ExecuteOneRound()` 建立一個新的 `ContextManager` 實例：

1. 載入靈魂文件 + 最近的聊天歷史。
2. 將請求發送到 AI 客戶端。
3. 循環處理工具調用，直到 AI 返回純文字。
4. 將響應持久化到聊天系統。
5. 釋放。

這使每輪保持隔離和無狀態。

### 通過類別覆寫實現自我進化

矽基生命體可以在執行時覆寫自己的 C# 類別：

1. AI 生成新類別程式碼（必須繼承 `SiliconBeingBase`）。
2. **編譯時引用控制**（主要防禦）：編譯器只獲得允許的裝配列表 —— `System.IO`、`System.Reflection` 等被排除，因此危險程式碼在類型級別是不可能的。
3. **執行時靜態分析**（次要防禦）：`SecurityScanner` 在成功編譯後掃描程式碼中的危險模式。
4. Roslyn 在記憶體中編譯程式碼。
5. 成功時：`SiliconBeingManager.ReplaceBeing()` 交換當前實例，遷移狀態，並將加密程式碼持久化到磁碟。
6. 失敗時：丟棄新程式碼，保留現有實現。

自訂 `IPermissionCallback` 實現也可以通過 `ReplacePermissionCallback()` 編譯和注入，允許生命體自訂自己的權限邏輯。

程式碼在磁碟上以 AES-256 加密儲存。加密金鑰從生命體的 GUID（大寫）通過 PBKDF2 派生。

---

## Token 使用審計

`TokenUsageAuditManager` 跟蹤所有生命體的 AI token 消耗：

- `TokenUsageRecord` —— 每次請求的記錄（生命體 ID、模型、提示詞 token、補完 token、時間戳）
- `TokenUsageSummary` —— 聚合統計
- `TokenUsageQuery` —— 用於過濾記錄的查詢參數
- 通過 `ITimeStorage` 持久化以進行時間序列查詢
- 可通過 Web UI（AuditController）和 `TokenAuditTool`（僅主理人）存取

---

### 日曆系統

系統包含 **32 種日曆實現**，派生自抽象 `CalendarBase` 類別，涵蓋世界主要日曆系統：

| 日曆 | ID | 描述 |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | 佛曆（BE），年份 + 543 |
| CherokeeCalendar | `cherokee` | 切羅基日曆系統 |
| ChineseLunarCalendar | `lunar` | 中國農曆，帶閏月 |
| ChineseHistoricalCalendar | `chinese_historical` | 中國歷史曆法，支援干支紀年和帝王年號 |
| ChulaSakaratCalendar | `chula_sakarat` | 朱拉薩卡拉特曆（CS），年份 - 638 |
| CopticCalendar | `coptic` | 科普特曆 |
| DaiCalendar | `dai` | 傣曆，帶完整農曆計算 |
| DehongDaiCalendar | `dehong_dai` | 德宏傣曆變體 |
| EthiopianCalendar | `ethiopian` | 衣索比亞曆 |
| FrenchRepublicanCalendar | `french_republican` | 法國共和曆 |
| GregorianCalendar | `gregorian` | 標準公曆 |
| HebrewCalendar | `hebrew` | 希伯來（猶太）曆 |
| IndianCalendar | `indian` | 印度國曆 |
| InuitCalendar | `inuit` | 因紐特日曆系統 |
| IslamicCalendar | `islamic` | 伊斯蘭回曆 |
| JapaneseCalendar | `japanese` | 日本年號（Nengo）曆 |
| JavaneseCalendar | `javanese` | 爪哇伊斯蘭曆 |
| JucheCalendar | `juche` | 主體曆（朝鮮），年份 - 1911 |
| JulianCalendar | `julian` | 儒略曆 |
| KhmerCalendar | `khmer` | 高棉曆 |
| MayanCalendar | `mayan` | 瑪雅長計曆 |
| MongolianCalendar | `mongolian` | 蒙古曆 |
| PersianCalendar | `persian` | 波斯（太陽回曆）曆 |
| RepublicOfChinaCalendar | `roc` | 中華民國（民國）曆，年份 - 1911 |
| RomanCalendar | `roman` | 羅馬曆 |
| SakaCalendar | `saka` | 薩卡曆（印度尼西亞） |
| SexagenaryCalendar | `sexagenary` | 中國干支曆（Ganzhi） |
| TibetanCalendar | `tibetan` | 藏曆 |
| VietnameseCalendar | `vietnamese` | 越南農曆（貓生肖變體） |
| VikramSamvatCalendar | `vikram_samvat` | 維克拉姆桑巴特曆 |
| YiCalendar | `yi` | 彝曆系統 |
| ZoroastrianCalendar | `zoroastrian` | 祆曆 |

`CalendarTool` 提供操作：`now`、`format`、`add_days`、`diff`、`list_calendars`、`get_components`、`get_now_components`、`convert`（跨日曆日期轉換）。

---

## Web UI 架構

### 皮膚系統

Web UI 具有**可插拔的皮膚系統**，允許完整的 UI 客製化，無需更改應用程式邏輯：

- **ISkin 介面** —— 定義所有皮膚的契約，包括：
  - 核心渲染方法（`RenderHtml`、`RenderError`）
  - 20+ UI 元件方法（按鈕、輸入、卡片、表格、徽章、氣泡、進度、標籤等）
  - 通過 `CssBuilder` 生成主題 CSS
  - `SkinPreviewInfo` —— 初始化頁面皮膚選擇器的調色盤和圖示

- **內建皮膚** —— 4 種生產就緒的皮膚：
  - **Admin** —— 專業、資料聚焦的系統管理介面
  - **Chat** —— 對話式、以訊息為中心的設計，用於 AI 互動
  - **Creative** —— 藝術性、視覺豐富的創意工作流程佈局
  - **Dev** —— 以開發者為中心、以程式碼為中心的介面，帶語法高亮

- **皮膚發現** —— `SkinManager` 通過反射自動發現和註冊所有 `ISkin` 實現

### HTML / CSS / JS 建構器

Web UI 完全避免範本文件，在 C# 中生成所有標記：

- **`H`** —— 流式 HTML 建構器 DSL，用於在程式碼中構建 HTML 樹
- **`CssBuilder`** —— CSS 建構器，支援選擇器和媒體查詢
- **`JsBuilder`（`JsSyntax`）** —— JavaScript 建構器，用於內嵌腳本

### 控制器系統

Web UI 遵循**類 MVC 模式**，20+ 個控制器處理不同方面：

| 控制器 | 用途 |
|------------|---------|
| About | 關於頁面和專案資訊 |
| Audit | Token 使用審計儀表板，帶趨勢圖和匯出 |
| Being | 矽基生命體管理和狀態 |
| Chat | 帶 SSE 的即時聊天介面 |
| ChatHistory | 聊天歷史檢視，支援會話列表和訊息詳情 |
| CodeBrowser | 程式碼檢視和編輯 |
| CodeHover | 程式碼懸浮提示，支援語法高亮 |
| Config | 系統設定管理 |
| Dashboard | 系統概覽和指標 |
| Executor | 執行器狀態和管理 |
| Help | 說明文件系統，多語言支援 |
| Init | 首次執行初始化嚮導 |
| Knowledge | 知識圖譜視覺化和查詢 |
| Log | 系統日誌檢視器，支援矽基生命體篩選 |
| Memory | 長期記憶瀏覽器，支援進階過濾、統計和詳情檢視 |
| Permission | 權限管理 |
| PermissionRequest | 權限請求佇列 |
| Project | 專案管理，包含工作筆記和任務系統 |
| Task | 任務系統介面 |
| Timer | 定時器系統管理，包含執行歷史 |
| WorkNote | 工作筆記管理，支援搜尋和目錄生成 |

### 即時更新

- **SSE（伺服器發送事件）** —— 通過 `SSEHandler` 推送聊天訊息、生命體狀態和系統事件的更新
- **無需 WebSocket** —— 使用 SSE 滿足大多數即時需求的更簡單架構
- **自動重連** —— 客戶端重連邏輯實現彈性連線

### 本地化

系統支援 **21 種語言變體**的全面本地化：
- **中文（6 種）**：zh-CN（簡體）、zh-HK（繁體）、zh-SG（新加坡）、zh-MO（澳門）、zh-TW（台灣）、zhMY（馬來西亞）
- **英文（10 種）**：en-US、en-GB、en-CA、en-AU、en-IN、en-SG、en-ZA、en-IE、en-NZ、en-MY
- **西班牙語（2 種）**：es-ES、es-MX
- **其他（3 種）**：ja-JP（日語）、ko-KR（韓語）、cs-CZ（捷克語）

通過 `DefaultConfigData.Language` 選擇活動語言環境，並通過 `LocalizationManager` 解析。

---

### WebView 瀏覽器自動化系統（新增）

系統整合了基於 **Playwright** 的 WebView 瀏覽器自動化功能：

- **個體隔離**：每個矽基生命體擁有獨立的瀏覽器實例、Cookie 和會話儲存，完全隔離互不干擾。
- **無頭模式**：瀏覽器運行在使用者完全不可見的無頭模式下，矽基生命體後台自主操作。
- **WebViewBrowserTool**：提供完整的瀏覽器操作能力，包括：
  - 頁面導航、點擊、輸入文字、獲取頁面內容
  - 執行 JavaScript、獲取截圖、等待元素出現
  - 瀏覽器狀態管理和資源清理
- **安全控制**：所有瀏覽器操作均需通過權限驗證鏈，防止惡意網頁存取。

### 知識網路系統（新增）

系統內建基於**三元組結構**的知識圖譜系統：

- **知識表示**：採用「主體-關係-客體」三元組結構（例如：Python-is_a-programming_language）
- **KnowledgeTool**：提供知識的全生命週期管理：
  - `add`/`query`/`update`/`delete` - 基礎 CRUD 操作
  - `search` - 全文搜尋和關鍵詞匹配
  - `get_path` - 發現兩個概念間的關聯路徑
  - `validate` - 知識完整性檢查
  - `stats` - 知識網路統計分析
- **持久化儲存**：知識三元組持久化到檔案系統，支援時間索引查詢。
- **置信度評分**：每個知識條目帶有置信度評分（0-1），支援知識的模糊匹配和排序。
- **標籤分類**：支援為知識添加標籤，便於分類和檢索。

---

## 資料目錄結構

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # 主理人的靈魂文件
    │   ├── state.json       # 執行時狀態
    │   ├── code.enc         # AES 加密的自訂類別程式碼
    │   └── permission.enc   # AES 加密的自訂權限回呼
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```
