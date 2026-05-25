# Rychlý Start

> **Verze: v0.2.0-alpha**

[English](../en/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | **Čeština** | [Русский](../ru-RU/getting-started.md)

## Vybrat Verzi

Tento projekt poskytuje dvě implementační verze:

### SiliconLife.Default (Výchozí Verze)
- **Pozicování**: Výchozí implementace, používána především pro ověření proveditelnosti architektury
- **Režim Spuštění**: Konzolová aplikace
- **Úložiště**: Souborové JSON úložiště
- **Scénář Použití**: Priorita bezpečnosti dat, malý objem dat, vývojové ladění, ověření architektury
- **Podpora Platformy**: Windows, Linux, macOS
- **Popis role**: Referenční implementace pro ověření architektury, poskytuje jednoduché a spolehlivé spuštění, vhodná pro první kontakt nebo vývojové ladění

### SiliconLife.Fast (Vysoce Výkonná Verze)
- **Pozicování**: Hlavní produkční verze
- **Režim Spuštění**: Desktopová aplikace (Windows/macOS systémová lišta / Linux stavové okno)
- **Úložiště**: SpeedyPack paměťové úložiště + asynchronní perzistence (.spk souborový formát)
- **Scénář Použití**: Vysoká souběžnost, nízká latence, velký objem dat, dlouhodobý produkční provoz
- **Podpora Platformy**: Windows/macOS (plné funkce, včetně systémové lišty), Linux (stavové okno, bez ikony v liště)
- **Popis role**: Produkční implementace s hlubokou optimalizací, nejlepší volba pro dlouhodobý provoz a reálné produkční prostředí

> **Doporučení pro Začátečníky**: Noví uživatelé by měli začít s **SiliconLife.Default**, aby rychle ověřili proveditelnost architektury. Po seznámení se systémem se důrazně doporučuje migrovat na **SiliconLife.Fast**.

## Předpoklady

- **.NET 9 SDK** - [Stáhnout](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Stáhnout](https://git-scm.com/)
- **Ollama** (volitelné, pro lokální AI) - [Stáhnout](https://ollama.com/)
- **Bailian API klíč** (volitelné, pro cloudovou AI) - [Žádat](https://bailian.console.aliyun.com/)
- **Volcengine Ark API klíč** (volitelné, pro cloudovou AI) - [Žádat](https://console.volcengine.com/ark)

## Rychlý Start

### 1. Klonovat Repozitář

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. Sestavit Projekt

```bash
dotnet build
```

### 3. Konfigurovat AI Backend

Upravte `src/SiliconLife.Default/Config/DefaultConfigData.cs` nebo změňte konfiguraci za běhu prostřednictvím Web UI.

#### Možnost A: Ollama (Lokální)

```json
{
  "AIClients": {
    "Ollama": {
      "BaseUrl": "http://localhost:11434",
      "Model": "qwen2.5:7b"
    }
  }
}
```

#### Možnost B: Bailian (Cloudový)

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "vas-api-klic-zde",
      "Model": "qwen3.6-plus",
      "Region": "beijing"
    }
  }
}
```

#### Možnost C: Volcengine Ark (Cloudový)

```json
{
  "AIClients": {
    "VolcengineArk": {
      "ApiKey": "vas-api-klic-zde",
      "Endpoint": "https://ark.cn-beijing.volces.com/api/v3/chat/completions",
      "Model": "ep-xxxxxxxxxxxxx-xxxxx"
    }
  }
}
```

> **Poznámka**: Parametr Model služby Volcengine Ark přijímá ID inferenčního koncového bodu (např. `ep-20241212123456-abcde`), nikoli název modelu.

### 4. Spustit Aplikaci

#### Spustit Default Verzi

```bash
cd src/SiliconLife.Default
dotnet run
```

Webový server se spustí na `http://localhost:8080`

#### Spustit Fast Verzi

```bash
cd src/SiliconLife.Fast
dotnet run
```

**Windows/macOS**: Aplikace se spustí v režimu formuláře, minimalizována do systémové lišty, s webovým serverem také na `http://localhost:8080`

**Linux**: Aplikace zobrazí stavové okno (bez ikony v systémové liště) a automaticky otevře prohlížeč pro přístup k Web UI. Parametrem `--no-tray` lze přeskočit automatické otevření prohlížeče:

```bash
dotnet run -- --no-tray
```

### 5. Přístup k Web UI

Otevřete prohlížeč a navigujte na:

```
http://localhost:8080
```

Uvidíte dashboard obsahující:
- Správu silikonových bytostí
- Rozhraní chatu
- Konfigurační panel
- Monitorování systému

## Vaše První Silikonová Bytost

### Vytvoření Vaší První Bytosti

1. Navigujte na **Správa Bytostí** ve Web UI
2. Klikněte na **Vytvořit Novou Bytost**
3. Konfigurujte soubor duše (`soul.md`) s osobností a chováním
4. Spusťte bytost

### Příklad soul.md

```markdown
# My First Silicon Being

## Personality
You are a helpful assistant specializing in code review.

## Capabilities
- Review code quality
- Suggest improvements
- Explain complex concepts

## Behavior
- Always provide constructive feedback
- Use clear examples
- Be concise but thorough
```

## Časté Otázky

### Ollama Odmítnutí Připojení

**Problém**: Nelze se připojit k Ollama na `http://localhost:11434`

**Řešení**:
```bash
# Zkontrolujte, zda Ollama běží
ollama list

# Pokud potřebujete spustit Ollama
ollama serve
```

### Model Nenalezen

**Problém**: `model "qwen2.5:7b" not found`

**Řešení**:
```bash
# Stáhněte požadovaný model
ollama pull qwen2.5:7b
```

### Port Již Používán

**Problém**: `HttpListenerException: Address already in use`

**Řešení**:
- Změňte port v konfiguraci
- Nebo ukončete proces používající port 8080:

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## Další Kroky

- 📚 Přečtěte si [Průvodce Architektury](architecture.md) pro pochopení designu systému
- 🛠️ Podívejte se na [Vývojářskou Příručku](development-guide.md) pro rozšíření systému
- 📖 Prozkoumejte [Referenci API](api-reference.md) pro detaily integrace
- 🔒 Podívejte se na [Bezpečnostní Dokumentaci](security.md) pro systém oprávnění
- 🧰 Podívejte se na [Referenci Nástrojů](tools-reference.md) pro všechny vestavěné nástroje
- 🌐 Podívejte se na [Průvodce Web UI](web-ui-guide.md) pro funkce rozhraní

## Struktura Projektu

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Core rozhraní a abstraktní třídy
│   ├── SiliconLife.Common/          # Sdílené implementace (společné pro obě verze)
│   ├── SiliconLife.App/             # Aplikační vrstva sdílená Default a Fast
│   ├── SiliconLife.Default/         # Výchozí implementace + vstupní bod (konzolová verze)
│   ├── SiliconLife.Fast/            # Vysoce výkonná implementace + vstupní bod (verze s formuláři)
│   ├── SiliconLife.Speedy/          # SpeedyPack vysoce výkonný úložný engine
│   └── SiliconLife.Speedy.Manager/  # SpeedyPack nástroj pro správu (Windows Forms)
├── docs/                            # Dokumentace (vícejazyčná, 29 jazykových variant)
│   ├── en/                          # Angličtina
│   ├── zh-CN/                       # Zjednodušená čínština
│   ├── zh-HK/                       # Tradiční čínština
│   ├── es-ES/                       # Španělština
│   ├── ja-JP/                       # Japonština
│   ├── ko-KR/                       # Korejština
│   └── cs-CZ/                       # Čeština
├── 总文档/                           # Požadavky a dokumenty architektury (čínština)
└── README.md                        # Popis projektu
```

## Potřebujete Pomoc?

- 📖 Podívejte se na [Systém Nápovědní Dokumentace](web-ui-guide.md) (vícejazyčná podpora)
- 📚 Přečtěte si [Kompletní Dokumentaci](docs/)
- 🐛 Nahlaste problémy na [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Zapojte se do komunitní diskuse
