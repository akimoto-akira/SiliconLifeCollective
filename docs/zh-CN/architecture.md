# 架构设计

[English](../../architecture.md) | [繁體中文](../zh-HK/architecture.md)

## 核心概念

### 硅基人

系统中的每个 AI 智能体都是一个**硅基人**——拥有独立身份、个性和能力的自主实体。每个硅基人由**灵魂文件**（Markdown 提示词）驱动，定义其行为模式。

### 硅基主理人

**硅基主理人**是拥有最高系统权限的特殊硅基人，充当系统管理员：

- 创建和管理其他硅基人
- 分析用户请求并分解为子任务
- 将任务分发给合适的硅基人
- 监控执行质量，处理失败情况
- 对用户消息享有**优先调度权**（详见下文）

### 灵魂文件

存储在每个硅基人数据目录中的 Markdown 文件（`soul.md`），作为系统提示词注入每次 AI 请求，定义硅基人的个性、决策模式和行为约束。

---

## 调度机制：时间分片公平调度

### MainLoop + TickObject

系统在专用后台线程运行**Tick 驱动的主循环**：

```
MainLoop（专用线程，看门狗 + 熔断器）
  └── TickObject A（Priority=0, Interval=100ms）
  └── TickObject B（Priority=1, Interval=500ms）
  └── SiliconBeingManager（由 MainLoop 直接驱动）
        └── SiliconBeingRunner → 硅基人 1 → Tick → ExecuteOneRound
        └── SiliconBeingRunner → 硅基人 2 → Tick → ExecuteOneRound
        └── SiliconBeingRunner → 硅基人 3 → Tick → ExecuteOneRound
        └── ...
```

关键设计决策：

- **硅基人不继承 TickObject**，拥有独立的 `Tick()` 方法，由 `SiliconBeingManager` 通过 `SiliconBeingRunner` 调用，不直接注册到 MainLoop。
- **SiliconBeingManager** 由 MainLoop 直接驱动，作为所有硅基人的唯一调度代理。
- **SiliconBeingRunner** 将每个硅基人的 `Tick()` 包装在独立临时线程中执行，带超时控制和每硅基人独立熔断器（连续 3 次超时 → 冷却 1 分钟）。
- 每个硅基人每次 Tick 只执行**一轮** AI 请求 + ToolCall，确保没有硅基人能独占主循环。
- **PerformanceMonitor** 追踪每次 Tick 的执行耗时，用于可观测性。

### 主理人优先响应

当用户向硅基主理人发送消息时：

1. 当前正在执行的硅基人（如硅基人 A）完成本轮执行——**不中断**。
2. 管理器**跳过剩余队列**。
3. 循环**从主理人重新开始**，使其立即获得执行机会。

这保证了用户交互的响应速度，同时不破坏正在执行的任务完整性。

---

## 组件架构

