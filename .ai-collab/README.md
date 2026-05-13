# .ai-collab/ — 多 AI 协作框架（任务列表驱动）

## 设计目标

本目录用于支持 **多 AI 以不同方式接力协作** 的模式，通过**任务列表驱动**实现协同。

## 核心理念

> **不是 handoff 交接，而是任务列表接力。**
> 
> 所有 AI 都在处理同一个项目，各自以不同的方式驱动进展。

## 核心假设

1. **任务为中心**：任务是第一公民，AI 是执行者
2. **Git 是真相源**：所有代码变更最终通过 Git 追踪
3. **AI 间接通信**：AI 之间通过任务列表和文件交换上下文
4. **人类是仲裁者**：冲突无法自动解决时，由人类决策

---

## 目录结构

```
.ai-collab/
├── README.md              ← 本文件（AI 必读）
├── registry.json          ← AI 注册表（已知参与者）
├── state.json             ← 当前状态（运行时）
├── tasks.json             ← 任务列表（核心驱动）
├── activity.log           ← 操作日志（所有 AI 追加）
├── changes/               ← 每次会话的修改记录
│   └── <actor>-<timestamp>.json
├── sessions/              ← 活跃/历史会话状态
│   └── <actor>-<timestamp>.json
└── merge-reports/         ← 合并报告（需要人类确认时）
    └── merge-<timestamp>.json
```

---

## 任务列表驱动模式

### tasks.json 结构

```json
{
  "version": "1.0",
  "lastUpdated": "2026-05-14T00:17:00+08:00",
  "tasks": [
    {
      "id": "task-001",
      "type": "code_review",
      "target": "src/SiliconLife.Core/Runtime/MainLoop.cs",
      "title": "修复 Watchdog 竞态条件",
      "description": "WatchdogRun 方法中存在竞态条件，需要添加锁保护",
      "priority": "P0|P1|P2|P3",
      "status": "pending|inProgress|completed|cancelled",
      "createdAt": "2026-05-14T00:15:00+08:00",
      "createdBy": "openclaw-main",
      "assignedTo": null,
      "relatedCommit": null,
      "completedAt": null
    }
  ],
  "taskIndex": {
    "pending": ["task-001"],
    "inProgress": [],
    "completed": [],
    "cancelled": []
  }
}
```

### 任务优先级定义

| 优先级 | 说明 | 响应时间 |
|--------|------|----------|
| **P0** | 严重 bug，阻塞性问题 | 立即处理 |
| **P1** | 重要功能/改进 | 当前会话处理 |
| **P2** | 一般改进，优化 | 有空时处理 |
| **P3** | 可选，锦上添花 | 长期待办 |

---

## AI 生命周期协议

### 阶段 1：启动/会话开始

**触发条件**：人类打开 AI IDE 或与常驻 AI 交互

**AI 必须执行**：

1. **读取 registry.json**
   - 确认自己的 `actorId` 在注册表中
   - **如果不在 → 停止执行，提示人类授权**

2. **读取 tasks.json**
   - 筛选 `assignedTo=自己的 actorId` 或 `assignedTo=null` 的任务
   - 按优先级排序，选择最高优先级的任务

3. **读取 state.json**
   - 检查 `currentActor` 是否有活跃会话
   - 了解项目当前状态

4. **读取 activity.log 最新 10 条**
   - 了解最近发生什么

5. **读取 changes/ 最新 2-3 个文件**
   - 了解其他 AI 最近修改了什么

6. **创建会话文件**
   ```json
   // sessions/<actorId>-<timestamp>.json
   {
     "sessionId": "cursor-20260513-0900",
     "actorId": "cursor-gpt4",
     "startTime": "2026-05-13T09:00+08:00",
     "endTime": null,
     "status": "active",
     "selectedTask": "task-001",
     "modifiedFiles": []
   }
   ```

7. **更新 state.json**
   ```json
   {
     "currentActor": "cursor-gpt4",
     "currentSession": "cursor-20260513-0900",
     "lastUpdate": "2026-05-13T09:00+08:00"
   }
   ```

8. **追加 activity.log**
   ```
   2026-05-13T09:00:00+08:00 | cursor-gpt4 | session_start | sessionId=cursor-20260513-0900 | task=task-001
   ```

---

### 阶段 2：工作期间

**AI 可以执行**：

1. **领取任务**
   - 更新 `tasks.json` 中任务状态为 `inProgress`
   - 更新 `taskIndex`，将任务从 `pending` 移到 `inProgress`
   - 设置 `assignedTo=自己的 actorId`

