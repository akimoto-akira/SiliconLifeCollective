# Tool-Referenz

Dieses Dokument beschreibt detailliert alle integrierten Tools der Silicon Life Collective-Plattform.

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md)

## Übersicht

Das Tool-System ermöglicht Silicon Beings die Interaktion mit der Außenwelt über standardisierte Schnittstellen. Jedes Tool implementiert die `ITool`-Schnittstelle und wird vom `ToolManager` automatisch durch Reflektion entdeckt und registriert.

### Tool-Kategorien

- **Systemverwaltungstools** — Konfiguration, Berechtigungen, dynamische Kompilierung
- **Kommunikationstools** — Chat, Netzwerkanfragen
- **Datenspeicherungstools** — Festplattenoperationen, Datenbank, Speicher, Arbeitsnotizen
- **Zeitmanagement-Tools** — Kalender, Timer, Aufgaben
- **Entwicklungstools** — Codeausführung, Protokollabfragen
- **Dienstprogramme** — Systeminformationen, Token-Audit, Hilfedokumentation, Wissensnetzwerk
- **Browser-Tools** — WebView-Browserautomatisierung

---

## Integrierte Tool-Liste

### 1. Kalender-Tool (CalendarTool)

**Tool-Name**: `calendar`

**Funktionsbeschreibung**: Unterstützt Datumskonvertierung und -berechnung für 32 Kalendersysteme.

**Unterstützte Aktionen**:
- `now` — Aktuelle Zeit abrufen
- `format` — Datum formatieren
- `add_days` — Datum addieren/subtrahieren
- `diff` — Datumsdifferenz berechnen
- `list_calendars` — Alle unterstützten Kalender auflisten
- `get_components` — Datumskomponenten abrufen
- `get_now_components` — Aktuelle Zeitkomponenten abrufen
- `convert` — Zwischen Kalendersystemen konvertieren

**Unterstützte Kalendersysteme** (32):
- Gregorianischer Kalender (Gregorian)
- Chinesischer Lunarkalender (Chinese Lunar)
- Chinesischer historischer Kalender (Chinese Historical) — Ganzhi-Zyklus, Kaiser-Ären
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

### 2. Chat-Tool (ChatTool)

**Tool-Name**: `chat`

**Funktionsbeschreibung**: Verwaltung von Chat-Sitzungen und Nachrichtenversand.

**Unterstützte Aktionen**:
- `send_message` — Nachricht senden
- `get_messages` — Historische Nachrichten abrufen
- `create_group` — Gruppenchat erstellen
- `add_member` — Gruppenmitglied hinzufügen
- `remove_member` — Gruppenmitglied entfernen
- `get_chat_info` — Chat-Informationen abrufen
- `terminate_chat` — Chat beenden (gelesen, keine Antwort)

**Verwendungsbeispiel**:
```json
{
  "action": "send_message",
  "target_id": "being-uuid-or-user-0",
  "message": "你好，让我们协作吧！"
}
```

---

### 3. Konfigurations-Tool (ConfigTool)

**Tool-Name**: `config`

**Funktionsbeschreibung**: Systemkonfiguration lesen und ändern.

**Unterstützte Aktionen**:
- `read` — Konfigurationseintrag lesen
- `write` — Konfigurationseintrag schreiben
- `list` — Alle Konfigurationen auflisten
- `get_ai_config` — KI-Client-Konfiguration abrufen
- `set_ai_config` — KI-Client-Konfiguration setzen

**Verwendungsbeispiel**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. Curator-Tool (CuratorTool) 🔒

**Tool-Name**: `curator`

**Berechtigungsanforderung**: Nur für Silicon Curator

**Funktionsbeschreibung**: Systemverwaltungstool exklusiv für Silicon Curator.

**Unterstützte Aktionen**:
- `create_being` — Neues Silicon Being erstellen
- `list_beings` — Alle Silicon Beings auflisten
- `get_being_info` — Being-Informationen abrufen
- `assign_task` — Aufgabe zuweisen
- `manage_permissions` — Berechtigungen verwalten

**Verwendungsbeispiel**:
```json
{
  "action": "create_being",
  "name": "助手",
  "soul_file": "assistant_soul.md"
}
```

---

### 5. Datenbank-Tool (DatabaseTool)

**Tool-Name**: `database`

**Funktionsbeschreibung**: Strukturierte Datenbankabfragen und -operationen.

**Unterstützte Aktionen**:
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

