# Architektur

> **Version: v0.2.0-alpha**

[English](../en/architecture.md) | **Deutsch** | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Čeština](../cs-CZ/architecture.md)

## Duale Versionsarchitektur

Dieses Projekt bietet zwei Implementierungsversionen, die dasselbe Architekturentwurf teilen, sich aber in Speicher und Leistungsoptimierung unterscheiden:

### SiliconLife.Default (Standardversion)
- **Positionierung**: Standardimplementierung, hauptsächlich für Architektur-Machbarkeitsverifizierung
- **Ausführungsmodus**: Konsolenanwendung
- **Speicher**: Reines Dateisystem-JSON-Speicher
- **Anwendungsszenario**: Hohe Datensicherheitsanforderungen, begrenzte Speicherressourcen, Szenarien mit kleinem Datenvolumen
- **Rollenbeschreibung**: Referenzimplementierung für Architekturverifizierung, bietet eine einfache und zuverlässige Ausführung, geeignet für ersten Kontakt, Entwicklungs-Debugging oder Szenarien mit Datensicherheitspriorität

### SiliconLife.Fast (Hochleistungsversion)
- **Positionierung**: Haupt-Produktionsversion
- **Ausführungsmodus**: Desktop-Anwendung (Windows/macOS System Tray / Linux Statusfenster)
- **Speicher**: SpeedyPack-In-Memory-Speicher + asynchrone Batch-Persistenz (.spk-Dateiformat)
- **Anwendungsszenario**: Szenarien mit hoher Parallelität, niedriger Latenz, großem Datenvolumen
- **Plattformunterstützung**: Windows (volle Funktionen, inkl. System Tray), Linux (Statusfenster, kein Tray-Icon)
- **Merkmale**:
  - Windows/macOS System Tray-Hintergrundausführung mit Echtzeitüberwachung über Tray-Statusfenster; Linux Statusfenster direkt angezeigt
  - SpeedyPack-Engine + automatische Komprimierung gewährleisten Datensicherheit
  - Component-UI-Architektur, 30+ deklarative Komponenten
  - 7 Skin-Themes, unterstützt automatische Erkennung und Umschaltung
  - Hot-Reload-Tool für Online-Updates und Neustarts
  - Linux öffnet automatisch den Browser für Web-UI-Zugriff, unterstützt `--no-tray` Parameter
- **Leistungsverbesserung**: Speicherlese-Latenz um das 1000-fache reduziert, Schreiblatenz um das 15000-fache reduziert
- **Rollenbeschreibung**: Produktionsreife Implementierung mit tiefer Optimierung, mit System Tray-Hintergrundausführung, SpeedyPack-Engine + automatische Komprimierung, die beste Wahl für Langzeitbetrieb und echte Produktionsumgebungen

> **Hinweis**: Die in diesem Dokument beschriebene Architektur gilt für beide Versionen, mit Unterschieden nur im Speicherimplementierungsteil. SiliconLife.Default ist die Referenz für Architekturverifizierung, SiliconLife.Fast ist die Hauptversion für Produktionsumgebungen.

---

## Kernkonzepte

### Silicon Being

Jeder KI-Agent im System ist ein **Silicon Being** — eine autonome Entität mit eigener Identität, Persönlichkeit und Fähigkeiten. Jedes Silicon Being wird durch eine **Soul-Datei** (Markdown-Prompt) gesteuert, die seine Verhaltensmuster definiert.

### Silicon Curator

Der **Silicon Curator** ist ein spezielles Silicon Being mit höchsten Systemberechtigungen. Er fungiert als Systemadministrator:

- Erstellt und verwaltet andere Silicon Beings
- Analysiert Benutzeranfragen und zerlegt sie in Aufgaben
- Verteilt Aufgaben an geeignete Silicon Beings
- Überwacht Ausführungsqualität und behandelt Fehler
- Antwortet auf Benutzernachrichten mit **Prioritäts-Scheduling** (siehe unten)

### Soul-Datei

Eine Markdown-Datei (`soul.md`), gespeichert im Datenverzeichnis jedes Silicon Beings. Sie wird als System-Prompt in jede KI-Anfrage injiziert und definiert die Persönlichkeit, Entscheidungsmuster und Verhaltensbeschränkungen des Beings.

---

## Scheduling: Time-Slice Fair Scheduling

### Hauptschleife + Clock-Objekte

Das System betreibt eine **Clock-gesteuerte Hauptschleife** auf einem dedizierten Hintergrund-Thread:

```
Hauptschleife (dedizierter Thread, Watchdog + Circuit Breaker)
  └── Clock-Objekt A (Priorität=0, Intervall=100ms)
  └── Clock-Objekt B (Priorität=1, Intervall=500ms)
  └── Silicon Being Manager (direkt durch Hauptschleife clock-getriggert)
        └── Silicon Being Runner → Silicon Being 1 → Clock-Trigger → Eine Runde ausführen
        └── Silicon Being Runner → Silicon Being 2 → Clock-Trigger → Eine Runde ausführen
        └── Silicon Being Runner → Silicon Being 3 → Clock-Trigger → Eine Runde ausführen
        └── ...
```

Wichtige Design-Entscheidungen:

- **Silicon Beings erben keine Clock-Objekte.** Sie haben eigene `Tick()`-Methoden, die von `SiliconBeingManager` durch `SiliconBeingRunner` aufgerufen werden, statt direkt in der Hauptschleife registriert zu sein.
- **Silicon Being Manager** wird direkt durch die Hauptschleife clock-getriggert und dient als einzelner Proxy für alle Beings.
- **Silicon Being Runner** wrapped jedes Being's `Tick()` auf einem temporären Thread mit Timeout und pro-Being Circuit Breaker (3 aufeinanderfolgende Timeouts → 1 Minute Abkühlung).
- Jedes Being's Ausführung ist auf **eine Runde** KI-Anfrage + Tool-Aufrufe pro Clock-Trigger beschränkt, stellt sicher, dass kein Being die Hauptschleife monopolisieren kann.
- **Leistungsmonitor** trackt Clock-Ausführungszeiten für Beobachtbarkeit.

### Curator-Prioritätsantwort

Wenn ein Benutzer eine Nachricht an den Silicon Curator sendet:

1. Aktuelles Being (z.B. Being A) beendet seine aktuelle Runde — **keine Unterbrechung**.
2. Manager **überspringt verbleibende Warteschlange**.
3. Schleife **startet neu beim Curator**, ermöglicht sofortige Ausführung.

Dies gewährleistet Reaktion auf Benutzerinteraktionen ohne Beeinträchtigung laufender Aufgaben.

---

## Komponentearchitektur

