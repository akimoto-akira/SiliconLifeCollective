# Roadmap

[中文](../zh-CN/roadmap.md)

## Guiding Principle

Every phase ends with a **runnable, observable** system. No phase produces "a pile of infrastructure with nothing to show."

---

## Phase 1: Can Chat

**Goal**: Console input → AI call → Console output. Minimum verifiable unit.

| # | Module | Description |
|---|--------|-------------|
| 1.1 | Solution & project structure | Create `SiliconLifeCollective.sln` with `src/SiliconLife.Core/` (core library) and `src/SiliconLife.Default/` (default implementation + entry point) |
| 1.2 | Config (minimal) | Singleton + JSON deserialization. Read `config.json`. Auto-generate defaults if missing |
| 1.3 | Localization (minimal) | `LocalizationBase` abstract class, `ZhCN` implementation. Add `Language` to config |
| 1.4 | OllamaClient (minimal) | `IAIClient` interface, HTTP call to local Ollama `/api/chat`. No streaming, no ToolCall yet |
| 1.5 | Console I/O | `while(true) + Console.ReadLine()`, read input → call AI → print response |
| 1.6 | Copyright headers | Add Apache 2.0 headers to all C# source files |

**Deliverable**: A console chat program that talks to a local Ollama model.

**Verification**: Run the program, type "hello", see an AI response.

---

## Phase 2: Has Skeleton

**Goal**: Replace the "bare loop" with a framework structure. Behavior unchanged.

| # | Module | Description |
|---|--------|-------------|
| 2.1 | Storage (minimal) | `IStorage` interface (Read/Write/Exists/Delete, key-value). `FileSystemStorage` implementation. Instance class (not static). Direct file system access — **AI cannot control IStorage** |
| 2.2 | MainLoop + TickObject | Infinite loop with precise tick intervals (`Stopwatch` + `Thread.Sleep`). Priority scheduling |
| 2.3 | IAIClient standardization | `IAIClientFactory` interface. OllamaClient refactored to implement standard interface |
| 2.4 | Console migration | Migrate `while(true)` to MainLoop-driven TickObject. Behavior identical to Phase 1 |

**Deliverable**: MainLoop running ticks, console chat still works.

**Verification**: Register a test TickObject that prints tick count every second; console chat still works.

---

## Phase 3: Has a Soul

**Goal**: The first Silicon Being is alive within the framework.

| # | Module | Description |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Abstract base class with Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. Abstract `Tick()` and `ExecuteOneRound()` |
| 3.2 | Soul file loading | `SoulFileManager`: read `soul.md` from being's data directory |
| 3.3 | ContextManager (minimal) | Concatenate soul file + recent messages → call AI → get response. No ToolCall, no persistence yet |
| 3.4 | ISiliconBeingFactory | Factory interface for creating being instances |
| 3.5 | SiliconBeingManager (minimal) | Inherits TickObject (Priority=0). Iterates all beings, calls their Tick in sequence |
| 3.6 | DefaultSiliconBeing | Standard behavior implementation. Check unread messages → create ContextManager → ExecuteOneRound → output |
| 3.7 | Being directory structure | `DataDirectory/SiliconManager/{GUID}/` with `soul.md` and `state.json` |

**Deliverable**: Silicon Being driven by MainLoop, receiving console input, loading soul file, calling AI.

**Verification**: Console input → MainLoop Tick → Being processes (with soul-file-guided behavior) → AI response. Response style should differ from Phase 1.

---

## Phase 4: Has Memory

**Goal**: Conversations persist across restarts.

| # | Module | Description |
|---|--------|-------------|
| 4.1 | ChatSystem | Channel concept (two GUIDs = one channel). Message model with persistence. No group chat yet |
| 4.2 | IIMProvider + IMManager | `IIMProvider` interface. `ConsoleProvider` as formal IM channel. `IMManager` routes messages |
| 4.3 | ContextManager enhanced | Pull history from ChatSystem. Persist AI responses. Support multi-round ToolCall continuation |
| 4.4 | IMessage model | Unified message model shared by ChatSystem and IMManager |

