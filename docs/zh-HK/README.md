# Silicon Life Collective

**⚠️ 警告：動态編譯可以工作，但需要程式碼模板才能正常執行。正在進行全面測試。**

一個基於 .NET 9 的多智能体協作平台，AI 智能体被称為**硅基生命体**，通過 Roslyn 動态編譯實現自我進化。

[English](../en/README.md) | [简体中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Español](../es-ES/README.md) | [Čeština](../cs-CZ/README.md)

## 功能特性

- **多智能体编排** — 由*硅基主理人*管理，采用时钟驅動程式的时隙公平排程（主循环 + 时钟物件 + 看門狗 + 斷路器）
- **靈魂檔案驅動程式** — 每個硅基生命体由核心提示檔案（`soul.md`）驅動程式，定義其個性和行為
- **身體-大腦架構** — *身體*（SiliconBeing）保持存活狀態並检测触發場景；*大腦*（ContextManager）載入歷史記录、调用 AI、執行工具並持久化回應
- **工具调用循环** — AI 返回工具调用 → 執行工具 → 将結果回饋给 AI → AI 继续 → 直到返回纯文本回應
- **執行器-權限安全** — 所有磁碟、網路和命令行操作都通過執行器進行權限驗證
  - 5級權限链：IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - 所有權限決策都有稽核記錄
- **Token 使用稽核** — 通過 `ITokenUsageAudit` / `TokenUsageAuditManager` 內置 token 使用跟踪和報告
- **多 AI 後端** — 支援 Ollama（本地）和阿里雲百炼（雲端）
  - **Ollama** — 本地模型托管，使用原生 HTTP API
  - **百炼（DashScope）** — 雲端 AI 服務，相容 OpenAI API，多區域部署，支援 13+ 模型（通義千問、DeepSeek、GLM、Kimi、Llama）
- **32 种日歷系統** — 多日歷支援，包括公歷、農歷、伊斯蘭歷、希伯來歷、日本歷、波斯歷、瑪雅歷等
- **最小依赖** — 核心程式庫僅依赖 Microsoft.CodeAnalysis.CSharp 用於 Roslyn 動态編譯
- **零資料庫依赖** — 基於檔案的儲存（JSON），通過 `ITimeStorage` 支援時間索引查詢
- **在地化** — 全面的多語言支援,包含 21 種語言變體
  - 中文:zh-CN, zh-HK, zh-SG, zh-MO, zh-TW, zhMY(6 種變體)
  - 英文:en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY(10 種變體)
  - 西班牙語：es-ES, es-MX（2 種變體）
  - 日语：ja-JP
  - 韩语：ko-KR
  - 捷克語：cs-CZ
- **Web UI** — 內建 HTTP 伺服器,支援 SSE,多种皮肤和全面的儀表板
  - **皮肤系統** — 4 种內置皮肤(管理版、聊天版、創作版、開發版),支援可插拔的 ISkin 介面和自動發現
  - **20+ 個控制器** — 關於、稽核、生命体、聊天、聊天歷史、程式碼瀏覽器、程式碼懸浮提示、配置、儀表板、執行器、初始化、知識、日誌、記憶、權限、權限請求、項目、任務、計時器、計時器執行歷史
  - **聊天歷史查看** — 完整的硅基生命体聊天歷史瀏覽功能
  - **檔案上傳支援** — 檔案來源對話框和檔案上傳功能
  - **載入指示器** — 聊天頁面的載入狀態指示器
  - **实时更新** — SSE（伺服器發送事件）用於聊天訊息、生命体狀態和系統事件
  - **HTML/CSS/JS 构建器** — 通過 `H`、`CssBuilder` 和 `JsBuilder` 進行伺服器端标記生成（零前端架構依赖）
  - **在地化** — 21 种內置语言变体，通過 LocalizationManager 解析

## 技術栈

| 元件 | 技術 |
|-----------|-----------|
| 執行时 | .NET 9 |
| 语言 | C# |
| AI 集成 | Ollama（本地）、阿里雲百炼（雲端） |
| 儲存 | 檔案系統（JSON + 時間索引目錄） |
| Web 伺服器 | HttpListener（.NET 內置） |
| 動态編譯 | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） |
| 授權證 | Apache-2.0 |

