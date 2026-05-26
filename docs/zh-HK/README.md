![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**版本：v0.2.0-alpha** | **矽基生命群** — 一個基於 .NET 9 的多智慧體協作平臺，AI 智慧體被稱為**矽基生命體**，透過 Roslyn 動態編譯實作自我進化。

[English](../README.md) | [Deutsch](../de-DE/README.md) | [简体中文](../zh-CN/README.md) | **繁體中文** | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Français](../fr-FR/README.md) | [Čeština](../cs-CZ/README.md) | [Italiano](../it-IT/README.md) | [Polski](../pl-PL/README.md) | [Português](../pt-PT/README.md) | [Русский](../ru-RU/README.md)

## 🌟 核心特性

### 智慧體系統
- **多智慧體編排** — 由*矽基主理人*統一管理，採用時鐘驅動的時隙公平排程機制
- **靈魂檔案驅動** — 每個矽基生命體由核心提示檔案（`soul.md`）驅動，定義獨特個性和行為模式
- **身體-大腦架構** — *身體*（SiliconBeing）維持生命體徵並偵測觸發場景；*大腦*（ContextManager）負責載入歷史、呼叫 AI、執行工具和持久化回應
- **自我進化能力** — 透過 Roslyn 動態編譯技術，矽基生命體可以重寫自己的程式碼實作進化
- **活動狀態管理** — 支援 Idle（空閒）、Working（工作）、Error（錯誤）、Stopped（已停止）四種活動狀態，連續 10 次錯誤自動進入 Stopped 狀態

### 插件系統
- **插件擴充架構** — 透過 IPlugin 介面實作功能擴充，支援從目錄動態載入插件 DLL
- **安全沙箱** — 插件載入器執行嚴格的安全掃描，禁止存取 System.IO、System.Net 等命名空間
- **隔離載入** — 使用自訂 AssemblyLoadContext 隔離載入，防止插件影響主程式穩定性
- **工具整合** — 插件可透過 ITool 介面註冊自訂工具，自動整合到工具呼叫迴圈

### 工具與執行
- **24 個內建工具** — 涵蓋日曆、聊天、設定、磁碟、網路、記憶、任務、定時器、知識庫、工作筆記、項目工作區、WebView 瀏覽器、熱重載等
- **工具場景隔離** — 每個工具透過 `ToolScenario` 屬性宣告可用場景（Chat、Task、Timer、MemoryCompression、Project），`ChatOnly` 屬性限制工具僅在聊天場景使用
- **熱重載工具** — 支援 SiliconLife.Fast 在執行中自動編譯、更新檔案並重啟，無需手動干預
- **工具呼叫迴圈** — AI 回傳工具呼叫 → 執行工具 → 結果回饋給 AI → 持續迴圈直到回傳純文字回應
- **執行器-權限安全** — 所有 I/O 操作透過執行器進行嚴格的權限驗證
  - 3 級權限驗證鏈：UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: GlobalACL → 預設拒絕)
  - 完整的審計日誌記錄所有權限決策

### AI 與知識
- **多 AI 後端支援**
  - **Ollama** — 本地模型部署，使用原生 HTTP API
  - **阿里雲百煉（DashScope）** — 雲端 AI 服務，相容 OpenAI API，支援 13+ 模型，多區域部署
  - **火山引擎 Ark（VolcengineArk）** — 字節跳動雲端 AI 服務，支援串流和非串流模式，內建速率控制
- **32 種日曆系統** — 全球主要曆法全覆蓋，包括公曆、農曆、伊斯蘭曆、希伯來曆、日本曆、波斯曆、瑪雅曆、中國歷史曆法等
- **知識網絡系統** — 基於三元組（主體-關係-客體）的知識圖譜，支援儲存、查詢和路徑發現
- **項目工作區** — 項目空間管理，支援項目建立/歸檔/銷毀、角色分配、工作筆記、任務追蹤和工具權限隔離
- **工作流引擎** — 基於範本的狀態機引擎，支援自訂工作流範本、狀態轉換、Tick 驅動執行和實例生命週期管理
- **記憶淡忘機制** — 定時衰減服務（MemoryFadeService），每小時自動對所有矽基生命體的記憶進行重要性衰減和自動歸檔

