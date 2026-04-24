# Permission System

## Overview

The permission system ensures all AI-initiated operations are properly validated and audited.

## 5-Level Permission Chain

```
┌─────────────────────────────────────────────┐
│          Permission Validation              │
├─────────────────────────────────────────────┤
│  Level 1: IsCurator                         │
│  ↓ Bypass if true                           │
│  Level 2: UserFrequencyCache                │
│  ↓ Rate limiting                            │
│  Level 3: GlobalACL                         │
│  ↓ Access control list                      │
│  Level 4: IPermissionCallback               │
│  ↓ Custom logic                             │
│  Level 5: IPermissionAskHandler             │
│  ↓ Ask user                                 │
│  Result: Allowed or Denied                  │
└─────────────────────────────────────────────┘
```

## Level 1: IsCurator

Manager/curator bypasses all permission checks.

```csharp
if (user.IsCurator)
{
    return PermissionResult.Allowed("Curator access");
}
```

## Level 2: UserFrequencyCache

Rate limiting per user to prevent abuse.

```csharp
var cache = new UserFrequencyCache();
if (!cache.CheckLimit(userId, resource))
{
    return PermissionResult.Denied("Rate limit exceeded");
}
```

## Level 3: GlobalACL

Global Access Control List defines explicit rules.

### ACL Structure

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

### Resource Format

```
{type}:{action}

Examples:
- disk:read
- disk:write
- network:http
- compile:execute
- system:info
```

## Level 4: IPermissionCallback

Custom callback for dynamic permission logic.

### DefaultPermissionCallback Default Implementation

`DefaultPermissionCallback` provides comprehensive default permission rules, including:

#### Network Access Rules
- **Loopback addresses**: Allow localhost, 127.0.0.1, ::1
- **Private IP addresses**:
  - 192.168.x.x (Class C) - Allowed
  - 10.x.x.x (Class A) - Allowed
  - 172.16-31.x.x (Class B) - Ask User
- **Domain whitelist**:
  - Search engines: Google, Bing, DuckDuckGo, Yandex, Sogou, etc.
  - AI services: OpenAI, Anthropic, HuggingFace, Ollama, etc.
  - Developer services: GitHub, StackOverflow, npm, NuGet, etc.
  - Social media: Weibo, Zhihu, Reddit, Discord, etc.
  - Video platforms: YouTube, Bilibili, Douyin, TikTok, etc.
  - **Weather information**: wttr.in
  - Government websites: .gov, .go.jp, .go.kr
- **Domain blacklist**:
  - AI impersonation sites: chatgpt, openai, deepseek, etc. (fake domains)
  - Malicious AI tools: wormgpt, darkgpt, fraudgpt, etc.
  - AI content farm and black market related domains

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public async Task<PermissionResult> CheckAsync(PermissionRequest request)
    {
        // Custom logic here
        if (IsSafeOperation(request))
        {
            return PermissionResult.Allowed("Safe operation");
        }
        
        return PermissionResult.Undecided("Needs user confirmation");
    }
}
```

## Level 5: IPermissionAskHandler

Ask the user for permission when all other levels are undecided.

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public async Task<AskPermissionResult> AskAsync(PermissionRequest request)
    {
        // Send message to user via IM
        await SendMessageAsync($"Allow {request.Resource}?");
        
        // Wait for user response
        var response = await WaitForResponseAsync();
        
        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
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

The `PermissionManager.EvaluatePermission()` method provides read-only pre-evaluation of permissions without triggering user prompts. This is used by the `PermissionTool` to let AI check permission states before attempting operations.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**Returns**: Three-state `PermissionResult`:
- `Allowed` - Operation is permitted
- `Denied` - Operation is blocked
- `AskUser` - User confirmation will be needed at execution time

**Evaluation Order**:
1. **Frequency Cache** - Check cached user decisions
2. **IPermissionCallback** - Custom callback evaluation
3. **Curator Status** - If curator, returns `AskUser` (needs confirmation)
4. **Global ACL** - Check access control rules
5. **Default** - Deny if no matching rule

> **Note**: Unlike the full permission chain, `EvaluatePermission` does **not** invoke `IPermissionAskHandler`. It only reports what the result *would be* at execution time.

## Managing Permissions

### Grant Permission

**Via Web UI**:
1. Navigate to **Permission Management**
2. Click **Add Rule**
3. Configure:
   - User
   - Resource
   - Allowed/Denied
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

### Revoke Permission

```bash
curl -X DELETE http://localhost:8080/api/permissions/{rule-id}
```

### View Permissions

```bash
curl http://localhost:8080/api/permissions?userId=user-uuid
```

## Best Practices

### 1. Principle of Least Privilege

Grant only the minimum permissions needed:

```json
{
  "resource": "disk:read",  // Not disk:*
  "allowed": true,
  "expiresAt": "2026-04-21T00:00:00Z"  // Always set expiry
}
```

### 2. Use Time-Limited Permissions

Never grant permanent permissions unless absolutely necessary.

### 3. Monitor Permission Logs

Regularly review the audit logs for:
- Denied access attempts
- Unusual patterns
- Permission escalations

### 4. Implement Custom Callbacks

For complex logic, use `IPermissionCallback`:

```csharp
public async Task<PermissionResult> CheckAsync(PermissionRequest request)
{
    // Time-based permissions
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied("Outside business hours");
    }
    
    // Resource-based permissions
    if (IsSensitiveResource(request.Resource))
    {
        return PermissionResult.Undecided("Requires approval");
    }
    
    return PermissionResult.Allowed();
}
```

## Common Scenarios

### Scenario 1: AI Wants to Read File

```
AI: "I need to read config.json"
↓
Permission Chain:
1. IsCurator? No
2. Rate limit? OK
3. GlobalACL? Found rule: disk:read = Allowed
4. Result: Allowed
```

### Scenario 2: AI Wants to Execute Code

```
AI: "I want to compile and run code"
↓
Permission Chain:
1. IsCurator? No
2. Rate limit? OK
3. GlobalACL? No rule found
4. Callback? Returns Undecided
5. Ask User? User approves
6. Result: Allowed
```

### Scenario 3: Rate Limit Exceeded

```
AI: "I need to make 100 HTTP requests"
↓
Permission Chain:
1. IsCurator? No
2. Rate limit? EXCEEDED
3. Result: Denied
```

## Troubleshooting

### Permission Denied Unexpectedly

**Check**:
1. User's IsCurator status
2. Rate limit settings
3. GlobalACL rules
4. Callback logic
5. User response timeout

### Permission Not Expiring

**Check**:
- `expiresAt` field is set correctly
- Timezone is correct
- Clock synchronization

### Audit Logs Not Recording

**Check**:
- Audit logger is registered
- Storage backend is accessible
- Sufficient disk space

## Next Steps

- 📚 Read the [Architecture Guide](architecture.md)
- 🛠️ Check the [Development Guide](development-guide.md)
- 🔒 Review the [Security Documentation](security.md)
- 🚀 See the [Getting Started Guide](getting-started.md)
