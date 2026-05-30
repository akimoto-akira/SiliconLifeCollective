# API 参考

> **版本：v0.2.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | **中文** | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | [Русский](../ru-RU/api-reference.md)

## Web API 端点

基础 URL：`http://localhost:8080`

### 认证

大多数端点需要通过 Web UI 管理的会话 cookie 进行认证。系统初始化前，除帮助页面外的所有请求将重定向到初始化页面。

---

## 仪表板

### 获取仪表板统计数据

**GET** `/api/dashboard/stats`

返回系统概览数据（生命体数量、运行状态等）。

### 获取性能指标

**GET** `/api/dashboard/metrics`

返回实时性能指标数据。

---

## 聊天系统

### 聊天页面

**GET** `/chat`

返回聊天界面页面。

### 流式聊天（SSE）

**GET** `/api/chat/stream`

通过服务器发送事件（SSE）进行流式聊天。

**响应**：服务器发送事件流

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### 获取会话列表

**GET** `/api/chat/conversations`

返回所有活跃的聊天会话列表。

**响应示例**：
```json
{
  "conversations": [
    {
      "sessionId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
      "beingId": "being-uuid",
      "type": "single",
      "displayName": "与小游聊天",
      "lastMessage": "最后消息内容",
      "lastTime": "2026-05-20T10:30:00Z"
    }
  ]
}
```

### 获取消息历史

**GET** `/api/chat/messages`

查询参数：`channelId` — 频道/会话 ID

返回指定会话的消息历史记录。

### 获取聊天历史

**GET** `/api/chat/history`

返回全局聊天历史记录。

### 发送消息

**POST** `/api/chat/send`

**请求体**：
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
  "content": "测试消息内容"
}
```

**响应**：
```json
{
  "success": true,
  "messageId": "50156b26-f3b9-4735-be3d-51e547bd3a4a"
}
```

### 停止 AI 思考

**POST** `/api/chat/stop`

停止当前正在进行的 AI 响应生成。

**请求体**：
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d"
}
```

### 上传文件

**POST** `/api/chat/upload`

上传文件到聊天会话中（支持 multipart/form-data）。

---

## 硅基生命体管理

### 生命体管理页面

**GET** `/beings`

返回硅基生命体管理界面页面。

### 获取生命体列表

**GET** `/api/beings` 或 **GET** `/api/beings/list`

返回所有已注册的硅基生命体列表。

**响应示例**：
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

**状态值**：`idle` | `running` | `waiting_permission` | `stopped`

### 获取生命体详情

**GET** `/api/beings/detail`

查询参数：`beingId` — 生命体 ID

返回指定生命体的详细信息。

### 获取生命体活动状态

**GET** `/api/beings/activity`

返回各生命体的活动状态信息。

### 灵魂文件编辑器页面

**GET** `/beings/soul`

返回灵魂文件编辑器界面。

### 保存灵魂文件

**POST** `/api/beings/soul/save`

**请求体**：
```json
{
  "beingId": "being-uuid",
  "soulContent": "# Personality\nYou are helpful..."
}
```

### AI 配置编辑器页面

**GET** `/beings/ai-config`

返回 AI 配置编辑器界面。

### 保存 AI 配置

**POST** `/api/beings/ai-config/save`

**请求体**：
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

### 获取可用 AI 模型列表

**GET** `/api/beings/ai-config/models`

查询参数：`clientType`, `apiKey`, `region`

返回指定 AI 客户端的可用模型列表。

---

## 聊天历史查看

### 聊天历史页面

**GET** `/chat-history`

返回聊天历史主页面。

### 聊天历史详情页面

**GET** `/chat-history-detail`

返回指定会话的聊天历史详情页面。

### 群聊历史详情页面

**GET** `/group-chat-history-detail`

返回群聊的历史详情页面。

### 广播历史详情页面

**GET** `/broadcast-history-detail`

返回广播频道的历史详情页面。

### 获取历史会话列表

**GET** `/api/chat-history/conversations`

返回所有历史会话列表。

### 获取历史消息

