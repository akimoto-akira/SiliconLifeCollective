# 权限系统

## 概述

权限系统确保所有 AI 发起的操作都经过适当验证和审计。

## 5 级权限链

```
┌─────────────────────────────────────────────┐
│          权限验证                            │
├─────────────────────────────────────────────┤
│  级别 1：IsCurator                           │
│  ↓ 如果为真则绕过                            │
│  级别 2：UserFrequencyCache                  │
│  ↓ 速率限制                                  │
│  级别 3：GlobalACL                           │
│  ↓ 访问控制列表                              │
│  级别 4：IPermissionCallback                 │
│  ↓ 自定义逻辑                                │
│  级别 5：IPermissionAskHandler               │
│  ↓ 询问用户                                  │
│  结果：允许或拒绝                            │
└─────────────────────────────────────────────┘
```

## 级别 1：IsCurator

管理员/主理人绕过所有权限检查。

```csharp
if (user.IsCurator)
{
    return PermissionResult.Allowed("Curator access");
}
```

## 级别 2：UserFrequencyCache

每个用户的速率限制以防止滥用。

```csharp
var cache = new UserFrequencyCache();
if (!cache.CheckLimit(userId, resource))
{
    return PermissionResult.Denied("Rate limit exceeded");
}
```

## 级别 3：GlobalACL

全局访问控制列表定义明确规则。

### ACL 结构

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

### 资源格式

```
{type}:{action}

示例：
- disk:read
- disk:write
- network:http
- compile:execute
- system:info
```

## 级别 4：IPermissionCallback

用于动态权限逻辑的自定义回调。

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public async Task<PermissionResult> CheckAsync(PermissionRequest request)
    {
        // 自定义逻辑
        if (IsSafeOperation(request))
        {
            return PermissionResult.Allowed("Safe operation");
        }
        
        return PermissionResult.Undecided("Needs user confirmation");
    }
}
```

## 级别 5：IPermissionAskHandler

当所有其他级别都未决定时询问用户权限。

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public async Task<AskPermissionResult> AskAsync(PermissionRequest request)
    {
        // 通过即时通讯向用户发送消息
        await SendMessageAsync($"Allow {request.Resource}?");
        
        // 等待用户响应
        var response = await WaitForResponseAsync();
        
        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

## 审计系统

所有权限决策都被记录：

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

## 管理权限

### 授予权限

**通过 Web UI**：
1. 导航到**权限管理**
2. 点击**添加规则**
3. 配置：
   - 用户
   - 资源
   - 允许/拒绝
   - 持续时间

**通过 API**：
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

### 撤销权限

```bash
curl -X DELETE http://localhost:8080/api/permissions/{rule-id}
```

### 查看权限

```bash
curl http://localhost:8080/api/permissions?userId=user-uuid
```

## 最佳实践

### 1. 最小权限原则

仅授予所需的最小权限：

```json
{
  "resource": "disk:read",  // 不是 disk:*
  "allowed": true,
  "expiresAt": "2026-04-21T00:00:00Z"  // 始终设置过期
}
```

### 2. 使用时间限制权限

除非绝对必要，否则永远不要授予永久权限。

### 3. 监控权限日志

定期查看审计日志以了解：
- 拒绝的访问尝试
- 异常模式
- 权限升级

### 4. 实现自定义回调

对于复杂逻辑，使用 `IPermissionCallback`：

```csharp
public async Task<PermissionResult> CheckAsync(PermissionRequest request)
{
    // 基于时间的权限
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied("Outside business hours");
    }
    
    // 基于资源的权限
    if (IsSensitiveResource(request.Resource))
    {
        return PermissionResult.Undecided("Requires approval");
    }
    
    return PermissionResult.Allowed();
}
```

## 常见场景

### 场景 1：AI 想要读取文件

```
AI："我需要读取 config.json"
↓
权限链：
1. IsCurator？否
2. 速率限制？正常
3. GlobalACL？找到规则：disk:read = 允许
4. 结果：允许
```

### 场景 2：AI 想要执行代码

```
AI："我想编译和运行代码"
↓
权限链：
1. IsCurator？否
2. 速率限制？正常
3. GlobalACL？未找到规则
4. 回调？返回未决定
5. 询问用户？用户批准
6. 结果：允许
```

### 场景 3：超过速率限制

```
AI："我需要发出 100 个 HTTP 请求"
↓
权限链：
1. IsCurator？否
2. 速率限制？已超过
3. 结果：拒绝
```

## 故障排除

### 意外拒绝权限

**检查**：
1. 用户的 IsCurator 状态
2. 速率限制设置
3. GlobalACL 规则
4. 回调逻辑
5. 用户响应超时

### 权限未过期

**检查**：
- `expiresAt` 字段设置正确
- 时区正确
- 时钟同步

### 审计日志未记录

**检查**：
- 审计日志器已注册
- 存储后端可访问
- 磁盘空间充足

## 下一步

- 📚 阅读[架构指南](architecture.md)
- 🛠️ 查看[开发指南](development-guide.md)
- 🔒 查看[安全文档](security.md)
- 🚀 查看[快速开始指南](getting-started.md)
