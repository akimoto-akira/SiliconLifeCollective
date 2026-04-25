# API Reference

[English](api-reference.md) | [简体中文](docs/zh-CN/api-reference.md) | [繁體中文](docs/zh-HK/api-reference.md) | [Español](docs/es-ES/api-reference.md) | [日本語](docs/ja-JP/api-reference.md) | [한국어](docs/ko-KR/api-reference.md) | [Čeština](docs/cs-CZ/api-reference.md)

## Web API Endpoints

Base URL: `http://localhost:8080`

### Authentication

Most endpoints require authentication via session cookies managed by the Web UI.

---

## Silicon Being Management

### Get All Beings

**GET** `/api/beings`

**Response**:
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

### Create Being

**POST** `/api/beings`

**Request**:
```json
{
  "name": "New Being",
  "soul": "# Personality\nYou are helpful..."
}
```

**Response**: `201 Created`

### Start Being

**POST** `/api/beings/{id}/start`

### Stop Being

**POST** `/api/beings/{id}/stop`

### Get Being Details

**GET** `/api/beings/{id}`

---

## Chat System

### Send Message

**POST** `/api/chat`

**Request**:
```json
{
  "beingId": "being-uuid",
  "message": "Hello, how are you?",
  "sessionId": "optional-session-id"
}
```

**Response** (Non-streaming):
```json
{
  "reply": "I'm doing well, thank you!",
  "sessionId": "session-uuid",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### Stream Chat (SSE)

**GET** `/api/chat/stream?beingId={id}&message={msg}`

**Response**: Server-Sent Events stream

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Get Chat History

**GET** `/api/chat/{sessionId}/history`

**Response**:
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

## Configuration

### Get Configuration

**GET** `/api/config`

**Response**:
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

### Update Configuration

**POST** `/api/config`

**Request**:
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

## Permission System

### Get Permissions

**GET** `/api/permissions`

**Response**:
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

### Grant Permission

**POST** `/api/permissions`

**Request**:
```json
{
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "duration": 3600
}
```

### Revoke Permission

**DELETE** `/api/permissions/{id}`

### Check Permission

**POST** `/api/permissions/check`

**Request**:
```json
{
  "userId": "user-uuid",
  "resource": "network:http"
}
```

**Response**:
```json
{
  "allowed": true,
  "reason": "Granted by curator"
}
```

---

## Task & Timer System

### Create Task

**POST** `/api/tasks`

**Request**:
```json
{
  "beingId": "being-uuid",
  "description": "Review code",
  "priority": 5,
  "dueDate": "2026-04-21T12:00:00Z"
}
```

### Get Tasks

**GET** `/api/tasks?beingId={id}&status=pending`

### Update Task Status

**PATCH** `/api/tasks/{id}`

**Request**:
```json
{
  "status": "completed"
}
```

### Create Timer

**POST** `/api/timers`

**Request**:
```json
{
  "beingId": "being-uuid",
  "interval": 3600,
  "action": "think",
  "repeat": true
}
```

### Delete Timer

**DELETE** `/api/timers/{id}`

---

## Audit & Logging

### Get Token Usage

**GET** `/api/audit/tokens?startDate={date}&endDate={date}`

**Response**:
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

### Get Logs

**GET** `/api/logs?level=error&limit=100`

**Response**:
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

### Read Value

**GET** `/api/storage?key={key}`

**Response**:
```json
{
  "key": "being:uuid:memory",
  "value": "{...}",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### Write Value

**POST** `/api/storage`

**Request**:
```json
{
  "key": "being:uuid:memory",
  "value": "{...}"
}
```

### Query by Time Range

**GET** `/api/storage/time?start={start}&end={end}&prefix={prefix}`

**Response**:
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

## System Information

### Get Dashboard Data

**GET** `/api/dashboard`

**Response**:
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

### Get System Status

**GET** `/api/status`

**Response**:
```json
{
  "version": "1.0.0",
  "runtime": ".NET 9.0",
  "uptime": 86400,
  "health": "healthy"
}
```

---

## Error Responses

All endpoints return standardized error responses:

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: disk:write, Current: disk:read"
  }
}
```

### Common Error Codes

| Code | HTTP Status | Description |
|------|-------------|-------------|
| `PERMISSION_DENIED` | 403 | Insufficient permissions |
| `NOT_FOUND` | 404 | Resource not found |
| `VALIDATION_ERROR` | 400 | Invalid request parameters |
| `INTERNAL_ERROR` | 500 | Internal server error |
| `SERVICE_UNAVAILABLE` | 503 | AI service unavailable |

---

## SSE Events

Server-Sent Events are used for real-time updates:

### Chat Events

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

### Being Status Events

```javascript
const beingEvents = new EventSource('/api/beings/events');