### Web 介面
- **現代化 Web UI** — 內建 HTTP 伺服器，支援 SSE 即時更新
- **7 種佈景主題** — 管理版、聊天版、創作版、開發版、高對比度、淺色、極簡，支援自動發現和切換
- **24 個控制器** — 完整的系統管理、聊天、設定、監控功能
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
  - 熱重載工具支援線上更新和重啟
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
| AI 整合 | Ollama（本地）、阿里雲百煉（雲端）、火山引擎Ark（雲端） | Ollama（本地）、阿里雲百煉（雲端）、火山引擎Ark（雲端） |
| 資料儲存 | 檔案系統（JSON + 時間索引目錄） | SpeedyPack（.spk 格式，記憶體映射 + 非同步持久化） |
| Web 伺服器 | HttpListener（.NET 內建） | HttpListener（.NET 內建） |
| 動態編譯 | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） |
| 瀏覽器自動化 | Playwright（WebView） | Playwright（WebView） |
| 插件系統 | ✅ 支援（IPlugin + PluginLoader） | ✅ 支援（IPlugin + PluginLoader） |
| 系統匣 | ❌ 不支援 | ✅ Windows/macOS 支援（NotifyIcon）；Linux 無系統匣圖示 |
| 授權條款 | Apache-2.0 | Apache-2.0 |

## 📁 專案結構

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # 核心库（接口、抽象类）
│   │   ├── AI/                            # AI 客户端接口、上下文管理器、消息模型
│   │   ├── Audit/                         # Token 使用审计系统
│   │   ├── Chat/                          # 聊天系统、会话管理、广播频道
│   │   ├── Compilation/                   # 动态编译、安全扫描、代码加密
│   │   ├── Config/                        # 配置管理系统
│   │   ├── Executors/                     # 执行器（磁盘、网络、命令行）
│   │   ├── IM/                            # 即时通讯提供者接口
│   │   ├── Knowledge/                     # 知识网络系统
│   │   ├── Localization/                  # 本地化系统
│   │   ├── Logging/                       # 日志系统
│   │   ├── Plugins/                       # 插件系统（IPlugin 接口、PluginLoader 加载器）
│   │   ├── Project/                       # 项目管理系统
│   │   ├── Runtime/                       # 主循环、时钟对象、核心主机
│   │   ├── Security/                      # 权限管理系统
│   │   ├── SiliconBeing/                  # 硅基生命体基类、管理器、工厂
│   │   ├── Storage/                       # 存储接口
│   │   ├── Time/                          # 不完整日期（时间范围查询）
│   │   ├── Tools/                         # 工具接口和工具管理器
│   │   ├── WebView/                       # WebView 浏览器接口
│   │   ├── Workflow/                      # 工作流引擎（模板、实例、状态转换）
│   │   └── ServiceLocator.cs              # 全局服务定位器
│   │
│   ├── SiliconLife.Common/                # 共享实现（两个版本共用）
│   │   ├── AI/                            # AI 客户端与工厂（Ollama、DashScope、VolcengineArk）
│   │   ├── Calendar/                      # 32 种日历实现
│   │   ├── Localization/                  # 本地化基类与 34 种语言/地区变体实现
│   │   ├── Resources/                     # 共享资源文件
│   │   ├── Security/                      # 权限管理器
│   │   ├── SiliconBeing/                  # 默认硅基生命体实现
│   │   ├── Tools/                         # 23 个通用工具实现
│   │   ├── Web/                           # Web 基础设施
│   │   └── WebView/                       # Playwright WebView 实现
│   │
│   ├── SiliconLife.App/                   # 应用层（Web UI + 帮助文档，Default 与 Fast 共享）
│   │   ├── Config/                        # 应用配置
│   │   ├── Data/                          # 数据目录
│   │   ├── Help/                          # 帮助文档本地化（多语言）
│   │   ├── Tools/                         # HelpTool（帮助文档查询工具）
│   │   └── Web/                           # Web UI 实现
│   │       ├── Component/                 # UI 组件库（27 个组件）
│   │       ├── Controllers/               # 24 个控制器
│   │       ├── Models/                    # 视图模型
│   │       ├── Views/                     # HTML 视图
│   │       └── Skins/                     # 7 种皮肤主题
│   │
│   ├── SiliconLife.Default/               # 默认实现 + 应用程序入口（控制台版）
│   │   ├── Program.cs                     # 入口点（装配所有组件）
│   │   ├── Config/                        # 默认配置数据
│   │   ├── Knowledge/                     # 知识网络实现
│   │   ├── Logging/                       # 日志提供者实现（控制台 + 文件系统）
│   │   ├── Project/                       # 项目系统实现
│   │   └── Storage/                       # 文件系统存储实现
│   │
│   ├── SiliconLife.Fast/                  # 高性能实现 + 应用程序入口（窗体版）
│   │   ├── Program.cs                     # 入口点（窗体应用程序）
│   │   ├── App.axaml / App.cs             # Avalonia 应用定义
│   │   ├── Config/                        # 配置数据（与 Default 共享）
│   │   ├── Knowledge/                     # 知识网络实现（内存优化）
│   │   ├── Logging/                       # 高性能日志提供者
│   │   ├── Project/                       # 项目系统实现
│   │   ├── Storage/                       # SpeedyPack 存储适配器
│   │   └── Tray/                          # 系统托盘（34 种语言变体本地化）
│   │
│   ├── SiliconLife.Speedy/                # SpeedyPack 高性能存储引擎
│   │   ├── SpeedyPack.cs                  # 核心类（内存目录映射 + 缓存 + 异步写入）
│   │   ├── SpeedyPackOptions.cs           # 配置选项（缓存 TTL、最大条目数等）
│   │   ├── IPackTransaction.cs            # 事务接口
│   │   ├── SpkFileInfo.cs                 # 文件信息
│   │   └── Internal/                      # 内部实现
│   │       ├── DirectoryMap.cs            # 内存目录映射
│   │       ├── EntryCache.cs              # 条目缓存
│   │       ├── FreeList.cs                # 空闲空间管理
│   │       ├── PackFileReader.cs          # 包文件读取器
│   │       ├── PackFileWriter.cs          # 包文件写入器
│   │       ├── WriteQueue.cs              # 异步写入队列
│   │       ├── WriteOperation.cs          # 写入操作
│   │       ├── SpeedyTransaction.cs       # 事务实现
│   │       ├── SpkHeader.cs               # 包文件头
│   │       └── PathNormalizer.cs          # 路径规范化
│   │
│   └── SiliconLife.Speedy.Manager/        # SpeedyPack 管理工具（Avalonia UI）
│       ├── MainForm.cs                    # 主窗体
│       ├── Program.cs                     # 入口点
│       └── slc.ico                        # 应用图标
│
├── docs/                                  # 多语言文档
│   ├── zh-CN/                             # 简体中文文档
│   ├── en/                                # 英文文档
│   └── ...                                # 其他语言文档
│
└── 总文档/                                 # 需求文档和架构文档
    ├── 需求文档.md
    ├── 架构大纲.md
    └── 实现顺序.md
