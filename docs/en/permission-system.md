# Permission System

> **Version: v0.2.0-alpha**

**English** | [Deutsch](../de-DE/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Русский](../ru-RU/permission-system.md)

## Overview

The permission system ensures all AI-initiated operations are properly validated and audited.

## Permission Verification Chain

```
┌─────────────────────────────────────────────┐
│          权限验证                            │
├─────────────────────────────────────────────┤
│  级别 1：UserFrequencyCache                  │
│  ↓ 高频用户决策缓存（HighDeny/HighAllow）    │
│  级别 2：IPermissionCallback                 │
│  ↓ 自定义逻辑（Allowed/Denied/AskUser）     │
│  级别 3：IsCurator?                          │
│  ↓ 是 → IPermissionAskHandler（询问用户）    │
│  ↓ 否 → GlobalACL → 默认拒绝                │
│  结果：允许或拒绝                            │
└─────────────────────────────────────────────┘
```

> **Note**: The actual query priority in `PermissionManager.CheckPermission()` is:
> 1. **UserFrequencyCache** — Check cached high-frequency user decisions first
> 2. **IPermissionCallback** — Evaluate custom callback rules
> 3. **Curator branch** — When callback returns AskUser or no callback:
>    - **Curator** → `IPermissionAskHandler` (ask user via IM)
>    - **Non-curator** → `GlobalACL` → default deny

## Level 1: UserFrequencyCache

Per-being high-frequency user decision cache (HighDeny/HighAllow), memory-only.

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny** takes priority over **HighAllow**
- **Memory-only**: Caches are not persisted, lost on restart
- **Configurable expiration**: Users can set validity period for cache entries

## Level 2: IPermissionCallback

Custom callbacks for dynamic permission logic.

### DefaultPermissionCallback Default Implementation

`DefaultPermissionCallback` provides comprehensive default permission rules, including:

#### Network Access Rules
- **Loopback addresses**: Allow localhost, 127.0.0.1, ::1
- **Private IP addresses**:
  - 192.168.x.x (Class C) - Allowed
  - 10.x.x.x (Class A) - Allowed
  - 172.16-31.x.x (Class B) - AskUser
- **Domain whitelist**:
  - Search engines: Google, Bing, DuckDuckGo, Yandex, Sogou, etc.
  - AI services: OpenAI, Anthropic, HuggingFace, Ollama, etc.
  - Developer services: GitHub, StackOverflow, npm, NuGet, etc.
  - Social media: Weibo, Zhihu, Reddit, Discord, etc.
  - Video platforms: YouTube, Bilibili, Douyin, TikTok, etc.
  - **Weather information**: wttr.in
  - Government websites: .gov, .go.jp, .go.kr
