# API Reference

[English](../en/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | **Čeština**

## Webové API Endpointy

Základní URL: `http://localhost:8080`

### Autentizace

Většina endpointů vyžaduje autentizaci prostřednictvím session cookies spravovaných Web UI.

---

## Správa Silikonových Bytostí

### Získat Všechny Bytosti

**GET** `/api/beings`

**Odpověď**:
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

### Vytvořit Bytost

**POST** `/api/beings`

**Požadavek**:
```json
{
  "name": "New Being",
  "soul": "# Personality\nYou are helpful..."
}
```

**Odpověď**: `201 Created`

### Spustit Bytost

**POST** `/api/beings/{id}/start`

### Zastavit Bytost

**POST** `/api/beings/{id}/stop`

### Získat Detaily Bytosti

**GET** `/api/beings/{id}`

---

## Chatovací Systém

### Odeslat Zprávu

**POST** `/api/chat`

**Požadavek**:
```json
{
  "beingId": "being-uuid",
  "message": "Hello, how are you?",
  "sessionId": "optional-session-id"
}
```

**Odpověď** (non-streaming):
```json
{
  "reply": "I'm doing well, thank you!",
  "sessionId": "session-uuid",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### Streamovaný Chat (SSE)

**GET** `/api/chat/stream?beingId={id}&message={msg}`

**Odpověď**: Server-sent events stream

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Získat Historii Chatu

**GET** `/api/chat/{sessionId}/history`

**Odpověď**:
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

## Konfigurace

### Získat Konfiguraci

**GET** `/api/config`

**Odpověď**:
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

### Aktualizovat Konfiguraci

**POST** `/api/config`

**Požadavek**:
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

## Systém Oprávnění

### Získat Oprávnění

**GET** `/api/permissions`

**Odpověď**:
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

### Udělit Oprávnění

**POST** `/api/permissions`

**Požadavek**:
```json
{
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "duration": 3600
}
```

### Odvolat Oprávnění

**DELETE** `/api/permissions/{id}`

### Zkontrolovat Oprávnění

**POST** `/api/permissions/check`

**Požadavek**:
```json
{
  "userId": "user-uuid",
  "resource": "network:http"
}
```

**Odpověď**:
```json
{
  "allowed": true,
  "reason": "Granted by curator"
}
```

---

## Systém Úkolů a Časovačů

### Vytvořit Úkol

**POST** `/api/tasks`

**Požadavek**:
```json
{
  "beingId": "being-uuid",
  "description": "Review code",
  "priority": 5,
  "dueDate": "2026-04-21T12:00:00Z"
}
```

### Získat Úkoly

**GET** `/api/tasks?beingId={id}&status=pending`

### Aktualizovat Stav Úkolu

**PATCH** `/api/tasks/{id}`

**Požadavek**:
```json
{
  "status": "completed"
}
```

### Vytvořit Časovač

**POST** `/api/timers`

**Požadavek**:
```json
{
  "beingId": "being-uuid",
  "interval": 3600,
  "action": "think",
  "repeat": true
}
```

### Smazat Časovač

**DELETE** `/api/timers/{id}`

---

## Audit a Logy

### Získat Využití Tokenů

**GET** `/api/audit/tokens?startDate={date}&endDate={date}`

**Odpověď**:
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

### Získat Logy

**GET** `/api/logs?level=error&limit=100`

**Odpověď**:
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

## Storage API

### Číst Hodnotu

**GET** `/api/storage?key={key}`

**Odpověď**:
```json
{
  "key": "being:uuid:memory",
  "value": "{...}",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### Zapisovat Hodnotu

**POST** `/api/storage`

**Požadavek**:
```json
{
  "key": "being:uuid:memory",
  "value": "{...}"
}
```

### Dotaz podle Časového Rozsahu

**GET** `/api/storage/time?start={start}&end={end}&prefix={prefix}`

**Odpověď**:
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

## Systémové Informace

### Získat Data Dashboardu

**GET** `/api/dashboard`

**Odpověď**:
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

### Získat Stav Systému

**GET** `/api/status`

**Odpověď**:
```json
{
  "version": "1.0.0",
  "runtime": ".NET 9.0",
  "uptime": 86400,
  "health": "healthy"
}
```

---

## Chybové Odpovědi

Všechny endpointy vrací standardizované chybové odpovědi:

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: disk:write, Current: disk:read"
  }
}
```

### Běžné Chybové Kódy

| Kód | HTTP Stav | Popis |
|------|-------------|-------------|
| `PERMISSION_DENIED` | 403 | Nedostatečné oprávnění |
| `NOT_FOUND` | 404 | Zdroj nenalezen |
| `VALIDATION_ERROR` | 400 | Neplatné parametry požadavku |
| `INTERNAL_ERROR` | 500 | Interní chyba serveru |
| `SERVICE_UNAVAILABLE` | 503 | AI služba není dostupná |

---

## SSE Události

Server-sent events pro aktualizace v reálném čase:

### Chatovací Události

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

### Události Stavu Bytosti

```javascript
const beingEvents = new EventSource('/api/beings/events');

beingEvents.onmessage = (event) => {
  const data = JSON.parse(event.data);
  console.log(`Being ${data.beingId} status: ${data.status}`);
};
```

---

## AI Klientské API

### Rozhraní IAIClient

```csharp
public interface IAIClient
{
    string Name { get; }
    
    Task<AIResponse> ChatAsync(AIRequest request);
    
