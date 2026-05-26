![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**Verze: v0.2.0-alpha** | **Silicon Life Collective** — platforma pro multiagentní spolupráci založená na .NET 9, kde jsou AI agenti nazýváni **Křemíkové Bytosti**, a pomocí dynamické kompilace Roslyn dosahují sebeevoluce.

[English](../README.md) | [Deutsch](../de-DE/README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Français](../fr-FR/README.md) | **Čeština** | [Italiano](../it-IT/README.md) | [Polski](../pl-PL/README.md) | [Português](../pt-PT/README.md) | [Русский](../ru-RU/README.md)

## 🌟 Klíčové vlastnosti

### Systém agentů
- **Orchestrace více agentů** — jednotně spravována *Kurátorem Křemíku*, využívá hodinami řízený mechanismus spravedlivého plánování časových slotů
- **Řízeno Souborem Duše** — každou Křemíkovou Bytost pohání základní soubor s výzvami (`soul.md`), který definuje unikátní osobnost a vzorce chování
- **Architektura Tělo-Mozek** — *Tělo* (SiliconBeing) udržuje životní funkce a detekuje spouštěcí scénáře; *Mozek* (Správce Kontextu) načítá historii, volá AI, provádí nástroje a perzistuje odpovědi
- **Schopnost sebeevoluce** — pomocí technologie dynamické kompilace Roslyn mohou Křemíkové Bytosti přepisovat vlastní kód a dosáhnout evoluce
- **Správa aktivních stavů** — podporuje čtyři aktivní stavy: Idle (nečinný), Working (pracující), Error (chyba), Stopped (zastavený), po 10 po sobě jdoucích chybách automaticky přejde do stavu Stopped

### Systém zásuvných modulů
- **Architektura rozšíření zásuvnými moduly** — rozšíření funkcí prostřednictvím rozhraní IPlugin, podpora dynamického načítání DLL zásuvných modulů z adresáře
- **Bezpečnostní Sandbox** — zavaděč zásuvných modulů provádí přísné bezpečnostní skenování, zakazuje přístup k jmenným prostorům System.IO, System.Net atd.
- **Izolované načítání** — používá vlastní AssemblyLoadContext pro izolované načítání, zabraňuje vlivu zásuvných modulů na stabilitu hlavního programu
- **Integrace nástrojů** — zásuvné moduly mohou registrovat vlastní nástroje prostřednictvím rozhraní ITool, automaticky se integrují do smyčky volání nástrojů

### Nástroje a exekuce
- **24 vestavěných nástrojů** — pokrývající kalendář, chat, konfiguraci, disk, síť, paměť, úkoly, časovače, znalostní bázi, pracovní poznámky, projektový pracovní prostor, WebView prohlížeč, hot-reload atd.
- **Izolace scénářů nástrojů** — každý nástroj deklaruje dostupné scénáře pomocí vlastnosti `ToolScenario` (Chat, Task, Timer, MemoryCompression, Project), vlastnost `ChatOnly` omezuje nástroj pouze na scénář chatu
- **Nástroj hot-reload** — podporuje automatickou kompilaci, aktualizaci souborů a restart SiliconLife.Fast za běhu, bez nutnosti ručního zásahu
- **Smyčka volání nástrojů** — AI vrací volání nástroje → provede nástroj → výsledek je předán AI → smyčka pokračuje, dokud není vrácena čistě textová odpověď
- **Exekutor-oprávnění zabezpečení** — všechny I/O operace procházejí exekutorem s přísným ověřováním oprávnění
  - 3-úrovňový řetězec ověření oprávnění: Uživatelská Frekvenční Mezipaměť → Rozhraní Zpětného Volání Oprávnění → (IsCurator: Zpracovatel Dotazů na Oprávnění | Non-curator: Globální ACL → výchozí zamítnutí)
  - Kompletní auditní protokol zaznamenává všechna rozhodnutí o oprávněních

### AI a znalosti
- **Podpora více AI backendů**
  - **Ollama** — lokální nasazení modelů, používá nativní HTTP API
  - **Alibaba Cloud Bailian (DashScope)** — cloudová AI služba, kompatibilní s OpenAI API, podporuje 13+ modelů, nasazení ve více regionech
  - **Volcengine Ark** — cloudová AI služba ByteDance, podporuje streamovací a nestreamovací režim, vestavěné řízení rychlosti
- **32 kalendářních systémů** — úplné pokrytí hlavních světových kalendářů, včetně gregoriánského, čínského lunárního, islámského, hebrejského, japonského, perského, mayského, čínských historických kalendářů atd.
- **Znalostní síť** — znalostní graf založený na trojicích (subjekt-relace-objekt), podporuje ukládání, dotazování a objevování cest
- **Projektový pracovní prostor** — správa projektového prostoru, podpora vytváření/archivace/ničení projektů, přiřazování rolí, pracovní poznámky, sledování úkolů a izolace oprávnění nástrojů
- **Engine pracovních postupů** — stavový stroj založený na šablonách, podporuje vlastní šablony pracovních postupů, přechody stavů, Tick-řízenou exekuci a správu životního cyklu instancí
- **Mechanismus zapomínání paměti** — služba časového útlumu (MemoryFadeService), každou hodinu automaticky aplikuje útlum důležitosti a automatickou archivaci paměti všech Křemíkových Bytostí

### Webové rozhraní
- **Moderní Web UI** — vestavěný HTTP server, podpora SSE pro aktualizace v reálném čase
- **7 skinových témat** — administrativní, chatovací, kreativní, vývojářské, vysoký kontrast, světlé, minimalistické, podpora automatického objevování a přepínání
- **24 kontrolerů** — kompletní správa systému, chat, konfigurace, monitorovací funkce
- **Nulová závislost na frontendovém frameworku** — generování HTML/CSS/JS na straně serveru pomocí `H`, `CssBuilder` a `JsBuilder`

### Internacionalizace a lokalizace
- **34 jazykových variant** s plnou podporou, pokrývajících 2 písemné systémy a více regionálních variant
  - **Zjednodušená čínština**: zh-CN (pevninská Čína), zh-SG (Singapur), zh-MY (Malajsie) (3 varianty)
  - **Tradiční čínština**: zh-HK (Hongkong), zh-TW (Tchaj-wan), zh-MO (Macao) (3 varianty)
  - **Angličtina**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 variant)
  - **Španělština**: es-ES, es-MX (2 varianty)
  - **Němčina**: de-DE, de-AT, de-CH, de-LU, de-LI (5 variant)
  - **Francouzština**: fr-FR, fr-CA, fr-CH (3 varianty)
  - **Japonština**: ja-JP | **Korejština**: ko-KR | **Čeština**: cs-CZ (3 varianty)
  - **Italština**: it-IT | **Polština**: pl-PL | **Portugalština**: pt-PT, pt-BR (4 varianty)

