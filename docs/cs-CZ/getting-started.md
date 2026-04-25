# Rychlý start

[English](../en/getting-started.md) | [中文文档](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | [Čeština](../cs-CZ/getting-started.md)

## Požadavky

- **.NET 9 SDK** - [Stáhnout](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Stáhnout](https://git-scm.com/)
- **Ollama** (volitelné, pro lokální AI) - [Stáhnout](https://ollama.com/)
- **Bailian API klíč** (volitelné, pro cloudovou AI) - [Získat](https://bailian.console.aliyun.com/)

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

Upravte `src/SiliconLife.Default/Config/DefaultConfigData.cs` nebo změňte konfiguraci za běhu prostřednictvím Web UI.

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
      "ApiKey": "vas-api-klic-zde",
      "Model": "qwen-plus",
      "Region": "cn-hangzhou"
    }
  }
}
```

### 4. Spuštění aplikace

```bash
cd src/SiliconLife.Default
dotnet run
```

Webový server se spustí na adrese `http://localhost:8080`

### 5. Přístup k Web UI

Otevřete prohlížeč a přejděte na:

```
http://localhost:8080
```

Uvidíte dashboard s:
- Správou křemíkových bytostí
- Rozhraním chatu
- Panelem konfigurace
- Monitorováním systému

## Vaše první křemíková bytost

### Vytvoření vaší první bytosti

1. Přejděte na **Správa bytostí** ve Web UI
2. Klikněte na **Vytvořit novou bytost**
3. Nakonfigurujte soubor duše (`soul.md`) s osobností a chováním
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

## Často kladené otázky

### Ollama Connection Refused

**Problém**: Nelze se připojit k Ollama na `http://localhost:11434`

**Řešení**:
```bash
# Zkontrolujte, zda Ollama běží
ollama list

# Pokud potřebujete spustit Ollama
ollama serve
```

### Model Not Found

**Problém**: `model "qwen2.5:7b" not found`

**Řešení**:
```bash
# Stáhněte požadovaný model
ollama pull qwen2.5:7b
```

### Port Already in Use

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

- 📚 Přečtěte si [Průvodce architekturou](architecture.md) pro pochopení návrhu systému
- 🛠️ Prozkoumejte [Vývojový průvodce](development-guide.md) pro rozšíření systému
- 📖 Prozkoumejte [API Reference](api-reference.md) pro detaily integrace
- 🔒 Podívejte se na [Dokumentaci bezpečnosti](security.md) pro systém oprávnění

## Struktura projektu

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/      # Core rozhraní a abstraktní třídy
│   └── SiliconLife.Default/   # Výchozí implementace + vstupní bod
├── docs/                      # Dokumentace (vícejazyčná)
│   ├── en/                    # Angličtina
│   ├── zh-CN/                 # Zjednodušená čínština
│   ├── zh-HK/                 # Tradiční čínština
│   ├── ja-JP/                 # Japonština
│   ├── ko-KR/                 # Korejština
│   └── cs-CZ/                 # Čeština
└── README.md                  # Tento soubor
```

## Potřebujete pomoc?

- 📖 Podívejte se na [Dokumentaci](docs/)
- 🐛 Nahlaste problémy na GitHubu
- 💬 Připojte se ke komunitní diskusi