beingEvents.onmessage = (event) => {
  const data = JSON.parse(event.data);
  console.log(`Being ${data.beingId} status: ${data.status}`);
};
```

---

## AI Client API

### IAIClient Interface

```csharp
public interface IAIClient
{
    string Name { get; }
    
    Task<AIResponse> ChatAsync(AIRequest request);
    
    IAsyncEnumerable<string> StreamChatAsync(AIRequest request);
}
```

### AIRequest Structure

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

### AIResponse Structure

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

## Tool System API

### ITool Interface

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    ToolDefinition Definition { get; }
    
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### ToolCall Structure

```csharp
public class ToolCall
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}
```

### ToolResult Structure

```csharp
public class ToolResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }
}
```

---

## Work Notes API

### Get Work Notes List

**GET** `/api/beings/{id}/work-notes`

**Response**:
```json
{
  "notes": [
    {
      "id": "note-uuid",
      "pageNumber": 1,
      "summary": "User authentication module completed",
      "keywords": ["authentication", "JWT", "OAuth2"],
      "createdAt": "2026-04-25T10:00:00Z",
      "updatedAt": "2026-04-25T10:00:00Z"
    }
  ],
  "totalCount": 15
}
```

### Get Single Note Details

**GET** `/api/beings/{id}/work-notes/{pageNumber}`

**Response**:
```json
{
  "id": "note-uuid",
  "pageNumber": 1,
  "summary": "User authentication module completed",
  "content": "## Implementation Details\n\n- Using JWT token\n- OAuth2 support",
  "keywords": ["authentication", "JWT", "OAuth2"],
  "createdAt": "2026-04-25T10:00:00Z",
  "updatedAt": "2026-04-25T10:00:00Z"
}
```

### Create New Note

**POST** `/api/beings/{id}/work-notes`

**Request**:
```json
{
  "summary": "User authentication module completed",
  "content": "## Implementation Details\n\n- Using JWT token",
  "keywords": "authentication,JWT,OAuth2"
}
```

**Response**: `201 Created`

### Update Note

**PUT** `/api/beings/{id}/work-notes/{pageNumber}`

**Request**:
```json
{
  "summary": "User authentication module and tests completed",
  "content": "## Updated Content\n\nUnit tests added",
  "keywords": "authentication,JWT,OAuth2,tests"
}
```

### Delete Note

**DELETE** `/api/beings/{id}/work-notes/{pageNumber}`

### Search Notes

**GET** `/api/beings/{id}/work-notes/search?keyword=authentication&maxResults=10`

### Get Note Directory

**GET** `/api/beings/{id}/work-notes/directory`

---

## Knowledge Network API

### Get Knowledge Statistics

**GET** `/api/knowledge/stats`

**Response**:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Add Knowledge Triple

**POST** `/api/knowledge/triples`

**Request**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95,
  "tags": ["programming", "language"]
}
```

**Response**: `201 Created`

### Query Knowledge

**GET** `/api/knowledge/query?subject=Python&predicate=is_a`

**Response**:
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

### Search Knowledge

**GET** `/api/knowledge/search?query=programming+language&limit=10`

### Get Knowledge Path

**GET** `/api/knowledge/path?from=Python&to=computer_science`

**Response**:
```json
{
  "path": [
    {"subject": "Python", "predicate": "is_a", "object": "programming_language"},
    {"subject": "programming_language", "predicate": "belongs_to", "object": "computer_science"}
  ],
  "length": 2
}
```

### Validate Knowledge

**POST** `/api/knowledge/validate`

**Request**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

### Delete Knowledge

**DELETE** `/api/knowledge/triples/{id}`

---

## Project Management API

### Get Projects List

**GET** `/api/projects`

**Response**:
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

### Create Project

**POST** `/api/projects`

**Request**:
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### Get Project Details

**GET** `/api/projects/{id}`

### Update Project

**PUT** `/api/projects/{id}`

### Delete Project

**DELETE** `/api/projects/{id}`

---

## Next Steps

- 🚀 Check the [Getting Started Guide](getting-started.md)
- 🛠️ Read the [Development Guide](development-guide.md)
- 📚 Review the [Architecture Documentation](architecture.md)
- 🔒 Understand the [Security Model](security.md)
