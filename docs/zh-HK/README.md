# 矽基生命群

**⚠️ 警告：動態編譯尚未經過深度測試，但確實可以運作。必須在程式碼中增加範本才能正常工作**

**⚠️ 警告：Web 頁面正處於快速調整期，當前頁面功能可能無法正常響應**

基於 .NET 9 的多智能體協作平台，AI 智能體（矽基人）可透過 Roslyn 動態編譯實現自我進化。

[English](../../README.md) | [简体中文](../zh-CN/README.md)

## 特性

- **多智能體編排** — 由矽基主理人統一管理，基於 Tick 驅動的時間分片公平排程（MainLoop + TickObject + 看門狗 + 熔斷器）
- **靈魂檔案驅動** — 每個矽基人由核心提示詞檔案（`soul.md`）驅動，定義其個性與行為模式
- **身體-大腦架構** — 身體（SiliconBeing）保持存活並偵測觸發場景；大腦（ContextManager）載入歷史、呼叫 AI、執行工具並持久化響應
- **工具呼叫循環** — AI 返回 tool_calls → 執行工具 → 回饋結果 → AI 繼續 → 直到返回純文字
- **執行器-權限安全體系** — 所有磁碟、網路、命令列操作均透過執行器進行權限驗證
  - 5 級權限查詢鏈：IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - 所有權限決策均有稽核日誌
- **Token 用量稽核** — 內建 Token 用量追蹤與報告（`ITokenUsageAudit` / `TokenUsageAuditManager`）
- **最小化依賴** — 核心庫僅依賴 Microsoft.CodeAnalysis.CSharp 用於 Roslyn 動態編譯
- **零資料庫依賴** — 基於檔案系統儲存（JSON），支援透過 `ITimeStorage` 進行時間索引查詢
- **國際化** — 內建簡體中文、繁體中文和英文支援
- **Web 介面** — 內建 HTTP 伺服器，支援 SSE，多種佈景主題，完整的儀表板
  - **佈景主題系統** — 4 種內建佈景主題（Admin、Chat、Creative、Dev），提供完整的 UI 元件庫
  - **16 個控制器** — About、Being、Chat、CodeBrowser、Config、Dashboard、Executor、Init、Knowledge、Log、Memory、Permission、PermissionRequest、Project、Task、Timer
  - **即時更新** — 透過 SSE（Server-Sent Events）實現即時資料串流

## 技術棧

| 元件 | 技術 |
|------|------|
| 執行環境 | .NET 9 |
| 開發語言 | C# |
| AI 接入 | Ollama（原生 HTTP API） |
| 資料儲存 | 檔案系統（JSON + 時間索引目錄結構） |
| Web 伺服器 | HttpListener（.NET 內建） |
| 動態編譯 | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） |
| 開源授權 | Apache-2.0 |

