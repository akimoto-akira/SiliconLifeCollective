![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**Version: v0.2.0-alpha** | **Silicon Life Collective** — A multi-agent collaboration platform based on .NET 9, where AI agents are called **Silicon Beings**, capable of self-evolution through Roslyn dynamic compilation.

[English](../README.md) | [Deutsch](../de-DE/README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Français](../fr-FR/README.md) | [Čeština](../cs-CZ/README.md) | [Italiano](../it-IT/README.md) | [Polski](../pl-PL/README.md) | [Português](../pt-PT/README.md) | [Русский](../ru-RU/README.md)

## 🌟 Core Features

### Agent System
- **Multi-Agent Orchestration** — Unified management by the *Silicon Curator*, using a clock-driven time-slice fair scheduling mechanism
- **Soul File Driven** — Each Silicon Being is driven by a core prompt file (`soul.md`), defining unique personality and behavior patterns
- **Body-Brain Architecture** — *Body* (SiliconBeing) maintains vital signs and detects trigger scenarios; *Brain* (ContextManager) handles loading history, calling AI, executing tools, and persisting responses
- **Self-Evolution Capability** — Through Roslyn dynamic compilation technology, Silicon Beings can rewrite their own code to achieve evolution
- **Activity State Management** — Supports nine activity states: Idle, SingleChat, GroupChat, Task, Timer, Broadcast, Project, MemoryCompression, Stopped. Automatically enters Stopped state after 10 consecutive errors

### Plugin System
- **Plugin Extension Architecture** — Feature extension via IPlugin interface, supporting dynamic loading of plugin DLLs from directories
- **Plugin Capability Declaration** — Plugins declare required capabilities (Network, FileIO, Process, AI) via the `[PluginCapability]` attribute, and the loader relaxes security scanning rules accordingly; non-declarable capabilities (P/Invoke, Unsafe, Reflection Emit, etc.) are always blocked
- **Isolated Loading** — Uses custom AssemblyLoadContext for isolated loading, preventing plugins from affecting main program stability
- **Tool Integration** — Plugins can register custom tools via ITool interface, automatically integrated into the tool call loop

### Tools & Execution
- **24 Built-in Tools** — Covering calendar, chat, configuration, disk, network, memory, tasks, timers, knowledge base, work notes, project workspace, WebView browser, and more
- **Tool Scenario Isolation** — Each tool declares available scenarios via the `ToolScenario` attribute (Chat, Task, Timer, MemoryCompression, Project); the `ChatOnly` attribute restricts tools to chat scenarios only
- **IAIClient Capability Interface** — AI clients declare capabilities for streaming mode, tool calls, context window, vision, and audio, and the ContextManager adapts its behavior accordingly
- **Tool Call Loop** — AI returns tool call → Execute tool → Results fed back to AI → Continue loop until pure text response
- **Executor-Permission Security** — All I/O operations go through strict permission validation via executors
  - 3-level permission validation chain: UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: GlobalACL → Deny by default)
  - Complete audit logging of all permission decisions

### Skill System
- **Reusable Capability Units** — Encapsulate "tool orchestration + prompt templates" as declarable, evolvable, schedulable skills; AI calls skills just like calling tools
- **Dual Trigger Modes** — Manual (AI function call decides autonomously) + Auto (schedule-based: daily fixed time / interval cycle / cron subset)
- **Markdown-First** — YAML frontmatter metadata + prompt body; pure Markdown saved with AI auto-completing missing metadata (user fields are never overwritten)
- **Hot Reload & Version Archiving** — 30-second fingerprint detection for auto-effect; each update archived to `skills/archive/{id}/{version}.md` forming an evolution history
- **Multiple Guardrails** — Global switch, quota limits (default 50/being), global round and timeout clamps, tool whitelist, recursion protection, skill-level action permissions

### MCP Integration
- **External Tool Access** — Connect to external MCP (Model Context Protocol) servers; their tools are auto-injected into all Silicon Beings with `mcp_{serverId}_{toolName}` naming, no code required
- **Dual Transport** — stdio (local subprocess) and http (remote endpoint)
- **User Sovereignty** — Server add/remove/enable/disable is Web UI only; AI-side `mcp` tool is read-only query
- **Consistent Permissions** — MCP wrapper tools are included in the two-level tool permission matrix, can be disabled per being/project

