# Roadmap

> **Version: v0.1.0-alpha**

[English](../en/roadmap.md) | [Deutsch](../de-DE/roadmap.md) | [中文](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | [Español](../es-ES/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Čeština](../cs-CZ/roadmap.md)

## Leitprinzipien

Jede Phase endet mit einem **funktionsfähigen, beobachtbaren** System. Keine Phase produziert "eine Menge Infrastruktur ohne etwas Zeigbares".

---

## ~~Phase 1: Kann chatten~~ ✅ Abgeschlossen

**Ziel**: Konsoleneingabe → KI-Aufruf → Konsolenausgabe. Minimal verifizierbare Einheit.

| # | Modul | Beschreibung |
|---|--------|-------------|
| 1.1 | Solutions- und Projektstruktur | `SiliconLifeCollective.sln` erstellen, mit `src/SiliconLife.Core/` (Core-Bibliothek) und `src/SiliconLife.Default/` (Standardimplementierung + Einstiegspunkt) |
| 1.2 | Konfiguration (minimal) | Singleton + JSON-Deserialisierung. Liest `config.json`. Generiert automatisch Defaults wenn fehlend |
| 1.3 | Lokalisierung (minimal) | `LocalizationBase` abstrakte Klasse, `ZhCN`-Implementierung. `Language` in Konfiguration hinzufügen |
| 1.4 | OllamaClient (minimal) | `IAIClient`-Schnittstelle, HTTP-Aufruf an lokalen Ollama `/api/chat`. Noch kein Streaming, keine Tool-Aufrufe |
| 1.5 | Konsolen-I/O | `while(true) + Console.ReadLine()`, Eingabe lesen → KI aufrufen → Antwort drucken |
| 1.6 | Copyright-Header | Apache 2.0-Header für alle C#-Quelldateien hinzufügen |

**Liefergegenstand**: Konsolen-Chat-Programm zur Konversation mit lokalem Ollama-Modell.

**Verifizierung**: Programm ausführen, "hello" eingeben, KI-Antwort sehen.

---

## ~~Phase 2: Hat Skelett~~ ✅ Abgeschlossen

**Ziel**: "Nackte Schleife" durch Framework-Struktur ersetzen. Verhalten unverändert.

| # | Modul | Beschreibung |
|---|--------|-------------|
| 2.1 | Storage (minimal) | `IStorage`-Schnittstelle (Read/Write/Exists/Delete, Key-Value-Paare). `FileSystemStorage`-Implementierung. Instanzklasse (nicht statisch). Direkter Dateisystemzugriff —— **AI kann IStorage nicht kontrollieren** |
| 2.2 | Hauptschleife + Clock-Objekt | Endlosschleife, präzises Clock-Intervall (`Stopwatch` + `Thread.Sleep`). Prioritäts-Scheduling |
| 2.3 | IAIClient-Standardisierung | `IAIClientFactory`-Schnittstelle. OllamaClient refaktoriert für Standardschnittstelle |
| 2.4 | Konsolen-Migration | `while(true)` zu Hauptschleife-getriebenem Clock-Objekt migrieren. Verhalten wie Phase 1 |

**Liefergegenstand**: Hauptschleife läuft Clock, Konsolen-Chat funktioniert noch.

**Verifizierung**: Test-Clock-Objekt registrieren, zählt Clock jede Sekunde; Konsolen-Chat funktioniert noch.

---

## ~~Phase 3: Hat Seele~~ ✅ Abgeschlossen

**Ziel**: Erstes Silicon Being lebt im Framework.

| # | Modul | Beschreibung |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Abstrakte Basisklasse mit Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. Abstrakte `Tick()` und `ExecuteOneRound()` |
| 3.2 | Soul-Datei-Laden | `SoulFileManager`: Liest `soul.md` aus Being-Datenverzeichnis |
| 3.3 | ContextManager (minimal) | Verkettet Soul-Datei + neueste Nachrichten → ruft KI auf → erhält Antwort. Noch keine Tool-Aufrufe, keine Persistenz |
| 3.4 | ISiliconBeingFactory | Factory-Schnittstelle zum Erstellen von Being-Instanzen |
| 3.5 | SiliconBeingManager (minimal) | Erbt Clock-Objekt (Priorität=0). Iteriert alle Beings, ruft deren Tick sequentiell auf |
| 3.6 | DefaultSiliconBeing | Standardverhalten-Implementierung. Prüft ungelesene Nachrichten → erstellt ContextManager → ExecuteOneRound → Ausgabe |
| 3.7 | Being-Verzeichnisstruktur | `DataDirectory/SiliconManager/{GUID}/`, enthält `soul.md` und `state.json` |

**Liefergegenstand**: Von Hauptschleife gesteuertes Silicon Being, empfängt Konsoleneingabe, lädt Soul-Datei, ruft KI auf.

**Verifizierung**: Konsoleneingabe → Hauptschleife-Clock triggert → Being verarbeitet (mit Soul-Datei-geführtem Verhalten) → KI-Antwort. Antwort-Stil sollte sich von Phase 1 unterscheiden.

---

## ~~Phase 4: Hat Erinnerung~~ ✅ Abgeschlossen

**Ziel**: Konversationen persistieren nach Neustart.

| # | Modul | Beschreibung |
|---|--------|-------------|
| 4.1 | ChatSystem | Kanal-Konzept (zwei GUIDs = ein Kanal). Nachrichtenmodell mit Persistenz. Noch kein Gruppenchat |
| 4.2 | IIMProvider + IMManager | `IIMProvider`-Schnittstelle. `ConsoleProvider` als formaler IM-Kanal. `IMManager` routet Nachrichten |
| 4.3 | ContextManager-Erweiterung | Pullt Historie aus Chat-System. Persistiert KI-Antworten. Unterstützt mehrstufige Tool-Aufruf-Fortsetzung |
| 4.4 | IMessage-Modell | Einheitliches Nachrichtenmodell geteilt zwischen Chat-System und IM-Manager |

**Liefergegenstand**: Chat-System mit persistierendem Speicher.

**Verifizierung**: Mehrere Runden chatten → Beenden → Neustart → Fragen "Worüber haben wir gesprochen?" → Being kann antworten.

---

## ~~Phase 5: Kann handeln (Tool-System)~~ ✅ Abgeschlossen

**Ziel**: Silicon Beings können Aktionen ausführen, nicht nur chatten.

| # | Modul | Beschreibung |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | `ITool`-Schnittstelle mit Name, Description, Execute. `ToolResult` mit Success, Message, Data |
| 5.2 | ToolManager | Instanz pro Being. Reflektionsbasierte Tool-Entdeckung. `[SiliconManagerOnly]`-Attribut-Unterstützung |
| 5.3 | IAIClient: Tool-Aufruf-Unterstützung | Parst AI tool_calls. Schleife: Tools ausführen → Ergebnisse zurückschicken → KI weiter → bis reiner Text |
| 5.4 | Executor-Basisklasse | Abstrakte Basisklasse mit eigenem Dispatcher-Thread, Anfrage-Warteschlange, Timeout-Steuerung |
| 5.5 | NetworkExecutor | HTTP-Anfragen über Executor. Timeout, Queuing |
| 5.6 | CommandLineExecutor | Shell-Ausführung über Executor. Plattformübergreifende Trennzeichen-Erkennung |
| 5.7 | DiskExecutor | Dateioperationen über Executor. Noch keine Berechtigungsprüfung (Phase 6) |
| 5.8–5.12 | Integrierte Tools | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Liefergegenstand**: Silicon Beings können Tools zur Aktionen-Ausführung aufrufen.

**Verifizierung**: Fragen "Welcher Tag ist heute" → CalendarTool antwortet; Fragen "Prozesse prüfen" → SystemTool führt aus; Being anweisen, anderem Being Nachricht zu senden → ChatTool funktioniert.

---

## ~~Phase 6: Folgt Regeln (Berechtigungssystem)~~ ✅ Abgeschlossen

**Ziel**: Silicon Beings können ohne Autorisierung nicht auf sensible Ressourcen zugreifen.

| # | Modul | Beschreibung |
|---|--------|-------------|
| 6.1 | PermissionManager | Private Instanz pro Being. Callback-basiert, ternäres Ergebnis (Allowed/Deny/AskUser). Abfragepriorität: HighDeny → HighAllow → Callback. IsCurator-Flag |
| 6.2 | PermissionType-Enum | NetworkAccess, CommandLine, FileAccess, Function, DataAccess |
| 6.3 | DefaultPermissionCallback | Netzwerk-Whitelist/Blacklist, CLI-Klassifizierung, Dateipfad-Sicherheitsregeln |
| 6.4 | GlobalACL | Präfix-Matching-Regeltabelle, persistiert in Storage |
| 6.5 | UserFrequencyCache | HighAllow/HighDeny-Listen. Benutzerauswahl (nicht Auto-Erkennung). Präfix-Matching, nur Speicher, konfigurierbarer Ablauf |
| 6.6 | UserAsk-Mechanismus (Konsole) | Bei Rückgabe AskUser Konsolen-Prompt y/n |
| 6.7 | Executor-Berechtigungsintegration | Alle Executoren prüfen Berechtigung vor Ausführung |
| 6.8 | IStorage-Isolations-Hinweis | IStorage ist interne Systempersistenz —— direkter Dateizugriff, **nicht** über Executor geroutet, **nicht** von AI kontrollierbar. Executoren managen nur von AI-Tools initiiertes IO |
| 6.9 | Audit-Logs | Protokolliert alle Berechtigungsentscheidungen mit Zeitstempel, Anfrager, Ressource, Ergebnis |

**Liefergegenstand**: Berechtigungsprompt wenn Being sensible Operation versucht.

**Verifizierung**: Being anweisen, Datei zu löschen → Konsole zeigt Berechtigungsprompt → `n` eingeben → Operation verweigert. Being anweisen, Whitelist-Website zu besuchen → sofort erlaubt.

---

## ~~Phase 7: Kann sich entwickeln (Dynamische Kompilierung)~~ ✅ Abgeschlossen

**Ziel**: Silicon Beings können eigenen Code neu schreiben.

| # | Modul | Beschreibung |
|---|--------|-------------|
| 7.1 | CodeEncryption | AES-256 Verschlüsselung/Entschlüsselung. PBKDF2-Schlüssel von GUID abgeleitet |
| 7.2 | DynamicCompilationExecutor | Roslyn-basierte In-Memory-Kompilierungs-Sandbox. Kompilierzeit-Assembly-Referenzkontrolle (Hauptverteidigung: ausschließen von System.IO, Reflection etc.) |
| 7.3 | Sicherheits-Scan | Laufzeit-statische Analyse gefährlicher Code-Muster (Nebenverteidigung). Blockiert Laden wenn Scan fehlschlägt |
| 7.4 | Being-Lebenszyklus-Erweiterung | Laden: Entschlüsseln → Scannen → Kompilieren → Instanziieren. Laufzeit: In-Memory kompilieren → Atomar ersetzen → Verschlüsselt persistieren |
| 7.5 | SiliconCurator | Curator-Abstraktbasisklasse. IsCurator=true. Höchste Berechtigung |
| 7.6 | DefaultCurator | Standard-Curator-Implementierung mit eingebauter Soul-Datei und Admin-Tools |
| 7.7 | CuratorTool | `[SiliconManagerOnly]`-Tools: list_beings, create_being, get_code, reset |
| 7.8 | Permission-Callback-Override | Beings können benutzerdefinierte Permission-Callbacks kompilieren |
| 7.9 | SiliconBeingManager-Erweiterung | Replace-Methode (Laufzeit-Instanz-Austausch). MigrateState (Status zwischen alter und neuer Instanz transferieren) |

**Liefergegenstand**: Silicon Beings können durch AI generierten neuen Code kompilieren und sich ersetzen.

**Verifizierung**: Being anweisen "Füge dir selbst ein neues Feature hinzu" → Kompilierung beobachten → Neustart → Neues Feature funktioniert.

---

## ~~Phase 8: Erinnerung und Planung~~ ✅ Abgeschlossen

**Ziel**: Langzeitspeicher, Aufgabenverwaltung, Timer-Trigger.

| # | Modul | Beschreibung |
|---|--------|-------------|
| 8.1 | FileSystemMemory | Kurzzeit-/Langzeit-segmentierter Speicher. Zeitlicher Verfall. Komprimierung (ähnliche Erinnerungen zusammenführen). Multidimensionale Suche |
| 8.2 | TaskSystem | Einmalige + DAG-abhängige Aufgaben. Prioritäts-Scheduling. Status-Tracking |
| 8.3 | TimerSystem | Einmal-Alarm + periodische Timer. Millisekunden-Präzision. Persistiert in Storage |
| 8.4 | IncompleteDate | Unscharfe Datumsbereichs-Struktur (z.B. "April 2026", "Frühling 2026") |
| 8.5–8.7 | Erinnerungs-/Aufgaben-/Timer-Tools | Tools für Beings zum Abfragen von Erinnerungen, Verwalten von Aufgaben, Setzen von Timern |

**Liefergegenstand**: Beings können sich Schlüsselpunkte merken, Aufgaben erstellen/verfolgen, Alarme setzen.

**Verifizierung**: Aufgabe erstellen → Aufgabenliste prüfen → 1-Minuten-Alarm setzen → Benachrichtigung bei Auslösung erhalten.

---

## ~~Phase 9: Framework abgeschlossen~~ ✅ Abgeschlossen

**Ziel**: Vereinheitlichter Einstiegspunkt, Multi-Being-Kollaboration.

| # | Modul | Beschreibung |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | Einheitlicher Host mit Builder-Pattern. Graceful Shutdown (Ctrl+C / SIGTERM) |
| 9.2 | Program.Main-Refaktorierung | Migration zu CoreHostBuilder-Pattern |
| 9.3 | SiliconBeingManager-Erweiterung | Curator-first-Antwort. Ausnahme-Isolierung. Regelmäßige Persistenz |
| 9.4 | Multi-Being-Laden | Lädt mehrere Beings aus Datenverzeichnis. Being-zu-Being-Kommunikation über ChatTool |
| 9.5 | Performance-Monitoring | Ausführungszeit-Tracking pro Clock-Objekt |
| 9.6 | ServiceLocator | Globaler Service-Locator mit Register/Get-Methoden |

**Liefergegenstand**: Mehrere Beings laufen gleichzeitig, kollaborieren, von CoreHost gemanagt.

**Verifizierung**: Zwei Beings erstellen → A sendet B Nachricht → B empfängt und antwortet → Framework-Scheduling ohne Fehler. Curator antwortet zuerst bei Benutzer-Nachrichten.

---

## ~~Phase 10: Zum Web~~ ✅ Abgeschlossen

**Ziel**: Von Konsole zu Browser-Interface migrieren.

| # | Modul | Beschreibung |
|---|--------|-------------|
| 10.1 | Router | HTTP-Anfrage-Router. Sequenzparameter-Routing und Static-File-Service |
| 10.2 | Controller-Basisklasse | Anfrage/Antwort-Kontext. HTML- und JSON-Antwort-Unterstützung |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | C#-Server-side-Builder. Null Frontend-Framework-Abhängigkeit |
| 10.6 | SSE (Server-Sent Events) | Push-basierte Echtzeit-Updates für Chat, Being-Status und System-Events. Einfacher als WebSocket, mit automatischem Client-Reconnect |
| 10.7 | WebUIProvider | SSE-basierter Echtzeit-IM-Kanal. Ersetzt Konsole als Hauptinterface |
| 10.8 | Web-Sicherheit | IP-Blacklist/Whitelist. `[WebCode]`-Attribut. Dynamische Updates |
| 10.9–10.17 | Web-Controller | Chat, Dashboard, Beings, Aufgaben, Berechtigungen, Berechtigungsanfragen, Executoren, Logs, Konfiguration, Speicher, Timer, Initialisierung, Über, Code-Browser, Wissen, Projekte, Audit |

**Liefergegenstand**: Vollständige Web-UI, vom Browser aus zugänglich.

**Verifizierung**: Browser öffnen → Mit Being chatten → Dashboard ansehen → Berechtigungen verwalten → Alles funktioniert.

---

## ~~Phase 10.5: Inkrementelle Erweiterungen~~ ✅ Abgeschlossen

**Ziel**: Bestehendes System mit neuen Features erweitern, die während der Entwicklung entdeckt wurden.

| # | Modul | Beschreibung |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | Neuer Sitzungstyp für systemweite Ankündigungen. Feste Kanal-ID, dynamisches Abonnement, Pending-Nachrichten-Filterung |
| 10.5.2 | ChatMessage-Erweiterung | ToolCallId, ToolCallsJson, Thinking-Felder für AI-Kontext; PromptTokens, CompletionTokens, TotalTokens für Token-Tracking; SystemNotification-Nachrichtentyp |
| 10.5.3 | TokenUsageAuditManager | Token-Verbrauchs-Tracking über alle Beings hinweg pro Anfrage. Aggregierte Statistiken, Zeitreihen-Abfragen, persistenter Speicher |
| 10.5.4 | TokenAuditTool | `[SiliconManagerOnly]`-Tool für Curator zum Abfragen und Zusammenfassen von Token-Nutzung |
| 10.5.5 | ConfigTool | `[SiliconManagerOnly]`-Tool für Curator zum Lesen und Ändern der Systemkonfiguration |
| 10.5.6 | AuditController | Web-Dashboard für Token-Nutzungs-Audit mit Trend-Diagrammen und Daten-Export |
| 10.5.7 | Kalendersystem-Erweiterung | 32 Kalender-Implementierungen, abdeckend Welt-Kalendersysteme (Buddhistisch, Chinesisch Lunar, Islamisch, Hebräisch, Japanisch, Persisch, Mayan etc.) |
| 10.5.8 | DiskTool-Erweiterung | Neue Operationen: count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all, list_drives |
| 10.5.9 | SystemTool-Erweiterung | Neue Operationen: find_process (mit Wildcard-Unterstützung), resource_usage |
| 10.5.10 | CalendarTool-Erweiterung | Neue Operationen: diff, list_calendars, get_components, get_now_components, convert (Cross-Kalender-Konvertierung) |
| 10.5.11 | DashScopeClient | Alibaba Cloud DashScope AI-Client, OpenAI-API-kompatibel. Unterstützt Streaming, Tool-Aufrufe, Reasoning-Content |
| 10.5.12 | DashScopeClientFactory | Factory zum Erstellen von DashScope-Clients. Dynamische Modell-Entdeckung über API. Multi-Region-Unterstützung (Peking, Virginia, Singapur, Hongkong, Frankfurt) |
| 10.5.13 | AI-Client-Konfigurationssystem | AI-Client-Konfiguration pro Being. Dynamische Konfigurations-Schlüsseloptionen (Modell, Region). Lokalisierte Anzeigenamen |
| 10.5.14 | Lokalisierungs-Erweiterung | Simplified Chinese, Traditional Chinese, English und Japanese Lokalisierung für DashScope-Konfigurationsoptionen, Modellnamen und Regionsnamen |

**Liefergegenstand**: Erweiterte Tools, Observability, Kalender-Abdeckung und Multi-AI-Backend-Unterstützung.

**Verifizierung**: Curator fragt Token-Nutzung über TokenAuditTool ab → Audit-Dashboard zeigt Trends → CalendarTool konvertiert Datum zwischen 32 Kalendersystemen → AI-Backend zu DashScope wechseln → Mit Qwen-Modell über Cloud-API chatten.

---

## ~~Phase 10.6: Verfeinerung und Optimierung~~ ✅ Abgeschlossen

**Ziel**: Systemfunktionen verfeinern, neue Features hinzufügen, Benutzererfahrung optimieren.

| # | Modul | Beschreibung |
|---|--------|-------------|
| 10.6.1 | WebViewBrowserTool | Cross-Plattform-Browserautomatisierungs-Tool basierend auf Playwright, mit Headless-Modus, individueller Isolation, voller JS/CSS-Unterstützung |
| 10.6.2 | HelpTool | Hilfedokumentationssystem-Tool, unterstützt mehrsprachige Dokumentabfrage und -anzeige |
| 10.6.3 | ProjectWorkNoteTool | Projekt-Arbeitsnotizen-Tool, unterstützt projektbezogene Arbeitsaufzeichnungen und -verwaltung |
| 10.6.4 | ProjectTaskTool | Projekt-Aufgabenverwaltungstool, unterstützt Aufgabenzuweisung, Fortschrittsverfolgung |
| 10.6.5 | KnowledgeTool | Wissensnetzwerk-Tool, unterstützt Triple-Wissens CRUD und Pfadfindung |
| 10.6.6 | ChatHistoryController | Chat-Historie-Anzeige-Controller, unterstützt Sitzungsliste und Nachrichtendetails |
| 10.6.7 | CodeHoverController | Code-Hover-Tooltip-Controller, unterstützt Syntax-Hervorhebung und Code-Vervollständigung |
| 10.6.8 | WorkNoteController | Arbeitsnotizen-Verwaltungs-Controller, unterstützt Suche und Verzeichnisgenerierung |
| 10.6.9 | TimerExecutionHistory | Timer-Ausführungshistorie-Funktion, zeichnet Timer-Trigger-Historie auf und ermöglicht Ansicht |
| 10.6.10 | Lokalisierungs-Erweiterung | Tschechisch (cs-CZ) Lokalisierungsunterstützung hinzufügen, insgesamt 21 Sprachvarianten |
| 10.6.11 | Web-UI-Optimierung | Datei-Upload-Unterstützung, Ladeindikator, Tool-Aufruf-Rendering-Optimierung, Arbeitsnotizen-Modal-Reparatur |
| 10.6.12 | Speicherverwaltungs-Erweiterung | Erweiterte Filterung, Statistiken, Detailansicht, Komprimierungsalgorithmus-Optimierung |
| 10.6.13 | Log-System-Refaktorierung | System/Silicon-Being-Logs getrennt, Log-Lese-API, Being-Filter |
| 10.6.14 | Berechtigungssystem-Erweiterung | Permission-Callback-Precompile-Validierung, Assembly-Referenz-Validierung, wttr.in Wetterdienst-Whitelist |

**Liefergegenstand**: Vollständige WebView-Browserautomatisierung, Hilfedokumentationssystem, Projekt-Workspace, Wissensnetzwerk, Chat-Historie-Ansicht und andere erweiterte Funktionen.

**Verifizierung**: Silicon Being kann Browser über WebViewBrowserTool bedienen → Hilfe-Dokumentation über HelpTool erhalten → Projekt-Arbeitsnotizen und -Aufgaben verwalten → Wissensnetzwerk abfragen → Chat-Historie ansehen.

---

## Phase 11: Externe IM-Integration

**Ziel**: Verbindung zu externen Messaging-Plattformen für breitere Benutzerzugänglichkeit.

| # | Modul | Beschreibung |
|---|--------|-------------|
| 11.1 | FeishuProvider | Feishu (Lark) Bot-Integration mit Card-Unterstützung |
| 11.2 | WhatsAppProvider | WhatsApp Business API-Integration |
| 11.3 | TelegramProvider | Telegram Bot API-Integration mit Inline-Keyboard-Unterstützung |
| 11.4 | IMManager-Erweiterung | Multi-Provider-Routing, einheitliches Nachrichtenformat, plattformübergreifende Berechtigungsanfrage-Behandlung |

**Liefergegenstand**: Benutzer können über externe IM-Plattformen mit Silicon Beings interagieren.

---

## Phase 12: Erweiterte Funktionen

**Ziel**: Optionale erweiterte Funktionen für verbesserte Funktionalität.

| # | Modul | Beschreibung |
|---|--------|-------------|
| 12.1 | Wissensnetzwerk | Geteiltes Wissensgraph mit Triple-Struktur (Subjekt-Prädikat-Objekt) |
| 12.2 | Plugin-System | Externes Plugin-Laden mit Sicherheitsprüfung und Sandboxing |
| 12.3 | Skill-Ökosystem | Wiederverwendbarer Skill-Marktplatz für Being-Fähigkeiten |
