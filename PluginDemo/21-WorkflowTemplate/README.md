# Workflow Template Demo — Complete Business Workflow

Demonstrates how to define a complete business workflow using `WorkflowTemplate`, including state machine design, async conditions/actions, role-based gating, timeout handling, and transition injection. Unlike the simpler [05-WorkflowPlugin](../05-WorkflowPlugin/) demo (which only shows `IWorkflowPlugin` interface implementation), this example builds a full "PluginOnboarding" workflow with real business logic.

## WorkflowTemplate State Machine Pattern

A `WorkflowTemplate` defines a declarative state machine:

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

| Property | Purpose |
|----------|---------|
| `Name` | Unique template name (registered with `WorkflowEngine`) |
| `States` | All possible states; `States[0]` is the `InitialState` |
| `TerminalStates` | Workflow completes when reaching these states |
| `Transitions` | List of transition rules (added after construction) |
| `RoleDefinitions` | Role requirements for the workflow |
| `Metadata` | Extension fields for plugin use |

## State Flow Diagram

```
Submitted ──BeginScreening──▶ Screening ──PassScreening──▶ Testing ──ApprovePlugin──▶ Approved (terminal)
     │                            │                           │
     │                            └──FailScreening──▶ Rejected (terminal)
     │                                                        ▲
     └─── (timeout: 2 days) ──▶ Blocked                      │
                                                   FailTesting─┘
```

| State | Description | Type |
|-------|-------------|------|
| `Submitted` | Initial — plugin submitted for onboarding | Normal |
| `Screening` | Under code quality and API compliance review | Normal |
| `Testing` | Under functional and security testing | Normal |
| `Approved` | Plugin approved for production use | Terminal |
| `Rejected` | Plugin rejected (at either stage) | Terminal |

## Transition Async Mechanism

Each `Transition` has two async delegates:

```csharp
template.Transitions.Add(new Transition
{
    TransitionName = "PassScreening",
    FromState = "Screening",
    ToState = "Testing",
    Condition = async (instance, serviceProvider) =>
    {
        // Async condition: can query databases, check APIs, read files
        var result = instance.Metadata["ScreeningResult"]?.ToString();
        return result == "Passed";
    },
    Action = async (instance, serviceProvider) =>
    {
        // Async action: side effects after state transition
        instance.StageOutputs["ScreeningCompleted"] = new { CompletedAt = DateTime.UtcNow };
        instance.MarkProgress();
    }
});
```

| Delegate | When Called | Purpose |
|----------|------------|---------|
| `Condition` | Every Tick (60s) | Returns `true` to allow transition |
| `Action` | After state change | Executes side effects (notifications, task creation, logging) |

**Key**: Only one transition executes per Tick per instance. Transitions are evaluated in `Priority` order (lower = higher priority).

## RoleDefinition Constraints & ValidateRoleAssignments

Role definitions declare the human/being resources required:

```csharp
RoleDefinitions = new Dictionary<string, RoleDefinition>
{
    ["Screener"] = new RoleDefinition
    {
        RoleName = "Screener",
        Description = "Reviews plugin submissions for quality and compliance",
        MinCount = 1,  // At least 1 screener required
        MaxCount = 3   // No more than 3 screeners
    },
    ["Tester"] = new RoleDefinition
    {
        RoleName = "Tester",
        Description = "Performs functional and security testing",
        MinCount = 1,  // At least 1 tester required
        MaxCount = 0   // 0 = unlimited
    }
};
```

| Method | Purpose |
|--------|---------|
| `IsSatisfied(count)` | Returns `true` if `count` is within `[MinCount, MaxCount]` |
| `GetStaffingStatus(count)` | Returns `Understaffed`, `Overstaffed`, `Full`, or `Sufficient` |
| `GetStatusText(count)` | Localized human-readable status string |
| `ValidateRoleAssignments(assignments, out unsatisfied)` | Template-level validation against project's role pool |

