# API-Referenz

> **Version: v0.2.0-alpha**

[English](../en/api-reference.md) | **Deutsch** | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | [Русский](../ru-RU/api-reference.md)

## Web-API-Endpunkte

Basis-URL: `http://localhost:8080`

### Authentifizierung

Die meisten Endpunkte erfordern eine Authentifizierung über Sitzungs-Cookies, die von der Web-UI verwaltet werden. Vor der Systeminitialisierung werden alle Anfragen außer der Hilfeseite zur Initialisierungsseite weitergeleitet.

---

## Dashboard

### Dashboard-Statistiken abrufen

**GET** `/api/dashboard/stats`

Gibt Systemübersichtsdaten zurück (Anzahl der Silicon Beings, Laufzeitstatus usw.).

### Leistungsmetriken abrufen

**GET** `/api/dashboard/metrics`

Gibt Echtzeit-Leistungsmetrikdaten zurück.

---

## Chat-System

### Chat-Seite

**GET** `/chat`

Gibt die Chat-Interface-Seite zurück.

### Gestreamter Chat (SSE)

**GET** `/api/chat/stream`

Streaming-Chat über Server-Sent Events (SSE).

**Antwort**: Server-Sent Events-Stream

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Sitzungsliste abrufen

**GET** `/api/chat/conversations`

Gibt eine Liste aller aktiven Chat-Sitzungen zurück.

**Antwortbeispiel**:
```json
{
  "conversations": [
    {
      "sessionId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
      "beingId": "being-uuid",
      "type": "single",
      "displayName": "Chat mit Xiaoyou",
      "lastMessage": "Inhalt der letzten Nachricht",
      "lastTime": "2026-05-20T10:30:00Z"
    }
  ]
}
```

### Nachrichtenverlauf abrufen

**GET** `/api/chat/messages`

Abfrageparameter: `channelId` — Kanal-/Sitzungs-ID

Gibt den Nachrichtenverlauf der angegebenen Sitzung zurück.

### Chat-Verlauf abrufen

**GET** `/api/chat/history`

Gibt den globalen Chat-Verlauf zurück.

### Nachricht senden

**POST** `/api/chat/send`

**Anfragetext**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
  "content": "Testnachrichtinhalt"
}
```

**Antwort**:
```json
{
  "success": true,
  "messageId": "50156b26-f3b9-4735-be3d-51e547bd3a4a"
}
```

### KI-Denken stoppen

**POST** `/api/chat/stop`

Stoppt die aktuell laufende KI-Antwortgenerierung.

**Anfragetext**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d"
}
```

### Datei hochladen

**POST** `/api/chat/upload`

Lädt eine Datei in die Chat-Sitzung hoch (unterstützt multipart/form-data).

---

## Silicon-Being-Verwaltung

### Being-Verwaltungsseite

**GET** `/beings`

Gibt die Silicon-Being-Verwaltungsoberfläche zurück.

### Being-Liste abrufen

**GET** `/api/beings` oder **GET** `/api/beings/list`

Gibt eine Liste aller registrierten Silicon Beings zurück.

**Antwortbeispiel**:
```json
{
  "beings": [
    {
      "id": "being-uuid",
      "name": "Assistant",
      "status": "running",
      "soulPath": "path/to/soul.md"
    }
  ]
}
```

**Statuswerte**: `idle` | `running` | `waiting_permission` | `stopped`

### Being-Details abrufen

**GET** `/api/beings/detail`

Abfrageparameter: `beingId` — Being-ID

Gibt detaillierte Informationen zum angegebenen Silicon Being zurück.

### Being-Aktivitätsstatus abrufen

**GET** `/api/beings/activity`

Gibt die Aktivitätsstatusinformationen der einzelnen Silicon Beings zurück.

### Soul-Datei-Editor-Seite

**GET** `/beings/soul`

Gibt die Soul-Datei-Editor-Oberfläche zurück.

### Soul-Datei speichern

**POST** `/api/beings/soul/save`

