# Architektur

> **Version: v0.2.0-alpha**

[English](../en/architecture.md) | **Deutsch** | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Čeština](../cs-CZ/architecture.md) | [Русский](../ru-RU/architecture.md)

## Dual-Version-Architektur

Dieses Projekt bietet zwei Implementierungsversionen, die das gleiche Architekturdesign teilen, sich jedoch in der Speicherung und Leistungsoptimierung unterscheiden:

### SiliconLife.Default (Standardversion)
- **Positionierung**: Standardimplementierung, hauptsächlich zur Validierung der Architektur
- **Ausführungsmodus**: Konsolenanwendung
- **Speicherart**: Reines Dateisystem-JSON-Speicherung
- **Einsatzszenarien**: Hohe Datensicherheitsanforderungen, eingeschränkte Speicherressourcen, kleine Datenmengen
- **Rollenbeschreibung**: Dient als Referenzimplementierung zur Architekturvalidierung, bietet eine einfache, zuverlässige Ausführung, ideal für den ersten Kontakt mit dem Projekt, die Entwicklung und das Debugging oder Szenarien mit Datensicherheitspriorität

### SiliconLife.Fast (Hochleistungsversion)
- **Positionierung**: Empfohlene Produktivversion
- **Ausführungsmodus**: Desktop-Anwendung (Windows System-Tray / Linux Statusfenster)
- **Speicherart**: SpeedyPack-In-Memory-Speicherung + asynchrone Batch-Persistierung (.spk-Dateiformat)
- **Einsatzszenarien**: Hohe Nebenläufigkeit, geringe Latenz, große Datenmengen
- **Plattformunterstützung**: Windows/macOS (volle Funktionalität inkl. System-Tray), Linux (Statusfenster, kein Tray-Icon)
- **Merkmale**:
  - Windows/macOS System-Tray-Hintergrundbetrieb, Tray-Statusfenster mit Echtzeitüberwachung; Linux Statusfenster wird direkt angezeigt
  - SpeedyPack-Engine + automatische Komprimierung gewährleisten Datensicherheit
  - Component-UI-Architektur, 27 deklarative Komponenten
  - 7 Skin-Themes, Unterstützung für automatische Erkennung und Wechsel
  - Hot-Reload-Werkzeug unterstützt Online-Updates und Neustart
  - Linux öffnet automatisch den Browser für den Zugriff auf die Web-UI, unterstützt den Parameter `--no-tray`
- **Leistungssteigerung**: Speicherlese-Latenz um das 1000-fache reduziert, Schreib-Latenz um das 15000-fache reduziert
- **Rollenbeschreibung**: Eine tiefgreifend optimierte produktionsreife Implementierung mit System-Tray-Hintergrundbetrieb, SpeedyPack-Engine + automatischer Komprimierung und weiteren Eigenschaften, die erste Wahl für den Langzeitbetrieb und echte Produktivumgebungen

> **Hinweis**: Die in diesem Dokument beschriebene Architektur gilt für beide Versionen, lediglich der Speicherimplementierungsteil unterscheidet sich. SiliconLife.Default dient als Architekturvalidierungsreferenz, SiliconLife.Fast als empfohlene Produktivversion.

---

## Kernkonzepte

### Silicon Being

Jeder KI-Agent im System ist ein **Silicon Being** — eine autonome Entität mit eigener Identität, Persönlichkeit und Fähigkeiten. Jedes Silicon Being wird durch eine **Soul-Datei** (Markdown-Prompt) angetrieben, die sein Verhaltensmuster definiert.

### Silicon Curator

Der **Silicon Curator** ist ein spezielles Silicon Being mit den höchsten Systemberechtigungen. Er fungiert als Systemadministrator:

- Erstellt und verwaltet andere Silicon Beings
- Analysiert Benutzeranfragen und zerlegt sie in Aufgaben
- Verteilt Aufgaben an die entsprechenden Silicon Beings
- Überwacht die Ausführungsqualität und behandelt Fehler
- Verwendet **Prioritätsscheduling** zur Beantwortung von Benutzernachrichten (siehe unten)

### Soul-Datei

Eine Markdown-Datei (`soul.md`), die im Datenverzeichnis jedes Silicon Beings gespeichert ist. Sie wird als System-Prompt in jede KI-Anfrage injiziert und definiert die Persönlichkeit, Entscheidungsstrukturen und Verhaltensbeschränkungen des Wesens.

---

## Scheduling: Zeitschlitz-Fair-Scheduling

### Hauptschleife + Tick-Objekte

Das System betreibt eine **taktgetriebene Hauptschleife** auf einem dedizierten Hintergrund-Thread:

```
Hauptschleife (dedizierter Thread, Watchdog + Circuit Breaker)
  └── Tick-Objekt A (Priorität=0, Intervall=100ms)
  └── Tick-Objekt B (Priorität=1, Intervall=500ms)
  └── Silicon Being Manager (direkt durch Hauptschleife getaktet)
        └── Silicon Being Runner → Silicon Being 1 → Takt → eine Runde ausführen
        └── Silicon Being Runner → Silicon Being 2 → Takt → eine Runde ausführen
        └── Silicon Being Runner → Silicon Being 3 → Takt → eine Runde ausführen
        └── ...
```

Wichtige Designentscheidungen:

- **Silicon Beings erben nicht von Tick-Objekten.** Sie haben eine eigene `Tick()`-Methode, die von `SiliconBeingManager` über `SiliconBeingRunner` aufgerufen wird, anstatt sich direkt bei der Hauptschleife zu registrieren.
- Der **Silicon Being Manager** wird direkt durch die Hauptschleife getaktet und fungiert als einzelner Proxy für alle Wesen.
- Der **Silicon Being Runner** kapselt die `Tick()`-Methode jedes Wesens auf einem temporären Thread mit Timeout und einem pro-Wesen-Circuit-Breaker (3 aufeinanderfolgende Timeouts → 1 Minute Abkühlzeit).
- Die Ausführung jedes Wesens ist pro Takt auf **eine Runde** KI-Anfrage + Werkzeugaufruf beschränkt, wodurch sichergestellt wird, dass kein Wesen die Hauptschleife monopolisieren kann.
- Der **Leistungsmonitor** verfolgt die Takt-Ausführungszeiten für Beobachtbarkeit.

### Curator-Prioritätsantwort

Wenn ein Benutzer eine Nachricht an den Silicon Curator sendet:

1. Das aktuelle Wesen (z. B. Wesen A) beendet seine aktuelle Runde — **kein Abbruch**.
2. Der Manager **überspringt die restliche Warteschlange**.
3. Die Schleife **beginnt erneut beim Curator**, sodass dieser sofort ausgeführt wird.

Dies stellt sicher, dass auf Benutzerinteraktionen reagiert wird, ohne laufende Aufgaben zu stören.

---

## Komponentenarchitektur

```
┌─────────────────────────────────────────────────────────┐
│                        Core Host                         │
│  (Einheitlicher Host — assembliert und verwaltet        │
│   alle Komponenten)                                      │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ Hauptsch. │  │ Service-     │  │  Konfiguration   │  │
│  │ schleife  │  │ Lokator      │  │                  │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │      Silicon Being Manager (Tick-Objekt)          │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Curator   │ │Wesen A  │ │Wesen B  │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Gemeinsame Dienste                   │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Chat-     │  │ Speicher │  │  Berechtigungs-  │  │   │
│  │  │System    │  │          │  │  manager         │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │ KI-      │  │Executor  │  │  Werkzeug-       │  │   │
│  │  │ Client   │  │          │  │  manager         │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  │  ┌──────────┐ ┌──────────┐                        │   │
│  │  │Plugin-   │  │Wissens-  │                        │   │
│  │  │Lader     │  │netzwerk  │                        │   │
│  │  └──────────┘ └──────────┘                        │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Executors                        │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │  Disk-   │  │ Netzwerk-│  │  Kommando-       │  │   │
│  │  │ Executor │  │ Executor │  │  zeilen-Executor │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              IM-Provider                          │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Konsolen-│  │  Web-    │  │  Feishu / ...    │  │   │
│  │  │ Provider │  │ Provider │  │  Provider        │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## Service-Lokator

Der `ServiceLocator` ist eine threadsichere Singleton-Registrierung, die Zugriff auf alle Kerndienste bietet:

| Eigenschaft | Typ | Beschreibung |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | Zentraler Chat-Sitzungsmanager |
| `IMManager` | `IMManager` | IM-Provider-Router |
| `AuditLogger` | `AuditLogger` | Berechtigungs-Audit-Trail |
| `GlobalAcl` | `GlobalACL` | Globale ACL |
| `BeingFactory` | `ISiliconBeingFactory` | Fabrik zur Erstellung von Wesen |
| `BeingManager` | `SiliconBeingManager` | Lebenszyklusmanager für aktive Wesen |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Dynamischer Kompilierungs-Lader |
| `TokenUsageAudit` | `ITokenUsageAudit` | Token-Nutzungs-Tracking |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Token-Nutzungsbericht |

Er verwaltet außerdem eine Registrierung von `PermissionManager` pro Wesen, verschlüsselt mit der Wesen-GUID als Schlüssel.

---

## Chat-System

### Sitzungstypen

Das Chat-System unterstützt drei Sitzungstypen über `SessionBase`:

| Typ | Klasse | Beschreibung |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | Eins-zu-eins-Gespräch zwischen zwei Teilnehmern |
| `GroupChat` | `GroupChatSession` | Mehrteilnehmer-Gruppenchat |
| `Broadcast` | `BroadcastChannel` | Offener Kanal mit fester ID; Wesen abonnieren dynamisch und empfangen nur Nachrichten nach dem Abonnement |

### Broadcast-Kanal

Der `BroadcastChannel` ist ein spezieller Sitzungstyp für systemweite Ankündigungen:

- **Feste Kanal-ID** — Im Gegensatz zu `SingleChatSession` und `GroupChatSession` ist die Kanal-ID eine wohlbekannte Konstante und nicht von Mitglieder-GUIDs abgeleitet.
- **Dynamisches Abonnement** — Wesen abonnieren/melden sich zur Laufzeit ab; sie empfangen nur Nachrichten, die nach dem Abonnement veröffentlicht wurden.
- **Ausstehende Nachrichten-Filterung** — `GetPendingMessages()` gibt nur Nachrichten zurück, die nach der Abonnementzeit des Wesens veröffentlicht und noch nicht gelesen wurden.
- **Vom Chat-System verwaltet** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### Chat-Nachricht

Das `ChatMessage`-Modell enthält Felder für KI-Konversationskontext und Token-Tracking:

| Feld | Typ | Beschreibung |
|-------|------|-------------|
| `Id` | `Guid` | Eindeutiger Nachrichtenidentifikator |
| `SenderId` | `Guid` | Eindeutiger Identifikator des Absenders |
| `ChannelId` | `Guid` | Kanal/Konversations-Identifikator |
| `Content` | `string` | Nachrichteninhalt |
| `Timestamp` | `DateTime` | Sendezeitpunkt der Nachricht |
| `Type` | `MessageType` | Text, Bild, Datei oder Systembenachrichtigung |
| `ReadBy` | `List<Guid>` | IDs der Teilnehmer, die diese Nachricht gelesen haben |
| `Role` | `MessageRole` | KI-Konversationsrolle (Benutzer, Assistent, Werkzeug) |
| `ToolCallId` | `string?` | Werkzeugaufruf-ID für Werkzeugergebnis-Nachrichten |
| `ToolCallsJson` | `string?` | Serialisiertes Werkzeugaufruf-JSON für Assistenten-Nachrichten |
| `Thinking` | `string?` | Gedankenketten-Schlussfolgerung der KI |
| `PromptTokens` | `int?` | Anzahl der Token im Prompt (Eingabe) |
| `CompletionTokens` | `int?` | Anzahl der Token in der Vervollständigung (Ausgabe) |
| `TotalTokens` | `int?` | Gesamtzahl der verwendeten Token (Eingabe + Ausgabe) |
| `FileMetadata` | `FileMetadata?` | Angehängte Dateimetadaten (falls die Nachricht eine Datei enthält) |

### Chat-Nachrichtenwarteschlange

Die `ChatMessageQueue` ist ein threadsicheres Nachrichtenwarteschlangensystem zur Verwaltung der asynchronen Verarbeitung von Chat-Nachrichten:

- **Threadsicherheit** - Verwendet Sperrmechanismen zur Gewährleistung sicherer gleichzeitiger Zugriffe
- **Asynchrone Verarbeitung** - Unterstützt asynchrones Ein- und Ausreihen von Nachrichten
- **Nachrichtensortierung** - Behält die zeitliche Reihenfolge der Nachrichten bei
- **Stapeloperationen** - Unterstützt den Abruf von Nachrichten in Stapeln

### Dateimetadaten

`FileMetadata` verwaltet Dateiinformationen, die an Chat-Nachrichten angehängt sind:

- **Dateiinformationen** - Dateiname, Größe, Typ, Pfad
- **Upload-Zeitpunkt** - Zeitstempel des Datei-Uploads
- **Hochladender** - Benutzer- oder Silicon Being-ID des Hochladenden

### Stream-Abbruch-Manager

Der `StreamCancellationManager` bietet einen Abbruchmechanismus für KI-Streaming-Antworten:

- **Stream-Steuerung** - Unterstützt den Abbruch laufender KI-Streaming-Antworten
- **Ressourcenbereinigung** - Ordnungsgemäße Bereinigung zugehöriger Ressourcen bei Abbruch
- **Nebenläufigkeitssicherheit** - Unterstützt die gleichzeitige Verwaltung mehrerer Streams

### Chat-Verlaufsansicht

Die neue Chat-Verlaufsansichtsfunktion ermöglicht Benutzern, historische Konversationen von Silicon Beings zu durchsuchen:

- **Sitzungsliste** - Zeigt alle historischen Sitzungen an
- **Nachrichtendetails** - Anzeige der vollständigen Nachrichtenhistorie
- **Zeitachsenansicht** - Chronologische Darstellung der Nachrichten
- **API-Unterstützung** - Bietet RESTful-API zum Abrufen von Sitzungs- und Nachrichtendaten

---

## KI-Client-System

Das System unterstützt mehrere KI-Backends über die `IAIClient`-Schnittstelle:

### OllamaClient

- **Typ**: Lokaler KI-Dienst
- **Protokoll**: Native Ollama HTTP-API (`/api/chat`, `/api/generate`)
- **Funktionen**: Streaming, Werkzeugaufrufe, lokales Modell-Hosting
- **Konfiguration**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (Alibaba Cloud Bailian)

- **Typ**: Cloud-KI-Dienst
- **Protokoll**: OpenAI-kompatible API (`/compatible-mode/v1/chat/completions`)
- **Authentifizierung**: Bearer-Token (API-Schlüssel)
- **Funktionen**: Streaming, Werkzeugaufrufe, Inferenz-Inhalt (Gedatenkette), Multi-Region-Bereitstellung
- **Unterstützte Regionen**:
  - `beijing` — Nordchina 2 (Peking)
  - `virginia` — USA (Virginia)
  - `singapore` — Singapur
  - `hongkong` — Hongkong, China
  - `frankfurt` — Deutschland (Frankfurt)
- **Unterstützte Modelle** (dynamisch über API entdeckt, mit Fallback-Liste):
  - **Qwen-Serie**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **Inferenz**: qwq-plus
  - **Drittanbieter**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **Konfiguration**: `apiKey`, `region`, `model`
- **Modellentdeckung**: Abruf verfügbarer Modelle zur Laufzeit von der Bailian-API; Fallback auf kuratierte Liste bei Netzwerkfehlern

### VolcengineArkClient (Volcengine Ark)

- **Typ**: Cloud-KI-Dienst
- **Protokoll**: OpenAI-kompatible API
- **Authentifizierung**: Bearer-Token (API-Schlüssel)
- **Funktionen**: Unterstützt Streaming- und Nicht-Streaming-Modi, integrierte zweistufige Ratensteuerung
  - Selbst-Ratensteuerung: Erzwingt minimale Intervalle zwischen Anfragen
  - Server-Ratenbegrenzung: Behandelt 429-Fehler mit exponentiellem Backoff-Retry
- **Konfiguration**: `apiKey`, `endpoint`, `model`
- **Besonderheit**: KI-Dienst von ByteDance, unterstützt verschiedene Doubao-Modelle

### HerdsmanClient

- **Typ**: Lokale/Cloud-Inferenz-Engine
- **Protokoll**: OpenAI-kompatible API
- **Authentifizierung**: Keine
- **Funktionen**: Streaming, Werkzeugaufrufe, Reasoning-Inhalte
- **Konfiguration**: `endpoint`, `model`

### LongCatClient (Meituan LongCat)

- **Typ**: Cloud-KI-Service
- **Protokoll**: OpenAI-kompatible API
- **Authentifizierung**: Bearer-Token (API-Schlüssel)
- **Funktionen**: Streaming, Werkzeugaufrufe
- **Konfiguration**: `apiKey`, `endpoint`, `model`

### QiniuAIClient (Qiniu Cloud AI)

- **Typ**: Cloud-KI-Service
- **Protokoll**: OpenAI-kompatible API
- **Authentifizierung**: Bearer-Token (API-Schlüssel)
- **Funktionen**: Streaming, Werkzeugaufrufe
- **Konfiguration**: `apiKey`, `endpoint`, `model`

### IAIClient-Fähigkeitsschnittstelle

Die `IAIClient`-Schnittstelle definiert die Fähigkeiten jedes KI-Clients und ermöglicht es dem `ContextManager`, sein Verhalten anzupassen:

| Fähigkeit | Rückgabetyp | Beschreibung |
|-----------|------------|-------------|
| `StreamingMode` | `StreamingMode` | Unterstützter Streaming-Modus (None/Streaming/Reasoning) |
| `SupportsToolCalls` | `bool` | Werkzeugaufruf-Unterstützung |
| `ContextWindowTokens` | `int` | Kontextfenstergröße in Tokens |
| `SupportsVision` | `bool` | Vision- (Bild-)Unterstützung |
| `SupportsAudio` | `bool` | Audio-Unterstützung |

### Client-Fabrikmuster

Jeder KI-Client-Typ hat eine entsprechende Fabrikimplementierung von `IAIClientFactory`:

- `OllamaClientFactory` — Erstellt OllamaClient-Instanzen
- `DashScopeClientFactory` — Erstellt DashScopeClient-Instanzen
- `VolcengineArkClientFactory` — Erstellt VolcengineArkClient-Instanzen
- `HerdsmanClientFactory` — Erstellt HerdsmanClient-Instanzen
- `LongCatClientFactory` — Erstellt LongCatClient-Instanzen
- `QiniuAIClientFactory` — Erstellt QiniuAIClient-Instanzen

Die Fabriken bieten:
- `CreateClient(Dictionary<string, object> config)` — Instanziiert einen Client aus der Konfiguration
- `GetConfigKeyOptions(string key, ...)` — Gibt dynamische Optionen für Konfigurationsschlüssel zurück (z. B. verfügbare Modelle, Regionen)
- `GetDisplayName()` — Lokalisierter Anzeigename des Client-Typs

### KI-Plattform-Support-Liste

#### Statusbeschreibung
- ✅ Implementiert
- 🚧 In Entwicklung
- 📋 Geplant
- 💡 In Erwägung

*Hinweis: Aufgrund der Netzwerkumgebung des Entwicklers kann der Zugriff auf [in Erwägung befindliche] ausländische Cloud-KI-Dienste möglicherweise Netzwerk-Proxy-Tools erfordern, und der Debugging-Prozess kann instabil sein.*

#### Plattformliste

| Plattform | Status | Typ | Beschreibung |
|------|------|------|------|
| Ollama | ✅ | Lokal | Lokaler KI-Dienst, unterstützt lokale Modellbereitstellung |
| DashScope (Alibaba Cloud Bailian) | ✅ | Cloud | Alibaba Cloud Bailian KI-Dienst, unterstützt Multi-Region-Bereitstellung |
| Baidu Qianfan (Wenxin Yiyan) | 📋 | Cloud | Baidu Wenxin Yiyan KI-Dienst |
| Zhipu AI (GLM) | 📋 | Cloud | Zhipu Qingyan KI-Dienst |
| Moonshot (Kimi) | 📋 | Cloud | Moonshot Kimi KI-Dienst |
| Volcengine Ark Doubao | ✅ | Cloud | ByteDance Doubao KI-Dienst |
| Herdsman | ✅ | Lokal/Cloud | Authentifizierungsfreie Inferenz-Engine, kompatibel mit OpenAI-API-Format |
| Meituan LongCat | ✅ | Cloud | Meituans eigenes Großmodell, kompatibel mit OpenAI-API-Format, API-Schlüssel-Authentifizierung |
| Qiniu Cloud AI | ✅ | Cloud | Qiniu Cloud-KI-Service, API-Schlüssel-Authentifizierung |
| DeepSeek (Direktverbindung) | 📋 | Cloud | DeepSeek KI-Dienst |
| 01.AI (Yi) | 📋 | Cloud | 01.AI KI-Dienst |
| Tencent Hunyuan | 📋 | Cloud | Tencent Hunyuan KI-Dienst |
| SiliconFlow | 📋 | Cloud | SiliconFlow KI-Dienst |
| MiniMax | 📋 | Cloud | MiniMax KI-Dienst |
| OpenAI | 💡 | Cloud | OpenAI API-Dienst (GPT-Serie) |
| Anthropic | 💡 | Cloud | Anthropic Claude KI-Dienst |
| Google DeepMind | 💡 | Cloud | Google Gemini KI-Dienst |
| Mistral AI | 💡 | Cloud | Mistral AI Dienst |
| Groq | 💡 | Cloud | Groq Hochgeschwindigkeits-KI-Inferenzdienst |
| Together AI | 💡 | Cloud | Together AI Open-Source-Modelldienst |
| xAI | 💡 | Cloud | xAI Grok Dienst |
| Cohere | 💡 | Cloud | Cohere Enterprise NLP-Dienst |
| Replicate | 💡 | Cloud | Replicate Open-Source-Modell-Hosting-Plattform |
| Hugging Face | 💡 | Cloud | Hugging Face Open-Source-KI-Community und Modellplattform |
| Cerebras | 💡 | Cloud | Cerebras KI-Inferenz-Optimierungsdienst |
| Databricks | 💡 | Cloud | Databricks Enterprise-KI-Plattform (MosaicML) |
| Perplexity AI | 💡 | Cloud | Perplexity AI Such- und Frage-Antwort-Dienst |
| NVIDIA NIM | 💡 | Cloud | NVIDIA KI-Inferenz-Mikrodienst |

---

## Wichtige Designentscheidungen

### Speicherung als Instanzklasse (nicht statisch)

`IStorage` ist als injizierbare Instanz konzipiert, nicht als statisches Hilfsmittel. Dies stellt sicher:

- Direkter Dateisystemzugriff — IStorage ist der interne Persistenzkanal des Systems und wird **nicht** über Executors geleitet.
- **KI hat keine Kontrolle über IStorage** — Executors verwalten IO, das von KI-Werkzeugen initiiert wird; IStorage verwaltet die internen Datenlese- und schreibvorgänge des Frameworks. Dies sind grundlegend unterschiedliche Belange.
- Testbarkeit mit Mock-Implementierungen.
- Zukünftige Unterstützung unterschiedlicher Speicher-Backends ohne Änderung der Konsumenten.

### Executor als Sicherheitsgrenze

Executors sind der **einzige** Weg für I/O-Operationen. Werkzeuge, die Festplatten-, Netzwerk- oder Kommandozeilenzugriff benötigen, **müssen** über Executors gehen. Dieses Design erzwingt:

- Jeder Executor hat einen **eigenen Scheduler-Thread** mit Thread-Locking zur Berechtigungsvalidierung.
- Zentrale Berechtigungsprüfung — Executors fragen den **privaten Berechtigungsmanager** des Wesens ab.
- Anfragewarteschlange mit Prioritäts- und Timeout-Unterstützung.
- Audit-Log für alle externen Operationen.
- Ausnahmeisolation — Der Ausfall eines Executors beeinflusst andere Executors nicht.
- Circuit Breaker — Aufeinanderfolgende Fehler stoppen den Executor vorübergehend, um Kaskadenausfälle zu verhindern.

### ContextManager als leichtgewichtiges Objekt

Bei jedem `ExecuteOneRound()` wird eine neue `ContextManager`-Instanz erstellt:

1. Soul-Datei + letzten Chat-Verlauf laden.
2. Anfrage an den KI-Client senden.
3. Werkzeugaufrufe in einer Schleife verarbeiten, bis die KI reinen Text zurückgibt.
4. Antwort im Chat-System persistieren.
5. Freigeben.

Dies hält jede Runde isoliert und zustandslos.

### Selbst-Evolution durch Klassen-Überschreibung

Silicon Beings können ihre eigenen C#-Klassen zur Laufzeit überschreiben:

1. Die KI generiert neuen Klassencode (muss von `SiliconBeingBase` erben).
2. **Kompilierzeit-Referenzkontrolle** (primäre Verteidigung): Der Compiler erhält nur die erlaubte Assembly-Liste — `System.IO`, `System.Reflection` usw. werden ausgeschlossen, sodass gefährlicher Code auf Typebene unmöglich ist.
3. **Laufzeit-Statische Analyse** (sekundäre Verteidigung): Der `SecurityScanner` scannt den Code nach der erfolgreichen Kompilierung auf gefährliche Muster.
4. Roslyn kompiliert den Code im Speicher.
5. Bei Erfolg: `SiliconBeingManager.ReplaceBeing()` tauscht die aktuelle Instanz aus, migriert den Zustand und persistiert den verschlüsselten Code auf der Festplatte.
6. Bei Fehlschlag: Der neue Code wird verworfen, die bestehende Implementierung bleibt erhalten.

Eine benutzerdefinierte `IPermissionCallback`-Implementierung kann ebenfalls über `ReplacePermissionCallback()` kompiliert und injiziert werden, wodurch Wesen ihre eigene Berechtigungslogik anpassen können.

Der Code wird auf der Festplatte AES-256-verschlüsselt gespeichert. Der Verschlüsselungsschlüssel wird aus der GUID des Wesens (Großbuchstaben) über PBKDF2 abgeleitet.

---

## Token-Nutzungsaudit

Der `TokenUsageAuditManager` verfolgt den KI-Token-Verbrauch aller Wesen:

- `TokenUsageRecord` — Datensatz pro Anfrage (Wesen-ID, Modell, Prompt-Token, Vervollständigungs-Token, Zeitstempel)
- `TokenUsageSummary` — Aggregierte Statistiken
- `TokenUsageQuery` — Abfrageparameter zum Filtern von Datensätzen
- Persistiert über `ITimeStorage` für Zeitreihenabfragen
- Zugänglich über die Web-UI (UsageController) und das `TokenAuditTool` (nur Curator)

---

### Kalendersystem

Das System enthält **32 Kalenderimplementierungen**, abgeleitet von der abstrakten `CalendarBase`-Klasse, die die wichtigsten Kalendersysteme der Welt abdecken:

| Kalender | ID | Beschreibung |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | Buddhistischer Kalender (BE), Jahr + 543 |
| CherokeeCalendar | `cherokee` | Cherokee-Kalendersystem |
| ChineseLunarCalendar | `lunar` | Chinesischer Mondkalender mit Schaltmonaten |
| ChineseHistoricalCalendar | `chinese_historical` | Chinesischer historischer Kalender, unterstützt Ganzhi-Ära- und Kaiser-Ära-Systeme |
| ChulaSakaratCalendar | `chula_sakarat` | Chula-Sakarat-Kalender (CS), Jahr - 638 |
| CopticCalendar | `coptic` | Koptischer Kalender |
| DaiCalendar | `dai` | Dai-Kalender mit vollständiger Mondkalenderberechnung |
| DehongDaiCalendar | `dehong_dai` | Dehong-Dai-Kalendervariante |
| EthiopianCalendar | `ethiopian` | Äthiopischer Kalender |
| FrenchRepublicanCalendar | `french_republican` | Französischer Republikanischer Kalender |
| GregorianCalendar | `gregorian` | Standard-Gregorianischer Kalender |
| HebrewCalendar | `hebrew` | Hebräischer (Jüdischer) Kalender |
| IndianCalendar | `indian` | Indischer Nationalkalender |
| InuitCalendar | `inuit` | Inuit-Kalendersystem |
| IslamicCalendar | `islamic` | Islamischer Hidschra-Kalender |
| JapaneseCalendar | `japanese` | Japanischer Nengo-Kalender |
| JavaneseCalendar | `javanese` | Javanischer Islamischer Kalender |
| JucheCalendar | `juche` | Juche-Kalender (Nordkorea), Jahr - 1911 |
| JulianCalendar | `julian` | Julianischer Kalender |
| KhmerCalendar | `khmer` | Khmer-Kalender |
| MayanCalendar | `mayan` | Maya-Langzählkalender |
| MongolianCalendar | `mongolian` | Mongolischer Kalender |
| PersianCalendar | `persian` | Persischer (Solar-Hidschra-) Kalender |
| RepublicOfChinaCalendar | `roc` | Republik-China-Kalender, Jahr - 1911 |
| RomanCalendar | `roman` | Römischer Kalender |
| SakaCalendar | `saka` | Saka-Kalender (Indonesien) |
| SexagenaryCalendar | `sexagenary` | Chinesischer Ganzhi-Kalender |
| TibetanCalendar | `tibetan` | Tibetischer Kalender |
| VietnameseCalendar | `vietnamese` | Vietnamesischer Mondkalender (Katzen-Tierkreis-Variante) |
| VikramSamvatCalendar | `vikram_samvat` | Vikram-Samvat-Kalender |
| YiCalendar | `yi` | Yi-Kalendersystem |
| ZoroastrianCalendar | `zoroastrian` | Zoroastrischer Kalender |

Das `CalendarTool` bietet Operationen: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (kalenderübergreifende Datumskonvertierung).

---

## Web-UI-Architektur

### Skin-System

Die Web-UI verfügt über ein **steckbares Skin-System**, das eine vollständige UI-Anpassung ohne Änderung der Anwendungslogik ermöglicht:

- **ISkin-Schnittstelle** — Definiert den Vertrag für alle Skins, einschließlich:
  - Kern-Rendering-Methoden (`RenderHtml`, `RenderError`)
  - 20+ UI-Komponentenmethoden (Schaltflächen, Eingaben, Karten, Tabellen, Badges, Blasen, Fortschritt, Tabs usw.)
  - Theming-CSS-Generierung über `CssBuilder`
  - `SkinPreviewInfo` — Farbpalette und Icons für den Initialisierungsseiten-Skin-Selektor

- **Integrierte Skins** — 7 produktionsreife Skins:
  - **Admin** — Professionelles, datenfokussiertes Systemverwaltungsinterface
  - **Chat** — Konversationelles, nachrichtenorientiertes Design für KI-Interaktion
  - **Creative** — Künstlerisches, visuell reichhaltiges Layout für kreative Workflows
  - **Dev** — Entwicklerorientiertes, code-zentriertes Interface mit Syntaxhervorhebung
  - **HighContrast** — Hoher-Kontrast-Barrierefreiheits-Theme
  - **Light** — Frisches Hell-Theme
  - **Minimal** — Minimalistisches Theme

- **Skin-Entdeckung** — Der `SkinManager` entdeckt und registriert automatisch alle `ISkin`-Implementierungen über Reflexion

### HTML / CSS / JS-Builder

Die Web-UI vermeidet Template-Dateien vollständig und generiert das gesamte Markup in C#:

- **`H`** — Fließender HTML-Builder-DSL zum Erstellen von HTML-Bäumen im Code
- **`CssBuilder`** — CSS-Builder mit Selektor- und Media-Query-Unterstützung
- **`JsBuilder`** (`JsSyntax`) — JavaScript-Builder für Inline-Skripte

### Controller-System

Die Web-UI folgt einem **MVC-ähnlichen Muster** mit 24 Controllern für verschiedene Aspekte:

| Controller | Zweck |
|------------|---------|
| About | Info-Seite und Projektinformationen |
| Audit | Token-Nutzungsaudit-Dashboard |
| Being | Silicon Being Verwaltung und Status |
| Chat | Echtzeit-Chat-Oberfläche mit SSE |
| ChatHistory | Chat-Verlaufsansicht mit Sitzungsliste und Nachrichtendetails |
| CodeBrowser | Code-Anzeige und -Bearbeitung |
| CodeHover | Code-Hover-Tooltips mit Syntaxhervorhebung |
| Config | Systemkonfigurationsverwaltung |
| Dashboard | Systemübersicht und Metriken |
| Executor | Executor-Status und Verwaltung |
| Help | Hilfedokumentationssystem, Mehrsprachunterstützung |
| Init | Erstausführungs-Initialisierungsassistent |
| Knowledge | Wissensgraph-Visualisierung und Abfrage |
| Log | System-Log-Viewer mit Silicon-Being-Filter |
| Memory | Langzeit-Speicher-Browser mit erweitertem Filter, Statistiken und Detailansicht |
| Permission | Berechtigungsverwaltung |
| PermissionRequest | Berechtigungsanfragewarteschlange |
| Project | Projektverwaltung mit Arbeitsnotizen, Aufgabensystem und Werkzeugberechtigungen |
| System | Systemverwaltung und Laufzeitüberwachung |
| Task | Aufgabensystem-Oberfläche |
| Timer | Timer-Systemverwaltung mit Ausführungsverlauf |
| ToolPermission | Werkzeugberechtigungsverwaltung mit Silicon-Being- und Projektebenen-Berechtigungskonfiguration |
| Usage | Token-Nutzungsaudit-Dashboard mit Trenddiagrammen und Export |
| WorkNote | Arbeitsnotizverwaltung mit Suche und Verzeichnisgenerierung |

### Echtzeit-Updates

- **SSE (Server-Sent Events)** — Über `SSEHandler` werden Updates für Chat-Nachrichten, Wesen-Status und Systemereignisse gepusht
- **Kein WebSocket erforderlich** — Einfachere Architektur, die SSE für die meisten Echtzeit-Anforderungen verwendet
- **Automatische Wiederverbindung** — Client-Reconnect-Logik für resiliente Verbindungen

### Lokalisierung

Das System unterstützt umfassende Lokalisierung in **34 Sprachvarianten**:
- **Chinesisch (6 Varianten)**: zh-CN (Vereinfacht), zh-HK (Traditionell), zh-SG (Singapur), zh-MO (Macau), zh-TW (Taiwan), zh-MY (Malaysia)
- **Englisch (10 Varianten)**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **Spanisch (2 Varianten)**: es-ES, es-MX
- **Deutsch (5 Varianten)**: de-DE, de-AT, de-CH, de-LU, de-LI
- **Französisch (3 Varianten)**: fr-FR, fr-CA, fr-CH
- **Weitere (8 Varianten)**: ja-JP (Japanisch), ko-KR (Koreanisch), cs-CZ (Tschechisch), it-IT (Italienisch), pl-PL (Polnisch), pt-PT (Portugiesisch), pt-BR (Brasilianisches Portugiesisch), ru-RU (Russisch)

Die aktive Sprachumgebung wird über `DefaultConfigData.Language` ausgewählt und über den `Lokalisierungsmanager` aufgelöst.

---

### WebView-Browser-Automatisierungssystem (Neu)

Das System integriert **Playwright**-basierte WebView-Browser-Automatisierung:

- **Individuelle Isolierung**: Jedes Silicon Being besitzt eine separate Browserinstanz, Cookies und Sitzungsspeicher, vollständig isoliert und unabhängig voneinander.
- **Headless-Modus**: Der Browser läuft in einem für den Benutzer vollständig unsichtbaren Headless-Modus; Silicon Beings operieren autonom im Hintergrund.
- **WebViewBrowserTool**: Bietet vollständige Browser-Bedienungsfähigkeiten, einschließlich:
  - Seitennavigation, Klicken, Texteingabe, Seiteninhalt abrufen
  - JavaScript ausführen, Screenshots erstellen, auf das Erscheinen von Elementen warten
  - Browser-Statusverwaltung und Ressourcenbereinigung
- **Sicherheitskontrolle**: Alle Browseroperationen müssen die Berechtigungsvalidierungskette durchlaufen, um bösartige Webseitenzugriffe zu verhindern.

### Wissensnetzwerk-System (Neu)

Das System verfügt über ein integriertes **Tripel-Struktur**-basiertes Wissensgraphsystem:

- **Wissensrepräsentation**: Verwendet eine "Subjekt-Relation-Objekt"-Tripelstruktur (z. B.: Python-is_a-programming_language)
- **KnowledgeTool**: Bietet vollständigen Lebenszyklus-Management des Wissens:
  - `add`/`query`/`update`/`delete` - Grundlegende CRUD-Operationen
  - `search` - Volltextsuche und Schlüsselwortabgleich
  - `get_path` - Entdeckt Verbindungspfade zwischen zwei Konzepten
  - `validate` - Wissensvollständigkeitsprüfung
  - `stats` - Statistische Analyse des Wissensnetzwerks
- **Persistente Speicherung**: Wissenstripel werden im Dateisystem persistiert und unterstützen zeitindizierte Abfragen.
- **Konfidenzbewertung**: Jeder Wissenseintrag hat eine Konfidenzbewertung (0-1), die Fuzzy-Matching und Rangordnung von Wissen unterstützt.
- **Tag-Klassifizierung**: Unterstützt das Hinzufügen von Tags zu Wissen zur Kategorisierung und zum Abruf.

---

## Datenverzeichnisstruktur

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # Soul-Datei des Curators
    │   ├── state.json       # Laufzeitstatus
    │   ├── code.enc         # AES-verschlüsselter benutzerdefinierter Klassencode
    │   └── permission.enc   # AES-verschlüsselter benutzerdefinierter Berechtigungs-Callback
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```

