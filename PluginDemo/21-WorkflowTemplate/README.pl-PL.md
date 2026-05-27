# Demo szablonu Workflow — kompletny workflow biznesowy

Pokazuje, jak zdefiniować kompletny workflow biznesowy przy użyciu `WorkflowTemplate`, w tym projektowanie maszyny stanów, warunki/akcje asynchroniczne, kontrolę dostępu opartą na rolach, obsługę timeoutów i wstrzykiwanie przejść. W przeciwieństwie do prostszego demo [05-WorkflowPlugin](../05-WorkflowPlugin/) (które pokazuje jedynie implementację interfejsu `IWorkflowPlugin`), ten przykład buduje pełny workflow "PluginOnboarding" z realną logiką biznesową.

## Wzorzec maszyny stanów WorkflowTemplate

`WorkflowTemplate` definiuje deklaratywną maszynę stanów z właściwościami: `Name`, `States`, `TerminalStates`, `Transitions`, `RoleDefinitions` i `Metadata`. `States[0]` jest automatycznie `InitialState`.

## Diagram przepływu stanów

```
Submitted ──BeginScreening──▶ Screening ──PassScreening──▶ Testing ──ApprovePlugin──▶ Approved (terminalny)
     │                            │                           │
     │                            └──FailScreening──▶ Rejected (terminalny)
     │                                                        ▲
     └─── (timeout: 2 dni) ──▶ Blocked                       │
                                                   FailTesting─┘
```

## Mechanizm asynchroniczny Transition

Każdy `Transition` ma dwa delegaty asynchroniczne: `Condition` (zwraca bool, wywoływany co Tick/60s) i `Action` (efekty uboczne po zmianie stanu). Maksymalnie jedno przejście na Tick na instancję, oceniane w kolejności `Priority`.

## RoleDefinition — ograniczenia ról

Definicje ról deklarują wymagane zasoby: `Screener` (min=1, max=3) i `Tester` (min=1, max=0/nieograniczony). Metody: `IsSatisfied()`, `GetStaffingStatus()`, `GetStatusText()`, `ValidateRoleAssignments()`.

## RequiredRoles — sprawdzanie w runtime i broadcast

Przejścia mogą deklarować `RequiredRoles`. Engine sprawdza spełnienie ról przed ewaluacją `Condition`. Niespełnione role blokują przejście i wyzwalają powiadomienie broadcast (ograniczone: 1x/24h na przejście).

## TimeoutDays — wykrywanie zastoju

Jeśli `(now - LastProgressAt).TotalDays > TimeoutDays`, instancja jest oznaczana jako `"Blocked"`, tworzony jest rekord `WorkflowLog` i curator being jest powiadamiany.

## WorkflowTickObject i integracja z MainLoop

`WorkflowTickObject` jest podklasą `TickObject` — napędza workflow engine co 60 sekund z priorytetem 90 (niższy niż beings=100, wyższy niż zadania w tle).

## Różnica wobec 05-WorkflowPlugin

| Aspekt | 05-WorkflowPlugin | 21-WorkflowTemplate (ten demo) |
|--------|-------------------|----------------------------------|
| Focus | Wzorzec implementacji interfejsu | Kompletny projekt workflow biznesowego |
| Stany | 4 proste | 5 stanów, realistyczny przepływ |
| Role | 1 rola | 2 role z różnymi strategiami MaxCount |
| RequiredRoles | 2 przejścia | Gating międzyetapowy |
| Logika biznesowa | Minimalna | Kompletny cykl życia onboardingu |

## Zobacz też

- [05-WorkflowPlugin](../05-WorkflowPlugin/) — prostsze demo interfejsu IWorkflowPlugin
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — definicja szablonu
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — reguły przejść
- [RoleDefinition](../../src/SiliconLife.Core/Workflow/RoleDefinition.cs) — ograniczenia ról
- [WorkflowEngine](../../src/SiliconLife.Core/Workflow/WorkflowEngine.cs) — rdzeń engine
- [WorkflowTickObject](../../src/SiliconLife.Core/Workflow/WorkflowTickObject.cs) — driver tick
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — wbudowany workflow
