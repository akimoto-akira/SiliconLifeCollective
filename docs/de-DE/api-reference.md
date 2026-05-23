# API-Referenz

> **Version: v0.2.0-alpha**

[English](../en/api-reference.md) | **Deutsch** | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md)

## Web-API-Endpunkte

Basis-URL: `http://localhost:8080`

### Authentifizierung

Die meisten Endpunkte erfordern Authentifizierung über Sitzungs-Cookies, die von der Web-UI verwaltet werden.

---

## Silicon-Being-Verwaltung

### Alle Beings abrufen

**GET** `/api/beings`

**Antwort**:
```json
{
  "beings": [
    {
      "id": "being-uuid",
      "name": "Assistant",
      "activity": "Idle",
      "soul": "path/to/soul.md"
    }
  ]
}
```

**Aktivitätswerte**: `Idle` | `SingleChat` | `GroupChat` | `Task` | `Timer` | `Broadcast` | `Project` | `MemoryCompression` | `Stopped`

### Being erstellen

**POST** `/api/beings`

**Anfrage**:
```json
{
  "name": "New Being",
  "soul": "# Personality\nYou are helpful..."
}
```

**Antwort**: `201 Created`

### Being starten

**POST** `/api/beings/{id}/start`

### Being stoppen

**POST** `/api/beings/{id}/stop`

### Being-Details abrufen

**GET** `/api/beings/{id}`

---

## Chat-System

### Nachricht senden

**POST** `/api/chat/send`

**Anfrage**:
```json
{
  "channelId": "session-uuid",
  "content": "Hello, how are you?"
}
```

**Antwort**:
```json
{
  "success": true,
  "messageId": "message-uuid"
}
```

### Gestreamter Chat (SSE)

**GET** `/api/chat/stream`

Server-Sent Events-Stream für Echtzeit-Chat-Updates.

### Sitzungsliste abrufen

**GET** `/api/chat/conversations`

**Antwort**:
```json
{
  "conversations": [
    {
      "sessionId": "session-uuid",
      "beingId": "being-uuid",
      "type": "single",
      "displayName": "Chat mit Being",
      "lastMessage": "Hallo!",
      "lastTime": "2026-04-20T10:30:00Z"
    }
  ]
}
```

### Nachrichtenverlauf abrufen

**GET** `/api/chat/messages?channelId={sessionId}`

**Antwort**:
```json
{
  "messages": [
    {
      "id": "message-uuid",
      "senderId": "sender-uuid",
      "channelId": "session-uuid",
      "content": "Hallo",
      "timestamp": "2026-04-20T10:30:00Z",
      "role": "user"
    }
  ]
}
```

### Chat-Verlauf abrufen

**GET** `/api/chat/history`

Gibt Chat-Verlaufssitzungen zurück.

### AI-Denken stoppen

**POST** `/api/chat/stop`

Stoppt die aktuelle KI-Streaming-Antwort.

### Datei hochladen

**POST** `/api/chat/upload`

Lädt eine Datei in die Chat-Sitzung hoch.

---

## Konfiguration

### Konfiguration abrufen

**GET** `/api/config`

**Antwort**:
```json
{
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:7b"
    }
  },
  "storage": {
    "basePath": "./data"
  }
}
```

### Konfiguration aktualisieren

**POST** `/api/config`

**Anfrage**:
```json
{
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:14b"
    }
  }
}
```

---

## Berechtigungssystem

### Berechtigungen abrufen

**GET** `/api/permissions`

**Antwort**:
```json
{
  "rules": [
    {
      "userId": "user-uuid",
      "resource": "disk:read",
      "allowed": true,
      "expiresAt": "2026-04-21T00:00:00Z"
    }
  ]
}
```

### Berechtigung erteilen

**POST** `/api/permissions`

**Anfrage**:
```json
{
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "duration": 3600
}
```

### Berechtigung widerrufen

**DELETE** `/api/permissions/{id}`

### Berechtigung prüfen

**POST** `/api/permissions/check`

**Anfrage**:
```json
{
  "userId": "user-uuid",
  "resource": "network:http"
}
```

**Antwort**:
```json
{
  "allowed": true,
  "reason": "Granted by curator"
}
```

---

## Aufgaben- und Timer-System

### Aufgabe erstellen

**POST** `/api/tasks`

**Anfrage**:
```json
{
  "beingId": "being-uuid",
  "description": "Review code",
  "priority": 5,
  "dueDate": "2026-04-21T12:00:00Z"
}
```

### Aufgaben abrufen

**GET** `/api/tasks?beingId={id}&status=pending`

### Aufgabenstatus aktualisieren

**PATCH** `/api/tasks/{id}`

**Anfrage**:
```json
{
  "status": "completed"
}
```

### Timer erstellen

**POST** `/api/timers`

