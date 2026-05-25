# Architektura

> **Verze: v0.2.0-alpha**

[English](../en/architecture.md) | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Deutsch](../de-DE/architecture.md) | **Čeština** | [Русский](../ru-RU/architecture.md)

## Duální Verze Architektury

Tento projekt poskytuje dvě implementační verze, které sdílejí stejný návrh architektury, ale liší se v úložišti a optimalizaci výkonu:

### SiliconLife.Default (Výchozí Verze)
- **Pozicování**: Výchozí implementace, používána především pro ověření proveditelnosti architektury
- **Režim Spuštění**: Konzolová aplikace
- **Úložiště**: Čisté souborové JSON úložiště
- **Scénář Použití**: Vysoké požadavky na bezpečnost dat, omezené paměťové zdroje, scénáře s malým objemem dat
- **Popis role**: Referenční implementace pro ověření architektury, poskytuje jednoduché a spolehlivé spuštění, vhodná pro první kontakt, vývojové ladění nebo scénáře s prioritou bezpečnosti dat

### SiliconLife.Fast (Vysoce Výkonná Verze)
- **Pozicování**: Hlavní produkční verze
- **Režim Spuštění**: Desktopová aplikace (Windows systémová lišta / Linux stavové okno)
- **Úložiště**: SpeedyPack paměťové úložiště + asynchronní dávková perzistence (.spk formát souboru)
- **Scénář Použití**: Scénáře s vysokou souběžností, nízkou latencí, velkým objemem dat
- **Podpora Platformy**: Windows/macOS (plné funkce, včetně systémové lišty), Linux (stavové okno, bez ikony v liště)
- **Charakteristiky**:
  - Windows/macOS běh na pozadí v systémové liště s monitorováním v reálném čase prostřednictvím stavového okna lišty; Linux stavové okno zobrazeno přímo
  - SpeedyPack engine + automatická komprese zajišťují bezpečnost dat
  - Architektura Component UI, 30+ deklarativních komponent
  - 7 témat skinů, podpora automatické detekce a přepínání
  - Nástroj hot reload pro online aktualizace a restarty
  - Linux automaticky otevře prohlížeč pro přístup k Web UI, podporuje parametr `--no-tray`
- **Zlepšení Výkonu**: Latence čtení úložiště snížena 1000x, latence zápisu snížena 15000x
- **Popis role**: Produkční implementace s hlubokou optimalizací, se systémovou lištou na pozadí, SpeedyPack engine + automatická komprese zajišťují bezpečnost dat, nejlepší volba pro dlouhodobý provoz a reálné produkční prostředí

> **Poznámka**: Architektura popsaná v tomto dokumentu platí pro obě verze, s rozdíly pouze v části implementace úložiště. SiliconLife.Default je referenční ověření architektury, SiliconLife.Fast je hlavní verze pro produkční prostředí.

---

## Core Koncepty

### Silikonová Bytost

Každý AI agent v systému je **silikonová bytost** — autonomní entita s vlastní identitou, osobností a schopnostmi. Každá silikonová bytost je řízena **souborem duše** (Markdown prompt), který definuje její vzorce chování.

### Silikonový Kurátor

**Silikonový kurátor** je speciální silikonová bytost s nejvyšším systémovým oprávněním. Působí jako správce systému:

- Vytváří a spravuje další silikonové bytosti
- Analyzuje požadavky uživatelů a rozkládá je na úkoly
- Přiděluje úkoly odpovídajícím silikonovým bytostem
- Monitoruje kvalitu provádění a řeší selhání
- Odpovídá na zprávy uživatelů pomocí **prioritního rozvrhování** (viz níže)

### Soubor Duše

Markdown soubor (`soul.md`) uložený v datovém adresáři každé silikonové bytosti. Je vstřikován jako systémový prompt do každého AI požadavku a definuje osobnost bytosti, vzorce rozhodování a behaviorální omezení.

---

## Plánování: Spravedlivé Plánování Časových Slotů

### Hlavní Smyčka + Objekt Hodin

Systém běží na **hlavní smyčce řízené hodinami** na vyhrazeném vláknu na pozadí:

```
Hlavní smyčka (vyhrazené vlákno, watchdog + jistič)
  └── Objekt hodin A (priorita=0, interval=100ms)
  └── Objekt hodin B (priorita=1, interval=500ms)
  └── Správce silikonových bytostí (spouštěn přímo hlavní smyčkou)
        └── Běžeč bytosti → Bytost 1 → Spuštění hodin → Proveď jedno kolo
        └── Běžeč bytosti → Bytost 2 → Spuštění hodin → Proveď jedno kolo
        └── Běžeč bytosti → Bytost 3 → Spuštění hodin → Proveď jedno kolo
        └── ...
```

