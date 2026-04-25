# Architektura

[English](../en/architecture.md) | [中文文档](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Čeština](../cs-CZ/architecture.md)

## Základní koncepty

### Křemíková Bytost

Každý AI agent v systému je **Křemíková Bytost** — autonomní entita s vlastní identitou, osobností a schopnostmi. Každá křemíková bytost je řízena **souborem duše** (Markdown výzva), který definuje její vzorce chování.

### Křemíkový Kurátor

**Křemíkový Kurátor** je speciální křemíková bytost s nejvyšším systémovým oprávněním. Funguje jako správce systému:

- Vytváří a spravuje ostatní křemíkové bytosti
- Analyzuje požadavky uživatelů a rozkládá je na úkoly
- Přiděluje úkoly odpovídajícím křemíkovým bytostem
- Monitoruje kvalitu provádění a zpracovává selhání
- Odpovídá na zprávy uživatelů pomocí **prioritního scheduling** (viz níže)

### Soubor Duše

Soubor Markdown (`soul.md`) uložený v datovém adresáři každé křemíkové bytosti. Funguje jako systémová výzva vložená do každého požadavku AI a definuje osobnost, rozhodovací vzorce a behaviorální omezení bytosti.

---

## Scheduling: Time-Slice Fair Scheduling

### Hlavní smyčka + Objekty Clock

Systém běží na **hlavní smyčce řízené clock** na vyhrazeném background threadu:

```
Hlavní smyčka (vyhrazený thread, watchdog + jistič)
  └── Objekt Clock A (priorita=0, interval=100ms)
  └── Objekt Clock B (priorita=1, interval=500ms)
  └── SiliconBeingManager (spouštěn clock přímo z hlavní smyčky)
        └── SiliconBeingRunner → Bytost 1 → tick clock → provede jedno kolo
        └── SiliconBeingRunner → Bytost 2 → tick clock → provede jedno kolo
        └── SiliconBeingRunner → Bytost 3 → tick clock → provede jedno kolo
        └── ...
```

Klíčová designová rozhodnutí:

- **Křemíkové bytosti nedědí objekty Clock.** Mají vlastní metodu `Tick()`, kterou volá `SiliconBeingManager` prostřednictvím `SiliconBeingRunner`, místo aby byly přímo registrovány do hlavní smyčky.
- **SiliconBeingManager** je spouštěn clock přímo z hlavní smyčky a funguje jako jediný proxy pro všechny bytosti.
- **SiliconBeingRunner** obaluje `Tick()` každé bytosti na dočasném threadu s timeoutem a jističem pro každou bytost (3 po sobě jdoucí timeouty → 1 minuta cooldown).
- Provádění každé bytosti je omezeno na **jedno kolo** požadavku AI + volání nástrojů na každý tick clocku, což zajišťuje, že žádná bytost nemůže monopolizovat hlavní smyčku.
- **Performance Monitor** sleduje dobu provádění clocku pro pozorovatelnost.

### Prioritní odpověď Kurátora

Když uživatel pošle zprávu Křemíkovému Kurátorovi:

1. Aktuální bytost (např. Bytost A) dokončí své aktuální kolo — **bez přerušení**.
2. Manager **přeskočí zbývající frontu**.
3. Smyčka **začne znovu od Kurátora**, což mu umožní okamžité provedení.

To zajišťuje reakci na interakce uživatele, aniž by docházelo k narušení probíhajících úkolů.

---

## Architektura komponent

