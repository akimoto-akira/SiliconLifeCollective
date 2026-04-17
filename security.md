# Security Design

[中文](docs/zh-CN/security.md) | [繁體中文](docs/zh-HK/security.md)

## Overview

Security in Silicon Life Collective is built on a **layered defense** model. The core principle: **all I/O operations must pass through executors**, and executors enforce permission checks before execution.

```
Tool Call → Executor → PermissionManager → HighDeny Cache → HighAllow Cache → Callback → AskUser
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

Every permission check returns one of three results:

| Result | Behavior |
|--------|----------|
| **Allowed** | Operation proceeds immediately |
| **Denied** | Operation is blocked, audit log recorded |
| **AskUser** | Operation is paused, user confirmation required |

### Special Role: Silicon Curator

The Silicon Curator holds the highest privilege level (`IsCurator = true`). Permission checks for the Curator are short-circuited to **Allowed** unless explicitly overridden by the user.

### Private Permission Managers

Each Silicon Being has its own **private PermissionManager** instance. Permission states are not shared between beings.

---

## Permission Verification Flow

The query priority is: **1. User HighDeny → 2. User HighAllow → 3. Callback Function**

```
┌─────────────┐
│ Tool Call   │
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Executor   │────▶│ Private Permission  │
│ (Disk/Net/  │     │ Manager (per-being) │
│  CLI/...)   │     └────────┬────────────┘
└─────────────┘            │
                           ▼
                  ┌─────────────────┐
                  │ 1. IsCurator?   │──Yes──▶ Allowed
                  └────────┬────────┘
                           │ No
                           ▼
                  ┌─────────────────┐
                  │ 2. User HighDeny│──Match──▶ Denied
                  │ (memory cache)  │
                  └────────┬────────┘
                           │ Miss
                           ▼
                  ┌─────────────────┐
                  │ 3. User HighAllow│──Match──▶ Allowed
                  │ (memory cache)  │
                  └────────┬────────┘
                           │ Miss
                           ▼
                  ┌─────────────────┐
                  │ 4. Permission   │
                  │  Callback Func  │──▶ Allowed / Denied / AskUser
                  └─────────────────┘
