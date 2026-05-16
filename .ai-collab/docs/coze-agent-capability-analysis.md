# 系统能力分析报告（coze-agent）

> 生成时间: 2026-05-17 | 分析者: coze-agent | 对照源码验证 + 需求文档偏差分析

---

## 一、qoder 报告验证结论

### 验证通过的部分

**1. 群聊决策逻辑缺失 -- 确认**

`ContextManager.cs:912` 定义了 `BuildGroupChatScenarioContext()`，仅在 GUIDELINES 文本中写入 "Only respond when addressed or when you have valuable input"，代码层面零判定。搜索全项目，`ShouldReplyInGroupChat` 不存在，`MentionedIds` 字段不存在。群聊是否回复完全依赖 AI 自行判断，没有代码级控制。

**2. 项目创建时没有自动创建工作流实例 -- 确认**

`ProjectManager.cs:52-130`（Default 实现）中，`CreateProject()` 方法自动创建了：
- 群聊会话（第 88-93 行）
- 广播频道（第 96-111 行）
- 工作笔记系统（第 113-118 行）

但唯独没有调用 `_workflowEngine.CreateInstanceAsync()`，尽管 `_workflowEngine` 字段已在第 32 行声明。Fast 实现同样如此（`ProjectManager.cs:53` 起）。项目存储了 `WorkflowTemplateName`，但从未用其创建实例。

**3. ProjectView 只有列表没有创建表单 -- 确认**

`ProjectView.cs` 共 203 行，只有 `loadProjects` 的 fetch + 渲染逻辑，没有创建按钮、没有表单、没有模板选择。API 层 `/api/projects/create` 存在（`ProjectController.cs:154`），但前端没有入口触发它。

**4. 广播"下课铃"模型 -- 准确**

广播系统代码确实只接收不回复，return 后继续下一轮 Tick。这是设计意图，不是缺陷。

**5. Tick 驱动模型的一致性分析 -- 准确**

全局 Tick 模型保证了单线程顺序执行、可预测性和容错性，是合理的架构选择。

### qoder 报告有偏差的部分

**偏差 1：IAIClient 缺少 ToolCallSupport -- 严重程度过高**

qoder 报告将此标记为 P1。实际上 ToolCall 已经实现了：`ContextManager.cs:618` 检查 `response.HasToolCalls`，第 537 行定义 `ExecuteToolCalls()`，第 621 行调用 `PersistAndDeliverToolCallRound()`。`ToolScenarioFlag` 枚举控制不同场景的工具可用性。

真正的问题是 `IAIClient` 接口（`IAIClient.cs:19`）没有声明 `SupportsToolCall` 属性。需求文档 7.3.2 节明确要求：
> "提供布尔型属性(`bool`)标记模型ToolCall支持：true=模型支持ToolCall，false=模型不支持ToolCall"

当前接口只有 `StreamingMode` 属性（第 39 行，`bool?`），没有 ToolCall 能力声明。如果某个 AI 后端不支持 function calling，没有优雅降级路径。应从 P1 降为 **P2**。这不是功能缺失，是接口契约不完整。

**偏差 2：报告完全没提到 ThinkOnProject 已移除但 Tick 优先级链仍残留引用**

`DefaultSiliconBeing.cs:329` 注释写 "ThinkOnProject has been removed to simplify the Tick scheduling logic"。`ContextManager.cs:1160` 和 `ContextManager.cs:1346` 同样有移除注释。但 `SiliconBeingBase.cs:41` 枚举仍保留了 `BeingActivity.ProjectWork`，注释写 "Working on project tasks (corresponds to ThinkOnProject)"。代码清理不彻底。

**偏差 3：ProjectController.CreateIndex() 传给 View 的 ProjectItem 没有包含关键字段**

`ProjectController.cs:92-113`，`Index()` 方法构建 `ProjectItem` 时只映射了 Id、Name、Description、CreatedAt、UpdatedAt、Status（第 101-109 行），没有包含 WorkflowTemplateName、GroupChatSessionId、BroadcastChannelId。而 `GetList()` API（第 115 行起）返回了这三个字段（第 141-143 行）。

这意味着服务端渲染的首页看不到工作流和群聊信息，只有 JS 异步加载的列表才有。虽然 ProjectView.cs 的 JS 渲染逻辑（第 132-144 行）引用了这三个字段，但首页直出的 ViewModel 不含这些字段，首页首次渲染必然缺失。

**偏差 4：没有 `/api/projects/list-workflow-templates` 端点 -- 部分不准确**