### IM Integration
- **Multi-Instance Architecture** — Simultaneously connect to multiple IM platforms (Web UI / Feishu / WeChat Enterprise / DingTalk), each instance independently started/stopped, messages aggregated and routed
- **OAuth Authorization Wizard** — Feishu one-click authorization (state for CSRF protection, SSE real-time status push), tokens auto-written back to config
- **Key Security** — Config values support `${ENV_VAR}` environment variable placeholders, plaintext keys never stored on disk

### AI & Knowledge
- **Multiple AI Backend Support**
  - **Ollama** — Local model deployment, using native HTTP API
  - **Alibaba Cloud DashScope (Bailian)** — Cloud AI service, OpenAI API compatible, supporting 13+ models, multi-region deployment
  - **DeepSeek** — Cloud AI service, thinking mode, reasoning effort control, up to 1M context window
  - **Zhipu AI (GLM)** — Cloud AI service, thinking mode (GLM-5), vision by model, free model (glm-4-flash), up to 1M context window
  - **Ernie (Baidu/Qianfan)** — Cloud AI service via Qianfan v2, free models (ernie-speed, ernie-tiny), up to 131K context window
  - **Tencent Hunyuan** — Cloud AI service, dual endpoints (TokenHub + Legacy), thinking mode, up to 262K context window
  - **MiniMax** — Cloud AI service, thinking mode with reasoning_split, M3 multimodal, up to 1M context window
  - **Moonshot (Kimi)** — Cloud AI service, thinking mode, multimodal, up to 262K context window
  - **SiliconFlow** — Cloud AI service, dynamic model list aggregation (100+ models), up to 1M context window
  - **Volcengine Ark** — ByteDance cloud AI service, supporting streaming and non-streaming modes, built-in rate control
  - **Herdsman** — Authentication-free inference engine, compatible with OpenAI API format
  - **Meituan LongCat** — Meituan's self-developed large model, compatible with OpenAI API format, API key authentication
  - **Qiniu Cloud AI** — Qiniu cloud AI service, API key authentication
- **32 Calendar Systems** — Comprehensive coverage of global major calendars, including Gregorian, Chinese Lunar, Islamic, Hebrew, Japanese, Persian, Mayan, Chinese Historical Calendar, and more
- **Knowledge Network System** — Knowledge graph based on triplets (subject-relation-object), supporting storage, querying, and path discovery
- **Project Workspace** — Project space management, supporting project creation/archival/destruction, role assignment, work notes, task tracking, and tool permission isolation
- **Workflow Engine** — Template-based state machine engine, supporting custom workflow templates, state transitions, tick-driven execution, and instance lifecycle management
- **Memory Fade Mechanism** — Scheduled decay service (MemoryFadeService) that automatically performs importance decay and auto-archival on all Silicon Beings' memories every hour

### Web Interface
- **Modern Web UI** — Built-in HTTP server with SSE real-time updates
- **7 Skin Themes** — Admin, Chat, Creative, Dev, High Contrast, Light, Minimal, supporting auto-discovery and switching
- **24 Controllers** — Complete system management, chat, configuration, and monitoring functionality
- **Zero Frontend Framework Dependency** — HTML/CSS/JS generated server-side via `H`, `CssBuilder`, and `JsBuilder`

### Internationalization & Localization
- **34 Language Variants** fully supported, covering 2 writing systems and multiple regional variants
  - **Simplified Chinese**: zh-CN (China mainland), zh-SG (Singapore), zh-MY (Malaysia) (3 variants)
  - **Traditional Chinese**: zh-HK (Hong Kong), zh-TW (Taiwan), zh-MO (Macau) (3 variants)
  - **English**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 variants)
  - **Spanish**: es-ES, es-MX (2 variants)
  - **German**: de-DE, de-AT, de-CH, de-LU, de-LI (5 variants)
  - **French**: fr-FR, fr-CA, fr-CH (3 variants)
  - **Japanese**: ja-JP | **Korean**: ko-KR | **Czech**: cs-CZ (3 variants)
  - **Italian**: it-IT | **Polish**: pl-PL | **Portuguese**: pt-PT, pt-BR (4 variants)