```

## 🏗️ 架構概覽

### 排程架構
```
主循环（专用线程，看门狗 + 熔断器）
  └── 时钟对象（按优先级排序）
       └── 硅基生命体管理器
            └── 硅基生命体运行器（临时线程，超时 + 熔断器）
                 └── 硅基生命体.Tick()
                      └── 上下文管理器.思考()
                           └── AI 客户端.聊天()
                                └── 工具调用循环 → 持久化到聊天系统
```

### 安全架構
所有 AI 發起的 I/O 操作必須透過嚴格的安全鏈：

```
工具调用 → 执行器 → 权限管理器 → [频率缓存 → 回调 → (IsCurator: 询问用户 | Non-curator: 全局ACL)]
```

## 🚀 快速開始

### 前置條件

- **.NET 9 SDK** — [下載連結](https://dotnet.microsoft.com/download/dotnet/9.0)
- **AI 後端**（擇一使用）：
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
- [x] 階段 10：Web UI（HTTP + SSE，24 個控制器，7 種佈景主題）
- [x] 階段 10.5：增量增強（廣播頻道、Token 審計、32 種日曆、工具增強、34 種語言變體在地化）
- [x] 階段 10.6：完善與最佳化（WebView、說明系統、項目工作區、知識網絡、工作流引擎）
- [x] 階段 11：SpeedyPack 儲存引擎（替換 LiteDB、記憶體映射、非同步寫入佇列、自動壓縮）
- [x] 階段 12：插件系統（IPlugin 介面、PluginLoader 安全沙箱、隔離載入、工具整合）

### 🚧 計劃中
- [ ] 階段 13：外部即時通訊整合（飛書 / WhatsApp / Telegram）
- [ ] 階段 14：技能生態系統（插件市集、技能包分發）

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
开发/验证环境：SiliconLife.Default（验证架构、调试功能）
生产环境：SiliconLife.Fast（高性能、后台运行、处理实时请求）
```

**策略 2：Fast 主執行，Default 定期備份**
```
SiliconLife.Fast（日常使用，处理实时请求）
    ↓ 定期备份
SiliconLife.Default（冷数据归档，数据安全兜底）
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