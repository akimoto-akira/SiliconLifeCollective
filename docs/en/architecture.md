# Architecture

> **Version: v0.1.0-alpha**

[English](../en/architecture.md) | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Deutsch](../de-DE/architecture.md) | [Čeština](../cs-CZ/architecture.md)

## Core Concepts

### Silicon Beings

Each AI agent in the system is a **Silicon Being** — an autonomous entity with its own identity, personality, and capabilities. Each silicon being is driven by a **soul file** (a Markdown prompt) that defines its behavior patterns.

### Silicon Curator

The **Silicon Curator** is a special silicon being with the highest system privileges. It acts as the system administrator:

- Creates and manages other silicon beings
- Analyzes user requests and breaks them into tasks
- Distributes tasks to appropriate silicon beings
- Monitors execution quality and handles failures
- Responds to user messages using **priority scheduling** (see below)

### Soul File

A Markdown file (`soul.md`) stored in each silicon being's data directory. It is injected as a system prompt into every AI request, defining the being's personality, decision-making patterns, and behavioral constraints.

---

## Scheduling: Time-Slice Fair Scheduling

### Main Loop + Clock Objects

The system runs a **clock-driven main loop** on a dedicated background thread:

```
Main Loop (dedicated thread, watchdog + circuit breaker)
  └── Clock Object A (priority=0, interval=100ms)
  └── Clock Object B (priority=1, interval=500ms)
  └── Silicon Being Manager (clocked directly by main loop)
        └── Silicon Being Runner → Being 1 → clock trigger → execute one round
        └── Silicon Being Runner → Being 2 → clock trigger → execute one round
        └── Silicon Being Runner → Being 3 → clock trigger → execute one round
        └── ...
```

Key design decisions:

- **Silicon beings do NOT inherit clock objects.** They have their own `Tick()` method, called by `SiliconBeingManager` through `SiliconBeingRunner`, rather than registering directly to the main loop.
- **Silicon Being Manager** is clocked directly by the main loop and acts as a single proxy for all beings.
- **Silicon Being Runner** wraps each being's `Tick()` on a temporary thread, with timeout and per-being circuit breaker (3 consecutive timeouts → 1 minute cooldown).
- Each being's execution is limited to **one round** of AI request + tool calls per clock trigger, ensuring no being can monopolize the main loop.
- **Performance Monitor** tracks clock execution time for observability.

### Curator Priority Response

When a user sends a message to the silicon curator:

1. The current being (e.g., Being A) finishes its current round — **no interruption**.
2. The manager **skips the remaining queue**.
3. The loop **restarts from the curator**, making it execute immediately.

This ensures responsiveness to user interactions while not disrupting in-progress tasks.

---

## Component Architecture

