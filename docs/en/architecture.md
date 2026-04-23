# Architecture

[English](architecture.md) | [中文](docs/zh-CN/architecture.md) | [繁體中文](docs/zh-HK/architecture.md) | [日本語](docs/ja-JP/architecture.md) | [한국어](docs/ko-KR/architecture.md)

## Core Concepts

### Silicon Being

Each AI agent in the system is a **Silicon Being** — an autonomous entity with its own identity, personality, and capabilities. Every Silicon Being is driven by a **Soul File** (Markdown prompt) that defines its behavior patterns.

### Silicon Curator

The **Silicon Curator** is a special Silicon Being with the highest system privileges. It acts as the system administrator:

- Creates and manages other Silicon Beings
- Analyzes user requests and decomposes them into tasks
- Distributes tasks to appropriate Silicon Beings
- Monitors execution quality and handles failures
- Responds to user messages with **priority scheduling** (see below)

### Soul File

A Markdown file (`soul.md`) stored in each Silicon Being's data directory. It serves as the system prompt injected into every AI request, defining the being's personality, decision-making patterns, and behavioral constraints.

---

## Scheduling: Time-Sliced Fair Scheduling

### MainLoop + TickObject

The system runs a **tick-driven main loop** on a dedicated background thread:

```
MainLoop (dedicated thread, watchdog + circuit breaker)
  └── TickObject A (Priority=0, Interval=100ms)
  └── TickObject B (Priority=1, Interval=500ms)
  └── SiliconBeingManager (ticked directly by MainLoop)
        └── SiliconBeingRunner → Silicon Being 1 → Tick → ExecuteOneRound
        └── SiliconBeingRunner → Silicon Being 2 → Tick → ExecuteOneRound
        └── SiliconBeingRunner → Silicon Being 3 → Tick → ExecuteOneRound
        └── ...
```

Key design decisions:

- **Silicon Beings do NOT inherit TickObject.** They have their own `Tick()` method, invoked by `SiliconBeingManager` via a `SiliconBeingRunner`, not registered directly to the MainLoop.
- **SiliconBeingManager** is ticked directly by MainLoop and acts as the single proxy for all beings.
- **SiliconBeingRunner** wraps each being's `Tick()` on a temporary thread with timeout and per-being circuit breaker (3 consecutive timeouts → 1-minute cooldown).
- Each being's execution is limited to **one round** of AI request + ToolCalls per tick, ensuring no being can monopolize the main loop.
- **PerformanceMonitor** tracks tick execution times for observability.

### Curator Priority Response

When the user sends a message to the Silicon Curator:

1. The current being (e.g., Being A) finishes its current round — **no interruption**.
2. The manager **skips the remaining queue**.
3. The loop **restarts from the Curator**, giving it immediate execution.

This ensures responsive user interaction without disrupting in-progress tasks.

---

## Component Architecture

