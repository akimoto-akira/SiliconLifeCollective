# Security Design

> **Version: v0.2.0-alpha**

[**English**](../en/security.md) | [Deutsch](../de-DE/security.md) | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [Español](../es-ES/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Čeština](../cs-CZ/security.md) | [Русский](../ru-RU/security.md)

## Overview

Security in Silicon Life Collective is built on a **defense in depth** model. Core principle: **All I/O operations must go through executors**, and executors enforce permission checks before execution.

```
Tool Call → Executor → Permission Manager → Frequency Cache → Callback → (IsCurator: Ask User | Non-curator: Global ACL)
```

---

## Permission Model

### Permission Types

| Type | Description |
|------|-------------|
| `NetworkAccess` | Outbound HTTP/HTTPS requests |
| `CommandLine` | Shell command execution |
| `FileAccess` | File and directory operations |
| `Function` | Sensitive function calls |
| `DataAccess` | Access to system or user data |

### Permission Results

Each permission check returns one of three outcomes:

| Result | Behavior |
|--------|----------|
| **Allowed** | Operation proceeds immediately |
| **Denied** | Operation is blocked, audit log recorded |
| **AskUser** | Operation pauses, requires user confirmation |

### Special Role: Silicon Curator

The Silicon Curator has the highest permission level (`IsCurator = true`). When the permission chain reaches the branch decision, the curator's operations are routed through `IPermissionAskHandler` to ask the user for confirmation, rather than being short-circuited to Allowed. Non-curator beings query the Global ACL instead.

### Private Permission Manager

Each Silicon Being has its own **private PermissionManager** instance. Permission state is not shared between beings.

---

## Permission Verification Flow

Query priority is: **1. Frequency Cache → 2. Callback Function → 3. Branch Decision (IsCurator/GlobalACL)**

```
┌─────────────┐
│  Tool Call   │
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Executor    │────▶│ Private Permission  │
│ (Disk/Net/   │     │ Manager (per being)  │
│  CLI...)     │     └────────┬────────────┘
└─────────────┘            │
                           ▼
                  ┌─────────────────┐
                  │ 1. Frequency    │──Match──▶ Allowed / Denied
                  │    Cache        │
                  │ (HighDeny takes │
                  │  priority over  │
                  │  HighAllow)     │
                  └────────┬────────┘
                           │ No Match
                           ▼
                  ┌─────────────────┐
                  │ 2. Permission   │──▶ Allowed / Denied / AskUser
                  │    Callback     │
                  └────────┬────────┘
                           │ AskUser
                           ▼
                  ┌─────────────────┐
                  │ 3. IsCurator?   │
                  └────────┬────────┘
                           │
                 ┌─────────┴─────────┐
                 │                   │
                 ▼ Yes               ▼ No
          ┌─────────────┐    ┌─────────────┐
          │ Ask User    │    │ Global ACL  │
          │ (AskHandler)│    │ Query Rules │
          └─────────────┘    └─────────────┘
```

**Key Point**: Executors only see booleans (Allowed/Denied). The Permission Manager internally handles the ternary decision (Allowed/Denied/AskUser) and resolves AskUser before returning to the executor.

---

## Executors (Security Boundary)

Executors are the **only** path for I/O operations. They enforce:

### Independent Scheduling Thread

Each executor has an **independent scheduling thread**:

- Thread isolation between executors — one executor's thread blocking does not affect other executors.
- Each executor can set independent resource limits (CPU, memory, etc.).
- Thread pool management for executor threads.

### Request Queue

Each executor maintains a request queue:

- Requests are routed to the corresponding executor by type.
- Priority queuing is supported.
- Per-request timeout control.

### Thread Locking for Permission Verification

When a tool initiates resource access:

1. Executor receives the request and **locks its thread**.
2. Executor queries the being's private Permission Manager.
3. If the callback returns AskUser, the executor thread **remains locked** waiting for user response.
4. The being only sees the final result (success or denial) — it never sees the intermediate "pending" or "waiting" state.
5. Only the Silicon Curator triggers real user prompts. Normal beings query the Global ACL synchronously without blocking.
6. On timeout, the request is treated as Denied, and the thread lock is released.

### Executor Types

