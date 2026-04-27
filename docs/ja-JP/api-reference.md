# API リファレンス

> **バージョン: v0.1.0-alpha**

[English](api-reference.md) | [简体中文](docs/zh-CN/api-reference.md) | [繁體中文](docs/zh-HK/api-reference.md) | [Español](docs/es-ES/api-reference.md) | [日本語](docs/ja-JP/api-reference.md) | [한국어](docs/ko-KR/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [Čeština](docs/cs-CZ/api-reference.md)

## Web API エンドポイント

ベース URL：`http://localhost:8080`

### 認証

ほとんどのエンドポイントは、Web UI によって管理されるセッションクッキーを介した認証が必要です。

---

## シリコンビーイング管理

### すべての生命体を取得

**GET** `/api/beings`

**レスポンス**：
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

### 生命体を作成

**POST** `/api/beings`

**リクエスト**：
```json
{
  "name": "New Being",
  "soul": "# Personality\nYou are helpful..."
}
```

**レスポンス**：`201 Created`

### 生命体を開始

**POST** `/api/beings/{id}/start`

### 生命体を停止

**POST** `/api/beings/{id}/stop`

### 生命体の詳細を取得

**GET** `/api/beings/{id}`

---

## チャットシステム

### メッセージを送信

**POST** `/api/chat`

**リクエスト**：
```json
{
  "beingId": "being-uuid",
  "message": "Hello, how are you?",
  "sessionId": "optional-session-id"
}
```

**レスポンス**（非ストリーミング）：
```json
{
  "reply": "I'm doing well, thank you!",
  "sessionId": "session-uuid",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### チャットをストリーム（SSE）

**GET** `/api/chat/stream?beingId={id}&message={msg}`

**レスポンス**：Server-Sent Events ストリーム

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### チャット履歴を取得

**GET** `/api/chat/{sessionId}/history`

**レスポンス**：
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

### 設定を取得

**GET** `/api/config`

**レスポンス**：
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

### 設定を更新

**POST** `/api/config`

**リクエスト**：
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

## 権限システム

### 権限を取得

**GET** `/api/permissions`

**レスポンス**：
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

### 権限を付与

**POST** `/api/permissions`

**リクエスト**：
```json
{
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "duration": 3600
}
```

### 権限を取り消し

**DELETE** `/api/permissions/{id}`

### 権限をチェック

**POST** `/api/permissions/check`

**リクエスト**：
```json
{
  "userId": "user-uuid",
  "resource": "network:http"
}
```

**レスポンス**：
```json
{
  "allowed": true,
  "reason": "Granted by curator"
}
```

---

## タスク＆タイマーシステム

### タスクを作成

**POST** `/api/tasks`

**リクエスト**：
```json
{
  "beingId": "being-uuid",
  "description": "Review code",
  "priority": 5,
  "dueDate": "2026-04-21T12:00:00Z"
}
```

### タスクを取得

**GET** `/api/tasks?beingId={id}&status=pending`

### タスクステータスを更新

**PATCH** `/api/tasks/{id}`

**リクエスト**：
```json
{
  "status": "completed"
}
```

### タイマーを作成

**POST** `/api/timers`

**リクエスト**：
```json
{
  "beingId": "being-uuid",
  "interval": 3600,
  "action": "think",
  "repeat": true
}
```

### タイマーを削除

**DELETE** `/api/timers/{id}`

---

## 監査＆ログ

### トークン使用を取得

**GET** `/api/audit/tokens?startDate={date}&endDate={date}`

**レスポンス**：
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

### ログを取得

**GET** `/api/logs?level=error&limit=100`

**レスポンス**：
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

## ストレージ API

### 値を読み取り

**GET** `/api/storage?key={key}`

**レスポンス**：
```json
{
  "key": "being:uuid:memory",
  "value": "{...}",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### 値を書き込み

**POST** `/api/storage`

**リクエスト**：
```json
{
  "key": "being:uuid:memory",
  "value": "{...}"
}
```

### 時間範囲でクエリ

**GET** `/api/storage/time?start={start}&end={end}&prefix={prefix}`

**レスポンス**：
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

## システム情報

### ダッシュボードデータを取得

**GET** `/api/dashboard`

**レスポンス**：
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

### システムステータスを取得

**GET** `/api/status`

**レスポンス**：
```json
{
  "version": "1.0.0",
  "runtime": ".NET 9.0",
  "uptime": 86400,
  "health": "healthy"
}
```

---

## エラーレスポンス

すべてのエンドポイントは標準化されたエラーレスポンスを返します：

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: disk:write, Current: disk:read"
  }
}
```

### 一般的なエラーコード

| コード | HTTP ステータス | 説明 |
|------|-------------|-------------|
| `PERMISSION_DENIED` | 403 | 権限不足 |
| `NOT_FOUND` | 404 | リソースが見つからない |
| `VALIDATION_ERROR` | 400 | リクエストパラメータが無効 |
| `INTERNAL_ERROR` | 500 | 内部サーバーエラー |
| `SERVICE_UNAVAILABLE` | 503 | AI サービスが利用不可 |

---

## SSE イベント

Server-Sent Events はリアルタイム更新に使用されます：

### チャットイベント

```javascript
const eventSource = new EventSource('/api/chat/stream?beingId=xxx&message=xxx');