qoder 报告建议前端调这个 API，但 HTTP 路由层不存在此端点。不过 `ProjectTool.cs:143` 中 Tool 层有 `"list-workflow-templates" => ExecuteListWorkflowTemplates()` 的处理，第 374 行定义了 `ExecuteListWorkflowTemplates()` 方法。所以这个能力通过 AI Tool 调用可用，但 Web API 层没暴露。

**偏差 5：报告视角局限于"能力完整性"，缺少对照原始需求文档的偏差分析**

qoder 报告只做了代码级的能力完整性分析，没有对照需求文档逐项校验实现偏差，也没有区分"合理偏差"和"问题偏差"。本报告第二节补充此分析。

---

## 二、对照原始需求文档的偏差分析

原始需求文档共 2086 行、23 章，以下按严重程度分级。

### P0 级断点（场景断裂，alpha-0.2 必须修）

**P0-1：群聊无决策逻辑**

- 需求（8.1.3 节）：群聊通过聊天工具显式创建，应有成员管理、消息类型、已读机制。
- 需求（8.3 节）：聊天工具应提供 `create_group`、`send_message`、`terminate_chat` 等操作。
- 代码现状：`BuildGroupChatScenarioContext()` 只写 GUIDELINES 文本，无代码判定。没有 `MentionedIds` 字段，没有 `ShouldReplyInGroupChat()` 方法。
- 影响：群聊场景无法正常运转。每条消息都回或漏回，完全依赖 AI 自行判断，不可控。

**P0-2：项目到工作流链路断裂**

- 需求（12.2 节）：项目空间用于储存硅基人的过程数据和结果数据，项目可配置工作流。
- 代码现状：`ProjectManager.CreateProject()` 存储了 `WorkflowTemplateName`，声明了 `_workflowEngine` 字段（`ProjectManager.cs:32`），但从未调用 `CreateInstanceAsync()`。WorkflowEngine 完整实现了 375 行（`WorkflowEngine.cs`），WorkflowTemplate、WorkflowInstance、WorkflowTickObject、Transition、CodeReviewWorkflow 均已实现，但启动链路断裂，这些能力等于白写。
- 影响：工作流引擎完整但无法被项目系统激活。

**P0-3：群聊触发链路缺失**

- 需求（8.1.3 节）：群聊只能通过聊天工具显式创建。
- 代码现状：群聊只在项目创建时自动创建（`ProjectManager.cs:88-93`），没有其他触发时机。没有 Web UI 创建群聊的入口，没有 API 端点直接创建群聊（只有项目创建的副作用）。
- 影响：用户无法独立发起群聊，群聊只能作为项目的附属品存在。

### P1 级断点（功能不完整，影响基本使用）

**P1-1：项目列表页缺创建入口**

- 需求（12.2 节）：项目空间必须由硅基主理人发起创建，提供预定义的项目空间模板。
- 代码现状：`ProjectView.cs` 203 行，只有列表渲染，没有创建按钮、没有表单、没有模板选择。`ProjectController.cs:154` 有 `CreateProject()` 的 API 端点，但前端无入口。
- 影响：用户无法通过 UI 触发任何项目流程。只能通过 AI Tool 调用或直接 API 请求创建。

**P1-2：服务端渲染缺关键字段**

- 代码现状：`ProjectController.cs:101-109` 的 `Index()` 方法映射 ProjectItem 时缺少 WorkflowTemplateName、GroupChatSessionId、BroadcastChannelId。`GetList()` API（第 141-143 行）包含了这些字段。
- 影响：首页直出的项目卡片看不到工作流名称、群聊和广播信息。JS 异步加载的列表才有。

**P1-3：历法系统实现度不足**

- 需求（9.1 节）：实现尽可能多的世界历法（已收集 25 种算法）、基于历法的智能定时需求、基于历法计算节假日和特殊日期。
- 代码现状：
  - 历法转换：30+ 种历法实现（Calendar 目录下 32 个 .cs 文件，共 5620 行），覆盖中国农历、伊斯兰历、希伯来历、波斯历、日本历、藏历等，远超需求 25 种。这是强项。
  - 智能定时：`CalendarTimerResolvers.cs`（176 行）实现了历法条件的定时器解析，支持真实历法和虚拟 interval 日历。已实现。
  - 节假日计算：搜索全项目，没有 Holiday/Festival 相关实现。需求要求"基于历法计算节假日和特殊日期"，此部分完全缺失。
