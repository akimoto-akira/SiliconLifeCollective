# Schnellstart

> **Version: v0.2.0-alpha**

[English](../en/getting-started.md) | **Deutsch** | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | [Čeština](../cs-CZ/getting-started.md) | [Русский](../ru-RU/getting-started.md)

## Version auswählen

Dieses Projekt bietet zwei Implementierungsversionen:

### SiliconLife.Default (Standardversion)
- **Positionierung**: Standardimplementierung, hauptsächlich zur Validierung der Architektur
- **Ausführungsmodus**: Konsolenanwendung
- **Speicherart**: Dateisystem-JSON-Speicherung
- **Einsatzszenarien**: Datensicherheit hat Priorität, kleine Datenmengen, Entwicklung und Debugging, Architekturvalidierung
- **Plattformunterstützung**: Windows, Linux, macOS
- **Rollenbeschreibung**: Dient als Referenzimplementierung zur Architekturvalidierung und bietet eine einfache, zuverlässige Ausführung, ideal für den ersten Kontakt mit dem Projekt oder für die Entwicklung und das Debugging

### SiliconLife.Fast (Hochleistungsversion)
- **Positionierung**: Empfohlene Produktivversion
- **Ausführungsmodus**: Desktop-Anwendung (Windows/macOS System-Tray / Linux Statusfenster)
- **Speicherart**: SpeedyPack-In-Memory-Speicherung + asynchrone Persistierung (.spk-Dateiformat)
- **Einsatzszenarien**: Hohe Nebenläufigkeit, geringe Latenz, große Datenmengen, langfristiger Produktivbetrieb
- **Plattformunterstützung**: Windows/macOS (volle Funktionalität inkl. System-Tray), Linux (Statusfenster, kein Tray-Icon)
- **Rollenbeschreibung**: Eine tiefgreifend optimierte produktionsreife Implementierung, die erste Wahl für den Langzeitbetrieb und echte Produktivumgebungen

> **Empfehlung für Einsteiger**: Beim ersten Mal wird empfohlen, mit **SiliconLife.Default** zu beginnen, um die Architektur schnell zu validieren. Nach Vertrautheit mit dem System wird dringend empfohlen, auf **SiliconLife.Fast** als Produktivversion zu migrieren.

## Voraussetzungen

