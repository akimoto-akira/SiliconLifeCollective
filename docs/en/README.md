# Silicon Life Collective

**Version: v0.1.0-alpha** | **Silicon Life Collective** — A multi-agent collaboration platform based on .NET 9, where AI agents are called **Silicon Beings**, capable of self-evolution through Roslyn dynamic compilation.

[English](../README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Deutsch](../de-DE/README.md) | [Čeština](../cs-CZ/README.md)

## 🌟 Core Features

### Agent System
- **Multi-Agent Orchestration** — Unified management by *Silicon Curator*, using clock-driven time-slice fair scheduling mechanism
- **Soul File Driven** — Each silicon being is driven by a core prompt file (`soul.md`), defining unique personality and behavior patterns
- **Body-Brain Architecture** — *Body* (SiliconBeing) maintains vital signs and detects trigger scenarios; *Brain* (ContextManager) handles loading history, calling AI, executing tools, and persisting responses
- **Self-Evolution Capability** — Through Roslyn dynamic compilation technology, silicon beings can rewrite their own code to achieve evolution

### Tools & Execution
- **23 Built-in Tools** — Covering calendar, chat, configuration, disk, network, memory, tasks, timers, knowledge base, work notes, WebView browser, etc.
- **Tool Call Loop** — AI returns tool call → Execute tool → Results fed back to AI → Continue loop until pure text response
- **Executor-Permission Security** — All I/O operations go through strict permission validation via executors
  - 5-level permission chain: IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - Complete audit logging of all permission decisions

### AI & Knowledge
- **Multiple AI Backend Support**
  - **Ollama** — Local model deployment, using native HTTP API
  - **Alibaba Cloud DashScope (Bailian)** — Cloud AI service, OpenAI API compatible, supporting 13+ models, multi-region deployment
- **32 Calendar Systems** — Comprehensive coverage of global major calendars, including Gregorian, Chinese Lunar, Islamic, Hebrew, Japanese, Persian, Mayan, Chinese Historical Calendar, etc.
- **Knowledge Network System** — Knowledge graph based on triplets (subject-relation-object), supporting storage, querying, and path discovery

### Web Interface
- **Modern Web UI** — Built-in HTTP server with SSE real-time updates
- **4 Skin Themes** — Admin, Chat, Creative, Dev versions, supporting auto-discovery and switching
- **20+ Controllers** — Complete system management, chat, configuration, monitoring functionality
- **Zero Frontend Framework Dependency** — HTML/CSS/JS generated server-side via `H`, `CssBuilder`, and `JsBuilder`

### Internationalization & Localization
- **Comprehensive support for 21 language variants**
  - Chinese: zh-CN, zh-HK, zh-SG, zh-MO, zh-TW, zhMY (6 variants)
  - English: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 variants)
  - Spanish: es-ES, es-MX (2 variants)
  - Japanese: ja-JP | Korean: ko-KR | Czech: cs-CZ

### Data & Storage
- **Zero Database Dependency** — Pure file system storage (JSON format)
- **Time Index Query** — Efficient querying by time range via `ITimeStorage` interface
- **Minimal Dependencies** — Core library only depends on Microsoft.CodeAnalysis.CSharp for dynamic compilation

## 🛠️ Technology Stack

| Component | Technology |
|-----------|------------|
| Runtime | .NET 9 |
| Programming Language | C# |
| AI Integration | Ollama (local), Alibaba Cloud DashScope (cloud) |
| Data Storage | File system (JSON + time-indexed directories) |
| Web Server | HttpListener (.NET built-in) |
| Dynamic Compilation | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Browser Automation | Playwright (WebView) |
| License | Apache-2.0 |

## 📁 Project Structure

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Core library (interfaces, abstract classes)
│   │   ├── AI/                            # AI client interfaces, context manager, message models
│   │   ├── Audit/                         # Token usage audit system
│   │   ├── Chat/                          # Chat system, session management, broadcast channels
│   │   ├── Compilation/                   # Dynamic compilation, security scanning, code encryption
│   │   ├── Config/                        # Configuration management system
│   │   ├── Executors/                     # Executors (disk, network, command line)
│   │   ├── IM/                            # Instant messaging provider interfaces
│   │   ├── Knowledge/                     # Knowledge network system
│   │   ├── Localization/                  # Localization system
│   │   ├── Logging/                       # Logging system
│   │   ├── Project/                       # Project management system
│   │   ├── Runtime/                       # Main loop, clock objects, core host
│   │   ├── Security/                      # Permission management system
│   │   ├── SiliconBeing/                  # Silicon being base class, manager, factory
│   │   ├── Storage/                       # Storage interfaces
│   │   ├── Time/                          # Incomplete dates (time range queries)
│   │   ├── Tools/                         # Tool interfaces and tool manager
│   │   ├── WebView/                       # WebView browser interface
│   │   └── ServiceLocator.cs              # Global service locator
│   │
│   └── SiliconLife.Default/               # Default implementation + application entry
│       ├── Program.cs                     # Entry point (assembles all components)
│       ├── AI/                            # Ollama client, DashScope client
│       ├── Calendar/                      # 32 calendar implementations
│       ├── Config/                        # Default configuration data
│       ├── Executors/                     # Default executor implementations
│       ├── Help/                          # Help documentation system
│       ├── IM/                            # WebUI provider
│       ├── Knowledge/                     # Knowledge network implementation
│       ├── Localization/                  # 21 language localization
│       ├── Logging/                       # Log provider implementations
│       ├── Project/                       # Project system implementation
│       ├── Runtime/                       # Test clock objects
│       ├── Security/                      # Default permission callbacks
│       ├── SiliconBeing/                  # Default silicon being implementation
│       ├── Storage/                       # File system storage implementation
│       ├── Tools/                         # 23 built-in tool implementations
│       ├── WebView/                       # Playwright WebView implementation
│       └── Web/                           # Web UI implementation
│           ├── Controllers/               # 20+ controllers
│           ├── Models/                    # View models
│           ├── Views/                     # HTML views
│           └── Skins/                     # 4 skin themes
│
├── docs/                                  # Multi-language documentation
│   ├── zh-CN/                             # Simplified Chinese documentation
│   ├── en/                                # English documentation
│   └── ...                                # Other language documentation
│
└── 总文档/                                 # Requirements and architecture documentation (Chinese)
    ├── 需求文档.md
    ├── 架构大纲.md
    └── 实现顺序.md