## 項目結構

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # 核心程式庫（介面、抽象類別）
│   │   ├── ServiceLocator.cs             # 全局服務定位器：Register/Get, ChatSystem, IMManager, AuditLogger, GlobalACL, BeingFactory, BeingManager, DynamicBeingLoader, TokenUsageAudit
│   │   ├── Runtime/                       # 主循环、时钟物件、核心主機、核心主機构建器、效能監控器
│   │   ├── SiliconBeing/                  # 硅基生命体基類別、硅基生命体管理器、硅基主理人、硅基生命体工廠介面、靈魂檔案管理器、記憶、工作系統、定时器系統
│   │   ├── AI/                            # AI 客戶端介面、AI 客戶端工廠介面、上下文管理器（"大腦"）、訊息、AI 要求/AI 回應
│   │   ├── Audit/                         # Token 使用稽核介面、Token 使用稽核管理器、Token 使用記录、Token 使用摘要、Token 使用查詢
│   │   ├── Chat/                          # 聊天系統、聊天服務介面、简单聊天服務、會话基類別、单聊會话、群聊會话、廣播频道、聊天訊息
│   │   ├── Executors/                     # 執行器基類別、磁碟執行器、網路執行器、命令行執行器、執行器要求、執行器結果
│   │   ├── Tools/                         # 工具介面、工具管理器（反射掃描）、工具调用/工具結果、工具定義、硅基管理員專用屬性
│   │   ├── Security/                      # 權限管理器、全局訪問控制列表、稽核記錄器、使用者频率快取、權限結果、權限類型、權限回调介面、權限询問處理器介面
│   │   ├── IM/                            # 即时通訊提供者介面、即时通訊管理器（訊息路由）
│   │   ├── Storage/                       # 儲存介面、時間儲存介面（键值對 + 時間索引）
│   │   ├── Config/                        # 設定資料基類別、設定（单例 + JSON）、設定資料基類別转换器、Guid 转换器、AI 客戶端設定屬性、設定群組屬性、設定忽略屬性、目錄資訊转换器
│   │   ├── Localization/                  # 在地化基類別、在地化管理器、語言枚舉
│   │   ├── Logging/                       # 記錄介面、記錄提供者介面、記錄条目、記錄級别、記錄管理器
│   │   ├── Compilation/                   # 動态生命体載入器、動态編譯執行器、安全掃描器、程式碼加密
│   │   └── Time/                          # 不完整日期（時間范围查詢）
│   │
│   └── SiliconLife.Default/               # 默認實現 + 入口点
│       ├── Program.cs                     # 應用程式程式入口（装配所有元件）
│       ├── AI/                            # Ollama 客戶端、Ollama 客戶端工廠（原生 Ollama HTTP API）；百炼客戶端、百炼客戶端工廠（阿里雲百炼）
│       ├── SiliconBeing/                  # 默認硅基生命体、默認硅基生命体工廠
│       ├── Calendar/                      # 32 种日歷實現：佛歷、切罗基歷、農歷、朱拉萨卡拉特歷、科普特歷、傣歷、德宏傣歷、埃塞俄比亞歷、法国共和歷、公歷、希伯來歷、印度歷、因纽特歷、伊斯蘭歷、日本歷、爪哇歷、主体歷、儒略歷、高棉歷、瑪雅歷、蒙古歷、波斯歷、民国歷、罗馬歷、萨卡歷、幹支歷、藏歷、越南歷、维克拉姆桑巴特歷、彝歷、祆歷
│       ├── Executors/                     # 默認執行器實現
│       ├── IM/                            # WebUI 提供者（Web UI 作為即时通訊频道）、即时通訊權限询問處理器
│       ├── Tools/                         # 內置工具：日歷、聊天、設定、主理人、磁碟、動态編譯、記憶、網路、系統、工作、定时器、Token 稽核
│       ├── Config/                        # 默認設定資料
│       ├── Localization/                  # 简体中文、繁体中文、美式英语、日语、韩语、西班牙語、捷克语、默認在地化基類別、其他英语(英式英语、加拿大英语、澳大利亞英语、印度英语、新加坡英语、南非英语、愛爾蘭英语、新西蘭英语、馬來西亞英语)、其他中文(新加坡中文、澳門中文、臺灣中文、馬來西亞中文)、其他西班牙語（墨西哥西班牙語）
│       ├── Logging/                       # 控制臺記錄提供者、檔案系統記錄提供者
│       ├── Storage/                       # 檔案系統儲存、檔案系統時間儲存
│       ├── Security/                      # 默認權限回调
│       ├── Runtime/                       # 測試时钟物件
│       └── Web/                           # Web UI 實現
│           ├── Controllers/               # 18 個控制器:關於、稽核、生命体、聊天、程式碼瀏覽器、程式碼悬浮提示、設定、儀表板、執行器、初始化、知識、記錄、記憶、權限、權限要求、項目、工作、定时器
│           ├── Models/                    # 檢視模型：關於檢視模型、稽核檢視模型、生命体檢視模型、聊天訊息、聊天檢視模型、程式碼瀏覽器檢視模型、設定檢視模型、儀表板檢視模型、執行器檢視模型、知識檢視模型、記錄檢視模型、記憶檢視模型、權限檢視模型、權限要求檢視模型、項目檢視模型、工作檢視模型、定时器檢視模型、檢視模型基類別
│           ├── Views/                     # 19 個 HTML 檢視：檢視基類別、關於檢視、稽核檢視、生命体檢視、聊天檢視、程式碼瀏覽器檢視、程式碼编辑器檢視、設定檢視、儀表板檢視、執行器檢視、知識檢視、記錄檢視、Markdown 编辑器檢視、記憶檢視、權限檢視、項目檢視、靈魂编辑器檢視、工作檢視、定时器檢視
│           ├── Skins/                     # 4 种皮肤：管理版（專業）、聊天版（對话）、創作版（藝術）、開發版（開發者導向）
│           ├── ISkin.cs                   # 皮肤介面 + 皮肤預览資訊 + 皮肤管理器（自動發現）
│           ├── Controller.cs              # 控制器基類別
│           ├── WebHost.cs                 # HTTP 伺服器（HttpListener）
│           ├── Router.cs                  # 要求路由，支援模式匹配
│           ├── SSEHandler.cs              # 伺服器發送事件
│           ├── WebSecurity.cs             # Web 安全工具
│           ├── H.cs                       # 流式 HTML 构建器 DSL
│           ├── CssBuilder.cs              # CSS 构建器工具
│           └── JsBuilder.cs               # JavaScript 构建器工具
│
├── docs/
│   └── zh-CN/                             # 中文文檔
```

## 架構概覽

```
主循环(專用线程,看門狗 + 斷路器)
  └── 时钟物件(按優先級排序)
       └── 硅基生命体管理器
            └── 硅基生命体執行器(每次时钟建立臨時线程,超時 + 斷路器)
                 └── 默認硅基生命体.Tick()
                      └── 上下文管理器.思考聊天()
                           └── AI 客戶端.聊天() -> 工具调用循环 -> 持久化到聊天系統
