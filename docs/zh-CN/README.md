![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**版本：v0.1.0-alpha** | **硅基生命群** — 一个基于 .NET 9 的多智能体协作平台，AI 智能体被称为**硅基生命体**，通过 Roslyn 动态编译实现自我进化。

[English](../README.md) | [Deutsch](../de-DE/README.md) | **中文** | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Français](../fr-FR/README.md) | [Čeština](../cs-CZ/README.md)

## 🌟 核心特性

### 智能体系统
- **多智能体编排** — 由*硅基主理人*统一管理，采用时钟驱动的时隙公平调度机制
- **灵魂文件驱动** — 每个硅基生命体由核心提示文件（`soul.md`）驱动，定义独特个性和行为模式
- **身体-大脑架构** — *身体*（SiliconBeing）维持生命体征并检测触发场景；*大脑*（ContextManager）负责加载历史、调用 AI、执行工具和持久化响应
- **自我进化能力** — 通过 Roslyn 动态编译技术，硅基生命体可以重写自己的代码实现进化
- **活动状态管理** — 支持 Idle（空闲）、Working（工作）、Error（错误）、Stopped（已停止）四种活动状态，连续 10 次错误自动进入 Stopped 状态

### 插件系统
- **插件扩展架构** — 通过 IPlugin 接口实现功能扩展，支持从目录动态加载插件 DLL
- **安全沙箱** — 插件加载器执行严格的安全扫描，禁止访问 System.IO、System.Net 等命名空间
- **隔离加载** — 使用自定义 AssemblyLoadContext 隔离加载，防止插件影响主程序稳定性
- **工具集成** — 插件可通过 ITool 接口注册自定义工具，自动集成到工具调用循环

### 工具与执行
- **24 个内置工具** — 涵盖日历、聊天、配置、磁盘、网络、记忆、任务、定时器、知识库、工作笔记、WebView 浏览器、热重载等
- **热重载工具** — 支持 SiliconLife.Fast 在运行中自动编译、更新文件并重启，无需手动干预
- **工具调用循环** — AI 返回工具调用 → 执行工具 → 结果反馈给 AI → 持续循环直到返回纯文本响应
- **执行器-权限安全** — 所有 I/O 操作通过执行器进行严格的权限验证
  - 5 级权限链：IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - 完整的审计日志记录所有权限决策

### AI 与知识
- **多 AI 后端支持**
  - **Ollama** — 本地模型部署，使用原生 HTTP API
  - **阿里云百炼（DashScope）** — 云端 AI 服务，兼容 OpenAI API，支持 13+ 模型，多区域部署
  - **火山引擎 Ark（VolcengineArk）** — 字节跳动云端 AI 服务，支持流式和非流式模式，内置速率控制
- **32 种日历系统** — 全球主要历法全覆盖，包括公历、农历、伊斯兰历、希伯来历、日本历、波斯历、玛雅历、中国历史历法等
- **知识网络系统** — 基于三元组（主体-关系-客体）的知识图谱，支持存储、查询和路径发现

### Web 界面
- **现代化 Web UI** — 内置 HTTP 服务器，支持 SSE 实时更新
- **7 种皮肤主题** — 管理版、聊天版、创作版、开发版、高对比度、浅色、极简，支持自动发现和切换
- **20+ 个控制器** — 完整的系统管理、聊天、配置、监控功能
- **零前端框架依赖** — 通过 `H`、`CssBuilder` 和 `JsBuilder` 在服务端生成 HTML/CSS/JS

### 国际化与本地化
- **29 种语言实现**全面支持，涵盖 2 种书写系统和多个地区变体
  - **简体中文**：zh-CN（中国大陆）、zh-SG（新加坡）、zh-MY（马来西亚）（3 种）
  - **繁体中文**：zh-HK（香港）、zh-TW（台湾）、zh-MO（澳门）（3 种）
  - **英语**：en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY（10 种）
  - **西班牙语**：es-ES, es-MX（2 种）
  - **德语**：de-DE, de-AT, de-CH, de-LU, de-LI（5 种）
  - **法语**：fr-FR, fr-CA, fr-CH（3 种）
  - **日语**：ja-JP | **韩语**：ko-KR | **捷克语**：cs-CZ（3 种）

