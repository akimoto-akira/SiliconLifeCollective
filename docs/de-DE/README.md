# Silicon Life Collective

**Silicon Life Collective** — Eine auf .NET 9 basierende Multi-Agenten-Kollaborationsplattform, auf der KI-Agenten als **Silicon Beings** bezeichnet werden und sich durch Roslyn-Dynamikkompilierung selbst weiterentwickeln können.

[English](../README.md) | [Deutsch](../de-DE/README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Čeština](../cs-CZ/README.md)

## 🌟 Kernfunktionen

### Agentensystem
- **Multi-Agenten-Orchestrierung** — Zentrale Verwaltung durch den *Silicon Curator*, mit clock-gesteuertem Time-Slice-Fair-Scheduling-Mechanismus
- **Soul-Datei-gesteuert** — Jedes Silicon Being wird durch eine zentrale Prompt-Datei (`soul.md`) gesteuert, die einzigartige Persönlichkeit und Verhaltensmuster definiert
- **Body-Brain-Architektur** — *Body* (SiliconBeing) erhält Vitalzeichen und erkennt Triggerszenarien; *Brain* (ContextManager) ist verantwortlich für das Laden von Verlauf, KI-Aufruf, Tool-Ausführung und Persistierung von Antworten
- **Selbstentwicklungsfähigkeit** — Durch Roslyn-Dynamikkompilierungstechnologie können Silicon Beings ihren eigenen Code überschreiben, um Evolution zu实现

### Tools & Ausführung
- **23 integrierte Tools** — Abdeckend Kalender, Chat, Konfiguration, Festplatte, Netzwerk, Speicher, Aufgaben, Timer, Wissensdatenbank, Arbeitsnotizen, WebView-Browser usw.
- **Tool-Aufruf-Schleife** — KI gibt Tool-Aufruf zurück → Tool ausführen → Ergebnisse an KI zurückgeben → Schleife fortsetzen bis reine Textantwort
- **Executor-Berechtigungssicherheit** — Alle I/O-Operationen durchlaufen strenge Berechtigungsvalidierung über Executoren
  - 5-stufige Berechtigungskette: IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - Vollständige Audit-Protokollierung aller Berechtigungsentscheidungen

### KI & Wissen
- **Multiple KI-Backend-Unterstützung**
  - **Ollama** — Lokale Modellbereitstellung, mit nativer HTTP-API
  - **Alibaba Cloud DashScope (Bailian)** — Cloud-KI-Service, OpenAI-API-kompatibel, unterstützt 13+ Modelle, Multi-Region-Bereitstellung
- **32 Kalendersysteme** — Vollständige Abdeckung der wichtigsten globalen Kalender, einschließlich Gregorianischer Kalender, Chinesischer Mondkalender, Islamischer Kalender, Hebräischer Kalender, Japanischer Kalender, Persischer Kalender, Maya-Kalender, Chinesischer Historischer Kalender usw.
- **Wissensnetzwerksystem** — Wissensgraph basierend auf Triplen (Subjekt-Relation-Objekt), unterstützt Speicherung, Abfrage und Pfadentdeckung

### Web-Oberfläche
- **Moderne Web-UI** — Integrierter HTTP-Server mit SSE-Echtzeitaktualisierungen
- **4 Skin-Themes** — Admin-, Chat-, Creative-, Dev-Versionen, unterstützt automatische Erkennung und Umschaltung
- **20+ Controller** — Vollständige Systemverwaltung, Chat, Konfiguration, Überwachungsfunktionalität
- **Null Frontend-Framework-Abhängigkeit** — HTML/CSS/JS serverseitig generiert durch `H`, `CssBuilder` und `JsBuilder`

### Internationalisierung & Lokalisierung
- **Umfassende Unterstützung für 21 Sprachvarianten**
  - Chinesisch: zh-CN, zh-HK, zh-SG, zh-MO, zh-TW, zhMY (6 Varianten)
  - Englisch: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 Varianten)
  - Spanisch: es-ES, es-MX (2 Varianten)
  - Japanisch: ja-JP | Koreanisch: ko-KR | Tschechisch: cs-CZ

### Daten & Speicherung
- **Null Datenbankabhängigkeit** — Reines Dateisystem-Speicher (JSON-Format)
- **Zeitindex-Abfrage** — Effiziente Abfrage nach Zeitbereich über `ITimeStorage`-Schnittstelle
- **Minimale Abhängigkeiten** — Kernbibliothek abhängt nur von Microsoft.CodeAnalysis.CSharp für Dynamikkompilierung

## 🛠️ Technologie-Stack