```
┌─────────────────────────────────────────────────────────┐
│                        Core Host                         │
│  (Unified Host — assembliert und verwaltet alle Komponenten) │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ Hauptschleife│ │ServiceLocator│ │      Konfig        │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │     Silicon Being Manager (Clock-Objekt)           │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Curator    │ │Being A  │ │Being B  │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Shared Services                       │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ChatSystem│  │ Storage  │  │PermissionManager  │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │AI-Client │  │Executors │  │   ToolManager     │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  │  ┌──────────┐ ┌──────────┐                        │   │
│  │  │Plugin-   │ │Wissens-  │                        │   │
│  │  │Loader    │ │netzwerk  │                        │   │
│  │  └──────────┘ └──────────┘                        │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Executoren                        │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │  Disk     │  │ Netzwerk │  │  Befehlszeile     │  │   │
│  │  │Executor   │  │Executor  │  │  Executor         │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              IM-Provider                           │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Konsole   │  │  Web     │  │  Feishu / ...     │  │   │
│  │  │Provider   │  │Provider  │  │  Provider          │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## Service Locator

`ServiceLocator` ist eine threadsichere Singleton-Registrierung, die Zugriff auf alle Kerndienste bietet:

| Eigenschaft | Typ | Beschreibung |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | Zentraler Chat-Sitzungsmanager |
| `IMManager` | `IMManager` | IM-Provider-Router |
| `AuditLogger` | `AuditLogger` | Berechtigungs-Audit-Trail |
| `GlobalAcl` | `GlobalACL` | Globale Zugriffssteuerungsliste |
| `BeingFactory` | `ISiliconBeingFactory` | Factory zum Erstellen von Beings |
| `BeingManager` | `SiliconBeingManager` | Lebenszyklusmanager aktiver Beings |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Dynamischer Kompilierungsloader |
| `TokenUsageAudit` | `ITokenUsageAudit` | Token-Nutzungsverfolgung |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Token-Nutzungsberichte |

Er verwaltet auch eine Registrierung von `PermissionManager` pro Being, keyiert nach Being-GUID.

---

## Chat-System

### Sitzungstypen

Das Chat-System unterstützt drei Sitzungstypen durch `SessionBase`:

| Typ | Klasse | Beschreibung |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | Eins-zu-eins-Gespräch zwischen zwei Teilnehmern |
| `GroupChat` | `GroupChatSession` | Multi-Teilnehmer-Gruppenchat |
| `Broadcast` | `BroadcastChannel` | Offener Kanal mit fester ID; Beings abonnieren dynamisch, empfangen nur Nachrichten nach Abonnement |

### Broadcast-Kanäle

`BroadcastChannel` ist ein spezieller Sitzungstyp für systemweite Ankündigungen:

- **Feste Kanal-IDs** — Im Gegensatz zu `SingleChatSession` und `GroupChatSession` sind Kanal-IDs bekannte Konstanten, nicht von Mitglieder-GUIDs abgeleitet.
- **Dynamisches Abonnement** — Beings abonnieren/deabonnieren zur Laufzeit; sie empfangen nur Nachrichten, die nach ihrem Abonnement veröffentlicht werden.
- **Ausstehende Nachrichtenfilterung** — `GetPendingMessages()` gibt nur Nachrichten zurück, die nach dem Abonnementzeitpunkt des Beings veröffentlicht und noch nicht gelesen wurden.
- **Verwaltet durch Chat-System** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### Chat-Nachrichten

Das `ChatMessage`-Modell enthält Felder für KI-Dialogkontext und Token-Verfolgung:

| Feld | Typ | Beschreibung |
|-------|------|-------------|
| `Id` | `Guid` | Eindeutiger Nachrichtenidentifikator |
| `SenderId` | `Guid` | Eindeutiger Identifikator des Senders |
| `ChannelId` | `Guid` | Kanal-/Konversationsidentifikator |
| `Content` | `string` | Nachrichteninhalt |
| `Timestamp` | `DateTime` | Wann die Nachricht gesendet wurde |
| `Type` | `MessageType` | Text, Bild, Datei oder Systembenachrichtigung |
| `ReadBy` | `List<Guid>` | IDs der Teilnehmer, die diese Nachricht gelesen haben |
| `Role` | `MessageRole` | KI-Dialogrolle (user, assistant, tool) |
| `ToolCallId` | `string?` | Tool-Aufruf-ID für Tool-Ergebnisnachrichten |
| `ToolCallsJson` | `string?` | Serialisierte Tool-Aufrufe JSON für Assistant-Nachrichten |
| `Thinking` | `string?` | KI's Chain-of-Thought-Überlegung |
| `PromptTokens` | `int?` | Anzahl Token im Prompt (Eingabe) |
| `CompletionTokens` | `int?` | Anzahl Token in der Vervollständigung (Ausgabe) |
| `TotalTokens` | `int?` | Gesamtzahl verwendeter Token (Eingabe + Ausgabe) |
| `FileMetadata` | `FileMetadata?` | Angehängte Dateimetadaten (wenn Nachricht Dateien enthält) |

### Chat-Nachrichtenwarteschlange

`ChatMessageQueue` ist ein threadsicheres Nachrichtenwarteschlangensystem zur Verwaltung der asynchronen Verarbeitung von Chat-Nachrichten:

- **Threadsicher** — Verwendet Sperrmechanismus für sicheren gleichzeitigen Zugriff
- **Asynchrone Verarbeitung** — Unterstützt asynchrones Enqueue und Dequeue von Nachrichten
- **Nachrichtensortierung** — Hält zeitliche Reihenfolge der Nachrichten
- **Batch-Operationen** — Unterstützt Batch-Abruf von Nachrichten

### Dateimetadaten

`FileMetadata` verwaltet Dateiinformationen, die an Chat-Nachrichten angehängt sind:

- **Dateiinformationen** — Dateiname, Größe, Typ, Pfad
- **Upload-Zeit** — Zeitstempel des Dateiuploads
- **Uploader** — ID des Benutzers oder Silicon Being, der die Datei hochgeladen hat

### Stream-Abbruchmanager

`StreamCancellationManager` bietet Abbruchmechanismus für KI-Streaming-Antworten:

- **Stream-Kontrolle** — Unterstützt Abbruch laufender KI-Streaming-Antworten
- **Ressourcenbereinigung** — Bereinigt ordnungsgemäß verwandte Ressourcen beim Abbruch
- **Nebenläufigkeitssicher** — Unterstützt Verwaltung mehrerer Streams gleichzeitig

### Chat-Verlaufsansicht

Neue Chat-Verlaufsansicht ermöglicht Benutzern das Durchsuchen historischer Konversationen von Silicon Beings:

- **Sitzungsliste** — Zeigt alle historischen Sitzungen
- **Nachrichtendetails** — Vollständige Nachrichtenhistorie anzeigen
- **Timeline-Ansicht** — Nachrichten in chronologischer Reihenfolge
- **API-Unterstützung** — Bietet RESTful API für Sitzungs- und Nachrichtendaten

---

## KI-Client-System

Das System unterstützt mehrere KI-Backends durch die `IAIClient`-Schnittstelle:

### OllamaClient

- **Typ**: Lokaler KI-Service
- **Protokoll**: Native Ollama HTTP API (`/api/chat`, `/api/generate`)
- **Funktionen**: Streaming, Tool-Aufrufe, lokales Modell-Hosting
- **Konfiguration**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (Alibaba Cloud Bailian)

- **Typ**: Cloud-KI-Service
- **Protokoll**: OpenAI-kompatible API (`/compatible-mode/v1/chat/completions`)
- **Authentifizierung**: Bearer-Token (API-Schlüssel)
- **Funktionen**: Streaming, Tool-Aufrufe, Reasoning-Inhalte (Chain-of-Thought), Multi-Region-Bereitstellung
- **Unterstützte Regionen**:
  - `beijing` — Nordchina 2 (Peking)
  - `virginia` — USA (Virginia)
  - `singapore` — Singapur
  - `hongkong` — Hongkong, China
  - `frankfurt` — Deutschland (Frankfurt)
- **Unterstützte Modelle** (durch API dynamisch entdeckt, mit Fallback-Liste):
  - **Qwen-Serie**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **Reasoning**: qwq-plus
  - **Drittanbieter**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **Konfiguration**: `apiKey`, `region`, `model`
- **Modell-Entdeckung**: Runtime-Abruf verfügbarer Modelle von Bailian API; Fallback auf kuratierte Liste bei Netzwerkfehlern

### VolcengineArkClient (Volcengine Ark)

- **Typ**: Cloud-KI-Service
- **Protokoll**: OpenAI-kompatible API
- **Authentifizierung**: Bearer-Token (API-Schlüssel)
- **Funktionen**: Unterstützt Streaming- und Nicht-Streaming-Modi, integrierte doppelte Ratenbegrenzung
  - Clientseitige Ratenbegrenzung: Erzwingt minimales Intervall zwischen Anfragen
  - Server-Ratenbegrenzung: Handhabt 429-Fehler, Wiederholungen mit exponentiellem Backoff
- **Konfiguration**: `apiKey`, `endpoint`, `model`
- **Merkmale**: ByteDances KI-Service, unterstützt verschiedene Doubao-Modelle

### Client-Factory-Pattern

Jeder KI-Client-Typ hat entsprechende Factory-Implementierungen von `IAIClientFactory`:

- `OllamaClientFactory` — Erstellt OllamaClient-Instanzen
- `DashScopeClientFactory` — Erstellt DashScopeClient-Instanzen
- `VolcengineArkClientFactory` — Erstellt VolcengineArkClient-Instanzen

Factorys bieten:
- `CreateClient(Dictionary<string, object> config)` — Instanziiert Client aus Konfiguration
- `GetConfigKeyOptions(string key, ...)` — Gibt dynamische Optionen für Konfigurations-Schlüssel zurück (z.B. verfügbare Modelle, Regionen)
- `GetDisplayName()` — Lokalisierter Anzeigename des Client-Typs

### KI-Plattform-Support-Matrix

#### Statusbeschreibung
- ✅ Implementiert
- 🚧 In Entwicklung
- 📋 Geplant
- 💡 In Betrachtung

*Hinweis: Aufgrund der Netzwerkumgebung des Entwicklers kann der Zugriff auf [In Betrachtung] ausländische Cloud-KI-Dienste Netzwerk-Proxy-Tools erfordern, Debugging kann instabil sein.*

#### Plattformliste

| Plattform | Status | Typ | Beschreibung |
|------|------|------|------|
| Ollama | ✅ | Lokal | Lokaler KI-Service, unterstützt lokale Modellbereitstellung |
| DashScope (Alibaba Cloud Bailian) | ✅ | Cloud | Alibaba Cloud Bailian KI-Service, unterstützt Multi-Region-Bereitstellung |
| Baidu Qianfan (Wenxin Yiyan) | 📋 | Cloud | Baidu Wenxin Yiyan KI-Service |
| Zhipu AI (GLM) | 📋 | Cloud | Zhipu Qingyan KI-Service |
| Moonshot (Kimi) | 📋 | Cloud | Moonshot Kimi KI-Service |
| Volcano Ark Engine.Doubao | ✅ | Cloud | ByteDance Doubao KI-Service |
| DeepSeek (Direktverbindung) | 📋 | Cloud | DeepSeek KI-Service |
| 01.AI | 📋 | Cloud | 01.AI KI-Service |
| Tencent Hunyuan | 📋 | Cloud | Tencent Hunyuan KI-Service |
| Silicon Flow | 📋 | Cloud | Silicon Flow KI-Service |
| MiniMax | 📋 | Cloud | MiniMax KI-Service |
| OpenAI | 💡 | Cloud | OpenAI API-Service (GPT-Serie) |
| Anthropic | 💡 | Cloud | Anthropic Claude KI-Service |
| Google DeepMind | 💡 | Cloud | Google Gemini KI-Service |
| Mistral AI | 💡 | Cloud | Mistral KI-Service |
| Groq | 💡 | Cloud | Groq Hochgeschwindigkeits-KI-Inferenz |
| Together AI | 💡 | Cloud | Together AI Open-Source-Modelle |
| xAI | 💡 | Cloud | xAI Grok-Service |
| Cohere | 💡 | Cloud | Cohere Enterprise-NLP-Service |
| Replicate | 💡 | Cloud | Replicate Open-Source-Modell-Hosting |
| Hugging Face | 💡 | Cloud | Hugging Face Open-Source-KI-Community |
| Cerebras | 💡 | Cloud | Cerebras KI-Inferenz-Optimierung |
| Databricks | 💡 | Cloud | Databricks Enterprise-KI-Plattform |
| Perplexity AI | 💡 | Cloud | Perplexity AI Suchantwort-Service |
| NVIDIA NIM | 💡 | Cloud | NVIDIA KI-Inferenz-Mikroservice |

---

## Wichtige Design-Entscheidungen

### Storage als Instanzklasse (nicht statisch)

`IStorage` ist als injizierbare Instanz konzipiert, nicht als statisches Utility. Dies gewährleistet:

- Direkter Dateizugriff — IStorage ist der interne Persistenzkanal des Systems, wird **nicht** durch Executoren geroutet.
- **KI kontrolliert IStorage nicht** — Executoren verwalten IO von KI-Tools; IStorage verwaltet interne Datenlesungen/-schreibungen des Frameworks. Dies sind grundlegend verschiedene Anliegen.
- Testbar mit Mock-Implementierungen.
- Zukunftige Unterstützung verschiedener Storage-Backends ohne Änderung der Consumer.

### Executoren als Sicherheitsgrenze

Executoren sind der **einzige** Pfad für I/O-Operationen. Tools, die Festplatten-, Netzwerk- oder Befehlszeilenzugriff benötigen, **müssen** durch Executoren gehen. Dieses Design erzwingt:

- **Separater Scheduling-Thread** pro Executor mit Thread-Sperrung für Berechtigungsvalidierung.
- Zentrale Berechtigungsprüfung — Executoren queryen das **private PermissionManager** des Beings.
- Anfrage-Warteschlangen mit Prioritäts- und Timeout-Kontrolle.
- Audit-Logging aller externen Operationen.
- Ausnahmeisolation — Ausfall eines Executors beeinflusst andere Executoren nicht.
- Circuit Breaker — Aufeinanderfolgende Fehler pausieren Executor vorübergehend, um Kaskadenfehler zu verhindern.

### ContextManager als leichtgewichtiges Objekt

Jede `ExecuteOneRound()` erstellt eine neue `ContextManager`-Instanz:

1. Lädt Soul-Datei + aktuelle Chat-Historie.
2. Sendet Anfrage an KI-Client.
3. Schleife verarbeitet Tool-Aufrufe bis KI reinen Text zurückgibt.
4. Persistiert Antwort im Chat-System.
5. Verwirft.

Dies hält jede Runde isoliert und zustandslos.

### Selbstentwicklung durch Klassenüberschreibung

Silicon Beings können ihre eigenen C#-Klassen zur Laufzeit überschreiben:

1. KI generiert neuen Klassencode (muss `SiliconBeingBase` erben).
2. **Kompilierzeit-Referenzkontrolle** (primäre Verteidigung): Compiler erhält nur erlaubte Assembly-Liste — `System.IO`, `System.Reflection` etc. ausgeschlossen, gefährlicher Code ist auf Typebene unmöglich.
3. **Runtime-Statische Analyse** (sekundäre Verteidigung): `SecurityScanner` scannt Code nach gefährlichen Mustern nach erfolgreicher Kompilierung.
4. Roslyn kompiliert Code im Speicher.
5. Bei Erfolg: `SiliconBeingManager.ReplaceBeing()` tauscht aktuelle Instanz, migriert Status, persistiert verschlüsselten Code auf Festplatte.
6. Bei Fehler: Neuer Code verworfen, bestehende Implementierung beibehalten.

Benutzerdefinierte `IPermissionCallback`-Implementierungen können auch durch `ReplacePermissionCallback()` kompiliert und injiziert werden, ermöglicht Beings eigene Berechtigungslogik anzupassen.

Code wird auf Festplatte mit AES-256 verschlüsselt gespeichert. Verschlüsselungsschlüssel abgeleitet von Being-GUID (Großschrift) durch PBKDF2.

---

## Token-Nutzungsaudit

`TokenUsageAuditManager` trackt KI-Token-Verbrauch aller Beings:

- `TokenUsageRecord` — Datensatz pro Anfrage (Being-ID, Modell, Prompt-Token, Completion-Token, Zeitstempel)
- `TokenUsageSummary` — Aggregierte Statistiken
- `TokenUsageQuery` — Query-Parameter zum Filtern von Datensätzen
- Persistiert durch `ITimeStorage` für Zeitreihenabfragen
- Zugänglich durch Web-UI (UsageController) und `TokenAuditTool` (nur Curator)

---

### Kalendersystem

Das System enthält **32 Kalenderimplementierungen**, abgeleitet von der abstrakten `CalendarBase`-Klasse, die wichtige globale Kalendersysteme abdeckt:

| Kalender | ID | Beschreibung |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | Buddhistischer Kalender (BE), Jahr + 543 |
| CherokeeCalendar | `cherokee` | Cherokee-Kalendersystem |
| ChineseLunarCalendar | `lunar` | Chinesischer Mondkalender mit Schaltmonaten |
| ChineseHistoricalCalendar | `chinese_historical` | Chinesischer Historischer Kalender mit Ganzhi-Zyklus und Kaiser-Ären |
| ChulaSakaratCalendar | `chula_sakarat` | Chula Sakarat (CS), Jahr - 638 |
| CopticCalendar | `coptic` | Koptischer Kalender |
| DaiCalendar | `dai` | Dai-Kalender mit vollständigem Mondkalender |
| DehongDaiCalendar | `dehong_dai` | Dehong Dai-Variante |
| EthiopianCalendar | `ethiopian` | Äthiopischer Kalender |
| FrenchRepublicanCalendar | `french_republican` | Französischer Revolutionskalender |
| GregorianCalendar | `gregorian` | Standard Gregorianischer Kalender |
| HebrewCalendar | `hebrew` | Hebräischer (jüdischer) Kalender |
| IndianCalendar | `indian` | Indischer Nationalkalender |
| InuitCalendar | `inuit` | Inuit-Kalendersystem |
| IslamicCalendar | `islamic` | Islamischer Hijri-Kalender |
| JapaneseCalendar | `japanese` | Japanischer Nengo (Ära)-Kalender |
| JavaneseCalendar | `javanese` | Javanesischer Islamischer Kalender |
| JucheCalendar | `juche` | Juche-Kalender (Nordkorea), Jahr - 1911 |
| JulianCalendar | `julian` | Julianischer Kalender |
| KhmerCalendar | `khmer` | Khmer-Kalender |
| MayanCalendar | `mayan` | Maya Long Count Kalender |
| MongolianCalendar | `mongolian` | Mongolischer Kalender |
| PersianCalendar | `persian` | Persischer (Solar Hijri) Kalender |
| RepublicOfChinaCalendar | `roc` | Republik China (Minguo) Kalender, Jahr - 1911 |
| RomanCalendar | `roman` | Römischer Kalender |
| SakaCalendar | `saka` | Saka-Kalender (Indonesien) |
| SexagenaryCalendar | `sexagenary` | Chinesischer Ganzhi (Sexagenary) Kalender |
| TibetanCalendar | `tibetan` | Tibetischer Kalender |
| VietnameseCalendar | `vietnamese` | Vietnamesischer Mondkalender (Katzen-Zodiak-Variante) |
| VikramSamvatCalendar | `vikram_samvat` | Vikram Samvat Kalender |
| YiCalendar | `yi` | Yi-Kalendersystem |
| ZoroastrianCalendar | `zoroastrian` | Zoroastrischer Kalender |

`CalendarTool` bietet Operationen: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (Kalender-übergreifende DatumsKonvertierung).

---

## Web-UI-Architektur

### Skin-System

Die Web-UI hat ein **pluggable Skin-System**, das vollständige UI-Anpassung ohne Änderung der Anwendungslogik ermöglicht:

- **ISkin-Schnittstelle** — Definiert Vertrag für alle Skins, einschließlich:
  - Kern-Rendering-Methoden (`RenderHtml`, `RenderError`)
  - 20+ UI-Komponentenmethoden (Buttons, Inputs, Cards, Tables, Badges, Bubbles, Progress, Tags etc.)
  - Generiert Theme-CSS durch `CssBuilder`
  - `SkinPreviewInfo` — Farbpalette und Icons für Skin-Auswahl auf Initialisierungsseite

- **Eingebaute Skins** — 7 produktionsreife Skins:
  - **Admin** — Professionelle, datenfokussierte Systemverwaltungs-Oberfläche
  - **Chat** — Konversationelles, nachrichtenzentriertes Design für KI-Interaktion
  - **Creative** — Künstlerisches, visuell reichhaltiges Layout für kreative Workflows
  - **Dev** — Entwicklerzentriertes, codezentriertes Interface mit Syntax-Hervorhebung
  - **High Contrast** — Hoher Kontrast für Barrierefreiheit
  - **Light** — Helles, klares Design
  - **Minimal** — Reduziertes, minimales Interface

- **Skin-Entdeckung** — `SkinManager` entdeckt und registriert automatisch alle `ISkin`-Implementierungen durch Reflektion

### HTML / CSS / JS Builder

Die Web-UI vermeidet vollständig Template-Dateien, generiert alle Markierungen in C#:

- **`H`** — Fluent HTML-Builder-DSL zum Konstruieren von HTML-Bäumen im Code
- **`CssBuilder`** — CSS-Builder mit Selektor- und Media-Query-Unterstützung
- **`JsBuilder` (`JsSyntax`)** — JavaScript-Builder für Inline-Skripte

### Controller-System

Die Web-UI folgt **MVC-ähnlichem Pattern**, 22 Controller behandeln verschiedene Aspekte:

| Controller | Zweck |
|------------|---------|
| About | Über-Seite und Projektinformationen |
| Being | Silicon Being-Verwaltung und Status |
| Chat | Echtzeit-Chat-Oberfläche mit SSE |
| ChatHistory | Chat-Verlaufsansicht mit Sitzungsliste und Nachrichtendetails |
| CodeBrowser | Code-Ansicht und -Bearbeitung |
| CodeHover | Code-Hover-Hinweise mit Syntax-Hervorhebung |
| Config | Systemkonfigurationsverwaltung |
| Dashboard | Systemübersicht und Metriken |
| Executor | Executor-Status und -Verwaltung |
| Help | Hilfedokumentationssystem mit mehrsprachiger Unterstützung |
| Init | Initialisierungsassistent für erste Ausführung |
| Knowledge | Wissensgraph-Visualisierung und -Abfrage |
| Log | Systemprotokoll-Viewer mit Silicon Being-Filterung |
| Memory | Langzeitspeicher-Browser mit erweiterten Filtern, Statistiken und Detailansicht |
| Permission | Berechtigungsverwaltung |
| PermissionRequest | Berechtigungsanfrage-Warteschlange |
| Project | Projektverwaltung mit Arbeitsnotizen und Aufgabensystem |
| System | Systemverwaltung und Laufzeitüberwachung |
| Task | Aufgabensystem-Oberfläche |
| Timer | Timer-Systemverwaltung mit Ausführungsverlauf |
| Usage | Token-Nutzungsaudit-Dashboard mit Trendgrafiken und Export |
| WorkNote | Arbeitsnotizenverwaltung mit Suche und Inhaltsverzeichnis |

### Echtzeitaktualisierungen

- **SSE (Server-Sent Events)** — Push-Updates für Chat-Nachrichten, Being-Status und Systemereignisse durch `SSEHandler`
- **Kein WebSocket** — Einfachere Architektur mit SSE für die meisten Echtzeitanforderungen
- **Automatische Wiederverbindung** — Client-Wiederverbindungslogik für resiliente Verbindung

### Lokalisierung

Das System unterstützt umfassende Lokalisierung für **29 Sprachvarianten**:
- **Chinesisch (6)**: zh-CN (Simplified), zh-HK (Traditional), zh-SG (Singapur), zh-MO (Macau), zh-TW (Taiwan), zh-MY (Malaysia)
- **Englisch (10)**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **Spanisch (2)**: es-ES, es-MX
- **Deutsch (5)**: de-DE, de-AT, de-CH, de-LU, de-LI
- **Französisch (3)**: fr-FR, fr-CA, fr-CH
- **Andere (3)**: ja-JP (Japanisch), ko-KR (Koreanisch), cs-CZ (Tschechisch)

Aktives Locale ausgewählt durch `DefaultConfigData.Language`, aufgelöst durch `LocalizationManager`.

---

### WebView-Browserautomatisierungssystem (Neu)

Das System integriert WebView-Browserautomatisierung basierend auf **Playwright**:

- **Individuelle Isolation**: Jedes Silicon Being hat eigene Browser-Instanz, Cookies und Session-Speicher, vollständig isoliert.
- **Headless-Modus**: Browser läuft im vollständig unsichtbaren Headless-Modus, Silicon Beings arbeiten autonom im Hintergrund.
- **WebViewBrowserTool**: Bietet vollständige Browseroperationen, einschließlich:
  - Seitennavigation, Klicken, Texteingabe, Seiteninhalt abrufen
  - JavaScript ausführen, Screenshots aufnehmen, auf Elemente warten
  - Browserstatusverwaltung und Ressourcenbereinigung
- **Sicherheitskontrolle**: Alle Browseroperationen müssen Berechtigungskette durchlaufen, verhindert bösartigen Webseitenzugriff.

### Wissensnetzwerksystem (Neu)

Das System hat eingebautes Wissensgraph-System basierend auf **Triple-Struktur**:

- **Wissensdarstellung**: Verwendet "Subjekt-Relation-Objekt" Triple-Struktur (z.B.: Python-ist_ein-Programmiersprache)
- **KnowledgeTool**: Bietet vollständigen Wissenslebenszyklusmanagement:
  - `add`/`query`/`update`/`delete` — Basis-CRUD-Operationen
  - `search` — Volltextsuche und Keyword-Matching
  - `get_path` — Entdeckt Assoziationspfade zwischen zwei Konzepten
  - `validate` — Wissensvollständigkeitsprüfung
  - `stats` — Wissensnetzwerk-Statistik
- **Persistenter Speicher**: Wissens-Tripel persistent im Dateisystem, unterstützt Zeitindexabfragen.
- **Konfidenz-Scoring**: Jeder Wissenseintrag hat Konfidenz-Score (0-1), unterstützt unscharfes Matching und Sortierung.
- **Tag-Klassifikation**: Unterstützt Tags für Wissen, erleichtert Klassifikation und Retrieval.

---

## Datenverzeichnisstruktur

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # Curator's Soul-Datei
    │   ├── state.json       # Runtime-Status
    │   ├── code.enc         # AES-verschlüsselter benutzerdefinierter Klassencode
    │   └── permission.enc   # AES-verschlüsselter benutzerdefinierter Permission-Callback
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```

