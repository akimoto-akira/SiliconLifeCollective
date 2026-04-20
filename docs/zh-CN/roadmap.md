# 路线图

[English](roadmap.md) | [中文](docs/zh-CN/roadmap.md) | [繁體中文](docs/zh-HK/roadmap.md) | [日本語](docs/ja-JP/roadmap.md) | [한국어](docs/ko-KR/roadmap.md)

## 指导原则

每个阶段都以**可运行、可观察**的系统结束。没有阶段会产生"一堆基础设施却没有可展示的东西"。

---

## ~~阶段 1：可以聊天~~ ✅ 已完成

**目标**：控制台输入 → AI 调用 → 控制台输出。最小可验证单元。

| # | 模块 | 描述 |
|---|--------|-------------|
| 1.1 | 解决方案和项目结构 | 创建 `SiliconLifeCollective.sln`，包含 `src/SiliconLife.Core/`（核心库）和 `src/SiliconLife.Default/`（默认实现 + 入口点） |
| 1.2 | 配置（最小化） | 单例 + JSON 反序列化。读取 `config.json`。如果缺失则自动生成默认值 |
| 1.3 | 本地化（最小化） | `LocalizationBase` 抽象类，`ZhCN` 实现。在配置中添加 `Language` |
| 1.4 | OllamaClient（最小化） | `IAIClient` 接口，HTTP 调用本地 Ollama `/api/chat`。尚无流式传输，尚无工具调用 |
| 1.5 | 控制台 I/O | `while(true) + Console.ReadLine()`，读取输入 → 调用 AI → 打印响应 |
| 1.6 | 版权头 | 为所有 C# 源文件添加 Apache 2.0 头 |

**交付物**：与本地 Ollama 模型对话的控制台聊天程序。

**验证**：运行程序，输入"hello"，看到 AI 响应。

---

## ~~阶段 2：有骨架~~ ✅ 已完成

**目标**：用框架结构替换"裸循环"。行为不变。

| # | 模块 | 描述 |
|---|--------|-------------|
| 2.1 | 存储（最小化） | `IStorage` 接口（Read/Write/Exists/Delete，键值对）。`FileSystemStorage` 实现。实例类（非静态）。直接文件系统访问 —— **AI 无法控制 IStorage** |
| 2.2 | 主循环 + 时钟对象 | 无限循环，精确时钟间隔（`Stopwatch` + `Thread.Sleep`）。优先级调度 |
| 2.3 | IAIClient 标准化 | `IAIClientFactory` 接口。OllamaClient 重构以实现标准接口 |
| 2.4 | 控制台迁移 | 将 `while(true)` 迁移到主循环驱动的时钟对象。行为与阶段 1 相同 |

**交付物**：主循环运行时钟，控制台聊天仍然工作。

**验证**：注册一个测试时钟对象，每秒打印时钟计数；控制台聊天仍然工作。

---

## ~~阶段 3：有灵魂~~ ✅ 已完成

**目标**：第一个硅基生命体在框架中存活。

| # | 模块 | 描述 |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | 抽象基类，包含 Id、Name、ToolManager、AIClient、ChatService、Storage、PermissionService。抽象 `Tick()` 和 `ExecuteOneRound()` |
| 3.2 | 灵魂文件加载 | `SoulFileManager`：从生命体数据目录读取 `soul.md` |
| 3.3 | ContextManager（最小化） | 连接灵魂文件 + 最近消息 → 调用 AI → 获取响应。尚无工具调用，尚无持久化 |
| 3.4 | ISiliconBeingFactory | 用于创建生命体实例的工厂接口 |
| 3.5 | SiliconBeingManager（最小化） | 继承时钟对象（优先级=0）。迭代所有生命体，依次调用它们的 Tick |
| 3.6 | DefaultSiliconBeing | 标准行为实现。检查未读消息 → 创建 ContextManager → ExecuteOneRound → 输出 |
| 3.7 | 生命体目录结构 | `DataDirectory/SiliconManager/{GUID}/`，包含 `soul.md` 和 `state.json` |

**交付物**：由主循环驱动的硅基生命体，接收控制台输入，加载灵魂文件，调用 AI。

