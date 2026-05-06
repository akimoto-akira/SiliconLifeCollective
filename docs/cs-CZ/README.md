![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**Verze: v0.1.0-alpha** | **Silikonové bytosti** — Multiagentní platforma založená na .NET 9, kde jsou AI agenti nazýváni **silikonové bytosti** s schopností sebevývoje prostřednictvím dynamické kompilace Roslyn.

[English](../README.md) | [Deutsch](../de-DE/README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Français](../fr-FR/README.md) | **Čeština**

## 🌟 Klíčové funkce

### Agentní systém
- **Orchestrace více agentů** — Spravováno *Silikonovým kurátorem* s mechanismem spravedlivého rozvrhování časových slotů řízeným hodinami
- **Řízeno souborem duše** — Každá silikonová bytost je řízena souborem core prompt (`soul.md`), který definuje jedinečnou osobnost a vzorce chování
- **Architektura Tělo-Mozek** — *Tělo* (SiliconBeing) udržuje stav života a detekuje spouštěcí scénáře; *Mozek* (ContextManager) načítá historii, volá AI, provádí nástroje a perzistuje odpovědi
- **Schopnost sebevývoje** — Prostřednictvím technologie dynamické kompilace Roslyn mohou silikonové bytosti přepisovat svůj vlastní kód pro evoluci
- **Správa stavů aktivity** — Podporuje čtyři stavy aktivity: Idle (nečinný), Working (pracující), Error (chyba), Stopped (zastavený). Automatický přechod do stavu Stopped po 10 po sobě jdoucích chybách

### Plugin systém
- **Architektura rozšíření pluginů** — Rozšíření funkcí prostřednictvím rozhraní IPlugin, podpora dynamického načítání plugin DLL z adresáře
- **Bezpečnostní sandbox** — Zavaděč pluginů provádí přísné bezpečnostní skenování, zakazuje přístup k jmenným prostorům System.IO, System.Net atd.
- **Izolované načítání** — Izolované načítání pomocí vlastního AssemblyLoadContext, zabraňuje vlivu pluginů na stabilitu hlavního programu
- **Integrace nástrojů** — Pluginy mohou registrovat vlastní nástroje prostřednictvím rozhraní ITool, automatická integrace do cyklu volání nástrojů

### Nástroje a provádění
- **24 vestavěných nástrojů** — Pokrývá kalendář, chat, konfiguraci, disk, síť, paměť, úkoly, časovače, znalostní bázi, pracovní poznámky, WebView prohlížeč, hot reload a další
- **Nástroj Hot Reload** — Podporuje automatickou kompilaci, aktualizaci souborů a restart SiliconLife.Fast za běhu, bez manuálního zásahu
- **Cyklus volání nástrojů** — AI vrací volání nástroje → provádění nástroje → výsledky zpět AI → pokračuje dokud nevrací čistý text
- **Bezpečnost oprávnění-exekutor** — Všechny I/O operace procházejí přísným ověřováním oprávnění prostřednictvím exekutorů
  - 5úrovňový řetězec oprávnění: IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - Kompletní auditní záznam všech rozhodnutí o oprávněních

### AI a znalosti
- **Podpora více AI backendů**
  - **Ollama** — Lokální nasazení modelů pomocí nativního HTTP API
  - **Alibaba Cloud Bailian (DashScope)** — Cloudová AI služba, kompatibilní s OpenAI API, podpora 13+ modelů, více regionů
  - **Volcengine Ark** — Cloudová AI služba ByteDance, podpora streaming a non-streaming režimů, integrované dvojí omezení rychlosti
- **32 kalendářních systémů** — Globální pokrytí hlavních kalendářů včetně gregoriánského, lunárního, islámského, hebrejského, japonského, perského, mayského, čínského historického kalendáře atd.
- **Systém znalostní sítě** — Znalostní graf založený na triplech (subjekt-vztah-objekt) s podporou ukládání, dotazování a objevování cest

### Webové rozhraní
- **Moderní Web UI** — Vestavěný HTTP server s podporou SSE pro aktualizace v reálném čase
- **7 témat skinů** — Administrační, chat, kreativní, vývojářská, vysoký kontrast, světlý, minimalistický, s automatickou detekcí a přepínáním
- **20+ kontrolerů** — Kompletní správa systému, chat, konfigurace, monitoring
- **Žádná závislost na frontendovém frameworku** — Generování HTML/CSS/JS na serveru pomocí `H`, `CssBuilder` a `JsBuilder`

### Internacionalizace a lokalizace
- **Plná podpora 29 jazykových implementací**, pokrývající 2 psací systémy a více regionálních variant
  - **Zjednodušená čínština**: zh-CN (pevninská Čína), zh-SG (Singapur), zh-MY (Malajsie) (3 varianty)
  - **Tradiční čínština**: zh-HK (Hongkong), zh-TW (Tchaj-wan), zh-MO (Macao) (3 varianty)
  - **Angličtina**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 variant)
  - **Španělština**: es-ES, es-MX (2 varianty)
  - **Němčina**: de-DE, de-AT, de-CH, de-LU, de-LI (5 variant)
  - **Francouzština**: fr-FR, fr-CA, fr-CH (3 varianty)
  - **Japonština**: ja-JP | **Korejština**: ko-KR | **Čeština**: cs-CZ (3 varianty)

