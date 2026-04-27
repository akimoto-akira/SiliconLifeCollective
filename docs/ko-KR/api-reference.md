# API 참고

> **버전: v0.1.0-alpha**

[English](../en/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | **한국어** | [Deutsch](../de-DE/api-reference.md) | [Čeština](../cs-CZ/api-reference.md)

## Web API 엔드포인트

기본 URL: `http://localhost:8080`

### 인증

대부분의 엔드포인트는 Web UI가 관리하는 세션 쿠키를 통한 인증이 필요합니다.

---

## 실리콘 생명체 관리

### 모든 생명체 가져오기

**GET** `/api/beings`

**응답**:
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

### 생명체 만들기

**POST** `/api/beings`

**요청**:
```json
{
  "name": "New Being",
  "soul": "# Personality\nYou are helpful..."
}
```

**응답**: `201 Created`

### 생명체 시작

**POST** `/api/beings/{id}/start`

### 생명체 중지

**POST** `/api/beings/{id}/stop`

### 생명체 세부정보 가져오기

**GET** `/api/beings/{id}`

---

## 채팅 시스템

### 메시지 보내기

**POST** `/api/chat`

**요청**:
```json
{
  "beingId": "being-uuid",
  "message": "Hello, how are you?",
  "sessionId": "optional-session-id"
}
```

**응답** (비스트리밍):
```json
{
  "reply": "I'm doing well, thank you!",
  "sessionId": "session-uuid",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### 스트리밍 채팅 (SSE)

**GET** `/api/chat/stream?beingId={id}&message={msg}`

**응답**: 서버 전송 이벤트 스트림

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### 채팅 기록 가져오기

**GET** `/api/chat/{sessionId}/history`

**응답**:
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

## 구성

### 구성 가져오기

**GET** `/api/config`

**응답**:
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

### 구성 업데이트

**POST** `/api/config`

**요청**:
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

## 권한 시스템

### 권한 가져오기

**GET** `/api/permissions`

**응답**:
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

### 권한 부여

**POST** `/api/permissions`

**요청**:
```json
{
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "duration": 3600
}
```

### 권한 취소

**DELETE** `/api/permissions/{id}`

### 권한 검사

**POST** `/api/permissions/check`

**요청**:
```json
{
  "userId": "user-uuid",
  "resource": "network:http"
}
```

**응답**:
```json
{
  "allowed": true,
  "reason": "Granted by curator"
}
```

---

## 작업 및 타이머 시스템

### 작업 만들기

**POST** `/api/tasks`

**요청**:
```json
{
  "beingId": "being-uuid",
  "description": "Review code",
  "priority": 5,
  "dueDate": "2026-04-21T12:00:00Z"
}
```

### 작업 가져오기

**GET** `/api/tasks?beingId={id}&status=pending`

### 작업 상태 업데이트

**PATCH** `/api/tasks/{id}`

**요청**:
```json
{
  "status": "completed"
}
```

### 타이머 만들기

**POST** `/api/timers`

**요청**:
```json
{
  "beingId": "being-uuid",
  "interval": 3600,
  "action": "think",
  "repeat": true
}
```

### 타이머 삭제

**DELETE** `/api/timers/{id}`

---

## 감사 및 로그

### Token 사용 가져오기

**GET** `/api/audit/tokens?startDate={date}&endDate={date}`

**응답**:
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

### 로그 가져오기

**GET** `/api/logs?level=error&limit=100`

**응답**:
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

## 저장 API

### 값 읽기

**GET** `/api/storage?key={key}`

**응답**:
```json
{
  "key": "being:uuid:memory",
  "value": "{...}",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### 값 쓰기

**POST** `/api/storage`

**요청**:
```json
{
  "key": "being:uuid:memory",
  "value": "{...}"
}
```

### 시간 범위별 쿼리

**GET** `/api/storage/time?start={start}&end={end}&prefix={prefix}`

**응답**:
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

## 시스템 정보

### 대시보드 데이터 가져오기

**GET** `/api/dashboard`

**응답**:
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

### 시스템 상태 가져오기

**GET** `/api/status`

**응답**:
```json
{
  "version": "1.0.0",
  "runtime": ".NET 9.0",
  "uptime": 86400,
  "health": "healthy"
}
```

---

## 오류 응답

모든 엔드포인트는 표준화된 오류 응답을 반환합니다:

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: disk:write, Current: disk:read"
  }
}
```

### 일반 오류 코드

| 코드 | HTTP 상태 | 설명 |
|------|-------------|-------------|
| `PERMISSION_DENIED` | 403 | 권한 부족 |
| `NOT_FOUND` | 404 | 리소스를 찾을 수 없음 |
| `VALIDATION_ERROR` | 400 | 요청 매개변수 잘못됨 |
| `INTERNAL_ERROR` | 500 | 내부 서버 오류 |
| `SERVICE_UNAVAILABLE` | 503 | AI 서비스 사용 불가 |

---

## SSE 이벤트

서버 전송 이벤트는 실시간 업데이트에 사용됩니다:

### 채팅 이벤트

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

### 생명체 상태 이벤트

```javascript
const beingEvents = new EventSource('/api/beings/events');

beingEvents.onmessage = (event) => {
  const data = JSON.parse(event.data);
  console.log(`Being ${data.beingId} status: ${data.status}`);
};
```

---

## AI 클라이언트 API

### IAIClient 인터페이스

```csharp
public interface IAIClient
{
    string Name { get; }
    
    Task<AIResponse> ChatAsync(AIRequest request);
    
