# Změny

[English](../en/changelog.md) | [中文文档](../zh-CN/changelog.md) | [Texto](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md)

Všechny významné změny v tomto projektu budou zaznamenány v tomto souboru.

Formát je založen na [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
tento projekt dodržuje [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

**Poznámka: Tento projekt zatím nebyl oficiálně vydán, veškerý obsah je ve vývoji.**

---

## [Nevydáno]

### 2026-04-25

#### Správa Pracovního Prostoru Projektu
- `785c551` - Implementace správy pracovního prostoru projektu s pracovními poznámkami a systémem úkolů
  - Nový systém správy pracovního prostoru projektu
  - Funkce pracovních poznámek pro sledování průběhu projektu
  - Integrace systému správy úkolů
  - Zlepšené možnosti organizace a sledování projektu

#### Česká Lokalizace
- `b4bbf39` - Přidání kompletní české (cs-CZ) lokalizace a aktualizace všech jazykových dokumentací
  - Kompletní podpora českého jazyka
  - Aktualizace všech jazykových dokumentací včetně češtiny
  - Plná lokalizace prvků uživatelského rozhraní a zpráv
- `faf078f` - Oprava chyb kompilace české lokalizace
  - Vyřešení problémů s kompilací v souborech české lokalizace
  - Zajištění správné integrace českého jazykového balíčku

#### Rozšíření Systému Znalostí
- `20adaac` - Přidání KnowledgeTool s plnou podporou lokalizace
  - Nový nástroj KnowledgeTool pro správu znalostí
  - Plná vícejazyčná lokalizační podpora
  - Rozšířené možnosti sítě znalostí

### 2026-04-24

#### Vylepšení Správy Paměti
- `c7b2ecc` - Rozšíření funkcí správy paměti, přidání pokročilého filtrování, statistik a zobrazení detailů
  - Přidáno pokročilé filtrování paměti s podporou vícerozměrného filtrování podle typu, časového rozsahu, tagů
  - Implementace statistik paměti zobrazující počet pamětí, distribuci typů
  - Přidána stránka detailu paměti pro zobrazení kompletních informací o jednotlivých pamětech
  - Optimalizace rozhraní správy paměti, zlepšení uživatelského zážitku
  - Vícejazyčná lokalizační podpora (6 jazyků)

#### Rozšíření Systému Oprávnění
- `4489ad6` - Přidání služby wttr.in weather do network whitelist
  - Povolení přístupu křemíkových bytostí k wttr.in weather API pro informace o počasí
  - Aktualizace dokumentace systému oprávnění

#### Optimalizace Historie Chatu
- `057b09d` - Optimalizace zobrazení detailu historie chatu, zlepšení renderování volání nástrojů
- `0df599c` - Oprava problému, kdy výsledky nástrojů byly renderovány jako samostatné zprávy

#### Opravy Web UI
- `d9d72e9` - Oprava problému s CSS prioritou modálního okna detailu pracovních poznámek

#### Vylepšení Core Funkcí
- `1e7c7b2` - Zlepšení komprese paměti a sledování provádění nástrojů

#### Rozšíření Lokalizace
- `c13cb17` - Registrace španělské jazykové varianty
- `9c44f34` - Přidání vícejazyčné lokalizace pro Chinese Historical Calendar
- `192fc6e` - Přidání chybějících lokalizací názvů nástrojů pro 5 nástrojů

### 2026-04-23

#### Historie Chatu a Indikátor Načítání
- `e483348` - Implementace funkce zobrazení historie chatu křemíkových bytostí
- `65c157b` - Přidání indikátoru načítání pro chatovací stránku a automatický výběr relace kurátora

#### Vylepšení AI Flow Control
- `30a2d4e` - Rozšíření zrušení AI streamu, IM integrace a inicializace CoreHost

#### Podpora Nahrávání Souborů
- `28fb344` - Implementace dialogu zdroje souboru a podpory nahrávání souborů
- `1d3e2cc` - Přidání lokalizačních řetězců pro dialog zdroje souboru

#### Fronta Chatových Zpráv
- `db48c51` - Přidání fronty chatových zpráv, metadat souboru a podpory zrušení streamu

### 2026-04-22

#### Rozšíření Lokalizace
- `b574b2b` - Přidání senderName pro historické zprávy pro AI identifikaci
- `0a8d750` - Přidání obecného systémového promptu pro aktivní chování křemíkových bytostí

#### Rozšíření Systému Nástrojů
- `70ce7fb` - Implementace DatabaseTool pro strukturované databázové dotazy
- `be29a09` - Implementace LogTool pro dotazy na historii operací a konverzací
- `4ea7702` - Implementace PermissionTool pro dynamickou správu oprávnění
- `1384ff4` - Implementace ExecuteCodeTool pro vícejazyčné provádění kódu
- `82d1e11` - Implementace SearchTool pro vyhledávání informací

#### Refaktoring Systému Logů
- `8f6cb1e` - Přidání parametru beingId do rozhraní ILogger pro oddělení systémových/křemíkových logů
- `2b771f3` - Decoupling LogController od file I/O, přidání API pro čtení logů
- `12da302` - Přidání filtru křemíkových bytostí pro zobrazení logů

#### Vylepšení Systému Oprávnění
- `4c747ad` - Refaktoring PermissionTool, ExecuteCodeTool, přidání EvaluatePermission API

#### Optimalizace Web UI
- `702b3f3` - Rozšíření zobrazení úkolů, přidání status badges a metadat
- `6ed9a79` - Zlepšení ukládání chatových zpráv a renderování pohledů
- `0675c45` - Optimalizace zvýraznění markdown code bloků v preview panelu

#### Integrace Nástrojů
- `135710d` - Odstranění SearchTool, přesunutí lokálního vyhledávání do DiskTool
- `7a03a19` - Zlepšení flexibility dotazů na konverzace v LogTool

#### Správa Konfigurace
- `4305769` - Přidání .gitattributes pro správu konců řádků

#### Opravy Chyb
- `1c96e99` - Oprava selhání vyhledávání v kořenovém adresáři pro search_files a search_content

### 2026-04-21

#### Vylepšení Systému Oprávnění
- `5879621` - Přidání pre-kompilační validace permission callback a rozšíření zpracování chyb
- `833ead2` - Přidání verifikace assembly referencí pro dynamickou kompilaci

#### Vylepšení Web UI
- `0a826f5` - Přidání indikátoru úspěšného uložení v editoru kódu
- `2940373` - Rozšíření Web UI, přidání code hover hints a vylepšení UI
- `6ba591d` - Přidání nezávislého AI konfigurátoru pro křemíkové bytosti
- `634e8ca` - Přidání odkazu na návrat z stránky oprávnění
- `188c6f8` - Registrace API routy pro seznam úkolů a přidání zobrazení prázdného stavu

#### Opravy Chyb
- `592c7ab` - Oprava pořadí instantizace a registrace callbacků
- `c6b518b` - Oprava předávání zpráv časovače a ukládání chatových zpráv

#### Lokalizace
- `7940d9c` - Přidání korejské lokalizace
- `4ff98ad` - Refaktoring dokumentace pro podporu více jazyků

#### AI a Kalendář
- `646813e` - Zlepšení implementace AI client factory
- `928a96d` - Oprava implementace výpočtu kalendáře

#### Konfigurace a Vstupní Bod
- `0fc1693` - Aktualizace vstupního bodu programu a konfigurace projektu

### 2026-04-20

#### Dokonšení Core Funkcí
- `28905b5` - Kompletní vícejazyčná podpora, AI client factory, systém oprávnění a nastavení lokalizace

### 2026-04-19

#### Časovače a Kalendář
- `c933fd8` - Aktualizace lokalizace, systému časovačů, Web pohledů a přidání nástrojů

**Architektonická Vylepšení**
- Redesign architektury Web pohledů pro lepší podporu skinů
- Zlepšení systému správy bytostí s lepším zpracováním stavu

### 2026-04-18

- `9f585e1` - Aktualizace lokalizace, systému časovačů, Web pohledů a přidání nástrojů

### 2026-04-17

- `9b71fcd` - Aktualizace core modulů, přidání zh-HK dokumentace, broadcast channel, ConfigTool a audit Web pohledu

### 2026-04-16

- `5040f05` - Aktualizace core a default modulů

### 2026-04-15

- `3efab5f` - Aktualizace více modulů: AI, Chat, IM, Tools, Web, Localization, Storage

### 2026-04-14

- `4241a2f` - Dokonšení základních funkcí chatu, optimalizace UI pro nahrávání

### 2026-04-13

- `c498c31` - Aktualizace kódu

### 2026-04-12

- `2161002` - Refaktoring dokumentace a rozšíření lokalizace
- `03d94e4` - Rozšíření systému konfigurace a lokalizace
- `9976a35` - Přidání stránky About a lokalizace
- `0c8ccfc` - Rozšíření chatového systému, lokalizace a Web pohledů
- `a8f1342` - Redesign Web komunikační vrstvy, přechod z WebSocket na SSE

### 2026-04-11

- `e8fe259` - Přidání systému logů a optimalizace kódu
- `f01c519` - Přidání systému logů, aktualizace AI rozhraní a Web pohledů

### 2026-04-10

- `4962924` - Rozšíření WebSocket handleru, chat pohledu a IM interakcí

### 2026-04-09

- `f9302bf` - Rozšíření IM provider rozhraní, chatového systému a Web UI interakcí

### 2026-04-07

- `6831ee8` - Redesign Web pohledů a JavaScript builderu

### 2026-04-05

- `41e97fb` - Aktualizace více core modulů a Web controllerů
- `67988d4` - Vylepšení Web UI modulů, přidání Executor pohledu

### 2026-04-04

- `b58bb1c` - Přidání Init controlleru a redesign Web modulů
- `f03ac0b` - Přidání Web UI modulu, vylepšení IM funkcionality

### 2026-04-03

- `192e57b` - Aktualizace struktury projektu a core runtime komponent
- `59faec8` - Aktualizace core a default implementace
- `d488485` - Přidání dynamické kompilace a modulů CuratorTool
- `753d1d9` - Přidání bezpečnostních modulů, aktualizace executorů, IM providerů, lokalizace a nástrojů
- `a378697` - Dokončení Fáze 5 - Systém nástrojů + Executory

### 2026-04-02

- `e6ad94b` - Oprava problému s načítáním historie chatu při smazání konfiguračního souboru během testů
- `daa56f5` - Dokončení Fáze 4: Persistující paměť (Chat System + IM kanál)

### 2026-04-01

- `bbe2dbb` - Oprava načítání konfigurace a routování zpráv ChatService
- `2fa6305` - Implementace Fáze 2: Framework hlavní smyčky a systém Clock objektů
- `32b99a1` - Implementace Fáze 1 - Základní funkce chatu
- `358e368` - Počáteční commit: Dokumentace projektu a licence