Klíčová rozhodnutí o designu:

- **Silikonové bytosti nedědí objekt hodin.** Mají vlastní metodu `Tick()`, která je volána `SiliconBeingManagerem` prostřednictvím `SiliconBeingRunneru`, místo aby byly přímo registrovány do hlavní smyčky.
- **Správce silikonových bytostí** je spouštěn přímo hlavní smyčkou a působí jako jediný agent pro všechny bytosti.
- **Běžeč silikonových bytostí** obaluje `Tick()` každé bytosti na dočasném vlákně s časovým limitem a jističem pro každou bytost (3 po sobě jdoucí časové limity → 1minutové ochlazení).
- Provádění každé bytosti je omezeno na **jedno kolo** AI požadavku + volání nástrojů na spuštění hodin, což zajišťuje, že žádná bytost nemůže monopolizovat hlavní smyčku.
- **Monitor výkonu** sleduje dobu provádění hodin pro pozorovatelnost.

### Prioritní Odpověď Kurátora

Když uživatel odešle zprávu silikonovému kurátorovi:

1. Aktuální bytost (např. Bytost A) dokončí své aktuální kolo — **bez přerušení**.
2. Správce **přeskočí zbývající frontu**.
3. Smyčka **začne znovu od kurátora**, což mu umožní okamžité provedení.

To zajišťuje reakci na interakci uživatele, aniž by narušovalo probíhající úkoly.

---

## Architektura Komponent

