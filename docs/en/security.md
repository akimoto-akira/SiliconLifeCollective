# Security Design

> **Version: v0.1.0-alpha**

[English](../en/security.md) | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [Español](../es-ES/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Deutsch](../de-DE/security.md) | [Čeština](../cs-CZ/security.md)

## Overview

Security in Silicon Life Collective is built on a **defense in depth** model. Core principle: **All I/O operations must go through executors**, and executors enforce permission checks before execution.

```
Tool Call → Executor → Permission Manager → High Deny Cache → High Allow Cache → Callback → Ask User
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

The silicon curator has the highest permission level (`IsCurator = true`). Permission checks for the curator are short-circuited to **Allowed**, unless the user explicitly overrides.

### Private Permission Manager

Each silicon being has its own **private PermissionManager** instance. Permission state is not shared between beings.

---

## Permission Verification Flow

Query priority is: **1. User High Deny → 2. User High Allow → 3. Callback Function**

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
                  │ 1. IsCurator?   │──Yes──▶ Allowed
                  └────────┬────────┘
                           │ No
                           ▼
                  ┌─────────────────┐
                  │ 2. User High    │──Match──▶ Denied
                  │ Deny Cache      │
                  └────────┬────────┘
                           │ No Match
                           ▼
                  ┌─────────────────┐
                  │ 3. User High    │──Match──▶ Allowed
                  │ Allow Cache     │
                  └────────┬────────┘
                           │ No Match
                           ▼
                  ┌─────────────────┐
                  │ 4. Permission   │
                  │ Callback        │──▶ Allowed / Denied / AskUser
                  └─────────────────┘
```

**Key Point**: Executors only see booleans (Allowed/Denied). The permission manager internally handles the ternary decision (Allowed/Denied/AskUser) and resolves AskUser before returning to the executor.

---

## Executors (Security Boundary)

Executors are the **only** path for I/O operations. They enforce:

### Independent Scheduling Threads

Each executor has its **own scheduling thread**:

- Thread isolation between executors — one executor's thread blocking doesn't affect others.
- Each executor can set independent resource limits (CPU, memory, etc.).
- Thread pool management for executor threads.

### Request Queues

Each executor maintains a request queue:

- Requests are routed to the appropriate executor by type.
- Priority queuing support.
- Timeout control per request.

### Thread Locking for Permission Verification

When a tool initiates resource access:

1. Executor receives the request and **locks its thread**.
2. Executor queries the being's private permission manager.
3. If callback returns AskUser, the executor thread **remains locked** waiting for user response.
4. The being only sees the final result (success or denial) — it never sees the intermediate "pending" or "waiting" state.
5. Only the silicon curator triggers real user prompts. Normal beings query the global ACL synchronously without blocking.
6. On timeout, the request is treated as denied, and the thread lock is released.

### Executor Types

| Executor | Scope | Default Timeout |
|----------|-------|-----------------|
| `DiskExecutor` | File read/write, directory operations | 30 seconds |
| `NetworkExecutor` | HTTP requests, WebSocket connections | 60 seconds |
| `CommandLineExecutor` | Shell command execution | 120 seconds |
| `DynamicCompilationExecutor` | Roslyn in-memory compilation | 60 seconds |

### Exception Isolation and Fault Tolerance

- Exceptions in one executor don't affect other executors.
- Automatic restart on thread crash.
- Circuit breakers: temporarily halt executor after consecutive failures to prevent cascade failures.

---

## Global ACL (Access Control List)

Shared rule table persisted to storage, managed only by the silicon curator:

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
- Only the silicon curator can modify the global ACL (via its dedicated tools).
- Changes take effect immediately.
- Global ACL is **not** in the priority chain per query mentioned above — it is referenced internally by the callback function.

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

All custom silicon being classes **must** inherit `SiliconBeingBase`. The compiler enforces this constraint at the type level.

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
- **Curator-only access** — `TokenAuditTool` (marked `[SiliconManagerOnly]`) allows curator to query and summarize token usage.
- **Web dashboard** — `AuditController` provides browser-based dashboard with trend charts and data export.
- **Persistent storage** — Records are stored via `ITimeStorage` for time-series queries and long-term analysis.
