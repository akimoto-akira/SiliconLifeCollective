# 快速開始

[English](getting-started.md) | [简体中文](docs/zh-CN/getting-started.md) | [繁體中文](docs/zh-HK/getting-started.md) | [Español](docs/es-ES/getting-started.md) | [日本語](docs/ja-JP/getting-started.md) | [한국어](docs/ko-KR/getting-started.md) | [Čeština](docs/cs-CZ/getting-started.md)

## 前置條件

- **.NET 9 SDK** - [下载](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [下载](https://git-scm.com/)
- **Ollama**（可选，用於本地 AI） - [下载](https://ollama.com/)
- **百炼 API 金鑰**（可选，用於雲端 AI） - [申请](https://bailian.console.aliyun.com/)

## 快速開始

### 1. 克隆倉程式庫

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. 构建項目

```bash
dotnet build
```

### 3. 設定 AI 後端

编辑 `src/SiliconLife.Default/Config/DefaultConfigData.cs` 或通過 Web UI 在執行时修改設定。

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

#### 選項 B：百炼（雲端）

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

### 4. 執行應用程式程式

```bash
cd src/SiliconLife.Default
dotnet run
```

Web 伺服器将在 `http://localhost:8080` 啟動

### 5. 訪問 Web UI

打開瀏覽器並導航到：

```
http://localhost:8080
```

您将看到包含以下內容的儀表板：
- 硅基生命体管理
- 聊天界面
- 設定面板
- 系統監控

## 第一個硅基生命体

### 建立您的第一個生命体

1. 在 Web UI 中導航到**生命体管理**
2. 点擊**建立新生命体**
3. 設定灵魂檔案（`soul.md`），包含個性和行為
4. 啟動生命体

### soul.md 示例

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

### Ollama 連接被拒絕

**問題**：无法連接到 `http://localhost:11434` 的 Ollama

**解決方案**：
```bash
# 檢查 Ollama 是否正在執行
ollama list

# 如需啟動 Ollama
ollama serve
```

### 未找到模型

**問題**：`model "qwen2.5:7b" not found`

**解決方案**：
```bash
# 拉取所需模型
ollama pull qwen2.5:7b
```

### 端口已被占用

**問題**：`HttpListenerException: Address already in use`

**解決方案**：
- 在設定中更改端口
- 或终止使用端口 8080 的進程：

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## 下一步

- 📚 阅读[架構指南](architecture.md)了解系統設計
- 🛠️ 查看[開發指南](development-guide.md)擴充系統
- 📖 探索[API 参考](api-reference.md)了解集成详情
- 🔒 查看[安全文檔](security.md)了解權限系統

## 項目結構

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/      # 核心介面和抽象類別
│   └── SiliconLife.Default/   # 默認實現 + 入口点
├── docs/                      # 文檔（多语言）
│   ├── en/                    # 英语
│   ├── zh-CN/                 # 简体中文
│   ├── zh-HK/                 # 繁体中文
│   ├── ja-JP/                 # 日语
│   └── ko-KR/                 # 韩语
└── README.md                  # 本檔案
```

## 需要幫助？

- 📖 查看[文檔](docs/)
- 🐛 在 GitHub 上報告問題
- 💬 加入社群討論
