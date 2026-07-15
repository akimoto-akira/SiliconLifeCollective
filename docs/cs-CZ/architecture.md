# Architektura

> **Verze: v0.2.0-alpha**

[English](../en/architecture.md) | [Deutsch](../de-DE/architecture.md) | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | **Čeština** | [Русский](../ru-RU/architecture.md)

## Duální architektura verzí

Tento projekt nabízí dvě implementační verze, které sdílejí stejný architektonický návrh, ale liší se v úložišti a optimalizaci výkonu:

### SiliconLife.Default (výchozí verze)
- **Zaměření**: výchozí implementace, primárně pro ověření proveditelnosti architektury
- **Režim běhu**: konzolová aplikace
- **Způsob úložiště**: čistě souborový systém JSON úložiště
- **Vhodné scénáře**: vysoké požadavky na bezpečnost dat, omezené paměťové prostředky, malé objemy dat
- **Popis role**: jako referenční implementace pro ověření architektury, poskytuje jednoduchý a spolehlivý způsob běhu, vhodná pro první seznámení s projektem, vývoj a ladění nebo scénáře s prioritou bezpečnosti dat

### SiliconLife.Fast (vysoce výkonná verze)
- **Zaměření**: hlavní produkční verze
- **Režim běhu**: desktopová aplikace (systémová lišta Windows / stavové okno Linux)
- **Způsob úložiště**: SpeedyPack paměťové úložiště + asynchronní dávková perzistence (formát souboru .spk)
- **Vhodné scénáře**: vysoká souběžnost, nízká latence, velké objemy dat
- **Podpora platforem**: Windows/macOS (kompletní funkce včetně systémové lišty), Linux (stavové okno, bez ikony v liště)
- **Vlastnosti**:
  - Windows/macOS běh na pozadí v systémové liště, stavové okno pro sledování v reálném čase; Linux zobrazuje stavové okno přímo
  - SpeedyPack engine + automatická komprimace zaručuje bezpečnost dat
  - Component UI architektura, 27 deklarativních komponent
  - 7 skinových témat, podpora automatického objevování a přepínání
  - Nástroj hot-reload podporuje online aktualizace a restart
  - Linux automaticky otevře prohlížeč pro přístup k Web UI, podpora parametru `--no-tray`
- **Zvýšení výkonu**: latence čtení úložiště snížena 1000x, latence zápisu snížena 15000x
- **Popis role**: hluboce optimalizovaná produkční implementace s funkcemi jako běh na pozadí v systémové liště, SpeedyPack engine + automatická komprimace, první volba pro dlouhodobý provoz a skutečné produkční prostředí

> **Poznámka**: Architektura popsaná v tomto dokumentu platí pro obě verze, s rozdíly pouze v implementaci úložiště. SiliconLife.Default slouží jako referenční implementace pro ověření architektury, SiliconLife.Fast jako hlavní produkční verze.

---

## Základní koncepty

### Křemíková Bytost

Každý AI agent v systému je **Křemíková Bytost** — autonomní entita s vlastní identitou, osobností a schopnostmi. Každou Křemíkovou Bytost pohání **Soubor Duše** (Markdown s výzvami), který definuje její vzorce chování.

### Kurátor Křemíku

**Kurátor Křemíku** je speciální Křemíková Bytost s nejvyššími systémovými oprávněními. Působí jako správce systému:

- Vytváří a spravuje ostatní Křemíkové Bytosti
- Analyzuje požadavky uživatelů a rozděluje je na úkoly
- Distribuuje úkoly příslušným Křemíkovým Bytostem
- Sleduje kvalitu provádění a zpracovává selhání
- Reaguje na zprávy uživatelů pomocí **prioritního plánování** (viz níže)

### Soubor Duše

Markdown soubor (`soul.md`) uložený v datovém adresáři každé Křemíkové Bytosti. Je vkládán jako systémová výzva do každého AI požadavku a definuje osobnost, rozhodovací vzorce a behaviorální omezení bytosti.

---

## Plánování: Spravedlivé plánování časových slotů

### Hlavní Smyčka + Tick Objekty

Systém běží na vyhrazeném vlákně na pozadí s **hodinami řízenou Hlavní Smyčkou**:

```
Hlavní Smyčka (vyhrazené vlákno, Hlídač + Jistič)
  └── Tick Objekt A (priorita=0, interval=100ms)
  └── Tick Objekt B (priorita=1, interval=500ms)
  └── Správce Křemíkových Bytostí (přímo hodinami spouštěný z Hlavní Smyčky)
        └── Runner Křemíkové Bytosti → Křemíková Bytost 1 → Hodinové spuštění → Provedení jednoho kola
        └── Runner Křemíkové Bytosti → Křemíková Bytost 2 → Hodinové spuštění → Provedení jednoho kola
        └── Runner Křemíkové Bytosti → Křemíková Bytost 3 → Hodinové spuštění → Provedení jednoho kola
        └── ...
```

Klíčová architektonická rozhodnutí:

- **Křemíkové Bytosti nedědí z Tick Objektů.** Mají vlastní metodu `Tick()`, kterou volá `SiliconBeingManager` prostřednictvím `SiliconBeingRunner`, místo přímé registrace v Hlavní Smyčce.
- **Správce Křemíkových Bytostí** je přímo hodinami spouštěn z Hlavní Smyčky a funguje jako jediný proxy pro všechny bytosti.
- **Runner Křemíkové Bytosti** obaluje `Tick()` každé bytosti na dočasném vlákně s timeoutem a per-bytost Jističem (3 po sobě jdoucí timeouty → 1 minuta ochlazení).
- Provedení každé bytosti je omezeno na **jedno kolo** AI požadavku + volání nástroje na každé hodinové spuštění, což zajišťuje, že žádná bytost nemůže monopolizovat Hlavní Smyčku.
- **Monitor Výkonu** sleduje dobu provádění hodin pro zajištění pozorovatelnosti.

### Prioritní odpověď Kurátora

Když uživatel pošle zprávu Kurátorovi Křemíku:

1. Aktuální bytost (např. bytost A) dokončí své aktuální kolo — **bez přerušení**.
2. Správce **přeskočí zbývající frontu**.
3. Smyčka **začne znovu od Kurátora**, což mu umožní okamžité provedení.

To zajišťuje rychlou odpověď na interakci uživatele bez narušení probíhajících úkolů.

---

## Architektura komponent

