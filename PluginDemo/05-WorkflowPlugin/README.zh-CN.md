# 工作流插件示例

演示在同一个类中同时实现 `IPlugin` 和 `IWorkflowPlugin`。展示如何注册自定义工作流模板和向已有模板注入转移规则。

## IWorkflowPlugin 接口

`IWorkflowPlugin` 是插件可选实现的接口，与 `IPlugin` 一起实现，向宿主的 `WorkflowEngine` 贡献工作流定义。

```csharp
public interface IWorkflowPlugin
{
    string PluginId { get; }
    List<WorkflowTemplate> RegisterTemplates();
    void InjectTransitions(WorkflowTemplate template);
}
```

| 成员 | 用途 |
|------|------|
| `PluginId` | 唯一标识符 — 必须与 `IPlugin.Id` 匹配 |
| `RegisterTemplates()` | 返回新的工作流模板供引擎注册 |
| `InjectTransitions(template)` | 向已有模板注入额外的转移规则 |

## 如何同时实现两个接口

一个类同时实现 `IPlugin` 和 `IWorkflowPlugin`：

```
┌──────────────────────────────────────────────────────────────────────┐
│  WorkflowPluginDemo : IPlugin, IWorkflowPlugin                      │
│                                                                      │
│  IPlugin 成员:                                                       │
│    Id, GetName, Version, GetDescription, GetAuthor                  │
│    OnLoad(), OnStart(), OnStop(), OnUnload()                        │
│                                                                      │
│  IWorkflowPlugin 成员:                                               │
│    PluginId  →  返回 Id（必须与 IPlugin.Id 匹配）                   │
│    RegisterTemplates()  →  创建 PluginApproval 工作流               │
│    InjectTransitions()  →  向 CodeReview 工作流添加 FastTrack       │
└──────────────────────────────────────────────────────────────────────┘
```

### 关键要点

1. **PluginId 必须与 IPlugin.Id 匹配** — 宿主使用此标识符关联插件与其工作流贡献。

2. **RegisterTemplates() 在启动时调用一次** — 宿主收集所有 `IWorkflowPlugin` 实现的模板，通过 `WorkflowEngine.RegisterTemplate()` 注册。

3. **InjectTransitions() 对每个已注册模板调用** — 宿主遍历所有已注册模板，将每个传给每个 `IWorkflowPlugin`，允许跨插件扩展。

4. **调用顺序很重要** — 先为所有插件调用 `RegisterTemplates()`，再对每个模板调用 `InjectTransitions()`。这确保注入的转移可以引用模板定义的状态。

## 示例工作流：PluginApproval

本示例创建一个简单的 4 状态审批工作流：

```
Submitted ──StartReview──▶ Reviewing ──Approve──▶ Approved (终止状态)
                             │
                             └──Reject──▶ Rejected (终止状态)
```

| 状态 | 描述 | 类型 |
|------|------|------|
| `Submitted` | 初始状态 — 插件已提交 | 普通 |
| `Reviewing` | Screener 正在审核 | 普通 |
| `Approved` | 插件已批准 | 终止 |
| `Rejected` | 插件已拒绝 | 终止 |

### 角色定义

| 角色 | 最少 | 最多 | 描述 |
|------|------|------|------|
| Screener | 1 | 3 | 审核插件提交 |

### 转移规则

| 名称 | 从 → 到 | 条件 | 超时 | 所需角色 |
|------|---------|------|------|---------|
| StartReview | Submitted → Reviewing | 自动（Screener 已配备） | 2 天 | Screener |
| Approve | Reviewing → Approved | 元数据中有 `ApprovedBy` | 5 天 | Screener |
| Reject | Reviewing → Rejected | 元数据中有 `RejectedBy` | — | — |

## InjectTransitions 示例：FastTrack

插件向内置的 `CodeReview` 工作流注入 "FastTrack" 转移：

```
Draft ──FastTrack──▶ Approved  (跳过 Reviewing 状态)
```

这演示了一个插件如何扩展其他插件或宿主定义的工作流。`FastTrack` 转移仅在工作流实例元数据包含 `TrustedAuthor = true` 时启用。

## 调用顺序

```
宿主启动
  │
  ├── 加载 DLL → PluginLoader 扫描并创建 WorkflowPluginDemo
  │
  ├── OnLoad()
  │     └── 插件准备工作流定义
  │
  ├── RegisterTemplates()  (IWorkflowPlugin)
  │     └── 返回 PluginApproval 模板
  │     └── WorkflowEngine.RegisterTemplate("PluginApproval")
  │
  ├── InjectTransitions(CodeReview 模板)
  │     └── 添加 FastTrack 转移
  │
  ├── InjectTransitions(PluginApproval 模板)
  │     └── (无操作 — 本插件不扩展自己的模板)
  │
  └── OnStart()
        └── 插件完全就绪
```

## 本示例

| 类 | 角色 |
|----|------|
| `WorkflowPluginDemo` | 同时实现 `IPlugin` 和 `IWorkflowPlugin` — 注册 PluginApproval 工作流并向 CodeReview 注入 FastTrack |

## 相关参考

- [IPlugin 接口](../../src/SiliconLife.Core/Plugins/IPlugin.cs) — 基础插件接口
- [IWorkflowPlugin 接口](../../src/SiliconLife.Core/Workflow/IWorkflowPlugin.cs) — 工作流插件接口
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — 模板定义
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — 状态转移规则
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — 内置工作流示例
