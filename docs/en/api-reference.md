# API Reference

> **Version: v0.2.0-alpha**

[**English**](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | [Русский](../ru-RU/api-reference.md)

## Web API Endpoints

Base URL: `http://localhost:8080`

### Authentication

Most endpoints require authentication via session cookies managed through the Web UI. Before system initialization, all requests except the help page will be redirected to the initialization page.

---

## Dashboard

### Get Dashboard Statistics

**GET** `/api/dashboard/stats`

Returns system overview data (number of beings, running status, etc.).

### Get Performance Metrics

**GET** `/api/dashboard/metrics`

Returns real-time performance metric data.

---

## Chat System

### Chat Page

**GET** `/chat`

Returns the chat interface page.

### Streaming Chat (SSE)

**GET** `/api/chat/stream`

Streaming chat via Server-Sent Events (SSE).

**Response**: Server-Sent Event stream

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Get Conversation List

**GET** `/api/chat/conversations`

Returns a list of all active Chat Sessions.

**Response Example**:
```json
{
  "conversations": [
    {
      "sessionId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
      "beingId": "being-uuid",
      "type": "single",
      "displayName": "Chat with Assistant",
      "lastMessage": "Last message content",
      "lastTime": "2026-05-20T10:30:00Z"
    }
  ]
}
```

### Get Message History

**GET** `/api/chat/messages`

Query parameter: `channelId` — Channel/Session ID

Returns the message history for the specified session.

### Get Chat History

**GET** `/api/chat/history`

Returns global chat history records.

### Send Message

**POST** `/api/chat/send`

**Request Body**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
  "content": "Test message content"
}
```

**Response**:
```json
{
  "success": true,
  "messageId": "50156b26-f3b9-4735-be3d-51e547bd3a4a"
}
```

### Stop AI Thinking

**POST** `/api/chat/stop`

Stops the currently ongoing AI response generation.

**Request Body**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d"
}
```

### Upload File

**POST** `/api/chat/upload`

Uploads a file to the Chat Session (supports multipart/form-data).

---

## Silicon Being Management

### Being Management Page

**GET** `/beings`

Returns the Silicon Being management interface page.

### Get Being List

**GET** `/api/beings` or **GET** `/api/beings/list`

Returns a list of all registered Silicon Beings.

**Response Example**:
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

**Status values**: `idle` | `running` | `waiting_permission` | `stopped`

### Get Being Detail

**GET** `/api/beings/detail`

Query parameter: `beingId` — Silicon Being ID

Returns detailed information about the specified Silicon Being.

### Get Being Activity Status

**GET** `/api/beings/activity`

Returns activity status information for each Silicon Being.

### Soul File Editor Page

**GET** `/beings/soul`

Returns the Soul File editor interface.

### Save Soul File

**POST** `/api/beings/soul/save`

**Request Body**:
```json
{
  "beingId": "being-uuid",
  "soulContent": "# Personality\nYou are helpful..."
}
```

### AI Config Editor Page

**GET** `/beings/ai-config`

Returns the AI configuration editor interface.

### Save AI Config

**POST** `/api/beings/ai-config/save`

**Request Body**:
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

### Get Available AI Model List

**GET** `/api/beings/ai-config/models`

Query parameters: `clientType`, `apiKey`, `region`

Returns the list of available models for the specified AI client.

---

## Chat History View

### Chat History Page

**GET** `/chat-history`

Returns the chat history main page.

### Chat History Detail Page

**GET** `/chat-history-detail`

Returns the chat history detail page for the specified session.

### Group Chat History Detail Page

**GET** `/group-chat-history-detail`

Returns the history detail page for Group Chat Sessions.

### Broadcast History Detail Page

**GET** `/broadcast-history-detail`

Returns the history detail page for Broadcast Channels.

### Get History Conversation List

**GET** `/api/chat-history/conversations`

Returns a list of all historical conversations.

### Get History Messages

**GET** `/api/chat-history/messages`

Query parameter: `sessionId` — Session ID

Returns the message records for the specified historical session.

---

## Timer Management

### Timer Page

**GET** `/timers`

Returns the Timer management interface page.

### Get Timer List

**GET** `/api/timers/list`

Returns a list of all timers.

### Timer Cycle Detail Page

**GET** `/timer-cycles/{timerId}`

Returns the execution cycle detail page for the specified timer.

### Get Timer Cycle List

**GET** `/api/timer-cycles/list`

Query parameter: `timerId` — Timer ID

