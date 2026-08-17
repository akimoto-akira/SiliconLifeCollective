# 工具参考

> **版本：v0.2.0-alpha**

本文档详细介绍 Silicon Life Collective 平台的所有内置工具。

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | **中文** | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md) | [Русский](../ru-RU/tools-reference.md)

## 概述

工具系统允许硅基生命体通过标准化接口与外部世界交互。每个工具实现 `ITool` 接口，由 `ToolManager` 通过反射自动发现和注册。

### 工具分类

- **系统管理工具** — 配置、权限、动态编译、主理人管理
- **通信工具** — 聊天、网络请求
- **数据存储工具** — 磁盘操作、数据库、记忆、工作笔记
- **时间管理工具** — 日历、定时器、任务
- **开发工具** — 代码执行、日志查询
- **实用工具** — 系统信息、Token 审计、帮助文档、知识网络
- **浏览器工具** — WebView 浏览器自动化
- **项目工具** — 项目管理、项目任务、项目工作笔记、项目工作
- **扩展工具** — MCP 外部服务器工具、技能管理
- **插件工具** — 通过插件系统注册的第三方工具

### 工具场景系统

每个工具通过 `[ToolScenario]` 属性声明其可用场景：

| 场景标志 | 值 | 描述 |
|----------|------|-------------|
| `Chat` | `1 << 0` | 聊天场景（用户与硅基生命体对话时） |
| `Task` | `1 << 1` | 任务场景（硅基生命体执行任务时） |
| `Timer` | `1 << 2` | 定时器场景（硅基生命体执行定时任务时） |
| `MemoryCompression` | `1 << 3` | 记忆压缩场景 |
| `Project` | `1 << 4` | 项目场景（ThinkOnProject 模式） |
| `All` | 上述所有 | 所有场景均可用 |

此外，`[ChatOnly]` 属性标记的工具仅在聊天场景可用（如 HelpTool），不会出现在任务和定时器场景中。

---

## 内置工具列表

### 1. 日历工具 (CalendarTool)

**工具名称**: `calendar`

**功能描述**: 支持 32 种日历系统的日期转换和计算。

**支持的操作**:
- `now` — 获取当前时间
- `format` — 格式化日期
- `add_days` — 日期加减
- `diff` — 计算日期差
- `list_calendars` — 列出所有支持的日历
- `get_components` — 获取日期组件
- `get_now_components` — 获取当前时间组件
- `convert` — 日历系统间转换

**支持的日历系统** (32 种):
- 公历 (Gregorian)
- 中国农历 (Chinese Lunar)
- 中国历史历法 (Chinese Historical) — 干支纪年、帝王年号
- 伊斯兰历 (Islamic)
- 希伯来历 (Hebrew)
- 日本历 (Japanese)
- 波斯历 (Persian)
- 玛雅历 (Mayan)
- 佛历 (Buddhist)
- 藏历 (Tibetan)
- 等 24 种其他日历...

