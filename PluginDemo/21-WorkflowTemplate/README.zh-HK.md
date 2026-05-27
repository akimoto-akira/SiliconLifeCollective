# 工作流模板示例 — 完整業務工作流

演示如何使用 `WorkflowTemplate` 定義完整的業務工作流，包括狀態機設計、異步條件/動作、基於角色的準入控制、超時處理和轉移注入。與較簡單的 [05-WorkflowPlugin](../05-WorkflowPlugin/) 示例（僅展示 `IWorkflowPlugin` 介面實現）不同，本示例構建了一個具有真實業務邏輯的完整 "PluginOnboarding" 工作流。

## WorkflowTemplate 狀態機設計模式

`WorkflowTemplate` 定義了一個聲明式狀態機：

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

| 屬性 | 用途 |
|------|------|
| `Name` | 唯一模板名稱（通過 `WorkflowEngine` 註冊） |
| `States` | 所有可能狀態；`States[0]` 即為 `InitialState` |
| `TerminalStates` | 到達這些狀態時工作流完成 |
| `Transitions` | 轉移規則列表（構造後添加） |
| `RoleDefinitions` | 工作流所需的角色要求 |
| `Metadata` | 插件擴展字段 |

## 狀態流轉圖

```
Submitted ──BeginScreening──▶ Screening ──PassScreening──▶ Testing ──ApprovePlugin──▶ Approved (終止)
     │                            │                           │
     │                            └──FailScreening──▶ Rejected (終止)
     │                                                        ▲
     └─── (超時: 2天) ──▶ Blocked                             │
                                                   FailTesting─┘
```

| 狀態 | 描述 | 類型 |
|------|------|------|
| `Submitted` | 初始 — 插件已提交審核 | 普通 |
| `Screening` | 代碼質量和 API 合規性審查中 | 普通 |
| `Testing` | 功能和安全測試中 | 普通 |
| `Approved` | 插件已批准上線 | 終止 |
| `Rejected` | 插件被拒絕（任一階段） | 終止 |

## Transition 異步機制

每個 `Transition` 包含兩個異步委託：

```csharp
template.Transitions.Add(new Transition
{
    TransitionName = "PassScreening",
    FromState = "Screening",
    ToState = "Testing",
    Condition = async (instance, serviceProvider) =>
    {
        // 異步條件：可查詢數據庫、調用 API、讀取文件
        var result = instance.Metadata["ScreeningResult"]?.ToString();
        return result == "Passed";
    },
    Action = async (instance, serviceProvider) =>
    {
        // 異步動作：狀態轉移後的副作用
        instance.StageOutputs["ScreeningCompleted"] = new { CompletedAt = DateTime.UtcNow };
        instance.MarkProgress();
    }
});
```

| 委託 | 調用時機 | 用途 |
|------|---------|------|
| `Condition` | 每次 Tick（60秒） | 返回 `true` 允許轉移 |
| `Action` | 狀態變更後 | 執行副作用（通知、創建任務、日誌） |

**關鍵**：每次 Tick 每個實例最多執行一個轉移。按 `Priority` 順序評估（值越小優先級越高）。

## RoleDefinition 角色約束與 ValidateRoleAssignments

角色定義聲明所需的人員/矽基人資源：

```csharp
RoleDefinitions = new Dictionary<string, RoleDefinition>
{
    ["Screener"] = new RoleDefinition
    {
        RoleName = "Screener",
        Description = "審核插件提交的代碼質量和合規性",
        MinCount = 1,  // 至少需要 1 個審核員
        MaxCount = 3   // 最多 3 個審核員
    },
    ["Tester"] = new RoleDefinition
    {
        RoleName = "Tester",
        Description = "執行功能和安全測試",
        MinCount = 1,  // 至少需要 1 個測試員
        MaxCount = 0   // 0 = 無限制
    }
};
```

