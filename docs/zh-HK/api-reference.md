# API 參考

> **版本：v0.2.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [中文](../zh-CN/api-reference.md) | **繁體中文** | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | [Русский](../ru-RU/api-reference.md)

## Web API 端點

基礎 URL：`http://localhost:8080`

### 認證

大多數端點需要透過 Web UI 管理的會話 Cookie 進行認證。系統初始化前，除說明頁面外的所有請求將重新導向到初始化頁面。

---

## 儀表板

### 取得儀表板統計資料

**GET** `/api/dashboard/stats`

傳回系統概覽資料（生命體數量、執行狀態等）。

### 取得效能指標

**GET** `/api/dashboard/metrics`

傳回即時效能指標資料。

---

## 聊天系統

### 聊天頁面

**GET** `/chat`

傳回聊天介面頁面。

### 串流聊天（SSE）

**GET** `/api/chat/stream`

透過伺服器傳送事件（SSE）進行串流聊天。

**回應**：伺服器傳送事件流

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### 取得會話清單

**GET** `/api/chat/conversations`

傳回所有活躍的聊天會話清單。

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

傳回指定會話的訊息歷史記錄。

### 取得聊天歷史

**GET** `/api/chat/history`

傳回全域聊天歷史記錄。

### 傳送訊息

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

停止目前正在進行的 AI 回應產生。

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

傳回矽基生命體管理介面頁面。

### 取得生命體清單

**GET** `/api/beings` 或 **GET** `/api/beings/list`

傳回所有已註冊的矽基生命體清單。

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

### 取得生命體詳細資料

**GET** `/api/beings/detail`

查詢參數：`beingId` — 生命體 ID

傳回指定生命體的詳細資料。

### 取得生命體活動狀態

**GET** `/api/beings/activity`

傳回各生命體的活動狀態資訊。

### 靈魂檔案編輯器頁面

**GET** `/beings/soul`

傳回靈魂檔案編輯器介面。

### 儲存靈魂檔案

**POST** `/api/beings/soul/save`

**請求體**：
```json
{
  "beingId": "being-uuid",
  "soulContent": "# Personality\nYou are helpful..."
}
```

### AI 設定編輯器頁面

**GET** `/beings/ai-config`

傳回 AI 設定編輯器介面。

### 儲存 AI 設定

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

### 取得可用 AI 模型清單

**GET** `/api/beings/ai-config/models`

查詢參數：`clientType`, `apiKey`, `region`

傳回指定 AI 用戶端的可用模型清單。

---

## 聊天歷史檢視

### 聊天歷史頁面

**GET** `/chat-history`

傳回聊天歷史主頁面。

### 聊天歷史詳細資料頁面

**GET** `/chat-history-detail`

傳回指定會話的聊天歷史詳細資料頁面。

### 群聊歷史詳細資料頁面

**GET** `/group-chat-history-detail`

傳回群聊的歷史詳細資料頁面。

### 廣播歷史詳細資料頁面

**GET** `/broadcast-history-detail`

傳回廣播頻道的歷史詳細資料頁面。

### 取得歷史會話清單

**GET** `/api/chat-history/conversations`

傳回所有歷史會話清單。

### 取得歷史訊息

**GET** `/api/chat-history/messages`

查詢參數：`sessionId` — 會話 ID

傳回指定歷史會話的訊息記錄。

---

## 定時器管理

### 定時器頁面

**GET** `/timers`

傳回定時器管理介面頁面。

### 取得定時器清單

**GET** `/api/timers/list`

傳回所有定時器的清單。

### 定時器週期詳細資料頁面

**GET** `/timer-cycles/{timerId}`

傳回指定定時器的執行週期詳細資料頁面。

### 取得定時器週期清單

**GET** `/api/timer-cycles/list`

查詢參數：`timerId` — 定時器 ID

傳回指定定時器的所有執行週期清單。

### 單次執行週期詳細資料頁面

**GET** `/timer-cycle/{cycleIndex}`

傳回單次執行的詳細頁面。

### 取得週期訊息

**GET** `/api/timer-cycle/messages`

查詢參數：`cycleIndex` — 週期索引

傳回指定執行週期的相關訊息。

---

## 任務管理

### 任務頁面

**GET** `/tasks`

傳回任務管理介面頁面。

### 取得任務清單

**GET** `/api/tasks/list`

