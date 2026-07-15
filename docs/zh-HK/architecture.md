# 架構

> **版本：v0.2.0-alpha**

[English](../en/architecture.md) | [Deutsch](../de-DE/architecture.md) | [中文](../zh-CN/architecture.md) | **繁體中文** | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Čeština](../cs-CZ/architecture.md) | [Русский](../ru-RU/architecture.md)

## 雙版本架構

本項目提供兩個實作版本，共享相同的架構設計，但在儲存和效能最佳化方面有所不同：

### SiliconLife.Default（預設版本）
- **定位**：預設實作，主要用於驗證架構可行性
- **執行模式**：主控台應用程式
- **儲存方式**：純檔案系統 JSON 儲存
- **適用場景**：資料安全性要求高、記憶體資源受限、資料量小的場景
- **角色說明**：作為架構驗證的基準實作，提供簡單可靠的執行方式，適合初次接觸本項目、進行開發除錯或資料安全優先的場景

### SiliconLife.Fast（高效能版本）
- **定位**：主推生產版本
- **執行模式**：桌面應用程式（Windows 系統匣 / Linux 狀態視窗）
- **儲存方式**：SpeedyPack 記憶體儲存 + 非同步批次持久化（.spk 檔案格式）
- **適用場景**：高並行、低延遲、大資料量場景
- **平台支援**：Windows/macOS（完整功能，含系統匣）、Linux（狀態視窗，無系統匣圖示）
- **特點**：
  - Windows/macOS 系統匣背景執行，系統匣狀態視窗即時監控；Linux 狀態視窗直接顯示
  - SpeedyPack 引擎 + 自動壓縮保證資料安全
  - Component UI 架構，27 個宣告式元件
  - 7 種佈景主題，支援自動探索和切換
  - Linux 自動開啟瀏覽器存取 Web UI，支援 `--no-tray` 參數
- **效能提升**：儲存讀取延遲降低 1000 倍，寫入延遲降低 15000 倍
- **角色說明**：經過深度最佳化的生產級實作，具備系統匣背景執行、SpeedyPack 引擎 + 自動壓縮等特性，是長期執行和實際生產環境的首選

> **注意**：本文檔描述的架構適用於兩個版本，僅在儲存實作部分有所不同。SiliconLife.Default 作為架構驗證基準，SiliconLife.Fast 作為生產環境主推版本。

---

## 核心概念

### 矽基生命體

系統中的每個 AI 智慧體都是一個**矽基生命體** —— 一個具有自身身份、個性和能力的自主實體。每個矽基生命體都由一個**靈魂檔案**（Markdown 提示詞）驅動，定義其行為模式。

### 矽基主理人

**矽基主理人**是一個具有最高系統權限的特殊矽基生命體。它充當系統管理員：

- 建立和管理其他矽基生命體
- 分析使用者請求並將其分解為任務
- 將任務分派給適當的矽基生命體
- 監控執行品質並處理失敗
- 使用**優先排程**回應使用者訊息（見下文）

### 靈魂檔案

儲存在每個矽基生命體資料目錄中的 Markdown 檔案（`soul.md`）。它作為系統提示詞注入到每個 AI 請求中，定義生命體的個性、決策模式和行為約束。

---

## 排程：時隙公平排程

### 主迴圈 + 時鐘物件

系統在專用背景執行緒上執行一個**時鐘驅動的主迴圈**：

```
主迴圈（專用執行緒，看門狗 + 熔斷器）
  └── 時鐘物件 A（優先級=0，間隔=100ms）
  └── 時鐘物件 B（優先級=1，間隔=500ms）
  └── 矽基生命管理器（由主迴圈直接時鐘觸發）
        └── 矽基生命體執行器 → 矽基生命體 1 → 時鐘觸發 → 執行一輪
        └── 矽基生命體執行器 → 矽基生命體 2 → 時鐘觸發 → 執行一輪
        └── 矽基生命體執行器 → 矽基生命體 3 → 時鐘觸發 → 執行一輪
        └── ...
```

關鍵設計決策：

- **矽基生命體不繼承時鐘物件。** 它們有自己的 `Tick()` 方法，由 `SiliconBeingManager` 透過 `SiliconBeingRunner` 呼叫，而不是直接註冊到主迴圈。
- **矽基生命管理器**由主迴圈直接時鐘觸發，並作為所有生命體的單一代理。
- **矽基生命體執行器**在臨時執行緒上包裝每個生命體的 `Tick()`，具有逾時和每個生命體的熔斷器（連續 3 次逾時 → 1 分鐘冷卻）。
- 每個生命體的執行限制為每次時鐘觸發**一輪** AI 請求 + 工具呼叫，確保沒有生命體可以壟斷主迴圈。
- **效能監視器**追蹤時鐘執行時間以實作可觀察性。

### 主理人優先回應

當使用者向矽基主理人發送訊息時：

