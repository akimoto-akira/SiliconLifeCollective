# Silicon Life Collective

**硅基生命群** — 一个基于 .NET 9 的多智能体协作平台，AI 智能体被称为**硅基生命体**，通过 Roslyn 动态编译实现自我进化。

[English](../README.md) | [Deutsch](../de-DE/README.md) | **中文** | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Čeština](../cs-CZ/README.md)

## 🌟 核心特性

### 智能体系统
- **多智能体编排** — 由*硅基主理人*统一管理，采用时钟驱动的时隙公平调度机制
- **灵魂文件驱动** — 每个硅基生命体由核心提示文件（`soul.md`）驱动，定义独特个性和行为模式
- **身体-大脑架构** — *身体*（SiliconBeing）维持生命体征并检测触发场景；*大脑*（ContextManager）负责加载历史、调用 AI、执行工具和持久化响应
- **自我进化能力** — 通过 Roslyn 动态编译技术，硅基生命体可以重写自己的代码实现进化

### 工具与执行
- **23 个内置工具** — 涵盖日历、聊天、配置、磁盘、网络、记忆、任务、定时器、知识库、工作笔记、WebView 浏览器等
- **工具调用循环** — AI 返回工具调用 → 执行工具 → 结果反馈给 AI → 持续循环直到返回纯文本响应
- **执行器-权限安全** — 所有 I/O 操作通过执行器进行严格的权限验证
  - 5 级权限链：IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - 完整的审计日志记录所有权限决策

### AI 与知识
- **多 AI 后端支持**
  - **Ollama** — 本地模型部署，使用原生 HTTP API
  - **阿里云百炼（DashScope）** — 云端 AI 服务，兼容 OpenAI API，支持 13+ 模型，多区域部署
- **32 种日历系统** — 全球主要历法全覆盖，包括公历、农历、伊斯兰历、希伯来历、日本历、波斯历、玛雅历、中国历史历法等
- **知识网络系统** — 基于三元组（主体-关系-客体）的知识图谱，支持存储、查询和路径发现

### Web 界面
- **现代化 Web UI** — 内置 HTTP 服务器，支持 SSE 实时更新
- **4 种皮肤主题** — 管理版、聊天版、创作版、开发版，支持自动发现和切换
- **20+ 个控制器** — 完整的系统管理、聊天、配置、监控功能
- **零前端框架依赖** — 通过 `H`、`CssBuilder` 和 `JsBuilder` 在服务端生成 HTML/CSS/JS

### 国际化与本地化
- **21 种语言变体**全面支持
  - 中文：zh-CN, zh-HK, zh-SG, zh-MO, zh-TW, zhMY（6 种）
  - 英文：en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY（10 种）
  - 西班牙语：es-ES, es-MX（2 种）
  - 日语：ja-JP | 韩语：ko-KR | 捷克语：cs-CZ

### 数据与存储
- **零数据库依赖** — 纯文件系统存储（JSON 格式）
- **时间索引查询** — 通过 `ITimeStorage` 接口支持按时间范围的高效查询
- **最小依赖** — 核心库仅依赖 Microsoft.CodeAnalysis.CSharp 用于动态编译

## 🛠️ 技术栈

| 组件 | 技术 |
|------|------|
| 运行时 | .NET 9 |
| 编程语言 | C# |
| AI 集成 | Ollama（本地）、阿里云百炼（云端） |
| 数据存储 | 文件系统（JSON + 时间索引目录） |
| Web 服务器 | HttpListener（.NET 内置） |
| 动态编译 | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） |
| 浏览器自动化 | Playwright（WebView） |
| 许可证 | Apache-2.0 |

## 📁 项目结构

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
│   │   ├── Project/                       # 项目管理系统
│   │   ├── Runtime/                       # 主循环、时钟对象、核心主机
│   │   ├── Security/                      # 权限管理系统
│   │   ├── SiliconBeing/                  # 硅基生命体基类、管理器、工厂
│   │   ├── Storage/                       # 存储接口
│   │   ├── Time/                          # 不完整日期（时间范围查询）
│   │   ├── Tools/                         # 工具接口和工具管理器
│   │   ├── WebView/                       # WebView 浏览器接口
│   │   └── ServiceLocator.cs              # 全局服务定位器
│   │
│   └── SiliconLife.Default/               # 默认实现 + 应用程序入口
│       ├── Program.cs                     # 入口点（装配所有组件）
│       ├── AI/                            # Ollama 客户端、百炼客户端
│       ├── Calendar/                      # 32 种日历实现
│       ├── Config/                        # 默认配置数据
│       ├── Executors/                     # 默认执行器实现
│       ├── Help/                          # 帮助文档系统
│       ├── IM/                            # WebUI 提供者
│       ├── Knowledge/                     # 知识网络实现
│       ├── Localization/                  # 21 种语言本地化
│       ├── Logging/                       # 日志提供者实现
│       ├── Project/                       # 项目系统实现
│       ├── Runtime/                       # 测试时钟对象
│       ├── Security/                      # 默认权限回调
│       ├── SiliconBeing/                  # 默认硅基生命体实现
│       ├── Storage/                       # 文件系统存储实现
│       ├── Tools/                         # 23 个内置工具实现
│       ├── WebView/                       # Playwright WebView 实现
│       └── Web/                           # Web UI 实现
│           ├── Controllers/               # 20+ 个控制器
│           ├── Models/                    # 视图模型
│           ├── Views/                     # HTML 视图
│           └── Skins/                     # 4 种皮肤主题
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

