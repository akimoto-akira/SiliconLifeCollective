# Roadmap

[English](../en/roadmap.md) | [中文](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | [Español](../es-ES/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Deutsch](../de-DE/roadmap.md) | [Čeština](../cs-CZ/roadmap.md)

## Guiding Principles

Each phase ends with a **working, observable** system. No phase produces "a bunch of infrastructure with nothing to show."

---

## ~~Phase 1: Can Chat~~ ✅ Completed

**Goal**: Console input → AI call → Console output. Minimal verifiable unit.

| # | Module | Description |
|---|--------|-------------|
| 1.1 | Solution and Project Structure | Create `SiliconLifeCollective.sln`, containing `src/SiliconLife.Core/` (core library) and `src/SiliconLife.Default/` (default implementation + entry point) |
| 1.2 | Configuration (Minimal) | Singleton + JSON deserialization. Reads `config.json`. Auto-generates defaults if missing |
| 1.3 | Localization (Minimal) | `LocalizationBase` abstract class, `ZhCN` implementation. Add `Language` to configuration |
| 1.4 | OllamaClient (Minimal) | `IAIClient` interface, HTTP call to local Ollama `/api/chat`. No streaming, no tool calls yet |
| 1.5 | Console I/O | `while(true) + Console.ReadLine()`, read input → call AI → print response |
| 1.6 | Copyright Header | Add Apache 2.0 header to all C# source files |

**Deliverable**: Console chat program that converses with local Ollama model.

**Verification**: Run program, type "hello", see AI response.

---

## ~~Phase 2: Has Skeleton~~ ✅ Completed

**Goal**: Replace "bare loop" with framework structure. Behavior unchanged.

| # | Module | Description |
|---|--------|-------------|
| 2.1 | Storage (Minimal) | `IStorage` interface (Read/Write/Exists/Delete, key-value pairs). `FileSystemStorage` implementation. Instance class (not static). Direct file system access — **AI cannot control IStorage** |
| 2.2 | Main Loop + Clock Object | Infinite loop, precise clock interval (`Stopwatch` + `Thread.Sleep`). Priority scheduling |
| 2.3 | IAIClient Standardization | `IAIClientFactory` interface. OllamaClient refactored to implement standard interface |
| 2.4 | Console Migration | Migrate `while(true)` to main loop-driven clock object. Same behavior as Phase 1 |

**Deliverable**: Main loop running clock, console chat still works.

**Verification**: Register a test clock object, print clock count every second; console chat still works.

---

## ~~Phase 3: Has Soul~~ ✅ Completed

**Goal**: First silicon being alive in the framework.

| # | Module | Description |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Abstract base class with Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. Abstract `Tick()` and `ExecuteOneRound()` |
| 3.2 | Soul File Loading | `SoulFileManager`: Reads `soul.md` from being's data directory |
| 3.3 | ContextManager (Minimal) | Connects soul file + recent messages → calls AI → gets response. No tool calls, no persistence yet |
| 3.4 | ISiliconBeingFactory | Factory interface for creating being instances |
| 3.5 | SiliconBeingManager (Minimal) | Inherits clock object (priority=0). Iterates all beings, calls their Tick in sequence |
| 3.6 | DefaultSiliconBeing | Standard behavior implementation. Checks unread messages → creates ContextManager → ExecuteOneRound → outputs |
| 3.7 | Being Directory Structure | `DataDirectory/SiliconManager/{GUID}/`, containing `soul.md` and `state.json` |

**Deliverable**: Silicon being driven by main loop, receives console input, loads soul file, calls AI.

**Verification**: Console input → main loop clock triggers → being processes (with soul file guided behavior) → AI response. Response style should differ from Phase 1.

---

## ~~Phase 4: Has Memory~~ ✅ Completed

**Goal**: Conversations persist across restarts.

| # | Module | Description |
|---|--------|-------------|
| 4.1 | ChatSystem | Channel concept (two GUIDs = one channel). Message model with persistence. No group chat yet |
| 4.2 | IIMProvider + IMManager | `IIMProvider` interface. `ConsoleProvider` as formal IM channel. `IMManager` routes messages |
| 4.3 | ContextManager Enhancement | Pulls history from chat system. Persists AI responses. Supports multi-turn tool call continuation |
| 4.4 | IMessage Model | Unified message model shared by chat system and IM manager |

**Deliverable**: Chat system with persistent memory.

**Verification**: Chat a few rounds → exit → restart → ask "what did we talk about?" → being can answer.

---

## ~~Phase 5: Can Act (Tool System)~~ ✅ Completed

**Goal**: Silicon beings can perform actions, not just chat.

| # | Module | Description |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | `ITool` interface with Name, Description, Execute. `ToolResult` with Success, Message, Data |
| 5.2 | ToolManager | Per-being instance. Reflection-based tool discovery. `[SiliconManagerOnly]` attribute support |
| 5.3 | IAIClient: Tool Call Support | Parses AI tool_calls. Loop: execute tool → send results back → AI continues → until plain text |
| 5.4 | Executor Base Class | Abstract base class with independent dispatch thread, request queue, timeout control |
| 5.5 | NetworkExecutor | HTTP requests via executor. Timeout, queuing |
| 5.6 | CommandLineExecutor | Shell execution via executor. Cross-platform separator detection |
| 5.7 | DiskExecutor | File operations via executor. No permission checks yet (Phase 6) |
| 5.8–5.12 | Built-in Tools | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Deliverable**: Silicon beings can call tools to perform actions.

**Verification**: Ask "what day is today" → CalendarTool answers; ask "check processes" → SystemTool executes; tell being to send message to another being → ChatTool works.

---

## ~~Phase 6: Follows Rules (Permission System)~~ ✅ Completed

**Goal**: Silicon beings cannot access sensitive resources without authorization.

| # | Module | Description |
|---|--------|-------------|
| 6.1 | PermissionManager | Per-being private instance. Callback-based, ternary result (Allowed/Deny/AskUser). Query priority: HighDeny → HighAllow → Callback. IsCurator flag |
| 6.2 | PermissionType Enum | NetworkAccess, CommandLine, FileAccess, Function, DataAccess |
| 6.3 | DefaultPermissionCallback | Network whitelist/blacklist, CLI classification, file path security rules |
| 6.4 | GlobalACL | Prefix matching rule table, persisted to storage |
| 6.5 | UserFrequencyCache | HighAllow/HighDeny list. User choice (not auto-detected). Prefix matching, memory-only, configurable expiration |
| 6.6 | UserAskMechanism (Console) | Console prompt y/n when returns AskUser |
| 6.7 | Executor Permission Integration | All executors check permissions before execution |
| 6.8 | IStorage Isolation Note | IStorage is internal system persistence — direct file access, **not** routed through executor, **not** controllable by AI. Executors only manage IO initiated by AI tools |
| 6.9 | Audit Log | Records all permission decisions with timestamp, requester, resource, result |

**Deliverable**: Permission prompt when being attempts sensitive operation.

**Verification**: Tell being to delete file → console shows permission prompt → input `n` → operation denied. Tell being to access whitelisted website → immediately allowed.

---

## ~~Phase 7: Can Evolve (Dynamic Compilation)~~ ✅ Completed

**Goal**: Silicon beings can rewrite their own code.

| # | Module | Description |
|---|--------|-------------|
| 7.1 | CodeEncryption | AES-256 encryption/decryption. PBKDF2 key derived from GUID |
| 7.2 | DynamicCompilationExecutor | Roslyn-based in-memory compilation sandbox. Compile-time assembly reference control (primary defense: excludes System.IO, Reflection, etc.) |
| 7.3 | Security Scanning | Runtime static analysis for dangerous code patterns (secondary defense). Prevents loading if scan fails |
| 7.4 | Being Lifecycle Enhancement | Load: decrypt → scan → compile → instantiate. Runtime: compile in memory → atomic replacement → persist encrypted |
| 7.5 | SiliconCurator | Curator abstract base class. IsCurator=true. Highest privilege |
| 7.6 | DefaultCurator | Default curator implementation with built-in soul file and management tools |
| 7.7 | CuratorTool | `[SiliconManagerOnly]` tool: list_beings, create_being, get_code, reset |
| 7.8 | Permission Callback Override | Beings can compile custom permission callbacks |
| 7.9 | SiliconBeingManager Enhancement | Replace method (runtime instance swap). MigrateState (transfer state between old and new instances) |

**Deliverable**: Silicon beings can generate new code via AI, compile and replace themselves.

**Verification**: Tell being "add a new feature to yourself" → observe compilation → restart → new feature works.

---

## ~~Phase 8: Memory and Planning~~ ✅ Completed

**Goal**: Long-term memory, task management, timed triggers.

| # | Module | Description |
|---|--------|-------------|
| 8.1 | FileSystemMemory | Short/long-term segmented storage. Time decay. Compression (merge similar memories). Multi-dimensional search |
| 8.2 | TaskSystem | One-time + DAG dependency tasks. Priority scheduling. State tracking |
| 8.3 | TimerSystem | One-time alarms + periodic timers. Millisecond precision. Persisted to storage |
| 8.4 | IncompleteDate | Fuzzy date range structure (e.g., "April 2026", "Spring 2026") |
| 8.5–8.7 | Memory/Task/Timer Tools | Tools for beings to query memory, manage tasks, set timers |

**Deliverable**: Beings can remember key points, create/track tasks, set alarms.

**Verification**: Create task → check task list → set 1-minute alarm → receive notification when time expires.

---

## ~~Phase 9: Framework Complete~~ ✅ Completed

**Goal**: Unified entry point, multi-being collaboration.

| # | Module | Description |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | Unified host using builder pattern. Graceful shutdown (Ctrl+C / SIGTERM) |
| 9.2 | Program.Main Refactoring | Migrate to CoreHostBuilder pattern |
| 9.3 | SiliconBeingManager Enhancement | Curator-first response. Exception isolation. Periodic persistence |
| 9.4 | Multi-Being Loading | Load multiple beings from data directory. Inter-being communication via ChatTool |
| 9.5 | Performance Monitoring | Per-clock object execution time tracking |
| 9.6 | ServiceLocator | Global service locator with Register/Get methods |

**Deliverable**: Multiple beings running simultaneously, collaborating, managed by CoreHost.

**Verification**: Create two beings → A sends message to B → B receives and replies → framework scheduling without errors. Curator responds first when user messages arrive.

---

## ~~Phase 10: Moving to Web~~ ✅ Completed

**Goal**: Migrate from console to browser interface.

| # | Module | Description |
|---|--------|-------------|
| 10.1 | Router | HTTP request router. Sequence parameter routing and static file serving |
| 10.2 | Controller Base Class | Request/response context. HTML and JSON response support |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | C# server-side builders. Zero frontend framework dependency |
| 10.6 | SSE (Server-Sent Events) | Push-style real-time updates for chat, being status, and system events. Simpler than WebSocket with automatic client reconnection |
| 10.7 | WebUIProvider | SSE-based real-time IM channel. Replaces console as primary interface |
| 10.8 | Web Security | IP blacklist/whitelist. `[WebCode]` attribute. Dynamic updates |
| 10.9–10.17 | Web Controllers | Chat, dashboard, beings, tasks, permissions, permission requests, executors, logs, configuration, memory, timers, initialization, about, code browser, knowledge, projects, audit |

**Deliverable**: Complete Web UI accessible from browser.

**Verification**: Open browser → chat with being → view dashboard → manage permissions → all features working.

---

## ~~Phase 10.5: Incremental Enhancement~~ ✅ Completed

**Goal**: Enhance existing system with new features discovered during development.

| # | Module | Description |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | New session type for system-wide announcements. Fixed channel ID, dynamic subscription, pending message filtering |
| 10.5.2 | ChatMessage Enhancement | ToolCallId, ToolCallsJson, Thinking fields for AI context; PromptTokens, CompletionTokens, TotalTokens for token tracking; SystemNotification message type |
| 10.5.3 | TokenUsageAuditManager | Token consumption tracking per request across all beings. Aggregated statistics, time-series queries, persistent storage |
| 10.5.4 | TokenAuditTool | `[SiliconManagerOnly]` tool for curator to query and aggregate token usage |
| 10.5.5 | ConfigTool | `[SiliconManagerOnly]` tool for curator to read and modify system configuration |
| 10.5.6 | AuditController | Web dashboard for token usage audit with trend charts and data export |
| 10.5.7 | Calendar System Expansion | 32 calendar implementations covering world calendar systems (Buddhist, Lunar, Islamic, Hebrew, Japanese, Persian, Mayan, etc.) |
| 10.5.8 | DiskTool Enhancement | New operations: count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all, list_drives |
| 10.5.9 | SystemTool Enhancement | New operations: find_process (supports wildcards), resource_usage |
| 10.5.10 | CalendarTool Enhancement | New operations: diff, list_calendars, get_components, get_now_components, convert (cross-calendar conversion) |
| 10.5.11 | DashScopeClient | Alibaba Cloud Bailian AI client, OpenAI API compatible. Supports streaming, tool calls, reasoning content |
| 10.5.12 | DashScopeClientFactory | Factory for creating Bailian clients. Dynamic model discovery via API. Multi-region support (Beijing, Virginia, Singapore, Hong Kong, Frankfurt) |
| 10.5.13 | AI Client Configuration System | Per-being AI client configuration. Dynamic configuration key options (model, region). Localized display names |
| 10.5.14 | Localization Expansion | Simplified Chinese, Traditional Chinese, English, and Japanese localization for Bailian configuration options, model names, and region names |

**Deliverable**: Enhanced tools, observability, calendar coverage, and multi-AI backend support.

**Verification**: Curator queries token usage via TokenAuditTool → audit dashboard shows trends → CalendarTool converts dates across 32 calendar systems → switch AI backend to Bailian → chat with Qwen model via cloud API.

---

## ~~Phase 10.6: Refinement and Optimization~~ ✅ Completed

**Goal**: Refine system functionality, add new features, optimize user experience.

| # | Module | Description |
|---|--------|-------------|
| 10.6.1 | WebViewBrowserTool | Cross-platform browser automation tool based on Playwright, supporting headless mode, individual isolation, full JS/CSS support |
| 10.6.2 | HelpTool | Help documentation system tool, supporting multi-language documentation query and display |
| 10.6.3 | ProjectWorkNoteTool | Project work notes tool, supporting project-dimensional work recording and management |
| 10.6.4 | ProjectTaskTool | Project task management tool, supporting task assignment and progress tracking |
| 10.6.5 | KnowledgeTool | Knowledge network tool, supporting CRUD and path discovery for triple knowledge |
| 10.6.6 | ChatHistoryController | Chat history view controller, supporting session list and message details |
| 10.6.7 | CodeHoverController | Code hover hint controller, supporting syntax highlighting and code hints |
| 10.6.8 | WorkNoteController | Work notes management controller, supporting search and directory generation |
| 10.6.9 | TimerExecutionHistory | Timer execution history feature, recording and viewing timer trigger history |
| 10.6.10 | Localization Expansion | Added Czech (cs-CZ) localization support, totaling 21 language variants |
| 10.6.11 | Web UI Optimization | File upload support, loading indicators, tool call rendering optimization, work note modal fix |
| 10.6.12 | Memory Management Enhancement | Advanced filtering, statistics, detail view, compression algorithm optimization |
| 10.6.13 | Logging System Refactoring | System/silicon being log separation, log read API, being filter |
| 10.6.14 | Permission System Enhancement | Permission callback pre-compilation validation, assembly reference validation, wttr.in weather service whitelist |

**Deliverable**: Complete WebView browser automation, help documentation system, project workspace, knowledge network, chat history view, and enhanced features.

**Verification**: Silicon beings can operate browsers via WebViewBrowserTool → access help documentation via HelpTool → manage project work notes and tasks → query knowledge network → view chat history.

---

## Phase 11: External IM Integration

**Goal**: Connect to external messaging platforms for broader user accessibility.

| # | Module | Description |
|---|--------|-------------|
| 11.1 | FeishuProvider | Feishu (Lark) bot integration, supporting cards |
| 11.2 | WhatsAppProvider | WhatsApp Business API integration |
| 11.3 | TelegramProvider | Telegram Bot API integration, supporting inline keyboards |
| 11.4 | IMManager Enhancement | Multi-provider routing, unified message format, cross-platform permission ask handling |

**Deliverable**: Users can interact with silicon beings through external IM platforms.

---

## Phase 12: Advanced Features

**Goal**: Optional advanced features for enhanced functionality.

| # | Module | Description |
|---|--------|-------------|
| 12.1 | Knowledge Network | Shared knowledge graph using triple structure (subject-predicate-object) |
| 12.2 | Plugin System | External plugin loading with security checks and sandboxing |
| 12.3 | Skills Ecosystem | Reusable skills marketplace for being capabilities |
