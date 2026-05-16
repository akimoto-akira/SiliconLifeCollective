# ThinkOnProject 设计方案

> 日期: 2026-05-17 | 作者: coze-agent | 状态: 待讨论

## 一、设计背景

### 当前问题

1. 项目创建时如果没选工作流模板，项目就没人驱动，群聊空转
2. 群聊没有触发时机——没有东西让硅基人在群里说话
3. 项目系统"做了容器但没做流程"
4. 典型场景：一个老师跟主理人说"给我做个背单词网站"，主理人应该自己创建项目、拆任务、驱动执行，但现在做不到

### 已有基础设施

- 工作流引擎完整（`WorkflowEngine`、`WorkflowTickObject`、`CodeReviewWorkflow` 等模板）
- 任务系统完整（`TaskCenter`、`TaskEnumerator`）
- 群聊系统 90%（缺决策逻辑）
- 广播系统完整
- 主理人角色已有（`IsCurator` 属性，`CuratorTool`、`ProjectTool` 等工具）
- `BeingActivity.Project` 枚举值已存在（`SiliconBeingBase.cs:42`）
- `ThinkOnProject` 曾存在但已移除（`DefaultSiliconBeing.cs:324-329` 注释说明）

### 移除原因回顾

`DefaultSiliconBeing.cs:324-329` 的注释写道：

```csharp
// Project work: query project-related tasks from TaskCenter instead of ThinkOnProject
// This has been replaced by the centralized task management strategy:
// - Project scenarios are now handled through TaskCenter
// - Project-related tasks are queried from TaskCenter
// - Being uses ThinkOnTask to handle project tasks
// ThinkOnProject has been removed to simplify the Tick scheduling logic
// Project work is now handled through the regular task processing pipeline
```

这个简化是合理的——在当时的阶段，项目没有"谁来做决策"的问题，因为工作流模板解决了一切。但现在需求变了：无模板项目需要有人驱动，而 `ThinkOnTask` 只能执行已有任务，不能创建任务。

## 二、编排方式互斥模型

```
项目创建
  |
  +-- 有工作流模板？
       +-- 是 --> 工作流驱动（程序化，严格按模板执行）
       |          - WorkflowTickObject 每60秒驱动状态转移
       |          - AI 只执行模板 Action 定义的任务
       |          - 状态转移由 Condition 精确判定
       |          - AI 不可越权决策（模板是程序，必须服从）
       |
       +-- 否 --> 主理人驱动（涌现式，ThinkOnProject）
                  - 主理人在 Tick 时主动检查项目状态
                  - AI 自己判断下一步该做什么
                  - 产出同样是任务池中的任务
                  - 通过群聊协调团队成员
  |
  +-- 产出：都是任务
```

### 关键约束

- 有模板的项目，主理人不得触发 `ThinkOnProject`
- 模板执行完毕后，如果项目未关闭，也不自动切换为主理人驱动（模板是程序，程序结束就是结束）
- 如需切换编排方式，必须由人类显式操作（修改项目设置）
- 两种编排方式的产出一致：都往 `TaskCenter` 的任务池塞任务，硅基人通过同一套 `ThinkOnTask` 逻辑执行

### 为什么不是降级关系

工作流驱动和主理人驱动是两种平等的编排策略，各自适用不同场景：

| 维度 | 工作流驱动 | 主理人驱动 |
|------|-----------|-----------|
| 决策者 | 模板定义的条件和动作 | 主理人 AI |
| 确定性 | 高（程序化执行） | 中（AI 判断） |
| 灵活性 | 低（需预先定义） | 高（可应对未预期情况） |
| 适合场景 | 标准化流程（代码审查、发布） | 探索性项目（做个网站、写个文档） |
| 错误模式 | 流程卡在未定义的状态 | AI 判断失误或遗漏 |

## 三、ThinkOnProject 触发逻辑

不是每个 Tick 都想，太烧 token。需要明确的触发条件。

### 触发条件