### Data a úložiště
- **SpeedyPack vysoce výkonné úložiště** — Fast verze používá vlastní .spk úložný engine, mapování paměťových adresářů + mezipaměť záznamů + asynchronní fronta zápisu
- **Úložiště na souborovém systému** — Default verze používá čisté úložiště JSON na souborovém systému
- **Časově indexované dotazy** — Efektivní dotazování podle časového rozsahu prostřednictvím rozhraní `ITimeStorage`
- **Automatická komprese** — SpeedyPack podporuje pravidelnou automatickou kompresi pro uvolnění volného prostoru
- **Minimální závislosti** — Core knihovna závisí pouze na Microsoft.CodeAnalysis.CSharp pro dynamickou kompilaci

## 🔄 Duální verze architektury

Tento projekt poskytuje dvě implementační verze pro splnění různých požadavků scénářů:

### SiliconLife.Default (Výchozí verze)
- **Pozicování**: Výchozí implementace, používána především pro ověření proveditelnosti architektury
- **Režim spuštění**: Konzolová aplikace
- **Metoda úložiště**: Čisté úložiště JSON na souborovém systému
- **Použitelné scénáře**: Vysoké požadavky na zabezpečení dat, omezené paměťové zdroje, malý objem dat
- **Charakteristiky**: Jednoduché a spolehlivé, okamžitá perzistence dat, žádné riziko ztráty paměti
- **Popis role**: Referenční implementace pro ověření architektury, vhodná pro první kontakt, vývojové ladění nebo scénáře s prioritou bezpečnosti dat
- **Spouštěcí příkaz**: `dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast (Vysoce výkonná verze)
- **Pozicování**: Hlavní produkční verze
- **Režim spuštění**: Windows Forms aplikace (podporuje systémový tray)
- **Metoda úložiště**: SpeedyPack paměťové úložiště + asynchronní dávková perzistence (.spk formát souboru)
- **Použitelné scénáře**: Vysoká souběžnost, nízká latence, scénáře s velkým objemem dat
- **Charakteristiky**:
  - Extrémní optimalizace výkonu
  - Běh na pozadí v tray s monitorováním v reálném čase prostřednictvím stavového okna tray
  - SpeedyPack engine + automatická komprese zajišťují bezpečnost dat
  - Architektura Component UI, 30+ deklarativních komponent
  - 7 témat skinů, podpora automatické detekce a přepínání
  - Nástroj hot reload pro online aktualizace a restarty
- **Zlepšení výkonu**: Latence čtení úložiště snížena 1000x, latence zápisu snížena 15000x, kapacita souběžného zpracování zvýšena 50x
- **Popis role**: Produkční implementace s hlubokou optimalizací, nejlepší volba pro dlouhodobý provoz a reálné produkční prostředí
- **Spouštěcí příkaz**: `dotnet run --project src/SiliconLife.Fast`

### Porovnání verzí

| Funkce | SiliconLife.Default | SiliconLife.Fast |
|--------|---------------------|------------------|
| **Režim spuštění** | Konzolová aplikace | Forms aplikace (systémový tray) |
| **Uživatelské rozhraní** | Web UI (přístup přes prohlížeč) | Ikona tray + okno tray + Web UI |
| **Systémový tray** | ❌ Žádný | ✅ Podporuje minimalizaci do tray |
| **Běh na pozadí** | ❌ Ukončí se při zavření konzole | ✅ Nepřetržitý běh na pozadí v tray |
| **Metoda úložiště** | Úložiště JSON na souborovém systému | SpeedyPack paměťové úložiště + asynchronní perzistence |
| **Úložný engine** | I/O souborového systému | SiliconLife.Speedy (.spk formát) |
| **Latence čtení** | ~10ms (I/O disku) | ~0.01ms (operace v paměti) |
| **Latence zápisu** | ~15ms (synchronní zápis) | ~0.001ms (asynchronní zápis) |
| **Souběžnost** | ~100 req/s | ~5000 req/s |
| **Využití paměti** | ~200MB | ~500MB |
| **Bezpečnost dat** | Extrémně vysoká (okamžitá perzistence) | Vysoká (asynchronní perzistence + automatická komprese) |
| **Použitelné scénáře** | Bezpečnost dat jako priorita, malá data | Výkon jako priorita, velká data, vysoká souběžnost |

## 🛠️ Technologický stack

| Komponenta | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| Runtime | .NET 9 | .NET 9 Windows |
| Programovací jazyk | C# | C# |
| Typ aplikace | Konzolová aplikace | Windows Forms aplikace |
| AI integrace | Ollama (lokální), Alibaba Cloud Bailian (cloud) | Ollama (lokální), Alibaba Cloud Bailian (cloud), Volcengine Ark (cloud) |
| Úložiště dat | Souborový systém (JSON + časově indexované adresáře) | SpeedyPack (.spk formát, mapování paměti + asynchronní perzistence) |
| Webový server | HttpListener (vestavěný v .NET) | HttpListener (vestavěný v .NET) |
| Dynamická kompilace | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Automatizace prohlížeče | Playwright (WebView) | Playwright (WebView) |
| Plugin systém | ✅ Podporováno (IPlugin + PluginLoader) | ✅ Podporováno (IPlugin + PluginLoader) |
| Systémový tray | ❌ Nepodporováno | ✅ Podporováno (NotifyIcon) |
| Licence | Apache-2.0 | Apache-2.0 |

## 📁 Struktura projektu

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Core knihovna (rozhraní, abstraktní třídy)
│   │   ├── AI/                            # Rozhraní AI klientů, správce kontextu, modely zpráv
│   │   ├── Audit/                         # Systém auditu využití tokenů
│   │   ├── Chat/                          # Systém chatu, správa relací, broadcast kanály
│   │   ├── Compilation/                   # Dynamická kompilace, bezpečnostní skenování, šifrování kódu
│   │   ├── Config/                        # Systém správy konfigurace
│   │   ├── Executors/                     # Exekutory (disk, síť, příkazový řádek)
│   │   ├── IM/                            # Rozhraní poskytovatelů instantních zpráv
│   │   ├── Knowledge/                     # Systém znalostní sítě
│   │   ├── Localization/                  # Lokalizační systém
│   │   ├── Logging/                       # Systém protokolování
│   │   ├── Plugins/                       # Plugin systém (rozhraní IPlugin, zavaděč PluginLoader)
│   │   ├── Project/                       # Systém správy projektů
│   │   ├── Runtime/                       # Hlavní smyčka, objekty hodin, core hostitel
│   │   ├── Security/                      # Systém správy oprávnění
│   │   ├── SiliconBeing/                  # Základní třída silikonových bytostí, správce, továrna
│   │   ├── Storage/                       # Rozhraní úložiště
│   │   ├── Time/                          # Neúplná data (dotazování časového rozsahu)
│   │   ├── Tools/                         # Rozhraní nástrojů a správce nástrojů
│   │   ├── WebView/                       # Rozhraní WebView prohlížeče
│   │   └── ServiceLocator.cs              # Globální lokátor služeb
│   │
│   ├── SiliconLife.Common/                # Sdílená implementace (společná pro obě verze)
│   │   ├── AI/                            # Továrna AI klientů
│   │   ├── Calendar/                      # 32 implementací kalendářů
│   │   ├── Localization/                  # Základní třída lokalizace
│   │   ├── Security/                      # Správce oprávnění
│   │   ├── SiliconBeing/                  # Výchozí implementace silikonových bytostí
│   │   ├── Tools/                         # Obecná implementace nástrojů
│   │   └── WebView/                       # Rozhraní WebView
│   │
│   ├── SiliconLife.Default/               # Výchozí implementace + vstup aplikace (konzolová verze)
│   │   ├── Program.cs                     # Vstupní bod (sestavení všech komponent)
│   │   ├── Config/                        # Výchozí konfigurační data
│   │   ├── Executors/                     # Výchozí implementace exekutorů
│   │   ├── Help/                          # Systém nápovědy
│   │   ├── IM/                            # Poskytovatel WebUI
│   │   ├── Knowledge/                     # Implementace znalostní sítě
│   │   ├── Localization/                  # Lokalizace 21 jazyků
│   │   ├── Logging/                       # Implementace poskytovatelů protokolů
│   │   ├── Project/                       # Implementace systémů projektů
│   │   ├── Runtime/                       # Testovací objekty hodin
│   │   ├── Security/                      # Výchozí zpětné volání oprávnění
│   │   ├── SiliconBeing/                  # Výchozí implementace silikonových bytostí
│   │   ├── Storage/                       # Implementace úložiště na souborovém systému
│   │   ├── Tools/                         # Implementace vestavěných nástrojů
│   │   ├── WebView/                       # Implementace Playwright WebView
│   │   └── Web/                           # Implementace Web UI
│   │       ├── Controllers/               # 20+ kontrolerů
│   │       ├── Models/                    # View modely
│   │       ├── Views/                     # HTML pohledy
│   │       └── Skins/                     # 4 témata skinů
│   │
│   └── SiliconLife.Fast/                  # Vysoce výkonná implementace + vstup aplikace (Forms verze)
│       ├── Program.cs                     # Vstupní bod (Forms aplikace)
│       ├── Config/                        # Konfigurační data (sdílená s Default)
│       ├── Executors/                     # Optimalizovaná implementace exekutorů
│       ├── Help/                          # Systém nápovědy
│       ├── IM/                            # Poskytovatel WebUI
│       ├── Knowledge/                     # Implementace znalostní sítě (optimalizace paměti)
│       ├── Localization/                  # Lokalizace 21 jazyků
│       ├── Logging/                       # Vysoce výkonný poskytovatel protokolů
│       ├── Project/                       # Implementace systémů projektů
│       ├── Security/                      # Optimalizované zpětné volání oprávnění
│       ├── SiliconBeing/                  # Vysoce výkonná implementace silikonových bytostí
│       ├── Storage/                       # SpeedyPack adaptér úložiště
│       ├── Tools/                         # Optimalizovaná implementace vestavěných nástrojů
│       ├── Tray/                          # Systémový tray (9 jazykových lokalizací)
│       ├── WebView/                       # Implementace Playwright WebView
│       └── Web/                           # Vysoce výkonná implementace Web UI
│           ├── Component/                 # Knihovna UI komponent (30+ komponent)
│           ├── Controllers/               # 20+ kontrolerů
│           ├── Models/                    # View modely
│           ├── Views/                     # HTML pohledy
│           └── Skins/                     # 7 témat skinů
│
│   ├── SiliconLife.Speedy/                # SpeedyPack vysoce výkonný úložný engine
│   │   ├── SpeedyPack.cs                  # Jádrová třída (mapování paměťových adresářů + mezipaměť + asynchronní zápis)
│   │   ├── SpeedyPackOptions.cs           # Konfigurační možnosti (TTL mezipaměti, max. počet záznamů atd.)
│   │   ├── IPackTransaction.cs            # Rozhraní transakcí
│   │   ├── SpkFileInfo.cs                 # Informace o souboru
│   │   └── Internal/                      # Interní implementace
│       │   ├── DirectoryMap.cs            # Mapování paměťových adresářů
│       │   ├── EntryCache.cs              # Mezipaměť záznamů
│       │   ├── FreeList.cs                # Správa volného prostoru
│       │   ├── PackFileReader.cs          # Čtečka balíčkových souborů
│       │   ├── PackFileWriter.cs          # Zapisovač balíčkových souborů
│       │   ├── WriteQueue.cs              # Asynchronní fronta zápisu
│       │   ├── WriteOperation.cs          # Operace zápisu
│       │   ├── SpeedyTransaction.cs       # Implementace transakcí
│       │   ├── SpkHeader.cs              # Hlavička balíčkového souboru
│       │   └── PathNormalizer.cs          # Normalizace cest
│   │
│   └── SiliconLife.Speedy.Manager/        # SpeedyPack správní nástroj (WPF)
│       ├── MainForm.cs                    # Hlavní okno
│       ├── Program.cs                     # Vstupní bod
│       └── slc.ico                        # Ikona aplikace
│
├── docs/                                  # Vícejazyčná dokumentace
│   ├── zh-CN/                             # Dokumentace v zjednodušené čínštině
│   ├── en/                                # Anglická dokumentace
│   └── ...                                # Další jazykové dokumentace
│
└── 总文档/                                 # Dokumentace požadavků a architektury
    ├── 需求文档.md
    ├── 架构大纲.md
    └── 实现顺序.md
```

