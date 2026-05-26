# Tool-Referenz

> **Version: v0.2.0-alpha**

Dieses Dokument beschreibt detailliert alle integrierten Tools der Silicon Life Collective-Plattform.

[English](../en/tools-reference.md) | **Deutsch** | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md) | [Русский](../ru-RU/tools-reference.md)

## Übersicht

Das Werkzeugsystem ermöglicht es Silicon Beings, über standardisierte Schnittstellen mit der Außenwelt zu interagieren. Jedes Werkzeug implementiert die `ITool`-Schnittstelle und wird vom Werkzeugmanager über Reflexion automatisch entdeckt und registriert.

### Werkzeugkategorien

- **Systemverwaltungswerkzeuge** — Konfiguration, Berechtigungen, dynamische Kompilierung, Curator-Verwaltung
- **Kommunikationswerkzeuge** — Chat, Netzwerkanfragen
- **Datenspeicherwerkzeuge** — Festplattenoperationen, Datenbank, Speicher, Arbeitsnotizen
- **Zeitverwaltungswerkzeuge** — Kalender, Timer, Aufgaben
- **Entwicklungswerkzeuge** — Codeausführung, Protokollabfrage
- **Dienstprogramme** — Systeminformationen, Token-Nutzungsaudit, Hilfedokumentation, Wissensnetzwerk
- **Browserwerkzeuge** — WebView-Browser-Automatisierung
- **Projektwerkzeuge** — Projektverwaltung, Projektaufgaben, Projekt-Arbeitsnotizen, Projektarbeit
- **Plugin-Werkzeuge** — Über das Plugin-System registrierte Drittanbieter-Werkzeuge

### Werkzeugszenario-System

Jedes Werkzeug deklariert seine verfügbaren Szenarien über das `[ToolScenario]`-Attribut:

| Szenario-Flag | Wert | Beschreibung |
|----------|------|-------------|
| `Chat` | `1 << 0` | Chat-Szenario (wenn Benutzer mit einem Silicon Being konversieren) |
| `Task` | `1 << 1` | Aufgaben-Szenario (wenn ein Silicon Being eine Aufgabe ausführt) |
| `Timer` | `1 << 2` | Timer-Szenario (wenn ein Silicon Being eine zeitgesteuerte Aufgabe ausführt) |
| `MemoryCompression` | `1 << 3` | Speicherkomprimierungsszenario |
| `Project` | `1 << 4` | Projekt-Szenario (ThinkOnProject-Modus) |
| `All` | Alle oben genannten | In allen Szenarien verfügbar |

Darüber hinaus sind mit dem `[ChatOnly]`-Attribut markierte Werkzeuge nur im Chat-Szenario verfügbar (z. B. HelpTool) und erscheinen nicht in Aufgaben- und Timer-Szenarien.

---

## Liste der integrierten Werkzeuge

### 1. Kalender-Werkzeug (CalendarTool)

**Werkzeugname**: `calendar`

**Funktionsbeschreibung**: Unterstützung für Datumsumrechnung und -berechnung in 32 Kalendersystemen.

**Unterstützte Operationen**:
- `now` — Aktuelle Zeit abrufen
- `format` — Datum formatieren
- `add_days` — Datum addieren/subtrahieren
- `diff` — Datumsdifferenz berechnen
- `list_calendars` — Alle unterstützten Kalender auflisten
- `get_components` — Datumskomponenten abrufen
- `get_now_components` — Aktuelle Zeitkomponenten abrufen
- `convert` — Zwischen Kalendersystemen konvertieren

**Unterstützte Kalendersysteme** (32):
- Gregorianischer Kalender
- Chinesischer Mondkalender (Chinese Lunar)
- Chinesische historische Kalender — Ganzhi-Jahreszählung, Kaiserära-Namen
- Islamischer Kalender (Islamic)
- Hebräischer Kalender (Hebrew)
- Japanischer Kalender (Japanese)
- Persischer Kalender (Persian)
- Maya-Kalender (Mayan)
- Buddhistischer Kalender (Buddhist)
- Tibetischer Kalender (Tibetan)
- und 24 weitere Kalender...

