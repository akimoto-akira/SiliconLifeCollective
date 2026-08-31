![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**版本：v0.2.0-alpha** | **矽基生命群** — 一個基於 .NET 9 的多智慧體協作平臺，AI 智慧體被稱為**矽基生命體**，透過 Roslyn 動態編譯實作自我進化。

[English](../README.md) | [Deutsch](../de-DE/README.md) | [中文](../zh-CN/README.md) | **繁體中文** | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Français](../fr-FR/README.md) | [Čeština](../cs-CZ/README.md) | [Italiano](../it-IT/README.md) | [Polski](../pl-PL/README.md) | [Português](../pt-PT/README.md) | [Русский](../ru-RU/README.md)

## 🌟 核心特性

### 智慧體系統
- **多智慧體編排** — 由*矽基主理人*統一管理，採用時鐘驅動的時隙公平排程機制
- **靈魂檔案驅動** — 每個矽基生命體由核心提示檔案（`soul.md`）驅動，定義獨特個性和行為模式
- **身體-大腦架構** — *身體*（SiliconBeing）維持生命體徵並偵測觸發場景；*大腦*（ContextManager）負責載入歷史、呼叫 AI、執行工具和持久化回應
- **自我進化能力** — 透過 Roslyn 動態編譯技術，矽基生命體可以重寫自己的程式碼實作進化
- **活動狀態管理** — 支援 Idle（空閒）、SingleChat（一對一聊天）、GroupChat（群聊）、Task（任務）、Timer（定時器）、Broadcast（廣播）、Project（專案）、MemoryCompression（記憶壓縮）、Stopped（已停止）九種活動狀態，連續 10 次錯誤自動進入 Stopped 狀態

### 外掛程式系統
- **外掛程式擴充架構** — 透過 IPlugin 介面實作功能擴充，支援從目錄動態載入外掛程式 DLL
- **外掛程式能力宣告** — 外掛程式透過 `[PluginCapability]` 屬性宣告所需能力（Network、FileIO、Process、AI），載入器據此放寬安全掃描規則；不可宣告的能力（P/Invoke、Unsafe、反射發射等）始終被阻止
- **隔離載入** — 使用自訂 AssemblyLoadContext 隔離載入，防止外掛程式影響主程式穩定性
- **工具整合** — 外掛程式可透過 ITool 介面註冊自訂工具，自動整合到工具呼叫迴圈

### 工具與執行
- **26 個內建工具** — 涵蓋日曆、聊天、設定、磁碟、網路、記憶、任務、定時器、知識庫、工作筆記、專案工作區、專案任務、專案工作筆記、專案工作、WebView 瀏覽器、動態編譯、程式碼執行、權限管理、Token 審計、日誌查詢、資料庫、系統資訊、說明文件、技能管理、MCP 查詢等
- **工具場景隔離** — 每個工具透過 `ToolScenario` 屬性宣告可用場景（Chat、Task、Timer、MemoryCompression、Project），`ChatOnly` 屬性限制工具僅在聊天場景使用，`SiliconManagerOnly` 屬性限制工具僅主理人使用
- **工具呼叫迴圈** — AI 回傳工具呼叫 → 執行工具 → 結果回饋給 AI → 持續迴圈直到回傳純文字回應
- **執行器-權限安全** — 所有 I/O 操作透過執行器進行嚴格的權限驗證
  - 3 級權限驗證鏈：UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: GlobalACL → 預設拒絕)
  - 完整的審計日誌記錄所有權限決策

### 技能系統
- **可重用能力單元** — 把「工具編排 + 提示詞範本」封裝為可宣告、可進化、可排程的技能，AI 像呼叫工具一樣呼叫技能
- **雙觸發模式** — Manual（AI 函式呼叫自主決定）+ Auto（schedule 排程：每日定點 / 間隔週期 / cron 子集）
- **Markdown 優先** — YAML 前置元資料 + 提示詞正文；純 Markdown 儲存時 AI 自動補全缺失元資料（使用者欄位不被覆寫）
- **熱重載與版本歸檔** — 30 秒指紋偵測自動生效；每次更新歸檔到 `skills/archive/{id}/{version}.md` 形成進化史
- **多重護欄** — 全域開關、配額限制（預設 50/生命體）、全域輪數與逾時鉗制、工具白名單、遞迴防護、技能級動作權限

