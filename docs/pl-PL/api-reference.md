# Referencja API

> **Wersja: v0.1.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | [Polski](../pl-PL/api-reference.md)

## Punkty końcowe Web API

Bazowy URL: `http://localhost:8080`

### Uwierzytelnianie

Większość punktów końcowych wymaga uwierzytelnienia za pomocą ciasteczka sesji zarządzanego przez Web UI.

---

## Zarządzanie Istotami Krzemowymi

### Pobranie wszystkich istot

**GET** `/api/beings`

**Odpowiedź**:
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

**Wartości stanu**: `idle` | `running` | `waiting_permission` | `stopped`

### Utworzenie istoty

**POST** `/api/beings`

**Żądanie**:
```json
{
  "name": "New Being",
  "soul": "# Personality\nYou are helpful..."
}
```

**Odpowiedź**: `201 Created`

### Uruchomienie istoty

**POST** `/api/beings/{id}/start`

### Zatrzymanie istoty

**POST** `/api/beings/{id}/stop`

### Pobranie szczegółów istoty

**GET** `/api/beings/{id}`

---

## System czatu

### Wysłanie wiadomości

**POST** `/api/chat`

**Żądanie**:
```json
{
  "beingId": "being-uuid",
  "message": "Hello, how are you?",
  "sessionId": "optional-session-id"
}
```

**Odpowiedź** (niestrumieniowa):
```json
{
  "reply": "I'm doing well, thank you!",
  "sessionId": "session-uuid",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### Czat strumieniowy (SSE)

**GET** `/api/chat/stream?beingId={id}&message={msg}`

**Odpowiedź**: Strumień zdarzeń wysyłanych przez serwer

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Pobranie historii czatu

**GET** `/api/chat/{sessionId}/history`

**Odpowiedź**:
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

## Konfiguracja

### Pobranie konfiguracji

**GET** `/api/config`

**Odpowiedź**:
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

### Aktualizacja konfiguracji

**POST** `/api/config`

**Żądanie**:
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

## System uprawnień

### Pobranie uprawnień

**GET** `/api/permissions`

**Odpowiedź**:
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

### Nadanie uprawnień

**POST** `/api/permissions`

**Żądanie**:
```json
{
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "duration": 3600
}
```

### Odwołanie uprawnień

**DELETE** `/api/permissions/{id}`

### Sprawdzenie uprawnień

**POST** `/api/permissions/check`

**Żądanie**:
```json
{
  "userId": "user-uuid",
  "resource": "network:http"
}
```

**Odpowiedź**:
```json
{
  "allowed": true,
  "reason": "Granted by curator"
}
```

---

## System zadań i czasomierzy

### Utworzenie zadania

**POST** `/api/tasks`

**Żądanie**:
```json
{
  "beingId": "being-uuid",
  "description": "Review code",
  "priority": 5,
  "dueDate": "2026-04-21T12:00:00Z"
}
```

### Pobranie zadań

**GET** `/api/tasks?beingId={id}&status=pending`

### Aktualizacja stanu zadania

**PATCH** `/api/tasks/{id}`

**Żądanie**:
```json
{
  "status": "completed"
}
```

### Utworzenie czasomierza

**POST** `/api/timers`

**Żądanie**:
```json
{
  "beingId": "being-uuid",
  "interval": 3600,
  "action": "think",
  "repeat": true
}
```

### Usunięcie czasomierza

**DELETE** `/api/timers/{id}`

---

## Audyt i logi

### Pobranie użycia Tokenów

**GET** `/api/audit/tokens?startDate={date}&endDate={date}`

**Odpowiedź**:
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

### Pobranie logów

**GET** `/api/logs?level=error&limit=100`

**Odpowiedź**:
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

## API przechowywania

### Odczyt wartości

**GET** `/api/storage?key={key}`

**Odpowiedź**:
```json
{
  "key": "being:uuid:memory",
  "value": "{...}",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### Zapis wartości

**POST** `/api/storage`

**Żądanie**:
```json
{
  "key": "being:uuid:memory",
  "value": "{...}"
}
```

### Zapytanie według zakresu czasowego

**GET** `/api/storage/time?start={start}&end={end}&prefix={prefix}`

**Odpowiedź**:
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

## Informacje o systemie

### Pobranie strony o systemie

**GET** `/about`

Zwraca stronę o systemie, zawierającą informacje o systemie i listę załadowanych wtyczek.

**Dane listy wtyczek**:
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

### Żądanie uprawnień

**GET** `/permission/request?userId={id}&type={type}&resource={resource}`

Wyświetla stronę żądania uprawnień, pozwalając użytkownikowi zatwierdzić lub odrzucić żądanie uprawnień Istoty Krzemowej.

**Parametry zapytania**:

| Parametr | Typ | Opis |
|----------|-----|------|
| `userId` | `Guid` | ID Istoty Krzemowej żądającej uprawnień |
| `type` | `string` | Typ uprawnień |
| `resource` | `string` | Ścieżka żądanego zasobu |
| `allowCode` | `string` | Identyfikator kodu operacji zezwolenia |
| `denyCode` | `string` | Identyfikator kodu operacji odmowy |

**Sprawdzenie oczekujących żądań uprawnień**:

**GET** `/permission/check?userId={id}`

**Odpowiedź**:
```json
{
  "pending": true
}
```

**Odpowiedź na żądanie uprawnień**:

**GET** `/permission/respond?userId={id}&allowed={bool}&addToCache={bool}&cacheDuration={hours}`

**Parametry zapytania**:

| Parametr | Typ | Opis |
|----------|-----|------|
| `userId` | `Guid` | ID Istoty Krzemowej |
| `allowed` | `bool` | Czy zezwolić |
| `addToCache` | `bool` | Czy zapisać decyzję w pamięci podręcznej |
| `cacheDuration` | `double` | Czas trwania pamięci podręcznej (godziny) |

**Odpowiedź**:
```json
{
  "success": true
}
```

### Pobranie danych pulpitu nawigacyjnego

**GET** `/api/dashboard`

**Odpowiedź**:
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

### Pobranie stanu systemu

**GET** `/api/status`

**Odpowiedź**:
```json
{
  "version": "1.0.0",
  "runtime": ".NET 9.0",
  "uptime": 86400,
  "health": "healthy"
}
```

---

## Odpowiedzi błędów

Wszystkie punkty końcowe zwracają ustandaryzowane odpowiedzi błędów:

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: disk:write, Current: disk:read"
  }
}
```

### Typowe kody błędów

| Kod | Status HTTP | Opis |
|-----|-------------|------|
| `PERMISSION_DENIED` | 403 | Niewystarczające uprawnienia |
| `NOT_FOUND` | 404 | Zasób nie znaleziony |
| `VALIDATION_ERROR` | 400 | Nieprawidłowe parametry żądania |
| `INTERNAL_ERROR` | 500 | Wewnętrzny błąd serwera |
| `SERVICE_UNAVAILABLE` | 503 | Usługa AI niedostępna |

---

## Zdarzenia SSE

Zdarzenia wysyłane przez serwer są używane do aktualizacji w czasie rzeczywistym:

### Zdarzenia czatu

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

### Zdarzenia stanu istoty

```javascript
const beingEvents = new EventSource('/api/beings/events');