**验证**：控制台输入 → 主循环时钟触发 → 生命体处理（带灵魂文件指导的行为） → AI 响应。响应风格应与阶段 1 不同。

---

## ~~阶段 4：有记忆~~ ✅ 已完成

**目标**：对话在重启后持久化。

| # | 模块 | 描述 |
|---|--------|-------------|
| 4.1 | ChatSystem | 频道概念（两个 GUID = 一个频道）。带持久化的消息模型。尚无群聊 |
| 4.2 | IIMProvider + IMManager | `IIMProvider` 接口。`ConsoleProvider` 作为正式即时通讯频道。`IMManager` 路由消息 |
| 4.3 | ContextManager 增强 | 从聊天系统拉取历史。持久化 AI 响应。支持多轮工具调用延续 |
| 4.4 | IMessage 模型 | 聊天系统和即时通讯管理器共享的统一消息模型 |

**交付物**：具有持久化记忆的聊天系统。

**验证**：聊天几轮 → 退出 → 重启 → 问"我们聊了什么？" → 生命体可以回答。

---

## ~~阶段 5：可以行动（工具系统）~~ ✅ 已完成

**目标**：硅基生命体可以执行操作，而不仅仅是聊天。

| # | 模块 | 描述 |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | `ITool` 接口，包含 Name、Description、Execute。`ToolResult` 包含 Success、Message、Data |
| 5.2 | ToolManager | 每个生命体的实例。基于反射的工具发现。`[SiliconManagerOnly]` 属性支持 |
| 5.3 | IAIClient：工具调用支持 | 解析 AI tool_calls。循环：执行工具 → 发送结果回去 → AI 继续 → 直到纯文本 |
| 5.4 | 执行器基类 | 抽象基类，具有独立调度线程、请求队列、超时控制 |
| 5.5 | NetworkExecutor | 通过执行器进行 HTTP 请求。超时、排队 |
| 5.6 | CommandLineExecutor | 通过执行器进行 Shell 执行。跨平台分隔符检测 |
| 5.7 | DiskExecutor | 通过执行器进行文件操作。尚无权限检查（阶段 6） |
| 5.8–5.12 | 内置工具 | CalendarTool、SystemTool、NetworkTool、ChatTool、DiskTool |

**交付物**：硅基生命体可以调用工具执行操作。

**验证**：问"今天星期几" → CalendarTool 回答；问"检查进程" → SystemTool 执行；告诉生命体给另一个生命体发消息 → ChatTool 工作。

---

## ~~阶段 6：遵守规则（权限系统）~~ ✅ 已完成

**目标**：硅基生命体未经授权无法访问敏感资源。

| # | 模块 | 描述 |
|---|--------|-------------|
| 6.1 | PermissionManager | 每个生命体的私有实例。基于回调，三元结果（Allowed/Deny/AskUser）。查询优先级：HighDeny → HighAllow → Callback。IsCurator 标志 |
| 6.2 | PermissionType 枚举 | NetworkAccess、CommandLine、FileAccess、Function、DataAccess |
| 6.3 | DefaultPermissionCallback | 网络白名单/黑名单、CLI 分类、文件路径安全规则 |
| 6.4 | GlobalACL | 前缀匹配规则表，持久化到存储 |
| 6.5 | UserFrequencyCache | HighAllow/HighDeny 列表。用户选择（非自动检测）。前缀匹配，仅内存，可配置过期 |
| 6.6 | UserAskMechanism（控制台） | 当返回 AskUser 时控制台提示 y/n |
| 6.7 | 执行器权限集成 | 所有执行器在执行前检查权限 |
| 6.8 | IStorage 隔离说明 | IStorage 是系统内部持久化 —— 直接文件访问，**不**通过执行器路由，**不**可由 AI 控制。执行器仅管理 AI 工具发起的 IO |
| 6.9 | 审计日志 | 记录所有权限决策，带时间戳、请求者、资源、结果 |

**交付物**：当生命体尝试敏感操作时出现权限提示。

**验证**：告诉生命体删除文件 → 控制台显示权限提示 → 输入 `n` → 操作被拒绝。告诉生命体访问白名单网站 → 立即允许。

---

## ~~阶段 7：可以进化（动态编译）~~ ✅ 已完成

**目标**：硅基生命体可以重写自己的代码。