**使用示例**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_lunar"
}
```

---

### 2. 聊天工具 (ChatTool)

**工具名称**: `chat`

**可用场景**: Chat（`[ChatOnly]`，仅在聊天场景可用）

**功能描述**: 管理聊天会话和消息发送。

**支持的操作**:
- `send` — 发送消息到指定会话
- `mark_read` — 标记消息为已读

**使用示例**:
```json
{
  "action": "send",
  "channel_id": "session-uuid",
  "content": "你好，让我们协作吧！"
}
```

---

### 3. 配置工具 (ConfigTool)

**工具名称**: `config`

**功能描述**: 读取系统配置信息。

**支持的操作**:
- `get_all` — 获取所有配置项
- `get_group` — 获取指定分组的配置项
- `get_field` — 获取指定配置字段
- `get_enum_values` — 获取枚举类型的可选值（如可用模型、区域等）

**使用示例**:
```json
{
  "action": "get_field",
  "group": "AIClients.Ollama",
  "field": "Model"
}
```

---

### 4. 主理人工具 (CuratorTool) 🔒

**工具名称**: `silicon_manager`

**权限要求**: 仅限硅基主理人使用（`[SiliconManagerOnly]`）

**可用场景**: Chat、Task、Timer

**功能描述**: 硅基主理人专用的系统管理工具，用于管理硅基生命体的创建、查看和重置。

**支持的操作**:
- `list_beings` — 列出所有硅基生命体及其状态
- `create_being` — 创建新硅基生命体（需要 `name` 和 `soul` 参数）
- `get_code` — 查看硅基生命体的自定义源代码
- `reset` — 将硅基生命体重置为默认实现

**使用示例**:
```json
{
  "action": "create_being",
  "name": "助手",
  "soul": "你是一个有用的助手..."
}
```

---

### 5. 数据库工具 (DatabaseTool)

**工具名称**: `database`

**功能描述**: 结构化数据库查询和操作。

**支持的操作**:
- `query` — 查询数据
- `insert` — 插入数据
- `update` — 更新数据
- `delete` — 删除数据
- `create_table` — 创建表
- `list_tables` — 列出所有表

**使用示例**:
```json
{
  "action": "query",
  "table": "users",
  "conditions": {"status": "active"},
  "limit": 100
}
```

---

### 6. 磁盘工具 (DiskTool)

**工具名称**: `disk`

**功能描述**: 文件系统操作和本地搜索。

**支持的操作**:
- `read` — 读取文件
- `write` — 写入文件
- `list` — 列出目录
- `delete` — 删除文件
- `create_directory` — 创建目录
- `search_files` — 搜索文件
- `search_content` — 搜索文件内容
- `count_lines` — 统计文件行数
- `read_lines` — 读取指定行范围
- `replace_text` — 替换文本
- `clear_file` — 清空文件内容
- `replace_lines` — 替换指定行范围
- `append` — 追加内容到文件

**权限要求**: `FileAccess`

**使用示例**:
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

---

### 7. 动态编译工具 (DynamicCompileTool) 🔒

**工具名称**: `compile`

**功能描述**: 动态编译 C# 代码（用于硅基生命体自我进化）。

**支持的操作**:
- `compile_class` — 编译类
- `compile_callback` — 编译权限回调函数
- `validate_code` — 验证代码安全性

**安全机制**:
- 编译时引用控制（排除危险程序集）
- 运行时静态代码扫描
- AES-256 加密存储

**使用示例**:
```json
{
  "action": "compile_class",
  "code": "public class MyBeing : SiliconBeingBase { ... }"
}
```

---

### 8. 代码执行工具 (ExecuteCodeTool) 🔒

**工具名称**: `execute_code`

**权限要求**: 仅限硅基主理人使用

**功能描述**: 编译并执行 C# 代码片段。

**支持的操作**:
- `run_script` — 执行代码脚本

**使用示例**:
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

---

### 9. 帮助工具 (HelpTool)

**工具名称**: `help`

**可用场景**: Chat（`[ChatOnly]`，仅在聊天场景可用）

**功能描述**: 搜索和获取系统帮助文档内容，允许 AI 查询系统功能使用方法。

**支持的操作**:
- `list` — 列出所有帮助主题 ID
- `search` — 按关键词搜索帮助文档
- `get` — 获取指定 ID 的帮助文档内容

**使用示例**:
```json
{
  "action": "search",
  "keyword": "权限"
}
```

---

### 10. 知识网络工具 (KnowledgeTool)

**工具名称**: `knowledge`

**功能描述**: 知识图谱操作（基于三元组：主体-关系-客体）。

**支持的操作**:
- `add` — 添加知识三元组
- `query` — 查询知识
- `update` — 更新知识
- `delete` — 删除知识
- `search` — 搜索知识
- `get_path` — 获取知识路径
- `validate` — 验证知识
- `stats` — 获取统计信息

**使用示例**:
```json
{
  "action": "add",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95
}
```

---

### 11. MCP 查询工具 (McpTool)

**工具名称**: `mcp`

**功能描述**: 查询 MCP（Model Context Protocol）集成状态——已连接的外部服务器、它们提供的工具以及如何调用。这是只读工具：服务器的添加/删除只能由用户通过 Web UI 完成，AI 无法修改服务器列表。

**支持的操作**:
- `status` — 全局概览（启用状态、服务器数量、工具数量）
- `list_servers` — 列出已配置的服务器（含连接状态和工具数量）
- `list_tools` — 列出可用工具（带 `mcp_{server}_{tool}` 前缀名、描述和参数 schema；可选 `server_id` 过滤单个服务器）

**使用示例**:
```json
{
  "action": "list_tools",
  "server_id": "filesystem",
  "include_schema": true
}
```

**MCP 包装工具**: 每个已连接 MCP 服务器提供的工具会以独立工具形式动态注册到硅基生命体，命名格式为 `mcp_{serverId}_{toolName}`（如 `mcp_filesystem_read_file`）。AI 可以像调用普通工具一样直接按前缀名调用它们，无需通过本查询工具中转。包装工具在权限矩阵中以单一 `execute` 动作呈现，可被逐个禁用。

**场景**: 所有场景（`All`）

---

### 12. 日志工具 (LogTool)

**工具名称**: `log`

**功能描述**: 查询操作历史、工具调用历史和对话历史。

**支持的操作**:
- `query_operations` — 查询操作历史
- `query_tool_calls` — 查询工具调用历史
- `query_conversations` — 查询对话历史
- `export` — 导出日志
- `get_system_info` — 获取系统信息

**使用示例**:
```json
{
  "action": "query_operations",
  "being_id": "being-uuid",
  "start_time": "2026-04-20T00:00:00Z",
  "end_time": "2026-04-26T23:59:59Z"
}
```

---

### 13. 记忆工具 (MemoryTool)

**工具名称**: `memory`

**功能描述**: 管理硅基生命体的长期和短期记忆。

**支持的操作**:
- `add` — 添加记忆
- `recent` — 获取最近的记忆
- `query` — 搜索记忆
- `stats` — 获取记忆统计

**使用示例**:
```json
{
  "action": "read",
  "key": "important_fact",
  "time_range": {
    "start": "2026-04-01",
    "end": "2026-04-26"
  }
}
```

---

### 14. 网络工具 (NetworkTool)

**工具名称**: `network`

**功能描述**: 发起 HTTP/HTTPS 请求。

**支持的操作**:
- `get` — GET 请求
- `post` — POST 请求
- `put` — PUT 请求
- `delete` — DELETE 请求
- `download` — 下载文件
- `upload` — 上传文件

**权限要求**: `network:http`

**使用示例**:
```json
{
  "action": "get",
  "url": "https://api.example.com/data"
}
```

---

### 15. 权限工具 (PermissionTool) 🔒

**工具名称**: `permission`

**权限要求**: 仅限硅基主理人使用

**功能描述**: 管理权限和访问控制列表。

**支持的操作**:
- `query_permission` — 查询权限
- `manage_acl` — 管理全局 ACL
- `get_callback` — 获取权限回调函数
- `set_callback` — 设置权限回调函数

**使用示例**:
```json
{
  "action": "manage_acl",
  "acl_action": "add_rule",
  "permission_type": "filesystem",
  "resource_prefix": "/data/",
  "acl_result": "allow"
}
```

---

### 16. 项目工具 (ProjectTool) 🔒

**工具名称**: `project`

**权限要求**: 仅限硅基主理人使用（`[SiliconManagerOnly]`）

**可用场景**: Chat、Task、Timer

**功能描述**: 管理项目工作区，支持项目生命周期管理、成员分配和角色管理。

**支持的操作**:
- `create` — 创建新项目空间
- `archive` — 归档项目
- `restore` — 恢复已归档的项目
- `destroy` — 销毁项目并清理数据（不可恢复）
- `list` — 列出所有项目
- `get` — 获取项目详情
- `assign` — 将硅基生命体分配到项目
- `remove` — 从项目中移除硅基生命体
- `update` — 更新项目名称/描述
- `list-workflow-templates` — 列出可用的工作流模板
- `assign_role` — 为硅基生命体分配项目角色
- `remove_role` — 移除硅基生命体的项目角色
- `list_roles` — 列出项目的角色分配

**使用示例**:
```json
{
  "action": "create",
  "name": "My Project",
  "description": "项目描述"
}
```

---

### 17. 项目任务工具 (ProjectTaskTool)

**工具名称**: `project_task`

**可用场景**: Chat、Task、Timer

**功能描述**: 管理项目空间内的任务，支持完整的任务生命周期。

**支持的操作**:
- `create` — 创建项目任务
- `list` — 列出项目任务
- `get` — 获取任务详情
- `update` — 更新任务标题/描述/优先级
- `assign` — 为任务分配负责人
- `remove_assignee` — 移除任务负责人
- `start` — 开始任务
- `complete` — 标记任务完成
- `fail` — 标记任务失败
- `cancel` — 取消任务
- `delete` — 删除任务
- `stats` — 获取任务统计

**使用示例**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "完成任务描述",
  "priority": 5
}
```