beingEvents.onmessage = (event) => {
  const data = JSON.parse(event.data);
  console.log(`Being ${data.beingId} status: ${data.status}`);
};
```

---

## API klienta AI

### Interfejs IAIClient

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

## API notatek pracy

### Pobranie listy notatek pracy

**GET** `/api/beings/{id}/work-notes`

**Odpowiedź**:
```json
{
  "notes": [
    {
      "id": "note-uuid",
      "pageNumber": 1,
      "summary": "Ukończono moduł uwierzytelniania użytkowników",
      "keywords": ["uwierzytelnianie", "JWT", "OAuth2"],
      "createdAt": "2026-04-25T10:00:00Z",
      "updatedAt": "2026-04-25T10:00:00Z"
    }
  ],
  "totalCount": 15
}
```

### Pobranie szczegółów pojedynczej notatki

**GET** `/api/beings/{id}/work-notes/{pageNumber}`

**Odpowiedź**:
```json
{
  "id": "note-uuid",
  "pageNumber": 1,
  "summary": "Ukończono moduł uwierzytelniania użytkowników",
  "content": "## Szczegóły implementacji\n\n- Użycie tokena JWT\n- Obsługa OAuth2",
  "keywords": ["uwierzytelnianie", "JWT", "OAuth2"],
  "createdAt": "2026-04-25T10:00:00Z",
  "updatedAt": "2026-04-25T10:00:00Z"
}
```

### Utworzenie nowej notatki

**POST** `/api/beings/{id}/work-notes`

**Żądanie**:
```json
{
  "summary": "Ukończono moduł uwierzytelniania użytkowników",
  "content": "## Szczegóły implementacji\n\n- Użycie tokena JWT",
  "keywords": "uwierzytelnianie,JWT,OAuth2"
}
```

**Odpowiedź**: `201 Created`

### Aktualizacja notatki

**PUT** `/api/beings/{id}/work-notes/{pageNumber}`

**Żądanie**:
```json
{
  "summary": "Ukończono moduł uwierzytelniania użytkowników i testy",
  "content": "## Zaktualizowana treść\n\nDodano testy jednostkowe",
  "keywords": "uwierzytelnianie,JWT,OAuth2,testy"
}
```

### Usunięcie notatki

**DELETE** `/api/beings/{id}/work-notes/{pageNumber}`

### Wyszukanie notatek

**GET** `/api/beings/{id}/work-notes/search?keyword=uwierzytelnianie&maxResults=10`

### Pobranie katalogu notatek

**GET** `/api/beings/{id}/work-notes/directory`

---

## API sieci wiedzy

### Pobranie statystyk wiedzy

**GET** `/api/knowledge/stats`

**Odpowiedź**:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Dodanie trójki wiedzy

**POST** `/api/knowledge/triples`

**Żądanie**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95,
  "tags": ["programming", "language"]
}
```