**Verwendungsbeispiel**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_lunar"
}
```

---

### 2. Chat-Werkzeug (ChatTool)

**Werkzeugname**: `chat`

**Funktionsbeschreibung**: Verwaltung von Chat-Sitzungen und Nachrichtensendung.

**Unterstützte Operationen**:
- `send_message` — Nachricht senden
- `get_messages` — Historische Nachrichten abrufen
- `create_group` — Gruppenchat erstellen
- `add_member` — Gruppenmitglied hinzufügen
- `remove_member` — Gruppenmitglied entfernen
- `get_chat_info` — Chat-Informationen abrufen
- `terminate_chat` — Chat beenden (gelesen, nicht beantwortet)

**Verwendungsbeispiel**:
```json
{
  "action": "send_message",
  "target_id": "being-uuid-or-user-0",
  "message": "Hallo, lassen Sie uns zusammenarbeiten!"
}
```

---

### 3. Konfigurations-Werkzeug (ConfigTool)

**Werkzeugname**: `config`

**Funktionsbeschreibung**: Lesen und Ändern der Systemkonfiguration.

**Unterstützte Operationen**:
- `read` — Konfigurationseintrag lesen
- `write` — Konfigurationseintrag schreiben
- `list` — Alle Konfigurationen auflisten
- `get_ai_config` — KI-Client-Konfiguration abrufen
- `set_ai_config` — KI-Client-Konfiguration festlegen

**Verwendungsbeispiel**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. Curator-Werkzeug (CuratorTool) 🔒

**Werkzeugname**: `silicon_manager`

**Berechtigungsanforderung**: Nur für Silicon Curator verfügbar (`[SiliconManagerOnly]`)

**Verfügbare Szenarien**: Chat, Task, Timer

**Funktionsbeschreibung**: Exklusives Systemverwaltungswerkzeug für den Silicon Curator, zur Verwaltung der Erstellung, Einsicht und des Zurücksetzens von Silicon Beings.

**Unterstützte Operationen**:
- `list_beings` — Alle Silicon Beings und deren Status auflisten
- `create_being` — Neues Silicon Being erstellen (erfordert `name`- und `soul`-Parameter)
- `get_code` — Quellcode eines Silicon Beings einsehen
- `reset` — Silicon Being auf Standardimplementierung zurücksetzen

**Verwendungsbeispiel**:
```json
{
  "action": "create_being",
  "name": "Assistent",
  "soul": "Du bist ein hilfreicher Assistent..."
}
```

---

### 5. Datenbank-Werkzeug (DatabaseTool)

**Werkzeugname**: `database`

**Funktionsbeschreibung**: Strukturierte Datenbankabfragen und -operationen.

**Unterstützte Operationen**:
- `query` — Daten abfragen
- `insert` — Daten einfügen
- `update` — Daten aktualisieren
- `delete` — Daten löschen
- `create_table` — Tabelle erstellen
- `list_tables` — Alle Tabellen auflisten

**Verwendungsbeispiel**:
```json
{
  "action": "query",
  "table": "users",
  "conditions": {"status": "active"},
  "limit": 100
}
```

---

### 6. Disk-Werkzeug (DiskTool)

**Werkzeugname**: `disk`

**Funktionsbeschreibung**: Dateisystemoperationen und lokale Suche.

**Unterstützte Operationen**:
- `read` — Datei lesen
- `write` — Datei schreiben
- `list` — Verzeichnis auflisten
- `delete` — Datei löschen
- `create_directory` — Verzeichnis erstellen
- `search_files` — Dateien suchen
- `search_content` — Dateiinhalte suchen
- `count_lines` — Zeilen zählen
- `read_lines` — Bestimmte Zeilen lesen
- `replace_text` — Text ersetzen

**Berechtigungsanforderung**: `FileAccess`

**Verwendungsbeispiel**:
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

---

### 7. Dynamische Kompilierungs-Werkzeug (DynamicCompileTool) 🔒

**Werkzeugname**: `compile`

**Funktionsbeschreibung**: Dynamisches Kompilieren von C#-Code (für die Selbstevolution von Silicon Beings).

**Unterstützte Operationen**:
- `compile_class` — Klasse kompilieren
- `compile_callback` — Berechtigungs-Callback-Funktion kompilieren
- `validate_code` — Codesicherheit validieren

**Sicherheitsmechanismen**:
- Referenzkontrolle bei der Kompilierung (gefährliche Assemblys ausschließen)
- Statische Code-Analyse zur Laufzeit
- AES-256-verschlüsselte Speicherung

**Verwendungsbeispiel**:
```json
{
  "action": "compile_class",
  "code": "public class MyBeing : SiliconBeingBase { ... }"
}
```

---

### 8. Codeausführungs-Werkzeug (ExecuteCodeTool) 🔒

**Werkzeugname**: `execute_code`

**Berechtigungsanforderung**: Nur für Silicon Curator verfügbar

**Funktionsbeschreibung**: Kompilieren und Ausführen von C#-Code-Snippets.

**Unterstützte Operationen**:
- `run_script` — Code-Skript ausführen

**Verwendungsbeispiel**:
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

---

### 9. Hilfe-Werkzeug (HelpTool)

**Werkzeugname**: `help`

**Verfügbare Szenarien**: Chat (`[ChatOnly]`, nur im Chat-Szenario verfügbar)

**Funktionsbeschreibung**: Suchen und Abrufen von Systemhilfedokumentation, ermöglicht es der KI, die Verwendung von Systemfunktionen abzufragen.

**Unterstützte Operationen**:
- `list` — Alle Hilfethemen-IDs auflisten
- `search` — Hilfedokumentation nach Schlüsselwort durchsuchen
- `get` — Hilfedokumentation für eine bestimmte ID abrufen

**Verwendungsbeispiel**:
```json
{
  "action": "search",
  "keyword": "Berechtigung"
}
```

---

### 10. Wissensnetzwerk-Werkzeug (KnowledgeTool)

**Werkzeugname**: `knowledge`

**Funktionsbeschreibung**: Wissensgraph-Operationen (basierend auf Tripeln: Subjekt-Prädikat-Objekt).

**Unterstützte Operationen**:
- `add` — Wissens-Tripel hinzufügen
- `query` — Wissen abfragen
- `update` — Wissen aktualisieren
- `delete` — Wissen löschen
- `search` — Wissen suchen
- `get_path` — Wissenspfad abrufen
- `validate` — Wissen validieren
- `stats` — Statistiken abrufen

**Verwendungsbeispiel**:
```json
{
  "action": "add",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95
}
```

---

### 11. Protokoll-Werkzeug (LogTool)

**Werkzeugname**: `log`

**Funktionsbeschreibung**: Abfrage der Operationshistorie und Chat-Historie.

**Unterstützte Operationen**:
- `query_logs` — Systemprotokolle abfragen
- `query_conversations` — Chat-Historie abfragen
- `get_stats` — Protokollstatistiken abrufen

**Verwendungsbeispiel**:
```json
{
  "action": "query_logs",
  "being_id": "being-uuid",
  "start_time": "2026-04-20T00:00:00Z",
  "end_time": "2026-04-26T23:59:59Z",
  "level": "info"
}
```

---

### 12. Speicher-Werkzeug (MemoryTool)

**Werkzeugname**: `memory`

**Funktionsbeschreibung**: Verwaltung des Langzeit- und Kurzzeitspeichers von Silicon Beings.

**Unterstützte Operationen**:
- `read` — Speicher lesen
- `write` — Speicher schreiben
- `search` — Speicher durchsuchen
- `delete` — Speicher löschen
- `list` — Speichereinträge auflisten
- `get_stats` — Speicherstatistiken abrufen
- `compress` — Speicher komprimieren

**Verwendungsbeispiel**:
```json
{
  "action": "read",
  "key": "important_fact",
  "time_range": {
    "start": "2026-04-01",
    "end": "2026-04-26"
  }
}
```

---

### 13. Netzwerk-Werkzeug (NetworkTool)

**Werkzeugname**: `network`

**Funktionsbeschreibung**: HTTP/HTTPS-Anfragen durchführen.

**Unterstützte Operationen**:
- `get` — GET-Anfrage
- `post` — POST-Anfrage
- `put` — PUT-Anfrage
- `delete` — DELETE-Anfrage
- `download` — Datei herunterladen
- `upload` — Datei hochladen

**Berechtigungsanforderung**: `network:http`

**Verwendungsbeispiel**:
```json
{
  "action": "get",
  "url": "https://api.example.com/data"
}
```

---

### 14. Berechtigungs-Werkzeug (PermissionTool) 🔒

**Werkzeugname**: `permission`

**Berechtigungsanforderung**: Nur für Silicon Curator verfügbar

**Funktionsbeschreibung**: Verwaltung von Berechtigungen und Zugriffssteuerungslisten.

**Unterstützte Operationen**:
- `query_permission` — Berechtigung abfragen
- `manage_acl` — Globale ACL verwalten
- `get_callback` — Berechtigungs-Callback-Funktion abrufen
- `set_callback` — Berechtigungs-Callback-Funktion festlegen

**Verwendungsbeispiel**:
```json
{
  "action": "manage_acl",
  "acl_action": "add_rule",
  "permission_type": "filesystem",
  "resource_prefix": "/data/",
  "acl_result": "allow"
}
```

---

### 15. Projekt-Werkzeug (ProjectTool) 🔒

**Werkzeugname**: `project`

**Berechtigungsanforderung**: Nur für Silicon Curator verfügbar (`[SiliconManagerOnly]`)

**Verfügbare Szenarien**: Chat, Task, Timer

**Funktionsbeschreibung**: Verwaltung von Projektarbeitsbereichen, unterstützt Projekt-Lebenszyklusverwaltung, Mitgliederzuweisung und Rollenverwaltung.

**Unterstützte Operationen**:
- `create` — Neuen Projektbereich erstellen
- `archive` — Projekt archivieren
- `restore` — Archiviertes Projekt wiederherstellen
- `destroy` — Projekt zerstören und Daten bereinigen (unwiderruflich)
- `list` — Alle Projekte auflisten
- `get` — Projektdetails abrufen
- `assign` — Silicon Being einem Projekt zuweisen
- `remove` — Silicon Being aus einem Projekt entfernen
- `update` — Projektname/-beschreibung aktualisieren
- `list-workflow-templates` — Verfügbare Workflow-Vorlagen auflisten
- `assign_role` — Silicon Being eine Projektrolle zuweisen
- `remove_role` — Projektrolle eines Silicon Beings entfernen
- `list_roles` — Rollenzuweisungen des Projekts auflisten

**Verwendungsbeispiel**:
```json
{
  "action": "create",
  "name": "My Project",
  "description": "Projektbeschreibung"
}
```

---

### 16. Projektaufgaben-Werkzeug (ProjectTaskTool)

**Werkzeugname**: `project_task`

**Verfügbare Szenarien**: Chat, Task, Timer

**Funktionsbeschreibung**: Verwaltung von Aufgaben innerhalb eines Projektbereichs, unterstützt den vollständigen Aufgaben-Lebenszyklus.

**Unterstützte Operationen**:
- `create` — Projektaufgabe erstellen
- `list` — Projektaufgaben auflisten
- `get` — Aufgabendetails abrufen
- `update` — Aufgabentitel/-beschreibung/-priorität aktualisieren
- `assign` — Verantwortlichen einer Aufgabe zuweisen
- `remove_assignee` — Verantwortlichen einer Aufgabe entfernen
- `start` — Aufgabe starten
- `complete` — Aufgabe als abgeschlossen markieren
- `fail` — Aufgabe als fehlgeschlagen markieren
- `cancel` — Aufgabe abbrechen
- `delete` — Aufgabe löschen
- `stats` — Aufgabenstatistiken abrufen

**Verwendungsbeispiel**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "Aufgabenbeschreibung abschließen",
  "priority": 5
}
```