```
┌─────────────────────────────────────────────────────────┐
│                        Core Host                         │
│  (Unifikovaný host — montáž a správa všech komponent)   │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ Hlavní    │  │ Service      │  │    Konfigurace    │  │
│  │ smyčka    │  │ Locator      │  │                  │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │     Správce silikonových bytostí (objekt hodin)    │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Kurátor   │ │Bytost A │ │Bytost B │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Sdílené služby                        │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Chat     │  │ Úložiště │  │ Správce         │  │   │
│  │  │ systém   │  │          │  │ oprávnění       │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │ AI       │  │Exekutor  │  │ Správce         │  │   │
│  │  │ klient   │  │          │  │ nástrojů        │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  │  ┌──────────┐ ┌──────────┐                        │   │
│  │  │Zavaděč   │  │Znalostní │                        │   │
│  │  │pluginů   │  │ síť      │                        │   │
│  │  └──────────┘ └──────────┘                        │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Exekutory                         │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │  Disk    │  │ Síť      │  │ Příkazový       │  │   │
│  │  │ exekutor │  │ exekutor │  │ exekutor        │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              IM Provideři                         │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Konzole  │  │  Web     │  │  Feishu / ...    │  │   │
│  │  │ provider │  │ provider │  │  provider        │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## Service Locator

`ServiceLocator` je thread-safe singleton registr poskytující přístup ke všem core službám:

| Vlastnost | Typ | Popis |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | Centrální správce chatovacích relací |
| `IMManager` | `IMManager` | Router IM providerů |
| `AuditLogger` | `AuditLogger` | Auditní stopa oprávnění |
| `GlobalAcl` | `GlobalACL` | Globální seznam řízení přístupu |
| `BeingFactory` | `ISiliconBeingFactory` | Factory pro vytváření bytostí |
| `BeingManager` | `SiliconBeingManager` | Správce životního cyklu aktivních bytostí |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Načítač dynamické kompilace |
| `TokenUsageAudit` | `ITokenUsageAudit` | Sledování využití tokenů |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Vykazování využití tokenů |

Také udržuje registr `PermissionManager` pro každou bytost, klíčovaný podle GUID bytosti.

---

## Chatovací Systém

### Typy Relací

Chatovací systém podporuje tři typy relací prostřednictvím `SessionBase`:

| Typ | Třída | Popis |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | Jedna konverzace mezi dvěma účastníky |
| `GroupChat` | `GroupChatSession` | Skupinový chat s více účastníky |
| `Broadcast` | `BroadcastChannel` | Otevřený kanál s pevným ID; bytosti se dynamicky přihlašují, přijímají zprávy pouze po přihlášení |

### Vysílací Kanál

`BroadcastChannel` je speciální typ relace pro systémová oznámení:

- **Pevné ID kanálu** — Na rozdíl od `SingleChatSession` a `GroupChatSession`, ID kanálu je známá konstanta, ne odvozená z GUID členů.
- **Dynamické přihlášení** — Bytosti se přihlašují/odhlašují za běhu; přijímají pouze zprávy publikované po jejich přihlášení.
- **Filtrování čekajících zpráv** — `GetPendingMessages()` vrací pouze zprávy publikované po čase přihlášení bytosti, které ještě nebyly přečteny.
- **Spravováno chatovacím systémem** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### Chatovací Zpráva

Model `ChatMessage` obsahuje pole pro kontext AI konverzace a sledování tokenů:

| Pole | Typ | Popis |
|-------|------|-------------|
| `Id` | `Guid` | Unikátní identifikátor zprávy |
| `SenderId` | `Guid` | Unikátní identifikátor odesílatele |
| `ChannelId` | `Guid` | Identifikátor kanálu/konverzace |
| `Content` | `string` | Obsah zprávy |
| `Timestamp` | `DateTime` | Čas odeslání zprávy |
| `Type` | `MessageType` | Text, obrázek, soubor nebo systémové oznámení |
| `ReadBy` | `List<Guid>` | ID účastníků, kteří si tuto zprávu přečetli |
| `Role` | `MessageRole` | Role AI konverzace (uživatel, asistent, nástroj) |
| `ToolCallId` | `string?` | ID volání nástroje pro zprávy výsledků nástroje |
| `ToolCallsJson` | `string?` | Serializovaný JSON volání nástrojů pro zprávy asistenta |
| `Thinking` | `string?` | Řetězec myšlenek AI reasoningu |
| `PromptTokens` | `int?` | Počet tokenů v promptu (vstup) |
| `CompletionTokens` | `int?` | Počet tokenů v completion (výstup) |
| `TotalTokens` | `int?` | Celkový počet použitých tokenů (vstup + výstup) |
| `FileMetadata` | `FileMetadata?` | Připojená metadata souboru (pokud zpráva obsahuje soubor) |

### Fronta Chatovacích Zpráv

`ChatMessageQueue` je thread-safe systém fronty zpráv pro správu asynchronního zpracování chatovacích zpráv:

- **Thread-safe** — Používá zamykací mechanismus pro bezpečný souběžný přístup
- **Asynchronní zpracování** — Podporuje asynchronní enqueue a dequeue zpráv
- **Řazení zpráv** — Zachovává časové pořadí zpráv
- **Dávkové operace** — Podporuje dávkové načítání zpráv

### Metadata Souboru

`FileMetadata` slouží ke správě informací o souborech připojených k chatovacím zprávám:

- **Informace o souboru** — Název souboru, velikost, typ, cesta
- **Čas nahrání** — Časové razítko nahrání souboru
- **Nahrávající** — ID uživatele nebo silikonové bytosti, která soubor nahrála

### Správce Zrušení Streamu

`StreamCancellationManager` poskytuje mechanismus zrušení pro AI streamované odpovědi:

- **Ovládání streamu** — Podporuje zrušení probíhajících AI streamovaných odpovědí
- **Čištění zdrojů** — Správně čistí přidružené zdroje při zrušení
- **Souběžně bezpečný** — Podporuje správu více streamů současně

### Zobrazení Historie Chatu

Nová funkce zobrazení historie chatu umožňuje uživatelům procházet historické konverzace silikonových bytostí:

- **Seznam relací** — Zobrazuje všechny historické relace
- **Detaily zpráv** — Zobrazení úplné historie zpráv
- **Časová osa** — Zobrazení zpráv v chronologickém pořadí
- **API podpora** — Poskytuje RESTful API pro získávání dat relací a zpráv

---

## AI Klientský Systém

Systém podporuje více AI backendů prostřednictvím rozhraní `IAIClient`:

### OllamaClient

- **Typ**: Lokální AI služba
- **Protokol**: Nativní Ollama HTTP API (`/api/chat`, `/api/generate`)
- **Funkce**: Streamování, volání nástrojů, lokální hostování modelů
- **Konfigurace**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (Alibaba Cloud Bailian)

- **Typ**: Cloudová AI služba
- **Protokol**: API kompatibilní s OpenAI (`/compatible-mode/v1/chat/completions`)
- **Autentizace**: Bearer token (API klíč)
- **Funkce**: Streamování, volání nástrojů, obsah reasoningu (řetězec myšlenek), nasazení ve více regionech
- **Podporované regiony**:
  - `beijing` — Severní Čína 2 (Peking)
  - `virginia` — USA (Virginie)
  - `singapore` — Singapur
  - `hongkong` — Hongkong, Čína
  - `frankfurt` — Německo (Frankfurt)
- **Podporované modely** (dynamicky objevované přes API, s fallback seznamem):
  - **Řada Qwen**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **Reasoning**: qwq-plus
  - **Třetí strany**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **Konfigurace**: `apiKey`, `region`, `model`
- **Objevování modelů**: Za běhu načítá dostupné modely z Bailian API; při selhání sítě se vrátí k kurátorovanému seznamu

### VolcengineArkClient (Volcengine Ark)

- **Typ**: Cloudová AI služba
- **Protokol**: API kompatibilní s OpenAI
- **Autentizace**: Bearer token (API klíč)
- **Funkce**: Podporuje streaming a non-streaming režimy, integrované dvojí omezení rychlosti
  - Klientské omezení rychlosti: Vynucuje minimální interval mezi požadavky
  - Serverové omezení rychlosti: Zpracovává chyby 429, opakování s exponenciálním backoff
- **Konfigurace**: `apiKey`, `endpoint`, `model`
- **Charakteristiky**: AI služba ByteDance, podporuje různé modely Doubao

### Factory Pattern Klientů

Každý typ AI klienta má odpovídající factory implementaci `IAIClientFactory`:

- `OllamaClientFactory` — Vytváří instance OllamaClient
- `DashScopeClientFactory` — Vytváří instance DashScopeClient
- `VolcengineArkClientFactory` — Vytváří instance VolcengineArkClient

Factory poskytují:
- `CreateClient(Dictionary<string, object> config)` — Inicializuje klienta z konfigurace
- `GetConfigKeyOptions(string key, ...)` — Vrací dynamické možnosti pro konfigurační klíč (např. dostupné modely, regiony)
- `GetDisplayName()` — Lokalizovaný zobrazovaný název typu klienta

### Seznam Podpory AI Platforem

#### Popis Stavu
- ✅ Implementováno
- 🚧 Ve vývoji
- 📋 Plánováno
- 💡 Zvažováno

*Poznámka: Vzhledem k síťovému prostředí vývojáře může přístup ke zvažovaným zámořským cloudovým AI službám vyžadovat použití síťových proxy nástrojů a proces ladění může být nestabilní.*

#### Seznam Platforem

| Platforma | Stav | Typ | Popis |
|------|------|------|------|
| Ollama | ✅ | Lokální | Lokální AI služba, podpora nasazení lokálních modelů |
| DashScope (Alibaba Cloud Bailian) | ✅ | Cloud | Alibaba Cloud Bailian AI služba, podpora nasazení ve více regionech |
| Baidu Qianfan (Wenxin Yiyan) | 📋 | Cloud | Baidu Wenxin Yiyan AI služba |
| Zhipu AI (GLM) | 📋 | Cloud | Zhipu Qingyan AI služba |
| Moonshot (Kimi) | 📋 | Cloud | Moonshot Kimi AI služba |
| Volcano Ark Engine Doubao | ✅ | Cloud | ByteDance Doubao AI služba |
| DeepSeek (přímé připojení) | 📋 | Cloud | DeepSeek AI služba |
| Yi (Zero-One) | 📋 | Cloud | Yi AI služba |
| Tencent Hunyuan | 📋 | Cloud | Tencent Hunyuan AI služba |
| SiliconFlow | 📋 | Cloud | SiliconFlow AI služba |
| MiniMax | 📋 | Cloud | MiniMax AI služba |
| OpenAI | 💡 | Cloud | OpenAI API služba (řada GPT) |
| Anthropic | 💡 | Cloud | Anthropic Claude AI služba |
| Google DeepMind | 💡 | Cloud | Google Gemini AI služba |
| Mistral AI | 💡 | Cloud | Mistral AI služba |
| Groq | 💡 | Cloud | Groq vysokorychlostní AI inference služba |
| Together AI | 💡 | Cloud | Together AI služba open-source modelů |
| xAI | 💡 | Cloud | xAI Grok služba |
| Cohere | 💡 | Cloud | Cohere enterprise NLP služba |
| Replicate | 💡 | Cloud | Replicate platforma pro hostování open-source modelů |
| Hugging Face | 💡 | Cloud | Hugging Face open-source AI komunita a platforma modelů |
| Cerebras | 💡 | Cloud | Cerebras AI inference optimalizační služba |
| Databricks | 💡 | Cloud | Databricks enterprise AI platforma (MosaicML) |
| Perplexity AI | 💡 | Cloud | Perplexity AI vyhledávací Q&A služba |
| NVIDIA NIM | 💡 | Cloud | NVIDIA AI inference mikroservis |

---

## Web UI Architektura

### Systém Skinů

Web UI má **pluginový systém skinů**, který umožňuje kompletní přizpůsobení UI bez změny aplikační logiky:

- **Rozhraní ISkin** — Definuje kontrakt pro všechny skiny, včetně:
  - Core renderovací metody (`RenderHtml`, `RenderError`)
  - 20+ UI komponentových metod (tlačítka, vstupy, karty, tabulky, odznaky, bubliny, progres, štítky atd.)
  - Generování CSS motivu prostřednictvím `CssBuilder`
  - `SkinPreviewInfo` — Paleta barev a ikony pro výběr skinu na úvodní stránce

- **Vestavěné skiny** — 7 produkčně připravených skinů:
  - **Admin** — Profesionální, datově zaměřené rozhraní správy systému
  - **Chat** — Konverzační, na zprávy zaměřený design pro AI interakce
  - **Creative** — Umělecké, vizuálně bohaté rozložení pro kreativní pracovní postupy
  - **Dev** — Na vývojáře zaměřené, kódově orientované rozhraní se zvýrazněním syntaxe
  - **HighContrast** — Téma s vysokým kontrastem pro přístupnost
  - **Light** — Čisté světlé téma
  - **Minimal** — Minimalistické téma

- **Objevování skinů** — `SkinManager` automaticky objevuje a registruje všechny implementace `ISkin` prostřednictvím reflexe

### HTML / CSS / JS Buildery

Web UI se zcela vyhýbá šablonovým souborům a generuje veškerý markup v C#:

- **`H`** — Flow HTML builder DSL pro vytváření stromu HTML v kódu
- **`CssBuilder`** — CSS builder podporující selektory a media queries
- **`JsBuilder` (`JsSyntax`)** — JavaScript builder pro inline skripty

### Kontrolerový Systém

Web UI následuje **MVC-like pattern** s 23 kontrolery zpracovávajícími různé aspekty:

| Kontroler | Účel |
|------------|---------|
| About | O aplikaci a informace o projektu |
| Being | Správa a stav silikonových bytostí |
| Chat | Rozhraní chatu v reálném čase s SSE |
| ChatHistory | Zobrazení historie chatu, podpora seznamu relací a detailů zpráv |
| CodeBrowser | Prohlížení a úprava kódu |
| CodeHover | Plovoucí nápovědy kódu, podpora zvýraznění syntaxe |
| Config | Správa systémové konfigurace |
| Dashboard | Přehled systému a metriky |
| Executor | Stav a správa exekutorů |
| Help | Systém nápovědní dokumentace, vícejazyčná podpora |
| Init | Průvodce inicializací při prvním spuštění |
| Knowledge | Vizualizace a dotazování znalostního grafu |
| Log | Prohlížeč systémových logů, podpora filtrování silikonových bytostí |
| Memory | Prohlížeč dlouhodobé paměti, podpora pokročilého filtrování, statistik a detailů |
| Permission | Správa oprávnění |
| PermissionRequest | Fronta žádostí o oprávnění |
| Project | Správa projektů, včetně pracovních poznámek a úkolového systému |
| System | Systémová správa a monitorování běhu |
| Task | Rozhraní úkolového systému |
| Timer | Správa systému časovačů, včetně historie provádění |
| Usage | Dashboard auditu využití tokenů s grafy trendů a exportem |
| WorkNote | Správa pracovních poznámek, podpora vyhledávání a generování obsahu |

### Aktualizace v Reálném Čase

- **SSE (Server-Sent Events)** — Push aktualizace chatových zpráv, stavu bytostí a systémových událostí prostřednictvím `SSEHandler`
- **Žádný WebSocket** — Jednodušší architektura s SSE pro většinu požadavků v reálném čase
- **Automatické znovupřipojení** — Logika znovupřipojení na straně klienta pro odolné připojení

### Lokalizace

Systém podporuje kompletní lokalizaci **29 jazykových variant**:
- **Čínština (6)**: zh-CN (zjednodušená), zh-HK (tradiční), zh-SG (Singapur), zh-MO (Macao), zh-TW (Tchaj-wan), zh-MY (Malajsie)
- **Angličtina (10)**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **Španělština (2)**: es-ES, es-MX
- **Němčina (5)**: de-DE, de-AT, de-CH, de-LU, de-LI
- **Francouzština (3)**: fr-FR, fr-CA, fr-CH
- **Ostatní (3)**: ja-JP (japonština), ko-KR (korejština), cs-CZ (čeština)

Aktivní jazykové prostředí je vybráno prostřednictvím `DefaultConfigData.Language` a vyhodnoceno přes `LocalizationManager`.

---

## Klíčová Rozhodnutí o Designu

### Úložiště jako Instance (ne Statické)

`IStorage` je navrženo jako injectable instance, nikoli jako statický nástroj. To zajišťuje:

- Přímý přístup k souborovému systému — IStorage je interní perzistenční kanál systému, **není** směrován přes exekutory.
- **AI nemůže ovládat IStorage** — Exekutory spravují IO iniciované AI nástroji; IStorage spravuje vlastní interní čtení a zápis dat frameworku. Toto jsou zásadně odlišné zájmy.
- Testovatelnost s mock implementacemi.
- Budoucí podpora různých úložných backendů bez úpravy konzumentů.

### Exekutory jako Bezpečnostní Hranice

Exekutory jsou **jedinou** cestou pro I/O operace. Nástroje vyžadující přístup k disku, síti nebo příkazovému řádku **musí** procházet přes exekutory. Tento design vynucuje:

- **Nezávislé plánovací vlákno** pro každý exekutor s uzamčením vlákna pro ověřování oprávnění.
- Centralizovaná kontrola oprávnění — exekutory dotazují **privátního správce oprávnění** bytosti.
- Fronta požadavků s podporou priority a řízení časového limitu.
- Auditní logování všech externích operací.
- Izolace výjimek — selhání jednoho exekutoru neovlivňuje ostatní.
- Jistič — po sobě jdoucí selhání dočasně zastaví exekutor, aby se předešlo kaskádovým selháním.

### ContextManager jako Lehký Objekt

Každé `ExecuteOneRound()` vytváří novou instanci `ContextManager`:

1. Načte soubor duše + nedávnou historii chatu.
2. Odešle požadavek AI klientovi.
3. Zpracuje volání nástrojů ve smyčce, dokud AI nevrátí čistý text.
4. Perzistuje odpověď do chatovacího systému.
5. Uvolní.

To udržuje každé kolo izolované a bezstavové.

### Sebevývoj Přepsáním Třídy

Silikonové bytosti mohou za běhu přepisovat své vlastní C# třídy:

1. AI generuje nový kód třídy (musí dědit z `SiliconBeingBase`).
2. **Kontrola referencí při kompilaci** (primární obrana): kompilátor získá pouze povolený seznam sestav — `System.IO`, `System.Reflection` atd. jsou vyloučeny, takže nebezpečný kód je nemožný na úrovni typů.
3. **Statická analýza za běhu** (sekundární obrana): `SecurityScanner` skenuje kód po úspěšné kompilaci na nebezpečné vzory.
4. Roslyn zkompiluje kód v paměti.
5. Při úspěchu: `SiliconBeingManager.ReplaceBeing()` vymění aktuální instanci, migruje stav a perzistuje šifrovaný kód na disk.
6. Při selhání: nový kód je zahozen, stávající implementace zůstává.

Vlastní implementace `IPermissionCallback` může být také zkompilována a injektována prostřednictvím `ReplacePermissionCallback()`, což umožňuje bytostem přizpůsobit svou vlastní logiku oprávnění.

Kód je na disku uložen šifrovaný pomocí AES-256. Šifrovací klíč je odvozen z GUID bytosti (velká písmena) pomocí PBKDF2.

---

## Audit Využití Tokenů

`TokenUsageAuditManager` sleduje spotřebu AI tokenů napříč všemi bytostmi:

- `TokenUsageRecord` — Záznam každého požadavku (ID bytosti, model, prompt tokeny, completion tokeny, časové razítko)
- `TokenUsageSummary` — Agregované statistiky
- `TokenUsageQuery` — Parametry dotazu pro filtrování záznamů
- Perzistentní prostřednictvím `ITimeStorage` pro časové řady dotazů
- Přístupné přes Web UI (UsageController) a `TokenAuditTool` (pouze kurátor)

---

### Systém Automatizace WebView Prohlížeče (Nové)

Systém integruje automatizační funkce WebView prohlížeče založené na **Playwright**:

- **Individuální izolace**: Každá silikonová bytost má vlastní instanci prohlížeče, cookies a úložiště relací, zcela izolované a vzájemně neovlivňované.
- **Headless režim**: Prohlížeč běží v uživateli zcela neviditelném headless režimu, silikonové bytosti operují autonomně na pozadí.
- **WebViewBrowserTool**: Poskytuje kompletní schopnosti ovládání prohlížeče, včetně:
  - Navigace na stránce, kliknutí, zadávání textu, získávání obsahu stránky
  - Spouštění JavaScriptu, pořizování snímků obrazovky, čekání na prvky
  - Správa stavu prohlížeče a čištění zdrojů
- **Bezpečnostní kontrola**: Všechny operace prohlížeče musí projít řetězcem ověřování oprávnění, aby se zabránilo přístupu k škodlivým webovým stránkám.

### Znalostní Síť (Nové)

Systém obsahuje vestavěný systém znalostního grafu založený na **trojicové struktuře**:

- **Reprezentace znalostí**: Používá strukturu trojice "subjekt-relace-objekt" (např.: Python-je_programovací_jazyk)
- **KnowledgeTool**: Poskytuje správu celého životního cyklu znalostí:
  - `add`/`query`/`update`/`delete` - základní CRUD operace
  - `search` - fulltextové vyhledávání a shoda klíčových slov
  - `get_path` - objevování asociačních cest mezi dvěma koncepty
  - `validate` - kontrola úplnosti znalostí
  - `stats` - statistická analýza znalostní sítě
- **Perzistentní úložiště**: Trojice znalostí jsou perzistovány na souborový systém s podporou časově indexovaných dotazů.
- **Skóre důvěryhodnosti**: Každá položka znalostí má skóre důvěryhodnosti (0-1), podporuje fuzzy matching a řazení znalostí.
- **Tagová klasifikace**: Podpora přidávání tagů ke znalostem pro snadnější kategorizaci a vyhledávání.

---

## Struktura Datového Adresáře

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # Soubor duše kurátora
    │   ├── state.json       # Stav za běhu
    │   ├── code.enc         # AES šifrovaný kód vlastní třídy
    │   └── permission.enc   # AES šifrované vlastní zpětné volání oprávnění
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```