Returns a list of all execution cycles for the specified timer.

### Single Execution Cycle Detail Page

**GET** `/timer-cycle/{cycleIndex}`

Returns the detail page for a single execution.

### Get Cycle Messages

**GET** `/api/timer-cycle/messages`

Query parameter: `cycleIndex` — Cycle index

Returns the related messages for the specified execution cycle.

---

## Task Management

### Task Page

**GET** `/tasks`

Returns the Task management interface page.

### Get Task List

**GET** `/api/tasks/list`

Returns a list of all tasks.

### Task Cycle Detail Page

**GET** `/task-cycles/{taskId}`

Returns the execution cycle detail page for the specified task.

### Get Task Cycle List

**GET** `/api/task-cycles/list`

Query parameter: `taskId` — Task ID

Returns a list of all execution cycles for the specified task.

### Single Execution Cycle Detail Page

**GET** `/task-cycle/{cycleIndex}`

Returns the detail page for a single task execution.

### Get Cycle Messages

**GET** `/api/task-cycle/messages`

Query parameter: `cycleIndex` — Cycle index

Returns the related messages for the specified task execution cycle.

---

## Permission System

### Permission Management Page

**GET** `/permissions`

Returns the Permission Manager interface page.

### Get Permission Rule List

**GET** `/api/permissions/list`

Returns all currently configured permission rules.

**Response Example**:
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

### Save Permission Rule

**POST** `/api/permissions/save`

**Request Body**:
```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects",
  "result": "Allowed",
  "description": "Allow project directory access"
}
```

### Permission Request Page

**GET** `/permission/request`

Displays the permission request page, allowing users to approve or deny permission requests from Silicon Beings.

**Query Parameters**:

| Parameter | Type | Description |
|-----------|------|-------------|
| `userId` | `Guid` | The Silicon Being ID requesting permission |
| `type` | `string` | Permission Type |
| `resource` | `string` | Requested resource path |
| `allowCode` | `string` | Code identifier for the allow action |
| `denyCode` | `string` | Code identifier for the deny action |

### Check Pending Permission Requests

**GET** `/permission/check`

Query parameter: `userId` — Silicon Being ID

**Response**:
```json
{
  "pending": true
}
```

### Respond to Permission Request

**GET** `/permission/respond`

**Query Parameters**:

| Parameter | Type | Description |
|-----------|------|-------------|
| `userId` | `Guid` | Silicon Being ID |
| `allowed` | `bool` | Whether to allow |
| `addToCache` | `bool` | Whether to cache the decision |
| `cacheDuration` | `double` | Cache duration (hours) |

**Response**:
```json
{
  "success": true
}
```

---

## Logging System

### Log Page

**GET** `/logs`

Returns the log viewer interface page.

### Get Log List

**GET** `/api/logs/list`

Query parameters support filtering by level and time range.

**Response Example**:
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

### Get Logs Grouped by Being

**GET** `/api/logs/beings`

Returns log statistics grouped by Silicon Being.

### Get Available Log Levels

**GET** `/api/logs/levels`

Returns the list of available Log Levels in the system.

---

## Usage Statistics

### Usage Statistics Page

**GET** `/usage`

Returns the usage statistics interface page.

### Get Usage Summary

**GET** `/api/usage/summary`

Returns a Token usage and cost summary.

### Get Trend Data

**GET** `/api/usage/trend`

Query parameters: `startDate`, `endDate`

Returns usage trend data for the specified time period.

### Export Usage Data

**GET** `/api/usage/export`

Exports usage data in a downloadable format.

---

## Audit Trail

### Audit Page

**GET** `/audit`

Returns the audit trail interface page.

### Get Audit List

**GET** `/api/audit/list`

Returns a list of audit log entries.

### Get Audit Summary

**GET** `/api/audit/summary`

Returns summary statistics of audit data.

### Get Audit Grouped by Being

**GET** `/api/audit/beings`

Returns audit statistics grouped by Silicon Being.

---

## Configuration Management

### Configuration Page

**GET** `/config`

Returns the system configuration interface page.

### Save Configuration

**POST** `/config/save`

