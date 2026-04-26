# 架构

[English](../en/architecture.md) | [Deutsch](../de-DE/architecture.md) | **中文** | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Čeština](../cs-CZ/architecture.md)

## 核心概念

### 硅基生命体

系统中的每个 AI 智能体都是一个**硅基生命体** —— 一个具有自身身份、个性和能力的自主实体。每个硅基生命体都由一个**灵魂文件**（Markdown 提示词）驱动，定义其行为模式。

### 硅基主理人

**硅基主理人**是一个具有最高系统权限的特殊硅基生命体。它充当系统管理员：

- 创建和管理其他硅基生命体
- 分析用户请求并将其分解为任务
- 将任务分发给适当的硅基生命体
- 监控执行质量并处理失败
- 使用**优先调度**响应用户消息（见下文）

### 灵魂文件

存储在每個硅基生命体数据目录中的 Markdown 文件（`soul.md`）。它作为系统提示词注入到每个 AI 请求中，定义生命体的个性、决策模式和行为约束。

---

## 调度：时隙公平调度

### 主循环 + 时钟对象

系统在专用后台线程上运行一个**时钟驱动的主循环**：

```
主循环（专用线程，看门狗 + 熔断器）
  └── 时钟对象 A（优先级=0，间隔=100ms）
  └── 时钟对象 B（优先级=1，间隔=500ms）
  └── 硅基生命体管理器（由主循环直接时钟触发）
        └── 硅基生命体运行器 → 硅基生命体 1 → 时钟触发 → 执行一轮
        └── 硅基生命体运行器 → 硅基生命体 2 → 时钟触发 → 执行一轮
        └── 硅基生命体运行器 → 硅基生命体 3 → 时钟触发 → 执行一轮
        └── ...
```

关键设计决策：

- **硅基生命体不继承时钟对象。** 它们有自己的 `Tick()` 方法，由 `SiliconBeingManager` 通过 `SiliconBeingRunner` 调用，而不是直接注册到主循环。
- **硅基生命体管理器**由主循环直接时钟触发，并作为所有生命体的单一代理。
- **硅基生命体运行器**在临时线程上包装每个生命体的 `Tick()`，具有超时和每个生命体的熔断器（连续 3 次超时 → 1 分钟冷却）。
- 每个生命体的执行限制为每次时钟触发**一轮** AI 请求 + 工具调用，确保没有生命体可以垄断主循环。
- **性能监控器**跟踪时钟执行时间以实现可观察性。

### 主理人优先响应

当用户向硅基主理人发送消息时：

1. 当前生命体（例如生命体 A）完成其当前轮次 —— **不中断**。
2. 管理器**跳过剩余队列**。
3. 循环**从主理人重新开始**，使其立即执行。

这确保了响应用户交互，同时不干扰进行中的任务。

---

## 组件架构

