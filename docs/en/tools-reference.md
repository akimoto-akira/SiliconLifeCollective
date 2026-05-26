# Tools Reference

> **Version: v0.2.0-alpha**

This document provides detailed information about all built-in tools in the Silicon Life Collective platform.

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md) | [Русский](../ru-RU/tools-reference.md)

## Overview

The tool system allows silicon beings to interact with the external world through a standardized interface. Each tool implements the `ITool` interface and is automatically discovered and registered by the `ToolManager` through reflection.

### Tool Categories

- **System Management Tools** — Configuration, permissions, dynamic compilation, curator management
- **Communication Tools** — Chat, network requests
- **Data Storage Tools** — Disk operations, databases, memory, work notes
- **Time Management Tools** — Calendar, timers, tasks
- **Development Tools** — Code execution, log queries
- **Utility Tools** — System information, token audit, help documentation, knowledge network
- **Browser Tools** — WebView browser automation
- **Project Tools** — Project management, project tasks, project work notes, project work
- **Plugin Tools** — Third-party tools registered through the plugin system

### Tool Scenario System

Each tool declares its available scenarios through the `[ToolScenario]` attribute:

| Scenario Flag | Value | Description |
|----------|------|-------------|
| `Chat` | `1 << 0` | Chat scenario (when users converse with silicon beings) |
| `Task` | `1 << 1` | Task scenario (when silicon beings execute tasks) |
| `Timer` | `1 << 2` | Timer scenario (when silicon beings execute scheduled tasks) |
| `MemoryCompression` | `1 << 3` | Memory compression scenario |
| `Project` | `1 << 4` | Project scenario (ThinkOnProject mode) |
| `All` | All of the above | Available in all scenarios |

Additionally, tools marked with the `[ChatOnly]` attribute are only available in the chat scenario (e.g., HelpTool) and will not appear in task and timer scenarios.

---

## Built-in Tools List

### 1. Calendar Tool (CalendarTool)

**Tool Name**: `calendar`

**Description**: Supports date conversion and calculations across 32 calendar systems.

**Supported Operations**:
- `now` — Get current time
- `format` — Format date
- `add_days` — Add/subtract days
- `diff` — Calculate date difference
- `list_calendars` — List all supported calendars
- `get_components` — Get date components
- `get_now_components` — Get current time components
- `convert` — Convert between calendar systems

**Supported Calendar Systems** (32 types):
- Gregorian Calendar
- Chinese Lunar Calendar
- Chinese Historical Calendar — Sexagenary cycle, imperial era names
- Islamic Calendar
- Hebrew Calendar
- Japanese Calendar
- Persian Calendar
- Mayan Calendar
- Buddhist Calendar
- Tibetan Calendar
- And 24 other calendar systems...

