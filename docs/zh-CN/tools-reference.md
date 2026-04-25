# 工具参考

[English](tools-reference.md) | [中文](docs/zh-CN/tools-reference.md) | [繁體中文](docs/zh-HK/tools-reference.md) | [Español](docs/es-ES/tools-reference.md) | [日本語](docs/ja-JP/tools-reference.md) | [한국어](docs/ko-KR/tools-reference.md) | [Čeština](docs/cs-CZ/tools-reference.md)

## 概述

工具系统允许 AI 智能体通过标准化接口与外部世界交互。

## 内置工具

### 1. 日历工具

**名称**：`calendar`

**描述**：在不同日历系统之间转换日期。

**参数**：
```json
{
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**支持的日历**（32 种系统）：
- 公历、农历、伊斯兰历、希伯来历
- 日本历、波斯历、玛雅历、藏历
- 还有 24 种...

### 2. 聊天工具

**名称**：`chat`

**描述**：向其他生命体或用户发送消息。

**参数**：
```json
{
  "targetId": "being-uuid",
  "message": "Hello, let's collaborate"
}
```

### 3. 配置工具

**名称**：`config`

**描述**：读取和修改系统配置。

**参数**：
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

### 4. 磁盘工具

**名称**：`disk`

**描述**：文件系统操作（读取、写入、列表）。

**参数**：
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

**所需权限**：`disk:read`、`disk:write`

### 5. 动态编译工具

**名称**：`compile`

**描述**：动态编译和执行 C# 代码。

**参数**：
```json
{
  "code": "public class Test { ... }",
  "references": ["System.Linq"]
}
```

**安全**：代码在执行前被扫描。

### 6. 记忆工具

**名称**：`memory`

**描述**：存储和检索生命体记忆。

**参数**：
```json
{
  "action": "read",
  "key": "important_fact",
  "timeRange": {
    "start": "2026-04-01",
    "end": "2026-04-20"
  }
}
```

### 7. 网络工具

**名称**：`network`

**描述**：发出 HTTP 请求。

**参数**：
```json
{
  "method": "GET",
  "url": "https://api.example.com/data",
  "headers": {}
}
```

**所需权限**：`network:http`

### 8. 系统工具

**名称**：`system`

**描述**：获取系统信息。

**参数**：
```json
{
  "action": "info"
}
```

### 9. 任务工具

**名称**：`task`

**描述**：管理生命体任务。

**参数**：
```json
{
  "action": "create",
  "description": "Review code",
  "priority": 5
}
```

### 10. 定时器工具

**名称**：`timer`

**描述**：创建和管理定时器。

**参数**：
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true
}
```

### 11. Token 审计工具

**名称**：`token_audit`

**描述**：查询 token 使用统计。

**参数**：
```json
{
  "startDate": "2026-04-01",
  "endDate": "2026-04-20"
}
```

### 12. 权限工具

**名称**：`permission`

**描述**：管理硅基生命体的权限。仅限主理人。

**动作**：`query_permission`、`manage_acl`

**参数**（query_permission）：
```json
{
  "action": "query_permission",
  "being_id": "being-uuid",
  "permission_type": "network",
  "resource": "https://api.example.com"
}
```

**权限类型**：`network`、`command`、`filesystem`、`function`、`data`

**返回**：三态结果（`ALLOWED`、`DENIED`、`ASK_USER`），包含主理人状态和频率缓存信息。

**参数**（manage_acl）：
```json
{
  "action": "manage_acl",
  "acl_action": "add_rule",
  "permission_type": "filesystem",
  "resource_prefix": "/data/",
  "acl_result": "allow",
  "description": "允许访问数据目录"
}
```

**权限**：需要 `IsCurator` 标志。

### 13. 代码执行工具

**名称**：`execute_code`

**描述**：编译并执行带有安全扫描的 C# 代码。仅限主理人。

**动作**：`run_script`

**参数**：
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

**详细说明**：
- 代码被包装在 `ScriptExecutor` 类中的 `Execute()` 方法中
- 编译前进行安全扫描
- 支持可配置的超时时间（默认：30 秒）
- 失败时返回编译错误和安全违规信息

**权限**：需要 `IsCurator` 标志。

### 14. 数据库工具

**名称**：`database`

**描述**：结构化数据库查询和操作。

**动作**：`query`、`insert`、`update`、`delete`

**参数**（query）：
```json
{
  "action": "query",
  "table": "users",
  "conditions": {"status": "active"},
  "limit": 100
}
```

**权限**：需要相应的数据库访问权限。

### 15. 日志工具

**名称**：`log`

**描述**：操作历史和对话历史查询。

**动作**：`query_logs`、`query_conversations`

**参数**（query_logs）：
```json
{
  "action": "query_logs",
  "being_id": "being-uuid",
  "start_time": "2026-04-20T00:00:00Z",
  "end_time": "2026-04-23T23:59:59Z",
  "level": "info"
}
```

**参数**（query_conversations）：
```json
{
  "action": "query_conversations",
  "being_id": "being-uuid",
  "session_id": "session-uuid",
  "limit": 50
}
```

**特性**：
- 支持按硅基生命体过滤日志
- 支持时间范围查询
- 支持日志级别过滤
- 对话历史检索

### 16. 知识网络工具

**名称**：`knowledge`

**描述**：知识网络操作工具，用于添加、查询、更新、删除和搜索知识三元组。

**动作**：`add`、`query`、`update`、`delete`、`search`、`get_path`、`validate`、`stats`

**参数**（add - 添加知识）：
```json
{
  "action": "add",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95,
  "tags": ["programming", "language"]
}
```

**参数**（query - 查询知识）：
```json
{
  "action": "query",
  "subject": "Python",
  "predicate": "is_a"
}
```