```
┌─────────────────────────────────────────────────────────┐
│                        CoreHost                          │
│  (Unifikovaný hostitel — sestavení a správa všech komponent)│
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ Hlavní    │  │ Service       │  │      Config       │  │
│  │ smyčka    │  │ Locator       │  │                  │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │        SiliconBeingManager (objekt Clock)         │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Kurátor   │ │Bytost A │ │Bytost B │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Sdílené služby                        │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Chat       │  │ Storage  │  │ Permission        │  │   │
│  │  │System     │  │          │  │ Manager           │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │ AI       │  │Executory │  │ Tool              │  │   │
│  │  │ Klient   │  │          │  │ Manager           │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Executory                         │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Disk     │  │ Síť      │  │ Příkazový        │  │   │
│  │  │ Executor │  │ Executor │  │ Executor          │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              IM Provideři                          │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Console  │  │  Web     │  │  Feishu / ...     │  │   │
│  │  │ Provider │  │ Provider │  │  Provider          │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## Service Locator

`ServiceLocator` je threadsafe singleton registr poskytující přístup ke všem core službám:

| Vlastnost | Typ | Popis |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | Centrální manažer chatových relací |
| `IMManager` | `IMManager` | Router IM providerů |
| `AuditLogger` | `AuditLogger` | Auditní stopa oprávnění |
| `GlobalAcl` | `GlobalACL` | Globální seznam řízení přístupu |
| `BeingFactory` | `ISiliconBeingFactory` | Továrna pro vytváření bytostí |
| `BeingManager` | `SiliconBeingManager` | Manažer životního cyklu aktivních bytostí |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Loader dynamické kompilace |
| `TokenUsageAudit` | `ITokenUsageAudit` | Sledování použití tokenů |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Reportování použití tokenů |

Také udržuje registr `PermissionManager` pro každou bytost, klíčovaný podle GUID bytosti.

---

## Chatový Systém

### Typy relací

Chatový systém podporuje tři typy relací prostřednictvím `SessionBase`:

| Typ | Třída | Popis |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | Konverzace jeden-na-jednoho mezi dvěma účastníky |
| `GroupChat` | `GroupChatSession` | Skupinový chat s více účastníky |
| `Broadcast` | `BroadcastChannel` | Otevřený kanál s pevným ID; bytosti se dynamicky přihlašují a přijímají zprávy pouze po přihlášení |

### Broadcast Channel

`BroadcastChannel` je speciální typ relace pro systémová oznámení:

- **Pevné ID kanálu** — Na rozdíl od `SingleChatSession` a `GroupChatSession` je ID kanálu veřejně známá konstanta, nikoli odvozená od GUID členů.
- **Dynamické přihlašování** — Bytosti se přihlašují/odhlašují za běhu; přijímají pouze zprávy publikované po jejich přihlášení.
- **Filtrování čekajících zpráv** — `GetPendingMessages()` vrací pouze zprávy publikované po čase přihlášení bytosti, které ještě nebyly přečteny.
- **Spravováno Chat Systemem** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### Chatové zprávy

Model `ChatMessage` obsahuje pole pro kontext konverzace AI a sledování tokenů:

| Pole | Typ | Popis |
|-------|------|-------------|
| `Id` | `Guid` | Jedinečný identifikátor zprávy |
| `SenderId` | `Guid` | Jedinečný identifikátor odesílatele |
| `ChannelId` | `Guid` | Identifikátor kanálu/konverzace |
| `Content` | `string` | Obsah zprávy |
| `Timestamp` | `DateTime` | Čas odeslání zprávy |
| `Type` | `MessageType` | Text, obrázek, soubor nebo systémové oznámení |
| `ReadBy` | `List<Guid>` | ID účastníků, kteří si tuto zprávu přečetli |
| `Role` | `MessageRole` | Role konverzace AI (uživatel, asistent, nástroj) |
| `ToolCallId` | `string?` | ID volání nástroje pro zprávy výsledků nástroje |
| `ToolCallsJson` | `string?` | Serializovaný JSON volání nástroje pro zprávy asistenta |
| `Thinking` | `string?` | Řetězec myšlení AI |
| `PromptTokens` | `int?` | Počet tokenů v promptu (vstup) |
| `CompletionTokens` | `int?` | Počet tokenů v completion (výstup) |
| `TotalTokens` | `int?` | Celkový počet použitých tokenů (vstup + výstup) |
| `FileMetadata` | `FileMetadata?` | Připojená metadata souboru (pokud zpráva obsahuje soubor) |

### Fronta chatových zpráv

`ChatMessageQueue` je threadsafe systém fronty zpráv pro správu asynchronního zpracování chatových zpráv:

- **Threadsafe** — Zajišťuje bezpečný přístup při souběžném přístupu pomocí zamykacího mechanismu
- **Asynchronní zpracování** — Podporuje asynchronní enqueue a dequeue zpráv
- **Řazení zpráv** — Zachovává časové pořadí zpráv
- **Dávkové operace** — Podporuje dávkové načítání zpráv

### Metadata souboru

`FileMetadata` slouží ke správě informací o souborech připojených k chatovým zprávám:

- **Informace o souboru** — Název souboru, velikost, typ, cesta
- **Čas nahrání** — Časové razítko nahrání souboru
- **Nahrávající** — ID uživatele nebo křemíkové bytosti, která soubor nahrála

### Manažer zrušení streamu

`StreamCancellationManager` poskytuje mechanismus zrušení pro streamované odpovědi AI:

- **Řízení streamu** — Podporuje zrušení probíhajících streamovaných odpovědí AI
- **Čištění zdrojů** — Správně čistí související zdroje při zrušení
- **Concurrenční bezpečí** — Podporuje správu více streamů současně

### Zobrazení historie chatu

Nově přidaná funkce zobrazení historie chatu umožňuje uživatelům procházet historické konverzace křemíkových bytostí:

- **Seznam relací** — Zobrazuje všechny historické relace
- **Detaily zpráv** — Zobrazuje úplnou historii zpráv
- **Zobrazení časové osy** — Zobrazuje zprávy v chronologickém pořadí
- **Podpora API** — Poskytuje RESTful API pro získávání dat relací a zpráv

---

## Systém AI Klientů

Systém podporuje více AI backendů prostřednictvím rozhraní `IAIClient`:

### OllamaClient

- **Typ**: Lokální AI služba
- **Protokol**: Nativní Ollama HTTP API (`/api/chat`, `/api/generate`)
- **Funkce**: Streaming, volání nástrojů, lokální hostování modelů
- **Konfigurace**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (Alibaba Cloud Bailian)

- **Typ**: Cloudová AI služba
- **Protokol**: API kompatibilní s OpenAI (`/compatible-mode/v1/chat/completions`)
- **Autentizace**: Bearer token (API klíč)
- **Funkce**: Streaming, volání nástrojů, reasoning content (řetězec myšlení), více oblastí nasazení
- **Podporované oblasti**:
  - `beijing` — Severní Čína 2 (Peking)
  - `virginia` — USA (Virginie)
  - `singapore` — Singapur
  - `hongkong` — Hongkong, Čína
  - `frankfurt` — Německo (Frankfurt)
- **Podporované modely** (dynamicky zjišťovány prostřednictvím API s fallback seznamem):
  - **Řada Tongyi Qianwen**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **Reasoning**: qwq-plus
  - **Třetí strany**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **Konfigurace**: `apiKey`, `region`, `model`
- **Zjišťování modelů**: Runtime získávání dostupných modelů z Bailian API; fallback na kurátovaný seznam při selhání sítě

### Factory vzor klientů

Každý typ AI klienta má odpovídající factory implementaci `IAIClientFactory`:

- `OllamaClientFactory` — Vytváří instance OllamaClient
- `DashScopeClientFactory` — Vytváří instance DashScopeClient

Factory poskytují:
- `CreateClient(Dictionary<string, object> config)` — Vytvoří klienta z konfigurace
- `GetConfigKeyOptions(string key, ...)` — Vrací dynamické možnosti pro klíče konfigurace (např. dostupné modely, oblasti)
- `GetDisplayName()` — Lokalizovaný zobrazovaný název typu klienta

### Přehled podpory AI platforem

#### Vysvětlení stavu
- ✅ Implementováno
- 🚧 Ve vývoji
- 📋 Plánováno
- 💡 Zvažováno

*Poznámka: Kvůli síťovému prostředí vývojáře může přístup k [Zvažováno] cloudovým AI službám v zahraničí vyžadovat použití proxy nástrojů a proces ladění může být nestabilní.*

#### Seznam platforem

| Platforma | Stav | Typ | Popis |
|------|------|------|------|
| Ollama | ✅ | Lokální | Lokální AI služba, podpora lokálního nasazení modelů |
| DashScope (Alibaba Cloud Bailian) | ✅ | Cloud | AI služba Alibaba Cloud Bailian, podpora více oblastí nasazení |
| Baidu Qianfan (ERNIE Bot) | 📋 | Cloud | AI služba Baidu ERNIE Bot |
| Zhipu AI (GLM) | 📋 | Cloud | AI služba Zhipu Qingyan |
| Moonshot AI (Kimi) | 📋 | Cloud | AI služba Moonshot Kimi |
| Volcano Ark Engine.Doubao | 📋 | Cloud | AI služba ByteDance Doubao |
| DeepSeek (přímé připojení) | 📋 | Cloud | AI služba DeepSeek |
| 01.AI | 📋 | Cloud | AI služba 01.AI |
| Tencent Hunyuan | 📋 | Cloud | AI služba Tencent Hunyuan |
| SiliconFlow | 📋 | Cloud | AI služba SiliconFlow |
| MiniMax | 📋 | Cloud | AI služba MiniMax |
| OpenAI | 💡 | Cloud | API služba OpenAI (řada GPT) |
| Anthropic | 💡 | Cloud | AI služba Anthropic Claude |
| Google DeepMind | 💡 | Cloud | AI služba Google Gemini |
| Mistral AI | 💡 | Cloud | AI služba Mistral |
| Groq | 💡 | Cloud | Služba rychlého AI inference Groq |
| Together AI | 💡 | Cloud | Služba open-source modelů Together AI |
| xAI | 💡 | Cloud | Služba xAI Grok |
| Cohere | 💡 | Cloud | Podniková NLP služba Cohere |
| Replicate | 💡 | Cloud | Platforma pro hostování open-source modelů Replicate |
| Hugging Face | 💡 | Cloud | Open-source AI komunita a platforma modelů Hugging Face |
| Cerebras | 💡 | Cloud | Služba optimalizovaná pro AI inference Cerebras |
| Databricks | 💡 | Cloud | Podniková AI platforma Databricks (MosaicML) |
| Perplexity AI | 💡 | Cloud | Služba vyhledávání a odpovědí Perplexity AI |
| NVIDIA NIM | 💡 | Cloud | Microslužba AI inference NVIDIA |

---

## Klíčová designová rozhodnutí

### Storage jako instance třídy (ne statická)

`IStorage` je navrženo jako injectovatelná instance, nikoli statický nástroj. To zajišťuje:

- Přímý přístup k souborovému systému — IStorage je interní persistenční kanál systému, **ne** routovaný přes Executory.
- **AI nemůže ovládat IStorage** — Executory spravují IO iniciované nástroji AI; IStorage spravuje interní čtení/zápis dat frameworku. Jde o zásadně odlišné concerns.
- Testovatelnost s mock implementacemi.
- Podpora různých storage backendů v budoucnosti bez úpravy spotřebitelů.

### Executory jako bezpečnostní hranice

Executory jsou **jedinou** cestou pro I/O operace. Nástroje vyžadující přístup k disku, síti nebo příkazovému řádku **musí** procházet přes Executory. Tento design vynucuje:

- **Samostatný dispatch thread** pro každý executor s thread lockem pro ověřování oprávnění.
- Centralizované kontroly oprávnění — Executory dotazují **soukromý Permission Manager** bytosti.
- Fronta požadavků s podporou priority a timeout kontroly.
- Audit log všech externích operací.
- Izolace výjimek — selhání jednoho executoru neovlivní ostatní executory.
- Jistič — Dočasné pozastavení executoru po连续的 selháních k prevenci kaskádových selhání.

### ContextManager jako lehký objekt

Každé `ExecuteOneRound()` vytváří novou instanci `ContextManager`:

1. Načte soubor duše + nedávnou historii chatu.
2. Odešle požadavek na AI klienta.
3. Smyčka zpracování volání nástrojů, dokud AI nevrátí čistý text.
4. Persistuje odpověď do chatového systému.
5. Dispose.

To udržuje každé kolo izolované a bezstavové.

### Sebe-evoluce prostřednictvím override třídy

Křemíkové bytosti mohou přepsat svou vlastní C# třídu za běhu:

1. AI generuje kód nové třídy (musí dědit `SiliconBeingBase`).
2. **Kontrola referencí při kompilaci** (primární obrana): Kompilátor získá pouze povolený seznam assembly — `System.IO`, `System.Reflection` atd. jsou vyloučeny, takže nebezpečný kód je nemožný na úrovni typů.
3. **Statická analýza za běhu** (sekundární obrana): `SecurityScanner` skenuje kód pro nebezpečné vzory po úspěšné kompilaci.
4. Roslyn zkompiluje kód v paměti.
5. Při úspěchu: `SiliconBeingManager.ReplaceBeing()` vymění aktuální instanci, migruje stav a persistuje zašifrovaný kód na disk.
6. Při selhání: Zahodí nový kód, zachová existující implementaci.

Vlastní implementace `IPermissionCallback` mohou být také kompilovány a injektovány prostřednictvím `ReplacePermissionCallback()`, což umožňuje bytostem přizpůsobit si vlastní logiku oprávnění.

Kód je uložen na disku šifrovaný pomocí AES-256. Šifrovací klíč je odvozen z GUID bytosti (velká písmena) prostřednictvím PBKDF2.

---

## Audit použití Tokenů

`TokenUsageAuditManager` sleduje spotřebu AI tokenů pro všechny bytosti:

- `TokenUsageRecord` — Záznam pro každý požadavek (ID bytosti, model, prompt tokeny, completion tokeny, časové razítko)
- `TokenUsageSummary` — Agregované statistiky
- `TokenUsageQuery` — Parametry dotazu pro filtrování záznamů
- Persistováno prostřednictvím `ITimeStorage` pro časové řady dotazů
- Přístupné prostřednictvím Web UI (AuditController) a `TokenAuditTool` (pouze pro Kurátora)

---

## Kalendářový Systém

Systém obsahuje **32 kalendářních implementací** odvozených od abstraktní třídy `CalendarBase`, pokrývajících hlavní světové kalendářní systémy:

| Kalendář | ID | Popis |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | Buddhistský kalendář (BE), rok + 543 |
| CherokeeCalendar | `cherokee` | Kalendářní systém Cherokee |
| ChineseLunarCalendar | `lunar` | Čínský lunární kalendář s přestupnými měsíci |
| ChulaSakaratCalendar | `chula_sakarat` | Chula Sakarat (CS), rok - 638 |
| CopticCalendar | `coptic` | Koptský kalendář |
| DaiCalendar | `dai` | Dai kalendář s kompletním lunárním výpočtem |
| DehongDaiCalendar | `dehong_dai` | Varianta Dehong Dai kalendáře |
| EthiopianCalendar | `ethiopian` | Etiopský kalendář |
| FrenchRepublicanCalendar | `french_republican` | Francouzský republikánský kalendář |
| GregorianCalendar | `gregorian` | Standardní gregoriánský kalendář |
| HebrewCalendar | `hebrew` | Hebrejský (židovský) kalendář |
| IndianCalendar | `indian` | Indický národní kalendář |
| InuitCalendar | `inuit` | Kalendářní systém Inuit |
| IslamicCalendar | `islamic` | Islámský Hijri kalendář |
| JapaneseCalendar | `japanese` | Japonský kalendář éry (Nengo) |
| JavaneseCalendar | `javanese` | Jávský islámský kalendář |
| JucheCalendar | `juche` | Kalendář Juche (Severní Korea), rok - 1911 |
| JulianCalendar | `julian` | Juliánský kalendář |
| KhmerCalendar | `khmer` | Khmerský kalendář |
| MayanCalendar | `mayan` | Mayský dlouhý kalendář |
| MongolianCalendar | `mongolian` | Mongolský kalendář |
| PersianCalendar | `persian` | Perský (Solar Hijri) kalendář |
| RepublicOfChinaCalendar | `roc` | Kalendář Čínské republiky (Minguo), rok - 1911 |
| RomanCalendar | `roman` | Římský kalendář |
| SakaCalendar | `saka` | Saka kalendář (Indonésie) |
| SexagenaryCalendar | `sexagenary` | Čínský kalendář Ganzhi |
| TibetanCalendar | `tibetan` | Tibetský kalendář |
| VietnameseCalendar | `vietnamese` | Vietnamský lunární kalendář (varianta kočky) |
| VikramSamvatCalendar | `vikram_samvat` | Kalendář Vikram Samvat |
| YiCalendar | `yi` | Kalendářní systém Yi |
| ZoroastrianCalendar | `zoroastrian` | Zoroastriánský kalendář |

`CalendarTool` poskytuje operace: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (konverze data mezi kalendáři).

---

## Architektura Web UI

### Systém Skinů

Web UI má **rozšiřitelný systém skinů**, který umožňuje kompletní přizpůsobení UI bez změny aplikační logiky:

- **Rozhraní ISkin** — Definuje kontrakty pro všechny skiny, včetně:
  - Core renderovací metody (`RenderHtml`, `RenderError`)
  - 20+ metod UI komponent (tlačítka, vstupy, karty, tabulky, odznaky, bubliny, progress, tagy atd.)
  - Generování témat CSS prostřednictvím `CssBuilder`
  - `SkinPreviewInfo` — Paleta barev a ikony pro výběr skinu na inicializační stránce

- **Vestavěné skiny** — 4 produkčně připravené skiny:
  - **Admin** — Profesionální, datově orientované rozhraní pro správu systému
  - **Chat** — Konverzační, message-centric design pro AI interakce
  - **Creative** — Umělecké, vizuálně bohaté rozložení pro kreativní workflow
  - **Dev** — Developer-centric, code-centric rozhraní se zvýrazněním syntaxe

- **Objevování skinů** — `SkinManager` automaticky objevuje a registruje všechny implementace `ISkin` prostřednictvím reflexe

### HTML / CSS / JS Buildery

Web UI se zcela vyhýbá souborům šablon a generuje veškeré značky v C#:

- **`H`** — Streamovaný HTML Builder DSL pro sestavování HTML stromů v kódu
- **`CssBuilder`** — CSS Builder s podporou selektorů a media queries
- **`JsBuilder` (`JsSyntax`)** — JavaScript Builder pro inline skripty

### Systém Kontrolerů

Web UI následuje **MVC-like vzor** s 17 kontrolery obsluhujícími různé aspekty:

| Kontroler | Účel |
|------------|---------|
| About | O stránce a informace o projektu |
| Audit | Dashboard auditu použití tokenů s grafy trendů a exportem |
| Being | Správa a stav křemíkových bytostí |
| Chat | Rozhraní chatu v reálném čase se SSE |
| CodeBrowser | Prohlížení a úprava kódu |
| Config | Správa systémové konfigurace |
| Dashboard | Přehled systému a metriky |
| Executor | Stav a správa executorů |
| Init | Průvodce inicializací pro první spuštění |
| Knowledge | Vizualizace knowledge graph (placeholder) |
| Log | Prohlížeč systémových logů |
| Memory | Prohlížeč dlouhodobé paměti s pokročilým filtrováním, statistikami a zobrazením detailů |
| Permission | Správa oprávnění |
| PermissionRequest | Fronta požadavků na oprávnění |
| Project | Správa projektů (placeholder) |
| Task | Rozhraní systému úkolů |
| Timer | Správa systému časovačů |

### Aktualizace v reálném čase

- **SSE (Server-Sent Events)** — Push aktualizace pro chatové zprávy, stav bytostí a systémové události prostřednictvím `SSEHandler`
- **Žádné WebSocket** — Jednodušší architektura pomocí SSE pro většinu potřeb v reálném čase
- **Automatické opětovné připojení** — Logika opětovného připojení klienta pro odolné připojení

### Lokalizace

Vestavěny tři locale: `ZhCN` (zjednodušená čínština), `ZhHK` (tradiční čínština) a `EnUS` (angličtina). Aktivní locale se vybírá prostřednictvím `DefaultConfigData.Language` a parsuje se prostřednictvím `LocalizationManager`.

---

## Struktura datového adresáře

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # Soubor duše Kurátora
    │   ├── state.json       # Stav za běhu
    │   ├── code.enc         # Šifrovaný kód vlastní třídy AES
    │   └── permission.enc   # Šifrovaný vlastní callback oprávnění AES
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```
