# Rychlý start

> **Verze: v0.2.0-alpha**

[English](../en/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | **Čeština** | [Русский](../ru-RU/getting-started.md)

## Výběr verze

Tento projekt nabízí dvě implementační verze:

### SiliconLife.Default (výchozí verze)
- **Zaměření**: výchozí implementace, primárně pro ověření proveditelnosti architektury
- **Režim běhu**: konzolová aplikace
- **Způsob úložiště**: souborový systém JSON úložiště
- **Vhodné scénáře**: priorita bezpečnosti dat, malé objemy dat, vývoj a ladění, ověření architektury
- **Podpora platforem**: Windows, Linux, macOS
- **Popis role**: jako referenční implementace pro ověření architektury, poskytuje jednoduchý a spolehlivý způsob běhu, vhodná pro první seznámení s projektem nebo pro vývoj a ladění

### SiliconLife.Fast (vysoce výkonná verze)
- **Zaměření**: hlavní produkční verze
- **Režim běhu**: desktopová aplikace (systémová lišta Windows/macOS / stavové okno Linux)
- **Způsob úložiště**: SpeedyPack paměťové úložiště + asynchronní perzistence (formát souboru .spk)
- **Vhodné scénáře**: vysoká souběžnost, nízká latence, velké objemy dat, dlouhodobý produkční provoz
- **Podpora platforem**: Windows/macOS (kompletní funkce včetně systémové lišty), Linux (stavové okno, bez ikony v liště)
- **Popis role**: hluboce optimalizovaná produkční implementace, první volba pro dlouhodobý provoz a skutečné produkční prostředí

> **Doporučení pro začátečníky**: Pro první použití doporučujeme začít s **SiliconLife.Default** pro rychlé ověření proveditelnosti architektury; po seznámení se systémem se důrazně doporučuje migrovat na **SiliconLife.Fast** jako produkční verzi.

## Předpoklady

- **.NET 9 SDK** - [Stažení](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Stažení](https://git-scm.com/)
- **Ollama** (volitelné, pro lokální AI) - [Stažení](https://ollama.com/)
- **API klíč Bailian** (volitelné, pro cloudovou AI) - [Žádost](https://bailian.console.aliyun.com/)
- **API klíč Volcengine Ark** (volitelné, pro cloudovou AI) - [Žádost](https://console.volcengine.com/ark)

## Rychlý start

### 1. Klonování repozitáře

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. Sestavení projektu

```bash
dotnet build
```

### 3. Konfigurace AI backendu

Upravte `src/SiliconLife.Default/Config/DefaultConfigData.cs` nebo upravte konfiguraci za běhu prostřednictvím Web UI.

#### Možnost A: Ollama (lokální)

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

#### Možnost B: Bailian (cloud)

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "your-api-key-here",
      "Model": "qwen-plus",
      "Region": "beijing"
    }
  }
}
```

> **Dostupné regiony**: `beijing` (Peking), `virginia` (Virginie), `singapore` (Singapur), `hongkong` (Hongkong), `frankfurt` (Frankfurt)

#### Možnost C: Volcengine Ark (cloud)

```json
{
  "AIClients": {
    "VolcengineArk": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://ark.cn-beijing.volces.com/api/v3/chat/completions",
      "Model": "ep-xxxxxxxxxxxxx-xxxxx"
    }
  }
}
```

> **Poznámka**: Parametr Model pro Volcengine Ark přijímá ID inferenčního přístupového bodu (např. `ep-20241212123456-abcde`), nikoli název modelu.

### 4. Spuštění aplikace

#### Spuštění verze Default

```bash
cd src/SiliconLife.Default
dotnet run
```

Webový server se spustí na `http://localhost:8080`

#### Spuštění verze Fast

```bash
cd src/SiliconLife.Fast
dotnet run
```

**Windows/macOS**: Aplikace se spustí v okenním režimu, minimalizuje se do systémové lišty, webový server se spustí na `http://localhost:8080`

**Linux**: Aplikace zobrazí stavové okno (bez ikony v systémové liště) a automaticky otevře prohlížeč pro přístup k Web UI. Lze také použít parametr `--no-tray` pro přeskočení automatického otevření prohlížeče:

```bash
dotnet run -- --no-tray
```

### 5. Přístup k Web UI

Otevřete prohlížeč a přejděte na:

```
http://localhost:8080
```

Uvidíte řídicí panel obsahující:
- Správu Křemíkových Bytostí
- Chatovací rozhraní
- Konfigurační panel
- Sledování systému

## První Křemíková Bytost

### Vytvoření vaší první bytosti

1. V Web UI přejděte na **Správa bytostí**
2. Klikněte na **Vytvořit novou bytost**
3. Nakonfigurujte Soubor Duše (`soul.md`) s osobností a chováním
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

## Časté problémy

### Ollama připojení zamítnuto

**Problém**: Nelze se připojit k Ollama na `http://localhost:11434`

**Řešení**:
```bash
# Zkontrolujte, zda Ollama běží
ollama list

# Pokud potřebujete spustit Ollama
ollama serve
```

### Model nenalezen

**Problém**: `model "qwen2.5:7b" not found`

**Řešení**:
```bash
# Stáhněte požadovaný model
ollama pull qwen2.5:7b
```

### Port je již obsazen

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

## Další kroky

- 📚 Přečtěte [příručku architektury](architecture.md) pro pochopení návrhu systému
- 🛠️ Prohlédněte [vývojářskou příručku](development-guide.md) pro rozšíření systému
- 📖 Prozkoumejte [API referenci](api-reference.md) pro detaily integrace
- 🔒 Prohlédněte [dokumentaci zabezpečení](security.md) pro systém oprávnění
- 🧰 Prohlédněte [referenci nástrojů](tools-reference.md) pro všechny vestavěné nástroje
- 🌐 Prohlédněte [příručku Web UI](web-ui-guide.md) pro funkce rozhraní

## Struktura projektu

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Jádrová rozhraní a abstraktní třídy
│   ├── SiliconLife.Common/          # Sdílená implementace (společná pro obě verze)
│   ├── SiliconLife.App/             # Aplikační vrstva sdílená mezi Default a Fast
│   ├── SiliconLife.Default/         # Výchozí implementace + vstupní bod (konzolová verze)
│   ├── SiliconLife.Fast/            # Výkonnostní implementace + vstupní bod (okenní verze)
│   ├── SiliconLife.Speedy/          # SpeedyPack vysoce výkonný úložný engine
│   └── SiliconLife.Speedy.Manager/  # Správa SpeedyPack (Avalonia UI)
├── docs/                            # Dokumentace (vícejazyčná, 34 jazykových variant)
│   ├── en/                          # Angličtina
│   ├── zh-CN/                       # Zjednodušená čínština
│   ├── zh-HK/                       # Tradiční čínština
│   ├── es-ES/                       # Španělština
│   ├── ja-JP/                       # Japonština
│   ├── ko-KR/                       # Korejština
│   └── cs-CZ/                       # Čeština
├── 总文档/                           # Dokumentace požadavků a architektury (čínština)
└── README.md                        # Popis projektu
```

## Potřebujete pomoc?

- 📖 Prohlédněte [systém nápovědy](web-ui-guide.md#帮助文档系统新增) (vícejazyčná podpora)
- 📚 Přečtěte [kompletní dokumentaci](docs/)
- 🐛 Nahlaste problémy na [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Zapojte se do komunitní diskuse