---

### 18. 项目工作笔记工具 (ProjectWorkNoteTool)

**工具名称**: `project_work_note`

**可用场景**: Chat、Task、Timer

**功能描述**: 管理项目空间内的工作笔记（公开，类似工作本），支持页面式笔记管理。

**支持的操作**:
- `create` — 创建笔记页面（需要 `project_id`、`summary` 和 `content`，可选 `keywords`）
- `read` — 读取笔记页面（需要 `project_id` 和 `page_number` 或 `note_id`）
- `update` — 更新笔记页面（需要 `project_id`、`page_number` 和 `content`，可选 `summary` 和 `keywords`）
- `delete` — 删除笔记页面（需要 `project_id` 和 `page_number` 或 `note_id`）
- `list` — 列出项目的所有笔记页面摘要
- `directory` — 生成笔记目录/概览
- `search` — 按关键词搜索笔记（需要 `project_id` 和 `keyword`，可选 `max_results`）

**使用示例**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "完成用户认证模块",
  "content": "## 实现细节\n\n- 使用 JWT token",
  "keywords": "认证,JWT"
}
```

---

### 19. 项目工作工具 (ProjectWorkTool) 🔒

**工具名称**: `project_work`

**权限要求**: 仅限硅基主理人使用（`[SiliconManagerOnly]`）

**可用场景**: Project（`[ToolScenario(ToolScenarioFlag.Project)]`，仅在项目场景可用）

**功能描述**: 项目工作操作工具，用于主理人在 ThinkOnProject 场景中管理项目工作流。

**支持的操作**:
- `create-task` — 创建项目任务
- `assign-task` — 为任务分配硅基生命体
- `chat` — 发送消息到项目群聊
- `broadcast` — 广播消息到项目频道
- `complete` — 标记项目为已完成
- `status` — 获取项目状态

**使用示例**:
```json
{
  "action": "create-task",
  "project_id": "project-uuid",
  "title": "实现用户认证"
}
```

---

### 20. 技能工具 (SkillTool)

**工具名称**: `skill`

**功能描述**: 管理硅基生命体的技能（可复用的"工具编排 + 提示词模板"能力单元），支持创建、列出、更新、删除、导入导出。缺失的元数据（id、描述、参数 schema 等）会由 AI 自动补全。

**支持的操作**:
- `create` — 创建新技能（需要 `id` 和 `system_prompt`，可选 `description`、`parameter_schema`、`tool_whitelist`、`tags`、`max_tool_round`、`timeout`、`on_complete`、`trigger_mode`、`auto_trigger_condition`）
- `list` — 列出所有可用技能（含摘要）
- `update` — 通过参数更新已有技能（需要 `skill_id`）
- `update_from_md` — 从 Markdown 字符串更新技能（YAML 前置元数据 + 提示词正文）
- `delete` — 删除技能（需要 `skill_id`）
- `export` — 导出技能为 JSON（需要 `skill_id`）
- `export_md` — 导出技能为 Markdown（需要 `skill_id`）
- `import` — 从 JSON 导入技能（需要 `json`）
- `import_md` — 从 Markdown 导入技能（需要 `markdown`）

**使用示例**:
```json
{
  "action": "create",
  "id": "daily_news_digest",
  "description": "搜索今日科技新闻并生成摘要",
  "system_prompt": "请使用 network 工具搜索 {topic} 的最新新闻，并生成一份 500 字摘要。",
  "parameter_schema": {
    "type": "object",
    "properties": {
      "topic": { "type": "string", "description": "新闻主题" }
    },
    "required": ["topic"]
  },
  "tool_whitelist": ["network", "work_note"],
  "trigger_mode": "Auto",
  "auto_trigger_condition": "schedule",
  "metadata": { "schedule": "0 9 * * *" }
}
```

**修改权限**: 硅基主理人可修改所有技能；普通生命体只能修改来源为 `Being` 或 `User` 的技能（不能修改内置与插件技能）。

**数量限制**: 每个生命体的自定义技能数受配置 `MaxCustomSkillsPerBeing`（默认 50）限制。

**场景**: 所有场景（`All`）

> 关于技能系统（触发模式、白名单、热重载、自动调度等）的完整说明，参见 [硅基生命体指南](silicon-being-guide.md#技能系统)。

---

### 21. 系统工具 (SystemTool)

**工具名称**: `system`

**功能描述**: 获取系统信息和资源使用情况。

**支持的操作**:
- `info` — 获取系统信息
- `resource_usage` — 获取资源使用情况
- `find_process` — 查找进程
- `list_beings` — 列出硅基生命体

**使用示例**:
```json
{
  "action": "info"
}
```

---

### 22. 任务工具 (TaskTool)

**工具名称**: `task`

**功能描述**: 管理硅基生命体个人任务。

**支持的操作**:
- `create` — 创建任务
- `list` — 列出任务
- `update` — 更新任务
- `complete` — 完成任务
- `delete` — 删除任务
- `get_dependencies` — 获取依赖关系

**使用示例**:
```json
{
  "action": "create",
  "description": "审查代码",
  "priority": 5
}
```

---

### 23. 定时器工具 (TimerTool)

**工具名称**: `timer`

**功能描述**: 创建和管理定时器。

**支持的操作**:
- `create` — 创建定时器
- `list` — 列出定时器
- `delete` — 删除定时器
- `pause` — 暂停定时器
- `resume` — 恢复定时器
- `get_execution_history` — 获取执行历史

**使用示例**:
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true,
  "message": "每小时提醒"
}
```