傳回所有任務的清單。

### 任務週期詳細資料頁面

**GET** `/task-cycles/{taskId}`

傳回指定任務的執行週期詳細資料頁面。

### 取得任務週期清單

**GET** `/api/task-cycles/list`

查詢參數：`taskId` — 任務 ID

傳回指定任務的所有執行週期清單。

### 單次執行週期詳細資料頁面

**GET** `/task-cycle/{cycleIndex}`

傳回單次任務執行的詳細頁面。

### 取得週期訊息

**GET** `/api/task-cycle/messages`

查詢參數：`cycleIndex` — 週期索引

傳回指定任務執行週期的相關訊息。

---

## 權限系統

### 權限管理頁面

**GET** `/permissions`

傳回權限管理介面頁面。

### 取得權限規則清單

**GET** `/api/permissions/list`

傳回目前設定的所有權限規則。

**回應範例**：
```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed",
      "description": "Allow GitHub API access"
    }
  ]
}
```

### 儲存權限規則

**POST** `/api/permissions/save`

**請求體**：
```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects",
  "result": "Allowed",
  "description": "Allow project directory access"
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
| `allowCode` | `string` | 允許操作的程式碼識別 |
| `denyCode` | `string` | 拒絕操作的程式碼識別 |

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

傳回日誌檢視介面頁面。

### 取得日誌清單

**GET** `/api/logs/list`

查詢參數支援按層級、時間範圍過濾。

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

### 取得可用日誌層級

**GET** `/api/logs/levels`

傳回系統中可用的日誌層級清單。

---

## 使用統計

### 使用統計頁面

**GET** `/usage`

傳回使用統計介面頁面。

### 取得使用摘要

**GET** `/api/usage/summary`

傳回 Token 使用量和費用摘要。

### 取得趨勢資料

**GET** `/api/usage/trend`

查詢參數：`startDate`, `endDate`

傳回指定時間範圍內的使用趨勢資料。

### 匯出使用資料

**GET** `/api/usage/export`

匯出使用資料為可下載格式。

---

## 稽核追蹤

### 稽核頁面

**GET** `/audit`

傳回稽核追蹤介面頁面。

### 取得稽核清單

**GET** `/api/audit/list`

傳回稽核日誌條目清單。

### 取得稽核摘要

**GET** `/api/audit/summary`

傳回稽核資料的彙總統計。

### 取得稽核按生命體分組

**GET** `/api/audit/beings`

按矽基生命體分組的稽核統計。

---

## 設定管理

### 設定頁面

**GET** `/config`

傳回系統設定介面頁面。

### 儲存設定

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
    },
    "VolcengineArk": {
      "apiKey": "...",
      "endpoint": "...",
      "model": "..."
    },
    "Herdsman": {
      "endpoint": "http://localhost:8000",
      "model": "..."
    },
    "LongCat": {
      "apiKey": "...",
      "endpoint": "https://api.longcat.chat/openai",
      "model": "LongCat-2.0"
    },
    "QiniuAI": {
      "apiKey": "...",
      "endpoint": "...",
      "model": "..."
    },
    "DeepSeek": {
      "apiKey": "...",
      "model": "..."
    },
    "Zhipu": {
      "apiKey": "...",
      "model": "..."
    },
    "Ernie": {
      "apiKey": "...",
      "model": "..."
    },
    "Hunyuan": {
      "apiKey": "...",
      "model": "..."
    },
    "MiniMax": {
      "apiKey": "...",
      "model": "..."
    },
    "Moonshot": {
      "apiKey": "...",
      "model": "..."
    },
    "SiliconFlow": {
      "apiKey": "...",
      "model": "..."
    }
  },
  "imPlatforms": [
    {
      "platform": "webui",
      "enabled": true,
      "config": {}
    },
    {
      "platform": "feishu",
      "enabled": true,
      "config": {
        "appId": "cli_xxx",
        "appSecret": "${FEISHU_APP_SECRET}",
        "verificationToken": "..."
      }
    }
  ]
}
```

`imPlatforms` 為多實例清單：每個條目代表一個 IM 平台實例，可同時啟用多個平台（各自獨立啟停）。`config` 中的 `${ENV_VAR}` 佔位符會在執行時從環境變數解析，明文密鑰不會寫回 config.json。

### 取得 AI 設定選項

**GET** `/config/aioptions`

