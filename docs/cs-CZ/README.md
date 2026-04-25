# Silicon Life Collective

**⚠️ VAROVÁNÍ: Dynamická kompilace funguje, ale pro správný běh vyžaduje kódové šablony. Probíhá komplexní testování.**

Multi-agentní platforma pro spolupráci založená na .NET 9, kde AI agenti jsou nazýváni **Křemíkové Bytosti** a umožňují sebe-evoluci prostřednictvím dynamické kompilace Roslyn.

[English](../en/README.md) | [中文文档](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Čeština](../cs-CZ/README.md)

## Funkce

- **Multi-agentní orchestrace** — Řízeno *Křemíkovým Kurátorem* s clock-driven time-slice fair scheduling (hlavní smyčka + objekt Clock + watchdog + jistič)
- **Řízeno souborem duše** — Každá křemíková bytost je řízena souborem s výzvou (`soul.md`), který definuje její osobnost a chování
- **Architektura Tělo-Mozek** — *Tělo* (SiliconBeing) udržuje stav naživu a detekuje spouštěcí scénáře; *Mozek* (ContextManager) načítá historii, volá AI, executes nástroje a persistuje odpovědi
- **Cyklus volání nástrojů** — AI vrací volání nástrojů → execute nástroje → vrátit výsledky AI → AI pokračuje → dokud nevrátí čistý text
- **Executor-Permission Security** — Všechny operace disku, sítě a příkazového řádku jsou autorizovány přes executory
  - 5-úrovňový řetězec oprávnění: IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - Všechna rozhodnutí o oprávněních mají auditní záznam
- **Audit použití Tokenů** — Vestavěné sledování a reportování použití tokenů prostřednictvím `ITokenUsageAudit` / `TokenUsageAuditManager`
- **Více AI backendů** — Podpora Ollama (lokální) a Alibaba Cloud Bailian (cloud)
  - **Ollama** — Lokální hostování modelů s nativním HTTP API
  - **Bailian (DashScope)** — Cloudová AI služba, kompatibilní s OpenAI API, více oblastí nasazení, podpora 13+ modelů (Tongyi Qianwen, DeepSeek, GLM, Kimi, Llama)
- **32 kalendářních systémů** — Podpora více kalendářů včetně gregoriánského, lunárního, islámského, hebrejského, japonského, perského, mayského atd.
- **Minimální závislosti** — Core knihovna závisí pouze na Microsoft.CodeAnalysis.CSharp pro dynamickou kompilaci Roslyn
- **Žádná databázová závislost** — Úložiště založené na souborech (JSON) s podporou časově indexovaných dotazů prostřednictvím `ITimeStorage`
- **Lokalizace** — Komplexní vícejazyčná podpora s 21 jazykovými variantami
  - Čínština: zh-CN, zh-HK, zh-SG, zh-MO, zh-TW, zhMY (6 variant)
  - Angličtina: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 variant)
  - Španělština: es-ES, es-MX (2 varianty)
  - Japonština: ja-JP
  - Korejština: ko-KR
  - Čeština: cs-CZ
- **Web UI** — Vestavěný HTTP server s podporou SSE, více skinů a komplexní dashboard
  - **Systém skinů** — 4 vestavěné skiny (Admin, Chat, Creative, Dev) s rozhraním ISkin a automatickým objevováním
  - **20+ kontrolerů** — About, Audit, Being, Chat, ChatHistory, CodeBrowser, CodeHover, Config, Dashboard, Executor, Init, Knowledge, Log, Memory, Permission, PermissionRequest, Project, Task, Timer, TimerExecutionHistory
  - **Aktualizace v reálném čase** — SSE (Server-Sent Events) pro chatové zprávy, stav bytostí a systémové události
  - **HTML/CSS/JS Buildery** — Generování značek na straně serveru prostřednictvím `H`, `CssBuilder` a `JsBuilder` (žádná závislost na frontendovém frameworku)
  - **Lokalizace** — 21 vestavěných jazykových variant s parsowaniem přes LocalizationManager
  - **Zobrazení historie chatu** — Kompletní funkce procházení historie chatu křemíkových bytostí s výpisem relací a detaily zpráv
  - **Podpora nahrávání souborů** — Dialogové okno zdroje souboru a funkce nahrávání souborů
  - **Indikátor načítání** — Indikátor stavu načítání na stránce chatu a automatický výběr relace kurátora

## Technologie

| Komponenta | Technologie |
|-----------|-----------|
| Runtime | .NET 9 |
| Jazyk | C# |
| AI Integrace | Ollama (lokální), Alibaba Cloud Bailian (cloud) |
| Úložiště | Souborový systém (JSON + adresáře s časovým indexem) |
| Webový server | HttpListener (vestavěný v .NET) |
| Dynamická kompilace | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Licence | Apache-2.0 |