```
┌─────────────────────────────────────────────────────────┐
│                        Hlavní Uzel                      │
│  (Unifikovaný hostitel — sestavuje a spravuje všechny komponenty) │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ Hlavní   │  │ Lokátor      │  │   Konfigurace    │  │
│  │ Smyčka   │  │ Služeb       │  │                  │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │      Správce Křemíkových Bytostí (Tick Objekt)    │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Kurátor   │ │Bytost A │ │Bytost B │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Sdílené služby                       │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Chatovací │  │ Úložiště │  │  Správce        │  │   │
│  │  │ Systém   │  │          │  │  Oprávnění      │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │AI Klient │  │Exekutor  │  │  Správce        │  │   │
│  │  │          │  │          │  │  Nástrojů       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  │  ┌──────────┐ ┌──────────┐                        │   │
│  │  │Zavaděč   │  │Znalostní │                        │   │
│  │  │Zás. Mod. │  │ Síť      │                        │   │
│  │  └──────────┘ └──────────┘                        │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Exekutoři                        │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │  Diskový │  │ Síťový   │  │  Příkazového    │  │   │
│  │  │ Exekutor │  │ Exekutor │  │  Řádku Exekutor │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              Poskytovatelé IM                     │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Konzole  │  │   Web    │  │  Feishu / ...   │  │   │
│  │  │Poskytov. │  │Poskytov. │  │  Poskytovatel   │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## Lokátor Služeb

`ServiceLocator` je vláknově bezpečný singleton registr poskytující přístup ke všem základním službám:

| Vlastnost | Typ | Popis |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | Centrální správce chatovacích relací |
| `IMManager` | `IMManager` | Směrovač poskytovatelů okamžitých zpráv |
| `AuditLogger` | `AuditLogger` | Auditní stopa oprávnění |
| `GlobalAcl` | `GlobalACL` | Globální seznam řízení přístupu |
| `BeingFactory` | `ISiliconBeingFactory` | Továrna pro vytváření bytostí |
| `BeingManager` | `SiliconBeingManager` | Správce životního cyklu aktivních bytostí |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Zavaděč dynamické kompilace |
| `TokenUsageAudit` | `ITokenUsageAudit` | Sledování využití Tokenů |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Reportování využití Tokenů |

Také udržuje registr `PermissionManager` pro každou bytost, klíčovaný podle GUID bytosti.

---

## Chatovací Systém

### Typy relací

Chatovací Systém podporuje tři typy relací prostřednictvím `SessionBase`:

| Typ | Třída | Popis |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | Soukromý rozhovor mezi dvěma účastníky |
| `GroupChat` | `GroupChatSession` | Skupinový chat s více účastníky |
| `Broadcast` | `BroadcastChannel` | Otevřený kanál s pevným ID; bytosti se dynamicky přihlašují a přijímají zprávy až po přihlášení |

### Vysílací Kanál

`BroadcastChannel` je speciální typ relace pro celosystémová oznámení:

- **Pevné ID kanálu** — na rozdíl od `SingleChatSession` a `GroupChatSession` je ID kanálu dobře známá konstanta, nikoli odvozená z GUID členů.
- **Dynamické přihlášení** — bytosti se přihlašují/odhlašují za běhu; přijímají pouze zprávy publikované po jejich přihlášení.
- **Filtrování nevyřízených zpráv** — `GetPendingMessages()` vrací pouze zprávy publikované po čase přihlášení bytosti, které dosud nebyly přečteny.
- **Spravováno Chatovacím Systémem** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

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
| `Role` | `MessageRole` | Role v AI konverzaci (uživatel, asistent, nástroj) |
| `ToolCallId` | `string?` | ID volání nástroje pro zprávy s výsledkem nástroje |
| `ToolCallsJson` | `string?` | Serializovaný JSON volání nástrojů pro zprávy asistenta |
| `Thinking` | `string?` | Řetězec myšlenek AI |
| `PromptTokens` | `int?` | Počet tokenů ve výzvě (vstup) |
| `CompletionTokens` | `int?` | Počet tokenů v doplnění (výstup) |
| `TotalTokens` | `int?` | Celkový počet použitých tokenů (vstup + výstup) |
| `FileMetadata` | `FileMetadata?` | Metadata připojeného souboru (pokud zpráva obsahuje soubor) |

### Fronta chatovacích zpráv

`ChatMessageQueue` je vláknově bezpečný systém fronty zpráv pro správu asynchronního zpracování chatovacích zpráv:

- **Vláknová bezpečnost** - Používá mechanismus zámků pro zajištění bezpečnosti souběžného přístupu
- **Asynchronní zpracování** - Podpora asynchronního zařazování a vyřazování zpráv
- **Řazení zpráv** - Udržuje časové pořadí zpráv
- **Dávkové operace** - Podpora dávkového získávání zpráv

### Metadata souborů

`FileMetadata` spravuje informace o souborech připojených k chatovacím zprávám:

- **Informace o souboru** - Název, velikost, typ, cesta souboru
- **Čas nahrání** - Časové razítko nahrání souboru
- **Nahrávající** - ID uživatele nebo Křemíkové Bytosti, která soubor nahrála

### Správce zrušení streamu

`StreamCancellationManager` poskytuje mechanismus zrušení pro streamované AI odpovědi:

- **Řízení streamu** - Podpora zrušení probíhající streamované AI odpovědi
- **Čištění prostředků** - Správné uvolnění souvisejících prostředků při zrušení
- **Souběžná bezpečnost** - Podpora správy více streamů současně

### Zobrazení historie chatu

Nová funkce zobrazení historie chatu umožňuje uživatelům procházet historické konverzace Křemíkových Bytostí:

- **Seznam relací** - Zobrazuje všechny historické relace
- **Detaily zpráv** - Zobrazení kompletní historie zpráv
- **Časová osa** - Zobrazení zpráv v chronologickém pořadí
- **API podpora** - Poskytuje RESTful API pro získávání dat relací a zpráv

---

## Systém AI klientů

Systém podporuje více AI backendů prostřednictvím rozhraní `IAIClient`:

### OllamaClient

- **Typ**: lokální AI služba
- **Protokol**: nativní Ollama HTTP API (`/api/chat`, `/api/generate`)
- **Funkce**: streamování, volání nástrojů, lokální hostování modelů
- **Konfigurace**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (Alibaba Cloud Bailian)

- **Typ**: cloudová AI služba
- **Protokol**: API kompatibilní s OpenAI (`/compatible-mode/v1/chat/completions`)
- **Autentizace**: Bearer token (API klíč)
- **Funkce**: streamování, volání nástrojů, obsah uvažování (řetězec myšlenek), nasazení ve více regionech
- **Podporované regiony**:
  - `beijing` — Severní Čína 2 (Peking)
  - `virginia` — USA (Virginie)
  - `singapore` — Singapur
  - `hongkong` — Hongkong
  - `frankfurt` — Německo (Frankfurt)
- **Podporované modely** (dynamicky objevované přes API, s náhradním seznamem):
  - **Řada Qwen**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **Uvažování**: qwq-plus
  - **Třetí strany**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **Konfigurace**: `apiKey`, `region`, `model`
- **Objevení modelů**: Za běhu načítá dostupné modely z Bailian API; při selhání sítě používá kurátorský seznam

### VolcengineArkClient (Volcengine Ark)

- **Typ**: cloudová AI služba
- **Protokol**: API kompatibilní s OpenAI
- **Autentizace**: Bearer token (API klíč)
- **Funkce**: podpora streamovacího a nestreamovacího režimu, vestavěné dvouúrovňové řízení rychlosti
  - Vlastní řízení rychlosti: vynucuje minimální interval mezi požadavky
  - Serverové omezení rychlosti: zpracovává chyby 429, exponenciální backoff opakování
- **Konfigurace**: `apiKey`, `endpoint`, `model`
- **Charakteristika**: AI služba ByteDance, podpora více modelů Doubao

### HerdsmanClient

- **Typ**: Lokální/cloudový inferenční engine
- **Protokol**: OpenAI-kompatibilní API
- **Autentizace**: Žádná
- **Funkce**: Streamování, volání nástrojů, reasoning obsah
- **Konfigurace**: `endpoint`, `model`

### LongCatClient (Meituan LongCat)

- **Typ**: Cloudová AI služba
- **Protokol**: OpenAI-kompatibilní API
- **Autentizace**: Bearer token (API klíč)
- **Funkce**: Streamování, volání nástrojů
- **Konfigurace**: `apiKey`, `endpoint`, `model`

### QiniuAIClient (Qiniu Cloud AI)

- **Typ**: Cloudová AI služba
- **Protokol**: OpenAI-kompatibilní API
- **Autentizace**: Bearer token (API klíč)
- **Funkce**: Streamování, volání nástrojů
- **Konfigurace**: `apiKey`, `endpoint`, `model`

### DeepSeekClient

- **Typ**: cloudová AI služba
- **Protokol**: API kompatibilní s OpenAI (`/chat/completions`)
- **Autentizace**: Bearer token (API klíč)
- **Endpoint**: `https://api.deepseek.com`
- **Funkce**: streamování, volání nástrojů, thinking mode (řetězec myšlenek), reasoning effort
- **Kontextové okno**: až 1M tokenů (DeepSeek-V4)
- **Podporované modely**: deepseek-v4-flash, deepseek-v4-pro
- **Konfigurace**: `apiKey`, `endpoint`, `model`, `thinkingEnabled`, `reasoningEffort`, `contextWindowTokens`