### 数据与存储
- **SpeedyPack 高性能存储** — Fast 版本使用自研 .spk 存储引擎，内存目录映射 + 条目缓存 + 异步写入队列
- **文件系统存储** — Default 版本使用纯文件系统 JSON 存储
- **时间索引查询** — 通过 `ITimeStorage` 接口支持按时间范围的高效查询
- **自动压缩** — SpeedyPack 支持定时自动压缩，回收空闲空间
- **最小依赖** — 核心库仅依赖 Microsoft.CodeAnalysis.CSharp 用于动态编译

## 🔄 双版本架构

本项目提供两个实现版本，满足不同场景需求：

### SiliconLife.Default（默认版本）
- **定位**：默认实现，主要用于验证架构可行性
- **运行模式**：控制台应用程序
- **存储方式**：纯文件系统 JSON 存储
- **适用场景**：数据安全性要求高、内存资源受限、数据量小的场景
- **特点**：简单可靠、数据持久化即时、无内存丢失风险
- **角色说明**：作为架构验证的基准实现，适合初次接触、开发调试或数据安全优先的场景
- **启动命令**：`dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast（高性能版本）
- **定位**：主推生产版本
- **运行模式**：Windows 窗体应用程序（支持系统托盘）
- **存储方式**：SpeedyPack 内存存储 + 异步批量持久化（.spk 文件格式）
- **适用场景**：高并发、低延迟、大数据量场景
- **特点**：
  - 极致性能优化
  - 托盘后台运行，支持托盘状态窗口实时监控
  - SpeedyPack 引擎 + 自动压缩保证数据安全
  - Component UI 架构，30+ 声明式组件
  - 7 种皮肤主题，支持自动发现和切换
  - 热重载工具支持在线更新和重启
- **性能提升**：存储读取延迟降低 1000 倍，写入延迟降低 15000 倍，并发处理能力提升 50 倍
- **角色说明**：经过深度优化的生产级实现，是长期运行和实际生产环境的首选
- **启动命令**：`dotnet run --project src/SiliconLife.Fast`

### 版本对比

| 特性 | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| **运行模式** | 控制台程序 | 窗体程序（系统托盘） |
| **用户界面** | Web UI（浏览器访问） | 托盘图标 + 托盘窗口 + Web UI |
| **系统托盘** | ❌ 无 | ✅ 支持最小化到托盘 |
| **后台运行** | ❌ 控制台关闭即退出 | ✅ 托盘后台持续运行 |
| **存储方式** | 文件系统 JSON 存储 | SpeedyPack 内存存储 + 异步持久化 |
| **存储引擎** | 文件系统 I/O | SiliconLife.Speedy（.spk 格式） |
| **读取延迟** | ~10ms（磁盘 I/O） | ~0.01ms（内存操作） |
| **写入延迟** | ~15ms（同步写入） | ~0.001ms（异步写入） |
| **并发能力** | ~100 req/s | ~5000 req/s |
| **内存占用** | ~200MB | ~500MB |
| **数据安全性** | 极高（即时持久化） | 高（异步持久化 + 自动压缩） |
| **适用场景** | 数据安全优先、小数据量 | 性能优先、大数据量、高并发 |

## 🛠️ 技术栈

| 组件 | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| 运行时 | .NET 9 | .NET 9 Windows |
| 编程语言 | C# | C# |
| 应用类型 | 控制台应用程序 | Windows 窗体应用程序 |
| AI 集成 | Ollama（本地）、阿里云百炼（云端） | Ollama（本地）、阿里云百炼（云端）、火山引擎Ark（云端） |
| 数据存储 | 文件系统（JSON + 时间索引目录） | SpeedyPack（.spk 格式，内存映射 + 异步持久化） |
| Web 服务器 | HttpListener（.NET 内置） | HttpListener（.NET 内置） |
| 动态编译 | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） |
| 浏览器自动化 | Playwright（WebView） | Playwright（WebView） |
| 插件系统 | ✅ 支持（IPlugin + PluginLoader） | ✅ 支持（IPlugin + PluginLoader） |
| 系统托盘 | ❌ 不支持 | ✅ 支持（NotifyIcon） |
| 许可证 | Apache-2.0 | Apache-2.0 |

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
│   │   ├── Plugins/                       # 插件系统（IPlugin 接口、PluginLoader 加载器）
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
│   ├── SiliconLife.Common/                # 共享实现（两个版本共用）
│   │   ├── AI/                            # AI 客户端工厂（Ollama、DashScope、VolcengineArk）
│   │   ├── Calendar/                      # 32 种日历实现
│   │   ├── Localization/                  # 本地化基类
│   │   ├── Security/                      # 权限管理器
│   │   ├── SiliconBeing/                  # 默认硅基生命体实现
│   │   ├── Tools/                         # 通用工具实现（含热重载工具）
│   │   └── WebView/                       # WebView 接口
│   │
│   ├── SiliconLife.Default/               # 默认实现 + 应用程序入口（控制台版）
│   │   ├── Program.cs                     # 入口点（装配所有组件）
│   │   ├── Config/                        # 默认配置数据
│   │   ├── Executors/                     # 默认执行器实现
│   │   ├── Help/                          # 帮助文档系统
│   │   ├── IM/                            # WebUI 提供者
│   │   ├── Knowledge/                     # 知识网络实现
│   │   ├── Localization/                  # 21 种语言本地化
│   │   ├── Logging/                       # 日志提供者实现
│   │   ├── Project/                       # 项目系统实现
│   │   ├── Runtime/                       # 测试时钟对象
│   │   ├── Security/                      # 默认权限回调
│   │   ├── SiliconBeing/                  # 默认硅基生命体实现
│   │   ├── Storage/                       # 文件系统存储实现
│   │   ├── Tools/                         # 内置工具实现
│   │   ├── WebView/                       # Playwright WebView 实现
│   │   └── Web/                           # Web UI 实现
│   │       ├── Controllers/               # 20+ 个控制器
│   │       ├── Models/                    # 视图模型
│   │       ├── Views/                     # HTML 视图
│   │       └── Skins/                     # 4 种皮肤主题
│   │
│   └── SiliconLife.Fast/                  # 高性能实现 + 应用程序入口（窗体版）
│       ├── Program.cs                     # 入口点（窗体应用程序）
│       ├── Config/                        # 配置数据（与 Default 共享）
│       ├── Executors/                     # 优化执行器实现
│       ├── Help/                          # 帮助文档系统
│       ├── IM/                            # WebUI 提供者
│       ├── Knowledge/                     # 知识网络实现（内存优化）
│       ├── Localization/                  # 21 种语言本地化
│       ├── Logging/                       # 高性能日志提供者
│       ├── Project/                       # 项目系统实现
│       ├── Security/                      # 优化权限回调
│       ├── SiliconBeing/                  # 高性能硅基生命体实现
│       ├── Storage/                       # SpeedyPack 存储适配器
│       ├── Tools/                         # 优化内置工具实现
│       ├── Tray/                          # 系统托盘（9 种语言本地化）
│       ├── WebView/                       # Playwright WebView 实现
│       └── Web/                           # 高性能 Web UI 实现
│           ├── Component/                 # UI 组件库（30+ 组件）
│           ├── Controllers/               # 20+ 个控制器
│           ├── Models/                    # 视图模型
│           ├── Views/                     # HTML 视图
│           └── Skins/                     # 7 种皮肤主题
│
│   ├── SiliconLife.Speedy/                # SpeedyPack 高性能存储引擎
│   │   ├── SpeedyPack.cs                  # 核心类（内存目录映射 + 缓存 + 异步写入）
│   │   ├── SpeedyPackOptions.cs           # 配置选项（缓存 TTL、最大条目数等）
│   │   ├── IPackTransaction.cs            # 事务接口
│   │   ├── SpkFileInfo.cs                 # 文件信息
│   │   └── Internal/                      # 内部实现
│       │   ├── DirectoryMap.cs            # 内存目录映射
│       │   ├── EntryCache.cs              # 条目缓存
│       │   ├── FreeList.cs                # 空闲空间管理
│       │   ├── PackFileReader.cs          # 包文件读取器
│       │   ├── PackFileWriter.cs          # 包文件写入器
│       │   ├── WriteQueue.cs              # 异步写入队列
│       │   ├── WriteOperation.cs          # 写入操作
│       │   ├── SpeedyTransaction.cs       # 事务实现
│       │   ├── SpkHeader.cs              # 包文件头
│       │   └── PathNormalizer.cs          # 路径规范化
│   │
│   └── SiliconLife.Speedy.Manager/        # SpeedyPack 管理工具（WPF）
│       ├── MainForm.cs                    # 主窗体
│       ├── Program.cs                     # 入口点
│       └── slc.ico                        # 应用图标
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

#### 方式 1：运行 Default 版本（控制台应用程序）

```bash
dotnet run --project src/SiliconLife.Default
```

应用程序将启动 Web 服务器并自动在浏览器中打开 Web UI。

**适用场景**：
- ✅ 数据安全性要求极高
- ✅ 内存资源受限（RAM < 2GB）
- ✅ 数据量小，短期使用
- ✅ 开发调试阶段

#### 方式 2：运行 Fast 版本（Windows 窗体应用程序）

```bash
dotnet run --project src/SiliconLife.Fast
```

应用程序将以窗体模式启动，最小化到系统托盘，后台持续运行。

**适用场景**：
- ✅ 高并发场景（> 5 用户）
- ✅ 大数据量（使用 3 个月以上）
- ✅ 需要低延迟响应
- ✅ 需要托盘后台运行

### 发布单文件

```bash
# Windows - Default 版本
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Windows - Fast 版本
dotnet publish src/SiliconLife.Fast -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux - 仅 Default 版本
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - 仅 Default 版本
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
- [x] 阶段 11：SpeedyPack 存储引擎（替换 LiteDB、内存映射、异步写入队列、自动压缩）
- [x] 阶段 12：插件系统（IPlugin 接口、PluginLoader 安全沙箱、隔离加载、工具集成）