**GET** `/api/chat-history/messages`

查询参数：`sessionId` — 会话 ID

返回指定历史会话的消息记录。

---

## 定时器管理

### 定时器页面

**GET** `/timers`

返回定时器管理界面页面。

### 获取定时器列表

**GET** `/api/timers/list`

返回所有定时器的列表。

### 定时器周期详情页面

**GET** `/timer-cycles/{timerId}`

返回指定定时器的执行周期详情页面。

### 获取定时器周期列表

**GET** `/api/timer-cycles/list`

查询参数：`timerId` — 定时器 ID

返回指定定时器的所有执行周期列表。

### 单次执行周期详情页面

**GET** `/timer-cycle/{cycleIndex}`

返回单次执行的详细页面。

### 获取周期消息

**GET** `/api/timer-cycle/messages`

查询参数：`cycleIndex` — 周期索引

返回指定执行周期的相关消息。

---

## 任务管理

### 任务页面

**GET** `/tasks`

返回任务管理界面页面。

### 获取任务列表

**GET** `/api/tasks/list`

返回所有任务的列表。

### 任务周期详情页面

**GET** `/task-cycles/{taskId}`

返回指定任务的执行周期详情页面。

### 获取任务周期列表

**GET** `/api/task-cycles/list`

查询参数：`taskId` — 任务 ID

返回指定任务的所有执行周期列表。

### 单次执行周期详情页面

**GET** `/task-cycle/{cycleIndex}`

返回单次任务执行的详细页面。

### 获取周期消息

**GET** `/api/task-cycle/messages`

查询参数：`cycleIndex` — 周期索引

返回指定任务执行周期的相关消息。

---

## 权限系统

### 权限管理页面

**GET** `/permissions`

返回权限管理界面页面。

### 获取权限规则列表

**GET** `/api/permissions/list`

返回当前配置的所有权限规则。

**响应示例**：
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

### 保存权限规则

**POST** `/api/permissions/save`

**请求体**：
```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects",
  "result": "Allowed",
  "description": "Allow project directory access"
}
```

### 权限请求页面

**GET** `/permission/request`

显示权限请求页面，允许用户批准或拒绝硅基生命体的权限请求。

**查询参数**：

| 参数 | 类型 | 描述 |
|------|------|------|
| `userId` | `Guid` | 请求权限的硅基生命体 ID |
| `type` | `string` | 权限类型 |
| `resource` | `string` | 请求的资源路径 |
| `allowCode` | `string` | 允许操作的代码标识 |
| `denyCode` | `string` | 拒绝操作的代码标识 |

### 检查待处理权限请求

**GET** `/permission/check`

查询参数：`userId` — 硅基生命体 ID

**响应**：
```json
{
  "pending": true
}
```

### 响应权限请求

**GET** `/permission/respond`

**查询参数**：

| 参数 | 类型 | 描述 |
|------|------|------|
| `userId` | `Guid` | 硅基生命体 ID |
| `allowed` | `bool` | 是否允许 |
| `addToCache` | `bool` | 是否将决策缓存 |
| `cacheDuration` | `double` | 缓存持续时间（小时） |

**响应**：
```json
{
  "success": true
}
```

---

## 日志系统

### 日志页面

**GET** `/logs`

返回日志查看界面页面。

### 获取日志列表

**GET** `/api/logs/list`

查询参数支持按级别、时间范围过滤。

**响应示例**：
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

### 获取日志按生命体分组

**GET** `/api/logs/beings`

按硅基生命体分组的日志统计。

### 获取可用日志级别

**GET** `/api/logs/levels`

返回系统中可用的日志级别列表。

---

## 使用统计

### 使用统计页面

**GET** `/usage`

返回使用统计界面页面。

### 获取使用摘要

**GET** `/api/usage/summary`

返回 Token 使用量和费用摘要。

### 获取趋势数据

**GET** `/api/usage/trend`

查询参数：`startDate`, `endDate`

返回指定时间段内的使用趋势数据。

### 导出使用数据

**GET** `/api/usage/export`