**Anfragetext**:
```json
{
  "beingId": "being-uuid",
  "soulContent": "# Personality\nYou are helpful..."
}
```

### KI-Konfigurations-Editor-Seite

**GET** `/beings/ai-config`

Gibt die KI-Konfigurations-Editor-Oberfläche zurück.

### KI-Konfiguration speichern

**POST** `/api/beings/ai-config/save`

**Anfragetext**:
```json
{
  "beingId": "being-uuid",
  "aiClientType": "DashScope",
  "config": {
    "apiKey": "...",
    "region": "beijing",
    "model": "qwen3.6-plus"
  }
}
```

### Verfügbare KI-Modellliste abrufen

**GET** `/api/beings/ai-config/models`

Abfrageparameter: `clientType`, `apiKey`, `region`

Gibt die Liste der verfügbaren Modelle für den angegebenen KI-Client zurück.

---

## Chat-Verlaufsansicht

### Chat-Verlaufsseite

**GET** `/chat-history`

Gibt die Chat-Verlaufshauptseite zurück.

### Chat-Verlaufsdetailseite

**GET** `/chat-history-detail`

Gibt die Chat-Verlaufsdetailseite für die angegebene Sitzung zurück.

### Gruppenchat-Verlaufsdetailseite

**GET** `/group-chat-history-detail`

Gibt die Verlaufsdetailseite für Gruppenchats zurück.

### Broadcast-Verlaufsdetailseite

**GET** `/broadcast-history-detail`

Gibt die Verlaufsdetailseite für den Broadcast-Kanal zurück.

### Verlaufssitzungsliste abrufen

**GET** `/api/chat-history/conversations`

Gibt eine Liste aller Verlaufssitzungen zurück.

### Verlaufsnachrichten abrufen

**GET** `/api/chat-history/messages`

Abfrageparameter: `sessionId` — Sitzungs-ID

Gibt die Nachrichtenaufzeichnungen der angegebenen Verlaufssitzung zurück.

---

## Timer-Verwaltung

### Timer-Seite

**GET** `/timers`

Gibt die Timer-Verwaltungsoberfläche zurück.

### Timer-Liste abrufen

**GET** `/api/timers/list`

Gibt eine Liste aller Timer zurück.

### Timer-Zyklusdetailseite

**GET** `/timer-cycles/{timerId}`

Gibt die Ausführungszyklusdetailseite für den angegebenen Timer zurück.

### Timer-Zyklusliste abrufen

**GET** `/api/timer-cycles/list`

Abfrageparameter: `timerId` — Timer-ID

Gibt eine Liste aller Ausführungszyklen des angegebenen Timers zurück.

### Einzelne Ausführungszyklusdetailseite

**GET** `/timer-cycle/{cycleIndex}`

Gibt die Detailseite einer einzelnen Ausführung zurück.

### Zyklusnachrichten abrufen

**GET** `/api/timer-cycle/messages`

Abfrageparameter: `cycleIndex` — Zyklusindex

Gibt die zugehörigen Nachrichten des angegebenen Ausführungszyklus zurück.

---

## Aufgabenverwaltung

### Aufgabenseite

**GET** `/tasks`

Gibt die Aufgabenverwaltungsoberfläche zurück.

### Aufgabenliste abrufen

**GET** `/api/tasks/list`

Gibt eine Liste aller Aufgaben zurück.

### Aufgabenzyklusdetailseite

**GET** `/task-cycles/{taskId}`

Gibt die Ausführungszyklusdetailseite für die angegebene Aufgabe zurück.

### Aufgabenzyklusliste abrufen

**GET** `/api/task-cycles/list`

Abfrageparameter: `taskId` — Aufgaben-ID

Gibt eine Liste aller Ausführungszyklen der angegebenen Aufgabe zurück.

### Einzelne Ausführungszyklusdetailseite

**GET** `/task-cycle/{cycleIndex}`

Gibt die Detailseite einer einzelnen Aufgabenausführung zurück.

### Zyklusnachrichten abrufen

**GET** `/api/task-cycle/messages`