### 🚧 计划中
- [ ] 阶段 13：外部即时通讯集成（飞书 / WhatsApp / Telegram）
- [ ] 阶段 14：技能生态系统（插件市场、技能包分发）

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

## 💡 版本选择指南

### 我应该使用哪个版本？

**SiliconLife.Default（默认实现 — 验证架构可行性）：**
- 📌 您第一次接触本项目，希望快速了解系统架构
- 📌 您正在进行开发调试，需要简单直接的运行方式
- 📌 数据安全性是您的首要考虑
- 📌 您的系统内存小于 4GB
- 📌 您只需要单人使用或数据量较小

**SiliconLife.Fast（主推生产版本）：**
- ⚡ 您需要长期稳定运行的生产环境
- ⚡ 您已经熟悉系统架构，准备正式部署
- ⚡ 您需要支持多用户并发访问
- ⚡ 您需要系统托盘后台运行
- ⚡ 您追求极致的性能体验

> **总体建议**：SiliconLife.Default 适合作为架构验证和入门体验；对于实际生产环境，强烈推荐使用 SiliconLife.Fast。

### 可以从 Default 迁移到 Fast 吗？

**完全可以！** 两个版本共享相同的：
- ✅ 配置文件格式（config.json）
- ✅ 工具接口
- ✅ Being 配置
- ✅ Web UI 界面

**迁移步骤：**
1. 备份您的 Default 数据目录
2. 使用相同的数据目录启动 Fast 版本
3. Fast 会自动将现有数据导入 SpeedyPack 存储引擎
4. 验证功能正常后，即可日常使用 Fast 版本

### 两个版本可以共存吗？

**可以！** 推荐以下部署策略：

**策略 1：Default 验证，Fast 生产**
```
开发/验证环境：SiliconLife.Default（验证架构、调试功能）
生产环境：SiliconLife.Fast（高性能、后台运行、处理实时请求）
```

**策略 2：Fast 主运行，Default 定期备份**
```
SiliconLife.Fast（日常使用，处理实时请求）
    ↓ 定期备份
SiliconLife.Default（冷数据归档，数据安全兜底）
```

## 📄 许可证

本项目采用 Apache License 2.0 许可证 — 详见 [LICENSE](../../LICENSE) 文件。

## 👨‍💻 作者

**天源垦骥**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- 码云: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- 哔哩哔哩: [617827040](https://space.bilibili.com/617827040)

## 🙏 致谢

感谢所有为本项目做出贡献的开发者和 AI 平台提供者。

---

**Silicon Life Collective** — 让 AI 智能体真正"活"起来