    IAsyncEnumerable<string> StreamChatAsync(AIRequest request);
}
```

### Struktura AIRequest

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

### Struktura AIResponse

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

## API Pracovních Poznámek

### Získat Seznam Pracovních Poznámek

**GET** `/api/beings/{id}/work-notes`

**Odpověď**:
```json
{
  "notes": [
    {
      "id": "note-uuid",
      "pageNumber": 1,
      "summary": "Dokončen modul autentizace uživatelů",
      "keywords": ["autentizace", "JWT", "OAuth2"],
      "createdAt": "2026-04-25T10:00:00Z",
      "updatedAt": "2026-04-25T10:00:00Z"
    }
  ],
  "totalCount": 15
}
```

### Získat Detail Jedné Poznámky

**GET** `/api/beings/{id}/work-notes/{pageNumber}`

**Odpověď**:
```json
{
  "id": "note-uuid",
  "pageNumber": 1,
  "summary": "Dokončen modul autentizace uživatelů",
  "content": "## Implementační detaily\n\n- Použití JWT tokenu\n- Podpora OAuth2",
  "keywords": ["autentizace", "JWT", "OAuth2"],
  "createdAt": "2026-04-25T10:00:00Z",
  "updatedAt": "2026-04-25T10:00:00Z"
}
```

### Vytvořit Novou Poznámku

**POST** `/api/beings/{id}/work-notes`

**Požadavek**:
```json
{
  "summary": "Dokončen modul autentizace uživatelů",
  "content": "## Implementační detaily\n\n- Použití JWT tokenu",
  "keywords": "autentizace,JWT,OAuth2"
}
```

**Odpověď**: `201 Created`

### Aktualizovat Poznámku

**PUT** `/api/beings/{id}/work-notes/{pageNumber}`

**Požadavek**:
```json
{
  "summary": "Dokončen modul autentizace uživatelů a testy",
  "content": "## Aktualizovaný obsah\n\nPřidány unit testy",
  "keywords": "autentizace,JWT,OAuth2,testy"
}
```

### Smazat Poznámku

**DELETE** `/api/beings/{id}/work-notes/{pageNumber}`

### Vyhledat Poznámky

**GET** `/api/beings/{id}/work-notes/search?keyword=autentizace&maxResults=10`

### Získat Adresář Poznámek

**GET** `/api/beings/{id}/work-notes/directory`

---

## API Znalostní Sítě

### Získat Statistiky Znalostí

**GET** `/api/knowledge/stats`

**Odpověď**:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Přidat Znalostní Triple

**POST** `/api/knowledge/triples`

**Požadavek**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95,
  "tags": ["programming", "language"]
}
```

**Odpověď**: `201 Created`

### Dotazovat Znalosti

**GET** `/api/knowledge/query?subject=Python&predicate=is_a`

**Odpověď**:
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

### Vyhledat Znalosti

**GET** `/api/knowledge/search?query=programming+language&limit=10`

### Získat Cestu Znalostí

**GET** `/api/knowledge/path?from=Python&to=computer_science`

**Odpověď**:
```json
{
  "path": [
    {"subject": "Python", "predicate": "is_a", "object": "programming_language"},
    {"subject": "programming_language", "predicate": "belongs_to", "object": "computer_science"}
  ],
  "length": 2
}
```

### Ověřit Znalost

**POST** `/api/knowledge/validate`

**Požadavek**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

### Smazat Znalost

**DELETE** `/api/knowledge/triples/{id}`

---

## API Nápovědní Dokumentace

### Získat Seznam Nápovědních Dokumentů

**GET** `/api/help`

**Odpověď**:
```json
{
  "topics": [
    {
      "id": "getting-started",
      "title": "Rychlý start",
      "category": "Úvodní příručka"
    }
  ]
}
```

### Získat Detail Nápovědního Dokumentu

**GET** `/api/help/{topicId}`

**Odpověď**:
```json
{
  "id": "getting-started",
  "title": "Rychlý start",
  "content": "# Rychlý start\n\n...",
  "category": "Úvodní příručka"
}
```

---

## API WebView Prohlížeče

### Získat Stav Prohlížeče

**GET** `/api/beings/{id}/browser/status`

**Odpověď**:
```json
{
  "is_open": true,
  "current_url": "https://example.com",
  "page_title": "Example Page",
  "is_loading": false,
  "last_operation_time": "2026-04-26T10:00:00Z"
}
```

### Otevřít Prohlížeč

**POST** `/api/beings/{id}/browser/open`

### Zavřít Prohlížeč

**POST** `/api/beings/{id}/browser/close`

### Navigovat na URL

**POST** `/api/beings/{id}/browser/navigate`

**Požadavek**:
```json
{
  "url": "https://example.com"
}
```

### Spustit JavaScript

**POST** `/api/beings/{id}/browser/execute-script`

**Požadavek**:
```json
{
  "script": "return document.title;"
}
```

### Získat Screenshot Stránky

**GET** `/api/beings/{id}/browser/screenshot`

---

## API Projektového Pracovního Prostoru

### Získat Seznam Projektů

**GET** `/api/projects`

**Odpověď**:
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

### Vytvořit Projekt

**POST** `/api/projects`

**Požadavek**:
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### Získat Detail Projektu

**GET** `/api/projects/{id}`

### Aktualizovat Projekt

**PUT** `/api/projects/{id}`

### Smazat Projekt

**DELETE** `/api/projects/{id}`

---

## API Systému Nástrojů

### Rozhraní ITool

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    ToolDefinition Definition { get; }
    
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### Struktura ToolCall

```csharp
public class ToolCall
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}
```

### Struktura ToolResult

```csharp
public class ToolResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }
}
```

---

## Další Kroky

- 🚀 Podívejte se na [Průvodce rychlým startem](getting-started.md)
- 🛠️ Přečtěte si [Vývojářskou příručku](development-guide.md)
- 📚 Prozkoumejte [Dokument architektury](architecture.md)
- 🔒 Pochopte [Bezpečnostní model](security.md)
