# Démo de Template Workflow — Workflow Métier Complet

Montre comment définir un workflow métier complet avec `WorkflowTemplate`, incluant la conception de machine à états, les conditions/actions asynchrones, le contrôle d'accès basé sur les rôles, la gestion des timeouts et l'injection de transitions. Contrairement au démo plus simple [05-WorkflowPlugin](../05-WorkflowPlugin/) (qui ne montre que l'implémentation de l'interface `IWorkflowPlugin`), cet exemple construit un workflow complet "PluginOnboarding" avec une logique métier réelle.

## Pattern Machine à États WorkflowTemplate

`WorkflowTemplate` définit une machine à états déclarative avec les propriétés : `Name`, `States`, `TerminalStates`, `Transitions`, `RoleDefinitions` et `Metadata`. `States[0]` est automatiquement l'`InitialState`.

## Diagramme de Flux d'États

```
Submitted ──BeginScreening──▶ Screening ──PassScreening──▶ Testing ──ApprovePlugin──▶ Approved (terminal)
     │                            │                           │
     │                            └──FailScreening──▶ Rejected (terminal)
     │                                                        ▲
     └─── (timeout : 2 jours) ──▶ Blocked                    │
                                                   FailTesting─┘
```

## Mécanisme Asynchrone de Transition

Chaque `Transition` possède deux délégués asynchrones : `Condition` (retourne bool, appelé chaque Tick/60s) et `Action` (effets secondaires après changement d'état). Maximum une transition par Tick par instance, évaluée dans l'ordre de `Priority`.

## RoleDefinition — Contraintes de Rôles

Les définitions de rôles déclarent les ressources requises : `Screener` (min=1, max=3) et `Tester` (min=1, max=0/illimité). Méthodes : `IsSatisfied()`, `GetStaffingStatus()`, `GetStatusText()`, `ValidateRoleAssignments()`.

## RequiredRoles — Vérification à l'Exécution et Broadcast

Les transitions peuvent déclarer `RequiredRoles`. L'engine vérifie la satisfaction des rôles avant d'évaluer `Condition`. Les rôles insatisfaits bloquent la transition et déclenchent une notification broadcast (limitée : 1x/24h par transition).

## TimeoutDays — Détection d'État Stagnant

Si `(now - LastProgressAt).TotalDays > TimeoutDays`, l'instance est marquée comme `"Blocked"`, un enregistrement `WorkflowLog` est créé et le curator being est notifié.

## WorkflowTickObject et Intégration MainLoop

`WorkflowTickObject` est une sous-classe de `TickObject` — pilote le workflow engine toutes les 60 secondes avec une priorité de 90 (inférieure aux beings=100, supérieure aux tâches de fond).

## Différence avec 05-WorkflowPlugin

| Aspect | 05-WorkflowPlugin | 21-WorkflowTemplate (ce démo) |
|--------|-------------------|--------------------------------|
| Focus | Pattern d'implémentation d'interface | Conception complète de workflow métier |
| États | 4 simples | 5 états, flux réaliste |
| Rôles | 1 rôle | 2 rôles avec différentes stratégies MaxCount |
| RequiredRoles | 2 transitions | Gating inter-étapes |
| Logique métier | Minimale | Cycle de vie complet d'onboarding |

## Voir aussi

- [05-WorkflowPlugin](../05-WorkflowPlugin/) — démo plus simple de l'interface IWorkflowPlugin
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — définition de template
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — règles de transition
- [RoleDefinition](../../src/SiliconLife.Core/Workflow/RoleDefinition.cs) — contraintes de rôles
- [WorkflowEngine](../../src/SiliconLife.Core/Workflow/WorkflowEngine.cs) — cœur de l'engine
- [WorkflowTickObject](../../src/SiliconLife.Core/Workflow/WorkflowTickObject.cs) — driver de tick
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — workflow intégré