    IAsyncEnumerable<string> StreamChatAsync(AIRequest request);
}
```

### AIRequest 구조

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

### AIResponse 구조

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

## 작업 노트 API

### 작업 노트 목록 가져오기

**GET** `/api/beings/{id}/work-notes`

**응답**:
```json
{
  "notes": [
    {
      "id": "note-uuid",
      "pageNumber": 1,
      "summary": "사용자 인증 모듈 완성",
      "keywords": ["인증", "JWT", "OAuth2"],
      "createdAt": "2026-04-25T10:00:00Z",
      "updatedAt": "2026-04-25T10:00:00Z"
    }
  ],
  "totalCount": 15
}
```

### 단일 노트 세부정보 가져오기

**GET** `/api/beings/{id}/work-notes/{pageNumber}`

**응답**:
```json
{
  "id": "note-uuid",
  "pageNumber": 1,
  "summary": "사용자 인증 모듈 완성",
  "content": "## 구현 세부사항\n\n- JWT token 사용\n- OAuth2 지원",
  "keywords": ["인증", "JWT", "OAuth2"],
  "createdAt": "2026-04-25T10:00:00Z",
  "updatedAt": "2026-04-25T10:00:00Z"
}
```

### 새 노트 만들기

**POST** `/api/beings/{id}/work-notes`

**요청**:
```json
{
  "summary": "사용자 인증 모듈 완성",
  "content": "## 구현 세부사항\n\n- JWT token 사용",
  "keywords": "인증,JWT,OAuth2"
}
```

**응답**: `201 Created`

### 노트 업데이트

**PUT** `/api/beings/{id}/work-notes/{pageNumber}`

**요청**:
```json
{
  "summary": "사용자 인증 모듈 및 테스트 완성",
  "content": "## 업데이트된 내용\n\n단위 테스트 추가",
  "keywords": "인증,JWT,OAuth2,테스트"
}
```

### 노트 삭제

**DELETE** `/api/beings/{id}/work-notes/{pageNumber}`

### 노트 검색

**GET** `/api/beings/{id}/work-notes/search?keyword=인증&maxResults=10`

### 노트 디렉토리 가져오기

**GET** `/api/beings/{id}/work-notes/directory`

---

## 지식 네트워크 API

### 지식 통계 가져오기

**GET** `/api/knowledge/stats`

**응답**:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### 지식 트리플 추가

**POST** `/api/knowledge/triples`

**요청**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95,
  "tags": ["programming", "language"]
}
```

**응답**: `201 Created`

### 지식 쿼리

**GET** `/api/knowledge/query?subject=Python&predicate=is_a`

**응답**:
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

### 지식 검색

**GET** `/api/knowledge/search?query=programming+language&limit=10`

### 지식 경로 가져오기

**GET** `/api/knowledge/path?from=Python&to=computer_science`

**응답**:
```json
{
  "path": [
    {"subject": "Python", "predicate": "is_a", "object": "programming_language"},
    {"subject": "programming_language", "predicate": "belongs_to", "object": "computer_science"}
  ],
  "length": 2
}
```

### 지식 검증

**POST** `/api/knowledge/validate`

**요청**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

### 지식 삭제

**DELETE** `/api/knowledge/triples/{id}`

---

## 도움말 문서 시스템 API

### 도움말 문서 목록 가져오기

**GET** `/api/help`

**응답**:
```json
{
  "topics": [
    {
      "id": "getting-started",
      "title": "빠른 시작",
      "category": "시작 가이드"
    }
  ]
}
```

### 도움말 문서 세부정보 가져오기

**GET** `/api/help/{topicId}`

**응답**:
```json
{
  "id": "getting-started",
  "title": "빠른 시작",
  "content": "# 빠른 시작\n\n...",
  "category": "시작 가이드"
}
```

---

## WebView 브라우저 API

### 브라우저 상태 가져오기

**GET** `/api/beings/{id}/browser/status`

**응답**:
```json
{
  "is_open": true,
  "current_url": "https://example.com",
  "page_title": "Example Page",
  "is_loading": false,
  "last_operation_time": "2026-04-26T10:00:00Z"
}
```

### 브라우저 열기

**POST** `/api/beings/{id}/browser/open`

### 브라우저 닫기

**POST** `/api/beings/{id}/browser/close`

### URL로 탐색

**POST** `/api/beings/{id}/browser/navigate`

**요청**:
```json
{
  "url": "https://example.com"
}
```

### JavaScript 실행

**POST** `/api/beings/{id}/browser/execute-script`

**요청**:
```json
{
  "script": "return document.title;"
}
```

### 페이지 스크린샷 가져오기

**GET** `/api/beings/{id}/browser/screenshot`

---

## 프로젝트 작업 영역 API

### 프로젝트 목록 가져오기

**GET** `/api/projects`

**응답**:
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

### 프로젝트 만들기

**POST** `/api/projects`

**요청**:
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### 프로젝트 세부정보 가져오기

**GET** `/api/projects/{id}`

### 프로젝트 업데이트

**PUT** `/api/projects/{id}`

### 프로젝트 삭제

**DELETE** `/api/projects/{id}`

---

## 도구 시스템 API

### ITool 인터페이스

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    ToolDefinition Definition { get; }
    
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### ToolCall 구조

```csharp
public class ToolCall
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}
```

### ToolResult 구조

```csharp
public class ToolResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }
}
```

---

## 다음 단계

- 🚀 [빠른 시작 가이드](getting-started.md) 보기
- 🛠️ [개발 가이드](development-guide.md) 읽기
- 📚 [아키텍처 문서](architecture.md) 확인
- 🔒 [보안 모델](security.md) 알아보기