---

## SpeedyPack-Speicher-Engine

SiliconLife.Fast verwendet die selbstentwickelte SpeedyPack-Speicher-Engine (.spk-Format), die die vorherige LiteDB-Lösung ersetzt und extreme Lese- und Schreibleistung ermöglicht.

### Architekturdesign

```
┌──────────────────────────────────────────────────────────┐
│                    SpeedyPack                             │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────────┐  │
│  │ DirectoryMap  │  │  EntryCache   │  │  WriteQueue   │  │
│  │ (Verzeichnis- │  │ (Eintrags-   │  │ (Asynchrone   │  │
│  │  abbildung)   │  │  Cache)      │  │  Schreib-     │  │
│  │              │  │              │  │  warteschlange)│  │
│  └──────┬───────┘  └──────┬───────┘  └───────┬───────┘  │
│         │                  │                   │          │
│  ┌──────▼──────────────────▼───────────────────▼───────┐  │
│  │              PackFileReader / PackFileWriter          │  │
│  │              (Pack-Datei-Leser/Schreiber)             │  │
│  └──────────────────────────┬──────────────────────────┘  │
│                              │                             │
│  ┌──────────────────────────▼──────────────────────────┐  │
│  │              .spk-Datei (MessagePack + LZ4-Komprim.)  │  │
│  └─────────────────────────────────────────────────────┘  │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐                      │
│  │  FreeList     │  │ SpeedyPack   │                      │
│  │ (Freeliste)   │  │ AutoCompactor│                      │
│  │              │  │ (Auto-       │                      │
│  │              │  │  Kompaktor)  │                      │
│  └──────────────┘  └──────────────┘                      │
└──────────────────────────────────────────────────────────┘
```

