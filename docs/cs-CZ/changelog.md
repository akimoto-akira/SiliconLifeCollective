# Záznam změn

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | **Čeština**

Všechny důležité změny tohoto projektu budou zdokumentovány v tomto souboru.

Formát je založen na [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
a tento projekt dodržuje [Semantické verzování](https://semver.org/spec/v2.0.0.html).

---

## O tomto záznamu změn

### Původ projektu

- Tento projekt vznikl 20. března 2026.
- Před tímto projektem existoval ověřovací demo, který selhal kvůli iracionálnímu návrhu architektury, což znemožnilo integraci s více AI platformami.

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
- Většinu času byl tento projekt primárně udržován pomocí Trae.
- Trae je AI IDE vyvinuté společností SPRING PTE, singapurskou dceřinou společností ByteDance.
- Jako 10x AI Engineer, schopný samostatně budovat softwarová řešení.
- Obsahuje inteligentní produktivity nástroje, flexibilní adaptaci vývojového rytmu a kolaborativní doručování projektů.
- Poskytuje výkon podnikové úrovně s konfigurovatelným agentním systémem.

#### Qoder (Alibaba)
- Od 18. dubna 2026 je tento projekt udržován pomocí Qoder.
- Qoder vyniká v analýze zdrojového kódu a generování doménové dokumentace, s vynikající schopností porozumět komplexním kódovým základnám.
- Adoptuje model cenování s nulovými výpočetními náklady, což jej činí vysoce nákladově efektivním pro automatizované zpracování dokumentace a rutinní úkoly.
- AI-powered agentic kódovací platforma navržená pro skutečný vývoj softwaru.
- Obsahuje inteligentní generování kódu, konverzační programování, pokročilý engine analýzy kontextu a multi-agentní spolupráci.
- Poskytuje hluboké porozumění kódu s minimální spotřebou zdrojů, ideální pro dlouhodobou údržbu projektů a akumulaci znalostí.

### Požadavkový dokument

- Požadavkový dokument pro tento projekt není veřejně dostupný.
- Požadavky byly validovány prostřednictvím iterací s 12+ mezinárodními AI platformami a velkými modelovými sériemi, což vytvořilo uživatelskými příběhy řízený požadavkový dokument o více než 2000 řádcích, který je téměř nesrozumitelný pro lidi.

---

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
  - Celkem: 41 souborů změněno (+1820/-903 řádků)

### 2026-04-27

#### Vylepšení systému nápovědní dokumentace
- `9989d79` - Aktualizována lokalizace, systém nápovědy a webová zobrazení
  - Přidáno rozhraní nápovědní dokumentace továrny AI klienta IAIClientFactoryHelp.cs
  - Dokončen překlad všech nápovědních dokumentů do 9 jazyků
  - HelpTopics.cs přidáno 40 definic témat nápovědy
  - Webová zobrazení komplexně aktualizována: InitController, AuditView, ConfigView, KnowledgeView, LogView, atd.
  - Vylepšení systému lokalizace: všechny jazykové verze přidány nové lokalizační klíče
  - Aktualizace továrny AI klienta: vylepšení DashScopeClientFactory, OllamaClientFactory

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

### 2026-04-26

#### Systém nápovědní dokumentace
- `07895d7` - Vylepšen systém nápovědní dokumentace, přidány 3 dokumenty a dokončen překlad do 9 jazyků
  - Přidán průvodce použitím systému paměti, konfigurace instalace Ollama, platformy Alibaba Cloud Bailian
  - Dokončen překlad všech 10 nápovědních dokumentů do 9 jazyků
  - Zjednodušena logika vykreslování HelpView

#### Německá lokalizace
- `0cfd8a1` - Přidána kompletní podpora německé (de-DE) lokalizace
  - Kompletní soubory německé lokalizace
  - Přidána německá podpora pro čínský historický kalendář
  - Přidán německý překlad nápovědní dokumentace
  - Plně synchronizovány všechny dokumenty v 9 jazycích

#### Synchronizace dokumentace
- `3aada7d` - Synchronizována dokumentace v tradiční čínštině (zh-HK) se zjednodušenou čínštinou
- `2f6abff` - Přidána lokalizace zobrazovaných názvů nástrojů nápovědy pro všechny jazyky

#### Refaktorizace systému znalostí
- `60944fe` - Sjednocen jmenný prostor na SiliconLife.Collective
- `69c51c5` - Přidán systém nápovědní dokumentace a přeloženy komentáře kódu do angličtiny

### 2026-04-25

#### Automatizace prohlížeče WebView
- `41757c3` - Implementována跨platformní automatizace prohlížeče WebView založená na Playwright

#### Aktualizace dokumentace
- `0ff797b` - Přidána dokumentace KnowledgeTool a WorkNoteTool (7 jazyků)
- `ad77415` - Aktualizovány všechny soubory changelog, přidána historie Git z 2026-04-25

#### Správa pracovního prostoru projektu
- `785c551` - Implementována správa pracovního prostoru projektu s pracovními poznámkami a systémem úkolů
  - Přidán systém správy pracovního prostoru projektu
  - Funkce pracovních poznámek pro sledování postupu projektu
  - Integrace systému správy úkolů

#### Česká lokalizace
- `b4bbf39` - Přidána kompletní česká (cs-CZ) lokalizace a aktualizována veškerá jazyková dokumentace
- `faf078f` - Opraveny chyby kompilace české lokalizace

#### Vylepšení systému znalostí
- `20adaac` - Přidán KnowledgeTool s kompletní podporou lokalizace

### 2026-04-24

#### Vylepšení správy paměti
- `c7b2ecc` - Vylepšena správa paměti s pokročilým filtrováním, statistikami a podrobnými zobrazeními
  - Přidáno pokročilé filtrování paměti
  - Implementovány statistiky paměti
  - Přidána stránka podrobného zobrazení paměti
  - Podpora vícejazyčné lokalizace (6 jazyků)

#### Rozšíření systému oprávnění
- `4489ad6` - Přidána meteorologická služba wttr.in na seznam povolených sítí
  - Kompletní synchronizace vícejazyčné dokumentace (6 jazyků)

#### Opravy webového rozhraní
- `d9d72e9` - Opraven problém priority CSS v modálním okně detailu pracovní poznámky

#### Optimalizace historie chatu
- `0df599c` - Opraveno vykreslování výsledků nástrojů jako samostatných zpráv chatu
- `057b09d` - Optimalizováno zobrazení detailů historie chatu, zlepšeno vykreslování volání nástrojů

#### Historie provádění časovače
- `fa3f06f` - Přidána funkce historie provádění časovače s podrobným zobrazením
- `d824835` - Přidány lokalizační klíče historie provádění časovače (všechny jazyky)

#### Vylepšení lokalizace
- `c13cb17` - Registrována jazyková varianta španělštiny
- `9c44f34` - Přidána podpora vícejazyčné lokalizace pro čínský historický kalendář

#### Vylepšení hlavní funkčnosti
- `1e7c7b2` - Zlepšena komprese paměti a sledování provádění nástrojů

### 2026-04-23

#### Lokalizace nástrojů
- `192fc6e` - Přidána chybějící lokalizace názvů nástrojů pro 5 nástrojů

#### Aktualizace dokumentace
- `882c08f` - Aktualizovány všechny soubory changelog, přidána kompletní historie Git a odstraněna falešná čísla verzí

#### Vylepšení stránky chatu
- `65c157b` - Přidán indikátor načítání na stránku chatu a automatický výběr relace kurátora

#### Funkce historie chatu
- `e483348` - Implementována funkce zobrazení historie chatu křemíkové bytosti
  - Přidán ChatHistoryController
  - Vytvořen ChatHistoryViewModel
  - Implementovány stránky ChatHistoryListView a ChatHistoryDetailView
  - Přidány lokalizační klíče pro historii chatu (5 jazyků)

#### Vylepšení řízení toku AI
- `30a2d4e` - Vylepšeno rušení toku AI, integrace IM a inicializace hlavního hostitele

#### Fronta zpráv chatu
- `db48c51` - Přidána fronta zpráv chatu, metadata souborů a podpora rušení streamu

#### Podpora nahrávání souborů
- `28fb344` - Implementován dialog zdrojů souborů a podpora nahrávání souborů
- `1d3e2cc` - Přidány lokalizační řetězce dialogu zdrojů souborů (6 jazyků)

#### Aktualizace dokumentace
- `8111e92` - Přidán odkaz Wiki do sekce repozitáře README

### 2026-04-22

#### Lokalizace dokumentace
- `66c11eb` - Přeloženy čínské komentáře do angličtiny a aktualizovány všechny changelogy

#### Vylepšení zpráv SSE
- `b574b2b` - Přidáno senderName do historických zpráv pro identifikaci AI

#### Funkce chatu
- `601fc14` - Přidána akce mark_read pro označení konce relace

#### Optimalizace systému nástrojů
- `7a03a19` - Zlepšena flexibilita dotazů na konverzaci LogTool

#### Vylepšení lokalizace
- `0a8d750` - Přidán běžný systémový prompt pro aktivní chování křemíkové bytosti

#### Refaktorizace systému protokolů
- `2b771f3` - Odpojen LogController od souborového I/O, přidáno API pro čtení protokolů
- `12da302` - Přidán filtr křemíkové bytosti do zobrazení protokolů
- `8f6cb1e` - Přidán parametr beingId do rozhraní ILogger, implementováno oddělení protokolů systému/křemíkové bytosti

#### Vylepšení systému oprávnění
- `4c747ad` - Refaktorizovány PermissionTool, ExecuteCodeTool, přidáno API EvaluatePermission

#### Opravy chyb
- `1c96e99` - Opraveno selhání vyhledávání v kořenovém adresáři search_files a search_content

#### Integrace nástrojů
- `135710d` - Odstraněn SearchTool, místní vyhledávání přesunuto do DiskTool

#### Rozšíření systému nástrojů
- `70ce7fb` - Implementován DatabaseTool pro strukturované dotazy na databázi
- `be29a09` - Implementován LogTool pro dotazy na historii operací a konverzací
- `4ea7702` - Implementován PermissionTool pro dynamickou správu oprávnění
- `1384ff4` - Implementován ExecuteCodeTool pro více-jazykové provádění kódu
- `82d1e11` - Implementován SearchTool pro vyhledávání informací

#### Optimalizace webového rozhraní
- `0675c45` - Optimalizováno zvýraznění bloků kódu markdown v náhledovém panelu
- `702b3f3` - Vylepšeno zobrazení úkolů se stavovými odznaky a zobrazením metadat
- `6ed9a79` - Zlepšeno ukládání zpráv chatu a vykreslování zobrazení

### 2026-04-21

#### Opravy chyb
- `c6b518b` - Opraveno doručování zpráv časovače a ukládání zpráv chatu

#### Správa konfigurace
- `4305769` - Přidán .gitattributes pro správu konců řádků

#### Vylepšení webového rozhraní
- `188c6f8` - Registrována trasa API seznamu úkolů a přidáno zobrazení prázdného stavu
- `634e8ca` - Přidán odkaz na návrat na seznam stránky oprávnění
- `6ba591d` - Přidán nezávislý editor konfigurace AI pro křemíkové bytosti
- `0a826f5` - Přidán prompt úspěšného uložení v editoru kódu
- `2940373` - Vylepšeno webové rozhraní s nápovědami při najetí na kód a vylepšeními UI

#### Opravy systému oprávnění
- `592c7ab` - Opravena inicializace callbacku a pořadí registrace

#### Vylepšení zabezpečení
- `833ead2` - Přidána ověření referencí sestavení pro dynamickou kompilaci

#### Vylepšení systému oprávnění
- `5879621` - Přidáno ověření předkompilace callbacku oprávnění a vylepšené zpracování chyb

#### Aktualizace dokumentace
- `4dbf659` - Aktualizován changelog na v0.5.1, nahrazeny URL zástupných symbolů GitHub, přidán zrcadlový server Gitee, lokalizován název Bilibili podle jazyka, aktualizován email

#### Konfigurace a vstup
- `0fc1693` - Aktualizován vstup programu a konfigurace projektu

#### Refaktorizace systému oprávnění
- `ea9179a` - Vylepšena implementace systému oprávnění

#### Opravy chyb
- `928a96d` - Opravena implementace výpočtu kalendáře

#### AI a kalendář
- `646813e` - Vylepšena implementace továrny AI klienta

#### Lokalizace
- `7940d9c` - Přidána podpora korejské lokalizace
- `4ff98ad` - Refaktorizována dokumentace pro podporu více jazyků

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

### 2026-04-19

#### Časovač a kalendář
- `c933fd8` - Aktualizována lokalizace, systém časovače, webová zobrazení a přidány nástroje
  - Lepší manažer lokalizace
  - Systém plánování pro časované úkoly
  - Konfigurace AI a správa kontextu
  - Nástroj kalendáře podporující 32 typů kalendáře
  - Webový kontrolér pro API kalendáře
  - Nástroj správy úkolů

**Vylepšení architektury**
- Znovu navržena architektura webového zobrazení pro lepší podporu skinů
- Vylepšen systém správy bytostí s lepším zpracováním stavu

### 2026-04-18

- `9f585e1` - Aktualizována lokalizace, systém časovače, webová zobrazení a přidány nástroje
  - Vylepšení časovače a plánování
  - Lepší webová zobrazení s vylepšenými komponentami UI
  - Více implementací nástrojů

### 2026-04-17

- `9b71fcd` - Aktualizovány hlavní moduly, přidána dokumentace zh-HK, vysílací kanál, nástroje konfigurace a auditní webová zobrazení
  - Vysílací kanál pro více křemíkových bytostí chatujících společně
  - Systém nástrojů konfigurace
  - Auditní webová zobrazení
  - Dokumentace v tradiční čínštině

### 2026-04-16

- `5040f05` - Aktualizovány hlavní a výchozí moduly
  - Optimalizace modulů a opravy chyb
  - Aktualizace a vylepšení implementace

### 2026-04-15

- `3efab5f` - Aktualizovány více modulů: AI, Chat, IM, Tools, Web, Localization, Storage
  - Vylepšení AI klienta
  - Vylepšení systému chatu
  - Aktualizace poskytovatele messengeru
  - Optimalizace systému nástrojů
  - Vylepšení webové infrastruktury
  - Optimalizace lokalizace
  - Aktualizace systému úložiště

### 2026-04-14

- `4241a2f` - Funkce chatu základně dokončeny, optimalizace nahrávání UI
  - Funkce systému chatu dokončeny
  - Optimalizace UI pro nahrávání souborů

### 2026-04-13

- `c498c31` - Aktualizace kódu
  - Obecná vylepšení a optimalizace kódu

### 2026-04-12

#### Dokumentace a lokalizace
- `2161002` - Refaktorizována dokumentace a vylepšena lokalizace
- `03d94e4` - Vylepšen systém konfigurace a lokalizace
- `9976a35` - Přidána stránka o projektu a lokalizace

#### Chat a webová zobrazení
- `0c8ccfc` - Vylepšen systém chatu, lokalizace a webová zobrazení
- `a8f1342` - Znovu navržena vrstva webové komunikace, přepnuto z WebSocket na SSE

### 2026-04-11

#### Systém protokolů
- `e8fe259` - Přidán systém protokolů a optimalizace kódu
- `f01c519` - Přidán systém protokolů, aktualizováno rozhraní AI a webová zobrazení

### 2026-04-10

- `4962924` - Vylepšen obslužný program WebSocket, zobrazení chatu a interakce messengeru
  - Vylepšení správce kontextu
  - Vylepšení systému chatu
  - Aktualizace rozhraní poskytovatele messengeru
  - Znovu navržen poskytovatel WebUI
  - Aktualizace sestavovače JavaScriptu a směrovače
  - Optimalizace zobrazení chatu
  - Vylepšení obslužného programu WebSocket

### 2026-04-09

- `f9302bf` - Vylepšeno rozhraní poskytovatele messengeru, systém chatu a interakce webového UI
  - Rozšíření rozhraní poskytovatele messengeru
  - Vylepšení zpráv chatu a systému
  - Optimalizace správce kontextu
  - Vylepšení výchozí křemíkové bytosti
  - Vylepšení zobrazení chatu webového UI
  - Aktualizace obslužného programu WebSocket

### 2026-04-07

- `6831ee8` - Znovu navržena webová zobrazení a sestavovač JavaScriptu
  - Kompletní znovu návrh webových kontrolérů
  - Kompletní přepsání sestavovače JavaScriptu
  - Všechny komponenty zobrazení aktualizovány
  - Vylepšení systému skinů
  - Upgrade architektury základní třídy zobrazení

### 2026-04-05

- `41e97fb` - Aktualizovány více hlavních modulů a webové kontroléry
  - Vylepšení správce kontextu
  - Systém chatu a správa relací
  - Znovu navržen lokátor služeb
  - Aktualizace základní třídy a správce křemíkové bytosti
  - Webové kontroléry komplexně aktualizovány (17 kontrolérů)
  - Vylepšení továrny výchozí křemíkové bytosti
- `67988d4` - Vylepšen modul webového UI, přidáno zobrazení exekutoru, vyčištěna zobrazení a hlavní moduly

### 2026-04-04

- `b58bb1c` - Přidán inicializační kontrolér a znovu navržen webový modul
  - Inicializační kontrolér
  - Znovu navržen modul konfigurace
  - Aktualizace modulu lokalizace
  - Vylepšení systému skinů
  - Vylepšení směrovače
- `f03ac0b` - Přidán modul webového UI, vylepšena funkce messengeru

### 2026-04-03

- `192e57b` - Aktualizována struktura projektu a hlavní runtime komponenty
- `59faec8` - Aktualizace hlavní a výchozí implementace
- `d488485` - Přidána funkce dynamické kompilace a modul nástrojů kurátora
- `753d1d9` - Přidán modul zabezpečení, aktualizovány exekutory, poskytovatelé messengeru, lokalizace a nástroje
- `a378697` - Dokončena fáze 5 - systém nástrojů + exekutory

### 2026-04-02

- `e6ad94b` - Opraveno selhání načítání historie chatu při odstraňování konfiguračních souborů během testování
- `daa56f5` - Dokončena fáze 4: perzistentní paměť (systém chatu + kanál messengeru)

### 2026-04-01

- `bbe2dbb` - Opraveno načítání konfigurace a směrování zpráv služby chatu
- `2fa6305` - Implementována fáze 2: rámec hlavní smyčky a systém objektů hodin
- `32b99a1` - Implementována fáze 1 - základní funkce chatu
- `358e368` - Počáteční commit: dokumentace projektu a licence