```

**Key point**: The executor only sees a boolean (Allowed/Denied). The PermissionManager internally handles the three-way decision (Allowed/Denied/AskUser) and resolves AskUser before returning to the executor.

---

## Executors (Security Boundary)

Executors are the **only** path for I/O operations. They enforce:

### Independent Scheduling Thread

Each executor owns an **independent scheduling thread**:

- Threads are isolated between executors — one executor's thread block does not affect others.
- Each executor can set independent resource limits (CPU, memory, etc.).
- Thread pool management for executor threads.

### Request Queue

Each executor maintains a request queue:

- Requests are routed to the corresponding executor by type.
- Supports priority queuing.
- Timeout control per request.

### Thread Locking for Permission Verification

When a tool initiates a resource access:

1. The executor receives the request and **locks its thread**.
2. The executor queries the being's private PermissionManager.
3. If the callback returns AskUser, the executor thread **remains locked** waiting for the user's response.
4. The being only sees the final result (success or denied) — it never sees the intermediate "pending" or "waiting" state.
5. Only the Silicon Curator triggers real user prompts. Ordinary beings query the Global ACL synchronously without blocking.
6. On timeout, the request is treated as denied and the thread lock is released.

### Executor Types

| Executor | Scope | Default Timeout |
|----------|-------|-----------------|
| `DiskExecutor` | File read/write, directory operations | 30s |
| `NetworkExecutor` | HTTP requests, WebSocket connections | 60s |
| `CommandLineExecutor` | Shell command execution | 120s |
| `DynamicCompilationExecutor` | Roslyn in-memory compilation | 60s |

### Exception Isolation & Fault Tolerance

- One executor's exception does not affect other executors.
- Auto-restart on thread crash.
- Circuit breaker: temporarily halt an executor after consecutive failures to prevent cascading failures.

---

## Global ACL (Access Control List)

A shared rule table persisted to storage, managed exclusively by the Silicon Curator:

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
- The Global ACL is **not** in the per-query priority chain above — it is referenced by the callback function internally.

---

## User Frequency Cache

To reduce repetitive permission prompts, the system maintains two **per-being, memory-only** caches:

| Cache | Purpose |
|-------|---------|
| **HighAllow** | Resources the user has frequently allowed |
| **HighDeny** | Resources the user has frequently denied |

### How It Works

- **User-opted, not auto-detected**: When AskUser is triggered, the user chooses whether to add the resource to the cache.
- **Prefix matching**: Supports resource path prefix matching (e.g., `network:api.example.com/*`).
- **Priority**: HighDeny has higher priority than HighAllow.
- **Memory-only**: Caches are not persisted. They are lost on restart.
- **Configurable expiration**: Users can set cache entry validity duration.

### Cache Update Flow

1. Permission callback returns `AskUser`.
2. Permission system sends an inquiry to the card system (Web UI or IM).
3. User makes a decision (Allow/Deny) and **chooses whether to cache**.
4. Card system returns the decision + cache flag.
5. Permission system updates the corresponding cache list.
6. Future requests matching the cached prefix are resolved immediately.

---

## User Ask Mechanism

When a permission check returns `AskUser`:

### Web UI: Interactive Card

The web frontend immediately displays an **interactive card** showing:

- Resource type and path
- Operation description
- Allow / Deny buttons
- Optional "Always allow" / "Always deny" checkbox (adds to frequency cache)

### IM (No Card Support): Random Code

For messaging platforms that don't support interactive cards:

1. System generates two random 6-digit codes: an **allow code** and a **deny code**.
2. Sends a message containing the resource info and both codes.
3. User must reply with the exact allow code to authorize. Any other reply is treated as denial.
4. Codes are single-use to prevent replay attacks.

### Timeout

- A timeout is set for all AskUser requests.
- On timeout, the request is treated as **denied** and the executor thread lock is released.

---

## Dynamic Compilation Security

Self-evolution (class rewriting) introduces unique security risks. The system mitigates them with a **layered strategy**:

### Layer 1: Compile-Time Reference Control (Primary Defense)

- The compiler is only given an **allowed assembly reference list**.
- **Allowed**: `System.Runtime`, `System.Private.CoreLib`, project assemblies (ITool interfaces, etc.)
- **Blocked**: `System.IO`, `System.Reflection`, `System.Runtime.InteropServices`, etc.
- If the code references a blocked assembly, the **compiler itself rejects** the code.
- This is more reliable than runtime scanning — dangerous operations are impossible at the type level.

### Layer 2: Runtime Static Analysis (Secondary Defense)

- Even after successful compilation, the code undergoes static pattern scanning.
- Detects dangerous operation patterns (direct I/O, system calls, etc.).
- If dangerous code is found, loading is rejected and the system falls back to the default function.

### Inheritance Constraint

All custom Silicon Being classes **must** inherit `SiliconBeingBase`. The compiler enforces this at the type level.

### Encrypted Storage

Compiled code is stored AES-256 encrypted on disk:

- **Key derivation**: PBKDF2 from the being's GUID (uppercase).
- **Decryption failure**: Falls back to the default implementation.
- **Runtime recompilation**: New code is compiled in memory first; only persisted after successful compilation and instance replacement.

### Atomic Replacement

The replacement process is atomic:

1. Compile new code in memory → get `Type`.
2. Create new instance from `Type`.
3. Migrate state from old instance to new instance.
4. Swap references.
5. Persist encrypted code.

If any step fails, the old instance remains active.

---

## Permission Callback Function

### Design

Each PermissionManager holds a **callback function variable**:

- **Default**: Points to the built-in default permission function.
- **After dynamic compilation**: Overridden by the being's custom permission function.
- **One-or-the-other**: Only one callback is active at any time.
- **Compilation failure**: Does not affect the current callback — the default or last successful custom function remains in effect.

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

Logs are persisted to storage and viewable via the Web UI (Log Controller).
