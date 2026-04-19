# 硅基生命群

**⚠️ 警告：动态编译功能已可用，但需要代码模板才能正常工作。深度测试正在进行中。**

基于 .NET 9 的多智能体协作平台，AI 智能体（硅基人）可通过 Roslyn 动态编译实现自我进化。

[English](../../README.md) | [繁體中文](../zh-HK/README.md) | [日本語](../ja-JP/README.md)

## 特性

- **多智能体编排** — 由硅基主理人统一管理，基于 Tick 驱动的时间分片公平调度（MainLoop + TickObject + 看门狗 + 熔断器）
- **灵魂文件驱动** — 每个硅基人由核心提示词文件（`soul.md`）驱动，定义其个性与行为模式
- **身体-大脑架构** — 身体（SiliconBeing）保持存活并检测触发场景；大脑（ContextManager）加载历史、调用 AI、执行工具并持久化响应
- **工具调用循环** — AI 返回 tool_calls → 执行工具 → 反馈结果 → AI 继续 → 直到返回纯文本
- **执行器-权限安全体系** — 所有磁盘、网络、命令行操作均通过执行器进行权限验证
  - 5 级权限查询链：IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - 所有权限决策均有审计日志
- **Token 用量审计** — 内置 Token 用量追踪与报告（`ITokenUsageAudit` / `TokenUsageAuditManager`）
- **多 AI 后端支持** — 支持 Ollama（本地）和阿里云百炼（云端）
  - **Ollama** — 本地模型托管，原生 HTTP API
  - **DashScope（百炼）** — 云端 AI 服务，OpenAI 兼容 API，多地域部署，13+ 模型支持（千问、DeepSeek、GLM、Kimi、Llama）
- **32 种历法体系** — 多历法支持，包括公历、中国农历、伊斯兰历、希伯来历、日本年号历、波斯历、玛雅历等
- **最小化依赖** — 核心库仅依赖 Microsoft.CodeAnalysis.CSharp 用于 Roslyn 动态编译
- **零数据库依赖** — 基于文件系统存储（JSON），支持通过 `ITimeStorage` 进行时间索引查询
- **国际化** — 内置简体中文、繁体中文和英文支持
- **Web 界面** — 内置 HTTP 服务器，支持 SSE，多种皮肤，完整的仪表盘
  - **皮肤系统** — 4 种内置皮肤（Admin、Chat、Creative、Dev），可插拔 ISkin 接口，自动发现注册
  - **17 个控制器** — About、Audit、Being、Chat、CodeBrowser、Config、Dashboard、Executor、Init、Knowledge、Log、Memory、Permission、PermissionRequest、Project、Task、Timer
  - **实时更新** — 通过 SSE（Server-Sent Events）实现聊天消息、硅基人状态和系统事件的实时推送
  - **HTML/CSS/JS 构建器** — 服务端标记语言生成，通过 `H`、`CssBuilder` 和 `JsBuilder` 实现（零前端框架依赖）
  - **本地化** — 三种内置语言环境（zh-CN、zh-HK、en-US），通过 LocalizationManager 解析

## 技术栈

| 组件 | 技术 |
|------|------|
| 运行时 | .NET 9 |
| 开发语言 | C# |
| AI 接入 | Ollama（本地）、阿里云 DashScope（云端） |
| 数据存储 | 文件系统（JSON + 时间索引目录结构） |
| Web 服务器 | HttpListener（.NET 内置） |
| 动态编译 | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） |
| 开源许可证 | Apache-2.0 |