1. **项目刚创建且无模板** -- 立即触发一次（项目初始化）
2. **项目下所有已分配任务都完成** -- 触发（推进下一步）
3. **项目群聊有新消息且一段时间无人响应** -- 触发（主理人该说话了）
4. **周期性兜底** -- 每隔 N 分钟检查有任务但卡住的项目

### 在 DefaultSiliconBeing.Tick() 中的位置

当前 Tick 优先级链（`DefaultSiliconBeing.cs:162-346`）：

```
1. AI config change (CheckAndRebuildAIClient)
2. Consecutive error check
3. Continuation (ThinkContinuation)
4. Pending messages (ThinkOnChat / ThinkOnGroupChat)
5. Broadcast processing
6. Timer processing (ThinkOnTimer)
7. Task continuation (ThinkOnTask)
8. Task execution (ThinkOnTask)
9. [当前为注释占位] Project work
10. Memory compression (ThinkOnMemoryCompress)
```

`ThinkOnProject` 插入位置：在任务执行之后、记忆压缩之前，即第 9 步替换当前的注释占位。原因：

- 主理人先完成自己的任务（第 7-8 步），再考虑项目整体推进
- 记忆压缩始终是最低优先级
- 位置与原有 `BeingActivity.Project` 枚举值和代码注释完全对齐

### 触发前置判断

```csharp
// src/SiliconLife.Common/SiliconBeing/DefaultSiliconBeing.cs
// 替换第324-330行的注释块

if (IsCurator && HasProjectsWithoutTemplate())
{
    _activityRaw = (int)BeingActivity.Project;
    _logger.Info(Id, "Being {0}: checking projects without workflow template", Name);
    if (!ExecuteBrain("ThinkOnProject", null, brain => brain.ThinkOnProject()))
        errorOccurred = true;
    return;
}
```

`HasProjectsWithoutTemplate()` 的实现：

```csharp
// src/SiliconLife.Common/SiliconBeing/DefaultSiliconBeing.cs

private bool HasProjectsWithoutTemplate()
{
    var projectManager = ServiceLocator.Instance.GetService<IProjectManager>();
    if (projectManager == null) return false;

    var projects = projectManager.ListProjects(includeArchived: false);
    foreach (var project in projects)
    {
        // 只关注自己创建的、无模板的、活跃的项目
        if (project.CreatedBy == Id
            && string.IsNullOrEmpty(project.WorkflowTemplateName)
            && project.Status == ProjectStatus.Active)
        {
            // 检查是否满足触发条件
            if (ShouldThinkOnProject(project))
                return true;
        }
    }
    return false;
}

private bool ShouldThinkOnProject(ProjectSpace project)
{
    var taskSystem = ServiceLocator.Instance.GetService<IProjectManager>()?.GetTaskSystem(project.Id);
    if (taskSystem == null) return true; // 新项目无任务系统，需要初始化

    var tasks = taskSystem.GetAll();

    // 条件1：项目下没有任何任务（刚创建）
    if (tasks.Count == 0) return true;

    // 条件2：所有任务都已完成
    if (tasks.All(t => t.Status == TaskStatus.Completed)) return true;

    // 条件3：周期性兜底（每隔N分钟）
    // 通过 project.UpdatedAt 与当前时间比较
    if (DateTime.UtcNow - project.UpdatedAt > TimeSpan.FromMinutes(10))
    {
        var hasStuckTasks = tasks.Any(t =>
            t.Status == TaskStatus.Running &&
            DateTime.UtcNow - t.UpdatedAt > TimeSpan.FromMinutes(15));
        if (hasStuckTasks) return true;
    }

    return false;
}
```

## 四、ThinkOnProject 上下文构建

### ContextManager 中的方法

