# Permission System

> **Version: v0.2.0-alpha**

**English** | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [Čeština](../cs-CZ/permission-system.md)

## Overview

The permission system ensures all AI-initiated operations are properly validated and audited.

## 3-Level Permission Chain

```
┌─────────────────────────────────────────────┐
│          Permission Verification             │
├─────────────────────────────────────────────┤
│  Level 1: UserFrequencyCache                 │
│  ↓ Cached user decisions (HighDeny/HighAllow)│
│  Level 2: IPermissionCallback                │
│  ↓ Custom logic (Allowed/Denied/AskUser)     │
│  Level 3: IsCurator?                         │
│  ↓ Yes → IPermissionAskHandler (ask user)    │
│  ↓ No  → GlobalACL → Default Deny            │
│  Result: Allowed or Denied                   │
└─────────────────────────────────────────────┘
```

> **Note**: The actual query priority in `PermissionManager.CheckPermission()` is:
> 1. **UserFrequencyCache** — Check cached high-frequency user decisions first
> 2. **IPermissionCallback** — Evaluate custom callback rules
> 3. **Curator branch** — If callback returns AskUser or no callback:
>    - **Curator** → `IPermissionAskHandler` (prompt user via IM)
>    - **Non-curator** → `GlobalACL` → default deny

## Level 1: UserFrequencyCache

Per-being, memory-only cache of high-frequency user decisions (HighDeny/HighAllow).

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
        // Custom logic
        if (IsSafeOperation(permissionType, resource))
        {
            return PermissionResult.Allowed;
        }
        
        return PermissionResult.AskUser;
    }
}
```

## Level 3: Curator Branch (IsCurator → AskHandler / GlobalACL)

When the callback returns `AskUser` or no callback is configured, the system branches based on curator status:

### Curator Path: IPermissionAskHandler

For the silicon curator, the system prompts the user for a decision via IM.

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        SendMessage($"Allow {resource}?");

        var response = WaitForResponse();

        return new AskPermissionResult
        {
            Allowed = response.Approved,
            AddToCache = response.AddToCache,
            CacheDuration = response.CacheDuration
        };
    }
}
```

### Non-Curator Path: GlobalACL → Default Deny

For non-curator beings, the system checks the Global Access Control List. If no matching rule is found, the request is denied by default.

#### GlobalACL Structure

```json
{
  "rules": [
    {
      "prefix": "network:api.github.com",
      "result": "Allowed"
    },
    {
      "prefix": "file:C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

Rules are evaluated in order; first match wins. Only the silicon curator can modify the global ACL.

#### Resource Format

```
{type}:{path}

Examples:
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## Audit System

All permission decisions are logged:

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
1. **UserFrequencyCache** - Check cached user decisions
2. **IPermissionCallback** - Custom callback evaluation
3. **Curator branch** - If curator, returns `AskUser` (needs confirmation); if non-curator, checks **GlobalACL**, then default deny

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
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "user-uuid",
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

### Revoke Permissions

```bash
curl -X DELETE http://localhost:8080/api/permissions/{rule-id}
```

### View Permissions

```bash
curl http://localhost:8080/api/permissions?userId=user-uuid
```

## Best Practices

### 1. Principle of Least Privilege

Grant only the minimum permissions required:

```json
{
  "resource": "disk:read",  // Not disk:*
  "allowed": true,
  "expiresAt": "2026-04-21T00:00:00Z"  // Always set expiration
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
    // Time-based permissions
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }
    
    // Resource-based permissions
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
2. Rate limit settings
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
