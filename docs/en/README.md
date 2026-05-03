![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**Version: v0.1.0-alpha** | **Silicon Life Collective** — A multi-agent collaboration platform based on .NET 9, where AI agents are called **Silicon Beings**, capable of self-evolution through Roslyn dynamic compilation.

**English** | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Deutsch](../de-DE/README.md) | [Français](../fr-FR/README.md) | [Čeština](../cs-CZ/README.md)

## 🌟 Core Features

### Agent System
- **Multi-Agent Orchestration** — Unified management by *Silicon Curator*, using clock-driven time-slice fair scheduling mechanism
- **Soul File Driven** — Each silicon being is driven by a core prompt file (`soul.md`), defining unique personality and behavior patterns
- **Body-Brain Architecture** — *Body* (SiliconBeing) maintains vital signs and detects trigger scenarios; *Brain* (ContextManager) handles loading history, calling AI, executing tools, and persisting responses
- **Self-Evolution Capability** — Through Roslyn dynamic compilation technology, silicon beings can rewrite their own code to achieve evolution

### Plugin System
- **Plugin Extension Architecture** — Feature extension via IPlugin interface, supporting dynamic loading of plugin DLLs from directories
- **Security Sandbox** — Plugin loader performs strict security scanning, blocking access to System.IO, System.Net, and other namespaces
- **Isolated Loading** — Uses custom AssemblyLoadContext for isolated loading, preventing plugins from affecting main program stability
- **Tool Integration** — Plugins can register custom tools via ITool interface, automatically integrated into the tool call loop

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
- **7 Skin Themes** — Admin, Chat, Creative, Dev, High Contrast, Light, Minimal versions, supporting auto-discovery and switching
- **20+ Controllers** — Complete system management, chat, configuration, monitoring functionality
- **Zero Frontend Framework Dependency** — HTML/CSS/JS generated server-side via `H`, `CssBuilder`, and `JsBuilder`

### Internationalization & Localization
- **Comprehensive support for 21 language variants**
  - Chinese: zh-CN, zh-HK, zh-SG, zh-MO, zh-TW, zhMY (6 variants)
  - English: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 variants)
  - Spanish: es-ES, es-MX (2 variants)
  - Japanese: ja-JP | Korean: ko-KR | Czech: cs-CZ

### Data & Storage
- **SpeedyPack High-Performance Storage** — Custom .spk storage engine, in-memory directory mapping + entry cache + asynchronous write queue
- **Zero Database Dependency** — Default version uses pure file system storage (JSON format), Fast version uses SpeedyPack in-memory storage
- **Time Index Query** — Efficient querying by time range via `ITimeStorage` interface
- **Auto-Compaction** — SpeedyPack supports scheduled auto-compaction, reclaiming free space
- **Minimal Dependencies** — Core library only depends on Microsoft.CodeAnalysis.CSharp for dynamic compilation

## 🔄 Dual-Version Architecture

This project provides two implementation versions to meet different scenario needs:

