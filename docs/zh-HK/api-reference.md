# API 參考

[English](api-reference.md) | [简体中文](docs/zh-CN/api-reference.md) | [繁體中文](docs/zh-HK/api-reference.md) | [Español](docs/es-ES/api-reference.md) | [日本語](docs/ja-JP/api-reference.md) | [한국어](docs/ko-KR/api-reference.md) | [Čeština](docs/cs-CZ/api-reference.md)

## Web API 端点

基础 URL：`http://localhost:8080`

### 認證

大多数端点需要通過 Web UI 管理的會话 cookie 進行認證。

---

## 硅基生命体管理

### 获取所有生命体

**GET** `/api/beings`

**回應**：
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

### 建立生命体

**POST** `/api/beings`

**要求**：
```json
{
  "name": "New Being",
  "soul": "# Personality\nYou are helpful..."
}
```

**回應**：`201 Created`

### 啟動生命体

**POST** `/api/beings/{id}/start`

### 停止生命体

**POST** `/api/beings/{id}/stop`

### 获取生命体详情

**GET** `/api/beings/{id}`

---

## 聊天系統

### 發送訊息

**POST** `/api/chat`

**要求**：
```json
{
  "beingId": "being-uuid",
  "message": "Hello, how are you?",
  "sessionId": "optional-session-id"
}
```

**回應**（非流式）：
```json
{
  "reply": "I'm doing well, thank you!",
  "sessionId": "session-uuid",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### 流式聊天（SSE）

**GET** `/api/chat/stream?beingId={id}&message={msg}`

**回應**：伺服器發送事件流

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### 获取聊天歷史

**GET** `/api/chat/{sessionId}/history`

**回應**：
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

## 設定

### 获取設定

**GET** `/api/config`

**回應**：
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

### 更新設定

**POST** `/api/config`

**要求**：
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

## 權限系統

### 获取權限

**GET** `/api/permissions`

**回應**：
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

### 授予權限

**POST** `/api/permissions`

**要求**：
```json
{
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "duration": 3600
}
```

### 撤销權限

**DELETE** `/api/permissions/{id}`

### 檢查權限

**POST** `/api/permissions/check`

**要求**：
```json
{
  "userId": "user-uuid",
  "resource": "network:http"
}
```

**回應**：
```json
{
  "allowed": true,
  "reason": "Granted by curator"
}
```

---

## 工作和定时器系統

### 建立工作

**POST** `/api/tasks`

**要求**：
```json
{
  "beingId": "being-uuid",
  "description": "Review code",
  "priority": 5,
  "dueDate": "2026-04-21T12:00:00Z"
}
```

### 获取工作

**GET** `/api/tasks?beingId={id}&status=pending`

### 更新工作狀態

**PATCH** `/api/tasks/{id}`

**要求**：
```json
{
  "status": "completed"
}
```

### 建立定时器

**POST** `/api/timers`

**要求**：
```json
{
  "beingId": "being-uuid",
  "interval": 3600,
  "action": "think",
  "repeat": true
}
```

### 刪除定时器

**DELETE** `/api/timers/{id}`

---

## 稽核和記錄

### 获取 Token 使用

**GET** `/api/audit/tokens?startDate={date}&endDate={date}`

**回應**：
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

### 获取記錄

**GET** `/api/logs?level=error&limit=100`

**回應**：
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

## 儲存 API

### 读取值

**GET** `/api/storage?key={key}`

**回應**：
```json
{
  "key": "being:uuid:memory",
  "value": "{...}",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### 寫入值

**POST** `/api/storage`

**要求**：
```json
{
  "key": "being:uuid:memory",
  "value": "{...}"
}
```

### 按時間范围查詢

**GET** `/api/storage/time?start={start}&end={end}&prefix={prefix}`

**回應**：
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

## 系統資訊

### 获取儀表板資料

**GET** `/api/dashboard`

**回應**：
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

### 获取系統狀態

**GET** `/api/status`

**回應**：
```json
{
  "version": "1.0.0",
  "runtime": ".NET 9.0",
  "uptime": 86400,
  "health": "healthy"
}
```

---

## 錯誤回應

所有端点返回標準化的錯誤回應：

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: disk:write, Current: disk:read"
  }
}
```

### 常見錯誤程式碼

| 程式碼 | HTTP 狀態 | 描述 |
|------|-------------|-------------|
| `PERMISSION_DENIED` | 403 | 權限不足 |
| `NOT_FOUND` | 404 | 資源未找到 |
| `VALIDATION_ERROR` | 400 | 要求參數无效 |
| `INTERNAL_ERROR` | 500 | 內部伺服器錯誤 |
| `SERVICE_UNAVAILABLE` | 503 | AI 服務不可用 |

---

## SSE 事件

伺服器發送事件用於实时更新：

### 聊天事件

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

### 生命体狀態事件

```javascript
const beingEvents = new EventSource('/api/beings/events');