| # | 模块 | 描述 |
|---|--------|-------------|
| 7.1 | CodeEncryption | AES-256 加密/解密。从 GUID 派生 PBKDF2 密钥 |
| 7.2 | DynamicCompilationExecutor | 基于 Roslyn 的内存编译沙箱。编译时装配引用控制（主要防御：排除 System.IO、Reflection 等） |
| 7.3 | 安全扫描 | 运行时静态分析危险代码模式（次要防御）。如果扫描失败则阻止加载 |
| 7.4 | 生命体生命周期增强 | 加载：解密 → 扫描 → 编译 → 实例化。运行时：在内存中编译 → 原子替换 → 持久化加密 |
| 7.5 | SiliconCurator | 主理人抽象基类。IsCurator=true。最高权限 |
| 7.6 | DefaultCurator | 默认主理人实现，带内置灵魂文件和管理工具 |
| 7.7 | CuratorTool | `[SiliconManagerOnly]` 工具：list_beings、create_being、get_code、reset |
| 7.8 | 权限回调覆盖 | 生命体可以编译自定义权限回调 |
| 7.9 | SiliconBeingManager 增强 | Replace 方法（运行时实例交换）。MigrateState（在旧实例和新实例之间转移状态） |

**交付物**：硅基生命体可以通过 AI 生成新代码，编译并替换自己。

**验证**：告诉生命体"给自己添加一个新功能" → 观察编译 → 重启 → 新功能工作。

---

## ~~阶段 8：记忆和计划~~ ✅ 已完成

**目标**：长期记忆、任务管理、定时触发。

| # | 模块 | 描述 |
|---|--------|-------------|
| 8.1 | FileSystemMemory | 短期/长期分段存储。时间衰减。压缩（合并相似记忆）。多维搜索 |
| 8.2 | TaskSystem | 一次性 + DAG 依赖任务。优先级调度。状态跟踪 |
| 8.3 | TimerSystem | 一次性闹钟 + 周期定时器。毫秒精度。持久化到存储 |
| 8.4 | IncompleteDate | 模糊日期范围结构（例如"2026 年 4 月"、"2026 年春"） |
| 8.5–8.7 | 记忆/任务/定时器工具 | 生命体查询记忆、管理任务、设置定时器的工具 |

**交付物**：生命体可以记住关键点、创建/跟踪任务、设置闹钟。

**验证**：创建任务 → 检查任务列表 → 设置 1 分钟闹钟 → 时间到时接收通知。

---

## ~~阶段 9：框架完成~~ ✅ 已完成

**目标**：统一入口点，多生命体协作。

| # | 模块 | 描述 |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | 使用构建器模式的统一主机。优雅关闭（Ctrl+C / SIGTERM） |
| 9.2 | Program.Main 重构 | 迁移到 CoreHostBuilder 模式 |
| 9.3 | SiliconBeingManager 增强 | 主理人优先响应。异常隔离。定期持久化 |
| 9.4 | 多生命体加载 | 从数据目录加载多个生命体。通过 ChatTool 进行生命体间通信 |
| 9.5 | 性能监控 | 每个时钟对象执行时间跟踪 |
| 9.6 | ServiceLocator | 全局服务定位器，带 Register/Get 方法 |

**交付物**：多个生命体同时运行，协作，由 CoreHost 管理。

**验证**：创建两个生命体 → A 给 B 发消息 → B 接收并回复 → 框架调度无错误。当用户消息到达时主理人优先响应。

---

## ~~阶段 10：走向 Web~~ ✅ 已完成

**目标**：从控制台迁移到浏览器界面。

| # | 模块 | 描述 |
|---|--------|-------------|
| 10.1 | Router | HTTP 请求路由器。序列参数路由和静态文件服务 |
| 10.2 | Controller 基类 | 请求/响应上下文。HTML 和 JSON 响应支持 |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | C# 服务器端构建器。零前端框架依赖 |
| 10.6 | SSE（服务器发送事件） | 推送式实时更新聊天、生命体状态和系统事件。比 WebSocket 更简单，带自动客户端重连 |
| 10.7 | WebUIProvider | 基于 SSE 的实时即时通讯频道。替换控制台作为主要界面 |
| 10.8 | Web 安全 | IP 黑名单/白名单。`[WebCode]` 属性。动态更新 |
| 10.9–10.17 | Web 控制器 | 聊天、仪表板、生命体、任务、权限、权限请求、执行器、日志、配置、记忆、定时器、初始化、关于、代码浏览器、知识、项目、审计 |

