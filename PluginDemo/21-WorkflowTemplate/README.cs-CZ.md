# Demo šablony workflow — kompletní obchodní workflow

Ukazuje, jak definovat kompletní obchodní workflow pomocí `WorkflowTemplate`, včetně návrhu stavového automatu, asynchronních podmínek/akcí, řízení přístupu na základě rolí, zpracování timeoutů a injekce přechodů. Na rozdíl od jednoduššího dema [05-WorkflowPlugin](../05-WorkflowPlugin/) (které pouze ukazuje implementaci rozhraní `IWorkflowPlugin`) tento příklad vytváří kompletní workflow "PluginOnboarding" s reálnou obchodní logikou.

## Vzor stavového automatu WorkflowTemplate

`WorkflowTemplate` definuje deklarativní stavový automat s vlastnostmi: `Name`, `States`, `TerminalStates`, `Transitions`, `RoleDefinitions` a `Metadata`. `States[0]` je automaticky `InitialState`.

## Diagram toku stavů

```
Submitted ──BeginScreening──▶ Screening ──PassScreening──▶ Testing ──ApprovePlugin──▶ Approved (terminální)
     │                            │                           │
     │                            └──FailScreening──▶ Rejected (terminální)
     │                                                        ▲
     └─── (timeout: 2 dny) ──▶ Blocked                       │
                                                   FailTesting─┘
```

## Asynchronní mechanismus Transition

Každý `Transition` má dva asynchronní delegáty: `Condition` (vrací bool, voláno každý Tick/60s) a `Action` (vedlejší efekty po změně stavu). Na instanci se provede maximálně jeden přechod za Tick, vyhodnocovaný v pořadí `Priority`.

## RoleDefinition — omezení rolí

Definice rolí deklarují požadované zdroje: `Screener` (min=1, max=3) a `Tester` (min=1, max=0/neomezeně). Metody: `IsSatisfied()`, `GetStaffingStatus()`, `GetStatusText()`, `ValidateRoleAssignments()`.

## RequiredRoles — běhová kontrola a broadcast

Přechody mohou deklarovat `RequiredRoles`. Engine kontroluje splnění rolí před vyhodnocením `Condition`. Nesplněné role blokují přechod a spouštějí broadcast notifikaci (throttled: 1x/24h na přechod).

## TimeoutDays — detekce zastaralého stavu

Pokud `(now - LastProgressAt).TotalDays > TimeoutDays`, instance je označena jako `"Blocked"`, vytvoří se záznam `WorkflowLog` a curator being je notifikován.

## WorkflowTickObject a integrace s MainLoop

`WorkflowTickObject` je podtřída `TickObject` — řídí workflow engine každých 60 sekund s prioritou 90 (nižší než beings=100, vyšší než background tasky).

## Rozdíl oproti 05-WorkflowPlugin

| Aspekt | 05-WorkflowPlugin | 21-WorkflowTemplate (toto demo) |
|--------|-------------------|----------------------------------|
| Zaměření | Vzor implementace rozhraní | Kompletní návrh obchodního workflow |
| Stavy | 4 jednoduché | 5 stavů, realistický tok |
| Role | 1 role | 2 role s různými strategiemi MaxCount |
| RequiredRoles | 2 přechody | Cross-stage gating |
| Obchodní logika | Minimální | Kompletní životní cyklus onboardingu |

## Viz také

- [05-WorkflowPlugin](../05-WorkflowPlugin/) — jednodušší demo rozhraní IWorkflowPlugin
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — definice šablony
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — pravidla přechodů
- [RoleDefinition](../../src/SiliconLife.Core/Workflow/RoleDefinition.cs) — omezení rolí
- [WorkflowEngine](../../src/SiliconLife.Core/Workflow/WorkflowEngine.cs) — jádro engine
- [WorkflowTickObject](../../src/SiliconLife.Core/Workflow/WorkflowTickObject.cs) — tick driver
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — vestavěný workflow