```csharp
// src/SiliconLife.Core/AI/ContextManager.cs

public AIResponse ThinkOnProject()
{
    // 找到需要思考的项目（与 HasProjectsWithoutTemplate 逻辑一致）
    var project = FindProjectNeedingAttention();
    if (project == null)
    {
        // 没有需要关注的项目，不消耗 token
        return AIResponse.Success("No project needs attention at this time.");
    }

    var systemPrompt = BuildProjectScenarioContext(project);
    // ... 调用 AI，传入 ToolScenarioFlag.Project 对应的工具集
}
```

### 上下文模板

```
你是项目"{project.Name}"的主理人。
项目目标：{project.Description}
团队成员：{列出分配的硅基人名称和角色}
当前任务状态：
{遍历项目任务列表，格式化输出每条任务的标题、状态、执行人}
项目群聊最近消息：
{最近N条群聊消息，来自 project.GroupChatSessionId}
广播记录：
{最近N条广播，来自 project.BroadcastChannelId}
请问：下一步该做什么？你可以创建任务、分配任务、在群里说话、或标记项目完成。
```

### 上下文构建细节

```csharp
// src/SiliconLife.Core/AI/ContextManager.cs

private string BuildProjectScenarioContext(ProjectSpace project)
{
    var sb = new StringBuilder();
    sb.AppendLine($"你是项目\"{project.Name}\"的主理人。");
    sb.AppendLine($"项目目标：{project.Description}");

    // 团队成员
    var beingManager = ServiceLocator.Instance.BeingManager;
    sb.AppendLine("团队成员：");
    foreach (var beingId in project.AssignedBeings)
    {
        var being = beingManager?.GetBeing(beingId);
        sb.AppendLine($"  - {being?.Name ?? beingId.ToString()}");
    }

    // 任务状态
    var taskSystem = ServiceLocator.Instance.GetService<IProjectManager>()?.GetTaskSystem(project.Id);
    sb.AppendLine("当前任务状态：");
    if (taskSystem != null)
    {
        foreach (var task in taskSystem.GetAll())
        {
            var executor = beingManager?.GetBeing(task.ExecutorGuid);
            sb.AppendLine($"  [{task.Status}] {task.Title} (执行人: {executor?.Name ?? "未分配"})");
        }
    }
    else
    {
        sb.AppendLine("  （暂无任务）");
    }

    // 群聊消息
    if (project.GroupChatSessionId.HasValue)
    {
        var chatSystem = ServiceLocator.Instance.ChatSystem;
        var messages = chatSystem?.GetRecentMessages(project.GroupChatSessionId.Value, count: 10);
        sb.AppendLine("项目群聊最近消息：");
        if (messages != null)
        {
            foreach (var msg in messages)
            {
                sb.AppendLine($"  {msg.SenderName}: {msg.Content}");
            }
        }
    }

    return sb.ToString();
}
```

## 五、ThinkOnProject 工具集

### ToolScenarioFlag 扩展

当前枚举（`src/SiliconLife.Core/Tools/ToolScenarioAttribute.cs:28-36`）：

```csharp
[Flags]
public enum ToolScenarioFlag
{
    None = 0,
    Chat = 1 << 0,
    Task = 1 << 1,
    Timer = 1 << 2,
    MemoryCompression = 1 << 3,
    All = Chat | Task | Timer | MemoryCompression,
}
```

修改为：

```csharp
[Flags]
public enum ToolScenarioFlag
{
    None = 0,
    Chat = 1 << 0,
    Task = 1 << 1,
    Timer = 1 << 2,
    MemoryCompression = 1 << 3,
    Project = 1 << 4,  // 新增：ThinkOnProject 场景
    All = Chat | Task | Timer | MemoryCompression | Project,
}
```

### 新增工具：ProjectWorkTool

独立的工具类，与 `ProjectTool`（项目管理）职责分离。`ProjectTool` 是 Chat/Task 场景下的项目管理工具（创建项目、分配人员等），`ProjectWorkTool` 是 ThinkOnProject 场景下的项目推进工具（创建任务、群聊发言等）。