Abfrageparameter: `cycleIndex` — Zyklusindex

Gibt die zugehörigen Nachrichten des angegebenen Aufgabenausführungszyklus zurück.

---

## Berechtigungssystem

### Berechtigungsverwaltungsseite

**GET** `/permissions`

Gibt die Berechtigungsverwaltungsoberfläche zurück.

### Berechtigungsregelliste abrufen

**GET** `/api/permissions/list`

Gibt alle aktuell konfigurierten Berechtigungsregeln zurück.

**Antwortbeispiel**:
```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed",
      "description": "Allow GitHub API access"
    }
  ]
}
```

### Berechtigungsregel speichern

**POST** `/api/permissions/save`

**Anfragetext**:
```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects",
  "result": "Allowed",
  "description": "Allow project directory access"
}
```

### Berechtigungsanfrageseite

**GET** `/permission/request`

Zeigt die Berechtigungsanfrageseite an, die es Benutzern ermöglicht, Berechtigungsanfragen von Silicon Beings zu genehmigen oder abzulehnen.

**Abfrageparameter**:

| Parameter | Typ | Beschreibung |
|------|------|------|
| `userId` | `Guid` | ID des Silicon Beings, das die Berechtigung anfragt |
| `type` | `string` | Berechtigungstyp |
| `resource` | `string` | Angeforderter Ressourcenpfad |
| `allowCode` | `string` | Code-ID für die Erlaubnisoperation |
| `denyCode` | `string` | Code-ID für die Verweigerungsoperation |

### Ausstehende Berechtigungsanfragen prüfen

**GET** `/permission/check`

Abfrageparameter: `userId` — Silicon-Being-ID

**Antwort**:
```json
{
  "pending": true
}
```

### Auf Berechtigungsanfrage antworten

**GET** `/permission/respond`

**Abfrageparameter**:

| Parameter | Typ | Beschreibung |
|------|------|------|
| `userId` | `Guid` | Silicon-Being-ID |
| `allowed` | `bool` | Ob erlaubt |
| `addToCache` | `bool` | Ob die Entscheidung zwischengespeichert werden soll |
| `cacheDuration` | `double` | Cache-Dauer (Stunden) |

**Antwort**:
```json
{
  "success": true
}
```

---

## Protokollierungssystem

### Protokollseite

**GET** `/logs`

Gibt die Protokollansichtsoberfläche zurück.

### Protokollliste abrufen

**GET** `/api/logs/list`

Abfrageparameter unterstützen die Filterung nach Protokollstufe und Zeitbereich.

**Antwortbeispiel**:
```json
{
  "logs": [
    {
      "timestamp": "2026-04-20T10:30:00Z",
      "level": "error",
      "message": "Failed to connect to AI service",
      "source": "OllamaClient"
    }
  ]
}
```

### Protokolle nach Being gruppiert abrufen

**GET** `/api/logs/beings`

Protokollstatistiken gruppiert nach Silicon Being.

### Verfügbare Protokollstufen abrufen

**GET** `/api/logs/levels`

Gibt die Liste der im System verfügbaren Protokollstufen zurück.

---

## Nutzungsstatistiken

### Nutzungsstatistikseite

**GET** `/usage`

Gibt die Nutzungsstatistikoberfläche zurück.

### Nutzungszusammenfassung abrufen

**GET** `/api/usage/summary`

Gibt eine Zusammenfassung der Token-Nutzung und Kosten zurück.

### Trenddaten abrufen

**GET** `/api/usage/trend`

Abfrageparameter: `startDate`, `endDate`

Gibt die Nutzungstrenddaten für den angegebenen Zeitraum zurück.

### Nutzungsdaten exportieren

**GET** `/api/usage/export`

Exportiert Nutzungsdaten in ein herunterladbares Format.

---

## Audit-Trail

### Audit-Seite

**GET** `/audit`

Gibt die Audit-Trail-Oberfläche zurück.

### Audit-Liste abrufen

**GET** `/api/audit/list`

Gibt eine Liste der Audit-Protokolleinträge zurück.