---

## SpeedyPack Úložný Engine

SiliconLife.Fast používá vlastní SpeedyPack úložný engine (.spk formát), který nahradil předchozí řešení LiteDB a dosahuje extrémní výkon čtení a zápisu.

### Návrh Architektury

```
┌──────────────────────────────────────────────────────────┐
│                    SpeedyPack                             │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────────┐  │
│  │ DirectoryMap  │  │  EntryCache   │  │  WriteQueue   │  │
│  │ (mapování     │  │  (mezipaměť   │  │ (asynchronní  │  │
│  │  paměťových   │  │   záznamů)    │  │  fronta       │  │
│  │  adresářů)    │  │              │  │  zápisu)      │  │
│  └──────┬───────┘  └──────┬───────┘  └───────┬───────┘  │
│         │                  │                   │          │
│  ┌──────▼──────────────────▼───────────────────▼───────┐  │
│  │              PackFileReader / PackFileWriter          │  │
│  │              (čtečka/zapisovač balíčkových souborů)   │  │
│  └──────────────────────────┬──────────────────────────┘  │
│                              │                             │
│  ┌──────────────────────────▼──────────────────────────┐  │
│  │              .spk soubor (MessagePack + LZ4 komprese) │  │
│  └─────────────────────────────────────────────────────┘  │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐                      │
│  │  FreeList     │  │ SpeedyPack   │                      │
│  │ (správa       │  │ AutoCompactor│                      │
│  │  volného      │  │ (automatická │                      │
│  │  prostoru)    │  │  komprese)   │                      │
│  └──────────────┘  └──────────────┘                      │
└──────────────────────────────────────────────────────────┘
```