```

所有 AI 發起的 I/O 操作都必須通過安全链：

```
工具调用 -> 執行器 -> 權限管理器 -> [IsCurator -> 頻率快取 -> 全局訪問控制列表 -> 回調 -> 詢問使用者]
```

## 快速開始

### 前置條件

- .NET 9 SDK
- AI 後端(选择其一):
  - **Ollama**：[Ollama](https://ollama.com) 在本地執行並已拉取模型（例如 `ollama pull llama3`）
  - **阿里雲百煉**：從[百煉控制臺](https://bailian.console.aliyun.com/)获取有效的 API 金鑰

### 建立

```bash
dotnet restore
dotnet build
```

### 執行

```bash
dotnet run --project src/SiliconLife.Default
```

應用程式程式将啟動 Web 伺服器並自動在瀏覽器中打開 Web UI。

### 發佈(单檔案)

```bash
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

## 路線圖

- [x] 階段 1:控制臺 AI 聊天
- [x] 階段 2:架構骨架(主循环 + 时钟物件 + 看門狗 + 斷路器)
- [x] 階段 3:第一個帶有靈魂檔案的硅基生命体(身體-大腦架構)
- [x] 階段 4:持久化記憶(聊天系統 + 時間儲存介面)
- [x] 階段 5:工具系統 + 執行器
- [x] 階段 6:權限系統(5 級链、稽核記錄器、全局訪問控制列表)
- [x] 階段 7:動态編譯 + 自我進化(Roslyn)
- [x] 階段 8:長期記憶 + 工作 + 定时器
- [x] 階段 9:核心主機 + 多智能体協作
- [x] 階段 10:Web UI(HTTP + SSE,18 個控制器,4 种皮肤)
- [x] 階段 10.5:增量增強(廣播频道、Token 稽核、32 种日歷、工具增強、21 語言在地化)
- [ ] 階段 11：外部即时通訊集成（飛書 / WhatsApp / Telegram）
- [ ] 階段 12：知识图谱、外掛程式系統和技能生态系統

## 文檔

- [架構](architecture.md) — 系統設計、排程、元件架構
- [安全](security.md) — 權限模型、執行器、動态編譯安全
- [路線圖](roadmap.md) — 詳細的 12 階段開發計畫

## 授權證

本項目采用 Apache License 2.0 授權證 — 詳見 [LICENSE](LICENSE) 檔案。

## 作者

Hoshino Kennji — [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective) | [碼雲](https://gitee.com/hoshinokennji/SiliconLifeCollective) | [YouTube](https://www.youtube.com/@hoshinokennji) | [嗶哩嗶哩](https://space.bilibili.com/617827040)