### Audit-Zusammenfassung abrufen

**GET** `/api/audit/summary`

Gibt zusammengefasste Statistiken der Audit-Daten zurück.

### Audits nach Being gruppiert abrufen

**GET** `/api/audit/beings`

Audit-Statistiken gruppiert nach Silicon Being.

---

## Konfigurationsverwaltung

### Konfigurationsseite

**GET** `/config`

Gibt die Systemkonfigurationsoberfläche zurück.

### Konfiguration speichern

**POST** `/config/save`

**Anfragetext**:
```json
{
  "language": "ZhCN",
  "port": 8080,
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:7b"
    },
    "DashScope": {
      "apiKey": "...",
      "region": "beijing",
      "model": "qwen3.6-plus"
    },
    "VolcengineArk": {
      "apiKey": "...",
      "endpoint": "...",
      "model": "..."
    },
    "Herdsman": {
      "endpoint": "http://localhost:8000",
      "model": "..."
    },
    "LongCat": {
      "apiKey": "...",
      "endpoint": "...",
      "model": "..."
    },
    "QiniuAI": {
      "apiKey": "...",
      "endpoint": "...",
      "model": "..."
    },
    "DeepSeek": {
      "apiKey": "...",
      "endpoint": "https://api.deepseek.com",
      "model": "deepseek-v4-flash"
    },
    "Zhipu": {
      "apiKey": "...",
      "endpoint": "https://open.bigmodel.cn/api/paas/v4",
      "model": "glm-4-flash"
    },
    "Ernie": {
      "apiKey": "...",
      "endpoint": "https://qianfan.baidubce.com/v2",
      "model": "ernie-5.1"
    },
    "Hunyuan": {
      "apiKey": "...",
      "endpoint": "https://tokenhub.tencentmaas.com/v1",
      "model": "hy3"
    },
    "MiniMax": {
      "apiKey": "...",
      "endpoint": "https://api.minimaxi.com/v1",
      "model": "MiniMax-M3"
    },
    "Moonshot": {
      "apiKey": "...",
      "endpoint": "https://api.moonshot.cn/v1",
      "model": "kimi-k2.6"
    },
    "SiliconFlow": {
      "apiKey": "...",
      "endpoint": "https://api.siliconflow.cn/v1",
      "model": "deepseek-ai/DeepSeek-V3.2"
    }
  }
}
```

### KI-Konfigurationsoptionen abrufen

**GET** `/config/aioptions`

Gibt verfügbare KI-Client-Typen und deren dynamische Optionen (verfügbare Modelle, Regionen usw.) zurück.

---

## Speichersystem

### Speicherseite

**GET** `/memory`

Gibt die Speicherverwaltungsoberfläche zurück.

### Speicherliste abrufen

**GET** `/api/memory/list`

Gibt eine Liste der Speichereinträge der Silicon Beings zurück.

### Speicherdetails abrufen

**GET** `/api/memory/detail/{id}`

Pfadparameter: `id` — Speichereintrags-ID

Gibt den vollständigen Inhalt des angegebenen Speichereintrags zurück.

### Speicherstatistiken abrufen

**GET** `/api/memory/stats`

Gibt statistische Informationen des Speichersystems zurück.

### Speicher durchsuchen

**GET** `/api/memory/search`

Abfrageparameter: `keyword` — Suchschlüsselwort

Durchsucht übereinstimmende Speichereinträge.

### Speicher nach Being gruppiert abrufen

**GET** `/api/memory/beings`

Speicherstatistiken gruppiert nach Silicon Being.

### Speicherursprung zurückverfolgen

**GET** `/api/memory/trace/{id}`

Pfadparameter: `id` — Speichereintrags-ID

Gibt die Ursprungs-Rückverfolgungskette des angegebenen Speichereintrags zurück.

### Speicher-Zeitachse HTML abrufen

**GET** `/api/memory/timeline-html`

Gibt die HTML-Ansicht der Speicher-Zeitachse zurück.

---

## Arbeitsnotizen