### Core Komponenty

| Komponenta | Popis |
|------|------|
| `SpeedyPack` | Jádrová třída, kombinuje DirectoryMap, EntryCache a WriteQueue pro nízkolatencní čtení a zápis |
| `DirectoryMap` | Mapování paměťových adresářů, udržuje mapování virtuálních cest na záznamy souborů |
| `EntryCache` | Mezipaměť záznamů, TTL-based mezipaměť nedávno přístupných záznamů |
| `WriteQueue` | Asynchronní fronta zápisu, řadí operace zápisu do vlákna na pozadí |
| `FreeList` | Správa volného prostoru, sleduje znovupoužitelný prostor v .spk souborech |
| `PackFileReader` | Čtečka balíčkových souborů, čte data z .spk souborů |
| `PackFileWriter` | Zapisovač balíčkových souborů, zapisuje data do .spk souborů |
| `SpeedyPackAutoCompactor` | Automatický kompresní časovač, pravidelně komprimuje .spk soubory pro uvolnění volného prostoru |
| `SpeedyPackRegistry` | Správce singleton na úrovni procesu, zajišťuje, že celá aplikace používá stejnou instanci SpeedyPack |

### Úložné Adaptéry

SiliconLife.Fast integruje SpeedyPack do systémových rozhraní prostřednictvím následujících adaptérů:

| Adaptér | Rozhraní | Popis |
|--------|------|------|
| `SpeedyStorage` | `IStorage` | Adaptér obecného úložiště klíč-hodnota |
| `SpeedyTimeStorage` | `ITimeStorage` | Adaptér časově indexovaného úložiště |
| `SpeedyWorkNoteStorage` | `IWorkNoteStorage` | Adaptér úložiště pracovních poznámek |

### Konfigurační Možnosti

`SpeedyPackOptions` poskytuje následující konfiguraci:

| Možnost | Typ | Výchozí hodnota | Popis |
|------|------|--------|------|
| `CacheTtl` | `TimeSpan` | 5 minut | Doba života položek mezipaměti |
| `MaxCacheEntries` | `int` | 1000 | Maximální počet položek mezipaměti |
| `ReadOnly` | `bool` | false | Režim pouze pro čtení |

### Podpora Transakcí

SpeedyPack podporuje atomické operace zápisu prostřednictvím rozhraní `IPackTransaction`:

- `SpeedyTransaction` implementuje transakční mechanismus
- Podporuje atomicitu dávkového zápisu
- Při commitu transakce všechny operace zápisu buď uspějí, nebo se všechny vrátí zpět

---

## Plugin Systém

SiliconLife podporuje rozšíření funkcí prostřednictvím plugin systému, umožňující vývojářům třetích stran přidávat do platformy nové funkce.