## 專案結構

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # 核心庫（介面、抽象類別）
│   │   ├── ServiceLocator.cs             # 全域服務定位器：Register/Get、ChatSystem、IMManager、AuditLogger、GlobalACL、BeingFactory、BeingManager、DynamicBeingLoader、TokenUsageAudit
│   │   ├── Runtime/                       # MainLoop、TickObject、CoreHost、CoreHostBuilder、PerformanceMonitor
│   │   ├── SiliconBeing/                  # SiliconBeingBase、SiliconBeingManager、SiliconCurator、ISiliconBeingFactory、SoulFileManager、Memory、TaskSystem、TimerSystem
│   │   ├── AI/                            # IAIClient、IAIClientFactory、ContextManager（「大腦」）、Message、AIRequest/AIResponse
│   │   ├── Audit/                         # ITokenUsageAudit、TokenUsageAuditManager、TokenUsageRecord、TokenUsageSummary、TokenUsageQuery
│   │   ├── Chat/                          # ChatSystem、IChatService、SimpleChatService、SessionBase、SingleChatSession、GroupChatSession、ChatMessage
│   │   ├── Executors/                     # ExecutorBase、DiskExecutor、NetworkExecutor、CommandLineExecutor、ExecutorRequest、ExecutorResult
│   │   ├── Tools/                         # ITool、ToolManager（反射掃描）、ToolCall/ToolResult、ToolDefinition、SiliconManagerOnlyAttribute
│   │   ├── Security/                      # PermissionManager、GlobalACL、AuditLogger、UserFrequencyCache、PermissionResult、PermissionType、IPermissionCallback、IPermissionAskHandler
│   │   ├── IM/                            # IIMProvider、IMManager（訊息路由）
│   │   ├── Storage/                       # IStorage、ITimeStorage（鍵值儲存 + 時間索引儲存）
│   │   ├── Config/                        # ConfigDataBase、Config（單例 + JSON）、ConfigDataBaseConverter、GuidConverter、AIClientConfigAttribute、ConfigGroupAttribute、ConfigIgnoreAttribute、DirectoryInfoConverter
│   │   ├── Localization/                  # LocalizationBase、LocalizationManager、Language 列舉
│   │   ├── Logging/                       # ILogger、ILoggerProvider、LogEntry、LogLevel、LogManager
│   │   ├── Compilation/                   # DynamicBeingLoader、DynamicCompilationExecutor、SecurityScanner、CodeEncryption
│   │   └── Time/                          # IncompleteDate（時間範圍查詢）
│   │
│   └── SiliconLife.Default/               # 預設實作 + 程式進入點
│       ├── Program.cs                     # 應用程式進入點（組裝所有元件）
│       ├── AI/                            # OllamaClient、OllamaClientFactory（原生 Ollama HTTP API）
│       ├── SiliconBeing/                  # DefaultSiliconBeing、DefaultSiliconBeingFactory
│       ├── Calendar/                      # 日曆工具
│       ├── Executors/                     # 預設執行器實作
│       ├── IM/                            # WebUIProvider（Web UI 作為 IM 通道）、IMPermissionAskHandler
│       ├── Tools/                         # 內建工具：日曆、聊天、主理人、磁碟、動態編譯、記憶、網路、系統、任務、計時器
│       ├── Config/                        # DefaultConfigData
│       ├── Localization/                  # ZhCN、ZhHK、EnUS、DefaultLocalizationBase
│       ├── Logging/                       # ConsoleLoggerProvider、FileSystemLoggerProvider
│       ├── Storage/                       # FileSystemStorage、FileSystemTimeStorage
│       ├── Security/                      # DefaultPermissionCallback
│       ├── Runtime/                       # TestTickObject
│       └── Web/                           # Web UI 實作
│           ├── Controllers/               # 16 個控制器：About、Being、Chat、CodeBrowser、Config、Dashboard、Executor、Init、Knowledge、Log、Memory、Permission、PermissionRequest、Project、Task、Timer
│           ├── Models/                    # ViewModel：AboutViewModel、BeingViewModel、ChatViewModel、CodeBrowserViewModel、ConfigViewModel、DashboardViewModel、ExecutorViewModel、KnowledgeViewModel、LogViewModel、MemoryViewModel、PermissionViewModel、PermissionRequestViewModel、ProjectViewModel、TaskViewModel、TimerViewModel、ViewModelBase
│           ├── Views/                     # HTML 視圖：ViewBase、AboutView、BeingView、ChatView、CodeBrowserView、CodeEditorView、ConfigView、DashboardView、ExecutorView、KnowledgeView、LogView、MarkdownEditorView、MemoryView、PermissionView、ProjectView、TaskView、TimerView
│           ├── Skins/                     # 4 種佈景主題：Admin（專業）、Chat（對話）、Creative（創意）、Dev（開發者）
│           ├── ISkin.cs                   # 佈景主題介面 + SkinPreviewInfo + SkinManager（自動探索）
│           ├── Controller.cs              # 控制器基底類別
│           ├── WebHost.cs                 # HTTP 伺服器（HttpListener）
│           ├── Router.cs                  # 請求路由（模式比對）
│           ├── SSEHandler.cs              # 伺服器推送事件
│           ├── WebSecurity.cs             # Web 安全工具
│           ├── H.cs                       # 流式 HTML 建構器 DSL
│           ├── CssBuilder.cs              # CSS 建構工具
│           └── JsBuilder.cs               # JavaScript 建構工具
│
├── docs/
│   ├── zh-CN/                             # 簡體中文文件
│   └── zh-HK/                             # 繁體中文文件
```

## 架構概覽

```
MainLoop（專用執行緒，看門狗 + 熔斷器）
  └── TickObject（按優先順序排序）
       └── SiliconBeingManager
            └── SiliconBeingRunner（每次 Tick 獨立臨時執行緒，帶逾時和熔斷）
                 └── DefaultSiliconBeing.Tick()
                      └── ContextManager.ThinkOnChat()
                           └── IAIClient.Chat() → 工具呼叫循環 → 持久化到 ChatSystem
```

所有 AI 發起的 I/O 操作均經過安全鏈：

```
工具呼叫 → 執行器 → 權限管理器 → [IsCurator → 頻率快取 → 全域ACL → 回呼 → 詢問使用者]
```

## 快速開始

### 環境需求

- .NET 9 SDK
- 本機執行 [Ollama](https://ollama.com) 並拉取模型（如 `ollama pull llama3`）

### 建置

```bash
dotnet restore
dotnet build
```

### 執行

```bash
dotnet run --project src/SiliconLife.Default
```

應用程式將啟動 Web 伺服器並自動在瀏覽器中開啟 Web UI。

### 發佈（單一檔案）

```bash
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

## 開發路線

- [x] 第一階段：主控台 AI 對話
- [x] 第二階段：框架骨架（MainLoop + TickObject + 看門狗 + 熔斷器）
- [x] 第三階段：第一個矽基人（靈魂檔案驅動，身體-大腦架構）
- [x] 第四階段：持久化記憶（ChatSystem + ITimeStorage）
- [x] 第五階段：工具系統 + 執行器
- [x] 第六階段：權限系統（5 級查詢鏈、稽核日誌、全域 ACL）
- [x] 第七階段：動態編譯 + 自我進化（Roslyn）
- [x] 第八階段：長期記憶 + 任務 + 計時器
- [x] 第九階段：CoreHost + 多矽基人協作
- [x] 第十階段：Web 介面（HTTP + SSE，16 個控制器，4 種佈景主題）
- [ ] 第十一階段：外接 IM（飛書 / WhatsApp / Telegram）
- [ ] 第十二階段：知識圖譜、外掛程式及其他

## 文件

- [架構設計](architecture.md) — 系統設計、排程機制、元件架構
- [安全設計](security.md) — 權限模型、執行器、動態編譯安全
- [開發路線](roadmap.md) — 詳細的 12 階段開發計畫

## 授權

本專案基於 Apache License 2.0 開源 — 詳見 [LICENSE](../../LICENSE) 檔案。

## 作者

天源垦骥 (Hoshino Kennji) — [B站](https://space.bilibili.com/617827040) | [YouTube](https://www.youtube.com/@hoshinokennji)
