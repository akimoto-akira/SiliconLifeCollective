# Roadmap

> **Verze: v0.1.0-alpha**

[English](../en/roadmap.md) | [中文](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | [Español](../es-ES/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Deutsch](../de-DE/roadmap.md) | **Čeština**

## Guiding Principles

Každá fáze končí **fungujícím, pozorovatelným** systémem. Žádná fáze nevytváří "spoustu infrastruktury s ničím k předvedení."

---

## ~~Fáze 1: Umí Chatovat~~ ✅ Dokončeno

**Cíl**: Konzolový vstup → Volání AI → Konzolový výstup. Minimální ověřitelná jednotka.

| # | Modul | Popis |
|---|--------|-------------|
| 1.1 | Struktura řešení a projektů | Vytvoření `SiliconLifeCollective.sln`, obsahující `src/SiliconLife.Core/` (core knihovna) a `src/SiliconLife.Default/` (výchozí implementace + vstupní bod) |
| 1.2 | Konfigurace (Minimální) | Singleton + JSON deserializace. Čte `config.json`. Automaticky generuje výchozí hodnoty, pokud chybí |
| 1.3 | Lokalizace (Minimální) | Abstraktní třída `LocalizationBase`, implementace `ZhCN`. Přidání `Language` do konfigurace |
| 1.4 | OllamaClient (Minimální) | Rozhraní `IAIClient`, HTTP volání na lokální Ollama `/api/chat`. Zatím žádné streamování, žádná volání nástrojů |
| 1.5 | Konzolový I/O | `while(true) + Console.ReadLine()`, čtení vstupu → volání AI → tisk odpovědi |
| 1.6 | Autorská záhlaví | Přidání Apache 2.0 záhlaví do všech C# zdrojových souborů |

**Výsledek**: Konzolový chatovací program komunikující s lokálním modelem Ollama.

**Ověření**: Spusťte program, zadejte "hello", uvidíte odpověď AI.

---

## ~~Fáze 2: Má Kostru~~ ✅ Dokončeno

**Cíl**: Nahradit "nahou smyčku" framework strukturou. Chování se nemění.

| # | Modul | Popis |
|---|--------|-------------|
| 2.1 | Úložiště (Minimální) | Rozhraní `IStorage` (Read/Write/Exists/Delete, klíč-hodnota). Implementace `FileSystemStorage`. Třída instance (ne statická). Přímý přístup k souborovému systému — **AI nemůže ovládat IStorage** |
| 2.2 | Hlavní smyčka + Objekt hodin | Nekonečná smyčka, přesný interval hodin (`Stopwatch` + `Thread.Sleep`). Prioritní plánování |
| 2.3 | Standardizace IAIClient | Rozhraní `IAIClientFactory`. Refaktorizace OllamaClient pro implementaci standardního rozhraní |
| 2.4 | Migrace konzole | Migrace `while(true)` do hlavní smyčky řízené objektem hodin. Chování stejné jako Fáze 1 |

**Výsledek**: Hlavní smyčka spouští hodiny, konzolový chat stále funguje.

**Ověření**: Zaregistrujte testovací objekt hodin, každou sekundu tiskněte počet hodin; konzolový chat stále funguje.

---

## ~~Fáze 3: Má Duši~~ ✅ Dokončeno

**Cíl**: První silikonová bytost žije ve frameworku.

| # | Modul | Popis |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Abstraktní základní třída s Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. Abstraktní metody `Tick()` a `ExecuteOneRound()` |
| 3.2 | Načítání souboru duše | `SoulFileManager`: čtení `soul.md` z datového adresáře bytosti |
| 3.3 | ContextManager (Minimální) | Propojení souboru duše + nedávné zprávy → volání AI → získání odpovědi. Zatím žádná volání nástrojů, žádná perzistence |
| 3.4 | ISiliconBeingFactory | Rozhraní factory pro vytváření instancí bytostí |
| 3.5 | SiliconBeingManager (Minimální) | Dědí objekt hodin (priorita=0). Iteruje všechny bytosti, postupně volá jejich Tick |
| 3.6 | DefaultSiliconBeing | Implementace standardního chování. Kontrola nepřečtených zpráv → vytvoření ContextManager → ExecuteOneRound → výstup |
| 3.7 | Struktura adresáře bytostí | `DataDirectory/SiliconManager/{GUID}/`, obsahující `soul.md` a `state.json` |

**Výsledek**: Silikonová bytost řízená hlavní smyčkou, přijímající konzolový vstup, načítající soubor duše, volající AI.

**Ověření**: Konzolový vstup → Hlavní smyčka spouští hodiny → Bytost zpracovává (s chováním řízeným souborem duše) → Odpověď AI. Styl odpovědi by se měl lišit od Fáze 1.

---

## ~~Fáze 4: Má Paměť~~ ✅ Dokončeno

**Cíl**: Konverzace přetrvávají po restartu.

| # | Modul | Popis |
|---|--------|-------------|
| 4.1 | ChatSystem | Koncept kanálů (dva GUID = jeden kanál). Model zpráv s perzistencí. Zatím žádné skupinové chaty |
| 4.2 | IIMProvider + IMManager | Rozhraní `IIMProvider`. `ConsoleProvider` jako formální IM kanál. `IMManager` směruje zprávy |
| 4.3 | Vylepšení ContextManager | Stažení historie z chatovacího systému. Perzistence odpovědí AI. Podpora více kol volání nástrojů |
| 4.4 | Model IMessage | Sjednocený model zpráv sdílený chatovacím systémem a IM managerem |

**Výsledek**: Chatovací systém s perzistentní pamětí.

**Ověření**: Chatujte několik kol → Ukončete → Restartujte → Zeptejte se "O čem jsme mluvili?" → Bytost může odpovědět.

---

## ~~Fáze 5: Může Jednat (Systém Nástrojů)~~ ✅ Dokončeno

**Cíl**: Silikonové bytosti mohou provádět akce, nejen chatovat.

| # | Modul | Popis |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | Rozhraní `ITool` s Name, Description, Execute. `ToolResult` s Success, Message, Data |
| 5.2 | ToolManager | Instance na bytost. Objevování nástrojů založené na reflexi. Podpora atributu `[SiliconManagerOnly]` |
| 5.3 | IAIClient: Podpora volání nástrojů | Parsování AI tool_calls. Smyčka: provedení nástroje → odeslání výsledku zpět → AI pokračuje → dokud čistý text |
| 5.4 | Základní třída exekutoru | Abstraktní základní třída s nezávislým plánovacím vláknem, frontou požadavků, kontrolou časového limitu |
| 5.5 | NetworkExecutor | HTTP požadavky prostřednictvím exekutoru. Časový limit, fronta |
| 5.6 | CommandLineExecutor | Shell provádění prostřednictvím exekutoru. Detekce platformového oddělovače |
| 5.7 | DiskExecutor | Operace se soubory prostřednictvím exekutoru. Zatím žádná kontrola oprávnění (Fáze 6) |
| 5.8–5.12 | Vestavěné nástroje | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Výsledek**: Silikonové bytosti mohou volat nástroje k provádění akcí.

**Ověření**: Zeptejte se "Jaký je den v týdnu" → CalendarTool odpoví; Zeptejte se "Zkontroluj procesy" → SystemTool provede; Řekněte bytosti, aby poslala zprávu jiné bytosti → ChatTool funguje.

---

## ~~Fáze 6: Dodržuje Pravidla (Systém Oprávnění)~~ ✅ Dokončeno

**Cíl**: Silikonové bytosti nemohou přistupovat k citlivým zdrojům bez autorizace.

| # | Modul | Popis |
|---|--------|-------------|
| 6.1 | PermissionManager | Soukromá instance na bytost. Založeno na callbacku, tříhodnotový výsledek (Allowed/Deny/AskUser). Priorita dotazu: HighDeny → HighAllow → Callback. Příznak IsCurator |
| 6.2 | Výčet PermissionType | NetworkAccess, CommandLine, FileAccess, Function, DataAccess |
| 6.3 | DefaultPermissionCallback | Bílá/černá listina sítě, klasifikace CLI, bezpečnostní pravidla cest souborů |
| 6.4 | GlobalACL | Tabulka pravidel shody prefixů, perzistence do úložiště |
| 6.5 | UserFrequencyCache | Seznamy HighAllow/HighDeny. Uživatelská volba (ne automatická detekce). Shoda prefixů, pouze paměť, konfigurovatelná expirace |
| 6.6 | UserAskMechanism (Konzole) | Výzva y/n v konzoli při vrácení AskUser |
| 6.7 | Integrace oprávnění exekutoru | Všechny exekutory kontrolují oprávnění před provedením |
| 6.8 | Poznámka k izolaci IStorage | IStorage je interní perzistence systému — přímý přístup k souborům, **ne** směrován přes exekutory, **ne** ovladatelný AI. Exekutory spravují pouze IO iniciované nástroji AI |
| 6.9 | Audit logy | Zaznamenávání všech rozhodnutí o oprávněních s časovým razítkem, žadatelem, zdrojem, výsledkem |

**Výsledek**: Výzva oprávnění, když se bytost pokusí o citlivou operaci.

**Ověření**: Řekněte bytosti, aby smazala soubor → Konzole zobrazí výzvu oprávnění → Zadejte `n` → Operace zamítnuta. Řekněte bytosti, aby přistoupila k webové stránce na bílé listině → Okamžitě povoleno.

---

## ~~Fáze 7: Může Se Vyvíjet (Dynamická Kompilace)~~ ✅ Dokončeno

**Cíl**: Silikonové bytosti mohou přepisovat svůj vlastní kód.

| # | Modul | Popis |
|---|--------|-------------|
| 7.1 | CodeEncryption | AES-256 šifrování/dešifrování. PBKDF2 klíč odvozený z GUID |
| 7.2 | DynamicCompilationExecutor | Paměťová kompilační sandbox založený na Roslyn. Kontrola referencí při kompilaci (hlavní obrana: vyloučení System.IO, Reflection atd.) |
| 7.3 | Bezpečnostní skenování | Statická analýza nebezpečných vzorů kódu za běhu (sekundární obrana). Blokování načtení, pokud skenování selže |
| 7.4 | Vylepšení životního cyklu bytosti | Načtení: dešifrování → skenování → kompilace → instance. Za běhu: kompilace v paměti → atomická výměna → perzistentní šifrování |
| 7.5 | SiliconCurator | Abstraktní základní třída kurátora. IsCurator=true. Nejvyšší oprávnění |
| 7.6 | DefaultCurator | Výchozí implementace kurátora s vestavěným souborem duše a správcovskými nástroji |
| 7.7 | CuratorTool | Nástroj `[SiliconManagerOnly]`: list_beings, create_being, get_code, reset |
| 7.8 | Přepsání callbacku oprávnění | Bytosti mohou kompilovat vlastní callbacky oprávnění |
| 7.9 | Vylepšení SiliconBeingManager | Metoda Replace (výměna instance za běhu). MigrateState (přenos stavu mezi starou a novou instancí) |

**Výsledek**: Silikonové bytosti mohou generovat nový kód prostřednictvím AI, kompilovat a nahrazovat samy sebe.

**Ověření**: Řekněte bytosti "přidej si novou funkci" → Sledujte kompilaci → Restart → Nová funkce funguje.

---

## ~~Fáze 8: Paměť a Plánování~~ ✅ Dokončeno

**Cíl**: Dlouhodobá paměť, správa úkolů, časové spouštění.

| # | Modul | Popis |
|---|--------|-------------|
| 8.1 | FileSystemMemory | Krátkodobé/dlouhodobé segmentované úložiště. Časový útlum. Komprese (sloučení podobných vzpomínek). Vícerozměrné vyhledávání |
| 8.2 | TaskSystem | Jednorázové + DAG závislé úkoly. Prioritní plánování. Sledování stavu |
| 8.3 | TimerSystem | Jednorázové alarmy + periodické časovače. Přesnost na milisekundy. Perzistence do úložiště |
| 8.4 | IncompleteDate | Struktura fuzzy rozsahu data (např. "duben 2026", "jaro 2026") |
| 8.5–8.7 | Nástroje paměti/úkolů/časovačů | Nástroje pro bytosti k dotazování paměti, správě úkolů, nastavování časovačů |

**Výsledek**: Bytosti si mohou pamatovat klíčové body, vytvářet/sledovat úkoly, nastavovat alarmy.

**Ověření**: Vytvořte úkol → Zkontrolujte seznam úkolů → Nastavte 1minutový alarm → Přijměte oznámení při vypršení.

---

## ~~Fáze 9: Framework Dokončen~~ ✅ Dokončeno

**Cíl**: Sjednocený vstupní bod, spolupráce více bytostí.

| # | Modul | Popis |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | Sjednocený hostitel s patternem builder. Elegantní vypnutí (Ctrl+C / SIGTERM) |
| 9.2 | Refaktorizace Program.Main | Migrace na pattern CoreHostBuilder |
| 9.3 | Vylepšení SiliconBeingManager | Prioritní odpověď kurátora. Izolace výjimek. Pravidelná perzistence |
| 9.4 | Načítání více bytostí | Načtení více bytostí z datového adresáře. Komunikace mezi bytostmi prostřednictvím ChatTool |
| 9.5 | Monitorování výkonu | Sledování doby provádění každého objektu hodin |
| 9.6 | ServiceLocator | Globální service locator s metodami Register/Get |

**Výsledek**: Více bytostí běží současně, spolupracují, řízeno CoreHost.

**Ověření**: Vytvořte dvě bytosti → A pošle zprávu B → B přijme a odpoví → Framework plánuje bez chyb. Kurátor prioritně odpovídá, když přijde zpráva uživatele.

---

## ~~Fáze 10: Směřování k Webu~~ ✅ Dokončeno

**Cíl**: Migrace z konzole na rozhraní prohlížeče.

| # | Modul | Popis |
|---|--------|-------------|
| 10.1 | Router | HTTP request router. Cesty se sériovými parametry a obsluha statických souborů |
| 10.2 | Základní třída Controller | Kontext požadavku/odpovědi. Podpora HTML a JSON odpovědí |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | C# server-side buildery. Nulová závislost na frontendovém frameworku |
| 10.6 | SSE (Server-Sent Events) | Aktualizace push v reálném čase pro chat, stav bytostí a systémové události. Jednodušší než WebSocket s automatickým opětovným připojením klienta |
| 10.7 | WebUIProvider | IM kanál založený na SSE v reálném čase. Nahrazuje konzoli jako hlavní rozhraní |
| 10.8 | Webová bezpečnost | Černá/bílá listina IP. Atribut `[WebCode]`. Dynamické aktualizace |
| 10.9–10.17 | Webové controllery | Chat, dashboard, bytosti, úkoly, oprávnění, žádosti o oprávnění, exekutory, logy, konfigurace, paměť, časovače, inicializace, o aplikaci, prohlížeč kódu, znalosti, projekty, audit |

**Výsledek**: Kompletní Web UI přístupné z prohlížeče.

**Ověření**: Otevřete prohlížeč → Chatujte s bytostí → Zobrazte dashboard → Spravujte oprávnění → Vše funguje.

---

## ~~Fáze 10.5: Přírůstková Vylepšení~~ ✅ Dokončeno

**Cíl**: Vylepšení existujícího systému o nové funkce objevené během vývoje.

| # | Modul | Popis |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | Nový typ relace pro systémová oznámení. Pevné ID kanálu, dynamické přihlašování, filtrování čekajících zpráv |
| 10.5.2 | Vylepšení ChatMessage | Pole ToolCallId, ToolCallsJson, Thinking pro kontext AI; PromptTokens, CompletionTokens, TotalTokens pro sledování tokenů; Typ zprávy SystemNotification |
| 10.5.3 | TokenUsageAuditManager | Sledování spotřeby tokenů pro každý požadavek napříč všemi bytostmi. Agregované statistiky, dotazy časové řady, perzistentní úložiště |
| 10.5.4 | TokenAuditTool | Nástroj `[SiliconManagerOnly]` pro kurátory k dotazování a shrnutí využití tokenů |
| 10.5.5 | ConfigTool | Nástroj `[SiliconManagerOnly]` pro kurátory ke čtení a úpravě systémové konfigurace |
| 10.5.6 | AuditController | Web dashboard pro audit využití tokenů s grafy trendů a exportem dat |
| 10.5.7 | Rozšíření kalendářového systému | 32 implementací kalendářů pokrývajících světové kalendářní systémy (Buddhistický, Lunární, Islámský, Hebrejský, Japonský, Perský, Mayský atd.) |
| 10.5.8 | Vylepšení DiskTool | Nové operace: count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all, list_drives |
| 10.5.9 | Vylepšení SystemTool | Nové operace: find_process (podpora wildcardů), resource_usage |
| 10.5.10 | Vylepšení CalendarTool | Nové operace: diff, list_calendars, get_components, get_now_components, convert (převod mezi kalendáři) |
| 10.5.11 | DashScopeClient | AI klient Alibaba Cloud Bailian, kompatibilní s OpenAI API. Podpora streamování, volání nástrojů, reasoning obsahu |
| 10.5.12 | DashScopeClientFactory | Factory pro vytváření Bailian klientů. Dynamický objev modelů prostřednictvím API. Podpora více regionů (Peking, Virginie, Singapur, Hongkong, Frankfurt) |
| 10.5.13 | Konfigurační systém AI klientů | Konfigurace AI klienta na bytost. Možnosti dynamických konfiguračních klíčů (model, region). Lokalizované zobrazované názvy |
| 10.5.14 | Rozšíření lokalizace | Zjednodušená čínština, tradiční čínština, angličtina a japonská lokalizace pro možnosti konfigurace Bailian, názvy modelů a názvy regionů |

**Výsledek**: Vylepšené nástroje, pozorovatelnost, pokrytí kalendářů a podpora více AI backendů.

**Ověření**: Kurátor dotazuje využití tokenů prostřednictvím TokenAuditTool → Audit dashboard zobrazuje trendy → CalendarTool převádí data mezi 32 kalendářovými systémy → Přepněte AI backend na Bailian → Chatujte s modely Tongyi Qianwen prostřednictvím cloudového API.

---

## ~~Fáze 10.6: Dokončení a Optimalizace~~ ✅ Dokončeno

**Cíl**: Dokončení funkcí systému, přidání nových funkcí, optimalizace uživatelského zážitku.

| # | Modul | Popis |
|---|--------|-------------|
| 10.6.1 | WebViewBrowserTool | Cross-platformní nástroj automatizace prohlížeče založený na Playwright, podporující headless mód, individuální izolaci, plnou podporu JS/CSS |
| 10.6.2 | HelpTool | Nástroj systému nápovědní dokumentace, podporující vícejazyčné dotazování a zobrazování dokumentace |
| 10.6.3 | ProjectWorkNoteTool | Nástroj projektových pracovních poznámek, podporující záznamy a správu práce v dimenzi projektu |
| 10.6.4 | ProjectTaskTool | Nástroj správy projektových úkolů, podporující přiřazování úkolů, sledování postupu |
| 10.6.5 | KnowledgeTool | Nástroj znalostní sítě, podporující CRUD a objevování cest pro trojčlenné znalosti |
| 10.6.6 | ChatHistoryController | Controller zobrazení historie chatu, podporující seznam relací a detaily zpráv |
| 10.6.7 | CodeHoverController | Controller plovoucích nápověd kódu, podporující zvýraznění syntaxe a nápovědy kódu |
| 10.6.8 | WorkNoteController | Controller správy pracovních poznámek, podporující vyhledávání a generování obsahu |
| 10.6.9 | TimerExecutionHistory | Funkce historie provádění časovače, zaznamenávání a zobrazování historie spuštění časovače |
| 10.6.10 | Rozšíření lokalizace | Přidání podpory lokalizace češtiny (cs-CZ), celkem 21 jazykových variant |
| 10.6.11 | Optimalizace Web UI | Podpora nahrávání souborů, indikátory načítání, optimalizace vykreslování volání nástrojů, oprava modálních oken pracovních poznámek |
| 10.6.12 | Vylepšení správy paměti | Pokročilé filtrování, statistiky, zobrazení detailů, optimalizace kompresního algoritmu |
| 10.6.13 | Refaktorizace logovacího systému | Oddělení logů systému/silikonových bytostí, API čtení logů, filtr silikonových bytostí |
| 10.6.14 | Vylepšení systému oprávnění | Ověření předkompilace callbacku oprávnění, ověření reference sestavení, bílá listina služby wttr.in počasí |

**Výsledek**: Kompletní automatizace prohlížeče WebView, systém nápovědní dokumentace, projektový pracovní prostor, znalostní síť, zobrazení historie chatu a další vylepšení.

**Ověření**: Silikonové bytosti mohou ovládat prohlížeč prostřednictvím WebViewBrowserTool → Získat nápovědní dokumentaci prostřednictvím HelpTool → Spravovat projektové pracovní poznámky a úkoly → Dotazovat znalostní síť → Zobrazit historii chatu.

---

## Fáze 11: Integrace Externího IM

**Cíl**: Připojení k externím platformám zpráv pro širší dostupnost uživatelů.

| # | Modul | Popis |
|---|--------|-------------|
| 11.1 | FeishuProvider | Integrace robota Feishu (Lark), podpora karet |
| 11.2 | WhatsAppProvider | Integrace WhatsApp Business API |
| 11.3 | TelegramProvider | Integrace Telegram Bot API, podpora inline klávesnice |
| 11.4 | Vylepšení IMManager | Směrování více poskytovatelů, sjednocený formát zpráv, cross-platformní zpracování žádostí o oprávnění |

**Výsledek**: Uživatelé mohou komunikovat se silikonovými bytostmi prostřednictvím externích IM platforem.

---

## Fáze 12: Pokročilé Funkce

**Cíl**: Volitelné pokročilé funkce pro vylepšené schopnosti.

| # | Modul | Popis |
|---|--------|-------------|
| 12.1 | Znalostní síť | Sdílený znalostní graf pomocí trojčlenné struktury (subjekt-predikát-objekt) |
| 12.2 | Systém pluginů | Načítání externích pluginů s bezpečnostními kontrolami a sandboxem |
| 12.3 | Ekosystém dovedností | Trh znovu použitelných dovedností pro schopnosti bytostí |