- **Domain blacklist**:
  - AI impersonation websites: chatgpt, openai, deepseek, etc. spoofed domains
  - Malicious AI tools: wormgpt, darkgpt, fraudgpt, etc.
  - AI content farms and black market related domains

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
    {
        if (IsSafeOperation(permissionType, resource))
        {
            return PermissionResult.Allowed;
        }
        
        return PermissionResult.AskUser;
    }
}
```

## Level 3: Branch Decision (IsCurator / GlobalACL)

When the callback returns `AskUser` or no callback is configured, the system branches based on curator status:

### Curator Branch (IsCurator = true)

For the Silicon Curator, the system requests a decision from the user via IM:

```csharp
if (IsCurator)
{
    if (_askHandler != null)
    {
        AskPermissionResult userDecision = _askHandler.AskUser(callerId, permissionType, resource);
        // 用户在 Web UI 中确认或拒绝
    }
}
```

### Non-Curator Branch (IsCurator = false)

For non-curator beings, the system checks the Global ACL. If no matching rule is found, the request is denied by default.

### GlobalACL Structure

```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed"
    },
    {
      "permissionType": "FileAccess",
      "resourcePrefix": "C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

Rules are evaluated in order; the first matching rule takes effect. Only the Silicon Curator can modify the Global ACL.

### Resource Format

```
{type}:{path}

示例：
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler

When a curator operation requires user confirmation, the system asks the user for permission via `IPermissionAskHandler`.

### IMPermissionAskHandler Implementation

`IMPermissionAskHandler` sends permission requests to the user via the Web UI:

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        // 通过即时通讯向用户发送消息
        SendMessageAsync($"Allow {resource}?");

        // 等待用户响应
        var response = WaitForResponseAsync();

        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### PermissionRequestQueue

`PermissionRequestQueue` manages pending permission requests, supporting asynchronous waiting for user responses:

- **Request enqueue** — When the permission chain reaches Level 5, a `TaskCompletionSource<AskPermissionResult>` is created and enqueued
- **Web UI display** — Pending permission requests are displayed in the Web UI via `PermissionRequestController`
- **User response** — Users approve or deny in the Web UI, with options to cache the decision and set cache duration
- **Cache options** — Users can cache permission decisions for 1 hour, 24 hours, 7 days, or 30 days
- **Timeout mechanism** — Requests are automatically closed after 60 seconds of no response

## Audit System

All permission decisions are logged:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "callerId": "being-uuid",
  "permissionType": "FileAccess",
  "resource": "C:\\data\\config.json",
  "result": "Allowed",
  "reason": "Global ACL"
}
```

## Programmatic Permission Evaluation

### EvaluatePermission API

The `PermissionManager.EvaluatePermission()` method provides read-only permission pre-evaluation without triggering user prompts. `PermissionTool` uses this method to let AI check permission status before attempting operations.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**Return value**: Three-state `PermissionResult`:
- `Allowed` - Operation is allowed
- `Denied` - Operation is denied
- `AskUser` - User confirmation required on execution

**Evaluation order**:
1. **User Frequency Cache** - Check cached user decisions
2. **IPermissionCallback** - Custom callback evaluation
3. **Curator status** - If curator, returns `AskUser` (needs confirmation)
4. **Global ACL** - Check access control rules
5. **Default** - Deny when no matching rule

> **Note**: Unlike the full permission chain, `EvaluatePermission` does **not** call `IPermissionAskHandler`. It only reports what the result *would be* on execution.

## Managing Permissions

### Grant Permissions

**Via Web UI**:
1. Navigate to **Permission Management**
2. Click **Add Rule**
3. Configure:
   - User
   - Resource
   - Allow/Deny
   - Duration

**Via API**:
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

### Revoke Permissions

Operate through the permission management page in the Web UI.

### View Permissions

```bash
curl http://localhost:8080/api/permissions/list
```

## Tool Permission System

In addition to the operation-level permission verification chain, the system also provides a **Tool Permission** management mechanism to control which tools Silicon Beings can use.

### Two-Level Tool Permissions

Tool permissions are divided into two levels:

1. **Silicon Being level** — Controls which tool operations an individual Silicon Being can use
2. **Project level** — Controls available tool operations within a project space, independent of Silicon Being level permissions

### Tool Permission Configuration

Each operation of each tool can be independently configured as allowed or denied:

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network:get": "allowed",
    "network:post": "denied",
    "disk:read": "allowed",
    "disk:write": "denied",
    "database:query": "allowed"
  }
}
```

### Permission Templates

The system provides predefined tool permission templates that can be quickly applied to Silicon Beings:

- **readonly** — Read-only permission (allow read operations, deny write operations)
- **full** — Full permission (allow all operations)
- **restricted** — Restricted permission (only allow basic operations)

### Web UI Management

Manage tool permissions via the Web UI:

- **Silicon Being Tool Permissions page** — `/beings/tool-permissions`
- **Project Tool Permissions page** — `/project/{id}/tool-permissions`

### API Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/beings/tool-permissions` | GET | Get Silicon Being tool permissions |
| `/api/beings/tool-permissions` | PUT | Update Silicon Being tool permissions |
| `/api/beings/tool-permissions/templates` | GET | Get permission template list |
| `/api/beings/tool-permissions/apply-template` | POST | Apply permission template |
| `/api/projects/{id}/tool-permissions` | GET | Get project tool permissions |
| `/api/projects/{id}/tool-permissions` | PUT | Update project tool permissions |

---

## Best Practices

### 1. Principle of Least Privilege

Grant only the minimum permissions required:

```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects\\MyApp\\config.json",
  "result": "Allowed"
}
```

### 2. Use Time-Limited Permissions

Never grant permanent permissions unless absolutely necessary.

### 3. Monitor Permission Logs

Regularly review audit logs for:
- Denied access attempts
- Unusual patterns
- Permission escalation

### 4. Implement Custom Callbacks

For complex logic, use `IPermissionCallback`:

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    // 基于时间的权限
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }
    
    // 基于资源的权限
    if (IsSensitiveResource(resource))
    {
        return PermissionResult.AskUser;
    }
    
    return PermissionResult.Allowed;
}
```

## Common Scenarios

### Scenario 1: AI Wants to Read a File

```
AI: "I need to read config.json"
↓
Permission chain:
1. UserFrequencyCache? No cached decision
2. IPermissionCallback? Returns AskUser (not explicitly allowed)
3. IsCurator? No → Check GlobalACL
4. GlobalACL? Found rule: file:... = Allowed
5. Result: Allowed
```

### Scenario 2: AI Wants to Execute Code

```
AI: "I want to compile and run code"
↓
Permission chain:
1. UserFrequencyCache? No cached decision
2. IPermissionCallback? Returns AskUser
3. IsCurator? Yes → IPermissionAskHandler
4. User approves
5. Result: Allowed
```

### Scenario 3: Cached Denial

```
AI: "I need to access C:\Windows"
↓
Permission chain:
1. UserFrequencyCache? Found in HighDeny cache
2. Result: Denied (no further checks needed)
```

## Troubleshooting

### Unexpected Permission Denial

**Check**:
1. User's IsCurator status
2. HighDeny entries in the frequency cache
3. GlobalACL rules
4. Callback logic
5. User response timeout

### Permissions Not Expiring

**Check**:
- `expiresAt` field set correctly
- Timezone is correct
- Clock synchronization

### Audit Logs Not Recording

**Check**:
- Audit logger is registered
- Storage backend accessible
- Sufficient disk space

## Next Steps

- 📚 Read the [Architecture Guide](architecture.md)
- 🛠️ Check the [Development Guide](development-guide.md)
- 🔒 Review the [Security Documentation](security.md)
- 🚀 See the [Quick Start Guide](getting-started.md)
