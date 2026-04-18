# 开发路线

[English](../../roadmap.md) | [繁體中文](../zh-HK/roadmap.md)

## 指导原则

每个阶段结束都要能**从命令行跑起来、看到行为**。绝不做"写完一堆基础设施但什么都看不到"的阶段。

---

## ~~第一阶段：能对话~~ ✅ 已完成

**目标**：命令行输入 → 调 AI → 命令行输出。最小可验证单元。

| # | 模块 | 说明 |
|---|------|------|
| 1.1 | 解决方案与项目结构 | 创建 `SiliconLifeCollective.sln`，建立 `src/SiliconLife.Core/`（核心库）和 `src/SiliconLife.Default/`（默认实现+入口） |
| 1.2 | Config（最小） | 单例模式 + JSON 反序列化，读 `config.json`。不存在时自动生成默认值 |
| 1.3 | Localization（最小） | `LocalizationBase` 抽象基类，`ZhCN` 实现。Config 中增加 `Language` 配置项 |
| 1.4 | OllamaClient（最小） | `IAIClient` 接口，HTTP 调用本地 Ollama `/api/chat`。暂不支持流式输出和 ToolCall |
| 1.5 | 控制台 I/O | `while(true) + Console.ReadLine()`，读用户输入 → 调 AI → 打印回复 |
| 1.6 | 版权页头 | 所有 C# 源文件顶部添加 Apache 2.0 版权页头 |

**阶段产出**：一个控制台聊天程序，能和本地 Ollama 模型对话。

**验证方式**：运行程序，输入"你好"，看到 AI 回复。

---

## ~~第二阶段：有骨架~~ ✅ 已完成

**目标**：把"裸循环"替换成框架结构，行为不变。

| # | 模块 | 说明 |
|---|------|------|
| 2.1 | Storage（最小） | `IStorage` 接口（Read/Write/Exists/Delete，key-value 模式）。`FileSystemStorage` 实现。实例类（非静态）。直接操作文件系统——**AI 无法控制 IStorage** |
| 2.2 | MainLoop + TickObject | 无限循环，精确计算 Tick 间隔（`Stopwatch` + `Thread.Sleep`）。优先级调度 |
| 2.3 | IAIClient 接口标准化 | `IAIClientFactory` 接口。OllamaClient 改为实现标准接口 |
| 2.4 | 控制台对话迁移 | 将 `while(true)` 迁移为 MainLoop 驱动的 TickObject。行为与第一阶段完全一致 |

**阶段产出**：MainLoop 在跑 Tick，控制台对话仍正常工作。

**验证方式**：注册一个测试用 TickObject，每秒打印一次 tick 计数；控制台对话仍正常工作。

---

## ~~第三阶段：有灵魂~~ ✅ 已完成

**目标**：第一个硅基人在框架内活了。

| # | 模块 | 说明 |
|---|------|------|
| 3.1 | SiliconBeingBase | 抽象基类，包含 Id、Name、ToolManager、AIClient、ChatService、Storage、PermissionService。抽象方法 `Tick()` 和 `ExecuteOneRound()` |
| 3.2 | 灵魂文件加载 | `SoulFileManager`：从硅基人目录读取 `soul.md` 文件 |
| 3.3 | ContextManager（最小） | 拼接灵魂文件 + 最近消息 → 调 AI → 拿回复。暂不处理 ToolCall 和持久化 |
| 3.4 | ISiliconBeingFactory | 工厂接口，负责创建硅基人实例 |
| 3.5 | SiliconBeingManager（最小） | 继承 TickObject（Priority=0），遍历所有硅基人，依次调用其 Tick |
| 3.6 | DefaultSiliconBeing | 标准行为实现。检测未读消息 → 创建 ContextManager → ExecuteOneRound → 输出回复 |
| 3.7 | 硅基人目录结构 | `DataDirectory/SiliconManager/{GUID}/`，包含 `soul.md` 和 `state.json` |

**阶段产出**：硅基人由 MainLoop 驱动，从控制台接收输入，加载灵魂文件，调 AI 回复。

