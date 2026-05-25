# API 參考

> **版本：v0.2.0-alpha**

[English](../en/api-reference.md) | [中文](../zh-CN/api-reference.md) | **繁體中文** | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | [Русский](../ru-RU/api-reference.md)

## Web API 端點

基礎 URL：`http://localhost:8080`

### 認證

大多數端點需要透過 Web UI 管理的會話 cookie 進行認證。系統初始化前，除幫助頁面外的所有請求將重新導向至初始化頁面。

---

## 儀表板

### 取得儀表板統計資料

**GET** `/api/dashboard/stats`

返回系統概覽資料（生命體數量、運行狀態等）。

### 取得效能指標

**GET** `/api/dashboard/metrics`

返回即時效能指標資料。

---

## 聊天系統

### 聊天頁面

**GET** `/chat`

返回聊天介面頁面。

### 流式聊天（SSE）

**GET** `/api/chat/stream`

透過伺服器發送事件（SSE）進行流式聊天。

**回應**：伺服器發送事件流

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### 取得會話列表

**GET** `/api/chat/conversations`

返回所有活躍的聊天會話列表。

**回應範例**：
```json
{
  "conversations": [
    {
      "sessionId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
      "beingId": "being-uuid",
      "type": "single",
      "displayName": "與小游聊天",
      "lastMessage": "最後訊息內容",
      "lastTime": "2026-05-20T10:30:00Z"
    }
  ]
}
```

### 取得訊息歷史

**GET** `/api/chat/messages`

查詢參數：`channelId` — 頻道/會話 ID

返回指定會話的訊息歷史記錄。

### 取得聊天歷史

**GET** `/api/chat/history`

返回全域聊天歷史記錄。

### 發送訊息

**POST** `/api/chat/send`

**請求體**：
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
  "content": "測試訊息內容"
}
```

**回應**：
```json
{
  "success": true,
  "messageId": "50156b26-f3b9-4735-be3d-51e547bd3a4a"
}
```

### 停止 AI 思考

**POST** `/api/chat/stop`

停止目前正在進行的 AI 回應生成。

**請求體**：
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d"
}
```

### 上傳檔案

**POST** `/api/chat/upload`

上傳檔案到聊天會話中（支援 multipart/form-data）。

---

## 矽基生命體管理

### 生命體管理頁面

**GET** `/beings`

返回矽基生命體管理介面頁面。

### 取得生命體列表

**GET** `/api/beings` 或 **GET** `/api/beings/list`

返回所有已註冊的矽基生命體列表。

**回應範例**：
```json
{
  "beings": [
    {
      "id": "being-uuid",
      "name": "Assistant",
      "status": "running",
      "soulPath": "path/to/soul.md"
    }
  ]
}
```

**狀態值**：`idle` | `running` | `waiting_permission` | `stopped`

### 取得生命體詳情

**GET** `/api/beings/detail`

查詢參數：`beingId` — 生命體 ID

返回指定生命體的詳細資訊。

### 取得生命體活動狀態

**GET** `/api/beings/activity`

返回各生命體的活動狀態資訊。

### 靈魂檔案編輯器頁面

**GET** `/beings/soul`

返回靈魂檔案編輯器介面。

### 儲存靈魂檔案

**POST** `/api/beings/soul/save`

**請求體**：
```json
{
  "beingId": "being-uuid",
  "soulContent": "# Personality\nYou are helpful..."
}
```

### AI 配置編輯器頁面

**GET** `/beings/ai-config`

返回 AI 配置編輯器介面。

### 儲存 AI 配置

**POST** `/api/beings/ai-config/save`

**請求體**：
```json
{
  "beingId": "being-uuid",
  "aiClientType": "DashScope",
  "config": {
    "apiKey": "...",
    "region": "beijing",
    "model": "qwen3.6-plus"
  }
}
```

### 取得可用 AI 模型列表

**GET** `/api/beings/ai-config/models`

查詢參數：`clientType`, `apiKey`, `region`

返回指定 AI 客戶端的可用模型列表。