1. 目前生命體（例如生命體 A）完成其目前輪次 —— **不中斷**。
2. 管理器**跳過剩餘佇列**。
3. 迴圈**從主理人重新開始**，使其立即執行。

這確保了回應使用者互動，同時不干擾進行中的任務。

---

## 元件架構

```
┌─────────────────────────────────────────────────────────┐
│                        核心主機                          │
│  （統一主機 —— 裝配和管理所有元件）                        │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ 主迴圈    │  │ 服務定位器    │  │      設定         │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │           矽基生命管理器（時鐘物件）                 │   │
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
│  │  │ AI 用戶端 │  │執行器     │  │   工具管理器      │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  │  ┌──────────┐ ┌──────────┐                        │   │
│  │  │插件載入器 │ │知識網絡   │                        │   │
│  │  └──────────┘ └──────────┘                        │   │
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
│  │  │ 主控台    │  │  Web     │  │  飛書 / ...      │  │   │
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
| `TokenUsageAudit` | `ITokenUsageAudit` | Token 使用追蹤 |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Token 使用報告 |

它還維護每個生命體的 `PermissionManager` 註冊表，以生命體 GUID 為鍵。

---

## 聊天系統

### 會話類型

聊天系統透過 `SessionBase` 支援三種會話類型：

| 類型 | 類別 | 描述 |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | 兩個參與者之間的一對一對話 |
| `GroupChat` | `GroupChatSession` | 多參與者群聊 |
| `Broadcast` | `BroadcastChannel` | 具有固定 ID 的開放頻道；生命體動態訂閱，僅在訂閱後接收訊息 |

### 廣播頻道

`BroadcastChannel` 是一種特殊的會話類型，用於系統範圍的公告：

- **固定頻道 ID** —— 與 `SingleChatSession` 和 `GroupChatSession` 不同，頻道 ID 是眾所周知的常數，而不是從成員 GUID 衍生。
- **動態訂閱** —— 生命體在執行時期訂閱/取消訂閱；它們只接收訂閱後發布的訊息。
- **待處理訊息篩選** —— `GetPendingMessages()` 僅傳回在生命體訂閱時間之後發布且尚未讀取的訊息。
- **由聊天系統管理** —— `GetOrCreateBroadcastChannel()`、`Broadcast()`、`GetPendingBroadcasts()`。

### 聊天訊息

`ChatMessage` 模型包含 AI 對話上下文和 token 追蹤的欄位：

| 欄位 | 類型 | 描述 |
|-------|------|-------------|
| `Id` | `Guid` | 唯一訊息識別碼 |
| `SenderId` | `Guid` | 發送者的唯一識別碼 |
| `ChannelId` | `Guid` | 頻道/對話識別碼 |
| `Content` | `string` | 訊息內容 |
| `Timestamp` | `DateTime` | 訊息發送時間 |
| `Type` | `MessageType` | 文字、圖片、檔案或系統通知 |
| `ReadBy` | `List<Guid>` | 已閱讀此訊息的參與者 ID |
| `Role` | `MessageRole` | AI 對話角色（使用者、助手、工具） |
| `ToolCallId` | `string?` | 工具結果訊息的工具呼叫 ID |
| `ToolCallsJson` | `string?` | 助手訊息的序列化工具呼叫 JSON |
| `Thinking` | `string?` | AI 的思維鏈推理 |
| `PromptTokens` | `int?` | 提示詞中的 token 數量（輸入） |
| `CompletionTokens` | `int?` | 補全中的 token 數量（輸出） |
| `TotalTokens` | `int?` | 使用的總 token 數量（輸入 + 輸出） |
| `FileMetadata` | `FileMetadata?` | 附加的檔案元資料（如果訊息包含檔案） |

### 聊天訊息佇列

`ChatMessageQueue` 是一個執行緒安全的訊息佇列系統，用於管理聊天訊息的非同步處理：

- **執行緒安全** - 使用鎖定機制確保並行存取安全
- **非同步處理** - 支援非同步訊息入隊和出隊
- **訊息排序** - 保持訊息的時間順序
- **批次操作** - 支援批次取得訊息

### 檔案元資料

`FileMetadata` 用於管理附加到聊天訊息的檔案資訊：

- **檔案資訊** - 檔案名稱、大小、類型、路徑
- **上傳時間** - 檔案上傳的時間戳
- **上傳者** - 上傳檔案的使用者或矽基生命體 ID

### 串流取消管理器

`StreamCancellationManager` 提供 AI 串流回應的取消機制：

- **串流控制** - 支援取消正在進行的 AI 串流回應
- **資源清理** - 取消時正確清理相關資源
- **並行安全** - 支援多個串流同時管理

### 聊天歷史檢視

新增的聊天歷史檢視功能允許使用者瀏覽矽基生命體的歷史對話：

- **會話清單** - 顯示所有歷史會話
- **訊息詳情** - 檢視完整訊息歷史
- **時間線檢視** - 按時間順序展示訊息
- **API 支援** - 提供 RESTful API 取得會話和訊息資料

---

## AI 用戶端系統

系統透過 `IAIClient` 介面支援多個 AI 後端：

### OllamaClient

- **類型**：本地 AI 服務
- **協定**：原生 Ollama HTTP API（`/api/chat`、`/api/generate`）
- **功能**：串流傳輸、工具呼叫、本地模型託管
- **設定**：`endpoint`、`model`、`temperature`、`maxTokens`

### DashScopeClient（阿里雲百煉）

- **類型**：雲端 AI 服務
- **協定**：相容 OpenAI 的 API（`/compatible-mode/v1/chat/completions`）
- **認證**：Bearer token（API 金鑰）
- **功能**：串流傳輸、工具呼叫、推理內容（思維鏈）、多區域部署
- **支援的區域**：
  - `beijing` —— 華北2（北京）
  - `virginia` —— 美國（維吉尼亞）
  - `singapore` —— 新加坡
  - `hongkong` —— 中國香港
  - `frankfurt` —— 德國（法蘭克福）
- **支援的模型**（透過 API 動態探索，帶有回退清單）：
  - **通義千問系列**：qwen3-max、qwen3.6-plus、qwen3.6-flash、qwen-max、qwen-plus、qwen-turbo、qwen3-coder-plus
  - **推理**：qwq-plus
  - **第三方**：deepseek-v3.2、deepseek-r1、glm-5.1、kimi-k2.5、llama-4-maverick
- **設定**：`apiKey`、`region`、`model`
- **模型探索**：執行時期從百煉 API 取得可用模型；網路故障時回退到精選清單

### VolcengineArkClient（火山引擎 Ark）

- **類型**：雲端 AI 服務
- **協定**：相容 OpenAI 的 API
- **認證**：Bearer token（API 金鑰）
- **功能**：支援串流和非串流模式，內建雙層速率控制
  - 自我速率控制：強制執行請求間最小間隔
  - 伺服器速率限制：處理 429 錯誤，指數退避重試
- **設定**：`apiKey`、`endpoint`、`model`
- **特點**：位元組跳動旗下 AI 服務，支援多種豆包模型

### DeepSeekClient

- **類型**：雲端 AI 服務
- **協定**：相容 OpenAI 的 API（`https://api.deepseek.com`）
- **認證**：Bearer token（API 金鑰）
- **功能**：串流傳輸、工具呼叫、thinking 模式（reasoning_content）、reasoning_effort 參數
- **上下文視窗**：1,048,576 tokens
- **設定**：`apiKey`、`model`