```
┌─────────────────────────────────────────────────────────┐
│                        CoreHost                         │
│  (Unified host — assembles and manages all components)  │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ MainLoop │  │ ServiceLocator│  │      Config      │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │           SiliconBeingManager (TickObject)        │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Curator  │ │Being A  │ │Being B  │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Shared Services                      │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ChatSystem│  │ Storage  │  │  PermissionMgr  │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │ IAIClient│  │Executor  │  │   ToolManager   │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Executors                       │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │  Disk    │  │ Network  │  │  CommandLine    │  │   │
│  │  │Executor  │  │Executor  │  │  Executor       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              IM Providers                        │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Console  │  │  Web     │  │  Feishu / ...   │  │   │
│  │  │Provider  │  │Provider  │  │  Provider       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## ServiceLocator

`ServiceLocator` is a thread-safe singleton registry that provides access to all core services:

| Property | Type | Description |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | Central chat session manager |
| `IMManager` | `IMManager` | Instant messaging provider router |
| `AuditLogger` | `AuditLogger` | Permission audit trail |
| `GlobalAcl` | `GlobalACL` | Global access control list |
| `BeingFactory` | `ISiliconBeingFactory` | Factory for creating beings |
| `BeingManager` | `SiliconBeingManager` | Active being lifecycle manager |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Dynamic compilation loader |
| `TokenUsageAudit` | `ITokenUsageAudit` | Token usage tracking |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Token usage reporting |

It also maintains a per-being `PermissionManager` registry, keyed by being GUID.

---

## Chat System

### Session Types

The chat system supports three session types via `SessionBase`:

| Type | Class | Description |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | 1-on-1 conversation between two participants |
| `GroupChat` | `GroupChatSession` | Multi-participant group conversation |
| `Broadcast` | `BroadcastChannel` | Open channel with fixed ID; beings subscribe dynamically and only receive messages after subscription time |

### BroadcastChannel

`BroadcastChannel` is a special session type for system-wide announcements:

- **Fixed channel ID** — Unlike `SingleChatSession` and `GroupChatSession`, the channel ID is a well-known constant, not derived from member GUIDs.
- **Dynamic subscription** — Beings subscribe/unsubscribe at runtime; they only receive messages posted after their subscription time.
- **Pending message filtering** — `GetPendingMessages()` returns only messages posted after the being's subscription time that they haven't read yet.
- **Managed by ChatSystem** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### ChatMessage

The `ChatMessage` model includes fields for AI conversation context and token tracking:

| Field | Type | Description |
|-------|------|-------------|
| `Id` | `Guid` | Unique message identifier |
| `SenderId` | `Guid` | Sender's unique identifier |
| `ChannelId` | `Guid` | Channel/conversation identifier |
| `Content` | `string` | Message content |
| `Timestamp` | `DateTime` | When the message was sent |
| `Type` | `MessageType` | Text, Image, File, or SystemNotification |
| `ReadBy` | `List<Guid>` | IDs of participants who have read this message |
| `Role` | `MessageRole` | AI conversation role (User, Assistant, Tool) |
| `ToolCallId` | `string?` | Tool call ID for tool result messages |
| `ToolCallsJson` | `string?` | Serialized tool calls JSON for assistant messages |
| `Thinking` | `string?` | Chain-of-thought reasoning from the AI |
| `PromptTokens` | `int?` | Number of tokens in the prompt (input) |
| `CompletionTokens` | `int?` | Number of tokens in the completion (output) |
| `TotalTokens` | `int?` | Total tokens used (input + output) |
| `FileMetadata` | `FileMetadata?` | Attached file metadata (if message contains files) |

### Chat Message Queue

`ChatMessageQueue` is a thread-safe message queue system for managing asynchronous processing of chat messages:

- **Thread-safe** - Uses lock mechanism to ensure safe concurrent access
- **Asynchronous processing** - Supports async message enqueue and dequeue
- **Message ordering** - Maintains chronological order of messages
- **Batch operations** - Supports batch message retrieval

### File Metadata

`FileMetadata` is used to manage file information attached to chat messages:

- **File information** - File name, size, type, path
- **Upload time** - Timestamp of file upload
- **Uploader** - ID of user or silicon being who uploaded the file

### Stream Cancellation Manager

`StreamCancellationManager` provides cancellation mechanism for AI streaming responses:

- **Stream control** - Supports cancelling ongoing AI streaming responses
- **Resource cleanup** - Properly cleans up related resources on cancellation
- **Concurrency safe** - Supports managing multiple streams simultaneously

### Chat History View

The new chat history view feature allows users to browse historical conversations of silicon beings:

- **Conversation list** - Displays all historical conversations
- **Message details** - View complete message history
- **Timeline view** - Shows messages in chronological order
- **API support** - Provides RESTful API to retrieve conversation and message data

---

## AI Client System

The system supports multiple AI backends through the `IAIClient` interface:

### OllamaClient

- **Type**: Local AI service
- **Protocol**: Native Ollama HTTP API (`/api/chat`, `/api/generate`)
- **Features**: Streaming, tool calling, local model hosting
- **Configuration**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (Alibaba Cloud Bailian)

- **Type**: Cloud AI service
- **Protocol**: OpenAI-compatible API (`/compatible-mode/v1/chat/completions`)
- **Authentication**: Bearer token (API key)
- **Features**: Streaming, tool calling, reasoning content (Chain-of-Thought), multi-region deployment
- **Supported Regions**:
  - `beijing` — 华北2（北京）
  - `virginia` — 美国（弗吉尼亚）
  - `singapore` — 新加坡
  - `hongkong` — 中国香港
  - `frankfurt` — 德国（法兰克福）
- **Supported Models** (dynamic discovery via API, with fallback list):
  - **Qwen Series**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **Reasoning**: qwq-plus
  - **Third-party**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **Configuration**: `apiKey`, `region`, `model`
- **Model Discovery**: Fetches available models from DashScope API at runtime; falls back to curated list on network failure

### Client Factory Pattern

Each AI client type has a corresponding factory implementing `IAIClientFactory`:

- `OllamaClientFactory` — Creates OllamaClient instances
- `DashScopeClientFactory` — Creates DashScopeClient instances

Factories provide:
- `CreateClient(Dictionary<string, object> config)` — Instantiates client from configuration
- `GetConfigKeyOptions(string key, ...)` — Returns dynamic options for configuration keys (e.g., available models, regions)
- `GetDisplayName()` — Localized display name for the client type

### AI Platform Support List

#### Status Indicators
- ✅ Implemented
- 🚧 In Development
- 📋 Planned
- 💡 Under Consideration

*Note: Due to the developer's network environment, integrating [Under Consideration] overseas cloud AI services may require network proxy tools for access, and the debugging process may be unstable.*

#### Platform List

| Platform | Status | Type | Description |
|----------|--------|------|-------------|
| Ollama | ✅ | Local | Local AI service, supports local model deployment |
| DashScope (Alibaba Cloud Bailian) | ✅ | Cloud | Alibaba Cloud Bailian AI service, supports multi-region deployment |
| Baidu Qianfan (ERNIE Bot) | 📋 | Cloud | Baidu ERNIE Bot AI service |
| Zhipu AI (GLM) | 📋 | Cloud | Zhipu Qingyan AI service |
| Moonshot AI (Kimi) | 📋 | Cloud | Moonshot Kimi AI service |
| Doubao (Volcano Engine) | 📋 | Cloud | ByteDance Doubao AI service |
| DeepSeek (Direct Connection) | 📋 | Cloud | Deepseek AI service |
| 01.AI | 📋 | Cloud | 01.AI AI service |
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

### Storage as Instance Class (not static)

`IStorage` is designed as an injectable instance, not a static utility. This ensures:

- Direct file system access — IStorage is the system's internal persistence channel and does **not** route through executors.
- **AI cannot control IStorage** — executors govern IO initiated by AI tools; IStorage governs the framework's own internal data reads/writes. These are fundamentally different concerns.
- Testable with mock implementations.
- Future support for different storage backends without modifying consumers.

### Executor as Security Boundary

Executors are the **only** path for I/O operations. Tools that need disk, network, or command-line access **must** go through executors. This design enforces:

- Each executor owns an **independent scheduling thread** with thread locking for permission verification.
- Centralized permission checking — the executor queries the being's **private PermissionManager**.
- Request queuing with priority support and timeout control.
- Audit logging for all external operations.
- Exception isolation — one executor's failure does not affect others.
- Circuit breaker — consecutive failures temporarily halt an executor to prevent cascading failures.

### ContextManager as Lightweight Object

Each `ExecuteOneRound()` creates a new `ContextManager` instance that:

1. Loads the Soul File + recent chat history.
2. Sends the request to the AI client.
3. Loops through ToolCalls until the AI returns plain text.
4. Persists the response to ChatSystem.
5. Disposes.

This keeps each round isolated and stateless.

### Self-Evolution via Class Rewriting

Silicon Beings can rewrite their own C# classes at runtime:

1. AI generates new class code (must inherit `SiliconBeingBase`).
2. **Compile-time reference control** (primary defense): the compiler is only given an allowed assembly list — `System.IO`, `System.Reflection`, etc. are excluded, so dangerous code is impossible at the type level.
3. **Runtime static analysis** (secondary defense): `SecurityScanner` scans the code for dangerous patterns even after successful compilation.
4. Roslyn compiles the code in memory.
5. On success: `SiliconBeingManager.ReplaceBeing()` swaps the current instance, state is migrated, and encrypted code is persisted to disk.
6. On failure: discard new code, keep the existing implementation.

Custom `IPermissionCallback` implementations can also be compiled and injected via `ReplacePermissionCallback()`, allowing beings to customize their own permission logic.

Code is stored AES-256 encrypted on disk. The encryption key is derived from the being's GUID (uppercase) via PBKDF2.

---

## Token Usage Audit

The `TokenUsageAuditManager` tracks AI token consumption across all beings:

- `TokenUsageRecord` — per-request record (being ID, model, prompt tokens, completion tokens, timestamp)
- `TokenUsageSummary` — aggregated statistics
- `TokenUsageQuery` — query parameters for filtering records
- Persisted via `ITimeStorage` for time-series queries
- Accessible via Web UI (AuditController) and `TokenAuditTool` (curator-only)

---

## Calendar System

The system includes **32 calendar implementations** derived from the abstract `CalendarBase` class, covering major world calendar systems:

| Calendar | ID | Description |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | Buddhist Era (BE), year + 543 |
| CherokeeCalendar | `cherokee` | Cherokee calendar system |
| ChineseLunarCalendar | `lunar` | Chinese lunar calendar with leap months |
| ChulaSakaratCalendar | `chula_sakarat` | Chula Sakarat (CS), year - 638 |
| CopticCalendar | `coptic` | Coptic calendar |
| DaiCalendar | `dai` | Dai calendar with full lunar computation |
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
| SakaCalendar | `saka` | Saka calendar (Indonesian) |
| SexagenaryCalendar | `sexagenary` | Chinese Sexagenary Cycle (干支 Ganzhi) |
| TibetanCalendar | `tibetan` | Tibetan calendar |
| VietnameseCalendar | `vietnamese` | Vietnamese lunar calendar (Cat zodiac variant) |
| VikramSamvatCalendar | `vikram_samvat` | Vikram Samvat calendar |
| YiCalendar | `yi` | Yi calendar system |
| ZoroastrianCalendar | `zoroastrian` | Zoroastrian calendar |

The `CalendarTool` provides actions: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (cross-calendar date conversion).

---

## Web UI Architecture

### Skin System

The Web UI features a **pluggable skin system** that allows complete UI customization without changing application logic:

- **ISkin Interface** — Defines the contract for all skins, including:
  - Core rendering methods (`RenderHtml`, `RenderError`)
  - 20+ UI component methods (buttons, inputs, cards, tables, badges, bubbles, progress, tabs, etc.)
  - Theme CSS generation via `CssBuilder`
  - `SkinPreviewInfo` — color palette and icon for the init page skin selector

- **Built-in Skins** — 4 production-ready skins:
  - **Admin** — Professional, data-focused interface for system administration
  - **Chat** — Conversational, message-centric design for AI interactions
  - **Creative** — Artistic, visually rich layout for creative workflows
  - **Dev** — Developer-focused, code-centric interface with syntax highlighting

- **Skin Discovery** — `SkinManager` auto-discovers and registers all `ISkin` implementations via reflection

### HTML / CSS / JS Builders

The Web UI avoids template files entirely, generating all markup in C#:

- **`H`** — Fluent HTML builder DSL for constructing HTML trees in code
- **`CssBuilder`** — CSS builder with selector and media query support
- **`JsBuilder` (`JsSyntax`)** — JavaScript builder for inline scripts

### Controller System

The Web UI follows a **MVC-like pattern** with 17 controllers handling different aspects:

| Controller | Purpose |
|------------|---------|
| About | About page and project information |
| Audit | Token usage audit dashboard with trend charts and export |
| Being | Silicon Being management and status |
| Chat | Real-time chat interface with SSE |
| CodeBrowser | Code viewing and editing |
| Config | System configuration management |
| Dashboard | System overview and metrics |
| Executor | Executor status and management |
| Init | First-run initialization wizard |
| Knowledge | Knowledge graph visualization (placeholder) |
| Log | System log viewer |
| Memory | Long-term memory browser |
| Permission | Permission management |
| PermissionRequest | Permission request queue |
| Project | Project management (placeholder) |
| Task | Task system interface |
| Timer | Timer system management |

### Real-time Updates

- **SSE (Server-Sent Events)** — Push-based updates for chat messages, being status, and system events via `SSEHandler`
- **No WebSocket Required** — Simpler architecture using SSE for most real-time needs
- **Automatic Reconnection** — Client-side reconnection logic for resilient connections

### Localization

Three locales are built in: `ZhCN` (Simplified Chinese), `ZhHK` (Traditional Chinese), and `EnUS` (English). The active locale is selected via `DefaultConfigData.Language` and resolved through `LocalizationManager`.

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
