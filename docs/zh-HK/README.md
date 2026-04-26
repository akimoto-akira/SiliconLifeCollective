# Silicon Life Collective

**矽基生命群** — 一個基於 .NET 9 的多智能體協作平台，AI 智能體被稱為**矽基生命體**，通過 Roslyn 動態編譯實現自我進化。

[English](../README.md) | [中文](../zh-CN/README.md) | **繁體中文** | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Čeština](../cs-CZ/README.md)

## 🌟 核心特性

### 智能體系統
- **多智能體編排** — 由*矽基主理人*統一管理，採用時鐘驅動的時隙公平調度機制
- **靈魂文件驅動** — 每個矽基生命體由核心提示文件（`soul.md`）驅動，定義獨特個性和行為模式
- **身體-大腦架構** — *身體*（SiliconBeing）維持生命體徵並檢測觸發場景；*大腦*（ContextManager）負責載入歷史、呼叫 AI、執行工具和持久化響應
- **自我進化能力** — 通過 Roslyn 動態編譯技術，矽基生命體可以重寫自己的程式碼實現進化

### 工具與執行
- **23 個內建工具** — 涵蓋日曆、聊天、配置、磁碟、網路、記憶、任務、定時器、知識庫、工作筆記、WebView 瀏覽器等
- **工具呼叫循環** — AI 返回工具呼叫 → 執行工具 → 結果反饋給 AI → 持續循環直到返回純文字響應
- **執行器-權限安全** — 所有 I/O 操作通過執行器進行嚴格的權限驗證
  - 5 級權限鏈：IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - 完整的審計日誌記錄所有權限決策

### AI 與知識
- **多 AI 後端支援**
  - **Ollama** — 本地模型部署，使用原生 HTTP API
  - **阿里雲百煉（DashScope）** — 雲端 AI 服務，相容 OpenAI API，支援 13+ 模型，多區域部署
- **32 種日曆系統** — 全球主要曆法全覆蓋，包括公曆、農曆、伊斯蘭曆、希伯來曆、日本曆、波斯曆、瑪雅曆、中國歷史曆法等
- **知識網路系統** — 基於三元組（主體-關係-客體）的知識圖譜，支援儲存、查詢和路徑發現

### Web 介面
- **現代化 Web UI** — 內建 HTTP 伺服器，支援 SSE 即時更新
- **4 種皮膚主題** — 管理版、聊天版、創作版、開發版，支援自動發現和切換
- **20+ 個控制器** — 完整的系統管理、聊天、配置、監控功能
- **零前端框架依賴** — 通過 `H`、`CssBuilder` 和 `JsBuilder` 在服務端生成 HTML/CSS/JS

### 國際化與本地化
- **21 種語言變體**全面支援
  - 中文：zh-CN, zh-HK, zh-SG, zh-MO, zh-TW, zhMY（6 種）
  - 英文：en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY（10 種）
  - 西班牙語：es-ES, es-MX（2 種）
  - 日語：ja-JP | 韓語：ko-KR | 捷克語：cs-CZ

### 資料與儲存
- **零資料庫依賴** — 純檔案系統儲存（JSON 格式）
- **時間索引查詢** — 通過 `ITimeStorage` 介面支援按時間範圍的高效查詢
- **最小依賴** — 核心庫僅依賴 Microsoft.CodeAnalysis.CSharp 用於動態編譯

## 🛠️ 技術堆疊

| 元件 | 技術 |
|------|------|
| 執行時 | .NET 9 |
| 程式語言 | C# |
| AI 整合 | Ollama（本地）、阿里雲百煉（雲端） |
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
│   └── SiliconLife.Default/               # 預設實現 + 應用程式入口
│       ├── Program.cs                     # 入口點（裝配所有元件）
│       ├── AI/                            # Ollama 客戶端、百煉客戶端
│       ├── Calendar/                      # 32 種日曆實現
│       ├── Config/                        # 預設配置資料
│       ├── Executors/                     # 預設執行器實現
│       ├── Help/                          # 幫助文件系統
│       ├── IM/                            # WebUI 提供者
│       ├── Knowledge/                     # 知識網路實現
│       ├── Localization/                  # 21 種語言本地化
│       ├── Logging/                       # 日誌提供者實現
│       ├── Project/                       # 專案系統實現
│       ├── Runtime/                       # 測試時鐘物件
│       ├── Security/                      # 預設權限回呼
│       ├── SiliconBeing/                  # 預設矽基生命體實現
│       ├── Storage/                       # 檔案系統儲存實現
│       ├── Tools/                         # 23 個內建工具實現
│       ├── WebView/                       # Playwright WebView 實現
│       └── Web/                           # Web UI 實現
│           ├── Controllers/               # 20+ 個控制器
│           ├── Models/                    # 檢視模型
│           ├── Views/                     # HTML 檢視
│           └── Skins/                     # 4 種皮膚主題
│
├── docs/                                  # 多語言文件
│   ├── zh-CN/                             # 簡體中文文件
│   ├── zh-HK/                             # 繁體中文文件
│   ├── en/                                # 英文文件
│   └── ...                                # 其他語言文件
│
└── 總文件/                                 # 需求文件和架構文件
    ├── 需求文件.md
    ├── 架構大綱.md
    └── 實現順序.md
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

### 構建專案

```bash
dotnet restore
dotnet build
```

### 運行系統

```bash
dotnet run --project src/SiliconLife.Default
```

應用程式將啟動 Web 伺服器並自動在瀏覽器中開啟 Web UI。

### 發布單一檔案

```bash
# Windows
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
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

### 🚧 計劃中
- [ ] 階段 11：外部即時通訊整合（飛書 / WhatsApp / Telegram）
- [ ] 階段 12：插件系統和技能生態系統

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

**Hoshino Kennji**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- 碼雲: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- 嗶哩嗶哩: [617827040](https://space.bilibili.com/617827040)

## 🙏 致謝

感謝所有為本專案做出貢獻的開發者和 AI 平台提供者。

---

**Silicon Life Collective** — 讓 AI 智能體真正"活"起來