### Kernkomponenten

| Komponente | Beschreibung |
|------|------|
| `SpeedyPack` | Kernklasse, kombiniert DirectoryMap, EntryCache und WriteQueue für Latenz-armes Lesen/Schreiben |
| `DirectoryMap` | Verzeichnisabbildung im Speicher, pflegt die Zuordnung von virtuellen Pfaden zu Dateieinträgen |
| `EntryCache` | Eintrags-Cache, TTL-basierter Cache für kürzlich zugegriffene Einträge |
| `WriteQueue` | Asynchrone Schreibwarteschlange, reiht Schreiboperationen zur Ausführung im Hintergrund-Thread ein |
| `FreeList` | Freelist-Verwaltung, verfolgt wiederverwendbaren Speicherplatz in .spk-Dateien |
| `PackFileReader` | Pack-Datei-Leser, liest Daten aus .spk-Dateien |
| `PackFileWriter` | Pack-Datei-Schreiber, schreibt Daten in .spk-Dateien |
| `SpeedyPackAutoCompactor` | Automatischer Komprimierungs-Timer, komprimiert .spk-Dateien regelmäßig, um freien Speicherplatz zurückzugewinnen |
| `SpeedyPackRegistry` | Prozessweiter Singleton-Manager, stellt sicher, dass die gesamte Anwendung dieselbe SpeedyPack-Instanz verwendet |