beingEvents.onmessage = (event) => {
  const data = JSON.parse(event.data);
  console.log(`Being ${data.beingId} status: ${data.status}`);
};
```

---

## AI 客戶端 API

### IAIClient 介面

```csharp
public interface IAIClient
{
    string Name { get; }
    
    Task<AIResponse> ChatAsync(AIRequest request);
    
    IAsyncEnumerable<string> StreamChatAsync(AIRequest request);
}
```

### AIRequest 結構

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

### AIResponse 結構

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

## 工具系統 API

### ITool 介面

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    ToolDefinition Definition { get; }
    
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### ToolCall 結構

```csharp
public class ToolCall
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}
```

### ToolResult 結構

```csharp
public class ToolResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }
}
```

---

## 工作筆記 API

### 獲取工作筆記列表

**GET** `/api/beings/{id}/work-notes`

**響應**：
```json
{
  "notes": [
    {
      "id": "note-uuid",
      "pageNumber": 1,
      "summary": "完成用戶認證模組",
      "keywords": ["認證", "JWT", "OAuth2"],
      "createdAt": "2026-04-25T10:00:00Z",
      "updatedAt": "2026-04-25T10:00:00Z"
    }
  ],
  "totalCount": 15
}
```

### 獲取單條筆記詳情

**GET** `/api/beings/{id}/work-notes/{pageNumber}`

**響應**：
```json
{
  "id": "note-uuid",
  "pageNumber": 1,
  "summary": "完成用戶認證模組",
  "content": "## 實現細節\n\n- 使用 JWT token\n- 支持 OAuth2",
  "keywords": ["認證", "JWT", "OAuth2"],
  "createdAt": "2026-04-25T10:00:00Z",
  "updatedAt": "2026-04-25T10:00:00Z"
}
```

### 創建新筆記

**POST** `/api/beings/{id}/work-notes`

**請求**：
```json
{
  "summary": "完成用戶認證模組",
  "content": "## 實現細節\n\n- 使用 JWT token",
  "keywords": "認證,JWT,OAuth2"
}
```

**響應**：`201 Created`

### 更新筆記

**PUT** `/api/beings/{id}/work-notes/{pageNumber}`

**請求**：
```json
{
  "summary": "完成用戶認證模組及測試",
  "content": "## 更新後的內容\n\n添加了單元測試",
  "keywords": "認證,JWT,OAuth2,測試"
}
```

### 刪除筆記

**DELETE** `/api/beings/{id}/work-notes/{pageNumber}`

### 搜索筆記

**GET** `/api/beings/{id}/work-notes/search?keyword=認證&maxResults=10`

### 獲取筆記目錄

**GET** `/api/beings/{id}/work-notes/directory`

---

## 知識網絡 API

### 獲取知識統計

**GET** `/api/knowledge/stats`

**響應**：
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### 添加知識三元組

**POST** `/api/knowledge/triples`

**請求**：
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95,
  "tags": ["programming", "language"]
}
```

**響應**：`201 Created`

### 查詢知識

**GET** `/api/knowledge/query?subject=Python&predicate=is_a`

**響應**：
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

### 搜索知識

**GET** `/api/knowledge/search?query=programming+language&limit=10`

### 獲取知識路徑

**GET** `/api/knowledge/path?from=Python&to=computer_science`

**響應**：
```json
{
  "path": [
    {"subject": "Python", "predicate": "is_a", "object": "programming_language"},
    {"subject": "programming_language", "predicate": "belongs_to", "object": "computer_science"}
  ],
  "length": 2
}
```

### 驗證知識

**POST** `/api/knowledge/validate`

**請求**：
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

### 刪除知識

**DELETE** `/api/knowledge/triples/{id}`

---

## 專案管理 API

### 獲取專案列表

**GET** `/api/projects`

**響應**：
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

### 創建專案

**POST** `/api/projects`

**請求**：
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### 獲取專案詳情

**GET** `/api/projects/{id}`

### 更新專案

**PUT** `/api/projects/{id}`

### 刪除專案

**DELETE** `/api/projects/{id}`

---

## 下一步

- 🚀 查看[快速開始指南](getting-started.md)
- 🛠️ 阅读[開發指南](development-guide.md)
- 📚 查看[架構文檔](architecture.md)
- 🔒 了解[安全模型](security.md)
