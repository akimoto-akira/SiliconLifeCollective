# Architecture

[中文](../zh-CN/architecture.md)

## Core Concepts

### Silicon Being

Each AI agent in the system is a **Silicon Being** — an autonomous entity with its own identity, personality, and capabilities. Every Silicon Being is driven by a **Soul File** (Markdown prompt) that defines its behavior patterns.

### Silicon Curator

The **Silicon Curator** is a special Silicon Being with the highest system privileges. It acts as the system administrator:

- Creates and manages other Silicon Beings
- Analyzes user requests and decomposes them into tasks
- Distributes tasks to appropriate Silicon Beings
- Monitors execution quality and handles failures
- Responds to user messages with **priority scheduling** (see below)

### Soul File

A Markdown file (`soul.md`) stored in each Silicon Being's data directory. It serves as the system prompt injected into every AI request, defining the being's personality, decision-making patterns, and behavioral constraints.

---

## Scheduling: Time-Sliced Fair Scheduling

### MainLoop + TickObject

The system runs a **tick-driven main loop** on a background thread:

```
MainLoop (infinite loop)
  └── TickObject A (Priority=0, Interval=100ms)
  └── TickObject B (Priority=1, Interval=500ms)
  └── SiliconBeingManager (Priority=0, Interval=100ms)
        └── Silicon Being 1 → Tick → ExecuteOneRound
        └── Silicon Being 2 → Tick → ExecuteOneRound
        └── Silicon Being 3 → Tick → ExecuteOneRound
        └── ...
```

Key design decisions:

- **Silicon Beings do NOT inherit TickObject.** They have their own `Tick()` method, but it is invoked by the SiliconBeingManager, not registered directly to the MainLoop.
- **SiliconBeingManager inherits TickObject** and acts as the single proxy for all beings.
- Each being's execution is limited to **one round** of AI request + ToolCalls per tick, ensuring no being can monopolize the main loop.

### Curator Priority Response

When the user sends a message to the Silicon Curator:

1. The current being (e.g., Being A) finishes its current round — **no interruption**.
2. The manager **skips the remaining queue**.
3. The loop **restarts from the Curator**, giving it immediate execution.

This ensures responsive user interaction without disrupting in-progress tasks.

---

## Component Architecture

```
┌─────────────────────────────────────────────────────────┐
│                        CoreHost                         │
│  (Unified host — assembles and manages all components)  │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ MainLoop │  │ ServiceLocator│  │      Config      │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │           SiliconBeingManager (TickObject)        │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Curator  │ │Being A  │ │Being B  │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Shared Services                      │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ChatSystem│  │ Storage  │  │  PermissionMgr  │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │ IAIClient│  │Executor  │  │   ToolManager   │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Executors                       │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │  Disk    │  │ Network  │  │  CommandLine    │  │   │
│  │  │Executor  │  │Executor  │  │  Executor       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              IM Providers                        │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Console  │  │  Web     │  │  Feishu / ...   │  │   │
│  │  │Provider  │  │Provider  │  │  Provider       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## Key Design Decisions

### Storage as Instance Class (not static)

`IStorage` is designed as an injectable instance, not a static utility. This ensures:

- Direct file system access — IStorage is the system's internal persistence channel and does **not** route through executors.
- **AI cannot control IStorage** — executors govern IO initiated by AI tools; IStorage governs the framework's own internal data reads/writes. These are fundamentally different concerns.
- Testable with mock implementations.
- Future support for different storage backends without modifying consumers.

### Executor as Security Boundary

Executors are the **only** path for I/O operations. Tools that need disk, network, or command-line access **must** go through executors. This design enforces:

- Each executor owns an **independent scheduling thread** with thread locking for permission verification.
- Centralized permission checking — the executor queries the being's **private PermissionManager**.
- Request queuing with priority support and timeout control.
- Audit logging for all external operations.
- Exception isolation — one executor's failure does not affect others.
- Circuit breaker — consecutive failures temporarily halt an executor to prevent cascading failures.

### ContextManager as Lightweight Object

Each `ExecuteOneRound()` creates a new `ContextManager` instance that:

1. Loads the Soul File + recent chat history.
2. Sends the request to the AI client.
3. Loops through ToolCalls until the AI returns plain text.
4. Persists the response to ChatSystem.
5. Disposes.

This keeps each round isolated and stateless.

### Self-Evolution via Class Rewriting

Silicon Beings can rewrite their own C# classes at runtime:

1. AI generates new class code (must inherit `SiliconBeingBase`).
2. **Compile-time reference control** (primary defense): the compiler is only given an allowed assembly list — `System.IO`, `System.Reflection`, etc. are excluded, so dangerous code is impossible at the type level.
3. **Runtime static analysis** (secondary defense): even after successful compilation, code is scanned for dangerous patterns.
4. Roslyn compiles the code in memory.
5. On success: replace the current instance, persist encrypted code to disk.
6. On failure: discard new code, keep the existing implementation.

Code is stored AES-256 encrypted on disk. The encryption key is derived from the being's GUID (uppercase) via PBKDF2.

---

## Web UI Architecture

### Skin System

The Web UI features a **pluggable skin system** that allows complete UI customization without changing application logic:

- **ISkin Interface** — Defines the contract for all skins, including:
  - Core rendering methods (`RenderHtml`, `RenderError`)
  - UI component library (buttons, inputs, cards, tables, etc.)
  - Theme CSS generation
  - Preview information for skin selector

- **Built-in Skins** — 4 production-ready skins:
  - **Admin** — Professional, data-focused interface for system administration
  - **Chat** — Conversational, message-centric design for AI interactions
  - **Creative** — Artistic, visually rich layout for creative workflows
  - **Dev** — Developer-focused, code-centric interface with syntax highlighting

- **Skin Discovery** — Automatic discovery and registration via reflection (`SkinManager.DiscoverSkins()`)

### Controller System

The Web UI follows a **MVC-like pattern** with 15 controllers handling different aspects:

| Controller | Purpose |
|------------|---------|
| About | About page and project information |
| Being | Silicon Being management and status |
| Chat | Real-time chat interface with SSE |
| CodeBrowser | Code viewing and editing |
| Config | System configuration |
| Dashboard | System overview and metrics |
| Executor | Executor status and management |
| Init | First-run initialization wizard |
| Knowledge | Knowledge graph visualization (placeholder) |
| Log | System log viewer |
| Memory | Long-term memory browser |
| Permission | Permission management |
| PermissionRequest | Permission request queue |
| Project | Project management (placeholder) |
| Task | Task system interface |

### Real-time Updates

- **SSE (Server-Sent Events)** — Push-based updates for chat messages, being status, and system events
- **No WebSocket Required** — Simpler architecture using SSE for most real-time needs
- **Automatic Reconnection** — Client-side reconnection logic for resilient connections

---

## Data Directory Structure

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # Curator's soul file
    │   ├── state.json       # Runtime state
    │   ├── code.enc         # AES-encrypted custom class code
    │   └── permission.enc   # AES-encrypted custom permission callback
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```
