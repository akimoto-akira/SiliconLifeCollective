# 權限系統

## 概述

權限系統确保所有 AI 發起的操作都经過适當驗證和稽核。

## 5 級權限链

```
┌─────────────────────────────────────────────┐
│          權限驗證                            │
├─────────────────────────────────────────────┤
│  級别 1：IsCurator                           │
│  ↓ 如果為真則绕過                            │
│  級别 2：UserFrequencyCache                  │
│  ↓ 速率限制                                  │
│  級别 3：GlobalACL                           │
│  ↓ 訪問控制列表                              │
│  級别 4：IPermissionCallback                 │
│  ↓ 自定義逻辑                                │
│  級别 5：IPermissionAskHandler               │
│  ↓ 询問使用者                                  │
│  結果：允許或拒絕                            │
└─────────────────────────────────────────────┘
```

## 級别 1：IsCurator

管理員/館長绕過所有權限檢查。

```csharp
if (user.IsCurator)
{
    return PermissionResult.Allowed("Curator access");
}
```

## 級别 2：UserFrequencyCache

每個使用者的速率限制以防止滥用。

```csharp
var cache = new UserFrequencyCache();
if (!cache.CheckLimit(userId, resource))
{
    return PermissionResult.Denied("Rate limit exceeded");
}
```

## 級别 3：GlobalACL

全局訪問控制列表定義明确規則。

### ACL 結構

```json
{
  "rules": [
    {
      "userId": "user-uuid",
      "resource": "disk:read",
      "allowed": true,
      "expiresAt": "2026-04-21T00:00:00Z"
    }
  ]
}
```

### 資源格式

```
{type}:{action}

示例：
- disk:read
- disk:write
- network:http
- compile:execute
- system:info
```

## 級别 4：IPermissionCallback

用於動态權限逻辑的自定義回调。

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public async Task<PermissionResult> CheckAsync(PermissionRequest request)
    {
        // 自定義逻辑
        if (IsSafeOperation(request))
        {
            return PermissionResult.Allowed("Safe operation");
        }
        
        return PermissionResult.Undecided("Needs user confirmation");
    }
}
```

## 級别 5：IPermissionAskHandler

當所有其他級别都未決定时询問使用者權限。

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public async Task<AskPermissionResult> AskAsync(PermissionRequest request)
    {
        // 通過即时通訊向使用者發送訊息
        await SendMessageAsync($"Allow {request.Resource}?");
        
        // 等待使用者回應
        var response = await WaitForResponseAsync();
        
        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

## 稽核系統

所有權限決策都被記录：

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "level": "GlobalACL",
  "reason": "Explicit rule granted"
}
```

## 管理權限

### 授予權限

**通過 Web UI**：
1. 導航到**權限管理**
2. 点擊**添加規則**
3. 設定：
   - 使用者
   - 資源
   - 允許/拒絕
   - 持续時間

**通過 API**：
```bash
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "user-uuid",
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

### 撤销權限

```bash
curl -X DELETE http://localhost:8080/api/permissions/{rule-id}
```

### 查看權限

```bash
curl http://localhost:8080/api/permissions?userId=user-uuid
```

## 最佳实践

### 1. 最小權限原則

僅授予所需的最小權限：

```json
{
  "resource": "disk:read",  // 不是 disk:*
  "allowed": true,
  "expiresAt": "2026-04-21T00:00:00Z"  // 始终設定過期
}
```

### 2. 使用時間限制權限

除非绝對必要，否則永遠不要授予永久權限。

### 3. 監控權限記錄

定期查看稽核記錄以了解：
- 拒絕的訪問尝试
- 例外模式
- 權限升級

### 4. 實現自定義回调

對於复雜逻辑，使用 `IPermissionCallback`：

```csharp
public async Task<PermissionResult> CheckAsync(PermissionRequest request)
{
    // 基於時間的權限
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied("Outside business hours");
    }
    
    // 基於資源的權限
    if (IsSensitiveResource(request.Resource))
    {
        return PermissionResult.Undecided("Requires approval");
    }
    
    return PermissionResult.Allowed();
}
```

## 常見場景

### 場景 1：AI 想要读取檔案

```
AI："我需要读取 config.json"
↓
權限链：
1. IsCurator？否
2. 速率限制？正常
3. GlobalACL？找到規則：disk:read = 允許
4. 結果：允許
```

### 場景 2：AI 想要執行程式碼

```
AI："我想編譯和執行程式碼"
↓
權限链：
1. IsCurator？否
2. 速率限制？正常
3. GlobalACL？未找到規則
4. 回调？返回未決定
5. 询問使用者？使用者核准
6. 結果：允許
```

### 場景 3：超過速率限制

```
AI："我需要發出 100 個 HTTP 要求"
↓
權限链：
1. IsCurator？否
2. 速率限制？已超過
3. 結果：拒絕
```

## 故障排除

### 意外拒絕權限

**檢查**：
1. 使用者的 IsCurator 狀態
2. 速率限制設定
3. GlobalACL 規則
4. 回调逻辑
5. 使用者回應超时

### 權限未過期

**檢查**：
- `expiresAt` 字段設定正确
- 时區正确
- 时钟同步

### 稽核記錄未記录

**檢查**：
- 稽核記錄器已註冊
- 儲存後端可訪問
- 磁碟空间充足

## 下一步

- 📚 阅读[架構指南](architecture.md)
- 🛠️ 查看[開發指南](development-guide.md)
- 🔒 查看[安全文檔](security.md)
- 🚀 查看[快速開始指南](getting-started.md)