### MCP 整合
- **外部工具接入** — 連接外部 MCP（Model Context Protocol）伺服器，其工具以 `mcp_{serverId}_{toolName}` 命名自動注入所有矽基生命體，無需撰寫程式碼
- **雙傳輸** — stdio（本地子程序）與 http（遠端端點）
- **使用者主權** — 伺服器增刪啟停僅限 Web UI 操作，AI 側 `mcp` 工具唯讀查詢
- **權限一致** — MCP 包裝工具納入兩級工具權限矩陣，可按生命體/專案停用

### 即時通訊整合
- **多實例架構** — 可同時接入多個 IM 平臺（Web UI / 飛書 / 企業微信 / 釘釘），每實例獨立啟停，訊息聚合路由
- **OAuth 授權精靈** — 飛書一鍵授權（state 防 CSRF、SSE 即時狀態推送），權杖自動寫回設定
- **金鑰安全** — 設定值支援 `${ENV_VAR}` 環境變數佔位符，明文金鑰不落磁碟

### AI 與知識
- **多 AI 後端支援**
  - **Ollama** — 本地模型部署，使用原生 HTTP API
  - **阿里雲百煉（DashScope）** — 雲端 AI 服務，相容 OpenAI API，支援 13+ 模型，多區域部署
  - **火山引擎 Ark（VolcengineArk）** — 字節跳動雲端 AI 服務，支援串流和非串流模式，內建速率控制
  - **牧馬人推理引擎（Herdsman）** — 無需認證的推理引擎，相容 OpenAI API 格式
  - **美團 LongCat** — 美團自研大模型，LongCat-2.0 支援 1M 上下文與思考模式，相容 OpenAI API 格式
  - **七牛雲 AI** — 七牛雲大模型推理服務，相容 OpenAI API 格式，API Key 認證
  - **DeepSeek（直連）** — 深度求索AI服務，支援 thinking 模式，1,048,576 上下文
  - **智譜 GLM** — 智譜清言AI服務，支援 thinking，按模型判斷視覺，1,048,576 上下文
  - **百度千帆/文心一言** — 百度千帆平臺，131,072 上下文
  - **騰訊混元** — 騰訊混元AI服務，雙端點 TokenHub/Legacy，262,144 上下文
  - **MiniMax** — MiniMax AI服務，1,048,576 上下文
  - **月之暗面 Kimi** — 月之暗面Kimi AI服務，262,144 上下文
  - **矽基流動** — 矽基流動聚合平臺，支援動態模型清單，1,048,576 上下文
- **AI 客戶端能力發現** — IAIClient 介面支援宣告串流模式、工具呼叫、視覺輸入、音訊輸入、上下文視窗大小等能力，ContextManager 據此自適應調整行為
- **32 種日曆系統** — 全球主要曆法全覆蓋，包括公曆、農曆、伊斯蘭曆、希伯來曆、日本曆、波斯曆、瑪雅曆、中國歷史曆法等
- **知識網路系統** — 基於三元組（主體-關係-客體）的知識圖譜，支援儲存、查詢和路徑發現
- **專案工作區** — 專案空間管理，支援專案建立/歸檔/銷毀、角色分配、工作筆記、任務追蹤和工具權限隔離
- **工作流引擎** — 基於範本的狀態機引擎，支援自訂工作流範本、狀態轉換、Tick 驅動執行和實例生命週期管理
- **記憶淡忘機制** — 定時衰減服務（MemoryFadeService），每小時自動對所有矽基生命體的記憶進行重要性衰減和自動歸檔

### Web 介面
- **現代化 Web UI** — 內建 HTTP 伺服器，支援 SSE 即時更新
- **7 種佈景主題** — 管理版、聊天版、創作版、開發版、高對比度、淺色、極簡，支援自動發現和切換
- **27 個控制器** — 完整的系統管理、聊天、設定、技能、MCP、監控功能
- **零前端框架依賴** — 透過 `H`、`CssBuilder` 和 `JsBuilder` 在伺服器端生成 HTML/CSS/JS

