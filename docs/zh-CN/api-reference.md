# API 参考

> **版本：v0.1.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | **中文** | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md)

## Web API 端点

基础 URL：`http://localhost:8080`

### 认证

大多数端点需要通过 Web UI 管理的会话 cookie 进行认证。

---

## 硅基生命体管理

### 获取所有生命体

**GET** `/api/beings`

**响应**：
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

### 创建生命体

**POST** `/api/beings`

**请求**：
```json
{
  "name": "New Being",
  "soul": "# Personality\nYou are helpful..."
}
```

**响应**：`201 Created`

### 启动生命体

**POST** `/api/beings/{id}/start`

### 停止生命体

**POST** `/api/beings/{id}/stop`

### 获取生命体详情

**GET** `/api/beings/{id}`

---

## 聊天系统

### 发送消息

**POST** `/api/chat`

**请求**：
```json
{
  "beingId": "being-uuid",
  "message": "Hello, how are you?",
  "sessionId": "optional-session-id"
}
```

**响应**（非流式）：
```json
{
  "reply": "I'm doing well, thank you!",
  "sessionId": "session-uuid",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### 流式聊天（SSE）

**GET** `/api/chat/stream?beingId={id}&message={msg}`

**响应**：服务器发送事件流

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### 获取聊天历史

**GET** `/api/chat/{sessionId}/history`

**响应**：
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

## 配置

### 获取配置

**GET** `/api/config`

**响应**：
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

### 更新配置

**POST** `/api/config`

**请求**：
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

## 权限系统

### 获取权限

**GET** `/api/permissions`

**响应**：
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

### 授予权限

**POST** `/api/permissions`

**请求**：
```json
{
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "duration": 3600
}
```

### 撤销权限

**DELETE** `/api/permissions/{id}`

### 检查权限

**POST** `/api/permissions/check`

**请求**：
```json
{
  "userId": "user-uuid",
  "resource": "network:http"
}
```

**响应**：
```json
{
  "allowed": true,
  "reason": "Granted by curator"
}
```

---

## 任务和定时器系统

### 创建任务

**POST** `/api/tasks`

**请求**：
```json
{
  "beingId": "being-uuid",
  "description": "Review code",
  "priority": 5,
  "dueDate": "2026-04-21T12:00:00Z"
}
```

### 获取任务

**GET** `/api/tasks?beingId={id}&status=pending`

### 更新任务状态

**PATCH** `/api/tasks/{id}`

**请求**：
```json
{
  "status": "completed"
}
```

### 创建定时器

**POST** `/api/timers`

**请求**：
```json
{
  "beingId": "being-uuid",
  "interval": 3600,
  "action": "think",
  "repeat": true
}
```

### 删除定时器

**DELETE** `/api/timers/{id}`

---

## 审计和日志

### 获取 Token 使用

**GET** `/api/audit/tokens?startDate={date}&endDate={date}`

**响应**：
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

### 获取日志

**GET** `/api/logs?level=error&limit=100`

**响应**：
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

## 存储 API

### 读取值

**GET** `/api/storage?key={key}`

**响应**：
```json
{
  "key": "being:uuid:memory",
  "value": "{...}",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### 写入值

**POST** `/api/storage`

**请求**：
```json
{
  "key": "being:uuid:memory",
  "value": "{...}"
}
```

### 按时间范围查询

**GET** `/api/storage/time?start={start}&end={end}&prefix={prefix}`

**响应**：
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

## 系统信息

### 获取仪表板数据

**GET** `/api/dashboard`

**响应**：
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

### 获取系统状态

**GET** `/api/status`

**响应**：
```json
{
  "version": "1.0.0",
  "runtime": ".NET 9.0",
  "uptime": 86400,
  "health": "healthy"
}
```

---

## 错误响应

所有端点返回标准化的错误响应：

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: disk:write, Current: disk:read"
  }
}
```

### 常见错误代码

| 代码 | HTTP 状态 | 描述 |
|------|-------------|-------------|
| `PERMISSION_DENIED` | 403 | 权限不足 |
| `NOT_FOUND` | 404 | 资源未找到 |
| `VALIDATION_ERROR` | 400 | 请求参数无效 |
| `INTERNAL_ERROR` | 500 | 内部服务器错误 |
| `SERVICE_UNAVAILABLE` | 503 | AI 服务不可用 |

---

## SSE 事件

服务器发送事件用于实时更新：

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

### 生命体状态事件

```javascript
const beingEvents = new EventSource('/api/beings/events');