- 实现度评估：约 **65%**。历法转换和定时解析扎实，但节假日计算完全缺失，且历法间的双向转换覆盖率未验证。

**P1-4：知识网络实现度不足**

- 需求（14.1 节）：三元组存储、检索算法（精确匹配、路径查询、聚合查询、图遍历）、交叉验证、置信度评估、自动发现关联关系、推理推导、语义搜索、版本管理。
- 代码现状：
  - 已实现：`KnowledgeGraph.cs`（266 行）采用邻接表结构，有 SubjectIndex/ObjectIndex/PredicateIndex 三个索引，支持精确匹配查询和统计。`KnowledgeTriple.cs` 有 Confidence 属性（0.0-1.0）。`KnowledgeEntry.cs` 有 Version 属性。`KnowledgeTool.cs` 提供增删改查操作。`KnowledgeNetwork.cs` 有 Default 和 Fast 两个实现。
  - 未实现：路径查询（A 与 B 之间的关系路径）、BFS/DFS 图遍历、环检测、最短路径算法、交叉验证、自动发现关联关系、推理推导、语义搜索、相似性搜索。
- 实现度评估：约 **50%**。存储和基础 CRUD 完整，高级查询和推理能力缺失。但需求文档 14.1.2 节标注"可选功能，AI 调用概率较低"，且 22.6 节将其归入"第六阶段:增强功能(可选实现)"，所以严格来说这不是 P1。考虑到对 alpha-0.2 的影响，降为 **P2**。

**P1-5：IAIClient 缺少 ToolCall 能力声明**

- 需求（7.3.2 节）：提供布尔型属性标记模型 ToolCall 支持，对不支持 ToolCall 的模型提供替代交互模式。
- 代码现状：`IAIClient.cs` 有 `StreamingMode` 属性（`bool?`），但没有 `SupportsToolCall` 属性。ToolCall 的执行逻辑在 ContextManager 中完整实现，但接口层没有声明能力。
- 影响：当前只有 Ollama 一个后端实现，没有问题。多后端时会炸。P2 级。

### P2 级断点（设计缺陷，不影响 alpha-0.2 但应记录）

**P2-1：Tick 链残留引用**

- `SiliconBeingBase.cs:41` 枚举保留了 `BeingActivity.ProjectWork`，注释写 "corresponds to ThinkOnProject"，但 ThinkOnProject 已在 `DefaultSiliconBeing.cs:329` 明确移除。
- 不影响运行，但代码清理不彻底。新开发者会困惑。

**P2-2：工作流和 TaskSystem/ProjectTaskSystem 存在功能重叠**

- WorkflowEngine（`WorkflowEngine.cs`，375 行）提供了状态机式的工作流管理。
- TaskCenter（任务系统）提供了任务队列管理。
- ProjectTaskSystem（项目任务系统）提供了项目维度的任务管理。
- 三者的边界不清晰。工作流引擎有 Transition/State，任务系统有 Status/Priority，项目任务有 Assignment。需求文档中没有工作流引擎的描述（属于超纲实现），但任务系统有。

**P2-3：知识网络高级查询缺失**

- 如上 P1-4 分析，路径查询、图遍历、推理推导等未实现。需求标注为可选，降为 P2。

**P2-4：默认权限策略网络维度不完整**

- 需求（19A.2 节）：域名白名单应覆盖搜索引擎、AI服务、开发者平台、社交媒体、百科地图、政府网站、证券资讯等。
- 代码现状（`DefaultPermissionCallback.cs:75-356`）：白名单覆盖面广（Google、Bing、Bilibili、GitHub、Wikipedia 等），黑名单有 AI 仿冒和恶意工具，证券交易类域名标记为 AskUser。但 172.16.0.0/12 B 类私有地址段返回 AskUser 而非 Allowed，与需求"默认开放 192.168.* 网段"的意图不完全一致（需求只提到 C 类，代码也覆盖了 A 类 10.0.0.0/8，但 B 类处理不一致）。
- 整体实现度约 **70%**，主要缺失在于动态更新黑白名单（需求 11.1 节要求"支持运行时动态更新黑白名单配置"）。

### 合理偏差（与需求不同但更合理的实现）