**Anfrage**:
```json
{
  "beingId": "being-uuid",
  "interval": 3600,
  "action": "think",
  "repeat": true
}
```

### Timer löschen

**DELETE** `/api/timers/{id}`

---

## Audit und Protokollierung

### Token-Nutzung abrufen

**GET** `/api/audit/tokens?startDate={date}&endDate={date}`

**Antwort**:
```json
{
  "summary": {
    "totalTokens": 150000,
    "promptTokens": 100000,
    "completionTokens": 50000,
    "totalCost": 0.15
  },
  "byModel": {
    "qwen2.5:7b": {
      "tokens": 100000,
      "cost": 0.10
    }
  }
}
```

### Protokolle abrufen

**GET** `/api/logs?level=error&limit=100`

**Antwort**:
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

---

## Storage-API

### Wert lesen

**GET** `/api/storage?key={key}`

**Antwort**:
```json
{
  "key": "being:uuid:memory",
  "value": "{...}",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### Wert schreiben

**POST** `/api/storage`

**Anfrage**:
```json
{
  "key": "being:uuid:memory",
  "value": "{...}"
}
```

### Nach Zeitbereich abfragen

**GET** `/api/storage/time?start={start}&end={end}&prefix={prefix}`

**Antwort**:
```json
{
  "entries": [
    {
      "key": "being:uuid:chat:2026-04-20",
      "value": "{...}",
      "timestamp": "2026-04-20T10:30:00Z"
    }
  ]
}
```

---

## Speicher-API

### Speicherliste abrufen

**GET** `/api/memory/list`

**Abfrageparameter**: `beingId`, `type`, `limit`

### Speicherdetails abrufen

**GET** `/api/memory/detail/{id}`

### Speicherstatistiken abrufen

**GET** `/api/memory/stats`

**Abfrageparameter**: `beingId`

### Speicher durchsuchen

**GET** `/api/memory/search`

**Abfrageparameter**: `beingId`, `keyword`, `limit`

### Speicher-Beings abrufen

**GET** `/api/memory/beings`

Gibt Liste der Beings mit Speicherdaten zurück.

### Speicher-Ursprung zurückverfolgen

**GET** `/api/memory/trace/{id}`

Verfolgt die ursprüngliche Quelle eines Speichereintrags.

### Speicher-Zeitachse HTML abrufen

**GET** `/api/memory/timeline-html`

**Abfrageparameter**: `beingId`

Gibt HTML-Fragment für Speicher-Zeitachsenvisualisierung zurück.

---

## Code-Browser-API

### Code-Typen abrufen

**GET** `/api/code/types`

Gibt alle verfügbaren Typen für Code-Browsing zurück.

### Code-Details abrufen

**GET** `/api/code/detail`

**Abfrageparameter**: `type`, `member`

Gibt detaillierte Informationen über einen bestimmten Typ oder Member zurück.

---

## Executor-API

### Executor-Status abrufen

**GET** `/api/executors/status`

**Antwort**:
```json
[
  { "name": "DiskExecutor", "status": "Idle", "queueCount": 0 },
  { "name": "NetworkExecutor", "status": "Idle", "queueCount": 0 },
  { "name": "CommandLineExecutor", "status": "Idle", "queueCount": 0 }
]
```

---

## Systeminformationen

### Über-Seite abrufen

**GET** `/about`

Gibt die Über-Seite mit Systeminformationen und Liste der geladenen Plugins zurück.

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

### Berechtigungsanfrage

**GET** `/permission/request?userId={id}&type={type}&resource={resource}`

Zeigt die Berechtigungsanfrageseite an, die es Benutzern ermöglicht, Berechtigungsanfragen von Silicon Beings zu genehmigen oder abzulehnen.

**Abfrageparameter**:

| Parameter | Typ | Beschreibung |
|------|------|------|
| `userId` | `Guid` | ID des Silicon Beings, das Berechtigung anfragt |
| `type` | `string` | Berechtigungstyp |
| `resource` | `string` | Angeforderter Ressourcenpfad |
| `allowCode` | `string` | Code-ID für Erlaubnisoperation |
| `denyCode` | `string` | Code-ID für Verweigerungsoperation |

**Ausstehende Berechtigungsanfragen prüfen**:

**GET** `/permission/check?userId={id}`

**Antwort**:
```json
{
  "pending": true
}
```

**Auf Berechtigungsanfrage antworten**:

**GET** `/permission/respond?userId={id}&allowed={bool}&addToCache={bool}&cacheDuration={hours}`

**Abfrageparameter**:

| Parameter | Typ | Beschreibung |
|------|------|------|
| `userId` | `Guid` | Silicon Being-ID |
| `allowed` | `bool` | Ob erlaubt |
| `addToCache` | `bool` | Ob Entscheidung zwischengespeichert werden soll |
| `cacheDuration` | `double` | Cache-Dauer (Stunden) |

**Antwort**:
```json
{
  "success": true
}
```

### Dashboard-Daten abrufen

**GET** `/api/dashboard`

**Antwort**:
```json
{
  "beings": {
    "total": 5,
    "running": 3,
    "stopped": 2
  },
  "performance": {
    "cpu": 45.2,
    "memory": 1024,
    "uptime": 86400
  },
  "aiUsage": {
    "todayTokens": 50000,
    "todayCost": 0.05
  }
}
```

### Systemstatus abrufen

**GET** `/api/status`

**Antwort**:
```json
{
  "version": "1.0.0",
  "runtime": ".NET 9.0",
  "uptime": 86400,
  "health": "healthy"
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
    "details": "Required: disk:write, Current: disk:read"
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
const eventSource = new EventSource('/api/chat/stream?beingId=xxx&message=xxx');

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

### Being-Status-Ereignisse

```javascript
const beingEvents = new EventSource('/api/beings/events');

beingEvents.onmessage = (event) => {
  const data = JSON.parse(event.data);
  console.log(`Being ${data.beingId} status: ${data.status}`);
};
```

---

## KI-Client-API

### IAIClient-Schnittstelle

```csharp
public interface IAIClient
{
    string Endpoint { get; }
    string DefaultModel { get; }
    bool? StreamingMode { get; }
    bool? SupportsToolCalls { get; }
    
    AIResponse Chat(AIRequest request);
    Task<AIResponse> ChatAsync(AIRequest request);
    IAsyncEnumerable<AIResponse> ChatStreamAsync(AIRequest request, CancellationToken cancellationToken = default);
    
    AIResponse Chat(string userMessage);
    Task<AIResponse> ChatAsync(string userMessage);
    AIResponse Chat(string systemPrompt, string userMessage);
    Task<AIResponse> ChatAsync(string systemPrompt, string userMessage);
    
    AIResponse Generate(string prompt);
    Task<AIResponse> GenerateAsync(string prompt);
    AIResponse Generate(string systemPrompt, string prompt);
    Task<AIResponse> GenerateAsync(string systemPrompt, string prompt);
}
```

### AIRequest-Struktur

```csharp
public class AIRequest
{
    public string Model { get; set; } = string.Empty;
    public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    public List<ToolDefinition>? Tools { get; set; }
}
```

### AIResponse-Struktur

```csharp
public class AIResponse
{
    public string Model { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Thinking { get; set; }
    public List<ToolCall>? ToolCalls { get; set; }
    public int? PromptTokens { get; set; }
    public int? CompletionTokens { get; set; }
    public int? TotalTokens { get; set; }
    public bool Success { get; set; } = true;
    public string? ErrorMessage { get; set; }
    public bool IsStreamFinal { get; set; }
    public bool HasToolCalls => ToolCalls != null && ToolCalls.Count > 0;
}
```

---

## Arbeitsnotizen-API

### Arbeitsnotizen-Liste abrufen

**GET** `/api/beings/{id}/work-notes`

**Antwort**:
```json
{
  "notes": [
    {
      "id": "note-uuid",
      "pageNumber": 1,
      "summary": "Benutzerauthentifizierungsmodul abgeschlossen",
      "keywords": ["Authentifizierung", "JWT", "OAuth2"],
      "createdAt": "2026-04-25T10:00:00Z",
      "updatedAt": "2026-04-25T10:00:00Z"
    }
  ],
  "totalCount": 15
}
```

### Einzelne Notizdetails abrufen

**GET** `/api/beings/{id}/work-notes/{pageNumber}`

**Antwort**:
```json
{
  "id": "note-uuid",
  "pageNumber": 1,
  "summary": "Benutzerauthentifizierungsmodul abgeschlossen",
  "content": "## Implementierungsdetails\n\n- JWT-Token verwenden\n- OAuth2 unterstützen",
  "keywords": ["Authentifizierung", "JWT", "OAuth2"],
  "createdAt": "2026-04-25T10:00:00Z",
  "updatedAt": "2026-04-25T10:00:00Z"
}
```

### Neue Notiz erstellen

**POST** `/api/beings/{id}/work-notes`

**Anfrage**:
```json
{
  "summary": "Benutzerauthentifizierungsmodul abgeschlossen",
  "content": "## Implementierungsdetails\n\n- JWT-Token verwenden",
  "keywords": "Authentifizierung,JWT,OAuth2"
}
```

**Antwort**: `201 Created`

### Notiz aktualisieren

**PUT** `/api/beings/{id}/work-notes/{pageNumber}`

**Anfrage**:
```json
{
  "summary": "Benutzerauthentifizierungsmodul und Tests abgeschlossen",
  "content": "## Aktualisierter Inhalt\n\nUnit-Tests hinzugefügt",
  "keywords": "Authentifizierung,JWT,OAuth2,Tests"
}
```

### Notiz löschen

**DELETE** `/api/beings/{id}/work-notes/{pageNumber}`

### Notizen suchen

**GET** `/api/beings/{id}/work-notes/search?keyword=Authentifizierung&maxResults=10`

### Notizverzeichnis abrufen

**GET** `/api/beings/{id}/work-notes/directory`

---

## Wissensnetzwerk-API

### Wissensstatistiken abrufen

**GET** `/api/knowledge/stats`

**Antwort**:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Wissens-Tripel hinzufügen

**POST** `/api/knowledge/triples`

**Anfrage**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95,
  "tags": ["programming", "language"]
}
```

**Antwort**: `201 Created`

### Wissen abfragen

**GET** `/api/knowledge/query?subject=Python&predicate=is_a`

**Antwort**:
```json
{
  "triples": [
    {
      "subject": "Python",
      "predicate": "is_a",
      "object": "programming_language",
      "confidence": 0.95,
      "tags": ["programming", "language"]
    }
  ]
}
```

### Wissen suchen

**GET** `/api/knowledge/search?query=programming+language&limit=10`

### Wissenspfad abrufen

**GET** `/api/knowledge/path?from=Python&to=computer_science`

**Antwort**:
```json
{
  "path": [
    {"subject": "Python", "predicate": "is_a", "object": "programming_language"},
    {"subject": "programming_language", "predicate": "belongs_to", "object": "computer_science"}
  ],
  "length": 2
}
```

### Wissen validieren

**POST** `/api/knowledge/validate`

**Anfrage**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

### Wissen löschen

**DELETE** `/api/knowledge/triples/{id}`

---

## Hilfedokumentation-API

### Hilfedokumentationsliste abrufen

**GET** `/api/help`

**Antwort**:
```json
{
  "topics": [
    {
      "id": "getting-started",
      "title": "Schnellstart",
      "category": "Einstiegsleitfaden"
    }
  ]
}
```

### Hilfedokumentationsdetails abrufen

**GET** `/api/help/{topicId}`

**Antwort**:
```json
{
  "id": "getting-started",
  "title": "Schnellstart",
  "content": "# Schnellstart\n\n...",
  "category": "Einstiegsleitfaden"
}
```

---

## WebView-Browser-API

### Browserstatus abrufen

**GET** `/api/beings/{id}/browser/status`

**Antwort**:
```json
{
  "is_open": true,
  "current_url": "https://example.com",
  "page_title": "Example Page",
  "is_loading": false,
  "last_operation_time": "2026-04-26T10:00:00Z"
}
```

### Browser öffnen

**POST** `/api/beings/{id}/browser/open`

### Browser schließen

**POST** `/api/beings/{id}/browser/close`

### Zu URL navigieren

**POST** `/api/beings/{id}/browser/navigate`

**Anfrage**:
```json
{
  "url": "https://example.com"
}
```

### JavaScript ausführen

**POST** `/api/beings/{id}/browser/execute-script`

**Anfrage**:
```json
{
  "script": "return document.title;"
}
```

### Seiten-Screenshot abrufen

**GET** `/api/beings/{id}/browser/screenshot`

---

## Projekt-Workspace-API

### Projektliste abrufen

**GET** `/api/projects`

**Antwort**:
```json
{
  "projects": [
    {
      "id": "project-uuid",
      "name": "My Project",
      "description": "Project description",
      "createdAt": "2026-04-25T10:00:00Z"
    }
  ]
}
```

### Projekt erstellen

**POST** `/api/projects`

**Anfrage**:
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### Projekt-Details abrufen

**GET** `/api/projects/{id}`

### Projekt aktualisieren

**PUT** `/api/projects/{id}`

### Projekt löschen

**DELETE** `/api/projects/{id}`

---

## Tool-System-API

### ITool-Schnittstelle

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    string GetDisplayName(Language language);
    Dictionary<string, object> GetParameterSchema();
    
    ToolResult Execute(Guid callerId, Dictionary<string, object> parameters);
}
```

### ToolCall-Struktur

```csharp
public class ToolCall
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public Dictionary<string, object> Arguments { get; set; } = new();
}
```

### ToolResult-Struktur

```csharp
public class ToolResult
{
    public bool Success { get; }
    public string Message { get; }
    public object? Data { get; }
}
```

---

## Nächste Schritte

- 🚀 [Schnellstart-Leitfaden](getting-started.md) ansehen
- 🛠️ [Entwicklungsleitfaden](development-guide.md) lesen
- 📚 [Architekturdokumentation](architecture.md) prüfen
- 🔒 [Sicherheitsmodell](security.md) verstehen
