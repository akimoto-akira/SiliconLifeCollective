![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**Version: v0.1.0-alpha** | **Silicon Life Collective** — Eine auf .NET 9 basierende Multi-Agenten-Kollaborationsplattform, auf der KI-Agenten als **Silicon Beings** bezeichnet werden und sich durch Roslyn-Dynamikkompilierung selbst weiterentwickeln können.

[English](../README.md) | **Deutsch** | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Français](../fr-FR/README.md) | [Čeština](../cs-CZ/README.md) | [Italiano](../it-IT/README.md) | [Polski](../pl-PL/README.md)

## 🌟 Kernfunktionen

### Agentensystem
- **Multi-Agenten-Orchestrierung** — Zentrale Verwaltung durch den *Silicon Curator*, mit clock-gesteuertem Time-Slice-Fair-Scheduling-Mechanismus
- **Soul-Datei-gesteuert** — Jedes Silicon Being wird durch eine zentrale Prompt-Datei (`soul.md`) gesteuert, die einzigartige Persönlichkeit und Verhaltensmuster definiert
- **Body-Brain-Architektur** — *Body* (SiliconBeing) erhält Vitalzeichen und erkennt Triggerszenarien; *Brain* (ContextManager) ist verantwortlich für das Laden von Verlauf, KI-Aufruf, Tool-Ausführung und Persistierung von Antworten
- **Selbstentwicklungsfähigkeit** — Durch Roslyn-Dynamikkompilierungstechnologie können Silicon Beings ihren eigenen Code überschreiben, um Evolution zu realisieren
- **Aktivitätszustandsverwaltung** — Unterstützt vier Aktivitätszustände: Idle (inaktiv), Working (arbeitend), Error (Fehler), Stopped (gestoppt). Automatischer Wechsel zu Stopped nach 10 aufeinanderfolgenden Fehlern

### Plugin-System
- **Plugin-Erweiterungsarchitektur** — Funktionserweiterung durch IPlugin-Schnittstelle, unterstützt dynamisches Laden von Plugin-DLLs aus Verzeichnis
- **Sicherer Sandkasten** — Plugin-Loader führt strenge Sicherheits-Scans durch, verbietet Zugriff auf System.IO, System.Net und andere Namespaces
- **Isoliertes Laden** — Verwendung von benutzerdefiniertem AssemblyLoadContext für isoliertes Laden, verhindert, dass Plugins die Hauptprogrammstabilität beeinträchtigen
- **Tool-Integration** — Plugins können über die ITool-Schnittstelle benutzerdefinierte Tools registrieren, die automatisch in den Tool-Aufruf-Zyklus integriert werden

### Tools & Ausführung
- **24 integrierte Tools** — Abdeckend Kalender, Chat, Konfiguration, Festplatte, Netzwerk, Speicher, Aufgaben, Timer, Wissensdatenbank, Arbeitsnotizen, WebView-Browser, Hot-Reload usw.
- **Hot-Reload-Tool** — Unterstützt automatische Kompilierung, Dateiaktualisierung und Neustart von SiliconLife.Fast während der Laufzeit, ohne manuelles Eingreifen
- **Tool-Aufruf-Schleife** — KI gibt Tool-Aufruf zurück → Tool ausführen → Ergebnisse an KI zurückgeben → Schleife fortsetzen bis reine Textantwort
- **Executor-Berechtigungssicherheit** — Alle I/O-Operationen durchlaufen strenge Berechtigungsvalidierung über Executoren
  - 5-stufige Berechtigungskette: IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - Vollständige Audit-Protokollierung aller Berechtigungsentscheidungen

### KI & Wissen
- **Multiple KI-Backend-Unterstützung**
  - **Ollama** — Lokale Modellbereitstellung, mit nativer HTTP-API
  - **Alibaba Cloud DashScope (Bailian)** — Cloud-KI-Service, OpenAI-API-kompatibel, unterstützt 13+ Modelle, Multi-Region-Bereitstellung
  - **Volcengine Ark** — ByteDance Cloud-KI-Service, unterstützt Streaming- und Nicht-Streaming-Modi, integrierte doppelte Ratenbegrenzung
