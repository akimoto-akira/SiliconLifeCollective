# Silicon Life Collective

**⚠️ 警告：动态编译可以工作，但需要代码模板才能正常运行。正在进行全面测试。**

一个基于 .NET 9 的多智能体协作平台，AI 智能体被称为**硅基生命体**，通过 Roslyn 动态编译实现自我进化。

[English](README.md) | [中文文档](docs/zh-CN/README.md) | [繁體中文](docs/zh-HK/README.md) | [Español](docs/es-ES/README.md) | [日本語](docs/ja-JP/README.md) | [한국어](docs/ko-KR/README.md)

## 功能特性

- **多智能体编排** — 由*硅基主理人*管理，采用时钟驱动的时隙公平调度（主循环 + 时钟对象 + 看门狗 + 熔断器）
- **灵魂文件驱动** — 每个硅基生命体由核心提示文件（`soul.md`）驱动，定义其个性和行为
- **身体-大脑架构** — *身体*（SiliconBeing）保持存活状态并检测触发场景；*大脑*（ContextManager）加载历史记录、调用 AI、执行工具并持久化响应
- **工具调用循环** — AI 返回工具调用 → 执行工具 → 将结果反馈给 AI → AI 继续 → 直到返回纯文本响应
- **执行器-权限安全** — 所有磁盘、网络和命令行操作都通过执行器进行权限验证
  - 5级权限链：IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - 所有权限决策都有审计日志
- **Token 使用审计** — 通过 `ITokenUsageAudit` / `TokenUsageAuditManager` 内置 token 使用跟踪和报告
- **多 AI 后端** — 支持 Ollama（本地）和阿里云百炼（云端）
  - **Ollama** — 本地模型托管，使用原生 HTTP API
  - **百炼（DashScope）** — 云端 AI 服务，兼容 OpenAI API，多区域部署，支持 13+ 模型（通义千问、DeepSeek、GLM、Kimi、Llama）
- **32 种日历系统** — 多日历支持，包括公历、农历、伊斯兰历、希伯来历、日本历、波斯历、玛雅历等
- **最小依赖** — 核心库仅依赖 Microsoft.CodeAnalysis.CSharp 用于 Roslyn 动态编译
- **零数据库依赖** — 基于文件的存储（JSON），通过 `ITimeStorage` 支持时间索引查询
- **本地化** — 全面的多语言支持，包含 20 种语言变体
  - 中文：zh-CN, zh-HK, zh-SG, zh-MO, zh-TW, zhMY（6 种变体）
  - 英文：en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY（10 种变体）
  - 西班牙语：es-ES, es-MX（2 种变体）
  - 日语：ja-JP
  - 韩语：ko-KR
- **Web UI** — 内置 HTTP 服务器，支持 SSE，多种皮肤和全面的仪表板
  - **皮肤系统** — 4 种内置皮肤（管理版、聊天版、创作版、开发版），支持可插拔的 ISkin 接口和自动发现
  - **20+ 个控制器** — 关于、审计、生命体、聊天、聊天历史、代码浏览器、代码悬浮提示、配置、仪表板、执行器、初始化、知识、日志、记忆、权限、权限请求、项目、任务、定时器
  - **实时更新** — SSE（服务器发送事件）用于聊天消息、生命体状态和系统事件
  - **HTML/CSS/JS 构建器** — 通过 `H`、`CssBuilder` 和 `JsBuilder` 进行服务器端标记生成（零前端框架依赖）
  - **本地化** — 20 种内置语言变体，通过 LocalizationManager 解析
  - **聊天历史查看** — 完整的硅基生命体聊天历史浏览功能，支持会话列表和消息详情
  - **文件上传支持** — 文件源对话框和文件上传功能
  - **加载指示器** — 聊天页面的加载状态指示器和主理人会话自动选择

## 技术栈

| 组件 | 技术 |
|-----------|-----------|
| 运行时 | .NET 9 |
| 语言 | C# |
| AI 集成 | Ollama（本地）、阿里云百炼（云端） |
| 存储 | 文件系统（JSON + 时间索引目录） |
| Web 服务器 | HttpListener（.NET 内置） |
| 动态编译 | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） |
| 许可证 | Apache-2.0 |