---

## 聊天歷史檢視

### 聊天歷史頁面

**GET** `/chat-history`

返回聊天歷史主頁面。

### 聊天歷史詳情頁面

**GET** `/chat-history-detail`

返回指定會話的聊天歷史詳情頁面。

### 群聊歷史詳情頁面

**GET** `/group-chat-history-detail`

返回群聊的歷史詳情頁面。

### 廣播歷史詳情頁面

**GET** `/broadcast-history-detail`

返回廣播頻道的歷史詳情頁面。

### 取得歷史會話列表

**GET** `/api/chat-history/conversations`

返回所有歷史會話列表。

### 取得歷史訊息

**GET** `/api/chat-history/messages`

查詢參數：`sessionId` — 會話 ID

返回指定歷史會話的訊息記錄。

---

## 定時器管理

### 定時器頁面

**GET** `/timers`

返回定時器管理介面頁面。

### 取得定時器列表

**GET** `/api/timers/list`

返回所有定時器的列表。

### 定時器週期詳情頁面

**GET** `/timer-cycles/{timerId}`

返回指定定時器的執行週期詳情頁面。

### 取得定時器週期列表

**GET** `/api/timer-cycles/list`

查詢參數：`timerId` — 定時器 ID

返回指定定時器的所有執行週期列表。

### 單次執行週期詳情頁面

**GET** `/timer-cycle/{cycleIndex}`

返回單次執行的詳細頁面。

### 取得週期訊息

**GET** `/api/timer-cycle/messages`

查詢參數：`cycleIndex` — 週期索引

返回指定執行週期的相關訊息。

---

## 任務管理

### 任務頁面

**GET** `/tasks`

返回任務管理介面頁面。

### 取得任務列表

**GET** `/api/tasks/list`

返回所有任務的列表。

### 任務週期詳情頁面

**GET** `/task-cycles/{taskId}`

返回指定任務的執行週期詳情頁面。

### 取得任務週期列表

**GET** `/api/task-cycles/list`

查詢參數：`taskId` — 任務 ID

返回指定任務的所有執行週期列表。

### 單次執行週期詳情頁面

**GET** `/task-cycle/{cycleIndex}`

返回單次任務執行的詳細頁面。

### 取得週期訊息

**GET** `/api/task-cycle/messages`

查詢參數：`cycleIndex` — 週期索引

返回指定任務執行週期的相關訊息。

---

## 權限系統

### 權限管理頁面

**GET** `/permissions`

返回權限管理介面頁面。

### 取得權限規則列表

**GET** `/api/permissions/list`

返回目前配置的所有權限規則。

**回應範例**：
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

### 儲存權限規則

**POST** `/api/permissions/save`

**請求體**：
```json
{
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "duration": 3600
}
```

### 權限請求頁面

**GET** `/permission/request`

顯示權限請求頁面，允許使用者批准或拒絕矽基生命體的權限請求。

**查詢參數**：

| 參數 | 類型 | 描述 |
|------|------|------|
| `userId` | `Guid` | 請求權限的矽基生命體 ID |
| `type` | `string` | 權限類型 |
| `resource` | `string` | 請求的資源路徑 |
| `allowCode` | `string` | 允許操作的代碼標識 |
| `denyCode` | `string` | 拒絕操作的代碼標識 |

### 檢查待處理權限請求

**GET** `/permission/check`

查詢參數：`userId` — 矽基生命體 ID

**回應**：
```json
{
  "pending": true
}
```

### 回應權限請求

**GET** `/permission/respond`

**查詢參數**：

| 參數 | 類型 | 描述 |
|------|------|------|
| `userId` | `Guid` | 矽基生命體 ID |
| `allowed` | `bool` | 是否允許 |
| `addToCache` | `bool` | 是否將決策快取 |
| `cacheDuration` | `double` | 快取持續時間（小時） |

**回應**：
```json
{
  "success": true
}
```

---

## 日誌系統

### 日誌頁面

**GET** `/logs`

返回日誌檢視介面頁面。