### ZhipuClient（智譜 GLM）

- **類型**：雲端 AI 服務
- **協定**：相容 OpenAI 的 API（`https://open.bigmodel.cn/api/paas/v4`）
- **認證**：Bearer token（API 金鑰）
- **功能**：串流傳輸、工具呼叫、thinking 模式、按模型判斷視覺支援
- **上下文視窗**：1,048,576 tokens
- **設定**：`apiKey`、`model`

### ErnieClient（百度千帆/文心一言）

- **類型**：雲端 AI 服務
- **協定**：相容 OpenAI 的 API（`https://qianfan.baidubce.com/v2`）
- **認證**：Bearer token（API 金鑰）
- **功能**：串流傳輸、工具呼叫、按模型判斷視覺支援
- **上下文視窗**：131,072 tokens
- **設定**：`apiKey`、`model`

### HunyuanClient（騰訊混元）

- **類型**：雲端 AI 服務
- **協定**：相容 OpenAI 的 API（雙端點：TokenHub 推薦 + Legacy `https://api.hunyuan.cloud.tencent.com/v1`）
- **認證**：Bearer token（API 金鑰）
- **功能**：串流傳輸、按模型判斷工具呼叫、不支援視覺
- **上下文視窗**：262,144 tokens
- **支援模型**：hy3（推薦）、hy3-preview
- **設定**：`apiKey`、`model`

### MiniMaxClient

- **類型**：雲端 AI 服務
- **協定**：相容 OpenAI 的 API（`https://api.minimaxi.com/v1`）
- **認證**：Bearer token（API 金鑰）
- **功能**：串流傳輸、工具呼叫、按模型判斷視覺支援
- **上下文視窗**：1,048,576 tokens
- **設定**：`apiKey`、`model`

### MoonshotClient（月之暗面/Kimi）

- **類型**：雲端 AI 服務
- **協定**：相容 OpenAI 的 API（`https://api.moonshot.cn/v1`）
- **認證**：Bearer token（API 金鑰）
- **功能**：串流傳輸、工具呼叫、按模型判斷視覺支援
- **上下文視窗**：262,144 tokens
- **設定**：`apiKey`、`model`

### SiliconFlowClient（矽基流動）