---

### 17. Projekt-Arbeitsnotiz-Werkzeug (ProjectWorkNoteTool)

**Werkzeugname**: `project_work_note`

**Verfügbare Szenarien**: Chat, Task, Timer

**Funktionsbeschreibung**: Verwaltung von Arbeitsnotizen innerhalb eines Projektbereichs (öffentlich, ähnlich einem Arbeitsheft), unterstützt seitenbasierte Notizverwaltung.

**Unterstützte Operationen**:
- `create` — Notizseite erstellen (erfordert `project_id`, `summary` und `content`, optional `keywords`)
- `read` — Notizseite lesen (erfordert `project_id` und `page_number` oder `note_id`)
- `update` — Notizseite aktualisieren (erfordert `project_id`, `page_number` und `content`, optional `summary` und `keywords`)
- `delete` — Notizseite löschen (erfordert `project_id` und `page_number` oder `note_id`)
- `list` — Alle Notizseiten-Zusammenfassungen des Projekts auflisten
- `directory` — Notizverzeichnis/-übersicht generieren
- `search` — Notizen nach Schlüsselwort durchsuchen (erfordert `project_id` und `keyword`, optional `max_results`)

**Verwendungsbeispiel**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Benutzerauthentifizierungsmodul abgeschlossen",
  "content": "## Implementierungsdetails\n\n- JWT-Token verwenden",
  "keywords": "Authentifizierung,JWT"
}
```

---

### 18. Projektarbeit-Werkzeug (ProjectWorkTool) 🔒

**Werkzeugname**: `project_work`

**Berechtigungsanforderung**: Nur für Silicon Curator verfügbar (`[SiliconManagerOnly]`)

**Verfügbare Szenarien**: Project (`[ToolScenario(ToolScenarioFlag.Project)]`, nur im Projekt-Szenario verfügbar)

**Funktionsbeschreibung**: Projektarbeits-Operationswerkzeug, das vom Curator zur Verwaltung von Projekt-Workflows im ThinkOnProject-Szenario verwendet wird.

**Unterstützte Operationen**:
- `create-task` — Projektaufgabe erstellen
- `assign-task` — Silicon Being einer Aufgabe zuweisen
- `chat` — Nachricht an Projekt-Gruppenchat senden
- `broadcast` — Nachricht an Projekt-Kanal senden
- `complete` — Projekt als abgeschlossen markieren
- `status` — Projektstatus abrufen

**Verwendungsbeispiel**:
```json
{
  "action": "create-task",
  "project_id": "project-uuid",
  "title": "Benutzerauthentifizierung implementieren"
}
```

---

### 19. System-Werkzeug (SystemTool)

**Werkzeugname**: `system`

**Funktionsbeschreibung**: Systeminformationen und Ressourcennutzung abrufen.

**Unterstützte Operationen**:
- `info` — Systeminformationen abrufen
- `resource_usage` — Ressourcennutzung abrufen
- `find_process` — Prozess finden
- `list_beings` — Silicon Beings auflisten

**Verwendungsbeispiel**:
```json
{
  "action": "info"
}
```

---

### 20. Aufgaben-Werkzeug (TaskTool)

**Werkzeugname**: `task`

**Funktionsbeschreibung**: Verwaltung der persönlichen Aufgaben eines Silicon Beings.

**Unterstützte Operationen**:
- `create` — Aufgabe erstellen
- `list` — Aufgaben auflisten
- `update` — Aufgabe aktualisieren
- `complete` — Aufgabe abschließen
- `delete` — Aufgabe löschen
- `get_dependencies` — Abhängigkeiten abrufen

**Verwendungsbeispiel**:
```json
{
  "action": "create",
  "description": "Code überprüfen",
  "priority": 5
}
```

---

### 21. Timer-Werkzeug (TimerTool)

**Werkzeugname**: `timer`

**Funktionsbeschreibung**: Erstellen und Verwalten von Timern.

**Unterstützte Operationen**:
- `create` — Timer erstellen
- `list` — Timer auflisten
- `delete` — Timer löschen
- `pause` — Timer anhalten
- `resume` — Timer fortsetzen
- `get_execution_history` — Ausführungshistorie abrufen

**Verwendungsbeispiel**:
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true,
  "message": "Stündliche Erinnerung"
}
```