**Odpowiedź**: `201 Created`

### Zapytanie o wiedzę

**GET** `/api/knowledge/query?subject=Python&predicate=is_a`

**Odpowiedź**:
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

### Wyszukanie wiedzy

**GET** `/api/knowledge/search?query=programming+language&limit=10`

### Pobranie ścieżki wiedzy

**GET** `/api/knowledge/path?from=Python&to=computer_science`

**Odpowiedź**:
```json
{
  "path": [
    {"subject": "Python", "predicate": "is_a", "object": "programming_language"},
    {"subject": "programming_language", "predicate": "belongs_to", "object": "computer_science"}
  ],
  "length": 2
}
```

### Walidacja wiedzy

**POST** `/api/knowledge/validate`

**Żądanie**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

### Usunięcie wiedzy

**DELETE** `/api/knowledge/triples/{id}`

---

## API systemu dokumentacji pomocy

### Pobranie listy dokumentacji pomocy

**GET** `/api/help`

**Odpowiedź**:
```json
{
  "topics": [
    {
      "id": "getting-started",
      "title": "Szybki start",
      "category": "Przewodnik początkujący"
    }
  ]
}
```

### Pobranie szczegółów dokumentacji pomocy

**GET** `/api/help/{topicId}`

**Odpowiedź**:
```json
{
  "id": "getting-started",
  "title": "Szybki start",
  "content": "# Szybki start\n\n...",
  "category": "Przewodnik początkujący"
}
```

---

## API przeglądarki WebView

### Pobranie stanu przeglądarki

**GET** `/api/beings/{id}/browser/status`

**Odpowiedź**:
```json
{
  "is_open": true,
  "current_url": "https://example.com",
  "page_title": "Example Page",
  "is_loading": false,
  "last_operation_time": "2026-04-26T10:00:00Z"
}
```

### Otwarcie przeglądarki

**POST** `/api/beings/{id}/browser/open`

### Zamknięcie przeglądarki

**POST** `/api/beings/{id}/browser/close`

### Nawigacja do URL

**POST** `/api/beings/{id}/browser/navigate`

**Żądanie**:
```json
{
  "url": "https://example.com"
}
```

### Wykonanie JavaScript

**POST** `/api/beings/{id}/browser/execute-script`

**Żądanie**:
```json
{
  "script": "return document.title;"
}
```

### Pobranie zrzutu ekranu strony

**GET** `/api/beings/{id}/browser/screenshot`

---

## API obszaru roboczego projektów

### Pobranie listy projektów

**GET** `/api/projects`

**Odpowiedź**:
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

### Utworzenie projektu

**POST** `/api/projects`

**Żądanie**:
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### Pobranie szczegółów projektu

**GET** `/api/projects/{id}`

### Aktualizacja projektu

**PUT** `/api/projects/{id}`

### Usunięcie projektu

**DELETE** `/api/projects/{id}`

---

## API systemu narzędzi

### Interfejs ITool

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

## Następne kroki

- 🚀 Zobacz [przewodnik szybkiego startu](getting-started.md)
- 🛠️ Przeczytaj [przewodnik programistyczny](development-guide.md)
- 📚 Zobacz [dokumentację architektury](architecture.md)
- 🔒 Poznaj [model bezpieczeństwa](security.md)