## 项目结构

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # 核心库（接口、抽象类）
│   │   ├── ServiceLocator.cs             # 全局服务定位器：Register/Get、ChatSystem、IMManager、AuditLogger、GlobalACL、BeingFactory、BeingManager、DynamicBeingLoader、TokenUsageAudit
│   │   ├── Runtime/                       # MainLoop、TickObject、CoreHost、CoreHostBuilder、PerformanceMonitor
│   │   ├── SiliconBeing/                  # SiliconBeingBase、SiliconBeingManager、SiliconCurator、ISiliconBeingFactory、SoulFileManager、Memory、TaskSystem、TimerSystem
│   │   ├── AI/                            # IAIClient、IAIClientFactory、ContextManager（"大脑"）、Message、AIRequest/AIResponse
│   │   ├── Audit/                         # ITokenUsageAudit、TokenUsageAuditManager、TokenUsageRecord、TokenUsageSummary、TokenUsageQuery
│   │   ├── Chat/                          # ChatSystem、IChatService、SimpleChatService、SessionBase、SingleChatSession、GroupChatSession、BroadcastChannel、ChatMessage
│   │   ├── Executors/                     # ExecutorBase、DiskExecutor、NetworkExecutor、CommandLineExecutor、ExecutorRequest、ExecutorResult
│   │   ├── Tools/                         # ITool、ToolManager（反射扫描）、ToolCall/ToolResult、ToolDefinition、SiliconManagerOnlyAttribute
│   │   ├── Security/                      # PermissionManager、GlobalACL、AuditLogger、UserFrequencyCache、PermissionResult、PermissionType、IPermissionCallback、IPermissionAskHandler
│   │   ├── IM/                            # IIMProvider、IMManager（消息路由）
│   │   ├── Storage/                       # IStorage、ITimeStorage（键值存储 + 时间索引存储）
│   │   ├── Config/                        # ConfigDataBase、Config（单例 + JSON）、ConfigDataBaseConverter、GuidConverter、AIClientConfigAttribute、ConfigGroupAttribute、ConfigIgnoreAttribute、DirectoryInfoConverter
│   │   ├── Localization/                  # LocalizationBase、LocalizationManager、Language 枚举
│   │   ├── Logging/                       # ILogger、ILoggerProvider、LogEntry、LogLevel、LogManager
│   │   ├── Compilation/                   # DynamicBeingLoader、DynamicCompilationExecutor、SecurityScanner、CodeEncryption
│   │   └── Time/                          # IncompleteDate（时间范围查询）
│   │
│   └── SiliconLife.Default/               # 默认实现 + 程序入口
│       ├── Program.cs                     # 应用程序入口（组装所有组件）
│       ├── AI/                            # OllamaClient、OllamaClientFactory（原生 Ollama HTTP API）；DashScopeClient、DashScopeClientFactory（阿里云百炼）
│       ├── SiliconBeing/                  # DefaultSiliconBeing、DefaultSiliconBeingFactory
│       ├── Calendar/                      # 32 种历法实现：Buddhist、Cherokee、ChineseLunar、ChulaSakarat、Coptic、Dai、DehongDai、Ethiopian、FrenchRepublican、Gregorian、Hebrew、Indian、Inuit、Islamic、Japanese、Javanese、Juche、Julian、Khmer、Mayan、Mongolian、Persian、RepublicOfChina、Roman、Saka、Sexagenary、Tibetan、Vietnamese、VikramSamvat、Yi、Zoroastrian
│       ├── Executors/                     # 默认执行器实现
│       ├── IM/                            # WebUIProvider（Web UI 作为 IM 通道）、IMPermissionAskHandler
│       ├── Tools/                         # 内置工具：日历、聊天、配置、主理人、磁盘、动态编译、记忆、网络、系统、任务、定时器、Token审计
│       ├── Config/                        # DefaultConfigData
│       ├── Localization/                  # ZhCN、ZhHK、EnUS、DefaultLocalizationBase
│       ├── Logging/                       # ConsoleLoggerProvider、FileSystemLoggerProvider
│       ├── Storage/                       # FileSystemStorage、FileSystemTimeStorage
│       ├── Security/                      # DefaultPermissionCallback
│       ├── Runtime/                       # TestTickObject
│       └── Web/                           # Web UI 实现
│           ├── Controllers/               # 17 个控制器：About、Audit、Being、Chat、CodeBrowser、Config、Dashboard、Executor、Init、Knowledge、Log、Memory、Permission、PermissionRequest、Project、Task、Timer
│           ├── Models/                    # ViewModel：AboutViewModel、AuditViewModel、BeingViewModel、ChatMessage、ChatViewModel、CodeBrowserViewModel、ConfigViewModel、DashboardViewModel、ExecutorViewModel、KnowledgeViewModel、LogViewModel、MemoryViewModel、PermissionViewModel、PermissionRequestViewModel、ProjectViewModel、TaskViewModel、TimerViewModel、ViewModelBase
│           ├── Views/                     # HTML 视图：ViewBase、AboutView、AuditView、BeingView、ChatView、CodeBrowserView、CodeEditorView、ConfigView、DashboardView、ExecutorView、KnowledgeView、LogView、MarkdownEditorView、MemoryView、PermissionView、ProjectView、TaskView、TimerView
│           ├── Skins/                     # 4 种皮肤：Admin（专业）、Chat（对话）、Creative（创意）、Dev（开发者）
│           ├── ISkin.cs                   # 皮肤接口 + SkinPreviewInfo + SkinManager（自动发现）
│           ├── Controller.cs              # 控制器基类
│           ├── WebHost.cs                 # HTTP 服务器（HttpListener）
│           ├── Router.cs                  # 请求路由（模式匹配）
│           ├── SSEHandler.cs              # 服务器推送事件
│           ├── WebSecurity.cs             # Web 安全工具
│           ├── H.cs                       # 流式 HTML 构建器 DSL
│           ├── CssBuilder.cs              # CSS 构建工具
│           └── JsBuilder.cs               # JavaScript 构建工具
│
├── docs/
│   └── zh-CN/                             # 中文文档
```

## 架构概览

```
MainLoop（专用线程，看门狗 + 熔断器）
  └── TickObject（按优先级排序）
       └── SiliconBeingManager
            └── SiliconBeingRunner（每次 Tick 独立临时线程，带超时和熔断）
                 └── DefaultSiliconBeing.Tick()
                      └── ContextManager.ThinkOnChat()
                           └── IAIClient.Chat() → 工具调用循环 → 持久化到 ChatSystem
