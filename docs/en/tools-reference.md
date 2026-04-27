# Tools Reference

> **Version: v0.1.0-alpha**

This document provides detailed information about all built-in tools in the Silicon Life Collective platform.

[English](../en/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md)

## Overview

The tool system allows silicon beings to interact with the external world through a standardized interface. Each tool implements the `ITool` interface and is automatically discovered and registered by the `ToolManager` through reflection.

### Tool Categories

- **System Management Tools** — Configuration, permissions, dynamic compilation
- **Communication Tools** — Chat, network requests
- **Data Storage Tools** — Disk operations, databases, memory, work notes
- **Time Management Tools** — Calendar, timers, tasks
- **Development Tools** — Code execution, log queries
- **Utility Tools** — System information, token audit, help documentation, knowledge network
- **Browser Tools** — WebView browser automation

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
  "message": "Hello, let's collaborate!"
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

**Tool Name**: `curator`

**Permission Requirement**: Silicon Curator only

**Description**: System management tool exclusive to Silicon Curators.

**Supported Operations**:
- `create_being` — Create new silicon being
- `list_beings` — List all silicon beings
- `get_being_info` — Get being information
- `assign_task` — Assign task
- `manage_permissions` — Manage permissions

**Usage Example**:
```json
{
  "action": "create_being",
  "name": "Assistant",
  "soul_file": "assistant_soul.md"
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

**Permission Requirements**: `disk:read`, `disk:write`

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

**Description**: Retrieves system help documentation and usage guides.

**Supported Operations**:
- `get_topics` — Get help topic list
- `get_topic` — Get specific topic details
- `search` — Search help documentation

**Usage Example**:
```json
{
  "action": "get_topics"
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

**Permission Requirements**: `network:http`

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

### 15. Project Tool (ProjectTool)

**Tool Name**: `project`

**Description**: Manages project workspaces.

**Supported Operations**:
- `create` — Create project
- `list` — List projects
- `get_info` — Get project information
- `update` — Update project
- `archive` — Archive project

**Usage Example**:
```json
{
  "action": "create",
  "name": "My Project",
  "description": "Project description"
}
```

---

### 16. Project Task Tool (ProjectTaskTool)

**Tool Name**: `project_task`

**Description**: Manages project tasks.

**Supported Operations**:
- `create` — Create task
- `list` — List tasks
- `update` — Update task
- `complete` — Complete task
- `get_stats` — Get task statistics

**Usage Example**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "Task description",
  "priority": 5
}
```

---

### 17. Project Work Note Tool (ProjectWorkNoteTool)

**Tool Name**: `project_work_note`

**Description**: Manages project work notes (public, similar to a work notebook).

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
  "project_id": "project-uuid",
  "summary": "Completed user authentication module",
  "content": "## Implementation Details\n\n- Using JWT token",
  "keywords": "authentication,JWT"
}
```

---

### 18. System Tool (SystemTool)

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

### 19. Task Tool (TaskTool)

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
  "description": "Review code",
  "priority": 5
}
```

---

### 20. Timer Tool (TimerTool)

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
  "message": "Hourly reminder"
}
```

---

### 21. Token Audit Tool (TokenAuditTool) 🔒

**Tool Name**: `token_audit`

**Permission Requirement**: Silicon Curator only

**Description**: Queries and aggregates AI token usage.

**Supported Operations**:
- `get_usage` — Get token usage statistics
- `get_by_being` — Get usage by being
- `get_by_model` — Get usage by model
- `get_trend` — Get usage trend
- `export` — Export data

**Usage Example**:
```json
{
  "action": "get_usage",
  "start_date": "2026-04-01",
  "end_date": "2026-04-26"
}
```

---

### 22. WebView Browser Tool (WebViewBrowserTool)

**Tool Name**: `webview`

**Description**: Browser automation based on Playwright.

**Supported Operations**:
- `open_browser` — Open browser
- `close_browser` — Close browser
- `navigate` — Navigate to URL
- `click` — Click element
- `input` — Input text
- `get_page_text` — Get page text
- `get_screenshot` — Get screenshot
- `execute_script` — Execute JavaScript
- `wait_for_element` — Wait for element
- `get_browser_status` — Get browser status

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

### 23. Work Note Tool (WorkNoteTool)

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
  "summary": "Completed user authentication module",
  "content": "## Implementation Details\n\n- Using JWT token\n- Supports OAuth2",
  "keywords": "authentication,JWT,OAuth2"
}
```

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

All tool executions go through a 5-level permission chain:

1. **IsCurator** — Silicon Curator bypasses all checks
2. **UserFrequencyCache** — User high-frequency allow/deny cache
3. **GlobalACL** — Global Access Control List
4. **IPermissionCallback** — Custom permission callback function
5. **IPermissionAskHandler** — Ask user

## Creating Custom Tools

### Step 1: Implement ITool Interface

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "Tool description";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, object>
        {
            ["param1"] = new { type = "string", description = "Parameter description" }
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

Place the tool file in the `src/SiliconLife.Default/Tools/` directory. The `ToolManager` will automatically discover and register it through reflection at startup.

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
    return ToolResult.Failure("Missing required parameter: required_param");
}
```

### 2. Handle Errors Gracefully

```csharp
try
{
    // Perform operation
}
catch (Exception ex)
{
    Logger.Error($"Tool {Name} execution failed: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. Respect the Permission System

Never bypass permission checks. Always access resources through the executor:

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return ToolResult.Denied(permission.Reason);
}
```

### 4. Provide Clear Tool Descriptions

Help the AI understand when and how to use the tool:

```csharp
public string Description => 
    "Used to convert dates between different calendar systems. " +
    "Requires 'date', 'from_calendar', and 'to_calendar' parameters.";
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
- Verify the silicon being has the required permission
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