---

### 24. Token 审计工具 (TokenAuditTool) 🔒

**工具名称**: `token_audit`

**权限要求**: 仅限硅基主理人使用（`[SiliconManagerOnly]`）

**可用场景**: Chat、Task、Timer

**功能描述**: 查询 AI Token 使用统计和趋势数据。

**支持的操作**:
- `summary` — 获取 Token 使用汇总统计
- `trend` — 获取 Token 使用趋势数据点

**支持的时间范围**:
- `today` — 最近 24 小时
- `week` — 最近 7×24 小时
- `month` — 按天统计
- `year` — 按月统计

**使用示例**:
```json
{
  "action": "summary",
  "time_range": "week"
}
```

---

### 25. WebView 浏览器工具 (WebViewBrowserTool)

**工具名称**: `webview_browser`

**可用场景**: Chat、Task、Timer

**功能描述**: 基于 Playwright 的浏览器自动化操作，提供完整的网页导航、交互和数据提取能力。

**支持的操作**:
- `open` — 打开浏览器
- `close` — 关闭浏览器
- `navigate` — 导航到 URL
- `click` — 点击元素
- `input` — 输入文本
- `scroll` — 滚动页面
- `execute_script` — 执行 JavaScript
- `get_page_text` — 获取页面文本
- `get_screenshot` — 获取截图
- `wait_for_element` — 等待元素出现
- `get_element_info` — 获取元素信息
- `upload_file` — 上传文件
- `get_browser_status` — 获取浏览器状态
- `set_timeout` — 设置超时时间
- `clear_session` — 清除浏览器会话