傳回可用的 AI 用戶端類型及其動態選項（可用模型、區域等）。

### 取得 IM 平台選項

**GET** `/config/imoptions`

傳回 IM 平台元資料（供設定精靈動態渲染表單）：

```json
{
  "success": true,
  "platforms": [
    {
      "value": "feishu",
      "display": "飛書 (Feishu)",
      "authModes": ["manual", "oauth"],
      "needsPublicCallback": false,
      "help": "...",
      "helpUrl": "https://open.feishu.cn/app",
      "fields": [
        { "key": "appId", "label": "App ID", "type": "text", "required": true },
        { "key": "appSecret", "label": "App Secret", "type": "password", "required": true, "isSecret": true }
      ]
    }
  ]
}
```

### 瀏覽設定

**GET** `/config/browse`

傳回設定項目的瀏覽資料（用於設定介面的分組展示）。

---

## 記憶系統

### 記憶頁面

**GET** `/memory`

傳回記憶管理介面頁面。

### 取得記憶清單

**GET** `/api/memory/list`

傳回矽基生命體的記憶條目清單。

### 取得記憶詳細資料

**GET** `/api/memory/detail/{id}`

路徑參數：`id` — 記憶條目 ID

傳回指定記憶條目的完整內容。

### 取得記憶統計

**GET** `/api/memory/stats`

傳回記憶系統的統計資訊。

### 搜尋記憶

**GET** `/api/memory/search`

查詢參數：`keyword` — 搜尋關鍵字

搜尋相符的記憶條目。

### 取得記憶按生命體分組

**GET** `/api/memory/beings`

按矽基生命體分組的記憶統計。

### 取得記憶追溯

**GET** `/api/memory/trace/{id}`

路徑參數：`id` — 記憶條目 ID

傳回指定記憶條目的來源追溯鏈。

### 取得記憶時間線 HTML

**GET** `/api/memory/timeline-html`

傳回記憶時間線的 HTML 檢視。

---

## 工作筆記

### 工作筆記頁面

**GET** `/work-notes`

傳回工作筆記介面頁面。

### 取得工作筆記清單

**GET** `/api/work-notes/list`

傳回工作筆記清單。

### 讀取工作筆記

**GET** `/api/work-notes/read`

查詢參數：`noteId` — 筆記 ID

傳回指定筆記的內容。

### 取得筆記目錄

**GET** `/api/work-notes/directory`

傳回筆記目錄結構。

### 搜尋工作筆記

**GET** `/api/work-notes/search`

查詢參數：`keyword` — 搜尋關鍵字

搜尋相符的工作筆記。

### 建立工作筆記

**POST** `/api/work-notes/create`

