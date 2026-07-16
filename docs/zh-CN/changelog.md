# 变更日志

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | **中文** | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md) | [Русский](../ru-RU/changelog.md)

本项目的所有重要更改都将记录在此文件中。

格式基于 [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)，
本项目遵循 [语义化版本控制](https://semver.org/spec/v2.0.0.html)。

---

## 关于此变更日志

### 项目双版本

本项目提供两个实现版本：

- **SiliconLife.Default**：默认实现，主要用于验证架构可行性。控制台应用程序，文件系统 JSON 存储。
- **SiliconLife.Fast**：主推生产版本。跨平台桌面应用程序（Windows / macOS / Linux），SpeedyPack 内存存储 + 异步持久化，经过深度性能优化。

两个版本共享相同的接口和功能，仅在存储实现和运行模式上有所不同。SiliconLife.Default 作为架构验证基准，SiliconLife.Fast 作为生产环境主推版本。

### 项目起源

- 本项目起源于 2026 年 3 月 20 日。
- 在此项目之前，有一个验证 Demo 因架构设计不合理而失败，导致无法与多个 AI 平台集成。

### 使用的 AI IDE 工具

#### Kiro（Amazon AWS）
- 项目最初由 Kiro 维护，并使用 Spec 模式启动。
- Kiro 是 Amazon AWS 构建的 agentic AI 开发环境。
- 基于 Code OSS（VS Code），支持 VS Code 设置和 Open VSX 兼容插件。
- 具有规格驱动的开发工作流程，用于结构化 AI 编码。

#### Comate AI IDE / 文心快码（百度）
- 偶尔用于文案和文档工作。
- Comate AI IDE 是百度文心于 2025 年 6 月 23 日发布的 AI 原生开发环境工具。
- 行业首个多模态、多智能体协同的 AI IDE。
- 功能包括设计到代码转换和全流程 AI 辅助编码。
- 由百度文心 4.0 X1 Turbo 模型驱动。

#### Trae（字节跳动）
- 2025 年 10 月至 2026 年 4 月期间使用。
- AI IDE，支持智能代码生成和项目管理。

#### Qoder（阿里巴巴）
- 自 2026 年 4 月 18 日起用于项目维护。
- AI 编码平台，支持代码分析、文档生成和多智能体协作。

#### CatPaw（美团）
- 自 2026 年 5 月 6 日起与 Qoder 混合使用。
- 基于美团自研 LongCat 系列模型，具有强大的全代码架构重构能力。

#### DuMate（百度千帆）
- 自 2026 年 7 月起用于代码开发、本地化和文档编写。
- 运行于千帆桌面平台的通用 AI 助手，具备多工具编排、文件操作、浏览器自动化和多步任务执行能力。
- 直接在用户 Windows 桌面上读写本地文件、执行 Shell 命令、进行网络搜索。

### 需求文档

- 本项目的需求文档未公开。
- 需求经过 12 多个国际 AI 平台和大型模型系列的反复验证，产生了超过 2000 行、几乎人类无法理解的用户故事驱动需求文档。

---

## [未发布]

### 2026-07-16

#### 新功能
- `7431312` - 补全 13 个语言文件的 AI 客户端配置翻译 - CsCZ/PlPL 从 stub 改为完整字典实现，其余 10 个文件补充 7 个新客户端（DeepSeek/ZhipuGLM/MoonshotKimi/SiliconFlow/MiniMax/Ernie/Hunyuan）的 ConfigDisplayNames/ConfigDescriptions/ConfigGroupNames 条目，同步更新 6 个 ClientFactory 的配置键元数据
  - 20 个文件变更

#### 文档
- `d6608ea` - 在所有 13 个语言版本的 changelog 中添加 DuMate（百度千帆）的 AI IDE 工具介绍
  - 13 个文件变更

#### 协作框架
- `c607c97` - 注册 DuMate（百度千帆）为常驻 AI 协作者到 .ai-collab 注册表
  - 1 个文件变更


### 2026-07-15

#### 新功能
- `c007263` - 补全 10 个 AI 客户端的帮助文档 - HelpTopics 注册 10 个主题，HelpLocalizationBase 添加 30 个抽象属性，12 个语言文件实现完整 Markdown 帮助内容（平台简介/注册步骤/配置方法/可用模型/计费/常见问题），覆盖 Herdsman/LongCat/QiniuAI/DeepSeek/ZhipuGLM/MoonshotKimi/SiliconFlow/MiniMax/Ernie/Hunyuan
  - 12 个文件变更
- `4634e33` - 实现 7 个国内 AI 平台客户端（DeepSeek/智谱GLM/月之暗面Kimi/硅基流动/MiniMax/百度文心/腾讯混元）- 14 个独立类文件，遵循 LongCatClient 风格，不使用继承，全部 OpenAI 兼容 + Bearer Token，支持 Tool Calling/流式/思考模式，在 DefaultSiliconBeing 和 DefaultSiliconBeingFactory 注册
  - 16 个文件变更

#### 文档
- `108c4ea` - 更新全部 13 语言文档以反映 7 个新 AI 客户端 - 状态 📋→✅，01.AI 标记为已废弃
  - 94 个文件变更


### 2026-07-14

#### 文档
- `344b429` - 全语种 architecture.md AI 平台状态新增「已废弃」状态，标记零一万物为已废弃（停止新用户注册）
  - 13 个文件变更


### 2026-07-07

#### 清理
- `e06e6f2` - 移除 OsmStore 工具链和 TravelCodeWikiWithAI 插件 - 删除 tools/OsmStore.* 三个项目，删除 src/TravelCodeWikiWithAI/ 插件项目，清理 sln 引用，项目回归独立版 TCW 开发路线
  - 45 个文件变更


### 2026-07-06

#### 修复
- `1b15886` - OSM 数据模型标准化与元素类型安全修复
  - 7 个文件变更


### 2026-07-05

#### 新功能
- `be4320b` - TravelCodeWikiWithAI 新增 CLDR 数据提供模块
  - 4 个文件变更


### 2026-07-04

#### 新功能
- `dbcabf3` - 插件权限系统增强 - 重构网络/文件 IO 为 Executor 模式 + GeneratedCodeAttribute 白名单豁免
  - 34 个文件变更
- `e84bb63` - 修复编译错误并新增 TravelCodeWikiWithAI 项目
  - 53 个文件变更

#### 重构
- `9e5a345` - TravelCodeWikiWithAI 全量迁移 PBF 至同步在线 OSM API
  - 4 个文件变更


### 2026-05-31

#### 新功能
- `a5f37bd` - 更新项目思考、对话系统及存储相关功能
  - 13 个文件变更


### 2026-05-30

#### 新功能
- `c3cf429` - 新增 QiniuAIClient AI 客户端（七牛云 AI 大模型推理服务） (ref task-409)
  - 20 个文件变更
- `d04131f` - 新增 LongCatClient AI 客户端（美团 LongCat 大模型） (ref task-408)
  - 19 个文件变更

#### 协作框架
- `e9564f5` - 更新所有修改的文件
  - 140 个文件变更
- `9c8b42f` - 归档 2026-05-29 的 sessions 和 changes
  - 20 个文件变更


### 2026-05-29

#### 新功能
- `d548e48` - 项目思考详情页按轮次（Cycle）分组展示消息并支持折叠 (ref task-407)
  - 23 个文件变更
- `28d893d` - IAIClient 增加多模态能力声明接口 + ChatMessage 增加多模态字段 (ref task-402)
  - 13 个文件变更
- `ebe6a49` - 项目思考详情页增加会话状态、创建时间、完成时间展示 (ref task-406)
  - 22 个文件变更
- `9a53d55` - IAIClient 增加 ContextWindowTokens + Token 预算制 + 工厂配置化 (ref task-401, task-403)
  - 26 个文件变更
- `202b99c` - 新增 HerdsmanClient AI 客户端 + 修复初始化界面下拉菜单不刷新 (ref task-399, task-400)
  - 20 个文件变更
- `285ab2f` - 项目处理记录前端展示 (ref task-397)
  - 25 个文件变更
- `b4b633f` - ThinkOnProject 伪 Session 多轮对话机制 (ref task-395)
  - 13 个文件变更
- `d3e543f` - ThinkOnProject 场景上下文增加可用硅基人信息 (ref task-394)
  - 21 个文件变更
- `07eb628` - BuildRequest 动态注入硅基人项目归属信息 (ref task-396)
  - 21 个文件变更
- `2089696` - Tool 添加 Project 场景支持 + PluginLoader 多目录统一重构
  - 12 个文件变更

#### 修复
- `b80a33b` - 修复项目思考详情页加载提示文本硬编码英文及缺少本地化 (ref task-405)
  - 6 个文件变更
- `90b60c5` - 修复工具调用轮次中 AI 正文 Content 和 Thinking 被隐藏的问题 (ref task-404)
  - 8 个文件变更
- `a7d9a97` - ThinkOnProject 多轮循环续接及项目提醒信息丢失修复
  - 6 个文件变更
- `c0838dd` - 修复 ProjectThinkSession 消息未写入 Cycle 及完成后历史被删除的问题 (ref task-398)
  - 7 个文件变更
- `f3d1794` - 修复硅基人 Project/Broadcast/Stopped 状态本地化缺失及展示异常 (ref task-393)
  - 20 个文件变更
- `3eaa90d` - 移除已删除项目 TravelCodeWikiWithAI 的解决方案引用
  - 1 个文件变更

#### 协作框架
- `f3cbed7` - 注册 task-394~396（ThinkOnProject 增强）
  - 3 个文件变更
- `e1971f5` - 注册 task-393（BeingActivity 本地化与展示修复）
  - 1 个文件变更
- `e710fa4` - 更新 changes commitHash 和 state 会话结束
  - 2 个文件变更
- `4cacc4a` - 归档 2026-05-28 的 sessions 和 changes
  - 4 个文件变更


### 2026-05-28

#### 新功能
- `ae8b673` - 插件目录配置从单一路径升级为多目录列表 (ref task-391)
  - 29 个文件变更
- `aac46c1` - PluginLoader 增加 CS 源码模式，无 DLL 时编译加载插件 (ref task-389)
  - 6 个文件变更

#### 修复
- `63047b0` - 注册所有 PluginLoader 到 ServiceLocator，修复多目录插件反射不全 (ref task-391)
  - 3 个文件变更
- `fcad655` - 修复 directoryList 浏览按钮交互问题 (ref task-392)
  - 9 个文件变更

#### 文档
- `e6d3037` - PluginDemo-22 CS 源码编译加载模式示例 (ref task-390)
  - 21 个文件变更

#### 协作框架
- `09d9e9c` - 归档 30 个已完成任务（task-362~task-391）
  - 2 个文件变更
- `66204a1` - 归档 2026-05-28 的 sessions（8）和 changes（8）
  - 18 个文件变更
- `308a8d0` - 更新 task-391 relatedCommit
  - 1 个文件变更
- `6fc4e05` - 注册 task-389（CS 源码模式）和 task-390（PluginDemo-22）
  - 1 个文件变更


### 2026-05-27

#### 新功能
- `e154a18` - 完成 PluginDemo-21 WorkflowTemplate 完整业务工作流示例 (ref task-388)
  - 19 个文件变更
- `aa771b3` - 实现 PluginCapability 声明式权限系统 (ref task-379)
  - 9 个文件变更
- `5e5e9d1` - 添加 04-SafeSystemIO System.IO 白名单安全类型示例 (ref task-370)
  - 20 个文件变更

#### 文档
- `48f6702` - 对齐 19-TickObject 和 20-SpeedyPack 所有语言 README 翻译至基准 (ref task-386, task-387)
  - 119 个文件变更
- `5d570e5` - 完成 task-378 禁止的字符串反射绕过反例 (ref task-378)
  - 19 个文件变更
- `348c410` - PluginDemo-11 禁止的 P/Invoke 和 unsafe 代码反例 (ref task-377)
  - 19 个文件变更
- `fc92a49` - PluginDemo-10 禁止的反射操作反例 (ref task-376)
  - 19 个文件变更
- `826ad2a` - 创建 PluginDemo-09 禁止进程操作反例插件 (ref task-375)
  - 19 个文件变更
- `7870b05` - 添加 PluginDemo-08 禁止网络操作反例 (ref task-374)
  - 15 个文件变更
- `8636e31` - PluginDemo-07 禁止文件 I/O 操作反例 (ref task-373)
  - 19 个文件变更
- `322312e` - 添加 PluginDemo-06 TrustedAssemblies 受信依赖示例 (ref task-372)
  - 19 个文件变更
- `6df98a0` - 添加 IWorkflowPlugin 工作流插件示例 (ref task-371)
  - 20 个文件变更
- `f3787ba` - PluginDemo-03 IObjectFactory 注册与创建示例 (ref task-369)
  - 20 个文件变更
- `bb4324d` - PluginDemo-02 ITypeRegistry 注册与查询示例 (ref task-368)
  - 20 个文件变更
- `bbdfa3c` - PluginDemo-01 最简 IPlugin 实现示例 (ref task-367)
  - 19 个文件变更

#### 协作框架
- `de44057` - 归档 5 月 25 日和 27 日的 sessions 和 changes
  - 58 个文件变更
- `9e4a84c` - 更新 tasks.json lastCommitHash 为 48f6702
  - 1 个文件变更
- `beb58b2` - 补充 taskIndex 索引（8 pending, 19 completed）
  - 1 个文件变更
- `63f7bfc` - 更新 task-388 relatedCommit (ref task-388)
  - 1 个文件变更
- `e61be6f` - 更新 task-378 relatedCommit (ref task-378)
  - 1 个文件变更
- `dde579b` - 发布 WorkflowTemplate 完整使用示例任务（task-388）
  - 1 个文件变更
- `2294fa7` - 发布 TickObject 和 SpeedyPack 示例任务（task-386~387）
  - 1 个文件变更
- `82b9f63` - 发布 6 个 PluginCapability 示例任务（task-380~385）
  - 1 个文件变更
- `588539b` - 发布 PluginCapability 声明式权限系统任务（task-379）
  - 1 个文件变更
- `37f9c23` - 更新解决方案和项目文件引用
  - 8 个文件变更
- `e1f7892` - 发布 12 个 PluginDemo 待领取任务（task-367~378）
  - 3 个文件变更
- `87ae858` - 创建 PluginDemo 插件正反例任务注册（task-367）
  - 2 个文件变更
- `f77a102` - 归档 2026-05-26 的 sessions 和 changes
  - 7 个文件变更

## [Alpha-0.2] - 2026-05-16

### 2026-05-16

#### 发布准备
- `476d839` - 添加 alpha-0.2 发布任务
  - 创建 task-114（CHANGELOG 编写）和 task-115（版本号更新）
  - 1 个文件变更

### 2026-05-15

#### 基础设施
- `672627b` - 添加 Gitee 同步工作流（带权限配置）
  - 更新 sync-from-gitee.yml 工作流权限配置
  - 1 个文件变更，7 行新增，4 行删除

- `3cd5256` - 添加 GitHub Actions 自动同步 Gitee 代码
  - 新增 sync-from-gitee.yml 工作流
  - 1 个文件变更，50 行新增

#### 文档更新
- `aa1d2ad` - 更新全部 11 语言 README/架构/入门文档，体现 SiliconLife.Fast 多平台支持 (ref task-112, task-113)
  - 修正文档中 SiliconLife.Fast 仅 Windows 的描述，体现实际多平台支持（Windows / macOS / Linux）
  - 更新 11 种语言的 README.md、architecture.md、getting-started.md
  - SelectComponent 添加 hint 属性支持
  - ConfigView 枚举下拉框传入 hint
  - 11 种语言本地化新增 SelectSearchHint 键值
  - 53 个文件变更，690 行新增，194 行删除

#### 任务系统
- `3329f3d` - 添加任务系统巡检机制 + 本地化 Bug 修复任务
  - 创建 task-113：修复关于页面本地化问题
  - 更新 task-112：更新 Fast 版本文档支持 Linux
  - 归档已完成任务（11 个）到 .ai-collab/archive/
  - 巡检机制配置完成：快速巡检（每 30 分钟）+ 全量巡检（每天 06:00）
  - 2 个文件变更，148 行新增，171 行删除

#### 协作框架
- `6038e22` - 注册 coze-agent 到 .ai-collab 协作注册表
  - 新增扣子平台常驻 AI 注册信息
  - 1 个文件变更

### 2026-05-14

#### AI 协作框架
- `7344fbb` - 移除 handoff 模式，改为任务列表驱动 (v2.0)
  - 重构 .ai-collab 目录结构，从 handoff 交接模式改为任务列表驱动
  - 新增 tasks.json 任务列表核心文件
  - 新增 activity.log 操作日志
  - 新增 changes/ 和 sessions/ 目录

- `589a48e` - 添加 .ai-collab 会话记录
  - 新增 AI 协作会话状态记录

- `5481bcf` - 注册 Qoder AI IDE 到协作注册表
  - 新增 Qoder AI 编程助手注册信息

- `e2d7b61` - 补充 tasks.json relatedCommit 和 changes commitHash
  - 完善任务元数据关联

- `a087f0c` - 验收 task-101~110 全部任务
  - 确认 10 个任务修复全部完成

#### Bug 修复
- `fac9435` - 完成 task-101~110 全部 10 个任务修复与实现
  - 修复搜索选择组件缺少提示文字
  - 修复关于页面本地化问题
  - 修复帮助系统搜索 JS 错误
  - 39 个文件变更，684 行新增，121 行删除

- `c46dfbc` - 完成所有待办任务 (task-001~006)
  - 完成初始 6 个待办任务

- `ec176b2` - 覆盖任务列表 - 代码审查发现 10 个新 bug
  - 创建 task-101~110 共 10 个新任务

#### 重构
- `ab15915` - 统一版权头 + 修复 HelpController BOM 和 HelpView 搜索 JS
  - 统一所有 C# 源文件 Apache 2.0 版权头
  - 修复 HelpController BOM 编码问题
  - 修复 HelpView 搜索 JavaScript 错误

#### 新功能
- `18a6f5d` - 创建 MCP 浏览器能力服务器 (ref task-111)
  - 新增 SiliconLife.McpServer 项目
  - 实现 Playwright 浏览器自动化 MCP 服务器

- `9eb251a` - 移除 SiliconLife.McpServer 模块 (ref task-111)
  - 移除独立 MCP 服务器，功能已集成到主项目

### 2026-05-13

#### 本地化
- `7a62590` - 添加波兰语本地化支持
  - 新增 pl-PL 波兰语本地化实现（PlPL.cs，1089 行）
  - 新增波兰语帮助文档本地化（HelpLocalizationPlPL.cs，3972 行）
  - 新增波兰语中国历史日历支持（ChineseHistoricalPlPL.cs，600 行）
  - 新增波兰语托盘本地化（TrayPlPL.cs，135 行）
  - 新增波兰语完整文档集（15 个文档）
  - Language 枚举新增波兰语
  - 35 个文件变更，14379 行新增，11 行删除

- `51f9c8e` - 更新文档中的 Ark AI 引用和术语改进
  - 更新多语言文档中的 AI 客户端术语

- `7587c12` - 为所有语言添加变更日志条目
  - 同步更新所有语言版本的 changelog

#### 窗口系统迁移
- `b49a07d` - 迁移到 Avalonia 窗口常驻模式
  - 移除 Windows Forms 依赖，完全迁移到 Avalonia UI 框架
  - 状态窗口在 Linux 上正常显示（远程桌面验证）
  - 添加窗口控制：右键菜单、双击打开 Web、关闭按钮
  - 添加多 AI 协作框架 (.ai-collab/)
  - 修复托盘图标初始化（优雅降级）
  - 新增 App.axaml 和 App.cs Avalonia 应用入口
  - 13 个文件变更，1442 行新增，541 行删除

- `d335aaf` - Linux 平台窗口始终显示 + 关闭确认对话框
  - Linux 上自动显示状态窗口（无托盘图标）
  - Linux 上关闭窗口时弹出确认对话框
  - Windows/macOS 保持原有托盘行为
  - 支持 --no-tray 参数强制禁用托盘
  - 新增 ShowMessageBoxAsync 方法用于确认对话框
  - 3 个文件变更，206 行新增，29 行删除

#### 托盘系统重构
- `841d384` - 重构托盘系统并初始化 AI 协作框架
  - 精简 TrayLocalizationBase 移除未使用属性
  - 添加 ShowStatus 本地化项
  - App.cs 添加托盘图标点击显示状态窗口、本地化菜单项
  - Program.cs 将托盘图标初始化移至 StartAsync
  - TrayStatusWindow 关闭时隐藏而非退出
  - 注册 trae-glm5 和 catpaw 至 .ai-collab 协作框架
  - 更新 .gitignore 确保 .ai-collab 所有文件均被追踪
  - 22 个文件变更，178 行新增，1226 行删除

#### 文档
- `43653bc` - 更新仓库说明和 AI 注册表
  - 更新项目 README 和 .ai-collab 注册信息

### 2026-05-12

#### 任务系统 Web 视图
- `0891b3c` - 添加任务执行详情和历史视图
  - 新增 TaskExecutionDetailView 任务执行详情视图
  - 新增 TaskExecutionHistoryView 任务执行历史视图
  - TaskController 新增执行详情和历史查询接口
  - 新增 TaskViewModel 任务视图模型
  - TaskCenter 任务中心增强
  - TaskSystem 任务系统更新
  - 9 种语言本地化新增任务相关键值
  - 26 个文件变更，803 行新增，55 行删除

### 2026-05-11

#### Web 组件架构重构
- `5e687ad` - 将组件渲染从字符串迁移到 H-tree
  - ComponentBase 渲染方法从字符串模式迁移到 H-tree 结构
  - 所有 28 个组件适配新渲染架构（A、Accordion、Button、Calendar、Card、Chart 等）
  - SelectComponent 大幅重构（889 行改进）
  - 控制器和视图同步更新
  - 33 个文件变更，667 行新增，435 行删除

- `bfd332d` - 将 Style 从字符串迁移到 CssBuilder 内联样式
  - 新增 CssBuilder 样式构建器
  - ComponentBase 样式系统从字符串迁移到结构化 CssBuilder
  - LoadingComponent 大幅增强（103 行新增）
  - ConfigController、LogController、MemoryController 控制器样式迁移
  - ChatView、ConfigView、LogView、MemoryView 视图样式迁移
  - 37 个文件变更，351 行新增，157 行删除

#### 存储系统优化
- `d67a7ee` - 优化 QueryLatest 大型数据集查询
  - SpeedyTimeStorage QueryLatest 方法性能优化
  - SpeedyLoggerProvider 日志提供者增强
  - 2 个文件变更，44 行新增，5 行删除

#### 日历系统重构
- `9629f88` - 提取 TimerExecution 并增强定时器 Web 视图
  - TimerSystem 提取 TimerExecution 逻辑（175 行移除）
  - SelectComponent 大幅增强（427 行改进）
  - TimerController 和定时器视图增强
  - ContextManager 上下文管理器更新
  - 12 个文件变更，458 行新增，267 行删除

#### 本地化
- `5d8ca79` - 添加 LogsLoading 本地化键值
  - 9 种语言新增 LogsLoading 键值
  - DefaultLocalizationBase 基类新增定义
  - 11 个文件变更，15 行新增

### 2026-05-10

#### 任务系统重构
- `54394f6` - 合并任务系统与聊天历史周期
  - ProjectTaskSystem 项目任务系统大幅精简（411 行重构）
  - TaskSystem 任务系统精简（254 行重构）
  - TaskCenter 任务中心重构（188 行改进）
  - ContextManager 上下文管理器优化（347 行重构）
  - DefaultSiliconBeing 硅基生命体增强
  - TimerSystem 定时器系统整合任务
  - IWorkNoteStorage 接口更新
  - SpeedyWorkNoteStorage 和 FileSystemWorkNoteStorage 适配
  - 16 个文件变更，648 行新增，897 行删除

### 2026-05-09

#### Web 界面增强
- `bc50dd7` - 改进聊天视图并添加审计功能
  - 新增 AuditController 审计控制器（261 行）
  - 新增 AuditView 审计视图（379 行）
  - 新增 AuditViewModel 审计视图模型
  - ChatView 聊天视图大幅改进（171 行增强）
  - ChatController 聊天控制器更新
  - MarkdownEditorComponent 组件增强
  - InitController 初始化控制器改进
  - ChatSystem 聊天系统新增功能
  - 14 个文件变更，1030 行新增，112 行删除

- `c9babce` - 改进聊天视图中的工具调用渲染
  - ChatView 工具调用块渲染增强
  - 1 个文件变更，54 行新增，11 行删除

#### AI 工具场景系统
- `ff2eddd` - 实现工具场景过滤系统
  - 新增 ToolScenarioAttribute 工具场景属性（36 行）
  - 新增 ChatOnlyAttribute 仅聊天场景属性（19 行）
  - ToolManager 工具管理器新增场景过滤功能（40 行）
  - ContextManager 上下文管理器适配场景过滤
  - 4 个文件变更，115 行新增，30 行删除

- `5709a33` - 为工具类添加场景属性
  - 24 个工具类添加 ToolScenario 属性标注
  - 包括日历、聊天、配置、策展、数据库、磁盘、动态编译等工具
  - 24 个文件变更，46 行新增，20 行删除

#### 任务系统重构
- `2f19a5f` - 使用 TaskCenter 和 TaskEnumerator 重构任务系统
  - 新增 TaskCenter 任务中心（235 行）
  - 新增 TaskEnumerator 任务枚举器（297 行）
  - TaskSystem 任务系统重构精简
  - DefaultSiliconBeing 硅基生命体适配新架构
  - DefaultSiliconBeingFactory 工厂更新
  - SiliconBeingBase 基类增强
  - 7 个文件变更，796 行新增，275 行删除

#### 权限系统迁移
- `a06ed09` - 将 IM 和权限系统迁移到 App 项目
  - PermissionRequestQueue 从 Default/Fast 迁移到 App 项目（443 行新增）
  - 移除 Default 版本 WebUIProvider（403 行删除）
  - 移除 Default 版本 HelpTool（194 行删除）
  - 移除 Default/Fast 版本重复的 PermissionRequestQueue
  - 移除 Default 版本 IMPermissionAskHandler
  - PermissionRequestController 控制器更新
  - 14 个文件变更，496 行新增，1183 行删除

#### AI 上下文优化
- `4c8aaff` - 优化上下文管理器并增强服务定位器
  - ContextManager 上下文管理器精简优化
  - ServiceLocator 服务定位器增强（36 行新增）
  - ToolManager 工具管理器增强（34 行新增）
  - DashScopeClient 和 VolcengineArkClient 客户端改进
  - 执行器（CommandLine、Disk、Network）更新
  - 8 个文件变更，116 行新增，98 行删除

#### 本地化
- `5c5eef7` - 添加审计和任务本地化键值
  - DefaultLocalizationBase 新增 127 行本地化定义
  - 9 种语言新增审计和任务相关键值（每种 26 行）
  - 11 个文件变更，387 行新增

#### 项目配置
- `2067db6` - 更新项目配置和 gitignore 规则
  - .gitignore 规则更新
  - DefaultConfigData 和 Fast DefaultConfigData 配置增强
  - SpeedyWorkNoteStorage 存储改进
  - SpeedyPack 核心增强
  - 5 个文件变更，32 行新增，6 行删除

### 2026-05-07

#### 意大利语本地化
- `8adc18c` - 添加意大利语本地化支持并更新多语言文档
  - 新增 it-IT 意大利语本地化
  - 新增 ItIT 本地化实现（1909 行）
  - 新增 ChineseHistoricalItIT 中国历史日历意大利语支持（586 行）
  - 新增 TrayItIT 托盘意大利语本地化（135 行）
  - 新增意大利语完整文档集（14 个文档：README、API 参考、架构、日历系统、变更日志、贡献指南等）
  - 更新所有语言版本的架构、开发指南、入门指南等文档
  - Language 语言枚举新增意大利语
  - 86 个文件变更，11573 行新增，769 行删除

#### 文档同步
- `12a5deb` - 更新架构、变更日志和硅基生命体指南的多语言文档
  - 8 种语言的 README 更新
  - 8 种语言的架构文档更新
  - 8 种语言的变更日志更新
  - 8 种语言的硅基生命体指南更新
  - 8 种语言的工具参考更新
  - 词汇表重构
  - 46 个文件变更，1697 行新增，442 行删除

### 2026-05-06

#### 大规模模块重构
- `eeb3be6` - 大规模模块重构和重组
  - SiliconLife.App 项目结构调整
  - SiliconLife.Fast 项目重组
  - SiliconLife.Default 项目重组
  - SiliconLife.Common 共享模块重组
  - SiliconLife.Core 核心模块重组
  - SiliconLife.Speedy 存储引擎重组
  - SiliconLife.Speedy.Manager 管理工具重组
  - 119 个文件变更，6926 行新增，3066 行删除

### 2026-05-04

#### AI 客户端
- `24d2c86` - 添加 VolcengineArkClient 并替换 Audit 为 Usage tracking
  - 新增 VolcengineArkClient 火山引擎 Ark AI 客户端
  - 支持流式和非流式模式
  - 内置双层速率控制（自我速率控制 + 服务器速率限制）
  - 兼容 OpenAI API 协议
  - Audit 系统替换为 Usage tracking
  - 24 个文件变更，802 行新增，21 行删除

#### 工具系统
- `f27650a` - 添加热重载工具用于 Fast 自重启
  - 新增 HotReloadTool 热重载工具
  - 支持 SiliconLife.Fast 在线编译、更新和重启
  - 新增 HotReload.exe 独立更新器
  - 安全文件复制机制（不覆盖自身）
  - 优雅关闭和端口释放等待
  - 9 个文件变更，581 行新增

#### 本地化
- `6a5aad8` - 更新所有文件并添加法语本地化支持
  - 新增 fr-FR 法语本地化
  - 更新所有语言版本
  - 帮助文档法语翻译
  - 界面法语翻译
  - 100+ 个文件变更

### 2026-05-03

#### 项目基础设施
- `2664b0c` - 更新项目基础设施和依赖
  - SiliconLife.Speedy.Manager 新增 WPF 管理界面（MainForm.Designer.cs、MainForm.resx）
  - 新增 slc.ico 图标资源（1.5MB）
  - PluginLoader 大幅增强安全扫描（622 行新增）
  - 新增 PermissionedStreamFactory 权限流工厂（779 行）
  - 新增 PermissionRequestQueue 权限请求队列（Default 和 Fast 版本）
  - 新增 DebugLoggerProvider 调试日志提供者
  - ConfigDataBase 配置基类增强
  - ToolManager 新增插件工具扫描功能（ScanAllPluginAssemblies）
  - SiliconBeingManager 生命周期管理增强
  - DashScopeClient 阿里云 AI 客户端大幅增强（227 行新增）
  - DefaultSiliconBeingFactory 工厂增强
  - Web 视图和控制器更新（ChatView、WorkNoteView、PermissionRequestController）
  - 9 种语言本地化新增键值
  - 35 个文件变更，28080 行新增，336 行删除

### 2026-05-02

#### AI 客户端增强
- `c16f99f` - 更新 AI 客户端、Web UI 和存储组件
  - DashScopeClient 阿里云客户端大幅改进
  - SpeedyPackAutoCompactor 自动压缩器优化
  - Web 视图基类和 BeingView 改进
  - 6 个文件变更，240 行新增，81 行删除

#### 插件系统
- `242dc98` - 在关于页面添加插件列表
  - AboutController 新增插件信息展示
  - AboutViewModel 新增插件数据模型
  - AboutView 新增插件列表渲染
  - 9 种语言本地化新增插件相关键值
  - 14 个文件变更，160 行新增，1 行删除

#### AI 优化
- `147f8f4` - 简化上下文记忆提示文本
  - ContextManager 优化 AI 提示词
  - 1 个文件变更，1 行新增，1 行删除

#### Speedy 存储优化
- `8bda2d3` - 更新 Speedy 存储和记忆控制器实现
  - SpeedyPackAutoCompactor 间隔修正
  - SpeedyTimeStorage 路径处理优化
  - MemoryController 记忆控制器改进
  - SpeedyPack.Manager UI 更新
  - 4 个文件变更，21 行新增，18 行删除

#### 托盘增强
- `8972654` - 增强托盘状态窗口的本地化支持
  - 9 种语言托盘本地化新增 Speedy 管理入口
  - TrayStatusWindow 新增 Speedy 管理菜单项
  - 11 个文件变更，72 行新增

#### Speedy.Manager 优化
- `6f5db09` - 优化 SpeedyPack 管理器 UI 和内部组件
  - MainForm 界面重构
  - FreeList 内存管理优化
  - WriteQueue 写入队列改进
  - SpeedyPack 核心优化
  - 5 个文件变更，96 行新增，88 行删除

#### 存储系统增强
- `57f9d5d` - 改进存储系统，添加自动压缩和不完整日期支持
  - 新增 SpeedyPackAutoCompactor 自动压缩定时器（30 分钟间隔）
  - SpeedyPackRegistry 单例管理器增强
  - SpeedyStorage、SpeedyTimeStorage、SpeedyWorkNoteStorage 适配改进
  - SpeedyPack 新增 FreeList 空闲空间管理（149 行）
  - PackFileWriter 写入器重构优化
  - WriteOperation、WriteQueue 写入队列增强
  - SpeedyPackOptions 配置选项扩展
  - IncompleteDate 新增比较方法
  - PluginLoader 插件加载器改进
  - Default 和 Fast 版本 Program.cs 初始化流程更新
  - DefaultConfigData 配置数据简化
  - KnowledgeNetwork 知识网络精简
  - ChatController、MemoryController 控制器优化
  - SpeedyPack.Manager MainForm 功能增强
  - 22 个文件变更，639 行新增，253 行删除

#### Speedy.Manager 更新
- `b04ed33` - 更新 Speedy.Manager 文件

### 2026-05-01

#### 架构重构：Speedy 存储替换 LiteDB
- `6600972` - 用 Speedy 存储替换 LiteDB，添加插件系统和 Speedy 项目
  - **新增 SiliconLife.Speedy 项目**：高性能 .spk 存储引擎
    - SpeedyPack 核心类（489 行）：内存目录映射 + 条目缓存 + 异步写入队列
    - SpeedyPackOptions 配置类：缓存 TTL、最大缓存条目数、只读模式
    - IPackTransaction 事务接口：支持原子写入操作
    - SpkFileInfo 文件信息类
    - Internal 目录：DirectoryMap、EntryCache、PackFileReader、PackFileWriter、WriteQueue、WriteOperation、SpeedyTransaction、SpkHeader、PathNormalizer、FreeList
    - 依赖 MessagePack 3.1.4 进行二进制序列化（LZ4 压缩）
  - **新增 SiliconLife.Speedy.Manager 项目**：WPF 管理工具
    - MVVM 架构：MainViewModel、DirectoryTreeViewModel、ContentViewerViewModel 等
    - 服务层：PackService、FileDialogService、RecentFilesService、NotificationService
    - 转换器：BoolToVisibility、ByteSizeToString、ContentTypeToIcon、NullToCollapsed
    - 视图：MainWindow、DirectoryTreeView、ContentViewerPanel、MetadataPanel
    - 对话框：FileInfoDialog、ImportDialog、NewEntryDialog
  - **SiliconLife.Fast 存储迁移**：LiteDB → SpeedyPack
    - 新增 SpeedyStorage（IStorage 适配器）
    - 新增 SpeedyTimeStorage（ITimeStorage 适配器）
    - 新增 SpeedyWorkNoteStorage（IWorkNoteStorage 适配器）
    - 新增 SpeedyPackRegistry（进程级单例管理）
    - 新增 SpeedyPackAutoCompactor（自动压缩定时器）
    - 移除 LiteDB 相关存储实现（LiteDBStorage、LiteDBTimeStorage、LiteDBWorkNoteStorage、LiteDBLoggerProvider、LiteDBManager、LiteDBModels）
    - 移除 LiteDB 管理窗口相关代码
  - **插件系统**：
    - 新增 IPlugin 接口（Core/Plugins/IPlugin.cs）
    - 新增 PluginLoader 插件加载器（Core/Plugins/PluginLoader.cs）
    - 支持从目录加载插件 DLL
    - 安全扫描：禁止命名空间检查（System.IO、System.Net、Microsoft.CodeAnalysis 等）
    - 可信程序集白名单（Google.Protobuf、Newtonsoft.Json、MessagePack 等）
    - 自定义 AssemblyLoadContext 隔离加载
    - ToolManager 新增 ScanAllPluginAssemblies 方法
    - CoreHost 集成插件加载器
  - 119 个文件变更，6926 行新增，3066 行删除

#### 硅基生命体增强
- `3aef4c3` - 添加 Stopped 活动状态和错误处理改进
  - 硅基生命体新增 Stopped 状态
  - 错误处理和恢复机制增强

#### 本地化更新
- `513c65d` - 更新所有语言版本和文档
  - 新增 MarkdownEditorComponent 组件（625 行）
  - 新增 DetailsComponent 组件（130 行）
  - 新增 AccordionComponent 手风琴组件（285 行）
  - BeingController、ChatController、MemoryController、PermissionController 控制器更新
  - BeingView、ChatView、MemoryView、SoulEditorView 视图重构
  - 移除旧 MarkdownEditorView
  - InitController 组件化迁移
  - 115 个文件变更，5761 行新增，2362 行删除

### 2026-04-30

#### 系统托盘功能
- `101b203` - 实现托盘状态窗口和 ApplicationContext
  - 新增托盘图标资源（alpha.png、noWord.png、slc.ico、wordIcon.png）
  - 实现 TrayStatusWindow 状态窗口
  - 支持 9 种语言的托盘本地化（TrayCsCZ、TrayDeDE、TrayEnUS 等）
  - TrayLocalizationBase 抽象基类
  - 24 个文件变更，27995 行新增，1 行删除（含资源文件）

#### 组件化 UI 架构
- `e61cfaa` - 完成组件化 UI 架构，实现 24 个组件
  - MVP 阶段（8 个）：ComponentBase、Div、Span、Button、Input、Form、Select、Label
  - 第二阶段（6 个）：Accordion、Card、Tabs、Table、Modal、Message
  - 第三阶段（5 个）：Calendar、Tree、Chart、FileUpload、RichText
  - 新增 Js、Behavior、DomUpdate 等辅助类
  - 25 个文件变更，2666 行新增

- `7449e51` - 改进组件系统并添加新皮肤主题
  - 增强 A、Button、Div、Form、Input 等组件
  - 新增 3 种皮肤主题：HighContrast（高对比度）、Light（浅色）、Minimal（极简）
  - 更新现有皮肤（Admin、Chat、Creative、Dev）
  - InitController 组件化迁移
  - 32 个文件变更，1466 行新增，1238 行删除

- `1ba8636` - 启动 InitController 组件化迁移（进行中）
  - 9 个文件变更，574 行新增，145 行删除

#### 存储系统统一
- `895dff9` - 统一 soul.md 和 state.json 使用 IStorage 接口
  - DefaultSiliconBeing 使用 IStorage 读写灵魂文件和状态
  - 新增 StateFileManager 状态文件管理器
  - SoulFileManager 重构适配 IStorage
  - 8 个文件变更，201 行新增，116 行删除

#### LiteDB 管理增强
- `a34bef4` - 添加 LiteDBManager 并增强托盘本地化
  - 托盘菜单新增 LiteDB 管理入口
  - 9 种语言托盘本地化更新
  - 10 个文件变更，196 行新增

- `c4a79ca` - 添加 LiteDB 管理窗口的语言感知本地化工厂
  - 1 个文件变更，78 行新增

- `5ebc55e` - 将 LiteDBAdminLocalization 转换为抽象基类
  - 10 个文件变更，1356 行新增

#### 配置系统修复
- `2da5256` - 添加 ConfigExists 抽象方法并修复 LiteDB 重复配置记录
  - ConfigDataBase 新增 ConfigExists 方法
  - Fast 版本 DefaultConfigData 实现 LiteDB 配置存在性检查
  - 修复 LiteDB 重复配置键问题
  - 9 个文件变更，210 行新增，2 行删除

#### 聊天和视图优化
- `d3618ec` - 优化聊天会话、存储系统、时间模型和视图基类
  - BroadcastChannel、GroupChatSession、SingleChatSession 优化
  - ITimeStorage 新增查询方法
  - FileSystemStorage 和 LiteDBStorage 同步更新
  - ViewBase 重构优化（Default 和 Fast 版本）
  - 11 个文件变更，622 行新增，392 行删除

### 2026-04-29

#### 架构重构：共享模块提取
- `a102428` - 将共享模块从 SiliconLife.Default 迁移到 SiliconLife.Common
  - 提取 32 种日历实现到 Common 项目
  - 提取本地化基类及 21 种语言实现到 Common 项目
  - 提取权限管理器、默认硅基生命体实现到 Common 项目
  - 提取 23 个内置工具实现到 Common 项目
  - 提取 Playwright WebView 实现到 Common 项目
  - 更新命名空间为 SiliconLife.Collective
  - 122 个文件变更，586 行新增，343 行删除

#### 代码质量改进
- `17566fe` - 将 Core、Common 和 Default 项目中的 Console.WriteLine 替换为日志系统
  - ContextManager、AuditLogger、DefaultConfigData 等 6 个文件更新
  - 统一使用 ILogger 接口，提升代码可维护性
  - 6 个文件变更，12 行新增，8 行删除

#### SiliconLife.Fast 高性能版本
- `54a0307` - 添加 SiliconLife.Fast 项目并完成编译修复
  - 完整的 Windows 窗体应用程序入口点
  - 系统托盘支持（NotifyIcon）
  - 移植全部 Web UI 控制器（20+ 个）
  - 移植全部 Web 视图组件
  - 移植 4 种皮肤主题（Admin、Chat、Creative、Dev）
  - 125 个文件变更，61186 行新增

#### 多语言文档同步
- `265fde8` - 将双版本架构文档同步到所有语言
  - 更新 7 种语言的 architecture.md、changelog.md
  - 更新 6 种语言的 contributing.md
  - 更新 7 种语言的 getting-started.md、roadmap.md
  - 47 个文件变更，1214 行新增，38 行删除

#### LiteDB 存储系统（Fast 版本）
- `4704862` - 添加 LiteDB 依赖和基础设施
  - 新增 LiteDBManager 管理类
  - 新增 LiteDBModels 数据模型
  - 3 个文件变更，252 行新增

- `4220036` - 实现 LiteDB 存储类
  - LiteDBStorage：实现 IStorage 接口
  - LiteDBTimeStorage：实现 ITimeStorage 接口
  - LiteDBWorkNoteStorage：实现 IWorkNoteStorage 接口
  - 3 个文件变更，581 行新增

- `38ebd23` - 将配置和日志系统迁移到 LiteDB
  - DefaultConfigData 适配 LiteDB 存储
  - 新增 LiteDBLoggerProvider 日志提供者
  - 2 个文件变更，203 行新增，67 行删除

- `e687157` - 将知识网络从文件系统迁移到 LiteDB
  - KnowledgeNetwork 全面重构，使用 LiteDB 存储三元组数据
  - 1 个文件变更，231 行新增，72 行删除

- `4220169` - 将 LiteDB 存储集成到 Program 和 ProjectManager
  - Program.cs 初始化 LiteDB 存储
  - ProjectManager 适配 LiteDB 工作笔记存储
  - 2 个文件变更，40 行新增，17 行删除

- `5f3a709` - 移除废弃的文件系统存储实现
  - 删除 FileSystemLoggerProvider、FileSystemStorage、FileSystemTimeStorage 等
  - 6 个文件变更，1518 行删除

- `e1a4ef2` - docs: add v0.1.0-alpha version identifier to all documentation
  - 127 个文件变更，2297 行新增，2471 行删除

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### 存储系统重构
- `8dd26e3` - 统一 ITimeStorage 接口使用 IncompleteDate 并添加分级查询 API
  - 移除 ITimeStorage 接口中的 DateTime 重载方法，统一使用 IncompleteDate
  - IncompleteDate 新增 CompareTo(DateTime) 比较方法和 Expand() 展开方法
  - 新增 GetEarliestTimestamp()、GetLatestTimestamp() 分级查询 API
  - 新增 HasSummary() 和 QueryWithLevel() 方法，支持按时间层级查询
  - Memory.cs 重构压缩算法，使用新的分级查询 API 提升效率
  - FileSystemTimeStorage.cs 完整实现新的接口方法
  - 同步更新所有调用方：ChatSystem、ChatSession、BroadcastChannel、AuditLogger、TokenUsageRecord 等
  - 工具系统更新：HelpTool、LogTool、TokenAuditTool 适配新接口
  - Web 控制器更新：AuditController、ChatController、ChatHistoryController 适配新接口
  - 41 个文件变更，1820 行新增，903 行删除

### 2026-04-27

#### 帮助文档系统增强
- `9989d79` - 更新本地化、帮助系统和 Web 视图
  - 新增 IAIClientFactoryHelp.cs AI 客户端工厂帮助文档接口
  - 完成全部帮助文档的 9 种语言翻译
  - HelpTopics.cs 新增 40 个帮助主题定义
  - Web 视图全面更新：InitController、AuditView、ConfigView、KnowledgeView、LogView 等
  - 本地化系统增强：所有语言版本添加新的本地化键
  - AI 客户端工厂更新：DashScopeClientFactory、OllamaClientFactory 改进
  - 30 个文件变更，10086 行新增，15 行删除

#### 帮助文档新增内容
- `e7afe94` - 新增灵魂文件和审计日志帮助文档
  - 新增灵魂文件管理帮助文档
  - 新增审计日志帮助文档
  - HelpTopics.cs 新增主题定义
  - HelpView.cs 大幅重构，改进文档渲染逻辑
  - PermissionView.cs 重构，改进权限管理界面
  - 核心模块增强：SiliconBeingManager、TaskSystem、ToolManager 改进
  - TaskTool.cs 重构，改进任务管理功能
  - Web 视图全面更新：所有视图组件同步更新
  - HelpController.cs 简化，优化控制器逻辑
  - 30 个文件变更，7100 行新增，897 行删除

### 2026-04-26

#### 帮助文档系统
- `07895d7` - 增强帮助文档系统，新增 3 个文档并完成 9 种语言翻译
  - 新增记忆系统、Ollama 安装配置、阿里云百炼平台使用指南
  - 完成全部 10 个帮助文档的 9 种语言翻译
  - 简化 HelpView 渲染逻辑
  - 18 个文件变更，14418 行新增，1364 行删除

#### 德语本地化
- `0cfd8a1` - 添加完整的德语 (de-DE) 本地化支持
  - 完整的德语本地化文件
  - 新增中国历史日历德语支持
  - 新增帮助文档德语翻译
  - 完整同步 9 种语言的所有文档
  - 135 个文件变更，26186 行新增，14371 行删除

#### 文档同步
- `3aada7d` - 同步繁体中文 (zh-HK) 文档与简体中文保持一致
  - 3 个文件变更，519 行新增，422 行删除
- `2f6abff` - 为所有语言添加帮助工具显示名称本地化
  - 7 个文件变更，47 行新增，7 行删除

#### 知识系统重构
- `60944fe` - 统一命名空间到 SiliconLife.Collective
  - 8 个文件变更，5 行新增，8 行删除
- `69c51c5` - 添加帮助文档系统并将代码注释翻译为英文
  - 29 个文件变更，3385 行新增，22 行删除

### 2026-04-25

#### WebView 浏览器自动化
- `41757c3` - 实现基于 Playwright 的跨平台 WebView 浏览器自动化
  - 6 个文件变更，1152 行新增

#### 文档更新
- `0ff797b` - 添加 KnowledgeTool 和 WorkNoteTool 文档（7 种语言）
  - 28 个文件变更，4983 行新增
- `ad77415` - 更新所有 changelog 文件，添加 2026-04-25 Git 历史记录
  - 7 个文件变更，168 行新增

#### 项目工作区管理
- `785c551` - 实现项目工作区管理，包含工作笔记和任务系统
  - 新增项目工作区管理系统
  - 工作笔记功能，用于跟踪项目进度
  - 任务管理系统集成
  - 29 个文件变更，4256 行新增，36 行删除

#### 捷克语本地化
- `b4bbf39` - 添加完整的捷克语 (cs-CZ) 本地化并更新所有语言文档
  - 116 个文件变更，4933 行新增，222 行删除
- `faf078f` - 修复捷克语本地化编译错误
  - 3 个文件变更，910 行新增，1 行删除

#### 知识系统增强
- `20adaac` - 添加 KnowledgeTool 并支持完整本地化
  - 34 个文件变更，2331 行新增，56 行删除

### 2026-04-24

#### 记忆管理系统增强
- `c7b2ecc` - 增强记忆管理功能，添加高级过滤、统计和详情视图功能
  - 新增记忆高级过滤功能
  - 实现记忆统计功能
  - 添加记忆详情视图页面
  - 多语言本地化支持（6 种语言）
  - 13 个文件变更，840 行新增，86 行删除

#### 权限系统扩展
- `4489ad6` - 将 wttr.in 天气服务添加到网络白名单
  - 完整的多语言文档同步更新（6 种语言）
  - 14 个文件变更，417 行新增，1 行删除

#### Web 界面修复
- `d9d72e9` - 修复工作笔记详情模态框 CSS 优先级问题
  - 19 个文件变更，1744 行新增，6 行删除

#### 聊天历史优化
- `0df599c` - 修复工具结果被渲染为独立聊天消息的问题
  - 1 个文件变更，222 行新增，21 行删除
- `057b09d` - 优化聊天历史详情显示，改进工具调用渲染
  - 3 个文件变更，389 行新增，68 行删除

#### 定时器执行历史
- `fa3f06f` - 添加定时器执行历史功能，包含详情视图
  - 8 个文件变更，937 行新增，10 行删除
- `d824835` - 添加定时器执行历史本地化键（所有语言）
  - 7 个文件变更，88 行新增

#### 本地化增强
- `c13cb17` - 注册西班牙语语言变体
  - 1 个文件变更，4 行新增
- `9c44f34` - 添加中国历史日历多语言本地化支持
  - 16 个文件变更，6049 行新增，1 行删除

#### 核心功能改进
- `1e7c7b2` - 改进记忆压缩和工具执行追踪
  - 4 个文件变更，338 行新增，86 行删除

### 2026-04-23

#### 工具本地化
- `192fc6e` - 为 5 个工具添加缺失的工具名称本地化
  - 6 个文件变更，30 行新增

#### 文档更新
- `882c08f` - 更新所有 changelog 文件，添加完整 Git 历史记录并移除虚假版本号
  - 45 个文件变更，8815 行新增，1611 行删除

#### 聊天页面增强
- `65c157b` - 为聊天页面添加加载指示器并自动选择主理人会话
  - 10 个文件变更，211 行新增，7 行删除

#### 聊天历史功能
- `e483348` - 实现硅基生命体聊天历史查看功能
  - 新增 ChatHistoryController
  - 创建 ChatHistoryViewModel
  - 实现 ChatHistoryListView 和 ChatHistoryDetailView 页面
  - 添加聊天历史的本地化键（5 种语言）
  - 12 个文件变更，1178 行新增

#### AI 流控制增强
- `30a2d4e` - 增强 AI 流取消、IM 集成和核心主机初始化
  - 11 个文件变更，387 行新增，12 行删除

#### 聊天消息队列
- `db48c51` - 添加聊天消息队列、文件元数据和流取消支持
  - 4 个文件变更，357 行新增

#### 文件上传支持
- `28fb344` - 实现文件源对话框和文件上传支持
  - 3 个文件变更，1100 行新增，2 行删除
- `1d3e2cc` - 添加文件源对话框本地化字符串（6 种语言）
  - 6 个文件变更，30 行新增

#### 文档更新
- `8111e92` - 在 README 的仓库部分添加 Wiki 链接
  - 1 个文件变更，3 行新增，1 行删除

### 2026-04-22

#### 文档本地化
- `66c11eb` - 将中文注释翻译为英文并更新所有 changelog
  - 11 个文件变更，373 行新增，163 行删除

#### SSE 消息增强
- `b574b2b` - 为历史消息添加 senderName 用于 AI 识别
  - 1 个文件变更，9 行新增

#### 聊天功能
- `601fc14` - 添加 mark_read 操作，用于会话结束标记
  - 7 个文件变更，196 行新增，36 行删除

#### 工具系统优化
- `7a03a19` - 改进 LogTool 对话查询灵活性
  - 1 个文件变更，57 行新增，24 行删除

#### 本地化增强
- `0a8d750` - 添加主动硅基生命体行为的通用系统提示
  - 8 个文件变更，460 行新增，48 行删除

#### 日志系统重构
- `2b771f3` - 解耦 LogController 与文件 I/O，添加日志读取 API
  - 4 个文件变更，172 行新增，137 行删除
- `12da302` - 为日志视图添加硅基生命体筛选器
  - 9 个文件变更，147 行新增，10 行删除
- `8f6cb1e` - 为 ILogger 接口添加 beingId 参数，实现系统/硅基生命体日志分离
  - 47 个文件变更，524 行新增，490 行删除

#### 权限系统改进
- `4c747ad` - 重构 PermissionTool、ExecuteCodeTool，添加 EvaluatePermission API
  - 18 个文件变更，680 行新增，492 行删除

#### Bug 修复
- `1c96e99` - 修复 search_files 和 search_content 根目录搜索失败
  - 1 个文件变更，98 行新增，41 行删除

#### 工具整合
- `135710d` - 移除 SearchTool，将本地搜索移至 DiskTool
  - 2 个文件变更，185 行新增，365 行删除

#### 工具系统扩展
- `70ce7fb` - 实现 DatabaseTool 用于结构化数据库查询
  - 1 个文件变更，382 行新增
- `be29a09` - 实现 LogTool 用于操作和对话历史查询
  - 1 个文件变更，298 行新增
- `4ea7702` - 实现 PermissionTool 用于动态权限管理
  - 1 个文件变更，457 行新增
- `1384ff4` - 实现 ExecuteCodeTool 用于多语言代码执行
  - 1 个文件变更，477 行新增
- `82d1e11` - 实现 SearchTool 用于信息检索
  - 1 个文件变更，363 行新增

#### Web 界面优化
- `0675c45` - 优化预览窗格中的 markdown 代码块高亮
  - 1 个文件变更，4 行新增，23 行删除
- `702b3f3` - 增强任务视图，添加状态徽章和元数据展示
  - 8 个文件变更，221 行新增，9 行删除
- `6ed9a79` - 改进聊天消息存储和视图渲染
  - 8 个文件变更，140 行新增，29 行删除

### 2026-04-21

#### Bug 修复
- `c6b518b` - 修复定时器消息传递和聊天消息存储
  - 3 个文件变更，297 行新增，124 行删除

#### 配置管理
- `4305769` - 添加 .gitattributes 用于行尾管理
  - 1 个文件变更，32 行新增

#### Web 界面改进
- `188c6f8` - 注册任务列表 API 路由并添加空状态显示
  - 2 个文件变更，35 行新增，2 行删除
- `634e8ca` - 添加权限页面返回列表链接
  - 1 个文件变更，16 行新增
- `6ba591d` - 添加独立 AI 配置编辑器用于硅基生命体
  - 11 个文件变更，842 行新增，18 行删除
- `0a826f5` - 在代码编辑器中添加保存成功提示
  - 1 个文件变更，9 行新增，2 行删除
- `2940373` - 增强 Web 界面，添加代码悬浮提示和 UI 改进
  - 11 个文件变更，1054 行新增，75 行删除

#### 权限系统修复
- `592c7ab` - 修复回调实例化和注册顺序
  - 2 个文件变更，38 行新增，7 行删除

#### 安全增强
- `833ead2` - 为动态编译添加程序集引用验证
  - 4 个文件变更，135 行新增，8 行删除

#### 权限系统增强
- `5879621` - 添加权限回调预编译验证和增强错误处理
  - 21 个文件变更，617 行新增，26 行删除

#### 文档更新
- `4dbf659` - 更新 changelog 到 v0.5.1，替换 GitHub 占位符 URL，添加 Gitee 镜像，按语言本地化 Bilibili 名称，更新邮箱
  - 32 个文件变更，489 行新增，180 行删除

#### 配置与入口
- `0fc1693` - 更新程序入口和项目配置
  - 2 个文件变更，7 行新增

#### 权限系统重构
- `ea9179a` - 改进权限系统实现
  - 5 个文件变更，358 行新增，152 行删除

#### Bug 修复
- `928a96d` - 修复日历计算实现
  - 4 个文件变更，12 行新增，12 行删除

#### AI 与日历
- `646813e` - 改进 AI 客户端工厂实现
  - 2 个文件变更，21 行新增，20 行删除

#### 本地化
- `7940d9c` - 添加韩语本地化支持
  - 7 个文件变更，2424 行新增，10 行删除
- `4ff98ad` - 重构文档，支持多语言
  - 81 个文件变更，23818 行新增，1886 行删除

### 2026-04-20

#### 核心功能完善
- `28905b5` - 完整的多语言支持、AI 客户端工厂、权限系统和本地化设置
  - 带管理器、条目和不同日志级别的日志系统
  - 用于查询和跟踪 token 使用的 token 审计系统
  - 自动发现不同 AI 平台的 AI 客户端工厂
  - 带自己存储的权限回调系统
  - 控制台日志器实现
  - 英语和简体中文的多语言支持
  - 带 WebSocket 的 WebUI 信使，用于实时聊天
  - 使用本地化增强默认硅基生命体
  - 39 个文件变更，4670 行新增，175 行删除

### 2026-04-19

#### 定时器与日历
- `c933fd8` - 更新本地化、定时器系统、Web 视图并添加工具
  - 更好的本地化管理器
  - 定时任务的调度系统
  - AI 配置和上下文管理
  - 支持 32 种日历类型的日历工具
  - 用于日历 API 的 Web 控制器
  - 任务管理工具
  - 46 个文件变更，4018 行新增，975 行删除

**架构改进**
- 重新设计 Web 视图架构以更好地支持皮肤
- 改进生命体管理系统，具有更好的状态处理

### 2026-04-18

- `9f585e1` - 更新本地化、定时器系统、Web 视图并添加工具
  - 定时器和调度改进
  - 带改进 UI 组件的更好 Web 视图
  - 更多工具实现
  - 57 个文件变更，3328 行新增，389 行删除

### 2026-04-17

- `9b71fcd` - 更新核心模块，添加 zh-HK 文档、广播频道、配置工具和审计 Web 视图
  - 广播频道，用于多个硅基生命体一起聊天
  - 配置工具系统
  - 审计 Web 视图
  - 繁体中文文档
  - 42 个文件变更，3533 行新增，268 行删除

### 2026-04-16

- `5040f05` - 更新核心和默认模块
  - 模块优化和 bug 修复
  - 实现更新和改进
  - 58 个文件变更，9916 行新增，111 行删除

### 2026-04-15

- `3efab5f` - 更新多个模块：AI、Chat、IM、Tools、Web、Localization、Storage
  - AI 客户端改进
  - 聊天系统增强
  - 信使提供者更新
  - 工具系统优化
  - Web 基础设施改进
  - 本地化优化
  - 存储系统更新
  - 33 个文件变更，788 行新增，232 行删除

### 2026-04-14

- `4241a2f` - 聊天功能基本完成，UI 上传优化
  - 聊天系统功能完成
  - 文件上传的 UI 优化
  - 16 个文件变更，1234 行新增，102 行删除

### 2026-04-13

- `c498c31` - 代码更新
  - 通用代码改进和优化
  - 32 个文件变更，1045 行新增，546 行删除

### 2026-04-12

#### 文档与本地化
- `2161002` - 重构文档并增强本地化
  - 17 个文件变更，982 行新增，92 行删除
- `03d94e4` - 增强配置系统和本地化
  - 25 个文件变更，1378 行新增，154 行删除
- `9976a35` - 添加关于页面和本地化
  - 14 个文件变更，699 行新增，44 行删除

#### 聊天与 Web 视图
- `0c8ccfc` - 增强聊天系统、本地化和 Web 视图
  - 13 个文件变更，402 行新增，56 行删除
- `a8f1342` - 重新设计 Web 通信层，从 WebSocket 切换到 SSE
  - 27 个文件变更，793 行新增，935 行删除

### 2026-04-11

#### 日志系统
- `e8fe259` - 添加日志系统和代码优化
  - 37 个文件变更，624 行新增，91 行删除
- `f01c519` - 添加日志系统，更新 AI 接口和 Web 视图
  - 31 个文件变更，1758 行新增，63 行删除

### 2026-04-10

- `4962924` - 增强 WebSocket 处理程序、聊天视图和信使交互
  - 上下文管理器改进
  - 聊天系统增强
  - 信使提供者接口更新
  - WebUI 提供者重新设计
  - JavaScript 构建器和路由器更新
  - 聊天视图优化
  - WebSocket 处理程序改进
  - 9 个文件变更，365 行新增，134 行删除

### 2026-04-09

- `f9302bf` - 增强信使提供者接口、聊天系统和 Web UI 交互
  - 信使提供者接口扩展
  - 聊天消息和系统改进
  - 上下文管理器优化
  - 默认硅基生命体增强
  - Web UI 聊天视图改进
  - WebSocket 处理程序更新
  - 10 个文件变更，427 行新增，93 行删除

### 2026-04-07

- `6831ee8` - 重新设计 Web 视图和 JavaScript 构建器
  - 完整 Web 控制器重新设计
  - JavaScript 构建器完全重写
  - 所有视图组件更新
  - 皮肤系统改进
  - 视图基类架构提升
  - 23 个文件变更，2004 行新增，1983 行删除

### 2026-04-05

- `41e97fb` - 更新多个核心模块和 Web 控制器
  - 上下文管理器改进
  - 聊天系统和会话管理
  - 服务定位器重新设计
  - 硅基生命体基类和管理器更新
  - Web 控制器全面更新（17 个控制器）
  - 默认硅基生命体工厂改进
  - 31 个文件变更，681 行新增，326 行删除
- `67988d4` - 改进 Web UI 模块，添加执行器视图，清理视图和核心模块
  - 61 个文件变更，3148 行新增，3726 行删除

### 2026-04-04

- `b58bb1c` - 添加初始化控制器并重新设计 Web 模块
  - 初始化控制器
  - 配置模块重新设计
  - 本地化模块更新
  - 皮肤系统改进
  - 路由器增强
  - 29 个文件变更，1269 行新增，289 行删除
- `f03ac0b` - 添加 Web UI 模块，改进信使功能
  - 60 个文件变更，8481 行新增，165 行删除

### 2026-04-03

- `192e57b` - 更新项目结构和核心运行时组件
  - 22 个文件变更，446 行新增，179 行删除
- `59faec8` - 核心和默认实现更新
  - 25 个文件变更，3056 行新增，18 行删除
- `d488485` - 添加动态编译功能和主理人工具模块
  - 19 个文件变更，1727 行新增，11 行删除
- `753d1d9` - 添加安全模块，更新执行器、信使提供者、本地化和工具
  - 29 个文件变更，2352 行新增，93 行删除
- `a378697` - 完成阶段 5 - 工具系统 + 执行器
  - 41 个文件变更，2651 行新增，363 行删除

### 2026-04-02

- `e6ad94b` - 修复测试期间删除配置文件时聊天历史加载失败的问题
  - 4 个文件变更，49 行新增，45 行删除
- `daa56f5` - 完成阶段 4：持久化记忆（聊天系统 + 信使频道）
  - 29 个文件变更，2051 行新增，538 行删除

### 2026-04-01

- `bbe2dbb` - 修复配置加载和聊天服务消息路由
  - 27 个文件变更，1633 行新增，147 行删除
- `2fa6305` - 实现阶段 2：主循环框架和时钟对象系统
  - 9 个文件变更，594 行新增，41 行删除
- `32b99a1` - 实现阶段 1 - 基本聊天功能
  - 19 个文件变更，1185 行新增
- `358e368` - 初始提交：项目文档和许可证
  - 10 个文件变更，1873 行新增
