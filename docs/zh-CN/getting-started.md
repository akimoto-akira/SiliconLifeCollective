# 快速开始

## 前置条件

- **.NET 9 SDK** - [下载](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [下载](https://git-scm.com/)
- **Ollama**（可选，用于本地 AI） - [下载](https://ollama.com/)
- **百炼 API 密钥**（可选，用于云端 AI） - [申请](https://bailian.console.aliyun.com/)

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
      "Region": "cn-hangzhou"
    }
  }
}
```

### 4. 运行应用程序

```bash
cd src/SiliconLife.Default
dotnet run
```

Web 服务器将在 `http://localhost:8080` 启动

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

## 项目结构

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/      # 核心接口和抽象类
│   └── SiliconLife.Default/   # 默认实现 + 入口点
├── docs/                      # 文档（多语言）
│   ├── en/                    # 英语
│   ├── zh-CN/                 # 简体中文
│   ├── zh-HK/                 # 繁体中文
│   ├── ja-JP/                 # 日语
│   └── ko-KR/                 # 韩语
└── README.md                  # 本文件
```

## 需要帮助？

- 📖 查看[文档](docs/)
- 🐛 在 GitHub 上报告问题
- 💬 加入社区讨论