- **類型**：雲端 AI 服務（聚合平臺）
- **協定**：相容 OpenAI 的 API（`https://api.siliconflow.cn/v1`）
- **認證**：Bearer token（API 金鑰）
- **功能**：串流傳輸、工具呼叫、按模型判斷視覺支援、支援動態取得可用模型清單（/models 介面）
- **上下文視窗**：1,048,576 tokens
- **設定**：`apiKey`、`model`

### 用戶端工廠模式

每種 AI 用戶端類型都有相應的工廠實作 `IAIClientFactory`：

- `OllamaClientFactory` —— 建立 OllamaClient 實例
- `DashScopeClientFactory` —— 建立 DashScopeClient 實例
- `VolcengineArkClientFactory` —— 建立 VolcengineArkClient 實例
- `HerdsmanClientFactory` —— 建立 HerdsmanClient 實例
- `LongCatClientFactory` —— 建立 LongCatClient 實例
- `QiniuAIClientFactory` —— 建立 QiniuAIClient 實例
- `DeepSeekClientFactory` —— 建立 DeepSeekClient 實例
- `ZhipuClientFactory` —— 建立 ZhipuClient 實例
- `ErnieClientFactory` —— 建立 ErnieClient 實例
- `HunyuanClientFactory` —— 建立 HunyuanClient 實例
- `MiniMaxClientFactory` —— 建立 MiniMaxClient 實例
- `MoonshotClientFactory` —— 建立 MoonshotClient 實例
- `SiliconFlowClientFactory` —— 建立 SiliconFlowClient 實例

工廠提供：
- `CreateClient(Dictionary<string, object> config)` —— 從設定實例化用戶端
- `GetConfigKeyOptions(string key, ...)` —— 傳回設定鍵的動態選項（例如可用模型、區域）
- `GetDisplayName()` —— 用戶端類型的在地化顯示名稱

### IAIClient 能力介面

`IAIClient` 介面定義了 AI 用戶端的能力宣告屬性，`ContextManager` 據此自適應調整行為：

| 屬性 | 類型 | 描述 |
|------|------|------|
| `StreamingMode` | `bool?` | 串流模式支援：true=僅串流、false=僅非串流、null=兩種均支援（預設串流） |
| `SupportsToolCalls` | `bool?` | 工具呼叫支援：true=支援、false=不支援（跳過工具注入）、null=未知（預設支援） |
| `ContextWindowTokens` | `int?` | 上下文視窗大小（token 數），用於 token 預算裁剪替代固定 MaxContextMessages |
| `SupportsVision` | `bool?` | 視覺輸入支援：true=支援圖片、false=不支援、null=未知（預設不支援） |
| `SupportsAudio` | `bool?` | 音訊輸入支援：true=支援音訊、false=不支援、null=未知（預設不支援） |

### AI平台支援清單

#### 狀態說明
- ✅ 已實作
- 🚧 開發中
- 📋 計劃中
- 💡 考慮中
- ⚠️ 已廢棄

*註：受開發者所在網路環境影響，對接[考慮中]的海外雲端AI服務可能需要藉助網路代理工具進行存取，除錯過程可能存在不穩定性。*

#### 平台清單

| 平台 | 狀態 | 類型 | 說明 |
|------|------|------|------|
| Ollama | ✅ | 本地 | 本地AI服務，支援本地模型部署 |
| DashScope（阿里雲百煉） | ✅ | 雲端 | 阿里雲百煉AI服務，支援多區域部署 |
| 百度千帆（文心一言） | ✅ | 雲端 | 百度文心一言AI服務 — ErnieClient |
| 智譜AI（GLM） | ✅ | 雲端 | 智譜清言AI服務 — ZhipuClient |
| 月之暗面（Kimi） | ✅ | 雲端 | 月之暗面Kimi AI服務 — MoonshotClient |
| 火山方舟引擎.豆包 | ✅ | 雲端 | 位元組跳動豆包AI服務 |
| 牧馬人推理引擎（Herdsman） | ✅ | 本地/雲端 | 無需認證的推理引擎，相容 OpenAI API 格式 |
| 美團 LongCat | ✅ | 雲端 | 美團自研大模型，相容 OpenAI API 格式，API Key 認證 |
| 七牛雲 AI | ✅ | 雲端 | 七牛雲大模型推理服務，相容 OpenAI API 格式，API Key 認證 |
| DeepSeek（直連） | ✅ | 雲端 | 深度求索AI服務 — DeepSeekClient，支援 thinking 模式 |
| 零一萬物 | ⚠️ | 雲端 | 零一萬物AI服務（已廢棄：停止新使用者註冊） |
| 騰訊混元 | ✅ | 雲端 | 騰訊混元AI服務 — HunyuanClient，雙端點 TokenHub/Legacy |
| 矽基流動 | ✅ | 雲端 | 矽基流動AI服務 — SiliconFlowClient，支援動態模型清單 |
| MiniMax | ✅ | 雲端 | MiniMax AI服務 — MiniMaxClient |
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
| Cerebras | 💡 | 雲端 | Cerebras AI推理最佳化服務 |
| Databricks | 💡 | 雲端 | Databricks企業AI平台（MosaicML） |
| Perplexity AI | 💡 | 雲端 | Perplexity AI搜尋問答服務 |
| NVIDIA NIM | 💡 | 雲端 | NVIDIA AI推理微服務 |