---

### 22. Token-Nutzungsaudit-Werkzeug (TokenAuditTool) 🔒

**Werkzeugname**: `token_audit`

**Berechtigungsanforderung**: Nur für Silicon Curator verfügbar (`[SiliconManagerOnly]`)

**Verfügbare Szenarien**: Chat, Task, Timer

**Funktionsbeschreibung**: Abfrage von KI-Token-Nutzungsstatistiken und Trenddaten.

**Unterstützte Operationen**:
- `summary` — Token-Nutzungszusammenfassung abrufen
- `trend` — Token-Nutzungstrend-Datenpunkte abrufen

**Unterstützte Zeitbereiche**:
- `today` — Letzte 24 Stunden
- `week` — Letzte 7×24 Stunden
- `month` — Tagesweise Statistik
- `year` — Monatsweise Statistik

**Verwendungsbeispiel**:
```json
{
  "action": "summary",
  "time_range": "week"
}
```

---

### 23. WebView-Browser-Werkzeug (WebViewBrowserTool)

**Werkzeugname**: `webview_browser`

**Verfügbare Szenarien**: Chat, Task, Timer

**Funktionsbeschreibung**: Playwright-basierte Browser-Automatisierung, bietet vollständige Web-Navigation, Interaktion und Datenextraktion.