**特性**:
- 每个硅基生命体独立实例
- 完全隔离的 Cookie 和会话
- 用户完全不可见（无头模式）
- 完整 JavaScript 和 CSS 支持

**使用示例**:
```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

---

### 26. 工作笔记工具 (WorkNoteTool)

**工具名称**: `work_note`

**功能描述**: 管理硅基生命体个人工作笔记（私有，类似日记本）。

**支持的操作**:
- `create` — 创建笔记
- `read` — 读取笔记
- `update` — 更新笔记
- `delete` — 删除笔记
- `list` — 列出笔记
- `search` — 搜索笔记
- `directory` — 生成目录

**使用示例**:
```json
{
  "action": "create",
  "summary": "完成用户认证模块",
  "content": "## 实现细节\n\n- 使用 JWT token\n- 支持 OAuth2",
  "keywords": "认证,JWT,OAuth2"
}
```

---

## 工具调用流程

```
┌──────────┐
│   AI     │ 返回 tool_calls
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ 查找和验证工具使用权
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ 检查权限链
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ 执行资源访问操作
└────┬─────────┘
     ↓
┌──────────┐
│   AI     │ 接收工具结果，继续思考
└──────────┘
```

## 权限验证

所有工具执行都通过权限验证链：

1. **UserFrequencyCache** — 高频用户决策缓存（HighDeny 优先于 HighAllow）
2. **IPermissionCallback** — 自定义权限回调函数（Allowed/Denied/AskUser）
3. **IsCurator 分支** — 主理人通过 IPermissionAskHandler 询问用户；非主理人查询 GlobalACL，无匹配规则则默认拒绝

## 创建自定义工具

### 步骤 1: 实现 ITool 接口

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "工具描述";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, object>
        {
            ["param1"] = new { type = "string", description = "参数说明" }
        }
    };
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        try
        {
            var param1 = call.Parameters["param1"]?.ToString();
            var result = await DoWork(param1);
            
            return new ToolResult
            {
                Success = true,
                Output = result
            };
        }
        catch (Exception ex)
        {
            return new ToolResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }
}
```