**Usage Example**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_lunar"
}
```

---

### 2. Chat Tool (ChatTool)

**Tool Name**: `chat`

**Description**: Manages chat sessions and message sending.

**Supported Operations**:
- `send_message` — Send message
- `get_messages` — Get message history
- `create_group` — Create group chat
- `add_member` — Add group member
- `remove_member` — Remove group member
- `get_chat_info` — Get chat information
- `terminate_chat` — Terminate chat (read without reply)

**Usage Example**:
```json
{
  "action": "send_message",
  "target_id": "being-uuid-or-user-0",
  "message": "你好，让我们协作吧！"
}
```

---

### 3. Config Tool (ConfigTool)

**Tool Name**: `config`

**Description**: Reads and modifies system configuration.

**Supported Operations**:
- `read` — Read configuration item
- `write` — Write configuration item
- `list` — List all configurations
- `get_ai_config` — Get AI client configuration
- `set_ai_config` — Set AI client configuration

**Usage Example**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. Curator Tool (CuratorTool) 🔒

**Tool Name**: `silicon_manager`

**Permission Requirement**: Silicon Curator only (`[SiliconManagerOnly]`)

**Available Scenarios**: Chat, Task, Timer

**Description**: System management tool exclusive to Silicon Curators, used to manage the creation, viewing, and resetting of silicon beings.

**Supported Operations**:
- `list_beings` — List all silicon beings and their status
- `create_being` — Create new silicon being (requires `name` and `soul` parameters)
- `get_code` — View custom source code of a silicon being
- `reset` — Reset a silicon being to default implementation

**Usage Example**:
```json
{
  "action": "create_being",
  "name": "助手",
  "soul": "你是一个有用的助手..."
}
```

---

### 5. Database Tool (DatabaseTool)

**Tool Name**: `database`

**Description**: Structured database queries and operations.

**Supported Operations**:
- `query` — Query data
- `insert` — Insert data
- `update` — Update data
- `delete` — Delete data
- `create_table` — Create table
- `list_tables` — List all tables

**Usage Example**:
```json
{
  "action": "query",
  "table": "users",
  "conditions": {"status": "active"},
  "limit": 100
}
```

---

### 6. Disk Tool (DiskTool)

**Tool Name**: `disk`

**Description**: File system operations and local search.

**Supported Operations**:
- `read` — Read file
- `write` — Write file
- `list` — List directory
- `delete` — Delete file
- `create_directory` — Create directory
- `search_files` — Search files
- `search_content` — Search file content
- `count_lines` — Count lines
- `read_lines` — Read specified lines
- `replace_text` — Replace text

**Permission Requirement**: `FileAccess`

**Usage Example**:
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

---

### 7. Dynamic Compile Tool (DynamicCompileTool) 🔒

**Tool Name**: `compile`

**Description**: Dynamically compiles C# code (for silicon being self-evolution).

**Supported Operations**:
- `compile_class` — Compile class
- `compile_callback` — Compile permission callback function
- `validate_code` — Validate code security

**Security Mechanisms**:
- Compile-time reference control (excludes dangerous assemblies)
- Runtime static code scanning
- AES-256 encrypted storage

**Usage Example**:
```json
{
  "action": "compile_class",
  "code": "public class MyBeing : SiliconBeingBase { ... }"
}
```

---

### 8. Execute Code Tool (ExecuteCodeTool) 🔒

**Tool Name**: `execute_code`

**Permission Requirement**: Silicon Curator only

**Description**: Compiles and executes C# code snippets.

**Supported Operations**:
- `run_script` — Execute code script

**Usage Example**:
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

---

### 9. Help Tool (HelpTool)

**Tool Name**: `help`

**Available Scenarios**: Chat (`[ChatOnly]`, only available in chat scenario)

**Description**: Searches and retrieves system help documentation content, allowing AI to query how to use system features.

**Supported Operations**:
- `list` — List all help topic IDs
- `search` — Search help documentation by keyword
- `get` — Get help documentation content by ID

**Usage Example**:
```json
{
  "action": "search",
  "keyword": "权限"
}
```

---

### 10. Knowledge Network Tool (KnowledgeTool)

**Tool Name**: `knowledge`

**Description**: Knowledge graph operations (based on triples: subject-predicate-object).

**Supported Operations**:
- `add` — Add knowledge triple
- `query` — Query knowledge
- `update` — Update knowledge
- `delete` — Delete knowledge
- `search` — Search knowledge
- `get_path` — Get knowledge path
- `validate` — Validate knowledge
- `stats` — Get statistics

**Usage Example**:
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

### 11. Log Tool (LogTool)

**Tool Name**: `log`

**Description**: Queries operation history and conversation history.

**Supported Operations**:
- `query_logs` — Query system logs
- `query_conversations` — Query conversation history
- `get_stats` — Get log statistics

**Usage Example**:
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

### 12. Memory Tool (MemoryTool)

**Tool Name**: `memory`

**Description**: Manages long-term and short-term memory for silicon beings.

**Supported Operations**:
- `read` — Read memory
- `write` — Write memory
- `search` — Search memory
- `delete` — Delete memory
- `list` — List memory
- `get_stats` — Get memory statistics
- `compress` — Compress memory

**Usage Example**:
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

### 13. Network Tool (NetworkTool)

**Tool Name**: `network`

**Description**: Initiates HTTP/HTTPS requests.

**Supported Operations**:
- `get` — GET request
- `post` — POST request
- `put` — PUT request
- `delete` — DELETE request
- `download` — Download file
- `upload` — Upload file

**Permission Requirement**: `network:http`

**Usage Example**:
```json
{
  "action": "get",
  "url": "https://api.example.com/data"
}
```

---

### 14. Permission Tool (PermissionTool) 🔒

**Tool Name**: `permission`

**Permission Requirement**: Silicon Curator only

**Description**: Manages permissions and access control lists.

**Supported Operations**:
- `query_permission` — Query permissions
- `manage_acl` — Manage global ACL
- `get_callback` — Get permission callback function
- `set_callback` — Set permission callback function

**Usage Example**:
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

### 15. Project Tool (ProjectTool) 🔒

**Tool Name**: `project`

**Permission Requirement**: Silicon Curator only (`[SiliconManagerOnly]`)

**Available Scenarios**: Chat, Task, Timer

**Description**: Manages project workspaces, supporting project lifecycle management, member assignment, and role management.

**Supported Operations**:
- `create` — Create new project space
- `archive` — Archive project
- `restore` — Restore archived project
- `destroy` — Destroy project and clean up data (irreversible)
- `list` — List all projects
- `get` — Get project details
- `assign` — Assign silicon being to project
- `remove` — Remove silicon being from project
- `update` — Update project name/description
- `list-workflow-templates` — List available workflow templates
- `assign_role` — Assign project role to silicon being
- `remove_role` — Remove project role from silicon being
- `list_roles` — List role assignments for project

**Usage Example**:
```json
{
  "action": "create",
  "name": "My Project",
  "description": "项目描述"
}
```

---

### 16. Project Task Tool (ProjectTaskTool)

**Tool Name**: `project_task`

**Available Scenarios**: Chat, Task, Timer

**Description**: Manages tasks within project spaces, supporting the complete task lifecycle.

**Supported Operations**:
- `create` — Create project task
- `list` — List project tasks
- `get` — Get task details
- `update` — Update task title/description/priority
- `assign` — Assign responsible being to task
- `remove_assignee` — Remove responsible being from task
- `start` — Start task
- `complete` — Mark task as completed
- `fail` — Mark task as failed
- `cancel` — Cancel task
- `delete` — Delete task
- `stats` — Get task statistics

**Usage Example**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "完成任务描述",
  "priority": 5
}
```