## 项目结构

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # 核心库（接口、抽象类）
│   │   ├── ServiceLocator.cs             # 全局服务定位器：Register/Get, ChatSystem, IMManager, AuditLogger, GlobalACL, BeingFactory, BeingManager, DynamicBeingLoader, TokenUsageAudit
│   │   ├── Runtime/                       # 主循环、时钟对象、核心主机、核心主机构建器、性能监控器
│   │   ├── SiliconBeing/                  # 硅基生命体基类、硅基生命体管理器、硅基主理人、硅基生命体工厂接口、灵魂文件管理器、记忆、任务系统、定时器系统
│   │   ├── AI/                            # AI 客户端接口、AI 客户端工厂接口、上下文管理器（"大脑"）、消息、AI 请求/AI 响应
│   │   ├── Audit/                         # Token 使用审计接口、Token 使用审计管理器、Token 使用记录、Token 使用摘要、Token 使用查询
│   │   ├── Chat/                          # 聊天系统、聊天服务接口、简单聊天服务、会话基类、单聊会话、群聊会话、广播频道、聊天消息
│   │   ├── Executors/                     # 执行器基类、磁盘执行器、网络执行器、命令行执行器、执行器请求、执行器结果
│   │   ├── Tools/                         # 工具接口、工具管理器（反射扫描）、工具调用/工具结果、工具定义、硅基管理员专用属性
│   │   ├── Security/                      # 权限管理器、全局访问控制列表、审计日志器、用户频率缓存、权限结果、权限类型、权限回调接口、权限询问处理器接口
│   │   ├── IM/                            # 即时通讯提供者接口、即时通讯管理器（消息路由）
│   │   ├── Storage/                       # 存储接口、时间存储接口（键值对 + 时间索引）
│   │   ├── Config/                        # 配置数据基类、配置（单例 + JSON）、配置数据基类转换器、Guid 转换器、AI 客户端配置属性、配置组属性、配置忽略属性、目录信息转换器
│   │   ├── Localization/                  # 本地化基类、本地化管理器、语言枚举
│   │   ├── Logging/                       # 日志接口、日志提供者接口、日志条目、日志级别、日志管理器
│   │   ├── Compilation/                   # 动态生命体加载器、动态编译执行器、安全扫描器、代码加密
│   │   └── Time/                          # 不完整日期（时间范围查询）
│   │
│   └── SiliconLife.Default/               # 默认实现 + 入口点
│       ├── Program.cs                     # 应用程序入口（装配所有组件）
│       ├── AI/                            # Ollama 客户端、Ollama 客户端工厂（原生 Ollama HTTP API）；百炼客户端、百炼客户端工厂（阿里云百炼）
│       ├── SiliconBeing/                  # 默认硅基生命体、默认硅基生命体工厂
│       ├── Calendar/                      # 32 种日历实现：佛历、切罗基历、农历、朱拉萨卡拉特历、科普特历、傣历、德宏傣历、埃塞俄比亚历、法国共和历、公历、希伯来历、印度历、因纽特历、伊斯兰历、日本历、爪哇历、主体历、儒略历、高棉历、玛雅历、蒙古历、波斯历、民国历、罗马历、萨卡历、干支历、藏历、越南历、维克拉姆桑巴特历、彝历、祆历
│       ├── Executors/                     # 默认执行器实现
│       ├── IM/                            # WebUI 提供者（Web UI 作为即时通讯频道）、即时通讯权限询问处理器
│       ├── Tools/                         # 内置工具：日历、聊天、配置、主理人、磁盘、动态编译、记忆、网络、系统、任务、定时器、Token 审计
│       ├── Config/                        # 默认配置数据
│       ├── Localization/                  # 简体中文、繁体中文、美式英语、日语、韩语、西班牙语、默认本地化基类、其他英语（英式英语、加拿大英语、澳大利亚英语、印度英语、新加坡英语、南非英语、爱尔兰英语、新西兰英语、马来西亚英语）、其他中文（新加坡中文、澳门中文、台湾中文、马来西亚中文）、其他西班牙语（墨西哥西班牙语）
│       ├── Logging/                       # 控制台日志提供者、文件系统日志提供者
│       ├── Storage/                       # 文件系统存储、文件系统时间存储
│       ├── Security/                      # 默认权限回调
│       ├── Runtime/                       # 测试时钟对象
│       └── Web/                           # Web UI 实现
│           ├── Controllers/               # 18 个控制器：关于、审计、生命体、聊天、代码浏览器、代码悬浮提示、配置、仪表板、执行器、初始化、知识、日志、记忆、权限、权限请求、项目、任务、定时器
│           ├── Models/                    # 视图模型：关于视图模型、审计视图模型、生命体视图模型、聊天消息、聊天视图模型、代码浏览器视图模型、配置视图模型、仪表板视图模型、执行器视图模型、知识视图模型、日志视图模型、记忆视图模型、权限视图模型、权限请求视图模型、项目视图模型、任务视图模型、定时器视图模型、视图模型基类
│           ├── Views/                     # 19 个 HTML 视图：视图基类、关于视图、审计视图、生命体视图、聊天视图、代码浏览器视图、代码编辑器视图、配置视图、仪表板视图、执行器视图、知识视图、日志视图、Markdown 编辑器视图、记忆视图、权限视图、项目视图、灵魂编辑器视图、任务视图、定时器视图
│           ├── Skins/                     # 4 种皮肤：管理版（专业）、聊天版（对话）、创作版（艺术）、开发版（开发者导向）
│           ├── ISkin.cs                   # 皮肤接口 + 皮肤预览信息 + 皮肤管理器（自动发现）
│           ├── Controller.cs              # 控制器基类
│           ├── WebHost.cs                 # HTTP 服务器（HttpListener）
│           ├── Router.cs                  # 请求路由，支持模式匹配
│           ├── SSEHandler.cs              # 服务器发送事件
│           ├── WebSecurity.cs             # Web 安全工具
│           ├── H.cs                       # 流式 HTML 构建器 DSL
│           ├── CssBuilder.cs              # CSS 构建器工具
│           └── JsBuilder.cs               # JavaScript 构建器工具
│
├── docs/
│   └── zh-CN/                             # 中文文档
```

## 架构概览

```
主循环（专用线程，看门狗 + 熔断器）
  └── 时钟对象（按优先级排序）
       └── 硅基生命体管理器
            └── 硅基生命体运行器（每次时钟创建临时线程，超时 + 熔断器）
                 └── 默认硅基生命体.Tick()
                      └── 上下文管理器.思考聊天()
                           └── AI 客户端.聊天() -> 工具调用循环 -> 持久化到聊天系统
