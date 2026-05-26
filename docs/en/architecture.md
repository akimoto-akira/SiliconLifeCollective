# Architecture

> **Version: v0.2.0-alpha**

[**English**](../en/architecture.md) | [Deutsch](../de-DE/architecture.md) | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Čeština](../cs-CZ/architecture.md) | [Русский](../ru-RU/architecture.md)

## Dual-Version Architecture

This project provides two implementation versions that share the same architectural design but differ in storage and performance optimization:

### SiliconLife.Default (Default Version)
- **Positioning**: Default implementation, primarily used to validate architectural feasibility
- **Runtime Mode**: Console application
- **Storage**: Pure file system JSON storage
- **Use Cases**: Scenarios requiring high data safety, limited memory resources, or small data volumes
- **Role**: Serves as the baseline implementation for architectural validation, providing a simple and reliable runtime suitable for first-time users, development debugging, or data-safety-prioritized scenarios

### SiliconLife.Fast (High-Performance Version)
- **Positioning**: Primary production version
- **Runtime Mode**: Desktop application (Windows system tray / Linux status window)
- **Storage**: SpeedyPack in-memory storage + asynchronous batch persistence (.spk file format)
- **Use Cases**: High-concurrency, low-latency, large-data-volume scenarios
- **Platform Support**: Windows/macOS (full features including system tray), Linux (status window, no tray icon)
- **Features**:
  - Windows/macOS system tray background running with real-time tray status window monitoring; Linux status window displayed directly
  - SpeedyPack Storage Engine + auto-compaction to ensure data safety
  - Component UI architecture with 27 declarative components
  - 7 skin themes with auto-discovery and switching
  - Hot-reload tool support for online updates and restarts
  - Linux auto-opens browser for Web UI access; supports `--no-tray` parameter
- **Performance Gains**: Storage read latency reduced by 1000x, write latency reduced by 15000x
- **Role**: Deeply optimized production-grade implementation with system tray background running, SpeedyPack Storage Engine + auto-compaction features; the preferred choice for long-running and actual production environments

> **Note**: The architecture described in this document applies to both versions, differing only in the storage implementation. SiliconLife.Default serves as the architectural validation baseline, while SiliconLife.Fast is the recommended production version.

---

## Core Concepts

### Silicon Being

Each AI agent in the system is a **Silicon Being** — an autonomous entity with its own identity, personality, and capabilities. Each Silicon Being is driven by a **Soul File** (a Markdown prompt) that defines its behavioral patterns.

### Silicon Curator

The **Silicon Curator** is a special Silicon Being with the highest system privileges. It acts as the system administrator:

- Creates and manages other Silicon Beings
- Analyzes user requests and decomposes them into tasks
- Distributes tasks to appropriate Silicon Beings
- Monitors execution quality and handles failures
- Responds to user messages using **priority scheduling** (see below)

### Soul File

A Markdown file (`soul.md`) stored in each Silicon Being's data directory. It is injected as a system prompt into every AI request, defining the being's personality, decision-making patterns, and behavioral constraints.

---

## Scheduling: Time-Slot Fair Scheduling

### Main Loop + Clock Objects

The system runs a **clock-driven Main Loop** on a dedicated background thread:

```
Main Loop (dedicated thread, Watchdog + Circuit Breaker)
  └── Clock Object A (priority=0, interval=100ms)
  └── Clock Object B (priority=1, interval=500ms)
  └── Silicon Being Manager (clock-triggered directly by Main Loop)
        └── Silicon Being Runner → Being 1 → clock trigger → execute one round
        └── Silicon Being Runner → Being 2 → clock trigger → execute one round
        └── Silicon Being Runner → Being 3 → clock trigger → execute one round
        └── ...
```

Key design decisions:

- **Silicon Beings do not inherit Tick Objects.** They have their own `Tick()` method, called by `SiliconBeingManager` through `SiliconBeingRunner`, rather than being registered directly with the Main Loop.
- **Silicon Being Manager** is clock-triggered directly by the Main Loop and acts as the single proxy for all beings.
- **Silicon Being Runner** wraps each being's `Tick()` on a transient thread with timeout and per-being circuit breaker (3 consecutive timeouts → 1-minute cooldown).
- Each being's execution is limited to **one round** of AI request + tool call per clock tick, ensuring no being can monopolize the Main Loop.
- **Performance Monitor** tracks clock execution times for observability.