### ZhipuClient (GLM)

- **Typ**: cloudová AI služba
- **Protokol**: API kompatibilní s OpenAI (`/chat/completions`)
- **Autentizace**: Bearer token (API klíč)
- **Endpoint**: `https://open.bigmodel.cn/api/paas/v4`
- **Funkce**: streamování, volání nástrojů, thinking mode, vision (podle modelu — glm-4v*, glm-5v*), bezplatné modely
- **Kontextové okno**: až 1M tokenů (GLM-4-Long)
- **Podporované modely**: glm-4-flash (bezplatný), glm-4.7-flash, glm-4-plus, glm-4-long (1M), glm-5, glm-5.1
- **Konfigurace**: `apiKey`, `endpoint`, `model`, `thinkingEnabled`, `contextWindowTokens`

### ErnieClient (Baidu/Qianfan)

- **Typ**: cloudová AI služba
- **Protokol**: Qianfan v2 API kompatibilní s OpenAI (`/chat/completions`)
- **Autentizace**: Bearer token (API klíč, formát `bce-v3/...`)
- **Endpoint**: `https://qianfan.baidubce.com/v2`
- **Funkce**: streamování, volání nástrojů, bezplatné modely (ernie-speed, ernie-tiny)
- **Kontextové okno**: až 131K tokenů
- **Podporované modely**: ernie-5.1 (vlajková loď), ernie-4.0-turbo, ernie-speed-128k (bezplatný), ernie-tiny-8k (bezplatný)
- **Konfigurace**: `apiKey`, `endpoint`, `model`, `contextWindowTokens`

### HunyuanClient (Tencent)

- **Typ**: cloudová AI služba
- **Protokol**: API kompatibilní s OpenAI (`/chat/completions`)
- **Autentizace**: Bearer token (API klíč)
- **Endpoint**: duální — TokenHub (`https://tokenhub.tencentmaas.com/v1`, doporučeno) + Legacy (`https://api.hunyuan.cloud.tencent.com/v1`)
- **Funkce**: streamování, volání nástrojů, thinking mode, automatický výběr endpointu podle modelu
- **Kontextové okno**: až 262K tokenů
- **Podporované modely**: hy3 (TokenHub, doporučeno), hunyuan-lite (bezplatný), hunyuan-turbos-latest, hunyuan-t1-latest
- **Konfigurace**: `apiKey`, `endpoint`, `model`, `thinkingEnabled`, `contextWindowTokens`

### MiniMaxClient

- **Typ**: cloudová AI služba
- **Protokol**: API kompatibilní s OpenAI (`/chat/completions`)
- **Autentizace**: Bearer token (API klíč)
- **Endpoint**: duální — domácí (`https://api.minimaxi.com/v1`) + mezinárodní (`https://api.minimax.io/v1`)
- **Funkce**: streamování, volání nástrojů, thinking mode (adaptivní), reasoning split, multimodální
- **Kontextové okno**: až 1M tokenů (MiniMax-M3)
- **Podporované modely**: MiniMax-M3 (vlajková loď, 1M), MiniMax-M2.7, MiniMax-M2.5, MiniMax-M2
- **Konfigurace**: `apiKey`, `endpoint` (domácí/mezinárodní), `model`, `contextWindowTokens`

### MoonshotClient (Kimi)

