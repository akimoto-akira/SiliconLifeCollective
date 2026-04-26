# 工具参考

本文档详细介绍 Silicon Life Collective 平台的所有内置工具。

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | **中文** | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md)

## 概述

工具系统允许硅基生命体通过标准化接口与外部世界交互。每个工具实现 `ITool` 接口，由 `ToolManager` 通过反射自动发现和注册。

### 工具分类

- **系统管理工具** — 配置、权限、动态编译
- **通信工具** — 聊天、网络请求
- **数据存储工具** — 磁盘操作、数据库、记忆、工作笔记
- **时间管理工具** — 日历、定时器、任务
- **开发工具** — 代码执行、日志查询
- **实用工具** — 系统信息、Token 审计、帮助文档、知识网络
- **浏览器工具** — WebView 浏览器自动化

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

**功能描述**: 管理聊天会话和消息发送。

**支持的操作**:
- `send_message` — 发送消息
- `get_messages` — 获取历史消息
- `create_group` — 创建群聊
- `add_member` — 添加群成员
- `remove_member` — 移除群成员
- `get_chat_info` — 获取聊天信息
- `terminate_chat` — 终止聊天（已读不回）

**使用示例**:
```json
{
  "action": "send_message",
  "target_id": "being-uuid-or-user-0",
  "message": "你好，让我们协作吧！"
}
```

---

### 3. 配置工具 (ConfigTool)

**工具名称**: `config`

**功能描述**: 读取和修改系统配置。

**支持的操作**:
- `read` — 读取配置项
- `write` — 写入配置项
- `list` — 列出所有配置
- `get_ai_config` — 获取 AI 客户端配置
- `set_ai_config` — 设置 AI 客户端配置

**使用示例**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. 主理人工具 (CuratorTool) 🔒

**工具名称**: `curator`

**权限要求**: 仅限硅基主理人使用

**功能描述**: 硅基主理人专用的系统管理工具。

**支持的操作**:
- `create_being` — 创建新硅基生命体
- `list_beings` — 列出所有硅基生命体
- `get_being_info` — 获取生命体信息
- `assign_task` — 分配任务
- `manage_permissions` — 管理权限

**使用示例**:
```json
{
  "action": "create_being",
  "name": "助手",
  "soul_file": "assistant_soul.md"
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
- `count_lines` — 统计行数
- `read_lines` — 读取指定行
- `replace_text` — 替换文本

**权限要求**: `disk:read`, `disk:write`

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

**功能描述**: 获取系统帮助文档和使用指南。

**支持的操作**:
- `get_topics` — 获取帮助主题列表
- `get_topic` — 获取特定主题详情
- `search` — 搜索帮助文档

**使用示例**:
```json
{
  "action": "get_topics"
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

### 11. 日志工具 (LogTool)

**工具名称**: `log`

**功能描述**: 查询操作历史和对话历史。

**支持的操作**:
- `query_logs` — 查询系统日志
- `query_conversations` — 查询对话历史
- `get_stats` — 获取日志统计

**使用示例**:
```json
{
  "action": "query_logs",
  "being_id": "being-uuid",
  "start_time": "2026-04-20T00:00:00Z",
  "end_time": "2026-04-26T23:59:59Z",
  "level": "info"
}
```

---

### 12. 记忆工具 (MemoryTool)

**工具名称**: `memory`

**功能描述**: 管理硅基生命体的长期和短期记忆。

**支持的操作**:
- `read` — 读取记忆
- `write` — 写入记忆
- `search` — 搜索记忆
- `delete` — 删除记忆
- `list` — 列出记忆
- `get_stats` — 获取记忆统计
- `compress` — 压缩记忆

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

### 13. 网络工具 (NetworkTool)

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

### 14. 权限工具 (PermissionTool) 🔒

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

### 15. 项目工具 (ProjectTool)

**工具名称**: `project`

**功能描述**: 管理项目工作区。

**支持的操作**:
- `create` — 创建项目
- `list` — 列出项目
- `get_info` — 获取项目信息
- `update` — 更新项目
- `archive` — 归档项目

**使用示例**:
```json
{
  "action": "create",
  "name": "My Project",
  "description": "项目描述"
}
```

---

### 16. 项目任务工具 (ProjectTaskTool)

**工具名称**: `project_task`

**功能描述**: 管理项目任务。

**支持的操作**:
- `create` — 创建任务
- `list` — 列出任务
- `update` — 更新任务
- `complete` — 完成任务
- `get_stats` — 获取任务统计

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

### 17. 项目工作笔记工具 (ProjectWorkNoteTool)

**工具名称**: `project_work_note`

**功能描述**: 管理项目工作笔记（公开，类似工作本）。

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
  "project_id": "project-uuid",
  "summary": "完成用户认证模块",
  "content": "## 实现细节\n\n- 使用 JWT token",
  "keywords": "认证,JWT"
}
```

---

### 18. 系统工具 (SystemTool)

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

### 19. 任务工具 (TaskTool)

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

### 20. 定时器工具 (TimerTool)

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

### 21. Token 审计工具 (TokenAuditTool) 🔒

**工具名称**: `token_audit`

**权限要求**: 仅限硅基主理人使用

**功能描述**: 查询和汇总 AI token 使用情况。

**支持的操作**:
- `get_usage` — 获取 token 使用统计
- `get_by_being` — 按生命体获取使用情况
- `get_by_model` — 按模型获取使用情况
- `get_trend` — 获取使用趋势
- `export` — 导出数据

**使用示例**:
```json
{
  "action": "get_usage",
  "start_date": "2026-04-01",
  "end_date": "2026-04-26"
}
```

---

### 22. WebView 浏览器工具 (WebViewBrowserTool)

**工具名称**: `webview`

**功能描述**: 基于 Playwright 的浏览器自动化操作。

**支持的操作**:
- `open_browser` — 打开浏览器
- `close_browser` — 关闭浏览器
- `navigate` — 导航到 URL
- `click` — 点击元素
- `input` — 输入文本
- `get_page_text` — 获取页面文本
- `get_screenshot` — 获取截图
- `execute_script` — 执行 JavaScript
- `wait_for_element` — 等待元素出现
- `get_browser_status` — 获取浏览器状态

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

### 23. 工作笔记工具 (WorkNoteTool)

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

所有工具执行都通过 5 级权限链：

1. **IsCurator** — 硅基主理人绕过所有检查
2. **UserFrequencyCache** — 用户高频允许/拒绝缓存
3. **GlobalACL** — 全局访问控制列表
4. **IPermissionCallback** — 自定义权限回调函数
5. **IPermissionAskHandler** — 询问用户

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

将工具文件放置在 `src/SiliconLife.Default/Tools/` 目录中。`ToolManager` 会在启动时通过反射自动发现并注册。

### 步骤 3: （可选）标记为主理人专用

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // 仅硅基主理人可访问
}
```

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
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return ToolResult.Denied(permission.Reason);
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