**Deliverable**: A chat system with persistent memory.

**Verification**: Chat for a few turns → exit → restart → ask "what did we talk about?" → being can answer.

---

## Phase 5: Can Act (Tool System)

**Goal**: Silicon Beings can execute operations, not just talk.

| # | Module | Description |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | `ITool` interface with Name, Description, Execute. `ToolResult` with Success, Message, Data |
| 5.2 | ToolManager | Per-being instance. Reflection-based tool discovery. `[SiliconManagerOnly]` attribute support |
| 5.3 | IAIClient: ToolCall support | Parse AI tool_calls. Loop: execute tool → send result back → AI continues → until plain text |
| 5.4 | Executor base class | Abstract base with independent scheduling thread, request queue, timeout control |
| 5.5 | NetworkExecutor | HTTP requests through executor. Timeout, queuing |
| 5.6 | CommandLineExecutor | Shell execution through executor. Cross-platform separator detection |
| 5.7 | DiskExecutor | File operations through executor. No permission check yet (Phase 6) |
| 5.8–5.12 | Built-in tools | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Deliverable**: Silicon Beings can call tools to perform operations.

**Verification**: Ask "what day is it" → CalendarTool answers; ask "check processes" → SystemTool executes; tell being to message another being → ChatTool works.

---

## Phase 6: Follows Rules (Permission System)

**Goal**: Silicon Beings cannot access sensitive resources without authorization.

| # | Module | Description |
|---|--------|-------------|
| 6.1 | PermissionManager | Per-being private instance. Callback-based, ternary results (Allowed/Deny/AskUser). Query priority: HighDeny → HighAllow → Callback. IsCurator flag |
| 6.2 | PermissionType enum | NetworkAccess, CommandLine, FileAccess, Function, DataAccess |
| 6.3 | DefaultPermissionCallback | Network whitelist/blacklist, CLI classification, file path safety rules |
| 6.4 | GlobalACL | Prefix-matching rule table, persisted to Storage |
| 6.5 | UserFrequencyCache | HighAllow/HighDeny lists. User-opted (not auto-detected). Prefix matching, memory-only, configurable expiration |
| 6.6 | UserAskMechanism (console) | Console prompt for y/n when AskUser is returned |
| 6.7 | Executor permission integration | All executors check permissions before execution |
| 6.8 | IStorage isolation note | IStorage is system-internal persistence — direct file access, **not** routed through executors, **not** controllable by AI. Executors only govern IO initiated by AI tools |
| 6.9 | Audit logging | Log all permission decisions with timestamp, requester, resource, result |

**Deliverable**: Permission prompts appear when beings attempt sensitive operations.

**Verification**: Tell being to delete a file → console shows permission prompt → type `n` → operation denied. Tell being to access whitelisted site → allowed immediately.

---

## Phase 7: Can Evolve (Dynamic Compilation)

**Goal**: Silicon Beings can rewrite their own code.

| # | Module | Description |
|---|--------|-------------|
| 7.1 | CodeEncryption | AES-256 encrypt/decrypt. PBKDF2 key derivation from GUID |
| 7.2 | DynamicCompilationExecutor | Roslyn-based in-memory compilation sandbox. Compile-time assembly reference control (primary defense: exclude System.IO, Reflection, etc.) |
| 7.3 | Security scanning | Runtime static analysis for dangerous code patterns (secondary defense). Loading blocked if scan fails |
| 7.4 | Being lifecycle enhancement | Load: decrypt → scan → compile → instantiate. Runtime: compile in memory → atomic replace → persist encrypted |
| 7.5 | SiliconCurator | Curator abstract base class. IsCurator=true. Highest privileges |
| 7.6 | DefaultCurator | Default curator implementation with built-in soul file and management tools |
| 7.7 | CuratorTool | `[SiliconManagerOnly]` tools: list_beings, recompile, get_code, update_soul, scan_code |
| 7.8 | Permission callback override | Beings can compile custom permission callbacks |
| 7.9 | SiliconBeingManager enhancement | Replace method (runtime instance swap). MigrateState (transfer state between old and new instances) |