## Struktura projektu

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Core knihovna (rozhraní, abstraktní třídy)
│   │   ├── ServiceLocator.cs             # Globální service locator: Register/Get, ChatSystem, IMManager, AuditLogger, GlobalACL, BeingFactory, BeingManager, DynamicBeingLoader, TokenUsageAudit
│   │   ├── Runtime/                       # Hlavní smyčka, objekty Clock, CoreHost, CoreHostBuilder, PerformanceMonitor
│   │   ├── SiliconBeing/                  # Základní třída SiliconBeing, SiliconBeingManager, SiliconCurator, rozhraní ISiliconBeingFactory, SoulFileManager, Paměť, Systém úkolů, Systém časovačů
│   │   ├── AI/                            # Rozhraní AI klienta, rozhraní AI Client Factory, ContextManager ("mozek"), Zprávy, AI Request/AI Response
│   │   ├── Audit/                         # Rozhraní auditu použití tokenů, Token Usage Audit Manager, Token Usage Record, Token Usage Summary, Token Usage Query
│   │   ├── Chat/                          # Chatový systém, rozhraní Chat Service, Simple Chat Service, Základ relace, Single Chat Session, Group Chat Session, Broadcast Channel, Chat Message
│   │   ├── Executors/                     # Základní třída Executor, Disk Executor, Network Executor, Command Line Executor, Executor Request, Executor Result
│   │   ├── Tools/                         # Rozhraní nástroje, Tool Manager (reflexní skenování), Tool Call/Tool Result, definice nástroje, atribut pouze pro Silicon Manager
│   │   ├── Security/                      # Permission Manager, Global Access Control List, Audit Logger, User Frequency Cache, Permission Result, Permission Type, rozhraní Permission Callback, rozhraní Permission Ask Handler
│   │   ├── IM/                            # Rozhraní IM providera, IM Manager (směrování zpráv)
│   │   ├── Storage/                       # Rozhraní úložiště, rozhraní Time Storage (key-value + časový index)
│   │   ├── Config/                        # Základní třída Config Data, Config (singleton + JSON), převaděč základní třídy Config Data, převaděč Guid, atribut konfigurace AI klienta, atribut skupiny konfigurace, atribut ignorování konfigurace, převaděč informací o adresáři
│   │   ├── Localization/                  # Základní třída lokalizace, Localization Manager, enumerace jazyka
│   │   ├── Logging/                       # Rozhraní loggeru, rozhraní Logging providera, položka logu, úroveň logu, Logging Manager
│   │   ├── Compilation/                   # Dynamic Being Loader, Dynamic Compilation Executor, Security Scanner, šifrování kódu
│   │   └── Time/                          # Neúplné datum (dotaz rozsahu času)
│   │
│   └── SiliconLife.Default/               # Výchozí implementace + vstupní bod
│       ├── Program.cs                     # Vstupní bod aplikace (sestavení všech komponent)
│       ├── AI/                            # Ollama klient, Ollama Client Factory (nativní Ollama HTTP API); DashScope klient, DashScope Client Factory (Alibaba Cloud Bailian)
│       ├── SiliconBeing/                  # Výchozí křemíková bytost, výchozí továrna křemíkových bytostí
│       ├── Calendar/                      # 32 kalendářních implementací: Buddhist, Cherokee, Chinese Historical, Chinese Lunar, Chula Sakarat, Coptic, Dai, Dehong Dai, Ethiopian, French Republican, Gregorian, Hebrew, Indian, Inuit, Islamic, Japanese, Javanese, Juche, Julian, Khmer, Mayan, Mongolian, Persian, Republic of China, Roman, Saka, Sexagenary, Tibetan, Vietnamese, Vikram Samvat, Yi, Zoroastrian
│       ├── Executors/                     # Výchozí implementace executorů
│       ├── IM/                            # WebUI provider (Web UI jako IM kanál), IM Permission Ask Handler
│       ├── Tools/                         # Vestavěné nástroje: Calendar, Chat, Config, Curator, Disk, Dynamic Compilation, Memory, Network, System, Task, Timer, Token Audit
│       ├── Config/                        # Výchozí konfigurační data
│       ├── Localization/                  # Zjednodušená čínština, Tradiční čínština, Americká angličtina, Japonština, Korejština, Španělština, Čeština, výchozí základ lokalizace, další angličtina (britská, kanadská, australská, indická, singapurská, jihoafrická, irská, novozélandská, malajsijská), další čínština (singapurská, macajská, tchajwanská, malajsijská), další španělština (mexická)
│       ├── Logging/                       # Console Logging provider, FileSystem Logging provider
│       ├── Storage/                       # FileSystem Storage, FileSystem Time Storage
│       ├── Security/                      # Výchozí Permission Callback
│       ├── Runtime/                       # Testovací objekt Clock
│       └── Web/                           # Web UI implementace
│           ├── Controllers/               # 18 kontrolerů: About, Audit, Being, Chat, CodeBrowser, CodeHover, Config, Dashboard, Executor, Init, Knowledge, Log, Memory, Permission, PermissionRequest, Project, Task, Timer
│           ├── Models/                    # ViewModely: About, Audit, Being, Chat Message, Chat, CodeBrowser, Config, Dashboard, Executor, Knowledge, Log, Memory, Permission, PermissionRequest, Project, Task, Timer, ViewModel Base
│           ├── Views/                     # 19 HTML pohledů: View Base, About, Audit, Being, Chat, CodeBrowser, CodeEditor, Config, Dashboard, Executor, Knowledge, Log, MarkdownEditor, Memory, Permission, Project, SoulEditor, Task, Timer
│           ├── Skins/                     # 4 skiny: Admin (profesionální), Chat (konverzační), Creative (umělecký), Dev (orientovaný na vývojáře)
│           ├── ISkin.cs                   # Rozhraní skinu + informace o náhledu skinu + Skin Manager (automatické objevování)
│           ├── Controller.cs              # Základní třída kontroleru
│           ├── WebHost.cs                 # HTTP server (HttpListener)
│           ├── Router.cs                  # Směrování požadavků s podporou vzorů
│           ├── SSEHandler.cs              # Server-Sent Events
│           ├── WebSecurity.cs             # Nástroje webové bezpečnosti
│           ├── H.cs                       # Streamovaný HTML Builder DSL
│           ├── CssBuilder.cs              # Nástroj CSS Builderu
│           └── JsBuilder.cs               # Nástroj JavaScript Builderu
│
├── docs/
│   └── cs-CZ/                             # Česká dokumentace
```

## Přehled architektury

```
Hlavní smyčka (vyhrazený thread, watchdog + jistič)
  └── Objekty Clock (tříděno podle priority)
       └── SiliconBeingManager
            └── SiliconBeingRunner (dočasný thread na tick, timeout + jistič)
                 └── DefaultSiliconBeing.Tick()
                      └── ContextManager.ThinkChat()
                           └── AI Klient.Chat() -> cyklus volání nástrojů -> persist do ChatSystemu
