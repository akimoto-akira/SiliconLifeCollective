# 工作流插件示例

演示在同一個類別中同時實作 `IPlugin` 和 `IWorkflowPlugin`。展示如何註冊自訂工作流範本和向已有範本注入轉移規則。

## IWorkflowPlugin 介面

`IWorkflowPlugin` 是插件可選實作的介面，與 `IPlugin` 一起實作，向宿主的 `WorkflowEngine` 貢獻工作流定義。

```csharp
public interface IWorkflowPlugin
{
    string PluginId { get; }
    List<WorkflowTemplate> RegisterTemplates();
    void InjectTransitions(WorkflowTemplate template);
}
```

| 成員 | 用途 |
|------|------|
| `PluginId` | 唯一識別符 — 必須與 `IPlugin.Id` 匹配 |
| `RegisterTemplates()` | 傳回新的工作流範本供引擎註冊 |
| `InjectTransitions(template)` | 向已有範本注入額外的轉移規則 |

## 如何同時實作兩個介面

一個類別同時實作 `IPlugin` 和 `IWorkflowPlugin`：

```
┌──────────────────────────────────────────────────────────────────────┐
│  WorkflowPluginDemo : IPlugin, IWorkflowPlugin                      │
│                                                                      │
│  IPlugin 成員:                                                       │
│    Id, GetName, Version, GetDescription, GetAuthor                  │
│    OnLoad(), OnStart(), OnStop(), OnUnload()                        │
│                                                                      │
│  IWorkflowPlugin 成員:                                               │
│    PluginId  →  傳回 Id（必須與 IPlugin.Id 匹配）                   │
│    RegisterTemplates()  →  建立 PluginApproval 工作流               │
│    InjectTransitions()  →  向 CodeReview 工作流新增 FastTrack       │
└──────────────────────────────────────────────────────────────────────┘
```

### 關鍵要點

1. **PluginId 必須與 IPlugin.Id 匹配** — 宿主使用此識別符關聯插件與其工作流貢獻。

2. **RegisterTemplates() 在啟動時呼叫一次** — 宿主收集所有 `IWorkflowPlugin` 實作的範本，透過 `WorkflowEngine.RegisterTemplate()` 註冊。

3. **InjectTransitions() 對每個已註冊範本呼叫** — 宿主遍歷所有已註冊範本，將每個傳給每個 `IWorkflowPlugin`，允許跨插件擴展。

4. **呼叫順序很重要** — 先為所有插件呼叫 `RegisterTemplates()`，再對每個範本呼叫 `InjectTransitions()`。這確保注入的轉移可以引用範本定義的狀態。

## 示例工作流：PluginApproval

本示例建立一個簡單的 4 狀態審批工作流：

```
Submitted ──StartReview──▶ Reviewing ──Approve──▶ Approved (終止狀態)
                             │
                             └──Reject──▶ Rejected (終止狀態)
```

| 狀態 | 描述 | 類型 |
|------|------|------|
| `Submitted` | 初始狀態 — 插件已提交 | 普通 |
| `Reviewing` | Screener 正在審核 | 普通 |
| `Approved` | 插件已核准 | 終止 |
| `Rejected` | 插件已拒絕 | 終止 |

### 角色定義

| 角色 | 最少 | 最多 | 描述 |
|------|------|------|------|
| Screener | 1 | 3 | 審核插件提交 |

### 轉移規則

| 名稱 | 從 → 到 | 條件 | 逾時 | 所需角色 |
|------|---------|------|------|---------|
| StartReview | Submitted → Reviewing | 自動（Screener 已配備） | 2 天 | Screener |
| Approve | Reviewing → Approved | 元資料中有 `ApprovedBy` | 5 天 | Screener |
| Reject | Reviewing → Rejected | 元資料中有 `RejectedBy` | — | — |

## InjectTransitions 示例：FastTrack

插件向內建的 `CodeReview` 工作流注入 "FastTrack" 轉移：

```
Draft ──FastTrack──▶ Approved  (跳過 Reviewing 狀態)
```

這示範了一個插件如何擴展其他插件或宿主定義的工作流。`FastTrack` 轉移僅在工作流實例元資料包含 `TrustedAuthor = true` 時啟用。

## 呼叫順序

```
宿主啟動
  │
  ├── 載入 DLL → PluginLoader 掃描並建立 WorkflowPluginDemo
  │
  ├── OnLoad()
  │     └── 插件準備工作流定義
  │
  ├── RegisterTemplates()  (IWorkflowPlugin)
  │     └── 傳回 PluginApproval 範本
  │     └── WorkflowEngine.RegisterTemplate("PluginApproval")
  │
  ├── InjectTransitions(CodeReview 範本)
  │     └── 新增 FastTrack 轉移
  │
  ├── InjectTransitions(PluginApproval 範本)
  │     └── (無操作 — 本插件不擴展自己的範本)
  │
  └── OnStart()
        └── 插件完全就緒
```

## 本示例

| 類別 | 角色 |
|------|------|
| `WorkflowPluginDemo` | 同時實作 `IPlugin` 和 `IWorkflowPlugin` — 註冊 PluginApproval 工作流並向 CodeReview 注入 FastTrack |

## 相關參考

- [IPlugin 介面](../../src/SiliconLife.Core/Plugins/IPlugin.cs) — 基礎插件介面
- [IWorkflowPlugin 介面](../../src/SiliconLife.Core/Workflow/IWorkflowPlugin.cs) — 工作流插件介面
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — 範本定義
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — 狀態轉移規則
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — 內建工作流示例