---

## SpeedyPack-Speicher-Engine

SiliconLife.Fast verwendet die eigenentwickelte SpeedyPack-Speicher-Engine (.spk-Format), die das vorherige LiteDB-Schema ersetzt und extreme Lese-/Schreibleistung realisiert.

### Architekturdesign

```
┌──────────────────────────────────────────────────────────┐
│                    SpeedyPack                             │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────────┐  │
│  │ DirectoryMap  │  │  EntryCache   │  │  WriteQueue   │  │
│  │ (In-Memory-  │  │  (Eintrags-   │  │ (Asynchrone   │  │
│  │  Verzeichnis) │  │   Cache)      │  │  Schreib-     │  │
│  │              │  │              │  │  warteschlange)│  │
│  └──────┬───────┘  └──────┬───────┘  └───────┬───────┘  │
│         │                  │                   │          │
│  ┌──────▼──────────────────▼───────────────────▼───────┐  │
│  │              PackFileReader / PackFileWriter          │  │
│  │              (Paketdatei-Leser/Schreiber)             │  │
│  └──────────────────────────┬──────────────────────────┘  │
│                              │                             │
│  ┌──────────────────────────▼──────────────────────────┐  │
│  │              .spk-Datei (MessagePack + LZ4-Komprimierung) │  │
│  └─────────────────────────────────────────────────────┘  │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐                      │
│  │  FreeList     │  │ SpeedyPack   │                      │
│  │ (Freiraum-   │  │ AutoCompactor│                      │
│  │  verwaltung) │  │ (Auto-       │                      │
│  │              │  │  Komprimierung)│                     │
│  └──────────────┘  └──────────────┘                      │
└──────────────────────────────────────────────────────────┘
```