### Core Rozhraní

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

### Zavaděč Pluginů

`PluginLoader` je zodpovědný za načítání plugin DLL z určeného adresáře a provádění přísných bezpečnostních kontrol:

1. **Skenování adresáře** — Skenuje všechny .dll soubory v adresáři pluginů
2. **Bezpečnostní skenování** — Kontroluje, zda pluginy neodkazují na zakázané jmenné prostory
3. **Izolované načítání** — Používá vlastní `AssemblyLoadContext` pro izolované načítání pluginů
4. **Správa životního cyklu** — Volá metody OnLoad, OnStart, OnStop, OnUnload pluginů

### Bezpečnostní Sandbox

Zavaděč pluginů provádí následující bezpečnostní kontroly:

| Kontrola | Popis |
|--------|------|
| Zakázané jmenné prostory | System.IO, System.Net.Http, System.Net.WebSockets, System.Net.Sockets, Microsoft.CodeAnalysis |
| Whitelist důvěryhodných sestav | Google.Protobuf, Newtonsoft.Json, MessagePack, Serilog, Microsoft.Extensions.Logging.Abstractions, Dapper |
| Kontrola zakázaných typů | Skenování nebezpečných typů odkazovaných v pluginech |
| Kontrola zakázaných členů | Skenování nebezpečných metod volaných v pluginech |