- **Typ**: cloudová AI služba
- **Protokol**: API kompatibilní s OpenAI (`/chat/completions`)
- **Autentizace**: Bearer token (API klíč)
- **Endpoint**: `https://api.moonshot.cn/v1`
- **Funkce**: streamování, volání nástrojů, thinking mode, multimodální
- **Kontextové okno**: až 262K tokenů
- **Podporované modely**: kimi-k2.6 (vlajková loď, multimodální), kimi-k2.5, kimi-k2.7-code, moonshot-v1-128k
- **Konfigurace**: `apiKey`, `endpoint`, `model`, `contextWindowTokens`

### SiliconFlowClient

- **Typ**: cloudová AI služba (agregátor)
- **Protokol**: API kompatibilní s OpenAI (`/chat/completions`)
- **Autentizace**: Bearer token (API klíč)
- **Endpoint**: `https://api.siliconflow.cn/v1`
- **Funkce**: streamování, volání nástrojů, reasoning obsah, dynamické objevení modelů přes API
- **Kontextové okno**: až 1M tokenů
- **Popis**: Agreguje 100+ open-source modelů od více dodavatelů (DeepSeek, Qwen, GLM, Kimi, Step, MiniMax atd.)
- **Objevení modelů**: Za běhu načítá dostupné modely z `/models` API; při selhání sítě používá náhradní seznam
- **Konfigurace**: `apiKey`, `endpoint`, `model`, `contextWindowTokens`

### Rozhraní schopností IAIClient

Rozhraní `IAIClient` definuje schopnosti každého AI klienta a umožňuje `Správci Kontextu` přizpůsobit své chování:

| Schopnost | Návratový typ | Popis |
|-----------|--------------|-------|
| `StreamingMode` | `StreamingMode` | Podporovaný streamovací režim (None/Streaming/Reasoning) |
| `SupportsToolCalls` | `bool` | Podpora volání nástrojů |
| `ContextWindowTokens` | `int` | Velikost kontextového okna v tokenech |
| `SupportsVision` | `bool` | Podpora vidění (obrázky) |
| `SupportsAudio` | `bool` | Podpora audia |

### Vzor továrny klientů

Každý typ AI klienta má odpovídající implementaci továrny `IAIClientFactory`:

- `OllamaClientFactory` — vytváří instance OllamaClient
- `DashScopeClientFactory` — vytváří instance DashScopeClient
- `VolcengineArkClientFactory` — vytváří instance VolcengineArkClient
- `HerdsmanClientFactory` — vytváří instance HerdsmanClient
- `LongCatClientFactory` — vytváří instance LongCatClient
- `QiniuAIClientFactory` — vytváří instance QiniuAIClient
- `DeepSeekClientFactory` — vytváří instance DeepSeekClient
- `ZhipuClientFactory` — vytváří instance ZhipuClient
- `ErnieClientFactory` — vytváří instance ErnieClient
- `HunyuanClientFactory` — vytváří instance HunyuanClient
- `MiniMaxClientFactory` — vytváří instance MiniMaxClient
- `MoonshotClientFactory` — vytváří instance MoonshotClient
- `SiliconFlowClientFactory` — vytváří instance SiliconFlowClient

Továrny poskytují:
- `CreateClient(Dictionary<string, object> config)` — instancuje klienta z konfigurace
- `GetConfigKeyOptions(string key, ...)` — vrací dynamické možnosti pro konfigurační klíč (např. dostupné modely, regiony)
- `GetDisplayName()` — lokalizovaný zobrazovaný název typu klienta

### Seznam podpory AI platforem

#### Popis stavů
- ✅ Implementováno
- 🚧 Ve vývoji
- 📋 Plánováno
- 💡 Zvažováno
- ⚠️ Zastaralé

*Poznámka: Vzhledem k síťovému prostředí vývojáře může přístup ke cloudovým AI službám ze zahraničí ve stavu [Zvažováno] vyžadovat použití síťových proxy nástrojů a proces ladění může být nestabilní.*

#### Seznam platforem

| Platforma | Stav | Typ | Popis |
|------|------|------|------|
| Ollama | ✅ | Lokální | Lokální AI služba, podpora lokálního nasazení modelů |
| DashScope (Alibaba Cloud Bailian) | ✅ | Cloud | Alibaba Cloud Bailian AI služba, podpora nasazení ve více regionech |
| Baidu Qianfan (Wenxin Yiyan) | ✅ | Cloud | Baidu Wenxin Yiyan AI služba (ErnieClient), 131K kontext, bezplatné modely |
| Zhipu AI (GLM) | ✅ | Cloud | Zhipu Qingyan AI služba, thinking mode, vision podle modelu, 1M kontext |
| Moonshot (Kimi) | ✅ | Cloud | Moonshot Kimi AI služba, 262K kontext, multimodální |
| Volcengine Ark.Doubao | ✅ | Cloud | ByteDance Doubao AI služba |
| Herdsman | ✅ | Lokální/Cloud | Inferenční engine bez autentizace, kompatibilní s OpenAI API formátem |
| Meituan LongCat | ✅ | Cloud | Vlastní velký model Meituan, kompatibilní s OpenAI API formátem, autentizace API klíčem |
| Qiniu Cloud AI | ✅ | Cloud | Cloudová AI služba Qiniu, autentizace API klíčem |
| DeepSeek (přímé připojení) | ✅ | Cloud | DeepSeek AI služba, thinking mode, 1M kontext |
| 01.AI (Yi) | ⚠️ | Cloud | 01.AI AI služba (Zastaralé: registrace nových uživatelů zastavena) |
| Tencent Hunyuan | ✅ | Cloud | Tencent Hunyuan AI služba, duální endpoint (TokenHub + Legacy), 262K kontext |
| SiliconFlow | ✅ | Cloud | SiliconFlow AI služba, dynamický seznam modelů, 1M kontext |
| MiniMax | ✅ | Cloud | MiniMax AI služba, domácí/mezinárodní endpoint, 1M kontext |
| OpenAI | 💡 | Cloud | OpenAI API služba (řada GPT) |
| Anthropic | 💡 | Cloud | Anthropic Claude AI služba |
| Google DeepMind | 💡 | Cloud | Google Gemini AI služba |
| Mistral AI | 💡 | Cloud | Mistral AI služba |
| Groq | 💡 | Cloud | Groq vysokorychlostní AI inferenční služba |
| Together AI | 💡 | Cloud | Together AI služba open-source modelů |
| xAI | 💡 | Cloud | xAI Grok služba |
| Cohere | 💡 | Cloud | Cohere podniková NLP služba |
| Replicate | 💡 | Cloud | Replicate platforma pro hostování open-source modelů |
| Hugging Face | 💡 | Cloud | Hugging Face open-source AI komunita a platforma modelů |
| Cerebras | 💡 | Cloud | Cerebras AI inferenční optimalizační služba |
| Databricks | 💡 | Cloud | Databricks podniková AI platforma (MosaicML) |
| Perplexity AI | 💡 | Cloud | Perplexity AI vyhledávací Q&A služba |
| NVIDIA NIM | 💡 | Cloud | NVIDIA AI inferenční mikroslužby |