---

## 關鍵設計決策

### 儲存作為實例類別（而非靜態）

`IStorage` 被設計為可注入的實例，而不是靜態工具。這確保：

- 直接檔案系統存取 —— IStorage 是系統的內部持久化通道，**不**透過執行器路由。
- **AI 無法控制 IStorage** —— 執行器管理 AI 工具發起的 IO；IStorage 管理框架自身的內部資料讀寫。這些是根本不同的關注點。
- 可使用模擬實作進行測試。
- 未來支援不同的儲存後端，無需修改消費者。

### 執行器作為安全邊界

執行器是 I/O 操作的**唯一**路徑。需要磁碟、網路或命令列存取的工具**必須**透過執行器。此設計強制執行：

- 每個執行器擁有**獨立的排程執行緒**，帶有用於權限驗證的執行緒鎖定。
- 集中式權限檢查 —— 執行器查詢生命體的**私有權限管理器**。
- 支援優先級和逾時控制的請求佇列。
- 所有外部操作的審計日誌。
- 異常隔離 —— 一個執行器的失敗不影響其他執行器。
- 熔斷器 —— 連續失敗暫時停止執行器以防止級聯失敗。

### ContextManager 作為輕量級物件

每次 `ExecuteOneRound()` 建立一個新的 `ContextManager` 實例：

1. 載入靈魂檔案 + 最近的聊天歷史。
2. 將請求發送到 AI 用戶端。
3. 迴圈處理工具呼叫，直到 AI 傳回純文字。
4. 將回應持久化到聊天系統。
5. 釋放。

這使每輪保持隔離和無狀態。

### 透過類別覆寫實作自我進化

矽基生命體可以在執行時期覆寫自己的 C# 類別：

1. AI 生成新類別程式碼（必須繼承 `SiliconBeingBase`）。
2. **編譯時參考控制**（主要防禦）：編譯器只獲得允許的組件清單 —— `System.IO`、`System.Reflection` 等被排除，因此危險程式碼在類型級別是不可能的。
3. **執行時期靜態分析**（次要防禦）：`SecurityScanner` 在成功編譯後掃描程式碼中的危險模式。
4. Roslyn 在記憶體中編譯程式碼。
5. 成功時：`SiliconBeingManager.ReplaceBeing()` 交換目前實例，遷移狀態，並將加密程式碼持久化到磁碟。
6. 失敗時：丟棄新程式碼，保留現有實作。

自訂 `IPermissionCallback` 實作也可以透過 `ReplacePermissionCallback()` 編譯和注入，允許生命體自訂自己的權限邏輯。

程式碼在磁碟上以 AES-256 加密儲存。加密金鑰從生命體的 GUID（大寫）透過 PBKDF2 衍生。

---

## Token 使用審計

`TokenUsageAuditManager` 追蹤所有生命體的 AI token 消耗：

- `TokenUsageRecord` —— 每次請求的記錄（生命體 ID、模型、提示詞 token、補全 token、時間戳）
- `TokenUsageSummary` —— 聚合統計
- `TokenUsageQuery` —— 用於篩選記錄的查詢參數
- 透過 `ITimeStorage` 持久化以進行時間序列查詢
- 可透過 Web UI（UsageController）和 `TokenAuditTool`（僅主理人）存取

---

### 日曆系統

系統包含 **32 種日曆實作**，衍生自抽象 `CalendarBase` 類別，涵蓋世界主要日曆系統：

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

### 外觀系統

Web UI 具有**可插拔的外觀系統**，允許完整的 UI 自訂，無需更改應用程式邏輯：

- **ISkin 介面** —— 定義所有外觀的契約，包括：
  - 核心繪製方法（`RenderHtml`、`RenderError`）
  - 20+ UI 元件方法（按鈕、輸入、卡片、表格、徽章、氣泡、進度、標籤等）
  - 透過 `CssBuilder` 生成主題 CSS
  - `SkinPreviewInfo` —— 初始化頁面外觀選擇器的調色盤和圖示

- **內建外觀** —— 7 種生產就緒的外觀：
  - **Admin** —— 專業、資料聚焦的系統管理介面
  - **Chat** —— 對話式、以訊息為中心的設計，用於 AI 互動
  - **Creative** —— 藝術性、視覺豐富的創意工作流程佈局
  - **Dev** —— 以開發者為中心、以程式碼為中心的介面，帶語法高亮
  - **HighContrast** —— 高對比度無障礙主題
  - **Light** —— 清爽的淺色主題
  - **Minimal** —— 極簡主義主題