```
┌─────────────────────────────────────────────────────────┐
│                        核心主机                          │
│  （统一主机 —— 装配和管理所有组件）                        │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ 主循环    │  │ 服务定位器    │  │      配置         │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │           硅基生命体管理器（时钟对象）               │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │主理人      │ │生命体 A  │ │生命体 B  │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              共享服务                              │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │聊天系统   │  │ 存储     │  │  权限管理器       │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │ AI 客户端 │  │执行器     │  │   工具管理器      │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  执行器                            │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │  磁盘     │  │ 网络     │  │  命令行          │  │   │
│  │  │执行器     │  │执行器     │  │  执行器          │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              即时通讯提供者                        │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ 控制台    │  │  Web     │  │  飞书 / ...      │  │   │
│  │  │提供者     │  │提供者     │  │  提供者          │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## 服务定位器

`ServiceLocator` 是一个线程安全的单例注册表，提供对所有核心服务的访问：

| 属性 | 类型 | 描述 |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | 中央聊天会话管理器 |
| `IMManager` | `IMManager` | 即时通讯提供者路由器 |
| `AuditLogger` | `AuditLogger` | 权限审计跟踪 |
| `GlobalAcl` | `GlobalACL` | 全局访问控制列表 |
| `BeingFactory` | `ISiliconBeingFactory` | 创建生命体的工厂 |
| `BeingManager` | `SiliconBeingManager` | 活动生命体生命周期管理器 |
| `DynamicBeingLoader` | `DynamicBeingLoader` | 动态编译加载器 |
| `TokenUsageAudit` | `ITokenUsageAudit` | Token 使用跟踪 |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Token 使用报告 |

它还维护每个生命体的 `PermissionManager` 注册表，以生命体 GUID 为键。

---

## 聊天系统

### 会话类型

聊天系统通过 `SessionBase` 支持三种会话类型：

| 类型 | 类 | 描述 |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | 两个参与者之间的一对一对话 |
| `GroupChat` | `GroupChatSession` | 多参与者群聊 |
| `Broadcast` | `BroadcastChannel` | 具有固定 ID 的开放频道；生命体动态订阅，仅在订阅后接收消息 |

### 广播频道

`BroadcastChannel` 是一种特殊的会话类型，用于系统范围的公告：

- **固定频道 ID** —— 与 `SingleChatSession` 和 `GroupChatSession` 不同，频道 ID 是众所周知的常量，而不是从成员 GUID 派生。
- **动态订阅** —— 生命体在运行时订阅/取消订阅；它们只接收订阅后发布的消息。
- **待处理消息过滤** —— `GetPendingMessages()` 仅返回在生命体订阅时间之后发布且尚未读取的消息。
- **由聊天系统管理** —— `GetOrCreateBroadcastChannel()`、`Broadcast()`、`GetPendingBroadcasts()`。

### 聊天消息

`ChatMessage` 模型包含 AI 对话上下文和 token 跟踪的字段：

| 字段 | 类型 | 描述 |
|-------|------|-------------|
| `Id` | `Guid` | 唯一消息标识符 |
| `SenderId` | `Guid` | 发送者的唯一标识符 |
| `ChannelId` | `Guid` | 频道/对话标识符 |
| `Content` | `string` | 消息内容 |
| `Timestamp` | `DateTime` | 消息发送时间 |
| `Type` | `MessageType` | 文本、图片、文件或系统通知 |
| `ReadBy` | `List<Guid>` | 已阅读此消息的参与者 ID |
| `Role` | `MessageRole` | AI 对话角色（用户、助手、工具） |
| `ToolCallId` | `string?` | 工具结果消息的工具调用 ID |
| `ToolCallsJson` | `string?` | 助手消息的序列化工具调用 JSON |
| `Thinking` | `string?` | AI 的思维链推理 |
| `PromptTokens` | `int?` | 提示词中的 token 数量（输入） |
| `CompletionTokens` | `int?` | 补全中的 token 数量（输出） |
| `TotalTokens` | `int?` | 使用的总 token 数量（输入 + 输出） |
| `FileMetadata` | `FileMetadata?` | 附加的文件元数据（如果消息包含文件） |

### 聊天消息队列

`ChatMessageQueue` 是一个线程安全的消息队列系统，用于管理聊天消息的异步处理：

- **线程安全** - 使用锁机制确保并发访问安全
- **异步处理** - 支持异步消息入队和出队
- **消息排序** - 保持消息的时间顺序
- **批量操作** - 支持批量获取消息

### 文件元数据

`FileMetadata` 用于管理附加到聊天消息的文件信息：

- **文件信息** - 文件名、大小、类型、路径
- **上传时间** - 文件上传的时间戳
- **上传者** - 上传文件的用户或硅基生命体 ID

### 流取消管理器

`StreamCancellationManager` 提供 AI 流式响应的取消机制：

- **流控制** - 支持取消正在进行的 AI 流式响应
- **资源清理** - 取消时正确清理相关资源
- **并发安全** - 支持多个流同时管理

### 聊天历史查看

新增的聊天历史查看功能允许用户浏览硅基生命体的历史对话：

- **会话列表** - 显示所有历史会话
- **消息详情** - 查看完整消息历史
- **时间线视图** - 按时间顺序展示消息
- **API 支持** - 提供 RESTful API 获取会话和消息数据

---

## AI 客户端系统

系统通过 `IAIClient` 接口支持多个 AI 后端：

### OllamaClient

- **类型**：本地 AI 服务
- **协议**：原生 Ollama HTTP API（`/api/chat`、`/api/generate`）
- **功能**：流式传输、工具调用、本地模型托管
- **配置**：`endpoint`、`model`、`temperature`、`maxTokens`

### DashScopeClient（阿里云百炼）

- **类型**：云端 AI 服务
- **协议**：兼容 OpenAI 的 API（`/compatible-mode/v1/chat/completions`）
- **认证**：Bearer token（API 密钥）
- **功能**：流式传输、工具调用、推理内容（思维链）、多区域部署
- **支持的区域**：
  - `beijing` —— 华北2（北京）
  - `virginia` —— 美国（弗吉尼亚）
  - `singapore` —— 新加坡
  - `hongkong` —— 中国香港
  - `frankfurt` —— 德国（法兰克福）
- **支持的模型**（通过 API 动态发现，带有回退列表）：
  - **通义千问系列**：qwen3-max、qwen3.6-plus、qwen3.6-flash、qwen-max、qwen-plus、qwen-turbo、qwen3-coder-plus
  - **推理**：qwq-plus
  - **第三方**：deepseek-v3.2、deepseek-r1、glm-5.1、kimi-k2.5、llama-4-maverick
- **配置**：`apiKey`、`region`、`model`
- **模型发现**：运行时从百炼 API 获取可用模型；网络故障时回退到精选列表

### 客户端工厂模式

每种 AI 客户端类型都有相应的工厂实现 `IAIClientFactory`：

- `OllamaClientFactory` —— 创建 OllamaClient 实例
- `DashScopeClientFactory` —— 创建 DashScopeClient 实例

工厂提供：
- `CreateClient(Dictionary<string, object> config)` —— 从配置实例化客户端
- `GetConfigKeyOptions(string key, ...)` —— 返回配置键的动态选项（例如可用模型、区域）
- `GetDisplayName()` —— 客户端类型的本地化显示名称

### AI平台支持清单

#### 状态说明
- ✅ 已实现
- 🚧 开发中
- 📋 计划中
- 💡 考虑中

*注：受开发者所在网络环境影响，对接[考虑中]的海外云端AI服务可能需要借助网络代理工具进行访问，调试过程可能存在不稳定性。*

#### 平台列表

| 平台 | 状态 | 类型 | 说明 |
|------|------|------|------|
| Ollama | ✅ | 本地 | 本地AI服务，支持本地模型部署 |
| DashScope（阿里云百炼） | ✅ | 云端 | 阿里云百炼AI服务，支持多区域部署 |
| 百度千帆（文心一言） | 📋 | 云端 | 百度文心一言AI服务 |
| 智普AI（GLM） | 📋 | 云端 | 智谱清言AI服务 |
| 月之暗面（Kimi） | 📋 | 云端 | 月之暗面Kimi AI服务 |
| 火山方舟引擎.豆包 | 📋 | 云端 | 字节跳动豆包AI服务 |
| DeepSeek（直连） | 📋 | 云端 | 深度求索AI服务 |
| 零一万物 | 📋 | 云端 | 零一万物AI服务 |
| 腾讯混元 | 📋 | 云端 | 腾讯混元AI服务 |
| 硅基流动 | 📋 | 云端 | 硅基流动AI服务 |
| MiniMax | 📋 | 云端 | MiniMax AI服务 |
| OpenAI | 💡 | 云端 | OpenAI API服务（GPT系列） |
| Anthropic | 💡 | 云端 | Anthropic Claude AI服务 |
| Google DeepMind | 💡 | 云端 | Google Gemini AI服务 |
| Mistral AI | 💡 | 云端 | Mistral AI服务 |
| Groq | 💡 | 云端 | Groq高速AI推理服务 |
| Together AI | 💡 | 云端 | Together AI开源模型服务 |
| xAI | 💡 | 云端 | xAI Grok服务 |
| Cohere | 💡 | 云端 | Cohere企业级NLP服务 |
| Replicate | 💡 | 云端 | Replicate开源模型托管平台 |
| Hugging Face | 💡 | 云端 | Hugging Face开源AI社区和模型平台 |
| Cerebras | 💡 | 云端 | Cerebras AI推理优化服务 |
| Databricks | 💡 | 云端 | Databricks企业AI平台（MosaicML） |
| Perplexity AI | 💡 | 云端 | Perplexity AI搜索问答服务 |
| NVIDIA NIM | 💡 | 云端 | NVIDIA AI推理微服务 |

---

## 关键设计决策

### 存储作为实例类（而非静态）

`IStorage` 被设计为可注入的实例，而不是静态工具。这确保：

- 直接文件系统访问 —— IStorage 是系统的内部持久化通道，**不**通过执行器路由。
- **AI 无法控制 IStorage** —— 执行器管理 AI 工具发起的 IO；IStorage 管理框架自身的内部数据读写。这些是根本不同的关注点。
- 可使用模拟实现进行测试。
- 未来支持不同的存储后端，无需修改消费者。

### 执行器作为安全边界

执行器是 I/O 操作的**唯一**路径。需要磁盘、网络或命令行访问的工具**必须**通过执行器。此设计强制执行：

- 每个执行器拥有**独立的调度线程**，带有用于权限验证的线程锁定。
- 集中式权限检查 —— 执行器查询生命体的**私有权限管理器**。
- 支持优先级和超时控制的请求队列。
- 所有外部操作的审计日志。
- 异常隔离 —— 一个执行器的失败不影响其他执行器。
- 熔断器 —— 连续失败暂时停止执行器以防止级联失败。

### ContextManager 作为轻量级对象

每次 `ExecuteOneRound()` 创建一个新的 `ContextManager` 实例：

1. 加载灵魂文件 + 最近的聊天历史。
2. 将请求发送到 AI 客户端。
3. 循环处理工具调用，直到 AI 返回纯文本。
4. 将响应持久化到聊天系统。
5. 释放。

这使每轮保持隔离和无状态。

### 通过类重写实现自我进化

硅基生命体可以在运行时重写自己的 C# 类：

1. AI 生成新类代码（必须继承 `SiliconBeingBase`）。
2. **编译时引用控制**（主要防御）：编译器只获得允许的装配列表 —— `System.IO`、`System.Reflection` 等被排除，因此危险代码在类型级别是不可能的。
3. **运行时静态分析**（次要防御）：`SecurityScanner` 在成功编译后扫描代码中的危险模式。
4. Roslyn 在内存中编译代码。
5. 成功时：`SiliconBeingManager.ReplaceBeing()` 交换当前实例，迁移状态，并将加密代码持久化到磁盘。
6. 失败时：丢弃新代码，保留现有实现。

自定义 `IPermissionCallback` 实现也可以通过 `ReplacePermissionCallback()` 编译和注入，允许生命体自定义自己的权限逻辑。

代码在磁盘上以 AES-256 加密存储。加密密钥从生命体的 GUID（大写）通过 PBKDF2 派生。

---

## Token 使用审计

`TokenUsageAuditManager` 跟踪所有生命体的 AI token 消耗：

- `TokenUsageRecord` —— 每次请求的记录（生命体 ID、模型、提示词 token、补全 token、时间戳）
- `TokenUsageSummary` —— 聚合统计
- `TokenUsageQuery` —— 用于过滤记录的查询参数
- 通过 `ITimeStorage` 持久化以进行时间序列查询
- 可通过 Web UI（AuditController）和 `TokenAuditTool`（仅主理人）访问

---

### 日历系统

系统包含 **32 种日历实现**，派生自抽象 `CalendarBase` 类，涵盖世界主要日历系统：

| 日历 | ID | 描述 |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | 佛历（BE），年份 + 543 |
| CherokeeCalendar | `cherokee` | 切罗基日历系统 |
| ChineseLunarCalendar | `lunar` | 中国农历，带闰月 |
| ChineseHistoricalCalendar | `chinese_historical` | 中国历史历法，支持干支纪年和帝王年号 |
| ChulaSakaratCalendar | `chula_sakarat` | 朱拉萨卡拉特历（CS），年份 - 638 |
| CopticCalendar | `coptic` | 科普特历 |
| DaiCalendar | `dai` | 傣历，带完整农历计算 |
| DehongDaiCalendar | `dehong_dai` | 德宏傣历变体 |
| EthiopianCalendar | `ethiopian` | 埃塞俄比亚历 |
| FrenchRepublicanCalendar | `french_republican` | 法国共和历 |
| GregorianCalendar | `gregorian` | 标准公历 |
| HebrewCalendar | `hebrew` | 希伯来（犹太）历 |
| IndianCalendar | `indian` | 印度国历 |
| InuitCalendar | `inuit` | 因纽特日历系统 |
| IslamicCalendar | `islamic` | 伊斯兰回历 |
| JapaneseCalendar | `japanese` | 日本年号（Nengo）历 |
| JavaneseCalendar | `javanese` | 爪哇伊斯兰历 |
| JucheCalendar | `juche` | 主体历（朝鲜），年份 - 1911 |
| JulianCalendar | `julian` | 儒略历 |
| KhmerCalendar | `khmer` | 高棉历 |
| MayanCalendar | `mayan` | 玛雅长计历 |
| MongolianCalendar | `mongolian` | 蒙古历 |
| PersianCalendar | `persian` | 波斯（太阳回历）历 |
| RepublicOfChinaCalendar | `roc` | 中华民国（民国）历，年份 - 1911 |
| RomanCalendar | `roman` | 罗马历 |
| SakaCalendar | `saka` | 萨卡历（印度尼西亚） |
| SexagenaryCalendar | `sexagenary` | 中国干支历（Ganzhi） |
| TibetanCalendar | `tibetan` | 藏历 |
| VietnameseCalendar | `vietnamese` | 越南农历（猫生肖变体） |
| VikramSamvatCalendar | `vikram_samvat` | 维克拉姆桑巴特历 |
| YiCalendar | `yi` | 彝历系统 |
| ZoroastrianCalendar | `zoroastrian` | 祆历 |

`CalendarTool` 提供操作：`now`、`format`、`add_days`、`diff`、`list_calendars`、`get_components`、`get_now_components`、`convert`（跨日历日期转换）。

---

## Web UI 架构

### 皮肤系统

Web UI 具有**可插拔的皮肤系统**，允许完整的 UI 定制，无需更改应用程序逻辑：

- **ISkin 接口** —— 定义所有皮肤的契约，包括：
  - 核心渲染方法（`RenderHtml`、`RenderError`）
  - 20+ UI 组件方法（按钮、输入、卡片、表格、徽章、气泡、进度、标签等）
  - 通过 `CssBuilder` 生成主题 CSS
  - `SkinPreviewInfo` —— 初始化页面皮肤选择器的调色板和图标

- **内置皮肤** —— 4 种生产就绪的皮肤：
  - **Admin** —— 专业、数据聚焦的系统管理界面
  - **Chat** —— 对话式、以消息为中心的设计，用于 AI 交互
  - **Creative** —— 艺术性、视觉丰富的创意工作流程布局
  - **Dev** —— 以开发者为中心、以代码为中心的界面，带语法高亮

- **皮肤发现** —— `SkinManager` 通过反射自动发现和注册所有 `ISkin` 实现

### HTML / CSS / JS 构建器

Web UI 完全避免模板文件，在 C# 中生成所有标记：

- **`H`** —— 流式 HTML 构建器 DSL，用于在代码中构建 HTML 树
- **`CssBuilder`** —— CSS 构建器，支持选择器和媒体查询
- **`JsBuilder`（`JsSyntax`）** —— JavaScript 构建器，用于内联脚本

### 控制器系统

Web UI 遵循**类 MVC 模式**，20+ 个控制器处理不同方面：

| 控制器 | 用途 |
|------------|---------|
| About | 关于页面和项目信息 |
| Audit | Token 使用审计仪表板，带趋势图和导出 |
| Being | 硅基生命体管理和状态 |
| Chat | 带 SSE 的实时聊天界面 |
| ChatHistory | 聊天历史查看，支持会话列表和消息详情 |
| CodeBrowser | 代码查看和编辑 |
| CodeHover | 代码悬浮提示，支持语法高亮 |
| Config | 系统配置管理 |
| Dashboard | 系统概览和指标 |
| Executor | 执行器状态和管理 |
| Help | 帮助文档系统，多语言支持 |
| Init | 首次运行初始化向导 |
| Knowledge | 知识图谱可视化和查询 |
| Log | 系统日志查看器，支持硅基生命体筛选 |
| Memory | 长期记忆浏览器，支持高级过滤、统计和详情视图 |
| Permission | 权限管理 |
| PermissionRequest | 权限请求队列 |
| Project | 项目管理，包含工作笔记和任务系统 |
| Task | 任务系统界面 |
| Timer | 定时器系统管理，包含执行历史 |
| WorkNote | 工作笔记管理，支持搜索和目录生成 |

### 实时更新

- **SSE（服务器发送事件）** —— 通过 `SSEHandler` 推送聊天消息、生命体状态和系统事件的更新
- **无需 WebSocket** —— 使用 SSE 满足大多数实时需求的更简单架构
- **自动重连** —— 客户端重连逻辑实现弹性连接

### 本地化

系统支持 **21 种语言变体**的全面本地化：
- **中文（6 种）**：zh-CN（简体）、zh-HK（繁体）、zh-SG（新加坡）、zh-MO（澳门）、zh-TW（台湾）、zhMY（马来西亚）
- **英文（10 种）**：en-US、en-GB、en-CA、en-AU、en-IN、en-SG、en-ZA、en-IE、en-NZ、en-MY
- **西班牙语（2 种）**：es-ES、es-MX
- **其他（3 种）**：ja-JP（日语）、ko-KR（韩语）、cs-CZ（捷克语）

通过 `DefaultConfigData.Language` 选择活动语言环境，并通过 `LocalizationManager` 解析。

---

### WebView 浏览器自动化系统（新增）

系统集成了基于 **Playwright** 的 WebView 浏览器自动化功能：

- **个体隔离**：每个硅基生命体拥有独立的浏览器实例、Cookie 和会话存储，完全隔离互不干扰。
- **无头模式**：浏览器运行在用户完全不可见的无头模式下，硅基生命体后台自主操作。
- **WebViewBrowserTool**：提供完整的浏览器操作能力，包括：
  - 页面导航、点击、输入文本、获取页面内容
  - 执行 JavaScript、获取截图、等待元素出现
  - 浏览器状态管理和资源清理
- **安全控制**：所有浏览器操作均需通过权限验证链，防止恶意网页访问。

### 知识网络系统（新增）

系统内置基于**三元组结构**的知识图谱系统：

- **知识表示**：采用“主体-关系-客体”三元组结构（例如：Python-is_a-programming_language）
- **KnowledgeTool**：提供知识的全生命周期管理：
  - `add`/`query`/`update`/`delete` - 基础 CRUD 操作
  - `search` - 全文搜索和关键词匹配
  - `get_path` - 发现两个概念间的关联路径
  - `validate` - 知识完整性检查
  - `stats` - 知识网络统计分析
- **持久化存储**：知识三元组持久化到文件系统，支持时间索引查询。
- **置信度评分**：每个知识条目带有置信度评分（0-1），支持知识的模糊匹配和排序。
- **标签分类**：支持为知识添加标签，便于分类和检索。

---

## 数据目录结构

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # 主理人的灵魂文件
    │   ├── state.json       # 运行时状态
    │   ├── code.enc         # AES 加密的自定义类代码
    │   └── permission.enc   # AES 加密的自定义权限回调
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```