### Kernkomponenten

| Komponente | Beschreibung |
|------|------|
| `SpeedyPack` | Kernklasse, kombiniert DirectoryMap, EntryCache und WriteQueue für niedrige Latenz Lese-/Schreibzugriff |
| `DirectoryMap` | In-Memory-Verzeichniszuordnung, pflegt Zuordnung von virtuellen Pfaden zu Dateieinträgen |
| `EntryCache` | Eintrags-Cache, TTL-basierter Cache für zuletzt zugegriffene Einträge |
| `WriteQueue` | Asynchrone Schreibwarteschlange, reiht Schreiboperationen zur Ausführung im Hintergrund-Thread ein |
| `FreeList` | Freiraumverwaltung, verfolgt wiederverwendbaren Speicherplatz in .spk-Dateien |
| `PackFileReader` | Paketdatei-Leser, liest Daten aus .spk-Dateien |
| `PackFileWriter` | Paketdatei-Schreiber, schreibt Daten in .spk-Dateien |
| `SpeedyPackAutoCompactor` | Automatischer Komprimierungs-Timer, komprimiert .spk-Dateien regelmäßig zur Rückgewinnung von freiem Speicherplatz |
| `SpeedyPackRegistry` | Prozessweiter Singleton-Manager, stellt sicher, dass die gesamte Anwendung dieselbe SpeedyPack-Instanz verwendet |

