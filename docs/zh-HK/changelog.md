# 變更日誌

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | **繁體中文** | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md)

本專案的所有重要更改都將記錄在此檔案中。

格式基於 [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)，
本專案遵循 [語義化版本控制](https://semver.org/spec/v2.0.0.html)。

---

## 關於此變更日誌

### 專案雙版本

本專案提供兩個實現版本：

- **SiliconLife.Default**：預設實現，主要用於驗證架構可行性。主控台應用程式，檔案系統 JSON 儲存。
- **SiliconLife.Fast**：主推生產版本。Windows 窗體應用程式，記憶體儲存 + 非同步持久化，經過深度效能最佳化。

兩個版本共享相同的介面和功能，僅在儲存實現和執行模式上有所不同。SiliconLife.Default 作為架構驗證基準，SiliconLife.Fast 作為生產環境主推版本。

### 專案起源

- 本專案起源於 2026 年 3 月 20 日。
- 在此專案之前，有一個驗證 Demo 因架構設計不合理而失敗，導致無法與多個 AI 平台整合。

### 使用的 AI IDE 工具

#### Kiro（Amazon AWS）
- 專案最初由 Kiro 維護，並使用 Spec 模式啟動。
- Kiro 是 Amazon AWS 構建的 agentic AI 開發環境。
- 基於 Code OSS（VS Code），支援 VS Code 設定和 Open VSX 相容外掛。
- 具有規格驅動的開發工作流程，用於結構化 AI 編碼。

#### Comate AI IDE / 文心快碼（百度）
- 偶爾用於文案和文件工作。
- Comate AI IDE 是百度文心於 2025 年 6 月 23 日發布的 AI 原生開發環境工具。
- 行業首個多模態、多智慧體協同的 AI IDE。
- 功能包括設計到程式碼轉換和全流程 AI 輔助編碼。
- 由百度文心 4.0 X1 Turbo 模型驅動。

#### Trae（字節跳動）
- 2025 年 10 月至 2026 年 4 月期間使用。
- AI IDE，支援智能程式碼生成和專案管理。

#### Qoder（阿里巴巴）
- 自 2026 年 4 月 18 日起用於專案維護。
- AI 編碼平台，支援程式碼分析、文件生成和多智能體協作。

#### CatPaw（美團）
- 自 2026 年 5 月 6 日起與 Qoder 混合使用。
- 基於美團自研 LongCat 系列模型，具有強大的全程式碼架構重構能力。

### 需求文件

- 本專案的需求文件未公開。
- 需求經過 12 多個國際 AI 平台和大型模型系列的反覆驗證，產生了超過 2000 行、幾乎人類無法理解的使用者故事驅動需求文件。

---

## [未發布]

### 2026-05-12

#### 任務系統 Web 視圖
- `0891b3c` - 添加任務執行詳情和歷史視圖
  - 新增 TaskExecutionDetailView 任務執行詳情視圖
  - 新增 TaskExecutionHistoryView 任務執行歷史視圖
  - TaskController 新增執行詳情和歷史查詢介面
  - TaskViewModel 新增任務視圖模型
  - TaskCenter 任務中心增強
  - TaskSystem 任務系統更新
  - 9 種語言本地化新增任務相關鍵值
  - 26 個檔案變更，803 行新增，55 行刪除

### 2026-05-11

#### Web 元件架構重構
- `5e687ad` - 將元件渲染從字串遷移到 H-tree
  - ComponentBase 渲染方法從字串模式遷移到 H-tree 結構
  - 所有 28 個元件適配新渲染架構（A、Accordion、Button、Calendar、Card、Chart 等）
  - SelectComponent 大幅重構（889 行改進）
  - 控制器和視圖同步更新
  - 33 個檔案變更，667 行新增，435 行刪除

- `bfd332d` - 將 Style 從字串遷移到 CssBuilder 行內樣式
  - 新增 CssBuilder 樣式構建器
  - ComponentBase 樣式系統從字串遷移到結構化 CssBuilder
  - LoadingComponent 大幅增強（103 行新增）
  - ConfigController、LogController、MemoryController 控制器樣式遷移
  - ChatView、ConfigView、LogView、MemoryView 視圖樣式遷移
  - 37 個檔案變更，351 行新增，157 行刪除

#### 儲存系統最佳化
- `d67a7ee` - 最佳化 QueryLatest 大型資料集查詢
  - SpeedyTimeStorage QueryLatest 方法效能最佳化
  - SpeedyLoggerProvider 日誌提供者增強
  - 2 個檔案變更，44 行新增，5 行刪除

#### 日曆系統重構
- `9629f88` - 提取 TimerExecution 並增強定時器 Web 視圖
  - TimerSystem 提取 TimerExecution 邏輯（175 行移除）
  - SelectComponent 大幅增強（427 行改進）
  - TimerController 和定時器視圖增強
  - ContextManager 上下文管理器更新
  - 12 個檔案變更，458 行新增，267 行刪除

#### 本地化
- `5d8ca79` - 添加 LogsLoading 本地化鍵值
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
- `bc50dd7` - 改進聊天視圖並添加稽核功能
  - 新增 AuditController 稽核控制器（261 行）
  - 新增 AuditView 稽核視圖（379 行）
  - 新增 AuditViewModel 稽核視圖模型
  - ChatView 聊天視圖大幅改進（171 行增強）
  - ChatController 聊天控制器更新
  - MarkdownEditorComponent 元件增強
  - InitController 初始化控制器改進
  - ChatSystem 聊天系統新增功能
  - 14 個檔案變更，1030 行新增，112 行刪除

- `c9babce` - 改進聊天視圖中的工具呼叫渲染
  - ChatView 工具呼叫區塊渲染增強
  - 1 個檔案變更，54 行新增，11 行刪除

#### AI 工具場景系統
- `ff2eddd` - 實現工具場景過濾系統
  - 新增 ToolScenarioAttribute 工具場景屬性（36 行）
  - 新增 ChatOnlyAttribute 僅聊天場景屬性（19 行）
  - ToolManager 工具管理器新增場景過濾功能（40 行）
  - ContextManager 上下文管理器適配場景過濾
  - 4 個檔案變更，115 行新增，30 行刪除

- `5709a33` - 為工具類別添加場景屬性
  - 24 個工具類別添加 ToolScenario 屬性標註
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
  - DashScopeClient 和 VolcengineArkClient 客戶端改進
  - 執行器（CommandLine、Disk、Network）更新
  - 8 個檔案變更，116 行新增，98 行刪除

#### 本地化
- `5c5eef7` - 添加稽核和任務本地化鍵值
  - DefaultLocalizationBase 新增 127 行本地化定義
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

#### 義大利語本地化
- `8adc18c` - 添加義大利語本地化支援並更新多語言文件
  - 新增 it-IT 義大利語本地化
  - 新增 ItIT 本地化實現（1909 行）
  - 新增 ChineseHistoricalItIT 中國歷史日曆義大利語支援（586 行）
  - 新增 TrayItIT 托盤義大利語本地化（135 行）
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

#### AI 客戶端
- `24d2c86` - 添加 VolcengineArkClient 並替換 Audit 為 Usage tracking
  - 新增 VolcengineArkClient 火山引擎 Ark AI 客戶端
  - 支援流式和非流式模式
  - 內建雙層速率控制（自我速率控制 + 伺服器速率限制）
  - 相容 OpenAI API 協議
  - Audit 系統替換為 Usage tracking
  - 24 個檔案變更，802 行新增，21 行刪除

#### 工具系統
- `f27650a` - 添加熱重載工具用於 Fast 自重啟
  - 新增 HotReloadTool 熱重載工具
  - 支援 SiliconLife.Fast 線上編譯、更新和重啟
  - 新增 HotReload.exe 獨立更新器
  - 安全檔案複製機制（不覆蓋自身）
  - 優雅關閉和連接埠釋放等待
  - 9 個檔案變更，581 行新增

#### 本地化
- `6a5aad8` - 更新所有檔案並添加法語本地化支援
  - 新增 fr-FR 法語本地化
  - 更新所有語言版本
  - 幫助文件法語翻譯
  - 界面法語翻譯
  - 100+ 個檔案變更

### 2026-05-03

#### 專案基礎設施
- `2664b0c` - 更新專案基礎設施和相依性
  - SiliconLife.Speedy.Manager 新增 WPF 管理介面（MainForm.Designer.cs、MainForm.resx）
  - 新增 slc.ico 圖示資源（1.5MB）
  - PluginLoader 大幅增強安全掃描（622 行新增）
  - 新增 PermissionedStreamFactory 權限流工廠（779 行）
  - 新增 PermissionRequestQueue 權限請求佇列（Default 和 Fast 版本）
  - 新增 DebugLoggerProvider 除錯日誌提供者
  - ConfigDataBase 設定基底類別增強
  - ToolManager 新增外掛程式工具掃描功能（ScanAllPluginAssemblies）
  - SiliconBeingManager 生命週期管理增強
  - DashScopeClient 阿里雲 AI 客戶端大幅增強（227 行新增）
  - DefaultSiliconBeingFactory 工廠增強
  - Web 視圖和控制器更新（ChatView、WorkNoteView、PermissionRequestController）
  - 9 種語言本地化新增鍵值
  - 35 個檔案變更，28080 行新增，336 行刪除

### 2026-05-02

#### AI 客戶端增強
- `c16f99f` - 更新 AI 客戶端、Web UI 和儲存元件
  - DashScopeClient 阿里雲客戶端大幅改進
  - SpeedyPackAutoCompactor 自動壓縮器最佳化
  - Web 視圖基底類別和 BeingView 改進
  - 6 個檔案變更，240 行新增，81 行刪除

#### 外掛程式系統
- `242dc98` - 在關於頁面添加外掛程式列表
  - AboutController 新增外掛程式資訊展示
  - AboutViewModel 新增外掛程式資料模型
  - AboutView 新增外掛程式列表渲染
  - 9 種語言本地化新增外掛程式相關鍵值
  - 14 個檔案變更，160 行新增，1 行刪除

#### AI 最佳化
- `147f8f4` - 簡化上下文記憶提示文字
  - ContextManager 最佳化 AI 提示詞
  - 1 個檔案變更，1 行新增，1 行刪除

#### Speedy 儲存最佳化
- `8bda2d3` - 更新 Speedy 儲存和記憶控制器實現
  - SpeedyPackAutoCompactor 間隔修正
  - SpeedyTimeStorage 路徑處理最佳化
  - MemoryController 記憶控制器改進
  - SpeedyPack.Manager UI 更新
  - 4 個檔案變更，21 行新增，18 行刪除

#### 托盤增強
- `8972654` - 增強托盤狀態視窗的本地化支援
  - 9 種語言托盤本地化新增 Speedy 管理入口
  - TrayStatusWindow 新增 Speedy 管理功能表項
  - 11 個檔案變更，72 行新增

#### Speedy.Manager 最佳化
- `6f5db09` - 最佳化 SpeedyPack 管理器 UI 和內部元件
  - MainForm 介面重構
  - FreeList 記憶體管理最佳化
  - WriteQueue 寫入佇列改進
  - SpeedyPack 核心最佳化
  - 5 個檔案變更，96 行新增，88 行刪除

#### 儲存系統增強
- `57f9d5d` - 改進儲存系統，添加自動壓縮和不完整日期支援
  - 新增 SpeedyPackAutoCompactor 自動壓縮定時器（30 分鐘間隔）
  - SpeedyPackRegistry 單例管理器增強
  - SpeedyStorage、SpeedyTimeStorage、SpeedyWorkNoteStorage 適配改進
  - SpeedyPack 新增 FreeList 空閒空間管理（149 行）
  - PackFileWriter 寫入器重構最佳化
  - WriteOperation、WriteQueue 寫入佇列增強
  - SpeedyPackOptions 設定選項擴展
  - IncompleteDate 新增比較方法
  - PluginLoader 外掛程式載入器改進
  - Default 和 Fast 版本 Program.cs 初始化流程更新
  - DefaultConfigData 設定資料簡化
  - KnowledgeNetwork 知識網路精簡
  - ChatController、MemoryController 控制器最佳化
  - SpeedyPack.Manager MainForm 功能增強
  - 22 個檔案變更，639 行新增，253 行刪除

#### Speedy.Manager 更新
- `b04ed33` - 更新 Speedy.Manager 檔案

### 2026-05-01

#### 架構重構：Speedy 儲存替換 LiteDB
- `6600972` - 用 Speedy 儲存替換 LiteDB，添加外掛程式系統和 Speedy 專案
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
    - 視圖：MainWindow、DirectoryTreeView、ContentViewerPanel、MetadataPanel
    - 對話方塊：FileInfoDialog、ImportDialog、NewEntryDialog
  - **SiliconLife.Fast 儲存遷移**：LiteDB → SpeedyPack
    - 新增 SpeedyStorage（IStorage 適配器）
    - 新增 SpeedyTimeStorage（ITimeStorage 適配器）
    - 新增 SpeedyWorkNoteStorage（IWorkNoteStorage 適配器）
    - 新增 SpeedyPackRegistry（行程級單例管理）
    - 新增 SpeedyPackAutoCompactor（自動壓縮定時器）
    - 移除 LiteDB 相關儲存實現（LiteDBStorage、LiteDBTimeStorage、LiteDBWorkNoteStorage、LiteDBLoggerProvider、LiteDBManager、LiteDBModels）
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
- `3aef4c3` - 添加 Stopped 活動狀態和錯誤處理改進
  - 矽基生命體新增 Stopped 狀態
  - 錯誤處理和恢復機制增強

#### 本地化更新
- `513c65d` - 更新所有語言版本和文件
  - 新增 MarkdownEditorComponent 元件（625 行）
  - 新增 DetailsComponent 元件（130 行）
  - 新增 AccordionComponent 手風琴元件（285 行）
  - BeingController、ChatController、MemoryController、PermissionController 控制器更新
  - BeingView、ChatView、MemoryView、SoulEditorView 視圖重構
  - 移除舊 MarkdownEditorView
  - InitController 元件化遷移
  - 115 個檔案變更，5761 行新增，2362 行刪除

### 2026-04-30

#### 系統托盤功能
- `101b203` - 實現托盤狀態視窗和 ApplicationContext
  - 新增托盤圖示資源（alpha.png、noWord.png、slc.ico、wordIcon.png）
  - 實現 TrayStatusWindow 狀態視窗
  - 支援 9 種語言的托盤本地化（TrayCsCZ、TrayDeDE、TrayEnUS 等）
  - TrayLocalizationBase 抽象基底類別
  - 24 個檔案變更，27995 行新增，1 行刪除（含資源檔案）

#### 元件化 UI 架構
- `e61cfaa` - 完成元件化 UI 架構，實現 24 個元件
  - MVP 階段（8 個）：ComponentBase、Div、Span、Button、Input、Form、Select、Label
  - 第二階段（6 個）：Accordion、Card、Tabs、Table、Modal、Message
  - 第三階段（5 個）：Calendar、Tree、Chart、FileUpload、RichText
  - 新增 Js、Behavior、DomUpdate 等輔助類別
  - 25 個檔案變更，2666 行新增

- `7449e51` - 改進元件系統並新增新外觀主題
  - 增強 A、Button、Div、Form、Input 等元件
  - 新增 3 種外觀主題：HighContrast（高對比度）、Light（淺色）、Minimal（極簡）
  - 更新現有外觀（Admin、Chat、Creative、Dev）
  - InitController 元件化遷移
  - 32 個檔案變更，1466 行新增，1238 行刪除

- `1ba8636` - 啟動 InitController 元件化遷移（進行中）
  - 9 個檔案變更，574 行新增，145 行刪除

#### 儲存系統統一
- `895dff9` - 統一 soul.md 和 state.json 使用 IStorage 介面
  - DefaultSiliconBeing 使用 IStorage 讀寫靈魂檔案和狀態
  - 新增 StateFileManager 狀態檔案管理員
  - SoulFileManager 重構適配 IStorage
  - 8 個檔案變更，201 行新增，116 行刪除

#### LiteDB 管理增強
- `a34bef4` - 添加 LiteDBManager 並增強托盤本地化
  - 托盤功能表新增 LiteDB 管理入口
  - 9 種語言托盤本地化更新
  - 10 個檔案變更，196 行新增

- `c4a79ca` - 添加 LiteDB 管理視窗的語言感知本機工廠
  - 1 個檔案變更，78 行新增

- `5ebc55e` - 將 LiteDBAdminLocalization 轉換為抽象基底類別
  - 10 個檔案變更，1356 行新增

#### 設定系統修復
- `2da5256` - 添加 ConfigExists 抽象方法並修復 LiteDB 重複設定記錄
  - ConfigDataBase 新增 ConfigExists 方法
  - Fast 版本 DefaultConfigData 實現 LiteDB 設定存在性檢查
  - 修復 LiteDB 重複設定鍵問題
  - 9 個檔案變更，210 行新增，2 行刪除

#### 聊天和視圖最佳化
- `d3618ec` - 最佳化聊天會話、儲存系統、時間模型和視圖基底類別
  - BroadcastChannel、GroupChatSession、SingleChatSession 最佳化
  - ITimeStorage 新增查詢方法
  - FileSystemStorage 和 LiteDBStorage 同步更新
  - ViewBase 重構最佳化（Default 和 Fast 版本）
  - 11 個檔案變更，622 行新增，392 行刪除

### 2026-04-29

#### 架構重構：共用模組提取
- `a102428` - 將共用模組從 SiliconLife.Default 遷移到 SiliconLife.Common
  - 提取 32 種日曆實現到 Common 專案
  - 提取本地化基底類別及 21 種語言實現到 Common 專案
  - 提取權限管理員、預設矽基生命體實現到 Common 專案
  - 提取 23 個內建工具實現到 Common 專案
  - 提取 Playwright WebView 實現到 Common 專案
  - 更新命名空間為 SiliconLife.Collective
  - 122 個檔案變更，586 行新增，343 行刪除

#### 程式碼品質改進
- `17566fe` - 將 Core、Common 和 Default 專案中的 Console.WriteLine 替換為日誌系統
  - ContextManager、AuditLogger、DefaultConfigData 等 6 個檔案更新
  - 統一使用 ILogger 介面，提升程式碼可維護性
  - 6 個檔案變更，12 行新增，8 行刪除

#### SiliconLife.Fast 高效能版本
- `54a0307` - 添加 SiliconLife.Fast 專案並完成編譯修復
  - 完整的 Windows 窗體應用程式進入點
  - 系統托盤支援（NotifyIcon）
  - 移植全部 Web UI 控制器（20+ 個）
  - 移植全部 Web 視圖元件
  - 移植 4 種外觀主題（Admin、Chat、Creative、Dev）
  - 125 個檔案變更，61186 行新增

#### 多語言文件同步
- `265fde8` - 將雙版本架構文件同步到所有語言
  - 更新 7 種語言的 architecture.md、changelog.md
  - 更新 6 種語言的 contributing.md
  - 更新 7 種語言的 getting-started.md、roadmap.md
  - 47 個檔案變更，1214 行新增，38 行刪除

#### LiteDB 儲存系統（Fast 版本）
- `4704862` - 添加 LiteDB 相依性和基礎設施
  - 新增 LiteDBManager 管理類別
  - 新增 LiteDBModels 資料模型
  - 3 個檔案變更，252 行新增

- `4220036` - 實現 LiteDB 儲存類別
  - LiteDBStorage：實現 IStorage 介面
  - LiteDBTimeStorage：實現 ITimeStorage 介面
  - LiteDBWorkNoteStorage：實現 IWorkNoteStorage 介面
  - 3 個檔案變更，581 行新增

- `38ebd23` - 將設定和日誌系統遷移到 LiteDB
  - DefaultConfigData 適配 LiteDB 儲存
  - 新增 LiteDBLoggerProvider 日誌提供者
  - 2 個檔案變更，203 行新增，67 行刪除

- `e687157` - 將知識網路從檔案系統遷移到 LiteDB
  - KnowledgeNetwork 全面重構，使用 LiteDB 儲存三元組資料
  - 1 個檔案變更，231 行新增，72 行刪除

- `4220169` - 將 LiteDB 儲存整合到 Program 和 ProjectManager
  - Program.cs 初始化 LiteDB 儲存
  - ProjectManager 適配 LiteDB 工作筆記儲存
  - 2 個檔案變更，40 行新增，17 行刪除

- `5f3a709` - 移除廢棄的檔案系統儲存實現
  - 刪除 FileSystemLoggerProvider、FileSystemStorage、FileSystemTimeStorage 等
  - 6 個檔案變更，1518 行刪除

- `e1a4ef2` - docs: add v0.1.0-alpha version identifier to all documentation
  - 127 個檔案變更，2297 行新增，2471 行刪除

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### 儲存系統重構
- `8dd26e3` - 統一 ITimeStorage 介面使用 IncompleteDate 並添加分級查詢 API
  - 移除 ITimeStorage 介面中的 DateTime 多載方法，統一使用 IncompleteDate
  - IncompleteDate 新增 CompareTo(DateTime) 比較方法和 Expand() 展開方法
  - 新增 GetEarliestTimestamp()、GetLatestTimestamp() 分級查詢 API
  - 新增 HasSummary() 和 QueryWithLevel() 方法，支援按時間層級查詢
  - Memory.cs 重構壓縮演算法，使用新的分級查詢 API 提升效率
  - FileSystemTimeStorage.cs 完整實現新的介面方法
  - 同步更新所有呼叫方：ChatSystem、ChatSession、BroadcastChannel、AuditLogger、TokenUsageRecord 等
  - 工具系統更新：HelpTool、LogTool、TokenAuditTool 適配新介面
  - Web 控制器更新：AuditController、ChatController、ChatHistoryController 適配新介面
  - 41 個檔案變更，1820 行新增，903 行刪除

### 2026-04-27

#### 幫助文件系統增強
- `9989d79` - 更新本地化、幫助系統和 Web 視圖
  - 新增 IAIClientFactoryHelp.cs AI 客戶端工廠幫助文件介面
  - 完成全部幫助文件的 9 種語言翻譯
  - HelpTopics.cs 新增 40 個幫助主題定義
  - Web 視圖全面更新：InitController、AuditView、ConfigView、KnowledgeView、LogView 等
  - 本地化系統增強：所有語言版本添加新的本地化鍵
  - AI 客戶端工廠更新：DashScopeClientFactory、OllamaClientFactory 改進
  - 30 個檔案變更，10086 行新增，15 行刪除

#### 幫助文件新增內容
- `e7afe94` - 新增靈魂檔案和稽核日誌幫助文件
  - 新增靈魂檔案管理幫助文件
  - 新增稽核日誌幫助文件
  - HelpTopics.cs 新增主題定義
  - HelpView.cs 大幅重構，改進文件渲染邏輯
  - PermissionView.cs 重構，改進權限管理介面
  - 核心模組增強：SiliconBeingManager、TaskSystem、ToolManager 改進
  - TaskTool.cs 重構，改進任務管理功能
  - Web 視圖全面更新：所有視圖元件同步更新
  - HelpController.cs 簡化，最佳化控制器邏輯
  - 30 個檔案變更，7100 行新增，897 行刪除

### 2026-04-26

#### 幫助文件系統
- `07895d7` - 增強幫助文件系統，新增 3 個文件並完成 9 種語言翻譯
  - 新增記憶系統、Ollama 安裝設定、阿里雲百煉平台使用指南
  - 完成全部 10 個幫助文件的 9 種語言翻譯
  - 簡化 HelpView 渲染邏輯
  - 18 個檔案變更，14418 行新增，1364 行刪除

#### 德語本地化
- `0cfd8a1` - 添加完整的德語 (de-DE) 本地化支援
  - 完整的德語本地化檔案
  - 新增中國歷史日曆德語支援
  - 新增幫助文件德語翻譯
  - 完整同步 9 種語言的所有文件
  - 135 個檔案變更，26186 行新增，14371 行刪除

#### 文件同步
- `3aada7d` - 同步繁體中文 (zh-HK) 文件與簡體中文保持一致
  - 3 個檔案變更，519 行新增，422 行刪除
- `2f6abff` - 為所有語言添加幫助工具顯示名稱本地化
  - 7 個檔案變更，47 行新增，7 行刪除

#### 知識系統重構
- `60944fe` - 統一命名空間到 SiliconLife.Collective
  - 8 個檔案變更，5 行新增，8 行刪除
- `69c51c5` - 添加幫助文件系統並將程式碼註釋翻譯為英文
  - 29 個檔案變更，3385 行新增，22 行刪除

### 2026-04-25

#### WebView 瀏覽器自動化
- `41757c3` - 實現基於 Playwright 的跨平台 WebView 瀏覽器自動化
  - 6 個檔案變更，1152 行新增

#### 文件更新
- `0ff797b` - 添加 KnowledgeTool 和 WorkNoteTool 文件（7 種語言）
  - 28 個檔案變更，4983 行新增
- `ad77415` - 更新所有 changelog 檔案，添加 2026-04-25 Git 歷史記錄
  - 7 個檔案變更，168 行新增

#### 專案工作區管理
- `785c551` - 實現專案工作區管理，包含工作筆記和任務系統
  - 新增專案工作區管理系統
  - 工作筆記功能，用於追蹤專案進度
  - 任務管理系統整合
  - 29 個檔案變更，4256 行新增，36 行刪除

#### 捷克語本地化
- `b4bbf39` - 添加完整的捷克語 (cs-CZ) 本地化並更新所有語言文件
  - 116 個檔案變更，4933 行新增，222 行刪除
- `faf078f` - 修復捷克語本地化編譯錯誤
  - 3 個檔案變更，910 行新增，1 行刪除

#### 知識系統增強
- `20adaac` - 添加 KnowledgeTool 並支援完整本地化
  - 34 個檔案變更，2331 行新增，56 行刪除

### 2026-04-24

#### 記憶管理系統增強
- `c7b2ecc` - 增強記憶管理功能，添加高級過濾、統計和詳情視圖功能
  - 新增記憶高級過濾功能
  - 實現記憶統計功能
  - 添加記憶詳情視圖頁面
  - 多語言本地化支援（6 種語言）
  - 13 個檔案變更，840 行新增，86 行刪除

#### 權限系統擴展
- `4489ad6` - 將 wttr.in 天氣服務添加到網路白名單
  - 完整的多語言文件同步更新（6 種語言）
  - 14 個檔案變更，417 行新增，1 行刪除

#### Web 介面修復
- `d9d72e9` - 修復工作筆記詳情對話方塊 CSS 優先順序問題
  - 19 個檔案變更，1744 行新增，6 行刪除

#### 聊天歷史最佳化
- `0df599c` - 修復工具結果被渲染為獨立聊天訊息的問題
  - 1 個檔案變更，222 行新增，21 行刪除
- `057b09d` - 最佳化聊天歷史詳情顯示，改進工具呼叫渲染
  - 3 個檔案變更，389 行新增，68 行刪除

#### 定時器執行歷史
- `fa3f06f` - 添加定時器執行歷史功能，包含詳情視圖
  - 8 個檔案變更，937 行新增，10 行刪除
- `d824835` - 添加定時器執行歷史本地化鍵（所有語言）
  - 7 個檔案變更，88 行新增

#### 本地化增強
- `c13cb17` - 註冊西班牙語語言變體
  - 1 個檔案變更，4 行新增
- `9c44f34` - 添加中國歷史日曆多語言本地化支援
  - 16 個檔案變更，6049 行新增，1 行刪除

#### 核心功能改進
- `1e7c7b2` - 改進記憶壓縮和工具執行追蹤
  - 4 個檔案變更，338 行新增，86 行刪除

### 2026-04-23

#### 工具本地化
- `192fc6e` - 為 5 個工具添加缺失的工具名稱本地化
  - 6 個檔案變更，30 行新增

#### 文件更新
- `882c08f` - 更新所有 changelog 檔案，添加完整 Git 歷史記錄並移除虛假版本號
  - 45 個檔案變更，8815 行新增，1611 行刪除

#### 聊天頁面增強
- `65c157b` - 為聊天頁面添加載入指示器並自動選擇主理人會話
  - 10 個檔案變更，211 行新增，7 行刪除

#### 聊天歷史功能
- `e483348` - 實現矽基生命體聊天歷史查看功能
  - 新增 ChatHistoryController
  - 建立 ChatHistoryViewModel
  - 實現 ChatHistoryListView 和 ChatHistoryDetailView 頁面
  - 添加聊天歷史的本地化鍵（5 種語言）
  - 12 個檔案變更，1178 行新增

#### AI 流控制增強
- `30a2d4e` - 增強 AI 流取消、IM 整合和核心主機初始化
  - 11 個檔案變更，387 行新增，12 行刪除

#### 聊天訊息佇列
- `db48c51` - 添加聊天訊息佇列、檔案中繼資料和流取消支援
  - 4 個檔案變更，357 行新增

#### 檔案上傳支援
- `28fb344` - 實現檔案來源對話方塊和檔案上傳支援
  - 3 個檔案變更，1100 行新增，2 行刪除
- `1d3e2cc` - 添加檔案來源對話方塊本地化字串（6 種語言）
  - 6 個檔案變更，30 行新增

#### 文件更新
- `8111e92` - 在 README 的儲存庫部分添加 Wiki 連結
  - 1 個檔案變更，3 行新增，1 行刪除

### 2026-04-22

#### 文件本地化
- `66c11eb` - 將中文註釋翻譯為英文並更新所有 changelog
  - 11 個檔案變更，373 行新增，163 行刪除

#### SSE 訊息增強
- `b574b2b` - 為歷史訊息添加 senderName 用於 AI 識別
  - 1 個檔案變更，9 行新增

#### 聊天功能
- `601fc14` - 添加 mark_read 操作，用於會話結束標記
  - 7 個檔案變更，196 行新增，36 行刪除

#### 工具系統最佳化
- `7a03a19` - 改進 LogTool 對話查詢靈活性
  - 1 個檔案變更，57 行新增，24 行刪除

#### 本地化增強
- `0a8d750` - 添加主動矽基生命體行為的通用系統提示
  - 8 個檔案變更，460 行新增，48 行刪除

#### 日誌系統重構
- `2b771f3` - 解耦 LogController 與檔案 I/O，添加日誌讀取 API
  - 4 個檔案變更，172 行新增，137 行刪除
- `12da302` - 為日誌視圖添加矽基生命體篩選器
  - 9 個檔案變更，147 行新增，10 行刪除
- `8f6cb1e` - 為 ILogger 介面添加 beingId 參數，實現系統/矽基生命體日誌分離
  - 47 個檔案變更，524 行新增，490 行刪除

#### 權限系統改進
- `4c747ad` - 重構 PermissionTool、ExecuteCodeTool，添加 EvaluatePermission API
  - 18 個檔案變更，680 行新增，492 行刪除

#### Bug 修復
- `1c96e99` - 修復 search_files 和 search_content 根目錄搜尋失敗
  - 1 個檔案變更，98 行新增，41 行刪除

#### 工具整合
- `135710d` - 移除 SearchTool，將本地搜尋移至 DiskTool
  - 2 個檔案變更，185 行新增，365 行刪除

#### 工具系統擴展
- `70ce7fb` - 實現 DatabaseTool 用於結構化資料庫查詢
  - 1 個檔案變更，382 行新增
- `be29a09` - 實現 LogTool 用於操作和對話歷史查詢
  - 1 個檔案變更，298 行新增
- `4ea7702` - 實現 PermissionTool 用於動態權限管理
  - 1 個檔案變更，457 行新增
- `1384ff4` - 實現 ExecuteCodeTool 用於多語言程式碼執行
  - 1 個檔案變更，477 行新增
- `82d1e11` - 實現 SearchTool 用於資訊檢索
  - 1 個檔案變更，363 行新增

#### Web 介面最佳化
- `0675c45` - 最佳化預覽窗格中的 markdown 程式碼區塊高亮
  - 1 個檔案變更，4 行新增，23 行刪除
- `702b3f3` - 增強任務視圖，添加狀態徽章和中繼資料展示
  - 8 個檔案變更，221 行新增，9 行刪除
- `6ed9a79` - 改進聊天訊息儲存和視圖渲染
  - 8 個檔案變更，140 行新增，29 行刪除

### 2026-04-21

#### Bug 修復
- `c6b518b` - 修復定時器訊息傳遞和聊天訊息儲存
  - 3 個檔案變更，297 行新增，124 行刪除

#### 設定管理
- `4305769` - 添加 .gitattributes 用於行尾管理
  - 1 個檔案變更，32 行新增

#### Web 介面改進
- `188c6f8` - 註冊任務列表 API 路由並添加空狀態顯示
  - 2 個檔案變更，35 行新增，2 行刪除
- `634e8ca` - 添加權限頁面返回列表連結
  - 1 個檔案變更，16 行新增
- `6ba591d` - 添加獨立 AI 設定編輯器用於矽基生命體
  - 11 個檔案變更，842 行新增，18 行刪除
- `0a826f5` - 在程式碼編輯器中添加儲存成功提示
  - 1 個檔案變更，9 行新增，2 行刪除
- `2940373` - 增強 Web 介面，添加程式碼懸浮提示和 UI 改進
  - 11 個檔案變更，1054 行新增，75 行刪除

#### 權限系統修復
- `592c7ab` - 修復回呼實例化和註冊順序
  - 2 個檔案變更，38 行新增，7 行刪除

#### 安全增強
- `833ead2` - 為動態編譯添加組件參考驗證
  - 4 個檔案變更，135 行新增，8 行刪除

#### 權限系統增強
- `5879621` - 添加權限回呼預編譯驗證和增強錯誤處理
  - 21 個檔案變更，617 行新增，26 行刪除

#### 文件更新
- `4dbf659` - 更新 changelog 到 v0.5.1，替換 GitHub 預留位置 URL，添加 Gitee 鏡像，按語言本地化 Bilibili 名稱，更新電子郵件
  - 32 個檔案變更，489 行新增，180 行刪除

#### 設定與入口
- `0fc1693` - 更新程式入口和專案設定
  - 2 個檔案變更，7 行新增

#### 權限系統重構
- `ea9179a` - 改進權限系統實現
  - 5 個檔案變更，358 行新增，152 行刪除

#### Bug 修復
- `928a96d` - 修復日曆計算實現
  - 4 個檔案變更，12 行新增，12 行刪除

#### AI 與日曆
- `646813e` - 改進 AI 客戶端工廠實現
  - 2 個檔案變更，21 行新增，20 行刪除

#### 本地化
- `7940d9c` - 添加韓語本地化支援
  - 7 個檔案變更，2424 行新增，10 行刪除
- `4ff98ad` - 重構文件，支援多語言
  - 81 個檔案變更，23818 行新增，1886 行刪除

### 2026-04-20

#### 核心功能完善
- `28905b5` - 完整的多語言支援、AI 客戶端工廠、權限系統和本地化設定
  - 帶管理員、條目和不同日誌級別的日誌系統
  - 用於查詢和追蹤 token 使用的 token 稽核系統
  - 自動發現不同 AI 平台的 AI 客戶端工廠
  - 帶自己儲存的權限回呼系統
  - 主控台日誌器實現
  - 英語和簡體中文的多語言支援
  - 帶 WebSocket 的 WebUI 信使，用於即時聊天
  - 使用本地化增強預設矽基生命體
  - 39 個檔案變更，4670 行新增，175 行刪除

### 2026-04-19

#### 定時器與日曆
- `c933fd8` - 更新本地化、定時器系統、Web 視圖並添加工具
  - 更好的本地化管理員
  - 定時任務的排程系統
  - AI 設定和上下文管理
  - 支援 32 種日曆類型的日曆工具
  - 用於日曆 API 的 Web 控制器
  - 任務管理工具
  - 46 個檔案變更，4018 行新增，975 行刪除

**架構改進**
- 重新設計 Web 視圖架構以更好地支援外觀
- 改進生命體管理系統，具有更好的狀態處理

### 2026-04-18

- `9f585e1` - 更新本地化、定時器系統、Web 視圖並添加工具
  - 定時器和排程改進
  - 帶改進 UI 元件的更好 Web 視圖
  - 更多工具實現
  - 57 個檔案變更，3328 行新增，389 行刪除

### 2026-04-17

- `9b71fcd` - 更新核心模組，添加 zh-HK 文件、廣播頻道、設定工具和稽核 Web 視圖
  - 廣播頻道，用於多個矽基生命體一起聊天
  - 設定工具系統
  - 稽核 Web 視圖
  - 繁體中文文件
  - 42 個檔案變更，3533 行新增，268 行刪除

### 2026-04-16

- `5040f05` - 更新核心和預設模組
  - 模組最佳化和 bug 修復
  - 實現更新和改進
  - 58 個檔案變更，9916 行新增，111 行刪除

### 2026-04-15

- `3efab5f` - 更新多個模組：AI、Chat、IM、Tools、Web、Localization、Storage
  - AI 客戶端改進
  - 聊天系統增強
  - 信使提供者更新
  - 工具系統最佳化
  - Web 基礎設施改進
  - 本地化最佳化
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

#### 文件與本地化
- `2161002` - 重構文件並增強本地化
  - 17 個檔案變更，982 行新增，92 行刪除
- `03d94e4` - 增強設定系統和本地化
  - 25 個檔案變更，1378 行新增，154 行刪除
- `9976a35` - 添加關於頁面和本地化
  - 14 個檔案變更，699 行新增，44 行刪除

#### 聊天與 Web 視圖
- `0c8ccfc` - 增強聊天系統、本地化和 Web 視圖
  - 13 個檔案變更，402 行新增，56 行刪除
- `a8f1342` - 重新設計 Web 通訊層，從 WebSocket 切換到 SSE
  - 27 個檔案變更，793 行新增，935 行刪除

### 2026-04-11

#### 日誌系統
- `e8fe259` - 添加日誌系統和程式碼最佳化
  - 37 個檔案變更，624 行新增，91 行刪除
- `f01c519` - 添加日誌系統，更新 AI 介面和 Web 視圖
  - 31 個檔案變更，1758 行新增，63 行刪除

### 2026-04-10

- `4962924` - 增強 WebSocket 處理程式、聊天視圖和信使互動
  - 上下文管理員改進
  - 聊天系統增強
  - 信使提供者介面更新
  - WebUI 提供者重新設計
  - JavaScript 建構器和路由器更新
  - 聊天視圖最佳化
  - WebSocket 處理程式改進
  - 9 個檔案變更，365 行新增，134 行刪除

### 2026-04-09

- `f9302bf` - 增強信使提供者介面、聊天系統和 Web UI 互動
  - 信使提供者介面擴展
  - 聊天訊息和系統改進
  - 上下文管理員最佳化
  - 預設矽基生命體增強
  - Web UI 聊天視圖改進
  - WebSocket 處理程式更新
  - 10 個檔案變更，427 行新增，93 行刪除

### 2026-04-07

- `6831ee8` - 重新設計 Web 視圖和 JavaScript 建構器
  - 完整 Web 控制器重新設計
  - JavaScript 建構器完全重寫
  - 所有視圖元件更新
  - 外觀系統改進
  - 視圖基底類別架構提升
  - 23 個檔案變更，2004 行新增，1983 行刪除

### 2026-04-05

- `41e97fb` - 更新多個核心模組和 Web 控制器
  - 上下文管理員改進
  - 聊天系統和會話管理
  - 服務定位器重新設計
  - 矽基生命體基底類別和管理員更新
  - Web 控制器全面更新（17 個控制器）
  - 預設矽基生命體工廠改進
  - 31 個檔案變更，681 行新增，326 行刪除
- `67988d4` - 改進 Web UI 模組，添加執行器視圖，清理視圖和核心模組
  - 61 個檔案變更，3148 行新增，3726 行刪除

### 2026-04-04

- `b58bb1c` - 添加初始化控制器並重新設計 Web 模組
  - 初始化控制器
  - 設定模組重新設計
  - 本地化模組更新
  - 外觀系統改進
  - 路由器增強
  - 29 個檔案變更，1269 行新增，289 行刪除
- `f03ac0b` - 添加 Web UI 模組，改進信使功能
  - 60 個檔案變更，8481 行新增，165 行刪除

### 2026-04-03

- `192e57b` - 更新專案結構和核心執行時間元件
  - 22 個檔案變更，446 行新增，179 行刪除
- `59faec8` - 核心和預設實現更新
  - 25 個檔案變更，3056 行新增，18 行刪除
- `d488485` - 添加動態編譯功能和主理人工具模組
  - 19 個檔案變更，1727 行新增，11 行刪除
- `753d1d9` - 添加安全模組，更新執行器、信使提供者、本地化和工具
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
- `2fa6305` - 實現階段 2：主迴圈框架和時鐘物件系統
  - 9 個檔案變更，594 行新增，41 行刪除
- `32b99a1` - 實現階段 1 - 基本聊天功能
  - 19 個檔案變更，1185 行新增
- `358e368` - 初始提交：專案文件和許可證
  - 10 個檔案變更，1873 行新增