### Data & Storage
- **SpeedyPack High-Performance Storage** — Fast version uses custom .spk storage engine, in-memory directory mapping + entry cache + asynchronous write queue
- **File System Storage** — Default version uses pure file system JSON storage
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
- **Runtime Mode**: Desktop application (Windows/macOS system tray / Linux status window)
- **Storage Method**: SpeedyPack in-memory storage + asynchronous batch persistence (.spk file format)
- **Applicable Scenarios**: High concurrency, low latency, large data volume scenarios
- **Platform Support**: Windows/macOS (full features, including system tray), Linux (status window, no tray icon)
- **Features**:
  - Extreme performance optimization
  - Windows/macOS tray background operation with real-time monitoring via tray status window; Linux status window displayed directly
  - SpeedyPack engine + auto-compaction ensures data security
  - Component UI architecture, 27 declarative components
  - 7 skin themes with auto-discovery and switching
  - Hot reload tool for online updates and restarts → Linux automatically opens browser to access Web UI, supports `--no-tray` parameter
- **Performance Improvement**: Storage read latency reduced by 1000x, write latency reduced by 15000x, concurrent processing capacity increased by 50x
- **Role Description**: A production-grade implementation with deep optimization, the first choice for long-term operation and actual production environments
- **Startup Command**: `dotnet run --project src/SiliconLife.Fast`

### Version Comparison

