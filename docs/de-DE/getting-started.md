# Schnellstart

[English](../en/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | [Čeština](../cs-CZ/getting-started.md)

## Voraussetzungen

- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Download](https://git-scm.com/)
- **Ollama** (optional, für lokale KI) - [Download](https://ollama.com/)
- **DashScope API-Schlüssel** (optional, für Cloud-KI) - [Beantragen](https://bailian.console.aliyun.com/)

## Schnellstart

### 1. Repository klonen

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. Projekt bauen

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

#### Option B: DashScope (Cloud)

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "ihr-api-key-hier",
      "Model": "qwen-plus",
      "Region": "cn-hangzhou"
    }
  }
}
```

### 4. Anwendung ausführen

```bash
cd src/SiliconLife.Default
dotnet run
```

Der Webserver startet unter `http://localhost:8080`

### 5. Web-UI zugreifen

Öffnen Sie einen Browser und navigieren Sie zu:

```
http://localhost:8080
```

Sie werden ein Dashboard sehen mit:
- Silicon Being-Verwaltung
- Chat-Oberfläche
- Konfigurationspanel
- Systemüberwachung

## Ihr erstes Silicon Being

### Erstellen Sie Ihr erstes Being

1. In der Web-UI zu **Being-Verwaltung** navigieren
2. Auf **Neues Being erstellen** klicken
3. Soul-Datei (`soul.md`) mit Persönlichkeit und Verhalten konfigurieren
4. Being starten

### Beispiel soul.md

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

## Häufig gestellte Fragen

### Ollama-Verbindung abgelehnt

**Problem**: Keine Verbindung zu Ollama unter `http://localhost:11434` möglich

**Lösung**:
```bash
# Prüfen ob Ollama läuft
ollama list

# Ollama starten falls nötig
ollama serve
```

### Modell nicht gefunden

**Problem**: `model "qwen2.5:7b" not found`

**Lösung**:
```bash
# Erforderliches Modell herunterladen
ollama pull qwen2.5:7b
```

### Port bereits belegt

**Problem**: `HttpListenerException: Address already in use`

**Lösung**:
- Port in der Konfiguration ändern
- Oder Prozess auf Port 8080 beenden:

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## Nächste Schritte

- 📚 [Architekturleitfaden](architecture.md) lesen, um das Systemdesign zu verstehen
- 🛠️ [Entwicklungsleitfaden](development-guide.md) ansehen, um das System zu erweitern
- 📖 [API-Referenz](api-reference.md) für Integrationsdetails erkunden
- 🔒 [Sicherheitsdokumentation](security.md) für das Berechtigungssystem prüfen
- 🧰 [Tool-Referenz](tools-reference.md) für alle integrierten Tools durchsehen
- 🌐 [Web-UI-Leitfaden](web-ui-guide.md) für Oberflächenfunktionen lesen

## Projektstruktur

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/      # Kernschnittstellen und abstrakte Klassen
│   └── SiliconLife.Default/   # Standardimplementierung + Einstiegspunkt
├── docs/                      # Dokumentation (mehrsprachig, 21 Sprachvarianten)
│   ├── en/                    # Englisch
│   ├── de-DE/                 # Deutsch
│   ├── zh-CN/                 # Vereinfachtes Chinesisch
│   ├── zh-HK/                 # Traditionelles Chinesisch
│   ├── es-ES/                 # Spanisch
│   ├── ja-JP/                 # Japanisch
│   ├── ko-KR/                 # Koreanisch
│   └── cs-CZ/                 # Tschechisch
├── 总文档/                     # Anforderungs- und Architekturdokumente (Chinesisch)
└── README.md                  # Projektübersicht
```

## Hilfe benötigt?

- 📖 [Hilfedokumentationssystem](web-ui-guide.md#帮助文档系统新增) prüfen (mehrsprachige Unterstützung)
- 📚 [Vollständige Dokumentation](docs/) lesen
- 🐛 Probleme auf [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues) melden
- 💬 An Community-Diskussionen teilnehmen
