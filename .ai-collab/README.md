# .ai-collab/ — 多 AI 协作框架

## 设计目标

本目录用于支持 **人类轮换使用多个 AI IDE + 常驻 AI** 的协作模式，无需文件锁，通过**意图驱动的异步合并**实现协同。

## 核心假设

1. **时间序列隔离**：人类不会同时打开多个 AI IDE，修改不会并发发生
2. **Git 是真相源**：所有代码变更最终通过 Git 追踪
3. **AI 间接通信**：AI 之间不直接通信，通过文件交换上下文
4. **人类是仲裁者**：冲突无法自动解决时，由人类决策

## 目录结构

```
.ai-collab/
├── README.md              ← 本文件（AI 必读）
├── registry.json          ← AI 注册表（已知参与者）
├── state.json             ← 当前状态（运行时）
├── activity.log           ← 操作日志（所有 AI 追加）
├── changes/               ← 每次会话的修改记录
│   └── <actor>-<timestamp>.json
├── sessions/              ← 活跃/历史会话状态
│   └── <actor>-<timestamp>.json
└── merge-reports/         ← 合并报告（需要人类确认时）
    └── merge-<timestamp>.json
```

---

## AI 生命周期协议

### 阶段 1：启动/会话开始

**触发条件**：人类打开 AI IDE 或与常驻 AI 交互

**AI 必须执行**：

1. **读取 registry.json**
   - 确认自己的 `actorId` 在注册表中
   - **如果不在 → 停止执行，提示人类授权**
     ```
     提示："我检测到自己的 actorId 未在 registry.json 中注册。
          请人类手动添加我至 .ai-collab/registry.json 并 commit，
          完成后我才能参与多 AI 协作。"
     ```

2. **读取 state.json**
   - 检查 `lastSession` 了解上一个会话是谁
   - 检查是否有未处理的 `handoff` 任务

3. **读取 activity.log 最新 10 条**
   - 了解最近发生什么

4. **读取 changes/ 最新 2-3 个文件**
   - 了解其他 AI 最近修改了什么

5. **创建会话文件**
   ```json
   // sessions/<actorId>-<timestamp>.json
   {
     "sessionId": "cursor-20260513-0900",
     "actorId": "cursor-gpt4",
     "startTime": "2026-05-13T09:00+08:00",
     "endTime": null,
     "status": "active",
     "lockedFiles": [],
     "modifiedFiles": [],
     "intent": "人类开始新任务",
     "handoffFrom": null
   }
   ```

6. **更新 state.json**
   ```json
   {
     "currentActor": "cursor-gpt4",
     "currentSession": "cursor-20260513-0900",
     "lastUpdate": "2026-05-13T09:00+08:00"
   }
   ```

7. **追加 activity.log**
   ```
   2026-05-13T09:00:00+08:00 | cursor-gpt4 | session_start | sessionId=cursor-20260513-0900
   ```

---

### 阶段 2：工作期间

**AI 可以执行**：

1. **修改代码**
   - 正常编辑文件
   - Git commit 时使用规范格式：
     ```
     [actorId] 类型：描述
     
     意图：<一句话描述修改目的>
     相关文件：<文件列表>
     ```

2. **更新会话文件**（可选，长任务建议更新）
   - 添加 `modifiedFiles`
   - 更新 `intent`

3. **追加 activity.log**（关键操作时）
   ```
   2026-05-13T09:15:00+08:00 | cursor-gpt4 | file_modified | src/login.cs
   ```

---

### 阶段 3：会话结束/交接

**触发条件**：人类完成当前任务、关闭 AI IDE、或需要切换 AI

**AI 必须执行**：

1. **创建修改记录**
   ```json
   // changes/<actorId>-<timestamp>.json
   {
     "sessionId": "cursor-20260513-0900",
     "actorId": "cursor-gpt4",
     "startTime": "2026-05-13T09:00+08:00",
     "endTime": "2026-05-13T09:30+08:00",
     "files": [
       {
         "path": "src/login.cs",
         "intent": "添加登录核心逻辑",
         "gitDiff": "git diff abc123 def456 -- src/login.cs",
         "changeSummary": {
           "additions": 45,
           "deletions": 5,
           "modifications": 3
         }
       }
     ],
     "commitHash": "def456",
     "handoff": {
       "nextActor": "openclaw-main",
       "pendingTasks": [
         { "type": "write_tests", "target": "src/login.cs" },
         { "type": "update_docs", "target": "docs/api.md" }
       ],
       "notes": "登录功能核心逻辑完成，待补充测试和文档"
     }
   }
   ```

2. **更新会话文件**
   ```json
   {
     "endTime": "2026-05-13T09:30+08:00",
     "status": "completed",
     "changeRecord": "changes/cursor-20260513-0900.json"
   }
   ```

3. **更新 state.json**
   ```json
   {
     "currentActor": null,
     "lastSession": "cursor-20260513-0900",
     "lastUpdate": "2026-05-13T09:30+08:00",
     "pendingHandoff": {
       "from": "cursor-gpt4",
       "to": "openclaw-main",
       "tasks": ["write_tests", "update_docs"]
     }
   }
   ```

4. **追加 activity.log**
   ```
   2026-05-13T09:30:00+08:00 | cursor-gpt4 | session_end | handoff=openclaw-main
   ```

---

### 阶段 4：常驻 AI 处理交接

**触发条件**：常驻 AI 轮询检测到 `pendingHandoff`

**AI 必须执行**：

1. **读取 handoff 任务**
   - 检查 `state.json.pendingHandoff`
   - 确认 `nextActor` 是自己

2. **分析变更意图**
   - 读取 `changes/` 最新文件
   - 理解人类/AI IDE 完成了什么

