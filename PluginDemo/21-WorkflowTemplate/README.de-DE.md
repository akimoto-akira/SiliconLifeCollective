# Workflow-Template-Demo — Vollständiger Geschäfts-Workflow

Zeigt, wie ein vollständiger Geschäfts-Workflow mit `WorkflowTemplate` definiert wird, einschließlich State-Machine-Design, asynchronen Bedingungen/Aktionen, rollenbasierter Zugangskontrolle, Timeout-Behandlung und Transition-Injection. Anders als das einfachere [05-WorkflowPlugin](../05-WorkflowPlugin/)-Demo (das nur die `IWorkflowPlugin`-Interface-Implementierung zeigt) erstellt dieses Beispiel einen vollständigen "PluginOnboarding"-Workflow mit realer Geschäftslogik.

## WorkflowTemplate State-Machine-Muster

`WorkflowTemplate` definiert eine deklarative State Machine mit Eigenschaften: `Name`, `States`, `TerminalStates`, `Transitions`, `RoleDefinitions` und `Metadata`. `States[0]` ist automatisch der `InitialState`.

## Zustandsflussdiagramm

```
Submitted ──BeginScreening──▶ Screening ──PassScreening──▶ Testing ──ApprovePlugin──▶ Approved (terminal)
     │                            │                           │
     │                            └──FailScreening──▶ Rejected (terminal)
     │                                                        ▲
     └─── (Timeout: 2 Tage) ──▶ Blocked                      │
                                                   FailTesting─┘
```

## Asynchroner Transition-Mechanismus

Jede `Transition` hat zwei asynchrone Delegates: `Condition` (gibt bool zurück, wird jeden Tick/60s aufgerufen) und `Action` (Seiteneffekte nach Zustandsänderung). Pro Tick wird maximal ein Übergang pro Instanz ausgeführt, ausgewertet in `Priority`-Reihenfolge.

## RoleDefinition — Rollenbeschränkungen

Rollendefinitionen deklarieren benötigte Ressourcen: `Screener` (min=1, max=3) und `Tester` (min=1, max=0/unbegrenzt). Methoden: `IsSatisfied()`, `GetStaffingStatus()`, `GetStatusText()`, `ValidateRoleAssignments()`.

## RequiredRoles — Laufzeitprüfung und Broadcast

Transitionen können `RequiredRoles` deklarieren. Die Engine prüft die Rollenerfüllung vor der Auswertung von `Condition`. Unerfüllte Rollen blockieren den Übergang und lösen eine Broadcast-Benachrichtigung aus (gedrosselt: 1x/24h pro Transition).

## TimeoutDays — Erkennung veralteter Zustände

Wenn `(now - LastProgressAt).TotalDays > TimeoutDays`, wird die Instanz als `"Blocked"` markiert, ein `WorkflowLog`-Eintrag erstellt und der Curator Being benachrichtigt.

## WorkflowTickObject und MainLoop-Integration

`WorkflowTickObject` ist eine `TickObject`-Unterklasse — treibt die Workflow-Engine alle 60 Sekunden mit Priorität 90 an (niedriger als Beings=100, höher als Hintergrundaufgaben).

## Unterschied zu 05-WorkflowPlugin

| Aspekt | 05-WorkflowPlugin | 21-WorkflowTemplate (dieses Demo) |
|--------|-------------------|-------------------------------------|
| Fokus | Interface-Implementierungsmuster | Vollständiges Geschäfts-Workflow-Design |
| Zustände | 4 einfache | 5 Zustände, realistischer Ablauf |
| Rollen | 1 Rolle | 2 Rollen mit verschiedenen MaxCount-Strategien |
| RequiredRoles | 2 Transitionen | Cross-Stage-Gating |
| Geschäftslogik | Minimal | Vollständiger Onboarding-Lebenszyklus |

## Siehe auch

- [05-WorkflowPlugin](../05-WorkflowPlugin/) — einfacheres IWorkflowPlugin-Interface-Demo
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — Template-Definition
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — Übergangsregeln
- [RoleDefinition](../../src/SiliconLife.Core/Workflow/RoleDefinition.cs) — Rollenbeschränkungen
- [WorkflowEngine](../../src/SiliconLife.Core/Workflow/WorkflowEngine.cs) — Engine-Kern
- [WorkflowTickObject](../../src/SiliconLife.Core/Workflow/WorkflowTickObject.cs) — Tick-Treiber
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — eingebauter Workflow