```
┌─────────────────────────────────────────────────────────┐
│                        CoreHost                         │
│  （统一宿主 — 组装和管理所有组件）                        │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ MainLoop │  │ ServiceLocator│  │      Config      │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │           SiliconBeingManager (TickObject)        │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │ 主理人  │ │硅基人 A │ │硅基人 B │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              共享服务层                            │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ChatSystem│  │ Storage  │  │  PermissionMgr  │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │ IAIClient│  │执行器层  │  │   ToolManager   │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                    执行器                         │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │  磁盘    │  │  网络    │  │    命令行       │  │   │
│  │  │执行器    │  │执行器    │  │    执行器       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              IM 通道层                            │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ 控制台   │  │  Web     │  │ 飞书 / ...      │  │   │
│  │  │通道      │  │通道      │  │ 通道            │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## ServiceLocator

`ServiceLocator` 是线程安全的单例服务注册表，提供对所有核心服务的访问：

| 属性 | 类型 | 说明 |
|------|------|------|
| `ChatSystem` | `ChatSystem` | 中央聊天会话管理器 |
| `IMManager` | `IMManager` | 即时通讯通道路由器 |
| `AuditLogger` | `AuditLogger` | 权限审计日志 |
| `GlobalAcl` | `GlobalACL` | 全局访问控制列表 |
| `BeingFactory` | `ISiliconBeingFactory` | 硅基人创建工厂 |
| `BeingManager` | `SiliconBeingManager` | 硅基人生命周期管理器 |
| `DynamicBeingLoader` | `DynamicBeingLoader` | 动态编译加载器 |
| `TokenUsageAudit` | `ITokenUsageAudit` | Token 用量追踪 |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Token 用量报告 |

同时维护以硅基人 GUID 为键的每硅基人 `PermissionManager` 注册表。

---

## 聊天系统

### 会话类型

聊天系统通过 `SessionBase` 支持三种会话类型：

| 类型 | 类 | 说明 |
|------|-----|------|
| `SingleChat` | `SingleChatSession` | 两个参与者之间的一对一对话 |
| `GroupChat` | `GroupChatSession` | 多参与者群组对话 |
| `Broadcast` | `BroadcastChannel` | 固定 ID 的开放频道；硅基人动态订阅，仅接收订阅后的消息 |

### BroadcastChannel

`BroadcastChannel` 是一种特殊的会话类型，用于系统级广播：

- **固定频道 ID** — 与 `SingleChatSession` 和 `GroupChatSession` 不同，频道 ID 是已知常量，而非由成员 GUID 派生。
- **动态订阅** — 硅基人在运行时订阅/取消订阅；仅接收订阅后发布的消息。
- **待读消息过滤** — `GetPendingMessages()` 仅返回硅基人订阅后且尚未阅读的消息。
- **由 ChatSystem 管理** — `GetOrCreateBroadcastChannel()`、`Broadcast()`、`GetPendingBroadcasts()`。

### ChatMessage

`ChatMessage` 模型包含 AI 对话上下文和 Token 追踪字段：

| 字段 | 类型 | 说明 |
|------|------|------|
| `Id` | `Guid` | 消息唯一标识 |
| `SenderId` | `Guid` | 发送者唯一标识 |
| `ChannelId` | `Guid` | 频道/会话标识 |
| `Content` | `string` | 消息内容 |
| `Timestamp` | `DateTime` | 消息发送时间 |
| `Type` | `MessageType` | Text、Image、File 或 SystemNotification |
| `ReadBy` | `List<Guid>` | 已阅读此消息的参与者 ID 列表 |
| `Role` | `MessageRole` | AI 对话角色（User、Assistant、Tool） |
| `ToolCallId` | `string?` | 工具结果消息的工具调用 ID |
| `ToolCallsJson` | `string?` | 助手消息的序列化工具调用 JSON |
| `Thinking` | `string?` | AI 的思维链推理内容 |
| `PromptTokens` | `int?` | 提示词 Token 数（输入） |
| `CompletionTokens` | `int?` | 补全 Token 数（输出） |
| `TotalTokens` | `int?` | 总 Token 数（输入 + 输出） |

---

## 关键设计决策

### Storage 设计为实例类（非静态）

`IStorage` 设计为可注入的实例类，而非静态工具类。这确保了：

- 直接操作文件系统——IStorage 是系统自身的持久化通道，**不经过执行器**。
- **AI 无法控制 IStorage**——执行器管辖的是 AI 通过工具发起的 IO，IStorage 管辖的是框架自身的内部数据读写。两者性质不同。
- 可通过 Mock 实现进行单元测试。
- 未来支持不同存储后端时，无需修改消费者代码。

### 执行器作为安全边界

执行器是 I/O 操作的**唯一通道**。需要磁盘、网络或命令行访问的工具**必须**通过执行器执行。这一设计强制：

- 每个执行器拥有**独立的调度线程**，通过线程锁定进行权限验证。
- 集中的权限检查——执行器查询硅基人的**私有权限管理器**。
- 支持优先级的请求排队和超时控制。
- 所有外部操作的审计日志。
- 异常隔离——一个执行器的失败不影响其他执行器。
- 熔断机制——连续失败时暂时熔断，防止雪崩。

### ContextManager 为轻量级对象

每次 `ExecuteOneRound()` 创建一个新的 `ContextManager` 实例：

1. 加载灵魂文件 + 最近聊天记录。
2. 发送请求到 AI 客户端。
3. 循环处理 ToolCall，直到 AI 返回纯文本。
4. 将回复持久化到 ChatSystem。
5. 释放资源。

这保证了每轮执行相互隔离、无状态。

### 通过类重写实现自我进化

硅基人可以在运行时重写自身的 C# 类：

1. AI 生成新的类代码（必须继承 `SiliconBeingBase`）。
2. **编译时引用控制**（主要防线）：编译器只获得允许的程序集列表——`System.IO`、`System.Reflection` 等被排除，危险操作在类型层面就不可能存在。
3. **运行时静态分析**（辅助防线）：`SecurityScanner` 即使在编译成功后仍对代码进行危险模式扫描。
4. Roslyn 在内存中编译代码。
5. 编译成功：`SiliconBeingManager.ReplaceBeing()` 替换当前实例，迁移状态，将加密代码持久化到磁盘。
6. 编译失败：丢弃新代码，保持现有实现。

自定义 `IPermissionCallback` 实现也可通过 `ReplacePermissionCallback()` 编译并注入，允许硅基人自定义自身的权限逻辑。

代码以 AES-256 加密形式存储在磁盘上。加密密钥通过 PBKDF2 从硅基人 GUID（全大写）派生。

---

## Token 用量审计

`TokenUsageAuditManager` 追踪所有硅基人的 AI Token 消耗：

- `TokenUsageRecord` — 单次请求记录（硅基人 ID、模型、提示词 Token 数、补全 Token 数、时间戳）
- `TokenUsageSummary` — 聚合统计
- `TokenUsageQuery` — 查询过滤参数
- 通过 `ITimeStorage` 持久化，支持时间序列查询
- 可通过 Web UI（AuditController）和 `TokenAuditTool`（仅主理人可用）访问

---

## 历法系统

系统包含 **32 种历法实现**，均派生自抽象基类 `CalendarBase`，覆盖世界主要历法体系：

| 历法 | ID | 说明 |
|------|-----|------|
| BuddhistCalendar | `buddhist` | 佛历（BE），年份 + 543 |
| CherokeeCalendar | `cherokee` | 切罗基历法 |
| ChineseLunarCalendar | `lunar` | 中国农历，含闰月计算 |
| ChulaSakaratCalendar | `chula_sakarat` | 朱拉历（CS），年份 - 638 |
| CopticCalendar | `coptic` | 科普特历 |
| DaiCalendar | `dai` | 傣历，含完整月相计算 |
| DehongDaiCalendar | `dehong_dai` | 德宏傣历变体 |
| EthiopianCalendar | `ethiopian` | 埃塞俄比亚历 |
| FrenchRepublicanCalendar | `french_republican` | 法国共和历 |
| GregorianCalendar | `gregorian` | 标准公历 |
| HebrewCalendar | `hebrew` | 希伯来历（犹太历） |
| IndianCalendar | `indian` | 印度国历 |
| InuitCalendar | `inuit` | 因纽特历法 |
| IslamicCalendar | `islamic` | 伊斯兰教历（希吉来历） |
| JapaneseCalendar | `japanese` | 日本年号历 |
| JavaneseCalendar | `javanese` | 爪哇伊斯兰历 |
| JucheCalendar | `juche` | 主体历（朝鲜），年份 - 1911 |
| JulianCalendar | `julian` | 儒略历 |
| KhmerCalendar | `khmer` | 高棉历 |
| MayanCalendar | `mayan` | 玛雅长计历 |
| MongolianCalendar | `mongolian` | 蒙古历 |
| PersianCalendar | `persian` | 波斯历（太阳希吉来历） |
| RepublicOfChinaCalendar | `roc` | 民国纪年，年份 - 1911 |
| RomanCalendar | `roman` | 罗马历 |
| SakaCalendar | `saka` | 塞迦历（印尼） |
| SexagenaryCalendar | `sexagenary` | 干支纪年 |
| TibetanCalendar | `tibetan` | 藏历 |
| VietnameseCalendar | `vietnamese` | 越南农历（猫年变体） |
| VikramSamvatCalendar | `vikram_samvat` | 维克拉姆历 |
| YiCalendar | `yi` | 彝历 |
| ZoroastrianCalendar | `zoroastrian` | 琐罗亚斯德历 |

`CalendarTool` 提供操作：`now`、`format`、`add_days`、`diff`、`list_calendars`、`get_components`、`get_now_components`、`convert`（跨历法日期转换）。

---

## Web UI 架构

### 皮肤系统

Web UI 采用**可插拔皮肤系统**，允许在不改变应用逻辑的情况下完全自定义 UI：

- **ISkin 接口** — 定义所有皮肤的契约，包括：
  - 核心渲染方法（`RenderHtml`、`RenderError`）
  - 20+ UI 组件方法（按钮、输入框、卡片、表格、徽章、气泡、进度条、标签页等）
  - 通过 `CssBuilder` 生成主题 CSS
  - `SkinPreviewInfo` — 初始化页面皮肤选择器的颜色预览信息

- **内置皮肤** — 4 种生产就绪的皮肤：
  - **Admin** — 专业、数据导向的界面，用于系统管理
  - **Chat** — 对话式、消息中心设计，用于 AI 交互
  - **Creative** — 艺术化、视觉丰富的布局，用于创意工作流
  - **Dev** — 开发者导向、代码中心的界面，支持语法高亮

- **皮肤发现** — `SkinManager` 通过反射自动发现并注册所有 `ISkin` 实现

### HTML / CSS / JS 构建器

Web UI 完全在 C# 中生成标记，不依赖任何模板文件：

- **`H`** — 流式 HTML 构建器 DSL，用于在代码中构建 HTML 树
- **`CssBuilder`** — 支持选择器和媒体查询的 CSS 构建器
- **`JsBuilder`（`JsSyntax`）** — 用于内联脚本的 JavaScript 构建器

### 控制器系统

Web UI 遵循 **MVC 模式**，有 17 个控制器处理不同方面：

| 控制器 | 用途 |
|--------|------|
| About | 关于页面和项目信息 |
| Audit | Token 用量审计仪表盘，含趋势图表和数据导出 |
| Being | 硅基人管理和状态 |
| Chat | 实时聊天界面（支持 SSE） |
| CodeBrowser | 代码查看和编辑 |
| Config | 系统配置 |
| Dashboard | 系统概览和指标 |
| Executor | 执行器状态和管理 |
| Init | 首次运行初始化向导 |
| Knowledge | 知识图谱可视化（占位符） |
| Log | 系统日志查看器 |
| Memory | 长期记忆浏览器 |
| Permission | 权限管理 |
| PermissionRequest | 权限请求队列 |
| Project | 项目管理（占位符） |
| Task | 任务系统界面 |
| Timer | 定时器系统管理 |

### 实时更新

- **SSE（Server-Sent Events）** — 通过 `SSEHandler` 推送聊天消息、硅基人状态和系统事件
- **无需 WebSocket** — 使用 SSE 满足大部分实时需求，架构更简单
- **自动重连** — 客户端重连逻辑，确保连接弹性

### 国际化

内置三种语言：`ZhCN`（简体中文）、`ZhHK`（繁体中文）和 `EnUS`（英文）。通过 `DefaultConfigData.Language` 配置，由 `LocalizationManager` 解析。

---

## 数据目录结构

```
data/
└── SiliconManager/
    ├── {主理人-guid}/
    │   ├── soul.md          # 主理人灵魂文件
    │   ├── state.json       # 运行时状态
    │   ├── code.enc         # AES 加密的自定义类代码
    │   └── permission.enc   # AES 加密的自定义权限回调
    │
    └── {硅基人-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```
