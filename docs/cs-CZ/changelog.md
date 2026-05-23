# Záznam změn

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | **Čeština**

Všechny důležité změny tohoto projektu budou zdokumentovány v tomto souboru.

Formát je založen na [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
a tento projekt dodržuje [Semantické verzování](https://semver.org/spec/v2.0.0.html).

---

## O tomto záznamu změn

### Duální Verze Projektu

Tento projekt poskytuje dvě implementační verze:

- **SiliconLife.Default**: Výchozí implementace, primárně použita pro ověření architectury. Konzolová aplikace, JSON úložiště v souborovém systému.
- **SiliconLife.Fast**: Verze pro produkční prostředí. Multiplatformní desktopová aplikace (Windows / macOS / Linux), paměťové úložiště SpeedyPack + asynchronní perzistence, hloubce optimalizovaná pro výkon.

Obě verze sdílejí stejná rozhraní a funkce, liší se pouze implementací úložiště a režimem spuštění. SiliconLife.Default slouží jako referenční baseline pro ověření architektury, zatímco SiliconLife.Fast je hlavní verze pro produkční prostředí.

### Původ projektu

- Tento projekt vznikl 20. března 2026.
- Před tímto projektem existoval ověřovací demo, které selhalo kvůli špatnému návrhu architektury, což znemožnilo integraci s více AI platformami.

### Použité AI IDE nástroje

#### Kiro (Amazon AWS)
- Projekt byl zpočátku udržován Kiro a spuštěn pomocí režimu Spec.
- Kiro je agentic AI vývojové prostředí vytvořené společností Amazon AWS.
- Založeno na Code OSS (VS Code), podporuje nastavení VS Code a rozšíření kompatibilní s Open VSX.
- Obsahuje spec-driven vývojový workflow pro strukturované AI kódování.

#### Comate AI IDE / 文心快码 (Baidu)
- Příležitostně používán pro copywriting a dokumentační práce.
- Comate AI IDE je AI-nativní vývojový nástroj vydaný společností Baidu Wenxin 23. června 2025.
- První multimodální, multi-agentní kolaborativní AI IDE v průmyslu.
- Funkce zahrnují převod designu na kód a plnou AI asistenci při kódování.
- Poháněno modelem Baidu Wenxin 4.0 X1 Turbo.

#### Trae (ByteDance)
- Používáno od října 2025 do dubna 2026.
- AI IDE s inteligentním generováním kódu a správou projektů.

#### Qoder (Alibaba)
- Používáno pro údržbu projektu od 18. dubna 2026.
- AI kódovací platforma podporující analýzu kódu, generování dokumentace a spolupráci více agentů.

#### CatPaw (Meituan)
- Používáno v kombinaci s Qoder od 6. května 2026.
- Založeno na vlastních modelech LongCat společnosti Meituan, s výkonnými schopnostmi kompletní refactorizace architektury kódu.

### Požadavkový dokument

- Požadavkový dokument pro tento projekt není veřejně dostupný.
- Požadavky byly validovány prostřednictvím iterací s více než 12 mezinárodními AI platformami a velkými modelovými sériemi, což vytvořilo uživatelskými příběhy řízený požadavkový dokument o více než 2000 řádcích, který je téměř nesrozumitelný pro lidi.

---

## [Neuvedeno]

### 2026-05-22

#### Opravy konzistence dokumentace
- `9e07b27` - Opravit nesrovnalosti francouzské dokumentace (fr-FR) se zdrojovým kódem (ref task-307)
  - 10 souborů změněno

- `9e3be72` - Opravit nesrovnalosti německé dokumentace (de-DE) se zdrojovým kódem (ref task-308)
  - 5 souborů změněno

- `2bc7151` - Opravit nesrovnalosti španělské dokumentace (es-ES) se zdrojovým kódem (ref task-309)
  - 13 souborů změněno

- `f95088e` - Opravit nesrovnalosti italské dokumentace (it-IT) se zdrojovým kódem (ref task-310)
  - 11 souborů změněno

- `6ea9f4a` - Opravit nesrovnalosti polské dokumentace (pl-PL) se zdrojovým kódem (ref task-311)
  - 16 souborů změněno

- `7646923` - Opravit nesrovnalosti portugalské dokumentace (pt-PT) se zdrojovým kódem (ref task-312)
  - 12 souborů změněno

- `7eaf9db` - Opravit nesrovnalosti české dokumentace (cs-CZ) se zdrojovým kódem (ref task-313)
  - 12 souborů změněno

#### Rámec spolupráce
- `3cb7347` - Aktualizace task-313 relatedCommit=7eaf9db
  - 1 souborů změněno

### 2026-05-21

#### Nové funkce
- `99eca78` - Přidat 'Zobrazit úložiště (jen pro čtení)' do kontextové nabídky, volání Speedy.Manager v procesu (ref task-301)
  - 26 souborů změněno

#### Opravy konzistence dokumentace
- `7f65cf1` - Opravit nesrovnalosti dokumentace zh-CN se zdrojovým kódem (ref task-303)
  - 15 souborů změněno

- `a9e2a2c` - Opravit nesrovnalosti anglické dokumentace (en) se zdrojovým kódem (ref task-302)
  - 9 souborů změněno

- `2549105` - Opravit nesrovnalosti tradiční čínské dokumentace (zh-HK) se zdrojovým kódem (ref task-304)
  - 12 souborů změněno

- `277eb50` - Opravit nesrovnalosti japonské dokumentace se zdrojovým kódem (ref task-305)
  - 10 souborů změněno

- `edce413` - Opravit nesrovnalosti korejské dokumentace (ko-KR) se zdrojovým kódem (ref task-306)
  - 18 souborů změněno

- `f2adcae` - Opravit nekonzistence portugalské dokumentace se zdrojovým kódem (ref task-220)
  - 15 souborů změněno

- `3332987` - Opravit nekonzistence tradiční čínské (Hong Kong) dokumentace se zdrojovým kódem (ref task-218)
  - 14 souborů změněno

- `af9f715` - Opravit nekonzistence polské dokumentace se zdrojovým kódem (ref task-217)
  - 15 souborů změněno

- `2e2b18b` - Opravit nekonzistence korejské dokumentace se zdrojovým kódem (ref task-216)
  - 16 souborů změněno

- `626ebc9` - Opravit nekonzistence japonské dokumentace se zdrojovým kódem (ref task-215)
  - 19 souborů změněno

- `48d061b` - Opravit nekonzistence italské dokumentace se zdrojovým kódem (ref task-214)
  - 14 souborů změněno

#### Rámec spolupráce
- `6683bee` - Registrace týmu Marvis AI, aktualizace stavu úkolů
  - 3 souborů změněno

- `03fc905` - Archivace task-210~220
  - 5 souborů změněno

### 2026-05-20

#### Nové funkce
- `65176d4` - Přidání úplné podpory portugalské lokalizace (pt-PT + pt-BR) (ref task-208)
  - 41 souborů změněno

#### Opravy konzistence dokumentace
- `af4dffd` - Opravit všechny nekonzistence dokumentace zh-CN se zdrojovým kódem (ref task-209)
  - 11 souborů změněno

- `144b945` - Opravit nekonzistence anglické (en) a české (cs-CZ) dokumentace se zdrojovým kódem (ref task-219, task-210)
  - 22 souborů změněno

- `08bec55` - Opravit nekonzistence německé dokumentace (de-DE) se zdrojovým kódem (ref task-211)
  - 14 souborů změněno

- `7ff28de` - Opravit nekonzistence španělské dokumentace (es-ES) se zdrojovým kódem (ref task-212)
  - 14 souborů změněno

- `15e2133` - Opravit nekonzistence francouzské dokumentace (fr-FR) se zdrojovým kódem (ref task-213)
  - 13 souborů změněno

#### Opravy chyb
- `7dac388` - Opravit nezobrazující se seznam úkolů projektu (ref task-207)
  - 6 souborů změněno

#### Rámec spolupráce
- `7890223` - Archivace task-201~209, publikování úkolů opravy konzistence dokumentace task-210~220
  - 5 souborů změněno

### 2026-05-19

#### Nové funkce
- `cd72846` - Implementace bezpečné alternativy pro obejití bezpečnostní kontroly PluginLoader (ref task-203)
  - 13 souborů změněno

- `fc0c00c` - Vylepšení Speedy.Manager - Vytvořit/Importovat/Exportovat/Hierarchie TreeView/Okno průběhu (ref task-206)
  - 9 souborů změněno

#### Opravy chyb
- `ec07118` - Opravit problém ITypeRegistry/IObjectFactory neregistrovaných před načtením pluginů (ref task-205)
  - 8 souborů změněno

- `9e749db` - Opravit chybu Creator ID is required při vytváření projektu (ref task-204)
  - 4 souborů změněno

#### Infrastruktura
- `43dc092` - Migrace CLDR - přidání CldrDataProvider, odebrání .github
  - 1 souborů změněno

- `c09ec1f` - Přidat cldr/ do .gitignore
  - 1 souborů změněno

- `221f818` - Synchronizace GitHub změněna na schéma Gitee push mirror, workflow ponechán pouze jako ruční záloha
  - 1 souborů změněno

- `08cdf1a` - Opravit workflow synchronizace GitHub - přidat logiku opakování a přeskočení bez změn
  - 1 souborů změněno

- `fb4e77d` - Aktualizace SiliconLife.Speedy.Manager.csproj
  - 1 souborů změněno

#### Rámec spolupráce
- `df90af0` - Aktualizace task-203 relatedCommit=cd72846
  - 1 souborů změněno

### 2026-05-18

#### Refaktoring
- `e720d06` - Kompletní refaktoring Speedy.Manager z WinForms na Avalonia (ref task-202)
  - 17 souborů změněno

#### Opravy chyb
- `08894a9` - Opravit chybu zobrazení úrovně položek shrnutí časové osy paměti (ref task-201)
  - 3 souborů změněno

#### Rámec spolupráce
- `2871afb` - Archivovat všechny úkoly, vyčistit tasks.json
  - 2 souborů změněno

### 2026-05-17

#### Nové funkce
- `d6eb994` - Přidat vstup pro vytvoření projektu a výběr šablony workflow na stránku seznamu projektů (ref task-203)
  - 14 souborů změněno

- `0872134` - Orchestrace řízená kurátorem ThinkOnProject pro projekty bez šablony (ref task-202)
  - 6 souborů změněno

- `cb3188e` - Vizualizace @zmínek ve skupinovém chatu (ref task-208)
  - 4 souborů změněno

- `f9968e5` - Deklarace schopností ToolCall AI klienta a elegantní degradace (ref task-205)
  - 4 souborů změněno

- `0d2b843` - Rozhodovací logika skupinového chatu ShouldReplyInGroupChat (ref task-201)
  - 6 souborů změněno

- `277a2b1` - Doplnění znalostní sítě - pokročilé dotazy a průchod grafy (ref task-207)
  - 9 souborů změněno

#### Opravy chyb
- `6d0b66e` - Opravit TypeError appendMessage při odesílání zpráv ve skupinovém chatu (ref task-209)
  - 5 souborů změněno

- `b15167c` - Doplňující odeslání chybějící registrace trasy list-workflow-templates z task-203 (ref task-203)
  - 1 souborů změněno

- `dc549a2` - Opravit workflow synchronizace Gitee - přidat uživatelské jméno do URL tokenu
  - 1 souborů změněno

#### Infrastruktura
- `e5fa3ad` - Deaktivovat automatickou synchronizaci GitHub schedule, čekání na oficiální řešení Gitee
  - 1 souborů změněno

#### Rámec spolupráce
- `4a58c82` - Přidat zprávu o analýze systémových schopností + návrh ThinkOnProject
  - 5 souborů změněno

- `8ab29e6` - Archivace zprávy o analýze úplnosti systémových schopností v .ai-collab/docs
  - 2 souborů změněno

- `b412d9c` - Archivovat staré úkoly, znovu publikovat task-201~208 na základě komplexní analýzy
  - 2 souborů změněno

- `437884a` - Aktualizace metadat spolupráce - task-202/203/204 dokončeny (ref task-202, task-203, task-204)
  - 2 souborů změněno

- `bf78d79` - Aktualizace metadat spolupráce - task-201/205/208 dokončeny
  - 2 souborů změněno

- `de6ee0e` - Záznam konce relace catpaw-20260517-2215
  - 5 souborů změněno

- `7223b6f` - Záznam konce relace catpaw-20260517-2200
  - 4 souborů změněno


## [Alpha-0.2] - 2026-05-16

### 2026-05-16

#### Příprava vydání
- `476d839` - Přidány úkoly pro vydání alpha-0.2
  - Vytvořeny task-114 (psaní CHANGELOG) a task-115 (aktualizace čísla verze)
  - 1 soubor změněn

### 2026-05-15

#### Infrastruktura
- `672627b` - Přidán workflow synchronizace Gitee (s konfigurací oprávnění)
  - Aktualizována oprávnění workflow sync-from-gitee.yml
  - 1 soubor změněn, 7 přidání(+), 4 odebrání(-)

- `3cd5256` - Přidána automatická synchronizace Gitee přes GitHub Actions
  - Přidán workflow sync-from-gitee.yml
  - 1 soubor změněn, 50 přidání(+)

#### Aktualizace dokumentace
- `aa1d2ad` - Aktualizovány README/architektura/začínáme ve všech 11 jazycích, odrážející multiplatformní podporu SiliconLife.Fast (ref task-112, task-113)
  - Opravena dokumentace popisující SiliconLife.Fast jako pouze Windows, odrážející skutečnou multiplatformní podporu (Windows / macOS / Linux)
  - Aktualizovány README.md, architecture.md, getting-started.md ve 11 jazycích
  - Přidána podpora vlastnosti hint v SelectComponent
  - ConfigView nyní předává parametr hint rozbalovacím seznamům výčtů
  - Přidán klíč SelectSearchHint do lokalizací 11 jazyků
  - 53 souborů změněno, 690 přidání(+), 194 odebrání(-)

#### Systém úkolů
- `3329f3d` - Přidán mechanismus inspekce systému úkolů + úkoly opravy lokalizačních chyb
  - Vytvořen task-113: opravit problém lokalizace na informační stránce
  - Aktualizován task-112: aktualizovat dokumentaci verze Fast pro podporu Linuxu
  - Archivovány dokončené úkoly (11) do .ai-collab/archive/
  - Mechanismus inspekce nakonfigurován: rychlá inspekce (každých 30 min) + úplná inspekce (denně 06:00)
  - 2 soubory změněny, 148 přidání(+), 171 odebrání(-)

#### Rámec spolupráce
- `6038e22` - coze-agent registrován v registru .ai-collab
  - Přidány registrační informace rezidentní AI platformy Coze
  - 1 soubor změněn

### 2026-05-14

#### Rámec AI spolupráce
- `7344fbb` - Režim handoff odstraněn, přechod na přístup řízený seznamem úkolů (v2.0)
  - Restrukturalizován adresář .ai-collab z režimu handoff na přístup řízený seznamem úkolů
  - Přidán hlavní soubor seznamu úkolů tasks.json
  - Přidán protokol operací activity.log
  - Přidány adresáře changes/ a sessions/

- `589a48e` - Přidány záznamy relací .ai-collab
  - Přidány záznamy stavu relace AI spolupráce

- `5481bcf` - Qoder AI IDE registrován v registru spolupráce
  - Přidány registrační informace asistenta kódování Qoder AI

- `e2d7b61` - relatedCommit a changes commitHash v tasks.json doplněny
  - Asociace metadat úkolů dokončeny

- `a087f0c` - Všechny úkoly task-101~110 přijaty
  - Potvrzeno, že všech 10 oprav úkolů je dokončeno

#### Opravy chyb
- `fac9435` - Dokončeny všechny opravy a implementace task-101~110
  - Opraven chybějící text nápovědy v komponentě výběru hledání
  - Opraveny problémy lokalizace na informační stránce
  - Opravena chyba JS vyhledávání v systému nápovědy
  - 39 souborů změněno, 684 přidání(+), 121 odebrání(-)

- `c46dfbc` - Dokončeny všechny čekající úkoly (task-001~006)
  - Dokončeno 6 počátečních čekajících úkolů

- `ec176b2` - Seznam úkolů přepsán - revize kódu našla 10 nových chyb
  - Vytvořeny task-101~110 (10 nových úkolů)

#### Refaktoring
- `ab15915` - Sjednoceny hlavičky autorských práv + opraveny HelpController BOM a HelpView vyhledávací JS
  - Sjednoceny hlavičky autorských práv Apache 2.0 ve všech zdrojových souborech C#
  - Opraven problém kódování BOM v HelpController
  - Opravena chyba JavaScript vyhledávání v HelpView

#### Nové funkce
- `18a6f5d` - Vytvořen MCP server schopností prohlížeče (ref task-111)
  - Přidán projekt SiliconLife.McpServer
  - Implementován MCP server automatizace prohlížeče Playwright

- `9eb251a` - Odstraněn modul SiliconLife.McpServer (ref task-111)
  - Samostatný MCP server odstraněn, funkce integrována do hlavního projektu

### 2026-05-13

#### Lokalizace
- `7a62590` - Přidána podpora polské lokalizace
  - Přidána implementace polské lokalizace pl-PL (PlPL.cs, 1089 řádků)
  - Přidána lokalizace polské dokumentace nápovědy (HelpLocalizationPlPL.cs, 3972 řádků)
  - Přidána podpora polského čínského historického kalendáře (ChineseHistoricalPlPL.cs, 600 řádků)
  - Přidána lokalizace polského traye (TrayPlPL.cs, 135 řádků)
  - Přidána kompletní sada polské dokumentace (15 dokumentů)
  - Enumerace Language rozšířena o polštinu
  - 35 souborů změněno, 14379 přidání(+), 11 odebrání(-)

- `51f9c8e` - Aktualizovány odkazy na Ark AI a vylepšení terminologie v dokumentaci
  - Aktualizována terminologie AI klienta ve vícejazyčné dokumentaci

- `7587c12` - Přidány položky protokolu změn pro všechny jazyky
  - Aktualizace protokolu změn synchronizovány ve všech jazykových verzích

#### Migrace systému oken
- `b49a07d` - Migrováno na rezidentní režim okna Avalonia
  - Odstraněna závislost na Windows Forms, úplná migrace na framework Avalonia UI
  - Okno stavu se správně zobrazuje na Linuxu (ověřeno přes vzdálenou plochu)
  - Přidány ovládací prvky okna: kontextové menu, dvojklik pro otevření Webu, tlačítko zavření
  - Přidán rámec multi-AI spolupráce (.ai-collab/)
  - Opravena inicializace ikony traye (graceful degradation)
  - Přidány App.axaml a App.cs jako vstupní body aplikace Avalonia
  - 13 souborů změněno, 1442 přidání(+), 541 odebrání(-)

- `d335aaf` - Okno vždy viditelné na platformě Linux + dialog potvrzení zavření
  - Linux automaticky zobrazuje okno stavu (bez ikony traye)
  - Linux zobrazuje dialog potvrzení při zavření okna
  - Windows/macOS zachovávají původní chování traye
  - Podporován parametr --no-tray pro vynucené zakázání traye
  - Přidána metoda ShowMessageBoxAsync pro potvrzovací dialogy
  - 3 soubory změněny, 206 přidání(+), 29 odebrání(-)

#### Refaktoring systému traye
- `841d384` - Refaktorován systém traye a inicializován rámec AI spolupráce
  - TrayLocalizationBase zjednodušen, nepoužité vlastnosti odstraněny
  - Přidána položka lokalizace ShowStatus
  - App.cs: kliknutí na ikonu traye zobrazí okno stavu, přidány lokalizované položky menu
  - Program.cs: inicializace ikony traye přesunuta do StartAsync
  - TrayStatusWindow se při zavření skrývá místo ukončení
  - Registrovány trae-glm5 a catpaw v rámci .ai-collab
  - Aktualizován .gitignore pro zajištění sledování všech souborů .ai-collab
  - 22 souborů změněno, 178 přidání(+), 1226 odebrání(-)

#### Dokumentace
- `43653bc` - Aktualizován popis repozitáře a AI registr
  - Aktualizováno README projektu a registrační informace .ai-collab

### 2026-05-12

#### Webová zobrazení systému úloh
- `0891b3c` - Přidat zobrazení detailu a historie provádění úloh
  - Přidán TaskExecutionDetailView detailní pohled na provádění úloh
  - Přidán TaskExecutionHistoryView pohled na historii provádění úloh
  - TaskController přidána rozhraní pro dotazy na detail a historii provádění
  - Přidán TaskViewModel model zobrazení úloh
  - TaskCenter centrum úloh vylepšeno
  - TaskSystem systém úloh aktualizován
  - 9 jazyků lokalizace přidány klíče související s úlohami
  - 26 souborů změněno, 803 vložení(+), 55 odstranění(-)

### 2026-05-11

#### Refaktorizace architektury webových komponent
- `5e687ad` - Migrace vykreslování komponent z řetězce do H-tree
  - ComponentBase metoda vykreslování migrována ze vzoru řetězce do struktury H-tree
  - Všech 28 komponent přizpůsobeno nové architektuře vykreslování (A, Accordion, Button, Calendar, Card, Chart atd.)
  - SelectComponent velká refaktorizace (889 řádků vylepšeno)
  - Kontroléry a zobrazení odpovídajícím způsobem aktualizovány
  - 33 souborů změněno, 667 vložení(+), 435 odstranění(-)

- `bfd332d` - Migrace Style z řetězce do CssBuilder inline stylů
  - Přidán CssBuilder tvůrce stylů
  - ComponentBase systém stylů migrován z řetězce do strukturovaného CssBuilder
  - LoadingComponent výrazně vylepšen (103 řádků přidáno)
  - ConfigController, LogController, MemoryController migrace stylů kontrolérů
  - ChatView, ConfigView, LogView, MemoryView migrace stylů zobrazení
  - 37 souborů změněno, 351 vložení(+), 157 odstranění(-)

#### Optimalizace úložného systému
- `d67a7ee` - Optimalizace QueryLatest pro velké datové sady
  - SpeedyTimeStorage QueryLatest optimalizace výkonu metody
  - SpeedyLoggerProvider poskytovatel protokolování vylepšen
  - 2 soubory změněny, 44 vložení(+), 5 odstranění(-)

#### Refaktorizace kalendářového systému
- `9629f88` - Extrakce TimerExecution a vylepšení webových zobrazení časovače
  - TimerSystem extrahována logika TimerExecution (175 řádků odstraněno)
  - SelectComponent výrazně vylepšen (427 řádků vylepšeno)
  - TimerController a zobrazení časovače vylepšeny
  - ContextManager správce kontextu aktualizován
  - 12 souborů změněno, 458 vložení(+), 267 odstranění(-)

#### Lokalizace
- `5d8ca79` - Přidat lokalizační klíč LogsLoading
  - 9 jazyků přidán klíč LogsLoading
  - DefaultLocalizationBase základní třída přidána definice
  - 11 souborů změněno, 15 vložení(+)

### 2026-05-10

#### Refaktorizace systému úloh
- `54394f6` - Sloučení systému úloh s cykly historie chatu
  - ProjectTaskSystem systém úloh projektu výrazně zjednodušen (411 řádků refaktorováno)
  - TaskSystem systém úloh zjednodušen (254 řádků refaktorováno)
  - TaskCenter centrum úloh refaktorováno (188 řádků vylepšeno)
  - ContextManager správce kontextu optimalizován (347 řádků refaktorováno)
  - DefaultSiliconBeing křemíková bytost vylepšena
  - TimerSystem systém časovače integrován s úlohami
  - IWorkNoteStorage rozhraní aktualizováno
  - SpeedyWorkNoteStorage a FileSystemWorkNoteStorage přizpůsobeny
  - 16 souborů změněno, 648 vložení(+), 897 odstranění(-)

### 2026-05-09

#### Vylepšení webového rozhraní
- `bc50dd7` - Vylepšení zobrazení chatu a přidání funkce auditu
  - Přidán AuditController kontrolér auditu (261 řádků)
  - Přidán AuditView zobrazení auditu (379 řádků)
  - Přidán AuditViewModel model zobrazení auditu
  - ChatView zobrazení chatu výrazně vylepšeno (171 řádků vylepšeno)
  - ChatController kontrolér chatu aktualizován
  - MarkdownEditorComponent komponenta vylepšena
  - InitController kontrolér inicializace vylepšen
  - ChatSystem systém chatu přidány funkce
  - 14 souborů změněno, 1030 vložení(+), 112 odstranění(-)

- `c9babce` - Vylepšení vykreslování volání nástrojů v zobrazení chatu
  - ChatView vykreslování bloku volání nástrojů vylepšeno
  - 1 soubor změněn, 54 vložení(+), 11 odstranění(-)

#### Systém scénářů nástrojů AI
- `ff2eddd` - Implementace systému filtrování scénářů nástrojů
  - Přidán ToolScenarioAttribute atribut scénáře nástrojů (36 řádků)
  - Přidán ChatOnlyAttribute atribut scénáře pouze chat (19 řádků)
  - ToolManager správce nástrojů přidáno filtrování scénářů (40 řádků)
  - ContextManager správce kontextu přizpůsoben filtrování scénářů
  - 4 soubory změněny, 115 vložení(+), 30 odstranění(-)

- `5709a33` - Přidat atributy scénáře ke třídám nástrojů
  - 24 třídám nástrojů přidány anotace atributu ToolScenario
  - Včetně kalendáře, chatu, konfigurace, kurátora, databáze, disku, dynamické kompilace atd.
  - 24 souborů změněno, 46 vložení(+), 20 odstranění(-)

#### Refaktorizace systému úloh
- `2f19a5f` - Restrukturalizace systému úloh pomocí TaskCenter a TaskEnumerator
  - Přidán TaskCenter centrum úloh (235 řádků)
  - Přidán TaskEnumerator enumerátor úloh (297 řádků)
  - TaskSystem systém úloh refaktorován a zjednodušen
  - DefaultSiliconBeing křemíková bytost přizpůsobena nové architektuře
  - DefaultSiliconBeingFactory továrna aktualizována
  - SiliconBeingBase základní třída vylepšena
  - 7 souborů změněno, 796 vložení(+), 275 odstranění(-)

#### Migrace systému oprávnění
- `a06ed09` - Migrace IM a systému oprávnění do projektu App
  - PermissionRequestQueue migrována z Default/Fast do projektu App (443 řádků přidáno)
  - Odstraněn WebUIProvider verze Default (403 řádků odstraněno)
  - Odstraněn HelpTool verze Default (194 řádků odstraněno)
  - Odstraněny duplicitní PermissionRequestQueue z Default/Fast
  - Odstraněn IMPermissionAskHandler verze Default
  - PermissionRequestController kontrolér aktualizován
  - 14 souborů změněno, 496 vložení(+), 1183 odstranění(-)

#### Optimalizace kontextu AI
- `4c8aaff` - Optimalizace správce kontextu a vylepšení lokátoru služeb
  - ContextManager správce kontextu zjednodušen a optimalizován
  - ServiceLocator lokátor služeb vylepšen (36 řádků přidáno)
  - ToolManager správce nástrojů vylepšen (34 řádků přidáno)
  - DashScopeClient a VolcengineArkClient klienti vylepšeni
  - Exekutoři (CommandLine, Disk, Network) aktualizováni
  - 8 souborů změněno, 116 vložení(+), 98 odstranění(-)

#### Lokalizace
- `5c5eef7` - Přidat lokalizační klíče auditu a úloh
  - DefaultLocalizationBase přidáno 127 řádků lokalizačních definic
  - 9 jazyků přidány klíče související s auditem a úlohami (každý 26 řádků)
  - 11 souborů změněno, 387 vložení(+)

#### Konfigurace projektu
- `2067db6` - Aktualizace konfigurací projektu a pravidel gitignore
  - Pravidla .gitignore aktualizována
  - DefaultConfigData a Fast DefaultConfigData konfigurace vylepšeny
  - SpeedyWorkNoteStorage úložiště vylepšeno
  - SpeedyPack jádro vylepšeno
  - 5 souborů změněno, 32 vložení(+), 6 odstranění(-)

### 2026-05-07

#### Italská lokalizace
- `8adc18c` - Přidat podporu italské lokalizace a aktualizovat vícejazyčnou dokumentaci
  - Přidána it-IT italská lokalizace
  - Přidána implementace lokalizace ItIT (1909 řádků)
  - Přidán ChineseHistoricalItIT italská podpora čínského historického kalendáře (586 řádků)
  - Přidána TrayItIT italská lokalizace systémové lišty (135 řádků)
  - Přidána kompletní sada italské dokumentace (14 dokumentů: README, API reference, architektura, kalendářový systém, seznam změn, průvodce příspěvky atd.)
  - Aktualizována architektura, průvodce vývojem, průvodce začátkem atd. pro všechny jazykové verze
  - Language výčet jazyků přidána italština
  - 86 souborů změněno, 11573 vložení(+), 769 odstranění(-)

#### Synchronizace dokumentace
- `12a5deb` - Aktualizovat vícejazyčnou dokumentaci pro architekturu, seznam změn a průvodce křemíkovou bytostí
  - 8 jazyků README aktualizováno
  - 8 jazyků dokumentace architektury aktualizována
  - 8 jazyků seznam změn aktualizován
  - 8 jazyků průvodce křemíkovou bytostí aktualizován
  - 8 jazyků reference nástrojů aktualizována
  - Glosář restrukturalizován
  - 46 souborů změněno, 1697 vložení(+), 442 odstranění(-)

### 2026-05-06

#### Velká refaktorizace modulů
- `eeb3be6` - Velká refaktorizace a reorganizace modulů
  - Restrukturalizace projektu SiliconLife.App
  - Reorganizace projektu SiliconLife.Fast
  - Reorganizace projektu SiliconLife.Default
  - Reorganizace sdílených modulů SiliconLife.Common
  - Reorganizace core modulů SiliconLife.Core
  - Reorganizace úložného enginu SiliconLife.Speedy
  - Reorganizace správcovských nástrojů SiliconLife.Speedy.Manager
  - 119 souborů změněno, 6926 řádků přidáno, 3066 řádků smazáno

### 2026-05-04

#### AI klient
- `24d2c86` - Přidán VolcengineArkClient a nahrazen Audit sledováním Usage
  - Nový VolcengineArkClient Volcengine Ark AI klient
  - Podporuje streaming a non-streaming režimy
  - Integrované dvojí omezení rychlosti (klientské + serverové)
  - Kompatibilní s protokolem OpenAI API
  - Nahrazen systém Audit sledováním Usage
  - 24 souborů změněno, 802 řádků přidáno, 21 řádků smazáno

#### Systém nástrojů
- `f27650a` - Přidán nástroj hot reload pro automatický restart Fast
  - Nový nástroj HotReloadTool hot reload
  - Podporuje online kompilaci, aktualizaci a restart SiliconLife.Fast
  - Nový samostatný aktualizační program HotReload.exe
  - Bezpečný mechanismus kopírování souborů (nepřepisuje sám sebe)
  - Elegantní vypnutí a čekání na uvolnění portu
  - 9 souborů změněno, 581 řádků přidáno

#### Lokalizace
- `6a5aad8` - Aktualizovány všechny soubory a přidána francouzská lokalizace
  - Nová francouzská lokalizace fr-FR
  - Aktualizovány všechny jazykové verze
  - Francouzský překlad nápovědy
  - Francouzský překlad rozhraní
  - 100+ souborů změněno

### 2026-05-03

#### Projektová infrastruktura
- `2664b0c` - Aktualizována projektová infrastruktura a závislosti
  - SiliconLife.Speedy.Manager přidáno WPF rozhraní (MainForm.Designer.cs, MainForm.resx)
  - Přidán zdroj ikony slc.ico (1.5MB)
  - PluginLoader výrazně vylepšeno bezpečnostní skenování (622 řádků přidáno)
  - Přidána PermissionedStreamFactory továrna oprávněných streamů (779 řádků)
  - Přidána PermissionRequestQueue fronta požadavků na oprávnění (verze Default a Fast)
  - Přidán DebugLoggerProvider poskytovatel ladících logů
  - ConfigDataBase vylepšena konfigurační základní třída
  - ToolManager přidána funkce skenování pluginových nástrojů (ScanAllPluginAssemblies)
  - SiliconBeingManager vylepšena správa životního cyklu
  - DashScopeClient Alibaba Cloud AI klient výrazně vylepšen (227 řádků přidáno)
  - DefaultSiliconBeingFactory vylepšena továrna
  - Aktualizována webová zobrazení a kontroléry (ChatView, WorkNoteView, PermissionRequestController)
  - 9 jazyků lokalizace přidány nové klíče
  - 35 souborů změněno, 28080 řádků přidáno, 336 řádků smazáno

### 2026-05-02

#### Vylepšení AI klienta
- `c16f99f` - Aktualizován AI klient, Web UI a komponenty úložiště
  - DashScopeClient Alibaba Cloud klient výrazně vylepšen
  - SpeedyPackAutoCompactor optimalizace automatického kompresoru
  - Vylepšeny základní třídy webových zobrazení a BeingView
  - 6 souborů změněno, 240 řádků přidáno, 81 řádků smazáno

#### Pluginový systém
- `242dc98` - Přidán seznam pluginů na stránku O aplikaci
  - AboutController přidáno zobrazení informací o pluginech
  - AboutViewModel přidán datový model pluginů
  - AboutView přidáno vykreslení seznamu pluginů
  - 9 jazyků lokalizace přidány klíče související s pluginy
  - 14 souborů změněno, 160 řádků přidáno, 1 řádek smazán

#### AI optimalizace
- `147f8f4` - Zjednodušen text promptu kontextové paměti
  - ContextManager optimalizovány AI prompty
  - 1 soubor změněn, 1 řádek přidán, 1 řádek smazán

#### Optimalizace Speedy úložiště
- `8bda2d3` - Aktualizována implementace Speedy úložiště a kontroléru paměti
  - SpeedyPackAutoCompactor oprava intervalu
  - SpeedyTimeStorage optimalizace zpracování cest
  - MemoryController vylepšení kontroléru paměti
  - SpeedyPack.Manager aktualizace UI
  - 4 soubory změněny, 21 řádků přidáno, 18 řádků smazáno

#### Vylepšení systémové lišty
- `8972654` - Vylepšena lokalizační podpora okna stavu lišty
  - 9 jazyků lokalizace lišty přidána položka Speedy správy
  - TrayStatusWindow přidána položka menu Speedy správy
  - 11 souborů změněno, 72 řádků přidáno

#### Speedy.Manager optimalizace
- `6f5db09` - Optimalizováno UI SpeedyPack manažeru a interní komponenty
  - MainForm rekonstrukce rozhraní
  - FreeList optimalizace správy paměti
  - WriteQueue vylepšení fronty zápisu
  - SpeedyPack optimalizace jádra
  - 5 souborů změněno, 96 řádků přidáno, 88 řádků smazáno

#### Vylepšení úložného systému
- `57f9d5d` - Vylepšen úložný systém, přidána automatická komprese a podpora neúplných dat
  - Přidán SpeedyPackAutoCompactor automatický kompresní časovač (30minutový interval)
  - SpeedyPackRegistry vylepšen správce singletonu
  - SpeedyStorage, SpeedyTimeStorage, SpeedyWorkNoteStorage vylepšeny adaptéry
  - SpeedyPack přidána správa volného prostoru FreeList (149 řádků)
  - PackFileWriter refaktorizován a optimalizován zapisovač
  - WriteOperation, WriteQueue vylepšena fronta zápisu
  - SpeedyPackOptions rozšířeny konfigurační možnosti
  - IncompleteDate přidány metody porovnání
  - PluginLoader vylepšen zavaděč pluginů
  - Default a Fast verze Program.cs aktualizován inicializační proces
  - DefaultConfigData zjednodušena konfigurační data
  - KnowledgeNetwork zjednodušena znalostní síť
  - ChatController, MemoryController optimalizovány kontroléry
  - SpeedyPack.Manager MainForm vylepšena funkčnost
  - 22 souborů změněno, 639 řádků přidáno, 253 řádků smazáno

#### Speedy.Manager aktualizace
- `b04ed33` - Aktualizovány soubory Speedy.Manager

### 2026-05-01

#### Refaktoring architektury: Speedy úložiště nahrazuje LiteDB
- `6600972` - Nahrazeno Speedy úložištěm za LiteDB, přidán pluginový systém a Speedy projekt
  - **Přidán projekt SiliconLife.Speedy**: vysoce výkonný .spk úložný engine
    - SpeedyPack jádrová třída (489 řádků): mapování paměťových adresářů + mezipaměť záznamů + asynchronní fronta zápisu
    - SpeedyPackOptions konfigurační třída: TTL mezipaměti, max. položek mezipaměti, režim pouze pro čtení
    - IPackTransaction transakční rozhraní: podpora atomických operací zápisu
    - SpkFileInfo třída informací o souboru
    - Adresář Internal: DirectoryMap, EntryCache, PackFileReader, PackFileWriter, WriteQueue, WriteOperation, SpeedyTransaction, SpkHeader, PathNormalizer, FreeList
    - Závisí na MessagePack 3.1.4 pro binární serializaci (LZ4 komprese)
  - **Přidán projekt SiliconLife.Speedy.Manager**: WPF nástroj pro správu
    - MVVM architektura: MainViewModel, DirectoryTreeViewModel, ContentViewerViewModel atd.
    - Službová vrstva: PackService, FileDialogService, RecentFilesService, NotificationService
    - Konvertory: BoolToVisibility, ByteSizeToString, ContentTypeToIcon, NullToCollapsed
    - Zobrazení: MainWindow, DirectoryTreeView, ContentViewerPanel, MetadataPanel
    - Dialogy: FileInfoDialog, ImportDialog, NewEntryDialog
  - **Migrace úložiště SiliconLife.Fast**: LiteDB → SpeedyPack
    - Přidán SpeedyStorage (IStorage adaptér)
    - Přidán SpeedyTimeStorage (ITimeStorage adaptér)
    - Přidán SpeedyWorkNoteStorage (IWorkNoteStorage adaptér)
    - Přidán SpeedyPackRegistry (správa singletonu na úrovni procesu)
    - Přidán SpeedyPackAutoCompactor (automatický kompresní časovač)
    - Odstraněny implementace úložiště související s LiteDB (LiteDBStorage, LiteDBTimeStorage, LiteDBWorkNoteStorage, LiteDBLoggerProvider, LiteDBManager, LiteDBModels)
    - Odstraněn kód související s oknem správy LiteDB
  - **Pluginový systém**:
    - Přidáno rozhraní IPlugin (Core/Plugins/IPlugin.cs)
    - Přidán zavaděč pluginů PluginLoader (Core/Plugins/PluginLoader.cs)
    - Podpora načítání pluginových DLL z adresáře
    - Bezpečnostní skenování: kontrola zakázaných jmenných prostorů (System.IO, System.Net, Microsoft.CodeAnalysis atd.)
    - Seznam důvěryhodných sestavení (Google.Protobuf, Newtonsoft.Json, MessagePack atd.)
    - Izolované načítání s vlastním AssemblyLoadContext
    - ToolManager přidána metoda ScanAllPluginAssemblies
    - CoreHost integrován zavaděč pluginů
  - 119 souborů změněno, 6926 řádků přidáno, 3066 řádků smazáno

#### Vylepšení křemíkových bytostí
- `3aef4c3` - Přidán stav aktivity Stopped a vylepšeno zpracování chyb
  - Křemíkové bytosti přidán stav Stopped
  - Vylepšeno zpracování chyb a mechanismus obnovy

#### Aktualizace lokalizace
- `513c65d` - Aktualizovány všechny jazykové verze a dokumentace
  - Přidána komponenta MarkdownEditorComponent (625 řádků)
  - Přidána komponenta DetailsComponent (130 řádků)
  - Přidána komponenta AccordionComponent (285 řádků)
  - Aktualizovány kontroléry BeingController, ChatController, MemoryController, PermissionController
  - Refaktorizována zobrazení BeingView, ChatView, MemoryView, SoulEditorView
  - Odstraněno staré MarkdownEditorView
  - Migrace komponentizace InitController
  - 115 souborů změněno, 5761 řádků přidáno, 2362 řádků smazáno

### 2026-04-30

#### Funkcionalita systémové lišty
- `101b203` - Implementováno okno stavu lišty a ApplicationContext
  - Přidány zdroje ikon lišty (alpha.png, noWord.png, slc.ico, wordIcon.png)
  - Implementováno stavové okno TrayStatusWindow
  - Podpora lokalizace lišty v 9 jazycích (TrayCsCZ, TrayDeDE, TrayEnUS atd.)
  - Abstraktní základní třída TrayLocalizationBase
  - 24 souborů změněno, 27995 řádků přidáno, 1 řádek smazán (včetně zdrojových souborů)

#### Komponentová architektura UI
- `e61cfaa` - Dokončena komponentová architektura UI, implementováno 24 komponent
  - Fáze MVP (8): ComponentBase, Div, Span, Button, Input, Form, Select, Label
  - Fáze 2 (6): Accordion, Card, Tabs, Table, Modal, Message
  - Fáze 3 (5): Calendar, Tree, Chart, FileUpload, RichText
  - Přidány pomocné třídy Js, Behavior, DomUpdate
  - 25 souborů změněno, 2666 řádků přidáno

- `7449e51` - Vylepšen systém komponent a přidány nové skinové motivy
  - Vylepšeny komponenty A, Button, Div, Form, Input
  - Přidány 3 skinové motivy: HighContrast, Light, Minimal
  - Aktualizovány existující skiny (Admin, Chat, Creative, Dev)
  - Migrace komponentizace InitController
  - 32 souborů změněno, 1466 řádků přidáno, 1238 řádků smazáno

- `1ba8636` - Zahájena migrace komponentizace InitController (v průběhu)
  - 9 souborů změněno, 574 řádků přidáno, 145 řádků smazáno

#### Sjednocení úložného systému
- `895dff9` - Sjednoceny soul.md a state.json pro použití rozhraní IStorage
  - DefaultSiliconBeing používá IStorage pro čtení/zápis souborů duše a stavu
  - Přidán správce stavových souborů StateFileManager
  - Refaktoring SoulFileManager pro přizpůsobení IStorage
  - 8 souborů změněno, 201 řádků přidáno, 116 řádků smazáno

#### Rozšíření správy LiteDB
- `a34bef4` - Přidán LiteDBManager a rozšířena lokalizace lišty
  - Přidána položka správy LiteDB do menu lišty
  - Aktualizována lokalizace lišty v 9 jazycích
  - 10 souborů změněno, 196 řádků přidáno

- `c4a79ca` - Přidána jazykově vědomá lokalizační továrna pro okno správy LiteDB
  - 1 soubor změněn, 78 řádků přidáno

- `5ebc55e` - Převedena LiteDBAdminLocalization na abstraktní základní třídu
  - 10 souborů změněno, 1356 řádků přidáno

#### Oprava konfiguračního systému
- `2da5256` - Přidána abstraktní metoda ConfigExists a opraveny duplicitní konfigurační záznamy LiteDB
  - ConfigDataBase přidal metodu ConfigExists
  - Verze Fast DefaultConfigData implementuje kontrolu existence konfigurace LiteDB
  - Opraven problém s duplicitními konfiguračními klíči LiteDB
  - 9 souborů změněno, 210 řádků přidáno, 2 řádky smazány

#### Optimalizace chatu a zobrazení
- `d3618ec` - Optimalizovány chatovací relace, úložný systém, časový model a základní třídy zobrazení
  - Optimalizace BroadcastChannel, GroupChatSession, SingleChatSession
  - ITimeStorage přidal metody dotazování
  - Synchronizované aktualizace FileSystemStorage a LiteDBStorage
  - Optimalizace refaktoringu ViewBase (verze Default a Fast)
  - 11 souborů změněno, 622 řádků přidáno, 392 řádků smazáno

### 2026-04-29

#### Refaktoring architektury: Extrakce sdílených modulů
- `a102428` - Migrovány sdílené moduly z SiliconLife.Default do SiliconLife.Common
  - Extrahováno 32 implementací kalendáře do projektu Common
  - Extrahovány základní třídy lokalizace a 21 jazykových implementací do projektu Common
  - Extrahován správce oprávnění a výchozí implementace silicon being do projektu Common
  - Extrahováno 23 implementací vestavěných nástrojů do projektu Common
  - Extrahována implementace Playwright WebView do projektu Common
  - Aktualizován namespace na SiliconLife.Collective
  - 122 souborů změněno, 586 řádků přidáno, 343 řádků smazáno

#### Zlepšení kvality kódu
- `17566fe` - Nahrazen Console.WriteLine systémem logování v projektech Core, Common a Default
  - Aktualizováno ContextManager, AuditLogger, DefaultConfigData a 6 dalších souborů
  - Jednotné použití rozhraní ILogger, zlepšení udržovatelnosti kódu
  - 6 souborů změněno, 12 řádků přidáno, 8 řádků smazáno

#### Verze SiliconLife.Fast s vysokým výkonem
- `54a0307` - Přidán projekt SiliconLife.Fast a dokončeny opravy kompilace
  - Úplný vstupní bod aplikace Windows Forms
  - Podpora systémové lišty (NotifyIcon)
  - Portovány všechny Web UI kontroléry (20+)
  - Portovány všechny Web zobrazovací komponenty
  - Portovány 4 skinové motivy (Admin, Chat, Creative, Dev)
  - 125 souborů změněno, 61186 řádků přidáno

#### Synchronizace vícejazyčné dokumentace
- `265fde8` - Dokumentace architektury duální verze synchronizována do všech jazyků
  - Aktualizovány architecture.md, changelog.md v 7 jazycích
  - Aktualizován contributing.md v 6 jazycích
  - Aktualizovány getting-started.md, roadmap.md v 7 jazycích
  - 47 souborů změněno, 1214 řádků přidáno, 38 řádků smazáno

#### Úložný systém LiteDB (verze Fast)
- `4704862` - Přidány závislosti a infrastruktura LiteDB
  - Přidána správcovská třída LiteDBManager
  - Přidány datové modely LiteDBModels
  - 3 soubory změněny, 252 řádků přidáno

- `4220036` - Implementovány třídy úložiště LiteDB
  - LiteDBStorage: implementuje rozhraní IStorage
  - LiteDBTimeStorage: implementuje rozhraní ITimeStorage
  - LiteDBWorkNoteStorage: implementuje rozhraní IWorkNoteStorage
  - 3 soubory změněny, 581 řádků přidáno

- `38ebd23` - Migrován konfigurační a logovací systém na LiteDB
  - DefaultConfigData přizpůsoben úložišti LiteDB
  - Přidán poskytovatel logování LiteDBLoggerProvider
  - 2 soubory změněny, 203 řádků přidáno, 67 řádků smazáno

- `e687157` - Migrována znalostní síť ze souborového systému na LiteDB
  - KnowledgeNetwork kompletně refaktoringován, používá LiteDB pro ukládání trojic dat
  - 1 soubor změněn, 231 řádků přidáno, 72 řádků smazáno

- `4220169` - Integrováno úložiště LiteDB do Program a ProjectManager
  - Program.cs inicializuje úložiště LiteDB
  - ProjectManager přizpůsoben úložišti pracovních poznámek LiteDB
  - 2 soubory změněny, 40 řádků přidáno, 17 řádků smazáno

- `5f3a709` - Odstraněny zastaralé implementace úložiště souborového systému
  - Smazány FileSystemLoggerProvider, FileSystemStorage, FileSystemTimeStorage atd.
  - 6 souborů změněno, 1518 řádků smazáno

- `e1a4ef2` - docs: add v0.1.0-alpha version identifier to all documentation
  - 127 souborů změněno, 2297 řádků přidáno, 2471 řádků smazáno

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### Refaktoriza systému úložiště
- `8dd26e3` - Sjednoceno rozhraní ITimeStorage pro použití IncompleteDate a přidáno hierarchické dotazovací API
  - Odstraněny přetížené metody DateTime z rozhraní ITimeStorage, sjednoceno pro použití IncompleteDate
  - Přidána metoda porovnání CompareTo(DateTime) a metoda rozšíření Expand() do IncompleteDate
  - Přidáno hierarchické dotazovací API GetEarliestTimestamp(), GetLatestTimestamp()
  - Přidány metody HasSummary() a QueryWithLevel(), podporující dotazy podle časové úrovně
  - Memory.cs refaktorizován kompresní algoritmus, použití nového hierarchického dotazovacího API pro zlepšení efektivity
  - FileSystemTimeStorage.cs kompletně implementuje nové metody rozhraní
  - Synchronizované aktualizace všech volajících: ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord, atd.
  - Aktualizace systému nástrojů: HelpTool, LogTool, TokenAuditTool přizpůsobeny novému rozhraní
  - Aktualizace webových kontrolerů: AuditController, ChatController, ChatHistoryController přizpůsobeny novému rozhraní
  - 41 souborů změněno, 1820 řádků přidáno, 903 řádků smazáno

### 2026-04-27

#### Vylepšení systému nápovědní dokumentace
- `9989d79` - Aktualizována lokalizace, systém nápovědy a webová zobrazení
  - Přidáno rozhraní nápovědní dokumentace továrny AI klienta IAIClientFactoryHelp.cs
  - Dokončen překlad všech nápovědních dokumentů do 9 jazyků
  - HelpTopics.cs přidáno 40 definic témat nápovědy
  - Webová zobrazení komplexně aktualizována: InitController, AuditView, ConfigView, KnowledgeView, LogView, atd.
  - Vylepšení systému lokalizace: všechny jazykové verze přidány nové lokalizační klíče
  - Aktualizace továrny AI klienta: vylepšení DashScopeClientFactory, OllamaClientFactory
  - 30 souborů změněno, 10086 řádků přidáno, 15 řádků smazáno

#### Nový obsah nápovědní dokumentace
- `e7afe94` - Přidána nápovědní dokumentace pro soubory duše a auditní záznamy
  - Přidána nápovědní dokumentace pro správu souborů duše
  - Přidána nápovědní dokumentace pro auditní záznamy
  - HelpTopics.cs přidány definice témat
  - HelpView.cs výrazně refaktorizován, zlepšena logika vykreslování dokumentů
  - PermissionView.cs refaktorizován, zlepšeno rozhraní správy oprávnění
  - Vylepšení hlavního modulu: vylepšení SiliconBeingManager, TaskSystem, ToolManager
  - TaskTool.cs refaktorizován, zlepšena funkce správy úkolů
  - Webová zobrazení komplexně aktualizována: všechny komponenty zobrazení synchronizovány
  - HelpController.cs zjednodušen, optimalizována logika kontroléru
  - 30 souborů změněno, 7100 řádků přidáno, 897 řádků smazáno

### 2026-04-26

#### Systém nápovědní dokumentace
- `07895d7` - Vylepšen systém nápovědní dokumentace, přidány 3 dokumenty a dokončen překlad do 9 jazyků
  - Přidán průvodce použitím systému paměti, konfigurace instalace Ollama, platformy Alibaba Cloud DashScope
  - Dokončen překlad všech 10 nápovědních dokumentů do 9 jazyků
  - Zjednodušena logika vykreslování HelpView
  - 18 souborů změněno, 14418 řádků přidáno, 1364 řádků smazáno

#### Německá lokalizace
- `0cfd8a1` - Přidána kompletní podpora německé (de-DE) lokalizace
  - Kompletní soubory německé lokalizace
  - Přidána německá podpora pro čínský historický kalendář
  - Přidán německý překlad nápovědní dokumentace
  - Plně synchronizovány všechny dokumenty v 9 jazycích
  - 135 souborů změněno, 26186 řádků přidáno, 14371 řádků smazáno

#### Synchronizace dokumentace
- `3aada7d` - Synchronizována dokumentace v tradiční čínštině (zh-HK) se zjednodušenou čínštinou
  - 3 soubory změněny, 519 řádků přidáno, 422 řádky smazány
- `2f6abff` - Přidána lokalizace zobrazovaných názvů nástrojů nápovědy pro všechny jazyky
  - 7 souborů změněno, 47 řádků přidáno, 7 řádků smazáno

#### Refaktorizace systému znalostí
- `60944fe` - Sjednocen jmenný prostor na SiliconLife.Collective
  - 8 souborů změněno, 5 řádků přidáno, 8 řádků smazáno
- `69c51c5` - Přidán systém nápovědní dokumentace a přeloženy komentáře kódu do angličtiny
  - 29 souborů změněno, 3385 řádků přidáno, 22 řádky smazány

### 2026-04-25

#### Automatizace prohlížeče WebView
- `41757c3` - Implementována cross-platformní automatizace prohlížeče WebView založená na Playwright
  - 6 souborů změněno, 1152 řádků přidáno

#### Aktualizace dokumentace
- `0ff797b` - Přidána dokumentace KnowledgeTool a WorkNoteTool (7 jazyků)
  - 28 souborů změněno, 4983 řádků přidáno
- `ad77415` - Aktualizovány všechny soubory changelog, přidána historie Git z 2026-04-25
  - 7 souborů změněno, 168 řádků přidáno

#### Správa pracovního prostoru projektu
- `785c551` - Implementována správa pracovního prostoru projektu s pracovními poznámkami a systémem úkolů
  - Přidán systém správy pracovního prostoru projektu
  - Funkce pracovních poznámek pro sledování postupu projektu
  - Integrace systému správy úkolů
  - 29 souborů změněno, 4256 řádků přidáno, 36 řádků smazáno

#### Česká lokalizace
- `b4bbf39` - Přidána kompletní česká (cs-CZ) lokalizace a aktualizována veškerá jazyková dokumentace
  - 116 souborů změněno, 4933 řádků přidáno, 222 řádky smazány
- `faf078f` - Opraveny chyby kompilace české lokalizace
  - 3 soubory změněny, 910 řádků přidáno, 1 řádek smazán

#### Vylepšení systému znalostí
- `20adaac` - Přidán KnowledgeTool s kompletní podporou lokalizace
  - 34 souborů změněno, 2331 řádků přidáno, 56 řádků smazáno

### 2026-04-24

#### Vylepšení správy paměti
- `c7b2ecc` - Vylepšena správa paměti s pokročilým filtrováním, statistikami a podrobnými zobrazeními
  - Přidáno pokročilé filtrování paměti
  - Implementovány statistiky paměti
  - Přidána stránka podrobného zobrazení paměti
  - Podpora vícejazyčné lokalizace (6 jazyků)
  - 13 souborů změněno, 840 řádků přidáno, 86 řádků smazáno

#### Rozšíření systému oprávnění
- `4489ad6` - Přidána meteorologická služba wttr.in na seznam povolených sítí
  - Kompletní synchronizace vícejazyčné dokumentace (6 jazyků)
  - 14 souborů změněno, 417 řádků přidáno, 1 řádek smazán

#### Opravy webového rozhraní
- `d9d72e9` - Opraven problém priority CSS v modálním okně detailu pracovní poznámky
  - 19 souborů změněno, 1744 řádků přidáno, 6 řádků smazáno

#### Optimalizace historie chatu
- `0df599c` - Opraveno vykreslování výsledků nástrojů jako samostatných zpráv chatu
  - 1 soubor změněn, 222 řádků přidáno, 21 řádků smazáno
- `057b09d` - Optimalizováno zobrazení detailů historie chatu, zlepšeno vykreslování volání nástrojů
  - 3 soubory změněny, 389 řádků přidáno, 68 řádků smazáno

#### Historie provádění časovače
- `fa3f06f` - Přidána funkce historie provádění časovače s podrobným zobrazením
  - 8 souborů změněno, 937 řádků přidáno, 10 řádků smazáno
- `d824835` - Přidány lokalizační klíče historie provádění časovače (všechny jazyky)
  - 7 souborů změněno, 88 řádků přidáno

#### Vylepšení lokalizace
- `c13cb17` - Registrována jazyková varianta španělštiny
  - 1 soubor změněn, 4 řádky přidáno
- `9c44f34` - Přidána podpora vícejazyčné lokalizace pro čínský historický kalendář
  - 16 souborů změněno, 6049 řádků přidáno, 1 řádek smazán

#### Vylepšení hlavní funkčnosti
- `1e7c7b2` - Zlepšena komprese paměti a sledování provádění nástrojů
  - 4 soubory změněny, 338 řádků přidáno, 86 řádků smazáno

### 2026-04-23

#### Lokalizace nástrojů
- `192fc6e` - Přidána chybějící lokalizace názvů nástrojů pro 5 nástrojů
  - 6 souborů změněno, 30 řádků přidáno

#### Aktualizace dokumentace
- `882c08f` - Aktualizovány všechny soubory changelog, přidána kompletní historie Git a odstraněna falešná čísla verzí
  - 45 souborů změněno, 8815 řádků přidáno, 1611 řádků smazáno

#### Vylepšení stránky chatu
- `65c157b` - Přidán indikátor načítání na stránku chatu a automatický výběr relace kurátora
  - 10 souborů změněno, 211 řádků přidáno, 7 řádků smazáno

#### Funkce historie chatu
- `e483348` - Implementována funkce zobrazení historie chatu křemíkové bytosti
  - Přidán ChatHistoryController
  - Vytvořen ChatHistoryViewModel
  - Implementovány stránky ChatHistoryListView a ChatHistoryDetailView
  - Přidány lokalizační klíče pro historii chatu (5 jazyků)
  - 12 souborů změněno, 1178 řádků přidáno

#### Vylepšení řízení toku AI
- `30a2d4e` - Vylepšeno rušení toku AI, integrace IM a inicializace hlavního hostitele
  - 11 souborů změněno, 387 řádků přidáno, 12 řádků smazáno

#### Fronta zpráv chatu
- `db48c51` - Přidána fronta zpráv chatu, metadata souborů a podpora rušení streamu
  - 4 soubory změněny, 357 řádků přidáno

#### Podpora nahrávání souborů
- `28fb344` - Implementován dialog zdrojů souborů a podpora nahrávání souborů
  - 3 soubory změněny, 1100 řádků přidáno, 2 řádky smazány
- `1d3e2cc` - Přidány lokalizační řetězce dialogu zdrojů souborů (6 jazyků)
  - 6 souborů změněno, 30 řádků přidáno

#### Aktualizace dokumentace
- `8111e92` - Přidán odkaz Wiki do sekce repozitáře README
  - 1 soubor změněn, 3 řádky přidáno, 1 řádek smazán

### 2026-04-22

#### Lokalizace dokumentace
- `66c11eb` - Přeloženy čínské komentáře do angličtiny a aktualizovány všechny changelogy
  - 11 souborů změněno, 373 řádků přidáno, 163 řádky smazány

#### Vylepšení zpráv SSE
- `b574b2b` - Přidáno senderName do historických zpráv pro identifikaci AI
  - 1 soubor změněn, 9 řádků přidáno

#### Funkce chatu
- `601fc14` - Přidána akce mark_read pro označení konce relace
  - 7 souborů změněno, 196 řádků přidáno, 36 řádků smazáno

#### Optimalizace systému nástrojů
- `7a03a19` - Zlepšena flexibilita dotazů na konverzaci LogTool
  - 1 soubor změněn, 57 řádků přidáno, 24 řádky smazány

#### Vylepšení lokalizace
- `0a8d750` - Přidán běžný systémový prompt pro aktivní chování křemíkové bytosti
  - 8 souborů změněno, 460 řádků přidáno, 48 řádků smazáno

#### Refaktorizace systému protokolů
- `2b771f3` - Odpojen LogController od souborového I/O, přidáno API pro čtení protokolů
  - 4 soubory změněny, 172 řádků přidáno, 137 řádků smazáno
- `12da302` - Přidán filtr křemíkové bytosti do zobrazení protokolů
  - 9 souborů změněno, 147 řádků přidáno, 10 řádků smazáno
- `8f6cb1e` - Přidán parametr beingId do rozhraní ILogger, implementováno oddělení protokolů systému/křemíkové bytosti
  - 47 souborů změněno, 524 řádků přidáno, 490 řádků smazáno

#### Vylepšení systému oprávnění
- `4c747ad` - Refaktorizovány PermissionTool, ExecuteCodeTool, přidáno API EvaluatePermission
  - 18 souborů změněno, 680 řádků přidáno, 492 řádků smazáno

#### Opravy chyb
- `1c96e99` - Opraveno selhání vyhledávání v kořenovém adresáři search_files a search_content
  - 1 soubor změněn, 98 řádků přidáno, 41 řádek smazán

#### Integrace nástrojů
- `135710d` - Odstraněn SearchTool, místní vyhledávání přesunuto do DiskTool
  - 2 soubory změněny, 185 řádků přidáno, 365 řádků smazáno

#### Rozšíření systému nástrojů
- `70ce7fb` - Implementován DatabaseTool pro strukturované dotazy na databázi
  - 1 soubor změněn, 382 řádky přidáno
- `be29a09` - Implementován LogTool pro dotazy na historii operací a konverzací
  - 1 soubor změněn, 298 řádků přidáno
- `4ea7702` - Implementován PermissionTool pro dynamickou správu oprávnění
  - 1 soubor změněn, 457 řádků přidáno
- `1384ff4` - Implementován ExecuteCodeTool pro více-jazykové provádění kódu
  - 1 soubor změněn, 477 řádků přidáno
- `82d1e11` - Implementován SearchTool pro vyhledávání informací
  - 1 soubor změněn, 363 řádky přidáno

#### Optimalizace webového rozhraní
- `0675c45` - Optimalizováno zvýraznění bloků kódu markdown v náhledovém panelu
  - 1 soubor změněn, 4 řádky přidáno, 23 řádky smazány
- `702b3f3` - Vylepšeno zobrazení úkolů se stavovými odznaky a zobrazením metadat
  - 8 souborů změněno, 221 řádků přidáno, 9 řádků smazáno
- `6ed9a79` - Zlepšeno ukládání zpráv chatu a vykreslování zobrazení
  - 8 souborů změněno, 140 řádků přidáno, 29 řádků smazáno

### 2026-04-21

#### Opravy chyb
- `c6b518b` - Opraveno doručování zpráv časovače a ukládání zpráv chatu
  - 3 soubory změněny, 297 řádků přidáno, 124 řádky smazány

#### Správa konfigurace
- `4305769` - Přidán .gitattributes pro správu konců řádků
  - 1 soubor změněn, 32 řádky přidáno

#### Vylepšení webového rozhraní
- `188c6f8` - Registrována trasa API seznamu úkolů a přidáno zobrazení prázdného stavu
  - 2 soubory změněny, 35 řádků přidáno, 2 řádky smazány
- `634e8ca` - Přidán odkaz na návrat na seznam stránky oprávnění
  - 1 soubor změněn, 16 řádků přidáno
- `6ba591d` - Přidán nezávislý editor konfigurace AI pro křemíkové bytosti
  - 11 souborů změněno, 842 řádků přidáno, 18 řádků smazáno
- `0a826f5` - Přidán prompt úspěšného uložení v editoru kódu
  - 1 soubor změněn, 9 řádků přidáno, 2 řádky smazány
- `2940373` - Vylepšeno webové rozhraní s nápovědami při najetí na kód a vylepšeními UI
  - 11 souborů změněno, 1054 řádků přidáno, 75 řádků smazáno

#### Opravy systému oprávnění
- `592c7ab` - Opravena inicializace callbacku a pořadí registrace
  - 2 soubory změněny, 38 řádků přidáno, 7 řádků smazáno

#### Vylepšení zabezpečení
- `833ead2` - Přidána ověření referencí sestavení pro dynamickou kompilaci
  - 4 soubory změněny, 135 řádků přidáno, 8 řádků smazáno

#### Vylepšení systému oprávnění
- `5879621` - Přidáno ověření předkompilace callbacku oprávnění a vylepšené zpracování chyb
  - 21 souborů změněno, 617 řádků přidáno, 26 řádků smazáno

#### Aktualizace dokumentace
- `4dbf659` - Aktualizován changelog na v0.5.1, nahrazeny URL zástupných symbolů GitHub, přidán zrcadlový server Gitee, lokalizován název Bilibili podle jazyka, aktualizován email
  - 32 souborů změněno, 489 řádků přidáno, 180 řádků smazáno

#### Konfigurace a vstup
- `0fc1693` - Aktualizován vstup programu a konfigurace projektu
  - 2 soubory změněny, 7 řádků přidáno

#### Refaktorizace systému oprávnění
- `ea9179a` - Vylepšena implementace systému oprávnění
  - 5 souborů změněno, 358 řádků přidáno, 152 řádků smazáno

#### Opravy chyb
- `928a96d` - Opravena implementace výpočtu kalendáře
  - 4 soubory změněny, 12 řádků přidáno, 12 řádků smazáno

#### AI a kalendář
- `646813e` - Vylepšena implementace továrny AI klienta
  - 2 soubory změněny, 21 řádků přidáno, 20 řádků smazáno

#### Lokalizace
- `7940d9c` - Přidána podpora korejské lokalizace
  - 7 souborů změněno, 2424 řádků přidáno, 10 řádků smazáno
- `4ff98ad` - Refaktorizována dokumentace pro podporu více jazyků
  - 81 souborů změněno, 23818 řádků přidáno, 1886 řádků smazáno

### 2026-04-20

#### Dokončení hlavní funkčnosti
- `28905b5` - Kompletní podpora více jazyků, továrna AI klienta, systém oprávnění a nastavení lokalizace
  - Systém protokolů s manažerem, záznamy a různými úrovněmi protokolů
  - Systém auditu tokenů pro dotazování a sledování používání tokenů
  - Továrny AI klienta pro automatické objevování různých AI platforem
  - Systém callbacku oprávnění s vlastním úložištěm
  - Implementace konzolového protokolovače
  - Podpora více jazyků pro angličtinu a zjednodušenou čínštinu
  - WebUI messenger s WebSocket pro chat v reálném čase
  - Vylepšená výchozí křemíková bytost s lokalizací
  - 39 souborů změněno, 4670 řádků přidáno, 175 řádků smazáno

### 2026-04-19

#### Časovač a kalendář
- `c933fd8` - Aktualizována lokalizace, systém časovače, webová zobrazení a přidány nástroje
  - Lepší manažer lokalizace
  - Systém plánování pro časované úkoly
  - Konfigurace AI a správa kontextu
  - Nástroj kalendáře podporující 32 typů kalendáře
  - Webový kontrolér pro API kalendáře
  - Nástroj správy úkolů
  - 46 souborů změněno, 4018 řádků přidáno, 975 řádků smazáno

**Vylepšení architektury**
- Znovu navržena architektura webového zobrazení pro lepší podporu skinů
- Vylepšen systém správy bytostí s lepším zpracováním stavu

### 2026-04-18

- `9f585e1` - Aktualizována lokalizace, systém časovače, webová zobrazení a přidány nástroje
  - Vylepšení časovače a plánování
  - Lepší webová zobrazení s vylepšenými komponentami UI
  - Více implementací nástrojů
  - 57 souborů změněno, 3328 řádků přidáno, 389 řádků smazáno

### 2026-04-17

- `9b71fcd` - Aktualizovány hlavní moduly, přidána dokumentace zh-HK, vysílací kanál, nástroje konfigurace a auditní webová zobrazení
  - Vysílací kanál pro více křemíkových bytostí chatujících společně
  - Systém nástrojů konfigurace
  - Auditní webová zobrazení
  - Dokumentace v tradiční čínštině
  - 42 souborů změněno, 3533 řádků přidáno, 268 řádků smazáno

### 2026-04-16

- `5040f05` - Aktualizovány hlavní a výchozí moduly
  - Optimalizace modulů a opravy chyb
  - Aktualizace a vylepšení implementace
  - 58 souborů změněno, 9916 řádků přidáno, 111 řádků smazáno

### 2026-04-15

- `3efab5f` - Aktualizovány více modulů: AI, Chat, IM, Tools, Web, Localization, Storage
  - Vylepšení AI klienta
  - Vylepšení systému chatu
  - Aktualizace poskytovatele messengeru
  - Optimalizace systému nástrojů
  - Vylepšení webové infrastruktury
  - Optimalizace lokalizace
  - Aktualizace systému úložiště
  - 33 souborů změněno, 788 řádků přidáno, 232 řádků smazáno

### 2026-04-14

- `4241a2f` - Funkce chatu základně dokončeny, optimalizace nahrávání UI
  - Funkce systému chatu dokončeny
  - Optimalizace UI pro nahrávání souborů
  - 16 souborů změněno, 1234 řádků přidáno, 102 řádky smazány

### 2026-04-13

- `c498c31` - Aktualizace kódu
  - Obecná vylepšení a optimalizace kódu
  - 32 soubory změněny, 1045 řádků přidáno, 546 řádků smazáno

### 2026-04-12

#### Dokumentace a lokalizace
- `2161002` - Refaktorizována dokumentace a vylepšena lokalizace
  - 17 souborů změněno, 982 řádků přidáno, 92 řádky smazány
- `03d94e4` - Vylepšen systém konfigurace a lokalizace
  - 25 souborů změněno, 1378 řádků přidáno, 154 řádky smazány
- `9976a35` - Přidána stránka o projektu a lokalizace
  - 14 souborů změněno, 699 řádků přidáno, 44 řádky smazány

#### Chat a webová zobrazení
- `0c8ccfc` - Vylepšen systém chatu, lokalizace a webová zobrazení
  - 13 souborů změněno, 402 řádků přidáno, 56 řádků smazáno
- `a8f1342` - Znovu navržena vrstva webové komunikace, přepnuto z WebSocket na SSE
  - 27 souborů změněno, 793 řádků přidáno, 935 řádků smazáno

### 2026-04-11

#### Systém protokolů
- `e8fe259` - Přidán systém protokolů a optimalizace kódu
  - 37 souborů změněno, 624 řádků přidáno, 91 řádků smazáno
- `f01c519` - Přidán systém protokolů, aktualizováno rozhraní AI a webová zobrazení
  - 31 souborů změněno, 1758 řádků přidáno, 63 řádky smazány

### 2026-04-10

- `4962924` - Vylepšen obslužný program WebSocket, zobrazení chatu a interakce messengeru
  - Vylepšení správce kontextu
  - Vylepšení systému chatu
  - Aktualizace rozhraní poskytovatele messengeru
  - Znovu navržen poskytovatel WebUI
  - Aktualizace sestavovače JavaScriptu a směrovače
  - Optimalizace zobrazení chatu
  - Vylepšení obslužného programu WebSocket
  - 9 souborů změněno, 365 řádků přidáno, 134 řádky smazány

### 2026-04-09

- `f9302bf` - Vylepšeno rozhraní poskytovatele messengeru, systém chatu a interakce webového UI
  - Rozšíření rozhraní poskytovatele messengeru
  - Vylepšení zpráv chatu a systému
  - Optimalizace správce kontextu
  - Vylepšení výchozí křemíkové bytosti
  - Vylepšení zobrazení chatu webového UI
  - Aktualizace obslužného programu WebSocket
  - 10 souborů změněno, 427 řádků přidáno, 93 řádky smazány

### 2026-04-07

- `6831ee8` - Znovu navržena webová zobrazení a sestavovač JavaScriptu
  - Kompletní znovu návrh webových kontrolérů
  - Kompletní přepsání sestavovače JavaScriptu
  - Všechny komponenty zobrazení aktualizovány
  - Vylepšení systému skinů
  - Upgrade architektury základní třídy zobrazení
  - 23 soubory změněny, 2004 řádků přidáno, 1983 řádky smazány

### 2026-04-05

- `41e97fb` - Aktualizovány více hlavních modulů a webové kontroléry
  - Vylepšení správce kontextu
  - Systém chatu a správa relací
  - Znovu navržen lokátor služeb
  - Aktualizace základní třídy a správce křemíkové bytosti
  - Webové kontroléry komplexně aktualizovány (17 kontrolérů)
  - Vylepšení továrny výchozí křemíkové bytosti
  - 31 souborů změněno, 681 řádků přidáno, 326 řádků smazáno
- `67988d4` - Vylepšen modul webového UI, přidáno zobrazení exekutoru, vyčištěna zobrazení a hlavní moduly
  - 61 souborů změněno, 3148 řádků přidáno, 3726 řádků smazáno

### 2026-04-04

- `b58bb1c` - Přidán inicializační kontrolér a znovu navržen webový modul
  - Inicializační kontrolér
  - Znovu navržen modul konfigurace
  - Aktualizace modulu lokalizace
  - Vylepšení systému skinů
  - Vylepšení směrovače
  - 29 souborů změněno, 1269 řádků přidáno, 289 řádků smazáno
- `f03ac0b` - Přidán modul webového UI, vylepšena funkce messengeru
  - 60 souborů změněno, 8481 řádků přidáno, 165 řádků smazáno

### 2026-04-03

- `192e57b` - Aktualizována struktura projektu a hlavní runtime komponenty
  - 22 soubory změněny, 446 řádků přidáno, 179 řádků smazáno
- `59faec8` - Aktualizace hlavní a výchozí implementace
  - 25 souborů změněno, 3056 řádků přidáno, 18 řádků smazáno
- `d488485` - Přidána funkce dynamické kompilace a modul nástrojů kurátora
  - 19 souborů změněno, 1727 řádků přidáno, 11 řádků smazáno
- `753d1d9` - Přidán modul zabezpečení, aktualizovány exekutory, poskytovatelé messengeru, lokalizace a nástroje
  - 29 souborů změněno, 2352 řádků přidáno, 93 řádků smazáno
- `a378697` - Dokončena fáze 5 - systém nástrojů + exekutory
  - 41 souborů změněno, 2651 řádků přidáno, 363 řádky smazány

### 2026-04-02

- `e6ad94b` - Opraveno selhání načítání historie chatu při odstraňování konfiguračních souborů během testování
  - 4 soubory změněny, 49 řádků přidáno, 45 řádků smazáno
- `daa56f5` - Dokončena fáze 4: perzistentní paměť (systém chatu + kanál messengeru)
  - 29 souborů změněno, 2051 řádků přidáno, 538 řádků smazáno

### 2026-04-01

- `bbe2dbb` - Opraveno načítání konfigurace a směrování zpráv služby chatu
  - 27 souborů změněno, 1633 řádků přidáno, 147 řádků smazáno
- `2fa6305` - Implementována fáze 2: rámec hlavní smyčky a systém objektů hodin
  - 9 souborů změněno, 594 řádků přidáno, 41 řádek smazán
- `32b99a1` - Implementována fáze 1 - základní funkce chatu
  - 19 souborů změněno, 1185 řádků přidáno
- `358e368` - Počáteční commit: dokumentace projektu a licence
  - 10 souborů změněno, 1873 řádků přidáno