### 6. Festplatten-Tool (DiskTool)

**Tool-Name**: `disk`

**Funktionsbeschreibung**: Dateisystemoperationen und lokale Suche.

**Unterstützte Aktionen**:
- `read` — Datei lesen
- `write` — Datei schreiben
- `list` — Verzeichnis auflisten
- `delete` — Datei löschen
- `create_directory` — Verzeichnis erstellen
- `search_files` — Dateien suchen
- `search_content` — Dateiinhalt suchen
- `count_lines` — Zeilen zählen
- `read_lines` — Bestimmte Zeilen lesen
- `replace_text` — Text ersetzen

**Berechtigungsanforderung**: `disk:read`, `disk:write`

**Verwendungsbeispiel**:
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

---

### 7. Dynamisches Kompilierungs-Tool (DynamicCompileTool) 🔒

**Tool-Name**: `compile`

**Funktionsbeschreibung**: Dynamische C#-Code-Kompilierung (für Silicon Being-Selbstevolution).

**Unterstützte Aktionen**:
- `compile_class` — Klasse kompilieren
- `compile_callback` — Berechtigungs-Callback-Funktion kompilieren
- `validate_code` — Codesicherheit validieren

**Sicherheitsmechanismen**:
- Referenzkontrolle bei Kompilierung (gefährliche Assemblies ausgeschlossen)
- Statische Code-Scans zur Laufzeit
- AES-256 verschlüsselte Speicherung

**Verwendungsbeispiel**:
```json
{
  "action": "compile_class",
  "code": "public class MyBeing : SiliconBeingBase { ... }"
}
```

---

### 8. Codeausführungs-Tool (ExecuteCodeTool) 🔒

**Tool-Name**: `execute_code`

**Berechtigungsanforderung**: Nur für Silicon Curator

**Funktionsbeschreibung**: C#-Code-Snippets kompilieren und ausführen.

**Unterstützte Aktionen**:
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

### 9. Hilfe-Tool (HelpTool)

**Tool-Name**: `help`

**Funktionsbeschreibung**: Systemhilfedokumentation und Bedienungsanleitungen abrufen.

**Unterstützte Aktionen**:
- `get_topics` — Hilfe-Themenliste abrufen
- `get_topic` — Bestimmtes Thema-Details abrufen
- `search` — Hilfedokumentation suchen

**Verwendungsbeispiel**:
```json
{
  "action": "get_topics"
}
```

---

### 10. Wissensnetzwerk-Tool (KnowledgeTool)

**Tool-Name**: `knowledge`

**Funktionsbeschreibung**: Wissensgraph-Operationen (basierend auf Tripeln: Subjekt-Prädikat-Objekt).

**Unterstützte Aktionen**:
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

### 11. Protokoll-Tool (LogTool)

**Tool-Name**: `log`

**Funktionsbeschreibung**: Operationshistorie und Konversationsverlauf abfragen.

**Unterstützte Aktionen**:
- `query_logs` — Systemprotokolle abfragen
- `query_conversations` — Konversationsverlauf abfragen
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

### 12. Speicher-Tool (MemoryTool)

**Tool-Name**: `memory`

**Funktionsbeschreibung**: Langzeit- und Kurzzeitspeicher von Silicon Beings verwalten.

**Unterstützte Aktionen**:
- `read` — Speicher lesen
- `write` — Speicher schreiben
- `search` — Speicher suchen
- `delete` — Speicher löschen
- `list` — Speicher auflisten
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

### 13. Netzwerk-Tool (NetworkTool)

**Tool-Name**: `network`

**Funktionsbeschreibung**: HTTP/HTTPS-Anfragen initiieren.

**Unterstützte Aktionen**:
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

### 14. Berechtigungs-Tool (PermissionTool) 🔒

**Tool-Name**: `permission`

**Berechtigungsanforderung**: Nur für Silicon Curator

**Funktionsbeschreibung**: Berechtigungen und Access Control Lists verwalten.

**Unterstützte Aktionen**:
- `query_permission` — Berechtigungen abfragen
- `manage_acl` — Globale ACL verwalten
- `get_callback` — Berechtigungs-Callback-Funktion abrufen
- `set_callback` — Berechtigungs-Callback-Funktion setzen

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

### 15. Projekt-Tool (ProjectTool)

**Tool-Name**: `project`

**Funktionsbeschreibung**: Projekt-Workspaces verwalten.