2. **修改代码**
   - 正常编辑文件
   - Git commit 时使用规范格式：
     ```
     [actorId] 类型：描述 (ref task-001)
     
     意图：<一句话描述修改目的>
     会话：<sessionId>
     相关文件：
     - src/xxx.cs
     ```

3. **更新 tasks.json**（任务完成后）
   - 设置 `status=completed`
   - 填写 `completedAt`
   - 更新 `taskIndex`，将任务从 `inProgress` 移到 `completed`
   - 填写 `relatedCommit`

4. **创建新任务**（发现新问题时）
   - 添加到 `tasks` 数组
   - 更新 `taskIndex.pending`
   - 更新 `lastUpdated`

5. **追加 activity.log**（关键操作时）
   ```
   2026-05-13T09:15:00+08:00 | cursor-gpt4 | task_start | task=task-001
   2026-05-13T09:30:00+08:00 | cursor-gpt4 | task_complete | task=task-001 | commit=abc123
   ```

---

### 阶段 3：会话结束

**触发条件**：人类完成当前任务、关闭 AI IDE

**AI 必须执行**：

1. **创建修改记录**
   ```json
   // changes/<actorId>-<timestamp>.json
   {
     "sessionId": "cursor-20260513-0900",
     "actorId": "cursor-gpt4",
     "startTime": "2026-05-13T09:00+08:00",
     "endTime": "2026-05-13T09:30:00+08:00",
     "files": [
       {
         "path": "src/login.cs",
         "intent": "添加登录核心逻辑",
         "gitDiff": "git diff abc123 def456 -- src/login.cs",
         "changeSummary": {
           "additions": 45,
           "deletions": 5,
           "modifications": 3
         },
         "relatedTask": "task-001"
       }
     ],
     "commitHash": "def456",
     "tasksCompleted": ["task-001"],
     "tasksCreated": ["task-002", "task-003"]
   }
   ```

2. **更新会话文件**
   ```json
   {
     "endTime": "2026-05-13T09:30:00+08:00",
     "status": "completed",
     "changeRecord": "changes/cursor-20260513-0900.json"
   }
   ```

3. **更新 state.json**
   ```json
   {
     "currentActor": null,
     "lastSession": "cursor-20260513-0900",
     "lastUpdate": "2026-05-13T09:30:00+08:00"
   }
   ```

4. **追加 activity.log**
   ```
   2026-05-13T09:30:00+08:00 | cursor-gpt4 | session_end | completed=task-001
   ```

---

### 阶段 4：常驻 AI 监控

**触发条件**：常驻 AI 定期轮询

**AI 必须执行**：

1. **检查 tasks.json**
   - 筛选 `assignedTo=null` 的 pending 任务
   - 按优先级排序，领取最高优先级任务

