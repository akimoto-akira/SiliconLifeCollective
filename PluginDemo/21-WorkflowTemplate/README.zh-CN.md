# 工作流模板示例 — 完整业务工作流

演示如何使用 `WorkflowTemplate` 定义完整的业务工作流，包括状态机设计、异步条件/动作、基于角色的准入控制、超时处理和转移注入。与较简单的 [05-WorkflowPlugin](../05-WorkflowPlugin/) 示例（仅展示 `IWorkflowPlugin` 接口实现）不同，本示例构建了一个具有真实业务逻辑的完整 "PluginOnboarding" 工作流。

## WorkflowTemplate 状态机设计模式

`WorkflowTemplate` 定义了一个声明式状态机：

```csharp
var template = new WorkflowTemplate
{
    Name = "PluginOnboarding",
    States = new List<string> { "Submitted", "Screening", "Testing", "Approved", "Rejected" },
    TerminalStates = new List<string> { "Approved", "Rejected" },
    RoleDefinitions = new Dictionary<string, RoleDefinition> { ... },
    Metadata = new Dictionary<string, object> { ... }
};
```

| 属性 | 用途 |
|------|------|
| `Name` | 唯一模板名称（通过 `WorkflowEngine` 注册） |
| `States` | 所有可能状态；`States[0]` 即为 `InitialState` |
| `TerminalStates` | 到达这些状态时工作流完成 |
| `Transitions` | 转移规则列表（构造后添加） |
| `RoleDefinitions` | 工作流所需的角色要求 |
| `Metadata` | 插件扩展字段 |

## 状态流转图

```
Submitted ──BeginScreening──▶ Screening ──PassScreening──▶ Testing ──ApprovePlugin──▶ Approved (终止)
     │                            │                           │
     │                            └──FailScreening──▶ Rejected (终止)
     │                                                        ▲
     └─── (超时: 2天) ──▶ Blocked                             │
                                                   FailTesting─┘
```

| 状态 | 描述 | 类型 |
|------|------|------|
| `Submitted` | 初始 — 插件已提交审核 | 普通 |
| `Screening` | 代码质量和 API 合规性审查中 | 普通 |
| `Testing` | 功能和安全测试中 | 普通 |
| `Approved` | 插件已批准上线 | 终止 |
| `Rejected` | 插件被拒绝（任一阶段） | 终止 |

## Transition 异步机制

每个 `Transition` 包含两个异步委托：

```csharp
template.Transitions.Add(new Transition
{
    TransitionName = "PassScreening",
    FromState = "Screening",
    ToState = "Testing",
    Condition = async (instance, serviceProvider) =>
    {
        // 异步条件：可查询数据库、调用 API、读取文件
        var result = instance.Metadata["ScreeningResult"]?.ToString();
        return result == "Passed";
    },
    Action = async (instance, serviceProvider) =>
    {
        // 异步动作：状态转移后的副作用
        instance.StageOutputs["ScreeningCompleted"] = new { CompletedAt = DateTime.UtcNow };
        instance.MarkProgress();
    }
});
```

| 委托 | 调用时机 | 用途 |
|------|---------|------|
| `Condition` | 每次 Tick（60秒） | 返回 `true` 允许转移 |
| `Action` | 状态变更后 | 执行副作用（通知、创建任务、日志） |

**关键**：每次 Tick 每个实例最多执行一个转移。按 `Priority` 顺序评估（值越小优先级越高）。

## RoleDefinition 角色约束与 ValidateRoleAssignments

角色定义声明所需的人员/硅基人资源：

```csharp
RoleDefinitions = new Dictionary<string, RoleDefinition>
{
    ["Screener"] = new RoleDefinition
    {
        RoleName = "Screener",
        Description = "审核插件提交的代码质量和合规性",
        MinCount = 1,  // 至少需要 1 个审核员
        MaxCount = 3   // 最多 3 个审核员
    },
    ["Tester"] = new RoleDefinition
    {
        RoleName = "Tester",
        Description = "执行功能和安全测试",
        MinCount = 1,  // 至少需要 1 个测试员
        MaxCount = 0   // 0 = 无限制
    }
};
```

| 方法 | 用途 |
|------|------|
| `IsSatisfied(count)` | `count` 在 `[MinCount, MaxCount]` 范围内返回 `true` |
| `GetStaffingStatus(count)` | 返回 `Understaffed`/`Overstaffed`/`Full`/`Sufficient` |
| `GetStatusText(count)` | 本地化的人类可读状态文本 |
| `ValidateRoleAssignments(assignments, out unsatisfied)` | 模板级验证，对照项目的角色池 |