**Unterstützte Aktionen**:
- `create` — Projekt erstellen
- `list` — Projekte auflisten
- `get_info` — Projektinformationen abrufen
- `update` — Projekt aktualisieren
- `archive` — Projekt archivieren

**Verwendungsbeispiel**:
```json
{
  "action": "create",
  "name": "My Project",
  "description": "项目描述"
}
```

---

### 16. Projekt-Aufgaben-Tool (ProjectTaskTool)

**Tool-Name**: `project_task`

**Funktionsbeschreibung**: Projektaufgaben verwalten.

**Unterstützte Aktionen**:
- `create` — Aufgabe erstellen
- `list` — Aufgaben auflisten
- `update` — Aufgabe aktualisieren
- `complete` — Aufgabe abschließen
- `get_stats` — Aufgabenstatistiken abrufen

**Verwendungsbeispiel**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "完成任务描述",
  "priority": 5
}
```

---

### 17. Projekt-Arbeitsnotizen-Tool (ProjectWorkNoteTool)

**Tool-Name**: `project_work_note`

**Funktionsbeschreibung**: Projekt-Arbeitsnotizen verwalten (öffentlich, ähnlich einem Arbeitsbuch).

**Unterstützte Aktionen**:
- `create` — Notiz erstellen
- `read` — Notiz lesen
- `update` — Notiz aktualisieren
- `delete` — Notiz löschen
- `list` — Notizen auflisten
- `search` — Notizen suchen
- `directory` — Inhaltsverzeichnis generieren

**Verwendungsbeispiel**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "完成用户认证模块",
  "content": "## 实现细节\n\n- 使用 JWT token",
  "keywords": "认证,JWT"
}
```

---

### 18. System-Tool (SystemTool)

**Tool-Name**: `system`

**Funktionsbeschreibung**: Systeminformationen und Ressourcennutzung abrufen.

**Unterstützte Aktionen**:
- `info` — Systeminformationen abrufen
- `resource_usage` — Ressourcennutzung abrufen
- `find_process` — Prozess suchen
- `list_beings` — Silicon Beings auflisten

**Verwendungsbeispiel**:
```json
{
  "action": "info"
}
```

---

### 19. Aufgaben-Tool (TaskTool)

**Tool-Name**: `task`

**Funktionsbeschreibung**: Persönliche Aufgaben von Silicon Beings verwalten.

**Unterstützte Aktionen**:
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
  "description": "审查代码",
  "priority": 5
}
```

---

### 20. Timer-Tool (TimerTool)

**Tool-Name**: `timer`

**Funktionsbeschreibung**: Timer erstellen und verwalten.

**Unterstützte Aktionen**:
- `create` — Timer erstellen
- `list` — Timer auflisten
- `delete` — Timer löschen
- `pause` — Timer pausieren
- `resume` — Timer fortsetzen
- `get_execution_history` — Ausführungshistorie abrufen

**Verwendungsbeispiel**:
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true,
  "message": "每小时提醒"
}
```

---

### 21. Token-Audit-Tool (TokenAuditTool) 🔒

**Tool-Name**: `token_audit`

**Berechtigungsanforderung**: Nur für Silicon Curator

**Funktionsbeschreibung**: AI-Token-Nutzung abfragen und zusammenfassen.

**Unterstützte Aktionen**:
- `get_usage` — Token-Nutzungsstatistiken abrufen
- `get_by_being` — Nutzung nach Being abrufen
- `get_by_model` — Nutzung nach Modell abrufen
- `get_trend` — Nutzungstrend abrufen
- `export` — Daten exportieren

**Verwendungsbeispiel**:
```json
{
  "action": "get_usage",
  "start_date": "2026-04-01",
  "end_date": "2026-04-26"
}
```

---

### 22. WebView-Browser-Tool (WebViewBrowserTool)

**Tool-Name**: `webview`

**Funktionsbeschreibung**: Browserautomatisierung basierend auf Playwright.

**Unterstützte Aktionen**:
- `open_browser` — Browser öffnen
- `close_browser` — Browser schließen
- `navigate` — Zu URL navigieren
- `click` — Element anklicken
- `input` — Text eingeben
- `get_page_text` — Seitentext abrufen
- `get_screenshot` — Screenshot abrufen
- `execute_script` — JavaScript ausführen
- `wait_for_element` — Auf Element warten
- `get_browser_status` — Browserstatus abrufen

**Features**:
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

### 23. Arbeitsnotizen-Tool (WorkNoteTool)

**Tool-Name**: `work_note`