**验证方式**：控制台输入 → MainLoop Tick → 硅基人处理（含灵魂文件引导的行为）→ AI 回复。回复风格应与第一阶段不同。

---

## ~~第四阶段：能记忆~~ ✅ 已完成

**目标**：对话有持久化，重启不丢失。

| # | 模块 | 说明 |
|---|------|------|
| 4.1 | ChatSystem | 频道概念：两 GUID 配对为一个频道。消息模型持久化。暂不实现群聊 |
| 4.2 | IIMProvider + IMManager | `IIMProvider` 接口。`ConsoleProvider` 作为正式 IM 通道。`IMManager` 路由消息 |
| 4.3 | ContextManager 增强 | 从 ChatSystem 拉取历史消息；AI 回复持久化；支持跨轮次 ToolCall 延续 |
| 4.4 | IMessage 模型 | 统一消息模型，ChatSystem 和 IMManager 共用 |

**阶段产出**：有记忆的对话系统。

**验证方式**：聊几句话 → 退出程序 → 重新启动 → 问"我们刚才聊了什么"，硅基人能回答。

---

## ~~第五阶段：能动手（工具系统）~~ ✅ 已完成

**目标**：硅基人能执行操作，不只是说话。

| # | 模块 | 说明 |
|---|------|------|
| 5.1 | ITool + ToolResult | `ITool` 接口：Name、Description、Execute。`ToolResult`：Success、Message、Data |
| 5.2 | ToolManager | 每个硅基人独立持有。反射式扫描程序集发现工具。`[SiliconManagerOnly]` 特性支持 |
| 5.3 | IAIClient：ToolCall 支持 | 解析 AI 返回的 tool_calls；循环执行工具并回传结果；直到 AI 返回纯文本 |
| 5.4 | Executor 基类 | 抽象基类，独立调度线程，请求队列，超时控制 |
| 5.5 | NetworkExecutor | HTTP 请求走执行器。超时控制、请求队列 |
| 5.6 | CommandLineExecutor | 命令行执行走执行器。跨平台分隔符检测 |
| 5.7 | DiskExecutor | 文件操作走执行器。暂不接入权限验证 |
| 5.8–5.12 | 内置工具 | CalendarTool、SystemTool、NetworkTool、ChatTool、DiskTool |

**阶段产出**：硅基人能调用工具执行操作。

**验证方式**：问"今天星期几"→ CalendarTool 回答；问"帮我查一下进程"→ SystemTool 执行；让硅基人发消息给另一个硅基人 → ChatTool 验证。

---

## ~~第六阶段：能守规矩（权限系统）~~ ✅ 已完成

**目标**：硅基人不能随便动东西。

| # | 模块 | 说明 |
|---|------|------|
| 6.1 | PermissionManager | 每个硅基人私有实例。回调函数模式，三元结果（Allowed/Deny/AskUser）。查询优先级：高频拒绝 → 高频允许 → 回调函数。IsCurator 标记 |
| 6.2 | PermissionType 枚举 | NetworkAccess、CommandLine、FileAccess、Function、DataAccess |
| 6.3 | DefaultPermissionCallback | 网络域名白名单/黑名单、命令行分级、文件路径安全规则 |
| 6.4 | GlobalACL | 前缀匹配规则表，持久化到 Storage |
| 6.5 | UserFrequencyCache | 高频允许/拒绝列表。用户主动选择加入（非自动检测）。前缀匹配，仅内存态，可配置有效期 |
| 6.6 | UserAskMechanism（控制台版） | 权限回调返回 AskUser 时，控制台打印描述 → 等待用户输入 y/n |
| 6.7 | 执行器接入权限 | 所有执行器在执行前增加权限检查 |
| 6.8 | IStorage 隔离说明 | IStorage 是系统内部持久化——直接操作文件系统，**不经过执行器**，**AI 不可控**。执行器仅管辖 AI 通过工具发起的 IO |
| 6.9 | 权限审计日志 | 记录所有权限决策（时间、请求者、资源、结果） |