```

Všechny I/O operace iniciované AI musí procházet bezpečnostním řetězcem:

```
Volání nástroje -> Executor -> Permission Manager -> [IsCurator -> Frequence Cache -> GlobalACL -> Callback -> Dotaz uživatele]
```

## Rychlý start

### Požadavky

- .NET 9 SDK
- AI backend (vyberte jeden):
  - **Ollama**: [Ollama](https://ollama.com) běžící lokálně s nataženými modely (např. `ollama pull llama3`)
  - **Alibaba Cloud Bailian**: Získejte platný API klíč z [Bailian Console](https://bailian.console.aliyun.com/)

### Build

```bash
dotnet restore
dotnet build
```

### Spuštění

```bash
dotnet run --project src/SiliconLife.Default
```

Aplikace spustí webový server a automaticky otevře Web UI v prohlížeči.

### Publikování (jeden soubor)

```bash
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

## Roadmapa

- [x] Fáze 1: Konzolový AI Chat
- [x] Fáze 2: Kostra frameworku (hlavní smyčka + objekty Clock + watchdog + jistič)
- [x] Fáze 3: První křemíková bytost se souborem duše (architektura Tělo-Mozek)
- [x] Fáze 4: Persistující paměť (Chat System + rozhraní Time Storage)
- [x] Fáze 5: Systém nástrojů + Executory
- [x] Fáze 6: Systém oprávnění (5-úrovňový řetězec, Audit Logger, GlobalACL)
- [x] Fáze 7: Dynamická kompilace + sebe-evoluce (Roslyn)
- [x] Fáze 8: Dlouhodobá paměť + Úkoly + Časovače
- [x] Fáze 9: Core Host + multi-agentní spolupráce
- [x] Fáze 10: Web UI (HTTP + SSE, 18 kontrolerů, 4 skiny)
- [x] Fáze 10.5: Přírůstková vylepšení (BroadcastChannel, TokenAudit, 32 kalendářů, vylepšení nástrojů, lokalizace 21 jazyků)
- [ ] Fáze 11: Externí IM integrace (Feishu / WhatsApp / Telegram)
- [ ] Fáze 12: Knowledge Graph, Plugin System a Skill Ecosystem

## Dokumentace

- [Architektura](architecture.md) — Návrh systému, scheduling, architektura komponent
- [Bezpečnost](security.md) — Model oprávnění, Executory, bezpečnost dynamické kompilace
- [Roadmapa](roadmap.md) — Detailní 12-fázový vývojový plán

## Licence

Tento projekt je licencován pod licencí Apache License 2.0 — viz soubor [LICENSE](LICENSE).

## Autor

Hoshino Kennji — [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective) | [Gitee](https://gitee.com/hoshinokennji/SiliconLifeCollective) | [YouTube](https://www.youtube.com/@hoshinokennji) | [Bilibili](https://space.bilibili.com/617827040)