### 取得日誌列表

**GET** `/api/logs/list`

查詢參數支援按級別、時間範圍過濾。

**回應範例**：
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

### 取得日誌按生命體分組

**GET** `/api/logs/beings`

按矽基生命體分組的日誌統計。

### 取得可用日誌級別

**GET** `/api/logs/levels`

返回系統中可用的日誌級別列表。

---

## 使用統計

### 使用統計頁面

**GET** `/usage`

返回使用統計介面頁面。

### 取得使用摘要

**GET** `/api/usage/summary`

返回 Token 使用量和費用摘要。

### 取得趨勢資料

**GET** `/api/usage/trend`

查詢參數：`startDate`, `endDate`

返回指定時間段內的使用趨勢資料。

### 匯出使用資料

**GET** `/api/usage/export`

匯出使用資料為可下載格式。

---

## 稽核追蹤

### 稽核頁面

**GET** `/audit`

返回稽核追蹤介面頁面。

### 取得稽核列表

**GET** `/api/audit/list`

返回稽核日誌條目列表。

### 取得稽核摘要

**GET** `/api/audit/summary`

返回稽核資料的彙總統計。

### 取得稽核按生命體分組

**GET** `/api/audit/beings`

按矽基生命體分組的稽核統計。

---

## 配置管理

### 配置頁面

**GET** `/config`

返回系統配置介面頁面。

### 儲存配置

**POST** `/config/save`

**請求體**：
```json
{
  "language": "ZhCN",
  "port": 8080,
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:7b"
    },
    "DashScope": {
      "apiKey": "...",
      "region": "beijing",
      "model": "qwen3.6-plus"
    }
  }
}
```

### 取得 AI 配置選項

**GET** `/config/aioptions`

返回可用的 AI 客戶端類型及其動態選項（可用模型、區域等）。

---

## 記憶系統

### 記憶頁面

**GET** `/memory`

返回記憶管理介面頁面。

### 取得記憶列表

**GET** `/api/memory/list`

返回矽基生命體的記憶條目列表。

### 取得記憶詳情

**GET** `/api/memory/detail/{id}`

路徑參數：`id` — 記憶條目 ID

返回指定記憶條目的完整內容。

### 取得記憶統計

**GET** `/api/memory/stats`

返回記憶系統的統計資訊。

### 搜尋記憶

**GET** `/api/memory/search`

查詢參數：`keyword` — 搜尋關鍵詞

搜尋匹配的記憶條目。

### 取得記憶按生命體分組

**GET** `/api/memory/beings`

按矽基生命體分組的記憶統計。

### 取得記憶追溯

**GET** `/api/memory/trace/{id}`

路徑參數：`id` — 記憶條目 ID

返回指定記憶條目的來源追溯鏈。

### 取得記憶時間線 HTML

**GET** `/api/memory/timeline-html`

返回記憶時間線的 HTML 檢視。

---

## 工作筆記

### 工作筆記頁面

**GET** `/work-notes`

返回工作筆記介面頁面。

### 取得工作筆記列表

**GET** `/api/work-notes/list`

返回工作筆記列表。

### 讀取工作筆記

**GET** `/api/work-notes/read`

查詢參數：`noteId` — 筆記 ID

返回指定筆記的內容。

### 取得筆記目錄

**GET** `/api/work-notes/directory`

返回筆記目錄結構。

### 搜尋工作筆記

**GET** `/api/work-notes/search`

查詢參數：`keyword` — 搜尋關鍵詞

搜尋匹配的工作筆記。

### 建立工作筆記

**POST** `/api/work-notes/create`

**請求體**：
```json
{
  "title": "筆記標題",
  "content": "筆記內容",
  "keywords": ["關鍵詞1", "關鍵詞2"]
}
```

### 更新工作筆記

**POST** `/api/work-notes/update`

**請求體**：
```json
{
  "noteId": "note-uuid",
  "title": "更新後的標題",
  "content": "更新後的內容"
}
```

### 刪除工作筆記

**POST** `/api/work-notes/delete`

