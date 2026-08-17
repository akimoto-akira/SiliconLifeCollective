# 變更日誌

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | **繁體中文** | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md) | [Русский](../ru-RU/changelog.md)

本專案的所有重要變更都將記錄在此檔案中。

格式基於 [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)，
本專案遵循 [語意化版本控制](https://semver.org/spec/v2.0.0.html)。

---

## 關於此變更日誌

### 專案雙版本

本專案提供兩個實作版本：

- **SiliconLife.Default**：預設實作，主要用於驗證架構可行性。主控台應用程式，檔案系統 JSON 儲存。
- **SiliconLife.Fast**：主推生產版本。跨平台桌面應用程式（Windows / macOS / Linux），SpeedyPack 記憶體儲存 + 非同步持久化，經過深度效能最佳化。

兩個版本共享相同的介面和功能，僅在儲存實作和執行模式上有所不同。SiliconLife.Default 作為架構驗證基準，SiliconLife.Fast 作為生產環境主推版本。

### 專案起源

- 本專案起源於 2026 年 3 月 20 日。
- 在此專案之前，有一個驗證 Demo 因架構設計不合理而失敗，導致無法與多個 AI 平台整合。

### 使用的 AI IDE 工具

#### Kiro（Amazon AWS）
- 專案最初由 Kiro 維護，並使用 Spec 模式啟動。
- Kiro 是 Amazon AWS 建構的 agentic AI 開發環境。
- 基於 Code OSS（VS Code），支援 VS Code 設定和 Open VSX 相容外掛程式。
- 具有規格驅動的開發工作流程，用於結構化 AI 編碼。

#### Comate AI IDE / 文心快碼（百度）
- 偶爾用於文案和文件工作。
- Comate AI IDE 是百度文心於 2025 年 6 月 23 日發佈的 AI 原生開發環境工具。
- 行業首個多模態、多智慧代理協同的 AI IDE。
- 功能包括設計到程式碼轉換和全流程 AI 輔助編碼。
- 由百度文心 4.0 X1 Turbo 模型驅動。

#### Trae（字節跳動）
- 2025 年 10 月至 2026 年 4 月期間使用。
- AI IDE，支援智慧程式碼產生和專案管理。

#### Qoder（阿里巴巴）
- 自 2026 年 4 月 18 日起用於專案維護。
- AI 編碼平台，支援程式碼分析、文件產生和多智慧代理協作。

#### CatPaw（美團）
- 自 2026 年 5 月 6 日起與 Qoder 混合使用。
- 基於美團自研 LongCat 系列模型，具有強大的全程式碼架構重構能力。

#### DuMate（百度千帆）
- 自 2026 年 7 月起用於程式碼開發、在地化和文件編寫。
- 執行於千帆桌面平台的通用 AI 助手，具備多工具編排、檔案操作、瀏覽器自動化和多步任務執行能力。
- 直接在使用者 Windows 桌面上讀寫本地檔案、執行 Shell 命令、進行網路搜尋。

### 需求文件

- 本專案的需求文件未公開。
- 需求經過 12 多個國際 AI 平台和大型模型系列的反覆驗證，產生了超過 2000 行、幾乎人類無法理解的使用者故事驅動需求文件。

---

## [未發佈]

### 2026-08-17

#### 新功能
- `c7b575b` - 實作 MCP 整合——外部伺服器工具接入、設定管理與說明文件
  - 新增 MCP 核心（SiliconLife.Core/Mcp/）：McpManager 伺服器生命週期管理、stdio/http 雙傳輸、McpClientConnection 連線封裝、按伺服器包裝工具並以 `mcp_{serverId}_{toolName}` 命名注入所有矽基生命體
  - 新增 Web 管理頁面（/mcp）與 7 個 API 端點（list-servers/list-tools/add-server/toggle/remove-server/reconnect/test-tool）
  - 新增 McpTool 查詢工具（status/list_servers/list_tools，唯讀）；伺服器增刪僅限使用者透過 Web UI，AI 無法修改伺服器清單
  - 設定頁支援 MCP 伺服器陣列編輯器（強制回應視窗內行內新增/刪除）
  - 註冊 MCP 說明主題（🔌），10 種語言實作完整說明文件
  - MCP 包裝工具在權限矩陣中以 `execute` 動作呈現，支援按生命體/專案停用
  - 45 個檔案變更

### 2026-08-16

#### 新功能
- `5d76c5a` - 實作技能系統——工具編排與提示詞模板的重用抽象層
  - 新增 SkillDefinition（id/描述/參數 schema/系統提示詞模板/工具白名單/動作限制/最大輪數/逾時/完成動作/觸發模式）
  - 新增 SkillManager：技能註冊中心 + 執行引擎（子 AIRequest 迴圈、遞迴防護、全域輪數與逾時鉗制）
  - 雙觸發模式：Manual（AI 函式呼叫，技能以 ToolDefinition 注入、排程側優先路由）+ Auto（schedule 排程，支援 `HH:mm` / `N s|m|h|d` / cron 子集）
  - Markdown 優先儲存（YAML 前置 + 提示詞正文），純 Markdown 由 AI 自動補全中繼資料（使用者欄位不被覆蓋）
  - 熱重載（30 秒指紋偵測）、版本歸檔（skills/archive/）、3 個內建技能（summarize_document/code_review/research_topic）
  - 新增 skill 工具（create/list/update/update_from_md/delete/export/export_md/import/import_md）
  - 新增技能管理頁面（/skill）與 10 個 API 端點；配額 MaxCustomSkillsPerBeing（預設 50）
  - 權限：技能級 `execute` 動作權限、技能內工具白名單與生命體權限取嚴格側聯集
- `b60fc68` - 更新千帆模型列表與上下文視窗映射 - 新增 glm-5.2/glm-5.1/deepseek-v4-pro/deepseek-v4-flash/kimi-k2.6/ernie-5.1/qianfan-code-latest 模型，1M/128K 分級上下文視窗與視覺能力映射

### 2026-08-15

#### 新功能
- `eaa8417` - 實作 IM 平台 OAuth 授權精靈與設定密鑰環境變數解析
  - 新增 ImOAuthController/ImOAuthService 支援飛書 OAuth 授權流程（authorize/callback/status），含 state 防 CSRF、5 分鐘逾時、SSE 狀態推送
  - 新增 IMProviderRegistry 統一管理 IM 平台中繼資料（設定欄位 schema/OAuth 端點模板/Provider 工廠）
  - 新增 ConfigSecretResolver 解析設定中的 `${ENV_VAR}` 佔位符，深層複製替換不寫回原始設定
  - 設定頁整合 IM 授權精靈 UI（行內授權區 + SSE 即時狀態）
  - 補全 13 個語言檔案的 IM 授權狀態/說明文案翻譯

### 2026-07-26

#### 重構
- `ffc45c2` - 重構 IM 平台為多實例設定架構 - IMPlatforms 清單化（每平台獨立啟停）、AggregateIMProvider 聚合多平台訊息收發與權限競速、設定頁多實例編輯器

### 2026-07-19

#### 新功能
- `9bf2103` - Speedy.Manager 樹狀檢視整合多選刪除與多選匯出

#### 修復
- `0df0674` - 修復 Speedy.Manager 多選刪除僅刪除首項的問題

### 2026-07-16

#### 新功能
- `7431312` - 補全 13 個語言檔案的 AI 用戶端設定翻譯 - CsCZ/PlPL 從 stub 改為完整字典實作，其餘 10 個檔案補充 7 個新用戶端（DeepSeek/ZhipuGLM/MoonshotKimi/SiliconFlow/MiniMax/Ernie/Hunyuan）的 ConfigDisplayNames/ConfigDescriptions/ConfigGroupNames 條目，同步更新 6 個 ClientFactory 的設定鍵中繼資料
  - 20 個檔案變更

#### 文件
- `ce36036` - 根據 git 記錄重寫所有 13 個語言版本 changelog 的 2026-05-26 後內容
- `d6608ea` - 在所有 13 個語言版本的 changelog 中新增 DuMate（百度千帆）的 AI IDE 工具介紹
  - 13 個檔案變更

#### 協作框架
- `c607c97` - 註冊 DuMate（百度千帆）為常駐 AI 協作者到 .ai-collab 註冊表
  - 1 個檔案變更


### 2026-07-15

#### 新功能
- `c007263` - 補全 10 個 AI 用戶端的說明文件 - HelpTopics 註冊 10 個主題，HelpLocalizationBase 新增 30 個抽象屬性，12 個語言檔案實作完整 Markdown 說明內容（平台簡介/註冊步驟/設定方法/可用模型/計費/常見問題），覆蓋 Herdsman/LongCat/QiniuAI/DeepSeek/ZhipuGLM/MoonshotKimi/SiliconFlow/MiniMax/Ernie/Hunyuan
  - 12 個檔案變更
- `4634e33` - 實作 7 個國內 AI 平台用戶端（DeepSeek/智譜GLM/月之暗面Kimi/矽基流動/MiniMax/百度文心/騰訊混元）- 14 個獨立類別檔案，遵循 LongCatClient 風格，不使用繼承，全部 OpenAI 相容 + Bearer Token，支援 Tool Calling/串流/思考模式，在 DefaultSiliconBeing 和 DefaultSiliconBeingFactory 註冊
  - 16 個檔案變更

#### 文件
- `108c4ea` - 更新全部 13 語言文件以反映 7 個新 AI 用戶端 - 狀態 📋→✅，01.AI 標記為已廢棄
  - 94 個檔案變更


### 2026-07-14

#### 文件
- `344b429` - 全語種 architecture.md AI 平台狀態新增「已廢棄」狀態，標記零一萬物為已廢棄（停止新使用者註冊）
  - 13 個檔案變更


### 2026-07-07

#### 清理
- `e06e6f2` - 移除 OsmStore 工具鏈和 TravelCodeWikiWithAI 外掛程式 - 刪除 tools/OsmStore.* 三個專案，刪除 src/TravelCodeWikiWithAI/ 外掛程式專案，清理 sln 參照，專案回歸獨立版 TCW 開發路線
  - 45 個檔案變更


### 2026-07-06

#### 修復
- `1b15886` - OSM 資料模型標準化與元素類型安全修復
  - 7 個檔案變更


### 2026-07-05

#### 新功能
- `be4320b` - TravelCodeWikiWithAI 新增 CLDR 資料提供模組
  - 4 個檔案變更


### 2026-07-04

#### 新功能
- `dbcabf3` - 外掛程式權限系統增強 - 重構網路/檔案 IO 為 Executor 模式 + GeneratedCodeAttribute 白名單豁免
  - 34 個檔案變更
- `e84bb63` - 修復編譯錯誤並新增 TravelCodeWikiWithAI 專案
  - 53 個檔案變更

#### 重構
- `9e5a345` - TravelCodeWikiWithAI 全量遷移 PBF 至同步線上 OSM API
  - 4 個檔案變更


### 2026-05-31

#### 新功能
- `a5f37bd` - 更新專案思考、對話系統及儲存相關功能
  - 13 個檔案變更


### 2026-05-30

#### 新功能
- `c3cf429` - 新增 QiniuAIClient AI 用戶端（七牛雲 AI 大模型推理服務） (ref task-409)
  - 20 個檔案變更
- `d04131f` - 新增 LongCatClient AI 用戶端（美團 LongCat 大模型） (ref task-408)
  - 19 個檔案變更

#### 協作框架
- `e9564f5` - 更新所有修改的檔案
  - 140 個檔案變更
- `9c8b42f` - 歸檔 2026-05-29 的 sessions 和 changes
  - 20 個檔案變更


### 2026-05-29

#### 新功能
- `d548e48` - 專案思考詳情頁按輪次（Cycle）分組展示訊息並支援摺疊 (ref task-407)
  - 23 個檔案變更
- `28d893d` - IAIClient 增加多模態能力宣告介面 + ChatMessage 增加多模態欄位 (ref task-402)
  - 13 個檔案變更
- `ebe6a49` - 專案思考詳情頁增加會話狀態、建立時間、完成時間展示 (ref task-406)
  - 22 個檔案變更
- `9a53d55` - IAIClient 增加 ContextWindowTokens + Token 預算制 + 工廠設定化 (ref task-401, task-403)
  - 26 個檔案變更
- `202b99c` - 新增 HerdsmanClient AI 用戶端 + 修復初始化介面下拉選單不重新整理 (ref task-399, task-400)
  - 20 個檔案變更
- `285ab2f` - 專案處理記錄前端展示 (ref task-397)
  - 25 個檔案變更
- `b4b633f` - ThinkOnProject 偽 Session 多輪對話機制 (ref task-395)
  - 13 個檔案變更
- `d3e543f` - ThinkOnProject 場景上下文增加可用矽基人資訊 (ref task-394)
  - 21 個檔案變更
- `07eb628` - BuildRequest 動態注入矽基人專案歸屬資訊 (ref task-396)
  - 21 個檔案變更
- `2089696` - Tool 新增 Project 場景支援 + PluginLoader 多目錄統一重構
  - 12 個檔案變更

#### 修復
- `b80a33b` - 修復專案思考詳情頁載入提示文字硬編碼英文及缺少在地化 (ref task-405)
  - 6 個檔案變更
- `90b60c5` - 修復工具呼叫輪次中 AI 正文 Content 和 Thinking 被隱藏的問題 (ref task-404)
  - 8 個檔案變更
- `a7d9a97` - ThinkOnProject 多輪循環續接及專案提醒資訊遺失修復
  - 6 個檔案變更
- `c0838dd` - 修復 ProjectThinkSession 訊息未寫入 Cycle 及完成後歷史被刪除的問題 (ref task-398)
  - 7 個檔案變更
- `f3d1794` - 修復矽基人 Project/Broadcast/Stopped 狀態在地化缺失及展示異常 (ref task-393)
  - 20 個檔案變更
- `3eaa90d` - 移除已刪除專案 TravelCodeWikiWithAI 的解決方案參照
  - 1 個檔案變更

#### 協作框架
- `f3cbed7` - 註冊 task-394~396（ThinkOnProject 增強）
  - 3 個檔案變更
- `e1971f5` - 註冊 task-393（BeingActivity 在地化與展示修復）
  - 1 個檔案變更
- `e710fa4` - 更新 changes commitHash 和 state 會話結束
  - 2 個檔案變更
- `4cacc4a` - 歸檔 2026-05-28 的 sessions 和 changes
  - 4 個檔案變更


### 2026-05-28

#### 新功能
- `ae8b673` - 外掛程式目錄設定從單一路徑升級為多目錄清單 (ref task-391)
  - 29 個檔案變更
- `aac46c1` - PluginLoader 增加 CS 原始碼模式，無 DLL 時編譯載入外掛程式 (ref task-389)
  - 6 個檔案變更

#### 修復
- `63047b0` - 註冊所有 PluginLoader 到 ServiceLocator，修復多目錄外掛程式反射不全 (ref task-391)
  - 3 個檔案變更
- `fcad655` - 修復 directoryList 瀏覽按鈕互動問題 (ref task-392)
  - 9 個檔案變更

#### 文件
- `e6d3037` - PluginDemo-22 CS 原始碼編譯載入模式範例 (ref task-390)
  - 21 個檔案變更

#### 協作框架
- `09d9e9c` - 歸檔 30 個已完成任務（task-362~task-391）
  - 2 個檔案變更
- `66204a1` - 歸檔 2026-05-28 的 sessions（8）和 changes（8）
  - 18 個檔案變更
- `308a8d0` - 更新 task-391 relatedCommit
  - 1 個檔案變更
- `6fc4e05` - 註冊 task-389（CS 原始碼模式）和 task-390（PluginDemo-22）
  - 1 個檔案變更


### 2026-05-27

#### 新功能
- `e154a18` - 完成 PluginDemo-21 WorkflowTemplate 完整業務工作流程範例 (ref task-388)
  - 19 個檔案變更
- `aa771b3` - 實作 PluginCapability 宣告式權限系統 (ref task-379)
  - 9 個檔案變更
- `5e5e9d1` - 新增 04-SafeSystemIO System.IO 白名單安全類型範例 (ref task-370)
  - 20 個檔案變更

#### 文件
- `48f6702` - 對齊 19-TickObject 和 20-SpeedyPack 所有語言 README 翻譯至基準 (ref task-386, task-387)
  - 119 個檔案變更
- `5d570e5` - 完成 task-378 禁止的字串反射繞過反例 (ref task-378)
  - 19 個檔案變更
- `348c410` - PluginDemo-11 禁止的 P/Invoke 和 unsafe 程式碼反例 (ref task-377)
  - 19 個檔案變更
- `fc92a49` - PluginDemo-10 禁止的反射操作反例 (ref task-376)
  - 19 個檔案變更
- `826ad2a` - 建立 PluginDemo-09 禁止處理序操作反例外掛程式 (ref task-375)
  - 19 個檔案變更
- `7870b05` - 新增 PluginDemo-08 禁止網路操作反例 (ref task-374)
  - 15 個檔案變更
- `8636e31` - PluginDemo-07 禁止檔案 I/O 操作反例 (ref task-373)
  - 19 個檔案變更
- `322312e` - 新增 PluginDemo-06 TrustedAssemblies 受信相依範例 (ref task-372)
  - 19 個檔案變更
- `6df98a0` - 新增 IWorkflowPlugin 工作流程外掛程式範例 (ref task-371)
  - 20 個檔案變更
- `f3787ba` - PluginDemo-03 IObjectFactory 註冊與建立範例 (ref task-369)
  - 20 個檔案變更
- `bb4324d` - PluginDemo-02 ITypeRegistry 註冊與查詢範例 (ref task-368)
  - 20 個檔案變更
- `bbdfa3c` - PluginDemo-01 最簡 IPlugin 實作範例 (ref task-367)
  - 19 個檔案變更

#### 協作框架
- `de44057` - 歸檔 5 月 25 日和 27 日的 sessions 和 changes
  - 58 個檔案變更
- `9e4a84c` - 更新 tasks.json lastCommitHash 為 48f6702
  - 1 個檔案變更
- `beb58b2` - 補充 taskIndex 索引（8 pending, 19 completed）
  - 1 個檔案變更
- `63f7bfc` - 更新 task-388 relatedCommit (ref task-388)
  - 1 個檔案變更
- `e61be6f` - 更新 task-378 relatedCommit (ref task-378)
  - 1 個檔案變更
- `dde579b` - 發佈 WorkflowTemplate 完整使用範例任務（task-388）
  - 1 個檔案變更
- `2294fa7` - 發佈 TickObject 和 SpeedyPack 範例任務（task-386~387）
  - 1 個檔案變更
- `82b9f63` - 發佈 6 個 PluginCapability 範例任務（task-380~385）
  - 1 個檔案變更
- `588539b` - 發佈 PluginCapability 宣告式權限系統任務（task-379）
  - 1 個檔案變更
- `37f9c23` - 更新解決方案和專案檔案參照
  - 8 個檔案變更
- `e1f7892` - 發佈 12 個 PluginDemo 待領取任務（task-367~378）
  - 3 個檔案變更
- `87ae858` - 建立 PluginDemo 外掛程式正反例任務註冊（task-367）
  - 2 個檔案變更
- `f77a102` - 歸檔 2026-05-26 的 sessions 和 changes
  - 7 個檔案變更

## [Alpha-0.2] - 2026-05-16

### 2026-05-16

#### 發佈準備
- `476d839` - 新增 alpha-0.2 發佈任務
  - 建立 task-114（CHANGELOG 編寫）和 task-115（版本號更新）
  - 1 個檔案變更

### 2026-05-15

#### 基礎設施
- `672627b` - 新增 Gitee 同步工作流程（帶權限設定）
  - 更新 sync-from-gitee.yml 工作流程權限設定
  - 1 個檔案變更，7 行新增，4 行刪除

- `3cd5256` - 新增 GitHub Actions 自動同步 Gitee 程式碼
  - 新增 sync-from-gitee.yml 工作流程
  - 1 個檔案變更，50 行新增

#### 文件更新
- `aa1d2ad` - 更新全部 11 語言 README/架構/入門文件，體現 SiliconLife.Fast 多平台支援 (ref task-112, task-113)
  - 修正文件中 SiliconLife.Fast 僅 Windows 的描述，體現實際多平台支援（Windows / macOS / Linux）
  - 更新 11 種語言的 README.md、architecture.md、getting-started.md
  - SelectComponent 新增 hint 屬性支援
  - ConfigView 列舉下拉式清單傳入 hint
  - 11 種語言在地化新增 SelectSearchHint 鍵值
  - 53 個檔案變更，690 行新增，194 行刪除

#### 任務系統
- `3329f3d` - 新增任務系統巡檢機制 + 在地化 Bug 修復任務
  - 建立 task-113：修復關於頁面在地化問題
  - 更新 task-112：更新 Fast 版本文件支援 Linux
  - 歸檔已完成任務（11 個）到 .ai-collab/archive/
  - 巡檢機制設定完成：快速巡檢（每 30 分鐘）+ 全量巡檢（每天 06:00）
  - 2 個檔案變更，148 行新增，171 行刪除

#### 協作框架
- `6038e22` - 註冊 coze-agent 到 .ai-collab 協作註冊表
  - 新增扣子平台常駐 AI 註冊資訊
  - 1 個檔案變更

### 2026-05-14

#### AI 協作框架
- `7344fbb` - 移除 handoff 模式，改為任務清單驅動 (v2.0)
  - 重構 .ai-collab 目錄結構，從 handoff 交接模式改為任務清單驅動
  - 新增 tasks.json 任務清單核心檔案
  - 新增 activity.log 操作日誌
  - 新增 changes/ 和 sessions/ 目錄

- `589a48e` - 新增 .ai-collab 會話記錄
  - 新增 AI 協作會話狀態記錄

- `5481bcf` - 註冊 Qoder AI IDE 到協作註冊表
  - 新增 Qoder AI 程式設計助手註冊資訊

- `e2d7b61` - 補充 tasks.json relatedCommit 和 changes commitHash
  - 完善任務中繼資料關聯

- `a087f0c` - 驗收 task-101~110 全部任務
  - 確認 10 個任務修復全部完成

#### Bug 修復
- `fac9435` - 完成 task-101~110 全部 10 個任務修復與實作
  - 修復搜尋選擇元件缺少提示文字
  - 修復關於頁面在地化問題
  - 修復說明系統搜尋 JS 錯誤
  - 39 個檔案變更，684 行新增，121 行刪除

- `c46dfbc` - 完成所有待辦任務 (task-001~006)
  - 完成初始 6 個待辦任務

- `ec176b2` - 覆蓋任務清單 - 程式碼審查發現 10 個新 bug
  - 建立 task-101~110 共 10 個新任務

#### 重構
- `ab15915` - 統一版權頭 + 修復 HelpController BOM 和 HelpView 搜尋 JS
  - 統一所有 C# 原始檔 Apache 2.0 版權頭
  - 修復 HelpController BOM 編碼問題
  - 修復 HelpView 搜尋 JavaScript 錯誤

#### 新功能
- `18a6f5d` - 建立 MCP 瀏覽器能力伺服器 (ref task-111)
  - 新增 SiliconLife.McpServer 專案
  - 實作 Playwright 瀏覽器自動化 MCP 伺服器

- `9eb251a` - 移除 SiliconLife.McpServer 模組 (ref task-111)
  - 移除獨立 MCP 伺服器，功能已整合到主專案

### 2026-05-13

#### 在地化
- `7a62590` - 新增波蘭語在地化支援
  - 新增 pl-PL 波蘭語在地化實作（PlPL.cs，1089 行）
  - 新增波蘭語說明文件在地化（HelpLocalizationPlPL.cs，3972 行）
  - 新增波蘭語中國歷史日曆支援（ChineseHistoricalPlPL.cs，600 行）
  - 新增波蘭語系統匣在地化（TrayPlPL.cs，135 行）
  - 新增波蘭語完整文件集（15 個文件）
  - Language 列舉新增波蘭語
  - 35 個檔案變更，14379 行新增，11 行刪除

- `51f9c8e` - 更新文件中的 Ark AI 參考和術語改進
  - 更新多語言文件中的 AI 用戶端術語

- `7587c12` - 為所有語言新增變更日誌條目
  - 同步更新所有語言版本的 changelog

#### 視窗系統遷移
- `b49a07d` - 遷移到 Avalonia 視窗常駐模式
  - 移除 Windows Forms 相依，完全遷移到 Avalonia UI 框架
  - 狀態視窗在 Linux 上正常顯示（遠端桌面驗證）
  - 新增視窗控制：右鍵選單、雙擊開啟 Web、關閉按鈕
  - 新增多 AI 協作框架 (.ai-collab/)
  - 修復系統匣圖示初始化（優雅降級）
  - 新增 App.axaml 和 App.cs Avalonia 應用入口
  - 13 個檔案變更，1442 行新增，541 行刪除

- `d335aaf` - Linux 平台視窗始終顯示 + 關閉確認對話方塊
  - Linux 上自動顯示狀態視窗（無系統匣圖示）
  - Linux 上關閉視窗時彈出確認對話方塊
  - Windows/macOS 保持原有系統匣行為
  - 支援 --no-tray 參數強制停用系統匣
  - 新增 ShowMessageBoxAsync 方法用於確認對話方塊
  - 3 個檔案變更，206 行新增，29 行刪除

#### 系統匣重構
- `841d384` - 重構系統匣系統並初始化 AI 協作框架
  - 精簡 TrayLocalizationBase 移除未使用屬性
  - 新增 ShowStatus 在地化項目
  - App.cs 新增系統匣圖示點擊顯示狀態視窗、在地化選單項目
  - Program.cs 將系統匣圖示初始化移至 StartAsync
  - TrayStatusWindow 關閉時隱藏而非結束
  - 註冊 trae-glm5 和 catpaw 至 .ai-collab 協作框架
  - 更新 .gitignore 確保 .ai-collab 所有檔案均被追蹤
  - 22 個檔案變更，178 行新增，1226 行刪除

#### 文件
- `43653bc` - 更新儲存庫說明和 AI 註冊表
  - 更新專案 README 和 .ai-collab 註冊資訊

### 2026-05-12

#### 任務系統 Web 檢視
- `0891b3c` - 新增任務執行詳情和歷史檢視
  - 新增 TaskExecutionDetailView 任務執行詳情檢視
  - 新增 TaskExecutionHistoryView 任務執行歷史檢視
  - TaskController 新增執行詳情和歷史查詢介面
  - 新增 TaskViewModel 任務檢視模型
  - TaskCenter 任務中心增強
  - TaskSystem 任務系統更新
  - 9 種語言在地化新增任務相關鍵值
  - 26 個檔案變更，803 行新增，55 行刪除

### 2026-05-11

#### Web 元件架構重構
- `5e687ad` - 將元件渲染從字串遷移到 H-tree
  - ComponentBase 渲染方法從字串模式遷移到 H-tree 結構
  - 所有 28 個元件適配新渲染架構（A、Accordion、Button、Calendar、Card、Chart 等）
  - SelectComponent 大幅重構（889 行改進）
  - 控制器和檢視同步更新
  - 33 個檔案變更，667 行新增，435 行刪除

- `bfd332d` - 將 Style 從字串遷移到 CssBuilder 內聯樣式
  - 新增 CssBuilder 樣式建構器
  - ComponentBase 樣式系統從字串遷移到結構化 CssBuilder
  - LoadingComponent 大幅增強（103 行新增）
  - ConfigController、LogController、MemoryController 控制器樣式遷移
  - ChatView、ConfigView、LogView、MemoryView 檢視樣式遷移
  - 37 個檔案變更，351 行新增，157 行刪除

#### 儲存系統最佳化
- `d67a7ee` - 最佳化 QueryLatest 大型資料集查詢
  - SpeedyTimeStorage QueryLatest 方法效能最佳化
  - SpeedyLoggerProvider 日誌提供者增強
  - 2 個檔案變更，44 行新增，5 行刪除

#### 日曆系統重構
- `9629f88` - 提取 TimerExecution 並增強定時器 Web 檢視
  - TimerSystem 提取 TimerExecution 邏輯（175 行移除）
  - SelectComponent 大幅增強（427 行改進）
  - TimerController 和定時器檢視增強
  - ContextManager 上下文管理器更新
  - 12 個檔案變更，458 行新增，267 行刪除

#### 在地化
- `5d8ca79` - 新增 LogsLoading 在地化鍵值
  - 9 種語言新增 LogsLoading 鍵值
  - DefaultLocalizationBase 基底類別新增定義
  - 11 個檔案變更，15 行新增

### 2026-05-10

#### 任務系統重構
- `54394f6` - 合併任務系統與聊天歷史週期
  - ProjectTaskSystem 專案任務系統大幅精簡（411 行重構）
  - TaskSystem 任務系統精簡（254 行重構）
  - TaskCenter 任務中心重構（188 行改進）
  - ContextManager 上下文管理器最佳化（347 行重構）
  - DefaultSiliconBeing 矽基生命體增強
  - TimerSystem 定時器系統整合任務
  - IWorkNoteStorage 介面更新
  - SpeedyWorkNoteStorage 和 FileSystemWorkNoteStorage 適配
  - 16 個檔案變更，648 行新增，897 行刪除

### 2026-05-09

#### Web 介面增強
- `bc50dd7` - 改進聊天檢視並新增稽核功能
  - 新增 AuditController 稽核控制器（261 行）
  - 新增 AuditView 稽核檢視（379 行）
  - 新增 AuditViewModel 稽核檢視模型
  - ChatView 聊天檢視大幅改進（171 行增強）
  - ChatController 聊天控制器更新
  - MarkdownEditorComponent 元件增強
  - InitController 初始化控制器改進
  - ChatSystem 聊天系統新增功能
  - 14 個檔案變更，1030 行新增，112 行刪除

- `c9babce` - 改進聊天檢視中的工具呼叫渲染
  - ChatView 工具呼叫塊渲染增強
  - 1 個檔案變更，54 行新增，11 行刪除

#### AI 工具場景系統
- `ff2eddd` - 實作工具場景篩選系統
  - 新增 ToolScenarioAttribute 工具場景屬性（36 行）
  - 新增 ChatOnlyAttribute 僅聊天場景屬性（19 行）
  - ToolManager 工具管理器新增場景篩選功能（40 行）
  - ContextManager 上下文管理器適配場景篩選
  - 4 個檔案變更，115 行新增，30 行刪除

- `5709a33` - 為工具類別新增場景屬性
  - 24 個工具類別新增 ToolScenario 屬性標註
  - 包括日曆、聊天、設定、策展、資料庫、磁碟、動態編譯等工具
  - 24 個檔案變更，46 行新增，20 行刪除

#### 任務系統重構
- `2f19a5f` - 使用 TaskCenter 和 TaskEnumerator 重構任務系統
  - 新增 TaskCenter 任務中心（235 行）
  - 新增 TaskEnumerator 任務列舉器（297 行）
  - TaskSystem 任務系統重構精簡
  - DefaultSiliconBeing 矽基生命體適配新架構
  - DefaultSiliconBeingFactory 工廠更新
  - SiliconBeingBase 基底類別增強
  - 7 個檔案變更，796 行新增，275 行刪除

#### 權限系統遷移
- `a06ed09` - 將 IM 和權限系統遷移到 App 專案
  - PermissionRequestQueue 從 Default/Fast 遷移到 App 專案（443 行新增）
  - 移除 Default 版本 WebUIProvider（403 行刪除）
  - 移除 Default 版本 HelpTool（194 行刪除）
  - 移除 Default/Fast 版本重複的 PermissionRequestQueue
  - 移除 Default 版本 IMPermissionAskHandler
  - PermissionRequestController 控制器更新
  - 14 個檔案變更，496 行新增，1183 行刪除

#### AI 上下文最佳化
- `4c8aaff` - 最佳化上下文管理器並增強服務定位器
  - ContextManager 上下文管理器精簡最佳化
  - ServiceLocator 服務定位器增強（36 行新增）
  - ToolManager 工具管理器增強（34 行新增）
  - DashScopeClient 和 VolcengineArkClient 用戶端改進
  - 執行器（CommandLine、Disk、Network）更新
  - 8 個檔案變更，116 行新增，98 行刪除

#### 在地化
- `5c5eef7` - 新增稽核和任務在地化鍵值
  - DefaultLocalizationBase 新增 127 行在地化定義
  - 9 種語言新增稽核和任務相關鍵值（每種 26 行）
  - 11 個檔案變更，387 行新增

#### 專案設定
- `2067db6` - 更新專案設定和 gitignore 規則
  - .gitignore 規則更新
  - DefaultConfigData 和 Fast DefaultConfigData 設定增強
  - SpeedyWorkNoteStorage 儲存改進
  - SpeedyPack 核心增強
  - 5 個檔案變更，32 行新增，6 行刪除

### 2026-05-07

#### 義大利語在地化
- `8adc18c` - 新增義大利語在地化支援並更新多語言文件
  - 新增 it-IT 義大利語在地化
  - 新增 ItIT 在地化實作（1909 行）
  - 新增 ChineseHistoricalItIT 中國歷史日曆義大利語支援（586 行）
  - 新增 TrayItIT 系統匣義大利語在地化（135 行）
  - 新增義大利語完整文件集（14 個文件：README、API 參考、架構、日曆系統、變更日誌、貢獻指南等）
  - 更新所有語言版本的架構、開發指南、入門指南等文件
  - Language 語言列舉新增義大利語
  - 86 個檔案變更，11573 行新增，769 行刪除

#### 文件同步
- `12a5deb` - 更新架構、變更日誌和矽基生命體指南的多語言文件
  - 8 種語言的 README 更新
  - 8 種語言的架構文件更新
  - 8 種語言的變更日誌更新
  - 8 種語言的矽基生命體指南更新
  - 8 種語言的工具參考更新
  - 詞彙表重構
  - 46 個檔案變更，1697 行新增，442 行刪除

### 2026-05-06

#### 大規模模組重構
- `eeb3be6` - 大規模模組重構和重組
  - SiliconLife.App 專案結構調整
  - SiliconLife.Fast 專案重組
  - SiliconLife.Default 專案重組
  - SiliconLife.Common 共享模組重組
  - SiliconLife.Core 核心模組重組
  - SiliconLife.Speedy 儲存引擎重組
  - SiliconLife.Speedy.Manager 管理工具重組
  - 119 個檔案變更，6926 行新增，3066 行刪除

### 2026-05-04

#### AI 用戶端
- `24d2c86` - 新增 VolcengineArkClient 並取代 Audit 為 Usage tracking
  - 新增 VolcengineArkClient 火山引擎 Ark AI 用戶端
  - 支援串流和非串流模式
  - 內建雙層速率控制（自我速率控制 + 伺服器速率限制）
  - 相容 OpenAI API 協定
  - Audit 系統取代為 Usage tracking
  - 24 個檔案變更，802 行新增，21 行刪除

#### 工具系統
- `f27650a` - 新增熱重載工具用於 Fast 自重啟
  - 新增 HotReloadTool 熱重載工具
  - 支援 SiliconLife.Fast 線上編譯、更新和重啟
  - 新增 HotReload.exe 獨立更新器
  - 安全檔案複製機制（不覆蓋自身）
  - 優雅關閉和連接埠釋放等待
  - 9 個檔案變更，581 行新增

#### 在地化
- `6a5aad8` - 更新所有檔案並新增法語在地化支援
  - 新增 fr-FR 法語在地化
  - 更新所有語言版本
  - 說明文件法語翻譯
  - 介面法語翻譯
  - 100+ 個檔案變更

### 2026-05-03

#### 專案基礎設施
- `2664b0c` - 更新專案基礎設施和相依
  - SiliconLife.Speedy.Manager 新增 WPF 管理介面（MainForm.Designer.cs、MainForm.resx）
  - 新增 slc.ico 圖示資源（1.5MB）
  - PluginLoader 大幅增強安全掃描（622 行新增）
  - 新增 PermissionedStreamFactory 權限流工廠（779 行）
  - 新增 PermissionRequestQueue 權限請求佇列（Default 和 Fast 版本）
  - 新增 DebugLoggerProvider 偵錯日誌提供者
  - ConfigDataBase 設定基底類別增強
  - ToolManager 新增外掛程式工具掃描功能（ScanAllPluginAssemblies）
  - SiliconBeingManager 生命週期管理增強
  - DashScopeClient 阿里雲 AI 用戶端大幅增強（227 行新增）
  - DefaultSiliconBeingFactory 工廠增強
  - Web 檢視和控制器更新（ChatView、WorkNoteView、PermissionRequestController）
  - 9 種語言在地化新增鍵值
  - 35 個檔案變更，28080 行新增，336 行刪除

### 2026-05-02

#### AI 用戶端增強
- `c16f99f` - 更新 AI 用戶端、Web UI 和儲存元件
  - DashScopeClient 阿里雲用戶端大幅改進
  - SpeedyPackAutoCompactor 自動壓縮器最佳化
  - Web 檢視基底類別和 BeingView 改進
  - 6 個檔案變更，240 行新增，81 行刪除

#### 外掛程式系統
- `242dc98` - 在關於頁面新增外掛程式清單
  - AboutController 新增外掛程式資訊展示
  - AboutViewModel 新增外掛程式資料模型
  - AboutView 新增外掛程式清單渲染
  - 9 種語言在地化新增外掛程式相關鍵值
  - 14 個檔案變更，160 行新增，1 行刪除

#### AI 最佳化
- `147f8f4` - 簡化上下文記憶提示文字
  - ContextManager 最佳化 AI 提示詞
  - 1 個檔案變更，1 行新增，1 行刪除

#### Speedy 儲存最佳化
- `8bda2d3` - 更新 Speedy 儲存和記憶控制器實作
  - SpeedyPackAutoCompactor 間隔修正
  - SpeedyTimeStorage 路徑處理最佳化
  - MemoryController 記憶控制器改進
  - SpeedyPack.Manager UI 更新
  - 4 個檔案變更，21 行新增，18 行刪除

#### 系統匣增強
- `8972654` - 增強系統匣狀態視窗的在地化支援
  - 9 種語言系統匣在地化新增 Speedy 管理入口
  - TrayStatusWindow 新增 Speedy 管理選單項目
  - 11 個檔案變更，72 行新增

#### Speedy.Manager 最佳化
- `6f5db09` - 最佳化 SpeedyPack 管理器 UI 和內部元件
  - MainForm 介面重構
  - FreeList 記憶體管理最佳化
  - WriteQueue 寫入佇列改進
  - SpeedyPack 核心最佳化
  - 5 個檔案變更，96 行新增，88 行刪除

#### 儲存系統增強
- `57f9d5d` - 改進儲存系統，新增自動壓縮和不完整日期支援
  - 新增 SpeedyPackAutoCompactor 自動壓縮定時器（30 分鐘間隔）
  - SpeedyPackRegistry 單例管理器增強
  - SpeedyStorage、SpeedyTimeStorage、SpeedyWorkNoteStorage 適配改進
  - SpeedyPack 新增 FreeList 空閒空間管理（149 行）
  - PackFileWriter 寫入器重構最佳化
  - WriteOperation、WriteQueue 寫入佇列增強
  - SpeedyPackOptions 設定選項擴充
  - IncompleteDate 新增比較方法
  - PluginLoader 外掛程式載入器改進
  - Default 和 Fast 版本 Program.cs 初始化流程更新
  - DefaultConfigData 設定資料簡化
  - KnowledgeNetwork 知識網絡精簡
  - ChatController、MemoryController 控制器最佳化
  - SpeedyPack.Manager MainForm 功能增強
  - 22 個檔案變更，639 行新增，253 行刪除

#### Speedy.Manager 更新
- `b04ed33` - 更新 Speedy.Manager 檔案

### 2026-05-01

#### 架構重構：Speedy 儲存取代 LiteDB
- `6600972` - 用 Speedy 儲存取代 LiteDB，新增外掛程式系統和 Speedy 專案
  - **新增 SiliconLife.Speedy 專案**：高效能 .spk 儲存引擎
    - SpeedyPack 核心類別（489 行）：記憶體目錄映射 + 條目快取 + 非同步寫入佇列
    - SpeedyPackOptions 設定類別：快取 TTL、最大快取條目數、唯讀模式
    - IPackTransaction 事務介面：支援原子寫入操作
    - SpkFileInfo 檔案資訊類別
    - Internal 目錄：DirectoryMap、EntryCache、PackFileReader、PackFileWriter、WriteQueue、WriteOperation、SpeedyTransaction、SpkHeader、PathNormalizer、FreeList
    - 相依 MessagePack 3.1.4 進行二進位序列化（LZ4 壓縮）
  - **新增 SiliconLife.Speedy.Manager 專案**：WPF 管理工具
    - MVVM 架構：MainViewModel、DirectoryTreeViewModel、ContentViewerViewModel 等
    - 服務層：PackService、FileDialogService、RecentFilesService、NotificationService
    - 轉換器：BoolToVisibility、ByteSizeToString、ContentTypeToIcon、NullToCollapsed
    - 檢視：MainWindow、DirectoryTreeView、ContentViewerPanel、MetadataPanel
    - 對話方塊：FileInfoDialog、ImportDialog、NewEntryDialog
  - **SiliconLife.Fast 儲存遷移**：LiteDB → SpeedyPack
    - 新增 SpeedyStorage（IStorage 適配器）
    - 新增 SpeedyTimeStorage（ITimeStorage 適配器）
    - 新增 SpeedyWorkNoteStorage（IWorkNoteStorage 適配器）
    - 新增 SpeedyPackRegistry（處理序級單例管理）
    - 新增 SpeedyPackAutoCompactor（自動壓縮定時器）
    - 移除 LiteDB 相關儲存實作（LiteDBStorage、LiteDBTimeStorage、LiteDBWorkNoteStorage、LiteDBLoggerProvider、LiteDBManager、LiteDBModels）
    - 移除 LiteDB 管理視窗相關程式碼
  - **外掛程式系統**：
    - 新增 IPlugin 介面（Core/Plugins/IPlugin.cs）
    - 新增 PluginLoader 外掛程式載入器（Core/Plugins/PluginLoader.cs）
    - 支援從目錄載入外掛程式 DLL
    - 安全掃描：禁止命名空間檢查（System.IO、System.Net、Microsoft.CodeAnalysis 等）
    - 可信組件白名單（Google.Protobuf、Newtonsoft.Json、MessagePack 等）
    - 自訂 AssemblyLoadContext 隔離載入
    - ToolManager 新增 ScanAllPluginAssemblies 方法
    - CoreHost 整合外掛程式載入器
  - 119 個檔案變更，6926 行新增，3066 行刪除

#### 矽基生命體增強
- `3aef4c3` - 新增 Stopped 活動狀態和錯誤處理改進
  - 矽基生命體新增 Stopped 狀態
  - 錯誤處理和復原機制增強

#### 在地化更新
- `513c65d` - 更新所有語言版本和文件
  - 新增 MarkdownEditorComponent 元件（625 行）
  - 新增 DetailsComponent 元件（130 行）
  - 新增 AccordionComponent 手風琴元件（285 行）
  - BeingController、ChatController、MemoryController、PermissionController 控制器更新
  - BeingView、ChatView、MemoryView、SoulEditorView 檢視重構
  - 移除舊 MarkdownEditorView
  - InitController 元件化遷移
  - 115 個檔案變更，5761 行新增，2362 行刪除

### 2026-04-30

#### 系統匣功能
- `101b203` - 實作系統匣狀態視窗和 ApplicationContext
  - 新增系統匣圖示資源（alpha.png、noWord.png、slc.ico、wordIcon.png）
  - 實作 TrayStatusWindow 狀態視窗
  - 支援 9 種語言的系統匣在地化（TrayCsCZ、TrayDeDE、TrayEnUS 等）
  - TrayLocalizationBase 抽象基底類別
  - 24 個檔案變更，27995 行新增，1 行刪除（含資源檔案）

#### 元件化 UI 架構
- `e61cfaa` - 完成元件化 UI 架構，實作 24 個元件
  - MVP 階段（8 個）：ComponentBase、Div、Span、Button、Input、Form、Select、Label
  - 第二階段（6 個）：Accordion、Card、Tabs、Table、Modal、Message
  - 第三階段（5 個）：Calendar、Tree、Chart、FileUpload、RichText
  - 新增 Js、Behavior、DomUpdate 等輔助類別
  - 25 個檔案變更，2666 行新增

- `7449e51` - 改進元件系統並新增新佈景主題
  - 增強 A、Button、Div、Form、Input 等元件
  - 新增 3 種佈景主題：HighContrast（高對比度）、Light（淺色）、Minimal（極簡）
  - 更新現有佈景主題（Admin、Chat、Creative、Dev）
  - InitController 元件化遷移
  - 32 個檔案變更，1466 行新增，1238 行刪除

- `1ba8636` - 啟動 InitController 元件化遷移（進行中）
  - 9 個檔案變更，574 行新增，145 行刪除

#### 儲存系統統一
- `895dff9` - 統一 soul.md 和 state.json 使用 IStorage 介面
  - DefaultSiliconBeing 使用 IStorage 讀寫靈魂檔案和狀態
  - 新增 StateFileManager 狀態檔案管理器
  - SoulFileManager 重構適配 IStorage
  - 8 個檔案變更，201 行新增，116 行刪除

#### LiteDB 管理增強
- `a34bef4` - 新增 LiteDBManager 並增強系統匣在地化
  - 系統匣選單新增 LiteDB 管理入口
  - 9 種語言系統匣在地化更新
  - 10 個檔案變更，196 行新增

- `c4a79ca` - 新增 LiteDB 管理視窗的語言感知在地化工廠
  - 1 個檔案變更，78 行新增

- `5ebc55e` - 將 LiteDBAdminLocalization 轉換為抽象基底類別
  - 10 個檔案變更，1356 行新增

#### 設定系統修復
- `2da5256` - 新增 ConfigExists 抽象方法並修復 LiteDB 重複設定記錄
  - ConfigDataBase 新增 ConfigExists 方法
  - Fast 版本 DefaultConfigData 實作 LiteDB 設定存在性檢查
  - 修復 LiteDB 重複設定鍵問題
  - 9 個檔案變更，210 行新增，2 行刪除

#### 聊天和檢視最佳化
- `d3618ec` - 最佳化聊天會話、儲存系統、時間模型和檢視基底類別
  - BroadcastChannel、GroupChatSession、SingleChatSession 最佳化
  - ITimeStorage 新增查詢方法
  - FileSystemStorage 和 LiteDBStorage 同步更新
  - ViewBase 重構最佳化（Default 和 Fast 版本）
  - 11 個檔案變更，622 行新增，392 行刪除

### 2026-04-29

#### 架構重構：共享模組提取
- `a102428` - 將共享模組從 SiliconLife.Default 遷移到 SiliconLife.Common
  - 提取 32 種日曆實作到 Common 專案
  - 提取在地化基底類別及 21 種語言實作到 Common 專案
  - 提取權限管理器、預設矽基生命體實作到 Common 專案
  - 提取 23 個內建工具實作到 Common 專案
  - 提取 Playwright WebView 實作到 Common 專案
  - 更新命名空間為 SiliconLife.Collective
  - 122 個檔案變更，586 行新增，343 行刪除

#### 程式碼品質改進
- `17566fe` - 將 Core、Common 和 Default 專案中的 Console.WriteLine 取代為日誌系統
  - ContextManager、AuditLogger、DefaultConfigData 等 6 個檔案更新
  - 統一使用 ILogger 介面，提升程式碼可維護性
  - 6 個檔案變更，12 行新增，8 行刪除

#### SiliconLife.Fast 高效能版本
- `54a0307` - 新增 SiliconLife.Fast 專案並完成編譯修復
  - 完整的 Windows 表單應用程式進入點
  - 系統匣支援（NotifyIcon）
  - 移植全部 Web UI 控制器（20+ 個）
  - 移植全部 Web 檢視元件
  - 移植 4 種佈景主題（Admin、Chat、Creative、Dev）
  - 125 個檔案變更，61186 行新增

#### 多語言文件同步
- `265fde8` - 將雙版本架構文件同步到所有語言
  - 更新 7 種語言的 architecture.md、changelog.md
  - 更新 6 種語言的 contributing.md
  - 更新 7 種語言的 getting-started.md、roadmap.md
  - 47 個檔案變更，1214 行新增，38 行刪除

#### LiteDB 儲存系統（Fast 版本）
- `4704862` - 新增 LiteDB 相依和基礎設施
  - 新增 LiteDBManager 管理類別
  - 新增 LiteDBModels 資料模型
  - 3 個檔案變更，252 行新增

- `4220036` - 實作 LiteDB 儲存類別
  - LiteDBStorage：實作 IStorage 介面
  - LiteDBTimeStorage：實作 ITimeStorage 介面
  - LiteDBWorkNoteStorage：實作 IWorkNoteStorage 介面
  - 3 個檔案變更，581 行新增

- `38ebd23` - 將設定和日誌系統遷移到 LiteDB
  - DefaultConfigData 適配 LiteDB 儲存
  - 新增 LiteDBLoggerProvider 日誌提供者
  - 2 個檔案變更，203 行新增，67 行刪除

- `e687157` - 將知識網絡從檔案系統遷移到 LiteDB
  - KnowledgeNetwork 全面重構，使用 LiteDB 儲存三元組資料
  - 1 個檔案變更，231 行新增，72 行刪除

- `4220169` - 將 LiteDB 儲存整合到 Program 和 ProjectManager
  - Program.cs 初始化 LiteDB 儲存
  - ProjectManager 適配 LiteDB 工作筆記儲存
  - 2 個檔案變更，40 行新增，17 行刪除

- `5f3a709` - 移除廢棄的檔案系統儲存實作
  - 刪除 FileSystemLoggerProvider、FileSystemStorage、FileSystemTimeStorage 等
  - 6 個檔案變更，1518 行刪除

- `e1a4ef2` - docs: add v0.1.0-alpha version identifier to all documentation
  - 127 個檔案變更，2297 行新增，2471 行刪除

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### 儲存系統重構
- `8dd26e3` - 統一 ITimeStorage 介面使用 IncompleteDate 並新增分級查詢 API
  - 移除 ITimeStorage 介面中的 DateTime 多載方法，統一使用 IncompleteDate
  - IncompleteDate 新增 CompareTo(DateTime) 比較方法和 Expand() 展開方法
  - 新增 GetEarliestTimestamp()、GetLatestTimestamp() 分級查詢 API
  - 新增 HasSummary() 和 QueryWithLevel() 方法，支援按時間層級查詢
  - Memory.cs 重構壓縮演算法，使用新的分級查詢 API 提升效率
  - FileSystemTimeStorage.cs 完整實作新的介面方法
  - 同步更新所有呼叫方：ChatSystem、ChatSession、BroadcastChannel、AuditLogger、TokenUsageRecord 等
  - 工具系統更新：HelpTool、LogTool、TokenAuditTool 適配新介面
  - Web 控制器更新：AuditController、ChatController、ChatHistoryController 適配新介面
  - 41 個檔案變更，1820 行新增，903 行刪除

### 2026-04-27

#### 說明文件系統增強
- `9989d79` - 更新在地化、說明系統和 Web 檢視
  - 新增 IAIClientFactoryHelp.cs AI 用戶端工廠說明文件介面
  - 完成全部說明文件的 9 種語言翻譯
  - HelpTopics.cs 新增 40 個說明主題定義
  - Web 檢視全面更新：InitController、AuditView、ConfigView、KnowledgeView、LogView 等
  - 在地化系統增強：所有語言版本新增新的在地化鍵
  - AI 用戶端工廠更新：DashScopeClientFactory、OllamaClientFactory 改進
  - 30 個檔案變更，10086 行新增，15 行刪除

#### 說明文件新增內容
- `e7afe94` - 新增靈魂檔案和稽核日誌說明文件
  - 新增靈魂檔案管理說明文件
  - 新增稽核日誌說明文件
  - HelpTopics.cs 新增主題定義
  - HelpView.cs 大幅重構，改進文件渲染邏輯
  - PermissionView.cs 重構，改進權限管理介面
  - 核心模組增強：SiliconBeingManager、TaskSystem、ToolManager 改進
  - TaskTool.cs 重構，改進任務管理功能
  - Web 檢視全面更新：所有檢視元件同步更新
  - HelpController.cs 簡化，最佳化控制器邏輯
  - 30 個檔案變更，7100 行新增，897 行刪除

### 2026-04-26

#### 說明文件系統
- `07895d7` - 增強說明文件系統，新增 3 個文件並完成 9 種語言翻譯
  - 新增記憶系統、Ollama 安裝設定、阿里雲百煉平台使用指南
  - 完成全部 10 個說明文件的 9 種語言翻譯
  - 簡化 HelpView 渲染邏輯
  - 18 個檔案變更，14418 行新增，1364 行刪除

#### 德語在地化
- `0cfd8a1` - 新增完整的德語 (de-DE) 在地化支援
  - 完整的德語在地化檔案
  - 新增中國歷史日曆德語支援
  - 新增說明文件德語翻譯
  - 完整同步 9 種語言的所有文件
  - 135 個檔案變更，26186 行新增，14371 行刪除

#### 文件同步
- `3aada7d` - 同步繁體中文 (zh-HK) 文件與簡體中文保持一致
  - 3 個檔案變更，519 行新增，422 行刪除
- `2f6abff` - 為所有語言新增說明工具顯示名稱在地化
  - 7 個檔案變更，47 行新增，7 行刪除

#### 知識系統重構
- `60944fe` - 統一命名空間到 SiliconLife.Collective
  - 8 個檔案變更，5 行新增，8 行刪除
- `69c51c5` - 新增說明文件系統並將程式碼註解翻譯為英文
  - 29 個檔案變更，3385 行新增，22 行刪除

### 2026-04-25

#### WebView 瀏覽器自動化
- `41757c3` - 實作基於 Playwright 的跨平台 WebView 瀏覽器自動化
  - 6 個檔案變更，1152 行新增

#### 文件更新
- `0ff797b` - 新增 KnowledgeTool 和 WorkNoteTool 文件（7 種語言）
  - 28 個檔案變更，4983 行新增
- `ad77415` - 更新所有 changelog 檔案，新增 2026-04-25 Git 歷史記錄
  - 7 個檔案變更，168 行新增

#### 專案工作區管理
- `785c551` - 實作專案工作區管理，包含工作筆記和任務系統
  - 新增專案工作區管理系統
  - 工作筆記功能，用於追蹤專案進度
  - 任務管理系統整合
  - 29 個檔案變更，4256 行新增，36 行刪除

#### 捷克語在地化
- `b4bbf39` - 新增完整的捷克語 (cs-CZ) 在地化並更新所有語言文件
  - 116 個檔案變更，4933 行新增，222 行刪除
- `faf078f` - 修復捷克語在地化編譯錯誤
  - 3 個檔案變更，910 行新增，1 行刪除

#### 知識系統增強
- `20adaac` - 新增 KnowledgeTool 並支援完整在地化
  - 34 個檔案變更，2331 行新增，56 行刪除

### 2026-04-24

#### 記憶管理系統增強
- `c7b2ecc` - 增強記憶管理功能，新增進階篩選、統計和詳情檢視功能
  - 新增記憶進階篩選功能
  - 實作記憶統計功能
  - 新增記憶詳情檢視頁面
  - 多語言在地化支援（6 種語言）
  - 13 個檔案變更，840 行新增，86 行刪除

#### 權限系統擴充
- `4489ad6` - 將 wttr.in 天氣服務新增到網路白名單
  - 完整的多語言文件同步更新（6 種語言）
  - 14 個檔案變更，417 行新增，1 行刪除

#### Web 介面修復
- `d9d72e9` - 修復工作筆記詳情強制回應視窗 CSS 優先順序問題
  - 19 個檔案變更，1744 行新增，6 行刪除

#### 聊天歷史最佳化
- `0df599c` - 修復工具結果被渲染為獨立聊天訊息的問題
  - 1 個檔案變更，222 行新增，21 行刪除
- `057b09d` - 最佳化聊天歷史詳情顯示，改進工具呼叫渲染
  - 3 個檔案變更，389 行新增，68 行刪除

#### 定時器執行歷史
- `fa3f06f` - 新增定時器執行歷史功能，包含詳情檢視
  - 8 個檔案變更，937 行新增，10 行刪除
- `d824835` - 新增定時器執行歷史在地化鍵（所有語言）
  - 7 個檔案變更，88 行新增

#### 在地化增強
- `c13cb17` - 註冊西班牙語語言變體
  - 1 個檔案變更，4 行新增
- `9c44f34` - 新增中國歷史日曆多語言在地化支援
  - 16 個檔案變更，6049 行新增，1 行刪除

#### 核心功能改進
- `1e7c7b2` - 改進記憶壓縮和工具執行追蹤
  - 4 個檔案變更，338 行新增，86 行刪除

### 2026-04-23

#### 工具在地化
- `192fc6e` - 為 5 個工具新增缺失的工具名稱在地化
  - 6 個檔案變更，30 行新增

#### 文件更新
- `882c08f` - 更新所有 changelog 檔案，新增完整 Git 歷史記錄並移除虛假版本號
  - 45 個檔案變更，8815 行新增，1611 行刪除

#### 聊天頁面增強
- `65c157b` - 為聊天頁面新增載入指示器並自動選擇主理人會話
  - 10 個檔案變更，211 行新增，7 行刪除

#### 聊天歷史功能
- `e483348` - 實作矽基生命體聊天歷史檢視功能
  - 新增 ChatHistoryController
  - 建立 ChatHistoryViewModel
  - 實作 ChatHistoryListView 和 ChatHistoryDetailView 頁面
  - 新增聊天歷史的在地化鍵（5 種語言）
  - 12 個檔案變更，1178 行新增

#### AI 流控制增強
- `30a2d4e` - 增強 AI 流取消、IM 整合和核心主機初始化
  - 11 個檔案變更，387 行新增，12 行刪除

#### 聊天訊息佇列
- `db48c51` - 新增聊天訊息佇列、檔案中繼資料和流取消支援
  - 4 個檔案變更，357 行新增

#### 檔案上傳支援
- `28fb344` - 實作檔案來源對話方塊和檔案上傳支援
  - 3 個檔案變更，1100 行新增，2 行刪除
- `1d3e2cc` - 新增檔案來源對話方塊在地化字串（6 種語言）
  - 6 個檔案變更，30 行新增

#### 文件更新
- `8111e92` - 在 README 的儲存庫部分新增 Wiki 連結
  - 1 個檔案變更，3 行新增，1 行刪除

### 2026-04-22

#### 文件在地化
- `66c11eb` - 將中文註解翻譯為英文並更新所有 changelog
  - 11 個檔案變更，373 行新增，163 行刪除

#### SSE 訊息增強
- `b574b2b` - 為歷史訊息新增 senderName 用於 AI 識別
  - 1 個檔案變更，9 行新增

#### 聊天功能
- `601fc14` - 新增 mark_read 操作，用於會話結束標記
  - 7 個檔案變更，196 行新增，36 行刪除

#### 工具系統最佳化
- `7a03a19` - 改進 LogTool 對話查詢靈活性
  - 1 個檔案變更，57 行新增，24 行刪除

#### 在地化增強
- `0a8d750` - 新增主動矽基生命體行為的通用系統提示
  - 8 個檔案變更，460 行新增，48 行刪除

#### 日誌系統重構
- `2b771f3` - 解耦 LogController 與檔案 I/O，新增日誌讀取 API
  - 4 個檔案變更，172 行新增，137 行刪除
- `12da302` - 為日誌檢視新增矽基生命體篩選器
  - 9 個檔案變更，147 行新增，10 行刪除
- `8f6cb1e` - 為 ILogger 介面新增 beingId 參數，實作系統/矽基生命體日誌分離
  - 47 個檔案變更，524 行新增，490 行刪除

#### 權限系統改進
- `4c747ad` - 重構 PermissionTool、ExecuteCodeTool，新增 EvaluatePermission API
  - 18 個檔案變更，680 行新增，492 行刪除

#### Bug 修復
- `1c96e99` - 修復 search_files 和 search_content 根目錄搜尋失敗
  - 1 個檔案變更，98 行新增，41 行刪除

#### 工具整合
- `135710d` - 移除 SearchTool，將本地搜尋移至 DiskTool
  - 2 個檔案變更，185 行新增，365 行刪除

#### 工具系統擴充
- `70ce7fb` - 實作 DatabaseTool 用於結構化資料庫查詢
  - 1 個檔案變更，382 行新增
- `be29a09` - 實作 LogTool 用於操作和對話歷史查詢
  - 1 個檔案變更，298 行新增
- `4ea7702` - 實作 PermissionTool 用於動態權限管理
  - 1 個檔案變更，457 行新增
- `1384ff4` - 實作 ExecuteCodeTool 用於多語言程式碼執行
  - 1 個檔案變更，477 行新增
- `82d1e11` - 實作 SearchTool 用於資訊檢索
  - 1 個檔案變更，363 行新增

#### Web 介面最佳化
- `0675c45` - 最佳化預覽窗格中的 markdown 程式碼區塊標示
  - 1 個檔案變更，4 行新增，23 行刪除
- `702b3f3` - 增強任務檢視，新增狀態徽章和中繼資料展示
  - 8 個檔案變更，221 行新增，9 行刪除
- `6ed9a79` - 改進聊天訊息儲存和檢視渲染
  - 8 個檔案變更，140 行新增，29 行刪除

### 2026-04-21

#### Bug 修復
- `c6b518b` - 修復定時器訊息傳遞和聊天訊息儲存
  - 3 個檔案變更，297 行新增，124 行刪除

#### 設定管理
- `4305769` - 新增 .gitattributes 用於行尾管理
  - 1 個檔案變更，32 行新增

#### Web 介面改進
- `188c6f8` - 註冊任務清單 API 路由並新增空狀態顯示
  - 2 個檔案變更，35 行新增，2 行刪除
- `634e8ca` - 新增權限頁面返回清單連結
  - 1 個檔案變更，16 行新增
- `6ba591d` - 新增獨立 AI 設定編輯器用於矽基生命體
  - 11 個檔案變更，842 行新增，18 行刪除
- `0a826f5` - 在程式碼編輯器中新增儲存成功提示
  - 1 個檔案變更，9 行新增，2 行刪除
- `2940373` - 增強 Web 介面，新增程式碼懸浮提示和 UI 改進
  - 11 個檔案變更，1054 行新增，75 行刪除

#### 權限系統修復
- `592c7ab` - 修復回呼實例化和註冊順序
  - 2 個檔案變更，38 行新增，7 行刪除

#### 安全增強
- `833ead2` - 為動態編譯新增組件參考驗證
  - 4 個檔案變更，135 行新增，8 行刪除

#### 權限系統增強
- `5879621` - 新增權限回呼預編譯驗證和增強錯誤處理
  - 21 個檔案變更，617 行新增，26 行刪除

#### 文件更新
- `4dbf659` - 更新 changelog 到 v0.5.1，取代 GitHub 佔位符 URL，新增 Gitee 映像，按語言在地化 Bilibili 名稱，更新信箱
  - 32 個檔案變更，489 行新增，180 行刪除

#### 設定與入口
- `0fc1693` - 更新程式入口和專案設定
  - 2 個檔案變更，7 行新增

#### 權限系統重構
- `ea9179a` - 改進權限系統實作
  - 5 個檔案變更，358 行新增，152 行刪除

#### Bug 修復
- `928a96d` - 修復日曆計算實作
  - 4 個檔案變更，12 行新增，12 行刪除

#### AI 與日曆
- `646813e` - 改進 AI 用戶端工廠實作
  - 2 個檔案變更，21 行新增，20 行刪除

#### 在地化
- `7940d9c` - 新增韓語在地化支援
  - 7 個檔案變更，2424 行新增，10 行刪除
- `4ff98ad` - 重構文件，支援多語言
  - 81 個檔案變更，23818 行新增，1886 行刪除

### 2026-04-20

#### 核心功能完善
- `28905b5` - 完整的多語言支援、AI 用戶端工廠、權限系統和在地化設定
  - 帶管理器、條目和不同日誌等級的日誌系統
  - 用於查詢和追蹤 token 使用的 token 稽核系統
  - 自動發現不同 AI 平台的 AI 用戶端工廠
  - 帶自己儲存的權限回呼系統
  - 主控台日誌器實作
  - 英語和簡體中文的多語言支援
  - 帶 WebSocket 的 WebUI 信使，用於即時聊天
  - 使用在地化增強預設矽基生命體
  - 39 個檔案變更，4670 行新增，175 行刪除

### 2026-04-19

#### 定時器與日曆
- `c933fd8` - 更新在地化、定時器系統、Web 檢視並新增工具
  - 更好的在地化管理器
  - 定時任務的排程系統
  - AI 設定和上下文管理
  - 支援 32 種日曆類型的日曆工具
  - 用於日曆 API 的 Web 控制器
  - 任務管理工具
  - 46 個檔案變更，4018 行新增，975 行刪除

**架構改進**
- 重新設計 Web 檢視架構以更好地支援佈景主題
- 改進生命體管理系統，具有更好的狀態處理

### 2026-04-18

- `9f585e1` - 更新在地化、定時器系統、Web 檢視並新增工具
  - 定時器和排程改進
  - 帶改進 UI 元件的更好 Web 檢視
  - 更多工具實作
  - 57 個檔案變更，3328 行新增，389 行刪除

### 2026-04-17

- `9b71fcd` - 更新核心模組，新增 zh-HK 文件、廣播頻道、設定工具和稽核 Web 檢視
  - 廣播頻道，用於多個矽基生命體一起聊天
  - 設定工具系統
  - 稽核 Web 檢視
  - 繁體中文文件
  - 42 個檔案變更，3533 行新增，268 行刪除

### 2026-04-16

- `5040f05` - 更新核心和預設模組
  - 模組最佳化和 bug 修復
  - 實作更新和改進
  - 58 個檔案變更，9916 行新增，111 行刪除

### 2026-04-15

- `3efab5f` - 更新多個模組：AI、Chat、IM、Tools、Web、Localization、Storage
  - AI 用戶端改進
  - 聊天系統增強
  - 信使提供者更新
  - 工具系統最佳化
  - Web 基礎設施改進
  - 在地化最佳化
  - 儲存系統更新
  - 33 個檔案變更，788 行新增，232 行刪除

### 2026-04-14

- `4241a2f` - 聊天功能基本完成，UI 上傳最佳化
  - 聊天系統功能完成
  - 檔案上傳的 UI 最佳化
  - 16 個檔案變更，1234 行新增，102 行刪除

### 2026-04-13

- `c498c31` - 程式碼更新
  - 通用程式碼改進和最佳化
  - 32 個檔案變更，1045 行新增，546 行刪除

### 2026-04-12

#### 文件與在地化
- `2161002` - 重構文件並增強在地化
  - 17 個檔案變更，982 行新增，92 行刪除
- `03d94e4` - 增強設定系統和在地化
  - 25 個檔案變更，1378 行新增，154 行刪除
- `9976a35` - 新增關於頁面和在地化
  - 14 個檔案變更，699 行新增，44 行刪除

#### 聊天與 Web 檢視
- `0c8ccfc` - 增強聊天系統、在地化和 Web 檢視
  - 13 個檔案變更，402 行新增，56 行刪除
- `a8f1342` - 重新設計 Web 通訊層，從 WebSocket 切換到 SSE
  - 27 個檔案變更，793 行新增，935 行刪除

### 2026-04-11

#### 日誌系統
- `e8fe259` - 新增日誌系統和程式碼最佳化
  - 37 個檔案變更，624 行新增，91 行刪除
- `f01c519` - 新增日誌系統，更新 AI 介面和 Web 檢視
  - 31 個檔案變更，1758 行新增，63 行刪除

### 2026-04-10

- `4962924` - 增強 WebSocket 處理常式、聊天檢視和信使互動
  - 上下文管理器改進
  - 聊天系統增強
  - 信使提供者介面更新
  - WebUI 提供者重新設計
  - JavaScript 建構器和路由器更新
  - 聊天檢視最佳化
  - WebSocket 處理常式改進
  - 9 個檔案變更，365 行新增，134 行刪除

### 2026-04-09

- `f9302bf` - 增強信使提供者介面、聊天系統和 Web UI 互動
  - 信使提供者介面擴充
  - 聊天訊息和系統改進
  - 上下文管理器最佳化
  - 預設矽基生命體增強
  - Web UI 聊天檢視改進
  - WebSocket 處理常式更新
  - 10 個檔案變更，427 行新增，93 行刪除

### 2026-04-07

- `6831ee8` - 重新設計 Web 檢視和 JavaScript 建構器
  - 完整 Web 控制器重新設計
  - JavaScript 建構器完全重寫
  - 所有檢視元件更新
  - 佈景主題系統改進
  - 檢視基底類別架構提升
  - 23 個檔案變更，2004 行新增，1983 行刪除

### 2026-04-05

- `41e97fb` - 更新多個核心模組和 Web 控制器
  - 上下文管理器改進
  - 聊天系統和會話管理
  - 服務定位器重新設計
  - 矽基生命體基底類別和管理器更新
  - Web 控制器全面更新（17 個控制器）
  - 預設矽基生命體工廠改進
  - 31 個檔案變更，681 行新增，326 行刪除
- `67988d4` - 改進 Web UI 模組，新增執行器檢視，清理檢視和核心模組
  - 61 個檔案變更，3148 行新增，3726 行刪除

### 2026-04-04

- `b58bb1c` - 新增初始化控制器並重新設計 Web 模組
  - 初始化控制器
  - 設定模組重新設計
  - 在地化模組更新
  - 佈景主題系統改進
  - 路由器增強
  - 29 個檔案變更，1269 行新增，289 行刪除
- `f03ac0b` - 新增 Web UI 模組，改進信使功能
  - 60 個檔案變更，8481 行新增，165 行刪除

### 2026-04-03

- `192e57b` - 更新專案結構和核心執行時期元件
  - 22 個檔案變更，446 行新增，179 行刪除
- `59faec8` - 核心和預設實作更新
  - 25 個檔案變更，3056 行新增，18 行刪除
- `d488485` - 新增動態編譯功能和主理人工具模組
  - 19 個檔案變更，1727 行新增，11 行刪除
- `753d1d9` - 新增安全模組，更新執行器、信使提供者、在地化和工具
  - 29 個檔案變更，2352 行新增，93 行刪除
- `a378697` - 完成階段 5 - 工具系統 + 執行器
  - 41 個檔案變更，2651 行新增，363 行刪除

### 2026-04-02

- `e6ad94b` - 修復測試期間刪除設定檔案時聊天歷史載入失敗的問題
  - 4 個檔案變更，49 行新增，45 行刪除
- `daa56f5` - 完成階段 4：持久化記憶（聊天系統 + 信使頻道）
  - 29 個檔案變更，2051 行新增，538 行刪除

### 2026-04-01

- `bbe2dbb` - 修復設定載入和聊天服務訊息路由
  - 27 個檔案變更，1633 行新增，147 行刪除
- `2fa6305` - 實作階段 2：主迴圈框架和時鐘物件系統
  - 9 個檔案變更，594 行新增，41 行刪除
- `32b99a1` - 實作階段 1 - 基本聊天功能
  - 19 個檔案變更，1185 行新增
- `358e368` - 初始提交：專案文件和授權
  - 10 個檔案變更，1873 行新增