---

## Klíčová architektonická rozhodnutí

### Úložiště jako instance (nikoli statické)

`IStorage` je navrženo jako injektovatelná instance, nikoli jako statický nástroj. To zajišťuje:

- Přímý přístup k souborovému systému — IStorage je interní perzistenční kanál systému, **není** směrován přes exekutor.
- **AI nemůže ovládat IStorage** — exekutor spravuje IO iniciované AI nástroji; IStorage spravuje vlastní interní čtení a zápis dat frameworku. Toto jsou zásadně odlišné oblasti zájmu.
- Lze testovat s mock implementacemi.
- Budoucí podpora různých úložných backendů bez úpravy konzumentů.

### Exekutor jako bezpečnostní hranice

Exekutor je **jediná** cesta pro I/O operace. Nástroje vyžadující přístup k disku, síti nebo příkazovému řádku **musí** projít exekutorem. Tento návrh vynucuje:

- **Nezávislé dispečerské vlákno** pro každý exekutor se zamykáním vláken pro ověřování oprávnění.
- Centralizovanou kontrolu oprávnění — exekutor dotazuje **privátního Správce Oprávnění** bytosti.
- Frontu požadavků s podporou priority a řízením timeoutu.
- Auditní protokolování všech externích operací.
- Izolaci výjimek — selhání jednoho exekutoru neovlivňuje ostatní exekutory.
- Jistič — po sobě jdoucí selhání dočasně zastaví exekutor pro zabránění kaskádovým selháním.

### Správce Kontextu jako lehký objekt

Každé volání `ExecuteOneRound()` vytvoří novou instanci `ContextManager`:

1. Načte Soubor Duše + nedávnou historii chatu.
2. Odešle požadavek AI klientovi.
3. Smyčkově zpracuje volání nástrojů, dokud AI nevrátí čistý text.
4. Perzistuje odpověď do Chatovacího Systému.
5. Uvolní se.

Toto udržuje každé kolo izolované a bezstavové.

### Sebeevoluce prostřednictvím přepsání třídy

Křemíkové Bytosti mohou za běhu přepsat vlastní C# třídy:

1. AI vygeneruje nový kód třídy (musí dědit z `SiliconBeingBase`).
2. **Kontrola referencí při kompilaci** (primární obrana): kompilátor získá pouze povolený seznam sestavení — `System.IO`, `System.Reflection` atd. jsou vyloučeny, takže nebezpečný kód je nemožný na úrovni typů.
3. **Statická analýza za běhu** (sekundární obrana): `SecurityScanner` prohledává kód po úspěšné kompilaci na nebezpečné vzory.
4. Roslyn zkompiluje kód v paměti.
5. Při úspěchu: `SiliconBeingManager.ReplaceBeing()` vymění aktuální instanci, migruje stav a perzistuje šifrovaný kód na disk.
6. Při selhání: nový kód je zahozen, stávající implementace je zachována.

Vlastní implementace `IPermissionCallback` může být také kompilována a injektována prostřednictvím `ReplacePermissionCallback()`, což umožňuje bytostem přizpůsobit vlastní logiku oprávnění.

Kód je na disku uložen šifrovaný pomocí AES-256. Šifrovací klíč je odvozen z GUID bytosti (velkými písmeny) pomocí PBKDF2.

---

## Audit využití Tokenů

`TokenUsageAuditManager` sleduje spotřebu AI tokenů všech bytostí:

- `TokenUsageRecord` — záznam pro každý požadavek (ID bytosti, model, tokeny výzvy, tokeny doplnění, časové razítko)
- `TokenUsageSummary` — agregovaná statistika
- `TokenUsageQuery` — parametry dotazu pro filtrování záznamů
- Perzistence prostřednictvím `ITimeStorage` pro časové řady dotazy
- Přístupné přes Web UI (UsageController) a `TokenAuditTool` (pouze Kurátor)

---

### Kalendářní systém

Systém obsahuje **32 implementací kalendáře**, odvozených z abstraktní třídy `CalendarBase`, pokrývajících hlavní světové kalendářní systémy:

| Kalendář | ID | Popis |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | Buddhistský kalendář (BE), rok + 543 |
| CherokeeCalendar | `cherokee` | Kalendářní systém Cherokee |
| ChineseLunarCalendar | `lunar` | Čínský lunární kalendář s přestupnými měsíci |
| ChineseHistoricalCalendar | `chinese_historical` | Čínský historický kalendář, podpora ganzhi a éry císařů |
| ChulaSakaratCalendar | `chula_sakarat` | Chula Sakarat (CS), rok - 638 |
| CopticCalendar | `coptic` | Koptský kalendář |
| DaiCalendar | `dai` | Dajský kalendář s kompletním lunárním výpočtem |
| DehongDaiCalendar | `dehong_dai` | Dehong dajská varianta kalendáře |
| EthiopianCalendar | `ethiopian` | Etiopský kalendář |
| FrenchRepublicanCalendar | `french_republican` | Francouzský republikánský kalendář |
| GregorianCalendar | `gregorian` | Standardní gregoriánský kalendář |
| HebrewCalendar | `hebrew` | Hebrejský (židovský) kalendář |
| IndianCalendar | `indian` | Indický národní kalendář |
| InuitCalendar | `inuit` | Inuitský kalendářní systém |
| IslamicCalendar | `islamic` | Islámský lunární kalendář |
| JapaneseCalendar | `japanese` | Japonský kalendář éry (Nengo) |
| JavaneseCalendar | `javanese` | Jáveský islámský kalendář |
| JucheCalendar | `juche` | Džuche kalendář (Korea), rok - 1911 |
| JulianCalendar | `julian` | Juliánský kalendář |
| KhmerCalendar | `khmer` | Khmerský kalendář |
| MayanCalendar | `mayan` | Mayský dlouhý počet |
| MongolianCalendar | `mongolian` | Mongolský kalendář |
| PersianCalendar | `persian` | Perský (solární hidžra) kalendář |
| RepublicOfChinaCalendar | `roc` | Čínská republika (ROC) kalendář, rok - 1911 |
| RomanCalendar | `roman` | Římský kalendář |
| SakaCalendar | `saka` | Saka kalendář (Indonésie) |
| SexagenaryCalendar | `sexagenary` | Čínský ganzhi kalendář (sexagenární cyklus) |
| TibetanCalendar | `tibetan` | Tibetský kalendář |
| VietnameseCalendar | `vietnamese` | Vietnamský lunární kalendář (varianta s kočkou zvěrokruhu) |
| VikramSamvatCalendar | `vikram_samvat` | Vikram Samvat kalendář |
| YiCalendar | `yi` | Yiský kalendářní systém |
| ZoroastrianCalendar | `zoroastrian` | Zoroastrijský kalendář |