| Feature | SiliconLife.Default | SiliconLife.Fast |
|---------|---------------------|------------------|
| **Runtime Mode** | Console application | Desktop application (Windows/macOS system tray / Linux status window) |
| **User Interface** | Web UI (browser access) | Windows/macOS: Tray icon + tray window + Web UI; Linux: Status window + Web UI |
| **System Tray** | ❌ None | ✅ Windows/macOS supports minimize to tray; Linux no tray icon |
| **Background Operation** | ❌ Exits when console closes | ✅ Windows/macOS continuous tray background operation; Linux status window operation |
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
| Runtime | .NET 9 | .NET 9 (Windows/macOS/Linux) |
| Programming Language | C# | C# |
| Application Type | Console application | Desktop application (Windows/macOS system tray / Linux status window) |
| AI Integration | Ollama (local), Alibaba Cloud DashScope (cloud), DeepSeek, Zhipu AI (GLM), Ernie/Baidu Qianfan, Tencent Hunyuan, MiniMax, Moonshot/Kimi, SiliconFlow, Volcengine Ark (cloud), Herdsman, Meituan LongCat, Qiniu Cloud AI | Ollama (local), Alibaba Cloud DashScope (cloud), DeepSeek, Zhipu AI (GLM), Ernie/Baidu Qianfan, Tencent Hunyuan, MiniMax, Moonshot/Kimi, SiliconFlow, Volcengine Ark (cloud), Herdsman, Meituan LongCat, Qiniu Cloud AI |
| Data Storage | File system (JSON + time-indexed directories) | SpeedyPack (.spk format, memory mapping + asynchronous persistence) |
| Web Server | HttpListener (.NET built-in) | HttpListener (.NET built-in) |
| Dynamic Compilation | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Browser Automation | Playwright (WebView) | Playwright (WebView) |
| Plugin System | ✅ Supported (IPlugin + PluginLoader) | ✅ Supported (IPlugin + PluginLoader) |
| System Tray | ❌ Not supported | ✅ Windows/macOS supported (NotifyIcon); Linux no tray icon |
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
│   │   ├── SiliconBeing/                  # Silicon Being base class, manager, factory
│   │   ├── Storage/                       # Storage interfaces
│   │   ├── Time/                          # Incomplete dates (time range queries)
│   │   ├── Tools/                         # Tool interfaces and tool manager
│   │   ├── WebView/                       # WebView browser interface
│   │   ├── Workflow/                      # Workflow engine (templates, instances, state transitions)
│   │   └── ServiceLocator.cs              # Global service locator
│   │
│   ├── SiliconLife.Common/                # Shared implementation (used by both versions)
│   │   ├── AI/                            # AI clients and factories (Ollama, DashScope, DeepSeek, Zhipu, Ernie, Hunyuan, MiniMax, Moonshot, SiliconFlow, VolcengineArk, Herdsman, LongCat, QiniuAI)
│   │   ├── Calendar/                      # 32 calendar implementations
│   │   ├── Localization/                  # Localization base classes and 34 language/region variant implementations
│   │   ├── Resources/                     # Shared resource files
│   │   ├── Security/                      # Permission manager
│   │   ├── SiliconBeing/                  # Default Silicon Being implementation
│   │   ├── Tools/                         # 23 common tool implementations
│   │   ├── Web/                           # Web infrastructure
│   │   └── WebView/                       # Playwright WebView implementation
│   │
│   ├── SiliconLife.App/                   # Application layer (Web UI + Help docs, shared by Default and Fast)
│   │   ├── Config/                        # Application configuration
│   │   ├── Data/                          # Data directory
│   │   ├── Help/                          # Help documentation localization (multi-language)
│   │   ├── Tools/                         # HelpTool (help documentation query tool)
│   │   └── Web/                           # Web UI implementation
│   │       ├── Component/                 # UI component library (27 components)
│   │       ├── Controllers/               # 24 controllers
│   │       ├── Models/                    # View models
│   │       ├── Views/                     # HTML views
│   │       └── Skins/                     # 7 skin themes
│   │
│   ├── SiliconLife.Default/               # Default implementation + application entry (console version)
│   │   ├── Program.cs                     # Entry point (assembles all components)
│   │   ├── Config/                        # Default configuration data
│   │   ├── Knowledge/                     # Knowledge network implementation
│   │   ├── Logging/                       # Log provider implementations (console + file system)
│   │   ├── Project/                       # Project system implementation
│   │   └── Storage/                       # File system storage implementation
│   │
│   ├── SiliconLife.Fast/                  # High-performance implementation + application entry (forms version)
│   │   ├── Program.cs                     # Entry point (forms application)
│   │   ├── App.axaml / App.cs             # Avalonia application definition
│   │   ├── Config/                        # Configuration data (shared with Default)
│   │   ├── Knowledge/                     # Knowledge network implementation (memory-optimized)
│   │   ├── Logging/                       # High-performance log providers
│   │   ├── Project/                       # Project system implementation
│   │   ├── Storage/                       # SpeedyPack storage adapter
│   │   └── Tray/                          # System tray (34 language variant localization)
│   │
│   ├── SiliconLife.Speedy/                # SpeedyPack high-performance storage engine
│   │   ├── SpeedyPack.cs                  # Core class (in-memory directory mapping + cache + async write)
│   │   ├── SpeedyPackOptions.cs           # Configuration options (cache TTL, max entries, etc.)
│   │   ├── IPackTransaction.cs            # Transaction interface
│   │   ├── SpkFileInfo.cs                 # File information
│   │   └── Internal/                      # Internal implementation
│   │       ├── DirectoryMap.cs            # In-memory directory mapping
│   │       ├── EntryCache.cs              # Entry cache
│   │       ├── FreeList.cs                # Free space management
│   │       ├── PackFileReader.cs          # Pack file reader
│   │       ├── PackFileWriter.cs          # Pack file writer
│   │       ├── WriteQueue.cs              # Asynchronous write queue
│   │       ├── WriteOperation.cs          # Write operation
│   │       ├── SpeedyTransaction.cs       # Transaction implementation
│   │       ├── SpkHeader.cs               # Pack file header
│   │       └── PathNormalizer.cs          # Path normalization
│   │
│   └── SiliconLife.Speedy.Manager/        # SpeedyPack management tool (Avalonia UI)
│       ├── MainForm.cs                    # Main form
│       ├── Program.cs                     # Entry point
│       └── slc.ico                        # Application icon
│
├── docs/                                  # Multi-language documentation
│   ├── zh-CN/                             # Simplified Chinese documentation
│   ├── en/                                # English documentation
│   └── ...                                # Other language documentation
│
└── docs/                                 # Requirements and architecture documentation
    ├── Requirements.md
    ├── Architecture_Outline.md
    └── Implementation_Order.md
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
Tool Call → Executor → Permission Manager → [Frequency Cache → Callback → (IsCurator: Ask User | Non-curator: Global ACL)]
```

## 🚀 Quick Start

### Prerequisites

- **.NET 9 SDK** — [Download Link](https://dotnet.microsoft.com/download/dotnet/9.0)
- **AI Backend** (choose one):
  - **Ollama**: [Install Ollama](https://ollama.com) and pull models (e.g., `ollama pull llama3`)
  - **Alibaba Cloud DashScope**: Get API key from [DashScope Console](https://bailian.console.aliyun.com/)
  - **Volcengine Ark**: Get API key from [Volcengine Console](https://console.volcengine.com/ark)
  - **Herdsman**: No authentication required, compatible with OpenAI API format
  - **Meituan LongCat**: Get API key from Meituan platform
  - **Qiniu Cloud AI**: Get API key from [Qiniu Console](https://portal.qiniu.com/)
  - **DeepSeek**: Get API key from [DeepSeek Platform](https://platform.deepseek.com/)
  - **Zhipu AI (GLM)**: Get API key from [Zhipu Open Platform](https://open.bigmodel.cn/)
  - **Baidu Qianfan**: Get API key from [Qianfan Console](https://console.bce.baidu.com/qianfan/)
  - **Tencent Hunyuan**: Get API key from [Tencent Cloud](https://cloud.tencent.com/product/hunyuan)
  - **MiniMax**: Get API key from [MiniMax Platform](https://platform.minimaxi.com/)
  - **Moonshot (Kimi)**: Get API key from [Moonshot Platform](https://platform.moonshot.cn/)
  - **SiliconFlow**: Get API key from [SiliconFlow Cloud](https://cloud.siliconflow.cn/)

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

#### Option 2: Run Fast Version (Desktop Application)

```bash
dotnet run --project src/SiliconLife.Fast
```

**Windows/macOS**: The application will start in forms mode, minimize to the system tray, and run continuously in the background.

**Linux**: The application will display a status window (no system tray icon) and automatically open the browser to access the Web UI. You can also use the `--no-tray` parameter to skip auto-opening the browser:

```bash
dotnet run --project src/SiliconLife.Fast -- --no-tray
```

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

# Linux - Default version
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# Linux - Fast version
dotnet publish src/SiliconLife.Fast -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - Default version
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true

# macOS - Fast version
dotnet publish src/SiliconLife.Fast -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 Development Roadmap

### ✅ Completed
- [x] Phase 1: Console AI Chat
- [x] Phase 2: Framework Skeleton (Main Loop + Clock Objects + Watchdog + Circuit Breaker)
- [x] Phase 3: First Silicon Being with Soul File (Body-Brain Architecture)
- [x] Phase 4: Persistent Memory (Chat System + Time Storage Interface)
- [x] Phase 5: Tool System + Executors
- [x] Phase 6: Permission System (5-level chain, Audit Logger, Global ACL)
- [x] Phase 7: Dynamic Compilation + Self-Evolution (Roslyn)
- [x] Phase 8: Long-term Memory + Tasks + Timers
- [x] Phase 9: Core Host + Multi-Agent Collaboration
- [x] Phase 10: Web UI (HTTP + SSE, 24 Controllers, 7 Skins)
- [x] Phase 10.5: Incremental Enhancements (Broadcast Channels, Token Audit, 32 Calendars, Tool Enhancements, 34 Language Variant Localization)
- [x] Phase 10.6: Refinement & Optimization (WebView, Help System, Project Workspace, Knowledge Network, Workflow Engine)
- [x] Phase 11: SpeedyPack Storage Engine (Replaced LiteDB, memory mapping, async write queue, auto-compaction)
- [x] Phase 12: Plugin System (IPlugin interface, PluginLoader Capability Declaration, isolated loading, tool integration)

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
- 📌 Data security is your top priority
- 📌 Your system has less than 4GB of memory
- 📌 You only need single-user access or have a small data volume

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
