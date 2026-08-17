# Plán vývoje

> **Verze: v0.2.0-alpha**

[English](../en/roadmap.md) | [Deutsch](../de-DE/roadmap.md) | [中文](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | [Español](../es-ES/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | **Čeština** | [Русский](../ru-RU/roadmap.md)

## Plán duálních verzí

### SiliconLife.Default (výchozí verze)
- **Zaměření**: Výchozí implementace, primárně pro ověření proveditelnosti architektury
- **Aktuální stav**: Fáze 1-10.6 dokončeny, systém stabilně běží
- **Popis role**: Jako referenční implementace pro ověření architektury, zajišťuje správnost a proveditelnost základního architektonického návrhu

### SiliconLife.Fast (vysoce výkonná verze)
- **Zaměření**: Hlavní produkční verze
- **Aktuální stav**: Základní architektura portována, úložný engine SpeedyPack a systém zásuvných modulů implementovány
- **Popis role**: Na architektuře ověřené verzí Default staví s hlubokou optimalizací výkonu a vylepšeními pro produkční nasazení, je první volbou pro skutečné nasazení

**Vývojový plán verze Fast**:
- ✅ Fáze 1: Základní struktura projektu a port konfiguračního systému
- ✅ Fáze 2: Port Web UI a kontrolerů
- ✅ Fáze 3: Optimalizace úložného systému (SpeedyPack paměťové úložiště + asynchronní perzistence)
- ✅ Fáze 3.5: Správa SpeedyPack (Avalonia UI aplikace SiliconLife.Speedy.Manager)
- ✅ Fáze 3.6: Systém zásuvných modulů (rozhraní IPlugin, Bezpečnostní Sandbox, izolace AssemblyLoadContext)
- ✅ Fáze 4: Avalonia okenní aplikace (multiplatformní desktopová aplikace, systémová lišta Windows/macOS, stavové okno Linux)

---

## Vodící principy

Každá fáze končí **běžícím, pozorovatelným** systémem. Žádná fáze nevytváří "hromadu infrastruktury bez ničeho k předvedení".

---

## ~~Fáze 1: Lze chatovat~~ ✅ Dokončeno

**Cíl**: Vstup z konzole → volání AI → výstup do konzole. Minimální ověřitelná jednotka.

| # | Modul | Popis |
|---|--------|-------------|
| 1.1 | Řešení a struktura projektu | Vytvoření `SiliconLifeCollective.sln`, obsahující `src/SiliconLife.Core/` (jádrová knihovna) a `src/SiliconLife.Default/` (výchozí implementace + vstupní bod) |
| 1.2 | Konfigurace (minimalistická) | Singleton + JSON deserializace. Čtení `config.json`. Pokud chybí, automaticky generuje výchozí hodnoty |
| 1.3 | Lokalizace (minimalistická) | Abstraktní třída `LocalizationBase`, implementace `ZhCN`. Přidání `Language` do konfigurace |
| 1.4 | OllamaClient (minimalistický) | Rozhraní `IAIClient`, HTTP volání lokálního Ollama `/api/chat`. Zatím bez streamování, bez volání nástrojů |
| 1.5 | Konzolové I/O | `while(true) + Console.ReadLine()`, čtení vstupu → volání AI → tisk odpovědi |
| 1.6 | Hlavička autorských práv | Přidání hlavičky Apache 2.0 ke všem C# zdrojovým souborům |

**Výstup**: Konzolový chatovací program konverzující s lokálním modelem Ollama.

**Ověření**: Spusťte program, zadejte "hello", uvidíte odpověď AI.

---

## ~~Fáze 2: Má kostru~~ ✅ Dokončeno

**Cíl**: Nahrazení "holé smyčky" rámcovou strukturou. Chování se nemění.

| # | Modul | Popis |
|---|--------|-------------|
| 2.1 | Úložiště (minimalistické) | Rozhraní `IStorage` (Read/Write/Exists/Delete, klíč-hodnota). Implementace `FileSystemStorage`. Třída instance (nikoli statická). Přímý přístup k souborovému systému — **AI nemůže ovládat IStorage** |
| 2.2 | Hlavní Smyčka + Tick Objekty | Nekonečná smyčka, přesné hodinové intervaly (`Stopwatch` + `Thread.Sleep`). Plánování podle priority |
| 2.3 | Standardizace IAIClient | Rozhraní `IAIClientFactory`. Refaktoring OllamaClient pro implementaci standardního rozhraní |
| 2.4 | Migrace konzole | Migrace `while(true)` do hodinami řízených Tick Objektů Hlavní Smyčky. Chování stejné jako fáze 1 |

**Výstup**: Hlavní Smyčka běží hodiny, konzolový chat stále funguje.

**Ověření**: Zaregistrujte testovací Tick Objekt, tisknoucí počet tiků každou sekundu; konzolový chat stále funguje.

---

## ~~Fáze 3: Má duši~~ ✅ Dokončeno

**Cíl**: První Křemíková Bytost žije v rámci frameworku.

| # | Modul | Popis |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Abstraktní základní třída, obsahující Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. Abstraktní `Tick()` a `ExecuteOneRound()` |
| 3.2 | Načítání Souboru Duše | `SoulFileManager`: čtení `soul.md` z datového adresáře bytosti |
| 3.3 | ContextManager (minimalistický) | Propojení Souboru Duše + nedávné zprávy → volání AI → získání odpovědi. Zatím bez volání nástrojů, bez perzistence |
| 3.4 | ISiliconBeingFactory | Tovární rozhraní pro vytváření instancí bytostí |
| 3.5 | SiliconBeingManager (minimalistický) | Dědí z Tick Objektu (priorita=0). Iteruje všechny bytosti, postupně volá jejich Tick |
| 3.6 | DefaultSiliconBeing | Standardní implementace chování. Kontrola nepřečtených zpráv → vytvoření ContextManager → ExecuteOneRound → výstup |
| 3.7 | Adresářová struktura bytosti | `DataDirectory/SiliconManager/{GUID}/`, obsahující `soul.md` a `state.json` |

**Výstup**: Křemíková Bytost řízená Hlavní Smyčkou, přijímající konzolový vstup, načítající Soubor Duše, volající AI.

**Ověření**: Konzolový vstup → hodinové spuštění Hlavní Smyčky → zpracování bytostí (s chováním řízeným Souborem Duše) → odpověď AI. Styl odpovědi by měl být odlišný od fáze 1.

---

## ~~Fáze 4: Má paměť~~ ✅ Dokončeno

**Cíl**: Konverzace přetrvají po restartu.

| # | Modul | Popis |
|---|--------|-------------|
| 4.1 | Chatovací Systém | Koncept kanálu (dvě GUID = jeden kanál). Model zpráv s perzistencí. Zatím bez skupinového chatu |
| 4.2 | IIMProvider + IMManager | Rozhraní `IIMProvider`. `ConsoleProvider` jako formální IM kanál. `IMManager` směruje zprávy |
| 4.3 | Vylepšení ContextManager | Načítání historie z Chatovacího Systému. Perzistence AI odpovědí. Podpora vícekolového pokračování volání nástrojů |
| 4.4 | Model IMessage | Sjednocený model zpráv sdílený Chatovacím Systémem a Správcem IM |

**Výstup**: Chatovací systém s perzistentní pamětí.

**Ověření**: Několik kol chatu → ukončení → restart → dotaz "O čem jsme mluvili?" → bytost dokáže odpovědět.

---

## ~~Fáze 5: Může jednat (systém nástrojů)~~ ✅ Dokončeno

**Cíl**: Křemíkové Bytosti mohou provádět operace, ne jen chatovat.

| # | Modul | Popis |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | Rozhraní `ITool`, obsahující Name, Description, Execute. `ToolResult` obsahující Success, Message, Data |
| 5.2 | ToolManager | Instance pro každou bytost. Objevení nástrojů pomocí reflexe. Podpora atributu `[SiliconManagerOnly]` |
| 5.3 | IAIClient: Podpora volání nástrojů | Parsování AI tool_calls. Smyčka: provedení nástroje → odeslání výsledku zpět → AI pokračuje → dokud čistý text |
| 5.4 | Základní třída exekutoru | Abstraktní základní třída s nezávislým dispečerským vláknem, frontou požadavků, řízením timeoutu |
| 5.5 | NetworkExecutor | HTTP požadavky přes exekutor. Timeout, řazení do fronty |
| 5.6 | CommandLineExecutor | Spouštění shell příkazů přes exekutor. Detekce oddělovačů napříč platformami |
| 5.7 | DiskExecutor | Operace se soubory přes exekutor. Zatím bez kontroly oprávnění (fáze 6) |
| 5.8–5.12 | Vestavěné nástroje | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Výstup**: Křemíkové Bytosti mohou volat nástroje k provádění operací.

**Ověření**: Zeptejte se "Jaký je dnes den?" → CalendarTool odpoví; zeptejte se "Zkontroluj procesy" → SystemTool provede; řekněte bytosti, aby poslala zprávu jiné bytosti → ChatTool funguje.

---

## ~~Fáze 6: Dodržuje pravidla (systém oprávnění)~~ ✅ Dokončeno

**Cíl**: Křemíkové Bytosti nemohou přistupovat k citlivým prostředkům bez autorizace.

| # | Modul | Popis |
|---|--------|-------------|
| 6.1 | PermissionManager | Privátní instance pro každou bytost. Na základě zpětného volání, trojstavový výsledek (Allowed/Denied/AskUser). Priorita dotazů: HighDeny → HighAllow → Callback. Příznak IsCurator |
| 6.2 | Enum PermissionType | NetworkAccess, CommandLine, FileAccess, Function, DataAccess |
| 6.3 | DefaultPermissionCallback | Whitelist/blacklist sítě, klasifikace CLI, bezpečnostní pravidla cest souborů |
| 6.4 | GlobalACL | Tabulka pravidel se shodou předpony, perzistentní v úložišti |
| 6.5 | UserFrequencyCache | Seznamy HighAllow/HighDeny. Výběr uživatelem (nikoli automatická detekce). Shoda předpony, pouze v paměti, konfigurovatelná expirace |
| 6.6 | UserAskMechanism (konzole) | Konzolový dotaz y/n při návratu AskUser |
| 6.7 | Integrace oprávnění exekutorů | Všichni exekutoři kontrolují oprávnění před provedením |
| 6.8 | Vysvětlení izolace IStorage | IStorage je interní perzistence systému — přímý přístup k souborům, **není** směrován přes exekutor, **není** kontrolovatelný AI. Exekutoři spravují pouze IO iniciované AI nástroji |
| 6.9 | Auditní protokol | Záznam všech rozhodnutí o oprávněních s časovým razítkem, žadatelem, prostředkem a výsledkem |

**Výstup**: Výzva k oprávnění, když bytost zkouší citlivou operaci.

**Ověření**: Řekněte bytosti smazat soubor → konzole zobrazí výzvu k oprávnění → zadejte `n` → operace zamítnuta. Řekněte bytosti přistoupit na whitelistovaný web → okamžitě povoleno.

---

## ~~Fáze 7: Může se vyvíjet (dynamická kompilace)~~ ✅ Dokončeno

**Cíl**: Křemíkové Bytosti mohou přepsat svůj vlastní kód.

| # | Modul | Popis |
|---|--------|-------------|
| 7.1 | CodeEncryption | Šifrování/dešifrování AES-256. Odvození klíče PBKDF2 z GUID |
| 7.2 | DynamicCompilationExecutor | Roslyn kompilační sandbox v paměti. Kontrola referencí sestavení při kompilaci (primární obrana: vyloučení System.IO, Reflection atd.) |
| 7.3 | Bezpečnostní skener | Statická analýza za běhu pro nebezpečné vzory kódu (sekundární obrana). Pokud skenování selže, načtení je zablokováno |
| 7.4 | Vylepšení životního cyklu bytosti | Načtení: dešifrování → skenování → kompilace → instanciace. Za běhu: kompilace v paměti → atomická výměna → perzistence šifrovaného kódu |
| 7.5 | SiliconCurator | Abstraktní základní třída Kurátora. IsCurator=true. Nejvyšší oprávnění |
| 7.6 | DefaultCurator | Výchozí implementace Kurátora s vestavěným Souborem Duše a nástroji správy |
| 7.7 | CuratorTool | Nástroj `[SiliconManagerOnly]`: list_beings, create_being, get_code, reset |
| 7.8 | Přepsání zpětného volání oprávnění | Bytosti mohou kompilovat vlastní zpětné volání oprávnění |
| 7.9 | Vylepšení SiliconBeingManager | Metoda Replace (výměna instance za běhu). MigrateState (přenos stavu mezi starou a novou instancí) |

**Výstup**: Křemíkové Bytosti mohou generovat nový kód pomocí AI, kompilovat a nahradit sebe sama.

**Ověření**: Řekněte bytosti "přidej si novou funkci" → sledujte kompilaci → restart → nová funkce funguje.

---

## ~~Fáze 8: Paměť a plánování~~ ✅ Dokončeno

**Cíl**: Dlouhodobá paměť, správa úkolů, časované spouštění.

| # | Modul | Popis |
|---|--------|-------------|
| 8.1 | FileSystemMemory | Krátkodobé/dlouhodobé segmentované úložiště. Časový útlum. Komprese (slučování podobných pamětí). Multidimenzionální vyhledávání |
| 8.2 | TaskSystem | Jednorázové + DAG závislostní úkoly. Plánování podle priority. Sledování stavu |
| 8.3 | TimerSystem | Jednorázové budíky + periodické časovače. Milisekundová přesnost. Perzistence do úložiště |
| 8.4 | IncompleteDate | Struktura fuzzy rozsahu dat (např. "duben 2026", "jaro 2026") |
| 8.5–8.7 | Nástroje paměti/úkolů/časovačů | Nástroje pro dotazování paměti, správu úkolů a nastavení časovačů bytostí |

**Výstup**: Bytosti mohou pamatovat klíčové body, vytvářet/sledovat úkoly, nastavovat budíky.

**Ověření**: Vytvořte úkol → zkontrolujte seznam úkolů → nastavte 1minutový budík → po uplynutí času přijměte oznámení.

---

## ~~Fáze 9: Framework dokončen~~ ✅ Dokončeno

**Cíl**: Sjednocený vstupní bod, spolupráce více bytostí.

| # | Modul | Popis |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | Sjednocený hostitel používající vzor builder. Elegantní vypnutí (Ctrl+C / SIGTERM) |
| 9.2 | Refaktoring Program.Main | Migrace do vzoru CoreHostBuilder |
| 9.3 | Vylepšení SiliconBeingManager | Prioritní odpověď Kurátora. Izolace výjimek. Pravidelná perzistence |
| 9.4 | Načítání více bytostí | Načítání více bytostí z datového adresáře. Komunikace mezi bytostmi prostřednictvím ChatTool |
| 9.5 | Monitorování výkonu | Sledování doby provádění každého Tick Objektu |
| 9.6 | ServiceLocator | Globální lokátor služeb s metodami Register/Get |

**Výstup**: Více bytostí běžících současně, spolupracujících, spravovaných CoreHost.

**Ověření**: Vytvořte dvě bytosti → A pošle zprávu B → B přijme a odpoví → plánování frameworku bez chyb. Kurátor odpovídá prioritně, když dorazí zpráva uživatele.

---

## ~~Fáze 10: Cesta na Web~~ ✅ Dokončeno

**Cíl**: Migrace z konzole do rozhraní prohlížeče.

| # | Modul | Popis |
|---|--------|-------------|
| 10.1 | Router | Směrovač HTTP požadavků. Sériové parametrické směrování a obsluha statických souborů |
| 10.2 | Základní třída Controller | Kontext požadavku/odpovědi. Podpora HTML a JSON odpovědí |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | C# server-side buildery. Nulová závislost na frontendovém frameworku |
| 10.6 | SSE (Server-Sent Events) | Push aktualizace v reálném čase pro chat, stav bytostí a systémové události. Jednodušší než WebSocket, s automatickým opětovným připojením klienta |
| 10.7 | WebUIProvider | IM kanál v reálném čase založený na SSE. Nahrazuje konzoli jako hlavní rozhraní |
| 10.8 | Webová bezpečnost | IP blacklist/whitelist. Atribut `[WebCode]`. Dynamické aktualizace |
| 10.9–10.17 | Webové kontrolery | Chat, řídicí panel, bytosti, úkoly, oprávnění, žádosti o oprávnění, exekutoři, protokoly, konfigurace, paměť, časovače, inicializace, o projektu, prohlížeč kódu, znalosti, projekty, audit |

**Výstup**: Kompletní Web UI přístupné z prohlížeče.

**Ověření**: Otevřete prohlížeč → chatujte s bytostí → zobrazte řídicí panel → spravujte oprávnění → vše funguje.

---

## ~~Fáze 10.5: Inkrementální vylepšení~~ ✅ Dokončeno

**Cíl**: Vylepšení stávajícího systému o nové funkce objevené během vývoje.

| # | Modul | Popis |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | Nový typ relace pro celosystémová oznámení. Pevné ID kanálu, dynamické přihlášení, filtrování nevyřízených zpráv |
| 10.5.2 | Vylepšení ChatMessage | Pole ToolCallId, ToolCallsJson, Thinking pro AI kontext; PromptTokens, CompletionTokens, TotalTokens pro sledování tokenů; typ zprávy SystemNotification |
| 10.5.3 | TokenUsageAuditManager | Sledování spotřeby tokenů pro každý požadavek napříč všemi bytostmi. Agregované statistiky, časové řady dotazy, perzistentní úložiště |
| 10.5.4 | TokenAuditTool | Nástroj `[SiliconManagerOnly]` pro Kurátora k dotazování a shrnování využití tokenů |
| 10.5.5 | ConfigTool | Nástroj `[SiliconManagerOnly]` pro Kurátora ke čtení a úpravě systémové konfigurace |
| 10.5.6 | AuditController | Webový dashboard pro audit využití tokenů s grafy trendů a exportem dat |
| 10.5.7 | Rozšíření kalendářního systému | 32 implementací kalendáře pokrývajících světové kalendářní systémy (buddhistský, lunární, islámský, hebrejský, japonský, perský, mayský atd.) |
| 10.5.8 | Vylepšení DiskTool | Nové operace: count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all, list_drives |
| 10.5.9 | Vylepšení SystemTool | Nové operace: find_process (podpora zástupných znaků), resource_usage |
| 10.5.10 | Vylepšení CalendarTool | Nové operace: diff, list_calendars, get_components, get_now_components, convert (konverze mezi kalendáři) |
| 10.5.11 | DashScopeClient | AI klient Alibaba Cloud Bailian, kompatibilní s OpenAI API. Podpora streamování, volání nástrojů, obsah uvažování |
| 10.5.12 | DashScopeClientFactory | Továrna pro vytváření klientů Bailian. Dynamické objevení modelů přes API. Podpora více regionů (Peking, Virginie, Singapur, Hongkong, Frankfurt) |
| 10.5.13 | Konfigurační systém AI klientů | Konfigurace AI klientů pro každou bytost. Dynamické možnosti konfiguračních klíčů (modely, regiony). Lokalizované zobrazované názvy |
| 10.5.14 | Rozšíření lokalizace | Lokalizace zjednodušené čínštiny, tradiční čínštiny, angličtiny a japonštiny pro konfigurační možnosti Bailian, názvy modelů a názvy regionů |
| 10.5.15 | DeepSeekClient | AI klient DeepSeek, thinking mode, reasoning effort, až 1M kontextové okno, endpoint `https://api.deepseek.com` |
| 10.5.16 | ZhipuClient (GLM) | AI klient Zhipu AI, thinking mode, vision podle modelu, bezplatné modely, až 1M kontext, endpoint `https://open.bigmodel.cn/api/paas/v4` |
| 10.5.17 | ErnieClient (Baidu/Qianfan) | AI klient Baidu Qianfan v2, bezplatné modely, až 131K kontext, endpoint `https://qianfan.baidubce.com/v2` |
| 10.5.18 | HunyuanClient (Tencent) | AI klient Tencent Hunyuan, duální endpoint (TokenHub + Legacy), thinking mode, až 262K kontext |
| 10.5.19 | MiniMaxClient | AI klient MiniMax, domácí/mezinárodní endpoint, adaptivní thinking, multimodální, až 1M kontext |
| 10.5.20 | MoonshotClient (Kimi) | AI klient Moonshot/Kimi, thinking mode, multimodální, až 262K kontext, endpoint `https://api.moonshot.cn/v1` |
| 10.5.21 | SiliconFlowClient | AI klient SiliconFlow, agregátor 100+ open-source modelů, dynamické objevení modelů, až 1M kontext, endpoint `https://api.siliconflow.cn/v1` |

**Výstup**: Vylepšené nástroje, pozorovatelnost, pokrytí kalendářů a podpora více AI backendů.

**Ověření**: Kurátor se dotazuje na využití tokenů přes TokenAuditTool → auditní dashboard zobrazuje trendy → CalendarTool konvertuje data mezi 32 kalendářními systémy → přepnutí AI backendu na Bailian → chatování s modelem Qwen přes cloudové API.

---

## ~~Fáze 10.6: Dokončování a optimalizace~~ ✅ Dokončeno

**Cíl**: Dokončení systémových funkcí, přidání nových vlastností, optimalizace uživatelské zkušenosti.

| # | Modul | Popis |
|---|--------|-------------|
| 10.6.1 | WebViewBrowserTool | Nástroj automatizace prohlížeče založený na Playwright, podpora headless režimu, individuální izolace, kompletní podpora JS/CSS |
| 10.6.2 | HelpTool | Nástroj systému dokumentace nápovědy, podpora vícejazyčného dotazování a zobrazování dokumentace |
| 10.6.3 | ProjectWorkNoteTool | Nástroj projektových pracovních poznámek, podpora pracovních záznamů a správy v dimenzi projektu |
| 10.6.4 | ProjectTaskTool | Nástroj správy projektových úkolů, podpora přiřazování úkolů a sledování postupu |
| 10.6.5 | KnowledgeTool | Nástroj znalostní sítě, podpora CRUD operací trojic znalostí a objevování cest |
| 10.6.6 | ChatHistoryController | Kontroler zobrazení historie chatu, podpora seznamu relací a detailů zpráv |
| 10.6.7 | CodeHoverController | Kontroler plovoucích tipů kódu, podpora zvýraznění syntaxe a tipů pro kód |
| 10.6.8 | WorkNoteController | Kontroler správy pracovních poznámek, podpora vyhledávání a generování obsahu |
| 10.6.9 | TimerExecutionHistory | Funkce historie provádění časovačů, záznam a prohlížení historie spouštění časovačů |
| 10.6.10 | Rozšíření lokalizace | Přidána podpora lokalizace češtiny (cs-CZ), celkem 21 jazykových variant |
| 10.6.11 | Optimalizace Web UI | Podpora nahrávání souborů, indikátor načítání, optimalizace renderování volání nástrojů, oprava modálního okna pracovních poznámek |
| 10.6.12 | Vylepšení správy paměti | Pokročilé filtrování, statistiky, detailní zobrazení, optimalizace kompresního algoritmu |
| 10.6.13 | Refaktoring protokolového systému | Separace systémových/bytostních protokolů, API pro čtení protokolů, filtr bytostí |
| 10.6.14 | Vylepšení systému oprávnění | Předběžná validace kompilace zpětného volání oprávnění, validace referencí sestavení, whitelist wttr.in služby počasí |

**Výstup**: Kompletní automatizace WebView prohlížeče, systém dokumentace nápovědy, projektový pracovní prostor, znalostní síť, zobrazení historie chatu a další vylepšení.

**Ověření**: Křemíkové Bytosti mohou operovat prohlížeč přes WebViewBrowserTool → získat dokumentaci nápovědy přes HelpTool → spravovat projektové pracovní poznámky a úkoly → dotazovat znalostní síť → zobrazit historii chatu.

---

## ~~Fáze 10.7: Projektová spolupráce a pracovní postupy~~ ✅ Dokončeno

**Cíl**: Přidání projektového pracovního prostoru, enginu pracovních postupů, mechanismu zapomínání paměti a systému oprávnění nástrojů.

| # | Modul | Popis |
|---|--------|-------------|
| 10.7.1 | Správa projektových rolí | ProjectTool přidává operace assign_role, remove_role, list_roles |
| 10.7.2 | Engine pracovních postupů | Jádrový engine WorkflowEngine, podpora definice šablon, přechodů stavů, Tick-řízené exekuce |
| 10.7.3 | Šablony pracovních postupů | Základní třída WorkflowTemplate, definice sady stavů a pravidel přechodů |
| 10.7.4 | Instance pracovních postupů | Správa instancí WorkflowInstance, vázání na konkrétní projekt, sledování aktuálního stavu |
| 10.7.5 | Protokoly pracovních postupů | WorkflowLog zaznamenává historii přechodů stavů |
| 10.7.6 | Mechanismus zapomínání paměti | MemoryFadeService služba časového útlumu, každou hodinu automaticky aplikuje útlum důležitosti a archivaci pamětí |
| 10.7.7 | Systém oprávnění nástrojů | Dvouúrovňová oprávnění nástrojů (úroveň Křemíkové Bytosti + úroveň projektu), šablony oprávnění, granularita operací |
| 10.7.8 | ToolPermissionController | Webový kontroler správy oprávnění nástrojů |
| 10.7.9 | ProjectWorkTool | Nástroj projektových pracovních operací ([SiliconManagerOnly], [ToolScenario(Project)]) |
| 10.7.10 | Systém scénářů nástrojů | ToolScenarioAttribute a ChatOnlyAttribute, podpora filtrování scénářů Chat/Task/Timer/MemoryCompression/Project |
| 10.7.11 | Rozšíření lokalizace | Přidána lokalizace ruštiny, portugalštiny, italštiny, nizozemštiny, polštiny, švédštiny, celkem 34 jazykových variant |

**Výstup**: Kompletní systém projektové spolupráce, engine pracovních postupů, mechanismus zapomínání paměti a správa oprávnění nástrojů.

**Ověření**: Vytvoření projektu → přiřazení rolí → vázání šablony pracovního postupu → bytosti spolupracují v projektovém prostoru → automatický útlum a archivace pamětí → izolace oprávnění nástrojů funguje.

---

## Fáze 11: Integrace externího IM

**Cíl**: Připojení k externím platformám okamžitých zpráv pro širší dostupnost pro uživatele.

| # | Modul | Popis |
|---|--------|-------------|
| 11.1 | FeishuProvider | Integrace robota Feishu (Lark), podpora karet |
| 11.2 | WhatsAppProvider | Integrace WhatsApp Business API |
| 11.3 | TelegramProvider | Integrace Telegram Bot API, podpora inline klávesnic |
| 11.4 | Vylepšení IMManager | Směrování více poskytovatelů, sjednocený formát zpráv, zpracování dotazů na oprávnění napříč platformami |

**Výstup**: Uživatelé mohou interagovat s Křemíkovými Bytostmi přes externí platformy okamžitých zpráv (Feishu / WeChat Enterprise / DingTalk), více platforem může být povoleno současně.

---

## Fáze 11.5: Systém dovedností a integrace MCP

**Cíl**: Znovupoužitelná abstraktní vrstva schopností a přístup k ekosystému externích nástrojů.

| # | Modul | Popis |
|---|--------|-------------|
| 11.5.1 | ~~Systém dovedností~~ ✅ Dokončeno | Znovupoužitelná abstraktní vrstva orchestrace nástrojů + šablon výzev (SkillManager, duální spouštěcí režim, hot-reload, verzování archivace, AI doplňování metadat) |
| 11.5.2 | ~~Integrace MCP~~ ✅ Dokončeno | Přístup k nástrojům externích MCP serverů (duální transport stdio/http, injektáž názvů `mcp_{serverId}_{toolName}`, Webová stránka správy, integrace matice oprávnění) |

**Výstup**: Stránka správy dovedností (/skill), stránka správy MCP (/mcp), vestavěné nástroje `skill` a `mcp`, dokumentace dovedností/MCP.

---

## Fáze 12: Pokročilé funkce

**Cíl**: Volitelné pokročilé funkce pro rozšíření možností.

| # | Modul | Popis |
|---|--------|-------------|
| 12.1 | ~~Znalostní síť~~ ✅ Dokončeno | Znalostní graf založený na trojicové struktuře (subjekt-predikát-objekt), podpora CRUD, objevování cest, pokročilé dotazy a průchod grafem |
| 12.2 | ~~Systém zásuvných modulů~~ ✅ Dokončeno | Načítání externích zásuvných modulů s bezpečnostními kontrolami a sandboxem (rozhraní IPlugin, PluginLoader, izolace AssemblyLoadContext) |
| 12.3 | Ekosystém dovedností | Tržiště opakovaně použitelných dovedností pro schopnosti bytostí (jádro systému dovedností již implementováno, viz Fáze 11.5; zbývá: tržiště zásuvných modulů, distribuce balíčků dovedností) |