| 方法 | 用途 |
|------|------|
| `IsSatisfied(count)` | `count` 在 `[MinCount, MaxCount]` 範圍內返回 `true` |
| `GetStaffingStatus(count)` | 返回 `Understaffed`/`Overstaffed`/`Full`/`Sufficient` |
| `GetStatusText(count)` | 本地化的人類可讀狀態文本 |
| `ValidateRoleAssignments(assignments, out unsatisfied)` | 模板級驗證，對照項目的角色池 |

## RequiredRoles 運行時檢查與廣播通知

轉移可以聲明 `RequiredRoles` 實現聲明式角色準入：

**運行時行為**（由 `WorkflowEngine` 處理）：
1. 在評估 `Condition` 前，引擎檢查 `RequiredRoles`
2. 對每個所需角色，檢查項目的 `RoleAssignments` 是否滿足 `RoleDefinition.IsSatisfied()`
3. 若任何角色不滿足 → 轉移被**阻塞**
4. 向項目的廣播頻道發送通知（限流：每個轉移每 24 小時一次）
5. 實例的 `Metadata["MissingRoles"]` 記錄哪些角色不足

## TimeoutDays 超時處理

**超時流程**：
1. `WorkflowEngine.TickAsync()` 檢查 `instance.LastProgressAt` 與 `TimeoutDays` 的差值
2. 若 `(now - LastProgressAt).TotalDays > TimeoutDays` → 實例 `Status` 設為 `"Blocked"`
3. 創建 `TransitionName = "Timeout"` 的 `WorkflowLog` 記錄
4. 通知管理者矽基人進行人工干預

## WorkflowTickObject 與 MainLoop 集成

`WorkflowTickObject` 是 `TickObject` 子類，驅動工作流引擎：

| 屬性 | 值 | 含義 |
|------|-----|------|
| 間隔 | 60秒 | 每分鐘調用 `TickAsync()` |
| 優先級 | 90 | 低於矽基人(100)，高於後台任務 |
| `autoRegister` | `true` | 自動加入 MainLoop 的 Tick 循環 |

## 與 05-WorkflowPlugin 示例的區別

| 方面 | 05-WorkflowPlugin | 21-WorkflowTemplate（本示例） |
|------|-------------------|-------------------------------|
| 重點 | 介面實現模式 | 完整業務工作流設計 |
| 狀態 | 4 個簡單狀態 | 5 個狀態，真實流程 |
| 條件 | 簡單（元數據鍵檢查） | 異步多字段驗證 |
| 角色 | 1 個角色，基礎用法 | 2 個角色，不同 MaxCount 策略 |
| RequiredRoles | 2 個轉移使用 | 跨階段準入（Testing→Approved 需要 Screener） |
| TimeoutDays | 簡單超時 | 多個階段不同超時 |
| InjectTransitions | 向 CodeReview 添加 FastTrack | 添加帶優先級元數據的 ExpediteReview |
| 業務邏輯 | 最小化 | 完整上架生命周期 |

## 本示例

| 類 | 角色 |
|----|------|
| `WorkflowTemplateDemo` | 實現 `IPlugin` + `IWorkflowPlugin` — 定義 PluginOnboarding 工作流並向 CodeReview 注入 ExpediteReview |

## 相關參考

- [05-WorkflowPlugin](../05-WorkflowPlugin/) — 更簡單的 IWorkflowPlugin 介面示例
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — 模板定義
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — 狀態轉移規則
- [RoleDefinition](../../src/SiliconLife.Core/Workflow/RoleDefinition.cs) — 角色約束
- [WorkflowEngine](../../src/SiliconLife.Core/Workflow/WorkflowEngine.cs) — 引擎核心
- [WorkflowInstance](../../src/SiliconLife.Core/Workflow/WorkflowInstance.cs) — 運行時實例
- [WorkflowTickObject](../../src/SiliconLife.Core/Workflow/WorkflowTickObject.cs) — Tick 驅動器
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — 內置工作流