### SiliconLife.Default (Default Version)
- **Positioning**: Default implementation, primarily used for architecture feasibility verification
- **Runtime Mode**: Console application
- **Storage Method**: Pure file system JSON storage
- **Applicable Scenarios**: High data security requirements, limited memory resources, small data volume scenarios
- **Features**: Simple and reliable, immediate data persistence, no memory loss risk
- **Role Description**: Serves as the baseline implementation for architecture verification, suitable for first-time contact, development debugging, or data security priority scenarios
- **Startup Command**: `dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast (High-Performance Version)
- **Positioning**: Main production version
- **Runtime Mode**: Windows Forms application (supports system tray)
- **Storage Method**: SpeedyPack in-memory storage + asynchronous batch persistence (.spk file format)
- **Applicable Scenarios**: High concurrency, low latency, large data volume scenarios
- **Features**: Extreme performance optimization, tray background operation, SpeedyPack engine + auto-compaction ensures data security
- **Performance Improvement**: Storage read latency reduced by 1000x, write latency reduced by 15000x, concurrent processing capacity increased by 50x
- **Role Description**: A production-grade implementation with deep optimization, the first choice for long-term operation and actual production environments
- **Startup Command**: `dotnet run --project src/SiliconLife.Fast`

### Version Comparison

| Feature | SiliconLife.Default | SiliconLife.Fast |
|---------|---------------------|------------------|
| **Runtime Mode** | Console application | Forms application (system tray) |
| **User Interface** | Web UI (browser access) | Tray icon + tray window + Web UI |
| **System Tray** | ❌ None | ✅ Supports minimize to tray |
| **Background Operation** | ❌ Exits when console closes | ✅ Continuous tray background operation |
| **Storage Method** | File system JSON storage | SpeedyPack in-memory storage + asynchronous persistence |
| **Storage Engine** | File system I/O | SiliconLife.Speedy (.spk format) |
| **Read Latency** | ~10ms (disk I/O) | ~0.01ms (memory operation) |
| **Write Latency** | ~15ms (synchronous write) | ~0.001ms (asynchronous write) |
| **Concurrency** | ~100 req/s | ~5000 req/s |
| **Memory Usage** | ~200MB | ~500MB |
| **Data Security** | Extremely high (immediate persistence) | High (asynchronous persistence + auto-compaction) |
| **Applicable Scenarios** | Data security first, small data | Performance first, large data, high concurrency |

## 🛠️ Technology Stack

| Component | SiliconLife.Default | SiliconLife.Fast |
|-----------|---------------------|------------------|
| Runtime | .NET 9 | .NET 9 Windows |
| Programming Language | C# | C# |
| Application Type | Console application | Windows Forms application |
| AI Integration | Ollama (local), Alibaba Cloud DashScope (cloud) | Ollama (local), Alibaba Cloud DashScope (cloud) |
| Data Storage | File system (JSON + time-indexed directories) | SpeedyPack (.spk format, memory mapping + asynchronous persistence) |
| Web Server | HttpListener (.NET built-in) | HttpListener (.NET built-in) |
| Dynamic Compilation | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Browser Automation | Playwright (WebView) | Playwright (WebView) |
| Plugin System | ✅ Supported (IPlugin + PluginLoader) | ✅ Supported (IPlugin + PluginLoader) |
| System Tray | ❌ Not supported | ✅ Supported (NotifyIcon) |
| License | Apache-2.0 | Apache-2.0 |

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
│   │   ├── Plugins/                       # Plugin system (IPlugin interface, PluginLoader)
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
│   ├── SiliconLife.Common/                # Shared implementation (used by both versions)
│   │   ├── AI/                            # AI client factories
│   │   ├── Calendar/                      # 32 calendar implementations
│   │   ├── Localization/                  # Localization base classes
│   │   ├── Security/                      # Permission manager
│   │   ├── SiliconBeing/                  # Default silicon being implementation
│   │   ├── Tools/                         # Common tool implementations
│   │   └── WebView/                       # WebView interface
│   │
│   ├── SiliconLife.Default/               # Default implementation + application entry (console version)
│   │   ├── Program.cs                     # Entry point (assembles all components)
│   │   ├── Config/                        # Default configuration data
│   │   ├── Executors/                     # Default executor implementations
│   │   ├── Help/                          # Help documentation system
│   │   ├── IM/                            # WebUI provider
│   │   ├── Knowledge/                     # Knowledge network implementation
│   │   ├── Localization/                  # 21 language localization
│   │   ├── Logging/                       # Log provider implementations
│   │   ├── Project/                       # Project system implementation
│   │   ├── Runtime/                       # Test clock objects
│   │   ├── Security/                      # Default permission callbacks
│   │   ├── SiliconBeing/                  # Default silicon being implementation
│   │   ├── Storage/                       # File system storage implementation
│   │   ├── Tools/                         # Built-in tool implementations
│   │   ├── WebView/                       # Playwright WebView implementation
│   │   └── Web/                           # Web UI implementation
│   │       ├── Controllers/               # 20+ controllers
│   │       ├── Models/                    # View models
│   │       ├── Views/                     # HTML views
│   │       └── Skins/                     # 4 skin themes
│   │
│   └── SiliconLife.Fast/                  # High-performance implementation + application entry (forms version)
│       ├── Program.cs                     # Entry point (forms application)
│       ├── Config/                        # Configuration data (shared with Default)
│       ├── Executors/                     # Optimized executor implementations
│       ├── Help/                          # Help documentation system
│       ├── IM/                            # WebUI provider
│       ├── Knowledge/                     # Knowledge network implementation (memory-optimized)
│       ├── Localization/                  # 21 language localization
│       ├── Logging/                       # High-performance log providers
│       ├── Project/                       # Project system implementation
│       ├── Security/                      # Optimized permission callbacks
│       ├── SiliconBeing/                  # High-performance silicon being implementation
│       ├── Storage/                       # SpeedyPack storage adapters
│       ├── Tools/                         # Optimized built-in tool implementations
│       ├── Tray/                          # System tray (9 language localization)
│       ├── WebView/                       # Playwright WebView implementation
│       └── Web/                           # High-performance Web UI implementation
│           ├── Component/                 # UI component library (30+ components)
│           ├── Controllers/               # 20+ controllers
│           ├── Models/                    # View models
│           ├── Views/                     # HTML views
│           └── Skins/                     # 7 skin themes
│
│   ├── SiliconLife.Speedy/                # SpeedyPack high-performance storage engine
│   │   ├── SpeedyPack.cs                  # Core class (in-memory directory mapping + cache + async write)
│   │   ├── SpeedyPackOptions.cs           # Configuration options (cache TTL, max entries, etc.)
│   │   ├── IPackTransaction.cs            # Transaction interface
│   │   ├── SpkFileInfo.cs                 # File information
│   │   └── Internal/                      # Internal implementation
│       │   ├── DirectoryMap.cs            # In-memory directory mapping
│       │   ├── EntryCache.cs              # Entry cache
│       │   ├── FreeList.cs                # Free space management
│       │   ├── PackFileReader.cs          # Pack file reader
│       │   ├── PackFileWriter.cs          # Pack file writer
│       │   ├── WriteQueue.cs              # Asynchronous write queue
│       │   ├── WriteOperation.cs          # Write operation
│       │   ├── SpeedyTransaction.cs       # Transaction implementation
│       │   ├── SpkHeader.cs              # Pack file header
│       │   └── PathNormalizer.cs          # Path normalization
│   │
│   └── SiliconLife.Speedy.Manager/        # SpeedyPack management tool (WPF)
│       ├── MainForm.cs                    # Main form
│       ├── Program.cs                     # Entry point
│       └── slc.ico                        # Application icon
│
├── docs/                                  # Multi-language documentation
│   ├── zh-CN/                             # Simplified Chinese documentation
│   ├── en/                                # English documentation
│   └── ...                                # Other language documentation
│
└── 总文档/                                 # Requirements and architecture documentation
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

#### Option 1: Run Default Version (Console Application)

```bash
dotnet run --project src/SiliconLife.Default
```

The application will start the web server and automatically open the Web UI in the browser.

**Applicable Scenarios**:
- ✅ Extremely high data security requirements
- ✅ Limited memory resources (RAM < 2GB)
- ✅ Small data volume, short-term use
- ✅ Development and debugging phase

#### Option 2: Run Fast Version (Windows Forms Application)

```bash
dotnet run --project src/SiliconLife.Fast
```

The application will start in forms mode, minimize to the system tray, and run continuously in the background.

**Applicable Scenarios**:
- ✅ High concurrency scenarios (> 5 users)
- ✅ Large data volume (3+ months of usage)
- ✅ Low latency response required
- ✅ Tray background operation required

### Publish Single File

```bash
# Windows - Default version
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Windows - Fast version
dotnet publish src/SiliconLife.Fast -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux - Default version only
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - Default version only
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
- [x] Phase 11: SpeedyPack Storage Engine (Replaced LiteDB, memory mapping, async write queue, auto-compaction)
- [x] Phase 12: Plugin System (IPlugin interface, PluginLoader security sandbox, isolated loading, tool integration)