**請求體**：
```json
{
  "title": "筆記標題",
  "content": "筆記內容",
  "keywords": ["關鍵字1", "關鍵字2"]
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

## 知識網路

### 知識網路頁面

**GET** `/knowledge`

傳回知識網路管理介面頁面。

### 取得知識圖譜

**GET** `/api/knowledge/graph`

傳回知識三元組圖譜資料（主體-關係-客體）。

---

## 專案管理

### 專案頁面

**GET** `/project`

傳回專案管理介面頁面。

### 專案工作筆記頁面

**GET** `/project/{id}/work-notes`

路徑參數：`id` — 專案 ID

傳回指定專案的工作筆記頁面。

### 專案任務頁面

**GET** `/project/{id}/tasks`

路徑參數：`id` — 專案 ID

傳回指定專案的任務管理頁面。

### 專案工具權限頁面

**GET** `/project/{id}/tool-permissions`

路徑參數：`id` — 專案 ID

傳回指定專案的工具權限管理頁面。

### 專案工作流程頁面

**GET** `/project/{id}/workflow`

路徑參數：`id` — 專案 ID

傳回指定專案的工作流程管理頁面。

### 取得專案工作流程詳細資料

**GET** `/api/projects/workflow-detail`

查詢參數：`projectId` — 專案 ID

傳回專案關聯的工作流程詳細資料。

### 指派專案角色

**POST** `/api/projects/assign-role`

**請求體**：
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### 移除專案角色

**POST** `/api/projects/remove-role`

**請求體**：
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### 取得專案清單

**GET** `/api/projects/list`

傳回所有專案的清單。

### 取得專案工作流程範本清單

**GET** `/api/projects/list-workflow-templates`

傳回可用的工作流程範本清單。

### 建立專案

**POST** `/api/projects/create`

**請求體**：
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### 封存專案

**POST** `/api/projects/{id}/archive`

路徑參數：`id` — 專案 ID

封存指定專案。

### 復原專案

**POST** `/api/projects/{id}/restore`

路徑參數：`id` — 專案 ID

復原已封存的專案。

### 銷毀專案

**POST** `/api/projects/{id}/destroy`

路徑參數：`id` — 專案 ID

永久刪除指定專案（不可復原）。

### 取得專案詳細資料

**GET** `/api/projects/detail`

查詢參數：`projectId` — 專案 ID

傳回專案的詳細資料。

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

### 指派成員到專案

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

### 取得專案工作筆記清單

**GET** `/api/projects/{id}/work-notes/list`

路徑參數：`id` — 專案 ID

傳回指定專案的工作筆記清單。

### 讀取專案工作筆記

**GET** `/api/projects/{id}/work-notes/read`

路徑參數：`id` — 專案 ID

傳回指定專案的工作筆記內容。

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

### 取得專案任務清單

**GET** `/api/projects/{id}/tasks/list`

路徑參數：`id` — 專案 ID

傳回指定專案的任務清單。

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

### 指派任務負責人

**POST** `/api/projects/{id}/tasks/assign`

路徑參數：`id` — 專案 ID

為專案任務指派負責人。

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

## 工具權限管理

### 取得矽基生命體工具權限

**GET** `/api/beings/tool-permissions`

查詢參數：`beingId` — 矽基生命體 ID

傳回指定矽基生命體的工具權限設定。

### 更新矽基生命體工具權限

**PUT** `/api/beings/tool-permissions`

**請求體**：
```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network": "allowed",
    "disk_read": "allowed",
    "disk_write": "denied"
  }
}
```

### 取得工具權限範本

**GET** `/api/beings/tool-permissions/templates`

傳回可用的工具權限範本清單。

### 套用工具權限範本

**POST** `/api/beings/tool-permissions/apply-template`

**請求體**：
```json
{
  "beingId": "being-uuid",
  "templateName": "readonly"
}
```

### 取得專案工具權限

**GET** `/api/projects/{id}/tool-permissions`

路徑參數：`id` — 專案 ID

傳回指定專案的工具權限設定。

### 更新專案工具權限

**PUT** `/api/projects/{id}/tool-permissions`

路徑參數：`id` — 專案 ID

**請求體**：
```json
{
  "permissions": {
    "network": "allowed",
    "disk_read": "allowed",
    "disk_write": "denied"
  }
}
```

---

## 執行器管理

### 執行器頁面

**GET** `/executor`

傳回執行器管理介面頁面。

### 取得執行器狀態

**GET** `/api/executors/status`

傳回各執行器（磁碟、網路、命令列）的執行狀態。

---

## 程式碼瀏覽器

### 程式碼瀏覽器頁面

**GET** `/code`

傳回程式碼瀏覽器介面頁面。

### 取得程式碼類型清單

**GET** `/api/code/types`

傳回支援的程式碼類型/語言清單。

### 取得程式碼詳細資料

**GET** `/api/code/detail`

查詢參數：`filePath`, `lineNumber`

傳回指定檔案的程式碼詳細資料。

---

## 程式碼懸浮提示

### 取得懸浮提示

**GET** `/api/code/hover`
**POST** `/api/code/hover`

取得程式碼位置的懸浮提示資訊（類似 IDE 的智慧提示）。

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

## 技能管理

### 技能管理頁面

**GET** `/skill` 或 **GET** `/skill/index`

查詢參數：`beingId` — 生命體 ID（必需）

傳回指定矽基生命體的技能管理頁面（技能清單 + Markdown 編輯器）。

### 取得技能清單

**GET** `/api/skills/list`

查詢參數：`beingId` — 生命體 ID（必需）

傳回生命體的所有技能（id、description、version、tags、source、triggerMode、toolWhitelist、maxToolRound、timeoutSeconds、parameterCount），以及統計資訊（技能總數 / 自訂技能數 / 配額上限）。

### 取得技能 Markdown

**GET** `/api/skills/get-md`

查詢參數：`beingId`、`skillId`

傳回指定技能的 Markdown 文字（YAML 前置元資料 + 提示詞正文）。

### 儲存技能 Markdown

**POST** `/api/skills/update-md?beingId={beingId}`

請求體（`application/json`）：

```json
{
  "markdown": "---\nid: my_skill\n...\n---\n\n提示詞正文",
  "skillId": "my_skill"
}
```

以 Markdown 更新或新建技能（upsert 語義）。缺失的元資料由 AI 自動補全；透過 Web UI 儲存的技能 `Source` 標記為 `User`。受配額 `MaxCustomSkillsPerBeing` 限制。

### 匯入技能（JSON）

**POST** `/api/skills/import?beingId={beingId}`

請求體：`{ "json": "<技能定義 JSON>" }`

從 JSON 匯入技能，同樣受配額限制。

### 匯入技能（Markdown）

**POST** `/api/skills/import-md?beingId={beingId}`

請求體：`{ "markdown": "<Markdown 文字>" }`

從 Markdown 匯入新技能，缺失元資料由 AI 自動補全。

### 刪除技能

**POST** `/api/skills/delete?beingId={beingId}`

請求體：`{ "skillId": "my_skill" }`

刪除技能（同時刪除對應的 `.md` 與 `.json` 持久化檔案）。

### 匯出技能（JSON）

**GET** `/api/skills/export?beingId={beingId}&skillId={skillId}`

以 JSON 附件形式下載技能定義（`{id}.json`）。

### 匯出技能（Markdown）

**GET** `/api/skills/export-md?beingId={beingId}&skillId={skillId}`

以 Markdown 附件形式下載技能（`{id}.md`）。

### 測試執行技能

**POST** `/api/skills/test?beingId={beingId}`

請求體：

```json
{
  "skillId": "my_skill",
  "parametersJson": "{ \"topic\": \"AI 新聞\" }"
}
```

以給定參數執行一次技能並傳回 `ToolResult`（含 AI 執行輪數與最終輸出）。

---

## MCP 管理

### MCP 管理頁面

**GET** `/mcp`

查詢參數：`beingId` — 生命體 ID（可選，用於顯示該生命體可見的 MCP 工具）

傳回 MCP 伺服器管理頁面。

### 取得伺服器清單

**GET** `/api/mcp/list-servers`

傳回所有已設定的 MCP 伺服器狀態：

```json
{
  "success": true,
  "data": [
    {
      "id": "filesystem",
      "name": "Filesystem",
      "transport": "stdio",
      "state": "connected",
      "enabled": true,
      "toolCount": 8,
      "endpoint": null,
      "lastError": null
    }
  ],
  "mcpEnabled": true,
  "connected": 1,
  "toolTotal": 8
}
```

`state` 取值：`connected` / `disconnected` / `connecting` / `error`。

### 取得伺服器工具清單

**GET** `/api/mcp/list-tools?serverId={serverId}`

傳回指定伺服器提供的工具（`name` 為帶前綴的完整名 `mcp_{serverId}_{toolName}`、`description`、`schema`）。伺服器未連線時傳回錯誤。

### 新增伺服器

**POST** `/api/mcp/add-server`

請求體（`McpServerConfig`）：

```json
{
  "id": "filesystem",
  "name": "Filesystem",
  "transport": "stdio",
  "command": "npx",
  "arguments": ["-y", "@modelcontextprotocol/server-filesystem", "/data"],
  "env": {},
  "endpoint": null,
  "enabled": true
}
```

`transport` 支援 `stdio`（本機處理程序：`command` + `arguments`）與 `http`（遠端端點：`endpoint`）。伺服器 ID 僅允許小寫字母、數字和底線。新增後立即連線並同步到所有矽基生命體。

### 啟用/停用伺服器

**POST** `/api/mcp/toggle`

請求體：`{ "serverId": "filesystem", "enabled": true }`

### 移除伺服器

**POST** `/api/mcp/remove-server`

請求體：`{ "serverId": "filesystem" }`

刪除伺服器設定並從所有生命體註銷其工具。

### 重新連線伺服器

**POST** `/api/mcp/reconnect`

請求體：`{ "serverId": "filesystem" }`

強制斷線並重新建立連線，重新整理工具清單。

### 測試工具呼叫

**POST** `/api/mcp/test-tool`

請求體：

```json
{
  "serverId": "filesystem",
  "toolName": "read_file",
  "argumentsJson": "{ \"path\": \"/data/hello.txt\" }"
}
```

直接呼叫 MCP 伺服器的工具（無需 AI 參與），用於驗證連通性。

---

## IM 平台 OAuth 授權

### 發起授權

**GET** `/im/{platform}/authorize`

路徑參數：`platform` — IM 平台識別（如 `feishu`）

產生防 CSRF 的隨機 `state`，登記 5 分鐘有效的授權會話，傳回授權 URL 並自動開啟系統預設瀏覽器。同一平台重複發起會覆蓋舊會話。

### 授權回呼

**GET** `/im/{platform}/callback?code={code}&state={state}`

由 IM 平台重新導向呼叫。驗證 `state` 後用授權碼換取存取權杖，將 `accessToken`、`refreshToken`、`tokenExpiresAt`、`authMode=oauth` 寫回該平台的設定並持久化，最後渲染授權結果落地頁（成功/失敗）。

### 查詢授權狀態

**GET** `/im/{platform}/status`

傳回 `{ platform, status, tokenExpiresAt }`。`status` 取值：`pending` / `success` / `failed` / `timeout` / `none`。前端優先透過 SSE 事件 `im_auth_status` 接收狀態推送，此介面作為輪詢兜底。

---

## 說明文件系統

### 說明頁面

**GET** `/help` 或 **GET** `/help/index`

傳回說明文件主頁。

### 說明主題頁面

**GET** `/help/{topic}`

路徑參數：`topic` — 主題識別碼

傳回指定主題的說明文件頁面。

### 搜尋說明文件

**GET** `/api/help/search`

查詢參數：`keyword` — 搜尋關鍵字

搜尋相符的說明文件主題。

---

## 初始化

### 初始化精靈頁面

**GET** `/init`

傳回首次執行初始化精靈頁面。

### 提交初始化

**POST** `/init`

提交首次執行的初始化設定。

### 瀏覽選擇資料目錄

**GET** `/init/browse`

開啟目錄瀏覽器以選擇資料儲存位置。

### 取得 AI 設定元資料

**GET** `/init/ai-config-metadata`

傳回可用的 AI 用戶端類型及其設定欄位元資料。

---

## 系統控制

### 優雅關閉

**POST** `/api/system/shutdown`

> **注意**：僅允許來自 localhost 的請求

觸發應用程式的優雅關閉流程：

1. 停止主迴圈（MainLoop）
2. 儲存目前設定
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

傳回關於頁面，包含系統資訊和已載入的外掛程式清單。

**外掛程式清單資料**：
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

所有端點傳回標準化的錯誤回應：

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: FileAccess, Denied by GlobalACL"
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

伺服器傳送事件用於即時更新：

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

### IM 授權狀態事件

IM 平台 OAuth 授權精靈透過共享 SSE 連線推送狀態（事件名 `im_auth_status`）：

```javascript
eventSource.addEventListener('im_auth_status', (event) => {
  const data = JSON.parse(event.data);
  // data.platform — 平台識別（feishu / wecom / dingtalk）
  // data.status  — pending / success / failed / timeout
  // data.message — 附加說明
  updateAuthStatus(data.platform, data.status);
});
```

---

## AI 用戶端介面

### IAIClient 介面

```csharp
public interface IAIClient
{
    string Endpoint { get; }
    string DefaultModel { get; }
    bool? StreamingMode { get; }
    bool? SupportsToolCalls { get; }
    int? ContextWindowTokens { get; }
    bool? SupportsVision { get; }
    bool? SupportsAudio { get; }
    
    AIResponse Chat(AIRequest request);
}
```

| 屬性 | 類型 | 描述 |
|------|------|------|
| `Endpoint` | `string` | AI 服務端點 URL |
| `DefaultModel` | `string` | 預設模型名稱 |
| `StreamingMode` | `bool?` | 串流模式：true=僅串流、false=僅非串流、null=兩種均支援 |
| `SupportsToolCalls` | `bool?` | 工具呼叫支援：true=支援、false=不支援（跳過工具注入）、null=未知 |
| `ContextWindowTokens` | `int?` | 上下文視窗大小（token 數），用於 token 預算裁剪 |
| `SupportsVision` | `bool?` | 視覺輸入支援：true=支援圖片、false=不支援、null=未知 |
| `SupportsAudio` | `bool?` | 音訊輸入支援：true=支援音訊、false=不支援、null=未知 |

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
