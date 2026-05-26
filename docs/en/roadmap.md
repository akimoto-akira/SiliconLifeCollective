# Roadmap

> **Version: v0.2.0-alpha**

[**English**](../en/roadmap.md) | [Deutsch](../de-DE/roadmap.md) | [中文](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | [Español](../es-ES/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Čeština](../cs-CZ/roadmap.md) | [Русский](../ru-RU/roadmap.md)

## Dual-Version Roadmap

### SiliconLife.Default (Default Version)
- **Positioning**: Default implementation, primarily for validating architectural feasibility
- **Current Status**: Stages 1–10.6 completed, system running stably
- **Role**: Serves as the baseline implementation for architecture validation, ensuring the correctness and feasibility of the core architectural design

### SiliconLife.Fast (High-Performance Version)
- **Positioning**: Primary production version
- **Current Status**: Base architecture migration completed; SpeedyPack Storage Engine and Plugin System implemented
- **Role**: Built on the architecture validated by the Default version, with deep performance optimization and production-grade feature enhancements; the preferred choice for actual deployment

**Fast Version Development Plan**:
- ✅ Stage 1: Base project structure and Config System migration
- ✅ Stage 2: Web UI and Controller migration
- ✅ Stage 3: Storage System optimization (SpeedyPack in-memory storage + async persistence)
- ✅ Stage 3.5: SpeedyPack Manager (SiliconLife.Speedy.Manager Avalonia UI application)
- ✅ Stage 3.6: Plugin System (IPlugin interface, Security Sandbox, AssemblyLoadContext isolation)
- ✅ Stage 4: Avalonia windowed application (cross-platform desktop app, Windows/macOS system tray, Linux status window)

---

## Guiding Principles

Every stage ends with a **runnable, observable** system. No stage produces "a pile of infrastructure with nothing to show for it."

---

## ~~Stage 1: Can Chat~~ ✅ Completed

**Goal**: Console input → AI call → Console output. Minimum verifiable unit.

| # | Module | Description |
|---|--------|-------------|
| 1.1 | Solution and Project Structure | Create `SiliconLifeCollective.sln` with `src/SiliconLife.Core/` (core library) and `src/SiliconLife.Default/` (default implementation + entry point) |
| 1.2 | Config (Minimal) | Singleton + JSON deserialization. Read `config.json`. Auto-generate defaults if missing |
| 1.3 | Localization (Minimal) | `LocalizationBase` abstract class, `ZhCN` implementation. Add `Language` to config |
| 1.4 | Ollama Client (Minimal) | `IAIClient` interface, HTTP call to local Ollama `/api/chat`. No streaming yet, no Tool Call yet |
| 1.5 | Console I/O | `while(true) + Console.ReadLine()`, read input → call AI → print response |
| 1.6 | Copyright Header | Add Apache 2.0 header to all C# source files |

**Deliverable**: A console chat program that talks to a local Ollama model.

**Verification**: Run the program, type "hello", see AI response.

---

## ~~Stage 2: Has Skeleton~~ ✅ Completed

**Goal**: Replace the "bare loop" with a framework structure. Behavior unchanged.

| # | Module | Description |
|---|--------|-------------|
| 2.1 | Storage (Minimal) | `IStorage` interface (Read/Write/Exists/Delete, key-value). `FileSystemStorage` implementation. Instance class (not static). Direct file system access — **AI cannot control IStorage** |
| 2.2 | Main Loop + Tick Object | Infinite loop, precise clock interval (`Stopwatch` + `Thread.Sleep`). Priority scheduling |
| 2.3 | IAIClient Standardization | `IAIClientFactory` interface. Ollama Client refactored to implement the standard interface |
| 2.4 | Console Migration | Migrate `while(true)` to Main Loop-driven Tick Object. Behavior same as Stage 1 |

**Deliverable**: Main Loop runs Tick Objects, console chat still works.

**Verification**: Register a Test Tick Object that prints tick count every second; console chat still works.

---

## ~~Stage 3: Has Soul~~ ✅ Completed

**Goal**: The first Silicon Being lives within the framework.

| # | Module | Description |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Abstract base class with Id, Name, Tool Manager, AI Client, Chat Service, Storage, Permission Service. Abstract `Tick()` and `ExecuteOneRound()` |
| 3.2 | Soul File Loading | `SoulFileManager`: reads `soul.md` from the being's data directory |
| 3.3 | Context Manager (Minimal) | Concatenate Soul File + recent messages → call AI → get response. No Tool Call yet, no persistence yet |
| 3.4 | ISiliconBeingFactory | Factory interface for creating Silicon Being instances |
| 3.5 | Silicon Being Manager (Minimal) | Inherits Tick Object (priority=0). Iterates all beings, calls their Tick in sequence |
| 3.6 | DefaultSiliconBeing | Standard behavior implementation. Check unread messages → create Context Manager → ExecuteOneRound → output |
| 3.7 | Being Directory Structure | `DataDirectory/SiliconManager/{GUID}/`, containing `soul.md` and `state.json` |

**Deliverable**: A Silicon Being driven by the Main Loop, receiving console input, loading a Soul File, calling AI.

**Verification**: Console input → Main Loop tick triggers → being processes (with Soul File-guided behavior) → AI responds. Response style should differ from Stage 1.

---

## ~~Stage 4: Has Memory~~ ✅ Completed

**Goal**: Conversations persist across restarts.

| # | Module | Description |
|---|--------|-------------|
| 4.1 | Chat System | Channel concept (two GUIDs = one channel). Message model with persistence. No group chat yet |
| 4.2 | IM Provider + IM Manager | `IIMProvider` interface. `ConsoleProvider` as a formal IM channel. `IM Manager` routes messages |
| 4.3 | Context Manager Enhancement | Pull history from Chat System. Persist AI responses. Support multi-round Tool Call continuation |
| 4.4 | IMessage Model | Unified message model shared by Chat System and IM Manager |

**Deliverable**: A Chat System with persistent memory.

**Verification**: Chat a few rounds → exit → restart → ask "what did we talk about?" → being can answer.

---

## ~~Stage 5: Can Act (Tool System)~~ ✅ Completed

**Goal**: Silicon Beings can perform actions, not just chat.

| # | Module | Description |
|---|--------|-------------|
| 5.1 | ITool + Tool Result | `ITool` interface with Name, Description, Execute. `ToolResult` with Success, Message, Data |
| 5.2 | Tool Manager | Per-being instance. Reflection-based tool discovery. `[SiliconManagerOnly]` attribute support |
| 5.3 | IAIClient: Tool Call Support | Parse AI tool_calls. Loop: execute tool → send result back → AI continues → until plain text |
| 5.4 | Executor Base | Abstract base class with independent scheduling thread, request queue, timeout control |
| 5.5 | Network Executor | HTTP requests through Executor. Timeout, queuing |
| 5.6 | CommandLine Executor | Shell execution through Executor. Cross-platform separator detection |
| 5.7 | Disk Executor | File operations through Executor. No permission check yet (Stage 6) |
| 5.8–5.12 | Built-in Tools | Calendar Tool, System Tool, Network Tool, Chat Tool, Disk Tool |

**Deliverable**: Silicon Beings can invoke tools to perform actions.

**Verification**: Ask "what day is it" → Calendar Tool answers; ask "check processes" → System Tool executes; tell the being to message another being → Chat Tool works.

---

## ~~Stage 6: Follows Rules (Permission System)~~ ✅ Completed

**Goal**: Silicon Beings cannot access sensitive resources without authorization.

| # | Module | Description |
|---|--------|-------------|
| 6.1 | Permission Manager | Per-being private instance. Callback-based, ternary result (Allowed/Deny/AskUser). Query priority: HighDeny → HighAllow → Callback. IsCurator flag |
| 6.2 | Permission Type Enum | NetworkAccess, CommandLine, FileAccess, Function, DataAccess |
| 6.3 | DefaultPermissionCallback | Network whitelist/blacklist, CLI categorization, file path safety rules |
| 6.4 | Global ACL | Prefix-match rule table, persisted to storage |
| 6.5 | User Frequency Cache | HighAllow/HighDeny lists. User choice (not auto-detection). Prefix match, memory-only, configurable expiry |
| 6.6 | User Ask Mechanism (Console) | Console prompts y/n when AskUser is returned |
| 6.7 | Executor Permission Integration | All Executors check permissions before execution |
| 6.8 | IStorage Isolation Note | IStorage is internal system persistence — direct file access, **not** routed through Executors, **not** controllable by AI. Executors only manage IO initiated by AI tools |
| 6.9 | Audit Log | Log all permission decisions with timestamp, requester, resource, result |

**Deliverable**: Permission prompts appear when a being attempts sensitive operations.

**Verification**: Tell the being to delete a file → console shows permission prompt → type `n` → operation denied. Tell the being to access a whitelisted website → immediately allowed.

---

## ~~Stage 7: Can Evolve (Dynamic Compilation)~~ ✅ Completed

**Goal**: Silicon Beings can rewrite their own code.

| # | Module | Description |
|---|--------|-------------|
| 7.1 | Code Encryption | AES-256 encryption/decryption. PBKDF2 key derived from GUID |
| 7.2 | Dynamic Compilation Executor | Roslyn-based in-memory compilation sandbox. Compile-time assembly reference control (primary defense: exclude System.IO, Reflection, etc.) |
| 7.3 | Security Scanner | Runtime static analysis for dangerous code patterns (secondary defense). Block loading if scan fails |
| 7.4 | Being Lifecycle Enhancement | Load: decrypt → scan → compile → instantiate. Runtime: compile in memory → atomic replacement → persist encrypted |
| 7.5 | Silicon Curator | Curator abstract base class. IsCurator=true. Highest permissions |
| 7.6 | DefaultCurator | Default Curator implementation with built-in Soul File and management tools |
| 7.7 | Curator Tool | `[SiliconManagerOnly]` tool: list_beings, create_being, get_code, reset |
| 7.8 | Permission Callback Override | Beings can compile custom permission callbacks |
| 7.9 | Silicon Being Manager Enhancement | Replace method (runtime instance swap). MigrateState (transfer state between old and new instances) |

**Deliverable**: Silicon Beings can generate new code via AI, compile it, and replace themselves.

**Verification**: Tell the being "add a new feature to yourself" → observe compilation → restart → new feature works.

---

## ~~Stage 8: Memory and Planning~~ ✅ Completed

**Goal**: Long-term memory, task management, timed triggers.

| # | Module | Description |
|---|--------|-------------|
| 8.1 | FileSystemMemory | Short-term/long-term segmented storage. Time decay. Compression (merge similar memories). Multi-dimensional search |
| 8.2 | Task System | One-time + DAG dependency tasks. Priority scheduling. State tracking |
| 8.3 | Timer System | One-shot alarm + periodic timers. Millisecond precision. Persisted to storage |
| 8.4 | Incomplete Date | Fuzzy date range structure (e.g., "April 2026", "Spring 2026") |
| 8.5–8.7 | Memory/Task/Timer Tools | Tools for beings to query memories, manage tasks, set timers |

**Deliverable**: Beings can remember key points, create/track tasks, set alarms.

**Verification**: Create a task → check task list → set a 1-minute alarm → receive notification when time is up.

---

## ~~Stage 9: Framework Complete~~ ✅ Completed

**Goal**: Unified entry point, multi-being collaboration.

| # | Module | Description |
|---|--------|-------------|
| 9.1 | Core Host + Core Host Builder | Unified host using builder pattern. Graceful shutdown (Ctrl+C / SIGTERM) |
| 9.2 | Program.Main Refactoring | Migrate to Core Host Builder pattern |
| 9.3 | Silicon Being Manager Enhancement | Curator priority response. Exception isolation. Periodic persistence |
| 9.4 | Multi-Being Loading | Load multiple beings from data directory. Inter-being communication via Chat Tool |
| 9.5 | Performance Monitor | Per-Tick Object execution time tracking |
| 9.6 | ServiceLocator | Global service locator with Register/Get methods |

**Deliverable**: Multiple beings running simultaneously, collaborating, managed by Core Host.

**Verification**: Create two beings → A sends message to B → B receives and replies → framework schedules without errors. Curator responds with priority when user messages arrive.

---

## ~~Stage 10: Going Web~~ ✅ Completed

**Goal**: Migrate from console to browser interface.

| # | Module | Description |
|---|--------|-------------|
| 10.1 | Router | HTTP request router. Serialization parameter routing and static file serving |
| 10.2 | Controller Base | Request/response context. HTML and JSON response support |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | C# server-side builders. Zero frontend framework dependency |
| 10.6 | SSE (Server-Sent Events) | Push-based real-time updates for chat, being status, and system events. Simpler than WebSocket, with automatic client reconnection |
| 10.7 | WebUIProvider | SSE-based real-time IM channel. Replaces console as primary interface |
| 10.8 | Web Security | IP blacklist/whitelist. `[WebCode]` attribute. Dynamic updates |
| 10.9–10.17 | Web Controllers | Chat, Dashboard, Beings, Tasks, Permissions, Permission Requests, Executors, Logs, Config, Memory, Timers, Initialization, About, Code Browser, Knowledge, Projects, Audit |

**Deliverable**: A complete Web UI accessible from the browser.

**Verification**: Open browser → chat with a being → view dashboard → manage permissions → all features work.

---

## ~~Stage 10.5: Incremental Enhancement~~ ✅ Completed

**Goal**: Enhance the existing system with new features discovered during development.

| # | Module | Description |
|---|--------|-------------|
| 10.5.1 | Broadcast Channel | New session type for system-wide announcements. Fixed channel ID, dynamic subscription, pending message filtering |
| 10.5.2 | Chat Message Enhancement | ToolCallId, ToolCallsJson, Thinking fields for AI context; PromptTokens, CompletionTokens, TotalTokens for token tracking; SystemNotification message type |
| 10.5.3 | Token Usage Audit Manager | Per-request token consumption tracking across all beings. Aggregated statistics, time-series queries, persistent storage |
| 10.5.4 | Token Audit Tool | `[SiliconManagerOnly]` tool for Curator to query and summarize Token Usage |
| 10.5.5 | Config Tool | `[SiliconManagerOnly]` tool for Curator to read and modify system configuration |
| 10.5.6 | Audit Controller | Web dashboard for Token Usage Audit with trend charts and data export |
| 10.5.7 | Calendar System Extension | 32 calendar implementations covering world calendar systems (Buddhist, Chinese, Islamic, Hebrew, Japanese, Persian, Mayan, etc.) |
| 10.5.8 | Disk Tool Enhancement | New operations: count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all, list_drives |
| 10.5.9 | System Tool Enhancement | New operations: find_process (with wildcard support), resource_usage |
| 10.5.10 | Calendar Tool Enhancement | New operations: diff, list_calendars, get_components, get_now_components, convert (cross-calendar conversion) |
| 10.5.11 | DashScope Client | Alibaba Cloud Bailian AI client, compatible with OpenAI API. Supports streaming, Tool Call, reasoning content |
| 10.5.12 | DashScope Client Factory | Factory for creating Bailian clients. Dynamic model discovery via API. Multi-region support (Beijing, Virginia, Singapore, Hong Kong, Frankfurt) |
| 10.5.13 | AI Client Config System | Per-being AI client configuration. Dynamic config key options (model, region). Localized display names |
| 10.5.14 | Localization Extension | Simplified Chinese, Traditional Chinese, English, and Japanese localization for Bailian config options, model names, and region names |

**Deliverable**: Enhanced tools, observability, calendar coverage, and multi-AI backend support.

**Verification**: Curator queries Token Usage via Token Audit Tool → audit dashboard shows trends → Calendar Tool converts dates across 32 calendar systems → switch AI backend to Bailian → chat with Qwen model via cloud API.

---

## ~~Stage 10.6: Refinement and Optimization~~ ✅ Completed

**Goal**: Refine system functionality, add new features, optimize user experience.

| # | Module | Description |
|---|--------|-------------|
| 10.6.1 | WebViewBrowserTool | Playwright-based cross-platform browser automation tool, supporting headless mode, per-being isolation, full JS/CSS support |
| 10.6.2 | HelpTool | Help documentation system tool, supporting multi-language document queries and display |
| 10.6.3 | ProjectWorkNoteTool | Project work note tool, supporting project-dimension work records and management |
| 10.6.4 | ProjectTaskTool | Project task management tool, supporting task assignment and progress tracking |
| 10.6.5 | KnowledgeTool | Knowledge Network tool, supporting CRUD operations on triple-structured knowledge and path discovery |
| 10.6.6 | ChatHistoryController | Chat history viewer controller, supporting session list and message details |
| 10.6.7 | CodeHoverController | Code hover tooltip controller, supporting syntax highlighting and code hints |
| 10.6.8 | WorkNoteController | Work note management controller, supporting search and directory generation |
| 10.6.9 | TimerExecutionHistory | Timer execution history feature, recording and viewing timer trigger history |
| 10.6.10 | Localization Extension | Added Czech (cs-CZ) localization support, totaling 21 language variants |
| 10.6.11 | Web UI Optimization | File upload support, loading indicators, Tool Call rendering optimization, work note modal fix |
| 10.6.12 | Memory Management Enhancement | Advanced filtering, statistics, detail view, compression algorithm optimization |
| 10.6.13 | Logging System Refactoring | System/Silicon Being log separation, log read API, Silicon Being filter |
| 10.6.14 | Permission System Enhancement | Permission callback pre-compilation validation, assembly reference validation, wttr.in weather service whitelist |

**Deliverable**: Complete WebView browser automation, help documentation system, project workspace, Knowledge Network, chat history viewer, and other enhancements.

**Verification**: Silicon Being can operate a browser via WebViewBrowserTool → get help docs via HelpTool → manage project work notes and tasks → query Knowledge Network → view chat history.

---

## ~~Stage 10.7: Project Collaboration and Workflow~~ ✅ Completed

**Goal**: Add project workspace, workflow engine, memory fade mechanism, and tool permission system.

| # | Module | Description |
|---|--------|-------------|
| 10.7.1 | Project Role Management | ProjectTool adds assign_role, remove_role, list_roles operations |
| 10.7.2 | Workflow Engine | WorkflowEngine core engine, supporting template definition, state transitions, Tick-driven execution |
| 10.7.3 | Workflow Template | WorkflowTemplate base class, defining state sets and transition rules |
| 10.7.4 | Workflow Instance | WorkflowInstance management, bound to a specific project, tracking current state |
| 10.7.5 | Workflow Log | WorkflowLog records state transition history |
| 10.7.6 | Memory Fade Mechanism | MemoryFadeService timed decay service, automatically decaying and archiving memories by importance every hour |
| 10.7.7 | Tool Permission System | Two-level tool permissions (Silicon Being level + project level), permission templates, operation-granularity control |
| 10.7.8 | ToolPermissionController | Tool permission management Web controller |
| 10.7.9 | ProjectWorkTool | Project work operation tool ([SiliconManagerOnly], [ToolScenario(Project)]) |
| 10.7.10 | Tool Scenario System | ToolScenarioAttribute and ChatOnlyAttribute, supporting Chat/Task/Timer/MemoryCompression/Project scenario filtering |
| 10.7.11 | Localization Extension | Added Russian, Portuguese, Italian, Dutch, Polish, Swedish localization, totaling 34 language variants |

**Deliverable**: Complete project collaboration system, workflow engine, memory fade mechanism, and tool permission management.

**Verification**: Create project → assign roles → bind workflow template → beings collaborate within project space → memories automatically decay and archive → tool permission isolation takes effect.

---

## Stage 11: External IM Integration

**Goal**: Connect to external messaging platforms for broader user accessibility.

| # | Module | Description |
|---|--------|-------------|
| 11.1 | FeishuProvider | Feishu (Lark) bot integration with card support |
| 11.2 | WhatsAppProvider | WhatsApp Business API integration |
| 11.3 | TelegramProvider | Telegram Bot API integration with inline keyboard support |
| 11.4 | IM Manager Enhancement | Multi-provider routing, unified message format, cross-platform permission ask handling |

**Deliverable**: Users can interact with Silicon Beings through external IM platforms.

---

## Stage 12: Advanced Features

**Goal**: Optional advanced features for enhanced capabilities.

| # | Module | Description |
|---|--------|-------------|
| 12.1 | ~~Knowledge Network~~ ✅ Completed | Triple-structured (subject-predicate-object) knowledge graph, supporting CRUD, path discovery, advanced queries, and graph traversal |
| 12.2 | ~~Plugin System~~ ✅ Completed | External plugin loading with security checks and sandbox (IPlugin interface, Plugin Loader, AssemblyLoadContext isolation) |
| 12.3 | Skill Ecosystem | Reusable skill marketplace for being capabilities |