### 國際化與在地化
- **34 種語言變體**全面支援，涵蓋 2 種書寫系統和多個地區變體
  - **簡體中文**：zh-CN（中國大陸）、zh-SG（新加坡）、zh-MY（馬來西亞）（3 種）
  - **繁體中文**：zh-HK（香港）、zh-TW（臺灣）、zh-MO（澳門）（3 種）
  - **英語**：en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY（10 種）
  - **西班牙語**：es-ES, es-MX（2 種）
  - **德語**：de-DE, de-AT, de-CH, de-LU, de-LI（5 種）
  - **法語**：fr-FR, fr-CA, fr-CH（3 種）
  - **日語**：ja-JP | **韓語**：ko-KR | **捷克語**：cs-CZ（3 種）
  - **義大利語**：it-IT | **波蘭語**：pl-PL | **葡萄牙語**：pt-PT, pt-BR（4 種）

### 資料與儲存
- **SpeedyPack 高效能儲存** — Fast 版本使用自研 .spk 儲存引擎，記憶體目錄映射 + 條目快取 + 非同步寫入佇列
- **檔案系統儲存** — Default 版本使用純檔案系統 JSON 儲存
- **時間索引查詢** — 透過 `ITimeStorage` 介面支援按時間範圍的高效查詢
- **自動壓縮** — SpeedyPack 支援定時自動壓縮，回收空閒空間
- **最小依賴** — 核心函式庫僅依賴 Microsoft.CodeAnalysis.CSharp 用於動態編譯

## 🔄 雙版本架構

本專案提供兩個實作版本，滿足不同場景需求：

