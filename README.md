# Silicon Life Collective

**⚠️ WARNING: Dynamic compilation works but requires code templates to function properly. Thorough testing is ongoing.**

A .NET 9 multi-agent collaboration platform where AI agents called **Silicon Beings** self-evolve through Roslyn dynamic compilation.

[中文文档](docs/zh-CN/README.md) | [繁體中文](docs/zh-HK/README.md) | [日本語](docs/ja-JP/README.md)

## Features

- **Multi-Agent Orchestration** — Managed by a *Silicon Curator* with tick-driven time-sliced fair scheduling (MainLoop + TickObject + Watchdog + Circuit Breaker)
- **Soul File Driven** — Each Silicon Being is driven by a core prompt file (`soul.md`) that defines its personality and behavior
- **Body-Brain Architecture** — *Body* (SiliconBeing) stays alive and detects triggers; *Brain* (ContextManager) loads history, calls AI, executes tools, and persists responses
- **Tool Call Loop** — AI returns tool_calls → execute tools → feed results back → AI continues → until plain text response
- **Executor-Permission Security** — All disk, network, and command-line operations pass through executors with permission verification
  - 5-level permission chain: IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - Audit logging for all permission decisions
- **Token Usage Audit** — Built-in token usage tracking and reporting via `ITokenUsageAudit` / `TokenUsageAuditManager`
- **Multiple AI Backends** — Support for Ollama (local) and Alibaba Cloud DashScope (cloud)
  - **Ollama** — Local model hosting with native HTTP API
  - **DashScope (百炼)** — Cloud AI service with OpenAI-compatible API, multi-region deployment, and 13+ models (Qwen, DeepSeek, GLM, Kimi, Llama)
- **32 Calendar Systems** — Multi-calendar support including Gregorian, Chinese Lunar, Islamic, Hebrew, Japanese, Persian, Mayan, and more
- **Minimal Dependencies** — Core library only depends on Microsoft.CodeAnalysis.CSharp for Roslyn dynamic compilation
- **Zero Database Dependency** — File-based storage (JSON) with time-indexed queries via `ITimeStorage`
- **Localization** — Built-in Chinese (Simplified & Traditional) and English support
- **Web UI** — Built-in HTTP server with SSE support, multiple skins, and comprehensive dashboard
  - **Skin System** — 4 built-in skins (Admin, Chat, Creative, Dev) with pluggable ISkin interface and auto-discovery
  - **17 Controllers** — About, Audit, Being, Chat, CodeBrowser, Config, Dashboard, Executor, Init, Knowledge, Log, Memory, Permission, PermissionRequest, Project, Task, Timer
  - **Real-time Updates** — SSE (Server-Sent Events) for chat messages, being status, and system events
  - **HTML/CSS/JS Builders** — Server-side markup generation via `H`, `CssBuilder`, and `JsBuilder` (zero frontend framework dependency)
  - **Localization** — Three built-in locales (zh-CN, zh-HK, en-US) with LocalizationManager resolution

## Tech Stack

| Component | Technology |
|-----------|-----------|
| Runtime | .NET 9 |
| Language | C# |
| AI Integration | Ollama (local), Alibaba Cloud DashScope (cloud) |
| Storage | File system (JSON + time-indexed directories) |
| Web Server | HttpListener (built-in .NET) |
| Dynamic Compilation | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| License | Apache-2.0 |