### Arbeitsnotizenseite

**GET** `/work-notes`

Gibt die Arbeitsnotizenoberfläche zurück.

### Arbeitsnotizenliste abrufen

**GET** `/api/work-notes/list`

Gibt die Liste der Arbeitsnotizen zurück.

### Arbeitsnotiz lesen

**GET** `/api/work-notes/read`

Abfrageparameter: `noteId` — Notiz-ID

Gibt den Inhalt der angegebenen Notiz zurück.

### Notizverzeichnis abrufen

**GET** `/api/work-notes/directory`

Gibt die Notizverzeichnisstruktur zurück.

### Arbeitsnotizen durchsuchen

**GET** `/api/work-notes/search`

Abfrageparameter: `keyword` — Suchschlüsselwort

Durchsucht übereinstimmende Arbeitsnotizen.

### Arbeitsnotiz erstellen

**POST** `/api/work-notes/create`

**Anfragetext**:
```json
{
  "title": "Notiztitel",
  "content": "Notizinhalt",
  "keywords": ["Schlüsselwort1", "Schlüsselwort2"]
}
```

### Arbeitsnotiz aktualisieren

**POST** `/api/work-notes/update`

**Anfragetext**:
```json
{
  "noteId": "note-uuid",
  "title": "Aktualisierter Titel",
  "content": "Aktualisierter Inhalt"
}
```

### Arbeitsnotiz löschen

**POST** `/api/work-notes/delete`

**Anfragetext**:
```json
{
  "noteId": "note-uuid"
}
```

---

## Wissensnetzwerk

### Wissensnetzwerkseite

**GET** `/knowledge`

Gibt die Wissensnetzwerk-Verwaltungsoberfläche zurück.

### Wissensgraph abrufen

**GET** `/api/knowledge/graph`

Gibt Wissens-Tripel-Graphdaten zurück (Subjekt-Relation-Objekt).

---

## Projektverwaltung

### Projektseite

**GET** `/project`

Gibt die Projektverwaltungsoberfläche zurück.

### Projekt-Arbeitsnotizenseite

**GET** `/project/{id}/work-notes`

Pfadparameter: `id` — Projekt-ID

Gibt die Arbeitsnotizenseite des angegebenen Projekts zurück.

### Projekt-Aufgabenseite

**GET** `/project/{id}/tasks`

Pfadparameter: `id` — Projekt-ID

Gibt die Aufgabenverwaltungsseite des angegebenen Projekts zurück.

### Projekt-Werkzeugberechtigungsseite

**GET** `/project/{id}/tool-permissions`

Pfadparameter: `id` — Projekt-ID

Gibt die Werkzeugberechtigungsverwaltungsseite des angegebenen Projekts zurück.

### Projekt-Workflow-Seite

**GET** `/project/{id}/workflow`

Pfadparameter: `id` — Projekt-ID

Gibt die Workflow-Verwaltungsseite des angegebenen Projekts zurück.

### Projekt-Workflow-Details abrufen

**GET** `/api/projects/workflow-detail`

Abfrageparameter: `projectId` — Projekt-ID

Gibt die Workflow-Details des verknüpften Projekts zurück.

### Projektrolle zuweisen

**POST** `/api/projects/assign-role`

**Anfragetext**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### Projektrolle entfernen

**POST** `/api/projects/remove-role`

**Anfragetext**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### Projektliste abrufen

**GET** `/api/projects/list`

Gibt eine Liste aller Projekte zurück.

### Projekt-Workflow-Vorlagenliste abrufen

**GET** `/api/projects/list-workflow-templates`

Gibt eine Liste der verfügbaren Workflow-Vorlagen zurück.

### Projekt erstellen

**POST** `/api/projects/create`

**Anfragetext**:
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### Projekt archivieren

**POST** `/api/projects/{id}/archive`

Pfadparameter: `id` — Projekt-ID

Archiviert das angegebene Projekt.

### Projekt wiederherstellen

**POST** `/api/projects/{id}/restore`

Pfadparameter: `id` — Projekt-ID