**Unterstützte Operationen**:
- `open` — Browser öffnen
- `close` — Browser schließen
- `navigate` — Zu URL navigieren
- `click` — Element anklicken
- `input` — Text eingeben
- `scroll` — Seite scrollen
- `execute_script` — JavaScript ausführen
- `get_page_text` — Seitentext abrufen
- `get_screenshot` — Screenshot abrufen
- `wait_for_element` — Auf Element warten
- `get_element_info` — Elementinformationen abrufen
- `upload_file` — Datei hochladen
- `get_browser_status` — Browserstatus abrufen
- `set_timeout` — Zeitüberschreitung festlegen
- `clear_session` — Browsersitzung löschen

**Eigenschaften**:
- Unabhängige Instanz pro Silicon Being
- Vollständig isolierte Cookies und Sitzungen
- Für Benutzer vollständig unsichtbar (Headless-Modus)
- Vollständige JavaScript- und CSS-Unterstützung

**Verwendungsbeispiel**:
```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

---

### 24. Arbeitsnotiz-Werkzeug (WorkNoteTool)

**Werkzeugname**: `work_note`

**Funktionsbeschreibung**: Verwaltung der persönlichen Arbeitsnotizen eines Silicon Beings (privat, ähnlich einem Tagebuch).

**Unterstützte Operationen**:
- `create` — Notiz erstellen
- `read` — Notiz lesen
- `update` — Notiz aktualisieren
- `delete` — Notiz löschen
- `list` — Notizen auflisten
- `search` — Notizen durchsuchen
- `directory` — Verzeichnis generieren

**Verwendungsbeispiel**:
```json
{
  "action": "create",
  "summary": "Benutzerauthentifizierungsmodul abgeschlossen",
  "content": "## Implementierungsdetails\n\n- JWT-Token verwenden\n- OAuth2 unterstützen",
  "keywords": "Authentifizierung,JWT,OAuth2"
}
```

---

### 25. Hot-Reload-Werkzeug (HotReloadTool)

**Werkzeugname**: `hot_reload`

**Funktionsbeschreibung**: Unterstützt automatische Kompilierung, Dateiaktualisierung und Neustart von SiliconLife.Fast während der Laufzeit ohne manuellen Eingriff.

**Unterstützte Operationen**:
- `execute` — Vollständigen Build-, Kopier- und Neustart-Prozess ausführen
- `build_only` — Nur das Projekt kompilieren, ohne Kopieren und Neustarten

**Arbeitsablauf**:
1. SiliconLife.Fast-Projekt kompilieren
2. Aktuell laufende Fast-Instanz ordnungsgemäß herunterfahren (über HTTP-API)
3. Auf Prozessbeendigung und Portfreigabe warten
4. Build-Ausgabe in Zielverzeichnis kopieren (HotReload-eigene Dateien überspringen)
5. Fast-Instanz neu starten

**Eigenschaften**:
- Automatische Erkennung und Beendigung alter Prozesse
- Sicheres Dateikopieren (HotReload.exe wird nicht überschrieben)
- Portfreigabe-Wartemechanismus
- Unterstützung für benutzerdefinierte Portkonfiguration

**Verwendungsbeispiel**:
```json
{
  "action": "execute",
  "project_path": "src/SiliconLife.Fast",
  "source_path": "src/SiliconLife.Fast/bin/Debug/net9.0",
  "configuration": "Debug",
  "port": 8080
}
```

**Parameterbeschreibung**:
- `project_path`: Projektpfad (relativ zum Lösungs-Stammverzeichnis)
- `source_path`: Build-Ausgabeverzeichnis
- `configuration`: Build-Konfiguration (Debug/Release)
- `port`: Web-Port der Fast-Instanz (Standard 8080)

**Hinweise**:
- Nur für die SiliconLife.Fast-Version anwendbar
- Erfordert HotReload.exe im Verzeichnis tools/HotReload
- Während des Neustarts gibt es eine kurze Dienstunterbrechung (ca. 3-5 Sekunden)

---

## Werkzeugaufruf-Prozess

```
┌──────────┐
│   AI     │ Gibt tool_calls zurück
└────┬─────┘
     ↓
