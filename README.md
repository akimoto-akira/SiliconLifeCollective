# Silicon Life Collective

**⚠️ WARNING: Dynamic compilation has not been thoroughly tested, but it does work. Templates must be added in the code for it to function properly.**

**⚠️ WARNING: Web UI is in rapid adjustment period, page functionality may not work properly.**

A .NET 9 multi-agent collaboration platform where AI agents called **Silicon Beings** self-evolve through Roslyn dynamic compilation.

[中文文档](docs/zh-CN/README.md)

## Features

- **Multi-Agent Orchestration** — Managed by a *Silicon Curator* with tick-driven time-sliced fair scheduling (MainLoop + TickObject + Watchdog + Circuit Breaker)
- **Soul File Driven** — Each Silicon Being is driven by a core prompt file (`soul.md`) that defines its personality and behavior
- **Body-Brain Architecture** — *Body* (SiliconBeing) stays alive and detects triggers; *Brain* (ContextManager) loads history, calls AI, executes tools, and persists responses
- **Tool Call Loop** — AI returns tool_calls -> execute tools -> feed results back -> AI continues -> until plain text response
- **Executor-Permission Security** — All disk, network, and command-line operations pass through executors with permission verification
  - 5-level permission chain: IsCurator -> UserFrequencyCache -> GlobalACL -> IPermissionCallback -> IPermissionAskHandler
  - Audit logging for all permission decisions
- **Minimal Dependencies** — Core library only depends on Microsoft.CodeAnalysis.CSharp for Roslyn dynamic compilation
- **Zero Database Dependency** — File-based storage (JSON) with time-indexed queries via `ITimeStorage`
- **Localization** — Built-in Chinese and English support
- **Web UI** — Built-in HTTP server with WebSocket/SSE support, multiple skins, and comprehensive dashboard
  - **Skin System** — 4 built-in skins (Admin, Chat, Creative, Dev) with complete UI component library
  - **15 Controllers** — About, Being, Chat, CodeBrowser, Config, Dashboard, Executor, Init, Knowledge, Log, Memory, Permission, PermissionRequest, Project, Task
  - **Real-time Updates** — SSE (Server-Sent Events) for live data streaming

## Tech Stack

| Component | Technology |
|-----------|-----------|
| Runtime | .NET 9 |
| Language | C# |
| AI Integration | Ollama (native HTTP API) |
| Storage | File system (JSON + time-indexed directories) |
| Web Server | HttpListener (built-in .NET) |
| Dynamic Compilation | Roslyn (Microsoft.CodeAnalysis.CSharp) |
| License | Apache-2.0 |