Stellt ein archiviertes Projekt wieder her.

### Projekt zerstören

**POST** `/api/projects/{id}/destroy`

Pfadparameter: `id` — Projekt-ID

Löscht das angegebene Projekt dauerhaft (unwiderruflich).

### Projektdetails abrufen

**GET** `/api/projects/detail`

Abfrageparameter: `projectId` — Projekt-ID

Gibt detaillierte Informationen zum Projekt zurück.

### Projekt aktualisieren

**POST** `/api/projects/update`

**Anfragetext**:
```json
{
  "projectId": "project-uuid",
  "name": "Updated Name",
  "description": "Updated description"
}
```

### Mitglied einem Projekt zuweisen

**POST** `/api/projects/assign`

**Anfragetext**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Mitglied aus einem Projekt entfernen

**POST** `/api/projects/remove`

**Anfragetext**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Projekt-Arbeitsnotizenliste abrufen

**GET** `/api/projects/{id}/work-notes/list`

Pfadparameter: `id` — Projekt-ID

Gibt die Arbeitsnotizenliste des angegebenen Projekts zurück.

### Projekt-Arbeitsnotiz lesen

**GET** `/api/projects/{id}/work-notes/read`

Pfadparameter: `id` — Projekt-ID

Gibt den Inhalt der Arbeitsnotiz des angegebenen Projekts zurück.

### Projekt-Arbeitsnotiz erstellen

**POST** `/api/projects/{id}/work-notes/create`

Pfadparameter: `id` — Projekt-ID

Erstellt eine neue Arbeitsnotiz im angegebenen Projekt.

### Projekt-Arbeitsnotiz aktualisieren

**POST** `/api/projects/{id}/work-notes/update`

Pfadparameter: `id` — Projekt-ID

Aktualisiert die Arbeitsnotiz im angegebenen Projekt.

### Projekt-Arbeitsnotiz löschen

**POST** `/api/projects/{id}/work-notes/delete`

Pfadparameter: `id` — Projekt-ID

Löscht die Arbeitsnotiz im angegebenen Projekt.

### Projektaufgabenliste abrufen

**GET** `/api/projects/{id}/tasks/list`

Pfadparameter: `id` — Projekt-ID

Gibt die Aufgabenliste des angegebenen Projekts zurück.

### Projektaufgabe erstellen

**POST** `/api/projects/{id}/tasks/create`

Pfadparameter: `id` — Projekt-ID

Erstellt eine neue Aufgabe im angegebenen Projekt.

### Projektaufgabe aktualisieren

**POST** `/api/projects/{id}/tasks/update`

Pfadparameter: `id` — Projekt-ID

Aktualisiert die Aufgabe im angegebenen Projekt.

### Projektaufgabe löschen

**POST** `/api/projects/{id}/tasks/delete`

Pfadparameter: `id` — Projekt-ID

Löscht die Aufgabe im angegebenen Projekt.

### Aufgabenverantwortlichen zuweisen

**POST** `/api/projects/{id}/tasks/assign`

Pfadparameter: `id` — Projekt-ID

Weist einem Projektaufgabe einen Verantwortlichen zu.

### Aufgabenverantwortlichen entfernen

**POST** `/api/projects/{id}/tasks/remove-assignee`

Pfadparameter: `id` — Projekt-ID

Entfernt den Verantwortlichen von der Projektaufgabe.

### Aufgabe als abgeschlossen markieren

**POST** `/api/projects/{id}/tasks/complete`

Pfadparameter: `id` — Projekt-ID

Markiert die Projektaufgabe als abgeschlossen.

### Aufgabe als fehlgeschlagen markieren

**POST** `/api/projects/{id}/tasks/fail`

Pfadparameter: `id` — Projekt-ID

Markiert die Projektaufgabe als fehlgeschlagen.

### Aufgabe abbrechen

**POST** `/api/projects/{id}/tasks/cancel`

Pfadparameter: `id` — Projekt-ID

Bricht die Projektaufgabe ab.

---