beingEvents.onmessage = (event) => {
  const data = JSON.parse(event.data);
  console.log(`Being ${data.beingId} status: ${data.status}`);
};
```

---

## AI 客户端 API

### IAIClient 接口

```csharp
public interface IAIClient
{
    string Name { get; }
    
    Task<AIResponse> ChatAsync(AIRequest request);
    
    IAsyncEnumerable<string> StreamChatAsync(AIRequest request);
}
```

### AIRequest 结构

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

### AIResponse 结构

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

## 工作笔记 API

### 获取工作笔记列表

**GET** `/api/beings/{id}/work-notes`

**响应**：
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

### 获取单条笔记详情

**GET** `/api/beings/{id}/work-notes/{pageNumber}`

**响应**：
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

### 创建新笔记

**POST** `/api/beings/{id}/work-notes`

**请求**：
```json
{
  "summary": "完成用户认证模块",
  "content": "## 实现细节\n\n- 使用 JWT token",
  "keywords": "认证,JWT,OAuth2"
}
```

**响应**：`201 Created`

### 更新笔记

**PUT** `/api/beings/{id}/work-notes/{pageNumber}`

**请求**：
```json
{
  "summary": "完成用户认证模块及测试",
  "content": "## 更新后的内容\n\n添加了单元测试",
  "keywords": "认证,JWT,OAuth2,测试"
}
```

### 删除笔记

**DELETE** `/api/beings/{id}/work-notes/{pageNumber}`

### 搜索笔记

**GET** `/api/beings/{id}/work-notes/search?keyword=认证&maxResults=10`

### 获取笔记目录

**GET** `/api/beings/{id}/work-notes/directory`

---

## 知识网络 API

### 获取知识统计

**GET** `/api/knowledge/stats`

**响应**：
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### 添加知识三元组

**POST** `/api/knowledge/triples`

**请求**：
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95,
  "tags": ["programming", "language"]
}
```

**响应**：`201 Created`

### 查询知识

**GET** `/api/knowledge/query?subject=Python&predicate=is_a`

**响应**：
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

### 搜索知识

**GET** `/api/knowledge/search?query=programming+language&limit=10`

### 获取知识路径

**GET** `/api/knowledge/path?from=Python&to=computer_science`

**响应**：
```json
{
  "path": [
    {"subject": "Python", "predicate": "is_a", "object": "programming_language"},
    {"subject": "programming_language", "predicate": "belongs_to", "object": "computer_science"}
  ],
  "length": 2
}
```

### 验证知识

**POST** `/api/knowledge/validate`

**请求**：
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

### 删除知识

**DELETE** `/api/knowledge/triples/{id}`

---

## 帮助文档系统 API

### 获取帮助文档列表

**GET** `/api/help`

**响应**：
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

### 获取帮助文档详情

**GET** `/api/help/{topicId}`

**响应**：
```json
{
  "id": "getting-started",
  "title": "快速开始",
  "content": "# 快速开始\n\n...",
  "category": "入门指南"
}
```

---

## WebView 浏览器 API

### 获取浏览器状态

**GET** `/api/beings/{id}/browser/status`

**响应**：
```json
{
  "is_open": true,
  "current_url": "https://example.com",
  "page_title": "Example Page",
  "is_loading": false,
  "last_operation_time": "2026-04-26T10:00:00Z"
}
```

### 打开浏览器

**POST** `/api/beings/{id}/browser/open`

### 关闭浏览器

**POST** `/api/beings/{id}/browser/close`

### 导航到 URL

**POST** `/api/beings/{id}/browser/navigate`

**请求**：
```json
{
  "url": "https://example.com"
}
```

### 执行 JavaScript

**POST** `/api/beings/{id}/browser/execute-script`

**请求**：
```json
{
  "script": "return document.title;"
}
```

### 获取页面截图

**GET** `/api/beings/{id}/browser/screenshot`

---

## 项目工作区 API

### 获取项目列表

**GET** `/api/projects`

**响应**：
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

### 创建项目

**POST** `/api/projects`

**请求**：
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### 获取项目详情

**GET** `/api/projects/{id}`

### 更新项目

**PUT** `/api/projects/{id}`

### 删除项目

**DELETE** `/api/projects/{id}`

---

## 工具系统 API

### ITool 接口

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    ToolDefinition Definition { get; }
    
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### ToolCall 结构

```csharp
public class ToolCall
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}
```

### ToolResult 结构

```csharp
public class ToolResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }
}
```

---

## 下一步

- 🚀 查看[快速开始指南](getting-started.md)
- 🛠️ 阅读[开发指南](development-guide.md)
- 📚 查看[架构文档](architecture.md)
- 🔒 了解[安全模型](security.md)