**Request Body**:
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
      "endpoint": "...",
      "model": "..."
    },
    "QiniuAI": {
      "apiKey": "...",
      "endpoint": "...",
      "model": "..."
    },
    "DeepSeek": {
      "apiKey": "...",
      "endpoint": "https://api.deepseek.com",
      "model": "deepseek-v4-flash"
    },
    "Zhipu": {
      "apiKey": "...",
      "endpoint": "https://open.bigmodel.cn/api/paas/v4",
      "model": "glm-4-flash"
    },
    "Ernie": {
      "apiKey": "...",
      "endpoint": "https://qianfan.baidubce.com/v2",
      "model": "ernie-5.1"
    },
    "Hunyuan": {
      "apiKey": "...",
      "endpoint": "https://tokenhub.tencentmaas.com/v1",
      "model": "hy3"
    },
    "MiniMax": {
      "apiKey": "...",
      "endpoint": "https://api.minimaxi.com/v1",
      "model": "MiniMax-M3"
    },
    "Moonshot": {
      "apiKey": "...",
      "endpoint": "https://api.moonshot.cn/v1",
      "model": "kimi-k2.6"
    },
    "SiliconFlow": {
      "apiKey": "...",
      "endpoint": "https://api.siliconflow.cn/v1",
      "model": "deepseek-ai/DeepSeek-V3.2"
    }
  }
}
```

### Get AI Configuration Options

**GET** `/config/aioptions`

Returns available AI client types and their dynamic options (available models, regions, etc.).

### Get IM Platform Options

**GET** `/config/imoptions`

Returns IM platform metadata (for the configuration wizard to dynamically render forms):

```json
{
  "success": true,
  "platforms": [
    {
      "value": "feishu",
      "display": "Feishu",
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

### Browse Configuration

**GET** `/config/browse`

Returns browse data for configuration items (used for grouped display in the configuration interface).

---

## Memory System

### Memory Page

**GET** `/memory`

Returns the memory management interface page.

### Get Memory List

**GET** `/api/memory/list`

Returns a list of memory entries for Silicon Beings.

### Get Memory Detail

**GET** `/api/memory/detail/{id}`

Path parameter: `id` — Memory entry ID

Returns the full content of the specified memory entry.

### Get Memory Statistics

**GET** `/api/memory/stats`

Returns statistics for the memory system.

### Search Memory

**GET** `/api/memory/search`

Query parameter: `keyword` — Search keyword

Searches for matching memory entries.

### Get Memory Grouped by Being

**GET** `/api/memory/beings`

Returns memory statistics grouped by Silicon Being.

### Get Memory Trace

**GET** `/api/memory/trace/{id}`

Path parameter: `id` — Memory entry ID

Returns the source trace chain for the specified memory entry.

### Get Memory Timeline HTML

**GET** `/api/memory/timeline-html`

Returns an HTML view of the memory timeline.

---

## Work Notes

### Work Notes Page

**GET** `/work-notes`

Returns the Work Note System interface page.

### Get Work Notes List

**GET** `/api/work-notes/list`

Returns a list of work notes.

### Read Work Note

**GET** `/api/work-notes/read`

Query parameter: `noteId` — Note ID

Returns the content of the specified note.

### Get Note Directory

**GET** `/api/work-notes/directory`

Returns the note directory structure.

### Search Work Notes

**GET** `/api/work-notes/search`

Query parameter: `keyword` — Search keyword

Searches for matching work notes.

### Create Work Note

**POST** `/api/work-notes/create`

**Request Body**:
```json
{
  "title": "Note title",
  "content": "Note content",
  "keywords": ["keyword1", "keyword2"]
}
```

### Update Work Note

**POST** `/api/work-notes/update`

**Request Body**:
```json
{
  "noteId": "note-uuid",
  "title": "Updated title",
  "content": "Updated content"
}
```

### Delete Work Note

**POST** `/api/work-notes/delete`

**Request Body**:
```json
{
  "noteId": "note-uuid"
}
```

---

## Knowledge Network

### Knowledge Network Page

**GET** `/knowledge`

Returns the Knowledge Network management interface page.

### Get Knowledge Graph

**GET** `/api/knowledge/graph`

Returns knowledge triple graph data (subject-relation-object).

---

## Project Management

### Project Page

**GET** `/project`

Returns the Project System interface page.

### Project Work Notes Page

**GET** `/project/{id}/work-notes`

Path parameter: `id` — Project ID

Returns the work notes page for the specified project.

### Project Tasks Page

**GET** `/project/{id}/tasks`

Path parameter: `id` — Project ID

Returns the task management page for the specified project.

### Project Tool Permissions Page

**GET** `/project/{id}/tool-permissions`

Path parameter: `id` — Project ID

Returns the tool permission management page for the specified project.

### Project Workflow Page

**GET** `/project/{id}/workflow`

Path parameter: `id` — Project ID

Returns the workflow management page for the specified project.

### Get Project Workflow Detail

**GET** `/api/projects/workflow-detail`

Query parameter: `projectId` — Project ID

Returns the workflow details associated with the project.

### Assign Project Role

**POST** `/api/projects/assign-role`

**Request Body**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### Remove Project Role

**POST** `/api/projects/remove-role`

**Request Body**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### Get Project List

**GET** `/api/projects/list`

Returns a list of all projects.

### Get Project Workflow Template List

**GET** `/api/projects/list-workflow-templates`

Returns a list of available workflow templates.

### Create Project

**POST** `/api/projects/create`

**Request Body**:
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### Archive Project

**POST** `/api/projects/{id}/archive`

Path parameter: `id` — Project ID

Archives the specified project.

### Restore Project

**POST** `/api/projects/{id}/restore`

Path parameter: `id` — Project ID

Restores an archived project.

### Destroy Project

**POST** `/api/projects/{id}/destroy`

Path parameter: `id` — Project ID

Permanently deletes the specified project (irreversible).

### Get Project Detail

**GET** `/api/projects/detail`

Query parameter: `projectId` — Project ID

Returns detailed information about the project.

### Update Project

**POST** `/api/projects/update`

**Request Body**:
```json
{
  "projectId": "project-uuid",
  "name": "Updated Name",
  "description": "Updated description"
}
```

### Assign Member to Project

**POST** `/api/projects/assign`

**Request Body**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Remove Member from Project

**POST** `/api/projects/remove`

**Request Body**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Get Project Work Notes List

**GET** `/api/projects/{id}/work-notes/list`

Path parameter: `id` — Project ID

Returns a list of work notes for the specified project.

### Read Project Work Note

**GET** `/api/projects/{id}/work-notes/read`

Path parameter: `id` — Project ID

Returns the content of a work note in the specified project.

### Create Project Work Note

**POST** `/api/projects/{id}/work-notes/create`

Path parameter: `id` — Project ID

Creates a new work note in the specified project.

### Update Project Work Note

**POST** `/api/projects/{id}/work-notes/update`

Path parameter: `id` — Project ID

Updates a work note in the specified project.

### Delete Project Work Note

**POST** `/api/projects/{id}/work-notes/delete`

Path parameter: `id` — Project ID

Deletes a work note in the specified project.

### Get Project Task List

**GET** `/api/projects/{id}/tasks/list`

Path parameter: `id` — Project ID

Returns a list of tasks for the specified project.

### Create Project Task

**POST** `/api/projects/{id}/tasks/create`

Path parameter: `id` — Project ID

Creates a new task in the specified project.

### Update Project Task

**POST** `/api/projects/{id}/tasks/update`

Path parameter: `id` — Project ID

Updates a task in the specified project.

### Delete Project Task

**POST** `/api/projects/{id}/tasks/delete`

Path parameter: `id` — Project ID

Deletes a task in the specified project.

### Assign Task Assignee

**POST** `/api/projects/{id}/tasks/assign`

Path parameter: `id` — Project ID

Assigns an assignee to a project task.

### Remove Task Assignee

**POST** `/api/projects/{id}/tasks/remove-assignee`

Path parameter: `id` — Project ID

Removes the assignee from a project task.

### Mark Task Complete

**POST** `/api/projects/{id}/tasks/complete`

Path parameter: `id` — Project ID

Marks a project task as completed.

### Mark Task Failed

**POST** `/api/projects/{id}/tasks/fail`

Path parameter: `id` — Project ID

Marks a project task as failed.

### Cancel Task

**POST** `/api/projects/{id}/tasks/cancel`

Path parameter: `id` — Project ID

Cancels a project task.

---

## Tool Permission Management

### Get Silicon Being Tool Permissions

**GET** `/api/beings/tool-permissions`

Query parameter: `beingId` — Silicon Being ID

Returns the tool permission configuration for the specified Silicon Being.

### Update Silicon Being Tool Permissions

**PUT** `/api/beings/tool-permissions`

**Request Body**:
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

### Get Tool Permission Templates

**GET** `/api/beings/tool-permissions/templates`

Returns a list of available tool permission templates.

### Apply Tool Permission Template

**POST** `/api/beings/tool-permissions/apply-template`

**Request Body**:
```json
{
  "beingId": "being-uuid",
  "templateName": "readonly"
}
```

### Get Project Tool Permissions

**GET** `/api/projects/{id}/tool-permissions`

Path parameter: `id` — Project ID

Returns the tool permission configuration for the specified project.

### Update Project Tool Permissions

**PUT** `/api/projects/{id}/tool-permissions`

Path parameter: `id` — Project ID

**Request Body**:
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

## Executor Management

### Executor Page

**GET** `/executor`

Returns the Executor management interface page.

### Get Executor Status

**GET** `/api/executors/status`

Returns the running status of each Executor (Disk, Network, CommandLine).

---

## Code Browser

### Code Browser Page

**GET** `/code`

Returns the code browser interface page.

### Get Code Type List

**GET** `/api/code/types`

Returns the list of supported code types/languages.

### Get Code Detail

**GET** `/api/code/detail`

Query parameters: `filePath`, `lineNumber`

Returns code details for the specified file.

---

## Code Hover Tips

### Get Hover Tip

**GET** `/api/code/hover`
**POST** `/api/code/hover`

Gets hover tip information for a code position (similar to IDE IntelliSense).

### Register Code Position

**POST** `/api/code/register`

Registers a code position to monitor.

### Update Code Position

**POST** `/api/code/update`

Updates a registered code position.

### Unregister Code Position

**POST** `/api/code/unregister`

Unregisters a code position that no longer needs monitoring.

---

## Skill Management

### Skill Management Page

**GET** `/skill` or **GET** `/skill/index`

Query parameter: `beingId` — Silicon Being ID (required)

Returns the Skill management page for the specified Silicon Being (skill list + Markdown editor).

### Get Skill List

**GET** `/api/skills/list`

Query parameter: `beingId` — Silicon Being ID (required)

Returns all skills for the Silicon Being (id, description, version, tags, source, triggerMode, toolWhitelist, maxToolRound, timeoutSeconds, parameterCount), along with statistics (total skills / custom skills / quota limit).

### Get Skill Markdown

**GET** `/api/skills/get-md`

Query parameters: `beingId`, `skillId`

Returns the Markdown text of the specified skill (YAML front matter + prompt body).

### Save Skill Markdown

**POST** `/api/skills/update-md?beingId={beingId}`

Request body (`application/json`):

```json
{
  "markdown": "---\nid: my_skill\n...\n---\n\nPrompt body",
  "skillId": "my_skill"
}
```

Updates or creates a skill via Markdown (upsert semantics). Missing metadata is automatically filled by AI; skills saved through the Web UI are marked with `Source` as `User`. Subject to the `MaxCustomSkillsPerBeing` quota.

### Import Skill (JSON)

**POST** `/api/skills/import?beingId={beingId}`

Request body: `{ "json": "<skill definition JSON>" }`

Imports a skill from JSON, also subject to the quota limit.

### Import Skill (Markdown)

**POST** `/api/skills/import-md?beingId={beingId}`

Request body: `{ "markdown": "<Markdown text>" }`

Imports a new skill from Markdown; missing metadata is automatically filled by AI.

### Delete Skill

**POST** `/api/skills/delete?beingId={beingId}`

Request body: `{ "skillId": "my_skill" }`

Deletes a skill (also deletes the corresponding `.md` and `.json` persistence files).

### Export Skill (JSON)

**GET** `/api/skills/export?beingId={beingId}&skillId={skillId}`

Downloads the skill definition as a JSON attachment (`{id}.json`).

### Export Skill (Markdown)

**GET** `/api/skills/export-md?beingId={beingId}&skillId={skillId}`

Downloads the skill as a Markdown attachment (`{id}.md`).

### Test Execute Skill

**POST** `/api/skills/test?beingId={beingId}`

Request body:

```json
{
  "skillId": "my_skill",
  "parametersJson": "{ \"topic\": \"AI news\" }"
}
```

Executes a skill once with the given parameters and returns a `ToolResult` (including AI execution rounds and final output).

---

## MCP Management

### MCP Management Page

**GET** `/mcp`

Query parameter: `beingId` — Silicon Being ID (optional, used to display MCP tools visible to that Silicon Being)

Returns the MCP server management page.

### Get Server List

**GET** `/api/mcp/list-servers`

Returns the status of all configured MCP servers:

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

`state` values: `connected` / `disconnected` / `connecting` / `error`.

### Get Server Tool List

**GET** `/api/mcp/list-tools?serverId={serverId}`

Returns the tools provided by the specified server (`name` is the fully qualified name with prefix `mcp_{serverId}_{toolName}`, `description`, `schema`). Returns an error if the server is not connected.

### Add Server

**POST** `/api/mcp/add-server`

Request body (`McpServerConfig`):

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

`transport` supports `stdio` (local process: `command` + `arguments`) and `http` (remote endpoint: `endpoint`). Server IDs only allow lowercase letters, numbers, and underscores. After adding, it immediately connects and syncs to all Silicon Beings.

### Enable/Disable Server

**POST** `/api/mcp/toggle`

Request body: `{ "serverId": "filesystem", "enabled": true }`

### Remove Server

**POST** `/api/mcp/remove-server`

Request body: `{ "serverId": "filesystem" }`

Removes the server configuration and unregisters its tools from all Silicon Beings.

### Reconnect Server

**POST** `/api/mcp/reconnect`

Request body: `{ "serverId": "filesystem" }`

Forces a disconnect and re-establishes the connection, refreshing the tool list.

### Test Tool Call

**POST** `/api/mcp/test-tool`

Request body:

```json
{
  "serverId": "filesystem",
  "toolName": "read_file",
  "argumentsJson": "{ \"path\": \"/data/hello.txt\" }"
}
```

Directly calls an MCP server tool (without AI involvement), used to verify connectivity.

---

## IM Platform OAuth Authorization

### Initiate Authorization

**GET** `/im/{platform}/authorize`

Path parameter: `platform` — IM platform identifier (e.g., `feishu`)

Generates a CSRF-protected random `state`, registers an authorization session valid for 5 minutes, returns the authorization URL, and automatically opens the system default browser. Repeated requests for the same platform will overwrite the previous session.

### Authorization Callback

**GET** `/im/{platform}/callback?code={code}&state={state}`

Called by the IM platform redirect. After validating `state`, it exchanges the authorization code for an access token, writes `accessToken`, `refreshToken`, `tokenExpiresAt`, and `authMode=oauth` back to the platform's configuration and persists it, then renders the authorization result landing page (success/failure).

### Query Authorization Status

**GET** `/im/{platform}/status`

Returns `{ platform, status, tokenExpiresAt }`. `status` values: `pending` / `success` / `failed` / `timeout` / `none`. The frontend primarily receives status updates via the SSE event `im_auth_status`; this endpoint serves as a polling fallback.

---

## Help Documentation System

### Help Page

**GET** `/help` or **GET** `/help/index`

Returns the help documentation main page.

### Help Topic Page

**GET** `/help/{topic}`

Path parameter: `topic` — Topic identifier

Returns the help documentation page for the specified topic.

### Search Help Documentation

**GET** `/api/help/search`

Query parameter: `keyword` — Search keyword

Searches for matching help documentation topics.

---

## Initialization

### Initialization Wizard Page

**GET** `/init`

Returns the first-run initialization wizard page.

### Submit Initialization

**POST** `/init`

Submits the first-run initialization configuration.

### Browse Data Directory

**GET** `/init/browse`

Opens a directory browser to select the data storage location.

### Get AI Config Metadata

**GET** `/init/ai-config-metadata`

Returns available AI client types and their configuration field metadata.

---

## System Control

### Graceful Shutdown

**POST** `/api/system/shutdown`

> **Note**: Only requests from localhost are allowed

Triggers the application's graceful shutdown process:

1. Stops the Main Loop
2. Saves the current configuration
3. Closes the HTTP listener

**Response**:
```json
{
  "status": "shutting_down",
  "message": "Application is shutting down gracefully"
}
```

---

## About

### About Page

**GET** `/about`

Returns the about page, containing system information and a list of loaded plugins.

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

---

## Error Responses

All endpoints return standardized error responses:

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: FileAccess, Denied by GlobalACL"
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

### IM Authorization Status Events

The IM platform OAuth authorization wizard pushes status updates via a shared SSE connection (event name `im_auth_status`):

```javascript
eventSource.addEventListener('im_auth_status', (event) => {
  const data = JSON.parse(event.data);
  // data.platform — Platform identifier (feishu / wecom / dingtalk)
  // data.status  — pending / success / failed / timeout
  // data.message — Additional information
  updateAuthStatus(data.platform, data.status);
});
```

---

## AI Client Interface

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

## Tool System Interface

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

## Next Steps

- 🚀 Check out the [Getting Started Guide](getting-started.md)
- 🛠️ Read the [Development Guide](development-guide.md)
- 📚 View the [Architecture Documentation](architecture.md)
- 🔒 Learn about the [Security Model](security.md)