**交付物**：可从浏览器访问的完整 Web UI。

**验证**：打开浏览器 → 与生命体聊天 → 查看仪表板 → 管理权限 → 全部功能正常。

---

## ~~阶段 10.5：增量增强~~ ✅ 已完成

**目标**：使用开发过程中发现的新功能增强现有系统。

| # | 模块 | 描述 |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | 用于系统范围公告的新会话类型。固定频道 ID、动态订阅、待处理消息过滤 |
| 10.5.2 | ChatMessage 增强 | ToolCallId、ToolCallsJson、Thinking 字段用于 AI 上下文；PromptTokens、CompletionTokens、TotalTokens 用于 token 跟踪；SystemNotification 消息类型 |
| 10.5.3 | TokenUsageAuditManager | 跨所有生命体的每次请求 token 消耗跟踪。聚合统计、时间序列查询、持久化存储 |
| 10.5.4 | TokenAuditTool | `[SiliconManagerOnly]` 工具，供主理人查询和汇总 token 使用 |
| 10.5.5 | ConfigTool | `[SiliconManagerOnly]` 工具，供主理人读取和修改系统配置 |
| 10.5.6 | AuditController | Web 仪表板用于 token 使用审计，带趋势图和数据导出 |
| 10.5.7 | 日历系统扩展 | 32 种日历实现，涵盖世界日历系统（佛历、农历、伊斯兰历、希伯来历、日本历、波斯历、玛雅历等） |
| 10.5.8 | DiskTool 增强 | 新操作：count_lines、read_lines、clear_file、replace_lines、replace_text、replace_text_all、list_drives |
| 10.5.9 | SystemTool 增强 | 新操作：find_process（支持通配符）、resource_usage |
| 10.5.10 | CalendarTool 增强 | 新操作：diff、list_calendars、get_components、get_now_components、convert（跨日历转换） |
| 10.5.11 | DashScopeClient | 阿里云百炼 AI 客户端，兼容 OpenAI API。支持流式传输、工具调用、推理内容 |
| 10.5.12 | DashScopeClientFactory | 用于创建百炼客户端的工厂。通过 API 动态模型发现。多区域支持（北京、弗吉尼亚、新加坡、香港、法兰克福） |
| 10.5.13 | AI 客户端配置系统 | 每个生命体的 AI 客户端配置。动态配置键选项（模型、区域）。本地化显示名称 |
| 10.5.14 | 本地化扩展 | 简体中文、繁体中文、英文和日语本地化，用于百炼配置选项、模型名称和区域名称 |

**交付物**：增强的工具、可观察性、日历覆盖范围和多 AI 后端支持。

**验证**：主理人通过 TokenAuditTool 查询 token 使用 → 审计仪表板显示趋势 → CalendarTool 在 32 种日历系统之间转换日期 → 将 AI 后端切换到百炼 → 通过云端 API 与通义千问模型聊天。

---

## 阶段 11：外部即时通讯集成

**目标**：连接到外部消息传递平台，以更广泛的用户可访问性。

| # | 模块 | 描述 |
|---|--------|-------------|
| 11.1 | FeishuProvider | 飞书（Lark）机器人集成，支持卡片 |
| 11.2 | WhatsAppProvider | WhatsApp Business API 集成 |
| 11.3 | TelegramProvider | Telegram Bot API 集成，支持内联键盘 |
| 11.4 | IMManager 增强 | 多提供者路由、统一消息格式、跨平台权限询问处理 |

**交付物**：用户可以通过外部即时通讯平台与硅基生命体交互。

---

## 阶段 12：高级功能

**目标**：用于增强功能的可选高级功能。

| # | 模块 | 描述 |
|---|--------|-------------|
| 12.1 | 知识网络 | 使用三元结构（主谓宾）的共享知识图谱 |
| 12.2 | 插件系统 | 外部插件加载，带安全检查和沙箱 |
| 12.3 | 技能生态系统 | 可重用技能市场，用于生命体能力 |