### Speicheradapter

SiliconLife.Fast integriert SpeedyPack über folgende Adapter in die Systemschnittstellen:

| Adapter | Schnittstelle | Beschreibung |
|--------|------|------|
| `SpeedyStorage` | `IStorage` | Allgemeiner Schlüssel-Wert-Speicheradapter |
| `SpeedyTimeStorage` | `ITimeStorage` | Zeitindizierter Speicheradapter |
| `SpeedyWorkNoteStorage` | `IWorkNoteStorage` | Arbeitsnotiz-Speicheradapter |

### Konfigurationsoptionen

`SpeedyPackOptions` bietet folgende Konfiguration:

| Option | Typ | Standardwert | Beschreibung |
|------|------|--------|------|
| `CacheTtl` | `TimeSpan` | 5 Minuten | Gültigkeitsdauer der Cache-Einträge |
| `MaxCacheEntries` | `int` | 1000 | Maximale Anzahl der Cache-Einträge |
| `ReadOnly` | `bool` | false | Nur-Lese-Modus |

### Transaktionsunterstützung

SpeedyPack unterstützt atomare Schreiboperationen über die `IPackTransaction`-Schnittstelle:

- `SpeedyTransaction` implementiert den Transaktionsmechanismus
- Unterstützt Atomarität von Stapelschreibvorgängen
- Beim Transaktionscommit werden entweder alle Schreibvorgänge erfolgreich abgeschlossen oder alle zurückgerollt