### SiliconLife.Default（預設版本）
- **定位**：預設實作，主要用於驗證架構可行性
- **執行模式**：主控台應用程式
- **儲存方式**：純檔案系統 JSON 儲存
- **適用場景**：資料安全性要求高、記憶體資源受限、資料量小的場景
- **特點**：簡單可靠、資料持久化即時、無記憶體遺失風險
- **角色說明**：作為架構驗證的基準實作，適合初次接觸、開發偵錯或資料安全優先的場景
- **啟動命令**：`dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast（高效能版本）
- **定位**：主推生產版本
- **執行模式**：桌面應用程式（Windows/macOS 系統匣 / Linux 狀態視窗）
- **儲存方式**：SpeedyPack 記憶體儲存 + 非同步批次持久化（.spk 檔案格式）
- **適用場景**：高並行、低延遲、大資料量場景
- **平臺支援**：Windows/macOS（完整功能，含系統匣）、Linux（狀態視窗，無系統匣圖示）
- **特點**：
  - 極致效能最佳化
  - Windows/macOS 系統匣背景執行，支援系統匣狀態視窗即時監控；Linux 狀態視窗直接顯示
  - SpeedyPack 引擎 + 自動壓縮保證資料安全
  - Component UI 架構，27 個宣告式元件
  - 7 種佈景主題，支援自動發現和切換
- **效能提升**：儲存讀取延遲降低 1000 倍，寫入延遲降低 15000 倍，並行處理能力提升 50 倍
- **角色說明**：經過深度最佳化的生產級實作，是長期執行和實際生產環境的首選
- **啟動命令**：`dotnet run --project src/SiliconLife.Fast`

### 版本對比

| 特性 | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| **執行模式** | 主控台程式 | 桌面程式（Windows/macOS 系統匣 / Linux 狀態視窗） |
| **使用者介面** | Web UI（瀏覽器存取） | Windows/macOS：系統匣圖示 + 系統匣視窗 + Web UI；Linux：狀態視窗 + Web UI |
| **系統匣** | ❌ 無 | ✅ Windows/macOS 支援最小化到系統匣；Linux 無系統匣圖示 |
| **背景執行** | ❌ 主控台關閉即退出 | ✅ Windows/macOS 系統匣背景持續執行；Linux 狀態視窗執行 |
| **儲存方式** | 檔案系統 JSON 儲存 | SpeedyPack 記憶體儲存 + 非同步持久化 |
| **儲存引擎** | 檔案系統 I/O | SiliconLife.Speedy（.spk 格式） |
| **讀取延遲** | ~10ms（磁碟 I/O） | ~0.01ms（記憶體操作） |
| **寫入延遲** | ~15ms（同步寫入） | ~0.001ms（非同步寫入） |
| **並行能力** | ~100 req/s | ~5000 req/s |
| **記憶體佔用** | ~200MB | ~500MB |
| **資料安全性** | 極高（即時持久化） | 高（非同步持久化 + 自動壓縮） |
| **適用場景** | 資料安全優先、小資料量 | 效能優先、大資料量、高並行 |

## 🛠️ 技術棧

| 元件 | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| 執行時期 | .NET 9 | .NET 9（Windows/macOS/Linux） |
| 程式語言 | C# | C# |
| 應用類型 | 主控台應用程式 | 桌面應用程式（Windows/macOS 系統匣 / Linux 狀態視窗） |
| AI 整合 | Ollama（本地）、阿里雲百煉（雲端）、火山引擎Ark（雲端）、牧馬人推理引擎、美團LongCat、七牛雲AI、DeepSeek、智譜GLM、百度千帆、騰訊混元、MiniMax、月之暗面Kimi、矽基流動 | Ollama（本地）、阿里雲百煉（雲端）、火山引擎Ark（雲端）、牧馬人推理引擎、美團LongCat、七牛雲AI、DeepSeek、智譜GLM、百度千帆、騰訊混元、MiniMax、月之暗面Kimi、矽基流動 |
| 資料儲存 | 檔案系統（JSON + 時間索引目錄） | SpeedyPack（.spk 格式，記憶體映射 + 非同步持久化） |
| Web 伺服器 | HttpListener（.NET 內建） | HttpListener（.NET 內建） |
| 動態編譯 | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） |
| 瀏覽器自動化 | Playwright（WebView） | Playwright（WebView） |
| 外掛程式系統 | ✅ 支援（IPlugin + PluginLoader） | ✅ 支援（IPlugin + PluginLoader） |
| 系統匣 | ❌ 不支援 | ✅ Windows/macOS 支援（NotifyIcon）；Linux 無系統匣圖示 |
| 授權條款 | Apache-2.0 | Apache-2.0 |

## 📁 專案結構

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # 核心函式庫（介面、抽象類別）
│   │   ├── AI/                            # AI 客戶端介面、上下文管理器、訊息模型
│   │   ├── Audit/                         # Token 使用審計系統
│   │   ├── Chat/                          # 聊天系統、工作階段管理、廣播頻道
│   │   ├── Compilation/                   # 動態編譯、安全掃描、程式碼加密
│   │   ├── Config/                        # 設定管理系統
│   │   ├── Executors/                     # 執行器（磁碟、網路、命令列）
│   │   ├── IM/                            # 即時通訊提供者介面
│   │   ├── Knowledge/                     # 知識網路系統
│   │   ├── Localization/                  # 在地化系統
│   │   ├── Logging/                       # 日誌系統
│   │   ├── Plugins/                       # 外掛程式系統（IPlugin 介面、PluginLoader 載入器）
│   │   ├── Project/                       # 專案管理系統
│   │   ├── Runtime/                       # 主迴圈、時鐘物件、核心主機
│   │   ├── Security/                      # 權限管理系統
│   │   ├── SiliconBeing/                  # 矽基生命體基類、管理器、工廠
│   │   ├── Storage/                       # 儲存介面
│   │   ├── Time/                          # 不完整日期（時間範圍查詢）
│   │   ├── Tools/                         # 工具介面和工具管理器
│   │   ├── WebView/                       # WebView 瀏覽器介面
│   │   ├── Workflow/                      # 工作流引擎（範本、實例、狀態轉換）
│   │   └── ServiceLocator.cs              # 全域服務定位器
│   │
│   ├── SiliconLife.Common/                # 共享實作（兩個版本共用）
│   │   ├── AI/                            # AI 客戶端與工廠（Ollama、DashScope、VolcengineArk、Herdsman、LongCat、QiniuAI、DeepSeek、Zhipu、Ernie、Hunyuan、MiniMax、Moonshot、SiliconFlow）
│   │   ├── Calendar/                      # 32 種日曆實作
│   │   ├── Localization/                  # 在地化基類與 34 種語言/地區變體實作
│   │   ├── Resources/                     # 共享資源檔案
│   │   ├── Security/                      # 權限管理器
│   │   ├── SiliconBeing/                  # 預設矽基生命體實作
│   │   ├── Tools/                         # 25 個通用工具實作
│   │   ├── Web/                           # Web 基礎設施
│   │   └── WebView/                       # Playwright WebView 實作
│   │
│   ├── SiliconLife.App/                   # 應用層（Web UI + 說明文件，Default 與 Fast 共享）
│   │   ├── Config/                        # 應用設定
│   │   ├── Data/                          # 資料目錄
│   │   ├── Help/                          # 說明文件在地化（多語言）
│   │   ├── Tools/                         # HelpTool（說明文件查詢工具）
│   │   └── Web/                           # Web UI 實作
│   │       ├── Component/                 # UI 元件庫（27 個元件）
│   │       ├── Controllers/               # 27 個控制器
│   │       ├── Models/                    # 視圖模型
│   │       ├── Views/                     # HTML 視圖
│   │       └── Skins/                     # 7 種佈景主題
│   │
│   ├── SiliconLife.Default/               # 預設實作 + 應用程式入口（主控台版）
│   │   ├── Program.cs                     # 進入點（裝配所有元件）
│   │   ├── Config/                        # 預設設定資料
│   │   ├── Knowledge/                     # 知識網路實作
│   │   ├── Logging/                       # 日誌提供者實作（主控台 + 檔案系統）
│   │   ├── Project/                       # 專案系統實作
│   │   └── Storage/                       # 檔案系統儲存實作
│   │
│   ├── SiliconLife.Fast/                  # 高效能實作 + 應用程式入口（視窗版）
│   │   ├── Program.cs                     # 進入點（視窗應用程式）
│   │   ├── App.axaml / App.cs             # Avalonia 應用定義
│   │   ├── Config/                        # 設定資料（與 Default 共享）
│   │   ├── Knowledge/                     # 知識網路實作（記憶體最佳化）
│   │   ├── Logging/                       # 高效能日誌提供者
│   │   ├── Project/                       # 專案系統實作
│   │   ├── Storage/                       # SpeedyPack 儲存配接器
│   │   └── Tray/                          # 系統匣（34 種語言變體在地化）
│   │
│   ├── SiliconLife.Speedy/                # SpeedyPack 高效能儲存引擎
│   │   ├── SpeedyPack.cs                  # 核心類別（記憶體目錄映射 + 快取 + 非同步寫入）
│   │   ├── SpeedyPackOptions.cs           # 設定選項（快取 TTL、最大條目數等）
│   │   ├── IPackTransaction.cs            # 事務介面
│   │   ├── SpkFileInfo.cs                 # 檔案資訊
│   │   └── Internal/                      # 內部實作
│   │       ├── DirectoryMap.cs            # 記憶體目錄映射
│   │       ├── EntryCache.cs              # 條目快取
│   │       ├── FreeList.cs                # 空閒空間管理
│   │       ├── PackFileReader.cs          # 封包檔案讀取器
│   │       ├── PackFileWriter.cs          # 封包檔案寫入器
│   │       ├── WriteQueue.cs              # 非同步寫入佇列
│   │       ├── WriteOperation.cs          # 寫入操作
│   │       ├── SpeedyTransaction.cs       # 事務實作
│   │       ├── SpkHeader.cs               # 封包檔案標頭
│   │       └── PathNormalizer.cs          # 路徑正規化
│   │
│   └── SiliconLife.Speedy.Manager/        # SpeedyPack 管理工具（Avalonia UI）
│       ├── MainForm.cs                    # 主表單
│       ├── Program.cs                     # 進入點
│       └── slc.ico                        # 應用程式圖示
│
├── docs/                                  # 多語言文件
│   ├── zh-CN/                             # 簡體中文文件
│   ├── en/                                # 英文文件
│   └── ...                                # 其他語言文件
│
└── 總文件/                                 # 需求文件和架構文件
    ├── 需求文件.md
    ├── 架構大綱.md
    └── 實作順序.md
```