### Data a úložiště
- **SpeedyPack vysoce výkonné úložiště** — verze Fast používá vlastní úložný engine .spk, mapování adresářů v paměti + mezipaměť záznamů + asynchronní fronta zápisů
- **Souborový systém úložiště** — verze Default používá čistě souborový systém JSON úložiště
- **Časově indexované dotazy** — prostřednictvím rozhraní `ITimeStorage` podporuje efektivní dotazy podle časového rozsahu
- **Automatická komprimace** — SpeedyPack podporuje pravidelnou automatickou komprimaci, recyklace volného prostoru
- **Minimální závislosti** — jádro knihovny závisí pouze na Microsoft.CodeAnalysis.CSharp pro dynamickou kompilaci

## 🔄 Duální architektura verzí

Tento projekt nabízí dvě implementační verze pro různé scénáře:

### SiliconLife.Default (výchozí verze)
- **Zaměření**: výchozí implementace, primárně pro ověření proveditelnosti architektury
- **Režim běhu**: konzolová aplikace
- **Způsob úložiště**: čistě souborový systém JSON úložiště
- **Vhodné scénáře**: vysoké požadavky na bezpečnost dat, omezené paměťové prostředky, malé objemy dat
- **Vlastnosti**: jednoduché a spolehlivé, okamžitá perzistence dat, žádné riziko ztráty dat v paměti
- **Popis role**: jako referenční implementace pro ověření architektury, vhodná pro první seznámení, vývoj a ladění nebo scénáře s prioritou bezpečnosti dat
- **Spouštěcí příkaz**: `dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast (vysoce výkonná verze)
- **Zaměření**: hlavní produkční verze
- **Režim běhu**: desktopová aplikace (systémová lišta Windows/macOS / stavové okno Linux)
- **Způsob úložiště**: SpeedyPack paměťové úložiště + asynchronní dávková perzistence (formát souboru .spk)
- **Vhodné scénáře**: vysoká souběžnost, nízká latence, velké objemy dat
- **Podpora platforem**: Windows/macOS (kompletní funkce včetně systémové lišty), Linux (stavové okno, bez ikony v liště)
- **Vlastnosti**:
  - Extrémní optimalizace výkonu
  - Windows/macOS běh na pozadí v systémové liště, podpora stavového okna pro sledování v reálném čase; Linux zobrazuje stavové okno přímo
  - SpeedyPack engine + automatická komprimace zaručuje bezpečnost dat
  - Component UI architektura, 27 deklarativních komponent
  - 7 skinových témat, podpora automatického objevování a přepínání
  - Nástroj hot-reload podporuje online aktualizace a restart
- **Zvýšení výkonu**: latence čtení úložiště snížena 1000x, latence zápisu snížena 15000x, kapacita souběžného zpracování zvýšena 50x
- **Popis role**: hluboce optimalizovaná produkční implementace, první volba pro dlouhodobý provoz a skutečné produkční prostředí
- **Spouštěcí příkaz**: `dotnet run --project src/SiliconLife.Fast`

### Srovnání verzí

| Vlastnost | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| **Režim běhu** | Konzolová aplikace | Desktopová aplikace (systémová lišta Windows/macOS / stavové okno Linux) |
| **Uživatelské rozhraní** | Web UI (přístup přes prohlížeč) | Windows/macOS: ikona v liště + okno lišty + Web UI; Linux: stavové okno + Web UI |
| **Systémová lišta** | ❌ Ne | ✅ Windows/macOS podporuje minimalizaci do lišty; Linux bez ikony v liště |
| **Běh na pozadí** | ❌ Zavření konzole ukončí aplikaci | ✅ Windows/macOS běží na pozadí v liště; Linux běží ve stavovém okně |
| **Způsob úložiště** | Souborový systém JSON úložiště | SpeedyPack paměťové úložiště + asynchronní perzistence |
| **Úložný engine** | Souborový systém I/O | SiliconLife.Speedy (formát .spk) |
| **Latence čtení** | ~10ms (disk I/O) | ~0.01ms (operace v paměti) |
| **Latence zápisu** | ~15ms (synchronní zápis) | ~0.001ms (asynchronní zápis) |
| **Souběžnost** | ~100 req/s | ~5000 req/s |
| **Spotřeba paměti** | ~200MB | ~500MB |
| **Bezpečnost dat** | Velmi vysoká (okamžitá perzistence) | Vysoká (asynchronní perzistence + automatická komprimace) |
| **Vhodné scénáře** | Priorita bezpečnosti dat, malé objemy dat | Priorita výkonu, velké objemy dat, vysoká souběžnost |

## 🛠️ Technologický stack

| Komponenta | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| Runtime | .NET 9 | .NET 9 (Windows/macOS/Linux) |
| Programovací jazyk | C# | C# |
| Typ aplikace | Konzolová aplikace | Desktopová aplikace (systémová lišta Windows/macOS / stavové okno Linux) |
| AI integrace | Ollama (lokální), Alibaba Cloud Bailian (cloud), Volcengine Ark (cloud) | Ollama (lokální), Alibaba Cloud Bailian (cloud), Volcengine Ark (cloud) |
| Datové úložiště | Souborový systém (JSON + časově indexované adresáře) | SpeedyPack (formát .spk, mapování v paměti + asynchronní perzistence) |
| Webový server | HttpListener (vestavěný v .NET) | HttpListener (vestavěný v .NET) |
| Dynamická kompilace | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Automatizace prohlížeče | Playwright (WebView) | Playwright (WebView) |
| Systém zásuvných modulů | ✅ Podpora (IPlugin + PluginLoader) | ✅ Podpora (IPlugin + PluginLoader) |
| Systémová lišta | ❌ Nepodporováno | ✅ Windows/macOS podporováno (NotifyIcon); Linux bez ikony v liště |
| Licence | Apache-2.0 | Apache-2.0 |

## 📁 Struktura projektu

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Jádrová knihovna (rozhraní, abstraktní třídy)
│   │   ├── AI/                            # Rozhraní AI klienta, Správce Kontextu, modely zpráv
│   │   ├── Audit/                         # Systém auditu využití Tokenů
│   │   ├── Chat/                          # Chatovací Systém, správa relací, Vysílací Kanál
│   │   ├── Compilation/                   # Dynamická kompilace, bezpečnostní skenování, šifrování kódu
│   │   ├── Config/                        # Konfigurační Systém
│   │   ├── Executors/                     # Exekutoři (disk, síť, příkazový řádek)
│   │   ├── IM/                            # Rozhraní Poskytovatele IM
│   │   ├── Knowledge/                     # Znalostní Síť
│   │   ├── Localization/                  # Lokalizační Systém
│   │   ├── Logging/                       # Protokolovací Systém
│   │   ├── Plugins/                       # Systém zásuvných modulů (rozhraní IPlugin, zavaděč PluginLoader)
│   │   ├── Project/                       # Projektový Systém
│   │   ├── Runtime/                       # Hlavní Smyčka, Tick Objekty, Hlavní Uzel
│   │   ├── Security/                      # Systém Správy Oprávnění
│   │   ├── SiliconBeing/                  # Základní třída Křemíkových Bytostí, Správce, továrna
│   │   ├── Storage/                       # Rozhraní úložiště
│   │   ├── Time/                          # Neúplné Datum (dotazy na časové rozsahy)
│   │   ├── Tools/                         # Rozhraní nástrojů a Správce Nástrojů
│   │   ├── WebView/                       # Rozhraní WebView prohlížeče
│   │   ├── Workflow/                      # Engine pracovních postupů (šablony, instance, přechody stavů)
│   │   └── ServiceLocator.cs              # Globální lokátor služeb
│   │
│   ├── SiliconLife.Common/                # Sdílená implementace (společná pro obě verze)
│   │   ├── AI/                            # AI klienti a továrny (Ollama, DashScope, VolcengineArk)
│   │   ├── Calendar/                      # 32 implementací kalendáře
│   │   ├── Localization/                  # Základ lokalizace a 34 jazykových/regionálních variant
│   │   ├── Resources/                     # Sdílené zdrojové soubory
│   │   ├── Security/                      # Správce Oprávnění
│   │   ├── SiliconBeing/                  # Výchozí implementace Křemíkové Bytosti
│   │   ├── Tools/                         # 23 implementací obecných nástrojů
│   │   ├── Web/                           # Webová infrastruktura
│   │   └── WebView/                       # Implementace Playwright WebView
│   │
│   ├── SiliconLife.App/                   # Aplikační vrstva (Web UI + nápověda, sdílená mezi Default a Fast)
│   │   ├── Config/                        # Konfigurace aplikace
│   │   ├── Data/                          # Datový adresář
│   │   ├── Help/                          # Lokalizace nápovědy (vícejazyčná)
│   │   ├── Tools/                         # HelpTool (nástroj pro dotazy na nápovědu)
│   │   └── Web/                           # Implementace Web UI
│   │       ├── Component/                 # Knihovna UI komponent (27 komponent)
│   │       ├── Controllers/               # 24 kontrolerů
│   │       ├── Models/                    # Modely pohledů
│   │       ├── Views/                     # HTML pohledy
│   │       └── Skins/                     # 7 skinových témat
│   │
│   ├── SiliconLife.Default/               # Výchozí implementace + vstupní bod aplikace (konzolová verze)
│   │   ├── Program.cs                     # Vstupní bod (sestavuje všechny komponenty)
│   │   ├── Config/                        # Výchozí konfigurační data
│   │   ├── Knowledge/                     # Implementace Znalostní Sítě
│   │   ├── Logging/                       # Implementace Poskytovatele Protokolů (konzole + souborový systém)
│   │   ├── Project/                       # Implementace projektového systému
│   │   └── Storage/                       # Implementace souborového systému úložiště
│   │
│   ├── SiliconLife.Fast/                  # Výkonnostní implementace + vstupní bod aplikace (okenní verze)
│   │   ├── Program.cs                     # Vstupní bod (desktopová aplikace)
│   │   ├── App.axaml / App.cs             # Definice Avalonia aplikace
│   │   ├── Config/                        # Konfigurační data (sdílená s Default)
│   │   ├── Knowledge/                     # Implementace Znalostní Sítě (optimalizace paměti)
│   │   ├── Logging/                       # Výkonnostní Poskytovatel Protokolů
│   │   ├── Project/                       # Implementace projektového systému
│   │   ├── Storage/                       # Adaptéry úložiště SpeedyPack
│   │   └── Tray/                          # Systémová lišta (34 jazykových variant lokalizace)
│   │
│   ├── SiliconLife.Speedy/                # SpeedyPack vysoce výkonný úložný engine
│   │   ├── SpeedyPack.cs                  # Základní třída (mapování adresářů v paměti + mezipaměť + asynchronní zápis)
│   │   ├── SpeedyPackOptions.cs           # Konfigurační možnosti (TTL mezipaměti, max. záznamů atd.)
│   │   ├── IPackTransaction.cs            # Rozhraní transakcí
│   │   ├── SpkFileInfo.cs                 # Informace o souboru
│   │   └── Internal/                      # Interní implementace
│   │       ├── DirectoryMap.cs            # Mapování adresářů v paměti
│   │       ├── EntryCache.cs              # Mezipaměť záznamů
│   │       ├── FreeList.cs                # Správa volného prostoru
│   │       ├── PackFileReader.cs          # Čtečka balíčkových souborů
│   │       ├── PackFileWriter.cs          # Zapisovač balíčkových souborů
│   │       ├── WriteQueue.cs              # Asynchronní fronta zápisů
│   │       ├── WriteOperation.cs          # Operace zápisu
│   │       ├── SpeedyTransaction.cs       # Implementace transakcí
│   │       ├── SpkHeader.cs               # Hlavička balíčkového souboru
│   │       └── PathNormalizer.cs          # Normalizace cest
│   │
│   └── SiliconLife.Speedy.Manager/        # Správa SpeedyPack (Avalonia UI)
│       ├── MainForm.cs                    # Hlavní formulář
│       ├── Program.cs                     # Vstupní bod
│       └── slc.ico                        # Ikona aplikace
│
├── docs/                                  # Vícejazyčná dokumentace
│   ├── zh-CN/                             # Dokumentace ve zjednodušené čínštině
│   ├── en/                                # Dokumentace v angličtině
│   └── ...                                # Dokumentace v dalších jazycích
│
└── 总文档/                                 # Dokumentace požadavků a architektury
    ├── 需求文档.md
    ├── 架构大纲.md
    └── 实现顺序.md
```