## Werkzeugberechtigungsverwaltung

### Werkzeugberechtigungen eines Silicon Beings abrufen

**GET** `/api/beings/tool-permissions`

Abfrageparameter: `beingId` — Silicon-Being-ID

Gibt die Werkzeugberechtigungskonfiguration des angegebenen Silicon Beings zurück.

### Werkzeugberechtigungen eines Silicon Beings aktualisieren

**PUT** `/api/beings/tool-permissions`

**Anfragetext**:
```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network": "allowed",
    "disk_read": "allowed",
    "disk_write": "denied"
  }
}
```

### Werkzeugberechtigungsvorlagen abrufen

**GET** `/api/beings/tool-permissions/templates`

Gibt eine Liste der verfügbaren Werkzeugberechtigungsvorlagen zurück.

### Werkzeugberechtigungsvorlage anwenden

**POST** `/api/beings/tool-permissions/apply-template`

**Anfragetext**:
```json
{
  "beingId": "being-uuid",
  "templateName": "readonly"
}
```

### Projekt-Werkzeugberechtigungen abrufen

**GET** `/api/projects/{id}/tool-permissions`

Pfadparameter: `id` — Projekt-ID

Gibt die Werkzeugberechtigungskonfiguration des angegebenen Projekts zurück.

### Projekt-Werkzeugberechtigungen aktualisieren

**PUT** `/api/projects/{id}/tool-permissions`

Pfadparameter: `id` — Projekt-ID

**Anfragetext**:
```json
{
  "permissions": {
    "network": "allowed",
    "disk_read": "allowed",
    "disk_write": "denied"
  }
}
```

---

## Executor-Verwaltung

### Executor-Seite

**GET** `/executor`

Gibt die Executor-Verwaltungsoberfläche zurück.

### Executor-Status abrufen

**GET** `/api/executors/status`

Gibt den Laufzeitstatus der einzelnen Executors (Disk, Netzwerk, Kommandozeile) zurück.

---

## Code-Browser

### Code-Browser-Seite

**GET** `/code`

Gibt die Code-Browser-Oberfläche zurück.

### Code-Typenliste abrufen

**GET** `/api/code/types`

Gibt die Liste der unterstützten Code-Typen/Sprachen zurück.

### Code-Details abrufen

**GET** `/api/code/detail`

Abfrageparameter: `filePath`, `lineNumber`

Gibt die Code-Details der angegebenen Datei zurück.

---

## Code-Hover-Tooltips

### Hover-Tooltips abrufen

**GET** `/api/code/hover`
**POST** `/api/code/hover`

Ruft Hover-Tooltip-Informationen für eine Code-Position ab (ähnlich wie IDE-IntelliSense).

### Code-Position registrieren

**POST** `/api/code/register`

Registriert eine zu überwachende Code-Position.

### Code-Position aktualisieren

**POST** `/api/code/update`

Aktualisiert die Informationen einer registrierten Code-Position.

### Code-Position deregistrieren

**POST** `/api/code/unregister`

Deregistriert eine nicht mehr benötigte Code-Positionsüberwachung.

---

## Hilfedokumentationssystem

### Hilfeseite

**GET** `/help` oder **GET** `/help/index`

Gibt die Hilfedokumentations-Hauptseite zurück.

### Hilfethemenseite

**GET** `/help/{topic}`

Pfadparameter: `topic` — Themenbezeichner

Gibt die Hilfedokumentationsseite für das angegebene Thema zurück.

### Hilfedokumentation durchsuchen

**GET** `/api/help/search`

Abfrageparameter: `keyword` — Suchschlüsselwort

Durchsucht übereinstimmende Hilfedokumentationsthemen.

---

## Initialisierung

### Initialisierungsassistent-Seite

**GET** `/init`

Gibt die Initialisierungsassistent-Seite für den ersten Start zurück.

### Initialisierung absenden

**POST** `/init`

Sendet die Initialisierungskonfiguration für den ersten Start.

### Datenverzeichnis durchsuchen und auswählen

**GET** `/init/browse`

