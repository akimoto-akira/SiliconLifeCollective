# Silicon Life Collective

A .NET 9 multi-agent collaboration platform where AI agents called **Silicon Beings** self-evolve through Roslyn dynamic compilation.

[中文文档](docs/zh-CN/README.md)

## Features

- **Multi-Agent Orchestration** — Managed by a *Silicon Curator* with tick-driven time-sliced fair scheduling
- **Self-Evolution** — Silicon Beings rewrite their own C# classes at runtime via Roslyn, with AES-encrypted persistence and security scanning
- **Executor-Permission Security** — All disk, network, and command-line operations pass through executors with permission verification
- **Zero Database Dependency** — File-based storage, single-file deployable, cross-platform (Windows / Linux / macOS)
- **Built-in Web Server** — HTTP + WebSocket, with a server-side HTML/CSS/JS builder (no frontend framework required)
- **Soul Files** — Each Silicon Being is driven by a core prompt file that defines its personality and behavior

## Tech Stack

| Component | Technology |
|-----------|-----------|
| Runtime | .NET 9 |
| Language | C# |
| AI Integration | Ollama / OpenAI-compatible APIs |
| Dynamic Compilation | Roslyn |
| Web Server | Built-in HTTP + WebSocket |
| Storage | File system (JSON) |
| License | Apache-2.0 |

## Project Structure

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/              # Core library (interfaces, abstractions)
│   │   ├── Core/                      # MainLoop, TickObject, ServiceLocator
│   │   ├── Agents/                    # SiliconBeingBase, SiliconBeingManager
│   │   ├── Tools/                     # ITool interface, ToolManager
│   │   ├── Executors/                 # Executor base, Disk/Network/CLI/Compilation
│   │   ├── Services/                  # IChatService, IStorageService, IAIClient
│   │   ├── IM/                        # IM provider abstraction
│   │   ├── Security/                  # Encryption, permission system
│   │   └── Web/                       # Router, Controller, HtmlBuilder
│   │
│   └── SiliconLife.Default/           # Default implementations + entry point
│       ├── AI/                        # OllamaClient
│       ├── Agents/                    # DefaultSiliconBeing, DefaultCurator
│       ├── Tools/                     # Built-in tools (Disk, Network, Calendar, etc.)
│       ├── Chat/                      # ChatSystem, ConsoleProvider
│       ├── Storage/                   # FileSystemStorage
│       └── Program.cs                 # Application entry
│
└── data/                              # Runtime data (gitignored)
    └── SiliconManager/{GUID}/         # Per-being data directory
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

### Publish (single file)

```bash
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

## Roadmap

- [x] Phase 1: Console AI chat
- [x] Phase 2: Framework skeleton (MainLoop + TickObject)
- [x] Phase 3: First Silicon Being with soul file
- [x] Phase 4: Persistent memory (ChatSystem)
- [x] Phase 5: Tool system + Executors
- [ ] Phase 6: Permission system
- [ ] Phase 7: Dynamic compilation + self-evolution
- [ ] Phase 8: Long-term memory + Task + Timer
- [ ] Phase 9: CoreHost + multi-agent collaboration
- [ ] Phase 10: Web UI
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