### Speicheradapter

SiliconLife.Fast integriert SpeedyPack über folgende Adapter in die Systemschnittstellen:

| Adapter | Schnittstelle | Beschreibung |
|--------|------|------|
| `SpeedyStorage` | `IStorage` | Allgemeiner Schlüssel-Wert-Speicheradapter |
| `SpeedyTimeStorage` | `ITimeStorage` | Zeitindex-Speicheradapter |
| `SpeedyWorkNoteStorage` | `IWorkNoteStorage` | Arbeitsnotiz-Speicheradapter |

### Konfigurationsoptionen

`SpeedyPackOptions` bietet folgende Konfiguration:

| Option | Typ | Standardwert | Beschreibung |
|------|------|--------|------|
| `CacheTtl` | `TimeSpan` | 5 Minuten | Lebensdauer der Cache-Einträge |
| `MaxCacheEntries` | `int` | 1000 | Maximale Anzahl von Cache-Einträgen |
| `ReadOnly` | `bool` | false | Schreibgeschützter Modus |

### Transaktionsunterstützung

SpeedyPack unterstützt atomare Schreiboperationen über die `IPackTransaction`-Schnittstelle:

- `SpeedyTransaction` implementiert den Transaktionsmechanismus
- Unterstützt Atomizität von Batch-Schreibvorgängen
- Beim Transaktions-Commit werden alle Schreiboperationen entweder vollständig erfolgreich oder vollständig zurückgerollt

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