2. **检查 sessions/**
   - 检测是否有 session 超过 2 小时未更新
   - 标记为 `abandoned` 并通知人类

3. **检查 merge-reports/**
   - 处理待确认的合并报告
   - 通知人类需要决策

4. **更新 activity.log**
   ```
   2026-05-13T10:00:00+08:00 | openclaw-main | poll | pending_tasks=3
   2026-05-13T10:00:00+08:00 | openclaw-main | task_claim | task=task-002
   ```

---

### 阶段 5：冲突检测与合并

**触发条件**：检测到多个会话修改了同一文件

**AI 必须执行**：

1. **检测重叠**
   - 比较 `changes/` 中各文件的修改行
   - 识别 Git 冲突

2. **分析意图兼容性**
   ```
   意图组合              合并策略
   ─────────────────────────────────────
   功能 + 测试          → 自动合并
   功能 + 文档          → 自动合并
   功能 + 功能 (不同区域) → 自动合并
   功能 + 功能 (重叠区域) → 需要确认
   重构 + 修复 bug      → 需要确认
   ```

3. **决策**
   - 可自动合并 → 执行合并 + commit
   - 需要确认 → 创建 merge-report

4. **创建合并报告**（需要确认时）
   ```json
   // merge-reports/merge-20260513-1000.json
   {
     "reportId": "merge-20260513-1000",
     "timestamp": "2026-05-13T10:00+08:00",
     "status": "pending_human_review",
     "conflicts": [
       {
         "file": "src/login.cs",
         "sessions": [
           { "id": "cursor-0900", "intent": "添加登录逻辑", "lines": [30-50], "task": "task-001" },
           { "id": "windsurf-0930", "intent": "添加验证逻辑", "lines": [45-60], "task": "task-004" }
         ],
         "overlap": [45-50],
         "recommendation": "需要人类确认合并策略"
       }
     ],
     "autoMerged": [
       { "file": "tests/login.test.cs", "sessions": ["cursor-0900", "openclaw-1000"] }
     ]
   }
   ```

5. **通知人类**
   - 通过消息/通知告知有合并报告待确认
   - 提供清晰的选项（A/B/C/D）

---

## 文件规范

### registry.json — AI 注册表

```json
{
  "knownActors": [
    {
      "id": "human",
      "type": "human",
      "name": "用户 2682",
      "runtimeEnv": null
    },
    {
      "id": "openclaw-main",
      "type": "resident-ai",
      "platform": "OpenClaw",
      "model": "gateway/jarvis",
      "description": "常驻 AI，负责监控、任务协调和自动合并",
      "runtimeEnv": {
        "host": "iZwz92nm2lvnbib5tctc94Z",
        "os": "Linux 5.15.0-144-generic (x64)",
        "node": "v24.14.0",
        "shell": "bash",
        "workspace": "/home/admin/openclaw/workspace",
        "timezone": "Asia/Shanghai",
        "channel": "jvsclaw"
      }
    },
    {
      "id": "trae-glm5",
      "type": "ai-ide",
      "platform": "Trae",
      "model": "GLM-5.1",
      "description": "Trae IDE AI，负责代码开发与协作",
      "runtimeEnv": {
        "host": "PURELIGHT-WJX",
        "os": "Windows NT 10.0.26200.0 (x64)",
        "node": "v24.14.0",
        "shell": "PowerShell",
        "workspace": "D:\\SiliconLifeCollective",
        "timezone": "Asia/Shanghai",
        "channel": "trae-ide"
      }
    }
  ],
  "rules": {
    "defaultMergeStrategy": "auto-if-no-conflict",
    "humanReviewRequired": ["core-logic", "config-files", "architecture"],
    "autoMergeAllowed": ["tests", "docs", "comments", "formatting"],
    "newActorAuthorization": "human-only"
  },
  "authorizationPolicy": {
    "newActorAddition": "必须由人类手动编辑 registry.json 并 commit，AI 不得自行添加",
    "aiBehavior": "AI 检测到未知 actorId 时，应提示人类添加至注册表，而非自行操作"
  }
}
```

### state.json — 当前状态

```json
{
  "currentActor": null,
  "currentSession": null,
  "lastSession": "cursor-20260513-0900",
  "lastUpdate": "2026-05-13T09:30:00+08:00",
  "activeLocks": [],
  "pendingMergeReports": []
}
```

### activity.log — 操作日志

```
# 格式：timestamp | actorId | eventType | details
2026-05-13T09:00:00+08:00 | cursor-gpt4 | session_start | sessionId=cursor-20260513-0900 | task=task-001
2026-05-13T09:15:00+08:00 | cursor-gpt4 | task_start | task=task-001
2026-05-13T09:15:00+08:00 | cursor-gpt4 | file_modified | src/login.cs
2026-05-13T09:30:00+08:00 | cursor-gpt4 | task_complete | task=task-001 | commit=def456
2026-05-13T09:30:00+08:00 | cursor-gpt4 | session_end | completed=task-001
2026-05-13T10:00:00+08:00 | openclaw-main | poll | pending_tasks=3
2026-05-13T10:00:00+08:00 | openclaw-main | task_claim | task=task-002
```

### tasks.json — 任务列表

```json
{
  "version": "1.0",
  "lastUpdated": "2026-05-14T00:17:00+08:00",
  "tasks": [
    {
      "id": "task-001",
      "type": "code_review|bugfix|feature|refactor|docs|test|config",
      "target": "src/SiliconLife.Core/Runtime/MainLoop.cs",
      "title": "修复 Watchdog 竞态条件",
      "description": "详细描述...",
      "priority": "P0|P1|P2|P3",
      "status": "pending|inProgress|completed|cancelled",
      "createdAt": "2026-05-14T00:15:00+08:00",
      "createdBy": "openclaw-main",
      "assignedTo": null,
      "relatedCommit": "abc123",
      "completedAt": "2026-05-14T00:30:00+08:00"
    }
  ],
  "taskIndex": {
    "pending": ["task-002", "task-003"],
    "inProgress": ["task-001"],
    "completed": ["task-000"],
    "cancelled": []
  }
}
```

---

## Git 集成

### Commit 消息规范

```
[actorId] 类型：简短描述 (ref task-001)

意图：一句话描述修改目的
会话：<sessionId>
任务：<taskId>
相关文件：
- src/xxx.cs
- tests/xxx.test.cs

---
变更统计：+XX -XX
```

**示例**：
```
[cursor-gpt4] fix: 修复 Watchdog 竞态条件 (ref task-001)

意图：添加锁保护防止竞态条件
会话：cursor-20260513-0900
任务：task-001
相关文件：
- src/SiliconLife.Core/Runtime/MainLoop.cs

---
变更统计：+15 -3
```

### .gitignore 建议

```gitignore
# .ai-collab/ 部分文件需要追踪，部分不需要

# 追踪（重要元数据）
!.ai-collab/README.md
!.ai-collab/registry.json
!.ai-collab/tasks.json
!.ai-collab/changes/
!.ai-collab/merge-reports/
!.ai-collab/state.json
!.ai-collab/sessions/
!.ai-collab/activity.log

# 注意：*.log 规则会屏蔽 activity.log，需加例外
```

---

## 人类操作指南

### 添加新任务

1. **编辑 tasks.json**
   ```json
   {
     "tasks": [
       {
         "id": "task-005",
         "type": "feature",
         "target": "src/xxx.cs",
         "title": "实现 XXX 功能",
         "priority": "P1",
         "status": "pending",
         "assignedTo": null
       }
     ]
   }
   ```

2. **Git commit**
   ```bash
   git add .ai-collab/tasks.json
   git commit -m "task: 添加 task-005 - 实现 XXX 功能"
   ```

3. **通知 AI**
   - 告诉 AI："有新任务 task-005，请查看 tasks.json"

### 切换 AI IDE 时

1. **完成当前任务**（或标记为 inProgress）
2. **关闭当前 AI IDE**
3. **打开新 AI IDE**
4. **告诉新 AI**："读取 .ai-collab/ 了解上下文，从任务列表领取任务"

### 添加新 AI IDE 时（人类授权）

**重要：新 AI IDE 必须由人类手动添加并授权，AI 不得自行添加。**

1. **人类编辑 registry.json**
   ```json
   {
     "knownActors": [
       // ... 现有条目 ...
       {
         "id": "cursor-gpt4",
         "type": "ai-ide",
         "platform": "Cursor",
         "model": "GPT-4",
         "description": "Cursor 默认 AI"
       }
     ]
   }
   ```

2. **人类 Git commit**
   ```bash
   git add .ai-collab/registry.json
   git commit -m "auth: 添加新 AI IDE - cursor-gpt4"
   ```

3. **通知 AI IDE**
   - 告诉新 AI IDE："你已添加到 .ai-collab/registry.json，可以开始工作"
   - AI IDE 启动时读取 registry.json 确认自己的身份

---

## 故障恢复

### 会话文件残留

```
检测：sessions/ 中有 session 超过 2 小时未更新
处理：常驻 AI 标记为 abandoned，释放相关任务回 pending
```

### 任务长时间未完成

```
检测：task 状态为 inProgress 超过 4 小时
处理：常驻 AI 通知人类确认，或重置为 pending
```

### activity.log 过大

```
检测：文件超过 10MB
处理：常驻 AI 归档旧日志 → activity-YYYY-MM.log
```

### state.json 损坏

```
检测：JSON 解析失败
处理：从 activity.log 最新条目重建
```

---

## 版本历史

| 版本 | 日期 | 变更 |
|------|------|------|
| v2.0 | 2026-05-14 | 移除 handoff 模式，改为任务列表驱动 |
| v1.0 | 2026-05-13 | 初始版本（handoff 交接模式） |

---

## 常见问题

**Q: AI IDE 不主动读取这些文件怎么办？**

A: 人类在对话中提供上下文，或配置 AI IDE 插件自动读取。

**Q: 新 AI IDE 如何获得授权？**

A: 人类手动编辑 registry.json 添加新 AI 的 actorId，然后 Git commit。AI 不得自行添加。

**Q: AI 发现自己不在注册表中怎么办？**

A: AI 应提示人类："我未在 registry.json 中注册，请人类添加并授权我使用此协作框架。"

**Q: 常驻 AI 多久轮询一次？**

A: 建议 30 秒 -5 分钟，根据任务紧急程度调整。

**Q: 如何防止 AI 覆盖彼此的修改？**

A: 时间序列隔离 + Git 历史 + 意图分析，冲突时通知人类。

**Q: 任务列表和 handoff 有什么区别？**

A: handoff 是一对一交接，任务列表是所有 AI 共同从池中领取任务。任务列表更灵活，支持多 AI 并行/接力。

**Q: 人类可以手动编辑这些文件吗？**

A: 可以，但建议通过 AI 操作以保持格式一致。
