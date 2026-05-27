# Demo de Plantilla de Workflow — Workflow de Negocio Completo

Demuestra cómo definir un workflow de negocio completo usando `WorkflowTemplate`, incluyendo diseño de máquina de estados, condiciones/acciones asíncronas, control de acceso basado en roles, manejo de timeouts e inyección de transiciones. A diferencia del demo más simple [05-WorkflowPlugin](../05-WorkflowPlugin/) (que solo muestra la implementación de la interfaz `IWorkflowPlugin`), este ejemplo construye un workflow completo "PluginOnboarding" con lógica de negocio real.

## Patrón de Máquina de Estados WorkflowTemplate

`WorkflowTemplate` define una máquina de estados declarativa con propiedades: `Name`, `States`, `TerminalStates`, `Transitions`, `RoleDefinitions` y `Metadata`. `States[0]` es automáticamente el `InitialState`.

## Diagrama de Flujo de Estados

```
Submitted ──BeginScreening──▶ Screening ──PassScreening──▶ Testing ──ApprovePlugin──▶ Approved (terminal)
     │                            │                           │
     │                            └──FailScreening──▶ Rejected (terminal)
     │                                                        ▲
     └─── (timeout: 2 días) ──▶ Blocked                      │
                                                   FailTesting─┘
```

## Mecanismo Asíncrono de Transition

Cada `Transition` tiene dos delegados asíncronos: `Condition` (retorna bool, llamado cada Tick/60s) y `Action` (efectos secundarios después del cambio de estado). Se ejecuta máximo una transición por Tick por instancia, evaluada en orden de `Priority`.

## RoleDefinition — Restricciones de Roles

Las definiciones de roles declaran los recursos necesarios: `Screener` (min=1, max=3) y `Tester` (min=1, max=0/ilimitado). Métodos: `IsSatisfied()`, `GetStaffingStatus()`, `GetStatusText()`, `ValidateRoleAssignments()`.

## RequiredRoles — Verificación en Tiempo de Ejecución y Broadcast

Las transiciones pueden declarar `RequiredRoles`. El engine verifica el cumplimiento de roles antes de evaluar `Condition`. Los roles insatisfechos bloquean la transición y disparan una notificación broadcast (limitada: 1x/24h por transición).

## TimeoutDays — Detección de Estado Estancado

Si `(now - LastProgressAt).TotalDays > TimeoutDays`, la instancia se marca como `"Blocked"`, se crea un registro `WorkflowLog` y se notifica al curator being.

## WorkflowTickObject e Integración con MainLoop

`WorkflowTickObject` es una subclase de `TickObject` — impulsa el workflow engine cada 60 segundos con prioridad 90 (menor que beings=100, mayor que tareas de fondo).

## Diferencia con 05-WorkflowPlugin

| Aspecto | 05-WorkflowPlugin | 21-WorkflowTemplate (este demo) |
|---------|-------------------|----------------------------------|
| Enfoque | Patrón de implementación de interfaz | Diseño completo de workflow de negocio |
| Estados | 4 simples | 5 estados, flujo realista |
| Roles | 1 rol | 2 roles con diferentes estrategias MaxCount |
| RequiredRoles | 2 transiciones | Gating entre etapas |
| Lógica de negocio | Mínima | Ciclo de vida completo de onboarding |

## Ver también

- [05-WorkflowPlugin](../05-WorkflowPlugin/) — demo más simple de interfaz IWorkflowPlugin
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — definición de plantilla
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — reglas de transición
- [RoleDefinition](../../src/SiliconLife.Core/Workflow/RoleDefinition.cs) — restricciones de roles
- [WorkflowEngine](../../src/SiliconLife.Core/Workflow/WorkflowEngine.cs) — núcleo del engine
- [WorkflowTickObject](../../src/SiliconLife.Core/Workflow/WorkflowTickObject.cs) — driver de tick
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — workflow integrado