`CalendarTool` poskytuje operace: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (konverze data mezi kalendáři).

---

## Architektura Web UI

### Systém skinů

Web UI má **zásuvný systém skinů**, který umožňuje kompletní přizpůsobení UI bez změny logiky aplikace:

- **Rozhraní ISkin** — definuje kontrakt pro všechny skiny, včetně:
  - Základní renderovací metody (`RenderHtml`, `RenderError`)
  - 20+ metod UI komponent (tlačítko, vstup, karta, tabulka, odznak, bublina, průběh, záložka atd.)
  - Generování tematického CSS pomocí `CssBuilder`
  - `SkinPreviewInfo` — paleta barev a ikona pro výběr skinu na inicializační stránce

- **Vestavěné skiny** — 7 produkčně připravených skinů:
  - **Admin** — profesionální, na datech zaměřené rozhraní pro správu systému
  - **Chat** — konverzační, na zprávách zaměřený design pro AI interakci
  - **Creative** — umělecké, vizuálně bohaté rozložení pro kreativní pracovní postupy
  - **Dev** — na vývojáře zaměřené rozhraní se zvýrazněním syntaxe
  - **HighContrast** — téma s vysokým kontrastem pro přístupnost
  - **Light** — svěží světlé téma
  - **Minimal** — minimalistické téma

- **Objevení skinů** — `SkinManager` automaticky objevuje a registruje všechny implementace `ISkin` pomocí reflexe

### HTML / CSS / JS buildery

Web UI se zcela vyhýbá souborům šablon a generuje veškerý markup v C#:

- **`H`** — streamovací HTML builder DSL pro konstrukci HTML stromů v kódu
- **`CssBuilder`** — CSS builder s podporou selektorů a mediálních dotazů
- **`JsBuilder`** (`JsSyntax`) — JavaScript builder pro inline skripty

### Systém kontrolerů

Web UI následuje **vzor podobný MVC**, s 24 kontrolery zpracovávajícími různé aspekty:

| Kontroler | Účel |
|------------|---------|
| About | Stránka o projektu a informace |
| Audit | Dashboard auditu využití Tokenů |
| Being | Správa a stav Křemíkových Bytostí |
| Chat | Rozhraní pro chat v reálném čase s SSE |
| ChatHistory | Zobrazení historie chatu, podpora seznamu relací a detailů zpráv |
| CodeBrowser | Prohlížení a úpravy kódu |
| CodeHover | Tooltipy pro kód se zvýrazněním syntaxe |
| Config | Správa konfigurace systému |
| Dashboard | Přehled systému a metriky |
| Executor | Stav a správa exekutorů |
| Help | Systém nápovědy, vícejazyčná podpora |
| Init | Průvodce inicializací při prvním spuštění |
| Knowledge | Vizualizace a dotazování znalostního grafu |
| Log | Prohlížeč systémových protokolů, podpora filtrování podle Křemíkových Bytostí |
| Memory | Prohlížeč dlouhodobé paměti, podpora pokročilého filtrování, statistik a detailního zobrazení |
| Permission | Správa oprávnění |
| PermissionRequest | Fronta žádostí o oprávnění |
| Project | Správa projektů, včetně pracovních poznámek, systému úkolů a oprávnění nástrojů |
| System | Správa systému a monitorování běhu |
| Task | Rozhraní systému úkolů |
| Timer | Správa systému časovačů, včetně historie provádění |
| ToolPermission | Správa oprávnění nástrojů, podpora konfigurace oprávnění na úrovni Křemíkových Bytostí a projektů |
| Usage | Dashboard auditu využití Tokenů s trendy a exportem |
| WorkNote | Správa pracovních poznámek, podpora vyhledávání a generování obsahu |

### Aktualizace v reálném čase

- **SSE (Server-Sent Events)** — aktualizace chatovacích zpráv, stavu bytostí a systémových událostí prostřednictvím `SSEHandler`
- **Žádný WebSocket** — jednodušší architektura využívající SSE pro většinu potřeb v reálném čase
- **Automatické opětovné připojení** — logika opětovného připojení klienta pro odolné spojení

### Lokalizace

Systém podporuje komplexní lokalizaci pro **34 jazykových variant**:
- **Čínština (6 variant)**: zh-CN (zjednodušená), zh-HK (tradiční), zh-SG (Singapur), zh-MO (Macao), zh-TW (Tchaj-wan), zh-MY (Malajsie)
- **Angličtina (10 variant)**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **Španělština (2 varianty)**: es-ES, es-MX
- **Němčina (5 variant)**: de-DE, de-AT, de-CH, de-LU, de-LI
- **Francouzština (3 varianty)**: fr-FR, fr-CA, fr-CH
- **Ostatní (8 variant)**: ja-JP (japonština), ko-KR (korejština), cs-CZ (čeština), it-IT (italština), pl-PL (polština), pt-PT (portugalština), pt-BR (brazilská portugalština), ru-RU (ruština)

Aktivní jazykové prostředí je vybráno pomocí `DefaultConfigData.Language` a řešeno prostřednictvím `LocalizationManager`.

---

### Systém automatizace WebView prohlížeče (nové)

Systém integruje funkci automatizace WebView prohlížeče založenou na **Playwright**:

