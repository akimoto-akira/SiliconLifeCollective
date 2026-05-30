![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**Version: v0.2.0-alpha** | **Silicon Life Collective** — Eine auf .NET 9 basierende Multi-Agenten-Kollaborationsplattform, deren KI-Agenten als **Silicon Beings** bezeichnet werden und durch Roslyn-Dynamische Kompilierung Selbstevolution realisieren.

[English](../README.md) | **Deutsch** | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Français](../fr-FR/README.md) | [Čeština](../cs-CZ/README.md) | [Italiano](../it-IT/README.md) | [Polski](../pl-PL/README.md) | [Português](../pt-PT/README.md) | [Русский](../ru-RU/README.md)

## 🌟 Kernfunktionen

### Agentensystem
- **Multi-Agenten-Orchestrierung** — Einheitlich verwaltet durch den *Silicon Curator*, mit taktgesteuertem Zeitschlitz-Fair-Scheduling
- **Soul-Datei-gesteuert** — Jedes Silicon Being wird durch eine Kern-Prompt-Datei (`soul.md`) gesteuert, die einzigartige Persönlichkeit und Verhaltensmuster definiert
- **Körper-Gehirn-Architektur** — Der *Körper* (SiliconBeing) erhält Vitalfunktionen und erkennt Triggerszenarien; das *Gehirn* (ContextManager) lädt Historie, ruft KI auf, führt Werkzeuge aus und persistiert Antworten
- **Selbstevolutionsfähigkeit** — Durch Roslyn-Dynamische Kompilierung können Silicon Beings ihren eigenen Code umschreiben und sich weiterentwickeln
- **Aktivitätszustandsverwaltung** — Unterstützt neun Aktivitätszustände: Idle (Leerlauf), SingleChat, GroupChat, Task, Timer, Broadcast, Project, MemoryCompression, Stopped (Angehalten); bei 10 aufeinanderfolgenden Fehlern automatischer Übergang in den Stopped-Zustand

### Plugin-System
- **Plugin-Erweiterungsarchitektur** — Funktionserweiterung über die IPlugin-Schnittstelle, dynamisches Laden von Plugin-DLLs aus Verzeichnissen
- **Plugin-Fähigkeitsdeklaration** — Plugins deklarieren erforderliche Fähigkeiten (Network, FileIO, Process, AI) über das `[PluginCapability]`-Attribut; der Lader lockert die Sicherheitsprüfregeln entsprechend; nicht deklarierbare Fähigkeiten (P/Invoke, Unsafe, Reflection Emit usw.) werden immer blockiert
- **Isoliertes Laden** — Isoliertes Laden über benutzerdefinierten AssemblyLoadContext, um zu verhindern, dass Plugins die Stabilität des Hauptprogramms beeinträchtigen
- **Werkzeugintegration** — Plugins können über die ITool-Schnittstelle benutzerdefinierte Werkzeuge registrieren, die automatisch in den Werkzeugaufruf-Zyklus integriert werden

### Werkzeuge und Ausführung
- **24 integrierte Werkzeuge** — Umfassen Kalender, Chat, Konfiguration, Festplatte, Netzwerk, Speicher, Aufgaben, Timer, Wissensnetzwerk, Arbeitsnotizen, Projektarbeitsbereich, WebView-Browser und mehr
- **Werkzeugszenario-Isolierung** — Jedes Werkzeug deklariert über die `ToolScenario`-Eigenschaft verfügbare Szenarien (Chat, Task, Timer, MemoryCompression, Project); die `ChatOnly`-Eigenschaft beschränkt Werkzeuge auf Chat-Szenarien
- **IAIClient-Fähigkeitsschnittstelle** — KI-Clients deklarieren Fähigkeiten für Streaming-Modus, Werkzeugaufrufe, Kontextfenster, Vision und Audio; der ContextManager passt sein Verhalten entsprechend an
- **Werkzeugaufruf-Zyklus** — KI gibt Werkzeugaufruf zurück → Werkzeug wird ausgeführt → Ergebnis wird an KI zurückgemeldet → Zyklus wird fortgesetzt, bis eine reine Textantwort zurückgegeben wird
- **Executor-Berechtigungssicherheit** — Alle I/O-Operationen durchlaufen über den Executor eine strenge Berechtigungsprüfung
  - 3-stufige Berechtigungsprüfungskette: Benutzerfrequenz-Cache → Berechtigungs-Callback-Schnittstelle → (IsCurator: Berechtigungsanfrage-Handler | Non-curator: Globale ACL → Standardablehnung)
  - Vollständiges Audit-Protokoll erfasst alle Berechtigungsentscheidungen

