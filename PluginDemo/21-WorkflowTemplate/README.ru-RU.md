# Демо шаблона Workflow — полный бизнес-процесс

Демонстрирует определение полного бизнес-процесса с помощью `WorkflowTemplate`, включая проектирование конечного автомата, асинхронные условия/действия, ролевой контроль доступа, обработку таймаутов и инъекцию переходов. В отличие от более простого демо [05-WorkflowPlugin](../05-WorkflowPlugin/) (показывающего только реализацию интерфейса `IWorkflowPlugin`), этот пример создаёт полный workflow "PluginOnboarding" с реальной бизнес-логикой.

## Паттерн конечного автомата WorkflowTemplate

`WorkflowTemplate` определяет декларативный конечный автомат со свойствами: `Name`, `States`, `TerminalStates`, `Transitions`, `RoleDefinitions` и `Metadata`. `States[0]` автоматически является `InitialState`.

## Диаграмма потока состояний

```
Submitted ──BeginScreening──▶ Screening ──PassScreening──▶ Testing ──ApprovePlugin──▶ Approved (терминальное)
     │                            │                           │
     │                            └──FailScreening──▶ Rejected (терминальное)
     │                                                        ▲
     └─── (таймаут: 2 дня) ──▶ Blocked                       │
                                                   FailTesting─┘
```

## Асинхронный механизм Transition

Каждый `Transition` имеет два асинхронных делегата: `Condition` (возвращает bool, вызывается каждый Tick/60с) и `Action` (побочные эффекты после смены состояния). Максимум один переход за Tick на экземпляр, оцениваемый в порядке `Priority`.

## RoleDefinition — ограничения ролей

Определения ролей декларируют необходимые ресурсы: `Screener` (min=1, max=3) и `Tester` (min=1, max=0/неограниченно). Методы: `IsSatisfied()`, `GetStaffingStatus()`, `GetStatusText()`, `ValidateRoleAssignments()`.

## RequiredRoles — проверка во время выполнения и broadcast

Переходы могут декларировать `RequiredRoles`. Движок проверяет удовлетворённость ролей перед оценкой `Condition`. Неудовлетворённые роли блокируют переход и инициируют broadcast-уведомление (ограничение: 1 раз/24ч на переход).

## TimeoutDays — обнаружение застоя

Если `(now - LastProgressAt).TotalDays > TimeoutDays`, экземпляр помечается как `"Blocked"`, создаётся запись `WorkflowLog` и уведомляется куратор being.

## WorkflowTickObject и интеграция с MainLoop

`WorkflowTickObject` — подкласс `TickObject`, управляет движком workflow каждые 60 секунд с приоритетом 90 (ниже beings=100, выше фоновых задач).

## Отличие от 05-WorkflowPlugin

| Аспект | 05-WorkflowPlugin | 21-WorkflowTemplate (этот демо) |
|--------|-------------------|----------------------------------|
| Фокус | Паттерн реализации интерфейса | Полный дизайн бизнес-процесса |
| Состояния | 4 простых | 5 состояний, реалистичный поток |
| Роли | 1 роль | 2 роли с разными стратегиями MaxCount |
| RequiredRoles | 2 перехода | Межэтапный гейтинг |
| Бизнес-логика | Минимальная | Полный жизненный цикл онбординга |

## См. также

- [05-WorkflowPlugin](../05-WorkflowPlugin/) — более простое демо интерфейса IWorkflowPlugin
- [WorkflowTemplate](../../src/SiliconLife.Core/Workflow/WorkflowTemplate.cs) — определение шаблона
- [Transition](../../src/SiliconLife.Core/Workflow/Transition.cs) — правила переходов
- [RoleDefinition](../../src/SiliconLife.Core/Workflow/RoleDefinition.cs) — ограничения ролей
- [WorkflowEngine](../../src/SiliconLife.Core/Workflow/WorkflowEngine.cs) — ядро движка
- [WorkflowTickObject](../../src/SiliconLife.Core/Workflow/WorkflowTickObject.cs) — tick-драйвер
- [CodeReviewWorkflow](../../src/SiliconLife.Core/Workflow/CodeReviewWorkflow.cs) — встроенный workflow