- **Individuální izolace**: Každá Křemíková Bytost má nezávislou instanci prohlížeče, cookies a úložiště relací, zcela izolované a vzájemně neovlivňující.
- **Headless režim**: Prohlížeč běží v uživateli zcela neviditelném headless režimu, Křemíkové Bytosti operují autonomně na pozadí.
- **WebViewBrowserTool**: Poskytuje kompletní schopnosti ovládání prohlížeče, včetně:
  - Navigace na stránce, klikání, zadávání textu, získávání obsahu stránky
  - Provádění JavaScriptu, pořizování snímků obrazovky, čekání na výskyt prvků
  - Správa stavu prohlížeče a čištění prostředků
- **Bezpečnostní řízení**: Všechny operace prohlížeče musí projít řetězcem ověřování oprávnění, aby se zabránilo přístupu na škodlivé webové stránky.

### Znalostní Síť (nové)

Systém obsahuje vestavěný systém znalostního grafu založený na **trojicové struktuře**:

- **Reprezentace znalostí**: Používá trojicovou strukturu "subjekt-relace-objekt" (např.: Python-is_a-programming_language)
- **KnowledgeTool**: Poskytuje správu celého životního cyklu znalostí:
  - `add`/`query`/`update`/`delete` - základní CRUD operace
  - `search` - fulltextové vyhledávání a shoda klíčových slov
  - `get_path` - objevování asociačních cest mezi dvěma koncepty
  - `validate` - kontrola úplnosti znalostí
  - `stats` - statistická analýza znalostní sítě
- **Perzistentní úložiště**: Trojice znalostí jsou perzistovány do souborového systému, podpora časově indexovaných dotazů.
- **Skóre důvěry**: Každá položka znalostí má skóre důvěry (0-1), podporuje fuzzy shodu a řazení znalostí.
- **Kategorizace štítky**: Podpora přidávání štítků ke znalostem pro snadnější kategorizaci a vyhledávání.

---

## Struktura datového adresáře

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # Soubor Duše Kurátora
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

## Úložný engine SpeedyPack

SiliconLife.Fast používá vlastní úložný engine SpeedyPack (formát .spk), který nahradil předchozí řešení LiteDB a dosáhl extrémní výkon čtení a zápisu.

### Architektonický návrh

```
┌──────────────────────────────────────────────────────────┐
│                    SpeedyPack                             │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────────┐  │
│  │ DirectoryMap  │  │  EntryCache   │  │  WriteQueue   │  │
│  │ (mapování     │  │  (mezipaměť   │  │ (asynchronní  │  │
│  │  adresářů     │  │   záznamů)    │  │  fronta zápisů│  │
│  │  v paměti)    │  │              │  │               │  │
│  └──────┬───────┘  └──────┬───────┘  └───────┬───────┘  │
│         │                  │                   │          │
│  ┌──────▼──────────────────▼───────────────────▼───────┐  │
│  │              PackFileReader / PackFileWriter          │  │
│  │              (Čtečka/Zapisovač balíčkových souborů)   │  │
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
│  │  prostoru)    │  │  komprimace) │                      │
│  └──────────────┘  └──────────────┘                      │
└──────────────────────────────────────────────────────────┘
```

### Základní komponenty

| Komponenta | Popis |
|------|------|
| `SpeedyPack` | Základní třída, kombinuje DirectoryMap, EntryCache a WriteQueue pro nízkolatenční čtení a zápis |
| `DirectoryMap` | Mapování adresářů v paměti, udržuje mapování virtuálních cest na záznamy souborů |
| `EntryCache` | Mezipaměť záznamů, TTL-based mezipaměť naposledy přistupovaných záznamů |
| `WriteQueue` | Asynchronní fronta zápisů, řadí operace zápisu do fronty pro provedení na vlákně na pozadí |
| `FreeList` | Správa volného prostoru, sleduje opětovně použitelný prostor v .spk souborech |
| `PackFileReader` | Čtečka balíčkových souborů, čte data ze .spk souborů |
| `PackFileWriter` | Zapisovač balíčkových souborů, zapisuje data do .spk souborů |
| `SpeedyPackAutoCompactor` | Automatický kompakční časovač, pravidelně komprimuje .spk soubory pro recyklaci volného prostoru |
| `SpeedyPackRegistry` | Procesní singleton správce, zajišťuje, že celá aplikace používá stejnou instanci SpeedyPack |

### Úložné adaptéry

SiliconLife.Fast integruje SpeedyPack do systémových rozhraní prostřednictvím následujících adaptérů:

| Adaptér | Rozhraní | Popis |
|--------|------|-------------|
| `SpeedyStorage` | `IStorage` | Adaptér obecného úložiště klíč-hodnota |
| `SpeedyTimeStorage` | `ITimeStorage` | Adaptér časově indexovaného úložiště |
| `SpeedyWorkNoteStorage` | `IWorkNoteStorage` | Adaptér úložiště pracovních poznámek |

### Konfigurační možnosti

`SpeedyPackOptions` poskytuje následující konfiguraci:

| Možnost | Typ | Výchozí hodnota | Popis |
|------|------|--------|------|
| `CacheTtl` | `TimeSpan` | 5 minut | Doba života položek v mezipaměti |
| `MaxCacheEntries` | `int` | 1000 | Maximální počet položek v mezipaměti |
| `ReadOnly` | `bool` | false | Režim pouze pro čtení |

### Podpora transakcí

SpeedyPack podporuje atomické operace zápisu prostřednictvím rozhraní `IPackTransaction`:

- `SpeedyTransaction` implementuje transakční mechanismus
- Podporuje atomicitu dávkových zápisů
- Při potvrzení transakce jsou všechny operace zápisu buď všechny úspěšné, nebo všechny vráceny zpět

---

## Systém zásuvných modulů

SiliconLife podporuje rozšíření funkcí prostřednictvím systému zásuvných modulů, což umožňuje vývojářům třetích stran přidávat do platformy nové funkce.

### Základní rozhraní

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

### Zavaděč zásuvných modulů

`PluginLoader` je zodpovědný za načítání DLL zásuvných modulů ze zadaného adresáře a provádí bezpečnostní kontroly s deklarací schopností:

1. **Skenování adresáře** — prohledá všechny .dll soubory v adresáři zásuvných modulů
2. **Kontrola schopností** — kontroluje schopnosti deklarované pomocí `[PluginCapability]` a odpovídajícím způsobem uvolňuje bezpečnostní pravidla
3. **Izolované načítání** — používá vlastní `AssemblyLoadContext` pro izolované načítání zásuvných modulů
4. **Správa životního cyklu** — volá metody OnLoad, OnStart, OnStop, OnUnload zásuvného modulu