## Project Structure

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Core library (interfaces, abstractions)
│   │   ├── ServiceLocator.cs             # Global service locator: Register/Get, ChatSystem, IMManager, AuditLogger, GlobalACL, BeingFactory, BeingManager, DynamicBeingLoader, TokenUsageAudit
│   │   ├── Runtime/                       # MainLoop, TickObject, CoreHost, CoreHostBuilder, PerformanceMonitor
│   │   ├── SiliconBeing/                  # SiliconBeingBase, SiliconBeingManager, SiliconCurator, ISiliconBeingFactory, SoulFileManager, Memory, TaskSystem, TimerSystem
│   │   ├── AI/                            # IAIClient, IAIClientFactory, ContextManager ("brain"), Message, AIRequest/AIResponse
│   │   ├── Audit/                         # ITokenUsageAudit, TokenUsageAuditManager, TokenUsageRecord, TokenUsageSummary, TokenUsageQuery
│   │   ├── Chat/                          # ChatSystem, IChatService, SimpleChatService, SessionBase, SingleChatSession, GroupChatSession, BroadcastChannel, ChatMessage
│   │   ├── Executors/                     # ExecutorBase, DiskExecutor, NetworkExecutor, CommandLineExecutor, ExecutorRequest, ExecutorResult
│   │   ├── Tools/                         # ITool, ToolManager (reflection scanning), ToolCall/ToolResult, ToolDefinition, SiliconManagerOnlyAttribute
│   │   ├── Security/                      # PermissionManager, GlobalACL, AuditLogger, UserFrequencyCache, PermissionResult, PermissionType, IPermissionCallback, IPermissionAskHandler
│   │   ├── IM/                            # IIMProvider, IMManager (message routing)
│   │   ├── Storage/                       # IStorage, ITimeStorage (key-value + time-indexed)
│   │   ├── Config/                        # ConfigDataBase, Config (singleton + JSON), ConfigDataBaseConverter, GuidConverter, AIClientConfigAttribute, ConfigGroupAttribute, ConfigIgnoreAttribute, DirectoryInfoConverter
│   │   ├── Localization/                  # LocalizationBase, LocalizationManager, Language enum
│   │   ├── Logging/                       # ILogger, ILoggerProvider, LogEntry, LogLevel, LogManager
│   │   ├── Compilation/                   # DynamicBeingLoader, DynamicCompilationExecutor, SecurityScanner, CodeEncryption
│   │   └── Time/                          # IncompleteDate (time range queries)
│   │
│   └── SiliconLife.Default/               # Default implementations + entry point
│       ├── Program.cs                     # Application entry (wiring all components)
│       ├── AI/                            # OllamaClient, OllamaClientFactory (native Ollama HTTP API); DashScopeClient, DashScopeClientFactory (Alibaba Cloud Bailian)
│       ├── SiliconBeing/                  # DefaultSiliconBeing, DefaultSiliconBeingFactory
│       ├── Calendar/                      # 32 calendar implementations: Buddhist, Cherokee, ChineseLunar, ChulaSakarat, Coptic, Dai, DehongDai, Ethiopian, FrenchRepublican, Gregorian, Hebrew, Indian, Inuit, Islamic, Japanese, Javanese, Juche, Julian, Khmer, Mayan, Mongolian, Persian, RepublicOfChina, Roman, Saka, Sexagenary, Tibetan, Vietnamese, VikramSamvat, Yi, Zoroastrian
│       ├── Executors/                     # Default executor implementations
│       ├── IM/                            # WebUIProvider (Web UI as IM channel), IMPermissionAskHandler
│       ├── Tools/                         # Built-in tools: Calendar, Chat, Config, Curator, Disk, DynamicCompile, Memory, Network, System, Task, Timer, TokenAudit
│       ├── Config/                        # DefaultConfigData
│       ├── Localization/                  # ZhCN, ZhHK, EnUS, DefaultLocalizationBase
│       ├── Logging/                       # ConsoleLoggerProvider, FileSystemLoggerProvider
│       ├── Storage/                       # FileSystemStorage, FileSystemTimeStorage
│       ├── Security/                      # DefaultPermissionCallback
│       ├── Runtime/                       # TestTickObject
│       └── Web/                           # Web UI implementation
│           ├── Controllers/               # 17 controllers: About, Audit, Being, Chat, CodeBrowser, Config, Dashboard, Executor, Init, Knowledge, Log, Memory, Permission, PermissionRequest, Project, Task, Timer
│           ├── Models/                    # ViewModels: AboutViewModel, AuditViewModel, BeingViewModel, ChatMessage, ChatViewModel, CodeBrowserViewModel, ConfigViewModel, DashboardViewModel, ExecutorViewModel, KnowledgeViewModel, LogViewModel, MemoryViewModel, PermissionViewModel, PermissionRequestViewModel, ProjectViewModel, TaskViewModel, TimerViewModel, ViewModelBase
│           ├── Views/                     # HTML views: ViewBase, AboutView, AuditView, BeingView, ChatView, CodeBrowserView, CodeEditorView, ConfigView, DashboardView, ExecutorView, KnowledgeView, LogView, MarkdownEditorView, MemoryView, PermissionView, ProjectView, TaskView, TimerView
│           ├── Skins/                     # 4 skins: Admin (professional), Chat (conversational), Creative (artistic), Dev (developer-focused)
│           ├── ISkin.cs                   # Skin interface + SkinPreviewInfo + SkinManager (auto-discovery)
│           ├── Controller.cs              # Base controller class
│           ├── WebHost.cs                 # HTTP server (HttpListener)
│           ├── Router.cs                  # Request routing with pattern matching
│           ├── SSEHandler.cs              # Server-Sent Events
│           ├── WebSecurity.cs             # Web security utilities
│           ├── H.cs                       # Fluent HTML builder DSL
│           ├── CssBuilder.cs              # CSS builder utility
│           └── JsBuilder.cs               # JavaScript builder utility
│
├── docs/
│   └── zh-CN/                             # Chinese documentation
```

## Architecture Overview

```
MainLoop (dedicated thread, watchdog + circuit breaker)
  └── TickObject (priority-sorted)
       └── SiliconBeingManager
            └── SiliconBeingRunner (temporary thread per tick, timeout + circuit breaker)
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
- AI backend (choose one):
  - **Ollama**: [Ollama](https://ollama.com) running locally with a model pulled (e.g., `ollama pull llama3`)
  - **Alibaba Cloud DashScope**: Valid API key from [Bailian Console](https://bailian.console.aliyun.com/)

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
- [x] Phase 10: Web UI (HTTP + SSE, 17 controllers, 4 skins)
- [x] Phase 10.5: Incremental enhancements (BroadcastChannel, TokenAudit, 32 calendars, tool enhancements)
- [ ] Phase 11: External IM integration (Feishu / WhatsApp / Telegram)
- [ ] Phase 12: Knowledge graph, plugin system, and skills ecosystem

## Documentation

- [Architecture](architecture.md) — System design, scheduling, component architecture
- [Security](security.md) — Permission model, executors, dynamic compilation security
- [Roadmap](roadmap.md) — Detailed 12-phase development plan

## License

This project is licensed under the Apache License 2.0 — see the [LICENSE](LICENSE) file for details.

## Author

Hoshino Kennji — [YouTube](https://www.youtube.com/@hoshinokennji) | [Bilibili](https://space.bilibili.com/617827040)