| Executor | Scope | Default Timeout |
|----------|-------|-----------------|
| `DiskExecutor` | File read/write, directory operations | 30 seconds |
| `NetworkExecutor` | HTTP requests, WebSocket connections | 60 seconds |
| `CommandLineExecutor` | Shell command execution | 120 seconds |

> **Note**: `DynamicCompilationExecutor` (located in the `SiliconLife.Core.Compilation` namespace) handles Roslyn in-memory compilation and is not an I/O executor, but is still subject to the permission system.

### Exception Isolation and Fault Tolerance

- Exceptions in one executor don't affect other executors.
- Automatic restart on thread crash.
- Circuit breakers: temporarily halt executor after consecutive failures to prevent cascade failures.

---

## Global ACL (Access Control List)

Shared rule table persisted to storage, managed only by the Silicon Curator:

```json
{
  "rules": [
    { "prefix": "network:api.github.com", "result": "Allowed" },
    { "prefix": "file:C:\\Windows", "result": "Denied" },
    { "prefix": "cli:rm -rf", "result": "Denied" }
  ]
}
```

- Rules are evaluated in order; first match wins.
- Only the Silicon Curator can modify the Global ACL (via its dedicated tools).
- Changes take effect immediately.
- The Global ACL is **not** in the per-query priority chain described above — it is referenced internally by the callback function.

---

## User Frequency Cache

To reduce repetitive permission prompts, the system maintains two **per-being, memory-only** caches:

| Cache | Purpose |
|-------|---------|
| **HighAllow** | Resources frequently allowed by user |
| **HighDeny** | Resources frequently denied by user |

### How It Works

- **User choice, not auto-detection**: When AskUser is triggered, the user chooses whether to add the resource to cache.
- **Prefix matching**: Supports resource path prefix matching (e.g., `network:api.example.com/*`).
- **Priority**: HighDeny takes priority over HighAllow.
- **Memory-only**: Caches are not persisted. Lost on restart.
- **Configurable expiration**: Users can set validity period for cache entries.

### Cache Update Flow

1. Permission callback returns `AskUser`.
2. Permission system sends query to card system (Web UI or IM).
3. User makes decision (Allow/Deny) and **chooses whether to cache**.
4. Card system returns decision + cache flag.
5. Permission system updates the appropriate cache list.
6. Future requests matching cached prefixes are resolved immediately.

---

## User Ask Mechanism

When a permission check returns `AskUser`:

### Web UI: Interactive Cards

The web frontend immediately displays an **interactive card** showing:

- Resource type and path
- Operation description
- Allow / Deny buttons
- Optional "Always Allow" / "Always Deny" checkbox (add to frequency cache)

### IM (No Card Support): Random Codes

For messaging platforms that don't support interactive cards:

1. System generates two random 6-digit codes: **Allow Code** and **Deny Code**.
2. Sends message with resource information and both codes.
3. User must reply with the exact allow code to authorize. Any other reply is treated as denial.
4. Codes are single-use to prevent replay attacks.

### Timeout

- Timeout is set for all AskUser requests.
- On timeout, the request is treated as **Denied**, and the executor thread lock is released.

---

## Dynamic Compilation Security

Self-evolution (class override) introduces unique security risks. The system mitigates them with a **layered strategy**:

### Layer 1: Compile-Time Reference Control (Primary Defense)

- Compiler only receives a **list of allowed assembly references**.
- **Allowed**: `System.Runtime`, `System.Private.CoreLib`, project assemblies (ITool interface, etc.)
- **Blocked**: `System.IO`, `System.Reflection`, `System.Runtime.InteropServices`, etc.
- If code references a blocked assembly, **the compiler itself rejects** the code.
- This is more reliable than runtime scanning — dangerous operations are impossible at the type level.

### Layer 2: Runtime Static Analysis (Secondary Defense)

- Even after successful compilation, code is scanned for static patterns.
- Detects dangerous operation patterns (direct I/O, system calls, etc.).
- If dangerous code is found, loading is rejected and the system falls back to default functionality.

### Inheritance Constraint

All custom Silicon Being classes **must** inherit `SiliconBeingBase`. The compiler enforces this constraint at the type level.

### Encrypted Storage

Compiled code is stored on disk encrypted with AES-256:

- **Key derivation**: From being's GUID (uppercase) using PBKDF2.
- **Decryption failure**: Falls back to default implementation.
- **Runtime recompilation**: New code is compiled in memory first; only persisted after successful compilation and instance replacement.

