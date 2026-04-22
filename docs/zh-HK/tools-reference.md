# 工具参考

## 概述

工具系統允許 AI 智能体通過標準化介面與外部世界交互。

## 內置工具

### 1. 日歷工具

**名称**：`calendar`

**描述**：在不同日歷系統之间转换日期。

**參數**：
```json
{
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**支援的日歷**（32 种系統）：
- 公歷、農歷、伊斯蘭歷、希伯來歷
- 日本歷、波斯歷、瑪雅歷、藏歷
- 還有 24 种...

### 2. 聊天工具

**名称**：`chat`

**描述**：向其他生命体或使用者發送訊息。

**參數**：
```json
{
  "targetId": "being-uuid",
  "message": "Hello, let's collaborate"
}
```

### 3. 設定工具

**名称**：`config`

**描述**：读取和修改系統設定。

**參數**：
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

### 4. 磁碟工具

**名称**：`disk`

**描述**：檔案系統操作（读取、寫入、列表）。

**參數**：
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

**所需權限**：`disk:read`、`disk:write`

### 5. 動态編譯工具

**名称**：`compile`

**描述**：動态編譯和執行 C# 程式碼。

**參數**：
```json
{
  "code": "public class Test { ... }",
  "references": ["System.Linq"]
}
```

**安全**：程式碼在執行前被掃描。

### 6. 記憶工具

**名称**：`memory`

**描述**：儲存和检索生命体記憶。

**參數**：
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

### 7. 網路工具

**名称**：`network`

**描述**：發出 HTTP 要求。

**參數**：
```json
{
  "method": "GET",
  "url": "https://api.example.com/data",
  "headers": {}
}
```

**所需權限**：`network:http`

### 8. 系統工具

**名称**：`system`

**描述**：获取系統資訊。

**參數**：
```json
{
  "action": "info"
}
```

### 9. 工作工具

**名称**：`task`

**描述**：管理生命体工作。

**參數**：
```json
{
  "action": "create",
  "description": "Review code",
  "priority": 5
}
```

### 10. 定时器工具

**名称**：`timer`

**描述**：建立和管理定时器。

**參數**：
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true
}
```

### 11. Token 稽核工具

**名称**：`token_audit`

**描述**：查詢 token 使用統計。

**參數**：
```json
{
  "startDate": "2026-04-01",
  "endDate": "2026-04-20"
}
```

### 12. 權限工具

**名称**：`permission`

**描述**：管理矽基生命體的權限。僅限館長。

**動作**：`query_permission`、`manage_acl`

**參數**（query_permission）：
```json
{
  "action": "query_permission",
  "being_id": "being-uuid",
  "permission_type": "network",
  "resource": "https://api.example.com"
}
```

**權限類型**：`network`、`command`、`filesystem`、`function`、`data`

**返回**：三態結果（`ALLOWED`、`DENIED`、`ASK_USER`），包含館長狀態和頻率快取資訊。

**參數**（manage_acl）：
```json
{
  "action": "manage_acl",
  "acl_action": "add_rule",
  "permission_type": "filesystem",
  "resource_prefix": "/data/",
  "acl_result": "allow",
  "description": "允許訪問數據目錄"
}
```

**權限**：需要 `IsCurator` 标志。

### 13. 程式碼執行工具

**名称**：`execute_code`

**描述**：編譯並執行帶有安全掃描的 C# 程式碼。僅限館長。

**動作**：`run_script`

**參數**：
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

**詳細說明**：
- 程式碼被包裝在 `ScriptExecutor` 類中的 `Execute()` 方法中
- 編譯前進行安全掃描
- 支援可配置的逾時時間（預設：30 秒）
- 失敗時返回編譯錯誤和安全違規資訊

**權限**：需要 `IsCurator` 标志。

---

## 工具调用流程

```
┌──────────┐
│   AI     │ 返回 tool_calls
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ 查找和驗證工具
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ 檢查權限链
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ 執行操作
└────┬─────────┘
     ↓
┌──────────┐
│   AI     │ 接收工具結果
└──────────┘
```

## 權限驗證

所有工具都通過 5 級權限链：

1. **IsCurator**：管理員绕過所有檢查
2. **UserFrequencyCache**：每個使用者的速率限制
3. **GlobalACL**：訪問控制列表
4. **IPermissionCallback**：自定義回调逻辑
5. **IPermissionAskHandler**：询問使用者權限

## 建立自定義工具

### 步驟 1：實現 ITool

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

### 步驟 2：添加到項目

将工具放置在 `src/SiliconLife.Default/Tools/` 中。

`ToolManager` 将通過反射自動發現它。

### 步驟 3：（可选）标記為僅管理員可用

```csharp
[SiliconManagerOnly]
public class AdminTool : ITool
{
    // 僅主理人可訪問
}
```

## 最佳实践

### 1. 始终驗證參數

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("Missing required parameter: required_param");
}
```

### 2. 優雅處理錯誤

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

### 3. 尊重權限

永遠不要绕過權限系統。始终使用：

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return ToolResult.Denied(permission.Reason);
}
```

### 4. 提供清晰描述

幫助 AI 理解何时以及如何使用您的工具：

```csharp
public string Description => 
    "Use this tool to convert dates between calendar systems. " +
    "Requires 'date', 'fromCalendar', and 'toCalendar' parameters.";
```

## 測試工具

### 单元測試示例

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

**問題**：AI 尝试调用不存在的工具。

**解決方案**：
- 檢查工具名称完全匹配
- 驗證工具在 Tools 目錄中
- 重新构建項目

### 權限被拒絕

**問題**：工具執行失敗，出现權限錯誤。

**解決方案**：
- 檢查權限記錄
- 驗證使用者具有所需權限
- 查看 GlobalACL 設定

### 工具返回錯誤

**問題**：工具執行但返回失敗。

**解決方案**：
- 檢查工具記錄以获取详细錯誤
- 驗證输入參數
- 独立測試工具

## 下一步

- 📚 阅读[架構指南](architecture.md)
- 🛠️ 查看[開發指南](development-guide.md)
- 🔒 查看[權限系統](permission-system.md)
- 🚀 查看[快速開始指南](getting-started.md)