---

## Plugin-System

SiliconLife unterstützt Funktionserweiterungen über ein Plugin-System, das Drittanbietern ermöglicht, neue Funktionen zur Plattform hinzuzufügen.

### Kernschnittstelle

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### Plugin-Lader

Der `PluginLoader` ist verantwortlich für das Laden von Plugin-DLLs aus einem angegebenen Verzeichnis und führt Sicherheitsprüfungen mit Fähigkeitsdeklaration durch:

1. **Verzeichnisscan** — Scannt alle .dll-Dateien im Plugin-Verzeichnis
2. **Fähigkeitsprüfung** — Prüft die über `[PluginCapability]` deklarierten Fähigkeiten und lockert die Sicherheitsregeln entsprechend
3. **Isoliertes Laden** — Verwendet einen benutzerdefinierten `AssemblyLoadContext` zum isolierten Laden von Plugins
4. **Lebenszyklusverwaltung** — Ruft die Methoden OnLoad, OnStart, OnStop, OnUnload des Plugins auf

### Sicherer Sandkasten

Der Plugin-Lader führt folgende Sicherheitsprüfungen durch:

| Prüfungsgegenstand | Beschreibung |
|--------|------|
| Verbotene Namespaces | System.IO, System.Net.Http, System.Net.WebSockets, System.Net.Sockets, Microsoft.CodeAnalysis |
| Vertrauenswürdige Assembly-Whitelist | Google.Protobuf, Newtonsoft.Json, MessagePack, Serilog, Microsoft.Extensions.Logging.Abstractions, Dapper |
| Verbotene Typ-Prüfung | Scannt gefährliche Typen, die im Plugin referenziert werden |
| Verbotene Member-Prüfung | Scannt gefährliche Methoden, die im Plugin aufgerufen werden |