| 偏差点 | 需求 | 实现 | 判断 |
|--------|------|------|------|
| 项目自动创建群聊 | 需求未明确要求 | `ProjectManager.cs:88-93` 自动创建 | 合理。项目协作需要群聊，自动创建减少配置负担 |
| 工作流引擎 | 需求未涉及 | `WorkflowEngine.cs` 375 行完整实现 | 合理。超纲但补充了项目管理的流程控制能力，只是启动链路断裂 |
| Playwright 替代 WebView2 | 需求 13.1.2 节指定 WebView2 | `PlaywrightWebView.cs` 使用 Playwright | 合理。跨平台兼容性更好，需求也提到"IWebViewCore 抽象接口"隔离实现 |
| PBKDF2 替代直接 GUID 密钥 | 需求 3.5 节"密码为全大写的硅基人GUID" | `CodeEncryption.cs:21-107` 使用 PBKDF2 + AES-256-CBC | 合理。安全性大幅提升，直接用 GUID 做密钥在密码学上不安全 |
| SSE 替代 WebSocket | 需求 21A.4 节指定 WebSocket | `WebUIProvider.cs` 使用 SSE | 部分合理。SSE 实现更简单，单向推送够用。但群聊场景可能需要双向通信 |

### 问题偏差（与需求不同且不合理的实现）

**偏差 1：SSE 替代 WebSocket 在群聊场景可能有问题**

- 需求 21A.4 节明确要求 WebSocket 支持"聊天消息的实时推送"和"权限询问卡片的实时弹出"。
- 当前实现使用 SSE（`WebUIProvider.cs:49`），SSE 是单向的（服务端到客户端），客户端发送消息仍需 HTTP POST。
- 对私聊场景影响不大，但群聊多端实时同步可能受影响。需求文档提到"支持聊天消息的实时推送"，SSE 能做到服务端推送，但无法做到客户端低延迟发送。如果群聊消息量不大，SSE 够用。

**偏差 2：代码加密密钥派生方式变更**

- 需求 3.5 节："加密方式: 常用对称加密（如AES），密码为全大写的硅基人GUID"。
- 实际实现（`CodeEncryption.cs:44`）："Key is derived from the being's GUID using PBKDF2"。
- 安全性提升但与需求规格不符。需求文档后续版本应更新此描述。

---

## 三、各模块实现度评估

| 模块 | 实现度 | 说明 |
|------|--------|------|
| 聊天系统 | 90% | 单聊完整（懒加载+唯一性保证），群聊缺决策逻辑和独立创建入口 |
| 任务系统 | 85% | TaskCenter 核心完整，ProjectTaskSystem 独立实现，与 WorkflowEngine 边界模糊 |
| 记忆系统 | 80% | 压缩、衰减、分层完整，淡忘机制刚实现，共享记忆未实现 |
| 工作流引擎 | 85% | 框架完整（WorkflowEngine/Template/Instance/Transition/TickObject/Plugin/CodeReviewWorkflow），但项目启动链路断裂，无法被项目系统激活 |
| 项目系统 | 70% | 后端完整（CRUD + 群聊/广播/工作笔记自动创建），UI 缺创建入口，服务端渲染缺关键字段 |
| 广播系统 | 95% | 完整且符合"下课铃"设计意图 |
| 定时系统 | 90% | 完整，含历法条件解析（CalendarTimerResolvers） |
| 历法系统 | 65% | 30+ 历法实现（超过需求 25 种），定时解析完整，但节假日计算完全缺失 |
| 知识网络 | 50% | 三元组存储+邻接表索引+CRUD完整，Confidence/Version已实现；路径查询、图遍历、推理、语义搜索均缺失 |
| 安全系统 | 70% | 默认权限策略覆盖网络/命令行/文件系统，PBKDF2加密，编译时引用控制；缺动态更新黑白名单、ACL管理UI |
| 动态编译 | 85% | 安全引用控制完整，插件隔离（AssemblyLoadContext）完整，代码加密完整；安全扫描第二层（静态模式分析）实现情况待验证 |
| AI客户端 | 75% | Ollama实现完整，StreamingMode属性已实现，但缺 SupportsToolCall 声明和降级路径 |
| Web前端 | 60% | HtmlBuilder/CssBuilder/JsBuilder 服务端生成体系完整，但各控制器页面覆盖率低，缺项目创建表单、群聊管理页面 |

---

## 四、alpha-0.2 最小验证场景

从"一个人坐下来做一件事"出发，验证系统能跑起来的最小闭环：

**必须通过的 5 步：**

1. 打开浏览器，看到首页 -- 首页渲染 + 导航
2. 看到一个硅基人在线 -- 在线状态显示（SiliconManager 列表）
3. 和它说一句话，它回一句话 -- 私聊链路（ChatSystem + ContextManager + IAIClient）
4. 它开始干活，能看到进度 -- 任务执行 + 状态可见（TaskCenter + Tick 调度）
5. 它干完告诉你 -- 通知链路（广播或私聊回推）