```

## 🏗️ Architecture Overview

### Scheduling Architecture
```
Main Loop (dedicated thread, watchdog + circuit breaker)
  └── Clock Objects (sorted by priority)
       └── Silicon Being Manager
            └── Silicon Being Runner (temporary thread, timeout + circuit breaker)
                 └── SiliconBeing.Tick()
                      └── ContextManager.Think()
                           └── AI Client.Chat()
                                └── Tool Call Loop → Persist to Chat System
```

### Security Architecture
All AI-initiated I/O operations must pass through a strict security chain:

```
Tool Call → Executor → Permission Manager → [IsCurator → Frequency Cache → Global ACL → Callback → Ask User]
```

## 🚀 Quick Start

### Prerequisites

- **.NET 9 SDK** — [Download Link](https://dotnet.microsoft.com/download/dotnet/9.0)
- **AI Backend** (choose one):
  - **Ollama**: [Install Ollama](https://ollama.com) and pull models (e.g., `ollama pull llama3`)
  - **Alibaba Cloud DashScope**: Get API key from [DashScope Console](https://bailian.console.aliyun.com/)

### Build the Project

```bash
dotnet restore
dotnet build
```

### Run the System

```bash
dotnet run --project src/SiliconLife.Default
```

The application will start the web server and automatically open the Web UI in the browser.

### Publish Single File

```bash
# Windows
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 Development Roadmap

### ✅ Completed
- [x] Phase 1: Console AI Chat
- [x] Phase 2: Framework Skeleton (Main Loop + Clock Objects + Watchdog + Circuit Breaker)
- [x] Phase 3: First Silicon Being with Soul File (Body-Brain Architecture)
- [x] Phase 4: Persistent Memory (Chat System + Time Storage Interface)
- [x] Phase 5: Tool System + Executors
- [x] Phase 6: Permission System (5-Level Chain, Audit Logger, Global ACL)
- [x] Phase 7: Dynamic Compilation + Self-Evolution (Roslyn)
- [x] Phase 8: Long-term Memory + Tasks + Timers
- [x] Phase 9: Core Host + Multi-Agent Collaboration
- [x] Phase 10: Web UI (HTTP + SSE, 20+ Controllers, 4 Skins)
- [x] Phase 10.5: Incremental Enhancements (Broadcast Channels, Token Audit, 32 Calendars, Tool Enhancements, 21 Language Localization)
- [x] Phase 10.6: Refinement & Optimization (WebView, Help System, Project Workspace, Knowledge Network)

### 🚧 Planned
- [ ] Phase 11: External IM Integration (Feishu / WhatsApp / Telegram)
- [ ] Phase 12: Plugin System and Skill Ecosystem

## 📚 Documentation

- [Architecture Design](architecture.md) — System design, scheduling mechanism, component architecture
- [Security Model](security.md) — Permission model, executors, dynamic compilation security
- [Development Guide](development-guide.md) — Tool development, extension guide
- [API Reference](api-reference.md) — Web API endpoint documentation
- [Tool Reference](tools-reference.md) — Detailed built-in tool descriptions
- [Web UI Guide](web-ui-guide.md) — Web interface usage guide
- [Silicon Being Guide](silicon-being-guide.md) — Agent development guide
- [Permission System](permission-system.md) — Permission management details
- [Calendar System](calendar-system.md) — 32 calendar system descriptions
- [Getting Started](getting-started.md) — Detailed introduction guide
- [Troubleshooting](troubleshooting.md) — Frequently asked questions
- [Roadmap](roadmap.md) — Complete development plan
- [Changelog](changelog.md) — Version update history
- [Contributing](contributing.md) — How to participate in the project

## 🤝 Contributing

We welcome contributions of all kinds! Please see the [Contributing Guide](contributing.md) for details.

### Development Workflow
1. Fork this repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'feat: add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Submit a Pull Request

## 📄 License

This project is licensed under the Apache License 2.0 — see the [LICENSE](../../LICENSE) file for details.

## 👨‍💻 Author

**Hoshino Kennji**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- Gitee: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili: [617827040](https://space.bilibili.com/617827040)

## 🙏 Acknowledgments

Thank you to all developers and AI platform providers who have contributed to this project.

---

**Silicon Life Collective** — Making AI agents truly "alive"