### Werkzeugintegration

Plugins können benutzerdefinierte Werkzeuge über die `ITool`-Schnittstelle registrieren:

- Die Methode `ToolManager.ScanAllPluginAssemblies()` scannt alle geladenen Plugins nach ITool-Implementierungen
- Plugin-Werkzeuge werden automatisch in den Werkzeugaufruf-Zyklus integriert
- Plugin-Werkzeuge unterliegen demselben Berechtigungssystem

### Plugin-Lebenszyklus

```
Laden (OnLoad) → Starten (OnStart) → Laufend → Stoppen (OnStop) → Entladen (OnUnload)
```

---

## Silicon Being Aktivitätszustände

Silicon Beings haben folgende Aktivitätszustände:

| Zustand | Beschreibung |
|------|------|
| `Idle` | Leerlauf, wartet auf Takt-Auslösung |
| `SingleChat` | Führt einen Einzelchat durch |
| `GroupChat` | Führt einen Gruppenchat durch |
| `Task` | Führt eine Aufgabe aus |
| `Timer` | Führt einen Timer aus |
| `Stopped` | Gestoppt, aufgrund aufeinanderfolgender Fehler oder manuellem Stopp |

**Stopped-Zustandsmechanismus**:
- Wenn ein Silicon Being 10 aufeinanderfolgende Fehler verursacht, wechselt es automatisch in den `Stopped`-Zustand
- Im Stopped-Zustand führt das Wesen keine Aufgaben mehr aus
- Wenn eine neue Chat-Nachricht eintrifft, wird der Fehlerzähler zurückgesetzt und das Wesen nimmt den Betrieb wieder auf

