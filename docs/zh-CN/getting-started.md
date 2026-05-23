# 快速开始

> **版本：v0.2.0-alpha**

[English](../en/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | **中文** | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | [Čeština](../cs-CZ/getting-started.md)

## 选择版本

本项目提供两个实现版本：

### SiliconLife.Default（默认版本）
- **定位**：默认实现，主要用于验证架构可行性
- **运行模式**：控制台应用程序
- **存储方式**：文件系统 JSON 存储
- **适用场景**：数据安全优先、小数据量、开发调试、架构验证
- **平台支持**：Windows、Linux、macOS
- **角色说明**：作为架构验证的基准实现，提供简单可靠的运行方式，适合初次接触本项目或进行开发调试

### SiliconLife.Fast（高性能版本）
- **定位**：主推生产版本
- **运行模式**：桌面应用程序（Windows/macOS 系统托盘 / Linux 状态窗口）
- **存储方式**：SpeedyPack 内存存储 + 异步持久化（.spk 文件格式）
- **适用场景**：高并发、低延迟、大数据量、长期生产运行
- **平台支持**：Windows/macOS（完整功能，含系统托盘）、Linux（状态窗口，无托盘图标）
- **角色说明**：经过深度优化的生产级实现，是长期运行和实际生产环境的首选

> **新手建议**：首次使用推荐从 **SiliconLife.Default** 开始，快速验证架构可行性；熟悉系统后，强烈建议迁移到 **SiliconLife.Fast** 作为生产环境运行版本。

## 前置条件

- **.NET 9 SDK** - [下载](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [下载](https://git-scm.com/)
- **Ollama**（可选，用于本地 AI） - [下载](https://ollama.com/)
- **百炼 API 密钥**（可选，用于云端 AI） - [申请](https://bailian.console.aliyun.com/)
- **火山引擎 Ark API 密钥**（可选，用于云端 AI） - [申请](https://console.volcengine.com/ark)

## 快速开始

### 1. 克隆仓库

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. 构建项目

```bash
dotnet build
```

### 3. 配置 AI 后端

编辑 `src/SiliconLife.Default/Config/DefaultConfigData.cs` 或通过 Web UI 在运行时修改配置。

#### 选项 A：Ollama（本地）

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

#### 选项 B：百炼（云端）

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "your-api-key-here",
      "Model": "qwen-plus",
      "Region": "beijing"
    }
  }
}
```

> **可用区域**：`beijing`（北京）、`virginia`（弗吉尼亚）、`singapore`（新加坡）、`hongkong`（香港）、`frankfurt`（法兰克福）

#### 选项 C：火山引擎 Ark（云端）

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

> **注意**：火山引擎 Ark 的 Model 参数接受推理接入点 ID（例如 `ep-20241212123456-abcde`），而非模型名称。

### 4. 运行应用程序

#### 运行 Default 版本

```bash
cd src/SiliconLife.Default
dotnet run
```

Web 服务器将在 `http://localhost:8080` 启动

#### 运行 Fast 版本

```bash
cd src/SiliconLife.Fast
dotnet run
```

**Windows/macOS**：应用程序将以窗体模式启动，最小化到系统托盘，Web 服务器同样在 `http://localhost:8080` 启动

**Linux**：应用程序将显示状态窗口（无系统托盘图标），并自动打开浏览器访问 Web UI。也可使用 `--no-tray` 参数跳过浏览器自动打开：

```bash
dotnet run -- --no-tray
```

### 5. 访问 Web UI

打开浏览器并导航到：

```
http://localhost:8080
```

您将看到包含以下内容的仪表板：
- 硅基生命体管理
- 聊天界面
- 配置面板
- 系统监控

## 第一个硅基生命体

### 创建您的第一个生命体

1. 在 Web UI 中导航到**生命体管理**
2. 点击**创建新生命体**
3. 配置灵魂文件（`soul.md`），包含个性和行为
4. 启动生命体

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

## 常见问题

### Ollama 连接被拒绝

**问题**：无法连接到 `http://localhost:11434` 的 Ollama

**解决方案**：
```bash
# 检查 Ollama 是否正在运行
ollama list

# 如需启动 Ollama
ollama serve
```

### 未找到模型

**问题**：`model "qwen2.5:7b" not found`

**解决方案**：
```bash
# 拉取所需模型
ollama pull qwen2.5:7b
```

### 端口已被占用

**问题**：`HttpListenerException: Address already in use`

**解决方案**：
- 在配置中更改端口
- 或终止使用端口 8080 的进程：

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## 下一步

- 📚 阅读[架构指南](architecture.md)了解系统设计
- 🛠️ 查看[开发指南](development-guide.md)扩展系统
- 📖 探索[API 参考](api-reference.md)了解集成详情
- 🔒 查看[安全文档](security.md)了解权限系统
- 🧰 查看[工具参考](tools-reference.md)了解所有内置工具
- 🌐 查看[Web UI 指南](web-ui-guide.md)了解界面功能

## 项目结构

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # 核心接口和抽象类
│   ├── SiliconLife.Common/          # 共享实现（两个版本共用）
│   ├── SiliconLife.App/             # Default 与 Fast 共享的应用层
│   ├── SiliconLife.Default/         # 默认实现 + 入口点（控制台版）
│   ├── SiliconLife.Fast/            # 高性能实现 + 入口点（窗体版）
│   ├── SiliconLife.Speedy/          # SpeedyPack 高性能存储引擎
│   └── SiliconLife.Speedy.Manager/  # SpeedyPack 管理工具（Avalonia UI）
├── docs/                            # 文档（多语言，33 种语言变体）
│   ├── en/                          # 英语
│   ├── zh-CN/                       # 简体中文
│   ├── zh-HK/                       # 繁体中文
│   ├── es-ES/                       # 西班牙语
│   ├── ja-JP/                       # 日语
│   ├── ko-KR/                       # 韩语
│   └── cs-CZ/                       # 捷克语
├── 总文档/                           # 需求和架构文档（中文）
└── README.md                        # 项目说明
```

## 需要帮助？

- 📖 查看[帮助文档系统](web-ui-guide.md#帮助文档系统新增)（多语言支持）
- 📚 阅读[完整文档](docs/)
- 🐛 在 [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues) 上报告问题
- 💬 参与社区讨论