### KI und Wissen
- **Multi-KI-Backend-Unterstützung**
  - **Ollama** — Lokale Modellbereitstellung mit nativer HTTP-API
  - **Alibaba Cloud Bailian (DashScope)** — Cloud-KI-Service, kompatibel mit OpenAI-API, unterstützt 13+ Modelle, Multi-Region-Bereitstellung
  - **Volcengine Ark (VolcengineArk)** — ByteDance Cloud-KI-Service, unterstützt Streaming- und Non-Streaming-Modi, integrierte Ratensteuerung
  - **Herdsman** — Authentifizierungsfreie Inferenz-Engine, kompatibel mit OpenAI-API-Format
  - **Meituan LongCat** — Meituans eigenes Großmodell, kompatibel mit OpenAI-API-Format, API-Schlüssel-Authentifizierung
  - **Qiniu Cloud AI** — Qiniu Cloud-KI-Service, API-Schlüssel-Authentifizierung
- **32 Kalendersysteme** — Vollständige Abdeckung der wichtigsten weltweiten Kalendersysteme, einschließlich Gregorianisch, Chinesisch, Islamisch, Hebräisch, Japanisch, Persisch, Maya, historische chinesische Kalender usw.
- **Wissensnetzwerk-System** — Auf Tripeln (Subjekt-Relation-Objekt) basierendes Wissensnetzwerk mit Speicherung, Abfrage und Pfadfindung
- **Projektarbeitsbereich** — Projektraumverwaltung mit Projekterstellung/-archivierung/-zerstörung, Rollenzuweisung, Arbeitsnotizen, Aufgabenverfolgung und Werkzeugberechtigungsisolierung
- **Workflow-Engine** — Auf Vorlagen basierende Zustandsautomaten-Engine mit benutzerdefinierten Workflow-Vorlagen, Zustandsübergängen, Tick-gesteuerter Ausführung und Instanz-Lebenszyklusverwaltung
- **Gedächtnisverblassungsmechanismus** — Zeitgesteuerter Zerfallsdienst (MemoryFadeService), der stündlich automatisch die Wichtigkeit der Erinnerungen aller Silicon Beings abschwächt und automatisch archiviert

### Web-Oberfläche
- **Moderne Web-UI** — Integrierter HTTP-Server mit SSE-Echtzeitaktualisierungen
- **7 Skin-Themes** — Verwaltungsversion, Chat-Version, Kreativversion, Entwicklerversion, Hoher Kontrast, Hell, Minimalistisch, mit automatischer Erkennung und Umschaltung
- **24 Controller** — Vollständige Systemverwaltung, Chat, Konfiguration und Überwachungsfunktionen
- **Keine Frontend-Framework-Abhängigkeit** — Serverseitige Generierung von HTML/CSS/JS über `H`, `CssBuilder` und `JsBuilder`

### Internationalisierung und Lokalisierung
- **34 Sprachvarianten** vollständig unterstützt, 2 Schriftsysteme und mehrere Regionalvarianten abdeckend
  - **Vereinfachtes Chinesisch**: zh-CN (Festlandchina), zh-SG (Singapur), zh-MY (Malaysia) (3 Varianten)
  - **Traditionelles Chinesisch**: zh-HK (Hongkong), zh-TW (Taiwan), zh-MO (Macau) (3 Varianten)
  - **Englisch**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 Varianten)
  - **Spanisch**: es-ES, es-MX (2 Varianten)
  - **Deutsch**: de-DE, de-AT, de-CH, de-LU, de-LI (5 Varianten)
  - **Französisch**: fr-FR, fr-CA, fr-CH (3 Varianten)
  - **Japanisch**: ja-JP | **Koreanisch**: ko-KR | **Tschechisch**: cs-CZ (3 Varianten)
  - **Italienisch**: it-IT | **Polnisch**: pl-PL | **Portugiesisch**: pt-PT, pt-BR (4 Varianten)

