# API-Referenz

> **Version: v0.1.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md)

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
      "status": "running",
      "soul": "path/to/soul.md"
    }
  ]
}
```

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

**POST** `/api/chat`

**Anfrage**:
```json
{
  "beingId": "being-uuid",
  "message": "Hello, how are you?",
  "sessionId": "optional-session-id"
}
```

**Antwort** (nicht gestreamt):
```json
{
  "reply": "I'm doing well, thank you!",
  "sessionId": "session-uuid",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### Gestreamter Chat (SSE)

**GET** `/api/chat/stream?beingId={id}&message={msg}`

**Antwort**: Server-Sent Events-Stream

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Chat-Verlauf abrufen

**GET** `/api/chat/{sessionId}/history`

**Antwort**:
```json
{
  "messages": [
    {
      "role": "user",
      "content": "Hello",
      "timestamp": "2026-04-20T10:30:00Z"
    },
    {
      "role": "assistant",
      "content": "Hi there!",
      "timestamp": "2026-04-20T10:30:05Z"
    }
  ]
}
```

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

## Systeminformationen

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
      "summary": "完成用户认证模块",
      "keywords": ["认证", "JWT", "OAuth2"],
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
  "summary": "完成用户认证模块",
  "content": "## 实现细节\n\n- 使用 JWT token\n- 支持 OAuth2",
  "keywords": ["认证", "JWT", "OAuth2"],
  "createdAt": "2026-04-25T10:00:00Z",
  "updatedAt": "2026-04-25T10:00:00Z"
}
```

### Neue Notiz erstellen

**POST** `/api/beings/{id}/work-notes`

**Anfrage**:
```json
{
  "summary": "完成用户认证模块",
  "content": "## 实现细节\n\n- 使用 JWT token",
  "keywords": "认证,JWT,OAuth2"
}
```

**Antwort**: `201 Created`

### Notiz aktualisieren

**PUT** `/api/beings/{id}/work-notes/{pageNumber}`

**Anfrage**:
```json
{
  "summary": "完成用户认证模块及测试",
  "content": "## 更新后的内容\n\n添加了单元测试",
  "keywords": "认证,JWT,OAuth2,测试"
}
```

### Notiz löschen

**DELETE** `/api/beings/{id}/work-notes/{pageNumber}`

### Notizen suchen

**GET** `/api/beings/{id}/work-notes/search?keyword=认证&maxResults=10`

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
      "title": "快速开始",
      "category": "入门指南"
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
  "title": "快速开始",
  "content": "# 快速开始\n\n...",
  "category": "入门指南"
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