### Plugin-Loader

`PluginLoader` ist verantwortlich für das Laden von Plugin-DLLs aus einem angegebenen Verzeichnis und führt strenge Sicherheitsprüfungen durch:

1. **Verzeichnis-Scan** — Scannt alle .dll-Dateien im Plugin-Verzeichnis
2. **Sicherheits-Scan** — Prüft, ob das Plugin verbotene Namespaces referenziert
3. **Isoliertes Laden** — Verwendet benutzerdefinierten `AssemblyLoadContext` zum isolierten Laden von Plugins
4. **Lebenszyklusverwaltung** — Ruft OnLoad-, OnStart-, OnStop-, OnUnload-Methoden des Plugins auf

### Sicherheits-Sandkasten

Der Plugin-Loader führt folgende Sicherheitsprüfungen durch:

| Prüfgegenstand | Beschreibung |
|--------|------|
| Verbotene Namespaces | System.IO, System.Net.Http, System.Net.WebSockets, System.Net.Sockets, Microsoft.CodeAnalysis |
| Vertrauenswürdige Assembly-Whitelist | Google.Protobuf, Newtonsoft.Json, MessagePack, Serilog, Microsoft.Extensions.Logging.Abstractions, Dapper |
| Verbotene Typ-Prüfung | Scannt gefährliche Typen, die im Plugin referenziert werden |
| Verbotene Member-Prüfung | Scannt gefährliche Methoden, die im Plugin aufgerufen werden |