**請求體**：
```json
{
  "noteId": "note-uuid"
}
```

---

## 知識網絡

### 知識網絡頁面

**GET** `/knowledge`

返回知識網絡管理介面頁面。

### 取得知識圖譜

**GET** `/api/knowledge/graph`

返回知識三元組圖譜資料（主體-關係-客體）。

---

## 專案管理

### 專案頁面

**GET** `/project`

返回專案管理介面頁面。

### 專案工作筆記頁面

**GET** `/project/{id}/work-notes`

路徑參數：`id` — 專案 ID

返回指定專案的工作筆記頁面。

### 專案任務頁面

**GET** `/project/{id}/tasks`

路徑參數：`id` — 專案 ID

返回指定專案的任務管理頁面。

### 取得專案列表

**GET** `/api/projects/list`

返回所有專案的列表。

### 取得專案工作流範本列表

**GET** `/api/projects/list-workflow-templates`

返回可用的工作流範本列表。

### 建立專案

**POST** `/api/projects/create`

**請求體**：
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### 歸檔專案

**POST** `/api/projects/{id}/archive`

路徑參數：`id` — 專案 ID

歸檔指定專案。

### 還原專案

**POST** `/api/projects/{id}/restore`

路徑參數：`id` — 專案 ID

還原已歸檔的專案。

### 銷毀專案

**POST** `/api/projects/{id}/destroy`

路徑參數：`id` — 專案 ID

永久刪除指定專案（不可還原）。

### 取得專案詳情

**GET** `/api/projects/detail`

查詢參數：`projectId` — 專案 ID

返回專案的詳細資訊。

### 更新專案

**POST** `/api/projects/update`

**請求體**：
```json
{
  "projectId": "project-uuid",
  "name": "Updated Name",
  "description": "Updated description"
}
```

### 分配成員到專案

**POST** `/api/projects/assign`

**請求體**：
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### 從專案中移除成員

**POST** `/api/projects/remove`

**請求體**：
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### 取得專案工作筆記列表

**GET** `/api/projects/{id}/work-notes/list`

路徑參數：`id` — 專案 ID

返回指定專案的工作筆記列表。

### 讀取專案工作筆記

**GET** `/api/projects/{id}/work-notes/read`

路徑參數：`id` — 專案 ID

返回指定專案的工作筆記內容。

### 建立專案工作筆記

**POST** `/api/projects/{id}/work-notes/create`

路徑參數：`id` — 專案 ID

在指定專案中建立新的工作筆記。

### 更新專案工作筆記

**POST** `/api/projects/{id}/work-notes/update`

路徑參數：`id` — 專案 ID

更新指定專案中的工作筆記。

### 刪除專案工作筆記

**POST** `/api/projects/{id}/work-notes/delete`

路徑參數：`id` — 專案 ID

刪除指定專案中的工作筆記。

### 取得專案任務列表

**GET** `/api/projects/{id}/tasks/list`

路徑參數：`id` — 專案 ID

返回指定專案的任務列表。

### 建立專案任務

**POST** `/api/projects/{id}/tasks/create`

路徑參數：`id` — 專案 ID

在指定專案中建立新任務。

### 更新專案任務

**POST** `/api/projects/{id}/tasks/update`

路徑參數：`id` — 專案 ID

更新指定專案中的任務。

### 刪除專案任務

**POST** `/api/projects/{id}/tasks/delete`

路徑參數：`id` — 專案 ID

刪除指定專案中的任務。

### 分配任務負責人

**POST** `/api/projects/{id}/tasks/assign`

路徑參數：`id` — 專案 ID

為專案任務分配負責人。

### 移除任務負責人

**POST** `/api/projects/{id}/tasks/remove-assignee`

路徑參數：`id` — 專案 ID

移除專案任務的負責人。

### 標記任務完成

**POST** `/api/projects/{id}/tasks/complete`

路徑參數：`id` — 專案 ID

標記專案任務為已完成。

### 標記任務失敗

**POST** `/api/projects/{id}/tasks/fail`