**关键链路验证点：**

- 第 3 步依赖：ChatSystem 消息持久化 + ContextManager 上下文拼接 + IAIClient 请求发送 + ToolCall 执行循环。当前代码完整。
- 第 4 步依赖：TaskCenter 任务枚举 + DefaultSiliconBeing.Tick 调度 + ContextManager 分步调用。当前代码完整。
- 第 5 步依赖：任务完成后 AI 主动发消息或广播通知。需要验证任务完成后的消息推送是否正常。

**第二步的事（alpha-0.3 及以后）：**

- 群聊决策逻辑（P0-1）
- 项目到工作流链路打通（P0-2）
- 群聊独立创建入口（P0-3）
- 项目创建 UI（P1-1）

---

## 五、与 qoder 报告的差异点

| 序号 | 差异点 | qoder 报告 | 本报告 | 原因 |
|------|--------|-----------|--------|------|
| 1 | IAIClient ToolCall 严重程度 | P1（功能缺失） | P2（接口契约不完整） | ToolCall 执行逻辑完整实现，问题仅在于接口层未声明能力 |
| 2 | 历法系统实现度 | 未单独评估 | 65% | 30+ 历法实现是强项，节假日计算是缺口 |
| 3 | 知识网络实现度 | 未单独评估 | 50% | 基础 CRUD 完整，高级查询缺失；但需求标注为可选 |
| 4 | ThinkOnProject 残留引用 | 未提及 | P2 级记录 | 枚举残留导致代码清理不彻底 |
| 5 | CreateIndex 缺关键字段 | 未提及 | P1 级 | 首页直出 vs JS 异步加载的数据不一致 |
| 6 | list-workflow-templates 端点 | 报告说代码不存在 | Tool 层存在，HTTP 路由层不存在 | `ProjectTool.cs:143` 有实现，但 Web API 未暴露 |
| 7 | 需求文档偏差分析 | 缺失 | 完整 | qoder 报告只做代码级分析，未对照需求文档 |
| 8 | SSE vs WebSocket | 未提及 | 部分问题偏差 | 需求要求 WebSocket，实现用 SSE，群聊场景可能有影响 |
| 9 | PBKDF2 vs GUID 密钥 | 未提及 | 合理偏差 | 安全性提升但与需求规格不符 |
| 10 | 安全系统实现度 | 未单独评估 | 70% | 默认权限策略覆盖面广，缺动态更新和管理 UI |

---

## 附录：关键代码路径索引

| 文件 | 行号 | 说明 |
|------|------|------|
| `ContextManager.cs` | 912 | `BuildGroupChatScenarioContext()` 定义 |
| `ContextManager.cs` | 618 | `HasToolCalls` 检查 |
| `ContextManager.cs` | 537 | `ExecuteToolCalls()` 定义 |
| `IAIClient.cs` | 19 | 接口定义，缺 SupportsToolCall |
| `IAIClient.cs` | 39 | StreamingMode 属性 |
| `DefaultSiliconBeing.cs` | 329 | ThinkOnProject 移除注释 |
| `SiliconBeingBase.cs` | 41 | BeingActivity.ProjectWork 残留 |
| `ProjectManager.cs` (Default) | 52-130 | CreateProject 方法，缺工作流实例创建 |
| `ProjectManager.cs` (Default) | 32 | _workflowEngine 字段声明 |
| `ProjectController.cs` | 92-113 | Index() 缺 WorkflowTemplate 等字段映射 |
| `ProjectController.cs` | 141-143 | GetList() 包含完整字段 |
| `ProjectView.cs` | 全文 203 行 | 只有列表渲染，无创建表单 |
| `ProjectTool.cs` | 143 | list-workflow-templates Tool 层实现 |
| `DefaultPermissionCallback.cs` | 75-356 | 网络权限评估，覆盖白/黑/AskUser |
| `CodeEncryption.cs` | 44 | PBKDF2 密钥派生（替代直接 GUID） |
| `PlaywrightWebView.cs` | 22-25 | Playwright 替代 WebView2 |
| `WebUIProvider.cs` | 45-79 | SSE 替代 WebSocket |
| `KnowledgeGraph.cs` | 全文 266 行 | 邻接表存储+三索引 |
| `CalendarTool.cs` | 27 | 历法工具入口 |
| `Calendar/` 目录 | 32 文件 5620 行 | 30+ 历法实现 |