**阶段产出**：硅基人想操作敏感资源时，控制台弹出确认提示。

**验证方式**：让硅基人尝试删除文件 → 控制台打印权限询问 → 输入 n → 操作被拒绝。让硅基人访问白名单网站 → 直接放行。

---

## ~~第七阶段：能进化（动态编译）~~ ✅ 已完成

**目标**：硅基人能重写自己的代码。

| # | 模块 | 说明 |
|---|------|------|
| 7.1 | CodeEncryption | AES-256 加密/解密。PBKDF2 从 GUID 派生密钥 |
| 7.2 | DynamicCompilationExecutor | 基于 Roslyn 的内存编译沙箱。编译时引用控制（主要防线：排除 System.IO、Reflection 等危险程序集） |
| 7.3 | 安全扫描 | 运行时静态分析（辅助防线）：检测直接 IO、系统调用、反射滥用等危险模式 |
| 7.4 | 硅基人生命周期增强 | 加载：解密 → 扫描 → 编译 → 实例化。运行时：内存编译 → 原子替换 → 持久化加密代码 |
| 7.5 | SiliconCurator | 主理人抽象基类。IsCurator=true。最高权限 |
| 7.6 | DefaultCurator | 默认主理人实现。内置灵魂文件和管理工具集 |
| 7.7 | CuratorTool | `[SiliconManagerOnly]` 工具：list_beings、create_being、get_code、reset |
| 7.8 | 权限回调动态覆盖 | 硅基人可编译自定义权限回调代码 |
| 7.9 | SiliconBeingManager 增强 | Replace 方法（运行时替换实例）。MigrateState（状态迁移） |

**阶段产出**：硅基人能通过 AI 生成新代码，编译后替换自身。

**验证方式**：让硅基人"给自己加一个新功能" → 观察编译过程 → 重启后新功能生效。

---

## ~~第八阶段：有记忆有计划~~ ✅ 已完成

**目标**：长期记忆、任务管理、定时触发。

| # | 模块 | 说明 |
|---|------|------|
| 8.1 | FileSystemMemory | 短期/长期分阶段存储。时间衰减。压缩整合。多维检索 |
| 8.2 | TaskSystem | 一次性任务 + DAG 依赖任务。优先级调度。状态跟踪 |
| 8.3 | TimerSystem | 一次性闹钟 + 周期定时。毫秒级精度。持久化到 Storage |
| 8.4 | IncompleteDate | 模糊时间范围结构体（如"2026年4月"、"2026年春天"） |
| 8.5–8.7 | 记忆/任务/定时器工具 | 硅基人可查询记忆、管理任务、设置定时器 |

**阶段产出**：硅基人能记住历史要点、创建和跟踪任务、设置闹钟。

**验证方式**：让硅基人创建任务 → 查看任务列表 → 设置一个 1 分钟后的闹钟 → 到时间收到提醒。

---

## ~~第九阶段：框架完备~~ ✅ 已完成

**目标**：统一入口，多硅基人协作。

| # | 模块 | 说明 |
|---|------|------|
| 9.1 | CoreHost + CoreHostBuilder | 统一宿主。Builder 模式注册所有组件。优雅关闭（Ctrl+C / SIGTERM） |
| 9.2 | Program.Main 重构 | 改为 CoreHostBuilder 模式 |
| 9.3 | SiliconBeingManager 增强 | 主理人优先响应机制。异常隔离。定期持久化 |
| 9.4 | 多硅基人加载 | 从数据目录加载多个硅基人。硅基人间通过 ChatTool 通信 |
| 9.5 | 性能监控 | 监控每个 TickObject 的执行时间 |
| 9.6 | ServiceLocator | 全局服务定位器。Register/Get 方法 |

**阶段产出**：多个硅基人同时运行，互相协作，框架通过 CoreHost 统一管理。

**验证方式**：创建两个硅基人 → 让 A 给 B 发消息 → B 收到并回复 → 框架正常调度无异常。

---

## ~~第十阶段：上 Web~~ ✅ 已完成

**目标**：从控制台迁移到浏览器界面。

