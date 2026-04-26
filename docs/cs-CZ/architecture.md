# Architektura

[English](../en/architecture.md) | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Deutsch](../de-DE/architecture.md) | **Čeština**

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

---

## AI Klientský Systém

### IAIClient Rozhraní

```csharp
public interface IAIClient
{
    string Name { get; }
    Task<AIResponse> ChatAsync(AIRequest request);
    IAsyncEnumerable<string> StreamChatAsync(AIRequest request);
}
```

### Implementace Klientů

- **OllamaClient** — Lokální AI přes Ollama API
- **DashScopeClient** — Cloudová AI přes Alibaba Cloud Bailian API

### Factory Pattern

Každý AI klient má odpovídající factory:

```csharp
public interface IAIClientFactory
{
    IAIClient CreateClient(AIClientConfig config);
}
```

Factory jsou automaticky objevovány a registrovány prostřednictvím reflexe.

---

## Systém Nástrojů

### Objevování Nástrojů

Nástroje jsou automaticky objevovány prostřednictvím reflexe:

1. Skenování všech sestav pro třídy implementující `ITool`
2. Vytvoření instance a registrace do `ToolManager` každé bytosti
3. Podpora atributu `[SiliconManagerOnly]` pro nástroje pouze pro kurátora

### Vykonávání Nástrojů

```
AI vrací tool_calls
  ↓
ToolManager najde a ověří použití nástroje
  ↓
Správce oprávnění zkontroluje řetězec oprávnění
  ↓
Exekutor provede operaci přístupu ke zdroji
  ↓
AI přijme výsledek nástroje, pokračuje v myšlení
```

---

## 32 Kalendářových Systémů

Platforma podporuje 32 kalendářových systémů:

### Hlavní Kalendáře (6)
- Gregoriánský, Čínský lunární, Islámský, Hebrejský, Perský, Indický

### Čínské Historické (2)
- Čínský historický (cyklický letopočet + éry panovníků), Sexagenární

### Východoasijské (6)
- Japonský, Vietnamský, Tibetský, Mongolský, Dai, Dehong Dai

### Historické (6)
- Mayský, Římský, Juliánský, Francouzský republikánský, Koptský, Etiopský

### Regionální (6)
- Buddhistický, Saka, Vikram Samvat, Jávský, Chula Sakarat, Khmerský

### Moderní (3)
- ROC, Čučche, Zoroastriánský

### Etnické (3)
- Yi, Čerokézský, Inuitský

---

## Web UI Architektura

### Server-Side Rendering

Web UI používá čistý server-side rendering, žádná závislost na frontendovém frameworku:

- **HtmlBuilder** — Generuje HTML z C#
- **CssBuilder** — Generuje CSS styly
- **JsBuilder** — Generuje JavaScript

### SSE (Server-Sent Events)

Real-time aktualizace přes Server-Sent Events:

- Streamování chatovacích odpovědí
- Aktualizace stavu bytostí
- Systémová oznámení
- Automatické opětovné připojení klienta

### Systém Skinů

Skinů jsou automaticky objevovány prostřednictvím reflexe:

- **Admin** — Profesionální správcovské rozhraní
- **Chat** — Design zaměřený na konverzaci
- **Creative** — Kreativní a umělecký styl
- **Dev** — Rozložení orientované na vývojáře

---

## Bezpečnostní Architektura

### Vrstvená Obrana

```
Volání nástroje → Exekutor → Správce oprávnění → Vysoké zamítnutí → Vysoké povolení → Callback → Dotaz uživatele
```

### Exekutory jako Bezpečnostní Hranice

- Všechny I/O operace musí procházet přes exekutory
- Každý exekutor má nezávislé plánovací vlákno
- Kontrola oprávnění před provedením
- Zpracování časového limitu a izolace výjimek

### Dynamická Kompilační Bezpečnost

- AES-256 šifrování zkompilovaného kódu
- Statické skenování nebezpečných vzorů kódu
- Kontrola referencí při kompilaci (vyloučení nebezpečných sestav)
- Izolace paměti pro kompilovaný kód

---

## Datové Úložiště

### IStorage Rozhraní

```csharp
public interface IStorage
{
    Task<string> ReadAsync(string key);
    Task WriteAsync(string key, string value);
    Task<bool> ExistsAsync(string key);
    Task DeleteAsync(string key);
}
```

### ITimeStorage Rozhraní

```csharp
public interface ITimeStorage
{
    Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end);
    Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end, string prefix);
}
```

### Implementace

- **FileSystemStorage** — Výchozí implementace založená na souborovém systému
- Struktura klíč-hodnota
- Časově indexované dotazy
- Přístup k přímému souborovému systému — **AI nemůže ovládat IStorage**

---

## Lokalizační Systém

### Podporované Jazyky (21 variant)

- **Čínština (6)**: Zjednodušená, Tradiční, Singapurská, Macajská, Tchajwanská, Malajsijská
- **Angličtina (10)**: Americká, Britská, Kanadská, Australská, Indická, Singapurská, Jihhoafrická, Irská, Novozélandská, Malajsijská
- **Španělština (2)**: Španělská, Mexická
- **Japonština, Korejština, Čeština**

### Implementace

```csharp
public abstract class LocalizationBase
{
    public abstract string LanguageCode { get; }
    public abstract string GetTranslation(string key);
}
```

---

## Další Kroky

- 🚀 Podívejte se na [Průvodce Rychlým Startem](getting-started.md)
- 🛠️ Přečtěte si [Vývojářskou Příručku](development-guide.md)
- 📚 Prozkoumejte [Referenci API](api-reference.md)
- 🔒 Pochopte [Bezpečnostní Návrh](security.md)