导出使用数据为可下载格式。

---

## 审计跟踪

### 审计页面

**GET** `/audit`

返回审计跟踪界面页面。

### 获取审计列表

**GET** `/api/audit/list`

返回审计日志条目列表。

### 获取审计摘要

**GET** `/api/audit/summary`

返回审计数据的汇总统计。

### 获取审计按生命体分组

**GET** `/api/audit/beings`

按硅基生命体分组的审计统计。

---

## 配置管理

### 配置页面

**GET** `/config`

返回系统配置界面页面。

### 保存配置

**POST** `/config/save`

**请求体**：
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
    }
  }
}
```

### 获取 AI 配置选项

**GET** `/config/aioptions`

返回可用的 AI 客户端类型及其动态选项（可用模型、区域等）。

---

## 记忆系统

### 记忆页面

**GET** `/memory`

返回记忆管理界面页面。

### 获取记忆列表

**GET** `/api/memory/list`

返回硅基生命体的记忆条目列表。

### 获取记忆详情

**GET** `/api/memory/detail/{id}`

路径参数：`id` — 记忆条目 ID

返回指定记忆条目的完整内容。

### 获取记忆统计

**GET** `/api/memory/stats`

返回记忆系统的统计信息。

### 搜索记忆

**GET** `/api/memory/search`

查询参数：`keyword` — 搜索关键词

搜索匹配的记忆条目。

### 获取记忆按生命体分组

**GET** `/api/memory/beings`

按硅基生命体分组的记忆统计。

### 获取记忆追溯

**GET** `/api/memory/trace/{id}`

路径参数：`id` — 记忆条目 ID

返回指定记忆条目的来源追溯链。

### 获取记忆时间线 HTML

**GET** `/api/memory/timeline-html`

返回记忆时间线的 HTML 视图。

---

## 工作笔记

### 工作笔记页面

**GET** `/work-notes`

返回工作笔记界面页面。

### 获取工作笔记列表

**GET** `/api/work-notes/list`

返回工作笔记列表。

### 读取工作笔记

**GET** `/api/work-notes/read`

查询参数：`noteId` — 笔记 ID

返回指定笔记的内容。

### 获取笔记目录

**GET** `/api/work-notes/directory`

返回笔记目录结构。

### 搜索工作笔记

**GET** `/api/work-notes/search`

查询参数：`keyword` — 搜索关键词

搜索匹配的工作笔记。

### 创建工作笔记

**POST** `/api/work-notes/create`

**请求体**：
```json
{
  "title": "笔记标题",
  "content": "笔记内容",
  "keywords": ["关键词1", "关键词2"]
}
```

### 更新工作笔记

**POST** `/api/work-notes/update`

**请求体**：
```json
{
  "noteId": "note-uuid",
  "title": "更新后的标题",
  "content": "更新后的内容"
}
```

### 删除工作笔记

**POST** `/api/work-notes/delete`

**请求体**：
```json
{
  "noteId": "note-uuid"
}
```

---

## 知识网络

### 知识网络页面

**GET** `/knowledge`

返回知识网络管理界面页面。

### 获取知识图谱

**GET** `/api/knowledge/graph`

返回知识三元组图谱数据（主体-关系-客体）。

---

## 项目管理

### 项目页面

**GET** `/project`

返回项目管理界面页面。

### 项目工作笔记页面

**GET** `/project/{id}/work-notes`

路径参数：`id` — 项目 ID

返回指定项目的工作笔记页面。

### 项目任务页面

**GET** `/project/{id}/tasks`

路径参数：`id` — 项目 ID

返回指定项目的任务管理页面。

### 项目工具权限页面

**GET** `/project/{id}/tool-permissions`

路径参数：`id` — 项目 ID

返回指定项目的工具权限管理页面。

### 项目工作流页面

**GET** `/project/{id}/workflow`

路径参数：`id` — 项目 ID

返回指定项目的工作流管理页面。

### 获取项目工作流详情

**GET** `/api/projects/workflow-detail`

查询参数：`projectId` — 项目 ID

返回项目关联的工作流详情。

### 分配项目角色

**POST** `/api/projects/assign-role`

**请求体**：
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### 移除项目角色

**POST** `/api/projects/remove-role`

**请求体**：
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### 获取项目列表

**GET** `/api/projects/list`

返回所有项目的列表。

### 获取项目工作流模板列表

**GET** `/api/projects/list-workflow-templates`

返回可用的工作流模板列表。

### 创建项目

**POST** `/api/projects/create`

**请求体**：
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### 归档项目

**POST** `/api/projects/{id}/archive`

路径参数：`id` — 项目 ID

归档指定项目。

### 恢复项目

**POST** `/api/projects/{id}/restore`

路径参数：`id` — 项目 ID

恢复已归档的项目。

### 销毁项目

**POST** `/api/projects/{id}/destroy`

路径参数：`id` — 项目 ID

永久删除指定项目（不可恢复）。

### 获取项目详情

**GET** `/api/projects/detail`

查询参数：`projectId` — 项目 ID

返回项目的详细信息。

### 更新项目

**POST** `/api/projects/update`

**请求体**：
```json
{
  "projectId": "project-uuid",
  "name": "Updated Name",
  "description": "Updated description"
}
```

### 分配成员到项目

**POST** `/api/projects/assign`

**请求体**：
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### 从项目中移除成员

**POST** `/api/projects/remove`

**请求体**：
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### 获取项目工作笔记列表

**GET** `/api/projects/{id}/work-notes/list`

路径参数：`id` — 项目 ID

返回指定项目的工作笔记列表。

### 读取项目工作笔记

**GET** `/api/projects/{id}/work-notes/read`

路径参数：`id` — 项目 ID

返回指定项目的工作笔记内容。

### 创建项目工作笔记

**POST** `/api/projects/{id}/work-notes/create`

路径参数：`id` — 项目 ID

在指定项目中创建新的工作笔记。

### 更新项目工作笔记

**POST** `/api/projects/{id}/work-notes/update`

路径参数：`id` — 项目 ID

更新指定项目中的工作笔记。

### 删除项目工作笔记

**POST** `/api/projects/{id}/work-notes/delete`

路径参数：`id` — 项目 ID

删除指定项目中的工作笔记。

### 获取项目任务列表

**GET** `/api/projects/{id}/tasks/list`

路径参数：`id` — 项目 ID

返回指定项目的任务列表。

### 创建项目任务

**POST** `/api/projects/{id}/tasks/create`

路径参数：`id` — 项目 ID

在指定项目中创建新任务。

### 更新项目任务

**POST** `/api/projects/{id}/tasks/update`

路径参数：`id` — 项目 ID

更新指定项目中的任务。

### 删除项目任务

**POST** `/api/projects/{id}/tasks/delete`

路径参数：`id` — 项目 ID

删除指定项目中的任务。

### 分配任务负责人

**POST** `/api/projects/{id}/tasks/assign`

路径参数：`id` — 项目 ID

为项目任务分配负责人。

### 移除任务负责人

**POST** `/api/projects/{id}/tasks/remove-assignee`

路径参数：`id` — 项目 ID

移除项目任务的负责人。

### 标记任务完成

**POST** `/api/projects/{id}/tasks/complete`

路径参数：`id` — 项目 ID

标记项目任务为已完成。

### 标记任务失败

**POST** `/api/projects/{id}/tasks/fail`

路径参数：`id` — 项目 ID

标记项目任务为失败。

### 取消任务

**POST** `/api/projects/{id}/tasks/cancel`

路径参数：`id` — 项目 ID

取消项目任务。

---

## 工具权限管理

### 获取硅基生命体工具权限

**GET** `/api/beings/tool-permissions`

查询参数：`beingId` — 硅基生命体 ID

返回指定硅基生命体的工具权限配置。

### 更新硅基生命体工具权限

**PUT** `/api/beings/tool-permissions`

**请求体**：
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

### 获取工具权限模板

**GET** `/api/beings/tool-permissions/templates`

返回可用的工具权限模板列表。

### 应用工具权限模板

**POST** `/api/beings/tool-permissions/apply-template`

**请求体**：
```json
{
  "beingId": "being-uuid",
  "templateName": "readonly"
}
```

### 获取项目工具权限

**GET** `/api/projects/{id}/tool-permissions`

路径参数：`id` — 项目 ID

返回指定项目的工具权限配置。

### 更新项目工具权限

**PUT** `/api/projects/{id}/tool-permissions`

路径参数：`id` — 项目 ID

**请求体**：
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

## 执行器管理

### 执行器页面

**GET** `/executor`

返回执行器管理界面页面。

### 获取执行器状态

**GET** `/api/executors/status`

返回各执行器（磁盘、网络、命令行）的运行状态。

---

## 代码浏览器

### 代码浏览器页面

**GET** `/code`

返回代码浏览器界面页面。

### 获取代码类型列表

**GET** `/api/code/types`

返回支持的代码类型/语言列表。

### 获取代码详情

**GET** `/api/code/detail`

查询参数：`filePath`, `lineNumber`

返回指定文件的代码详情。

---

## 代码悬浮提示

### 获取悬浮提示

**GET** `/api/code/hover`
**POST** `/api/code/hover`

获取代码位置的悬浮提示信息（类似 IDE 的智能提示）。

### 注册代码位置

**POST** `/api/code/register`

注册需要监控的代码位置。

### 更新代码位置

**POST** `/api/code/update`

更新已注册的代码位置信息。

### 注销代码位置

**POST** `/api/code/unregister`

注销不再需要的代码位置监控。

---

## 帮助文档系统

### 帮助页面

**GET** `/help` 或 **GET** `/help/index`

返回帮助文档主页。

### 帮助主题页面

**GET** `/help/{topic}`

路径参数：`topic` — 主题标识符

返回指定主题的帮助文档页面。

### 搜索帮助文档

**GET** `/api/help/search`

查询参数：`keyword` — 搜索关键词

搜索匹配的帮助文档主题。

---

## 初始化

### 初始化向导页面

**GET** `/init`

返回首次运行初始化向导页面。

### 提交初始化

**POST** `/init`

提交首次运行的初始化配置。

### 浏览选择数据目录

**GET** `/init/browse`

打开目录浏览器以选择数据存储位置。

### 获取 AI 配置元数据

**GET** `/init/ai-config-metadata`

返回可用的 AI 客户端类型及其配置字段元数据。

---

## 系统控制

### 优雅关闭

**POST** `/api/system/shutdown`

> **注意**：仅允许来自 localhost 的请求

触发应用程序的优雅关闭流程：

1. 停止主循环（MainLoop）
2. 保存当前配置
3. 关闭 HTTP 监听器

**响应**：
```json
{
  "status": "shutting_down",
  "message": "Application is shutting down gracefully"
}
```

---

## 关于

### 关于页面

**GET** `/about`

返回关于页面，包含系统信息和已加载的插件列表。

**插件列表数据**：
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

## 错误响应

所有端点返回标准化的错误响应：

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: FileAccess, Denied by GlobalACL"
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

## AI 客户端接口

### IAIClient 接口

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

| 属性 | 类型 | 描述 |
|------|------|------|
| `Endpoint` | `string` | AI 服务端点 URL |
| `DefaultModel` | `string` | 默认模型名称 |
| `StreamingMode` | `bool?` | 流式模式：true=仅流式、false=仅非流式、null=两种均支持 |
| `SupportsToolCalls` | `bool?` | 工具调用支持：true=支持、false=不支持（跳过工具注入）、null=未知 |
| `ContextWindowTokens` | `int?` | 上下文窗口大小（token 数），用于 token 预算裁剪 |
| `SupportsVision` | `bool?` | 视觉输入支持：true=支持图片、false=不支持、null=未知 |
| `SupportsAudio` | `bool?` | 音频输入支持：true=支持音频、false=不支持、null=未知 |

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

## 工具系统接口

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