### Tool-Integration

Plugins können benutzerdefinierte Tools durch Implementierung der `ITool`-Schnittstelle registrieren:

- `ToolManager.ScanAllPluginAssemblies()` scannt alle geladenen Plugins nach ITool-Implementierungen
- Plugin-Tools werden automatisch in den Tool-Aufruf-Zyklus integriert
- Plugin-Tools unterliegen demselben Berechtigungssystem

### Plugin-Lebenszyklus

```
Laden (OnLoad) → Starten (OnStart) → Laufend → Stoppen (OnStop) → Entladen (OnUnload)
```

---

## Silicon Being-Aktivitätsstatus

Silicon Beings haben folgende Aktivitätszustände:

| Status | Beschreibung |
|------|------|
| `Idle` | Leerlauf, wartet auf Clock-Trigger |
| `Working` | Führt eine Runde KI-Anfrage + Tool-Aufrufe aus |
| `Error` | Fehler während der Ausführung aufgetreten |
| `Stopped` | Gestoppt, aufgrund von aufeinanderfolgenden Fehlern oder manuellem Stopp |

**Gestoppt-Status-Mechanismus**:
- Wenn ein Silicon Being 10 aufeinanderfolgende Fehler auftritt, wechselt es automatisch in den `Stopped`-Status
- Im Stopped-Status führt das Being keine Aufgaben mehr aus
- Manueller Eingriff ist erforderlich, um es neu zu starten

Zustandsübergänge:
```
Idle → Working → Idle (Normaler Abschluss)
Working → Error → Working (Fehlerwiederherstellung)
Working → Stopped (10 aufeinanderfolgende Fehler oder manueller Stopp)
Stopped → Idle (Neustart)
```