**Funktionsbeschreibung**: Persönliche Arbeitsnotizen von Silicon Beings verwalten (privat, ähnlich einem Tagebuch).

**Unterstützte Aktionen**:
- `create` — Notiz erstellen
- `read` — Notiz lesen
- `update` — Notiz aktualisieren
- `delete` — Notiz löschen
- `list` — Notizen auflisten
- `search` — Notizen suchen
- `directory` — Inhaltsverzeichnis generieren

**Verwendungsbeispiel**:
```json
{
  "action": "create",
  "summary": "完成用户认证模块",
  "content": "## 实现细节\n\n- 使用 JWT token\n- 支持 OAuth2",
  "keywords": "认证,JWT,OAuth2"
}
```

---

## Tool-Aufrufablauf

```
┌──────────┐
│   AI     │ Gibt tool_calls zurück
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ Tool finden und Nutzung berechtigen
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ Berechtigungskette prüfen
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ Ressourcenzugriffsoperation ausführen
└────┬─────────┘
     ↓
┌──────────┐
│   AI     │ Tool-Ergebnis empfangen, weiter denken
└──────────┘
```

## Berechtigungsvalidierung

Alle Tool-Ausführungen durchlaufen eine 5-stufige Berechtigungskette:

1. **IsCurator** — Silicon Curator umgeht alle Prüfungen
2. **UserFrequencyCache** — Benutzer-Häufigkeit-Cache für Allow/Deny
3. **GlobalACL** — Globale Access Control List
4. **IPermissionCallback** — Benutzerdefinierte Berechtigungs-Callback-Funktion
5. **IPermissionAskHandler** — Benutzer fragen

## Benutzerdefinierte Tools erstellen

### Schritt 1: ITool-Schnittstelle implementieren

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "工具描述";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, object>
        {
            ["param1"] = new { type = "string", description = "参数说明" }
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

Platzieren Sie die Tool-Datei im Verzeichnis `src/SiliconLife.Default/Tools/`. Der `ToolManager` wird es beim Start automatisch durch Reflektion entdecken und registrieren.

### Schritt 3: (Optional) Als Curator-exklusiv markieren

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // Nur für Silicon Curator zugänglich
}
```

## Best Practices

### 1. Immer Parameter validieren

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("缺少必需参数: required_param");
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
    Logger.Error($"工具 {Name} 执行失败: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. Berechtigungssystem respektieren

Umgehen Sie niemals die Berechtigungsprüfung. Greifen Sie immer über den Executor auf Ressourcen zu:

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return ToolResult.Denied(permission.Reason);
}
```

### 4. Klare Tool-Beschreibungen bereitstellen

Helfen Sie der KI zu verstehen, wann und wie das Tool verwendet wird:

```csharp
public string Description => 
    "用于在不同日历系统之间转换日期。" +
    "需要提供 'date'、'from_calendar' 和 'to_calendar' 参数。";
```

## Fehlerbehebung

### Tool nicht gefunden

**Problem**: KI versucht, ein nicht existierendes Tool aufzurufen.

**Lösung**:
- Prüfen Sie, ob der Tool-Name exakt übereinstimmt
- Validieren Sie, dass die Tool-Datei im `Tools/`-Verzeichnis ist
- Projekt neu bauen (`dotnet build`)

### Berechtigung verweigert

**Problem**: Tool-Ausführung fehlschlägt mit Berechtigungsfehler.

**Lösung**:
- Berechtigungs-Audit-Protokoll prüfen
- Validieren Sie, dass das Being die erforderlichen Berechtigungen hat
- Globale ACL-Einstellungen prüfen
- Wenn Curator, prüfen Sie ob `[SiliconManagerOnly]`-Attribut verwendet wird

### Tool-Ausführung gibt Fehler zurück

**Problem**: Tool wird ausgeführt, aber gibt Fehlerergebnis zurück.

**Lösung**:
- Fehlermeldung des Tools prüfen
- Validieren Sie, dass Eingabeparameter korrekt formatiert sind
- Systemprotokolle für detaillierte Fehlerinformationen prüfen
- Tool-Funktionalität unabhängig testen

## Nächste Schritte

- 📚 [Architekturleitfaden](architecture.md) lesen
- 🛠️ [Entwicklungsleitfaden](development-guide.md) prüfen
- 🔒 [Berechtigungssystem](permission-system.md) verstehen
- 🚀 [Schnellstart-Leitfaden](getting-started.md) ansehen