```
┌─────────────────────────────────────────────────────────┐
│                        Core Host                         │
│  (Unified host — assembles and manages all components)    │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ Main Loop │  │ Service      │  │   Configuration   │  │
│  │          │  │ Locator      │  │                   │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │        Silicon Being Manager (Clock Object)       │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Curator  │ │ Being A │ │ Being B │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Shared Services                      │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Chat     │ │ Storage  │ │ Permission        │  │   │
│  │  │ System   │ │          │ │ Manager           │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │ AI       │ │Executor  │ │   Tool            │  │   │
│  │  │ Client   │ │          │ │   Manager         │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Executors                         │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Disk     │ │ Network  │ │ Command Line      │  │   │
│  │  │ Executor │ │ Executor │ │ Executor          │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              IM Providers                          │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Console  │ │ Web      │ │ Feishu / ...      │  │   │
│  │  │ Provider │ │ Provider │ │ Provider          │  │   │
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
| `IMManager` | `IMManager` | IM provider router |
| `AuditLogger` | `AuditLogger` | Permission audit trail |
| `GlobalAcl` | `GlobalACL` | Global access control list |
| `BeingFactory` | `ISiliconBeingFactory` | Factory for creating beings |
| `BeingManager` | `SiliconBeingManager` | Active being lifecycle manager |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Dynamic compilation loader |
| `TokenUsageAudit` | `ITokenUsageAudit` | Token usage tracking |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Token usage reporting |

It also maintains a registry of per-being `PermissionManager` instances, keyed by being GUID.

---

## Chat System

### Session Types

The chat system supports three session types through `SessionBase`:

| Type | Class | Description |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | One-on-one conversation between two participants |
| `GroupChat` | `GroupChatSession` | Multi-participant group chat |
| `Broadcast` | `BroadcastChannel` | Open channel with fixed ID; beings dynamically subscribe and only receive messages after subscription |

### Broadcast Channels

`BroadcastChannel` is a special session type for system-wide announcements:

- **Fixed channel ID** — Unlike `SingleChatSession` and `GroupChatSession`, channel IDs are well-known constants rather than derived from member GUIDs.
- **Dynamic subscription** — Beings subscribe/unsubscribe at runtime; they only receive messages published after subscription.
- **Pending message filtering** — `GetPendingMessages()` returns only messages published after the being's subscription time that haven't been read yet.
- **Managed by chat system** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### Chat Messages

The `ChatMessage` model includes fields for AI conversation context and token tracking:

| Field | Type | Description |
|-------|------|-------------|
| `Id` | `Guid` | Unique message identifier |
| `SenderId` | `Guid` | Sender's unique identifier |
| `ChannelId` | `Guid` | Channel/conversation identifier |
| `Content` | `string` | Message content |
| `Timestamp` | `DateTime` | When the message was sent |
| `Type` | `MessageType` | Text, image, file, or system notification |
| `ReadBy` | `List<Guid>` | Participant IDs who have read this message |
| `Role` | `MessageRole` | AI conversation role (user, assistant, tool) |
| `ToolCallId` | `string?` | Tool call ID for tool result messages |
| `ToolCallsJson` | `string?` | Serialized tool calls JSON for assistant messages |
| `Thinking` | `string?` | AI's chain-of-thought reasoning |
| `PromptTokens` | `int?` | Number of tokens in prompt (input) |
| `CompletionTokens` | `int?` | Number of tokens in completion (output) |
| `TotalTokens` | `int?` | Total tokens used (input + output) |
| `FileMetadata` | `FileMetadata?` | Attached file metadata (if message contains a file) |

### Chat Message Queue

`ChatMessageQueue` is a thread-safe message queue system for managing asynchronous processing of chat messages:

- **Thread-safe** - Uses lock mechanism to ensure safe concurrent access
- **Asynchronous processing** - Supports async message enqueue and dequeue
- **Message ordering** - Maintains temporal order of messages
- **Batch operations** - Supports batch message retrieval

### File Metadata

`FileMetadata` is used to manage file information attached to chat messages:

- **File info** - Filename, size, type, path
- **Upload time** - Timestamp of file upload
- **Uploader** - User or silicon being ID who uploaded the file

### Stream Cancellation Manager

`StreamCancellationManager` provides cancellation mechanism for AI streaming responses:

- **Stream control** - Supports canceling ongoing AI streaming responses
- **Resource cleanup** - Properly cleans up related resources on cancellation
- **Concurrency safe** - Supports managing multiple streams simultaneously

### Chat History Viewing

Newly added chat history viewing functionality allows users to browse silicon beings' historical conversations:

- **Session list** - Displays all historical sessions
- **Message details** - View complete message history
- **Timeline view** - Shows messages in chronological order
- **API support** - Provides RESTful API to retrieve session and message data

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
  - `beijing` — China North 2 (Beijing)
  - `virginia` — US (Virginia)
  - `singapore` — Singapore
  - `hongkong` — China (Hong Kong)
  - `frankfurt` — Germany (Frankfurt)
- **Supported Models** (dynamically discovered via API, with fallback list):
  - **Qwen Series**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **Reasoning**: qwq-plus
  - **Third-party**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **Configuration**: `apiKey`, `region`, `model`
- **Model Discovery**: Fetches available models from Bailian API at runtime; falls back to curated list on network failure

### Client Factory Pattern

Each AI client type has a corresponding factory implementation of `IAIClientFactory`:

- `OllamaClientFactory` — Creates OllamaClient instances
- `DashScopeClientFactory` — Creates DashScopeClient instances

Factories provide:
- `CreateClient(Dictionary<string, object> config)` — Instantiate client from configuration
- `GetConfigKeyOptions(string key, ...)` — Returns dynamic options for config keys (e.g., available models, regions)
- `GetDisplayName()` — Localized display name for client type

### AI Platform Support Matrix

#### Status Legend
- ✅ Implemented
- 🚧 In Development
- 📋 Planned
- 💡 Considering

*Note: Due to the developer's network environment, connecting to [Considering] overseas cloud AI services may require using proxy tools, and the debugging process may be unstable.*

#### Platform List

| Platform | Status | Type | Description |
|----------|--------|------|-------------|
| Ollama | ✅ | Local | Local AI service, supports local model deployment |
| DashScope (Alibaba Cloud Bailian) | ✅ | Cloud | Alibaba Cloud Bailian AI service, supports multi-region deployment |
| Baidu Qianfan (ERNIE) | 📋 | Cloud | Baidu ERNIE AI service |
| Zhipu AI (GLM) | 📋 | Cloud | Zhipu AI service |
| Moonshot AI (Kimi) | 📋 | Cloud | Moonshot Kimi AI service |
| Volcengine Ark (Doubao) | 📋 | Cloud | ByteDance Doubao AI service |
| DeepSeek (Direct) | 📋 | Cloud | DeepSeek AI service |
| 01.AI | 📋 | Cloud | 01.AI service |
| Tencent Hunyuan | 📋 | Cloud | Tencent Hunyuan AI service |
| Silicon Flow | 📋 | Cloud | Silicon Flow AI service |
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
| Perplexity AI | 💡 | Cloud | Perplexity AI search and Q&A service |
| NVIDIA NIM | 💡 | Cloud | NVIDIA AI inference microservices |

---

## Key Design Decisions

### Storage as Instance Class (Not Static)

`IStorage` is designed as an injectable instance, not a static utility. This ensures:

- Direct filesystem access — IStorage is the system's internal persistence channel, **not** routed through executors.
- **AI cannot control IStorage** — Executors manage IO initiated by AI tools; IStorage manages the framework's own internal data read/write. These are fundamentally different concerns.
- Testability with mock implementations.
- Future support for different storage backends without modifying consumers.

### Executors as Security Boundary

Executors are the **only** path for I/O operations. Tools requiring disk, network, or command-line access **must** go through executors. This design enforces:

- **Independent scheduling thread** per executor, with thread locking for permission verification.
- Centralized permission checking — executors query the being's **private permission manager**.
- Request queues supporting priority and timeout control.
- Audit logs for all external operations.
- Exception isolation — one executor's failure doesn't affect others.
- Circuit breakers — consecutive failures temporarily halt the executor to prevent cascade failures.

### ContextManager as Lightweight Object

Each `ExecuteOneRound()` creates a new `ContextManager` instance:

1. Load soul file + recent chat history.
2. Send request to AI client.
3. Loop through tool calls until AI returns pure text.
4. Persist response to chat system.
5. Dispose.

This keeps each round isolated and stateless.

### Self-Evolution Through Class Override

Silicon beings can override their own C# classes at runtime:

1. AI generates new class code (must inherit `SiliconBeingBase`).
2. **Compile-time reference control** (primary defense): Compiler only gets allowed assembly list — `System.IO`, `System.Reflection`, etc. are excluded, making dangerous operations impossible at the type level.
3. **Runtime static analysis** (secondary defense): `SecurityScanner` scans code for dangerous patterns after successful compilation.
4. Roslyn compiles the code in memory.
5. On success: `SiliconBeingManager.ReplaceBeing()` swaps the current instance, migrates state, and persists encrypted code to disk.
6. On failure: Discard new code, keep existing implementation.

Custom `IPermissionCallback` implementations can also be compiled and injected via `ReplacePermissionCallback()`, allowing beings to customize their own permission logic.

Code is stored on disk encrypted with AES-256. Encryption key is derived from the being's GUID (uppercase) via PBKDF2.

---

## Token Usage Audit

`TokenUsageAuditManager` tracks AI token consumption across all beings:

- `TokenUsageRecord` — Per-request record (being ID, model, prompt tokens, completion tokens, timestamp)
- `TokenUsageSummary` — Aggregated statistics
- `TokenUsageQuery` — Query parameters for filtering records
- Persisted via `ITimeStorage` for time-series queries
- Accessible via Web UI (AuditController) and `TokenAuditTool` (curator-only)

---

### Calendar System

The system includes **32 calendar implementations** deriving from the abstract `CalendarBase` class, covering major world calendar systems:

| Calendar | ID | Description |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | Buddhist Era (BE), year + 543 |
| CherokeeCalendar | `cherokee` | Cherokee calendar system |
| ChineseLunarCalendar | `lunar` | Chinese lunar calendar with leap months |
| ChineseHistoricalCalendar | `chinese_historical` | Chinese historical calendar with Ganzhi year system and imperial era names |
| ChulaSakaratCalendar | `chula_sakarat` | Chula Sakarat (CS), year - 638 |
| CopticCalendar | `coptic` | Coptic calendar |
| DaiCalendar | `dai` | Dai calendar with complete lunar calculations |
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
| JucheCalendar | `juche` | Juche calendar (North Korea), year - 1911 |
| JulianCalendar | `julian` | Julian calendar |
| KhmerCalendar | `khmer` | Khmer calendar |
| MayanCalendar | `mayan` | Mayan Long Count calendar |
| MongolianCalendar | `mongolian` | Mongolian calendar |
| PersianCalendar | `persian` | Persian (Solar Hijri) calendar |
| RepublicOfChinaCalendar | `roc` | Republic of China (Minguo) calendar, year - 1911 |
| RomanCalendar | `roman` | Roman calendar |
| SakaCalendar | `saka` | Saka calendar (Indonesia) |
| SexagenaryCalendar | `sexagenary` | Chinese Sexagenary (Ganzhi) calendar |
| TibetanCalendar | `tibetan` | Tibetan calendar |
| VietnameseCalendar | `vietnamese` | Vietnamese lunar calendar (Cat zodiac variant) |
| VikramSamvatCalendar | `vikram_samvat` | Vikram Samvat calendar |
| YiCalendar | `yi` | Yi calendar system |
| ZoroastrianCalendar | `zoroastrian` | Zoroastrian calendar |

`CalendarTool` provides operations: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (cross-calendar date conversion).

---

## Web UI Architecture

### Skin System

The Web UI features a **pluggable skin system** allowing complete UI customization without changing application logic:

- **ISkin Interface** — Defines contract for all skins, including:
  - Core rendering methods (`RenderHtml`, `RenderError`)
  - 20+ UI component methods (buttons, inputs, cards, tables, badges, bubbles, progress, tabs, etc.)
  - Theme CSS generation via `CssBuilder`
  - `SkinPreviewInfo` — Color palette and icons for initialization page skin selector

- **Built-in Skins** — 4 production-ready skins:
  - **Admin** — Professional, data-focused system administration interface
  - **Chat** — Conversational, message-centric design for AI interaction
  - **Creative** — Artistic, visually rich layout for creative workflows
  - **Dev** — Developer-centric, code-focused interface with syntax highlighting

- **Skin Discovery** — `SkinManager` automatically discovers and registers all `ISkin` implementations via reflection

### HTML / CSS / JS Builders

The Web UI completely avoids template files, generating all markup in C#:

- **`H`** — Streaming HTML builder DSL for constructing HTML trees in code
- **`CssBuilder`** — CSS builder supporting selectors and media queries
- **`JsBuilder` (`JsSyntax`)** — JavaScript builder for inline scripts

### Controller System

The Web UI follows a **MVC-like pattern** with 20+ controllers handling different aspects:

| Controller | Purpose |
|------------|---------|
| About | About page and project information |
| Audit | Token usage audit dashboard with trend charts and export |
| Being | Silicon being management and status |
| Chat | Real-time chat interface with SSE |
| ChatHistory | Chat history viewer with session list and message details |
| CodeBrowser | Code viewer and editor |
| CodeHover | Code hover hints with syntax highlighting |
| Config | System configuration management |
| Dashboard | System overview and metrics |
| Executor | Executor status and management |
| Help | Help documentation system with multi-language support |
| Init | First-run initialization wizard |
| Knowledge | Knowledge graph visualization and query |
| Log | System log viewer with silicon being filtering |
| Memory | Long-term memory browser with advanced filtering, statistics, and detail views |
| Permission | Permission management |
| PermissionRequest | Permission request queue |
| Project | Project management with work notes and task system |
| Task | Task system interface |
| Timer | Timer system management with execution history |
| WorkNote | Work note management with search and directory generation |

### Real-time Updates

- **SSE (Server-Sent Events)** — Pushes updates for chat messages, being status, and system events via `SSEHandler`
- **No WebSocket** — Simpler architecture using SSE for most real-time needs
- **Auto-reconnection** — Client reconnection logic for resilient connections

### Localization

The system supports comprehensive localization across **21 language variants**:
- **Chinese (6)**: zh-CN (Simplified), zh-HK (Traditional), zh-SG (Singapore), zh-MO (Macau), zh-TW (Taiwan), zhMY (Malaysia)
- **English (10)**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **Spanish (2)**: es-ES, es-MX
- **Others (3)**: ja-JP (Japanese), ko-KR (Korean), cs-CZ (Czech)

Active locale is selected via `DefaultConfigData.Language` and resolved through `LocalizationManager`.

---

### WebView Browser Automation System (New)

The system integrates **Playwright**-based WebView browser automation:

- **Individual Isolation**: Each silicon being has its own browser instance, cookies, and session storage, completely isolated from each other.
- **Headless Mode**: Browser runs in completely invisible headless mode, with silicon beings operating autonomously in the background.
- **WebViewBrowserTool**: Provides complete browser operation capabilities including:
  - Page navigation, clicking, text input, getting page content
  - Executing JavaScript, taking screenshots, waiting for elements
  - Browser state management and resource cleanup
- **Security Control**: All browser operations must pass through the permission verification chain to prevent malicious web page access.

### Knowledge Network System (New)

The system includes a built-in knowledge graph system based on **triple structure**:

- **Knowledge Representation**: Uses "subject-predicate-object" triple structure (e.g., Python-is_a-programming_language)
- **KnowledgeTool**: Provides full lifecycle management of knowledge:
  - `add`/`query`/`update`/`delete` - Basic CRUD operations
  - `search` - Full-text search and keyword matching
  - `get_path` - Discover association paths between two concepts
  - `validate` - Knowledge integrity checking
  - `stats` - Knowledge network statistical analysis
- **Persistent Storage**: Knowledge triples are persisted to filesystem, supporting time-indexed queries.
- **Confidence Scoring**: Each knowledge entry has a confidence score (0-1), supporting fuzzy matching and ranking of knowledge.
- **Tag Classification**: Supports adding tags to knowledge for categorization and retrieval.

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
