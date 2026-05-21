# API Reference

> **Version: v0.2.0-alpha**

**English** | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [Čeština](../cs-CZ/api-reference.md)

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
      "activity": "Idle",
      "soul": "path/to/soul.md"
    }
  ]
}
```

**Activity Values**: `Idle` | `SingleChat` | `GroupChat` | `Task` | `Timer` | `Broadcast` | `Project` | `MemoryCompression` | `Stopped`

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

**POST** `/api/chat/send`

**Request**:
```json
{
  "channelId": "session-uuid",
  "content": "Hello, how are you?"
}
```

**Response**:
```json
{
  "success": true,
  "messageId": "message-uuid"
}
```

### Streaming Chat (SSE)

**GET** `/api/chat/stream`

Server-Sent Events stream for real-time chat updates.

### Get Conversations

**GET** `/api/chat/conversations`

**Response**:
```json
{
  "conversations": [
    {
      "sessionId": "session-uuid",
      "beingId": "being-uuid",
      "type": "single",
      "displayName": "Chat with Assistant",
      "lastMessage": "Hello!",
      "lastTime": "2026-04-20T10:30:00Z"
    }
  ]
}
```

### Get Messages

**GET** `/api/chat/messages?channelId={sessionId}`

**Response**:
```json
{
  "messages": [
    {
      "id": "message-uuid",
      "senderId": "sender-uuid",
      "channelId": "session-uuid",
      "content": "Hello",
      "timestamp": "2026-04-20T10:30:00Z",
      "role": "user"
    }
  ]
}
```

### Get Chat History

**GET** `/api/chat/history`

Returns chat history sessions.

### Stop AI Thinking

**POST** `/api/chat/stop`

Stops the current AI streaming response.

### Upload File

**POST** `/api/chat/upload`

Uploads a file to the chat session.

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

## Task and Timer System

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

## Audit and Logging

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

## Memory API

### Get Memory List

**GET** `/api/memory/list`

**Query Parameters**: `beingId`, `type`, `limit`

### Get Memory Detail

**GET** `/api/memory/detail/{id}`

### Get Memory Statistics

**GET** `/api/memory/stats`

**Query Parameters**: `beingId`

### Search Memory

**GET** `/api/memory/search`

**Query Parameters**: `beingId`, `keyword`, `limit`

### Get Memory Beings

**GET** `/api/memory/beings`

Returns list of beings with memory data.

### Trace Memory Original

**GET** `/api/memory/trace/{id}`

Traces the original source of a memory entry.

### Get Memory Timeline HTML

**GET** `/api/memory/timeline-html`

**Query Parameters**: `beingId`

Returns HTML fragment for memory timeline visualization.

---

## Code Browser API

### Get Code Types

**GET** `/api/code/types`

Returns all available types for code browsing.

### Get Code Detail

**GET** `/api/code/detail`

**Query Parameters**: `type`, `member`

Returns detailed information about a specific type or member.

---

## Executor API

### Get Executor Status

**GET** `/api/executors/status`

**Response**:
```json
[
  { "name": "DiskExecutor", "status": "Idle", "queueCount": 0 },
  { "name": "NetworkExecutor", "status": "Idle", "queueCount": 0 },
  { "name": "CommandLineExecutor", "status": "Idle", "queueCount": 0 }
]
```

---

## System Information

### Get About Page

**GET** `/about`

Returns the about page, including system information and loaded plugin list.

**Plugin List Data**:
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

### Permission Request

**GET** `/permission/request?userId={id}&type={type}&resource={resource}`

Displays the permission request page, allowing users to approve or deny a silicon being's permission request.

**Query Parameters**:

| Parameter | Type | Description |
|------|------|------|
| `userId` | `Guid` | Silicon being ID requesting permission |
| `type` | `string` | Permission type |
| `resource` | `string` | Requested resource path |
| `allowCode` | `string` | Code identifier for allow action |
| `denyCode` | `string` | Code identifier for deny action |

**Check Pending Permission Requests**:

**GET** `/permission/check?userId={id}`

**Response**:
```json
{
  "pending": true
}
```

**Respond to Permission Request**:

**GET** `/permission/respond?userId={id}&allowed={bool}&addToCache={bool}&cacheDuration={hours}`

**Query Parameters**:

| Parameter | Type | Description |
|------|------|------|
| `userId` | `Guid` | Silicon being ID |
| `allowed` | `bool` | Whether to allow |
| `addToCache` | `bool` | Whether to cache the decision |
| `cacheDuration` | `double` | Cache duration (hours) |

**Response**:
```json
{
  "success": true
}
```

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

Server-Sent Events for real-time updates:

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

### AIRequest Structure

```csharp
public class AIRequest
{
    public string Model { get; set; } = string.Empty;
    public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    public List<ToolDefinition>? Tools { get; set; }
}
```

### AIResponse Structure

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
      "summary": "Completed user authentication module",
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
  "summary": "Completed user authentication module",
  "content": "## Implementation Details\n\n- Using JWT token\n- Supports OAuth2",
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
  "summary": "Completed user authentication module",
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
  "summary": "Completed user authentication module and tests",
  "content": "## Updated Content\n\nAdded unit tests",
  "keywords": "authentication,JWT,OAuth2,tests"
}
```

### Delete Note

**DELETE** `/api/beings/{id}/work-notes/{pageNumber}`

### Search Notes

**GET** `/api/beings/{id}/work-notes/search?keyword=authentication&maxResults=10`

### Get Notes Directory

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

## Help Documentation System API

### Get Help Documentation List

**GET** `/api/help`

**Response**:
```json
{
  "topics": [
    {
      "id": "getting-started",
      "title": "Getting Started",
      "category": "Getting Started Guide"
    }
  ]
}
```

### Get Help Documentation Details

**GET** `/api/help/{topicId}`

**Response**:
```json
{
  "id": "getting-started",
  "title": "Getting Started",
  "content": "# Getting Started\n\n...",
  "category": "Getting Started Guide"
}
```

---

## WebView Browser API

### Get Browser Status

**GET** `/api/beings/{id}/browser/status`

**Response**:
```json
{
  "is_open": true,
  "current_url": "https://example.com",
  "page_title": "Example Page",
  "is_loading": false,
  "last_operation_time": "2026-04-26T10:00:00Z"
}
```

### Open Browser

**POST** `/api/beings/{id}/browser/open`

### Close Browser

**POST** `/api/beings/{id}/browser/close`

### Navigate to URL

**POST** `/api/beings/{id}/browser/navigate`

**Request**:
```json
{
  "url": "https://example.com"
}
```

### Execute JavaScript

**POST** `/api/beings/{id}/browser/execute-script`

**Request**:
```json
{
  "script": "return document.title;"
}
```

### Get Page Screenshot

**GET** `/api/beings/{id}/browser/screenshot`

---

## Project Workspace API

### Get Project List

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

## Tool System API

### ITool Interface

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

### ToolCall Structure

```csharp
public class ToolCall
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public Dictionary<string, object> Arguments { get; set; } = new();
}
```

### ToolResult Structure

```csharp
public class ToolResult
{
    public bool Success { get; }
    public string Message { get; }
    public object? Data { get; }
}
```

---

## Next Steps

- 🚀 View [Getting Started Guide](getting-started.md)
- 🛠️ Read [Development Guide](development-guide.md)
- 📚 View [Architecture Document](architecture.md)
- 🔒 Learn about [Security Model](security.md)