### 步骤 2: 添加到项目

将工具文件放置在 `src/SiliconLife.Common/Tools/` 目录中（共享工具）或 `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` 目录中（版本特定工具）。`ToolManager` 会在启动时通过反射自动发现并注册。

### 步骤 2a: 通过插件注册工具

也可以通过插件系统注册自定义工具：

1. 在插件项目中实现 `ITool` 接口
2. 编译插件 DLL 并放入插件目录
3. `ToolManager.ScanAllPluginAssemblies()` 会自动扫描所有已加载插件中的 ITool 实现
4. 插件工具受相同的权限系统约束

### 步骤 3: （可选）标记为主理人专用

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // 仅硅基主理人可访问
}
```

### 替代方案：技能与 MCP 工具

除编写 C# 工具类外，还有两种无需编译的扩展方式：

- **技能（Skill）**：通过 Web UI 或 `skill` 工具创建"工具编排 + 提示词模板"组合，适合把常用工作流封装为可复用能力。参见 [硅基生命体指南 — 技能系统](silicon-being-guide.md#技能系统)。
- **MCP 服务器**：在 Web UI 配置外部 MCP 服务器后，其工具自动以 `mcp_{serverId}_{toolName}` 形式注入，无需编写任何代码。参见 [Web UI 指南 — MCP 管理](web-ui-guide.md)。

## 最佳实践

### 1. 始终验证参数

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("缺少必需参数: required_param");
}
```

### 2. 优雅处理错误

```csharp
try
{
    // 执行操作
}
catch (Exception ex)
{
    Logger.Error($"工具 {Name} 执行失败: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. 尊重权限系统

永远不要绕过权限检查。始终通过执行器访问资源：

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return ToolResult.Denied("Permission denied");
}
```

### 4. 提供清晰的工具描述

帮助 AI 理解何时以及如何使用工具：

```csharp
public string Description => 
    "用于在不同日历系统之间转换日期。" +
    "需要提供 'date'、'from_calendar' 和 'to_calendar' 参数。";
```

## 故障排除

### 工具未找到

**问题**: AI 尝试调用不存在的工具。

**解决方案**:
- 检查工具名称是否完全匹配
- 验证工具文件在 `Tools/` 目录中
- 重新构建项目 (`dotnet build`)

### 权限被拒绝

**问题**: 工具执行失败，返回权限错误。

**解决方案**:
- 检查权限审计日志
- 验证硅基生命体具有所需权限
- 查看全局 ACL 设置
- 如果是主理人，检查是否使用了 `[SiliconManagerOnly]` 标记

### 工具执行返回错误

**问题**: 工具执行但返回失败结果。

**解决方案**:
- 检查工具返回的错误消息
- 验证输入参数格式正确
- 查看系统日志获取详细错误信息
- 独立测试工具功能

## 下一步

- 📚 阅读[架构指南](architecture.md)
- 🛠️ 查看[开发指南](development-guide.md)
- 🔒 了解[权限系统](permission-system.md)
- 🚀 查看[快速开始指南](getting-started.md)