### Bezpečnostní Sandbox

Zavaděč zásuvných modulů provádí následující bezpečnostní kontroly:

| Kontrola | Popis |
|--------|------|
| Zakázané jmenné prostory | System.IO, System.Net.Http, System.Net.WebSockets, System.Net.Sockets, Microsoft.CodeAnalysis |
| Seznam důvěryhodných sestavení | Google.Protobuf, Newtonsoft.Json, MessagePack, Serilog, Microsoft.Extensions.Logging.Abstractions, Dapper |
| Kontrola zakázaných typů | Skenování nebezpečných typů odkazovaných v zásuvném modulu |
| Kontrola zakázaných členů | Skenování nebezpečných metod volaných v zásuvném modulu |

### Integrace nástrojů

Zásuvné moduly mohou registrovat vlastní nástroje implementací rozhraní `ITool`:

- Metoda `ToolManager.ScanAllPluginAssemblies()` skenuje implementace ITool ve všech načtených zásuvných modulech
- Nástroje zásuvných modulů jsou automaticky integrovány do smyčky volání nástrojů
- Nástroje zásuvných modulů podléhají stejnému systému oprávnění

### Životní cyklus zásuvného modulu

```
Načtení (OnLoad) → Spuštění (OnStart) → Běžící → Zastavení (OnStop) → Uvolnění (OnUnload)
```

---

## Aktivní stavy Křemíkových Bytostí

Křemíkové Bytosti mají následující aktivní stavy:

| Stav | Popis |
|------|------|
| `Idle` | Nečinný stav, čeká na hodinové spuštění |
| `SingleChat` | Probíhá individuální chat |
| `GroupChat` | Probíhá skupinový chat |
| `Task` | Provádění úkolu |
| `Timer` | Provádění časovače |
| `Stopped` | Zastaveno, z důvodu po sobě jdoucích chyb nebo ručního zastavení |

**Mechanismus stavu Stopped**:
- Když Křemíková Bytost zaznamená 10 po sobě jdoucích chyb, automaticky přejde do stavu `Stopped`
- Po přechodu do stavu Stopped bytost již neprovádí žádné úkoly
- Když dorazí nová chatovací zpráva, čítač chyb je resetován a bytost obnoví činnost

Přechody stavů:
```
Idle → SingleChat → Idle (chat dokončen)
Idle → GroupChat → Idle (skupinový chat dokončen)
Idle → Task → Idle (úkol dokončen)
Idle → Timer → Idle (časovač dokončen)
Jakýkoliv → Stopped (10 po sobě jdoucích chyb)
Stopped → Idle (nová chatovací zpráva dorazila nebo ruční restart)
```

---

## Engine pracovních postupů

Engine pracovních postupů je systém stavového stroje založený na šablonách, který řídí procesy spolupráce Křemíkových Bytostí v projektovém prostoru:

### Základní komponenty

| Komponenta | Popis |
|------|------|
| `WorkflowEngine` | Jádro enginu pracovních postupů, spravuje šablony a instance, provádí Tick-řízené přechody stavů |
| `WorkflowTemplate` | Šablona pracovního postupu, definuje sadu stavů a pravidla přechodů |
| `WorkflowInstance` | Instance pracovního postupu, vázaná na konkrétní projekt, sleduje aktuální stav |
| `WorkflowLog` | Protokol pracovního postupu, zaznamenává historii přechodů stavů |

### Mechanismus práce

- **Registrace šablon**: Registrace šablon pracovních postupů pomocí `RegisterTemplate()`, definování stavů a pravidel přechodů
- **Vytváření instancí**: Vytváření instancí ze šablon, vázání na projektový prostor
- **Tick-řízení**: Přechody stavů jsou řízeny mechanismem Tick Hlavní Smyčky
- **Protokolování**: Všechny přechody stavů jsou automaticky zaznamenávány do protokolu

---

### Mechanismus zapomínání paměti

`MemoryFadeService` je služba časového útlumu, která simuluje vlastnost zapomínání biologické paměti:

### Mechanismus práce

- **Pravidelné provádění**: Dědí z `TickObject`, ve výchozím nastavení provádí cyklus útlumu každou hodinu
- **Útlum důležitosti**: Aplikuje algoritmus útlumu na položky paměti každé Křemíkové Bytosti, snižuje skóre důležitosti
- **Automatická archivace**: Paměti s důležitostí pod prahem jsou automaticky archivovány (`ArchiveFadingMemories()`)
- **Sledování statistik**: Zaznamenává počet cyklů útlumu, počet změněných položek stavu a další statistiky

### Průběh útlumu

```
MemoryFadeService.OnTick()
  └── Iterace přes všechny Křemíkové Bytosti
       └── being.Memory.ApplyDecay()      # Aplikace útlumu důležitosti
       └── being.Memory.ArchiveFadingMemories()  # Archivace pamětí s nízkou důležitostí
```

---

## Systém projektového pracovního prostoru

Projektový pracovní prostor je mechanismus správy prostoru podporující spolupráci více Křemíkových Bytostí:

### Základní funkce

- **Životní cyklus projektu**: Vytvoření → Aktivní → Archivace → Zničení
- **Přiřazování rolí**: Podpora přiřazování projektových rolí Křemíkovým Bytostem
- **Izolace oprávnění nástrojů**: Konfigurace oprávnění nástrojů na úrovni projektu, nezávislá na oprávněních na úrovni Křemíkových Bytostí
- **Pracovní poznámky**: Stránkový systém poznámek v projektovém prostoru, podpora generování obsahu a vyhledávání klíčových slov
- **Sledování úkolů**: Správa úkolů na úrovni projektu, podpora vytváření, přiřazování a sledování stavu
- **Integrace pracovních postupů**: Projekty mohou být vázány na šablony pracovních postupů, které řídí procesy spolupráce

### Související nástroje

| Nástroj | Účel |
|------|------|
| `ProjectTool` | Správa projektového prostoru (vytvoření, archivace, zničení, přiřazení rolí) |
| `ProjectTaskTool` | Správa projektových úkolů (vytvoření, přiřazení, aktualizace stavu) |
| `ProjectWorkNoteTool` | Pracovní poznámky projektu (vytvoření, vyhledávání, generování obsahu) |
| `ProjectWorkTool` | Pracovní operace projektu (vytvoření úkolů, skupinový chat, vysílání, dokončení projektu) |