- **外觀探索** —— `SkinManager` 透過反射自動探索和註冊所有 `ISkin` 實作

### HTML / CSS / JS 建構器

Web UI 完全避免範本檔案，在 C# 中生成所有標記：

- **`H`** —— 串流 HTML 建構器 DSL，用於在程式碼中建構 HTML 樹
- **`CssBuilder`** —— CSS 建構器，支援選擇器和媒體查詢
- **`JsBuilder`（`JsSyntax`）** —— JavaScript 建構器，用於內嵌指令碼

### 控制器系統

Web UI 遵循**類 MVC 模式**，24 個控制器處理不同方面：

| 控制器 | 用途 |
|------------|---------|
| About | 關於頁面和專案資訊 |
| Audit | Token 使用審計儀表板 |
| Being | 矽基生命體管理和狀態 |
| Chat | 帶 SSE 的即時聊天介面 |
| ChatHistory | 聊天歷史檢視，支援會話清單和訊息詳情 |
| CodeBrowser | 程式碼檢視和編輯 |
| CodeHover | 程式碼懸浮提示，支援語法高亮 |
| Config | 系統設定管理 |
| Dashboard | 系統概覽和指標 |
| Executor | 執行器狀態和管理 |
| Help | 說明文件系統，多語言支援 |
| Init | 首次執行初始化精靈 |
| Knowledge | 知識圖譜視覺化和查詢 |
| Log | 系統日誌檢視器，支援矽基生命體篩選 |
| Memory | 長期記憶瀏覽器，支援進階篩選、統計和詳情檢視 |
| Permission | 權限管理 |
| PermissionRequest | 權限請求佇列 |
| Project | 專案管理，包含工作筆記、任務系統和工具權限 |
| System | 系統管理與執行時期監控 |
| Task | 任務系統介面 |
| Timer | 定時器系統管理，包含執行歷史 |
| ToolPermission | 工具權限管理，支援矽基生命體和專案級別的權限設定 |
| Usage | Token 使用審計儀表板，帶趨勢圖和匯出 |
| WorkNote | 工作筆記管理，支援搜尋和目錄生成 |

### 即時更新

- **SSE（伺服器發送事件）** —— 透過 `SSEHandler` 推送聊天訊息、生命體狀態和系統事件的更新
- **無需 WebSocket** —— 使用 SSE 滿足大多數即時需求的更簡單架構
- **自動重連** —— 用戶端重連邏輯實作彈性連線

### 在地化

系統支援 **34 種語言變體**的全面在地化：
- **中文（6 種）**：zh-CN（簡體）、zh-HK（繁體）、zh-SG（新加坡）、zh-MO（澳門）、zh-TW（台灣）、zh-MY（馬來西亞）
- **英文（10 種）**：en-US、en-GB、en-CA、en-AU、en-IN、en-SG、en-ZA、en-IE、en-NZ、en-MY
- **西班牙語（2 種）**：es-ES、es-MX
- **德語（5 種）**：de-DE、de-AT、de-CH、de-LU、de-LI
- **法語（3 種）**：fr-FR、fr-CA、fr-CH
- **其他（8 種）**：ja-JP（日語）、ko-KR（韓語）、cs-CZ（捷克語）、it-IT（義大利語）、pl-PL（波蘭語）、pt-PT（葡萄牙語）、pt-BR（巴西葡萄牙語）、ru-RU（俄語）

透過 `DefaultConfigData.Language` 選擇活動語言環境，並透過 `LocalizationManager` 解析。

---

### WebView 瀏覽器自動化系統（新增）

系統整合了基於 **Playwright** 的 WebView 瀏覽器自動化功能：

- **個體隔離**：每個矽基生命體擁有獨立的瀏覽器實例、Cookie 和會話儲存，完全隔離互不干擾。
- **無頭模式**：瀏覽器執行在使用者完全不可見的無頭模式下，矽基生命體背景自主操作。
- **WebViewBrowserTool**：提供完整的瀏覽器操作能力，包括：
  - 頁面導航、點擊、輸入文字、取得頁面內容
  - 執行 JavaScript、取得螢幕擷取、等待元素出現
  - 瀏覽器狀態管理和資源清理
- **安全控制**：所有瀏覽器操作均需透過權限驗證鏈，防止惡意網頁存取。

### 知識網絡系統（新增）

系統內建基於**三元組結構**的知識圖譜系統：

- **知識表示**：採用「主體-關係-客體」三元組結構（例如：Python-is_a-programming_language）
- **KnowledgeTool**：提供知識的全生命週期管理：
  - `add`/`query`/`update`/`delete` - 基礎 CRUD 操作
  - `search` - 全文搜尋和關鍵詞匹配
  - `get_path` - 探索兩個概念間的關聯路徑
  - `validate` - 知識完整性檢查
  - `stats` - 知識網絡統計分析