┌──────────────┐
│ Werkzeug-    │ Sucht und validiert Werkzeugnutzungsberechtigung
│ manager      │
└────┬─────────┘
     ↓
┌──────────────┐
│ Berechtigungs│ Prüft Berechtigungskette
│ manager      │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ Führt Ressourcenzugriffsoperationen aus
└────┬─────────┘
     ↓
┌──────────┐
│   AI     │ Empfängt Werkzeugergebnis, denkt weiter
└──────────┘
```

## Berechtigungsvalidierung

Alle Werkzeugausführungen durchlaufen die Berechtigungsvalidierungskette:

1. **Benutzerfrequenz-Cache** — Hochfrequente Benutzerentscheidungen zwischengespeichert (HighDeny hat Vorrang vor HighAllow)
2. **Berechtigungs-Callback-Schnittstelle** — Benutzerdefinierte Berechtigungs-Callback-Funktion (Allowed/Denied/AskUser)
3. **IsCurator-Verzweigung** — Curator fragt den Benutzer über IPermissionAskHandler; Nicht-Curator fragt GlobalACL ab, bei fehlender passender Regel wird standardmäßig abgelehnt

## Benutzerdefinierte Werkzeuge erstellen

### Schritt 1: ITool-Schnittstelle implementieren

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "Werkzeugbeschreibung";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, object>
        {
            ["param1"] = new { type = "string", description = "Parameterbeschreibung" }
        }
    };
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        try
        {
            var param1 = call.Parameters["param1"]?.ToString();
            var result = await DoWork(param1);
            
            return new ToolResult
            {
                Success = true,
                Output = result
            };
        }
        catch (Exception ex)
        {
            return new ToolResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }
}
```

