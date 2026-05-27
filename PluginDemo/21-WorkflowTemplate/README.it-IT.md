# Demo Template Workflow — Workflow Aziendale Completo

Dimostra come definire un workflow aziendale completo usando `WorkflowTemplate`, incluso il design della macchina a stati, condizioni/azioni asincrone, controllo d'accesso basato sui ruoli, gestione dei timeout e iniezione di transizioni. A differenza del demo più semplice [05-WorkflowPlugin](../05-WorkflowPlugin/) (che mostra solo l'implementazione dell'interfaccia `IWorkflowPlugin`), questo esempio costruisce un workflow completo "PluginOnboarding" con logica aziendale reale.

## Pattern Macchina a Stati WorkflowTemplate

`WorkflowTemplate` definisce una macchina a stati dichiarativa con proprietà: `Name`, `States`, `TerminalStates`, `Transitions`, `RoleDefinitions` e `Metadata`. `States[0]` è automaticamente l'`InitialState`.

## Diagramma di Flusso degli Stati

```
Submitted ──BeginScreening──▶ Screening ──PassScreening──▶ Testing ──ApprovePlugin──▶ Approved (terminale)
     │                            │                           │
     │                            └──FailScreening──▶ Rejected (terminale)
     │                                                        ▲
     └─── (timeout: 2 giorni) ──▶ Blocked                    │
                                                   FailTesting─┘
```

## Meccanismo Asincrono di Transition

Ogni `Transition` ha due delegati asincroni: `Condition` (restituisce bool, chiamato ogni Tick/60s) e `Action` (effetti collaterali dopo il cambio di stato). Massimo una transizione per Tick per istanza, valutata nell'ordine di `Priority`.

## RoleDefinition — Vincoli dei Ruoli

Le definizioni dei ruoli dichiarano le risorse necessarie: `Screener` (min=1, max=3) e `Tester` (min=1, max=0/illimitato). Metodi: `IsSatisfied()`, `GetStaffingStatus()`, `GetStatusText()`, `ValidateRoleAssignments()`.

## RequiredRoles — Verifica Runtime e Broadcast

Le transizioni possono dichiarare `RequiredRoles`. L'engine verifica il soddisfacimento dei ruoli prima di valutare `Condition`. I ruoli insoddisfatti bloccano la transizione e attivano una notifica broadcast (limitata: 1x/24h per transizione).

## TimeoutDays — Rilevamento Stato Stagnante

Se `(now - LastProgressAt).TotalDays > TimeoutDays`, l'istanza viene marcata come `"Blocked"`, viene creato un record `WorkflowLog` e il curator being viene notificato.

## WorkflowTickObject e Integrazione MainLoop

`WorkflowTickObject` è una sottoclasse di `TickObject` — guida il workflow engine ogni 60 secondi con priorità 90 (inferiore a beings=100, superiore ai task di background).

## Differenza con 05-WorkflowPlugin

| Aspetto | 05-WorkflowPlugin | 21-WorkflowTemplate (questo demo) |
|---------|-------------------|-------------------------------------|
| Focus | Pattern di implementazione interfaccia | Design completo workflow aziendale |
| Stati | 4 semplici | 5 stati, flusso realistico |
| Ruoli | 1 ruolo | 2 ruoli con diverse strategie MaxCount |
| RequiredRoles | 2 transizioni | Gating tra fasi |
| Logica aziendale | Minimale | Ciclo di vita completo di onboarding |

## Vedi anche

- [05-WorkflowPlugin](../05-WorkflowPlugin/) — demo più semplice dell'interfaccia IWorkflowPlugin
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — definizione template
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — regole di transizione
- [RoleDefinition](../../src/SiliconLife.Core/Workflow/RoleDefinition.cs) — vincoli dei ruoli
- [WorkflowEngine](../../src/SiliconLife.Core/Workflow/WorkflowEngine.cs) — nucleo engine
- [WorkflowTickObject](../../src/SiliconLife.Core/Workflow/WorkflowTickObject.cs) — driver di tick
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — workflow integrato