**参数**（search - 搜索知识）：
```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

**参数**（get_path - 获取知识路径）：
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

**参数**（stats - 统计信息）：
```json
{
  "action": "stats"
}
```

**特性**：
- 基于三元组结构（主语-谓语-宾语）的知识表示
- 支持知识置信度评分
- 支持标签分类和搜索
- 支持知识路径发现（两点间的关联路径）
- 支持知识验证和完整性检查
- 持久化存储到文件系统

**权限**：所有生命体均可使用。

### 17. 工作笔记工具

**名称**：`work_note`

**描述**：管理硅基生命体的工作笔记。工作笔记采用页式设计，类似个人日记（默认私有）。

**动作**：`create`、`read`、`update`、`delete`、`list`、`directory`、`search`

**参数**（create - 创建笔记）：
```json
{
  "action": "create",
  "summary": "完成用户认证模块",
  "content": "## 实现细节\n\n- 使用 JWT token\n- 支持 OAuth2\n- 添加了刷新 token 机制",
  "keywords": "认证,JWT,OAuth2"
}
```

**参数**（read - 读取笔记）：
```json
{
  "action": "read",
  "page_number": 1
}
```

或使用 note_id：
```json
{
  "action": "read",
  "note_id": "550e8400-e29b-41d4-a716-446655440000"
}
```

**参数**（update - 更新笔记）：
```json
{
  "action": "update",
  "page_number": 1,
  "content": "## 更新后的内容\n\n添加了单元测试",
  "summary": "完成用户认证模块及测试"
}
```

**参数**（list - 列出所有笔记）：
```json
{
  "action": "list"
}
```

**参数**（directory - 生成笔记目录）：
```json
{
  "action": "directory"
}
```

**参数**（search - 搜索笔记）：
```json
{
  "action": "search",
  "keyword": "认证",
  "max_results": 10
}
```

**特性**：
- 页式设计，每页独立管理
- 支持摘要、内容、关键词
- 支持按关键词搜索
- 支持生成目录概览（用于上下文理解）
- 内容支持 Markdown 格式（文本、列表、表格、代码块）
- 自动时间戳记录
- 默认私有，仅生命体自身可访问

**权限**：生命体访问自己的工作笔记，主理人可管理所有笔记。

### 磁盘工具增强

磁盘工具现在包含本地搜索功能（从 SearchTool 整合）：

**新增动作**：`search_files`、`search_content`

**参数**（search_files）：
```json
{
  "action": "search_files",
  "path": "/data",
  "pattern": "*.json",
  "recursive": true
}
```

**参数**（search_content）：
```json
{
  "action": "search_content",
  "path": "/data",
  "query": "search term",
  "filePattern": "*.md"
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
│ ToolManager  │ 查找和验证工具
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ 检查权限链
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ 执行操作
└────┬─────────┘
     ↓
┌──────────┐
│   AI     │ 接收工具结果
└──────────┘
```

## 权限验证

所有工具都通过 5 级权限链：

1. **IsCurator**：管理员绕过所有检查
2. **UserFrequencyCache**：每个用户的速率限制
3. **GlobalACL**：访问控制列表
4. **IPermissionCallback**：自定义回调逻辑
5. **IPermissionAskHandler**：询问用户权限

## 创建自定义工具

### 步骤 1：实现 ITool

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "Does something useful";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, object>
        {
            ["param1"] = new { type = "string", description = "Description" }
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

### 步骤 2：添加到项目

将工具放置在 `src/SiliconLife.Default/Tools/` 中。

`ToolManager` 将通过反射自动发现它。

### 步骤 3：（可选）标记为仅管理员可用

```csharp
[SiliconManagerOnly]
public class AdminTool : ITool
{
    // 仅主理人可访问
}
```

## 最佳实践

### 1. 始终验证参数

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("Missing required parameter: required_param");
}
```

### 2. 优雅处理错误

```csharp
try
{
    // 操作
}
catch (Exception ex)
{
    Logger.Error($"Tool {Name} failed: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. 尊重权限

永远不要绕过权限系统。始终使用：

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return ToolResult.Denied(permission.Reason);
}
```

### 4. 提供清晰描述

帮助 AI 理解何时以及如何使用您的工具：

```csharp
public string Description => 
    "Use this tool to convert dates between calendar systems. " +
    "Requires 'date', 'fromCalendar', and 'toCalendar' parameters.";
```

## 测试工具

### 单元测试示例

```csharp
[TestMethod]
public async Task CalendarTool_ConvertDate_ReturnsCorrectResult()
{
    var tool = new CalendarTool();
    var call = new ToolCall
    {
        Name = "calendar",
        Parameters = new Dictionary<string, object>
        {
            ["date"] = "2026-04-20",
            ["fromCalendar"] = "gregorian",
            ["toCalendar"] = "chinese_lunar"
        }
    };
    
    var result = await tool.ExecuteAsync(call);
    
    Assert.IsTrue(result.Success);
    Assert.IsNotNull(result.Output);
}
```

## 故障排除

### 未找到工具

**问题**：AI 尝试调用不存在的工具。

**解决方案**：
- 检查工具名称完全匹配
- 验证工具在 Tools 目录中
- 重新构建项目

### 权限被拒绝

**问题**：工具执行失败，出现权限错误。

**解决方案**：
- 检查权限日志
- 验证用户具有所需权限
- 查看 GlobalACL 设置

### 工具返回错误

**问题**：工具执行但返回失败。

**解决方案**：
- 检查工具日志以获取详细错误
- 验证输入参数
- 独立测试工具

## 下一步

- 📚 阅读[架构指南](architecture.md)
- 🛠️ 查看[开发指南](development-guide.md)
- 🔒 查看[权限系统](permission-system.md)
- 🚀 查看[快速开始指南](getting-started.md)