### Schritt 2: Zum Projekt hinzufügen

Die Werkzeugdatei im Verzeichnis `src/SiliconLife.Common/Tools/` ablegen (gemeinsame Werkzeuge) oder in `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` (versionsspezifische Werkzeuge). Der Werkzeugmanager entdeckt und registriert diese beim Start automatisch über Reflexion.

### Schritt 2a: Werkzeug über Plugin registrieren

Benutzerdefinierte Werkzeuge können auch über das Plugin-System registriert werden:

1. `ITool`-Schnittstelle im Plugin-Projekt implementieren
2. Plugin-DLL kompilieren und im Plugin-Verzeichnis ablegen
3. `ToolManager.ScanAllPluginAssemblies()` scannt automatisch alle geladenen Plugins nach ITool-Implementierungen
4. Plugin-Werkzeuge unterliegen demselben Berechtigungssystem

### Schritt 3: (Optional) Als Curator-exklusiv markieren

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // Nur für Silicon Curator zugänglich
}
```

## Bewährte Praktiken

### 1. Parameter immer validieren

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("Erforderlicher Parameter fehlt: required_param");
}
```

### 2. Fehler elegant behandeln

```csharp
try
{
    // Operation ausführen
}
catch (Exception ex)
{
    Logger.Error($"Werkzeug {Name} Ausführung fehlgeschlagen: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. Berechtigungssystem respektieren

Berechtigungsprüfungen niemals umgehen. Immer über den Executor auf Ressourcen zugreifen:

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return ToolResult.Denied("Permission denied");
}
```