```csharp
// src/SiliconLife.Common/Tools/ProjectWorkTool.cs

[SiliconManagerOnly]
[ToolScenario(ToolScenarioFlag.Project)]
public class ProjectWorkTool : ITool
{
    public string Name => "project_work";
    public string Description =>
        "Project work actions for the curator. " +
        "Actions: 'create-task', 'assign-task', 'chat', 'broadcast', 'complete', 'status'.";

    public Dictionary<string, object> GetParameterSchema()
    {
        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The action to perform",
                    ["enum"] = new[] { "create-task", "assign-task", "chat", "broadcast", "complete", "status" }
                },
                ["title"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Task title (for create-task)"
                },
                ["description"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Task description (for create-task)"
                },
                ["assignee_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Silicon being GUID to assign (for create-task, assign-task)"
                },
                ["priority"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Task priority, lower = higher priority (for create-task, default 100)"
                },
                ["task_id"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Task GUID (for assign-task)"
                },
                ["message"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Message content (for chat)"
                },
                ["content"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Broadcast content (for broadcast)"
                },
                ["summary"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Completion summary (for complete)"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        // 根据 action 分发到具体实现
        // 操作对象是当前 ThinkOnProject 关注的项目
    }
}
```

### 各 action 的行为定义

| Action | 参数 | 行为 | 前置条件 |
|--------|------|------|---------|
| `create-task` | title, description, assignee_id?, priority? | 通过 `ProjectTaskSystem.Create()` 在项目下创建任务 | 项目活跃 |
| `assign-task` | task_id, assignee_id | 修改 `TaskItem.AssigneeGuid` 和 `ExecutorGuid` | 任务存在且未完成 |
| `chat` | message | 向 `project.GroupChatSessionId` 发送消息 | 群聊存在 |
| `broadcast` | content | 向 `project.BroadcastChannelId` 发送广播 | 广播频道存在 |
| `complete` | summary | 设置 `project.Status = Completed`，归档项目 | 所有任务已完成 |
| `status` | (无) | 返回项目完整状态（任务列表、成员、群聊摘要） | 项目存在 |

### 与现有 ProjectTool 的关系

- `ProjectTool`（`src/SiliconLife.Common/Tools/ProjectTool.cs`）已有 `create`/`assign`/`remove`/`update` 等 action，作用域是 Chat/Task 场景
- `ProjectWorkTool` 专注 Project 场景，新增项目推进能力（创建任务、群聊、广播、完成）
- 两者不冲突：`ProjectTool` 管理项目生命周期，`ProjectWorkTool` 驱动项目执行

## 六、老师场景完整链路

以"老师让主理人做个背单词网站"为例：

```
1. 老师在聊天中跟主理人说"给我做个背单词网站"

2. 主理人在 ThinkOnChat 场景中识别出项目级意图
   - 当前 ProjectTool 已支持 Chat 场景（ToolScenario 标记了 Chat flag）
   - AI 调用 project(action=create, name="背单词网站", description="...")

3. IProjectManager.CreateProject() 执行
   - 自动创建群聊 Session（GroupChatSessionId）
   - 自动创建广播频道（BroadcastChannelId）
   - 无 workflow_template 参数 → WorkflowTemplateName 为空

4. 下一个 Tick，主理人进入 HasProjectsWithoutTemplate() 判断
   - 发现"背单词网站"项目无模板 + 无任务 → 满足触发条件
   - _activityRaw = BeingActivity.Project
   - 进入 ThinkOnProject

5. ThinkOnProject → AI 看到空项目
   - AI 调用 project_work(action=create-task, title="设计数据库结构", ...)
   - AI 调用 project_work(action=create-task, title="搭建前端框架", ...)
   - AI 调用 project_work(action=chat, message="大家开始干活了，先领任务")

6. 其他硅基人 Tick → TaskEnumerator 检测到 Pending 任务
   - 通过 ThinkOnTask 执行 → 完成

7. 主理人 Tick → HasProjectsWithoutTemplate()
   - ShouldThinkOnProject() 检测到所有任务完成
   - ThinkOnProject → AI 创建下一批任务 + 群聊通报进度

8. 循环 5-7 直到项目完成
   - AI 调用 project_work(action=complete, summary="背单词网站已完成")

9. 主理人在聊天中跟老师说"做完了"
```