## RequiredRoles Runtime Check & Broadcast

Transitions can declare `RequiredRoles` for declarative role-based gating:

```csharp
new Transition
{
    TransitionName = "ApprovePlugin",
    FromState = "Testing",
    ToState = "Approved",
    RequiredRoles = new List<string> { "Screener" },  // Screener role must be satisfied
    ...
}
```

**Runtime behavior** (handled by `WorkflowEngine`):
1. Before evaluating `Condition`, the engine checks `RequiredRoles`
2. For each required role, it checks the project's `RoleAssignments` against `RoleDefinition.IsSatisfied()`
3. If any role is unsatisfied → transition is **blocked**
4. A broadcast notification is sent to the project's channel (throttled: once per 24 hours per transition)
5. The instance's `Metadata["MissingRoles"]` records which roles are insufficient

## TimeoutDays & Stale State Detection

```csharp
new Transition
{
    TransitionName = "PassScreening",
    TimeoutDays = 3,  // If no progress in 3 days, mark instance as Blocked
    ...
}
```

**Timeout flow**:
1. `WorkflowEngine.TickAsync()` checks `instance.LastProgressAt` against `TimeoutDays`
2. If `(now - LastProgressAt).TotalDays > TimeoutDays` → instance `Status` is set to `"Blocked"`
3. A `WorkflowLog` entry with `TransitionName = "Timeout"` is created
4. The curator being is notified for manual intervention

## WorkflowTickObject & MainLoop Integration

`WorkflowTickObject` is a `TickObject` subclass that drives the workflow engine:

```csharp
// Created during host startup (automatic registration with MainLoop)
var tickObject = new WorkflowTickObject(workflowEngine);
// Default interval: 60 seconds, Priority: 90
```

| Property | Value | Meaning |
|----------|-------|---------|
| Interval | 60s | Calls `TickAsync()` every minute |
| Priority | 90 | Lower than beings (100), higher than background tasks |
| `autoRegister` | `true` | Automatically joins the MainLoop tick cycle |

Each tick: `WorkflowEngine.TickAsync()` iterates all `InProgress` instances, evaluates transitions, and executes at most one transition per instance.

## Difference from 05-WorkflowPlugin

| Aspect | 05-WorkflowPlugin | 21-WorkflowTemplate (this demo) |
|--------|-------------------|----------------------------------|
| Focus | Interface implementation pattern | Complete business workflow design |
| States | 4 simple states | 5 states with realistic flow |
| Conditions | Trivial (metadata key check) | Async with multi-field validation |
| Roles | 1 role, basic usage | 2 roles with different MaxCount strategies |
| RequiredRoles | Used on 2 transitions | Cross-stage gating (Testing→Approved needs Screener) |
| TimeoutDays | Simple timeout | Multiple timeouts at different stages |
| InjectTransitions | Adds FastTrack to CodeReview | Adds ExpediteReview with priority metadata |
| Business Logic | Minimal | Full onboarding lifecycle |

## This Demo

| Class | Role |
|-------|------|
| `WorkflowTemplateDemo` | Implements `IPlugin` + `IWorkflowPlugin` — defines PluginOnboarding workflow and injects ExpediteReview into CodeReview |

## See Also

- [05-WorkflowPlugin](../05-WorkflowPlugin/) — simpler IWorkflowPlugin interface demo
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — template definition
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — state transition rules
- [RoleDefinition](../../src/SiliconLife.Core/Workflow/RoleDefinition.cs) — role constraints
- [WorkflowEngine](../../src/SiliconLife.Core/Workflow/WorkflowEngine.cs) — engine core
- [WorkflowInstance](../../src/SiliconLife.Core/Workflow/WorkflowInstance.cs) — runtime instance
- [WorkflowTickObject](../../src/SiliconLife.Core/Workflow/WorkflowTickObject.cs) — tick driver
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — built-in workflow