### Daten und Speicherung
- **SpeedyPack Hochleistungsspeicher** — Die Fast-Version verwendet die eigenentwickelte .spk-Speicher-Engine mit Speicherverzeichnisabbildung + Eintrags-Cache + asynchroner Schreibwarteschlange
- **Dateisystemspeicher** — Die Default-Version verwendet reinen Dateisystem-JSON-Speicher
- **Zeitindexabfrage** — Über die `ITimeStorage`-Schnittstelle effiziente Abfragen nach Zeitbereich
- **Automatische Komprimierung** — SpeedyPack unterstützt zeitgesteuerte automatische Komprimierung zur Rückgewinnung von freiem Speicherplatz
- **Minimale Abhängigkeiten** — Die Kernbibliothek hängt nur von Microsoft.CodeAnalysis.CSharp für Dynamische Kompilierung ab

## 🔄 Dual-Version-Architektur

Dieses Projekt bietet zwei Implementierungsversionen für unterschiedliche Anwendungsszenarien:

### SiliconLife.Default (Standardversion)
- **Positionierung**: Standardimplementierung, hauptsächlich zur Validierung der Architekturmachbarkeit
- **Ausführungsmodus**: Konsolenanwendung
- **Speichermethode**: Reiner Dateisystem-JSON-Speicher
- **Anwendungsszenarien**: Szenarien mit hohen Datensicherheitsanforderungen, begrenzten Speicherressourcen und kleinen Datenmengen
- **Merkmale**: Einfach und zuverlässig, sofortige Datenpersistierung, kein Risiko von Speicherverlust
- **Rollenbeschreibung**: Als Referenzimplementierung zur Architekturvalidierung geeignet für Erstkontakt, Entwicklung/Debugging oder datensicherheitspriorisierte Szenarien
- **Startbefehl**: `dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast (Hochleistungsversion)
- **Positionierung**: Empfohlene Produktivversion
- **Ausführungsmodus**: Desktop-Anwendung (Windows/macOS-System-Tray / Linux-Statusfenster)
- **Speichermethode**: SpeedyPack-Speicher + asynchrone Batch-Persistierung (.spk-Dateiformat)
- **Anwendungsszenarien**: Hochparallele, latenzarme und große Datenmengen-Szenarien
- **Plattformunterstützung**: Windows/macOS (volle Funktionalität inkl. System-Tray), Linux (Statusfenster, kein Tray-Icon)
- **Merkmale**:
  - Extremes Performance-Optimierung
  - Windows/macOS-Tray-Hintergrundbetrieb mit Echtzeit-Überwachung über Tray-Statusfenster; Linux-Statusfenster wird direkt angezeigt
  - SpeedyPack-Engine + automatische Komprimierung gewährleisten Datensicherheit
  - Component-UI-Architektur, 27 deklarative Komponenten
  - 7 Skin-Themes mit automatischer Erkennung und Umschaltung
  - Hot-Reload-Werkzeug unterstützt Online-Aktualisierung und Neustart → Linux öffnet automatisch den Browser für den Zugriff auf die Web-UI, unterstützt `--no-tray`-Parameter
- **Leistungssteigerung**: Speicherlese-Latenz um das 1000-fache reduziert, Schreib-Latenz um das 15000-fache reduziert, Parallelverarbeitungsleistung um das 50-fache gesteigert
- **Rollenbeschreibung**: Tief optimierte Produktivimplementierung, erste Wahl für langfristigen Betrieb und echte Produktionsumgebungen
- **Startbefehl**: `dotnet run --project src/SiliconLife.Fast`

### Versionsvergleich

| Funktion | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| **Ausführungsmodus** | Konsolenanwendung | Desktop-Anwendung (Windows/macOS-System-Tray / Linux-Statusfenster) |
| **Benutzeroberfläche** | Web-UI (Browserzugriff) | Windows/macOS: Tray-Icon + Tray-Fenster + Web-UI; Linux: Statusfenster + Web-UI |
| **System-Tray** | ❌ Nicht verfügbar | ✅ Windows/macOS: Minimierung zum Tray unterstützt; Linux: kein Tray-Icon |
| **Hintergrundbetrieb** | ❌ Konsole schließen beendet die Anwendung | ✅ Windows/macOS: Tray-Hintergrundbetrieb; Linux: Statusfenster-Betrieb |
| **Speichermethode** | Dateisystem-JSON-Speicher | SpeedyPack-Speicher + asynchrone Persistierung |
| **Speicher-Engine** | Dateisystem-I/O | SiliconLife.Speedy (.spk-Format) |
| **Lese-Latenz** | ~10ms (Disk-I/O) | ~0,01ms (Speicheroperation) |
| **Schreib-Latenz** | ~15ms (synchrone Schreibung) | ~0,001ms (asynchrone Schreibung) |
| **Parallelfähigkeit** | ~100 req/s | ~5000 req/s |
| **Speicherverbrauch** | ~200MB | ~500MB |
| **Datensicherheit** | Sehr hoch (sofortige Persistierung) | Hoch (asynchrone Persistierung + automatische Komprimierung) |
| **Anwendungsszenarien** | Datensicherheitspriorität, kleine Datenmengen | Performancepriorität, große Datenmengen, hohe Parallelität |

## 🛠️ Technologie-Stack

| Komponente | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| Laufzeit | .NET 9 | .NET 9 (Windows/macOS/Linux) |
| Programmiersprache | C# | C# |
| Anwendungstyp | Konsolenanwendung | Desktop-Anwendung (Windows/macOS-System-Tray / Linux-Statusfenster) |
| KI-Integration | Ollama (lokal), Alibaba Cloud Bailian (Cloud), Volcengine Ark (Cloud), Herdsman, Meituan LongCat, Qiniu Cloud AI | Ollama (lokal), Alibaba Cloud Bailian (Cloud), Volcengine Ark (Cloud), Herdsman, Meituan LongCat, Qiniu Cloud AI |
| Datenspeicher | Dateisystem (JSON + Zeitindex-Verzeichnisse) | SpeedyPack (.spk-Format, Speicherabbildung + asynchrone Persistierung) |
| Web-Server | HttpListener (.NET-integriert) | HttpListener (.NET-integriert) |
| Dynamische Kompilierung | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Browser-Automatisierung | Playwright (WebView) | Playwright (WebView) |
| Plugin-System | ✅ Unterstützt (IPlugin + PluginLoader) | ✅ Unterstützt (IPlugin + PluginLoader) |
| System-Tray | ❌ Nicht unterstützt | ✅ Windows/macOS unterstützt (NotifyIcon); Linux ohne Tray-Icon |
| Lizenz | Apache-2.0 | Apache-2.0 |

## 📁 Projektstruktur

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Kernbibliothek (Schnittstellen, abstrakte Klassen)
│   │   ├── AI/                            # KI-Client-Schnittstellen, Kontextmanager, Nachrichtenmodelle
│   │   ├── Audit/                         # Token-Nutzungsaudit-System
│   │   ├── Chat/                          # Chat-System, Sitzungsverwaltung, Broadcast-Kanal
│   │   ├── Compilation/                   # Dynamische Kompilierung, Sicherheitsscanner, Code-Verschlüsselung
│   │   ├── Config/                        # Konfigurationssystem
│   │   ├── Executors/                     # Executor (Festplatte, Netzwerk, Kommandozeile)
│   │   ├── IM/                            # IM-Provider-Schnittstelle
│   │   ├── Knowledge/                     # Wissensnetzwerk-System
│   │   ├── Localization/                  # Lokalisierungssystem
│   │   ├── Logging/                       # Protokollierungssystem
│   │   ├── Plugins/                       # Plugin-System (IPlugin-Schnittstelle, PluginLoader)
│   │   ├── Project/                       # Projektsystem
│   │   ├── Runtime/                       # Hauptschleife, Tick-Objekte, Core Host
│   │   ├── Security/                      # Berechtigungsverwaltungssystem
│   │   ├── SiliconBeing/                  # Silicon Being-Basisklasse, Manager, Fabrik
│   │   ├── Storage/                       # Speicherschnittstellen
│   │   ├── Time/                          # Unvollständiges Datum (Zeitbereichsabfragen)
│   │   ├── Tools/                         # Werkzeugschnittstellen und Werkzeugmanager
│   │   ├── WebView/                       # WebView-Browser-Schnittstelle
│   │   ├── Workflow/                      # Workflow-Engine (Vorlagen, Instanzen, Zustandsübergänge)
│   │   └── ServiceLocator.cs              # Globaler Service-Locator
│   │
│   ├── SiliconLife.Common/                # Gemeinsame Implementierung (von beiden Versionen genutzt)
│   │   ├── AI/                            # KI-Clients und Fabriken (Ollama, DashScope, VolcengineArk, Herdsman, LongCat, QiniuAI)
│   │   ├── Calendar/                      # 32 Kalenderimplementierungen
│   │   ├── Localization/                  # Lokalisierungsbasis und 34 Sprach-/Regionalvarianten
│   │   ├── Resources/                     # Gemeinsame Ressourcendateien
│   │   ├── Security/                      # Berechtigungsmanager
│   │   ├── SiliconBeing/                  # Standard-Silicon Being-Implementierung
│   │   ├── Tools/                         # 23 allgemeine Werkzeugimplementierungen
│   │   ├── Web/                           # Web-Infrastruktur
│   │   └── WebView/                       # Playwright WebView-Implementierung
│   │
│   ├── SiliconLife.App/                   # Anwendungsschicht (Web-UI + Hilfedokumentation, Default und Fast gemeinsam)
│   │   ├── Config/                        # Anwendungskonfiguration
│   │   ├── Data/                          # Datenverzeichnis
│   │   ├── Help/                          # Hilfedokumentation-Lokalisierung (mehrsprachig)
│   │   ├── Tools/                         # HelpTool (Hilfedokumentation-Abfragewerkzeug)
│   │   └── Web/                           # Web-UI-Implementierung
│   │       ├── Component/                 # UI-Komponentenbibliothek (27 Komponenten)
│   │       ├── Controllers/               # 24 Controller
│   │       ├── Models/                    # View-Modelle
│   │       ├── Views/                     # HTML-Views
│   │       └── Skins/                     # 7 Skin-Themes
│   │
│   ├── SiliconLife.Default/               # Standardimplementierung + Anwendungseinstieg (Konsolenversion)
│   │   ├── Program.cs                     # Einstiegspunkt (alle Komponenten assemblierend)
│   │   ├── Config/                        # Standard-Konfigurationsdaten
│   │   ├── Knowledge/                     # Wissensnetzwerk-Implementierung
│   │   ├── Logging/                       # Protokoll-Provider-Implementierung (Konsole + Dateisystem)
│   │   ├── Project/                       # Projektsystem-Implementierung
│   │   └── Storage/                       # Dateisystem-Speicherimplementierung
│   │
│   ├── SiliconLife.Fast/                  # Hochleistungsimplementierung + Anwendungseinstieg (Fensterversion)
│   │   ├── Program.cs                     # Einstiegspunkt (Fensteranwendung)
│   │   ├── App.axaml / App.cs             # Avalonia-Anwendungsdefinition
│   │   ├── Config/                        # Konfigurationsdaten (mit Default geteilt)
│   │   ├── Knowledge/                     # Wissensnetzwerk-Implementierung (Speicheroptimiert)
│   │   ├── Logging/                       # Hochleistungs-Protokoll-Provider
│   │   ├── Project/                       # Projektsystem-Implementierung
│   │   ├── Storage/                       # SpeedyPack-Speicheradapter
│   │   └── Tray/                          # System-Tray (34 Sprachvarianten-Lokalisierung)
│   │
│   ├── SiliconLife.Speedy/                # SpeedyPack Hochleistungs-Speicher-Engine
│   │   ├── SpeedyPack.cs                  # Kernklasse (Speicherverzeichnisabbildung + Cache + asynchrone Schreibung)
│   │   ├── SpeedyPackOptions.cs           # Konfigurationsoptionen (Cache-TTL, max. Einträge usw.)
│   │   ├── IPackTransaction.cs            # Transaktionsschnittstelle
│   │   ├── SpkFileInfo.cs                 # Dateiinformationen
│   │   └── Internal/                      # Interne Implementierung
│   │       ├── DirectoryMap.cs            # Speicherverzeichnisabbildung
│   │       ├── EntryCache.cs              # Eintrags-Cache
│   │       ├── FreeList.cs                # Freeliste
│   │       ├── PackFileReader.cs          # Pack-Datei-Leser
│   │       ├── PackFileWriter.cs          # Pack-Datei-Schreiber
│   │       ├── WriteQueue.cs              # Asynchrone Schreibwarteschlange
│   │       ├── WriteOperation.cs          # Schreiboperation
│   │       ├── SpeedyTransaction.cs       # Transaktionsimplementierung
│   │       ├── SpkHeader.cs               # Pack-Datei-Header
│   │       └── PathNormalizer.cs          # Pfadnormalisierung
│   │
│   └── SiliconLife.Speedy.Manager/        # SpeedyPack-Manager (Avalonia UI)
│       ├── MainForm.cs                    # Hauptfenster
│       ├── Program.cs                     # Einstiegspunkt
│       └── slc.ico                        # Anwendungssymbol
│
├── docs/                                  # Mehrsprachige Dokumentation
│   ├── zh-CN/                             # Vereinfachtes Chinesisch
│   ├── en/                                # Englisch
│   └── ...                                # Andere Sprachen
│
└── 总文档/                                 # Anforderungs- und Architekturdokumentation
    ├── 需求文档.md
    ├── 架构大纲.md
    └── 实现顺序.md
```