```

所有 AI 发起的 I/O 操作均经过安全链：

```
工具调用 → 执行器 → 权限管理器 → [IsCurator → 频率缓存 → 全局ACL → 回调 → 询问用户]
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

应用程序将启动 Web 服务器并自动在浏览器中打开 Web UI。

### 发布（单文件）

```bash
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

## 开发路线

- [x] 第一阶段：控制台 AI 对话
- [x] 第二阶段：框架骨架（MainLoop + TickObject + 看门狗 + 熔断器）
- [x] 第三阶段：第一个硅基人（灵魂文件驱动，身体-大脑架构）
- [x] 第四阶段：持久化记忆（ChatSystem + ITimeStorage）
- [x] 第五阶段：工具系统 + 执行器
- [x] 第六阶段：权限系统（5 级查询链、审计日志、全局 ACL）
- [x] 第七阶段：动态编译 + 自我进化（Roslyn）
- [x] 第八阶段：长期记忆 + 任务 + 定时器
- [x] 第九阶段：CoreHost + 多硅基人协作
- [x] 第十阶段：Web 界面（HTTP + SSE，17 个控制器，4 种皮肤）
- [x] 第十点五阶段：增量增强（BroadcastChannel、Token审计、32 种历法、工具增强）
- [ ] 第十一阶段：外接 IM（飞书 / WhatsApp / Telegram）
- [ ] 第十二阶段：知识图谱、插件及其他

## 文档

- [架构设计](architecture.md) — 系统设计、调度机制、组件架构
- [安全设计](security.md) — 权限模型、执行器、动态编译安全
- [开发路线](roadmap.md) — 详细的 12 阶段开发计划

## 许可证

本项目基于 Apache License 2.0 开源 — 详见 [LICENSE](../../LICENSE) 文件。

## 作者

天源垦骥 (Hoshino Kennji) — [B站](https://space.bilibili.com/617827040) | [YouTube](https://www.youtube.com/@hoshinokennji)