Öffnet den Verzeichnis-Browser zur Auswahl des Datenspeicherorts.

### KI-Konfigurationsmetadaten abrufen

**GET** `/init/ai-config-metadata`

Gibt verfügbare KI-Client-Typen und deren Konfigurationsfeld-Metadaten zurück.

---

## Systemsteuerung

### Ordnungsgemäßes Herunterfahren

**POST** `/api/system/shutdown`

> **Hinweis**: Nur Anfragen von localhost sind zulässig

Löst den ordnungsgemäßen Herunterfahrprozess der Anwendung aus:

1. Hauptschleife stoppen (MainLoop)
2. Aktuelle Konfiguration speichern
3. HTTP-Listener schließen

**Antwort**:
```json
{
  "status": "shutting_down",
  "message": "Application is shutting down gracefully"
}
```

---

## Über

### Über-Seite

**GET** `/about`

Gibt die Über-Seite mit Systeminformationen und der Liste der geladenen Plugins zurück.

**Plugin-Listendaten**:
```json
{
  "plugins": {
    "plugin-id": {
      "name": "My Plugin",
      "version": "1.0.0",
      "description": "Plugin description",
      "author": "Author Name"
    }
  }
}
```

---

## Fehlerantworten

Alle Endpunkte geben standardisierte Fehlerantworten zurück:

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: FileAccess, Denied by GlobalACL"
  }
}
```

### Häufige Fehlercodes

| Code | HTTP-Status | Beschreibung |
|------|-------------|--------------|
| `PERMISSION_DENIED` | 403 | Unzureichende Berechtigungen |
| `NOT_FOUND` | 404 | Ressource nicht gefunden |
| `VALIDATION_ERROR` | 400 | Ungültige Anfrageparameter |
| `INTERNAL_ERROR` | 500 | Interner Serverfehler |
| `SERVICE_UNAVAILABLE` | 503 | KI-Dienst nicht verfügbar |

---

## SSE-Ereignisse

Server-Sent Events für Echtzeit-Updates:

### Chat-Ereignisse

```javascript
const eventSource = new EventSource('/api/chat/stream');

eventSource.onmessage = (event) => {
  const data = JSON.parse(event.data);

  switch(data.type) {
    case 'chunk':
      console.log('Streaming:', data.content);
      break;
    case 'tool_call':
      console.log('Tool executing:', data.tool);
      break;
    case 'complete':
      console.log('Chat complete, session:', data.sessionId);
      break;
    case 'error':
      console.error('Error:', data.message);
      break;
  }
};
```

---

## KI-Client-Schnittstelle

### IAIClient-Schnittstelle

```csharp
public interface IAIClient
{
    string Name { get; }

    Task<AIResponse> ChatAsync(AIRequest request);

    IAsyncEnumerable<string> StreamChatAsync(AIRequest request);
}
```

### AIRequest-Struktur

```csharp
public class AIRequest
{
    public List<Message> Messages { get; set; }
    public List<ToolDefinition> Tools { get; set; }
    public double Temperature { get; set; } = 0.7;
    public int MaxTokens { get; set; } = 2000;
    public string Model { get; set; }
}
```

### AIResponse-Struktur

```csharp
public class AIResponse
{
    public string Content { get; set; }
    public List<ToolCall> ToolCalls { get; set; }
    public TokenUsage Usage { get; set; }
    public string Model { get; set; }
}
```

---

## Werkzeugsystem-Schnittstelle

### ITool-Schnittstelle

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    ToolDefinition Definition { get; }

    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### ToolCall-Struktur

```csharp
public class ToolCall
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}
```

### ToolResult-Struktur

```csharp
public class ToolResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }
}
```

---

## Nächste Schritte

- 🚀 [Schnellstart-Leitfaden](getting-started.md) ansehen
- 🛠️ [Entwicklungsleitfaden](development-guide.md) lesen
- 📚 [Architekturdokumentation](architecture.md) prüfen
- 🔒 [Sicherheitsmodell](security.md) verstehen
