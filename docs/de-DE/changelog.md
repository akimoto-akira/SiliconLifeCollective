# Änderungsprotokoll

[English](../en/changelog.md) | **Deutsch** | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md)

Alle wichtigen Änderungen an diesem Projekt werden in dieser Datei dokumentiert.

Das Format basiert auf [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
und dieses Projekt遵循 [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## Über dieses Änderungsprotokoll

### Projektursprung

- Dieses Projekt entstand am 20. März 2026.
- Vor diesem Projekt gab es ein Verifizierungs-Demo, das aufgrund eines irrationalen Architekturentwurfs scheiterte, was die Integration mit mehreren KI-Plattformen verhinderte.

### Verwendete KI-IDE-Tools

#### Kiro (Amazon AWS)
- Das Projekt wurde ursprünglich von Kiro gepflegt und im Spec-Modus gestartet.
- Kiro ist eine von Amazon AWS entwickelte agentic KI-Entwicklungsumgebung.
- Basierend auf Code OSS (VS Code), unterstützt VS Code-Einstellungen und Open VSX-kompatible Erweiterungen.
- Verfügt über einen spezifikationsgetriebenen Entwicklungsworkflow für strukturiertes KI-Coding.

#### Comate AI IDE / 文心快码 (Baidu)
- Gelegentlich verwendet für Copywriting und Dokumentationsarbeiten.
- Comate AI IDE ist ein von Baidu Wenxin am 23. Juni 2025 veröffentlichtes KI-natives Entwicklungsumgebungstool.
- Die erste multimodale, multi-agent kollaborative KI-IDE der Branche.
- Funktionen umfassen Design-zu-Code-Konvertierung und vollständige KI-unterstützte Codierung.
- Angetrieben vom Baidu Wenxin 4.0 X1 Turbo-Modell.

#### Trae (ByteDance)
- Dieses Projekt wurde die meiste Zeit hauptsächlich mit Trae gepflegt.
- Trae ist eine von SPRING PTE, einer singapurischen Tochtergesellschaft von ByteDance, entwickelte KI-IDE.
- Als 10x AI Engineer, fähig, eigenständig Softwarelösungen zu entwickeln.
- Verfügt über intelligente Produktivitätstools, flexible Entwicklungsrythmusanpassung und kollaborative Projektlieferung.
- Bietet Enterprise-Grade-Leistung mit konfigurierbarem Agentensystem.

#### Qoder (Alibaba)
- Seit dem 18. April 2026 wird dieses Projekt mit Qoder gepflegt.
- Qoder excelt bei Quellcodeanalyse und Domänendokumentgenerierung und leistet außergewöhnliche Arbeit beim Verständnis komplexer Codebasen.
- Adoptiert ein Null-Berechnungskosten-Preismodell, was es hocheffizient für automatisierte Dokumentverarbeitung und Routineaufgaben macht.
- Eine KI-gestützte agentic Coding-Plattform, entworfen für echte Softwareentwicklung.
- Verfügt über intelligente Codegenerierung, konversationelle Programmierung, fortgeschrittene Kontextanalyse-Engine und Multi-Agent-Kollaboration.
- Bietet tiefes Codeverständnis mit minimalem Ressourcenverbrauch, ideal für langfristige Projektwartung und Wissensakkumulation.

### Anforderungsdokument

- Das Anforderungsdokument für dieses Projekt ist nicht öffentlich verfügbar.
- Anforderungen wurden durch Iterationen mit 12+ internationalen KI-Plattformen und großen Modellserien validiert, was ein benutzergeschichtengesteuertes Anforderungsdokument von über 2000 Zeilen erzeugte, das für Menschen fast unverständlich ist.

---

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### Speichersystem-Refaktorierung
- `8dd26e3` - Vereinheitlichte ITimeStorage-Schnittstelle zur Verwendung von IncompleteDate und hierarchische Query-API hinzugefügt
  - DateTime-Überladungsmethoden aus ITimeStorage-Schnittstelle entfernt, vereinheitlicht zur Verwendung von IncompleteDate
  - CompareTo(DateTime)-Vergleichsmethode und Expand()-Erweiterungsmethode zu IncompleteDate hinzugefügt
  - GetEarliestTimestamp(), GetLatestTimestamp() hierarchische Query-API hinzugefügt
  - HasSummary()- und QueryWithLevel()-Methoden hinzugefügt, unterstützen Abfragen nach Zeitebene
  - Memory.cs Refaktorierung des Kompressionsalgorithmus, Verwendung der neuen hierarchischen Query-API zur Effizienzsteigerung
  - FileSystemTimeStorage.cs implementiert vollständig die neuen Schnittstellenmethoden
  - Synchronisierte Updates aller Aufrufer: ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord, usw.
  - Tool-System-Updates: HelpTool, LogTool, TokenAuditTool an neue Schnittstelle angepasst
  - Web-Controller-Updates: AuditController, ChatController, ChatHistoryController an neue Schnittstelle angepasst
  - Gesamt: 41 Dateien geändert (+1820/-903 Zeilen)

### 2026-04-27

#### Hilfedokumentationssystem-Erweiterung
- `9989d79` - Aktualisierte Lokalisierung, Hilfesystem und Webansichten
  - IAIClientFactoryHelp.cs KI-Clientfabrik-Hilfedokumentationsschnittstelle hinzugefügt
  - 9-Sprachen-Übersetzung für alle Hilfedokumente abgeschlossen
  - HelpTopics.cs 40 Hilfethemendefinitionen hinzugefügt
  - Webansichten umfassend aktualisiert: InitController, AuditView, ConfigView, KnowledgeView, LogView, usw.
  - Lokalisierungssystem-Erweiterung: alle Sprachversionen haben neue Lokalisierungsschlüssel hinzugefügt
  - KI-Clientfabrik-Updates: DashScopeClientFactory, OllamaClientFactory Verbesserungen

#### Hilfedokumentation neuer Inhalt
- `e7afe94` - Seelendatei- und Auditprotokoll-Hilfedokumentation hinzugefügt
  - Seelendateiverwaltung-Hilfedokumentation hinzugefügt
  - Auditprotokoll-Hilfedokumentation hinzugefügt
  - HelpTopics.cs Themendefinitionen hinzugefügt
  - HelpView.cs erheblich refaktorisiert, Dokumentrenderlogik verbessert
  - PermissionView.cs refaktorisiert, Berechtigungsverwaltungsschnittstelle verbessert
  - Kernmodul-Erweiterung: SiliconBeingManager, TaskSystem, ToolManager Verbesserungen
  - TaskTool.cs refaktorisiert, Aufgabenverwaltungsfunktionalität verbessert
  - Webansichten umfassend aktualisiert: alle Ansichtskomponenten synchronisiert
  - HelpController.cs vereinfacht, Controllerlogik optimiert

### 2026-04-26

#### Hilfedokumentationssystem
- `07895d7` - Hilfedokumentationssystem erweitert, 3 Dokumente hinzugefügt und 9-Sprachen-Übersetzung abgeschlossen
  - Speichersystem-, Ollama-Installationskonfigurations-, Alibaba Cloud Bailian-Plattformbenutzungsleitfaden hinzugefügt
  - 9-Sprachen-Übersetzung für alle 10 Hilfedokumente abgeschlossen
  - HelpView-Renderlogik vereinfacht

#### Deutsche Lokalisierung
- `0cfd8a1` - Vollständige deutsche (de-DE) Lokalisierungsunterstützung hinzugefügt
  - Vollständige deutsche Lokalisierungsdateien
  - Deutsche Unterstützung für chinesischen historischen Kalender hinzugefügt
  - Deutsche Übersetzung der Hilfedokumentation hinzugefügt
  - Alle Dokumente in 9 Sprachen vollständig synchronisiert

#### Dokumentationssynchronisation
- `3aada7d` - Traditionelle chinesische (zh-HK) Dokumentation mit vereinfachter Chinesisch synchronisiert
- `2f6abff` - Hilfstool-Anzeigename-Lokalisierung für alle Sprachen hinzugefügt

#### Wissenssystem-Refaktorierung
- `60944fe` - Namespace zu SiliconLife.Collective vereinheitlicht
- `69c51c5` - Hilfedokumentationssystem hinzugefügt und Codekommentare ins Englische übersetzt

### 2026-04-25

#### WebView-Browserautomatisierung
- `41757c3` - Plattformübergreifende WebView-Browserautomatisierung basierend auf Playwright implementiert

#### Dokumentationsupdates
- `0ff797b` - KnowledgeTool- und WorkNoteTool-Dokumentation hinzugefügt (7 Sprachen)
- `ad77415` - Alle Changelog-Dateien aktualisiert, Git-Verlauf vom 2026-04-25 hinzugefügt

#### Projektarbeitsbereichsverwaltung
- `785c551` - Projektarbeitsbereichsverwaltung mit Arbeitsnotizen und Aufgabensystem implementiert
  - Projektarbeitsbereichsverwaltungssystem hinzugefügt
  - Arbeitsnotizenfunktionalität zur Verfolgung des Projektfortschritts
  - Aufgabenverwaltungssystemintegration

#### Tschechische Lokalisierung
- `b4bbf39` - Vollständige tschechische (cs-CZ) Lokalisierung hinzugefügt und alle Sprachdokumentation aktualisiert
- `faf078f` - Tschechische Lokalisierungs-Kompilierungsfehler behoben

#### Wissenssystem-Erweiterung
- `20adaac` - KnowledgeTool mit vollständiger Lokalisierungsunterstützung hinzugefügt

### 2026-04-24

#### Speicherverwaltung-Erweiterung
- `c7b2ecc` - Speicherverwaltung mit erweitertem Filtern, Statistiken und Detailansichten erweitert
  - Erweiterte Speicherfilterung hinzugefügt
  - Speicherstatistiken implementiert
  - Speicherdetailansichtseite hinzugefügt
  - Mehrsprachige Lokalisierungsunterstützung (6 Sprachen)

#### Berechtigungssystem-Erweiterung
- `4489ad6` - wttr.in-Wetterservice zur Netzwerk-Whitelist hinzugefügt
  - Vollständige mehrsprachige Dokumentationssynchronisation (6 Sprachen)

#### Webinterface-Korrekturen
- `d9d72e9` - CSS-Prioritätsproblem im Arbeitsnotiz-Detailmodal behoben

#### Chatverlauf-Optimierung
- `0df599c` - Problem behoben, bei dem Toolergebnisse als separate Chatnachrichten gerendert wurden
- `057b09d` - Chatverlaufdetailanzeige optimiert, Toolaufrufrendering verbessert

#### Timer-Ausführungsverlauf
- `fa3f06f` - Timer-Ausführungsverlaufsfunktion mit Detailansicht hinzugefügt
- `d824835` - Timer-Ausführungsverlauf-Lokalisierungsschlüssel hinzugefügt (alle Sprachen)

#### Lokalisierungs-Erweiterung
- `c13cb17` - Spanische Sprachvariante registriert
- `9c44f34` - Mehrsprachige Lokalisierungsunterstützung für chinesischen historischen Kalender hinzugefügt

#### Kernfunktionalitätsverbesserungen
- `1e7c7b2` - Speicherkomprimierung und Toolausführungsverfolgung verbessert

### 2026-04-23

#### Tool-Lokalisierung
- `192fc6e` - Fehlende Toolnamen-Lokalisierung für 5 Tools hinzugefügt

#### Dokumentationsupdates
- `882c08f` - Alle Changelog-Dateien aktualisiert, vollständigen Git-Verlauf hinzugefügt und falsche Versionsnummern entfernt

#### Chatseite-Erweiterung
- `65c157b` - Ladeindikator zur Chatseite hinzugefügt und Kuratorsitzung automatisch ausgewählt

#### Chatverlaufsfunktion
- `e483348` - Siliziumwesen-Chatverlaufsanzeigefunktion implementiert
  - ChatHistoryController hinzugefügt
  - ChatHistoryViewModel erstellt
  - ChatHistoryListView- und ChatHistoryDetailView-Seiten implementiert
  - Lokalisierungsschlüssel für Chatverlauf hinzugefügt (5 Sprachen)

#### KI-Flusssteuerung-Erweiterung
- `30a2d4e` - KI-Flussabbruch, IM-Integration und Kernhostinitialisierung erweitert

#### Chatnachrichtenwarteschlange
- `db48c51` - Chatnachrichtenwarteschlange, Dateimetadaten und Streamabbruchunterstützung hinzugefügt

#### Dateiupload-Unterstützung
- `28fb344` - Dateiquellendialog und Dateiuploadunterstützung implementiert
- `1d3e2cc` - Dateiquellendialog-Lokalisierungszeichenfolgen hinzugefügt (6 Sprachen)

#### Dokumentationsupdates
- `8111e92` - Wiki-Link zum README-Repositoryabschnitt hinzugefügt

### 2026-04-22

#### Dokumentationslokalisierung
- `66c11eb` - Chinesische Kommentare ins Englische übersetzt und alle Changelogs aktualisiert

#### SSE-Nachrichtenerweiterung
- `b574b2b` - senderName zu historischen Nachrichten für KI-Identifikation hinzugefügt

#### Chatfunktionen
- `601fc14` - mark_read-Aktion für Sitzungsendemarkierung hinzugefügt

#### Toolsystem-Optimierung
- `7a03a19` - LogTool-Konversationsabfrageflexibilität verbessert

#### Lokalisierungs-Erweiterung
- `0a8d750` - Gemeinsamen Systemprompt für aktive Siliziumwesenverhaltens hinzugefügt

#### Protokollsystem-Refaktorierung
- `2b771f3` - LogController von Datei-I/O entkoppelt, Protokolllese-API hinzugefügt
- `12da302` - Siliziumwesenfilter zur Protokollansicht hinzugefügt
- `8f6cb1e` - beingId-Parameter zu ILogger-Schnittstelle hinzugefügt, System/Siliziumwesen-Protokolltrennung implementiert

#### Berechtigungssystem-Verbesserungen
- `4c747ad` - PermissionTool, ExecuteCodeTool refaktorisiert, EvaluatePermission-API hinzugefügt

#### Bugfixes
- `1c96e99` - Suchfehler im Stammverzeichnis von search_files und search_content behoben

#### Toolintegration
- `135710d` - SearchTool entfernt, lokale Suche zu DiskTool verschoben

#### Toolsystem-Erweiterung
- `70ce7fb` - DatabaseTool für strukturierte Datenbankabfragen implementiert
- `be29a09` - LogTool für Operations- und Konversationsverlaufabfragen implementiert
- `4ea7702` - PermissionTool für dynamische Berechtigungsverwaltung implementiert
- `1384ff4` - ExecuteCodeTool für mehrsprachige Codeausführung implementiert
- `82d1e11` - SearchTool für Informationsabruf implementiert

#### Webinterface-Optimierung
- `0675c45` - Markdown-Codeblockhervorhebung im Vorschaubereich optimiert
- `702b3f3` - Aufgabenansicht mit Statusabzeichen und Metadatenanzeige erweitert
- `6ed9a79` - Chatnachrichtenspeicher und Ansichtsrendering verbessert

### 2026-04-21

#### Bugfixes
- `c6b518b` - Timer-Nachrichtenübermittlung und Chatnachrichtenspeicher behoben

#### Konfigurationsverwaltung
- `4305769` - .gitattributes für Zeilenendeverwaltung hinzugefügt

#### Webinterface-Verbesserungen
- `188c6f8` - Aufgabenlisten-API-Route registriert und Leerezustandsanzeige hinzugefügt
- `634e8ca` - Berechtigungsseite-Rückkehr-zur-Liste-Link hinzugefügt
- `6ba591d` - Unabhängigen KI-Konfigurationseditor für Siliziumwesen hinzugefügt
- `0a826f5` - Speichererfolgshinweis im Codeeditor hinzugefügt
- `2940373` - Webinterface mit Code-Hoverhinweisen und UI-Verbesserungen erweitert

#### Berechtigungssystemkorrekturen
- `592c7ab` - Callback-Instanziierung und Registrierungsreihenfolge behoben

#### Sicherheitserweiterung
- `833ead2` - Assemblyreferenzverifizierung für dynamische Kompilierung hinzugefügt

#### Berechtigungssystem-Erweiterung
- `5879621` - Berechtigung-Callback-Vorkompilierungsverifizierung und erweiterte Fehlerbehandlung hinzugefügt

#### Dokumentationsupdates
- `4dbf659` - Changelog auf v0.5.1 aktualisiert, GitHub-Platzhalter-URLs ersetzt, Gitee-Spiegel hinzugefügt, Bilibili-Name nach Sprache lokalisiert, E-Mail aktualisiert

#### Konfiguration und Einstieg
- `0fc1693` - Programmeinstieg und Projektkonfiguration aktualisiert

#### Berechtigungssystem-Refaktorierung
- `ea9179a` - Berechtigungssystemimplementierung verbessert

#### Bugfixes
- `928a96d` - Kalenderberechnungsimplementierung behoben

#### KI und Kalender
- `646813e` - KI-Clientfabrikimplementierung verbessert

#### Lokalisierung
- `7940d9c` - Koreanische Lokalisierungsunterstützung hinzugefügt
- `4ff98ad` - Dokumentation für mehrsprachige Unterstützung refaktorisiert

### 2026-04-20

#### Kernfunktionalitätsabschluss
- `28905b5` - Vollständige mehrsprachige Unterstützung, KI-Clientfabrik, Berechtigungssystem und Lokalisierungseinstellungen
  - Protokollsystem mit Manager, Einträgen und verschiedenen Protokollebenen
  - Tokenauditssystem zum Abfragen und Verfolgen der Tokenverwendung
  - KI-Clientfabriken zur automatischen Erkennung verschiedener KI-Plattformen
  - Berechtigung-Callback-System mit eigenem Speicher
  - Konsolenprotokolliererimplementierung
  - Mehrsprachige Unterstützung für Englisch und vereinfachtes Chinesisch
  - WebUI-Messenger mit WebSocket für Echtzeit-Chat
  - Standard-Siliziumwesen mit Lokalisierung erweitert

### 2026-04-19

#### Timer und Kalender
- `c933fd8` - Lokalisierung, Timersystem, Webansichten aktualisiert und Tools hinzugefügt
  - Besserer Lokalisierungsmanager
  - Planungssystem für zeitgesteuerte Aufgaben
  - KI-Konfiguration und Kontextverwaltung
  - Kalendertool, das 32 Kalendertypen unterstützt
  - Webcontroller für Kalender-APIs
  - Aufgabenverwaltungstool

**Architekturverbesserungen**
- Webansichtsarchitektur für bessere Skinunterstützung neu gestaltet
- Wesensverwaltungssystem mit besserer Zustandsverarbeitung verbessert

### 2026-04-18

- `9f585e1` - Lokalisierung, Timersystem, Webansichten aktualisiert und Tools hinzugefügt
  - Timer- und Planungsverbesserungen
  - Bessere Webansichten mit verbesserten UI-Komponenten
  - Weitere Toolimplementierungen

### 2026-04-17

- `9b71fcd` - Kernmodule aktualisiert, zh-HK-Dokumentation, Broadcastkanal, Konfigurationstools und Auditwebansichten hinzugefügt
  - Broadcastkanal für mehrere Siliziumwesen, die zusammen chatten
  - Konfigurationstoolsystem
  - Auditwebansichten
  - Traditionelle chinesische Dokumentation

### 2026-04-16

- `5040f05` - Kern- und Standardmodule aktualisiert
  - Moduloptimierung und Bugfixes
  - Implementierungsupdates und Verbesserungen

### 2026-04-15

- `3efab5f` - Mehrere Module aktualisiert: AI, Chat, IM, Tools, Web, Localization, Storage
  - KI-Clientverbesserungen
  - Chatsystemerweiterung
  - Messengerprovider-Updates
  - Toolsystemoptimierung
  - Webinfrastrukturverbesserungen
  - Lokalisierungsoptimierung
  - Speichersystemupdates

### 2026-04-14

- `4241a2f` - Chatfunktionen grundlegend abgeschlossen, UI-Uploadoptimierung
  - Chatsystemfunktionalität abgeschlossen
  - UI-Optimierung für Dateiuploads

### 2026-04-13

- `c498c31` - Codeupdates
  - Allgemeine Codeverbesserungen und Optimierung

### 2026-04-12

#### Dokumentation und Lokalisierung
- `2161002` - Dokumentation refaktorisiert und Lokalisierung erweitert
- `03d94e4` - Konfigurationssystem und Lokalisierung erweitert
- `9976a35` - Über-Seite und Lokalisierung hinzugefügt

#### Chat und Webansichten
- `0c8ccfc` - Chatsystem, Lokalisierung und Webansichten erweitert
- `a8f1342` - Webkommunikationsschicht neu gestaltet, von WebSocket zu SSE gewechselt

### 2026-04-11

#### Protokollsystem
- `e8fe259` - Protokollsystem und Codeoptimierung hinzugefügt
- `f01c519` - Protokollsystem hinzugefügt, KI-Schnittstelle und Webansichten aktualisiert

### 2026-04-10

- `4962924` - WebSocket-Handler, Chatansichten und Messengerinteraktion erweitert
  - Kontextmanagerverbesserungen
  - Chatsystemerweiterung
  - Messengerprovider-Schnittstellenupdates
  - WebUIproviderneugestaltung
  - JavaScriptbuilder und Router-Updates
  - Chatansichtoptimierung
  - WebSockethandlerverbesserungen

### 2026-04-09

- `f9302bf` - Messengerprovider-Schnittstelle, Chatsystem und Web-UI-Interaktion erweitert
  - Messengerprovider-Schnittstellenerweiterung
  - Chatnachrichten- und Systemverbesserungen
  - Kontextmanageroptimierung
  - Standard-Siliziumwesenerweiterung
  - Web-UI-Chatansichtverbesserungen
  - WebSockethandlerupdates

### 2026-04-07

- `6831ee8` - Webansichten und JavaScriptbuilder neu gestaltet
  - Vollständige Webcontrollerneugestaltung
  - JavaScriptbuilder komplett neu geschrieben
  - Alle Ansichtskomponenten aktualisiert
  - Skinsystemverbesserungen
  - Ansichtsbasisklassenarchitekturaktualisierung

### 2026-04-05

- `41e97fb` - Mehrere Kernmodule und Webcontroller aktualisiert
  - Kontextmanagerverbesserungen
  - Chatsystem und Sitzungsverwaltung
  - Servicelocatorneugestaltung
  - Siliziumwesenbasisklasse und Managerupdates
  - Webcontroller umfassend aktualisiert (17 Controller)
  - Standard-Siliziumwesenfabrikverbesserungen
- `67988d4` - Web-UI-Modul verbessert, Executoransicht hinzugefügt, Ansichten und Kernmodule bereinigt

### 2026-04-04

- `b58bb1c` - Initialisierungscontroller hinzugefügt und Webmodul neu gestaltet
  - Initialisierungscontroller
  - Konfigurationsmodulneugestaltung
  - Lokalisierungsmodulupdates
  - Skinsystemverbesserungen
  - Routererweiterung
- `f03ac0b` - Web-UI-Modul hinzugefügt, Messengerfunktionalität verbessert

### 2026-04-03

- `192e57b` - Projektstruktur und Kernruntimekomponenten aktualisiert
- `59faec8` - Kern- und Standardimplementierungsupdates
- `d488485` - Dynamische Kompilierungsfunktionalität und Kuratortoolmodul hinzugefügt
- `753d1d9` - Sicherheitsmodul hinzugefügt, Executoren, Messengerprovider, Lokalisierung und Tools aktualisiert
- `a378697` - Stufe 5 abgeschlossen - Toolsystem + Executoren

### 2026-04-02

- `e6ad94b` - Problem beim Laden des Chatverlaufs beim Löschen von Konfigurationsdateien während des Testens behoben
- `daa56f5` - Stufe 4 abgeschlossen: persistenter Speicher (Chatsystem + Messengerkanal)

### 2026-04-01

- `bbe2dbb` - Konfigurationsladen und Chatdienstnachrichtenrouting behoben
- `2fa6305` - Stufe 2 implementiert: Hauptschleifenframework und Uhrzeitobjektsystem
- `32b99a1` - Stufe 1 implementiert - grundlegende Chatfunktionalität
- `358e368` - Initialer Commit: Projektdokumentation und Lizenz