## 🏗️ Přehled architektury

### Architektura rozvrhování
```
Hlavní smyčka (vyhrazený thread, watchdog + jistič)
  └── Objekty hodin (seřazeno podle priority)
       └── Správce silikonových bytostí
            └── Běžec silikonových bytostí (dočasný thread, timeout + jistič)
                 └── SiliconBeing.Tick()
                      └── ContextManager.Přemýšlet()
                           └── AI Klient.Chat()
                                └── Cyklus volání nástrojů → Perzistence do systému chatu
```

### Bezpečnostní architektura
Všechny I/O operace iniciované AI musí procházet přísným bezpečnostním řetězcem:

```
Volání nástroje → Exekutor → Správce oprávnění → [IsCurator → Frekvenční cache → GlobalACL → Zpětné volání → Dotaz uživatele]
```

## 🚀 Rychlý start

### Předpoklady

- **.NET 9 SDK** — [Odkaz ke stažení](https://dotnet.microsoft.com/download/dotnet/9.0)
- **AI Backend** (vyberte jeden):
  - **Ollama**: [Instalace Ollama](https://ollama.com) a pull modelu (např. `ollama pull llama3`)
  - **Alibaba Cloud Bailian**: Získejte API klíč z [Bailian konzole](https://bailian.console.aliyun.com/)

### Sestavení projektu

```bash
dotnet restore
dotnet build
```

### Spuštění systému

#### Metoda 1: Spuštění výchozí verze (konzolová aplikace)

```bash
dotnet run --project src/SiliconLife.Default
```

Aplikace spustí webový server a automaticky otevře Web UI v prohlížeči.

**Použitelné scénáře**:
- ✅ Extrémně vysoké požadavky na bezpečnost dat
- ✅ Omezené paměťové zdroje (RAM < 2GB)
- ✅ Malý objem dat, krátkodobé použití
- ✅ Fáze vývojového ladění

#### Metoda 2: Spuštění Fast verze (Windows Forms aplikace)

```bash
dotnet run --project src/SiliconLife.Fast
```

Aplikace se spustí v režimu okna, minimalizuje se do systémového tray a běží nepřetržitě na pozadí.

**Použitelné scénáře**:
- ✅ Scénáře s vysokou souběžností (> 5 uživatelů)
- ✅ Velký objem dat (použití více než 3 měsíce)
- ✅ Potřeba nízkolatencních odpovědí
- ✅ Potřeba běhu na pozadí v tray

### Publikování jako jeden soubor

```bash
# Windows - výchozí verze
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Windows - Fast verze
dotnet publish src/SiliconLife.Fast -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux - pouze výchozí verze
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - pouze výchozí verze
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 Roadmapa vývoje

### ✅ Dokončeno
- [x] Fáze 1: Konzolový AI chat
- [x] Fáze 2: Kostra frameworku (hlavní smyčka + objekty hodin + watchdog + jistič)
- [x] Fáze 3: První silikonová bytost se souborem duše (architektura Tělo-Mozek)
- [x] Fáze 4: Perzistentní paměť (systém chatu + rozhraní časového úložiště)
- [x] Fáze 5: Systém nástrojů + Exekutory
- [x] Fáze 6: Systém oprávnění (5úrovňový řetězec, audit logger, Global ACL)
- [x] Fáze 7: Dynamická kompilace + Sebevývoj (Roslyn)
- [x] Fáze 8: Dlouhodobá paměť + Úkoly + Časovače
- [x] Fáze 9: Core Hostitel + Spolupráce více agentů
- [x] Fáze 10: Web UI (HTTP + SSE, 20+ kontrolerů, 4 skiny)
- [x] Fáze 10.5: Přírůstková vylepšení (broadcast kanál, audit tokenů, 32 kalendářů, vylepšení nástrojů, lokalizace 21 jazyků)
- [x] Fáze 10.6: Dokončení a optimalizace (WebView, systém nápovědy, pracovní prostor projektu, znalostní síť)
- [x] Fáze 11: SpeedyPack úložný engine (náhrada LiteDB, mapování paměti, asynchronní fronta zápisu, automatická komprese)
- [x] Fáze 12: Plugin systém (rozhraní IPlugin, bezpečnostní sandbox PluginLoader, izolované načítání, integrace nástrojů)

### 🚧 Plánováno
- [ ] Fáze 13: Integrace externích instantních zpráv (Feishu / WhatsApp / Telegram)
- [ ] Fáze 14: Ekosystém dovedností (tržiště pluginů, distribuce balíčků dovedností)

## 📚 Dokumentace

- [Návrh architektury](architecture.md) — Systémový design, mechanismus rozvrhování, architektura komponent
- [Bezpečnostní model](security.md) — Model oprávnění, exekutory, bezpečnost dynamické kompilace
- [Vývojářský průvodce](development-guide.md) — Vývoj nástrojů, průvodce rozšířením
- [API reference](api-reference.md) — Dokumentace Web API endpointů
- [Reference nástrojů](tools-reference.md) — Podrobný popis vestavěných nástrojů
- [Průvodce Web UI](web-ui-guide.md) — Průvodce použitím webového rozhraní
- [Průvodce silikonovou bytostí](silicon-being-guide.md) — Průvodce vývojem agentů
- [Systém oprávnění](permission-system.md) — Podrobnosti správy oprávnění
- [Kalendářní systém](calendar-system.md) — Popis 32 kalendářních systémů
- [Rychlý start](getting-started.md) — Podrobný úvodní průvodce
- [Odstraňování problémů](troubleshooting.md) — FAQ
- [Roadmapa](roadmap.md) — Kompletní plán vývoje
- [Changelog](changelog.md) — Historie aktualizací verzí
- [Příspěvek](contributing.md) — Jak se zapojit do projektu

## 🤝 Přispívání

Vítáme všechny formy příspěvků! Podrobnosti naleznete v [Průvodci přispíváním](contributing.md).

### Vývojářský workflow
1. Forkněte toto repo
2. Vytvořte větev funkce (`git checkout -b feature/AmazingFeature`)
3. Commitněte změny (`git commit -m 'feat: add some AmazingFeature'`)
4. Pushněte do větve (`git push origin feature/AmazingFeature`)
5. Otevřete Pull Request

## 💡 Průvodce výběrem verze

### Kterou verzi bych měl použít?

**SiliconLife.Default (Výchozí implementace — ověření proveditelnosti architektury):**
- 📌 Poprvé se setkáváte s tímto projektem a chcete rychle pochopit systémovou architekturu
- 📌 Provádíte vývojové ladění a potřebujete jednoduchý a přímý způsob spuštění

**SiliconLife.Fast (Vysoce výkonná verze — hlavní produkční verze):**
- 🚀 Provozní prostředí vyžadující vysoký výkon a nízkou latenci
- 🚀 Dlouhodobý provoz, nepřetržitý běh na pozadí
- 🚀 Velký objem dat, scénáře s vysokou souběžností

### Jak migrovat z Default na Fast?

**Lze migrovat?** Ano! Obě verze sdílejí:
- ✔️ Formát konfiguračních souborů (config.json)
- ✔️ Strukturu datových adresářů
- ✔️ Rozhraní nástrojů
- ✔️ Konfiguraci Being
- ✔️ Rozhraní Web UI

**Kroky migrace:**
1. Zálohujte svůj datový adresář Default
2. Spusťte Fast verzi se stejným datovým adresářem
3. Fast automaticky importuje existující data do SpeedyPack úložného enginu
4. Po ověření správné funkce můžete Fast verzi používat pro každodenní práci

### Mohou obě verze existovat vedle sebe?
Ano, ale **nedoporučuje se** spouštět obě verze současně se stejným datovým adresářem, aby se předešlo konfliktům při zápisu dat. Doporučuje se používat pouze jednu verzi v daném okamžiku.

## 📄 Licence

Tento projekt je licencován pod Apache License 2.0 — viz soubor [LICENSE](../../LICENSE).

## 👨‍💻 Autor

**Hoshino Kennji**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- Gitee: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili: [617827040](https://space.bilibili.com/617827040)

## 🙏 Poděkování

Děkujeme všem vývojářům a poskytovatelům AI platforem, kteří přispěli k tomuto projektu.

---

**Silicon Life Collective** — Nechte AI agenty skutečně "ožít"
