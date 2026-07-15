# Seznam změn

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | **Čeština** | [Русский](../ru-RU/changelog.md)

Všechny důležité změny tohoto projektu budou zaznamenány v tomto souboru.

Formát je založen na [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
tento projekt dodržuje [Sémantické verzování](https://semver.org/spec/v2.0.0.html).

---

## O tomto seznamu změn

### Duální verze projektu

Tento projekt nabízí dvě implementační verze:

- **SiliconLife.Default**：Výchozí implementace, primárně určená pro ověření proveditelnosti architektury. Konzolová aplikace, ukládání do JSON v souborovém systému.
- **SiliconLife.Fast**：Hlavní produkční verze. Multiplatformní desktopová aplikace (Windows / macOS / Linux), úložiště SpeedyPack v paměti + asynchronní perzistence, hluboká optimalizace výkonu.

Obě verze sdílejí stejná rozhraní a funkce, liší se pouze v implementaci úložiště a režimu běhu. SiliconLife.Default slouží jako referenční základ architektury, SiliconLife.Fast jako hlavní produkční verze.

### Původ projektu

- Tento projekt vznikl 20. března 2026.
- Před tímto projektem existovalo ověřovací demo, které selhalo kvůli nevhodnému návrhu architektury, což znemožnilo integraci s více AI platformami.

### Použité AI IDE nástroje

#### Kiro (Amazon AWS)
- Projekt byl původně spravován pomocí Kiro a spuštěn v režimu Spec.
- Kiro je agentic AI vývojové prostředí od Amazon AWS.
- Postaveno na Code OSS (VS Code), podporuje nastavení VS Code a pluginy kompatibilní s Open VSX.
- Nabízí specifikacemi řízený vývojový pracovní postup pro strukturované AI kódování.

#### Comate AI IDE / 文心快码 (Baidu)
- Občas používáno pro redakční a dokumentační práci.
- Comate AI IDE je AI-nativní vývojové prostředí vydané Baidu Wenxin 23. června 2025.
- První multimodální, multi-agentní kolaborativní AI IDE v oboru.
- Funkce zahrnují převod designu na kód a celoprocesovou AI asistenci kódování.
- Poháněno modelem Baidu Wenxin 4.0 X1 Turbo.

#### Trae (ByteDance)
- Používáno v období od října 2025 do dubna 2026.
- AI IDE podporující inteligentní generování kódu a řízení projektů.

#### Qoder (Alibaba)
- Od 18. dubna 2026 používáno pro údržbu projektu.
- AI kódovací platforma podporující analýzu kódu, generování dokumentace a multi-agentní spolupráci.

#### CatPaw (Meituan)
- Od 6. května 2026 používáno v kombinaci s Qoder.
- Postaveno na vlastních modelech Meituan LongCat, s silnými schopností celkové refaktorizace kódové architektury.

### Požadavková dokumentace

- Požadavková dokumentace tohoto projektu není veřejná.
- Požadavky byly opakovaně ověřovány na více než 12 mezinárodních AI platformách a velkých modelových řadách, což vedlo k vytvoření více než 2000 řádků dokumentu požadavků řízeného uživatelskými příběhy, který je pro člověka téměř nesrozumitelný.

---

## [Nezveřejněno]

### Nové AI klienty

- **DeepSeekClient** — přímé připojení k DeepSeek API, thinking mode, reasoning effort, až 1M kontextové okno, endpoint `https://api.deepseek.com`
- **ZhipuClient (GLM)** — přímé připojení k Zhipu AI API, thinking mode, vision podle modelu, bezplatné modely, až 1M kontext, endpoint `https://open.bigmodel.cn/api/paas/v4`
- **ErnieClient (Baidu/Qianfan)** — přímé připojení k Baidu Qianfan v2 API, bezplatné modely (ernie-speed, ernie-tiny), až 131K kontext, endpoint `https://qianfan.baidubce.com/v2`
- **HunyuanClient (Tencent)** — přímé připojení k Tencent Hunyuan API, duální endpoint (TokenHub + Legacy), thinking mode, až 262K kontext
- **MiniMaxClient** — přímé připojení k MiniMax API, domácí/mezinárodní endpoint, adaptivní thinking, multimodální, až 1M kontext, endpoint `https://api.minimaxi.com/v1`
- **MoonshotClient (Kimi)** — přímé připojení k Moonshot/Kimi API, thinking mode, multimodální, až 262K kontext, endpoint `https://api.moonshot.cn/v1`
- **SiliconFlowClient** — přímé připojení k SiliconFlow API, agregátor 100+ open-source modelů, dynamické objevení modelů přes API, reasoning obsah, až 1M kontext, endpoint `https://api.siliconflow.cn/v1`
- **01.AI (Yi)** — označeno jako zastaralé (⚠️), nové registrace ukončeny

### 2026-05-26

#### Nové funkce
- `a49041b` - Přidána podpora ruštiny (ru-RU) lokalizace (ref task-364)
  - 216 změněných souborů

#### Opravy
- `79096f2` - Formát tabulky glossary převeden na standardní Markdown, odstraněny nadbytečné mezery pro zarovnání
  - 1 změněný soubor

#### Dokumentace
- `174a954` - Doplněny chybějící překlady terminologie ve sloupcích Deutsch/Polski/Português v glossary
  - 1 změněný soubor

#### Kolaborační rámec
- `5b03d53` - Aktualizovány záznamy úkolů .ai-collab - task-364 ruská lokalizace (ref task-364)
  - 5 změněných souborů

- `018947d` - Archivovány sessions a changes z 2026-05-25
  - 2 změněné soubory

### 2026-05-25

#### Nové funkce
- `14721a9` - Zjemněny personální tipy ThinkOnProject na detailní proveditelný akční plán (ref task-363)
  - 20 změněných souborů

#### Opravy
- `abb4285` - Opravena chyba umístění volání .join() v beingsHtml (ref task-361)
  - 1 změněný soubor

- `1c0b9ed` - Odstraněn bug s duplicitním řetězcem state-initial způsobeným vykreslením states-overview v WorkflowDetailView (ref task-362)
  - 6 změněných souborů

#### Kolaborační rámec
- `ecc48a1` - Aktualizována metadata .ai-collab (relatedCommit a activity log) (ref task-361)
  - 4 změněné soubory

- `64529a7` - Archivovány sessions a changes z 2026-05-24 (ruční doplnění provedení)
  - 28 změněných souborů

- `4150e52` - Archivovány dokončené úkoly task-341~361 (ref archive)
  - 2 změněné soubory

### 2026-05-24

#### Nové funkce
- `db60fd9` - Seznam oprávnění nástrojů zobrazuje nástroje bez deklarace ToolAction a označuje je jako nenastavitelné (ref task-331, task-332, task-333)
  - 21 změněných souborů

- `6004a7f` - WorkflowTemplate přidána podpora definic rolí + 12 jazyková lokalizace + oprava DiskTool (ref task-346)
  - 24 změněných souborů

- `75ce452` - Role pool ProjectSpace a akce správy rolí ProjectTool (ref task-347)
  - 12 změněných souborů

- `edfb600` - BuildProjectScenarioContext přidány informace o rolích (ref task-348)
  - 21 změněných souborů

- `6a2d713` - HasProjectsWithoutTemplate rozšířeno na HasProjectsNeedingAttention (ref task-349)
  - 21 změněných souborů

- `a773224` - Vytváření úkolů workflow používá role pool pro přiřazení exekutorů (ref task-350)
  - 6 změněných souborů

- `77a27f9` - Rozšířen TravelCodeWikiTool jako vstupní bod pro geografické entity (ref task-353)
  - 8 změněných souborů

- `873ef23` - Implementace GeoDataTool dokončena, aktualizován stav .ai-collab (ref task-352)
  - 7 změněných souborů

- `feaccab` - Implementace GeoContentTool dokončena, aktualizován stav .ai-collab (ref task-351)
  - 6 změněných souborů

- `6e60ad1` - Rozšířen GeoLanguageTool (podpora ObjectPath + set_word), doplněny metadata (ref task-356, task-355)
  - 7 změněných souborů

- `4eff807` - Implementováno GetWikiDocuments() v podtřídách GeoLocation (ref task-357)
  - 5 změněných souborů

- `baad5df` - Implementována služba publikace MediaWiki API (ref task-358)
  - 6 změněných souborů

- `b846a21` - Implementována stránka detailu workflow (ref task-361)
  - 24 změněných souborů

#### Opravy
- `a290088` - Křemíková bytost vytvořená přes CuratorTool se ztrácí po restartu (ref task-334)
  - 11 změněných souborů

- `69a8cba` - Opraven bug, kdy stránka úkolů nefiltrovala podle beingId (ref task-360)
  - 8 změněných souborů

- `7dd1a65` - Registrace routy stránky detailu workflow v Router.cs (ref task-361)
  - 1 změněný soubor

#### Refaktoring
- `5e02711` - Refaktorována abstrakce cest úložiště ve veřejné vrstvě, odstraněno hardcodování souborového systému (ref task-335)
  - 12 změněných souborů

- `0ec0929` - DynamicBeingLoader.SaveBeingCode používá IStorage místo přímých operací souborového systému (ref task-336)
  - 7 změněných souborů

- `9a44b48` - PlaywrightWebView IStorage bridge + decoupling základní třídy WebViewBrowserTool (ref task-337, task-340)
  - 11 změněných souborů

- `8fea742` - Ukládání snímků obrazovky WebViewBrowserTool používá IStorage místo přímých operací souborového systému (ref task-338)
  - 6 změněných souborů

- `4c24e6d` - DefaultPermissionCallback používá BeingPathResolver místo hardcodovaných cest (ref task-339)
  - 6 změněných souborů

- `ab428cd` - Odstraněn downcasting v DefaultSiliconBeing, přímé volání SaveState() základní třídy (ref task-344)
  - 7 změněných souborů

- `1e6eb80` - Dočasné soubory stavu prohlížeče PlaywrightWebView převedeny na přímé čtení/zápis přes IStorage (ref task-341)
  - 7 změněných souborů

- `17f00e9` - Vyhledávací operace DiskTool převedeny přes DiskExecutor (ref task-342)
  - 8 změněných souborů

- `8158703` - Kontrola příloh ChatController převedena přes DiskExecutor (ref task-343)
  - 7 změněných souborů

- `3243ae6` - Přepsán TravelCodeWikiPublishWorkflow jako 7-krokový stavový automat, odstraněny vynuceně sledované soubory TravelCodeWikiWithAI (ref task-355)
  - 6 změněných souborů

#### Čištění
- `d685288` - Smazány HotReloadTool.cs a adresář tools/HotReload (ref task-345)
  - 8 změněných souborů

#### Dokumentace
- `f1789d1` - Optimalizován popisný řádek v README.md (ref task-359)
  - 9 změněných souborů

#### Kolaborační rámec
- `982c6bb` - Doplněny chybějící pole relatedCommit a commitHash v .ai-collab
  - 6 změněných souborů

- `d91e9f8` - Archivovány task-331~340, task board vyčištěn
  - 2 změněné soubory

- `9135e30` - Zveřejněny task-341~344 refaktoring IStorage ve veřejné vrstvě + oprava abstrakce
  - 1 změněný soubor

- `f70b350` - Přidáno 13 úkolů architektonické transformace TravelCodeWikiWithAI (ref task-346~358)
  - 2 změněné soubory

- `f81d38b` - Aktualizovány soubory ai-collab session a task tracking
  - 3 změněné soubory

### 2026-05-23

#### Opravy
- `9c3c64e` - Opraveno obejití ověření oprávnění za běhu v ExecuteTool u projektových omezení (ref task-324)
  - 7 změněných souborů

- `94a9e35` - Opravena nekonzistence mezi definicí šablony oprávnění a deklarací ToolActionAttribute (ref task-325)
  - 6 změněných souborů

- `e8d8371` - Nástroje se všemi zakázanými akcemi jsou zcela odstraněny z AI požadavků (ref task-326)
  - 6 změněných souborů

- `32c7d8a` - API oprávnění nástrojů přidáno ověření názvu akce + oprava vykreslování Markdown v historii chatu (ref task-327, task-328, task-329)
  - 9 změněných souborů

- `797db8c` - Fallback vykreslování Markdown nesprávně nastavuje mdRendered, což způsobuje, že se po načtení marked neznovu vykresluje (ref task-330)
  - 9 změněných souborů

#### Kolaborační rámec
- `1496094` - Zveřejněny opravné úkoly task-324~327 rámce oprávnění nástrojů
  - 776 změněných souborů

- `0d16e63` - Aktualizován stav kolaboračních úkolů, přiřazen task-330 ke commitu 797db8c, příprava na archivaci
  - 2 změněné soubory

- `e602e1c` - Archivovány task-316~330, task board vyčištěn (ref task-316~330)
  - 2 změněné soubory

- `20291ce` - Denní archivace sessions a changes (13.-22. května)
  - 106 změněných souborů

### 2026-05-22

#### Opravy konzistence dokumentace
- `9e07b27` - Opraveny rozdíly v konzistenci mezi francouzskou dokumentací (fr-FR) a zdrojovým kódem (ref task-307)
  - 10 změněných souborů

- `9e3be72` - Opravena konzistence mezi německou dokumentací (de-DE) a zdrojovým kódem (ref task-308)
  - 5 změněných souborů

- `2bc7151` - Opraveny rozdíly v konzistenci mezi španělskou dokumentací (es-ES) a zdrojovým kódem (ref task-309)
  - 13 změněných souborů

- `f95088e` - Opravena konzistence mezi italskou dokumentací (it-IT) a zdrojovým kódem (ref task-310)
  - 11 změněných souborů

- `6ea9f4a` - Opravena konzistence mezi polskou dokumentací (pl-PL) a zdrojovým kódem (ref task-311)
  - 16 změněných souborů

- `7646923` - Opravena konzistence mezi portugalskou dokumentací (pt-PT) a zdrojovým kódem (ref task-312)
  - 12 změněných souborů

- `7eaf9db` - Opravena konzistence mezi českou dokumentací (cs-CZ) a zdrojovým kódem (ref task-313)
  - 12 změněných souborů

#### Kolaborační rámec
- `3cb7347` - Aktualizován task-313 relatedCommit=7eaf9db
  - 1 změněný soubor

### 2026-05-21

#### Nové funkce
- `99eca78` - Kontextové menu přidáno "Zobrazit úložiště (jen pro čtení)", volání Speedy.Manager v procesu (ref task-301)
  - 26 změněných souborů

#### Opravy konzistence dokumentace
- `7f65cf1` - Opraveny rozdíly v konzistenci mezi dokumentací zh-CN a zdrojovým kódem (ref task-303)
  - 15 změněných souborů

- `a9e2a2c` - Opraveny rozdíly v konzistenci mezi anglickou dokumentací (en) a zdrojovým kódem (ref task-302)
  - 9 změněných souborů

- `2549105` - Opraveny rozdíly v konzistenci mezi dokumentací v tradiční čínštině (zh-HK) a zdrojovým kódem (ref task-304)
  - 12 změněných souborů

- `277eb50` - Opraveny rozdíly v konzistenci mezi japonskou dokumentací a zdrojovým kódem (ref task-305)
  - 10 změněných souborů

- `edce413` - Opraveny rozdíly v konzistenci mezi korejskou dokumentací (ko-KR) a zdrojovým kódem (ref task-306)
  - 18 změněných souborů

- `f2adcae` - Opraven problém nekonzistence portugalské dokumentace se zdrojovým kódem (ref task-220)
  - 15 změněných souborů

- `3332987` - Opraven problém nekonzistence dokumentace v tradiční čínštině (Hongkong) se zdrojovým kódem (ref task-218)
  - 14 změněných souborů

- `af9f715` - Opraven problém nekonzistence polské dokumentace se zdrojovým kódem (ref task-217)
  - 15 změněných souborů

- `2e2b18b` - Opraven problém nekonzistence korejské dokumentace se zdrojovým kódem (ref task-216)
  - 16 změněných souborů

- `626ebc9` - Opraven problém nekonzistence japonské dokumentace se zdrojovým kódem (ref task-215)
  - 19 změněných souborů

- `48d061b` - Opraven problém nekonzistence italské dokumentace se zdrojovým kódem (ref task-214)
  - 14 změněných souborů

#### Kolaborační rámec
- `6683bee` - Registrován tým Marvis AI, aktualizován stav úkolů
  - 3 změněné soubory

- `03fc905` - Archivovány task-210~220
  - 5 změněných souborů

### 2026-05-20

#### Nové funkce
- `65176d4` - Přidána kompletní podpora lokalizace portugalštiny (pt-PT + pt-BR) (ref task-208)
  - 41 změněných souborů

#### Opravy konzistence dokumentace
- `af4dffd` - Opraveny všechny nekonzistence mezi dokumentací zh-CN a zdrojovým kódem (ref task-209)
  - 11 změněných souborů

- `144b945` - Opraveny nekonzistence mezi anglickou (en) a českou (cs-CZ) dokumentací a zdrojovým kódem (ref task-219, task-210)
  - 22 změněných souborů

- `08bec55` - Opraveny nekonzistence mezi německou dokumentací (de-DE) a zdrojovým kódem (ref task-211)
  - 14 změněných souborů

- `7ff28de` - Opraveny nekonzistence mezi španělskou dokumentací (es-ES) a zdrojovým kódem (ref task-212)
  - 14 změněných souborů

- `15e2133` - Opraveny nekonzistence mezi francouzskou dokumentací (fr-FR) a zdrojovým kódem (ref task-213)
  - 13 změněných souborů

#### Opravy
- `7dac388` - Opraveno nezobrazování seznamu projektových úkolů (ref task-207)
  - 6 změněných souborů

#### Kolaborační rámec
- `7890223` - Archivovány task-201~209, zveřejněny úkoly task-210~220 pro opravu konzistence dokumentace
  - 5 změněných souborů

### 2026-05-19

#### Nové funkce
- `cd72846` - Implementováno bezpečné alternativní řešení pro obejití bezpečnostního skenu PluginLoader (ref task-203)
  - 13 změněných souborů

- `fc0c00c` - Vylepšení Speedy.Manager - vytvoření/import/export/TreeView hierarchie/okno průběhu (ref task-206)
  - 9 změněných souborů

#### Opravy
- `ec07118` - Opraven problém, kdy ITypeRegistry/IObjectFactory nebyly registrovány před načtením pluginů (ref task-205)
  - 8 změněných souborů

- `9e749db` - Opravena chyba "Creator ID is required" při vytváření projektu (ref task-204)
  - 4 změněné soubory

#### Infrastruktura
- `43dc092` - Migrace CLDR - přidán CldrDataProvider, odstraněn .github
  - 1 změněný soubor

- `c09ec1f` - Přidáno cldr/ do .gitignore
  - 1 změněný soubor

- `221f818` - Synchronizace GitHub převedena na schéma zrcadlení Gitee push, workflow ponecháno pouze jako ruční záloha
  - 1 změněný soubor

- `08cdf1a` - Opraven synchronizační workflow GitHub - přidána logika opakování a přeskočení při žádných změnách
  - 1 změněný soubor

- `fb4e77d` - Aktualizován SiliconLife.Speedy.Manager.csproj
  - 1 změněný soubor

#### Kolaborační rámec
- `df90af0` - Aktualizován task-203 relatedCommit=cd72846
  - 1 změněný soubor

### 2026-05-18

#### Refaktoring
- `e720d06` - Speedy.Manager kompletně přepracován z WinForms na Avalonia (ref task-202)
  - 17 změněných souborů

#### Opravy
- `08894a9` - Opravena chyba zobrazení úrovně položek v časové ose paměti (ref task-201)
  - 3 změněné soubory

#### Kolaborační rámec
- `2871afb` - Archivovány všechny úkoly, tasks.json vyčištěn
  - 2 změněné soubory

### 2026-05-17

#### Nové funkce
- `d6eb994` - Stránka seznamu projektů přidala vstup pro vytvoření projektu a výběr šablony workflow (ref task-203)
  - 14 změněných souborů

- `0872134` - ThinkOnProject orchestrace řízená Kurátorem pro projekty bez šablony (ref task-202)
  - 6 změněných souborů

- `cb3188e` - Vizualizace @zmínky ve skupinovém chatu (ref task-208)
  - 4 změněné soubory

- `f9968e5` - Deklarace schopnosti ToolCall AI klienta a elegantní degradace (ref task-205)
  - 4 změněné soubory

- `0d2b843` - Rozhodovací logika skupinového chatu ShouldReplyInGroupChat (ref task-201)
  - 6 změněných souborů

- `277a2b1` - Doplnění znalostní sítě - pokročilé dotazy a průchod grafem (ref task-207)
  - 9 změněných souborů

#### Opravy
- `6d0b66e` - Opraven appendMessage TypeError při odesílání zpráv ve skupinovém chatu (ref task-209)
  - 5 změněných souborů

- `b15167c` - Doplněna chybějící registrace routy list-workflow-templates z commitu task-203 (ref task-203)
  - 1 změněný soubor

- `dc549a2` - Opraven synchronizační workflow Gitee - přidáno uživatelské jméno do URL tokenu
  - 1 změněný soubor

#### Infrastruktura
- `e5fa3ad` - Zakázán automatický plán synchronizace GitHub, čeká na oficiální řešení synchronizace Gitee
  - 1 změněný soubor

#### Kolaborační rámec
- `4a58c82` - Přidána zpráva o analýze systémových schopností + návrh ThinkOnProject
  - 5 změněných souborů

- `8ab29e6` - Zpráva o úplnosti systémových schopností archivována do .ai-collab/docs
  - 2 změněné soubory

- `b412d9c` - Archivovány staré úkoly, na základě komplexní analýzy znovu zveřejněny task-201~208
  - 2 změněné soubory

- `437884a` - Aktualizována kolaborační metadata - task-202/203/204 dokončeny (ref task-202, task-203, task-204)
  - 2 změněné soubory

- `bf78d79` - Aktualizována kolaborační metadata - task-201/205/208 dokončeny
  - 2 změněné soubory

- `de6ee0e` - Záznam o ukončení relace catpaw-20260517-2215
  - 5 změněných souborů

- `7223b6f` - Záznam o ukončení relace catpaw-20260517-2200
  - 4 změněné soubory


## [Alpha-0.2] - 2026-05-16

### 2026-05-16

#### Příprava vydání
- `476d839` - Přidány úkoly pro vydání alpha-0.2
  - Vytvořen task-114 (psaní CHANGELOG) a task-115 (aktualizace čísla verze)
  - 1 změněný soubor

### 2026-05-15

#### Infrastruktura
- `672627b` - Přidán synchronizační workflow Gitee (s konfigurací oprávnění)
  - Aktualizována konfigurace oprávnění workflow sync-from-gitee.yml
  - 1 změněný soubor, 7 řádků přidáno, 4 řádků odstraněno

- `3cd5256` - Přidány GitHub Actions pro automatickou synchronizaci kódu z Gitee
  - Nový workflow sync-from-gitee.yml
  - 1 změněný soubor, 50 řádků přidáno

#### Aktualizace dokumentace
- `aa1d2ad` - Aktualizována všechna README/architektura/začínáme dokumenty pro 11 jazyků, odrážející multiplatformní podporu SiliconLife.Fast (ref task-112, task-113)
  - Opraven popis v dokumentaci, že SiliconLife.Fast je pouze pro Windows, aby odrážel skutečnou multiplatformní podporu (Windows / macOS / Linux)
  - Aktualizovány README.md, architecture.md, getting-started.md pro 11 jazyků
  - SelectComponent přidána podpora atributu hint
  - ConfigView enum dropdown předán hint
  - 11 jazyků lokalizace přidán klíč SelectSearchHint
  - 53 změněných souborů, 690 řádků přidáno, 194 řádků odstraněno

#### Úkolový systém
- `3329f3d` - Přidán mechanismus inspekce úkolového systému + úkoly opravy lokalizačních bugů
  - Vytvořen task-113: oprava problému lokalizace na stránce O aplikaci
  - Aktualizován task-112: aktualizace dokumentace Fast verze pro podporu Linuxu
  - Archivovány dokončené úkoly (11) do .ai-collab/archive/
  - Konfigurace mechanismu inspekce dokončena: rychlá inspekce (každých 30 minut) + úplná inspekce (denně v 06:00)
  - 2 změněné soubory, 148 řádků přidáno, 171 řádků odstraněno

#### Kolaborační rámec
- `6038e22` - Registrován coze-agent do registru .ai-collab
  - Přidány registrační informace stálého AI platformy Coze
  - 1 změněný soubor

### 2026-05-14

#### AI kolaborační rámec
- `7344fbb` - Odstraněn režim handoff, převedeno na řízení seznamem úkolů (v2.0)
  - Refaktorována struktura adresáře .ai-collab, z režimu předávání handoff na řízení seznamem úkolů
  - Přidán soubor tasks.json jako jádro seznamu úkolů
  - Přidán activity log operací
  - Přidány adresáře changes/ a sessions/

- `589a48e` - Přidány záznamy relací .ai-collab
  - Přidány záznamy stavu relací AI spolupráce

- `5481bcf` - Registrován Qoder AI IDE do kolaboračního registru
  - Přidány registrační informace AI programovacího asistenta Qoder

- `e2d7b61` - Doplněny relatedCommit v tasks.json a commitHash v changes
  - Vylepšeno propojení metadat úkolů

- `a087f0c` - Uzavřeny všechny úkoly task-101~110
  - Potvrzeno dokončení všech 10 úkolů oprav

#### Opravy bugů
- `fac9435` - Dokončeny všechny 10 oprav a implementací úkolů task-101~110
  - Opraveno chybějící nápovědní texty v komponentě vyhledávání a výběru
  - Opraveny problémy lokalizace na stránce O aplikaci
  - Opravena chyba JS ve vyhledávání systému nápovědy
  - 39 změněných souborů, 684 řádků přidáno, 121 řádků odstraněno

- `c46dfbc` - Dokončeny všechny nevyřízené úkoly (task-001~006)
  - Dokončeno prvních 6 nevyřízených úkolů

- `ec176b2` - Přepsán seznam úkolů - code review odhalil 10 nových bugů
  - Vytvořeno 10 nových úkolů task-101~110

#### Refaktoring
- `ab15915` - Unifikovány hlavičky autorských práv + opraven BOM v HelpController a JS vyhledávání v HelpView
  - Unifikovány hlavičky autorských práv Apache 2.0 ve všech C# zdrojových souborech
  - Opraven problém kódování BOM v HelpController
  - Opravena chyba JavaScriptu ve vyhledávání HelpView

#### Nové funkce
- `18a6f5d` - Vytvořen MCP server schopností prohlížeče (ref task-111)
  - Přidán projekt SiliconLife.McpServer
  - Implementován MCP server automatizace prohlížeče Playwright

- `9eb251a` - Odstraněn modul SiliconLife.McpServer (ref task-111)
  - Odstraněn samostatný MCP server, funkce integrovány do hlavního projektu

### 2026-05-13

#### Lokalizace
- `7a62590` - Přidána podpora polské lokalizace
  - Nová implementace lokalizace pl-PL (PlPL.cs, 1089 řádků)
  - Nová lokalizace polské nápovědy (HelpLocalizationPlPL.cs, 3972 řádků)
  - Nová podpora polské lokalizace čínského historického kalendáře (ChineseHistoricalPlPL.cs, 600 řádků)
  - Nová lokalizace polského systémového panelu (TrayPlPL.cs, 135 řádků)
  - Nová kompletní sada polské dokumentace (15 dokumentů)
  - Enum Language přidána polština
  - 35 změněných souborů, 14379 řádků přidáno, 11 řádků odstraněno

- `51f9c8e` - Aktualizovány odkazy na Ark AI v dokumentaci a vylepšena terminologie
  - Aktualizována terminologie AI klientů v vícejazyčné dokumentaci

- `7587c12` - Přidány položky seznamu změn pro všechny jazyky
  - Synchronizovány aktualizace changelog pro všechny jazykové verze

#### Migrace okenního systému
- `b49a07d` - Migrace na režim trvalého okna Avalonia
  - Odstraněna závislost na Windows Forms, kompletní migrace na UI framework Avalonia
  - Okno stavu se správně zobrazuje na Linuxu (ověřeno přes vzdálenou plochu)
  - Přidány ovládací prvky okna: kontextové menu, dvojité kliknutí pro otevření webu, tlačítko zavření
  - Přidán rámec multi-AI spolupráce (.ai-collab/)
  - Opravena inicializace ikony systémového panelu (elegantní degradace)
  - Přidány App.axaml a App.cs jako vstupní body aplikace Avalonia
  - 13 změněných souborů, 1442 řádků přidáno, 541 řádků odstraněno

- `d335aaf` - Okno vždy zobrazeno na platformě Linux + potvrzovací dialog při zavření
  - Na Linuxu automaticky zobrazeno stavové okno (bez ikony systémového panelu)
  - Na Linuxu potvrzovací dialog při zavření okna
  - Windows/macOS zachovává původní chování systémového panelu
  - Podpora parametru --no-tray pro vynucené zakázání systémového panelu
  - Přidána metoda ShowMessageBoxAsync pro potvrzovací dialog
  - 3 změněné soubory, 206 řádků přidáno, 0 řádků odstraněno

#### Refaktoring systémového panelu
- `841d384` - Refaktorován systémový panel a inicializován rámec AI spolupráce
  - Zjednodušen TrayLocalizationBase, odstraněny nepoužívané vlastnosti
  - Přidána lokalizační položka ShowStatus
  - App.cs přidáno kliknutí na ikonu panelu pro zobrazení stavového okna, lokalizované položky menu
  - Program.cs přesunuta inicializace ikony panelu do StartAsync
  - TrayStatusWindow se při zavření skrývá místo ukončení
  - Registrovány trae-glm5 a catpaw do kolaboračního rámce .ai-collab
  - Aktualizován .gitignore pro zajištění sledování všech souborů .ai-collab
  - 22 změněných souborů, 178 řádků přidáno, 1226 řádků odstraněno

#### Dokumentace
- `43653bc` - Aktualizován popis repozitáře a registr AI
  - Aktualizován README projektu a registrační informace .ai-collab

### 2026-05-12

#### Webové zobrazení úkolového systému
- `0891b3c` - Přidáno zobrazení detailu provádění úkolů a historie
  - Nové zobrazení TaskExecutionDetailView detailu provádění úkolů
  - Nové zobrazení TaskExecutionHistoryView historie provádění úkolů
  - TaskController přidány rozhraní pro dotazy na detail provádění a historii
  - Nový TaskViewModel model zobrazení úkolů
  - Vylepšen TaskCenter centrum úkolů
  - Aktualizován TaskSystem úkolový systém
  - 9 jazyků lokalizace přidány klíče související s úkoly
  - 26 změněných souborů, 803 řádků přidáno, 55 řádků odstraněno

### 2026-05-11

#### Refaktoring architektury webových komponent
- `5e687ad` - Migrace vykreslování komponent z řetězců na H-tree
  - Metoda vykreslování ComponentBase migrována z řetězcového režimu na strukturu H-tree
  - Všechny 28 komponent adaptovány na novou vykreslovací architekturu (A, Accordion, Button, Calendar, Card, Chart atd.)
  - SelectComponent výrazně refaktorován (889 řádků vylepšení)
  - Kontrolery a zobrazení synchronně aktualizovány
  - 33 změněných souborů, 667 řádků přidáno, 435 řádků odstraněno

- `bfd332d` - Migrace Style z řetězců na inline styly CssBuilder
  - Přidán CssBuilder stylový tvůrce
  - Stylový systém ComponentBase migrován z řetězců na strukturovaný CssBuilder
  - LoadingComponent výrazně vylepšen (103 řádků přidáno)
  - Migrace stylů kontrolerů ConfigController, LogController, MemoryController
  - Migrace stylů zobrazení ChatView, ConfigView, LogView, MemoryView
  - 37 změněných souborů, 351 řádků přidáno, 157 řádků odstraněno

#### Optimalizace úložného systému
- `d67a7ee` - Optimalizován dotaz QueryLatest pro velké datové sady
  - Optimalizace výkonu metody SpeedyTimeStorage QueryLatest
  - Vylepšení poskytovatele protokolů SpeedyLoggerProvider
  - 2 změněné soubory, 44 řádků přidáno, 5 řádků odstraněno

#### Refaktoring kalendářního systému
- `9629f88` - Extrahován TimerExecution a vylepšeno webové zobrazení časovačů
  - TimerSystem extrahována logika TimerExecution (175 řádků odstraněno)
  - SelectComponent výrazně vylepšen (427 řádků vylepšení)
  - TimerController a zobrazení časovačů vylepšeny
  - ContextManager správce kontextu aktualizován
  - 12 změněných souborů, 458 řádků přidáno, 267 řádků odstraněno

#### Lokalizace
- `5d8ca79` - Přidány lokalizační klíče LogsLoading
  - 9 jazyků přidány klíče LogsLoading
  - Základní třída DefaultLocalizationBase přidána definice
  - 11 změněných souborů, 15 řádků přidáno

### 2026-05-10

#### Refaktoring úkolového systému
- `54394f6` - Sloučen úkolový systém s cyklem historie chatu
  - ProjectTaskSystem projektový úkolový systém výrazně zjednodušen (411 řádků refaktorováno)
  - TaskSystem úkolový systém zjednodušen (254 řádků refaktorováno)
  - TaskCenter centrum úkolů refaktorováno (188 řádků vylepšení)
  - ContextManager správce kontextu optimalizován (347 řádků refaktorováno)
  - DefaultSiliconBeing křemíková bytost vylepšena
  - TimerSystem systém časovačů integrován s úkoly
  - Rozhraní IWorkNoteStorage aktualizováno
  - SpeedyWorkNoteStorage a FileSystemWorkNoteStorage adaptovány
  - 16 změněných souborů, 648 řádků přidáno, 897 řádků odstraněno

### 2026-05-09

#### Vylepšení webového rozhraní
- `bc50dd7` - Vylepšeno zobrazení chatu a přidána funkce auditu
  - Nový AuditController auditní kontroler (261 řádků)
  - Nové AuditView auditní zobrazení (379 řádků)
  - Nový AuditViewModel model auditního zobrazení
  - ChatView zobrazení chatu výrazně vylepšeno (171 řádků vylepšení)
  - ChatController kontroler chatu aktualizován
  - Komponenta MarkdownEditorComponent vylepšena
  - InitController inicializační kontroler vylepšen
  - ChatSystem chatovací systém přidány nové funkce
  - 14 změněných souborů, 1030 řádků přidáno, 112 řádků odstraněno

- `c9babce` - Vylepšeno vykreslování volání nástrojů v zobrazení chatu
  - Vylepšeno vykreslování bloků volání nástrojů v ChatView
  - 1 změněný soubor, 54 řádků přidáno, 11 řádků odstraněno

#### Systém scénářů AI nástrojů
- `ff2eddd` - Implementován systém filtrování scénářů nástrojů
  - Nový atribut ToolScenarioAttribute scénáře nástrojů (36 řádků)
  - Nový atribut ChatOnlyAttribute pouze pro scénář chatu (19 řádků)
  - Správce Nástrojů ToolManager přidána funkce filtrování scénářů (40 řádků)
  - ContextManager správce kontextu adaptován na filtrování scénářů
  - 4 změněné soubory, 115 řádků přidáno, 30 řádků odstraněno

- `5709a33` - Přidány atributy scénářů ke třídám nástrojů
  - 24 třídám nástrojů přidány atributy ToolScenario
  - Včetně nástrojů kalendáře, chatu, konfigurace, kurátor, databáze, disku, dynamické kompilace atd.
  - 24 změněných souborů, 46 řádků přidáno, 20 řádků odstraněno

#### Refaktoring úkolového systému
- `2f19a5f` - Refaktorován úkolový systém pomocí TaskCenter a TaskEnumerator
  - Nový TaskCenter centrum úkolů (235 řádků)
  - Nový TaskEnumerator enumerátor úkolů (297 řádků)
  - TaskSystem úkolový systém refaktorován a zjednodušen
  - DefaultSiliconBeing křemíková bytost adaptována na novou architekturu
  - DefaultSiliconBeingFactory továrna aktualizována
  - SiliconBeingBase základní třída vylepšena
  - 7 změněných souborů, 796 řádků přidáno, 275 řádků odstraněno

#### Migrace systému oprávnění
- `a06ed09` - Migrace IM a systému oprávnění do projektu App
  - PermissionRequestQueue migrována z Default/Fast do projektu App (443 řádků přidáno)
  - Odstraněn WebUIProvider verze Default (403 řádků odstraněno)
  - Odstraněn HelpTool verze Default (194 řádků odstraněno)
  - Odstraněny duplicitní PermissionRequestQueue ve verzích Default/Fast
  - Odstraněn IMPermissionAskHandler verze Default
  - PermissionRequestController kontroler aktualizován
  - 14 změněných souborů, 496 řádků přidáno, 1183 řádků odstraněno

#### Optimalizace AI kontextu
- `4c8aaff` - Optimalizován správce kontextu a vylepšen service locator
  - ContextManager správce kontextu zjednodušen a optimalizován
  - ServiceLocator service locator vylepšen (36 řádků přidáno)
  - Správce Nástrojů ToolManager vylepšen (34 řádků přidáno)
  - DashScopeClient a VolcengineArkClient klienti vylepšeni
  - Exekutoři (CommandLine, Disk, Network) aktualizováni
  - 8 změněných souborů, 116 řádků přidáno, 98 řádků odstraněno

#### Lokalizace
- `5c5eef7` - Přidány lokalizační klíče pro audit a úkoly
  - DefaultLocalizationBase přidáno 127 řádků lokalizačních definic
  - 9 jazyků přidány klíče související s auditem a úkoly (každý 26 řádků)
  - 11 změněných souborů, 387 řádků přidáno

#### Konfigurace projektu
- `2067db6` - Aktualizována konfigurace projektu a pravidla gitignore
  - Pravidla .gitignore aktualizována
  - DefaultConfigData a Fast DefaultConfigData konfigurace vylepšeny
  - SpeedyWorkNoteStorage úložiště vylepšeno
  - SpeedyPack jádro vylepšeno
  - 5 změněných souborů, 32 řádků přidáno, 6 řádků odstraněno

### 2026-05-07

#### Italská lokalizace
- `8adc18c` - Přidána podpora italské lokalizace a aktualizována vícejazyčná dokumentace
  - Nová lokalizace it-IT
  - Nová implementace ItIT (1909 řádků)
  - Nová podpora italské lokalizace čínského historického kalendáře ChineseHistoricalItIT (586 řádků)
  - Nová lokalizace italského systémového panelu TrayItIT (135 řádků)
  - Nová kompletní sada italské dokumentace (14 dokumentů: README, API reference, architektura, kalendářní systém, seznam změn, průvodce přispívání atd.)
  - Aktualizována architektura, vývojářský průvodce a průvodce začínáme pro všechny jazykové verze
  - Enum Language přidána italština
  - 86 změněných souborů, 11573 řádků přidáno, 769 řádků odstraněno

#### Synchronizace dokumentace
- `12a5deb` - Aktualizována vícejazyčná dokumentace architektury, seznamu změn a průvodce křemíkovými bytostmi
  - Aktualizován README pro 8 jazyků
  - Aktualizována dokumentace architektury pro 8 jazyků
  - Aktualizován seznam změn pro 8 jazyků
  - Aktualizován průvodce křemíkovými bytostmi pro 8 jazyků
  - Aktualizován odkaz na nástroje pro 8 jazyků
  - Refaktorován glosář
  - 46 změněných souborů, 1697 řádků přidáno, 442 řádků odstraněno

### 2026-05-06

#### Rozsáhlá refaktorizace modulů
- `eeb3be6` - Rozsáhlá refaktorizace a reorganizace modulů
  - Úprava struktury projektu SiliconLife.App
  - Reorganizace projektu SiliconLife.Fast
  - Reorganizace projektu SiliconLife.Default
  - Reorganizace sdíleného modulu SiliconLife.Common
  - Reorganizace jádrového modulu SiliconLife.Core
  - Reorganizace úložného enginu SiliconLife.Speedy
  - Reorganizace správcovského nástroje SiliconLife.Speedy.Manager
  - 119 změněných souborů, 6926 řádků přidáno, 3066 řádků odstraněno

### 2026-05-04

#### AI klient
- `24d2c86` - Přidán VolcengineArkClient a nahrazen Audit sledováním Usage
  - Nový VolcengineArkClient AI klient Volcengine Ark
  - Podpora streamovacího a nestreamovacího režimu
  - Vestavěná dvouúrovňová kontrola rychlosti (vlastní řízení rychlosti + serverové limity rychlosti)
  - Kompatibilní s protokolem OpenAI API
  - Systém Audit nahrazen sledováním Usage
  - 24 změněných souborů, 802 řádků přidáno, 21 řádků odstraněno

#### Systém nástrojů
- `f27650a` - Přidán nástroj hot reload pro automatický restart Fast
  - Nový HotReloadTool nástroj hot reload
  - Podpora online kompilace, aktualizace a restartu SiliconLife.Fast
  - Nový HotReload.exe samostatný aktualizátor
  - Bezpečný mechanismus kopírování souborů (nepřepisuje sebe sama)
  - Elegantní vypnutí a čekání na uvolnění portu
  - 9 změněných souborů, 581 řádků přidáno

#### Lokalizace
- `6a5aad8` - Aktualizovány všechny soubory a přidána podpora francouzské lokalizace
  - Nová lokalizace fr-FR
  - Aktualizovány všechny jazykové verze
  - Francouzský překlad dokumentace nápovědy
  - Francouzský překlad rozhraní
  - 100+ změněných souborů

### 2026-05-03

#### Projektová infrastruktura
- `2664b0c` - Aktualizována projektová infrastruktura a závislosti
  - SiliconLife.Speedy.Manager přidáno WPF rozhraní pro správu (MainForm.Designer.cs, MainForm.resx)
  - Nový ikonový zdroj slc.ico (1.5MB)
  - PluginLoader výrazně vylepšen bezpečnostní sken (622 řádků přidáno)
  - Nová továrna streamů s oprávněními PermissionedStreamFactory (779 řádků)
  - Nová fronta žádostí o oprávnění PermissionRequestQueue (verze Default a Fast)
  - Nový poskytovatel ladících protokolů DebugLoggerProvider
  - Základní třída ConfigDataBase konfigurace vylepšena
  - ToolManager přidána funkce skenování pluginových nástrojů (ScanAllPluginAssemblies)
  - SiliconBeingManager vylepšena správa životního cyklu
  - DashScopeClient AI klient Alibaba Cloud výrazně vylepšen (227 řádků přidáno)
  - DefaultSiliconBeingFactory továrna vylepšena
  - Aktualizována webová zobrazení a kontrolery (ChatView, WorkNoteView, PermissionRequestController)
  - 9 jazyků lokalizace přidány nové klíče
  - 35 změněných souborů, 28080 řádků přidáno, 336 řádků odstraněno

### 2026-05-02

#### Vylepšení AI klienta
- `c16f99f` - Aktualizováni AI klienti, webové UI a komponenty úložiště
  - DashScopeClient klient Alibaba Cloud výrazně vylepšen
  - SpeedyPackAutoCompactor automatický kompaktor optimalizován
  - Základní třída webových zobrazení a BeingView vylepšeny
  - 6 změněných souborů, 240 řádků přidáno, 81 řádků odstraněno

#### Systém zásuvných modulů
- `242dc98` - Přidán seznam zásuvných modulů na stránku O aplikaci
  - AboutController přidáno zobrazení informací o pluginech
  - AboutViewModel přidán datový model pluginů
  - AboutView přidáno vykreslení seznamu pluginů
  - 9 jazyků lokalizace přidány klíče související s pluginy
  - 14 změněných souborů, 160 řádků přidáno, 1 řádek odstraněn

#### AI optimalizace
- `147f8f4` - Zjednodušen text výzvy kontextové paměti
  - ContextManager optimalizovány AI prompt texty
  - 1 změněný soubor, 1 řádek přidán, 1 řádek odstraněn

#### Optimalizace Speedy úložiště
- `8bda2d3` - Aktualizována implementace Speedy úložiště a kontroleru paměti
  - Opraven interval SpeedyPackAutoCompactor
  - Optimalizováno zpracování cest v SpeedyTimeStorage
  - MemoryController kontroler paměti vylepšen
  - SpeedyPack.Manager UI aktualizováno
  - 4 změněné soubory, 21 řádků přidáno, 18 řádků odstraněno

#### Vylepšení systémového panelu
- `8972654` - Vylepšena lokalizační podpora stavového okna systémového panelu
  - 9 jazyků lokalizace panelu přidán vstup pro správu Speedy
  - TrayStatusWindow přidána položka menu správy Speedy
  - 11 změněných souborů, 72 řádků přidáno

#### Optimalizace Speedy.Manager
- `6f5db09` - Optimalizováno UI SpeedyPack Manageru a interní komponenty
  - MainForm rozhraní refaktorováno
  - Optimalizována správa paměti FreeList
  - WriteQueue fronta zápisů vylepšena
  - SpeedyPack jádro optimalizováno
  - 5 změněných souborů, 96 řádků přidáno, 88 řádků odstraněno

#### Vylepšení úložného systému
- `57f9d5d` - Vylepšen úložný systém, přidána automatická kompakce a podpora neúplných dat
  - Nový automatický kompakční časovač SpeedyPackAutoCompactor (30minutový interval)
  - SpeedyPackRegistry singleton správce vylepšen
  - SpeedyStorage, SpeedyTimeStorage, SpeedyWorkNoteStorage adaptivní vylepšení
  - SpeedyPack přidána správa volného prostoru FreeList (149 řádků)
  - PackFileWriter writer refaktorován a optimalizován
  - WriteOperation, WriteQueue fronta zápisů vylepšena
  - SpeedyPackOptions konfigurační možnosti rozšířeny
  - IncompleteDate přidány metody porovnání
  - PluginLoader zavaděč pluginů vylepšen
  - Aktualizován inicializační tok Program.cs verzí Default a Fast
  - DefaultConfigData konfigurační data zjednodušena
  - KnowledgeNetwork znalostní síť zjednodušena
  - ChatController, MemoryController kontrolery optimalizovány
  - SpeedyPack.Manager MainForm funkce vylepšeny
  - 22 změněných souborů, 639 řádků přidáno, 253 řádků odstraněno

#### Aktualizace Speedy.Manager
- `b04ed33` - Aktualizovány soubory Speedy.Manager

### 2026-05-01

#### Architektonická refaktorizace: Speedy úložiště nahrazuje LiteDB
- `6600972` - Nahrazení LiteDB úložištěm Speedy, přidán systém pluginů a projekt Speedy
  - **Nový projekt SiliconLife.Speedy**：Výkonný .spk úložný engine
    - SpeedyPack jádrová třída (489 řádků): mapování adresářů v paměti + mezipaměť záznamů + asynchronní fronta zápisů
    - SpeedyPackOptions konfigurační třída: cache TTL, maximální počet položek mezipaměti, režim pouze pro čtení
    - IPackTransaction transakční rozhraní: podpora atomických operací zápisu
    - SpkFileInfo třída informací o souboru
    - Adresář Internal: DirectoryMap, EntryCache, PackFileReader, PackFileWriter, WriteQueue, WriteOperation, SpeedyTransaction, SpkHeader, PathNormalizer, FreeList
    - Závislost na MessagePack 3.1.4 pro binární serializaci (LZ4 komprese)
  - **Nový projekt SiliconLife.Speedy.Manager**：WPF nástroj pro správu
    - Architektura MVVM: MainViewModel, DirectoryTreeViewModel, ContentViewerViewModel atd.
    - Službová vrstva: PackService, FileDialogService, RecentFilesService, NotificationService
    - Konvertory: BoolToVisibility, ByteSizeToString, ContentTypeToIcon, NullToCollapsed
    - Zobrazení: MainWindow, DirectoryTreeView, ContentViewerPanel, MetadataPanel
    - Dialogy: FileInfoDialog, ImportDialog, NewEntryDialog
  - **Migrace úložiště SiliconLife.Fast**：LiteDB → SpeedyPack
    - Nový SpeedyStorage (IStorage adaptér)
    - Nový SpeedyTimeStorage (ITimeStorage adaptér)
    - Nový SpeedyWorkNoteStorage (IWorkNoteStorage adaptér)
    - Nový SpeedyPackRegistry (správa singleton na úrovni procesu)
    - Nový SpeedyPackAutoCompactor (automatický kompakční časovač)
    - Odstraněny implementace úložiště související s LiteDB (LiteDBStorage, LiteDBTimeStorage, LiteDBWorkNoteStorage, LiteDBLoggerProvider, LiteDBManager, LiteDBModels)
    - Odstraněn kód související s oknem správy LiteDB
  - **Systém zásuvných modulů**：
    - Nové rozhraní IPlugin (Core/Plugins/IPlugin.cs)
    - Nový zavaděč pluginů PluginLoader (Core/Plugins/PluginLoader.cs)
    - Podpora načítání plugin DLL z adresáře
    - Bezpečnostní sken: kontrola zakázaných jmenných prostorů (System.IO, System.Net, Microsoft.CodeAnalysis atd.)
    - Seznam důvěryhodných sestavení (Google.Protobuf, Newtonsoft.Json, MessagePack atd.)
    - Izolované načítání pomocí vlastního AssemblyLoadContext
    - ToolManager přidána metoda ScanAllPluginAssemblies
    - CoreHost integrován zavaděč pluginů
  - 119 změněných souborů, 6926 řádků přidáno, 3066 řádků odstraněno

#### Vylepšení křemíkových bytostí
- `3aef4c3` - Přidán aktivní stav Stopped a vylepšeno zpracování chyb
  - Křemíková bytost přidán stav Stopped
  - Vylepšeno zpracování chyb a mechanismus obnovy

#### Aktualizace lokalizace
- `513c65d` - Aktualizovány všechny jazykové verze a dokumentace
  - Nová komponenta MarkdownEditorComponent (625 řádků)
  - Nová komponenta DetailsComponent (130 řádků)
  - Nová komponenta AccordionComponent (285 řádků)
  - BeingController, ChatController, MemoryController, PermissionController kontrolery aktualizovány
  - BeingView, ChatView, MemoryView, SoulEditorView zobrazení refaktorována
  - Odstraněno staré MarkdownEditorView
  - InitController migrace na komponenty
  - 115 změněných souborů, 5761 řádků přidáno, 2362 řádků odstraněno

### 2026-04-30

#### Funkce systémového panelu
- `101b203` - Implementováno stavové okno systémového panelu a ApplicationContext
  - Nové zdroje ikon systémového panelu (alpha.png, noWord.png, slc.ico, wordIcon.png)
  - Implementováno TrayStatusWindow stavové okno
  - Podpora lokalizace systémového panelu pro 9 jazyků (TrayCsCZ, TrayDeDE, TrayEnUS atd.)
  - TrayLocalizationBase abstraktní základní třída
  - 24 změněných souborů, 27995 řádků přidáno, 1 řádek odstraněn (včetně zdrojových souborů)

#### Komponentová UI architektura
- `e61cfaa` - Dokončena komponentová UI architektura, implementováno 24 komponent
  - Fáze MVP (8): ComponentBase, Div, Span, Button, Input, Form, Select, Label
  - Druhá fáze (6): Accordion, Card, Tabs, Table, Modal, Message
  - Třetí fáze (5): Calendar, Tree, Chart, FileUpload, RichText
  - Přidány pomocné třídy Js, Behavior, DomUpdate atd.
  - 25 změněných souborů, 2666 řádků přidáno

- `7449e51` - Vylepšen systém komponent a přidány nové motivy vzhledu
  - Vylepšeny komponenty A, Button, Div, Form, Input atd.
  - Přidány 3 motivy vzhledu: HighContrast (vysoký kontrast), Light (světlý), Minimal (minimalistický)
  - Aktualizovány stávající motivy (Admin, Chat, Creative, Dev)
  - InitController migrace na komponenty
  - 32 změněných souborů, 1466 řádků přidáno, 1238 řádků odstraněno

- `1ba8636` - Zahájena migrace InitController na komponenty (v průběhu)
  - 9 změněných souborů, 574 řádků přidáno, 145 řádků odstraněno

#### Unifikace úložného systému
- `895dff9` - Unifikováno používání soul.md a state.json přes rozhraní IStorage
  - DefaultSiliconBeing používá IStorage pro čtení a zápis Souboru Duše a stavu
  - Nový StateFileManager správce stavových souborů
  - SoulFileManager refaktorován pro adaptaci na IStorage
  - 8 změněných souborů, 201 řádků přidáno, 116 řádků odstraněno

#### Vylepšení správy LiteDB
- `a34bef4` - Přidán LiteDBManager a vylepšena lokalizace systémového panelu
  - Systémový panel přidán vstup pro správu LiteDB
  - 9 jazyků lokalizace panelu aktualizováno
  - 10 změněných souborů, 196 řádků přidáno

- `c4a79ca` - Přidána jazykově vnímající tovární třída lokalizace pro okno správy LiteDB
  - 1 změněný soubor, 78 řádků přidáno

- `5ebc55e` - Převedena LiteDBAdminLocalization na abstraktní základní třídu
  - 10 změněných souborů, 1356 řádků přidáno

#### Oprava konfiguračního systému
- `2da5256` - Přidána abstraktní metoda ConfigExists a opraveny duplicitní konfigurační záznamy LiteDB
  - ConfigDataBase přidána metoda ConfigExists
  - Fast verze DefaultConfigData implementována kontrola existence konfigurace LiteDB
  - Opraven problém duplicitních konfiguračních klíčů LiteDB
  - 9 změněných souborů, 210 řádků přidáno, 2 řádky odstraněny

#### Optimalizace chatu a zobrazení
- `d3618ec` - Optimalizovány chatovací relace, úložný systém, časový model a základní třída zobrazení
  - BroadcastChannel, GroupChatSession, SingleChatSession optimalizovány
  - ITimeStorage přidány metody dotazů
  - FileSystemStorage a LiteDBStorage synchronně aktualizovány
  - ViewBase refaktorován a optimalizován (verze Default a Fast)
  - 11 změněných souborů, 622 řádků přidáno, 392 řádků odstraněno

### 2026-04-29

#### Architektonická refaktorizace: extrakce sdílených modulů
- `a102428` - Migrace sdílených modulů z SiliconLife.Default do SiliconLife.Common
  - Extrahováno 32 implementací kalendáře do projektu Common
  - Extrahovány základní třídy lokalizace a 21 jazykových implementací do projektu Common
  - Extrahován Správce Oprávnění a výchozí implementace křemíkové bytosti do projektu Common
  - Extrahováno 23 implementací vestavěných nástrojů do projektu Common
  - Extrahována implementace Playwright WebView do projektu Common
  - Aktualizovány jmenné prostory na SiliconLife.Collective
  - 122 změněných souborů, 586 řádků přidáno, 343 řádků odstraněno

#### Vylepšení kvality kódu
- `17566fe` - Nahrazeno Console.WriteLine systémem protokolování v projektech Core, Common a Default
  - Aktualizováno 6 souborů včetně ContextManager, AuditLogger, DefaultConfigData atd.
  - Unifikováno použití rozhraní ILogger, zlepšena udržovatelnost kódu
  - 6 změněných souborů, 12 řádků přidáno, 8 řádků odstraněno

#### SiliconLife.Fast vysoce výkonná verze
- `54a0307` - Přidán projekt SiliconLife.Fast a dokončeny opravy kompilace
  - Kompletní vstupní bod Windows Forms aplikace
  - Podpora systémového panelu (NotifyIcon)
  - Portováno všech 20+ webových UI kontrolerů
  - Portovány všechny komponenty webových zobrazení
  - Portovány 4 motivy vzhledu (Admin, Chat, Creative, Dev)
  - 125 změněných souborů, 61186 řádků přidáno

#### Vícejazyčná synchronizace dokumentace
- `265fde8` - Synchronizována dokumentace duální architektury do všech jazyků
  - Aktualizována architecture.md, changelog.md pro 7 jazyků
  - Aktualizována contributing.md pro 6 jazyků
  - Aktualizována getting-started.md, roadmap.md pro 7 jazyků
  - 47 změněných souborů, 1214 řádků přidáno, 38 řádků odstraněno

#### Úložný systém LiteDB (verze Fast)
- `4704862` - Přidány závislosti a infrastruktura LiteDB
  - Nová správní třída LiteDBManager
  - Nové datové modely LiteDBModels
  - 3 změněné soubory, 252 řádků přidáno

- `4220036` - Implementovány třídy úložiště LiteDB
  - LiteDBStorage: implementace rozhraní IStorage
  - LiteDBTimeStorage: implementace rozhraní ITimeStorage
  - LiteDBWorkNoteStorage: implementace rozhraní IWorkNoteStorage
  - 3 změněné soubory, 581 řádků přidáno

- `38ebd23` - Migrace konfiguračního a protokolovacího systému do LiteDB
  - DefaultConfigData adaptována na úložiště LiteDB
  - Nový poskytovatel protokolů LiteDBLoggerProvider
  - 2 změněné soubory, 203 řádky přidáno, 67 řádků odstraněno

- `e687157` - Migrace znalostní sítě ze souborového systému do LiteDB
  - KnowledgeNetwork kompletně refaktorována, používá LiteDB pro ukládání trojičkových dat
  - 1 změněný soubor, 231 řádků přidáno, 72 řádků odstraněno

- `4220169` - Integrace úložiště LiteDB do Program a ProjectManager
  - Program.cs inicializuje úložiště LiteDB
  - ProjectManager adaptován na úložiště pracovních poznámek LiteDB
  - 2 změněné soubory, 40 řádků přidáno, 17 řádků odstraněno

- `5f3a709` - Odstraněny zastaralé implementace souborového systému úložiště
  - Smazány FileSystemLoggerProvider, FileSystemStorage, FileSystemTimeStorage atd.
  - 6 změněných souborů, 1518 řádků odstraněno

- `e1a4ef2` - docs: přidán identifikátor verze v0.1.0-alpha do veškeré dokumentace
  - 127 změněných souborů, 2297 řádků přidáno, 2471 řádků odstraněno

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### Refaktoring úložného systému
- `8dd26e3` - Unifikováno použití ITimeStorage s IncompleteDate a přidána hierarchické dotazovací API
  - Odstraněny přetížené metody DateTime v rozhraní ITimeStorage, unifikováno použití IncompleteDate
  - IncompleteDate přidána metoda porovnání CompareTo(DateTime) a metoda Expand()
  - Přidány hierarchické dotazovací API GetEarliestTimestamp(), GetLatestTimestamp()
  - Přidány metody HasSummary() a QueryWithLevel() pro dotazování podle časové úrovně
  - Memory.cs refaktorován kompresní algoritmus, používá nové hierarchické dotazovací API pro zvýšení efektivity
  - FileSystemTimeStorage.cs kompletně implementovány nové metody rozhraní
  - Synchronně aktualizováni všichni volající: ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord atd.
  - Systém nástrojů aktualizován: HelpTool, LogTool, TokenAuditTool adaptovány na nové rozhraní
  - Webové kontrolery aktualizovány: AuditController, ChatController, ChatHistoryController adaptovány na nové rozhraní
  - 41 změněných souborů, 1820 řádků přidáno, 903 řádků odstraněno

### 2026-04-27

#### Vylepšení systému dokumentace nápovědy
- `9989d79` - Aktualizována lokalizace, systém nápovědy a webová zobrazení
  - Nové rozhraní IAIClientFactoryHelp.cs dokumentace nápovědy továrny AI klientů
  - Dokončen překlad veškeré dokumentace nápovědy do 9 jazyků
  - HelpTopics.cs přidáno 40 definic témat nápovědy
  - Komplexní aktualizace webových zobrazení: InitController, AuditView, ConfigView, KnowledgeView, LogView atd.
  - Vylepšen lokalizační systém: všechny jazykové verze přidány nové lokalizační klíče
  - Aktualizována továrna AI klientů: DashScopeClientFactory, OllamaClientFactory vylepšeny
  - 30 změněných souborů, 10086 řádků přidáno, 15 řádků odstraněno

#### Nový obsah dokumentace nápovědy
- `e7afe94` - Přidána dokumentace nápovědy pro Soubor Duše a auditní protokoly
  - Nová dokumentace nápovědy pro správu Souboru Duše
  - Nová dokumentace nápovědy pro auditní protokoly
  - HelpTopics.cs přidány definice témat
  - HelpView.cs výrazně refaktorováno, vylepšena logika vykreslování dokumentace
  - PermissionView.cs refaktorováno, vylepšeno rozhraní správy oprávnění
  - Vylepšeny jádrové moduly: SiliconBeingManager, TaskSystem, ToolManager
  - TaskTool.cs refaktorován, vylepšena funkce správy úkolů
  - Komplexní aktualizace webových zobrazení: všechny komponenty zobrazení synchronně aktualizovány
  - HelpController.cs zjednodušen, optimalizována logika kontroleru
  - 30 změněných souborů, 7100 řádků přidáno, 897 řádků odstraněno

### 2026-04-26

#### Systém dokumentace nápovědy
- `07895d7` - Vylepšen systém dokumentace nápovědy, přidány 3 dokumenty a dokončen překlad do 9 jazyků
  - Přidány průvodce systémem paměti, instalací a konfigurací Ollama a používáním platformy Alibaba Cloud Bailian
  - Dokončen překlad všech 10 dokumentů nápovědy do 9 jazyků
  - Zjednodušena logika vykreslování HelpView
  - 18 změněných souborů, 14418 řádků přidáno, 1364 řádků odstraněno

#### Německá lokalizace
- `0cfd8a1` - Přidána kompletní podpora německé lokalizace (de-DE)
  - Kompletní soubor německé lokalizace
  - Nová podpora německé lokalizace čínského historického kalendáře
  - Nový překlad dokumentace nápovědy do němčiny
  - Kompletní synchronizace veškeré dokumentace pro 9 jazyků
  - 135 změněných souborů, 26186 řádků přidáno, 14371 řádků odstraněno

#### Synchronizace dokumentace
- `3aada7d` - Synchronizována dokumentace v tradiční čínštině (zh-HK) se zjednodušenou čínštinou
  - 3 změněné soubory, 519 řádků přidáno, 422 řádků odstraněno
- `2f6abff` - Přidána lokalizace zobrazovaného názvu nástroje nápovědy pro všechny jazyky
  - 7 změněných souborů, 47 řádků přidáno, 7 řádků odstraněno

#### Refaktoring znalostního systému
- `60944fe` - Unifikovány jmenné prostory na SiliconLife.Collective
  - 8 změněných souborů, 5 řádků přidáno, 8 řádků odstraněno
- `69c51c5` - Přidán systém dokumentace nápovědy a překlad komentářů v kódu do angličtiny
  - 29 změněných souborů, 3385 řádků přidáno, 22 řádků odstraněno

### 2026-04-25

#### Automatizace prohlížeče WebView
- `41757c3` - Implementována multiplatformní automatizace prohlížeče WebView založená na Playwright
  - 6 změněných souborů, 1152 řádků přidáno

#### Aktualizace dokumentace
- `0ff797b` - Přidána dokumentace KnowledgeTool a WorkNoteTool (7 jazyků)
  - 28 změněných souborů, 4983 řádků přidáno
- `ad77415` - Aktualizovány všechny soubory changelog, přidány záznamy historie Git z 2026-04-25
  - 7 změněných souborů, 168 řádků přidáno

#### Správa projektového pracoviště
- `785c551` - Implementována správa projektového pracoviště včetně pracovních poznámek a úkolového systému
  - Nový systém správy projektového pracoviště
  - Funkce pracovních poznámek pro sledování postupu projektu
  - Integrace systému správy úkolů
  - 29 změněných souborů, 4256 řádků přidáno, 36 řádků odstraněno

#### Česká lokalizace
- `b4bbf39` - Přidána kompletní česká lokalizace (cs-CZ) a aktualizována dokumentace pro všechny jazyky
  - 116 změněných souborů, 4933 řádků přidáno, 222 řádků odstraněno
- `faf078f` - Opravena chyba kompilace české lokalizace
  - 3 změněné soubory, 910 řádků přidáno, 1 řádek odstraněn

#### Vylepšení znalostního systému
- `20adaac` - Přidán KnowledgeTool s podporou kompletní lokalizace
  - 34 změněných souborů, 2331 řádků přidáno, 56 řádků odstraněno

### 2026-04-24

#### Vylepšení systému správy paměti
- `c7b2ecc` - Vylepšeny funkce správy paměti, přidáno pokročilé filtrování, statistiky a zobrazení detailů
  - Nová funkce pokročilého filtrování paměti
  - Implementována funkce statistik paměti
  - Přidána stránka zobrazení detailů paměti
  - Podpora vícejazyčné lokalizace (6 jazyků)
  - 13 změněných souborů, 840 řádků přidáno, 86 řádků odstraněno

#### Rozšíření systému oprávnění
- `4489ad6` - Přidána služba počasí wttr.in do síťového seznamu povolených
  - Kompletní synchronní aktualizace vícejazyčné dokumentace (6 jazyků)
  - 14 změněných souborů, 417 řádků přidáno, 1 řádek odstraněn

#### Opravy webového rozhraní
- `d9d72e9` - Opraven problém priority CSS v modálním okně detailu pracovních poznámek
  - 19 změněných souborů, 1744 řádků přidáno, 6 řádků odstraněno

#### Optimalizace historie chatu
- `0df599c` - Opraven problém, kdy výsledky nástrojů byly vykreslovány jako nezávislé chatovací zprávy
  - 1 změněný soubor, 222 řádků přidáno, 21 řádků odstraněno
- `057b09d` - Optimalizováno zobrazení detailu historie chatu, vylepšeno vykreslování volání nástrojů
  - 3 změněné soubory, 389 řádků přidáno, 68 řádků odstraněno

#### Historie provádění časovačů
- `fa3f06f` - Přidána funkce historie provádění časovačů včetně zobrazení detailů
  - 8 změněných souborů, 937 řádků přidáno, 10 řádků odstraněno
- `d824835` - Přidány lokalizační klíče historie provádění časovačů (všechny jazyky)
  - 7 změněných souborů, 88 řádků přidáno

#### Vylepšení lokalizace
- `c13cb17` - Registrována španělská jazyková varianta
  - 1 změněný soubor, 4 řádky přidáno
- `9c44f34` - Přidána vícejazyčná lokalizační podpora pro čínský historický kalendář
  - 16 změněných souborů, 6049 řádků přidáno, 1 řádek odstraněn

#### Vylepšení jádrových funkcí
- `1e7c7b2` - Vylepšena komprese paměti a sledování provádění nástrojů
  - 4 změněné soubory, 338 řádků přidáno, 86 řádků odstraněno

### 2026-04-23

#### Lokalizace nástrojů
- `192fc6e` - Přidány chybějící lokalizace názvů nástrojů pro 5 nástrojů
  - 6 změněných souborů, 30 řádků přidáno

#### Aktualizace dokumentace
- `882c08f` - Aktualizovány všechny soubory changelog, přidány kompletní záznamy historie Git a odstraněny falešné verze
  - 45 změněných souborů, 8815 řádků přidáno, 1611 řádků odstraněno

#### Vylepšení stránky chatu
- `65c157b` - Přidán indikátor načítání na stránku chatu a automatický výběr relace Kurátora
  - 10 změněných souborů, 211 řádků přidáno, 7 řádků odstraněno

#### Funkce historie chatu
- `e483348` - Implementována funkce zobrazení historie chatu křemíkových bytostí
  - Nový ChatHistoryController
  - Vytvořen ChatHistoryViewModel
  - Implementovány stránky ChatHistoryListView a ChatHistoryDetailView
  - Přidány lokalizační klíče historie chatu (5 jazyků)
  - 12 změněných souborů, 1178 řádků přidáno

#### Vylepšení řízení AI streamu
- `30a2d4e` - Vylepšeno zrušení AI streamu, integrace IM a inicializace hlavního uzlu
  - 11 změněných souborů, 387 řádků přidáno, 12 řádků odstraněno

#### Fronta chatovacích zpráv
- `db48c51` - Přidána fronta chatovacích zpráv, metadata souborů a podpora zrušení streamu
  - 4 změněné soubory, 357 řádků přidáno

#### Podpora nahrávání souborů
- `28fb344` - Implementován dialog zdroje souborů a podpora nahrávání souborů
  - 3 změněné soubory, 1100 řádků přidáno, 2 řádky odstraněny
- `1d3e2cc` - Přidány lokalizační řetězce dialogu zdroje souborů (6 jazyků)
  - 6 změněných souborů, 30 řádků přidáno

#### Aktualizace dokumentace
- `8111e92` - Přidán odkaz Wiki do sekce repozitáře v README
  - 1 změněný soubor, 3 řádky přidáno, 1 řádek odstraněn

### 2026-04-22

#### Lokalizace dokumentace
- `66c11eb` - Překlad čínských komentářů do angličtiny a aktualizace všech changelogů
  - 11 změněných souborů, 373 řádků přidáno, 163 řádků odstraněno

#### Vylepšení SSE zpráv
- `b574b2b` - Přidáno senderName pro historické zprávy pro identifikaci AI
  - 1 změněný soubor, 9 řádků přidáno

#### Funkce chatu
- `601fc14` - Přidána operace mark_read pro označení konce relace
  - 7 změněných souborů, 196 řádků přidáno, 36 řádků odstraněno

#### Optimalizace systému nástrojů
- `7a03a19` - Vylepšena flexibilita dotazování LogTool
  - 1 změněný soubor, 57 řádků přidáno, 24 řádků odstraněno

#### Vylepšení lokalizace
- `0a8d750` - Přidán obecný systémový prompt pro proaktivní chování křemíkových bytostí
  - 8 změněných souborů, 460 řádků přidáno, 48 řádků odstraněno

#### Refaktoring protokolovacího systému
- `2b771f3` - Decoupling LogController od souborového I/O, přidáno API pro čtení protokolů
  - 4 změněné soubory, 172 řádků přidáno, 137 řádků odstraněno
- `12da302` - Přidán filtr křemíkových bytostí do zobrazení protokolů
  - 9 změněných souborů, 147 řádků přidáno, 10 řádků odstraněno
- `8f6cb1e` - Přidán parametr beingId do rozhraní ILogger, implementováno oddělení systémových protokolů od protokolů křemíkových bytostí
  - 47 změněných souborů, 524 řádků přidáno, 490 řádků odstraněno

#### Vylepšení systému oprávnění
- `4c747ad` - Refaktorovány PermissionTool, ExecuteCodeTool, přidáno API EvaluatePermission
  - 18 změněných souborů, 680 řádků přidáno, 492 řádků odstraněno

#### Opravy bugů
- `1c96e99` - Opraveno selhání vyhledávání search_files a search_content v kořenovém adresáři
  - 1 změněný soubor, 98 řádků přidáno, 41 řádků odstraněno

#### Integrace nástrojů
- `135710d` - Odstraněn SearchTool, místní vyhledávání přesunuto do DiskTool
  - 2 změněné soubory, 185 řádků přidáno, 365 řádků odstraněno

#### Rozšíření systému nástrojů
- `70ce7fb` - Implementován DatabaseTool pro strukturované databázové dotazy
  - 1 změněný soubor, 382 řádků přidáno
- `be29a09` - Implementován LogTool pro dotazy na historii operací a konverzací
  - 1 změněný soubor, 298 řádků přidáno
- `4ea7702` - Implementován PermissionTool pro dynamickou správu oprávnění
  - 1 změněný soubor, 457 řádků přidáno
- `1384ff4` - Implementován ExecuteCodeTool pro vícejazyčné spouštění kódu
  - 1 změněný soubor, 477 řádků přidáno
- `82d1e11` - Implementován SearchTool pro vyhledávání informací
  - 1 změněný soubor, 363 řádků přidáno

#### Optimalizace webového rozhraní
- `0675c45` - Optimalizováno zvýraznění bloků kódu markdown v podokně náhledu
  - 1 změněný soubor, 4 řádky přidáno, 23 řádků odstraněno
- `702b3f3` - Vylepšeno zobrazení úkolů, přidány stavové odznaky a zobrazení metadat
  - 8 změněných souborů, 221 řádků přidáno, 9 řádků odstraněno
- `6ed9a79` - Vylepšeno ukládání chatovacích zpráv a vykreslování zobrazení
  - 8 změněných souborů, 140 řádků přidáno, 29 řádků odstraněno

### 2026-04-21

#### Opravy bugů
- `c6b518b` - Opraveno předávání zpráv časovačů a ukládání chatovacích zpráv
  - 3 změněné soubory, 297 řádků přidáno, 124 řádků odstraněno

#### Správa konfigurace
- `4305769` - Přidán .gitattributes pro správu konců řádků
  - 1 změněný soubor, 32 řádků přidáno

#### Vylepšení webového rozhraní
- `188c6f8` - Registrována API routa seznamu úkolů a přidáno zobrazení prázdného stavu
  - 2 změněné soubory, 35 řádků přidáno, 2 řádky odstraněny
- `634e8ca` - Přidán odkaz zpět na seznam na stránce oprávnění
  - 1 změněný soubor, 16 řádků přidáno
- `6ba591d` - Přidán samostatný editor konfigurace AI pro křemíkové bytosti
  - 11 změněných souborů, 842 řádků přidáno, 18 řádků odstraněno
- `0a826f5` - Přidáno oznámení o úspěšném uložení v editoru kódu
  - 1 změněný soubor, 9 řádků přidáno, 2 řádky odstraněny
- `2940373` - Vylepšeno webové rozhraní, přidány tooltipy pro kód a vylepšení UI
  - 11 změněných souborů, 1054 řádků přidáno, 75 řádků odstraněno

#### Oprava systému oprávnění
- `592c7ab` - Opravena instance zpětného volání a pořadí registrace
  - 2 změněné soubory, 38 řádků přidáno, 7 řádků odstraněno

#### Vylepšení zabezpečení
- `833ead2` - Přidáno ověření odkazů na sestavení pro dynamickou kompilaci
  - 4 změněné soubory, 135 řádků přidáno, 8 řádků odstraněno

#### Vylepšení systému oprávnění
- `5879621` - Přidána předkompilační validace zpětného volání oprávnění a vylepšeno zpracování chyb
  - 21 změněných souborů, 617 řádků přidáno, 26 řádků odstraněno

#### Aktualizace dokumentace
- `4dbf659` - Aktualizován changelog na v0.5.1, nahrazeny zástupné URL GitHub, přidáno zrcadlo Gitee, lokalizován název Bilibili podle jazyka, aktualizován email
  - 32 změněných souborů, 489 řádků přidáno, 180 řádků odstraněno

#### Konfigurace a vstupní bod
- `0fc1693` - Aktualizován vstupní bod programu a konfigurace projektu
  - 2 změněné soubory, 7 řádků přidáno

#### Refaktoring systému oprávnění
- `ea9179a` - Vylepšena implementace systému oprávnění
  - 5 změněných souborů, 358 řádků přidáno, 152 řádků odstraněno

#### Opravy bugů
- `928a96d` - Opravena implementace výpočtu kalendáře
  - 4 změněné soubory, 12 řádků přidáno, 12 řádků odstraněno

#### AI a kalendář
- `646813e` - Vylepšena implementace továrny AI klientů
  - 2 změněné soubory, 21 řádků přidáno, 20 řádků odstraněno

#### Lokalizace
- `7940d9c` - Přidána podpora korejské lokalizace
  - 7 změněných souborů, 2424 řádků přidáno, 10 řádků odstraněno
- `4ff98ad` - Refaktorována dokumentace, podpora více jazyků
  - 81 změněných souborů, 23818 řádků přidáno, 1886 řádků odstraněno

### 2026-04-20

#### Dokončení jádrových funkcí
- `28905b5` - Kompletní vícejazyčná podpora, továrna AI klientů, systém oprávnění a lokalizační nastavení
  - Protokolovací systém se správcem, záznamy a různými úrovněmi protokolů
  - Auditní systém tokenů pro dotazování a sledování využití tokenů
  - Továrna AI klientů s automatickým objevováním různých AI platforem
  - Systém zpětného volání oprávnění s vlastním úložištěm
  - Implementace konzolového loggeru
  - Vícejazyčná podpora pro angličtinu a zjednodušenou čínštinu
  - WebUI messenger s WebSocket pro chat v reálném čase
  - Vylepšení výchozí křemíkové bytosti pomocí lokalizace
  - 39 změněných souborů, 4670 řádků přidáno, 175 řádků odstraněno

### 2026-04-19

#### Časovače a kalendář
- `c933fd8` - Aktualizována lokalizace, systém časovačů, webová zobrazení a přidány nástroje
  - Lepší správce lokalizace
  - Plánovací systém pro časované úlohy
  - Konfigurace AI a správa kontextu
  - Kalendářový nástroj podporující 32 typů kalendářů
  - Webový kontroler pro kalendářní API
  - Nástroj pro správu úkolů
  - 46 změněných souborů, 4018 řádků přidáno, 975 řádků odstraněno

**Architektonická vylepšení**
- Předesignována architektura webových zobrazení pro lepší podporu motivů vzhledu
- Vylepšen systém správy bytostí s lepším zpracováním stavů

### 2026-04-18

- `9f585e1` - Aktualizována lokalizace, systém časovačů, webová zobrazení a přidány nástroje
  - Vylepšení časovačů a plánování
  - Lepší webová zobrazení s vylepšenými UI komponentami
  - Více implementací nástrojů
  - 57 změněných souborů, 3328 řádků přidáno, 389 řádků odstraněno

### 2026-04-17

- `9b71fcd` - Aktualizovány jádrové moduly, přidána dokumentace zh-HK, vysílací kanál, konfigurační nástroj a auditní webové zobrazení
  - Vysílací kanál pro společný chat více křemíkových bytostí
  - Konfigurační nástroj
  - Auditní webové zobrazení
  - Dokumentace v tradiční čínštině
  - 42 změněných souborů, 3533 řádků přidáno, 268 řádků odstraněno

### 2026-04-16

- `5040f05` - Aktualizovány jádrové a výchozí moduly
  - Optimalizace modulů a opravy bugů
  - Aktualizace implementací a vylepšení
  - 58 změněných souborů, 9916 řádků přidáno, 111 řádků odstraněno

### 2026-04-15

- `3efab5f` - Aktualizováno více modulů: AI, Chat, IM, Nástroje, Web, Lokalizace, Úložiště
  - Vylepšení AI klienta
  - Vylepšení chatovacího systému
  - Aktualizace poskytovatele messengeru
  - Optimalizace systému nástrojů
  - Vylepšení webové infrastruktury
  - Optimalizace lokalizace
  - Aktualizace úložného systému
  - 33 změněných souborů, 788 řádků přidáno, 232 řádků odstraněno

### 2026-04-14

- `4241a2f` - Základní funkce chatu dokončeny, optimalizace nahrávání UI
  - Dokončeny funkce chatovacího systému
  - Optimalizace UI pro nahrávání souborů
  - 16 změněných souborů, 1234 řádků přidáno, 102 řádky odstraněny

### 2026-04-13

- `c498c31` - Aktualizace kódu
  - Obecná vylepšení a optimalizace kódu
  - 32 změněných souborů, 1045 řádků přidáno, 546 řádků odstraněno

### 2026-04-12

#### Dokumentace a lokalizace
- `2161002` - Refaktorována dokumentace a vylepšena lokalizace
  - 17 změněných souborů, 982 řádků přidáno, 92 řádků odstraněno
- `03d94e4` - Vylepšen konfigurační systém a lokalizace
  - 25 změněných souborů, 1378 řádků přidáno, 154 řádků odstraněno
- `9976a35` - Přidána stránka O aplikaci a lokalizace
  - 14 změněných souborů, 699 řádků přidáno, 44 řádků odstraněno

#### Chat a webová zobrazení
- `0c8ccfc` - Vylepšen chatovací systém, lokalizace a webová zobrazení
  - 13 změněných souborů, 402 řádků přidáno, 56 řádků odstraněno
- `a8f1342` - Předesignována webová komunikační vrstva, přechod z WebSocket na SSE
  - 27 změněných souborů, 793 řádků přidáno, 935 řádků odstraněno

### 2026-04-11

#### Protokolovací systém
- `e8fe259` - Přidán protokolovací systém a optimalizace kódu
  - 37 změněných souborů, 624 řádků přidáno, 91 řádků odstraněno
- `f01c519` - Přidán protokolovací systém, aktualizována rozhraní AI a webová zobrazení
  - 31 změněných souborů, 1758 řádků přidáno, 63 řádků odstraněno

### 2026-04-10

- `4962924` - Vylepšen WebSocket handler, zobrazení chatu a interakce messengeru
  - Vylepšení správce kontextu
  - Vylepšení chatovacího systému
  - Aktualizace rozhraní poskytovatele messengeru
  - Předesignován WebUI poskytovatel
  - Aktualizován JavaScript builder a router
  - Optimalizováno zobrazení chatu
  - Vylepšen WebSocket handler
  - 9 změněných souborů, 365 řádků přidáno, 134 řádků odstraněno

### 2026-04-09

- `f9302bf` - Vylepšeno rozhraní poskytovatele messengeru, chatovací systém a interakce Web UI
  - Rozšířeno rozhraní poskytovatele messengeru
  - Vylepšení chatovacích zpráv a systému
  - Optimalizace správce kontextu
  - Vylepšení výchozí křemíkové bytosti
  - Vylepšení webového zobrazení chatu
  - Aktualizován WebSocket handler
  - 10 změněných souborů, 427 řádků přidáno, 93 řádků odstraněno

### 2026-04-07

- `6831ee8` - Předesignována webová zobrazení a JavaScript builder
  - Kompletní předesign webových kontrolerů
  - Kompletní přepis JavaScript builderu
  - Aktualizovány všechny komponenty zobrazení
  - Vylepšen systém motivů vzhledu
  - Vylepšena architektura základní třídy zobrazení
  - 23 změněných souborů, 2004 řádků přidáno, 1983 řádků odstraněno

### 2026-04-05

- `41e97fb` - Aktualizováno více jádrových modulů a webových kontrolerů
  - Vylepšení správce kontextu
  - Chatovací systém a správa relací
  - Předesignován service locator
  - Aktualizována základní třída a správce křemíkových bytostí
  - Komplexní aktualizace webových kontrolerů (17 kontrolerů)
  - Vylepšena továrna výchozích křemíkových bytostí
  - 31 změněných souborů, 681 řádků přidáno, 326 řádků odstraněno
- `67988d4` - Vylepšen modul Web UI, přidáno zobrazení exekutorů, vyčištěny zobrazení a jádrové moduly
  - 61 změněných souborů, 3148 řádků přidáno, 3726 řádků odstraněno

### 2026-04-04

- `b58bb1c` - Přidán inicializační kontroler a předesignován webový modul
  - Inicializační kontroler
  - Předesign konfiguračního modulu
  - Aktualizace lokalizačního modulu
  - Vylepšení systému motivů vzhledu
  - Vylepšení routeru
  - 29 změněných souborů, 1269 řádků přidáno, 289 řádků odstraněno
- `f03ac0b` - Přidán modul Web UI, vylepšena funkce messengeru
  - 60 změněných souborů, 8481 řádků přidáno, 165 řádků odstraněno

### 2026-04-03

- `192e57b` - Aktualizována struktura projektu a jádrové běhové komponenty
  - 22 změněných souborů, 446 řádků přidáno, 179 řádků odstraněno
- `59faec8` - Aktualizace jádra a výchozí implementace
  - 25 změněných souborů, 3056 řádků přidáno, 18 řádků odstraněno
- `d488485` - Přidána funkce dynamické kompilace a modul nástroje Kurátora
  - 19 změněných souborů, 1727 řádků přidáno, 11 řádků odstraněno
- `753d1d9` - Přidán bezpečnostní modul, aktualizováni exekutoři, poskytovatel messengeru, lokalizace a nástroje
  - 29 změněných souborů, 2352 řádků přidáno, 93 řádků odstraněno
- `a378697` - Dokončena fáze 5 - systém nástrojů + exekutoři
  - 41 změněných souborů, 2651 řádků přidáno, 363 řádků odstraněno

### 2026-04-02

- `e6ad94b` - Opraveno selhání načítání historie chatu při odstranění konfiguračního souboru během testování
  - 4 změněné soubory, 49 řádků přidáno, 45 řádků odstraněno
- `daa56f5` - Dokončena fáze 4: perzistentní paměť (chatovací systém + kanál messengeru)
  - 29 změněných souborů, 2051 řádků přidáno, 538 řádků odstraněno

### 2026-04-01

- `bbe2dbb` - Opraveno načítání konfigurace a směrování servisních zpráv chatu
  - 27 změněných souborů, 1633 řádků přidáno, 147 řádků odstraněno
- `2fa6305` - Implementována fáze 2: rámec Hlavní Smyčky a systém Tick Objektů
  - 9 změněných souborů, 594 řádků přidáno, 41 řádků odstraněno
- `32b99a1` - Implementována fáze 1 - základní funkce chatu
  - 19 změněných souborů, 1185 řádků přidáno
- `358e368` - Počáteční commit: projektová dokumentace a licence
  - 10 změněných souborů, 1873 řádků přidáno