eventSource.onmessage = (event) => {
  const data = JSON.parse(event.data);
  
  switch(data.type) {
    case 'chunk':
      console.log('ストリーミング：', data.content);
      break;
    case 'tool_call':
      console.log('ツール実行中：', data.tool);
      break;
    case 'complete':
      console.log('チャット完了、セッション：', data.sessionId);
      break;
    case 'error':
      console.error('エラー：', data.message);
      break;
  }
};
```

### 生命体ステータスイベント

```javascript
const beingEvents = new EventSource('/api/beings/events');

beingEvents.onmessage = (event) => {
  const data = JSON.parse(event.data);
  console.log(`生命体 ${data.beingId} ステータス：${data.status}`);
};
```

---

## AI クライアント API

### IAIClient インターフェース

```csharp
public interface IAIClient
{
    string Name { get; }
    
    Task<AIResponse> ChatAsync(AIRequest request);
    
    IAsyncEnumerable<string> StreamChatAsync(AIRequest request);
}
```

### AIRequest 構造体

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

### AIResponse 構造体

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

## ツールシステム API

### ITool インターフェース

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    ToolDefinition Definition { get; }
    
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### ToolCall 構造体

```csharp
public class ToolCall
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}
```

### ToolResult 構造体

```csharp
public class ToolResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }
}
```

---

## 作業ノート API

### 作業ノートリスト取得

**GET** `/api/beings/{id}/work-notes`

**レスポンス**:
```json
{
  "notes": [
    {
      "id": "note-uuid",
      "pageNumber": 1,
      "summary": "ユーザー認証モジュール完了",
      "keywords": ["認証", "JWT", "OAuth2"],
      "createdAt": "2026-04-25T10:00:00Z",
      "updatedAt": "2026-04-25T10:00:00Z"
    }
  ],
  "totalCount": 15
}
```

### 単一ノート詳細取得

**GET** `/api/beings/{id}/work-notes/{pageNumber}`

**レスポンス**:
```json
{
  "id": "note-uuid",
  "pageNumber": 1,
  "summary": "ユーザー認証モジュール完了",
  "content": "## 実装詳細\n\n- JWT token使用\n- OAuth2対応",
  "keywords": ["認証", "JWT", "OAuth2"],
  "createdAt": "2026-04-25T10:00:00Z",
  "updatedAt": "2026-04-25T10:00:00Z"
}
```

### 新規ノート作成

**POST** `/api/beings/{id}/work-notes`

**リクエスト**:
```json
{
  "summary": "ユーザー認証モジュール完了",
  "content": "## 実装詳細\n\n- JWT token使用",
  "keywords": "認証,JWT,OAuth2"
}
```

**レスポンス**: `201 Created`

### ノート更新

**PUT** `/api/beings/{id}/work-notes/{pageNumber}`

**リクエスト**:
```json
{
  "summary": "ユーザー認証モジュールおよびテスト完了",
  "content": "## 更新後の内容\n\nユニットテスト追加",
  "keywords": "認証,JWT,OAuth2,テスト"
}
```

### ノート削除

**DELETE** `/api/beings/{id}/work-notes/{pageNumber}`

### ノート検索

**GET** `/api/beings/{id}/work-notes/search?keyword=認証&maxResults=10`

### ノート目次取得

**GET** `/api/beings/{id}/work-notes/directory`

---

## ナレッジネットワーク API

### ナレッジ統計取得

**GET** `/api/knowledge/stats`

**レスポンス**:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### ナレッジトリプル追加

**POST** `/api/knowledge/triples`

**リクエスト**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95,
  "tags": ["programming", "language"]
}
```

**レスポンス**: `201 Created`

### ナレッジ照会

**GET** `/api/knowledge/query?subject=Python&predicate=is_a`

**レスポンス**:
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

### ナレッジ検索

**GET** `/api/knowledge/search?query=programming+language&limit=10`

### ナレッジパス取得

**GET** `/api/knowledge/path?from=Python&to=computer_science`

**レスポンス**:
```json
{
  "path": [
    {"subject": "Python", "predicate": "is_a", "object": "programming_language"},
    {"subject": "programming_language", "predicate": "belongs_to", "object": "computer_science"}
  ],
  "length": 2
}
```

### ナレッジ検証

**POST** `/api/knowledge/validate`

**リクエスト**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

### ナレッジ削除

**DELETE** `/api/knowledge/triples/{id}`

---

## プロジェクト管理 API

### プロジェクトリスト取得

**GET** `/api/projects`

**レスポンス**:
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

### プロジェクト作成

**POST** `/api/projects`

**リクエスト**:
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### プロジェクト詳細取得

**GET** `/api/projects/{id}`

### プロジェクト更新

**PUT** `/api/projects/{id}`

### プロジェクト削除

**DELETE** `/api/projects/{id}`

---

## 次のステップ

- 🚀 [はじめにガイド](getting-started.md)を確認
- 🛠️ [開発ガイド](development-guide.md)を読む
- 📚 [アーキテクチャドキュメント](architecture.md)を確認
- 🔒 [セキュリティモデル](security.md)を理解