### Atomic Replacement

The replacement process is atomic:

1. Compile new code in memory → get `Type`.
2. Create new instance from `Type`.
3. Migrate state from old instance to new instance.
4. Swap references.
5. Persist encrypted code.

If any step fails, the old instance remains alive.

---

## Permission Callback Functions

### Design

Each PermissionManager holds a **callback function variable**:

- **Default**: Points to built-in default permission function.
- **After dynamic compilation**: Overridden by the being's custom permission function.
- **One or the other**: Only one callback is active at any time.
- **Compilation failure**: Doesn't affect current callback — default or last successful custom function remains effective.

### Callback Signature

```
PermissionResult Callback(PermissionType type, string resourcePath, Guid callerId)
```

Returns `Allowed`, `Denied`, or `AskUser`.

---

## Audit Logging

All permission decisions are logged:

```
[2026-04-01 15:30:25] ALLOWED  | Being:AssistantA | Type:NetworkAccess | Resource:api.github.com | Source:HighAllowCache
[2026-04-01 15:30:26] DENIED   | Being:AssistantB | Type:FileAccess    | Resource:C:\Windows\System32 | Source:HighDenyCache
[2026-04-01 15:30:27] ASK_USER | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:Callback
[2026-04-01 15:30:28] ALLOWED  | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:UserDecision
```

Logs are persisted to storage and viewable via Web UI (Log Controller).

---

## Token Usage Audit

`TokenUsageAuditManager` provides AI token consumption tracking related to security:

- **Per-request logging** — Each AI call records being ID, model, prompt tokens, completion tokens, and timestamp.
- **Anomaly detection** — Unusual token consumption patterns may indicate prompt injection or resource abuse.
- **Curator-only access** — `TokenAuditTool` (marked `[SiliconManagerOnly]`) allows the Silicon Curator to query and summarize token usage.
- **Web dashboard** — `UsageController` provides browser-based dashboard with trend charts and data export.
- **Persistent storage** — Records are stored via `ITimeStorage` for time-series queries and long-term analysis.

---

## Plugin Security

The plugin system introduces security risks from third-party code execution, mitigated through the following mechanisms:

### Security Sandbox & Capability Declaration

`PluginLoader` performs security scanning when loading plugins and simultaneously supports the capability declaration mechanism:

1. **Declarable Capabilities** — Plugins declare required capabilities via the `[PluginCapability]` attribute:
   - `Network` — Network access (allows references to `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`)
   - `FileIO` — File read/write (allows references to `System.IO`)
   - `Process` — Process management
   - `AI` — AI calls

2. **Non-Declarable Capabilities** — The following capabilities are always blocked:
   - P/Invoke (`System.Runtime.InteropServices`)
   - Unsafe code (`System.Runtime.CompilerServices.Unsafe`)
   - Reflection Emit (`System.Reflection.Emit`)
   - Compiler API (`Microsoft.CodeAnalysis`)

3. **Trusted Assembly Whitelist** — References to the following assemblies are allowed:
   - `Google.Protobuf`, `Newtonsoft.Json`, `MessagePack`
   - `Serilog`, `Microsoft.Extensions.Logging.Abstractions`
   - `Dapper`

4. **Forbidden Type Check** — Scans for dangerous types referenced in plugins

5. **Forbidden Member Check** — Scans for dangerous methods called in plugins

### Isolated Loading

- Each plugin is loaded in isolation using a custom `AssemblyLoadContext`
- Types and assemblies between plugins do not interfere with each other
- Resources can be released when a plugin is unloaded

### Tool Permission Constraints

- Tools registered by plugins via the `ITool` interface are subject to the same permission system constraints
- Plugin tools cannot bypass the permission verification chain
- Plugin tools are subject to `[SiliconManagerOnly]` attribute constraints

---

## Tool Permission Security

The tool permission system provides an additional security layer controlling which tool operations Silicon Beings can use:

### Two-Level Permission Isolation

1. **Silicon Being Level** — Each Silicon Being has independent tool permission configuration
2. **Project Level** — Tool permissions within a project space are independent of the Silicon Being level, achieving permission isolation between projects

### Permission Templates

The system provides predefined permission templates to ensure a security baseline:

