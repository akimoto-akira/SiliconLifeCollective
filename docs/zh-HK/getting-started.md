# 快速開始

> **版本：v0.1.0-alpha**

[English](../en/getting-started.md) | [中文](../zh-CN/getting-started.md) | **繁體中文** | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | [Čeština](../cs-CZ/getting-started.md)

## 選擇版本

本專案提供兩個實現版本：

### SiliconLife.Default（預設版本）
- **定位**：預設實現，主要用於驗證架構可行性
- **執行模式**：主控台應用程式
- **儲存方式**：檔案系統 JSON 儲存
- **適用場景**：資料安全優先、小資料量、開發調試、架構驗證
- **平台支援**：Windows、Linux、macOS
- **角色說明**：作為架構驗證的基準實現，提供簡單可靠的運行方式，適合初次接觸本項目或進行開發調試

### SiliconLife.Fast（高效能版本）
- **定位**：主推生產版本
- **執行模式**：Windows 視窗應用程式（系統匣）
- **儲存方式**：SpeedyPack 記憶體儲存 + 異步持久化
- **適用場景**：高併發、低延遲、大資料量、長期生產運行
- **平台支援**：僅 Windows
- **角色說明**：經過深度優化的生產級實現，是長期運行和實際生產環境的首選

> **新手建議**：首次使用推薦從 **SiliconLife.Default** 開始，快速驗證架構可行性；熟悉系統後，強烈建議遷移到 **SiliconLife.Fast** 作為生產環境執行版本。

## 前置條件

- **.NET 9 SDK** - [下載](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [下載](https://git-scm.com/)
- **Ollama**（可選，用於本地 AI） - [下載](https://ollama.com/)
- **百煉 API 金鑰**（可選，用於雲端 AI） - [申請](https://bailian.console.aliyun.com/)
- **火山引擎 Ark API 金鑰**（可選，用於雲端 AI） - [申請](https://console.volcengine.com/ark)

## 快速開始

### 1. 複製儲存庫

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. 建構專案

```bash
dotnet build
```

### 3. 設定 AI 後端

編輯 `src/SiliconLife.Default/Config/DefaultConfigData.cs` 或通過 Web UI 在執行時修改設定。

#### 選項 A：Ollama（本地）

```json
{
  "AIClients": {
    "Ollama": {
      "BaseUrl": "http://localhost:11434",
      "Model": "qwen2.5:7b"
    }
  }
}
```

#### 選項 B：百煉（雲端）

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "your-api-key-here",
      "Model": "qwen-plus",
      "Region": "cn-hangzhou"
    }
  }
}
```

#### 選項 C：火山引擎 Ark（雲端）

```json
{
  "AIClients": {
    "VolcengineArk": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://ark.cn-beijing.volces.com/api/v3/chat/completions",
      "Model": "ep-xxxxxxxxxxxxx-xxxxx"
    }
  }
}
```

> **注意**：火山引擎 Ark 的 Model 參數接受推理接入點 ID（例如 `ep-20241212123456-abcde`），而非模型名稱。

### 4. 執行應用程式

#### 執行 Default 版本

```bash
cd src/SiliconLife.Default
dotnet run
```

Web 伺服器將在 `http://localhost:8080` 啟動

#### 執行 Fast 版本（僅 Windows）

```bash
cd src/SiliconLife.Fast
dotnet run
```

應用程式將以視窗模式啟動，最小化到系統匣，Web 伺服器同樣在 `http://localhost:8080` 啟動

### 5. 存取 Web UI

開啟瀏覽器並導航到：

```
http://localhost:8080
```

您將看到包含以下內容的儀表板：
- 矽基生命體管理
- 聊天介面
- 設定面板
- 系統監控

## 第一個矽基生命體

### 建立您的第一個生命體

1. 在 Web UI 中導航到**生命體管理**
2. 點擊**建立新生命體**
3. 設定靈魂檔案（`soul.md`），包含個性和行為
4. 啟動生命體

### soul.md 範例

```markdown
# My First Silicon Being

## Personality
You are a helpful assistant specializing in code review.

## Capabilities
- Review code quality
- Suggest improvements
- Explain complex concepts

## Behavior
- Always provide constructive feedback
- Use clear examples
- Be concise but thorough
```

## 常見問題

### Ollama 連線被拒絕

**問題**：無法連線到 `http://localhost:11434` 的 Ollama

**解決方案**：
```bash
# 檢查 Ollama 是否正在執行
ollama list

# 如需啟動 Ollama
ollama serve
```

### 找不到模型

**問題**：`model "qwen2.5:7b" not found`

**解決方案**：
```bash
# 拉取所需模型
ollama pull qwen2.5:7b
```

### 連接埠已被佔用

**問題**：`HttpListenerException: Address already in use`

**解決方案**：
- 在設定中更改連接埠
- 或終止使用連接埠 8080 的行程：

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## 下一步

- 📚 閱讀[架構指南](architecture.md)了解系統設計
- 🛠️ 查看[開發指南](development-guide.md)擴展系統
- 📖 探索[API 參考](api-reference.md)了解整合詳情
- 🔒 查看[安全文檔](security.md)了解權限系統
- 🧰 查看[工具參考](tools-reference.md)了解所有內建工具
- 🌐 查看[Web UI 指南](web-ui-guide.md)了解介面功能

## 專案結構

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # 核心介面和抽象類別
│   ├── SiliconLife.Common/          # 共享實現
│   ├── SiliconLife.App/             # Default 和 Fast 共享的應用層
│   ├── SiliconLife.Default/         # 預設實現 + 入口點
│   ├── SiliconLife.Fast/            # 高效能實現 + 入口點
│   ├── SiliconLife.Speedy/          # SpeedyPack 高效能儲存引擎
│   └── SiliconLife.Speedy.Manager/  # SpeedyPack 管理工具（Windows Forms）
├── docs/                            # 文檔（多語言，29 種語言變體）
│   ├── en/                    # 英語
│   ├── zh-CN/                 # 簡體中文
│   ├── zh-HK/                 # 繁體中文
│   ├── es-ES/                 # 西班牙語
│   ├── ja-JP/                 # 日語
│   ├── ko-KR/                 # 韓語
│   └── cs-CZ/                 # 捷克語
├── 總文件/                     # 需求和架構文件（中文）
└── README.md                  # 專案說明
```

## 需要協助？

- 📖 查看[說明文檔系統](web-ui-guide.md#幫助文檔系統新增)（多語言支援）
- 📚 閱讀[完整文檔](docs/)
- 🐛 在 [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues) 上回報問題
- 💬 參與社群討論