## RequiredRoles 运行时检查与广播通知

转移可以声明 `RequiredRoles` 实现声明式角色准入：

```csharp
new Transition
{
    TransitionName = "ApprovePlugin",
    FromState = "Testing",
    ToState = "Approved",
    RequiredRoles = new List<string> { "Screener" },  // Screener 角色必须满足
    ...
}
```

**运行时行为**（由 `WorkflowEngine` 处理）：
1. 在评估 `Condition` 前，引擎检查 `RequiredRoles`
2. 对每个所需角色，检查项目的 `RoleAssignments` 是否满足 `RoleDefinition.IsSatisfied()`
3. 若任何角色不满足 → 转移被**阻塞**
4. 向项目的广播频道发送通知（限流：每个转移每 24 小时一次）
5. 实例的 `Metadata["MissingRoles"]` 记录哪些角色不足

## TimeoutDays 超时处理

```csharp
new Transition
{
    TransitionName = "PassScreening",
    TimeoutDays = 3,  // 3 天无进展则标记为 Blocked
    ...
}
```

**超时流程**：
1. `WorkflowEngine.TickAsync()` 检查 `instance.LastProgressAt` 与 `TimeoutDays` 的差值
2. 若 `(now - LastProgressAt).TotalDays > TimeoutDays` → 实例 `Status` 设为 `"Blocked"`
3. 创建 `TransitionName = "Timeout"` 的 `WorkflowLog` 记录
4. 通知管理者硅基人进行人工干预

## WorkflowTickObject 与 MainLoop 集成

`WorkflowTickObject` 是 `TickObject` 子类，驱动工作流引擎：

```csharp
// 宿主启动时创建（自动注册到 MainLoop）
var tickObject = new WorkflowTickObject(workflowEngine);
// 默认间隔: 60秒, 优先级: 90
```

| 属性 | 值 | 含义 |
|------|-----|------|
| 间隔 | 60秒 | 每分钟调用 `TickAsync()` |
| 优先级 | 90 | 低于硅基人(100)，高于后台任务 |
| `autoRegister` | `true` | 自动加入 MainLoop 的 Tick 循环 |

每次 Tick：`WorkflowEngine.TickAsync()` 遍历所有 `InProgress` 实例，评估转移，每个实例最多执行一个转移。

## 与 05-WorkflowPlugin 示例的区别

| 方面 | 05-WorkflowPlugin | 21-WorkflowTemplate（本示例） |
|------|-------------------|-------------------------------|
| 重点 | 接口实现模式 | 完整业务工作流设计 |
| 状态 | 4 个简单状态 | 5 个状态，真实流程 |
| 条件 | 简单（元数据键检查） | 异步多字段验证 |
| 角色 | 1 个角色，基础用法 | 2 个角色，不同 MaxCount 策略 |
| RequiredRoles | 2 个转移使用 | 跨阶段准入（Testing→Approved 需要 Screener） |
| TimeoutDays | 简单超时 | 多个阶段不同超时 |
| InjectTransitions | 向 CodeReview 添加 FastTrack | 添加带优先级元数据的 ExpediteReview |
| 业务逻辑 | 最小化 | 完整上架生命周期 |

## 本示例

| 类 | 角色 |
|----|------|
| `WorkflowTemplateDemo` | 实现 `IPlugin` + `IWorkflowPlugin` — 定义 PluginOnboarding 工作流并向 CodeReview 注入 ExpediteReview |

## 相关参考

- [05-WorkflowPlugin](../05-WorkflowPlugin/) — 更简单的 IWorkflowPlugin 接口示例
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — 模板定义
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — 状态转移规则
- [RoleDefinition](../../src/SiliconLife.Core/Workflow/RoleDefinition.cs) — 角色约束
- [WorkflowEngine](../../src/SiliconLife.Core/Workflow/WorkflowEngine.cs) — 引擎核心
- [WorkflowInstance](../../src/SiliconLife.Core/Workflow/WorkflowInstance.cs) — 运行时实例
- [WorkflowTickObject](../../src/SiliconLife.Core/Workflow/WorkflowTickObject.cs) — Tick 驱动器
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — 内置工作流