路徑參數：`id` — 專案 ID

標記專案任務為失敗。

### 取消任務

**POST** `/api/projects/{id}/tasks/cancel`

路徑參數：`id` — 專案 ID

取消專案任務。

---

## 執行器管理

### 執行器頁面

**GET** `/executor`

返回執行器管理介面頁面。

### 取得執行器狀態

**GET** `/api/executors/status`

返回各執行器（磁碟、網絡、命令列）的運行狀態。

---

## 程式碼瀏覽器

### 程式碼瀏覽器頁面

**GET** `/code`

返回程式碼瀏覽器介面頁面。

### 取得程式碼類型列表

**GET** `/api/code/types`

返回支援的程式碼類型/語言列表。

### 取得程式碼詳情

**GET** `/api/code/detail`

查詢參數：`filePath`, `lineNumber`

返回指定檔案的程式碼詳情。

---

## 程式碼懸浮提示

### 取得懸浮提示

**GET** `/api/code/hover`
**POST** `/api/code/hover`

取得程式碼位置的懸浮提示資訊（類似 IDE 的智能提示）。

### 註冊程式碼位置

**POST** `/api/code/register`

註冊需要監控的程式碼位置。

### 更新程式碼位置

**POST** `/api/code/update`

更新已註冊的程式碼位置資訊。

### 註銷程式碼位置

**POST** `/api/code/unregister`

註銷不再需要的程式碼位置監控。

---

## 幫助文件系統

### 幫助頁面

**GET** `/help` 或 **GET** `/help/index`

返回幫助文件主頁。

### 幫助主題頁面

**GET** `/help/{topic}`

路徑參數：`topic` — 主題識別符

返回指定主題的幫助文件頁面。

### 搜尋幫助文件

**GET** `/api/help/search`

查詢參數：`keyword` — 搜尋關鍵詞

搜尋匹配的幫助文件主題。

---

## 初始化

### 初始化精靈頁面

**GET** `/init`

返回首次運行初始化精靈頁面。

### 提交初始化

**POST** `/init`

提交首次運行的初始化配置。

### 瀏覽選擇資料目錄

**GET** `/init/browse`

開啟目錄瀏覽器以選擇資料儲存位置。

### 取得 AI 配置元資料

**GET** `/init/ai-config-metadata`

返回可用的 AI 客戶端類型及其配置欄位元資料。

---

## 系統控制

### 優雅關閉

**POST** `/api/system/shutdown`

> **注意**：僅允許來自 localhost 的請求

觸發應用程式的優雅關閉流程：

1. 停止主迴圈（MainLoop）
2. 儲存目前配置
3. 關閉 HTTP 監聽器

**回應**：
```json
{
  "status": "shutting_down",
  "message": "Application is shutting down gracefully"
}
```

---

## 關於

### 關於頁面

**GET** `/about`

返回關於頁面，包含系統資訊和已載入的外掛程式列表。

**外掛程式列表資料**：
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

---

## 錯誤回應

所有端點返回標準化的錯誤回應：

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: disk:write, Current: disk:read"
  }
}
```

### 常見錯誤代碼

| 代碼 | HTTP 狀態 | 描述 |
|------|-------------|-------------|
| `PERMISSION_DENIED` | 403 | 權限不足 |
| `NOT_FOUND` | 404 | 資源未找到 |
| `VALIDATION_ERROR` | 400 | 請求參數無效 |
| `INTERNAL_ERROR` | 500 | 內部伺服器錯誤 |
| `SERVICE_UNAVAILABLE` | 503 | AI 服務不可用 |

---

## SSE 事件

伺服器發送事件用於即時更新：

### 聊天事件

```javascript
const eventSource = new EventSource('/api/chat/stream');

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

---

## AI 客戶端介面

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

## 工具系統介面

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

## 下一步

- 🚀 查看[快速開始指南](getting-started.md)
- 🛠️ 閱讀[開發指南](development-guide.md)
- 📚 查看[架構文件](architecture.md)
- 🔒 了解[安全模型](security.md)