## 🏗️ Přehled architektury

### Architektura plánování
```
Hlavní Smyčka (vyhrazené vlákno, Hlídač + Jistič)
  └── Tick Objekty (seřazené podle priority)
       └── Správce Křemíkových Bytostí
            └── Runner Křemíkové Bytosti (dočasné vlákno, timeout + Jistič)
                 └── Křemíková Bytost.Tick()
                      └── Správce Kontextu.Přemýšlet()
                           └── AI Klient.Chat()
                                └── Smyčka volání nástrojů → Perzistence do Chatovacího Systému
```

### Bezpečnostní architektura
Všechny I/O operace iniciované AI musí projít přísným bezpečnostním řetězcem:

```
Volání nástroje → Exekutor → Správce Oprávnění → [Frekvenční mezipaměť → Zpětné volání → (IsCurator: Dotaz uživatele | Non-curator: Globální ACL)]
```

## 🚀 Rychlý start

### Předpoklady

- **.NET 9 SDK** — [Odkaz ke stažení](https://dotnet.microsoft.com/download/dotnet/9.0)
- **AI backend** (vyberte jeden):
  - **Ollama**: [Nainstalujte Ollama](https://ollama.com) a stáhněte model (např. `ollama pull llama3`)
  - **Alibaba Cloud Bailian**: Získejte API klíč z [konzole Bailian](https://bailian.console.aliyun.com/)
  - **Volcengine Ark**: Získejte API klíč z [konzole Volcengine](https://console.volcengine.com/ark)

### Sestavení projektu

```bash
dotnet restore
dotnet build
```

### Spuštění systému

#### Způsob 1: Spuštění verze Default (konzolová aplikace)

```bash
dotnet run --project src/SiliconLife.Default
```

Aplikace spustí webový server a automaticky otevře Web UI v prohlížeči.

**Vhodné scénáře**:
- ✅ Velmi vysoké požadavky na bezpečnost dat
- ✅ Omezené paměťové prostředky (RAM < 2GB)
- ✅ Malé objemy dat, krátkodobé použití
- ✅ Fáze vývoje a ladění

#### Způsob 2: Spuštění verze Fast (desktopová aplikace)

```bash
dotnet run --project src/SiliconLife.Fast
```

**Windows/macOS**: Aplikace se spustí v okenním režimu, minimalizuje se do systémové lišty a běží na pozadí.

**Linux**: Aplikace zobrazí stavové okno (bez ikony v systémové liště) a automaticky otevře prohlížeč pro přístup k Web UI. Lze také použít parametr `--no-tray` pro přeskočení automatického otevření prohlížeče:

```bash
dotnet run --project src/SiliconLife.Fast -- --no-tray
```

**Vhodné scénáře**:
- ✅ Scénáře s vysokou souběžností (> 5 uživatelů)
- ✅ Velké objemy dat (použití déle než 3 měsíce)
- ✅ Potřeba nízké latence odpovědi
- ✅ Potřeba běhu na pozadí v systémové liště

### Publikování jako jeden soubor

```bash
# Windows - verze Default
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Windows - verze Fast
dotnet publish src/SiliconLife.Fast -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux - verze Default
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# Linux - verze Fast
dotnet publish src/SiliconLife.Fast -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - verze Default
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true

# macOS - verze Fast
dotnet publish src/SiliconLife.Fast -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 Plán vývoje

### ✅ Dokončeno
- [x] Fáze 1: Konzolový AI chat
- [x] Fáze 2: Kostra frameworku (Hlavní Smyčka + Tick Objekty + Hlídač + Jistič)
- [x] Fáze 3: První Křemíková Bytost se Souborem Duše (architektura Tělo-Mozek)
- [x] Fáze 4: Perzistentní paměť (Chatovací Systém + rozhraní Časového Úložiště)
- [x] Fáze 5: Systém nástrojů + Exekutoři
- [x] Fáze 6: Systém oprávnění (5-úrovňový řetězec, Auditní Protokolovač, Globální ACL)
- [x] Fáze 7: Dynamická kompilace + sebeevoluce (Roslyn)
- [x] Fáze 8: Dlouhodobá paměť + úkoly + časovače
- [x] Fáze 9: Hlavní Uzel + multiagentní spolupráce
- [x] Fáze 10: Web UI (HTTP + SSE, 24 kontrolerů, 7 skinů)
- [x] Fáze 10.5: Inkrementální vylepšení (Vysílací Kanál, Audit Tokenů, 32 kalendářů, vylepšení nástrojů, lokalizace 34 jazykových variant)
- [x] Fáze 10.6: Dokončování a optimalizace (WebView, systém nápovědy, projektový pracovní prostor, Znalostní Síť, engine pracovních postupů)
- [x] Fáze 11: Úložný engine SpeedyPack (náhrada LiteDB, mapování v paměti, asynchronní fronta zápisů, automatická komprimace)
- [x] Fáze 12: Systém zásuvných modulů (rozhraní IPlugin, Bezpečnostní Sandbox PluginLoader, izolované načítání, integrace nástrojů)

### 🚧 V plánu
- [ ] Fáze 13: Integrace externího IM (Feishu / WhatsApp / Telegram)
- [ ] Fáze 14: Ekosystém dovedností (tržiště zásuvných modulů, distribuce balíčků dovedností)

## 📚 Dokumentace

- [Architektura](architecture.md) — návrh systému, mechanismus plánování, architektura komponent
- [Bezpečnostní model](security.md) — model oprávnění, exekutoři, bezpečnost dynamické kompilace
- [Vývojářská příručka](development-guide.md) — vývoj nástrojů, příručka rozšíření
- [API reference](api-reference.md) — dokumentace Web API koncových bodů
- [Reference nástrojů](tools-reference.md) — podrobný popis vestavěných nástrojů
- [Příručka Web UI](web-ui-guide.md) — příručka pro používání webového rozhraní
- [Příručka Křemíkové Bytosti](silicon-being-guide.md) — příručka pro vývoj agentů
- [Systém oprávnění](permission-system.md) — podrobný popis správy oprávnění
- [Kalendářní systém](calendar-system.md) — popis 32 kalendářních systémů
- [Rychlý start](getting-started.md) — podrobná příručka pro začátečníky
- [Řešení problémů](troubleshooting.md) — odpovědi na časté otázky
- [Plán](roadmap.md) — kompletní plán vývoje
- [Seznam změn](changelog.md) — historie aktualizací verzí
- [Příručka přispívání](contributing.md) — jak se zapojit do projektu

## 🤝 Přispívání

Vítáme všechny formy příspěvků! Podrobnosti naleznete v [příručce přispívání](contributing.md).

### Vývojový pracovní postup
1. Fork tohoto repozitáře
2. Vytvořte větev pro funkci (`git checkout -b feature/AmazingFeature`)
3. Potvrďte změny (`git commit -m 'feat: add some AmazingFeature'`)
4. Pushněte do větve (`git push origin feature/AmazingFeature`)
5. Vložte Pull Request

## 💡 Průvodce výběrem verze

### Kterou verzi bych měl použít?

**SiliconLife.Default (výchozí implementace — ověření proveditelnosti architektury):**
- 📌 Poprvé se setkáváte s tímto projektem a chcete rychle pochopit architekturu systému
- 📌 Provádíte vývoj a ladění a potřebujete jednoduchý a přímočarý způsob spuštění
- 📌 Bezpečnost dat je vaší hlavní prioritou
- 📌 Váš systém má méně než 4GB paměti
- 📌 Potřebujete pouze pro jednoho uživatele nebo máte malé objemy dat

**SiliconLife.Fast (hlavní produkční verze):**
- ⚡ Potřebujete dlouhodobě stabilní produkční prostředí
- ⚡ Již znáte architekturu systému a jste připraveni na formální nasazení
- ⚡ Potřebujete podporu souběžného přístupu více uživatelů
- ⚡ Potřebujete běh na pozadí v systémové liště
- ⚡ Usilujete o maximální výkon

> **Obecné doporučení**: SiliconLife.Default je vhodný jako ověření architektury a úvodní zkušenost; pro skutečné produkční prostředí se důrazně doporučuje používat SiliconLife.Fast.

### Lze migrovat z Default na Fast?

**Rozhodně ano!** Obě verze sdílejí stejný:
- ✅ Formát konfiguračního souboru (config.json)
- ✅ Rozhraní nástrojů
- ✅ Konfiguraci Bytostí
- ✅ Rozhraní Web UI

**Kroky migrace:**
1. Zálohujte svůj datový adresář Default
2. Spusťte verzi Fast se stejným datovým adresářem
3. Fast automaticky importuje existující data do úložného enginu SpeedyPack
4. Po ověření správné funkčnosti můžete verzi Fast používat každodenně

### Mohou obě verze existovat vedle sebe?

**Ano!** Doporučujeme následující strategii nasazení:

**Strategie 1: Default pro ověření, Fast pro produkci**
```
Vývojové/ověřovací prostředí: SiliconLife.Default (ověřování architektury, ladění funkcí)
Produkční prostředí: SiliconLife.Fast (vysoký výkon, běh na pozadí, zpracování požadavků v reálném čase)
```

**Strategie 2: Fast jako hlavní běh, Default pro pravidelné zálohování**
```
SiliconLife.Fast (denní používání, zpracování požadavků v reálném čase)
    ↓ Pravidelné zálohování
SiliconLife.Default (archivace studených dat, záchrana bezpečnosti dat)
```

## 📄 Licence

Tento projekt je licencován pod Apache License 2.0 — viz soubor [LICENSE](../../LICENSE).

## 👨‍💻 Autor

**Tianyuan Kenji**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- Gitee: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili: [617827040](https://space.bilibili.com/617827040)

## 🙏 Poděkování

Děkujeme všem vývojářům a poskytovatelům AI platforem, kteří přispěli k tomuto projektu.

---

**Silicon Life Collective** — nechte AI agenty skutečně "ožít"