| Komponente | Technologie |
|------|------|
| Runtime | .NET 9 |
| Programmiersprache | C# |
| KI-Integration | Ollama (lokal), Alibaba Cloud DashScope (Cloud) |
| Datenspeicherung | Dateisystem (JSON + Zeitindex-Verzeichnis) |
| Webserver | HttpListener (.NET integriert) |
| Dynamikkompilierung | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Browser-Automatisierung | Playwright (WebView) |
| Lizenz | Apache-2.0 |

## 📁 Projektstruktur

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Kernbibliothek (Schnittstellen, abstrakte Klassen)
│   │   ├── AI/                            # KI-Client-Schnittstellen, Context Manager, Nachrichtenmodelle
│   │   ├── Audit/                         # Token-Nutzungs-Audit-System
│   │   ├── Chat/                          # Chat-System, Sitzungsverwaltung, Broadcast-Kanäle
│   │   ├── Compilation/                   # Dynamikkompilierung, Sicherheits-Scanning, Code-Verschlüsselung
│   │   ├── Config/                        # Konfigurationsverwaltungssystem
│   │   ├── Executors/                     # Executoren (Festplatte, Netzwerk, Befehlszeile)
│   │   ├── IM/                            # Instant-Messaging-Provider-Schnittstellen
│   │   ├── Knowledge/                     # Wissensnetzwerksystem
│   │   ├── Localization/                  # Lokalisierungssystem
│   │   ├── Logging/                       # Protokollierungssystem
│   │   ├── Project/                       # Projektverwaltungssystem
│   │   ├── Runtime/                       # Hauptschleife, Clock-Objekte, Kern-Host
│   │   ├── Security/                      # Berechtigungsmanagementsystem
│   │   ├── SiliconBeing/                  # Silicon Being-Basisklasse, Manager, Factory
│   │   ├── Storage/                       # Speicher-Schnittstellen
│   │   ├── Time/                          # Unvollständige Datumsangaben (Zeitbereichsabfragen)
│   │   ├── Tools/                         # Tool-Schnittstellen und Tool-Manager
│   │   ├── WebView/                       # WebView-Browser-Schnittstellen
│   │   └── ServiceLocator.cs              # Globaler Service-Locator
│   │
│   └── SiliconLife.Default/               # Standardimplementierung + Anwendungseinstieg
│       ├── Program.cs                     # Einstiegspunkt (Alle Komponenten assemblieren)
│       ├── AI/                            # Ollama-Client, DashScope-Client
│       ├── Calendar/                      # 32 Kalenderimplementierungen
│       ├── Config/                        # Standard-Konfigurationsdaten
│       ├── Executors/                     # Standard-Executor-Implementierungen
│       ├── Help/                          # Hilfedokumentationssystem
│       ├── IM/                            # WebUI-Provider
│       ├── Knowledge/                     # Wissensnetzwerk-Implementierung
│       ├── Localization/                  # 21 Sprachlokalisierungen
│       ├── Logging/                       # Logging-Provider-Implementierungen
│       ├── Project/                       # Projektssystem-Implementierung
│       ├── Runtime/                       # Test-Clock-Objekte
│       ├── Security/                      # Standard-Berechtigungs-Callbacks
│       ├── SiliconBeing/                  # Standard-Silicon-Being-Implementierung
│       ├── Storage/                       # Dateisystem-Speicherimplementierung
│       ├── Tools/                         # 23 integrierte Tool-Implementierungen
│       ├── WebView/                       # Playwright-WebView-Implementierung
│       └── Web/                           # Web-UI-Implementierung
│           ├── Controllers/               # 20+ Controller
│           ├── Models/                    # View-Modelle
│           ├── Views/                     # HTML-Ansichten
│           └── Skins/                     # 4 Skin-Themes
│
├── docs/                                  # Mehrsprachige Dokumentation
│   ├── zh-CN/                             # Vereinfachtes Chinesisch
│   ├── de-DE/                             # Deutsch
│   ├── en/                                # Englisch
│   └── ...                                # Andere Sprachen
│
└── 总文档/                                 # Anforderungs- und Architekturdokumente
    ├── 需求文档.md
    ├── 架构大纲.md
    └── 实现顺序.md
```

## 🏗️ Architekturübersicht

### Scheduling-Architektur
```
Hauptschleife (dedizierter Thread, Watchdog + Circuit Breaker)
  └── Clock-Objekt (nach Priorität sortiert)
       └── Silicon Being Manager
            └── Silicon Being Runner (temporärer Thread, Timeout + Circuit Breaker)
                 └── SiliconBeing.Tick()
                      └── ContextManager.Denken()
                           └── AI-Client.Chat()
                                └── Tool-Aufruf-Schleife → Persistierung im Chat-System
