# Workflow Plugin Demo

Demonstrates implementing both `IPlugin` and `IWorkflowPlugin` in a single class. Shows how to register custom workflow templates and inject transitions into existing templates.

## IWorkflowPlugin Interface

`IWorkflowPlugin` is an optional interface that plugins can implement alongside `IPlugin` to contribute workflow definitions to the host's `WorkflowEngine`.

```csharp
public interface IWorkflowPlugin
{
    string PluginId { get; }
    List<WorkflowTemplate> RegisterTemplates();
    void InjectTransitions(WorkflowTemplate template);
}
```

| Member | Purpose |
|--------|---------|
| `PluginId` | Unique identifier — must match `IPlugin.Id` |
| `RegisterTemplates()` | Returns new workflow templates for the engine to register |
| `InjectTransitions(template)` | Injects additional transition rules into an existing template |

## How to Implement Both Interfaces

A single class implements both `IPlugin` and `IWorkflowPlugin`:

```
┌──────────────────────────────────────────────────────────────────────┐
│  WorkflowPluginDemo : IPlugin, IWorkflowPlugin                      │
│                                                                      │
│  IPlugin members:                                                    │
│    Id, GetName, Version, GetDescription, GetAuthor                  │
│    OnLoad(), OnStart(), OnStop(), OnUnload()                        │
│                                                                      │
│  IWorkflowPlugin members:                                            │
│    PluginId  →  returns Id (must match IPlugin.Id)                  │
│    RegisterTemplates()  →  creates PluginApproval workflow          │
│    InjectTransitions()  →  adds FastTrack to CodeReview workflow    │
└──────────────────────────────────────────────────────────────────────┘
```

### Key Points

1. **PluginId must match IPlugin.Id** — the host correlates the plugin with its workflow contributions using this identifier.

2. **RegisterTemplates() is called once during startup** — the host collects all templates from all `IWorkflowPlugin` implementations and registers them with `WorkflowEngine.RegisterTemplate()`.

3. **InjectTransitions() is called for every registered template** — the host iterates through all registered templates and passes each one to every `IWorkflowPlugin`, allowing cross-plugin extension.

4. **The call order matters** — `RegisterTemplates()` is called first for all plugins, then `InjectTransitions()` is called for each template. This ensures injected transitions can reference states defined by the template.

## Demo Workflow: PluginApproval

This demo creates a simple 4-state approval workflow:

```
Submitted ──StartReview──▶ Reviewing ──Approve──▶ Approved (terminal)
                             │
                             └──Reject──▶ Rejected (terminal)
```

| State | Description | Type |
|-------|-------------|------|
| `Submitted` | Initial state — plugin has been submitted | Normal |
| `Reviewing` | Under review by a Screener | Normal |
| `Approved` | Plugin approved | Terminal |
| `Rejected` | Plugin rejected | Terminal |

### Role Definitions

| Role | Min | Max | Description |
|------|-----|-----|-------------|
| Screener | 1 | 3 | Reviews plugin submissions |

### Transitions

| Name | From → To | Condition | Timeout | Required Roles |
|------|-----------|-----------|---------|---------------|
| StartReview | Submitted → Reviewing | Auto (Screener staffed) | 2 days | Screener |
| Approve | Reviewing → Approved | `ApprovedBy` in metadata | 5 days | Screener |
| Reject | Reviewing → Rejected | `RejectedBy` in metadata | — | — |

## InjectTransitions Demo: FastTrack

The plugin injects a "FastTrack" transition into the built-in `CodeReview` workflow:

```
Draft ──FastTrack──▶ Approved  (bypasses Reviewing state)
```

This demonstrates how one plugin can extend workflows defined by other plugins or by the host. The `FastTrack` transition is only enabled when the workflow instance metadata contains `TrustedAuthor = true`.

## Call Sequence

```
Host Startup
  │
  ├── Load DLL → PluginLoader scans & creates WorkflowPluginDemo
  │
  ├── OnLoad()
  │     └── Plugin prepares workflow definitions
  │
  ├── RegisterTemplates()  (IWorkflowPlugin)
  │     └── Returns PluginApproval template
  │     └── WorkflowEngine.RegisterTemplate("PluginApproval")
  │
  ├── InjectTransitions(CodeReview template)
  │     └── Adds FastTrack transition
  │
  ├── InjectTransitions(PluginApproval template)
  │     └── (no-op — this plugin doesn't extend its own template)
  │
  └── OnStart()
        └── Plugin is now fully operational
```

## This Demo

| Class | Role |
|-------|------|
| `WorkflowPluginDemo` | Implements both `IPlugin` and `IWorkflowPlugin` — registers PluginApproval workflow and injects FastTrack into CodeReview |

## See Also

- [IPlugin Interface](../../src/SiliconLife.Core/Plugins/IPlugin.cs) — base plugin interface
- [IWorkflowPlugin Interface](../../src/SiliconLife.Core/Workflow/IWorkflowPlugin.cs) — workflow plugin interface
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — template definition
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — state transition rules
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — built-in workflow example
