![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**版本：v0.2.0-alpha** | **矽基生命群** — 一個基於 .NET 9 的多智能體協作平台，AI 智能體被稱為**矽基生命體**，通過 Roslyn 動態編譯實現自我進化。

[English](../README.md) | [中文](../zh-CN/README.md) | **繁體中文** | [Deutsch](../de-DE/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Français](../fr-FR/README.md) | [Čeština](../cs-CZ/README.md) | [Italiano](../it-IT/README.md) | [Polski](../pl-PL/README.md)

## 🌟 核心特性

### 智能體系統
- **多智能體編排** — 由*矽基主理人*統一管理，採用時鐘驅動的時隙公平調度機制
- **靈魂文件驅動** — 每個矽基生命體由核心提示文件（`soul.md`）驅動，定義獨特個性和行為模式
- **身體-大腦架構** — *身體*（SiliconBeing）維持生命體徵並檢測觸發場景；*大腦*（ContextManager）負責載入歷史、呼叫 AI、執行工具和持久化響應
- **自我進化能力** — 通過 Roslyn 動態編譯技術，矽基生命體可以重寫自己的程式碼實現進化
- **活動狀態管理** — 支援 Idle（空閒）、Working（工作）、Error（錯誤）、Stopped（已停止）四種活動狀態，連續 10 次錯誤自動進入 Stopped 狀態

### 工具與執行
- **24 個內建工具** — 涵蓋日曆、聊天、配置、磁碟、網路、記憶、任務、定時器、知識庫、工作筆記、WebView 瀏覽器、熱重載等
- **熱重載工具** — 支援 SiliconLife.Fast 在執行中自動編譯、更新檔案並重啟，無需手動干預
- **工具呼叫循環** — AI 返回工具呼叫 → 執行工具 → 結果反饋給 AI → 持續循環直到返回純文字響應
- **執行器-權限安全** — 所有 I/O 操作通過執行器進行嚴格的權限驗證
  - 5 級權限鏈：IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - 完整的審計日誌記錄所有權限決策

### AI 與知識
- **多 AI 後端支援**
  - **Ollama** — 本地模型部署，使用原生 HTTP API
  - **阿里雲百煉（DashScope）** — 雲端 AI 服務，相容 OpenAI API，支援 13+ 模型，多區域部署
  - **火山引擎 Ark（VolcengineArk）** — 位元組跳動雲端 AI 服務，支援流式和非流式模式，內建速率控制
- **32 種日曆系統** — 全球主要曆法全覆蓋，包括公曆、農曆、伊斯蘭曆、希伯來曆、日本曆、波斯曆、瑪雅曆、中國歷史曆法等
- **知識網路系統** — 基於三元組（主體-關係-客體）的知識圖譜，支援儲存、查詢和路徑發現

### Web 介面
- **現代化 Web UI** — 內建 HTTP 伺服器，支援 SSE 即時更新
- **4 種皮膚主題** — 管理版、聊天版、創作版、開發版，支援自動發現和切換
- **20+ 個控制器** — 完整的系統管理、聊天、配置、監控功能
- **零前端框架依賴** — 通過 `H`、`CssBuilder` 和 `JsBuilder` 在服務端生成 HTML/CSS/JS

### 國際化與本地化
- **29 種語言實現**全面支援，涵蓋 2 種書寫系統和多個地區變體
  - **簡體中文**：zh-CN（中國大陸）、zh-SG（新加坡）、zh-MY（馬來西亞）（3 種）
  - **繁體中文**：zh-HK（香港）、zh-TW（台灣）、zh-MO（澳門）（3 種）
  - **英文**：en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY（10 種）
  - **西班牙語**：es-ES, es-MX（2 種）
  - **德語**：de-DE, de-AT, de-CH, de-LU, de-LI（5 種）
  - **法語**：fr-FR, fr-CA, fr-CH（3 種）
  - **日語**：ja-JP | **韓語**：ko-KR | **捷克語**：cs-CZ（3 種）

### 資料與儲存
- **SpeedyPack 高效能儲存** — Fast 版本使用自研 .spk 儲存引擎，記憶體目錄對映 + 條目快取 + 非同步寫入佇列
- **檔案系統儲存** — Default 版本使用純檔案系統 JSON 儲存
- **時間索引查詢** — 通過 `ITimeStorage` 介面支援按時間範圍的高效查詢
- **最小依賴** — 核心庫僅依賴 Microsoft.CodeAnalysis.CSharp 用於動態編譯

## 🔄 雙版本架構

本項目提供兩個實現版本，滿足不同場景需求：

### SiliconLife.Default（預設版本）
- **定位**：預設實現，主要用於驗證架構可行性
- **運行模式**：控制台應用程式
- **儲存方式**：純檔案系統 JSON 儲存
- **適用場景**：資料安全性要求高、記憶體資源受限、資料量小的場景
- **特點**：簡單可靠、資料持久化即時、無記憶體遺失風險
- **角色說明**：作為架構驗證的基準實現，適合初次接觸、開發調試或資料安全優先的場景
- **啟動命令**：`dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast（高效能版本）
- **定位**：主推生產版本
- **運行模式**：桌面應用程式（Windows/macOS 系統匣 / Linux 狀態視窗）
- **儲存方式**：SpeedyPack 記憶體儲存 + 異步批次持久化
- **適用場景**：高並發、低延遲、大資料量場景
- **平台支援**：Windows/macOS（完整功能，含系統匣）、Linux（狀態視窗，無匣圖示）
- **特點**：
  - 極致效能最佳化
  - Windows/macOS 託盤後臺執行，支援託盤狀態視窗實時監控；Linux 狀態視窗直接顯示
  - SpeedyPack 引擎 + 自動壓縮保證資料安全
  - Component UI 架構，30+ 宣告式元件
  - 7 種皮膚主題，支援自動發現和切換
  - 熱重載工具支援線上更新和重啟
- **效能提升**：儲存讀取延遲降低 1000 倍，寫入延遲降低 15000 倍，並發處理能力提升 50 倍
- **角色說明**：經過深度優化的生產級實現，是長期運行和實際生產環境的首選
- **啟動命令**：`dotnet run --project src/SiliconLife.Fast`

### 版本比較

| 特性 | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| **運行模式** | 控制台程式 | 桌面程式（Windows/macOS 系統匣 / Linux 狀態視窗） |
| **使用者介面** | Web UI（瀏覽器存取） | Windows/macOS：匣圖示 + 匣視窗 + Web UI；Linux：狀態視窗 + Web UI |
| **系統匣** | ❌ 無 | ✅ Windows/macOS 支援最小化到匣；Linux 無匣圖示 |
| **後台運行** | ❌ 控制台關閉即退出 | ✅ Windows/macOS 匣後台持續運行；Linux 狀態視窗運行 |
| **儲存方式** | 檔案系統 JSON 儲存 | SpeedyPack 記憶體儲存 + 異步持久化 |
| **讀取延遲** | ~10ms（磁碟 I/O） | ~0.01ms（記憶體操作） |
| **寫入延遲** | ~15ms（同步寫入） | ~0.001ms（非同步寫入） |
| **並發能力** | ~100 req/s | ~5000 req/s |
| **記憶體佔用** | ~200MB | ~500MB |
| **資料安全性** | 極高（即時持久化） | 高（WAL 日誌 + 非同步持久化） |
| **適用場景** | 資料安全優先、小資料量 | 效能優先、大資料量、高並發 |

## 🛠️ 技術堆疊

| 元件 | 技術 |
|------|------|
| 執行時 | .NET 9 |
| 程式語言 | C# |
| AI 整合 | Ollama（本地）、阿里雲百煉（雲端）、火山引擎Ark（雲端） |
| 資料儲存 | 檔案系統（JSON + 時間索引目錄） |
| Web 伺服器 | HttpListener（.NET 內建） |
| 動態編譯 | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） |
| 瀏覽器自動化 | Playwright（WebView） |
| 授權條款 | Apache-2.0 |

## 📁 專案結構

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # 核心庫（介面、抽象類別）
│   │   ├── AI/                            # AI 客戶端介面、上下文管理器、訊息模型
│   │   ├── Audit/                         # Token 使用審計系統
│   │   ├── Chat/                          # 聊天系統、會話管理、廣播頻道
│   │   ├── Compilation/                   # 動態編譯、安全掃描、程式碼加密
│   │   ├── Config/                        # 配置管理系統
│   │   ├── Executors/                     # 執行器（磁碟、網路、命令列）
│   │   ├── IM/                            # 即時通訊提供者介面
│   │   ├── Knowledge/                     # 知識網路系統
│   │   ├── Localization/                  # 本地化系統
│   │   ├── Logging/                       # 日誌系統
│   │   ├── Project/                       # 專案管理系統
│   │   ├── Runtime/                       # 主循環、時鐘物件、核心主機
│   │   ├── Security/                      # 權限管理系統
│   │   ├── SiliconBeing/                  # 矽基生命體基類、管理器、工廠
│   │   ├── Storage/                       # 儲存介面
│   │   ├── Time/                          # 不完整日期（時間範圍查詢）
│   │   ├── Tools/                         # 工具介面和工具管理器
│   │   ├── WebView/                       # WebView 瀏覽器介面
│   │   └── ServiceLocator.cs              # 全域服務定位器
│   │
│   ├── SiliconLife.Common/                # 共享實現（兩個版本共用）
│   │   ├── AI/                            # AI 客戶端與工廠（Ollama、DashScope、VolcengineArk）
│   │   ├── Calendar/                      # 32 種日曆實現
│   │   ├── Localization/                  # 本地化基類與 29 種語言/地區變體實現
│   │   ├── Resources/                     # 共享資源文件
│   │   ├── Security/                      # 權限管理器
│   │   ├── SiliconBeing/                  # 預設矽基生命體實現
│   │   ├── Tools/                         # 23 個通用工具實現（含熱重載工具）
│   │   ├── Web/                           # Web 基礎設施
│   │   └── WebView/                       # Playwright WebView 實現
│   │
│   ├── SiliconLife.App/                   # 應用層（Web UI + 幫助文檔，Default 與 Fast 共享）
│   │   ├── Config/                        # 應用配置
│   │   ├── Data/                          # 數據目錄
│   │   ├── Help/                          # 幫助文檔本地化（多語言）
│   │   └── Web/                           # Web UI 實現
│   │       ├── Component/                 # UI 組件庫（30+ 組件）
│   │       ├── Controllers/               # 22 個控制器
│   │       ├── Models/                    # 視圖模型
│   │       ├── Views/                     # HTML 視圖
│   │       └── Skins/                     # 7 種皮膚主題
│   │
│   ├── SiliconLife.Default/               # 預設實現 + 應用程式入口（控制台版）
│   │   ├── Program.cs                     # 入口點（裝配所有組件）
│   │   ├── Config/                        # 預設配置資料
│   │   ├── IM/                            # WebUI 提供者
│   │   ├── Knowledge/                     # 知識網路實現
│   │   ├── Logging/                       # 日誌提供者實現
│   │   ├── Project/                       # 專案系統實現
│   │   ├── Security/                      # 預設權限回呼
│   │   ├── Storage/                       # 檔案系統儲存實現
│   │   └── Tools/                         # 版本特有的工具實現（HelpTool）
│   │
│   ├── SiliconLife.Fast/                  # 高效能實現 + 應用程式入口（視窗版）
│   │   ├── Program.cs                     # 入口點（視窗應用程式）
│   │   ├── Config/                        # 配置資料（與 Default 共享）
│   │   ├── IM/                            # WebUI 提供者
│   │   ├── Knowledge/                     # 知識網路實現（記憶體最佳化）
│   │   ├── Logging/                       # 高效能日誌提供者
│   │   ├── Project/                       # 專案系統實現
│   │   ├── Security/                      # 最佳化權限回呼
│   │   ├── Storage/                       # SpeedyPack 儲存配接器
│   │   ├── Tools/                         # 版本特有的工具實現（HelpTool）
│   │   └── Tray/                          # 系統匣（29 種語言變體本地化）
│   │
│   ├── SiliconLife.Speedy/                # SpeedyPack 高效能儲存引擎
│   │   ├── SpeedyPack.cs                  # 核心類別（記憶體目錄對映 + 快取 + 非同步寫入）
│   │   ├── SpeedyPackOptions.cs           # 配置選項（快取 TTL、最大條目數等）
│   │   ├── IPackTransaction.cs            # 交易介面
│   │   ├── SpkFileInfo.cs                 # 檔案資訊
│   │   └── Internal/                      # 內部實現
│   │       ├── DirectoryMap.cs            # 記憶體目錄對映
│   │       ├── EntryCache.cs              # 條目快取
│   │       ├── FreeList.cs                # 空閒空間管理
│   │       ├── PackFileReader.cs          # 封包檔案讀取器
│   │       ├── PackFileWriter.cs          # 封包檔案寫入器
│   │       ├── WriteQueue.cs              # 非同步寫入佇列
│   │       ├── WriteOperation.cs          # 寫入操作
│   │       ├── SpeedyTransaction.cs       # 交易實現
│   │       ├── SpkHeader.cs               # 封包檔案標頭
│   │       └── PathNormalizer.cs          # 路徑規範化
│   │
│   └── SiliconLife.Speedy.Manager/        # SpeedyPack 管理工具（Windows Forms）
│       ├── MainForm.cs                    # 主表單
│       ├── Program.cs                     # 入口點
│       └── slc.ico                        # 應用程式圖示
│
├── docs/                                  # 多語言文檔
    ├── en/                                # 英語
    ├── zh-CN/                             # 簡體中文
    ├── zh-HK/                             # 繁體中文
    ├── de-DE/                             # 德語
    ├── ja-JP/                             # 日語
    ├── ko-KR/                             # 韓語
    ├── es-ES/                             # 西班牙語
    ├── fr-FR/                             # 法語
    ├── cs-CZ/                             # 捷克語
    └── ...                                # 其他語言文檔
├── 總文件/                     # 需求和架構文件（中文）
└── README.md                  # 專案說明
```

## 🏗️ 架構概覽

### 調度架構
```
主循環（專用執行緒，看門狗 + 熔斷器）
  └── 時鐘物件（按優先順序排序）
       └── 矽基生命體管理器
            └── 矽基生命體執行器（臨時執行緒，超時 + 熔斷器）
                 └── 矽基生命體.Tick()
                      └── 上下文管理器.思考()
                           └── AI 客戶端.聊天()
                                └── 工具呼叫循環 → 持久化到聊天系統
```

### 安全架構
所有 AI 發起的 I/O 操作必須通過嚴格的安全鏈：

```
工具呼叫 → 執行器 → 權限管理器 → [IsCurator → 頻率快取 → 全域ACL → 回呼 → 詢問用戶]
```

## 🚀 快速開始

### 前置條件

- **.NET 9 SDK** — [下載連結](https://dotnet.microsoft.com/download/dotnet/9.0)
- **AI 後端**（選擇其一）：
  - **Ollama**：[安裝 Ollama](https://ollama.com) 並拉取模型（例如 `ollama pull llama3`）
  - **阿里雲百煉**：從[百煉控制台](https://bailian.console.aliyun.com/)獲取 API 金鑰
  - **火山引擎 Ark**：從[火山引擎控制台](https://console.volcengine.com/ark)獲取 API 金鑰

### 建構專案

```bash
dotnet restore
dotnet build
```

### 執行系統

#### 方式 1：執行 Default 版本（控制台應用程式）

```bash
dotnet run --project src/SiliconLife.Default
```

應用程式將啟動 Web 伺服器並自動在瀏覽器中開啟 Web UI。

#### 方式 2：執行 Fast 版本（桌面應用程式）

```bash
dotnet run --project src/SiliconLife.Fast
```

**Windows/macOS**：應用程式將以視窗模式啟動，最小化到系統匣，後台持續運行。

**Linux**：應用程式將顯示狀態視窗（無系統匣圖示），並自動開啟瀏覽器存取 Web UI。也可使用 `--no-tray` 參數跳過瀏覽器自動開啟：

```bash
dotnet run --project src/SiliconLife.Fast -- --no-tray
```

### 發佈單一檔案

```
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
- [x] 階段 1：控制台 AI 聊天
- [x] 階段 2：框架骨架（主循環 + 時鐘物件 + 看門狗 + 熔斷器）
- [x] 階段 3：第一個帶有靈魂文件的矽基生命體（身體-大腦架構）
- [x] 階段 4：持久化記憶（聊天系統 + 時間儲存介面）
- [x] 階段 5：工具系統 + 執行器
- [x] 階段 6：權限系統（5 級鏈、審計日誌器、全域存取控制清單）
- [x] 階段 7：動態編譯 + 自我進化（Roslyn）
- [x] 階段 8：長期記憶 + 任務 + 定時器
- [x] 階段 9：核心主機 + 多智能體協作
- [x] 階段 10：Web UI（HTTP + SSE，20+ 控制器，4 種皮膚）
- [x] 階段 10.5：增量增強（廣播頻道、Token 審計、32 種日曆、工具增強、21 語言本地化）
- [x] 階段 10.6：完善與優化（WebView、幫助系統、專案工作區、知識網路）
- [x] 階段 12.2：插件系統（IPlugin 介面、安全沙箱、AssemblyLoadContext 隔離）

### 🚧 計劃中
- [ ] 階段 11：外部即時通訊整合（飛書 / WhatsApp / Telegram）
- [ ] 階段 12：技能生態系統

## 📚 文件

- [架構設計](architecture.md) — 系統設計、調度機制、元件架構
- [安全模型](security.md) — 權限模型、執行器、動態編譯安全
- [開發指南](development-guide.md) — 工具開發、擴展指南
- [API 參考](api-reference.md) — Web API 端點文件
- [工具參考](tools-reference.md) — 內建工具詳細說明
- [Web UI 指南](web-ui-guide.md) — Web 介面使用指南
- [矽基生命體指南](silicon-being-guide.md) — 智能體開發指南
- [權限系統](permission-system.md) — 權限管理詳解
- [日曆系統](calendar-system.md) — 32 種日曆系統說明
- [快速開始](getting-started.md) — 詳細入門指南
- [故障排除](troubleshooting.md) — 常見問題解答
- [路線圖](roadmap.md) — 完整開發計劃
- [變更日誌](changelog.md) — 版本更新歷史
- [貢獻指南](contributing.md) — 如何參與專案

## 🤝 參與貢獻

我們歡迎所有形式的貢獻！詳情請參閱[貢獻指南](contributing.md)。

### 開發工作流程
1. Fork 本儲存庫
2. 建立特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'feat: add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 提交 Pull Request

## 📄 授權條款

本專案採用 Apache License 2.0 授權條款 — 詳見 [LICENSE](../../LICENSE) 文件。

## 👨‍💻 作者

**天源墾驥**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- 碼雲: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- 嗨哩嗶哩: [617827040](https://space.bilibili.com/617827040)

## 🙏 致謝

感謝所有為本專案做出貢獻的開發者和 AI 平台提供者。

---

**Silicon Life Collective** — 讓 AI 智能體真正"活"起來