- **32 Kalendersysteme** — Vollständige Abdeckung der wichtigsten globalen Kalender, einschließlich Gregorianischer Kalender, Chinesischer Mondkalender, Islamischer Kalender, Hebräischer Kalender, Japanischer Kalender, Persischer Kalender, Maya-Kalender, Chinesischer Historischer Kalender usw.
- **Wissensnetzwerksystem** — Wissensgraph basierend auf Triplen (Subjekt-Relation-Objekt), unterstützt Speicherung, Abfrage und Pfadentdeckung

### Web-Oberfläche
- **Moderne Web-UI** — Integrierter HTTP-Server mit SSE-Echtzeitaktualisierungen
- **7 Skin-Themes** — Admin-, Chat-, Creative-, Dev-, High-Contrast-, Light-, Minimal-Versionen, unterstützt automatische Erkennung und Umschaltung
- **22 Controller** — Vollständige Systemverwaltung, Chat, Konfiguration, Überwachungsfunktionalität
- **Null Frontend-Framework-Abhängigkeit** — HTML/CSS/JS serverseitig generiert durch `H`, `CssBuilder` und `JsBuilder`

### Internationalisierung & Lokalisierung
- **Umfassende Unterstützung für 29 Sprachimplementierungen**, abdeckend 2 Schriftsysteme und mehrere regionale Varianten
  - **Chinesisch (Vereinfacht)**: zh-CN (China Festland), zh-SG (Singapur), zh-MY (Malaysia) (3 Varianten)
  - **Chinesisch (Traditionell)**: zh-HK (Hongkong), zh-TW (Taiwan), zh-MO (Macao) (3 Varianten)
  - **Englisch**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 Varianten)
  - **Spanisch**: es-ES, es-MX (2 Varianten)
  - **Deutsch**: de-DE, de-AT, de-CH, de-LU, de-LI (5 Varianten)
  - **Französisch**: fr-FR, fr-CA, fr-CH (3 Varianten)
  - **Japanisch**: ja-JP | **Koreanisch**: ko-KR | **Tschechisch**: cs-CZ (3 Varianten)

### Daten & Speicherung
- **SpeedyPack Hochleistungsspeicher** — Fast-Version verwendet eigenentwickelte .spk-Speicher-Engine, In-Memory-Verzeichniszuordnung + Eintrags-Cache + asynchrone Schreibwarteschlange
- **Dateisystem-Speicher** — Default-Version verwendet reinen Dateisystem-JSON-Speicher
- **Zeitindex-Abfrage** — Effiziente Abfrage nach Zeitbereich über `ITimeStorage`-Schnittstelle
- **Automatische Komprimierung** — SpeedyPack unterstützt zeitgesteuerte automatische Komprimierung zur Rückgewinnung von freiem Speicherplatz
- **Minimale Abhängigkeiten** — Kernbibliothek abhängt nur von Microsoft.CodeAnalysis.CSharp für Dynamikkompilierung

## 🔄 Duale Versionsarchitektur

Dieses Projekt bietet zwei Implementierungsversionen, um unterschiedliche Szenarioanforderungen zu erfüllen:

### SiliconLife.Default (Standardversion)
- **Positionierung**: Standardimplementierung, hauptsächlich für Architektur-Machbarkeitsverifizierung
- **Ausführungsmodus**: Konsolenanwendung
- **Speichermethode**: Reines Dateisystem-JSON-Speicher
- **Anwendbare Szenarien**: Hohe Datensicherheitsanforderungen, begrenzte Speicherressourcen, kleines Datenvolumen
- **Merkmale**: Einfach und zuverlässig, sofortige Datenpersistenz, kein Speicherverlustrisiko
- **Rollenbeschreibung**: Referenzimplementierung für Architekturverifizierung, geeignet für ersten Kontakt, Entwicklungs-Debugging oder Szenarien mit Datensicherheitspriorität
- **Startbefehl**: `dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast (Hochleistungsversion)
- **Positionierung**: Haupt-Produktionsversion
- **Ausführungsmodus**: Windows Forms-Anwendung (unterstützt Systemtray)
- **Speichermethode**: SpeedyPack-In-Memory-Speicher + asynchrone Batch-Persistenz (.spk-Dateiformat)
- **Anwendbare Szenarien**: Hohe Parallelität, niedrige Latenz, große Datenmengen
- **Merkmale**:
  - Extreme Performance-Optimierung
  - Tray-Hintergrundausführung mit Echtzeitüberwachung über Tray-Statusfenster
  - SpeedyPack-Engine + automatische Komprimierung gewährleisten Datensicherheit
  - Component-UI-Architektur, 30+ deklarative Komponenten
  - 7 Skin-Themes, unterstützt automatische Erkennung und Umschaltung
  - Hot-Reload-Tool für Online-Updates und Neustarts
- **Performance-Verbesserung**: Speicherlese-Latenz um 1000x reduziert, Schreiblatenz um 15000x reduziert, parallele Verarbeitungskapazität um 50x erhöht
- **Rollenbeschreibung**: Produktionsreife Implementierung mit tiefer Optimierung, die beste Wahl für Langzeitbetrieb und echte Produktionsumgebungen
- **Startbefehl**: `dotnet run --project src/SiliconLife.Fast`

### Versionsvergleich

| Merkmal | SiliconLife.Default | SiliconLife.Fast |
|---------|---------------------|------------------|
| **Ausführungsmodus** | Konsolenanwendung | Forms-Anwendung (Systemtray) |
| **Benutzeroberfläche** | Web UI (Browser-Zugriff) | Tray-Symbol + Tray-Fenster + Web UI |
| **Systemtray** | ❌ Keine | ✅ Unterstützt Minimieren ins Tray |
| **Hintergrundausführung** | ❌ Beendet beim Schließen der Konsole | ✅ Kontinuierliche Tray-Hintergrundausführung |
| **Speichermethode** | Dateisystem-JSON-Speicher | SpeedyPack-In-Memory-Speicher + asynchrone Persistenz |
| **Speicher-Engine** | Dateisystem-I/O | SiliconLife.Speedy (.spk-Format) |
| **Leselatenz** | ~10ms (Festplatten-I/O) | ~0.01ms (Speicheroperation) |
| **Schreiblatenz** | ~15ms (synchrones Schreiben) | ~0.001ms (asynchrones Schreiben) |
| **Parallelität** | ~100 req/s | ~5000 req/s |
| **Speichernutzung** | ~200MB | ~500MB |
| **Datensicherheit** | Extrem hoch (sofortige Persistenz) | Hoch (asynchrone Persistenz + automatische Komprimierung) |
| **Anwendbare Szenarien** | Datensicherheit zuerst, kleine Daten | Performance zuerst, große Daten, hohe Parallelität |

## 🛠️ Technologie-Stack

| Komponente | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| Runtime | .NET 9 | .NET 9 Windows |
| Programmiersprache | C# | C# |
| Anwendungstyp | Konsolenanwendung | Windows Forms-Anwendung |
| KI-Integration | Ollama (lokal), Alibaba Cloud DashScope (Cloud) | Ollama (lokal), Alibaba Cloud DashScope (Cloud), Volcengine Ark (Cloud) |
| Datenspeicherung | Dateisystem (JSON + Zeitindex-Verzeichnis) | SpeedyPack (.spk-Format, In-Memory-Zuordnung + asynchrone Persistenz) |
| Webserver | HttpListener (.NET integriert) | HttpListener (.NET integriert) |
| Dynamikkompilierung | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Browser-Automatisierung | Playwright (WebView) | Playwright (WebView) |
| Plugin-System | ✅ Unterstützt (IPlugin + PluginLoader) | ✅ Unterstützt (IPlugin + PluginLoader) |
| Systemtray | ❌ Nicht unterstützt | ✅ Unterstützt (NotifyIcon) |
| Lizenz | Apache-2.0 | Apache-2.0 |

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
│   │   ├── Plugins/                       # Plugin-System (IPlugin-Schnittstelle, PluginLoader)
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
│   ├── SiliconLife.Common/                # Gemeinsame Implementierung (beide Versionen)
│   │   ├── AI/                            # KI-Client-Factory (Ollama, DashScope, VolcengineArk)
│   │   ├── Calendar/                      # 32 Kalenderimplementierungen
│   │   ├── Localization/                  # Lokalisierung-Basisklasse mit 29 Sprach-/Regionvarianten
│   │   ├── Resources/                     # Gemeinsame Ressourcendateien
│   │   ├── Security/                      # Berechtigungsmanager
│   │   ├── SiliconBeing/                  # Standard-Silicon-Being-Implementierung
│   │   ├── Tools/                         # 23 gemeinsame Tool-Implementierungen (inkl. Hot-Reload-Tool)
│   │   ├── Web/                           # Web-Infrastruktur
│   │   └── WebView/                       # Playwright WebView-Implementierung
│   │
│   ├── SiliconLife.App/                   # Anwendungsschicht (Web UI + Hilfe, von Default und Fast gemeinsam genutzt)
│   │   ├── Config/                        # Anwendungskonfiguration
│   │   ├── Data/                          # Datenverzeichnis
│   │   ├── Help/                          # Hilfedokumentations-Lokalisierung (mehrsprachig)
│   │   └── Web/                           # Web-UI-Implementierung
│   │       ├── Component/                 # UI-Komponentenbibliothek (30+ Komponenten)
│   │       ├── Controllers/               # 22 Controller
│   │       ├── Models/                    # View-Modelle
│   │       ├── Views/                     # HTML-Ansichten
│   │       └── Skins/                     # 7 Skin-Themes
│   │
│   ├── SiliconLife.Default/               # Standardimplementierung + Anwendungseinstieg (Konsolenversion)
│   │   ├── Program.cs                     # Einstiegspunkt (Alle Komponenten assemblieren)
│   │   ├── Config/                        # Standard-Konfigurationsdaten
│   │   ├── IM/                            # WebUI-Provider
│   │   ├── Knowledge/                     # Wissensnetzwerk-Implementierung
│   │   ├── Logging/                       # Logging-Provider-Implementierungen
│   │   ├── Project/                       # Projektssystem-Implementierung
│   │   ├── Security/                      # Standard-Berechtigungs-Callbacks
│   │   ├── Storage/                       # Dateisystem-Speicherimplementierung
│   │   └── Tools/                         # Versionsspezifische Tool-Implementierungen (HelpTool)
│   │
│   ├── SiliconLife.Fast/                  # Hochleistungsimplementierung + Anwendungseinstieg (Forms-Version)
│   │   ├── Program.cs                     # Einstiegspunkt (Forms-Anwendung)
│   │   ├── Config/                        # Konfigurationsdaten (mit Default geteilt)
│   │   ├── IM/                            # WebUI-Provider
│   │   ├── Knowledge/                     # Wissensnetzwerk-Implementierung (Speicheroptimiert)
│   │   ├── Logging/                       # Hochleistungs-Logging-Provider
│   │   ├── Project/                       # Projektssystem-Implementierung
│   │   ├── Security/                      # Optimierte Berechtigungs-Callbacks
│   │   ├── Storage/                       # SpeedyPack-Speicheradapter
│   │   ├── Tools/                         # Versionsspezifische Tool-Implementierungen (HelpTool)
│   │   └── Tray/                          # Systemtray (29 Sprachlokalisierungen)
│   │
│   ├── SiliconLife.Speedy/                # SpeedyPack Hochleistungsspeicher-Engine
│   │   ├── SpeedyPack.cs                  # Kernklasse (In-Memory-Verzeichniszuordnung + Cache + asynchrones Schreiben)
│   │   ├── SpeedyPackOptions.cs           # Konfigurationsoptionen (Cache-TTL, max. Einträge usw.)
│   │   ├── IPackTransaction.cs            # Transaktionsschnittstelle
│   │   ├── SpkFileInfo.cs                 # Dateiinformationen
│   │   └── Internal/                      # Interne Implementierung
│   │       ├── DirectoryMap.cs            # In-Memory-Verzeichniszuordnung
│   │       ├── EntryCache.cs              # Eintrags-Cache
│   │       ├── FreeList.cs                # Freiraumverwaltung
│   │       ├── PackFileReader.cs          # Paketdatei-Leser
│   │       ├── PackFileWriter.cs          # Paketdatei-Schreiber
│   │       ├── WriteQueue.cs              # Asynchrone Schreibwarteschlange
│   │       ├── WriteOperation.cs          # Schreiboperation
│   │       ├── SpeedyTransaction.cs       # Transaktionsimplementierung
│   │       ├── SpkHeader.cs               # Paketdatei-Header
│   │       └── PathNormalizer.cs          # Pfadnormalisierung
│   │
│   └── SiliconLife.Speedy.Manager/        # SpeedyPack-Verwaltungstool (Windows Forms)
│       ├── MainForm.cs                    # Hauptformular
│       ├── Program.cs                     # Einstiegspunkt
│       └── slc.ico                        # Anwendungssymbol
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
  - **Volcengine Ark**: API-Schlüssel von [Volcengine-Konsole](https://console.volcengine.com/ark) erhalten

### Projekt bauen

```bash
dotnet restore
dotnet build
```

### System ausführen

#### Methode 1: Default-Version ausführen (Konsolenanwendung)

```bash
dotnet run --project src/SiliconLife.Default
```

Die Anwendung startet den Webserver und öffnet automatisch die Web-UI im Browser.

**Anwendbare Szenarien**:
- ✅ Äußerst hohe Datensicherheitsanforderungen
- ✅ Begrenzte Speicherressourcen (RAM < 2GB)
- ✅ Kleines Datenvolumen, kurzfristige Nutzung
- ✅ Entwicklungs- und Debugging-Phase

#### Methode 2: Fast-Version ausführen (Windows Forms-Anwendung)

```bash
dotnet run --project src/SiliconLife.Fast
```

Die Anwendung startet im Forms-Modus, minimiert sich in den Systemtray und läuft im Hintergrund weiter.

**Anwendbare Szenarien**:
- ✅ Hochparallele Szenarien (> 5 Benutzer)
- ✅ Große Datenmengen (Nutzung über 3 Monate)
- ✅ Niedrige Latenz-Anforderungen
- ✅ Tray-Hintergrundbetrieb erforderlich

### Einzeldatei veröffentlichen

```bash
# Windows - Default-Version
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Windows - Fast-Version
dotnet publish src/SiliconLife.Fast -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux - Nur Default-Version
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - Nur Default-Version
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
- [x] Phase 11: SpeedyPack-Speicher-Engine (Ersetzung von LiteDB, In-Memory-Zuordnung, asynchrone Schreibwarteschlange, automatische Komprimierung)
- [x] Phase 12: Plugin-System (IPlugin-Schnittstelle, PluginLoader-Sicherheits-Sandkasten, isoliertes Laden, Tool-Integration)

### 🚧 Geplant
- [ ] Phase 13: Externe IM-Integration (Feishu / WhatsApp / Telegram)
- [ ] Phase 14: Skill-Ökosystem (Plugin-Marktplatz, Skill-Paket-Verteilung)

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

## 💡 Versionsauswahl-Leitfaden

### Welche Version sollte ich verwenden?

**SiliconLife.Default (Standardimplementierung — Architektur-Machbarkeitsverifizierung):**
- 📌 Sie haben zum ersten Mal Kontakt mit diesem Projekt und möchten die Systemarchitektur schnell verstehen
- 📌 Sie befinden sich in der Entwicklungs- und Debugging-Phase und benötigen eine einfache, direkte Ausführungsmethode

**SiliconLife.Fast (Hochleistungsversion — Produktionseinsatz):**
- 📌 Sie benötigen Langzeitbetrieb und hohe Performance
- 📌 Sie haben große Datenmengen oder hohe Parallelitätsanforderungen
- 📌 Sie möchten die Anwendung im Systemtray im Hintergrund laufen lassen

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
