# Silicon Life Collective

**Verze: v0.1.0-alpha** | **Siliconové bytosti** — Multiagentní platforma založená na .NET 9, kde jsou AI agenti nazýváni **silikonové bytosti** s schopností sebevývoje prostřednictvím dynamické kompilace Roslyn.

[English](../README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Deutsch](../de-DE/README.md) | **Čeština**

## 🌟 Klíčové funkce

### Agentní systém
- **Orchestrace více agentů** — Spravováno *Silikonovým kurátorem* s mechanismem spravedlivého rozvrhování časových slotů řízeným hodinami
- **Řízeno souborem duše** — Každá silikonová bytost je řízena souborem core prompt (`soul.md`), který definuje jedinečnou osobnost a vzorce chování
- **Architektura Tělo-Mozek** — *Tělo* (SiliconBeing) udržuje stav života a detekuje spouštěcí scénáře; *Mozek* (ContextManager) načítá historii, volá AI, provádí nástroje a perzistuje odpovědi
- **Schopnost sebevývoje** — Prostřednictvím technologie dynamické kompilace Roslyn mohou silikonové bytosti přepisovat svůj vlastní kód pro evoluci

### Nástroje a provádění
- **23 vestavěných nástrojů** — Pokrývá kalendář, chat, konfiguraci, disk, síť, paměť, úkoly, časovače, znalostní bázi, pracovní poznámky, WebView prohlížeč a další
- **Cyklus volání nástrojů** — AI vrací volání nástroje → provádění nástroje → výsledky zpět AI → pokračuje dokud nevrací čistý text
- **Bezpečnost oprávnění-exekutor** — Všechny I/O operace procházejí přísným ověřováním oprávnění prostřednictvím exekutorů
  - 5úrovňový řetězec oprávnění: IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - Kompletní auditní záznam všech rozhodnutí o oprávněních

### AI a znalosti
- **Podpora více AI backendů**
  - **Ollama** — Lokální nasazení modelů pomocí nativního HTTP API
  - **Alibaba Cloud Bailian (DashScope)** — Cloudová AI služba, kompatibilní s OpenAI API, podpora 13+ modelů, více regionů
- **32 kalendářních systémů** — Globální pokrytí hlavních kalendářů včetně gregoriánského, lunárního, islámského, hebrejského, japonského, perského, mayského, čínského historického kalendáře atd.
- **Systém znalostní sítě** — Znalostní graf založený na triplech (subjekt-vztah-objekt) s podporou ukládání, dotazování a objevování cest

### Webové rozhraní
- **Moderní Web UI** — Vestavěný HTTP server s podporou SSE pro aktualizace v reálném čase
- **4 témata skinů** — Administrační, chat, kreativní, vývojářská verze s automatickou detekcí a přepínáním
- **20+ kontrolerů** — Kompletní správa systému, chat, konfigurace, monitoring
- **Žádná závislost na frontendovém frameworku** — Generování HTML/CSS/JS na serveru pomocí `H`, `CssBuilder` a `JsBuilder`

### Internacionalizace a lokalizace
- **Plná podpora 21 jazykových variant**
  - Čínština: zh-CN, zh-HK, zh-SG, zh-MO, zh-TW, zhMY (6 variant)
  - Angličtina: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 variant)
  - Španělština: es-ES, es-MX (2 varianty)
  - Japonština: ja-JP | Korejština: ko-KR | Čeština: cs-CZ

### Data a úložiště
- **Žádná závislost na databázi** — Čisté úložiště na souborovém systému (formát JSON)
- **Časově indexované dotazy** — Efektivní dotazování podle časového rozsahu prostřednictvím rozhraní `ITimeStorage`
- **Minimální závislosti** — Core knihovna závisí pouze na Microsoft.CodeAnalysis.CSharp pro dynamickou kompilaci

## 🔄 Duální verze architektury

Tento projekt poskytuje dvě implementační verze pro splnění různých požadavků scénářů:

### SiliconLife.Default (Výchozí verze)
- **Režim spuštění**: Konzolová aplikace
- **Metoda úložiště**: Čisté úložiště JSON na souborovém systému
- **Použitelné scénáře**: Vysoké požadavky na zabezpečení dat, omezené paměťové zdroje, malý objem dat
- **Charakteristiky**: Jednoduché a spolehlivé, okamžitá perzistence dat, žádné riziko ztráty paměti
- **Spouštěcí příkaz**: `dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast (Vysoce výkonná verze)
- **Režim spuštění**: Windows Forms aplikace (podporuje systémový tray)
- **Metoda úložiště**: Úložiště v paměti + asynchronní dávková perzistence
- **Použitelné scénáře**: Vysoká souběžnost, nízká latence, scénáře s velkým objemem dat
- **Charakteristiky**: Extrémní optimalizace výkonu, běh na pozadí v tray, databáze v paměti + WAL protokoly zajišťují bezpečnost dat
- **Zlepšení výkonu**: Latence čtení úložiště snížena 1000x, latence zápisu snížena 15000x, kapacita souběžného zpracování zvýšena 50x
- **Spouštěcí příkaz**: `dotnet run --project src/SiliconLife.Fast`

### Porovnání verzí

| Funkce | SiliconLife.Default | SiliconLife.Fast |
|--------|---------------------|------------------|
| **Režim spuštění** | Konzolová aplikace | Forms aplikace (systémový tray) |
| **Uživatelské rozhraní** | Web UI (přístup přes prohlížeč) | Ikona tray + okno tray + Web UI |
| **Systémový tray** | ❌ Žádný | ✅ Podporuje minimalizaci do tray |
| **Běh na pozadí** | ❌ Ukončí se při zavření konzole | ✅ Nepřetržitý běh na pozadí v tray |
| **Metoda úložiště** | Úložiště JSON na souborovém systému | Úložiště v paměti + asynchronní perzistence |
| **Latence čtení** | ~10ms (I/O disku) | ~0.01ms (operace v paměti) |
| **Latence zápisu** | ~15ms (synchronní zápis) | ~0.001ms (asynchronní zápis) |
| **Souběžnost** | ~100 req/s | ~5000 req/s |
| **Využití paměti** | ~200MB | ~500MB |
| **Bezpečnost dat** | Extrémně vysoká (okamžitá perzistence) | Vysoká (WAL protokoly + asynchronní perzistence) |
| **Použitelné scénáře** | Bezpečnost dat jako priorita, malá data | Výkon jako priorita, velká data, vysoká souběžnost |

## 🛠️ Technologický stack

| Komponenta | Technologie |
|------|------|
| Runtime | .NET 9 |
| Programovací jazyk | C# |
| AI integrace | Ollama (lokální), Alibaba Cloud Bailian (cloud) |
| Úložiště dat | Souborový systém (JSON + časově indexované adresáře) |
| Webový server | HttpListener (vestavěný v .NET) |
| Dynamická kompilace | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Automatizace prohlížeče | Playwright (WebView) |
| Licence | Apache-2.0 |

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
│   └── SiliconLife.Default/               # Výchozí implementace + vstup aplikace
│       ├── Program.cs                     # Vstupní bod (sestavení všech komponent)
│       ├── AI/                            # Klient Ollama, klient Bailian
│       ├── Calendar/                      # 32 implementací kalendářů
│       ├── Config/                        # Výchozí konfigurační data
│       ├── Executors/                     # Výchozí implementace exekutorů
│       ├── Help/                          # Systém nápovědy
│       ├── IM/                            # Poskytovatel WebUI
│       ├── Knowledge/                     # Implementace znalostní sítě
│       ├── Localization/                  # Lokalizace 21 jazyků
│       ├── Logging/                       # Implementace poskytovatelů protokolů
│       ├── Project/                       # Implementace systémů projektů
│       ├── Runtime/                       # Testovací objekty hodin
│       ├── Security/                      # Výchozí zpětné volání oprávnění
│       ├── SiliconBeing/                  # Výchozí implementace silikonových bytostí
│       ├── Storage/                       # Implementace úložiště na souborovém systému
│       ├── Tools/                         # 23 vestavěných nástrojů
│       ├── WebView/                       # Implementace Playwright WebView
│       └── Web/                           # Implementace Web UI
│           ├── Controllers/               # 20+ kontrolerů
│           ├── Models/                    # View modely
│           ├── Views/                     # HTML pohledy
│           └── Skins/                     # 4 témata skinů
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
                      └── ContextManager.思考()
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

```bash
dotnet run --project src/SiliconLife.Default
```

Aplikace spustí webový server a automaticky otevře Web UI v prohlížeči.

### Publikování jako jeden soubor

```bash
# Windows
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS
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

### 🚧 Plánováno
- [ ] Fáze 11: Integrace externích instantních zpráv (Feishu / WhatsApp / Telegram)
- [ ] Fáze 12: Systém pluginů a ekosystém dovedností

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