- **.NET 9 SDK** - [Herunterladen](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Herunterladen](https://git-scm.com/)
- **Ollama** (optional, für lokale KI) - [Herunterladen](https://ollama.com/)
- **Bailian API-Schlüssel** (optional, für Cloud-KI) - [Beantragen](https://bailian.console.aliyun.com/)
- **Volcengine Ark API-Schlüssel** (optional, für Cloud-KI) - [Beantragen](https://console.volcengine.com/ark)
- **Herdsman** (optional, lokale/Cloud-Inferenz-Engine) — Keine Authentifizierung, kompatibel mit OpenAI-API-Format
- **Meituan LongCat API-Schlüssel** (optional, für Cloud-KI) — API-Schlüssel-Authentifizierung
- **Qiniu Cloud AI API-Schlüssel** (optional, für Cloud-KI) — API-Schlüssel-Authentifizierung
- **DeepSeek API-Schlüssel** (optional, für Cloud-KI) — [Beantragen](https://platform.deepseek.com/)
- **Zhipu AI API-Schlüssel** (optional, für Cloud-KI) — [Beantragen](https://open.bigmodel.cn/)
- **Baidu Qianfan API-Schlüssel** (optional, für Cloud-KI) — [Beantragen](https://qianfan.baidubce.com/)
- **Tencent Hunyuan API-Schlüssel** (optional, für Cloud-KI) — [Beantragen](https://console.cloud.tencent.com/hunyuan)
- **MiniMax API-Schlüssel** (optional, für Cloud-KI) — [Beantragen](https://platform.minimaxi.com/)
- **Moonshot (Kimi) API-Schlüssel** (optional, für Cloud-KI) — [Beantragen](https://platform.moonshot.cn/)
- **SiliconFlow API-Schlüssel** (optional, für Cloud-KI) — [Beantragen](https://cloud.siliconflow.cn/)

## Schnellstart

### 1. Repository klonen

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. Projekt erstellen

```bash
dotnet build
```

### 3. KI-Backend konfigurieren

Bearbeiten Sie `src/SiliconLife.Default/Config/DefaultConfigData.cs` oder ändern Sie die Konfiguration zur Laufzeit über die Web-UI.

#### Option A: Ollama (lokal)

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

#### Option B: Bailian (Cloud)

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

> **Verfügbare Regionen**: `beijing` (Peking), `virginia` (Virginia), `singapore` (Singapur), `hongkong` (Hongkong), `frankfurt` (Frankfurt)

#### Option C: Volcengine Ark (Cloud)

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

> **Hinweis**: Der Model-Parameter von Volcengine Ark akzeptiert eine Inferenz-Endpunkt-ID (z. B. `ep-20241212123456-abcde`) und keinen Modellnamen.

#### Option D: Herdsman (Lokal/Cloud)

```json
{
  "AIClients": {
    "Herdsman": {
      "Endpoint": "http://localhost:8000",
      "Model": "modell-name"
    }
  }
}
```

> **Funktionen**: Keine Authentifizierung, kompatibel mit OpenAI-API-Format, unterstützt Werkzeugaufrufe und Reasoning-Inhalte.

#### Option E: Meituan LongCat (Cloud)

```json
{
  "AIClients": {
    "LongCat": {
      "ApiKey": "api-schlüssel",
      "Endpoint": "https://api.longcat.ai/v1/chat/completions",
      "Model": "modell-name"
    }
  }
}
```

#### Option F: Qiniu Cloud AI (Cloud)

```json
{
  "AIClients": {
    "QiniuAI": {
      "ApiKey": "api-schlüssel",
      "Endpoint": "https://api.qiniu.com/v1/chat/completions",
      "Model": "modell-name"
    }
  }
}
```

#### Option G: DeepSeek (Cloud)

```json
{
  "AIClients": {
    "DeepSeek": {
      "ApiKey": "api-schlüssel",
      "Endpoint": "https://api.deepseek.com",
      "Model": "deepseek-v4-flash"
    }
  }
}
```

> **Funktionen**: Thinking-Modus (reasoning_content), konfigurierbare Schlussfolgerungsstärke, bis zu 1M Tokens Kontext (deepseek-v4-Serie).

#### Option H: Zhipu AI / GLM (Cloud)

```json
{
  "AIClients": {
    "Zhipu": {
      "ApiKey": "api-schlüssel",
      "Endpoint": "https://open.bigmodel.cn/api/paas/v4",
      "Model": "glm-4-flash"
    }
  }
}
```

> **Funktionen**: Thinking-Modus (GLM-5-Serie), Vision (modellabhängig), kostenloses Modell `glm-4-flash`, bis zu 1M Tokens Kontext.

#### Option I: Baidu Qianfan / ERNIE (Cloud)

```json
{
  "AIClients": {
    "Ernie": {
      "ApiKey": "api-schlüssel",
      "Endpoint": "https://qianfan.baidubce.com/v2",
      "Model": "ernie-5.1"
    }
  }
}
```

> **Funktionen**: Qianfan v2 OpenAI-kompatible API, Vision (ERNIE-5), kostenlose Modelle `ernie-speed`/`ernie-tiny`, bis zu 131K Tokens Kontext.

#### Option J: Tencent Hunyuan (Cloud)

```json
{
  "AIClients": {
    "Hunyuan": {
      "ApiKey": "api-schlüssel",
      "Endpoint": "https://tokenhub.tencentmaas.com/v1",
      "Model": "hy3"
    }
  }
}
```

> **Funktionen**: Dual-Endpunkte (TokenHub empfohlen + Legacy), Thinking-Modus (hy3, hy-2.0), Werkzeugaufrufe (modellabhängig), bis zu 262K Tokens Kontext.

#### Option K: MiniMax (Cloud)

```json
{
  "AIClients": {
    "MiniMax": {
      "ApiKey": "api-schlüssel",
      "Endpoint": "https://api.minimaxi.com/v1",
      "Model": "MiniMax-M3"
    }
  }
}
```

> **Funktionen**: Thinking-Modus mit reasoning_split (adaptiv bei M3), Vision (M3: Bild + Video), bis zu 1M Tokens Kontext.

#### Option L: Moonshot / Kimi (Cloud)

```json
{
  "AIClients": {
    "Moonshot": {
      "ApiKey": "api-schlüssel",
      "Endpoint": "https://api.moonshot.cn/v1",
      "Model": "kimi-k2.6"
    }
  }
}
```

> **Funktionen**: Thinking-Modus (kimi-k2.7: enabled+keep=all), Vision (kimi-k2.5+), bis zu 262K Tokens Kontext.

#### Option M: SiliconFlow (Cloud, Modell-Aggregator)

```json
{
  "AIClients": {
    "SiliconFlow": {
      "ApiKey": "api-schlüssel",
      "Endpoint": "https://api.siliconflow.cn/v1",
      "Model": "deepseek-ai/DeepSeek-V3.2"
    }
  }
}
```

> **Funktionen**: Aggregiert 100+ Open-Source-Modelle von mehreren Anbietern, dynamische Modellliste, Reasoning-Inhalte, Vision (modellabhängig), bis zu 1M Tokens Kontext.

### 4. Anwendung ausführen

#### Default-Version ausführen

```bash
cd src/SiliconLife.Default
dotnet run
```

Der Webserver wird unter `http://localhost:8080` gestartet

#### Fast-Version ausführen

```bash
cd src/SiliconLife.Fast
dotnet run
```

**Windows/macOS**: Die Anwendung startet im Fenstermodus und minimiert sich in den System-Tray. Der Webserver wird ebenfalls unter `http://localhost:8080` gestartet.

**Linux**: Die Anwendung zeigt ein Statusfenster an (kein System-Tray-Icon) und öffnet automatisch den Browser für den Zugriff auf die Web-UI. Der Parameter `--no-tray` verhindert das automatische Öffnen des Browsers:

```bash
dotnet run -- --no-tray
```

### 5. Auf die Web-UI zugreifen

Öffnen Sie einen Browser und navigieren Sie zu:

```
http://localhost:8080
```

Sie sehen ein Dashboard mit folgenden Inhalten:
- Silicon Being Verwaltung
- Chat-Oberfläche
- Konfigurationspanel
- Systemüberwachung

## Das erste Silicon Being

### Ihr erstes Wesen erstellen

1. Navigieren Sie in der Web-UI zur **Silicon Being Verwaltung**
2. Klicken Sie auf **Neues Wesen erstellen**
3. Konfigurieren Sie die Soul-Datei (`soul.md`) mit Persönlichkeit und Verhalten
4. Starten Sie das Wesen

### soul.md Beispiel

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

## Häufige Probleme

### Ollama-Verbindung verweigert

**Problem**: Verbindung zu Ollama unter `http://localhost:11434` nicht möglich

**Lösung**:
```bash
# Prüfen, ob Ollama ausgeführt wird
ollama list

# Ollama starten, falls erforderlich
ollama serve
```

### Modell nicht gefunden

**Problem**: `model "qwen2.5:7b" not found`

**Lösung**:
```bash
# Gewünschtes Modell herunterladen
ollama pull qwen2.5:7b
```

### Port bereits belegt

**Problem**: `HttpListenerException: Address already in use`

**Lösung**:
- Port in der Konfiguration ändern
- Oder den Prozess beenden, der Port 8080 verwendet:

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## Nächste Schritte

- 📚 Lesen Sie den [Architekturleitfaden](architecture.md), um das Systemdesign zu verstehen
- 🛠️ Sehen Sie den [Entwicklungsleitfaden](development-guide.md) zur Erweiterung des Systems
- 📖 Erkunden Sie die [API-Referenz](api-reference.md) für Integrationsdetails
- 🔒 Sehen Sie die [Sicherheitsdokumentation](security.md) zum Berechtigungssystem
- 🧰 Sehen Sie die [Werkzeugreferenz](tools-reference.md) für alle integrierten Werkzeuge
- 🌐 Sehen Sie den [Web-UI-Leitfaden](web-ui-guide.md) für die Oberflächenfunktionen

## Projektstruktur

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Core Host und abstrakte Klassen
│   ├── SiliconLife.Common/          # Gemeinsame Implementierung (von beiden Versionen genutzt)
│   ├── SiliconLife.App/             # Von Default und Fast gemeinsam genutzte Anwendungsschicht
│   ├── SiliconLife.Default/         # Standardimplementierung + Einstiegspunkt (Konsolenversion)
│   ├── SiliconLife.Fast/            # Hochleistungsimplementierung + Einstiegspunkt (Fensterversion)
│   ├── SiliconLife.Speedy/          # SpeedyPack-Speicher-Engine
│   └── SiliconLife.Speedy.Manager/  # SpeedyPack-Verwaltungswerkzeug (Avalonia UI)
├── docs/                            # Dokumentation (mehrsprachig, 34 Sprachvarianten)
│   ├── en/                          # Englisch
│   ├── zh-CN/                       # Vereinfachtes Chinesisch
│   ├── zh-HK/                       # Traditionelles Chinesisch
│   ├── es-ES/                       # Spanisch
│   ├── ja-JP/                       # Japanisch
│   ├── ko-KR/                       # Koreanisch
│   └── cs-CZ/                       # Tschechisch
├── docs/                           # Anforderungen und Architekturdokumentation
└── README.md                        # Projektbeschreibung
```

## Brauchen Sie Hilfe?

- 📖 Sehen Sie das [Hilfedokumentationssystem](web-ui-guide.md#hilfedokumentationssystem) (mit Mehrsprachunterstützung)
- 📚 Lesen Sie die [vollständige Dokumentation](docs/)
- 🐛 Melden Sie Probleme auf [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Beteiligen Sie sich an Community-Diskussionen
