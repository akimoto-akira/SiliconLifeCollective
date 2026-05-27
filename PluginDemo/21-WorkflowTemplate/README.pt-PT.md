# Demo de Template de Workflow — Workflow de Negócio Completo

Demonstra como definir um workflow de negócio completo usando `WorkflowTemplate`, incluindo design de máquina de estados, condições/ações assíncronas, controlo de acesso baseado em funções, tratamento de timeouts e injeção de transições. Ao contrário do demo mais simples [05-WorkflowPlugin](../05-WorkflowPlugin/) (que apenas mostra a implementação da interface `IWorkflowPlugin`), este exemplo constrói um workflow completo "PluginOnboarding" com lógica de negócio real.

## Padrão de Máquina de Estados WorkflowTemplate

`WorkflowTemplate` define uma máquina de estados declarativa com propriedades: `Name`, `States`, `TerminalStates`, `Transitions`, `RoleDefinitions` e `Metadata`. `States[0]` é automaticamente o `InitialState`.

## Diagrama de Fluxo de Estados

```
Submitted ──BeginScreening──▶ Screening ──PassScreening──▶ Testing ──ApprovePlugin──▶ Approved (terminal)
     │                            │                           │
     │                            └──FailScreening──▶ Rejected (terminal)
     │                                                        ▲
     └─── (timeout: 2 dias) ──▶ Blocked                      │
                                                   FailTesting─┘
```

## Mecanismo Assíncrono de Transition

Cada `Transition` tem dois delegados assíncronos: `Condition` (retorna bool, chamado a cada Tick/60s) e `Action` (efeitos secundários após mudança de estado). Máximo uma transição por Tick por instância, avaliada por ordem de `Priority`.

## RoleDefinition — Restrições de Funções

As definições de funções declaram os recursos necessários: `Screener` (min=1, max=3) e `Tester` (min=1, max=0/ilimitado). Métodos: `IsSatisfied()`, `GetStaffingStatus()`, `GetStatusText()`, `ValidateRoleAssignments()`.

## RequiredRoles — Verificação em Runtime e Broadcast

As transições podem declarar `RequiredRoles`. O engine verifica o cumprimento das funções antes de avaliar `Condition`. Funções insatisfeitas bloqueiam a transição e disparam uma notificação broadcast (limitada: 1x/24h por transição).

## TimeoutDays — Deteção de Estado Estagnado

Se `(now - LastProgressAt).TotalDays > TimeoutDays`, a instância é marcada como `"Blocked"`, é criado um registo `WorkflowLog` e o curator being é notificado.

## WorkflowTickObject e Integração com MainLoop

`WorkflowTickObject` é uma subclasse de `TickObject` — aciona o workflow engine a cada 60 segundos com prioridade 90 (inferior a beings=100, superior a tarefas de fundo).

## Diferença em relação ao 05-WorkflowPlugin

| Aspeto | 05-WorkflowPlugin | 21-WorkflowTemplate (este demo) |
|--------|-------------------|----------------------------------|
| Foco | Padrão de implementação de interface | Design completo de workflow de negócio |
| Estados | 4 simples | 5 estados, fluxo realista |
| Funções | 1 função | 2 funções com diferentes estratégias MaxCount |
| RequiredRoles | 2 transições | Gating entre etapas |
| Lógica de negócio | Mínima | Ciclo de vida completo de onboarding |

## Ver também

- [05-WorkflowPlugin](../05-WorkflowPlugin/) — demo mais simples da interface IWorkflowPlugin
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — definição de template
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — regras de transição
- [RoleDefinition](../../src/SiliconLife.Core/Workflow/RoleDefinition.cs) — restrições de funções
- [WorkflowEngine](../../src/SiliconLife.Core/Workflow/WorkflowEngine.cs) — núcleo do engine
- [WorkflowTickObject](../../src/SiliconLife.Core/Workflow/WorkflowTickObject.cs) — driver de tick
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — workflow integrado