---

### 17. Project Work Note Tool (ProjectWorkNoteTool)

**Tool Name**: `project_work_note`

**Available Scenarios**: Chat, Task, Timer

**Description**: Manages work notes within project spaces (public, similar to a work notebook), supporting page-style note management.

**Supported Operations**:
- `create` — Create note page (requires `project_id`, `summary`, and `content`; optional `keywords`)
- `read` — Read note page (requires `project_id` and `page_number` or `note_id`)
- `update` — Update note page (requires `project_id`, `page_number`, and `content`; optional `summary` and `keywords`)
- `delete` — Delete note page (requires `project_id` and `page_number` or `note_id`)
- `list` — List all note page summaries for the project
- `directory` — Generate note directory/overview
- `search` — Search notes by keyword (requires `project_id` and `keyword`; optional `max_results`)

**Usage Example**:
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

### 18. Project Work Tool (ProjectWorkTool) 🔒

**Tool Name**: `project_work`

**Permission Requirement**: Silicon Curator only (`[SiliconManagerOnly]`)

**Available Scenarios**: Project (`[ToolScenario(ToolScenarioFlag.Project)]`, only available in project scenario)

**Description**: Project work operation tool, used by curators to manage project workflows in the ThinkOnProject scenario.

**Supported Operations**:
- `create-task` — Create project task
- `assign-task` — Assign silicon being to task
- `chat` — Send message to project group chat
- `broadcast` — Broadcast message to project channel
- `complete` — Mark project as completed
- `status` — Get project status