3. **执行任务**
   - 按 `pendingTasks` 列表执行
   - 每个任务完成后 Git commit

4. **创建自己的修改记录**
   - 同阶段 3 的格式

5. **更新 state.json**
   - 清除 `pendingHandoff`
   - 记录自己的会话

6. **追加 activity.log**

---

### 阶段 5：冲突检测与合并

**触发条件**：常驻 AI 轮询检测到多个会话修改了同一文件

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
           { "id": "cursor-0900", "intent": "添加登录逻辑", "lines": [30-50] },
           { "id": "windsurf-0930", "intent": "添加验证逻辑", "lines": [45-60] }
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
      "id": "cursor-gpt4",
      "type": "ai-ide",
      "platform": "Cursor",
      "model": "GPT-4",
      "description": "Cursor 默认 AI",
      "runtimeEnv": {
        "host": null,
        "os": "Windows 11 / macOS",
        "node": null,
        "shell": "PowerShell / bash",
        "workspace": "C:\\Projects\\... 或 ~/Projects/...",
        "timezone": "Asia/Shanghai",
        "channel": "vscode-cursor"
      }
    },
    {
      "id": "openclaw-main",
      "type": "resident-ai",
      "platform": "OpenClaw",
      "model": "gateway/jarvis",
      "description": "常驻 AI，负责监控和收尾",
      "runtimeEnv": {
        "host": "iZwz92nm2lvnbib5tctc94Z",
        "os": "Linux 5.15.0-144-generic (x64)",
        "node": "v24.14.0",
        "shell": "bash",
        "workspace": "/home/admin/openclaw/workspace",
        "timezone": "Asia/Shanghai",
        "channel": "jvsclaw"
      }
    }
  ],
  "rules": {
    "defaultMergeStrategy": "auto-if-no-conflict",
    "humanReviewRequired": ["core-logic", "config-files"],
    "autoMergeAllowed": ["tests", "docs", "comments"]
  }
}
```

**`runtimeEnv` 字段说明**：

| 字段 | 说明 | 示例 |
|------|------|------|
| `host` | 主机名/实例 ID | `iZwz92nm2lvnbib5tctc94Z` |
| `os` | 操作系统及版本 | `Linux 5.15.0-144-generic (x64)` |
| `node` | Node.js 版本 | `v24.14.0` |
| `shell` | 默认 Shell | `bash`, `PowerShell` |
| `workspace` | 工作区路径 | `/home/admin/openclaw/workspace` |
| `timezone` | 时区 | `Asia/Shanghai` |
| `channel` | 通信渠道 | `jvsclaw`, `vscode-cursor` |

**用途**：
- 帮助 AI 理解运行环境差异（路径分隔符、命令兼容性等）
- 交接时告知下一个 AI 运行环境信息
- 生成环境相关的配置建议

### state.json — 当前状态

```json
{
  "currentActor": null,
  "currentSession": null,
  "lastSession": "cursor-20260513-0900",
  "lastUpdate": "2026-05-13T09:30+08:00",
  "pendingHandoff": null,
  "activeLocks": [],
  "pendingMergeReports": []
}
```

### activity.log — 操作日志

```
# 格式：timestamp | actorId | eventType | details
2026-05-13T09:00:00+08:00 | cursor-gpt4 | session_start | sessionId=cursor-20260513-0900
2026-05-13T09:15:00+08:00 | cursor-gpt4 | file_modified | src/login.cs
2026-05-13T09:30:00+08:00 | cursor-gpt4 | session_end | handoff=openclaw-main
2026-05-13T10:00:00+08:00 | openclaw-main | handoff_received | from=cursor-gpt4
2026-05-13T10:30:00+08:00 | openclaw-main | session_end | completed=write_tests
```

---

## Git 集成

### Commit 消息规范

```
[actorId] 类型：简短描述

意图：一句话描述修改目的
会话：<sessionId>
相关文件：
- src/xxx.cs
- tests/xxx.test.cs

---
变更统计：+XX -XX
```

**示例**：
```
[cursor-gpt4] feat: 添加登录功能

意图：实现用户登录核心逻辑
会话：cursor-20260513-0900
相关文件：
- src/Auth/Login.cs

---
变更统计：+45 -5
```

### .gitignore 建议

```gitignore
# .ai-collab/ 部分文件需要追踪，部分不需要

# 追踪（重要元数据）
!.ai-collab/README.md
!.ai-collab/registry.json
!.ai-collab/changes/
!.ai-collab/merge-reports/

# 不追踪（运行时状态）
.ai-collab/state.json
.ai-collab/sessions/
.ai-collab/activity.log
```

---

## 人类操作指南

### 切换 AI IDE 时

1. **完成当前任务**
2. **告诉当前 AI**："创建交接记录，下一个 AI 是 XXX"
3. **关闭当前 AI IDE**
4. **打开新 AI IDE**
5. **告诉新 AI**："读取 .ai-collab/ 了解上下文，继续 XXX 任务"

### 处理合并冲突时

1. **收到通知**（常驻 AI 发送）
2. **读取 merge-reports/ 最新报告**
3. **回复选项**（A/B/C/D）
4. **等待 AI 执行**

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

**AI 行为规范：**
- AI 检测到自己的 actorId 不在 registry.json 中 → 提示人类添加，不得自行修改
- AI 检测到未知 actorId 的活动 → 通知人类确认是否授权

---

## 故障恢复

### 会话文件残留

```
检测：sessions/ 中有 session 超过 2 小时未更新
处理：常驻 AI 发送通知 → 人类确认 → 标记为 abandoned
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
| v1.0 | 2026-05-13 | 初始版本 |

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

**Q: 人类可以手动编辑这些文件吗？**

A: 可以，但建议通过 AI 操作以保持格式一致。