## 🏗️ 架构概览

### 调度架构
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

### 安全架构
所有 AI 发起的 I/O 操作必须通过严格的安全链：

```
工具调用 → 执行器 → 权限管理器 → [IsCurator → 频率缓存 → 全局ACL → 回调 → 询问用户]
```

## 🚀 快速开始

### 前置条件

- **.NET 9 SDK** — [下载链接](https://dotnet.microsoft.com/download/dotnet/9.0)
- **AI 后端**（选择其一）：
  - **Ollama**：[安装 Ollama](https://ollama.com) 并拉取模型（例如 `ollama pull llama3`）
  - **阿里云百炼**：从[百炼控制台](https://bailian.console.aliyun.com/)获取 API 密钥

### 构建项目

```bash
dotnet restore
dotnet build
```

### 运行系统

```bash
dotnet run --project src/SiliconLife.Default
```

应用程序将启动 Web 服务器并自动在浏览器中打开 Web UI。

### 发布单文件

```bash
# Windows
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 开发路线图

### ✅ 已完成
- [x] 阶段 1：控制台 AI 聊天
- [x] 阶段 2：框架骨架（主循环 + 时钟对象 + 看门狗 + 熔断器）
- [x] 阶段 3：第一个带有灵魂文件的硅基生命体（身体-大脑架构）
- [x] 阶段 4：持久化记忆（聊天系统 + 时间存储接口）
- [x] 阶段 5：工具系统 + 执行器
- [x] 阶段 6：权限系统（5 级链、审计日志器、全局访问控制列表）
- [x] 阶段 7：动态编译 + 自我进化（Roslyn）
- [x] 阶段 8：长期记忆 + 任务 + 定时器
- [x] 阶段 9：核心主机 + 多智能体协作
- [x] 阶段 10：Web UI（HTTP + SSE，20+ 控制器，4 种皮肤）
- [x] 阶段 10.5：增量增强（广播频道、Token 审计、32 种日历、工具增强、21 语言本地化）
- [x] 阶段 10.6：完善与优化（WebView、帮助系统、项目工作区、知识网络）

### 🚧 计划中
- [ ] 阶段 11：外部即时通讯集成（飞书 / WhatsApp / Telegram）
- [ ] 阶段 12：插件系统和技能生态系统

## 📚 文档

- [架构设计](architecture.md) — 系统设计、调度机制、组件架构
- [安全模型](security.md) — 权限模型、执行器、动态编译安全
- [开发指南](development-guide.md) — 工具开发、扩展指南
- [API 参考](api-reference.md) — Web API 端点文档
- [工具参考](tools-reference.md) — 内置工具详细说明
- [Web UI 指南](web-ui-guide.md) — Web 界面使用指南
- [硅基生命体指南](silicon-being-guide.md) — 智能体开发指南
- [权限系统](permission-system.md) — 权限管理详解
- [日历系统](calendar-system.md) — 32 种日历系统说明
- [快速开始](getting-started.md) — 详细入门指南
- [故障排除](troubleshooting.md) — 常见问题解答
- [路线图](roadmap.md) — 完整开发计划
- [变更日志](changelog.md) — 版本更新历史
- [贡献指南](contributing.md) — 如何参与项目

## 🤝 参与贡献

我们欢迎所有形式的贡献！详情请参阅[贡献指南](contributing.md)。

### 开发工作流
1. Fork 本仓库
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'feat: add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 提交 Pull Request

## 📄 许可证

本项目采用 Apache License 2.0 许可证 — 详见 [LICENSE](../../LICENSE) 文件。

## 👨‍💻 作者

**Hoshino Kennji**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- 码云: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- 哔哩哔哩: [617827040](https://space.bilibili.com/617827040)

## 🙏 致谢

感谢所有为本项目做出贡献的开发者和 AI 平台提供者。

---

**Silicon Life Collective** — 让 AI 智能体真正"活"起来