### Integrace Nástrojů

Pluginy mohou registrovat vlastní nástroje implementací rozhraní `ITool`:

- Metoda `ToolManager.ScanAllPluginAssemblies()` skenuje implementace ITool ve všech načtených pluginech
- Pluginové nástroje jsou automaticky integrovány do cyklu volání nástrojů
- Pluginové nástroje podléhají stejnému systému oprávnění

### Životní Cyklus Pluginu

```
Načtení (OnLoad) → Spuštění (OnStart) → Běží → Zastavení (OnStop) → Uvolnění (OnUnload)
```

---

## Stavy Aktivity Silikonové Bytosti

Silikonové bytosti mají následující stavy aktivity:

| Stav | Popis |
|------|------|
| `Idle` | Nečinný stav, čeká na spuštění hodin |
| `SingleChat` | Soukromý chat s uživatelem |
| `GroupChat` | Skupinový chat |
| `Task` | Provádění úkolu |
| `Timer` | Časovač spuštěn |
| `Broadcast` | Vysílání zpráv |
| `Project` | Práce na projektu |
| `MemoryCompression` | Komprese paměti |
| `Stopped` | Zastaveno, z důvodu po sobě jdoucích chyb nebo ručního zastavení |

**Mechanismus stavu Stopped**:
- Když silikonová bytost zaznamená 10 po sobě jdoucích chyb, automaticky přejde do stavu `Stopped`
- Ve stavu Stopped již bytost nebude provádět žádné úkoly
- K restartu je vyžadován manuální zásah

Přechody stavů:
```
Idle → SingleChat → Idle
Idle → GroupChat → Idle
Idle → Task → Idle
Idle → Timer → Idle
Idle → Broadcast → Idle
Idle → Project → Idle
Idle → MemoryCompression → Idle
Jakýkoli stav → Stopped (10 po sobě jdoucích chyb nebo ruční zastavení)
Stopped → Idle (restartování)
```