## Project Structure

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Core library (interfaces, abstractions)
│   │   ├── ServiceLocator.cs             # Global service locator: Register/Get, ChatSystem, IMManager, AuditLogger, GlobalACL
│   │       ├── Runtime/                       # MainLoop, TickObject, CoreHost, CoreHostBuilder, PerformanceMonitor, TestTickObject
│   │   ├── SiliconBeing/                  # SiliconBeingBase, ISiliconBeingFactory, SiliconBeingManager, SoulFileManager, TaskSystem, TimerSystem, SiliconCurator, Memory
│   │   ├── AI/                            # IAIClient, IAIClientFactory, ContextManager ("brain"), Message, AIRequest/AIResponse
│   │   ├── Chat/                          # ChatSystem, IChatService, SimpleChatService, ISession, SingleChatSession, GroupChatSession, ChatMessage
│   │   ├── Executors/                     # ExecutorBase, DiskExecutor, NetworkExecutor, CommandLineExecutor, ExecutorRequest, ExecutorResult
│   │   ├── Tools/                         # ITool, ToolManager (reflection scanning), ToolCall/ToolResult, ToolDefinition, SiliconManagerOnlyAttribute
│   │   ├── Security/                      # PermissionManager, GlobalACL, UserFrequencyCache, AuditLogger, PermissionResult, PermissionType
│   │   ├── IM/                            # IIMProvider, IMManager (message routing)
│   │   ├── Storage/                       # IStorage, ITimeStorage (key-value + time-indexed)
│   │   ├── Config/                        # ConfigDataBase, Config (singleton + JSON), ConfigDataBaseConverter, GuidConverter
│   │   ├── Localization/                  # LocalizationBase, LocalizationManager, Language enum
│   │   ├── Logging/                       # ILogger, ILoggerProvider, LogEntry, LogLevel, LogManager
│   │   ├── Compilation/                   # DynamicBeingLoader, DynamicCompilationExecutor, SecurityScanner, CodeEncryption
│   │   └── Time/                          # IncompleteDate (time range queries)
│   │
│   └── SiliconLife.Default/               # Default implementations + entry point
│       ├── Program.cs                     # Application entry (wiring all components)
│       ├── AI/                            # OllamaClient, OllamaClientFactory (native Ollama HTTP API)
│       ├── SiliconBeing/                  # DefaultSiliconBeing, DefaultSiliconBeingFactory
│       ├── Executors/                     # Default executor implementations
│       ├── IM/                            # WebUIProvider (Web UI as IM channel)
│       ├── Tools/                         # Built-in tools: Calendar, Chat, Curator, Disk, DynamicCompile, Memory, Network, System, Task, Timer
│       ├── Config/                        # DefaultConfigData
│       ├── Localization/                  # ZhCN, EnUS, DefaultLocalizationBase
│       ├── Logging/                       # ConsoleLoggerProvider, FileSystemLoggerProvider
│       ├── Storage/                       # FileSystemStorage, FileSystemTimeStorage
│       ├── Security/                      # DefaultPermissionCallback, IMPermissionAskHandler
│       └── Web/                           # Web UI implementation
│           ├── Controllers/               # 15 controllers: About, Being, Chat, CodeBrowser, Config, Dashboard, Executor, Init, Knowledge, Log, Memory, Permission, PermissionRequest, Project, Task
│           ├── Models/                    # ViewModels for all controllers (AboutViewModel, BeingViewModel, etc.)
│           ├── Views/                     # HTML views (ViewBase, AboutView, BeingView, ChatView, etc.)
│           ├── Skins/                     # 4 skins: Admin (professional), Chat (conversational), Creative (artistic), Dev (developer-focused)
│           ├── ISkin.cs                   # Skin interface with UI component library
│           ├── Controller.cs              # Base controller class
│           ├── WebHost.cs                 # HTTP server
│           ├── Router.cs                  # Request routing
│           ├── SSEHandler.cs              # Server-Sent Events
│           ├── WebSecurity.cs             # Web security utilities
│           ├── H.cs                       # HTML helper utilities
│           ├── CssBuilder.cs              # CSS builder utility
│           └── JsBuilder.cs               # JavaScript builder utility
│
├── docs/
│   ├── en-US/                             # English documentation
│   └── zh-CN/                             # Chinese documentation
```

## Architecture Overview

```
MainLoop (dedicated thread, watchdog + circuit breaker)
  └── TickObject (priority-sorted)
       └── SiliconBeingManager
            └── SiliconBeingRunner (temporary thread, timeout + circuit breaker)
                 └── DefaultSiliconBeing.Tick()
                      └── ContextManager.ThinkOnChat()
                           └── IAIClient.Chat() -> Tool Call Loop -> Persist to ChatSystem
```

All AI-initiated I/O operations pass through the security chain:

```
Tool Call -> Executor -> PermissionManager -> [IsCurator -> FrequencyCache -> GlobalACL -> Callback -> AskUser]
```

## Getting Started

### Prerequisites

- .NET 9 SDK
- [Ollama](https://ollama.com) running locally with a model pulled (e.g., `ollama pull llama3`)

### Build

```bash
dotnet restore
dotnet build
```

### Run

```bash
dotnet run --project src/SiliconLife.Default
```

The application will start a web server and automatically open the Web UI in your browser.

### Publish (single file)

```bash
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

## Roadmap

- [x] Phase 1: Console AI chat
- [x] Phase 2: Framework skeleton (MainLoop + TickObject + Watchdog + Circuit Breaker)
- [x] Phase 3: First Silicon Being with soul file (Body-Brain architecture)
- [x] Phase 4: Persistent memory (ChatSystem + ITimeStorage)
- [x] Phase 5: Tool system + Executors
- [x] Phase 6: Permission system (5-level chain, AuditLogger, GlobalACL)
- [x] Phase 7: Dynamic compilation + self-evolution (Roslyn)
- [x] Phase 8: Long-term memory + Task + Timer
- [x] Phase 9: CoreHost + multi-agent collaboration
- [x] Phase 10: Web UI (HTTP + WebSocket + SSE)
- [ ] Phase 11: External IM (Feishu / WhatsApp / Telegram)
- [ ] Phase 12: Knowledge graph, plugins, and extras

## Documentation

- [Architecture](docs/en-US/architecture.md) — System design, scheduling, component architecture
- [Security](docs/en-US/security.md) — Permission model, executors, dynamic compilation security
- [Roadmap](docs/en-US/roadmap.md) — Detailed 12-phase development plan

## License

This project is licensed under the Apache License 2.0 — see the [LICENSE](LICENSE) file for details.

## Author

Hoshino Kennji — [YouTube](https://www.youtube.com/@hoshinokennji) | [Bilibili](https://space.bilibili.com/617827040)