### 🚧 Planned
- [ ] Phase 13: External IM Integration (Feishu / WhatsApp / Telegram)
- [ ] Phase 14: Skill Ecosystem (Plugin marketplace, skill pack distribution)

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

## 💡 Version Selection Guide

### Which version should I use?

**SiliconLife.Default (Default Implementation — Architecture Verification):**
- 📌 You're new to this project and want to quickly understand the system architecture
- 📌 You're doing development debugging and need a simple, straightforward runtime

**SiliconLife.Fast (Recommended Production Version):**
- ⚡ You need a long-term stable production environment
- ⚡ You're familiar with the system architecture and ready for production deployment
- ⚡ You need multi-user concurrent access support
- ⚡ You need system tray background operation
- ⚡ You pursue extreme performance

> **Overall Recommendation**: SiliconLife.Default is suitable for architecture verification and initial experience; for actual production environments, SiliconLife.Fast is strongly recommended.

### Can I migrate from Default to Fast?

**Absolutely!** Both versions share the same:
- ✅ Configuration file format (config.json)
- ✅ Tool interfaces
- ✅ Being configurations
- ✅ Web UI interface

**Migration Steps:**
1. Back up your Default data directory
2. Start the Fast version using the same data directory
3. Fast will automatically import existing data into the SpeedyPack storage engine
4. After verifying functionality, you can use the Fast version for daily operations

### Can both versions coexist?

**Yes!** The following deployment strategies are recommended:

**Strategy 1: Default for Verification, Fast for Production**
```
Development/Verification Environment: SiliconLife.Default (verify architecture, debug features)
Production Environment: SiliconLife.Fast (high performance, background operation, handle real-time requests)
```

**Strategy 2: Fast for Main Operation, Default for Periodic Backup**
```
SiliconLife.Fast (daily use, handle real-time requests)
    ↓ Periodic backup
SiliconLife.Default (cold data archival, data safety fallback)
```

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