### 4. Klare Werkzeugbeschreibung bereitstellen

Hilft der KI zu verstehen, wann und wie das Werkzeug zu verwenden ist:

```csharp
public string Description => 
    "Zur Konvertierung von Daten zwischen verschiedenen Kalendersystemen." +
    "Erfordert die Parameter 'date', 'from_calendar' und 'to_calendar'.";
```

## Fehlerbehebung

### Werkzeug nicht gefunden

**Problem**: Die KI versucht, ein nicht existierendes Werkzeug aufzurufen.

**Lösung**:
- Prüfen, ob der Werkzeugname exakt übereinstimmt
- Verifizieren, dass die Werkzeugdatei im `Tools/`-Verzeichnis liegt
- Projekt neu erstellen (`dotnet build`)

### Berechtigung verweigert

**Problem**: Werkzeugausführung schlägt fehl, gibt Berechtigungsfehler zurück.

**Lösung**:
- Berechtigungsaudit-Protokoll prüfen
- Verifizieren, dass das Silicon Being die erforderlichen Berechtigungen hat
- Globale ACL-Einstellungen überprüfen
- Wenn es sich um den Curator handelt, prüfen, ob das `[SiliconManagerOnly]`-Attribut verwendet wurde

### Werkzeugausführung gibt Fehler zurück

**Problem**: Werkzeug wird ausgeführt, gibt aber ein fehlgeschlagenes Ergebnis zurück.

**Lösung**:
- Die vom Werkzeug zurückgegebene Fehlermeldung prüfen
- Eingabeparameterformat validieren
- Systemprotokolle für detaillierte Fehlerinformationen einsehen
- Werkzeugfunktionalität unabhängig testen

## Nächste Schritte

- 📚 [Architektur-Leitfaden](architecture.md) lesen
- 🛠️ [Entwicklungsleitfaden](development-guide.md) ansehen
- 🔒 [Berechtigungssystem](permission-system.md) verstehen
- 🚀 [Schnellstart-Leitfaden](getting-started.md) ansehen
