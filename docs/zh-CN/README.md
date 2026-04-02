# 硅基生命群

基于 .NET 9 的多智能体协作平台，AI 智能体（硅基人）可通过 Roslyn 动态编译实现自我进化。

[English](../README.md)

## 特性

- **多智能体编排** — 由硅基主理人统一管理，基于 Tick 驱动的时间分片公平调度
- **自我进化** — 硅基人通过 Roslyn 在运行时重写自身 C# 类，AES 加密持久化，内置安全扫描
- **执行器-权限安全体系** — 所有磁盘、网络、命令行操作均通过执行器进行权限验证
- **零数据库依赖** — 基于文件系统存储，支持跨平台单文件部署（Windows / Linux / macOS）
- **内置 Web 服务器** — HTTP + WebSocket，服务端 HTML/CSS/JS 构建器（无需前端框架）
- **灵魂文件** — 每个硅基人由核心提示词文件驱动，定义其个性与行为模式

## 技术栈

| 组件 | 技术 |
|------|------|
| 运行时 | .NET 9 |
| 开发语言 | C# |
| AI 接入 | Ollama / OpenAI 兼容 API |
| 动态编译 | Roslyn |
| Web 服务器 | 内置 HTTP + WebSocket |
| 数据存储 | 文件系统（JSON） |
| 开源许可证 | Apache-2.0 |

## 项目结构

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/              # 核心库（接口、抽象类）
│   │   ├── Core/                      # MainLoop, TickObject, ServiceLocator
│   │   ├── Agents/                    # SiliconBeingBase, SiliconBeingManager
│   │   ├── Tools/                     # ITool 接口, ToolManager
│   │   ├── Executors/                 # 执行器基类, 磁盘/网络/命令行/编译
│   │   ├── Services/                  # IChatService, IStorageService, IAIClient
│   │   ├── IM/                        # IM 通道抽象
│   │   ├── Security/                  # 加密、权限系统
│   │   └── Web/                       # 路由器, 控制器, HtmlBuilder
│   │
│   └── SiliconLife.Default/           # 默认实现 + 程序入口
│       ├── AI/                        # OllamaClient
│       ├── Agents/                    # DefaultSiliconBeing, DefaultCurator
│       ├── Tools/                     # 内置工具（磁盘、网络、历法等）
│       ├── Chat/                      # ChatSystem, ConsoleProvider
│       ├── Storage/                   # FileSystemStorage
│       └── Program.cs                 # 应用程序入口
│
└── data/                              # 运行时数据（已 gitignore）
    └── SiliconManager/{GUID}/         # 每个硅基人的独立数据目录
```

## 快速开始

### 环境要求

- .NET 9 SDK
- 本地运行 [Ollama](https://ollama.com) 并拉取模型（如 `ollama pull llama3`）

### 构建

```bash
dotnet restore
dotnet build
```

### 运行

```bash
dotnet run --project src/SiliconLife.Default
```

### 发布（单文件）

```bash
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

## 开发路线

- [x] 第一阶段：控制台 AI 对话
- [x] 第二阶段：框架骨架（MainLoop + TickObject）
- [x] 第三阶段：第一个硅基人（灵魂文件驱动）
- [x] 第四阶段：持久化记忆（ChatSystem）
- [x] 第五阶段：工具系统 + 执行器
- [ ] 第六阶段：权限系统
- [ ] 第七阶段：动态编译 + 自我进化
- [ ] 第八阶段：长期记忆 + 任务 + 定时器
- [ ] 第九阶段：CoreHost + 多硅基人协作
- [ ] 第十阶段：Web 界面
- [ ] 第十一阶段：外接 IM（飞书 / WhatsApp / Telegram）
- [ ] 第十二阶段：知识图谱、插件及其他

## 文档

- [架构设计](architecture.md) — 系统设计、调度机制、组件架构
- [安全设计](security.md) — 权限模型、执行器、动态编译安全
- [开发路线](roadmap.md) — 详细的 12 阶段开发计划

## 许可证

本项目基于 Apache License 2.0 开源 — 详见 [LICENSE](../LICENSE) 文件。

## 作者

天源垦骥 (Hoshino Kennji) — [B站](https://space.bilibili.com/617827040) | [YouTube](https://www.youtube.com/@hoshinokennji)