| # | 模块 | 说明 |
|---|------|------|
| 10.1 | Router | HTTP 请求路由器。序号参数型路由和静态文件服务 |
| 10.2 | Controller 抽象类 | 请求/响应上下文。HTML 和 JSON 响应支持 |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | C# 服务端构建器。零前端框架依赖 |
| 10.6 | SSE（Server-Sent Events） | 基于推送的实时更新，用于聊天、硅基人状态和系统事件。比 WebSocket 更简单，支持客户端自动重连 |
| 10.7 | WebUIProvider | 基于 SSE 的实时 IM 通道。替代控制台作为主要界面 |
| 10.8 | Web 安全 | IP 黑白名单。`[WebCode]` 特性。动态更新 |
| 10.9–10.17 | Web 控制器 | Chat、Dashboard、Being、Task、Permission、PermissionRequest、Executor、Log、Config、Memory、Timer、Init、About、CodeBrowser、Knowledge、Project、Audit |

**阶段产出**：完整的 Web 界面，可通过浏览器访问。

**验证方式**：打开浏览器 → 与硅基人聊天 → 查看仪表盘 → 管理权限 → 全部功能正常。

---

## ~~第十点五阶段：增量增强~~ ✅ 已完成

**目标**：在已有系统基础上增加开发过程中发现的新能力。

| # | 模块 | 说明 |
|---|------|------|
| 10.5.1 | BroadcastChannel | 新会话类型，用于系统级广播。固定频道 ID，动态订阅，待读消息过滤 |
| 10.5.2 | ChatMessage 增强 | ToolCallId、ToolCallsJson、Thinking 字段用于 AI 上下文；PromptTokens、CompletionTokens、TotalTokens 用于 Token 追踪；SystemNotification 消息类型 |
| 10.5.3 | TokenUsageAuditManager | 跨硅基人的单次请求 Token 消耗追踪。聚合统计、时间序列查询、持久化存储 |
| 10.5.4 | TokenAuditTool | `[SiliconManagerOnly]` 工具，主理人可查询和汇总 Token 用量 |
| 10.5.5 | ConfigTool | `[SiliconManagerOnly]` 工具，主理人可读取和修改系统配置 |
| 10.5.6 | AuditController | Token 用量审计 Web 仪表盘，含趋势图表和数据导出 |
| 10.5.7 | 历法系统扩展 | 32 种历法实现，覆盖世界主要历法体系（佛历、中国农历、伊斯兰历、希伯来历、日本年号历、波斯历、玛雅历等） |
| 10.5.8 | DiskTool 增强 | 新增操作：count_lines、read_lines、clear_file、replace_lines、replace_text、replace_text_all、list_drives |
| 10.5.9 | SystemTool 增强 | 新增操作：find_process（支持通配符）、resource_usage |
| 10.5.10 | CalendarTool 增强 | 新增操作：diff、list_calendars、get_components、get_now_components、convert（跨历法转换） |

**阶段产出**：增强的工具集、可观测性和历法覆盖。

**验证方式**：主理人通过 TokenAuditTool 查询 Token 用量 → 审计仪表盘显示趋势 → CalendarTool 跨 32 种历法转换日期。

---

## 第十一阶段：外接 IM

**目标**：接入外部即时通讯平台。

| # | 模块 | 说明 |
|---|------|------|
| 11.1 | FeishuProvider | 飞书机器人集成 |
| 11.2 | WhatsAppProvider | WhatsApp Business API 集成 |
| 11.3 | TelegramProvider | Telegram Bot API 集成 |
| 11.4 | IMManager 增强 | 多通道路由，统一消息格式 |

**阶段产出**：用户可通过外部 IM 平台与硅基人交互。

---

## 第十二阶段：锦上添花

**目标**：可选的高级功能。

| # | 模块 | 说明 |
|---|------|------|
| 12.1 | 知识网络 | 共享知识图谱，三元组结构 |
| 12.2 | 插件系统 | 外部插件加载，安全检查 |
| 12.3 | 技能生态系统 | 可复用的功能模块市场 |