- **readonly** — Minimal permissions, only allows read operations
- **restricted** — Restricted permissions, only allows basic operations
- **full** — Full permissions (only used by the Silicon Curator)

### Security Features

- **Default Deny** — Tool operations not explicitly allowed are denied by default
- **Operation Granularity** — Each operation of each tool is independently controlled (e.g., `network:get` allowed but `network:post` denied)
- **Curator Management** — Tool permissions can only be configured by the Silicon Curator
- **Audit Trail** — Tool permission changes are recorded in the audit log

---

## Skill Security

The Skill system reuses the tool permission framework and provides multiple layers of guardrails:

### Execution Permissions

- Skill id serves as the tool name, included in the `ToolActionPermissionConfig` permission matrix with the `execute` action
- Disabled skills do not appear in AI-visible tool definitions (Schema-layer filtering + runtime re-check dual safeguard)
- The Silicon Curator can always execute; normal beings require `IsActionAllowed(skillId, "execute")`

### Tool Whitelist and Permission Union

- During skill execution, only tools within the `ToolWhitelist` are allowed (empty list = inherit all being tools)
- The skill's action restrictions and being permissions are merged on the **strict side** (`MergePermissions`): skills can only further narrow permissions, never widen them
- Tool calls outside the whitelist fail directly (`Tool not in whitelist`)

### Resource Consumption Guardrails

- **Global switch**: `SkillEnabled` disables the entire Skill system with one click
- **Quantity quota**: Custom skills per being are limited by `MaxCustomSkillsPerBeing` (default 50)
- **Round clamping**: `maxToolRound = Min(skill declared value, GlobalMaxToolRound default 10)`, preventing runaway loops
- **Timeout clamping**: `timeout = Min(skill declared value, GlobalSkillTimeoutSeconds default 300s)`
- **Recursion protection**: A skill cannot invoke itself during execution

### Modification Permissions

- The Silicon Curator can modify all skills; normal beings can only modify skills with source `Being`/`User`
- Automatic metadata completion only fills in missing fields; user-provided fields are never overwritten by AI

---

## MCP Security

MCP integration follows the principle of "user sovereignty + permission consistency":

### User Sovereignty

- Adding, deleting, starting, stopping, and reconnecting MCP servers **can only be done by the user through the Web UI** (/mcp or configuration page)
- The AI-side `mcp` tool is read-only query (status/list_servers/list_tools) and cannot modify the server list
- The `McpEnabled` global switch can cut off all external tools with one click

### Tool Isolation and Permissions

- Wrapper tools are named `mcp_{serverId}_{toolName}`, isolated from the built-in/plugin tool namespace
- Each wrapper tool automatically declares a single `execute` action, included in the two-level tool permission matrix, and can be individually disabled per being/project
- When a server is disabled, its tools are immediately unregistered from all beings

### Transport and Process Boundary

- `stdio` servers run as subprocesses, inheriting only explicitly configured environment variables (`env` field)
- `http` servers communicate via configured endpoints; connection failures automatically enter error status and expose `lastError`

---

## IM Key Security

### Environment Variable Placeholders

IM platform configuration values support `${ENV_VAR}` placeholders (e.g., `"${FEISHU_APP_SECRET}"`):

- `ConfigSecretResolver` resolves placeholders on a **deep copy**, so the original `config.json` always retains placeholders as-is
- Subsequent `SaveConfig` calls never write resolved plaintext secrets back to disk
- Supports both whole-value placeholders and inline-embedded placeholders (e.g., `prefix-${VAR}`)

### OAuth Authorization Security

- **CSRF protection via state**: 16-byte cryptographic random number, strictly validated on callback
- **5-minute timeout**: Authorization sessions are automatically invalidated on timeout; old sessions are immediately cancelled when overwritten
- **Token storage**: accessToken/refreshToken/tokenExpiresAt are written back to the platform configuration and persisted, with `authMode` marked as `oauth`
- Callback URL supports `redirectBaseUrl` configuration (for public callback scenarios)

### Message Security

- Feishu: Signature verification (`X-Lark-Signature`, SHA256) + AES-256-CBC event decryption + event deduplication (10-minute window)
- WeChat Enterprise: WXBizMsgCrypt encryption/decryption and signature verification
- DingTalk: Stream mode uses encrypted WebSocket; HTTP mode performs callback verification