### Curator Priority Response

When a user sends a message to the Silicon Curator:

1. The current being (e.g., Being A) completes its current round — **no interruption**.
2. The manager **skips the remaining queue**.
3. The loop **restarts from the Curator**, causing it to execute immediately.

This ensures responsiveness to user interactions without disrupting in-progress tasks.

---

## Component Architecture

```
┌─────────────────────────────────────────────────────────┐
│                      Core Host                          │
│  (Unified host — assembles and manages all components)  │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │Main Loop │  │Service Locator│  │     Config       │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │       Silicon Being Manager (Tick Object)         │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Curator  │ │Being A  │ │Being B  │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Shared Services                      │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Chat System│ │ Storage │  │Permission Manager│  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │AI Client │  │Executor │  │  Tool Manager    │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  │  ┌──────────┐ ┌──────────┐                        │   │
│  │  │Plugin    │ │Knowledge │                        │   │
│  │  │Loader    │ │Network   │                        │   │
│  │  └──────────┘ └──────────┘                        │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Executor                         │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │  Disk    │ │ Network  │ │  CommandLine     │  │   │
│  │  │ Executor │ │ Executor │ │  Executor        │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              IM Provider                         │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Console  │ │  Web     │ │  Feishu / ...    │  │   │
│  │  │ Provider │ │ Provider │ │  Provider        │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## Service Locator

`ServiceLocator` is a thread-safe singleton registry providing access to all core services:

| Property | Type | Description |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | Central chat session manager |
| `IMManager` | `IMManager` | IM Provider router |
| `AuditLogger` | `AuditLogger` | Permission audit trail |
| `GlobalAcl` | `GlobalACL` | Global Access Control List |
| `BeingFactory` | `ISiliconBeingFactory` | Factory for creating beings |
| `BeingManager` | `SiliconBeingManager` | Active being lifecycle manager |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Dynamic compilation loader |
| `TokenUsageAudit` | `ITokenUsageAudit` | Token usage tracking |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Token usage reporting |

It also maintains a per-being `PermissionManager` registry, keyed by being GUID.

---

## Chat System

### Session Types

The Chat System supports three session types through `SessionBase`:

| Type | Class | Description |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | One-on-one conversation between two participants |
| `GroupChat` | `GroupChatSession` | Multi-participant group chat |
| `Broadcast` | `BroadcastChannel` | Open channel with a fixed ID; beings dynamically subscribe and only receive messages after subscribing |

### Broadcast Channel

`BroadcastChannel` is a special session type used for system-wide announcements:

- **Fixed Channel ID** — Unlike `SingleChatSession` and `GroupChatSession`, the channel ID is a well-known constant rather than derived from member GUIDs.
- **Dynamic Subscription** — Beings subscribe/unsubscribe at runtime; they only receive messages published after their subscription.
- **Pending Message Filtering** — `GetPendingMessages()` only returns messages published after the being's subscription time that have not yet been read.
- **Managed by Chat System** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### Chat Message

The `ChatMessage` model includes fields for AI conversation context and token tracking:

| Field | Type | Description |
|-------|------|-------------|
| `Id` | `Guid` | Unique message identifier |
| `SenderId` | `Guid` | Unique identifier of the sender |
| `ChannelId` | `Guid` | Channel/conversation identifier |
| `Content` | `string` | Message content |
| `Timestamp` | `DateTime` | Message send time |
| `Type` | `MessageType` | Text, image, file, or system notification |
| `ReadBy` | `List<Guid>` | Participant IDs who have read this message |
| `Role` | `MessageRole` | AI conversation role (user, assistant, tool) |
| `ToolCallId` | `string?` | Tool call ID for tool result messages |
| `ToolCallsJson` | `string?` | Serialized tool call JSON for assistant messages |
| `Thinking` | `string?` | AI's chain-of-thought reasoning |
| `PromptTokens` | `int?` | Number of tokens in the prompt (input) |
| `CompletionTokens` | `int?` | Number of tokens in the completion (output) |
| `TotalTokens` | `int?` | Total tokens used (input + output) |
| `FileMetadata` | `FileMetadata?` | Attached file metadata (if the message contains a file) |

### Chat Message Queue

`ChatMessageQueue` is a thread-safe message queue system for managing asynchronous processing of chat messages:

- **Thread Safety** - Uses locking mechanisms to ensure safe concurrent access
- **Asynchronous Processing** - Supports asynchronous message enqueue and dequeue
- **Message Ordering** - Maintains chronological ordering of messages
- **Batch Operations** - Supports batch retrieval of messages

### File Metadata

`FileMetadata` manages file information attached to chat messages:

- **File Information** - File name, size, type, path
- **Upload Time** - Timestamp of file upload
- **Uploader** - User or Silicon Being ID that uploaded the file

### Stream Cancellation Manager

`StreamCancellationManager` provides cancellation mechanisms for AI streaming responses:

- **Stream Control** - Supports cancellation of in-progress AI streaming responses
- **Resource Cleanup** - Properly cleans up related resources upon cancellation
- **Concurrency Safety** - Supports simultaneous management of multiple streams

### Chat History Viewing

The chat history viewing feature allows users to browse historical conversations of Silicon Beings:

- **Session List** - Displays all historical sessions
- **Message Details** - View complete message history
- **Timeline View** - Display messages in chronological order
- **API Support** - Provides RESTful API for retrieving session and message data

---

## AI Client System

The system supports multiple AI backends through the `IAIClient` interface:

### OllamaClient

- **Type**: Local AI service
- **Protocol**: Native Ollama HTTP API (`/api/chat`, `/api/generate`)
- **Features**: Streaming, tool calls, local model hosting
- **Configuration**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (Alibaba Cloud Bailian)

- **Type**: Cloud AI service
- **Protocol**: OpenAI-compatible API (`/compatible-mode/v1/chat/completions`)
- **Authentication**: Bearer token (API key)
- **Features**: Streaming, tool calls, reasoning content (chain-of-thought), multi-region deployment
- **Supported Regions**:
  - `beijing` — North China 2 (Beijing)
  - `virginia` — US (Virginia)
  - `singapore` — Singapore
  - `hongkong` — Hong Kong, China
  - `frankfurt` — Germany (Frankfurt)
- **Supported Models** (dynamically discovered via API, with fallback list):
  - **Qwen Series**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **Reasoning**: qwq-plus
  - **Third-party**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **Configuration**: `apiKey`, `region`, `model`
- **Model Discovery**: Fetches available models from the Bailian API at runtime; falls back to a curated list on network failure

### VolcengineArkClient (Volcengine Ark)

- **Type**: Cloud AI service
- **Protocol**: OpenAI-compatible API
- **Authentication**: Bearer token (API key)
- **Features**: Supports streaming and non-streaming modes, built-in dual-layer rate control
  - Self rate control: Enforces minimum interval between requests
  - Server rate limiting: Handles 429 errors with exponential backoff retry
- **Configuration**: `apiKey`, `endpoint`, `model`
- **Characteristics**: ByteDance AI service, supports various Doubao models

### Client Factory Pattern

Each AI client type has a corresponding factory implementing `IAIClientFactory`:

- `OllamaClientFactory` — Creates OllamaClient instances
- `DashScopeClientFactory` — Creates DashScopeClient instances
- `VolcengineArkClientFactory` — Creates VolcengineArkClient instances

Factories provide:
- `CreateClient(Dictionary<string, object> config)` — Instantiates a client from configuration
- `GetConfigKeyOptions(string key, ...)` — Returns dynamic options for configuration keys (e.g., available models, regions)
- `GetDisplayName()` — Localized display name for the client type

### AI Platform Support List

#### Status Legend
- ✅ Implemented
- 🚧 In Development
- 📋 Planned
- 💡 Under Consideration

*Note: Due to the developer's network environment, accessing overseas cloud AI services that are "Under Consideration" may require network proxy tools, and the debugging process may be unstable.*

#### Platform List

| Platform | Status | Type | Description |
|----------|--------|------|-------------|
| Ollama | ✅ | Local | Local AI service, supports local model deployment |
| DashScope (Alibaba Cloud Bailian) | ✅ | Cloud | Alibaba Cloud Bailian AI service, supports multi-region deployment |
| Baidu Qianfan (Wenxin Yiyan) | 📋 | Cloud | Baidu Wenxin Yiyan AI service |
| Zhipu AI (GLM) | 📋 | Cloud | Zhipu Qingyan AI service |
| Moonshot (Kimi) | 📋 | Cloud | Moonshot Kimi AI service |
| Volcengine Ark Doubao | ✅ | Cloud | ByteDance Doubao AI service |
| DeepSeek (Direct) | 📋 | Cloud | DeepSeek AI service |
| Yi (01.AI) | 📋 | Cloud | Yi AI service |
| Tencent Hunyuan | 📋 | Cloud | Tencent Hunyuan AI service |
| SiliconFlow | 📋 | Cloud | SiliconFlow AI service |
| MiniMax | 📋 | Cloud | MiniMax AI service |
| OpenAI | 💡 | Cloud | OpenAI API service (GPT series) |
| Anthropic | 💡 | Cloud | Anthropic Claude AI service |
| Google DeepMind | 💡 | Cloud | Google Gemini AI service |
| Mistral AI | 💡 | Cloud | Mistral AI service |
| Groq | 💡 | Cloud | Groq high-speed AI inference service |
| Together AI | 💡 | Cloud | Together AI open-source model service |
| xAI | 💡 | Cloud | xAI Grok service |
| Cohere | 💡 | Cloud | Cohere enterprise NLP service |
| Replicate | 💡 | Cloud | Replicate open-source model hosting platform |
| Hugging Face | 💡 | Cloud | Hugging Face open-source AI community and model platform |
| Cerebras | 💡 | Cloud | Cerebras AI inference optimization service |
| Databricks | 💡 | Cloud | Databricks enterprise AI platform (MosaicML) |
| Perplexity AI | 💡 | Cloud | Perplexity AI search Q&A service |
| NVIDIA NIM | 💡 | Cloud | NVIDIA AI inference microservice |

---

## Key Design Decisions

### Storage as Instance Class (Not Static)

`IStorage` is designed as an injectable instance rather than a static utility. This ensures:

- Direct file system access — IStorage is the system's internal persistence channel, **not** routed through executors.
- **AI cannot control IStorage** — Executors manage IO initiated by AI tools; IStorage manages the framework's own internal data reads and writes. These are fundamentally different concerns.
- Testability with mock implementations.
- Future support for different storage backends without modifying consumers.

### Executor as Security Boundary

Executors are the **sole** path for I/O operations. Tools requiring disk, network, or command-line access **must** go through executors. This design enforces:

- Each executor has an **independent dispatch thread** with thread locking for permission validation.
- Centralized permission checking — Executors query the being's **private Permission Manager**.
- Request queues with priority and timeout control.
- Audit logging for all external operations.
- Exception isolation — One executor's failure does not affect other executors.
- Circuit Breaker — Consecutive failures temporarily halt the executor to prevent cascading failures.

### Context Manager as Lightweight Object

Each `ExecuteOneRound()` creates a new `ContextManager` instance:

1. Loads the Soul File + recent chat history.
2. Sends the request to the AI client.
3. Loops through tool calls until the AI returns plain text.
4. Persists the response to the Chat System.
5. Releases.

This keeps each round isolated and stateless.

### Self-Evolution via Class Override

Silicon Beings can rewrite their own C# classes at runtime:

1. The AI generates new class code (must inherit `SiliconBeingBase`).
2. **Compile-time reference control** (primary defense): The compiler only receives the allowed assembly list — `System.IO`, `System.Reflection`, etc. are excluded, so dangerous code is impossible at the type level.
3. **Runtime static analysis** (secondary defense): `SecurityScanner` scans the code for dangerous patterns after successful compilation.
4. Roslyn compiles the code in memory.
5. On success: `SiliconBeingManager.ReplaceBeing()` swaps the current instance, migrates state, and persists encrypted code to disk.
6. On failure: The new code is discarded, and the existing implementation is retained.

Custom `IPermissionCallback` implementations can also be compiled and injected via `ReplacePermissionCallback()`, allowing beings to customize their own permission logic.

Code is stored on disk encrypted with AES-256. The encryption key is derived from the being's GUID (uppercase) via PBKDF2.

---

## Token Usage Audit

`TokenUsageAuditManager` tracks AI token consumption across all beings:

- `TokenUsageRecord` — Per-request record (being ID, model, prompt tokens, completion tokens, timestamp)
- `TokenUsageSummary` — Aggregated statistics
- `TokenUsageQuery` — Query parameters for filtering records
- Persisted via `ITimeStorage` for time-series queries
- Accessible through Web UI (UsageController) and `TokenAuditTool` (Curator only)

---

### Calendar System

The system includes **32 calendar implementations** derived from the abstract `CalendarBase` class, covering major world calendar systems:

| Calendar | ID | Description |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | Buddhist Era (BE), year + 543 |
| CherokeeCalendar | `cherokee` | Cherokee calendar system |
| ChineseLunarCalendar | `lunar` | Chinese lunar calendar with leap months |
| ChineseHistoricalCalendar | `chinese_historical` | Chinese historical calendar, supports sexagenary cycle and imperial era names |
| ChulaSakaratCalendar | `chula_sakarat` | Chula Sakarat (CS), year - 638 |
| CopticCalendar | `coptic` | Coptic calendar |
| DaiCalendar | `dai` | Dai calendar with full lunar calculation |
| DehongDaiCalendar | `dehong_dai` | Dehong Dai calendar variant |
| EthiopianCalendar | `ethiopian` | Ethiopian calendar |
| FrenchRepublicanCalendar | `french_republican` | French Republican calendar |
| GregorianCalendar | `gregorian` | Standard Gregorian calendar |
| HebrewCalendar | `hebrew` | Hebrew (Jewish) calendar |
| IndianCalendar | `indian` | Indian national calendar |
| InuitCalendar | `inuit` | Inuit calendar system |
| IslamicCalendar | `islamic` | Islamic Hijri calendar |
| JapaneseCalendar | `japanese` | Japanese era (Nengo) calendar |
| JavaneseCalendar | `javanese` | Javanese Islamic calendar |
| JucheCalendar | `juche` | Juche calendar (DPRK), year - 1911 |
| JulianCalendar | `julian` | Julian calendar |
| KhmerCalendar | `khmer` | Khmer calendar |
| MayanCalendar | `mayan` | Mayan Long Count calendar |
| MongolianCalendar | `mongolian` | Mongolian calendar |
| PersianCalendar | `persian` | Persian (Solar Hijri) calendar |
| RepublicOfChinaCalendar | `roc` | Republic of China (Minguo) calendar, year - 1911 |
| RomanCalendar | `roman` | Roman calendar |
| SakaCalendar | `saka` | Saka calendar (Indonesia) |
| SexagenaryCalendar | `sexagenary` | Chinese sexagenary (Ganzhi) calendar |
| TibetanCalendar | `tibetan` | Tibetan calendar |
| VietnameseCalendar | `vietnamese` | Vietnamese lunar calendar (cat zodiac variant) |
| VikramSamvatCalendar | `vikram_samvat` | Vikram Samvat calendar |
| YiCalendar | `yi` | Yi calendar system |
| ZoroastrianCalendar | `zoroastrian` | Zoroastrian calendar |

`CalendarTool` provides operations: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (cross-calendar date conversion).

---

## Web UI Architecture

### Skin System

The Web UI features a **pluggable skin system** that allows complete UI customization without changing application logic:

- **ISkin Interface** — Defines the contract for all skins, including:
  - Core rendering methods (`RenderHtml`, `RenderError`)
  - 20+ UI component methods (buttons, inputs, cards, tables, badges, bubbles, progress, tabs, etc.)
  - Theme CSS generation via `CssBuilder`
  - `SkinPreviewInfo` — Color palette and icons for the initialization page skin selector

- **Built-in Skins** — 7 production-ready skins:
  - **Admin** — Professional, data-focused system management interface
  - **Chat** — Conversational, message-centric design for AI interaction
  - **Creative** — Artistic, visually rich creative workflow layout
  - **Dev** — Developer-centric, code-focused interface with syntax highlighting
  - **HighContrast** — High-contrast accessibility theme
  - **Light** — Clean light theme
  - **Minimal** — Minimalist theme

- **Skin Discovery** — `SkinManager` automatically discovers and registers all `ISkin` implementations via reflection

### HTML / CSS / JS Builders

The Web UI entirely avoids template files, generating all markup in C#:

- **`H`** — Fluent HTML builder DSL for constructing HTML trees in code
- **`CssBuilder`** — CSS builder with selector and media query support
- **`JsBuilder` (`JsSyntax`)** — JavaScript builder for inline scripts

### Controller System

The Web UI follows a **MVC-like pattern** with 24 controllers handling different aspects:

| Controller | Purpose |
|------------|---------|
| About | About page and project information |
| Audit | Token Usage Audit dashboard |
| Being | Silicon Being management and status |
| Chat | Real-time chat interface with SSE |
| ChatHistory | Chat history viewing with session list and message details |
| CodeBrowser | Code viewing and editing |
| CodeHover | Code hover tooltips with syntax highlighting |
| Config | System configuration management |
| Dashboard | System overview and metrics |
| Executor | Executor status and management |
| Help | Help documentation system, multi-language support |
| Init | First-run initialization wizard |
| Knowledge | Knowledge graph visualization and querying |
| Log | System log viewer with Silicon Being filtering |
| Memory | Long-term memory browser with advanced filtering, statistics, and detail views |
| Permission | Permission management |
| PermissionRequest | Permission request queue |
| Project | Project management with work notes, task system, and tool permissions |
| System | System administration and runtime monitoring |
| Task | Task system interface |
| Timer | Timer system management with execution history |
| ToolPermission | Tool permission management with Silicon Being and project-level permission configuration |
| Usage | Token Usage Audit dashboard with trend charts and export |
| WorkNote | Work note management with search and directory generation |

### Real-Time Updates

- **SSE (Server-Sent Events)** — Pushes updates for chat messages, being status, and system events via `SSEHandler`
- **No WebSocket Required** — Simpler architecture using SSE for most real-time needs
- **Auto-Reconnect** — Client reconnection logic for resilient connections

### Localization

The system supports comprehensive localization in **34 language variants**:
- **Chinese (6)**: zh-CN (Simplified), zh-HK (Traditional), zh-SG (Singapore), zh-MO (Macau), zh-TW (Taiwan), zh-MY (Malaysia)
- **English (10)**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **Spanish (2)**: es-ES, es-MX
- **German (5)**: de-DE, de-AT, de-CH, de-LU, de-LI
- **French (3)**: fr-FR, fr-CA, fr-CH
- **Others (8)**: ja-JP (Japanese), ko-KR (Korean), cs-CZ (Czech), it-IT (Italian), pl-PL (Polish), pt-PT (Portuguese), pt-BR (Brazilian Portuguese), ru-RU (Russian)

The active locale is selected via `DefaultConfigData.Language` and resolved through `LocalizationManager`.

---

### WebView Browser Automation System (New)

The system integrates **Playwright**-based WebView browser automation:

- **Per-Being Isolation**: Each Silicon Being has its own browser instance, cookies, and session storage, fully isolated from one another.
- **Headless Mode**: The browser runs in a completely invisible headless mode, with Silicon Beings operating autonomously in the background.
- **WebViewBrowserTool**: Provides complete browser operation capabilities, including:
  - Page navigation, clicking, text input, page content retrieval
  - JavaScript execution, screenshot capture, waiting for elements
  - Browser state management and resource cleanup
- **Security Control**: All browser operations must pass through the permission validation chain, preventing malicious web access.

### Knowledge Network System (New)

The system includes a built-in **triple-structure**-based knowledge graph system:

- **Knowledge Representation**: Uses "subject-relation-object" triple structure (e.g., Python-is_a-programming_language)
- **KnowledgeTool**: Provides full lifecycle management of knowledge:
  - `add`/`query`/`update`/`delete` - Basic CRUD operations
  - `search` - Full-text search and keyword matching
  - `get_path` - Discover association paths between two concepts
  - `validate` - Knowledge integrity check
  - `stats` - Knowledge network statistical analysis
- **Persistent Storage**: Knowledge triples are persisted to the file system with time-indexed query support.
- **Confidence Score**: Each knowledge entry has a confidence score (0-1), supporting fuzzy matching and ranking of knowledge.
- **Tag Classification**: Supports adding tags to knowledge for categorization and retrieval.

---

## Data Directory Structure

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # Curator's Soul File
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

---

## SpeedyPack Storage Engine

SiliconLife.Fast uses the self-developed SpeedyPack Storage Engine (.spk format), replacing the previous LiteDB approach, achieving extreme read/write performance.

### Architecture Design

```
┌──────────────────────────────────────────────────────────┐
│                    SpeedyPack                             │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────────┐  │
│  │ DirectoryMap  │  │  EntryCache   │  │  WriteQueue   │  │
│  │(In-memory     │  │(Entry cache)  │  │(Async write   │  │
│  │ directory map)│  │               │  │ queue)        │  │
│  └──────┬───────┘  └──────┬───────┘  └───────┬───────┘  │
│         │                  │                   │          │
│  ┌──────▼──────────────────▼───────────────────▼───────┐  │
│  │              PackFileReader / PackFileWriter          │  │
│  │              (Pack file reader/writer)                │  │
│  └──────────────────────────┬──────────────────────────┘  │
│                              │                             │
│  ┌──────────────────────────▼──────────────────────────┐  │
│  │              .spk file (MessagePack + LZ4 compression)│  │
│  └─────────────────────────────────────────────────────┘  │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐                      │
│  │  FreeList     │  │ SpeedyPack   │                      │
│  │(Free space    │  │ AutoCompactor│                      │
│  │ management)   │  │(Auto         │                      │
│  │               │  │ compaction)  │                      │
│  └──────────────┘  └──────────────┘                      │
└──────────────────────────────────────────────────────────┘
```

### Core Components

| Component | Description |
|-----------|-------------|
| `SpeedyPack` | Core class, composes DirectoryMap, EntryCache, and WriteQueue to provide low-latency read/write |
| `DirectoryMap` | In-memory directory mapping, maintains virtual path to file entry mappings |
| `EntryCache` | Entry cache, TTL-based cache for recently accessed entries |
| `WriteQueue` | Async write queue, queues write operations for execution on background threads |
| `FreeList` | Free space management, tracks reusable space in .spk files |
| `PackFileReader` | Pack file reader, reads data from .spk files |
| `PackFileWriter` | Pack file writer, writes data to .spk files |
| `SpeedyPackAutoCompactor` | Auto-compaction timer, periodically compacts .spk files to reclaim free space |
| `SpeedyPackRegistry` | Process-level singleton manager, ensures the entire application uses the same SpeedyPack instance |

### Storage Adapters

SiliconLife.Fast integrates SpeedyPack into system interfaces through the following adapters:

| Adapter | Interface | Description |
|---------|-----------|-------------|
| `SpeedyStorage` | `IStorage` | General key-value storage adapter |
| `SpeedyTimeStorage` | `ITimeStorage` | Time-indexed storage adapter |
| `SpeedyWorkNoteStorage` | `IWorkNoteStorage` | Work note storage adapter |

### Configuration Options

`SpeedyPackOptions` provides the following configuration:

| Option | Type | Default | Description |
|--------|------|---------|-------------|
| `CacheTtl` | `TimeSpan` | 5 minutes | Time-to-live for cache entries |
| `MaxCacheEntries` | `int` | 1000 | Maximum number of cache entries |
| `ReadOnly` | `bool` | false | Read-only mode |

### Transaction Support

SpeedyPack supports atomic write operations through the `IPackTransaction` interface:

- `SpeedyTransaction` implements the transaction mechanism
- Supports atomicity for batch writes
- On transaction commit, all write operations either all succeed or all roll back

---

## Plugin System

SiliconLife supports feature extension through a plugin system, allowing third-party developers to add new functionality to the platform.

### Core Interface

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### Plugin Loader

`PluginLoader` is responsible for loading plugin DLLs from specified directories and performing strict security checks:

1. **Directory Scanning** — Scans all .dll files in the plugin directory
2. **Security Scanning** — Checks whether plugins reference forbidden namespaces
3. **Isolated Loading** — Uses a custom `AssemblyLoadContext` to load plugins in isolation
4. **Lifecycle Management** — Calls the plugin's OnLoad, OnStart, OnStop, OnUnload methods

### Security Sandbox

The plugin loader performs the following security checks:

| Check Item | Description |
|------------|-------------|
| Forbidden Namespaces | System.IO, System.Net.Http, System.Net.WebSockets, System.Net.Sockets, Microsoft.CodeAnalysis |
| Trusted Assembly Whitelist | Google.Protobuf, Newtonsoft.Json, MessagePack, Serilog, Microsoft.Extensions.Logging.Abstractions, Dapper |
| Forbidden Type Check | Scans for dangerous types referenced in plugins |
| Forbidden Member Check | Scans for dangerous methods called in plugins |

### Tool Integration

Plugins can register custom tools by implementing the `ITool` interface:

- `ToolManager.ScanAllPluginAssemblies()` method scans all loaded plugins for ITool implementations
- Plugin tools are automatically integrated into the tool call loop
- Plugin tools are subject to the same permission system constraints

### Plugin Lifecycle

```
Load (OnLoad) → Start (OnStart) → Running → Stop (OnStop) → Unload (OnUnload)
```

---

## Silicon Being Activity States

Silicon Beings have the following activity states:

| State | Description |
|-------|-------------|
| `Idle` | Idle state, waiting for clock trigger |
| `SingleChat` | Engaged in one-on-one chat |
| `GroupChat` | Engaged in group chat |
| `Task` | Executing a task |
| `Timer` | Executing a timer |
| `Stopped` | Stopped due to consecutive errors or manual stop |

**Stopped State Mechanism**:
- When a Silicon Being encounters 10 consecutive errors, it automatically enters the `Stopped` state
- After entering the Stopped state, the being will no longer execute any tasks
- When a new chat message arrives, the error counter is reset and the being resumes operation

State transitions:
```
Idle → SingleChat → Idle (chat completed)
Idle → GroupChat → Idle (group chat completed)
Idle → Task → Idle (task completed)
Idle → Timer → Idle (timer completed)
Any → Stopped (10 consecutive errors)
Stopped → Idle (new chat message arrives or manual restart)
```

---

## Workflow Engine

The workflow engine is a template-based state machine system for driving collaborative processes of Silicon Beings within project spaces:

### Core Components

| Component | Description |
|-----------|-------------|
| `WorkflowEngine` | Workflow engine core, manages templates and instances, executes Tick-driven state transitions |
| `WorkflowTemplate` | Workflow template, defines state sets and transition rules |
| `WorkflowInstance` | Workflow instance, bound to a specific project, tracks current state |
| `WorkflowLog` | Workflow log, records state transition history |

### Working Mechanism

- **Template Registration**: Register workflow templates via `RegisterTemplate()`, defining states and transition rules
- **Instance Creation**: Create instances from templates, bound to project spaces
- **Tick-Driven**: State transitions are driven by the Main Loop's Tick mechanism
- **Log Recording**: All state transitions are automatically logged

---

## Memory Fade Mechanism

`MemoryFadeService` is a timed decay service that simulates the forgetting characteristics of biological memory:

### Working Mechanism

- **Timed Execution**: Inherits from `TickObject`, executes a decay cycle every hour by default
- **Importance Decay**: Applies a decay algorithm to each Silicon Being's memory entries, reducing importance scores
- **Auto-Archiving**: Memories below the importance threshold are automatically archived (`ArchiveFadingMemories()`)
- **Statistics Tracking**: Records decay cycle count, number of state-changed entries, and other statistics

### Decay Process

```
MemoryFadeService.OnTick()
  └── Iterate over all Silicon Beings
       └── being.Memory.ApplyDecay()      # Apply importance decay
       └── being.Memory.ArchiveFadingMemories()  # Archive low-importance memories
```

---

## Project Workspace System

Project workspaces are a spatial management mechanism supporting multi-Silicon Being collaboration:

### Core Features

- **Project Lifecycle**: Create → Active → Archived → Destroy
- **Role Assignment**: Supports assigning project roles to Silicon Beings
- **Tool Permission Isolation**: Project-level tool permission configuration, independent of Silicon Being-level permissions
- **Work Notes**: Page-style note system within project spaces, supporting directory generation and keyword search
- **Task Tracking**: Project-level task management with creation, assignment, and status tracking
- **Workflow Integration**: Projects can be bound to workflow templates to drive collaborative processes

### Related Tools

| Tool | Purpose |
|------|---------|
| `ProjectTool` | Project space management (create, archive, destroy, role assignment) |
| `ProjectTaskTool` | Project task management (create, assign, status update) |
| `ProjectWorkNoteTool` | Project work notes (create, search, directory generation) |
| `ProjectWorkTool` | Project work operations (create tasks, group chat, broadcast, complete project) |