Zustandsübergänge:
```
Idle → SingleChat → Idle (Chat abgeschlossen)
Idle → GroupChat → Idle (Gruppenchat abgeschlossen)
Idle → Task → Idle (Aufgabe abgeschlossen)
Idle → Timer → Idle (Timer abgeschlossen)
Beliebig → Stopped (10 aufeinanderfolgende Fehler)
Stopped → Idle (Neue Chat-Nachricht eingetroffen oder manueller Neustart)
```

---

## Workflow-Engine

Die Workflow-Engine ist ein vorlagenbasiertes Zustandsmaschinensystem zur Steuerung der Kollaborationsprozesse von Silicon Beings im Projektraum:

### Kernkomponenten

| Komponente | Beschreibung |
|------|------|
| `WorkflowEngine` | Workflow-Engine-Kern, verwaltet Vorlagen und Instanzen, führt Tick-gesteuerte Zustandsübergänge aus |
| `WorkflowTemplate` | Workflow-Vorlage, definiert Zustandsmengen und Übergangsregeln |
| `WorkflowInstance` | Workflow-Instanz, an ein konkretes Projekt gebunden, verfolgt den aktuellen Zustand |
| `WorkflowLog` | Workflow-Log, protokolliert die Zustandsübergangshistorie |

### Funktionsweise

- **Vorlagenregistrierung**: Workflow-Vorlagen werden über `RegisterTemplate()` registriert, wobei Zustände und Übergangsregeln definiert werden
- **Instanzerstellung**: Aus einer Vorlage wird eine Instanz erstellt und an einen Projektraum gebunden
- **Tick-gesteuert**: Zustandsübergänge werden durch den Tick-Mechanismus der Hauptschleife gesteuert
- **Protokollierung**: Alle Zustandsübergänge werden automatisch im Log erfasst

---

## Gedächtnis-Verblass-Mechanismus

Der `MemoryFadeService` ist ein zeitgesteuerter Zerfallsdienst, der das Vergessen biologischer Gedächtnisse simuliert:

### Funktionsweise

- **Zeitgesteuerte Ausführung**: Erbt von `TickObject`, führt standardmäßig stündlich einen Zerfallszyklus aus
- **Wichtigkeitszerfall**: Wendet einen Zerfallsalgorithmus auf die Gedächtniseinträge jedes Silicon Beings an und senkt die Wichtigkeitsbewertung
- **Automatische Archivierung**: Gedächtniseinträge unterhalb des Schwellenwerts werden automatisch archiviert (`ArchiveFadingMemories()`)
- **Statistik-Tracking**: Erfasst Zerfallszyklen, Anzahl der Zustandsänderungen und weitere Statistiken

### Zerfallsprozess

```
MemoryFadeService.OnTick()
  └── Iteriert über alle Silicon Beings
       └── being.Memory.ApplyDecay()      # Wichtigkeitszerfall anwenden
       └── being.Memory.ArchiveFadingMemories()  # Gedächtniseinträge mit geringer Wichtigkeit archivieren
```

---

## Projektarbeitsbereich-System

Der Projektarbeitsbereich ist ein Raumverwaltungsmechanismus, der die Kollaboration mehrerer Silicon Beings unterstützt:

### Kernfunktionen

- **Projektlebenszyklus**: Erstellung → Aktiv → Archiviert → Zerstört
- **Rollenverteilung**: Unterstützt die Zuweisung von Projektrollen an Silicon Beings
- **Werkzeugberechtigungs-Isolierung**: Werkzeugberechtigungskonfiguration auf Projektebene, unabhängig von den Berechtigungen auf Silicon-Being-Ebene
- **Arbeitsnotizen**: Seitenbasiertes Notizsystem im Projektraum mit Verzeichnisgenerierung und Schlüsselwortsuche
- **Aufgabenverfolgung**: Projektweites Aufgabenmanagement mit Erstellung, Zuweisung und Statusverfolgung
- **Workflow-Integration**: Projekte können an Workflow-Vorlagen gebunden werden, die Kollaborationsprozesse steuern

### Zugehörige Werkzeuge

| Werkzeug | Zweck |
|------|------|
| `ProjectTool` | Projektraumverwaltung (Erstellung, Archivierung, Zerstörung, Rollenzuweisung) |
| `ProjectTaskTool` | Projektaufgabenverwaltung (Erstellung, Zuweisung, Statusaktualisierung) |
| `ProjectWorkNoteTool` | Projektarbeitsnotizen (Erstellung, Suche, Verzeichnisgenerierung) |
| `ProjectWorkTool` | Projektarbeitsoperationen (Aufgaben erstellen, Gruppenchat, Broadcast, Projekt abschließen) |