- **持久化儲存**：知識三元組持久化到檔案系統，支援時間索引查詢。
- **置信度評分**：每個知識條目帶有置信度評分（0-1），支援知識的模糊匹配和排序。
- **標籤分類**：支援為知識添加標籤，便於分類和檢索。

---

## 資料目錄結構

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # 主理人的靈魂檔案
    │   ├── state.json       # 執行時期狀態
    │   ├── code.enc         # AES 加密的自訂類別程式碼
    │   └── permission.enc   # AES 加密的自訂權限回呼
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```

---

## SpeedyPack 儲存引擎

SiliconLife.Fast 使用自研的 SpeedyPack 儲存引擎（.spk 格式），替代了之前的 LiteDB 方案，實作了極致的讀寫效能。

### 架構設計

```
┌──────────────────────────────────────────────────────────┐
│                    SpeedyPack                             │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────────┐  │
│  │ DirectoryMap  │  │  EntryCache   │  │  WriteQueue   │  │
│  │ (記憶體目錄映射) │  │  (條目快取)    │  │ (非同步寫入佇列) │  │
│  └──────┬───────┘  └──────┬───────┘  └───────┬───────┘  │
│         │                  │                   │          │
│  ┌──────▼──────────────────▼───────────────────▼───────┐  │
│  │              PackFileReader / PackFileWriter          │  │
│  │              (包檔案讀寫器)                             │  │
│  └──────────────────────────┬──────────────────────────┘  │
│                              │                             │
│  ┌──────────────────────────▼──────────────────────────┐  │
│  │              .spk 檔案 (MessagePack + LZ4 壓縮)       │  │
│  └─────────────────────────────────────────────────────┘  │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐                      │
│  │  FreeList     │  │ SpeedyPack   │                      │
│  │ (空閒空間管理) │  │ AutoCompactor│                      │
│  │              │  │ (自動壓縮)    │                      │
│  └──────────────┘  └──────────────┘                      │
└──────────────────────────────────────────────────────────┘
```

### 核心元件

| 元件 | 描述 |
|------|------|
| `SpeedyPack` | 核心類別，組合 DirectoryMap、EntryCache 和 WriteQueue 提供低延遲讀寫 |
| `DirectoryMap` | 記憶體目錄映射，維護虛擬路徑到檔案條目的映射關係 |
| `EntryCache` | 條目快取，基於 TTL 的最近存取條目快取 |
| `WriteQueue` | 非同步寫入佇列，將寫入操作排隊到背景執行緒執行 |
| `FreeList` | 空閒空間管理，追蹤 .spk 檔案中的可重用空間 |
| `PackFileReader` | 包檔案讀取器，從 .spk 檔案中讀取資料 |
| `PackFileWriter` | 包檔案寫入器，將資料寫入 .spk 檔案 |
| `SpeedyPackAutoCompactor` | 自動壓縮定時器，定期壓縮 .spk 檔案回收空閒空間 |
| `SpeedyPackRegistry` | 處理序級單例管理器，確保整個應用使用同一個 SpeedyPack 實例 |

### 儲存適配器

SiliconLife.Fast 透過以下適配器將 SpeedyPack 整合到系統介面：

| 適配器 | 介面 | 描述 |
|--------|------|------|
| `SpeedyStorage` | `IStorage` | 通用鍵值儲存適配器 |
| `SpeedyTimeStorage` | `ITimeStorage` | 時間索引儲存適配器 |
| `SpeedyWorkNoteStorage` | `IWorkNoteStorage` | 工作筆記儲存適配器 |

### 設定選項

`SpeedyPackOptions` 提供以下設定：

| 選項 | 類型 | 預設值 | 描述 |
|------|------|--------|------|
| `CacheTtl` | `TimeSpan` | 5 分鐘 | 快取條目的存活時間 |
| `MaxCacheEntries` | `int` | 1000 | 最大快取條目數 |
| `ReadOnly` | `bool` | false | 唯讀模式 |

### 交易支援

SpeedyPack 透過 `IPackTransaction` 介面支援原子寫入操作：

- `SpeedyTransaction` 實作了交易機制
- 支援批次寫入的原子性
- 交易提交時所有寫入操作要麼全部成功，要麼全部回復

---

## 插件系統

SiliconLife 透過插件系統支援功能擴充，允許第三方開發者為平台添加新功能。

### 核心介面

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### 插件載入器

`PluginLoader` 負責從指定目錄載入插件 DLL，並執行嚴格的安全檢查：

1. **目錄掃描** — 掃描插件目錄中的所有 .dll 檔案
2. **安全掃描** — 檢查插件是否參考了禁止的命名空間
3. **隔離載入** — 使用自訂 `AssemblyLoadContext` 隔離載入插件
4. **生命週期管理** — 呼叫插件的 OnLoad、OnStart、OnStop、OnUnload 方法

### 安全沙箱

插件載入器執行以下安全檢查：

| 檢查項 | 描述 |
|--------|------|
| 禁止命名空間 | System.IO、System.Net.Http、System.Net.WebSockets、System.Net.Sockets、Microsoft.CodeAnalysis |
| 可信組件白名單 | Google.Protobuf、Newtonsoft.Json、MessagePack、Serilog、Microsoft.Extensions.Logging.Abstractions、Dapper |
| 禁止類型檢查 | 掃描插件中參考的危險類型 |
| 禁止成員檢查 | 掃描插件中呼叫的危險方法 |

### 工具整合

插件可以透過實作 `ITool` 介面註冊自訂工具：

- `ToolManager.ScanAllPluginAssemblies()` 方法掃描所有已載入插件中的 ITool 實作
- 插件工具自動整合到工具呼叫迴圈
- 插件工具受相同的權限系統約束

### 插件生命週期

```
載入（OnLoad）→ 啟動（OnStart）→ 執行中 → 停止（OnStop）→ 卸載（OnUnload）
```

---

## 矽基生命體活動狀態

矽基生命體具有以下活動狀態：

| 狀態 | 描述 |
|------|------|
| `Idle` | 空閒狀態，等待時鐘觸發 |
| `SingleChat` | 正在進行一對一聊天 |
| `GroupChat` | 正在進行群聊 |
| `Task` | 正在執行任務 |
| `Timer` | 正在執行定時器 |
| `Stopped` | 已停止，因連續錯誤或手動停止 |

**Stopped 狀態機制**：
- 當矽基生命體連續發生 10 次錯誤時，自動進入 `Stopped` 狀態
- 進入 Stopped 狀態後，生命體將不再執行任何任務
- 當有新的聊天訊息到達時，錯誤計數器會被重設，生命體復原執行

狀態轉換：
```
Idle → SingleChat → Idle（聊天完成）
Idle → GroupChat → Idle（群聊完成）
Idle → Task → Idle（任務完成）
Idle → Timer → Idle（定時器完成）
任意 → Stopped（連續 10 次錯誤）
Stopped → Idle（新聊天訊息到達或手動重啟）
```

---

## 工作流引擎

工作流引擎是基於範本的狀態機系統，用於驅動矽基生命體在專案空間中的協作流程：

### 核心元件

| 元件 | 描述 |
|------|------|
| `WorkflowEngine` | 工作流引擎核心，管理範本和實例，執行 Tick 驅動的狀態轉換 |
| `WorkflowTemplate` | 工作流範本，定義狀態集合和轉換規則 |
| `WorkflowInstance` | 工作流實例，繫結到具體專案，追蹤目前狀態 |
| `WorkflowLog` | 工作流日誌，記錄狀態轉換歷史 |

### 工作機制

- **範本註冊**：透過 `RegisterTemplate()` 註冊工作流範本，定義狀態和轉換規則
- **實例建立**：從範本建立實例，繫結到專案空間
- **Tick 驅動**：狀態轉換由主迴圈的 Tick 機制驅動
- **日誌記錄**：所有狀態轉換自動記錄到日誌

---

## 記憶淡忘機制

`MemoryFadeService` 是一個定時衰減服務，模擬生物記憶的遺忘特性：

### 工作機制

- **定時執行**：繼承自 `TickObject`，預設每小時執行一次衰減週期
- **重要性衰減**：對每個矽基生命體的記憶條目應用衰減演算法，降低重要性評分
- **自動歸檔**：重要性低於閾值的記憶自動歸檔（`ArchiveFadingMemories()`）
- **統計追蹤**：記錄衰減週期數、狀態變更條目數等統計資料

### 衰減流程

```
MemoryFadeService.OnTick()
  └── 遍歷所有矽基生命體
       └── being.Memory.ApplyDecay()      # 應用重要性衰減
       └── being.Memory.ArchiveFadingMemories()  # 歸檔低重要性記憶
```

---

## 專案工作區系統

專案工作區是支援多矽基生命體協作的空間管理機制：

### 核心功能

- **專案生命週期**：建立 → 活躍 → 歸檔 → 銷毀
- **角色分配**：支援為矽基生命體分配專案角色
- **工具權限隔離**：專案級別的工具權限設定，獨立於矽基生命體級別的權限
- **工作筆記**：專案空間內的頁面式筆記系統，支援目錄生成和關鍵詞搜尋
- **任務追蹤**：專案級別的任務管理，支援建立、分配、狀態追蹤
- **工作流整合**：專案可繫結工作流範本，驅動協作流程

### 相關工具

| 工具 | 用途 |
|------|------|
| `ProjectTool` | 專案空間管理（建立、歸檔、銷毀、角色分配） |
| `ProjectTaskTool` | 專案任務管理（建立、分配、狀態更新） |
| `ProjectWorkNoteTool` | 專案工作筆記（建立、搜尋、目錄生成） |
| `ProjectWorkTool` | 專案工作操作（建立任務、群聊、廣播、完成專案） |
