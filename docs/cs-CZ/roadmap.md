# Roadmapa

[English](../en/roadmap.md) | [中文文档](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | [Español](../es-ES/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Čeština](../cs-CZ/roadmap.md)

## Vodící principy

Každá fáze končí **funkčním, pozorovatelným** systémem. Žádná fáze nevyústí v "hromadu infrastruktury bez ničeho k předvedení".

---

## ~~Fáze 1: Umí chatovat~~ ✅ Dokončeno

**Cíl**: Vstup z konzole → Volání AI → Výstup na konzoli. Minimální ověřitelná jednotka.

| # | Modul | Popis |
|---|--------|-------------|
| 1.1 | Struktura řešení a projektů | Vytvoření `SiliconLifeCollective.sln`, včetně `src/SiliconLife.Core/` (core knihovna) a `src/SiliconLife.Default/` (výchozí implementace + vstupní bod) |
| 1.2 | Konfigurace (minimální) | Singleton + JSON deserializace. Čtení `config.json`. Automatické generování výchozích hodnot pokud chybí |
| 1.3 | Lokalizace (minimální) | Abstraktní třída `LocalizationBase`, implementace `ZhCN`. Přidání `Language` do konfigurace |
| 1.4 | OllamaClient (minimální) | Rozhraní `IAIClient`, HTTP volání lokálního Ollama `/api/chat`. Zatím bez streamování, bez volání nástrojů |
| 1.5 | Konzolový I/O | `while(true) + Console.ReadLine()`, čtení vstupu → volání AI → tisk odpovědi |
| 1.6 | Copyright hlavička | Přidání Apache 2.0 hlavičky do všech C# zdrojových souborů |

**Dodávka**: Konzolový chatovací program komunikující s lokálním Ollama modelem.

**Ověření**: Spustit program, zadat "hello", vidět odpověď AI.

---

## ~~Fáze 2: Má kostru~~ ✅ Dokončeno

**Cíl**: Nahradit "nahou smyčku" framework strukturou. Chování nezměněno.

| # | Modul | Popis |
|---|--------|-------------|
| 2.1 | Storage (minimální) | Rozhraní `IStorage` (Read/Write/Exists/Delete, key-value). Implementace `FileSystemStorage`. Třída instance (ne statická). Přímý přístup k souborovému systému — **AI nemůže ovládat IStorage** |
| 2.2 | Hlavní smyčka + objekty Clock | Nekonečná smyčka, přesné intervaly clocku (`Stopwatch` + `Thread.Sleep`). Prioritní scheduling |
| 2.3 | Standardizace IAIClient | Rozhraní `IAIClientFactory`. Refaktoring OllamaClient pro implementaci standardního rozhraní |
| 2.4 | Migrace konzole | Migrace `while(true)` na clock objekt řízený hlavní smyčkou. Chování shodné s Fází 1 |

**Dodávka**: Hlavní smyčka běží clock, konzolový chat stále funguje.

**Ověření**: Zaregistrovat testovací clock objekt, vytisknout počet clocků za sekundu; konzolový chat stále funguje.

---

## ~~Fáze 3: Má duši~~ ✅ Dokončeno

**Cíl**: První křemíková bytost žije ve frameworku.

| # | Modul | Popis |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Abstraktní základní třída s Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. Abstraktní `Tick()` a `ExecuteOneRound()` |
| 3.2 | Načítání souboru duše | `SoulFileManager`: čtení `soul.md` z datového adresáře bytosti |
| 3.3 | ContextManager (minimální) | Připojení souboru duše + nedávné zprávy → volání AI → získání odpovědi. Zatím bez volání nástrojů, bez persistence |
| 3.4 | ISiliconBeingFactory | Rozhraní továrny pro vytváření instancí bytostí |
| 3.5 | SiliconBeingManager (minimální) | Dědí clock objekt (priorita=0). Iteruje všechny bytosti, sekvenčně volá jejich Tick |
| 3.6 | DefaultSiliconBeing | Implementace standardního chování. Kontrola nepřečtených zpráv → vytvoření ContextManager → ExecuteOneRound → výstup |
| 3.7 | Struktura adresáře bytosti | `DataDirectory/SiliconManager/{GUID}/`, včetně `soul.md` a `state.json` |

**Dodávka**: Křemíková bytost řízená hlavní smyčkou, přijímající konzolový vstup, načítající soubor duše, volající AI.

**Ověření**: Konzolový vstup → tick hlavní smyčky → zpracování bytosti (chování řízené souborem duše) → odpověď AI. Styl odpovědi by se měl lišit od Fáze 1.

---

## ~~Fáze 4: Má paměť~~ ✅ Dokončeno

**Cíl**: Konverzace persistují po restartu.

| # | Modul | Popis |
|---|--------|-------------|
| 4.1 | ChatSystem | Koncept kanálu (dva GUID = jeden kanál). Model zprávy s persistencí. Zatím bez skupinového chatu |
| 4.2 | IIMProvider + IMManager | Rozhraní `IIMProvider`. `ConsoleProvider` jako formální IM kanál. `IMManager` směruje zprávy |
| 4.3 | Vylepšení ContextManager | Stažení historie z chatového systému. Persistování odpovědí AI. Podpora pokračování více kol volání nástrojů |
| 4.4 | Model IMessage | Sjednocený model zprávy sdílený chatovým systémem a IM managerem |

**Dodávka**: Chatový systém s persistující pamětí.

**Ověření**: Chatovat několik kol → ukončit → restartovat → zeptat se "O čem jsme mluvili?" → bytost může odpovědět.

---

## ~~Fáze 5: Umí jednat (systém nástrojů)~~ ✅ Dokončeno

**Cíl**: Křemíkové bytosti mohou provádět akce, nejen chatovat.

| # | Modul | Popis |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | Rozhraní `ITool` s Name, Description, Execute. `ToolResult` obsahuje Success, Message, Data |
| 5.2 | ToolManager | Instance pro každou bytost. Objevování nástrojů založené na reflexi. Podpora atributu `[SiliconManagerOnly]` |
| 5.3 | IAIClient: Podpora volání nástrojů | Parsování AI tool_calls. Smyčka: execute nástroj → odeslat výsledek zpět → AI pokračuje → dokud čistý text |
| 5.4 | Základní třída Executoru | Abstraktní základní třída s nezávislým dispatch threadem, frontou požadavků, kontrolou timeoutu |
| 5.5 | NetworkExecutor | HTTP požadavky přes executor. Timeout, fronta |
| 5.6 | CommandLineExecutor | Shell execution přes executor. Detekce oddělovačů napříč platformami |
| 5.7 | DiskExecutor | Operace se soubory přes executor. Zatím bez kontroly oprávnění (Fáze 6) |
| 5.8–5.12 | Vestavěné nástroje | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Dodávka**: Křemíkové bytosti mohou volat nástroje k provádění akcí.

**Ověření**: Zeptat se "Jaký je dnes den" → CalendarTool odpoví; zeptat se "Zkontrolovat procesy" → SystemTool provede; říct bytosti aby poslala zprávu jiné bytosti → ChatTool funguje.

---

## ~~Fáze 6: Dodržuje pravidla (systém oprávnění)~~ ✅ Dokončeno

**Cíl**: Křemíkové bytosti nemohou přistupovat k citlivým zdrojům bez autorizace.

| # | Modul | Popis |
|---|--------|-------------|
| 6.1 | PermissionManager | Soukromá instance pro každou bytost. Založeno na callbacku, trojrozměrný výsledek (Allowed/Deny/AskUser). Priorita dotazu: HighDeny → HighAllow → Callback. Flag IsCurator |
| 6.2 | Enumerace PermissionType | NetworkAccess, CommandLine, FileAccess, Function, DataAccess |
| 6.3 | DefaultPermissionCallback | Síťový whitelist/blacklist, klasifikace CLI, bezpečnostní pravidla cest souborů |
| 6.4 | GlobalACL | Tabulka pravidel prefix matching, persistovaná do storage |
| 6.5 | UserFrequencyCache | Seznamy HighAllow/HighDeny. Výběr uživatele (ne automatická detekce). Prefix matching, pouze paměť, konfigurovatelná expirace |
| 6.6 | UserAskMechanism (konzole) | Výzva y/n na konzoli při návratu AskUser |
| 6.7 | Integrace oprávnění Executoru | Všechny executory kontrolují oprávnění před provedením |
| 6.8 | Izolace IStorage | IStorage je interní persistence systému — přímý přístup k souborům, **ne** routováno přes executory, **ne** ovladatelné AI. Executory spravují pouze IO iniciované nástroji AI |
| 6.9 | Audit log | Zaznamenává všechna rozhodnutí o oprávněních s časovým razítkem, žadatelem, zdrojem, výsledkem |

**Dodávka**: Výzva oprávnění když bytost zkusí citlivou operaci.

**Ověření**: Říct bytosti aby smazala soubor → konzole zobrazí výzvu oprávnění → zadat `n` → operace zamítnuta. Říct bytosti aby přistoupila k webu na whitelistu → okamžitě povoleno.

---

## ~~Fáze 7: Umí se vyvíjet (dynamická kompilace)~~ ✅ Dokončeno

**Cíl**: Křemíkové bytosti mohou přepisovat svůj vlastní kód.

| # | Modul | Popis |
|---|--------|-------------|
| 7.1 | CodeEncryption | Šifrování/dešifrování AES-256. Odvození klíče PBKDF2 z GUID |
| 7.2 | DynamicCompilationExecutor | Sandbox kompilace v paměti založený na Roslyn. Kontrola referencí assembly při kompilaci (primární obrana: vyloučení System.IO, Reflection atd.) |
| 7.3 | Bezpečnostní scanning | Statická analýza za běhu na vzory nebezpečného kódu (sekundární obrana). Blokování načtení pokud scanning selže |
| 7.4 | Vylepšení životního cyklu bytosti | Načtení: dešifrování → scanning → kompilace → instance. Za běhu: kompilace v paměti → atomická výměna → persistování šifrování |
| 7.5 | SiliconCurator | Abstraktní základní třída Kurátora. IsCurator=true. Nejvyšší oprávnění |
| 7.6 | DefaultCurator | Výchozí implementace Kurátora s vestavěným souborem duše a nástroji správy |
| 7.7 | CuratorTool | Nástroje `[SiliconManagerOnly]`: list_beings, create_being, get_code, reset |
| 7.8 | Přepsání callbacku oprávnění | Bytosti mohou kompilovat vlastní callbacky oprávnění |
| 7.9 | Vylepšení SiliconBeingManager | Metoda Replace (výměna instance za běhu). MigrateState (přenos stavu mezi starou a novou instancí) |

**Dodávka**: Křemíkové bytosti mohou generovat nový kód prostřednictvím AI, kompilovat a nahrazovat sebe.

**Ověření**: Říct bytosti "přidej si novou funkci" → sledovat kompilaci → restart → nová funkce funguje.

---

## ~~Fáze 8: Paměť a plánování~~ ✅ Dokončeno

**Cíl**: Dlouhodobá paměť, správa úkolů, časové spuštění.

| # | Modul | Popis |
|---|--------|-------------|
| 8.1 | FileSystemMemory | Krátkodobé/dlouhodobé segmentované úložiště. Časový útlum. Komprese (sloučení podobných pamětí). Multidimenzionální vyhledávání |
| 8.2 | TaskSystem | Jednorázové + DAG závislé úkoly. Prioritní scheduling. Sledování stavu |
| 8.3 | TimerSystem | Jednorázové alarmy + periodické časovače. Přesnost na milisekundy. Persistence do storage |
| 8.4 | IncompleteDate | Struktura nejasného rozsahu data (např. "duben 2026", "jaro 2026") |
| 8.5–8.7 | Nástroje paměti/úkolů/časovačů | Nástroje pro bytosti k dotazování paměti, správě úkolů, nastavování časovačů |

**Dodávka**: Bytosti si mohou pamatovat klíčové body, vytvářet/sledovat úkoly, nastavovat alarmy.

**Ověření**: Vytvořit úkol → zkontrolovat seznam úkolů → nastavit 1minutový alarm → přijmout notifikaci když vyprší.

---

## ~~Fáze 9: Framework dokončen~~ ✅ Dokončeno

**Cíl**: Sjednocený vstupní bod, spolupráce více bytostí.

| # | Modul | Popis |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | Sjednocený hostitel s builder patternem. Elegní vypnutí (Ctrl+C / SIGTERM) |
| 9.2 | Refaktoring Program.Main | Migrace na pattern CoreHostBuilder |
| 9.3 | Vylepšení SiliconBeingManager | Prioritní odpověď Kurátora. Izolace výjimek. Pravidelná persistence |
| 9.4 | Načítání více bytostí | Načtení více bytostí z datového adresáře. Komunikace mezi bytostmi přes ChatTool |
| 9.5 | Monitorování výkonu | Sledování doby provádění pro každý clock objekt |
| 9.6 | ServiceLocator | Globální service locator s metodami Register/Get |

**Dodávka**: Více bytostí běžících současně, spolupracujících, spravovaných CoreHost.

**Ověření**: Vytvořit dvě bytosti → A pošle zprávu B → B přijme a odpoví → scheduling frameworku bez chyb. Kurátor odpoví prioritně když přijde zpráva uživatele.

---

## ~~Fáze 10: Směřování k Web~~ ✅ Dokončeno

**Cíl**: Migrace z konzole na rozhraní prohlížeče.

| # | Modul | Popis |
|---|--------|-------------|
| 10.1 | Router | Router HTTP požadavků. Směrování sekvenčních parametrů a obsluha statických souborů |
| 10.2 | Základní třída Controller | Kontext požadavku/odpovědi. Podpora odpovědí HTML a JSON |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | Buildery na straně serveru v C#. Žádná závislost na frontendovém frameworku |
| 10.6 | SSE (Server-Sent Events) | Push aktualizace v reálném čase pro chat, stav bytostí a systémové události. Jednodušší než WebSocket s automatickým opětovným připojením klienta |
| 10.7 | WebUIProvider | IM kanál v reálném čase založený na SSE. Nahrazuje konzoli jako hlavní rozhraní |
| 10.8 | Webová bezpečnost | IP blacklist/whitelist. Atribut `[WebCode]`. Dynamické aktualizace |
| 10.9–10.17 | Webové controllery | Chat, Dashboard, Being, Task, Permission, PermissionRequest, Executor, Log, Config, Memory, Timer, Init, About, CodeBrowser, Knowledge, Project, Audit |

**Dodávka**: Kompletní Web UI přístupný z prohlížeče.

**Ověření**: Otevřít prohlížeč → chatovat s bytostí → zobrazit dashboard → spravovat oprávnění → vše funguje.

---

## ~~Fáze 10.5: Přírůstková vylepšení~~ ✅ Dokončeno

**Cíl**: Vylepšení existujícího systému s novými funkcemi objevenými během vývoje.

| # | Modul | Popis |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | Nový typ relace pro systémová oznámení. Pevné ID kanálu, dynamické přihlašování, filtrování čekajících zpráv |
| 10.5.2 | Vylepšení ChatMessage | Pole ToolCallId, ToolCallsJson, Thinking pro kontext AI; PromptTokens, CompletionTokens, TotalTokens pro sledování tokenů; typ zprávy SystemNotification |
| 10.5.3 | TokenUsageAuditManager | Sledování spotřeby tokenů na požadavek napříč všemi bytostmi. Agregované statistiky, dotazy časových řad, persistované úložiště |
| 10.5.4 | TokenAuditTool | Nástroj `[SiliconManagerOnly]` pro Kurátora k dotazování a sumarizaci použití tokenů |
| 10.5.5 | ConfigTool | Nástroj `[SiliconManagerOnly]` pro Kurátora ke čtení a úpravě systémové konfigurace |
| 10.5.6 | AuditController | Web dashboard pro audit použití tokenů s grafy trendů a exportem dat |
| 10.5.7 | Rozšíření kalendářového systému | 32 kalendářních implementací pokrývajících světové kalendářní systémy (Buddhist, Lunar, Islamic, Hebrew, Japanese, Persian, Mayan atd.) |
| 10.5.8 | Vylepšení DiskTool | Nové operace: count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all, list_drives |
| 10.5.9 | Vylepšení SystemTool | Nové operace: find_process (podpora wildcard), resource_usage |
| 10.5.10 | Vylepšení CalendarTool | Nové operace: diff, list_calendars, get_components, get_now_components, convert (konverze mezi kalendáři) |
| 10.5.11 | DashScopeClient | AI klient Alibaba Cloud Bailian, kompatibilní s OpenAI API. Podpora streamování, volání nástrojů, reasoning content |
| 10.5.12 | DashScopeClientFactory | Továrna pro vytváření klientů Bailian. Dynamické objevování modelů prostřednictvím API. Podpora více oblastí (Peking, Virginie, Singapur, Hongkong, Frankfurt) |
| 10.5.13 | Konfigurační systém AI klienta | Konfigurace AI klienta pro každou bytost. Dynamické možnosti klíčů konfigurace (modely, oblasti). Lokalizované zobrazované názvy |
| 10.5.14 | Rozšíření lokalizace | Zjednodušená čínština, tradiční čínština, angličtina a japonština pro možnosti konfigurace Bailian, názvy modelů a názvy oblastí |

**Dodávka**: Vylepšené nástroje, pozorovatelnost, pokrytí kalendářů a podpora více AI backendů.

**Ověření**: Kurátor dotazuje použití tokenů prostřednictvím TokenAuditTool → audit dashboard zobrazuje trendy → CalendarTool převádí datumy mezi 32 kalendářními systémy → přepnutí AI backendu na Bailian → chat s modelem Tongyi Qianwen prostřednictvím cloud API.

---

## Fáze 11: Externí IM integrace

**Cíl**: Připojení k externím messaging platformám pro širší dostupnost uživatelů.

| # | Modul | Popis |
|---|--------|-------------|
| 11.1 | FeishuProvider | Integrace robota Feishu (Lark) s podporou karet |
| 11.2 | WhatsAppProvider | Integrace WhatsApp Business API |
| 11.3 | TelegramProvider | Integrace Telegram Bot API s podporou inline keyboard |
| 11.4 | Vylepšení IMManager | Směrování více providerů, sjednocený formát zpráv, zpracování dotazů oprávnění napříč platformami |

**Dodávka**: Uživatelé mohou interagovat s křemíkovými bytostmi prostřednictvím externích IM platforem.

---

## Fáze 12: Pokročilé funkce

**Cíl**: Volitelné pokročilé funkce pro vylepšené schopnosti.

| # | Modul | Popis |
|---|--------|-------------|
| 12.1 | Knowledge Network | Sdílený knowledge graph s ternární strukturou (subjekt-predikát-objekt) |
| 12.2 | Plugin System | Načítání externích pluginů s bezpečnostními kontrolami a sandboxem |
| 12.3 | Skill Ecosystem | Trh znovu použitelných skillů pro schopnosti bytostí |
