# Tools Reference

## Overview

The tool system allows AI agents to interact with the external world through a standardized interface.

## Built-in Tools

### 1. Calendar Tool

**Name**: `calendar`

**Description**: Convert dates between different calendar systems.

**Parameters**:
```json
{
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**Supported Calendars** (32 systems):
- Gregorian, Chinese Lunar, Islamic, Hebrew
- Japanese, Persian, Mayan, Tibetan
- And 24 more...

### 2. Chat Tool

**Name**: `chat`

**Description**: Send messages to other beings or users.

**Parameters**:
```json
{
  "targetId": "being-uuid",
  "message": "Hello, let's collaborate"
}
```

### 3. Configuration Tool

**Name**: `config`

**Description**: Read and modify system configuration.

**Parameters**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

### 4. Disk Tool

**Name**: `disk`

**Description**: File system operations (read, write, list).

**Parameters**:
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

**Permission Required**: `disk:read`, `disk:write`

### 5. Dynamic Compile Tool

**Name**: `compile`

**Description**: Compile and execute C# code dynamically.

**Parameters**:
```json
{
  "code": "public class Test { ... }",
  "references": ["System.Linq"]
}
```

**Security**: Code is scanned before execution.

### 6. Memory Tool

**Name**: `memory`

**Description**: Store and retrieve being memories.

**Parameters**:
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

### 7. Network Tool

**Name**: `network`

**Description**: Make HTTP requests.

**Parameters**:
```json
{
  "method": "GET",
  "url": "https://api.example.com/data",
  "headers": {}
}
```

**Permission Required**: `network:http`

### 8. System Tool

**Name**: `system`

**Description**: Get system information.

**Parameters**:
```json
{
  "action": "info"
}
```

### 9. Task Tool

**Name**: `task`

**Description**: Manage being tasks.

**Parameters**:
```json
{
  "action": "create",
  "description": "Review code",
  "priority": 5
}
```

### 10. Timer Tool

**Name**: `timer`

**Description**: Create and manage timers.

**Parameters**:
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true
}
```

### 11. Token Audit Tool

**Name**: `token_audit`

**Description**: Query token usage statistics.

**Parameters**:
```json
{
  "startDate": "2026-04-01",
  "endDate": "2026-04-20"
}
```

### 12. Curator Tools

**Name**: `curator_*`

**Description**: Manager-only tools for system administration.

**Permission**: Requires `IsCurator` flag.

---

## Tool Call Flow

```
┌──────────┐
│   AI     │ Returns tool_calls
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ Finds and validates tool
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ Checks permission chain
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ Executes the operation
└────┬─────────┘
     ↓
┌──────────┐
│   AI     │ Receives tool result
└──────────┘
```

## Permission Validation

All tools go through the 5-level permission chain:

1. **IsCurator**: Manager bypasses all checks
2. **UserFrequencyCache**: Rate limiting per user
3. **GlobalACL**: Access control list
4. **IPermissionCallback**: Custom callback logic
5. **IPermissionAskHandler**: Ask user for permission

## Creating Custom Tools

### Step 1: Implement ITool

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

### Step 2: Add to Project

Place the tool in `src/SiliconLife.Default/Tools/`.

The `ToolManager` will automatically discover it via reflection.

### Step 3: (Optional) Mark as Manager-Only

```csharp
[SiliconManagerOnly]
public class AdminTool : ITool
{
    // Only accessible by curator
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
    // Operation
}
catch (Exception ex)
{
    Logger.Error($"Tool {Name} failed: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. Respect Permissions

Never bypass the permission system. Always use:

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return ToolResult.Denied(permission.Reason);
}
```

### 4. Provide Clear Descriptions

Help the AI understand when and how to use your tool:

```csharp
public string Description => 
    "Use this tool to convert dates between calendar systems. " +
    "Requires 'date', 'fromCalendar', and 'toCalendar' parameters.";
```

## Testing Tools

### Unit Test Example

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

## Troubleshooting

### Tool Not Found

**Problem**: AI tries to call a tool that doesn't exist.

**Solution**: 
- Check tool name matches exactly
- Verify tool is in the Tools directory
- Rebuild the project

### Permission Denied

**Problem**: Tool execution fails with permission error.

**Solution**:
- Check permission logs
- Verify user has required permissions
- Review GlobalACL settings

### Tool Returns Error

**Problem**: Tool executes but returns failure.

**Solution**:
- Check tool logs for detailed error
- Verify input parameters
- Test tool independently

## Next Steps

- 📚 Read the [Architecture Guide](architecture.md)
- 🛠️ Check the [Development Guide](development-guide.md)
- 🔒 Review the [Permission System](permission-system.md)
- 🚀 See the [Getting Started Guide](getting-started.md)