## 🏗️ Architekturübersicht

### Scheduling-Architektur
```
Hauptschleife (Dedizierter Thread, Watchdog + Circuit Breaker)
  └── Tick-Objekt (nach Priorität sortiert)
       └── Silicon Being Manager
            └── Silicon Being Runner (Temporärer Thread, Timeout + Circuit Breaker)
                 └── Silicon Being.Tick()
                      └── Kontextmanager.Denken()
                           └── KI-Client.Chat()
                                └── Werkzeugaufruf-Zyklus → Persistierung im Chat-System
```

### Sicherheitsarchitektur
Alle KI-initiierten I/O-Operationen müssen eine strenge Sicherheitskette durchlaufen:

```
Werkzeugaufruf → Executor → Berechtigungsmanager → [Benutzerfrequenz-Cache → Callback → (IsCurator: Benutzer fragen | Non-curator: Globale ACL)]
```

## 🚀 Schnellstart

### Voraussetzungen

- **.NET 9 SDK** — [Download-Link](https://dotnet.microsoft.com/download/dotnet/9.0)
- **KI-Backend** (eines auswählen):
  - **Ollama**: [Ollama installieren](https://ollama.com) und Modell laden (z. B. `ollama pull llama3`)
  - **Alibaba Cloud Bailian**: API-Schlüssel aus der [Bailian-Konsole](https://bailian.console.aliyun.com/) abrufen
  - **Volcengine Ark**: API-Schlüssel aus der [Volcengine-Konsole](https://console.volcengine.com/ark) abrufen
  - **Herdsman**: Keine Authentifizierung erforderlich, kompatibel mit OpenAI-API-Format
  - **Meituan LongCat**: API-Schlüssel von der Meituan-Plattform abrufen
  - **Qiniu Cloud AI**: API-Schlüssel von der [Qiniu-Konsole](https://portal.qiniu.com/) abrufen

### Projekt erstellen

```bash
dotnet restore
dotnet build
```

### System starten

#### Methode 1: Default-Version ausführen (Konsolenanwendung)

```bash
dotnet run --project src/SiliconLife.Default
```

Die Anwendung startet den Web-Server und öffnet die Web-UI automatisch im Browser.

**Anwendungsszenarien**:
- ✅ Höchste Datensicherheitsanforderungen
- ✅ Begrenzte Speicherressourcen (RAM < 2GB)
- ✅ Kleine Datenmengen, kurzfristige Nutzung
- ✅ Entwicklungs- und Debugging-Phase

#### Methode 2: Fast-Version ausführen (Desktop-Anwendung)

```bash
dotnet run --project src/SiliconLife.Fast
```

**Windows/macOS**: Die Anwendung startet im Fenstermodus, minimiert in den System-Tray und läuft im Hintergrund weiter.

**Linux**: Die Anwendung zeigt ein Statusfenster an (kein System-Tray-Icon) und öffnet automatisch den Browser für den Zugriff auf die Web-UI. Der Parameter `--no-tray` kann verwendet werden, um das automatische Öffnen des Browsers zu überspringen:

```bash
dotnet run --project src/SiliconLife.Fast -- --no-tray
```

**Anwendungsszenarien**:
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

# Linux - Default-Version
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# Linux - Fast-Version
dotnet publish src/SiliconLife.Fast -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - Default-Version
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true

# macOS - Fast-Version
dotnet publish src/SiliconLife.Fast -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 Entwicklungs-Roadmap

### ✅ Abgeschlossen
- [x] Phase 1: Konsolen-KI-Chat
- [x] Phase 2: Framework-Grundgerüst (Hauptschleife + Tick-Objekt + Watchdog + Circuit Breaker)
- [x] Phase 3: Erstes Silicon Being mit Soul-Datei (Körper-Gehirn-Architektur)
- [x] Phase 4: Persistente Erinnerung (Chat-System + Zeitspeicher-Schnittstelle)
- [x] Phase 5: Werkzeugsystem + Executor
- [x] Phase 6: Berechtigungssystem (5-stufige Kette, Audit-Logger, Globale ACL)
- [x] Phase 7: Dynamische Kompilierung + Selbstevolution (Roslyn)
- [x] Phase 8: Langzeitgedächtnis + Aufgaben + Timer
- [x] Phase 9: Core Host + Multi-Agenten-Kollaboration
- [x] Phase 10: Web-UI (HTTP + SSE, 24 Controller, 7 Skins)
- [x] Phase 10.5: Inkrementelle Erweiterungen (Broadcast-Kanal, Token-Audit, 32 Kalender, Werkzeugverbesserungen, 34 Sprachvarianten-Lokalisierung)
- [x] Phase 10.6: Verbesserung und Optimierung (WebView, Hilfesystem, Projektarbeitsbereich, Wissensnetzwerk, Workflow-Engine)
- [x] Phase 11: SpeedyPack-Speicher-Engine (ersetzt LiteDB, Speicherabbildung, asynchrone Schreibwarteschlange, automatische Komprimierung)
- [x] Phase 12: Plugin-System (IPlugin-Schnittstelle, PluginLoader Fähigkeitsdeklaration, isoliertes Laden, Werkzeugintegration)

### 🚧 Geplant
- [ ] Phase 13: Externe IM-Integration (Feishu / WhatsApp / Telegram)
- [ ] Phase 14: Skill-Ökosystem (Plugin-Marktplatz, Skill-Paket-Verteilung)

## 📚 Dokumentation

- [Architekturdesign](architecture.md) — Systemdesign, Scheduling-Mechanismus, Komponentenarchitektur
- [Sicherheitsmodell](security.md) — Berechtigungsmodell, Executor, Dynamische Kompilierungssicherheit
- [Entwicklungsleitfaden](development-guide.md) — Werkzeugentwicklung, Erweiterungsleitfaden
- [API-Referenz](api-reference.md) — Web-API-Endpunktdokumentation
- [Werkzeugreferenz](tools-reference.md) — Detaillierte Beschreibung der integrierten Werkzeuge
- [Web-UI-Leitfaden](web-ui-guide.md) — Leitfaden zur Web-Oberfläche
- [Silicon Being-Leitfaden](silicon-being-guide.md) — Agenten-Entwicklungsleitfaden
- [Berechtigungssystem](permission-system.md) — Detaillierte Erklärung der Berechtigungsverwaltung
- [Kalendersystem](calendar-system.md) — Beschreibung der 32 Kalendersysteme
- [Schnellstart](getting-started.md) — Detaillierter Einstiegsleitfaden
- [Fehlerbehebung](troubleshooting.md) — Häufig gestellte Fragen
- [Roadmap](roadmap.md) — Vollständiger Entwicklungsplan
- [Änderungsprotokoll](changelog.md) — Versionsaktualisierungsverlauf
- [Beitragshandbuch](contributing.md) — Wie Sie am Projekt mitwirken können

## 🤝 Mitwirken

Wir freuen uns über alle Formen der Mitarbeit! Details finden Sie im [Beitragshandbuch](contributing.md).

### Entwicklungs-Workflow
1. Repository forken
2. Feature-Branch erstellen (`git checkout -b feature/AmazingFeature`)
3. Änderungen committen (`git commit -m 'feat: add some AmazingFeature'`)
4. Zum Branch pushen (`git push origin feature/AmazingFeature`)
5. Pull Request einreichen

## 💡 Versionsauswahl-Leitfaden

### Welche Version sollte ich verwenden?

**SiliconLife.Default (Standardimplementierung — Validierung der Architekturmachbarkeit):**
- 📌 Sie kommen zum ersten Mal mit diesem Projekt in Kontakt und möchten die Systemarchitektur schnell verstehen
- 📌 Sie befinden sich in der Entwicklungs-/Debugging-Phase und benötigen eine einfache, direkte Ausführungsmethode
- 📌 Datensicherheit hat für Sie höchste Priorität
- 📌 Ihr System hat weniger als 4GB Arbeitsspeicher
- 📌 Sie benötigen nur Einzelnutzung oder haben kleine Datenmengen

**SiliconLife.Fast (Empfohlene Produktivversion):**
- ⚡ Sie benötigen eine langfristig stabile Produktionsumgebung
- ⚡ Sie sind mit der Systemarchitektur vertraut und bereit für den produktiven Einsatz
- ⚡ Sie müssen Mehrbenutzer-Parallelzugriff unterstützen
- ⚡ Sie benötigen Tray-Hintergrundbetrieb
- ⚡ Sie streben nach maximalem Leistungserlebnis

> **Gesamtempfehlung**: SiliconLife.Default eignet sich als Architekturvalidierung und Ersterfahrung; für echte Produktionsumgebungen wird dringend SiliconLife.Fast empfohlen.

### Kann man von Default zu Fast migrieren?

**Absolut!** Beide Versionen teilen sich:
- ✅ Konfigurationsdateiformat (config.json)
- ✅ Werkzeugschnittstellen
- ✅ Being-Konfiguration
- ✅ Web-UI-Oberfläche

**Migrationsschritte:**
1. Sichern Sie Ihr Default-Datenverzeichnis
2. Starten Sie die Fast-Version mit demselben Datenverzeichnis
3. Fast importiert automatisch die vorhandenen Daten in die SpeedyPack-Speicher-Engine
4. Nach Verifizierung der Funktionalität können Sie die Fast-Version regulär nutzen

### Können beide Versionen koexistieren?

**Ja!** Folgende Bereitstellungsstrategie wird empfohlen:

**Strategie 1: Default zur Validierung, Fast für Produktion**
```
Entwicklungs-/Validierungsumgebung: SiliconLife.Default (Architektur validieren, Funktionen debuggen)
Produktionsumgebung: SiliconLife.Fast (Hohe Leistung, Hintergrundbetrieb, Verarbeitung Echtzeit-Anfragen)
```

**Strategie 2: Fast als Hauptlaufzeit, Default für regelmäßige Backups**
```
SiliconLife.Fast (Tägliche Nutzung, Verarbeitung Echtzeit-Anfragen)
    ↓ Regelmäßige Sicherung
SiliconLife.Default (Kaltdatenarchivierung, Datensicherheits-Fallback)
```

## 📄 Lizenz

Dieses Projekt steht unter der Apache License 2.0 — siehe [LICENSE](../../LICENSE)-Datei für Details.

## 👨‍💻 Autor

**天源垦骥**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- Gitee: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili: [617827040](https://space.bilibili.com/617827040)

## 🙏 Danksagung

Vielen Dank an alle Entwickler und KI-Plattform-Anbieter, die zu diesem Projekt beigetragen haben.

---

**Silicon Life Collective** — KI-Agenten wirklich zum „Leben" erwecken