```

所有 AI 发起的 I/O 操作都必须通过安全链：

```
工具调用 -> 执行器 -> 权限管理器 -> [IsCurator -> 频率缓存 -> 全局访问控制列表 -> 回调 -> 询问用户]
```

## 快速开始

### 前置条件

- .NET 9 SDK
- AI 后端（选择其一）：
  - **Ollama**：[Ollama](https://ollama.com) 在本地运行并已拉取模型（例如 `ollama pull llama3`）
  - **阿里云百炼**：从[百炼控制台](https://bailian.console.aliyun.com/)获取有效的 API 密钥

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

## 路线图

- [x] 阶段 1：控制台 AI 聊天
- [x] 阶段 2：框架骨架（主循环 + 时钟对象 + 看门狗 + 熔断器）
- [x] 阶段 3：第一个带有灵魂文件的硅基生命体（身体-大脑架构）
- [x] 阶段 4：持久化记忆（聊天系统 + 时间存储接口）
- [x] 阶段 5：工具系统 + 执行器
- [x] 阶段 6：权限系统（5 级链、审计日志器、全局访问控制列表）
- [x] 阶段 7：动态编译 + 自我进化（Roslyn）
- [x] 阶段 8：长期记忆 + 任务 + 定时器
- [x] 阶段 9：核心主机 + 多智能体协作
- [x] 阶段 10：Web UI（HTTP + SSE，18 个控制器，4 种皮肤）
- [x] 阶段 10.5：增量增强（广播频道、Token 审计、32 种日历、工具增强、20 语言本地化）
- [ ] 阶段 11：外部即时通讯集成（飞书 / WhatsApp / Telegram）
- [ ] 阶段 12：知识图谱、插件系统和技能生态系统

## 文档

- [架构](architecture.md) — 系统设计、调度、组件架构
- [安全](security.md) — 权限模型、执行器、动态编译安全
- [路线图](roadmap.md) — 详细的 12 阶段开发计划

## 许可证

本项目采用 Apache License 2.0 许可证 — 详见 [LICENSE](LICENSE) 文件。

## 作者

Hoshino Kennji — [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective) | [码云](https://gitee.com/hoshinokennji/SiliconLifeCollective) | [YouTube](https://www.youtube.com/@hoshinokennji) | [哔哩哔哩](https://space.bilibili.com/617827040)
