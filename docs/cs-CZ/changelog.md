# Změny

[English](../en/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Deutsch](../de-DE/changelog.md) | **Čeština**

Všechny významné změny tohoto projektu budou zaznamenány v tomto souboru.

Formát je založen na [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
tento projekt dodržuje [Semantické verzování](https://semver.org/spec/v2.0.0.html).

**POZNÁMKA: Tento projekt ještě nevydal žádnou oficiální verzi, veškerý obsah je ve vývoji.**

---

## O tomto seznamu změn

### Původ projektu

- Tento projekt vznikl 20. března 2026.
- Před tímto projektem selhal ověřovací demo kvůli nevhodné architektuře, což znemožnilo integraci s více AI platformami.

### Použité AI IDE nástroje

#### Kiro (Amazon AWS)
- Projekt byl zpočátku udržován pomocí Kiro a spuštěn v režimu Spec.
- Kiro je agentic AI vývojové prostředí vytvořené Amazon AWS.
- Založeno na Code OSS (VS Code), podporuje nastavení VS Code a rozšíření kompatibilní s Open VSX.
- Má specifikace-řízený vývojový pracovní postup pro strukturované AI kódování.

#### Comate AI IDE / 文心快码 (Baidu)
- Příležitostně používán pro copywriting a dokumentační práci.
- Comate AI IDE je AI nativní vývojové prostředí nástroj vydaný Baidu Wenxin 23. června 2025.
- První multimodální, multi-agentní collaborative AI IDE v průmyslu.
- Funkce zahrnují převod designu na kód a celoprocesní AI asistované kódování.
- Poháněno modelem Baidu Wenxin 4.0 X1 Turbo.

#### Trae (ByteDance)
- Tento projekt byl převážně udržován hlavně pomocí Trae po většinu času.
- Trae je AI IDE vyvinuté společností SPRING PTE, singapurskou dceřinou společností ByteDance.
- Jako 10x AI Engineer dokáže samostatně budovat softwarová řešení.
- Má inteligentní produktivní nástroje, flexibilní přizpůsobení vývojového rytmu a collaborative funkce doručování projektů.
- Poskytuje výkon na podnikové úrovni s konfigurovatelným agentním systémem.

#### Qoder (Alibaba)
- Od 18. dubna 2026 je tento projekt udržován pomocí Qoder.
- Qoder vyniká v analýze zdrojového kódu a generování dokumentace v doméně, vynikající v porozumění komplexním codebases.
- Používá nulový výpočetní nákladový cenový model, což jej činí vysoce nákladově efektivním pro automatizované zpracování dokumentace a rutinní úlohy.
- AI-driven agentic kódovací platforma navržená pro skutečný vývoj softwaru.
- Má inteligentní generování kódu, konverzační programování, pokročilý kontextový analytický engine a multi-agentní collaborative funkce.
- Poskytuje hluboké porozumění kódu s minimální spotřebou zdrojů, ideální pro dlouhodobou údržbu projektů a akumulaci znalostí.

### Dokumentace požadavků

- Dokumentace požadavků tohoto projektu není veřejně dostupná.
- Požadavky byly opakovaně ověřeny více než 12 mezinárodními AI platformami a velkými modelovými sériemi, což vedlo k více než 2000 řádkům uživatelských příběhů-řízené dokumentace požadavků, kterou téměř nelze pochopit lidsky.

---

## [Nevydáno]

### 2026-04-26

#### Systém nápovědy dokumentace
- `69c51c5` - Přidán systém nápovědy dokumentace a přeloženy komentáře kódu do angličtiny

### 2026-04-25

#### Správa projektového pracovního prostoru
- `785c551` - Implementována správa projektového pracovního prostoru s pracovními poznámkami a systémem úkolů
  - Nový systém správy projektového pracovního prostoru
  - Funkce pracovních poznámek pro sledování pokroku projektu
  - Integrace systému správy úkolů
  - Zlepšené schopnosti organizace a sledování projektu

#### Lokalizace češtiny
- `b4bbf39` - Přidána kompletní lokalizace češtiny (cs-CZ) a aktualizovány všechny jazykové dokumentace
  - Kompletní podpora češtiny
  - Aktualizovány všechny jazykové dokumentace pro zahrnutí češtiny
  - Kompletní lokalizace UI prvků a zpráv
- `faf078f` - Opraveny chyby kompilace lokalizace češtiny
  - Vyřešeny problémy s kompilací v souborech lokalizace češtiny
  - Zajištěna správná integrace jazykového balíčku češtiny

#### Vylepšení systému znalostí
- `20adaac` - Přidán KnowledgeTool a podpora kompletní lokalizace
  - Nový KnowledgeTool pro správu znalostí
  - Kompletní vícejazyčná lokalizační podpora
  - Zlepšené schopnosti sítě znalostí

### 2026-04-24

#### Vylepšení správy paměti
- `c7b2ecc` - Vylepšeny funkce správy paměti s pokročilým filtrováním, statistikami a zobrazením detailů
  - Nové pokročilé filtrování paměti s podporou filtrování podle typu, časového rozsahu, štítků a více dimenzí
  - Implementována funkce statistik paměti zobrazující počet pamětí, distribuci typů a další statistické informace
  - Přidána stránka zobrazení detailů paměti s podporou zobrazení kompletních informací o jednotlivých pamětech
  - Optimalizováno rozhraní správy paměti pro zlepšení uživatelské zkušenosti a operační efektivity
  - Vícejazyčná lokalizační podpora (6 jazyků)

#### Rozšíření systému oprávnění
- `4489ad6` - Přidána služba počasí wttr.in na whitelist sítě
  - Povoleno silikonovým bytostem přístup k wttr.in weather API pro získání informací o počasí
  - Aktualizována dokumentace systému oprávnění vysvětlující konfiguraci oprávnění služby počasí
  - Kompletní synchronizace vícejazyčné dokumentace (6 jazyků)

#### Optimalizace historie chatu
- `057b09d` - Optimalizováno zobrazení detailů historie chatu, zlepšeno vykreslování volání nástrojů
  - Optimalizováno zobrazení volání nástrojů na stránce detailů historie chatu
  - Zlepšeno formátované zobrazení parametrů volání nástrojů
  - Zlepšena čitelnost historických zpráv
- `0df599c` - Opraven problém, kde byly výsledky nástrojů vykreslovány jako samostatné zprávy chatu
  - Výsledky provedení nástrojů jsou nyní správně přidruženy k původní zprávě
  - Zamezeno zobrazování výsledků nástrojů jako samostatných AI odpovědí
  - Zlepšena kontinuita zpráv chatu

#### Opravy webového rozhraní
- `d9d72e9` - Opraven problém priority CSS v modálním okně detailů pracovních poznámek
  - Oprava stylů modálního okna pracovních poznámek
  - Úprava priority CSS pro zajištění správné aplikace stylů
  - Zlepšen vizuální efekt modálního okna

#### Vylepšení základních funkcí
- `1e7c7b2` - Zlepšena komprese paměti a sledování provedení nástrojů
  - Optimalizace algoritmu komprese paměti
  - Vylepšený mechanismus sledování provedení nástrojů
  - Zvýšená efektivita správy paměti pro silikonové bytosti

#### Vylepšení lokalizace
- `c13cb17` - Registrována jazyková varianta španělštiny
  - Lokalizační podpora španělštiny (Španělsko)
  - Rozšíření vícejazyčného systému
- `9c44f34` - Přidána vícejazyčná lokalizační podpora pro čínský historický kalendář
  - Vícejazyčná lokalizace čínského historického kalendáře
  - Vícejazyčná podpora pro historické éry, dynastické tabulky a další informace
- `192fc6e` - Přidána chybějící lokalizace názvů nástrojů pro 5 nástrojů
  - Doplněno zobrazení lokalizovaných názvů nástrojů
  - Zlepšen vícejazyčný zážitek z rozhraní nástrojů

### 2026-04-23

#### Historie chatu a indikátory načítání
- `e483348` - Implementováno zobrazení historie chatu silikonových bytostí
  - Nový ChatHistoryController s API pro seznam relací a detaily zpráv
  - Vytvořen ChatHistoryViewModel pro přenos dat
  - Implementovány stránky ChatHistoryListView a ChatHistoryDetailView
  - Přidány lokalizační klíče pro historii chatu (5 jazyků)
  - Aktualizován Router pro zahrnutí tras historie chatu
  - Přidán vstupní odkaz historie chatu na stránce detailů BeingView
- `65c157b` - Přidány indikátory načítání pro stránky chatu a automatický výběr relace kurátora
  - Indikátor stavu načítání stránky chatu
  - Funkce automatického výběru relace kurátora
  - Vícejazyčná podpora (6 jazyků)

#### Vylepšení řízení toku AI
- `30a2d4e` - Vylepšeno zrušení toku AI, integrace IM a inicializace hlavního hostitele
  - Vylepšený mechanismus zrušení toku ContextManager
  - Zlepšená integrace systému IM
  - Optimalizovaná inicializace CoreHost
  - Vylepšená funkce DiskExecutor
  - Aktualizován WebUIProvider

#### Podpora nahrávání souborů
- `28fb344` - Implementován dialog zdrojů souborů a podpora nahrávání souborů
  - Dialog zdrojů souborů webového rozhraní
  - Implementace funkce nahrávání souborů
- `1d3e2cc` - Přidány lokalizační řetězce dialogu zdrojů souborů
  - Vícejazyčná podpora dialogu zdrojů souborů (6 jazyků)

#### Fronta zpráv chatu
- `db48c51` - Přidána fronta zpráv chatu, metadata souborů a podpora zrušení toku
  - Nový systém fronty zpráv chatu ChatMessageQueue
  - Nová správa metadat souborů FileMetadata
  - Nový správce zrušení toku StreamCancellationManager

### 2026-04-22

#### Vylepšení lokalizace
- `b574b2b` - Přidáno senderName pro historické zprávy pro AI identifikaci
  - Přidáno pole názvu odesílatele do historických zpráv SSE
  - Podpora identifikace AI zpráv
- `0a8d750` - Přidán obecný systémový výzva pro aktivní chování silikonových bytostí
  - Nová šablona obecného systémového výzvy v lokalizaci
  - Podpora vedení aktivního chování silikonových bytostí

#### Rozšíření systému nástrojů
- `70ce7fb` - Implementován DatabaseTool pro strukturované databázové dotazy
  - Nový nástroj databázových dotazů
  - Podpora strukturovaných datových operací
- `be29a09` - Implementován LogTool pro dotazy historie operací a dialogů
  - Nový nástroj dotazů na logy
  - Podpora vyhledávání historie operací a dialogů
- `4ea7702` - Implementován PermissionTool pro dynamickou správu oprávnění
  - Nový nástroj správy oprávnění
  - Podpora dynamických dotazů a správy oprávnění
- `1384ff4` - Implementován ExecuteCodeTool pro vícejazyčné provádění kódu
  - Nový nástroj provádění kódu
  - Podpora kompilace a provádění vícejazyčného kódu
- `82d1e11` - Implementován SearchTool pro vyhledávání informací
  - Nový nástroj vyhledávání informací
  - Podpora externího vyhledávání informací

#### Refaktorizační systém logů
- `8f6cb1e` - Přidán parametr beingId do rozhraní ILogger, implementováno oddělení logů systém/silikonová bytost
  - Rozšíření rozhraní ILogger
  - Podpora oddělení systémových logů a logů silikonových bytostí
  - Nový parametr beingId
- `2b771f3` - Oddělen LogController od souborového I/O, přidáno API pro čtení logů
  - Refaktorizace architektury LogController
  - Nové nezávislé API pro čtení logů
  - Oddělení operací souborového I/O
- `12da302` - Přidán filtr silikonových bytostí pro zobrazení logů
  - Nová funkce filtru silikonových bytostí ve webovém rozhraní
  - Filtrování záznamů logů podle silikonových bytostí

#### Vylepšení systému oprávnění
- `4c747ad` - Refaktorizován PermissionTool, ExecuteCodeTool, přidáno API EvaluatePermission
  - Refaktorizace PermissionTool a ExecuteCodeTool
  - Integrace API EvaluatePermission

#### Optimalizace webového rozhraní
- `702b3f3` - Vylepšen pohled úkolů s přídavkem stavových odznaků a zobrazením metadat
  - Zlepšení UI pohledu úkolů
  - Nové zobrazení stavových odznaků a metadat
- `6ed9a79` - Zlepšeno ukládání zpráv chatu a vykreslování pohledu
  - Optimalizace mechanismu ukládání zpráv chatu
  - Zvýšený výkon vykreslování pohledu
- `0675c45` - Optimalizováno zvýraznění bloků kódu markdown v náhledovém panelu
  - Optimalizace zvýraznění kódu v náhledu markdown
  - Zlepšený efekt zobrazení bloků kódu

#### Integrace nástrojů
- `135710d` - Odebrán SearchTool, místní vyhledávání přesunuto do DiskTool
  - Odebrání SearchTool
  - Funkce místního vyhledávání integrována do DiskTool
- `7a03a19` - Zlepšena flexibilita dotazů na dialogy LogTool
  - Optimalizace logiky dotazů na dialogy LogTool
  - Zvýšená flexibilita dotazů

#### Správa konfigurace
- `4305769` - Přidán .gitattributes pro správu konců řádků
  - Konfigurace konců řádků pro cross-platform kompatibilitu

#### Opravy chyb
- `1c96e99` - Opraveno selhání vyhledávání v kořenovém adresáři pro search_files a search_content
  - Oprava funkce vyhledávání v kořenovém adresáři DiskTool
  - Oprava logiky vyhledávání souborů a obsahu

### 2026-04-21

#### Vylepšení systému oprávnění
- `5879621` - Přidána předkompilační ověření callbacků oprávnění a vylepšené zpracování chyb
  - Funkce předkompilačního ověření callbacků oprávnění
  - Kompilace kódu callbacků oprávnění před uložením pro ověření správnosti
  - Zamezeno ukládání neplatného kódu na disk
  - Detailní zpracování chyb pro operace uložení oprávnění
  - Vylepšené chybové zprávy s podporou lokalizace
  - Oddělení kroků kompilace a bezpečnostního skenování pro lepší hlášení chyb
- `833ead2` - Přidáno ověření referencí sestavení pro dynamickou kompilaci
  - Zvýšení bezpečnosti dynamické kompilace
  - Mechanismus ověření referencí sestavení

#### Vylepšení webového rozhraní
- `0a826f5` - Přidána indikace úspěšného uložení v editoru kódu
  - Funkce indikace úspěšného uložení operací editoru kódu
  - Zlepšená zpětná vazba uživatelských operací
- `2940373` - Vylepšeno webové rozhraní s přídavkem hover nápověd kódu a vylepšeními UI
  - Funkce hover nápověd editoru kódu
  - Optimalizace UI webového rozhraní
- `6ba591d` - Přidán nezávislý editor konfigurace AI pro silikonové bytosti
- `634e8ca` - Přidán odkaz návratu na seznam ze stránky oprávnění
- `188c6f8` - Registrována trasa API seznamu úkolů a přidáno zobrazení prázdného stavu

#### Opravy chyb
- `592c7ab` - Opravena inicializace callbacků a pořadí registrace
  - Oprava systému callbacků oprávnění
  - Optimalizace pořadí registrace callbacků
- `c6b518b` - Opraven přenos zpráv timeru a ukládání zpráv chatu
  - Oprava mechanismu přenosu zpráv timeru
  - Optimalizace ukládání zpráv chatu

#### Lokalizace
- `7940d9c` - Přidána podpora lokalizace korejštiny
  - Soubory lokalizace korejštiny
  - Vylepšení vícejazyčného systému
- `4ff98ad` - Refaktorizovány dokumentace pro podporu více jazyků
  - Restrukturalizace struktury dokumentace
  - Synchronizace vícejazyčné dokumentace

#### AI a kalendář
- `646813e` - Zlepšena implementace továrny AI klientů
  - Refaktorizace továrny AI klientů
  - Optimalizace mechanismu objevování klientů
- `928a96d` - Opravena implementace výpočtu kalendáře
  - Oprava logiky výpočtu kalendáře
  - Zvýšená přesnost výpočtu více typů kalendářů

#### Konfigurace a vstupní bod
- `0fc1693` - Aktualizován vstup programu a konfigurace projektu
  - Optimalizace vstupního bodu programu
  - Zlepšení konfigurace projektu

### 2026-04-20

#### Dokončení základních funkcí
- `28905b5` - Kompletní vícejazyčná podpora, továrna AI klientů, systém oprávnění a nastavení lokalizace
  - Systém logů s manažery, položkami a různými úrovněmi logů
  - Systém auditu tokenů pro dotazování a sledování použití tokenů
  - Továrna AI klientů s automatickým objevováním různých AI platforem
  - Systém callbacků oprávnění s vlastním úložištěm
  - Implementace konzolového loggeru
  - Vícejazyčná podpora pro angličtinu a zjednodušenou čínštinu
  - WebUI Messenger s WebSocket pro chat v reálném čase
  - Vylepšená výchozí silikonová bytost s lokalizací

### 2026-04-19

#### Timer a kalendář
- `c933fd8` - Aktualizována lokalizace, systém timerů, webové zobrazení a přidány nástroje
  - Lepší správce lokalizace
  - Systém plánování pro časové úlohy
  - Konfigurace AI a správa kontextu
  - Nástroj kalendáře podporující 32 typů kalendářů
  - Webové kontrolery pro API kalendáře
  - Nástroj správy úkolů

**Architektonická vylepšení**
- Redesignováno architekturu webového zobrazení pro lepší podporu skinů
- Zlepšený systém správy bytostí s lepším zpracováním stavu

### 2026-04-18

- `9f585e1` - Aktualizována lokalizace, systém timerů, webové zobrazení a přidány nástroje
  - Vylepšení timerů a plánování
  - Lepší webové zobrazení s vylepšenými UI komponentami
  - Více implementací nástrojů

### 2026-04-17

- `9b71fcd` - Aktualizovány základní moduly, přidána dokumentace zh-HK, vysílací kanály, konfigurační nástroje a auditní webové zobrazení
  - Vysílací kanály pro chat více silikonových bytostí dohromady
  - Systém konfiguračních nástrojů
  - Auditní webové zobrazení
  - Dokumentace tradiční čínštiny

### 2026-04-16

- `5040f05` - Aktualizovány moduly Core a Default
  - Optimalizace modulů a opravy chyb
  - Aktualizace a vylepšení implementace

### 2026-04-15

- `3efab5f` - Aktualizováno více modulů: AI, Chat, IM, Tools, Web, Localization, Storage
  - Vylepšení AI klientů
  - Vylepšení systému chatu
  - Aktualizace poskytovatele messengeru
  - Optimalizace systému nástrojů
  - Vylepšení webové infrastruktury
  - Optimalizace lokalizace
  - Aktualizace systému úložiště

### 2026-04-14

- `4241a2f` - Základní dokončení funkcí chatu, optimalizace nahrávání UI
  - Dokončení funkcí systému chatu
  - Optimalizace UI pro nahrávání souborů

### 2026-04-13

- `c498c31` - Aktualizace kódu
  - Obecné vylepšení a optimalizace kódu

### 2026-04-12

- `2161002` - Refaktorizovány dokumentace a vylepšena lokalizace
  - Restrukturalizace dokumentace
  - Vylepšení systému lokalizace
- `03d94e4` - Vylepšen konfigurační systém a lokalizace
  - Vylepšení konfiguračního systému
  - Podpora dalších jazyků
- `9976a35` - Přidána stránka O nás a lokalizace
  - Stránka O nás
  - Vylepšení lokalizace
- `0c8ccfc` - Vylepšen systém chatu, lokalizace a webové zobrazení
  - Vylepšení systému chatu
  - Aktualizace lokalizace
  - Vylepšení webového zobrazení
- `a8f1342` - Redesignována webová komunikační vrstva, přepnutí z WebSocket na SSE
  - Webová komunikace nyní používá Server-Sent Events

### 2026-04-11

- `e8fe259` - Přidán systém logů a optimalizace kódu
  - Základní infrastruktura logů
  - Implementace loggeru
- `f01c519` - Přidán systém logů, aktualizována rozhraní AI a webové zobrazení
  - Aktualizace rozhraní AI
  - Vylepšení webového zobrazení

### 2026-04-10

- `4962924` - Vylepšeny obslužné rutiny WebSocket, zobrazení chatu a interakce messengeru
  - Vylepšení ContextManageru
  - Vylepšení systému chatu
  - Aktualizace rozhraní poskytovatele messengeru
  - Redesign WebUI Provideru
  - Aktualizace JavaScript Builderu a Routeru
  - Optimalizace zobrazení chatu
  - Vylepšení obslužných rutin WebSocket

### 2026-04-09

- `f9302bf` - Vylepšeno rozhraní poskytovatele messengeru, systém chatu a interakce Web UI
  - Rozšíření rozhraní poskytovatele messengeru
  - Vylepšení zpráv chatu a systému
  - Optimalizace ContextManageru
  - Vylepšení výchozí silikonové bytosti
  - Vylepšení zobrazení chatu Web UI
  - Aktualizace obslužných rutin WebSocket

### 2026-04-07

- `6831ee8` - Redesignováno webové zobrazení a JavaScript Builder
  - Kompletní redesign webových kontrolerů
  - Kompletní přepsání JavaScript Builderu
  - Aktualizace všech komponent zobrazení (zobrazení bytostí, zobrazení chatu, zobrazení prohlížeče kódu, zobrazení konfigurace atd.)
  - Vylepšení systému skinů (skin manažerské verze, skin chatovací verze, skin tvůrčí verze, skin vývojové verze)
  - Vylepšení architektury základní třídy zobrazení

### 2026-04-05

- `41e97fb` - Aktualizováno více základních modulů a webových kontrolerů
  - Vylepšení ContextManageru
  - Systém chatu a správa relací
  - Redesign ServiceLocatoru
  - Aktualizace základní třídy a manažeru silikonových bytostí
  - Kompletní aktualizace webových kontrolerů (17 kontrolerů)
  - Vylepšení továrny výchozí silikonové bytosti
- `67988d4` - Vylepšeny moduly Web UI, přidáno zobrazení exekutorů, vyčištěny zobrazení a základní moduly
  - Zobrazení exekutorů
  - Vyčištění a organizace modulů

### 2026-04-04

- `b58bb1c` - Přidán inicializační kontroler a redesignován webový modul
  - Inicializační kontroler
  - Redesign konfiguračního modulu
  - Aktualizace lokalizačního modulu
  - Vylepšení systému skinů
  - Vylepšení Routeru
- `f03ac0b` - Přidán modul Web UI, vylepšeny funkce messengeru
  - Modul Web UI
  - Vylepšení funkcí messengeru

### 2026-04-03

- `192e57b` - Aktualizována struktura projektu a základní komponenty runtime
  - Aktualizace systému runtime
  - Vylepšení struktury projektu
- `59faec8` - Aktualizace Core a Default implementací
  - Vylepšení základních modulů
  - Aktualizace výchozích implementací
- `d488485` - Přidána funkce dynamické kompilace a modul nástrojů kurátora
  - Exekutor dynamické kompilace
  - Implementace nástrojů kurátora
- `753d1d9` - Přidán bezpečnostní modul, aktualizovány exekutory, poskytovatelé messengeru, lokalizace a nástroje
  - Bezpečnostní systém
  - Aktualizace exekutorů
  - Vylepšení poskytovatelů messengeru
  - Vylepšení lokalizace
  - Aktualizace systému nástrojů
- `a378697` - Dokončena fáze 5 - Systém nástrojů + Exekutory
  - Správa a definice nástrojů
  - Exekutor příkazového řádku
  - Diskový exekutor
  - Síťový exekutor
  - Implementace nástrojů

### 2026-04-02

- `e6ad94b` - Opraven problém selhání načítání historie chatu při mazání konfiguračního souboru během testů
  - Vylepšené zpracování chyb Ollama klienta
  - Ověření konfiguračních dat
  - Vyčištění referencí projektu
- `daa56f5` - Dokončena fáze 4: Persistentní paměť (systém chatu + kanál messengeru)
  - Systém chatu s skupinovými a soukromými chaty
  - Rozhraní správce a poskytovatele messengeru
  - Úložiště časového indexu
  - Zpracování nekompletních dat
  - Časové úložiště souborového systému
  - Konzolový poskytovatel messengeru
  - Vylepšení továrny silikonových bytostí
  - Aktualizace inicializace programu

### 2026-04-01

- `bbe2dbb` - Opraveno načítání konfigurace a směrování zpráv služby chatu
  - Implementace ContextManageru (přidáno 188 řádků kódu)
  - Rozhraní AI klienta
  - Rozhraní a jednoduchá implementace služby chatu
  - Konfigurační systém s konvertory
  - Vylepšení hlavního smyčkového scheduleru
  - Systém správy silikonových bytostí
  - Implementace Ollama klienta
  - Nastavení systému lokalizace
  - Redesign inicializace programu
- `2fa6305` - Implementována fáze 2: Hlavní smyčkový framework a systém objektů hodin
  - Hlavní smyčkový scheduler
  - Základní třída objektů hodin
  - Rozhraní továrny AI klientů
  - Rozhraní úložiště
  - Továrna klientů Ollama
  - Konzolové hodiny
  - Testovací hodiny
  - Úložiště souborového systému
- `32b99a1` - Implementována fáze 1 - Základní funkce chatu
  - Modely požadavků a odpovědí AI
  - Rozhraní AI klienta
  - Modely zpráv
  - Systém lokalizace (jazyky, základní třída lokalizace, správce lokalizace)
  - Základní třída konfiguračních dat
  - Implementace Ollama klienta
  - Správa konfigurace
  - Vícejazyčná lokalizace (angličtina, zjednodušená čínština)
  - Vstupní bod programu
- `358e368` - Počáteční commit: Dokumentace projektu a licence
  - README projektu (angličtina a čínština)
  - Dokumentace architektury
  - Dokumentace roadmapy
  - Bezpečnostní dokumentace
  - Licence (Apache 2.0)
  - Konfigurace Git ignorace