## 🏗️ 架構概覽

### 排程架構
```
主迴圈（專用執行緒，看門狗 + 熔斷器）
  └── 時鐘物件（按優先順序排序）
       └── 矽基生命體管理器
            └── 矽基生命體執行器（臨時執行緒，逾時 + 熔斷器）
                 └── 矽基生命體.Tick()
                      └── 上下文管理器.思考()
                           └── AI 客戶端.聊天()
                                └── 工具呼叫迴圈 → 持久化到聊天系統
```

### 安全架構
所有 AI 發起的 I/O 操作必須透過嚴格的安全鏈：

```
工具呼叫 → 執行器 → 權限管理器 → [頻率快取 → 回呼 → (IsCurator: 詢問使用者 | Non-curator: 全域ACL)]
```

## 🚀 快速開始

### 前置條件

- **.NET 9 SDK** — [下載連結](https://dotnet.microsoft.com/download/dotnet/9.0)
- **AI 後端**（選擇其一）：
  - **Ollama**：[安裝 Ollama](https://ollama.com) 並拉取模型（例如 `ollama pull llama3`）
  - **阿里雲百煉**：從[百煉主控台](https://bailian.console.aliyun.com/)取得 API 金鑰
  - **火山引擎 Ark**：從[火山引擎主控台](https://console.volcengine.com/ark)取得 API 金鑰

### 建置專案

```bash
dotnet restore
dotnet build
```

### 執行系統

#### 方式 1：執行 Default 版本（主控台應用程式）

```bash
dotnet run --project src/SiliconLife.Default
```

應用程式將啟動 Web 伺服器並自動在瀏覽器中開啟 Web UI。

**適用場景**：
- ✅ 資料安全性要求極高
- ✅ 記憶體資源受限（RAM < 2GB）
- ✅ 資料量小，短期使用
- ✅ 開發偵錯階段

#### 方式 2：執行 Fast 版本（桌面應用程式）

```bash
dotnet run --project src/SiliconLife.Fast
```

**Windows/macOS**：應用程式將以視窗模式啟動，最小化到系統匣，背景持續執行。

**Linux**：應用程式將顯示狀態視窗（無系統匣圖示），並自動開啟瀏覽器存取 Web UI。也可使用 `--no-tray` 參數跳過瀏覽器自動開啟：

```bash
dotnet run --project src/SiliconLife.Fast -- --no-tray
```

**適用場景**：
- ✅ 高並行場景（> 5 使用者）
- ✅ 大資料量（使用 3 個月以上）
- ✅ 需要低延遲回應
- ✅ 需要系統匣背景執行

### 發佈單一檔案

```bash
# Windows - Default 版本
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Windows - Fast 版本
dotnet publish src/SiliconLife.Fast -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux - Default 版本
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# Linux - Fast 版本
dotnet publish src/SiliconLife.Fast -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - Default 版本
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true

# macOS - Fast 版本
dotnet publish src/SiliconLife.Fast -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 開發路線圖

### ✅ 已完成
- [x] 階段 1：主控台 AI 聊天
- [x] 階段 2：框架骨架（主迴圈 + 時鐘物件 + 看門狗 + 熔斷器）
- [x] 階段 3：第一個帶有靈魂檔案的矽基生命體（身體-大腦架構）
- [x] 階段 4：持久化記憶（聊天系統 + 時間儲存介面）
- [x] 階段 5：工具系統 + 執行器
- [x] 階段 6：權限系統（5 級鏈、審計日誌器、全域存取控制清單）
- [x] 階段 7：動態編譯 + 自我進化（Roslyn）
- [x] 階段 8：長期記憶 + 任務 + 定時器
- [x] 階段 9：核心主機 + 多智慧體協作
- [x] 階段 10：Web UI（HTTP + SSE，27 個控制器，7 種佈景主題）
- [x] 階段 10.5：增量增強（廣播頻道、Token 審計、32 種日曆、工具增強、34 種語言變體在地化）
- [x] 階段 10.6：完善與最佳化（WebView、說明系統、專案工作區、知識網路、工作流引擎）
- [x] 階段 11：SpeedyPack 儲存引擎（替換 LiteDB、記憶體映射、非同步寫入佇列、自動壓縮）
- [x] 階段 12：外掛程式系統（IPlugin 介面、PluginLoader 安全沙箱、隔離載入、工具整合、能力宣告系統）
- [x] 階段 12.5：國內 AI 平臺擴充（DeepSeek / 智譜 GLM / Kimi / 矽基流動 / MiniMax / 千帆 ERNIE / 騰訊混元，共 13 個 AI 客戶端）
- [x] 階段 13：外部即時通訊整合（多實例架構：飛書 / 企業微信 / 釘釘，飛書 OAuth 授權精靈）
- [x] 階段 13.5：技能系統（工具編排 + 提示詞範本、雙觸發模式、熱重載、版本歸檔）+ MCP 整合（外部伺服器工具接入、Web 管理頁面）

### 🚧 計劃中
- [ ] 階段 14：技能生態系統（外掛程式市集、技能包分發）

## 📚 文件

- [架構設計](architecture.md) — 系統設計、排程機制、元件架構
- [安全模型](security.md) — 權限模型、執行器、動態編譯安全
- [開發指南](development-guide.md) — 工具開發、擴充指南
- [API 參考](api-reference.md) — Web API 端點文件
- [工具參考](tools-reference.md) — 內建工具詳細說明
- [Web UI 指南](web-ui-guide.md) — Web 介面使用指南
- [矽基生命體指南](silicon-being-guide.md) — 智慧體開發指南
- [權限系統](permission-system.md) — 權限管理詳解
- [日曆系統](calendar-system.md) — 32 種日曆系統說明
- [快速開始](getting-started.md) — 詳細入門指南
- [故障排除](troubleshooting.md) — 常見問題解答
- [路線圖](roadmap.md) — 完整開發計劃
- [變更日誌](changelog.md) — 版本更新歷史
- [貢獻指南](contributing.md) — 如何參與專案

## 🤝 參與貢獻

我們歡迎所有形式的貢獻！詳情請參閱[貢獻指南](contributing.md)。

### 開發工作流
1. Fork 本儲存庫
2. 建立功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交變更 (`git commit -m 'feat: add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 提交 Pull Request

## 💡 版本選擇指南

### 我應該使用哪個版本？

**SiliconLife.Default（預設實作 — 驗證架構可行性）：**
- 📌 您第一次接觸本專案，希望快速了解系統架構
- 📌 您正在進行開發偵錯，需要簡單直接的執行方式
- 📌 資料安全性是您的首要考量
- 📌 您的系統記憶體小於 4GB
- 📌 您只需要單人使用或資料量較小

**SiliconLife.Fast（主推生產版本）：**
- ⚡ 您需要長期穩定執行的生產環境
- ⚡ 您已經熟悉系統架構，準備正式部署
- ⚡ 您需要支援多使用者並行存取
- ⚡ 您需要系統匣背景執行
- ⚡ 您追求極致的效能體驗

> **總體建議**：SiliconLife.Default 適合作為架構驗證和入門體驗；對於實際生產環境，強烈推薦使用 SiliconLife.Fast。

### 可以從 Default 遷移到 Fast 嗎？

**完全可以！** 兩個版本共享相同的：
- ✅ 設定檔案格式（config.json）
- ✅ 工具介面
- ✅ Being 設定
- ✅ Web UI 介面

**遷移步驟：**
1. 備份您的 Default 資料目錄
2. 使用相同的資料目錄啟動 Fast 版本
3. Fast 會自動將現有資料匯入 SpeedyPack 儲存引擎
4. 驗證功能正常後，即可日常使用 Fast 版本

### 兩個版本可以共存嗎？

**可以！** 推薦以下部署策略：

**策略 1：Default 驗證，Fast 生產**
```
開發/驗證環境：SiliconLife.Default（驗證架構、偵錯功能）
生產環境：SiliconLife.Fast（高效能、背景執行、處理即時請求）
```

**策略 2：Fast 主執行，Default 定期備份**
```
SiliconLife.Fast（日常使用，處理即時請求）
    ↓ 定期備份
SiliconLife.Default（冷資料歸檔，資料安全兜底）
```

## 📄 授權條款

本專案採用 Apache License 2.0 授權條款 — 詳見 [LICENSE](../../LICENSE) 檔案。

## 👨‍💻 作者

**天源墾驥**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- 碼雲: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- 嗶哩嗶哩: [617827040](https://space.bilibili.com/617827040)

## 🙏 致謝

感謝所有為本專案做出貢獻的開發者和 AI 平臺提供者。

---

**Silicon Life Collective** — 讓 AI 智慧體真正「活」起來