**Usage Example**:
```json
{
  "action": "create-task",
  "project_id": "project-uuid",
  "title": "实现用户认证"
}
```

---

### 19. System Tool (SystemTool)

**Tool Name**: `system`

**Description**: Retrieves system information and resource usage.

**Supported Operations**:
- `info` — Get system information
- `resource_usage` — Get resource usage
- `find_process` — Find process
- `list_beings` — List silicon beings

**Usage Example**:
```json
{
  "action": "info"
}
```

---

### 20. Task Tool (TaskTool)

**Tool Name**: `task`

**Description**: Manages silicon being personal tasks.

**Supported Operations**:
- `create` — Create task
- `list` — List tasks
- `update` — Update task
- `complete` — Complete task
- `delete` — Delete task
- `get_dependencies` — Get dependencies

**Usage Example**:
```json
{
  "action": "create",
  "description": "审查代码",
  "priority": 5
}
```

---

### 21. Timer Tool (TimerTool)

**Tool Name**: `timer`

**Description**: Creates and manages timers.

**Supported Operations**:
- `create` — Create timer
- `list` — List timers
- `delete` — Delete timer
- `pause` — Pause timer
- `resume` — Resume timer
- `get_execution_history` — Get execution history

**Usage Example**:
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true,
  "message": "每小时提醒"
}
```

---

### 22. Token Audit Tool (TokenAuditTool) 🔒

**Tool Name**: `token_audit`

**Permission Requirement**: Silicon Curator only (`[SiliconManagerOnly]`)

**Available Scenarios**: Chat, Task, Timer

**Description**: Queries AI token usage statistics and trend data.

**Supported Operations**:
- `summary` — Get token usage summary statistics
- `trend` — Get token usage trend data points

**Supported Time Ranges**:
- `today` — Last 24 hours
- `week` — Last 7×24 hours
- `month` — Daily statistics
- `year` — Monthly statistics

**Usage Example**:
```json
{
  "action": "summary",
  "time_range": "week"
}
```

---

### 23. WebView Browser Tool (WebViewBrowserTool)

**Tool Name**: `webview_browser`

**Available Scenarios**: Chat, Task, Timer

**Description**: Playwright-based browser automation, providing full web page navigation, interaction, and data extraction capabilities.

**Supported Operations**:
- `open` — Open browser
- `close` — Close browser
- `navigate` — Navigate to URL
- `click` — Click element
- `input` — Input text
- `scroll` — Scroll page
- `execute_script` — Execute JavaScript
- `get_page_text` — Get page text
- `get_screenshot` — Get screenshot
- `wait_for_element` — Wait for element to appear
- `get_element_info` — Get element information
- `upload_file` — Upload file
- `get_browser_status` — Get browser status
- `set_timeout` — Set timeout
- `clear_session` — Clear browser session

**Features**:
- Independent instance per silicon being
- Completely isolated cookies and sessions
- Fully invisible to users (headless mode)
- Full JavaScript and CSS support

**Usage Example**:
```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

---

### 24. Work Note Tool (WorkNoteTool)

**Tool Name**: `work_note`

**Description**: Manages silicon being personal work notes (private, similar to a diary).

**Supported Operations**:
- `create` — Create note
- `read` — Read note
- `update` — Update note
- `delete` — Delete note
- `list` — List notes
- `search` — Search notes
- `directory` — Generate directory

**Usage Example**:
```json
{
  "action": "create",
  "summary": "完成用户认证模块",
  "content": "## 实现细节\n\n- 使用 JWT token\n- 支持 OAuth2",
  "keywords": "认证,JWT,OAuth2"
}
```

---

### 25. Hot Reload Tool (HotReloadTool)

**Tool Name**: `hot_reload`

**Description**: Supports automatic compilation, file update, and restarting SiliconLife.Fast during runtime, without manual intervention.

**Supported Operations**:
- `execute` — Execute the complete build, copy, and restart process
- `build_only` — Only build the project, without copying or restarting