```

### Sicherheitsarchitektur
Alle von KI initiierten I/O-Operationen müssen eine strenge Sicherheitskette durchlaufen:

```
Tool-Aufruf → Executor → Berechtigungsmanager → [IsCurator → Frequenz-Cache → GlobalACL → Callback → Benutzer fragen]
```

## 🚀 Schnellstart

### Voraussetzungen

- **.NET 9 SDK** — [Download-Link](https://dotnet.microsoft.com/download/dotnet/9.0)
- **KI-Backend** (wählen Sie eines):
  - **Ollama**: [Ollama installieren](https://ollama.com) und Modell abrufen (z.B. `ollama pull llama3`)
  - **Alibaba Cloud DashScope**: API-Schlüssel von [DashScope-Konsole](https://bailian.console.aliyun.com/) erhalten

### Projekt bauen

```bash
dotnet restore
dotnet build
```

### System ausführen

```bash
dotnet run --project src/SiliconLife.Default
```

Die Anwendung startet den Webserver und öffnet automatisch die Web-UI im Browser.

### Einzeldatei veröffentlichen

```bash
# Windows
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 Entwicklungs-Roadmap

### ✅ Abgeschlossen
- [x] Phase 1: Console AI-Chat
- [x] Phase 2: Framework-Skelett (Hauptschleife + Clock-Objekt + Watchdog + Circuit Breaker)
- [x] Phase 3: Erstes Silicon Being mit Soul-Datei (Body-Brain-Architektur)
- [x] Phase 4: Persistenter Speicher (Chat-System + Time Storage-Schnittstelle)
- [x] Phase 5: Tool-System + Executoren
- [x] Phase 6: Berechtigungssystem (5-stufige Kette, Audit-Logger, GlobalACL)
- [x] Phase 7: Dynamikkompilierung + Selbstentwicklung (Roslyn)
- [x] Phase 8: Langzeitspeicher + Aufgaben + Timer
- [x] Phase 9: Kern-Host + Multi-Agenten-Kollaboration
- [x] Phase 10: Web-UI (HTTP + SSE, 20+ Controller, 4 Skins)
- [x] Phase 10.5: Inkrementelle Verbesserungen (Broadcast-Kanäle, Token-Audit, 32 Kalender, Tool-Verbesserungen, 21-Sprach-Lokalisierung)
- [x] Phase 10.6: Vervollständigung & Optimierung (WebView, Hilfesystem, Projekt-Workspace, Wissensnetzwerk)

### 🚧 Geplant
- [ ] Phase 11: Externe IM-Integration (Feishu / WhatsApp / Telegram)
- [ ] Phase 12: Plugin-System und Skill-Ökosystem

## 📚 Dokumentation

- [Architekturdesign](architecture.md) — Systemdesign, Scheduling-Mechanismen, Komponentearchitektur
- [Sicherheitsmodell](security.md) — Berechtigungsmodell, Executoren, Dynamikkompilierungssicherheit
- [Entwicklungsleitfaden](development-guide.md) — Tool-Entwicklung, Erweiterungsleitfaden
- [API-Referenz](api-reference.md) — Web API-Endpunktdokumentation
- [Tool-Referenz](tools-reference.md) — Detaillierte Beschreibung integrierter Tools
- [Web-UI-Leitfaden](web-ui-guide.md) — Web-Oberflächen-Benutzerleitfaden
- [Silicon Being-Leitfaden](silicon-being-guide.md) — Agentenentwicklungsleitfaden
- [Berechtigungssystem](permission-system.md) — Berechtigungsmanagement im Detail
- [Kalendersystem](calendar-system.md) — 32 Kalendersysteme Beschreibung
- [Schnellstart](getting-started.md) — Detaillierter Einsteigerleitfaden
- [Fehlerbehebung](troubleshooting.md) — Häufig gestellte Fragen
- [Roadmap](roadmap.md) — Vollständiger Entwicklungsplan
- [Änderungsprotokoll](changelog.md) — Versionsupdateverlauf
- [Beitragsleitfaden](contributing.md) — Wie am Projekt teilnehmen

## 🤝 Mitwirken

Wir begrüßen Beiträge aller Art! Für Details bitte den [Beitragsleitfaden](contributing.md) lesen.

### Entwicklungs-Workflow
1. Dieses Repository forken
2. Feature-Branch erstellen (`git checkout -b feature/AmazingFeature`)
3. Änderungen committen (`git commit -m 'feat: add some AmazingFeature'`)
4. Zum Branch pushen (`git push origin feature/AmazingFeature`)
5. Pull Request einreichen

## 📄 Lizenz

Dieses Projekt ist unter der Apache License 2.0 lizenziert — siehe [LICENSE](../../LICENSE)-Datei.

## 👨‍💻 Autor

**Hoshino Kennji**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- Gitee: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili: [617827040](https://space.bilibili.com/617827040)

## 🙏 Danksagung

Danke an alle Entwickler und KI-Plattformanbieter, die zu diesem Projekt beigetragen haben.

---

**Silicon Life Collective** — KI-Agenten wirklich "lebendig" machen