## 七、与现有系统的关系

### 与工作流引擎的关系

- **互斥**：同一项目只能有一种编排方式，由 `ProjectSpace.WorkflowTemplateName` 是否为空决定
- **产出一致**：都往 `TaskCenter` 塞任务，硅基人通过同一套 `ThinkOnTask` 执行
- **切换需人工**：不允许自动切换编排方式。如果项目创建时无模板，后来想加模板，需要人类修改项目设置并手动创建工作流实例
- **互不干扰**：有模板的项目由 `WorkflowTickObject` 驱动，主理人的 `HasProjectsWithoutTemplate()` 直接跳过

### 与群聊系统的关系

- `ThinkOnProject` 是群聊的自然触发源——主理人在 `ThinkOnProject` 中决定是否在群里说话
- 修复了"群聊没有触发时机"的根本问题：没有 `ThinkOnProject`，群聊只是个空壳
- 主理人通过 `project_work(action=chat)` 发言，普通硅基人通过 `ThinkOnGroupChat` 响应

### 与广播系统的关系

- 主理人可以用 `project_work(action=broadcast)` 通知项目状态变更
- 广播仍遵循"下课铃"模型：接收方在 Tick 的广播处理阶段读取，不回复
- 广播适合单向通知（如"第一阶段完成"），群聊适合双向协调

### 与任务系统的关系

- `ThinkOnProject` 产出的任务和普通任务无区别，都存在 `TaskCenter` 中
- 任务通过 `ProjectTaskSystem.Create()` 创建，自动关联 `ProjectId`
- 硅基人通过现有 `ThinkOnTask` 逻辑执行，不需要任何修改
- 任务完成后主理人通过下一轮 `ThinkOnProject` 的 `ShouldThinkOnProject()` 感知

## 八、实现优先级

### alpha-0.2 最小实现

1. `ToolScenarioFlag` 加 `Project` flag
   - 文件：`src/SiliconLife.Core/Tools/ToolScenarioAttribute.cs`
   - 改动：枚举加一个值，`All` 更新

2. `ThinkOnProject` 触发条件（项目创建 + 任务完成）
   - 文件：`src/SiliconLife.Common/SiliconBeing/DefaultSiliconBeing.cs`
   - 改动：替换第 324-330 行注释，实现 `HasProjectsWithoutTemplate()` 和 `ShouldThinkOnProject()`

3. `ThinkOnProject` 上下文构建
   - 文件：`src/SiliconLife.Core/AI/ContextManager.cs`
   - 改动：实现 `ThinkOnProject()` 方法和 `BuildProjectScenarioContext()`

4. 项目工作工具 `ProjectWorkTool`
   - 文件：`src/SiliconLife.Common/Tools/ProjectWorkTool.cs`（新建）
   - 实现 `create-task`、`chat`、`status` 三个核心 action

5. 群聊发消息工具
   - 包含在 `ProjectWorkTool.chat` action 中
   - 依赖 `ChatSystem` 向 `GroupChatSessionId` 发送消息

6. `CreateProject` 时有模板自动创建工作流实例（薄修复）
   - 文件：`src/SiliconLife.Core/Project/ProjectManager.cs`（或 `IProjectManager` 的实现）
   - 改动：在 `CreateProject()` 中，如果 `workflowTemplateName` 非空，自动创建 `WorkflowInstance`

### 后续迭代

7. 周期性兜底触发（检测卡住的任务）
8. 群聊无响应触发（群聊有新消息但无人回应时主理人介入）
9. 项目完成判定逻辑（任务全部完成时自动提示主理人）
10. 主理人识别项目级意图（从 Chat 场景的对话中自动判断是否需要创建项目）