**Workflow**:
1. Compile the SiliconLife.Fast project
2. Gracefully shut down the currently running Fast instance (via HTTP API)
3. Wait for process exit and port release
4. Copy build output to the target directory (skip HotReload's own files)
5. Restart the Fast instance

**Features**:
- Automatic detection and closure of the old process
- Safe file copying (does not overwrite HotReload.exe)
- Port release waiting mechanism
- Supports custom port configuration

**Usage Example**:
```json
{
  "action": "execute",
  "project_path": "src/SiliconLife.Fast",
  "source_path": "src/SiliconLife.Fast/bin/Debug/net9.0",
  "configuration": "Debug",
  "port": 8080
}
```

**Parameter Description**:
- `project_path`: Project path (relative to solution root directory)
- `source_path`: Build output directory
- `configuration`: Build configuration (Debug/Release)
- `port`: Fast instance Web port (default 8080)

**Notes**:
- Only applicable to the SiliconLife.Fast version
- Requires HotReload.exe in the tools/HotReload directory
- Brief service interruption during restart (approximately 3-5 seconds)

---

## Tool Invocation Flow

```
┌──────────┐
│   AI     │ Returns tool_calls
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ Finds and validates tool usage
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ Checks permission chain
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ Executes resource access operations
└────┬─────────┘
     ↓
┌──────────┐
│   AI     │ Receives tool results, continues thinking
└──────────┘
```

## Permission Validation

All tool executions go through the permission validation chain:

1. **UserFrequencyCache** — High-frequency user decision cache (HighDeny takes precedence over HighAllow)
2. **IPermissionCallback** — Custom permission callback function (Allowed/Denied/AskUser)
3. **IsCurator Branch** — Curators ask users via IPermissionAskHandler; non-curators query GlobalACL, defaulting to deny if no matching rule is found

## Creating Custom Tools

### Step 1: Implement ITool Interface

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

### Step 2: Add to Project

Place the tool file in the `src/SiliconLife.Common/Tools/` directory (shared tools) or the `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` directory (version-specific tools). The `ToolManager` will automatically discover and register it through reflection at startup.

### Step 2a: Register Tools via Plugin

You can also register custom tools through the plugin system:

1. Implement the `ITool` interface in the plugin project
2. Compile the plugin DLL and place it in the plugins directory
3. `ToolManager.ScanAllPluginAssemblies()` will automatically scan all loaded plugins for ITool implementations
4. Plugin tools are subject to the same permission system constraints

### Step 3: (Optional) Mark as Curator Only

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // Only accessible by Silicon Curator
}
```

## Best Practices

### 1. Always Validate Parameters

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("缺少必需参数: required_param");
}
```

### 2. Handle Errors Gracefully

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

### 3. Respect the Permission System

Never bypass permission checks. Always access resources through the executor:

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return ToolResult.Denied("Permission denied");
}
```

### 4. Provide Clear Tool Descriptions

Help the AI understand when and how to use the tool:

```csharp
public string Description => 
    "用于在不同日历系统之间转换日期。" +
    "需要提供 'date'、'from_calendar' 和 'to_calendar' 参数。";
```

## Troubleshooting

### Tool Not Found

**Problem**: AI attempts to call a non-existent tool.

**Solution**:
- Check if the tool name matches exactly
- Verify the tool file is in the `Tools/` directory
- Rebuild the project (`dotnet build`)

### Permission Denied

**Problem**: Tool execution fails with a permission error.

**Solution**:
- Check the permission audit log
- Verify the silicon being has the required permissions
- Review global ACL settings
- If it's a curator, check if the `[SiliconManagerOnly]` attribute is used

### Tool Execution Returns Error

**Problem**: Tool executes but returns a failure result.

**Solution**:
- Check the error message returned by the tool
- Verify input parameters are correctly formatted
- Review system logs for detailed error information
- Test tool functionality independently

## Next Steps

- 📚 Read [Architecture Guide](architecture.md)
- 🛠️ View [Development Guide](development-guide.md)
- 🔒 Learn about [Permission System](permission-system.md)
- 🚀 View [Getting Started Guide](getting-started.md)