**Deliverable**: Silicon Beings can generate new code via AI, compile it, and replace themselves.

**Verification**: Tell a being "add a new feature to yourself" → observe compilation → restart → new feature works.

---

## Phase 8: Remembers and Plans

**Goal**: Long-term memory, task management, scheduled triggers.

| # | Module | Description |
|---|--------|-------------|
| 8.1 | FileSystemMemory | Short/long-term staged storage. Time decay. Compression (merge similar memories). Multi-dimensional search |
| 8.2 | TaskSystem | One-shot + DAG dependency tasks. Priority scheduling. Status tracking |
| 8.3 | TimerSystem | One-shot alarms + periodic timers. Millisecond precision. Persisted to Storage |
| 8.4 | IncompleteDate | Fuzzy date range struct (e.g., "April 2026", "Spring 2026") |
| 8.5–8.7 | Memory/Task/Timer Tools | Tools for beings to query memories, manage tasks, set timers |

**Deliverable**: Beings can remember key points, create/track tasks, set alarms.

**Verification**: Create a task → check task list → set a 1-minute alarm → receive notification when time is up.

---

## Phase 9: Framework Complete

**Goal**: Unified entry point, multi-being collaboration.

| # | Module | Description |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | Unified host with Builder pattern. Graceful shutdown (Ctrl+C / SIGTERM) |
| 9.2 | Program.Main refactor | Migrate to CoreHostBuilder pattern |
| 9.3 | SiliconBeingManager enhancement | Curator priority response. Exception isolation. Periodic persistence |
| 9.4 | Multi-being loading | Load multiple beings from data directory. Inter-being communication via ChatTool |
| 9.5 | Performance monitoring | Per-TickObject execution time tracking |
| 9.6 | ServiceLocator | Global service locator with Register/Get methods |

**Deliverable**: Multiple beings running simultaneously, collaborating, managed by CoreHost.

**Verification**: Create two beings → A sends message to B → B receives and replies → framework schedules without errors. Curator responds with priority when user messages arrive.

---

## Phase 10: Goes Web

**Goal**: Migrate from console to browser interface.

| # | Module | Description |
|---|--------|-------------|
| 10.1 | Router | HTTP request router. Sequence-param routes and static file serving |
| 10.2 | Controller base | Request/response context. HTML and JSON response support |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | C# server-side builders. Zero frontend framework dependency |
| 10.6 | WebSocket | Real-time message push. Shared port with HTTP. JSON message format |
| 10.7 | WebUIProvider | WebSocket-based real-time IM channel. Replaces console as primary interface |
| 10.8 | Web security | IP blacklist/whitelist. `[WebCode]` attribute. Dynamic updates |
| 10.9–10.16 | Web controllers | Chat, Dashboard, Being, Task, Permission, Executor, Log, Config pages |

**Deliverable**: Full web UI accessible from browser.

**Verification**: Open browser → chat with beings → view dashboard → manage permissions → all functional.

---

## Phase 11: External IM

**Goal**: Connect to external messaging platforms.

| # | Module | Description |
|---|--------|-------------|
| 11.1 | FeishuProvider | Feishu (Lark) bot integration |
| 11.2 | WhatsAppProvider | WhatsApp Business API integration |
| 11.3 | TelegramProvider | Telegram Bot API integration |
| 11.4 | IMManager enhancement | Multi-provider routing, unified message format |

**Deliverable**: Users can interact with Silicon Beings via external IM platforms.

---

## Phase 12: Extras

**Goal**: Optional advanced features.

| # | Module | Description |
|---|--------|-------------|
| 12.1 | Knowledge Network | Shared knowledge graph using triple structure |
| 12.2 | Plugin system | External plugin loading with security checks |
| 12.3 | Skills ecosystem | Reusable skill marketplace |
| 12.4 | Calendar system | Multi-calendar support (Gregorian, Chinese, Islamic, etc.) |
